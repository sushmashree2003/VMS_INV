(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_Master_TaskGroupController', ISM_Master_TaskGroupController)

    ISM_Master_TaskGroupController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function ISM_Master_TaskGroupController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        // ============================ng_click function for module (ALL)========================//
        $scope.searchchkbx = "";
        $scope.all_checkC = function () {
            var checkStatus = $scope.usercheckC;
            var count = 0;
            angular.forEach($scope.department_list, function (itm) {
                itm.selected = checkStatus;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }
        //====================================clear data====================================//
        $scope.submitted = false;
        $scope.Clearid = function () {           
            $state.reload();
        };
        $scope.isOptionsRequired = function () {
            return !$scope.department_list.some(function (options) {
                return options.selected;
            });
        }
        $scope.itemsPerPage = 5;
        $scope.currentPage = 1;
        $scope.interacted = function (field) {
        return $scope.submitted;
        };
        //==================load data=========================//
        $scope.submitted = false;
        $scope.loaddata = function () {           
            var pageid = 2;
            apiService.getURI("ISM_Master_TaskGroup/loaddata", pageid).then(function (promise) {
          
                if (promise.msg !== "" && promise.msg !== null && promise.msg !== undefined) {
                    swal(promise.msg);
                }
                else {                    
                    $scope.department_list = promise.department_list;
                    $scope.alldata = promise.alldata;
                }
            });
        }
        $scope.viewdept = function (obj) {
            $scope.uploaddepartments = [];
            $scope.uploaddeptdetails = [];
            $scope.objdata = obj;
            apiService.create("ISM_Master_TaskGroup/viewuploaddept", obj).then(function (promise) {
                if (promise !== null) {
                    if (promise.viewuploaddept.length > 0) {
                        $scope.uploaddeptdetails = promise.viewuploaddept;
                    }
                    else {
                        swal('Data Not Available!');
                    }

                }
            });
        };
        //================================================save===================//
        $scope.savedata = function () {            
            if ($scope.myForm.$valid) {
                var departmentsss = [];
                angular.forEach($scope.department_list, function (rr) {
                    if (rr.selected == true) {
                        departmentsss.push(rr)
                    }
                })
                var data = {
                    "ISMMTGRP_Id": $scope.ismmtgrP_Id,
                    "ISMMTGRP_TaskGroupName": $scope.ISMMTGRP_TaskGroupName,
                    "ISMMTGRP_TGRemarks": $scope.ISMMTGRP_TGRemarks,
                    "deptid": departmentsss,
                }
                apiService.create("ISM_Master_TaskGroup/savedata", data).then(function (promise) {                 
                    if (promise.msg !== "") {
                        if (promise.duplicate == true) {
                            swal("Data Already Exist!");
                        }
                        else if (promise.msg == 'saved') {
                            swal("Data Saved Successfully");
                        }
                        else if (promise.msg == 'Failed') {
                            swal("Data not Saved Successfully");
                        }
                        else if (promise.msg == 'updated') {
                            swal("Data Updated Successfully ");
                        }
                        $state.reload();
                    }
                    else {
                        swal("You Dont Have Permission To Save Data");
                        $state.reload();
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }
        //=====================================================edit data================================//
        $scope.Editdata = function (user) {
            var data = {
                "ISMMTGRP_Id": user.ismmtgrP_Id
            }
            apiService.create("ISM_Master_TaskGroup/Editdata", data).then(function (promise) {

                if (promise.editlist.length > 0) {
                    $scope.ismmtgrP_Id = promise.editlist[0].ismmtgrP_Id;
                    $scope.ISMMTGRP_TaskGroupName = promise.editlist[0].ismmtgrP_TaskGroupName;
                    $scope.ISMMTGRP_TGRemarks = promise.editlist[0].ismmtgrP_TGRemarks;
                    angular.forEach($scope.department_list, function (yy) {
                        angular.forEach(promise.editlist, function (uu) {
                            if (yy.hrmD_Id == uu.hrmD_Id) {
                                yy.selected = true;
                            }
                        })
                    })
                }
            })
        }
        //==========================================deactivate=====================================//
        $scope.clientDecative = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.ismmtgrP_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (usersem.ismmtgrP_ActiveFlag == false) {
                dystring = "Activate"
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
                        apiService.create("ISM_Master_TaskGroup/clientDecative", usersem).
                            then(function (promise) {

                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                                else {
                                    swal("Record Not " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }
        //============================deptactivate
        $scope.DeptDeactive = function (usersem, SweetAlert) {
            debugger;
            var dystring = "";

            if (usersem.ismmtgrpD_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (usersem.ismmtgrpD_ActiveFlag == false) {
                dystring = "Activate"
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
                        apiService.create("ISM_Master_TaskGroup/DeptDeactive", usersem).
                            then(function (promise) {
                                debugger;
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                                else {
                                    swal("Record Not " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }
    }
})();
//======================================================================================================//
