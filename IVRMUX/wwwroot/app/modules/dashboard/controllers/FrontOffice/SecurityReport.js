(function () {
    'use strict';
    angular
        .module('app')
        .controller('SecurityRoasterReportController', SecurityRoasterReportController)
    SecurityRoasterReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce'];
    function SecurityRoasterReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce) {

        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.itemsPerPage1 = 10;
        $scope.currentPage1 = 1;
        $scope.page1 = "page1";
        $scope.search = " ";
        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };
        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        $scope.printdatatable = [];
             
        $scope.getalldetails = function () {
            var pageid = 2;
            apiService.getURI("SecurityRoaster/Binddata", pageid).then(function (promise) {
                $scope.floorlist = promise.floorlist;
            });
        };

        $scope.getreport = function () {
            if ($scope.myForm.$valid) { 
                $scope.printdatatable = [];
                var datefrom = $scope.from_Date === null ? "" : $filter('date')($scope.from_Date, "yyyy-MM-dd");
                var dateto = $scope.to_Date === null ? "" : $filter('date')($scope.to_Date, "yyyy-MM-dd");
                var data = {
                    "fromdate": datefrom,
                    "todate": dateto,
                    "HRMF_Id": $scope.hrmF_Id
                };
                apiService.create("SecurityRoaster/report", data).then(function (promise) {
                    $scope.report = promise.report;
                });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.printData = function (printSectionId) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                    '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                    '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
        }
        $scope.search = '';
        $scope.filtervalue1 = function (user) {
            //return (angular.lowercase(user.name)).indexOf(angular.lowercase($scope.search)) >= 0 ||
            //    (angular.lowercase(user.number)).indexOf(angular.lowercase($scope.search)) >= 0 ||
            //    (angular.lowercase(user.email)).indexOf(angular.lowercase($scope.search)) >= 0;
        };
        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();