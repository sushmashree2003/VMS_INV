(function () {
    'use strict';
    angular
        .module('app')
        .controller('LeaveReportController', LeaveReportController);
    LeaveReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'Excel', '$timeout', 'superCache'];
    function LeaveReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, Excel, $timeout, superCache) {
        $scope.disabledata = true;
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };
        $scope.showdepartment = true;
        $scope.showdesignation = true;
        var hrmeid;
        $scope.staff_types = [];
        $scope.Department_types = [];
        $scope.Designation_types = [];


        //Snippet for Page load
        $scope.loadData = function () {

            var id = 2;
            // $scope.all_check();
            apiService.getURI("LeaveReport/getleavereport", id).
                then(function (promise) {

                    //Current Date for New Grid
                    $scope.Current_Date = new Date();
                    $scope.actvmonth = true;
                    $scope.actvyear = true;
                    $scope.actvfromdate = true;
                    $scope.actvtodate = true;
                    $scope.Institution_List = promise.institution_List;
                    $scope.staff_types = promise.stf_types;
                    $scope.Department_types = promise.department_types;
                    $scope.Designation_types = promise.designation_types;
                    $scope.leave_name = promise.leave_name;
                    $scope.credit_month = promise.credit_month;
                    $scope.get_year = promise.get_year;
                    $scope.get_emp = promise.get_emp;

                    $scope.temp_type_arr = promise.stf_types;
                    $scope.temp_department_arr = promise.department_types;
                    $scope.temp_designation_arr = promise.designation_types;
                    $scope.temp_employee_arr = promise.get_emp;

                    $scope.count = $scope.Department_types.length;
                    $scope.count1 = $scope.Designation_types.length;
                    $scope.countl = $scope.get_emp.length;
                });
        };
        //Snippet to check all options in check box are selected for Institution
        $scope.all_check_Ins = function () {
            var toggleStatus = $scope.Inscheck;
            angular.forEach($scope.Institution_List, function (itm) {
                itm.class0 = toggleStatus;
            });
            $scope.get_stafftypes();
        };
        //Snippet to check all options in check box are selected for Stafftypes
        $scope.all_check = function () {
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.staff_types, function (itm) {
                itm.class = toggleStatus;
            });
            $scope.get_departments();
        };
        //Snippet to check all options in  check box are selected for Department
        $scope.all_check_dept = function () {
            var toggleStatus = $scope.deptcheck;
            angular.forEach($scope.Department_types, function (itm) {
                itm.class1 = toggleStatus;
            });
            $scope.get_designation();
        };
        //Snippet to check all options in check box are selected for Designation
        $scope.all_check_desg = function () {
            var toggleStatus = $scope.desgcheck;
            angular.forEach($scope.Designation_types, function (itm) {
                itm.class2 = toggleStatus;
            });
            $scope.get_Employees();
        };

        //Snippet to check all which options are selected in Institution
        $scope.Inschkbx = function () {
            $scope.Inscheck = $scope.Institution_List.every(function (options) {
                return options.class0;
            });
            $scope.get_stafftypes();
        };
        //Snippet to check all which options are selected in Stafftypes
        $scope.togchkbx = function () {
            $scope.usercheck = $scope.staff_types.every(function (options) {
                return options.class;
            });
            $scope.get_departments();
        };
        //Snippet to check all which options are selected in Department
        $scope.deptchkbx = function () {
            $scope.deptcheck = $scope.Department_types.every(function (options) {
                return options.class1;
            });
            $scope.get_designation();
        };
        //Snippet to check all which options are selected in Designation
        $scope.desgchkbx = function () {
            $scope.desgcheck = $scope.Designation_types.every(function (options) {
                return options.class2;
            });
            $scope.get_Employees();

        };

        $scope.cleardata = function () {
            $state.reload();
        };
        //Snippet of staff types linking from Institution List
        //$scope.get_stafftypes = function () {
        //    $scope.selectedemptypes = [];
        //    angular.forEach($scope.Institution_List, function (role) {
        //        if (role.class0) $scope.selectedemptypes.push(role);
        //    });
        //    if ($scope.selectedemptypes.length !== 0) {
        //        var data = {
        //            emptypes: $scope.selectedemptypes
        //        };
        //        apiService.create("LeaveReport/get_stafftypes", data).
        //            then(function (promise) {
        //                $scope.staff_types = promise.staff_List;
        //                $scope.count0 = $scope.staff_types.length;
        //                if ($scope.count0 === 0 && ($scope.selectedemptypes.length !== 0)) {
        //                    swal("No Staff Types Are Mapped with Selected Department !!!!");
        //                    //return false;                            
        //                }
        //            });
        //    }
        //    else if ($scope.selectedemptypes.length === 0) {
        //        $scope.Designation_types = $scope.temp_designation_arr;
        //        $scope.count1 = $scope.Designation_types.length;
        //    }
        //};

        $scope.get_stafftypes = function () {

            var data = {
                "Master_Id": $scope.class0
            };
            apiService.create("LeaveReport/get_stafftypes", data).
                then(function (promise) {
                    $scope.staff_types = promise.staff_List;
                    $scope.count0 = $scope.staff_types.length;
                    if ($scope.count0 === 0 && ($scope.selectedemptypes.length !== 0)) {
                        swal("No Staff Types Are Mapped with Selected Department !!!!");
                        //return false;                            
                    }
                });

        };
        //Snippet of departments linking from staff types
        $scope.get_departments = function () {
            $scope.selectedemptypes = [];
            angular.forEach($scope.staff_types, function (role) {
                if (role.class) $scope.selectedemptypes.push(role);
            });
            if ($scope.selectedemptypes.length !== 0) {
                var data = {
                    emptypes: $scope.selectedemptypes
                };
                apiService.create("LeaveReport/get_departments", data).
                    then(function (promise) {
                        $scope.Department_types = promise.department_types;
                        $scope.count = $scope.Department_types.length;

                        if ($scope.count === 0 && ($scope.selectedemptypes.length !== 0)) {
                            swal("No Department Are Mapped with Selected Group Type !!!!");
                        }
                    });
            }
            else if ($scope.selectedemptypes.length === 0) {
                $scope.Department_types = $scope.temp_department_arr;
                $scope.count = $scope.Department_types.length;
            }
        };
        //Snippet of designation linking from departments
        $scope.get_designation = function () {
            $scope.selectedemptypes = [];
            angular.forEach($scope.Department_types, function (role) {
                if (role.class1) $scope.selectedemptypes.push(role);
            });
            if ($scope.selectedemptypes.length !== 0) {
                var data = {
                    emptypes: $scope.selectedemptypes
                };
                apiService.create("LeaveReport/get_designation", data).
                    then(function (promise) {
                        $scope.Designation_types = promise.designation_types;
                        $scope.count1 = $scope.Designation_types.length;
                        if ($scope.count1 === 0 && ($scope.selectedemptypes.length !== 0)) {
                            swal("No Designation Are Mapped with Selected Department !!!!");
                            //return false;                            
                        }
                    });
            }
            else if ($scope.selectedemptypes.length === 0) {
                $scope.Designation_types = $scope.temp_designation_arr;
                $scope.count1 = $scope.Designation_types.length;
            }
        };
        //Snippet of Employees linking from designation
        $scope.get_Employees = function () {
            $scope.selectedemptypes = [];
            angular.forEach($scope.Designation_types, function (role) {
                if (role.class2) $scope.selectedemptypes.push(role);
            });
            if ($scope.selectedemptypes.length !== 0) {
                var data = {
                    emptypes: $scope.selectedemptypes
                };
                apiService.create("LeaveReport/get_Employees", data).
                    then(function (promise) {


                        $scope.get_emp = promise.get_emp;
                        $scope.countl = $scope.get_emp.length;

                        if ($scope.countl === 0 && ($scope.selectedemptypes.length !== 0)) {
                            swal("No Employee Mapped with Selected Designation !!!!");
                        }
                    });
            }
            else if ($scope.selectedemptypes.length === 0) {
                $scope.get_emp = $scope.temp_employee_arr;
                $scope.countl = $scope.get_emp.length;
            }
        };
        $scope.get_report = function () {

            if ($scope.myForm.$valid) {
                $scope.selectedemptypes = [];
                //angular.forEach($scope.get_emp, function (itm1) {
                //    if (itm1.hrmE_EmployeeFirstName) $scope.selectedemptypes.push(itm1);
                //});

                angular.forEach($scope.get_emp, function (mm) {

                    $scope.selectedemptypes.push({ HRME_Id: mm.hrmE_Id, HRMGT_Id: mm.hrmgT_Id, HRMD_Id: mm.hrmD_Id, HRMDES_Id: mm.hrmdeS_Id });


                })

                var data = {
                    authorizationreport: $scope.selectedemptypes
                };
                apiService.create("LeaveReport/get_authorization_report", data).
                    then(function (promise) {

                        if (promise.authorizationreport_details.length > 0 && promise.authorizationreport_details != null) {
                            $scope.levaeApproverDetails = promise.authorizationreport_details;

                            $scope.employeeid = [];

                            angular.forEach($scope.levaeApproverDetails, function (dev) {
                                if ($scope.employeeid.length === 0) {
                                    $scope.employeeid.push({
                                        HRLA_Id: dev.HRLA_Id,
                                        HRMGT_EmployeeGroupType: dev.HRMGT_EmployeeGroupType,
                                        HRMD_DepartmentName: dev.HRMD_DepartmentName,
                                        HRMDES_DesignationName: dev.HRMDES_DesignationName,
                                        HRML_LeaveName: dev.HRML_LeaveName,
                                    });
                                } else if ($scope.employeeid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.employeeid, function (emp) {
                                        if (emp.HRLA_Id === dev.HRLA_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.employeeid.push({
                                            HRLA_Id: dev.HRLA_Id,
                                            HRMGT_EmployeeGroupType: dev.HRMGT_EmployeeGroupType,
                                            HRMD_DepartmentName: dev.HRMD_DepartmentName,
                                            HRMDES_DesignationName: dev.HRMDES_DesignationName,
                                            HRML_LeaveName: dev.HRML_LeaveName,
                                        });
                                    }
                                }
                            });

                            angular.forEach($scope.employeeid, function (ddd) {
                                $scope.templist = [];
                                angular.forEach($scope.levaeApproverDetails, function (dd) {
                                    if (dd.HRLA_Id === ddd.HRLA_Id) {
                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.plannerdetails = $scope.templist;
                            });


                        }
                        else {

                            swal("No Data Found !!");
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }

        };

        //$scope.get_report = function () {

        //    if ($scope.myForm.$valid) {
        //        $scope.selectedemptypes = [];
        //        angular.forEach($scope.get_emp, function (itm1) {
        //            if (itm1.hrmE_EmployeeFirstName) $scope.selectedemptypes.push(itm1.hrmE_Id);
        //        });
        //        var data = {
        //            selectedreport: $scope.selectedemptypes
        //        };
        //        apiService.create("LeaveReport/get_authorization_report", data).
        //            then(function (promise) {

        //                if (promise.authorizationreport_details.length > 0 && promise.authorizationreport_details != null) {
        //                    debugger;
        //                    $scope.EmployeeDis = true;
        //                    $scope.ReportDetails = promise.authorizationreport_details;
        //                    $scope.temp_Emp_array = [];


        //                    angular.forEach($scope.ReportDetails, function (report) {
        //                        if ($scope.temp_Emp_array.length == 0) {
        //                            $scope.temp_Emp_array.push({ HRME_Id: report.HRME_Id, HRME_EmployeeFirstName: report.HRME_EmployeeFirstName, HRME_EmployeeMiddleName: report.HRME_EmployeeMiddleName, HRME_EmployeeLastName: report.HRME_EmployeeLastName});
        //                        }
        //                        else if ($scope.temp_Emp_array.length > 0) {
        //                            var counter = 0;
        //                            angular.forEach($scope.temp_Emp_array, function (rpt) {
        //                                if (rpt.HRME_Id == report.HRME_Id)
        //                                    counter += 1;
        //                            })
        //                            if (counter == 0) {
        //                                $scope.temp_Emp_array.push({ HRME_Id: report.HRME_Id, HRME_EmployeeFirstName: report.HRME_EmployeeFirstName, HRME_EmployeeMiddleName: report.HRME_EmployeeMiddleName, HRME_EmployeeLastName: report.HRME_EmployeeLastName });
        //                            }
        //                        }
        //                    })

        //                    console.log($scope.temp_Emp_array);


        //                    angular.forEach($scope.temp_Emp_array, function (rep) {
        //                        $scope.Leave_Types = [];
        //                        angular.forEach($scope.ReportDetails, function (report) {
        //                            if (report.HRME_Id == rep.HRME_Id) {
        //                                if ($scope.Leave_Types.length == 0) {
        //                                    $scope.Leave_Types.push({ HRME_Id: report.HRME_Id, HRME_EmployeeFirstName: report.HRME_EmployeeFirstName, HRML_LeaveType: report.HRML_LeaveType });
        //                                }
        //                                else if ($scope.Leave_Types.length > 0) {
        //                                    var counter1 = 0;
        //                                    angular.forEach($scope.Leave_Types, function (Ltrep) {
        //                                        if (Ltrep.HRML_LeaveType == report.HRML_LeaveType) {
        //                                            counter1 +=1;
        //                                        }
        //                                    })
        //                                    if (counter1 == 0) {
        //                                        $scope.Leave_Types.push({ HRME_Id: report.HRME_Id, HRME_EmployeeFirstName: report.HRME_EmployeeFirstName, HRML_LeaveType: report.HRML_LeaveType });
        //                                    }
        //                                }
        //                            }
        //                        })
        //                        rep.Leave_auth_types = $scope.Leave_Types;
        //                    })

        //                    angular.forEach($scope.temp_Emp_array, function (emp) {
        //                        var emp_rowspan = 0;
        //                        angular.forEach(emp.Leave_auth_types, function (leavetype) {
        //                            $scope.Leaveauthdetails = [];
        //                            var leavetype_rowspan = 0;
        //                            angular.forEach($scope.ReportDetails, function (reportDet) {
        //                                if (reportDet.HRML_LeaveType == leavetype.HRML_LeaveType && reportDet.HRME_Id == emp.HRME_Id) {
        //                                    emp_rowspan += 1;
        //                                    leavetype_rowspan += 1;
        //                                    $scope.Leaveauthdetails.push(reportDet);
        //                                }
        //                            })
        //                            leavetype.LeaveAuthDetails = $scope.Leaveauthdetails;
        //                            leavetype.leavetype_rowspan = leavetype_rowspan+1;
        //                        })
        //                        emp.emp_rowspan = emp_rowspan+1;
        //                    })
        //                }
        //                else {
        //                    $scope.EmployeeDis = false;
        //                    swal("No Data Found !!");
        //                }
        //            });
        //    }
        //    else {
        //        $scope.submitted = true;
        //    }

        //};

        //Snippet to print
        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        //Snippet to export to excel
        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'LeaveReport');

            $timeout(function () { location.href = exportHref; }, 100);
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };
    }
})();