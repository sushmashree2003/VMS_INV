(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_PlannerReport', ISM_PlannerReport);
    ISM_PlannerReport.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter'];
    function ISM_PlannerReport($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter) {


        var paginationformasters;
        $scope.multidepartment = false;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.obj = {};
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();

        $scope.loaddata = function () {
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("ISM_PlannerReport/getdetails", pageid).then(function (promise) {

                if (promise.ivrmrT_Role == 'admin') {
                    $scope.depdes = true;
                    $scope.departmentdropdown = promise.departmentList;
                    $scope.designationdropdown = promise.designation;

                    $scope.get_userEmplist = promise.get_userEmplist;
                    $scope.empdepartment = promise.empdepartment;
                    $scope.empname = promise.empname;
                }
                else {
                    $scope.depdes = false;
                    $scope.get_userEmplist = promise.get_userEmplist;
                    $scope.empdepartment = promise.empdepartment;
                    $scope.empname = promise.empname;
                }
                //$scope.get_userEmplist = promise.get_userEmplist;
                //$scope.empdepartment = promise.empdepartment;
                //$scope.empname = promise.empname;
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




        $scope.togglecheckboxAll = function (checkallemp) {
            var toggleStatus = checkallemp;
            angular.forEach($scope.employeeid, function (slc) {
                slc.checked = toggleStatus;
            });
        };
        $scope.optionToggled = function () {
            $scope.checkallemp = $scope.employeeid.every(function (options) {
                return options.checked;
            });
        };
        //*************************** Department REPORT *********************//        
        $scope.all_checkdept = function (departmentselectedAll) {
            var toggleStatus = departmentselectedAll;
            angular.forEach($scope.filterValue5, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.searchValueDesg = '';
            $scope.designationselectedAll = false;
            $scope.Deptselectiondetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };
        $scope.togchkbxdept = function (groupType) {
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {
                return itm.selected;
            });
            $scope.searchValueDesg == '';
            $scope.designationselectedAll = false;
            $scope.Deptselectiondetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };
        //*************************** Designation REPORT *********************//        
        $scope.all_checkdesg = function (designationselectedAll) {
            var toggleStatus = designationselectedAll;
            angular.forEach($scope.filterValue1, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.searchValueUEM = '';
            $scope.checkall = false;
            $scope.get_Plannerlist = [];
            $scope.Desgselectiondetails();
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
            $scope.get_Plannerlist = [];
            $scope.Desgselectiondetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };
        $scope.isOptionsRequired = function () {
            return !$scope.get_userEmplist.some(function (options) {
                return options.empck;
            });
        };

        $scope.onchangeReportType = function () {
            $scope.get_plannerreport = [];
        };

        //*************************** Deptselectiondetails REPORT *********************//
        $scope.Deptselectiondetails = function () {
            $scope.get_Plannerlist = [];
            $scope.plannerid = [];
            $scope.obj.HRME_Id = "";
            $scope.arrayuserEmp = [];
            $scope.designationdropdown = [];
            $scope.get_userEmplist = [];
            angular.forEach($scope.departmentdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserEmp.push(em);
                }
            });
            var data = {
                "departmentlist": $scope.arrayuserEmp
            };
            apiService.create("ISM_ApprovalReport/Deptselectiondetails", data).then(function (promise) {
                if (promise.designation != null && promise.designation.length > 0) {
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
                    swal("No Record  Found..... !!");
                }
            });
        };




        $scope.ename = '';
        $scope.plnn = '';
        $scope.overalleffort = 0;
        $scope.overalleffortC = 0;
        $scope.overalleffortCP = 0;
        $scope.overalleffortD = 0;
        $scope.overalleffortDP = 0;
        $scope.getplannerdetails = function (objp) {
            $scope.ename = '';
            $scope.overalleffortC = 0;
            $scope.overalleffortCP = 0;
            $scope.overalleffortD = 0;
            $scope.overalleffortDP = 0;
            $scope.overalleffort = parseFloat(objp.ISMTPL_TotalHrs);
            $scope.plnn = objp.ISMTPL_PlannerName;
            $scope.appflag = objp.ISMTPL_ApprovalFlg;

            $scope.sdate = objp.ISMTPL_StartDate;
            $scope.edate = objp.ISMTPL_EndDate;
            $scope.appchart_extratask = [];
            $scope.appchart_deviation = [];
            $scope.appchart_category = [];
            $('#myModalViewchart').modal('hide');

            $scope.get_plannerdetails = [];
            var data = {
                "ISMTPL_Id": objp.ISMTPL_Id
            };
            apiService.create("ISM_PlannerCreation/getplannerdetails", data).then(function (promise) {

                $scope.plannerdetailsall = [];
                if (promise.get_plannerdetails.length > 0) {
                    $scope.plannerdetails = promise.get_plannerdetails;
                    $scope.totaltaskcount = promise.get_plannerdetails.length;
                    var cntt = 0;
                    angular.forEach($scope.plannerdetails, function (pl) {
                        var effrt11 = 0;
                        var status = '';
                        cntt += 1;
                        if (cntt === 1) {
                            $scope.ename = pl.ENAME;
                            $scope.approvedby = pl.approvedby;
                        }
                        var cls = '';
                        if (pl.ISMTPLTA_PreviousTask !== null && pl.ISMTPLTA_PreviousTask == true) {
                            cls = 'backkk';
                        }
                        if (pl.ISMTPLAP_Id === null) {

                            if (pl.ISMTPL_ApprovalFlg === true) {
                                status = "APPROVED";
                            }
                            else {
                                status = "PENDING";
                            }
                        }
                        else {
                            if (pl.ISMTPLTA_ApprovalFlg === false) {

                                status = "REJECTED";
                            }
                            else {

                                status = "APPROVED";
                            }
                        }



                        $scope.get_plannerdetails.push({
                            ISMTPL_Id: pl.ISMTPL_Id, ISMTCR_Id: pl.ISMTCR_Id, HRMP_Name: pl.HRMP_Name, ISMTCR_BugOREnhancementFlg: pl.ISMTCR_BugOREnhancementFlg, ISMTCR_Title: pl.ISMTCR_Title, ISMTCR_Status: pl.ISMTCR_Status, ISMTCR_TaskNo: pl.ISMTCR_TaskNo, ISMMCLT_ClientName: pl.ISMMCLT_ClientName, ISMTPL_CreatedBy: pl.ISMTPL_CreatedBy, plannedby: pl.plannedby, ISMTPL_PlannerName: pl.ISMTPL_PlannerName, ISMTPLTA_Remarks: pl.ISMTPLTA_Remarks, CreatedDate: pl.CreatedDate, ISMTCR_Desc: pl.ISMTCR_Desc, ISMTPLTA_Status: pl.ISMTPLTA_Status,
                            ISMTCR_CreationDate: pl.ISMTCR_CreationDate, ISMTPLTA_StartDate: new Date(pl.ISMTPLTA_StartDate), ISMTPLTA_EndDate: new Date(pl.ISMTPLTA_EndDate), ISMTPLTA_EffortInHrs: pl.ISMTPLTA_EffortInHrs, ISMMTCAT_Id: pl.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: pl.ISMMTCAT_TaskCategoryName, ISMMTCAT_TaskPercentage: pl.ISMMTCAT_TaskPercentage, ISMMTCAT_DurationFlg: pl.ISMMTCAT_DurationFlg, ISMMTCAT_CompulsoryFlg: pl.ISMMTCAT_CompulsoryFlg, ISMMTCAT_EachTaskMaxDuration: pl.ISMMTCAT_EachTaskMaxDuration, Flg: pl.ISMTPLTA_PreviousTask, cls: cls, status: status, ISMTPLTA_ExtraFlg: pl.ISMTPLTA_ExtraFlg, Periodicity: pl.Periodicity
                        });


                        $scope.plannerdetailsall = $scope.get_plannerdetails;
                    });
                    //============Shivu start========

                    $scope.get_plannerdetails_approval = [];
                    $scope.get_plannerdetails_reject = [];
                    $scope.get_plannerdetails_previous = [];
                    $scope.get_plannerdetails_extratask = [];

                    angular.forEach($scope.get_plannerdetails, function (dev) {
                        if (dev.ISMTPLTA_ExtraFlg === false) {
                            if (dev.cls !== "backkk") {
                                if (dev.status === "APPROVED") {
                                    $scope.get_plannerdetails_approval.push(dev);
                                    $scope.pending = false;
                                }
                                else if (dev.status === "PENDING") {
                                    $scope.get_plannerdetails_approval.push(dev);
                                    $scope.pending = true;
                                }
                                else {
                                    $scope.get_plannerdetails_reject.push(dev);
                                }
                            }
                            else {
                                $scope.get_plannerdetails_previous.push(dev);
                            }
                        }
                        else {
                            $scope.get_plannerdetails_extratask.push(dev);
                        }
                    });

                    $scope.get_plannerdetails = $scope.get_plannerdetails_approval
                    if ($scope.get_plannerdetails_extratask.length > 0) {
                        $scope.extratask_one = $scope.get_plannerdetails_extratask.length
                    }
                    $scope.extratask = [];
                    $scope.extratask.push({ extracount: $scope.extratask_one })
                    //-------------------Approval/Pending
                    $scope.taskid = [];
                    angular.forEach($scope.get_plannerdetails, function (dev) {
                        if ($scope.taskid.length === 0) {
                            $scope.taskid.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                        } else if ($scope.taskid.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.taskid, function (emp) {
                                if (emp.ISMMTCAT_Id === dev.ISMMTCAT_Id) {
                                    intcount += 1;
                                }
                            });

                            if (intcount === 0) {
                                $scope.taskid.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                            }
                        }
                    });
                    console.log($scope.taskid);
                    angular.forEach($scope.taskid, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.get_plannerdetails, function (dd) {
                            if (dd.ISMMTCAT_Id === ddd.ISMMTCAT_Id) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.get_plannerdetails = $scope.templist;
                    });

                    //------------------Reject

                    $scope.taskid1 = [];
                    angular.forEach($scope.get_plannerdetails_reject, function (dev) {
                        if ($scope.taskid1.length === 0) {
                            $scope.taskid1.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                        } else if ($scope.taskid1.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.taskid1, function (emp) {
                                if (emp.ISMMTCAT_Id === dev.ISMMTCAT_Id) {
                                    intcount += 1;
                                }
                            });

                            if (intcount === 0) {
                                $scope.taskid1.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                            }
                        }
                    });
                    console.log($scope.taskid1);
                    angular.forEach($scope.taskid1, function (ddd) {
                        $scope.templist1 = [];
                        angular.forEach($scope.get_plannerdetails_reject, function (dd) {
                            if (dd.ISMMTCAT_Id === ddd.ISMMTCAT_Id) {
                                $scope.templist1.push(dd);
                            }
                        });
                        ddd.get_plannerdetails_reject = $scope.templist1;
                    });


                    //------------------extra task

                    $scope.taskid3 = [];
                    angular.forEach($scope.get_plannerdetails_extratask, function (dev) {

                        if ($scope.taskid3.length === 0) {
                            $scope.taskid3.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                        } else if ($scope.taskid3.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.taskid3, function (emp) {
                                if (emp.ISMMTCAT_Id === dev.ISMMTCAT_Id) {
                                    intcount += 1;
                                }
                            });

                            if (intcount === 0) {
                                $scope.taskid3.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                            }
                        }
                    });
                    console.log($scope.taskid3);
                    angular.forEach($scope.taskid3, function (ddd) {
                        $scope.templist3 = [];
                        angular.forEach($scope.get_plannerdetails_extratask, function (dd) {
                            if (dd.ISMMTCAT_Id === ddd.ISMMTCAT_Id) {
                                $scope.templist3.push(dd);
                            }
                        });
                        ddd.get_plannerdetails_extratask = $scope.templist3;
                    });
                    //-------------------Deviated
                    $scope.taskid5 = [];
                    angular.forEach($scope.get_plannerdetails_previous, function (dev) {
                        if ($scope.taskid5.length === 0) {
                            $scope.taskid5.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                        } else if ($scope.taskid5.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.taskid5, function (emp) {
                                if (emp.ISMMTCAT_Id === dev.ISMMTCAT_Id) {
                                    intcount += 1;
                                }
                            });

                            if (intcount === 0) {
                                $scope.taskid5.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                            }
                        }
                    });
                    console.log($scope.taskid5);
                    angular.forEach($scope.taskid5, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.get_plannerdetails_previous, function (dd) {
                            if (dd.ISMMTCAT_Id === ddd.ISMMTCAT_Id) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.get_plannerdetails_previous = $scope.templist;
                    });
                    //-------------------------------------

                    //============Shivu End========


                    $scope.categoryidlist1 = [];
                    var snocount = 0;
                    var countsubgrp = 0;
                    angular.forEach($scope.plannerdetailsall, function (sub) {
                        if ($scope.categoryidlist1.length === 0) {
                            snocount += 1;
                            $scope.categoryidlist1.push({ ISMMTCAT_Id: sub.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: sub.ISMMTCAT_TaskCategoryName, ISMMTCAT_TaskPercentage: sub.ISMMTCAT_TaskPercentage, ISMMTCAT_DurationFlg: sub.ISMMTCAT_DurationFlg, ISMMTCAT_CompulsoryFlg: sub.ISMMTCAT_CompulsoryFlg, ISMMTCAT_EachTaskMaxDuration: sub.ISMMTCAT_EachTaskMaxDuration });
                        } else if ($scope.categoryidlist1.length > 0) {
                            countsubgrp = 0;
                            angular.forEach($scope.categoryidlist1, function (subgrp) {
                                if (subgrp.ISMMTCAT_Id === sub.ISMMTCAT_Id) {
                                    countsubgrp += 1;
                                }
                            });
                            if (countsubgrp === 0) {
                                snocount += 1;
                                $scope.categoryidlist1.push({ ISMMTCAT_Id: sub.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: sub.ISMMTCAT_TaskCategoryName, ISMMTCAT_TaskPercentage: sub.ISMMTCAT_TaskPercentage, ISMMTCAT_DurationFlg: sub.ISMMTCAT_DurationFlg, ISMMTCAT_CompulsoryFlg: sub.ISMMTCAT_CompulsoryFlg, ISMMTCAT_EachTaskMaxDuration: sub.ISMMTCAT_EachTaskMaxDuration });
                            }
                        }
                    });



                    $scope.finalcategoryidlist = [];


                    $scope.overalleffortC = 0;
                    $scope.overalleffortCP = 0;
                    $scope.overalleffortD = 0;
                    $scope.overalleffortDP = 0;

                    angular.forEach($scope.plannerdetailsall, function (ww) {
                        if (ww.status != 'REJECTED') {
                            if (ww.Flg == true && ww.Flg != null) {
                                $scope.overalleffortD = parseFloat($scope.overalleffortD) + parseFloat(ww.ISMTPLTA_EffortInHrs)
                            }
                        }
                    });

                    if ($scope.overalleffort > 0) {
                        $scope.overalleffortCP = (($scope.overalleffort - $scope.overalleffortD) / $scope.overalleffort) * 100;
                        $scope.overalleffortDP = ($scope.overalleffortD / $scope.overalleffort) * 100;
                    }
                    angular.forEach($scope.categoryidlist1, function (rr) {
                        var cateffort = 0;
                        var devcateffort = 0;
                        var curper = 0;
                        var devper = 0;
                        angular.forEach($scope.plannerdetailsall, function (ff) {
                            if (ff.ISMMTCAT_Id != null && ff.ISMMTCAT_Id != undefined) {
                                if (parseInt(rr.ISMMTCAT_Id) == parseInt(ff.ISMMTCAT_Id)) {
                                    if (ff.status != 'REJECTED') {
                                        if (ff.Flg == true) {
                                            devcateffort = devcateffort + parseFloat(ff.ISMTPLTA_EffortInHrs);
                                        }
                                        if (ff.status != 'REJECTED') {
                                            cateffort = cateffort + parseFloat(ff.ISMTPLTA_EffortInHrs);
                                        }
                                    }
                                }
                            }
                        });

                        if (cateffort != 0) {
                            curper = (parseFloat(cateffort - devcateffort) / cateffort) * 100;
                            devper = (parseFloat(devcateffort) / cateffort) * 100;

                            $scope.finalcategoryidlist.push({
                                ISMMTCAT_Id: rr.ISMMTCAT_Id,
                                CNAME: rr.ISMMTCAT_TaskCategoryName,
                                MINP: rr.ISMMTCAT_TaskPercentage,
                                TE: cateffort, DE: devcateffort, CWP: curper, DP: devper, flg: rr.ISMMTCAT_CompulsoryFlg
                            })
                            $scope.finalcategory = [];
                            $scope.finalcategory = $scope.finalcategoryidlist
                        }


                    })


                    //====================
                    $scope.get_category = [];
                    angular.forEach($scope.finalcategory, function (qq) {

                        $scope.appname = qq.CNAME;
                        $scope.appdata = qq.MINP;
                        $scope.get_category.push({ Id: $scope.appname, Data: $scope.appdata });
                    })

                    $scope.appchart_category = [];
                    angular.forEach($scope.get_category, function (value, index) {
                        $scope.appchart_category.push({ 'label': value.Id, 'y': value.Data });
                    });
                    var chart = new CanvasJS.Chart("appchartcategory", {
                        title: {
                            text: " Chart Representation Of Category "
                        },
                        data: [
                            {
                                // Change type to "doughnut", "line", "splineArea", etc.
                                type: "line",
                                dataPoints: $scope.appchart_category
                            }
                        ]
                    });
                    chart.render();


                    //=====================
                    $scope.get_deviation = [];
                    $scope.get_deviationtotal = [];
                    $scope.appdatanew = 0;

                    $scope.appname = "Current Week Effort";
                    $scope.appdata = $scope.overalleffortCP / 100;
                    $scope.get_deviation.push({ Id: $scope.appname, Data: $scope.appdata });
                    $scope.appname = "Deviated Effort";
                    $scope.appdata = $scope.overalleffortDP / 100;
                    $scope.get_deviation.push({ Id: $scope.appname, Data: $scope.appdata });

                    $scope.appchart_deviation = [];
                    angular.forEach($scope.get_deviation, function (value, index) {
                        $scope.appchart_deviation.push({ 'label': value.Id, 'y': value.Data });
                    });
                    var chart = new CanvasJS.Chart("appchartdeviation", {
                        animationEnabled: true,
                        title: {
                            text: " Chart Representation Of Last Week Deviation "
                        },
                        data: [
                            {
                                // Change type to "doughnut", "line", "splineArea", etc.
                                type: "pie",
                                startAngle: 240,
                                yValueFormatString: "#,##0.########%",
                                indexLabel: "{label} {y}",
                                dataPoints: $scope.appchart_deviation
                            }
                        ]
                    });
                    chart.render();

                    //=======================================
                    //=====================
                    $scope.get_extratask = [];

                    $scope.appname = "Total Task";
                    $scope.appdata = $scope.totaltaskcount;
                    $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });

                    $scope.appname = "Deviated Task";
                    $scope.appdata = $scope.get_plannerdetails_previous.length;
                    $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });

                    if ($scope.pending === true) {
                        $scope.appname = "Approval Pending Task";
                        $scope.appdata = $scope.get_plannerdetails.length;
                        $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });
                    }
                    else {
                        $scope.appname = "Approved Task";
                        $scope.appdata = $scope.get_plannerdetails.length;
                        $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });
                    }
                    if ($scope.pending === false) {

                        $scope.appname = "Rejected Task";
                        $scope.appdata = $scope.get_plannerdetails_reject.length;
                        $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });

                        $scope.appname = "Extra Task";
                        $scope.appdata = $scope.get_plannerdetails_extratask.length;
                        $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });
                    }

                    $scope.appchart_extratask = [];
                    angular.forEach($scope.get_extratask, function (value, index) {
                        $scope.appchart_extratask.push({ 'label': value.Id, 'y': value.Data });
                    });
                    var chart = new CanvasJS.Chart("appcharttotaltask", {
                        title: {
                            text: " Chart Representation Of Total Task"
                        },
                        data: [
                            {
                                // Change type to "doughnut", "line", "splineArea", etc.
                                type: "column",
                                dataPoints: $scope.appchart_extratask
                            }
                        ]
                    });
                    chart.render();

                    //=======================================

                    console.log($scope.finalcategoryidlist);
                }
                else {
                    swal("No Record  Found..... !!");
                    $scope.get_plannerdetails = "";
                }
            });
        };
        //*************************** Desgselectiondetails REPORT *********************//
        $scope.getplannerdetailsgraph = function (objp) {
            $scope.ename = '';
            $scope.overalleffortC = 0;
            $scope.overalleffortCP = 0;
            $scope.overalleffortD = 0;
            $scope.overalleffortDP = 0;
            $scope.overalleffort = parseFloat(objp.ISMTPL_TotalHrs);
            $scope.plnn = objp.ISMTPL_PlannerName;
            $scope.appflag = objp.ISMTPL_ApprovalFlg;

            $scope.sdate = objp.ISMTPL_StartDate;
            $scope.edate = objp.ISMTPL_EndDate;
            $scope.appchart_extratask = [];
            $scope.appchart_deviation = [];
            $scope.appchart_category = [];
            $('#myModalViewchart').modal('hide');

            $scope.get_plannerdetails = [];
            var data = {
                "ISMTPL_Id": objp.ISMTPL_Id
            };
            apiService.create("ISM_PlannerCreation/getplannerdetails", data).then(function (promise) {

                $scope.plannerdetailsall = [];
                if (promise.get_plannerdetails.length > 0) {
                    $scope.plannerdetails = promise.get_plannerdetails;
                    $scope.totaltaskcount = promise.get_plannerdetails.length;
                    var cntt = 0;
                    angular.forEach($scope.plannerdetails, function (pl) {
                        var effrt11 = 0;
                        var status = '';
                        cntt += 1;
                        if (cntt === 1) {
                            $scope.ename = pl.ENAME;
                            $scope.approvedby = pl.approvedby;
                        }
                        var cls = '';
                        if (pl.ISMTPLTA_PreviousTask !== null && pl.ISMTPLTA_PreviousTask == true) {
                            cls = 'backkk';
                        }
                        if (pl.ISMTPLAP_Id === null) {

                            if (pl.ISMTPL_ApprovalFlg === true) {
                                status = "APPROVED";
                            }
                            else {
                                status = "PENDING";
                            }
                        }
                        else {
                            if (pl.ISMTPLTA_ApprovalFlg === false) {

                                status = "REJECTED";
                            }
                            else {

                                status = "APPROVED";
                            }
                        }



                        $scope.get_plannerdetails.push({
                            ISMTPL_Id: pl.ISMTPL_Id, ISMTCR_Id: pl.ISMTCR_Id, HRMP_Name: pl.HRMP_Name, ISMTCR_BugOREnhancementFlg: pl.ISMTCR_BugOREnhancementFlg, ISMTCR_Title: pl.ISMTCR_Title, ISMTCR_Status: pl.ISMTCR_Status, ISMTCR_TaskNo: pl.ISMTCR_TaskNo, ISMMCLT_ClientName: pl.ISMMCLT_ClientName, ISMTPL_CreatedBy: pl.ISMTPL_CreatedBy, plannedby: pl.plannedby, ISMTPL_PlannerName: pl.ISMTPL_PlannerName, ISMTPLTA_Remarks: pl.ISMTPLTA_Remarks, CreatedDate: pl.CreatedDate, ISMTCR_Desc: pl.ISMTCR_Desc, ISMTPLTA_Status: pl.ISMTPLTA_Status,
                            ISMTCR_CreationDate: pl.ISMTCR_CreationDate, ISMTPLTA_StartDate: new Date(pl.ISMTPLTA_StartDate), ISMTPLTA_EndDate: new Date(pl.ISMTPLTA_EndDate), ISMTPLTA_EffortInHrs: pl.ISMTPLTA_EffortInHrs, ISMMTCAT_Id: pl.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: pl.ISMMTCAT_TaskCategoryName, ISMMTCAT_TaskPercentage: pl.ISMMTCAT_TaskPercentage, ISMMTCAT_DurationFlg: pl.ISMMTCAT_DurationFlg, ISMMTCAT_CompulsoryFlg: pl.ISMMTCAT_CompulsoryFlg, ISMMTCAT_EachTaskMaxDuration: pl.ISMMTCAT_EachTaskMaxDuration, Flg: pl.ISMTPLTA_PreviousTask, cls: cls, status: status, ISMTPLTA_ExtraFlg: pl.ISMTPLTA_ExtraFlg, Periodicity: pl.Periodicity
                        });


                        $scope.plannerdetailsall = $scope.get_plannerdetails;
                    });
                    //============Shivu start========

                    $scope.get_plannerdetails_approval = [];
                    $scope.get_plannerdetails_reject = [];
                    $scope.get_plannerdetails_previous = [];
                    $scope.get_plannerdetails_extratask = [];

                    angular.forEach($scope.get_plannerdetails, function (dev) {
                        if (dev.ISMTPLTA_ExtraFlg === false) {
                            if (dev.cls !== "backkk") {
                                if (dev.status === "APPROVED") {
                                    $scope.get_plannerdetails_approval.push(dev);
                                    $scope.pending = false;
                                }
                                else if (dev.status === "PENDING") {
                                    $scope.get_plannerdetails_approval.push(dev);
                                    $scope.pending = true;
                                }
                                else {
                                    $scope.get_plannerdetails_reject.push(dev);
                                }
                            }
                            else {
                                $scope.get_plannerdetails_previous.push(dev);
                            }
                        }
                        else {
                            $scope.get_plannerdetails_extratask.push(dev);
                        }
                    });

                    $scope.get_plannerdetails = $scope.get_plannerdetails_approval
                    if ($scope.get_plannerdetails_extratask.length > 0) {
                        $scope.extratask_one = $scope.get_plannerdetails_extratask.length
                    }
                    $scope.extratask = [];
                    $scope.extratask.push({ extracount: $scope.extratask_one })
                    //-------------------Approval/Pending
                    $scope.taskid = [];
                    angular.forEach($scope.get_plannerdetails, function (dev) {
                        if ($scope.taskid.length === 0) {
                            $scope.taskid.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                        } else if ($scope.taskid.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.taskid, function (emp) {
                                if (emp.ISMMTCAT_Id === dev.ISMMTCAT_Id) {
                                    intcount += 1;
                                }
                            });

                            if (intcount === 0) {
                                $scope.taskid.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                            }
                        }
                    });
                    console.log($scope.taskid);
                    angular.forEach($scope.taskid, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.get_plannerdetails, function (dd) {
                            if (dd.ISMMTCAT_Id === ddd.ISMMTCAT_Id) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.get_plannerdetails = $scope.templist;
                    });

                    //------------------Reject

                    $scope.taskid1 = [];
                    angular.forEach($scope.get_plannerdetails_reject, function (dev) {
                        if ($scope.taskid1.length === 0) {
                            $scope.taskid1.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                        } else if ($scope.taskid1.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.taskid1, function (emp) {
                                if (emp.ISMMTCAT_Id === dev.ISMMTCAT_Id) {
                                    intcount += 1;
                                }
                            });

                            if (intcount === 0) {
                                $scope.taskid1.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                            }
                        }
                    });
                    console.log($scope.taskid1);
                    angular.forEach($scope.taskid1, function (ddd) {
                        $scope.templist1 = [];
                        angular.forEach($scope.get_plannerdetails_reject, function (dd) {
                            if (dd.ISMMTCAT_Id === ddd.ISMMTCAT_Id) {
                                $scope.templist1.push(dd);
                            }
                        });
                        ddd.get_plannerdetails_reject = $scope.templist1;
                    });


                    //------------------extra task

                    $scope.taskid3 = [];
                    angular.forEach($scope.get_plannerdetails_extratask, function (dev) {

                        if ($scope.taskid3.length === 0) {
                            $scope.taskid3.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                        } else if ($scope.taskid3.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.taskid3, function (emp) {
                                if (emp.ISMMTCAT_Id === dev.ISMMTCAT_Id) {
                                    intcount += 1;
                                }
                            });

                            if (intcount === 0) {
                                $scope.taskid3.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                            }
                        }
                    });
                    console.log($scope.taskid3);
                    angular.forEach($scope.taskid3, function (ddd) {
                        $scope.templist3 = [];
                        angular.forEach($scope.get_plannerdetails_extratask, function (dd) {
                            if (dd.ISMMTCAT_Id === ddd.ISMMTCAT_Id) {
                                $scope.templist3.push(dd);
                            }
                        });
                        ddd.get_plannerdetails_extratask = $scope.templist3;
                    });
                    //-------------------Deviated
                    $scope.taskid5 = [];
                    angular.forEach($scope.get_plannerdetails_previous, function (dev) {
                        if ($scope.taskid5.length === 0) {
                            $scope.taskid5.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                        } else if ($scope.taskid5.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.taskid5, function (emp) {
                                if (emp.ISMMTCAT_Id === dev.ISMMTCAT_Id) {
                                    intcount += 1;
                                }
                            });

                            if (intcount === 0) {
                                $scope.taskid5.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                            }
                        }
                    });
                    console.log($scope.taskid5);
                    angular.forEach($scope.taskid5, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.get_plannerdetails_previous, function (dd) {
                            if (dd.ISMMTCAT_Id === ddd.ISMMTCAT_Id) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.get_plannerdetails_previous = $scope.templist;
                    });
                    //-------------------------------------

                    //============Shivu End========


                    $scope.categoryidlist1 = [];
                    var snocount = 0;
                    var countsubgrp = 0;
                    angular.forEach($scope.plannerdetailsall, function (sub) {
                        if ($scope.categoryidlist1.length === 0) {
                            snocount += 1;
                            $scope.categoryidlist1.push({ ISMMTCAT_Id: sub.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: sub.ISMMTCAT_TaskCategoryName, ISMMTCAT_TaskPercentage: sub.ISMMTCAT_TaskPercentage, ISMMTCAT_DurationFlg: sub.ISMMTCAT_DurationFlg, ISMMTCAT_CompulsoryFlg: sub.ISMMTCAT_CompulsoryFlg, ISMMTCAT_EachTaskMaxDuration: sub.ISMMTCAT_EachTaskMaxDuration });
                        } else if ($scope.categoryidlist1.length > 0) {
                            countsubgrp = 0;
                            angular.forEach($scope.categoryidlist1, function (subgrp) {
                                if (subgrp.ISMMTCAT_Id === sub.ISMMTCAT_Id) {
                                    countsubgrp += 1;
                                }
                            });
                            if (countsubgrp === 0) {
                                snocount += 1;
                                $scope.categoryidlist1.push({ ISMMTCAT_Id: sub.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: sub.ISMMTCAT_TaskCategoryName, ISMMTCAT_TaskPercentage: sub.ISMMTCAT_TaskPercentage, ISMMTCAT_DurationFlg: sub.ISMMTCAT_DurationFlg, ISMMTCAT_CompulsoryFlg: sub.ISMMTCAT_CompulsoryFlg, ISMMTCAT_EachTaskMaxDuration: sub.ISMMTCAT_EachTaskMaxDuration });
                            }
                        }
                    });



                    $scope.finalcategoryidlist = [];


                    $scope.overalleffortC = 0;
                    $scope.overalleffortCP = 0;
                    $scope.overalleffortD = 0;
                    $scope.overalleffortDP = 0;

                    angular.forEach($scope.plannerdetailsall, function (ww) {
                        if (ww.status != 'REJECTED') {
                            if (ww.Flg == true && ww.Flg != null) {
                                $scope.overalleffortD = parseFloat($scope.overalleffortD) + parseFloat(ww.ISMTPLTA_EffortInHrs)
                            }
                        }
                    });

                    if ($scope.overalleffort > 0) {
                        $scope.overalleffortCP = (($scope.overalleffort - $scope.overalleffortD) / $scope.overalleffort) * 100;
                        $scope.overalleffortDP = ($scope.overalleffortD / $scope.overalleffort) * 100;
                    }
                    angular.forEach($scope.categoryidlist1, function (rr) {
                        var cateffort = 0;
                        var devcateffort = 0;
                        var curper = 0;
                        var devper = 0;
                        angular.forEach($scope.plannerdetailsall, function (ff) {
                            if (ff.ISMMTCAT_Id != null && ff.ISMMTCAT_Id != undefined) {
                                if (parseInt(rr.ISMMTCAT_Id) == parseInt(ff.ISMMTCAT_Id)) {
                                    if (ff.status != 'REJECTED') {
                                        if (ff.Flg == true) {
                                            devcateffort = devcateffort + parseFloat(ff.ISMTPLTA_EffortInHrs);
                                        }
                                        if (ff.status != 'REJECTED') {
                                            cateffort = cateffort + parseFloat(ff.ISMTPLTA_EffortInHrs);
                                        }
                                    }
                                }
                            }
                        });

                        if (cateffort != 0) {
                            curper = (parseFloat(cateffort - devcateffort) / cateffort) * 100;
                            devper = (parseFloat(devcateffort) / cateffort) * 100;

                            $scope.finalcategoryidlist.push({
                                ISMMTCAT_Id: rr.ISMMTCAT_Id,
                                CNAME: rr.ISMMTCAT_TaskCategoryName,
                                MINP: rr.ISMMTCAT_TaskPercentage,
                                TE: cateffort, DE: devcateffort, CWP: curper, DP: devper, flg: rr.ISMMTCAT_CompulsoryFlg
                            })
                            $scope.finalcategory = [];
                            $scope.finalcategory = $scope.finalcategoryidlist
                        }


                    })


                    //====================
                    $scope.get_category = [];
                    angular.forEach($scope.finalcategory, function (qq) {

                        $scope.appname = qq.CNAME;
                        $scope.appdata = qq.MINP;
                        $scope.get_category.push({ Id: $scope.appname, Data: $scope.appdata });
                    })

                    $scope.appchart_category = [];
                    angular.forEach($scope.get_category, function (value, index) {
                        $scope.appchart_category.push({ 'label': value.Id, 'y': value.Data });
                    });
                    var chart = new CanvasJS.Chart("appchartcategory", {
                        title: {
                            text: " Chart Representation Of Category "
                        },
                        data: [
                            {
                                // Change type to "doughnut", "line", "splineArea", etc.
                                type: "line",
                                dataPoints: $scope.appchart_category
                            }
                        ]
                    });
                    chart.render();


                    //=====================
                    $scope.get_deviation = [];
                    $scope.get_deviationtotal = [];
                    $scope.appdatanew = 0;

                    $scope.appname = "Current Week Effort";
                    $scope.appdata = $scope.overalleffortCP / 100;
                    $scope.get_deviation.push({ Id: $scope.appname, Data: $scope.appdata });
                    $scope.appname = "Deviated Effort";
                    $scope.appdata = $scope.overalleffortDP / 100;
                    $scope.get_deviation.push({ Id: $scope.appname, Data: $scope.appdata });

                    $scope.appchart_deviation = [];
                    angular.forEach($scope.get_deviation, function (value, index) {
                        $scope.appchart_deviation.push({ 'label': value.Id, 'y': value.Data });
                    });
                    var chart = new CanvasJS.Chart("appchartdeviation", {
                        animationEnabled: true,
                        title: {
                            text: " Chart Representation Of Last Week Deviation "
                        },
                        data: [
                            {
                                // Change type to "doughnut", "line", "splineArea", etc.
                                type: "pie",
                                startAngle: 240,
                                yValueFormatString: "#,##0.########%",
                                indexLabel: "{label} {y}",
                                dataPoints: $scope.appchart_deviation
                            }
                        ]
                    });
                    chart.render();

                    //=======================================
                    //=====================
                    $scope.get_extratask = [];

                    $scope.appname = "Total Task";
                    $scope.appdata = $scope.totaltaskcount;
                    $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });

                    $scope.appname = "Deviated Task";
                    $scope.appdata = $scope.get_plannerdetails_previous.length;
                    $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });

                    if ($scope.pending === true) {
                        $scope.appname = "Approval Pending Task";
                        $scope.appdata = $scope.get_plannerdetails.length;
                        $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });
                    }
                    else {
                        $scope.appname = "Approved Task";
                        $scope.appdata = $scope.get_plannerdetails.length;
                        $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });
                    }
                    if ($scope.pending === false) {

                        $scope.appname = "Rejected Task";
                        $scope.appdata = $scope.get_plannerdetails_reject.length;
                        $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });

                        $scope.appname = "Extra Task";
                        $scope.appdata = $scope.get_plannerdetails_extratask.length;
                        $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });
                    }

                    $scope.appchart_extratask = [];
                    angular.forEach($scope.get_extratask, function (value, index) {
                        $scope.appchart_extratask.push({ 'label': value.Id, 'y': value.Data });
                    });
                    var chart = new CanvasJS.Chart("appcharttotaltask", {
                        title: {
                            text: " Chart Representation Of Total Task"
                        },
                        data: [
                            {
                                // Change type to "doughnut", "line", "splineArea", etc.
                                type: "column",
                                dataPoints: $scope.appchart_extratask
                            }
                        ]
                    });
                    chart.render();

                    //=======================================

                    console.log($scope.finalcategoryidlist);
                }
                else {
                    swal("No Record  Found..... !!");
                    $scope.get_plannerdetails = "";
                }
            });
        };

        $scope.Desgselectiondetails = function () {
            $scope.get_Plannerlist = [];
            $scope.plannerid = [];
            $scope.obj.HRME_Id = "";
            $scope.arrayuserEmp = [];
            $scope.get_userEmplist = [];
            angular.forEach($scope.designationdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserEmp.push(em);
                }
            });
            var data = {
                "designationlist": $scope.arrayuserEmp
            };
            apiService.create("ISM_ApprovalReport/Desgselectiondetails", data).then(function (promise) {
                if (promise.get_userEmplist.length > 0) {
                    $scope.get_userEmplist = promise.get_userEmplist;
                    if ($scope.get_userEmplist.length > 0) {
                        angular.forEach($scope.get_userEmplist, function (uem) {
                            uem.empck = true;
                        });
                        $scope.checkall = true;
                        $scope.get_Plannerlist = [];
                        $scope.plannerid = [];
                    }
                }
                else {
                    swal("No Record  Found..... !!");
                }
            });
        };
        //***************************** Date Change
        $scope.onenddatechange = function () {
            $scope.startdate = new Date($scope.start_Date).toDateString();
            $scope.enddate = new Date($scope.end_Date).toDateString();
            $scope.dateflag = 1;
            $scope.onEmployeechange();

        };
        //***************************** Get Employee list
        //*************************** PLANNER REPORT *********************//        
        $scope.plannerReport = function () {
            if ($scope.myForm.$valid) {
                $scope.arrayuserEmp = [];
                angular.forEach($scope.get_userEmplist, function (em) {
                    if (em.empck === true) {
                        $scope.arrayuserEmp.push(em);
                    }
                });
                var data = {
                    "flag": $scope.reporttype,
                    "startdate": new Date($scope.start_Date).toDateString(),
                    "enddate": new Date($scope.end_Date).toDateString(),
                    arrayuserEmp: $scope.arrayuserEmp
                };
                apiService.create("ISM_PlannerReport/getplannerReport", data).then(function (promise) {

                    if (promise.get_plannerreport.length > 0) {
                        $scope.get_plannerreport = promise.get_plannerreport;
                        $scope.plannedby = $scope.get_plannerreport[0].plannedby;

                        $scope.employeeid = [];

                        angular.forEach($scope.get_plannerreport, function (dev) {

                            if ($scope.employeeid.length === 0) {
                                $scope.employeeid.push({ HRME_Id: dev.HRME_Id, employeename: dev.plannedby });
                            } else if ($scope.employeeid.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.employeeid, function (emp) {
                                    if (emp.HRME_Id === dev.HRME_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.employeeid.push({ HRME_Id: dev.HRME_Id, employeename: dev.plannedby });
                                }
                            }
                        });
                        console.log($scope.employeeid);

                        angular.forEach($scope.employeeid, function (ddd) {

                            $scope.templist = [];
                            angular.forEach($scope.get_plannerreport, function (dd) {
                                if (dd.HRME_Id === ddd.HRME_Id) {
                                    $scope.templist.push(dd);
                                }
                            });
                            ddd.plannerdetails = $scope.templist;
                        });


                        console.log($scope.employeeid);
                    }
                    else {
                        swal("No Record  Found..... !!");
                        $scope.get_plannerreport = "";
                    }

                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printDeviation").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.printData1 = function () {
            var innerContents = "";
            innerContents = document.getElementById("asdf").innerHTML;
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
            var blob = new Blob([document.getElementById('myTable').innerHTML], {
                type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
            });

            saveAs(blob, "Planner Report.xls");
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.Clear = function () {
            $scope.get_plannerreport = "";
            $scope.get_userEmplist = "";
            $scope.checkall = "";
            $scope.start_Date = "";
            $scope.end_Date = "";
            $scope.loaddata();
        };

    }
})();

