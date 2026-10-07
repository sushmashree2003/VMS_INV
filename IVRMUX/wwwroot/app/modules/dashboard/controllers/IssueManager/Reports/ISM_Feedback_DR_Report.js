(function () {
    'use strict';
    angular.module('app').controller('ISM_Feedback_DR_ReportController', ISM_Feedback_DR_ReportController);
    ISM_Feedback_DR_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', 'Excel', '$timeout'];
    function ISM_Feedback_DR_ReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter, Excel, $timeout) {


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
        };

        $scope.Desgselectiondetails = function () {
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
        };

        /* DEVIATION REPORT */
        $scope.getReport = function () {
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
                    "emplist": $scope.emplist
                };


                apiService.create("ISM_ConsolidateDailyReport/getfeedbackReport", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.get_report !== null && promise.get_report.length > 0) {
                            $scope.get_report = promise.get_report;


                            $scope.dept_headist = [];
                            var count1 = 0;

                            angular.forEach($scope.get_report, function (dd) {
                                count1 = 0;
                                if ($scope.dept_headist.length === 0) {
                                    $scope.dept_headist.push({ Emp_Dept_head: dd.DEPT_HEAD });
                                } else if ($scope.dept_headist.length > 0) {
                                    count1 = 0;
                                    angular.forEach($scope.dept_headist, function (d) {
                                        if (dd.DEPT_HEAD === d.Emp_Dept_head) {
                                            count1 += 1;
                                        }
                                    });
                                    if (count1 === 0) {
                                        $scope.dept_headist.push({ Emp_Dept_head: dd.DEPT_HEAD });
                                    }
                                }
                            });

                            $scope.emp_list = [];
                            $scope.emp_dr_dates = [];
                            var count2 = 0;

                            angular.forEach($scope.dept_headist, function (dd) {
                                $scope.emp_dr_dates = [];
                                angular.forEach($scope.get_report, function (d) {
                                    if (dd.Emp_Dept_head === d.DEPT_HEAD) {
                                        $scope.emp_dr_dates.push(d);
                                    }
                                });
                                dd.emp_dr_date_list = $scope.emp_dr_dates;
                            });




                            //angular.forEach($scope.dept_headist, function (dd) {
                            //    $scope.emp_list = [];
                            //    count2 = 0;
                            //    angular.forEach($scope.get_report, function (d) {
                            //        count2 = 0;
                            //        if (dd.Emp_Dept_head === d.DEPT_HEAD) {
                            //            if ($scope.emp_list.length === 0) {
                            //                $scope.emp_list.push({
                            //                    HRME_Id: d.HRME_Id, employeename: d.EMPLOYEENAME, employeecode: d.EMPLOYEECODE,
                            //                    empdept: d.DEPTNAME, empdesig: d.DESGNAME
                            //                });
                            //            } else if ($scope.emp_list.length > 0) {
                            //                count2 = 0;
                            //                angular.forEach($scope.emp_list, function (ddd) {
                            //                    if (ddd.HRME_Id === d.HRME_Id) {
                            //                        count2 += 1;
                            //                    }
                            //                });
                            //                if (count2 === 0) {
                            //                    $scope.emp_list.push({
                            //                        HRME_Id: d.HRME_Id, employeename: d.EMPLOYEENAME, employeecode: d.EMPLOYEECODE,
                            //                        empdept: d.DEPTNAME, empdesig: d.DESGNAME
                            //                    });
                            //                }
                            //            }
                            //        }
                            //    });

                            //    dd.emplist = $scope.emp_list;
                            //});

                            //$scope.emp_dr_dates = [];
                            //var rowcount = 0;
                            //angular.forEach($scope.dept_headist, function (dd) {
                            //    $scope.emp_dr_dates = [];
                            //    angular.forEach(dd.emplist, function (d) {
                            //        var rowcountemp = 0;
                            //        $scope.emp_dr_dates = [];
                            //        angular.forEach($scope.get_report, function (ddd) {
                            //            if (dd.Emp_Dept_head === ddd.DEPT_HEAD && d.HRME_Id === ddd.HRME_Id) {
                            //                $scope.emp_dr_dates.push(ddd);
                            //            }
                            //        });
                            //        d.emp_dates_list = $scope.emp_dr_dates;
                            //        rowcount += $scope.emp_dr_dates.length;
                            //        d.rowcountemp = $scope.emp_dr_dates.length;
                            //    });
                            //    dd.row_count = rowcount;
                            //});

                            console.log($scope.dept_headist);

                        } else {
                            swal("No Record Found.... !!");
                            $scope.get_report = [];
                        }
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.obj1 = {};
        $scope.viewcomments = function (obj1) {
            $scope.get_feedback = [];
            $scope.taskno = obj1.TASKNO;
            $scope.taskdate = obj1.DR_DATE;
            var data = {
                "ISMTCR_Id": obj1.ISMTCR_Id,
                "HRME_Id": obj1.HRME_Id,
                "Dept_Head_Id": obj1.DEPT_HEAD_Id,
                "DR_DATE": obj1.DR_DATE
            };
            apiService.create("ISM_ConsolidateDailyReport/viewcomments", data).then(function (promise) {
                if (promise !== null) {
                    $scope.get_feedback = promise.get_feedback;
                    if ($scope.get_feedback === null || $scope.get_feedback.length ===0) {
                        swal("No Records Found");
                    }
                } else {
                    swal("No Records Found");
                }
            });
        };

        $scope.onchangestartdate = function () {
            $scope.end_Date = null;

            $scope.getMaxDate = new Date(
                $scope.start_Date.getFullYear(),
                $scope.start_Date.getMonth(),
                $scope.start_Date.getDate() + 30);

        };

        $scope.otherdaydatechange = function () {
            $scope.end_Date = null;
            $scope.start_Date = null;
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
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
    }
})();

