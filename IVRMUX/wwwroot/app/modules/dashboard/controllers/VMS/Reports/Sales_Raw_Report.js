(function () {
    'use strict';
    angular
        .module('app')
        .controller('Sales_Raw_ReportController', Sales_Raw_ReportController);

    Sales_Raw_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce', '$timeout', 'Excel'];
    function Sales_Raw_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce, $timeout, Excel) {

        //=============image
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.searchValue = "";

        //Report        
        $scope.startdate = new Date(); $scope.enddate = new Date();
        $scope.loaddata = function () {   
            $scope.submitted = true;
            $scope.showreport();     
        };
        $scope.submitted = false;
        $scope.showreport = function () {
            $scope.submitted = true; $scope.LeadCount = 0; $scope.Admincount = 0; $scope.Telcount = 0; $scope.AdminReject = 0; $scope.TelcountReject = 0;
            $scope.leaddropdown = []; var LeadCount = 0; var Admincount = 0; var Telcount = 0; var AdminReject = 0; var TelcountReject = 0;
            var data = {
                "FROMDATE": new Date($scope.startdate).toDateString(),
                "TODATE": new Date($scope.enddate).toDateString(),
            }
            apiService.create("Sales_Lead/get_leads/", data).then(function (promise) {
                if (promise.leaddropdown != null && promise.leaddropdown.length > 0) {
                    $scope.leaddropdown = promise.leaddropdown;
                    angular.forEach($scope.leaddropdown, function (em) {
                        LeadCount = (Number(LeadCount) + (Number(em.LeadCount)));
                        Admincount = (Number(Admincount) + (Number(em.Admincount)));
                        Telcount = (Number(Telcount) + (Number(em.Telcount)));
                        AdminReject = (Number(AdminReject) + (Number(em.AdminReject)));
                        TelcountReject = (Number(TelcountReject) + (Number(em.TelcountReject)));

                    });
                    $scope.LeadCount = LeadCount;
                    $scope.Admincount = Admincount;
                    $scope.Telcount = Telcount;
                    $scope.AdminReject = AdminReject;
                    $scope.TelcountReject = TelcountReject;
                }
                else {
                    swal("No Records Found");

                }
            });
        };
    //Print
        $scope.printData = function () {
            var innerContents = document.getElementById("employee").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.exportToExcels = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };

        //Export To Excel
        $scope.exportToExcel = function (tableemployee) {
            var exportHref = Excel.tableToExcel(tableemployee, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }

        $scope.clearid = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        
      
        $scope.DemoGiven = function (obja, sub) {
            $scope.Sales_Raw_Report_OnclickGridData =[];
            $scope.submitted = true;
            var data = {
                "FROMDATE": new Date($scope.startdate).toDateString(),
                "TODATE": new Date($scope.enddate).toDateString(),
                "HRME_Id": obja.HRME_Id,
                "flag1": sub,
                "flag2": "Ind",
            }
           
            apiService.create("Sales_Lead/Sales_Raw_Report_OnclickGridData/", data).then(function (promise) {
                if (promise.sales_Raw_Report_OnclickGridData != null && promise.sales_Raw_Report_OnclickGridData.length > 0) {
                    $scope.Sales_Raw_Report_OnclickGridData = promise.sales_Raw_Report_OnclickGridData;
                    $('#myModal1').modal('show');
                }
                else {
                    swal("No Records Found");
                    $scope.Sales_Raw_Report_OnclickGridData = [];

                }
            });
        };


        $scope.DemoGivenTotal = function (sub) {
            $scope.Sales_Raw_Report_OnclickGridData = [];
            $scope.submitted = true;
            var data = {
                "FROMDATE": new Date($scope.startdate).toDateString(),
                "TODATE": new Date($scope.enddate).toDateString(),
                "HRME_Id": 0,
                "flag1": sub,
                "flag2": "Total"
            }

            apiService.create("Sales_Lead/Sales_Raw_Report_OnclickGridData/", data).then(function (promise) {
                if (promise.sales_Raw_Report_OnclickGridData != null && promise.sales_Raw_Report_OnclickGridData.length > 0) {
                    $scope.Sales_Raw_Report_OnclickGridData = promise.sales_Raw_Report_OnclickGridData;
                    $('#myModal1').modal('show');
                }
                else {
                    swal("No Records Found");
                    $scope.Sales_Raw_Report_OnclickGridData = [];

                }
            });
        };
      
        //export start
        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };

        $scope.exportToExcels = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };


    }

})();