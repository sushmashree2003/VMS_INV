(function () {
    'use strict';

    angular
        .module('app')
        .controller('FrontOfficeMonthEndReport', FrontOfficeMonthEndReport);

    FrontOfficeMonthEndReport.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', '$q']

    function FrontOfficeMonthEndReport($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, $q) {

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
            $scope.report = false;
            $scope.ivrM_Month_Name = "";
            angular.forEach($scope.fillmonth, function (check) {
                if (check.ivrM_Month_Id == mon) {
                    $scope.ivrM_Month_Name = check.ivrM_Month_Name;
                }
            });
            $scope.yearname = "";
            angular.forEach($scope.fillyear, function (check) {
                if (check.hrmlY_LeaveYear == yr) {
                    $scope.yearname = check.hrmlY_LeaveYear;
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
            apiService.getURI("FrontOffice/", id).then(function (promise) {
                $scope.fillmonth = promise.getmonth;
                $scope.fillyear = promise.getyear;

                $scope.pagePrivilege = [];
                if (promise.pagePrivilege !== null && promise.pagePrivilege.length > 0) {
                    $scope.pagePrivilege = promise.pagePrivilege;
                    $scope.IVRMRP_AddFlag = $scope.pagePrivilege[0].IVRMRP_AddFlag;
                    $scope.IVRMRP_UpdateFlag = $scope.pagePrivilege[0].IVRMRP_UpdateFlag;
                    $scope.IVRMRP_DeleteFlag = $scope.pagePrivilege[0].IVRMRP_DeleteFlag;
                    $scope.IVRMRP_ReportFlag = $scope.pagePrivilege[0].IVRMRP_ReportFlag;
                    $scope.IVRMRP_ProcessFlag = $scope.pagePrivilege[0].IVRMRP_ProcessFlag;
                }
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

        $scope.getempnoticememodetailreport = function (emp_id) {

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
            $scope.reporttype = "E";

            var data = {
                "startdate": new Date($scope.from_date).toDateString(),
                "enddate": new Date($scope.to_date).toDateString(),
                "reporttype": $scope.reporttype,
                "HRME_Id": emp_id,
                "yearid": $scope.year,
                "month": $scope.month,
                "rdbbutton": $scope.allind

            };
            $scope.overallempmemonotice = [];
            apiService.create("FrontOffice/consolidatedemployeemonthendreport", data).then(function (promise) {
                if (promise.overallempmemonotice.length > 0) {
                    $scope.overallempmemonotice = promise.overallempmemonotice;
                }
            });

        };

        $scope.getempmanagementdetailreport = function (emp_id) {
            $scope.empmanagementdetailreport = [];
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
            $scope.reporttype = "E";

            var data = {
                "startdate": new Date($scope.from_date).toDateString(),
                "enddate": new Date($scope.to_date).toDateString(),
                "reporttype": $scope.reporttype,
                "HRME_Id": emp_id,
                "yearid": $scope.year,
                "month": $scope.month,
                "rdbbutton": $scope.allind

            };

            apiService.create("FrontOffice/consolidatedemployeemonthendreport", data).then(function (promise) {
                if (promise.empmanagementdetailreport.length > 0) {
                    $scope.empmanagementdetailreport = promise.empmanagementdetailreport;
                }
            });

        };
        $scope.getemployeemonthendreport = function (emp_id) {

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


            $scope.reporttype = "E";

            var data = {
                "startdate": new Date($scope.from_date).toDateString(),
                "enddate": new Date($scope.to_date).toDateString(),
                "reporttype": $scope.reporttype,
                "HRME_Id": emp_id,
                "yearid": $scope.year,
                "month": $scope.month,
                "rdbbutton": $scope.allind

            };

            apiService.create("FrontOffice/consolidatedemployeemonthendreport", data).then(function (promise) {

                if (promise.successcount == 0) {
                    var countswal = 0;




                    //Employeewise Report
                    if (promise.departmentempissues != null && promise.departmentempissues.length > 0) {
                        $scope.departmentempissues = promise.departmentempissues;
                        $scope.departmentempissuesview = promise.departmentempissuesview;
                        $scope.overallempissuesall = promise.overallempissuesall;
                        $scope.temp_overallempissuesall = promise.temp_overallempissuesall;

                        $scope.statusfilteredlist = []; $scope.priorityfilteredlist = []; $scope.empfilteredlist = [];
                        var statuslist = []; var prioritylist = []; var emplist = [];
                        angular.forEach($scope.departmentempissues, function (value) {
                            statuslist.push(value.ISMTCR_Status);
                            prioritylist.push(value.Priority);
                            emplist.push(value.AssignedTo);
                        });

                        var statusfilteredlist = statuslist.filter(function (value, index) { return statuslist.indexOf(value) == index });
                        $scope.priorityfilteredlist = prioritylist.filter(function (value, index) { return prioritylist.indexOf(value) == index });
                        $scope.empfilteredlist = emplist.filter(function (value, index) { return emplist.indexOf(value) == index });

                        $scope.statusfilteredlist = [];
                        angular.forEach(statusfilteredlist, function (val) {
                            var count = 0;
                            angular.forEach($scope.departmentempissues, function (emp) {
                                if (emp.ISMTCR_Status.toLowerCase() == val.toLowerCase()) {
                                    count = count + emp.Rcount;
                                }
                            });
                            $scope.statusfilteredlist.push({ status: val, count: count });
                        });

                        $scope.statuswiseemployees = [];
                        angular.forEach($scope.empfilteredlist, function (emp) {
                            $scope.statuswiseemployees.push({ name: emp, subarray: [] });
                        });

                        angular.forEach($scope.statuswiseemployees, function (emp) {
                            var count = 0;
                            var pendingcount = 0;
                            var closecount = 0;
                            var opencount = 0;
                            angular.forEach($scope.departmentempissues, function (issue) {
                                if (emp.name == issue.AssignedTo) {
                                    count = count + issue.Rcount;
                                    if (issue.ISMTCR_Status != null && issue.ISMTCR_Status != undefined) {
                                        if (issue.ISMTCR_Status.toLowerCase() != "close") {
                                            pendingcount = pendingcount + issue.Rcount;
                                        }
                                        if (issue.ISMTCR_Status.toLowerCase() == "close") {
                                            closecount = closecount + issue.Rcount;
                                        }
                                        if (issue.ISMTCR_Status.toLowerCase() == "open" || issue.ISMTCR_Status.toLowerCase() == "reopen") {
                                            opencount = opencount + issue.Rcount;
                                        }
                                    }
                                    emp.subarray.push({ status: issue.ISMTCR_Status, priority: issue.Priority, rcount: issue.Rcount, hrme_id: issue.AssignedToEmpId, departid: issue.AssgToDeptId });
                                    emp.empid = issue.AssignedToEmpId;
                                    emp.departid = issue.AssgToDeptId;
                                    emp.department = issue.CreatedOnDepartment;
                                }
                            });
                            emp.count = count;
                            emp.pendingcount = pendingcount;
                            emp.closecount = closecount;
                            emp.opencount = opencount;
                        });
                    }
                    else {
                        countswal = countswal + 1;
                    }

                    if (promise.get_department != null && promise.get_department.length > 0) {
                        $scope.HRME_Id = promise.get_department[0].hrmE_Id;
                        $scope.empcode = promise.get_department[0].regno;
                        $scope.employeename = promise.get_department[0].hrmE_EmployeeFirstName;
                    }


                    if (promise.overallempissuesall != null && promise.overallempissuesall.length > 0) {

                        $scope.overallempissuesall = promise.overallempissuesall;
                        $scope.temp_overallempissuesall = promise.temp_overallempissuesall;


                        $scope.statusfilteredlistemp = []; $scope.priorityfilteredlistemp = []; $scope.empfilteredlistemp = []; $scope.clientfilteredlistemp = [];
                        var statuslistemp = []; var prioritylistemp = []; var emplistemp = []; var clientlistemp = [];
                        angular.forEach($scope.overallempissuesall, function (value) {
                            statuslistemp.push(value.ISMTCR_Status);
                            prioritylistemp.push(value.Priority);
                            emplistemp.push(value.AssignedTo);
                            clientlistemp.push(value.ISMMCLT_ClientName);
                        });

                        var statusfilteredlistemp = statuslistemp.filter(function (value, index) { return statuslistemp.indexOf(value) == index });
                        $scope.priorityfilteredlistemp = prioritylistemp.filter(function (value, index) { return prioritylistemp.indexOf(value) == index });
                        $scope.empfilteredlistemp = emplistemp.filter(function (value, index) { return emplistemp.indexOf(value) == index });
                        $scope.clientfilteredlistemp = clientlistemp.filter(function (value, index) { return clientlistemp.indexOf(value) == index });

                        $scope.statusfilteredlistemp = [];
                        angular.forEach(statusfilteredlistemp, function (val) {
                            var count = 0;
                            angular.forEach($scope.overallempissuesall, function (emp) {
                                if (emp.ISMTCR_Status.toLowerCase() == val.toLowerCase()) {
                                    count = count + emp.Rcount;
                                }
                            });
                            $scope.statusfilteredlistemp.push({ status: val, count: count });
                        });


                        $scope.statuswiseemployeesemp = [];
                        angular.forEach($scope.clientfilteredlistemp, function (client) {
                            $scope.statuswiseemployeesemp.push({ name: client, subarray: [] });
                        });

                        angular.forEach($scope.statuswiseemployeesemp, function (emp) {
                            var count = 0;
                            var pendingcount = 0;
                            var closecount = 0;
                            var opencount = 0;
                            var pendingpecentage = 0;
                            var closepercentage = 0;
                            angular.forEach($scope.overallempissuesall, function (issue) {
                                if (emp.name == issue.ISMMCLT_ClientName) {
                                    count = count + issue.Rcount;
                                    if (issue.ISMTCR_Status != null && issue.ISMTCR_Status != undefined) {
                                        if (issue.ISMTCR_Status.toLowerCase() != "close") {
                                            pendingcount = pendingcount + issue.Rcount;
                                        }
                                        if (issue.ISMTCR_Status.toLowerCase() == "close") {
                                            closecount = closecount + issue.Rcount;
                                        }
                                        if (issue.ISMTCR_Status.toLowerCase() == "open" || issue.ISMTCR_Status.toLowerCase() == "reopen") {
                                            opencount = opencount + issue.Rcount;
                                        }
                                    }
                                    emp.subarray.push({ status: issue.ISMTCR_Status, priority: issue.Priority, rcount: issue.Rcount, hrme_id: issue.AssignedToEmpId });
                                    emp.empid = issue.AssignedTo;
                                }
                            });
                            emp.count = count;
                            emp.pendingcount = pendingcount;
                            emp.closecount = closecount;
                            emp.opencount = opencount;
                            emp.pendingpecentage = parseFloat(pendingpecentage + (pendingcount * 100) / count).toFixed(2);
                            emp.closepercentage = parseFloat(closepercentage + (closecount * 100) / count).toFixed(2);
                        });
                    }

                    if (countswal == 6) {
                        swal("No Records Found!!");
                    }
                }
                else {
                    swal("Please Regenerate Report!!");
                    $state.reload();
                }
            });

        };


        $scope.ShowReportdata = function () {
            $scope.submitted = true;
            $scope.disablemonth = true;
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
                    "rdbbutton": $scope.allind
                };

                apiService.create("FrontOffice/getmonthreport", data).then(function (promise) {

                    $scope.institutename = promise.institutename;

                    //var doob = $scope.ddate == null ? "" : $filter('date')($scope.ddate, "yyyy-MM-dd");
                    //var doobyr = doob.substring(0, 4);
                    //var doobmnth = doob.substring(5, 7);
                    //var doobdays = doob.substring(8, 10);

                    $scope.salaryGenerated = promise.salaryGenerated;
                    if ($scope.salaryGenerated.length > 0) {
                        swal("Salary Is  Calculated For This Month Please Contact Account Department");
                        //$scope.Freezeattendence = true;
                        $scope.Freezeattendence = false;
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


                    for (var j = 0; j < promise.selectedActivityDetails1_stf.length; j++) {

                        // if (promise.selectedActivityDetails1_stf[j].hrmE_Id == 1675 || promise.selectedActivityDetails1_stf[j].hrmE_Id == 1887 || promise.selectedActivityDetails1_stf[j].hrmE_Id == 1770) {
                        //  if (promise.selectedActivityDetails1_stf[j].hrmE_Id == 1788) {
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
                            if (LateInEarlyOutDetails != null && LateInEarlyOutDetails.length > 0) {
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



                            //rating start
                            var emprating = [];
                            if (promise.employeeRetingdata != null && promise.employeeRetingdata != undefined && promise.employeeRetingdata.length > 0) {
                                var empratingdetails = $filter('filter')(promise.employeeRetingdata, function (d) {
                                    return d.HRME_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id;
                                });
                                if (empratingdetails != null) {
                                    angular.forEach(empratingdetails, function (com) {
                                        emprating.push({ Month1: com.Month1, Point: com.Point });   
                                    });
                                }

                            }
                            //rating End


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
                                            if (leaverptDetails[i].Absentday > 0.5 && leaverptDetails[i].Approvedday === 0) {
                                                $scope.unauthleavedatesdisplay = new Date(leaverptDetails[i].DrNotSentDates).getDate() + "(F)"
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

                            if (halffullDetails != null && halffullDetails.length > 0 && $scope.workdyas != 0) {
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
                                                    //else if (halffullDetails[i].DaysLop == "F") {
                                                    //    $scope.unauthleavecount = parseFloat($scope.unauthleavecount) + 1;
                                                    //    $scope.emptotlop = parseFloat($scope.emptotlop) + 1;
                                                    //    if ($scope.unauthleavedates == "") {
                                                    //        $scope.unauthleavedates = new Date(halffullDetails[i].punchdate).getDate();
                                                    //        if ($scope.unauthleavedatesdisplay == "") {
                                                    //            $scope.unauthleavedatesdisplay = new Date(halffullDetails[i].punchdate).getDate() + "(F)";
                                                    //        }
                                                    //        else {
                                                    //            $scope.unauthleavedatesdisplay = $scope.unauthleavedatesdisplay + ", " + new Date(halffullDetails[i].punchdate).getDate() + "(F)";
                                                    //        }
                                                    //        $scope.addholidaylop(new Date(halffullDetails[i].punchdate).getDate().toString(), 'F');
                                                    //    }
                                                    //    else {
                                                    //        $scope.unauthleavedates = $scope.unauthleavedates + ", " + new Date(halffullDetails[i].punchdate).getDate();
                                                    //        if ($scope.unauthleavedatesdisplay == "") {
                                                    //            $scope.unauthleavedatesdisplay = new Date(halffullDetails[i].punchdate).getDate() + "(F)";
                                                    //        }
                                                    //        else {
                                                    //            $scope.unauthleavedatesdisplay = $scope.unauthleavedatesdisplay + ", " + new Date(halffullDetails[i].punchdate).getDate() + "(F)";
                                                    //        }
                                                    //        $scope.addholidaylop(new Date(halffullDetails[i].punchdate).getDate().toString(), 'F');
                                                    //    }
                                                    //}
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
                                                //else if (halffullDetails[i].DaysLop == "F") {
                                                //    //&& halffullDetails[i].FOEPD_InOutFlg != "" && halffullDetails[i].DaysLop != null
                                                //    $scope.unauthleavecount = parseFloat($scope.unauthleavecount) + 1;
                                                //    $scope.emptotlop = parseFloat($scope.emptotlop) + 1;
                                                //    if ($scope.unauthleavedates == "") {
                                                //        $scope.unauthleavedates = new Date(halffullDetails[i].punchdate).getDate();
                                                //        $scope.unauthleavedatesdisplay = new Date(halffullDetails[i].punchdate).getDate() + "(F)";
                                                //        $scope.addholidaylop(new Date(halffullDetails[i].punchdate).getDate().toString(), 'F');
                                                //    }
                                                //    else {
                                                //        $scope.unauthleavedates = $scope.unauthleavedates + ", " + new Date(halffullDetails[i].punchdate).getDate();
                                                //        $scope.unauthleavedatesdisplay = $scope.unauthleavedatesdisplay + ", " + new Date(halffullDetails[i].punchdate).getDate() + "(F)";
                                                //        $scope.addholidaylop(new Date(halffullDetails[i].punchdate).getDate().toString(), 'F');
                                                //    }
                                                //}
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

                                var employeepuncheddates = $filter('filter')(promise.punchdates, function (d) {
                                    return d.HRME_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id;
                                });

                                var arr3 = employeepuncheddates[0].punchdate.split(',');

                                for (var i = 0; i < promise.holidaylist.length; i++) {
                                    var holidaydate = new Date(promise.holidaylist[i].fomhwdD_FromDate).getDate();



                                    var punched = $filter('filter')(arr3, function (d) {
                                        var datepunched = new Date(d).getDate()

                                        return datepunched == holidaydate
                                    });




                                    $scope.unauthleavedatesrevised = $scope.unauthleavedatestemp.replace(/[\s]/g, '');
                                    var arr = $scope.unauthleavedatesrevised.split(',');

                                    if ((arr2.indexOf((holidaydate + 1).toString()) > -1) && punched.length == 0) {
                                        if ((arr.indexOf((holidaydate - 1).toString()) > -1) && (arr.indexOf((holidaydate + 2).toString()) > -1)) {
                                            $scope.holidayleavecount = $scope.holidayleavecount + 2;
                                            $scope.holidayleavedates = $scope.holidayleavedates + ", " + new Date(promise.holidaylist[i].fomhwdD_FromDate).getDate(); $scope.holidayleavedatesdisplay = $scope.holidayleavedatesdisplay + ", " + new Date(promise.holidaylist[i].fomhwdD_FromDate).getDate() + "(P)";
                                            $scope.holidayleavedates = $scope.holidayleavedates + ", " + (parseInt(new Date(promise.holidaylist[i].fomhwdD_FromDate).getDate()) + 1).toString();
                                            $scope.holidayleavedatesdisplay = $scope.holidayleavedatesdisplay + ", " + (parseInt(new Date(promise.holidaylist[i].fomhwdD_FromDate).getDate()) + 1).toString() + "(P)";
                                            i = i + 1;
                                        }
                                    }
                                    else if ((arr.indexOf((holidaydate - 1).toString()) > -1) && (arr.indexOf((holidaydate - 2).toString()) > -1) && (arr.indexOf((holidaydate - 3).toString()) > -1) && punched.length == 0) {
                                        $scope.holidayleavecount = $scope.holidayleavecount + 1;
                                        $scope.holidayleavedates = $scope.holidayleavedates + ", " + new Date(promise.holidaylist[i].fomhwdD_FromDate).getDate();
                                        $scope.holidayleavedatesdisplay = $scope.holidayleavedatesdisplay + ", " + new Date(promise.holidaylist[i].fomhwdD_FromDate).getDate() + "(P)";
                                    }
                                    else if ((arr.indexOf((holidaydate + 1).toString()) > -1) && (arr.indexOf((holidaydate + 2).toString()) > -1) && (arr.indexOf((holidaydate + 3).toString()) > -1) && punched.length == 0) {
                                        $scope.holidayleavecount = $scope.holidayleavecount + 1;
                                        $scope.holidayleavedates = $scope.holidayleavedates + ", " + new Date(promise.holidaylist[i].fomhwdD_FromDate).getDate();
                                        $scope.holidayleavedatesdisplay = $scope.holidayleavedatesdisplay + ", " + new Date(promise.holidaylist[i].fomhwdD_FromDate).getDate() + "(P)";
                                    }
                                    else if ((arr.indexOf((holidaydate - 1).toString()) > -1) && (arr.indexOf((holidaydate + 1).toString()) > -1) && punched.length == 0) {
                                        $scope.holidayleavecount = $scope.holidayleavecount + 1;
                                        $scope.holidayleavedates = $scope.holidayleavedates + ", " + new Date(promise.holidaylist[i].fomhwdD_FromDate).getDate();
                                        $scope.holidayleavedatesdisplay = $scope.holidayleavedatesdisplay + ", " + new Date(promise.holidaylist[i].fomhwdD_FromDate).getDate() + "(P)";
                                    }

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
                                    var lopadjesteddates = "";
                                    angular.forEach(approvedleave, function (leaves) {
                                        if (add.leavename == leaves.leavename) {
                                            leaveasadd += parseFloat(leaves.Approvedday);

                                            if (leaves.lopadjest == true) {
                                                //if (leavesshowdates == "") {
                                                //    leavesshowdates = new Date(leaves.DrNotSentDates).getDate() + " (" + leaves.Approvedday + ")-CA";
                                                //}
                                                //else {
                                                //    leavesshowdates = leavesshowdates + "," + new Date(leaves.DrNotSentDates).getDate() + " (" + leaves.Approvedday + ")-CA";
                                                //}
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

                            //new con call added A
                            if (promise.sms_mail_count != null && promise.sms_mail_count.length > 0) {
                                var concallextention1 = $filter('filter')(promise.sms_mail_count, function (d) {
                                    return d.hrmE_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id;
                                });

                                var concallexdates = "";
                                if (concallextention1 != null && concallextention1.length > 0) {
                                    angular.forEach(concallextention1, function (mon) {
                                        if (concallexdates == "") {
                                            concallexdates = new Date(mon.hralD_Date).getDate();
                                        } else {
                                            concallexdates = concallexdates + ", " + new Date(mon.hralD_Date).getDate();
                                        }
                                    });
                                }
                            }
                            if (promise.employeeTypeList != null && promise.employeeTypeList.length > 0) {
                                var concallwithlopemp = $filter('filter')(promise.employeeTypeList, function (d) {
                                    return d.hrmE_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id;
                                });
                                if (concallwithlopemp.length > 0) {
                                    $scope.concallwithlopempcounct = concallwithlopemp[0].Nooflop
                                }
                            }
                            //con call end



                            $scope.addinallopdetails = promise.lunchdata;
                            var additionallopdetails = $filter('filter')(promise.lunchdata, function (d) {
                                return d.hrmE_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id;
                            });


                            var addinallopdates = "";
                            var addinallop = 0

                            if (additionallopdetails != null && additionallopdetails.length > 0) {
                                angular.forEach(additionallopdetails, function (mon) {
                                    addinallopdates = addinallopdates + "," + new Date(mon.hralD_Date).getDate()
                                    if (mon.hralD_Type != "Food Lop") {
                                        //addinallop = addinallop + 0.5; //lop concall old 
                                        addinallop = addinallop + (0.5 * $scope.concallwithlopempcounct); //lop concall new added A                                                                           
                                    }
                                });
                            }

                            $scope.emptotlop = parseFloat($scope.emptotlop) + parseFloat(addinallop);
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

                            //DR Rejected Count 

                            var rcdetails = $filter('filter')(promise.get_mi_list1, function (d) {
                                return d.HRME_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id;
                            });
                            if (rcdetails.length > 0) {
                                $scope.rcleavecounct = rcdetails.length;
                            }
                            else {
                                $scope.rcleavecounct = 0
                            }


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
                                missingpunchApproveddates.push({ missingpunchdate: missingpunchdate });
                            }
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

                            //HR COMMENT DETAILS
                            if ($scope.emptotlop >= $scope.numberofdays) {
                                $scope.emptotlop = $scope.numberofdays;
                                if ($scope.workdyas == 0 && $scope.newjoin === true) {
                                    $scope.emptotlop = $scope.emptotlop - $scope.latejoin;
                                }
                            }



                            var compoffapproveddetails = [];
                            compoffapproveddetails = $filter('filter')(promise.compoffdetails, function (d) {
                                return d.HRME_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id;
                            });





                            var compoffeligibility = [];
                            compoffeligibility = $filter('filter')(promise.compoffadjtelig, function (d) {
                                return d.HRME_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id;
                            });


                            var FinalLop = 0;
                            var nooffcompoffadjested = 0;
                            var compoffadjestdate = [];
                            $scope.compoffadjestedlop = 0;
                            var compoffapprovedcount = 0;
                            var compoffbalance = 0;
                            var Totalcompoffbalance = 0;
                            if ((compoffeligibility != null && compoffeligibility.length > 0) || compoffapproveddetails != null && compoffapproveddetails.length > 0) {
                                if (compoffeligibility != null && compoffeligibility.length > 0) {
                                    Totalcompoffbalance = compoffeligibility[0].HRELS_CBLeaves;
                                    compoffbalance = compoffeligibility[0].HRELS_CBLeaves;
                                }

                                var latedateafter = "0," + temp_array[0].latedateafter
                                if (latedateafter.length > 0) {
                                    var latedateafterdate = new Array();
                                    latedateafterdate = latedateafter.split(",");
                                    angular.forEach(latedateafterdate, function (lated) {
                                        if (lated > 0) {


                                            var dates = lated + "-" + $scope.month + "-" + $scope.year
                                            var dates1 = $scope.year + "-" + $scope.month + "-" + lated
                                            var latein = [];
                                            latein = $filter('filter')(compoffapproveddetails, function (d) {
                                                var filterdate = dates1 == null ? "" : $filter('date')(new Date(dates1), "yyyy-MM-dd");
                                                var filterdate1 = d.HRELAP_FromDate == null ? "" : $filter('date')(d.HRELAP_FromDate, "yyyy-MM-dd");
                                                return d.HRME_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id && filterdate == filterdate1;
                                            });

                                            if (latein != null && latein.length > 0) {
                                                compoffadjestdate.push({ HRME_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, Type: "Late In", Date: dates, LOP: 0.5, Status: "Approved" });
                                                compoffapprovedcount = compoffapprovedcount + 0.5
                                            }
                                            else if (compoffbalance > 0) {
                                                compoffadjestdate.push({ HRME_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, Type: "Late In", Date: dates, LOP: 0.5, Status: "Approved" });
                                                compoffbalance = compoffbalance - 0.5
                                            }





                                            //var dates = lated + "-" + $scope.month + "-" + $scope.year
                                            //compoffadjestdate.push({ HRME_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, Type: "Late In", Date: dates, LOP: 0.5, Status: "Approved", Totalcompoffbalance: Totalcompoffbalance });
                                            //compoffbalance = compoffbalance - 0.5
                                        }

                                    });
                                }

                                var dailyrptndates = "0," + $scope.dailyrptndates
                                if (dailyrptndates.length > 0) {

                                    var drnotsentdate = new Array();
                                    drnotsentdate = dailyrptndates.split(",");
                                    angular.forEach(drnotsentdate, function (drns) {
                                        if (drns > 0) {
                                            var dates = drns + "-" + $scope.month + "-" + $scope.year
                                            var dates1 = $scope.year + "-" + $scope.month + "-" + drns
                                            var drnotsentdate = [];
                                            drnotsentdate = $filter('filter')(compoffapproveddetails, function (d) {
                                                var filterdate = dates1 == null ? "" : $filter('date')(new Date(dates1), "yyyy-MM-dd");
                                                var filterdate1 = d.HRELAP_FromDate == null ? "" : $filter('date')(d.HRELAP_FromDate, "yyyy-MM-dd");
                                                return d.HRME_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id && filterdate == filterdate1;
                                            });

                                            if (drnotsentdate != null && drnotsentdate.length > 0) {

                                                compoffadjestdate.push({ HRME_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, Type: "Dr Not Sent", Date: dates, LOP: 0.5, Status: "Approved" });
                                                compoffapprovedcount = compoffapprovedcount + 0.5
                                            }
                                            else if (compoffbalance > 0) {
                                                compoffadjestdate.push({ HRME_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, Type: "Dr Not Sent", Date: dates, LOP: 0.5, Status: "Approved" });
                                                compoffbalance = compoffbalance - 0.5
                                            }



                                        }
                                    });
                                }

                                var addinallopdatestemp = "," + addinallopdates
                                addinallopdatestemp = "0," + addinallopdatestemp
                                if (addinallopdatestemp.length > 0) {
                                    var addinallopdatesarr = new Array();
                                    addinallopdatesarr = addinallopdatestemp.split(",");
                                    angular.forEach(addinallopdatesarr, function (additional) {
                                        //if (additional != '' && additional > 0) {
                                        //    if (compoffbalance > 0) {
                                        //        var dates = additional + "-" + $scope.month + "-" + $scope.year
                                        //        compoffadjestdate.push({ HRME_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, Type: "Additional LOP", Date: dates, LOP: 0.5, Status: "Approved", Totalcompoffbalance: Totalcompoffbalance });
                                        //        compoffbalance = compoffbalance - 0.5
                                        //    }
                                        //}


                                        if (additional > 0) {
                                            var dates = additional + "-" + $scope.month + "-" + $scope.year
                                            var dates1 = $scope.year + "-" + $scope.month + "-" + additional
                                            var additionallop = [];
                                            additionallop = $filter('filter')(compoffapproveddetails, function (d) {
                                                var filterdate = dates1 == null ? "" : $filter('date')(new Date(dates1), "yyyy-MM-dd");
                                                var filterdate1 = d.HRELAP_FromDate == null ? "" : $filter('date')(d.HRELAP_FromDate, "yyyy-MM-dd");
                                                return d.HRME_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id && filterdate == filterdate1;
                                            });

                                            if (additionallop != null && additionallop.length > 0) {

                                                compoffadjestdate.push({ HRME_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, Type: "Additional LOP", Date: dates, LOP: 0.5, Status: "Approved" });
                                                compoffapprovedcount = compoffapprovedcount + 0.5
                                            }
                                            else if (compoffbalance > 0) {
                                                compoffadjestdate.push({ HRME_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, Type: "Additional LOP", Date: dates, LOP: 0.5, Status: "Approved" });
                                                compoffbalance = compoffbalance - 0.5
                                            }



                                        }



                                    });
                                }

                                if (temp_array[0].earlydateafter > 0) {
                                    var earlydateafter = "," + temp_array[0].earlydateafter;
                                    var earlydateafterdate = new Array();
                                    earlydateafterdate = earlydateafter.split(",");
                                    angular.forEach(earlydateafterdate, function (early) {
                                        if (early > 0) {
                                            //var dates = early + "-" + $scope.month + "-" + $scope.year
                                            //var dates1 = $scope.year + "-" + $scope.month + "-" + early
                                            //var status = "Applied"
                                            //if (compoffapproveddetails != null && compoffapproveddetails.length > 0) {
                                            //    var earlyoutapprovedcount = [];
                                            //    earlyoutapprovedcount = $filter('filter')(compoffapproveddetails, function (d) {

                                            //        var filterdate = dates1 == null ? "" : $filter('date')(new Date(dates1), "yyyy-MM-dd");
                                            //        var filterdate1 = d.HRELAP_FromDate == null ? "" : $filter('date')(d.HRELAP_FromDate, "yyyy-MM-dd");
                                            //        return d.HRME_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id && filterdate == filterdate1;
                                            //    });
                                            //    if (compoffapproveddetails != null && earlyoutapprovedcount.length > 0) {
                                            //        status = earlyoutapprovedcount[0].HRELAP_ApplicationStatus;
                                            //    }
                                            //}


                                            var status = "Applied"
                                            var dates = early + "-" + $scope.month + "-" + $scope.year
                                            var dates1 = $scope.year + "-" + $scope.month + "-" + early
                                            var earlyout = [];
                                            earlyout = $filter('filter')(compoffapproveddetails, function (d) {
                                                var filterdate = dates1 == null ? "" : $filter('date')(new Date(dates1), "yyyy-MM-dd");
                                                var filterdate1 = d.HRELAP_FromDate == null ? "" : $filter('date')(d.HRELAP_FromDate, "yyyy-MM-dd");
                                                return d.HRME_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id && filterdate == filterdate1 && d.earlyout > 0;
                                            });
                                            if (earlyout != null && earlyout.length > 0 && earlyout[0].HRELAP_ApplicationStatus == "Approved") {
                                                compoffapprovedcount = compoffapprovedcount + 0.5
                                                status = "Approved"
                                            }

                                            if (earlyout != null && earlyout.length > 0) {
                                                compoffadjestdate.push({ HRME_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, Type: "Early Out", Date: dates, LOP: 0.5, Status: status });
                                            }
                                            else if (compoffbalance > 0) {
                                                compoffadjestdate.push({ HRME_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, Type: "Early Out", Date: dates, LOP: 0.5, Status: status });
                                            }






                                            //compoffadjestdate.push({ HRME_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, Type: "Early Out", Date: dates, LOP: 0.5, Status: status, Totalcompoffbalance: Totalcompoffbalance });
                                            //if (status == "Approved") {
                                            //    compoffbalance = compoffbalance - 0.5
                                            //}

                                        }
                                    });
                                }

                                var unauthleavedatesdisplay = $scope.unauthleavedatesdisplay
                                unauthleavedatesdisplay = "," + unauthleavedatesdisplay
                                if (unauthleavedatesdisplay.length > 0) {
                                    var unauthorisedleavedate = new Array();
                                    unauthorisedleavedate = unauthleavedatesdisplay.split(",");
                                    var currentcompoffbalance = compoffbalance;
                                    angular.forEach(unauthorisedleavedate, function (ual) {
                                        var datewithcar = ual
                                        ual = ual.replace(/F/g, '').replace(/\P/g, '').replace(/\H/g, '').replace(/\(/g, '').replace(/\)/g, '');
                                        if (ual > 0) {


                                            var LOPs = 0;
                                            if (datewithcar.indexOf('H') > 0) {
                                                LOPs = 0.5;
                                            }
                                            else {
                                                LOPs = 1;
                                            }


                                            var status = "Applied"
                                            var dates = ual + "-" + $scope.month + "-" + $scope.year
                                            var dates1 = $scope.year + "-" + $scope.month + "-" + ual
                                            var unauthorisedadjestedcount = [];
                                            unauthorisedadjestedcount = $filter('filter')(compoffapproveddetails, function (d) {
                                                var filterdate = dates1 == null ? "" : $filter('date')(new Date(dates1), "yyyy-MM-dd");
                                                var filterdate1 = d.HRELAP_FromDate == null ? "" : $filter('date')(d.HRELAP_FromDate, "yyyy-MM-dd");
                                                return d.HRME_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id && filterdate == filterdate1 && d.unauth > 0;
                                            });


                                            if (unauthorisedadjestedcount != null && unauthorisedadjestedcount.length > 0 && unauthorisedadjestedcount[0].HRELAP_ApplicationStatus == "Approved") {
                                                compoffapprovedcount = compoffapprovedcount + LOPs
                                                status = "Approved"
                                            }
                                            else if (unauthorisedadjestedcount != null && unauthorisedadjestedcount.length > 0 && unauthorisedadjestedcount[0].HRELAP_ApplicationStatus == "Applied") {
                                                compoffbalance = compoffbalance - LOPs
                                            }

                                            if (unauthorisedadjestedcount != null && unauthorisedadjestedcount.length > 0) {
                                                compoffadjestdate.push({ HRME_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, Type: "Unauthorised Leave", Date: dates, LOP: LOPs, Status: status });

                                            }
                                            else if (compoffbalance > 0) {
                                                compoffadjestdate.push({ HRME_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, Type: "Unauthorised Leave", Date: dates, LOP: LOPs, Status: status });
                                                compoffbalance = compoffbalance - LOPs
                                            }











                                            //var dates = ual + "-" + $scope.month + "-" + $scope.year
                                            //var dates1 = $scope.year + "-" + $scope.month + "-" + ual
                                            //var status = "Applied"

                                            //if (compoffapproveddetails != null && compoffapproveddetails.length > 0) {
                                            //    var unauthorisedadjestedcount = [];
                                            //    unauthorisedadjestedcount = $filter('filter')(compoffapproveddetails, function (d) {
                                            //        var filterdate = dates1 == null ? "" : $filter('date')(new Date(dates1), "yyyy-MM-dd");
                                            //        var filterdate1 = d.HRELAP_FromDate == null ? "" : $filter('date')(d.HRELAP_FromDate, "yyyy-MM-dd");

                                            //        return d.HRME_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id && filterdate == filterdate1;
                                            //    });

                                            //    if (unauthorisedadjestedcount != null && unauthorisedadjestedcount.length > 0) {
                                            //        status = unauthorisedadjestedcount[0].HRELAP_ApplicationStatus;
                                            //    }


                                            //}


                                            //if (datewithcar.indexOf('H') > 0) {
                                            //    compoffadjestdate.push({ HRME_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, Type: "Unauthorised Leave", Date: dates, LOP: 0.5, Status: status, Totalcompoffbalance: Totalcompoffbalance });

                                            //    if (compoffapproveddetails != null && compoffapproveddetails.length > 0 && unauthorisedadjestedcount != null && unauthorisedadjestedcount.length>0 && unauthorisedadjestedcount[0].HRELAP_ApplicationStatus == "Approved") {
                                            //        if (status == "Approved") {
                                            //            compoffbalance = compoffbalance - 0.5
                                            //        }
                                            //        else {
                                            //            currentcompoffbalance = currentcompoffbalance - 0.5
                                            //        }
                                            //    }
                                            //}
                                            //else {
                                            //    compoffadjestdate.push({ HRME_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, Type: "Unauthorised Leave", Date: dates, LOP: 1.0, Status: status, Totalcompoffbalance: Totalcompoffbalance });
                                            //    if (status == "Approved") {
                                            //        compoffbalance = compoffbalance - 1
                                            //    }
                                            //    else {
                                            //        currentcompoffbalance = currentcompoffbalance - 1
                                            //    }
                                            //}

                                        }

                                    });
                                }

                                //if (compoffeligibility != null && compoffeligibility.length > 0) {
                                //    nooffcompoffadjested = compoffeligibility[0].HRELS_CBLeaves - compoffbalance;
                                //    }


                                var totalcalculated = 0;
                                //var unauthorisedadjestedcount = $filter('filter')(compoffadjestdate, function (d) {
                                //    totalcalculated = totalcalculated + d.LOP
                                //    return d.HRME_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id && d.Status == "Approved";
                                //});


                                angular.forEach(compoffadjestdate, function (d) {
                                    if (d.HRME_Id == promise.selectedActivityDetails1_stf[j].hrmE_Id && d.Status == "Approved") {
                                        totalcalculated = totalcalculated + d.LOP
                                    }
                                });


                                nooffcompoffadjested = totalcalculated;

                                // var totalcalculated = 0;
                                // totalcalculated = Totalcompoffbalance - compoffbalance;
                                //nooffcompoffadjested = compoffapprovedcount + totalcalculated;


                                FinalLop = $scope.emptotlop - nooffcompoffadjested;

                            }
                            else {
                                FinalLop = $scope.emptotlop
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
                                    //if (nooffcompoffadjested>0) {                                    
                                    //    hrcommentDetails[0].vmscrT_Remarks = hrcommentDetails[0].vmscrT_Remarks + "\n " + nooffcompoffadjested +" Comp Off Adjested To LOP";
                                    //}
                                    newCol1 = { empdate: temp_array, emprating: emprating, rejdrdates: $scope.rejdailyrptndates, rdrcount: $scope.rejdailyrptncount, rccount: $scope.rcleavecounct, hrmE_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, ecode: promise.selectedActivityDetails1_stf[j].hrmE_EmployeeCode, ename: promise.selectedActivityDetails1_stf[j].hrmE_EmployeeFirstName, desgname: promise.selectedActivityDetails1_stf[j].hrmE_PerStreet, drcount: $scope.dailyrptncount, drdates: $scope.dailyrptndates, halfdrcount: $scope.dailyrpthalfdaycount, halfdrdates: $scope.dailyrpthalfdaydates, leavecount: $scope.unauthleavecount, leavedates: $scope.unauthleavedatesdisplay, blkdrcount: $scope.dailyrptblockedcount, blkdrdates: $scope.dailyrptblockeddates, totlop: $scope.emptotlop, HR_EO: hrcommentDetails[0].vmscrT_HREO, HR_DR: hrcommentDetails[0].vmscrT_HRDR, HR_HDR: hrcommentDetails[0].vmscrT_HRHDR, HR_BDR: hrcommentDetails[0].vmscrT_HRBDR, HR_LC: hrcommentDetails[0].vmscrT_HRLC, HR_UL: hrcommentDetails[0].vmscrT_HRUL, HR_LOP: FinalLop, HR_REM: hrcommentDetails[0].vmscrT_Remarks, vmscrT_DOJ: promise.selectedActivityDetails1_stf[j].hrmE_DOJ, approvedleavedetails: leave_array, salarydetails: salary_array, latejoin: $scope.latejoin, worksdaycount: $scope.workdyas, newjoin: $scope.newjoin, leave_arraytemp: leave_arraytemp, dolnumberofdays: $scope.dolnumberofdays, dol: new Date(promise.selectedActivityDetails1_stf[j].hrmE_DOL), leftflag: promise.selectedActivityDetails1_stf[j].hrmE_LeftFlag, hrmE_Relaxtime: promise.selectedActivityDetails1_stf[j].hrmE_Relaxtime, ISMCLMER_Id: ISMCLMER_Id, ieflag: ieflag, addinallopdates: addinallopdates, concallexdates: concallexdates, addinallop: addinallop, compoffadjestdate: compoffadjestdate, nooffcompoffadjested: nooffcompoffadjested };
                                }
                                else {
                                    //HR_LOP: 0
                                    newCol1 = { empdate: temp_array, emprating: emprating, rejdrdates: $scope.rejdailyrptndates, rdrcount: $scope.rejdailyrptncount, rccount: $scope.rcleavecounct, hrmE_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, ecode: promise.selectedActivityDetails1_stf[j].hrmE_EmployeeCode, ename: promise.selectedActivityDetails1_stf[j].hrmE_EmployeeFirstName, desgname: promise.selectedActivityDetails1_stf[j].hrmE_PerStreet, drcount: $scope.dailyrptncount, drdates: $scope.dailyrptndates, halfdrcount: $scope.dailyrpthalfdaycount, halfdrdates: $scope.dailyrpthalfdaydates, leavecount: $scope.unauthleavecount, leavedates: $scope.unauthleavedatesdisplay, blkdrcount: $scope.dailyrptblockedcount, blkdrdates: $scope.dailyrptblockeddates, totlop: $scope.emptotlop, HR_EO: 0, HR_DR: 0, HR_HDR: 0, HR_BDR: 0, HR_LC: 0, HR_UL: 0, HR_LOP: FinalLop, HR_REM: '', vmscrT_DOJ: promise.selectedActivityDetails1_stf[j].hrmE_DOJ, approvedleavedetails: leave_array, salarydetails: salary_array, latejoin: $scope.latejoin, worksdaycount: $scope.workdyas, newjoin: $scope.newjoin, leave_arraytemp: leave_arraytemp, dolnumberofdays: $scope.dolnumberofdays, dol: new Date(promise.selectedActivityDetails1_stf[j].hrmE_DOL), leftflag: promise.selectedActivityDetails1_stf[j].hrmE_LeftFlag, hrmE_Relaxtime: promise.selectedActivityDetails1_stf[j].hrmE_Relaxtime, ISMCLMER_Id: ISMCLMER_Id, ieflag: ieflag, addinallopdates: addinallopdates, concallexdates: concallexdates, addinallop: addinallop, compoffadjestdate: compoffadjestdate, nooffcompoffadjested: nooffcompoffadjested };
                                }
                            }
                            else {
                                //HR_LOP: 0
                                newCol1 = { empdate: temp_array, emprating: emprating, rejdrdates: $scope.rejdailyrptndates, rdrcount: $scope.rejdailyrptncount, rccount: $scope.rcleavecounct, hrmE_Id: promise.selectedActivityDetails1_stf[j].hrmE_Id, ecode: promise.selectedActivityDetails1_stf[j].hrmE_EmployeeCode, ename: promise.selectedActivityDetails1_stf[j].hrmE_EmployeeFirstName, desgname: promise.selectedActivityDetails1_stf[j].hrmE_PerStreet, drcount: $scope.dailyrptncount, drdates: $scope.dailyrptndates, halfdrcount: $scope.dailyrpthalfdaycount, halfdrdates: $scope.dailyrpthalfdaydates, leavecount: $scope.unauthleavecount, leavedates: $scope.unauthleavedatesdisplay, blkdrcount: $scope.dailyrptblockedcount, blkdrdates: $scope.dailyrptblockeddates, totlop: $scope.emptotlop, HR_EO: 0, HR_DR: 0, HR_HDR: 0, HR_BDR: 0, HR_LC: 0, HR_UL: 0, HR_LOP: FinalLop, HR_REM: '', vmscrT_DOJ: promise.selectedActivityDetails1_stf[j].hrmE_DOJ, approvedleavedetails: leave_array, salarydetails: salary_array, latejoin: $scope.latejoin, worksdaycount: $scope.workdyas, newjoin: $scope.newjoin, leave_arraytemp: leave_arraytemp, dolnumberofdays: $scope.dolnumberofdays, dol: new Date(promise.selectedActivityDetails1_stf[j].hrmE_DOL), leftflag: promise.selectedActivityDetails1_stf[j].hrmE_LeftFlag, hrmE_Relaxtime: promise.selectedActivityDetails1_stf[j].hrmE_Relaxtime, ISMCLMER_Id: ISMCLMER_Id, ieflag: ieflag, addinallopdates: addinallopdates, concallexdates: concallexdates, addinallop: addinallop, compoffadjestdate: compoffadjestdate, nooffcompoffadjested: nooffcompoffadjested };
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
                        //}
                        // }
                    }
                    //for j

                    $scope.monthendreport = temp_array1;

                    angular.forEach($scope.monthendreport, function (mon) {
                        mon.selected = true;
                        $scope.SelectedAll = true;
                    });

                    if ($scope.monthendreport.length > 0) {
                        $scope.report = true;
                    }
                    else {
                        swal("No Records Found !");
                        $scope.report = false;
                    }

                });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.pdetails = function (empid) {
            $scope.proxyflag = false;
            $scope.proxydetails = [];
            var data = {
                "HRME_Id": empid,
            };

            apiService.create("FrontOffice/proxydetails", data).then(function (promise) {
                if (promise.proxylist !== null && promise.proxylist.length > 0) {
                    $scope.proxydetails = promise.proxylist
                    $scope.proxyflag = true;
                }
            });
        };

        $scope.eflag = false;
        $scope.Evaldetails = function (empid) {
            $scope.Average = "";
            $scope.evaluationdetails = [];
            var data = {
                "HRME_Id": empid,
                "Fromdate": $scope.from_date,
                "Todate": $scope.to_date,
                "yearid": $scope.year,
                "month": $scope.month,
                "rdbbutton": $scope.allind,
                "flags": "E"
            };

            apiService.create("FrontOffice/proxydetails", data).then(function (promise) {
                if (promise.consolidateSalaryReport !== null && promise.consolidateSalaryReport.length > 0) {
                    angular.forEach(promise.consolidateSalaryReport, function (itm) {
                        if (itm.Flag == "1") {
                            $scope.evaluationdetails.push(itm);
                        }
                        if (itm.Flag == "0") {
                            $scope.Average = itm.Point;
                            $scope.Employee_Name = itm.Employee_Name;
                        }
                    });

                    $scope.eflag = true;
                }
            });
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
                $scope.gpssavedetails = promise.admindata
                if ($scope.punchdetails !== null && $scope.punchdetails.length > 0) {
                    $scope.viewattendance = true;
                }
                else { $scope.viewattendance = false; }

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
                            //   if (late.punchdate == punch.punchdate && late.HRME_Id == punch.HRME_Id) {
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


                            var gpsintimedetails = $filter('filter')($scope.gpssavedetails, function (d) {
                                return d.HRME_Id == punch.HRME_Id && d.Intime == punch.intime && ($filter('date')(new Date(d.Punchdate), 'yyyy-MM-dd')) == ($filter('date')(new Date(punch.punchdate), 'yyyy-MM-dd'));
                            });

                            var gpsouttimedetails = $filter('filter')($scope.gpssavedetails, function (d) {
                                return d.HRME_Id == punch.HRME_Id && d.Outtime == punch.outtime && ($filter('date')(new Date(d.Punchdate), 'yyyy-MM-dd')) == ($filter('date')(new Date(punch.punchdate), 'yyyy-MM-dd'));
                            });
                            if (gpsintimedetails != null && gpsintimedetails.length > 0) {
                                punch.gpsintime = "GPS";
                            }
                            if (gpsouttimedetails != null && gpsouttimedetails.length > 0) {
                                punch.gpsouttime = "GPS";
                            }

                        });
                    });
                }
                if (($scope.punchdetailsprevious != null && $scope.punchdetailsprevious.length > 0) && (promise.lateindataprevious != null && promise.lateindataprevious.length > 0)) {
                    angular.forEach(promise.lateindataprevious, function (late) {
                        angular.forEach($scope.punchdetailsprevious, function (punch) {
                            //if (late.punchdate == punch.punchdate && late.HRME_Id == punch.HRME_Id) {
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
            });
        };
        $scope.RcLeaveDetails = function (emp, empname) {
            $scope.rcflag = false;
            $scope.rcleavedetails = [];
            $scope.selectedList = [];
            $scope.emppunchname = empname;
            var data = {
                "fromdate": $scope.from_date,
                "todate": $scope.to_date,
                "HRME_Id": emp,
                "yearid": $scope.year,
                "month": $scope.month,
                "flag": $scope.allind,
                "TypeFlg": "RCLeave",
            };

            apiService.create("ISMDashboard/getlateearlyoutreport", data).then(function (promise) {
                if (promise.griddata !== null && promise.griddata.length > 0) {
                    $scope.rcleavedetails = promise.griddata
                    $scope.rcflag = true;
                }
            });
        };


        $scope.SaveRc = function () {
            //  if ($scope.myForm.$valid) {
            var data = {

                "rcLeaveRecords": $scope.selectedList

            };
            apiService.create("FrontOffice/saveRc", data).then(function (promise) {
                if (promise.retrunMsg === 'Success') {
                    swal("Leave Updated successfully");
                    $state.reload();
                }
                else if (promise.retrunMsg === 'NotSuccess') {
                    swal("Failed to Save/Update");
                    $state.reload();
                }
            });
            //    }
        };
        $scope.cancel = function () {
            $state.reload();
        };

        $scope.selectedList = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all2;
            angular.forEach($scope.rcleavedetails, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all2 == true) {
                    $scope.selectedList.push(itm);
                }
                else {
                    $scope.selectedList.splice(itm);
                }
            });
        }

        $scope.optionToggled1 = function (SelectedRecord) {

            $scope.all2 = $scope.rcleavedetails.every(function (itm) { return itm.selected; });
            if ($scope.selectedList.indexOf(SelectedRecord) === -1) {
                $scope.selectedList.push(SelectedRecord);
            }
            else {
                $scope.selectedList.splice($scope.selectedList.indexOf(SelectedRecord), 1);
            }
        }












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
                excelname = excelname.toUpperCase() + ' - ' + $scope.ivrM_Month_Name + ' - ' + $scope.yearname + ' – Attendance Report.xls';
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



        $scope.Notapprovedstatus = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "yearid": $scope.year,
                    "month": $scope.month,
                };
                apiService.create("FrontOffice/Notapprovedstatus", data).
                    then(function (promise) {
                        if (promise.leavepending != null && promise.leavepending != undefined && promise.leavepending.length > 0) {
                            $scope.Notapprovedsts = promise.leavepending;
                            $('#Notapprovedleave').modal('show');
                        }
                        else {
                            $scope.Savedata();
                        }
                    });
            }
        }




        $scope.Savedata = function () {
            if ($scope.myForm.$valid) {
                if ($scope.monthendreport.length > 0) {
                    $scope.temp_array2 = [];
                    for (var x = 0; x < $scope.monthendreport.length; x++) {
                        if ($scope.monthendreport[x].selected == true) {
                            $scope.temp_array2.push($scope.monthendreport[x]);
                        }
                    }
                    if ($scope.temp_array2 != null && $scope.temp_array2.length > 0) {
                        //$scope.screenshot();
                        swal({
                            title: "Are you sure?",
                            text: "Do you want to save attendance?",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Save",
                            cancelButtonText: "Cancel",
                            closeOnConfirm: false,
                            closeOnCancel: false
                        },
                            function (isConfirm) {
                                if (isConfirm) {
                                    var data = {}
                                    if ($scope.allind == "Month" && $scope.month > 0) {
                                        data = {
                                            "yearid": $scope.year,
                                            "months": $scope.month,
                                            "ConsolidateReportTempDTO": $scope.temp_array2
                                        };
                                    }
                                    else {

                                        data = {
                                            "Fromdate": new Date($scope.from_date).toDateString(),
                                            "Todate": new Date($scope.to_date).toDateString(),
                                            "months": $scope.month,
                                            "ConsolidateReportTempDTO": $scope.temp_array2
                                        };

                                    }
                                    apiService.create("FrontOffice/savereport", data).
                                        then(function (promise) {
                                            if (promise.retrunMsg === 'False') {
                                                swal("Failed to Save/Update");
                                            }
                                            else if (promise.retrunMsg === 'True') {
                                                //swal('Record Saved Successfully.');
                                                //$state.reload();
                                                angular.forEach($scope.monthendreport, function (mon) {
                                                    mon.selected = false;
                                                    $scope.SelectedAll = false;
                                                });
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


        $scope.compoffadjesteddetails = function (val) {
            $scope.emppunchname = val.ename;
            $scope.Totalcompoffbalance = val.compoffadjestdate[0].Totalcompoffbalance;
            $scope.nooffcompoffadjested = val.nooffcompoffadjested;
            $scope.compoffadjestdate = val.compoffadjestdate;
            $('#compoffadjesteddetailsshow').modal('show');
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


        $scope.previewimg_new = function (img) {
            $scope.imagepreview = img;
            $scope.view_videos = [];
            var img = $scope.imagepreview;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {

                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');

            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
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
            // $scope.additionallopdetails=[];
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

