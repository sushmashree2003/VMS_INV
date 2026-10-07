(function () {
    'use strict';
    angular
.module('app')
        .controller('DriverOTReportController', DriverOTReportController)

    DriverOTReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout', 'superCache']
    function DriverOTReportController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, Excel, $timeout, superCache) {

        $scope.disabledata = false;
        $scope.submitted = false;

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 20;
            apiService.getURI("DriverOT/getalldetails", pageid).
                then(function (promise) {
                    $scope.disabledata = false;
                    $scope.employeelist = promise.employeelist;
                    $scope.monthlist = promise.monthlist;
                    $scope.yearlist = promise.yearlist;
                });
        };

        $scope.finalOT = function () {
            var data = {
                "HRME_Id": $scope.hrmE_Id,
                "monthid": $scope.ivrM_Month_Id,
                "year": $scope.yearid
            };
            apiService.create("DriverOT/finalOTreport", data).
                then(function (promise) {
                    $scope.finalotreport = promise.finalotreport;
                    if (promise.finalotreport.length > 0) {
                        $scope.disabledata = true;
                    }
                });
        };

        $scope.exptoex = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'Table2');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };

        $scope.printData = function () {
            var innerContents = document.getElementById("Table2").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.cleardata = function () {
            $scope.disabledata = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.hrmE_Id = "";
            $scope.yearid = "";
            $scope.ivrM_Month_Id = "";
        };
    }
    
})();   