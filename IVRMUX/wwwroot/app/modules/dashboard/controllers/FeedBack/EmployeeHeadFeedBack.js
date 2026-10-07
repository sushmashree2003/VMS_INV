(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmployeeHeadFeedBackController', EmployeeHeadFeedBackController);
    EmployeeHeadFeedBackController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter', '$compile'];
    function EmployeeHeadFeedBackController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter, $compile) {


        var paginationformasters;
        $scope.multidepartment = false;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.obj = {};
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        $scope.disableemp = false;
        $scope.checkallemp = false;
     
        $scope.submitted = false;
    
        $scope.EmpD = true;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 20;
        $scope.loaddata = function () {
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;

            apiService.getURI("EmployeeHeadFeedBack/getdetails", pageid).then(function (promise) {

                $scope.get_userEmplist = promise.get_userEmplist;
                $scope.departmentdropdown = promise.departmentList;
                $scope.designationdropdown = promise.designation;
                $scope.empdepartment = promise.empdepartment;
                $scope.feedbackgrid = promise.feedbackgrid;
                $scope.presentCountgrid = $scope.feedbackgrid.length;
                
              
                $scope.roleflag = promise.roleflag;
                if ($scope.departmentdropdown.length > 0) {
                    angular.forEach($scope.departmentdropdown, function (uem) {
                        uem.selected = false;
                    });
                    $scope.departmentselectedAll = false;
                }
                if ($scope.designationdropdown.length > 0) {
                    angular.forEach($scope.designationdropdown, function (uem) {
                        uem.selected = false;
                    });
                    $scope.designationselectedAll = false;
                }
                if ($scope.get_userEmplist.length > 0) {
                    angular.forEach($scope.get_userEmplist, function (uem) {
                        uem.empck = false;
                    });
                    $scope.checkall = false;
                }
            });
        };

        //$scope.all_check = function (ack) {
        //    $scope.checkall = ack;
        //    var toggleStatus = $scope.checkall;
        //    angular.forEach($scope.filterValue3, function (uem) {
        //        uem.empck = toggleStatus;
        //    });
            $scope.all_check = function (checkall) {
                var toggleStatus = checkall;
                angular.forEach($scope.filterValue3, function (itm) {
                    itm.empck = toggleStatus;
                });

            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
                $scope.get_deviationreport = [];
                $scope.totalgrid = [];
                
                var checkStatus = $scope.checkall;
                var count = 0;
                angular.forEach($scope.get_userEmplist, function (itm) {
                    itm.empck = checkStatus;
                    if (itm.empck == true) {
                        count += 1;
                    }
                    else {
                        count = 0;
                    }
                });
                angular.forEach($scope.get_userEmplist, function (em) {
                    if (em.empck === true) {
                        $scope.totalgrid.push(em);
                    }
                });

             if ($scope.checkall == true) {
                    angular.forEach($scope.get_userEmplist, function (em) {
                        em.empck === true;
                    });

                }
                else {
                    angular.forEach($scope.get_userEmplist, function (em) {
                        em.empck === false;
                    });

                }
        };
        
        $scope.togchkbx = function (groupType) {

            $scope.totalgrid = [];
            $scope.checkall = $scope.get_userEmplist.every(function (options) {
                return options.empck;
            });
            angular.forEach($scope.get_userEmplist, function (em) {
                if (em.empck === true) {
                    $scope.totalgrid.push(em);

                }
              
            });

           

        };


        
        //=============================for grid check box selection
        $scope.togglecheckboxAllDept = function (checkallemp) {
            var toggleStatus = checkallemp;
            angular.forEach($scope.DeptdeviationRemarksReport, function (slc) {
                slc.checked = toggleStatus;
            });
            $scope.DeptselectionHeaddetails(checkallemp);
        };

        $scope.togglecheckboxAll = function (checkallemp) {
            var toggleStatus = checkallemp;
            angular.forEach($scope.employeeid, function (slc) {
                slc.checked = toggleStatus;
            });
        };
        $scope.optionToggled = function () {
            $scope.checkallemp = $scope.employeeid.every(function (options) {
                return options.checked;
            });
        };
        $scope.optionToggleddept = function (user) {
            $scope.checkalldept = $scope.DeptdeviationRemarksReport.every(function (options) {
                return options.checked;
            });
            $scope.DeptselectionHeaddetails(user);
        };
        //*************************** Department REPORT *********************//        
        $scope.all_checkdept = function (departmentselectedAll) {
            var toggleStatus = departmentselectedAll;
            angular.forEach($scope.filterValue5, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.searchValueDesg = '';            
            $scope.designationselectedAll = true;
            $scope.Deptselectiondetails();
        
            

        };
        $scope.togchkbxdept = function (groupType) {
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {
                return itm.selected;
            });
            $scope.searchValueDesg == '';
            $scope.designationselectedAll = false;
            $scope.Deptselectiondetails();
     
            $scope.searchValueUEM = '';         
            $scope.checkall = false;
          
        };
        //*************************** Designation REPORT *********************//        
        
       
        $scope.all_checkdesg = function (designationselectedAll) {
            var checkStatus = designationselectedAll;
                var count = 0;
            angular.forEach($scope.designationdropdown, function (itm) {
                    itm.selected = checkStatus;
                    if (itm.selected == true) {
                        count += 1;
                    }
                    else {
                        count = 0;
                }
                $scope.searchValueUEM = '';
                $scope.checkall = false;
                $scope.Desgselectiondetails();
                });
            }

        

        $scope.togchkbxdesg = function (groupType) {

            $scope.designationselectedAll = $scope.designationdropdown.every(function (options) {
                    return options.selected;
            });
            $scope.searchValueUEM = '';
            $scope.checkall = false;
            $scope.Desgselectiondetails();
            }





        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };

       
     

        $scope.isOptionsRequired = function () {
            return !$scope.get_userEmplist.some(function (options) {
                return options.empck;
            });
        };

        $scope.searchValueDesg = '';
        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.HRMDES_DesignationName).indexOf(angular.lowercase($scope.searchValueDesg)) >= 0 || angular.lowercase(obj.MI_Name).indexOf(angular.lowercase($scope.searchValueDesg)) >= 0;
        };

        $scope.searchValueDept = '';
        $scope.filterValue4 = function (obj) {
            return angular.lowercase(obj.HRMDC_Name).indexOf(angular.lowercase($scope.searchValueDept)) >= 0;
        };

        $scope.searchValueUEM = '';
        $scope.filterValue2 = function (obj) {
            return angular.lowercase(obj.userEmpName).indexOf(angular.lowercase($scope.searchValueUEM)) >= 0 || angular.lowercase(obj.HRMD_DepartmentName).indexOf(angular.lowercase($scope.searchValueUEM)) >= 0;
        };

       


        //*************************** Deptselectiondetails REPORT *********************//
        $scope.Deptselectiondetails = function () {
            $scope.arrayuserEmp = [];
            $scope.designationdropdown = [];
            $scope.get_userEmplist = [];
            angular.forEach($scope.departmentdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserEmp.push(em);
                }
            });
            var data = {
                "departmentlist": $scope.arrayuserEmp
            };
            apiService.create("ISM_PlannerDeptDeviationReport/Deptselectiondetails", data).then(function (promise) {
                if (promise.designation != null && promise.designation.length > 0) {
                    $scope.designationdropdown = promise.designation;
                    if ($scope.designationdropdown.length > 0) {
                        angular.forEach($scope.designationdropdown, function (uem) {
                            uem.selected = true;
                        });
                        $scope.designationselectedAll = true;
                    }
                    $scope.Desgselectiondetails();
                }
                else {
                    $scope.searchValueUEM = '';
                    $scope.employeeid = [];
                    $scope.checkall = false;
                    swal("No Record  Found..... !!");
                }
            });
        };
        //*************************** Desgselectiondetails REPORT *********************//
        $scope.Desgselectiondetails = function () {
            $scope.arrayuserEmp = [];
            $scope.get_userEmplist = [];
            angular.forEach($scope.designationdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserEmp.push(em);
                }
            });
            var data = {
                "designationlist": $scope.arrayuserEmp
            };
            apiService.create("ISM_PlannerDeptDeviationReport/Desgselectiondetails", data).then(function (promise) {
                if (promise.get_userEmplist.length > 0) {
                    $scope.get_userEmplist = promise.get_userEmplist;
                    $scope.totalgrid = promise.get_userEmplist;
                    if ($scope.get_userEmplist.length > 0) {
                        angular.forEach($scope.get_userEmplist, function (uem) {
                            uem.empck = true;
                        });
                        $scope.checkall = true;
                    }
                }
                else {
                    $scope.searchValueUEM = '';
                    $scope.employeeid = [];
                    $scope.checkall = false;
                    swal("No Record  Found..... !!");
                }
            });
        };

        $scope.savedata = function () {
            $scope.submitted = true;
            $scope.userfeedback = [];
            angular.forEach($scope.totalgrid, function (em) {
                
                $scope.userfeedback.push({
                    HRME_Id: em.HRME_Id,
                    HRGFE_FeedBack: em.hrgfE_FeedBack
                });
                
            });

            if ($scope.myForm.$valid) {
                var data = {
                    "HRGFE_FeedbackDate": $scope.hrgfE_FeedbackDate,
                    //"HRGFE_FeedBack": $scope.hrgfE_FeedBack,
                    "HRGFE_Id": $scope.hrgfE_Id,
                    //"HRME_Id": $scope.hrmE_Id,
                    //"userEmpName": $scope.userEmpName,
                    "totalgrid": $scope.userfeedback
                       

                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("EmployeeHeadFeedBack/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.hrmE_Id == 0 || promise.hrmE_Id < 0) {
                            swal('Record saved successfully');
                        }
                        
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.hrmE_Id == 0 || promise.hrmE_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                       
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        };








        $scope.Deletedata = function (del_id) {
            swal({
                title: "Are you sure?",
                text: "Do you want to Delete This Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: true,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {


                        apiService.getURI("EmployeeHeadFeedBack/delete_data/", del_id.HRGFE_Id).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record Deleted Successfully");
                                    $state.reload();
                                }
                                else {
                                    swal("Failed to Delete, please contact administrator.");
                                }

                            });
                    }
                    else {
                        swal("Cancelled");
                    }
                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.Clear = function () {
            
            $state.reload();
        };

    
    }
})();

