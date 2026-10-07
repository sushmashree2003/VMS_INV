(function () {
    'use strict';
    angular.module('app').controller('vmsmodulefeedbackreportController', ModuleWiseFeedbackReportController)
    ModuleWiseFeedbackReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'Excel', 'apiService', '$timeout', '$http', 'superCache', '$filter', '$q']
    function ModuleWiseFeedbackReportController($rootScope, $scope, $state, $location, Flash, appSettings, Excel, apiService, $timeout, $http, superCache, $filter, $q) {
        var paginationformasters;
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
        $scope.onloadmonths = function () {
            $scope.gridflag = true;
            var pageid = 2;
            apiService.getURI("ModuleWiseFeedbackReport/OnLoadClient", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.hr_masteryear = promise.hr_masteryear;
                } 
            });
            $scope.months = [
                {
                    "abbreviation": "January",
                    "name": "January"
                },
                {
                    "abbreviation": "January",
                    "name": "January"
                },
                {
                    "abbreviation": "March",
                    "name": "March"
                },
                {
                    "abbreviation": "April",
                    "name": "April"
                },
                {
                    "abbreviation": "May",
                    "name": "May"
                },
                {
                    "abbreviation": "June",
                    "name": "June"
                },
                {
                    "abbreviation": "Jul",
                    "name": "July"
                },
                {
                    "abbreviation": "August",
                    "name": "August"
                },
                {
                    "abbreviation": "September",
                    "name": "September"
                },
                {
                    "abbreviation": "October",
                    "name": "October"
                },
                {
                    "abbreviation": "November",
                    "name": "November"
                },
                {
                    "abbreviation": "December",
                    "name": "December"
                }
            ]
        };
        $scope.clearayyary = function () {
            $scope.vmsmoduleratingreport = [];
        }

        $scope.getreport = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "r_month": $scope.r_month,
                    "r_year": $scope.hreS_Year,
                    "optionflag": $scope.optionflag
                };
                apiService.create("ModuleWiseFeedbackReport/getvmsmodulefeedbackreport", data).then(function (promise) {
                    if (promise.vmsmoduleratingreport.length > 0 && promise.vmsmoduleratingreport != null) {
                        $scope.vmsmoduleratingreport = promise.vmsmoduleratingreport;
                    }
                    else {
                        swal("No Records Found For Selected Month and year")
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

        $scope.printData = function () {
            var innerContents = document.getElementById("modulefeedback").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();

            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();

        }
        $scope.exportToExcel = function (tabel1) {
            var exportHref = Excel.tableToExcel(tabel1, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }
        $scope.Clear = function () {
            $state.reload();
        };
    }
})();