(function () {
    'use strict';
    angular
        .module('app')
        .controller('VMS_Live_Interview_ReportController', VMS_Live_Interview_ReportController);

    VMS_Live_Interview_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache', 'Excel', '$timeout'];
    function VMS_Live_Interview_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache, Excel, $timeout) {
        //object

        //Employeee
        $scope.currentPage = 1;
        $scope.searchValue = '';
        $scope.itemsPerPage = 10;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;


        $scope.GetReport = function () {
            if ($scope.myForm.$valid) {


                $scope.FromDate1 = $filter('date')($scope.FromDate, "yyyy-MM-dd");

                $scope.ToDate1 = $filter('date')($scope.ToDate, "yyyy-MM-dd");

                var data = {
                    "FromDate": $scope.FromDate1,
                    "ToDate": $scope.ToDate1
                }
                apiService.create("VMS_Live_Interview/GetReport", data).then(function (promise) {
                    if (promise.vmslivemeetingreport != null || promise.vmslivemeetingreport > 0) {
                        $scope.vmslivemeetingreport = promise.vmslivemeetingreport;
                    }
                    else {
                        swal('No Data Found..!!')
                    }


                })
            }
            else {
                $scope.submitted = true;
            }
        }


        $scope.cleardata = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.printData = function () {
            var ids = 'printSectionId1'
            var innerContents = document.getElementById(ids).innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/ExchangableCoursePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 1000);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.ExportToExcel = function () {

            var ids = '#printSectionId1'

            var exportHref = Excel.tableToExcel(ids, '');
            $timeout(function () { location.href = exportHref; }, 1000);
        }

    }


})();