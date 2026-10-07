(function () {
    'use strict';
    angular.module('app').controller('VMS_Receipt_VoucherController', VMS_Receipt_VoucherController)
    VMS_Receipt_VoucherController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function VMS_Receipt_VoucherController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {
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
        $scope.obj.VBABRS_ClearedFlg = true;
        $scope.obj.VRECVOU_Date = new Date();
        $scope.obj.VBABRS_ClearedDate = new Date();
        $scope.loaddata = function () {
            $scope.obj.MI_Id = "";
            var pageid = 2;
            apiService.getURI("VMS_Receipt_Voucher/loaddata", pageid).then(function (promise) {
                $scope.allcompany = promise.allcompany;
                $scope.sourcelist = promise.sourcelist;
                $scope.checkbookSource = promise.checkbookSource;
            });
        };
        $scope.oncmpchange = function () {
            $scope.obj.HRMBD_Id = "";
            $scope.obj.ISMMCLT_Id = "";
            var data = {
                "MI_Id": $scope.obj.MI_Id,
            };
            apiService.create("VMS_Receipt_Voucher/onchange", data).then(function (promise) {
                $scope.banklist = promise.banklist;
                $scope.clientlist = promise.clientlist;
                $scope.ModeOfPayment = promise.modeOfPayment;
                $scope.alldata = promise.alldata; 
            });
        }
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
                "MI_Id": $scope.obj.MI_Id
            };
            apiService.create("CheckBookEntry/getbom", data).then(function (promise) {
                if (promise.msg != null && promise.msg != "" && promise.msg == "OB") {
                    $scope.obj.VPAYVOU_BankBalalnce = promise.projectlist[0].vbabrS_BankBalalnce;

                   
                }
                else {
                    if (Message != null && Message != "") {
                        swal("Must Be Entered  : " + Message + " - Opening Balnace !");
                    }                                    
                    $scope.obj.HRMBD_Id = "";
                    $scope.obj.VPAYVOU_BankBalalnce = 0;
                }


            });
        }
        $scope.RDIchaNge = function () {
           
        }
        $scope.savedata = function () {
            var VRECVOU_Id = 0; var ISMMCLT_Id = 0;
            var VBABRS_ClearedDate = null;
            if ($scope.obj.VRECVOU_Id > 0) {
                VRECVOU_Id = $scope.obj.VRECVOU_Id;
            }
            if ($scope.obj.VBABRS_ClearedFlg == true) {
                VBABRS_ClearedDate = new Date($scope.obj.VRECVOU_Date).toDateString();

            }

            if ($scope.obj.optionflag === 'Client') {
                $scope.obj.VCBSRC_Id = null; // Clear the source ID
            } else if ($scope.obj.optionflag === 'Other') {
                $scope.obj.ISMMCLT_Id = null; // Clear the client ID
            }
           
          
            if ($scope.myForm.$valid) {
                swal({
                    title: "Are you sure?",
                    text: "Do You Want To sure " + $scope.obj.VRECVOU_Amount +" Credit  Receipt Payment  ?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Payment ?",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            var data = {
                                "MI_Id": $scope.obj.MI_Id,
                                "HRMBD_Id": $scope.obj.HRMBD_Id,
                                "VRECVOU_FromClientFlg": $scope.obj.VRECVOU_FromClientFlg,
                                "ISMMCLT_Id": $scope.obj.ISMMCLT_Id,
                                "HRMBD_Id": $scope.obj.HRMBD_Id,
                                "VRECVOU_ReceiptMode": $scope.obj.VRECVOU_ReceiptMode,
                                "VRECVOU_ReceiptReference": $scope.obj.VRECVOU_ReceiptReference,                            
                                "VRECVOU_ReceivedFrom": $scope.obj.VRECVOU_ReceivedFrom,
                                "VRECVOU_ChequeDDNo": $scope.obj.VRECVOU_ChequeDDNo,
                                "VRECVOU_Remarks": $scope.obj.VRECVOU_Remarks,
                                "VRECVOU_Amount": $scope.obj.VRECVOU_Amount,
                                "VRECVOU_Id": VRECVOU_Id,
                                "VCBSRC_Id": $scope.obj.VCBSRC_Id,                              
                                "VRECVOU_Date": new Date($scope.obj.VRECVOU_Date).toDateString(),
                                "VBABRS_ClearedDate": VBABRS_ClearedDate,
                                "VBABRS_ClearedFlg": $scope.obj.VBABRS_ClearedFlg


                            };

                            apiService.create("VMS_Receipt_Voucher/savedata", data).then(function (promise) {
                                if (promise.returnval == "save") {
                                    swal("Data Saved  Successfully");
                                }
                                else if (promise.returnval == "notsave") {
                                    swal("Failed To Save  Record !");
                                }
                                else if (promise.returnval == "admin") {
                                    swal("please Contact Administrator !");
                                }
                                $state.reload();
                            });
                        }
                        else {
                            swal("Payment Recipient Cancelled !");
                        }
                    }
                );
            }
            else {
                $scope.submitted = true;
            }




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

    }
})();
