

(function () {
    'use strict';
    angular
        .module('app')
        .controller('LeaveReportController', LeaveReportController);
    LeaveReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'Excel', '$timeout', 'superCache'];
    function LeaveReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, Excel, $timeout, superCache) {
        $scope.editEmployee = {};
        $scope.chkfo = false;
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.albumNameArraycolumn = [];
        $scope.disabledata = true;
        $scope.ismeridian = true;
        $scope.allinsti == true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };
        $scope.showdepartment = true;
        $scope.showdesignation = true;
        var hrmeid;

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

                    
                   

                    //$scope.staff_types = promise.stf_types;
                    //$scope.Department_types = promise.department_types;
                    //$scope.Designation_types = promise.designation_types;

                    $scope.leave_name = promise.leave_name;
                    $scope.credit_month = promise.credit_month;
                    $scope.get_year = promise.get_year;
                    //$scope.get_emp = promise.get_emp;

                    $scope.temp_type_arr = promise.stf_types;
                    $scope.temp_department_arr = promise.department_types;
                    $scope.temp_designation_arr = promise.designation_types;
                    $scope.temp_employee_arr = promise.get_emp;

                    $scope.count = $scope.Department_types.length;
                    $scope.count1 = $scope.Designation_types.length;
                    $scope.countl = $scope.get_emp.length;

                    //if ($scope.count == 0) {
                    //    swal("Data not Found !!");
                    //    $scope.ckdept = false;
                    //}
                    $scope.all_check_Ins();
                });
        };



        $scope.cleardata = function () {
            $state.reload();

        };
        $scope.all_check = function (typecheck) {
            var toggleStatus = typecheck;
            angular.forEach($scope.staff_types, function (itm) {
                itm.class = toggleStatus;
            });
            $scope.get_departments();
        };
        $scope.all_check_dept = function (deptcheck) {
            var toggleStatus = deptcheck;
            angular.forEach($scope.Department_types, function (itm) {
                itm.class1 = toggleStatus;
            });
            $scope.get_designation();
        };
        $scope.all_check_desg = function (desgcheck) {
            var toggleStatus = desgcheck;
            angular.forEach($scope.Designation_types, function (itm) {
                itm.class2 = toggleStatus;
            });
            $scope.get_Employees();
        };
        $scope.all_check_emp = function () {
            var toggleStatus = $scope.empcheck;
            angular.forEach($scope.get_emp, function (itm) {
                itm.emplname = toggleStatus;
            });
        };

        $scope.Inschkbx = function () {
            $scope.Inscheck = $scope.Institution_List.every(function (options) {
                return options.class0;
            });
            $scope.get_stafftypes();
        };
        $scope.togchkbx = function () {
            $scope.usercheck = $scope.staff_types.every(function (options) {
                return options.class;
            });
            $scope.get_departments();
        };

        $scope.deptchkbx = function () {
            $scope.deptcheck = $scope.Department_types.every(function (options) {
                return options.class1;
            });
            $scope.get_designation();
        };
        $scope.desgchkbx = function () {
            $scope.desgcheck = $scope.Designation_types.every(function (options) {
                return options.class2;
            });
            $scope.get_Employees();

        };

        $scope.emp_namechkbx = function () {
            $scope.empcheck = $scope.get_emp.every(function (options) {
                return options.emplname;
            });
        };
        //------------------------------Enable and Disable

        $scope.checkleave = function () {
            if ($scope.ckvalleave === 1) {
                $scope.actleave = false;
            }
            else {
                $scope.actleave = true;
            }
        };

        $scope.showmonth = function () {
            $scope.obj.myDate_from = "";
            $scope.obj.myDate_to = "";
            if ($scope.monthcheck === 1) {
                $scope.actvmonth = false;
            }
            else {
                $scope.actvmonth = true;
            }
        };
        $scope.showyear = function () {
            if ($scope.checkboxval === 1) {
                $scope.actvyear = false;
            }
            else {
                $scope.actvyear = true;
            }
        };
        $scope.showfromdate = function () {
            $scope.classmonth = "";
            $scope.yearname = "";
            if ($scope.checkboxval_from === 1) {
                $scope.actvfromdate = false;
            }
            else {
                $scope.actvfromdate = true;
            }
        };
        $scope.showtodate = function () {
            if ($scope.checkboxval_to === 1) {
                $scope.actvtodate = false;
            }
            else {
                $scope.actvtodate = true;
            }
        };

        $scope.compdate = function () {
            var a = new Date($scope.obj.myDate_from);
            var b = new Date($scope.obj.myDate_to);

            if (a <= b) {
                return true;
            }
            else {
                swal("To Date must be greater then From selected Date !!!!");
            }
        };
        //$scope.count = 0;

        //Snippet of staff types linking from Institution List
        $scope.get_stafftypes2 = function () {
            $scope.selectedemptypes = [];
            angular.forEach($scope.Institution_List, function (role) {
                if (role.class0) $scope.selectedemptypes.push(role);
            });
            if ($scope.selectedemptypes.length !== 0) {
                var data = {
                    emptypes: $scope.selectedemptypes
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
            }
            else if ($scope.selectedemptypes.length === 0) {
                $scope.Designation_types = $scope.temp_designation_arr;
                $scope.count1 = $scope.Designation_types.length;
            }
        };



       

        //  $scope.countl = 0;

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



        $scope.gridReport = {
            enableFiltering: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'hrmE_Id', displayName: 'Employee ID' },
                { name: 'hrmgT_EmployeeGroupType', displayName: 'Group Type' },
                { name: 'hrmD_DepartmentName', displayName: 'Department' },
                { name: 'hrmdeS_DesignationName', displayName: 'Designation' },
                { name: 'hrmE_EmployeeFirstName', displayName: 'Employee Name' },
                { name: 'hrmL_LeaveName', displayName: 'Leave Name' },
                { name: 'hrmlY_FromDate', displayName: 'From Date' },
                { name: 'hrmlY_ToDate', displayName: 'To Date' }
            ]
        };


        $scope.leavedetails = function (hrmeid) {
            $scope.hrmeid = hrmeid.hrmE_Id;
        };

        //To Clear array ehrn check box is clicked
        $scope.clear_array = function () {
            $scope.activityIds = [];
            $scope.InstitutionList = [];
        };




        $scope.all_check_Ins = function () {
            var data = {
                "Is_Active": 1,
                emptypes: []
            };
            apiService.create("LeaveReport/get_Employees", data).
                then(function (promise) {


                    $scope.get_emp = promise.get_emp;
                    $scope.countl = $scope.get_emp.length;

                    if ($scope.countl === 0 && ($scope.selectedemptypes.length !== 0)) {
                        swal("No Employee Mapped with Selected Designation !!!!");
                    }
                });
        };



        $scope.get_report = function () {
            $scope.activityIds = [];
            $scope.InstitutionList = [];
            if ($scope.myForm.$valid) {
                $scope.selectedemptypes = [];
               

                if ($scope.obj.allind === 'All') {
                    angular.forEach($scope.get_emp, function (itm1) {
                        $scope.selectedemptypes.push(itm1.hrmE_Id);
                    });
                }
                else {
                    $scope.selectedemptypes.push($scope.hrmeid);
                }

                var instituteid=0
                if ($scope.allinsti == true) {
                    instituteid = 999;
                }
                else {
                    instituteid = $scope.mI_Id.mI_Id
                }

                $scope.selectedLeave = '';
                if ($scope.obj.hrmL_Id === '999') {
                    angular.forEach($scope.leave_name, function (itm1) {
                        if (itm1.hrmL_LeaveName) {
                            if ($scope.selectedLeave === '') { $scope.selectedLeave = itm1.hrmL_Id; }
                            else {
                                $scope.selectedLeave = $scope.selectedLeave + ',' + itm1.hrmL_Id;
                            }
                        }
                    });
                }
                else {
                    $scope.selectedLeave = $scope.obj.hrmL_Id;
                }

                $scope.from_date = $scope.obj.myDate_from;
                $scope.to_date = $scope.obj.myDate_to;              
                if ($scope.chkfo == false) {
                    var data = {
                        "HRML_Id": $scope.obj.hrmL_Id,
                      //  "Master_Id":instituteid,
                        "HRELTD_FromDate": new Date($scope.from_date).toDateString(),
                        "HRELT_ToDate": new Date($scope.to_date).toDateString(),
                        selectedreport: $scope.selectedemptypes,
                        selectedLeave: $scope.selectedLeave,
                        checkbox_value: $scope.chkfo
                    };
                }
                else {
                    var data = {
                        selectedreport: $scope.selectedemptypes,
                        checkbox_value: $scope.chkfo
                    };
                }
                apiService.create("LeaveReport/get_report", data).
                    then(function (promise) {
                        if ((promise.activityIds != null && promise.activityIds.length > 0) || (promise.report_details != null && promise.report_details.length > 0)) {
                            $scope.hrmL_LeaveCode = promise.hrmL_LeaveCode;
                            $scope.hrmL_LeaveName = promise.hrmL_LeaveName;
                            $scope.activityIds = promise.activityIds;
                            $scope.LeaveReportList = promise.report_details;
                            $scope.InstitutionList = [];
                            angular.forEach($scope.LeaveReportList, function (rep) {
                                if ($scope.InstitutionList.length == 0) {
                                    $scope.InstitutionList.push({ MI_Id: rep.MI_Id, MI_Name: rep.MI_Name });
                                }
                                else if ($scope.InstitutionList.length > 0) {
                                    var trc1 = 0;
                                    angular.forEach($scope.InstitutionList, function (tf) {
                                        if (tf.MI_Id == rep.MI_Id) {
                                            trc1 += 1;
                                        }
                                    })
                                    if (trc1 == 0) {
                                        $scope.InstitutionList.push({ MI_Id: rep.MI_Id, MI_Name: rep.MI_Name });
                                    }
                                }
                            })

                            angular.forEach($scope.InstitutionList, function (ins) {
                                $scope.employelist = [];
                                angular.forEach($scope.LeaveReportList, function (rep) {
                                    if (ins.MI_Id == rep.MI_Id) {
                                        if ($scope.employelist.length == 0) {
                                            $scope.employelist.push({ EmployeeCode: rep.EmployeeCode, EmpName: rep.EmpName });
                                        }
                                        else if ($scope.employelist.length > 0) {
                                            var trc2 = 0;
                                            angular.forEach($scope.employelist, function (tf) {
                                                if (tf.EmployeeCode == rep.EmployeeCode) {
                                                    trc2 += 1;
                                                }
                                            })
                                            if (trc2 == 0) {
                                                $scope.employelist.push({ EmployeeCode: rep.EmployeeCode, EmpName: rep.EmpName });
                                            }
                                        }
                                    }
                                })
                                ins.EMPLST = $scope.employelist;
                            })

                            angular.forEach($scope.InstitutionList, function (ins) {
                                angular.forEach(ins.EMPLST, function (empl) {
                                    $scope.leavetemp = [];
                                    angular.forEach($scope.LeaveReportList, function (lrl) {
                                        if (lrl.EmployeeCode == empl.EmployeeCode && ins.MI_Id == lrl.MI_Id) {
                                            $scope.leavetemp.push(lrl);
                                        }
                                    })
                                    empl.LeaveList = $scope.leavetemp;
                                })
                            })
                            if ($scope.hrmL_LeaveCode != "OD") {
                                angular.forEach($scope.activityIds, function (value, key) {
                                    var fdate = value.HRELT_FromDate.split('T');
                                    value.HRELT_FromDate = fdate[0];
                                    var tdate = value.HRELT_ToDate.split('T');
                                    value.HRELT_ToDate = tdate[0];
                                });
                            }
                            else {
                                angular.forEach($scope.activityIds, function (value, key) {
                                    var fdate = value.hrelaP_FromDate.split('T');
                                    value.hrelaP_FromDate = fdate[0];
                                    var tdate = value.hrelaP_ToDate.split('T');
                                    value.hrelaP_ToDate = tdate[0];
                                });
                            }

                            $scope.EmployeeDis = true;
                        }
                        else {
                            $scope.EmployeeDis = false;
                            swal("No Data Found !!");
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.get_stafftypes = function (companyid) {
            var data = {
                "Master_Id": companyid.mI_Id,
            };
            apiService.create("LeaveReport/get_stafftypes", data).
                then(function (promise) {
                    if (promise.staff_List !== null && promise.staff_List.length > 0) {
                        $scope.staff_types = promise.staff_List;
                    }
                    else {
                        swal(" No Record Found !");
                    }
                });
        };


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
                        //$scope.count = $scope.Department_types.length;

                        //if ($scope.count === 0 && ($scope.selectedemptypes.length !== 0)) {
                        //    swal("No Department Are Mapped with Selected Group Type !!!!");
                        //}
                    });
            }
            else if ($scope.selectedemptypes.length === 0) {
                //$scope.Department_types = $scope.temp_department_arr;
                //$scope.count = $scope.Department_types.length;
            }
        };

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

        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'LeaveReport');

            $timeout(function () { location.href = exportHref; }, 100);
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };

        $scope.All_Individual = function () {
            if ($scope.obj.allind === 'Indi')
                $scope.disabledata = false;
            else
                // $scope.hrmE_Id = "";
                $scope.disabledata = true;
        };
    }
})();