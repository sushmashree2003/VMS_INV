//(function () {
//    'use strict';
//    angular
//        .module('app')
//        .controller('TrainingAssigTopic', TrainingAssigTopic)

//    TrainingAssigTopicController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
//    function TrainingAssigTopicController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
//        $scope.sortKey = '';
//        $scope.sortReverse = true;


//        $scope.sort = function (key) {
//            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
//            $scope.sortKey = key;
//        };
//        $scope.searchValue = '';
//        $scope.filterValue = function (obj) {

//            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
//        };

//        //get data
//        $scope.loaddata = function () {
//            $scope.currentPage = 1;
//            $scope.itemsPerPage = 10;
//            var pageid = 2;
//            apiService.getURI("Training_Assignment_Topic/getdata_TAT", pageid)
//                .then(function (promise) {
//                    $scope.question = false;
//                    $scope.training = promise.training;
//                    $scope.getmasterdata = promise.getmasterdata;
                   
//                });
//        };
//    }
//})();

(function () {
    'use strict';
    angular.module('app').controller('TrainingAssigTopic', TrainingAssigTopic)
    TrainingAssigTopic.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache', 'myFactoryvisitor', '$q']
    function TrainingAssigTopic($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache, myFactoryvisitor, $q) {

        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.obj1 = {};
       
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        //ge data
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("Training_Assignment_Topic/loaddata", pageid).then(function (promise) {
                $scope.training = promise.training;
                $scope.getmasterdata = promise.getmasterdata;
            });
        };
       

    }
})();