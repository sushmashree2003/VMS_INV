(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_ImplTaskMappingController', ISM_ImplTaskMappingController);
    ISM_ImplTaskMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function ISM_ImplTaskMappingController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache) {

        //var paginationformasters;
        //var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings.length > 0) {
        //    paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        //}
        //else {
        //    paginationformasters = 0;
        //}
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        $scope.obj = {};
        $scope.responsetrans = 0;
        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.typeFlg = "Default";
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();
        $scope.ismtcrreS_ResponseDate = new Date();
        $scope.currentDate = new Date();

        var options = [];

        $('.dropdown-menu a').on('click', function (event) {

            var $target = $(event.currentTarget),
                val = $target.attr('data-value'),
                $inp = $target.find('input'),
                idx;

            if ((idx = options.indexOf(val)) > -1) {
                options.splice(idx, 1);
                setTimeout(function () { $inp.prop('checked', false) }, 0);
            } else {
                options.push(val);
                setTimeout(function () { $inp.prop('checked', true) }, 0);
            }

            $(event.target).blur();

            console.log(options);
            return false;
        });

        $scope.loaddata = function () {
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage = 1;
            $scope.get_comptasklist = [];
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;
            var data = {
                "typeFlg": $scope.typeFlg
            };
            apiService.create("ISM_ImplTaskMapping/getdetails", data).then(function (promise) {

                $scope.get_employeelist = promise.get_employeelist;

            
             
                $scope.implplannerlist = promise.implplannerlist;
               
              
            });
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;
                $scope.reverse = !$scope.reverse;
            };
        };

        $scope.all_task = function (atk) {
            $scope.alltask = atk;
            var toggleStatus = $scope.alltask;
            angular.forEach($scope.get_comptasklist, function (atck) {
                if (atck.tasktag === 1) {
                    atck.checkedvalue = toggleStatus;
                }
            });
        };
        $scope.togchkbxtask = function () {
            $scope.alltask = $scope.get_comptasklist.every(function (options) {
                if (options.assignedflg === false) {
                    return options.checkedvalue;
                }

            });
        };
        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.get_userEmplist, function (uem) {
                uem.empck = toggleStatus;
            });
        };
        $scope.togchkbx = function () {
            $scope.checkall = $scope.get_userEmplist.every(function (options) {
                return options.empck;
            });
        };
        $scope.all_checkS = function (ack) {
            $scope.checkallS = ack;
            var toggleStatus = $scope.checkallS;
            angular.forEach($scope.get_Status, function (sts) {
                sts.stck = toggleStatus;
            });
        };
        $scope.togchkbxS = function () {
            $scope.checkallS = $scope.get_Status.every(function (options) {
                return options.stck;
            });
        };
        $scope.HRME_Id = "";
        $scope.ismtcR_BugOREnhancementFlg = 'B'

        $scope.changedd = function (ddd) {

            if (ddd.ISMIMPPL_Periodicity == 'Weekly') {
                if (ddd.ISMTAPL_Day != undefined && ddd.ISMTAPL_Day != '' && ddd.ISMTAPL_Day != null) {
                    if (parseInt(ddd.ISMTAPL_Day)>6) {
                        ddd.ISMTAPL_Day = '';
                    }
                }
            }

            if (ddd.ISMIMPPL_Periodicity == 'Monthly' || ddd.ISMIMPPL_Periodicity == 'Once in Fortnight') {
                if (ddd.ISMTAPL_Day != undefined && ddd.ISMTAPL_Day != '' && ddd.ISMTAPL_Day != null) {
                    if (parseInt(ddd.ISMTAPL_Day) > 30) {
                        ddd.ISMTAPL_Day = '';
                    }
                }
            }

        }
        $scope.get_comptasklis = [];
        //======================================== Search List
        $scope.searchtasks = function () {
            $scope.get_comptasklis = [];
            if ($scope.myForm.$valid) {

                debugger;
                var data = {
                    "ISMMIMPPL_Id": $scope.ISMMIMPPL_Id,
                    "HRME_Id": $scope.HRME_Id.HRME_Id,
                };
                apiService.create("ISM_ImplTaskMapping/getsearchtask", data).
                    then(function (promise) {
                        $scope.get_comptasklist = promise.get_comptasklist;
                        $scope.clientlist = promise.clientlist;
                        $scope.categorylist = promise.categorylist;
                        $scope.projectlist = promise.projectlist;
                        $scope.get_priority = promise.get_priority;
                        $scope.modulelist = promise.modulelist;
                        debugger;
                        if ($scope.get_comptasklist.length > 0) {

                            anglar.forEach($scope.get_comptasklist, function (ee) {
                                ee.ismtcR_BugOREnhancementFlg = 'B';


                            })

                        }
                        else {
                            swal('No Task Found');
                        }
                    });
            }
            else {
                $scope.submitted1 = true;
            }
        };

        //======================================== Total Task & ASSIGNED & NOT-ASSIGNED  COUNTS 
        $scope.gettotalcount = function () {
            $scope.typeFlg = "Search";
            $scope.arrayuserEmp = [];
            $scope.arrayStatus = [];
            $scope.get_comptasklist = [];
            angular.forEach($scope.get_userEmplist, function (em) {
                if (em.empck === true) {
                    $scope.arrayuserEmp.push(em);
                }
            });
            angular.forEach($scope.get_Status, function (sts) {
                $scope.arrayStatus.push(sts);
            });


            var data = {
                "typeFlg": $scope.typeFlg,
                "arrayuserEmp": $scope.arrayuserEmp,
                "arrayStatus": $scope.arrayStatus
            };
            apiService.create("ISM_ImplTaskMapping/getsearchtask", data).
                then(function (promise) {
                    $scope.get_comptasklist = promise.get_comptasklist;
                    if ($scope.get_comptasklist.length > 0) {
                        var notassignedcount = 0;
                        var assignedcount = 0;
                        var pendingtaskcount = 0;
                        var transfertaskcount = 0;
                        angular.forEach($scope.get_comptasklist, function (clt) {
                            if (clt.ISMTCR_Status === 'Open') {
                                pendingtaskcount += 1;
                            }
                            if (clt.tasktag === 2) {
                                transfertaskcount += 1;
                            }
                            if (clt.addtoplannerflag === 1) {
                                notassignedcount += 1;
                            }
                            else {
                                assignedcount += 1;
                            }
                        });
                        $scope.assignedcount = assignedcount;
                        $scope.notassignedcount = notassignedcount;
                        $scope.pendingcount = pendingtaskcount;
                        $scope.transfercount = transfertaskcount;
                    }
                    else {
                        $scope.assignedcount = 0;
                        $scope.notassignedcount = 0;
                        $scope.pendingcount = 0;
                        $scope.transfercount = 0;
                    }
                });
        };
        $scope.getpendingcount = function () {
            $scope.typeFlg = "Search";
            var statusName = "Open";
            $scope.arrayuserEmp = [];
            $scope.arrayStatus = [];
            $scope.get_comptasklist = [];
            angular.forEach($scope.get_userEmplist, function (em) {
                if (em.empck === true) {
                    $scope.arrayuserEmp.push(em);
                }
            });
            $scope.arrayStatus.push({ ISMMISTS_StatusName: statusName });
            var data = {
                "typeFlg": $scope.typeFlg,
                "arrayuserEmp": $scope.arrayuserEmp,
                "arrayStatus": $scope.arrayStatus
            };
            apiService.create("ISM_ImplTaskMapping/getsearchtask", data).
                then(function (promise) {
                    $scope.get_comptasklist = promise.get_comptasklist;
                    if ($scope.get_comptasklist.length > 0) {
                        var notassignedcount = 0;
                        var assignedcount = 0;
                        var pendingtaskcount = 0;
                        var transfertaskcount = 0;
                        angular.forEach($scope.get_comptasklist, function (clt) {
                            if (clt.ISMTCR_Status === 'Open') {
                                pendingtaskcount += 1;
                            }
                            if (clt.tasktag === 2) {
                                transfertaskcount += 1;
                            }
                            if (clt.addtoplannerflag === 1) {
                                notassignedcount += 1;
                            }
                            else {
                                assignedcount += 1;
                            }
                        });
                        $scope.assignedcount = assignedcount;
                        $scope.notassignedcount = notassignedcount;
                        $scope.pendingcount = pendingtaskcount;
                        $scope.transfercount = transfertaskcount;
                    }
                    else {
                        $scope.assignedcount = 0;
                        $scope.notassignedcount = 0;
                        $scope.pendingcount = 0;
                        $scope.transfercount = 0;
                    }
                });
        };

      

        //======================================== PENDING , BY ME & To ME Count 
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
            apiService.create("ISM_ImplTaskMapping/getcount", data).
                then(function (promise) {
                    if (promise.get_tasks.length > 0) {
                        $scope.get_tasks = promise.get_tasks;
                        $scope.get_comptasklist = $scope.get_tasks;
                    }
                    else {
                        $scope.get_tasks = "";
                    }
                });
        };


        $scope.onDeptchange = function (objd) {

            $scope.get_employeelist = [];

            $scope.deptid = objd.HRMD_Id.HRMD_Id;
            var data = {
                "HRMD_Id": $scope.deptid,
                "MIId": objd.HRMD_Id.MI_Id
            };
            apiService.create("ISM_TaskRegisterReport/getemployeelist", data).
                then(function (promise) {
                    if (promise.get_employeelist.length > 0) {
                        $scope.get_employeelist = promise.get_employeelist;
                    }
                    else {
                        $scope.get_employeelist = "";
                        swal("No Record Found ....!!");
                    }

                });
        };
        //========================================= Planner




        //shilpa
        $scope.deptfilter = function (selected, arrlistchk) {

            $scope.albumNameArray = [];
            for (var j = 0; j < selected.length; j++) {
                for (var i = 0; i < arrlistchk.length; i++) {
                    if (selected[j] == arrlistchk[i].hrmdC_ID) {
                        $scope.albumNameArray.push(arrlistchk[i]);
                    }
                }
            }

            var data = {
                "savegroup": $scope.albumNameArray
            }
            apiService.create("ISM_ImplTaskMapping/deptfilter", data).then(function (promise) {

                $scope.taskplanner = [];
                $scope.get_employeelist = promise.get_employeelist;
                if ($scope.get_comptasklist.length > 0) {
                    angular.forEach($scope.get_comptasklist, function (objct) {
                        if (objct.checkedvalue === true) {
                            $scope.empll = [];
                            angular.forEach($scope.get_employeelist, function (ll) {
                                $scope.empll.push({
                                    id: objct.ISMTCR_Id + ll.HRME_Id, HRME_Id: ll.HRME_Id, employeeName:
                                        ll.employeeName, hrmD_DepartmentName: ll.HRMD_DepartmentName
                                });
                            });
                            objct.emplist = $scope.empll;
                            objct.taskDate = new Date(objct.ISMTCR_CreationDate);
                            $scope.taskplanner.push(objct);
                        }
                    });
                }
            })

        }


        $scope.addtoplanner = function (taskobj) {

            $scope.arrlistchk = $scope.arrlistchk1;
            $scope.taskplanner = [];
            if ($scope.get_comptasklist.length > 0) {
                angular.forEach($scope.get_comptasklist, function (objct) {
                    if (objct.checkedvalue === true) {
                        $scope.empll = [];
                        angular.forEach($scope.get_employeelist, function (ll) {
                            $scope.empll.push({ id: objct.ISMTCR_Id + ll.HRME_Id, HRME_Id: ll.HRME_Id, employeeName: ll.employeeName, hrmD_DepartmentName: ll.HRMD_DepartmentName });
                        });
                        objct.emplist = $scope.empll;
                        objct.taskDate = new Date(objct.ISMTCR_CreationDate);
                        $scope.taskplanner.push(objct);
                    }
                });
            }
            if ($scope.taskplanner.length > 0) {
                console.log($scope.taskplanner);
                //$('#myModalAddtoPlanner').modal('show');
            }
            else {
                swal("Please Select Any Task ..... !!");
            }
        };

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.employeeName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };
        $scope.savePlanner = function () {
            debugger;
            if ($scope.myForm.$valid) {
                $scope.plannerArray = [];
                $scope.taskEmpArray = [];
                var totcheck = 0;
                angular.forEach($scope.get_comptasklist, function (tp) {
                    if (tp.checkedvalue == true) {
                        $scope.plannerArray.push(tp);
                       
                    }
                });


                angular.forEach($scope.plannerArray, function (tt) {

                    tt.ismtcrastO_StartDate = new Date(tt.ismtcrastO_StartDate).toDateString();
                    tt.ismtcrastO_EndDate = new Date(tt.ismtcrastO_EndDate).toDateString();
                    if (tt.ISMTAPL_OFFDate != undefined && tt.ISMTAPL_OFFDate != null && tt.ISMTAPL_OFFDate != '') {
                        tt.ISMTAPL_OFFDate = new Date(tt.ISMTAPL_OFFDate).toDateString();
                    }


                }) 



              

                if ($scope.plannerArray.length > 0) {
                    var data = {
                        "HRME_Id": $scope.HRME_Id.HRME_Id,
                        "implplannerarr": $scope.plannerArray,
                    };
                    
                    apiService.create("ISM_ImplTaskMapping/savePlanner", data).then(function (promise) {
                        if (promise.returnval === true) {
                            
                                swal('Task Created Successfully!!!!');
                                $state.reload();
                         
                            
                        }
                        
                    });
                }
                else {
                    swal("Select Atleast one Planner from list.....!!");
                }
               
            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        //======================================== Transfer
        $scope.transfertask = function () {
            $scope.tasktransfer = [];
            if ($scope.get_comptasklist.length > 0) {
                angular.forEach($scope.get_comptasklist, function (objct) {
                    if (objct.checkedvalue === true) {
                        objct.taskcreatedate = new Date(objct.ISMTCR_CreationDate);
                        $scope.tasktransfer.push(objct);
                    }
                });
            }
            if ($scope.tasktransfer.length > 0) {
                //$('#myModalTransfer').modal('show');
            }
            else {
                swal("Please Select Any Task ..... !!");
            }
        };

        $scope.responsetransfer = function () {
            $('#myModalResponse').modal('hide');
            $('.modal-backdrop').remove();
            $scope.tasktransfer = [];
            if ($scope.get_taskdetails.length > 0) {
                $scope.tasktransfer.push({
                    ISMTCR_Id: $scope.get_taskdetails[0].ISMTCR_Id,
                    ISMTCR_Title: $scope.get_taskdetails[0].ISMTCR_Title,
                    ISMTCR_TaskNo: $scope.get_taskdetails[0].ISMTCR_TaskNo,
                    ISMMCLT_ClientName: $scope.get_taskdetails[0].ISMMCLT_ClientName,
                    ISMTCR_CreationDate: $scope.get_taskdetails[0].ISMTCR_CreationDate,
                    ISMTCR_Status: $scope.get_taskdetails[0].ISMTCR_Status
                });
            }
        };

        $scope.saveTransfer = function () {
            if ($scope.myForm.$valid) {
                $scope.transferArray = [];
                $scope.hrme_list = [];
                angular.forEach($scope.tasktransfer, function (tt) {
                    if (tt.ismtcrtrtO_StartDate === null || tt.ismtcrtrtO_StartDate === undefined || tt.ismtcrtrtO_StartDate === "not available") {
                        tt.ismtcrtrtO_StartDate = new Date();
                        tt.ismtcrtrtO_EndDate = new Date();
                    }
                    $scope.transferArray.push({ ISMTCR_Id: tt.ISMTCR_Id, ismtcrtrtO_StartDate: new Date(tt.ismtcrtrtO_StartDate).toDateString(), ismtcrtrtO_EndDate: new Date(tt.ismtcrtrtO_EndDate).toDateString(), ismtcrtrtO_EffortInHrs: tt.ismtcrtrtO_EffortInHrs,  ismtcrtrtO_Remarks: tt.ismtcrtrtO_Remarks, ismtcrtrtO_Id: tt.IsmtcrtrtO_Id });
                });

                angular.forEach($scope.get_employeelist, function (tt) {
                    if (tt.checked == true)
                        $scope.hrme_list.push({ HRME_Id: tt.HRME_Id });
                });
                    
               


                var data = {
                    "ISMTCRTRTO_TransferredDate": new Date($scope.ismtcrtrtO_TransferredDate).toDateString(),
                    "transferArray": $scope.transferArray,
                    "hrme_list": $scope.hrme_list,
                };
                apiService.create("ISM_ImplTaskMapping/saveTransfer", data).then(function (promise) {
                    if (promise.returnval === true) {
                        if (promise.ismtcrtrtO_Id === 0 || promise.ismtcrtrtO_Id < 0) {
                            swal('Task Transferred successfully ... !!');
                            $state.reload();
                        }
                        else if (promise.ismtcrtrtO_Id > 0) {
                            swal('Record updated successfully');
                            $state.reload();
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.ismtcrtrtO_Id === 0 || promise.ismtcrtrtO_Id < 0) {
                            swal('Failed to Transfer, please contact administrator');
                        }
                        else if (promise.ismtcrtrtO_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }

                });
            }
            else {
                $scope.submitted = true;
            }
        };

        //======================================== Response
        $scope.viewresponse = function (taskobj) {
            //$('#myModalResponse').modal('show');            
            $scope.ismtcrreS_Response = "";
            $scope.files_paths = "";
            $scope.ismmistS_StatusName = "";
            $scope.taskAttatchment = [];
            $scope.taskid = taskobj.ISMTCR_Id;
            $scope.created_by = taskobj.createdby;
            var data = {
                "ISMTCR_Id": $scope.taskid
            };
            apiService.create("ISM_ImplTaskMapping/gettaskdetails", data).
                then(function (promise) {
                    $scope.get_taskdetails = promise.get_taskdetails;
                    $scope.get_taskresponse = promise.get_taskresponse;
                    if ($scope.get_taskdetails !== null && $scope.get_taskdetails.length > 0) {
                        $scope.tasktitle = $scope.get_taskdetails[0].ISMTCR_Title;
                        $scope.taskno = $scope.get_taskdetails[0].ISMTCR_TaskNo;
                        $scope.taskclient = $scope.get_taskdetails[0].ISMMCLT_ClientName;
                        $scope.taskdate = $scope.get_taskdetails[0].ISMTCR_CreationDate;
                        $scope.tasktype = $scope.get_taskdetails[0].tasktype;
                        $scope.tsakpriority = $scope.get_taskdetails[0].HRMP_Name;
                        $scope.taskdescription = $scope.get_taskdetails[0].ISMTCR_Desc;
                        $scope.taskstatus = $scope.get_taskdetails[0].ISMTCR_Status;
                        $scope.createdby = $scope.get_taskdetails[0].createdby;
                        $scope.startdate = new Date($scope.get_taskdetails[0].ISMTCRASTO_StartDate).toDateString();
                        $scope.enddate = new Date($scope.get_taskdetails[0].ISMTCRASTO_EndDate).toDateString();

                        angular.forEach($scope.get_taskresponse, function (at) {
                            if (at.ISMTCRAT_Attatchment !== "" && at.ISMTCRAT_Attatchment !== null && at.ISMTCRAT_Attatchment !== undefined) {
                                $scope.taskAttatchment.push({ ISMTCRAT_Attatchment: at.ISMTCRAT_Attatchment });
                            }
                            else {
                                $scope.taskAttatchment.length = 0;
                            }
                        });
                    }
                });
        };

        //=================== Create
        $scope.responsetask = function () {
            if ($scope.files !== null && $scope.files !== undefined && $scope.files !== "") {
                UploadFiles();
            }
            else {
                $scope.saveResponse();
            }
        };
        //=========== View Response Image
        $scope.previewimg = function (img) {
            $scope.imagepreview = img;
            $('#preview').attr('src', $scope.imagepreview);
            $('#myModalResponse').modal('hide');
            $('.modal-backdrop').remove();
            $('#myModalPreview').modal('show');
        };
        //=========== Close Image Model
        $scope.closeimg = function () {
            $('#myModalPreview').modal('hide');
            $('.modal-backdrop').remove();

        };

        //shilpa
        $scope.clearids = function () {
            // $scope.HRMD_Id = '';
            $scope.selected = "";
            $scope.arrlistchk = [];
            $scope.label = '';
            // $scope.arrlistchk = [];
        }
        //=================== Upload
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
        //============= Remove Selected File 
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
                        $scope.saveResponse();
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.saveResponse = function () {
            if ($scope.myFormModel.$valid) {
                if ($scope.files_paths !== null && $scope.files_paths !== undefined && $scope.files_paths !== "") {
                    $scope.files_paths;
                }
                else {
                    $scope.files_paths = "";
                }
                var data = {
                    "ISMTCR_Id": $scope.taskid,
                    "ISMTCRRES_ResponseDate": new Date($scope.ismtcrreS_ResponseDate).toDateString(),
                    "ISMTCRRES_Response": $scope.ismtcrreS_Response,
                    "ISMMISTS_StatusName": $scope.ismmistS_StatusName,
                    attachmentArray: $scope.files_paths
                };
                apiService.create("ISM_ImplTaskMapping/saveResponse", data).then(function (promise) {
                    if (promise.returnval === true) {
                        if (promise.ismtcrreS_Id === 0 || promise.ismtcrreS_Id < 0) {
                            $('#myModalResponse').modal('hide');
                            $('.modal-backdrop').remove();
                            swal('Record saved successfully ... !!');
                            $state.reload();
                        }
                        else if (promise.ismtcrreS_Id > 0) {
                            $('#myModalResponse').modal('hide');
                            $('.modal-backdrop').remove();
                            swal('Record updated successfully');
                            $state.reload();
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.ismtcrreS_Id === 0 || promise.ismtcrreS_Id < 0) {
                            swal('Failed to Update, please contact administrator');
                        }
                        else if (promise.ismtcrreS_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                });
            }
            else {
                $scope.submitted1 = true;
                //$('#myModalResponse').modal('show');
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
        //===========Clear Field
        $scope.cancel = function () {
            $state.reload();
        };

    }
})();

