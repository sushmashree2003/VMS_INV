(function () {
    'use strict';
    angular
        .module('app')
        .controller('CheckBookEntryApprovalController', CheckBookEntryApprovalController)
    CheckBookEntryApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache','$window']
    function CheckBookEntryApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache, $window) {
        $scope.currentPage = 1;
        $scope.currentPage2 = 1;
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPage2 = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.searchValue = '';
        $scope.maxDatedof = new Date($scope.today);
        $scope.editflags = false;
        $scope.VERIFIEOtpFlag = false;
        $scope.OtpSuccess = false;
        $scope.obj = {};
 
        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        };
        // scope.tablediv = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.loaddata = function () {
            $scope.obj.MI_Id = "";
            var pageid = 1;
            apiService.getURI("CheckBookEntryApproval/getalldata", pageid).
                then(function (promise) {
                    $scope.allcompany = promise.allcompany;
                  
                })
        }

        //oncmpchange
        $scope.oncmpchange = function () {
            $scope.getadvancetada = [];
            var data = {
                "MI_Id": $scope.obj.MI_Id
            }
            apiService.create("CheckBookEntryApproval/ApprovalData", data).
                then(function (promise) {
                    //YearName

                   // 
                    if (promise.getadvancetada != null && promise.getadvancetada.length > 0) {
                        angular.forEach($scope.allcompany, function (dd) {
                            if ($scope.obj.MI_Id == dd.mI_Id) {
                                $scope.YearName = dd.mI_Name;
                            }
                        });
                        $scope.getadvancetada = promise.getadvancetada;
                        angular.forEach($scope.getadvancetada, function (dd) {
                            dd.checked = false;
                            if (dd.HRMBD_InterbankTransFlg == true) {
                                dd.classname = 'net';
                                dd.checked = true;
                            }
                         
                        });
                        $scope.prflag = true;
                        $scope.pppfl = false;
                        var SanctionLevelNo = $scope.getadvancetada[0].SanctionLevelNo;
                        if (SanctionLevelNo == 2) {
                            for (var i = 0; i < $scope.getadvancetada.length; i++) {

                                if ($scope.getadvancetada[i].VPAYVOU_StatusFlg == "Approved") {
                                    $scope.getadvancetada[i].VPAYVOUAUT_SanctionedAmount = $scope.getadvancetada[i].VPAYVOU_ApprovedAmount;
                                    $scope.getadvancetada[i].flag = 1;
                                    var charges = 0;
                                    charges = (Number($scope.getadvancetada[i].VPAYVOU_TDS) + Number($scope.getadvancetada[i].VPAYVOU_Penalty) + Number($scope.getadvancetada[i].VPAYVOU_ESICAmount) + Number($scope.getadvancetada[i].VPAYVOU_OtherAmount));
                                    $scope.getadvancetada[i].VPAYVOU_TotalCharges = (Number(charges)).toFixed(2);
                                    $scope.getadvancetada[i].VPAYVOU_TotalPaybles = (Number($scope.getadvancetada[i].VPAYVOU_AppliedAmount) - (Number($scope.getadvancetada[i].VPAYVOU_TotalCharges)));

                                    //VPAYVOU_TotalCharges
                                }
                                else {
                                    $scope.getadvancetada[i].VPAYVOUAUT_SanctionedAmount = 0;
                                    $scope.getadvancetada[i].flag = 0;
                                    $scope.getadvancetada[i].classname = 'oldd';
                                }
                            }
                        }
                        else {
                            for (var i = 0; i < $scope.getadvancetada.length; i++) {
                                $scope.getadvancetada[i].flag = 1;
                                var charges = 0;
                                charges = (Number($scope.getadvancetada[i].VPAYVOU_TDS) + Number($scope.getadvancetada[i].VPAYVOU_Penalty) + Number($scope.getadvancetada[i].VPAYVOU_ESICAmount) + Number($scope.getadvancetada[i].VPAYVOU_OtherAmount));
                                $scope.getadvancetada[i].VPAYVOU_TotalCharges = (Number(charges)).toFixed(2);
                                $scope.getadvancetada[i].VPAYVOUAUT_SanctionedAmount = (Number($scope.getadvancetada[i].VPAYVOU_AppliedAmount) - (Number($scope.getadvancetada[i].VPAYVOU_TotalCharges )));
                                //VPAYVOU_TotalPaybles
                                $scope.getadvancetada[i].VPAYVOU_TotalPaybles = (Number($scope.getadvancetada[i].VPAYVOU_AppliedAmount) - (Number($scope.getadvancetada[i].VPAYVOU_TotalCharges)));

                            }
                        }

                    }
                    else {
                        $scope.pppfl = true;
                    }
                })
        }
        $scope.reschedule = function (user) {
            var VPAYVOU_TDS = 0; var VPAYVOU_Penalty = 0; var VPAYVOU_OtherAmount = 0;
            var VPAYVOU_ESICAmount = 0;
            $scope.imagenew = user.MI_Logo;
            $scope.miname = user.MI_Name;
            $scope.pvno = user.VPAYVOU_VoucherNo;
            $scope.pdate = user.VPAYVOU_PaymentDate;
            $scope.pdate = user.VPAYVOU_PaymentDate;
            if (user.INVMS_SupplierName != null && user.INVMS_SupplierName != "") {
                $scope.supldetails = user.INVMS_SupplierName;
            }
            else {
                $scope.supldetails = user.VPAYVOU_PaymentTo;
            }

         //   $scope.supldetails = user.INVMS_SupplierName;
            $scope.bankname = user.HRMBD_BankName;
            $scope.bbacc = user.HRMBD_BankAccountNo;
            $scope.chno = user.VPAYVOU_ChequeNo;
            $scope.crby = user.createdby;
            if (user.VPAYVOU_ChequeNo != null && user.VPAYVOU_ChequeNo != "") {
                $scope.chno1 = user.VPAYVOU_ChequeNo;
            }
            else {
                $scope.chno1 = user.VPAYVOU_PaymentReference;

            }
            $scope.pmode = user.VPAYVOU_PaymentMode;
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
                $scope.amtt = (Number(user.VPAYVOU_ApprovedAmount) - Number(VPAYVOU_TDS) - Number(VPAYVOU_Penalty) - Number(VPAYVOU_OtherAmount) - Number(VPAYVOU_ESICAmount));
            }
            else {
                $scope.amtt = (Number(user.VPAYVOU_AppliedAmount) - Number(VPAYVOU_TDS) - Number(VPAYVOU_Penalty) - Number(VPAYVOU_OtherAmount) - Number(VPAYVOU_ESICAmount));
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
        $scope.submitted = false;
      //  var approvecnt = 0;  
        $scope.sendotpsms = function () {
            var data = {
                "EMAILOTP": $scope.EMAILOTP,
                "HRMDES_Id": $scope.obj.MI_Id,              
            };
            apiService.create("CheckBookEntryApproval/VerifyEmailOtpgen", data).then(function (promise) {
                if (promise.message !== null && promise.message === "Success") {
                    $scope.VERIFIEOtpFlag = true;
                    $scope.OtpSuccess = true;
                    $('#OTPgenration').modal('hide');
                   
                }
                else {
                    swal(promise.message);
                }

            });
        };
        $scope.ViewB = function (user) {
            $scope.CheckBookBudgetlist = [];
            $scope.imagename = "";                       
            var data = {
                "message": "ViewApproval",
                "VPAYVOU_Id": user.VPAYVOU_Id,
                //"IMFY_Id": IMFY_Id,

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
                    //swal("Record Not Found !");
                }
            });
        }
        $scope.VERIFIEOTP = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                //$scope.forgetEmail_id = "";
                $scope.EMAILOTP = "";
                var data = {
                    //"VPAYVOU_Id": $scope.VPAYVOU_Id,
                    "HRMDES_Id": $scope.obj.MI_Id,
                };
                apiService.create("CheckBookEntryApproval/ForgotOTPForEmailval", data).then(function (promise) {
                    if (promise.message !== null && promise.message !== "") {
                        swal(promise.message);
                        $scope.forgetEMaildisable = true;
                        $scope.forgetEmail_id = promise.emaiL_Id;

                    }
                });
                $('#OTPgenration').modal('show');
            }

        }
        //=====================Editrecord....
        $scope.edit = function (editid) {
            $scope.MI_Id = editid.MI_Id;
            $scope.VPAYVOU_Id = editid.VPAYVOU_Id;
            $scope.cname = editid.MI_Name;
            $scope.staffn = editid.EmpName;
            $scope.cityname = editid.CityName;
            if (editid.INVMS_SupplierName != null && editid.INVMS_SupplierName != "") {
                $scope.clientname = editid.INVMS_SupplierName;

            }
            else {
                $scope.clientname = editid.VPAYVOU_PaymentTo;
            }

            $scope.inddate = new Date(editid.VPAYVOU_PaymentDate);
            $scope.frmdate = new Date(editid.VTADAA_FromDate);
            $scope.todate = new Date(editid.VTADAA_ToDate);
            $scope.toadress = editid.VTADAA_ToAddress;
            $scope.remarks = editid.VPAYVOU_Remarks;
            $scope.aplamount = editid.VPAYVOU_AppliedAmount;
            $scope.aplamountbank = editid.VPAYVOU_BankBalalnce;
            $scope.level = editid.SanctionLevelNo;
            $scope.HRME_Id = editid.HRME_Id;
            $scope.vno = editid.VPAYVOU_VoucherNo;
            $scope.bbname = editid.HRMBD_BankName;
            $scope.acno = editid.HRMBD_BankAccountNo;
            $scope.blamt = editid.VPAYVOU_BankBalalnce;
            if (editid.VPAYVOU_ChequeNo != null && editid.VPAYVOU_ChequeNo != "") {
                $scope.chno1 = editid.VPAYVOU_ChequeNo;
            }
            else {
                $scope.chno1 = editid.VPAYVOU_PaymentReference;

            }
            $scope.pmode = editid.VPAYVOU_PaymentMode;

            $scope.createdby = editid.createdby;
            $scope.sactamt = editid.VPAYVOU_ApprovedAmount;
            $scope.prflag = false;
            $scope.editS = true;
            var pi_id = editid.INVMPI_Id;
            $scope.imagenew = editid.MI_Logo;
            $scope.GetFile = [];
            var data = {
                "VPAYVOU_Id": editid.VPAYVOU_Id,
                "MI_Id": editid.MI_Id,
            };

            apiService.create("CheckBookEntryApproval/edit", data).then(function (promise) {
                $scope.editarray = promise.editArray;
                var tds = 0; var penalty = 0; var escic = 0; var other = 0;
                tds = promise.editArray[0].VPAYVOU_TDS;
                penalty = promise.editArray[0].VPAYVOU_Penalty;
                escic = promise.editArray[0].VPAYVOU_ESICAmount;
                other = promise.editArray[0].VPAYVOU_OtherAmount;
             
                if (promise.filetwo != null && promise.filetwo.length > 0) {
                    $scope.GetFile = promise.filetwo;
                }
                if (tds != null && tds > 0) {
                    $scope.TDS = tds;
                }
                else {
                    $scope.TDS = 0;
                }
                if (penalty != null && penalty > 0) {
                    $scope.penalty = penalty;

                }
                else {
                    $scope.penalty = 0;
                }
                if (escic != null && escic > 0) {
                    $scope.ESIC = escic;

                }
                else {
                    $scope.ESIC = 0;
                }
                if (other != null && other > 0) {
                    $scope.OtherA = other;

                }
                else {
                    $scope.OtherA = 0;
                }
                $scope.totalappliedamount = parseFloat($scope.aplamount);

                $scope.GRnDetails = promise.gRnDetails;
                if ($scope.GRnDetails != null && $scope.GRnDetails.length > 0) {
                    $scope.VPAYVOUAUT_SanctionedAmount = $scope.totalappliedamount;
                }
                $scope.Tobank = promise.editArray[0].ToBank;
            });
            $scope.editflags = true;
            $scope.VERIFIEOtpFlag = false;
            $scope.OtpSuccess = false;
            $('#ViewDetails').modal('show');

        };
        $scope.cancel = function () {
            $scope.submitted = false;
            $state.reload();

        }
        //closed
        $scope.closed = function () {
         
            $('#myimagePreview').modal('hide');
            $('#ViewDetails').modal('show');
        }
        $scope.allChecks = function (allchecked) {
            if ($scope.getadvancetada != null && $scope.getadvancetada.length > 0) {
                if (allchecked == true) {
                    angular.forEach($scope.getadvancetada, function (dd) {
                        dd.checked = true;
                    });
                }
                else {
                    angular.forEach($scope.getadvancetada, function (dd) {
                        //dd.HRMBD_InterbankTransFlg == true
                        if (dd.HRMBD_InterbankTransFlg === true) {
                            dd.checked = true;
                        }
                        else {
                            dd.checked = false;
                        }
                        

                    });
                }
            }
        }
       
        $scope.previewimg_new = function (img) {
            $scope.vbabrsF_FilePath = img;
            var img = $scope.vbabrsF_FilePath;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.vbabrsF_FilePath);
                    $('#myimagePreview').modal('show');
                    $('#ViewDetails').modal('hide');
                    //ViewDetails
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
        $scope.radiochange = function (ee) {
            if (ee.flag === '1') {
                ee.classname = 'neww';
              
                var charges = 0;
                charges = (Number(ee.VPAYVOU_TDS) + Number(ee.VPAYVOU_Penalty) + Number(ee.VPAYVOU_ESICAmount) + Number(ee.VPAYVOU_OtherAmount));
                
                ee.VPAYVOUAUT_SanctionedAmount = (Number(ee.VPAYVOU_AppliedAmount) - (Number(charges)));
            }
            else {
                ee.VPAYVOUAUT_SanctionedAmount = 0;
                ee.classname = 'oldd';
            }
           // $scope.ContraValidation(ee);
        }
        $scope.AmountChange = function (user) {
          
            if (user.VPAYVOUAUT_SanctionedAmount <= user.VPAYVOU_AppliedAmount) {
                if ($scope.getadvancetada != null && $scope.getadvancetada.length > 0) {
                    //$scope.ContraValidation(user);
                }
            }
            else {
                swal("Applied Amount Not Greater Than Sanction Amount !");
                user.VPAYVOUAUT_SanctionedAmount = "";
                //user.VPAYVOUAUT_SanctionedAmount = user.VPAYVOU_AppliedAmount;
               // $scope.ContraValidation(user);
                return;
            }
           
        }
        //$scope.ContraValidation = function (user) {
        //    var AppliedAmount = 0; var InterNetAmount = 0;
        //    angular.forEach($scope.getadvancetada, function (dd) {
        //        if (dd.VPAYVOUAUT_SanctionedAmount > 0 && dd.VPAYVOU_AppliedAmount > 0 && dd.HRMBD_InterbankTransFlg === false && dd.MI_Id === user.MI_Id && dd.HRMBD_Id === user.HRMBD_Id) {
        //            AppliedAmount = (Number(dd.VPAYVOUAUT_SanctionedAmount) + Number(AppliedAmount));
        //        }
        //    });
        //    angular.forEach($scope.getadvancetada, function (dd) {
        //        if (dd.VPAYVOUAUT_SanctionedAmount > 0 && dd.VPAYVOU_AppliedAmount > 0 && dd.HRMBD_InterbankTransFlg === true && dd.MI_Id === user.MI_Id && user.HRMBD_Id === dd.VPAYVOU_HRMBD_Id) {
        //            InterNetAmount = (Number(AppliedAmount));
        //            dd.VPAYVOUAUT_SanctionedAmount = InterNetAmount;
        //            return;
        //        }
        //    });
        //}
        //=====addedby sanjeecv======//
        $scope.savedatatwo = function (tp) {
            $scope.submitted = true;
            $scope.Apprval = [];
            if ($scope.myForm.$valid) {
                angular.forEach($scope.getadvancetada, function (comp) {
                    if (comp.checked == true) {
                        $scope.Apprval.push({
                            VPAYVOU_Remarks: comp.VPAYVOU_RemarksA,
                            VPAYVOUAUT_SanctionedAmount: comp.VPAYVOUAUT_SanctionedAmount,
                            VPAYVOU_Id: comp.VPAYVOU_Id,
                            approvecnt: comp.flag,
                            level: comp.SanctionLevelNo,
                            MI_Id: comp.MI_Id,
                            VTADAAA_Remarks: comp.VPAYVOU_Remarks,
                        });
                    }
                   
                });
                if ($scope.Apprval != null && $scope.Apprval.length > 0) {
                    var data = {};
                    data = {
                        "Apprval": $scope.Apprval,
                        "VPAYVOUAUT_OTP": $scope.EMAILOTP
                    };
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    };
                    apiService.create("CheckBookEntryApproval/Getdata", data).then(function (promise) {
                        swal('ChequeBook Approved Successfully');
                        $state.reload();


                    });
                }
                else {
                    swal("Please Select Atleast One CheckBox !");
                }
               
            }
        };
        $scope.printDataModal = function () {
            var innerContents = document.getElementById("tablegrp").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');

            popupWinindow.document.close();
        };
    }
})();

