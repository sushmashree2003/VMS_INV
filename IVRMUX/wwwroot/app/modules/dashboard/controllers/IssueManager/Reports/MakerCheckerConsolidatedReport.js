(function () {
    'use strict';
    angular.module('app').controller('MackerCheakerReportController', MackerCheakerReportController);
    MackerCheakerReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', 'Excel', '$timeout'];
    function MackerCheakerReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter, Excel, $timeout) {

        $scope.completed = true;
        $scope.pending = false;
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
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();
        $scope.start_Date = new Date();
        $scope.end_Date = new Date();


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
            return angular.lowercase(obj.userEmpName).indexOf(angular.lowercase($scope.searchValueUEM)) >= 0 || angular.lowercase(obj.userEmpName).indexOf(angular.lowercase($scope.searchValueUEM)) >= 0;
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
         
            var pageid = 2;
            apiService.getURI("ISM_TaskRegisterReport/getconsuldatedetails", pageid).then(function (promise) {
                
                $scope.get_companyDeptist = promise.get_companyDeptist;
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


        
        //*************************** DEVIATION REPORT *********************//        
        $scope.taskReport = function () {
            $scope.prioritycheck = 0;
            if ($scope.myForm.$valid) {
               
                $scope.taskemployee = [];
                $scope.colarrayall = [];
                $scope.searchValueE = "";
                $scope.get_taskregisterReport = "";
               
                angular.forEach($scope.get_userEmplist, function (ep) {
                    if (ep.empck === true) {
                        $scope.taskemployee.push(ep);
                    }
                });
                $scope.sdate = $filter('date')($scope.start_Date, 'yyyy-MM-dd');
                $scope.edate = $filter('date')($scope.end_Date, 'yyyy-MM-dd'); 
                

                var data = {
                    "completed": $scope.completed,
                    "pending": $scope.pending,
                    "startdate_new": $scope.sdate,
                    "enddate_new": $scope.edate,
                     "taskemployee": $scope.taskemployee,
                   

                };


                apiService.create("ISM_TaskRegisterReport/getmakercheckerReport", data).then(function (promise) {
                    $scope.get_makercheckerstslist = promise.get_makercheckerstslist;

                    $scope.taskid5 = [];
                    angular.forEach($scope.get_makercheckerstslist, function (dev) {
                        if ($scope.taskid5.length === 0) {
                            $scope.taskid5.push({ HRME_Id: dev.HRME_Id, employeename: dev.employeename });
                        } else if ($scope.taskid5.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.taskid5, function (emp) {
                                if (emp.HRME_Id === dev.HRME_Id) {
                                    intcount += 1;
                                }
                            });

                            if (intcount === 0) {
                                $scope.taskid5.push({ HRME_Id: dev.HRME_Id, employeename: dev.employeename });
                            }
                        }
                    });

                    angular.forEach($scope.taskid5, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.get_makercheckerstslist, function (dd) {
                            if (dd.HRME_Id === ddd.HRME_Id) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.get_makercheckerstslist = $scope.templist;
                    });


                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.Viewclick = function (ww) {
            $scope.sdate1 = $filter('date')(ww.DRDate,'yyyy-MM-dd');
            var data = {
                "HRME_Id": ww.HRME_Id,
                "startdate_new": $scope.sdate1,
                "flag": ww.DRDaily_Status,
            }
            apiService.create("ISM_TaskRegisterReport/getmakercheckerview", data).then(function (promise) {
                $scope.get_makercheckerviewlist = promise.get_makercheckerviewlist;
                $scope.taskid6 = [];
                angular.forEach($scope.get_makercheckerviewlist, function (dev) {
                    if ($scope.taskid6.length === 0) {
                        $scope.taskid6.push({ HRME_Id: dev.HRME_Id, employeename: dev.employeename });
                    } else if ($scope.taskid6.length > 0) {
                        var intcount = 0;
                        angular.forEach($scope.taskid6, function (emp) {
                            if (emp.HRME_Id === dev.HRME_Id) {
                                intcount += 1;
                            }
                        });

                        if (intcount === 0) {
                            $scope.taskid6.push({ HRME_Id: dev.HRME_Id, employeename: dev.employeename });
                        }
                    }
                });

                angular.forEach($scope.taskid6, function (ddd) {
                    $scope.templist = [];
                    angular.forEach($scope.get_makercheckerviewlist, function (dd) {
                        if (dd.HRME_Id === ddd.HRME_Id) {
                            $scope.templist.push(dd);
                        }
                    });
                    ddd.get_makercheckerviewlist = $scope.templist;
                });
                $('#myModalViewPlanner').modal('show');

            });
        }
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
        $scope.printData1 = function () {
            var innerContents = "";
            innerContents = document.getElementById("myModalViewPlanner").innerHTML;

            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();

        };
               
        $scope.printexcel = function (table) {
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };

        $scope.printexcel1 = function (table) {
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };

        $scope.ngclickstatus = function () {
            if ($scope.completed === 1 && $scope.pending === 0) {
                $scope.completed = 1;
                $scope.pending = 0;
            }
            else if ($scope.completed === 0 && $scope.pending === 1) {
                $scope.completed = 0;
                $scope.pending = 1;
            }
            if ($scope.completed === 1 && $scope.pending === 1) {
                $scope.completed = 1;
                $scope.pending = 1;
            }
            if ($scope.completed === 0 && $scope.pending === 0) {
                $scope.completed = 1;
                $scope.pending = 0;
            }
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.Clear = function () {
            $state.reload();
        };

    }
})();

