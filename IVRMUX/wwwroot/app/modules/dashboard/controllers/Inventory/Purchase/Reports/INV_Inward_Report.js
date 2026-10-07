
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_Inward_ReportController', INV_Inward_ReportController);
    INV_Inward_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter']
    function INV_Inward_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {


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
            $scope.get_Inwarddetails = [];
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
            $scope.get_Inwarddetails = [];
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
            apiService.create("INV_Inward_Report/getloaddata", data).
                then(function (promise) {
                    $scope.staffcheck = promise.staffcheck;
                    if ($scope.staffcheck == 'Staff') {
                        $scope.MI_Name = promise.get_mi_list[0].MI_Name;
                        $scope.MI_Id = promise.get_mi_list[0].MI_Id;
                        angular.forEach($scope.get_mi_list, function (a) {
                            a.miid = true;
                        })
                        $scope.get_Inwarddetails = promise.get_Inwarddetails;

                    }
                    else {
                        $scope.get_Inwarddetails = promise.get_Inwarddetails;
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
            $scope.get_Inwarddetails = "";
            $scope.get_Inwardreport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag, $scope.get_mi_arraylist1);

        };

        //========================ITEM
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.get_Inwarddetails.every(function (itm) {
                return itm.itemck;
            });
        };
        $scope.togchkbxSP = function () {
            $scope.spall = $scope.get_Inwarddetails.every(function (itm) {
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
                return !$scope.get_Inwarddetails.some(function (options) {
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
                return !$scope.get_Inwarddetails.some(function (options) {
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
            angular.forEach($scope.get_Inwarddetails, function (itm) {
                itm.itemck = toggleStatus;
            });
        };
        $scope.all_checkSP = function (itmi) {
            $scope.spall = itmi;
            var toggleStatus = $scope.spall;
            angular.forEach($scope.get_Inwarddetails, function (itm) {
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
                $scope.InwarditemArray = [];
                $scope.InwardsuplierArray = [];
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
                    angular.forEach($scope.get_Inwarddetails, function (itm) {
                        if (itm.itemck === true) {
                            $scope.InwarditemArray.push(itm);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "InwarditemArray": $scope.InwarditemArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,

                    };
                }
                else if ($scope.optionflag === "Supplier") {
                    angular.forEach($scope.get_Inwarddetails, function (sp) {
                        if (sp.spck === true) {
                            $scope.InwardsuplierArray.push(sp);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "InwardsuplierArray": $scope.InwardsuplierArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,

                    };
                }
                apiService.create("INV_Inward_Report/onreport", data).
                    then(function (promise) {

                        var count = 0;

                        $scope.get_Inwardreport = true;
                        $scope.tempaggary = [];

                        $scope.insarray = [{ name: "INVMGRN_GRNNo" },
                        { name: "GRNDate" },
                        { name: "INVMI_ItemCode" },
                        { name: "INVMI_ItemName" },
                        { name: "INVMI_ItemDesc" },
                        { name: "INVMS_SupplierName" },
                        { name: "INVMGRN_InvoiceNo" },
                        { name: "INVMS_GSTNo" },
                        { name: "INVMI_HSNCode" },
                        { name: "INVMUOM_UOMName" },
                        { name: "INVMLO_LocationRoomName" },
                        { name: "INVTGRN_Qty" },
                        { name: "INVTGRN_PurchaseRate" },
                        { name: "INVTGRN_Amount" },
                        { name: "INVTGRN_TaxAmt" },
                        { name: "INVT_Otherchrges" },
                        { name: "INVMPO_ReferenceNo" },
                        { name: "INVSPT_PaymentReference" }];
                        $scope.columns1 = [{ field: "INVMGRN_GRNNo", name: "INVMGRN_GRNNo", title: "GRN No" },
                        { field: "GRNDate", name: "GRNDate", title: "GRN Date" },
                        { field: "INVMI_ItemCode", name: "INVMI_ItemCode", title: "Item Code" },
                        { field: "INVMI_ItemName", name: "INVMI_ItemName", title: "Item Name" },
                        { field: "INVMI_ItemDesc", name: "INVMI_ItemDesc", title: "Item Description" },
                        { field: "INVMS_SupplierName", name: "INVMS_SupplierName", title: "Supplier Name" },
                        { field: "INVMGRN_InvoiceNo", name: "INVMGRN_InvoiceNo", title: "Supplier InvoiceNo" },
                        { field: "INVMS_GSTNo", name: "INVMS_GSTNo", title: "GSTNo of Supplier" },
                        { field: "INVMI_HSNCode", name: "INVMI_HSNCode", title: "HSN Code" },
                        { field: "INVMUOM_UOMName", name: "INVMUOM_UOMName", title: "UOM" },
                        { field: "INVMLO_LocationRoomName", name: "INVMLO_LocationRoomName", title: "Location" },
                        { field: "INVTGRN_Qty", name: "INVTGRN_Qty", title: "Item Quantity" }, { field: "INVTGRN_PurchaseRate", name: "INVTGRN_PurchaseRate", title: "Basic Price", aggregates: ["sum"], footerTemplate: "Sum: #=sum#", groupFooterTemplate: "Sum: #=sum#" },
                        {
                            field: "INVTGRN_Amount", name: "INVTGRN_Amount", title: "Total value", aggregates: ["sum"], footerTemplate: "Sum: #=sum#", groupFooterTemplate: "Sum: #=sum#"
                        },
                        { field: "INVTGRN_TaxAmt", name: "INVTGRN_TaxAmt", title: "Taxes" },
                        { field: "INVT_Otherchrges", name: "INVT_Otherchrges", title: "Other Chargers" },
                        { field: "TotalAmount", name: "TotalAmount", title: "Total Amount" },
                        { field: "INVMPO_ReferenceNo", name: "INVMPO_ReferenceNo", title: "Purchase Order Reference" },
                        { field: "INVSPT_PaymentReference", name: "INVSPT_PaymentReference", title: "E-way Bill Reference" },

                        ];
                        $scope.newarray = [];
                        $scope.newarray1 = $scope.newarray;

                        $scope.data = promise.get_Inwardreport;
                        for (var i in $scope.data) {
                            $scope.data[i].rowNumber = count + 1;
                            $scope.data[i].GRNDate = $filter('date')($scope.data[i].GRNDate, "yyyy-MM-dd");
                            count = count + 1;
                        }
                        if (promise.get_Inwardreport != null && promise.get_Inwardreport.length > 0) {
                            var totalrate = 0.00;
                            var totaltax = 0.00;
                            var totalamt = 0.00;
                            //$scope.get_Inwardreport = promise.get_Inwardreport;
                            //$scope.presentCountgrid = $scope.get_Inwardreport.length;

                            angular.forEach($scope.get_Inwardreport, function (po) {
                            });

                            angular.forEach($scope.header_list, function (op) {
                                op.columns = [];
                                angular.forEach($scope.columns1, function (op123) {

                                    op.columns.push({ field: "hema" + op.ftI_Id + op123.field, name: "hema" + op.ftI_Id + op123.name, title: op123.title, width: 100 });

                                })
                                op.title = op.ftI_Name;
                            })



                            $scope.colarrayall = [{

                                title: "SLNO",
                                template: "<span class='row-number'></span>", width: "75px"

                            },
                            {
                                name: 'INVMGRN_GRNNo', field: 'INVMGRN_GRNNo', title: 'GRN No', width: "150px"
                            },
                            {
                                name: 'GRNDate', field: 'GRNDate', title: 'GRN Date', width: 150
                            },
                            {
                                name: 'INVMI_ItemCode', field: 'INVMI_ItemCode', title: 'Item Code', width: "150px"
                            },
                            {
                                name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item Name', width: "250px"
                            },
                            { name: "INVMI_ItemDesc", field: "INVMI_ItemDesc", title: "Item Description", width: "250px" },
                            {
                                name: 'INVMS_SupplierName', field: 'INVMS_SupplierName', title: 'Supplier Name', width: "200px"
                            },
                            {
                                name: 'INVMGRN_InvoiceNo', field: 'INVMGRN_InvoiceNo', title: 'Supplier InvoiceNo', width: "200px"
                            },
                            {
                                name: 'INVMS_GSTNo', field: 'INVMS_GSTNo', title: 'GSTNO of Supplier', width: "200px"
                            },
                            {
                                name: 'INVMI_HSNCode', field: 'INVMI_HSNCode', title: 'HSN Code', width: "200px"
                            },

                            {
                                name: 'INVMUOM_UOMName', field: 'INVMUOM_UOMName', title: 'UOM', width: "100px"
                            },
                            {
                                name: 'INVMLO_LocationRoomName', field: 'INVMLO_LocationRoomName', title: 'Location', width: "200px"
                            },
                            {
                                name: 'INVTGRN_Qty', field: 'INVTGRN_Qty', title: 'Item Quantity', width: "150px"
                            },
                            {
                                name: 'INVTGRN_PurchaseRate', field: 'INVTGRN_PurchaseRate', title: 'Basic Price', width: "100px", width: "160px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                            },
                            {
                                name: 'INVTGRN_Amount', field: 'INVTGRN_Amount', title: 'Total Basic Price', width: "160px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"

                            },
                            {
                                name: 'INVTGRN_TaxAmt', field: 'INVTGRN_TaxAmt', title: 'Taxes', width: 100, format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                //footerTemplate: "Total:",
                                //groupFooterTemplate: "Total: "
                            },
                            {
                                name: 'INVT_Otherchrges', field: 'INVT_Otherchrges', title: 'Other Chargers', width: "150px"
                            },
                            {

                                name: 'TotalAmount', field: 'TotalAmount', title: 'Total Amount', width: "150px", width: 100, format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                            },

                            {
                                name: 'INVMPO_ReferenceNo', field: 'INVMPO_ReferenceNo', title: 'Purchase Order Reference', width: 150
                            },
                            {
                                name: 'INVSPT_PaymentReference', field: 'INVSPT_PaymentReference', title: 'E-way Bill Reference', width: 150
                            }



                            ];


                            $scope.tempaggary.push(
                                { field: 'INVTGRN_PurchaseRate', name: 'INVTGRN_PurchaseRate', format: "{0:n2}", aggregate: "sum" },
                                {
                                    field: 'INVTGRN_TaxAmt', name: 'INVTGRN_TaxAmt', format: "{0:n2}", aggregate: "sum"
                                },
                                {

                                    field: 'INVTGRN_Amount', name: 'INVTGRN_Amount', format: "{0:n2}", aggregate: "sum"
                                },
                                {
                                    field: 'TotalAmount', name: 'TotalAmount', format: "{0:n2}", aggregate: "sum"
                                },
                                //INVTGRN_TaxAmt

                            );

                            ///function (e) { return e.userDetails.UserTypeId == 1 ? false : true; }
                            angular.forEach($scope.header_list, function (obj) {
                                $scope.colarrayall.push(obj);
                            })



                            console.log($scope.header_list);

                            console.log($scope.colarrayall);

                            //$scope.txtdata = "TIME :" + " " + $filter('date')(new Date(), 'HH:mm:ss') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname;
                            $scope.txtdata = "Inward Report";

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
                                $('#grid123').empty();

                                gridall = $("#grid123").kendoGrid({
                                    toolbar: ["excel", "pdf"],
                                    excel: {
                                        fileName: "InwardReport.xlsx",
                                        proxyURL: "",
                                        filterable: true,
                                        allPages: true

                                    },


                                    pdf: {
                                        paperSize: "A4",
                                        scale: 0.35,
                                        margin: {
                                            top: "2cm",
                                            left: "1cm",
                                            right: "1cm",
                                            bottom: "1cm"
                                        },
                                        avoidLinks: true,
                                        landscape: true,
                                        repeatHeaders: true,
                                        fileName: "InwardReport.pdf",
                                        allPages: true

                                    },
                                    dataSource: {

                                        //type: "odata",
                                        //transport: {
                                        //    read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Products"
                                        //},
                                        data: $scope.data,
                                        pageSize: 20,
                                        aggregate: $scope.tempaggary

                                    },
                                    sortable: true,
                                    pageable: true,
                                    groupable: false,
                                    filterable: true,
                                    columnMenu: true,
                                    reorderable: true,
                                    resizable: true,

                                    columns: $scope.aaaa,
                                    dataBound: function () {
                                        var pagenum = this.dataSource.page();
                                        var pageitms = this.dataSource.pageSize();
                                        var rows = this.items();
                                        $(rows).each(function () {
                                            var index = (pagenum - 1) * pageitms + $(this).index() + 1;
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
                            $scope.get_Inwardreport = "";
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
        $scope.exportToPdf = function (export_id) {
            var exportHref = Excel.tableTopdf(export_id, 'printInward');
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