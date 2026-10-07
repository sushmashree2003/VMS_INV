(function () {
    'use strict';
    angular
        .module('app')
        .controller('appointmentController', appointmentController);

    appointmentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter'];
    function appointmentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter) {


        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

       
        $scope.selectedList = [];
        $scope.get_scopenature = [];
        $scope.getAllDetail = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("Appointment/getalldetails", pageid).then(function (promise) {
                $scope.get_list = promise.get_scopenature;

                angular.forEach($scope.get_list, function (dd) {
                    if (dd.HRMNS_ApprovedStatus == "Pending" && dd.HRMNS_Activeflag == true) {
                        $scope.get_scopenature.push(dd);
                    }
                });
            });
        };


        $scope.toggleAll = function () {
            var toggleStatus = $scope.all2;
            angular.forEach($scope.get_scopenature, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all2 == true) {
                    $scope.selectedList.push(itm);
                }
                else {
                    $scope.selectedList.splice(itm);
                }
            });
        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all2 = $scope.get_scopenature.every(function (itm) { return itm.selected; });
            if ($scope.selectedList.indexOf(SelectedStudentRecord) === -1) {
                $scope.selectedList.push(SelectedStudentRecord);
            }
            else {
                $scope.selectedList.splice($scope.selectedList.indexOf(SelectedStudentRecord), 1);
            }
        }


        //save
        $scope.submitted = false;
        $scope.savedetails = function () {
            $scope.submitted = true;
            if ($scope.selectedList != null && $scope.selectedList.length > 0) {
                var data = {
                    "approvalList": $scope.selectedList,
                };
                apiService.create("Appointment/SaveUpdateApproval", data).then(function (promise) {
                    if (promise.retrunMsg == "Updated") {
                        swal('Record Saved Successfully... !');
                    }
                    else {
                        swal('Failed !');
                    }
                    $state.reload();
                });
            }
            else {
                swal('Please Select At Least One Check Box  !');
            }
        };


        $scope.cancel = function () {
            $state.reload();
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

    }
})();