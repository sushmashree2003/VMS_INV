(function () {
    'use strict';
    angular.module('app').controller('HR_NOC_ApprovalController', HR_NOC_ApprovalController)

    HR_NOC_ApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$filter', '$timeout']
    function HR_NOC_ApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $filter, $timeout) {

        $scope.submitted = false;
        $scope.ShowApprovedDetails = false;
        $scope.totalgrid = [];
        $scope.editflag = false;
        $scope.obj = {};
        $scope.object = {};
        $scope.hstep = 0;
        $scope.mstep = 0;
        $scope.ReceivingTime = "";
        //$scope.obj.Fromdate = new Date();

        var date = new Date();
        $scope.obj.collectiondate = new Date();
        $scope.obj.Fromdate = new Date(date.getFullYear(), date.getMonth(), date.getDate() + 1)
        $scope.obj.Todate = new Date(date.getFullYear(), date.getMonth(), date.getDate() + 7)

        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("HR_NOC_Process/loaddata", pageid).
                then(function (promise) {

                    $scope.certificate = promise.cerfificatelist;
                    $scope.getloaddata = promise.aprovedlist;

                })
        };



        $scope.toggleAll = function (all2) {
            var toggleStatus = all2;
            angular.forEach($scope.certificate, function (itm) {
                itm.selected = toggleStatus;
            });
        };


        $scope.viewaproval = function (user) {

            var data = {
                "ISMCERTREQ_Id": user.ISMCERTREQ_Id,
                "HRME_Id": user.HRME_Id
            };
            apiService.create("HR_NOC_Process/viewuploadflies", data).then(function (promise) {

                $scope.getcertificatedata = promise.document;
            });
        };

        $scope.viewaprovals = function (user) {
            var data = {
                "ISMCERTREQ_Id": user.ismcertreQ_Id
            };
            apiService.create("HR_NOC_Process/viewuploadflies", data).then(function (promise) {
                $scope.getviewdata = promise.attachment;
            });
        };

        $scope.ShowApprovedDetails1 = false;

        $scope.GetAllRelDatagrid = function (user) {
            $scope.ismcertreQ_AppRejFlag = user.ISMCERTREQAPP_AppRejFlag;
            $scope.hrmE_Id = user.HRME_Id
            $scope.GetAllRelData(user);
            $scope.ShowApprovedDetails1 = true;
            $scope.ShowApprovedDetails = false;



        }


        $scope.GetAllRelDatagrid = function (user) {
            $scope.ismcertreQ_AppRejFlag = user.ISMCERTREQAPP_AppRejFlag;
            $scope.hrmE_Id = user.HRME_Id
            $scope.ShowApprovedDetails1 = true;
            $scope.ShowApprovedDetails = false;
            $scope.getcertificatedata = [];
            $scope.aprovedlist = [];
            $scope.display_data = [];
            $scope.getloaddetails = [];
            $scope.getloaddetails = [];
            $scope.checklist = [];
            $scope.ISMCERTREQ_Remarks = "";
            var data = {
                "HRME_Id": user.HRME_Id,
                "ISMCERTREQ_Id": user.ISMCERTREQ_Id,
            };
            apiService.create("HR_NOC_Process/viewuploadflies", data).then(function (promise) {
                $scope.ISMCERTREQ_Id = user.ISMCERTREQ_Id
                //if (promise.document != null && promise.document.length > 0) {
                //    $scope.getcertificatedata = promise.document;                    
                //}
                //else {
                //    swal("CheckList Mapping Not Done!")
                //}
                if (promise.aprovedlist != null && promise.aprovedlist.length > 0) {
                    $scope.aprovedlist = promise.aprovedlist;
                }
                if (promise.modalview != null && promise.modalview.length > 0) {
                    $scope.invetarydetails = promise.modalview;


                }
                if (promise.checklist != null && promise.checklist.length > 0) {
                    $scope.checklist = promise.checklist;
                }
                if (promise.ismcertreqapP_Remarks != null && promise.ismcertreqapP_Remarks.length > 0) {
                    $scope.ISMCERTREQ_Remarks = promise.ismcertreqapP_Remarks;
                }
                if (promise.roleType != null && promise.roleType.length > 0) {
                    $scope.roleType = promise.roleType;
                }
                if (promise.viewlist != null && promise.viewlist.length > 0) {
                    $scope.viewlist = promise.viewlist;
                    $scope.HRPAON_SanctionLevelNo = $scope.viewlist[0].hrpaoN_SanctionLevelNo;
                    $scope.HRPAON_FinalFlg = $scope.viewlist[0].hrpaoN_FinalFlg;
                }
                if (promise.display_data != null && promise.display_data.length > 0) {
                    $scope.display_data = promise.display_data;
                    $scope.ISMCERTDISDET_ReceivedMode = $scope.display_data[0].ISMCERTDISDET_ReceivedMode;
                    $scope.ISMCERTDISDET_ReceivedateTime = $scope.display_data[0].ISMCERTDISDET_ReceivedateTime;
                    $scope.ISMCERTDISDET_FineAmount = $scope.display_data[0].ISMCERTDISDET_FineAmount;
                    $scope.ISMCERTDISDET_FileName = $scope.display_data[0].ISMCERTDISDET_FileName;
                    $scope.ISMCERTDISDET_Remarks = $scope.display_data[0].ISMCERTDISDET_Remarks;
                    $scope.ISMCERTDISDET_ReceivingFromDate = $scope.display_data[0].ISMCERTDISDET_ReceivingFromDate;
                    $scope.ISMCERTDISDET_ReceivingToDate = $scope.display_data[0].ISMCERTDISDET_ReceivingToDate;
                    $scope.ISMCERTREQAPP_ApprovedDate = $scope.display_data[0].ISMCERTREQAPP_ApprovedDate;
                    $scope.ISMCERTREQAPP_ReceiningDate = $scope.display_data[0].ISMCERTREQAPP_ReceiningDate;
                    $scope.ISMCERTREQAPP_Remarks = $scope.display_data[0].ISMCERTREQAPP_Remarks;
                    $scope.ISMCERTREQAPP_FinalFlg = $scope.display_data[0].ISMCERTREQAPP_FinalFlg;
                    $scope.ISMCERTREQAPP_AppRejFlag = $scope.display_data[0].ISMCERTREQAPP_AppRejFlag;
                    $scope.ISMCERTREQAPP_RecivingTime = $scope.display_data[0].ISMCERTREQAPP_RecivingTime;
                    $scope.ISMCERTREQAPP_ACK = $scope.display_data[0].ISMCERTREQAPP_ACK;
                    $scope.ISMCERTREQ_RequestDate = $scope.display_data[0].ISMCERTREQ_RequestDate;
                    $scope.ISMCERTREQ_Remarks = $scope.display_data[0].ISMCERTREQ_Remarks;
                    $scope.ISMCERTREQ_ReceivingMode = $scope.display_data[0].ISMCERTREQ_ReceivingMode;
                    $scope.ISMCERTREQ_FileName = $scope.display_data[0].ISMCERTREQ_FileName;
                    $scope.ISMCERTREQ_FilePath = $scope.display_data[0].ISMCERTREQ_FilePath;
                    $scope.ISMCERTREQ_FilePath = $scope.display_data[0].ISMCERTREQ_AppRejFlag;
                    $scope.ISMCERTREQ_RecivingAddress = $scope.display_data[0].ISMCERTREQ_RecivingAddress;
                    $scope.AuthorisedEmployee = $scope.display_data[0].AuthorisedEmployee;
                }

                if (promise.employees != null && promise.employees.length > 0) {
                    $scope.employees = promise.employees;
                }
                else {
                    $scope.employees = [];
                }

                if (promise.getloaddetails != null && promise.getloaddetails.length > 0) {
                    $scope.getloaddetails = promise.getloaddetails
                    $scope.DateOfJoining = $scope.getloaddetails[0].HRME_DOJ;
                    $scope.Department = $scope.getloaddetails[0].HRMD_DepartmentName;
                    $scope.Designation = $scope.getloaddetails[0].HRMDES_DesignationName;
                    $scope.mI_Name = $scope.getloaddetails[0].MI_Name;
                    //  $scope.ismresG_Remarks = promise.getloaddetails[0].ismresG_Remarks;
                    $scope.employeename = $scope.getloaddetails[0].employeename;
                    //$scope.ismresG_RegRelFlag = promise.getloaddetails[0].ismresG_RegRelFlag;
                    //$scope.hrmE_Id = promise.getloaddetails[0].hrmE_Id;
                    $scope.HRME_LeftFlag = $scope.getloaddetails[0].HRME_LeftFlag;
                    $scope.HRME_DOL = $scope.getloaddetails[0].HRME_DOL;
                    $scope.HRME_LeavingReason = $scope.getloaddetails[0].HRME_LeavingReason;
                    $scope.HRME_Photo = $scope.getloaddetails[0].HRME_Photo;

                    $scope.HRMEMNO_MobileNo = promise.getloaddetails[0].HRMEMNO_MobileNo;
                    $scope.HRME_EmailId = promise.getloaddetails[0].HRME_EmailId;
                }
                if (promise.ismcertreQ_FilePath != null && promise.ismcertreQ_FilePath.length > 0) {
                    $scope.ISMCERTREQAPP_ACK = promise.ismcertreQ_FilePath;
                }
                else {
                    $scope.ISMCERTREQAPP_ACK = "";
                }
                if (promise.maxmumlevel != null && promise.maxmumlevel > 0) {
                    $scope.maxmumlevel = promise.maxmumlevel - 1;
                }
                else {
                    $scope.maxmumlevel = 0;
                }
                if (promise.ismcertreqapP_ReceiningDate != null && promise.ismcertreqapP_ReceiningDate.length > 0) {
                    $scope.ISMCERTREQAPP_ReceiningDate = promise.ismcertreqapP_ReceiningDate;
                }
                else {
                    $scope.ISMCERTREQAPP_ReceiningDate = 0;
                }


                //$scope.getloaddata = promise.document;

            });
        };






        $scope.GetAllRelData = function (user) {
            $scope.getcertificatedata = [];
            $scope.aprovedlist = [];
            $scope.display_data = [];
            $scope.getloaddetails = [];
            $scope.getloaddetails = [];
            $scope.checklist = [];
            $scope.ISMCERTREQ_Remarks = "";
            var data = {
                "HRME_Id": user.HRME_Id,
                "ISMCERTREQ_Id": user.ISMCERTREQ_Id,
            };
            apiService.create("HR_NOC_Process/viewuploadflies", data).then(function (promise) {
                $scope.ISMCERTREQ_Id = user.ISMCERTREQ_Id
                $scope.ShowApprovedDetails = true;
                //if (promise.document != null && promise.document.length > 0) {
                //    $scope.getcertificatedata = promise.document;                    
                //}
                //else {
                //    swal("CheckList Mapping Not Done!")
                //}
                if (promise.aprovedlist != null && promise.aprovedlist.length > 0) {
                    $scope.aprovedlist = promise.aprovedlist;
                }
                if (promise.modalview != null && promise.modalview.length > 0) {
                    $scope.invetarydetails = promise.modalview;


                }
                if (promise.checklist != null && promise.checklist.length > 0) {
                    $scope.checklist = promise.checklist;
                }
                if (promise.ismcertreqapP_Remarks != null && promise.ismcertreqapP_Remarks.length > 0) {
                    $scope.ISMCERTREQ_Remarks = promise.ismcertreqapP_Remarks;
                }
                if (promise.roleType != null && promise.roleType.length > 0) {
                    $scope.roleType = promise.roleType;
                }
                if (promise.viewlist != null && promise.viewlist.length > 0) {
                    $scope.viewlist = promise.viewlist;
                    $scope.HRPAON_SanctionLevelNo = $scope.viewlist[0].hrpaoN_SanctionLevelNo;
                    $scope.HRPAON_FinalFlg = $scope.viewlist[0].hrpaoN_FinalFlg;
                }
                if (promise.display_data != null && promise.display_data.length > 0) {
                    $scope.display_data = promise.display_data;
                    $scope.ISMCERTDISDET_ReceivedMode = $scope.display_data[0].ISMCERTDISDET_ReceivedMode;
                    $scope.ISMCERTDISDET_ReceivedateTime = $scope.display_data[0].ISMCERTDISDET_ReceivedateTime;
                    $scope.ISMCERTDISDET_FineAmount = $scope.display_data[0].ISMCERTDISDET_FineAmount;
                    $scope.ISMCERTDISDET_FileName = $scope.display_data[0].ISMCERTDISDET_FileName;
                    $scope.ISMCERTDISDET_Remarks = $scope.display_data[0].ISMCERTDISDET_Remarks;
                    $scope.ISMCERTDISDET_ReceivingFromDate = $scope.display_data[0].ISMCERTDISDET_ReceivingFromDate;
                    $scope.ISMCERTDISDET_ReceivingToDate = $scope.display_data[0].ISMCERTDISDET_ReceivingToDate;
                    $scope.ISMCERTREQAPP_ApprovedDate = $scope.display_data[0].ISMCERTREQAPP_ApprovedDate;
                    $scope.ISMCERTREQAPP_ReceiningDate = $scope.display_data[0].ISMCERTREQAPP_ReceiningDate;
                    $scope.ISMCERTREQAPP_Remarks = $scope.display_data[0].ISMCERTREQAPP_Remarks;
                    $scope.ISMCERTREQAPP_FinalFlg = $scope.display_data[0].ISMCERTREQAPP_FinalFlg;
                    $scope.ISMCERTREQAPP_AppRejFlag = $scope.display_data[0].ISMCERTREQAPP_AppRejFlag;
                    $scope.ISMCERTREQAPP_RecivingTime = $scope.display_data[0].ISMCERTREQAPP_RecivingTime;
                    $scope.ISMCERTREQAPP_ACK = $scope.display_data[0].ISMCERTREQAPP_ACK;
                    $scope.ISMCERTREQ_RequestDate = $scope.display_data[0].ISMCERTREQ_RequestDate;
                    $scope.ISMCERTREQ_Remarks = $scope.display_data[0].ISMCERTREQ_Remarks;
                    $scope.ISMCERTREQ_ReceivingMode = $scope.display_data[0].ISMCERTREQ_ReceivingMode;
                    $scope.ISMCERTREQ_FileName = $scope.display_data[0].ISMCERTREQ_FileName;
                    $scope.ISMCERTREQ_FilePath = $scope.display_data[0].ISMCERTREQ_FilePath;
                    $scope.ISMCERTREQ_FilePath = $scope.display_data[0].ISMCERTREQ_AppRejFlag;
                    $scope.ISMCERTREQ_RecivingAddress = $scope.display_data[0].ISMCERTREQ_RecivingAddress;
                    $scope.AuthorisedEmployee = $scope.display_data[0].AuthorisedEmployee;
                }

                if (promise.employees != null && promise.employees.length > 0) {
                    $scope.employees = promise.employees;
                }
                else {
                    $scope.employees = [];
                }

                if (promise.getloaddetails != null && promise.getloaddetails.length > 0) {
                    $scope.getloaddetails = promise.getloaddetails
                    $scope.DateOfJoining = $scope.getloaddetails[0].HRME_DOJ;
                    $scope.Department = $scope.getloaddetails[0].HRMD_DepartmentName;
                    $scope.Designation = $scope.getloaddetails[0].HRMDES_DesignationName;
                    $scope.mI_Name = $scope.getloaddetails[0].MI_Name;
                    //  $scope.ismresG_Remarks = promise.getloaddetails[0].ismresG_Remarks;
                    $scope.employeename = $scope.getloaddetails[0].employeename;
                    //$scope.ismresG_RegRelFlag = promise.getloaddetails[0].ismresG_RegRelFlag;
                    //$scope.hrmE_Id = promise.getloaddetails[0].hrmE_Id;
                    $scope.HRME_LeftFlag = $scope.getloaddetails[0].HRME_LeftFlag;
                    $scope.HRME_DOL = $scope.getloaddetails[0].HRME_DOL;
                    $scope.HRME_LeavingReason = $scope.getloaddetails[0].HRME_LeavingReason;
                    $scope.HRME_Photo = $scope.getloaddetails[0].HRME_Photo;

                    $scope.HRMEMNO_MobileNo = promise.getloaddetails[0].HRMEMNO_MobileNo;
                    $scope.HRME_EmailId = promise.getloaddetails[0].HRME_EmailId;
                }
                if (promise.ismcertreQ_FilePath != null && promise.ismcertreQ_FilePath.length > 0) {
                    $scope.ISMCERTREQAPP_ACK = promise.ismcertreQ_FilePath;
                }
                else {
                    $scope.ISMCERTREQAPP_ACK = "";
                }
                if (promise.maxmumlevel != null && promise.maxmumlevel > 0) {
                    $scope.maxmumlevel = promise.maxmumlevel - 1;
                }
                else {
                    $scope.maxmumlevel = 0;
                }
                if (promise.ismcertreqapP_ReceiningDate != null && promise.ismcertreqapP_ReceiningDate.length > 0) {
                    $scope.ISMCERTREQAPP_ReceiningDate = promise.ismcertreqapP_ReceiningDate;
                }
                else {
                    $scope.ISMCERTREQAPP_ReceiningDate = 0;
                }


                //$scope.getloaddata = promise.document;

            });
        };



        $scope.assetcollect = function (user) {

            $scope.ISMRESGMCL_Id = user.ISMRESGMCL_Id
            if ($scope.invetarydetails != null && $scope.invetarydetails.length > 0) {

                $scope.employeeid = [];
                angular.forEach($scope.invetarydetails, function (dev) {
                    dev.search = "";
                    if ($scope.employeeid.length === 0) {
                        $scope.employeeid.push(dev);
                    }

                    else if ($scope.employeeid.length > 0) {
                        var intcount = 0;
                        angular.forEach($scope.employeeid, function (emp) {
                            if (emp.HRME_Id === dev.HRME_Id) {
                                intcount += 1;
                            }
                        });
                        if (intcount === 0) {
                            $scope.employeeid.push(dev);
                        }
                    }
                });

                angular.forEach($scope.employeeid, function (ddd) {
                    ddd.RecordDetails = [];
                    $scope.templist = [];
                    angular.forEach($scope.invetarydetails, function (dd) {
                        if (dd.HRME_Id === ddd.HRME_Id) {
                            dd.selected = false;
                            dd.assetdamageAmount = dd.AssetValue;
                            dd.damageRemarks = "";
                            $scope.templist.push(dd);
                        }
                    });
                    ddd.RecordDetails = $scope.templist;

                });

                $scope.totaldamageamount = 0;
                $('#departments').modal('show');

            }
            else {
                swal("Assets Not Mapped For This Employee");
            }

        };



        $scope.addtocart = function () {

            angular.forEach($scope.checklist, function (dd) {
                if ($scope.ISMRESGMCL_Id == dd.ISMRESGMCL_Id) {

                    angular.forEach($scope.invetarydetails[0].RecordDetails, function (ddd) {
                        if (ddd.selected == true) {
                            var overallremarks = "Item Name: " + ddd.INVMI_ItemName + ".\n Item No: " + ddd.INVAAT_AssetId + ".\n Damage Amount: " + ddd.assetdamageAmount + ".\n Damage Remarks: " + ddd.damageRemarks + ". \n\n\n";

                            dd.Remarks = overallremarks;
                        }

                    });
                }



            });

            $('#departments').modal('hide');
        };



        $scope.totaldamageamount = "";
        $scope.calculatedamageamt = function () {
            $scope.totaldamageamount = "";
            angular.forEach($scope.checklist, function (dd) {
                //  dd.DamageAmount = "";
                if ($scope.ISMRESGMCL_Id == dd.ISMRESGMCL_Id) {

                    angular.forEach($scope.invetarydetails[0].RecordDetails, function (ddd) {
                        if (ddd.selected == true && ddd.assetdamageAmount > 0) {
                            $scope.totaldamageamount = Number($scope.totaldamageamount) + Number(ddd.assetdamageAmount)
                        }

                    });
                    dd.DamageAmount = $scope.totaldamageamount;
                }



            });


        };




        $scope.cleardamage = function (itm) {
            angular.forEach($scope.checklist, function (dd) {
                if (itm.ISMRESGMCL_Id == dd.ISMRESGMCL_Id) {
                    dd.DamageAmount = "";
                    dd.Remarks = "";

                }
            });


        };







        $scope.remarksdisplay = function (remarks) {
            $scope.overviewremarksdisplay = "";
            $scope.overviewremarksdisplay = remarks;

            $('#remarksdisplay').modal('show');
        };






        $scope.Viewdocumentdata = function (user) {

            var data = {
                "HRMEDS_Id": user.hrmedS_Id
            };
            apiService.create("HR_NOC_Process/viewuploadflies", data).then(function (promise) {

                $scope.getdocumentdata = promise.viewattachment;
            });
        };


        $scope.approvedreject = function (objjj) {
            if ($scope.myForm.$valid) {

                $scope.typevalue = true;
                $scope.approveddetails = [];
                var HRME_Id = 0;
                $scope.Fromdate = new Date();
                $scope.todate = new Date();
                if ($scope.obj.empid != undefined && $scope.obj.empid != null) {
                    $scope.Fromdate = $scope.obj.Fromdate == null ? "" : $filter('date')($scope.obj.Fromdate);
                    $scope.todate = $scope.obj.Todate == null ? "" : $filter('date')($scope.obj.Todate);
                    HRME_Id = $scope.obj.empid.hrmE_Id;
                }
                else {
                    HRME_Id = 0
                }


                if ($scope.obj.collectiondate != null && $scope.obj.collectiondate != undefined) {
                    $scope.Fromdate = $scope.obj.collectiondate == null ? "" : $filter('date')($scope.obj.collectiondate);
                }
                var doc_list2 = []
                angular.forEach($scope.checklist, function (ty) {
                    doc_list2.push({ ISMRESGMCL_Id: ty.ISMRESGMCL_Id, Remarks: ty.Remarks, DamageAmount: ty.DamageAmount });
                })



                var data = {
                    "ISMCERTREQ_Id": $scope.ISMCERTREQ_Id,
                    "Fromdate": $scope.Fromdate,
                    "Todate": $scope.todate,
                    //  "ISMCERTREQAPP_ReceivingTime": $scope.sresult,
                    "ISMCERTREQAPP_Remarks": $scope.obj.ISMCERTREQAPP_Remarks,
                    "ISMCERTREQAPP_AppRejFlag": objjj,
                    "ISMCERTREQ_FilePath": $scope.ismcertreQ_FilePath,
                    "maxmumlevel": $scope.maxmumlevel,
                    "HRME_Id": HRME_Id,
                    doc_list2: doc_list2
                }
                apiService.create("HR_NOC_Process/approvedrecord", data).then(function (promise) {

                    if (promise.returnval == true) {
                        swal('Record saved successfully');
                        $state.reload();
                    }
                    else if (promise.returnval == false) {
                        swal('Record  Not saved successfully');
                    }


                    //$state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }

        };



        $scope.viewaproval = function (ISMCERTREQ_Id) {
            var data = {
                "ISMCERTREQ_Id": ISMCERTREQ_Id
            };
            apiService.create("HR_NOC_Process/Viewdata", data).
                then(function (promise) {
                    if (promise !== null && promise.modalview.length > 0) {
                        // $scope.modalview = promise.modalview;

                        $scope.modalview = [];
                        angular.forEach(promise.modalview, function (ty) {
                            if (ty.ISMCERTREQAPP_AppRejFlag == "Re-Request") {
                                angular.forEach(promise.modalview, function (ty1) {
                                    if (ty1.ISMCERTREQAPP_AppRejFlag == "Rejected" && ty1.HRME_Id == ty.HRME_Id) {
                                        if (ty.RejectedStatus == undefined) {
                                            ty.RejectedStatus = [];
                                        }

                                        ty.RejectedStatus.push(ty1);
                                    }
                                })
                            }

                            if (ty.ISMCERTREQAPP_AppRejFlag != "Rejected") {
                                // ty.RejectedStatus = [];
                                $scope.modalview.push(ty);
                            }

                        })

                        $('#mymodalviewdetailssecondtab').modal('show');
                    }
                    else {
                        swal(" First Level is Not Approved  !");
                        $('#mymodalviewdetailssecondtab').modal('hide');
                    }
                });
        };

        $scope.rejecteddetails = function (obj) {
            $scope.RejectedStatus = obj.RejectedStatus;
            $('#mymodulerejecteddetails').modal('show');

        };



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




        $scope.documentListOtherDetails = [{ id: 'document' }];
        $scope.selectFileforUploadzdOtherDetail = function (input, document) {
            $scope.ldr = true;
            //$('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdOtherDetail = input.files;
            if (input.files && input.files[0]) {


                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetail(document);

            }
        };
        function UploadEmployeeDocumentOtherDetail(data) {

            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetail.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetail[i]);
            }

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_ISMAttachment_new", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {

                        //data.file = d[0].path;
                        //data.ismcertreQ_FileName = d[0].name;

                        $scope.ismcertreQ_FilePath = d[0].path;;
                        $scope.ismcertreQ_FileName = d[0].name;



                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        //=============== view doc==========

        $scope.previewimg_new = function (img) {
            $scope.imagepreview = img;
            $scope.view_videos = [];
            var img = $scope.imagepreview;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }

            if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {
                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');
            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
            }

            else if ($scope.filetype2 == 'pdf') {
                ///=====================show pdf, img
                $('#showpdf').modal('hide');
                var imagedownload1 = "";
                imagedownload1 = $scope.imagepreview;
                $http.get(imagedownload1, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        var fileURL = "";
                        var file = "";
                        var embed = "";
                        var pdfId = "";
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        pdfId = document.getElementById("pdfIdzz");
                        pdfId.removeChild(pdfId.childNodes[0]);
                        embed = document.createElement('embed');
                        embed.setAttribute('src', fileURL);
                        embed.setAttribute('type', 'application/pdf');
                        embed.setAttribute('width', '100%');
                        embed.setAttribute('height', '1000');
                        pdfId.appendChild(embed);
                        $('#showpdf').modal('show');
                    });
            }
            else {
                $window.open($scope.imagepreview)
            }
        };


        $scope.viewaddress = function (address) {
            $scope.ISMCERTREQ_RecivingAddress = address;
            $('#receivingaddress').modal('show');

        };




        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };



        $scope.search = '';




    }
})();
