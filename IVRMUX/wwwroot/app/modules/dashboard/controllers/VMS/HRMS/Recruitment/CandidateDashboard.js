(function () {
    'use strict';
    angular
        .module('app')
        .controller('CandidateDashboardController', CandidateDashboardController);

    CandidateDashboardController.$inject = ['$scope','$state'];
    function CandidateDashboardController($scope, $state) {


        $scope.loaddata = function () {
            $scope.search = "";

            $scope.currentPage4 = 1;
            $scope.itemsPerPage4 = 10;
            apiService.getDATA("VMS_Live_Interview/GetOnloaddata").
                then(function (promise) {
                    $scope.get_userEmplist = promise.get_userEmplist;
                    if (promise.vmslivemeetinglist != null || promise.vmslivemeetinglist > 0) {
                        $scope.vmslivemeetinglist = promise.vmslivemeetinglist;
                    }

                })
        };

        $scope.redirectpage = function () {
            $state.go('app.addCandidate');
        };

        $scope.vcclick = function () {
            $state.go('app.VMS_CandiateMeeting_Join');
        };


        //$scope.vcclick = function () {
        //    $window.location.href = 'http://' + HostName + '/#/app/VMS_CandiateMeeting_Join/';
        //};
    }
})();