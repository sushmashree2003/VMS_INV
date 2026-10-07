(function () {
    'use strict';
    angular
.module('app')
        .controller('VapsMusterRollReportController', VapsMusterRollReportController)

    VapsMusterRollReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function VapsMusterRollReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("MonthEndReport/getalldetails", pageid).then(function (promise) {

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
            });
        };

        $scope.onchageDropdownValue = function () {
            $scope.EmployeeDis = false;
            //$scope.GetEmployeeList();
        };

        var chart;
        $scope.employeeSalaryslipDetails = [];
        $scope.EmployeeDis = false;
        //Search employee
        $scope.submitted = false;

        $scope.SearchEmployee = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var groupTypeselected = [];
                angular.forEach($scope.groupTypedropdown, function (itm) {
                    if (itm.selected) {
                        groupTypeselected.push(itm.hrmgT_Id);
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
                //var data = $scope.Employee;
                var data =
                {
                    "HRES_Year": $scope.Employee.hreS_Year,
                    "ivrM_Month_Id": $scope.Employee.ivrM_Month_Id,
                    groupTypeIdList: groupTypeselected,
                    hrmD_IdList: departmentselected,
                    hrmdeS_IdList: designationselected
                };

                apiService.create("MonthEndReport/getEmployeedetailsBySelection", data).
                    then(function (promise) {

                        $scope.selectedMonth = promise.hreS_Month;
                        $scope.showGrafh = true;
                        $scope.selectedYear = promise.hreS_Year;
                        $scope.hreS_Month = promise.hreS_Month;
                        $scope.employeeDetails = promise.employeeDetails;

                        if (promise.institutionDetails != null) {
                            $scope.EmployeeDis = true;
                            $scope.institutionDetails = promise.institutionDetails;

                            var instuteAddress = $scope.institutionDetails.mI_Name;
                            if ($scope.institutionDetails.mI_Address1 != null && $scope.institutionDetails.mI_Address1 != "") {
                                instuteAddress = instuteAddress + ', ' + $scope.institutionDetails.mI_Address1;
                            }
                            if ($scope.institutionDetails.mI_Address2 != null && $scope.institutionDetails.mI_Address2 != "") {
                                instuteAddress = instuteAddress + ', ' + $scope.institutionDetails.mI_Address2;
                            }

                            if ($scope.institutionDetails.mI_Address3 != null && $scope.institutionDetails.mI_Address3 != "") {
                                instuteAddress = instuteAddress + ', ' + $scope.institutionDetails.mI_Address3;
                            }
                            $scope.CurrentInstuteAddress = instuteAddress + ', ' + $scope.institutionDetails.mI_Pincode;
                        }
                        else {
                            $scope.EmployeeDis = false;
                            swal('No Record found to display .. !');
                            return;
                        }

                        $scope.yearlyemprep = promise.employeeAttendance;
                        var a = promise.columnnames.replace(/[[]/g, '');
                        var b = a.replace(/]/g, '');
                        $scope.columnnames = b.split(',');
                        $scope.columnnum = $scope.columnnames.length;
                    
                        var slno = 0;
                        angular.forEach($scope.employeeDetails, function (d) {
                            $scope.attendence = [];
                            d.attendenceList = [];
                            slno += 1;
                            d.slno = slno;
                            angular.forEach($scope.yearlyemprep, function (dd) {
                                if (d.hrmE_Id == dd.HRME_Id) {
                                    $scope.attendence.push(dd);
                                }
                            })
                            d.attendenceList = $scope.attendence;   
                        })

                        $scope.earningheadlist = promise.earningheadlist;
                        $scope.deductionheadlist = promise.deductionheadlist;

                        var earningvalues = [];
                        angular.forEach(promise.empid, function (empid) {
                            var headwisevalue = [];
                            angular.forEach(promise.earningheadlist, function (headname) {
                                var indheadname = {};
                                angular.forEach(promise.earningheadvalues, function(amtvalue){
                                    if (amtvalue.hrmE_Id == empid && amtvalue.hrmeD_Id == headname.hrmeD_Id) {
                                        indheadname = { empid: empid, headid: headname.hrmeD_Id, headvalue: amtvalue.hreeD_Amount };
                                    }
                                });
                                headwisevalue.push(indheadname);
                            });
                            earningvalues.push(headwisevalue);
                        });
                        $scope.earningheadvalues = earningvalues;


                        var deductionvalues = [];
                        angular.forEach(promise.empid, function (empid) {
                            var headwisevalue = [];
                            angular.forEach(promise.deductionheadlist, function (headname) {
                                var indheadname = {};
                                angular.forEach(promise.earningheadvalues, function (amtvalue) {
                                    if (amtvalue.hrmE_Id == empid && amtvalue.hrmeD_Id == headname.hrmeD_Id) {
                                        indheadname = { empid: empid, headid: headname.hrmeD_Id, headvalue: amtvalue.hreeD_Amount };
                                    }
                                });
                                headwisevalue.push(indheadname);
                            });
                            deductionvalues.push(headwisevalue);
                        });
                        $scope.deductionheadvalues = deductionvalues;
                    });
            }
        };       

        //Clear data
        $scope.Employee = {};
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.employeeSalaryslipDetails = [];
            $scope.submitted = false;
            $scope.EmployeeDis = false;
            $scope.search = "";

            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.onLoadGetData();
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.printToCart = function (Baldwin) {
            //var base64Image = chart.canvas.toDataURL();
            // document.getElementById('rangeBarChat').style.display = 'none';
            //document.getElementById('chartImage').src = base64Image;
            var innerContents = document.getElementById("tablegrp").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();

            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/EMPPFSchemePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };


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
        };

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
            });
            var data = {
                hrmgT_IdList: ids
            };
            apiService.create("EmployeeSalarySlipGeneration/get_depts", data).
                then(function (promise) {

                    if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                        $scope.departmentdropdown = promise.departmentdropdown;
                        $scope.departmentselectedAll = true;
                        $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                    }
                });
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
            });
            $scope.get_desig();
        };

        //By Department Single
        $scope.GetEmployeeByDepartment = function (department) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {

                return itm.selected;
            });
            $scope.get_desig();
        };

        $scope.get_desig = function () {
            var ids = [];
            angular.forEach($scope.groupTypedropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.hrmgT_Id);
                }
            });
            var ids1 = [];
            angular.forEach($scope.departmentdropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids1.push(grp_t.hrmD_Id);
                }
            });
            var data = {
                hrmgT_IdList: ids,
                hrmD_IdList: ids1
            };
            apiService.create("EmployeeSalarySlipGeneration/get_desig", data).
                then(function (promise) {
                    if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                        $scope.designationdropdown = promise.designationdropdown;
                        $scope.designationselectedAll = true;
                        $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                    }
                });
        };

        $scope.GetEmployeeByDesignationAll = function (designationselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.designationselectedAll;
            angular.forEach($scope.designationdropdown, function (itm) {
                itm.selected = toggleStatus;
            });
        };

        $scope.GetEmployeeByDesignation = function (designation) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {

                return itm.selected;
            });
        };

    }


})();
