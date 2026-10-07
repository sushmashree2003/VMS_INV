(function () {
    'use strict';
    angular
        .module('app')
        .controller('SalaryAdvanceApprovalController', SalaryAdvanceApprovalController)
    SalaryAdvanceApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function SalaryAdvanceApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

        $scope.HRPAON_SanctionLevelNo = "";
        $scope.obj = {};
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("SalaryAdvanceApproval/getalldetails", pageid).then(function (promise) {
                $scope.getloaddata = promise.aprovedlist;
                if (promise.empadvaList !== null && promise.empadvaList.length > 0) {
                    $scope.gridOptions = promise.empadvaList;
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
            $scope.HRESA_Remarks = "";
            $scope.HRPAON_SanctionLevelNo = "";

            $scope.HRESA_Remarks = user.HRESA_Remarks;
            $scope.HRESA_EntryDate = user.HRESA_EntryDate,
                $scope.HRESA_AdvYear = user.HRESA_AdvYear,
                $scope.HRESA_AdvMonth = user.HRESA_AdvMonth,
                $scope.HRESA_AppliedAmount = user.HRESA_AppliedAmount,
                $scope.HRESA_ModeOfPayment = user.HRESA_ModeOfPayment,
                $scope.HRME_Id = user.HRME_Id;
            $scope.HRESA_Id = user.HRESA_Id;

            $scope.aprovedlist = [];
            var data = {
                "HRME_Id": user.HRME_Id,
                "HRESA_Id": user.HRESA_Id,
            };
            apiService.create("SalaryAdvanceApproval/empdetails", data).then(function (promise) {

                $scope.HRPAON_SanctionLevelNo = promise.HRESAA_SanctionLevel;
                $scope.getdata = promise.griddisplay;
                if (promise.aprovedlist != null && promise.aprovedlist.length > 0) {
                    $scope.aprovedlist = promise.aprovedlist;
                }

            })
        }
        $scope.approvedreject = function (user) {
            if ($scope.myForm.$valid) {

                var data = {
                    "HRESA_Id": $scope.HRESA_Id,
                    "HRESAA_Remarks": $scope.obj.HRESAA_Remarks,
                    "HRESA_SanctinedAmount": $scope.obj.HRESA_SanctinedAmount,
                    "HRME_Id": $scope.HRME_Id,
                    "HRESA_AdvStatus": user,
                }
                apiService.create("SalaryAdvanceApproval/approvedrecord", data).then(function (promise) {

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
    }

})();