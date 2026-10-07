(function () {
    'use strict';
    angular.module('app').controller('VMS_CheckBook_PaymentReportController', VMS_CheckBook_PaymentReportController)
    VMS_CheckBook_PaymentReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', 'Excel', '$timeout']
    function VMS_CheckBook_PaymentReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, Excel, $timeout) {
        $scope.search = "";
        $scope.newdate = new Date();
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        if (transnumconfigsettings != null && transnumconfigsettings.length > 0) {
            for (var i = 0; i < transnumconfigsettings.length; i++) {
                if (transnumconfigsettings[i].imN_Flag === "CLIINV") {
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
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.currentPageone = 1;
        $scope.submitted = false;
        $scope.obj = {};
        $scope.flagg = true;
        $scope.loaddata = function () {
            $scope.obj.MI_Id = "";
            var pageid = 2;
            apiService.getURI("VMS_Receipt_Voucher/loaddata", pageid).then(function (promise) {
                $scope.allcompany = promise.allcompany;
               
            });
        };
        $scope.mi_clickhrmd = function () {
            $scope.allhrmd = $scope.banklist.every(function (itm) {
                return itm.hrmbid;
            });

        };
        $scope.viewdetailsTWO = function (dd) {
            var rdopunch = "All";
            $scope.checkDetails = [];
            $scope.GRnDetails = [];
            var data = {
                "VPAYVOU_Id": dd.VPAYVOU_Id,
                "optionflag": rdopunch
            };
            apiService.create("CheckBankBalance/Editdata", data).then(function (promise) {
                if (promise.checkDetails != null && promise.checkDetails.length > 0) {
                    $scope.checkDetails = promise.checkDetails;
                    $scope.GRnDetails = promise.checkbookgrn;
                    $scope.VPAYVOU_ChequeNo = $scope.checkDetails[0].VPAYVOU_ChequeNo;
                    if ($scope.checkDetails[0].INVMS_SupplierName != null && $scope.checkDetails[0].INVMS_SupplierName != "") {
                        $scope.partyname = $scope.checkDetails[0].INVMS_SupplierName;
                    }
                    else {
                        $scope.partyname = $scope.checkDetails[0].VPAYVOU_PaymentTo;
                    }
                    $scope.VoucherCreateby = $scope.checkDetails[0].createdby;
                    $scope.VPAYVOU_PaymentDate = new Date($scope.checkDetails[0].VPAYVOU_PaymentDate);
                    $scope.VPAYVOU_Remarks = $scope.checkDetails[0].VPAYVOU_Remarks;
                    $scope.VPAYVOU_StatusFlg = $scope.checkDetails[0].VPAYVOU_StatusFlg;
                    $scope.HRMBD_BankName = $scope.checkDetails[0].HRMBD_BankName;
                    $scope.HRMBD_BankAccountNo = $scope.checkDetails[0].HRMBD_BankAccountNo;
                    $scope.projectlistTWO = promise.projectlistTWO;
                    $('#popup123').modal('show');
                }
                else {
                    swal("Record Not found !");

                }

            });
        }
        $scope.printDataTT = function () {
            var innerContents = "";
            innerContents = document.getElementById("asdftPPt").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.allhrmdrr = function () {

            var toggleStatus = $scope.allhrmd;
            angular.forEach($scope.banklist, function (itm) {
                itm.hrmbid = toggleStatus;
            });

        };
        $scope.isRequiredmiBnk = function () {

            return !$scope.banklist.some(function (options) {
                return options.hrmbid;
            });

        };
        $scope.RDIchaNge = function () {

            $scope.allReport = [];
        };
        $scope.oncmpchange = function () {
            $scope.obj.HRMBD_Id = "";
           
            var data = {
                "MI_Id": $scope.obj.MI_Id,
            };
            apiService.create("VMS_Receipt_Voucher/onchange", data).then(function (promise) {
                $scope.banklist = promise.banklist;
                
            });
        }
      
        $scope.savedata = function () {  
            $scope.hrmdlist = [];
            $scope.allReport = [];
            if ($scope.myForm.$valid) {
                if ($scope.banklist != null && $scope.banklist.length > 0) {
                    angular.forEach($scope.banklist, function (comp) {
                        if (comp.hrmbid == true) {
                            $scope.hrmdlist.push({
                                HRMD_Id: comp.hrmbD_Id
                            });
                        }
                    });
                }
                var data = {  
                    "Banklistdetails": $scope.hrmdlist,
                    "MI_Id": $scope.obj.MI_Id,
                    "optionflag": $scope.obj.optionflag,
                    //"startdate": new Date($scope.obj.startdate).toDateString(),
                    //"enddate": new Date($scope.obj.enddate).toDateString(),

                };
                apiService.create("VMS_Receipt_Voucher/RecieptReport", data).then(function (promise) {
                    
                    if (promise.allReport != null && promise.allReport.length > 0) {
                        $scope.allReport = promise.allReport;
                        $scope.imagenewtwo = promise.logo;
                    }
                    else {
                        swal("Record Not Found !");
                    }
                   
                   

                });
            }          
            else {
                $scope.submitted = true;
            }
        };
        $scope.exportToExcel = function () {
            var PayAdavance;
            if ($scope.obj.optionflag == 'PAYMENT') {
                PayAdavance = payment;
            }
            else if ($scope.obj.optionflag == 'RECEIPT') {
                PayAdavance = reciept1;
            }
            else if ($scope.obj.optionflag == 'CONTRA') {
                PayAdavance = contr;
            }
            
            var exportHref = Excel.tableToExcel(PayAdavance, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };
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
        $scope.printDataREC = function () {
            var innerContents = "";
            if ($scope.obj.optionflag == 'PAYMENT') {
                innerContents = document.getElementById("payment").innerHTML;
            }
            else if ($scope.obj.optionflag == 'RECEIPT') {
                innerContents = document.getElementById("reciept").innerHTML;
            }
            else if ($scope.obj.optionflag == 'CONTRA') {
                innerContents = document.getElementById("contr").innerHTML;
            }        
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        //printDataREC
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.reschedule = function (user) {
            $('#popup111').modal('show');
            if (user.VRECVOU_ChequeDDNo != "") {
                $scope.chno = user.VRECVOU_ChequeDDNo;

            }
            else {
                $scope.paymode = user.VRECVOU_ReceiptMode
            }
            $scope.pdate = user.VRECVOU_CreatedDate;
            if (user.ISMMCLT_ClientName != null && user.ISMMCLT_ClientName != "") {
                $scope.supldetails = user.ISMMCLT_ClientName;
            }
            else {
                $scope.supldetails = user.VCBSRC_SourceName;
            }

            $scope.bankname = user.HRMBD_BankName;
            $scope.bbacc = user.HRMBD_BankAccountNo;
            $scope.rmrk = user.VRECVOU_Remarks;
            $scope.amtt = user.VRECVOU_Amount;

            $scope.Math = window.Math;
            $scope.crby = user.createdby;
            $scope.imagenew = user.MI_Logo;
            $scope.roundoffttl = $scope.Math.round($scope.amtt, 0)

            $scope.words = $scope.amountinwords($scope.roundoffttl);

        }
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
                        words_string += "Crores Rupees";
                    }
                    if ((i == 3 && atotalc != 0) || (i == 2 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Lakhs Rupees";
                    }
                    if ((i == 5 && atotalc != 0) || (i == 4 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Thousand Rupees ";
                    }
                    if (i == 6 && atotalc != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                        words_string += "Hundred and ";
                    } else if (i == 6 && atotalc != 0) {
                        words_string += "Hundred Rupees ";
                    }
                }
                words_string = words_string.split("  ").join(" ");
            }
            return words_string;
        }
        //==============End==============//
        $scope.rescheduletwo = function (user) {
            $scope.tdsamt = 0;
            $scope.penaltyamt = 0;
            $scope.imagenew = user.MI_Logo;
           // $scope.imagenew = $scope.mi_logo;
            $scope.miname = user.MI_Name;
            $scope.pvno = user.VPAYVOU_VoucherNo;
            $scope.chno = user.VPAYVOU_ChequeNo;
            $scope.pdate = user.VPAYVOU_PaymentDate;
            $scope.pdate = user.VPAYVOU_PaymentDate;
            $scope.supldetails = user.INVMS_SupplierName;
            $scope.bankname = user.HRMBD_BankName;
            $scope.bbacc = user.HRMBD_BankAccountNo;
            $scope.crby = user.createdby;
            if (user.VPAYVOU_StatusFlg == 'Approved') {
                $scope.amtt = user.VPAYVOU_ApprovedAmount;
            }
            else {
                var tds = 0; var penalty = 0;
                if (user.VPAYVOU_TDS != null && user.VPAYVOU_TDS > 0) {
                    tds = user.VPAYVOU_TDS;

                }
                if (user.VPAYVOU_Penalty != null && user.VPAYVOU_Penalty > 0) {
                    penalty = user.VPAYVOU_Penalty
                }
                $scope.amttthree = parseFloat(user.VPAYVOU_AppliedAmount) + parseFloat(tds) + parseFloat(penalty)
                $scope.amtt = user.VPAYVOU_AppliedAmount;
                if (user.VPAYVOU_TDS != null && user.VPAYVOU_TDS > 0) {
                    $scope.tdsamt = user.VPAYVOU_TDS;
                }
                if (user.VPAYVOU_Penalty != null && user.VPAYVOU_Penalty > 0) {
                    $scope.penaltyamt = user.VPAYVOU_Penalty;
                }
            }


            $scope.rmrk = user.VPAYVOU_Remarks;
            $scope.Math = window.Math;
            $scope.roundoffttl = $scope.Math.round($scope.amtt, 0)
            $scope.words = $scope.amountinwords($scope.roundoffttl);
            $('#popup11111').modal('show');
        }

        /////end3///
        $scope.reschedulethtree = function (user) {
            $scope.tdsamt = 0;
            $scope.penaltyamt = 0;
            $scope.imagenew = user.MI_Logo;
            $scope.miname = user.MI_Name;
            $scope.pvno = user.VPAYVOU_VoucherNo;
            $scope.chno = user.VPAYVOU_ChequeNo;
            $scope.pdate = user.VPAYVOU_PaymentDate;
            $scope.pdate = user.VPAYVOU_PaymentDate;
            $scope.supldetails = user.VPAYVOU_PaymentTo;
            $scope.bankname = user.HRMBD_BankName;
            $scope.bbacc = user.HRMBD_BankAccountNo;
            $scope.crby = user.createdby;
            $scope.Tobank = user.Tobank;
            $scope.toaccountnumber = user.toaccountnumber;
            if (user.VPAYVOU_StatusFlg == 'Approved') {
                $scope.amtt = user.VPAYVOU_ApprovedAmount;
            }
            else {
                var tds = 0; var penalty = 0;
                if (user.VPAYVOU_TDS != null && user.VPAYVOU_TDS > 0) {
                    tds = user.VPAYVOU_TDS;

                }
                if (user.VPAYVOU_Penalty != null && user.VPAYVOU_Penalty > 0) {
                    penalty = user.VPAYVOU_Penalty
                }
                $scope.amttthree = parseFloat(user.VPAYVOU_AppliedAmount) + parseFloat(tds) + parseFloat(penalty)
                $scope.amtt = user.VPAYVOU_AppliedAmount;
                if (user.VPAYVOU_TDS != null && user.VPAYVOU_TDS > 0) {
                    $scope.tdsamt = user.VPAYVOU_TDS;
                }
                if (user.VPAYVOU_Penalty != null && user.VPAYVOU_Penalty > 0) {
                    $scope.penaltyamt = user.VPAYVOU_Penalty;
                }
            }


            $scope.rmrk = user.VPAYVOU_Remarks;
            $scope.Math = window.Math;
            $scope.roundoffttl = $scope.Math.round($scope.amtt, 0)
            $scope.words = $scope.amountinwords($scope.roundoffttl);
            $('#popup341').modal('show');
        }
      
    }
})();
