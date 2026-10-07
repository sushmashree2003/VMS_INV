
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_Purchase_ReportController', INV_Purchase_ReportController);
    INV_Purchase_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter']
    function INV_Purchase_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {
        
        $scope.obj = {};
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        $scope.companyname = localStorage.getItem('MIName');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        $scope.coptyright = copty;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
       

        //==============================
      
        //$scope.mi_click = function () {
        //    $scope.allmi = $scope.get_mi_list.every(function (itm) {
        //        return itm.miid;
        //    });
        //    $scope.get_mi_arraylist1 = [];
        //    angular.forEach($scope.get_mi_list, function (qq) {
        //        if (qq.miid == true) {
        //            $scope.get_mi_arraylist1.push({ MI_Id: qq.MI_Id })
        //        }

        //    })
        //    $scope.flag = $scope.optionflag;
        //    $scope.loaddata($scope.flag, $scope.get_mi_arraylist1);

        //};


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
            apiService.create("INV_Purchase_Report/getloaddata", data).
                then(function (promise) {
                    $scope.staffcheck = promise.staffcheck;
                    if ($scope.staffcheck == 'Staff') {
                        $scope.MI_Name = promise.get_mi_list[0].MI_Name;
                        $scope.MI_Id = promise.get_mi_list[0].MI_Id;
                        angular.forEach($scope.get_mi_list, function (a) {
                            a.miid = true;
                        })
                        $scope.get_POdetails = promise.get_POdetails;

                    }
                    else {
                        $scope.get_POdetails = promise.get_POdetails;
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
            $scope.get_POdetails = "";
            $scope.get_POreport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag, $scope.get_mi_arraylist1);
        };
       
        //========================ITEM
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.get_POdetails.every(function (itm) {
                return itm.itemck;
            });
        };

        $scope.isOptionsRequiredG = function () {
            if ($scope.optionflag === 'GRNNo-Wise') {
                return !$scope.get_POdetails.some(function (options) {
                    return options.grnck;
                });
            } else {
                return false;
            }
        };
        

        $scope.all_checkG = function (itmi) {
            $scope.grnall = itmi;
            var toggleStatus = $scope.grnall;
            angular.forEach($scope.get_POdetails, function (itm) {
                itm.grnck = toggleStatus;
            });
        };

        $scope.togchkbxSP = function () {
            $scope.spall = $scope.get_POdetails.every(function (itm) {
                return itm.spck;
            });
        };
        $scope.isOptionsRequiredI = function () {
            if ($scope.optionflag === 'Item-wise') {
                return !$scope.get_POdetails.some(function (options) {
                    return options.itemck;
                });
            } else {
                return false;
            }
        };
        $scope.isOptionsRequiredSP = function () {
            if ($scope.optionflag === 'Supplier') {
                return !$scope.get_POdetails.some(function (options) {
                    return options.spck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkI = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.get_POdetails, function (itm) {
                itm.itemck = toggleStatus;
            });
        };
        $scope.all_checkSP = function (itmi) {
            $scope.spall = itmi;
            var toggleStatus = $scope.spall;
            angular.forEach($scope.get_POdetails, function (itm) {
                itm.spck = toggleStatus;
            });
        };
        $scope.togchkbxG = function () {
            $scope.grnall = $scope.get_POdetails.every(function (itm) {
                return itm.grnck;
            });
        };
        $scope.all_miid = function (ac) {
            $scope.get_POdetails = [];
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
            $scope.get_POdetails = [];
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

        //==================================PO Report
        $scope.submitted = false;
        $scope.onreport = function ()
        {

            $("#grid123").kendoGrid();
            $('#grid123').data('kendoGrid').dataSource.read();
            $('#grid123').data('kendoGrid').refresh();

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
               

                $scope.grnoArray = [];
                $scope.poitemArray = [];
                $scope.posuplierArray = [];
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
                else if ($scope.optionflag === "GRNNo-Wise") {
                    angular.forEach($scope.get_POdetails, function (grn) {
                        if (grn.grnck === true) {
                            $scope.grnoArray.push(grn);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "grnoArray": $scope.grnoArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,

                    };
                }
                else if ($scope.optionflag === "Item-wise") {
                    angular.forEach($scope.get_POdetails, function (itm) {
                        if (itm.itemck === true) {
                            $scope.poitemArray.push(itm);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "poitemArray": $scope.poitemArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,

                    };
                }
                else if ($scope.optionflag === "Supplier") {
                    angular.forEach($scope.get_POdetails, function (sp) {
                        if (sp.spck === true) {
                            $scope.posuplierArray.push(sp);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "posuplierArray": $scope.posuplierArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,

                    };
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("INV_Purchase_Report/onreport", data).
                    then(function (promise) {                      
                        $scope.get_POreport = true;
                        $scope.tempaggary = [];                    
                        $scope.insarray = [{ name: "INVMGRN_GRNNo" }, { name: "GRN_Date" },
                            { name: "INVMGRN_PurchaseDate" },
                            { name: "INVMS_SupplierName" },
                            { name: "INVMGRN_InvoiceNo" },
                            { name: "INVMI_ItemCode" },
                            { name: "INVMI_ItemName" },
                            { name: "INVMUOM_UOMName" },
                            { name: "INVTGRN_Qty" },
                            { name: "INVTGRN_PurchaseRate" },
                            { name: "INVTGRN_MRP" },
                            { name: "INVTGRN_SalesPrice" },
                            { name: "INVTGRN_DiscountAmt" },
                            { name: "INVTGRN_TaxAmt" },
                            { name: "INVTGRN_Amount" }];
                        $scope.columns1 = [{ field: "INVMGRN_GRNNo", name: "INVMGRN_GRNNo", title: "GRN No" },
                            { field: "GRN_Date", name: "GRN_Date", title: "GRN Date" }, { field: "INVMGRN_PurchaseDate", name: "INVMGRN_PurchaseDate", title: "Purchase Date" }, { field: "INVMS_SupplierName", name: "INVMS_SupplierName", title: "Supplier Name" }, { field: "INVMGRN_InvoiceNo", name: "INVMGRN_InvoiceNo", title: "Supplier InvoiceNo" }, { field: "INVMI_ItemCode", name: "INVMI_ItemCode", title: "Item Code" }, { field: "INVMI_ItemName", name: "INVMI_ItemName", title: "Item" }, { field: "INVMUOM_UOMName", name: "INVMUOM_UOMName", title: "UOM Code" }, { field: "INVTGRN_Qty", name: "INVTGRN_Qty", title: "Quantity" }, { field: "INVTGRN_PurchaseRate", name: "INVTGRN_PurchaseRate", title: "Rate" }, { field: "INVTGRN_MRP", name: "INVTGRN_MRP", title: "MRP" }, { field: "INVTGRN_SalesPrice", name: "INVTGRN_SalesPrice", title: "Sales Rate" }, { field: "INVTGRN_DiscountAmt", name: "INVTGRN_DiscountAmt", title: "Discount" }, { field: "INVTGRN_TaxAmt", name: "INVTGRN_TaxAmt", title: "Taxes" }, {
                            field: "INVTGRN_Amount", name: "INVTGRN_Amount", title: "Total Amount", aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                            groupFooterTemplate: "Sum: #=sum#" }];
                        $scope.newarray = [];                    
                        $scope.newarray1 = $scope.newarray;                       
                        $scope.data = promise.get_POreport;
                        //$scope.presentCountgrid = $scope.get_POreport.length;
                        if (promise.get_POreport.length > 0) {
                            angular.forEach($scope.get_POreport, function (po) {
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
                                template: "<span class='row-number'></span>", width: 75

                            },
                                {
                                    name: 'INVMGRN_GRNNo', field: 'INVMGRN_GRNNo', title: 'GRN No', width: 150
                                },
                                {
                                    name: 'GRN_Date', field: 'GRN_Date', title: 'GRN Date', width: 150, template: "#= kendo.toString(kendo.parseDate(GRN_Date, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                                },
                                {
                                    name: 'INVMGRN_PurchaseDate', field: 'INVMGRN_PurchaseDate', title: 'Purchase Date', width: 150, template: "#= kendo.toString(kendo.parseDate(INVMGRN_PurchaseDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                                },
                                {
                                    name: 'INVMS_SupplierName', field: 'INVMS_SupplierName', title: 'Supplier Name', width: 200
                                },
                                {
                                    name: 'INVMGRN_InvoiceNo', field: 'INVMGRN_InvoiceNo', title: 'Supplier InvoiceNo', width: 200
                                },

                                {
                                    name: 'INVMI_ItemCode', field: 'INVMI_ItemCode', title: 'Item Code', width: 150
                                },
                                {
                                    name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item', width: 250
                                },
                                {
                                    name: 'INVMUOM_UOMName', field: 'INVMUOM_UOMName', title: 'UOM', width: 100
                                },
                                {
                                    name: 'INVTGRN_Qty', field: 'INVTGRN_Qty', title: 'Quantity', width: 150
                                },
                                {
                                    name: 'INVTGRN_PurchaseRate', field: 'INVTGRN_PurchaseRate', title: 'Rate', width: 100
                                },
                                {
                                    name: 'INVTGRN_MRP', field: 'INVTGRN_MRP', title: 'MRP', width: 100
                                },
                                {
                                    name: 'INVTGRN_SalesPrice', field: 'INVTGRN_SalesPrice', title: 'Sales Rate', width: 150
                                },
                                {
                                    name: 'INVTGRN_DiscountAmt', field: 'INVTGRN_DiscountAmt', title: 'Discount', width: 150
                                },
                                {
                                    name: 'INVTGRN_TaxAmt', field: 'INVTGRN_TaxAmt', title: 'Taxes', width: 100, format: "{0:n2}", footerTemplate: "Total:",
                                    groupFooterTemplate: "Total: "
                                },
                                {
                                    name: 'INVTGRN_Amount', field: 'INVTGRN_Amount', title: 'Total Amount', width: 160, format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                   
                                }
                                
                            ];
                            $scope.tempaggary.push({ field: 'INVTGRN_Amount', name: 'INVTGRN_Amount', format: "{0:n2}",  aggregate: "sum" });
                         

                            angular.forEach($scope.header_list, function (obj) {
                                $scope.colarrayall.push(obj);
                            })
                          
                            console.log($scope.header_list);

                            console.log($scope.colarrayall);

                            //$scope.txtdata = "TIME :" + " " + $filter('date')(new Date(), 'HH:mm:ss') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname;
                            $scope.txtdata = "Purchase Report";

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
                                        fileName: "PurchaseReport.xlsx",
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
                                        fileName: "PurchaseReport.pdf",
                                        allPages: true
                                        
                                    },
                                    dataSource: {
                                     
                                        //type: "odata",
                                        //transport: {
                                        //    read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Products"
                                        //},
                                        data: $scope.data,
                                        aggregate: $scope.tempaggary,
                                        pageSize: 20
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
                               
                     
                    })
            }
            else {
                $scope.submitted = true;
                $scope.get_POreport = "";
            }    
                        //   $scope.mainsubhdr = [];
                       
        }

        
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
            var exportHref = Excel.tableTopdf(export_id, 'printpo');
            $timeout(function () {
                location.href = exportHref;
            }, 100);

        };

        $scope.printData = function () {

            var innerContents = document.getElementById("printpo").innerHTML;
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