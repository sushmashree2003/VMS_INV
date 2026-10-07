(function () {
    'use strict';
    angular
        .module('app')
        .controller('TrainingController', trainingController)

    trainingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter']
    function trainingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter) {

        $scope.sortKey = 'Id';
        $scope.sortReverse = true;
        $scope.donce = false;
        $scope.maxdate = new Date();
        $scope.date_of = new Date();
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian; todate
        };

        $scope.hstep = 1;
        $scope.mstep = 1;
        //get data 
        $scope.getAllDetail = function () {
            $scope.currentPage = 1;
            var pageid = 2;
            apiService.getURI("Training_Master/getdata_T", pageid)
                .then(function (promise) {
                    $scope.trainingIssue_lists = promise.observedIssuesList;
                    $scope.employeedropdown = promise.train_list;
                })
        }


        //save
        $scope.submitted = false;
        $scope.itemsPerPage = 10;
        $scope.currentPage = 10;
        $scope.savetmpldata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HRMETR_DOB": $scope.date_of,
                    "HRMETR_Remarks": $scope.description,
                    "HRMETR_Name": $scope.VMSMDSH_ShiftStartTime === null ? "" : $filter('date')($scope.VMSMDSH_ShiftStartTime, "HH:mm"),
                    "IVRMMG_GenderName": $scope.VMSMDSH_ShiftEndTime === null ? "" : $filter('date')($scope.VMSMDSH_ShiftEndTime, "HH:mm"),
                    "HRME_Id": $scope.hrmE_Id
                };
                apiService.create("Training_Master/Save_TM", data).then(function (promise) {
                    if (promise.returnvalue !== "") {
                        if (promise.returnvalue === "Success") {
                            swal('Record Saved Successfully');
                            $state.reload();
                            return;
                        }
                        else if (promise.returnvalue === "NotSuccess") {
                            swal('Record Not Saved successfully');
                            return;
                        }
                    }
                    else {
                        swal('Operation Failed');
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        //cancel
        $scope.cancel = function () {
            $state.reload();
        }










    }
})();



