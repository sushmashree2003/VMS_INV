
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_Purchase_ReportController', INV_Purchase_ReportController);
    INV_Purchase_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter']
    function INV_Purchase_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {


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
        $scope.togchkbxG = function () {
            $scope.grnall = $scope.get_POdetails.every(function (itm) {
                return itm.grnck;
            });
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
                apiService.create("INV_Purchase_Report/onreport", data).
                    then(function (promise) {
                        if (promise.get_POreport.length > 0) {
                            var totalrate = 0.00;
                            var totaltax = 0.00;
                            var totalamt = 0.00;                         
                            $scope.get_POreport = promise.get_POreport;
                            $scope.presentCountgrid = $scope.get_POreport.length;

                            angular.forEach($scope.get_POreport, function (po) {
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
                        }
                        else {
                            swal("No Record Found...!!");
                            $scope.get_POreport = "";
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