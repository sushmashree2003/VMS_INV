(function () {
    'use strict';
    angular.module('app').controller('ConCallTaskLopReportController', ConCallTaskLopReportController)
    ConCallTaskLopReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'Excel', 'apiService', '$timeout', '$http', 'superCache', '$filter', '$q', '$window']
    function ConCallTaskLopReportController($rootScope, $scope, $state, $location, Flash, appSettings, Excel, apiService, $timeout, $http, superCache, $filter, $q, $window) {
        var paginationformasters;
        var HostName = location.host;
        var copty;
        $scope.obj = {}
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
        $scope.objques = {};
        $scope.gridflag = false;
        // Master Type
        //$scope.onloadmonths = function () {
        //    $scope.gridflag = true;
        //    var pageid = 2;
        //    apiService.getURI("ModuleWiseFeedbackReport/OnLoadClient", pageid).then(function (promise) {
        //        if (promise !== null) {
        //            $scope.hr_masteryear = promise.hr_masteryear;
        //        } 
        //    });

        //};
        $scope.onloadmonths = function () {
            var pageid = 2;
            apiService.getURI("ISM_ApprovalReport/getonlodatdata", pageid).then(function (promise) {

                if (promise !== null) {
                    $scope.months = promise.monthnames;
                    $scope.years = promise.masteryear;
                    


                }
                else {
                    swal("No Data Found !");
                }
            })
        };
        $scope.clearayyary = function () {
            $scope.vmsmoduleratingreport = [];
        }

        $scope.submitted = false;
        $scope.getreport = function (obj) {
          
            var fromdate1 = "";
            var todate1 = "";


            if ($scope.optionflag11 == 'Datewise') {
                $scope.r_month = '';
                $scope.f_Year = 0;
                fromdate1 = new Date($scope.fromdate).toDateString();
                todate1 = new Date($scope.todate).toDateString();
            }

            else {
                $scope.f_Year = $scope.f_Year;
                $scope.r_month = $scope.r_month;
                fromdate1 = new Date().toDateString();
                todate1 = new Date().toDateString();

            }
            if ($scope.myForm.$valid) {
               
                

                var data = {
                    "Fromdate": fromdate1,
                    "Todate": todate1,
                    "year": $scope.f_Year,
                    "monthname": $scope.r_month,
                    "conflag": $scope.optionflag11,
                };

                apiService.create("ISM_ApprovalReport/getconcalllopdetails", data).then(function (promise) {

                    if (promise.griddata.length > 0 && promise.griddata != null) {
                        $scope.concalldata = promise.griddata;


                    }
                    else {
                        swal("No Records Found");
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

        $scope.Viewdetails = function () {
            $window.location.href = 'http://' + HostName + '/#/app/ISM_Employee_DailyReport_Concall_LOP/12';

            
        };
      

        $scope.printData = function () {
            var innerContents = document.getElementById("concall").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();

            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();

        }
        //$scope.exportToExcel = function (tabel1) {
        //    var exportHref = Excel.tableToExcel(tabel1, 'sheet name');
        //    $timeout(function () { location.href = exportHref; }, 100);
        //}
        $scope.exportToExcel = function (tabel1) {
            var excelnamemain = "CONCALLTASKLOP_REPORT";
            var printSectionId = '#concall';
            excelnamemain = excelnamemain + '' + '.xls';
            var exportHref = Excel.tableToExcel(printSectionId, 'CONCALLTASK LOP REPORT');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelnamemain;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);

        };
        $scope.Clear = function () {
            $state.reload();
        };
    

    }
})();