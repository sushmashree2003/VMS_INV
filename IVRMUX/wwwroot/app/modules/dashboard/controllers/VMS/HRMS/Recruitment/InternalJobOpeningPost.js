(function () {
    'use strict';
    angular
        .module('app')
        .controller('InternalJobOpeningPostController', InternalJobOpeningPostController);

    InternalJobOpeningPostController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'uiGridExporterService', 'uiGridExporterConstants'];
    function InternalJobOpeningPostController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, uiGridExporterService, uiGridExporterConstants) {

        $scope.onLoadGetData = function () {
            $scope.JobDetails = false;
            var pageid = 2;
            apiService.getURI("InternalJobOpeningPost/getalldetails", pageid).then(function (promise) {

                $scope.getreport = promise.getreport;
               
               
            });
        };

    }



})();