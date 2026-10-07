(function () {
    'use strict';
    angular.module('app').controller('CheckBankBalanceTransactionController', CheckBankBalanceTransactionController)
    CheckBankBalanceTransactionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', 'Excel','$timeout']
    function CheckBankBalanceTransactionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, Excel, $timeout) {
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag === "CLIINV") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }
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
        $scope.VBABRS_Dates = false;
        $scope.loaddata = function () {
            var pageid = 2;
            $scope.MI_Id = "";
            $scope.HRMBD_Id = "";
            apiService.getURI("CheckBankBalance/loaddata", pageid).then(function (promise) {
                $scope.allcompany = promise.allcompany;
            });
        };
        $scope.oncmpchange = function () {
            $scope.HRMBD_Id = "";
            var data = {
                "MI_Id": $scope.MI_Id
            };
            apiService.create("CheckBankBalance/companychange", data).then(function (promise) {
                $scope.banklist = promise.banklist;

            });
        }
        $scope.exportToExcel = function (export_id) {

            var exportHref = Excel.tableToExcel(export_id, 'sheet name');
            $timeout(function () {
                location.href = exportHref;
            }, 100);

        };
        //
        $scope.bankchange = function () {
            $scope.trasactionReport = "";
            var data = {
                "HRMBD_Id": $scope.HRMBD_Id,
                "MI_Id": $scope.MI_Id
            };
            apiService.create("CheckBookEntry/getbom", data).then(function (promise) {              
                $scope.BanKBalance = promise.projectlist[0].vbabrS_BankBalalnce;                
            });
        }
        $scope.submitted = false;
        $scope.savedata = function () {
            var INVMS_Id = 0;
            if ($scope.banklist != null && $scope.banklist.length > 0) {
                for (var i = 0; i < $scope.banklist.length; i++) {
                    if ($scope.HRMBD_Id == $scope.banklist[i].hrmbD_Id) {
                        $scope.BankName = $scope.banklist[i].hrmbD_BankName;
                        $scope.AccountNumber = $scope.banklist[i].hrmbD_BankAccountNo;
                    }
                }
            }
            var fromdate = "";
            var todate = "";
            if ($scope.fromdate != null && $scope.todate != null && $scope.fromdate != "" && $scope.todate != "") {
                fromdate = new Date($scope.fromdate).toDateString();
                todate = new Date($scope.todate).toDateString()
            }

            if ($scope.myForm.$valid) {
                var data = {
                    "MI_Id": $scope.MI_Id,
                    "HRMBD_Id": $scope.HRMBD_Id,
                    "INVMS_Id": INVMS_Id,
                    "fromdate": fromdate,
                    "todate": todate
                };
                apiService.create("CheckBankBalance/getReporttwo", data).then(function (promise) {
                    if (promise.trasactionReport != null && promise.trasactionReport.length > 0) {
                        $scope.trasactionReport = promise.trasactionReport;
                        $scope.imagenew = promise.fmI_Name;
                        
                    }
                    else {
                        swal("Record Not Found");
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
        $scope.Clearid = function () {
            $state.reload();
        };
        //excek
        $scope.printData = function () {
            var name = "printExcel";
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
        //excel export

    }
})();
