(function () {
    'use strict';
    angular.module('app').controller('ISM_TaskCreationReportController', ISM_TaskCreationReportController);
    ISM_TaskCreationReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', 'Excel', '$timeout'];
    function ISM_TaskCreationReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter, Excel, $timeout) {

        var paginationformasters = 10;
        $scope.obj = {};
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        $scope.taskeffort = [];
        $scope.tasklist = [];
        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();
        $scope.report = false;
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian; todate
        };
        $scope.onpageload = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.searchValue = "";
            var pageid = 2;
            $scope.flagone = false;
            $scope.flagtwo = false;
            $scope.flagtwos = false;
            $scope.flagones = false;
            apiService.getURI("TADA_Reports/getalldata", pageid).then(function (promise) {
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
                $scope.tasklistnumber();
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

            $scope.DeptdeviationRemarksReport = [];
            $scope.searchValueUEM = '';
            $scope.employeeid = [];
            $scope.checkall = false;
            $scope.get_deviationreport = [];
            $scope.Deptselectiondetails();
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
        $scope.otherdaydatechange = function () {

            $scope.get_Report = null;
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

            apiService.create("TADA_Reports/Deptselectiondetails", data).then(function (promise) {
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
                    $scope.get_Report = null;
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
            apiService.create("TADA_Reports/Desgselectiondetails", data).then(function (promise) {
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
                    $scope.get_Report = null;
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
                $scope.get_Status = [];
                $scope.get_taskdetails = [];
                $scope.get_taskresponse = [];
                if ($scope.tasklist != null && $scope.tasklist.length > 0) {
                    angular.forEach($scope.tasklist, function (d) {
                        if (d.ISMTCR_Id == $scope.obj.ISMTCR_Id.ISMTCR_Id) {

                            $scope.emplist.push({
                                HRME_Id: d.HRME_Id,
                                TTEDDP_ID: d.ISMTCR_Id,
                                TTEDDP_TaskNo: d.TaskNo
                            });
                        }
                      

                    });
                }

                if ($scope.emplist != null && $scope.emplist.length > 0) {
                    data = {
                        "flag": "2",
                        "MulttaskDepartments": $scope.emplist
                    };
                    apiService.create("ISM_TaskRegisterReport/ISM_TaskReport", data).then(function (promise) {

                        if (promise.get_employeelist !== null && promise.get_employeelist.length > 0) {
                            $scope.get_Report = promise.get_employeelist;
                            $scope.get_Status = promise.get_Status;
                            $scope.get_taskdetails = promise.get_taskdetails;
                            $scope.presentCountgrid = $scope.get_Report.length;
                            $scope.get_taskresponse = promise.get_taskclient;
                            if ($scope.get_taskresponse.length > 0) {
                                if ($scope.get_responseattachments.length > 0) {
                                    angular.forEach($scope.get_taskresponse, function (at) {
                                        var slno = 0;
                                        angular.forEach($scope.get_responseattachments, function (atr) {
                                            if (at.ISMTCRRES_Response === atr.ISMTCRRES_Response) {
                                                slno = slno + 1;
                                                atr.slno = slno;
                                            }
                                        });                                    
                                        at.student_rmrks = at.ISMTCRRES_Response.split("\n");
                                    });
                                }
                            }
                        }
                        else {
                            swal("No Record Found");
                            $scope.get_Report = null;
                        }
                    });
                }
                else {
                    swal("Task Number Not Select ")
                }
            }
            else {
                $scope.submitted = true;
            }
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
            var excelname = "";
            var exportHref = "";
            var excelsheetname = "Task Enhancement Time table";
            var reportname = "#printsection";           
            excelname = excelsheetname;
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
            var innerContents = document.getElementById("printsection").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
       
        $scope.tasklistnumber = function () {
            $scope.tasklist = [];
            $scope.emplist = [];
            angular.forEach($scope.get_userEmplist, function (d) {
                if (d.empck === true) {
                    $scope.emplist.push({ HRME_Id: d.HRME_Id });
                }
            });
            if ($scope.emplist != null && $scope.emplist.length > 0) {
                var data = {
                    "MulttaskDepartments": $scope.emplist,
                    "flag": "1",

                }
                apiService.create("ISM_TaskRegisterReport/ISM_TaskRegister", data).then(function (promise) {

                    if (promise.get_employeelist !== null && promise.get_employeelist.length > 0) {
                        $scope.tasklist = promise.get_employeelist;

                    }
                    else {
                        swal("No Record Found !");

                    }
                });
            }
            else {
                swal("Select Atleast One Emolyee !");
            }


        }
    }
})();

