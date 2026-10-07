(function () {
    'use strict';
    angular
        .module('app')
        .controller('TrackerReportController', TrackerReportController);

    TrackerReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'Excel', '$timeout', 'superCache', 'uiGridGroupingConstants'];
    function TrackerReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, Excel, $timeout, superCache, uiGridGroupingConstants) {

        $scope.EmployeeDis = false;

        $scope.monthlyactiveemployeegrid = {
            showGridFooter: true,
            showColumnFooter: true,
            enableFiltering: false,
            enableGridMenu: false,
            enableColumnMenus: false,
            treeRowHeaderAlwaysVisible: false,
            columnDefs: [
                { name: 'year', displayName: 'year', grouping: { groupPriority: 0 }, sort: { priority: 0, direction: 'asc' }, cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },

                { name: 'MonthName', displayName: 'Month Name', grouping: { groupPriority: 1 }, sort: { priority: 1, direction: 'asc' }, cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                {
                    name: 'ActEmpcount', displayName: ' Employee Count', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                }
            ],
            exporterMenuPdf: false,

            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            },
            gridMenuCustomItems: [{
                title: 'Monthly Active Employees',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'all', 'all');
                },
                order: 110
            },
            {
                title: 'Export visible data as EXCEL',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'visible', 'visible');
                },
                order: 111
            }
            ]
        };

        $scope.monthlyleftemployeegrid = {
            showGridFooter: true,
            showColumnFooter: true,
            enableFiltering: false,
            enableGridMenu: false,
            enableColumnMenus: false,
            treeRowHeaderAlwaysVisible: false,
            columnDefs: [
                { name: 'year', displayName: 'year', grouping: { groupPriority: 0 }, sort: { priority: 0, direction: 'asc' }, cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },

                { name: 'MonthName', displayName: 'Month Name', grouping: { groupPriority: 1 }, sort: { priority: 1, direction: 'asc' }, cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                {
                    name: 'LeftEmpcount', displayName: ' Employee Count', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                }
            ],
            exporterMenuPdf: false,

            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            },
            gridMenuCustomItems: [{
                title: 'Monthly Left Employees',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'all', 'all');
                },
                order: 110
            },
            {
                title: 'Export visible data as EXCEL',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'visible', 'visible');
                },
                order: 111
            }
            ]
        };

        $scope.monthlyloanadvanceemployeegrid = {
            showGridFooter: true,
            showColumnFooter: true,
            enableFiltering: false,
            enableGridMenu: false,
            enableColumnMenus: false,
            treeRowHeaderAlwaysVisible: false,
            columnDefs: [
                { name: 'year', displayName: 'year', grouping: { groupPriority: 0 }, sort: { priority: 0, direction: 'asc' }, cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },

                { name: 'MonthName', displayName: 'Month Name', grouping: { groupPriority: 1 }, sort: { priority: 1, direction: 'asc' }, cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                {
                    name: 'LAEmpcount', displayName: ' Employee Count', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                }
            ],
            exporterMenuPdf: false,

            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            },
            gridMenuCustomItems: [{
                title: 'Monthly Loan Employees',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'all', 'all');
                },
                order: 110
            },
            {
                title: 'Export visible data as EXCEL',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'visible', 'visible');
                },
                order: 111
            }
            ]
        };

        $scope.monthlymodeofpaymentgrid = {
            showGridFooter: true,
            showColumnFooter: true,
            enableFiltering: false,
            enableGridMenu: false,
            enableColumnMenus: false,
            treeRowHeaderAlwaysVisible: false,
            columnDefs: [
                { name: 'HRES_Year', displayName: 'year', grouping: { groupPriority: 0 }, sort: { priority: 0, direction: 'asc' }, cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },

                { name: 'HRES_Month', displayName: 'Month Name', grouping: { groupPriority: 1 }, sort: { priority: 1, direction: 'asc' }, cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                {
                    name: 'BankCount', displayName: ' Bank Count', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                },
                {
                    name: 'CashCount', displayName: ' Cash Count', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                }
            ],
            exporterMenuPdf: false,

            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            },
            gridMenuCustomItems: [{
                title: 'Monthly Mode Of Payment',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'all', 'all');
                },
                order: 110
            },
            {
                title: 'Export visible data as EXCEL',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'visible', 'visible');
                },
                order: 111
            }
            ]
        };

        $scope.monthlypfesiemployeegrid = {
            showGridFooter: true,
            showColumnFooter: true,
            enableFiltering: false,
            enableGridMenu: false,
            enableColumnMenus: false,
            treeRowHeaderAlwaysVisible: false,
            columnDefs: [
                { name: 'HRES_Year', displayName: 'year', grouping: { groupPriority: 0 }, sort: { priority: 0, direction: 'asc' }, cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },

                { name: 'HRES_Month', displayName: 'Month Name', grouping: { groupPriority: 1 }, sort: { priority: 1, direction: 'asc' }, cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                {
                    name: 'EPFCount', displayName: 'EPF Count', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                },
                {
                    name: 'ESICount', displayName: 'ESI Count', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                }
            ],
            exporterMenuPdf: false,

            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            },
            gridMenuCustomItems: [{
                title: 'Monthly PF/ESI count of Employees',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'all', 'all');
                },
                order: 110
            },
            {
                title: 'Export visible data as EXCEL',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'visible', 'visible');
                },
                order: 111
            }
            ]
        };

        $scope.monthlycumulativesalarygrid = {
            showGridFooter: true,
            showColumnFooter: true,
            enableFiltering: false,
            enableGridMenu: false,
            enableColumnMenus: false,
            treeRowHeaderAlwaysVisible: false,
            columnDefs: [
                { name: 'DeptName', displayName: 'Department Name', grouping: { groupPriority: 0 }, sort: { priority: 0, direction: 'asc' }, cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },

                { name: 'HRES_Year', displayName: 'year', grouping: { groupPriority: 0 }, sort: { priority: 0, direction: 'asc' }, cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },

                { name: 'MonthName', displayName: 'Month Name', grouping: { groupPriority: 1 }, sort: { priority: 1, direction: 'asc' }, cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                {
                    name: 'Earning', displayName: 'Earning Amount', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                },
                {
                    name: 'Deduction', displayName: 'Deduction Amount', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                },
                {
                    name: 'Netamount', displayName: 'Net Amount', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                }
            ],
            exporterMenuPdf: false,

            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            },
            gridMenuCustomItems: [{
                title: 'Monthly Cumulative Salary Details',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'all', 'all');
                },
                order: 110
            },
            {
                title: 'Export visible data as EXCEL',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'visible', 'visible');
                },
                order: 111
            }
            ]
        };

        $scope.monthlymobileemialgrid = {
            showGridFooter: true,
            showColumnFooter: true,
            enableFiltering: false,
            enableGridMenu: false,
            enableColumnMenus: false,
            treeRowHeaderAlwaysVisible: false,
            columnDefs: [
                { name: 'year', displayName: 'year', grouping: { groupPriority: 0 }, sort: { priority: 0, direction: 'asc' }, cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },

                { name: 'MonthName', displayName: 'Month Name', grouping: { groupPriority: 1 }, sort: { priority: 1, direction: 'asc' }, cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                {
                    name: 'MobileEmpcount', displayName: 'Mobile Count', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                },
                {
                    name: 'EmailCount', displayName: 'Email Count', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                }
            ],
            exporterMenuPdf: false,

            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            },
            gridMenuCustomItems: [{
                title: 'Monthly Mobile No/Email Registration',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'all', 'all');
                },
                order: 110
            },
            {
                title: 'Export visible data as EXCEL',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'visible', 'visible');
                },
                order: 111
            }
            ]
        };

        $scope.monthlyemployeesalarygrid = {
            showGridFooter: true,
            showColumnFooter: true,
            enableFiltering: false,
            enableGridMenu: false,
            enableColumnMenus: false,
            treeRowHeaderAlwaysVisible: false,
            columnDefs: [
                { name: 'HRES_Year', displayName: 'year', grouping: { groupPriority: 0 }, sort: { priority: 0, direction: 'asc' }, cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },

                { name: 'HRES_Month', displayName: 'Month Name', grouping: { groupPriority: 1 }, sort: { priority: 1, direction: 'asc' }, cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                {
                    name: 'Empcounts', displayName: ' Employee Count', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                }
            ],
            exporterMenuPdf: false,

            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            },
            gridMenuCustomItems: [{
                title: 'Monthly Salary Count',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'all', 'all');
                },
                order: 110
            },
            {
                title: 'Export visible data as EXCEL',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'visible', 'visible');
                },
                order: 111
            }
            ]
        };

        $scope.findstatistics = function () {
            var data = {
                "institution": $scope.institution,
                "ModuleName": $scope.module
            };

            apiService.create("TrackerReport/findstatistics", data).then(function (promise) {
                $scope.companyname = promise.companyname;
                
                $scope.totalactiveemployee = promise.totalactiveemployee;
                $scope.totalleftemployee = promise.totalleftemployee;

                $scope.ActEmpsYearly = promise.actEmpsYearly;
                $scope.monthlyactiveemployeegrid.data = promise.actEmpsMonthly;
                //$scope.ActEmpsMonthly = promise.actEmpsMonthly;

                $scope.LeftEmpsYearly = promise.leftEmpsYearly;
                $scope.monthlyleftemployeegrid.data = promise.leftEmpsMonthly;
                //$scope.LeftEmpsMonthly = promise.leftEmpsMonthly;

                $scope.LoanAdvEmpsYearly = promise.loanAdvEmpsYearly;
                $scope.monthlyloanadvanceemployeegrid.data = promise.loanAdvEmpsMonthly;
                //$scope.LoanAdvEmpsMonthly = promise.loanAdvEmpsMonthly;

                $scope.modeofPaymentyearly = promise.modeofPaymentyearly;
                $scope.monthlymodeofpaymentgrid.data = promise.modeofPaymentMonthly;
                //$scope.ModeofPaymentMonthly = promise.modeofPaymentMonthly;
                
                $scope.PFESIEmpsYearly = promise.pfesiEmpsYearly;
                $scope.monthlypfesiemployeegrid.data = promise.pfesiEmpsMonthly;
                //$scope.PFESIEmpsMonthly = promise.pfesiEmpsMonthly;

                $scope.CumulativeSalaryYearly = promise.cumulativeSalaryYearly;
                $scope.monthlycumulativesalarygrid.data = promise.cumulativeSalaryMonthly;
                //$scope.CumulativeSalaryMonthly = promise.cumulativeSalaryMonthly;

                $scope.EmpsMobEmailYearly = promise.empsMobEmailYearly;
                $scope.monthlymobileemialgrid.data = promise.empsMobEmailMonthly;
                //$scope.EmpsMobEmailMonthly = promise.empsMobEmailMonthly;

                $scope.salEmpsyearly = promise.salEmpsyearly;
                $scope.monthlyemployeesalarygrid.data = promise.salEmpsMonthly;
                //$scope.SalEmpsMonthly = promise.salEmpsMonthly;
                
                $scope.EmployeeDis = true;
            });
        };

        $scope.onLoadGetData = function () {
            var pageid = 2;
            //apiService.getURI("CumulativeSalaryReport/getalldetails", pageid).then(function (promise) {

            //});
        };

        $scope.cleardata = function () {
            $scope.EmployeeDis = false;
        };

        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'SalaryReport');

            $timeout(function () { location.href = exportHref; }, 100);
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };

        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
    }
})();