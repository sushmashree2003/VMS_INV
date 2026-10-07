(function () {
    'use strict';
    angular
        .module('app')
        .controller('MOM_TransactionController', MOM_TransactionController)

    MOM_TransactionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']

    function MOM_TransactionController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {

        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;

        $scope.OnLoad = function () {
            var pageid = 1;
            apiService.getURI("MOMUpdate/onloadVendors", pageid).then(function (promise) {
                if (promise.onloadVendors !== null && promise.onloadVendors.length > 0) {
                    $scope.onloadVendors = promise.onloadVendors;
                }
            });
        };

        $scope.submitted = false;

        $scope.SaveDataNew = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "INVMS_SupplierName": $scope.name,
                    "INVMS_SupplierConatctNo": $scope.ContactNo,
                    "INVMS_EmailId": $scope.EmailId
                };
                apiService.create("MOMUpdate/SaveVendors", data).then(function (promise) {
                    if (promise.retrunMsg === 'Add') {
                        swal("Record Saved Successfully");
                        $state.reload();
                    }
                    else if (promise.retrunMsg === 'duplicate') {
                        swal("Record already exist");
                        $state.reload();
                    }
                    else if (promise.retrunMsg === "false") {
                        swal("Failed to save record");
                    }
                    else {
                        swal("Email not sent");
                    }
                });
               
            }
        };
        $scope.cancel = function () {
            $state.reload();
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
    }
})();
