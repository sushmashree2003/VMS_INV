(function () {
    'use strict';
    angular.module('app').controller('Training_Cerificate_frFloorDRController', Training_Cerificate_frFloorDRController);
    Training_Cerificate_frFloorDRController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', 'Excel', '$timeout'];
    function Training_Cerificate_frFloorDRController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter, Excel, $timeout) {

        var paginationformasters = 10;
        $scope.obj = {};
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        $scope.StatusFlg = 'All';
        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();
        $scope.maxdate = new Date();

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("Appointment/getalldetails", pageid).then(function (promise) {

                $scope.get_userEmplist = promise.allEmp15dayDR;


                if ($scope.get_userEmplist.length > 0) {
                    angular.forEach($scope.get_userEmplist, function (uem) {
                        uem.empck = true;
                    });
                    $scope.checkall = true;
                }
            });
        };




        /* EMPLOYEE SELECTION */
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


        $scope.Deptselectiondetails = function () {
            $scope.arrayuserEmp = [];
            $scope.designationdropdown = [];
            $scope.get_userEmplist = [];
            angular.forEach($scope.departmentdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserEmp.push(em);
                }
            });
            if ($scope.arrayuserEmp.length > 0) {
                var data = {
                    "departmentlist": $scope.arrayuserEmp
                };
                apiService.create("ISM_PlannerDeptDeviationReport/Deptselectiondetails", data).then(function (promise) {
                    if (promise.designation !== null && promise.designation.length > 0) {
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
            }
        };

        $scope.Desgselectiondetails = function () {
            $scope.arrayuserEmp = [];
            $scope.get_userEmplist = [];
            angular.forEach($scope.designationdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserEmp.push(em);
                }
            });
            if ($scope.arrayuserEmp.length > 0) {
                var data = {
                    "designationlist": $scope.arrayuserEmp
                };
                apiService.create("ISM_PlannerDeptDeviationReport/Desgselectiondetails", data).then(function (promise) {
                    if (promise.get_userEmplist.length > 0) {
                        $scope.get_userEmplist = promise.get_userEmplist;
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
            }
        };

        /* DEVIATION REPORT */
        $scope.getReport = function () {
            if ($scope.myForm.$valid) {
                $scope.headreport = [];
                var data = "";
                $scope.get_report = [];
                $scope.deptlist = [];
                $scope.desglist = [];
                $scope.emplist = [];

                //angular.forEach($scope.departmentdropdown, function (d) {
                //    if (d.selected === true) {
                //        $scope.deptlist.push({ HRMDC_ID: d.HRMDC_ID });
                //    }
                //});

                //angular.forEach($scope.designationdropdown, function (d) {
                //    if (d.selected === true) {
                //        $scope.desglist.push({ HRMDES_Id: d.HRMDES_Id });
                //    }
                //});

                $scope.hrme_Ids = ""; 
                angular.forEach($scope.get_userEmplist, function (d) {
                    if (d.empck === true) {
                        $scope.hrme_Ids += d.HRME_Id + ",";
                    }
                    });

                if ($scope.hrme_Ids.endsWith(",")) {
                    $scope.hrme_Ids = $scope.hrme_Ids.slice(0, -1);
                }
                data = {
                    "Fromdate": new Date($scope.start_Date).toDateString(),
                    "Todate": new Date($scope.end_Date).toDateString(),
                    "reporttype": $scope.todayorothers,
                    "HRMED_Name": $scope.hrme_Ids,
                    "HRETF_Status": $scope.StatusFlg
                };

                apiService.create("Appointment/getReport", data).then(function (promise) {


                    $scope.get_report = promise.allReportTraningEMP;

                    console.log($scope.get_report)


                 

                 

                    
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.onchangestartdate = function () {
            $scope.end_Date = null;
            $scope.get_report = [];
            if ($scope.todayorothers !== 'consolidatereport') {
                $scope.getMaxDate = new Date(
                    $scope.start_Date.getFullYear(),
                    $scope.start_Date.getMonth(),
                    $scope.start_Date.getDate() + 30);
            }
        };

        $scope.onchangeenddate = function () {
            $scope.get_report = [];
        };

        $scope.otherdaydatechange = function () {
            //$scope.end_Date = null;
            //$scope.start_Date = null;
            $scope.StatusFlg = 'All';
            $scope.get_report = [];
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.OnChangeStatus = function (field) {
            $scope.get_report = [];
        };

        //===========Clear Field
        $scope.Clear = function () {
            $state.reload();
        };

        $scope.isOptionsRequired = function () {
            return !$scope.get_userEmplist.some(function (options) {
                return options.empck;
            });
        };

        $scope.isOptionsRequireddept = function () {
            return !$scope.departmentdropdown.some(function (options) {
                return options.selected;
            });
        };

        $scope.isOptionsRequireddesg = function () {
            return !$scope.designationdropdown.some(function (options) {
                return options.selected;
            });
        };

        $scope.obj.searchValueDeptd = '';
        $scope.filterValue4 = function (obj) {
            return angular.lowercase(obj.HRMDC_Name).indexOf(angular.lowercase($scope.obj.searchValueDeptd)) >= 0;
        };

        $scope.obj.searchValueDesgd = '';
        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.HRMDES_DesignationName).indexOf(angular.lowercase($scope.obj.searchValueDesgd)) >= 0 || angular.lowercase(obj.MI_Name).indexOf(angular.lowercase($scope.obj.searchValueDesgd)) >= 0;
        };

        $scope.obj.searchValueUEMd = '';
        $scope.filterValue2 = function (obj) {
            return angular.lowercase(obj.userEmpName).indexOf(angular.lowercase($scope.obj.searchValueUEMd)) >= 0 || angular.lowercase(obj.HRMD_DepartmentName).indexOf(angular.lowercase($scope.obj.searchValueUEMd)) >= 0;
        };

        $scope.exportToExcel = function (export_id) {
            $scope.stdate = $filter('date')($scope.start_Date, 'dd-MM-yyyy');
            $scope.end = $filter('date')($scope.end_Date, 'dd-MM-yyyy');
            var excelname = "";
            var exportHref = "";
            var excelsheetname = "";
            var reportname = "";
            if ($scope.todayorothers === "detailedreport") {
                reportname = "#detailedreport";
                excelsheetname = "CONSOLIDATED";
            } else if ($scope.todayorothers === "MultiLevelDRApproval") {
                reportname = "#excelmultidrapprovaldetails";
                excelsheetname = "DR MULTI APPROVAL DETAILS";
            } else if ($scope.todayorothers === "DailyReportBlockedCountDetails") {
                reportname = "#exceldrblockedcountdetails";
                excelsheetname = "DR BLOCKED COUNT DETAILS";
            } else {
                reportname = "#commentsexcelreport";
                excelsheetname = "COMMENTS";
            }

            excelname = excelsheetname + ' REPORT ' + $scope.stdate + ' TO ' + $scope.end;
            excelname = excelname.toUpperCase() + '.xls';
            exportHref = Excel.tableToExcel(reportname, excelsheetname);
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.printData = function () {
            var innerContents = "";
            var name = "";
            if ($scope.todayorothers === "detailedreport") {
                name = "detailedreportprint";
            } else if ($scope.todayorothers === "MultiLevelDRApproval") {
                name = "printmultidrapprovaldetails";
            } else if ($scope.todayorothers === "DailyReportBlockedCountDetails") {
                name = "printdrblockedcountdetails";
            } else {
                name = "commentsreportprint";
            }

            innerContents = document.getElementById(name).innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.ViewCheckListSavedDetails = function (objs) {

            var data = {
                "ISMDRPT_Id": objs.ISMDRPT_Id
            };

            apiService.create("ISM_ConsolidateDailyReport/ViewCheckListSavedDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getchecklistarray = promise.getchecklistarray;
                    if ($scope.getchecklistarray === null || $scope.getchecklistarray.length === 0) {
                        swal("No Records Found");
                    } else {
                        angular.forEach($scope.getchecklistarray, function (dd) {
                            if (dd.ismdrptfL_FilePath !== null && dd.ismdrptfL_FilePath !== "") {
                                var img = dd.ismdrptfL_FilePath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("dd.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
                                }
                            }
                        });
                        $('#viewfiles').modal('show');
                    }
                }
            });
        };

        $scope.viewresponse = function (taskobj) {
            $scope.temptaskreponsefiles = [];
            $scope.temptaskreponsefiles.push({ 'id': 'Teacher1' });
            $scope.ismtcrreS_Response = "";
            $scope.files_paths = "";
            $scope.ismmistS_StatusName = "";
            $scope.taskAttatchment = [];
            $scope.get_responseattachments = [];
            $scope.taskid = taskobj.ISMTCR_Id;
            $scope.plannertaskid = taskobj.ISMMTCAT_Id;
            $scope.created_by = taskobj.createdemp;
            $scope.files = [];
            var data = {
                "ISMTCR_Id": taskobj.ISMTCR_Id
            };
            apiService.create("ISM_ConsolidateDailyReport/viewresponse", data).then(function (promise) {
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
                    $scope.taskAttatchment = promise.get_taskattachments;
                    $scope.get_responseattachments = promise.get_responseattachments;

                    if ($scope.taskAttatchment.length > 0) {
                        angular.forEach($scope.taskAttatchment, function (at) {
                            if (at.ISMTCRAT_Attatchment !== null && at.ISMTCRAT_Attatchment !== "") {
                                var img = at.ISMTCRAT_Attatchment;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                at.filetype = lastelement;
                            }
                        });
                    }
                    if ($scope.get_responseattachments !== null && $scope.get_responseattachments.length > 0) {
                        angular.forEach($scope.get_responseattachments, function (at) {
                            if (at.ISMTCRRES_ResponseAttachment !== null && at.ISMTCRRES_ResponseAttachment !== "") {
                                var img1 = at.ISMTCRRES_ResponseAttachment;
                                var imagarr1 = img1.split('.');
                                var lastelement1 = imagarr1[imagarr1.length - 1];

                                at.filetype = lastelement1;
                            }
                        });
                    }

                    angular.forEach($scope.get_plannerlist, function (dd) {
                        if (dd.ISMTPLTA_Id === taskobj.ISMTPLTA_Id) {
                            $scope.taskstartdate1 = taskobj.ISMTPLTA_StartDate;
                            $scope.taskenddate1 = taskobj.ISMTPLTA_EndDate;
                        }
                    });

                    angular.forEach($scope.get_plannerlist, function (obj) {
                        if ($scope.taskid === obj.ISMTCR_Id && $scope.plannertaskid == obj.ISMMTCAT_Id) {

                            if (obj.Temp_TaskReponseList !== undefined && obj.Temp_TaskReponseList !== null && obj.Temp_TaskReponseList.length > 0) {
                                $scope.temptaskreponsefiles = obj.Temp_TaskReponseList;
                                angular.forEach($scope.temptaskreponsefiles, function (dd) {
                                    var img = dd.filepath;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];
                                    dd.filetype = lastelement;
                                    console.log("dd.filetype : " + dd.filetype);
                                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                        dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
                                    }
                                });
                            }
                        }
                    });
                }
            });
        };

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalPreview').modal('show');
        };

        $scope.previewimg = function (img) {
            $scope.imagepreview = img;
            $('#preview').attr('src', $scope.imagepreview);
            $('#myModalPreview').modal('show');
            $('.modal').on('hide.bs.modal', function (e) {
                e.stopPropagation();
                $('body').css('padding-right', '');
            });
        };

        // ****** Close Image Model ********** //
        $scope.closeimg = function () {
            $('#myModalPreview').modal('hide');
            $('.modal-backdrop').remove();
        };

        var imagedownload = "";
        var docname = "";
        var studentreg = "";

        $scope.onviewpdf = function (filepath, filename) {
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            document.getElementById("pdfviewdd").innerHTML = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var htmlElements = '<embed id="pdfview" src="' + $scope.content + '"  style="width: 100%;" height="1000" type="application/pdf" />';
                    document.getElementById("pdfviewdd").innerHTML = htmlElements;
                    $('#showpdf').modal('show');
                });
        };

        $scope.downloaddirectimage = function (data, idd) {
            var studentreg = idd;
            $scope.imagedownload = data;
            imagedownload = data;
            $http.get(imagedownload, { responseType: "arraybuffer" }).success(function (data) {
                var anchor = angular.element('<a/>');
                var blob = new Blob([data]);
                anchor.attr({
                    href: window.URL.createObjectURL(blob),
                    target: '_blank',
                    download: studentreg
                })[0].click();
            });
        };
    }
})();

