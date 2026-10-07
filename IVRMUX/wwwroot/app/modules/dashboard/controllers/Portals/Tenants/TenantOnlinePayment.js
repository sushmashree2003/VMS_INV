(function () {
    'use strict';
    angular
        .module('app')
        .controller('TenantOnlinePaymentController', TenantOnlinePaymentController)

    TenantOnlinePaymentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache', 'blockUI', '$compile']
    function TenantOnlinePaymentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache, blockUI, $compile) {

        //  var studentphtoto = "https://jshsstorage.blob.core.windows.net/files/NoImage.jpg";

        $scope.asmaY_Year = "";
        $scope.qwe = {};

        $scope.obj = {};
        $scope.search = "";
        $scope.edit_flag = false;
        $scope.bankdetails = false;
        //added on 09102017
        $scope.place_flg = true;
        $scope.name_flg = false;
        $scope.otherdetails == false;
        $scope.tenant_details = false;
        $scope.rolenamelist = "";
        $scope.tenantagrpaY_PaymentDate = new Date();
        $scope.searchby = false;
        $scope.itemsPerPage = 5;
        $scope.currentPage = 1;
        $scope.RegistrationNumbering = [];
        $scope.showsingle = false;



        //  var transnumconfig = promise.admTransNumSetting;
        //   localStorage.setItem("transnumconfigsettings", JSON.stringify(transnumconfig));
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        var admregcount = 0;
        var admnocount = 0;
        for (var i1 = 0; i1 < transnumconfigsettings.length; i1++) {
            if (transnumconfigsettings.length > 0) {
                $scope.transnumbconfigurationsettings = transnumconfigsettings[i1];

                //if (transnumconfigsettings[i1].imN_Flag === "TenantOfflinePayNo") {
                //    admregcount += 1;
                //    $scope.RegistrationNumbering = transnumconfigsettings[i1];

                //}

                if (transnumconfigsettings[i1].imN_Flag === "TenantOnlinePayNo") {
                    admregcount += 1;
                    $scope.RegistrationNumbering = transnumconfigsettings[i1];
                    $scope.IMN_AutoManualFlag = transnumconfigsettings[i1].imN_AutoManualFlag;
                }


            }
        }




        var transactionnumbering = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transactionnumbering.length; i++) {
            if (transactionnumbering.length > 0) {
                if (transactionnumbering[i].imN_Flag == "TenantOnlinePayNo") {
                    $scope.transnumbconfigurationsettingsassign = transactionnumbering[i];
                }
            }
        }

        var searchObject = $location.search();
        if (searchObject.status == "failure" || searchObject.status == "TXN_FAILURE") {
            swal("Payment Unsuccessfull");

            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        else if (searchObject.status == "success" || searchObject.status == "TXN_SUCCESS") {
            swal("Payment Successfull");

            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        else if (searchObject.status == "Networkfailure") {
            swal('Network failure..!!', 'Try again after some time');

            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }


        $scope.all1 == true
        $scope.countflagwater = true;
        $scope.countflagrent = true;
        $scope.countflagElectricity = true;
        $scope.countflagother = true;
        $scope.countflagAdvance = true;

        $scope.cfg = {};
        $scope.loaddata = function () {
            // var pageid = 1;

            var data = {
                "type": $scope.all1,
                //"waterflag": $scope.countflagwater,
                //"rentflag": $scope.countflagrent,
                //"Electricityflag": $scope.countflagElectricity,
                //"Advanceflag": $scope.countflagAdvance,

                "waterflag": 'WaterBill',
                "rentflag": 'Rent',
                "Electricityflag": 'Electrical',
                "Advanceflag": 'Advance',
                "Otherflag": 'Other'

            }

            apiService.create("TenantOnlinePayment/getalldetails", data).then(function (promise) {


                if (promise.tenant_array != null && promise.tenant_array.length > 0) {
                    $scope.tenant_array = promise.tenant_array;
                }

                if (promise.get_tenant_online_details != null && promise.get_tenant_online_details.length > 0) {
                    $scope.get_tenant_details = promise.get_tenant_online_details;
                }
                if (promise.get_online_month_details.length > 0) {
                    $scope.get_month_details = promise.get_online_month_details;
                }
                $scope.paymenttest = promise.fillpaymentgateway;

                if (promise.institutiondet.length > 0) {
                    for (var i = 0; i < promise.institutiondet.length; i++) {
                        $scope.institutioname = promise.institutiondet[0].mI_Name;
                        $scope.institulogo = promise.institutiondet[0].mI_Logo;
                    }
                }
            })





        }

        $scope.filterValue1 = function (obj) {
            return ($filter('amsT_Date')(obj.tenantagrpaY_PaymentDate, 'dd-MM-yyyy').indexOf($scope.search) >= 0) ||
                (angular.lowercase(obj.tenantagR_TenantName)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.tenantagrpaY_Amount)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.ivrmmoD_ModeOfPayment)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.ivrM_Month_Name)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.tenantagrpaY_Remarks)).indexOf(angular.lowercase($scope.search)) >= 0

        };






        $scope.studentlst = [];

        //$scope.getmonth = function (tenant_id) {
        //    var tenantagR_Id = 0;
        //    if (tenant_id != null && tenant_id.length > 0) {
        //        tenantagR_Id = tenant_id[0].tenantagR_Id;
        //    }
        //    else {
        //        tenantagR_Id = tenant_id.tenantagR_Id;
        //    }

        //    var data = "";
        //    data = {
        //        "TENANTAGR_Id": tenantagR_Id,
        //    };
        //    apiService.create("TenantOnlinePayment/getmonthdetails", data).then(function (promise) {
        //        if (promise.get_month_details.length > 0) {
        //            $scope.get_month_details = promise.get_month_details;


        //        } else {
        //            //$scope.AMST_Id = "";
        //            //("No tenants are found for your search");
        //        }
        //    });



        //};
        $scope.onradiobuttonSelect = function (type) {
            // $scope.labeldisable = false;


            if ($scope.all1 == true) {
                $scope.countflagwater = true;
                $scope.countflagrent = true;
                $scope.countflagElectricity = true;
                $scope.countflagother = true;
                $scope.countflagAdvance = true;
            }
            else if ($scope.all1 == false) {

                $scope.countflagwater = false;
                $scope.countflagrent = false;
                $scope.countflagElectricity = false;
                $scope.countflagother = false;
                $scope.countflagAdvance = false;
            }


            //var pageid = 1;
            var data = {
                "type": $scope.all1,
                //"waterflag": $scope.countflagwater,
                //"rentflag": $scope.countflagrent,
                //"Electricityflag": $scope.countflagElectricity,
                //"Advanceflag": $scope.countflagAdvance,

                "waterflag": 'WaterBill',
                "rentflag": 'Rent',
                "Electricityflag": 'Electrical',
                "Advanceflag": 'Advance',
                "Otherflag": 'Other'

            }

            apiService.create("TenantOnlinePayment/onchangetype", data).then(function (promise) {


                if (promise.tenant_array != null && promise.tenant_array.length > 0) {
                    $scope.tenant_array = promise.tenant_array;
                }

                if (promise.get_tenant_online_details != null && promise.get_tenant_online_details.length > 0) {
                    $scope.get_tenant_details = promise.get_tenant_online_details;
                }
                if (promise.get_online_month_details.length > 0) {
                    $scope.get_month_details = promise.get_online_month_details;
                }
                $scope.paymenttest = promise.fillpaymentgateway;

                $scope.fill_bankdetails = promise.fill_bankdetails;

                if (promise.institutiondet.length > 0) {
                    for (var i = 0; i < promise.institutiondet.length; i++) {
                        $scope.institutioname = promise.institutiondet[0].mI_Name;
                        $scope.institulogo = promise.institutiondet[0].mI_Logo;
                    }
                }
            })


        }
        $scope.onclickloaddata = function (total_payable, get_month_details, qwe) {
            $scope.gateway = false;

            if (qwe.paygtw === "RAZORPAY") {
                $scope.pamentsave(total_payable, get_month_details, qwe);
            }
        }

        $scope.pamentsave = function (total_payable, get_month_details, qwe) {



            if ($scope.monthlistarray.length != 0 || $scope.monthlistarray != "") {
                var selected_list = [];
                angular.forEach($scope.monthlistarray, function (qq) {


                    if (qq.Rentbill > 0) {

                        selected_list.push({
                            MonthId: qq.monthid,
                            TENANTAGRPAY_PaymentYear: qq.yearname,
                            TENANTAGR_MonthlyRent: qq.Rentbill,
                            Name: "Rent"
                        });


                    }

                    if (qq.waterbill > 0) {

                        selected_list.push({
                            MonthId: qq.monthid,
                            TENANTAGRPAY_PaymentYear: qq.yearname,
                            TENANTAGR_MonthlyRent: qq.waterbill,
                            Name: "WaterBill"
                        });


                    }
                    if (qq.otherbill > 0) {

                        selected_list.push({
                            MonthId: qq.monthid,
                            TENANTAGRPAY_PaymentYear: qq.yearname,
                            TENANTAGR_MonthlyRent: qq.otherbill,
                            Name: "other"
                        });


                    }
                    if (qq.Electricalbill > 0) {

                        selected_list.push({
                            MonthId: qq.monthid,
                            TENANTAGRPAY_PaymentYear: qq.yearname,
                            TENANTAGR_MonthlyRent: qq.Electricalbill,
                            Name: "Electrical"
                        });


                    }


                    //         selected_list.push({ monthid: qq.monthid, month_name: qq.month_name, monthamt: qq.monthamt, yearid: qq.yearname, waterbill: qq.waterbill, otherbill: qq.otherbill, Electricalbill: qq.Electricalbill, Rentbill: qq.Rentbill })

                })
            }

            var fromdate1 = $scope.tenantagrpaY_PaymentDate == null ? "" : $filter('date')($scope.tenantagrpaY_PaymentDate, "yyyy-MM-dd");
            var tenantagrpaY_PaymentDate = $scope.tenantagrpaY_PaymentDate
            var regconfigsettings
            if (transnumconfigsettings != null && transnumconfigsettings.length > 0) {
                regconfigsettings = transnumconfigsettings;
            }

            var data = {
                // "TENANTAGR_Id": tenantagR_Id,
                "TENANTAGRPAY_Amount": $scope.total_payable,
                // "TENANTAGRPAY_PaymentDate": new Date($scope.tenantagrpaY_PaymentDate).toDateString(),
                "TENANTAGRPAY_PaymentDate": fromdate1,
                "TENANTAGRPAY_Remarks": $scope.TENANTAGRPAY_Remarks,
                "Month_detailsarray": selected_list,
                transnumconfigsettings: $scope.transnumbconfigurationsettingsassign,
                "onlinepaygteway": qwe.paygtw,
                "type": $scope.all1
                //"transnumconfigsettings": regconfigsettings

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("TenantOnlinePayment/generatehashsequence", data).
                then(function (promise) {

                    // if (promise.message == "") {
                    if (qwe.paygtw == "BILLDESK") {
                        if (promise.pipeSepMsg != null && promise.pipeSepMsg != '') {
                            //var url "https://pgi.billdesk.com/pgidsk/PGIMerchantPayment";
                            var url = promise.url;
                            var method = 'POST';
                            var params = {
                                "msg": promise.pipeSepMsg
                            }
                            FormSubmitter.submit(url, method, params);
                        }
                        else {
                            swal("Sorry Something Went Wrong");
                        }
                    }
                    else if (qwe.paygtw == "PAYTM") {
                        $scope.MID = promise.paydet[0].mid;
                        $scope.ORDER_ID = promise.paydet[0].ordeR_ID;
                        $scope.CUST_ID = promise.paydet[0].cusT_ID;
                        $scope.TXN_AMOUNT = promise.paydet[0].txN_AMOUNT;
                        $scope.CHANNEL_ID = promise.paydet[0].channeL_ID;
                        $scope.EMAIL = promise.paydet[0].email;
                        $scope.MOBILE_NO = promise.paydet[0].mobilE_NO;
                        $scope.INDUSTRY_TYPE_ID = promise.paydet[0].industrY_TYPE_ID;
                        $scope.WEBSITE = promise.paydet[0].website;
                        $scope.CHECKSUMHASH = promise.paydet[0].checksumhash;
                        $scope.MERC_UNQ_REF = promise.paydet[0].merC_UNQ_REF;

                        $scope.payu_URL = promise.paydet[0].payu_URL;
                        var payu_URL = $scope.payu_URL;

                        var url = payu_URL;
                        var method = 'POST';
                        var params = {
                            "MID": $scope.MID,
                            "ORDER_ID": $scope.ORDER_ID,
                            "CUST_ID": $scope.CUST_ID,
                            "TXN_AMOUNT": $scope.TXN_AMOUNT,
                            "CHANNEL_ID": $scope.CHANNEL_ID,
                            "EMAIL": $scope.EMAIL,
                            "MOBILE_NO": $scope.MOBILE_NO,
                            "INDUSTRY_TYPE_ID": $scope.INDUSTRY_TYPE_ID,
                            "WEBSITE": $scope.WEBSITE,
                            "CHECKSUMHASH": $scope.CHECKSUMHASH,
                            "MERC_UNQ_REF": $scope.MERC_UNQ_REF,
                            "CALLBACK_URL": "http://localhost:51263/api/FeeOnlinePayment/paymentresponsePAYTM/",
                        }

                        FormSubmitter.submit(url, method, params);

                    }
                    else if (qwe.paygtw == "PAYU") {
                        $scope.key = promise.paydet[0].marchanT_ID;

                        $scope.txnid = promise.paydet[0].trans_id;
                        $scope.amount = promise.paydet[0].amount;

                        $scope.productinfo = promise.paydet[0].productinfo;
                        $scope.firstname = promise.paydet[0].firstname;
                        $scope.email = promise.paydet[0].email;
                        $scope.phone = promise.paydet[0].phone;
                        $scope.surl = promise.paydet[0].surl;
                        $scope.furl = promise.paydet[0].furl;
                        $scope.hash = promise.paydet[0].hash;
                        $scope.udf1 = promise.paydet[0].udf1;
                        $scope.udf2 = promise.paydet[0].udf2;
                        $scope.udf3 = promise.paydet[0].udf3;
                        $scope.udf4 = promise.paydet[0].udf4;
                        $scope.udf5 = promise.paydet[0].udf5;
                        $scope.udf6 = promise.paydet[0].udf6;
                        $scope.service_provider = promise.paydet[0].service_provider;

                        $scope.hash_string = promise.paydet[0].hash_string;
                        $scope.payu_URL = promise.paydet[0].payu_URL;

                        //radha

                        // var url = 'https://test.payu.in/_payment';

                        var payu_URL = $scope.payu_URL;
                        var url = payu_URL;
                        var method = 'POST';
                        //$scope.amount = 30;
                        var params = {
                            "key": $scope.key,
                            "txnid": $scope.txnid,
                            "amount": $scope.amount,
                            "productinfo": $scope.productinfo,
                            "firstname": $scope.firstname,
                            "email": $scope.email,
                            "phone": $scope.phone,
                            "udf1": $scope.udf1,
                            "udf2": $scope.udf2,
                            "udf3": $scope.udf3,
                            "udf4": $scope.udf4,
                            "udf5": $scope.udf5,
                            "udf6": $scope.udf6,
                            "service_provider": $scope.service_provider,
                            "hash": promise.paydet[0].hash,
                            "surl": "http://localhost:51263/api/FeeOnlinePayment/paymentresponse/",
                            "furl": "http://localhost:51263/api/FeeOnlinePayment/paymentresponse/"
                        }
                        FormSubmitter.submit(url, method, params);
                    }

                    else if (qwe.paygtw == "RAZORPAY") {

                        $scope.txnamt = Number(promise.tenantagrpaY_Amount);
                        $scope.SaltKey = promise.merchantkey;
                        $scope.orderid = promise.trans_id;

                        $scope.institutioname = $scope.institutioname;
                        $scope.institulogo = $scope.institulogo;

                        $scope.tenantname = promise.get_tenant_details[0].tenantagR_TenantName;
                        $scope.tenantemailid = promise.get_tenant_details[0].tenantagR_EmailId;
                        $scope.tenantaddress = promise.get_tenant_details[0].tenantagR_Address;
                        $scope.tenantmobileno = promise.get_tenant_details[0].tenantagR_ContactNo;
                        $scope.tenant_id = promise.get_tenant_details[0].tenantagR_Id;
                        $scope.tenantagrpay_PaymentDate = new Date(tenantagrpaY_PaymentDate);

                        //   $scope.splitpayinfor = promise.splitpayinformation;
                        $scope.AutoManualFlag = $scope.IMN_AutoManualFlag;
                        $scope.mI_Id = promise.mI_Id;
                        transnumconfigsettings: $scope.RegistrationNumbering;
                        $scope.Pay_type = $scope.all1
                        // $scope.asmaY_Id = promise.asmaY_Id;
                        // $scope.amst_Id = promise.amst_Id;
                    }

                    else if (qwe.paygtw == "EASEBUZZ") {
                        var strdata = promise.strForm;
                        $scope.htmldata = promise.strForm;
                        var e1 = angular.element(document.getElementById("test"));
                        $compile(e1.html($scope.htmldata))(($scope));
                    }

                    //}
                    //else {
                    //    swal(promise.message);
                    //}


                })
        }




        $scope.otherdetails = false;
        //$scope.onSelectMonth = function (monthid) {

        //    $scope.otherdetails = true;
        //    //if ($scope.myForm1.$valid) {
        //    var data = {
        //        "MonthId": monthid.MonthId

        //    };
        //    apiService.create("TenantOnlinePayment/getfloor", data).then(function (promise) {
        //        if (promise.floor_array != null && promise.floor_array.length > 0) {
        //            $scope.floor_array = promise.floor_array;
        //        }
        //    });
        //    // }
        //};

        $scope.month_pay = 0;
        $scope.al_monthclass = function (all) {


            //if ($scope.all1 == true) {
            //    $scope.countflagwater = true;
            //    $scope.countflagrent = true;
            //    $scope.countflagElectricity = true;
            //    $scope.countflagAdvance = true;  
            //}
            //else if ($scope.all1 == false) {

            //    $scope.countflagwater = false;
            //    $scope.countflagrent = false;
            //    $scope.countflagElectricity = false;
            //    $scope.countflagAdvance = false; 
            //}



            //if ($scope.all1 == 'Water Bill' || $scope.all1 == 'Electricity Bill' || $scope.all1 == 'Advance') {
            //    var data = "";
            //    data = {
            //        "TENANTAGR_Id": tenantagR_Id,
            //        "type": $scope.all1

            //    };
            //    apiService.create("TenantOnlinePayment/getamtwaterdetails", data).then(function (promise) {
            //        if (promise.get_online_month_details.length > 0) {
            //            $scope.get_month_details = promise.get_online_month_details;
            //            if ($scope.all1 == "Advance") {
            //                $scope.total_payable = promise.get_online_month_details[0].Name;
            //            }
            //            else {
            //                $scope.monthlistarray = [];
            //                angular.forEach($scope.get_month_details, function (qq) {
            //                    if (qq.selected == true) {
            //                        $scope.monthlistarray.push({ MonthId: aa.MonthId, TENANTAGR_MonthlyRent: aa.TENANTAGR_MonthlyRent, TENANTAGRPAY_PaymentYear: aa.yearid })
            //                        $scope.total_payable += aa.TENANTAGR_MonthlyRent;
            //                        $scope.month_pay = TENANTAGR_MonthlyRent;
            //                    }
            //                });
            //                if ($scope.monthlistarray.length > 0) {
            //                    $scope.otherdetails = true;
            //                }
            //            }


            //        } else {

            //        }
            //    });
            //}
            //else {
            $scope.monthlistarray = [];
            $scope.obj.usercheckCC = all;

            var toggleStatus = $scope.obj.usercheckCC;
            angular.forEach($scope.get_month_details, function (role) {
                role.selected = toggleStatus;
            });

            var ttl_amt = 0;
            $scope.monthlistarray = [];
            angular.forEach($scope.get_month_details, function (qq) {
                if (qq.selected == true) {
                    $scope.monthlistarray.push({ monthid: aa.MonthId, month_name: aa.IVRM_Month_Name, monthamt: qq.monthamt, yearid: qq.yearid, waterbill: qq.waterbill, otherbill: qq.otherbill, Electricalbill: qq.Electricalbill, Rentbill: qq.Rentbill, MonthName: qq.IVRM_Month_Name, yearname: qq.yearid })
                    //$scope.total_payable += qq.TENANTAGR_MonthlyRent;
                    //$scope.month_pay = qq.TENANTAGR_MonthlyRent;


                    ttl_amt += qq.monthamt;
                    //  $scope.month_pay = qq.TENANTAGR_MonthlyRent;
                }
            });
            $scope.total_payable = ttl_amt;
            if ($scope.monthlistarray.length > 0) {
                $scope.otherdetails = true;
            }


            // }



            //if ($scope.obj.usercheckCC == true) {
            //    $scope.getsection();
            //    $scope.classflag = true;
            //}
            //else {
            //    $scope.sectionlist = [];

            //}

        }


        $scope.onSelectMonthdetails = function (monthd) {

            $scope.monthlistarray = [];
            var ttl_rowamt = 0;
            var ttl_amt = 0;
            angular.forEach($scope.get_month_details, function (aa) {
                if (aa.selected == true) {
                    ttl_rowamt = aa.waterbill + aa.Electricalbill + aa.Rentbill + aa.otherbill;
                    ttl_amt += aa.waterbill + aa.Electricalbill + aa.Rentbill + aa.otherbill;

                    $scope.monthlistarray.push({ monthid: aa.MonthId, month_name: aa.IVRM_Month_Name, monthamt: aa.monthamt, waterbill: aa.waterbill, otherbill: aa.otherbill, Electricalbill: aa.Electricalbill, Rentbill: aa.Rentbill, Total: ttl_rowamt, MonthName: aa.IVRM_Month_Name, yearname: aa.yearid })
                    // ttl_amt += (Number(aa.monthamt));
                }


            });
            $scope.total_payable = ttl_amt;
            if ($scope.monthlistarray.length > 0) {
                $scope.otherdetails = true;
            }



        }


        $scope.savepaymentdetails = function () {
            $scope.monthlistarraytemp = [];
            //if ($scope.myForm1.$valid) {
            //    var TENANTAGRPAY_Id = 0;
            //    if ($scope.TENANTAGRPAY_Id > 0) {
            //        TENANTAGRPAY_Id = $scope.TENANTAGRPAY_Id;
            //    }

            //    //var tenantagrpaY_PaymentDate = new Date($scope.tenantagrpaY_PaymentDate).toDateString();
            //    if ($scope.monthlistarray != null && $scope.monthlistarray.length > 0) {
            //        angular.forEach($scope.get_month_details, function (aa) {
            //            if (aa.selected == true) {
            //                $scope.monthlistarraytemp.push({ MonthId: aa.MonthId, TENANTAGRPAY_PaymentYear: aa.yearid, TENANTAGR_MonthlyRent: aa.TENANTAGR_MonthlyRent })
            //            }

            //        });
            //    }
            //    else {
            //        if ($scope.get_month_details_edit != null && $scope.get_month_details_edit.length > 0) {
            //            angular.forEach($scope.get_month_details_edit, function (aa) {
            //                if (aa.selected == true) {
            //                    $scope.monthlistarraytemp.push({ MonthId: aa.MonthId, TENANTAGRPAY_PaymentYear: aa.yearid, TENANTAGR_MonthlyRent: aa.TENANTAGR_MonthlyRent })
            //                }

            //            });
            //        }
            //    }

            //    var tenantagrpaY_PaymentDate = new Date($scope.tenantagrpaY_PaymentDate).toDateString();

            //    var TENANTAGRPAYChequedate = new Date($scope.TENANTAGRPAY_Chequedate).toDateString();

            //    if ($scope.get_tenant_details != null && $scope.get_tenant_details.length > 0) {
            //        $scope.TENANTAGR_Id = $scope.get_tenant_details[0].tenantagR_Id
            //    }
            //    else {
            //        $scope.TENANTAGR_Id = $scope.TENANTAGR_Id.tenantagR_Id
            //    }
            //    var modeofpayment = 0;
            //    if ($scope.IVRMMOD_Id == 'C') {
            //        modeofpayment = 1;
            //    }
            //    if ($scope.IVRMMOD_Id == 'B') {
            //        modeofpayment = 2;
            //    }


            //    if ($scope.IVRMMOD_Id == 'C') {
            //        var data = {
            //            "TENANTAGRPAY_Id": TENANTAGRPAY_Id,
            //            "TENANTAGR_Id": $scope.TENANTAGR_Id,
            //            "TENANTAGRPAY_Amount": $scope.total_payable,
            //            "TENANTAGRPAY_PaymentDate": new Date(tenantagrpaY_PaymentDate).toDateString(),
            //            "TENANTAGRPAY_ModeOfPayment": modeofpayment,
            //            "TENANTAGRPAY_Remarks": $scope.TENANTAGRPAY_Remarks,
            //            "Month_detailsarray": $scope.monthlistarraytemp,
            //            "transnumconfigsettings": $scope.RegistrationNumbering,
            //            //"TENANTAGRPAY_PaymentMonth": $scope.obj.amsT_RegistrationNo,
            //            //"TENANTAGRPAY_PaymentYear": $scope.obj.amsT_AdmNo,

            //        }

            //    }
            //    if ($scope.IVRMMOD_Id == 'B') {
            //        var data = {
            //            "TENANTAGRPAY_Id": TENANTAGRPAY_Id,
            //            "TENANTAGR_Id": $scope.TENANTAGR_Id,
            //            "TENANTAGRPAY_Amount": $scope.total_payable,
            //            "TENANTAGRPAY_PaymentDate": new Date(tenantagrpaY_PaymentDate).toDateString(),
            //            "TENANTAGRPAY_ModeOfPayment": modeofpayment,
            //            "TENANTAGRPAY_Remarks": $scope.TENANTAGRPAY_Remarks,
            //            "Month_detailsarray": $scope.monthlistarraytemp,
            //            "TENANTAGRPAY_Chequedate": new Date(TENANTAGRPAYChequedate).toDateString(),
            //            "TENANTAGRPAY_Chequeno": $scope.TENANTAGRPAY_Chequeno,
            //            "TENANTAGRPAY_Bankname": $scope.TENANTAGRPAY_Bankname,
            //            "transnumconfigsettings": $scope.RegistrationNumbering,

            //        }
            //    }


            //    apiService.create("TenantOfflinePayment/savedata", data).then(function (promise) {

            //        if (promise.returnval == "add") {
            //            swal("Record Saved Successfully");


            //        }
            //        else if (promise.returnval == "false") {
            //            swal("Kindly contact administrator!");
            //        }
            //        $state.reload();
            //    })
            // }
            //else {
            //    $scope.submitted1 = true;
            //    //$scope.address = true;
            //}
        }
        $scope.ttlamt_flg = false;
        //$scope.Editdata = function (EditRecord, allRefrence, allSources, allActivity) {

        //    $scope.address = false;
        //    $scope.Parents = false;
        //    $scope.Others = false;
        //    $scope.DocumentUpload = false;
        //    $scope.edit_flag = true;


        //    $scope.EditId = EditRecord.tenantagrpaY_Id;
        //    var MEditId = $scope.EditId;



        //    apiService.getURI("TenantOfflinePayment/GetSelectedRowDetails/", MEditId).
        //        then(function (promise) {

        //            if (promise.editdata != null && promise.editdata != null) {

        //                $scope.total_payable = promise.editdata[0].tenantagrpaY_Amount;
        //                $scope.TENANTAGRPAY_PaymentDate = promise.editdata[0].tenantagrpaY_PaymentDate;
        //                $scope.TENANTAGRPAY_PaymentMonth = promise.editdata[0].tenantagrpaY_PaymentMonth;
        //                $scope.TENANTAGR_TenantName = promise.editdata[0].tenantagR_TenantName;
        //                $scope.TENANTAGR_EmailId = promise.editdata[0].tenantagR_EmailId;
        //                $scope.TENANTAGR_ContactNo = promise.editdata[0].tenantagR_ContactNo;
        //                $scope.TENANTAGRPAY_Id = promise.editdata[0].tenantagrpaY_Id;
        //                $scope.IVRMMOD_Id = promise.editdata[0].ivrmmoD_Id;
        //                $scope.HRMR_RoomName = promise.editdata[0].hrmR_RoomName
        //                $scope.HRMB_BuildingName == promise.editdata[0].hrmB_BuildingName
        //                $scope.HRMB_Id = promise.editdata[0].hrmB_Id;
        //                $scope.HRMR_Id = promise.editdata[0].hrmR_Id;
        //                $scope.TENANTAGRPAY_Remarks = promise.editdata[0].tenantagrpaY_Remarks;
        //                if (promise.get_month_details != null && promise.get_month_details.length > 0) {
        //                    $scope.get_month_details = promise.get_month_details;
        //                    angular.forEach($scope.get_month_details, function (dd) {
        //                        if (dd.MonthId == $scope.TENANTAGRPAY_PaymentMonth) {
        //                            dd.selected = true;
        //                            if(dd.tenantagR_MonthlyRent == dd.tenantagrpaY_Amount)
        //                            {
        //                                $scope.ttlamt_flg = true;
        //                            }

        //                        }

        //                    })
        //                }

        //            }

        //            if (promise.get_tenant_details != null && promise.get_tenant_details.length > 0) {
        //                $scope.get_tenant_details= promise.get_tenant_details
        //            }




        //        });
        //};


        $scope.isOptionsRequired = function () {
            return !$scope.get_month_details.some(function (item) {
                return item.selected;
            });
        }

        $scope.Editdata = function (EditRecord) {

            $scope.address = false;
            $scope.Parents = false;
            $scope.Others = false;
            $scope.DocumentUpload = false;
            $scope.edit_flag = true;


            $scope.EditId = EditRecord.tenantagrpaY_Id;
            var MEditId = $scope.EditId;



            apiService.getURI("TenantOfflinePayment/GetSelectedRowDetails/", MEditId).
                then(function (promise) {

                    if (promise.editdata != null && promise.editdata != null) {

                        $scope.total_payable = promise.editdata[0].tenantagrpaY_Amount;
                        $scope.TENANTAGRPAY_PaymentDate = promise.editdata[0].tenantagrpaY_PaymentDate;
                        $scope.TENANTAGRPAY_PaymentMonth = promise.editdata[0].tenantagrpaY_PaymentMonth;
                        $scope.TENANTAGR_TenantName = promise.editdata[0].tenantagR_TenantName;
                        $scope.TENANTAGR_EmailId = promise.editdata[0].tenantagR_EmailId;
                        $scope.TENANTAGR_ContactNo = promise.editdata[0].tenantagR_ContactNo;
                        $scope.TENANTAGRPAY_Id = promise.editdata[0].tenantagrpaY_Id;
                        $scope.IVRMMOD_Id = promise.editdata[0].ivrmmoD_Id;
                        $scope.HRMR_RoomName = promise.editdata[0].hrmR_RoomName
                        $scope.HRMB_BuildingName == promise.editdata[0].hrmB_BuildingName
                        $scope.obj.HRMB_Id = promise.editdata[0].hrmB_Id;
                        $scope.HRMR_Id = promise.editdata[0].hrmR_Id;
                        $scope.TENANTAGRPAY_Remarks = promise.editdata[0].tenantagrpaY_Remarks;
                        if (promise.get_month_details != null && promise.get_month_details.length > 0) {
                            $scope.get_month_details_edit = promise.get_month_details;
                            $scope.get_month_details = promise.get_month_details;
                            angular.forEach($scope.get_month_details, function (dd) {
                                if (dd.MonthId == $scope.TENANTAGRPAY_PaymentMonth) {
                                    dd.selected = true;
                                    if (dd.TENANTAGR_MonthlyRent == dd.TENANTAGRPAY_Amount) {
                                        $scope.ttlamt_flg = true;
                                    }

                                }

                            })
                        }

                    }

                    if (promise.get_tenant_details != null && promise.get_tenant_details.length > 0) {
                        $scope.get_tenant_details = promise.get_tenant_details
                    }


                });
        };



        $scope.clickimage = function (radioimage) {
            angular.forEach($scope.paymenttest, function (objectt) {
                if (objectt.fpgD_Image == radioimage) {
                    $scope.qwe.paygtw = objectt.fpgD_PGName;
                    //$scope.onclickloaddata = function (totalamt, students, checkbx, yearlst, customfeegroup, qwe) {
                    $scope.onclickloaddata($scope.totalamt, $scope.students, 1, 1, 1, $scope.qwe);
                    return;
                }
            });
        };




        $scope.onclickpayabledata = function (total_payable) {
            if (total_payable > 0) {
                if ($scope.total_payable > $scope.month_pay) {
                    swal("You cannot pay more than monthly payment!");
                }
            }

        }


        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

    }

})();