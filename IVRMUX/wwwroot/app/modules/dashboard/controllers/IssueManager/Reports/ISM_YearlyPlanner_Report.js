(function () {
    'use strict';
    angular.module('app').controller('ISM_YearlyPlanner_ReportController', ISM_YearlyPlanner_ReportController);
    ISM_YearlyPlanner_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', 'Excel', '$timeout'];
    function ISM_YearlyPlanner_ReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter, Excel, $timeout) {


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


                apiService.create("ISM_ConsolidateDailyReport/getyerlyplannerreport", data).then(function (promise) {

                    if (promise !== null) {
                        if (promise.get_report !== null && promise.get_report.length > 0) {
                            $scope.get_report = promise.get_report;

                            $scope.columnsheader = [];
                            $scope.columnsheader.push({ title: 'Department', field: 'HRMD_DepartmentName', name: 'HRMD_DepartmentName', width: 150 });
                            $scope.columnsheader.push({ title: 'Task No.', field: 'ISMTCR_TaskNo', name: 'ISMTCR_TaskNo', width: 150 });
                            $scope.columnsheader.push({ title: 'Task Title', field: 'ISMTCR_Title', name: 'ISMTCR_Title', width: 150 });
                            $scope.columnsheader.push({ title: 'Task Description', field: 'ISMTCR_Desc', name: 'ISMTCR_Desc', width: 150 });
                            $scope.columnsheader.push({ title: 'Module', field: 'IVRMM_ModuleName', name: 'IVRMM_ModuleName', width: 150 });
                            $scope.columnsheader.push({ title: 'Status', field: 'ISMTCR_Status', name: 'ISMTCR_Status', width: 100 });
                            $scope.columnsheader.push({ title: 'Employee', field: 'EMPLOYEE', name: 'EMPLOYEE', width: 100 });
                            $scope.columnsheader.push({ title: 'Assigned By', field: 'AssignedBy', name: 'AssignedBy', width: 100 });                            
                            $scope.columnsheader.push({ title: 'From Date', field: 'ISMTAPL_FromDate', name: 'ISMTAPL_FromDate', width: 100 });
                            $scope.columnsheader.push({ title: 'To Date', field: 'ISMTAPL_ToDate', name: 'ISMTAPL_ToDate', width: 100 });

                            $(document).ready(function () {
                                initGridall();
                            });

                            function initGridall() {
                                $('#gridall').empty();
                                $("#gridall").kendoGrid({
                                    toolbar: ["excel"],
                                    excel: {
                                        fileName: "Yearly Planner Report.xlsx",
                                        proxyURL: "",
                                        filterable: true,
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

                                    columns: $scope.columnsheader,
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
                            swal("No Record Found..... !!");
                            $scope.get_report = [];
                            $state.reload();
                        }
                    }

                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.onchangestartdate = function () {
            $scope.end_Date = null;
            if ($scope.todayorothers !== 'consolidatereport') {
                $scope.getMaxDate = new Date(
                    $scope.start_Date.getFullYear(),
                    $scope.start_Date.getMonth(),
                    $scope.start_Date.getDate() + 14);
            }
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