(function () {
    'use strict';
    angular
        .module('app')
        .controller('Dept_HeadWiseLeaveReportController', Dept_HeadWiseLeaveReportController)

    Dept_HeadWiseLeaveReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function Dept_HeadWiseLeaveReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {


        $scope.onLoadGetData = function () {
            
            var today = new Date();
            $scope.gettodate = today;
            
            var format = $filter('date')(today, "dd/MM/yyyy");
            var day = parseInt(format.substring(0, 2));            var month = parseInt(format.substring(3, 5));            var year = parseInt(format.substring(6, 10));            var date = new Date(year,month-2,day);
            $scope.getfromdate = date;
            
            $scope.getpresentabsentdetails();

        }
        
        $scope.getpresentabsentdetails = function () {

            var data = {
                "GETFROMDATE": new Date($scope.getfromdate).toDateString(),
                "GETTODATE": new Date($scope.gettodate).toDateString()
            };

            apiService.create("Dept_HeadWiseLeaveReport/getdatedetails", data).then(function (promise) {

                $scope.ProceduralArray = promise.getReport;

                angular.forEach($scope.ProceduralArray, function (grdobj) {
                    if (grdobj.RoleType == "ADMIN") {

                        $scope.GetReport = promise.getReport;

                        $scope.employeeid = [];

                        angular.forEach($scope.GetReport, function (dev) {
                            if ($scope.employeeid.length === 0) {
                                $scope.employeeid.push({
                                    HRME_Id: dev.HRME_Id,
                                    MI_Name: dev.MI_Name,
                                    HRMD_DepartmentName: dev.HRMD_DepartmentName,
                                    EmployeeName: dev.EmployeeName
                                });
                            } else if ($scope.employeeid.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.employeeid, function (emp) {
                                    if (emp.HRME_Id === dev.HRME_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.employeeid.push({
                                        HRME_Id: dev.HRME_Id,
                                        MI_Name: dev.MI_Name,
                                        HRMD_DepartmentName: dev.HRMD_DepartmentName,
                                        EmployeeName: dev.EmployeeName
                                    });
                                }
                            }
                        });

                        angular.forEach($scope.employeeid, function (ddd) {
                            $scope.templist = [];
                            angular.forEach($scope.GetReport, function (dd) {
                                if (dd.HRME_Id === ddd.HRME_Id) {
                                    $scope.templist.push(dd);
                                }
                            });
                            ddd.plannerdetails = $scope.templist;
                        });


                    }

                    else if (grdobj.RoleType == "DEPTHEAD" ) {
                        $scope.department = promise.getReport;


                        $scope.employeeid1 = [];

                        angular.forEach($scope.department, function (dev) {
                            if ($scope.employeeid1.length === 0) {
                                $scope.employeeid1.push({
                                    HRME_Id: dev.HRME_Id,
                                    EmployeeName: dev.EmployeeName,
                                    HRML_LeaveName: dev.HRML_LeaveName,
                                });
                            } else if ($scope.employeeid1.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.employeeid1, function (emp) {
                                    if (emp.HRME_Id === dev.HRME_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.employeeid1.push({
                                        HRME_Id: dev.HRME_Id,
                                        EmployeeName: dev.EmployeeName,
                                        HRML_LeaveName: dev.HRML_LeaveName,
                                    });
                                }
                            }
                        });

                        angular.forEach($scope.employeeid1, function (ddd) {
                            $scope.templist1 = [];
                            angular.forEach($scope.department, function (dd) {
                                if (dd.HRME_Id === ddd.HRME_Id) {
                                    $scope.templist1.push(dd);
                                }
                            });
                            ddd.plannerdetails1 = $scope.templist1;
                        });

                    }

                    else {
                        $scope.employee = promise.getReport;
                    }
                });
                
            });


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
            var exportHref = Excel.tableToExcel(export_id, 'Salary Approval Report');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };


        $scope.printData1 = function () {
            var innerContents = "";
            innerContents = document.getElementById("printDeviation1").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExcel1 = function (export_id) {
            var exportHref = Excel.tableToExcel(export_id, 'Salary Approval Report');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };


        $scope.printData2 = function () {
            var innerContents = "";
            innerContents = document.getElementById("printDeviation2").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExcel2 = function (export_id) {
            var exportHref = Excel.tableToExcel(export_id, 'Salary Approval Report');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };

    }
})();

