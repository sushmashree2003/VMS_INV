(function () {
    'use strict';
    angular.module('app').controller('ISM_DailyReportController', ISM_DailyReportController);
    ISM_DailyReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel','$timeout'];
    function ISM_DailyReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        $scope.submitted = false;

        $scope.maxdate = new Date();

        $scope.loaddata = function () {
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            $scope.dateflag = 0;
            var pageid = 2;
            apiService.getURI("ISM_ApprovalReport/getdetails", pageid).then(function (promise) {
                $scope.hide_n = false;
                if (promise.ivrmrT_Role === 'admin') {
                    $scope.depdes = true;
                    $scope.get_userEmplist = promise.get_userEmplist;
                }
                else {
                    $scope.depdes = false;
                    $scope.get_userEmplist = promise.get_userEmplist;
                }
            });
        };

        $scope.all_checkuu = function (ack) {
            $scope.checkalluu = ack;
            var toggleStatus = $scope.checkalluu;
            angular.forEach($scope.get_userEmplist, function (uem) {
                uem.empck = toggleStatus;
            });
            $scope.onEmployeechange();
        };

        $scope.togchkbxuu = function () {
            $scope.checkalluu = $scope.get_userEmplist.every(function (options) {
                return options.empck;
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.get_userEmplist.some(function (options) {
                return options.empck;
            });
        };

        $scope.approvalReport = function () {
            $scope.plannerid = [];
            if ($scope.myForm.$valid) {

                $scope.temp_Planner = [];
                $scope.searchValue = "";
                $scope.get_emp_dailyreport = "";

                $scope.arrayuserEmp = [];

                angular.forEach($scope.get_userEmplist, function (em) {
                    if (em.empck === true) {
                        $scope.arrayuserEmp.push(em);
                    }
                });

                var data = {
                    arrayuserEmp: $scope.arrayuserEmp,
                    "Fromdate": new Date($scope.start_Date).toDateString(),
                    "Todate": new Date($scope.end_Date).toDateString(),
                    "feedbackflg": true

                };
                apiService.create("ISM_ApprovalReport/getempdetailsReport", data).then(function (promise) {
                    $scope.plannerid = [];
                    $scope.hide_n = false;
                    $scope.feed = false;

                    if (promise.get_emp_dailyreport.length > 0) {
                        $scope.get_emp_dailyreport = promise.get_emp_dailyreport;
                        angular.forEach($scope.get_emp_dailyreport, function (dev) {
                            if ($scope.plannerid.length === 0) {
                                $scope.plannerid.push({ plannedby: dev.Employee_name, HRME_Id: dev.HRME_Id, MI_Id: dev.MI_Id });
                            }
                            else if ($scope.plannerid.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.plannerid, function (emp) {
                                    if (emp.HRME_Id === dev.HRME_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.plannerid.push({ plannedby: dev.Employee_name, HRME_Id: dev.HRME_Id, MI_Id: dev.MI_Id });
                                }
                            }
                        });

                        angular.forEach($scope.plannerid, function (ddd) {
                            $scope.templist = [];
                            angular.forEach($scope.get_emp_dailyreport, function (dd) {
                                if (dd.HRME_Id === ddd.HRME_Id) {

                                    var decimaltimetot = dd.ISMTCRASTO_EffortInHrs;

                                    var hrstot = parseInt(Number(decimaltimetot));
                                    var mintot = Math.round((Number(decimaltimetot) - hrstot) * 60);

                                    if (hrstot.toString().length === 1) {
                                        hrstot = '0' + hrstot;
                                    }

                                    if (mintot.toString().length === 1) {
                                        mintot = '0' + mintot;
                                    }

                                    dd.actualeffortinhrs = hrstot + ":" + mintot + " Hr";


                                    var decimaltimetaken = dd.ISMDRPT_TimeTakenInHrs;

                                    var hrstotime = parseInt(Number(decimaltimetaken));
                                    var mintotime = Math.round((Number(decimaltimetaken) - hrstotime) * 60);

                                    if (hrstotime.toString().length === 1) {
                                        hrstotime = '0' + hrstotime;
                                    }

                                    if (mintotime.toString().length === 1) {
                                        mintotime = '0' + mintotime;
                                    }

                                    dd.timetakeninhrs = hrstotime + ":" + mintotime + " Hr";



                                    $scope.templist.push(dd);
                                }
                            });
                            ddd.plannerdetails = $scope.templist;
                        });
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.feedbackflgclr = function () {
            if ($scope.feedbackflg === false) {
                $scope.start_Date = new Date();
            }
            else {
                $scope.start_Date = '';
                $scope.end_Date = '';
            }
        };

        $scope.Sendfeedback = function (aa) {
            $scope.feedbackdtl = [];
            $scope.plannerid1 = $scope.plannerid;
            angular.forEach($scope.plannerid, function (qq) {
                angular.forEach(qq.plannerdetails, function (ss) {
                    if (ss.feedbact !== undefined && ss.feedbact !== null && ss.feedbact !== "" && ss.FLAG === 0) {
                        $scope.feedbackdtl.push({
                            ISMDRF_RCV_HRME_Id: ss.HRME_Id, ISMTCR_Id: ss.ISMTCR_Id, ISMDRF_FeedBack: ss.feedbact, MI_Id_r: ss.MI_Id,
                            ISMDRF_Feedback_DR_Date: ss.ISMDRPT_Date
                        });
                    }
                });
            });

            if ($scope.feedbackdtl !== null && $scope.feedbackdtl.length > 0) {
                var data = {
                    feedbackdtl: $scope.feedbackdtl
                };

                apiService.create("ISM_ApprovalReport/send_feedback", data).then(function (fromise) {
                    if (fromise.send_flg === "Success") {
                        swal("Feedback Sent Successfully...");
                        $state.reload();
                        $scope.plnrtemp = [];
                        $scope.templist = [];
                        $scope.plnrtemp = $scope.plannerid;

                        angular.forEach($scope.plannerid, function (ddd) {
                            $scope.templist = [];
                            angular.forEach($scope.get_emp_dailyreport, function (dd) {
                                if (dd.HRME_Id === ddd.HRME_Id) {
                                    $scope.templist.push({
                                        Employee_name: dd.Employee_name, HRME_Id: dd.HRME_Id, ISMDRPT_Remarks: dd.ISMDRPT_Remarks,
                                        ISMDRPT_Status: dd.ISMDRPT_Status, ISMTCR_Id: dd.ISMTCR_Id, ISMTCR_TaskNo: dd.ISMTCR_TaskNo, ISMTCR_Title: dd.ISMTCR_Title,
                                        ISMTPL_Id: dd.ISMTPL_Id, feedbact: "", ISMDRPT_Date: dd.ISMDRPT_Date
                                    });
                                }
                            });
                            ddd.plannerdetails = $scope.templist;
                        });
                    }
                    else {
                        swal("Feedback Not sent successfully...");
                        $scope.feedbact = '';
                        $state.reload();
                    }
                });
            } else {
                swal("Enter Feedback");
            }
        };

        // ****** View The Task Description ****** //
        $scope.viewresponse = function (taskobj) {
            //$('#myModalResponse').modal('show');            
            $scope.ismtcrreS_Response = "";
            $scope.files_paths = "";
            $scope.ismmistS_StatusName = "";
            $scope.taskAttatchment = [];
            $scope.get_responseattachments = [];
            $scope.taskid = taskobj.ISMTCR_Id;
            $scope.created_by = taskobj.createdemp;
            var data = {
                "ISMTCR_Id": $scope.taskid
            };
            apiService.create("ISM_ApprovalReport/viewresponse", data).then(function (promise) {
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
                    if ($scope.get_responseattachments.length > 0) {
                        angular.forEach($scope.get_responseattachments, function (at) {
                            if (at.ISMTCRRES_ResponseAttachment !== null && at.ISMTCRRES_ResponseAttachment !== "") {
                                var img1 = at.ISMTCRRES_ResponseAttachment;
                                var imagarr1 = img1.split('.');
                                var lastelement1 = imagarr1[imagarr1.length - 1];

                                at.filetype = lastelement1;
                            }
                        });
                    }
                }
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clear = function () {
            $state.reload();
        };

        $scope.printData = function () {
            $scope.hide_n = false;
            var innerContents = "";
            innerContents = document.getElementById("printP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function () {
            //var blob = new Blob([document.getElementById('printP').innerHTML], {
            //    type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
            //});
            //saveAs(blob, "Approval Report.xls");


            var excelname = 'Employee Daily Report Details';
            excelname = excelname.toUpperCase() + '.xls';
            //var printSectionId = export_id;s
            var exportHref = Excel.tableToExcel('#excelid', 'FMS Employee Daily Report Details');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };
    }
})();