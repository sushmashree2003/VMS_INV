(function () {
    'use strict';
    angular.module('app').controller('ISM_ExtensionController', ISM_ExtensionController);
    ISM_ExtensionController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter'];
    function ISM_ExtensionController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.reverse3 = true;
        $scope.page3 = "page3";
        $scope.currentPage3 = 1;
        $scope.itemsPerPage3 = 10

        $scope.submitted = false;
        $scope.dflg = false;
        $scope.extensionDate = new Date();
        $scope.obj = {};
        $scope.roleflag = false;
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.currentPageP = 1;
            $scope.page1 = "page1";
            $scope.page2 = "page2";
            $scope.itemsPerPage = 10;
            $scope.itemsPerPagep = 10;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("ISM_Extension/getdata", pageid).then(function (promise) {
                $scope.roleflag = promise.notifi_chk;
                $scope.get_Emplist = promise.get_Emplist;
                $scope.month_list = promise.get_month;
                $scope.tADA_Extension = promise.tADA_Extension;
                $scope.tAADA_Extension = promise.tAADA_Extension;
                $scope.presentCountgridP = $scope.get_PlannerExtension.length;


            });
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;
                $scope.reverse = !$scope.reverse;
            };
            $scope.sortBy = function (keyname) {
                $scope.sortKey3 = keyname;   //set the sortKey to the param passed
                $scope.reverse3 = !$scope.reverse3; //if true make it false and vice versa
            }
        };
        //======================================== On Radio Change
        $scope.radiochange = function () {
            $scope.dflg = false;
            $scope.obj.ismodE_Noofdays = 0;
            $scope.start_Date = "";
            $scope.end_Date = "";
            $scope.extensionReason = "";
            $scope.HRME_Id.hrmE_Id = "";
        };
        //======================================== On End date Change
        $scope.enddatechange = function () {
            $scope.dflg = true;
            var fdate = new Date($scope.obj.start_Date);
            var tdate = new Date($scope.obj.end_Date);
            var dayDif = (tdate - fdate) / 1000 / 60 / 60 / 24 + 1;
            $scope.dayDiff = dayDif;
        };
        //======================================== On Employee Change
        $scope.onEmpchange = function () {
            $scope.get_month = $scope.month_list;
            $scope.totalotherdays = 0;
        };

        $scope.obj.notifi_chk = true;
        $scope.c1 = true;
        $scope.c2 = true;

        //======================================== On Month Change
        $scope.onmonthchange = function () {
            if ($scope.extensiontype === "TAADA") {
                $scope.dflg = true;
            }
            else {
                $scope.dflg = false;
            }
            var data = {
                "HRME_Id": $scope.obj.HRME_Id.HRME_Id,
                "IVRM_Month_Id": $scope.obj.ivrM_Month_Id
            };
            apiService.create("ISM_Extension/get_details", data).
                then(function (promise) {
                    $scope.get_totalotherdays = promise.get_totalotherdays;
                    $scope.totalotherdays = $scope.get_totalotherdays.length;
                });
        };

        $scope.clicktabs = function () {
            $scope.searchValueP = '';
            $scope.searchValueD = '';
        };

        $scope.save = function () {

            $scope.submitted = true;

            if ($scope.myForm.$valid) {

                var data = {};
                var dystring = "Approve";
                var message = "";
                var emplname = $scope.obj.HRME_Id.employeeName;
                if ($scope.extensiontype === "TAADA") {
                    data = {
                        //"IVRM_Month_Id": parseInt($scope.obj.ivrM_Month_Id),
                        //"ismodE_Noofdays": $scope.obj.ismodE_Noofdays,
                        "ExtensionType": $scope.extensiontype,
                        "HRME_Id": $scope.obj.HRME_Id.HRME_Id,
                        "TADAPLE_FromDate": new Date($scope.obj.start_Date).toDateString(),
                        "TADAPLE_ToDate": new Date($scope.obj.end_Date).toDateString(),
                        "TADAPLE_Reason": $scope.obj.extensionReason,
                        "notifi_chk": $scope.obj.notifi_chk,
                        "email_chk": $scope.obj.email_chk
                    };
                    var noofday = parseInt($scope.obj.ismodE_Noofdays);
                    message = "Do You Want To Approve " + noofday + " Day,\n TADA Extension to " + emplname + " ....?";
                }

                else {
                    data = {
                        "ExtensionType": $scope.extensiontype,
                        "HRME_Id": $scope.obj.HRME_Id.HRME_Id,
                        "TADAPLE_FromDate": new Date($scope.obj.start_Date).toDateString(),
                        "TADAPLE_ToDate": new Date($scope.obj.end_Date).toDateString(),
                        "TADAPLE_Reason": $scope.obj.extensionReason,
                        "notifi_chk": $scope.obj.notifi_chk,
                        "email_chk": $scope.obj.email_chk,
                    };
                    var fdate = new Date($scope.obj.start_Date).toDateString();
                    var tdate = new Date($scope.obj.end_Date).toDateString();
                    message = "Do You Want To Provide TADA  Extension from " + fdate + " To " + tdate + " for " + emplname + " ....?";
                }
                swal({
                    title: "Are you sure?",
                    text: message,
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " ",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("ISM_Extension/savedetails", data).then(function (promise) {

                                if (promise.returnval === true) {
                                    if ($scope.extensiontype === "TAADA") {
                                        if (promise.ismodE_Id === 0 || promise.ismodE_Id < 0) {
                                            swal('Advance TADA Extension Approved successfully ... !!');
                                            $state.reload();
                                        }
                                        else if (promise.ismodE_Id > 0) {
                                            swal('Extension updated successfully');
                                            $state.reload();
                                        }
                                    }
                                    else {
                                        if (promise.ismplE_Id === 0 || promise.ismplE_Id < 0) {
                                            swal('TADA Approved successfully ... !!');
                                            $state.reload();
                                        }
                                        else if (promise.ismplE_Id > 0) {
                                            swal('Extension updated successfully');
                                            $state.reload();
                                        }
                                    }
                                }
                                else if (promise.returnduplicatestatus === 'Duplicate') {
                                    swal('Record already exist');
                                }
                                else {
                                    if ($scope.extensiontype === "TAADA") {
                                        if (promise.ismodE_Id === 0 || promise.ismodE_Id < 0) {
                                            swal('Failed to Approved, please contact administrator');
                                        }
                                        else if (promise.ismodE_Id > 0) {
                                            swal('Failed to update, please contact administrator');
                                        }
                                    }
                                    else {
                                        if (promise.ismplE_Id === 0 || promise.ismplE_Id < 0) {
                                            swal('Failed to Approved, please contact administrator');
                                        }
                                        else if (promise.ismplE_Id > 0) {
                                            swal('Failed to update, please contact administrator');
                                        }
                                    }
                                }
                            });
                        }
                        else {
                            swal("Extension " + dystring + " Cancelled!!!");
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        //===========Clear Field
        $scope.cancel = function () {
            $state.reload();
        };

    }
})();

