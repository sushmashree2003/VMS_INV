(function () {
    'use strict';
    angular
        .module('app')
        .controller('IEandElearningLogReportController', IEandElearningLogReportController)

    IEandElearningLogReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function IEandElearningLogReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.sortKey = 'HRTRQNS_Id';
        $scope.sortReverse = true;
        $scope.obj = {};
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
            apiService.getURI("EmployeeLogReport/getalldetails", pageid)
                .then(function (promise) {
                    $scope.leaveyeardropdown = promise.fillyear;

                    $scope.employeelist = promise.employeename;
                });
        };



        //Report
        $scope.submitted = false;
        $scope.Showreport = function () {
            $scope.trainingfeedbackdetails = [];
            $scope.submitted = true;
          if ($scope.myForm.$valid) {
            var data = {
                "monthid": $scope.month,
                "HRMLY_Id": $scope.hreS_Year,
                "HRME_Id": $scope.obj.hrmE_Id.hrmE_Id
            };
              apiService.create("EmployeeLogReport/IEeLearningLogreport", data).
                then(function (promise) {
                    if (promise.logindetails != null && promise.logindetails.length != 0) {
                        $scope.logindetails = promise.logindetails;
                        $scope.presentCountgrid = $scope.logindetails.length;
                    }
                    if (promise.trainingfeedbackdetails.length = 0) {
                        swal("Records Not Found")
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };



        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.Clearid = function () {
            $state.reload();
        };
    }

})();



