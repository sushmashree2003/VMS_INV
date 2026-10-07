(function () {
    'use strict';
    angular
        .module('app')
        .controller('FF_Settlement_ApprovalProcessController', FF_Settlement_ApprovalProcessController)
    FF_Settlement_ApprovalProcessController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function FF_Settlement_ApprovalProcessController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

        $scope.HRPAON_SanctionLevelNo = "";
        $scope.obj = {};
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("FF_Settlement_Process/getdetails", pageid).then(function (promise) {
              
                if (promise.empadvaList !== null && promise.empadvaList.length > 0) {
                    $scope.gridOptions = promise.empadvaList;
                }
                if (promise.aprovedlist !== null && promise.aprovedlist.length > 0) {
                    $scope.getloaddata = promise.aprovedlist;;
                }
            })
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.Clear = function () {
            $state.reload();
        }

        $scope.GetAllRelData = function (user) {
            $scope.HREFFS_Remarks = "";
            $scope.HRPAON_SanctionLevelNo = "";
            $scope.HREFFS_Remarks = user.HREFFS_Remarks;
            $scope.HREFFS_EntryDate = user.HREFFS_EntryDate;
            $scope.HRME_Id = user.HRME_Id;
            $scope.HREFFS_Id = user.HREFFS_Id;

            $scope.aprovedlist = [];
            var data = {
                "HRME_Id": user.HRME_Id,
                "HREFFS_Id": user.HREFFS_Id,  
            };
            apiService.create("FF_Settlement_Process/empdetails", data).then(function (promise) {

                $scope.HRPAON_SanctionLevelNo = promise.HREFFSA_SanctionLevel;
                $scope.getdata = promise.griddisplay;
                if (promise.aprovedlist != null && promise.aprovedlist.length > 0) {
                    $scope.aprovedlist = promise.aprovedlist;
                }

            })
        }
        $scope.approvedreject = function (user) {
            if ($scope.myForm.$valid) {

                var data = {
                    "HREFFS_Id": $scope.HREFFS_Id,
                    "HREFFSA_Remarks": $scope.obj.HREFFSA_Remarks,
                    "HREFFS_SanctinedAmount": $scope.obj.HREFFS_SanctinedAmount,
                    "HRME_Id": $scope.HRME_Id,
                    "HREFFS_AdvStatus": user,
                }
                apiService.create("FF_Settlement_Process/approvedrecord", data).then(function (promise) {

                    if (promise.retrunMsg == "Update") {
                        swal('Record saved successfully');
                        $state.reload();
                    }
                    else if (promise.retrunMsg == "false") {
                        swal('Record  Not saved successfully');
                    }
                })
            }
            else {
                $scope.submitted = true;
            }

        };
        $scope.Clearid = function (user) {
            $state.reload();
        }
    
    }

})();