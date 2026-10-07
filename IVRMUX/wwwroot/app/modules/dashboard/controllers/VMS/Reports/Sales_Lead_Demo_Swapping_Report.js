(function () {
    'use strict';
    angular
        .module('app')
        .controller('SalesDrBlockedController', SalesDrBlockedController);

    SalesDrBlockedController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce', '$timeout', 'Excel'];
    function SalesDrBlockedController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce, $timeout, Excel) {

        $scope.Sales_Negotation_Detail_Report = [];
        //=============image
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.maxDatemf = new Date();
        //loading start

        $scope.athflag = false;

        $scope.togchkbxdept = function (groupType) {
            $scope.ISMSLE_Id = $scope.leadlist.every(function (itm) {
                return itm.selected;
            });


        };
        $scope.all_checkdept = function (ISMSLE_Id) {
            var toggleStatus = ISMSLE_Id;
            angular.forEach($scope.leadlist, function (itm) {
                itm.selected = toggleStatus;
            });

        };
        $scope.togchkbxdept1 = function (groupType) {
            $scope.HRMR_Id = $scope.roomlist.every(function (itm1) {
                return itm1.selected;
            });


        };
        $scope.all_checkdept1 = function (HRMR_Id) {
            var toggleStatus1 = HRMR_Id;
            angular.forEach($scope.roomlist, function (itm1) {
                itm1.selected = toggleStatus1;
            });
        };


        $scope.searchValue = '';
        $scope.searchValue1 = '';
        $scope.itemsPerPage = 15;
        $scope.currentPage = 1;


        $scope.grid_view = false;
        $scope.submitted = false;
        // $scope.leadlist = [];



        $scope.submitted = false;
        $scope.showreport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.Sales_Negotation_Detail_Report = [];
                $scope.fromdate = "";
                $scope.todate = "";
                $scope.fromdate = $filter('date')($scope.startdate, 'yyyy-MM-dd');
                $scope.todate = $filter('date')($scope.enddate, 'yyyy-MM-dd');
               
                var data = {
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate,
                }
                apiService.create("SalesDrBlocked/Sales_Lead_Demo_Swapping_Report/", data).
                    then(function (promise) {
                        if (promise.sales_Lead_Demo_Swapping_Report.length > 0 && promise.sales_Lead_Demo_Swapping_Report != null) {
                            $scope.Sales_Lead_Demo_Swapping_Report = promise.sales_Lead_Demo_Swapping_Report;
                        }
                        else {
                            swal('No Data Found!!!')
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
            var exportHref = Excel.tableToExcel(tableemployee, 'Sales Lead Demo Swapping Report');
            $timeout(function () { location.href = exportHref; }, 100);
        }



        $scope.clearid = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }

})();