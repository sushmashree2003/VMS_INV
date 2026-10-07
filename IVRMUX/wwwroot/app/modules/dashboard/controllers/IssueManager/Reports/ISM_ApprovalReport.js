(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_ApprovalReportController', ISM_ApprovalReportController);
    ISM_ApprovalReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function ISM_ApprovalReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache) {
        var paginationformasters;

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
        $scope.depdes = false;
        $scope.loaddata = function () {
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            $scope.dateflag = 0;
            var pageid = 2;
            apiService.getURI("ISM_ApprovalReport/getdetails", pageid).then(function (promise) {

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

            });
        };


        $scope.all_checkuu = function (ack) {
            $scope.checkalluu = ack;
            var toggleStatus = $scope.checkalluu;
            angular.forEach($scope.get_userEmplist, function (uem) {
                uem.empck = toggleStatus;
            });


            $scope.onEmployeechange();
        };


        $scope.exportToExcel = function () {
            debugger;
            var blob = new Blob([document.getElementById('printP').innerHTML], {
                type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
            });

            saveAs(blob, "Approval Report.xls");
        };


        $scope.togchkbxuu = function () {
            $scope.checkalluu = $scope.get_userEmplist.every(function (options) {
                return options.empck;
            });

            $scope.onEmployeechange();
          
        };
        $scope.all_check = function (ack) {
            $scope.checkall1 = ack;
            var toggleStatus = $scope.checkall1;
            angular.forEach($scope.filterValue3, function (uem) {
                uem.empck = toggleStatus;
            });
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };
        $scope.togchkbx = function () {
            $scope.checkall = $scope.get_userEmplist.every(function (options) {
                return options.empck;
            });
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };

        //=============================for grid check box selection
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
            $scope.checkalluu = false;
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
            $scope.checkalluu = false;
            $scope.get_Plannerlist = [];
            $scope.Desgselectiondetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };


        //$scope.isOptionsRequired = function () {
        //    return !$scope.get_userEmplist.some(function (options) {
        //        return options.empck;
        //    });
        //};

        $scope.searchValueDesg = '';
        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.HRMDES_DesignationName).indexOf(angular.lowercase($scope.searchValueDesg)) >= 0 || angular.lowercase(obj.MI_Name).indexOf(angular.lowercase($scope.searchValueDesg)) >= 0;
        };

        $scope.searchValueDept = '';
        $scope.filterValue4 = function (obj) {
            return angular.lowercase(obj.HRMDC_Name).indexOf(angular.lowercase($scope.searchValueDept)) >= 0;
        };

        $scope.searchValueUEM = '';
        $scope.filterValue2 = function (obj) {
            return angular.lowercase(obj.userEmpName).indexOf(angular.lowercase($scope.searchValueUEM)) >= 0 || angular.lowercase(obj.HRMD_DepartmentName).indexOf(angular.lowercase($scope.searchValueUEM)) >= 0;
        };


        $scope.isOptionsRequired5 = function () {
            return !$scope.get_Plannerlist.some(function (options) {
                return options.plnrck;
            });
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
        //*************************** Desgselectiondetails REPORT *********************//
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
                        //angular.forEach($scope.get_userEmplist, function (uem) {
                        //    uem.empck = true;
                        //});
                        $scope.checkall = true;
                        //$scope.checkalluu = true;
                        $scope.get_Plannerlist = [];
                        $scope.plannerid = [];

                       // $scope.onEmployeechange();
                    }
                }
                else {
                    swal("No Record  Found..... !!");
                }
            });
        };
        //***************************** Date Change
        $scope.onenddatechange = function () {
            //$scope.startdate = new Date($scope.start_Date).toDateString();
            //$scope.enddate = new Date($scope.end_Date).toDateString();
            //$scope.dateflag = 1;
            //$scope.onEmployeechange();

            angular.forEach($scope.get_userEmplist, function (em) {
                em.empck = false;                
            });
            $scope.checkalluu = false;
            $scope.get_Plannerlist = [];
        };
        //***************************** Get Employee list
        $scope.onEmployeechange = function () {
            $scope.plannerid = [];
            $scope.checkall = false;
            $scope.arrayuserEmp = [];
            angular.forEach($scope.get_userEmplist, function (em) {
                if (em.empck === true) {
                    $scope.arrayuserEmp.push(em);
                }
            });
            $scope.startdate = new Date($scope.start_Date).toDateString();
            $scope.enddate = new Date($scope.end_Date).toDateString();
            if ($scope.start_Date !== null && $scope.end_Date) {
                $scope.dateflag = 1;
            }
            else {
                $scope.dateflag = 0;
            }
         
            $scope.get_Plannerlist = [];
            $scope.plannerid = [];
            var data = {};
            if ($scope.dateflag === 1) {
                data = {
                    //"HRME_Id": $scope.obj.HRME_Id.HRME_Id,
                    "startdate": $scope.startdate,
                    "enddate": $scope.enddate,
                    "dateflag": $scope.dateflag,
                    "arrayuserEmp": $scope.arrayuserEmp
                };
            }
            else {
               // $scope.empid = objd.HRME_Id.HRME_Id;
                data = {
                    //"HRME_Id": $scope.empid
                    arrayuserEmp: $scope.arrayuserEmp
                };
            }
            apiService.create("ISM_ApprovalReport/getPlannerlist", data).
                then(function (promise) {
                    if (promise.get_Plannerlist.length > 0) {
                        $scope.get_Plannerlist = promise.get_Plannerlist;
                    }
                    else {
                        $scope.get_Plannerlist = "";
                        swal("Planner's Not  Found ....!!");
                    }
                });
        };
        //************************** Employee List 
        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.get_Plannerlist, function (uem) {
                uem.plnrck = toggleStatus;
            });
        };
        $scope.togchkbx = function () {
            $scope.checkall = $scope.get_Plannerlist.every(function (options) {
                return options.plnrck;
            });
        };

        //*************************** Approval Report *********************//        
        $scope.approvalReport = function () {
            $scope.plannerid = [];
            if ($scope.myForm.$valid) {
                //if ($scope.obj.HRME_Id != null && $scope.obj.HRME_Id != "" && $scope.obj.HRME_Id != undefined)
                //{
                $scope.temp_Planner = [];
                $scope.searchValue = "";
                $scope.get_approvalreport = "";
                angular.forEach($scope.get_Plannerlist, function (ep) {
                    if (ep.plnrck === true) {
                        $scope.temp_Planner.push(ep);
                    }
                });
                $scope.arrayuserEmp = [];
                angular.forEach($scope.get_userEmplist, function (em) {
                    if (em.empck === true) {
                        $scope.arrayuserEmp.push(em);
                    }
                });
                //var data = {
                //    "HRME_Id": $scope.obj.HRME_Id.HRME_Id,
                //    "temp_Planner": $scope.temp_Planner
                //};
                var data = {
                    arrayuserEmp: $scope.arrayuserEmp,
                    "temp_Planner": $scope.temp_Planner,
                    "approvalstatus": $scope.approved
                };
                apiService.create("ISM_ApprovalReport/getapprovalReport", data).then(function (promise) {
                    $scope.plannerid = [];
                    if (promise.get_approvalreport.length > 0) {
                        $scope.get_approvalreport = promise.get_approvalreport;
                        angular.forEach($scope.get_approvalreport, function (dev) {
                            if ($scope.plannerid.length === 0) {
                                $scope.plannerid.push({ ISMTPL_Id: dev.ISMTPL_Id, ISMTPL_PlannerName: dev.ISMTPL_PlannerName, ISMTPL_StartDate: new Date(dev.ISMTPL_StartDate), ISMTPL_EndDate: new Date(dev.ISMTPL_EndDate), plannedby: dev.plannedby, ISMTPLAP_Remarks: dev.ISMTPLAP_Remarks, approvedby: dev.approvedby });
                            } else if ($scope.plannerid.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.plannerid, function (emp) {
                                    if (emp.ISMTPL_Id === dev.ISMTPL_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.plannerid.push({ ISMTPL_Id: dev.ISMTPL_Id, ISMTPL_PlannerName: dev.ISMTPL_PlannerName, ISMTPL_StartDate: new Date(dev.ISMTPL_StartDate), ISMTPL_EndDate: new Date(dev.ISMTPL_EndDate), plannedby: dev.plannedby, ISMTPLAP_Remarks: dev.ISMTPLAP_Remarks, approvedby: dev.approvedby  });
                                }
                            }
                        });

                        angular.forEach($scope.plannerid, function (ddd) {
                            $scope.templist = [];
                            angular.forEach($scope.get_approvalreport, function (dd) {
                                if (dd.ISMTPL_Id === ddd.ISMTPL_Id) {
                                    $scope.templist.push(dd);
                                }
                            });
                            ddd.plannerdetails = $scope.templist;
                        });
                    }
                    else {
                        swal("No Records Found!!");
                    }

                });
            //}
            //else {
            //    swal('Select Employee');
            //}
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printP").innerHTML;
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

            $scope.departmentselectedAll = false;
            $scope.designationselectedAll = false;
            $scope.get_approvalreport = "";
            $scope.get_Plannerlist = "";
            $scope.checkall = "";
            $scope.start_Date = "";
            $scope.end_Date = "";
            $scope.obj.HRME_Id = "";
            $scope.loaddata();
        };
        $scope.clearreport = function () {
            $scope.get_approvalreport = [];
            $scope.plannerid = [];
        };

    }
})();

