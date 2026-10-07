(function () {
    'use strict';
    angular
        .module('app')
        .controller('employeeSalaryDetailsController', employeeSalaryDetailsController)

    employeeSalaryDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function employeeSalaryDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {



        $scope.onLoadGetData = function () {

            var pageid = 2;
            apiService.getURI("EmployeeSalaryDetails/getalldetails", pageid).then(function (promise) {

                if (promise.employeedetailList !== null && promise.employeedetailList.length > 0) {
                    $scope.employeedropdown = promise.employeedetailList;
                }


                $scope.groupTypedropdown = promise.groupTypedropdownlist;
                $scope.departmentdropdown = promise.departmentdropdownlist;
                $scope.designationdropdown = promise.designationdropdownlist;


                if ($scope.groupTypedropdown !== null && $scope.groupTypedropdown.length > 0) {
                    $scope.groupTypedropdown = $scope.groupTypedropdown;
                    $scope.groupTypeselectedAll = true;
                    $scope.GetEmployeeBygroupTypeAll($scope.groupTypeselectedAll);
                }


                if ($scope.departmentdropdown !== null && $scope.departmentdropdown.length > 0) {
                    $scope.departmentdropdown = $scope.departmentdropdown;
                    $scope.departmentselectedAll = true;
                    $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                }

                if ($scope.designationdropdown !== null && $scope.designationdropdown.length > 0) {
                    $scope.designationdropdown = $scope.designationdropdown;

                    $scope.designationselectedAll = true;
                    $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                }

                $scope.mi_id = promise.mI_Id;
            })
        }


        //By group Type
        $scope.GetEmployeeBygroupTypeAll = function (groupTypeselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.groupTypeselectedAll;
            angular.forEach($scope.groupTypedropdown, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_depts();
        }


        //single
        $scope.GetEmployeeBygroupType = function (groupType) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.groupTypeselectedAll = $scope.groupTypedropdown.every(function (itm) {

                return itm.selected;
            });

            $scope.get_depts();
        };
        $scope.get_depts = function () {
            var ids = [];
            angular.forEach($scope.groupTypedropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.hrmgT_Id);
                }
            })
            var data = {
                hrmgT_IdList: ids
            }
            apiService.create("EmployeeSalaryDetails/get_depts", data).
                then(function (promise) {

                    if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                        $scope.departmentdropdown = promise.departmentdropdown;
                        $scope.departmentselectedAll = true;
                        $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                    }
                })
        };



        //By Employee Type
        $scope.GetEmployeeByTypeAll = function (employeeTypeselectedAll) {

            var toggleStatus = $scope.employeeTypeselectedAll;
            angular.forEach($scope.employeeTypedropdown, function (itm) {
                itm.selected = toggleStatus;

            });
        }


        //single
        $scope.GetEmployeeByType = function (employeeType) {

            $scope.employeeTypeselectedAll = $scope.employeeTypedropdown.every(function (itm) {

                return itm.selected;
            });

        }
        $scope.get_desig = function () {
            var ids = [];
            angular.forEach($scope.groupTypedropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.hrmgT_Id);
                }
            })
            var ids1 = [];
            angular.forEach($scope.departmentdropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids1.push(grp_t.hrmD_Id);
                }
            })
            var data = {
                hrmgT_IdList: ids,
                hrmD_IdList: ids1
            }
            apiService.create("EmployeeSalaryDetails/get_desig", data).
                then(function (promise) {
                    if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                        $scope.designationdropdown = promise.designationdropdown;
                        $scope.designationselectedAll = true;
                        $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                    }
                })
        };


        //By Department
        $scope.GetEmployeeByDepartmentAll = function (departmentselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.departmentselectedAll;
            angular.forEach($scope.departmentdropdown, function (itm) {
                itm.selected = toggleStatus;

            });
            $scope.get_desig();

        }


        //By Department Single
        $scope.GetEmployeeByDepartment = function (department) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {

                return itm.selected;
            });
            $scope.get_desig();
        }


        //By Designation
        $scope.GetEmployeeByDesignationAll = function (designationselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.designationselectedAll;
            angular.forEach($scope.designationdropdown, function (itm) {
                itm.selected = toggleStatus;

            });

        }


        //By Designation Single
        $scope.GetEmployeeByDesignation = function (designation) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {

                return itm.selected;
            });
        }


        $scope.submitted = false;
        $scope.GetEmployeeList = function () {

            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }

            $scope.employeedropdown = [];
            //  $scope.Emp = {};

            var groupTypeselected = [];
            angular.forEach($scope.groupTypedropdown, function (itm) {
                if (itm.selected) {
                    groupTypeselected.push(itm.hrmgT_Id);//
                }

            });

            var departmentselected = [];
            angular.forEach($scope.departmentdropdown, function (itm) {
                if (itm.selected) {
                    departmentselected.push(itm.hrmD_Id);
                }

            });


            var designationselected = [];
            angular.forEach($scope.designationdropdown, function (itm) {
                if (itm.selected) {
                    designationselected.push(itm.hrmdeS_Id);
                }

            });

            if (groupTypeselected.length == 0 && departmentselected.length == 0 && designationselected.length == 0) {
                swal('Kindly select atleast one record');
                return;
            }


            var data = {
                groupTypeIdList: groupTypeselected,
                hrmD_IdList: departmentselected,
                hrmdeS_IdList: designationselected
            }

            apiService.create("EmployeeSalaryDetails/GetEmployeeDetailsBySelected", data).
                then(function (promise) {

                    if (promise.employeedetailList !== null && promise.employeedetailList.length > 0) {
                        $scope.employeedropdown = promise.employeedetailList;
                        $scope.EmployeeDis = true;
                    }
                    else {
                        $scope.employeedropdown = [];

                        swal("No Record Found to Display..");
                    }
                })
        }


        //Salary details form validation
        $scope.submittedSalarydetails = false;
        $scope.saveSalaryDetails = function () {
            $scope.submittedSalarydetails = true;
            if ($scope.myFormSalarydetails.$valid) {





                var data = {

                    "HRME_Id": $scope.Employee.hrmE_Id,
                    EarningDTO: selectedEarning,
                    DeductionDTO: selectedDetection,
                    ArrearDTO: selectedArrear,
                    "TabName": "SalaryTab"
                }
                apiService.create("EmployeeSalaryDetails/SalaryReversalSave", data).
                    then(function (promise) {

                        if (promise.retrunMsg !== "") {

                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            } else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                            } else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                            } else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                            } else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }

                            $scope.cancel();

                            $scope.onLoadGetData();
                        }
                    })
            }

        };


        $scope.interacted7 = function (field) {
            return $scope.submittedSalarydetails;
        };


        $scope.clear_Salarydetails_tab = function (data) {
            $scope.onLoadGetData();
        }




        $scope.getSalaryDetails = function () {
            $scope.submittedSalarydetails = true;
            if ($scope.myFormSalarydetails.$valid) {

                var data = $scope.Employee;
                apiService.create("EmployeeSalaryDetails/getEmployeeSalaryLopDetails", data).
                    then(function (promise) {

                        if (promise.employeedetailList !== null && promise.employeedetailList.length > 0) {
                            $scope.employeedropdown = promise.employeedetailList;
                        }

                    });
            }
        }





    }
})();



