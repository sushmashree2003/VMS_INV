(function () {
    'use strict';
    angular.module('app').controller('CheckBookEntryController', CheckBookEntryController)
    CheckBookEntryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', '$window', '$compile']
    function CheckBookEntryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, $window, $compile) {
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        if (transnumconfigsettings != null && transnumconfigsettings.length > 0) {
            for (var i = 0; i < transnumconfigsettings.length; i++) {
                if (transnumconfigsettings[i].imN_Flag === "PMTV") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
        }
        var configsettingsnew = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        $scope.leafcancelD = [];
        $scope.balchange = function () {
            $scope.obj.VPAYVOU_AppliedAmount = 0;
        }
        $scope.voucherBalance = 0;
        $scope.obj.modeFlag = true;
        $scope.Payble_AppliedAmount = 0;
        $scope.Budgetuploadone = [];
        $scope.Budgetuploadone = [{ id: 'materal' }];
        $scope.obj.chkitem = false;
        $scope.obj.TotalAmmountGrn = 0;
        $scope.amtvalidation = function () {
            $scope.obj.VMBDGHD_Id = "";

            $scope.Budgetupload = [];
            $scope.Budgetupload = [{ id: 'materal' }];
            var VPAYVOU_TDS = 0; var VPAYVOU_Penalty = 0; var VPAYVOU_OtherAmount = 0;
            var VPAYVOU_ESICAmount = 0;
            if ($scope.obj.VPAYVOU_TDS != null && $scope.obj.VPAYVOU_TDS > 0 && $scope.obj.TDS == true) {
                VPAYVOU_TDS = $scope.obj.VPAYVOU_TDS;
            }
            if ($scope.obj.VPAYVOU_Penaltiy != null && $scope.obj.VPAYVOU_Penaltiy > 0 && $scope.obj.Penaltiy == true) {
                VPAYVOU_Penalty = $scope.obj.VPAYVOU_Penaltiy;
            }
            if ($scope.obj.VPAYVOU_OtherAmount != null && $scope.obj.VPAYVOU_OtherAmount > 0 && $scope.obj.OtherAmount == true) {
                VPAYVOU_OtherAmount = $scope.obj.VPAYVOU_OtherAmount;
            }
            if ($scope.obj.VPAYVOU_ESICAmount != null && $scope.obj.VPAYVOU_ESICAmount > 0 && $scope.obj.ESICAmount == true) {
                VPAYVOU_ESICAmount = $scope.obj.VPAYVOU_ESICAmount;
            }
            if ($scope.obj.VPAYVOU_AppliedAmount > 0) {
                $scope.obj.Payble_AppliedAmount = (Number($scope.obj.VPAYVOU_AppliedAmount) - Number(VPAYVOU_TDS) - Number(VPAYVOU_Penalty) - Number(VPAYVOU_OtherAmount) - Number(VPAYVOU_ESICAmount));

            }
            else {
                $scope.obj.Payble_AppliedAmount = 0;
            }
            if ($scope.GRnDetails != null && $scope.GRnDetails.length > 0) {
                angular.forEach($scope.GRnDetails, function (comp) {
                    comp.chkitem = false;
                    comp.INVMGRN_PaidAmount = "";
                });
            }


        };
        //$scope.amtchange

        $scope.amtchange = function () {
            $scope.GRnDetails = [];
            if ($scope.obj.INVMS_Id.invmS_Id > 0) {
                var data = {
                    "MI_Id": $scope.obj.MI_Id,
                    "INVMS_Id": $scope.obj.INVMS_Id.invmS_Id,
                    "optionflag": "1"
                };
                apiService.create("CheckBookEntry/loaddatareport", data).then(function (promise) {
                    if (promise.alldata != null && promise.alldata.length > 0) {
                        $scope.GRnDetails = promise.alldata;
                        angular.forEach($scope.GRnDetails, function (comp) {
                            comp.chkitem = false;
                        });
                    }
                });
            }

        }
        $scope.mainmodelclick = function (id) {
            $scope.get_GRNItemDetails = [];
            var data = {
                "INVMGRN_Id": id.INVMGRN_Id,
                "MI_IdNew": id.MI_Id
            };
            apiService.create("INV_T_GRN/get_GRNitemDetails", data).
                then(function (promise) {
                    $scope.get_GRNItemDetails = promise.get_GRNItemDetails;
                    $scope.grnnumber = $scope.get_GRNItemDetails[0].invmgrN_GRNNo;
                });
        };
        //mainmodelclick
        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        };
        $scope.emailotpshow = [];
        $scope.searchchkbx = "";
        $scope.editflag = false;
        $scope.submitted = false;
        $scope.Penaltiy = false;
        $scope.TDS = false;
        $scope.vbabrsF_FileName = [];

        $scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPage1 = 5;
        $scope.currentPage1 = 1;
        $scope.currentPage = 1;
        $scope.totalgrid = [];
        $scope.submitted = false;
        $scope.obj.VPAYVOU_SupplierFlg = true;
        $scope.obj.VPAYVOU_SelfPayFlg = false;
        $scope.obj.VPAYVOU_PaymentDate = new Date();
        //$scope.plMaxdate = new Date();
        $scope.bankarrayll = [];
        $scope.CheckbookSource = [];
        $scope.mainbalance = 0;
        $scope.searchtwo = "";
        $scope.bankchange = function () {
            var Message = "";
            $scope.bomlist = [];
            $scope.CheckbookSource = [];
            $scope.obj.VPAYVOU_ChequeNo = $scope.CheckbookSource[0];
            if ($scope.banklist != null && $scope.banklist.length > 0) {
                for (var i = 0; i < $scope.banklist.length; i++) {
                    if ($scope.banklist[i].hrmbD_Id == $scope.obj.HRMBD_Id) {
                        Message = $scope.banklist[i].hrmbD_BankName + '/' + $scope.banklist[i].hrmbD_BankAccountNo;
                    }
                }
            }
            var data = {
                "HRMBD_Id": $scope.obj.HRMBD_Id,
                "MI_Id": $scope.obj.MI_Id,
                "optionflag": ""
            };
            apiService.create("CheckBookEntry/getbom", data).then(function (promise) {
                if (promise.msg != null && promise.msg != "" && promise.msg == "OB") {
                    $scope.bomlist = promise.projectlist;
                    $scope.CheckbookSource = promise.checkbookSource;
                    if ($scope.CheckbookSource != null && $scope.CheckbookSource.length > 0) {
                        for (var i = 0; i < 1; i++) {
                            $scope.obj.VPAYVOU_ChequeNo = $scope.CheckbookSource[0];
                        }

                    }
                    if (promise.projectlist != null && promise.projectlist.length > 0) {
                        $scope.mainbalance = $scope.bomlist[0].vbabrS_BankBalalnce;
                        $scope.obj.VPAYVOU_BankBalalnce = $scope.bomlist[0].vbabrS_BankBalalnce;
                    }
                    else {
                        $scope.mainbalance = 0;
                        $scope.obj.VPAYVOU_BankBalalnce = 0;
                    }
                    var ds = 0;
                    if ($scope.alldata != null && $scope.alldata.length > 0) {
                        angular.forEach($scope.alldata, function (comp) {
                            if ($scope.obj.HRMBD_Id == comp.HRMBD_Id && comp.VPAYVOU_StatusFlg == 'Pending') {
                                ds += (Number(comp.VPAYVOU_AppliedAmount));
                            }
                        });

                        $scope.voucherBalance = (Number(ds).toFixed(2));
                        if ($scope.voucherBalance > 0) {
                            swal("Voucher Running Balance : " + $scope.voucherBalance + "");
                        }
                    }

                    else {
                        $scope.voucherBalance = 0;
                    }
                }
                else {
                    if (Message != null && Message != "") {
                        swal("Must Be Entered  : " + Message + " - Opening Balnace !");
                    }

                    //$state.reload();
                    $scope.bomlist = [];
                    $scope.CheckbookSource = [];
                    $scope.obj.HRMBD_Id = "";
                    $scope.obj.VPAYVOU_BankBalalnce = 0;
                }


            });
        }

        $scope.bankarray = [];


        $scope.Validation = function (obj) {
            var VMBDGHD_AmountSpent = 0;

            if (obj.VMBDGHD_AmountSpent > 0) {
                if ($scope.Budgetupload != null && $scope.Budgetupload.length > 0) {
                    angular.forEach($scope.Budgetupload, function (itm) {
                        if (itm.VMBDGHD_Id === obj.VMBDGHD_Id && obj.VMBDGHD_AmountSpent != null && obj.VMBDGHD_AmountSpent > 0) {
                            VMBDGHD_AmountSpent = (Number(itm.VMBDGHD_AmountSpent) + VMBDGHD_AmountSpent);
                        }
                    });
                }
                if ((Number($scope.obj.VPAYVOU_AppliedAmount)) < (Number(VMBDGHD_AmountSpent))) {
                    swal("Payble Amount Not Greater Than Budget Amount !");
                    obj.VMBDGHD_AmountSpent = "";
                    return;
                }
                var data = {
                    "MI_Id": $scope.obj.MI_Id,
                    "VPAYVOU_PaymentDate": new Date($scope.obj.VPAYVOU_PaymentDate).toDateString(),
                    "VMBDGHD_Id": obj.VMBDGHD_Id,
                    "VPAYVOU_AppliedAmount": VMBDGHD_AmountSpent,
                };
                apiService.create("CheckBookEntry/paymentnotification", data).then(function (promise) {
                    if (promise.message == "Sucess") {

                    }
                    else {
                        obj.VMBDGHD_Id = "";
                        obj.VMBDGHD_AmountSpent = "";
                        obj.VPAYVOUBUD_Remark = "";
                        swal(promise.message);


                    }


                });
            }
            else {
                obj.VMBDGHD_AmountSpent = "";
                obj.VPAYVOUBUD_Remark = "";
            }
        }

        $scope.appliedtax = [];
        $scope.clientdetails = [];
        $scope.totaltaxamount = 0;
        $scope.oncmpchange = function () {
            $scope.alldata = [];
            $scope.ModeOfPayment = [];
            $scope.CheckBookBudget = [];
            $scope.obj.VPAYVOU_PaymentMode = "";
            $scope.obj.VMBDGHD_Id = "";
            $scope.obj.HRMBD_Id = "";
            var data = {
                "MI_Id": $scope.obj.MI_Id
            };
            apiService.create("CheckBookEntry/companychange", data).then(function (promise) {
                $scope.getClient = promise.client_Master;
                $scope.clientlist = promise.clientlist;
                $scope.yearlist = promise.yearlist;
                $scope.alldata = promise.alldata;
                $scope.get_supplier = promise.get_supplier;
                $scope.banklist = promise.banklist;
                $scope.mi_logo = promise.mI_logo;
                $scope.ModeOfPayment = promise.modeOfPayment;
                $scope.CheckBookBudget = promise.checkBookBudget;
                if ($scope.ModeOfPayment != null && $scope.ModeOfPayment.length > 0) {
                    angular.forEach($scope.ModeOfPayment, function (itm) {
                        if (itm.ivrmmoD_ModeOfPayment == "Cheque") {
                            $scope.obj.VPAYVOU_PaymentMode = itm.ivrmmoD_ModeOfPayment;
                            return;
                        }
                    });
                }

            });
        }
        $scope.Uploaddata = function (VMBDGHD_Id_temp) {
            $scope.CheckBookBudgetlist = [];
            $scope.imagename = "";
            var VMBDGHD_Id = 0;
            var IMFY_Id = 0;
            if ($scope.CheckBookBudget != null && $scope.CheckBookBudget.length > 0) {
                for (var i = 0; i < $scope.CheckBookBudget.length; i++) {
                    if ($scope.CheckBookBudget[i].VMBDGHD_Id == VMBDGHD_Id_temp) {
                        IMFY_Id = $scope.CheckBookBudget[i].IMFY_Id;
                        VMBDGHD_Id = $scope.CheckBookBudget[i].VMBDGHD_Id;
                    }
                }
            }
            var data = {
                "MI_Id": $scope.obj.MI_Id,
                "VMBDGHD_Id": VMBDGHD_Id,
                "IMFY_Id": IMFY_Id,

            };
            apiService.create("CheckBookEntry/getreport", data).then(function (promise) {
                if (promise.alldata != null && promise.alldata.length > 0) {
                    $scope.CheckBookBudgetlist = promise.alldata;
                    $scope.imagename = promise.mI_logo;
                    $scope.VMBDGHD_BudgetHead = promise.alldata[0].VMBDGHD_BudgetHead;
                    $scope.VMBDGHDAMT_Amount = promise.alldata[0].VMBDGHDAMT_Amount;
                    $scope.IMFY_FinancialYear = promise.alldata[0].IMFY_FinancialYear;
                    $scope.VMBDGHDAMT_MonthwiseAmount = promise.alldata[0].VMBDGHDAMT_MonthwiseAmount;
                    var TotalApproved = 0;
                    if ($scope.CheckBookBudgetlist != null && $scope.CheckBookBudgetlist.length > 0) {
                        for (var i = 0; i < $scope.CheckBookBudgetlist.length; i++) {
                            if ($scope.CheckBookBudgetlist[i].VPAYVOU_ApprovedAmount > 0) {
                                TotalApproved += (Number($scope.CheckBookBudgetlist[i].VPAYVOU_ApprovedAmount));
                            }
                            else {
                                TotalApproved += (Number($scope.CheckBookBudgetlist[i].VPAYVOU_AppliedAmount));
                            }
                        }
                    }

                    $scope.TotalApproved = TotalApproved;
                    $('#CheckBookBudget').modal('show');

                }
                else {
                    swal("Check Book Budget Not Found !");
                }
            });
        }
        $scope.UploaddataBudget = function (user) {
            $scope.CheckBookBudgetwo = [];
            $scope.obj.VMBDGHD_Idtwo = "";
            $scope.imagename = "";
            $scope.obj.VPAYVOU_Idthree = user.VPAYVOU_Id;
            $scope.obj.MI_IdTwo = user.MI_Id;
            $scope.Budgetuploadone = [];
            $scope.Budgetuploadone = [{ id: 'materal' }];
            $scope.obj.VPAYVOU_PaymentDatetwo = new Date(user.VPAYVOU_PaymentDate);
            if (user.VPAYVOU_ApprovedAmount > 0) {
                $scope.obj.VPAYVOU_AppliedAmounttwo = user.VPAYVOU_ApprovedAmount;
            }
            else {
                $scope.obj.VPAYVOU_AppliedAmounttwo = user.VPAYVOU_AppliedAmount;
            }

            var data = {
                "msg": "Fiancial",
                "VPAYVOU_Id": user.VPAYVOU_Id,

            };
            apiService.create("CheckBookEntry/paymentnotification", data).then(function (promise) {
                $scope.CheckBookBudgetwo = promise.checkBookBudget;
                if (promise.getconfigloaddata != null && promise.getconfigloaddata.length > 0) {
                    angular.forEach(promise.getconfigloaddata, function (dddd) {
                        dddd.VMBDGHD_Id = dddd.vmbdghD_Id;
                        dddd.VMBDGHD_AmountSpent = dddd.vmbdghD_AmountSpent;
                        dddd.VPAYVOUBUD_Remark = dddd.vpayvoubuD_Remark;

                    });
                    $scope.Budgetuploadone = promise.getconfigloaddata;

                }
                $('#UploadBudget').modal('show');

            });


        }
        $scope.PaymentDate = function () {
            $scope.CheckBookBudget = [];
            $scope.obj.VMBDGHD_Id = "";
            var data = {
                "MI_Id": $scope.obj.MI_Id,
                "VPAYVOU_PaymentDate": new Date($scope.obj.VPAYVOU_PaymentDate).toDateString(),
                "msg": "Fiancial"
            };
            apiService.create("CheckBookEntry/paymentnotification", data).then(function (promise) {
                if (promise.checkBookBudget != null && promise.checkBookBudget.length > 0) {
                    $scope.CheckBookBudget = promise.checkBookBudget;
                }
                else {
                    swal("Check Book Budget Not Found !");
                }



            });
        }
        $scope.loaddata = function () {
            $scope.ISMMCLT_Id = "";
            $scope.ISMMPR_Id = "";
            $scope.ISMCLTPRP_Year = "";
            $scope.FTI_Id = "";
            $scope.ISMCLTPRP_InstallmentAmt = "";
            $scope.ISMCLTPRP_PaymentDate = null;
            $scope.ismcltprP_Id = 0;
            $scope.projectlist = [];
            $scope.obj.MI_Id = "";
            $scope.editflag = false;
            var pageid = 2;
            apiService.getURI("CheckBookEntry/loaddata", pageid).then(function (promise) {
                $scope.allcompany = promise.allcompany;
            });
        };
        $scope.Supliearfrom = function () {
            if ($scope.obj.VPAYVOU_SupplierFlg = true) {
                $scope.obj.VPAYVOU_ToSupplierFlg = false;
            }
            else {
                $scope.obj.VPAYVOU_ToSupplierFlg = true;
            }
        }

        //voucherBalance
        $scope.ChequeNumbersArray = function () {
            $scope.leafcancelD = [];
            for (var i = 0; i < $scope.CheckbookSource.length; i++) {
                if ($scope.obj.VPAYVOU_ChequeNo.vpayvoU_ChequeNo == $scope.CheckbookSource[i].vpayvoU_ChequeNo) {
                    $('#chequecancel').modal('show');
                    return;
                }
                else {
                    $scope.leafcancelD.push({
                        vpayvoU_ChequeNoleaf: $scope.CheckbookSource[i].vpayvoU_ChequeNo,

                    });
                }

            }


            //
        }
        $scope.SupliearTO = function () {
            $scope.GRnDetails = [];

            $scope.obj.INVMS_Id = $scope.GRnDetails[0];
            if ($scope.obj.VPAYVOU_ToSupplierFlg = true) {
                $scope.obj.VPAYVOU_SupplierFlg = false;

            }
            else {
                $scope.obj.VPAYVOU_SupplierFlg = true;
            }
        }
        //
        $scope.savedataModal = function () {
            $scope.submitted1 = true;
            if ($scope.myFormone.$valid) {
                $('#chequecancel').modal('hide');
            }

        }
        $scope.viewCheckBook = function () {
            $('#chequecancel').modal('show');
        }

        $scope.ValidationFire = function (obj) {
            var VMBDGHD_AmountSpent = 0;

            if (obj.VMBDGHD_AmountSpent > 0) {
                if ($scope.Budgetuploadone != null && $scope.Budgetuploadone.length > 0) {
                    angular.forEach($scope.Budgetuploadone, function (itm) {
                        if ((Number(itm.VMBDGHD_Id)) === (Number(obj.VMBDGHD_Id)) && obj.VMBDGHD_AmountSpent != null && obj.VMBDGHD_AmountSpent > 0) {
                            VMBDGHD_AmountSpent = (Number(itm.VMBDGHD_AmountSpent) + VMBDGHD_AmountSpent);
                            //  VMBDGHD_AmountSpent = (Number(itm.VMBDGHDAMT_Amount) + VMBDGHD_AmountSpent);
                        }
                    });
                }
                if ((Number($scope.obj.VPAYVOU_AppliedAmounttwo)) < (Number(VMBDGHD_AmountSpent))) {
                    swal("Payble Amount Not Greater Than Budget Amount !");
                    obj.VMBDGHD_AmountSpent = "";
                    return;
                }
                var data = {
                    "MI_Id": $scope.obj.MI_IdTwo,
                    "VPAYVOU_PaymentDate": new Date($scope.obj.VPAYVOU_PaymentDatetwo).toDateString(),
                    "VMBDGHD_Id": obj.VMBDGHD_Id,
                    "VPAYVOU_AppliedAmount": VMBDGHD_AmountSpent,
                };
                apiService.create("CheckBookEntry/paymentnotification", data).then(function (promise) {
                    if (promise.message != "Sucess") {
                        obj.VMBDGHD_Id = "";
                        obj.VMBDGHD_AmountSpent = "";
                        obj.VPAYVOUBUD_Remark = "";
                        swal(promise.message);
                    }



                });
            }
            else {
                obj.VMBDGHD_AmountSpent = "";
                obj.VPAYVOUBUD_Remark = "";
            }
        }
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var dystring = $scope.obj.VPAYVOU_AppliedAmount;
                swal({
                    title: "Are you sure?",
                    text: "Do You Want To " + dystring + " Update Amount ?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Update Amount",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            $scope.uploadarray = [];
                            if ($scope.materaldocuupload.length > 0) {
                                angular.forEach($scope.materaldocuupload, function (itm) {
                                    if (itm.vbabrsF_FilePath !== undefined && itm.vbabrsF_FilePath !== null && itm.vbabrsF_FilePath !== "") {
                                        $scope.uploadarray.push({
                                            VBABRSF_FileName: itm.vbabrsF_FileName,
                                            VBABRSF_FilePath: itm.vbabrsF_FilePath,
                                            VBABRSF_Filedesc: itm.vbabrsF_Filedesc
                                        });
                                    }
                                });
                            }
                            $scope.Arrays = [];
                            var VPAYVOU_ChequeNo = "";
                            if ($scope.GRnDetails != null && $scope.GRnDetails.length > 0) {
                                angular.forEach($scope.GRnDetails, function (comp) {
                                    if (comp.chkitem == true && comp.INVMGRN_PaidAmount > 0) {
                                        $scope.Arrays.push({
                                            INVMS_Id: $scope.obj.INVMS_Id.invmS_Id,
                                            INVMGRN_Id: comp.INVMGRN_Id,
                                            VPAYVOUSGRN_Remarks: comp.VPAYVOU_RemarksGRN,
                                            VPAYVOUSGRN_PaidAmount: comp.INVMGRN_PaidAmount,
                                            VPAYVOUSGRN_ActualTotalAmount: comp.INVMGRN_ActualTotalAmount,
                                           //  VPAYVOUSGRN_ActualTotalAmount: (Number(comp.INVMGRN_ActualTotalAmount) + Number(comp.INVMGRN_PaidAmount)),
                                            VPAYVOUSGRN_BalanceAmount: (Number(comp.INVMGRN_ActualTotalAmount) - Number(comp.INVMGRN_PaidAmount))
                                        });
                                    }
                                });
                            }
                            //adde By Sanjeev
                            $scope.Budgetuploadtemp = [];
                            if ($scope.Budgetupload != null && $scope.Budgetupload.length > 0) {
                                angular.forEach($scope.Budgetupload, function (comp) {
                                    if (comp.VMBDGHD_AmountSpent != null && comp.VMBDGHD_AmountSpent != "") {
                                        $scope.Budgetuploadtemp.push({
                                            VMBDGHD_Id: comp.VMBDGHD_Id,
                                            VMBDGHD_AmountSpent: comp.VMBDGHD_AmountSpent,
                                            VPAYVOUBUD_Remark: comp.VPAYVOUBUD_Remark
                                        });

                                    }

                                });
                            }
                            else {

                            }
                            var VPAYVOU_SelfPayFlg = 0;
                            var VPAYVOU_AppliedAmount = 0;
                            var INVMS_Id = 0;

                            if ($scope.obj.VPAYVOU_SupplierFlg == true) {
                                INVMS_Id = $scope.obj.INVMS_Id.invmS_Id;
                            }
                            if ($scope.obj.VPAYVOU_SelfPayFlg == false) {
                                VPAYVOU_SelfPayFlg = 1;
                            }
                            if ($scope.obj.optionflag == "online") {
                                VPAYVOU_ChequeNo = "";
                            }
                            else {
                                VPAYVOU_ChequeNo = $scope.obj.VPAYVOU_ChequeNo.vpayvoU_ChequeNo;
                                // $scope.obj.VPAYVOU_PaymentMode = "Cheque";
                            }
                            if ($scope.obj.Payble_AppliedAmount != null && $scope.obj.Payble_AppliedAmount > 0) {
                                VPAYVOU_AppliedAmount = $scope.obj.Payble_AppliedAmount;
                            }
                            else {
                                VPAYVOU_AppliedAmount = $scope.obj.VPAYVOU_AppliedAmount;
                            }
                            var fromdate1 = $scope.obj.VPAYVOU_PaymentDate == null ? "" : $filter('date')($scope.obj.VPAYVOU_PaymentDate, "yyyy-MM-dd");
                            var data = {
                                "MI_Id": $scope.obj.MI_Id,
                                "VPAYVOU_Id": $scope.obj.VPAYVOU_Id,
                                "INVMS_Id": INVMS_Id,
                                "VPAYVOU_PaymentDate": fromdate1,
                                "VPAYVOU_AppliedAmount": $scope.obj.VPAYVOU_AppliedAmount,
                                "VPAYVOU_Remarks": $scope.obj.VPAYVOU_Remarks,
                                "VPAYVOU_BankBalalnce": $scope.obj.VPAYVOU_BankBalalnce,
                                "HRMBD_Id": $scope.obj.HRMBD_Id,
                                "VPAYVOU_ChequeNo": VPAYVOU_ChequeNo,
                                "VPAYVOU_TDS": $scope.obj.VPAYVOU_TDS,
                                "VPAYVOU_Penalty": $scope.obj.VPAYVOU_Penaltiy,
                                "VPAYVOU_ToSupplierFlg": $scope.obj.VPAYVOU_ToSupplierFlg,
                                "VPAYVOU_SupplierFlg": $scope.obj.VPAYVOU_SupplierFlg,
                                "VPAYVOU_SelfPayFlg": VPAYVOU_SelfPayFlg,
                                "VPAYVOU_PaymentTo": $scope.obj.VPAYVOU_PaymentTo,
                                "VPAYVOU_ESICAmount": $scope.obj.VPAYVOU_ESICAmount,
                                "VPAYVOU_OtherAmount": $scope.obj.VPAYVOU_OtherAmount,
                                "VPAYVOU_PaymentMode": $scope.obj.VPAYVOU_PaymentMode,
                                "VPAYVOU_PaymentReference": $scope.obj.VPAYVOU_PaymentReference,
                                "VMBDGHD_Id": $scope.obj.VMBDGHD_Id,
                                filelist: $scope.uploadarray,
                                "VoucherGRN": $scope.Arrays,
                                "leafcancelD": $scope.leafcancelD,
                                "BudgetMapings": $scope.Budgetuploadtemp
                            };

                            apiService.create("CheckBookEntry/savedata", data).then(function (promise) {
                                if (promise.msgval == "save") {
                                    swal("Data Saved/Updated Successfully");

                                }
                                else if (promise.msgval == "admin") {
                                    swal("Please Contact Administrtor !");
                                }
                                else if (promise.msgval == "duplicate") {
                                    swal("Record Already Exist !");
                                }
                                $state.reload();
                            });

                        }
                        else {
                            swal("Amount Cancelled !");
                        }
                    }
                );

            }



        };
        $scope.appliedtax = [];
        $scope.fileUploaddata = function (user) {
            $scope.appliedtax = [];
            $scope.materaldocuuploadtwo = [];
            $scope.materaldocuuploadtwo = [{ id: 'materal' }];
            $scope.obj.VPAYVOU_IdTwo = user.VPAYVOU_Id;
            var data = {
                "VPAYVOU_Id": user.VPAYVOU_Id,
                "MI_Id": $scope.obj.MI_Id
            };
            apiService.create("CheckBookEntry/viewdetails", data).then(function (promise) {
                if (promise.getFilesArray != null && promise.getFilesArray.length > 0) {
                    $scope.materaldocuuploadtwo = promise.getFilesArray;
                    angular.forEach($scope.materaldocuuploadtwo, function (dd) {
                        dd.vbabrsF_Filedesc = dd.vpayvouF_Filedesc;
                        dd.vbabrsF_FilePath = dd.vpayvouF_FilePath;
                        dd.vbabrsF_FileName = dd.vpayvouF_FileName;
                    });
                }

                $('#fileUpload').modal('show');
            });
        }
        $scope.savedataBudget = function () {
            $scope.Budgetuploadtemp = [];
            if ($scope.Budgetuploadone != null && $scope.Budgetuploadone.length > 0) {
                angular.forEach($scope.Budgetuploadone, function (comp) {
                    if (comp.VMBDGHD_AmountSpent != null && comp.VMBDGHD_AmountSpent != "") {
                        $scope.Budgetuploadtemp.push({
                            VMBDGHD_Id: comp.VMBDGHD_Id,
                            VMBDGHD_AmountSpent: comp.VMBDGHD_AmountSpent,
                            VPAYVOUBUD_Remark: comp.VPAYVOUBUD_Remark
                        });

                    }

                });
            }
            var data = {
                "VPAYVOU_Id": $scope.obj.VPAYVOU_Idthree,
                //  "VMBDGHD_Id": $scope.obj.VMBDGHD_Idtwo,
                "BudgetMapings": $scope.Budgetuploadtemp
            };
            apiService.create("CheckBookEntry/savedataconfig", data).then(function (promise) {
                if (promise.msg == "admin") {
                    swal("Please Contact Administrator !");
                }
                else if (promise.msg == "save") {
                    swal("Record saved Successfully !");
                    $('#UploadBudget').modal('hide');
                }
                else if (promise.msg == "notsave") {
                    swal("Record Not saved !");
                }
                $scope.obj.VPAYVOU_Idthree = "";
                $scope.obj.VMBDGHD_Idtwo = "";

                $scope.oncmpchange();

            });
        }
        //savedataFile
        $scope.savedataFile = function () {
            $scope.uploadarray = [];

            if ($scope.materaldocuuploadtwo != null && $scope.materaldocuuploadtwo.length > 0) {

                angular.forEach($scope.materaldocuuploadtwo, function (itm) {
                    if (itm.vbabrsF_FilePath !== undefined && itm.vbabrsF_FilePath !== null && itm.vbabrsF_FilePath !== "") {
                        $scope.uploadarray.push({
                            VBABRSF_FileName: itm.vbabrsF_FileName,
                            VBABRSF_FilePath: itm.vbabrsF_FilePath,
                            VBABRSF_Filedesc: itm.vbabrsF_Filedesc
                        });
                    }
                });
                if ($scope.uploadarray != null && $scope.uploadarray.length > 0) {
                    $('#fileUpload').modal('show');
                }
                else {
                    swal(" Upload At least One File !");
                    return;
                }
                var data = {
                    "VPAYVOU_Id": $scope.obj.VPAYVOU_IdTwo,
                    filelist: $scope.uploadarray
                    // "MI_Id": $scope.obj.MI_Id
                };
                apiService.create("CheckBookEntry/sms", data).then(function (promise) {
                    if (promise.msgval == "save") {
                        swal("Record saved Sucessfully ");
                    }
                    else if (promise.msgval == "notsave") {
                        swal("Record Not Saved !");
                    }
                    $scope.materaldocuuploadtwo = [];
                    $scope.materaldocuuploadtwo = [{ id: 'materal' }];
                    $('#fileUpload').modal('hide');
                    $scope.obj.VPAYVOU_IdTwo = "";
                });
            }
            else {
                swal(" Upload At least One File !");
            }
        }
        $scope.Editdata = function (user) {
            $scope.obj.VPAYVOU_PaymentMode = "";
            $scope.appliedtax = [];
            $scope.Budgetupload = [];
            $scope.Budgetupload = [{ id: 'materal' }];
            var data = {
                "VPAYVOU_Id": user.VPAYVOU_Id,
                "MI_Id": $scope.obj.MI_Id
            };
            apiService.create("CheckBookEntry/Editdata", data).then(function (promise) {
                if (promise.editlist !== null) {
                    $scope.editflag = true;
                    $scope.geteditclient = [];
                    $scope.newcaste = [];
                    $scope.clientid = [];
                    var checquenumber = 0;
                    $scope.geteditclient = promise.geteditclient;
                    $scope.obj.VPAYVOU_BankBalalnce = promise.editlist[0].vpayvoU_BankBalalnce;
                    $scope.obj.VPAYVOU_AppliedAmount = promise.editlist[0].vpayvoU_AppliedAmount;
                    $scope.VPAYVOU_ChequeNo = promise.editlist[0].vpayvoU_ChequeNo;
                    $scope.obj.VPAYVOU_Remarks = promise.editlist[0].vpayvoU_Remarks;
                    $scope.obj.VPAYVOU_PaymentDate = new Date(promise.editlist[0].vpayvoU_PaymentDate);
                    $scope.obj.VPAYVOU_Id = promise.editlist[0].vpayvoU_Id;
                    $scope.get_supplier = promise.clientlist;
                    $scope.obj.VPAYVOU_Id = promise.editlist[0].vpayvoU_Id;
                    $scope.clientid = promise.editlist[0].invmS_Id;

                    if (promise.editlist[0].vpayvoU_PaymentTo != null && promise.editlist[0].vpayvoU_PaymentTo != "") {
                        $scope.obj.VPAYVOU_PaymentTo = promise.editlist[0].vpayvoU_PaymentTo;
                        $scope.obj.VPAYVOU_ToSupplierFlg = true;
                    }
                    else {
                        $scope.obj.VPAYVOU_ToSupplierFlg = false;
                    }

                    if (promise.editlist[0].vpayvoU_TDS != null && promise.editlist[0].vpayvoU_TDS > 0) {
                        $scope.obj.TDS = true;
                        $scope.obj.VPAYVOU_TDS = promise.editlist[0].vpayvoU_TDS;

                    }
                    if (promise.editlist[0].vpayvoU_Penalty != null && promise.editlist[0].vpayvoU_Penalty > 0) {
                        $scope.obj.Penaltiy = true;
                        $scope.obj.VPAYVOU_Penaltiy = promise.editlist[0].vpayvoU_Penalty;


                    }
                    if (promise.editlist[0].vpayvoU_ESICAmount != null && promise.editlist[0].vpayvoU_ESICAmount > 0) {
                        $scope.obj.ESICAmount = true;
                        $scope.obj.VPAYVOU_ESICAmount = promise.editlist[0].vpayvoU_ESICAmount;

                    }
                    if (promise.editlist[0].vpayvoU_OtherAmount != null && promise.editlist[0].vpayvoU_OtherAmount > 0) {
                        $scope.obj.OtherAmount = true;
                        $scope.obj.VPAYVOU_OtherAmount = promise.editlist[0].vpayvoU_OtherAmount;

                    }
                    if ($scope.get_supplier != null && $scope.get_supplier.length > 0) {
                        angular.forEach($scope.get_supplier, function (dd) {
                            if (dd.invmS_Id === $scope.clientid) {
                                $scope.obj.INVMS_Id = $scope.geteditclient[0];
                            }
                        });
                        $scope.obj.VPAYVOU_SupplierFlg = true;
                    }
                    else {
                        $scope.obj.VPAYVOU_SupplierFlg = false;
                    }

                    if (promise.getFilesArray != null && promise.getFilesArray.length > 0) {
                        $scope.materaldocuupload = promise.getFilesArray;
                        angular.forEach($scope.materaldocuupload, function (dd) {
                            dd.vbabrsF_Filedesc = dd.vpayvouF_Filedesc;
                            dd.vbabrsF_FilePath = dd.vpayvouF_FilePath;
                            dd.vbabrsF_FileName = dd.vpayvouF_FileName;
                        });
                    }

                    //if (promise.grnDetails != null && promise.grnDetails.length > 0) {
                    //    $scope.GRnDetails = promise.grnDetails;
                    //    angular.forEach($scope.GRnDetails, function (itmtwo) {
                    //        itmtwo.chkitem = false;
                    //    });
                    //}
                    if (promise.alldata != null && promise.alldata.length > 0) {
                        $scope.GRnDetails = promise.alldata;
                        if (promise.grnDetails != null && promise.grnDetails.length > 0) {
                            angular.forEach($scope.GRnDetails, function (itm) {
                                itm.chkitem = false;
                                 angular.forEach(promise.grnDetails, function (itmtwo) {
                                    if (itm.INVMGRN_Id == itmtwo.INVMGRN_Id) {
                                        itm.chkitem = true;
                                        itm.INVMGRN_PaidAmount = itmtwo.INVMGRN_PaidAmount;
                                        itm.VPAYVOU_RemarksGRN = itmtwo.VPAYVOUSGRN_Remarks;
                                     
                                    }
                                   
                                });
                            });
                           
                        }
                    }
                    $scope.banklist = promise.editbanklist;
                    $scope.obj.HRMBD_Id = promise.editlist[0].hrmbD_Id;
                    $scope.CheckbookSource = promise.checkbookSource;

                    var checquenumber = promise.editlist[0].vpayvoU_ChequeNo;
                    if ($scope.CheckbookSource != null && $scope.CheckbookSource.length > 0) {
                        angular.forEach($scope.CheckbookSource, function (dddd) {
                            if (dddd.vpayvoU_ChequeNo === checquenumber) {
                                $scope.obj.VPAYVOU_ChequeNo = $scope.CheckbookSource[0];
                            }
                        });
                    }
                    if (promise.editlist[0].vpayvoU_PaymentMode != "") {
                        $scope.obj.VPAYVOU_PaymentMode = promise.editlist[0].vpayvoU_PaymentMode;
                        $scope.obj.VPAYVOU_PaymentReference = promise.editlist[0].vpayvoU_PaymentReference;
                    }
                    if ($scope.obj.VPAYVOU_ChequeNo != null && $scope.VPAYVOU_ChequeNo != "") {
                        $scope.obj.optionflag = "Cheque";
                    }
                    else {
                        $scope.obj.optionflag = "online";
                    }

                    if (promise.checkBookBudget != null && promise.checkBookBudget.length > 0) {

                        angular.forEach(promise.checkBookBudget, function (dddd) {
                            dddd.VMBDGHD_Id = dddd.vmbdghD_Id;
                            dddd.VMBDGHD_AmountSpent = dddd.vmbdghD_AmountSpent;
                            dddd.VPAYVOUBUD_Remark = dddd.vpayvoubuD_Remark;

                        });
                        $scope.Budgetupload = promise.checkBookBudget;
                    }

                    // $scope.obj.VMBDGHD_Id = promise.editlist[0].vmbdghD_Id;
                    $scope.scroll();
                }
                else if (promise.ismmclT_ClientName == "auturization") {
                    swal(" Already Voucher Pending Approval !");
                }
            });
        };
        $scope.rescheduletwo = function (user) {
            $scope.pdateaa = user.VPAYVOU_PaymentDate;
            $scope.minamess = user.MI_Name;
            if (user.VPAYVOU_StatusFlg == 'Approved') {
                $scope.amtt = user.VPAYVOU_ApprovedAmount;
            }
            else {
                $scope.amtt = user.VPAYVOU_AppliedAmount;
            }
            $scope.Math = window.Math;
            $scope.roundoffttltwo = $scope.Math.round($scope.amtt, 0)
            $scope.roundoffttlone = $scope.Math.round($scope.amtt, 0)
            $scope.wordsss = $scope.amountinwords($scope.roundoffttlone);
            //
            var data = {
                "VPAYVOU_Id": user.VPAYVOU_Id,
                "MI_Id": $scope.obj.MI_Id
            };
            apiService.create("CheckBookEntry/ForgotOTPForEmailval", data).then(function (promise) {
                swal(promise.message);
            });
            $('#myModalotp').modal('show');

        };
        $scope.rescheduleFile = function (user) {
            $scope.fileupload = [];
            var data = {
                "VPAYVOU_Id": user.VPAYVOU_Id

            };
            apiService.create("CheckBookEntry/sms", data).then(function (promise) {
                $scope.fileupload = promise.fileupload;
                if ($scope.fileupload != null && $scope.fileupload.length > 0) {
                    $('#myModalfile').modal('show');
                }
                else {
                    swal("File  Not Upload !");
                    $('#myModalfile').modal('hide');
                }
            });

        };
        $scope.onGRNchange = [];
      
        //Uploaddatass
        $scope.Uploaddatass = function (GRN, chkitem) {
            $scope.grnfile = [];
            // $('#myModaGrnFile').modal('hide');


            $scope.obj.chkitem = chkitem;
            var data = {
                "INVMS_Id": GRN.INVMGRN_Id,
                "optionflag": "2"
            };
            apiService.create("CheckBookEntry/loaddatareport", data).then(function (promise) {
                if (promise.alldata != null && promise.alldata.length > 0) {
                    $scope.grnfile = promise.alldata;
                    $('#myModaGrnFile').modal('show');
                }
                else {
                    swal("Record File Not Found !");
                }
            });
        }
        $scope.reschedule = function (user) {
            var VPAYVOU_TDS = 0; var VPAYVOU_Penalty = 0; var VPAYVOU_OtherAmount = 0;
            var VPAYVOU_ESICAmount = 0;
            $scope.imagenew = $scope.mi_logo;
            $scope.miname = user.MI_Name;
            $scope.pvno = user.VPAYVOU_VoucherNo;
            $scope.chno = user.VPAYVOU_ChequeNo;
            $scope.pdate = user.VPAYVOU_PaymentDate;
            $scope.pdate = user.VPAYVOU_PaymentDate;
            if (user.INVMS_SupplierName != null && user.INVMS_SupplierName != "") {
                $scope.supldetails = user.INVMS_SupplierName;
            }
            else {
                $scope.supldetails = user.VPAYVOU_PaymentTo;
            }

            $scope.bankname = user.HRMBD_BankName;
            $scope.bbacc = user.HRMBD_BankAccountNo;
            $scope.crby = user.createdby;
            if (user.VPAYVOU_TDS != null && user.VPAYVOU_TDS > 0) {
                VPAYVOU_TDS = user.VPAYVOU_TDS;
            }
            if (user.VPAYVOU_Penalty != null && user.VPAYVOU_Penalty > 0) {
                VPAYVOU_Penalty = user.VPAYVOU_Penalty
            }
            if (user.VPAYVOU_OtherAmount != null && user.VPAYVOU_OtherAmount > 0) {
                VPAYVOU_OtherAmount = user.VPAYVOU_OtherAmount;
            }
            if (user.VPAYVOU_ESICAmount != null && user.VPAYVOU_ESICAmount > 0) {
                VPAYVOU_ESICAmount = user.VPAYVOU_ESICAmount;
            }
            if (user.VPAYVOU_StatusFlg == 'Approved') {
                $scope.amtt = user.VPAYVOU_ApprovedAmount + VPAYVOU_TDS + VPAYVOU_Penalty + VPAYVOU_OtherAmount + VPAYVOU_ESICAmount;
            }
            else {
                $scope.amtt = user.VPAYVOU_AppliedAmount + VPAYVOU_TDS + VPAYVOU_Penalty + VPAYVOU_OtherAmount + VPAYVOU_ESICAmount;
            }
            $scope.rmrk = user.VPAYVOU_Remarks;
            $scope.Math = window.Math;
            $scope.roundoffttl = $scope.Math.round($scope.amtt, 0)
            $scope.words = $scope.amountinwords($scope.roundoffttl);
            $('#popup111').modal('show');
        }
        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("asdf").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 10000);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.clientDecative = function (usersem, SweetAlert) {

            var dystring = "";
            if (usersem.VPAYVOU_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (usersem.VPAYVOU_ActiveFlg === false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        var data = {
                            "VPAYVOU_Id": usersem.VPAYVOU_Id,

                        };
                        apiService.create("CheckBookEntry/clientDecative", data).then(function (promise) {
                            if (promise.ismmclT_ClientName == "auturization") {
                                swal("Voucher Already Generated");
                            }
                            else if (promise.returnval === true) {
                                swal("Record " + dystring + "d" + " Successfully!!!");

                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        };
        $scope.materaldocuuploadtwo = [{ id: 'materal' }];
        $scope.addgrnrowsmateral = function () {
            var newItemNo = $scope.materaldocuupload.length + 1;
            if (newItemNo <= 10) {
                $scope.materaldocuuploadtwo.push({ 'id': 'Materal' + newItemNo });
            }
        };
        $scope.removegrnrowsmateral = function (index) {
            var newItemNo = $scope.materaldocuuploadtwo.length - 1;
            $scope.materaldocuuploadtwo.splice(index, 1);


        };

        //adddd
        $scope.materaldocuupload = [{ id: 'materal' }];
        $scope.Budgetupload = [{ id: 'materal' }];

        $scope.addBudget = function () {
            var newItemNo = $scope.Budgetupload.length + 1;
            if (newItemNo <= 10) {
                $scope.Budgetupload.push({ 'id': 'Materal' + newItemNo });
            }
        };
        $scope.removeBudget = function (index) {
            var newItemNo = $scope.Budgetupload.length - 1;
            $scope.Budgetupload.splice(index, 1);


        };
        //Budgetone
        $scope.addBudgetone = function () {
            var newItemNo = $scope.Budgetuploadone.length + 1;
            if (newItemNo <= 10) {
                $scope.Budgetuploadone.push({ 'id': 'Materal' + newItemNo });
            }
        };
        $scope.removeBudgetwo = function (index) {
            var newItemNo = $scope.Budgetuploadone.length - 1;
            $scope.Budgetuploadone.splice(index, 1);


        };
        //Close
        $scope.addgrnrows = function () {
            var newItemNo = $scope.materaldocuupload.length + 1;
            if (newItemNo <= 10) {
                $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
            }
        };
        $scope.removegrnrows = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

            if ($scope.materaldocuupload.length === 0) {               //data
            }
        };



        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input, document) {
            $scope.SelectedFileForUploadzd = input.files;
            $scope.vbabrsF_FileName = input.files[0].name;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 5242880)  // 2097152 bytes = 2MB 
                {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/png" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/jpg" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/JPG" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/pdf" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/msword" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                //5242880
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].size > 5242880) {
                    swal("File size should be less than 5 MB");
                    return;
                }
            }
        };
        function UploaddianmateralPhoto(data) {
            console.log("check Book upload  Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= 1; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
            }

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadtrnsportdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.vbabrsF_FilePath = d;
                    data.vbabrsF_FileName = $scope.vbabrsF_FileName;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        };
        $scope.previewimg_new = function (img) {
            $scope.vbabrsF_FilePath = img;
            var img = $scope.vbabrsF_FilePath;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.vbabrsF_FilePath);
                    $('#myimagePreview').modal('show');
                    //$('#myModalfile').modal('show');
                }
                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                    $window.open($scope.vbabrsF_FilePath);
                }
                else if (lastelement == 'pdf') {
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.vbabrsF_FilePath;
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
            }
            else {
                $window.open($scope.vbabrsF_FilePath)
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interactedone = function (field) {
            return $scope.submitted1;
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.GrnACaluclate = function (cal) {
            cal.chkitem = false;
            if (cal.INVMGRN_TotalAmount < cal.VPAYVOUSGRN_Amount) {
                cal.VPAYVOUSGRN_Amount = "";
                swal('Amount Should Not be greater Than GRN Total Amount');
            }

        }
        $scope.GRNChkKk_Click = function () {
            $scope.detailScheck = [];


        }
        $scope.search = '';


        $scope.amountinwords = function convertNumberToWords(atotalc) {
            var words = new Array();
            words[0] = '';
            words[1] = 'One';
            words[2] = 'Two';
            words[3] = 'Three';
            words[4] = 'Four';
            words[5] = 'Five';
            words[6] = 'Six';
            words[7] = 'Seven';
            words[8] = 'Eight';
            words[9] = 'Nine';
            words[10] = 'Ten';
            words[11] = 'Eleven';
            words[12] = 'Twelve';
            words[13] = 'Thirteen';
            words[14] = 'Fourteen';
            words[15] = 'Fifteen';
            words[16] = 'Sixteen';
            words[17] = 'Seventeen';
            words[18] = 'Eighteen';
            words[19] = 'Nineteen';
            words[20] = 'Twenty';
            words[30] = 'Thirty';
            words[40] = 'Forty';
            words[50] = 'Fifty';
            words[60] = 'Sixty';
            words[70] = 'Seventy';
            words[80] = 'Eighty';
            words[90] = 'Ninety';
            atotalc = atotalc.toString();
            var atemp = atotalc.split(".");
            var number = atemp[0].split(",").join("");
            var n_length = number.length;
            var words_string = "";
            if (n_length <= 9) {
                var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
                var received_n_array = new Array();
                for (var i = 0; i < n_length; i++) {
                    received_n_array[i] = number.substr(i, 1);
                }
                for (var i = 9 - n_length, j = 0; i < 9; i++ , j++) {
                    n_array[i] = received_n_array[j];
                }
                for (var i = 0, j = 1; i < 9; i++ , j++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        if (n_array[i] == 1) {
                            n_array[j] = 10 + parseInt(n_array[j]);
                            n_array[i] = 0;
                        }
                    }
                }
                atotalc = "";
                for (var i = 0; i < 9; i++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        atotalc = n_array[i] * 10;
                    } else {
                        atotalc = n_array[i];
                    }
                    if (atotalc != 0) {
                        words_string += words[atotalc] + " ";
                    }
                    if ((i == 1 && atotalc != 0) || (i == 0 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Crores ";
                    }
                    if ((i == 3 && atotalc != 0) || (i == 2 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Lakhs ";
                    }
                    if ((i == 5 && atotalc != 0) || (i == 4 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Thousand ";
                    }
                    if (i == 6 && atotalc != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                        words_string += "Hundred and ";
                    } else if (i == 6 && atotalc != 0) {
                        words_string += "Hundred ";
                    }
                }
                words_string = words_string.split("  ").join(" ");
            }
            return words_string;
        }
        $scope.RDIchaNge = function () {
            $scope.obj.VPAYVOU_PaymentMode = "";
            $scope.obj.VPAYVOU_ChequeNo = $scope.CheckbookSource[0];
            $scope.leafcancelD = [];
            if ($scope.ModeOfPayment != null && $scope.ModeOfPayment.length > 0 && $scope.obj.optionflag == "Cheque") {
                angular.forEach($scope.ModeOfPayment, function (itm) {
                    if (itm.ivrmmoD_ModeOfPayment == "Cheque") {
                        $scope.obj.VPAYVOU_PaymentMode = itm.ivrmmoD_ModeOfPayment;
                        return;
                    }
                });
            }
        }
        // template rtgs
        $scope.objdata = [];
        $scope.viewdocument = function (user) {
            $scope.Priniting = [];
            $scope.companyName = "";
            var data = {
                "VPAYVOU_Id": user.VPAYVOU_Id,
                "MI_Id": user.MI_Id,
                "HRMBD_Id": user.HRMBD_Id,
                "ISMMCLT_ClientName": "Cheque",
            }
            apiService.create("CheckBookEntry/viewuploadflies", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.allcompany != null && promise.allcompany.length > 0) {
                        $scope.Priniting = promise.allcompany;
                        $scope.companyName = user.MI_Name;
                        $('#viewPrinting').modal('show');

                    }
                    else {
                        swal("Temaplate Is Not Found !");
                        $('#viewPrinting').modal('hide');
                    }


                }
            });
        };
        //viewdocumentTwo
        $scope.viewdocumentTwo = function (user) {
            $scope.PrinitingData = [];
            $scope.companyName = "";
            $scope.roundoffttl = "";
            var data = {
                "VPAYVOU_Id": user.vpayvoU_Id,
                "MI_Id": user.mI_Id,
                "ISMMCLT_ClientName": "",
            }
            apiService.create("CheckBookEntry/viewuploadflies", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.editfiles != null && promise.editfiles.length > 0) {
                        $scope.alldataa = promise.editfiles;
                        var e1 = angular.element(document.getElementById("report"));
                        $compile(e1.html(user.vcpseT_PrintingTemplateId))(($scope));
                        $('#popup11').modal('show');
                        $('#viewPrinting').modal('hide');
                        $scope.Math = window.Math;
                        $scope.roundoffttl = $scope.Math.round($scope.alldataa[0].VPAYVOU_ApprovedAmount, 0)
                        $scope.words = $scope.amountinwords(Number($scope.roundoffttl));
                    }
                    else {
                        swal("Temaplate Is Not Found !");
                        $('#popup11').modal('hide');
                        $('#viewPrinting').modal('show');
                    }


                }
            });
        };
        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()" onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        $scope.ViewB = function (user) {
            $scope.CheckBookBudgetlist = [];
            $scope.imagename = "";
            var data = {
                "message": "ViewApproval",
                "VPAYVOU_Id": user.VPAYVOU_Id,

            };
            apiService.create("CheckBookEntry/getreport", data).then(function (promise) {
                if (promise.alldata != null && promise.alldata.length > 0) {
                    $scope.CheckBookBudgetlist = promise.alldata;
                    $scope.imagename = promise.mI_logo;
                    $scope.VMBDGHD_BudgetHead = promise.alldata[0].VMBDGHD_BudgetHead;
                    $scope.VMBDGHDAMT_Amount = promise.alldata[0].VMBDGHDAMT_Amount;
                    $scope.IMFY_FinancialYear = promise.alldata[0].IMFY_FinancialYear;
                    $scope.VMBDGHDAMT_MonthwiseAmount = promise.alldata[0].VMBDGHDAMT_MonthwiseAmount;
                    var TotalApproved = 0;
                    if ($scope.CheckBookBudgetlist != null && $scope.CheckBookBudgetlist.length > 0) {
                        for (var i = 0; i < $scope.CheckBookBudgetlist.length; i++) {
                            if ($scope.CheckBookBudgetlist[i].VPAYVOU_ApprovedAmount > 0) {
                                TotalApproved += (Number($scope.CheckBookBudgetlist[i].VPAYVOU_ApprovedAmount));
                            }
                            else {
                                TotalApproved += (Number($scope.CheckBookBudgetlist[i].VPAYVOU_AppliedAmount));
                            }
                        }
                    }

                    $scope.TotalApproved = TotalApproved;
                    $('#CheckBookBudget').modal('show');

                }
                else {
                    swal("Budget Is Not Updated !");
                }
            });
        }
        //Moovefile
        $scope.Moovefile = function () {
            if ($scope.grnfile != null && $scope.grnfile.length > 0 && $scope.obj.chkitem == true) {
                angular.forEach($scope.grnfile, function (itm) {
                    $scope.materaldocuupload.push({
                        vpayvouF_FileName: itm.INVMGRNF_FileName,
                        vbabrsF_FilePath: itm.INVMGRNF_FilePath,
                        vbabrsF_Filedesc: itm.INVMGRNF_Filedesc
                    });

                });
                $('#myModaGrnFile').modal('hide');
            }
            else {
                swal("Please Select Grn Check Box  !");
            }
        }
        //Validationddd
        $scope.Validationddd = function (grn) {
            $scope.obj.TotalAmmountGrn = 0;
            if ((Number($scope.obj.Payble_AppliedAmount)) < (Number(grn.INVMGRN_PaidAmount))) {
                swal("Payble Amount Not Greater Than GRN Amount !");
                grn.INVMGRN_PaidAmount = "";
                return;
            }
            angular.forEach($scope.GRnDetails, function (itm) {
                if (itm.chkitem == true) {
                  //  $scope.obj.TotalAmmountGrn = $scope.obj.TotalAmmountGrn + itm.INVMGRN_PaidAmount;
                    $scope.obj.TotalAmmountGrn = (Number($scope.obj.TotalAmmountGrn)) + (Number(itm.INVMGRN_PaidAmount));
                }
            });
          //$scope.obj.TotalAmmountGrn
            if ((Number($scope.obj.Payble_AppliedAmount)) < (Number($scope.obj.TotalAmmountGrn))) {
                swal("Payble Amount Not Greater Than GRN Amount !");
                grn.INVMGRN_PaidAmount = "";
                return;
            }
        }
    }
})();
