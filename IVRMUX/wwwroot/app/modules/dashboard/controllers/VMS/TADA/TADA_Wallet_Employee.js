(function () {
    'use strict';
    angular
        .module('app')
        .controller('TADA_Wallet_Controller', TADA_Wallet_Controller)
    TADA_Wallet_Controller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache', '$q']
    function TADA_Wallet_Controller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache, $q) {
        $scope.currentPage = 1;
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        else {
            paginationformasters = 10;
        }

        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        };
        $scope.Datacategorynew = [];
        $scope.onpageload = function () {
            var pageid = 1;
            $scope.Opening = ""; $scope.Remaining = "";
            apiService.getURI("TADA_Advance_Tour/get_Wallet_load", pageid).
                then(function (promise) {
                    if (promise.getPlanerdata != null && promise.getPlanerdata.length > 0) {
                        $scope.getPlanerdata = promise.getPlanerdata;
                        $scope.IER_ID = 100000012;
                    }
                    $scope.Tour_Details = []; $scope.TourPlanDetails = [];
                    //Tour_Details
                    if (promise.tour_Details != null && promise.tour_Details.length > 0) {
                        
                        $scope.Opening = promise.tour_Details[0].RTW_Wallet;
                        $scope.Remaining = promise.tour_Details[0].RTW_Remaining;
                    }
                    if (promise.tourPlanDetails != null && promise.tourPlanDetails.length > 0) {
                        $scope.TourPlanDetails = promise.tourPlanDetails;                        
                    }

                    //TourPlanDetails
                })
        }
        $scope.submitted = true;
        $scope.interacted= function (field) {
            return $scope.submitted;
        };
               $scope.cancel_tada = function () {            $state.reload();        };
        $scope.OnTransction = function () {            $scope.TourPlanDetails = [];            if ($scope.IER_ID > 0 && $scope.IER_ID != 100000012) {                var data = {
                    "IER_ID": $scope.IER_ID
                }
                apiService.create("TADA_Advance_Tour/get_Wallet_Client", data).then(function (promise) {
                    if (promise.tourPlanDetails != null && promise.tourPlanDetails.length > 0) {
                        $scope.TourPlanDetails = promise.tourPlanDetails;
                    }
                    else {
                        swal("Record  Not Found !");
                    }
                })
            }            else {                $scope.onpageload();
            }                 };
        
    }
})();

