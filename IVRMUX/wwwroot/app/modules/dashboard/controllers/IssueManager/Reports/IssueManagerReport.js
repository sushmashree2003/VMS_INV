(function () {
    'use strict';
    angular
        .module('app')
        .controller('IssueManagerReportController', IssueManagerReportController);
    IssueManagerReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function IssueManagerReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache) {
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 10;
        }
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        $scope.submitted = false;
        //$scope.startdate = new Date();
        //$scope.enddate = new Date();
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.searchValue = "";
            var pageid = 2;
            apiService.getURI("IssueManagerReport/getdetails", pageid).then(function (promise) {
                if (promise.floorList !== null && promise.floorList.length > 0) {
                    $scope.floorList = promise.floorList;
                }
            });
        };
        $scope.GetReport = function () {
            $scope.floorListsss = [];
            if ($scope.myForm.$valid) {
                $scope.get_Report = [];
                var FROM_DATE = new Date();
                var TO_DATE = new Date();
                if ($scope.obj.hrmF_FloorName == null || $scope.obj.hrmF_FloorName === undefined) {
                    $scope.obj.hrmF_FloorName = "";
                }
                if ($scope.optionflag == "MAKERCHECKER" || $scope.optionflag == "PLANER" || $scope.optionflag == "LEAVE" || $scope.optionflag == "CHECKLIST" ) {
                    FROM_DATE = ""; TO_DATE = "";
                    FROM_DATE = new Date($scope.obj.startdate).toDateString();
                    TO_DATE = new Date($scope.obj.enddate).toDateString();
                }
                //if ($scope.optionflag == "CHECKLIST") {
                //    angular.forEach($scope.floorList, function (comp) {
                //        if (comp.selected == true) {
                //            $scope.floorListsss.push({ hrmF_FloorName: comp.hrmF_FloorName });
                //        }
                //    });
                //}                
                var data = {
                    "FromDate": FROM_DATE,
                    "ToDate": TO_DATE,
                    "optionflag": $scope.optionflag,
                    "HRMF_FloorNamelist": $scope.floorListsss,
                    "FloarName": $scope.obj.hrmF_FloorName
                };
                apiService.create("IssueManagerReport/getReport", data).then(function (promise) {
                    if (promise.getReport.length != null && promise.getReport.length > 0) {
                        $scope.get_Report = promise.getReport;
                        $scope.presentCountgrid = $scope.get_Report.length;
                        if ($scope.optionflag == "PLANER") {
                            $scope.employee = [];
                            $scope.employee = $scope.get_Report[0].PlnnedEmpName;
                            $scope.employeeid = [];
                            angular.forEach($scope.get_Report, function (dev) {
                                if ($scope.employeeid.length === 0) {
                                    $scope.employeeid.push({ HRME_Id: dev.HRME_Id, PlnnedEmpName: dev.PlnnedEmpName, ISMTPL_StartDate: dev.ISMTPL_StartDate, ISMTPL_EndDate: dev.ISMTPL_EndDate, ApprovedEmpName: dev.ApprovedEmpName, ApprovedDate: dev.ApprovedDate, ApprovedTime: dev.ApprovedTime, ISMTPL_PlannerName: dev.ISMTPL_PlannerName, ISMTPLAP_Remarks: dev.ISMTPLAP_Remarks });
                                }
                                else if ($scope.employeeid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.employeeid, function (emp) {
                                        if (emp.HRME_Id === dev.HRME_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.employeeid.push({ HRME_Id: dev.HRME_Id, PlnnedEmpName: dev.PlnnedEmpName, ISMTPL_StartDate: dev.ISMTPL_StartDate, ISMTPL_EndDate: dev.ISMTPL_EndDate, ApprovedEmpName: dev.ApprovedEmpName, ApprovedDate: dev.ApprovedDate, ApprovedTime: dev.ApprovedTime, ISMTPL_PlannerName: dev.ISMTPL_PlannerName, ISMTPLAP_Remarks: dev.ISMTPLAP_Remarks });
                                    }
                                }
                            });
                            console.log($scope.employeeid);
                            angular.forEach($scope.employeeid, function (ddd) {
                                $scope.templist = [];
                                angular.forEach($scope.get_Report, function (dd) {
                                    if (dd.HRME_Id === ddd.HRME_Id) {
                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.plannerdetails = $scope.templist;
                            });
                            console.log($scope.employeeid);
                        }
                    }
                    else {
                        swal("Record Not Found !");
                    }
                });
            }
        };
        $scope.exportToExcel = function () {
            var exportHref = Excel.tableToExcel(printExcel, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };
        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printExcel").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //===========Clear Field
        $scope.Clear = function () {
            $state.reload();
        };
        $scope.onradiochange = function () {
            $scope.get_Report = [];
            $scope.obj.startdate = ""; $scope.obj.enddate = "";
        };
        //$scope.all_checkCCCC = function () {
        //    var hrmF_FloorName = $scope.obj.hrmF_FloorName;
        //    var count = 0;
        //    angular.forEach($scope.floorList, function (itm) {
        //        itm.selected = hrmF_FloorName;
        //        if (itm.selected == true) {
        //            count += 1;
        //        }
        //        else {
        //            count = 0;
        //        }

        //    });
        //};
        //$scope.togchkbxCCCC = function () {
        //    $scope.obj.hrmF_FloorName = $scope.floorList.every(function (options) {
        //        return options.selected;
        //    });
        //};
        //$scope.isOptionsRequiredtwo = function () {
        //    return !$scope.floorList.some(function (options) {
        //        return options.selected;
        //    });
        //}
    }
})();

