
(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmployeeMappingController', EmployeeMappingController);
    EmployeeMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function EmployeeMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.obj = {};

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("AT_MasterLocation/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_sites = promise.get_sites;
                    $scope.get_employee = promise.get_employee;
                });
                
            apiService.getURI("AssetTagCheckOut/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_locations = promise.get_locations;
                });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.get_employee.some(function (options) {
                return options.Selected;
            });
        };

        $scope.all_check = function () {
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.get_employee, function (itm) {
                itm.Selected = toggleStatus;
            });
        };

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.get_employee.every(function (options) {
                return options.Selected;
            });
        };

        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var employee_list = [];
                angular.forEach($scope.get_employee, function (emp) {
                    if (emp.Selected) {
                        employee_list.push({ hrmE_Id: emp.hrmE_Id });
                    }
                });

                var data = {
                    "INVMSI_Id": $scope.obj.invmsI_Id,
                    "INVMLO_Id": $scope.obj.invmlO_Id.invmlO_Id,
                    "HRME_Id": $scope.obj.hrmE_Id,
                    "emp_list": employee_list
                };

                apiService.create("AT_MasterLocation/savedetailEmpFloorMapping/", data).then(function (promise) {
                    if (promise.message != null && promise.message != "") {
                        swal(promise.message);
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmI_Id == 0 || promise.invmI_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmI_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $state.reload();
                });
            }

            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };

    }
})();
