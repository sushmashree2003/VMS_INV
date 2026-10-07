(function () {
    'use strict';
    angular
        .module('app')
        .controller('TaskRejectReportController', TaskRejectReportController);

    TaskRejectReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', 'uiGridGroupingConstants', '$timeout', '$q', 'appSettings', '$http'];

    function TaskRejectReportController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, uiGridGroupingConstants, $timeout, $q, appSettings, $http) {

        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.search = '';
        $scope.submitted = false;
        $scope.filtervalue1 = function (user) {
        };

        $scope.loaddata = function () {
            $scope.start_Date = new Date();
            $scope.end_Date = new Date();
        };
        $scope.submitted = false;
        $scope.TaskRejectReport = function () {
            if ($scope.myForm.$valid) {

                $scope.RejectReport = [];

                var data = {
                    "startdate": new Date($scope.start_Date).toDateString(),
                    "enddate": new Date($scope.end_Date).toDateString(),

                };
                apiService.create("ISM_PlannerDeviationReport/taskRejectReport", data).then(function (promise) {
                    if (promise.rejectReport.length > 0 && promise.rejectReport != null) {
                        $scope.RejectReport = promise.rejectReport
                    }
                    else {
                        swal("No Records..!")
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.printData = function () {
            $scope.hide_n = false;
            var innerContents = "";
            innerContents = document.getElementById("printP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.exportToExcel = function () {
            var printSectionId = '#excelid';
            var excelname = 'TaskReject Report';
            excelname = excelname.toUpperCase() + '.xls';
            var exportHref = Excel.tableToExcel(printSectionId, 'TaskReject Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelnamemain;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };




    }
})();


