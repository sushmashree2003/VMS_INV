
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_ClosingStock_ReportController', INV_ClosingStock_ReportController);
    INV_ClosingStock_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter']
    function INV_ClosingStock_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {


        $scope.obj = {};
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));


        //==============================
        $scope.all_miid = function (ac) {
            $scope.get_ClosingStockdetails = [];
            $scope.allmi = ac;
            var toggleStatus = $scope.allmi;
            angular.forEach($scope.get_mi_list, function (itm) {
                itm.miid = toggleStatus;
            });
            if ($scope.allmi == true) {
                $scope.loaddata($scope.flag, $scope.get_mi_arraylist1);
            }
        };

        $scope.mi_click = function () {
            $scope.get_ClosingStockdetails = [];
            $scope.allmi = $scope.get_mi_list.every(function (itm) {
                return itm.miid;
            });
            $scope.get_mi_arraylist1 = [];
            angular.forEach($scope.get_mi_list, function (qq) {
                if (qq.miid == true) {
                    $scope.get_mi_arraylist1.push({ MI_Id: qq.MI_Id })
                }

            })
            $scope.flag = $scope.optionflag;
            $scope.loaddata($scope.flag, $scope.get_mi_arraylist1);

        };


        $scope.isRequiredmi = function () {

            return !$scope.get_mi_list.some(function (options) {
                return options.miid;
            });

        };

        //==========================================================


        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var data = {
                "optionflag": $scope.optionflag,
                "get_mi_arraylist": $scope.get_mi_arraylist1
            };
            apiService.create("INV_ClosingStock_Report/getloaddata", data).
                then(function (promise) {
                    $scope.staffcheck = promise.staffcheck;
                    if ($scope.staffcheck == 'Staff') {
                        $scope.MI_Name = promise.get_mi_list[0].MI_Name;
                        $scope.MI_Id = promise.get_mi_list[0].MI_Id;
                        angular.forEach($scope.get_mi_list, function (a) {
                            a.miid = true;
                        })
                        $scope.get_ClosingStockdetails = promise.get_ClosingStockdetails;

                    }
                    else {
                        $scope.get_ClosingStockdetails = promise.get_ClosingStockdetails;
                        if ($scope.get_mi_list == undefined) {
                            $scope.get_mi_list = promise.get_mi_list;
                            angular.forEach($scope.get_mi_list, function (qq) {
                                qq.miid = true;
                            })
                            $scope.allmi = true;
                        }
                    }

                });
        };

        $scope.onrdochange = function (optionflag) {
            $scope.get_mi_arraylist1 = [];
            angular.forEach($scope.get_mi_list, function (qq) {
                if (qq.miid == true) {
                    $scope.get_mi_arraylist1.push({ MI_Id: qq.MI_Id })
                }




            })
            $scope.get_ClosingStockdetails = "";
            $scope.get_Closingstockreport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag, $scope.get_mi_arraylist1);

        };

        //========================ITEM
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.get_ClosingStockdetails.every(function (itm) {
                return itm.itemck;
            });
        };
        $scope.togchkbxSP = function () {
            $scope.spall = $scope.get_ClosingStockdetails.every(function (itm) {
                return itm.spck;
            });
        };
        //$scope.togchkbxG = function () {
        //    $scope.grnall = $scope.get_Inwarddetails.every(function (itm) {
        //        return itm.grnck;
        //    });
        //};
        $scope.isOptionsRequiredI = function () {
            if ($scope.optionflag === 'Item-Wise') {
                return !$scope.get_ClosingStockdetails.some(function (options) {
                    return options.itemck;
                });
            } else {
                return false;
            }
        };
        //$scope.isOptionsRequiredG = function () {
        //    if ($scope.optionflag === 'GRN') {
        //        return !$scope.get_Inwarddetails.some(function (options) {
        //            return options.grnck;
        //        });
        //    } else {
        //        return false;
        //    }
        //};
        $scope.isOptionsRequiredSP = function () {
            if ($scope.optionflag === 'Supplier') {
                return !$scope.get_ClosingStockdetails.some(function (options) {
                    return options.spck;
                });
            } else {
                return false;
            }
        };
        //$scope.all_checkG = function (itmi) {
        //    $scope.grnall = itmi;
        //    var toggleStatus = $scope.grnall;
        //    angular.forEach($scope.get_Inwarddetails, function (itm) {
        //        itm.grnck = toggleStatus;
        //    });
        //};

        $scope.all_checkI = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.get_ClosingStockdetails, function (itm) {
                itm.itemck = toggleStatus;
            });
        };
        $scope.all_checkSP = function (itmi) {
            $scope.spall = itmi;
            var toggleStatus = $scope.spall;
            angular.forEach($scope.get_ClosingStockdetails, function (itm) {
                itm.spck = toggleStatus;
            });
        };

        //==================================PO Report
        $scope.submitted = false;
        $scope.onreport = function () {

            if ($scope.myForm.$valid) {
                var data = {};
                $scope.start_Date = $filter('date')($scope.startdate, "yyyy-MM-dd");
                $scope.end_Date = $filter('date')($scope.enddate, "yyyy-MM-dd");
                if ($scope.bw_dates === true) {
                    $scope.sDate = $scope.start_Date;
                    $scope.eDate = $scope.end_Date;
                }
                else {
                    $scope.sDate = "";
                    $scope.eDate = "";
                }
                if ($scope.accept == undefined) {
                    $scope.accept = 0;
                }
                if ($scope.reject == undefined) {
                    $scope.reject = 0;
                }
                if ($scope.pending == undefined) {
                    $scope.pending = 0;
                }

                $scope.grnoArray = [];
                $scope.ClosingStitemArray = [];
                $scope.ClosingsuplierArray = [];
                $scope.get_mi_arraylist = [];
                if ($scope.staffcheck == 'Staff') {
                    $scope.get_mi_arraylist.push({ MI_Id: $scope.MI_Id })
                }
                else {
                    angular.forEach($scope.get_mi_list, function (qq) {
                        if (qq.miid == true) {
                            $scope.get_mi_arraylist.push({ MI_Id: qq.MI_Id })
                        }

                    })
                }
                if ($scope.optionflag === "All") {
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,

                    };
                }


                else if ($scope.optionflag === "Item-Wise") {
                    angular.forEach($scope.get_ClosingStockdetails, function (itm) {
                        if (itm.itemck === true) {
                            $scope.ClosingStitemArray.push(itm);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "ClosingStitemArray": $scope.ClosingStitemArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,

                    };
                }
                else if ($scope.optionflag === "Supplier") {
                    angular.forEach($scope.get_ClosingStockdetails, function (sp) {
                        if (sp.spck === true) {
                            $scope.ClosingsuplierArray.push(sp);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "ClosingsuplierArray": $scope.ClosingsuplierArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,

                    };
                }
                apiService.create("INV_ClosingStock_Report/onreport", data).
                    then(function (promise) {


                        $('#grid123').empty();
                        $scope.get_Closingstockreport = true;

                        //IMS.INVMS_SupplierName
                        $scope.insarray = [
                            { name: "INVMS_SupplierName" },
                            { name: "INVMI_ItemCode" },
                            { name: "INVMI_ItemName" },
                            { name: "INVOB_Qty" },
                            { name: "INVOB_PurchaseRate" },
                            { name: "INVOB_TotalAmount" },
                            { name: "INVTGRN_Qty" },
                            { name: "INVTGRN_PurchaseRate" },
                            { name: "INVTGRN_Amount" },
                            { name: "INVTSL_SalesQty" },
                            { name: "INVTSL_SalesPrice" },
                            { name: "INVTSL_Amount" },
                            { name: "ClosingQty" },
                            { name: "ClosingRate" },
                            { name: "ClosingTotalAmount" }];

                        $scope.columns1 = [
                            { field: "INVMS_SupplierName", name: "INVMS_SupplierName", title: "Suplier Name" },
                            { field: "INVMI_ItemCode", name: "INVMI_ItemCode", title: "Item Code" },
                            { field: "INVMI_ItemName", name: "INVMI_ItemName", title: "Item Name" },
                            { field: "INVOB_Qty", name: "INVOB_Qty", title: "Quantity" },
                            { field: "INVOB_PurchaseRate", name: "INVOB_PurchaseRate", title: "Rate" },
                            { field: "INVOB_TotalAmount", name: "INVOB_TotalAmount", title: "Amount" },
                            { field: "INVTGRN_Qty", name: "INVTGRN_Qty", title: "Quantity" },
                            { field: "INVTGRN_PurchaseRate", name: "INVTGRN_PurchaseRate", title: "Rate" },
                            { field: "INVTGRN_Amount", name: "INVTGRN_Amount", title: "Amount" },
                            { field: "INVTSL_SalesQty", name: "INVTSL_SalesQty", title: "Qantity" },
                            { field: "INVTSL_SalesPrice", name: "INVTSL_SalesPrice", title: "Rate" },
                            { field: "INVTSL_Amount", name: "INVTSL_Amount", title: "Amount" },
                            { field: "ClosingQty", name: "ClosingQty", title: "Closing Qty" },
                            { field: "ClosingRate", name: "ClosingRate", title: "Rate" },
                            { field: "ClosingTotalAmount", name: "ClosingTotalAmount", title: "Amount" }


                        ];
                        $scope.newarray = [];
                        $scope.newarray1 = $scope.newarray;

                        $scope.data = promise.get_Closingstockreport;
                        debugger;
                        if (promise.get_Closingstockreport.length > 0) {
                            var totalrate = 0.00;
                            var totaltax = 0.00;
                            var totalamt = 0.00;
                            //$scope.get_Inwardreport = promise.get_Inwardreport;
                            //$scope.presentCountgrid = $scope.get_Inwardreport.length;

                            angular.forEach($scope.get_Closingstockreport, function (po) {
                            });

                            angular.forEach($scope.header_list, function (op) {
                                op.columns = [];
                                angular.forEach($scope.columns1, function (op123) {

                                    op.columns.push({ field: "hema" + op.ftI_Id + op123.field, name: "hema" + op.ftI_Id + op123.name, title: op123.title, width: 100 });

                                })
                                op.title = op.ftI_Name;
                            })


                            $scope.tempaggary = [];
                            $scope.colarrayall = [{

                                title: "SLNO",
                                template: "<span class='row-number'></span>", width: 75

                            },
                            {
                                name: "INVMS_SupplierName", field: "INVMS_SupplierName", title: "Suplier Name"
                            },
                            {
                                name: 'INVMI_ItemCode', field: 'INVMI_ItemCode', title: 'Item Code', width: 150
                            },

                            {
                                name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item Name', width: 150
                            },
                            {
                                title: "Open Balance",
                                columns: [
                                    {
                                        name: 'INVOB_Qty', field: 'INVOB_Qty', title: 'Quantity', width: 250
                                    },
                                    {
                                        name: 'INVOB_PurchaseRate', field: 'INVOB_PurchaseRate', title: 'Rate', width: 200, format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                    },
                                    {
                                        name: 'INVOB_TotalAmount', field: 'INVOB_TotalAmount', title: 'Amount', width: 200,
                                        format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"

                                    }
                                ]
                            },

                            {
                                title: "Inward Quantity",
                                columns: [
                                    {
                                        name: 'INVTGRN_Qty', field: 'INVTGRN_Qty', title: 'Quantity', width: 200
                                    },
                                    {
                                        name: 'INVTGRN_PurchaseRate', field: 'INVTGRN_PurchaseRate', title: 'Rate', width: 200, format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                    },

                                    {
                                        name: 'INVTGRN_Amount', field: 'INVTGRN_Amount', title: 'Amount', width: 100, format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                    }]


                            },

                            {
                                title: "Outward Quantity",
                                columns: [
                                    {
                                        name: 'INVTSL_SalesQty', field: 'INVTSL_SalesQty', title: 'Quantity', width: 200
                                    },
                                    {
                                        name: 'INVTSL_SalesPrice', field: 'INVTSL_SalesPrice', title: 'Rate', width: 150
                                    },
                                    {
                                        name: 'INVTSL_Amount', field: 'INVTSL_Amount', title: 'Amount', width: 100, format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                    },

                                ]
                            },

                            {
                                title: "Closing Balance Quantity",
                                columns: [
                                    {
                                        name: 'ClosingQty', field: 'ClosingQty', title: 'Quantity', width: 150
                                    },
                                    {
                                        name: 'ClosingRate', field: 'ClosingRate', title: 'Rate', width: 150
                                    },

                                    {
                                        name: 'INVTSL_Amount', field: 'ClosingTotalAmount', title: 'Amount', width: 150, format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                    }
                                ]
                            }




                            ];
                            $scope.tempaggary.push(
                                { field: 'ClosingTotalAmount', name: 'ClosingTotalAmount', format: "{0:n2}", aggregate: "sum" },
                                { field: 'INVTSL_Amount', name: 'INVTSL_Amount', format: "{0:n2}", aggregate: "sum" },
                                { field: 'INVTGRN_PurchaseRate', name: 'INVTGRN_PurchaseRate', format: "{0:n2}", aggregate: "sum" },
                                { field: 'INVOB_PurchaseRate', name: 'INVOB_PurchaseRate', format: "{0:n2}", aggregate: "sum" },
                                { field: 'INVTGRN_Amount', name: 'INVTGRN_Amount', format: "{0:n2}", aggregate: "sum" },
                                { field: 'INVOB_TotalAmount', name: 'INVOB_TotalAmount', format: "{0:n2}", aggregate: "sum" }
                                //INVOB_TotalAmount

                            );

                            ///function (e) { return e.userDetails.UserTypeId == 1 ? false : true; }
                            angular.forEach($scope.header_list, function (obj) {
                                $scope.colarrayall.push(obj);
                            })



                            console.log($scope.header_list);

                            console.log($scope.colarrayall);

                            $scope.txtdata = "TIME :" + " " + $filter('date')(new Date(), 'HH:mm:ss') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname;

                            console.log($scope.txtdata);
                            $scope.aaaa = [{
                                title: $scope.txtdata,
                                columns: $scope.colarrayall
                            }]
                            console.log($scope.data);
                            var gridall;


                            $(document).ready(function () {
                                initGridall();

                            });

                            function initGridall() {
                                gridall = $("#grid123").kendoGrid({
                                    toolbar: ["excel", "pdf"],
                                    excel: {
                                        fileName: "ClosingStockReport.xlsx",
                                        proxyURL: "",
                                        filterable: true,


                                    },


                                    pdf: {
                                        fileName: "ClosingStockReport.pdf"
                                    },

                                    dataSource: {

                                        //type: "odata",
                                        //transport: {
                                        //    read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Products"
                                        //},
                                        data: $scope.data,
                                        aggregate: $scope.tempaggary

                                    },
                                    sortable: true,
                                    pageable: false,
                                    groupable: false,
                                    filterable: true,
                                    columnMenu: true,
                                    reorderable: true,
                                    resizable: true,

                                    columns: $scope.aaaa,
                                    dataBound: function () {
                                        var rows = this.items();
                                        $(rows).each(function () {
                                            var index = $(this).index() + 1;
                                            var rowLabel = $(this).find(".row-number");
                                            $(rowLabel).html(index);
                                        });
                                    }
                                }).data("kendoGrid");
                                gridall.setOptions({
                                    sortable: true
                                });
                            }



                        }
                        else {
                            swal("No Record Found...!!");
                            $scope.get_Closingstockreport = "";
                        }
                    })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cancel = function () {
            $state.reload();
        };
        //======================================Print & Export to Excel
        //$scope.exportToExcel = function (export_id) {

        //    var exportHref = Excel.tableToExcel(export_id, 'printpr');
        //    $timeout(function () {
        //        location.href = exportHref;
        //    }, 100);
        //};
        $scope.exportToExcel = function (export_id) {

            var exportHref = Excel.tableToExcel(export_id, 'sheet name');
            $timeout(function () {
                location.href = exportHref;
            }, 100);

        };
        $scope.printData = function () {

            var innerContents = document.getElementById("printInward").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
    }
})();