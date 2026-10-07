
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_Outward_ReportController', INV_Outward_ReportController);
    INV_Outward_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter']
    function INV_Outward_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {


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
            $scope.get_Outwarddetails = [];
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
            $scope.get_Outwarddetails = [];
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
            apiService.create("INV_Outward_Report/getloaddata", data).
                then(function (promise) {
                   
                    $scope.staffcheck = promise.staffcheck;
                    if ($scope.staffcheck == 'Staff') {
                        $scope.MI_Name = promise.get_mi_list[0].MI_Name;
                        $scope.MI_Id = promise.get_mi_list[0].MI_Id;
                        angular.forEach($scope.get_mi_list, function (a) {
                            a.miid = true;
                        })
                        $scope.get_Outwarddetails = promise.get_Outwarddetails;

                    }
                    else {
                        $scope.get_Outwarddetails = promise.get_Outwarddetails;
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
            $scope.get_Outwarddetails = "";
            $scope.get_Outwardreport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag, $scope.get_mi_arraylist1);
        };
       
        //========================ITEM
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.get_Outwarddetails.every(function (itm) {
                return itm.itemck;
            });
        };

        

        $scope.togchkbxSP = function () {
            $scope.spall = $scope.get_Outwarddetails.every(function (itm) {
                return itm.spck;
            });
        };
        $scope.isOptionsRequiredI = function () {
            if ($scope.optionflag === 'Item-Wise') {
                return !$scope.get_Outwarddetails.some(function (options) {
                    return options.itemck;
                });
            } else {
                return false;
            }
        };
        $scope.isOptionsRequiredSP = function () {
            if ($scope.optionflag === 'Customer-Wise') {
                return !$scope.get_Outwarddetails.some(function (options) {
                    return options.spck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkI = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.get_Outwarddetails, function (itm) {
                itm.itemck = toggleStatus;
            });
        };
        $scope.all_checkSP = function (itmi) {
            $scope.spall = itmi;
            var toggleStatus = $scope.spall;
            angular.forEach($scope.get_Outwarddetails, function (itm) {
                itm.spck = toggleStatus;
            });
        };

        //==================================Outward Report
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
               

             
                $scope.OutwarditemArray = [];
                $scope.OutwardcustomerArray = [];
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
                    angular.forEach($scope.get_Outwarddetails, function (itm) {
                        if (itm.itemck === true) {
                            $scope.OutwarditemArray.push(itm);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "OutwarditemArray": $scope.OutwarditemArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,
                      
                    };
                }
                else if ($scope.optionflag === "Customer-Wise") {
                    angular.forEach($scope.get_Outwarddetails, function (sp) {
                        if (sp.spck === true) {
                            $scope.OutwardcustomerArray.push(sp);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "OutwardcustomerArray": $scope.OutwardcustomerArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,
                       
                    };
                }
                apiService.create("INV_Outward_Report/onreport", data).
                    then(function (promise) {

                        
                        $scope.get_Outwardreport = true;
                        $scope.tempaggary = [];

                        $scope.insarray = [{ name: "INVMSL_DCNo" }, { name: "INVMSL_DCDate" }, { name: "INVMI_ItemCode" }, { name: "INVMI_ItemName" }, { name: "INVMI_HSNCode" }, { name: "INVMC_CustomerName" }, { name: "INVMSL_SalesInvoiceNo" }, { name: "INVMC_GSTNo" }, { name: "INVMUOM_UOMName" }, { name: "INVMLO_LocationRoomName" }, { name: "INVTSL_SalesQty" }, { name: "INVTSL_SalesPrice" }, { name: "INVTSL_TaxAmt" }, { name: "INVT_Otherchrges" }, { name: "INVTSL_Amount" }, { name: "INVMSO_ReferenceNo" }, { name: "INVSPT_PaymentReference" }];

                        $scope.columns1 = [{ field: "INVMI_DCNo", name: "INVMI_DCNo", title: "DC No" }, { field: "INVMI_DCDate", name: "INVMI_DCDate", title: "DC Date" }, { field: "INVMI_ItemCode", name: "INVMI_ItemCode", title: "Item Code" }, { field: "INVMI_ItemName", name: "INVMI_ItemName", title: "Item Description" }, { field: "INVMI_HSNCode", name: "INVMI_HSNCode", title: "HSN Code" }, { field: "INVMC_CustomerName", name: "INVMC_CustomerName", title: "Customer Name" }, { field: "INVMSL_SalesInvoiceNo", name: "INVMSL_SalesInvoiceNo", title: "Sales Invoice No" }, { field: "INVMC_GSTNo", name: "INVMC_GSTNo", title: "GSTNO of Customer" }, { field: "INVMUOM_UOMName", name: "INVMUOM_UOMName", title: "UOM" }, { field: "INVMLO_LocationRoomName", name: "INVMLO_LocationRoomName", title: "Location" }, { field: "INVTSL_SalesQty", name: "INVTSL_SalesQty", title: "Item Quantity" },   { field: "INVTSL_SalesPrice", name: "INVTSL_SalesPrice", title: "Basic Price" }, { field: "INVTSL_TaxAmt", name: "INVTSL_TaxAmt", title: "Taxes" }, { field: "INVT_Otherchrges", name: "INVT_Otherchrges", title: "Other Chargers" }, {
                            field: "INVTSL_Amount", name: "INVTSL_Amount", title: "Total value", aggregates: ["sum"], footerTemplate: "Sum: #=sum#", groupFooterTemplate: "Sum: #=sum#"
                        },
                            { field: "INVMSO_ReferenceNo", name: "INVMSO_ReferenceNo", title: "Sales Order Reference" }, { field: "INVSPT_PaymentReference", name: "INVSPT_PaymentReference", title: "E-way Bill Reference" },
                        ];
                        $scope.newarray = [];
                        $scope.newarray1 = $scope.newarray;
                        $scope.data = promise.get_Outwardreport;



                        if (promise.get_Outwardreport.length > 0) {


                            var totalrate = 0.00;
                            var totaltax = 0.00;
                            var totalamt = 0.00;                         
                        

                            angular.forEach($scope.get_Outwardreport, function (po) {
                          
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
                                    name: 'INVMSL_DCNo', field: 'INVMSL_DCNo', title: 'DC No', width: 150
                                },
                                {
                                    name: 'INVMSL_DCDate', field: 'INVMSL_DCDate', title: 'DC Date', width: 250
                                },
                            {
                                name: 'INVMI_ItemCode', field: 'INVMI_ItemCode', title: 'Item Code', width: 150
                            },
                            {
                                name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item Description', width: 250
                            },
                            
                            {
                                name: 'INVMI_HSNCode', field: 'INVMI_HSNCode', title: 'HSN Code', width: 150
                            },
                                {
                                    name: 'INVMC_CustomerName', field: 'INVMC_CustomerName', title: 'Customer Name', width: 200
                                },
                                
                                 {
                                     name: 'INVMSL_SalesInvoiceNo', field: 'INVMSL_SalesInvoiceNo', title: 'Sales Invoice No', width: 200
                                },
                                {
                                    name: 'INVMC_GSTNo', field: 'INVMC_GSTNo', title: 'GSTNo of Customer', width: 100
                                },

                                
                                 {
                                     name: 'INVMUOM_UOMName', field: 'INVMUOM_UOMName', title: 'UOM', width: 100
                                },
                                {
                                    name: 'INVMLO_LocationRoomName', field: 'INVMLO_LocationRoomName', title: 'Location', width: 100
                                },

                            {
                                name: 'INVTSL_SalesQty', field: 'INVTSL_SalesQty', title: 'Item Quantity', width: 100
                            },
                            
                           
                             
                                {
                                    name: 'INVTSL_SalesPrice', field: 'INVTSL_SalesPrice', title: 'Basic price', width: 150
                                },
                                {
                                    name: 'INVTSL_TaxAmt', field: 'INVTSL_TaxAmt', title: 'Taxes', width: 150
                                },
                                {
                                    name: 'INVT_Otherchrges', field: 'INVT_Otherchrges', title: 'Other Chargers', width: 150, footerTemplate: "Total:",
                                    groupFooterTemplate: "Total: "
                                },
                            
                            {
                                name: 'INVTSL_Amount', field: 'INVTSL_Amount', title: 'Total Amount', width: 160, format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"

                                },
                                {
                                    name: 'INVMSO_ReferenceNo', field: 'INVMSO_ReferenceNo', title: 'Sales Order Reference', width: 100
                                },

                                {
                                    name: 'INVSPT_PaymentReference', field: 'INVSPT_PaymentReference', title: 'E-way Bill Reference', width: 150
                                },
                            

                            ];
                            $scope.tempaggary.push({ field: 'INVTSL_Amount', name: 'INVTSL_Amount', format: "{0:n2}", aggregate: "sum" });
                            angular.forEach($scope.header_list, function (obj) {
                                $scope.colarrayall.push(obj);
                            })



                            console.log($scope.header_list);

                            console.log($scope.colarrayall);

                            $scope.txtdata = "TIME :" + " " + $filter('date')(new Date(), 'HH:mm:ss') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname;
                            $scope.txtdata = "Outward Report"
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
                                        fileName: "OutwardReport.xlsx",
                                        proxyURL: "",
                                        filterable: true,
                                        allPages: true

                                    },

                                    pdf: {

                                        avoidLinks: true,
                                        landscape: true,
                                        repeatHeaders: true,
                                        fileName: "OutwardReport.pdf",
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
                        else {
                            swal("No Record Found...!!");
                            $scope.get_Outwardreport = "";
                        }
                    });
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

            var innerContents = document.getElementById("printSales").innerHTML;
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