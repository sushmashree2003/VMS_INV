
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_DepreciationCalculationController', INV_Depreciation_ReportController);
    INV_Depreciation_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter']
    function INV_Depreciation_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {
        $scope.obj = {};
        var paginationformasters;
        $scope.optionflag = "Supplier";
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        $scope.ngchange = function () {
            $scope.optionflag = false;
        }
        $scope.searchValue = "";[];
        $scope.year = [];
        //==========================================================
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("INV_Depreciation_Report/getloaddata", pageid).then(function (promise) {
                $scope.Institution = promise.institution;
                if (promise.year != null && promise.year.length > 0) {

                  
                    angular.forEach(promise.year, function (obj) {
                        if (obj.imfY_FinancialYear != '2016-2017' && obj.imfY_FinancialYear != '2014-2015' && obj.imfY_FinancialYear != '2015-2016') {
                            $scope.year.push(obj);
                        }

                    })
                }
                



            });
        };
        //==================================PI Report
        $scope.submitted = false;
        $scope.onreport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "MI_Id": $scope.MI_Id,
                    "FYear": $scope.imfY_FinancialYear
                };
                apiService.create("INV_Depreciation_Report/DepreciationCalculation", data).
                    then(function (promise) {
                        swal("Depreciation calculated Suceesfully  !");
                    });
            }


            
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cancel = function () {
            $state.reload();
        };
        $scope.exportToExcel = function (export_id) {

            var exportHref = Excel.tableToExcel(export_id, 'sheet name');
            $timeout(function () {
                location.href = exportHref;
            }, 100);

        };
        $scope.printData = function () {

            var innerContents = document.getElementById("printpm").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

    }
})();