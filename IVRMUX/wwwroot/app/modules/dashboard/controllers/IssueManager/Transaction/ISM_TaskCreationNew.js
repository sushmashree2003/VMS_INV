(function () {
    'use strict';
    angular.module('app').controller('ISM_TaskCreationNewController', ISM_TaskCreationController);
    ISM_TaskCreationController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$window', 'intracontroller', '$sce'];
    function ISM_TaskCreationController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter, $window, intracontroller, $sce) {

        $scope.emplisttemplist = [];
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.obj = {};
        $scope.fixedtime = false;
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        if (transnumconfigsettings != null) {
            for (var i = 0; i < transnumconfigsettings.length; i++) {
                if (transnumconfigsettings.length > 0) {
                    if (transnumconfigsettings[i].imN_Flag === "ISMTASK") {
                        $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                    }
                }
            }
        }
        $scope.obj.ismtcR_Hours = 0;
        $scope.plMaxdatem = new Date();
        $scope.obj.selfassign = 'N';
        $scope.ismtcR_Percentage = 100;
        $scope.excelflag = false;
        $scope.getserverdate = function () {
            var xmlHttp;
            function srvTime() {
                try {
                    //FF, Opera, Safari, Chrome
                    xmlHttp = new XMLHttpRequest();
                }
                catch (err1) {
                    //IE
                    try {
                        xmlHttp = new ActiveXObject('Msxml2.XMLHTTP');
                    }
                    catch (err2) {
                        try {
                            xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
                        }
                        catch (eerr3) {
                            //AJAX not supported, use CPU time.
                            alert("AJAX not supported");
                        }
                    }
                }
                xmlHttp.open('HEAD', window.location.href.toString(), false);
                xmlHttp.setRequestHeader("Content-Type", "text/html");
                xmlHttp.send('');
                return xmlHttp.getResponseHeader("Date");
            }
            $scope.today = srvTime();
        }
        $scope.getserverdate();
        $scope.obj.totalhours = 0;
        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.filterValue3, function (uem) {
                uem.checked = toggleStatus;
            });
        };
      
        $scope.togchkbx = function () {
            $scope.checkall = $scope.emplist.every(function (options) {
                return options.checked;
            });
        };
        $scope.TypeFlg = "";
        $scope.searchValueE = '';
        $scope.filterValue2 = function (obj) {
            return angular.lowercase(obj.employeeName).indexOf(angular.lowercase($scope.searchValueE)) >= 0 || angular.lowercase(obj.hrmD_DepartmentName).indexOf(angular.lowercase($scope.searchValueE)) >= 0
                || angular.lowercase(obj.hrmdeS_DesignationName).indexOf(angular.lowercase($scope.searchValueE)) >= 0;
        };

        $scope.submitted = false;
        $scope.ismtcR_CreationDate = new Date($scope.today);

        $scope.taskDate = new Date($scope.today);

        $scope.ismtcR_Status = "Open";

        $scope.ondatechange = function () {
            $scope.obj.startDate = new Date($scope.today);
            $scope.documentListOtherDetails = [];
            $scope.documentListOtherDetails = [{ id: 'document' }];
            //$scope.ismtcR_BugOREnhancementFlg = "B";
        };
        $scope.ondatechangedocument = function () {
            $scope.documentListOtherDetails = [];
            $scope.documentListOtherDetails = [{ id: 'document' }];
        }

        $scope.ondatechangesta = function () {
            var d = new Date($scope.obj.startDate);
            d.setDate(d.getDate() + 5);
            $scope.obj.endDate = d;
        };
        $scope.EmplyeeEnhancements = [];
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            var data = {
                "PageFlag": "app.ISM_TaskCreationNew"
            }

            apiService.create("ISM_TaskCreation/getdetails", data).then(function (promise) {
                if (promise.typeFlg == "PageNotMapped") {
                    $scope.TypeFlg = promise.typeFlg;
                }
                $scope.roletype = promise.roletype;
                $scope.get_department = promise.get_department;
                $scope.hrmeid = promise.hrmE_Id,
                    $scope.get_employeelist = promise.get_employeelist;
                $scope.get_priority = promise.get_priority;
                $scope.get_days = promise.get_days;
                $scope.get_taskdetails = promise.get_taskdetails;
                $scope.presentCountgrid = $scope.get_taskdetails.length;

                if (promise.plannerextapproval === true) {
                    $scope.plannerextapproval = false;
                    $scope.plannerMaxdate = promise.plannerMaxdate;
                    $scope.plMaxdate = new Date($scope.plannerMaxdate);
                    $scope.plMaxdate.setDate($scope.plMaxdate.getDate());
                    $scope.maxdate = new Date($scope.today);
                }
                else {
                    $scope.plannerextapproval = true;
                    $scope.plMaxdate = new Date($scope.today);
                    //$scope.ismtcR_CreationDate = new Date($scope.today);
                }

                angular.forEach($scope.get_taskdetails, function (atck) {
                    atck.atck = $filter('date')(atck.ISMTCR_CreationDate, "dd-MM-yyyy");
                });
                $scope.empll = [];
                var id = 0;
                if ($scope.get_employeelist.length > 0) {
                    angular.forEach($scope.get_employeelist, function (ll) {
                        id += 1;
                        if ($scope.hrmeid === ll.HRME_Id) {
                            if ($scope.get_employeelist.length === 1) {
                                $scope.empll.push({ id: id, HRME_Id: ll.HRME_Id, employeeName: ll.employeeName, hrmD_DepartmentName: ll.HRMD_DepartmentName, hrmdeS_DesignationName: ll.HRMDES_DesignationName, checked: true, hrmdc_id: ll.HRMDC_ID });
                            }
                            else {
                                $scope.empll.push({ id: id, HRME_Id: ll.HRME_Id, employeeName: ll.employeeName, hrmD_DepartmentName: ll.HRMD_DepartmentName, hrmdeS_DesignationName: ll.HRMDES_DesignationName, checked: false, hrmdc_id: ll.HRMDC_ID });
                            }
                        }
                        else {
                            $scope.empll.push({ id: id, HRME_Id: ll.HRME_Id, employeeName: ll.employeeName, hrmD_DepartmentName: ll.HRMD_DepartmentName, hrmdeS_DesignationName: ll.HRMDES_DesignationName, checked: false, hrmdc_id: ll.HRMDC_ID });
                        }
                    });
                    $scope.emplist = $scope.empll;
                    $scope.emplisttemplist = $scope.empll;
                }
                $scope.EmplyeeEnhancements = promise.emplyeeEnhancements;

            });
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;
                $scope.reverse = !$scope.reverse;
            };
        };
        //=====================get category============
        $scope.get_category1 = function () {
            $scope.emplist = [];
            $scope.emplist = $scope.emplisttemplist;
            $scope.get_module = [];
            $scope.get_category = [];
            $scope.get_project = [];
            $scope.get_client = [];
            $scope.materaldocuupload = [];
            $scope.materaldocuupload = [{ id: 'materal' }];

            $scope.documentListOtherDetails = [];
            $scope.documentListOtherDetails = [{ id: 'document' }];

            $scope.Module = "";
            $scope.Client = "";
            $scope.Project = "";
            $scope.Category = "";
            var data = {
                "HRMD_Id": $scope.Dept.hrmD_Id
            };
            apiService.create("ISM_TaskCreation/get_category", data).
                then(function (promise) {
                    if (promise.get_category !== null && promise.get_category.length > 0) {
                        $scope.get_category = promise.get_category;
                    }
                    else {
                        swal("No category mapped for this department!!");
                    }

                    if (promise.get_project !== null && promise.get_project.length > 0) {
                        $scope.get_project = promise.get_project;
                    }
                    else {
                        swal("No Project mapped for this department!!");
                    }
                    $scope.emplist = $filter('filter')($scope.emplist, function (d) {
                        return (d.hrmdc_id === $scope.Dept.hrmdC_ID);
                    });
                    angular.forEach($scope.emplist, function (atck) {
                        atck.checked = false;
                    });
                });
        };

        $scope.oncategorychange = function (ob) {
            $scope.obj.effmin = 0;
            $scope.obj.effhrs = 0;
            $scope.fixedtime = false;
            if (ob.ismmtcaT_EachTaskMaxDuration != undefined && ob.ismmtcaT_EachTaskMaxDuration != null && ob.ismmtcaT_EachTaskMaxDuration > 0) {
                if (ob.ismmtcaT_DurationFlg == "HOURS") {
                    $scope.obj.effhrs = parseInt(Number(ob.ismmtcaT_EachTaskMaxDuration));
                    $scope.obj.effmin = 0;
                    $scope.fixedtime = true;
                }
                if (ob.ismmtcaT_DurationFlg == "MINUTES") {
                    $scope.obj.effhrs = 0;
                    $scope.obj.effmin = parseInt(Number(ob.ismmtcaT_EachTaskMaxDuration));
                    $scope.fixedtime = true;
                }
            }
        };
        //======================================== Get Module
        $scope.onprojectChange = function () {
            $scope.get_module = [];
            $scope.get_client = [];
            $scope.Module = "";
            $scope.Client = "";
            var data = {
                "HRMD_Id": $scope.Dept.hrmD_Id,
                "roletype": $scope.roletype,
                "ISMMPR_Id": $scope.Project.ismmpR_Id
            };
            apiService.create("ISM_TaskCreation/getmodule", data).
                then(function (promise) {
                    if (promise.get_module !== null && promise.get_module.length > 0) {
                        $scope.get_module = promise.get_module;
                    }
                    else {
                        swal("No Module mapped for this Department-Project !!");
                    }
                    if (promise.get_client !== null && promise.get_client.length > 0) {
                        $scope.get_client = promise.get_client;
                    }
                    else {
                        swal("No Client mapped for this Project !!");
                    }

                });
        };
        //======================================== on client Change
        $scope.onclientchange = function () {
            $scope.get_module = [];
            $scope.Module = "";
            var data = {
                "HRMD_Id": $scope.Dept.hrmD_Id,
                "roletype": $scope.roletype,
                "ISMMPR_Id": $scope.Project.ismmpR_Id,
                "ISMMCLT_Id": $scope.Client.ismmclT_Id
            };
            apiService.create("ISM_TaskCreation/getIEuser", data).
                then(function (promise) {
                    $scope.get_IEuser = promise.get_IEuser;
                    $scope.get_module = promise.get_module;
                });
        };
        //=====================================Upload
        $scope.stepsModel = [];
        $scope.filedata = [];
        $scope.selectFileforUpload = function (event) {
            $scope.files = event.files;
            for (var i = 0; i < $scope.files.length; i++) {
                var file = $scope.files[i].name;
                $scope.filedata = $scope.files;
                $scope.fileimg = file;
                var reader = new FileReader();
                reader.onload = $scope.imageIsLoaded;
                reader.readAsDataURL($scope.files[i]);
            }
        };

        $scope.employeecheck = function (checkedvalue) {
            angular.forEach($scope.emplist, function (qq) {
                if (checkedvalue.id === qq.id) {
                    qq.checked = true;
                }
                else {
                    qq.checked = false;
                }
            });
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
            $scope.plannerArray = [];
            $scope.taskEmpArray = [];
            $scope.files_paths = [];
            if ($scope.myForm.$valid) {
                $scope.MulttaskDepartments = [];
                if ($scope.documentListOtherDetails !== null) {
                    angular.forEach($scope.documentListOtherDetails, function (qq) {
                        if (qq.file !== undefined && qq.file !== null) {
                            $scope.files_paths.push({ ISMTCRAT_Attatchment: qq.file, ISMTCRAT_File: qq.FileName });
                        }
                    });
                }
                else {
                    $scope.files_paths = undefined;
                }
                if ($scope.ismtcR_BugOREnhancementFlg == "E") {
                    if ($scope.files_paths != null && $scope.files_paths.length > 0) {

                    }
                    else {
                        swal("Please Attached BRD Discussion Poionts !");
                        return;
                    }
                    if ($scope.obj.totalhours >= 100) {

                    }
                    else {
                        swal("Total Percnatge Must Be  100 % !");
                        return;
                    }
                    //TTEDDP_HRMDCID
                    if ($scope.materaldocuupload != null && $scope.materaldocuupload.length > 0) {
                        angular.forEach($scope.materaldocuupload, function (materal) {
                            var counthrs = 0.00;
                            counthrs = (parseFloat(materal.effmin) * 0.0166667).toFixed(2);
                            counthrs = parseFloat(materal.effhrs) + parseFloat(counthrs);

                            if (materal.Deptid.hrmD_Id > 0 && materal.hrmE_Id.hrmE_Id > 0 && materal.percentage > 0) {
                                $scope.MulttaskDepartments.push({
                                    TTEDDP_HRMDCID: materal.Deptid.hrmD_Id,
                                    TTEDDP_Percentage: materal.percentage,
                                    TTEDDP_Days: materal.tteddP_Days,
                                    TTEDDP_Days: materal.tteddP_Days,
                                    TTEDDP_level: materal.tteddP_level,
                                    TTEDDP_Remarks: materal.tteddP_Remarks,
                                    HRME_Id: materal.hrmE_Id.hrmE_Id,
                                    TTEDDP_FromDate: new Date(materal.startDate).toDateString(),
                                    TTEDDP_ToDate: new Date(materal.endDate).toDateString(),
                                    TTEDDP_Hours: counthrs
                                })
                            }
                            else {
                                swal("Please select  Departments   !");
                                return;
                            }
                        });

                    }
                    if ($scope.MulttaskDepartments != null && $scope.MulttaskDepartments.length > 0) {

                    }
                    else {

                        swal("Please select BRD  Departments   !");
                        return;
                    }
                }
                if ($scope.obj.selfassign == "N") {
                    if ($scope.files_paths != null && $scope.files_paths.length > 0) {

                    }
                    else {
                        swal("Please Attached  Excel  File  !");
                        return;
                    }
                }
                //obj.selfassign
                data = {
                    "HRMD_Id": $scope.Dept.hrmD_Id,
                    "ISMMPR_Id": $scope.Project.ismmpR_Id,
                    "ISMMCLT_Id": $scope.Client.ismmclT_Id,
                    "ISMMTCAT_Id": $scope.Category.ismmtcaT_Id,
                    "ISMTCR_Title": $scope.ismtcR_Title,
                    "ISMTCR_Desc": $scope.ismtcR_Desc,
                    "ISMTCR_CreationDate": new Date($scope.ismtcR_CreationDate).toDateString(),
                    "IVRMM_Id": $scope.Module.ivrmM_Id,
                    "HRMPR_Id": $scope.hrmpR_Id,
                    "ISMTCR_Status": $scope.ismtcR_Status,
                    "ISMTCR_BugOREnhancementFlg": $scope.ismtcR_BugOREnhancementFlg,
                    attachmentArray: $scope.files_paths,
                    "ISMTCR_Id": $scope.ismtcR_Id,
                    "ISMCIM_IEList": $scope.ismciM_IEList,
                    "roletype": $scope.roletype,
                    transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                    "assignto": $scope.obj.selfassign,
                    "MulttaskDepartments": $scope.MulttaskDepartments,
                    "ISMTCR_Hours": $scope.obj.ismtcR_Hours,
                    "ISMTCR_Days": $scope.obj.ismtcR_Days
                };
                
                if ($scope.obj.selfassign === 'Y' || $scope.ismtcR_BugOREnhancementFlg === 'T') {
                    var counthrs = 0.00;
                    var totalhrs = 0.00;
                    counthrs = (parseFloat($scope.obj.effmin) * 0.0166667).toFixed(2);
                    counthrs = parseFloat($scope.obj.effhrs) + parseFloat(counthrs);
                    angular.forEach($scope.emplist, function (ll) {
                        if (ll.checked === true) {
                            $scope.taskEmpArray.push({ HRME_Id: ll.HRME_Id });
                        }
                    });
                    if ($scope.taskEmpArray.length === 0) {
                        swal("Select any employee to assign task!!");
                        return;
                    };
                    var Yearlydatenew = null;
                    if ($scope.obj.yearly !== null && $scope.obj.yearly !== undefined) {
                        Yearlydatenew: new Date($scope.obj.yearly).toDateString();
                    }

                    if ($scope.obj.taskday === null || $scope.obj.taskday === undefined) {
                        $scope.obj.taskday = 0;
                    }
                    if (counthrs == 0 || counthrs == null) {
                        swal("Kindly enter effort for the task!!");
                        return;
                    };
                    data.effortinhrs = counthrs;
                    data.periodicity = $scope.obj.periodicity;
                    data.taskEmpArray = $scope.taskEmpArray;
                    data.remarks = $scope.obj.remarks;
                    data.startdate = new Date($scope.obj.startDate).toDateString();
                    data.enddate = new Date($scope.obj.endDate).toDateString();
                    data.TimeRequiredFlg = "HOURS";
                    data.TaskDay = $scope.obj.taskday;
                    data.Yearlydate = new Date(Yearlydatenew).toDateString();
                }
                if ($scope.MulttaskDepartments != null && $scope.MulttaskDepartments.length > 0) {
                    data.ISMTCR_MainGroupTaskFlg = true;
                }
                else {
                    data.ISMTCR_MainGroupTaskFlg = false;

                }
                apiService.create("ISM_TaskCreation/savedata", data).then(function (promise) {
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



        $scope.savedata_backup = function (obj) {
            var data = {};
            $scope.plannerArray = [];
            $scope.taskEmpArray = [];
            $scope.files_paths = [];
            if ($scope.myForm.$valid) {
                if ($scope.documentListOtherDetails !== null) {
                    angular.forEach($scope.documentListOtherDetails, function (qq) {
                        if (qq.file !== undefined && qq.file !== null) {
                            $scope.files_paths.push({ ISMTCRAT_Attatchment: qq.file, ISMTCRAT_File: qq.FileName });
                        }
                    });
                }
                else {
                    $scope.files_paths = undefined;
                }


                if ($scope.roletype === "ClientUser") {
                    data = {
                        "HRMD_Id": $scope.Dept.hrmD_Id,
                        "ISMMPR_Id": $scope.Project.ismmpR_Id,
                        "ISMMCLT_Id": $scope.Client.ismmclT_Id,
                        "ISMMTCAT_Id": $scope.Category.ismmtcaT_Id,
                        "ISMTCR_Title": $scope.ismtcR_Title,
                        "ISMTCR_Desc": $scope.ismtcR_Desc,
                        "ISMTCR_CreationDate": new Date($scope.ismtcR_CreationDate).toDateString(),
                        "IVRMM_Id": $scope.Module.ivrmM_Id,
                        "HRMPR_Id": $scope.hrmpR_Id,
                        "ISMTCR_Status": $scope.ismtcR_Status,
                        "ISMTCR_BugOREnhancementFlg": $scope.ismtcR_BugOREnhancementFlg,
                        attachmentArray: $scope.files_paths,
                        "ISMTCR_Id": $scope.ismtcR_Id,
                        "ISMCIM_IEList": $scope.ismciM_IEList,
                        "roletype": $scope.roletype,
                        transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                        "assignto": $scope.obj.selfassign
                    };
                }
                else {
                    if ($scope.obj.selfassign === 'N' && $scope.ismtcR_BugOREnhancementFlg !== 'T') {
                        data = {
                            "HRMD_Id": $scope.Dept.hrmD_Id,
                            "ISMMPR_Id": $scope.Project.ismmpR_Id,
                            "ISMMCLT_Id": $scope.Client.ismmclT_Id,
                            "ISMMTCAT_Id": $scope.Category.ismmtcaT_Id,
                            "ISMTCR_Title": $scope.ismtcR_Title,
                            "ISMTCR_Desc": $scope.ismtcR_Desc,
                            "ISMTCR_CreationDate": new Date($scope.ismtcR_CreationDate).toDateString(),
                            "IVRMM_Id": $scope.Module.ivrmM_Id,
                            "HRMPR_Id": $scope.hrmpR_Id,
                            "ISMTCR_Status": $scope.ismtcR_Status,
                            "ISMTCR_BugOREnhancementFlg": $scope.ismtcR_BugOREnhancementFlg,
                            attachmentArray: $scope.files_paths,
                            "ISMTCR_Id": $scope.ismtcR_Id,
                            "ISMCIM_IEList": $scope.ismciM_IEList,
                            "roletype": $scope.roletype,
                            transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                            "assignto": $scope.obj.selfassign
                        };
                    }
                    else if ($scope.obj.selfassign === 'Y' || $scope.ismtcR_BugOREnhancementFlg === 'T') {
                        var counthrs = 0.00;
                        var totalhrs = 0.00;

                        //if ($scope.obj.durationFlg === "HOURS") {
                        //    counthrs = parseFloat($scope.obj.eachTaskMaxDuration);
                        //} else if ($scope.obj.durationFlg === "MINUTES") {
                        //    counthrs = (parseFloat($scope.obj.eachTaskMaxDuration) * 0.0166667).toFixed(2);
                        //}

                        counthrs = (parseFloat($scope.obj.effmin) * 0.0166667).toFixed(2);
                        counthrs = parseFloat($scope.obj.effhrs) + parseFloat(counthrs);

                        if (counthrs == 0 || counthrs == null) {
                            swal("Kindly enter effort for the task!!");
                            return;
                        };

                        angular.forEach($scope.emplist, function (ll) {
                            if (ll.checked === true) {
                                $scope.taskEmpArray.push({ HRME_Id: ll.HRME_Id });
                            }
                        });
                        if ($scope.taskEmpArray.length === 0) {
                            swal("Select any employee to assign task!!");
                            return;
                        };
                        var Yearlydatenew = null;
                        if ($scope.obj.yearly !== null && $scope.obj.yearly !== undefined) {
                            Yearlydatenew: new Date($scope.obj.yearly).toDateString();
                        }

                        if ($scope.obj.taskday === null || $scope.obj.taskday === undefined) {
                            $scope.obj.taskday = 0;
                        }

                        data = {
                            "HRMD_Id": $scope.Dept.hrmD_Id,
                            "ISMMPR_Id": $scope.Project.ismmpR_Id,
                            "ISMMCLT_Id": $scope.Client.ismmclT_Id,
                            "ISMMTCAT_Id": $scope.Category.ismmtcaT_Id,
                            "ISMTCR_Title": $scope.ismtcR_Title,
                            "ISMTCR_Desc": $scope.ismtcR_Desc,
                            "ISMTCR_CreationDate": new Date($scope.ismtcR_CreationDate).toDateString(),
                            "IVRMM_Id": $scope.Module.ivrmM_Id,
                            "HRMPR_Id": $scope.hrmpR_Id,
                            "ISMTCR_Status": $scope.ismtcR_Status,
                            "ISMTCR_BugOREnhancementFlg": $scope.ismtcR_BugOREnhancementFlg,
                            attachmentArray: $scope.files_paths,
                            "roletype": $scope.roletype,
                            transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                            "assignto": $scope.obj.selfassign,
                            "effortinhrs": counthrs,
                            "periodicity": $scope.obj.periodicity,
                            "taskEmpArray": $scope.taskEmpArray,
                            "remarks": $scope.obj.remarks,
                            "startdate": new Date($scope.obj.startDate).toDateString(),
                            "enddate": new Date($scope.obj.endDate).toDateString(),
                            "TimeRequiredFlg": "HOURS",
                            "TaskDay": $scope.obj.taskday,
                            "Yearlydate": new Date(Yearlydatenew).toDateString(),
                            "ISMTCR_Days": $scope.obj.ismtcR_Days
                        };
                    }
                }
                apiService.create("ISM_TaskCreation/savedata", data).then(function (promise) {
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
            apiService.create("ISM_TaskCreation/gettaskAttachment", data).then(function (promise) {

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
                    $scope.Client.ismmclT_Id = $scope.get_taskclient[0].ismmclT_Id;
                }
                else {
                    $scope.Client.ismmclT_Id = 0;
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
                        apiService.create("ISM_TaskCreation/deactive", task).
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
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.cancel = function () {
            $state.reload();
        };

        $scope.getbymecount = function () {
            $scope.countflag = "ByMe";
            $scope.getscounts();
        };
        $scope.gettomecount = function () {
            $scope.countflag = "ToMe";
            $scope.getscounts();
        };
        $scope.getscounts = function () {
            $scope.get_comptasklist = [];
            $scope.get_tasks = [];
            var data = {
                "countflag": $scope.countflag
            };
            apiService.create("ISM_ComplaintsTaskList/getcount", data).
                then(function (promise) {
                    if (promise.get_tasks.length > 0) {
                        $scope.get_taskdetails = promise.get_tasks;
                        $scope.presentCountgrid = $scope.get_taskdetails.length;
                        angular.forEach($scope.get_taskdetails, function (atck) {
                            atck.atck = $filter('date')(atck.ISMTCR_CreationDate, "dd-MM-yyyy");
                        });

                        //$scope.get_taskdetails = promise.get_taskdetails;
                        //$scope.presentCountgrid = $scope.get_taskdetails.length;

                        //angular.forEach($scope.get_taskdetails, function (atck) {
                        //    atck.atck = $filter('date')(atck.ISMTCR_CreationDate, "dd-MM-yyyy");
                        //});
                    }
                    else {
                        $scope.get_tasks = "";
                    }
                });
        };

        //===========================ADD==================================
        $scope.documentListOtherDetails = [{ id: 'document' }];
        $scope.addNewDocumentOtherDetail = function (other) {
            $scope.excelflag = false;
            var newItemNo = $scope.documentListOtherDetails.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListOtherDetails.push({ 'id': 'document' + newItemNo });
            }
            if (other.file != null && other.file != "") {
                $scope.excelflag = true;
            }
        };

        $scope.removeNewDocumentOtherDetail = function (index, data) {
            var newItemNo = $scope.documentListOtherDetails.length - 1;
            $scope.documentListOtherDetails.splice(index, 1);
            if (data.hreothdeT_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
            if ($scope.documentListOtherDetails != null && $scope.documentListOtherDetails.length > 1) {
                $scope.excelflag = true;
            }
            else {
                $scope.excelflag = false;
            }
            if (index == 0 && $scope.deptName == "Software") {
                $scope.documentListOtherDetails = [];
                $scope.documentListOtherDetails = [{ id: 'document' }];
            }
        };
        //==============================

        //========================upload files===========
        $scope.deptName = "";
        $scope.selectFileforUploadzdOtherDetail = function (input, document) {
            $scope.ldr = true;
            //$('#' + document.id).removeAttr('src');
            $scope.deptName = "";
            if ($scope.get_department != null && $scope.get_department.length > 0 && $scope.Dept.hrmD_Id > 0) {
                angular.forEach($scope.get_department, function (atck) {
                    if (atck.hrmD_Id == $scope.Dept.hrmD_Id) {
                        $scope.deptName = atck.hrmD_DepartmentName;
                    }
                });
            }
            else {
                swal("Plaese Select  Department Name");
                return;
            }           
            $scope.SelectedFileForUploadzdOtherDetail = input.files;
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                var extention = nameArray[nameArray.length - 1];
                document.extention = nameArray[nameArray.length - 1];
                if ($scope.obj.selfassign == "Y") {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blahD')
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentOtherDetail(document);
                }
                else {
                    if ($scope.deptName == "Software") {
                        $scope.excelflag == false;
                    }
                    else {
                        $scope.excelflag == true;
                    }
                    if (($scope.excelflag == false && $scope.deptName =="Software"  && (extention === "xlsx" || extention === "xls"))) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $('#blahD')
                                .attr('src', e.target.result);
                        };
                        reader.readAsDataURL(input.files[0]);
                        UploadEmployeeDocumentOtherDetail(document);
                    }
                    else if ($scope.excelflag == true) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $('#blahD')
                                .attr('src', e.target.result);
                        };
                        reader.readAsDataURL(input.files[0]);
                        UploadEmployeeDocumentOtherDetail(document);
                    }
                    else if ($scope.excelflag == false &&   $scope.deptName != "Software") {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $('#blahD')
                                .attr('src', e.target.result);
                        };
                        reader.readAsDataURL(input.files[0]);
                        UploadEmployeeDocumentOtherDetail(document);
                    }
                    else if ($scope.ismtcR_BugOREnhancementFlg == "E") {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $('#blahD')
                                .attr('src', e.target.result);
                        };
                        reader.readAsDataURL(input.files[0]);
                        UploadEmployeeDocumentOtherDetail(document);
                    }

                    else {
                        swal("First File Must Be  Excel Test Cases !");
                        return;
                    }
                }

            }
        };
        function UploadEmployeeDocumentOtherDetail(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetail.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetail[i]);
            }
            // We can send more data to server using append         
            //formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_ISMAttachment_new", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {

                        data.file = d[0].path;
                        data.FileName = d[0].name;

                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        //=============== view doc==========
        $scope.detailFrame = "";
        $scope.previewimg_new = function (img) {
            $scope.imagepreview = img;
            $scope.view_videos = [];
            var img = $scope.imagepreview;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myvideoPreview').modal('show');
            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {
                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');
            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                //$window.open($scope.imagepreview)
                $scope.detailFrame = $sce.trustAsResourceUrl(img);
                $('#myModaldocx').modal('show');
            }

            else if ($scope.filetype2 == 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');
            }
            else if ($scope.filetype2 == 'pdf') {
                ///=====================show pdf, img
                $('#showpdf').modal('hide');
                var imagedownload1 = "";
                imagedownload1 = $scope.imagepreview;
                $http.get(imagedownload1, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        var fileURL = "";
                        var file = "";
                        var embed = "";
                        var pdfId = "";
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        pdfId = document.getElementById("pdfIdzz");
                        pdfId.removeChild(pdfId.childNodes[0]);
                        embed = document.createElement('embed');
                        embed.setAttribute('src', fileURL);
                        embed.setAttribute('type', 'application/pdf');
                        embed.setAttribute('width', '100%');
                        embed.setAttribute('height', '1000');
                        pdfId.appendChild(embed);
                        $('#showpdf').modal('show');
                    });
            }
            else {
                $window.open($scope.imagepreview)
            }
        };
        //=========================================
        //materaldocuupload
        $scope.materaldocuupload = [];
        $scope.materaldocuupload = [{ id: 'materal' }];
        if ($scope.materaldocuupload != null && $scope.materaldocuupload.length > 0) {
            angular.forEach($scope.materaldocuupload, function (materal) {
                materal.plMaxdatem = new Date();
                materal.startDate = new Date();
                materal.endDate = new Date();
                materal.tteddP_level = 1;
            });
        }

        $scope.addgrnfile = function (materal) {
            if (materal.percentage > 0) {
                var newItemNo = $scope.materaldocuupload.length + 1;
                if (newItemNo <= 50) {
                    $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
                }
                var count = 0;
                angular.forEach($scope.materaldocuupload, function (materaltwo) {
                    count = (Number(count) + 1);
                    if (newItemNo == count) {
                        materaltwo.plMaxdatem = new Date(materal.endDate);
                        materaltwo.tteddP_level = count;
                        materaltwo.startDate = new Date(materal.endDate);
                        materaltwo.endDate = new Date(materal.endDate);
                    }
                });
            }
            else {
                swal("Please Enter  Percentage");
                return;
            }
        };
        //$scope.addgrnfile = function (materal) {
        //    if (materal.percentage > 0) {
        //        var newItemNo = $scope.materaldocuupload.length + 1;
        //        if (newItemNo <= 50) {
        //            $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
        //        }
        //        var count = 0;
        //        angular.forEach($scope.materaldocuupload, function (materaltwo) {
        //            count = (Number(count) + 1);
        //            if (newItemNo == count) {
        //                materaltwo.plMaxdatem = new Date(materal.endDate);
        //                materaltwo.tteddP_level = count;
        //                materal.tteddP_level = 1;
        //            }
        //        });
        //    }
        //    else {
        //        swal("Please Enter  Percentage");
        //        return;
        //    }
        //};
        $scope.removegrnfiles = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);
        };
        //ismtcR_Percentage
        $scope.validation = function (materaldocuupload) {
            if ($scope.obj.ismtcR_Days > 0) {
                var ismtcR_Percentage = 0;
                angular.forEach($scope.materaldocuupload, function (qq) {
                    ismtcR_Percentage = (Number(ismtcR_Percentage) + (Number(qq.percentage)));

                });

                if ((Number($scope.ismtcR_Percentage)) < (Number(ismtcR_Percentage))) {
                    swal("Total Percentage Not Greater Than 100 % !");
                    materaldocuupload.percentage = "";
                    return;
                }
                else {
                    var ismtcR_Days = 0;
                    ismtcR_Days = (Number($scope.obj.ismtcR_Days) * (Number(materaldocuupload.percentage)) / (Number(100)));
                    $scope.Math = window.Math;
                    materaldocuupload.tteddP_Days = $scope.Math.round(ismtcR_Days, 0);
                    $scope.obj.totalhours = ismtcR_Percentage;
                    return;
                }
            }
            else {
                swal("Please Enter  Total Days !");
                materaldocuupload.percentage = "";
                return;
            }
        }
        $scope.ClearEnhancement = function () {
            $scope.materaldocuupload = [];
            $scope.materaldocuupload = [{ id: 'materal' }];
            if ($scope.materaldocuupload != null && $scope.materaldocuupload.length > 0) {
                angular.forEach($scope.materaldocuupload, function (materal) {
                    materal.plMaxdatem = new Date();
                    materal.startDate = new Date();
                    materal.endDate = new Date();
                    materal.tteddP_level = 1;
                });
            }
        }
        $scope.EmployeeList = function (materal) {
            $scope.tempemp = [];
            if ($scope.EmplyeeEnhancements != null && $scope.EmplyeeEnhancements.length > 0) {
                angular.forEach($scope.EmplyeeEnhancements, function (Emplyee) {
                    if (Emplyee.hrmD_Id == materal.Deptid.hrmD_Id) {
                        $scope.tempemp.push(Emplyee);
                    }
                });
                materal.EmplyeeEnhancements = $scope.tempemp;
            }

        }
        //downloaddirectimage
        var imagedownload = "";
        $scope.downloaddirectimage = function (data, idd) {

            var studentreg = idd;

            $scope.imagedownload = data;
            imagedownload = data;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg
                    })[0].click();
                });
        };
        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };
        $scope.TotalHousrCalculate = function () {
            var totalhours = 0;
            if ($scope.materaldocuupload != null && $scope.materaldocuupload.length > 0) {
                angular.forEach($scope.materaldocuupload, function (materal) {
                    var dayDifference = 0;
                    var counthrs = 0.00;
                    counthrs = (parseFloat(materal.effmin) * 0.0166667).toFixed(2);
                    counthrs = parseFloat(materal.effhrs) + parseFloat(counthrs);
                    //----Corecct-----
                    var firstDate1 = $filter('date')(materal.startDate, "dd/MM/yy");
                    var  secondDate1 = $filter('date')(materal.endDate, "dd/MM/yy");

                    var date2 = new Date($scope.formatString(secondDate1));
                    var date1 = new Date($scope.formatString(firstDate1));

                   
                    var timeDiff = Math.abs(date2.getTime() - date1.getTime());
                     dayDifference = Math.ceil(timeDiff / (1000 * 3600 * 24));

                 

                    if (totalhours > 0) {
                        var days = 1;
                        if (dayDifference > 0) {
                            days = dayDifference;
                        }
                        totalhours = (Number(counthrs * days) + (Number(totalhours)));
                    }
                    else {
                        var days= 1;
                        if (dayDifference > 0) {
                            days = dayDifference;
                        }
                        totalhours = (counthrs * days);
                    }
                    
                    //totalhours
                });
                

                $scope.obj.ismtcR_Hours = totalhours;
              
            }
        }
        $scope.formatString = function (format) {
            if (format !== undefined && format !== null && format !== "") {
                var day = parseInt(format.substring(0, 2));
                var month = parseInt(format.substring(3, 5));
                var year = parseInt(format.substring(6, 10));
                var date = new Date(year, month - 1, day);
                return date;
            }
        };
    }
    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });
})();

