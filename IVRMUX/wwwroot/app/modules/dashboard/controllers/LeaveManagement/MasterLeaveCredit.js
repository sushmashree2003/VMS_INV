
(function () {
    'use strict';
    angular.module('app').controller('ManualLeaveCreditController', ManualLeaveCreditController)

    ManualLeaveCreditController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache', '$filter', 'uiGridConstants', 'appSettings', '$sce']
    function ManualLeaveCreditController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache, $filter, $uiGridConstants, appSettings, $sce) {

        var copty = "";
        $scope.rowsSelected = [];
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.obj = {};
        $scope.currentPage = 1;
        $scope.coptyright = copty;
        $scope.searchValue = '';
        $scope.submitted = false;
        $scope.submitted1 = false;

        $scope.OnLoadManualLeaveCredit = function () {
            $scope.getuserinstitution = [];
            $scope.leavearrayyear = [];
            $scope.get_leaveDetails = [];
            $scope.get_emp = [];
            $scope.get_leavestatus = [];
            $scope.GetEmp = [];
            $scope.MI_Id = "";
            $scope.submitted = false;
            $scope.submitted1 = false;
            $scope.HRMLY_Id = "";
            $scope.selectedleavevalue = false;
            var data = {
                "OnLoadOrChangeInstFlag": "OnLoad"
            };

            apiService.create("LeaveCredit/OnLoadManualLeaveCredit", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getuserinstitution = promise.getuserinstitution;
                    $scope.leavearrayyear = promise.leavearrayyear;
                    $scope.get_leaveDetails = promise.get_leaveDetails;
                    $scope.MI_Id = promise.mI_Id;
                }
            });
        };

        $scope.OnChangeInsitution = function () {
            $scope.leavearrayyear = [];
            $scope.get_leaveDetails = [];
            $scope.get_leavestatus = [];
            $scope.get_emp = [];
            $scope.GetEmp = [];
            $scope.selectedleavevalue = false;
            $scope.submitted = false;
            $scope.submitted1 = false;
            $scope.HRMLY_Id = "";
            var data = {
                "OnLoadOrChangeInstFlag": "OnChange",
                "MI_Id": $scope.MI_Id
            };

            apiService.create("LeaveCredit/OnLoadManualLeaveCredit", data).then(function (promise) {
                $scope.leavearrayyear = promise.leavearrayyear;
                $scope.get_leaveDetails = promise.get_leaveDetails;
            });
        };

        $scope.OnChangeLeaveYear = function () {
            $scope.get_emp = [];
            $scope.get_leavestatus = [];
            $scope.GetEmp = [];
            $scope.Tempselected_Leave = [];
            $scope.selectedreport = [];
            $scope.submitted1 = false;
            $scope.submitted = false;
        };

        $scope.SearchMaualDetails = function () {
            $scope.get_emp = [];
            $scope.get_leavestatus = [];
            $scope.GetEmp = [];
            $scope.Tempselected_Leave = [];
            $scope.selectedreport = [];

            angular.forEach($scope.get_leaveDetails, function (dd) {
                if (dd.selectedvalue) {
                    $scope.Tempselected_Leave.push({ hrmL_Id: dd.hrmL_Id, hrmL_LeaveName: dd.hrmL_LeaveName, leavecount: Number(dd.count) });
                    $scope.selectedreport.push(dd.hrmL_Id);
                }
            });

            if ($scope.MyForm1.$valid) {
                var data = {
                    "MI_Id": $scope.MI_Id,
                    "HRMLY_Id": $scope.HRMLY_Id,
                    "selectedreport": $scope.selectedreport
                };

                apiService.create("LeaveCredit/SearchManualLeaveCreditDetails", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.get_emp = promise.get_emp;
                        $scope.GetEmp = [];
                        $scope.obj.empselectedvalue = false;

                        angular.forEach($scope.get_emp, function (emp) {
                            $scope.Temp_leaves = [];
                            angular.forEach($scope.get_leaveDetails, function (dd) {
                                if (dd.selectedvalue) {
                                    $scope.Temp_leaves.push({
                                        hrmL_Id: dd.hrmL_Id, hrmL_LeaveName: dd.hrmL_LeaveName,
                                        leavecount: Number(dd.count), hrmE_Id: emp.hrmE_Id
                                    });
                                }
                            });

                            $scope.GetEmp.push({
                                hrmE_Id: emp.hrmE_Id, hrmE_EmployeeFirstName: emp.hrmE_EmployeeFirstName,
                                hrmE_DOJ: emp.hrmE_DOJ, leavedetails: $scope.Temp_leaves
                            });
                        });


                        $scope.get_leavestatus = promise.get_leavestatus;

                        angular.forEach($scope.GetEmp, function (emp) {
                            angular.forEach(emp.leavedetails, function (emp_leave) {
                                angular.forEach($scope.get_leavestatus, function (emp_status) {
                                    if (emp.hrmE_Id === emp_leave.hrmE_Id && emp_leave.hrmE_Id === emp_status.hrmE_Id && emp_leave.hrmL_Id === emp_status.hrmL_Id) {
                                        emp_leave.leavecount = emp_status.hrelS_CBLeaves;
                                        emp_leave.HRELS_Id = emp_status.hrelS_Id;
                                    }
                                });
                            });
                        });
                        console.log($scope.GetEmp);
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.SaveManualDetails = function (objform) {
            $scope.SelectedEmployees = [];
            if (objform.$valid) {

                angular.forEach($scope.GetEmp, function (emp) {
                    if (emp.empselected) {
                        $scope.SelectedEmployees.push(emp);
                    }
                });

                var data = {
                    "MI_Id": $scope.MI_Id,
                    "HRMLY_Id": $scope.HRMLY_Id,
                    "SelectedEmployees": $scope.SelectedEmployees
                };

                apiService.create("LeaveCredit/SaveManualLeaveCreditDetails", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Record Saved/Updated Successfully");
                        } else {
                            swal("Failed To Save /Update Records");
                        }

                        $scope.clear_first_tab();
                    }

                });

            } else {
                $scope.submitted1 = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.clear_first_tab = function () {
            $scope.submitted1 = false;
            $scope.OnChangeInsitution();
        };

        $scope.OnClickLeaveAll = function () {
            $scope.GetEmp = [];
            angular.forEach($scope.get_leaveDetails, function (d) {
                d.selectedvalue = $scope.selectedleavevalue;
            });
        };

        $scope.OnClickLeaveAllInd = function () {
            $scope.GetEmp = [];
            $scope.selectedleavevalue = $scope.get_leaveDetails.every(function (itm) { return itm.selectedvalue; });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.get_leaveDetails.some(function (options) {
                return options.selectedvalue;
            });
        };

        $scope.OnClickAllEmp = function () {
            angular.forEach($scope.GetEmp, function (dd) {
                dd.empselected = $scope.obj.empselectedvalue;
            });
        };

        $scope.OnClickIndEmp = function () {
            $scope.obj.empselectedvalue = $scope.GetEmp.every(function (itm) { return itm.empselected; });
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.GetEmp.some(function (options) {
                return options.empselected;
            });
        };
    }
})();