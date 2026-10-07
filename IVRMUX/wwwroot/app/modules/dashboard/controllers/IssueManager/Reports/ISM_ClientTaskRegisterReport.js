(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_TaskRegisterReportController', ISM_TaskRegisterReportController);
    ISM_TaskRegisterReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function ISM_TaskRegisterReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache) {


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

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.get_clientcomptasklist = [];
            $scope.totaltask = 0;
            $scope.pendingcount = 0;
            $scope.search = "";
            var data = {};
            apiService.create("ISM_Client_ComplaintsTaskList/getdetails", data).then(function (promise) {
                $scope.get_Status = [{ id: 'Open', value: 'Open' }, { id: 'Inprogress', value: 'Inprogress' }, { id: 'Completed', value: 'Completed' }, { id: 'Development Completed', value: 'Development Completed' }, { id: 'Deployement Completed in test link', value: 'Deployement Completed in test link' }, { id: 'Deployement Completed in Live link', value: 'Deployement Completed in Live link' }, { id: 'Close', value: 'Close' }];
                $scope.get_client = promise.get_client;
            });
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;
                $scope.reverse = !$scope.reverse;
            };
        };

        //====================================== Check Box Selection
        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.get_client, function (uem) {
                uem.cltck = toggleStatus;
            });
        };
        $scope.togchkbx = function () {
            $scope.checkall = $scope.get_client.every(function (options) {
                return options.cltck;
            });
        };
        $scope.isOptionsRequired = function () {
            return !$scope.get_client.some(function (options) {
                return options.cltck;
            });
        };
        $scope.all_checkS = function (ack) {
            $scope.checkallS = ack;
            var toggleStatus = $scope.checkallS;
            angular.forEach($scope.get_Status, function (sts) {
                sts.stck = toggleStatus;
            });
        };
        $scope.togchkbxS = function () {
            $scope.checkallS = $scope.get_Status.every(function (options) {
                return options.stck;
            });
        };

        $scope.onchangeReportType = function () {
            $scope.get_taskregisterReport = [];
        };
        //*************************** Client Task Register Report *********************//        
        $scope.taskReport = function () {
            if ($scope.myForm.$valid) {
                $scope.arrayStatus = [];
                $scope.priorityarray = [];
                $scope.arrayClient = [];
                $scope.get_taskregisterReport = "";
                angular.forEach($scope.get_Status, function (sts) {
                    if (sts.stck === true) {
                        $scope.arrayStatus.push(sts);
                    }
                });
                angular.forEach($scope.get_client, function (clt) {
                    if (clt.cltck === true) {
                        $scope.arrayClient.push(clt);
                    }
                });
                var data = {
                    "TypeFlg": $scope.reporttype,
                    "startdate": new Date($scope.start_Date).toDateString(),
                    "enddate": new Date($scope.end_Date).toDateString(),
                    "arrayClient": $scope.arrayClient,
                    "arrayStatus": $scope.arrayStatus,
                    "priorityarray": $scope.priorityarray
                };
                apiService.create("ISM_TaskRegisterReport/gettaskregisterReport", data).then(function (promise) {
                    $scope.get_taskregisterReport = promise.get_taskregisterReport;
                    $scope.clientid = [];
                    if ($scope.get_taskregisterReport.length > 0) {
                        angular.forEach($scope.get_taskregisterReport, function (dev) {
                            if ($scope.clientid.length === 0) {
                                $scope.clientid.push({ ISMMCLT_Id: dev.ISMMCLT_Id, ISMMCLT_ClientName: dev.ISMMCLT_ClientName });
                            } else if ($scope.clientid.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.clientid, function (emp) {
                                    if (emp.ISMMCLT_Id === dev.ISMMCLT_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.clientid.push({ ISMMCLT_Id: dev.ISMMCLT_Id, ISMMCLT_ClientName: dev.ISMMCLT_ClientName });
                                }
                            }
                        });
                        console.log($scope.clientid);
                        angular.forEach($scope.clientid, function (ddd) {
                            $scope.templist = [];
                            angular.forEach($scope.get_taskregisterReport, function (dd) {
                                if (dd.ISMMCLT_Id === ddd.ISMMCLT_Id) {
                                    $scope.templist.push(dd);
                                }
                            });
                            ddd.taskdetails = $scope.templist;
                        });
                    }
                    else {
                        swal("No Record Found.... !!");
                        $scope.get_taskregisterReport = "";
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printCTR").innerHTML;
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
            $scope.get_taskregisterReport = "";
            $scope.get_Status = "";
            $scope.checkallS = "";
            $scope.checkall = "";
            $scope.start_Date = "";
            $scope.end_Date = "";
            $scope.loaddata();
        };

    }
})();

