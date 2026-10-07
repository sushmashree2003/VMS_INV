(function () {
    'use strict';
    angular
        .module('app')
        .controller('Trainingmonthly_ReportController', Trainingmonthly_ReportController)

    Trainingmonthly_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function Trainingmonthly_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.sortKey = 'HRTRQNS_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        //get data
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("Training_Feedback/reportloaddata", pageid)
                .then(function (promise) {
                    //$scope.traninglisttrainer = promise.traninglisttrainer;
                    //$scope.traninglisttrainee = promise.traninglisttrainee;
                    $scope.leaveyeardropdown = promise.leaveyeardropdown;

                    //$scope.traninglisttrainee = promise.traninglisttrainer;
                });
        };



        //Report
        $scope.submitted = false;
        $scope.trainingreport = function () {
          $scope.feedbackdls = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
            var data = {
                "Monthid": $scope.month,
                "HRMLY_Id": $scope.hreS_Year,
                "feedbacktype": $scope.feedbacktype
            };
            apiService.create("Training_Feedback/trainigheldreport", data).
                then(function (promise) {
                    if (promise.trainingfeedbackdetails != null && promise.trainingfeedbackdetails.length  > 0) {
                        $scope.feedbackdls = promise.trainingfeedbackdetails;
                        $scope.presentCountgrid = $scope.feedbackdls.length;
                       
                    }
                    else {
                        swal("Records Not Found")
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


        //$scope.interacted = function (field) {
        //    return $scope.submitted || field.$dirty;
        //};

        //$scope.interacted2 = function (field) {
        //    return $scope.submitted || field.$dirty;
        //};

        $scope.Clearid = function () {
            $state.reload();
        };
    }

})();



