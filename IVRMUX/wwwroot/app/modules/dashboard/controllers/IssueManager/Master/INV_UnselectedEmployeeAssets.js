(function () {
    'use strict';
    angular.module('app').controller('INV_UnselectedEmployeeAssetsController', INV_UnselectedEmployeeAssetsController);

    INV_UnselectedEmployeeAssetsController.$inject = ['$compile', '$rootScope', '$scope', '$http', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$timeout', 'Excel'];

    function INV_UnselectedEmployeeAssetsController($compile, $rootScope, $scope, $state, $http, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, $timeout, Excel) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.searchValue = "";
        $scope.sortKey = 'UnselectedDate';
        $scope.reverse = true;

        $scope.unselectedassets = [];
        $scope.tempunselectedassets = [];
        $scope.totalUnselectedCount = 0;
        $scope.totalUnselectedAmount = 0;
        $scope.totalUniqueLocations = 0;
        $scope.totalUniqueEmployees = 0;

        // Load all unselected employee assets
        $scope.loaddata = function () {
            $scope.unselectedassets = [];
            $scope.tempunselectedassets = [];
            $scope.totalUnselectedCount = 0;
            $scope.totalUnselectedAmount = 0;
            $scope.totalUniqueLocations = 0;
            $scope.totalUniqueEmployees = 0;

            var data = {
                "Flag": "Unselected"
            };

            apiService.create("INV_LocationDashboard/GetUnselectedEmployeeAssets", data).then(function (promise) {
                if (promise && promise.unselectedAssetList && promise.unselectedAssetList.length > 0) {
                    $scope.unselectedassets = promise.unselectedAssetList;
                    $scope.tempunselectedassets = angular.copy(promise.unselectedAssetList);

                    $scope.totalUnselectedCount = $scope.unselectedassets.length;

                    var totalAmt = 0;
                    var locMap = {};
                    var empMap = {};

                    angular.forEach($scope.unselectedassets, function (item) {
                        var rate = parseFloat(item.INVTGRN_PurchaseRate) || 0;
                        totalAmt += rate;

                        if (item.INVMLO_LocationRoomName) {
                            locMap[item.INVMLO_LocationRoomName] = true;
                        }
                        if (item.EmployeeName) {
                            empMap[item.EmployeeName] = true;
                        }
                    });

                    $scope.totalUnselectedAmount = totalAmt;
                    $scope.totalUniqueLocations = Object.keys(locMap).length;
                    $scope.totalUniqueEmployees = Object.keys(empMap).length;
                } else {
                    $scope.unselectedassets = [];
                    $scope.totalUnselectedCount = 0;
                    $scope.totalUnselectedAmount = 0;
                    $scope.totalUniqueLocations = 0;
                    $scope.totalUniqueEmployees = 0;
                }
            });
        };

        // Sorting
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.sortKey === propertyName) ? !$scope.reverse : false;
            $scope.sortKey = propertyName;
        };

        // Total Gross Amount for filtered list
        $scope.TotalgrossAmount = function () {
            var total = 0;
            var list = $filter('filter')($scope.unselectedassets, $scope.searchValue);
            if (list != null && list.length > 0) {
                for (var i = 0; i < list.length; i++) {
                    total += parseFloat(list[i].INVTGRN_PurchaseRate) || 0;
                }
            }
            return total;
        };

        // Preview Image Modal
        $scope.previewimg_new = function (img) {
            $scope.imagepreview = img;
            $('#imagemodal').modal('show');
        };

        // Export to Excel
        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'Unselected_Employee_Assets');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = 'Unselected_Employee_Assets_' + $filter('date')(new Date(), 'ddMMyyyy_HHmmss') + '.xls';
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        // Print Data
        $scope.printData = function () {
            var innerContents = document.getElementById("printSection").innerHTML;
            var popupWin = window.open('', '_blank', 'width=1100,height=700,scrollbars=no,menubar=no,toolbar=no,location=no,status=no,titlebar=no');
            popupWin.document.open();
            popupWin.document.write('<html><head><title>Unselected Employee Assets Report</title><link rel="stylesheet" type="text/css" href="css/bootstrap.min.css" /><link rel="stylesheet" type="text/css" href="css/style.css" /><style>table { width: 100%; border-collapse: collapse; } th, td { border: 1px solid #ddd; padding: 8px; text-align: center; } th { background-color: #0066c0; color: white; } .badge { border: 1px solid #dc3545; color: #dc3545; padding: 3px 6px; font-weight: bold; }</style></head><body onload="window.print()">' + innerContents + '</body></html>');
            popupWin.document.close();
        };

    }
})();
