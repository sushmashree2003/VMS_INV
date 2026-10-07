(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_PlannerDetailsReportController', ISM_PlannerDetailsReportController);
    ISM_PlannerDetailsReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function ISM_PlannerDetailsReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache) {
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
        $scope.abcdef = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();
        $scope.depdes = false;
        $scope.newmainarray = [];
        $scope.loaddata = function () {
            $scope.newmainarray = [];
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.itemsPerPage = 1;
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
        $scope.get_plannerdetails = [];
        $scope.approvalReport = function () {
            $scope.plannerid = [];
            $scope.newmainarray = [];
            $scope.get_plannerdetails = [];
            if ($scope.myForm.$valid) {
               
                $scope.temp_Planner = [];
                $scope.searchValue = "";
                $scope.get_approvalreport = "";
                var cuuu = 0;
                angular.forEach($scope.get_Plannerlist, function (ep) {
                    if (ep.plnrck === true) {
                        $scope.temp_Planner.push(ep);
                        cuuu += 1;
                    }
                });
                $scope.arrayuserEmp = [];
                angular.forEach($scope.get_userEmplist, function (em) {
                    if (em.empck === true) {
                        $scope.arrayuserEmp.push(em);
                    }
                });
                if (cuuu<60) {

            
                var data = {
                    arrayuserEmp: $scope.arrayuserEmp,
                    "temp_Planner": $scope.temp_Planner,
                    "approvalstatus": $scope.approved
                };
                apiService.create("ISM_ApprovalReport/getPlannerDReport", data).then(function (promise) {
                    if (promise.get_plannerdetails.length > 0) {
                        $scope.plannerdetails = promise.get_plannerdetails;

                        console.log($scope.temp_Planner);

                        angular.forEach($scope.temp_Planner, function (mm) {

                            $scope.get_plannerdetails = [];
                   


                        var cntt = 0;
                            angular.forEach($scope.plannerdetails, function (pl) {
                                debugger;
                                if (pl.ISMTPL_Id == mm.ismtpL_Id) {

                               
                                cntt += 1;
                                if (cntt == 1) {
                                    $scope.ename = pl.ENAME;
                                }
                                var cls = '';
                                if (pl.ISMTPLTA_PreviousTask != null && pl.ISMTPLTA_PreviousTask == true) {
                                    cls = 'backkk';
                                }

                                    if (pl.ISMTPLAP_Id == null) {

                                        if (pl.ISMTPL_ApprovalFlg == true) {
                                            status = "APPROVED";
                                        }
                                        else {
                                            status = "PENDING";
                                        }
                                    }
                                    else {
                                        if (pl.ISMTPLTA_ApprovalFlg == false) {

                                            status = "REJECTED";
                                        }
                                        else {

                                            status = "APPROVED";
                                        }
                                    }


                                if (pl.ISMTPLC_Id != null && pl.ISMTPLC_Id > 0) {
                                    $scope.get_plannerdetails.push({
                                        ISMTPL_Id: pl.ISMTPL_Id, ISMTCR_Id: pl.ISMTCR_Id, HRMP_Name: pl.HRMP_Name, ISMTCR_BugOREnhancementFlg: pl.ISMTCR_BugOREnhancementFlg, ISMTCR_Title: pl.ISMTCR_Title, ISMTCR_Status: pl.ISMTCR_Status, ISMTCR_TaskNo: pl.ISMTCR_TaskNo, ISMMCLT_ClientName: pl.ISMMCLT_ClientName, ISMTPL_CreatedBy: pl.ISMTPL_CreatedBy, plannedby: pl.plannedby, ISMTPL_PlannerName: pl.ISMTPL_PlannerName, ISMTPLTA_Remarks: pl.ISMTPLTA_Remarks, CreatedDate: pl.CreatedDate, ISMTCR_Desc: pl.ISMTCR_Desc, ISMTPLTA_Status: pl.ISMTPLTA_Status,
                                        ISMTCR_CreationDate: pl.ISMTCR_CreationDate, ISMTPLTA_StartDate: new Date(pl.ISMTPLTA_StartDate), ISMTPLTA_EndDate: new Date(pl.ISMTPLTA_EndDate), ISMTPLTA_EffortInHrs: pl.ISMTPLTA_EffortInHrs,
                                        ISMMTCAT_Id: pl.ISMMTCAT_Id,
                                        ISMMTCAT_TaskCategoryName: pl.ISMMTCAT_TaskCategoryName, ISMMTCAT_TaskPercentage: pl.ISMTPLC_TaskPercentage, ISMMTCAT_DurationFlg: pl.ISMTPLC_DurationFlg, ISMMTCAT_CompulsoryFlg: pl.ISMTPLC_CompulsoryFlg, ISMMTCAT_EachTaskMaxDuration: pl.ISMTPLC_EachTaskMaxDuration,
                                        Flg: pl.ISMTPLTA_PreviousTask,
                                        cls: cls, status: status
                                    });
                                }
                                else {
                                    $scope.get_plannerdetails.push({
                                        ISMTPL_Id: pl.ISMTPL_Id, ISMTCR_Id: pl.ISMTCR_Id, HRMP_Name: pl.HRMP_Name, ISMTCR_BugOREnhancementFlg: pl.ISMTCR_BugOREnhancementFlg, ISMTCR_Title: pl.ISMTCR_Title, ISMTCR_Status: pl.ISMTCR_Status, ISMTCR_TaskNo: pl.ISMTCR_TaskNo, ISMMCLT_ClientName: pl.ISMMCLT_ClientName, ISMTPL_CreatedBy: pl.ISMTPL_CreatedBy, plannedby: pl.plannedby, ISMTPL_PlannerName: pl.ISMTPL_PlannerName, ISMTPLTA_Remarks: pl.ISMTPLTA_Remarks, CreatedDate: pl.CreatedDate, ISMTCR_Desc: pl.ISMTCR_Desc, ISMTPLTA_Status: pl.ISMTPLTA_Status,
                                        ISMTCR_CreationDate: pl.ISMTCR_CreationDate, ISMTPLTA_StartDate: new Date(pl.ISMTPLTA_StartDate), ISMTPLTA_EndDate: new Date(pl.ISMTPLTA_EndDate), ISMTPLTA_EffortInHrs: pl.ISMTPLTA_EffortInHrs,
                                        ISMMTCAT_Id: pl.ISMMTCAT_Id,
                                        ISMMTCAT_TaskCategoryName: pl.ISMMTCAT_TaskCategoryName, ISMMTCAT_TaskPercentage: pl.ISMMTCAT_TaskPercentage, ISMMTCAT_DurationFlg: pl.ISMMTCAT_DurationFlg, ISMMTCAT_CompulsoryFlg: pl.ISMMTCAT_CompulsoryFlg, ISMMTCAT_EachTaskMaxDuration: pl.ISMMTCAT_EachTaskMaxDuration,
                                        Flg: pl.ISMTPLTA_PreviousTask,
                                        cls: cls, status: status
                                    });
                                }
                            }
                        });

                        $scope.categoryidlist1 = [];
                        var snocount = 0;
                        var countsubgrp = 0;
                        angular.forEach($scope.get_plannerdetails, function (sub) {
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

                        angular.forEach($scope.get_plannerdetails, function (ww) {
                            if (ww.Flg == true && ww.Flg != null && ww.status != 'REJECTED') {
                                $scope.overalleffortD = parseFloat($scope.overalleffortD) + parseFloat(ww.ISMTPLTA_EffortInHrs)
                            }

                        })
                        debugger;
                            $scope.overalleffortCP = ((mm.ismtpL_TotalHrs - $scope.overalleffortD) / mm.ismtpL_TotalHrs) * 100;

                            $scope.overalleffortDP = ($scope.overalleffortD / mm.ismtpL_TotalHrs) * 100;


                        angular.forEach($scope.categoryidlist1, function (rr) {
                            var cateffort = 0;
                            var devcateffort = 0;
                            var curper = 0;
                            var devper = 0;
                            angular.forEach($scope.get_plannerdetails, function (ff) {
                                if (ff.ISMMTCAT_Id != null && ff.ISMMTCAT_Id != undefined) {
                                    if (parseInt(rr.ISMMTCAT_Id) == parseInt(ff.ISMMTCAT_Id)) {

                                        if (ff.Flg == true && ff.status != 'REJECTED') {
                                            devcateffort = devcateffort + parseFloat(ff.ISMTPLTA_EffortInHrs);
                                        }
                                        if (ff.status != 'REJECTED') {
                                            cateffort = cateffort + parseFloat(ff.ISMTPLTA_EffortInHrs);
                                        }

                                    }
                                }


                            })

                            if (cateffort != 0) {
                                curper = (parseFloat(cateffort - devcateffort) / cateffort) * 100;
                                devper = (parseFloat(devcateffort) / cateffort) * 100;

                                $scope.finalcategoryidlist.push({
                                    ISMMTCAT_Id: rr.ISMMTCAT_Id,
                                    CNAME: rr.ISMMTCAT_TaskCategoryName,
                                    MINP: rr.ISMMTCAT_TaskPercentage,
                                    TE: cateffort, DE: devcateffort, CWP: curper, DP: devper, flg: rr.ISMMTCAT_CompulsoryFlg
                                })
                            }


                        })

                            console.log($scope.finalcategoryidlist);

                            if ($scope.get_plannerdetails.length>0) {
                                $scope.newmainarray.push({ ISMTPL_Id: mm.ismtpL_Id, PlaNlist: $scope.get_plannerdetails, catlist: $scope.finalcategoryidlist, sdate: mm.ismtpL_StartDate, edate: mm.ismtpL_EndDate, planname: mm.ismtpL_PlannerName, hrs: mm.ismtpL_TotalHrs, overalleffortD: $scope.overalleffortD, overalleffortCP: $scope.overalleffortCP, overalleffortDP: $scope.overalleffortDP });
                            }
                          


                        })


                        console.log($scope.newmainarray);
                    }
                    else {
                        swal("No Record  Found..... !!");
                        $scope.get_plannerdetails = "";
                    }

                });
            }
            else {
                swal('You can select upto 60 Planner at a Time!!!');
            }
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

