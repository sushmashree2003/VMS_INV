(function () {    'use strict';    angular        .module('app')        .controller('Exit_EmployeeController', Exit_EmployeeController)    Exit_EmployeeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter','Excel', '$timeout']    function Exit_EmployeeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, Excel, $timeout) {
        
        $scope.loadData = function () {
            
            var pageid = 2;
            apiService.getURI("Exit_Employee/getalldetails", pageid).then(
                function (promise) {

                    $scope.getalldetails = promise.getalldetails;

                    $scope.fillyear = promise.fillyear;
                    $scope.fillmonth = promise.fillmonth;

                    $scope.hrmE_EmployeeFirstName = promise.hrmE_EmployeeFirstName;
                    $scope.hrmlY_LeaveYear = promise.hrmlY_LeaveYear;
                    $scope.month = promise.month;
                });
        };

        $scope.getchanged = function (yr, mon) {
            $scope.report = false;
            $scope.monthname = "";
            angular.forEach($scope.fillmonth, function (check) {
                if (check.ivrM_Month_Id == mon) {
                    $scope.monthname = check.monthname;
                }
            });
            $scope.yearname = "";
            angular.forEach($scope.fillyear, function (check) {
                if (check.hrmlY_LeaveYear == yr) {
                    $scope.yearname = check.hrmlY_LeaveYear;
                }
            });
        };

        ////$scope.all_checkuu = function (ac) {
        ////    $scope.get_GoodsAtCldetails = [];
        ////    $scope.checkalluu = ac;
        ////    var toggleStatus = $scope.checkalluu;
        ////    angular.forEach($scope.getalldetails, function (itm) {
        ////        itm.hrmE_EmployeeFirstName = toggleStatus;
        ////    });
        ////    if ($scope.checkalluu == true) {
        ////        $scope.loaddata($scope.flag, $scope.getalldetails);
        ////    }
        ////};

        $scope.deptcheck = "";
        $scope.togchkbxuu = function () {
            $scope.deptcheck = $scope.getalldetails.every(function (options) {
                return options.selected;
            });
        }
     
        $scope.isOptionsRequired = function () {

            return !$scope.getalldetails.some(function (options) {
                return options.selected;
            });
        };
        //----------------------------

        $scope.searchchkbx = "";
        $scope.all_checkuu = function () {
            var checkStatus = $scope.checkalluu;
            angular.forEach($scope.getalldetails, function (itm) {
                itm.selected = checkStatus;
            });
        }        $scope.isOptionsRequired = function () {
            return !$scope.getalldetails.some(function (options) {
                return options.selected;
            });
        }        $scope.togchkbx = function () {
            $scope.getalldetails.every(function (options) {
                return options.selected;
            });
        }

        // --------------------- 

        $scope.GetReportt = function () {
            $scope.submitted = true;
            $scope.disablemonth = true;
            $scope.EmpName = [];
            if ($scope.myForm.$valid) {
                if ($scope.allind == 'Month') {
                    $scope.allind = 'Month';
               }
                else {
                    $scope.allind = 'Day';
                }

                $scope.newdate = new Date();
                if ($scope.allind == "Month") {
                    $scope.from_date = new Date($scope.newdate).toDateString();
                    $scope.to_date = new Date($scope.newdate).toDateString();
                }
                else {
                    $scope.from_date = new Date($scope.fromdate).toDateString();
                    $scope.to_date = new Date($scope.todate).toDateString();
                    $scope.year = new Date($scope.fromdate).getFullYear();
                    $scope.month = (new Date($scope.fromdate).getMonth() + 1);
                }
                angular.forEach($scope.getalldetails, function (sts) {
                    if (sts.selected === true) {
                        $scope.EmpName.push({
                            HRME_Id: sts.HRME_Id,
                        });
                    }
                });

                var data = {

                    "EmpName": $scope.EmpName,
                    "Fromdate": $scope.from_date,
                    "Todate": $scope.to_date,
                    "year": $scope.year,
                    "month": $scope.month,
                    "rdopunch": $scope.rdopunch,
                    "type": $scope.allind,
                };
                apiService.create("Exit_Employee/getReport", data).
                    then(function (promise) {
                        $scope.getReport = promise.getReport;

                        $scope.employeeList = [];

                        angular.forEach($scope.getReport, function (dev) {
                            if ($scope.employeeList.length === 0) {
                                $scope.employeeList.push({
                                    HRME_Id: dev.HRME_ID,
                                    HRME_EmployeeCode: dev.HRME_EmployeeCode,
                                    HRME_EmployeeFirstName: dev.HRME_EmployeeFirstName,
                                    HRME_DOJ: dev.HRME_DOJ,
                                });
                            } else if ($scope.employeeList.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.employeeList, function (emp) {
                                    if (emp.HRME_Id === dev.HRME_ID) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.employeeList.push({
                                        HRME_Id: dev.HRME_ID, HRME_EmployeeCode: dev.HRME_EmployeeCode, HRME_EmployeeFirstName: dev.HRME_EmployeeFirstName, HRME_DOJ: dev.HRME_DOJ

                                    });
                                }
                            }
                        });

                        angular.forEach($scope.employeeList, function (ddd) {
                            $scope.templist = [];
                            var sum_deviation = 0;
                            angular.forEach($scope.getReport, function (dd) {
                                if (dd.HRME_ID === ddd.HRME_Id) {
                                    $scope.templist.push(dd);
                                }
                            });
                            ddd.listemp = $scope.templist;
                            //ddd.percentage = (sum_deviation / $scope.templist.length).toFixed(2);
                        });
                    });
            }
            else if ($scope.selectedemptypes.length == 0) {

            }

        }; 

        $scope.isOptionsRequired = function () {
            return !$scope.type.some(function (options) {
                return options.typ;
            });
        };

        $scope.clearid = function () {
            $state.reload();
        };

        $scope.printData = function () {
            var divToPrint = document.getElementById("grd");
            var newWin = window.open("");
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();
        }

        //$scope.exportToExcel = function () {
        //    var divToPrint = document.getElementById("tab1");
        //    var blob = new Blob([divToPrint.outerHTML], {
        //        type: "application/vnd.ms-excel;charset=utf-8"
        //    });
        //    saveAs(blob, "ExitOverallReport.xls");
        //};

        $scope.exportToExcel = function (export_id) {
            var exportHref = Excel.tableToExcel(export_id, 'Exit Overall Report');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };

        //$scope.exportToExcel = function (tableId) {
        //    var exportHref = Excel.tableToExcel(tableId, 'ExitOverallReport');

        //    $timeout(function () { location.href = exportHref; }, 100);
        //    $timeout(function () {
        //        location.href = exportHref;
        //    }, 100);
        //};

        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("p1").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

    }

})();