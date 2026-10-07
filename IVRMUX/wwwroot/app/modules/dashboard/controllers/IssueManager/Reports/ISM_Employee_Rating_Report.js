(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_Employee_Rating_ReportController', ISM_Employee_Rating_ReportController);
    ISM_Employee_Rating_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter'];
    function ISM_Employee_Rating_ReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter) {


        var paginationformasters;
        $scope.multidepartment = false;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.obj = {};
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();

        $scope.loaddata = function () {
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("ISM_PlannerDeviationReport/getdetails", pageid).then(function (promise) {
                $scope.get_userEmplist = promise.get_userEmplist;
                $scope.empdepartment = promise.empdepartment;
                $scope.empname = promise.empname;
            });
        };

        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.get_userEmplist, function (uem) {
                uem.empck = toggleStatus;
            });
        };
        $scope.togchkbx = function () {
            $scope.checkall = $scope.get_userEmplist.every(function (options) {
                return options.empck;
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.get_userEmplist.some(function (options) {
                return options.empck;
            });
        };
        //*************************** DEVIATION REPORT *********************//        
        $scope.deviationReport = function () {
            if ($scope.myForm.$valid) {
                $scope.arrayuserEmp = [];
                angular.forEach($scope.get_userEmplist, function (em) {
                    if (em.empck === true) {
                        $scope.arrayuserEmp.push(em);
                    }
                });
                var data = {
                    "startdate": new Date($scope.start_Date).toDateString(),
                    "enddate": new Date($scope.end_Date).toDateString(),
                    "arrayuserEmp": $scope.arrayuserEmp
                };
                apiService.create("ISM_PlannerDeviationReport/getEmp_ratingReport", data).then(function (promise) {
                    if (promise.get_emp_rating_report.length > 0) {
                        $scope.get_emp_rating_report = promise.get_emp_rating_report;
                        $scope.employee = $scope.get_emp_rating_report[0].employeename;

                        $scope.employeeid = [];

                        angular.forEach($scope.get_emp_rating_report, function (dev) {
                            if ($scope.employeeid.length === 0) {
                                $scope.employeeid.push({ HRME_Id: dev.HRME_Id, employeename: dev.employeename });
                            } else if ($scope.employeeid.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.employeeid, function (emp) {
                                    if (emp.HRME_Id === dev.HRME_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.employeeid.push({ HRME_Id: dev.HRME_Id, employeename: dev.employeename });
                                }
                            }
                        });
                        var cnt1 = 0;
                        angular.forEach($scope.get_emp_rating_report, function (qq) {
                           
                            if (qq.EffortInHrs > 0 && qq.TimeTakenInHrs != null) {
                                cnt1 += qq.EffortInHrs;
                            }
                            
                        })
                        var cnt2 = 0;
                        angular.forEach($scope.get_emp_rating_report, function (qq) {

                            if (qq.TimeTakenInHrs > 0 ) {
                                cnt2 += qq.TimeTakenInHrs;
                            }

                        })
                        $scope.totaltime = cnt1;
                        $scope.takintime = cnt2;
                        $scope.rating = 100 - (($scope.takintime / $scope.totaltime)*100)
                        console.log($scope.employeeid);

                        angular.forEach($scope.employeeid, function (ddd) {
                            $scope.templist = [];
                            angular.forEach($scope.get_emp_rating_report, function (dd) {
                                if (dd.HRME_Id === ddd.HRME_Id) {
                                    $scope.templist.push(dd);
                                }
                            });
                            ddd.plannerdetails = $scope.templist;
                        });


                        console.log($scope.employeeid);
                    }
                    else {
                        swal("No Record  Found..... !!");
                        $scope.get_emp_rating_report = "";
                    }

                });
            }
            else {
                $scope.submitted = true;
            }
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
        $scope.exportToExcel = function (export_id) {
            var exportHref = Excel.tableToExcel(export_id, 'printDeviation');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.Clear = function () {
            $scope.get_deviationreport = "";
            $scope.get_userEmplist = "";
            $scope.checkall = "";
            $scope.start_Date = "";
            $scope.end_Date = "";
            $scope.loaddata();
        };

    }
})();

