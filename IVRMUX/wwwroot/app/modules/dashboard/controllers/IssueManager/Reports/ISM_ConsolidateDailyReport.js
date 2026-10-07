(function () {
    'use strict';
    angular.module('app').controller('ISM_ConsolidateDailyReportController', ISM_ConsolidateDailyReportController);
    ISM_ConsolidateDailyReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', 'Excel', '$timeout'];
    function ISM_ConsolidateDailyReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter, Excel, $timeout) {

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
            apiService.getURI("ISM_ConsolidateDailyReport/getdetails", pageid).then(function (promise) {

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
            $scope.Deptselectiondetails();
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
            $scope.Deptselectiondetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.searchValueUEM = '';
            $scope.employeeid = [];
            $scope.checkall = false;
            $scope.get_deviationreport = [];
        };

        /* DESIGNATION SELECTION */
        $scope.all_checkdesg = function (designationselectedAll) {
            var toggleStatus = designationselectedAll;
            angular.forEach($scope.filterValue1, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.searchValueUEM = '';
            $scope.checkall = false;
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
            $scope.Desgselectiondetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
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

                angular.forEach($scope.get_userEmplist, function (d) {
                    if (d.empck === true) {
                        $scope.emplist.push({ HRME_Id: d.HRME_Id });
                    }
                });

                data = {
                    "start_date": new Date($scope.start_Date).toDateString(),
                    "end_date": new Date($scope.end_Date).toDateString(),
                    "reporttype": $scope.todayorothers,
                    "deptlist": $scope.deptlist,
                    "desglist": $scope.desglist,
                    "emplist": $scope.emplist,
                    "StatusFlg": $scope.StatusFlg
                };

                apiService.create("ISM_ConsolidateDailyReport/getReport", data).then(function (promise) {

                    if ($scope.todayorothers === "consolidatereport") {
                        $scope.get_report = promise.get_report;
                        console.log($scope.get_report);
                        $scope.headerlist = [];
                        if ($scope.get_report.length > 0) {
                            for (var i = 0; i < $scope.get_report.length; i++) {
                                if (i === 0) {
                                    angular.forEach($scope.get_report[i], function (key, r) {
                                        $scope.headerlist.push({ colmn: r });
                                    });
                                }
                            }

                            angular.forEach($scope.get_report, function (fff) {
                                var cnt = 0;
                                var cnto = 0;
                                var cntb = 0;
                                var cnth = 0;
                                var cnotsent = 0;
                                angular.forEach(fff, function (key, ggg) {
                                    if (key === "Y") {
                                        cnt += 1;
                                    }
                                    else if (key === "O") {
                                        cnto += 1;
                                    }
                                    else if (key === "B") {
                                        cntb += 1;
                                    }
                                    else if (key === "H") {
                                        cnth += 1;
                                    }
                                    else if (key === "N") {
                                        cnotsent += 1;
                                    }
                                });
                                fff.total = cnt;
                                fff.totalh = cnth;
                                fff.totalo = cnto;
                                fff.totalb = cntb;
                                fff.totalsent = fff.total + fff.totalo + fff.totalb + (fff.totalh / 2);

                                fff.totalnotsent = cnotsent;

                                //onDataBound = function () {
                                //    $('td').each(function () {
                                //        if (key === 'Y')
                                //        {
                                //            fff.name.addClass('darkred');                                        
                                //        }
                                //    });
                                //};
                            });

                            $scope.mainheader = [];

                            $scope.mainheader.push({ title: 'Employee Name', field: 'EmployeeName', name: 'EmployeeName', width: 150 });
                            $scope.mainheader.push({ title: 'OnDay', field: 'total', name: 'OnDay', width: 150 });
                            $scope.mainheader.push({ title: 'Half Day', field: 'totalh', name: 'HalfDay', width: 150 });
                            $scope.mainheader.push({ title: 'Other Days', field: 'totalo', name: 'Other Days', width: 150 });
                            $scope.mainheader.push({ title: 'Blocked', field: 'totalb', name: 'Blocked', width: 150 });
                            $scope.mainheader.push({ title: 'Total Sent', field: 'totalsent', name: 'Total Sent', width: 150 });
                            $scope.mainheader.push({ title: 'Total Not Sent', field: 'totalnotsent', name: 'Total Not Sent', width: 150 });
                            template: "<span class='#if(name === \'total\') {# red #}#'></span>";

                            angular.forEach($scope.headerlist, function (yyy) {
                                if (yyy.colmn !== 'HRME_Id' && yyy.colmn !== 'EmployeeName') {
                                    $scope.mainheader.push({ title: yyy.colmn, field: yyy.colmn, name: yyy.colmn, width: 150 });
                                }
                            });

                            $(document).ready(function () {
                                initGridall();
                            });


                            function initGridall() {
                                $('#gridall').empty();
                                $("#gridall").kendoGrid({
                                    toolbar: ["excel"],
                                    excel: {
                                        fileName: "ConsolidatedDailyReport.xlsx",
                                        proxyURL: "",
                                        filterable: true,
                                        allPages: true
                                    },
                                    pdf: {
                                        avoidLinks: true,
                                        landscape: true,
                                        repeatHeaders: true,
                                        fileName: "ConsolidatedDailyReport.pdf",
                                        allPages: true
                                    },
                                    dataSource: {
                                        data: $scope.get_report,
                                        pageSize: 10
                                    },
                                    sortable: true,
                                    pageable: true,
                                    groupable: false,
                                    filterable: true,
                                    columnMenu: true,
                                    reorderable: true,
                                    resizable: true,

                                    columns: $scope.mainheader,
                                    dataBound: function () {
                                        var pagenum = this.dataSource.page();
                                        var pageitms = this.dataSource.pageSize();
                                        var rows = this.items();
                                        $(rows).each(function () {
                                            var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                            var rowLabel = $(this).find(".row-number");
                                            $(rowLabel).html(index);
                                        });
                                    }
                                }).data("kendoGrid");
                            }
                        }
                        else {
                            swal("No Record Found.... !!");
                            $scope.get_report = "";
                        }
                    }

                    else if ($scope.todayorothers === "overallcommentsreport") {

                        if (promise.get_report !== null && promise.get_report.length > 0) {
                            $scope.get_report = promise.get_report;

                            $scope.emp_list = [];
                            var count12 = 0;

                            angular.forEach($scope.emplist, function (d) {
                                count12 = 0;
                                angular.forEach($scope.get_report, function (dd) {
                                    if (d.HRME_Id === dd.HRME_Id) {
                                        if ($scope.emp_list.length === 0) {
                                            $scope.emp_list.push({
                                                HRME_Id: dd.HRME_Id, employeename: dd.EMPLOYEENAME, employeecode: dd.EMPLOYEECODE,
                                                empdept: dd.DEPTNAME, empdesig: dd.DESGNAME
                                            });
                                        } else if ($scope.emp_list.length > 0) {
                                            count12 = 0;
                                            angular.forEach($scope.emp_list, function (ddd) {
                                                if (ddd.HRME_Id === d.HRME_Id) {
                                                    count12 += 1;
                                                }
                                            });

                                            if (count12 === 0) {
                                                $scope.emp_list.push({
                                                    HRME_Id: dd.HRME_Id, employeename: dd.EMPLOYEENAME, employeecode: dd.EMPLOYEECODE,
                                                    empdept: dd.DEPTNAME, empdesig: dd.DESGNAME
                                                });
                                            }
                                        }
                                    }
                                });
                            });

                            angular.forEach($scope.emp_list, function (dd) {
                                $scope.emp_drreport = [];
                                countdate = 0;
                                angular.forEach($scope.get_report, function (d) {
                                    if (dd.HRME_Id === d.HRME_Id) {

                                        d.approvedeffort = d.APPROVEDEFFORTS != null ? d.APPROVEDEFFORTS + " Hrs" : '';
                                        d.rejectedeffort = d.REJECTEDEFFORTS != null ? d.REJECTEDEFFORTS + " Hrs" : '';
                                        d.ISMDRPT_TimeTakenInHrs = d.ISMDRPT_TimeTakenInHrs != null ? d.ISMDRPT_TimeTakenInHrs + " Hrs" : '';
                                        $scope.emp_drreport.push(d);
                                    }
                                });
                                dd.emp_drdates = $scope.emp_drreport;
                            });
                        } else {
                            swal("No Records Found");
                        }
                    }

                    else if ($scope.todayorothers === "MultiLevelDRApproval") {
                        if (promise.get_report !== null && promise.get_report.length > 0) {
                            $scope.get_report = promise.get_report;
                            $scope.emp_list = [];
                            // Distinct Employee List
                            angular.forEach($scope.get_report, function (emp) {

                                if ($scope.emp_list.length === 0) {
                                    $scope.emp_list.push({
                                        HRME_Id: emp.HRME_Id, employeename: emp.EMPLOYEENAME, employeecode: emp.EMPLOYEECODE,
                                        empdept: emp.DEPTNAME, empdesig: emp.DESGNAME
                                    });
                                }
                                else if ($scope.emp_list.length > 0) {
                                    var emp_count = 0;
                                    angular.forEach($scope.emp_list, function (ddd) {
                                        if (ddd.HRME_Id === emp.HRME_Id) {
                                            emp_count += 1;
                                        }
                                    });
                                    if (emp_count === 0) {
                                        $scope.emp_list.push({
                                            HRME_Id: emp.HRME_Id, employeename: emp.EMPLOYEENAME, employeecode: emp.EMPLOYEECODE,
                                            empdept: emp.DEPTNAME, empdesig: emp.DESGNAME
                                        });
                                    }
                                }
                            });

                            // Employee Wise Distinct Task List

                            angular.forEach($scope.emp_list, function (emp) {
                                $scope.emp_task_list = [];
                                angular.forEach($scope.get_report, function (emp_task) {
                                    if (emp.HRME_Id === emp_task.HRME_Id) {

                                        /* Actual Efforts */
                                        var decimaltimetot1 = emp_task.ISMTPLTA_EffortInHrs;
                                        var hrstot1 = parseInt(Number(decimaltimetot1));
                                        var mintot1 = Math.round((Number(decimaltimetot1) - hrstot1) * 60);
                                        if (hrstot1.toString().length === 1) {
                                            hrstot1 = '0' + hrstot1;
                                        }
                                        if (mintot1.toString().length === 1) {
                                            mintot1 = '0' + mintot1;
                                        }
                                        emp_task.actualefforts = hrstot1 + " : " + mintot1 + " Hrs";


                                        /* Time Taken In DR Efforts */
                                        var decimaltimetot2 = emp_task.ISMDRPT_TimeTakenInHrs;
                                        var hrstot2 = parseInt(Number(decimaltimetot2));
                                        var mintot2 = Math.round((Number(decimaltimetot2) - hrstot2) * 60);

                                        if (hrstot2.toString().length === 1) {
                                            hrstot2 = '0' + hrstot2;
                                        }

                                        if (mintot2.toString().length === 1) {
                                            mintot2 = '0' + mintot2;
                                        }

                                        emp_task.detimetaken = hrstot2 + " : " + mintot2 + " Hrs";

                                        if ($scope.emp_task_list.length === 0) {
                                            $scope.emp_task_list.push(emp_task);
                                        } else if ($scope.emp_task_list.length > 0) {
                                            var temp_emp_task = 0;
                                            angular.forEach($scope.emp_task_list, function (emp_task_temp) {
                                                if (emp_task_temp.ISMDRPT_Id === emp_task.ISMDRPT_Id) {
                                                    temp_emp_task += 1;
                                                }
                                            });
                                            if (temp_emp_task === 0) {
                                                $scope.emp_task_list.push(emp_task);
                                            }
                                        }
                                    }
                                });


                                angular.forEach($scope.emp_task_list, function (emp_task_multi) {
                                    $scope.task_multi_details = [];
                                    angular.forEach($scope.get_report, function (emp_task_multi_dd) {
                                        if (emp_task_multi.ISMDRPT_Id === emp_task_multi_dd.ISMDRPT_Id && emp_task_multi_dd.HRME_Id === emp.HRME_Id) {

                                            var app_decimaltimetot2 = emp_task_multi_dd.Multi_Approved_Efforts;
                                            var app_hrstot2 = parseInt(Number(app_decimaltimetot2));
                                            var app_mintot2 = Math.round((Number(app_decimaltimetot2) - app_hrstot2) * 60);

                                            if (app_hrstot2.toString().length === 1) {
                                                app_hrstot2 = '0' + app_hrstot2;
                                            }

                                            if (app_mintot2.toString().length === 1) {
                                                app_mintot2 = '0' + app_mintot2;
                                            }

                                            emp_task_multi_dd.app_detimetaken = app_hrstot2 + " : " + app_mintot2 + " Hrs";

                                            $scope.task_multi_details.push({
                                                ISMDRPT_Id: emp_task_multi_dd.ISMDRPT_Id,
                                                MULTI_APPROVED_BY: emp_task_multi_dd.MULTI_APPROVED_BY,
                                                Multi_Approved_Date: emp_task_multi_dd.Multi_Approved_Date,
                                                Multi_Approved_Efforts: emp_task_multi_dd.Multi_Approved_Efforts,
                                                Multi_Approved_Flag: emp_task_multi_dd.Multi_Approved_Flag,
                                                Multi_Approved_Remarks: emp_task_multi_dd.Multi_Approved_Remarks,
                                                app_detimetaken: emp_task_multi_dd.app_detimetaken
                                            });
                                        }
                                    });

                                    emp_task_multi.multi_approved_details = $scope.task_multi_details;
                                });

                                emp.DR_task_list = $scope.emp_task_list;
                            });

                            console.log($scope.emp_list);

                        } else {
                            swal("No Records Found");
                        }
                    }

                    else if ($scope.todayorothers === "DailyReportBlockedCountDetails") {
                        if (promise.get_report !== null && promise.get_report.length > 0) {
                            $scope.get_report = promise.get_report;
                            $scope.emp_list = [];
                            
                            angular.forEach($scope.get_report, function (emp) {

                                if ($scope.emp_list.length === 0) {
                                    $scope.emp_list.push({
                                        HRME_Id: emp.HRME_Id, employeename: emp.EMPLOYEENAME, employeecode: emp.EMPLOYEECODE,
                                        empdept: emp.DEPTNAME, empdesig: emp.DESGNAME
                                    });
                                }
                                else if ($scope.emp_list.length > 0) {
                                    var emp_count = 0;
                                    angular.forEach($scope.emp_list, function (ddd) {
                                        if (ddd.HRME_Id === emp.HRME_Id) {
                                            emp_count += 1;
                                        }
                                    });
                                    if (emp_count === 0) {
                                        $scope.emp_list.push({
                                            HRME_Id: emp.HRME_Id, employeename: emp.EMPLOYEENAME, employeecode: emp.EMPLOYEECODE,
                                            empdept: emp.HRMD_DepartmentName, empdesig: emp.HRMDES_DesignationName
                                        });
                                    }
                                }
                            });

                            // Employee Wise Blocked Count Details
                            angular.forEach($scope.emp_list, function (emp) {
                                $scope.emp_task_list = [];
                                $scope.totalblockeddays = 0;
                                angular.forEach($scope.get_report, function (emp_task) {
                                    if (emp.HRME_Id === emp_task.HRME_Id) {
                                        $scope.totalblockeddays += emp_task.DR_BLOCKED_COUNT;
                                        $scope.emp_task_list.push({
                                            MONTHNAME: emp_task.MONTHNAME,
                                            MONTHID: emp_task.MONTHID,
                                            YEARID: emp_task.YEARID,
                                            DR_BLOCKED_COUNT: emp_task.DR_BLOCKED_COUNT,
                                        });
                                    }
                                });
                                emp.dr_blocked_countdetails = $scope.emp_task_list;
                                emp.total_count = $scope.totalblockeddays;
                            });

                            console.log($scope.emp_list);

                        } else {
                            swal("No Records Found");
                        }
                    }

                    else {
                        if (promise !== null) {
                            if (promise.get_report !== null && promise.get_report.length > 0) {
                                $scope.get_report = promise.get_report;

                                $scope.emp_list = [];
                                var count = 0;

                                angular.forEach($scope.emplist, function (d) {
                                    count = 0;
                                    angular.forEach($scope.get_report, function (dd) {
                                        if (d.HRME_Id === dd.HRME_Id) {
                                            if ($scope.emp_list.length === 0) {
                                                $scope.emp_list.push({
                                                    HRME_Id: dd.HRME_Id, employeename: dd.EMPLOYEENAME, employeecode: dd.EMPLOYEECODE,
                                                    empdept: dd.DEPTNAME, empdesig: dd.DESGNAME
                                                });
                                            } else if ($scope.emp_list.length > 0) {
                                                count = 0;
                                                angular.forEach($scope.emp_list, function (ddd) {
                                                    if (ddd.HRME_Id === d.HRME_Id) {
                                                        count += 1;
                                                    }
                                                });

                                                if (count === 0) {
                                                    $scope.emp_list.push({
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

                                angular.forEach($scope.emp_list, function (dd) {
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
                                console.log($scope.emp_list);
                            } else {
                                swal("No Record Found.... !!");
                                $scope.get_report = [];
                            }
                        }
                    }
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
            }else {
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
            }else {
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

