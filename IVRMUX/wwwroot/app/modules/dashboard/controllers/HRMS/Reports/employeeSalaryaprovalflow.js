(function () {
    'use strict';
    angular
        .module('app')
        .controller('CumulativeSalaryReportapprovalController', CumulativeSalaryReportapprovalController)

    CumulativeSalaryReportapprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function CumulativeSalaryReportapprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object
        $scope.currentPage = 10;
        $scope.itemsPerPage = 10;

        //Employeee

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("CumulativeSalaryReport/getalldetails", pageid).then(function (promise) {

                if (promise.employeeDetails !== null && promise.employeeDetails.length > 0) {
                    $scope.employeeDetails = promise.employeeDetails;
                    $('#employeeholdlist').modal('show');
                }
                else {
                    $('#employeeholdlist').modal('hide');
                }
                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {

                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }

                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;


                }
                if (promise.companydetails !== null && promise.companydetails.length > 0) {
                    $scope.companydetails = promise.companydetails;
                }


                if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                    $scope.employeedropdown = promise.employeedropdown;
                }


                if (promise.groupTypedropdown !== null && promise.groupTypedropdown.length > 0) {
                    $scope.groupTypedropdown = promise.groupTypedropdown;
                    $scope.groupTypeselectedAll = true;
                    $scope.GetEmployeeBygroupTypeAll($scope.groupTypeselectedAll);

                }


                if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                    $scope.departmentdropdown = promise.departmentdropdown;
                    $scope.departmentselectedAll = true;
                    $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                }

                if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                    $scope.designationdropdown = promise.designationdropdown;

                    $scope.designationselectedAll = true;
                    $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);


                }

                if (promise.configurationDetails != null) {

                    $scope.SalaryFromDay = promise.configurationDetails.hrC_SalaryFromDay;
                    $scope.SalaryToDay = promise.configurationDetails.hrC_SalaryToDay;

                }

            })
        }

        $scope.salaryapprovedgridview = [];
        $scope.salaryholddgridview = [];
        //

        $scope.getTotal = function () {
            var total = 0;
            for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                var product = $scope.employeeSalaryslipDetails[i];
                total += product.totalEmployees;
            }
            return Math.round(total);
        }





        $scope.employeeSalaryslipDetails = [];
        $scope.institutionDetails = {};
        $scope.EmployeeDis = false;
        //Search employee
        $scope.submitted = false;



        $scope.monthdays = "";
        $scope.earningheadlist = [];
        $scope.deductionheadlist = [];
        $scope.employeeSalaryslipDetails = [];

        $scope.SearchEmployee = function () {
            $scope.EmployeeDis = false;
            $scope.submitted = true;
            $scope.earningheadlist = [];
            $scope.deductionheadlist = [];
            $scope.employeeSalaryslipDetails = [];
            if ($scope.myForm.$valid) {
                $scope.monthdays = "";
                if ($scope.monthdropdown != null && $scope.monthdropdown.length > 0) {
                    angular.forEach($scope.monthdropdown, function (itm) {
                        if (itm.ivrM_Month_Name == $scope.Employee.hreS_Month) {
                            // monthdays.push(itm.hrmgT_Id);//
                            $scope.monthdays = Number(itm.ivrM_Month_Max_Days);
                        }

                    });
                }



                $scope.institutionDetails = {};
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
                    "HRES_Year": $scope.Employee.hreS_Year,
                    "HRES_Month": $scope.Employee.hreS_Month,
                    "institutionID": $scope.Employee.institutionID,
                    groupTypeIdList: groupTypeselected,
                    hrmD_IdList: departmentselected,
                    hrmdeS_IdList: designationselected,
                    "comm": "1"
                }

                apiService.create("CumulativeSalaryReport/getapprvaldata", data).
                    then(function (promise) {

                        if (promise.earninganddeduction !== null && promise.earninganddeduction.length > 0) {
                            $scope.earninganddeduction = promise.earninganddeduction;
                        }

                        angular.forEach($scope.earninganddeduction, function (aa) {
                            if (aa.hrmeD_EarnDedFlag == 'Earning') {
                                $scope.earningheadlist.push({ HRMED_EDTypeFlag: aa.hrmeD_EDTypeFlag })
                            }
                        })
                        angular.forEach($scope.earninganddeduction, function (aa) {
                            if (aa.hrmeD_EarnDedFlag == 'Deduction') {
                                $scope.deductionheadlist.push({ HRMED_EDTypeFlag: aa.hrmeD_EDTypeFlag })
                            }
                        })






                        $scope.prevremarksflag = 0;

                        if (promise.employeeSalaryslipDetails !== null && promise.employeeSalaryslipDetails.length > 0) {
                            $scope.EmployeeDis = true;

                            $scope.employeeSalaryslipDetails = promise.employeeSalaryslipDetails;


                            angular.forEach($scope.employeeSalaryslipDetails, function (itm) {
                                if (itm.prevremarks == undefined) {
                                    $scope.prevremarksflag = 0;
                                    
                                }
                                else {
                                    $scope.prevremarksflag = $scope.prevremarksflag + 1;
                                }
                            });


                            $scope.grossEarning = 0;
                            $scope.grossDeduction = 0;
                            $scope.TotalSalary = 0;

                            angular.forEach($scope.employeeSalaryslipDetails, function (itm) {
                                $scope.grossEarning = parseFloat($scope.grossEarning + itm.TotalEarnings);
                            });
                            $scope.grossEarning.toFixed(2);
                            angular.forEach($scope.employeeSalaryslipDetails, function (itm) {
                                $scope.grossDeduction = parseFloat($scope.grossDeduction + itm.Totaldeduction);
                            });
                            $scope.grossDeduction.toFixed(2);

                            angular.forEach($scope.employeeSalaryslipDetails, function (itm) {
                                $scope.TotalSalary = parseFloat($scope.TotalSalary + itm.TotalPayable);
                                //$scope.TotalSalary = 

                            });
                            $scope.TotalSalary.toFixed(2);

                            angular.forEach($scope.employeeSalaryslipDetails, function (itm) {
                                itm.HRESA_Remarks = "Approved";
                            });

                            $scope.alltask = true;
                            $scope.all_task($scope.alltask)


                            $scope.earnlen = $scope.earningheadlist.length;
                            $scope.dedlen = $scope.deductionheadlist.length;
                        }
                        else {
                            $scope.EmployeeDis = false;
                            swal('No Record found to display .. !');
                            return;
                        }

                    })
            }

        }

        $scope.usercheckC2 = "";
        $scope.get_evalistt = function (user) {
            $scope.usercheckC2 = $scope.employeeSalaryslipDetails.every(function (options) {
                return options.selectedd2;
            });


            angular.forEach($scope.employeeSalaryslipDetails, function (itm) {
                if (user.selectedd2 == false) {
                    user.HRESA_Remarks = "";
                }
                else {
                    user.HRESA_Remarks = "Approved";
                }
            });


        };

        $scope.selectedAll1 = "";
        $scope.all_task = function (alltask) {
            var checkStatus2 = alltask;
            angular.forEach($scope.employeeSalaryslipDetails, function (itm) {
                itm.selectedd2 = checkStatus2;

                if (itm.selectedd2 == false) {
                    itm.HRESA_Remarks = "";
                }
                else {
                    itm.HRESA_Remarks = "Approved";
                }
            });
        };



        //By group Type
        $scope.GetEmployeeBygroupTypeAll = function (groupTypeselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.groupTypeselectedAll;

            angular.forEach($scope.groupTypedropdown, function (itm) {
                itm.selected = toggleStatus;
            });

            angular.forEach($scope.designationdropdown, function (itm22) {
                itm22.selected = toggleStatus;
                $scope.designationselectedAll = toggleStatus;
            });

            angular.forEach($scope.departmentdropdown, function (itm232) {
                itm232.selected = toggleStatus;
                $scope.departmentselectedAll = toggleStatus;
            });

            $scope.get_depts();
        }

        //single
        $scope.GetEmployeeBygroupType = function (groupType) {
            $scope.departmentdropdown = [];
            $scope.designationdropdown = [];
            $scope.designationselectedAll = false;
            $scope.departmentselectedAll = false;

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
            apiService.create("CumulativeSalaryReport/get_depts", data).
                then(function (promise) {

                    if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                        $scope.departmentdropdown = promise.departmentdropdown;
                        $scope.departmentselectedAll = true;
                        $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
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
            angular.forEach($scope.designationdropdown, function (itm1) {
                itm1.selected = toggleStatus;
                $scope.designationselectedAll = toggleStatus;

            })

            $scope.get_desig();

        }

        //GET ALL SELECT
        $scope.GetEmployeeAll = function (employee) {
            var toggleStatus = $scope.employeeSelectedAll;
            angular.forEach($scope.employeeSalaryslipDetails, function (itm) {
                itm.selected = toggleStatus;
            });
        };

        $scope.GetEmployee = function (employee) {

            $scope.employeeSelectedAll = $scope.employeeSalaryslipDetails.every(function (itm) {
                return itm.selected;
            });

            if (employee.selected) {
                employee.disable = false;
            }
            else {

                employee.disable = true;
            }

        }
        //By Department Single
        $scope.GetEmployeeByDepartment = function (department) {
            $scope.designationdropdown = [];
            $scope.designationselectedAll = false;
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {

                return itm.selected;
            });
            $scope.get_desig();
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
            apiService.create("CumulativeSalaryReport/get_desig", data).
                then(function (promise) {
                    if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                        $scope.designationdropdown = promise.designationdropdown;
                        $scope.designationselectedAll = true;
                        $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                    }
                })
        };


        $scope.SetMonthByYear = function () {

            $scope.salaryapprovedgridview = [];
            $scope.salaryholddgridview = [];
            $scope.employeeSalaryslipDetails = [];
            $scope.HRES_Year = $scope.Employee.hreS_Year;
            $scope.HRES_Month = $scope.Employee.hreS_Month;

            if ($scope.Employee.hreS_Year.length > 0 && $scope.Employee.hreS_Month.length > 0) {
                var data = {
                    "HRES_Year": $scope.Employee.hreS_Year,
                    "HRES_Month": $scope.Employee.hreS_Month,
                    "institutionID": $scope.Employee.institutionID,
                }
            }

            apiService.create("CumulativeSalaryReport/SalaryApprovalgriddata", data).then(function (promise) {
                if (promise.salaryapprovedgridview !== null && promise.salaryapprovedgridview.length > 0) {
                    $scope.salaryapprovedgridview = promise.salaryapprovedgridview;
                }
                if (promise.salaryholddgridview !== null && promise.salaryholddgridview.length > 0) {
                    $scope.salaryholddgridview = promise.salaryholddgridview;
                }
            })
        };


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


        //Clear data
        $scope.Employee = {};
        $scope.cleardata = function () {
            $state.reload();

        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.UpdateEmployeeDetails = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                $scope.employeeselected = [];
                $scope.employeeselectedfalserec = [];
                $scope.employeeselectedapproved = [];

                var checkStatus2 = $scope.alltask;
                angular.forEach($scope.employeeSalaryslipDetails, function (emp) {
                    if (emp.selectedd2 === true) {
                        $scope.employeeselected.push({ HRES_Id: emp.HRES_Id, HRESA_Remarks: emp.HRESA_Remarks, retrunMsg:"Approved" });
                        $scope.employeeselectedapproved.push({ HRES_Id: emp.HRES_Id, HRESA_Remarks: emp.HRESA_Remarks, retrunMsg:"Approved" });
                    }
                    else {
                        $scope.employeeselected.push({ HRES_Id: emp.HRES_Id, HRESA_Remarks: emp.HRESA_Remarks, retrunMsg: "Hold" });
                        $scope.employeeselectedfalserec.push({ HRES_Id: emp.HRES_Id, HRESA_Remarks: emp.HRESA_Remarks });
                    }
                });

                //angular.forEach($scope.employeeSalaryslipDetails, function (emp) {
                //    if (emp.selectedd2 === false) {
                //        $scope.employeeselectedfalserec.push({ HRES_Id: emp.HRES_Id, HRESA_Remarks: emp.HRESA_Remarks });
                //    }
                //});


                swal({
                    title: "Are you sure?",
                    //text: "Do You Want To Updated The Record..",
                    text: "Do You Want To Updated The Record.. \n Salary Approval Count :  " + $scope.employeeselectedapproved.length + ", Salary Hold Count: " + $scope.employeeselectedfalserec.length,
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes, Update it!",
                    cancelButtonText: "Cancel..!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {



                            if ($scope.employeeselected.length > 0) {
                                var data = {
                                    "comm": "Approved",
                                    selectedEmpdetails: $scope.employeeselected,
                                    "HRES_Year": $scope.Employee.hreS_Year,
                                    "HRES_Month": $scope.Employee.hreS_Month,
                                    "MI_Id": $scope.Employee.institutionID
                                };
                                apiService.create("CumulativeSalaryReport/SalaryApproval", data).
                                    then(function (promise) {
                                        $scope.returnmsg = "Updated";


                                        if (promise.retrunMsg !== "") {
                                            if (promise.retrunMsg == "false") {
                                                // swal("Record Not Updated..");
                                                $scope.returnmsg = "NotUpdated";                                               
                                                return;
                                            }
                                            else if (promise.retrunMsg == "updated") {                                              
                                                $scope.returnmsg = "Updated";                                             

                                                if ($scope.employeeselectedapproved.length > 0 || $scope.employeeselectedfalserec.length > 0 && returnmsg == 1) {
                                                    swal("Record Updated Successfully.. \n Salary Approved :  " + $scope.employeeselectedapproved.length + ", Salary Hold : " + $scope.employeeselectedfalserec.length);


                                                    $scope.cleardata();

                                                }
                                                else {
                                                    swal("Contact Administrator..");
                                                }


                                            }
                                            else if (promise.retrunMsg == "Configuration Message") {
                                                // swal("You are not allowed to update Salary");

                                            }

                                        }
                                        else {
                                            $scope.EmployeeDis = false;
                                            //swal('No Record found to Update .. !');
                                            return;
                                        }


                                    })
                            }
                            //if ($scope.employeeselectedfalserec.length > 0) {

                            //    var data = {
                            //        "comm": "Hold",
                            //        selectedEmpdetails: $scope.employeeselectedfalserec,
                            //        "HRES_Year": $scope.Employee.hreS_Year,
                            //        "HRES_Month": $scope.Employee.hreS_Month
                            //    };
                            //    apiService.create("CumulativeSalaryReport/SalaryApproval", data).
                            //        then(function (promise) {

                            //            if (promise.retrunMsg !== "") {
                            //                if (promise.retrunMsg == "false") {
                            //                    // swal("Record Not Updated..");
                            //                    returnmsg = 0;
                            //                    return;
                            //                }
                            //                else if (promise.retrunMsg == "updated") {
                            //                    //swal("Record Updated Successfully..");
                            //                    returnmsg = 1;


                            //                }
                            //                else if (promise.retrunMsg == "Configuration Message") {
                            //                    // swal("You are not allowed to update Salary");

                            //                }

                            //            }
                            //            else {
                            //                $scope.EmployeeDis = false;
                            //                //swal('No Record found to Update .. !');
                            //                return;
                            //            }
                            //        })
                            //}



                            

                        }
                        else {
                            swal("Cancelled");
                            // $state.reload();
                        }
                    });





            }
            else {
                swal("Please Enter Remarks");
            }
        }

        //$scope.UpdateEmployeeDetails = function () {
        //    $scope.submitted = true;
        //    if ($scope.myForm.$valid) {

        //        $scope.employeeselected = [];
        //        angular.forEach($scope.employeeSalaryslipDetails, function (itm) {
        //            if (itm.selected) {
        //                $scope.employeeselected.push(itm);
        //            }
        //        });
        //        $scope.hresD_Id = $scope.employeeselected[0].earningresult.HRESD_Id;
        //        if ($scope.employeeselected.length == 0) {
        //            swal('Kindly select atleast one record');
        //            return;
        //        }

        //        var data = {
        //            selectedEmpdetails: $scope.employeeselected,
        //            selectedEmpssdetails: $scope.employeeselected,
        //            "HRES_Id": $scope.hreS_Id,
        //            "HRESD_Id": $scope.hresD_Id
        //        };



        //        // var data = $scope.Employee;

        //        apiService.create("salaryUpdation/getEmployeedetailsBySelection", data).
        //            then(function (promise) {

        //                if (promise.retrunMsg !== "") {
        //                    if (promise.retrunMsg == "false") {
        //                        swal("Record Not Updated..");
        //                        return;
        //                    }
        //                    else if (promise.retrunMsg == "updated") {
        //                        swal("Record Updated Successfully..");
        //                        $scope.cleardata();
        //                    }
        //                    else if (promise.retrunMsg == "Configuration Message") {
        //                        swal("You are not allowed to update Salary");
        //                        $scope.cleardata();
        //                    }

        //                }
        //                else {
        //                    $scope.EmployeeDis = false;
        //                    swal('No Record found to Update .. !');
        //                    return;
        //                }
        //            })
        //    }
        //}


        $scope.printData = function () {
            var divToPrint = document.getElementById("Table");
            var newWin = window.open();
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();
            // $state.reload();
        }

        //Total for per column

        $scope.TotalgrossEarning = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails != null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {


                    var product = $scope.employeeSalaryslipDetails[i];
                    total += product.grossEarning;
                }
            }

            return Math.round(total);
        }

        $scope.TotalgrossDeduction = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails != null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {


                    var product = $scope.employeeSalaryslipDetails[i];
                    total += product.grossDeduction;
                }
            }

            return Math.round(total);
        }

        $scope.TotalnetSalary = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails != null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {


                    var product = $scope.employeeSalaryslipDetails[i];
                    total += product.netSalary;
                }
            }

            return Math.round(total);
        }



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
        }


    }


})();