(function () {
    'use strict';
    angular
        .module('app')
        .controller('employeementVerificationController', employeementVerificationController);

    employeementVerificationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter'];
    function employeementVerificationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter) {

        $scope.EarningDet = {};
        $scope.addjob = {};
        $scope.addjob.hrjD_Id = 0;
        $scope.submitted = true;
        $scope.CandidateView = false;

        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("AddtoHRMS/getverifyemplist", pageid).then(function (promise) {
                $scope.candidatelist = promise.candidatelist;
            });
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.submitted = false;
        $scope.interacted1 = function (field) {
            return $scope.submitted;
        };

        $scope.findreport = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "HRME_ID" : $scope.hrmE_Id
                };

                apiService.create("AddtoHRMS/getverifyempdetails", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {
                            $scope.currentdate = new Date();

                            if (promise.candidateDetails.length > 0 && promise.candidateDetails != null) {
                                $scope.candidateDetails = promise.candidateDetails[0];
                                $scope.CandidateView = true;
                            }

                            if (promise.retrunMsg === "Not Found") {
                                swal("Record Not Found", 'Fail');
                            }
                        }
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.printToCart = function (vapsappointment) {
            var innerContents = document.getElementById("vapsappointment").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
    }
})();