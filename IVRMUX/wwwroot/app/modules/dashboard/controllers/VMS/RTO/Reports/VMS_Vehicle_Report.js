(function () {
    'use strict';
    angular.module('app').controller('VMS_Vehicle_ReportController', VMS_Vehicle_ReportController)
    VMS_Vehicle_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', 'Excel','$timeout']
    function VMS_Vehicle_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, Excel, $timeout) {
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };


        $scope.submitted = false;

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
        var configsettingsnew = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        $scope.itemsPerPage = paginationformasters;

        $scope.currentPage = 1;
        //-----------Tax Grid Select All
        $scope.ismeridian = true;
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.VMSVKEYS_DateTime = new Date();
        $scope.allmi = false;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian; todate
        };
        $scope.FromDate = new Date();
        $scope.ToDate = new Date();
        $scope.loaddata = function () {

            var pageid = 2;
            apiService.getURI("VMS_Vehicle_Report/loaddata", pageid).then(function (promise) {

                $scope.vehicale = promise.vehicales;
                if ($scope.vehicale != null && $scope.vehicale.length > 0) {
                    $scope.allmi = true;
                    angular.forEach($scope.vehicale, function (sne) {
                        sne.miid = true;
                    });
                  
                }
               
            });
        };
        $scope.all_miid = function () {
            if ($scope.allmi == true) {
                angular.forEach($scope.vehicale, function (sne) {
                    sne.miid = true;
                });
            }
            else {
                angular.forEach($scope.vehicale, function (sne) {
                    sne.miid = false;
                });
            }
        }
        $scope.getreport = function () {
            $scope.vehicalList = [];
           $scope.Vehicalisttemp = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                angular.forEach($scope.vehicale, function (sne) {
                   // sne.miid = true
                    if (sne.miid == true) {
                        $scope.Vehicalisttemp.push({
                            VMSVKEYS_Id: sne.trmV_Id
                        });
                    }

                });
                
                var data = {

                    "FromDate": new Date($scope.FromDate).toDateString(),
                    "ToDate": new Date($scope.ToDate).toDateString(),
                    "Vehicalist": $scope.Vehicalisttemp ,
                };
                apiService.create("VMS_Vehicle_Report/RecieptReport", data).then(function (promise) {
                    if (promise.getReport != null && promise.getReport.length > 0) {
                        $scope.vehicalList = promise.getReport;
                        $scope.milogo = promise.milogo
                        angular.forEach($scope.vehicalList, function (itm) {
                            itm.VMSVKEYS_RecivedTime = moment(itm.VMSVKEYS_RecivedTime, 'HH:mm a').format();
                            itm.VMSVKEYS_Time = moment(itm.VMSVKEYS_Time, 'HH:mm a').format();

                        });
                    }
                    else {
                        swal("Record Not Found  !");
                    }
                  
                    
                });
            }
        };
       
        $scope.isRequiredmi = function () {

            return !$scope.vehicale.some(function (options) {
                return options.miid;
            });
            
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clear = function () {
            $state.reload();
        };
        $scope.printData = function () {
            var name = "printed";
            var innerContents = document.getElementById(name).innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.exportToExcel = function (export_id) {

            var exportHref = Excel.tableToExcel(export_id, 'sheet name');
            $timeout(function () {
                location.href = exportHref;
            }, 100);

        };
    }
})();
