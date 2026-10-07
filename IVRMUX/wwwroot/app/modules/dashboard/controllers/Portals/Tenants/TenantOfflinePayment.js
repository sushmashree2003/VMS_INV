(function () {
    'use strict';
    angular
        .module('app')
        .controller('TenantOfflinePaymentController', TenantOfflinePaymentController)

    TenantOfflinePaymentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache', 'blockUI', '$compile']
    function TenantOfflinePaymentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache, blockUI, $compile) {

        //  var studentphtoto = "https://jshsstorage.blob.core.windows.net/files/NoImage.jpg";

        $scope.asmaY_Year = "";


        $scope.obj = {};
        $scope.search = "";
        $scope.edit_flag = false;
        $scope.bankdetails = false;
        //added on 09102017
        $scope.place_flg = false;
        $scope.name_flg = true;
        $scope.otherdetails == false;
        $scope.tenant_details = false;
        $scope.rolenamelist = "";
        $scope.obj.tenantagrpaY_PaymentDate = new Date();
        $scope.searchby = false;
        $scope.itemsPerPage = 5;
        $scope.currentPage = 1;
        $scope.RegistrationNumbering = [];





        //  var transnumconfig = promise.admTransNumSetting;
        //   localStorage.setItem("transnumconfigsettings", JSON.stringify(transnumconfig));
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        var admregcount = 0;
        var admnocount = 0;
        for (var i1 = 0; i1 < transnumconfigsettings.length; i1++) {
            if (transnumconfigsettings.length > 0) {
                $scope.transnumbconfigurationsettings = transnumconfigsettings[i1];

                if (transnumconfigsettings[i1].imN_Flag === "TenantOfflinePayNo") {
                    admregcount += 1;
                    $scope.RegistrationNumbering = transnumconfigsettings[i1];

                }


            }
        }


        $scope.obj.total_payable = 0;
        $scope.obj.total_payable_org=0
        $scope.ttl_amt_pay = 0;
        var ttamt = 0;
        var monthamt = 0;
        var ttfine = 0;
        var ttcnt = 0;
        var balanceamt = 0;
        var excessamt = 0;
        $scope.month_pay = 0;
        $scope.total_fine_paid = 0;
        $scope.total_balance = 0;
        $scope.total_fine = 0;
        $scope.monthly_fine = 0;


        //var institutionid, automanualreceiptnotranum

        //var grouporterm, autoreceipt, receiptformat, mergeinstallment, fineapplicable;
        //var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //// $scope.Mi_Id = configsettings[0].mI_Id;
        //institutionid = configsettings[0].mI_Id;
        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        //var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        //var transactionnumbering = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        //for (var i = 0; i < transactionnumbering.length; i++) {
        //    if (transactionnumbering.length > 0) {
        //        if (transactionnumbering[i].imN_Flag == "Online") {
        //            $scope.transnumbconfigurationsettingsassign = transactionnumbering[i];
        //        }
        //    }
        //}



        //if (configsettings.length > 0) {
        //    grouporterm = configsettings[0].fmC_GroupOrTermFlg;
        //    autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
        //    receiptformat = configsettings[0].fmC_Receipt_Format;
        //    mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
        //    fineapplicable = configsettings[0].fmC_FineEnableDisable;

        //    //added Praveen
        //    if (configsettings[0].fmC_AutoRecieptPrintFlag != null && configsettings[0].fmC_AutoRecieptPrintFlag != undefined) {
        //        $scope.fmC_AutoRecieptPrintFlag = configsettings[0].fmC_AutoRecieptPrintFlag;
        //    }
        //    //end
        //}

        //if (admfigsettings.length > 0) {
        //    var logopath = admfigsettings[0].asC_Logo_Path;
        //}

        // $scope.imgname = logopath;




        $scope.cfg = {};
        $scope.loaddata = function () {
            $scope.get_month_details = [];
            $scope.obj.MonthId = ""
            var pageid = 1;

            apiService.getURI("TenantOfflinePayment/getallofflinedetails", pageid).then(function (promise) {
                if (promise.building_array != null && promise.building_array.length > 0) {
                    $scope.building_array = promise.building_array;
                }
                if (promise.modeofpayment_array != null && promise.modeofpayment_array.length > 0) {
                    $scope.modeofpayment_array = promise.modeofpayment_array;
                    

                    $scope.obj.IVRMMOD_Id = promise.ivrmmoD_Id;

                    //TENANTAGRPAY_Amount
                }
                if (promise.tenant_array != null && promise.tenant_array.length > 0) {
                    $scope.tenant_array = promise.tenant_array;
                }
                if (promise.bank_array != null && promise.bank_array.length > 0) {
                    $scope.bank_array = promise.bank_array;
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

        $scope.onSelectbuilding = function (hrmb_id) {

            //if ($scope.myForm1.$valid) {
            var data = {
                "HRMB_Id": hrmb_id

            };
            apiService.create("TenantOfflinePayment/getfloor", data).then(function (promise) {
                if (promise.floor_array != null && promise.floor_array.length > 0) {
                    $scope.floor_array = promise.floor_array;
                }
            });
            // }
        };

        $scope.onSelectfloor = function (hrmf_id) {


            //if ($scope.myForm1.$valid) {
            var data = {
                "HRMF_Id": hrmf_id,
                "HRMB_Id": $scope.obj.HRMB_Id

            };
            apiService.create("TenantOfflinePayment/getroom", data).then(function (promise) {
                if (promise.room_array != null && promise.room_array.length > 0) {
                    $scope.room_array = promise.room_array;
                }
            });
            //}
        };

        $scope.editdetails = function (user) {

            $scope.all1 = user.tenantagrpaY_Flag;
            // $scope.MonthId = user.tenantagrpaY_PaymentMonth.concat(" ", user.tenantagrpaY_PaymentYear);
            $scope.obj.tenantagrpaY_PaymentDate =new Date(user.tenantagrpaY_PaymentDate);
            $scope.obj.total_payable = user.tenantagrpaY_Amount;
            $scope.month_pay = user.tenantagrpaY_Amount;

            if ($scope.tenantlist != null && $scope.tenantlist.length > 0) {
                for (var i = 0; i < $scope.tenantlist.length; i++) {
                    if (user.tenantagR_Id == $scope.tenantlist[i].tenantagR_Id) {
                        $scope.tenantlist[i].Selected = true;
                        $scope.TENANTAGR_Id = $scope.tenantlist[i];
                        
                    }
                }
            }

        }



        $scope.getTenantdetails = function (hrmr_id) {
            $scope.obj.total_payable = "";
            $scope.obj.IVRMMOD_Id = "";
            $scope.TENANTAGRPAY_Remarks = "";
            $scope.tenantagrpaY_PaymentDate = "";
            $scope.month_pay = 0;
            $scope.total_excess = 0;
            $scope.total_balance = 0;
            $scope.obj.total_payable = 0;
            $scope.ttl_amt_pay = 0;
            ttamt = 0;
            monthamt = 0;
            ttfine = 0;
            ttcnt = 0;
            balanceamt = 0;
            excessamt = 0;
           
            $scope.get_balnce_details = [];
            $scope.get_month_details = [];
            
            //if ($scope.myForm1.$valid) {
            if (($scope.obj.HRMB_Id > 0) && ($scope.obj.HRMF_Id > 0)) {
                $scope.searchby = false;
            }

         

            if ($scope.searchby == false) {
                var data = {
                    "HRMR_Id": hrmr_id,
                    "HRMB_Id": $scope.obj.HRMB_Id,
                    "HRMF_Id": $scope.obj.HRMF_Id,
                    "TENANTAGR_Id": 0,
                    "type": $scope.all1

                };
            }
            else if ($scope.searchby == true) {
                data = {
                    "TENANTAGR_Id": hrmr_id.tenantagR_Id,
                    "type": $scope.all1

                };

            }

            apiService.create("TenantOfflinePayment/gettenantdetails", data).then(function (promise) {
                if (promise.get_tenant_details != null && promise.get_tenant_details.length > 0) {
                    $scope.get_tenant_details = promise.get_tenant_details;
                    $scope.get_tenant_paid_details = promise.get_tenant_paid_details;
                    $scope.get_month_details = promise.get_month_details;
                    $scope.obj.MonthId = ""
                    $scope.tenant_details = true;

                    // 
                    $scope.TENANTAGR_Id = promise.get_tenant_details[0].tenantagR_Id;
                  //  $scope.getmonth($scope.get_tenant_details);
                    //$scope.getmonth($scope.TENANTAGR_Id);
                   
                    //  $scope.getmonth(hrmr_id);

                }

                $scope.obj.MonthId = ""
            });
            //}
        };

        $scope.getMonthlyPaid_details = function (type) {



            var data = {

                "TENANTAGR_Id": $scope.TENANTAGR_Id,
                "type": type

            };



            apiService.create("TenantOfflinePayment/getMonthlyPaid_details", data).then(function (promise) {
                if (promise.tenant_monthly_paid_details != null && promise.tenant_monthly_paid_details.length > 0) {
                    $scope.tenant_monthly_paid_details = promise.tenant_monthly_paid_details;

                    $('#myModalMonthlyDetails').modal('show');

                }
                else {
                    swal("No Record Found!")
                }
            });
            //}
        };


        


        $scope.onclickloaddata = function () {
            if ($scope.radioval == "Place") {

                $scope.place_flg = true;
                $scope.name_flg = false;
            }
            else {
                $scope.name_flg = true;
                $scope.place_flg = false;
            }

            $scope.obj.total_payable = "";
            $scope.obj.IVRMMOD_Id = "";
            $scope.TENANTAGRPAY_Remarks = "";
            $scope.tenantagrpaY_PaymentDate = "";
            $scope.get_tenant_details = [];
            $scope.get_month_details = [];
            $scope.month_pay = 0;
            $scope.total_excess = 0;
            $scope.total_balance = 0;
            $scope.obj.total_payable = 0;
            $scope.ttl_amt_pay = 0;
             ttamt = 0;
             monthamt = 0;
             ttfine = 0;
             ttcnt = 0;
             balanceamt = 0;
            excessamt = 0;
            $scope.obj.MonthId = ""

        }


        //$scope.onselectstudent = function (studentlst) {




        //};


        $scope.studentlst = [];

        $scope.searchfilter = function (objj) {
            var data = "";


            if (objj.search.length >= '2') {
                $scope.studentlst = "";

                data = {

                    "searchfilter": objj.search,

                };
                apiService.create("TenantOfflinePayment/searchfilter", data).then(function (promise) {
                    if (promise.fillstudlist.length > 0) {
                        $scope.tenantlist = promise.fillstudlist;
                        $scope.searchby = true;
                        $scope.obj.HRMB_Id = "";
                        $scope.obj.HRMF_Id = "";
                    } else {
                        $scope.AMST_Id = "";
                        swal("No tenants are found for your search");
                    }
                });
            }


        };

        $scope.getmonth = function (tenant_id) {
            var tenantagR_Id = 0;
            $scope.get_month_details = [];
            $scope.obj.MonthId = ""
            if (tenant_id != null && tenant_id.length > 0) {
                tenantagR_Id = tenant_id[0].tenantagR_Id;
            }
            else {
                tenantagR_Id = tenant_id.tenantagR_Id;
            }

          
            var  data = {

                "TENANTAGR_Id": tenantagR_Id,
                "type": $scope.all1

            };
            apiService.create("TenantOfflinePayment/getmonthdetails", data).then(function (promise) {
                if (promise.get_month_details.length > 0) {
                    $scope.get_month_details = promise.get_month_details;
                    if ($scope.all1 == "Advance") {
                        $scope.obj.total_payable = promise.get_month_details[0].Name;
                    }

                } else {
                    //$scope.AMST_Id = "";
                    //("No tenants are found for your search");
                }
                $scope.obj.MonthId = ""
            });



        };


        $scope.onradiobuttonSelect = function (type) {
            //var data = {
            //    "type": $scope.all1
            //}

            //apiService.create("TenantOfflinePayment/onchangetype", data).then(function (promise) {


            //    if (promise.tenant_array != null && promise.tenant_array.length > 0) {
            //        $scope.tenant_array = promise.tenant_array;
            //    }

            //    if (promise.get_tenant_details != null && promise.get_tenant_details.length > 0) {
            //        $scope.get_tenant_details = promise.get_tenant_online_details;
            //    }
            //    if (promise.get_month_details.length > 0) {
            //        $scope.get_month_details = promise.get_month_details;
            //    }
            //    $scope.paymenttest = promise.fillpaymentgateway;

            //    if (promise.institutiondet.length > 0) {
            //        for (var i = 0; i < promise.institutiondet.length; i++) {
            //            $scope.institutioname = promise.institutiondet[0].mI_Name;
            //            $scope.institulogo = promise.institutiondet[0].mI_Logo;
            //        }
            //    }
            //})

            // $state.reload();


            $scope.month_pay = 0;
            $scope.total_excess = 0;
            $scope.total_balance = 0;
            $scope.obj.total_payable = 0;
            $scope.ttl_amt_pay = 0;
            ttamt = 0;
            monthamt = 0;
            ttfine = 0;
            ttcnt = 0;
            balanceamt = 0;
            excessamt = 0;
            $scope.tenantlist = [];
            $scope.obj.total_payable = "";
            $scope.TENANTAGRPAY_Remarks = "";
            $scope.TENANTAGRPAY_Bankname = "";
            $scope.obj.usercheckCC = "";
            $scope.get_month_details = [];
            $scope.building_array = [];
            $scope.floor_array = [];
            $scope.room_array = [];
            $scope.radioval = "";
            $scope.obj.MonthId = ""

            var all1 =$scope.all1;


        
          //  $state.reload();
            $scope.loaddata();
          //  $scope.all1=all1;
            var toggleStatus = false;
           


            $scope.getmonth();
            angular.forEach($scope.get_month_details, function (role) {
                if (role.selected == true) {
                    role.selected = false;
                }

            });

        }


        $scope.otherdetails = false;
        $scope.onSelectMonth = function (monthid) {

            $scope.otherdetails = true;  //if ($scope.myForm1.$valid) {
            var data = {
                "MonthId": monthid.MonthId

            };
            apiService.create("TenantOfflinePayment/getfloor", data).then(function (promise) {
                if (promise.floor_array != null && promise.floor_array.length > 0) {
                    $scope.floor_array = promise.floor_array;
                }
            });
          
        };


        //$scope.getfine = function (paydate) {

        //    $scope.otherdetails = true;
        //    //if ($scope.myForm1.$valid) {
        //    var data = {
        //        "tenantagrpay_PaymentDate": paydate,
        //        "TENANTAGR_Id": $scope.TENANTAGR_Id,
        //        "MonthdetailsarrayBal": $scope.monthlistarray

        //    };
        //    apiService.create("TenantOfflinePayment/getfine", data).then(function (promise) {

        //        if ($scope.all1 == "Rent") {

        //            if (promise.get_fine_details != null && promise.get_fine_details.length > 0) {
        //                $scope.get_fine_details = promise.get_fine_details;

        //                $scope.monthly_fine = promise.get_fine_details[0].FINEAMOUNT;
        //                if (promise.get_fine_details[0].DIFFDAYS > 0 && promise.get_fine_details[0].DIFFDAYS != undefined) {
        //                    $scope.total_fine_paid = promise.get_fine_details[0].FinePerDay + '*' + promise.get_fine_details[0].DIFFDAYS;
        //                }
        //                else {
        //                    $scope.total_fine_paid = 0;
        //                }

        //                // $scope.monthly_stdfine = promise.get_fine_details[0].FinePerDay + '*' + promise.get_fine_details[0].DIFFDAYS;
        //            }
        //        }
        //        else {
        //            $scope.total_fine_paid = 0;
        //        }
               
        //    });

        //};


        $scope.getfine = function (paydate) {

            $scope.otherdetails = true;
            //if ($scope.myForm1.$valid) {
            var data = {
                "tenantagrpay_PaymentDate": paydate,
                "TENANTAGR_Id": $scope.TENANTAGR_Id,
                "MonthdetailsarrayBal": $scope.monthlistarray

            };
            apiService.create("TenantOfflinePayment/getfine", data).then(function (promise) {
                if ($scope.all1 == "Rent") {
                    if (promise.get_fine_details != null && promise.get_fine_details.length > 0) {
                        $scope.get_fine_details = promise.get_fine_details;

                        $scope.monthly_fine = promise.get_fine_details[0].FINEAMOUNT;
                        $scope.total_fine_paid = promise.get_fine_details[0].FinePerDay + '*' + promise.get_fine_details[0].DIFFDAYS;
                        // $scope.monthly_stdfine = promise.get_fine_details[0].FinePerDay + '*' + promise.get_fine_details[0].DIFFDAYS;
                    }
                } else {
                    $scope.total_fine_paid = 0;
                }
            });

        };

        $scope.month_pay = 0;
        $scope.al_monthclass = function (all) {

            if ($scope.all1 == 'Water Bill' || $scope.all1 == 'Electricity Bill') {
                var data = "";
                data = {
                    "TENANTAGR_Id": tenantagR_Id,
                    "type": $scope.all1

                };
                apiService.create("TenantOnlinePayment/getamtwaterdetails", data).then(function (promise) {
                    if (promise.get_month_details.length > 0) {
                        $scope.get_month_details = promise.get_month_details;


                        $scope.monthlistarray = [];

                        angular.forEach($scope.get_month_details, function (qq) {
                            if (qq.selected == true) {
                                $scope.monthlistarray.push({ MonthId: aa.MonthId, TENANTAGR_MonthlyRent: aa.TENANTAGR_MonthlyRent, TENANTAGRPAY_PaymentYear: aa.yearid, fine: aa.fine })

                            }

                        });

                        if ($scope.monthlistarray.length > 0) {
                            $scope.otherdetails = true;
                        }
                        $scope.ttl_amount();

                    } else {

                    }
                });
            }
            else {
                $scope.monthlistarray = [];
                $scope.obj.usercheckCC = all;

                var toggleStatus = $scope.obj.usercheckCC;
                angular.forEach($scope.get_month_details, function (role) {
                    role.selected = toggleStatus;
                });


                $scope.monthlistarray = [];
                angular.forEach($scope.get_month_details, function (qq) {
                    if (qq.selected == true) {
                        $scope.monthlistarray.push({ MonthId: qq.MonthId })

                    }
                });
                if ($scope.monthlistarray.length > 0) {
                    $scope.otherdetails = true;
                }
                $scope.ttl_amount();
            }




            //if ($scope.obj.usercheckCC == true) {
            //    $scope.getsection();
            //    $scope.classflag = true;
            //}
            //else {
            //    $scope.sectionlist = [];

            //}

        }




        $scope.fine = 0;
        $scope.obj.total_payable = 0;
        $scope.ttl_amt_pay = 0;
        var ttl_amt = 0;
        var ttl_fine = 0;
        $scope.month_id = 0;
        //$scope.onSelectMonthdetails = function (monthd) {
          
        //    $scope.monthlistarray = [];
        //    angular.forEach($scope.get_month_details, function (aa) {
        //        if ((Number(aa.MonthId)) ==  (Number(monthd))) {
        //            $scope.monthlistarray.push({ MonthId: aa.MonthId, yearid: aa.yearid, TENANTAGR_MonthlyRent: aa.TENANTAGR_MonthlyRent, Name: aa.Name, selected: true });
        //        }
        //        //else {
        //        //    $scope.monthlistarray.push({ MonthId: aa.MonthId, yearid: aa.yearid, TENANTAGR_MonthlyRent: aa.TENANTAGR_MonthlyRent, Name: aa.Name, selected: false });
        //        //}
             
        //    });

        //    if ($scope.monthlistarray.length != 0 && $scope.monthlistarray.length==1) {
        //        $scope.otherdetails = true;

        //    }
        //    $scope.ttl_amount();



        //}

        $scope.onSelectMonthdetailsTwo = function (monthd) {

            $scope.monthlistarray = [];
            angular.forEach($scope.get_month_details, function (aa) {
                if ((Number(aa.MonthId)) == (Number(monthd))) {
                    $scope.monthlistarray.push({ MonthId: aa.MonthId, yearid: aa.yearid, TENANTAGR_MonthlyRent: aa.TENANTAGR_MonthlyRent, Name: aa.Name, selected: true });
                }
               

            });

            if ($scope.monthlistarray.length != 0 && $scope.monthlistarray.length == 1) {
                $scope.otherdetails = true;

            }
            $scope.ttl_amount();



        }

      
       // $scope.onSelectMonthdetailsMHJ=

        //onSelectMonthdetailsMFG
        $scope.actualfineflag = false;

        $scope.ttl_amount = function () {
            // var pageid = 1;
            var data = "";
            data = {
                "type": $scope.all1,
                "MonthdetailsarrayBal": $scope.monthlistarray,
                "TENANTAGR_Id":$scope.TENANTAGR_Id

            };
            apiService.create("TenantOfflinePayment/getbalnce", data).then(function (promise) {
                if (promise.get_balnce_details != null && promise.get_balnce_details.length > 0) {
                     ttamt = 0;
                     monthamt = 0;
                     ttfine = 0;
                     ttcnt = 0;
                     balanceamt = 0;
                    excessamt = 0;

                    var actual_fine = 0;
                    var payment_id = 0;

                    $scope.month_pay = 0;
                    $scope.total_excess = 0;
                    $scope.total_balance = 0;
                    $scope.obj.total_payable = 0;
                    $scope.total_payable_org = 0;
                    $scope.ttl_amt_pay = 0;
                    $scope.total_fine = 0;
                    $scope.monthly_fine = 0;

                    $scope.get_balnce_details = promise.get_balnce_details;

                    if ($scope.all1 == "Fine") {
                        angular.forEach($scope.get_balnce_details, function (aa) {
                            actual_fine = Number(aa.ActualFineAmount)
                            payment_id = Number(aa.TENANTAGRPAY_Id)
                        });
                        $scope.actualfineflag = true;
                        $scope.total_fine_payable = actual_fine;
                        $scope.obj.fine = actual_fine;
                        $scope.TENANTAGRPAY_Id = payment_id;
                        $scope.obj.TENANTAGRPAY_Remarks = $scope.get_balnce_details[0].TENANTAGRPAY_Remarks;
                    }
                    else {
                        angular.forEach($scope.get_balnce_details, function (aa) {
                            //if (aa.selected == true) {
                            monthamt = Number(aa.TENANTAGR_MonthlyRent)
                            ttcnt += 1;
                            ttamt += (Number(aa.TENANTAGR_MonthlyRent));
                            balanceamt += (Number(aa.balanceamt));
                            excessamt += (Number(aa.excessamt));
                            //}
                            //else {

                            //}
                        });
                        $scope.obj.total_payable = ttamt + balanceamt;
                        $scope.total_payable_org = ttamt + balanceamt;
                        $scope.total_balance = balanceamt;
                        $scope.total_excess = excessamt;
                        $scope.month_pay = monthamt + ' * ' + ttcnt;
                        //$scope.fine = ttfine;
                    }
                }
            })
        }

        $scope.checkfine = function (fine) {
            if (fine > $scope.total_fine_payable) {
                $scope.obj.fine = $scope.total_fine_payable;
                swal("Payable Amount Is Should be Equal or Less Than Actual Fine Amount");
            }
           
        }

        //$scope.ttl_amount = function () {
        //    $scope.total_payable = 0;
        //    $scope.ttl_amt_pay = 0;
        //    var ttamt = 0;
        //    var monthamt = 0;
        //    var ttfine = 0;
        //    var ttcnt= 0;
        //    angular.forEach($scope.get_month_details, function (aa) {
        //        if (aa.selected == true) {
        //            monthamt = Number(aa.TENANTAGR_MonthlyRent)
        //            ttcnt += 1;
        //            ttfine += (Number(aa.fine));
        //            ttamt += (Number(aa.TENANTAGR_MonthlyRent));
        //        }
        //        else {

        //        }
        //    });
        //    $scope.total_payable = ttamt ;

        //    $scope.month_pay = monthamt + ' * ' + ttcnt;
        //    //$scope.fine = ttfine;
        //};

        $scope.savepaymentdetails = function () {
            $scope.monthlistarraytemp = [];
            if ($scope.myForm1.$valid) {

                if ($scope.all1 != "Fine") {
                    var TENANTAGRPAY_Id = 0;
                    if ($scope.TENANTAGRPAY_Id > 0) {
                        TENANTAGRPAY_Id = $scope.TENANTAGRPAY_Id;
                    }

                    //var tenantagrpaY_PaymentDate = new Date($scope.tenantagrpaY_PaymentDate).toDateString();
                    if ($scope.monthlistarray != null && $scope.monthlistarray.length > 0) {
                        angular.forEach($scope.monthlistarray, function (aa) {
                            if (aa.selected == true) {
                                $scope.monthlistarraytemp.push({ MonthId: aa.MonthId, TENANTAGRPAY_PaymentYear: aa.yearid, TENANTAGR_MonthlyRent: aa.TENANTAGR_MonthlyRent, Name: aa.Name })
                            }

                        });
                    }
                    else {
                        if ($scope.get_month_details_edit != null && $scope.get_month_details_edit.length > 0) {
                            angular.forEach($scope.get_month_details_edit, function (aa) {
                                if (aa.selected == true) {
                                    $scope.monthlistarraytemp.push({ MonthId: aa.MonthId, TENANTAGRPAY_PaymentYear: aa.yearid, TENANTAGR_MonthlyRent: aa.TENANTAGR_MonthlyRent, Name: aa.Name })
                                }

                            });
                        }
                    }

                    var tenantagrpaY_PaymentDate = new Date($scope.obj.tenantagrpaY_PaymentDate).toDateString();

                    var TENANTAGRPAYChequedate = new Date($scope.TENANTAGRPAY_Chequedate).toDateString();

                    if ($scope.get_tenant_details != null && $scope.get_tenant_details.length > 0) {
                        $scope.TENANTAGR_Id = $scope.get_tenant_details[0].tenantagR_Id
                    }
                    else {
                        $scope.TENANTAGR_Id = $scope.TENANTAGR_Id.tenantagR_Id
                    }
                    var modeofpayment = "";;
                    //if ($scope.obj.IVRMMOD_Id == 'C') {
                    //    modeofpayment = 1;
                    //}
                    //if ($scope.obj.IVRMMOD_Id == 'B') {
                    //    modeofpayment = 2;
                    //}

                    if ($scope.modeofpayment_array.length > 0) {
                        angular.forEach($scope.modeofpayment_array, function (aa) {
                            if ($scope.obj.IVRMMOD_Id == aa.ivrmmoD_Id) {
                                modeofpayment = aa.ivrmmoD_Flag;
                            }
                        });
                    }
                    var total_fine_paid = 0;
                    if ($scope.total_fine_paid > 0) {
                        total_fine_paid = $scope.total_fine_paid;
                    }

                    var monthly_fine = 0;
                    if ($scope.monthly_fine > 0) {
                        monthly_fine = $scope.monthly_fine;
                    }



                    if (modeofpayment == 'C') {
                        var data = {
                            "TENANTAGRPAY_Id": TENANTAGRPAY_Id,
                            "TENANTAGR_Id": $scope.TENANTAGR_Id,
                            "TENANTAGRPAY_Amount": $scope.obj.total_payable,
                            "TENANTAGRPAY_PaymentDate": new Date(tenantagrpaY_PaymentDate).toDateString(),
                            "TENANTAGRPAY_ModeOfPayment": $scope.obj.IVRMMOD_Id,
                            "TENANTAGRPAY_Remarks": $scope.obj.TENANTAGRPAY_Remarks,
                            "Month_detailsarray": $scope.monthlistarraytemp,
                            "transnumconfigsettings": $scope.RegistrationNumbering,
                            "type": $scope.all1,
                            "TENANTAGRPAY_ActualFineAmount": monthly_fine, 
                            "TENANTAGRPAY_FinePaidAmount": total_fine_paid
                            //"TENANTAGRPAY_PaymentMonth": $scope.obj.amsT_RegistrationNo,
                            //"TENANTAGRPAY_PaymentYear": $scope.obj.amsT_AdmNo,

                        }

                    }
                    if (modeofpayment == 'B') {
                        var data = {
                            "TENANTAGRPAY_Id": TENANTAGRPAY_Id,
                            "TENANTAGR_Id": $scope.TENANTAGR_Id,
                            "TENANTAGRPAY_Amount": $scope.obj.total_payable,
                            "TENANTAGRPAY_PaymentDate": new Date(tenantagrpaY_PaymentDate).toDateString(),
                            "TENANTAGRPAY_ModeOfPayment": $scope.obj.IVRMMOD_Id,
                            "TENANTAGRPAY_Remarks": $scope.obj.TENANTAGRPAY_Remarks,
                            "Month_detailsarray": $scope.monthlistarraytemp,
                            "TENANTAGRPAY_Chequedate": new Date(TENANTAGRPAYChequedate).toDateString(),
                            "TENANTAGRPAY_Chequeno": $scope.TENANTAGRPAY_Chequeno,
                            "TENANTAGRPAY_Bankname": $scope.TENANTAGRPAY_Bankname,
                            "transnumconfigsettings": $scope.RegistrationNumbering,
                            "type": $scope.all1,
                            "TENANTAGRPAY_ActualFineAmount": monthly_fine,
                            "TENANTAGRPAY_FinePaidAmount": total_fine_paid

                        }
                    }

                    if (modeofpayment == 'U') {
                        var data = {
                            "TENANTAGRPAY_Id": TENANTAGRPAY_Id,
                            "TENANTAGR_Id": $scope.TENANTAGR_Id,
                            "TENANTAGRPAY_Amount": $scope.obj.total_payable,
                            "TENANTAGRPAY_PaymentDate": new Date(tenantagrpaY_PaymentDate).toDateString(),
                            "TENANTAGRPAY_ModeOfPayment": $scope.obj.IVRMMOD_Id,
                            "TENANTAGRPAY_Remarks": $scope.obj.TENANTAGRPAY_Remarks,
                            "Month_detailsarray": $scope.monthlistarraytemp,
                            //"TENANTAGRPAY_Chequedate": new Date(TENANTAGRPAYChequedate).toDateString(),
                            //"TENANTAGRPAY_Chequeno": $scope.TENANTAGRPAY_Chequeno,
                            //"TENANTAGRPAY_Bankname": $scope.TENANTAGRPAY_Bankname,
                            "transnumconfigsettings": $scope.RegistrationNumbering,
                            "type": $scope.all1,
                            "TENANTAGRPAY_ActualFineAmount": monthly_fine,
                            "TENANTAGRPAY_FinePaidAmount": total_fine_paid

                        }
                    }


                    apiService.create("TenantOfflinePayment/savedata", data).then(function (promise) {

                        if (promise.returnval == "add") {
                            swal("Record Saved Successfully");


                        }
                        else if (promise.returnval == "false") {
                            swal("Kindly contact administrator!");
                        }
                        $state.reload();
                    })
                }
                else if ($scope.all1 == "Fine") {

                    if ($scope.TENANTAGRPAY_Id > 0) {
                        TENANTAGRPAY_Id = $scope.TENANTAGRPAY_Id;
                    }

                    if ($scope.monthlistarray != null && $scope.monthlistarray.length > 0) {
                        angular.forEach($scope.monthlistarray, function (aa) {
                            if (aa.selected == true) {
                                $scope.monthlistarraytemp.push({ MonthId: aa.MonthId, TENANTAGRPAY_PaymentYear: aa.yearid, TENANTAGR_MonthlyRent: aa.TENANTAGR_MonthlyRent, Name: aa.Name })
                            }

                        });
                    }
                    else {
                        if ($scope.get_month_details_edit != null && $scope.get_month_details_edit.length > 0) {
                            angular.forEach($scope.get_month_details_edit, function (aa) {
                                if (aa.selected == true) {
                                    $scope.monthlistarraytemp.push({ MonthId: aa.MonthId, TENANTAGRPAY_PaymentYear: aa.yearid, TENANTAGR_MonthlyRent: aa.TENANTAGR_MonthlyRent, Name: aa.Name })
                                }

                            });
                        }
                    }

                    var tenantagrpaY_PaymentDate = new Date($scope.obj.tenantagrpaY_PaymentDate).toDateString();

                    var data = {
                        "TENANTAGRPAY_Id": TENANTAGRPAY_Id,
                        "TENANTAGR_Id": $scope.TENANTAGR_Id,
                        "TENANTAGRPAY_PaymentDate": new Date(tenantagrpaY_PaymentDate).toDateString(),
                        "TENANTAGRPAY_Remarks": $scope.obj.TENANTAGRPAY_Remarks,
                        "Month_detailsarray": $scope.monthlistarraytemp,
                        "TENANTAGRPAY_ActualFineAmount": $scope.total_fine_payable,
                        "TENANTAGRPAY_FinePaidAmount": $scope.obj.fine
                    }
                    apiService.create("TenantOfflinePayment/savefinedata", data).then(function (promise) {

                        if (promise.returnval == "add") {
                            swal("Record Saved Successfully");
                        }
                        else if (promise.returnval == "false") {
                            swal("Kindly contact administrator!");
                        }
                        $state.reload();

                    });

                }
            }
            else {
                $scope.submitted1 = true;
                //$scope.address = true;
            }
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
        //                $scope.obj.IVRMMOD_Id = promise.editdata[0].ivrmmoD_Id;
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

        $scope.Clearid = function () {
            $state.reload();
        };


        $scope.isOptionsRequired = function () {
            return !$scope.get_month_details.some(function (item) {
                return item.selected;
            });
        }

        $scope.onselectmodeofpayment = function (ivrmmoD_ModeOfPayment_Code) {


            $scope.obj.IVRMMOD_Id = ivrmmoD_ModeOfPayment_Code;

            if ($scope.obj.IVRMMOD_Id == 'C') {
            }
            else if ($scope.obj.IVRMMOD_Id == 'B' || $scope.obj.IVRMMOD_Id == 'R' || $scope.obj.IVRMMOD_Id == 'S' || $scope.obj.IVRMMOD_Id == 'U') {
                $scope.bankdetails = true;
            }
            else {

            }

            if ($scope.obj.IVRMMOD_Id == 'C') {
                $scope.bankdetails = false;
            }
            else if ($scope.obj.IVRMMOD_Id == 'B') {
                $scope.bankdetails = true;
            }
        };
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

                        $scope.obj.total_payable = promise.editdata[0].tenantagrpaY_Amount;
                        $scope.obj.tenantagrpaY_PaymentDate = new Date(promise.editdata[0].tenantagrpaY_PaymentDate);
                        $scope.TENANTAGRPAY_PaymentMonth = promise.editdata[0].tenantagrpaY_PaymentMonth;
                        $scope.TENANTAGR_TenantName = promise.editdata[0].tenantagR_TenantName;
                        $scope.TENANTAGR_EmailId = promise.editdata[0].tenantagR_EmailId;
                        $scope.TENANTAGR_ContactNo = promise.editdata[0].tenantagR_ContactNo;
                        $scope.TENANTAGRPAY_Id = promise.editdata[0].tenantagrpaY_Id;
                        $scope.obj.IVRMMOD_Id = promise.editdata[0].ivrmmoD_Id;
                        $scope.HRMR_RoomName = promise.editdata[0].hrmR_RoomName
                        $scope.HRMB_BuildingName == promise.editdata[0].hrmB_BuildingName
                        $scope.obj.HRMB_Id = promise.editdata[0].hrmB_Id;
                        $scope.HRMR_Id = promise.editdata[0].hrmR_Id;
                        $scope.obj.TENANTAGRPAY_Remarks = promise.editdata[0].tenantagrpaY_Remarks;
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

        $scope.onclickpayabledata = function (total_payable) {
            //if ($scope.all1 == 'Rent' || $scope.all1 == 'WaterBill' || $scope.all1 == 'Electricity') {
            //    if (total_payable > 0) {
            //        if ($scope.total_payable > $scope.month_pay) {
            //            swal("You cannot pay more than monthly payment!");
            //        }
            //    }
            //}

            if ($scope.total_payable_org == $scope.obj.total_payable) {

            }
            else {
                var data = "";
                data = {
                    "type": $scope.all1,
                    "MonthdetailsarrayBal": $scope.monthlistarray,
                    "TENANTAGR_Id": $scope.TENANTAGR_Id,
                    "total_payable": $scope.obj.total_payable

                };
                apiService.create("TenantOfflinePayment/getbreakup", data).then(function (promise) {
                    if (promise.get_balnce_details != null && promise.get_balnce_details.length > 0) {
                        excessamt = 0;
                        balanceamt = 0;
                        $scope.get_balnce_details = promise.get_balnce_details;
                        angular.forEach($scope.get_balnce_details, function (aa) {
                            //if (aa.selected == true) {
                            monthamt = Number(aa.TENANTAGR_MonthlyRent)
                            //ttcnt += 1;
                            ttamt += (Number(aa.TENANTAGR_MonthlyRent));
                            balanceamt += (Number(aa.balanceamt));
                            excessamt += (Number(aa.excessamt));
                            //}
                            //else {

                            //}
                        });
                        // $scope.obj.total_payable ;
                        $scope.total_balance = balanceamt;
                        $scope.total_excess = excessamt;
                        //$scope.month_pay = monthamt + ' * ' + ttcnt;
                        //$scope.fine = ttfine;
                    }
                })
            }

           


        }


        $scope.viewdocument = function (month, year, id) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];


            var data = {
                "TENANTAGRPAY_PaymentMonth": month,
                "TENANTAGRPAY_PaymentYear": year,
                "TENANTAGRPAY_Id": id
            }

            apiService.create("TenantOfflinePayment/viewuploadflies", data).then(function (promise) {
                if (promise !== null) {

                    $scope.imagenew = promise.mI_Logo;                    $scope.name = promise.viewarray[0].tenantagR_TenantName;                    $scope.email = promise.viewarray[0].tenantagR_EmailId;                    $scope.phonenumber = promise.viewarray[0].tenantagR_ContactNo;                    $scope.agreementfromdate = promise.viewarray[0].tenantagR_AgrFromDate;                    $scope.agreementtodate = promise.viewarray[0].tenantagR_AgrToDate;                    $scope.receiptNo = promise.viewarray[0].tenantagrpaY_ReceiptNo;




                    $scope.uploadfilesdetails = promise.viewarray;
                    if (promise.viewarray !== null && promise.viewarray.length > 0) {
                        $scope.uploaddocfiles = promise.viewarray;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.filepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printDeviation").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        //added New
      
    }
})();