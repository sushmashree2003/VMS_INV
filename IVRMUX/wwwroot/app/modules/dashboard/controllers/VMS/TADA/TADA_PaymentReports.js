(function () {
    'use strict';
    angular
        .module('app')
        .controller('TADA_PaymentReportsController', TADA_PaymentReportsController)
    TADA_PaymentReportsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function TADA_PaymentReportsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {
        $scope.currentPage = 1;
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        else {
            paginationformasters = 10;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        //  $scope.imgname = logopath;
        $scope.searchValue = "";
        $scope.report = false;
        $scope.DisaBleemp = true;
        $scope.chk_All = true;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.onpageload = function () {
            var pageid = 1;
            apiService.getURI("TADA_PaymentReport/getalldata", pageid).
                then(function (promise) {
                    $scope.get_emp = promise.get_emp;
                })
        }
        $scope.All_Chk = function () {
            if ($scope.chk_All === true) {
                $scope.DisaBleemp = true;
            }
            else {
                $scope.DisaBleemp = false;
            }
        }
        $scope.otherdaydatechange = function () {
            $scope.get_Report = [];
        };
        $scope.submitted = false;
        //====drodowncity====
        $scope.getReport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.get_Report = [];
                $scope.arrayuserEmp = [];              
                angular.forEach($scope.get_emp, function (comp) {
                    if (comp.selected == true) {
                        $scope.arrayuserEmp.push({ HRME_IdS: comp.hrmE_Id });
                    }
                });
                var data = {
                    "RDO_PUNCH": $scope.rdopunch,                    
                    "FromDate": new Date($scope.startdate).toDateString(),
                    "ToDate": new Date($scope.enddate).toDateString(),
                    "HRME_Id_List": $scope.arrayuserEmp
                }
                apiService.create("TADA_PaymentReport/GetReport", data).then(function (promise) {
                    if (promise.getReport.length != null && promise.getReport.length > 0) {                         
                        $scope.get_Report = promise.getReport;
                        $scope.presentCountgrid = $scope.get_Report.length;
                    }
                    else {
                        swal("Record Not found !.");
                    }
                    $scope.imagenew = promise.mI_Logo;
                });
            }
        };
        $scope.exportToExcel = function () {
            var exportHref = Excel.tableToExcel(PayAdavance, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };
        $scope.printData = function () {
            var innerContents = document.getElementById("PayAdavance").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            // $scope.imgdiv = true;
            $scope.headerimg = true;
            popupWinindow.document.close();
        };
        $scope.clear = function () {            
            $state.reload();
        }
        $scope.isOptionsRequired = function () {
            return !$scope.get_emp.some(function (options) {
                return options.selected;
            });
        }
        $scope.all_checkC = function () {
            var checkStatus = $scope.usercheckC;
            var count = 0;
            angular.forEach($scope.get_emp, function (itm) {
                itm.selected = checkStatus;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }
        $scope.togchkbxC = function () {
            $scope.usercheckC = $scope.get_emp.every(function (options) {
                return options.selected;
            });
        }
    }
})();

