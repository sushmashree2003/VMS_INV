(function () {
    'use strict';
    angular
        .module('app')
        .controller('SalaryApprovalReportController', SalaryApprovalReportController)

    SalaryApprovalReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout',]
    function SalaryApprovalReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, Excel, $timeout, ) {

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("SalaryApprovalReport/getalldetails", pageid).then(function (promise) {

                $scope.getmonth = promise.monthdropdown;
                $scope.getyear = promise.leaveyeardropdown;
                $scope.institutionlist = promise.companydetails;
                $scope.getemployee = promise.getemployee;

                $scope.individualflag = false;
                $scope.usercheck = true;
                $scope.all_check();
            });
        };

        //cancel
        $scope.cleardata = function () {
            $state.reload();
        };

        // selecting all
        $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.institutionlist, function (itm) {
                itm.select = checkStatus;
            });
        }

        $scope.isOptionsRequired = function () {
            return !$scope.institutionlist.some(function (options) {
                return options.select;
            });
        }

        $scope.togchkbx = function () {
            $scope.institutionlist.every(function (options) {
                return options.select;
            });
        }
        $scope.selectemployee = function () {
            if ($scope.individualflag == false || $scope.individualflag == undefined) {
                $scope.hrmE_Id = "";
                $scope.individualflag == false;
            }
            else {
                $scope.individualflag = true;
            }
        }

        $scope.getYear = function () {
            $scope.employeeid = [];
            $scope.GetReport = [];
        }

        //to get report

        $scope.submitted = false;

        $scope.report = function () {
            $scope.griddata = [];
            $scope.submitted = true;
            $scope.selected_Inst = [];

            

            if ($scope.individualflag === false) {

                angular.forEach($scope.institutionlist, function (mm) {
                    if (mm.select) {
                        $scope.selected_Inst.push(mm);
                    }
                })
                var data = {

                    "HRES_Month": $scope.IVRM_Month_Name,
                    "HRES_Year": $scope.HRMLY_LeaveYear,
                    "selected_Inst": $scope.selected_Inst,
                    "radiotype": $scope.radiotype
                }

                apiService.create("SalaryApprovalReport/report", data).
                    then(function (promise) {
                        if (promise.getReport.length > 0 && promise.getReport != null) {
                            $scope.GetReport = promise.getReport;

                            $scope.employeeid = [];

                            angular.forEach($scope.GetReport, function (dev) {
                                if ($scope.employeeid.length === 0) {
                                    $scope.employeeid.push({
                                        HRME_Id: dev.HRME_ID,
                                        HRME_EmployeeFirstName: dev.HRME_EmployeeFirstName
                                    });
                                } else if ($scope.employeeid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.employeeid, function (emp) {
                                        if (emp.HRME_Id === dev.HRME_ID) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.employeeid.push({
                                            HRME_Id: dev.HRME_ID, HRME_EmployeeFirstName: dev.HRME_EmployeeFirstName

                                        });
                                    }
                                }
                            });

                            angular.forEach($scope.employeeid, function (ddd) {
                                $scope.templist = [];
                                angular.forEach($scope.GetReport, function (dd) {
                                    if (dd.HRME_ID === ddd.HRME_Id) {
                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.plannerdetails = $scope.templist;
                            });


                            $scope.Cumureport = true;
                            $scope.Cumureport1 = false;
                            $scope.screport = true;
                            $scope.export = true;


                        }
                        else {
                            swal("No Record  Found..... !!");
                            $scope.employeeid = [];
                            $scope.GetReport = [];
                            $scope.screport = false;
                            $scope.export = false;
                            $scope.Cumureport = false;
                            $scope.Cumureport1 = false;
                        }
                    })
            }
            else {
                angular.forEach($scope.institutionlist, function (mm) {

                    $scope.selected_Inst.push(mm);

                })
                var data = {
                    "HRES_Month": "",
                    "HRES_Year": "",
                    "selected_Inst": $scope.selected_Inst,
                    "radiotype": "",
                    "HRME_Id": $scope.hrmE_Id.hrmE_Id
                }

                apiService.create("SalaryApprovalReport/report", data).
                    then(function (promise) {
                        if (promise.getReport.length > 0 && promise.getReport != null) {
                            $scope.GetReport = promise.getReport;

                            $scope.employee = [];

                            angular.forEach($scope.GetReport, function (dev) {
                                if ($scope.employee.length === 0) {
                                    $scope.employee.push({
                                        HRES_Year: dev.HRES_Year,
                                        HRES_Month: dev.HRES_Month,
                                    });
                                } else if ($scope.employee.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.employee, function (emp) {
                                        if (emp.HRES_Month === dev.HRES_Month && emp.HRES_Year === dev.HRES_Year) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.employee.push({
                                            HRES_Year: dev.HRES_Year, HRES_Month: dev.HRES_Month

                                        });
                                    }
                                }
                            });

                            angular.forEach($scope.employee, function (ddd) {
                                $scope.templist = [];
                                angular.forEach($scope.GetReport, function (dd) {
                                    if (dd.HRES_Month === ddd.HRES_Month && dd.HRES_Year === ddd.HRES_Year) {
                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.plannerdetails = $scope.templist;
                                //ddd.percentage = (sum_deviation / $scope.templist.length).toFixed(2);
                            });


                            $scope.Cumureport = true;
                            $scope.Cumureport1 = false;
                            $scope.screport = true;
                            $scope.export = true;


                        }
                        else {
                            swal("No Record  Found..... !!");

                            $scope.screport = false;
                            $scope.export = false;
                            $scope.Cumureport = false;
                            $scope.Cumureport1 = false;
                        }
                    })
            }

            $scope.institutionlist = promise.institutionlist;
            $scope.month_list = promise.month_list;

        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printDeviation").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        //$scope.exportToExcel = function (export_id) {
        //    var exportHref = Excel.tableToExcel(export_id, 'Salary Approval Report');
        //    $timeout(function () {
        //        location.href = exportHref;
        //    }, 100);
        //};


        $scope.exportToExcel = function (tableId) {

            //var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            var exportHref = Excel.tableToExcel(tableId, 'SalaryReport');

            $timeout(function () { location.href = exportHref; }, 100);
            $timeout(function () {
                location.href = exportHref;
            }, 100);



        };


    }
})();