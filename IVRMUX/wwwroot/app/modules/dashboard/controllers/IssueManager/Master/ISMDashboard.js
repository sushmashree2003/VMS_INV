
(function () {
    'use strict';
    angular.module('app').controller('ISMDashboardController', ISMDashboardController);
    ISMDashboardController.$inject = ['$compile', '$rootScope', '$scope', '$state', '$http', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', '$window', '$timeout', 'Excel', 'intracontroller'];
    function ISMDashboardController($compile, $rootScope, $scope, $state, $http, $location, dashboardService, Flash, apiService, $stateParams, $filter, uiCalendarConfig, superCache, $window, $timeout, Excel, intracontroller) {


        $('.modal').on('hide.bs.modal', function (e) {
            e.stopPropagation();
            $('body').css('padding-right', '');
        });
        var HostName = location.host;

        $scope.HRESA_AdvStatus = 'Applied';
        //ada.ms
        $scope.dashdata = [];
        $scope.dashdata1 = [];
        $scope.dashdata2 = [];

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

        $scope.ordershow = false;  


        $scope.getserverdate();

        //Task pending list for approval
        $scope.taskpendingforapproval = function () {
            $scope.typeFlg = "Default";
            var data = {
                "typeFlg": $scope.typeFlg
            };
            apiService.create("ISM_ComplaintsTaskList/getdetails", data).then(function (promise) {
                if (promise.get_pendingcount !== null && promise.get_pendingcount.length > 0) {

                    $scope.get_pendingcount = promise.get_pendingcount.length;
                    $scope.get_pendingcountNew = promise.get_pendingcount;

                    $('#taskapprovallistmodel').modal('show');
                }
                else {
                    $scope.get_pendingcount = 0;
                }
            });
        }


        $scope.currentPagesales = 1;
        $scope.itemsPerPagesales = 5;
        $scope.date1 = new Date();
        var fromdate1 = $scope.date1 == null ? "" : $filter('date')($scope.date1, "yyyy-MM-dd");
        $scope.ViewDetails = function (item) {
            $scope.dashdata = [];
            $scope.dashdata1 = [];
            $scope.dashdata2 = [];

            $scope.pending = [];
            $scope.pending1 = [];
            $scope.pending2 = [];
            $scope.generated = [];
            $scope.generated1 = [];
            $scope.generated2 = [];

            var obj = {
                "type": item,
                "date": fromdate1
            };

            apiService.create("ISMDashboard/onclickdata", obj).then(function (promise) {

                $scope.dashdata = promise.admindata;
                //$scope.dashdata2 = promise.admindata;        
                //$scope.dashdata1 = promise.admindata;        
                //$scope.griddata = promise.griddata;


                angular.forEach($scope.dashdata, function (dd) {
                    if (dd.LNR_Status == "P" && dd.LNC_Id == 1) {
                        $scope.pending.push(dd);
                    }
                    else if (dd.LNR_Status == "G" && dd.LNC_Id == 1) {
                        $scope.generated.push(dd);
                    }
                });
                angular.forEach($scope.dashdata, function (dd) {
                    if (dd.LNR_Status == "P" && dd.LNC_Id == 2) {
                        $scope.pending1.push(dd);
                    }
                    else if (dd.LNR_Status == "G" && dd.LNC_Id == 1) {
                        $scope.generated1.push(dd);
                    }
                });
                angular.forEach($scope.dashdata, function (dd) {
                    if (dd.LNR_Status == "P" && dd.LNC_Id == 3) {
                        $scope.pending2.push(dd);
                    }
                    else if (dd.LNR_Status == "G" && dd.LNC_Id == 1) {
                        $scope.generated2.push(dd);
                    }
                });
                if (promise.admindata.length > 0) {

                    $scope.griddata = promise.griddata;
                    if (item == 'PunchDetails') {
                        $('#ViewAttendanceDetails').modal('show');
                    }
                    else if (item == 'LeaveApproved') {
                        $('#viewLeavedetails').modal('show');
                    }
                    else if (item == 'PlannerNotCreated') {
                        $('#Plannerdetails').modal('show'); w
                    }

                    else if (item == 'EmployeePendingLegalCount') {
                        $('#LegalCountdetails').modal('show');
                    }
                    else if (item == 'EmployeeGeneratedLegalCount') {
                        $('#LegalGeneratedCountDetails').modal('show');
                    }

                    else if (item == 'TADAApply') {
                        $('#viewTadadetails').modal('show');
                    }
                    else if (item == 'Deviationpercentage') {
                        $('#viewTaskdeviaton').modal('show');
                    }
                    else if (item == 'Recruitmentdetails') {
                        $('#Recruitmentdetails').modal('show');

                        $scope.employeeid = [];
                        angular.forEach($scope.dashdata, function (dev) {
                            if ($scope.employeeid.length === 0) {
                                $scope.employeeid.push({
                                    HRCD_Id: dev.HRCD_Id,
                                    CandidateName: dev.CandidateName,
                                    HRMP_Position: dev.HRMP_Position,
                                    HRCISC_InterviewVenue: dev.HRCISC_InterviewVenue

                                    //percentage: dev.percentage
                                });
                            } else if ($scope.employeeid.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.employeeid, function (emp) {
                                    if (emp.HRCD_Id === dev.HRCD_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.employeeid.push({
                                        HRCD_Id: dev.HRCD_Id, CandidateName: dev.CandidateName, HRMP_Position: dev.HRMP_Position, HRCISC_InterviewVenue: dev.HRCISC_InterviewVenue

                                        //percentage: dev.percentage
                                    });
                                }
                            }

                        });

                        angular.forEach($scope.employeeid, function (ddd) {
                            $scope.templist = [];
                            angular.forEach($scope.dashdata, function (dd) {
                                if (dd.HRCD_Id === ddd.HRCD_Id) {
                                    $scope.templist.push(dd);
                                }
                            });
                            ddd.plannerdetails = $scope.templist;
                            //  ddd.percentage = (sum_deviation / $scope.templist.length).toFixed(2);
                        });
                    }
                }
                else {
                    swal('No Data Found..!!');
                }
            });

            $scope.tabination1 = function (item2) {
                $scope.dashdata2 = [];
                var obj = {
                    "type": item2,
                    "date": fromdate1
                };
                apiService.create("ISMDashboard/onclickdata", obj).then(function (promise) {
                    $scope.dashdata2 = promise.admindata;
                });
            }

            $scope.tabination = function (item1) {
                $scope.dashdata1 = [];
                $scope.pending1 = [];
                var obj = {
                    "type": item1,
                    "date": fromdate1
                };
                apiService.create("ISMDashboard/onclickdata", obj).then(function (promise) {
                    angular.forEach($scope.dashdata1, function (dd) {
                        if (dd.LNR_Status == "P" && dd.LNC_Id == 2) {
                            $scope.pending1.push(dd);
                        }
                    });
                    $scope.dashdata1 = promise.admindata;

                    if (item1 == "LastRecruitmentdetails") {
                        $scope.employeeid1 = [];
                        angular.forEach($scope.dashdata1, function (dev) {
                            if ($scope.employeeid1.length === 0) {
                                $scope.employeeid1.push({
                                    HRCD_Id: dev.HRCD_Id,
                                    CandidateName: dev.CandidateName,
                                    HRMP_Position: dev.HRMP_Position,
                                    HRCISC_InterviewVenue: dev.HRCISC_InterviewVenue
                                });
                            } else if ($scope.employeeid1.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.employeeid1, function (emp) {
                                    if (emp.HRCD_Id === dev.HRCD_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.employeeid1.push({
                                        HRCD_Id: dev.HRCD_Id, CandidateName: dev.CandidateName, HRMP_Position: dev.HRMP_Position, HRCISC_InterviewVenue: dev.HRCISC_InterviewVenue

                                    });
                                }
                            }
                        });

                        angular.forEach($scope.employeeid1, function (ddd) {
                            $scope.templist = [];
                            angular.forEach($scope.dashdata1, function (dd) {
                                if (dd.HRCD_Id === ddd.HRCD_Id) {
                                    $scope.templist.push(dd);
                                }
                            });
                            ddd.recruitmendetails = $scope.templist;
                        });


                    }

                    //if (item1=="ClientPendingLegalCount") {

                    //}

                });
            }
        };


        $scope.RedirectToattendence = function () {
            window.location.href = "http://localhost:57606/#/app/EmployeeLateInEarlyOutReport/307";
            $('.modal-backdrop').remove();
        };
        $scope.RedirectToISM_PlannerReport = function () {
            window.location.href = "http://localhost:57606/#/app/ISM_PlannerReport/793";
            $('.modal-backdrop').remove();
        };

        $scope.RedirectToivrm_LegalProcess = function () {
            window.location.href = "http://localhost:57606/#/app/ivrm_LegalProcess/";
            $('.modal-backdrop').remove();
        };

        $scope.RedirectToLeaveReport = function () {
            window.location.href = "http://localhost:57606/#/app/LeaveReport/793";
            $('.modal-backdrop').remove();
        };
        $scope.RedirectTorecruitment = function () {
            window.location.href = "http://localhost:57606/#/app/InterviewReport/307";
            $('.modal-backdrop').remove();
        };




        //======================================================uiCalendarConfig
        var miid = "";
        var configsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 10;
        $scope.obj1 = {};
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.itemsPerPage12 = 10;
        $scope.currentPage12 = 1;
        $scope.logdetails = false;
        $scope.ddate = new Date();
        $scope.maxDatemf = new Date();
        $scope.maxDatemf.setDate($scope.maxDatemf.getDate() - 1);
        $scope.submitted = false;
        $scope.earlyoutcount = 0;
        $scope.latecomingcount = 0;
        $scope.summinutes = 0;
        $scope.lateindates = "";
        $scope.lateindateafter = "";
        $scope.earlyoutdates = "";
        $scope.earlyoutdateafter = "";
        $scope.emptotlop = 0;
        $scope.dailyrptncount = 0;
        $scope.dailyrptndates = 0;

        $scope.maxDatedof = new Date();
        $scope.ISMDRPT_Date = new Date();
        $scope.HREL_SanctionedAmount = 0;
        $scope.HREL_TotalPrincipalPaid = 0;
        $scope.HREL_TotalInterestPaid = 0;
        $scope.HREL_TotalPending = 0;
        $scope.inventorytails = false;
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.search = "";
            var pageid = 2;

            apiService.getURI("ISMDashboard/getdetails", pageid).then(function (promise) {
                $scope.leadfollowuppopup = promise.Leadfollowpopup;
                if (promise.role_flag != null && promise.role_flag.length > 0) {
                    $scope.role_flag = promise.role_flag;
                }
                if (promise.emp_deatils != null && promise.emp_deatils.length > 0) {
                    $scope.emp_deatils = promise.emp_deatils;

                    $scope.email = $scope.emp_deatils[0].HRME_EmailId;
                    $scope.emp_code = $scope.emp_deatils[0].HRME_EmployeeCode;
                    $scope.mobile_num = $scope.emp_deatils[0].HRME_MobileNo;
                    $scope.doj = $scope.emp_deatils[0].HRME_DOJ;
                    $scope.aadhar_num = $scope.emp_deatils[0].HRME_AadharCardNo;
                    $scope.pan_num = $scope.emp_deatils[0].HRME_PANCardNo;
                    $scope.HRMD_DepartmentName = promise.emp_deatils[0].HRMD_DepartmentName;

                    function convertMinutesToHHMM(minutes) {
                        const hours = Math.floor(minutes / 60);
                        const remainingMinutes = minutes % 60;
                        return `${String(hours).padStart(2, '0')}Hr:${String(remainingMinutes).padStart(2, '0')}Min`;
                    }

                    const totalMinutes = promise.emp_deatils[0].HRME_Relaxtime;
                    const hhmm = convertMinutesToHHMM(totalMinutes);
                    console.log(hhmm); // Outputs: "04:47"
                    $scope.HRME_Relaxtime = hhmm;

                    if (promise.emp_deatils[0].HRME_Id > 0) {
                        $scope.OnClickemployeedetails(promise.emp_deatils[0].HRME_Id, promise.emp_deatils[0].MI_Id || 0);
                    }

                }
                if (promise.totpending != null && promise.totpending.length > 0) {
                    $scope.totpending = promise.totpending;
                    $('#tadahrms').modal('show');
                }
                if (promise.proxyleavedetails != null && promise.proxyleavedetails.length > 0) {
                    $scope.proxyleavedetails = promise.proxyleavedetails
                    $scope.HRME_EmployeeFirstName = $scope.proxyleavedetails[0].HRME_EmployeeFirstName;
                    $scope.HRME_EmployeeCode = $scope.proxyleavedetails[0].HRME_EmployeeCode;
                    $scope.HRELAP_FromDate = $scope.proxyleavedetails[0].HRELAP_FromDate;
                    $scope.HRELAP_ToDate = $scope.proxyleavedetails[0].HRELAP_ToDate;
                    $('#proxyleavedetails').modal('show');

                }
                $scope.callcentredetails = promise.callCentreDetails
                //}

                if (promise.demo_list != null && promise.demo_list.length > 0) {
                    $scope.demotypeonlinee = 0;
                    $scope.demotypeoflinee = 0;
                    $scope.demo_list = promise.demo_list;
                    $scope.demo_list_count = promise.demo_list.length;
                    $scope.demotypeonline = [];
                    $scope.demotypeoffline = [];
                    angular.forEach($scope.demo_list, function (dev) {
                        if (dev.ismsledM_DemoType == 'Online') {
                            $scope.demotypeonline.push(dev);
                        }
                        else if (dev.ismsledM_DemoType == 'Offline') {
                            $scope.demotypeoffline.push(dev);
                        }

                    });
                    $scope.demotypeonlinee = $scope.demotypeonline.length;
                    $scope.demotypeoflinee = $scope.demotypeoffline.length;

                }


                if (promise.demo_list_tellicaller != null && promise.demo_list_tellicaller.length > 0) {
                    $scope.leadVerifycount = 0;
                    $scope.leadVerifycountnot = 0;
                    $scope.demo_list_tellicaller = promise.demo_list_tellicaller;
                    $scope.demo_list_count_tell = promise.demo_list_tellicaller.length;
                    $scope.leadVerify = [];
                    $scope.leadVerifynot = [];
                    angular.forEach($scope.demo_list_tellicaller, function (dev) {
                        if (dev.ISMSLE_VerifiedFlg == true) {
                            $scope.leadVerify.push(dev);
                        }
                        if (dev.ISMSLE_VerifiedFlg == null) {
                            $scope.leadVerifynot.push(dev);
                        }
                    });
                    $scope.leadVerifycount = $scope.leadVerify.length;
                    $scope.leadVerifycountnot = $scope.leadVerifynot.length;
                }


                $scope.ViewDemoDetailss = function (onlineofflineflag) {
                    if (promise.demo_list != null && promise.demo_list.length > 0) {
                        $scope.demo_list = promise.demo_list;
                        $scope.demotypeonline = [];
                        $scope.demotypeoffline = [];
                        angular.forEach($scope.demo_list, function (dev) {
                            if (dev.ismsledM_DemoType == 'Online') {
                                $scope.demotypeonline.push(dev);

                            } else if (dev.ismsledM_DemoType == 'Offline') {
                                $scope.demotypeoffline.push(dev);
                            }
                        });
                        if (onlineofflineflag == "Online") {
                            $('#DemoDetailsOnline').modal('show');
                        }
                        else {
                            $('#DemoDetailsOffline').modal('show');
                        }
                    }
                }
                $scope.leavedata();

                $scope.ViewDemoDetailsstell = function (onlineofflineflag1) {
                    if (promise.demo_list_tellicaller != null && promise.demo_list_tellicaller.length > 0) {
                        $scope.demo_list_tellicaller = promise.demo_list_tellicaller;
                        $scope.leadVerify = [];
                        $scope.leadVerifynot = [];
                        angular.forEach($scope.demo_list_tellicaller, function (dev) {
                            if (dev.ISMSLE_VerifiedFlg == true) {
                                $scope.leadVerify.push(dev);

                            } else if (dev.ISMSLE_VerifiedFlg == null) {
                                $scope.leadVerifynot.push(dev);
                            }

                        });
                        if (onlineofflineflag1 === "true") {
                            $('#DemoleadVerify').modal('show');
                        }
                        else if (onlineofflineflag1 == 'a') {
                            $('#DemoleadVerifynot').modal('show');
                        }

                        else {
                            $('#Demoleadtotal').modal('show');
                        }


                    }
                }






                $scope.ViewDemoDetails = function () {
                    if (promise.demo_list != null && promise.demo_list.length > 0) {
                        $scope.demo_list = promise.demo_list;
                        $scope.demo_list_count = promise.demo_list.length;
                        $('#DemoDetails').modal('show');

                    }
                }



                if (promise.ratingDetails != null) {
                    if (promise.ratingDetails.length > 0) {
                        $scope.QuestionCnt = promise.ratingDetails[0].QuestionCnt;
                        $scope.OverallRating = promise.ratingDetails[0].OverallRating;
                    } else {
                        $scope.QuestionCnt = 254;
                        $scope.OverallRating = 4.1;

                    }
                }
                else {
                    $scope.QuestionCnt = 254;
                    $scope.OverallRating = 4.1;

                }
                if (promise.dashboardloandetails != null) {
                    if (promise.dashboardloandetails.length > 0) {

                        $scope.dashboardloandetails = promise.dashboardloandetails;
                        $scope.HREL_SanctionedAmount = $scope.dashboardloandetails[0].HREL_SanctionedAmount;
                        $scope.HREL_TotalPrincipalPaid = $scope.dashboardloandetails[0].HREL_TotalPrincipalPaid;
                        $scope.HREL_TotalInterestPaid = $scope.dashboardloandetails[0].HREL_TotalInterestPaid;
                        $scope.HREL_TotalPending = $scope.dashboardloandetails[0].HREL_TotalPending;

                    }
                }


                $scope.qRcodeActivityDetailsTemp = [];
                $scope.QRcodeActivityDetails = [];
                $scope.qRcodeActivityDetailsTemp = promise.qRcodeActivityDetails;
                if ($scope.qRcodeActivityDetailsTemp != null && $scope.qRcodeActivityDetailsTemp.length > 0) {
                    for (var t = 0; t < $scope.qRcodeActivityDetailsTemp.length; t++) {
                        $scope.QRcodeActivityDetails.push({
                            HRMARACTT_Time: moment($scope.qRcodeActivityDetailsTemp[t].HRMARACTT_Time, 'HH:mm a').format(),
                            HRMB_BuildingName: $scope.qRcodeActivityDetailsTemp[t].HRMB_BuildingName,
                            HRMF_FloorName: $scope.qRcodeActivityDetailsTemp[t].HRMF_FloorName,
                            HRMAR_AreaName: $scope.qRcodeActivityDetailsTemp[t].HRMAR_AreaName,
                            HRMARACT_ActivityName: $scope.qRcodeActivityDetailsTemp[t].HRMARACT_ActivityName,
                            HRMARACTT_ActivityDuration: $scope.qRcodeActivityDetailsTemp[t].HRMARACTT_ActivityDuration,
                            HRMARACTSTMAP_Date: new Date($scope.qRcodeActivityDetailsTemp[t].HRMARACTSTMAP_Date),

                        });
                    }
                }

                if (promise.prioritylist != null && promise.prioritylist.length > 0) {
                    $scope.prioritylist = promise.prioritylist;
                }

                if (promise.proxyleavedetails != null && promise.proxyleavedetails.length > 0) {
                    $scope.proxyleavedetails = promise.proxyleavedetails;
                }

                if (promise.graduitydetails.length > 0 && promise.graduitydetails != null) {
                    $scope.Graduitydetails = promise.graduitydetails;
                    $scope.graduityamount = $scope.Graduitydetails[0].Gratuityamout;
                    $scope.dateofjoining = $scope.Graduitydetails[0].HRME_DOJ;
                }
                if (promise.employeePLAmount.length > 0 && promise.employeePLAmount != null) {
                    $scope.employeePLAmount = promise.employeePLAmount;
                    $scope.plamount = $scope.employeePLAmount[0].amount;
                    $scope.dateofjoining = $scope.employeePLAmount[0].HRME_DOJ;
                    $scope.pleaves = $scope.employeePLAmount[0].PLeaves;
                }
                if (promise.upcomingholiday != null && promise.upcomingholiday.length > 0) {
                    $scope.holidaylist = promise.upcomingholiday;
                    $scope.upcomingholiday = promise.upcomingholiday[0];
                }
                else {
                    $scope.upcomingholiday = promise.upcomingholiday;
                    $scope.holidaylist = [];
                }

                if (promise.leavedetails != null && promise.leavedetails.length > 0) {
                    $scope.HRELS_TotalLeaves = promise.leavedetails[0].hrelS_TotalLeaves;
                    $scope.HRELS_CBLeaves = promise.leavedetails[0].hrelS_CBLeaves;
                }
                else {
                    $scope.HRELS_TotalLeaves = 0;
                    $scope.HRELS_CBLeaves = 0;
                }

                if (promise.getbda_Report != null && promise.getbda_Report.length > 0) {
                    $('#tadaBdm').modal('show');
                    $scope.TadaDdM = [];
                    angular.forEach(promise.getbda_Report, function (dev) {
                        if ($scope.TadaDdM.length === 0) {
                            $scope.TadaDdM.push(dev);
                        }
                        else if ($scope.TadaDdM.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.TadaDdM, function (emp) {
                                if (emp.IER_ID === dev.IER_ID) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.TadaDdM.push(dev);
                            }
                        }
                    })
                    angular.forEach($scope.TadaDdM, function (ddd) {
                        $scope.templist = [];
                        angular.forEach(promise.getbda_Report, function (dd) {
                            if (dd.IER_ID === ddd.IER_ID) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.tadaCategory = $scope.templist;

                    });

                    angular.forEach(promise.TadaDdM, function (dd) {
                        if (dd.Blcokflag == true) {
                            $scope.generate = false;
                        }
                    });
                }
                else {
                    $('#tadaBdm').modal('hide');
                }
                if (promise.visitorcheckoutdetails != null && promise.visitorcheckoutdetails.length > 0) {
                    $scope.visitorcheckoutdetails = promise.visitorcheckoutdetails;
                    $('#visitorespendinglist').modal('show');
                }



                $scope.getdeviationreport = promise.getdeviationreport;
                if ($scope.getdeviationreport !== null && $scope.getdeviationreport.length > 0) {
                    $scope.completedpercentage = $scope.getdeviationreport[0].per;
                } else {
                    $scope.completedpercentage = 0;
                }

                $scope.punchdetails = promise.punchdetails;
                if ($scope.punchdetails !== null && $scope.punchdetails.length > 0) {
                    $scope.viewattendance = true;
                }
                else { $scope.viewattendance = false; }

                $scope.punchdetailsprevious = promise.punchdetailsprevious;
                if ($scope.punchdetailsprevious !== null && $scope.punchdetailsprevious.length > 0) {
                    $scope.viewattendancep = true;
                }
                else { $scope.viewattendancep = false; }


                if (promise.emp_deatils != null && promise.emp_deatils.length > 0) {
                    $scope.emp_deatils = promise.emp_deatils;
                    $scope.email = $scope.emp_deatils[0].HRME_EmailId;
                    $scope.emp_code = $scope.emp_deatils[0].HRME_EmployeeCode;
                    $scope.mobile_num = $scope.emp_deatils[0].HRME_MobileNo;
                    $scope.doj = $scope.emp_deatils[0].HRME_DOJ;
                    $scope.aadhar_num = $scope.emp_deatils[0].HRME_AadharCardNo;
                    $scope.pan_num = $scope.emp_deatils[0].HRME_PANCardNo;
                    $scope.HRME_PreviousRelaxtime = $scope.emp_deatils[0].HRME_PreviousRelaxtime

                    var totalMinutes = $scope.HRME_PreviousRelaxtime;
                    var hours = Math.floor(totalMinutes / 60);
                    var minutes = totalMinutes % 60;
                    $scope.PreviousRelaxtime = hours + " Hr " + minutes + " Min";

                }

                //$scope.lateindata = promise.lateindata;
                $scope.lateinminute = 0;
                var hourval = 0;
                var tothour = 0;
                var minval = 0;
                var totmin = 0;
                if (promise.lateindata != null && promise.lateindata.length > 0) {
                    $scope.todaydate = new Date();
                    $scope.lateearlylist = promise.lateindata;
                    if (($filter('date')(new Date($scope.lateearlylist[promise.lateindata.length - 1].punchdate), 'yyyy-MM-dd') == $filter('date')(new Date($scope.todaydate), 'yyyy-MM-dd')) && ($scope.lateearlylist[promise.lateindata.length - 1].FOEPD_InOutFlg == 'O')) {
                        $scope.lateearlylist.splice(promise.lateindata.length - 1, 1);
                    }

                    //LATE IN
                    angular.forEach($scope.lateearlylist, function (late) {
                        hourval = late.lateby.split(':')[0];
                        minval = late.lateby.split(':')[1];
                        if (parseInt(hourval) < 3) {
                            tothour = parseInt(tothour) + parseInt(hourval);
                            totmin = parseInt(totmin) + parseInt(minval);
                        }
                    });
                    //EARLY OUT
                    angular.forEach($scope.lateearlylist, function (late) {
                        hourval = late.earlyby.split(':')[0];
                        minval = late.earlyby.split(':')[1];
                        if (parseInt(hourval) < 3) {
                            tothour = parseInt(tothour) + parseInt(hourval);
                            totmin = parseInt(totmin) + parseInt(minval);
                        }
                    });
                }
                if (totmin > 60) {
                    tothour = parseInt(tothour) + parseInt(totmin / 60);
                    totmin = parseInt(totmin % 60);
                }
                //$scope.lateinminute = 180 - (parseInt(tothour * 60) + parseInt(totmin));
                $scope.lateinminute = tothour + " Hr " + totmin + " Min";

                //$scope.lateindata = promise.lateindata;
                $scope.lateinminutep = 0;
                var hourvalp = 0;
                var tothourp = 0;
                var minvalp = 0;
                var totminp = 0;
                if (promise.lateindataprevious != null && promise.lateindataprevious.length > 0) {
                    $scope.todaydatep = new Date();
                    $scope.lateearlylistp = promise.lateindataprevious;
                    if (($filter('date')(new Date($scope.lateearlylistp[promise.lateindataprevious.length - 1].punchdate), 'yyyy-MM-dd') == $filter('date')(new Date($scope.todaydatep), 'yyyy-MM-dd')) && ($scope.lateearlylistp[promise.lateindataprevious.length - 1].FOEPD_InOutFlg == 'O')) {
                        $scope.lateearlylistp.splice(promise.lateindataprevious.length - 1, 1);
                    }

                    //LATE IN
                    angular.forEach($scope.lateearlylistp, function (late) {
                        hourvalp = late.lateby.split(':')[0];
                        minvalp = late.lateby.split(':')[1];
                        if (hourvalp < 3) {
                            tothourp = parseInt(tothourp) + parseInt(hourvalp);
                            totminp = parseInt(totminp) + parseInt(minvalp);
                        }
                    });
                    //EARLY OUT
                    angular.forEach($scope.lateearlylistp, function (late) {
                        hourvalp = late.earlyby.split(':')[0];
                        minvalp = late.earlyby.split(':')[1];
                        if (hourvalp < 3) {
                            tothourp = parseInt(tothourp) + parseInt(hourvalp);
                            totminp = parseInt(totminp) + parseInt(minvalp);
                        }
                    });
                }
                if (totminp > 60) {
                    tothourp = parseInt(tothourp) + parseInt(totminp / 60);
                    totminp = parseInt(totminp % 60);
                }
                //$scope.lateinminute = 180 - (parseInt(tothour * 60) + parseInt(totmin));
                $scope.lateinminutep = tothourp + " Hr " + totminp + " Min";

                if (($scope.punchdetails != null && $scope.punchdetails.length > 0) && (promise.lateindata != null && promise.lateindata.length > 0)) {
                    angular.forEach(promise.lateindata, function (late) {
                        angular.forEach($scope.punchdetails, function (punch) {
                            // if (late.punchdate == punch.punchdate && late.HRME_Id == punch.HRME_Id) {
                            if (($filter('date')(new Date(late.punchdate), 'yyyy-MM-dd')) == ($filter('date')(new Date(punch.punchdate), 'yyyy-MM-dd')) && late.HRME_Id == punch.HRME_Id) {
                                if (late.FOEPD_InOutFlg == "I") {
                                    punch.lateclass = "lateinearly";
                                    punch.lateearly = "lateinearly";

                                }
                                else {
                                    punch.earlyclass = "lateinearly";
                                    punch.lateearly = "lateinearly";
                                }
                            }
                        });
                    });
                }
                if (($scope.punchdetailsprevious != null && $scope.punchdetailsprevious.length > 0) && (promise.lateindataprevious != null && promise.lateindataprevious.length > 0)) {
                    angular.forEach(promise.lateindataprevious, function (late) {
                        angular.forEach($scope.punchdetailsprevious, function (punch) {
                            // if (late.punchdate == punch.punchdate && late.HRME_Id == punch.HRME_Id) {
                            if (($filter('date')(new Date(late.punchdate), 'yyyy-MM-dd')) == ($filter('date')(new Date(punch.punchdate), 'yyyy-MM-dd')) && late.HRME_Id == punch.HRME_Id) {
                                if (late.FOEPD_InOutFlg == "I") {
                                    punch.lateclass = "lateinearly";
                                    punch.lateearly = "lateinearly";

                                }
                                else {
                                    punch.earlyclass = "lateinearly";
                                    punch.lateearly = "lateinearly";
                                }
                            }
                        });
                    });
                }

                $scope.employeedetails = [];

                $scope.employeedetails = promise.employeedetails;
                if (promise.role_flag === 'ADMIN' || promise.role_flag === 'admin') {
                    $scope.logdetails = true;
                    $scope.inventorytails = true;
                }
                if (promise.role_flag === 'Accountant' || promise.role_flag === 'accountant') {
                    $scope.inventorytails = true;
                }
                if (promise.role_flag === 'Accountant' || promise.role_flag === 'accountant') {
                    $scope.inventorytails = true;
                }
                if (promise.role_flag === 'HR' || promise.role_flag === 'hR') {
                    $scope.inventorytails = true;
                    $scope.ordershow = true;

                }
                //inventorytails //HR
                // $scope.inventorytails = true;
                if (promise.role_flag === 'Accountant') {
                    //  $('#account').modal('show');                    
                    $scope.ismDashBoard = promise.ismDashBoard;
                    if ($scope.ismDashBoard !== undefined && $scope.ismDashBoard !== null && $scope.ismDashBoard.length > 0) {
                        $scope.pppf2 = false;
                    }
                    else {
                        $scope.pppf2 = true;
                    }
                    $scope.getadvancetada = promise.getadvancetada;
                    if ($scope.getadvancetada !== null && $scope.getadvancetada.length > 0) {
                        $scope.pppfl = false;
                    }
                    else {
                        $scope.pppfl = true;
                    }
                    $scope.planerapproval = promise.planerapproval;
                    if ($scope.planerapproval !== undefined && $scope.planerapproval !== null && $scope.planerapproval.length > 0) {
                        $scope.pppf3 = false;
                    }
                    else {
                        $scope.pppf3 = true;
                    }
                    //planerapproval
                }
                //







                // Tenant Dashboard Detail
                $scope.dashbaordTenantDetailsCount = [];
                if (promise.role_flag.toUpperCase() === 'ADMIN' || promise.role_flag.toUpperCase() === 'ACCOUNTANT') {
                    //if (promise.role_flag.toUpperCase() === 'ADMIN' || promise.role_flag.toUpperCase() === 'ACCOUNTANT' || promise.emp_deatils[0].HRMD_DepartmentName.toUpperCase() === 'ACCOUNTS AND FINANCE') {
                    $scope.adminaccountflag = 1;
                    //$scope.accountsflag = 1;
                    if (promise.dashbaordTenantDetailsCount !== undefined && promise.dashbaordTenantDetailsCount !== null
                        && promise.dashbaordTenantDetailsCount.length > 0) {
                        $scope.dashbaordTenantDetailsCount = promise.dashbaordTenantDetailsCount;
                        $scope.currentmonthpaidamount = promise.dashbaordTenantDetailsCount[0].currentmonthpaidamount;
                        $scope.currentmonthbalanceamount = promise.dashbaordTenantDetailsCount[0].currentmonthbalanceamount;
                        $scope.joined = promise.dashbaordTenantDetailsCount[0].joined;
                        $scope.Left_Tenant = promise.dashbaordTenantDetailsCount[0].Left_Tenant;
                        $scope.agreementend = promise.dashbaordTenantDetailsCount[0].agreementend;

                    }
                }

                $scope.lastpunchdate = promise.lastpunchdate;
                var bdaylist = "";
                $scope.showemployeebd = [];

                if (promise.birthdaylist != null && promise.birthdaylist.length > 0) {
                    angular.forEach(promise.birthdaylist, function (bd) {
                        if (bd.hrmE_EmployeeMiddleName == null) bd.hrmE_EmployeeMiddleName = "";
                        if (bd.hrmE_EmployeeLastName == null) bd.hrmE_EmployeeLastName = "";
                        if (bdaylist == "") { bdaylist = bd.hrmE_EmployeeFirstName + " " + bd.hrmE_EmployeeMiddleName + " " + bd.hrmE_EmployeeLastName; }
                        else {
                            bdaylist = bdaylist + ", " + bd.hrmE_EmployeeFirstName + " " + bd.hrmE_EmployeeMiddleName + " " + bd.hrmE_EmployeeLastName;
                        }
                        if (bd.hrmE_Photo != null && bd.hrmE_Photo != '') {
                            $scope.showemployeebd.push({ name: bd.hrmE_EmployeeFirstName + " " + bd.hrmE_EmployeeMiddleName + " " + bd.hrmE_EmployeeLastName, desgname: bd.hrmdeS_DesignationName, photo: bd.hrmE_Photo })
                        }
                    });
                }
                if (bdaylist == "") {
                    $scope.bdaystring = "";
                }
                else {
                    $scope.bdaystring = "Happy Birthday " + bdaylist;
                }

                $scope.birthdaylist = promise.birthdaylist;

                if ($scope.employeedetails != null && $scope.employeedetails.length > 0) {
                    var alignFillDate = new Date($scope.employeedetails[0].hrmE_DOB);


                    var pickUpDate = new Date();

                    var empmonth = alignFillDate.getMonth();
                    var nowmonth = pickUpDate.getMonth();
                    var empday = alignFillDate.getDate();
                    var nowday = pickUpDate.getDate();
                    var lastdate = 0;
                    if ($scope.lastpunchdate !== null) {
                        lastdate = new Date($scope.lastpunchdate).getDate();
                    }

                    if (empmonth === nowmonth && empday === nowday) {
                        $scope.dobflg = true;
                    }
                    else {
                        $scope.dobflg = false;
                    }
                    if ($scope.dobflg === true) {
                        $('#showcake').modal('show');
                    }
                    else {
                        $('#showcake').modal('hide');
                        $('.modal-backdrop').remove();
                    }

                    $scope.totdayspresent = promise.totdayspresent;
                    $scope.totholiday = promise.totholiday;
                    if (lastdate > parseInt($scope.totholiday)) {
                        $scope.totabsent = parseInt(lastdate) - (parseInt($scope.totdayspresent) + parseInt($scope.totholiday));
                    }
                    else {
                        $scope.totabsent = 0;
                    }
                }

                $("#donutchart").kendoChart({
                    title: {
                        position: "bottom",
                        text: "Issue Log Details"
                    },
                    legend: {
                        visible: false
                    },
                    chartArea: {
                        background: ""
                    },
                    seriesDefaults: {
                        type: "column",
                        startAngle: 60
                    },
                    series: [{
                        data: [{
                            category: "OpenCount",
                            value: promise.openCount,
                            color: "#205d85"
                        }, {
                            category: "ClosedCount",
                            value: promise.closedCount,
                            color: "#89CFF0"
                        }, {
                            category: "InprogressCount ",
                            value: promise.inprogressCount,
                            color: "#3eaee2"
                        }, {
                            category: "CompletedCount",
                            value: promise.completedCount,
                            color: "#c2800b"

                        },]
                    },

                    {
                        labels: {
                            visible: true,
                            background: "transparent",
                            position: "outsideEnd",

                        }
                    }],
                    categoryAxis: {
                        categories: ["OpenCount", "ClosedCount", "InprogressCount ", "CompletedCount",],
                        line: {
                            visible: false
                        },
                        labels: {
                            padding: { top: 5 }
                        }
                    },
                    tooltip: {
                        visible: true,
                        template: "#= category # : #= value #"
                    }
                });


                function explodePie(e) {
                    if (typeof (e.dataSeries.dataPoints[e.dataPointIndex].exploded) === "undefined" || !e.dataSeries.dataPoints[e.dataPointIndex].exploded) {
                        e.dataSeries.dataPoints[e.dataPointIndex].exploded = true;
                    } else {
                        e.dataSeries.dataPoints[e.dataPointIndex].exploded = false;
                    }
                    e.chart.render();
                };

                $("#piechart").kendoChart({
                    title: {
                        position: "bottom",
                        text: "Attendance Details"
                    },
                    legend: {
                        visible: false
                    },
                    chartArea: {
                        background: ""
                    },
                    seriesDefaults: {
                        labels: {
                            visible: true,
                            background: "transparent",
                            template: "#= category #: \n #= value#"
                        }
                    },
                    series: [{
                        type: "pie",
                        startAngle: 20,
                        data: [{
                            category: "Present",
                            value: $scope.totdayspresent,
                            color: "#1a6a90"
                        }, {
                            category: "Holiday",
                            value: $scope.totholiday,
                            color: "#c2800b"
                        }, {
                            category: "Absent",
                            value: $scope.totabsent,
                            color: "#277fa8"
                        },]
                    }],
                    tooltip: {
                        visible: true,
                        format: "{0}"
                    }
                });

                $("#testttttttttt").kendoChart({
                    title: {
                        position: "bottom",
                        text: "Loan Details"
                    },
                    legend: {
                        visible: false
                    },
                    chartArea: {
                        background: ""
                    },
                    seriesDefaults: {
                        type: "column",
                        startAngle: 60
                    },
                    series: [{
                        data: [{
                            category: "SactionedAmount",
                            value: $scope.HREL_SanctionedAmount,
                            color: "#205d85"
                        }, {
                            category: "TotalPaid",
                            value: $scope.HREL_TotalPrincipalPaid,
                            color: "#89CFF0"
                        }, {
                            category: "InterestPaid ",
                            value: $scope.HREL_TotalInterestPaid,
                            color: "#3eaee2"
                        }, {
                            category: "Pending",
                            value: $scope.HREL_TotalPending,
                            color: "#c2800b"

                        },]
                    }, {
                        labels: {
                            visible: true,
                            background: "transparent",
                            position: "outsideEnd",
                            //   template: "#= category #: \n #= value#%"
                        }
                    }],
                    categoryAxis: {
                        categories: ["SactionedAmount", "TotalPaid", "InterestPaid", "Pending"],
                        line: {
                            visible: false
                        },
                        labels: {
                            padding: { top: 5 }
                        }
                    },
                    tooltip: {
                        visible: true,
                        template: "#= category # : #= value #"
                    }
                });







                // DeptHead Wise Bugs Count List
                $scope.checkdeptheadcode = promise.checkdeptheadcode;

                if ($scope.checkdeptheadcode > 0) {
                    $scope.currentPagecount = 1;
                    $scope.itemsPerPagecount = 5;
                    $scope.mappedemployeedetails = promise.mappedemployeedetails;
                }
                $scope.TempEmployee = [];
                if (promise.dR_NotApprovedTL === 0 && promise.drnotapprovedmessage !== null && promise.drnotapprovedmessage.length > 0
                    && (promise.role_flag !== 'ADMIN' && promise.role_flag !== 'admin')) {
                    $scope.drnotapprovedmessage = promise.drnotapprovedmessage;
                    $scope.TempEmployee = [];

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

                    $('#viewdrnotapproveddetails').modal('show');
                }


                $scope.RegeneratedDR = [];
                if (promise.dR_NotApprovedTL === 0 && promise.reapprovedmessage !== null && promise.reapprovedmessage.length > 0
                    && (promise.role_flag !== 'ADMIN' && promise.role_flag !== 'admin')) {
                    $scope.Reapprovedmessage = promise.reapprovedmessage;
                    $scope.RegeneratedDR = [];

                    angular.forEach($scope.Reapprovedmessage, function (dd) {
                        if ($scope.RegeneratedDR.length === 0) {
                            $scope.RegeneratedDR.push(dd);
                        } else if ($scope.RegeneratedDR.length > 0) {
                            var countemp = 0;
                            angular.forEach($scope.RegeneratedDR, function (d) {
                                if (d.HRME_Id === dd.HRME_Id) {
                                    countemp += 1;
                                }
                            });
                            if (countemp === 0) {
                                $scope.RegeneratedDR.push(dd);
                            }
                        }
                    });

                    angular.forEach($scope.RegeneratedDR, function (dd) {
                        $scope.RegeneratedDR_Details = [];
                        angular.forEach($scope.Reapprovedmessage, function (d) {
                            if (dd.HRME_Id === d.HRME_Id) {
                                $scope.RegeneratedDR_Details.push(d);
                            }
                        });
                        dd.RegeneratedDetails = $scope.RegeneratedDR_Details;
                    });

                    $('#viewRegenerateddetails').modal('show');
                }




                $scope.Laptopnotapprovedlist = [];
                if ($scope.checkdeptheadcode > 0) {
                    if (promise.laptopnotapprovedlist !== null && promise.laptopnotapprovedlist.length > 0) {
                        $scope.Laptopnotapprovedlist = promise.laptopnotapprovedlist;
                    }
                }

                if (promise.blockedcount !== null && promise.blockedcount > 0) {
                    $scope.blockedcount = promise.blockedcount;
                    $scope.blockeddetails = promise.blockeddetails;
                }

                $scope.adminhrflag = 0;
                $scope.accountsflag = 0;

                $scope.adminaccountflag = 0;
                $scope.totalabsenties = "";
                $scope.totalpresent = "";

                $scope.totalplannernotgenerate = "";
                $scope.totalplannergenerated = "";

                $scope.pendinglegalcount = "";
                $scope.legalnoticegenerate = "";
                $scope.legalnoticereceived = "";

                $scope.totalplannernotapproved = "";
                $scope.totalplannerapproved = "";

                $scope.totalpettycash = "";

                $scope.dashbaordPettyCashDetailsCount = [];
                $scope.roleflag = promise.role_flag.toUpperCase();

                if (promise.role_flag.toUpperCase() === 'ACCOUNTANT') {
                    $scope.accountsflag = 1;
                }

                if (promise.role_flag.toUpperCase() === 'ADMIN' || promise.role_flag.toUpperCase() === 'HR' || promise.role_flag === 'Front Office Executive') {
                    $scope.adminhrflag = 1;
                    //Attendance
                    if (promise.dashbaordAttendanceDetailsCount !== undefined && promise.dashbaordAttendanceDetailsCount !== null
                        && promise.dashbaordAttendanceDetailsCount.length > 0) {
                        $scope.DashbaordAttendanceDetailsCount = promise.dashbaordAttendanceDetailsCount;
                        $scope.totalabsenties = $scope.DashbaordAttendanceDetailsCount[0].AbsentCount;
                        $scope.totalpresent = $scope.DashbaordAttendanceDetailsCount[0].PresentCount;
                    }
                    //Planner Details
                    if (promise.dashbaordPlannerDetailsCount !== undefined && promise.dashbaordPlannerDetailsCount !== null
                        && promise.dashbaordPlannerDetailsCount.length > 0) {

                        $scope.DashbaordPlannerDetailsCount = promise.dashbaordPlannerDetailsCount;
                        $scope.totalplannernotgenerate = $scope.DashbaordPlannerDetailsCount[0].PlannerNotGeneratedCount;
                        $scope.totalplannergenerated = $scope.DashbaordPlannerDetailsCount[0].PlannerGeneratedCount;
                    }

                    //Legal Dashboard

                    if (promise.dashbaordLegalDetailsCount !== undefined && promise.dashbaordLegalDetailsCount !== null
                        && promise.dashbaordLegalDetailsCount.length > 0) {

                        $scope.DashbaordLegalDetailsCount = promise.dashbaordLegalDetailsCount;
                        $scope.totalpendinglegalcount = $scope.DashbaordLegalDetailsCount[0].PendingLegalCount;
                        $scope.totallegalgeneratecount = $scope.DashbaordLegalDetailsCount[0].LegalNoticeCount;
                        $scope.totallegalrecievedcount = $scope.DashbaordLegalDetailsCount[0].LegalNoticeReceivedCount;
                    }

                    //Planner Approval
                    if (promise.dashbaordPlannerApprovedDetailsCount !== undefined && promise.dashbaordPlannerApprovedDetailsCount !== null
                        && promise.dashbaordPlannerApprovedDetailsCount.length > 0) {

                        $scope.dashbaordPlannerApprovedDetailsCount = promise.dashbaordPlannerApprovedDetailsCount;
                        $scope.totalplannernotapproved = $scope.dashbaordPlannerApprovedDetailsCount[0].PlannerNotApprovedCount;
                        $scope.totalplannerapproved = $scope.dashbaordPlannerApprovedDetailsCount[0].PlannerApprovedCount;
                    }
                    //VMS MODULE AVERAGE
                    if (promise.dashboardmoduleratingreport != null && promise.dashboardmoduleratingreport.length > 0) {
                        $scope.dashboardmoduleratingreport = promise.dashboardmoduleratingreport;
                        $scope.displayaveragerating = $scope.dashboardmoduleratingreport[0].Overall_Average;
                    }
                }

                if (promise.role_flag.toUpperCase() === 'ADMIN' || promise.role_flag.toUpperCase() === 'SALES HEAD' || promise.role_flag.toUpperCase() === 'Sales Head') {
                    $scope.adminaccountflag = 1;
                    if (promise.statewiseLeadListDetails !== undefined && promise.statewiseLeadListDetails !== null
                        && promise.statewiseLeadListDetails.length > 0) {
                        $scope.statewiseLeadListDetails = promise.statewiseLeadListDetails;

                    }
                    $scope.dashbaordSalesDetailsCount = [];

                    if (promise.dashbaordSalesDetailsCount !== undefined && promise.dashbaordSalesDetailsCount !== null
                        && promise.dashbaordSalesDetailsCount.length > 0) {
                        $scope.dashbaordSalesDetailsCount = promise.dashbaordSalesDetailsCount;
                        $scope.Total_Leads = promise.dashbaordSalesDetailsCount[0].Total_Leads;
                        $scope.Total_Verified_Leads = promise.dashbaordSalesDetailsCount[0].Total_Verified_Leads;
                        $scope.Not_Verified = promise.dashbaordSalesDetailsCount[0].Not_Verified;
                        $scope.Total_Demo = promise.dashbaordSalesDetailsCount[0].Total_Demo;
                        $scope.Total_Demo_NotExecuted = promise.dashbaordSalesDetailsCount[0].Total_Demo_NotExecuted;
                        $scope.Total_Demo_Executed = promise.dashbaordSalesDetailsCount[0].Total_Demo_Executed;
                        $scope.Total_Quotation = promise.dashbaordSalesDetailsCount[0].Total_Quotation;

                    }

                    if (promise.dashbaordSalesDetailsCountCurrentDate !== undefined && promise.dashbaordSalesDetailsCountCurrentDate !== null
                        && promise.dashbaordSalesDetailsCountCurrentDate.length > 0) {
                        $scope.dashbaordSalesDetailsCountCurrentDate = promise.dashbaordSalesDetailsCountCurrentDate;
                        $scope.Total_Leads_CurrentDate = promise.dashbaordSalesDetailsCountCurrentDate[0].Total_Leads_CurrentDate;
                        $scope.Total_Verified_Leads_CurrentDate = promise.dashbaordSalesDetailsCountCurrentDate[0].Total_Verified_Leads_CurrentDate;
                        $scope.Not_Verified_CurrentDate = promise.dashbaordSalesDetailsCountCurrentDate[0].Not_Verified_CurrentDate;
                        $scope.Total_Demo_CurrentDate = promise.dashbaordSalesDetailsCountCurrentDate[0].Total_Demo_CurrentDate;
                        $scope.Total_Demo_NotExecuted_CurrentDate = promise.dashbaordSalesDetailsCountCurrentDate[0].Total_Demo_NotExecuted_CurrentDate;
                        $scope.Total_Demo_Executed_CurrentDate = promise.dashbaordSalesDetailsCountCurrentDate[0].Total_Demo_Executed_CurrentDate;
                        $scope.Total_Quotation_CurrentDate = promise.dashbaordSalesDetailsCountCurrentDate[0].Total_Quotation_CurrentDate;

                    }

                }
                if (promise.role_flag.toUpperCase() === 'ACCOUNTANT') {
                    $scope.TADAAdavancePaymentspopup = [];
                    if (promise.tadA_AdavancePayments_popup != null && promise.tadA_AdavancePayments_popup.length > 0) {
                        $scope.TADAAdavancePaymentspopup = promise.tadA_AdavancePayments_popup;
                        $('#TADAAdavancePayments').modal('show');
                    }
                    else {
                        $('#TADAAdavancePayments').modal('hide');
                    }
                    $scope.TADAPaymentpopup = [];
                    if (promise.tadA_Paymentpopup != null && promise.tadA_Paymentpopup.length > 0) {
                        $scope.TADAPaymentpopup = promise.tadA_Paymentpopup;
                        $('#TADAApplyPayment').modal('show');
                    }
                    else {
                        $('#TADAApplyPayment').modal('hide');
                    }
                    $scope.TADAAdvsalesPaymentpopup = [];
                    if (promise.tadA_AdvsalesPaymentpopup != null && promise.tadA_AdvsalesPaymentpopup.length > 0) {
                        $scope.TADAAdvsalesPaymentpopup = promise.tadA_AdvsalesPaymentpopup;
                        $('#TADASalesAdavancePayments').modal('show');
                    }
                    else {
                        $('#TADASalesAdavancePayments').modal('hide');
                    }
                }


                if (promise.role_flag.toUpperCase() === 'ADMIN' || promise.role_flag.toUpperCase() === 'ACCOUNTANT') {
                    $scope.adminaccountflag = 1;

                    //Petty Cash
                    $scope.dashbaordPettyCashDetailsCount = [];
                    if (promise.role_flag.toUpperCase() === 'ADMIN') {
                        $scope.pc_flag = "Indent Not Approved Count";
                    } else {
                        $scope.pc_flag = "Requisition Not Approved Count";
                    }
                    if (promise.dashbaordPettyCashDetailsCount !== undefined && promise.dashbaordPettyCashDetailsCount !== null
                        && promise.dashbaordPettyCashDetailsCount.length > 0) {

                        $scope.dashbaordPettyCashDetailsCount = promise.dashbaordPettyCashDetailsCount;
                        $scope.totalpettycash = 0;
                        angular.forEach($scope.dashbaordPettyCashDetailsCount, function (dd) {
                            $scope.totalpettycash += dd.Count
                        });
                    }



                    //Driver Indent
                    $scope.dashboardDriverIndentDetailsCount = [];
                    if (promise.dashboardDriverIndentDetailsCount !== undefined && promise.dashboardDriverIndentDetailsCount !== null
                        && promise.dashboardDriverIndentDetailsCount.length > 0) {
                        $scope.dashboardDriverIndentDetailsCount = promise.dashboardDriverIndentDetailsCount;
                    }



                }

                // Rating Fetch Task Count

                if (promise.ratingfetchemployee != null && promise.ratingfetchemployee.length > 0) {
                    $scope.ratingfetchemployee = promise.ratingfetchemployee;
                    $('#ratingfetchemployeedata').modal('show');
                } else {
                    $('#ratingfetchemployeedata').modal('hide');
                }


                //Leave Details
                $scope.dashboardleavecount = 0;
                if (promise.dashboardLeaveDetailsCount !== undefined && promise.dashboardLeaveDetailsCount !== null
                    && promise.dashboardLeaveDetailsCount.length > 0) {
                    $scope.dashboardleavecount = promise.dashboardLeaveDetailsCount.length;
                }

                //Purchase Indent Details
                $scope.dashboardpurchaseindentcount = 0;
                $scope.dashboardPurchaseIndentaseDetailsCount = [];
                if (promise.dashboardPurchaseIndentaseDetailsCount !== undefined && promise.dashboardPurchaseIndentaseDetailsCount !== null
                    && promise.dashboardPurchaseIndentaseDetailsCount.length > 0) {
                    $scope.dashboardpurchaseindentcount = promise.dashboardPurchaseIndentaseDetailsCount.length;
                    $scope.dashboardPurchaseIndentaseDetailsCount = promise.dashboardPurchaseIndentaseDetailsCount;
                }


                //dashboard salary display for  accounts added by ada.ms
                $scope.dashbaordHrmssalaryDetailslength = 0;
                $scope.dashbaordHrmssalaryDetailsCount = [];
                if (promise.dashbaordHrmssalaryDetailsCount !== undefined && promise.dashbaordHrmssalaryDetailsCount !== null
                    && promise.dashbaordHrmssalaryDetailsCount.length > 0) {
                    $scope.dashbaordHrmssalaryDetailslength = promise.dashbaordHrmssalaryDetailsCount.length;
                    $scope.dashbaordHrmssalaryDetailsCount = promise.dashbaordHrmssalaryDetailsCount;

                }
                //dashboard Inventory display for  accounts added by ada.ms
                $scope.dashbaordInventoryDetailsCount = [];
                $scope.dashbaordInventoryDetailsCount = promise.dashbaordInventoryDetailsCount;
                if (promise.dashbaordInventoryDetailsCount !== undefined && promise.dashbaordInventoryDetailsCount !== null
                    && promise.dashbaordInventoryDetailsCount.length > 0) {
                    $scope.dashbaordInventoryDetailsCountlength = promise.dashbaordInventoryDetailsCount.length;
                    $scope.dashbaordInventoryDetailsCount = promise.dashbaordInventoryDetailsCount;

                }


                //Purchase Order Details
                $scope.dashboardpurchaseordercount = 0;
                $scope.dashboardPurchaseOrderDetailsCount = [];
                if (promise.dashboardPurchaseOrderDetailsCount !== undefined && promise.dashboardPurchaseOrderDetailsCount !== null
                    && promise.dashboardPurchaseOrderDetailsCount.length > 0) {
                    $scope.dashboardpurchaseordercount = promise.dashboardPurchaseOrderDetailsCount.length;
                    $scope.dashboardPurchaseOrderDetailsCount = promise.dashboardPurchaseOrderDetailsCount;
                }


                // Salary Advance Apply Details
                $scope.salaryadvanceapplydetailscount = 0;
                $scope.getadvancesalaryCount = [];
                if (promise.getadvancesalary !== undefined && promise.getadvancesalary !== null && promise.getadvancesalary.length > 0) {
                    $scope.getadvancesalaryCount = promise.getadvancesalary;
                    $scope.salaryadvanceapplydetailscount = promise.getadvancesalary.length;
                }


                // TADA Advance Apply Details
                $scope.tadaadvanceapplydetailscount = 0;
                $scope.getadvancetadaCount = [];
                if (promise.getadvancetada !== undefined && promise.getadvancetada !== null && promise.getadvancetada.length > 0) {
                    $scope.getadvancetadaCount = promise.getadvancetada;
                    $scope.tadaadvanceapplydetailscount = promise.getadvancetada.length;
                }

                // TADA  Apply Details
                $scope.tadaapplydetailscount = 0;
                $scope.gettadaappliedcount = [];
                if (promise.dashboardTADAAppliedDetailsCount !== undefined && promise.dashboardTADAAppliedDetailsCount !== null && promise.dashboardTADAAppliedDetailsCount.length > 0) {
                    $scope.gettadaappliedcount = promise.dashboardTADAAppliedDetailsCount;
                    $scope.tadaapplydetailscount = promise.dashboardTADAAppliedDetailsCount.length;
                }

                // ChequeBook Apply Details
                $scope.ChequeBookdetailscount = 0;
                $scope.getchequebookdetailscount = [];
                if (promise.dashboardChequeBookDetailsCount !== undefined && promise.dashboardChequeBookDetailsCount !== null
                    && promise.dashboardChequeBookDetailsCount.length > 0) {
                    $scope.ChequeBookdetailscount = promise.dashboardChequeBookDetailsCount.length;
                    $scope.getchequebookdetailscount = promise.dashboardChequeBookDetailsCount;
                }

                //Visitor Appointment
                $scope.VisitorAppointmentdetailscount = 0;
                $scope.getVisitorAppointmentdetailscount = [];
                if (promise.dashboardVisitorAppointmentDetailsCount !== undefined && promise.dashboardVisitorAppointmentDetailsCount !== null
                    && promise.dashboardVisitorAppointmentDetailsCount.length > 0) {
                    $scope.VisitorAppointmentdetailscount = promise.dashboardVisitorAppointmentDetailsCount.length;
                    $scope.getVisitorAppointmentdetailscount = promise.dashboardVisitorAppointmentDetailsCount;
                }

                ////HW Health Checkup
                //$scope.HWHealthcheckupdetailsCount = 0;
                //$scope.DashboardHWHealthcheckupdetailsCount = [];
                //if (promise.dashboardHWHealthcheckupdetailsCount !== undefined && promise.dashboardHWHealthcheckupdetailsCount !== null
                //    && promise.dashboardHWHealthcheckupdetailsCount.length > 0) {
                //    $scope.HWHealthcheckupdetailsCount = promise.dashboardHWHealthcheckupdetailsCount.length;
                //    $scope.DashboardHWHealthcheckupdetailsCount = promise.dashboardHWHealthcheckupdetailsCount;
                //}



                $scope.searchinter = "";
                if ($scope.checkdeptheadcode > 0 && promise.interdeptmenttasklist !== undefined && promise.interdeptmenttasklist !== null
                    && promise.interdeptmenttasklist.length > 0) {
                    $scope.interdeptmenttasklist = promise.interdeptmenttasklist;
                    $('#interdepartmenttaslist').modal('show');
                }
                // termsandconditionagree
                $scope.momdrblock = [];
                if (promise.momdrblockreport != null && promise.momdrblockreport.length > 0) {
                    $scope.momdrblock = promise.momdrblockreport;
                    $('#momdrblock').modal('show');
                }
                else {
                    $('#momdrblock').modal('hide');
                }


                $scope.invvendoronboard = [];
                if (promise.invvendoronboardreport != null && promise.invvendoronboardreport.length > 0) {
                    $scope.invvendoronboard = promise.invvendoronboardreport;
                    $('#invvendoronboard').modal('show');
                }
                else {
                    $('#invvendoronboard').modal('hide');
                }

                $scope.gotoVendorApproval = function () {
                    $('#invvendoronboard').modal('hide');
                    $('body').removeClass('modal-open');
                    $('.modal-backdrop').remove();
                    if ($state && typeof $state.go === 'function') {
                        $state.go('app.INV_VenderApproval', { pageId: 2 });
                    } else {
                        window.location.href = '#/app/INV_VenderApproval/2';
                    }
                };






                if (promise.griddata != null && promise.griddata.length > 0) {
                    $scope.trainingIssue = promise.griddata;
                    $('#trainingpendinglist').modal('show');
                }
                if (promise.bondCompletingEmpList != null && promise.bondCompletingEmpList.length > 0) {
                    $scope.bondCompletingEmpList = promise.bondCompletingEmpList;
                    $('#bondCompletingEmp').modal('show');
                }
                ////Created By AMAN
                // Documents Verify Candidate's
                $scope.CandidateVerify = [];
                if (promise.totalemployees != null && promise.totalemployees.length > 0) {
                    $scope.candidatependinglist = promise.totalemployees;
                    $('#CandidateVerify').modal('show');
                }
                else {
                    $('#CandidateVerify').modal('hide');
                }

                $scope.dev_Completedtask = [];
                if (promise.dev_Completedtask !== null && promise.dev_Completedtask !== undefined && promise.dev_Completedtask.length > 0) {
                    $scope.Dev_Completedtask = promise.dev_Completedtask;
                    $('#DevPending').modal('show');
                }



                //added by praveenkumar
                $scope.leadfollowup = [];
                if (promise.leadfollowpopup != null && promise.leadfollowpopup.length > 0) {
                    $scope.Leadfollowpopup = promise.leadfollowpopup;
                    $('#leadfollowup').modal('show');
                }
                //else {
                //    $('#leadfollowup').modal('hide');
                //}
                // notpunchtoday
                $scope.notpunchtoday = [];
                if (promise.qRcode_emp_deatils != null && promise.qRcode_emp_deatils.length > 0) {
                    $scope.notpunchtoday = promise.qRcode_emp_deatils;
                    $('#termsandconditionagree').modal('show');
                }
                $scope.unauthorised = [];
                if (promise.unotherisedLeavedeatils !== null && promise.unotherisedLeavedeatils !== undefined && promise.unotherisedLeavedeatils.length > 0) {
                    $scope.unotherisedLeavedeatils = promise.unotherisedLeavedeatils;
                    $('#unauthorised').modal('show');
                }


                $scope.latinearlyout = [];
                if (promise.lateinearlyout !== null && promise.lateinearlyout !== undefined && promise.lateinearlyout.length > 0) {
                    $scope.latinearlyout = promise.lateinearlyout;
                    $('#latinearlyout').modal('show');
                }

                $scope.CloseTaskCoutnDetails = [];
                if (promise.closeTaskCoutnDetails !== undefined && promise.closeTaskCoutnDetails !== null
                    && promise.closeTaskCoutnDetails.length > 0) {
                    $scope.CloseTaskCoutnDetails = promise.closeTaskCoutnDetails
                    $('#tadadocument').modal('show');
                }
                if (promise.admindata != null && promise.admindata.length > 0) {

                    swal({
                        title: "Still You Did Not Approved TADA Advance Tour Plan !",
                        text: "Do You Want To Approved..?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Approved!",
                        cancelButtonText: "Cancel",
                        closeOnConfirm: true,
                        closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                intracontroller.viewcompleted = true;
                                $('.modal-backdrop').remove();
                                $('.modal').on('hide.bs.modal', function (e) {
                                    e.stopPropagation();
                                    $('body').css('padding-right', '');
                                });

                                window.location.href = 'http://' + HostName + '/#/app/TADATourApproval/12';
                            }

                        });
                }
                if (promise.sales_Completedtask != null && promise.sales_Completedtask.length > 0) {
                    swal({
                        title: "You still have not approved the sales negotiation. !",
                        text: "Do You Want To Approved..?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Approved!",
                        cancelButtonText: "Cancel",
                        closeOnConfirm: true,
                        closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                intracontroller.viewcompleted = true;
                                $('.modal-backdrop').remove();
                                $('.modal').on('hide.bs.modal', function (e) {
                                    e.stopPropagation();
                                    $('body').css('padding-right', '');
                                });

                                window.location.href = 'http://' + HostName + '/#/app/Sales_Lead_negotiationApproval/1140';
                            }

                        });
                }
                if (promise.getReport != null && promise.getReport.length > 0) {

                    swal({
                        title: "Still You Did Not Approved Final TADA Tour Plan !",
                        text: "Do You Want To Approved..?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Approved!",
                        cancelButtonText: "Cancel",
                        closeOnConfirm: true,
                        closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                intracontroller.viewcompleted = true;
                                $('.modal-backdrop').remove();
                                $('.modal').on('hide.bs.modal', function (e) {
                                    e.stopPropagation();
                                    $('body').css('padding-right', '');
                                });

                                window.location.href = 'http://' + HostName + '/#/app/TADAFinalApproval/12';
                            }

                        });
                }
                //OrderApproval
                if (promise.get_prNo != null && promise.get_prNo.length > 0) {
                    swal({
                        title: "Still You Did Not Approved Purchase Approvals !",
                        text: "Do You Want To Approved..?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Approved!",
                        cancelButtonText: "Cancel",
                        closeOnConfirm: true,
                        closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                intracontroller.viewcompleted = true;
                                $('.modal-backdrop').remove();
                                $('.modal').on('hide.bs.modal', function (e) {
                                    e.stopPropagation();
                                    $('body').css('padding-right', '');
                                });

                                window.location.href = 'http://' + HostName + '/#/app/OrderApproval/123';
                            }

                        });
                }

                //added by rajkumar
                if (promise.qrflag == true) {
                    $scope.HRME_QRCode = promise.hrmE_QRCode;
                    $('#QRCodeGenerate').modal('show');
                }
                else {
                    $scope.HRME_QRCodeone = promise.hrmE_QRCode;
                    var e1 = angular.element(document.getElementById("test"));
                    $compile(e1.html(promise.hrmE_QRCode))(($scope));
                }

                //newly added
                $scope.TodaysDemoSchedule = promise.sataewiselead;
                $scope.TodaysDemoScheduleCount = promise.sataewiselead[0].Total_leads;
                $scope.Total_Executed = promise.sataewiselead[0].Total_Executed;
                $scope.Total_NotExecuted = promise.sataewiselead[0].Total_NotExecuted;
                $scope.RawDataData = promise.dashbaordTenantLeft;
                $scope.TotalRawdata = promise.dashbaordTenantLeft[0].Total_Rawdata;
                $scope.ISMSLERawApproved = promise.dashbaordTenantLeft[0].ISMSLERaw_Approved;




                //adedd


                $scope.Departmentnamedetails = [];
                $scope.employeeidtwo = [];
                $scope.Departmentnamedetails = promise.departmentname;
                $scope.Departmentname = [];
                $scope.Departmentname = promise.departmentname;
                // $scope.TotalActive = promise.departmentcount[0].TotalActive;
                //$scope.TotalDective = promise.departmentcount[0].TotalDective;
                $scope.HRMDC_ID = $scope.Departmentname[0].HRMDC_ID;
                $scope.OnClickemployeedetails(promise.hrmE_Id, promise.mI_Id);

                //added by rohan for legal dashboard
                if (promise.pendingcount != null && promise.pendingcount.length > 0) {
                    $scope.pendingcount = promise.pendingcount.length;
                }
                if (promise.generatedcount != null && promise.generatedcount.length > 0) {
                    $scope.generatedcount = promise.generatedcount.length;
                }

            });

            $scope.taskpendingforapproval();
        };


        $scope.ratingclick = function () {

            window.location.href = "http://localhost:57606/#/app/EmployeeMonthlyRating/";
        };

        $scope.selectedAll1 = "";
        $scope.all_task = function () {
            var checkStatus2 = $scope.obj.alltask;
            angular.forEach($scope.visitorcheckoutdetails, function (itm) {
                itm.selectedd2 = checkStatus2;
            });
        };

        $scope.usercheckC2 = "";
        $scope.get_evalistt = function () {
            $scope.usercheckC2 = $scope.visitorcheckoutdetails.every(function (options) {
                return options.selectedd2;
            });
        };


        $scope.all_taskT = function () {
            var checkStatusT = $scope.obj.alltaskT;
            angular.forEach($scope.trainingIssue, function (itm) {
                itm.selectedT = checkStatusT;
            });
        };
        $scope.usercheckT = "";
        $scope.get_Tlistt = function () {
            $scope.usercheckT = $scope.trainingIssue.every(function (options) {
                return options.selecteT;
            });
        };

        //Petty Cash  View , Redirect
        $scope.ViewPettyCashDetails = function () {
            $scope.searchValuePC = "";
            if ($scope.dashbaordPettyCashDetailsCount !== undefined && $scope.dashbaordPettyCashDetailsCount !== null
                && $scope.dashbaordPettyCashDetailsCount.length > 0) {
                $('#ViewPettyCashDetailsModal').modal('show');
            } else {
                swal("No Records Found", " ", "error");
            }
        };


        $scope.RedirectToPettyCash = function () {
            if ($scope.roleflag === "ADMIN") {
                window.location.href = "http://localhost:57606/#/app/PC_Indent_Approval/845";
            } else {
                window.location.href = "http://localhost:57606/#/app/PC_Indent/845";
            }
            $('.modal-backdrop').remove();
        };

        //dashboardHRMS salary added by ada.ms
        $scope.Viewdashboardhrmsdetails = function () {
            if ($scope.dashbaordHrmssalaryDetailslength > 0) {
                $('#ViewdashboardhrmsdetailsModal').modal('show');
            } else {
                swal("No Records Found", " ", "error");
            }
        };

        $scope.RedirectToHRMS = function () {

            window.location.href = "http://localhost:57606/#/app/cumulativeSalaryGrossReport/598";

            $('.modal-backdrop').remove();
        };
        //dashboard Inventory GRN added by ada.ms
        $scope.ViewdashboardInventory = function () {
            if ($scope.dashbaordInventoryDetailsCountlength > 0) {
                $('#ViewdashboardInventoryetailsModal').modal('show');
            } else {
                swal("No Records Found", " ", "error");
            }
        };

        $scope.RedirectToGRN = function () {

            window.location.href = "http://localhost:57606/#/app/INV_R_GRN_New/845";

            $('.modal-backdrop').remove();
        };

        // Purchase Indent  View , Redirect
        $scope.ViewPurchaseIndentNotApprovedList = function () {
            $scope.searchValue1 = "";
            if ($scope.dashboardPurchaseIndentaseDetailsCount !== undefined && $scope.dashboardPurchaseIndentaseDetailsCount !== null
                && $scope.dashboardPurchaseIndentaseDetailsCount.length > 0) {
                $('#ViewPurchaseindentDetailsModal').modal('show');
            } else {
                swal("No Records Found", " ", "error");
            }
        };

        $scope.RedirectToPurchaseIndent = function () {
            window.location.href = "http://localhost:57606/#/app/IndentApproval/915";
            $('.modal-backdrop').remove();
        };

        //Purchase Order View , Redirect
        $scope.ViewPurchaseOrderNotApprovedList = function () {
            $scope.searchValue = "";
            if ($scope.dashboardPurchaseOrderDetailsCount !== undefined && $scope.dashboardPurchaseOrderDetailsCount !== null
                && $scope.dashboardPurchaseOrderDetailsCount.length > 0) {
                $('#ViewPurchaseOrderDetailsModal').modal('show');
            } else {
                swal("No Records Found", " ", "error");
            }
        };

        $scope.RedirectToPurchaseOrder = function () {
            window.location.href = "http://localhost:57606/#/app/OrderApproval/914";
            $('.modal-backdrop').remove();
        };

        //Driver Indent View , Redirect
        $scope.ViewDriverIndentDetails = function () {
            $scope.search1 = "";
            if ($scope.dashboardDriverIndentDetailsCount !== undefined && $scope.dashboardDriverIndentDetailsCount !== null
                && $scope.dashboardDriverIndentDetailsCount.length > 0) {
                $('#ViewDriverindentDetailsModal').modal('show');
            } else {
                swal("No Records Found", " ", "error");
            }
        };

        $scope.RedirectToDriverIndent = function () {
            window.location.href = "http://localhost:57606/#/app/DriverIndentApproval/853";
            $('.modal-backdrop').remove();
        };

        //Laptop Details View , Redirect
        $scope.ViewLaptopNotApprovedList = function () {
            if ($scope.Laptopnotapprovedlist !== undefined && $scope.Laptopnotapprovedlist !== null && $scope.Laptopnotapprovedlist.length > 0) {
                $('#ViewLaptopNotApprovedList').modal('show');
            } else {
                swal("No Records Found", " ", "error");
            }
        };

        $scope.RedirectToLaptop = function () {
            window.location.href = "http://localhost:57606/#/app/LaptopAppApproval/892";
            $('.modal-backdrop').remove();
        };

        //Salary Advance Apply View , Redirect
        $scope.ViewSalaryAdvanceApplyNotApprovedList = function () {
            $scope.searchtadaadvance = "";
            if ($scope.getadvancesalaryCount !== undefined && $scope.getadvancesalaryCount !== null && $scope.getadvancesalaryCount.length > 0) {
                $('#ViewSalaryAdvanceApplyNotApprovedModal').modal('show');
            } else {
                swal("No Records Found", " ", "error");
            }
        };

        $scope.TodaysDemoschedule = function () {
            $scope.searchtadaadvance = "";
            if ($scope.TodaysDemoSchedule !== undefined && $scope.TodaysDemoSchedule !== null && $scope.TodaysDemoSchedule.length > 0) {
                $('#TodaysDemoschedule').modal('show');
            } else {
                swal("No Records Found", " ", "error");
            }
        };
        $scope.RawData = function () {
            $scope.searchtadaadvance = "";
            if ($scope.RawDataData !== undefined && $scope.RawDataData !== null && $scope.RawDataData.length > 0) {
                $('#RawData').modal('show');
            } else {
                swal("No Records Found", " ", "error");
            }
        };

        $scope.RedirectToSalaryAdvanceApply = function () {
            window.location.href = "http://localhost:57606/#/app/AdvanceApproval/969";
            $('.modal-backdrop').remove();
        };
        $scope.RedirectToSalaryAdvanceReport = function () {
            window.location.href = "http://localhost:57606/#/app/AdvanceApprovalReport/969";
            $('.modal-backdrop').remove();
        };
        //TADA Advance Apply View , Redirect
        $scope.ViewTadaAdvanceApplyNotApprovedList = function () {
            $scope.searchtadaadvance = "";
            if ($scope.getadvancetadaCount !== undefined && $scope.getadvancetadaCount !== null && $scope.getadvancetadaCount.length > 0) {
                $('#ViewTadaAdvanceApplyDetailsModal').modal('show');
            } else {
                swal("No Records Found", " ", "error");
            }
        };

        $scope.RedirectToTADAdvanceAApply = function () {
            window.location.href = "http://localhost:57606/#/app/TADAAdvanceApproval/969";
            $('.modal-backdrop').remove();
        };

        //TADA Apply View , Redirect
        $scope.ViewTadaApplyNotApprovedList = function () {
            $scope.searchtada = "";
            if ($scope.gettadaappliedcount !== undefined && $scope.gettadaappliedcount !== null && $scope.gettadaappliedcount.length > 0) {
                $('#ViewTadaApplyDetailsModal').modal('show');
            } else {
                swal("No Records Found", " ", "error");
            }
        };

        $scope.RedirectToTADAApply = function () {
            window.location.href = "http://localhost:57606/#/app/TADAApproval/970";
            $('.modal-backdrop').remove();
        };

        //ChequeBook Apply View , Redirect
        $scope.ViewChequeBookNotApprovedList = function () {
            $scope.searchChequeBook = "";
            if ($scope.getchequebookdetailscount !== undefined && $scope.getchequebookdetailscount !== null && $scope.getchequebookdetailscount.length > 0) {
                $('#ViewChecqueBookDetailsModal').modal('show');
            } else {
                swal("No Records Found");
            }
        };

        $scope.RedirectToChequeBook = function () {
            window.location.href = "http://localhost:57606/#/app/TADAApproval/970";
            $('.modal-backdrop').remove();
        };

        //Visitor Appointment  View , Redirect
        $scope.ViewVisitorAppointmentNotApprovedList = function () {
            $scope.searchvisitorapointment = "";
            if ($scope.getVisitorAppointmentdetailscount !== undefined && $scope.getVisitorAppointmentdetailscount !== null
                && $scope.getVisitorAppointmentdetailscount.length > 0) {
                $('#ViewVisitorAppointmentNotApprovedList').modal('show');
            } else {
                swal("No Records Found");
            }
        };

        $scope.RedirectTovisitorapointment = function () {
            window.location.href = "http://localhost:57606/#/app/V_AppointmentApprovalStatus/";
            $('.modal-backdrop').remove();
        };

        $scope.RedirectToMakerChecker = function () {
            window.location.href = "http://localhost:57606/#/app/ISM_DailyReport_Approval/884";
            $('.modal-backdrop').remove();
        };
        $scope.RegeneratedToMakerChecker = function () {
            window.location.href = "http://localhost:57606/#/app/ISM_RegenerateDR_Approval/884";
            $('.modal-backdrop').remove();
        };
        $scope.RedirectToComplaintTask = function () {
            window.location.href = "http://localhost:57606/#/app/ISM_ComplaintsTaskList/729";
            $('.modal-backdrop').remove();
        };

        $scope.RedirectToComplaintTaskAssidnedTo = function () {
            window.location.href = "http://localhost:57606/#/app/ISM_ComplaintsTaskList/729";
        };

        $scope.RedirectToComplaintTaskDeviatedTo = function () {
            window.location.href = "http://localhost:57606/#/app/ISM_ComplaintsTaskList/729";
        };

        $scope.RedirectToComplaintTaskReopenTo = function () {
            window.location.href = "http://localhost:57606/#/app/ISM_ComplaintsTaskList/729";
        };

        $scope.RedirectToComplaintTaskLogDetails = function () {
            window.location.href = "http://localhost:57606/#/app/ISM_ComplaintsTaskList/729";
        };
        $scope.ViewDemoDetailsNEW = function () {
            window.location.href = "http://localhost:57606/#/app/ISM_Sales_Lead_CommentsReports/912";
        };
        $scope.TeleCallerDemodetails = function () {
            window.location.href = "http://localhost:57606/#/app/TeleCaller_DemoList/1";
        };


        $scope.pendingissues = function (user) {
            $scope.issue = user.HRMP_Name;
            $scope.company = user.MI_Name;
            var data = {
                "HRMPR_Id": user.HRMPR_Id
            };

            apiService.create("ISMDashboard/pendingissues", data).then(function (promise) {
                if (user.HRMPR_Id === 12345) {
                    if (promise.prioritypivot.length > 0 && promise.prioritypivot !== null) {
                        $scope.prioritypivot = promise.prioritypivot;
                        $('#pendingissuespivot').modal('show');
                    }
                    else {
                        swal('No Data Found..!!');
                    }
                }
                else {
                    if (promise.issueslist.length > 0 && promise.issueslist !== null) {
                        $scope.issueslist = promise.issueslist;
                        $('#pendingissues').modal('show');
                    }
                    else {
                        swal('No Data Found..!!');
                    }
                }
            });
        };


        //  Tenant ViewDetails
        $scope.dashbaordTenantTotalAmountPaid = [];
        $scope.dashbaordTenantBalanceAmount = [];
        $scope.dashbaordTenantJoined = [];
        $scope.dashbaordTenantLeft = [];
        $scope.dashbaordTenantagreementend = [];

        $scope.ViewTenantDetails = function () {
            var data = {

            };
            apiService.create("ISMDashboard/ViewTenantDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.dashbaordTenantTotalAmountPaid = promise.dashbaordTenantTotalAmountPaid;
                    $scope.dashbaordTenantBalanceAmount = promise.dashbaordTenantBalanceAmount;
                    $scope.dashbaordTenantJoined = promise.dashbaordTenantJoined;
                    $scope.dashbaordTenantLeft = promise.dashbaordTenantLeft;
                    $scope.dashbaordTenantagreementend = promise.dashbaordTenantagreementend;
                    $('#tenantdetails').modal('show');
                }
                else {
                    swal('No Data Found..!!');
                }
            });

        }


        //  Statutory ViewDetails


        $scope.ViewStatutorydetails = function () {
            $scope.Flagdata = [];

            $('#Statutorydetails').modal('show');
            $scope.Getincome('IncomeTax');

        }
        $scope.Getincome = function (tax) {
            $scope.Flagdata = [];
            var data = {
                "VMSSC_Flag": tax,

            };
            apiService.create("StatutoryTaxCompliance/Getincome", data).
                then(function (promise) {
                    if (promise.flagdata.length > 0 && promise.flagdata != null) {
                        $scope.Flagdata = promise.flagdata;
                    }

                });

        };
        $scope.ViewEmployeeIssueList = function (dd) {
            $scope.mappedemployeeissuedetails = [];
            $scope.get_statuslist = [];
            $scope.employeedetailsname = dd.userEmpName;
            var data = {
                "MI_Id": dd.MI_Id,
                "HRME_Id": dd.HRME_Id
            };

            apiService.create("ISMDashboard/ViewEmployeeIssueList", data).then(function (promise) {
                if (promise !== null) {
                    $scope.mappedemployeeissuedetails = promise.mappedemployeeissuedetails;
                    $scope.get_statuslist = promise.get_statuslist;

                    //angular.forEach($scope.get_statuslist, function (dd, index) {
                    //    dd.classbox = "";
                    //    dd.faicon = "fa-minus";
                    //    if (index !== 0) {
                    //        dd.classbox = "collapsed-box";
                    //        dd.faicon = "fa-plus";
                    //    }
                    //    dd.pageid = "pageid" + dd.ismmistS_Id;
                    //});

                    $scope.getstatus_list = [];

                    angular.forEach($scope.mappedemployeeissuedetails, function (dd) {
                        if ($scope.getstatus_list.length === 0) {
                            $scope.getstatus_list.push({ ismmistS_StatusName: dd.ISMTCR_Status });
                        } else if ($scope.getstatus_list.length > 0) {
                            var count = 0;

                            angular.forEach($scope.getstatus_list, function (d) {
                                if (d.ismmistS_StatusName === dd.ISMTCR_Status) {
                                    count += 1;
                                }
                            });
                            if (count === 0) {
                                $scope.getstatus_list.push({ ismmistS_StatusName: dd.ISMTCR_Status });
                            }
                        }
                    });

                    angular.forEach($scope.getstatus_list, function (dd, index) {
                        dd.classbox = "";
                        dd.faicon = "fa-minus";
                        if (index !== 0) {
                            dd.classbox = "collapsed-box";
                            dd.faicon = "fa-plus";
                        }
                        dd.count = index;
                        dd.pageid = "pageid" + index;
                        dd.search = "search" + index;
                    });


                    $scope.getStatuslist = [];
                    angular.forEach($scope.getstatus_list, function (dd) {
                        $scope.getStatuslist = [];
                        angular.forEach($scope.mappedemployeeissuedetails, function (d) {
                            if (dd.ismmistS_StatusName === d.ISMTCR_Status) {
                                $scope.getStatuslist.push(d);
                            }
                        });

                        dd.reportdetails = $scope.getStatuslist;
                    });
                    console.log($scope.get_statuslist);
                    if ($scope.mappedemployeeissuedetails !== null && $scope.mappedemployeeissuedetails.length > 0) {
                        $('#mymodalviewemployeeissuelist').modal('show');
                    }
                }
            });
        };

        $scope.pendingpivto = function (user) {

            var data = {
                "HRMPR_Id": user
            };

            apiService.create("ISMDashboard/pendingissues", data).then(function (promise) {
                $scope.header_list = [];
                if (promise.prioritypivot.length > 0 && promise.prioritypivot !== null) {
                    $scope.prioritypivot = promise.prioritypivot;

                    if ($scope.prioritypivot.length > 0) {
                        for (var i = 0; i < $scope.prioritypivot.length; i++) {

                            if (i === 0) {
                                angular.forEach($scope.prioritypivot[i], function (key, r) {
                                    $scope.header_list.push({ colmn: r, head: key });
                                });
                            }
                        }
                    }
                    $('#pendingissuespivot').modal('show');
                }
                else {
                    swal('No Data Found..!!');
                }


            });
        };
        $scope.makeminutes = function (time1) {
            var hour1 = 0;
            var minute1 = 0;
            var totmin = 0;

            hour1 = time1.split(':')[0];
            minute1 = time1.split(':')[1];

            if (hour1 > 0) {
                totmin = parseInt(hour1) * 60;
            }
            totmin = parseInt(totmin) + parseInt(minute1);
            return totmin;
        };
        $scope.addholidaylop = function (holidaylop, typ) {
            if (holidaylop != null && holidaylop != "") {
                $scope.loparray = $scope.unauthleavedatestemp.replace(/[\s]/g, '');
                var loplist = $scope.loparray.split(',');
                if ($scope.loparray.length > 0) {
                    angular.forEach(loplist, function (hl) {
                        if (parseInt(hl) == (parseInt(holidaylop) - 1)) {
                            var index = loplist.indexOf((hl).toString());
                            //var element = holidaylop + '(' + typ + ')';
                            loplist.splice(index + 1, 0, holidaylop);
                            $scope.loplisttemp = loplist;
                            var newvaluse = "";
                            angular.forEach($scope.loplisttemp, function (add) {
                                if (newvaluse == "") {
                                    newvaluse = add;
                                }
                                else {
                                    newvaluse = newvaluse + "," + add;
                                }
                            });
                            $scope.unauthleavedatestemp = newvaluse;
                            return;
                        }
                    });
                }
            }
        }
        $scope.closeemplop = function () {
            $('#lopemployee').modal('hide');
            $('.modal-backdrop').remove();
            $('#employeeorder').modal('show');
            $('.modal').on('hide.bs.modal', function (e) {
                e.stopPropagation();
                $('body').css('padding-right', '');
            });
        }



        //$scope.checkeligibility = function () {
        //    $scope.compoffadjtelig = $scope.compoffadjtelig

        //    if ((Number($scope.compoffadjtelig[0].TotalLOP)) < (Number($scope.obj1.LOPadjest))) {
        //        $scope.obj1.LOPadjest = 0;
        //        swal("Entered Value Should Less Then OR Equal To Total LOP");
        //    }
        //    else if ((Number($scope.obj1.LOPadjest)) > (Number($scope.compoffadjtelig[0].TotalBalance))) {
        //        $scope.obj1.LOPadjest = 0;
        //        swal("Entered Value Is Exiding Comp Off Balance");
        //    }
        //}

        // Rating TaskRole Category Fetch EmpoloyeeTask Category Wise

        $scope.ViewRatingQuestion = function (user) {
            var data = {
                "HRME_Id": user.HRME_Id,
                "MI_Id": user.MI_Id,
                "ISMMTCAT_Id": user.ISMMTCAT_Id
            }
            apiService.create("Rating_Transaction/viewemployeetaskcategory", data).then(function (promise) {

                if (promise.getRatingQuestion.length > 0 && promise.getRatingQuestion != null) {
                    $scope.getRatingQuestion = promise.getRatingQuestion;

                    $scope.RATMQE_Id = $scope.getRatingQuestion[0].ratmqE_Id;
                    $scope.RATMQE_RatingQuestions = $scope.getRatingQuestion[0].ratmqE_RatingQuestions;
                    $scope.RATMQECLSUB_QuestionsSubclassification = $scope.getRatingQuestion[0].ratmqeclsuB_QuestionsSubclassification;
                }

                if (promise.ratingRoleCategoryEmployeerTasks.length > 0 && promise.ratingRoleCategoryEmployeerTasks != null) {

                    $scope.ratingRoleCategoryEmployeerTasks = promise.ratingRoleCategoryEmployeerTasks;
                    $scope.taskcategoryquestionoptionlist = promise.taskcategoryquestionoptionlist;

                    $scope.TempGetoptionQuestionauto = [];

                    angular.forEach($scope.ratingRoleCategoryEmployeerTasks, function (rtt) {
                        $scope.TempGetoptionQuestionauto = [];
                        angular.forEach($scope.taskcategoryquestionoptionlist, function (rqq) {

                            $scope.TempGetoptionQuestionauto.push(rqq)

                        });
                        rtt.ratingtaskoptionlist = $scope.TempGetoptionQuestionauto;
                    });

                    $('#ratingfetchemployeedatawithoption').modal('show');
                }

            });

        }

        $scope.SaveRatingTaskCategory = function () {
            if ($scope.myformtaskcategory.$valid) {

                var data = {
                    "RATMQE_Id": $scope.RATMQE_Id,
                    "RATTR_FeedbackDate": new Date().toDateString(),
                    "Save_Rating_TaskCategory": $scope.ratingRoleCategoryEmployeerTasks,
                };
                apiService.create("Rating_Transaction/SaveRatingTaskCategory", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Record Saved/Updated Successfully");
                            $('#ratingfetchemployeedatawithoption').modal('hide');
                            //$('#ratingfetchemployeedata').modal('show');
                            $scope.loaddata();
                            //
                        }
                        else {
                            swal("Failed Saved/Updated Record");
                            $('#ratingfetchemployeedatawithoption').modal('show');
                        }

                        //$state.reload();
                        //  $scope.loaddata();
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted;
        };

        //$scope.AdjustCompOff = function () {
        //    if ($scope.obj1.LOPadjest > 0) {
        //        var data = {
        //            "LOP": $scope.obj1.LOPadjest,
        //            "Fromdate": $scope.from_date,
        //            "Todate": $scope.to_date
        //        };

        //        apiService.create("FrontOffice/AdjustCompOff", data).then(function (promise) {
        //            if (promise.gettodate.length > 0 && promise.gettodate != null) {
        //                swal("Comp Off Adjested Successfully");

        //            }
        //            else {
        //                swal("Record Not Updated");
        //            }

        //        });
        //        $state.reload();
        //    }
        //    else {
        //        swal("Kindly Enter LOP To Adjust");
        //    }
        //};




        $scope.getlop = function (check) {
            $scope.compoffadjtelig = [];
            $scope.lopshow = false;
            $scope.from_date = new Date().toDateString();
            $scope.to_date = new Date().toDateString();
            if (check == "0") {
                $scope.lopshow = true;
            }


            var data = {


                "Fromdate": $scope.from_date,
                "Todate": $scope.to_date,
                "yearid": 0,
                "month": 0,
                "rdbbutton": "Day",
                "flag": "Single",
                "flagl": check,
            };

            apiService.create("FrontOffice/getmonthreport", data).
                then(function (promise) {
                    $scope.compoffadjtelig = promise.compoffadjtelig;
                    $scope.institutename = promise.institutename;
                    $scope.newfromdate = new Date(promise.fromdate);
                    $scope.newtodate = new Date(promise.todate);

                    $scope.fromdate = new Date(promise.fromdate);
                    $scope.todate = new Date(promise.todate);
                    $scope.year = new Date($scope.fromdate).getFullYear();
                    $scope.month = (new Date($scope.fromdate).getMonth() + 1);

                    var timeDiff = Math.abs($scope.newfromdate.getTime() - $scope.newtodate.getTime());
                    $scope.numberofdays = Math.ceil(timeDiff / (1000 * 3600 * 24)) + 1;

                    var temp_array1 = [];
                    var temp_array = [];
                    var comment_array = [];
                    var arrhlf = [];

                    for (var j = 0; j < promise.selectedActivityDetails1_stf.length; j++) {

                        $scope.workdyas = 0;
                        var arrhlftwo = [];
                        var workingdays = $filter('filter')(promise.workinghourDetails, function (d) {
                            return d.HRME_Id === promise.selectedActivityDetails1_stf[j].hrmE_Id;
                        });

                        if (promise.selectedActivityDetails1_stf[j].hrmE_ExcPunch == false) {
                            if (workingdays != null && workingdays != undefined && workingdays.length != 0) {
                                $scope.workdyas = workingdays.length;
                            }
                            else {
                                $scope.emptotlop = parseFloat($scope.numberofdays);
                            }
                        }
                        else {
                            $scope.workdyas = $scope.numberofdays;
                        }

                        $scope.hrmE_Relaxtime = promise.selectedActivityDetails1_stf[j].hrmE_Relaxtime;

                        //LATE IN EARLY OUT DETAILS
                        var LateInEarlyOutDetails = $filter('filter')(promise.staffDetails, function (d) {
                            return d.HRME_Id === promise.selectedActivityDetails1_stf[j].hrmE_Id;
                        });
                        var newCol = {};
                        if (LateInEarlyOutDetails.length > 0) {
                            for (var i = 0; i < LateInEarlyOutDetails.length; i++) {
                                if (i == 0) {
                                    if (LateInEarlyOutDetails[i].FOEPD_InOutFlg == "I" && LateInEarlyOutDetails[i].DaysLop == 0 && LateInEarlyOutDetails[i].lateby != "00:00") {
                                        //$scope.summinutes = LateInEarlyOutDetails[i].UpdatedLateins;
                                        $scope.summinutes = $scope.makeminutes(LateInEarlyOutDetails[i].lateby);
                                        $scope.lateindates = new Date(LateInEarlyOutDetails[i].punchdate).getDate();
                                        if ($scope.summinutes > $scope.hrmE_Relaxtime) {
                                            $scope.lateindateafter = new Date(LateInEarlyOutDetails[i].punchdate).getDate();
                                            $scope.latecomingcount = $scope.latecomingcount + 1;
                                            $scope.emptotlop = parseFloat(0.5);
                                        }
                                    }
                                    if (LateInEarlyOutDetails[i].FOEPD_InOutFlg == "O" && LateInEarlyOutDetails[i].DaysLop == 0 && LateInEarlyOutDetails[i].earlyby != "00:00") {
                                        $scope.summinutes = $scope.makeminutes(LateInEarlyOutDetails[i].earlyby);
                                        $scope.earlyoutdates = new Date(LateInEarlyOutDetails[i].punchdate).getDate();
                                        if ($scope.summinutes > $scope.hrmE_Relaxtime) {
                                            $scope.earlyoutdateafter = new Date(LateInEarlyOutDetails[i].punchdate).getDate();
                                            $scope.earlyoutcount = $scope.earlyoutcount + 1;
                                            $scope.emptotlop = parseFloat(0.5);
                                        }
                                    }
                                }
                                else {
                                    if (LateInEarlyOutDetails[i].FOEPD_InOutFlg == "I" && LateInEarlyOutDetails[i].DaysLop == 0 && LateInEarlyOutDetails[i].lateby != "00:00") {
                                        $scope.summinutes = parseInt($scope.summinutes) + parseInt($scope.makeminutes(LateInEarlyOutDetails[i].lateby));
                                        //$scope.summinutes = parseInt($scope.summinutes) + parseInt(LateInEarlyOutDetails[i].UpdatedLateins);
                                        if ($scope.lateindates == "") {
                                            $scope.lateindates = new Date(LateInEarlyOutDetails[i].punchdate).getDate();
                                        }
                                        else {
                                            $scope.lateindates = $scope.lateindates + ", " + new Date(LateInEarlyOutDetails[i].punchdate).getDate();
                                        }

                                        if ($scope.summinutes > $scope.hrmE_Relaxtime) {
                                            if ($scope.lateindateafter == "") {
                                                $scope.lateindateafter = new Date(LateInEarlyOutDetails[i].punchdate).getDate();
                                            }
                                            else {
                                                $scope.lateindateafter = $scope.lateindateafter + ", " + new Date(LateInEarlyOutDetails[i].punchdate).getDate();
                                            }

                                            $scope.latecomingcount = $scope.latecomingcount + 1;
                                            $scope.emptotlop = parseFloat($scope.emptotlop) + 0.5;
                                        }
                                    }
                                    if (LateInEarlyOutDetails[i].FOEPD_InOutFlg == "O" && LateInEarlyOutDetails[i].DaysLop == 0 && LateInEarlyOutDetails[i].earlyby != "00:00") {
                                        $scope.summinutes = $scope.summinutes + $scope.makeminutes(LateInEarlyOutDetails[i].earlyby);
                                        if ($scope.earlyoutdates == "") {
                                            $scope.earlyoutdates = new Date(LateInEarlyOutDetails[i].punchdate).getDate();
                                        }
                                        else {
                                            $scope.earlyoutdates = $scope.earlyoutdates + ", " + new Date(LateInEarlyOutDetails[i].punchdate).getDate();
                                        }
                                        if ($scope.summinutes > $scope.hrmE_Relaxtime) {
                                            if ($scope.earlyoutdateafter == "") {
                                                $scope.earlyoutdateafter = new Date(LateInEarlyOutDetails[i].punchdate).getDate();
                                            } else {
                                                $scope.earlyoutdateafter = $scope.earlyoutdateafter + ", " + new Date(LateInEarlyOutDetails[i].punchdate).getDate();
                                            }

                                            $scope.earlyoutcount = $scope.earlyoutcount + 1;
                                            $scope.emptotlop = parseFloat($scope.emptotlop) + 0.5;
                                        }
                                    }
                                }

                                if (i < LateInEarlyOutDetails.length - 1) {
                                    if (LateInEarlyOutDetails[i].HRME_Id == LateInEarlyOutDetails[i + 1].HRME_Id)
                                        continue;
                                }

                                newCol = { earlyout: $scope.earlyoutcount, latecoming: $scope.latecomingcount, latedates: $scope.lateindates, latedateafter: $scope.lateindateafter, earlydates: $scope.earlyoutdates, earlydateafter: $scope.earlyoutdateafter };

                                temp_array.push(newCol);
                            }
                        }
                        else {
                            newCol = { earlyout: 0, latecoming: 0, latedates: "", latedateafter: "", earlydates: "", earlydateafter: "" };
                            temp_array.push(newCol);
                        }
                        //LATE IN EARLY OUT DETAILS

                        //DAILY REPORT DETAILS
                        var DailyrptDetails = $filter('filter')(promise.dailyreptdetails, function (d) {
                            return d.HRME_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id;
                        });

                        $scope.dailyrptncount = DailyrptDetails.length;
                        $scope.dailyrptndates = "";
                        for (var i = 0; i < DailyrptDetails.length; i++) {
                            if ($scope.dailyrptndates == "") {
                                $scope.dailyrptndates = new Date(DailyrptDetails[i].DrNotSentDates).getDate();
                            } else {
                                $scope.dailyrptndates = $scope.dailyrptndates + ", " + new Date(DailyrptDetails[i].DrNotSentDates).getDate();
                            }
                            $scope.emptotlop = parseFloat($scope.emptotlop) + 0.5;
                        }
                        //DAILY REPORT DETAILS                        


                        //DR Rejected LOP start
                        if (promise.rejectedDrList != null && promise.rejectedDrList != undefined && promise.rejectedDrList.length > 0) {
                            var DailyrptRejectedDetails = $filter('filter')(promise.rejectedDrList, function (d) {
                                return d.HRME_ID == promise.selectedActivityDetails1_stf[j].hrmE_Id;
                            });

                            $scope.rejdailyrptncount = DailyrptRejectedDetails.length;
                            $scope.rejdailyrptndates = "";
                            for (var i = 0; i < DailyrptRejectedDetails.length; i++) {
                                if ($scope.rejdailyrptndates == "") {
                                    $scope.rejdailyrptndates = new Date(DailyrptRejectedDetails[i].punchdate).getDate();
                                } else {
                                    $scope.rejdailyrptndates = $scope.rejdailyrptndates + ", " + new Date(DailyrptRejectedDetails[i].punchdate).getDate();
                                }
                                $scope.emptotlop = parseFloat($scope.emptotlop) + 0.5;
                            }

                        }
                        //DR Rejected LOP end


                        //UNAUTHORISED LEAVE REPORT DETAILS
                        var leaverptDetails = $filter('filter')(promise.staffleaveDetails, function (d) {
                            return d.HRME_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id;
                        });

                        $scope.unauthleavedates = "";
                        $scope.unauthleavedatestemp = "";
                        $scope.unauthleavedatesdisplay = "";
                        $scope.unauthleavecount = 0;

                        if (leaverptDetails.length > 0 && $scope.workdyas != 0) {
                            //$scope.unauthleavecount = leaverptDetails.length;
                            //$scope.emptotlop = parseFloat($scope.emptotlop) + leaverptDetails.length;

                            angular.forEach(leaverptDetails, function (check) {
                                if (check.Approvedday === 0) {
                                    $scope.unauthleavecount += parseFloat(check.Absentday);
                                }
                            });
                            $scope.emptotlop = parseFloat($scope.emptotlop) + parseFloat($scope.unauthleavecount);

                            for (var i = 0; i < leaverptDetails.length; i++) {
                                if ($scope.unauthleavedates == "") {
                                    $scope.unauthleavedates = new Date(leaverptDetails[i].DrNotSentDates).getDate();
                                    if (leaverptDetails[i].Approvedday === 0) {
                                        $scope.unauthleavedatesdisplay = new Date(leaverptDetails[i].DrNotSentDates).getDate() + "(F)";
                                        $scope.unauthleavedatestemp = new Date(leaverptDetails[i].DrNotSentDates).getDate().toString();
                                    }
                                }
                                else {
                                    $scope.unauthleavedates = $scope.unauthleavedates + ", " + new Date(leaverptDetails[i].DrNotSentDates).getDate();
                                    if (leaverptDetails[i].Approvedday === 0 && $scope.unauthleavedatesdisplay != "") {
                                        $scope.unauthleavedatesdisplay = $scope.unauthleavedatesdisplay + ", " + new Date(leaverptDetails[i].DrNotSentDates).getDate() + "(F)";
                                        $scope.unauthleavedatestemp = $scope.unauthleavedatestemp + ", " + new Date(leaverptDetails[i].DrNotSentDates).getDate();
                                    }
                                    else if (leaverptDetails[i].Approvedday === 0 && $scope.unauthleavedatesdisplay === "") {
                                        $scope.unauthleavedatesdisplay = new Date(leaverptDetails[i].DrNotSentDates).getDate() + "(F)";
                                        $scope.unauthleavedatestemp = new Date(leaverptDetails[i].DrNotSentDates).getDate().toString();
                                    }
                                }
                            }
                        }
                        //UNAUTHORISED LEAVE REPORT DETAILS
                        //Working Hours mismatch LOP
                        var halffullDetails = $filter('filter')(promise.staffDetails, function (d) {
                            return d.HRME_Id === promise.selectedActivityDetails1_stf[j].hrmE_Id;
                        });

                        var leavesapproved = $filter('filter')(promise.leaveDetails, function (d) {
                            return d.HRME_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id;
                        });

                        if (halffullDetails.length > 0 && $scope.workdyas != 0) {
                            for (var i = 0; i < halffullDetails.length; i++) {
                                if ($scope.unauthleavecount > 1) {
                                    $scope.unauthleavedatesrevised = $scope.unauthleavedates.replace(/[\s]/g, '');
                                    arrhlftwo = $scope.unauthleavedatesrevised.split(',');
                                }
                                if (leavesapproved.length > 0) {
                                    if (arrhlftwo.indexOf(new Date(halffullDetails[i].punchdate).getDate().toString()) < 0) {
                                        var included = $filter('filter')(leavesapproved, function (d) {
                                            return d.DrNotSentDates == halffullDetails[i].punchdate;
                                        });
                                        if (included.length == 0) {
                                            if (halffullDetails[i].DaysLop == "H") {
                                                $scope.unauthleavecount = parseFloat($scope.unauthleavecount) + 0.5;
                                                $scope.emptotlop = parseFloat($scope.emptotlop) + 0.5;
                                                if ($scope.unauthleavedates == "") {
                                                    $scope.unauthleavedates = new Date(halffullDetails[i].punchdate).getDate();
                                                    $scope.unauthleavedatesdisplay = new Date(halffullDetails[i].punchdate).getDate() + "(H)";
                                                    $scope.addholidaylop(new Date(halffullDetails[i].punchdate).getDate().toString(), 'H');
                                                }
                                                else {
                                                    $scope.unauthleavedates = $scope.unauthleavedates + ", " + new Date(halffullDetails[i].punchdate).getDate();
                                                    $scope.unauthleavedatesdisplay = $scope.unauthleavedatesdisplay + ", " + new Date(halffullDetails[i].punchdate).getDate() + "(H)";
                                                    $scope.addholidaylop(new Date(halffullDetails[i].punchdate).getDate().toString(), 'H');
                                                }
                                            }
                                            else if (halffullDetails[i].DaysLop == "F") {
                                                //&& halffullDetails[i].FOEPD_InOutFlg != "" && halffullDetails[i].DaysLop != null
                                                $scope.unauthleavecount = parseFloat($scope.unauthleavecount) + 1;
                                                $scope.emptotlop = parseFloat($scope.emptotlop) + 1;
                                                if ($scope.unauthleavedates == "") {
                                                    $scope.unauthleavedates = new Date(halffullDetails[i].punchdate).getDate();
                                                    $scope.unauthleavedatesdisplay = new Date(halffullDetails[i].punchdate).getDate() + "(F)";
                                                    $scope.addholidaylop(new Date(halffullDetails[i].punchdate).getDate().toString(), 'F');
                                                }
                                                else {
                                                    $scope.unauthleavedates = $scope.unauthleavedates + ", " + new Date(halffullDetails[i].punchdate).getDate();
                                                    $scope.unauthleavedatesdisplay = $scope.unauthleavedatesdisplay + ", " + new Date(halffullDetails[i].punchdate).getDate() + "(F)";
                                                    $scope.addholidaylop(new Date(halffullDetails[i].punchdate).getDate().toString(), 'F');
                                                }
                                            }
                                        }

                                    }
                                }
                                else {
                                    if (arrhlftwo.indexOf(new Date(halffullDetails[i].punchdate).getDate().toString()) < 0) {
                                        if (halffullDetails[i].DaysLop == "H") {
                                            //&& halffullDetails[i].FOEPD_InOutFlg != "" && halffullDetails[i].DaysLop != null
                                            $scope.unauthleavecount = parseFloat($scope.unauthleavecount) + 0.5;
                                            $scope.emptotlop = parseFloat($scope.emptotlop) + 0.5;
                                            if ($scope.unauthleavedates == "") {
                                                $scope.unauthleavedates = new Date(halffullDetails[i].punchdate).getDate();
                                                $scope.unauthleavedatesdisplay = new Date(halffullDetails[i].punchdate).getDate() + "(H)";
                                                $scope.addholidaylop(new Date(halffullDetails[i].punchdate).getDate().toString(), 'H');
                                            }
                                            else {
                                                $scope.unauthleavedates = $scope.unauthleavedates + ", " + new Date(halffullDetails[i].punchdate).getDate();
                                                $scope.unauthleavedatesdisplay = $scope.unauthleavedatesdisplay + ", " + new Date(halffullDetails[i].punchdate).getDate() + "(H)";
                                                $scope.addholidaylop(new Date(halffullDetails[i].punchdate).getDate().toString(), 'H');
                                            }
                                        }
                                        else if (halffullDetails[i].DaysLop == "F") {
                                            //&& halffullDetails[i].FOEPD_InOutFlg != "" && halffullDetails[i].DaysLop != null
                                            $scope.unauthleavecount = parseFloat($scope.unauthleavecount) + 1;
                                            $scope.emptotlop = parseFloat($scope.emptotlop) + 1;
                                            if ($scope.unauthleavedates == "") {
                                                $scope.unauthleavedates = new Date(halffullDetails[i].punchdate).getDate();
                                                $scope.unauthleavedatesdisplay = new Date(halffullDetails[i].punchdate).getDate() + "(F)";
                                                $scope.addholidaylop(new Date(halffullDetails[i].punchdate).getDate().toString(), 'F');
                                            }
                                            else {
                                                $scope.unauthleavedates = $scope.unauthleavedates + ", " + new Date(halffullDetails[i].punchdate).getDate();
                                                $scope.unauthleavedatesdisplay = $scope.unauthleavedatesdisplay + ", " + new Date(halffullDetails[i].punchdate).getDate() + "(F)";
                                                $scope.addholidaylop(new Date(halffullDetails[i].punchdate).getDate().toString(), 'F');
                                            }
                                        }
                                    }
                                }

                            }
                        }

                        if ($scope.unauthleavecount > 1) {
                            $scope.unauthleavedatesrevised = $scope.unauthleavedates.replace(/[\s]/g, '');
                            arrhlf = $scope.unauthleavedatesrevised.split(',');
                        }
                        else { $scope.unauthleavedatesrevised = $scope.unauthleavedates; }



                        //Working Hours mismatch LOP


                        //Holiday Counts
                        $scope.holidayleavedates = "";
                        $scope.holidayleavedatesdisplay = "";
                        $scope.holidayleavecount = 0;
                        if (leaverptDetails.length > 1 && $scope.unauthleavedatestemp != "" && $scope.workdyas != 0) {
                            $scope.holidayliststring = "";
                            angular.forEach(promise.holidaylist, function (hl) {
                                if ($scope.holidayliststring == "") { $scope.holidayliststring = new Date(hl.fomhwdD_FromDate).getDate() + " " }
                                else { $scope.holidayliststring = $scope.holidayliststring + ", " + new Date(hl.fomhwdD_FromDate).getDate(); }
                            });

                            $scope.holidaydatesrevised = $scope.holidayliststring.replace(/[\s]/g, '');
                            var arr2 = $scope.holidaydatesrevised.split(',');

                            for (var i = 0; i < promise.holidaylist.length; i++) {
                                var holidaydate = new Date(promise.holidaylist[i].fomhwdD_FromDate).getDate();
                                //console.log(holidaydate - 1);
                                //console.log($scope.unauthleavedates.includes(holidaydate - 1));
                                $scope.unauthleavedatesrevised = $scope.unauthleavedatestemp.replace(/[\s]/g, '');
                                var arr = $scope.unauthleavedatesrevised.split(',');

                                if ((arr2.indexOf((holidaydate + 1).toString()) > -1)) {
                                    if ((arr.indexOf((holidaydate - 1).toString()) > -1) && (arr.indexOf((holidaydate + 2).toString()) > -1)) {
                                        $scope.holidayleavecount = $scope.holidayleavecount + 2;
                                        $scope.holidayleavedates = $scope.holidayleavedates + ", " + new Date(promise.holidaylist[i].fomhwdD_FromDate).getDate(); $scope.holidayleavedatesdisplay = $scope.holidayleavedatesdisplay + ", " + new Date(promise.holidaylist[i].fomhwdD_FromDate).getDate() + "(P)";
                                        $scope.holidayleavedates = $scope.holidayleavedates + ", " + (parseInt(new Date(promise.holidaylist[i].fomhwdD_FromDate).getDate()) + 1).toString();
                                        $scope.holidayleavedatesdisplay = $scope.holidayleavedatesdisplay + ", " + (parseInt(new Date(promise.holidaylist[i].fomhwdD_FromDate).getDate()) + 1).toString() + "(P)";
                                        i = i + 1;
                                    }
                                }
                                else if ((arr.indexOf((holidaydate - 1).toString()) > -1) && (arr.indexOf((holidaydate - 2).toString()) > -1) && (arr.indexOf((holidaydate - 3).toString()) > -1)) {
                                    $scope.holidayleavecount = $scope.holidayleavecount + 1;
                                    $scope.holidayleavedates = $scope.holidayleavedates + ", " + new Date(promise.holidaylist[i].fomhwdD_FromDate).getDate();
                                    $scope.holidayleavedatesdisplay = $scope.holidayleavedatesdisplay + ", " + new Date(promise.holidaylist[i].fomhwdD_FromDate).getDate() + "(P)";
                                }
                                else if ((arr.indexOf((holidaydate + 1).toString()) > -1) && (arr.indexOf((holidaydate + 2).toString()) > -1) && (arr.indexOf((holidaydate + 3).toString()) > -1)) {
                                    $scope.holidayleavecount = $scope.holidayleavecount + 1;
                                    $scope.holidayleavedates = $scope.holidayleavedates + ", " + new Date(promise.holidaylist[i].fomhwdD_FromDate).getDate();
                                    $scope.holidayleavedatesdisplay = $scope.holidayleavedatesdisplay + ", " + new Date(promise.holidaylist[i].fomhwdD_FromDate).getDate() + "(P)";
                                }
                                else if ((arr.indexOf((holidaydate - 1).toString()) > -1) && (arr.indexOf((holidaydate + 1).toString()) > -1)) {
                                    $scope.holidayleavecount = $scope.holidayleavecount + 1;
                                    $scope.holidayleavedates = $scope.holidayleavedates + ", " + new Date(promise.holidaylist[i].fomhwdD_FromDate).getDate();
                                    $scope.holidayleavedatesdisplay = $scope.holidayleavedatesdisplay + ", " + new Date(promise.holidaylist[i].fomhwdD_FromDate).getDate() + "(P)";
                                }
                                //console.log((arr.indexOf((holidaydate - 1).toString()) > -1));
                            }

                            $scope.unauthleavedates = $scope.unauthleavedates + $scope.holidayleavedates;
                            $scope.unauthleavedatesdisplay = $scope.unauthleavedatesdisplay + $scope.holidayleavedatesdisplay;
                            $scope.unauthleavecount = parseFloat($scope.unauthleavecount) + parseFloat($scope.holidayleavecount);
                            $scope.emptotlop = parseFloat($scope.emptotlop) + $scope.holidayleavecount;
                        }
                        //Holiday Counts

                        //DOJ CHECKING
                        $scope.latejoin = 0;
                        $scope.newjoin = false;
                        var dateofjoin = new Date(promise.selectedActivityDetails1_stf[j].hrmE_DOJ);
                        if (((dateofjoin.getMonth() + 1) == $scope.month) && (dateofjoin.getFullYear() == $scope.year)) {
                            $scope.latejoin = (dateofjoin.getDate() - 1);

                        }
                        if (dateofjoin >= $scope.fromdate && dateofjoin <= $scope.todate) {
                            $scope.newjoin = true;
                        }
                        //DOJ CHECKING

                        //HALF DAY DAILY REPORT
                        var HalfdayDailyrpt = $filter('filter')(promise.halfdaydrdata, function (d) {
                            return d.hrmE_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id;
                        });
                        $scope.dailyrpthalfdaycount = 0;
                        $scope.dailyrpthalfdaydates = "";


                        //APPROVED LEAVE DETAILS
                        var leave_array = [];
                        var leave_arraytemp = [];
                        var approvedleave = $filter('filter')(promise.leaveDetails, function (d) {
                            return d.HRME_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id;
                        });
                        var newleave = {};
                        var newleavetype = [];
                        var appleavedates = "";
                        if (approvedleave != null && approvedleave.length > 0) {
                            angular.forEach(approvedleave, function (lv) {
                                if (newleavetype == undefined || newleavetype.length == 0) {
                                    newleavetype.push(lv);
                                }
                                else {
                                    var checkleavetype = $filter('filter')(newleavetype, function (d) {
                                        return d.leavename == lv.leavename;
                                    });
                                    if (checkleavetype.length == 0) {
                                        newleavetype.push(lv);
                                    }
                                }
                            });
                        }
                        var tempnewleavenew = [];
                        var newsalarytemp = {};
                        if (newleavetype.length > 0) {
                            angular.forEach(newleavetype, function (add) {
                                var leaveasadd = 0;
                                var leavesshowdates = "";
                                angular.forEach(approvedleave, function (leaves) {
                                    if (add.leavename == leaves.leavename) {
                                        leaveasadd += parseFloat(leaves.Approvedday);
                                        if (leavesshowdates == "") {
                                            leavesshowdates = new Date(leaves.DrNotSentDates).getDate() + " (" + leaves.Approvedday + ")";
                                        }
                                        else {
                                            leavesshowdates = leavesshowdates + "," + new Date(leaves.DrNotSentDates).getDate() + " (" + leaves.Approvedday + ")";
                                        }
                                    }
                                });
                                newsalarytemp = { approvedleavetype: add.leavename, approvedleavedays: leaveasadd, leavedate: leavesshowdates };
                                tempnewleavenew.push(newsalarytemp);
                                newsalarytemp = {};
                            });
                        }

                        leave_arraytemp = tempnewleavenew;
                        for (var i = 0; i < approvedleave.length; i++) {
                            newleave = { approvedleavetype: approvedleave[i].leavename, approvedleavedays: approvedleave[i].Approvedday, leavedate: new Date(approvedleave[i].DrNotSentDates).getDate() };
                            leave_array.push(newleave);
                            newleave = {};
                        }


                        if (leave_array.length > 0) {
                            $scope.approvedsum = 0;
                            if (approvedleave.length > 0) {
                                angular.forEach(approvedleave, function (emp) {
                                    $scope.approvedsum += parseFloat(emp.Approvedday);
                                });
                            }

                            if ($scope.workdyas == 0) {
                                $scope.emptotlop = parseFloat($scope.emptotlop) - parseFloat($scope.approvedsum);
                            }
                        }

                        //SALARY DETAILS
                        var salary_array = [];
                        var newsalary = {};
                        var salarydetails = $filter('filter')(promise.currentsalary, function (d) {
                            return d.hrmE_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id;
                        });

                        for (var i = 0; i < salarydetails.length; i++) {
                            newsalary = { salaryhead: salarydetails[i].hrmeD_Name, headamount: salarydetails[i].hresD_Amount };
                            salary_array.push(newsalary);
                            newsalary = {};
                        }
                        //SALARY DETAILS

                        //BLOCKED DAILY REPORT
                        var blockedDailyrpt = $filter('filter')(promise.blockeddrdata, function (d) {
                            return d.hrmE_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id;
                        });
                        $scope.dailyrptblockedcount = 0;

                        var exlude = [];
                        $scope.exludeulfromblockdays = $scope.unauthleavedatesdisplay.toString().replace(/[\s]/g, '');
                        exlude = $scope.exludeulfromblockdays.split(',');

                        $scope.dailyrptblockeddates = "";
                        for (var i = 0; i < blockedDailyrpt.length; i++) {

                            var exludeULfromblockdays = $filter('filter')(exlude, function (d) {
                                if (d.toLowerCase().indexOf("f") >= 0) {
                                    return parseInt(d) == new Date(blockedDailyrpt[i].ismdrpT_Date).getDate();
                                }
                            });

                            if (exludeULfromblockdays == null || exludeULfromblockdays.length == 0) {
                                //$scope.emptotlop = parseFloat($scope.emptotlop) + 0.5;
                                $scope.dailyrptblockedcount = $scope.dailyrptblockedcount + 1;
                                if ($scope.dailyrptblockeddates == "") {
                                    $scope.dailyrptblockeddates = new Date(blockedDailyrpt[i].ismdrpT_Date).getDate();
                                } else {
                                    $scope.dailyrptblockeddates = $scope.dailyrptblockeddates + ", " + new Date(blockedDailyrpt[i].ismdrpT_Date).getDate();
                                }
                            }
                        }
                        //BLOCKED DAILY REPORT

                        //HR COMMENT DETAILS
                        if ($scope.emptotlop >= $scope.numberofdays) {
                            $scope.emptotlop = $scope.numberofdays;
                            if ($scope.workdyas == 0 && $scope.newjoin === true) {
                                $scope.emptotlop = $scope.emptotlop - $scope.latejoin;
                            }
                        }
                        var newCol1 = {};
                        if (promise.hrcomments.length > 0) {
                            $scope.hrcomments = promise.hrcomments;
                            var hrcommentDetails = $filter('filter')(promise.hrcomments, function (d) {
                                return d.hrmE_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id;
                            });
                            //HR_LOP: hrcommentDetails[0].vmscrT_HRTLOP
                            if (hrcommentDetails.length > 0) {
                                newCol1 = { empdate: temp_array, hrmE_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, ecode: promise.selectedActivityDetails1_stf[j].hrmE_EmployeeCode, ename: promise.selectedActivityDetails1_stf[j].hrmE_EmployeeFirstName, desgname: promise.selectedActivityDetails1_stf[j].hrmE_PerStreet, drcount: $scope.dailyrptncount, drdates: $scope.dailyrptndates, halfdrcount: $scope.dailyrpthalfdaycount, halfdrdates: $scope.dailyrpthalfdaydates, rejdrdates: $scope.rejdailyrptndates, leavecount: $scope.unauthleavecount, leavedates: $scope.unauthleavedatesdisplay, blkdrcount: $scope.dailyrptblockedcount, blkdrdates: $scope.dailyrptblockeddates, totlop: $scope.emptotlop, HR_EO: hrcommentDetails[0].vmscrT_HREO, HR_DR: hrcommentDetails[0].vmscrT_HRDR, HR_HDR: hrcommentDetails[0].vmscrT_HRHDR, HR_BDR: hrcommentDetails[0].vmscrT_HRBDR, HR_LC: hrcommentDetails[0].vmscrT_HRLC, HR_UL: hrcommentDetails[0].vmscrT_HRUL, HR_LOP: hrcommentDetails[0].vmscrT_HRTLOP, HR_REM: hrcommentDetails[0].vmscrT_Remarks, vmscrT_DOJ: promise.selectedActivityDetails1_stf[j].hrmE_DOJ, approvedleavedetails: leave_array, salarydetails: salary_array, latejoin: $scope.latejoin, worksdaycount: $scope.workdyas, newjoin: $scope.newjoin, leave_arraytemp: leave_arraytemp };
                            }
                            else {
                                //HR_LOP: 0
                                newCol1 = { empdate: temp_array, hrmE_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, ecode: promise.selectedActivityDetails1_stf[j].hrmE_EmployeeCode, ename: promise.selectedActivityDetails1_stf[j].hrmE_EmployeeFirstName, desgname: promise.selectedActivityDetails1_stf[j].hrmE_PerStreet, drcount: $scope.dailyrptncount, drdates: $scope.dailyrptndates, halfdrcount: $scope.dailyrpthalfdaycount, halfdrdates: $scope.dailyrpthalfdaydates, rejdrdates: $scope.rejdailyrptndates, leavecount: $scope.unauthleavecount, leavedates: $scope.unauthleavedatesdisplay, blkdrcount: $scope.dailyrptblockedcount, blkdrdates: $scope.dailyrptblockeddates, totlop: $scope.emptotlop, HR_EO: 0, HR_DR: 0, HR_HDR: 0, HR_BDR: 0, HR_LC: 0, HR_UL: 0, HR_LOP: $scope.emptotlop, HR_REM: '', vmscrT_DOJ: promise.selectedActivityDetails1_stf[j].hrmE_DOJ, approvedleavedetails: leave_array, salarydetails: salary_array, latejoin: $scope.latejoin, worksdaycount: $scope.workdyas, newjoin: $scope.newjoin, leave_arraytemp: leave_arraytemp };
                            }
                        }
                        else {
                            //HR_LOP: 0
                            newCol1 = { empdate: temp_array, hrmE_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, ecode: promise.selectedActivityDetails1_stf[j].hrmE_EmployeeCode, ename: promise.selectedActivityDetails1_stf[j].hrmE_EmployeeFirstName, desgname: promise.selectedActivityDetails1_stf[j].hrmE_PerStreet, drcount: $scope.dailyrptncount, drdates: $scope.dailyrptndates, halfdrcount: $scope.dailyrpthalfdaycount, halfdrdates: $scope.dailyrpthalfdaydates, rejdrdates: $scope.rejdailyrptndates, leavecount: $scope.unauthleavecount, leavedates: $scope.unauthleavedatesdisplay, blkdrcount: $scope.dailyrptblockedcount, blkdrdates: $scope.dailyrptblockeddates, totlop: $scope.emptotlop, HR_EO: 0, HR_DR: 0, HR_HDR: 0, HR_BDR: 0, HR_LC: 0, HR_UL: 0, HR_LOP: $scope.emptotlop, HR_REM: '', vmscrT_DOJ: promise.selectedActivityDetails1_stf[j].hrmE_DOJ, approvedleavedetails: leave_array, salarydetails: salary_array, latejoin: $scope.latejoin, worksdaycount: $scope.workdyas, newjoin: $scope.newjoin, leave_arraytemp: leave_arraytemp };
                        }
                        //HR COMMENT DETAILS

                        temp_array1.push(newCol1);

                        temp_array = [];
                        $scope.earlyoutcount = 0;
                        $scope.latecomingcount = 0;
                        $scope.summinutes = 0;
                        $scope.lateindates = "";
                        $scope.lateindateafter = "";
                        $scope.earlyoutdates = "";
                        $scope.earlyoutdateafter = "";
                        $scope.emptotlop = 0;
                        arrhlf = [];
                    }

                    $scope.monthendreport = temp_array1;

                    if ($scope.monthendreport.length > 0) {
                        $scope.report = true;
                        $('#employeeorder').modal('hide');
                        $('.modal-backdrop').remove();
                        $('#lopemployee').modal('show');
                        $('.modal').on('hide.bs.modal', function (e) {
                            e.stopPropagation();
                            $('body').css('padding-right', '');
                        });
                    }
                    else {
                        swal("No Records Found !");
                        $('#lopemployee').modal('hide');
                        $('.modal-backdrop').remove();
                        $scope.report = false;
                    }
                });
        };


        $scope.viewtask = function () {
            //$window.location.href = 'http://' + HostName + '/#/app/ISM_TaskRegisterReport';
            window.location.href = "http://localhost:57606/#/app/ISM_TaskRegisterReport/";

            $('.modal-backdrop').remove();
        }


        $scope.ViewBlockedDetails = function (dd) {

            var data = {
                "ISMEMN_ID": dd.ISMEMN_ID,
                "ISMBE_Id": dd.ISMBE_Id,
                "ISMBE_Reason": dd.ISMBE_Reason,
            };
            apiService.create("ISM_DailyReportGeneration/ViewBlockedDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.drblockeddetailslist = promise.drblockeddetailslist;
                    if ($scope.drblockeddetailslist !== null && $scope.drblockeddetailslist.length > 0) {
                        $('#blockeddetailslist').modal('show');
                    }
                }
            });
        };

        $scope.updatevisitorscheckoutlist = function (dd) {
            $scope.visitorcheckoutlist = [];
            angular.forEach($scope.visitorcheckoutdetails, function (emp) {
                if (emp.selectedd2 === true) {
                    $scope.visitorcheckoutlist.push({ VMMV_Id: emp.VMMV_Id, VMMV_Remarks: emp.VMMV_Remarks });
                }
            });

            var data = {
                "visitorcheckoutlist": $scope.visitorcheckoutlist
            };
            apiService.create("ISMDashboard/updatevisitorscheckoutlist", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.message == "updated") {
                        swal("Record Updated Successfully");
                        $state.reload();
                    }
                    else if (promise.message == "notUpdated") {
                        swal("Record Not Updated Successfully");
                    }
                }
            });
        };

        $scope.savetrainingR = function () {
            $scope.trainingSelectedList = [];
            angular.forEach($scope.trainingIssue, function (emp) {
                if (emp.selectedT === true) {
                    $scope.trainingSelectedList.push({ TMO_Id: emp.TMO_Id, TMO_Remark: emp.TMO_Remark });
                }
            });
            var data = {
                "trainigProcessIssueR": $scope.trainingSelectedList
            };
            apiService.create("ISMDashboard/savetrainingRemarklist", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.message == "Saved") {
                        swal("Record Saved Successfully");
                        $state.reload();
                    }
                    else if (promise.message == "NotSaved") {
                        swal("Record Not Saved Successfully");
                    }
                }
            });
        };

        $scope.OnClickViewHrPlannerDetails = function () {
            $scope.hrremindersplannerdetails = [];
            $scope.overduehrplannerdetails = [];
            $scope.duehrplannerdetails = [];

            $scope.itemsPerPageoverduereminder = 10;
            $scope.itemsPerPageduereminder = 10;

            $scope.currentPageoverduereminder = 1;
            $scope.currentPagereminder = 1;
            $scope.ISMDRPT_Date = new Date();
            var data = {
                "ISMDRPT_Date": new Date().toDateString()
            };

            apiService.create("ISMDashboard/OnClickViewHrPlannerDetails", data).then(function (promise) {
                $('#myModalhrplanner').modal('show');
                if (promise.hrremindersplannerdetails !== undefined && promise.hrremindersplannerdetails !== null && promise.hrremindersplannerdetails.length > 0) {
                    $scope.hrremindersplannerdetails = promise.hrremindersplannerdetails;

                    angular.forEach($scope.hrremindersplannerdetails, function (dd) {
                        if (dd.HRPDSD_RepeatFlag === "OVER-DUE REMINDER") {
                            $scope.overduehrplannerdetails.push(dd);
                        } else if (dd.HRPDSD_RepeatFlag === "REMINDER") {
                            $scope.duehrplannerdetails.push(dd);
                        }
                    });
                }
                //else {
                //    swal("No Records Found");
                //}
                $scope.employeename_plannerdetails = promise.employeename;
            });
        };

        $scope.DemoGiven = function (user, sub) {
            var data = {
                "IVRMMS_Id": user.IVRMMS_Id,
                "flag": sub,
            };
            apiService.create("ISMDashboard/StatewiseleadDetails/", data).then(function (promise) {
                $scope.obj.sub = sub;
                if (promise.sataewiselead > 0 || promise.sataewiselead !== null) {
                    $scope.sataewiselead = promise.sataewiselead;
                    $scope.IVRMMS_Name = promise.sataewiselead[0].IVRMMS_Name;
                    $scope.Totalcount = promise.sataewiselead.length;
                    $scope.leadcommentsreportDeatilsonline = [];
                    $scope.leadcommentsreportDeatilsoffline = [];
                    angular.forEach($scope.sataewiselead, function (dev) {
                        if (dev.ismsledM_DemoType === 'Online') {
                            $scope.leadcommentsreportDeatilsonline.push(dev);
                        } else {
                            $scope.leadcommentsreportDeatilsoffline.push(dev);
                        }
                    });
                    $scope.offline = $scope.leadcommentsreportDeatilsoffline.length;
                    $scope.online = $scope.leadcommentsreportDeatilsonline.length;

                    $('#myModal1').modal('show');
                }
                else {
                    swal('No Data Found!!!');
                    $scope.leadcommentsreportDeatils = "";
                }

            });
        };



        //QrCodeAPIUpdate
        $scope.QrCodeAPIUpdate = function (e) {
            var sigText = document.getElementById("sig-dataUrl");
            var qrcodee = document.getElementById("qrcode");
            sigText.innerHTML = qrcodee.innerHTML;
            var ddd = sigText.innerHTML;
            var img = ddd;
            var imagarr = img.split('&lt;canvas width="256" height="256" style="display: none;"&gt;&lt;/canvas&gt;&lt;');
            sigText.innerHTML = imagarr[1];
            var data = {
                "HRME_QRCode": sigText.innerHTML = imagarr[1]
            };
            apiService.create("ISMDashboard/QrCodeAPIUpdate", data).then(function (promise) {
                swal("QR Code Saved Sucess");
                $('#QRCodeGenerate').modal('hide');
                $scope.loaddata();
            });
        }

        $scope.OnChangeDateViewHrPlannerDetails = function () {
            $scope.hrremindersplannerdetails = [];
            $scope.overduehrplannerdetails = [];
            $scope.duehrplannerdetails = [];

            $scope.itemsPerPageoverduereminder = 10;
            $scope.itemsPerPageduereminder = 10;
            $scope.currentPageoverduereminder = 1;
            $scope.currentPagereminder = 1;
            var data = {
                "ISMDRPT_Date": new Date($scope.ISMDRPT_Date).toDateString()
            };

            apiService.create("ISMDashboard/OnClickViewHrPlannerDetails", data).then(function (promise) {

                //HR Planner Details
                $scope.hrremindersplannerdetails = [];
                $scope.overduehrplannerdetails = [];
                $scope.duehrplannerdetails = [];
                if (promise.hrremindersplannerdetails !== undefined && promise.hrremindersplannerdetails !== null && promise.hrremindersplannerdetails.length > 0) {
                    $scope.hrremindersplannerdetails = promise.hrremindersplannerdetails;
                    angular.forEach($scope.hrremindersplannerdetails, function (dd) {
                        if (dd.HRPDSD_RepeatFlag === "OVER-DUE REMINDER") {
                            $scope.overduehrplannerdetails.push(dd);
                        } else if (dd.HRPDSD_RepeatFlag === "REMINDER") {
                            $scope.duehrplannerdetails.push(dd);
                        }
                    });
                }
                //else {
                //    swal("No Records Found");
                //}
                $scope.employeename_plannerdetails = promise.employeename;
            });
        };

        $scope.proxydetails = function () {
            $scope.header_list = [];
            $scope.HRME_EmployeeFirstName = $scope.proxyleavedetails[0].HRME_EmployeeFirstName;
            $scope.HRME_EmployeeCode = $scope.proxyleavedetails[0].HRME_EmployeeCode;
            $scope.HRELAP_FromDate = $scope.proxyleavedetails[0].HRELAP_FromDate;
            $scope.HRELAP_ToDate = $scope.proxyleavedetails[0].HRELAP_ToDate;

            var data = {
                "HRME_Id": $scope.proxyleavedetails[0].HRME_Id,
                "HRMPR_Id": 0
            };

            apiService.create("ISMDashboard/employeetasklist", data).then(function (promise) {

                if (promise.prioritylist !== undefined && promise.prioritylist !== null && promise.prioritylist.length > 0) {
                    $scope.proxytasklist = promise.prioritylist;
                    $('#proxyleavedetailsyy').modal('show');

                    for (var i = 0; i < $scope.v.length; i++) {

                        if (i === 0) {
                            angular.forEach($scope.proxytasklist[i], function (key, r) {
                                $scope.header_list.push({ colmn: r, head: key });
                            });
                        }
                    }



                }
                else {
                    swal("No Records Found");
                }
                $scope.employeename_plannerdetails = promise.employeename;
            });



        };


        $scope.OnClickexcel = function (export_id) {
            $scope.stdate = $filter('date')($scope.ISMDRPT_Date, 'dd-MM-yyyy');
            var excelname = "Planner Reminder Details" + $scope.stdate;
            var exportHref = "";
            var excelsheetname = "Planner Reminder Details";
            var reportname = "#dueexcel";


            var blob = new Blob([document.getElementById('dueexcel').innerHTML], {
                type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
            });

            saveAs(blob, excelname + ".xls");


            //excelname = excelsheetname + ' REPORT ' + $scope.stdate;
            //excelname = excelname.toUpperCase() + '.xls';
            //exportHref = Excel.tableToExcel(export_id, excelsheetname);
            //$timeout(function () {
            //    var a = document.createElement('a');
            //    a.href = exportHref;
            //    a.download = excelname;
            //    document.body.appendChild(a);
            //    a.click();
            //    a.remove();
            //}, 100);
        };



        $scope.encryptionadhar = false;
        $scope.encryptadhar = function (export_id) {

            if ($scope.encryptionadhar == false) {
                $scope.encryptionadhar = true;
            }
            else {
                $scope.encryptionadhar = false;
            }
        };

        $scope.encryptionpan = false;
        $scope.encryptpan = function (export_id) {

            if ($scope.encryptionpan == false) {
                $scope.encryptionpan = true;
            }
            else {
                $scope.encryptionpan = false;
            }
        };

        //added By sanjeev
        $scope.allEmployeeAssets = true;
        $scope.OnClickemployeedetails = function (HRME_Id, MI_ID) {
            //$scope.HRME_Id = item.HRME_Id
            $scope.employeeidtwo = [];
            $scope.locationdetails = [];
            $scope.employeeid = [];
            var data = {
                "MI_ID": MI_ID,
                "HRME_Id": HRME_Id,
                "Flag": "Dashboard"

            };
            apiService.create("INV_LocationDashboard/getemployeedetail", data).then(function (promise) {
                $scope.locationdetails = promise.employeedetails;

                $scope.employeeid = [];
                if ($scope.locationdetails != null && $scope.locationdetails.length > 0) {
                    var empName = "";
                    var deptName = "";
                    if ($scope.emp_deatils != null && $scope.emp_deatils.length > 0) {
                        var empObj = $scope.emp_deatils[0];
                        empName = ((empObj.HRME_EmployeeFirstName || "") + " " + (empObj.HRME_EmployeeMiddleName || "") + " " + (empObj.HRME_EmployeeLastName || "")).trim();
                        deptName = empObj.HRMD_DepartmentName || "";
                    }

                    var locationRoom = "";
                    angular.forEach($scope.locationdetails, function (dev) {
                        dev.search = "";
                        dev.popupSearch = "";
                        dev.selected = true;
                        if (!locationRoom && (dev.INVMLO_LocationRoomName || dev.Floor_Name)) {
                            locationRoom = dev.INVMLO_LocationRoomName || dev.Floor_Name;
                        }
                        if (!empName && dev.EmployeeName) {
                            empName = dev.EmployeeName;
                        }
                        if (!deptName && dev.HRMD_DepartmentName) {
                            deptName = dev.HRMD_DepartmentName;
                        }
                        if (!dev.HRME_Id && $scope.emp_deatils != null && $scope.emp_deatils.length > 0) {
                            dev.HRME_Id = $scope.emp_deatils[0].HRME_Id;
                        }
                    });

                    var singleUser = {
                        HRME_Id: ($scope.emp_deatils && $scope.emp_deatils.length > 0) ? $scope.emp_deatils[0].HRME_Id : $scope.locationdetails[0].HRME_Id,
                        EmployeeName: empName || "Employee",
                        HRMD_DepartmentName: deptName,
                        INVMLO_LocationRoomName: locationRoom,
                        RecordDetails: $scope.locationdetails
                    };

                    $scope.employeeid = [singleUser];
                    $scope.templist = $scope.locationdetails;
                    $scope.allEmployeeAssets = true;

                    // Show popup ONLY if there are unconfirmed/pending products assigned
                    var needsConfirmation = promise.hasPendingConfirmation;
                    if (needsConfirmation === undefined || needsConfirmation === null) {
                        needsConfirmation = false;
                        angular.forEach($scope.locationdetails, function (item) {
                            if (item.INVATCO_CheckOutRemarks !== "Confirmed by Employee" && !item.isConfirmed && item.INVATCO_CheckOutRemarks !== "Unselected by Employee" && item.INVATCO_CheckOutRemarks !== "Unselected by IT") {
                                needsConfirmation = true;
                            }
                        });
                    }

                    if (needsConfirmation === true) {
                        $('#employeeAssetConfirmationModal').modal('show');
                    }
                }
                else {

                }
            });

            //sushma
            var dataCommon = {
                "MI_ID": MI_ID || 0,
                "HRME_Id": HRME_Id || 0,
                "INVMLO_Id": 0
            };

            apiService.create("INV_LocationDashboard/getemployeedetailCommon", dataCommon).then(function (promiseCommon) {
                $scope.common = promiseCommon.employeedetailsCommon || [];
                if ($scope.common != null && $scope.common.length > 0) {
                    angular.forEach($scope.common, function (prod) {
                        prod.INVMI_ItemName = prod.INVMI_ItemName || prod['Item Name'] || prod['item_name'] || prod.ItemName || '';
                        prod.INVAAT_AssetId = prod.INVAAT_AssetId || prod['Asset-id'] || prod['Asset-Id'] || prod['Asset_id'] || prod.AssetId || '';
                        prod.TotalAmount = prod.TotalAmount !== undefined ? prod.TotalAmount : (prod['Total Amount'] !== undefined ? prod['Total Amount'] : (prod.INVTGRN_PurchaseRate !== undefined ? prod.INVTGRN_PurchaseRate : (prod.Total_Amount !== undefined ? prod.Total_Amount : 0)));
                        prod.MI_Name = prod.MI_Name || prod['Company Name'] || prod['CompanyName'] || prod.CompanyName || prod.company_name || '';
                        prod.INVATCO_CheckOutQty = prod.INVATCO_CheckOutQty !== undefined ? prod.INVATCO_CheckOutQty : (prod['CheckOut-Qty'] !== undefined ? prod['CheckOut-Qty'] : (prod['CheckOut_Qty'] !== undefined ? prod['CheckOut_Qty'] : (prod.CheckOutQty !== undefined ? prod.CheckOutQty : 1)));
                        prod.INVMLO_LocationRoomName = prod.INVMLO_LocationRoomName || prod['Location RoomName'] || prod['LocationRoomName'] || prod.Room_Name || prod.Location_RoomName || '';
                        prod.INVAAT_SerialNo = prod.INVAAT_SerialNo || prod['Serial.No'] || prod['Serial_No'] || prod.SerialNo || prod.serial_no || '';
                        prod.TotalEmployeesOnFloor = prod.TotalEmployeesOnFloor !== undefined ? prod.TotalEmployeesOnFloor : (prod['Total Employees on Floor'] !== undefined ? prod['Total Employees on Floor'] : (prod.TotalEmployees !== undefined ? prod.TotalEmployees : 0));
                        prod.EmployeeShare = prod.EmployeeShare !== undefined ? prod.EmployeeShare : (prod['Employee Share'] !== undefined ? prod['Employee Share'] : (prod.PerPersonCost !== undefined ? prod.PerPersonCost : (prod.Employee_Share !== undefined ? prod.Employee_Share : 0)));
                        prod.PerPersonCost = prod.PerPersonCost !== undefined ? prod.PerPersonCost : (prod['Employee Share'] !== undefined ? prod['Employee Share'] : (prod['Total Amount'] !== undefined ? prod['Total Amount'] : (prod.TotalAmount !== undefined ? prod.TotalAmount : 0)));
                        prod.INVATCO_Id = prod.INVATCO_Id || prod['INVATCO_Id'] || prod['invatcO_Id'] || prod['AssetTagCheckOutId'] || 0;
                        prod.INVAAT_FilePath = prod.INVAAT_FilePath || prod['FilePath'] || prod['INVAAT_FilePath'] || prod.FilePath || '';
                    });
                }
            });

        };

        $scope.checkAllEmployeeAssets = function (all) {
            $scope.allEmployeeAssets = all;
            if ($scope.employeeid != null && $scope.employeeid.length > 0) {
                angular.forEach($scope.employeeid, function (user) {
                    if (user.RecordDetails && user.RecordDetails.length > 0) {
                        angular.forEach(user.RecordDetails, function (itm) {
                            itm.selected = all;
                        });
                    }
                });
            }
        };

        $scope.optionToggledEmployeeAsset = function () {
            var allSelected = true;
            if ($scope.employeeid != null && $scope.employeeid.length > 0) {
                angular.forEach($scope.employeeid, function (user) {
                    if (user.RecordDetails && user.RecordDetails.length > 0) {
                        angular.forEach(user.RecordDetails, function (itm) {
                            if (!itm.selected) {
                                allSelected = false;
                            }
                        });
                    }
                });
            }
            $scope.allEmployeeAssets = allSelected;
        };

        $scope.saveEmployeeAssetConfirmation = function () {
            var selectedList = [];
            var unselectedList = [];

            if ($scope.employeeid != null && $scope.employeeid.length > 0) {
                angular.forEach($scope.employeeid, function (user) {
                    if (user.RecordDetails && user.RecordDetails.length > 0) {
                        angular.forEach(user.RecordDetails, function (itm) {
                            var itemHrmeId = itm.HRME_Id || user.HRME_Id || 0;
                            var itemMloId = itm.INVMLO_Id || user.INVMLO_Id || 0;
                            var itemInvaatId = itm.INVAAT_Id || 0;
                            var itemInvmiId = itm.INVMI_Id || 0;
                            var itemMiId = itm.MI_Id || (user && user.MI_Id) || 0;

                            if (itm.selected === true) {
                                selectedList.push({
                                    INVATCO_Id: itm.INVATCO_Id || 0,
                                    HRME_Id: itemHrmeId,
                                    INVMLO_Id: itemMloId,
                                    INVAAT_Id: itemInvaatId,
                                    INVMI_Id: itemInvmiId,
                                    MI_Id: itemMiId,
                                    selected: true
                                });
                            } else {
                                unselectedList.push({
                                    INVATCO_Id: itm.INVATCO_Id || 0,
                                    HRME_Id: itemHrmeId,
                                    INVMLO_Id: itemMloId,
                                    INVAAT_Id: itemInvaatId,
                                    INVMI_Id: itemInvmiId,
                                    MI_Id: itemMiId,
                                    selected: false
                                });
                            }
                        });
                    }
                });
            }

            var currentHrmeId = 0;
            var currentMiId = 0;
            if ($scope.emp_deatils != null && $scope.emp_deatils.length > 0) {
                currentHrmeId = $scope.emp_deatils[0].HRME_Id || 0;
                currentMiId = $scope.emp_deatils[0].MI_Id || 0;
            } else if ($scope.employeeid != null && $scope.employeeid.length > 0 && $scope.employeeid[0].HRME_Id > 0) {
                currentHrmeId = $scope.employeeid[0].HRME_Id;
                currentMiId = $scope.employeeid[0].MI_Id || 0;
            } else if ($scope.locationdetails != null && $scope.locationdetails.length > 0 && $scope.locationdetails[0].HRME_Id > 0) {
                currentHrmeId = $scope.locationdetails[0].HRME_Id;
                currentMiId = $scope.locationdetails[0].MI_Id || 0;
            }

            var data = {
                "HRME_Id": currentHrmeId,
                "MI_ID": currentMiId,
                "locationlistarrays": selectedList,
                "unselectedlocationlistarrays": unselectedList
            };

            apiService.create("INV_LocationDashboard/SaveEmployeeAssets", data).then(function (promise) {
                if (promise && (promise.returnval === true || promise.Flag === "save" || promise.flag === "save" || promise.message === "Success")) {
                    swal("Assets Confirmed Successfully!");
                    $('#employeeAssetConfirmationModal').modal('hide');

                    // Mark all remaining items as confirmed
                    angular.forEach($scope.locationdetails, function (item) {
                        item.INVATCO_CheckOutRemarks = "Confirmed by Employee";
                        item.isConfirmed = true;
                    });

                    // Filter out unselected products so only selected products show on home screen
                    angular.forEach($scope.employeeid, function (user) {
                        if (user.RecordDetails && user.RecordDetails.length > 0) {
                            user.RecordDetails = user.RecordDetails.filter(function (item) {
                                return item.selected === true;
                            });
                        }
                    });

                    // Keep employeeid only if they have remaining RecordDetails
                    $scope.employeeid = $scope.employeeid.filter(function (user) {
                        return user.RecordDetails && user.RecordDetails.length > 0;
                    });

                    if ($scope.locationdetails != null && $scope.locationdetails.length > 0) {
                        $scope.locationdetails = $scope.locationdetails.filter(function (item) {
                            return item.selected === true;
                        });
                    }

                    $scope.templist = [];
                    if ($scope.employeeid != null && $scope.employeeid.length > 0) {
                        angular.forEach($scope.employeeid, function (user) {
                            if (user.RecordDetails && user.RecordDetails.length > 0) {
                                $scope.templist = $scope.templist.concat(user.RecordDetails);
                            }
                        });
                    }
                } else {
                    swal("Record Not Saved!");
                }
            });
        };

        $scope.openEmployeeAssetModal = function () {
            $('#employeeAssetConfirmationModal').modal('show');
        };

        //$scope.templist
        $scope.TotalgrossAmount = function () {
            var total = 0;
            if ($scope.templist != null) {
                for (var i = 0; i < $scope.templist.length; i++) {
                    var product = $scope.templist[i];
                    if (product.selected !== false) {
                        total += product.INVTGRN_PurchaseRate || 0;
                    }
                }
            }

            return Math.round(total);
        };

        $scope.TotalgrossAmountCommon = function () {
            var totalCommon = 0;
            if ($scope.common != null && $scope.common.length > 0) {
                for (var j = 0; j < $scope.common.length; j++) {
                    var prod = $scope.common[j];
                    var rate = prod.TotalAmount || prod['Total Amount'] || 0;
                    totalCommon += parseFloat(rate) || 0;
                }
            }
            return (totalCommon % 1 === 0) ? totalCommon : parseFloat(totalCommon.toFixed(2));
        };

        $scope.TotalEmployeeShareCommon = function () {
            var totalShare = 0;
            if ($scope.common != null && $scope.common.length > 0) {
                for (var j = 0; j < $scope.common.length; j++) {
                    var prod = $scope.common[j];
                    var share = prod.EmployeeShare || prod['Employee Share'] || prod.PerPersonCost || 0;
                    totalShare += parseFloat(share) || 0;
                }
            }
            return (totalShare % 1 === 0) ? totalShare : parseFloat(totalShare.toFixed(2));
        };

        $scope.previewimg_new = function (img) {
            $scope.ilrT_FilePath = img;
            var imgUrl = $scope.ilrT_FilePath;
            if (imgUrl != null) {
                var imagarr = imgUrl.split('.');
                var lastelement = imagarr[imagarr.length - 1].toLowerCase();
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#tadapreviewX').attr('src', $scope.ilrT_FilePath);
                    $('#myimagePreviewtada').modal('show');
                }
                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                    $window.open($scope.ilrT_FilePath);
                }
                else if (lastelement == 'pdf') {
                    $('#showpdf').modal('hide');
                    $http.get($scope.ilrT_FilePath, { responseType: 'arraybuffer' })
                        .success(function (response) {
                            var file = new Blob([(response)], { type: 'application/pdf' });
                            var fileURL = URL.createObjectURL(file);
                            var pdfId = document.getElementById("pdfIdzz");
                            if (pdfId && pdfId.childNodes.length > 0) {
                                pdfId.removeChild(pdfId.childNodes[0]);
                            }
                            var embed = document.createElement('embed');
                            embed.setAttribute('src', fileURL);
                            embed.setAttribute('type', 'application/pdf');
                            embed.setAttribute('width', '100%');
                            embed.setAttribute('height', '1000');
                            if (pdfId) pdfId.appendChild(embed);
                            $('#showpdf').modal('show');
                        });
                }
            } else {
                $window.open($scope.ilrT_FilePath);
            }
        };
        //getActiveMembers
        //$scope.departments = function () {

        //    $('#departments').modal('show');
        //}
        //OnClickemployeetwo

        $scope.getActiveMembers = function (item) {
            $scope.employeeidtwo = [];
            $scope.employeedetail = [];
            $scope.Activate = "";
            $scope.Activate = item;
            $scope.employeeidthree = [];
            var data = {
                "Department_Code": $scope.Departmentname[0].HRMDC_Code,
                "Flags": $scope.Activate,
                "TypeFlg": "Onload"
            }
            apiService.create("ISMDashboard/getemployeedetail", data).then(function (promise) {
                if (promise.employeedetails != null && promise.employeedetails.length > 0) {
                    $scope.employeedetailstwo = promise.employeedetails;
                    angular.forEach($scope.employeedetailstwo, function (dev) {
                        if ($scope.employeeidthree.length === 0) {
                            $scope.employeeidthree.push(dev);
                        }
                        else if ($scope.employeeidthree.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.employeeidthree, function (emp) {
                                if (emp.HRME_Id === dev.HRME_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.employeeidthree.push(dev);
                            }
                        }
                    });
                    angular.forEach($scope.employeeidthree, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.employeedetailstwo, function (dd) {
                            if (dd.HRME_Id === ddd.HRME_Id) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.RecordDetails = $scope.templist;

                    });
                    if ($scope.Activate == "DeActive") {
                        $scope.Departmentname = [];
                        $scope.Departmentname = promise.departmentname;
                    }
                    $('#departments').modal('show');
                }

            });

        }
        $scope.Closeclick = function () {
            $scope.employeeidtwo = [];
            $scope.loaddata();
        }
        $scope.OnClickemployeetwo = function (item) {
            $scope.employeedetail = [];
            $scope.employeeidtwo = [];
            var data = {
                "Department_Code": item.HRMDC_Code,
                "Flags": $scope.Activate,
            }
            apiService.create("ISMDashboard/getemployeedetail", data).then(function (promise) {
                $scope.employeedetail = promise.employeedetails;
                if ($scope.employeedetail != null && $scope.employeedetail.length > 0) {
                    angular.forEach($scope.employeedetail, function (dev) {
                        if ($scope.employeeidtwo.length === 0) {
                            $scope.employeeidtwo.push(dev);
                        }
                        else if ($scope.employeeidtwo.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.employeeidtwo, function (emp) {
                                if (emp.HRME_Id === dev.HRME_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.employeeidtwo.push(dev);
                            }
                        }
                    });

                    angular.forEach($scope.employeeidtwo, function (ddd) {
                        $scope.templistwo = [];
                        angular.forEach($scope.employeedetail, function (dd) {
                            if (dd.HRME_Id === ddd.HRME_Id) {
                                $scope.templistwo.push(dd);
                            }
                        });
                        ddd.RecordDetailsIsm = $scope.templistwo;

                    });
                }

            });



        };
        $scope.leavedata = function () {
            var id = 2;
            $scope.get_leavestatus = [];
            apiService.getURI("LeaveApproval/getApprovalStatus", id).then(function (promise) {
                if (promise.get_leavestatus != null && promise.get_leavestatus.length > 0) {
                    $scope.get_leavestatus = promise.get_leavestatus;
                    $('#ViewdashboardleavedetailsModal').modal('show');
                }

            });
        };

        //added by prveenkumar mmd approval oa
        $scope.leavedata = function () {
            var id = 2;
            $scope.get_leavestatus = [];
            apiService.getURI("Lead_mou_/OnloadGetDetails", id).then(function (promise) {
                if (promise.signedmoudetails != null && promise.signedmoudetails.length > 0) {
                    $scope.Signedmoudetails = promise.signedmoudetails;
                    $scope.User_id = promise.user_Id
                    if ($scope.User_id == 61162) {
                        $('#ViewdashboardleavedetailsModalOA').modal('show');

                    }
                    else {
                        $('#ViewdashboardleavedetailsModalOA').modal('hide');
                    }

                }

            });
        };

        $scope.RedirectToLeave = function () {

            window.location.href = "http://localhost:57606/#/app/LeaveApproval/598";

            $('.modal-backdrop').remove();
        };

        $scope.RedirectToapproval = function () {

            window.location.href = "http://localhost:57606/#/app/ISM_LeadOA_Approval/598";

            $('.modal-backdrop').remove();
        };


        $scope.Consultantreport = function (IncomeTax) {
            var data = {
                "VMSSC_Flag": IncomeTax,

            }
            apiService.create("StatutoryTaxCompliance/Consultantreport", data).then(function (promise) {
                $scope.Consultant_Report = promise.consultant_Report
            });
        };
        $scope.Incomecasereport = function (IncomeTax) {
            var data = {
                "VMSSC_Flag": IncomeTax
            }
            apiService.create("StatutoryTaxCompliance/Casereports", data).then(function (promise) {
                $scope.Income_CaseList = promise.income_CaseList
                $scope.Income_CaseListtwo = promise.income_CaseListtwo
                $scope.Income_CaseListthree = promise.income_CaseListthree
                $scope.Income_CaseListfour = promise.income_CaseListfour
                $scope.Income_CaseListfive = promise.income_CaseListfive
                $scope.Income_CaseListsix = promise.income_CaseListsix
                $scope.Income_CaseListseven = promise.income_CaseListseven
            });
        };
        $scope.Incomeclosecasereport = function (IncomeTax) {
            var data = {
                "VMSSC_Flag": IncomeTax
            }
            apiService.create("StatutoryTaxCompliance/ClosedCasereports", data).then(function (promise) {
                $scope.Income_ClosedCaseList = promise.income_ClosedCaseList
                $scope.Income_ClosedCaseListtwo = promise.income_ClosedCaseListtwo
                $scope.Income_ClosedCaseListthree = promise.income_ClosedCaseListthree
                $scope.Income_ClosedCaseListfour = promise.income_ClosedCaseListfour
                $scope.Income_ClosedCaseListfive = promise.income_ClosedCaseListfive
                $scope.Income_ClosedCaseListsix = promise.income_ClosedCaseListsix
                $scope.Income_ClosedCaseListseven = promise.income_ClosedCaseListseven
            });
        };
        $scope.ViewUploadFiles = function (obj) {

            var data = {
                "VMSSC_Id": obj.VMSSC_Id,

            };

            apiService.create("StatutoryTaxCompliance/ViewFiles", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getviewfilesdetails !== null && promise.getviewfilesdetails.length > 0) {
                        $scope.getviewfilesdetails = promise.getviewfilesdetails;
                        angular.forEach($scope.getviewfilesdetails, function (dd) {
                            if (dd.vmsscD_Filepath !== null && dd.vmsscD_Filepath !== "") {
                                var img = dd.vmsscD_Filepath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("dd.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.VMSSCD_Filepath;
                                }
                            }
                        });
                        $('#modalviewfiles').modal('show');
                    } else {
                        swal("No Records Found");
                        $('#modalviewfiles').modal('hide');
                    }
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

        $scope.ViewDocExcelfles = function (docpath, filename, filetype) {
            docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + docpath;
            $scope.detailFrame = $sce.trustAsResourceUrl(docpath);
            $('#myModaldocx').modal('show');
        };

        $scope.downloaddirectimage = function (data, idd, type) {
            var studentreg = idd;
            $scope.imagedownload = data;
            var imagedownload = data;
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

        $scope.printToCart = function () {
            var innerContents = document.getElementById("bond").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        //dinner

        $scope.previewimg_new = function (img) {
            $scope.vbabrsF_FilePath = img;
            var img = $scope.vbabrsF_FilePath;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.vbabrsF_FilePath);
                    $('#myimagePreview').modal('show');
                }
                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                    $window.open($scope.vbabrsF_FilePath);
                }
                else if (lastelement == 'pdf') {
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.vbabrsF_FilePath;
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
            }
            else {
                $window.open($scope.vbabrsF_FilePath)
            }
        };

        $scope.DinnerOrdereList = function () {

            var data = {};

            apiService.create("ISMDashboard/dinnerorderlist", data).then(function (promise) {
                if (promise.message == "list") {
                    $scope.currentPagecount = 1;
                    $scope.itemsPerPagecount = 10;
                    $('#dinnerlist').modal('show');
                    $scope.orderlist = promise.dinnerorderlist;
                    $scope.ordercount = promise.dinnerorderlist.length;

                }
                else { swal("NO Order Found"); }

            });
        }

        $scope.OrdereDinner = function () {


            swal({
                title: "Are you sure?",
                text: "Do You Want To Order Dinner?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes Order Dinner",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: true
            },
                function (isConfirm) {
                    if (isConfirm) {

                        var data = {};

                        apiService.create("ISMDashboard/OrdereDinner", data).then(function (promise) {
                            if (promise.message == "Success") {
                                swal("Order Placed Successfully..!");

                            }
                            else if (promise.message == "Fail") {
                                swal("You Can't Order Now..!");
                            } else if (promise.message == "Exist") {
                                swal("You Already Ordered!");
                            }
                            else {
                                swal("Somthing Went Wrong");
                            }
                        });
                    }




                });


        }

        //dinner
    }
})();