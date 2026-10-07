(function () {
    'use strict';
    angular.module('app').controller('ISM_ClientProject_Payment_ConfigController', ISM_ClientProject_Payment_ConfigController)
    ISM_ClientProject_Payment_ConfigController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function ISM_ClientProject_Payment_ConfigController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {
      
        var paginationformasters;  
        var copty;  
        
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.submitted = false;

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("ISM_Master_ClientProject_Payment/loaddataconfig", pageid).then(function (promise) {
                if (promise.getconfigloaddata !== null && promise.getconfigloaddata.length > 0) {
                    $scope.ISMCPC_RemainderDays = promise.getconfigloaddata[0].ismcpC_RemainderDays;
                    $scope.ISMCPC_FullORPartialPayment = promise.getconfigloaddata[0].ismcpC_FullORPartialPayment;
                    $scope.ISMCPC_Id = promise.getconfigloaddata[0].ismcpC_Id;
                }
            });
        };

        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "ISMCPC_Id": $scope.ISMCPC_Id,
                    "ISMCPC_RemainderDays": $scope.ISMCPC_RemainderDays,
                    "ISMCPC_FullORPartialPayment": $scope.ISMCPC_FullORPartialPayment                   
                };

                apiService.create("ISM_Master_ClientProject_Payment/savedataconfig", data).then(function (promise) {
                    if (promise.duplicate === true) {
                        swal("Data Already Exist!");
                    }
                    else if (promise.msg === 'saved') {
                        swal("Data Saved Successfully");
                    }                    
                    else if (promise.msg === 'updated') {
                        swal("Data Updated Successfully ");
                    } else {
                        swal("Failed To Save/Update Record");
                    }
                    $scope.loaddata();
                });
            }
            else {
                $scope.submitted = true;
            }
        };       

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();