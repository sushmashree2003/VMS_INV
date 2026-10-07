(function () {
    'use strict';
    angular.module('app').controller('CheckBankBalanceController', CheckBankBalanceController)
    CheckBankBalanceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function CheckBankBalanceController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
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

        $scope.search = "";
        $scope.obj.optionflag = "";
        $scope.newdate = new Date();
        $scope.searchchkbx = "";

        $scope.submitted = false;
        $scope.obj.VBABRS_Date = new Date();
        $scope.maxdate = new Date();

        $scope.plMaxdate = new Date();
        $scope.plMaxdate.setDate($scope.plMaxdate.getDate());

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = paginationformasters;
        $scope.obj.bankbalanceone = false;
        $scope.submitted = false;



        $scope.obj.VBABRS_CRDRFlg = [];
        $scope.obj.VBABRS_CRDRFlg = "CR";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
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
                if ($scope.obj.VBABRS_BankBalalnceTWO < $scope.obj.VBABRS_Amounttwo) {
                    $scope.obj.VBABRS_Amounttwo = 0;
                    swal('Bank Balance Amount Not greater Enter Amount !');
                }
            }

        }
        $scope.typechange = function () {
            $scope.obj.VBABRS_CRDRFlg = "CR";
            if ($scope.obj.VBABRS_Amount > 0) {
                if ($scope.obj.VBABRS_BankBalalnce < $scope.obj.VBABRS_Amount && $scope.obj.VBABRS_CRDRFlg == 'DR') {
                    $scope.obj.VBABRS_Amount = "";
                    swal('Applied Amount Should not be greater than Bank Balance');
                }
                else {
                    $scope.obj.VBABRS_BankBalalnce = $scope.mainbalance;
                    if ($scope.obj.VBABRS_BankBalalnce == undefined || $scope.obj.VBABRS_BankBalalnce == null) {
                        $scope.obj.VBABRS_BankBalalnce = 0;
                    }
                    if ($scope.obj.VBABRS_Amount == undefined || $scope.obj.VBABRS_Amount == null) {
                        $scope.obj.VBABRS_Amount = 0;
                    }
                    if ($scope.obj.VBABRS_CRDRFlg == 'DR') {
                        $scope.obj.VBABRS_BankBalalnce = parseFloat($scope.obj.VBABRS_BankBalalnce) - parseFloat($scope.obj.VBABRS_Amount);
                    }
                    else if ($scope.obj.VBABRS_CRDRFlg == 'CR') {
                        $scope.obj.VBABRS_BankBalalnce = parseFloat($scope.obj.VBABRS_BankBalalnce) + parseFloat($scope.obj.VBABRS_Amount);
                    }
                }
            }
            else {
                $scope.obj.VBABRS_BankBalalnce = $scope.mainbalance;
            }

        }
        $scope.amtchange = function () {
            $scope.typechange();
        }

        $scope.bankchange = function () {
            $scope.obj.bankbalanceone = false;
            $scope.bomlist = [];
            var data = {
                "HRMBD_Id": $scope.obj.HRMBD_Id,
                "MI_Id": $scope.obj.MI_Id,
            };

            apiService.create("CheckBankBalance/getbom", data).then(function (promise) {
                $scope.bomlist = promise.projectlist;
                if (promise.projectlist != null && promise.projectlist.length > 0) {
                    $scope.mainbalance = $scope.bomlist[0].vbabrS_BankBalalnce;
                    $scope.obj.VBABRS_BankBalalnce = $scope.bomlist[0].vbabrS_BankBalalnce;
                    if ($scope.obj.VBABRS_BankBalalnce > 0) {
                        $scope.obj.bankbalanceone = true;
                    }


                }
                else {
                    $scope.mainbalance = 0;
                    $scope.obj.VBABRS_BankBalalnce = 0;
                }

            });

        }


        $scope.oncmpchange = function () {

            $scope.alldata = [];
            $scope.obj.HRMBD_Id = "";

            var data = {
                "MI_Id": $scope.obj.MI_Id
            };
            apiService.create("CheckBankBalance/companychange", data).then(function (promise) {
                $scope.alldata = promise.alldata;
                $scope.banklist = promise.banklist;
            });
        }

        $scope.loaddata = function () {
            $scope.obj.MI_Id = "";
            var pageid = 2;
            apiService.getURI("CheckBankBalance/loaddata", pageid).then(function (promise) {
                $scope.allcompany = promise.allcompany;
            });
        };
        $scope.savedataModal = function () {
            $scope.submitted1 = true;
            if ($scope.myFormone.$valid) {
                $('#chequecancel').modal('hide');
            }

        }
        $scope.savedataClearFlag = function () {
            $scope.cleararrflag = [];
            if ($scope.alldata != null && $scope.alldata.length > 0) {
                angular.forEach($scope.alldata, function (comp) {
                    if (comp.VBABRS_ClearedFlgone == true) {
                        $scope.cleararrflag.push({
                            VBABRS_Id: comp.VBABRS_Id,
                            VBABRS_ClearedDateone: new Date(comp.VBABRS_ClearedDateone).toDateString()
                        });
                    }
                });
            }
            if ($scope.cleararrflag != null && $scope.cleararrflag.length > 0) {
                var data = {
                    "cleararrflag": $scope.cleararrflag

                };

                apiService.create("CheckBankBalance/getreport", data).then(function (promise) {
                    if (promise.msg == "saved") {
                        swal("Data Saved/Updated Successfully");
                    } else {
                        swal("Contact Adminstrator");
                    }
                    $state.reload();
                });
            }
            else {
                swal("Select Atleast One Record  !");
            }

        }
        $scope.savedata = function () {
            $scope.obj.VBABRS_CRDRFlg = "CR";
            $scope.obj.optionflag = "payment";
            var fromdate1 = $scope.obj.VBABRS_Date == null ? "" : $filter('date')($scope.obj.VBABRS_Date, "yyyy-MM-dd");
            if ($scope.VBABRS_ClearedFlg == true) {
                var cleardate = $scope.obj.VBABRS_ClearedDate == null ? "" : $filter('date')($scope.obj.VBABRS_ClearedDate, "yyyy-MM-dd");
            }
            else {
                var cleardate = null;
            }
            var Debited;
            if ($scope.obj.VBABRS_CRDRFlg == 'CR') {
                Debited = "CREDIT";
            }
            else {
                Debited = "DEBIT";
            }
            var dystring = $scope.obj.VBABRS_Amount;
            if ($scope.myForm.$valid) {
                swal({
                    title: "Are you sure?",
                    text: "Do You Want To " + dystring + " " + Debited + " Bank Amount ?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes," + Debited + " Amount ?",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {

                            var data = {
                                "MI_Id": $scope.obj.MI_Id,
                                "VBABRS_Id": $scope.obj.VBABRS_Id,
                                "HRMBD_Id": $scope.obj.HRMBD_Id,
                                "VBABRS_Remarks": $scope.obj.VBABRS_Remarks,
                                "VBABRS_BankBalalnce": $scope.obj.VBABRS_BankBalalnce,
                                "VBABRS_Amount": $scope.obj.VBABRS_Amount,
                                "VBABRS_Date": fromdate1,
                                "VBABRS_ClearedDate": cleardate,
                                "VBABRS_CRDRFlg": $scope.obj.VBABRS_CRDRFlg,
                                "VBABRS_ClearedFlg": $scope.obj.VBABRS_ClearedFlg,
                                "optionflag": $scope.obj.optionflag,
                                //add voucher                                    

                            };

                            apiService.create("CheckBankBalance/savedata", data).then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Data Saved/Updated Successfully");
                                } else {
                                    swal("Failed To Save/Update Record");
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
    }
})();
