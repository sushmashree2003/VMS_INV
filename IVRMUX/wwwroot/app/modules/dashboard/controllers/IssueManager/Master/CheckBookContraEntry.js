(function () {
    'use strict';
    angular.module('app').controller('CheckBookContraEntryController', CheckBookContraEntryController)
    CheckBookContraEntryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q','$window']
    function CheckBookContraEntryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, $window) {
        
        $scope.transnumbconfigurationsettingsassign = {};      
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
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }

        $scope.search = "";
      
        $scope.newdate = new Date();
        $scope.searchchkbx = "";
        $scope.obj.optionflag = "";
        $scope.submitted = false;
        $scope.editflag = false;
        $scope.maxdate = new Date();
        var VPAYVOU_AppliedAmount = 0;
        $scope.plMaxdate = new Date();
        $scope.plMaxdate.setDate($scope.plMaxdate.getDate());
        $scope.obj.VPAYVOU_PaymentDate = new Date();
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = paginationformasters;    
        $scope.submitted = false;           
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        };
        $scope.obj = {};
        $scope.RDIchaNge = function () {
            $scope.obj.VBABRS_Amount = "";
            $scope.obj.VBABRS_BankBalalnce = "";
            $scope.obj.VBABRS_Amounttwo = "";
            $scope.obj.VBABRS_BankBalalnceTWO = "";
        }
        $scope.balchange = function () {
            $scope.VPAYVOU_AppliedAmount = 0;
        }
        $scope.amtchangetwo = function () {

            if ($scope.obj.VBABRS_Amounttwo > 0) {
                if ($scope.obj.VBABRS_Amounttwo < VPAYVOU_AppliedAmount && $scope.editflag == true) {
                    $scope.obj.VBABRS_Amounttwo = VPAYVOU_AppliedAmount;
                   // $scope.obj.VBABRS_Amounttwo = 0;
                    swal('Already ' + VPAYVOU_AppliedAmount +'  Voucher Generated !');
                }
            }
            if ($scope.editflag == true && $scope.obj.VBABRS_Amounttwo == 0) {
                $scope.obj.VBABRS_Amounttwo = VPAYVOU_AppliedAmount;
            }
        }
                 
        $scope.oncmpchange = function () {
            $scope.cleardata();
            $scope.alldata = [];
        
            $scope.obj.HRMBD_IdTWO = "";
            $scope.obj.HRMBD_IdThree = "";
            var data = {
                "MI_Id": $scope.obj.MI_Id
            };
            apiService.create("CheckBankBalance/companychange", data).then(function (promise) {                         
                $scope.banklist = promise.banklist;
                $scope.alldatatwo = promise.alldatatwo;
            });
        }
        $scope.viewCheckBook = function () {
            $('#chequecancel').modal('show');
        }
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

        $scope.loaddata = function () {
            $scope.obj.MI_Id = "";    
            var pageid = 2;
            apiService.getURI("CheckBankBalance/loaddata", pageid).then(function (promise) {
                $scope.allcompany = promise.allcompany;


            });
        };
        $scope.BankFroamChange = function () {
            var Message = "";
            $scope.obj.VBABRS_BankBalalnceTWO = "";
            $scope.banklistTwo = [];
            $scope.CheckbookSource = [];
            $scope.obj.HRMBD_IdThree = "";
            if ($scope.banklist != null && $scope.banklist.length > 0) {
                for (var i = 0; i < $scope.banklist.length; i++) {
                    if ($scope.banklist[i].hrmbD_Id == $scope.obj.HRMBD_IdTWO) {
                        Message = $scope.banklist[i].hrmbD_BankName + '/' + $scope.banklist[i].hrmbD_BankAccountNo;
                    }
                }
            }
            var data = {
                "HRMBD_Id": $scope.obj.HRMBD_IdTWO,
                "MI_Id": $scope.obj.MI_Id,
                "VPAYVOU_Id": $scope.obj.VPAYVOU_Id
            };
            apiService.create("CheckBankBalance/getinvoiceno", data).then(function (promise) {
                if (promise.banklistTwo != null && promise.msg != null && promise.banklistTwo.length > 0 && promise.msg =="OB") {                    
                    $scope.banklistTwo = promise.banklistTwo;
                    $scope.ModeOfPayment = promise.modeOfPayment;
                    $scope.CheckbookSource = promise.checkbookSource;
                    $scope.obj.VPAYVOU_ChequeNo = $scope.CheckbookSource[0];
                    $scope.obj.VBABRS_BankBalalnceTWO = promise.projectlistTWO[0].vbabrS_BankBalalnce;
                }
                else {
                    if (Message != null && Message != "") {
                        swal("Must Be Entered  : " + Message + " - Opening Balnace !");
                    }
                    $scope.obj.HRMBD_IdTWO = "";
                    
                    //swal("Bank  Amount Not Be Zero !");
                }


            });


        }
        $scope.clientdetails = [];
        $scope.BankToChange = function () {
            var Message = "";
            $scope.obj.VBABRS_BankBalalnceThree = "";
            if ($scope.banklistTwo != null && $scope.banklistTwo.length > 0) {
                for (var i = 0; i < $scope.banklistTwo.length; i++) {
                    if ($scope.banklistTwo[i].hrmbD_Id == $scope.obj.HRMBD_IdThree) {
                        Message = $scope.banklistTwo[i].hrmbD_BankName + '/' + $scope.banklistTwo[i].hrmbD_BankAccountNo;
                    }
                }
            }
            var data = {
                "HRMBD_Id": $scope.obj.HRMBD_IdThree,
                "MI_Id": $scope.obj.MI_Id
            };
            apiService.create("CheckBankBalance/getinvoiceno", data).then(function (promise) {
                if (promise.projectlistTWO != null && promise.msg != null && promise.projectlistTWO.length > 0 && promise.msg == "OB") {
                    $scope.obj.VBABRS_BankBalalnceThree = promise.projectlistTWO[0].vbabrS_BankBalalnce;

                }
                else {
                    if (Message != null && Message != "") {
                        swal("Must Be Entered  : " + Message + " - Opening Balnace !");
                    }
                    $scope.obj.VBABRS_BankBalalnceThree = 0;
                    $scope.obj.HRMBD_IdThree = "";
                }


            });
        };
        $scope.savedataModal = function () {
            $scope.submitted1 = true;
            if ($scope.myFormone.$valid) {
                $('#chequecancel').modal('hide');
            }
        }      
        $scope.savedata = function () {          
            var VPAYVOU_PaymentMode = "Cheque";
            $scope.obj.optionflag = "bnktobnk";           
            var Debited = "Transfer Bank Amount ?";
            var dystring = $scope.obj.VBABRS_Amounttwo;
            $scope.uploadarray = [];
            if ($scope.myForm.$valid) {
                swal({
                    title: "Are you sure?",
                    text: "Do You Want To " + dystring + " " + Debited + "",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes," + Debited + "",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
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
                            var data = {
                                "MI_Id": $scope.obj.MI_Id,
                                "HRMBD_Id": $scope.obj.HRMBD_IdTWO,
                                "VPAYVOU_Remarks": $scope.obj.VBABRS_RemarksTwo,                                
                                "optionflag": $scope.obj.optionflag,
                                "VPAYVOU_HRMBD_Id": $scope.obj.HRMBD_IdThree,
                                "VPAYVOU_Id": $scope.obj.VPAYVOU_Id,
                                "VPAYVOU_PaymentDate": new Date($scope.obj.VPAYVOU_PaymentDate).toDateString(),
                                "VPAYVOU_AppliedAmount": $scope.obj.VBABRS_Amounttwo,
                                "VPAYVOU_BankBalalnce": $scope.obj.VBABRS_BankBalalnceTWO,
                                "VPAYVOU_ChequeNo": $scope.obj.VPAYVOU_ChequeNo.vpayvoU_ChequeNo,
                                "VPAYVOU_PaymentTo": $scope.obj.VPAYVOU_PaymentTo,
                                "VPAYVOU_PaymentMode": $scope.obj.VPAYVOU_PaymentMode,
                                "VPAYVOU_PaymentReference": $scope.obj.VPAYVOU_PaymentReference,
                              //  transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,                                                             
                                "leafcancelD": $scope.leafcancelD,
                                "VPAYVOU_PaymentMode": VPAYVOU_PaymentMode,
                                filelist: $scope.uploadarray
                            };

                            apiService.create("CheckBookEntry/savedata", data).then(function (promise) {
                                if (promise.msgval == "save") {
                                    swal("Data Saved Successfully");

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
                            swal("Amount Update Cancelled !");
                        }
                    }
                );
            }
            else {
                $scope.submitted = true;
            }

        };
        $scope.appliedtax = [];
        $scope.Editdata = function (user) {
            $scope.obj.VPAYVOU_PaymentMode = "";
            $scope.appliedtax = [];
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
                    $scope.obj.VPAYVOU_Id = promise.editlist[0].vpayvoU_Id;                    
                    if (promise.getFilesArray != null && promise.getFilesArray.length > 0) {
                        $scope.materaldocuupload = promise.getFilesArray;
                        angular.forEach($scope.materaldocuupload, function (dd) {
                            dd.vbabrsF_Filedesc = dd.vpayvouF_Filedesc;
                            dd.vbabrsF_FilePath = dd.vpayvouF_FilePath;
                            dd.vbabrsF_FileName = dd.vpayvouF_FileName;
                            //dd.VTADAAF_Remarks = dd.vpayvouF_Filedesc;
                            //dd.ismclT_FilePath = dd.vpayvouF_FilePath;
                            //dd.ismclT_FileName = dd.vpayvouF_FileName;
                            //vbabrsF_FileName
                        });
                    }                  
                    $scope.banklist = promise.editbanklist;                   
                    $scope.obj.HRMBD_IdTWO = promise.editlist[0].hrmbD_Id;                 
                    $scope.BankFroamChange();
                    $scope.obj.HRMBD_IdThree = promise.editlist[0].vpayvoU_HRMBD_Id;
                    $scope.BankToChange();
                    $scope.obj.VPAYVOU_PaymentTo = promise.editlist[0].vpayvoU_PaymentTo;  
                    $scope.obj.VBABRS_Amounttwo = promise.editlist[0].vpayvoU_AppliedAmount;  
                    $scope.obj.VBABRS_RemarksTwo = promise.editlist[0].vpayvoU_Remarks;  
                    $scope.CheckbookSource = promise.checkbookSource;                 
                    var checquenumber = promise.editlist[0].vpayvoU_ChequeNo;
                    if ($scope.CheckbookSource != null && $scope.CheckbookSource.length > 0) {
                        angular.forEach($scope.CheckbookSource, function (dddd) {
                            if (dddd.vpayvoU_ChequeNo === checquenumber) {
                                $scope.obj.VPAYVOU_ChequeNo = $scope.CheckbookSource[0];
                            }
                        });
                    }
                   

                    VPAYVOU_AppliedAmount = promise.editlist[0].vpayvoU_AppliedAmount;
                    $scope.scroll();
                }
                else if (promise.ismmclT_ClientName == "auturization") {
                    swal(" Already Voucher Pending Approval !");
                }
            });
        };
        $scope.reschedule = function (user) {
            var data = {
                "ISMINC_Id": user.ISMINC_Id,
                "MI_Id": $scope.obj.MI_Id
            };
            apiService.create("CheckBankBalance/Editdata", data).then(function (promise) {
                $scope.clientdetails = promise.clientlist;
                $scope.imagename = $scope.instlist[0].mI_Logo;
                $scope.miname = $scope.instlist[0].mI_Name;
                $scope.cgst = $scope.instlist[0].mI_GSTNO;
                $scope.ccin = $scope.instlist[0].mI_CINNo;
                $scope.ccontact = "";
                $scope.cmail = "";
                if ($scope.instlistemail != null) {
                    if ($scope.instlistemail.length > 0) {
                        $scope.cmail = $scope.instlistemail[0].miE_EmailId;
                    }
                }
                if ($scope.instlistmobile != null) {
                    if ($scope.instlistmobile.length > 0) {
                        $scope.ccontact = $scope.instlistmobile[0].mipN_PhoneNo;
                    }
                }
                $scope.ISMMCLT_Address = $scope.clientdetails[0].ismmclT_Address;
                $scope.ISMMCLT_Desc = $scope.clientdetails[0].ismmclT_Desc;
                $scope.client = $scope.clientdetails[0].ismmclT_ClientName;
                $scope.clientgst = $scope.clientdetails[0].ismmclT_GSTNO;
                $scope.clmobile = $scope.clientdetails[0].ismmclT_ContactNo;
                $scope.clemail = $scope.clientdetails[0].ismmclT_EmailId;
                $scope.INVMSL_SalesDate = promise.editlist[0].isminC_Date;
                $scope.Remarks = promise.editlist[0].isminC_Remarks;
                $scope.INVMSL_SalesNo = promise.editlist[0].isminC_PrInviceNo
                $scope.ttl = $scope.invmgrN_TotalAmount;
                $scope.get_SaleItemDetails = promise.editlistdetails;
                $scope.appliedtax1 = []
                if (promise.editlisttax != null) {
                    if (promise.editlisttax.length > 0) {
                        angular.forEach(promise.editlisttax, function (pp) {
                            angular.forEach($scope.get_tax, function (ll) {
                                if (pp.invmT_Id == ll.invmT_Id) {
                                    ll.xyz = true;
                                    ll.invmiT_TaxValue = pp.ismintX_TaxPercent;

                                    $scope.appliedtax1.push({ invmT_Id: ll.invmT_Id, invmT_TaxName: ll.invmT_TaxName, taxamount: pp.ismintX_TaxAmount, invmiT_TaxValue: pp.ismintX_TaxPercent });
                                }

                            })

                        })
                    }
                }
                $scope.ttl = promise.editlist[0].isminC_TotalAmount;
                var totaltax = promise.editlist[0].isminC_TotalTaxAmount;
                $scope.masterTax = $scope.appliedtax1;
                $scope.mainttl = $scope.ttl + totaltax;
                $scope.Math = window.Math;
                $scope.roundoffttl = $scope.Math.round($scope.mainttl, 0)
                $scope.words = $scope.amountinwords($scope.roundoffttl);
                $scope.mdate = promise.editlist[0].isminC_MOUDate;
                $scope.mno = promise.editlist[0].isminC_MOURefNo;
                $scope.mof = promise.editlist[0].isminC_ModeOfPayment;

                $scope.bankarray = [];
                if (promise.editlist[0].hrmbD_Id != undefined && promise.editlist[0].hrmbD_Id != null && promise.editlist[0].hrmbD_Id != '') {
                    angular.forEach($scope.banklist, function (dd) {
                        if (dd.hrmbD_Id == promise.editlist[0].hrmbD_Id) {
                            $scope.bankarray.push(dd);
                        }
                    })
                }

                $('#popup111').modal('show');
            });

        }
        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("asdf").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
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
                        apiService.create("CheckBookEntry/clientDecative", usersem).then(function (promise) {
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
        $scope.viewdetails = function (dd) {
            $scope.submitted1 = false;
            $scope.totalgrid = [];
            $scope.totalgrid = [{ id: 'viewdetails1' }];

            $scope.ISMCLTPRP_Id_New = 0;
            var data = {
                "ISMCLTPRP_Id": dd.ismcltprP_Id
            };
            apiService.create("CheckBankBalance/viewdetails", data).then(function (promise) {
                $scope.clientname = dd.ismmclT_ClientName;
                $scope.projectname = dd.ismmpR_ProjectName;
                $scope.yearname = dd.asmaY_Year;
                $scope.installmentname = dd.ismcltprP_InstallmentName;
                $scope.paymentstatusflagnew = dd.paymentstatusflag;
                $scope.installmentamount = dd.ismcltprP_InstallmentAmt;
                $scope.ISMCLTPRP_Id_New = dd.ismcltprP_Id;
                $scope.duedate = new Date(dd.ismcltprP_PaymentDate);

                $scope.paymentmodedetails = promise.paymentmodedetails;
                $scope.getpaymentdetails = promise.getpaymentdetails;
                if ($scope.getpaymentdetails !== null && $scope.getpaymentdetails.length > 0) {
                    $scope.totalgrid = $scope.getpaymentdetails;

                    angular.forEach($scope.totalgrid, function (dd) {
                        dd.ismcppD_ReceivedDate = new Date(dd.ismcppD_ReceivedDate);
                        if (dd.ismcppD_ChequeDate !== null) {
                            dd.ismcppD_ChequeDate = new Date(dd.ismcppD_ChequeDate);
                        }

                        angular.forEach($scope.paymentmodedetails, function (ddd) {
                            if (ddd.ivrmmoD_Id === parseInt(dd.ivrmmoD_Id)) {
                                if (ddd.ivrmmoD_Flag === 'C' || ddd.ivrmmoD_Flag === 'c') {
                                    dd.flag = 0;
                                } else {
                                    dd.flag = 1;
                                }
                            }
                        });
                    });
                }
                $('#myModalreadmitview').modal('show');

            });
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.Clear = function () {
            $state.reload();
        };
        $scope.cleardata = function () {
            $scope.editflag = false;
            $scope.obj.VBABRS_Id = 0;
            $scope.obj.VBABRS_Amount = 0;
            $scope.obj.VBABRS_BankBalalnce = 0;
            $scope.obj.VBABRS_BankBalalnceTWO = 0;
            $scope.obj.HRMBD_Id = "";

            $scope.obj.VBABRS_Remarks = "";
            $scope.obj.VBABRS_Date = null;
            $scope.obj.VBABRS_ClearedDate = null;
            $scope.submitted = false;
            $scope.obj.VBABRS_ClearedFlg = false;
        }
        $scope.search = '';
        $scope.filterValue = function (obj) {
            return ($filter('date')(obj.ismcltprP_PaymentDate, 'dd/MM/yyyy').indexOf($scope.search) >= 0) ||
                (JSON.stringify(obj.ismcltprP_InstallmentAmt)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.ismmclT_ClientName)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.ismmpR_ProjectName)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.asmaY_Year)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.ismcltprP_InstallmentName)).indexOf(angular.lowercase($scope.search)) >= 0;
        };
        //======Amount Convert into word=========//
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
        //==============End==============//
        $scope.materaldocuupload = [{ id: 'materal' }];
        $scope.addgrnrows = function () {
            var newItemNo = $scope.materaldocuupload.length + 1;
            if (newItemNo <= 10) {
                $scope.materaldocuupload.push({ 'id': 'materal' + newItemNo });
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

    }
})();
