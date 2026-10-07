(function () {
    'use strict';
    angular.module('app').controller('CheckBookPrintEDController', CheckBookPrintEDController)
    CheckBookPrintEDController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function CheckBookPrintEDController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        $scope.searchchkbx = "";
        $scope.editflag = false;
        $scope.submitted = false;
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
        if (ivrmcofigsettings != null && i && ivrmcofigsettings.length > 0) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }
        // $('#myModalotp').modal('show');
        $scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPage2 = paginationformasters;
        $scope.currentPage = 1;
        $scope.currentPage2 = 1;
        $scope.totalgrid = [];
        $scope.submitted = false;
        $scope.bankarrayll = [];
        $scope.mainbalance = 0;
        //$scope.plMaxdate = new Date();
        //$scope.plMaxdate.setDate($scope.plMaxdate.getDate());
        $scope.obj = {};
        $scope.HRMBD_Idtwo = "";
        $scope.MI_Idttwo = "";
        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        };
        $scope.nameTChange = function () {
            if ($scope.PayName != null && $scope.PayName != "") {

            }
            else {

                $scope.clientname = $scope.paynametwo;
            }
        }
        $scope.oncmpchange = function () {


            var data = {
                "MI_Id": $scope.MI_Id,
                //"optionflag": $scope.obj.optionflag
            };
            apiService.create("CheckBookPrint/companychange", data).then(function (promise) {
                $scope.checkbooklist = promise.alldata;

            });
        }
        $scope.loaddata = function () {
            $scope.ISMMCLT_Id = "";
            $scope.ISMMPR_Id = "";
            $scope.ISMCLTPRP_Year = "";
            $scope.FTI_Id = "";
            $scope.ISMCLTPRP_InstallmentAmt = "";
            $scope.VPAYVOU_ChequePrintedDate = null;
            $scope.ismcltprP_Id = 0;
            $scope.projectlist = [];
            $scope.editflag = false;
            var pageid = 2;
            apiService.getURI("CheckBookPrint/loaddata", pageid).then(function (promise) {
                $scope.allcompany = promise.allcompany;
                $scope.userName = promise.username;
            });
        };
        $scope.edit = function (editid) {
            $scope.banknameselection = "";
            $scope.AccPayName = "";
            $scope.transrowsedit = [];
            $scope.MI_Id = editid.MI_Id;
            $scope.VPAYVOU_Id = editid.VPAYVOU_Id;
            $scope.cname = editid.MI_Name;
            $scope.staffn = editid.EmpName;
            $scope.cityname = editid.CityName;
            $scope.HRMBD_Idtwo = editid.HRMBD_Id;
            $scope.MI_Idttwo = editid.MI_Id;
            if (editid.INVMS_SupplierName != null && editid.INVMS_SupplierName != "") {
                $scope.clientname = editid.INVMS_SupplierName;
            }
            else {
                $scope.clientname = editid.VPAYVOU_PaymentTo;
            }

            $scope.paynametwo = editid.INVMS_SupplierName;;
            $scope.inddate = new Date(editid.VPAYVOU_PaymentDate);
            $scope.frmdate = new Date(editid.VTADAA_FromDate);
            $scope.todate = new Date(editid.VTADAA_ToDate);
            $scope.toadress = editid.VTADAA_ToAddress;
            $scope.remarks = editid.VPAYVOU_Remarks;
            $scope.aplamount = editid.VPAYVOU_AppliedAmount;
            $scope.level = editid.SanctionLevelNo;
            $scope.HRME_Id = editid.HRME_Id;
            $scope.vno = editid.VPAYVOU_VoucherNo;
            $scope.bbname = editid.HRMBD_BankName;
            $scope.acno = editid.HRMBD_BankAccountNo;
            $scope.blamt = editid.VPAYVOU_BankBalalnce;
            $scope.transrowsedit = [];
            $scope.sactamt = editid.VPAYVOU_ApprovedAmount;
            $scope.prflag = false;
            $scope.editS = true;
            $scope.editflag = true;
            var pi_id = editid.INVMPI_Id;
            $scope.words1 = $scope.amountinwords($scope.sactamt);
            $scope.words1 += "Rupees Only";
            ///check print
            $scope.pdateaa = new Date(editid.VPAYVOU_PaymentDate);
            //  $scope.minamess = editid.INVMS_SupplierName;
            if (editid.VPAYVOU_StatusFlg == 'Approved') {
                $scope.amtt = editid.VPAYVOU_ApprovedAmount;
            }
            else {
                $scope.amtt = editid.VPAYVOU_ApprovedAmount;
            }
            $scope.ChequeNo = editid.VPAYVOU_ChequeNo;
            $scope.Math = window.Math;
            var roundoffttltwo = $scope.Math.round($scope.amtt, 0);
            //  $scope.roundoffttltwo = $scope.Math.round($scope.amtt,0);
            $scope.roundoffttlone = $scope.Math.round($scope.amtt, 0);
            $scope.wordsss = $scope.amountinwords($scope.roundoffttlone);
            $scope.roundoffttltwo = roundoffttltwo + '.00/-';
            //" + dystring + "
            $scope.scroll();

        };
        $scope.PayFlag = function () {
            $scope.AccPayName = "";
            if ($scope.Selfpay == true) {
                $scope.Accountpay = false;
                $scope.Selfpay = true;
                $scope.AccPayName = "SELF PAY";
            }
            else {
                $scope.Selfpay = false;
                $scope.Accountpay = true;
                $scope.AccPayName = "ACCOUNT PAY";
            }

        }

        $scope.PayFlag1 = function () {

            if ($scope.Accountpay == true) {
                $scope.Selfpay = false;
                $scope.Accountpay = true;
                $scope.AccPayName = "ACCOUNT PAY";
            }
            else {
                $scope.Accountpay = false;
                $scope.Selfpay = true;
                $scope.AccPayName = "SELF PAY";
            }

        }
        $scope.appliedtax = [];
        $scope.PayName = false;
        $scope.reschedule = function (user) {
            $scope.imagenew = user.MI_Logo;
            $scope.miname = user.MI_Name;
            $scope.pvno = user.VPAYVOU_VoucherNo;
            $scope.pdate = user.VPAYVOU_PaymentDate;
            $scope.pdate = user.VPAYVOU_PaymentDate;
            $scope.supldetails = user.INVMS_SupplierName;
            $scope.bankname = user.HRMBD_BankName;
            $scope.bbacc = user.HRMBD_BankAccountNo;
            if (user.VPAYVOU_StatusFlg == 'Approved') {
                $scope.amtt = user.VPAYVOU_ApprovedAmount;
            }
            else {
                $scope.amtt = user.VPAYVOU_AppliedAmount;
            }
            $scope.rmrk = user.VPAYVOU_Remarks;


            $scope.Math = window.Math;
            $scope.roundoffttl = $scope.Math.round($scope.amtt, 0)
            $scope.words = $scope.amountinwords($scope.roundoffttl);
            $('#popup111').modal('show');



        }
        $scope.printDataV = function () {
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

        $scope.printData = function () {
            var innerContents = "";
            if ($scope.banknameselection == "HDFC") {
                innerContents = document.getElementById("printmodalnew").innerHTML;
            }
            else if ($scope.banknameselection == "BOB") {
                innerContents = document.getElementById("BOB").innerHTML;
            }
            else if ($scope.banknameselection == "BOI") {
                innerContents = document.getElementById("BOI").innerHTML;
            }

            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 10000);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.InterChangeCheque = function () {
            var data = {
                "EMAILOTP": $scope.EMAILOTP,
                "HRMDES_Id": $scope.MI_Idttwo,
            };
            apiService.create("CheckBookEntry/VerifyEmailOtpgen", data).then(function (promise) {
                if (promise.message !== null && promise.message === "Success") {
                    $scope.ChequeNo = "";
                    var VPAYVOU_ChequeNo = $scope.obj.VPAYVOU_ChequeNo.vpayvoU_ChequeNo;
                    $scope.ChequeNo = VPAYVOU_ChequeNo;
                    $('#UploadBudget').modal('hide');
                }
                else {
                    swal(promise.message);
                }

            });
        }

        $scope.search = '';
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return ($filter('date')(obj.ismcltprP_PaymentDate, 'dd/MM/yyyy').indexOf($scope.search) >= 0) ||
                (JSON.stringify(obj.ismcltprP_InstallmentAmt)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.ismmclT_ClientName)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.ismmpR_ProjectName)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.asmaY_Year)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.ismcltprP_InstallmentName)).indexOf(angular.lowercase($scope.search)) >= 0;
        };

        $scope.amountinwords = function convertNumberToWords(atotalc) {            var words = new Array();            words[0] = '';            words[1] = 'One';            words[2] = 'Two';            words[3] = 'Three';            words[4] = 'Four';            words[5] = 'Five';            words[6] = 'Six';            words[7] = 'Seven';            words[8] = 'Eight';            words[9] = 'Nine';            words[10] = 'Ten';            words[11] = 'Eleven';            words[12] = 'Twelve';            words[13] = 'Thirteen';            words[14] = 'Fourteen';            words[15] = 'Fifteen';            words[16] = 'Sixteen';            words[17] = 'Seventeen';            words[18] = 'Eighteen';            words[19] = 'Nineteen';            words[20] = 'Twenty';            words[30] = 'Thirty';            words[40] = 'Forty';            words[50] = 'Fifty';            words[60] = 'Sixty';            words[70] = 'Seventy';            words[80] = 'Eighty';            words[90] = 'Ninety';            atotalc = atotalc.toString();            var atemp = atotalc.split(".");            var number = atemp[0].split(",").join("");            var n_length = number.length;            var words_string = "";            if (n_length <= 9) {                var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);                var received_n_array = new Array();                for (var i = 0; i < n_length; i++) {                    received_n_array[i] = number.substr(i, 1);                }                for (var i = 9 - n_length, j = 0; i < 9; i++ , j++) {                    n_array[i] = received_n_array[j];                }                for (var i = 0, j = 1; i < 9; i++ , j++) {                    if (i == 0 || i == 2 || i == 4 || i == 7) {                        if (n_array[i] == 1) {                            n_array[j] = 10 + parseInt(n_array[j]);                            n_array[i] = 0;                        }                    }                }                atotalc = "";                for (var i = 0; i < 9; i++) {                    if (i == 0 || i == 2 || i == 4 || i == 7) {                        atotalc = n_array[i] * 10;                    } else {                        atotalc = n_array[i];                    }                    if (atotalc != 0) {                        words_string += words[atotalc] + " ";                    }                    if ((i == 1 && atotalc != 0) || (i == 0 && atotalc != 0 && n_array[i + 1] == 0)) {                        words_string += "Crores ";                    }                    if ((i == 3 && atotalc != 0) || (i == 2 && atotalc != 0 && n_array[i + 1] == 0)) {                        words_string += "Lakhs ";                    }                    if ((i == 5 && atotalc != 0) || (i == 4 && atotalc != 0 && n_array[i + 1] == 0)) {                        words_string += "Thousand ";                    }                    if (i == 6 && atotalc != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {                        words_string += "Hundred and ";                    } else if (i == 6 && atotalc != 0) {                        words_string += "Hundred ";                    }                }                words_string = words_string.split("  ").join(" ");            }            return words_string;        }

        $scope.UploadCheque = function () {
            $scope.bomlist = [];
            $scope.CheckbookSource = [];
            $scope.ChecqRemarkOld = "";
            $scope.forgetEmail_id = "";
            $scope.obj.VPAYVOU_ChequeNo = $scope.CheckbookSource[0];
            var data = {
                "HRMBD_Id": $scope.HRMBD_Idtwo,
                "MI_Id": $scope.MI_Idttwo,
                "optionflag": "OTPVeriFie"
            };
            apiService.create("CheckBookEntry/getbom", data).then(function (promise) {
                if (promise.checkbookSource != null && promise.checkbookSource.length > 0) {

                    $scope.CheckbookSource = promise.checkbookSource;
                    $scope.forgetEmail_id = promise.emaiL_Id;

                    for (var i = 0; i < 1; i++) {
                        $scope.obj.VPAYVOU_ChequeNo = $scope.CheckbookSource[0];
                    }
                    $('#UploadBudget').modal('show');
                    swal(promise.message);
                }
                else {
                    $('#UploadBudget').modal('hide');
                    swal("Cheque Number Not Found !");
                }

            });

        }
        $scope.forgortpwdprint = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.forgetEmail_id = "";
                $scope.EMAILOTP = "";
                var data = {
                    "VPAYVOU_Id": $scope.VPAYVOU_Id,
                    "MI_Id": $scope.MI_Id,
                };
                apiService.create("CheckBookPrint/ForgotOTPForEmailval", data).then(function (promise) {
                    if (promise.message !== null && promise.message !== "") {
                        // swal(promise.message);
                        $scope.forgetEMaildisable = true;
                        $scope.forgetEmail_id = promise.emaiL_Id;
                    }
                });
                $('#OTPgenration').modal('show');

            }
        };
        $scope.VERIFIEOTP = function () {
            // $scope.forgetEmail_id = "";
            $scope.EMAILOTP = "";
            var data = {
                "VPAYVOU_Id": $scope.VPAYVOU_Id,
                "MI_Id": $scope.MI_Id,
            };
            apiService.create("CheckBookEntryApproval/ForgotOTPForEmailval", data).then(function (promise) {
                if (promise.message !== null && promise.message !== "") {
                    //swal(promise.message);
                    $scope.forgetEMaildisable = true;
                    $scope.forgetEmail_id = promise.emaiL_Id;
                }
            });
            $('#OTPgenration').modal('show');
        }
        $scope.sendotpsms = function () {
            var message = "Success";
            var VPAYVOU_ChequeNo = "";
            //if ($scope.obj.VPAYVOU_ChequeNo.vpayvoU_ChequeNo != null && $scope.obj.VPAYVOU_ChequeNo.vpayvoU_ChequeNo != "") {
            //    VPAYVOU_ChequeNo = $scope.obj.VPAYVOU_ChequeNo.vpayvoU_ChequeNo;
            //}
           
          
            var data = {
                "EMAILOTP": 123456,
                "VPAYVOU_Id": $scope.VPAYVOU_Id,
                "password": $scope.Password,
                "VPAYVOU_ChequePrintedDate": new Date($scope.VPAYVOU_ChequePrintedDate).toDateString(),
                "message": message,
                "AccPayName": $scope.AccPayName,
                "VPAYVOU_ChequeNo": VPAYVOU_ChequeNo,
                "VPAYVOU_Remarks": $scope.ChecqRemarkOld
            };
            apiService.create("CheckBookPrint/VerifyEmailOtpgen", data).then(function (promise) {
                if (promise !== null) {
                    //   if (promise.message === "Success") {
                    var doob = $scope.VPAYVOU_ChequePrintedDate == null ? "" : $filter('date')($scope.VPAYVOU_ChequePrintedDate, "yyyy-MM-dd");
                    var doobyr = doob.substring(0, 4);
                    var doobmnth = doob.substring(5, 7);
                    var doobdays = doob.substring(8, 10);
                    $scope.b1 = doobdays.substring(0, 1);
                    $scope.b2 = doobdays.substring(1, 2);
                    $scope.b3 = doobmnth.substring(0, 1);
                    $scope.b4 = doobmnth.substring(1, 2);
                    $scope.b5 = doobyr.substring(0, 1);
                    $scope.b6 = doobyr.substring(1, 2);
                    $scope.b7 = doobyr.substring(2, 3);
                    $scope.b8 = doobyr.substring(3, 4);
                    $scope.minamess = $scope.clientname;
                    $scope.forgetEMaildisable = false;
                    $scope.forgetEmail_id = "";
                    $scope.EMAILOTP = "";
                    $('#OTPgenration').modal('hide');
                    $('#myModalotp').modal('show');
                    $scope.checkbooklist = [];
                    $scope.transrowsedit = [];
                    $scope.VPAYVOU_Id = "";;
                    $scope.cname = "";
                    $scope.staffn = "";
                    $scope.cityname = "";
                    $scope.clientname = "";
                    $scope.inddate = "";
                    $scope.frmdate = "";
                    $scope.todate = "";
                    $scope.toadress = "";
                    $scope.remarks = "";
                    $scope.aplamount = "";
                    $scope.level = "";
                    $scope.HRME_Id = "";
                    $scope.vno = "";
                    $scope.bbname = "";
                    $scope.acno = "";
                    $scope.blamt = "";
                    $scope.sactamt = "";;
                    $scope.pdateaa = "";
                    $scope.editflag = false;
                    $scope.MI_Id = "";
                    $scope.Selfpay = false;
                    $scope.Accountpay = false;
                    $scope.submitted = false;
                }
            });
        };
    }
})();
