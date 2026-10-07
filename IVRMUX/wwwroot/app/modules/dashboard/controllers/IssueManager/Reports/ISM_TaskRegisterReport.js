(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_TaskRegisterReportController', ISM_TaskRegisterReportController);
    ISM_TaskRegisterReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function ISM_TaskRegisterReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache) {


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

        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();


        //shilpa

        $scope.searchValueDept = '';
        $scope.filterValue4 = function (obj) {
            return angular.lowercase(obj.HRMDC_Name).indexOf(angular.lowercase($scope.searchValueDept)) >= 0;
        };
        $scope.searchValueDesg = '';
        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.HRMDES_DesignationName).indexOf(angular.lowercase($scope.searchValueDesg)) >= 0 || angular.lowercase(obj.MI_Name).indexOf(angular.lowercase($scope.searchValueDesg)) >= 0;
        };
        $scope.searchValueUEM = '';
        $scope.filterValue2 = function (obj) {
            return angular.lowercase(obj.userEmpName).indexOf(angular.lowercase($scope.searchValueUEM)) >= 0 || angular.lowercase(obj.HRMD_DepartmentName).indexOf(angular.lowercase($scope.searchValueUEM)) >= 0;
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

        $scope.isOptionsRequired = function () {
            return !$scope.get_userEmplist.some(function (options) {
                return options.empck;
            });
        };

        //*************************** Designation REPORT *********************//        
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

        //  $scope.togchkbxdesg = function (groupType) {
        //  $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {
        //      return itm.selected;
        //  });
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

        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.get_userEmplist, function (uem) {
                uem.empck = toggleStatus;
            });
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };


        //*************************** Deptselectiondetails REPORT *********************//
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
            apiService.create("ISM_TaskRegisterReport/Deptselectiondetails", data).then(function (promise) {

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
        //*************************** Desgselectiondetails REPORT *********************//
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
            apiService.create("ISM_TaskRegisterReport/Desgselectiondetails", data).then(function (promise) {
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
                    swal("No Record  Found..... !!");
                }
            });
        };

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.itemsPerPage = paginationformasters;
            $scope.searchValueE = "";
            $scope.get_Status = [];
            var pageid = 2;
            apiService.getURI("ISM_TaskRegisterReport/getdetails", pageid).then(function (promise) {
                //$scope.get_Status = [{ id: 'Open', value: 'Open' }, { id: 'Inprogress', value: 'Inprogress' }, { id: 'Completed', value: 'Completed' }];
                $scope.get_Status = promise.get_Status;
                $scope.prioritylist = promise.prioritylist;
                $scope.get_companyDeptist = promise.get_companyDeptist;
                $scope.client_Master = promise.client_Master;
                // $scope.departmentdropdown = promise.departmentList;
                //$scope.departmentdropdown = promise.DepartmentList;
                $scope.departmentdropdown = promise.departmentList;
                $scope.designationdropdown = promise.designation;
                $scope.get_userEmplist = promise.get_userEmplist;
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
                console.log($scope.get_Status);
            });
        };

        //***************************** Get Employee list
        $scope.onDeptchange = function (objd) {
            $scope.get_employeelist = [];
            $scope.deptid = objd.HRMD_Id.HRMD_Id;
            var data = {
                "HRMD_Id": $scope.deptid,
                "MIId": objd.HRMD_Id.MI_Id
            };
            apiService.create("ISM_TaskRegisterReport/getemployeelist", data).
                then(function (promise) {
                    if (promise.get_employeelist.length > 0) {
                        $scope.get_employeelist = promise.get_employeelist;
                    }
                    else {
                        $scope.get_employeelist = "";
                        swal("No Record Found ....!!");
                    }
                });
        };
        $scope.togchkbx = function () {
            $scope.checkall = $scope.get_userEmplist.every(function (options) {
                return options.empck;
            });
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };

        //************************** Status List
        $scope.all_checkP = function (ck) {
            $scope.checkallP = ck;
            var toggleStatus = $scope.checkallP;
            angular.forEach($scope.prioritylist, function (st) {
                st.statusck = toggleStatus;
            });
        };
        $scope.togchkbxP = function () {
            $scope.checkallP = $scope.prioritylist.every(function (options) {
                return options.statusck;
            });
        };
        $scope.isOptionsRequiredP = function () {
            if ($scope.prioritycheck === 1) {
                return !$scope.prioritylist.some(function (options) {
                    return options.statusck;
                });
            }

        };


        $scope.all_checkS = function (ck) {
            $scope.checkallS = ck;
            var toggleStatus = $scope.checkallS;
            angular.forEach($scope.get_Status, function (st) {
                st.statusck = toggleStatus;
            });
        };
        $scope.togchkbxS = function () {
            $scope.checkallS = $scope.get_Status.every(function (options) {
                return options.statusck;
            });
        };
        $scope.isOptionsRequiredS = function () {
            return !$scope.get_Status.some(function (options) {
                return options.statusck;
            });
        };
        //*************************** DEVIATION REPORT *********************//        
        $scope.taskReport = function () {
            $scope.prioritycheck = 0;
            if ($scope.myForm.$valid) {
                $scope.taskStatus = [];
                $scope.taskemployee = [];
                $scope.colarrayall = [];
                $scope.arrayClient   = [];
                $scope.searchValueE = "";
                $scope.get_taskregisterReport = "";
                angular.forEach($scope.get_Status, function (sts) {
                    if (sts.statusck === true) {
                        $scope.taskStatus.push({ id: sts.ismmistS_StatusName });
                    }
                });
                angular.forEach($scope.get_userEmplist, function (ep) {
                    if (ep.empck === true) {
                        $scope.taskemployee.push(ep);
                    }
                });

                if ($scope.taskemployee.length > 0) {
                    $scope.SelectionFlag = "2";
                }
                else {
                    $scope.SelectionFlag = "1";
                }
                
                if ($scope.prioritylist.length > 0) {
                    $scope.priorityarray = [];
                    angular.forEach($scope.prioritylist, function (qq) {
                        if (qq.statusck === true) {
                            $scope.priorityarray.push({ HRMPR_Id: qq.hrmpR_Id });
                        }
                    });

                }
                else {
                    $scope.priorityarray = undefined;
                }

                angular.forEach($scope.client_Master, function (comp) {
                    if (comp.selected == true) {
                        $scope.arrayClient.push({ ISMMCLT_Id: comp.ismmclT_Id });
                    }
                });

                    var data = {
                        "TypeFlg": $scope.reporttype,
                        "SelectionFlag": $scope.SelectionFlag,
                        "startdate": new Date($scope.start_Date).toDateString(),
                        "enddate": new Date($scope.end_Date).toDateString(),
                        "HRMD_Id": $scope.deptid,
                        "taskStatus": $scope.taskStatus,
                        "taskemployee": $scope.taskemployee,
                        "priorityarray": $scope.priorityarray,
                        "arrayClient": $scope.arrayClient
                       
                    };
                

                apiService.create("ISM_TaskRegisterReport/gettaskregisterReport", data).then(function (promise) {
                    $scope.get_taskregisterReport = promise.get_taskregisterReport;
                   
                        $scope.employeeid = [];
                        if ($scope.get_taskregisterReport.length > 0) {

                            angular.forEach($scope.get_taskregisterReport, function (dev) {
                                if ($scope.employeeid.length === 0) {
                                    $scope.employeeid.push({ HRME_Id: dev.HRME_Id, employeename: dev.employeename });
                                } else if ($scope.employeeid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.employeeid, function (emp) {
                                        if (emp.HRME_Id === dev.HRME_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.employeeid.push({ HRME_Id: dev.HRME_Id, employeename: dev.employeename });
                                    }
                                }
                            });
                            console.log($scope.employeeid);

                            angular.forEach($scope.employeeid, function (ddd) {
                                $scope.templist = [];
                                angular.forEach($scope.get_taskregisterReport, function (dd) {
                                    if (dd.HRME_Id === ddd.HRME_Id) {
                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.taskdetails = $scope.templist;
                            });
                            var totalcnt = 0;

                            if ($scope.reporttype === "Consolidated") {
                                angular.forEach($scope.get_taskregisterReport, function (tr) {
                                    totalcnt += parseFloat(tr.totalCount);
                                });
                                $scope.totalcount = totalcnt;
                                $scope.colarrayall = [{
                                    title: "SL.NO",
                                    template: "<span class='row-number'></span>", width: "50"
                                },
                                {
                                    name: 'employeename', field: 'employeename', title: 'Employee Name'
                                },
                                {
                                    name: 'ISMTCR_Status', field: 'ISMTCR_Status', title: 'Status'
                                    },
                                    {
                                        name: 'HRMP_Name', field: 'HRMP_Name', title: 'Priority'
                                },
                                {
                                    name: 'totalCount', field: 'totalCount', title: 'Count'
                                }
                                ];
                            }
                            else {
                                $scope.colarrayall = [{
                                    title: "SL.NO",
                                    template: "<span class='row-number'></span>"
                                },
                                {
                                    name: 'employeename', field: 'employeename', title: 'Employee Name'
                                },

                                {
                                    name: 'ISMTCR_TaskNo', field: 'ISMTCR_TaskNo', title: 'Task No.', width: "100"
                                },
                                {
                                    name: 'ISMTCR_Title', field: 'ISMTCR_Title', title: 'Task Title', width: "100"
                                },
                                {
                                    name: 'ISMTCR_Desc', field: 'ISMTCR_Desc', title: 'Task Description', width: "100"
                                },
                                {
                                    name: 'ISMTCR_BugOREnhancementFlg', field: 'ISMTCR_BugOREnhancementFlg', title: 'Type', width: "100"
                                },
                                {
                                    name: 'EffortInHrs', field: 'EffortInHrs', title: 'No Of Hours', width: "100"
                                },
                                {
                                    name: 'ISMTCR_Status', field: 'ISMTCR_Status', title: 'Status', width: "100"
                                },
                                {
                                    name: 'HRMP_Name', field: 'HRMP_Name', title: 'Priority ', width: "100"
                                },
                                {
                                    name: 'ISMTCR_CreationDate', field: 'ISMTCR_CreationDate', title: 'Created Date', width: "130", template: "#= kendo.toString(kendo.parseDate(ISMTCR_CreationDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                                },
                                {
                                    name: 'AssignedBy', field: 'AssignedBy', title: 'Assigned By', width: "100"
                                },
                                {
                                    name: 'StartDate', field: 'StartDate', title: 'Start Date', width: "130", template: "#= kendo.toString(kendo.parseDate(StartDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                                },
                                {
                                    name: 'EndDate', field: 'EndDate', title: 'End Date', width: "130", template: "#= kendo.toString(kendo.parseDate(EndDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                                }
                                ];
                            }

                            $(document).ready(function () {
                                initGridall();
                            });

                            function initGridall() {
                                $('#gridall').empty();
                                $("#gridall").kendoGrid({
                                    toolbar: ["excel", "pdf"],
                                    excel: {
                                        fileName: "TaskRegisterReport.xlsx",
                                        proxyURL: "",
                                        filterable: true,
                                        allPages: true
                                    },
                                    pdf: {
                                        avoidLinks: true,
                                        landscape: true,
                                        repeatHeaders: true,
                                        fileName: "TaskRegisterReport.pdf",
                                        allPages: true
                                    },
                                    dataSource: {
                                        data: $scope.get_taskregisterReport,
                                        pageSize: 10
                                    },
                                    sortable: true,
                                    pageable: true,
                                    groupable: false,
                                    filterable: true,
                                    columnMenu: true,
                                    reorderable: true,
                                    resizable: true,

                                    columns: $scope.colarrayall,
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
                            $scope.get_taskregisterReport = "";
                        }
                    
                    //===================


                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printTR").innerHTML;

            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();

        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.Clear = function () {
            $state.reload();
        };
        //=======client check
        $scope.all_checkCCCC = function () {
            var ismmclT_Id = $scope.ismmclT_Id;
            var count = 0;
            angular.forEach($scope.client_Master, function (itm) {
                itm.selected = ismmclT_Id;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        };
        $scope.togchkbxCCCC = function () {
            $scope.ismmclT_Id = $scope.client_Master.every(function (options) {
                return options.selected;
            });
        };
        $scope.isOptionsRequiredtwo = function () {
            return !$scope.client_Master.some(function (options) {
                return options.selected;
            });
        };
    }
})();

