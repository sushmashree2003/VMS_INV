
(function () {
    'use strict';
    angular
        .module('app')
        .controller('TenantRecordUpdationController', TenantRecordUpdationController);
    TenantRecordUpdationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', '$http', '$q', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$interval', '$sce', 'uiCalendarConfig', 'appSettings'];

    function TenantRecordUpdationController($rootScope, $scope, $state, $location, dashboardService, Flash, $http, $q, apiService, $stateParams, $filter, superCache, $window, $interval, $sce, uiCalendarConfig, appSettings) {
        var miid = "";

        $scope.closeupdate = false;
        $scope.conformflag = true;

        $scope.RedirectToNoticeboard = function () {
            window.location.href = "http://localhost:57606/#/app/TenantViewNotices/";
        };
        $scope.RedirectTopayment = function () {
            window.location.href = "http://localhost:57606/#/app/TenantPaymentReport/729";
        };


        // View Student Profile

        $scope.ViewTenantProfile = function () {

            //var data = {
            //    "AMST_Id": $scope.AMST_Id_session,
            //    "student_staffflag": 'Student'
            //};
            var pageid = 1;

            apiService.create("TenantRecordUpdation/ViewTenantProfile", pageid).then(function (promise) {
                //if (promise !== null) {


                if (promise.tenants_details != null) {
                    $scope.tenants_details = promise.tenants_details;

                    $scope.TENANTAGR_TenantName = $scope.tenants_details[0].tenantagR_TenantName;
                    $scope.TENANTAGR_Address = $scope.tenants_details[0].tenantagR_Address;
                    $scope.TENANTAGR_NoOfOccupants = $scope.tenants_details[0].tenantagR_NoOfOccupants;
                    $scope.TENANTAGR_ContactNo = $scope.tenants_details[0].tenantagR_ContactNo;
                    $scope.TENANTAGR_EmailId = $scope.tenants_details[0].tenantagR_EmailId;
                    $scope.TENANTAGR_Id = $scope.tenants_details[0].tenantagR_Id;
                }


                //}
            });
        };


   



        $scope.Saveupdate = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "TENANTAGR_Id": $scope.TENANTAGR_Id,
                    "TENANTAGR_TenantName": $scope.TENANTAGR_TenantName,
                    "TENANTAGR_Address": $scope.TENANTAGR_Address,
                    "TENANTAGR_NoOfOccupants": $scope.TENANTAGR_NoOfOccupants,
                    "TENANTAGR_ContactNo": $scope.TENANTAGR_ContactNo,
                    "TENANTAGR_EmailId": $scope.TENANTAGR_EmailId,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("TenantRecordUpdation/Editrenewal", data).then(function (promise) {
                    swal("Record saved Successfully!!!");
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };




        //anvesha
        //$scope.Saveupdate = function () {

        //    var data = {

        //        "TENANTAGR_Id": $scope.TENANTAGR_Id,
        //        "TENANTAGR_TenantName": $scope.TENANTAGR_TenantName,
        //        "TENANTAGR_Address": $scope.TENANTAGR_Address,
        //        "TENANTAGR_NoOfOccupants": $scope.TENANTAGR_NoOfOccupants,
        //        "TENANTAGR_ContactNo": $scope.TENANTAGR_ContactNo,
        //        "TENANTAGR_EmailId": $scope.TENANTAGR_EmailId,

        //    };

        //    apiService.create("TenantRecordUpdation/Editrenewal", data).then(function (promise) {
        //        swal("Record saved Successfully!!!");
        //        $state.reload();

        //    });
        //};





    }
})();

  
