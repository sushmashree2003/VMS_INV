(function () {
    'use strict';
    angular
        .module('app')
        .controller('VMS_CandiateMeeting_JoinController', VMS_CandiateMeeting_JoinController);

    VMS_CandiateMeeting_JoinController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache'];
    function VMS_CandiateMeeting_JoinController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object

        $scope.ttt = true;
        $scope.hostname = "";
        $scope.roomName = "";
        $scope.meetid = 0;
        $scope.showstaff = true;
        $scope.clearallsettings = function () {
            $state.reload();
        };

        //Employeee
        $scope.currentPage = 1;
        $scope.currentPage3 = 1;
        $scope.itemsPerPage = 5;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 5;
        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;
        $scope.itemsPerPage3 = 10;
        $scope.page1 = "page1";
        $scope.page2 = "page2";
        $scope.page3 = "page3";
        $scope.page4 = "page4";
        $scope.LMSLMEET_PlannedDate = new (Date);
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getDATA("VMS_Live_Interview/getempdetails_join").then(function (promise) {
                $scope.meetinglist = promise.meetinglist;
                $scope.vcflag = promise.vcflag;
                if (promise.meetingstart != null || promise.meetingstart > 0) {
                    $scope.meetingstart = promise.meetingstart;
                }

            });
        };



        $scope.type = '';
        $scope.meetingst = false;
        $scope.onstartmeeting = function (user) {
            $scope.topic = user.VMSLMEET_MeetingTopic;
            $scope.meetingst = true;
            $scope.meetid = user.VMSLMEET_MeetingId;

            var data = {
                "VMSLITVW_Id": user.VMSLITVW_Id,
                "VMSLMEET_MeetingId": user.VMSLMEET_MeetingId,
                "VMSLMEET_MeetingTopic": user.VMSLMEET_MeetingTopic,
                "HRCD_Id": user.HRCD_Id
            }
            apiService.create("VMS_Live_Interview/onstartmeeting_join", data).then(function (promise) {
                if ($scope.vcflag === "BBB") {
                    $scope.murl = promise.moderatorurl;
                    window.open($scope.murl, "_self");
                };
            });
        };
        $scope.stopstreeming = function (user) {
            debugger;
            var data = {
                "VMSLITVW_Id": user.VMSLITVW_Id,
                "HRCD_Id": user.HRCD_Id
            };

            $scope.ttt = false;
            apiService.create("VMS_Live_Interview/endmainmeeting_join", data).then(function (promise) {
                if (promise.returnval == true) {
                    swal('Meeting Completed')
                    $state.reload();
                }

            });
        }





    }


})();