(function () {
    'use strict';
    angular
        .module('app')
        .controller('TADA_Advance_ApplyController', TADA_Advance_ApplyController)
    TADA_Advance_ApplyController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function TADA_Advance_ApplyController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {
        $scope.currentPage = 1;
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        else {
            paginationformasters = 10;
        }
        $scope.srresult = [];
        $scope.erresult = [];


        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.ismeridian = true;
        $scope.countBalance = false;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian; todate
        };

        $scope.plMaxdate = new Date();
        $scope.plMaxdate.setDate($scope.plMaxdate.getDate());
        $scope.searchValue = "";
        $scope.Isdisabletxtss = true;
        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        };
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.searchValuetwo = "";
        $scope.onpageload = function () {
            var pageid = 1;
            apiService.getURI("TADA_Advance_Apply/getalldata", pageid).
                then(function (promise) {
                    //Allowense
                    if (promise.allowense != null && promise.allowense.length > 0) {                      
                        $scope.get_Report = promise.getReport;
                        $scope.presentCountgrid = $scope.get_Report.length;
                        $scope.getClient = promise.client_Master;
                        $scope.getClientwo = promise.client_Master;
                        $scope.getstate = promise.state;
                        $scope.countBalance = promise.countBalance;
                        if ($scope.countBalance == true) {
                            swal("Previous TA-DA Adavance Is Pending,Document Was Not Uploaded !", "", "info");
                        }
                    }
                    else {
                        swal("Emergency TADA Extenssion Is Not Given !");
                    }
                   
                })
        }
        $scope.EmpFormula = [];
        $scope.EmpFormula = function () {
            //formulall
            var b = 0; var c = 0; var d = 0; var e = 0;
            if ($scope.chk_AccAmt === true && $scope.txtslotAcc > 0) {
                b += parseFloat($scope.totaL_AccmamtTwo);
            }
            else {
                b = 0;
            }
            if ($scope.chk_TransAmt === true) {
                c += parseFloat($scope.totaL_transTwo);
            }
            else {
                c = 0;
            }
            if ($scope.chk_OthersAmt === true) {
                d += parseFloat($scope.totaL_otherTwo);
            }
            else {
                d = 0;
            }
            if ($scope.chk_FoodAmt === true && $scope.txtslot > 0) {
                e += parseFloat($scope.tOtaL_foodamt);
            }
            else {
                e = 0;
            }
            $scope.txt_approxTotAmount = e + b + c + d;
            if ($scope.chk_FoodAmt === false && $scope.chk_OthersAmt === false && $scope.chk_TransAmt === false && $scope.chk_AccAmt === false) {
                $scope.txt_approxTotAmount = "";
            }
        };
        $scope.Foodamt_Chk = function () {
            if ($scope.chk_FoodAmt === true) {
                var date2 = new Date($scope.startdate);
                var date1 = new Date($scope.enddate);
                var enddate = $filter('date')(new Date($scope.enddate), 'dd-MM-yyyy');
                var startdate = $filter('date')(new Date($scope.startdate), 'dd-MM-yyyy');
                var adddays = 0;
                if (enddate > startdate) {
                    adddays = 1;
                }
                else {
                    adddays = 1;
                }
                var timeDiff = Math.abs(date1.getTime() - date2.getTime());
                if (timeDiff == 0) {
                    timeDiff = 1;
                }
                var DateDifference = Math.ceil(timeDiff / (1000 * 3600 * 24) + adddays);
                var a = 0;
                $scope.daydiffrence = DateDifference;
                var VTADAA_DepartureTime = $scope.vtadaaaA_DepartureTime;
                var VTADAA_ArrivalTime = $scope.vtadaaaA_ArrivalTime;
                $scope.srresult = $filter('date')(VTADAA_DepartureTime, 'HH:mm');
                $scope.erresult = $filter('date')(VTADAA_ArrivalTime, 'HH:mm');
                var logik = moment(date2).format('DD/MM/YYYY');
                var logictwo = moment(date1).format('DD/MM/YYYY');
                var Departure = logik + 'T' + $scope.srresult;
                var Arrival = logictwo + 'T' + $scope.erresult;
                var departureDate = moment(Departure, "DD/MM/YYYYTHH:mm");
                var arrivalDate = moment(Arrival, "DD/MM/YYYYTHH:mm");
                var duration = moment.duration(arrivalDate.diff(departureDate));
                var hours = Math.floor(duration.asHours());
                var travelhour = hours;

                // var travelhour = (Number(hours) - Number($scope.VTADACM_TravelHour));
                if (travelhour > 0) {
                    var accdays = travelhour / 8;
                    var slots = Math.round(accdays);
                    if (slots > 0) {
                        $scope.foodtotlslots = slots;
                    }
                    else {
                        swal("Food Amount Not Provide !");
                    }
                }
                else {
                    $scope.chk_FoodAmt = false;
                    swal("Food Amount Not Provide !");
                }
                $scope.EmpFormula();
                $scope.txt_amt = '';
            }
            else {
                $scope.daydiffrence = "";
                $scope.tOtaL_foodamt = "";
                $scope.foodtotlslots = "";
                $scope.txtslot = "";
                $scope.EmpFormula();
            }
        };
        //added by adarsh
        $scope.clientnames_rem = [{ itrS_Id1: 'trans12' }];
        $scope.addgrnrowsclient = function () {
            if ($scope.clientnames_rem.length > 1) {
                for (var x = 0; x === $scope.clientnames_rem.length; x++) {
                    var id1 = $scope.clientnames_rem[x].itrS_Id1;
                    var lastChar1 = id1.substr(id1.length - 1);
                    $scope.cnt1 = parseInt(lastChar1);
                }
            }
            $scope.cnt1 = $scope.cnt1 + 1;
            $scope.tet = 'trans' + $scope.cnt1;
            var newItemNo1 = $scope.cnt1;
            $scope.clientnames_rem.push({ 'itrS_Id1': 'trans12' + newItemNo1 });
        };

        $scope.removeclient = function (index, data) {
            var newItemNo = $scope.clientnames_rem.length - 1;
            $scope.clientnames_rem.splice(index, 1);

        };
        $scope.accMandtionamt_Chk = function () {

            if ($scope.chk_AccAmt === true) {
                var a = 0;
                var date2 = new Date($scope.startdate);
                var date1 = new Date($scope.enddate);
                var enddate = $filter('date')(new Date($scope.enddate), 'dd-MM-yyyy');
                var startdate = $filter('date')(new Date($scope.startdate), 'dd-MM-yyyy');
                var adddays = 0;
                if (enddate > startdate) {
                    adddays = 1;
                }
                else {
                    adddays = 1;
                }

                var timeDiff = Math.abs(date1.getTime() - date2.getTime());
                if (timeDiff == 0) {
                    timeDiff = 1;
                }
                var DateDifference = Math.ceil(timeDiff / (1000 * 3600 * 24) + adddays);
                $scope.daydiffrence_two = DateDifference;
                var VTADAA_DepartureTime = $scope.vtadaaA_DepartureTime;
                var VTADAA_ArrivalTime = $scope.vtadaaA_ArrivalTime;
                $scope.srresult = $filter('date')(VTADAA_DepartureTime, 'HH:mm');
                $scope.erresult = $filter('date')(VTADAA_ArrivalTime, 'HH:mm');
                var logik = moment(date2).format('DD/MM/YYYY');
                var logictwo = moment(date1).format('DD/MM/YYYY');
                var Departure = logik + 'T' + $scope.srresult;
                var Arrival = logictwo + 'T' + $scope.erresult;
                var departureDate = moment(Departure, "DD/MM/YYYYTHH:mm");
                var arrivalDate = moment(Arrival, "DD/MM/YYYYTHH:mm");
                var duration = moment.duration(arrivalDate.diff(departureDate));
                var hours = Math.floor(duration.asHours());
                var travelhour = (Number(hours));
                //  var travelhour = (Number(hours) - Number($scope.VTADACM_TravelHour));
                if (travelhour > 0) {
                    var accdays = travelhour / 24;
                    var slots = Math.round(accdays);
                    if (slots > 0) {
                        $scope.Acctotolslots = slots;
                    }
                    else {
                        $scope.chk_AccAmt = false;
                        swal("Accommodation Amount Is Not provide U Stay Atleast One Day !");
                        $scope.daydiffrence_two = "";
                        $scope.totaL_AccmamtTwo = "";
                        $scope.txtslotAcc = "";
                        $scope.Acctotolslots = "";
                        $scope.EmpFormula();

                    }

                }
                else {
                    $scope.chk_AccAmt = false;
                    swal("Accommodation Amount Is Not provide U Stay Atleast One Day !");
                    $scope.daydiffrence_two = "";
                    $scope.totaL_AccmamtTwo = "";
                    $scope.txtslotAcc = "";
                    $scope.Acctotolslots = "";
                    $scope.EmpFormula();

                }

                $scope.EmpFormula();

            }
            else {
                $scope.daydiffrence_two = "";
                $scope.totaL_AccmamtTwo = "";
                $scope.txtslotAcc = "";
                $scope.Acctotolslots = "";
                $scope.chk_AccAmt = false;
                $scope.EmpFormula();
            }
        };
        $scope.TransAmt_chk = function () {

            if ($scope.chk_TransAmt === true) {
                var a = 0;
                var date2 = new Date($scope.startdate);
                var date1 = new Date($scope.enddate);
                var enddate = $filter('date')(new Date($scope.enddate), 'dd-MM-yyyy');
                var startdate = $filter('date')(new Date($scope.startdate), 'dd-MM-yyyy');

                var adddays = 0;
                if (enddate > startdate) {
                    adddays = 1;
                }
                var timeDiff = Math.abs(date1.getTime() - date2.getTime());
                if (timeDiff == 0) {
                    timeDiff = 1;
                }
                var DateDifference = Math.ceil(timeDiff / (1000 * 3600 * 24) + adddays);
                var totamt = $scope.vtadacM_TransportAmt;

                $scope.totaL_transTwo = totamt;
                $scope.EmpFormula();
            }
            else {
                $scope.daydiffrence_three = "";
                $scope.totaL_transTwo = "";
                $scope.EmpFormula();
            }
        };
        $scope.OtheraAmt_chk = function () {
            if ($scope.OtherTotal_amt != null) {
                var a = 0;
                var date2 = new Date($scope.startdate);
                var date1 = new Date($scope.enddate);
                var timeDiff = Math.abs(date1.getTime() - date2.getTime());
                var DateDifference = Math.ceil(timeDiff / (1000 * 3600 * 24) + 1);
                if ($scope.chk_OthersAmt === true) {
                    var totamt = $scope.OtherTotal_amt;
                    $scope.totaL_otherTwo = totamt;
                    $scope.EmpFormula();
                }
                else {

                    $scope.daydiffrence_Four = "";
                    $scope.totaL_otherTwo = "";
                    $scope.OtherTotal_amt = "";
                    $scope.EmpFormula();
                }
            }
            else {
                $scope.chk_OthersAmt = false;
                $scope.OtherTotal_amt = "";
                $scope.totaL_otherTwo = "";
            }
        };
        $scope.clientaddress = function () {
            if ($scope.getClient != null && $scope.getClient.length > 0 && $scope.Client_Id > 0) {
                for (var i = 0; i < $scope.getClient.length; i++) {
                    if ($scope.Client_Id == $scope.getClient[i].ismmclT_Id) {
                        //ISMMCLT_Address
                        $scope.txt_Address = $scope.getClient[i].ismmclT_Address;
                    }

                }
            }
            else {
                $scope.txt_Address = "";
            }
        };
        $scope.Dropdown_city = function () {
            if ($scope.startdate != null && $scope.enddate != null && $scope.startdate != "" && $scope.enddate != "") {
                if ($scope.VTADAAA_Id > 0) {

                }
                else {
                    $scope.chk_FoodAmt = false; $scope.chk_AccAmt = false; $scope.chk_TransAmt = false; $scope.chk_OthersAmt = false; $scope.txt_approxTotAmount = "";
                    $scope.OtherTotal_amt = ""; $scope.daydiffrence_three = "";
                    $scope.totaL_transTwo = ""; $scope.tOtaL_foodamt = "";
                    $scope.daydiffrence = ""; $scope.daydiffrence_two = "";
                    $scope.totaL_otherTwo = ""; $scope.totaL_AccmamtTwo = "";
                }
                if ($scope.IVRMMCT_Id != null && $scope.IVRMMCT_Id != "" && $scope.IVRMMCT_Id > 0) {
                    var data = {
                        "IVRMMCT_Id": $scope.IVRMMCT_Id
                    };
                    apiService.create("TADA_Advance_Apply/TadaAllwence", data).then(function (promise) {
                        $scope.get_allowence = [];
                        if (promise.allowense != null) {
                            $scope.get_allowence = promise.allowense;
                            if ($scope.get_allowence !== null && $scope.get_allowence.length > 0) {
                                $scope.vtadacM_FoodAmt = $scope.get_allowence[0].vtadacM_FoodAmt;
                                $scope.vtadacM_AccommodationAmt = $scope.get_allowence[0].vtadacM_AccommodationAmt;
                                $scope.vtadacM_TransportAmt = $scope.get_allowence[0].vtadacM_TransportAmt;
                                // $scope.VTADACM_TravelHour = $scope.get_allowence[0].vtadacM_TravelHour;

                            }
                            else {
                                swal("For Your Designation For This City TA DA Amount Is Not Mapped", "Contact Admin");
                            }
                        }
                        else {
                            swal("For Your Designation For This City TA DA Amount Is Not Mapped", "Contact Admin");
                        }

                    });
                }
                else {
                    swal("Plaese Select City");
                    $scope.txt_amt = "";
                }
                $scope.EmpFormula();
            }
            else {
                swal("Please Select Dates");
            }
        };
        $scope.MultiClient = function () {
            $('#showclient').modal('show');
        }
        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.submitted = true;
            var totalamount = 0;
            $scope.selecteddetails = [];
            $scope.client_names = [];
            if ($scope.txt_amt != null && $scope.txt_amt != "") {
                totalamount = $scope.txt_amt;
            }
            if ($scope.txt_approxTotAmount != "" && $scope.txt_approxTotAmount != null) {
                totalamount = $scope.txt_approxTotAmount;
            }
            if (totalamount > 0) {

                if ($scope.myForm.$valid) {
                    var VTADAA_DepartureTime = $scope.vtadaaaA_DepartureTime;
                    var VTADAA_ArrivalTime = $scope.vtadaaaA_ArrivalTime;
                    $scope.sresult = $filter('date')(VTADAA_DepartureTime, 'HH:mm');
                    $scope.eresult = $filter('date')(VTADAA_ArrivalTime, 'HH:mm');
                    if ($scope.chk_FoodAmt === true) {
                        $scope.foodamount_id = $scope.foodamount_id === undefined || $scope.foodamount_id === null ||
                            $scope.foodamount_id === "" ? 0 : $scope.foodamount_id;
                        $scope.selecteddetails.push({
                            Type: 'Food', Amount: $scope.tOtaL_foodamt,
                            Remarks: $scope.Food_Remarks, VTADAAAD_Id: $scope.foodamount_id,
                            VTADAAAD_TotalSlots: $scope.foodtotlslots, VTADAAAD_Slots: $scope.txtslot
                        });
                    }
                    if ($scope.chk_AccAmt === true) {
                        $scope.accamount_id = $scope.accamount_id === undefined || $scope.accamount_id === null ||
                            $scope.accamount_id === "" ? 0 : $scope.accamount_id;

                        $scope.selecteddetails.push({
                            Type: 'Accommodation', Amount: $scope.totaL_AccmamtTwo,
                            Remarks: $scope.Acc_Remarks, VTADAAAD_Id: $scope.accamount_id,
                            VTADAAAD_TotalSlots: $scope.Acctotolslots, VTADAAAD_Slots: $scope.txtslotAcc
                        });
                    }
                    if ($scope.chk_TransAmt === true) {
                        $scope.transamount_id = $scope.transamount_id === undefined || $scope.transamount_id === null ||
                            $scope.transamount_id === "" ? 0 : $scope.transamount_id;
                        $scope.selecteddetails.push({
                            Type: 'Travel', Amount: $scope.totaL_transTwo,
                            Remarks: $scope.transport_Remarks, VTADAAAD_Id: $scope.transamount_id
                        });
                    }
                    if ($scope.chk_OthersAmt === true) {
                        $scope.othersamount_id = $scope.othersamount_id === undefined || $scope.othersamount_id === null ||
                            $scope.othersamount_id === "" ? 0 : $scope.othersamount_id;
                        $scope.selecteddetails.push({
                            Type: 'Other', Amount: $scope.totaL_otherTwo,
                            Remarks: $scope.other_Remarks, VTADAAAD_Id: $scope.othersamount_id
                        });
                    }
                    //added by adarsh
                    if ($scope.clientnames_rem.length > 0) {
                        angular.forEach($scope.clientnames_rem, function (itmx) {
                            if (true) {
                                $scope.client_names.push({
                                    ISMMCLT_Id: itmx.Client_Id,
                                    VTADDAMCF_Remarks: itmx.VTADDAMCF_Remarks
                                });
                            }
                        });
                    }
                    var data = {
                        "IVRMMCT_Id": $scope.IVRMMCT_Id,
                        "VTADAAA_FromDate": new Date($scope.startdate).toDateString(),
                        "VTADAAA_ToDate": new Date($scope.enddate).toDateString(),
                        // "VTADAAA_ClientId": $scope.Client_Id,
                        "VTADAAA_ClientId": 0,
                        "VTADAAA_TotalAppliedAmount": totalamount,
                        "VTADAAA_ToAddress": $scope.txt_Address,
                        //"VTADAAA_Remarks": $scope.txt_Remarks,
                        "AllowenceArray": $scope.selecteddetails,
                        "VTADAAA_Id": $scope.VTADAAA_Id,
                        "VTADAAA_DepartureTime": $scope.sresult,
                        "VTADAAA_ArrivalTime": $scope.eresult,
                        "VTADAAA_ClientMultiple": $scope.vtadaaA_ClientMultiple,
                         multiple_client: $scope.client_names
                    };
                    apiService.create("TADA_Advance_Apply/savedta", data).then(function (promise) {
                        if (promise.returnvalue === true) {
                            if (promise.returnval === "Insert") {
                                swal("Record Saved Successfully", "", "success");
                            }
                            if (promise.returnval === "Failed") {
                                swal("Record Not saved", "", "info");
                            }
                            if (promise.returnval === "Duplicate") {
                                swal("Record Already Exist", "", "warning");
                            }
                            if (promise.returnval === "Update") {
                                swal("Record Update Successfully", "", "success");
                            }
                            if (promise.returnval === "UpdateFailed") {
                                swal("Record Not Update");
                            }
                            if (promise.returnval === "") {
                                swal("please contact administrator !");
                            }
                            $state.reload();
                        }
                        if (promise.returnvalue === false) {
                            if (promise.returnval === "") {
                                swal("please contact Administrator !");
                            }
                        }
                    });
                }
            }
            else {
                swal("Please Fill  Total Amount")
            }


        };
        //=====================Editrecord....
        $scope.edit = function (task) {
            $scope.get_allowence = [];
            $scope.VTADAAA_Id = task.vtadaaA_Id;
            $scope.vtadaaA_ClientMultiple = "";
            $scope.countBalance = false;
            var data = {
                "VTADAAA_Id": $scope.VTADAAA_Id
            };
            apiService.create("TADA_Advance_Apply/edit", data).then(function (promise) {
                if (promise.editArray !== null && promise.editArray.length > 0) {
                    $scope.IVRMMS_Id = promise.editArray[0].ivrmmS_Id;
                    $scope.Client_Id = promise.editArray[0].vtadaaA_ClientId;
                    $scope.vtadaaA_ClientMultiple = promise.editArray[0].vtadaaA_ClientMultiple;
                    $scope.startdate = new Date(promise.editArray[0].vtadaaA_FromDate);
                    $scope.enddate = new Date(promise.editArray[0].vtadaaA_ToDate);
                    $scope.getCity = promise.editArray;
                    $scope.ivrmmcT_Name = promise.editArray[0].ivrmmcT_Name;
                    $scope.IVRMMCT_Id = promise.editArray[0].ivrmmcT_Id;
                    $scope.txt_amt = promise.editArray[0].vtadaaA_TotalAppliedAmount;
                    $scope.txt_Address = promise.editArray[0].vtadaaA_ToAddress;
                    $scope.txt_Remarks = promise.editArray[0].vtadaaA_Remarks;
                    $scope.vtadaaaA_DepartureTime = moment(promise.editArray[0].vtadaaA_DepartureTime, 'HH:mm a').format();
                    $scope.vtadaaaA_ArrivalTime = moment(promise.editArray[0].vtadaaA_ArrivalTime, 'HH:mm a').format();

                    var enddate = $filter('date')(new Date(promise.editArray[0].vtadaaA_ToDate), 'dd-MM-yyyy');
                    var startdate = $filter('date')(new Date(promise.editArray[0].vtadaaA_FromDate), 'dd-MM-yyyy');
                    if (promise.editArrayTwo !== null && promise.editArrayTwo.length > 0) {
                        $scope.txt_approxTotAmount = promise.editArray[0].vtadaaA_TotalAppliedAmount;
                        $scope.txt_amt = "";
                        var date2 = new Date($scope.startdate);
                        var date1 = new Date($scope.enddate);

                        // var enddate = $filter('date')(new Date($scope.enddate), 'dd-MM-yyyy');
                        // var startdate = $filter('date')(new Date($scope.startdate), 'dd-MM-yyyy');
                        var adddays = 0;
                        if (enddate > startdate) {
                            adddays = 1;
                        }
                        var timeDiff = Math.abs(date1.getTime() - date2.getTime());
                        var DateDifference = Math.ceil(timeDiff / (1000 * 3600 * 24) + 1);
                        for (var i = 0; i < promise.editArrayTwo.length; i++) {
                            if (promise.editArrayTwo[i].vtadaaaD_ExpenditureHead == "Food") {
                                $scope.tOtaL_foodamt = promise.editArrayTwo[i].vtadaaaD_Amount;
                                $scope.Food_Remarks = promise.editArrayTwo[i].vtadaaaD_Remarks;
                                $scope.chk_FoodAmt = true;
                                $scope.foodamount_id = promise.editArrayTwo[i].vtadaaaD_Id;
                                $scope.daydiffrence = DateDifference;
                                $scope.foodtotlslots = promise.editArrayTwo[i].vtadaaaD_TotalSlots;
                                $scope.txtslot = promise.editArrayTwo[i].vtadaaaD_Slots;
                            }
                            if (promise.editArrayTwo[i].vtadaaaD_ExpenditureHead == "Accommodation") {
                                $scope.totaL_AccmamtTwo = promise.editArrayTwo[i].vtadaaaD_Amount;
                                $scope.Acc_Remarks = promise.editArrayTwo[i].vtadaaaD_Remarks;
                                $scope.chk_AccAmt = true;
                                $scope.accamount_id = promise.editArrayTwo[i].vtadaaaD_Id;
                                $scope.daydiffrence_two = DateDifference;
                                $scope.Acctotolslots = promise.editArrayTwo[i].vtadaaaD_TotalSlots;
                                $scope.txtslotAcc = promise.editArrayTwo[i].vtadaaaD_Slots;
                            }
                            if (promise.editArrayTwo[i].vtadaaaD_ExpenditureHead == "Travel") {
                                $scope.totaL_transTwo = promise.editArrayTwo[i].vtadaaaD_Amount;
                                $scope.transport_Remarks = promise.editArrayTwo[i].vtadaaaD_Remarks;
                                $scope.chk_TransAmt = true;
                                $scope.transamount_id = promise.editArrayTwo[i].vtadaaaD_Id;
                                // $scope.daydiffrence_three = DateDifference;
                            }
                            if (promise.editArrayTwo[i].vtadaaaD_ExpenditureHead == "Other") {
                                $scope.totaL_otherTwo = promise.editArrayTwo[i].vtadaaaD_Amount;
                                $scope.OtherTotal_amt = promise.editArrayTwo[i].vtadaaaD_Amount;
                                $scope.other_Remarks = promise.editArrayTwo[i].vtadaaaD_Remarks;
                                $scope.chk_OthersAmt = true;
                                $scope.othersamount_id = promise.editArrayTwo[i].vtadaaaD_Id;
                                $scope.daydiffrence_Four = DateDifference;
                            }
                        }
                        $scope.EmpFormula();
                    }
                    if (promise.allowense !== null && promise.allowense.length > 0) {
                        $scope.get_allowence = promise.allowense;
                        $scope.vtadacM_FoodAmt = $scope.get_allowence[0].vtadacM_FoodAmt;
                        $scope.vtadacM_AccommodationAmt = $scope.get_allowence[0].vtadacM_AccommodationAmt;
                        $scope.vtadacM_TransportAmt = $scope.get_allowence[0].vtadacM_TransportAmt;
                        $scope.VTADACM_TravelHour = $scope.get_allowence[0].vtadacM_TravelHour;
                    }
                    //addded by adarsh 
                    if (promise.editmuticlientarray !== null && promise.editmuticlientarray.length > 0) {
                        $scope.clientnames_rem = promise.editmuticlientarray;
                        angular.forEach($scope.clientnames_rem, function (dd) {
                            dd.VTADDAMCF_Remarks = dd.vtaddamcF_Remarks;
                            dd.Client_Id = dd.ismmclT_Id;
                        });
                    }

                }
                else {
                    if (promise.returnvalue === true) {
                        if (promise.returnval === "Firststage") {
                            swal("Result Is Wait Authorization ! ");
                            $scope.VTADAAA_Id = "";
                            $state.reload();
                        }
                    }
                }
                $scope.scroll();
            });
        };
        $scope.Deletedata = function (item, SweetAlert) {
            $scope.VTADAAA_Id = item.vtadaaA_Id;
            var data = {
                "VTADAAA_Id": $scope.VTADAAA_Id

            }
            var dystring = "";
            if (item.vtadaaA_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.vtadaaA_ActiveFlg == false) {
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
                        apiService.create("TADA_Advance_Apply/deactive", data).
                            then(function (promise) {
                                if (promise.returnvalue === true) {

                                    if (promise.returnval === "Delete") {
                                        swal("Record " + dystring + "d Successfully!!!", "", "success");
                                    }
                                    if (promise.returnval === "NotDelete") {
                                        swal("Record " + dystring + "d Successfully!!!", "", "success");
                                    }
                                    if (promise.returnval === "Firststage") {
                                        swal("Result Is Wait Authrization ! ", "", "warning");
                                    }
                                }
                                else {
                                    swal("Please Contact Administrator!!!", "", "warning");
                                }
                                $state.reload();


                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!", "", "cancel");
                    }
                    $state.reload();
                });
        }
        $scope.FromDates = function () {
            $scope.enddate = "";
        }

        $scope.Getdatas = function () {
            if ($scope.startdate != null && $scope.enddate != null && $scope.startdate != "" && $scope.enddate != "") {
                var data = {
                    "VTADAAA_FromDate": new Date($scope.startdate).toDateString(),
                    "VTADAAA_ToDate": new Date($scope.enddate).toDateString(),
                };
                apiService.create("TADA_Advance_Apply/TadaAllwence", data).then(function (promise) {
                    if (promise.client_Master != null && promise.client_Master.length > 0) {
                        $scope.chk_FoodAmt = false;
                        $scope.chk_AccAmt = false;
                        $scope.chk_TransAmt = false;
                        $scope.chk_OthersAmt = false;
                        $scope.Foodamt_Chk();
                        $scope.accMandtionamt_Chk();
                        $scope.TransAmt_chk();
                        $scope.OtheraAmt_chk();
                    }
                    else {
                        $scope.startdate = "";
                        $scope.enddate = "";
                        swal("Planer Was Not Created for Slected Between Dates!", "", "warning");
                        $state.reload();
                    }

                });

            }
            else {
                swal("Please Select From Date..", "", "warning");
                $scope.startdate = "";
                $scope.enddate = "";
            }
        };
        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.Approximate = function () {
            if ($scope.txt_amt != null && $scope.txt_amt != "" && $scope.txt_amt > 0) {
                $scope.txt_approxTotAmount = "";
            }
        };
        $scope.Dropdown_State = function () {
            $scope.get_allowence = [];
            $scope.getCity = [];
            if ($scope.startdate != null && $scope.enddate != null && $scope.startdate != "" && $scope.enddate != "") {
                if ($scope.IVRMMS_Id != null && $scope.IVRMMS_Id != "" && $scope.IVRMMS_Id > 0) {
                    var data = {
                        "IVRMMS_Id": $scope.IVRMMS_Id
                    };
                    apiService.create("TADA_Advance_Apply/getstate", data).then(function (promise) {

                        if (promise.city_Type.length != null && promise.city_Type.length > 0) {

                            $scope.getCity = promise.city_Type;
                        }
                        else {
                            swal("City Is Not Mapped  ! ");

                            $state.reload();
                        }
                    });
                }
                else {
                    swal("Please Select State", "", "warning");
                }
            }
            else {
                swal("Please Select Dates", "", "warning");
            }

        };

        $scope.amtchange = function () {
            if ($scope.txtslot > 0) {
                if (Number($scope.foodtotlslots) < Number($scope.txtslot)) {
                    swal("Enter Total slots Must Be Below Number of Slots ", "", "warning");
                    $scope.txtslot = "";
                    $scope.tOtaL_foodamt = "";
                    $scope.chk_FoodAmt = false;
                    $scope.daydiffrence = "";
                    $scope.foodtotlslots = "";


                }
                else {
                    var fixamount = 0; var slots = 0; var total = 0; var amount = 0;
                    fixamount = $scope.vtadacM_FoodAmt;
                    slots = $scope.txtslot;
                    amount = fixamount / 3;
                    total = slots * amount;
                    if (total > 0) {
                        $scope.tOtaL_foodamt = total;
                        $scope.Math = window.Math;
                        $scope.tOtaL_foodamt = $scope.Math.round(total, 0);
                    }
                }
            }
            else {
                $scope.tOtaL_foodamt = "";
                $scope.chk_FoodAmt = false;

            }
            $scope.EmpFormula();
        };
        $scope.amtchangeACC = function () {
            if ($scope.txtslotAcc > 0) {
                if (Number($scope.Acctotolslots) < Number($scope.txtslotAcc)) {
                    swal("Enter Total slots Must Be Below Number of Slots ", "", "warning");
                    $scope.txtslotAcc = "";
                    $scope.totaL_AccmamtTwo = "";
                    $scope.chk_AccAmt = false;
                    $scope.txtslotAcc = "";
                    $scope.totaL_AccmamtTwo = "";
                    $scope.chk_AccAmt = false;
                    $scope.daydiffrence_two = "";
                    $scope.Acctotolslots = "";

                }
                else {
                    var fixaccamount = 0; var days = 0; var Total = 0;
                    if ($scope.vtadacM_AccommodationAmt > 0 && $scope.daydiffrence_two > 0) {
                        fixaccamount = $scope.vtadacM_AccommodationAmt;
                        days = $scope.txtslotAcc;
                        Total = fixaccamount * days;
                        $scope.Math = window.Math;
                        $scope.totaL_AccmamtTwo = $scope.Math.round(Total, 0);
                    }
                }
            }
            else {
                $scope.totaL_AccmamtTwo = "";
                $scope.txtslotAcc = "";
                $scope.chk_AccAmt = false;
            }
            $scope.EmpFormula();
        };

        //added By
        $scope.allmidd = function (ac) {
            angular.forEach($scope.getClientwo, function (itm) {
                itm.miiddd = ac;
            });

        };
        //AddClient
        $scope.AddClient = function () {
            $scope.txt_Address = "";
            $scope.Client_Id = "";
            var txt_Address = "";
            var txt_Client = "";
            var K = 0;
            if ($scope.getClientwo != null && $scope.getClientwo.length > 0) {
                angular.forEach($scope.getClientwo, function (itm) {
                    if (itm.miiddd == true) {
                        K = K + 1;
                        txt_Address = txt_Address + K + ")    " + itm.ismmclT_Address;
                        txt_Client = txt_Client + K + ")     " + itm.ismmclT_ClientName;
                    }

                });
            }
            $scope.txt_Address = txt_Address;
            $scope.vtadaaA_ClientMultiple = txt_Client;
            if ($scope.vtadaaA_ClientMultiple != null && $scope.vtadaaA_ClientMultiple != "") {
                $('#showclient').modal('hide');
            }
        }
    }
})();

