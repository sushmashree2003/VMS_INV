
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_R_SalesRprtController', INV_R_SalesRprtController);
    INV_R_SalesRprtController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter']
    function INV_R_SalesRprtController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {


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
            $scope.get_Salesdetails = [];
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
            $scope.get_Salesdetails = [];
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
            apiService.create("INV_R_SalesRprt/getloaddata", data).
                then(function (promise) {

                    $scope.staffcheck = promise.staffcheck;
                    if ($scope.staffcheck == 'Staff') {
                        $scope.MI_Name = promise.get_mi_list[0].MI_Name;
                        $scope.MI_Id = promise.get_mi_list[0].MI_Id;
                        angular.forEach($scope.get_mi_list, function (a) {
                            a.miid = true;
                        })
                        $scope.get_Salesdetails = promise.get_Salesdetails;

                    }
                    else {
                        $scope.get_Salesdetails = promise.get_Salesdetails;
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
            $scope.get_Salesdetails = "";
            $scope.get_Salesreport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag, $scope.get_mi_arraylist1);
        };

        //========================ITEM
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.get_Salesdetails.every(function (itm) {
                return itm.itemck;
            });
        };



        $scope.togchkbxSP = function () {
            $scope.spall = $scope.get_Salesdetails.every(function (itm) {
                return itm.spck;
            });
        };
        $scope.isOptionsRequiredI = function () {
            if ($scope.optionflag === 'Item-Wise') {
                return !$scope.get_Salesdetails.some(function (options) {
                    return options.itemck;
                });
            } else {
                return false;
            }
        };
        $scope.isOptionsRequiredSP = function () {
            if ($scope.optionflag === 'Customer-Wise') {
                return !$scope.get_Salesdetails.some(function (options) {
                    return options.spck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkI = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.get_Salesdetails, function (itm) {
                itm.itemck = toggleStatus;
            });
        };
        $scope.all_checkSP = function (itmi) {
            $scope.spall = itmi;
            var toggleStatus = $scope.spall;
            angular.forEach($scope.get_Salesdetails, function (itm) {
                itm.spck = toggleStatus;
            });
        };

        //==================================Sales Report
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


                $scope.SalesitemArray = [];
                $scope.SalescustomerArray = [];
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
                    angular.forEach($scope.get_Salesdetails, function (itm) {
                        if (itm.itemck === true) {
                            $scope.SalesitemArray.push(itm);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "SalesitemArray": $scope.SalesitemArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,

                    };
                }
                else if ($scope.optionflag === "Customer-Wise") {
                    angular.forEach($scope.get_Salesdetails, function (sp) {
                        if (sp.spck === true) {
                            $scope.SalescustomerArray.push(sp);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "SalescustomerArray": $scope.SalescustomerArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,

                    };
                }
                apiService.create("INV_R_SalesRprt/onreport", data).
                    then(function (promise) {


                        $scope.get_Salesreport = true;


                        $scope.insarray = [{ name: "INVMI_ItemCode" }, { name: "INVMI_ItemName" }, { name: "INVMS_SupplierName" }, { name: "INVMI_HSNCode" }, { name: "INVTSL_SalesQty" }, { name: "INVMSL_SalesNo" }, { name: "INVMSL_SalesDate" }, { name: "INVMC_CustomerName" }, { name: "GstnumOfCustumer" }, { name: "INVMPO_ReferenceNo" }, { name: "INVSPT_PaymentReference" }, { name: "INVTSL_SalesPrice" }, { name: "INVTSL_TaxAmt" }, { name: "INV_Otherchargers" }, { name: "INVTSL_Amount" }];
                        $scope.columns1 = [{ field: "INVMI_ItemCode", name: "INVMI_ItemCode", title: "Item Code" }, { field: "INVMI_ItemName", name: "INVMI_ItemName", title: "Item Description" }, { field: "INVMI_HSNCode", name: "INVMI_HSNCode", title: "HSN Code" }, { field: "INVTSL_SalesQty", name: "INVTSL_SalesQty", title: "Sales Quantity" }, { field: "INVMSL_SalesNo", name: "INVMSL_SalesNo", title: "Sales Invoice No" }, { field: "INVMSL_SalesDate", name: "INVMSL_SalesDate", title: "Sales Date" }, { field: "INVMC_CustomerName", name: "INVMC_CustomerName", title: "Customer Name" }, { field: "GstnumOfCustumer", name: "GstnumOfCustumer", title: "GSTNO of Customer" }, { field: "INVMPO_ReferenceNo", name: "INVMPO_ReferenceNo", title: "Order Reference" }, { field: "INVSPT_PaymentReference", name: "INVSPT_PaymentReference", title: "E-way Bill Reference" }, { field: "INVTSL_SalesPrice", name: "INVTSL_SalesPrice", title: "Basic Price" }, { field: "INVTSL_TaxAmt", name: "INVTSL_TaxAmt", title: "GST" }, { field: "INVT_Otherchrges", name: "INVT_Otherchrges", title: "Other Chargers" }, {
                            field: "INVTSL_Amount", name: "INVTSL_Amount", title: "Total value", aggregates: ["sum"], footerTemplate: "Sum: #=sum#", groupFooterTemplate: "Sum: #=sum#"
                        }
                        ];
                        $scope.newarray = [];
                        $scope.newarray1 = $scope.newarray;
                        $scope.data = promise.get_Salesreport;



                        if (promise.get_Salesreport.length > 0) {


                            var totalrate = 0.00;
                            var totaltax = 0.00;
                            var totalamt = 0.00;
                            //$scope.get_Salesreport = promise.get_Salesreport;
                            //$scope.presentCountgrid = $scope.get_Salesreport.length;

                            angular.forEach($scope.get_Salesreport, function (po) {
                                //totalrate += parseFloat(po.INVTPO_RatePerUnit);
                                //$scope.totalrate = totalrate;
                                //$scope.totalrate = parseFloat($scope.totalrate);
                                //$scope.totalrate = $scope.totalrate.toFixed(2);

                                //totaltax += parseFloat(po.INVTPO_TaxAmount);
                                //$scope.totaltax = totaltax;
                                //$scope.totaltax = parseFloat($scope.totaltax);
                                //$scope.totaltax = $scope.totaltax.toFixed(2);

                                //totalamt += parseFloat(po.INVTPO_Amount);
                                //$scope.totalamt = totalamt;
                                //$scope.totalamt = parseFloat($scope.totalamt);
                                //$scope.totalamt = $scope.totalamt.toFixed(2);
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
                                name: 'INVMI_ItemCode', field: 'INVMI_ItemCode', title: 'Item Code', width: 150
                            },
                            {
                                name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item Description', width: 250
                            },

                            {
                                name: 'INVMI_HSNCode', field: 'INVMI_HSNCode', title: 'HSN Code', width: 150
                            },

                            {
                                name: 'INVTSL_SalesQty', field: 'INVTSL_SalesQty', title: 'Sales Quantity', width: 100
                            },
                            {
                                name: 'INVMSL_SalesNo', field: 'INVMSL_SalesNo', title: 'Sales InvoiceNo', width: 200
                            },
                            {
                                name: 'INVMSL_SalesDate', field: 'INVMSL_SalesDate', title: 'Sales Date', width: 150, template: "#= kendo.toString(kendo.parseDate(INVMSL_SalesDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                            },
                            {
                                name: 'INVMC_CustomerName', field: 'INVMC_CustomerName', title: 'Customer Name', width: 200
                            },
                            {
                                name: 'INVMC_GSTNo', field: 'INVMC_GSTNo', title: 'GSTNo of Customer', width: 200
                            },
                            {
                                name: 'INVMPO_ReferenceNo', field: 'INVMPO_ReferenceNo', title: 'Order Reference', width: 100
                            },

                            {
                                name: 'INVSPT_PaymentReference', field: 'INVSPT_PaymentReference', title: 'E-way Bill Reference', width: 150
                            },
                            {
                                name: 'INVTSL_SalesPrice', field: 'INVTSL_SalesPrice', title: 'Basic price', width: 150
                            },
                            {
                                name: 'INVTSL_TaxAmt', field: 'INVTSL_TaxAmt', title: 'GST', width: 150
                            },
                            {
                                name: 'INVT_Otherchrges', field: 'INVT_Otherchrges', title: 'Other Chargers', width: 150, footerTemplate: "Total:",
                                groupFooterTemplate: "Total: "
                            },

                            {
                                name: 'INVTSL_Amount', field: 'INVTSL_Amount', title: 'Total Amount', width: 160, format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"

                            }


                            ];
                            $scope.tempaggary.push({ field: 'INVTSL_Amount', name: 'INVTSL_Amount', format: "{0:n2}", aggregate: "sum" });
                            angular.forEach($scope.header_list, function (obj) {
                                $scope.colarrayall.push(obj);
                            })



                            console.log($scope.header_list);

                            console.log($scope.colarrayall);

                            //$scope.txtdata = "TIME :" + " " + $filter('date')(new Date(), 'HH:mm:ss') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname;
                            $scope.txtdata = "Sales Report";
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
                                        fileName: "SalesReport.xlsx",
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
                                        fileName: "SalesReport.pdf",
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
                            $scope.get_Salesreport = "";
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