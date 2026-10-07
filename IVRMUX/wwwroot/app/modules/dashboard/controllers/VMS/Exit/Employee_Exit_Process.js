(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmployeeExit_ProcessController', employeeExitProcessController)
    employeeExitProcessController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$stateParams', 'Excel', '$timeout', '$q']
    function employeeExitProcessController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $stateParams, Excel, $timeout, $q) {
        //======================================================
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.ISMRESG_FFTotalAmount = 0;
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }



        $scope.currentdate = new Date();
        $scope.ISMRESG_NoticePeriod = 0;
        $scope.imgname = logopath;
        $scope.dagameflag = false;
        $scope.chequerequired = false;
        $scope.obj = {};
        $scope.doc_list2 = [];
        $scope.hidetextbox = false;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };
        $scope.searchValue = '';
        $scope.searchValue1 = '';
        $scope.searchValue2 = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.employeename)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        };
        $scope.filterValue1 = function (obj1) {
            return (angular.lowercase(obj1.employeename)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        };
        $scope.filterValue2 = function (obj2) {
            return (angular.lowercase(obj2.employeename)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        };

        $scope.grid_flag = false;
        //priview document

        //==============================add no of days==============



        $scope.relieving_days = function () {
            $scope.reliving_date = new Date($scope.ISMRESG_ResignationDate.getFullYear(), $scope.ISMRESG_ResignationDate.getMonth(), $scope.ISMRESG_ResignationDate.getDate() + 90);
            $scope.Tentative_Leaving_Date = new Date($scope.reliving_date);

            if ($scope.ISMRESG_ResignationDate != undefined && $scope.ISMRESG_ResignationDate != "") {
                $scope.ISMRESG_ResignationDate = new Date($scope.ISMRESG_ResignationDate).toDateString();
            }
            else {
                $scope.ISMRESG_ResignationDate = "";
            }
        };

        //===============================AllDataLoad======================
        $scope.AllDataLoad = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = paginationformasters;
            $scope.currentPage2 = 1;
            $scope.itemsPerPage2 = paginationformasters;
            var pageid = 2;
            apiService.getURI("Exit_Employee/Load_all_data", pageid).then(function (promise) {
                $scope.employee_list_dd = promise.employee_list_dd;
                $scope.exit_employee_list = promise.exit_employee_list;
                $scope.relieving_emp_dd = promise.relieving_emp_dd;
                $scope.relieving_emp_dd1 = promise.relieving_emp_dd1;
                $scope.resignation_emp_dd = promise.resignation_emp_dd;
                $scope.relieving_check_list = promise.relieving_check_list;
                $scope.exit_print_list = promise.exit_print_list;
                $scope.presentCountgrid = $scope.exit_employee_list.length;
                $scope.presentCountgrid2 = $scope.exit_print_list.length;

                $scope.ResignationapprovalLevel = promise.resignationapprovalLevel;
                $scope.RelivingapprovalLevel = promise.relivingapprovalLevel;

                $scope.departmentCodeName = promise.departmentCodeName;
                if (promise.releavingapprovalGrid.length > 0 && promise.releavingapprovalGrid != null) {
                    $scope.releavingapprovalGrid = promise.releavingapprovalGrid;
                    $scope.presentCountgrid2 = $scope.releavingapprovalGrid.length;
                }
                if (promise.resignationapprovalGrid.length > 0) {
                    $scope.resignationapprovalGrid = promise.resignationapprovalGrid;
                    $scope.presentCountgrid1 = $scope.resignationapprovalGrid.length;
                }

            });
        };

        $scope.submitted = false;
        $scope.ApproveRejectdata_td = function (status) {
            $scope.submitted = true;

            var relievingdate = $scope.ISMRESG_RelievingDate == null ? "" : $filter('date')($scope.ISMRESG_RelievingDate, "yyyy-MM-dd");
            if ($scope.myForm.$valid) {
                var data = {
                    "ISMRESG_Id": $scope.ismresgmrEId,
                    "HRME_Id": $scope.hrmE_Id,
                    "ISMRESGAB_Remarks": $scope.CurrentISMRESGAB_Remarks,
                    "ISMRESGAB_RelievingDate": relievingdate,
                    "ISMRESGAB_AppRejFlg": status,
                    "ISMRESG_Flg": "Resignation"
                };
                apiService.create("Exit_Employee/ExitApproveal", data).then(function (promise) {
                    if (promise.returnval == "Approved") {
                        swal('Record Approved Successfully');
                    }
                    else if (promise.returnval == "Rejected") {
                        swal('Record Rejected Successfully');
                    }
                    else if (promise.returnval == "NotAdd") {
                        swal('Record  Saved Successfully');
                        // swal('Record Not Saved Successfully');
                    }
                    else {
                        swal('Operation Failed!');
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.RelievingApproveRejectdata_td = function (status) {


            $scope.submitted = true;
            $scope.damagerecord = [];
            if ($scope.damagesinsert != undefined && $scope.damagesinsert != null && $scope.damagesinsert.length > 1) {
                angular.forEach($scope.damagesinsert, function (emp) {
                    if (emp.dmgAmount != null || emp.dmgAmount.length > 0) {
                        $scope.damagerecord.push({ dmgRemarks: emp.dmgRemarks, dmgAmount: emp.dmgAmount });
                    }
                });
            }
            if ($scope.dagameflag == true && $scope.damagerecord == 0) {
                swal("Kindly Enter Damage Amount Or UnCheck The Damage Flag")
            }

            var relievingdate = $scope.ISMRESG_RelievingDate == null ? "" : $filter('date')($scope.ISMRESG_RelievingDate, "yyyy-MM-dd");
            if ($scope.myForm.$valid) {
                var data = {
                    "ISMRESG_Id": $scope.ismresgmrEId,
                    "HRME_Id": $scope.hrmE_Id,
                    "ISMRESGAB_Remarks": $scope.CurrentISMRESGAB_Remarks,
                    "ISMRESGAB_RelievingDate": relievingdate,
                    "ISMRESGAB_AppRejFlg": status,
                    "ISMRESG_Flg": "Relieving",
                    damagerecord: $scope.damagerecord
                };
                apiService.create("Exit_Employee/ExitApproveal", data).then(function (promise) {
                    if (promise.returnval == "Approved") {
                        swal('Record Approved Successfully');
                    }
                    else if (promise.returnval == "Rejected") {
                        swal('Record Rejected Successfully');
                    }
                    else if (promise.returnval == "NotAdd") {
                        // swal('Record Not Saved Successfully');
                        swal('Record  Saved Successfully');
                    }
                    else {
                        swal('Operation Failed!');
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.Clearid1 = function () {
            // $state.reload();
            $scope.hrmE_Id2 = '';
            $scope.ismresgmrEId2 = -1;
            $scope.AllDataLoad();

            $scope.obj.hrmE_Id = 0
            $scope.ismcertreQ_AppRejFlag = null;
            $scope.DateOfJoining = "";
            $scope.HRME_Photo = "";
            $scope.HRME_LeftFlag = "";
            $scope.HRME_DOL = "";
            $scope.HRME_LeavingReason = "";
            $scope.Department = "";
            $scope.Designation = "";
            $scope.RelievingDate = "";
            $scope.ResignationDate = "";
            $scope.ismresgmrEId = "";
            $scope.ismresgmrEId2 = 0;
            $scope.mI_Name = "";
            $scope.ismresgmrE_ResignationReasons = "";
            $scope.ismresG_Remarks = "";
            $scope.employeename = "";
            $scope.ismresG_RegRelFlag = "";
            $scope.ResignationapprovalLevel = "";
            $scope.hrmE_Id = "";
            $scope.ISMRESGAB_Remarks = "";
            $scope.ISMRESG_RelievingDate = "";
            $scope.documentList = [];
            $scope.ShowApprovedDetails = false;
        };
        $scope.Clearid2 = function () {
            // $state.reload();
            $scope.ismresgmrEId1 = '';
            $scope.searchValue2 = '';
            $scope.ismresgmrEId3 = -1;
            $scope.AllDataLoad();
        };
        $scope.view_photo = function (ee) {
            var id = ee.ISMRESG_Id;
            apiService.getURI("Exit_Employee/download_doc", id).then(function (promise) {
                $scope.download_photo = promise.download_photo;
            });
        };
        var imagedownload = "";
        var docname = "";




        $scope.downloaddirectimage = function (data, idd) {

            var studentreg = idd;

            $scope.imagedownload = data;
            imagedownload = data;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg + '.jpg'
                    })[0].click();
                });
        };
        $scope.ShowApprovedDetails = false;
        //=======================get data===============
        $scope.hrmE_Id = 0;
        $scope.GetAllRelData = function (data) {
            $scope.ShowApprovedDetails = false;
            var getid = data.ismresG_Id;
            apiService.getURI("Exit_Employee/GetAllRelData", getid).then(function (promise) {
                $scope.ismcertreQ_AppRejFlag = promise.getrelievingchecklist[0].ISMCERTREQ_AppRejFlag;
                $scope.DateOfJoining = promise.getrelievingchecklist[0].hrmE_DOJ;
                $scope.HRME_Photo = promise.getrelievingchecklist[0].HRME_Photo;
                $scope.HRME_LeftFlag = promise.getrelievingchecklist[0].HRME_LeftFlag;
                $scope.HRME_DOL = promise.getrelievingchecklist[0].HRME_DOL;
                $scope.HRME_LeavingReason = promise.getrelievingchecklist[0].HRME_LeavingReason;
                $scope.Department = promise.getrelievingchecklist[0].hrmD_DepartmentName;
                $scope.Designation = promise.getrelievingchecklist[0].hrmdeS_DesignationName;
                $scope.RelievingDate = new Date(promise.getrelievingchecklist[0].ismresG_TentativeLeavingDate);
                $scope.ResignationDate = new Date(promise.getrelievingchecklist[0].ISMRESG_ResignationDate);
                $scope.ismresgmrEId = promise.getrelievingchecklist[0].ISMRESG_Id;
                $scope.ismresgmrEId2 = promise.getrelievingchecklist[0].ISMRESG_Id;
                $scope.mI_Name = promise.getrelievingchecklist[0].mI_Name;
                $scope.ismresgmrE_ResignationReasons = promise.getrelievingchecklist[0].ismresgmrE_ResignationReasons;
                $scope.ismresG_Remarks = promise.getrelievingchecklist[0].ismresG_Remarks;
                $scope.employeename = promise.getrelievingchecklist[0].employeename;
                $scope.MI_Id = promise.mI_Id;
                $scope.ismresG_RegRelFlag = promise.getrelievingchecklist[0].ismresG_RegRelFlag;
                $scope.ResignationapprovalLevel = promise.getrelievingchecklist[0].SanctionLevelNo;
                $scope.hrmE_Id = promise.getrelievingchecklist[0].hrmE_Id;
                $scope.ISMRESGAB_Remarks = promise.getrelievingchecklist[0].ISMRESGAB_Remarks;
                $scope.ISMRESG_RelievingDate = new Date(promise.getrelievingchecklist[0].ismresG_RelievingDate);
                $scope.documentList = promise.doc_list;
                $scope.HRME_EmailId = promise.getrelievingchecklist[0].HRME_EmailId;
                $scope.HRMEMNO_MobileNo = promise.getrelievingchecklist[0].HRMEMNO_MobileNo;

                $scope.ISMRESG_FFTotalAmount = promise.getrelievingchecklist[0].ISMRESG_FFTotalAmount;
                if ($scope.ISMRESG_FFTotalAmount == null) {
                    $scope.ISMRESG_FFTotalAmount = 0;
                }
                if (promise.pendingsalarydetails != null && promise.pendingsalarydetails.length > 0) {

                    $scope.earningheadlist = promise.earningdetails;
                    $scope.deductionheadlist = promise.dectuiondetails;
                    $scope.employeeSalaryslipDetails = promise.pendingsalarydetails;
                    $scope.earnlen = $scope.earningheadlist.length;
                    $scope.dedlen = $scope.deductionheadlist.length;


                    angular.forEach($scope.earningheadlist, function (headresult) {
                        headresult.netamount = 0;
                    });

                    angular.forEach($scope.deductionheadlist, function (headresult) {
                        headresult.netamount = 0;
                    });

                    $scope.TotalDeduction = 0;
                    $scope.TotalEarning = 0;
                    $scope.TotalPayable = 0;

                    angular.forEach($scope.employeeSalaryslipDetails, function (result) {
                        angular.forEach($scope.earningheadlist, function (headresult) {
                            if (headresult.hrmeD_Name == result + headresult.hrmeD_Name) {
                                headresult.netamount = parseFloat(headresult.netamount + result);
                            }
                        });

                        angular.forEach($scope.deductionheadlist, function (headresult) {
                            if (headresult.hrmeD_Name == result) {
                                headresult.netamount = parseFloat(headresult.netamount + result);
                            }
                        });


                        $scope.TotalDeduction = $scope.TotalDeduction + result.Totaldeduction;
                        $scope.TotalEarning = $scope.TotalEarning + result.TotalEarnings;
                        $scope.TotalPayable = $scope.TotalPayable + result.TotalPayable;






                    });

                };

                if (promise.ratingarray != null && promise.ratingarray.length > 0) {
                    $scope.MAXVALUE = promise.ratingarray[0].MAXVALUE;
                    $scope.OVERALLRATING = promise.ratingarray[0].OVERALLRATING;
                }
                if (promise.prioritylist != null && promise.prioritylist.length > 0) {
                    $scope.prioritylist = promise.prioritylist;
                }



            });
        }





        $scope.GetAllRelDataGrid = function (data) {
            var getid = data.ISMRESG_Id;
            apiService.getURI("Exit_Employee/GetAllRelData", getid).then(function (promise) {
                $scope.ShowApprovedDetails = true;
                $scope.ismcertreQ_AppRejFlag = promise.getrelievingchecklist[0].ISMCERTREQ_AppRejFlag;
                $scope.DateOfJoining = promise.getrelievingchecklist[0].hrmE_DOJ;
                $scope.HRME_Photo = promise.getrelievingchecklist[0].HRME_Photo;
                $scope.HRME_LeftFlag = promise.getrelievingchecklist[0].HRME_LeftFlag;
                $scope.HRME_DOL = promise.getrelievingchecklist[0].HRME_DOL;
                $scope.HRME_LeavingReason = promise.getrelievingchecklist[0].HRME_LeavingReason;
                $scope.Department = promise.getrelievingchecklist[0].hrmD_DepartmentName;
                $scope.Designation = promise.getrelievingchecklist[0].hrmdeS_DesignationName;
                $scope.RelievingDate = new Date(promise.getrelievingchecklist[0].ismresG_TentativeLeavingDate);
                $scope.ResignationDate = new Date(promise.getrelievingchecklist[0].ISMRESG_ResignationDate);
                $scope.ismresgmrEId = promise.getrelievingchecklist[0].ISMRESG_Id;
                $scope.ismresgmrEId2 = promise.getrelievingchecklist[0].ismresG_Id;
                $scope.mI_Name = promise.getrelievingchecklist[0].mI_Name;
                $scope.ismresgmrE_ResignationReasons = promise.getrelievingchecklist[0].ismresgmrE_ResignationReasons;
                $scope.ismresG_Remarks = promise.getrelievingchecklist[0].ismresG_Remarks;
                $scope.employeename = promise.getrelievingchecklist[0].employeename;
                $scope.MI_Id = promise.mI_Id;
                $scope.ismresG_RegRelFlag = promise.getrelievingchecklist[0].ismresG_RegRelFlag;
                $scope.ResignationapprovalLevel = promise.getrelievingchecklist[0].SanctionLevelNo;
                $scope.hrmE_Id = promise.getrelievingchecklist[0].hrmE_Id;
                $scope.HRME_EmailId = promise.getrelievingchecklist[0].HRME_EmailId;
                $scope.HRMEMNO_MobileNo = promise.getrelievingchecklist[0].HRMEMNO_MobileNo;
                $scope.ISMRESGAB_Remarks = promise.getrelievingchecklist[0].ISMRESGAB_Remarks;
                $scope.ISMRESG_RelievingDate = new Date(promise.getrelievingchecklist[0].ismresG_RelievingDate);
                $scope.documentList = promise.doc_list;
                $scope.ISMRESG_FFTotalAmount = promise.getrelievingchecklist[0].ISMRESG_FFTotalAmount;
                if ($scope.ISMRESG_FFTotalAmount == null) {
                    $scope.ISMRESG_FFTotalAmount = 0;
                }
                if (promise.pendingsalarydetails != null && promise.pendingsalarydetails.length > 0) {

                    $scope.earningheadlist = promise.earningdetails;
                    $scope.deductionheadlist = promise.dectuiondetails;
                    $scope.employeeSalaryslipDetails = promise.pendingsalarydetails;
                    $scope.earnlen = $scope.earningheadlist.length;
                    $scope.dedlen = $scope.deductionheadlist.length;


                    angular.forEach($scope.earningheadlist, function (headresult) {
                        headresult.netamount = 0;
                    });

                    angular.forEach($scope.deductionheadlist, function (headresult) {
                        headresult.netamount = 0;
                    });

                    $scope.TotalDeduction = 0;
                    $scope.TotalEarning = 0;
                    $scope.TotalPayable = 0;

                    angular.forEach($scope.employeeSalaryslipDetails, function (result) {
                        angular.forEach($scope.earningheadlist, function (headresult) {
                            if (headresult.hrmeD_Name == result + headresult.hrmeD_Name) {
                                headresult.netamount = parseFloat(headresult.netamount + result);
                            }
                        });

                        angular.forEach($scope.deductionheadlist, function (headresult) {
                            if (headresult.hrmeD_Name == result) {
                                headresult.netamount = parseFloat(headresult.netamount + result);
                            }
                        });


                        $scope.TotalDeduction = $scope.TotalDeduction + result.Totaldeduction;
                        $scope.TotalEarning = $scope.TotalEarning + result.TotalEarnings;
                        $scope.TotalPayable = $scope.TotalPayable + result.TotalPayable;






                    });

                };

                if (promise.ratingarray != null && promise.ratingarray.length > 0) {
                    $scope.MAXVALUE = promise.ratingarray[0].MAXVALUE;
                    $scope.OVERALLRATING = promise.ratingarray[0].OVERALLRATING;
                }
                if (promise.prioritylist != null && promise.prioritylist.length > 0) {
                    $scope.prioritylist = promise.prioritylist;
                }



            });
        }


        $scope.displaybutton = false;

        $scope.GetAllRelData1 = function (hrmE_Id1) {
            var data = {
                "HRME_Id": hrmE_Id1,
            };
            apiService.create("Exit_Employee/HR_EmployeeDetails", data).then(function (promise) {
                $scope.RefNo = "";
                $scope.companyname = "";
                $scope.companyaddress = "";

                if (promise.managmentempdetails != null && promise.managmentempdetails.length > 0) {
                    $scope.managmentempdetails = promise.managmentempdetails;
                    $scope.employeeTL = promise.managmentempdetails[0].Deptheadname;
                    $scope.teamleadEmailId = promise.managmentempdetails[0].Deptheadmailid;
                    $scope.employeeManager = promise.managmentempdetails[0].Hrname;
                    $scope.mngrEmailId = promise.managmentempdetails[0].Hrmailid;
                    $scope.HRMDES_DesignationName = promise.managmentempdetails[0].HRMDES_DesignationName;
                    $scope.HRMD_DepartmentName = promise.managmentempdetails[0].HRMD_DepartmentName;

                    $scope.HRME_DOJ = new Date(promise.managmentempdetails[0].HRME_DOJ);
                    $scope.HRME_DOJ = new Date(promise.managmentempdetails[0].HRME_DOL);
                }

                if (promise.employeedata != null && promise.employeedata.length > 0) {
                    $scope.DateOfJoining = promise.employeedata[0].HRME_DOJ;
                    $scope.Department = promise.employeedata[0].HRMD_DepartmentName;
                    $scope.Designation = promise.employeedata[0].HRMDES_DesignationName;
                    $scope.mI_Name = promise.employeedata[0].MI_Name;
                    //  $scope.ismresG_Remarks = promise.employeedata[0].ismresG_Remarks;
                    $scope.employeename = promise.employeedata[0].employeename;
                    $scope.MI_Id = promise.mI_Id;
                    //$scope.ismresG_RegRelFlag = promise.employeedata[0].ismresG_RegRelFlag;
                    $scope.hrmE_Id = promise.employeedata[0].hrmE_Id;
                    $scope.HRME_LeftFlag = promise.employeedata[0].HRME_LeftFlag;
                    $scope.HRME_DOL = promise.employeedata[0].HRME_DOL;
                    $scope.HRME_LeavingReason = promise.employeedata[0].HRME_LeavingReason;
                    $scope.HRME_Photo = promise.employeedata[0].HRME_Photo;
                    $scope.ismresgmrEId3 = 1;
                }

                $scope.displaybutton = true;
            });
        }


        //==approvar Remarks===
        $scope.approverRemarks = function (ismresgmrEId) {
            var data = {
                "ISMRESG_Id": ismresgmrEId
            };
            apiService.create("Exit_Employee/approverRemark", data).then(function (promise) {
                if (promise.approvarremarks.length > 0) {
                    $scope.approvarremarks = promise.approvarremarks;
                }
            });
        };



        //========================save/edit=======================


        $scope.submitted = false;
        $scope.Approvedata_td = function () {
            var mm = $scope.dddd;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ISMRESG_Id": $scope.ismresgId_EDIT,
                    "HRME_Id": $scope.hrmE_Id,
                    "ISMRESG_ResignationDate": $scope.ISMRESG_ResignationDate,
                    "ISMRESGMRE_Id": $scope.ismresgmrE_Id,
                    "ISMRESG_NoticePeriod": $scope.ISMRESG_NoticePeriod,
                    "ISMRESG_TentativeLeavingDate": $scope.Tentative_Leaving_Date,
                    "ISMRESG_Remarks": $scope.ISMRESG_Remarks
                };
                apiService.create("Exit_Employee/Exit_empl_SaveEdit", data).then(function (promise) {
                    if (promise.returnval == "Add") {
                        swal('Record Saved Successfully');
                    }
                    else if (promise.returnval == "Update") {
                        swal('Record Updated Successfully');
                    }
                    else if (promise.returnval == "Exist") {
                        swal('Record Already Exist');
                    }
                    else {
                        swal('Operation Failed!');
                    }
                    $state.reload();
                });
            }
            else {

                $scope.submitted = true;
            }
        };

        //========================Edit=============================
        $scope.update_AccRej = function () {
            $scope.submittedapp = false;
            if ($scope.myFormapp.$valid) {
                var data = {
                    "ISMRESG_MgmtApprRejFlg": $scope.ISMRESG_MgmtApprRejFlg,
                    "ISMRESG_AccRejDate": $scope.ISMRESG_AccRejDate,
                    "ISMRESG_ManagementRemarks": $scope.ISMRESG_ManagementRemarks,
                    "ISMRESG_Id": $scope.ismresgId
                };
                apiService.create("Exit_Employee/Exit_empl_AccRej", data).then(function (promise) {
                    if (promise.returnval == "Update") {
                        swal('Record Updated Successfully');
                    }
                    else {
                        swal('Operation Failed!');
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submittedapp = true;
            }
        };
        $scope.c_approve_new = function (ss) {
            var id = ss.ismresG_Id;
            apiService.getURI("Exit_Employee/c_approve_new", id).then(function (promise) {
                if (promise.returnval === "Duplicate") {
                    swal("Already Resignation Letter Generated");
                    $state.reload();
                }
                else {
                    $scope.ismresgId = promise.ismresG_Id1;
                }
            });
        };

        $scope.c_approve = function (ss) {
            var id = ss.ismresG_Id;
            apiService.getURI("Exit_Employee/c_approve_new", id).then(function (promise) {
                if (promise.returnval === "Duplicate") {
                    swal("Already Resignation Letter Generated");
                    $state.reload();
                    $("#myModalgetvealuationlist").hide();

                }
                else {
                    $scope.ismresgId = promise.ismresG_Id1;
                    $("#myModalgetvealuationlist").show();

                }
            });

        };
        //=================\
        $scope.submitted1 = false;
        $scope.Savedata_td = function () {

            $scope.doc_list3 = [];
            //$scope.doc_list3 = $scope.doc_list;
            $scope.submitted1 = true;
            if ($scope.myForm1.$valid) {
                angular.forEach($scope.documentList, function (emp) {
                    if (emp.selectedd2 === true || emp.document_Path != null) {
                        $scope.doc_list2.push({ document_Path: emp.document_Path, ISMRESGCL_FileName: emp.ismresgmcL_CheckListName, ISMRESGMCL_Id: emp.ismresgmcL_Id });
                    }
                });
                var data = {
                    "ISMRESG_Id": $scope.ismresgmrEId,
                    doc_list2: $scope.doc_list2
                };

                apiService.create("Exit_Employee/Savedata_td", data).then(function (promise) {
                    if (promise.returnval = "Add") {
                        swal('Record Saved Successfully');
                    }
                    else if (promise.returnval = "Update") {
                        swal('Record Updated Successfully');
                    }
                    else {
                        swal('Operation Failed!');
                    }
                    $scope.hrmE_Id2 = '';
                    $scope.ismresgmrEId2 = -1;
                    $scope.AllDataLoad();
                });
            }
            else {
                $scope.submitted1 = true;
            }
        };
        //===========================================

        $scope.submitted1 = false;
        $scope.generateff = function () {
            var data = {
                "ISMRESG_Id": $scope.ismresgmrEId,
                "totalffamount": $scope.totalpayable
            };

            apiService.create("Exit_Employee/DamageSave", data).then(function (promise) {
                if (promise.ffdetailsarray != null && promise.ffdetailsarray.length > 0) {
                    $scope.MI_Id = promise.mI_Id;
                    $scope.ISMRESG_FFTotalAmount = promise.ffdetailsarray[0].ismresG_FFTotalAmount;
                    if ($scope.ISMRESG_FFTotalAmount == null) {
                        $scope.ISMRESG_FFTotalAmount = 0;
                    }
                    if ($scope.obj.HRMBD_Id > 0) {
                        var data = {
                            "MI_Id": $scope.MI_Id,
                            "VPAYVOU_PaymentDate": $scope.currentdate,
                            "VPAYVOU_AppliedAmount": $scope.totalpayable,
                            "VPAYVOU_Remarks": $scope.employeename + " Full And Final Settlement.",
                            "HRMBD_Id": $scope.obj.HRMBD_Id,
                            "VPAYVOU_ToSupplierFlg": 1,
                            "VPAYVOU_SupplierFlg": 0,
                            "VPAYVOU_SelfPayFlg": 0,
                            "VPAYVOU_PaymentTo": $scope.employeename,
                            "VPAYVOU_PaymentMode": "Cheque",
                        };

                        apiService.create("CheckBookEntry/savedata", data).then(function (promise) {
                            if (promise.msgval == "save") {
                                swal("Cheque Genarated Successfully");

                            }
                        });
                    }


                    swal("Full And Final Settlement Is Generated Successfully")
                }
            });

        };


        $scope.fullandfinalsettlement = function () {

            $scope.pendingsalarydavailable = true;
            $scope.leaveencashmentavailable = true;
            $scope.gratuitydetailsavailable = true;
            $scope.damageavailable = true;
            $scope.tadadetailsavailable = true;
            $scope.loansavailable = true;

            var data = {
                "HRME_Id": $scope.hrmE_Id,
                "ISMRESG_Id": $scope.ismresgmrEId
            }
            apiService.create("Exit_Employee/FANDFSettlement", data).then(function (promise) {
                $('#fullandfinalsettlement').modal('show');

                $scope.totalenchashedamt = 0;
                if (promise.bankdetails != null && promise.bankdetails.length > 0) {
                    $scope.bankdetails = promise.bankdetails
                }

                $scope.totalenchashedamt = 0;
                if (promise.leaveEnchashmentData != null && promise.leaveEnchashmentData.length > 0) {
                    $scope.LeaveEnchashmentData = promise.leaveEnchashmentData

                    angular.forEach($scope.LeaveEnchashmentData, function (itm) {
                        $scope.totalenchashedamt = $scope.totalenchashedamt + itm.Encash;
                    });
                }

                $scope.graduityamount = 0;
                if (promise.gratuitydetails != null && promise.gratuitydetails.length > 0) {

                    $scope.gratuitydetails = promise.gratuitydetails;
                    $scope.graduityamount = $scope.gratuitydetails[0].graduityamount
                }
                $scope.tadatotalamount = 0;
                if (promise.tadadetails != null && promise.tadadetails.length > 0) {
                    $scope.tadadetails = promise.tadadetails;
                    angular.forEach($scope.tadadetails, function (itm) {
                        $scope.tadatotalamount = $scope.tadatotalamount + itm.VTADAAA_TotalSactionedAmount;
                    });
                }
                $scope.loantotalamount = 0
                if (promise.loandetails != null && promise.loandetails.length > 0) {
                    $scope.loandetails = promise.loandetails
                    angular.forEach(promise.loandetails, function (itm) {
                        $scope.loantotalamount = $scope.loantotalamount + itm.HREL_TotalPending;
                    });
                }

                $scope.damageamount = 0
                if (promise.demagedetails != null && promise.demagedetails.length > 0) {
                    $scope.demagedetails = promise.demagedetails
                    angular.forEach(promise.demagedetails, function (itm) {
                        $scope.damageamount = $scope.damageamount + itm.ISMRESGCLAPP_DamageAmount;
                    });
                }

                if (promise.ffdetailsarray != null && promise.ffdetailsarray.length > 0) {
                    $scope.ISMRESG_FFTotalAmount = promise.ffdetailsarray[0].ismresG_FFTotalAmount;
                }

                $scope.totalearningamount = $scope.totalenchashedamt + $scope.tadatotalamount + $scope.graduityamount + $scope.TotalPayable
                $scope.totaldeductionamount = $scope.damageamount + $scope.loantotalamount
                $scope.totalpayable = $scope.totalearningamount - $scope.totaldeductionamount;

            });
        };


        $scope.totalFandFCalculationA = function () {

            //Earnings
            if ($scope.pendingsalarydavailable == true) {
                $scope.totalearningamount = $scope.totalearningamount + $scope.TotalPayable;
            }
            else {
                $scope.totalearningamount = $scope.totalearningamount - $scope.TotalPayable;
            }
        };
        $scope.totalFandFCalculationB = function () {
            if ($scope.leaveencashmentavailable == true) {
                $scope.totalearningamount = $scope.totalearningamount + $scope.totalenchashedamt;
            }
            else {
                $scope.totalearningamount = $scope.totalearningamount - $scope.totalenchashedamt;
            }
        };
        $scope.totalFandFCalculationC = function () {
            if ($scope.gratuitydetailsavailable == true) {
                $scope.totalearningamount = $scope.totalearningamount + $scope.graduityamount;
            }
            else {
                $scope.totalearningamount = $scope.totalearningamount - $scope.graduityamount;
            }
        };
        $scope.totalFandFCalculationD = function () {
            if ($scope.tadadetailsavailable == true) {
                $scope.totalearningamount = $scope.totalearningamount + $scope.tadatotalamount;
            }
            else {
                $scope.totalearningamount = $scope.totalearningamount - $scope.tadatotalamount;
            }
        };

        //Deductions
        $scope.totalFandFCalculationE = function () {
            if ($scope.damageavailable == true) {
                $scope.totaldeductionamount = $scope.totaldeductionamount + $scope.damageamount;
            }
            else {
                $scope.totaldeductionamount = $scope.totaldeductionamount - $scope.damageamount;
            }
        };
        $scope.totalFandFCalculationF = function () {
            if ($scope.loansavailable == true) {
                $scope.totaldeductionamount = $scope.totaldeductionamount + $scope.loantotalamount;
            }
            else {
                $scope.totaldeductionamount = $scope.totaldeductionamount - $scope.loantotalamount;
            }
        };

        $scope.totalpayable = $scope.totalearningamount - $scope.totaldeductionamount;



        $scope.nocdetails = function () {
            var data = {
                "HRME_Id": $scope.hrmE_Id,
            };
            apiService.create("HR_NOC_Process/nocdetails", data).then(function (promise) {

                if (promise.aprovedlist != null && promise.aprovedlist != undefined && promise.aprovedlist.length > 0) {
                    $scope.nocapproveddetails = promise.aprovedlist;
                }
                if (promise.checklist != null && promise.checklist != undefined && promise.checklist.length > 0) {
                    $scope.checklistapprovedlist = promise.checklist;
                }
            });
        };

        $scope.usercheckC2 = "";
        $scope.get_evalistt = function () {
            $scope.usercheckC2 = $scope.doc_list.every(function (options) {
                return options.selectedd2;
            });
        };

        $scope.remove_file = function () {

            $scope.file_detail = "";
            $scope.notice = "";
        };

        //===================================== save and generate Report===========================

        $scope.Savedata_print = function () {
            $scope.submitted2 = true;
            if ($scope.myForm3.$valid) {
                var data = {
                    "HRME_Id": $scope.obj.hrmE_Id1,
                    "RelievingLetterLNo": $scope.RefNo,
                    "companyname": $scope.companyname,
                    "companyaddress": $scope.companyaddress
                };
                apiService.create("Exit_Employee/Savedata_print", data).then(function (promise) {
                    if (promise.return_p === "Update") {
                        swal('Report Generate Successfully');

                    }
                    else if (promise.return_p === "Duplicate") {
                        swal('Report Already Generated');
                    }

                    else {
                        swal('Report Not Generate !');
                    }
                    $scope.ismresgmrEId1 = '';
                    $scope.searchValue2 = '';
                    $scope.ismresgmrEId3 = -1;
                    $scope.AllDataLoad();

                });
            }
            else {
                $scope.submitted2 = true;
                swal('Enter Required Field');

            }
        };
        $scope.Print_preview = function (ww) {
            var hrmeid = ww.hrmE_Id;
            apiService.getURI("Exit_Employee/print_exit_employee", hrmeid).then(function (promise) {
                $scope.exit_print_list2 = promise.exit_employee_print_report;
                $scope.employeename_p = promise.exit_employee_print_report[0].employeename_p;
                $scope.HRME_EmployeeCode_p = promise.exit_employee_print_report[0].HRME_EmployeeCode;
                $scope.HRMDES_DesignationName_p = promise.exit_employee_print_report[0].HRMDES_DesignationName_p;
                $scope.HRME_DOJ_p = promise.exit_employee_print_report[0].HRME_DOJ_p;
                $scope.HRME_LeftFlag = promise.exit_employee_print_report[0].HRME_LeftFlag;
                $scope.HRME_DOL = promise.exit_employee_print_report[0].HRME_DOL;
                $scope.year_p = promise.exit_employee_print_report[0].year_p;
                $scope.month_p = promise.exit_employee_print_report[0].month_p;
                $scope.company_Name_p = promise.exit_employee_print_report[0].company_Name_p;
                $scope.ISMRESG_AccRejDate_p = promise.exit_employee_print_report[0].ISMRESG_AccRejDate_p;
                $scope.ISMRESG_TentativeLeavingDate_p = promise.exit_employee_print_report[0].ISMRESG_TentativeLeavingDate_p;
                $scope.Gdate_p = promise.exit_employee_print_report[0].Gdate_p;
                $scope.ISMRESGRL_Id = promise.exit_employee_print_report[0].ismresgrL_Id;

                if (promise.r_ismsesg_id != null && promise.r_ismsesg_id.length > 0) {
                    $scope.MAXVALUE = promise.r_ismsesg_id[0].MAXVALUE;
                    $scope.OVERALLRATING = promise.r_ismsesg_id[0].OVERALLRATING;
                }

                $scope.rating = "4/5"


                $scope.employeeid = [];
                angular.forEach(promise.ratingarray, function (dev) {
                    if ($scope.employeeid.length === 0) {
                        $scope.employeeid.push({
                            RATMQECL_Id: dev.RATMQECL_Id,
                            RATMQECL_QuestionsClassification: dev.RATMQECL_QuestionsClassification,
                            RatingOption: dev.RatingOption
                        });
                    } else if ($scope.employeeid.length > 0) {
                        var intcount = 0;
                        angular.forEach($scope.employeeid, function (emp) {
                            if (emp.RATMQECL_Id === dev.RATMQECL_Id) {
                                intcount += 1;
                            }
                        });
                        if (intcount === 0) {
                            $scope.employeeid.push({
                                RATMQECL_Id: dev.RATMQECL_Id, RATMQECL_QuestionsClassification: dev.RATMQECL_QuestionsClassification,
                                RatingOption: dev.RatingOption
                            });
                        }
                    }
                });



                angular.forEach($scope.employeeid, function (ddd) {
                    $scope.templist = [];
                    angular.forEach(promise.ratingarray, function (dd) {
                        if (dd.RATMQECL_Id === ddd.RATMQECL_Id) {

                            $scope.templist.push(dd);
                        }
                    });
                    ddd.ratingdetails = $scope.templist;
                });







            });
        };
        $scope.view_rel = function (ww) {
            var ISMRESG_Id = ww.ISMRESG_Id;
            apiService.getURI("Exit_Employee/relieving_exit_employee_view", ISMRESG_Id).then(function (promise) {
                $scope.exit_print_list1 = promise.exit_print_list1;
            });
        };

        $scope.printold = function () {

            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BGIProgressReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();

        };

        $scope.print = function (obj) {
            $scope.referencenumber = "";
            $scope.companyname_p = "";
            $scope.companyaddress_p = "";
            $scope.referencenumber = obj.ISMRESGRL_RelievingLetterLNo;
            $scope.companyname_p = obj.ISMRESGRL_CompanyName;
            $scope.companyaddress_p = obj.ISMRESGRL_CompanyAddress;

            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Vikasa/VIKASATCREPORT/VIKASATCReportPdf1.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


        $scope.printff = function (Baldwin) {
            $scope.hidetextbox = true;
            var innerContents = document.getElementById("printSectionIdff").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Vikasa/VIKASATCREPORT/VIKASATCReportPdf1.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        $scope.printnoc = function (Baldwin) {
            var innerContents = document.getElementById("printnoccertificate").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Vikasa/VIKASATCREPORT/VIKASATCReportPdf1.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        //=====================================
        $scope.edit_rel = function (ss) {
            var pagid = ss.HRME_Id;
            apiService.getURI("Exit_Employee/edit_relieving", pagid).then(function (promise) {
                $scope.employeename1 = promise.relieving_check_edit_dd[0].employeename;
                $scope.hrmE_Id2 = promise.relieving_check_edit_dd[0].hrmE_Id;
                $scope.DateOfJoining = new Date(promise.relieving_check_edit_dd[0].hrmE_DOJ);
                $scope.Department = promise.relieving_check_edit_dd[0].hrmD_DepartmentName;
                $scope.Designation = promise.relieving_check_edit_dd[0].hrmdeS_DesignationName;
                $scope.RelievingDate = new Date(promise.relieving_check_edit_dd[0].ismresG_TentativeLeavingDate);
                $scope.ismresgmrEId = promise.relieving_check_edit_dd[0].ismresG_Id;
                $scope.documentList = promise.relieving_check_edit;
                if ($scope.documentList.length > 0) {
                    $scope.document = {};
                    $scope.documentList = promise.relieving_check_edit;
                    angular.forEach(promise.documentList, function (value, key) {
                        $('#' + value.ismresgmcL_Id).attr('src', value.document_Path);
                    });
                }
                if ($scope.documentList.length > 0) {
                    angular.forEach($scope.documentList, function (ss) {
                        if (ss.ismresgcL_Id > 0) {
                            ss.selectedd2 = true;
                        }
                    });
                }
            });
        };



        $scope.showmodaldetails = function (data) {
            $('#preview').attr('src', data.ismresgcL_FilePath);
        };


        $scope.showmodaldetailscheck34 = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];

            if (extention == "jpg" || extention == "jpeg" || extention == "png") {
                $('#preview').attr('src', data);
                $('#myModal').modal('show');
            }
            else if (extention == "doc" || extention == "docx") {
                $('#preview').removeAttr('src');
            }
            else if (extention == "pdf") {
                var imagedownload = data;
                $scope.content = "";
                var fileURL = "";
                var file = "";
                $http.get(imagedownload, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        var pdfId = document.getElementById("pdfIdzz");
                        pdfId.removeChild(pdfId.childNodes[0]);
                        var embed = document.createElement('embed');
                        embed.setAttribute('src', fileURL);
                        embed.setAttribute('type', 'application/pdf');
                        embed.setAttribute('width', '100%');
                        embed.setAttribute('height', '1000');
                        pdfId.appendChild(embed);
                        $('#showpdf').modal('show');

                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        $scope.content = $sce.trustAsResourceUrl(fileURL);
                        $('#showpdf').modal('show');
                    });
            }
        };



        $scope.showmodaldetailscheck1212 = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];

            if (extention == "jpg" || extention == "jpeg" || extention == "png") {
                $('#preview').attr('src', data);
                $('#myModal').modal('show');
            }
            else if (extention == "pdf") {
                var imagedownload = data;
                $scope.content = "";
                var fileURL = "";
                var file = "";
                $http.get(imagedownload, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        var pdfId = document.getElementById("pdfIdzz");
                        pdfId.removeChild(pdfId.childNodes[0]);
                        var embed = document.createElement('embed');
                        embed.setAttribute('src', fileURL);
                        embed.setAttribute('type', 'application/pdf');
                        embed.setAttribute('width', '100%');
                        embed.setAttribute('height', '1000');
                        pdfId.appendChild(embed);
                        $('#showpdf').modal('show');

                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        $scope.content = $sce.trustAsResourceUrl(fileURL);
                        $('#showpdf').modal('show');
                    });
            }
        };




        $scope.showmodaldetailscheck = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];

            if (extention == "jpg" || extention == "jpeg" || extention == "png") {
                $('#preview').attr('src', data);
                $('#myModal').modal('show');
            }
            else if (extention == "pdf") {
                var imagedownload = data;
                $scope.content = "";
                var fileURL = "";
                var file = "";
                $http.get(imagedownload, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        var pdfId = document.getElementById("pdfIdzz");
                        pdfId.removeChild(pdfId.childNodes[0]);
                        var embed = document.createElement('embed');
                        embed.setAttribute('src', fileURL);
                        embed.setAttribute('type', 'application/pdf');
                        embed.setAttribute('width', '100%');
                        embed.setAttribute('height', '1000');
                        pdfId.appendChild(embed);
                        $('#showpdf').modal('show');

                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        $scope.content = $sce.trustAsResourceUrl(fileURL);
                        $('#showpdf').modal('show');
                    });

            }
            else if (extention == "xlsx" || extention == "xls" || extention == "ppt" || extention == "zip" || extention == "doc" || extention == "docx" || extention == "txt") {
                //  $window.open(imagepreview)
                data = "https://view.officeapps.live.com/op/view.aspx?src=" + data;
                $scope.detailFrame = $sce.trustAsResourceUrl(data);
                $('#myModaldocx').modal('show');
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

        $scope.onview = function (filepath) {
            $('#showpdf').attr('src', filepath.document_Path);
        };
        $scope.onview = function (filepath, filename) {

            var imagedownload = filepath.document_Path;
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
        //===============


        //===========================Upload======================================
        //$scope.SelectedFileForUploadzdd = [];
        //$scope.selectFileforUploadzd = function (input, document) {

        //    $scope.SelectedFileForUploadzdd = input.files;

        //    if (input.files && input.files[0]) {

        //        if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
        //        {
        //            var reader = new FileReader();
        //            reader.onload = function (e) {
        //                $('#' + document.ismresgmcL_Id).attr('src', e.target.result);
        //            };
        //            reader.readAsDataURL(input.files[0]);
        //            Uploadprofiled(document);
        //        }
        //        else if (input.files[0].type === "application/pdf") {
        //            var reader = new FileReader();
        //            reader.onload = function (e) {
        //                $('#' + document.ismresgmcL_Id).attr('src', e.target.result);
        //            };
        //            reader.readAsDataURL(input.files[0]);
        //            Uploadprofiled(document);
        //        }
        //        else if (input.files[0].type === "application/msword") {
        //            var reader = new FileReader();
        //            reader.onload = function (e) {
        //                $('#' + document.ismresgmcL_Id).attr('src', e.target.result);
        //            };
        //            reader.readAsDataURL(input.files[0]);
        //            Uploadprofiled(document);
        //        }

        //        else if (input.files[0].size > 2097152) {
        //            swal("Image size should be less than 2MB");
        //            return;
        //        } else {
        //            swal("Upload MP4, Pdf, Image Files Only");
        //        }
        //    }
        //};


        $scope.SelectedFileForUploadzdd = [];
        $scope.selectFileforUploadzd = function (input, document) {

            $scope.SelectedFileForUploadzdd = input.files;

            if (input.files && input.files[0]) {


                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                Uploadprofiled(document);
            }
        };



        function Uploadprofiled(data) {
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.SelectedFileForUploadzdd.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdd[i]);
            }
            // We can send more data to server using append         
            formData.append("Id", data.ismresgmcL_Id);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadnaacdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    data.document_Path = d;
                    $scope.document_Path1 = d;
                    // swal(d);
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

            // Uploads1(miid);
        }

        //View UPLOADED Documents
        $scope.viewcomment = function (user) {
            $scope.uploadeddocumets = [];
            var data = {
                "ISMRESG_Id": user.ISMRESG_Id,
            };
            apiService.create("Exit_Employee/chcklistdocs", data).then(function (promise) {
                if (promise.uploadeddocumets.length > 0 || promise.uploadeddocumets == null) {
                    $scope.uploadeddocumets = promise.uploadeddocumets;
                }

            });
        };


        //Delete uploaded Checklist Documents
        $scope.deletechecklist = function (user) {
            var data = {
                "ISMRESGCL_Id": user.ismresgcL_Id,
            };
            apiService.create("Exit_Employee/chcklistdocs", data).then(function (promise) {
                if (promise.returnval == "Update") {
                    swal('Checklist Deleted Successfully.');
                    $state.reload();
                }
                else if (promise.returnval == "notUpdated") {
                    swal('Checklist Not Deleted Successfully.');
                }
                else {
                    swal('Operation Failed!!');
                }

            });
        };

        //===============================HR Reliving apply======================
        $scope.HR_RelivingApply = function () {
            $scope.submitted2 = true;
            if ($scope.myForm4.$valid) {
                if ($scope.releavingdoc_list.length > 0) {
                    angular.forEach($scope.releavingdoc_list, function (emp) {
                        if (emp.selectedd2 === true || emp.document_Path != null) {
                            $scope.doc_list2.push({ document_Path: emp.document_Path, ISMRESGCL_FileName: emp.ismresgmcL_CheckListName, ISMRESGMCL_Id: emp.ismresgmcL_Id });
                        }
                    });
                }
                else {
                    $scope.doc_list2 = [];
                }


                var data = {
                    "ISMRESG_Id": $scope.ISMRESG_Id,
                    "HRME_Id": $scope.obj.hrmE_Id.hrmE_Id,
                    "ISMRESGMRE_Id": $scope.obj.ismresgmrE_Id,
                    "ISMRESG_Remarks": $scope.obj.ISMRESG_Remarks,
                    "ISMRESG_Flg": "Relieving",
                    doc_list2: $scope.doc_list2,
                };
                apiService.create("Exit_Employee/HR_RelivingApply", data).then(function (promise) {
                    if (promise.returnval === "Add") {
                        swal('Record Saved Successfully');
                        $state.reload();
                    }
                    else if (promise.returnval === "dublicate") {
                        swal('Record Already Exist');
                    }
                    else {
                        swal('Record Not Saved Successfully !');
                    }
                    $scope.ismresgmrEId1 = '';
                    $scope.searchValue2 = '';
                    $scope.ismresgmrEId3 = -1;

                });
            }
            else {
                $scope.submitted2 = true;
            }
        };

        $scope.empdetails = function () {
            var data = {
                "HRME_Id": $scope.obj.hrmE_Id.hrmE_Id,
            };
            apiService.create("Exit_Employee/HR_EmployeeDetails", data).then(function (promise) {

                if (promise.managmentempdetails != null && promise.managmentempdetails.length > 0) {
                    $scope.managmentempdetails = promise.managmentempdetails;
                    $scope.employeeTL = promise.managmentempdetails[0].Deptheadname;
                    $scope.teamleadEmailId = promise.managmentempdetails[0].Deptheadmailid;
                    $scope.employeeManager = promise.managmentempdetails[0].Hrname;
                    $scope.mngrEmailId = promise.managmentempdetails[0].Hrmailid;
                    $scope.HRMDES_DesignationName = promise.managmentempdetails[0].HRMDES_DesignationName;
                    $scope.HRMD_DepartmentName = promise.managmentempdetails[0].HRMD_DepartmentName;
                    $scope.HRME_DOJ = new Date(promise.managmentempdetails[0].HRME_DOJ);
                    $scope.HRME_DOL = new Date(promise.managmentempdetails[0].HRME_DOL);
                }
                $scope.ismcertreQ_AppRejFlag = "";
                if (promise.statusFlag != null && promise.statusFlag.length > 0) {
                    $scope.ismcertreQ_AppRejFlag = promise.statusFlag;
                    $scope.hrmE_Id = $scope.obj.hrmE_Id.hrmE_Id
                }
                if (promise.ratingarray != null && promise.ratingarray.length > 0) {
                    $scope.MAXVALUE = promise.ratingarray[0].MAXVALUE;
                    $scope.OVERALLRATING = promise.ratingarray[0].OVERALLRATING;
                }

                if (promise.employeedata != null && promise.employeedata.length > 0) {
                    $scope.DateOfJoining = promise.employeedata[0].HRME_DOJ;
                    $scope.Department = promise.employeedata[0].HRMD_DepartmentName;
                    $scope.Designation = promise.employeedata[0].HRMDES_DesignationName;
                    $scope.mI_Name = promise.employeedata[0].MI_Name;
                    $scope.employeename = promise.employeedata[0].employeename;
                    //$scope.hrmE_Id = promise.employeedata[0].hrmE_Id;
                    $scope.HRME_LeftFlag = promise.employeedata[0].HRME_LeftFlag;
                    $scope.HRME_DOL = new Date(promise.employeedata[0].HRME_DOL);
                    $scope.HRME_LeavingReason = promise.employeedata[0].HRME_LeavingReason;
                    $scope.HRME_Photo = promise.employeedata[0].HRME_Photo;
                    $scope.HRMEMNO_MobileNo = promise.employeedata[0].HRMEMNO_MobileNo;
                    $scope.HRME_EmailId = promise.employeedata[0].HRME_EmailId;
                }

                if (promise.releavingdoc_list != null && promise.releavingdoc_list.length > 0) {
                    $scope.releavingdoc_list = promise.releavingdoc_list;
                }
                if (promise.reason_list_dd != null && promise.reason_list_dd.length > 0) {
                    $scope.reason_list_dd = promise.reason_list_dd;
                }

            });

        }



        $scope.selectedAll1 = "";
        $scope.all_taskrel = function () {
            var checkStatus2 = $scope.obj.alltaskrel;
            angular.forEach($scope.releavingdoc_list, function (itm) {
                itm.selectedd2 = checkStatus2;
            });
        };

        $scope.noticeperiodcalculation = function (ismresgmrE_Id) {
            angular.forEach($scope.reason_list_dd, function (itm) {
                if (ismresgmrE_Id == itm.ismresgmrE_Id) {
                    $scope.ISMRESG_NoticePeriod = itm.ismresgmrE_NoticePeriodDays
                }

            });
        }


        $scope.edit = function (ss) {
            var id = ss.ISMRESG_Id;
            apiService.getURI("Exit_Employee/Edit_Employee", id).then(function (promise) {

                $scope.newcaste = {};
                if ($scope.employee_list_dd != null && $scope.employee_list_dd.length > 0) {
                    for (var i = 0; i < $scope.employee_list_dd.length; i++) {
                        if (promise.exit_employee_details[0].hrmE_Id == $scope.employee_list_dd[i].hrmE_Id) {
                            $scope.employee_list_dd[i].Selected = true;
                            $scope.obj.hrmE_Id = $scope.employee_list_dd[i];
                            $scope.newcaste = $scope.employee_list_dd[0].hrmE_Id;
                            // return;
                        }
                    }
                }

                if (promise.managmentempdetails != null && promise.managmentempdetails.length > 0) {
                    $scope.managmentempdetails = promise.managmentempdetails;
                    $scope.employeeTL = promise.managmentempdetails[0].Deptheadname;
                    $scope.teamleadEmailId = promise.managmentempdetails[0].Deptheadmailid;
                    $scope.employeeManager = promise.managmentempdetails[0].Hrname;
                    $scope.mngrEmailId = promise.managmentempdetails[0].Hrmailid;
                    $scope.HRMDES_DesignationName = promise.managmentempdetails[0].HRMDES_DesignationName;
                    $scope.HRMD_DepartmentName = promise.managmentempdetails[0].HRMD_DepartmentName;

                    $scope.HRME_DOJ = new Date(promise.managmentempdetails[0].HRME_DOJ);
                    $scope.HRME_DOJ = new Date(promise.managmentempdetails[0].HRME_DOL);
                }

                if (promise.reason_list_dd != null && promise.reason_list_dd.length > 0) {
                    $scope.reason_list_dd = promise.reason_list_dd;
                }

                if (promise.exit_employee_details.length > 0) {
                    $scope.ISMRESG_ResignationDate = new Date(promise.exit_employee_details[0].ismresG_ResignationDate);
                    $scope.ismresgmrE_Id = promise.exit_employee_details[0].ismresgmrE_Id;
                    $scope.ISMRESG_Remarks = promise.exit_employee_details[0].ismresG_Remarks;
                    $scope.ISMRESG_NoticePeriod = promise.exit_employee_details[0].ismresG_NoticePeriod;
                    $scope.ismresgId_EDIT = promise.exit_employee_details[0].ismresG_Id;
                    $scope.ISMRESG_Id = promise.exit_employee_details[0].ismresG_Id;

                }

                if (promise.employeedata != null && promise.employeedata.length > 0) {
                    $scope.DateOfJoining = promise.employeedata[0].HRME_DOJ;
                    $scope.Department = promise.employeedata[0].HRMD_DepartmentName;
                    $scope.Designation = promise.employeedata[0].HRMDES_DesignationName;
                    $scope.mI_Name = promise.employeedata[0].MI_Name;
                    //  $scope.ismresG_Remarks = promise.employeedata[0].ismresG_Remarks;
                    $scope.employeename = promise.employeedata[0].employeename;
                    //$scope.ismresG_RegRelFlag = promise.employeedata[0].ismresG_RegRelFlag;
                    $scope.hrmE_Id = promise.employeedata[0].hrmE_Id;
                    $scope.HRME_LeftFlag = promise.employeedata[0].HRME_LeftFlag;
                    $scope.HRME_DOL = new Date(promise.employeedata[0].HRME_DOL);
                    $scope.HRME_LeavingReason = promise.employeedata[0].HRME_LeavingReason;
                    $scope.HRME_Photo = promise.employeedata[0].HRME_Photo;
                }




                if (promise.relieving_check_list != null && promise.relieving_check_list.length > 0) {
                    $scope.releavingdoc_list = [];
                    if (promise.relieving_check_list.length > 0) {
                        angular.forEach(promise.relieving_check_list, function (emp) {
                            $scope.releavingdoc_list.push({ ISMRESGMCL_Template: emp.ismresgcL_FilePath, ismresgmcL_CheckListName: emp.ismresgcL_FileName, ismresgmcL_Id: emp.ismresgmcL_Id, selectedd2: true });
                        });
                    }
                    else {
                        $scope.releavingdoc_list = [];
                    }
                }



            });
        };



        // ***************** GET OTP ****************** //
        $scope.getotp = function () {
            if ($scope.myForm3.$valid) {
                var data = {
                    "Emailid": "praveenkumar@globalqtytrainig.com"
                }

                apiService.create("Exit_Employee/getotp", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message !== null && promise.message !== "") {
                            swal(promise.message);
                            $('#otpverification').modal('show');

                        }
                    }

                });
            }
            else {
                $scope.submitted2 = true;
                swal('Enter Required Field');

            }


        };


        $scope.otpsucess = false;
        $scope.submitted2 = false;
        $scope.GPHS_OTP = "";
        $scope.getnewotpverification = function (user) {


            $scope.newotpsave = $scope.GPHS_OTP;
            var data = {
                "genotp": $scope.GPHS_OTP,
            }

            apiService.create("Exit_Employee/getnewotpverification", data).then(function (promise) {

                if (promise.returnval == true) {
                    swal('Your OTP Verification Is Success');
                    $state.reload();
                }
                else {
                    if (promise.message === "Success") {
                        $scope.otpsucess = true;
                        $scope.TriggerOTP = false;
                        swal(promise.message);
                        $scope.Savedata_print();
                        $('#otpverification').modal('hide');
                    } else {
                        swal(promise.message);
                        $scope.GPHS_OTP = "";
                    }
                }
            })

        }







        ///addd
        $scope.damagesinsert = [{ itrS_Id: 'trans1' }];
        if ($scope.damagesinsert.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addgrnrows = function () {
            if ($scope.damagesinsert != null && $scope.damagesinsert.length < 5) {
                for (var i = 0; i === $scope.damagesinsert.length; i++) {
                    var id = $scope.damagesinsert[i].itrS_Id;
                    var lastChar = id.substr(id.length - 1);
                    $scope.cnt = parseInt(lastChar);
                }
                $scope.cnt = $scope.cnt + 1;
                $scope.tet = 'trans' + $scope.cnt;
                var newItemNo = $scope.cnt;
                $scope.damagesinsert.push({ 'itrS_Id': 'trans' + newItemNo });
            }
        };
        $scope.removegrnrows = function (index, data) {
            var newItemNo = $scope.damagesinsert.length - 1;
            $scope.damagesinsert.splice(index, 1);

        };




        $scope.selectedAll1 = "";
        $scope.all_task = function () {
            var checkStatus2 = $scope.alltask;
            angular.forEach($scope.documentList, function (itm) {
                itm.selectedd2 = checkStatus2;
            });
        };
        $scope.usercheckC2 = "";
        $scope.get_evalistt = function () {

            $scope.alltask = $scope.documentList.every(function (options) {
                return options.selectedd2;
            });
        };
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1 || field.$dirty;
        };
        $scope.interacted2 = function (field) {
            return $scope.submitted2 || field.$dirty;
        };
        $scope.interactedapp = function (field) {
            return $scope.submittedapp || field.$dirty;
        };


    }
})();