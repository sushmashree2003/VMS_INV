(function () {
    'use strict';
    angular
.module('app')
.controller('EmployeeLoanReportController', EmployeeLoanReportController)

    EmployeeLoanReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache', 'Excel']
    function EmployeeLoanReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache, Excel) {
        //object

        $scope.Type = "Approval";
        $scope.type = 'Approval';
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("EmployeeLoanReport/getalldetails", pageid).then(function (promise) {

                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {

                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }

              if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;


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






        $scope.employeeSalaryslipDetails = [];
        $scope.institutionDetails = {};
        $scope.EmployeeDis = false;
        //Search employee
        $scope.submitted = false;
        $scope.SearchEmployee = function () {
            $scope.EmployeeDis = false;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

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
                  //  "HRELT_Year": $scope.Employee.hreS_Year,
                   // "month": Number($scope.Employee.hreS_Month),
                    groupTypeIdList: groupTypeselected,
                    hrmD_IdList: departmentselected,
                    hrmdeS_IdList: designationselected,
                    "type": $scope.type
                }

                apiService.create("EmployeeLoanReport/getEmployeedetailsBySelection", data).
                    then(function (promise) {

                        if ($scope.type == "Approval") {
                         
                            $scope.employeePayoutDetails = [];
                            $scope.employeeTransDetails = [];
                            if (promise.employeeSalaryslipDetails.length > 0) {
                                $scope.get_deviationreport = promise.employeeSalaryslipDetails;
                            

                                $scope.employeeid = [];

                                angular.forEach($scope.get_deviationreport, function (dev) {
                                    if ($scope.employeeid.length === 0) {
                                        $scope.employeeid.push({
                                            HREL_Id: dev.HREL_Id,
                                            HRME_EmployeeName: dev.HRME_EmployeeName,
                                            HREL_AppliedDate: dev.HREL_AppliedDate,
                                            HREL_EmpLoanId: dev.HREL_EmpLoanId,
                                            HRML_LoanType: dev.HRML_LoanType,
                                            HREL_LoanAmount: dev.HREL_LoanAmount,
                                            HRME_Id: dev.HRME_Id,
                                            HREL_SanctionedAmount:dev.HREL_SanctionedAmount
                                        });
                                    } else if ($scope.employeeid.length > 0) {
                                        var intcount = 0;
                                        angular.forEach($scope.employeeid, function (emp) {
                                            if (emp.HREL_Id === dev.HREL_Id) {
                                                intcount += 1;
                                            }
                                        });
                                        if (intcount === 0) {
                                            $scope.employeeid.push({
                                                HREL_Id: dev.HREL_Id,
                                                HRME_EmployeeName: dev.HRME_EmployeeName,
                                                HREL_AppliedDate: dev.HREL_AppliedDate,
                                                HREL_EmpLoanId: dev.HREL_EmpLoanId,
                                                HRML_LoanType: dev.HRML_LoanType,
                                                HREL_LoanAmount: dev.HREL_LoanAmount,
                                                HRME_Id: dev.HRME_Id,
                                                HREL_SanctionedAmount:dev.HREL_SanctionedAmount
                                            });
                                        }
                                    }
                                });


                                console.log($scope.employeeid);

                                angular.forEach($scope.employeeid, function (ddd) {
                                    $scope.templist = [];
                               
                                    angular.forEach($scope.get_deviationreport, function (dd) {
                                        if (dd.HREL_Id === ddd.HREL_Id) {
                                          
                                            $scope.templist.push(dd);
                                        }
                                    });
                                    ddd.plannerdetails = $scope.templist;
                                    
                                });


                                console.log($scope.employeeid);

                            }

                            else {
                                $scope.employeeid = [];
                                $scope.employeePayoutDetails = [];
                                $scope.employeeTransDetails = [];
                                $scope.EmployeeDis = false;
                                swal('No Record found to display .. !');
                                return;
                            }

                        }
                        else if ($scope.type == "Payout") {

                            if (promise.employeeSalaryslipDetails !== null && promise.employeeSalaryslipDetails.length > 0) {
                                $scope.employeeid = [];
                                $scope.employeeTransDetails = [];
                                $scope.employeePayoutDetails = promise.employeeSalaryslipDetails;
                            }

                            else {
                                $scope.employeeid = [];
                                $scope.employeePayoutDetails = [];
                                $scope.employeeTransDetails = [];

                                $scope.EmployeeDis = false;
                                swal('No Record found to display .. !');
                                return;
                            }
                        }
                        else if ($scope.type == "Transaction") {
                            if (promise.employeeSalaryslipDetails !== null && promise.employeeSalaryslipDetails.length > 0) {
                                $scope.employeeid = [];
                                $scope.employeePayoutDetails = [];
                                $scope.employeeTransDetails = promise.employeeSalaryslipDetails;
                            }

                            else {
                                $scope.employeeid = [];
                                $scope.employeePayoutDetails = [];
                                $scope.employeeTransDetails = [];
                                $scope.EmployeeDis = false;
                                swal('No Record found to display .. !');
                                return;
                            }
                        }

                                

                            })
            }

        }



        $scope.datachange = function (value) {


            $scope.type = value;

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
            $scope.Employee = {};
            $scope.employeeSalaryslipDetails = [];
            $scope.submitted = false;
            $scope.groupTypeselectedAll = false;
            $scope.departmentselectedAll = false;
            $scope.designationselectedAll = false;
            $scope.employeeSelectedAll = false;
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.search = "";

            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.onLoadGetData();

        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };



        $scope.printData = function () {
            var divToPrint = document.getElementById("Table");
            var newWin = window.open();
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();
            // $state.reload();
        }

      
        $scope.printToCart = function () {
            var innerContents = "";
            if ($scope.type == "Approval") {
                innerContents = document.getElementById("employeeApprovalDetails").innerHTML;
            }
            else if ($scope.type == "Payout") {
                innerContents = document.getElementById("employeePayoutDetails").innerHTML;
            }
            else if ($scope.type == "Transaction") {
                innerContents = document.getElementById("employeeTransDetails").innerHTML;
            }
           
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                  '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.ExportToexcel = function () {
            var innerContents = "";
            if ($scope.type == "Approval") {
                innerContents ="employeeApprovalDetails";
            }
            else if ($scope.type == "Payout") {
                innerContents ="employeePayoutDetails";
            }
            else if ($scope.type == "Transaction") {
                innerContents ="employeeTransDetails";
            }
            var exportHref = Excel.tableToExcel(export_id, innerContents);
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };

    }


})();