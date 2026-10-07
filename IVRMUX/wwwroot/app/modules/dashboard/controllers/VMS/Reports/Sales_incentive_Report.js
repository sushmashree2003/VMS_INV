(function () {
    'use strict';
    angular
        .module('app')
        .controller('Sales_incentive_ReportController', Sales_incentive_ReportController);

    Sales_incentive_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce', '$timeout', 'Excel'];
    function Sales_incentive_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce, $timeout, Excel) {


        //=============image
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.maxDatemf = new Date();
        //loading start

        $scope.athflag = false;
        $scope.loadData = function () {
            var id = 2;

            apiService.getURI("Sales_Lead/getalllead", id).
                then(function (promise) {
                    $scope.grid_view = false;
                    $scope.incentiveleads = promise.incentiveleads;
                    $scope.incentiveemployee = promise.incentiveemployee;
                    
                });
        };

        $scope.togchkbxdept = function (groupType) {
            $scope.ISMSLE_Id = $scope.leadlist.every(function (itm) {
                return itm.selected;
            });


        };
        $scope.all_checkdept = function (ISMSLE_Id) {
            var toggleStatus = ISMSLE_Id;
            angular.forEach($scope.incentiveleads, function (itm) {
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

        $scope.searchleadvalue = function (searchleadvalue) {
            $scope.searchleadvalue1 = searchleadvalue.length;
            if (searchleadvalue.length > 1) {
                var data = {
                    "searchleadvalue": searchleadvalue,
                    "flag": "Search"
                }
                apiService.create("ISM_Client_Project/searchleadvalue", data).
                    then(function (promise) {
                        $scope.leadlist = promise.leadlist;

                    })
            }
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
                $scope.leaddatalist = [];
                $scope.empdatalist = [];
                $scope.leaddemoreport = [];
                angular.forEach($scope.incentiveleads, function (d) {
                    if (d.selected === true) {
                        $scope.leaddatalist.push({ ISMSLE_Id: d.ismslE_Id });
                    }
                });
                angular.forEach($scope.incentiveemployee, function (d) {
                    if (d.selected === true) {
                        $scope.empdatalist.push({ HRME_Id: d.hrmE_Id });
                    }
                });

                var data = {
                    "leaddatalist": $scope.leaddatalist,
                    "empdatalist": $scope.empdatalist
                 
                }
                apiService.create("Sales_Lead/demo_report/", data).
                    then(function (promise) {
                        if (promise.leaddemoreport > 0 || promise.leaddemoreport !== null) {
                            $scope.leaddemoreport = promise.leaddemoreport;
                       
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
        $scope.printData = function () {
            var innerContents = document.getElementById("printExcel").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/Sports/HouseReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();

        }


        $scope.exportToExcel = function (tableId) {
            var excelname = "LEAD DEMO REPORT";
            excelname = excelname.toUpperCase() + '.xls';

            if ($scope.leaddemoreport !== null && $scope.leaddemoreport.length > 0) {
                var exportHref = Excel.tableToExcel(printExcel, 'LEAD DEMO REPORT');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = excelname;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
                //$state.reload();
            }
            else {
                swal("Please Select Records to be Exported");
            }

        };




        $scope.clearid = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }

})();