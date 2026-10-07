(function () {
    'use strict';
    angular
        .module('app')
        .controller('TADA_ApplyController', TADA_ApplyController)
    TADA_ApplyController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile']
    function TADA_ApplyController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile) {
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
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.obj = {};
        var date = new Date();
        $scope.searchValue = ""; $scope.vtadacM_TravelAmt = 0;
        $scope.Isdisabletxtss = true;
        $scope.get_allowence = [];
        $scope.srresult = [];
        $scope.erresult = [];
        $scope.getdata = [];
        $scope.Appriamt = false;
        $scope.vtadaA_FileName = [];
        $scope.newdate = new Date();
        $scope.ismeridian = true;
        $scope.myTabIndex = 0;
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.vtadaA_BalanceHRMS = "";
        //var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        //var daysDifference = firstDay.getDate() - $scope.newdate.getDate();       
        $scope.plMaxdate = new Date(date.getFullYear(), date.getMonth(), date.getDate() - 10);
        $scope.OldBalnceflag = false;
        //
        $scope.vtadacM_TransportAmtNew = 0;
        $scope.City_Id = "";
        $scope.searchValuetwo = "";
        $scope.vtadaaA_ClientMultiple = "";
        //console.log(noOfDays);    
        $scope.allmiddff = false; $scope.getstate = [];
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian; todate
        };
        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        };
        $scope.Tostates = function (Distance, index) {
            Distance.Tostate = []; Distance.Travel_Details = []; Distance.CityList = [];
            Distance.tostate = ""; Distance.vtadamT_Id = ""; Distance.VTADAMD_Id = ""; Distance.VTADAMC_Amount = ""; Distance.VTADAMC_AmountNew = "";
            Distance.IVRMMCT_Id = "";
            if (Distance.Fromstate > 0) {
                angular.forEach($scope.getstate, function (ob) {
                    Distance.Tostate.push(ob);
                });
            }  
              $scope.travleDetailsCalculation();
        }
        $scope.OnloadTostate = function (Distance) {
            Distance.Travel_Details = []; Distance.CityList = []; Distance.vtadamT_Id = ""; Distance.VTADAMD_Id = "";
            Distance.VTADAMC_AmountNew = ""; Distance.VTADAMC_Amount = ""; Distance.IVRMMCT_Id = "";
            $scope.VMSTADAAmountDTO = [];
            if ($scope.CityDistance != null && $scope.CityDistance.length > 0) {
                angular.forEach($scope.CityDistance, function (obc) {
                    if (obc.IVRMMCT_Id > 0) {
                        $scope.VMSTADAAmountDTO.push({
                            IVRMMCT_Id: obc.IVRMMCT_Id
                        })
                    }
                });
            }
            if (Distance.tostate > 0) {
                var data = {
                    "Fromstate": Distance.Fromstate,
                    "Tostate": Distance.tostate,
                    "IER_ID": 0,
                    "VMSTADAAmountDTO": $scope.VMSTADAAmountDTO
                };
                apiService.create("TADA_Advance_Tour/GetCity", data).then(function (promise) {
                    if (promise.travel_data != null && promise.travel_data.length > 0) {
                        Distance.CityList = promise.travel_data;
                    }
                    else {
                        swal("Amount Travel not mapped");
                    }
                });
            }

           $scope.travleDetailsCalculation();
        }
        $scope.VTADAA_Finaldocument = true; $scope.Travel_data = [];
        $scope.onpageload = function () {
            var pageid = 1;
            apiService.getURI("TADA_Apply/getalldata", pageid).
                then(function (promise) {
                    //Allowense
                    $scope.get_Report = promise.getReport;
                    $scope.Travel_data = promise.travel_data;
                    $scope.getstate = promise.state;
                    $scope.getClient = promise.client_Master;
                    $scope.getClientwo = promise.client_Master;
                    if (promise.getArray !== null && promise.getArray.length > 0) {
                        $scope.getdata = promise.getArray;
                        $scope.OldBalnceflag = true;
                    }
                    $scope.presentCountgrid = $scope.get_Report.length;
                })
        };
        $scope.CityDistance = [{ itrS_Id1: 'trans12' }];
        $scope.clientaddress = function () {
            if ($scope.getClient != null && $scope.getClient.length > 0 && $scope.Client_Id > 0) {
                for (var i = 0; i < $scope.getClient.length; i++) {
                    if ($scope.Client_Id == $scope.getClient[i].ismmclT_Id) {

                        $scope.txt_Address = $scope.getClient[i].ismmclT_Address;
                    }

                }
            }
            else {
                $scope.txt_Address = "";
            }
        };
        $scope.addrowsclient = function () {
            if ($scope.CityDistance.length > 1) {
                for (var x = 0; x === $scope.CityDistance.length; x++) {
                    var id1 = $scope.CityDistance[x].itrS_Id1;
                    var lastChar1 = id1.substr(id1.length - 1);
                    $scope.cnt1 = parseInt(lastChar1);
                }
            }
            $scope.cnt1 = $scope.cnt1 + 1;
            $scope.tet = 'trans' + $scope.cnt1;
            var newItemNo1 = $scope.cnt1;
            $scope.CityDistance.push({ 'itrS_Id1': 'trans12' + newItemNo1 });
        };
        $scope.amtchange = function () {
            if ($scope.txtslot > 0) {
                if (Number($scope.foodtotlslots) < Number($scope.txtslot)) {
                    swal("Enter Total slots Must Be Below Number of Slots ");
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
                    swal("Enter Total slots Must Be Below Number of Slots ");
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
        $scope.EmpFormula = [];
        $scope.EmpFormula = function () {
            var b = 0; var c = 0; var d = 0; var e = 0;
            if ($scope.chk_AccAmt === true && $scope.txtslotAcc > 0) {
                b += parseFloat($scope.totaL_AccmamtTwo);
            }
            else { b = 0; }
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
                var date2 = new Date($scope.obj.startdate);
                var date1 = new Date($scope.obj.enddate);
                var timeDiff = Math.abs(date1.getTime() - date2.getTime());
                var DateDifference = Math.ceil(timeDiff / (1000 * 3600 * 24) + 1);
                $scope.daydiffrence = DateDifference;
                var VTADAA_DepartureTime = $scope.vtadaA_DepartureTime;
                var VTADAA_ArrivalTime = $scope.vtadaA_ArrivalTime;
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
                //  var travelhour = (Number(hours) - Number($scope.VTADACM_TravelHour));
                if (travelhour > 0) {
                    var accdays = travelhour / 8;
                    var slots = Math.round(accdays);
                    if (slots > 0) {
                        $scope.foodtotlslots = slots;
                    }
                }
                else {
                    $scope.chk_FoodAmt = false;
                    swal("Food Amount Not Provide !");
                }
            }
            else {
                $scope.daydiffrence = "";
                $scope.tOtaL_foodamt = "";
                $scope.foodtotlslots = "";
                $scope.txtslot = "";
                $scope.EmpFormula();
            }
        };
        $scope.accMandtionamt_Chk = function () {
            if ($scope.chk_AccAmt === true) {
                var date2 = new Date($scope.obj.startdate);
                var date1 = new Date($scope.obj.enddate);
                var timeDiff = Math.abs(date1.getTime() - date2.getTime());
                var DateDifference = Math.ceil(timeDiff / (1000 * 3600 * 24) + 1);
                $scope.daydiffrence_two = DateDifference;
                var VTADAA_DepartureTime = $scope.vtadaA_DepartureTime;
                var VTADAA_ArrivalTime = $scope.vtadaA_ArrivalTime;
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
                        swal("Accommodation Amount Is Not provide U Not Stay  One Day!");
                        $scope.daydiffrence_two = "";

                    }

                }
                else {
                    $scope.chk_AccAmt = false;
                    swal("Accommodation Amount Is Not provide U Not Stay  One Day! !");
                    $scope.daydiffrence_two = "";

                }

                $scope.EmpFormula();

            }
            else {
                $scope.daydiffrence_two = "";
                $scope.totaL_AccmamtTwo = "";
                $scope.txtslotAcc = "";
                $scope.Acctotolslots = "";

            }
        };
        $scope.remove = function (index, data) {
            var newItemNo = $scope.CityDistance.length - 1;
            $scope.CityDistance.splice(index, 1);
            $scope.travleDetailsCalculation();
        };
        $scope.TransAmt_claculation = function () {
            if (Number($scope.vtadacM_TravelAmt) < (Number($scope.vtadacM_TransportAmt))) {
                $scope.vtadacM_TransportAmt = ""; $scope.totaL_transTwo = ""; //vtadacM_TravelAmt  //vtadacM_TransportAmt
                $scope.chk_TransAmt = false;
                swal("Travel Amount Not greater Than  Actual Amount  !");
            }
            else {
                $scope.totaL_transTwo = $scope.vtadacM_TransportAmt;                           
            }
            $scope.EmpFormula();
        }
        $scope.TransAmt_chk = function () {
            
            if ($scope.chk_TransAmt === true) {
                if (Number($scope.vtadacM_TravelAmt) < (Number($scope.vtadacM_TransportAmt))) {
                    $scope.vtadacM_TransportAmt = 0;
                    $scope.chk_TransAmt = false;
                    swal("Travel Amount Not greater Than  Actual Amount  !");
                }
                else {
                    $scope.vtadacM_TransportAmt = "";
                    $scope.totaL_transTwo = "";
                    $scope.EmpFormula();
                }
                
            }
            else {
                $scope.vtadacM_TransportAmt = "";
                $scope.daydiffrence_three = "";
                $scope.totaL_transTwo = "";
                $scope.EmpFormula();
            }
        };
        $scope.OtheraAmt_chk = function () {
            if ($scope.OtherTotal_amt != null) {
                if ($scope.chk_OthersAmt === true) {
                    //txt_approxTotAmount
                    var date2 = new Date($scope.obj.startdate);
                    var date1 = new Date($scope.obj.enddate);
                    var timeDiff = Math.abs(date1.getTime() - date2.getTime());
                    var DateDifference = Math.ceil(timeDiff / (1000 * 3600 * 24) + 1);
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
        $scope.previewimg_new = function (img) {
            $scope.ismclT_FilePath = img;
            var img = $scope.ismclT_FilePath;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.ismclT_FilePath);
                    $('#myimagePreview').modal('show');
                }
                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                    $window.open($scope.ismclT_FilePath);
                }
                else if (lastelement == 'pdf') {
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.ismclT_FilePath;
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
                $window.open($scope.ismclT_FilePath)
            }
        };
        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input, document) {
            $scope.SelectedFileForUploadzd = input.files;
            $scope.vtadaA_FileName = input.files[0].name;
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
            console.log("TADA  Upload  :" + data);
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
                    data.ismclT_FilePath = d;
                    data.vtadaaF_FileName = $scope.vtadaA_FileName;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        };
        $scope.materaldocuupload = [{ itrS_Id: 'trans1' }];
        if ($scope.materaldocuupload.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addgrnrows = function () {
            if ($scope.materaldocuupload.length > 1) {
                for (var i = 0; i === $scope.materaldocuupload.length; i++) {
                    var id = $scope.materaldocuupload[i].itrS_Id;
                    var lastChar = id.substr(id.length - 1);
                    $scope.cnt = parseInt(lastChar);
                }
            }
            $scope.cnt = $scope.cnt + 1;
            $scope.tet = 'trans' + $scope.cnt;
            var newItemNo = $scope.cnt;
            $scope.materaldocuupload.push({ 'itrS_Id': 'trans' + newItemNo });


        };
        //add multiple client names
        $scope.clientnames_rem = [{ itrS_Id1: 'trans12' }];
        if ($scope.clientnames_rem.length === 1) {
            $scope.cnt1 = 1;
        }
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
        //addgrnrowsUpdate
        $scope.addgrnrowsUpdate = function () {
            if ($scope.materaldocuuploadtwo.length > 1) {
                for (var i = 0; i === $scope.materaldocuuploadtwo.length; i++) {
                    var id = $scope.materaldocuuploadtwo[i].itrS_Id;
                    var lastChar = id.substr(id.length - 1);
                    $scope.cnt = parseInt(lastChar);
                }
            }
            $scope.cnt = $scope.cnt + 1;
            $scope.tet = 'trans' + $scope.cnt;
            var newItemNo = $scope.cnt;
            $scope.materaldocuuploadtwo.push({ 'itrS_Id': 'trans' + newItemNo });


        };
        $scope.removegrfiles = function (index, data) {
            var newItemNo = $scope.materaldocuuploadtwo.length - 1;
            $scope.materaldocuuploadtwo.splice(index, 1);

        };
        //FileUpload
        $scope.removegrnrows = function (index, data) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

        };
        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.selecteddetails = []; $scope.savestateamount = [];
            $scope.uploadarray = [];
            $scope.client_names = [];
            var totalamount = 0;
            var VTADAA_Id = 0;
            var VTADAAA_Id = 0;
            if ($scope.VTADAA_Id != null && $scope.VTADAA_Id > 0) {
                VTADAA_Id = $scope.VTADAA_Id;
            }
            if ($scope.VTADAAA_Id != null && $scope.VTADAAA_Id > 0) {
                VTADAAA_Id = $scope.VTADAAA_Id;
            }
            var VTADAA_DepartureTime = $scope.vtadaA_DepartureTime;
            var VTADAA_ArrivalTime = $scope.vtadaA_ArrivalTime;
            $scope.sresult = $filter('date')(VTADAA_DepartureTime, 'HH:mm');
            $scope.eresult = $filter('date')(VTADAA_ArrivalTime, 'HH:mm');
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.CityDistance!=null && $scope.CityDistance.length > 0) {
                    angular.forEach($scope.CityDistance, function (Distance) {
                        if (Distance.VTADAMC_Amount > 0) {
                            $scope.savestateamount.push({
                                VTADAAAC_FromState: Distance.Fromstate,
                                VTADAAAC_ToState: Distance.tostate,
                                VTADAMT_Id: Distance.vtadamT_Id,
                                VTADAMD_Id: Distance.VTADAMD_Id,
                                VTADAAAC_Travel: Distance.vtadamT_Id,
                                VTADAAAC_Amount: Distance.VTADAMC_Amount,
                                IVRMMCT_Id: Distance.IVRMMCT_Id
                            });
                        }
                    });
                }

                if ($scope.txt_amt != null && $scope.txt_amt != "") {
                    totalamount = $scope.txt_amt;
                }
                if ($scope.txt_approxTotAmount != null && $scope.txt_approxTotAmount != "") {
                    totalamount = $scope.txt_approxTotAmount;
                }
                if ($scope.materaldocuupload.length > 0) {
                    angular.forEach($scope.materaldocuupload, function (itm) {
                        if (itm.ismclT_FilePath !== undefined && itm.ismclT_FilePath !== null && itm.ismclT_FilePath !== "") {
                            $scope.uploadarray.push({
                                VTADAAF_FileName: itm.vtadaaF_FileName,
                                VTADAAF_FilePath: itm.ismclT_FilePath,
                                VTADAAF_Remarks: itm.VTADAAF_Remarks
                            });
                        }
                    });
                }

                if ($scope.clientnames_rem.length > 0) {
                    angular.forEach($scope.clientnames_rem, function (itmx) {
                        if (true) {
                            $scope.client_names.push({
                                ISMMCLT_Id: itmx.Client_Id,
                                VTADAMCF_Remarks: itmx.VTADAMCF_Remarks
                            });
                        }
                    });
                }
                if (totalamount > 0) {
                    if ($scope.uploadarray.length != null && $scope.uploadarray.length > 0) {
                        if ($scope.chk_FoodAmt === true) {
                            $scope.foodamount_id = $scope.foodamount_id === undefined || $scope.foodamount_id === null ||
                                $scope.foodamount_id === "" ? 0 : $scope.foodamount_id;
                            $scope.selecteddetails.push({
                                Type: 'Food', Amount: $scope.tOtaL_foodamt,
                                Remarks: $scope.Food_Remarks, VTADAAD_Id: $scope.foodamount_id,
                                VTADAAD_Totalslots: $scope.foodtotlslots, VTADAAD_Slots: $scope.txtslot
                            });
                        }
                        if ($scope.chk_AccAmt === true) {
                            $scope.accamount_id = $scope.accamount_id === undefined || $scope.accamount_id === null ||
                                $scope.accamount_id === "" ? 0 : $scope.accamount_id;
                            $scope.selecteddetails.push({
                                Type: 'Accommodation', Amount: $scope.totaL_AccmamtTwo,
                                Remarks: $scope.Acc_Remarks, VTADAAD_Id: $scope.accamount_id,
                                VTADAAD_Totalslots: $scope.Acctotolslots, VTADAAD_Slots: $scope.txtslotAcc
                            });
                        }
                        if ($scope.chk_TransAmt === true) {
                            $scope.transamount_id = $scope.transamount_id === undefined || $scope.transamount_id === null ||
                                $scope.transamount_id === "" ? 0 : $scope.transamount_id;
                            $scope.selecteddetails.push({
                                Type: 'Travel', Amount: $scope.totaL_transTwo,
                                Remarks: $scope.transport_Remarks, VTADAAD_Id: $scope.transamount_id
                            });
                        }
                        if ($scope.chk_OthersAmt === true) {
                            $scope.othersamount_id = $scope.othersamount_id === undefined || $scope.othersamount_id === null ||
                                $scope.othersamount_id === "" ? 0 : $scope.othersamount_id;
                            $scope.selecteddetails.push({
                                Type: 'Other', Amount: $scope.totaL_otherTwo,
                                Remarks: $scope.other_Remarks, VTADAAD_Id: $scope.othersamount_id
                            });
                        }
                        if ($scope.vtadaA_BalanceHRMS > 0) {
                            $scope.txt_amt = 0;
                            totalamount = 0;
                        }
                        else {
                            $scope.vtadaA_BalanceHRMS = "";
                        }
                        var data = {
                            "IVRMMCT_Id": $scope.City_Id,
                            "VTADAA_FromDate": new Date($scope.obj.startdate).toDateString(),
                            "VTADAA_ToDate": new Date($scope.obj.enddate).toDateString(),
                            "VTADAA_ClientId": 0,
                            "VTADAA_TotalAppliedAmount": totalamount,
                            "VTADAA_ToAddress": $scope.txt_Address,
                            "VTADAA_Remarks": $scope.txt_Remarks,
                            "AllowenceArray": $scope.selecteddetails,
                            "VTADAA_Id": VTADAA_Id,
                            "VTADAAA_Id": VTADAAA_Id,
                            filelist: $scope.uploadarray,
                            "VTADAA_DepartureTime": $scope.sresult,
                            "VTADAA_ArrivalTime": $scope.eresult,
                            //"vtadaaA_ClientMultiple": $scope.vtadaaA_ClientMultiple,
                            "VTADAA_BalanceHRMS": $scope.vtadaA_BalanceHRMS,
                            "VTADAA_Finaldocument": $scope.VTADAA_Finaldocument,
                            multiple_client: $scope.client_names,
                            "VMSTADAAmountDTO": $scope.savestateamount
                        };
                        apiService.create("TADA_Apply/savedta", data).then(function (promise) {
                            if (promise.returnvalue === true) {
                                if (promise.returnval === "Insert") {
                                    swal("Record Saved Successfully", "", "success");
                                    $state.reload();
                                }
                                if (promise.returnval === "Failed") {
                                    swal("Record Not saved", "", "error");
                                }
                                if (promise.returnval === "Duplicate") {
                                    swal("Record Already Exist", "", "info");
                                }
                                if (promise.returnval === "Update") {
                                    swal("Record Update Successfully", "", "success");
                                    $state.reload();
                                }
                                if (promise.returnval === "UpdateFailed") {
                                    swal("Record Not Update");
                                }
                                if (promise.returnval === "") {
                                    swal("please contact administrator !", "", "error");
                                    $state.reload();
                                }
                                if (promise.returnval === "RecordExit") {
                                    swal(" U Already Adavance Applied  , Select Correct Between Dates !");
                                    $state.reload();
                                }
                                if (promise.returnval === "DuplicateAdavance") {
                                    swal(" U Already Adavance Applied  , Select Correct Between Dates Result Is Pending !");
                                    $state.reload();
                                }

                            }
                            if (promise.returnvalue === false) {
                                if (promise.returnval === "") {
                                    swal("Please contact Administrator !");
                                    $state.reload();
                                }
                            }
                        });


                    }
                    else {
                        swal('Bill File  Not Attached');
                    }
                }
                else {
                    swal("Please Fill  Total Amount")
                }

            }
        };
        $scope.getView = function (usertwo) {
            var VTADAAA_Id = usertwo.vtadaaA_Id;
            if (VTADAAA_Id > 0) {
                var data = {
                    "VTADAAA_Id": VTADAAA_Id
                };
                apiService.create("TADA_Apply/Viewdetails", data).then(function (promise) {
                    $scope.ViewDetails = promise.viewdetails

                    if ($scope.ViewDetails != null && $scope.ViewDetails.length > 0) {
                        if ($scope.ViewDetails[0].VTADAAAD_ExpenditureHead != null && $scope.ViewDetails[0].VTADAAAD_ExpenditureHead != "") {
                            $scope.flagone = true;
                            $scope.flagtwo = false;
                        }
                        else {
                            $scope.flagtwo = true;
                            $scope.flagone = false
                        }
                        $scope.Getsancationed_List = [];
                        angular.forEach($scope.ViewDetails, function (dd) {
                            if ($scope.Getsancationed_List.length === 0) {
                                $scope.Getsancationed_List.push({
                                    VTADAAAA_SanctioningLevel: dd.VTADAAAA_SanctioningLevel,
                                    ApprovedBy: dd.ApprovedBy
                                });
                            } else if ($scope.Getsancationed_List.length > 0) {
                                var count = 0;

                                angular.forEach($scope.Getsancationed_List, function (d) {
                                    if (d.VTADAAAA_SanctioningLevel === dd.VTADAAAA_SanctioningLevel) {
                                        count += 1;
                                    }
                                });
                                if (count === 0) {
                                    $scope.Getsancationed_List.push({
                                        VTADAAAA_SanctioningLevel: dd.VTADAAAA_SanctioningLevel,
                                        ApprovedBy: dd.ApprovedBy
                                    });
                                }
                            }
                        });
                        $scope.getapproveddetails = [];
                        angular.forEach($scope.Getsancationed_List, function (dd) {
                            $scope.getapproveddetails = [];
                            angular.forEach($scope.ViewDetails, function (d) {
                                if (dd.VTADAAAA_SanctioningLevel === d.VTADAAAA_SanctioningLevel) {
                                    $scope.getapproveddetails.push(d);
                                }
                            });
                            dd.reportdetails = $scope.getapproveddetails;
                        });
                        $('#viewDeatils').modal('show');
                        $scope.plMaxdate = new Date(date.getFullYear(), date.getMonth(), date.getDate() - 365);

                    }
                    else {
                        swal("TADA Authrization Request Is Not  Approval");
                    }

                });
            }
        }
        $scope.Apprimate = function () {
            if ($scope.amountchanged === true) {
                if ($scope.txt_ApproxAmt !== null && $scope.BalanceAmount !== null && $scope.txt_ApproxAmt !== "" && $scope.BalanceAmount !== "") {
                    var a = 0;
                    a += parseFloat(Number($scope.txt_ApproxAmt) + Number($scope.BalanceAmount));
                    $scope.txt_amt = a;
                }
                else {
                    $scope.txt_amt = $scope.BalanceAmount;
                }
            }
            if ($scope.amountnegative === true) {
                if ($scope.txt_ApproxAmt !== null && $scope.BalanceAmount !== null && $scope.txt_ApproxAmt !== "" && $scope.BalanceAmount !== "") {
                    var a = 0;
                    a += parseFloat(Number($scope.BalanceAmount) - Number($scope.txt_ApproxAmt));
                    if (a > 0) {
                        $scope.txt_amt = a;
                    }
                    else {
                        $scope.txt_amt = $scope.BalanceAmount;
                        swal("Amount Not greater than Balanace Amount ! ");
                        $scope.txt_ApproxAmt = "";
                    }

                }
                else {
                    $scope.txt_amt = $scope.BalanceAmount;
                }
            };

        };
        $scope.Adavance_Chk = function () {
            if ($scope.amountchanged === true) {
                $scope.amountnegative = false;
            }
            else {
                $scope.amountnegative = true;
            }
            $scope.txt_ApproxAmt = "";
            $scope.txt_amt = $scope.BalanceAmount;
        };
        $scope.Adavance_ChkKk = function () {
            if ($scope.amountnegative === true) {
                $scope.amountchanged = false;
            }
            else {
                $scope.amountchanged = true;
            }
            $scope.txt_ApproxAmt = "";
            $scope.txt_amt = $scope.BalanceAmount;
        };
        $scope.select = function (usertwo) {
            $scope.getCity = $scope.getdata;
            $scope.Appriamt = true;

            $scope.obj.startdate = new Date(usertwo.vtadaaA_FromDate);
            $scope.obj.enddate = new Date(usertwo.vtadaaA_ToDate);
            $scope.vtadaA_DepartureTime = moment(usertwo.vtadaaA_DepartureTime, 'HH:mm a').format();
            $scope.vtadaA_ArrivalTime = moment(usertwo.vtadaaA_ArrivalTime, 'HH:mm a').format();
            $scope.IVRMMS_Id = usertwo.ivrmmS_Id;
            $scope.City_Id = usertwo.ivrmmcT_Id;
            $scope.Client_Id = usertwo.vtadaaA_ClientId;
            $scope.VTADAAA_Id = usertwo.vtadaaA_Id;
            $scope.txt_Remarks = usertwo.vtadaaA_Remarks;
            $scope.txt_Address = usertwo.vtadaaA_ToAddress;
            $scope.Total_ApplAmt = usertwo.vtadaaA_TotalAppliedAmount;
            $scope.Total_ApplAmthidee = usertwo.vtadaaA_TotalAppliedAmount;
            $scope.Total_PaidAmt = usertwo.vtadaaA_TotalPaidAmount;
            $scope.txt_paidAmt = usertwo.vtadaaA_TotalPaidAmount;
            $scope.amountchanged = false;
            $scope.amountnegative = false;
            $scope.ivrmmcT_Name = usertwo.ivrmmcT_Name;
            $scope.vtadaaA_ClientMultiple = usertwo.vtadaaA_ClientMultiple;
            $scope.BalanceAmount = (Number($scope.Total_ApplAmt) - Number($scope.Total_PaidAmt));
            if ($scope.BalanceAmount > 0) {
                $scope.txt_amt = $scope.BalanceAmount;
            }
            else {
                $scope.BalanceAmount = $scope.txt_amt = 0;
            }
            $scope.myTabIndex = $scope.myTabIndex - 1;
            $scope.OldBalnceflag = false;
            var data = {
                "VTADAA_FromDate": new Date($scope.obj.startdate).toDateString(),
                "VTADAA_ToDate": new Date($scope.obj.enddate).toDateString(),
            };
            apiService.create("TADA_Apply/TadaAllwence", data).then(function (promise) {
                $scope.GetArray = [];
                if (promise.getArray != null && promise.getArray.length > 0) {

                }
                else {
                    $state.reload();
                    swal("Daily Report Not Generate Between Dates  !");
                }



            });
            $scope.scroll();
        }
        $scope.FromDates = function () {
            $scope.obj.enddate = "";
        }
        $scope.edit = function (task) {
            $scope.get_allowence = [];
            $scope.getdata = [];
            $scope.VTADAA_Finaldocumentdoc = true;
            $scope.VTADAA_Id = task.vtadaA_Id;

            var data = {
                "VTADAA_Id": $scope.VTADAA_Id
            };
            apiService.create("TADA_Apply/edit", data).then(function (promise) {
                if (promise.editArray !== null && promise.editArray.length > 0) {
                    $scope.Appriamt = false;
                    $scope.getCity = promise.editArray;
                    $scope.IVRMMS_Id = promise.editArray[0].ivrmmS_Id;
                    $scope.Client_Id = promise.editArray[0].vtadaA_ClientId;
                    $scope.obj.startdate = new Date(promise.editArray[0].vtadaA_FromDate);
                    $scope.obj.enddate = new Date(promise.editArray[0].vtadaA_ToDate);
                    $scope.City_Id = promise.editArray[0].ivrmmcT_Id;
                    $scope.txt_amt = promise.editArray[0].vtadaA_TotalAppliedAmount;
                    $scope.txt_Address = promise.editArray[0].vtadaA_ToAddress;
                    $scope.txt_Remarks = promise.editArray[0].vtadaA_Remarks;
                    $scope.VTADAAA_Id = promise.editArray[0].vtadaaA_Id;

                    $scope.vtadaaA_ClientMultiple = promise.editArray[0].vtadaaA_ClientMultiple;
                    $scope.vtadaA_DepartureTime = moment(promise.editArray[0].vtadaA_DepartureTime, 'HH:mm a').format();
                    $scope.vtadaA_ArrivalTime = moment(promise.editArray[0].vtadaA_ArrivalTime, 'HH:mm a').format();
                    $scope.amountchanged = false;
                    $scope.amountnegative = false;


                    if (promise.editArrayTwo !== null && promise.editArrayTwo.length > 0) {
                        $scope.txt_approxTotAmount = promise.editArray[0].vtadaA_TotalAppliedAmount;
                        var date2 = new Date($scope.obj.startdate);
                        var date1 = new Date($scope.obj.enddate);
                        var timeDiff = Math.abs(date1.getTime() - date2.getTime());
                        var DateDifference = Math.ceil(timeDiff / (1000 * 3600 * 24) + 1);
                        for (var i = 0; i < promise.editArrayTwo.length; i++) {
                            if (promise.editArrayTwo[i].vtadaaD_ExpenditureHead == "Food") {
                                $scope.tOtaL_foodamt = promise.editArrayTwo[i].vtadaaD_Amount;
                                $scope.Food_Remarks = promise.editArrayTwo[i].vtadaaD_Remarks;
                                $scope.chk_FoodAmt = true;
                                $scope.foodamount_id = promise.editArrayTwo[i].vtadaaD_Id;
                                $scope.daydiffrence = DateDifference;
                                $scope.foodtotlslots = promise.editArrayTwo[i].vtadaaD_Totalslots;
                                $scope.txtslot = promise.editArrayTwo[i].vtadaaD_Slots;
                            }
                            if (promise.editArrayTwo[i].vtadaaD_ExpenditureHead == "Accommodation") {
                                $scope.totaL_AccmamtTwo = promise.editArrayTwo[i].vtadaaD_Amount;
                                $scope.Acc_Remarks = promise.editArrayTwo[i].vtadaaD_Remarks;
                                $scope.chk_AccAmt = true;
                                $scope.accamount_id = promise.editArrayTwo[i].vtadaaD_Id;
                                $scope.daydiffrence_two = DateDifference;
                                $scope.Acctotolslots = promise.editArrayTwo[i].vtadaaD_Totalslots;
                                $scope.txtslotAcc = promise.editArrayTwo[i].vtadaaD_Slots;

                            }
                            if (promise.editArrayTwo[i].vtadaaD_ExpenditureHead == "Travel") {
                                $scope.totaL_transTwo = promise.editArrayTwo[i].vtadaaD_Amount;
                                $scope.transport_Remarks = promise.editArrayTwo[i].vtadaaD_Remarks;
                                $scope.chk_TransAmt = true;
                                $scope.transamount_id = promise.editArrayTwo[i].vtadaaD_Id;
                                // $scope.daydiffrence_three = DateDifference;
                            }
                            if (promise.editArrayTwo[i].vtadaaD_ExpenditureHead == "Other") {
                                $scope.totaL_otherTwo = promise.editArrayTwo[i].vtadaaD_Amount;
                                $scope.OtherTotal_amt = promise.editArrayTwo[i].vtadaaD_Amount;
                                $scope.other_Remarks = promise.editArrayTwo[i].vtadaaD_Remarks;
                                $scope.chk_OthersAmt = true;
                                $scope.othersamount_id = promise.editArrayTwo[i].vtadaaD_Id;
                                $scope.daydiffrence_Four = DateDifference;
                                $scope.OtheramtDisable = true;
                            }
                            //added by adarsh
                            if (promise.tada_multipleclient !== null && promise.tada_multipleclient.length > 0) {
                                $scope.clientnames_rem = promise.tada_multipleclient;
                                angular.forEach($scope.clientnames_rem, function (dd) {
                                    dd.VTADAMCF_Remarks = dd.vtadamcF_Remarks;
                                    dd.Client_Id = dd.ismmclT_Id;
                                });

                            }
                            $scope.txt_amt = "";
                        }
                        $scope.EmpFormula();

                    }
                    if (promise.editArryThree != null && promise.editArryThree.length > 0) {
                        $scope.materaldocuupload = promise.editArryThree;
                        angular.forEach($scope.materaldocuupload, function (dd) {
                            dd.VTADAAF_Remarks = dd.vtadaaF_Remarks;
                            dd.ismclT_FilePath = dd.vtadaaF_FilePath;
                            dd.ismclT_FileName = dd.vtadaaF_FileName;
                        });
                    }
                    if (promise.editFour != null && promise.editFour.length > 0) {
                        $scope.Total_ApplAmt = promise.editFour[0].vtadaaA_TotalAppliedAmount;
                        $scope.Total_ApplAmthideen = promise.editFour[0].vtadaaA_TotalAppliedAmount;
                        $scope.txt_paidAmt = promise.editFour[0].vtadaaA_TotalPaidAmount;
                        $scope.Appriamt = true;
                        $scope.txt_approxTotAmount = "";
                        $scope.get_allowence = [];
                        $scope.BalanceAmount = (Number($scope.Total_ApplAmt) - Number($scope.txt_paidAmt));
                        $scope.txt_ApproxAmt = (Number($scope.txt_amt) - Number($scope.BalanceAmount));
                        if ($scope.txt_ApproxAmt > 0) {

                        }
                        else {
                            swal('Balance Amount Is Minus values ', "" + $scope.txt_ApproxAmt + "");

                            $scope.txt_ApproxAmt = "";
                        }

                    }
                    else {

                        $scope.get_allowence = promise.editFive;
                        if ($scope.get_allowence != null && $scope.get_allowence.length > 0) {
                            $scope.vtadacM_FoodAmt = $scope.get_allowence[0].vtadacM_FoodAmt;
                            $scope.vtadacM_AccommodationAmt = $scope.get_allowence[0].vtadacM_AccommodationAmt;
                           // $scope.vtadacM_TransportAmt = $scope.get_allowence[0].vtadacM_TransportAmt;
                            //  $scope.VTADACM_TravelHour = $scope.get_allowence[0].vtadacM_TravelHour; 
                        }

                    }
                    $scope.VTADAA_Finaldocumentdoc = promise.editArray[0].vtadaA_Finaldocument;
                    $scope.vtadaA_BalanceHRMS = promise.editArray[0].vtadaA_BalanceHRMS;
                    $scope.VTADAA_Finaldocument = promise.editArray[0].vtadaA_Finaldocument;
                }
                else {
                    if (promise.returnvalue === true) {
                        if (promise.returnval === "Firststage") {
                            swal("Result Is Wait Authrization ! ");
                            $scope.VTADAAA_Id = "";
                            $state.reload();
                        }
                    }
                }
                $scope.scroll();
            });
        };
        $scope.Deletedata = function (item, SweetAlert) {
            $scope.VTADAA_Id = item.vtadaA_Id;
            var data = {
                "VTADAA_Id": $scope.VTADAA_Id

            }
            var dystring = "";
            if (item.vtadaA_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.vtadaA_ActiveFlg == false) {
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
                        apiService.create("TADA_Apply/deactive", data).
                            then(function (promise) {
                                if (promise.returnvalue === true) {

                                    if (promise.returnval === "Delete") {
                                        swal("Record " + dystring + "d Successfully!!!");
                                    }
                                    if (promise.returnval === "NotDelete") {
                                        swal("Record " + dystring + "d Successfully!!!");
                                    }
                                    if (promise.returnval === "Firststage") {
                                        swal("Result Is Wait Authrization ! ");
                                    }
                                }
                                else {
                                    swal("Please Contact Administrator!!!");
                                }


                                $state.reload();
                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                    $state.reload();
                });
        }
        $scope.onchangecitylist = function (Distance) {

            Distance.Travel_Details = []; Distance.vtadamT_Id = ""; Distance.VTADAMD_Id = ""; Distance.VTADAMC_Amount = ""; Distance.VTADAMC_AmountNew = "";
            $scope.IVRMMS_Id = ""; 
            if ($scope.CityDistance.length > 1) {
                $scope.IVRMMS_Id = Distance.Fromstate;
              
            }
            else {
                $scope.City_Id = "";
                $scope.IVRMMS_Id = Distance.tostate;
                $scope.City_Id = Distance.IVRMMCT_Id;
                $scope.get_allowence = []; 
                $scope.Dropdown_cityTWO();
            }           
            
           $scope.travleDetailsCalculation();
        }
        $scope.Getdatas = function () {
            if ($scope.obj.startdate != null && $scope.obj.enddate != null && $scope.obj.startdate != "" && $scope.obj.enddate != "") {

                var data = {
                    "VTADAA_FromDate": new Date($scope.obj.startdate).toDateString(),
                    "VTADAA_ToDate": new Date($scope.obj.enddate).toDateString(),
                };
                apiService.create("TADA_Apply/TadaAllwence", data).then(function (promise) {
                    $scope.GetArray = [];
                    if (promise.getArray != null && promise.getArray.length > 0) {
                        $scope.chk_FoodAmt = false;
                        $scope.chk_AccAmt = false;
                        $scope.chk_TransAmt = false;
                        $scope.chk_OthersAmt = false;
                        $scope.Foodamt_Chk();
                        $scope.accMandtionamt_Chk();
                        $scope.TransAmt_chk();
                        $scope.OtheraAmt_chk();
                        $scope.txt_amt = "";
                    }
                    //else {
                    //    $scope.obj.startdate = "";
                    //    $scope.obj.enddate = "";
                    //    swal("Daily Report Not Generate Between Dates  !");
                    //}



                });
            }
            else {
                swal("Please Select  Dates");
                $scope.obj.startdate = "";
                $scope.obj.enddate = "";
            }
        };
        $scope.changeTravelDetails = function (Distance) {
            Distance.Travel_Details = []; Distance.VTADAMD_Id = []; Distance.VTADAMC_Amount = ""; Distance.VTADAMC_AmountNew = "";
            if (Distance.vtadamT_Id > 0) {
                var data = {
                    "VTADAMT_Id": Distance.vtadamT_Id
                };
                apiService.create("TADA_Advance_Tour/TravelDetails", data).then(function (promise) {
                    if (promise.travel_Details != null && promise.travel_Details.length > 0) {
                        Distance.Travel_Details = promise.travel_Details;
                    }
                    $scope.travleDetailsCalculation();
                });
            }

        };
        $scope.Clearid = function () {

            $state.reload();
        };
        $scope.travleDetailsCalculation = function () {
            $scope.vtadacM_TravelAmt = 0; var vtadacM_TravelAmt = 0; $scope.chk_TransAmt = false;
            angular.forEach($scope.CityDistance, function (ob) {
                if (ob.VTADAMC_Amount > 0) {
                    vtadacM_TravelAmt = (Number(vtadacM_TravelAmt) + (Number(ob.VTADAMC_Amount)));
                }
            });
            $scope.vtadacM_TravelAmt = vtadacM_TravelAmt;
            $scope.EmpFormula();
        }
        $scope.Getamount = function (Distance) {
            var VRMMDT_Id = 0;
            Distance.VTADAMC_AmountNew = ""; Distance.VTADAMC_Amount = "";

            angular.forEach(Distance.CityList, function (ob) {
                if (ob.IVRMMCT_Id == Distance.IVRMMCT_Id) {
                    VRMMDT_Id = ob.VRMMDT_Id;
                }
            });
            var data = {
                "Fromstate": Distance.Fromstate,
                "Tostate": Distance.tostate,
                "VTADAMT_Id": Distance.vtadamT_Id,
                "VTADAMD_Id": Distance.VTADAMD_Id,
                "VRMMDT_Id": VRMMDT_Id,
            };
            apiService.create("TADA_Advance_Tour/Getamount", data).then(function (promise) {
                if (promise.travelAmount != null && promise.travelAmount.length > 0) {
                    Distance.VTADAMC_AmountNew = promise.travelAmount[0].VTADAMC_Amount;
                    Distance.VTADAMC_Amount = promise.travelAmount[0].VTADAMC_Amount;
                }
                else {
                    swal("Amount Travel not mapped");

                }
                $scope.travleDetailsCalculation();
              
            });
        };
        $scope.travelAmountDetails = function (Distance) {
            if (Distance.VTADAMC_Amount > 0) {
                if (Distance.VTADAMC_AmountNew < Distance.VTADAMC_Amount) {
                    swal("Total Amount Not Greater Than Travel Amount !");
                    Distance.VTADAMC_Amount = "";

                }
            }
           $scope.travleDetailsCalculation();
        }
        $scope.Dropdown_cityTWO = function () {
            $scope.get_allowence = [];
            if ($scope.obj.startdate != null && $scope.obj.enddate != null && $scope.obj.startdate != "" && $scope.obj.enddate != "") {
                if ($scope.VTADAA_Id > 0) {

                }
                else {
                    $scope.chk_FoodAmt = false; $scope.chk_AccAmt = false; $scope.chk_TransAmt = false; $scope.chk_OthersAmt = false; $scope.txt_approxTotAmount = "";
                    $scope.OtherTotal_amt = ""; $scope.daydiffrence_three = "";
                    $scope.totaL_transTwo = ""; $scope.tOtaL_foodamt = "";
                    $scope.daydiffrence = ""; $scope.daydiffrence_two = "";
                    $scope.totaL_otherTwo = ""; $scope.totaL_AccmamtTwo = "";
                    $scope.txtslotAcc = ""; $scope.foodtotlslots = "";
                }
                if ($scope.City_Id > 0) {
                    var data = {
                        "IVRMMCT_Id": $scope.City_Id
                    };
                    apiService.create("TADA_Apply/TadaAllwence", data).then(function (promise) {
                       
                        if (promise.allowense != null) {
                            $scope.get_allowence = promise.allowense;
                            if ($scope.get_allowence !== null && $scope.get_allowence.length > 0) {
                                $scope.vtadacM_FoodAmt = $scope.get_allowence[0].vtadacM_FoodAmt;
                                $scope.vtadacM_AccommodationAmt = $scope.get_allowence[0].vtadacM_AccommodationAmt;
                              //  $scope.vtadacM_TransportAmt = $scope.get_allowence[0].vtadacM_TransportAmt;
                                // $scope.VTADACM_TravelHour = $scope.get_allowence[0].vtadacM_TravelHour;
                            }
                            else {
                                swal("For Your Designation  City TA DA Amount Is Not Mapped", "Contact Admin");

                            }
                        }
                        else {
                            swal("For Your Designation  City TA DA Amount Is Not Mapped", "Contact Admin");
                        }


                    });
                }
                else {

                    swal("Plaese Select City");
                    $scope.txt_amt = '';

                }
            }
            else {
                swal("Please Select Dates");
            }
            $scope.EmpFormula();
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //dropdown

        //extara adeed
        $scope.Dropdown_State = function () {
            $scope.get_allowence = [];
            $scope.getCity = [];
            if ($scope.obj.startdate != null && $scope.obj.enddate != null && $scope.obj.startdate != "" && $scope.obj.enddate != "") {
                if ($scope.IVRMMS_Id != null && $scope.IVRMMS_Id != "" && $scope.IVRMMS_Id > 0) {
                    var data = {
                        "IVRMMS_Id": $scope.IVRMMS_Id
                    };
                    apiService.create("TADA_Apply/getstate", data).then(function (promise) {

                        if (promise.city_Type != null && promise.city_Type.length > 0) {
                            $scope.getCity = promise.city_Type;
                        }
                        else {
                            swal("For Your Designation  City  Not Mapped !");

                        }
                    });
                }
                else {
                    swal("Please Select State");
                }
            }
            else {
                swal("Please Select Dates");
            }

        };

        $scope.validateTomintime = function (timedata) {
            $scope.hrelapD_OutTime = "";
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;
            $scope.min.setHours(hh);
            $scope.min.setMinutes(mm);
            $scope.minlnc = timedata;
            $scope.minlnc.setHours(hh);
            $scope.minlnc.setMinutes(mm);
            //$scope.hrelapD_InTime = "";
        };
        $scope.MultiClient = function () {
            $('#showclient').modal('show');
        }
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
        //RemainingPaid
        $scope.RemainingPaid = function () {
            if ($scope.vtadaA_BalanceHRMS <= $scope.txt_paidAmt) {


            }
            else {
                swal("Paid Amount  Not Greater Than Remaining Paid Amount !");
                $scope.vtadaA_BalanceHRMS = "";

                return;
            }

        }
        //UpdateDocument

        $scope.materaldocuuploadtwo = [];
        $scope.materaldocuuploadtwo = [{ id: 'materal' }];
        $scope.UpdateDocument = function (user) {
            $scope.temptada = [];
            $scope.emplYoeeName = "";
            $scope.emplYoeeName = user.emplYoeeName;
            $scope.materaldocuuploadtwo = [];
            $scope.materaldocuuploadtwo = [{ id: 'materal' }];
            $scope.VTADAA_IdNew = "";
            $scope.VTADAA_IdNew = user.vtadaA_Id;
            $scope.VTADAA_Finaldocumentdoc = true;
            var data = {
                "VTADAA_Id": user.vtadaA_Id
            };
            apiService.create("TADA_Apply/edit", data).then(function (promise) {
                if (promise.uplodaArray != null && promise.uplodaArray.length > 0) {
                    $scope.materaldocuuploadtwo = promise.uplodaArray;
                    angular.forEach($scope.materaldocuuploadtwo, function (dd) {
                        dd.VTADAAF_Remarks = dd.vtadaaF_Remarks;
                        dd.ismclT_FilePath = dd.vtadaaF_FilePath;
                        dd.ismclT_FileName = dd.vtadaaF_FileName;
                    });
                    $scope.temptada.push(user);
                    $('#FileviewDeatils').modal('show');
                }
                else {
                    swal("files  Are Not Found !");
                }
            });
        }
        //FileUpload
        $scope.FileUploadTada = function () {
            $scope.uploadarray = [];
            if ($scope.materaldocuuploadtwo.length > 0) {
                angular.forEach($scope.materaldocuuploadtwo, function (itm) {
                    if (itm.ismclT_FilePath !== undefined && itm.ismclT_FilePath !== null && itm.ismclT_FilePath !== "") {
                        $scope.uploadarray.push({
                            VTADAAF_FileName: itm.vtadaaF_FileName,
                            VTADAAF_FilePath: itm.ismclT_FilePath,
                            VTADAAF_Remarks: itm.VTADAAF_Remarks,
                            VTADAAF_VerfieRemarks: itm.vtadaaF_VerfieRemarks
                        });
                    }
                });
            }
            if ($scope.uploadarray != null && $scope.uploadarray.length > 0) {
                var msgnew = document.getElementById("documnetapproval").innerHTML;

                var data = {
                    "VTADAA_Id": $scope.VTADAA_IdNew,
                    filelist: $scope.uploadarray,
                    "VTADAA_Finaldocument": $scope.VTADAA_Finaldocumentdoc,
                    "MailSubject": "TADA DOCUMENT",
                    "Mailmsg": msgnew
                };
                apiService.create("TADA_Apply/Updatefile", data).then(function (promise) {
                    $scope.VTADAA_IdNew = "";
                    $('#FileviewDeatils').modal('hide');
                    swal("Record save Sucessfully  !");
                    $scope.onpageload();

                });
            }

        }

    }
})();

