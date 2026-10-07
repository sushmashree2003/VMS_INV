(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_PlannerListController', ISM_PlannerListController);
    ISM_PlannerListController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function ISM_PlannerListController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache) {


        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();
        $scope.loaddata = function () {
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            $scope.assignedtasklist = [];
            apiService.getURI("ISM_PlannerCreation/getdetails", pageid).then(function (promise) {
                $scope.get_Assignedtasklist = promise.get_Assignedtasklist;
                angular.forEach($scope.get_Assignedtasklist, function (tt) {
                    $scope.assignedtasklist.push({ ismtcrastO_Id: tt.ISMTCRASTO_Id, ISMTCR_Id: tt.ISMTCR_Id, HRMP_Name: tt.HRMP_Name, ISMTCR_BugOREnhancementFlg: tt.ISMTCR_BugOREnhancementFlg, ISMTCR_CreationDate: tt.ISMTCR_CreationDate, ISMTCR_Title: tt.ISMTCR_Title, ISMTCR_Status: tt.ISMTCR_Status, ISMTCR_TaskNo: tt.ISMTCR_TaskNo, ISMMCLT_ClientName: tt.ISMMCLT_ClientName, ISMTCRASTO_AssignedDate: tt.ISMTCRASTO_AssignedDate, ISMTCRASTO_StartDate: new Date(tt.ISMTCRASTO_StartDate), ISMTCRASTO_EndDate: new Date(tt.ISMTCRASTO_EndDate), ISMTCRASTO_EffortInHrs: tt.ISMTCRASTO_EffortInHrs, assignedby: tt.assignedby });
                });
                $scope.get_Status = promise.get_Status;
                $scope.get_employeelist = promise.get_employeelist;
                $scope.get_department = promise.get_department;
                $scope.presentCountgrid = $scope.get_Assignedtasklist.length;
            });
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;
                $scope.reverse = !$scope.reverse;
            };
        };
        //======================================== Add task Model
        $scope.addtask = function () {
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("ISM_ComplaintsTaskList/getdetails", pageid).then(function (promise) {
                $scope.taskplanner = promise.get_comptasklist;

                if ($scope.taskplanner.length > 0) {
                    $('#myModalAddtoPlanner').modal('show');
                }
                else {
                    swal("No Task Found..... !!");
                }
            });
        };
        $scope.onassignCheck = function () {
            var totcheck = 0;
            var toteffort = 0;
            angular.forEach($scope.assignedtasklist, function (ack) {
                if (ack.checkedvalue === true) {
                    totcheck = totcheck + 1;
                    toteffort += parseInt(ack.ISMTCRASTO_EffortInHrs);
                }
            });
            $scope.totalcheck = totcheck;
            $scope.totaleffort = toteffort;
        };
        //$scope.counteffort = function () {
        //    var toteffort = 0;
        //    var entereffort = 0;
        //    angular.forEach($scope.assignedtasklist, function (ack) {
        //        if (ack.checkedvalue === true) {
        //            entereffort = parseInt(ack.ISMTCRASTO_EffortInHrs);
        //            toteffort += parseInt(ack.ISMTCRASTO_EffortInHrs);
        //            if (entereffort > 24) {
        //                swal("Effort must be less than or equal to 24 Hours ... !!");
        //                ack.ISMTCRASTO_EffortInHrs = 0;
        //            }
        //        }
        //    });

        //    $scope.totaleffort = toteffort;
        //};
        $scope.counteffort = function () {
            var toteffort = 0;
            var entereffort = 0;
            angular.forEach($scope.assignedtasklist, function (ack) {
                if (ack.checkedvalue === true) {
                    entereffort = parseInt(ack.ISMTCRASTO_EffortInHrs);
                    toteffort += parseInt(ack.ISMTCRASTO_EffortInHrs);
                }
            });

            $scope.totaleffort = toteffort;
        };


        //========================================= Add Task
        $scope.saveAddTask = function () {
            $scope.addTaskArray = [];
            angular.forEach($scope.taskplanner, function (tp) {
                if (tp.checkedTask === true) {
                    $scope.addTaskArray.push(tp);
                }
            });
            if ($scope.addTaskArray.length > 0) {
                var data = {
                    "ISMTCRASTO_Id": $scope.ismtcrastO_Id,
                    "ISMTCRASTO_AssignedDate": $scope.ismtcrastO_AssignedDate,
                    "addTaskArray": $scope.addTaskArray
                };
            }
            else {
                swal("Please Select Any Task ..... !!");
            }
            apiService.create("ISM_PlannerCreation/saveAddTask", data).then(function (promise) {
                if (promise.returnval === true) {
                    if (promise.ismtcrastO_Id === 0 || promise.ismtcrastO_Id < 0) {
                        swal('Task add to planner successfully');
                        $state.reload();
                    }
                    else if (promise.ismtcrastO_Id > 0) {
                        swal('Record updated successfully');
                        $state.reload();
                    }
                }
                else if (promise.returnduplicatestatus === 'Duplicate') {
                    swal('Record already exist');
                }
                else {
                    if (promise.ismtcrastO_Id === 0 || promise.ismtcrastO_Id < 0) {
                        swal('Failed to add, please contact administrator');
                    }
                    else if (promise.ismtcrastO_Id > 0) {
                        swal('Failed to update, please contact administrator');
                    }
                }

            });

        };

        //========================================= Save Planner
        $scope.savePlanner = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.taskplannerArray = [];
                if ($scope.totaleffort >= 48) {
                    angular.forEach($scope.assignedtasklist, function (tp) {
                        if (tp.checkedvalue === true) {
                            $scope.taskplannerArray.push({ ISMTCR_Id: tp.ISMTCR_Id, ISMTPLTA_StartDate: new Date(tp.ISMTCRASTO_StartDate), ISMTPLTA_EndDate: new Date(tp.ISMTCRASTO_EndDate), ISMTPLTA_EffortInHrs: tp.ISMTCRASTO_EffortInHrs, ISMTPLTA_Remarks: tp.ISMTPLTA_Remarks, ISMTPLTA_Id: tp.ISMTPLTA_Id });
                        }
                    });

                    var data = {
                        "ISMTPL_Id": $scope.ismtpL_Id,
                        "ISMTPL_PlannerName": $scope.ismtpL_PlannerName,
                        "ISMTPL_StartDate": new Date($scope.ismtpL_StartDate),
                        "ISMTPL_EndDate": new Date($scope.ismtpL_EndDate),
                        "ISMTPL_TotalHrs": $scope.totaleffort,
                        "ISMTPL_Remarks": $scope.ismtpL_Remarks,
                        "taskplannerArray": $scope.taskplannerArray
                    };
                }
                else {
                    swal("Total Effort must be greater than or equal to 48 Hours ... !!");
                }
                apiService.create("ISM_PlannerCreation/savePlanner", data).then(function (promise) {
                    if (promise.returnval === true) {
                        if (promise.ismtpL_Id === 0 || promise.ismtpL_Id < 0) {
                            swal('Planner Saved successfully ... !!');
                            $state.reload();
                        }
                        else if (promise.ismtpL_Id > 0) {
                            swal('Record updated successfully');
                            $state.reload();
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.ismtpL_Id === 0 || promise.ismtpL_Id < 0) {
                            swal('Failed to Save, please contact administrator');
                        }
                        else if (promise.ismtpL_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }

                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.removetask = function (objtask) {
            $scope.ISMTCRASTO_Id = objtask.ismtcrastO_Id;
            var tasknum = objtask.ISMTCR_TaskNo;

            var dystring = "Remove";
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Task " + tasknum + "..?",

                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ISM_PlannerCreation/removetask", objtask).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Task " + dystring + "d Successfully..");
                                    $state.reload();
                                }
                                else {
                                    swal("Falied to " + dystring + " Task....!!!");
                                }

                            });
                    }
                    else {
                        swal("Task " + dystring + " Cancelled!!!");
                    }
                });
        };

        //=========================================Response
        $scope.viewresponse = function (taskobj) {
            $('#myModalResponse').modal('show');
            var data = {
                "ISMTCR_Id": taskobj.ISMTCR_Id
            };
            apiService.create("ISM_ComplaintsTaskList/gettaskdetails", data).
                then(function (promise) {
                    $scope.get_taskdetails = promise.get_taskdetails;
                    if ($scope.get_taskdetails !== null && $scope.get_taskdetails.length > 0) {
                        $scope.tasktitle = $scope.get_taskdetails[0].ISMTCR_Title;
                        $scope.taskno = $scope.get_taskdetails[0].ISMTCR_TaskNo;
                        $scope.createdby = $scope.get_taskdetails[0].createdby;
                        $scope.taskclient = $scope.get_taskdetails[0].ISMMCLT_ClientName;
                        $scope.taskdate = $scope.get_taskdetails[0].ISMTCR_CreationDate;
                        if ($scope.get_taskdetails[0].ISMTCR_BugOREnhancementFlg === "E") {
                            $scope.tasktype = "Enhancement";
                        }
                        else {
                            $scope.tasktype = "Bugs & Complaints";
                        }
                        $scope.tsakpriority = $scope.get_taskdetails[0].HRMP_Name;
                        $scope.taskdescription = $scope.get_taskdetails[0].ISMTCR_Desc;
                        $scope.taskstatus = $scope.get_taskdetails[0].ISMTCR_Status;
                    }
                });
        };
        //=====================================Upload
        $scope.stepsModel = [];
        $scope.filedata = [];
        $scope.selectFileforUpload = function (event) {
            $scope.files = event.files;
            for (var i = 0; i < $scope.files.length; i++) {
                var file = $scope.files[i].name;
                $scope.filedata = $scope.files.FileList;
                $scope.fileimg = file;
                var reader = new FileReader();
                reader.onload = $scope.imageIsLoaded;
                reader.readAsDataURL(file);
            }
        };
        //========Remove Selected File 
        $scope.remove_file = function () {
            $scope.file_detail = "";
            $scope.files = "";
        };
        $scope.imageIsLoaded = function (e) {
            $scope.$apply(function () {
                $scope.stepsModel.push(e.target.result);
            });
        };
        function UploadFiles() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.files.length; i++) {
                formData.append("File", $scope.files[i]);
                //   $scope.file_detail = $scope.files[i].name;
            }
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_ISMAttachment", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    swal(d);
                    $scope.files_paths = d;
                    if ($scope.files_paths.length > 0) {
                        $scope.savedata();
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.cancel = function () {
            $state.reload();
        };

    }
})();

