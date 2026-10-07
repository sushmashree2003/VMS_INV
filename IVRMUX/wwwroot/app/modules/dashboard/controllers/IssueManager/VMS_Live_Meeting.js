


(function () {
    'use strict';
    angular
        .module('app')
        .controller('VMS_Live_MeetingController', VMS_Live_MeetingController)

    VMS_Live_MeetingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache']
    function VMS_Live_MeetingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache) {

        //Date:23-12-2016 for displaying privileges.
       
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }


        $scope.presentCountgrid1 = 0;
        $scope.presentCountgrid2 = 0;
        $scope.presentCountgrid3 = 0;
        $scope.presentCountgrid4 = 0;
        $scope.filterValue = "";

       
        $scope.BindData = function () {
            $scope.currentPage4 = 1;
            $scope.itemsPerPage4 = 10;
            apiService.getDATA("VMS_Live_Meeting/Getdetails").
                then(function (promise) {
                    $scope.get_userEmplist = promise.get_userEmplist;
                    if (promise.vmslivemeetinglist != null || promise.vmslivemeetinglist > 0) {
                        $scope.vmslivemeetinglist = promise.vmslivemeetinglist;
                    }
                    
                })

        };

        //======== save meeting==========

        $scope.savemeeting = function () {
            if ($scope.myForm.$valid) {
                if ($scope.hrs === '12hr') {
                    $scope.ScheduleTime = $scope.ScheduleTime_12;
                    $scope.ScheduleTimeTo = $scope.ScheduleTimeTo_12;
                }
                else if ($scope.hrs === '24hr') {
                    $scope.ScheduleTime = $scope.ScheduleTime_24;
                    $scope.ScheduleTimeTo = $scope.ScheduleTimeTo_24;
                }
                $scope.ScheduleTime = $filter('date')($scope.ScheduleTime, "HH:mm");
                $scope.ScheduleTimeTo = $filter('date')($scope.ScheduleTimeTo, "HH:mm");
                $scope.arrayuserEmp = [];
                angular.forEach($scope.get_userEmplist, function (em) {
                    if (em.empck === true) {
                        $scope.arrayuserEmp.push({ HRME_Id: em.HRME_Id });
                    }
                });

                $scope.ddate = $filter('date')($scope.VMSLMEET_PlannedDate,'yyyy-MM-dd')
                swal({
                    title: "Are you sure?",
                    text: "Do you want to Schedule Meeting ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {

                            if ($scope.myForm.$valid) {

                                var data = {
                                    "VMSLMEET_MeetingTopic": $scope.VMSLMEET_MeetingTopic,
                                    "VMSLMEET_PlannedDate": $scope.ddate,
                                    "VMSLMEET_PlannedStartTime": $scope.ScheduleTime,
                                    "VMSLMEET_PlannedEndTime": $scope.ScheduleTimeTo,
                                    "arrayuserEmp": $scope.arrayuserEmp,
                                }
                                apiService.create("VMS_Live_Meeting/savemeeting", data).then(function (promise) {
                                    if (promise.message == 'Add') {
                                        swal('Schedule Created Successfully.')
                                    }
                                    else if (promise.message == 'Update') {
                                        swal('Schedule Updated Successfully.')
                                    }
                                    else if (promise.message == 'Error') {
                                        swal('Operation Failed..!!')
                                    }
                                    $state.reload();
                                })
                            }
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }

        };

        //=================
        $scope.isOptionsRequired = function () {
            return !$scope.get_userEmplist.some(function (options) {
                return options.empck;
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

        $scope.validateTomintime_12 = function (timedata) {

            $scope.ScheduleTimeTo_12 = "";
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;

            $scope.min.setMinutes(hh);
            $scope.min.setMinutes(mm);
        }
        $scope.validateTomintime_24 = function (timedata) {

            $scope.ScheduleTimeTo_24 = "";
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;

            $scope.min.setMinutes(hh);
            $scope.min.setMinutes(mm);
        }

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

    }

})();

