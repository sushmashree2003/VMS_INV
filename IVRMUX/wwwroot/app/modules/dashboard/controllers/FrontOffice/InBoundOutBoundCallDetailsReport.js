(function () {
    'use strict';
    angular
        .module('app')
        .controller('InBoundOutBoundCallDetailsReportController', InBoundOutBoundCallDetailsReportController);

    InBoundOutBoundCallDetailsReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter'];
    function InBoundOutBoundCallDetailsReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter) {

        $scope.addjob = {};
        $scope.EmployeeDis = false;
        $scope.submitted = false;

        $scope.cancel = function () {
            $scope.ismioC_Datefrom = "";
            $scope.ismioC_Dateto = "";
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cleardata = function () {
            $scope.EmployeeDis = false;
        };

        $scope.Report = function () {
            if ($scope.myForm.$valid) {

                $scope.ismioC_Date = $scope.ismioC_Date == null ? "" : $filter('date')($scope.ismioC_Date, "yyyy-MM-dd");
                var data = {
                    "ISMIOC_Datefrom": $scope.ismioC_Datefrom,
                    "ISMIOC_Dateto": $scope.ismioC_Dateto
                };

                apiService.create("InOutBoundCallDetail/report", data).
                    then(function (promise) {
                        if (promise.inoutboundlist != undefined && promise.inoutboundlist != null) {
                            $scope.EmployeeDis = true;
                            $scope.inoutboundlist = promise.inoutboundlist;
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.printToCart = function (Table) {
            var innerContents = document.getElementById("Table").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'Table');
            $timeout(function () { location.href = exportHref; }, 100);
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };
    }

})();