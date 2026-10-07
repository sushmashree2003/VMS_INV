(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_EmployeeBlockReportController', ISM_EmployeeBlockReportController);
    ISM_EmployeeBlockReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function ISM_EmployeeBlockReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache) {


        var paginationformasters;
        $scope.multidepartment = false;
        $scope.blockflg = false;
        $scope.unblockflg = false;
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

        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.itemsPerPage = paginationformasters;
            $scope.searchValueE = "";
            $scope.get_Status = [];
            var pageid = 2;
            apiService.getURI("ISM_TaskRegisterReport/getdetails", pageid).then(function (promise) {
                $scope.get_companyDeptist = promise.get_companyDeptist;
            });
        };

        //***************************** Get Employee list
        $scope.onDeptchange = function (objd) {
            $scope.get_employeelist = [];
            $scope.deptid = objd.HRMD_Id.HRMD_Id;
            var data = {
                "HRMD_Id": $scope.deptid,
                "MIId": objd.HRMD_Id.MI_Id
            };
            apiService.create("ISM_TaskRegisterReport/getemployeelist", data).
                then(function (promise) {
                    if (promise.get_employeelist.length > 0) {
                        $scope.get_employeelist = promise.get_employeelist;
                    }
                    else {
                        $scope.get_employeelist = "";
                        swal("No Record Found ....!!");
                    }

                });
        };
        //************************** Employee List 
        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.get_employeelist, function (uem) {
                uem.empck = toggleStatus;
            });
        };
        $scope.togchkbx = function () {
            $scope.checkall = $scope.get_employeelist.every(function (options) {
                return options.empck;
            });
        };

        //*************************** Employee Block Report *********************//        
        $scope.empblockReport = function () {
            if ($scope.myForm.$valid) {
                $scope.temp_Employee = [];
                $scope.searchValueE = "";
                $scope.get_empblockReport = "";
                angular.forEach($scope.get_employeelist, function (ep) {
                    if (ep.empck === true) {
                        $scope.temp_Employee.push(ep);
                    }
                });
                if ($scope.blockflg === true && $scope.unblockflg === false) {
                    $scope.typeflg = "Block";
                }
                else if ($scope.unblockflg === true && $scope.blockflg === false) {
                    $scope.typeflg = "Unblock";
                }
                else {
                    $scope.typeflg = "Both";
                }

                if ($scope.temp_Employee.length > 0) {
                    $scope.SelectionFlag = "2";
                }
                else {
                    $scope.SelectionFlag = "1";
                }

                var data = {
                    "TypeFlg": $scope.typeflg,
                    "SelectionFlag": $scope.SelectionFlag,
                    "startdate": new Date($scope.start_Date).toDateString(),
                    "enddate": new Date($scope.end_Date).toDateString(),
                    "HRMD_Id": $scope.deptid,
                    "temp_Employee": $scope.temp_Employee
                };
                apiService.create("ISM_EmployeeBlockReport/getempblockReport", data).then(function (promise) {
                    $scope.employeeid = [];
                    if (promise.get_empblockReport.length > 0) {
                        $scope.get_empblockReport = promise.get_empblockReport;
                        angular.forEach($scope.get_empblockReport, function (dev) {
                            if ($scope.employeeid.length === 0) {
                                $scope.employeeid.push({ HRME_Id: dev.HRME_Id, employeename: dev.employeename, HRME_EmployeeCode: dev.HRME_EmployeeCode });
                            } else if ($scope.employeeid.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.employeeid, function (emp) {
                                    if (emp.HRME_Id === dev.HRME_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.employeeid.push({ HRME_Id: dev.HRME_Id, employeename: dev.employeename, HRME_EmployeeCode: dev.HRME_EmployeeCode });
                                }
                            }
                        });

                        angular.forEach($scope.employeeid, function (ddd) {
                            $scope.templist = [];
                            angular.forEach($scope.get_empblockReport, function (dd) {
                                if (dd.HRME_Id === ddd.HRME_Id) {
                                    $scope.templist.push(dd);
                                }
                            });
                            ddd.blockdetails = $scope.templist;
                        });
                    }

                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printEB").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();

        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.Clear = function () {
            $scope.get_empblockReport = "";
            $scope.get_companyDeptist = "";
            $scope.get_employeelist = "";                    
            $scope.checkall = "";
            $scope.start_Date = "";
            $scope.end_Date = "";
            $scope.obj.HRMD_Id.HRMD_Id = "";
            $scope.loaddata();
        };

    }
})();

