(function () {
    'use strict';
    angular.module('app').controller('DailyReport_RejectedSummary_ReportController', DailyReport_RejectedSummary_ReportController);
    DailyReport_RejectedSummary_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout'];
    function DailyReport_RejectedSummary_ReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        $scope.submitted = false;

        $scope.maxdate = new Date();

        $scope.loaddata = function () {
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            $scope.dateflag = 0;
            $scope.start_Date = new Date();
            $scope.end_Date = new Date();


            var pageid = 2;
            apiService.getURI("ISM_ApprovalReport/getdetails", pageid).then(function (promise) {
                $scope.hide_n = false;
                if (promise.ivrmrT_Role === 'admin') {
                    $scope.depdes = true;
                    $scope.get_userEmplist = promise.get_userEmplist;
                }
                else {
                    $scope.depdes = false;
                    $scope.get_userEmplist = promise.get_userEmplist;
                }
            });
        };

        $scope.all_checkuu = function (ack) {
            $scope.checkalluu = ack;
            var toggleStatus = $scope.checkalluu;
            angular.forEach($scope.get_userEmplist, function (uem) {
                uem.empck = toggleStatus;
            });
            $scope.onEmployeechange();
        };

        $scope.togchkbxuu = function () {
            $scope.checkalluu = $scope.get_userEmplist.every(function (options) {
                return options.empck;
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.get_userEmplist.some(function (options) {
                return options.empck;
            });
        };
        $scope.submitted = false;
        $scope.SummaryReport = function () {
            $scope.plannerid = [];
            if ($scope.myForm.$valid) {
                $scope.Rejected_SummaryReport = [];
                $scope.temp_Planner = [];
                $scope.searchValue = "";
                $scope.get_emp_dailyreport = "";

                $scope.arrayuserEmp = [];

                angular.forEach($scope.get_userEmplist, function (em) {
                    if (em.empck === true) {
                        $scope.arrayuserEmp.push(em);
                    }
                });

                var data = {
                    arrayuserEmp: $scope.arrayuserEmp,
                    "Fromdate": new Date($scope.start_Date),
                    "Todate": new Date($scope.end_Date),


                };
                apiService.create("ISM_ApprovalReport/SummaryReport", data).then(function (promise) {


                    if (promise.rejected_SummaryReport.length > 0 && promise.rejected_SummaryReport != null) {
                        $scope.Rejected_SummaryReport = promise.rejected_SummaryReport;

                    }
                    else {
                        swal("No Records..!")
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

        $scope.Clear = function () {
            $state.reload();
        };

        $scope.printData = function () {
            $scope.hide_n = false;
            var innerContents = "";
            innerContents = document.getElementById("printP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };


    }
})();