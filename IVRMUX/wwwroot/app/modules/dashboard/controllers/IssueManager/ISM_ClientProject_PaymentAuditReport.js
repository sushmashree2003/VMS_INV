(function () {
    'use strict';
    angular.module('app').controller('ISM_ClientProject_PaymentAuditReportController', ISM_ClientProject_PaymentAuditReportController)
    ISM_ClientProject_PaymentAuditReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', 'Excel','$timeout']
    function ISM_ClientProject_PaymentAuditReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, Excel, $timeout) {

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
        $scope.submitted = false;
        $scope.reprotd = true;
        $scope.dated = true;

        $scope.maxdate = new Date();

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("ISM_Master_ClientProject_Payment/loaddatareport", pageid).then(function (promise) {
                if (promise.clientlist !== null && promise.clientlist.length > 0) {
                    $scope.clientlist = promise.clientlist;
                }
            });
        };
        $scope.obj = {};

        $scope.onchangeclient = function () {
            $scope.dated = true;
            $scope.reprotd = true;
            $scope.obj.ToDate = null;
            $scope.obj.FromDate = null;
            $scope.getreportdetails = [];
        };

        $scope.onchangefromdate = function () {
            $scope.dated = false;
            $scope.reprotd = true;
            $scope.obj.ToDate = null;
            $scope.getreportdetails = [];
        };

        $scope.onchangetodate = function () {
            $scope.reprotd = true;
            $scope.getreportdetails = [];
        };

        $scope.getreport = function (dd) {
            if ($scope.myForm.$valid) {
                $scope.getreportdetails = [];

                var data = {
                    "ISMMCLT_Id": $scope.ISMMCLT_Id.ismmclT_Id,
                    "fromdate": new Date(dd.FromDate).toDateString(),
                    "todate": new Date(dd.ToDate).toDateString()
                };

                apiService.create("ISM_Master_ClientProject_Payment/getreport", data).then(function (promise) {
                    $scope.getreportdetails = promise.getreportdetails;
                    if ($scope.getreportdetails !== null && $scope.getreportdetails.length > 0) {
                        $scope.reprotd = false;
                        $scope.stdate_new = $filter('date')(dd.FromDate, 'dd-MM-yyyy');
                        $scope.end_new = $filter('date')(dd.ToDate, 'dd-MM-yyyy');
                        $scope.datedisplay = "From Date :" + $scope.stdate_new + " To Date : " + $scope.end_new;
                    } else {
                        $scope.getreportdetails = [];
                        $scope.reprotd = true;
                        swal("No Records Found");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.print = function () {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function (dd) {
            $scope.stdate = $filter('date')(dd.FromDate, 'dd-MM-yyyy');
            $scope.end = $filter('date')(dd.ToDate, 'dd-MM-yyyy');
            var excelname = 'PAYMENT SUBSCRIPTION REPORT ' + $scope.stdate + ' TO ' + $scope.end;
            excelname = excelname.toUpperCase() + '.xls';             
            var exportHref = Excel.tableToExcel('#printexcel', 'PAYMENT SUBSCRIPTION REPORT');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();