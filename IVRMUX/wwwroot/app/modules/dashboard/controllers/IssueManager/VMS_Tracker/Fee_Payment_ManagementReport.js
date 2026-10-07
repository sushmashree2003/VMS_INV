
(function () {
    'use strict';
    angular
        .module('app')
        .controller('Fee_Payment_ManagementReportReportController', Fee_Payment_ManagementReportReportController);
    Fee_Payment_ManagementReportReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter'];
    function Fee_Payment_ManagementReportReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {

        $scope.obj = {};
        $scope.optionflag = "Item";
        var paginationformasters;
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


        //==========================================================

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var id = 1;
            apiService.getURI("Fee_Payment_Management/getloaddata", id).
                then(function (promise) {

                    $scope.clientlist = promise.clientlist;


                });
        };

        $scope.onrdochange = function (optionflag) {
            $scope.get_itemreportdetails = "";
            $scope.get_itemreport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag);
        };

        //========================ITEM
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.clientlist.every(function (itm) {
                return itm.itemck;
            });
        };
        $scope.isOptionsRequiredI = function () {

            return !$scope.clientlist.some(function (options) {
                return options.itemck;
            });

        };
        $scope.all_checkI = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.clientlist, function (itm) {
                itm.itemck = toggleStatus;
            });
        };


        //==================================Item Report
        $scope.submitted = false;
        $scope.onreport = function () {
            if ($scope.myForm.$valid) {

                $scope.itemsArray = [];
                if ($scope.optionflag === null || $scope.optionflag === undefined) {
                    $scope.optionflag = "";
                }


                $scope.start_Date = $filter('date')($scope.startdate, "yyyy-MM-dd");
                $scope.end_Date = $filter('date')($scope.enddate, "yyyy-MM-dd");
                $scope.sDate = $scope.start_Date;
                $scope.eDate = $scope.end_Date;
                $scope.clientlistarray = [];
                $scope.term = $scope.termswise;

                data = {
                    "ISMMCLT_Id": $scope.IVRM_MI_Id.ismmclT_Id,
                    "termswise": $scope.termswise,
                    "fromdate": $scope.sDate,
                    "todate": $scope.eDate
                };

                apiService.create("Fee_Payment_Management/getreport", data).
                    then(function (promise) {
                        if (promise.getreportdetails.length > 0) {
                            $scope.getreportdetails = promise.getreportdetails;
                            $scope.presentCountgrid = $scope.getreportdetails.length;
                        }
                        else {
                            swal("No Record Found...!!");
                            $scope.getreportdetails = "";
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cancel = function () {
            $state.reload();
        };
        //======================================Print & Export to Excel
        $scope.exportToExcel = function (export_id0, export_id1) {
            if ($scope.term === 1) {
                var exportHref = Excel.tableToExcel(export_id0, 'printexcel_0');
                $timeout(function () {
                    location.href = exportHref;
                }, 100);
            }
            else {
                var exportHref1 = Excel.tableToExcel(export_id1, 'printexcel_1');
                $timeout(function () {
                    location.href = exportHref1;
                }, 100);
            }
          
        };
        $scope.printData = function () {
            if ($scope.term === 1) {

                var innerContents = document.getElementById("printItem_0").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            else {

                var innerContents1 = document.getElementById("printItem_1").innerHTML;
                var popupWinindow1 = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents1 + '</html>');
                popupWinindow1.document.close();
            }
        };


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
    }
})();