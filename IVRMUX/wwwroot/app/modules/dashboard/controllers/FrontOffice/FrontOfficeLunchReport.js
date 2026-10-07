(function () {
    'use strict';

    angular
        .module('app')
        .controller('FrontOfficeLunchReport', FrontOfficeLunchReport);

    FrontOfficeLunchReport.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']

    function FrontOfficeLunchReport($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {
        
        $scope.ddate = new Date();
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        $scope.sortKey = "regno";
        $scope.reverse = true;

        $scope.presentCountgrid = 0;

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.loadData = function () {
            var id = 2;
            apiService.getURI("FrontOffice/", id).
                then(function (promise) {
                    $scope.staff_types = promise.filltypes;
                    $scope.fillmonth = promise.fillmonth;
                    $scope.fillyear = promise.fillyear;
                });
        };

        $scope.all_check = function () {
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.staff_types, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_departments();
        };

        $scope.get_departments = function () {
            $scope.usercheck = $scope.staff_types.every(function (options) {
                return options.selected;
            });
            $scope.deptcheck = "";
            $scope.desgcheck = "";

            var groupidss;
            for (var i = 0; i < $scope.staff_types.length; i++) {
                if ($scope.staff_types[i].selected == true) {

                    if (groupidss == undefined)
                        groupidss = $scope.staff_types[i].hrmgT_Id;
                    else
                        groupidss = groupidss + "," + $scope.staff_types[i].hrmgT_Id;
                }
            }
            if (groupidss != undefined) {
                var data = {
                    "multipletype": groupidss
                };
                apiService.create("EmployeeInOutReport/get_departments", data).
                    then(function (promise) {
                        $scope.Department_types = promise.filldepartment;
                        if ($scope.Department_types.length > 0) {
                            $scope.get_designation();
                        }
                    });
            }
            else {
                $scope.Department_types = "";
                $scope.Designation_types = "";
                $scope.Employeelst = "";
            }
        };

        $scope.isOptionsRequired = function () {
            return !$scope.staff_types.some(function (options) {
                return options.selected;
            });
        };

        $scope.all_checkdep = function () {
            var toggleStatus = $scope.deptcheck;
            angular.forEach($scope.Department_types, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_designation();
        };

        $scope.get_designation = function () {
            $scope.deptcheck = $scope.Department_types.every(function (options) {
                return options.selected;
            });
            $scope.get_designationnew();
        };

        $scope.get_designationnew = function () {
            $scope.desgcheck = "";
            var groupidss;
            for (var i = 0; i < $scope.Department_types.length; i++) {
                if ($scope.Department_types[i].selected == true) {
                    if (groupidss == undefined)
                        groupidss = $scope.Department_types[i].hrmD_Id;
                    else
                        groupidss = groupidss + "," + $scope.Department_types[i].hrmD_Id;
                }
            }
            if (groupidss != undefined) {
                var data = {
                    "multipledep": groupidss
                };
                apiService.create("EmployeeInOutReport/get_designation", data).
                    then(function (promise) {
                        $scope.Designation_types = promise.filldesignation;
                    });
            }
            else {
                $scope.Designation_types = "";
                $scope.Employeelst = "";
            }
        };

        $scope.all_checkdesg = function () {
            var toggleStatus = $scope.desgcheck;
            angular.forEach($scope.Designation_types, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_employee();
        };

        $scope.onchageDropdownValue = function () {
            $scope.report = false;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clear_Details = function () {
            $state.reload();
            $scope.loaddata();
        };

        $scope.user_check = function () {
            if ($scope.user_check == 1) {
                $scope.userl = 1;
            }
            else {
                $scope.userl = 0;
            }
        };

        $scope.ShowReportdata = function () {
            if ($scope.myForm.$valid) {

                var typeIds;
                for (var i = 0; i < $scope.staff_types.length; i++) {
                    if ($scope.staff_types[i].selected == true) {

                        if (typeIds == undefined)
                            typeIds = $scope.staff_types[i].hrmgT_Id;
                        else
                            typeIds = typeIds + "," + $scope.staff_types[i].hrmgT_Id;
                    }
                }

                var deptIds;
                for (var i = 0; i < $scope.Department_types.length; i++) {
                    if ($scope.Department_types[i].selected == true) {

                        if (deptIds == undefined)
                            deptIds = $scope.Department_types[i].hrmD_Id;
                        else
                            deptIds = deptIds + "," + $scope.Department_types[i].hrmD_Id;
                    }
                }

                var groupidss;
                for (var i = 0; i < $scope.Designation_types.length; i++) {
                    if ($scope.Designation_types[i].selected == true) {

                        if (groupidss == undefined)
                            groupidss = $scope.Designation_types[i].hrmdeS_Id;
                        else
                            groupidss = groupidss + "," + $scope.Designation_types[i].hrmdeS_Id;
                    }
                }

                var data = {
                    "employeetpList": typeIds,
                    "employeeDeptList": deptIds,
                    "employeeDesgList": groupidss,
                    "yearid": $scope.year,
                    "month": $scope.month
                };

                apiService.create("FrontOffice/getlunchreport", data).
                    then(function (promise) {
                        $scope.columnnames = promise.columnnames;
                        $scope.columnnum = $scope.columnnames.length + 5;
                        $scope.fromdate = promise.fromdate;
                        $scope.todate = promise.todate;
                        
                        var temp_array = [];
                        var temp_array1 = [];
                        for (var i = 0; i < promise.lunchdata.length; i++) {
                            var newCol = { punchdate: promise.lunchdata[i].FOEP_PunchDate, punchouttime: promise.lunchdata[i].AFLogout, punchintime: promise.lunchdata[i].AFLogin, timediff: promise.lunchdata[i].Diff };
                            temp_array.push(newCol);
                            if (i < promise.lunchdata.length - 1) {
                                if (promise.lunchdata[i].HRME_Id == promise.lunchdata[i + 1].HRME_Id)
                                    continue;
                            }
                            var newCol1 = { pdate: temp_array, hrmE_Id: promise.lunchdata[i].HRME_Id, empcode: promise.lunchdata[i].HRME_EmployeeCode, name: promise.lunchdata[i].name, hrmdeS_DesignationName: promise.lunchdata[i].HRMDES_DesignationName };
                            temp_array1.push(newCol1);
                            temp_array = [];
                        }
                        $scope.lunchdata = temp_array1;

                        if (temp_array1.length > 0) {
                            $scope.report = true;
                        } else {
                            $scope.report = false;
                        }
                        console.log($scope.lunchdata);
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.exptoex = function () {
            var divToPrint = document.getElementById("table");
            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            saveAs(blob, "LunchReport.xls");
        };

        $scope.printData = function () {
            var divToPrint = document.getElementById("table");
            var newWin = window.open("");
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();
        };

        $scope.cleardata = function () {
            $scope.year = "";
            $scope.month = "";
            $scope.report = false;
        };
    }
})();

