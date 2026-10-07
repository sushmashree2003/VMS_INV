(function () {
    'use strict';
    angular.module('app').controller('ISM_CommonTaskController', ISM_CommonTaskController)

    ISM_CommonTaskController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function ISM_CommonTaskController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.Type = "weekly";
        $scope.changeradiobutton = true;
        $scope.dateflag = false;
        $scope.Loaddata = function () {
            $scope.currentPageU = 1;
            $scope.itemsPerPageU = 10;
            $scope.search = "";

            var pageid = 2;
            apiService.getURI("ISM_CommonTask/Loaddata", pageid).then(function (promise) {
                $scope.daylist = promise.daylist;
                $scope.venuelsit = promise.venuelsit;
                $scope.mettingroomlist = promise.mettingroomlist;
                $scope.departmentlist = promise.departmentlist;
            });
        };

        $scope.submitted = false;    
        $scope.savedata = function () {
            if ($scope.frmmodule.$valid) {
                var data = {
                    "HRMD_Id": $scope.HRMD_Id,
                    "MI_Id": $scope.MI_Id,
                    "temp_deptremarks": $scope.tempadd
                };

                apiService.create("EmailDepartmentMapping/saveremarksdata", data).then(function (promise) {
                    if (promise !== null) {

                        if (promise.message === "Save") {
                            if (promise.returnval === true) {
                                swal("Record Saved /Updated  Successfully");
                            } else {
                                swal("Failed To Save /Update Record");
                            }
                        } else if (promise.message === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else {
                            swal("Failed To Save / Update Record");
                        }
                        $state.reload();
                    }

                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.Editdata = function (user) {
            var data = {
                "HRMD_Id": user.hrmD_Id,
                "MI_Id": user.mI_Id,
                "ISMDR_Id": user.ismdR_Id
            };
            apiService.create("EmailDepartmentMapping/Editremarksdata", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.geteditremarksdata !== null && promise.geteditremarksdata.length > 0) {

                        $scope.geteditremarksdatanew = promise.geteditremarksdata;
                        $scope.deptartmentlist = promise.getdepartment;

                        $scope.MI_Id = $scope.geteditremarksdatanew[0].mI_Id;
                        $scope.HRMD_Id = $scope.geteditremarksdatanew[0].hrmD_Id;
                        $scope.remarks = $scope.geteditremarksdatanew[0].ismdR_Remarks;
                        $scope.ISMDR_Id = $scope.geteditremarksdatanew[0].ismdR_Id;
                    }
                }
            });
        };

        $scope.deactive = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";

            if (deactiveRecord.ismdR_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "Record Deactivated ";
            } else {
                mgs = "Activate";
                confirmmgs = "Record Activated ";
            }

            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("EmailDepartmentMapping/deactivate", deactiveRecord).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal(confirmmgs + ' Successfully');
                                }
                                else {
                                    swal('Record Not  Activated/Deactivated');
                                }
                            });
                        $state.reload();
                    }
                    else {
                        swal("Record" + mgs + "Cancelled");
                    }
                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.change_dept = function () {
            $scope.selectedDept = [];

            $scope.usercheckdesg = "";
            $scope.designationlist = [];

            $scope.usercheckemp = "";
            $scope.employeelist = [];

            $scope.togchkbxDept();

            angular.forEach($scope.departmentlist, function (dpt) {
                if (dpt.selectdept) {
                    $scope.selectedDept.push({ HRMD_Id: dpt.hrmD_Id });
                }
            });
            var data = {
                //"HRMD_Id": $scope.HRMD_Id,
                "selectedDept": $scope.selectedDept
            };
            apiService.create("ISM_CommonTask/get_Designation/", data).
                then(function (promise) {
                    if (promise.designationlist.length > 0) {
                        $scope.designationlist = promise.designationlist;
                    }
                    else {
                        swal('Record Not Found!');
                    }
                });
        };

        $scope.change_desig = function () {

            $scope.usercheckemp = "";
            $scope.employeelist = [];
            $scope.selectedDept = [];
            $scope.selectedDesg = [];

            angular.forEach($scope.departmentlist, function (dpt) {
                if (dpt.selectdept) {
                    $scope.selectedDept.push({ HRMD_Id: dpt.hrmD_Id });
                }
            });

            angular.forEach($scope.designationlist, function (desg) {
                if (desg.selectdesg) {
                    $scope.selectedDesg.push({ HRMDES_Id: desg.hrmdeS_Id });
                }
            });

            $scope.togchkDes();

            var data = {
                //"HRMD_Id": $scope.HRMD_Id,
                //"HRMDES_Id": $scope.HRMDES_Id,
                "selectedDept": $scope.selectedDept,
                "selectedDesg": $scope.selectedDesg
            };

            apiService.create("ISM_CommonTask/get_Employee", data).
                then(function (promise) {
                    if (promise.employeelist.length > 0) {
                        $scope.employeelist = promise.employeelist;
                    }
                    else {
                        swal('Record Not Found!');
                    }
                });
        };

        $scope.togchkbxDept = function () {
            $scope.usercheckdept = $scope.departmentlist.every(function (options) {
                return options.selectdept;
            });
        };

        $scope.togchkDes = function () {
            $scope.usercheckdesg = $scope.designationlist.every(function (options) {
                return options.selectdesg;
            });
        };

        $scope.togchkbxemp = function () {
            $scope.usercheckemp = $scope.employeelist.every(function (options) {
                return options.selectemp;
            });
        };

        $scope.searchchkbxday = "";
        $scope.all_checkDay = function () {
            var allcheckday = $scope.usercheckday;
            angular.forEach($scope.daylist, function (all) {
                all.selectday = allcheckday;
            });
        };

        $scope.togchkbxDay = function () {
            $scope.usercheckday = $scope.daylist.every(function (single) {
                return single.selectday;
            });
        };

        $scope.isOptionsRequiredDay = function () {
            return !$scope.daylist.some(function (option) {
                return option.selectday;
            });
        };

        $scope.isOptionsRequireddept = function () {
            return !$scope.departmentlist.some(function (option) {
                return option.selectdept;
            });
        };

        $scope.isOptionsRequireddesg = function () {
            return !$scope.designationlist.some(function (option) {
                return option.selectdesg;
            });
        };

        $scope.isOptionsRequiredemp = function () {
            return !$scope.employeelist.some(function (option) {
                return option.selectemp;
            });
        };

        $scope.searchchkbxdept = "";
        $scope.all_checkdept = function () {
            var usercheckdept = $scope.usercheckdept;
            angular.forEach($scope.departmentlist, function (allchk) {
                allchk.selectdept = usercheckdept;
            });
            if ($scope.usercheckdept == false) {
                $scope.usercheckdesg = "";
                $scope.designationlist = [];

                $scope.usercheckemp = "";
                $scope.employeelist = [];
            }
            else if ($scope.usercheckdept == true) {
                $scope.change_dept();
            }
        };    

        $scope.searchchkbxdesg = "";
        $scope.usercheckdesg = "";
        $scope.all_checkdesg = function () {
            var allcheckDesg = $scope.usercheckdesg;
            angular.forEach($scope.designationlist, function (des) {
                des.selectdesg = allcheckDesg;
            });

            if ($scope.usercheckdesg == false) {
                $scope.usercheckemp = "";
                $scope.employeelist = [];
            }
            else if ($scope.usercheckdesg == true) {
                $scope.change_desig();
            }
        };

        $scope.searchchkbxemp = "";
        $scope.all_checkemp = function () {
            var allcheckboxEmp = $scope.usercheckemp;
            angular.forEach($scope.employeelist, function (allemp) {
                allemp.selectemp = allcheckboxEmp;
            });
        };

        $scope.get_radiobuttonvalue = function () {
            if ($scope.Type == "daily") {
                $scope.changeradiobutton = false;
                $scope.dateflag = false;
            }
            else if ($scope.Type == "weekly") {
                $scope.changeradiobutton = true;
                $scope.dateflag = false;
            }
            else {
                $scope.changeradiobutton = false;
                $scope.dateflag = true;
            }
        };
    }
})();

