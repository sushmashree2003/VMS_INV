(function () {
    'use strict';
    angular.module('app').controller('ISM_DailyReport_ApprovalMdController', ISM_DailyReport_ApprovalController);
    ISM_DailyReport_ApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', 'Excel', '$timeout', '$sce'];
    function ISM_DailyReport_ApprovalController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter, Excel, $timeout, $sce) {


        var paginationformasters = 10;

        $scope.obj = {};
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};

        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();

        $scope.getserverdate = function () {
            var xmlHttp;
            function srvTime() {
                try {

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
        $scope.newdate = new Date($scope.today);

        $scope.maxdate = new Date($scope.today);
        /* Load Data */
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            var data = {
                "DRApprovalTypeFlag": "DRApproval"
            }
            apiService.create("ISM_DailyReport_Approval/GetDetails", data).then(function (promise) {

                $scope.get_userEmplist = promise.get_userEmplist;
                $scope.departmentdropdown = promise.departmentList;
                $scope.designationdropdown = promise.designation;
                $scope.empdepartment = promise.empdepartment;
                $scope.deviationpercentage = promise.deviationpercentage;
                $scope.empname = promise.empname;
                $scope.roleflag = promise.roleflag;

                if ($scope.departmentdropdown.length > 0) {
                    angular.forEach($scope.departmentdropdown, function (uem) {
                        uem.selected = true;
                    });
                    $scope.departmentselectedAll = true;
                }
                if ($scope.designationdropdown.length > 0) {
                    angular.forEach($scope.designationdropdown, function (uem) {
                        uem.selected = true;
                    });
                    $scope.designationselectedAll = true;
                }
                if ($scope.get_userEmplist.length > 0) {
                    angular.forEach($scope.get_userEmplist, function (uem) {
                        uem.empck = true;
                    });
                    $scope.checkall = true;
                }

                if (promise.get_leaveDetails != null && promise.get_leaveDetails.length > 0 || promise.compoffdetails != null && promise.compoffdetails.length > 0) {
                    $scope.get_leaveDetails = promise.get_leaveDetails;
                    $scope.compoffdetails = promise.compoffdetails;
                    $('#LeaveApproval').modal('show');
                }
                else {
                    $('#LeaveApproval').modal('hide');
                }

                if (promise.cerfificatelist != null && promise.cerfificatelist.length > 0) {
                    $scope.Cerfificatelist = promise.cerfificatelist;
                    $('#certificateApproval').modal('show');
                }
                else {
                    $('#certificateApproval').modal('hide');
                }

                if (promise.compoffdetails != null && promise.compoffdetails.length > 0) {
                    $scope.compoffdetails = promise.compoffdetails;
                    $('#CompoffApproval').modal('show');
                }
                else {
                    $('#CompoffApproval').modal('hide');
                }

                if (promise.NOCdetails != null && promise.NOCdetails.length > 0) {
                    $scope.NOCdetails = promise.NOCdetails;
                    $('#nocApproval').modal('show');
                }
                else {
                    $('#nocApproval').modal('hide');
                }
                if (promise.get_unratedemployeelist != null && promise.get_unratedemployeelist.length > 0) {
                    $scope.get_unratedemployeelist = promise.get_unratedemployeelist;
                    $('#employeerating').modal('show');
                }
                else {
                    $('#employeerating').modal('hide');
                }

                $scope.Monthlist = promise.monthlist;
                $scope.Yearlist = promise.yearlist;

                $scope.previousdate = new Date(promise.previousdate);
                $scope.start_Date = new Date(promise.previousdate);
                $scope.end_Date = new Date(promise.previousdate);


                $timeout(function () { $scope.GetSavedDetails(); }, 1000);
                //added By sanjeev
                $scope.AdavanceList = promise.adavanceList;
                $scope.ApplyList = promise.applyList;
                if ($scope.AdavanceList != null && $scope.AdavanceList.length > 0 || $scope.ApplyList != null && $scope.ApplyList.length > 0) {
                    $('#TadaReport').modal('show');
                }
                else {
                    $('#TadaReport').modal('hide');
                }

            });
        };

        /* DEPARTMENT SELECTION */
        $scope.all_checkdept = function (departmentselectedAll) {
            var toggleStatus = departmentselectedAll;
            angular.forEach($scope.filterValue5, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.searchValueDesg = '';
            $scope.designationselectedAll = false;
            $scope.DeptSelectionDetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };

        $scope.togchkbxdept = function (groupType) {
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {
                return itm.selected;
            });
            $scope.searchValueDesg = '';
            $scope.designationselectedAll = false;
            $scope.DeptSelectionDetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.searchValueUEM = '';
            $scope.employeeid = [];
            $scope.checkall = false;
            $scope.get_deviationreport = [];
        };

        $scope.DeptSelectionDetails = function () {
            $scope.arrayuserEmp = [];
            $scope.designationdropdown = [];
            $scope.get_userEmplist = [];
            angular.forEach($scope.departmentdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserEmp.push(em);
                }
            });
            var data = {
                "DRApprovalTypeFlag": "DRApproval",
                "departmentlist": $scope.arrayuserEmp
            };
            apiService.create("ISM_DailyReport_Approval/DeptSelectionDetails", data).then(function (promise) {
                if (promise.designation !== null && promise.designation.length > 0) {
                    $scope.designationdropdown = promise.designation;
                    if ($scope.designationdropdown.length > 0) {
                        angular.forEach($scope.designationdropdown, function (uem) {
                            uem.selected = true;
                        });
                        $scope.designationselectedAll = true;
                    }
                    $scope.DesgSelectionDetails();
                }
                else {
                    $scope.searchValueUEM = '';
                    $scope.employeeid = [];
                    $scope.checkall = false;
                    swal("No Record  Found..... !!");
                }
            });
        };

        $scope.obj.searchValueDeptd = '';
        $scope.filterValue4 = function (obj) {
            return angular.lowercase(obj.HRMDC_Name).indexOf(angular.lowercase($scope.obj.searchValueDeptd)) >= 0;
        };

        /* DESIGNATION SELECTION */
        $scope.all_checkdesg = function (designationselectedAll) {
            var toggleStatus = designationselectedAll;
            angular.forEach($scope.filterValue1, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.searchValueUEM = '';
            $scope.checkall = false;
            $scope.DesgSelectionDetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };

        $scope.togchkbxdesg = function (groupType) {
            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {
                return itm.selected;
            });
            $scope.searchValueUEM = '';
            $scope.checkall = false;
            $scope.DesgSelectionDetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };

        $scope.DesgSelectionDetails = function () {
            $scope.arrayuserEmp = [];
            $scope.get_userEmplist = [];
            angular.forEach($scope.designationdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserEmp.push(em);
                }
            });
            var data = {
                "DRApprovalTypeFlag": "DRApproval",
                "designationlist": $scope.arrayuserEmp
            };
            apiService.create("ISM_DailyReport_Approval/DesgSelectionDetails", data).then(function (promise) {
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
        };

        $scope.obj.searchValueDesgd = '';
        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.HRMDES_DesignationName).indexOf(angular.lowercase($scope.obj.searchValueDesgd)) >= 0 || angular.lowercase(obj.MI_Name).indexOf(angular.lowercase($scope.obj.searchValueDesgd)) >= 0;
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

        $scope.obj.searchValueUEMd = '';
        $scope.filterValue2 = function (obj) {
            return angular.lowercase(obj.userEmpName).indexOf(angular.lowercase($scope.obj.searchValueUEMd)) >= 0 || angular.lowercase(obj.HRMD_DepartmentName).indexOf(angular.lowercase($scope.obj.searchValueUEMd)) >= 0;
        };

        $scope.onchangestartdate = function () {
           // $scope.end_Date = null;
         //   $scope.end_Date = $scope.start_Date;
            $scope.getsaveddetails = [];
            $scope.GetSavedDetails();
        };

        $scope.otherdaydatechange = function () {
            $scope.end_Date = null;
            $scope.start_Date = null;
        };
        $scope.OnApprovalvalidation = function (emp_drdates, Concattendance) {
            angular.forEach(emp_drdates, function (d) {
                d.status = Concattendance;
                if (Concattendance == 1) {
                    d.Remarks = "Approved";
                }
                else {
                    d.Remarks = "Rejected";
                }
                
            });
        }
        $scope.getReport = function (user) {          
            $scope.headreport = [];
            $scope.get_report = []; $scope.empattendance = [];    
            var data = {
                "start_date": new Date($scope.start_Date).toDateString(),
                "end_date": new Date($scope.end_Date).toDateString(),
                "reporttype": "DailyReportApproval",
                "HRME_Id": user.HRME_Id                 
                };
                apiService.create("ISM_ConsolidateDailyReport/getReport", data).then(function (promise) {
                    if (promise.get_report !== null && promise.get_report.length > 0) {
                        $scope.get_report = promise.get_report;
                        $scope.emp_listcon = [];
                        var count = 0; $scope.empattendance = promise.get_report_effort;

                        angular.forEach(promise.get_report_effort, function (d) {
                            count = 0;
                            angular.forEach($scope.get_report, function (dd) {
                                if (d.HRME_Id === dd.HRME_Id) {
                                    if ($scope.emp_listcon.length === 0) {
                                        $scope.emp_listcon.push({
                                            HRME_Id: dd.HRME_Id, employeename: dd.EMPLOYEENAME, employeecode: dd.EMPLOYEECODE,
                                            empdept: dd.DEPTNAME, empdesig: dd.DESGNAME
                                        });
                                    } else if ($scope.emp_listcon.length > 0) {
                                        count = 0;
                                        angular.forEach($scope.emp_listcon, function (ddd) {
                                            if (ddd.HRME_Id === d.HRME_Id) {
                                                count += 1;
                                            }
                                        });

                                        if (count === 0) {
                                            $scope.emp_listcon.push({
                                                HRME_Id: dd.HRME_Id, employeename: dd.EMPLOYEENAME, employeecode: dd.EMPLOYEECODE,
                                                empdept: dd.DEPTNAME, empdesig: dd.DESGNAME
                                            });
                                        }
                                    }
                                }
                            });
                        });
                        $scope.emp_date = [];
                        var countdate = 0;
                        $scope.get_report_effort = promise.get_report_effort;
                        angular.forEach($scope.emp_listcon, function (dd) {
                            $scope.emp_drreport = [];
                            countdate = 0;
                            angular.forEach($scope.get_report, function (d) {
                                if (dd.HRME_Id === d.HRME_Id) {
                                    /* Approved Efforts */
                                    var decimaltimetot = d.ISMDRPT_ApprovedTime;
                                    var hrstot = parseInt(Number(decimaltimetot));
                                    var mintot = Math.round((Number(decimaltimetot) - hrstot) * 60);
                                    if (hrstot.toString().length === 1) {
                                        hrstot = '0' + hrstot;
                                    }
                                    if (mintot.toString().length === 1) {
                                        mintot = '0' + mintot;
                                    }
                                    d.approvedeffort = hrstot + " : " + mintot + " Hrs";
                                    /* Actual Efforts */
                                    var decimaltimetot1 = d.ISMTPLTA_EffortInHrs;
                                    var hrstot1 = parseInt(Number(decimaltimetot1));
                                    var mintot1 = Math.round((Number(decimaltimetot1) - hrstot1) * 60);
                                    if (hrstot1.toString().length === 1) {
                                        hrstot1 = '0' + hrstot1;
                                    }
                                    if (mintot1.toString().length === 1) {
                                        mintot1 = '0' + mintot1;
                                    }
                                    d.actualefforts = hrstot1 + " : " + mintot1 + " Hrs";
                                    /* Time Taken In DR Efforts */
                                    var decimaltimetot2 = d.ISMDRPT_TimeTakenInHrs;
                                    var hrstot2 = parseInt(Number(decimaltimetot2));
                                    var mintot2 = Math.round((Number(decimaltimetot2) - hrstot2) * 60);
                                    if (hrstot2.toString().length === 1) {
                                        hrstot2 = '0' + hrstot2;
                                    }
                                    if (mintot2.toString().length === 1) {
                                        mintot2 = '0' + mintot2;
                                    }
                                    d.detimetaken = hrstot2 + " : " + mintot2 + " Hrs";
                                    $scope.emp_drreport.push(d);
                                }
                            });
                            dd.emp_drdates = $scope.emp_drreport;
                            angular.forEach($scope.get_report_effort, function (ddd) {
                                if (ddd.HRME_Id === dd.HRME_Id) {
                                    dd.totaltimetaken = ddd.TimeTakenInHrs;
                                    dd.totaltimeapproved = ddd.ApprovedTimeInHrs;
                                    dd.totaltimeefforts = ddd.EffortInHrs;
                                }
                            });
                        });
                        $scope.emp_drreport = [];
                        
                        $('#DailyModal1').modal('show');
                        console.log($scope.emp_listcon);
                    } else {
                        swal("No Record Found.... !!");
                        $scope.get_report = [];
                    }
                });
            
        };

        // GET SEARCH DETAILS
        $scope.GetSavedDetails = function () {
            if ($scope.myForm.$valid) {
                $scope.headreport = [];
                var data = "";
                $scope.deptlist = [];
                $scope.desglist = [];
                $scope.emplist = [];

                angular.forEach($scope.departmentdropdown, function (d) {
                    if (d.selected === true) {
                        $scope.deptlist.push({ HRMDC_ID: d.HRMDC_ID });
                    }
                });

                angular.forEach($scope.designationdropdown, function (d) {
                    if (d.selected === true) {
                        $scope.desglist.push({ HRMDES_Id: d.HRMDES_Id });
                    }
                });
                $scope.emplist.push({ HRME_Id: $scope.obj.HRME_Id.HRME_Id });

                //angular.forEach($scope.get_userEmplist, function (d) {
                //    if (d.empck === true) {
                //        $scope.emplist.push({ HRME_Id: d.HRME_Id });
                //    }
                //});

                data = {
                    "DRApprovalTypeFlag": "DRApproval",
                    "Fromdate": new Date($scope.start_Date).toDateString(),
                    "Todate": new Date($scope.end_Date).toDateString(),
                    "reporttype": $scope.todayorothers,
                    "deptlist": $scope.deptlist,
                    "desglist": $scope.desglist,
                    "emplist": $scope.emplist
                };

                apiService.create("ISM_DailyReport_Approval/GetSavedDetails", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.getsaveddetails !== null && promise.getsaveddetails.length > 0) {
                            $scope.getsaveddetails = promise.getsaveddetails;
                            $scope.empwise_statuslist = promise.empwise_statuslist;
                            $scope.emp_list = [];
                            var count = 0;
                            angular.forEach($scope.getsaveddetails, function (dd) {
                                if ($scope.emp_list.length === 0) {
                                    $scope.emp_list.push({
                                        HRME_Id: dd.HRME_Id, employeename: dd.EMPLOYEENAME, employeecode: dd.EMPLOYEECODE,
                                        empdept: dd.DEPTNAME, empdesig: dd.DESGNAME, ISMDRPT_ApprovedTime: dd.ISMDRPT_ApprovedTime,
                                        ISMDRPT_Date: dd.ISMDRPT_Date, HRMD_Id: dd.HRMD_Id, HRMDES_Id: dd.HRMDES_Id, MI_Id: dd.MI_Id 
                                    });
                                } else if ($scope.emp_list.length > 0) {
                                    count = 0;
                                    angular.forEach($scope.emp_list, function (ddd) {
                                        if (ddd.HRME_Id === dd.HRME_Id && ddd.ISMDRPT_Date == dd.ISMDRPT_Date) {
                                            count += 1;
                                        }
                                    });

                                    if (count === 0) {
                                        $scope.emp_list.push({
                                            HRME_Id: dd.HRME_Id, employeename: dd.EMPLOYEENAME, employeecode: dd.EMPLOYEECODE,
                                            empdept: dd.DEPTNAME, empdesig: dd.DESGNAME, overallremarks: "", ISMDRPT_ApprovedTime: dd.ISMDRPT_ApprovedTime,
                                            ISMDRPT_Date: dd.ISMDRPT_Date,
                                            HRMD_Id: dd.HRMD_Id, HRMDES_Id: dd.HRMDES_Id, MI_Id: dd.MI_Id 
                                        });
                                    }
                                }
                            });

                            //Emp Wise Status
                            $scope.temp_emp_status = [];

                            angular.forEach($scope.emp_list, function (emp) {
                                $scope.temp_emp_status = [];
                                angular.forEach($scope.empwise_statuslist, function (emp_status) {
                                    if (emp.HRME_Id === emp_status.HRME_Id) {
                                        $scope.temp_emp_status.push(emp_status);
                                    }
                                });
                                emp.status_list = $scope.temp_emp_status;
                            });

                            $scope.emp_date = [];
                            var countdate = 0;

                            angular.forEach($scope.emp_list, function (dd, i) {
                                $scope.emp_drreport = [];
                                var totalworkinghrs = 0;
                                var counthrs = 0.00;

                                angular.forEach($scope.getsaveddetails, function (d, j) {
                                    if (dd.HRME_Id === d.HRME_Id && dd.ISMDRPT_Date == d.ISMDRPT_Date) {
                                        $scope.timetakenactual = "";
                                        $scope.timetaken = "";
                                        countdate = d.HRME_Id + "NAME" + i + "Date" + j;

                                        var decimaltimetot = d.ISMDRPT_TimeTakenInHrs;

                                        var hrstot = parseInt(Number(d.ISMDRPT_TimeTakenInHrs));

                                        var mintot = Math.round((Number(decimaltimetot) - hrstot) * 60);

                                        counthrs = (parseFloat(mintot) * 0.0166667).toFixed(2);

                                        counthrs = parseFloat(hrstot) + parseFloat(counthrs);

                                        totalworkinghrs += counthrs;

                                        if (hrstot.toString().length === 1) {
                                            hrstot = '0' + hrstot;
                                        }
                                        if (mintot.toString().length === 1) {
                                            mintot = '0' + mintot;
                                        }

                                        $scope.timetaken = hrstot + " Hrs " + mintot + " Mins";


                                        var decimaltimetot1 = d.ISMTPLTA_EffortInHrs;
                                        var hrstot1 = parseInt(Number(d.ISMTPLTA_EffortInHrs));
                                        var mintot1 = Math.round((Number(decimaltimetot1) - hrstot1) * 60);
                                        if (hrstot1.toString().length === 1) {
                                            hrstot1 = '0' + hrstot1;
                                        }
                                        if (mintot1.toString().length === 1) {
                                            mintot1 = '0' + mintot1;
                                        }
                                        $scope.timetakenactual = hrstot1 + " Hrs " + mintot1 + " Mins";
                                        var res = "";
                                        var countlength = 0;
                                        if (d.ISMDRPT_Remarks !== null && d.ISMDRPT_Remarks !== "") {
                                            if (d.ISMDRPT_Remarks.toString().length > 100) {
                                                res = d.ISMDRPT_Remarks.substring(0, 100);
                                                res = res + "....";
                                                countlength = 1;
                                            } else {
                                                res = d.ISMDRPT_Remarks;
                                            }
                                        }

                                        $scope.emp_drreport.push({
                                            ISMTCR_TaskNo: d.ISMTCR_TaskNo, ISMDRPT_Id: d.ISMDRPT_Id,
                                            ISMTCR_Title: d.ISMTCR_Title, ISMDRPT_Status: d.ISMDRPT_Status, ISMTPLTA_EffortInHrs: $scope.timetakenactual,
                                            ISMDRPT_TimeTakenInHrs: $scope.timetaken, ISMDRPT_Date: d.ISMDRPT_Date, COUNTCHECKLIST: d.COUNTCHECKLIST,
                                            HRME_Id: d.HRME_Id, Approvedflag: false, Remarks: "Approved", Efforts: 0, index: countdate, ISMTCR_Id: d.ISMTCR_Id,
                                            effhrs: parseInt(hrstot), effmin: parseInt(mintot), drremarks: d.ISMDRPT_Remarks, ISMTPLTA_Id: d.ISMTPLTA_Id,
                                            status: "1", countlength: countlength, drresponsedetails: res,
                                            ISMTCR_Id_rejected: d.ISMTCR_Id_rejected, ISMMTCAT_TaskCategoryName: d.ISMMTCAT_TaskCategoryName,
                                            TASK_CREATED_BY: d.TASK_CREATED_BY, periodicitydailyflag: d.periodicitydailyflag,
                                            ISMTAPL_Periodicity: d.ISMTAPL_Periodicity, ISMDRPT_ApprovedTime: d.ISMDRPT_ApprovedTime,
                                            Concallattendance: "1", ConCallApprovedflag: false
                                        });
                                    }
                                });

                                var totalhrstot = parseInt(Number(totalworkinghrs));

                                var totalmintot = Math.round((Number(totalworkinghrs) - totalhrstot) * 60);

                                if (totalhrstot.toString().length === 1) {
                                    totalhrstot = '0' + totalhrstot;
                                }
                                if (totalmintot.toString().length === 1) {
                                    totalmintot = '0' + totalmintot;
                                }
                                $scope.totaltimetaken = totalhrstot + " Hrs" + " " + totalmintot + " Mins";

                                dd.totalhrs = $scope.totaltimetaken;
                                dd.emp_drdates = $scope.emp_drreport;
                            });

                            $scope.filldata = promise.filldata;
                            angular.forEach($scope.emp_list, function (dd) {
                                dd.totalpunchedhrs = "";
                                angular.forEach($scope.filldata, function (d) {                                   
                                    if (dd.HRME_Id === d.HRME_Id && d.FOEP_PunchDate_New == dd.ISMDRPT_Date) {
                                        if (d.WorkingHours != null && d.WorkingHours != "") {
                                            var PunchInOut = d.WorkingHours.split(':');
                                            var punchin = PunchInOut[0];
                                            var punchout = PunchInOut[1];
                                            if (punchin.toString().length === 1) {
                                                punchin = "0" + punchin;
                                            }
                                            dd.totalpunchedhrs = punchin + " Hrs " + punchout + " Mins";
                                            dd.starttime = d.StartTime;
                                            dd.endtime = d.EndTime;
                                            dd.titlehrs = "Punch In :" + d.StartTime + " Punch Out : " + d.EndTime;
                                        }
                                    }
                                });
                            });

                            $scope.emp_drreport = [];
                            console.log($scope.emp_list);
                        } else {
                            swal("No Record Found.... !!");
                            $scope.getsaveddetails = [];
                        }
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.OnChangeAppRejStatus = function (objplnr) {
            objplnr.empstatus = "";
        };

        $scope.OnchangeApproved = function (objplnr, userd) {
            if (objplnr.Approvedflag === true) {
                if (objplnr.Remarks !== undefined && objplnr.Remarks !== null && objplnr.Remarks !== "") {
                    objplnr.Remarks = objplnr.Remarks;
                } else {
                    objplnr.Remarks = "Approved";
                }

                if (objplnr.status !== undefined && objplnr.status !== null && objplnr.status !== "") {
                    objplnr.status = objplnr.status;
                } else {
                    objplnr.status = "1";
                }

            } else {
                if (objplnr.Remarks !== undefined && objplnr.Remarks !== null && objplnr.Remarks !== "") {
                    objplnr.Remarks = objplnr.Remarks;
                } else {
                    objplnr.Remarks = "";
                }
                if (objplnr.status !== undefined && objplnr.status !== null && objplnr.status !== "") {
                    objplnr.status = objplnr.status;
                } else {
                    objplnr.status = "";
                }
            }
            userd.All = userd.emp_drdates.every(function (itm) { return itm.Approvedflag; });
        };

        $scope.OnClickAll = function (dd, chckflag) {
            angular.forEach(dd.emp_drdates, function (d) {
                d.Approvedflag = chckflag;
                if (chckflag === true) {
                    d.Remarks = "Approved";
                    d.status = "1";
                } else {
                    d.Remarks = "";
                    d.status = "";
                }
            });
        };
        $scope.DemoGiven = function (obja, sub) {
            $scope.leadcommentsreportDeatils = [];
            var data = {}; $scope.ISMMTCAT_TaskCategoryName = "";
            if (obja.ISMMTCAT_TaskCategoryName == null) {
                $scope.ISMMTCAT_TaskCategoryName = '';
            }
            else {
                $scope.ISMMTCAT_TaskCategoryName = obja.ISMMTCAT_TaskCategoryName;
            }

            if (sub == 'COLD' || sub == 'HOT' || sub == 'WARM') {
                data = {
                    "HRME_Id": obja.HRME_Id,
                    "fromdate": obja.fromdate,
                    "todate": obja.todate,
                    "flag": obja.emp_flg,
                    "FlagType": 1,
                    "Status": sub,
                    "ISMSMPR_ProductName": $scope.ISMMTCAT_TaskCategoryName

                };
            } else {
                data = {
                    "HRME_Id": obja.HRME_Id,
                    "fromdate": obja.fromdate,
                    "todate": obja.todate,
                    "flag": obja.emp_flg,
                    "FlagType": sub,
                    "Status": "",
                    "ISMSMPR_ProductName": $scope.ISMMTCAT_TaskCategoryName         //existing DTO
                };
            }

            apiService.create("ISM_Client_Project/report_cmtrptDetails/", data).then(function (promise) {
                $scope.obj.sub = sub;
                if (promise.leadcommentsreportDeatils > 0 || promise.leadcommentsreportDeatils !== null) {
                    $scope.leadcommentsreportDeatils = promise.leadcommentsreportDeatils;
                    $scope.HRME_EmployeeFirstName = promise.leadcommentsreportDeatils[0].HRME_EmployeeFirstName;
                    $scope.Scheduleto = promise.leadcommentsreportDeatils[0].Scheduleto;
                    $scope.Totalcount = promise.leadcommentsreportDeatils.length;
                    $scope.leadcommentsreportDeatilsonline = [];
                    $scope.leadcommentsreportDeatilsoffline = [];
                    angular.forEach($scope.leadcommentsreportDeatils, function (dev) {
                        if (dev.ISMSLEDM_DemoType === 'Online') {
                            $scope.leadcommentsreportDeatilsonline.push(dev);
                        } else {
                            $scope.leadcommentsreportDeatilsoffline.push(dev);
                        }
                    });
                    $scope.offline = $scope.leadcommentsreportDeatilsoffline.length;
                    $scope.online = $scope.leadcommentsreportDeatilsonline.length;
                    $scope.givenflg = $scope.leadcommentsreportDeatils[0].givenflg;
                    $('#myModal1').modal('show');
                }
                else {
                    swal('No Data Found!!!');
                    $scope.leadcommentsreportDeatils = "";
                }

            });
        };

        //ViewDemoCheckList
       
        $scope.ViewDemoCheckList = function (user) {
            $scope.taskid5 = []; $scope.obj.givenflg = ""; $scope.demo_employeename = user.employeename;
          
           user.MI_Id = 17; user.HRME_Id = 1962;
           user.HRMD_Id = 218; user.HRMDES_Id = 590;
            var ids2 = []; ids2.push(user.MI_Id);             
            var ids = []; ids.push(user.HRMDES_Id);
            var ids1 = []; ids1.push(user.HRMD_Id);     
            var ids3 = []; ids3.push(user.HRME_Id);    
            
                $scope.fromdatee = "";
                $scope.todatee = "";
                $scope.fromdate = $filter('date')($scope.start_Date, 'yyyy-MM-dd');
                $scope.todate = $filter('date')($scope.end_Date, 'yyyy-MM-dd');
                $scope.Total = [];
                $scope.TotalDemoScheduled = 0;
                $scope.Executed = 0;
                $scope.NotExecuted = 0;
                $scope.Reschedule = 0;
                $scope.RequestQuotation = 0;
                $scope.HOT = 0;
                $scope.WARM = 0;
                $scope.COLD = 0;
            $scope.Charge_Leads = 0;
            $scope.obj.givenflg = "GivenBY";
                var data = {
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate,
                    MI_Id_IdList: ids2,
                    hrmdes_IdList: ids,
                    hrmD_IdList: ids1,
                    hrmE_IdList: ids3,
                    "flag": "GivenBY"
                }
                apiService.create("ISM_Client_Project/report_cmtrpt/", data).
                    then(function (promise) {                      
                        if (promise.leadcommentsreport !=null &&  promise.leadcommentsreport.length > 0) {
                            $scope.leadcommentsreport = promise.leadcommentsreport;
                            $scope.components_list = promise.components_list;                                                   
                            angular.forEach($scope.leadcommentsreport, function (dev) {
                                if ($scope.taskid5.length === 0) {
                                    $scope.taskid5.push({ HRME_Id: dev.HRME_Id, Emp_Name: dev.Emp_Name });
                                }
                                else if ($scope.taskid5.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.taskid5, function (emp) {
                                        if (emp.HRME_Id === dev.HRME_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.taskid5.push({ HRME_Id: dev.HRME_Id, Emp_Name: dev.Emp_Name });
                                    }
                                }
                            });
                            angular.forEach($scope.taskid5, function (ddd) {
                                $scope.templist = [];
                                angular.forEach($scope.leadcommentsreport, function (dd) {
                                    if (dd.HRME_Id === ddd.HRME_Id) {
                                        $scope.templist.push(dd);
                                    }
                                });

                                ddd.leadcommentsreport = $scope.templist;

                            });
                            if ($scope.leadcommentsreport != null && $scope.leadcommentsreport.length > 0) {                              
                                angular.forEach($scope.leadcommentsreport, function (itm) {
                                    $scope.TotalDemoScheduled = (Number($scope.TotalDemoScheduled) + (Number(itm.Total_Leads)));
                                    $scope.Executed = (Number($scope.Executed) + (Number(itm.Demo_Given)));
                                    $scope.NotExecuted = (Number($scope.NotExecuted) + (Number(itm.Not_Given_Demo)));
                                    $scope.Reschedule = (Number($scope.Reschedule) + (Number(itm.Total_Leads_Re)));
                                    $scope.RequestQuotation = (Number($scope.RequestQuotation) + (Number(itm.SendQuotation)));
                                    $scope.HOT = (Number($scope.HOT) + (Number(itm.HOT)));
                                    $scope.WARM = (Number($scope.WARM) + (Number(itm.WARM)));
                                    $scope.COLD = (Number($scope.COLD) + (Number(itm.COLD)));
                                    $scope.Charge_Leads = (Number($scope.Charge_Leads) + (Number(itm.Charge_Leads)));

                                });
                                

                            }
                            $('#viewdemo').modal('show');
                        }
                        else {
                            swal('No Data Found!!!');
                            $scope.leadcommentsreport = "";
                        }
                    })
            
        };
        /* SAVE APPROVAL */
        $scope.SaveApprovalDetails = function () {
            if ($scope.myForm1.$valid) {
                $scope.selecteddetails = [];
                $scope.selectedemployeedetails = [];
                angular.forEach($scope.emp_list, function (d) {
                    $scope.selecteddetails = [];
                    var totoalapprovedefforts = 0;
                    angular.forEach(d.emp_drdates, function (dd) {

                        var counthrs = 0.00;
                        if (dd.status === "1") {
                            counthrs = (parseFloat(dd.effmin) * 0.0166667).toFixed(2);

                            counthrs = parseFloat(dd.effhrs) + parseFloat(counthrs);
                        }

                        dd.Efforts = counthrs;

                        totoalapprovedefforts += counthrs;
                        var Approvedflag = false;
                        var task_status = "";

                        if (dd.status === "1") {
                            Approvedflag = true;
                            task_status = dd.empstatus != undefined && dd.empstatus != null && dd.empstatus != '' ? dd.empstatus : '';
                        } else if (dd.status === "0") {
                            task_status = "Open";
                        }

                        var ConCallApprovedflag = false;
                        if (dd.ISMMTCAT_TaskCategoryName.toUpperCase() === "CON CALL") {
                            if (dd.Concallattendance === "1") {
                                ConCallApprovedflag = true;
                            }
                            else {
                                ConCallApprovedflag = false;

                            }
                        }
                        $scope.selecteddetails.push({
                            Approvedflag: Approvedflag, Efforts: dd.Efforts, Remarks: dd.Remarks,
                            ISMDRPT_Id: dd.ISMDRPT_Id, ISMTCR_Id: dd.ISMTCR_Id, HRME_Id: dd.HRME_Id,
                            ISMDRPT_TimeTakenInHrsmin: "Hours", ISMTPLTA_Id: dd.ISMTPLTA_Id,
                            Task_Response: task_status, ISMDRPT_ApprovedTime: dd.ISMDRPT_ApprovedTime,
                            ConCallApprovedflag: ConCallApprovedflag, ISMMTCAT_TaskCategoryName: dd.ISMMTCAT_TaskCategoryName

                        });
                    });

                    $scope.selectedemployeedetails.push({
                        HRME_Id: d.HRME_Id, employeename: d.employeename, remarks: d.overallremarks, approvedefforts: totoalapprovedefforts,
                        selectedapprovaldetails: $scope.selecteddetails, fromdate: new Date($scope.start_Date).toDateString()
                    });
                });

                var data = {
                    "DRApprovalTypeFlag": "DRApproval",
                    selectedemployeedetails: $scope.selectedemployeedetails,
                    "Fromdate": new Date($scope.start_Date).toDateString(),
                    "Todate": new Date($scope.end_Date).toDateString()
                };
                apiService.create("ISM_DailyReport_Approval/SaveApprovalDetails", data).then(function (promise) {
                    if (promise.returnval === true) {
                        swal("Record Saved Successully");
                    } else {
                        swal("Faile To Save Records");
                    }
                    $scope.getsaveddetails = [];
                    $scope.emp_list = [];
                    //$scope.start_Date = null;
                    //$scope.end_Date = null;

                    // $state.reload();
                });
            } else {
                $scope.submitted1 = true;
            }
        };

        $scope.ViewCheckListSavedDetails = function (objs, user) {

            $scope.checklist_taskno = objs.ISMTCR_TaskNo;
            $scope.checklist_employeename = user.employeename;
            $scope.checklist_employeecode = user.employeecode;

            var data = {
                "DRApprovalTypeFlag": "DRApproval",
                "ISMDRPT_Id": objs.ISMDRPT_Id
            };

            apiService.create("ISM_DailyReport_Approval/ViewCheckListSavedDetails", data).then(function (promise) {
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
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.ismdrptfL_FilePath;
                                }
                            }
                        });
                        $('#viewfiles').modal('show');
                    }
                }
            });
        };

        $scope.ViewDRResponse = function (objss) {
            $scope.drresponse = "";
            $scope.drresponse = objss.drremarks;
            $('#showdresponse').modal('show');
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
                "DRApprovalTypeFlag": "DRApproval",
                "ISMTCR_Id": $scope.taskid
            };
            apiService.create("ISM_DailyReport_Approval/viewresponse", data).then(function (promise) {
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

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
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

        $scope.exportToExcel = function (export_id) {
            $scope.stdate = $filter('date')($scope.start_Date, 'dd-MM-yyyy');
            $scope.end = $filter('date')($scope.end_Date, 'dd-MM-yyyy');
            var excelname = 'CONSOLIDATED REPORT ' + $scope.stdate + ' TO ' + $scope.end;
            excelname = excelname.toUpperCase() + '.xls';
            //var printSectionId = export_id;
            var exportHref = Excel.tableToExcel('#detailedreport', $scope.routename);
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
            innerContents = document.getElementById("detailedreportprint").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
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


        $scope.previewimg = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalPreview').modal('show');

        };
        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalPreview').modal('show');

        };
        // DR Not Approved List

        $scope.OnClickNotApprovedBtn = function () {
            $scope.MonthId = "";
            $scope.YearId = "";
            $scope.submitteddr = false;
            $scope.TempEmployee = [];

            var monthid = $scope.newdate.getMonth() + 1;
            var yearid = $scope.newdate.getFullYear();
            $scope.MonthId = monthid;
            $scope.YearId = yearid;
            $scope.GetDetails();

            $('#viewdrnotapproveddetails').modal('show');
        };

        $scope.GetDRNotApprovedList = function () {
            $scope.submitteddr = false;
            $scope.TempEmployee = [];
            if ($scope.myformdrdatesnotapproved.$valid) {
                $scope.GetDetails();
            } else {
                $scope.submitteddr = true;
            }
        };

        $scope.GetDetails = function () {
            var data = {
                "DRApprovalTypeFlag": "DRApproval",
                "MonthId": $scope.MonthId,
                "YearId": $scope.YearId
            };

            apiService.create("ISM_DailyReport_Approval/GetDRNotApprovedList", data).then(function (promise) {

                if (promise.drnotapprovedmessage !== undefined && promise.drnotapprovedmessage !== null && promise.drnotapprovedmessage.length > 0) {
                    $scope.generate = false;
                    $scope.drnotapprovedmessage = promise.drnotapprovedmessage;
                    $scope.TempEmployee = [];

                    $scope.monthname = "";
                    $scope.yearname = "";
                    angular.forEach($scope.Monthlist, function (d) {
                        if (parseInt($scope.MonthId) === d.ivrM_Month_Id) {
                            $scope.monthname = d.ivrM_Month_Name;
                            $scope.yearname = $scope.YearId;
                        }
                    });
                    angular.forEach($scope.drnotapprovedmessage, function (dd) {
                        if ($scope.TempEmployee.length === 0) {
                            $scope.TempEmployee.push(dd);
                        } else if ($scope.TempEmployee.length > 0) {
                            var countemp = 0;
                            angular.forEach($scope.TempEmployee, function (d) {
                                if (d.HRME_Id === dd.HRME_Id) {
                                    countemp += 1;
                                }
                            });
                            if (countemp === 0) {
                                $scope.TempEmployee.push(dd);
                            }
                        }
                    });

                    angular.forEach($scope.TempEmployee, function (dd) {
                        $scope.TempDates_Details = [];
                        angular.forEach($scope.drnotapprovedmessage, function (d) {
                            if (dd.HRME_Id === d.HRME_Id) {
                                $scope.TempDates_Details.push(d);
                            }
                        });
                        dd.DatesDetails = $scope.TempDates_Details;
                    });
                }
                //else {
                //    swal("No Records Found");
                //}
            });
        }

        $scope.OnChangeMonth = function () {
            $scope.submitteddr = false;
            $scope.TempEmployee = [];
            if ($scope.MonthId !== undefined && $scope.MonthId !== null && $scope.MonthId !== "") {
                $scope.GetDetails();
            }
        };

        $scope.OnChangeYear = function () {
            $scope.submitteddr = false;
            $scope.TempEmployee = [];
            if ($scope.YearId !== undefined && $scope.YearId !== null && $scope.YearId !== "") {
                $scope.GetDetails();
            }
        };

        $scope.interacteddr = function () {
            return $scope.submitteddr;
        }

        $scope.PrintData = function () {
            var innerContents = document.getElementById("printdetails").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');

            popupWinindow.document.close();
        };
    }
})();