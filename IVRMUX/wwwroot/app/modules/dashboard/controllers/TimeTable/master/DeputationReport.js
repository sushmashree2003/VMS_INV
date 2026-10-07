
(function () {
    'use strict';
    angular
        .module('app')
        .controller('TTDeputationReportController', TTDeputationReportController)

    TTDeputationReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function TTDeputationReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.loaddata = function () {
            $scope.report = false;
            apiService.getDATA("TTMonthEndReport/getalldetails123").
                then(function (promise) {
                    $scope.acayyearbind = promise.acdlist;
                })
        }     

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clear_Details = function () {
            $state.reload();
            $scope.loaddata();
        }
        //for print
        $scope.printData = function () {

            var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/Sports/HouseReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            
        }
        $scope.exportToExcel = function (table) {


            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }

        $scope.ShowReportdata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate,
                    "ASMAY_ID": $scope.academicyr
                }
                apiService.create("TTDeputationReport/getreport", data).
                    then(function (promise) {
                        if (promise.reportdatelist.length > 0 && promise.reportdatelist != null) {
                            $scope.report = true;
                            $scope.datapages = promise.reportdatelist;
                        }
                        else {
                            swal("Record Not Found");
                        }
                    })
            }
            else {
                $scope.submitted = true;
            }
        }
    }
})();

