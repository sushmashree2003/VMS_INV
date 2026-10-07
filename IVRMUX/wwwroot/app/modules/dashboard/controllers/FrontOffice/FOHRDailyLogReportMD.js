(function () {
    'use strict';
    angular.module('app').controller('FOHRDailyLogReportMDController', FOHRDailyLogReportMDController)

    //FOHRDailyLogReportMDController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache', '$filter', 'uiGridConstants', 'appSettings', '$sce', 'Excel']
    //function FOHRDailyLogReportMDController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache, $filter, $uiGridConstants, appSettings, $sce, Excel)

    FOHRDailyLogReportMDController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache' ,'$sce', 'Excel', '$timeout']
    function FOHRDailyLogReportMDController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $sce, Excel, $timeout) {
        $scope.searchValue = '';
        $scope.pdfexcel = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.get_report = function () {
            var data = {
                "START_DATE": new Date($scope.myDate_from).toDateString(),
                "END_DATE": new Date($scope.myDate_to).toDateString()
            };
            apiService.create("EmployeeLogReport/getDailyAttendanceRprt", data).
                then(function (promise) {
                    $scope.induction_training_report = promise.allFOHRAttendenceReportMD;   
                    if ($scope.induction_training_report.length === 0) {
                        swal('Record not Found');
                    }

                });
        }; 


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };


        $scope.printToCart = function () {
            $scope.pdfexcel = true;
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.ViewDocExcelfles = function (afu_filepath, afu_filename, filetype) {
            var docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + encodeURIComponent(afu_filepath);
            $scope.detailFrame = $sce.trustAsResourceUrl(docpath);
            $('#myModaldocx').modal('show');
        };

        $scope.exportToExcel = function (table1) {
            $scope.pdfexcel = true;
            if ($scope.induction_training_report !== null && $scope.induction_training_report.length > 0) {
                var exportHref = Excel.tableToExcel(table1, 'report');

                $timeout(function () { location.href = exportHref; }, 100);
                $timeout(function () {
                    location.href = exportHref;
                }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }

        }

    }
})();