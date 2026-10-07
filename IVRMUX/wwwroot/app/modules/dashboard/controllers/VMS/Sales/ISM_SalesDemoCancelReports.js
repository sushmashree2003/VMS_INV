(function () {
    'use strict';
    angular
        .module('app')
        .controller('ism_sales_lead_demo_ReportController', ism_sales_lead_demo_ReportController);

    ism_sales_lead_demo_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce', '$timeout', 'Excel'];
    function ism_sales_lead_demo_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce, $timeout, Excel) {


        //=============image

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

        $scope.maxDatemf = new Date();
        //loading start

        $scope.athflag = false;
        $scope.loadData = function () {
            var id = 2;

            apiService.getURI("Sales_Lead/getalllead", id).
                then(function (promise) {
                    $scope.grid_view = false;
                    $scope.leadlist = promise.leadlist;
                    $scope.roomlist = promise.roomlist
                });
        };

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
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;


        $scope.grid_view = false;
        $scope.submitted = false;
        // $scope.leadlist = [];

        $scope.onchageradiobtn = function () {
            $scope.bomids = [];
            $scope.DemoCancelReportNew = [];
            //$scope.fromdate = "";
            //$scope.todate = "";
        }

        $scope.submitted = false;
        $scope.showreport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
            
                var data = {
                    "FROM_DATE": new Date($scope.fromdate).toDateString(),
                    "TO_DATE": new Date($scope.todate).toDateString(),
                    "EmpType": $scope.all1,
                    //"EmpType": "",
                    "Report_Type": $scope.all2
                }
                apiService.create("Sales_Lead/DemoCancelReportNew/", data).
                    then(function (promise) {
                        if (promise.demoCancelReportNew.length > 0 && promise.demoCancelReportNew !== null) {
                            $scope.DemoCancelReportNew = promise.demoCancelReportNew;

                            $scope.bomids = [];

                            angular.forEach($scope.DemoCancelReportNew, function (dev) {
                                if ($scope.bomids.length === 0) {
                                    $scope.bomids.push(dev);
                                } else if ($scope.bomids.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.bomids, function (emp) {
                                        if (emp.HRMDES_Id === dev.HRMDES_Id) {
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
                            $scope.templist = [];
                            angular.forEach($scope.bomids, function (ddd) {
                                $scope.templist = [];

                                angular.forEach($scope.DemoCancelReportNew, function (dd) {
                                    if (dd.HRMDES_Id === ddd.HRMDES_Id) {
                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.datadetails = $scope.templist;

                                
                            });



                            //$scope.templist123 = [];
                            //angular.forEach($scope.templist, function (ddd) {
                            //    $scope.templist123 = [];
                               
                            //    angular.forEach($scope.bomids, function (dd) {
                            //        if (dd.Hrme_id === ddd.Hrme_id) {
                            //            $scope.templist123.push(dd);
                            //        }
                            //    });
                            //    ddd.datadetails123 = $scope.templist123;
                                
                            //});

                            $scope.templist123 = [];
                            angular.forEach($scope.bomids, function (ddd) {
                                angular.forEach(ddd.datadetails, function (data) {
                                    data.datadetails123 = [];
                                    angular.forEach($scope.DemoCancelReportNew, function (dd) {
                                        if (dd.Hrme_id === data.Hrme_id) {
                                            $scope.templist123.push(dd);
                                        }
                                    });
                                    ddd.datadetails123 = $scope.templist123;
                                });
                               
                            });
                           



                          
                            console.log($scope.templist123);
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
            var exportHref = Excel.tableToExcel(tableemployee, 'sheet name');
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