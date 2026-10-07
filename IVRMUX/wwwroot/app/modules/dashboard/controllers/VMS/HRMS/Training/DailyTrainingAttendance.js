(function () {
    'use strict';
    angular
        .module('app')
        .controller('InductionController', inductionController)

    inductionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function inductionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {
   

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.HRDTA_Date = new Date();
        $scope.loadInitialData = function () {
            $scope.searchValue = "";
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.startdate1 = new Date($scope.HRDTA_Date).toDateString();
            var data = {
                "HRTCRINTTR_StartDate": $scope.startdate1
            };
            apiService.create("Induction_Training/loadinitialdata", data).
                then(function (promise) {
                    
                    $scope.trainingsub = promise.trainingsub;
                    $scope.trainingEmpList = promise.trainingEmpList;
                    $scope.buillist = promise.buillist;

            });
        };
        
        $scope.get_detail = function () {
            $scope.trainingsubdetails = [];
            angular.forEach($scope.trainingsub, function (dd) {
                if (dd.hrtcrinttR_Id == $scope.HRTCRINTTR_Id) {
                    $scope.trainingsubdetails.push(dd);
                }
            });          
        };

        $scope.submitted = false;
        $scope.SaveEmpAttendance = function () {

            if ($scope.myForm.$valid) {
                $scope.employeeAP= [];
                angular.forEach($scope.trainingEmpList, function (itm) {
                    if (itm.selected == true) {
                        $scope.employeeAP.push({
                            HRMTT_Id: itm.Hrme_id,
                            HRMTT_Topic: "A"                            
                        });
                    }
                    else{
                        $scope.employeeAP.push({
                            HRMTT_Id: itm.Hrme_id,
                            HRMTT_Topic: "P"
                        });
                    }
                });                
                var data = {
                    "HRTCRINTTR_Id": $scope.trainingsubdetails[0].hrtcrinttR_Id,
                    "HRTCRINTTR_StartDate": $scope.trainingsubdetails[0].hrtcrinttR_StartDate,
                    "START_DATE": $scope.trainingsubdetails[0].hrtcrinttR_StartTime,
                    "END_DATE": $scope.trainingsubdetails[0].hrtcrinttR_EndTime,
                    trainingTopicdetails: $scope.employeeAP
                };
                apiService.create("Induction_Training/SaveEmpAttendance", data).
                    then(function (promise) {
                        if (promise.message === 'Save') {
                            swal('Record Saved Successfully!', 'success');
                            loadInitialData();
                            $state.reload();
                        }
                        else if (promise.message === 'NotSave') {
                            swal('Record Not Saved Successfully!', 'Failed');
                            $state.reload();
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

        // Clear 
        $scope.clearid = function () {
            $state.reload();
        };

    }

})();