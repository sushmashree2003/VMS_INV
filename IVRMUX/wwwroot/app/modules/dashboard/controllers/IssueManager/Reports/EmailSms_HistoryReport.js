(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmailSms_HistoryReportController', EmailSms_HistoryReportController);
    EmailSms_HistoryReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function EmailSms_HistoryReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache) {
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 10;
        }
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        $scope.submitted = false;
        $scope.startdate = new Date();
        $scope.enddate = new Date();
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.searchValue = "";
            var pageid = 2;
            apiService.getURI("EmailSms_HistoryReport/getdetails", pageid).then(function (promise) {
                $scope.getname = promise.institution;
            });
        };
        $scope.Dropdown_State = function () {
            if ($scope.MI_Id !== null && $scope.MI_Id !== "") {
                var data = {
                    "MI_Id": $scope.MI_Id
                };
                apiService.create("EmailSms_HistoryReport/getmodule", data).then(function (promise) {
                    $scope.grneTLIst = promise.module;
                });
            }
            else {
                swal("Please Select Institution");
                $state.reload();
            }
        };
        $scope.GetReport = function () {
            $scope.uploadarray = [];
            if ($scope.myForm.$valid) {
                if ($scope.grneTLIst.length > 0) {
                    angular.forEach($scope.grneTLIst, function (itm) {
                        if (itm.selected == true) {
                            $scope.uploadarray.push({
                                IVRMMM_ModuleName: itm.ivrmM_ModuleName,
                                IVRMM_Id: itm.ivrmM_Id,                               
                            });
                        }
                    });
                }
                var data = {
                    "MI_Id": $scope.MI_Id,
                    "FromDate": new Date($scope.startdate).toDateString(),
                    "ToDate": new Date($scope.enddate).toDateString(),
                    "uploadarray": $scope.uploadarray,
                    "optionflag": $scope.optionflag
                };
                apiService.create("EmailSms_HistoryReport/getReport", data).then(function (promise) {
                    if (promise.getReport != null && promise.getReport.length != "") {
                        $scope.get_Report = promise.getReport;
                        $scope.presentCountgrid = $scope.get_Report.length;
                    }
                    else {
                        swal("Record Not Found");
                    }
                });
            }
        };
        $scope.exportToExcel = function () {            
                var exportHref = Excel.tableToExcel(printExcel, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);           
        };
        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printExcel").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //===========Clear Field
        $scope.Clear = function () {
            $state.reload();
        };
        $scope.all_checkCCCC = function () {
            var ivrmM_Id = $scope.ivrmM_Id;
            var count = 0;
            angular.forEach($scope.grneTLIst, function (itm) {
                itm.selected = ivrmM_Id;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }
        $scope.isOptionsRequiredtwo = function () {
            return !$scope.grneTLIst.some(function (options) {
                return options.selected;
            });
        }
        $scope.togchkbxCCCC = function () {
            $scope.ivrmM_Id = $scope.grneTLIst.every(function (options) {
                return options.selected;
            });
        }
        $scope.onradiochange = function () {
            $scope.get_Report = [];
        };
    }
})();

