(function () {
    'use strict';
    angular
        .module('app')
        .controller('LeaveRestrictionController', LeaveRestrictionController)
    LeaveRestrictionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function LeaveRestrictionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

        $scope.obj = {};
        $scope.institute = false;
        $scope.onLoadGetData = function () {
            var pageid = 2;     
            apiService.getURI("OnlineLeaveApplication/LRdetails", pageid).then(function (promise) {     
                $scope.leaverestriclist = promise.leaverestriclist; 
                $scope.institutionarray = promise.institutionarray;               
                $scope.employeelist = promise.employeelist;  
            })
        };
        $scope.submitted = false;
        $scope.all_check1 = function () {
            var checkStatus = $scope.hrmD_Id;
            var count = 0;
            angular.forEach($scope.departmentdropdown, function (itm) {
                itm.selected = checkStatus;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }     
        $scope.togchkbx1 = function () {

            $scope.hrmD_Id = $scope.departmentdropdown.every(function (options) {
                return options.selected;
            });
        }



        $scope.all_check1emp = function () {
            var checkStatus = $scope.hrmE_Id;
            var count = 0;
            angular.forEach($scope.employeelist, function (itm) {
                itm.selected = checkStatus;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }
        $scope.togchkbx1emp = function () {

            $scope.hrmE_Id = $scope.employeelist.every(function (options) {
                return options.selected;
            });
        }



        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //Onchange Institution, Departmnet Should Load

        $scope.getdepartment = function (companyid) {
            var data = {
                "MI_Id": companyid.mI_Id,
            };
            apiService.create("OnlineLeaveApplication/getdepartment", data).
                then(function (promise) {
                    if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                        $scope.departmentdropdown = promise.departmentdropdown;
                    }
                    else {
                        swal(" No Record Found !");
                    }
                });
        };

        $scope.SaveData = function () {
            $scope.submitted = true;
            var mI_Id = 0;
            $scope.selected_Dept = [];
            if ($scope.allind == "empwise") {
                angular.forEach($scope.employeelist, function (mm) {
                    if (mm.selected === true) {
                        $scope.selected_Dept.push({ HRME_Id: mm.hrmE_Id });
                    }
                })
            }
            else {
                mI_Id = $scope.mI_Id.mI_Id

                angular.forEach($scope.departmentdropdown, function (mm) {
                    if (mm.selected === true) {
                        $scope.selected_Dept.push({ HRMD_Id: mm.hrmD_Id });
                    }
                })
            }
          
           
            if ($scope.myForm.$valid) {
                var data = {
                    "HRLRD_Id": $scope.HRLRD_Id,
                    "MI_Id": mI_Id,
                    "HRLRD_FromDate": new Date($scope.HRLRD_FromDate).toDateString(),
                    "HRLRD_ToDate": new Date($scope.HRLRD_ToDate).toDateString(),
                    "HRLRD_Remarks": $scope.HRLRD_Remarks,
                    selected_Dept: $scope.selected_Dept,
                    
                }
                apiService.create("OnlineLeaveApplication/LRSaveUpdate", data).then(function (promise) {                   
                    
                    if (promise.retrunMsg !== "") {

                        if (promise.retrunMsg == "Duplicate") {
                            swal("Record already exist..!!");
                            return;
                        }

                        else if (promise.retrunMsg == "false") {
                            swal("Record Not saved / Updated..", 'Fail');

                        }
                        else if (promise.retrunMsg == "Add") {
                            swal("Record Saved Successfully..");
                            $state.reload();
                        }
                        else if (promise.retrunMsg == "Update") {
                            swal("Record Updated Successfully..");
                            $state.reload();
                        }
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                            return;
                        }
                        $scope.cancel();
                    }
                })

            }
        };
        $scope.Edit = function (item) {
            $scope.institute = true;
            $scope.departmentdropdown = [];
            var data = {
                "HRLRD_Id": item.hrlrD_Id,       
                "MI_Id": item.mI_Id,
                "HRMD_Id": item.hrmD_Id
            };
            apiService.create("OnlineLeaveApplication/LREdit", data).then(function (promise) {           
               
                if (promise !== null) {
                    $scope.lrlist = promise.lrlist; 
                    $scope.newcaste = {};
                    for (var i = 0; i < $scope.institutionarray.length; i++) {
                        name = $scope.institutionarray[i].mI_Id;
                        if (name == promise.lrlist[0].mI_Id) {
                            $scope.institutionarray[i].Selected = true
                            $scope.mI_Id = $scope.institutionarray[i];
                            $scope.newcaste = $scope.lrlist[0].mI_Id;
                        }
                        else {
                            $scope.institutionarray[i].Selected = false;
                        }
                    }   
                       $scope.departmentdropdown = promise.departmentdropdown;  
                    angular.forEach($scope.departmentdropdown, function (dep) {
                        if (dep.hrmD_Id == promise.lrlist[0].hrmD_Id) {
                            dep.selected = true;
                        }
                    });
                    
                    $scope.HRLRD_Id = promise.lrlist[0].hrlrD_Id;                 
                    $scope.HRLRD_Remarks = promise.lrlist[0].hrlrD_Remarks;
                    $scope.HRLRD_ToDate = new  Date(promise.lrlist[0].hrlrD_ToDate); 
                    $scope.HRLRD_FromDate = new Date(promise.lrlist[0].hrlrD_FromDate);                    

                 
                   
                }
            })
        }
        // clear form data
        $scope.cancel = function () {
            $scope.search = "";
            $state.reload();
        }; 
        $scope.DeletRecord = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var data = {
                "HRLRD_Id": user.hrlrD_Id,
            };
            if (data.hrlrD_ActiveFlg == false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Record..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("OnlineLeaveApplication/ActiveDeactiveRecord", data).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Activated") {
                                        swal("Record Activated successfully");
                                        $state.reload();
                                    }
                                    else if (promise.retrunMsg === "Deactivated") {
                                        swal("Record Deactivated successfully");
                                        $state.reload();
                                    }
                                    else {
                                        swal("Record Not Activated/Deactivated", 'Fail');
                                    }                                    
                                }

                            })
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                });
        }      
    }
})();