(function () {
    'use strict';
    angular.module('app').controller('FMS_Master_PhysicalFileNameController', FMS_Master_PhysicalFileNameController);

    FMS_Master_PhysicalFileNameController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter'];
    function FMS_Master_PhysicalFileNameController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter) {

        $scope.addjob = {};
        $scope.EmployeeDis = false;
        $scope.edit1 = false;
        $scope.edit2 = false;
        $scope.edit3 = false;
        $scope.rowwnames = [];
        $scope.colsnames = [];

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage1 = paginationformasters;
        $scope.currentPage1 = 1;

        $scope.itemsPerPage21 = paginationformasters;
        $scope.currentPage21 = 1;

        $scope.itemsPerPage22 = paginationformasters;
        $scope.currentPage22 = 1;

        $scope.itemsPerPage23 = paginationformasters;
        $scope.currentPage23 = 1;

        $scope.itemsPerPage25 = paginationformasters;
        $scope.currentPage25 = 1;

        $scope.fmsmpfN_Id = 0;
        $scope.fmsmdmD_Id = 0;
        $scope.fmsmfcbT_Id = 0;
        $scope.fmsmfC_Id = 0;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.submitted = false;
        $scope.submitted2 = false;
        $scope.submitted3 = false;
        $scope.submitted4 = false;

        // Master Physical File Name
        $scope.loadPhysicalFiledata = function () {
            $scope.fmsmpfN_PhysicalFileName = "";
            $scope.fmsmpfN_FileContent = "";
            $scope.fmsmpfN_CreatedYear = "";
            $scope.fmsmpfN_FileNo = "";
            $scope.fmsmpfN_FileType = "";
            $scope.search21 = "";
            $scope.MI_Id = "";
            $scope.submitted = false;
            $scope.edit1 = false;
            $scope.gridviewDetails = [];
            $scope.getinstitutiondetails = [];
            $scope.getdepartment_physical = [];
            $scope.fmsmpfN_Id = 0;
            var pageid = 2;
            apiService.getURI("FMS_Master_PhysicalFileName/loadPhysicalFiledata", pageid).then(function (promise) {
                if (promise.gridviewDetails !== null && promise.gridviewDetails.length > 0) {
                    $scope.gridPhysicalFileName = promise.gridviewDetails;
                    $scope.gridviewDetails = promise.gridviewDetails;
                }
                $scope.getinstitutiondetails = promise.getinstitutiondetails;
                $scope.getdepartment_physical = promise.getdepartmentlist;
                angular.forEach($scope.getdepartment_physical, function (dd) {
                    dd.FMSDPFN_Id = 0;
                    dd.selected = false;
                });

                $scope.MI_Id = promise.mI_Id;
            });
        };

        $scope.savejob = function () {
            if ($scope.myForm.$valid) {

                $scope.deptlist = [];
                $scope.searchchkbx_physical = "";
                angular.forEach($scope.getdepartment_physical, function (dd) {
                    if (dd.selected) {
                        $scope.deptlist.push({ HRMD_Id: dd.hrmD_Id, FMSDPFN_Id: dd.FMSDPFN_Id });
                    }
                });


                var data = {
                    "FMSMPFN_Id": $scope.fmsmpfN_Id,
                    "FMSMPFN_PhysicalFileName": $scope.fmsmpfN_PhysicalFileName,
                    "FMSMPFN_FileContent": $scope.fmsmpfN_FileContent,
                    "FMSMPFN_CreatedYear": $scope.fmsmpfN_CreatedYear,
                    "FMSMPFN_FileNo": $scope.fmsmpfN_FileNo,
                    "FMSMPFN_FileType": $scope.fmsmpfN_FileType,
                    "MI_Id": $scope.MI_Id,
                    "deptlist_temp": $scope.deptlist
                };

                apiService.create("FMS_Master_PhysicalFileName/savedata", data).then(function (promise) {

                    if (promise.retrunMsg !== "") {
                        if (promise.retrunMsg === "Duplicate") {
                            swal("Record already exist..!!");
                            return;
                        }
                        else if (promise.retrunMsg === "false") {
                            swal("Record Not saved / Updated", 'Fail');
                        }
                        else if (promise.retrunMsg === "Add") {
                            swal("Record Saved Successfully..");
                            $scope.cancel();
                        }
                        else if (promise.retrunMsg === "Update") {
                            swal("Record Updated Successfully..");
                            $scope.cancel();
                        }
                        else if (promise.retrunMsg === "Notification") {
                            swal("Notification!");
                            $scope.cancel();
                        }
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                        }
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.EditData = function (record) {
            angular.forEach($scope.getdepartment_physical, function (dd) {
                dd.selected = false;
            });

            var data = {
                "FMSMPFN_Id": record.fmsmpfN_Id,
                "MI_Id": $scope.MI_Id
            };
            apiService.create("FMS_Master_PhysicalFileName/editRecord", data).then(function (promise) {
                if (promise.editDetails !== null && promise.editDetails.length > 0) {
                    $scope.edit1 = true;
                    $scope.fmsmpfN_Id = promise.editDetails[0].fmsmpfN_Id;
                    $scope.fmsmpfN_PhysicalFileName = promise.editDetails[0].fmsmpfN_PhysicalFileName;
                    $scope.fmsmpfN_FileContent = promise.editDetails[0].fmsmpfN_FileContent;
                    $scope.fmsmpfN_CreatedYear = promise.editDetails[0].fmsmpfN_CreatedYear;
                    $scope.fmsmpfN_FileNo = promise.editDetails[0].fmsmpfN_FileNo;
                    $scope.fmsmpfN_FileType = promise.editDetails[0].fmsmpfN_FileType;
                    //$scope.edit1 = true;


                    if (promise.physicaldetails !== null && promise.physicaldetails.length > 0) {
                        $scope.physicaldetails = promise.physicaldetails;
                        angular.forEach($scope.getdepartment_physical, function (dd) {
                            angular.forEach($scope.physicaldetails, function (d) {
                                if (dd.hrmD_Id === d.hrmD_Id) {
                                    dd.selected = true;
                                  
                                }
                            });
                        });
                    }


                }
            });
        };

        $scope.ActivateRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.fmsmpfN_ActiveFlg === false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }

            data.MI_Id = $scope.MI_Id;

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
                        apiService.create("FMS_Master_PhysicalFileName/ActiveDeactiveRecord", data).then(function (promise) {
                            if (promise.retrunMsg !== "") {
                                if (promise.retrunMsg === "Activated") {
                                    swal("Record Activated successfully");
                                }
                                else if (promise.retrunMsg === "Deactivated") {
                                    swal("Record Deactivated successfully");
                                }
                                else {
                                    swal("Record Not Activated/Deactivated", 'Fail');
                                }
                                $scope.cancel();
                            }

                        });
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }
            );
        };

        $scope.ViewPhysicalFileDeptList = function (record) {
            $scope.physicalfilename = record.fmsmpfN_PhysicalFileName;
            var data = {
                "FMSMPFN_Id": record.fmsmpfN_Id,
                "MI_Id": $scope.MI_Id
            };
            apiService.create("FMS_Master_PhysicalFileName/ViewPhysicalFileDeptList", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getsaveddeptDetails = promise.getsaveddeptDetails;
                    if ($scope.getsaveddeptDetails !== null && $scope.getsaveddeptDetails.length > 0) {
                        $('#mymodalviewdeptlistphysical').modal('show');
                    } else {
                        swal("No Records Found");
                    }
                }
            });
        };

        $scope.ActiveDeactiveDeptPhyscialfile = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.fmsdpfN_ActiveFlg === false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }

            data.MI_Id = $scope.MI_Id;

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
                        apiService.create("FMS_Master_PhysicalFileName/ActiveDeactiveDeptPhyscialfile", data).then(function (promise) {
                            if (promise !== null) {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs +" Successfully");
                                } else {
                                    swal("Failed To " + confirmmgs+" Record");
                                }

                                $scope.getsaveddeptDetails = promise.getsaveddeptDetails;
                            }
                        });
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }
            );
        };        

        $scope.cancel = function () {
            $scope.fmsmpfN_PhysicalFileName = "";
            $scope.fmsmpfN_FileContent = "";
            $scope.fmsmpfN_CreatedYear = "";
            $scope.fmsmpfN_FileNo = "";
            $scope.fmsmpfN_FileType = "";
            $scope.submitted = false;
            $scope.search21 = false;
            $scope.fmsmpfN_Id = 0;
            $scope.gridviewDetails = [];
            $scope.getdepartment_physical = [];
            $scope.gridPhysicalFileName = [];
            $scope.MI_Id = "";
            $scope.getinstitutiondetails = [];
            $scope.loadPhysicalFiledata();
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.getdepartment_physical.some(function (options) {
                return options.selected;
            });
        };

        $scope.searchchkbx_physical = "";
        $scope.filterchkbx_physical = function (obj) {
            return angular.lowercase(obj.hrmD_DepartmentName).indexOf(angular.lowercase($scope.searchchkbx_physical)) >= 0;
        };

        // Master File Category
        $scope.loadFileCategorydata = function () {
            var pageid = 2;
            $scope.fmsmfC_FileCategoryName = "";
            $scope.fmsmfC_FileCategoryCode = "";
            $scope.search = "";
            $scope.MI_Id_Category = "";
            $scope.getdepartment = [];
            $scope.filecategoryDetails = [];
            $scope.getinstitutiondetails = [];
            $scope.submitted2 = false;
            $scope.edit2 = false;
            apiService.getURI("FMS_Master_PhysicalFileName/loadFileCategorydata", pageid).then(function (promise) {
                if (promise.filecategoryDetails !== null && promise.filecategoryDetails.length > 0) {
                    $scope.filecategoryDetails = promise.filecategoryDetails;
                }
                $scope.getdepartment = promise.getdepartmentlist;

                $scope.getinstitutiondetails = promise.getinstitutiondetails;
                $scope.MI_Id_Category = promise.mI_Id;

                angular.forEach($scope.getdepartment, function (dd) {
                    dd.FMSDFC_Id = 0;
                    dd.selected = false;
                });
            });
        };

        $scope.saveFileCategory = function () {
            if ($scope.myForm2.$valid) {

                $scope.deptlist = [];

                angular.forEach($scope.getdepartment, function (dd) {
                    if (dd.selected) {
                        $scope.deptlist.push({ HRMD_Id: dd.hrmD_Id, FMSDFC_Id: dd.FMSDFC_Id });
                    }
                });

                var data = {
                    "FMSMFC_Id": $scope.fmsmfC_Id,
                    "FMSMFC_FileCategoryName": $scope.fmsmfC_FileCategoryName,
                    "FMSMFC_FileCategoryCode": $scope.fmsmfC_FileCategoryCode,
                    "deptlisttemp": $scope.deptlist,
                    "MI_Id": $scope.MI_Id_Category
                };

                apiService.create("FMS_Master_PhysicalFileName/saveFileCategory", data).then(function (promise) {

                    if (promise.retrunMsg !== "") {
                        if (promise.retrunMsg === "Duplicate") {
                            swal("Record already exist..!!");
                            return;
                        }
                        else if (promise.retrunMsg === "false") {
                            swal("Record Not saved / Updated..", 'Fail');
                        }
                        else if (promise.retrunMsg === "Add") {
                            swal("Record Saved Successfully..");
                            $scope.cancel2();
                        }
                        else if (promise.retrunMsg === "Update") {
                            swal("Record Updated Successfully..");
                            $scope.cancel2();
                        }
                        else if (promise.retrunMsg === "Notification") {
                            swal("Notification!");
                            $scope.cancel2();
                        }
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                        }
                    }
                });
            }
            else {
                $scope.submitted2 = true;
            }
        };

        $scope.EditData2 = function (record) {

            angular.forEach($scope.getdepartment, function (dd) {
                dd.selected = false;
            });
            record.MI_Id = $scope.MI_Id_Category;

            apiService.create("FMS_Master_PhysicalFileName/editRecord2", record).then(function (promise) {
                if (promise.editDetails !== null && promise.editDetails.length > 0) {
                    $scope.edit2 = true;
                    $scope.fmsmfC_Id = promise.editDetails[0].fmsmfC_Id;
                    $scope.fmsmfC_FileCategoryName = promise.editDetails[0].fmsmfC_FileCategoryName;
                    $scope.fmsmfC_FileCategoryCode = promise.editDetails[0].fmsmfC_FileCategoryCode;

                    if (promise.geteditdeptDetails !== null && promise.geteditdeptDetails.length > 0) {
                        $scope.geteditdeptDetails = promise.geteditdeptDetails;
                        angular.forEach($scope.getdepartment, function (dd) {
                            angular.forEach($scope.geteditdeptDetails, function (d) {
                                if (dd.hrmD_Id === d.hrmD_Id) {
                                    dd.selected = true;
                                    dd.FMSDFC_Id = d.fmsdfC_Id;
                                }
                            });
                        });
                    }
                }
            });
        };

        $scope.ActivateRecord2 = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.fmsmfC_ActiveFlg === false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }

            data.MI_Id = $scope.MI_Id_Category;

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
                        apiService.create("FMS_Master_PhysicalFileName/ActiveDeactiveRecord2", data).then(function (promise) {
                            if (promise.retrunMsg !== "") {
                                if (promise.retrunMsg === "Activated") {
                                    swal("Record Activated successfully");
                                }
                                else if (promise.retrunMsg === "Deactivated") {
                                    swal("Record Deactivated successfully");
                                }
                                else {
                                    swal("Record Not Activated/Deactivated", 'Fail');
                                }
                                $scope.cancel2();
                            }

                        });
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }
            );
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };

        $scope.cancel2 = function () {
            $scope.fmsmfC_FileCategoryName = "";
            $scope.fmsmfC_FileCategoryCode = "";
            $scope.search = "";
            $scope.MI_Id_Category = "";
            $scope.getdepartment = [];
            $scope.getinstitutiondetails = [];
            $scope.filecategoryDetails = [];
            $scope.submitted2 = false;
            $scope.edit2 = false;
            $scope.loadFileCategorydata();
        };

        $scope.ViewDeptList = function (dd) {
            $scope.categoryname = dd.fmsmfC_FileCategoryName;
            var data = {
                "FMSMFC_Id": dd.fmsmfC_Id,
                "MI_Id": $scope.MI_Id_Category
            };

            apiService.create("FMS_Master_PhysicalFileName/ViewDeptList", data).then(function (promise) {
                if (promise.getsaveddeptDetails !== null && promise.getsaveddeptDetails.length > 0) {
                    $scope.getsaveddeptDetails = promise.getsaveddeptDetails;
                    $('#mymodalviewdetailsfirsttab').modal('show');
                } else {
                    swal("No Records Found");
                }
            });
        };

        $scope.ActiveDeactiveDept = function (data) {
            var mgs = "";
            var confirmmgs = "";
            if (data.fmsdfC_ActiveFlg === false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }
            data.MI_Id = $scope.MI_Id_Category;
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
                        apiService.create("FMS_Master_PhysicalFileName/ActiveDeactiveDept", data).then(function (promise) {
                            if (promise.retrunMsg !== "") {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else if (promise.returnval === false) {
                                    swal("Record " + mgs + " Failed");
                                }
                                else {
                                    swal("Record Not Activated/Deactivated", 'Fail');
                                }

                                if (promise.getsaveddeptDetails !== null && promise.getsaveddeptDetails.length > 0) {
                                    $scope.getsaveddeptDetails = promise.getsaveddeptDetails;
                                }
                            }
                        });
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }
            );
        };

        $scope.objs = {};
        $scope.objs.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.hrmD_DepartmentName).indexOf(angular.lowercase($scope.objs.searchchkbx)) >= 0;
        };

        $scope.isOptionsRequired = function () {
            return !$scope.getdepartment.some(function (options) {
                return options.selected;
            });
        };

        // Master Dispatch Mode
        $scope.loadDispatchModedata = function () {
            var pageid = 2;
            $scope.fmsmtmD_DispatchMode = "";
            $scope.fmsmtmD_DispatchModeDetails = "";
            $scope.search22 = "";
            $scope.MI_Id_Dispatch = "";
            $scope.getinstitutiondetails = [];
            $scope.submitted3 = false;
            $scope.edit3 = false;
            $scope.dispatchmodeDetails = [];

            apiService.getURI("FMS_Master_PhysicalFileName/loadDispatchModedata", pageid).then(function (promise) {
                if (promise.dispatchmodeDetails !== null && promise.dispatchmodeDetails.length > 0) {
                    $scope.gridDispatchMode = promise.dispatchmodeDetails;
                    $scope.dispatchmodeDetails = promise.dispatchmodeDetails;
                }
                $scope.getinstitutiondetails = promise.getinstitutiondetails;
                $scope.MI_Id_Dispatch = promise.mI_Id;
            });
        };

        $scope.saveDispatchMode = function () {
            if ($scope.myForm3.$valid) {
                var data = {
                    "FMSMDMD_Id": $scope.fmsmdmD_Id,
                    "FMSMTMD_DispatchMode": $scope.fmsmtmD_DispatchMode,
                    "FMSMTMD_DispatchModeDetails": $scope.fmsmtmD_DispatchModeDetails,
                    "MI_Id": $scope.MI_Id_Dispatch
                };

                apiService.create("FMS_Master_PhysicalFileName/saveDispatchMode", data).then(function (promise) {

                    if (promise.retrunMsg !== "") {
                        if (promise.retrunMsg === "Duplicate") {
                            swal("Record already exist..!!");
                            return;
                        }
                        else if (promise.retrunMsg === "false") {
                            swal("Record Not saved/Updated", 'Fail');
                        }
                        else if (promise.retrunMsg === "Add") {
                            swal("Record Saved Successfully..");
                            $scope.cancel3();
                        }
                        else if (promise.retrunMsg === "Update") {
                            swal("Record Updated Successfully..");
                            $scope.cancel3();
                        }
                        else if (promise.retrunMsg === "Notification") {
                            swal("Notification!");
                            $scope.cancel3();
                        }
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                        }
                    }
                });
            }
            else {
                $scope.submitted3 = true;
            }
        };

        $scope.EditData3 = function (record) {
            record.MI_Id = $scope.MI_Id_Dispatch;
            apiService.create("FMS_Master_PhysicalFileName/editRecord3", record).then(function (promise) {
                if (promise.editDetails !== null && promise.editDetails.length > 0) {
                    $scope.edit3 = true;
                    $scope.fmsmdmD_Id = promise.editDetails[0].fmsmdmD_Id;
                    $scope.fmsmtmD_DispatchMode = promise.editDetails[0].fmsmtmD_DispatchMode;
                    $scope.fmsmtmD_DispatchModeDetails = promise.editDetails[0].fmsmtmD_DispatchModeDetails;
                }
            });
        };

        $scope.cancel3 = function () {
            $scope.fmsmtmD_DispatchMode = "";
            $scope.fmsmtmD_DispatchModeDetails = "";
            $scope.search22 = "";
            $scope.submitted3 = false;
            $scope.edit3 = false;
            $scope.MI_Id_Dispatch = "";
            $scope.dispatchmodeDetails = [];
            $scope.getinstitutiondetails = [];
            $scope.loadDispatchModedata();
        };

        $scope.ActivateRecord3 = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.fmsmdmD_ActiveFlg === false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }

            data.MI_Id = $scope.MI_Id_Dispatch;

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
                        apiService.create("FMS_Master_PhysicalFileName/ActiveDeactiveRecord3", data).then(function (promise) {
                            if (promise.retrunMsg !== "") {
                                if (promise.retrunMsg === "Activated") {
                                    swal("Record Activated successfully");
                                }
                                else if (promise.retrunMsg === "Deactivated") {
                                    swal("Record Deactivated successfully");
                                }
                                else {
                                    swal("Record Not Activated/Deactivated", 'Fail');
                                }
                                $scope.cancel3();
                            }

                        });
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }
            );
        };

        $scope.interacted3 = function (field) {
            return $scope.submitted3;
        };

        // Master File Cabinet
        $scope.loadFileCabinetdata = function () {
            var pageid = 2;
            $scope.fmsmfcbT_FileCabinetName = "";
            $scope.hrmB_Id = "";
            $scope.hrmF_Id = "";
            $scope.hrmR_Id = "";
            $scope.fmsmfcbT_NoOfRows = "";
            $scope.fmsmfcbT_NoOfColumns = "";
            $scope.search23 = "";
            $scope.fmsmfcbT_Id = 0;
            $scope.rowwnames = [];
            $scope.colsnames = [];
            $scope.getinstitutiondetails = [];
            $scope.MI_Id_cabinet = "";
            $scope.submitted4 = false;
            $scope.edit4 = false;
            $scope.fileCabinetDetails = [];

            apiService.getURI("FMS_Master_PhysicalFileName/loadFileCabinetdata", pageid).then(function (promise) {
                if (promise.fileCabinetDetails !== null && promise.fileCabinetDetails.length > 0) {
                    $scope.gridFileCabinet = promise.fileCabinetDetails;
                    $scope.fileCabinetDetails = promise.fileCabinetDetails;
                }

                if (promise.buildingList !== null && promise.buildingList.length > 0) {
                    $scope.buildingList = promise.buildingList;
                }

                if (promise.roomList !== null && promise.roomList.length > 0) {
                    $scope.roomList = promise.roomList;
                }

                if (promise.floorList !== null && promise.floorList.length > 0) {
                    $scope.floorList = promise.floorList;
                }

                $scope.getinstitutiondetails = promise.getinstitutiondetails;
                $scope.MI_Id_cabinet = promise.mI_Id;
            });
        };

        $scope.cancel4 = function () {
            $scope.fmsmfcbT_FileCabinetName = "";
            $scope.hrmB_Id = "";
            $scope.hrmF_Id = "";
            $scope.hrmR_Id = "";
            $scope.fmsmfcbT_NoOfRows = "";
            $scope.fmsmfcbT_NoOfColumns = "";
            $scope.search23 = "";
            $scope.MI_Id_cabinet = "";
            $scope.fmsmfcbT_Id = 0;
            $scope.rowwnames = [];
            $scope.colsnames = [];
            $scope.getinstitutiondetails = [];
            $scope.submitted4 = false;
            $scope.edit4 = false;
            $scope.fileCabinetDetails = [];
            $scope.loadFileCabinetdata();
        };

        $scope.saveFileCabinet = function () {
            $scope.submitted4 = true;
            if ($scope.myForm4.$valid) {
                $scope.FMS_Master_FileCabinet_Rows = [];
                $scope.FMS_Master_FileCabinet_Columns = [];

                if ($scope.fmsmfcbT_NoOfRows > 0) {
                    angular.forEach($scope.rowwnames, function (row) {
                        $scope.FMS_Master_FileCabinet_Rows.push(row);
                    });
                }

                if ($scope.fmsmfcbT_NoOfColumns > 0) {
                    angular.forEach($scope.colsnames, function (col) {
                        $scope.FMS_Master_FileCabinet_Columns.push(col);
                    });
                }

                var data = {
                    "FMSMFCBT_Id": $scope.fmsmfcbT_Id,
                    "FMSMFCBT_FileCabinetName": $scope.fmsmfcbT_FileCabinetName,
                    "HRMB_Id": $scope.hrmB_Id,
                    "HRMF_Id": $scope.hrmF_Id,
                    "HRMR_Id": $scope.hrmR_Id,
                    "FMSMFCBT_NoOfColumns": $scope.fmsmfcbT_NoOfColumns,
                    "FMSMFCBT_NoOfRows": $scope.fmsmfcbT_NoOfRows,
                    "FMS_Master_FileCabinet_Rows": $scope.FMS_Master_FileCabinet_Rows,
                    "FMS_Master_FileCabinet_Columns": $scope.FMS_Master_FileCabinet_Columns,
                    "MI_Id": $scope.MI_Id_cabinet
                };

                apiService.create("FMS_Master_PhysicalFileName/saveFileCabinet", data).then(function (promise) {

                    if (promise.retrunMsg !== "") {
                        if (promise.retrunMsg === "Duplicate") {
                            swal("Record already exist..!!");
                            return;
                        }
                        else if (promise.retrunMsg === "false") {
                            swal("Record Not saved/Updated", 'Fail');
                        }
                        else if (promise.retrunMsg === "Add") {
                            swal("Record Saved Successfully.");
                            $scope.cancel4();
                        }
                        else if (promise.retrunMsg === "Update") {
                            swal("Record Updated Successfully");
                            $scope.cancel4();
                        }
                        else if (promise.retrunMsg === "Notification") {
                            swal("Notification!");
                            $scope.cancel4();
                        }
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                        }
                    }
                });
            }
            else {
                $scope.submitted4 = true;
            }
        };

        $scope.cleardata = function () {
            $scope.EmployeeDis = false;
        };

        $scope.addrowcolumns = function () {
            $scope.rowwnames = [];
            $scope.colsnames = [];

            for (var i = 1; i <= $scope.fmsmfcbT_NoOfRows; i++) {
                $scope.rowwnames.push({ id: 'rows' + i });
            }

            for (var i1 = 1; i1 <= $scope.fmsmfcbT_NoOfColumns; i1++) {
                $scope.colsnames.push({ id: 'columns' + i1 });
            }
        };

        $scope.EditData4 = function (record) {
            var id = record.fmsmfcbT_Id;
            record.MI_Id = $scope.MI_Id_cabinet;

            apiService.create("FMS_Master_PhysicalFileName/editRecord4", record).then(function (promise) {
                if (promise.editDetails !== null && promise.editDetails.length > 0) {
                    $scope.edit4 = true;
                    $scope.fmsmfcbT_Id = promise.editDetails[0].fmsmfcbT_Id;
                    $scope.fmsmfcbT_FileCabinetName = promise.editDetails[0].fmsmfcbT_FileCabinetName;
                    $scope.hrmB_Id = promise.editDetails[0].hrmB_Id;
                    $scope.hrmF_Id = promise.editDetails[0].hrmF_Id;
                    $scope.hrmR_Id = promise.editDetails[0].hrmR_Id;
                    $scope.fmsmfcbT_NoOfRows = promise.editDetails[0].fmsmfcbT_NoOfRows;
                    $scope.fmsmfcbT_NoOfColumns = promise.editDetails[0].fmsmfcbT_NoOfColumns;
                    $scope.rowwnames = promise.rowdetails;
                    $scope.colsnames = promise.columndetails;
                }
            });
        };

        $scope.ActivateRecord4 = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.fmsmfcbT_ActiveFlg === false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }
            data.MI_Id = $scope.MI_Id_cabinet;
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
                        apiService.create("FMS_Master_PhysicalFileName/ActiveDeactiveRecord4", data).then(function (promise) {
                            if (promise.retrunMsg !== "") {
                                if (promise.retrunMsg === "Activated") {
                                    swal("Record Activated successfully");
                                }
                                else if (promise.retrunMsg === "Deactivated") {
                                    swal("Record Deactivated successfully");
                                }
                                else {
                                    swal("Record Not Activated/Deactivated", 'Fail');
                                }

                                $scope.cancel4();
                            }

                        });
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }
            );
        };

        $scope.interactedFileCabinate = function (field) {
            return $scope.submitted4;
        };
        $scope.obj = {};

        /* Physical File Name & Cabinet Mapping  */
        $scope.LoadFilenameAndCabinet = function () {
            $scope.objs.FMSMPFN_Id_New = "";
            $scope.FMSMFCBT_Id_New = "";
            $scope.FMSMFCR_Id_New = "";
            $scope.FMSMFCC_Id_New = "";
            $scope.submitted5 = false;
            $scope.edit5 = false;
            $scope.search25 = "";
            $scope.MI_Id_mapping = "";
            $scope.getinstitutiondetails = [];
            $scope.gridFileNameCabinetDetails = [];
            $scope.filenamedetails = [];
            var pageid = 2;

            apiService.getURI("FMS_Master_PhysicalFileName/LoadFilenameAndCabinet", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.filenamedetails = promise.filenamedetails;
                    $scope.cabinetnamedetails = promise.cabinetnamedetails;
                    $scope.gridFileNameCabinetDetails = promise.gridFileNameCabinetDetails;

                    $scope.getinstitutiondetails = promise.getinstitutiondetails;
                    $scope.MI_Id_mapping = promise.mI_Id;
                }
            });
        };

        $scope.OnChangephysicalname = function (FMSMPFN_Id_New) {
            var data = {
                "FMSMPFN_Id": FMSMPFN_Id_New.fmsmpfN_Id,
                "MI_Id": $scope.MI_Id_mapping
            };
            apiService.create("FMS_Master_PhysicalFileName/OnChangephysicalname", data).then(function (promise) {
                if (promise !== null) {                    
                    $scope.Checkfilecabinet = promise.checkfilecabinet;
                    if ($scope.Checkfilecabinet !== null && $scope.Checkfilecabinet.length > 0) {
                        $scope.FMSMFCBT_Id_New = promise.fmsmfcbT_Id;
                        $scope.FMSMFCC_Id_New = $scope.Checkfilecabinet[0].fmsmfcC_Id;
                        $scope.FMSMFCR_Id_New = $scope.Checkfilecabinet[0].fmsmfcR_Id;
                        //$scope.FMSMPFN_Id_New = $scope.Checkfilecabinet[0].fmsmpfN_Id;
                        $scope.FMSMPFNFC_Id = $scope.Checkfilecabinet[0].fmsmpfnfC_Id;
                    }
                    

                    $scope.filecabinetrow = promise.filecabinetrow;
                    $scope.filecabinetcolumn = promise.filecabinetcolumn;
                }

            });
        };

        $scope.OnChangeFileCabinet = function () {
            var data = {
                "FMSMPFN_Id": $scope.objs.FMSMPFN_Id_New.fmsmpfN_Id,
                "FMSMFCBT_Id": $scope.FMSMFCBT_Id_New,
                "MI_Id": $scope.MI_Id_mapping
            };
            apiService.create("FMS_Master_PhysicalFileName/OnChangeFileCabinet", data).then(function (promise) {
                $scope.filecabinetrow = promise.filecabinetrow;
                $scope.filecabinetcolumn = promise.filecabinetcolumn;

            });
        };

        $scope.SaveFileNameAndCabinet = function () {
            if ($scope.myForm5.$valid) {
                var data = {
                    "FMSMPFNFC_Id": $scope.FMSMPFNFC_Id,
                    "FMSMPFN_Id": $scope.objs.FMSMPFN_Id_New.fmsmpfN_Id,
                    "FMSMFCBT_Id": $scope.FMSMFCBT_Id_New,
                    "FMSMFCC_Id": $scope.FMSMFCC_Id_New,
                    "FMSMFCR_Id": $scope.FMSMFCR_Id_New,
                    "MI_Id": $scope.MI_Id_mapping
                };
                apiService.create("FMS_Master_PhysicalFileName/SaveFileNameAndCabinet", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved/Updated", 'Fail');
                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully.");
                                $scope.cancel5();
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully");
                                $scope.cancel5();
                            }
                            else if (promise.retrunMsg === "Notification") {
                                swal("Notification!");
                                $scope.cancel5();
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                            }
                        }
                    }
                });
            } else {
                $scope.submitted5 = true;
            }
        };

        $scope.Editfilenamecabinet = function (dd) {
            var data = {
                "FMSMPFNFC_Id": dd.fmsmpfnfC_Id,
                "MI_Id": $scope.MI_Id_mapping
            };
            apiService.create("FMS_Master_PhysicalFileName/Editfilenamecabinet", data).then(function (promise) {


                $scope.Checkfilecabinet = promise.editphysicalfileandcabinet;

                $scope.FMSMFCC_Id_New = $scope.Checkfilecabinet[0].fmsmfcC_Id;
                $scope.FMSMFCR_Id_New = $scope.Checkfilecabinet[0].fmsmfcR_Id;

                //$scope.FMSMPFN_Id_New = $scope.Checkfilecabinet[0].fmsmpfN_Id;

                $scope.objs.FMSMPFN_Id_New = $scope.Checkfilecabinet[0];
                $scope.FMSMPFNFC_Id = $scope.Checkfilecabinet[0].fmsmpfnfC_Id;
                $scope.FMSMFCBT_Id_New = promise.fmsmfcbT_Id;

                $scope.filecabinetrow = promise.filecabinetrow;
                $scope.filecabinetcolumn = promise.filecabinetcolumn;

            });
        };

        $scope.ActivateRecord5 = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.fmsmpfN_ActiveFlg === false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }
            var data1 = {
                "FMSMPFNFC_Id": data.fmsmpfnfC_Id,
                "MI_Id": $scope.MI_Id_mapping
            };

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
                        apiService.create("FMS_Master_PhysicalFileName/ActivateRecord5", data1).then(function (promise) {
                            if (promise.retrunMsg !== "") {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " successfully");
                                }
                                else if (promise.returnval === false) {
                                    swal("Record  " + confirmmgs + "  successfully");
                                }
                                else {
                                    swal("Record Not Activated/Deactivated", 'Fail');
                                }
                                $scope.cancel5();
                            }
                        });
                    }
                    else {
                        swal("Cancelled", "Ok");
                    }
                }
            );
        };

        $scope.cancel5 = function () {
            $scope.objs.FMSMPFN_Id_New = "";
            $scope.FMSMFCBT_Id_New = "";
            $scope.FMSMFCR_Id_New = "";
            $scope.FMSMFCC_Id_New = "";
            $scope.submitted5 = false;
            $scope.edit5 = false;
            $scope.search25 = "";
            $scope.MI_Id_mapping = "";
            $scope.gridFileNameCabinetDetails = [];
            $scope.getinstitutiondetails = [];
            $scope.filenamedetails = [];
            $scope.LoadFilenameAndCabinet();
        };

        $scope.interactedFilenameCabinatemap = function (field) {
            return $scope.submitted5;
        };

        $scope.OnChangeInstitution = function (miid, typetab) {

            if (typetab === "Tab1") {
                $scope.gridviewDetails = [];
                $scope.gridPhysicalFileName = [];
                $scope.getdepartment_physical = [];
            } else if (typetab === "Tab2") {
                $scope.filecategoryDetails = [];
            } else if (typetab === "Tab3") {
                $scope.gridDispatchMode = [];
                $scope.dispatchmodeDetails = [];
            } else if (typetab === "Tab4") {
                $scope.gridFileCabinet = [];
                $scope.fileCabinetDetails = [];
                $scope.buildingList = [];
                $scope.roomList = [];
                $scope.floorList = [];
            } else if (typetab === "Tab3") {
                $scope.filenamedetails = [];
                $scope.cabinetnamedetails = [];
                $scope.gridFileNameCabinetDetails = [];
            }

            var data = {
                "MI_Id": miid,
                "Typetab": typetab
            };

            apiService.create("FMS_Master_PhysicalFileName/OnChangeInstitution", data).then(function (promise) {
                if (promise !== null) {
                    if (typetab === "Tab1") {
                        if (promise.gridviewDetails !== null && promise.gridviewDetails.length > 0) {
                            $scope.gridviewDetails = promise.gridviewDetails;
                            $scope.gridPhysicalFileName = promise.gridviewDetails;
                            $scope.getdepartment_physical = promise.getdepartmentlist;
                            angular.forEach($scope.getdepartment_physical, function (dd) {
                                dd.FMSDPFN_Id = 0;
                                dd.selected = false;
                            });
                        }
                    } else if (typetab === "Tab2") {
                        if (promise.filecategoryDetails !== null && promise.filecategoryDetails.length > 0) {
                            $scope.filecategoryDetails = promise.filecategoryDetails;
                        }
                        $scope.getdepartment = promise.getdepartmentlist;
                        angular.forEach($scope.getdepartment, function (dd) {
                            dd.FMSDFC_Id = 0;
                            dd.selected = false;
                        });
                    } else if (typetab === "Tab3") {
                        if (promise.dispatchmodeDetails !== null && promise.dispatchmodeDetails.length > 0) {
                            $scope.gridDispatchMode = promise.dispatchmodeDetails;
                            $scope.dispatchmodeDetails = promise.dispatchmodeDetails;
                        }
                    } else if (typetab === "Tab5") {
                        $scope.filenamedetails = promise.filenamedetails;
                        $scope.cabinetnamedetails = promise.cabinetnamedetails;
                        $scope.gridFileNameCabinetDetails = promise.gridFileNameCabinetDetails;
                    } else if (typetab === "Tab4") {
                        if (promise.fileCabinetDetails !== null && promise.fileCabinetDetails.length > 0) {
                            $scope.gridFileCabinet = promise.fileCabinetDetails;
                            $scope.fileCabinetDetails = promise.fileCabinetDetails;
                        }

                        if (promise.buildingList !== null && promise.buildingList.length > 0) {
                            $scope.buildingList = promise.buildingList;
                        }

                        if (promise.roomList !== null && promise.roomList.length > 0) {
                            $scope.roomList = promise.roomList;
                        }

                        if (promise.floorList !== null && promise.floorList.length > 0) {
                            $scope.floorList = promise.floorList;
                        }                       
                    }
                }
            });
        };
    }
})();