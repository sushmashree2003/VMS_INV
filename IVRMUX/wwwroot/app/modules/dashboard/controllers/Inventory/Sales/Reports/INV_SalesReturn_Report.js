
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_R_SalesReturnController', INV_R_SalesReturnController);
    INV_R_SalesReturnController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter']
    function INV_R_SalesReturnController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {


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
            $scope.get_SalesReturndetails = [];
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
            $scope.get_SalesReturndetails = [];
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
            apiService.create("INV_R_SalesReturn/getloaddata", data).
                then(function (promise) {
                    $scope.staffcheck = promise.staffcheck;
                    if ($scope.staffcheck == 'Staff') {
                        $scope.MI_Name = promise.get_mi_list[0].MI_Name;
                        $scope.MI_Id = promise.get_mi_list[0].MI_Id;
                        angular.forEach($scope.get_mi_list, function (a) {
                            a.miid = true;
                        })
                        $scope.get_SalesReturndetails = promise.get_SalesReturndetails;

                    }
                    else {
                        $scope.get_SalesReturndetails = promise.get_SalesReturndetails;
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
            $scope.get_SalesReturndetails = "";
            $scope.get_SalesReturnreport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag, $scope.get_mi_arraylist1);
        };
       
        //========================ITEM
        //$scope.togchkbxI = function () {
        //    $scope.itemall = $scope.get_SalesReturndetails.every(function (itm) {
        //        return itm.itemck;
        //    });
        //};
        //$scope.togchkbxSP = function () {
        //    $scope.spall = $scope.get_SalesReturndetails.every(function (itm) {
        //        return itm.spck;
        //    });
        //};
        $scope.togchkbxG = function () {
            $scope.grnall = $scope.get_SalesReturndetails.every(function (itm) {
                return itm.grnck;
            });
        };
        //$scope.isOptionsRequiredI = function () {
        //    if ($scope.optionflag === 'Item-Wise') {
        //        return !$scope.get_Inwarddetails.some(function (options) {
        //            return options.itemck;
        //        });
        //    } else {
        //        return false;
        //    }
        //};
        $scope.isOptionsRequiredG = function () {
            if ($scope.optionflag === 'GRNNo-wise') {
                return !$scope.get_SalesReturndetails.some(function (options) {
                    return options.grnck;
                });
            } else {
                return false;
            }
        };
        //$scope.isOptionsRequiredSP = function () {
        //    if ($scope.optionflag === 'Supplier') {
        //        return !$scope.get_Inwarddetails.some(function (options) {
        //            return options.spck;
        //        });
        //    } else {
        //        return false;
        //    }
        //};
        $scope.all_checkG = function (itmi) {
            $scope.grnall = itmi;
            var toggleStatus = $scope.grnall;
            angular.forEach($scope.get_SalesReturndetails, function (itm) {
                itm.grnck = toggleStatus;
            });
        };

        //$scope.all_checkI = function (itmi) {
        //    $scope.itemall = itmi;
        //    var toggleStatus = $scope.itemall;
        //    angular.forEach($scope.get_Inwarddetails, function (itm) {
        //        itm.itemck = toggleStatus;
        //    });
        //};
        //$scope.all_checkSP = function (itmi) {
        //    $scope.spall = itmi;
        //    var toggleStatus = $scope.spall;
        //    angular.forEach($scope.get_Inwarddetails, function (itm) {
        //        itm.spck = toggleStatus;
        //    });
        //};

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
              

                $scope.grnoArray = [];
              
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
                else if ($scope.optionflag === "GRNNo-wise") {
                    angular.forEach($scope.get_SalesReturndetails, function (grn) {
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
            
                //else if ($scope.optionflag === "Item-Wise") {
                //    angular.forEach($scope.get_Inwarddetails, function (itm) {
                //        if (itm.itemck === true) {
                //            $scope.InwarditemArray.push(itm);
                //        }
                //    });
                //    data = {
                //        "optionflag": $scope.optionflag,
                //        "startdate": $scope.sDate,
                //        "enddate": $scope.eDate,
                //        "InwarditemArray": $scope.InwarditemArray,
                //        "bwdateflag": $scope.bw_dates,
                //        "get_mi_arraylist": $scope.get_mi_arraylist,
                      
                //    };
                //}
                //else if ($scope.optionflag === "Supplier") {
                //    angular.forEach($scope.get_Inwarddetails, function (sp) {
                //        if (sp.spck === true) {
                //            $scope.InwardsuplierArray.push(sp);
                //        }
                //    });
                //    data = {
                //        "optionflag": $scope.optionflag,
                //        "startdate": $scope.sDate,
                //        "enddate": $scope.eDate,
                //        "InwardsuplierArray": $scope.InwardsuplierArray,
                //        "bwdateflag": $scope.bw_dates,
                //        "get_mi_arraylist": $scope.get_mi_arraylist,
                       
                //    };
                //}
                apiService.create("INV_R_SalesReturn/onreport", data).
                    then(function (promise) {


                        $('#grid123').empty();
                        $scope.get_SalesReturnreport = true;
                        var count = 0;

                        $scope.insarray = [{ name: "INVMGRN_GRNNo" },
                            { name: "GRNDate" },
                            { name: "INVMSLRET_SalesReturnNo" },
                            { name: "INVMSLRET_SalesReturnDate" },
                            { name: "INVMSL_SalesNo" },
                            { name: "INVMSL_SalesDate" },
                            { name: "INVMSLRET_ReturnRemarks" },
                            { name: "INVMI_HSNCode" },
                            { name: "INVMUOM_UOMName" },
                            { name: "INVTSLRET_SalesReturnQty" },
                            { name: "INVTGRN_Rate" },
                            { name: "INVTSLRET_SalesReturnAmount" },
                            { name: "INVTSLTRET_TaxAmt" },
                            { name: "INVMSLRET_TotalReturnAmount" },
                            { name: "INVSPT_PaymentReference" }];
                        $scope.columns1 = [{ field: "INVMGRN_GRNNo", name: "INVMGRN_GRNNo", title: "GRN No" }, { field: "GRNDate", name: "GRNDate", title: "GRN Date" }, { field: "INVMSLRET_SalesReturnNo", name: "INVMSLRET_SalesReturnNo", title: "Credit Note No " }, { field: "INVMSLRET_SalesReturnDate", name: "INVMSLRET_SalesReturnDate", title: "CN Date" }, { field: "INVMSL_SalesNo", name: "INVMSL_SalesNo", title: "Sales Invoice No" }, { field: "INVMSL_SalesDate", name: "INVMSL_SalesDate", title: "Sales Invoice Date" }, { field: "INVMSLRET_ReturnRemarks", name: "INVMSLRET_ReturnRemarks", title: "Remarks" }, { field: "INVMI_HSNCode", name: "INVMI_HSNCode", title: "HSN Code" }, { field: "INVMUOM_UOMName", name: "INVMUOM_UOMName", title: "UOM" }, { field: "INVTSLRET_SalesReturnQty", name: "INVTSLRET_SalesReturnQty", title: "Quantity" }, { field: "INVTGRN_Rate", name: "INVTGRN_Rate", title: "Rate" }, { field: "INVTSLRET_SalesReturnAmount", name: "INVTSLRET_SalesReturnAmount", title: "Amount" }, { field: "INVTSLTRET_TaxAmt", name: "INVTSLTRET_TaxAmt", title: "GST" }, {
                            field: "INVMSLRET_TotalReturnAmount", name: "INVMSLRET_TotalReturnAmount", title: "Total value", aggregates: ["sum"], footerTemplate: "Sum: #=sum#", groupFooterTemplate: "Sum: #=sum#"
                        },
                       
                            { field: "INVSPT_PaymentReference", name: "INVSPT_PaymentReference", title: "E-way Bill Number" }
                        ];
                        $scope.newarray = [];
                        $scope.newarray1 = $scope.newarray;

                        $scope.data = promise.get_SalesReturnreport;
                      //  $scope.data = promise.get_Agingreport;

                        for (var i in $scope.data) {
                            $scope.data[i].rowNumber = count + 1;
                            $scope.data[i].GRNDate = $filter('date')($scope.data[i].GRNDate, "yyyy-MM-dd");
                            $scope.data[i].INVMSLRET_SalesReturnDate = $filter('date')($scope.data[i].INVMSLRET_SalesReturnDate, "yyyy-MM-dd");
                            $scope.data[i].INVMS_SalesINVDate = $filter('date')($scope.data[i].INVMS_SalesINVDate, "yyyy-MM-dd");
                            $scope.data[i].INVMSLRET_SalesReturnDate = $filter('date')($scope.data[i].INVMSLRET_SalesReturnDate, "yyyy-MM-dd");
                            $scope.data[i].INVMS_SalesINVDate = $filter('date')($scope.data[i].INVMS_SalesINVDate, "yyyy-MM-dd");
                            $scope.data[i].INVTGRN_Rate = $filter('date')($scope.data[i].INVTGRN_Rate, "yyyy-MM-dd");

                            count = count + 1;
                        }
                        if (promise.get_SalesReturnreport.length > 0) {
                            var totalrate = 0.00;
                            var totaltax = 0.00;
                            var totalamt = 0.00;                         
                        

                            angular.forEach($scope.get_SalesReturnreport, function (po) {
                            });

                                angular.forEach($scope.header_list, function (op) {
                                    op.columns = [];
                                    angular.forEach($scope.columns1, function (op123) {

                                        op.columns.push({
                                            field: "hema" + op.ftI_Id + op123.field, name: "hema" + op.ftI_Id + op123.name,
                                            title: op123.title, width: 100
                                        });

                                    })
                                    op.title = op.ftI_Name;
                                })


                                $scope.tempaggary = [];
                                $scope.colarrayall = [{

                                    title: "SLNO",
                                    template: "<span class='row-number'></span>", width: 75

                                },
                                {
                                    name: 'INVMGRN_GRNNo', field: 'INVMGRN_GRNNo', title: 'GRN No', width: 150
                                },
                                {
                                    name: 'GRNDate', field: 'GRNDate', title: 'GRN Date', width: 150, template: "#= kendo.toString(kendo.parseDate(GRNDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                                },
                                  {
                                      name: 'INVMSLRET_SalesReturnNo', field: 'INVMSLRET_SalesReturnNo', title: 'Credit Note No', width: 150
                                    },
                                    {
                                        name: 'INVMSLRET_SalesReturnDate', field: 'INVMSLRET_SalesReturnDate', title: 'CN Date', width: 250, template: "#= kendo.toString(kendo.parseDate(INVMSLRET_SalesReturnDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                                    },
                                {
                                    name: 'INVMSL_SalesNo', field: 'INVMSL_SalesNo', title: 'Sales Invoice No', width: 200
                                },
                                {
                                    name: 'INVMSL_SalesDate', field: 'INVMSL_SalesDate', title: 'Sales Invoice Date', width: 200
                                },
                                    {
                                        name: 'INVMSLRET_ReturnRemarks', field: 'INVMSLRET_ReturnRemarks', title: 'Remarks', width: 200
                                    },
                                    {
                                        name: 'INVMI_HSNCode', field: 'INVMI_HSNCode', title: 'HSN Code', width: 200
                                    },
                                   
                                {
                                    name: 'INVMUOM_UOMName', field: 'INVMUOM_UOMName', title: 'UOM', width: 100
                                    },
                                    {
                                        name: 'INVTSLRET_SalesReturnQty', field: 'INVTSLRET_SalesReturnQty', title: 'Quantity', width: 200
                                    },
                                {
                                    name: 'INVTGRN_Rate', field: 'INVTGRN_Rate', title: 'Rate', width: 150
                                },
                                {
                                    name: 'INVTSLRET_SalesReturnAmount', field: 'INVTSLRET_SalesReturnAmount', title: 'Amount', width: 100
                                    },
                                    {
                                        name: 'INVTSLTRET_TaxAmt', field: 'INVTSLTRET_TaxAmt', title: 'GST', width: 100, format: "{0:n2}", footerTemplate: "Total:",
                                        groupFooterTemplate: "Total: "
                                    },
                                  
                                    {
                                        name: 'INVMSLRET_TotalReturnAmount', field: 'INVMSLRET_TotalReturnAmount', title: 'Total Amount', width: 160, format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"

                                    },
                                   
                               
                                {
                                    name: 'INVSPT_PaymentReference', field: 'INVSPT_PaymentReference', title: 'E-way Bill Number', width: 150
                                }
                                  
                               

                            ];
                          
                           
                            $scope.tempaggary.push(
                                { field: 'INVMSLRET_TotalReturnAmount', name: 'INVMSLRET_TotalReturnAmount', format: "{0:n2}", aggregate: "sum" }
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
                                            fileName: "SalesReturnReport.xlsx",
                                            proxyURL: "",
                                            filterable: true,
                                           
                                             
                                        },
                                    

                                        pdf: {
                                            fileName: "SalesReturnReport.pdf"
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
                            $scope.get_SalesReturnreport = "";
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