(function () {
    'use strict';

    angular
        .module('app')
        .controller('FF_Settlement_ReportProcessController', FF_Settlement_ReportProcessController);

    FF_Settlement_ReportProcessController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter', '$q']

    function FF_Settlement_ReportProcessController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter, $q) {

        $scope.takescreenshot = false;
        $scope.hrcommentflag = false;
        $scope.disablemonth = false;
        $scope.ddate = new Date();

        $scope.submitted = false;

        $scope.loadData = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;

            $scope.all_checkC = function () {
                var checkStatus = $scope.hrmE_Id;
                var count = 0;
                angular.forEach($scope.employees, function (itm) {
                    itm.selected = checkStatus;
                    if (itm.selected == true) {
                        count += 1;
                    }
                    else {
                        count = 0;
                    }
                });

            }

            $scope.all_checkC4 = function () {
                var checkStatus = $scope.ivrM_Month_Id;
                var count = 0;
                angular.forEach($scope.fillmonth, function (itm) {
                    itm.selected = checkStatus;
                    if (itm.selected == true) {
                        count += 1;
                    }
                    else {
                        count = 0;
                    }
                });

            }
            $scope.isOptionsRequired = function () {
                return !$scope.employees.some(function (options) {
                    return options.selected;
                });


            }



            $scope.isOptionsRequired4 = function () {
                return !$scope.fillmonth.some(function (options) {
                    return options.selected;
                });
            }

            $scope.togchkbxC = function () {

                $scope.ivrM_Month_Id = $scope.fillmonth.every(function (options) {
                    return options.selected;
                });
            }

            $scope.togchkbxC1 = function () {

                $scope.hrmE_Id = $scope.employees.every(function (options) {
                    return options.selected;
                });
            }

            apiService.getURI("FF_Settlement_Process/onloaddata", pageid).then(function (promise) {
                $scope.fillmonth = promise.fillmonth;
                $scope.fillyear = promise.fillyear;
                $scope.employees = promise.employees;
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clear_Details = function () {
            $state.reload();
        };

        $scope.ShowReportdata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

               
                var HRMEID = [];

                angular.forEach($scope.employees, function (ty) {
                    if (ty.selected) {
                        HRMEID.push({ HRME_Id: ty.hrmE_Id });
                    }
                })

                if ($scope.allind == "datewise") {
                    $scope.fromdate = new Date($scope.fromdate).toDateString();
                    $scope.todate = new Date($scope.todate).toDateString();
                    $scope.ASMAY_Id = 0;
                }

                var data = {
                    "HRMEID": HRMEID,
                  
                    "Fromdate": $scope.fromdate,
                    "Todate": $scope.todate,
                    "rdbbutton": $scope.allind,
                    "Status_Type": $scope.status
                };


                apiService.create("FF_Settlement_Process/getapprovalreport", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.approvalReport = promise.approvalReport;
                        if ($scope.approvalReport !== null && $scope.approvalReport.length > 0) {
                            $scope.approvaldetails = $scope.approvalReport;

                            console.log($scope.approvaldetails);


                            $scope.certificaterequestid = [];

                            angular.forEach($scope.approvaldetails, function (dev) {
                                if ($scope.certificaterequestid.length === 0) {
                                    $scope.certificaterequestid.push({
                                        HREFFS_Id: dev.HREFFS_Id,
                                        EMPLOYEENAME: dev.Employee_Name,
                                        REQDATE: dev.HREFFS_EntryDate,
                                        HREFFS_Remarks: dev.HREFFS_Remarks,
                                        HREFFS_ModeOfPayment: dev.HREFFS_ModeOfPayment,
                                        HREFFS_AdvYear: dev.HREFFS_AdvYear,
                                        HREFFS_AdvMonth: dev.HREFFS_AdvMonth,
                                        HREFFSA_Date: dev.HREFFSA_Date,
                                        HREFFS_AppliedAmount: dev.HREFFS_AppliedAmount,
                                        HREFFS_AdvStatus: dev.HREFFS_AdvStatus,
                                        HREFFSA_Status: dev.HREFFSA_Status,

                                    });
                                } else if ($scope.certificaterequestid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.certificaterequestid, function (emp) {
                                        if (emp.HREFFS_Id === dev.HREFFS_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.certificaterequestid.push({
                                            HREFFS_Id: dev.HREFFS_Id,
                                            EMPLOYEENAME: dev.Employee_Name,
                                            REQDATE: dev.HREFFS_EntryDate,
                                            HREFFS_Remarks: dev.HREFFS_Remarks,
                                            HREFFS_ModeOfPayment: dev.HREFFS_ModeOfPayment,
                                            HREFFS_AdvYear: dev.HREFFS_AdvYear,
                                            HREFFS_AdvMonth: dev.HREFFS_AdvMonth,
                                            HREFFSA_Date: dev.HREFFSA_Date,
                                            HREFFS_AppliedAmount: dev.HREFFS_AppliedAmount,
                                            HREFFS_AdvStatus: dev.HREFFS_AdvStatus,
                                            HREFFSA_Status: dev.HREFFSA_Status,
                                        });
                                    }
                                }
                            });

                            console.log($scope.certificaterequestid);

                            angular.forEach($scope.certificaterequestid, function (ddd) {
                                $scope.templist = [];
                                angular.forEach($scope.approvaldetails, function (dd) {
                                    if (dd.HREFFS_Id === ddd.HREFFS_Id) {
                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.templistdata = $scope.templist;
                            });

                            console.log($scope.certificaterequestid);

                        }

                        else {
                            swal("No Records Found");
                            $scope.approvaldetails = []
                        }
                    }

                });
            }

        };

        $scope.Print = function () {

            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExceldetails = function () {

            var excelname = 'Full & Final Settlement Approval Report';
            excelname = excelname.toUpperCase() + '.xls';
            var exportHref = Excel.tableToExcel('#exceltopd', 'Full & Final Settlement Approval Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

    }
})();


