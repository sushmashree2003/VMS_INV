(function () {
    'use strict';

    angular
        .module('app')
        .controller('FrontOfficeMonthEndReportApproval', FrontOfficeMonthEndReportApproval);

    FrontOfficeMonthEndReportApproval.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', '$q']

    function FrontOfficeMonthEndReportApproval($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, $q) {

        $scope.takescreenshot = false;
        $scope.hrcommentflag = false;
        $scope.disablemonth = false;
        $scope.ddate = new Date();
        //$scope.maxDatemf = new Date();
        //$scope.maxDatemf.setDate($scope.maxDatemf.getDate() - 1);
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

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;


        $scope.setItemsPerPage = function (num) {
            $scope.itemsPerPage = num;
            $scope.currentPage = 1;
        };


        $scope.optionchange = function () {
            $scope.monthendreport = [];
            $scope.report = false;
            if ($scope.allind == "Day") {
                $scope.maxDatemf = new Date();
                $scope.maxDatemf.setDate($scope.maxDatemf.getDate() - 1);
            }
            else {
                $scope.maxDatemf = "";
            }
        };

        $scope.gettodate = function () {
            $scope.minDatemf = $scope.fromdate;
        };

        $scope.getchanged = function (yr, mon) {
            $scope.griddatalist = [];
            $scope.report = false;
            $scope.monthname = "";
            angular.forEach($scope.fillmonth, function (check) {
                if (check.ivrM_Month_Id == mon) {
                    $scope.monthname = check.monthname;
                }
            });
            $scope.yearname = "";
            angular.forEach($scope.fillyear, function (check) {
                if (check.hrmlY_LeaveYear == yr) {
                    $scope.yearname = check.hrmlY_LeaveYear;

                }
            });

            var data = {
                "yearid": $scope.year,
                "months": $scope.month,
                "MI_ID": $scope.companay,
            };
            apiService.create("FrontOffice/attendenceapprovedlist", data).then(function (promise) {
                if (promise.griddatalist.length > 0) {
                    $scope.griddatalist = promise.griddatalist;
                }

            });

        };

        $scope.zoomdown = function () {
            $scope.takescreenshot = false;
            document.getElementById('the_canvas_element_id').style.zoom = "100%";
            document.getElementById('the_canvas_element_id').style.marginLeft = "0%";
            document.getElementById('the_canvas_element_id').style.marginBottom = "0%";
        };
        $scope.zoomup = function () {
            $scope.takescreenshot = true;
            //document.body.style.zoom = "100%";  
            document.getElementById('the_canvas_element_id').style.zoom = "50%";
            document.getElementById('the_canvas_element_id').style.marginLeft = "10%";
            document.getElementById('the_canvas_element_id').style.zIndex = "999px";
        };


        $scope.screenshot = function () {
            //$scope.zoomup();
            html2canvas(document.body, { allowTaint: false }, { useCORS: true }).then(function (canvas) {
                //document.body.appendChild(canvas);
                // Get base64URL
                var image = new Image();
                image.crossOrigin = "Anonymous";
                var base64URL = canvas.toDataURL('image/jpeg').replace('image/jpeg', 'image/octet-stream');

                Uploadprofilebase(base64URL);

            });
            //$scope.zoomdown();
        };

        function Uploadprofilebase(basedata) {

            var formData = new FormData();

            //We can send more data to server using append         
            formData.append("Id", 0);
            formData.append("Description", basedata);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Takeascreenshot", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    // swal(d);

                    onlineprofilepicpath = d;
                    //$scope.reg.PASR_Student_Pic_Path = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }
        
     
        $scope.loadData = function () {
            var id = 2;
            apiService.getURI("FrontOffice/getdataapprover", id).
                then(function (promise) {
            
                    if (promise.punchmissingapproval != null && promise.punchmissingapproval.length > 0) {
                        $('#missingpunchupdate').modal('show');
                        $scope.punchmissingapproval = promise.punchmissingapproval;                   
                    }
                    else {
                        $('#missingpunchupdate').modal('hide');
                    }

                    $scope.fillmonth = promise.fillmonth;
                    $scope.fillyear = promise.fillyear;
                    $scope.mappeduser = promise.mappeduser;
                    $scope.companylist = promise.companylist;
                   

                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clear_Details = function () {
            $state.reload();
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

        $scope.user_check = function () {
            if ($scope.user_check == 1) {
                $scope.userl = 1;
            }
            else {
                $scope.userl = 0;
            }
        };

        $scope.ShowReportdata = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {

                if ($scope.allind == "Month") {
                    $scope.from_date = new Date().toDateString();
                    $scope.to_date = new Date().toDateString();
                }
                else {
                    $scope.from_date = new Date($scope.fromdate).toDateString();
                    $scope.to_date = new Date($scope.todate).toDateString();
                    $scope.year = new Date($scope.fromdate).getFullYear();
                    $scope.month = (new Date($scope.fromdate).getMonth() + 1);
                }

                var data = {
                    "Fromdate": $scope.from_date,
                    "Todate": $scope.to_date,
                    "yearid": $scope.year,
                    "month": $scope.month,
                    "rdbbutton": $scope.allind,
                    "MI_Id": $scope.companay,
                };

                apiService.create("FrontOffice/getmonthreportforapproal", data).then(function (promise) {
                    $scope.institutename = promise.institutename;
                    $scope.disablemonth = true;
                    //var doob = $scope.ddate == null ? "" : $filter('date')($scope.ddate, "yyyy-MM-dd");
                    //var doobyr = doob.substring(0, 4);
                    //var doobmnth = doob.substring(5, 7);
                    //var doobdays = doob.substring(8, 10);

                    $scope.salaryGenerated = promise.salaryGenerated;
                    if ($scope.salaryGenerated.length > 0) {
                        swal("Salary Is  Calculated For This Month Please Contact Account Department");
                        $scope.Freezeattendence = true;
                    }

                    if ($scope.allind == "Month") {
                        $scope.numberofdays = new Date($scope.year, $scope.month, 0).getDate();
                        $scope.fromdate = new Date(promise.fromdate);
                        $scope.todate = new Date(promise.todate);
                    }
                    else {
                        var timeDiff = Math.abs($scope.fromdate.getTime() - $scope.todate.getTime());
                        $scope.numberofdays = Math.ceil(timeDiff / (1000 * 3600 * 24)) + 1;
                    }
                    var temp_array1 = [];
                    var temp_array = [];
                    var comment_array = [];
                    var arrhlf = [];
                    $scope.dolnumberofdays = 0;
                    $scope.HoldList = [];

                    for (var j = 0; j < promise.selectedActivityDetails1_stf.length; j++) {
                       // if (promise.selectedActivityDetails1_stf[j].hrmE_Id == 1675) {
                            if (promise.selectedActivityDetails1_stf[j].hrmE_Id != 0) {
                            $scope.workdyas = 0;
                            var arrhlftwo = [];
                            var tempdol = new Date(promise.selectedActivityDetails1_stf[j].hrmE_DOL);
                            var templeft = promise.selectedActivityDetails1_stf[j].hrmE_LeftFlag;

                            if (templeft == false) {
                                tempdol = new Date(promise.todate);
                            }
                            else if (templeft == true) {
                                $scope.dolfromdate = new Date(promise.fromdate);
                                $scope.hrmE_DOL = new Date(promise.selectedActivityDetails1_stf[j].hrmE_DOL);
                                var doltimeDiff = Math.abs($scope.dolfromdate.getTime() - $scope.hrmE_DOL.getTime());
                                $scope.dolnumberofdays = Math.ceil(doltimeDiff / (1000 * 3600 * 24)) + 1;
                            }

                            var workingdays = $filter('filter')(promise.workinghourDetails, function (d) {
                                return d.HRME_Id === promise.selectedActivityDetails1_stf[j].hrmE_Id;
                            });



                            var iemonthendreport = [];

                            iemonthendreport = $filter('filter')(promise.monthendreportclientwise, function (d) {
                                return d.hrmE_Id === promise.selectedActivityDetails1_stf[j].hrmE_Id;
                            });

                            var ISMCLMER_Id = 0
                            if (iemonthendreport.length > 0) {
                                ISMCLMER_Id = iemonthendreport[0].ismclmeR_Id;
                            }
                            else {
                                ISMCLMER_Id = 0;
                            }

                            var ielist = [];

                            ielist = $filter('filter')(promise.staffList, function (d) {
                                return d.hrmE_Id === promise.selectedActivityDetails1_stf[j].hrmE_Id;
                            });
                            var ieflag = false
                            if (ielist.length > 0) {
                                ieflag = true
                            }
                            else {
                                ieflag = false
                            }




                            if (promise.selectedActivityDetails1_stf[j].hrmE_ExcPunch == false) {
                                if (workingdays != null && workingdays != undefined && workingdays.length != 0) {
                                    $scope.workdyas = workingdays.length;
                                }
                                else {
                                    if (promise.selectedActivityDetails1_stf[j].hrmE_LeftFlag == false) {
                                        $scope.emptotlop = parseFloat($scope.numberofdays);
                                    }
                                    else {
                                        $scope.emptotlop = parseFloat($scope.dolnumberofdays);
                                    }
                                }
                            }
                            else {
                                $scope.workdyas = $scope.numberofdays;
                            }

                            //LATE IN EARLY OUT DETAILS
                            var LateInEarlyOutDetails = $filter('filter')(promise.staffDetails, function (d) {
                                return d.HRME_Id === promise.selectedActivityDetails1_stf[j].hrmE_Id;
                            });

                            if (promise.selectedActivityDetails1_stf[j].hrmE_Relaxtime == null) {
                                $scope.relaxationtime = 0;
                            }
                            else {
                                $scope.relaxationtime1 = promise.selectedActivityDetails1_stf[j].hrmE_EmployeeFirstName;
                                $scope.relaxationtime = promise.selectedActivityDetails1_stf[j].hrmE_Relaxtime;
                            }

                            var newCol = {};
                            if (LateInEarlyOutDetails.length > 0) {
                                for (var i = 0; i < LateInEarlyOutDetails.length; i++) {
                                    if (i == 0) {
                                        if (LateInEarlyOutDetails[i].FOEPD_InOutFlg == "I" && LateInEarlyOutDetails[i].DaysLop == 0 && LateInEarlyOutDetails[i].lateby != "00:00") {
                                            //$scope.summinutes = LateInEarlyOutDetails[i].UpdatedLateins;
                                            $scope.summinutes = $scope.makeminutes(LateInEarlyOutDetails[i].lateby);
                                            $scope.lateindates = new Date(LateInEarlyOutDetails[i].punchdate).getDate();
                                            if ($scope.summinutes > $scope.relaxationtime) {
                                                $scope.lateindateafter = new Date(LateInEarlyOutDetails[i].punchdate).getDate();
                                                $scope.latecomingcount = $scope.latecomingcount + 1;
                                                $scope.emptotlop = parseFloat(0.5);
                                            }
                                        }
                                        if (LateInEarlyOutDetails[i].FOEPD_InOutFlg == "O" && LateInEarlyOutDetails[i].DaysLop == 0 && LateInEarlyOutDetails[i].earlyby != "00:00") {
                                            $scope.summinutes = $scope.makeminutes(LateInEarlyOutDetails[i].earlyby);
                                            $scope.earlyoutdates = new Date(LateInEarlyOutDetails[i].punchdate).getDate();
                                            if ($scope.summinutes > $scope.relaxationtime) {
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

                                            if ($scope.summinutes > $scope.relaxationtime) {
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
                                            if ($scope.summinutes > $scope.relaxationtime) {
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

                            //DAILY REPORT not Approved DETAILS
                            var DailyrptNotApproved = $filter('filter')(promise.columnnames, function (d) {
                                return d.HRME_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id;
                            });

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

                            //UNAUTHORISED LEAVE REPORT DETAILS
                            var leaverptDetails = $filter('filter')(promise.staffleaveDetails, function (d) {
                                return d.HRME_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id;
                            });

                            $scope.unauthleavedates = "";
                            $scope.unauthleavedatestemp = "";
                            $scope.unauthleavedatesdisplay = "";
                            $scope.unauthleavecount = 0;

                            if (leaverptDetails.length > 0 && $scope.workdyas != 0) {
                                angular.forEach(leaverptDetails, function (check) {
                                    if (check.Approvedday === 0) {
                                        if (tempdol >= new Date(check.DrNotSentDates)) {
                                            $scope.unauthleavecount += parseFloat(check.Absentday);
                                        }
                                    }
                                });
                                $scope.emptotlop = parseFloat($scope.emptotlop) + parseFloat($scope.unauthleavecount);

                                for (var i = 0; i < leaverptDetails.length; i++) {
                                    if (tempdol >= new Date(leaverptDetails[i].DrNotSentDates)) {
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
                                                if (tempdol >= new Date(halffullDetails[i].punchdate)) {
                                                    if (halffullDetails[i].DaysLop == "H") {
                                                        $scope.unauthleavecount = parseFloat($scope.unauthleavecount) + 0.5;
                                                        $scope.emptotlop = parseFloat($scope.emptotlop) + 0.5;
                                                        if ($scope.unauthleavedates == "") {
                                                            $scope.unauthleavedates = new Date(halffullDetails[i].punchdate).getDate();
                                                            if ($scope.unauthleavedatesdisplay == "") {
                                                                $scope.unauthleavedatesdisplay = new Date(halffullDetails[i].punchdate).getDate() + "(H)";
                                                            }
                                                            else {
                                                                $scope.unauthleavedatesdisplay = $scope.unauthleavedatesdisplay + ", " + new Date(halffullDetails[i].punchdate).getDate() + "(H)";
                                                            }
                                                            $scope.addholidaylop(new Date(halffullDetails[i].punchdate).getDate().toString(), 'H');
                                                        }
                                                        else {
                                                            $scope.unauthleavedates = $scope.unauthleavedates + ", " + new Date(halffullDetails[i].punchdate).getDate();
                                                            if ($scope.unauthleavedatesdisplay == "") {
                                                                $scope.unauthleavedatesdisplay = new Date(halffullDetails[i].punchdate).getDate() + "(H)";
                                                            }
                                                            else {
                                                                $scope.unauthleavedatesdisplay = $scope.unauthleavedatesdisplay + ", " + new Date(halffullDetails[i].punchdate).getDate() + "(H)";
                                                            }
                                                            $scope.addholidaylop(new Date(halffullDetails[i].punchdate).getDate().toString(), 'H');
                                                        }
                                                    }
                                                    else if (halffullDetails[i].DaysLop == "F") {
                                                        //&& halffullDetails[i].FOEPD_InOutFlg != "" && halffullDetails[i].DaysLop != null
                                                        $scope.unauthleavecount = parseFloat($scope.unauthleavecount) + 1;
                                                        $scope.emptotlop = parseFloat($scope.emptotlop) + 1;
                                                        if ($scope.unauthleavedates == "") {
                                                            $scope.unauthleavedates = new Date(halffullDetails[i].punchdate).getDate();
                                                            if ($scope.unauthleavedatesdisplay == "") {
                                                                $scope.unauthleavedatesdisplay = new Date(halffullDetails[i].punchdate).getDate() + "(F)";
                                                            }
                                                            else {
                                                                $scope.unauthleavedatesdisplay = $scope.unauthleavedatesdisplay + ", " + new Date(halffullDetails[i].punchdate).getDate() + "(F)";
                                                            }
                                                            $scope.addholidaylop(new Date(halffullDetails[i].punchdate).getDate().toString(), 'F');
                                                        }
                                                        else {
                                                            $scope.unauthleavedates = $scope.unauthleavedates + ", " + new Date(halffullDetails[i].punchdate).getDate();
                                                            if ($scope.unauthleavedatesdisplay == "") {
                                                                $scope.unauthleavedatesdisplay = new Date(halffullDetails[i].punchdate).getDate() + "(F)";
                                                            }
                                                            else {
                                                                $scope.unauthleavedatesdisplay = $scope.unauthleavedatesdisplay + ", " + new Date(halffullDetails[i].punchdate).getDate() + "(F)";
                                                            }
                                                            $scope.addholidaylop(new Date(halffullDetails[i].punchdate).getDate().toString(), 'F');
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                    else {
                                        if (arrhlftwo.indexOf(new Date(halffullDetails[i].punchdate).getDate().toString()) < 0) {
                                            if (tempdol >= new Date(halffullDetails[i].punchdate)) {
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

                                            if (leaves.lopadjest == true) {
                                                if (leavesshowdates == "") {
                                                    leavesshowdates = new Date(leaves.DrNotSentDates).getDate() + " (" + leaves.Approvedday + ")-CA";
                                                }
                                                else {
                                                    leavesshowdates = leavesshowdates + "," + new Date(leaves.DrNotSentDates).getDate() + " (" + leaves.Approvedday + ")-CA";
                                                }
                                            }
                                            else {
                                                if (leavesshowdates == "") {
                                                    leavesshowdates = new Date(leaves.DrNotSentDates).getDate() + " (" + leaves.Approvedday + ")";
                                                }
                                                else {
                                                    leavesshowdates = leavesshowdates + "," + new Date(leaves.DrNotSentDates).getDate() + " (" + leaves.Approvedday + ")";
                                                }

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
                            //APPROVED LEAVE DETAILS

                            if (leave_array.length > 0) {
                                $scope.approvedsum = 0;
                                $scope.adjestedleave = 0;
                                if (approvedleave.length > 0) {
                                    angular.forEach(approvedleave, function (emp) {

                                        if (emp.lopadjest == true) {
                                            $scope.adjestedleave += 0.5;
                                        }
                                        else {
                                            $scope.approvedsum += parseFloat(emp.Approvedday);
                                        }

                                    });
                                }

                                if ($scope.workdyas == 0) {
                                    $scope.emptotlop = parseFloat($scope.emptotlop) - (parseFloat($scope.approvedsum) + parseFloat($scope.adjestedleave));
                                }
                                else {
                                    $scope.emptotlop = parseFloat($scope.emptotlop) - parseFloat($scope.adjestedleave);
                                }
                            }





                            $scope.addinallopdetails = promise.lunchdata;
                            var additionallopdetails = $filter('filter')(promise.lunchdata, function (d) {
                                return d.hrmE_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id;
                            });

                            var addinallopdates = "";
                            var addinallop = 0
                            if (additionallopdetails != null && additionallopdetails.length > 0) {
                                angular.forEach(additionallopdetails, function (mon) {
                                    addinallopdates = addinallopdates + "," + new Date(mon.hralD_Date).getDate()
                                    addinallop = addinallop + 0.5;
                                });

                            }
                            //$scope.emptotlop = parseFloat($scope.emptotlop) + parseFloat(addinallop);


                            //SALARY DETAILS
                            var salary_array = [];
                            var newsalary = {};
                            var salarydetails = $filter('filter')(promise.currentsalary, function (d) {
                                return d.hrmE_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id;
                            });
                           

                            var missingpunchApproved = [];
                            var missingpunchApproveddates = [];
                             missingpunchApproved = $filter('filter')(promise.punchmissingapproval, function (d) {
                                return d.HRME_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id && d.FOEPAR_ApprovalFlg == "Approved";
                            });
                        
                            var missingpunchdate = "";
                        
                            if (missingpunchApproved != null && missingpunchApproved.length > 0) {                                                         
                                angular.forEach(missingpunchApproved, function (mon) {                                  
                                    missingpunchdate = missingpunchdate + "," + new Date(mon.FOEPAR_PunchDate).getDate();                                  
                                });
                                missingpunchApproveddates.push({ missingpunchdate: missingpunchdate});
                            }

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
                                    // $scope.emptotlop = parseFloat($scope.emptotlop) + 0.5;
                                    $scope.dailyrptblockedcount = $scope.dailyrptblockedcount + 1;
                                    if ($scope.dailyrptblockeddates == "") {
                                        $scope.dailyrptblockeddates = new Date(blockedDailyrpt[i].ismdrpT_Date).getDate();
                                    } else {
                                        $scope.dailyrptblockeddates = $scope.dailyrptblockeddates + ", " + new Date(blockedDailyrpt[i].ismdrpT_Date).getDate();
                                    }
                                }
                            }
                            //BLOCKED DAILY REPORT
                            var attandanceapprovedlist = [];
                            var employeeRetingdata = [];

                            attandanceapprovedlist = $filter('filter')(promise.approvedlist, function (d) {
                                return d.vmscrT_Id == promise.selectedActivityDetails1_stf[j].vmscrT_Id;
                            });

                            employeeRetingdata = $filter('filter')(promise.employeeRetingdata, function (d) {
                                return d.HRME_ID == promise.selectedActivityDetails1_stf[j].hrmE_Id;
                            });

                            $scope.HREAA_Remarks = "Approved";
                            var approvalstatus = "Approved";
                            if (attandanceapprovedlist.length > 0) {
                                $scope.HREAA_Remarks = attandanceapprovedlist[0].hreaA_Remarks
                                approvalstatus = attandanceapprovedlist[0].hreaA_ApprovalFlg
                            }


                            //HR COMMENT DETAILS
                            if ($scope.emptotlop >= $scope.numberofdays) {
                                $scope.emptotlop = $scope.numberofdays;
                                if ($scope.workdyas == 0 && $scope.newjoin === true) {
                                    $scope.emptotlop = $scope.emptotlop - $scope.latejoin;
                                }
                            }

                            if (DailyrptNotApproved.length > 0) {
                                $scope.DRNotApprovedCount = DailyrptNotApproved[0].DRNotApprovedCount
                            }
                            else {
                                $scope.DRNotApprovedCount = 0;
                            }
                            var newCol1 = {};
                            if (promise.hrcomments.length > 0) {
                                $scope.hrcomments = promise.hrcomments;
                                $scope.hrcommentflag = true;
                                var hrcommentDetails = $filter('filter')(promise.hrcomments, function (d) {
                                    return d.hrmE_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id;
                                });
                                //HR_LOP: hrcommentDetails[0].vmscrT_HRTLOP
                                if (hrcommentDetails.length > 0) {
                                    newCol1 = { overallrating: employeeRetingdata[0].OVERALLRATING, maxvalue: employeeRetingdata[0].MAXVALUE, approvalstatus: approvalstatus, empdate: temp_array, hrmE_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, ecode: promise.selectedActivityDetails1_stf[j].hrmE_EmployeeCode, ename: promise.selectedActivityDetails1_stf[j].hrmE_EmployeeFirstName, desgname: promise.selectedActivityDetails1_stf[j].hrmE_PerStreet, drcount: $scope.dailyrptncount, drdates: $scope.dailyrptndates, halfdrcount: $scope.dailyrpthalfdaycount, halfdrdates: $scope.dailyrpthalfdaydates, leavecount: $scope.unauthleavecount, leavedates: $scope.unauthleavedatesdisplay, blkdrcount: $scope.dailyrptblockedcount, blkdrdates: $scope.dailyrptblockeddates, totlop: $scope.emptotlop, HR_EO: hrcommentDetails[0].vmscrT_HREO, HR_DR: hrcommentDetails[0].vmscrT_HRDR, HR_HDR: hrcommentDetails[0].vmscrT_HRHDR, HR_BDR: hrcommentDetails[0].vmscrT_HRBDR, HR_LC: hrcommentDetails[0].vmscrT_HRLC, HR_UL: hrcommentDetails[0].vmscrT_HRUL, HR_LOP: hrcommentDetails[0].vmscrT_HRTLOP, HREAA_LOP: hrcommentDetails[0].vmscrT_HRTLOP, HR_REM: hrcommentDetails[0].vmscrT_Remarks, vmscrT_DOJ: promise.selectedActivityDetails1_stf[j].hrmE_DOJ, approvedleavedetails: leave_array, salarydetails: salary_array, latejoin: $scope.latejoin, worksdaycount: $scope.workdyas, newjoin: $scope.newjoin, leave_arraytemp: leave_arraytemp, dolnumberofdays: $scope.dolnumberofdays, dol: new Date(promise.selectedActivityDetails1_stf[j].hrmE_DOL), leftflag: promise.selectedActivityDetails1_stf[j].hrmE_LeftFlag, hrmE_Relaxtime: promise.selectedActivityDetails1_stf[j].hrmE_Relaxtime, HREAA_Remarks: $scope.HREAA_Remarks, DRNotApprovedCount: $scope.DRNotApprovedCount, ISMCLMER_Id: ISMCLMER_Id, ieflag: ieflag,  missingpunchApproveddates: missingpunchApproveddates, addinallopdates: addinallopdates, addinallop: addinallop };
                                }
                                else {
                                    //HR_LOP: 0
                                    newCol1 = { approvalstatus: approvalstatus, empdate: temp_array, hrmE_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, ecode: promise.selectedActivityDetails1_stf[j].hrmE_EmployeeCode, ename: promise.selectedActivityDetails1_stf[j].hrmE_EmployeeFirstName, desgname: promise.selectedActivityDetails1_stf[j].hrmE_PerStreet, drcount: $scope.dailyrptncount, drdates: $scope.dailyrptndates, halfdrcount: $scope.dailyrpthalfdaycount, halfdrdates: $scope.dailyrpthalfdaydates, leavecount: $scope.unauthleavecount, leavedates: $scope.unauthleavedatesdisplay, blkdrcount: $scope.dailyrptblockedcount, blkdrdates: $scope.dailyrptblockeddates, totlop: $scope.emptotlop, HR_EO: 0, HR_DR: 0, HR_HDR: 0, HR_BDR: 0, HR_LC: 0, HR_UL: 0, HR_LOP: $scope.emptotlop, HREAA_LOP: $scope.emptotlop, HR_REM: '', vmscrT_DOJ: promise.selectedActivityDetails1_stf[j].hrmE_DOJ, approvedleavedetails: leave_array, salarydetails: salary_array, latejoin: $scope.latejoin, worksdaycount: $scope.workdyas, newjoin: $scope.newjoin, leave_arraytemp: leave_arraytemp, dolnumberofdays: $scope.dolnumberofdays, dol: new Date(promise.selectedActivityDetails1_stf[j].hrmE_DOL), leftflag: promise.selectedActivityDetails1_stf[j].hrmE_LeftFlag, hrmE_Relaxtime: promise.selectedActivityDetails1_stf[j].hrmE_Relaxtime, HREAA_Remarks: $scope.HREAA_Remarks, DRNotApprovedCount: $scope.DRNotApprovedCount, ISMCLMER_Id: ISMCLMER_Id, ieflag: ieflag,missingpunchApproveddates: missingpunchApproveddates, addinallopdates: addinallopdates, addinallop: addinallop };
                                }
                            }
                            else {
                                //HR_LOP: 0
                                newCol1 = { approvalstatus: approvalstatus, empdate: temp_array, hrmE_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, ecode: promise.selectedActivityDetails1_stf[j].hrmE_EmployeeCode, ename: promise.selectedActivityDetails1_stf[j].hrmE_EmployeeFirstName, desgname: promise.selectedActivityDetails1_stf[j].hrmE_PerStreet, drcount: $scope.dailyrptncount, drdates: $scope.dailyrptndates, halfdrcount: $scope.dailyrpthalfdaycount, halfdrdates: $scope.dailyrpthalfdaydates, leavecount: $scope.unauthleavecount, leavedates: $scope.unauthleavedatesdisplay, blkdrcount: $scope.dailyrptblockedcount, blkdrdates: $scope.dailyrptblockeddates, totlop: $scope.emptotlop, HR_EO: 0, HR_DR: 0, HR_HDR: 0, HR_BDR: 0, HR_LC: 0, HR_UL: 0, HR_LOP: $scope.emptotlop, HREAA_LOP: $scope.emptotlop, HR_REM: '', vmscrT_DOJ: promise.selectedActivityDetails1_stf[j].hrmE_DOJ, approvedleavedetails: leave_array, salarydetails: salary_array, latejoin: $scope.latejoin, worksdaycount: $scope.workdyas, newjoin: $scope.newjoin, leave_arraytemp: leave_arraytemp, dolnumberofdays: $scope.dolnumberofdays, dol: new Date(promise.selectedActivityDetails1_stf[j].hrmE_DOL), leftflag: promise.selectedActivityDetails1_stf[j].hrmE_LeftFlag, hrmE_Relaxtime: promise.selectedActivityDetails1_stf[j].hrmE_Relaxtime, HREAA_Remarks: $scope.HREAA_Remarks, DRNotApprovedCount: $scope.DRNotApprovedCount, ISMCLMER_Id: ISMCLMER_Id, ieflag: ieflag,  missingpunchApproveddates: missingpunchApproveddates, addinallopdates: addinallopdates, addinallop: addinallop };
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
                        //if end
                    }

                    $scope.monthendreport = temp_array1;

                    angular.forEach($scope.monthendreport, function (mon) {
                        if (mon.approvalstatus == "Approved") {
                            mon.approvalstatus = "Approved";
                        }
                        else if (mon.approvalstatus == "Hold") {
                            mon.approvalstatus = "Hold";
                            $scope.HoldList.push({ approvalstatus: mon.approvalstatus, ecode: mon.ecode, ename: mon.ename, desgname: mon.desgname, HREAA_Remarks: mon.HREAA_Remarks });
                            if ($scope.HoldList.length > 0) {
                                $('#employeeholdlist').modal('show');
                            }

                        }
                        mon.selected = true;
                        $scope.SelectedAll = true;
                        // mon.HREAA_Remarks = "Approved";
                    });

                    if ($scope.monthendreport.length > 0) {
                        $scope.report = true;
                    }
                    else {
                        swal("No Records Found !");
                        $scope.report = false;
                        $scope.disablemonth = false;
                    }

                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.statusupdate = function (emp) {
            angular.forEach($scope.monthendreport, function (val) {
                if (emp.hrmE_Id == val.hrmE_Id && emp.approvalstatus == "Hold") {
                    val.HREAA_Remarks = "Hold";
                }
                else if (emp.hrmE_Id == val.hrmE_Id && emp.approvalstatus == "Approved") {
                    val.HREAA_Remarks = "Approved";
                }




            });
        }



        $scope.viewdocument = function (obj) {           
            var data = {
                "FOEPAR_Id": obj.FOEPAR_Id
            };
            apiService.create("Employee_Add_logs_Manual/FindData", data).
                then(function (promise) {
                    if (promise.logapprovallist !== null && promise.logapprovallist.length > 0) {
                        $scope.logapprovallist1 = promise.logapprovallist;
                        angular.forEach($scope.logapprovallist1, function (item) {
                            item.FOEPARD_Time = moment(item.FOEPARD_Time, 'H:mm ').format();
                        });
                        $scope.EmployeeDis = true;
                       // $('#missingpunchupdate').modal('show');
                        $scope.FOEPAR_Id = obj.FOEPAR_Id;
                        $scope.hrmE_Id = obj.hrmE_Id;
                    }
                    else {
                        swal("No Manual Logs Found !!");
                        return;
                    }
                });
        };



        $scope.saveapproval = function (status) {
            if ($scope.myForm1.$valid) {
                $scope.temp_array2 = [];
                angular.forEach($scope.logapprovallist1, function (emp) {
                    if (emp.verified == true) {
                        $scope.temp_array2.push({ FOEPAR_Remarks: emp.ARemarks, FOEPD_PunchTime: $filter('date')(emp.FOEPARD_Time, "HH:mm"), FOEPARD_InOutFlg: emp.FOEPARD_InOutFlg, FOEPARD_Id: emp.FOEPARD_Id, Status: status });
                    }

                });

                var data = {
                    "temp1": $scope.temp_array2,
                    "HRMLY_LeaveYear": $scope.year,
                    "IVRM_Month_Id": $scope.month,
                    "MI_Id": $scope.companay
                };
                apiService.create("Employee_Add_logs_Manual/saveapproval", data).then(function (promise) {

                    if (promise.returnmsg !== "") {
                        if (promise.returnmsg === "Success") {
                            
                            //angular.forEach($scope.monthendreport, function (val) {
                            //    if (val.hrmE_Id == $scope.hrmE_Id) {
                            //        var missingpunchApproved = [];
                            //        missingpunchApproved = $filter('filter')(promise.punchdetails, function (d) {
                            //            return d.HRME_Id == val.hrmE_Id && $scope.FOEPAR_Id==d.FOEPAR_Id && d.FOEPAR_ApprovalFlg == "Approved";
                            //        });                              
                            //        var missingpunchdate = "";
                            //        if (missingpunchApproved != null && missingpunchApproved.length > 0) {                              
                            //            angular.forEach(missingpunchApproved, function (mon) {
                            //            val.FOEPAR_Id = 0;

                            //                missingpunchdate = missingpunchdate + "," + new Date(mon.FOEPAR_PunchDate).getDate()

                            //            });
                            //            val.missingpunchApproveddates[0].missingpunchdate = val.missingpunchApproveddates[0].missingpunchdate + " " + missingpunchdate
                            //        }

                            //    }


                            //});

                          //  $('#missingpunchupdate').modal('hide');
                            swal("Records Approved Successfully..");

                           // $state.reload();
                        }
                        else if (promise.returnmsg === "Failed") {
                            swal("Approval Failed!, Kindly contact Administrator ");
                        }
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                        }
                    }
                });
            }
        };






        $scope.getlateinearlyout = function (emp, empname) {
            $scope.emppunchname = empname;
            var data = {
                "fromdate": $scope.from_date,
                "todate": $scope.to_date,
                "HRME_Id": emp,
                "yearid": $scope.year,
                "month": $scope.month,
                "flag": $scope.allind
            };

            apiService.create("ISMDashboard/getlateearlyoutreport", data).then(function (promise) {
                $scope.punchdetails = promise.punchdetails;
                //$scope.gpssavedetails = promise.admindata
                if ($scope.punchdetails !== null && $scope.punchdetails.length > 0) {
                    $scope.viewattendance = true;
                }
                else { $scope.viewattendance = false; }

                if (promise.punchapproveddetails !== null && promise.punchapproveddetails.length > 0) {
                    $scope.logapprovallist = promise.punchapproveddetails;
                }

                $scope.punchdetailsprevious = promise.punchdetailsprevious;
                if ($scope.punchdetailsprevious !== null && $scope.punchdetailsprevious.length > 0) {
                    $scope.viewattendancep = true;
                }
                else { $scope.viewattendancep = false; }

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
                        if (hourval < 3) {
                            tothour = parseInt(tothour) + parseInt(hourval);
                            totmin = parseInt(totmin) + parseInt(minval);
                        }
                    });
                    //EARLY OUT
                    angular.forEach($scope.lateearlylist, function (late) {
                        hourval = late.earlyby.split(':')[0];
                        minval = late.earlyby.split(':')[1];
                        if (hourval < 3) {
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
                            if (late.punchdate == punch.punchdate && late.HRME_Id == punch.HRME_Id) {
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
                            if (late.punchdate == punch.punchdate && late.HRME_Id == punch.HRME_Id) {
                                if (late.FOEPD_InOutFlg == "I") {
                                    punch.lateclass = "lateinearly";
                                    punch.lateearly = "lateinearly";

                                }
                                else {
                                    punch.earlyclass = "lateinearly";
                                    punch.lateearly = "lateinearly";
                                }
                            }





                            //var gpsintimedetails = $filter('filter')($scope.gpssavedetails, function (d) {
                            //    return d.HRME_Id == punch.HRME_Id && d.Intime == punch.intime && ($filter('date')(new Date(d.Punchdate), 'yyyy-MM-dd')) == ($filter('date')(new Date(punch.punchdate), 'yyyy-MM-dd'));
                            //});

                            //var gpsouttimedetails = $filter('filter')($scope.gpssavedetails, function (d) {
                            //    return d.HRME_Id == punch.HRME_Id && d.Outtime == punch.outtime && ($filter('date')(new Date(d.Punchdate), 'yyyy-MM-dd')) == ($filter('date')(new Date(punch.punchdate), 'yyyy-MM-dd'));
                            //});
                            //if (gpsintimedetails != null && gpsintimedetails.length > 0) {
                            //    punch.gpsintime = "GPS";
                            //}
                            //if (gpsouttimedetails != null && gpsouttimedetails.length > 0) {
                            //    punch.gpsouttime = "GPS";
                            //}



                        });
                    });
                }
            });
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

        $scope.exptoex = function () {
            var excelname = $scope.institutename;
            $scope.from_dateex = $filter('date')($scope.from_date, 'yyyy-MM-dd');
            $scope.to_dateex = $filter('date')($scope.to_date, 'yyyy-MM-dd');
            if ($scope.allind != "Month") {
                excelname = excelname.toUpperCase() + ' - ' + $scope.from_dateex + ' To ' + $scope.to_dateex + ' – Attendance Report.xls';
            } else {
                excelname = excelname.toUpperCase() + ' - ' + $scope.monthname + ' - ' + $scope.yearname + ' – Attendance Report.xls';
            }

            var printSectionId = '#tablegrp';

            var exportHref = Excel.tableToExcel(printSectionId, 'Attendance Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                document.onfreeze = true;
                a.click();
                a.remove();
            }, 100);
        }

        $scope.printData = function () {
            var innerContents = document.getElementById("tablegrpprint").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };








        $scope.Savedata = function () {
            if ($scope.myForm.$valid) {
                if ($scope.monthendreport.length > 0) {
                    $scope.temp_array2 = [];
                    for (var x = 0; x < $scope.monthendreport.length; x++) {

                        // if ($scope.monthendreport[x].selected == true) {
                        $scope.temp_array2.push({ HREAA_LOP: $scope.monthendreport[x].HREAA_LOP, HREAA_Remarks: $scope.monthendreport[x].HREAA_Remarks, ecode: $scope.monthendreport[x].ecode, hrmE_Id: $scope.monthendreport[x].hrmE_Id, status: $scope.monthendreport[x].approvalstatus });
                        //  }
                    }
                    if ($scope.temp_array2 != null && $scope.temp_array2.length > 0) {
                        //$scope.screenshot();
                        swal({
                            title: "Are you sure?",
                            text: "Do you want to Approve attendance?",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Save",
                            cancelButtonText: "Cancel",
                            closeOnConfirm: false,
                            closeOnCancel: false
                        },
                            function (isConfirm) {
                                if (isConfirm) {
                                    var data = {
                                        "yearid": $scope.year,
                                        "months": $scope.month,
                                        "AttendenceApprovedDTO": $scope.temp_array2
                                    };
                                    apiService.create("FrontOffice/attendenceapprove", data).then(function (promise) {
                                        if (promise.message === 'Saved') {
                                            swal("Record Approved succussfully");
                                            $state.reload();
                                        }
                                        else if (promise.message === 'NotSaved') {
                                            swal("Failed to Save/Update");
                                            $scope.exporttoexcelpopup();
                                            $scope.hrcommentflag = true;
                                        }
                                    });

                                }
                                else {
                                    swal("save data Cancelled!");
                                }
                            });

                    }
                    else {
                        swal("Kindly select employees to save attendance!");
                    }
                }

            }
            else {
                $scope.submitted = true;
                swal("Kindly enter all mandatory fields!!");
            }
        };

        $scope.chkSelectedall = function (selectedvalue) {
            angular.forEach($scope.monthendreport, function (emp) {
                emp.selected = selectedvalue;
            });
        };

        $scope.optionToggled = function () {
            $scope.SelectedAll = $scope.monthendreport.every(function (options) {
                return options.selected;
            });
        };

        $scope.makesubit = function () {
            $scope.submitted = true;
        }

        $scope.checklopmatch = function (val, lop, hrlop, valind) {
            if (Number(hrlop) > Number($scope.numberofdays)) {
                $scope.updatelop(val, lop, hrlop, 2, valind);
                swal("LOP can't be more than total month days: " + $scope.numberofdays);
            }
            else {
                if (hrlop > lop) {
                    swal({
                        title: "LOP Exceed alert!!",
                        text: "You Are Entering More than LOP Calculated For " + val.ename,
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Add",
                        cancelButtonText: "No Cancel",
                        closeOnConfirm: true,
                        closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                $scope.updatelop(val, lop, hrlop, 1, valind);
                            }
                            else {
                                $scope.updatelop(val, lop, hrlop, 2, valind);
                            }
                        });
                }
            }

        };

        $scope.placeholder = function (val, lop, hrlop, valind) {
            val.placeholder = "";
            if (hrlop != lop) {
                angular.forEach($scope.monthendreport, function (options) {
                    if (options.hrmE_Id === val.hrmE_Id) {
                        options.placeholder = "Kindly enter remarks as software calculated LOP not match with manually entered LOP!!!";
                    }
                });
                val.placeholder = "Kindly enter remarks as software calculated LOP not match with manually entered LOP!!!";
            }
            else {
                angular.forEach($scope.monthendreport, function (options) {
                    if (options.hrmE_Id === val.hrmE_Id) {
                        options.placeholder = "";
                    }
                });
                val.placeholder = "";
            }
        }

        $scope.exporttoexcelpopup = function () {
            swal({
                title: "Records Updated Successfully.!!",
                text: "Do you want to export data to excel?",
                type: "success",
                showCancelButton: true,
                confirmButtonColor: "green", confirmButtonText: "Yes, export excel!",
                cancelButtonText: "No",
                closeOnConfirm: true,
                closeOnCancel: true
            },
                function (isConfirm) {
                    if (isConfirm) {
                        $scope.exptoex();
                    }
                    else {

                    }
                });
        };

        $scope.updatelop = function (val, lop, hrlop, pass, valind) {
            if (pass === 1) {
                angular.forEach($scope.monthendreport, function (options) {
                    if (options.hrmE_Id === val.hrmE_Id) {
                        options.HR_LOP = hrlop;
                    }
                });
                val.HR_LOP = hrlop;
                document.getElementById('comment5' + valind).value = hrlop;
                $scope.$apply(function () {
                    $scope.submitted = true;
                });
            }
            else {
                angular.forEach($scope.monthendreport, function (options) {
                    if (options.hrmE_Id === val.hrmE_Id) {
                        options.HR_LOP = lop;
                    }
                });
                val.HR_LOP = lop;
                document.getElementById('comment5' + valind).value = lop;
                $scope.$apply(function () {
                    $scope.submitted = false;
                });
            }
        }

        $scope.sendmail = function () {
            $scope.tempmonthendreport = [];
            angular.forEach($scope.monthendreport, function (options) {
                if (options.selected === true) {
                    $scope.tempmonthendreport.push(options);
                }
            });

            if ($scope.tempmonthendreport.length > 0) {
                swal({
                    title: "Are you sure?",
                    text: "Do you want to send mail to selected " + $scope.tempmonthendreport.length + " employees?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Send",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {

                            for (var i = 0; i < $scope.tempmonthendreport.length; i++) {
                                var html = "";
                                html = document.getElementById("html_" + $scope.tempmonthendreport[i].hrmE_Id).innerHTML.toString();
                                angular.forEach($scope.tempmonthendreport, function (options) {
                                    if (options.hrmE_Id === $scope.tempmonthendreport[i].hrmE_Id) {
                                        options.html = "";
                                        html = html.replace(/[\n]/g, '').toString();
                                        options.html = html.replace(/\\/g, '').toString();
                                    }
                                });
                            }

                            var data = {
                                "yearid": $scope.year,
                                "months": $scope.month,
                                "ConsolidateReportTempDTO": $scope.tempmonthendreport
                            };
                            apiService.create("FrontOffice/SendFinalMail", data).
                                then(function (promise) {
                                    if (promise.retrunMsg === 'True') {
                                        swal("Mail Sent Successfully To Selected " + $scope.tempmonthendreport.length + " Employees!");
                                    }
                                    else if (promise.retrunMsg === 'False') {
                                        swal("Mail Not sent!");
                                    }
                                });
                        }
                        else {
                            swal("Mail sending Cancelled!");
                        }
                    });
            }
            else {
                swal("Select Any Employee To Send Mail!");
            }
        };







        $scope.iemonthendreport = function (ISMCLMER_Id) {
            $scope.monthendreportotherdata = [];
            var data = {
                "ISMCLMER_Id": ISMCLMER_Id
            };

            apiService.create("FrontOffice/iemonthendreport", data).then(function (promise) {
                if (promise.monthendreportotherdata != null && promise.monthendreportotherdata.length > 0) {

                    $scope.monthendreportotherdata = promise.monthendreportotherdata
                    $scope.ClientName = $scope.monthendreportotherdata[0].ISMMCLT_ClientName
                }
                else {
                    swal("Client Month End Report Not Submitted");
                }
            });

        };



        $scope.viewcomment = function (value) {
            $scope.additionallopdetails = $filter('filter')($scope.addinallopdetails, function (d) {
                return d.hrmE_Id == value.hrmE_Id;
            });

            if ($scope.additionallopdetails != null && $scope.additionallopdetails.length > 0) {
                $('#myAdditionallopdetails').modal('show');
            }
            else {
                swal("No Additional LOP Found!");
            }

        };


    }
})();

