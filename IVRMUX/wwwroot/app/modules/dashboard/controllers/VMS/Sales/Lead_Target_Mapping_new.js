(function () {
    'use strict';
    angular.module('app')
        .controller('Lead_Target_Mapping_newController', Lead_Target_Mapping_newController)
    Lead_Target_Mapping_newController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
    function Lead_Target_Mapping_newController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {
        $scope.userPrivileges = "";
        $scope.taskid5 = [];
        $scope.empdoj = [];
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.searchValueDeptd = "";
        $scope.searchValueDesgd = "";
        $scope.searchValueUEMd = "";
        $scope.sortKey = "regno";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa
        $scope.presentCountgrid = 0;
        $scope.searchValue = "";
        $scope.tadprint = false;
        $scope.nameschedule = true;
        $scope.obj = {};
        $scope.printdatatable = [];
        $scope.loaddata = function () {
            $scope.screport = false;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            var pageid = 2;
            apiService.getURI("Lead_Target_Mappingnew/OnloadGetDetails", pageid).
                then(function (promise) {
                    $scope.leadlist = promise.leadlist;
                    //$scope.imgname = logo;

                    if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                        $scope.departmentdropdown = promise.departmentdropdown;
                      
                    }

                    if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                        $scope.designationdropdown = promise.designationdropdown;
                       
                    }

                    if (promise.targetMappinglists.length > 0) {
                        $scope.targetMappinglists = promise.targetMappinglists;
                      //  $scope.taskid5 = [];
                        angular.forEach($scope.targetMappinglists, function (dev) {
                            if ($scope.taskid5.length === 0) {
                                $scope.taskid5.push({
                                    HRME_Id: dev.HRME_Id,
                                    HRME_EmployeeFirstName: dev.HRME_EmployeeFirstName,
                                    ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName,
                                    ISMSLETARM_Periodicity: dev.ISMSLETARM_Periodicity

                                });
                            }
                            else if ($scope.taskid5.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.taskid5, function (emp) {
                                    if (emp.HRME_Id === dev.HRME_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.taskid5.push({
                                        HRME_Id: dev.HRME_Id,
                                        HRME_EmployeeFirstName: dev.HRME_EmployeeFirstName,
                                        ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName,
                                        ISMSLETARM_Periodicity: dev.ISMSLETARM_Periodicity
                                    });
                                }
                            }
                        });
                        angular.forEach($scope.taskid5, function (ddd) {
                            $scope.templist = [];
                            angular.forEach($scope.targetMappinglists, function (dd) {
                                if (dd.HRME_Id === ddd.HRME_Id) {
                                    $scope.templist.push(dd);
                                }
                            });
                            ddd.targetMappinglists = $scope.templist;

                        });

                    }
                    else {
                      //  swal('No Data Found!!!');
                        $scope.targetMappinglists = "";
                    }





                })
        };

        //By Department Single
        $scope.GetEmployeeByDepartment = function (department) {
            $scope.designationdropdown = [];
            $scope.designationselectedAll = false;

            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {

                return itm.selected;
            });
            $scope.get_desig();
        };

        $scope.GetEmployeeByDesignation = function (designation) {

            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {
                return itm.selected;
            });
            $scope.get_employee();
        };
        $scope.get_employee = function () {
            $scope.employeedropdown = [];
            var ids = [];
            angular.forEach($scope.designationdropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.hrmdeS_Id);
                }
            });
            var ids1 = [];
            angular.forEach($scope.departmentdropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids1.push(grp_t.hrmD_Id);
                }
            });
            var data = {
                hrmdes_IdList: ids,
                hrmD_IdList: ids1
            };
            apiService.create("Lead_Target_Mappingnew/get_employee", data).
                then(function (promise) {
                    if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                        $scope.employeedropdown = promise.employeedropdown;
                        //$scope.employeedropdownselectedAll = true;
                        // $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                    }
                });
        };


        $scope.get_desig = function () {
      
            var ids1 = [];
            angular.forEach($scope.departmentdropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids1.push(grp_t.hrmD_Id);
                }
            });
            var data = {
                hrmD_IdList: ids1,

            };
            apiService.create("Lead_Target_Mappingnew/get_desig", data).
                then(function (promise) {
                    if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                        $scope.designationdropdown = promise.designationdropdown;
                     
                    }
                });
        };

        //By Designation
        $scope.GetEmployeeByDesignationAll = function (designationselectedAll) {

            var toggleStatus = $scope.designationselectedAll;
            angular.forEach($scope.designationdropdown, function (itm) {
                itm.selected = toggleStatus;
            });

            $scope.get_employee();
        };

        $scope.GetEmployeeBygroupTypeAll = function (EmployeeselectedAll) {

            var toggleStatus = $scope.employeedropdownselectedAll;
            angular.forEach($scope.employeedropdown, function (itm) {
                itm.selected = toggleStatus;
            });
        };
      

        $scope.GetEmployeedropdown = function (designation) {
            $scope.employeedropdownselectedAll = $scope.employeedropdown.every(function (itm) {
                return itm.selected;
            });

        };
        $scope.GetEmployeeByDepartmentAll = function (departmentselectedAll) {
            var toggleStatus = $scope.departmentselectedAll;
            angular.forEach($scope.departmentdropdown, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_desig();
        };

        $scope.getdetails = function () {

            if ($scope.myForm.$valid) {
                $scope.emplistarray = [];
                angular.forEach($scope.employeedropdown, function (aa) {
                    if (aa.selected == true) {
                        $scope.emplistarray.push({ hrmE_Id: aa.hrmE_Id })
                    }
                });
                var data = {
                    "Emptwolist": $scope.emplistarray,
                };
                apiService.create("Lead_Target_Mappingnew/getdetails", data).
                    then(function (promise) {
                        $scope.emp_list = promise.emp_list;
                        $scope.status_dd = promise.status_dd;
                        if (promise.emp_list == null || promise.emp_list.length > 0) {
                            $scope.IsHiddendown = true;
                        } else {
                            swal("No Records Found");
                            $scope.IsHiddendown = false;

                        }

                    });


            }


        };

        $scope.getcatdetails = function (user) {
            $scope.savedfiles=[];
            $scope.materaldocuupload = [];
            $scope.materaldocuupload = [{ itrS_Id: 'trans1' }];
            $scope.emp_list_cat = [];
            if ($scope.myForm.$valid) {
                $scope.hrmE_Id = user.hrmE_Id;
                var data = {
                    "HRME_Id": user.hrmE_Id,
                };
                apiService.create("Lead_Target_Mappingnew/getcatdetails", data).
                    then(function (promise) {
                       

                        if (promise.emp_list_cat > 0 || promise.emp_list_cat !== null) {
                            $scope.emp_list_cat = promise.emp_list_cat;
                            //$scope.empdoj = promise.emp_list_cat[0].HRME_DOJ.toString();
                            $scope.empdoj = new Date(promise.emp_list_cat[0].HRME_DOJ);
                            $scope.empdoj.setDate($scope.empdoj.getDate());

                            $scope.currentdate = new Date();
                            $scope.currentdate.setHours(0, 0, 0, 0);
                            $('#myModal1').modal('show');
                        }
                        else {
                            swal('No Data Found!!!');

                        }

                    });
            }

        };

        $scope.addgrnrows = function () {
            if ($scope.materaldocuupload.length > 1) {
                for (var i = 0; i === $scope.materaldocuupload.length; i++) {
                    var id = $scope.materaldocuupload[i].itrS_Id;
                    var lastChar = id.substr(id.length - 1);
                    $scope.cnt = parseInt(lastChar);
                }
            }
            $scope.cnt = $scope.cnt + 1;
            $scope.tet = 'trans' + $scope.cnt;
            var newItemNo = $scope.cnt;
            $scope.materaldocuupload.push({ 'itrS_Id': 'trans' + newItemNo });


        };
        $scope.removegrnrows = function (index, data) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

        };
        $scope.materaldocuupload = [{ itrS_Id: 'trans1' }];
        if ($scope.materaldocuupload.length === 1) {
            $scope.cnt = 1;
        }

        $scope.savedfiles = [];
        $scope.AddFiles = function () {
            $scope.savedfiles = [];
            angular.forEach($scope.materaldocuupload, function (dd) {

                if (dd.ISMMTCAT_Id > 0 && dd.ismsletarM_Numbers != "" && dd.ismsletarM_LeadPeriodicity != "" && dd.ismsletarM_LeadPeriodicity != undefined && $scope.TargetCreatedDate != null && $scope.TargetCreatedDate != undefined && $scope.TargetCreatedDate != "") {
                    var ISMMTCAT_TaskCategoryName = "";
                    angular.forEach($scope.emp_list_cat, function (ddd) {
                        if (ddd.ISMMTCAT_Id == dd.ISMMTCAT_Id) {
                            ISMMTCAT_TaskCategoryName = ddd.ISMMTCAT_TaskCategoryName;
                        }
                    });

                    $scope.savedfiles.push({
                        ISMMTCAT_Id: dd.ISMMTCAT_Id,
                        ISMMTCAT_TaskCategoryName: ISMMTCAT_TaskCategoryName,
                        ISMSLETARM_Numbers: dd.ismsletarM_Numbers,
                        ISMSLETARM_Periodicity: dd.ismsletarM_LeadPeriodicity,
                    })
                }
                else {
                    swal('Please Enter All the Data To Proceed...');
                }
            });
        };
        $scope.hrmE_Id = 0;
        $scope.selectedcat = [];
        $scope.SaveCheckListDeails = function () {
            $('#myModal1').modal('hide');
            angular.forEach($scope.emp_list, function (ddd) {
                if (ddd.hrmE_Id == $scope.hrmE_Id) {
                    ddd.selectedcat = $scope.savedfiles;

                }
            });
        }

        // ****** View Added Checklist Details ***** //
        $scope.ViewAddChecklistDetails = function (ddd) {

            $scope.addedchecklist = ddd.selectedcat;

            if ($scope.addedchecklist !== null && $scope.addedchecklist.length > 0) {
                $('#viewaddedchecklist').modal('show');
            }
        };
        



        $scope.SaveTargetMapping = function () {
            $scope.emplistarray = [];
           
            angular.forEach($scope.emp_list, function (aa) {
                if (aa.selected == true && aa.selectedcat.length > 0) {

                    $scope.emplistarray.push({
                        hrmE_Id: aa.hrmE_Id,
                        selectedcat: aa.selectedcat
                    })
                }
            });
            if ($scope.emplistarray.length > 0 && $scope.TargetCreatedDate != null && $scope.TargetCreatedDate != undefined && $scope.TargetCreatedDate != "") {
                var creDate = new Date($scope.TargetCreatedDate).toDateString();
                var data = {
                    "Emplistnew": $scope.emplistarray,
                    "TargetCreatedDate": creDate

                    //  "Catlist": aa.selectedcat
                };
                apiService.create("Lead_Target_Mappingnew/SaveTarget", data).
                    then(function (promise) {

                        if (promise.retrunMsg == 'save') {
                            swal("Record Save Successfully!");
                            $state.reload();
                        }
                        else if (promise.retrunMsg == "Not save") {
                            swal(" Record Not Save Successfully");
                        }
                        else {
                            swal("Sorry...something went wrong");
                        }

                    });
            }
            else {
                swal('Please Enter All the Data To Proceed...');
            }
            
        }

        $scope.submitted = false;
        $scope.Update = function () {
            $scope.leadverify = [];
            if ($scope.myForm.$valid) {
                angular.forEach($scope.taskid5, function (item) {
                    angular.forEach(item.targetMappinglists, function (aa) {
                        if (aa.selected === true) {
                            $scope.leadverify.push({
                                ISMSLETARM_Numbers: aa.ISMSLETARM_Numbers,
                                HRME_Id: aa.HRME_Id,
                                ISMMTCAT_Id: aa.ISMMTCAT_Id,
                                ISMSLETARM_CreatedDate: aa.ISMSLETARM_CreatedDate,
                                ISMSLETARM_Id:aa.ISMSLETARM_Id
                            })
                        }
                    });
                });
                if ($scope.leadverify != null && $scope.leadverify.length > 0) {
                    var data = {
                        "tagtedit": $scope.leadverify
                    }
                    apiService.create("Lead_Target_Mappingnew/Edit", data).
                        then(function (promise) {
                            if (promise.retrunMsg == "update") {
                                swal("Record Update Successfully!");
                                $state.reload();
                            }
                            else if (promise.retrunMsg == "Not save") {
                                swal(" Please Select Verify Check box");
                            }
                            else {
                                swal("Sorry...something went wrong");
                            }

                        })
                }
                else {
                    swal("Please Select  Atleast one CheckBox");
                }

            }
            else {
                $scope.submitted = true;
            }

        }



        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.taskid5, function (itm) {
                angular.forEach(itm.targetMappinglists, function (aa) {
                    aa.selected = toggleStatus;
                })
            });
        }

        $scope.optionToggled = function () {

            $scope.all = $scope.taskid5.every(function (options) {

                return options.selected;
            });
        }

        $scope.cleardata = function () {
            $state.reload();
        };
        $scope.cleardata1 = function () {
            $state.reload();
        };
        $scope.cleardata = function () {
            $state.reload();
        };
    }
})();