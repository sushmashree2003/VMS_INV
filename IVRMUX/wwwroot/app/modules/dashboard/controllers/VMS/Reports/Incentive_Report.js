(function () {
    'use strict';
    angular
        .module('app')
        .controller('Incentive_ReportController', Incentive_ReportController);

    Incentive_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce', '$timeout', 'Excel'];
    function Incentive_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce, $timeout, Excel) {

        //=============image
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
       
        $scope.typerdochange = function () {
            $scope.get_employeereport = [];
            $scope.get_leadreport = [];
            if ($scope.typeflag == "Employee") {
            }
            else {
                $scope.typeflag == "Lead";
            }
        };

        //Report        
        $scope.submitted = false;
        $scope.showreport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {               

                $scope.fromdate = "";
                $scope.todate = "";
                $scope.fromdate = $filter('date')($scope.startdate, 'yyyy-MM-dd');
                $scope.todate = $filter('date')($scope.enddate, 'yyyy-MM-dd');
 
                var data = {
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate,
                    "reporttype": $scope.typeflag
                }
                apiService.create("Sales_Lead/onreport/", data).then(function (promise) {
                    if (promise.reporttype == "Employee") {
                        if (promise.get_employeereport != null && promise.get_employeereport.length > 0) {
                            $scope.get_employeereport = promise.get_employeereport;
                            $scope.Grid_view = true;
                            $scope.employee = true;
                            $scope.lead = false;
                        }
                        else {
                            swal("No Records Found");
                            $scope.Grid_view = false;
                        }
                    }
                    else if (promise.reporttype == "Lead") {
                        if (promise.get_leadreport != null && promise.get_leadreport.length > 0) {
                            $scope.get_leadreport = promise.get_leadreport;
                            $scope.Grid_view = true;
                            $scope.employee = false;
                            $scope.lead = true;
                        }
                        else {
                            swal("No Records Found");
                            $scope.Grid_view = false;
                        }
                    }

                });
            }
            else {
                $scope.submitted = true;
            }
        };


        //mainmodelclick
        $scope.mainmodelclick = function (id) {

            var data = {
                "ISMSLE_Id": id              
            };
            apiService.create("Sales_Lead/getleaddetail", data).
                then(function (promise) {

                    $scope.getleaddetails = promise.getleaddetails;                  
                });
            $('#myModalG').modal('show');
        };


        //Print
        $scope.printData = function () {
            if ($scope.typeflag == "Employee") {
                if ($scope.get_employeereport !== null && $scope.get_employeereport.length > 0) {
                    var innerContents = document.getElementById("printSectionIdEmployee").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                    $state.reload();
                }
            }
            else if ($scope.typeflag == "Lead") {
                if ($scope.get_leadreport !== null && $scope.get_leadreport.length > 0) {
                    var innerContents = document.getElementById("printSectionIdLead").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                    $state.reload();
                }
            }
        }

        //Export To Excel
        $scope.exportToExcel = function () {
            if ($scope.typeflag == "Employee") {
                if ($scope.get_employeereport !== null && $scope.get_employeereport.length > 0) {
                    var exportHref = Excel.tableToExcel(tableemployee, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
            }
            else if ($scope.typeflag == "Lead") {
                if ($scope.get_leadreport !== null && $scope.get_leadreport.length > 0) {
                    var exportHref = Excel.tableToExcel(tablelead, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
            }
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
        $scope.searchValue = '';

    }

})();