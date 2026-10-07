
(function () {
    'use strict';
    angular
        .module('app')
        .controller('Attendance_GPSLocationReportController', Attendance_GPSLocationReportController);
    Attendance_GPSLocationReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter'];
    function Attendance_GPSLocationReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {

        $scope.obj = {};
        $scope.optionflag = "Item";
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));


        //==========================================================

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var id = 1;
            apiService.getURI("GetVisitorReport/getGpsloaddata", id).
                then(function (promise) {
                    if (promise.employeelist != null && promise.employeelist.length > 0) {
                        $scope.employeelist = promise.employeelist;
                    }
                });
        };

        $scope.onrdochange = function (optionflag) {
            $scope.get_itemreportdetails = "";
            $scope.get_itemreport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag);
        };

        //========================ITEM
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.employeelist.every(function (itm) {
                return itm.itemck;
            });
        };
        $scope.isOptionsRequiredI = function () {
            if ($scope.optionflag === 'Employee') {
                return !$scope.employeelist.some(function (options) {
                    return options.itemck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkI = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.employeelist, function (itm) {
                itm.itemck = toggleStatus;
            });
        };

        //==================================Item Report
        $scope.submitted = false;
        $scope.onreport = function () {
            if ($scope.myForm.$valid) {

                $scope.itemsArray = [];
                if ($scope.optionflag === null || $scope.optionflag === undefined) {
                    $scope.optionflag = "";
                }
                var data = {};
                if ($scope.optionflag === "betweendate") {
                    $scope.start_Date = $filter('date')($scope.startdate, "yyyy-MM-dd");
                    $scope.end_Date = $filter('date')($scope.enddate, "yyyy-MM-dd");
                    $scope.sDate = $scope.start_Date;
                    $scope.eDate = $scope.end_Date;
                    data = {
                        "fromdate": $scope.sDate,
                        "todate": $scope.eDate,
                        "optionflag": $scope.optionflag
                    };
                }
                else {
                    $scope.employeearray = [];
                    angular.forEach($scope.employeelist, function (aa) {
                        if (aa.itemck === true) {
                            $scope.employeearray.push({ HRME_Id: aa.hrmE_Id });
                        }
                    });
                    $scope.sDate = "";
                    $scope.eDate = "";
                    data = {
                        "employeearray":$scope.employeearray,
                        "optionflag": $scope.optionflag,
                        "fromdate": $scope.sDate,
                        "todate": $scope.eDate
                    };
                }
                apiService.create("GetVisitorReport/gpsreport", data).
                    then(function (promise) {
                        if (promise.attendancegpslist.length > 0) {
                            $scope.attendancegpslist = promise.attendancegpslist;
                            $scope.presentCountgrid = $scope.attendancegpslist.length;
                        }
                        else {
                            swal("No Record Found...!!");
                            $scope.attendancegpslist = "";
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.RadiChange = function () {
            $scope.attendancegpslist = [];
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cancel = function () {
            $state.reload();
        };
        //======================================Print & Export to Excel
        $scope.exportToExcel = function (export_id) {

            var exportHref = Excel.tableToExcel(export_id, 'printexcel');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };
        $scope.printData = function () {

            var innerContents = document.getElementById("printItem").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
    }
})();