
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_PurchaseReturn_ReportController', INV_PurchaseReturn_ReportController);
    INV_PurchaseReturn_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter']
    function INV_PurchaseReturn_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {
        
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
            apiService.create("INV_PurchaseReturn_Report/getloaddata", data).
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
                       // $scope.get_POdetails = promise.get_POdetails;
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
         //   $scope.get_POdetails = "";
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

        //$scope.togchkbxSP = function () {
        //    $scope.spall = $scope.get_POdetails.every(function (itm) {
        //        return itm.spck;
        //    });
        //};
        //$scope.isOptionsRequiredI = function () {
        //    if ($scope.optionflag === 'Item-wise') {
        //        return !$scope.get_POdetails.some(function (options) {
        //            return options.itemck;
        //        });
        //    } else {
        //        return false;
        //    }
        //};
        //$scope.isOptionsRequiredSP = function () {
        //    if ($scope.optionflag === 'Supplier') {
        //        return !$scope.get_POdetails.some(function (options) {
        //            return options.spck;
        //        });
        //    } else {
        //        return false;
        //    }
        //};
        //$scope.all_checkI = function (itmi) {
        //    $scope.itemall = itmi;
        //    var toggleStatus = $scope.itemall;
        //    angular.forEach($scope.get_POdetails, function (itm) {
        //        itm.itemck = toggleStatus;
        //    });
        //};
        //$scope.all_checkSP = function (itmi) {
        //    $scope.spall = itmi;
        //    var toggleStatus = $scope.spall;
        //    angular.forEach($scope.get_POdetails, function (itm) {
        //        itm.spck = toggleStatus;
        //    });
        //};
        //$scope.togchkbxG = function () {
        //    $scope.grnall = $scope.get_POdetails.every(function (itm) {
        //        return itm.grnck;
        //    });
        //};
        $scope.all_miid = function (ac) {
          
            $scope.allmi = ac;
            var toggleStatus = $scope.allmi;
            angular.forEach($scope.get_mi_list, function (itm) {
                itm.miid = toggleStatus;
            });
        };


        $scope.mi_click = function () {
         
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
        $scope.onreport = function () {

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

                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,

                    };

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
                //}
                //else if ($scope.optionflag === "Item-wise") {
                //    angular.forEach($scope.get_POdetails, function (itm) {
                //        if (itm.itemck === true) {
                //            $scope.poitemArray.push(itm);
                //        }
                //    });
                //    data = {
                //        "optionflag": $scope.optionflag,
                //        "startdate": $scope.sDate,
                //        "enddate": $scope.eDate,
                //        "poitemArray": $scope.poitemArray,
                //        "bwdateflag": $scope.bw_dates,
                //        "get_mi_arraylist": $scope.get_mi_arraylist,

                //    };
                //}
                //else if ($scope.optionflag === "Supplier") {
                //    angular.forEach($scope.get_POdetails, function (sp) {
                //        if (sp.spck === true) {
                //            $scope.posuplierArray.push(sp);
                //        }
                //    });
                //    data = {
                //        "optionflag": $scope.optionflag,
                //        "startdate": $scope.sDate,
                //        "enddate": $scope.eDate,
                //        "posuplierArray": $scope.posuplierArray,
                //        "bwdateflag": $scope.bw_dates,
                //        "get_mi_arraylist": $scope.get_mi_arraylist,

                //    };
                //}
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("INV_PurchaseReturn_Report/onreport", data).
                    then(function (promise) {
                        //if (promise.get_PurReturnreport.length > 0) {
                        $('#grid123').empty();

                        var count = 0;
                   
                        $scope.insarray = [{ name: "rowNumber" },
                            { name: "INVMGRNRET_DCNo" },
                            { name: "INVMGRNRET_DCDate" },
                            { name: "INVMGRNRET_GRNRETNo" },
                            { name: "INVMGRNRET_ReturnDate" },
                            { name: "INVMGRN_InvoiceNo" },
                            { name: "INVMGRN_date" },
                            { name: "INVMGRNRET_Remarks" },
                            { name: "INVMI_HSNCode" }, { name: "INVMUOM_UOMName" },
                            { name: "INVTGRNRET_ReturnQty" },
                            { name: "INVTGRN_MRP" }, { name: "INVTGRNRET_ReturnAmount" },
                            { name: "INVTGRNTRET_TaxAmt" },
                            { name: "INVMGRNRET_TotalAmount" },
                            { name: "INVSPT_PaymentReference" }];
                        $scope.columns1 = [{ feild: "rowNumber", name: "rowNumber" }, { field: "INVMGRNRET_DCNo", name: "INVMGRNRET_DCNo", title: "DC No" }, { field: "INVMGRNRET_DCDate", name: "INVMGRNRET_DCDate", title: "DC Date", format: "{0: dd-MM-yy}" }, { field: "INVMGRNRET_GRNRETNo", name: "INVMGRNRET_GRNRETNo", title: "Debit Note No" }, { field: "INVMGRNRET_ReturnDate", name: "INVMGRNRET_ReturnDate", title: "DN Date", format: "{0: dd-MM-yy}"}, { field: "INVMGRN_InvoiceNo", name: "INVMGRN_InvoiceNo", title: "Supplier InvoiceNo" }, { field: "INVMGRN_date", name: "INVMGRN_date", title: "Invoice Date" }, { field: "INVMGRNRET_Remarks", name: "INVMGRNRET_Remarks", title: "Remarks" }, { field: "INVMI_HSNCode", name: "INVMI_HSNCode", title: "HSN Code" }, { field: "INVMUOM_UOMName", name: "INVMUOM_UOMName", title: "UOM" }, { field: "INVTGRNRET_ReturnQty", name: "INVTGRNRET_ReturnQty", title: "Quantity" }, { field: "INVTGRN_MRP", name: "INVTGRN_MRP", title: "Rate" }, { field: "INVTGRNRET_ReturnAmount", name: "INVTGRNRET_ReturnAmount", title: "Amount" }, { field: "INVTGRNTRET_TaxAmt", name: "INVTGRNTRET_TaxAmt", title: "GST" }, {
                            field: "INVMGRNRET_TotalAmount", name: "INVMGRNRET_TotalAmount", title: "Total Amount", aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                            groupFooterTemplate: "Sum: #=sum#"
                        },
                            { field: "INVSPT_PaymentReference", name: "INVSPT_PaymentReference", title: "E-way Bill Number" }];

                        $scope.newarray = [];

                        $scope.newarray1 = $scope.newarray;
                        $scope.data = promise.get_PurReturnreport;                    
                        if (promise.get_PurReturnreport.length > 0) {
                            $scope.get_PurReturnreport = true;                       
                            angular.forEach($scope.header_list, function (op) {
                                op.columns = [];
                                angular.forEach($scope.columns1, function (op123) {

                                    op.columns.push({ field: "hema" + op.ftI_Id + op123.field, name: "hema" + op.ftI_Id + op123.name, title: op123.title, width: 100 });

                                })
                                op.title = op.ftI_Name;
                            })

                        
                            for (var i in $scope.data) {
                                $scope.data[i].rowNumber = count + 1;
                                count = count + 1;
                            }
                          

                            $scope.tempaggary = [];
                            $scope.colarrayall = [
                                {

                         
                                    name: 'rowNumber',
                                    field: 'rowNumber',
                                title: "SLNO",
                                  template: "<span class='row-number'></span>",
                                    //template: "#= ++count #",
                                width:100
                    

                                },




                            {
                                name: 'INVMGRNRET_DCNo', field: 'INVMGRNRET_DCNo', title: 'DC No', width: 150
                                },


                               
                            {
                                name: 'INVMGRNRET_DCDate', field: 'INVMGRNRET_DCDate', title: 'DC Date', width: 150, format: "{0: dd-MM-yy}"
                                //, template: "#= kendo.toString(kendo.parseDate(INVMGRNRET_DCDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"

                            },

                            {
                                name: 'INVMGRNRET_DebitNoteNo', field: 'INVMGRNRET_DebitNoteNo', title: 'Debit Note No', width: 150
                            },
                            {
                                name: 'INVMGRNRET_ReturnDate', field: 'INVMGRNRET_ReturnDate', title: 'DN Date', width: 150, format: "{0: dd-MM-yy}"
                                //, template: "#= kendo.toString(kendo.parseDate(INVMGRNRET_ReturnDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                            },

                            {
                                name: 'INVMGRN_InvoiceNo', field: 'INVMGRN_InvoiceNo', title: 'Supplier InvoiceNo', width: 200
                            },
                            {
                                name: 'INVMGRN_date', field: 'INVMGRN_date', title: 'Supplier Invoice date', width: 200
                                },
                           
                            {
                                name: 'INVMGRNRET_Remarks', field: 'INVMGRNRET_Remarks', title: 'Remarks', width: 150
                            },
                            {
                                name: 'INVMI_HSNCode', field: 'INVMI_HSNCode', title: 'HSN Code', width: 250
                            },

                            {
                                name: 'INVMUOM_UOMName', field: 'INVMUOM_UOMName', title: 'UOM', width: 100
                            },
                            {
                                name: 'INVTGRNRET_ReturnQty', field: 'INVTGRNRET_ReturnQty', title: 'Quantity', width: 150
                            },
                            {
                                name: 'INVTGRN_MRP', field: 'INVTGRN_MRP', title: 'Rate', width: 100
                            },
                            {
                                name: 'INVTGRNRET_ReturnAmount', field: 'INVTGRNRET_ReturnAmount', title: 'Amount', width: 150
                            },

                            {
                                name: 'INVTGRNTRET_TaxAmt', field: 'INVTGRNTRET_TaxAmt', title: 'GST', width: 100, format: "{0:n2}", footerTemplate: "Total:",
                                groupFooterTemplate: "Total: "
                            },
                            {
                                name: 'INVMGRNRET_TotalAmount', field: 'INVMGRNRET_TotalAmount', title: 'Total Amount', width: 160, format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"

                            },
                            {
                                name: 'INVMGRNRET_EWayBillNo', field: 'INVMGRNRET_EWayBillNo', title: 'E-way Bill Number', width: 100
                            },

                            ];
                            $scope.tempaggary.push({ field: 'INVMGRNRET_TotalAmount', name: 'INVMGRNRET_TotalAmount', format: "{0:n2}", aggregate: "sum" });


                            angular.forEach($scope.header_list, function (obj) {
                                $scope.colarrayall.push(obj);
                            })

                            console.log($scope.header_list);

                            console.log($scope.colarrayall);

                         
                            $scope.aaaa = [{
                         
                                columns: $scope.colarrayall
                            }]

                            console.log($scope.colarrayall);
                      
                            var gridall;

                            $(document).ready(function () {
                                initGridall();

                            });


                           
                            function initGridall() {
                              
                          
                                gridall = $("#grid123").kendoGrid({
                                    toolbar: ["excel", "pdf"],
                                    excel: {
                                        fileName: "PurchaseReturnReport.xlsx",
                                        proxyURL: "",
                                        filterable: true,
                                        allPages: true,
                                      
                                        columnMenu: true
                                      
                                    },
                                    pdf: {
                                        fileName: "PurchaseReturnReport.pdf",
                                          allPages: true
                                    },
                                    dataSource: {


                                        data: $scope.data,
                                        pageSize: 10,
                                     
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
                                    sortable: true,
                                    index: true
                                });
                            }
                        }

                        else {
                            swal("Record Not Found");
                            $scope.get_PurReturnreport = false;
                        }
                    })
               
            
           
                }
                else {
                    $scope.submitted = true;
                }


            }
       
        
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cancel = function () {
            $state.reload();
        };
       
        $scope.exportToExcel = function (export_id) {

            var exportHref = Excel.tableToExcel(export_id, 'sheet name');
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