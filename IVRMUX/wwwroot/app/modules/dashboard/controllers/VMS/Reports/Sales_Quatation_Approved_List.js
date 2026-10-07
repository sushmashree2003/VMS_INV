(function () {
    'use strict';
    angular
        .module('app')
        .controller('SalesDrBlockedController', SalesDrBlockedController);

    SalesDrBlockedController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce', '$timeout', 'Excel'];
    function SalesDrBlockedController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce, $timeout, Excel) {


        //=============image
        $scope.SalesQuatationReport = [];
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;



        $scope.searchValue = '';
        $scope.searchValue1 = '';
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.itemsPerPage1 = 10;
        $scope.currentPage1 = 1;

        $scope.grid_view = false;
        $scope.submitted = false;
        // $scope.leadlist = [];

        $scope.onchageradiobtn = function () {

            $scope.SalesQuatationReport  = [];

        }

        $scope.submitted = false;
        $scope.showreport = function () {
            $scope.SalesQuatationReport = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.fromdate = "";
                $scope.todate = "";
                $scope.fromdate = $filter('date')($scope.startdate, 'yyyy-MM-dd');
                $scope.todate = $filter('date')($scope.enddate, 'yyyy-MM-dd');
                var data = {
                
                    "flag": $scope.all2,
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate,
                    //"EmpType": "",
                    //"Report_Type": $scope.all2
                }
                apiService.create("SalesDrBlocked/SalesQuatationReport/", data).
                    then(function (promise) {
                        if (promise.salesQuatationReport.length > 0 && promise.salesQuatationReport != null && promise.salesQuatationReport != undefined) {
                            $scope.SalesQuatationReport = promise.salesQuatationReport;

                            //for displaying grid
                            $scope.bomids = [];

                            angular.forEach($scope.SalesQuatationReport, function (dev) {
                                if ($scope.bomids.length === 0) {
                                    $scope.bomids.push(dev);
                                } else if ($scope.bomids.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.bomids, function (emp) {
                                        if (emp.ISMSLE_Id === dev.ISMSLE_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.bomids.push(dev
                                        );
                                    }
                                }
                            });


                            console.log($scope.bomids);

                            angular.forEach($scope.bomids, function (ddd) {
                                $scope.templist = [];

                                angular.forEach($scope.SalesQuatationReport, function (dd) {
                                    if (dd.ISMSLE_Id === ddd.ISMSLE_Id) {

                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.datadetails = $scope.templist;

                            });


                            console.log($scope.bomids);

                           
                        }
                        else {
                            swal('No Data Found!!!');
                            $state.reload();
                        }

                    })
            }
            else {
                $scope.submitted = true;
            }
        };
        //Print
        $scope.printData = function () {
            var innerContents = document.getElementById("tableexcell").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


        $scope.exportToExcel = function (tableemployee) {
            var exportHref = Excel.tableToExcel(tableemployee, 'Sales Quatation Status Report');
            $timeout(function () { location.href = exportHref; }, 100);
        }




        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }

})();