(function () {
    'use strict';
    angular.module('app').controller('ISM_NotificationController', ISM_NotificationController);
    ISM_NotificationController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$window'];
    function ISM_NotificationController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $window) {


        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        $scope.submitted = false;
        $scope.ismtcR_CreationDate = new Date();
        $scope.obj = {};

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("ISM_Notification/getdetails", pageid).then(function (promise) {
                $scope.get_notifications = promise.get_notifications;
                if ($scope.get_notifications.length > 0) {
                    $scope.totalnotification = $scope.get_notifications.length;
                }
                else {
                    $scope.totalnotification = 0;
                }
            });
        };
        //==============================  View Notification
        var HostName = location.host;
        $scope.Viewnotification = function (objn) {
            $scope.notificationType = objn.ISMPN_Type;
            var data = {
                "ISMPN_Id": objn.ISMPN_Id,
                "ISM_CommonId": objn.ISM_CommonId,
                "ISMPN_Type": objn.ISMPN_Type
            };
            apiService.create("ISM_Notification/getnotificationdetails", data).then(function (promise) {
                $('#myModalNotification').modal('show');
                if ($scope.notificationType === "DR") {
                    //********************************* DailyReport 
                    $scope.getemployeedetails = promise.getemployeedetails;
                    $scope.get_dailyreportdetails = promise.get_dailyreportdetails;
                    $scope.get_dailyreportdeviation = promise.get_dailyreportdeviation;
                    if ($scope.get_dailyreportdeviation.length > 0) {
                        $scope.completedper = $scope.get_dailyreportdeviation[0].per;
                        $scope.deviationper = $scope.get_dailyreportdeviation[0].deviation;
                    } else {
                        $scope.completedper = 0;
                        $scope.deviationper = 100;
                    }
                    $scope.drdate = promise.drdate;
                    $scope.employeename = $scope.getemployeedetails[0].employeename;
                    $scope.department = $scope.getemployeedetails[0].department;
                    $scope.designation = $scope.getemployeedetails[0].desgination;
                }
                else if ($scope.notificationType === "PL") {
                    //********************************* Planner Details 
                    $scope.get_plannerdata = promise.get_plannerdata;
                    $scope.get_plannerdetails = promise.get_plannerdetails;

                    $scope.planstartdate = $scope.get_plannerdata[0].ismtpL_StartDate;
                    $scope.planenddate = $scope.get_plannerdata[0].ismtpL_EndDate;
                    $scope.plannername = $scope.get_plannerdata[0].ismtpL_PlannerName;
                    $scope.plannedby = $scope.get_plannerdetails[0].assignedby;
                    $scope.plannereffort = $scope.get_plannerdetails[0].sL_TotalHrs;
                }
                else {
                    //********************************* Task Details                         
                    $scope.get_taskdetails = promise.get_plannerdetails;
                }
            });


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

