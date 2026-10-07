(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_UserEmployeeMappingController', ISM_UserEmployeeMappingController);

    ISM_UserEmployeeMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function ISM_UserEmployeeMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 10;
        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;
        $scope.self = true;
        $scope.page1 = "page1";
        $scope.page2 = "page2";
        $scope.search = "";
        $scope.obj = {};
        $scope.employeeList = [];
        $scope.tempemployeeList = [];
        $scope.userflg = false;
        $scope.obj.proxyflag = false;
        //=========================================Load data
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("ISM_UserEmployeeMapping/getdetails", pageid).then(function (promise) {
                $scope.get_departmentlist = promise.get_departmentlist;
                $scope.get_userslist = promise.get_userslist;
                $scope.get_userEmpcount = promise.get_userEmpcount;
                $scope.presentCountgrid = $scope.get_userEmpcount.length;
                $scope.get_employeelist = promise.get_employeelist;
            });
        };
        //============================ User Change
        $scope.onuserchange = function () {

            var count = 0;
            var selectedhrmeid = 0;
            angular.forEach($scope.get_employeelist, function (itm) {
                if (itm.selected === true) {
                    count = count + 1;
                    selectedhrmeid = itm.hrmE_Id;
                }
            });
            if (count === 1) {
                if (selectedhrmeid === $scope.obj.user_Id.hrmE_Id) {
                    $scope.order = '0';
                }
                else {
                    $scope.order = '';
                }
            }
            else {
                $scope.order = '';
            }

            //var data = {
            //    "User_Id": $scope.obj.user_Id.user_Id
            //};
            //apiService.create("ISM_UserEmployeeMapping/getmappedEmplist", data).then(function (promise) {
            //    if (promise.get_mappedEmplist.length > 0) {
            //        $scope.get_mappedEmplist = promise.get_mappedEmplist;
            //    }
            //    else {
            //        swal("No Record  Found..... !!");
            //        $scope.get_mappedEmplist = "";
            //    }
            //});
        };
        $scope.deletesecondgriddata = function (index, stuDelRecord, currenrow) {
            $scope.employeeList.splice($scope.employeeList.indexOf(stuDelRecord), 1);
            $scope.seletedcount = $scope.employeeList.length;
        }
        //============================ department Change
        $scope.ondepartmentchange = function (objd) {
            $scope.deptid = objd.HRMD_Id.HRMD_Id;
            var data = {
                "User_Id": objd.user_Id.user_Id,
                "HRMD_Id": $scope.deptid
            };
            apiService.create("ISM_UserEmployeeMapping/getemployeelist", data).then(function (promise) {
                if (promise.get_employeelist.length > 0) {
                    $scope.get_employeelist = promise.get_employeelist;
                }
                else {
                    swal("No Record  Found..... !!");
                    $scope.get_employeelist = "";
                }
            });
        };

        $scope.all_checkE = function () {
            var checkStatus = $scope.usercheckE;
            angular.forEach($scope.get_employeelist, function (itm) {
                itm.selected = checkStatus;
            });
            var count = 0;
            angular.forEach($scope.get_employeelist, function (itm) {
                if (itm.selected === true) {
                    count = count + 1;
                }
            });
            if (count > 1) {
                $scope.self = false;
            }
            else {
                $scope.self = true;
            }
        };

        $scope.togchkbxE = function () {
            $scope.usercheckE = $scope.get_employeelist.every(function (options) {
                return options.selected;
            });
            var count = 0;
            angular.forEach($scope.get_employeelist, function (itm) {
                if (itm.selected === true) {
                    count = count + 1;
                }                
            });
            if (count > 1) {
                $scope.self = false;
            }
            else {
                $scope.self = true;
            }
        };

        $scope.isOptionsRequired = function () {
            return !$scope.get_employeelist.some(function (options) {
                return options.selected;
            });
        };

        //============================= Add Employee 
        $scope.employeeList = [];
        $scope.reportinghead = "";
        $scope.AddEmployee = function () {
            if ($scope.obj.user_Id != undefined) {
                $scope.reportinghead = $scope.obj.user_Id.usersname;
                $scope.reportingheadhrmeid = $scope.obj.user_Id.hrmE_Id;
                if ($scope.order != null && $scope.order != undefined && $scope.order != "") {
                    var emplist = [];
                    var selfselected = false;
                    angular.forEach($scope.get_employeelist, function (empl) {
                        if (empl.selected === true) {
                            if ($scope.order === '0') {
                                if (empl.hrmE_Id === $scope.reportingheadhrmeid) {
                                    emplist.push({ hrmE_Id: empl.hrmE_Id, employeename: empl.employeename, hrmE_EmployeeCode: empl.hrmE_EmployeeCode, ismusemM_Order: $scope.order, proxyflag : false });
                                }
                                else {
                                    selfselected = true;
                                    return;
                                };
                            }
                            else {
                                if (empl.hrmE_Id === $scope.reportingheadhrmeid) {
                                    emplist.push({ hrmE_Id: empl.hrmE_Id, employeename: empl.employeename, hrmE_EmployeeCode: empl.hrmE_EmployeeCode, ismusemM_Order: 0, proxyflag: false });
                                }
                                else {
                                    emplist.push({ hrmE_Id: empl.hrmE_Id, employeename: empl.employeename, hrmE_EmployeeCode: empl.hrmE_EmployeeCode, ismusemM_Order: $scope.order, proxyflag: false});
                                }
                            }                                             
                        }
                    });
                    if (emplist.length > 0) {
                        angular.forEach(emplist, function (emp) {
                            if ($scope.employeeList != null && $scope.employeeList.length > 0) {
                                var exist = false;
                                angular.forEach($scope.employeeList, function (ex) {
                                    if (ex.hrmE_Id === emp.hrmE_Id) {
                                        exist = true;
                                    }
                                });
                                if (exist === false) {
                                    $scope.employeeList.push(emp);
                                    $scope.seletedcount = $scope.employeeList.length;
                                }
                            }
                            else {
                                $scope.employeeList.push(emp);
                                $scope.seletedcount = $scope.employeeList.length;
                            }
                            $scope.userflg = true;
                        });
                    }
                    else {
                        if (selfselected === true) {
                            swal("Self level cant map to other user!");
                        } else {
                            swal("Select Atleast one Employee from list.... !!");
                        }                        
                    }
                    angular.forEach($scope.get_employeelist, function (ex) {
                        ex.selected = false;
                    });
                    $scope.searchvalueE = "";
                    $scope.usercheckE = false;
                    $scope.order = '';
                }
                else {
                    swal("Select Reporting Level !!");
                }
            }
            else {
                swal("Please Select Reporting Head/User!!");
            }
        };

   
        //============================================ SAVE DATA
        
        $scope.savedata = function (obj) {
            //if ($scope.myForm.$valid) {
            $scope.tempemployeeList = [];
            angular.forEach($scope.employeeList, function (aa) {
                $scope.tempemployeeList.push({
                    hrmE_Id: aa.hrmE_Id, employeename: aa.employeename, employeename: aa.employeename, hrmE_EmployeeCode: aa.hrmE_EmployeeCode, ismusemM_Order: aa.ismusemM_Order, proxyflag: aa.proxyflag
                })
            });
            $scope.employeeList = [];
            $scope.employeeList = $scope.tempemployeeList;


                var data = {
                    "User_Id": $scope.obj.user_Id.user_Id,
                    //"HRMD_Id": $scope.deptid,                      
                    "employeeList": $scope.employeeList,
                    "ISMUSEMM_Id": $scope.ismusemM_Id
                };
                apiService.create("ISM_UserEmployeeMapping/savedata", data).then(function (promise) {
                    if (promise.returnval === true) {
                        if (promise.ismusemM_Id === 0 || promise.ismusemM_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.ismusemM_Id > 0) {
                            swal('Record updated successfully');
                        }
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.ismusemM_Id === 0 || promise.ismusemM_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.ismusemM_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                });
            //}
            //else {
            //    $scope.submitted = true;
            //}
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };

        //============================== Model View        
        $scope.onformclick = function (obju) {
            var data = {
                "User_Id": obju
            };
            apiService.create("ISM_UserEmployeeMapping/getemployeelist", data).then(function (promise) {
                if (promise.get_userEmpDetails.length > 0) {
                    $scope.get_userEmpDetails = promise.get_userEmpDetails;
                }
                else {
                    swal("No Record  Found..... !!");
                    $scope.get_userEmpDetails = "";
                }
            });
        };

        $scope.updateorder = function (obju) {
            var data = {
                "employeeList": $scope.get_userEmpDetails
            };
            apiService.create("ISM_UserEmployeeMapping/updateorder", data).then(function (promise) {
                if (promise.returnval == true) {
                    $('#myModal').modal('hide');
                    $('.modal-backdrop').remove();
                    swal("Order updated.. !!");
                }
                else {
                    swal("Order not updated..... !!");
                }
            });
        };


        //=================Active/Deactivate
        $scope.deactive = function (objd) {
            $scope.ISMUSEMM_Id = objd.ismusemM_Id;
            var dystring = "";
            if (objd.ismusemM_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (objd.ismusemM_ActiveFlag === false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ISM_UserEmployeeMapping/deactive", objd).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $('#myModal').modal('hide');
                                $('.modal-backdrop').remove();
                            });
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        };

        //=========================== cancel
        $scope.cancel = function () {
            $state.reload();
        };


    }
})();

