(function () {
    'use strict';
    angular.module('app').controller('ISM_InteractionController', ISM_InteractionController);
    ISM_InteractionController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function ISM_InteractionController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache) {

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
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag === "ISM_INTERACTION") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }
        $scope.submitted = false;
        $scope.ismtcR_CreationDate = new Date();
        $scope.obj = {};
        $scope.ismtcR_Status = "Open";
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("ISM_Interaction/getdetails", pageid).then(function (promise) {
                $scope.roletype = promise.roletype;
                if ($scope.roletype !== "ClientUser") {
                    $scope.get_department = promise.get_department;
                    if ($scope.get_department.length > 1) {
                        $scope.multidepartment = true;
                    }
                    else {
                        $scope.deaprtment = $scope.get_department[0].hrmD_DepartmentName;
                        $scope.hrmD_Id = $scope.get_department[0].hrmD_Id;
                        $scope.multidepartment = false;
                    }
                }
                if ($scope.roletype === "ClientUser") {
                    $scope.get_client = promise.get_client;
                    if ($scope.get_client.length > 1) {
                        $scope.multiclient = true;
                    }
                    else {
                        $scope.ismmclT_ClientName = $scope.get_client[0].ismmclT_ClientName;
                        $scope.obj.ismmclT_Id = $scope.get_client[0].ismmclT_Id;
                        $scope.multiclient = false;
                    }
                } else {
                    $scope.get_client = promise.get_client;
                }

                $scope.get_project = promise.get_project;
                $scope.get_priority = promise.get_priority;
                $scope.get_days = promise.get_days;
                $scope.get_taskdetails = promise.get_taskdetails;
                $scope.presentCountgrid = $scope.get_taskdetails.length;
            });
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;
                $scope.reverse = !$scope.reverse;
            };
        };
        //======================================== Get Module
        $scope.onprojectChange = function () {
            var data = {
                "HRMD_Id": $scope.hrmD_Id,
                "roletype": $scope.roletype,
                "ISMMPR_Id": $scope.obj.ismmpR_Id
            };
            apiService.create("ISM_Interaction/getmodule", data).
                then(function (promise) {
                    $scope.get_module = promise.get_module;
                });
        };
        //======================================== on client Change
        $scope.onclientchange = function () {
            var data = {
                "ISMMCLT_Id": $scope.obj.ismmclT_Id
            };
            apiService.create("ISM_Interaction/getIEuser", data).
                then(function (promise) {
                    $scope.get_IEuser = promise.get_IEuser;
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

        //========================================= Create
        $scope.createtask = function () {
            if ($scope.files !== null && $scope.files !== undefined && $scope.files !== "") {
                UploadFiles();
            }
            else {
                $scope.savedata();
            }
        };
        $scope.obj = {};
        $scope.savedata = function (obj) {
            var data = {};
            if ($scope.myForm.$valid) {
                if ($scope.files_paths !== null && $scope.files_paths !== undefined && $scope.files_paths !== "") {
                    $scope.files_paths;
                }
                else {
                    $scope.files_paths = "";
                }
                if ($scope.roletype === "ClientUser") {
                    data = {
                        "ISMMPR_Id": $scope.obj.ismmpR_Id,
                        "IVRMM_Id": $scope.obj.ivrmM_Id,
                        "ISMTCR_BugOREnhancementFlg": $scope.ismtcR_BugOREnhancementFlg,
                        "ISMTCR_CreationDate": new Date($scope.ismtcR_CreationDate).toDateString(),
                        "ISMTCR_Title": $scope.ismtcR_Title,
                        "HRMPR_Id": $scope.hrmpR_Id,
                        "ISMTCR_Desc": $scope.ismtcR_Desc,
                        "ISMTCR_Status": $scope.ismtcR_Status,
                        "ISMMCLT_Id": $scope.obj.ismmclT_Id,
                        attachmentArray: $scope.files_paths,
                        "ISMTCR_Id": $scope.ismtcR_Id,
                        "ISMCIM_IEList": $scope.ismciM_IEList,
                        "roletype": $scope.roletype,
                        transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                    };
                }
                else {
                    data = {
                        "HRMD_Id": $scope.hrmD_Id,
                        "ISMMPR_Id": $scope.obj.ismmpR_Id,
                        "IVRMM_Id": $scope.obj.ivrmM_Id,
                        "ISMTCR_BugOREnhancementFlg": $scope.ismtcR_BugOREnhancementFlg,
                        "ISMTCR_CreationDate": new Date($scope.ismtcR_CreationDate).toDateString(),
                        "ISMTCR_Title": $scope.ismtcR_Title,
                        "HRMPR_Id": $scope.hrmpR_Id,
                        "ISMTCR_Desc": $scope.ismtcR_Desc,
                        "ISMTCR_Status": $scope.ismtcR_Status,
                        "ISMMCLT_Id": $scope.obj.ismmclT_Id,
                        attachmentArray: $scope.files_paths,
                        "ISMTCR_Id": $scope.ismtcR_Id,
                        transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                    };
                }
                apiService.create("ISM_Interaction/savedata", data).then(function (promise) {
                    if (promise.returnval === true) {
                        if (promise.ismtcR_Id === 0 || promise.ismtcR_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.ismtcR_Id > 0) {
                            swal('Record updated successfully');
                        }
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.ismtcR_Id === 0 || promise.ismtcR_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.ismtcR_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }

                });
            }
            else {
                $scope.submitted = true;
            }
        };



        //=====================Editrecord....
        $scope.edit = function (task) {
            $scope.ismtcR_Id = task.ismtcR_Id;
            $scope.hrmD_Id = task.hrmD_Id;
            $scope.ismmpR_Id = task.ismmpR_Id;
            $scope.ivrmM_Id = task.ivrmM_Id;
            $scope.ismtcR_BugOREnhancementFlg = task.ismtcR_BugOREnhancementFlg;
            $scope.ismtcR_CreationDate = new Date(task.ismtcR_CreationDate);
            $scope.ismtcR_Title = task.ismtcR_Title;
            $scope.hrmpR_Id = task.hrmpR_Id;
            $scope.ismtcR_Desc = task.ismtcR_Desc;
            $scope.ismtcR_Status = task.ismtcR_Status;
            $scope.files_paths = "";
            $scope.get_taskproject = [];
            $scope.get_taskmodule = [];
            $scope.get_taskclient = [];
            $scope.get_Attdetails = [];
            var data = {
                "ISMTCR_Id": task.ismtcR_Id
            };
            apiService.create("ISM_Interaction/gettaskAttachment", data).then(function (promise) {

                if (promise.get_taskproject.length > 0) {
                    $scope.get_taskproject = promise.get_taskproject;
                    $scope.ismmpR_Id = $scope.get_taskproject[0].ismmpR_Id;
                }
                else {
                    $scope.ismmpR_Id = 0;
                }
                if (promise.get_taskmodule.length > 0) {
                    $scope.get_taskmodule = promise.get_taskmodule;
                    $scope.ivrmM_Id = $scope.get_taskmodule[0].ivrmM_Id;
                }
                else {
                    $scope.ivrmM_Id = 0;
                }

                if (promise.get_taskclient.length > 0) {
                    $scope.get_taskclient = promise.get_taskclient;
                    $scope.obj.ismmclT_Id = $scope.get_taskclient[0].ismmclT_Id;
                }
                else {
                    $scope.obj.ismmclT_Id = 0;
                    // $scope.get_client = "";
                }
                $scope.get_Attdetails = promise.get_Attdetails;
            });
        };

        //=================Active/Deactivate
        $scope.deactive = function (task) {
            $scope.ISMTCR_Id = task.ismtcR_Id;
            var dystring = "";
            if (task.ismtcR_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (task.ismtcR_ActiveFlg === false) {
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
                        apiService.create("ISM_Interaction/deactive", task).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        //===========Clear Field
        $scope.cancel = function () {
            $state.reload();
        };



    }
})();

