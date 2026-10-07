(function () {
    'use strict';
    angular
        .module('app')
        .controller('vmsVerifyCandidateReportsController', vmsVerifyCandidateReportsController);

    vmsVerifyCandidateReportsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce', '$timeout', 'Excel'];
    function vmsVerifyCandidateReportsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce, $timeout, Excel) {


        $scope.VerifyReports = function () {
            var dataToSave = {
                "Fromdate": $scope.fromdate,    
                "Todate": $scope.todate,
                //"HRME_Id": HRME_Id,
                "HRCDDV_RejectedFlg": $scope.all2
            };



            apiService.create("AddCandidateVMS/VerifyReports", dataToSave).then(function (promise) {
                if (promise.candidateDetailsverfy != null && promise.candidateDetailsverfy.length > 0) {
                    $scope.getreport = promise.candidateDetailsverfy
                }
                else {
                    swal("No Record Found");
                }
                //$state.reload();
            });


            //apiService.create("AddCandidateVMS/VerifyReports", dataToSave)
            //    .then(function (promise) {
            //        if (promise.CandidateDetailsverfy != null && promise.CandidateDetailsverfy.length > 0) {
            //            $scope.getreport = promise.CandidateDetailsverfy
            //        }
            //        else {
            //            swal("No Record Found");
            //        }

            //        $state.reload();
            //    });
        };








    }

})();