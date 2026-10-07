(function () {
    'use strict';
    angular.module('app').controller('FMS_File_Cabinet_ReportController', FMS_File_Cabinet_ReportController)
    FMS_File_Cabinet_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$sce', 'Excel','$timeout']
    function FMS_File_Cabinet_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $sce, Excel, $timeout) {

        $scope.getreport = [];
        var configsettings = JSON.parse(localStorage.getItem("configsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;


        $scope.OnloadFileCabinetReport = function () {
            var pageid = 2;
            apiService.getURI("FMS_Correspondance_Report/OnloadFileCabinetReport", pageid).then(function (promise) {
                $scope.MI_Id = promise.mI_Id;
                $scope.institutionlist = promise.institutionlist;
            });
        };

        $scope.OnChangeInst = function () {
            $scope.getreport = [];
        };

        $scope.SearchReport = function () {
            $scope.getreport = [];
            if ($scope.myForm2.$valid) {
                var data = "";
                data = {
                    "MI_Id": $scope.MI_Id                     
                };

                apiService.create("FMS_Correspondance_Report/FileCabinetReport", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.getreport = promise.getreport;
                        if ($scope.getreport === null || $scope.getreport.length === 0) {
                            swal("No Records Found");
                        }
                    }
                });

            } else {
                $scope.submitted2 = true;
            }
        };

        $scope.getclear = function () {

            $scope.MI_Id = "";
            $scope.getreport = [];
            $scope.OnloadFileCabinetReport();

        };

        $scope.interacted1 = function (field) {
            return $scope.submitted2;
        };

        $scope.exportToExceldetails = function () {

            var excelname = 'FMS FILE CABINET REPORT';
            excelname = excelname.toUpperCase() + '.xls';            
            var exportHref = Excel.tableToExcel('#exceldetail', 'FMS FILE CABINET REPORT');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.printDatadetails = function () {
            var innerContents = "";
            innerContents = document.getElementById("printdetail").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }; 
    } 
})();