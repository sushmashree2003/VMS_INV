(function () {
    'use strict';
    angular
        .module('app', ['ngSanitize'])
        .controller('OnlineLeaveApplicationController', OnlineLeaveApplicationController)
    OnlineLeaveApplicationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sanitize', '$sce']
    function OnlineLeaveApplicationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sanitize, $sce) {

        $scope.obj = {};
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.compoffOdapplyedrecord = [];
        $scope.albumNameArraycolumn = [];
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian; todate //Old Code Changes
        };
        $scope.setmindate = new Date();
        $scope.itemsPerPage = 5;
        $scope.itemsPerPage2 = 10;
        $scope.currentPage = 1;
        $scope.currentPage2 = 1;
        $scope.employeeleaveDetails = [];
        $scope.eanableproxyflag = false;
        $scope.Hlfday = function (row) {
            //var rowsss = $scope.gridApi.selection.getSelectedRows();
            var tempdays = row.hrelaP_TotalDays;
            if (row.selected == true) {
                if (row.cklop == true) {
                    row.hrelaP_TotalDays = tempdays - 0.5;
                }
                else if (row.cklop == false) {
                    row.hrelaP_TotalDays = tempdays + 0.5;
                }
            }
        };

        $scope.timeconvertion = function (date_obj) {
            let hours = 0;
            let mins = 0;
            let time = "";
            let sep = ":"
            let ampm = "am"
            //var str = "18.24";
            var res = date_obj.split(":", 2);

            hours = res[0];
            mins = res[1];
            res[2] = mins;
            res[1] = sep;
            res[3] = ampm;
            if (hours >= 12 && mins > 0) {
                res[3] = "pm";
            }
            if (hours > 12) {
                res[0] = hours - 12;
            }

            time = res.join("");

            return time;
        }

        $scope.loadData = function () {

            var date = new Date();
            var id = 2;
            //$scope.all_check();
            apiService.getURI("OnlineLeaveApplication/getonlineLeave", id).then(function (promise) {
                $scope.online_leave = promise.online_leave;

                $scope.hrmE_EmployeeFirstName = $scope.online_leave[0].hrmE_EmployeeFirstName;
                $scope.hrmE_DOJ = $scope.online_leave[0].hrmE_DOJ;
                $scope.hrmdeS_DesignationName = $scope.online_leave[0].hrmdeS_DesignationName;
                $scope.hrmE_EmailId = $scope.online_leave[0].hrmE_EmailId;
                $scope.hrmE_MobileNo = $scope.online_leave[0].hrmE_MobileNo;
                $scope.hrmE_Id = $scope.online_leave[0].hrmE_Id;
                $scope.contact = $scope.online_leave[0].hrmE_MobileNo;

                if (promise.lappcount != null && promise.lappcount.length > 0) {
                    $scope.Appliedleavecon = promise.lappcount[0].AppliedleaveDays;
                }

                $scope.leave_nameview = promise.leave_name;

                if (promise.countrecon != undefined && promise.countrecon != null && promise.countrecon.length > 0) {
                    $scope.countrecon = promise.countrecon.length;
                }
                //angular.forEach($scope.leave_nameview, function (dd) {
                //    if (dd.hrmL_LeaveName === "Casual Leave") {
                //        $scope.CLbalancecount = dd.hrelS_CBLeaves;
                //    }
                //});
                $scope.CLbalance = promise.leave_name;
                $scope.leaveview = promise.leaveName;
                $scope.leaverestrictflag = false;
                if (promise.leaverestriction != undefined && promise.leaverestriction != null && promise.leaverestriction.length > 0) {
                    $scope.leaverestriction = promise.leaverestriction;

                    $scope.Rfromdate = $scope.leaverestriction[0].hrlrD_FromDate;
                    $scope.Rtodate = $scope.leaverestriction[0].hrlrD_ToDate;
                    $scope.RRemark = $scope.leaverestriction[0].hrlrD_Remarks;
                    $scope.leaverestrictflag = true;
                }

                if (promise.deviationdetails != undefined && promise.deviationdetails != null && promise.deviationdetails.length > 0) {
                    $scope.deviationpercentage = promise.deviationdetails[0].Deviation_Percentage;
                }


                angular.forEach($scope.leave_nameview, function (grdobj) {

                    if (grdobj.hrmL_WhenToApplyFlg == "Both") {
                        grdobj.fromminDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() - grdobj.hrmL_NoOfDays);
                        grdobj.frommaxDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() + grdobj.hrmL_MaxLeavesApplyPerMonth);
                        //grdobj.tominDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() - grdobj.hrmL_NoOfDays);
                        //grdobj.tomaxDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() + 30);
                    }
                    else if (grdobj.hrmL_WhenToApplyFlg == "Before") {
                        grdobj.fromminDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() + grdobj.hrmL_NoOfDays);
                        grdobj.frommaxDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() + 30);
                        //grdobj.tominDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() + grdobj.hrmL_NoOfDays );
                        //grdobj.tomaxDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() + grdobj.hrmL_NoOfDays + grdobj.hrmL_MaxLeavesApplyPerMonth);
                    }
                    else if (grdobj.hrmL_WhenToApplyFlg == "After") {

                        grdobj.fromminDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() - grdobj.hrmL_NoOfDays);
                        grdobj.frommaxDate = new Date(grdobj.fromminDate.getFullYear(), grdobj.fromminDate.getMonth(), grdobj.fromminDate.getDate() + grdobj.hrmL_MaxLeavesApplyPerMonth);
                        if (Date(grdobj.frommaxDate) >= Date(date)) {
                            grdobj.frommaxDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() - 1);
                        }
                        else {
                            grdobj.frommaxDate = new Date(grdobj.fromminDate.getFullYear(), grdobj.fromminDate.getMonth(), grdobj.fromminDate.getDate() + grdobj.hrmL_MaxLeavesApplyPerMonth - 1);

                        }

                    }


                    if (grdobj.hrelS_CBLeaves > 0) {
                        grdobj.hrelaP_TotalDays = 1;
                    }
                    grdobj.disable = true;
                });
                if (promise.get_leavestatus != null && promise.get_leavestatus.length > 0) {
                    $scope.get_leavestatus = promise.get_leavestatus;

                    angular.forEach(promise.get_leavestatus, function (item) {

                        //InTime = $scope.timeconvertion(item.HRELAPD_InTime);
                        //OUTTime = $scope.timeconvertion(item.HRELAPD_OutTime);
                        //   InTime = moment(item.HRELAPD_InTime, 'H:mm ').format();
                        //OUTTime = moment(item.HRELAPD_OutTime, 'H:mm ').format();
                        var recordexist = [];
                        recordexist = $filter('filter')($scope.compoffOdapplyedrecord, function (d) {
                            return d.HRELAP_Id === item.HRELAP_Id;
                        });

                        if (recordexist.length == 0) {
                            $scope.compoffOdapplyedrecord.push({
                                HRELAP_Id: item.HRELAP_Id, HRME_EmployeeFirstName: item.HRME_EmployeeFirstName,
                                HRML_LeaveName: item.HRML_LeaveName, HRELAP_FromDate: item.HRELAP_FromDate,
                                HRELAP_ToDate: item.HRELAP_ToDate, HRELAPD_InTime: item.HRELAPD_InTime, HRELAPD_OutTime: item.HRELAPD_OutTime,
                                HRELAP_ApplicationStatus: item.HRELAP_ApplicationStatus, HRELAP_LeaveReason: item.HRELAP_LeaveReason,
                                HRELAPA_Remarks: item.HRELAPA_Remarks,
                                HRELAP_TotalDays: item.HRELAP_TotalDays,
                                HRME_Id: item.HRME_Id,
                                hstep: 1, mstep: 1, ismeridian: false
                            });
                        }




                    });

                }

                $scope.employeeleaveDetails = $scope.leave_nameview;
                $scope.temp_arry_grid = promise.leave_name;
                if ($scope.count === 0) {
                    swal("Data not Found !!");
                    $scope.ckdept = false;
                }
                apiService.getURI("OnlineLeaveApplication/getonlineLeavestatus", id).then(function (promise) {
                    $scope.leave_grid = [];
                    $scope.rejectedleaves = [];
                    $scope.leave_name = promise.leave_name;
                    //i comments out bcz i have to show regected status
                    angular.forEach($scope.leave_name, function (dd) {
                        if (dd.hrelaP_ApplicationStatus === "Rejected") {
                            $scope.rejectedleaves.push(dd);
                        }
                    });
                    //if this line code u want to run so staus not showing
                    //$scope.rejectedleaves = promise.lrlist;


                    $scope.leave_nameCompoff = promise.leave_namelist;
                    $scope.trustAsHtml = function (html) {
                        return $sce.trustAsHtml(html);
                    };

                    $scope.temp_arry_grid = promise.leave_name;
                    if (promise.optionalleavedata.length > 0 && promise.optionalleavedata != null) {
                        $scope.optionalleavedata = promise.optionalleavedata;
                    }
                    if ($scope.count === 0) {
                        swal("Data not Found !!");
                        $scope.ckdept = false;
                    }
                });

                $scope.fromminDate = new Date(
                    $scope.setmindate.getFullYear(),
                    $scope.setmindate.getMonth() - 1,
                    $scope.setmindate.getDate());
                $scope.fromDate = $scope.setmindate;
                $scope.tominDate = new Date(
                    $scope.fromDate.getFullYear(),
                    $scope.fromDate.getMonth(),
                    $scope.fromDate.getDate());
                $scope.toDate = $scope.tominDate;
                $scope.minDate = new Date(
                    $scope.toDate.getFullYear(),
                    $scope.toDate.getMonth(),
                    $scope.toDate.getDate() + 1);
                $scope.maxDate = new Date(
                    $scope.toDate.getFullYear(),
                    $scope.toDate.getMonth(),
                    $scope.toDate.getDate() + 1);
                //$scope.hrelaP_ReportingDate = $scope.minDate;
            });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.todate = function (secondDate1, row) {

            var firstDate1 = $filter('date')(row.hrelaP_FromDate, "dd/MM/yy");
            secondDate1 = $filter('date')(row.hrelaP_ToDate, "dd/MM/yy");
            var date2 = new Date($scope.formatString(secondDate1));
            var date1 = new Date($scope.formatString(firstDate1));

            if (secondDate1 != null) {
                row.disable = false;
            }



            if (date1 > date2) {
                angular.forEach($scope.leave_nameview, function (obj) {
                    if (obj.hrelS_Id === row.hrelS_Id) {
                        obj.hrelaP_ToDate = null;
                        obj.hrelaP_TotalDays = 0;
                    }
                });
                swal('Invalid Selection !');
                return;
            }

            var timeDiff = Math.abs(date2.getTime() - date1.getTime());
            $scope.dayDifference = Math.ceil(timeDiff / (1000 * 3600 * 24));
            angular.forEach($scope.leave_nameview, function (obj) {
                if (obj.hrelS_Id === row.hrelS_Id) {
                    obj.hrelaP_TotalDays = $scope.dayDifference + 1;
                }
            });



            $scope.hrelaP_ReportingDate = new Date(
                row.hrelaP_ToDate.getFullYear(),
                row.hrelaP_ToDate.getMonth(),
                row.hrelaP_ToDate.getDate() + 1);


            if (row.hrelS_CBLeaves == 0.5) {
                row.cklop = true;
                row.disable = true;
                row.hrelaP_TotalDays = 0.5;
            }


            if (row.hrelS_CBLeaves == 1) {
                row.cklop = false;
                row.disable = false;
                row.hrelaP_TotalDays = 1;
            }

            //if (row.appliedCount >= row.hrelS_CBLeaves || row.appliedCount >= row.hrelaP_TotalDays || row.hrelaP_TotalDays > row.hrelS_CBLeaves) {
            //    row.hrelaP_ToDate = null;
            //    swal('Insufficient Leave Balance');
            //}
            var addleave = 0
            if ((row.appliedCount + row.hrelaP_TotalDays) >= row.hrelS_TotalLeaves) {
                addleave = 0.5;
            }

            if (row.hrelaP_TotalDays != null && row.hrelaP_TotalDays > 1) {
                row.disable = true;
            }
            else {
                row.disable = false;
            }


            if ((row.appliedCount + row.hrelaP_TotalDays) > row.hrelS_TotalLeaves + addleave) {
                row.hrelaP_ToDate = null;
                swal('Insufficient Leave Balance');
            }

            if (row.hrelaP_TotalDays < 4 && row.hrmL_LeaveCode == "PL") {
                swal('You Can Not Apply Privilage Leave Less Than 4 Days');
                row.hrelaP_ToDate = "";
            }
            var tempdays = row.hrelaP_TotalDays;
            if (row.selected == true) {
                if (row.cklop == true) {
                    row.hrelaP_TotalDays = tempdays - 0.5;
                }
            }
            if (($scope.HRELTD_TotDays + row.hrelaP_TotalDays) > row.hrmL_MaxLeavesApplyPerMonth) {
                swal('Monthly Limit Is Completed For This Leave');
                row.hrelaP_ToDate = "";
            }


        };

        $scope.formatString = function (format) {
            if (format !== undefined && format !== null && format !== "") {
                var day = parseInt(format.substring(0, 2));
                var month = parseInt(format.substring(3, 5));
                var year = parseInt(format.substring(6, 10));
                var date = new Date(year, month - 1, day);
                return date;
            }
        };

        $scope.lopadjestdateset = function (row) {
            if (row.hrmL_LeaveCode == "COMPOFF" && row.lopadjest == true) {
                row.hrelaP_ToDate = new Date(row.hrelaP_FromDate.getFullYear(), row.hrelaP_FromDate.getMonth(), row.hrelaP_FromDate.getDate());
                row.hrelaP_TotalDays = 0.5
                row.cklop = true;
                return;
            }
        }

        $scope.fromdate = function (d1, row) {
            $scope.From_Date = d1;
            var date = new Date(d1);
            angular.forEach($scope.leave_nameview, function (grdobj) {
                grdobj.hrelaP_ToDate = null;
                if (grdobj.selected == true) {

                    if (grdobj.hrmL_LeaveCode == "COMPOFF" && grdobj.lopadjest == true) {
                        row.hrelaP_ToDate = new Date(row.hrelaP_FromDate.getFullYear(), row.hrelaP_FromDate.getMonth(), row.hrelaP_FromDate.getDate());
                        $scope.hrelaP_ReportingDate = new Date(row.hrelaP_FromDate.getFullYear(), row.hrelaP_FromDate.getMonth(), row.hrelaP_FromDate.getDate());
                        row.hrelaP_TotalDays = 0.5
                        row.cklop = true;
                        return;
                    }


                    if (grdobj.hrelS_Id === row.hrelS_Id) {
                        grdobj.toDate = d1;
                    }

                    if (row.hrelS_CBLeaves == 1) {
                        row.cklop = false;
                        row.disable = false;
                        row.hrelaP_TotalDays = 1;
                    }

                    if (row.hrelS_CBLeaves == 0.5) {
                        row.cklop = true;
                        row.disable = true;
                        row.hrelaP_TotalDays = 0.5;
                    }

                    if (grdobj.hrelS_CBLeaves !== 0 && row.hrmL_LeaveCode == "OL") {
                        var dummy = 0;
                        angular.forEach($scope.optionalleavedata, function (data) {
                            var doob5 = data.FOMHWDD_FromDate == null ? "" : $filter('date')(data.FOMHWDD_FromDate, "yyyy-MM-dd");
                            var selectedfromdate = $scope.From_Date == null ? "" : $filter('date')($scope.From_Date, "yyyy-MM-dd");
                            if (selectedfromdate == doob5) {
                                dummy = 1;
                            }
                        });

                        if (dummy == 0 && d1 > null) {
                            swal("Selected Date Is Not Have Optional Holiday");
                            $state.reload();
                        }
                        else {
                            if (grdobj.hrelS_CBLeaves !== 0 && row.hrmL_WhenToApplyFlg == "After") {
                                grdobj.hrelaP_TotalDays = 1;
                                $scope.fromminus = new Date();
                                var doob = $scope.fromminus == null ? "" : $filter('date')($scope.fromminus, "yyyy-MM-dd");
                                var doobyr = doob.substring(0, 4);
                                var doobmnth = doob.substring(5, 7);
                                var doobdays = doob.substring(8, 10);
                                var commondate = new Date(doobyr, doobmnth, doobdays - (grdobj.hrelS_CBLeaves));
                                if (grdobj.hrelS_CBLeaves >= row.hrmL_MaxLeavesApplyPerMonth) {
                                    grdobj.hrelaP_TotalDays = 1;
                                    grdobj.tomaxDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() + (grdobj.hrmL_MaxLeavesApplyPerMonth - 1));
                                    grdobj.hrelaP_ToDate = null;
                                }
                                else {
                                    grdobj.hrelaP_TotalDays = 1;
                                    grdobj.tomaxDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() + (grdobj.hrelS_CBLeaves - 1));
                                    grdobj.hrelaP_ToDate = null;
                                }
                                grdobj.hrelaP_ToDate = null;

                            }
                            else if (grdobj.hrelS_CBLeaves !== 0) {
                                if (grdobj.hrelS_CBLeaves >= row.hrmL_MaxLeavesApplyPerMonth) {
                                    grdobj.hrelaP_TotalDays = 1;
                                    grdobj.tomaxDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() + (row.hrmL_MaxLeavesApplyPerMonth - 1));
                                    $scope.fromminus = new Date();
                                    var doob = $scope.fromminus == null ? "" : $filter('date')($scope.fromminus, "yyyy-MM-dd");
                                    var doobyr = doob.substring(0, 4);
                                    var doobmnth = doob.substring(5, 7);
                                    var doobdays = doob.substring(8, 10);
                                    var commondate = new Date(doobyr, doobmnth, doobdays - (grdobj.hrelS_CBLeaves));
                                    //if ($scope.From_Date > commondate) {
                                    //    grdobj.tomaxDate = new Date();
                                    //}
                                    //else {
                                    //    grdobj.tomaxDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() + (grdobj.hrelS_CBLeaves - 1));
                                    //}
                                    if (grdobj.hrelS_CBLeaves >= row.hrmL_MaxLeavesApplyPerMonth) {
                                        if (grdobj.hrelS_CBLeaves == 0.5) {
                                            grdobj.hrelS_CBLeaves = 1
                                        }
                                        grdobj.hrelaP_TotalDays = 1;
                                        grdobj.tomaxDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() + (grdobj.hrmL_MaxLeavesApplyPerMonth - 1));
                                        grdobj.hrelaP_ToDate = null;
                                    }
                                    else {
                                        if (grdobj.hrelS_CBLeaves == 0.5) {
                                            grdobj.hrelS_CBLeaves = 1
                                            grdobj.hrelaP_TotalDays = 0.5;
                                            grdobj.tomaxDate = new Date(date.getFullYear(), date.getMonth(), date.getDate());
                                            grdobj.hrelaP_ToDate = null;
                                        }
                                        else {
                                            grdobj.hrelaP_TotalDays = 1;
                                            grdobj.tomaxDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() + (grdobj.hrelS_CBLeaves - 1));
                                            grdobj.hrelaP_ToDate = null;
                                        }

                                    }
                                    grdobj.hrelaP_ToDate = null;
                                }
                                else {
                                    grdobj.hrelaP_TotalDays = 1;
                                    grdobj.tomaxDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() + (grdobj.hrelS_CBLeaves - 1));
                                    grdobj.hrelaP_ToDate = null;
                                }
                            }

                        }

                    }
                    else {

                        if (grdobj.hrelS_CBLeaves !== 0 && row.hrmL_WhenToApplyFlg == "After") {
                            grdobj.hrelaP_TotalDays = 1;
                            $scope.fromminus = new Date();
                            var doob = $scope.fromminus == null ? "" : $filter('date')($scope.fromminus, "yyyy-MM-dd");
                            var doobyr = doob.substring(0, 4);
                            var doobmnth = doob.substring(5, 7);
                            var doobdays = doob.substring(8, 10);
                            var commondate = new Date(doobyr, doobmnth, doobdays - (grdobj.hrelS_CBLeaves));

                            if (row.hrelS_CBLeaves > row.hrmL_MaxLeavesApplyPerMonth) {
                                row.hrelaP_TotalDays = 1;
                                row.tomaxDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() + (grdobj.hrmL_MaxLeavesApplyPerMonth));
                                row.hrelaP_ToDate = null;
                            }
                            else {
                                if (row.hrelS_CBLeaves <= 1) {
                                    if (row.hrelS_CBLeaves == 0.5) {
                                        row.hrelaP_TotalDays = 0.5;
                                    }

                                    row.tomaxDate = new Date(date.getFullYear(), date.getMonth(), date.getDate());
                                    //row.hrelaP_ToDate = new Date(row.hrelaP_FromDate.getFullYear(), row.hrelaP_FromDate.getMonth(), row.hrelaP_FromDate.getDate());

                                    //$scope.hrelaP_ReportingDate = new Date(
                                    //    row.hrelaP_ToDate.getFullYear(),
                                    //    row.hrelaP_ToDate.getMonth(),
                                    //    row.hrelaP_ToDate.getDate() + 1);

                                }
                                else {
                                    row.hrelaP_TotalDays = 1;
                                    row.tomaxDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() + (grdobj.hrelS_CBLeaves - 1));
                                    row.hrelaP_ToDate = null;
                                }

                            }
                            var currentdate = new Date();
                            var filterdate = currentdate == null ? "" : $filter('date')(currentdate, "yyyy-MM-dd");
                            var filterdate1 = row.tomaxDate == null ? "" : $filter('date')(row.tomaxDate, "yyyy-MM-dd");


                            if (filterdate1 >= filterdate) {
                                row.tomaxDate = new Date(currentdate.getFullYear(), currentdate.getMonth(), currentdate.getDate() - 1);
                            }




                        }
                        else if (grdobj.hrelS_CBLeaves !== 0) {

                            if (grdobj.hrelS_CBLeaves >= row.hrmL_MaxLeavesApplyPerMonth) {
                                grdobj.hrelaP_TotalDays = 1;
                                grdobj.tomaxDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() + (grdobj.hrmL_MaxLeavesApplyPerMonth - 1));
                                grdobj.hrelaP_ToDate = null;
                            }
                            else {
                                if (row.hrelS_CBLeaves <= 1) {
                                    if (row.hrelS_CBLeaves == 0.5) {
                                        row.hrelaP_TotalDays = 0.5;
                                    }

                                    row.tomaxDate = new Date(date.getFullYear(), date.getMonth(), date.getDate());
                                    //row.hrelaP_ToDate = new Date(row.hrelaP_FromDate.getFullYear(), row.hrelaP_FromDate.getMonth(), row.hrelaP_FromDate.getDate());

                                    //$scope.hrelaP_ReportingDate = new Date(
                                    //    row.hrelaP_ToDate.getFullYear(),
                                    //    row.hrelaP_ToDate.getMonth(),
                                    //    row.hrelaP_ToDate.getDate() + 1);

                                }
                                else {
                                    grdobj.hrelaP_TotalDays = 1;
                                    grdobj.tomaxDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() + (grdobj.hrelS_CBLeaves - 1));
                                    grdobj.hrelaP_ToDate = null;

                                }

                            }


                        }
                    }

                }
            });
            $scope.FromDate = new Date(d1).toDateString();
            var data = {
                "HRELAP_FromDate": FromDate,
                "HRML_LeaveName": row.hrmL_LeaveName,
                "HRML_Id": row.hrmL_Id
            }

            apiService.create("OnlineLeaveApplication/CheckAppliedDate", data).then(function (promise) {
                $scope.HRELTD_TotDays = promise.hreltD_TotDays;
                if (promise.getemployeeleavedetails != null && promise.getemployeeleavedetails.length > 0) {

                }
                else {
                    swal("Leave Is Not Credited For This Leave Year");
                    row.hrelaP_FromDate = null;
                }

                //if (promise.message === "Applied") {
                //    swal("For This Date Already Leave Is Applied");
                //    row.hrelaP_FromDate = null;
                //}
            });

            //$scope.hrelaP_ReportingDate = $scope.minDate;
        };



        //sunday filter 25/10/2024
        $scope.dateFilter = function (date) {
            return date && date.getDay() !== 0;
        };

        $scope.submitted = false;
        $scope.ApplayLeave = function () {
                if ($scope.myForm.$valid) {
                    var temp_array = [];
                    var rows = [];
                    angular.forEach($scope.leave_nameview, function (row) {
                        if (row.selected == true) {
                            temp_array.push({
                                HRELAP_FromDate: new Date(row.hrelaP_FromDate).toDateString(),
                                HRELAP_ToDate: new Date(row.hrelaP_ToDate).toDateString(),
                                HRELAP_TotalDays: row.hrelaP_TotalDays,
                                LOPAdjest: row.lopadjest
                            });
                            rows.push({
                                HRML_LeaveName: row.hrmL_LeaveName,
                                HRML_Id: row.hrmL_Id,
                                HRML_LeaveCode: row.hrmL_LeaveCode
                            });
                        }
                    });

                    //var rows = $scope.gridApi.selection.getSelectedRows();

                    var today = new Date().toDateString();
                    if (temp_array.length !== 0) {
                        if (rows[0].HRML_LeaveCode == "PL") {
                            if ($scope.proxytaskList != null && $scope.proxytaskList.length > 0) {

                            }
                            else {
                                swal('Kindly transfer your task to proxy, to apply Privilege Leave...');
                                return;
                            }
                        }

                        if (rows[0].HRML_LeaveCode != "PL") {
                            $scope.leavelimitconut = temp_array[0].HRELAP_TotalDays //+ $scope.Appliedleavecon
                            if ($scope.leavelimitconut > 2) {
                                swal('Your monthly leave limit has been reached. You can only apply for 2 leaves per month.');
                                return;
                            } 
                        }

                        if ($scope.ProxyLeave == 1) {
                            swal('Your proxy has applied for leave on these dates. Please select different dates for your leave');
                            return;
                        }

                        if ($scope.Deptleave == 1 && rows[0].HRML_LeaveCode == "OL") {
                            swal("The number of employees on leave for this department has reached the 40% limit. You can't apply for optional leave at this time.");
                            return;
                        }

                        if (rows[0].HRML_LeaveCode == "EL") {
                            $scope.obj.HRELAP_LeaveReason = " <span style=" + "color:lightseagreen;font-weight:600;font-size:large" + ">" + $scope.obj.catagoryofleave + "</span><br\>" + $scope.obj.HRELAP_LeaveReason;
                        }
                        else {
                            $scope.obj.HRELAP_LeaveReason = $scope.obj.HRELAP_LeaveReason;
                        }
                        if (rows[0].HRML_LeaveCode == "SL") {
                            if ($scope.leavelimitconut == 1) {
                                var data = {
                                    "HRELAP_ApplicationDate": today,
                                    "HRELAP_LeaveReason": $scope.obj.HRELAP_LeaveReason,
                                    "HRELAP_ContactNoOnLeave": Number($scope.contact),
                                    "HRELAP_ReportingDate": new Date($scope.hrelaP_ReportingDate).toDateString(),
                                    "HRELT_SupportingDocument": $scope.hrelT_SupportingDocument,
                                    "HRME_Id": $scope.obj.HRME_Id.HRME_Id,
                                    temp_table_data: rows,
                                    frmToDates: temp_array
                                }
                            }
                            else {
                                if ($scope.hrelT_SupportingDocument != "" && $scope.hrelT_SupportingDocument != undefined && $scope.hrelT_SupportingDocument != null) {
                                    var data = {
                                        "HRELAP_ApplicationDate": today,
                                        "HRELAP_LeaveReason": $scope.obj.HRELAP_LeaveReason,
                                        "HRELAP_ContactNoOnLeave": Number($scope.contact),
                                        "HRELAP_ReportingDate": new Date($scope.hrelaP_ReportingDate).toDateString(),
                                        "HRELT_SupportingDocument": $scope.hrelT_SupportingDocument,
                                        "HRME_Id": $scope.obj.HRME_Id.HRME_Id,
                                        temp_table_data: rows,
                                        frmToDates: temp_array
                                    }
                                }
                                else {
                                    swal('Upload Document !');
                                }
                            }
                        }
                        else {
                            var data = {
                                "HRELAP_ApplicationDate": today,
                                "HRELAP_LeaveReason": $scope.obj.HRELAP_LeaveReason,
                                "HRELAP_ContactNoOnLeave": Number($scope.contact),
                                "HRELAP_ReportingDate": new Date($scope.hrelaP_ReportingDate).toDateString(),
                                "HRELT_SupportingDocument": $scope.hrelT_SupportingDocument,
                                "HRME_Id": $scope.obj.HRME_Id.HRME_Id,
                                temp_table_data: rows,
                                frmToDates: temp_array
                            }
                        }
                    }
                    else {
                        swal('Invalid Selection !');
                    }

                    apiService.create("OnlineLeaveApplication/save", data).
                        then(function (promise) {


                            if (promise.returnmsg === 'Nomapping') {
                                swal("Please Create Leave Numbering Using Transaction Numbering Page");
                                return;
                            }
                            else if (promise.returnval === true) {
                                swal('Leave Applied successfully');
                                $state.reload();
                            }
                            else if (promise.returnduplicatestatus === 'Duplicate') {
                                swal('Leave Already Applied !');
                                $state.reload();
                            }
                            else if (promise.message === "You Crossed Your Leave Limits") {
                                swal('You Crossed Your Leave Limits');
                                $state.reload();
                            }
                            else if (promise.message === "Leave Already applied on these dates") {
                                swal('Leave Already applied on these dates');
                                $state.reload();
                            }
                            else if (promise.returnval === false) {
                                swal('Leave not Applied. Try Again !');
                            }
                        });
                }
                else {
                    $scope.submitted = true;
                }

        };


        $scope.LeaveApplyPolicy11 = function () {
            $('#popup11').modal('hide');

        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };



        $scope.SelectedFileForUploadzdBOSBOE = [];
        $scope.selectFileforUploadzdBOSBOE = function (input) {
            $scope.SelectedFileForUploadzdBOSBOE = input.files;
            //$('#blah').removeAttr('src');
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                $scope.extention = nameArray[nameArray.length - 1];
                if (($scope.extention === "JPEG" || $scope.extention === "jpg" || $scope.extention === "JPG" || $scope.extention === "png" || $scope.extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blah')
                            .attr('src', e.target.result);

                        $('#documentid') //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentBOSBOE();
                }
                else if ($scope.extention !== "JPEG" && $scope.extention !== "png" && $scope.extention !== "jpg" && $scope.extention !== "JPG" && $scope.extention !== "pdf") {
                    $('#documentid').removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    $('#documentid').removeAttr('src');
                    swal("Document size should be less than 2MB");
                    return;
                }
            }
        };

        function UploadEmployeeDocumentBOSBOE() {

            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadzdBOSBOE.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdBOSBOE[i]);
            }

            formData.append("Id", 0);

            var sample1 = Object.fromEntries(formData.entries());

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadnaacdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {
                        $scope.hrelT_SupportingDocument = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#documentid').removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.showmodaldetailsBOSBOE = function () {
            //$('#preview').removeAttr('src');
            var filename = $scope.hrelT_SupportingDocument.toString();
            var nameArray = filename.split('.');
            $scope.extention = nameArray[nameArray.length - 1];
            if ($scope.extention === "jpg" || $scope.extention === "jpeg" || $scope.extention === "JPG" || $scope.extention === "JPEG" || $scope.extention === "png"
                || $scope.extention === "PNG") {
                $('#preview').attr('src', $scope.hrelT_SupportingDocument);
                //$('#myModal').modal('show');
            }
            else if ($scope.extention === "doc" || $scope.extention === "docx" || $scope.extention === "xls" || $scope.extention === "xlsx") {
                //$('#preview').removeAttr('src');
                $('#preview').attr('src', $scope.hrelT_SupportingDocument);
            }
            else if ($scope.extention === "pdf") {
                var imagedownload = $scope.hrelT_SupportingDocument;
                $scope.content = "";
                var fileURL = "";
                var file = "";
                $http.get(imagedownload, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        $scope.content = $sce.trustAsResourceUrl(fileURL);
                        $('#showpdf').modal('show');
                    });
            }
        };

        $scope.validateTomintime = function (timedata) {
            $scope.hrelapD_OutTime = "";
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;
            $scope.min.setHours(hh);
            $scope.min.setMinutes(mm);
            $scope.minlnc = timedata;
            $scope.minlnc.setHours(hh);
            $scope.minlnc.setMinutes(mm);
            //$scope.hrelapD_InTime = "";
        };

        $scope.validatemax = function (maxdata) {

            var dsttimee = $scope.hrelapD_InTime;
            $scope.sresult = $filter('date')(dsttimee, 'HH:mm:ss a');
            $scope.eresult = $filter('date')(maxdata, 'HH:mm:ss a');
            var startTime = moment($scope.sresult, "HH:mm:ss a");
            var endTime = moment($scope.eresult, "HH:mm:ss a");
            var duration = moment.duration(endTime.diff(startTime));
            var hours = parseInt(duration.asHours());
            var minutes = parseInt(duration.asMinutes()) - hours * 60;
            var finlrst = hours + ":" + minutes;

            $scope.tmin = new Date();
            $scope.tmin.setHours(hours);
            $scope.tmin.setMinutes(minutes);
            $scope.tmax = new Date();
            $scope.tmax.setHours(hours);
            $scope.tmax.setMinutes(minutes);

            $scope.ttst = new Date();
            $scope.ttst.setHours(hours);
            $scope.ttst.setMinutes(minutes);
            $scope.FOMST_FDWHrMin = $scope.ttst;
            $scope.htmax = new Date();
            $scope.htmax.setHours(hours);
            $scope.htmax.setMinutes(minutes);

            if (maxdata >= new Date($scope.hrelapD_InTime)) {
                $scope.totimemax = maxdata;
                var hh = $scope.totimemax.getHours();
                var mm = $scope.totimemax.getMinutes();
                $scope.max = maxdata;
                $scope.max.setMinutes(hh);
                $scope.max.setMinutes(mm);
            }
            else {
                $scope.hrelapD_OutTime = "";
            }
        };

        //$scope.reqodcompoff = function () {
        //    $scope.HRELAP_TotalDays = 0;
        //    var InTime = $scope.hrelapD_InTime;
        //    var OutTime = $scope.hrelapD_OutTime;

        //    var res = Math.abs(InTime - OutTime) / 1000;
        //    var hours = Math.abs(res / 3600) % 24;

        //    if (hours >= 3 && hours < 5) {
        //        $scope.HRELAP_TotalDays = 0.5;
        //        $scope.CompOffODRequest($scope.HRELAP_TotalDays);
        //    } else if (hours >= 4) {
        //        $scope.HRELAP_TotalDays = 1;
        //        $scope.CompOffODRequest($scope.HRELAP_TotalDays);
        //    } else {
        //        swal("Time is less than 5 Hours")
        //        $state.reload();
        //    }
        //}


        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };

        $scope.GetLeave = function (leave) {
            if (leave.hrmL_LeaveCode == "COMPOFF") {
                leave.selected = false;
                swal("Comp off will be auto adjusted to lop");
                return;
            }
            //else if (leave.hrmL_LeaveCode == "PL" || leave.hrmL_LeaveCode == "CL" && $scope.deviationpercentage > 120) {
            //  leave.selected = false;
            //  swal("You cannot apply for " + leave.hrmL_LeaveName + " since your deviation is greater than 20 %.");
            // return;
            //  }

            const secondDate = new Date(leave.HRME_doc);

            if (leave.HRML_ProbationaryNAFlg == true && $scope.setmindate <= secondDate) {
                leave.selected = false;
                swal("As you are in the probationary period, you are not eligible to apply for " + leave.hrmL_LeaveName);
                return;
            }

            if (leave.selected == true) {

                if (leave.hrelS_CBLeaves == 0) {
                    swal("No Leave Balance Found!");
                    leave.selected == false;
                }

                angular.forEach($scope.leave_nameview, function (itm) {
                    if (leave.hrmL_Id == itm.hrmL_Id) {

                        itm.selected = true;
                        //itm.disable = false;
                    }
                    else {
                        itm.selected = false;
                        //itm.disable = true;
                    }
                });

                var data = {
                    "HRML_Id": leave.hrmL_Id
                };

                apiService.create("OnlineLeaveApplication/CheckApprovalAuthorizationMapped", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.authorizationmessage === "NotMapped") {
                            swal("You Can Not Apply This Leave Because Authorized Person Is Not Mapped For Approval, So Contact Administrator / HR");
                            leave.selected = false;
                        }


                        if (leave.hrmL_LeaveCode == "PL" && leave.hrelaP_ToDate != 0) {
                            $scope.Staff_List = promise.staff_List
                            $scope.eanableproxyflag = true;
                        }
                        else {
                            $scope.eanableproxyflag = false;
                            $scope.obj.HRME_Id = 0;
                        }

                    }
                });
            }
            else {
                if (leave.selected) {
                    //   leave.disable = false;
                }
                else {
                    // leave.disable = true;
                }
            }


            $scope.leaveSelectedAll = $scope.employeeleaveDetails.every(function (itm) {
                return itm.selected;
            });

            //if (leave.selected) {
            //    leave.disable = false;
            //}
            //else {
            //    leave.disable = true;
            //}
        };

        // snippet to view document 
        $scope.viewdocument = function (obj) {

            if (obj != null) {
                var img = obj.HRELAP_SupportingDocument;
                var imagarr = img.split('.');
                var extention = imagarr[imagarr.length - 1];
                obj.filetype = extention;
                if (extention === 'doc' || extention === 'docx' || extention === 'ppt' || extention === 'pptx' || extention === 'xlsx' || extention === 'xls') {
                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + obj.HRELAP_SupportingDocument;
                }
                else if (extention === "jpg" || extention === "jpeg" || extention === "JPEG" || extention === "png") {
                    $('#preview').removeAttr('src');
                    $('#preview').attr('src', obj.HRELAP_SupportingDocument);
                    $('#myModal').modal();
                    $('#myModal').modal({ keyboard: false });
                    $('#myModal').modal('show');
                }
                else if (extention === "pdf") {
                    var imagedownload = obj.HRELAP_SupportingDocument;
                    $scope.content = "";
                    var fileURL = "";
                    var file = "";
                    $http.get(imagedownload, { responseType: 'arraybuffer' })
                        .success(function (response) {
                            file = new Blob([(response)], { type: 'application/pdf' });
                            fileURL = URL.createObjectURL(file);
                            $scope.content = $sce.trustAsResourceUrl(fileURL);
                            $('#showpdf').modal('show');
                        });
                }
            }
        };

        $scope.zoomin = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth >= 750) {
                swal("Maximum zoom-in level reached.");
            } else {
                myImg.style.width = (currWidth + 50) + "px";
            }
        };

        $scope.zoomout = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth <= 400) {
                swal("Maximum zoom-out level reached.");
            } else {
                myImg.style.width = (currWidth - 50) + "px";
            }
        };

        //$scope.viewcomment = function (obj) {
        //    $scope.commentlist = [];
        //    $scope.Applied_Fromdate = "";
        //    $scope.Applied_Todate = "";
        //    $scope.Supporting_Document = "";
        //    var data = {
        //        "HRELAP_Id": obj.hrelaP_Id
        //    };
        //    apiService.create("OnlineLeaveApplication/viewcomment", data).then(function (promise) {
        //        if (promise.commentlist != null && promise.commentlist != undefined) {
        //            $scope.commentlist = promise.commentlist;
        //            $scope.Applied_Fromdate = $scope.commentlist[0].hrelaP_FromDate;
        //            $scope.Applied_Todate = $scope.commentlist[0].hrelaP_ToDate;
        //            $scope.Supporting_Document = $scope.commentlist[0].hrelaP_SupportingDocument;
        //        }

        //        //if (obj.hrmE_IdMy == $scope.commentlist[0].hrmE_Id) {

        //        //    $scope.disfoname = true;
        //        //}
        //        //else {
        //        //    $scope.disfoname = false;
        //        //}


        //    });
        //};

        $scope.viewcomment = function (obj) {

            var data = {
                "HRELAP_Id": obj.hrelaP_Id
            };
            apiService.create("OnlineLeaveApplication/viewcomment", data).then(function (promise) {
                if (promise.commentlist != null && promise.commentlist != undefined) {
                    $scope.commentlist = promise.commentlist;
                    $scope.Applied_Fromdate = $scope.commentlist[0].HRELAP_FromDate;
                    $scope.Applied_Todate = $scope.commentlist[0].HRELAP_ToDate;
                    $scope.Supporting_Document = $scope.commentlist[0].HRELAP_SupportingDocument;
                    $scope.rejectedDay = promise.commentlist[0].HRELAPA_LeaveStatus;
                }


            });
        };

        $scope.approverName = function (obj) {

            var data = {
                "HRME_Id": $scope.hrmE_Id
            };
            apiService.create("OnlineLeaveApplication/Leaveapprover", data).then(function (promise) {
                if (promise.approverList != null && promise.approverList != undefined) {
                    $scope.approverList = promise.approverList;

                    $scope.employeeid = [];

                    angular.forEach($scope.approverList, function (dev) {
                        if ($scope.employeeid.length === 0) {
                            $scope.employeeid.push({
                                HRLA_Id: dev.HRLA_Id,
                                HRML_LeaveName: dev.HRML_LeaveName,
                            });
                        } else if ($scope.employeeid.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.employeeid, function (emp) {
                                if (emp.HRLA_Id === dev.HRLA_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.employeeid.push({
                                    HRLA_Id: dev.HRLA_Id,
                                    HRML_LeaveName: dev.HRML_LeaveName,
                                });
                            }
                        }
                    });

                    angular.forEach($scope.employeeid, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.approverList, function (dd) {
                            if (dd.HRLA_Id === ddd.HRLA_Id) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.plannerdetails = $scope.templist;
                    });

                }
            });
        };




        $scope.proxydetials = function (dd) {
            $scope.ProxyName = dd.Proxy;
            $scope.HRMDES_DesignationName = dd.HRMDES_DesignationName;
            $scope.HRME_EmployeeFirstName_Temp = dd.hrmE_EmployeeFirstName;
            $scope.HRELAP_LeaveReason = dd.hrelaP_LeaveReason;
            var data = {
                "HRELAP_Id": dd.hrelaP_Id,
                "HRELAP_FromDate": dd.hrelaP_FromDate,
                "HRELAP_ToDate": dd.hrelaP_ToDate,
                "HRME_Id": dd.hrmE_Id

            };
            apiService.create("OnlineLeaveApplication/proxytaskdetails", data).then(function (promise) {
                $scope.proxytaskListview = promise.proxytaskListview;
                if (promise.proxytaskListview != null && promise.proxytaskListview != undefined && promise.proxytaskListview.length > 0) {
                    $scope.proxytaskListview = promise.proxytaskListview;
                }
                else {
                    //swal("No Leave Balance Found!");
                }
            })
        };

        $scope.proxyempdetials = function () {
            $scope.temp_array = [];
            angular.forEach($scope.leave_nameview, function (row) {
                if (row.selected == true) {
                    $scope.temp_array.push({
                        HRELAP_FromDate: new Date(row.hrelaP_FromDate).toDateString(),
                        HRELAP_ToDate: new Date(row.hrelaP_ToDate).toDateString()
                    });
                }
            });
            var data = {
                "HRMEIdproxy": $scope.obj.HRME_Id.HRME_Id,
                "HRELAP_FromDate": $scope.temp_array[0].HRELAP_FromDate,
                "HRELAP_ToDate": $scope.temp_array[0].HRELAP_ToDate,
                "HRME_Id": $scope.hrmE_Id

            };
            apiService.create("OnlineLeaveApplication/proxytaskdetails", data).then(function (promise) {
                $scope.proxytaskList = promise.proxytaskList;
                if (promise.proxytaskList != null && promise.proxytaskList != undefined && promise.proxytaskList != 0) {
                    $scope.proxytaskList = promise.proxytaskList;
                }
                //else {
                //    swal('Kindly transfer your task to prox on leave');
                //}
            })
        }
        $scope.getotherdetails = function (itm, Type) {
            if ($scope.myForm.$valid) {
                $scope.report_details = [];
                $scope.HRME_EmployeeFirstName = itm.HRME_EmployeeFirstName;
                var data = {
                    "HRME_Id": itm.HRME_Id,
                    "HRMED_EntryDate": itm.HRELAP_FromDate,
                    "HRML_LeaveType": Type,
                };

                apiService.create("LeaveApproval/ViewOtherDetails", data).
                    then(function (promise) {
                        if (promise.get_leavestatus != null && promise.get_leavestatus.length > 0) {
                            $scope.report_details = promise.get_leavestatus;
                            if (Type == "DrDetails") {

                                $scope.totaldreffort = 0;
                                $scope.report_details = $filter('filter')(promise.get_leavestatus, function (d) {
                                    $scope.totaldreffort = Number($scope.totaldreffort) + Number(d.ISMDRPT_TimeTakenInHrs);
                                    return d;
                                });
                                $('#drsentlist').modal('show');
                            }
                            else {
                                $('#leaveapprovallist').modal('show');
                            }
                        }
                        else {
                            if (Type == "DrDetails") {
                                swal("DR Not Generated For This Date!!");
                            }
                            else {
                                swal("No Leave Approvar Found!!");
                            }

                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.submitted2 = false;
        $scope.reqodcompoff = function () {
            //var today = new Date().toDateString();
            //var HRELAP_FromDate = new Date($scope.fromDate).toDateString();
            var type = 'COMPOFF';
            $scope.rowsSelected = [];
            angular.forEach($scope.compoffOdapplyedrecord, function (record) {
                if (record.selectedd2 === true) {
                    $scope.rowsSelected.push({ HRELAP_Id: record.HRELAP_Id, HRME_Id: record.HRME_Id });//HRELAP_Remarks: $scope.obj.HRELAP_Remarks
                }
            });
            $scope.rowsSelected1dr = [];
            angular.forEach($scope.report_details, function (record12) {
                if (record12.selectedd22 === true) {
                    $scope.rowsSelected1dr.push({ ISMDRPT_Id: record12.ISMDRPT_Id });
                }
            });

            var data = {
                selectedcompoff: $scope.rowsSelected,
                selecteddr: $scope.rowsSelected1dr,
                "HRML_LeaveType": type
            };

            apiService.create("OnlineLeaveApplication/requestleave", data).
                then(function (promise) {
                    if (promise.returnval === true) {
                        swal('Requested successfully..');
                        //$('#myModal').modal('hide');
                        $state.reload();
                    }
                    else if (promise.returnmsg === 'Duplicate') {
                        swal('Records Already Exist !');
                    }
                    else if (promise.returnval === false) {
                        swal('Request Failed !! Kindly contact Administration.');
                    }
                });

        };

        $scope.ApplayLeaveReconsideration = function () {
            $("#popup11134").modal('show');

        };

        $scope.HalfDays = 1;

        $scope.isHalfDayAllowed = function () {
            if ($scope.cklop1 === true) {
                $scope.HalfDays = 0.5;
            }
            else {
                $scope.cklop1 === false
                $scope.HalfDays = 1;
                if ($scope.RcFromDate && $scope.RcToDate) {
                    const fromDate = new Date($scope.RcFromDate);
                    const toDate = new Date($scope.RcToDate);
                    fromDate.setHours(0, 0, 0, 0);
                    toDate.setHours(0, 0, 0, 0);
                    const diffTime = toDate - fromDate;
                    const diffDays = Math.ceil(diffTime / (1000 * 3600 * 24)) + 1;
                    $scope.HalfDays = diffDays
                    if ($scope.HalfDays > 1) {
                        $scope.cklop1 = false
                    }
                    return diffDays == 1;
                }
            }

            return true;
        };


        $scope.minToDate = null;
        $scope.updateMinToDate = function () {
            if ($scope.RcFromDate) {
                $scope.minToDate = new Date($scope.RcFromDate);
                $scope.RcToDate = null;
            } else {
                $scope.minToDate = null;
                $scope.RcToDate = null;
            }
        };


        $scope.submitted1 = false;
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.releaveapply = [];
        $scope.ReconsiderationSave = function () {
            if ($scope.selectedList.length > 0) {
                //if ($scope.myForm1.$valid) {
                if ($scope.countrecon > 2) {
                    swal('Your yearly reconsideration leave limit has been reached. You can only apply for 2 leaves per year.');
                    return;
                }

                angular.forEach($scope.selectedList, function (row) {
                    if (row.selected == true) {
                        $scope.releaveapply.push({
                            HRELAP_Id: row.hrelaP_Id,
                            HRME_Id: row.hrmE_Id,
                            HRERLAP_FromDate: row.hrelaP_FromDate,
                            HRERLAP_ToDate: row.hrelaP_ToDate,
                            HRELAP_TotalDays: row.hreltD_TotDays,
                            HRELAP_LeaveReason: row.hrelaP_LeaveReason,
                            HRML_Id: row.hrmL_Id,
                        });
                    }
                })



                var data = {
                    "multi_leave_name": $scope.releaveapply,
                    "returnmsg": "RCLeave"
                    //"HRERLAP_TotalDays": $scope.HalfDays,
                    //"HRERLAP_FromDate": new Date($scope.RcFromDate).toDateString(),
                    //"HRERLAP_ToDate": new Date($scope.RcToDate).toDateString(),
                    //"HRERLAP_LeaveReason": $scope.RcRemark,
                    //"HRME_Id": $scope.hrmE_Id
                }
                apiService.create("OnlineLeaveApplication/save", data).
                    then(function (promise) {
                        if (promise.retrunMsg === 'Success') {
                            swal('Leave Applied successfully');
                            //$state.reload();
                            $("#popup11134").modal('hide');
                        }
                        else if (promise.retrunMsg === 'NotSuccess') {
                            swal('Failed to Save/Update');
                            $state.reload();
                        }
                    });
                //}
                //else {
                //    $scope.submitted1 = true;
                //}
            }
            else {
                swal('Select atleast one check box!');
            }

        };




        $scope.selectedList = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all2;
            angular.forEach($scope.rejectedleaves, function (itm) {
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

            $scope.all2 = $scope.rejectedleaves.every(function (itm) { return itm.selected; });
            if ($scope.selectedList.indexOf(SelectedRecord) === -1) {
                $scope.selectedList.push(SelectedRecord);
            }
            else {
                $scope.selectedList.splice($scope.selectedList.indexOf(SelectedRecord), 1);
            }
        }

        $scope.cancel = function () {
            $state.reload();
        };


        $scope.leaveCondition = function (temp_array) {

            var data = {
                "HRELAP_FromDate": new Date(temp_array.hrelaP_FromDate).toDateString(),
                "HRELAP_ToDate": new Date(temp_array.hrelaP_ToDate).toDateString()
            };
            apiService.create("OnlineLeaveApplication/leveConditions", data).then(function (promise) {
                if (promise.commentlist != null && promise.commentlist != undefined) {
                    $scope.Deptleave = promise.commentlist[0].Deptleave;
                    $scope.ProxyLeave = promise.commentlist[0].ProxyLeave;
                }


            });
        };




    }
})();
