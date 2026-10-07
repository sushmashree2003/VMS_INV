
login.controller("loginCtrl", ['$rootScope', '$scope', '$state', '$location', 'loginService', 'Flash', 'apiService', '$stateParams', 'AuthService', '$cookieStore', '$http', 'FormSubmitter', '$q','$timeout',
    function ($rootScope, $scope, $state, $location, loginService, Flash, apiService, $stateParams, AuthService, $cookieStore, $http, FormSubmitter, $q, $timeout) {

        var mi_id;
        var ip = "";
        $scope.iposbrowser = "";
        //$.getJSON('https://ipinfo.io/json?token=a28739c42d2f25', function (data) {
        //    ip = data;
        //});
        
        class DetectOS {
            constructor() {
                this.browser = this.searchString(this.dataBrowser())
                this.version = this.searchVersion(navigator.userAgent) || this.searchVersion(navigator.appVersion)
                this.OS = this.searchString(this.dataOS())
            }

            searchString(data) {
                for (let i = 0; i < data.length; i++) {
                    let
                        dataString = data[i].string,
                        dataProp = data[i].prop
                    this.versionSearchString = data[i].versionSearch || data[i].identity
                    if (dataString) {
                        if (dataString.indexOf(data[i].subString) !== -1) {
                            return data[i].identity
                        }
                    } else if (dataProp) {
                        return data[i].identity
                    }
                }
            }

            searchVersion(dataString) {
                let index = dataString.indexOf(this.versionSearchString)
                if (index === -1) return
                return parseFloat(dataString.substring(index + this.versionSearchString.length + 1))
            }

            dataBrowser() {
                return [
                    /***************
                     * Chrome
                     ***************/
                    {
                        string: navigator.userAgent,
                        subString: "Chrome",
                        identity: "Chrome"
                    },
                    /***************
                     * Safari
                     ***************/
                    {
                        string: navigator.vendor,
                        subString: "Apple",
                        identity: "Safari",
                        versionSearch: "Version"
                    },
                    /***************
                     * For Older Opera (12.18-)
                     ***************/
                    {
                        prop: window.opera,
                        identity: "Opera",
                        versionSearch: "Version"
                    },
                    /***************
                     * Internet Explorer 10
                     ***************/
                    {
                        string: navigator.userAgent,
                        subString: "MSIE",
                        identity: "IE10",
                        versionSearch: "MSIE"
                    },
                    /***************
                     * Internet Explorer 11
                     ***************/
                    {
                        string: navigator.userAgent,
                        subString: "Trident",
                        identity: "IE11",
                        versionSearch: "rv"
                    },
                    /***************
                     * Edge
                     ***************/
                    {
                        string: navigator.userAgent,
                        subString: "Edge",
                        identity: "Edge",
                        versionSearch: "Edge"
                    },
                    /***************
                     * Firefox
                     ***************/
                    {
                        string: navigator.userAgent,
                        subString: "Firefox",
                        identity: "Firefox"
                    },
                    {
                        string: navigator.userAgent,
                        subString: "Gecko",
                        identity: "Mozilla",
                        versionSearch: "rv"
                    },
                    /***************
                     * For Older Netscapes (4-)
                     ***************/
                    {
                        string: navigator.userAgent,
                        subString: "Mozilla",
                        identity: "Netscape",
                        versionSearch: "Mozilla"
                    },
                    /***************
                     * For Newer Netscapes (6+)
                     ***************/
                    {
                        string: navigator.userAgent,
                        subString: "Netscape",
                        identity: "Netscape"
                    },
                    /***************
                     * Other Browsers
                     ***************/
                    {
                        string: navigator.userAgent,
                        subString: "OmniWeb",
                        versionSearch: "OmniWeb/",
                        identity: "OmniWeb"
                    },
                    {
                        string: navigator.vendor,
                        subString: "iCab",
                        identity: "iCab"
                    },
                    {
                        string: navigator.vendor,
                        subString: "KDE",
                        identity: "Konqueror"
                    },
                    {
                        string: navigator.vendor,
                        subString: "Camino",
                        identity: "Camino"
                    }
                ]
            }

            dataOS() {
                return [
                    {
                        string: navigator.platform,
                        subString: 'Win',
                        identity: 'Windows'
                    },
                    {
                        string: navigator.platform,
                        subString: 'Mac',
                        identity: 'macOS'
                    },
                    {
                        string: navigator.userAgent,
                        subString: 'iPhone',
                        identity: 'iOS'
                    },
                    {
                        string: navigator.userAgent,
                        subString: 'iPad',
                        identity: 'iOS'
                    },
                    {
                        string: navigator.userAgent,
                        subString: 'iPod',
                        identity: 'iOS'
                    },
                    {
                        string: navigator.userAgent,
                        subString: 'Android',
                        identity: 'Android'
                    },
                    {
                        string: navigator.platform,
                        subString: 'Linux',
                        identity: 'Linux'
                    }
                ]
            }
        }

        const Detect = new DetectOS()

        //console.log("We know your browser – it's " + Detect.browser + " " + Detect.version);
        //console.log("We know your OS – it's " + Detect.OS);

        var HostName = location.host;
        $scope.Username2F = "";
        $scope.Password2F = "";
        $scope.twofactor = false;
        $scope.buttonClicked = "";
        $scope.toggleModal = function (btnClicked) {
            $scope.buttonClicked = btnClicked;
            $scope.showModal = !$scope.showModal;
        };
        $scope.sendmobotp = false;
        $scope.sendemailotp = false;
        $scope.usermorebefore = true;
        $scope.Paymentnotdone = true;
        $scope.Paymentdone = false;
        $scope.alumni = false;
        $scope.ProspectuseScreen = true;
        $scope.PaymentMode = false;
        var vm = this;
        vm.getUser = {};
        vm.setUser = {};
        //vm.getRegUser = {};
        vm.signIn = true;
        $scope.SelectedFileForUploadz = [];
        $scope.loaddded = false;
        var onlineprofilepicpath;
        $scope.calcel = function () {
            //vm.getRegUser = '';
            //$scope.submitted = false;
            //$scope.vm.registrationForm.$setPristine();
            //$scope.vm.registrationForm.$setUntouched();
            //vm.signIn = true;

            $state.reload();
            vm.signIn = true;
        }
        //Draggable Modal Start
        //$("#myModal3").draggable({
        //    handle: ".modal-header"
        //});


        var searchObject = $location.search();

        $scope.classdesable = true;
        // alert(searchObject.status);
        if (searchObject.status == "failure") {
            swal("Payment Unsuccessfull");
            //  Request.QueryString.Remove("status");
            //$location.url($location.path)
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        else if (searchObject.status == "created") {
            swal("Payment Successfull", "Account created successfully!!");
            //Request.QueryString.Remove("status");
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        else if (searchObject.status == "Networkfailure") {
            swal('Network failure..!!', 'Try again after some time');
            //Request.QueryString.Remove("status");
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        else if (searchObject.status == "checkadmin") {
            swal('Please contact administrator!!');
            //Request.QueryString.Remove("status");
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        else if (searchObject.status == "Username Already exist .. !!") {
            swal('Username Already exist .. !!');
            //Request.QueryString.Remove("status");
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }


        //$scope.transnumbconfigurationsettings = [];
        //$scope.transnumbconfigurationsettingsassign = {};
        //var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        //for (var i = 0; i < transnumconfigsettings.length; i++) {
        //    if (transnumconfigsettings.length > 0) {
        //        if (transnumconfigsettings[i].imN_Flag == "Online") {
        //            $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
        //        }
        //    }
        //}



        // Get Virtual 
        if ($state.current.name == 'login') {
            getVirtual();
        }

        function getVirtual() {
            var virtualSchoolId = 0;
            if ($stateParams.virtualId != "") {
                virtualSchoolId = $stateParams.virtualId
            }

            var data = {
                "subdomainname": virtualSchoolId,
                "hostname": HostName
            }
            apiService.create("Login/setvirtauldeatilsnew", data).then(function (promise) {

                // apiService.getURI("Login/setvirtauldeatilsnew", virtualSchoolId).then(function (promise) {

                //if (promise.multiplewindowvalue == "multiplevalue" || promise.multiplewindowvalue == "logout") {
                //    $state.go('login');
                //}
                //else if (promise.multiplewindowvalue == "differentlogin" || promise.multiplewindowvalue != "logout") {
                //    $state.go('app.homepage');
                //}
                //else if (promise.multiplewindowvalue == "logout") {
                //    $state.go('login');
                //}
                mi_id = promise.mi_id;
                if (promise.subDomainName != null && promise.subDomainName != "0") {




                    var genconfig = promise.ivrmconfiglist;

                    var institutions = promise.fillinstitution;

                    var conlistlist = promise.configlist;
                    localStorage.setItem("Startconfig", JSON.stringify(genconfig));


                    $scope.configotp = promise.ivrmconfiglist;

                    $scope.noint = promise.noint;
                    $scope.institut = promise.fillinstitution;
                    $scope.emailarrayLis = promise.emailarrayList;
                    $scope.phonearrayLis = promise.phonearrayList;
                    $scope.getaccyear = promise.allyearlist;
                    $scope.getclass = promise.allclasslist;
                    if (promise.registration == true) {
                        $scope.prereg = false;
                        $scope.filepre = true;
                        $scope.preadmissionmsg = "";
                    }
                    else {
                        $scope.prereg = true;
                        $scope.filepre = false;
                        $scope.preadmissionmsg = "Preadmission Registration is closed for the current date..!!";
                    }


                    if (promise.payment == true) {
                        $scope.Paymentnotdone = false;
                        $scope.Paymentdone = true;


                    }
                    else {
                        $scope.Paymentnotdone = true;
                        $scope.Paymentdone = false;
                    }


                    angular.forEach($scope.configotp, function (object) {
                        $scope.moboto = object.ivrmgC_MobileValOTPFlag;
                        $scope.emailboto = object.ivrmgC_emailValOTPFlag;
                        if ($scope.moboto == true) {
                            $scope.sendmobotp = true;
                        }
                        else {
                            $scope.sendmobotp = false;
                        }
                        if ($scope.emailboto == true) {
                            $scope.sendemailotp = true;
                        } else {
                            $scope.sendemailotp = false;
                        }
                    })

                    angular.forEach($scope.institut, function (object) {
                        $scope.background = object.mI_BackgroundImage;
                        $scope.logo = object.mI_Logo;
                        $scope.Aboutinstitute = object.mI_AboutInstitute;
                        $scope.instituteContactDetails = object.mI_ContactDetails;
                        $scope.instituteHelpFile = object.mI_HelpFile;
                        $scope.ivrs = object.mI_IVRSVirtualNo;
                    })

                    angular.forEach($scope.emailarrayLis, function (object) {
                        $scope.emailinst = object.miE_EmailId;

                    })

                    angular.forEach($scope.phonearrayLis, function (object) {
                        $scope.phoneinst = object.mipN_PhoneNo;
                    })

                    $scope.loaddded = true;
                    $scope.trustorint = true;


                    if (promise.noint == true) {
                        swal("Please Enter Correct School Subdomain..!!");
                    }
                }
                else {

                    $scope.institut = promise.fillinstitution;
                    $scope.emailarrayLis = promise.emailarrayList;
                    $scope.phonearrayLis = promise.phonearrayList;

                    angular.forEach($scope.institut, function (object) {
                        $scope.logo = object.mO_Logo;
                        $scope.Aboutinstitute = object.mO_AboutInstitute;
                        $scope.instituteContactDetails = object.mO_ContactDetails;
                    })

                    angular.forEach($scope.emailarrayLis, function (object) {
                        $scope.emailinst = object.moE_EmailId;

                    })

                    angular.forEach($scope.phonearrayLis, function (object) {
                        $scope.phoneinst = object.mopN_PhoneNo;
                    })

                    $scope.Paymentnotdone = true;
                    $scope.Paymentdone = false;
                    $scope.loaddded = true;
                    $scope.prereg = true;
                    $scope.filepre = false;
                    $scope.trustorint = false;
                }

                if (promise.multiplewindowvalue == "differentlogin") {
                    $state.go('app.homepage');
                    //swal("You have already logged in with the same credentials in another window.This is for kind information!!")
                }
                else if (promise.multiplewindowvalue == "logout") {
                    $state.go('login');
                }
            });
        }
        //

        $scope.Back = function () {

            $scope.PaymentMode = false;
            $scope.ProspectuseScreen = true;

            // $scope.cancel();
            //  $state.reload();


        }

        $scope.alumniclick = function () {
            $scope.alumni = true;
            vm.signIn = false;
            $scope.Paymentdone = false;
            $scope.Paymentnotdone = false;
        }

        $scope.ForgetEmailInput = false;
        $scope.forgetEmailOtp = false;
        $scope.forgetemailOtpbutton = false;
        $scope.forgetemailResendOtpbutton = false;
        $scope.forgetemailOtpMatch = false;
        $scope.forgetemailOtpNotMatch = false;
        $scope.forgetEmailOtp = false;

        $scope.forgetmobileInput = false;
        $scope.forgetmobileInputotp = false;
        $scope.forgetmobileOtpbutton = false;
        $scope.forgetmobileResendOtpbutton = false;
        $scope.forgetmobileOtpMatch = false;
        $scope.forgetmobileOtpNotMatch = false;

        $scope.resetpassword = false;
        $scope.EmailMobileConform = false;
        $scope.UserNameSubmit = true;

        $scope.checkUsername = function (Username) {
            if (Username === "" || Username === undefined) {
                swal("Please Enter User name!");
            }
            else {
                var mobno = {
                    "UserName": Username
                }

                apiService.create("Login/VerifyUserName", mobno).then(function (promise) {
                    swal(promise);
                    if (promise.userNameVerifyStatus === "Success") {

                        $scope.EmailMobileConform = true;
                        $scope.forgetUserName = true;
                        $scope.UserNameSubmit = false;
                        $scope.forgetEMaildisable = true;
                        $scope.forgetMobiledisable = true;
                        $scope.vm.getRegUser.forgetEmail_id = promise.email;
                        $scope.vm.getRegUser.mobileNo = promise.mobileNo;
                    }
                    else {
                        swal("User Name Not Found..!");

                        $scope.EmailMobileConform = false;
                        $scope.forgetUserName = false;
                    }
                })

            }
        }

        $scope.forgetEmailOrMobileOption = function (Type) {
            if (Type === "EMAIL") {
                $scope.ForgetEmailInput = true;
                $scope.forgetEmailOtp = false;
                $scope.forgetemailOtpbutton = true;
                $scope.forgetemailResendOtpbutton = false;
                $scope.forgetemailOtpMatch = false;
                $scope.forgetemailOtpNotMatch = false;

                $scope.forgetmobileInput = false;
                $scope.forgetmobileInputotp = false;
                $scope.forgetmobileOtpbutton = false;
                $scope.forgetmobileResendOtpbutton = false;
                $scope.forgetmobileOtpMatch = false;
                $scope.forgetmobileOtpNotMatch = false;
            }
            else if (Type === "MOBILE") {

                $scope.forgetmobileInput = true;
                $scope.forgetmobileInputotp = false;
                $scope.forgetmobileOtpbutton = true;
                $scope.forgetmobileResendOtpbutton = false;
                $scope.forgetmobileOtpMatch = false;
                $scope.forgetmobileOtpNotMatch = false;


                $scope.ForgetEmailInput = false;
                $scope.forgetEmailOtp = false;
                $scope.forgetemailOtpbutton = false;
                $scope.forgetemailResendOtpbutton = false;
                $scope.forgetemailOtpMatch = false;
                $scope.forgetemailOtpNotMatch = false;
            }
        }

        $scope.clear = function () {
            $scope.twofactor = false;
        };

        $scope.forgetemailOtp = function (UserName, forgetEmail, usertype) {
            if (forgetEmail === "" || forgetEmail === undefined) {
                swal("Please Enter Email Id!");
            }
            else {
                var mobno = {
                    "Email": forgetEmail,
                    "clickedlinkname": $scope.clickedlinkname,
                    "usertype": usertype,
                    "UserName": UserName
                };
                apiService.create("Login/ForgotOTPForEmail", mobno).then(function (promise) {
                    if ($scope.clickedlinkname == 'forgotpwd') {
                        $scope.forgetemailOtpbutton = false;
                        $scope.forgetEmailOtp = true;
                        $scope.forgetemailResendOtpbutton = true;
                        swal(promise.message);
                    }
                    else if ($scope.clickedlinkname == 'forgotname') {

                        if (promise != null && promise != '') {
                            if (promise.userlist != null && promise.userlist.length > 0) {
                                $scope.usermore = true;
                                $scope.usermorebefore = false;
                                $scope.userlist = promise.userlist;
                            }
                            else {
                                $scope.usermore = false;
                                $scope.usermorebefore = true;
                            }
                            swal(promise.message);
                            $scope.clearforgot();
                            $('#myModal3').modal('hide');
                        }
                        else {
                            $scope.clearforgot();
                        }
                    }
                })
            }
        }

        $scope.twofactorloginOtp = function (Username2F, Password2F) {
            $scope.Username2F = Username2F;
            $scope.Password2F = Password2F;
            $scope.vm.getUser.twofactopotp = "";
            var mobno = {
                "UserName": Username2F,
                "logindetails": $scope.iposbrowser
            };
            apiService.create("Login/twofactorloginOtp", mobno).then(function (promise) {
                if (promise.message == "true") {
                    // $('#myModaluser').modal('show');
                    $scope.twofactor = true;
                    swal("OTP shared to your Email-Id!");
                }
                else {
                    $scope.twofactor = false;
                }
            })
        }

        $scope.Verifyfor2fEmailOtp = function (data) {          
            var mobno = {
                "EMAILOTP": data
            }
            apiService.create("Login/VerifyEmailOtp", mobno).then(function (promise) {
                if (promise === "Success") {
                    vm.login($scope.Username2F, $scope.Password2F, false);
                }
                else if (promise === "Fail") {
                    swal("OTP Mismatch!");
                } else if (promise === "otp") {
                    swal("Kindly Enter OTP!");
                }
                else {
                    swal("OTP Expired. Please resend OTP");
                }
            })
        }

        $scope.ForgotEmailstudent = function (usertype) {

            var mobno = {
                "AMST_Id": usertype,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("Login/ForgotEmailstudent", mobno).then(function (promise) {
                if (promise.message != null) {
                    swal(promise.message);
                    $scope.clearforgot();
                    $('#myModal3').modal('hide');
                }
            })
        }

        $scope.VerifyforgetEmailOtp = function (data) {
            if (data === "" || data === undefined) {
                swal("Please Enter OTP Number!");
            }
            else {

                var mobno = {
                    "EMAILOTP": data
                }

                apiService.create("Login/VerifyEmailOtp", mobno).then(function (promise) {

                    if (promise === "Success") {

                        $scope.forgetemailOtpbutton = false;
                        $scope.forgetEmailOtp = false;
                        $scope.forgetemailOtpMatch = true;
                        $scope.forgetemailOtpNotMatch = false;
                        $scope.forgetemailResendOtpbutton = false;
                        $scope.forgetEMaildisable = true;

                        $scope.resetpassword = true;
                    }
                    else if (promise === "Fail") {
                        swal("OTP Mismatch!");

                        $scope.forgetemailOtpbutton = false;
                        $scope.forgetEmailOtp = true;
                        $scope.forgetemailOtpMatch = false;
                        $scope.forgetemailOtpNotMatch = true;
                        $scope.forgetemailResendOtpbutton = true;
                        $scope.forgetEMaildisable = false;

                    }
                    else {
                        swal("OTP Expired. Please resend OTP");

                        $scope.forgetemailOtpbutton = false;
                        $scope.forgetEmailOtp = true;
                        $scope.forgetemailOtpMatch = false;
                        $scope.forgetemailOtpNotMatch = true;
                        $scope.forgetemailResendOtpbutton = true;
                        $scope.forgetEMaildisable = false;

                    }
                })
            }
        }


        $scope.forgetmobileOtp = function (MobileNO) {
            if (MobileNO === "" || MobileNO === undefined) {
                swal("Please Enter Mobile Number!");
            }
            else {
                var mobno = {
                    "mobileNo": MobileNO
                }

                apiService.create("Login/getOTPForMobile", mobno).then(function (promise) {
                    swal(promise);

                    $scope.forgetmobileInputotp = true;
                    $scope.forgetmobileOtpbutton = false;
                    $scope.forgetmobileResendOtpbutton = true;
                })

            }
        }

        $scope.VerifyforgetmobileOtp = function (data) {
            if (data === "" || data === undefined) {
                swal("Please Enter OTP Number!");
            }
            else {

                var mobno = {
                    "MOBILEOTP": data
                }

                apiService.create("Login/VerifymobileOtp", mobno).then(function (promise) {

                    if (promise === "Success") {

                        $scope.forgetmobileInputotp = false;
                        $scope.forgetmobileOtpbutton = false;
                        $scope.forgetmobileResendOtpbutton = false;
                        $scope.forgetmobileOtpMatch = true;
                        $scope.forgetmobileOtpNotMatch = false;
                        $scope.forgetMobiledisable = true;

                        $scope.resetpassword = true;
                    }
                    else if (promise === "Fail") {
                        swal("OTP Mismatch!!");

                        $scope.forgetmobileInputotp = true;
                        $scope.forgetmobileOtpbutton = false;
                        $scope.forgetmobileResendOtpbutton = false;
                        $scope.forgetmobileOtpMatch = false;
                        $scope.forgetmobileOtpNotMatch = true;
                        $scope.forgetMobiledisable = false;
                    }
                    else {
                        swal("OTP Expired. Please resend OTP");

                        $scope.forgetmobileInputotp = true;
                        $scope.forgetmobileOtpbutton = false;
                        $scope.forgetmobileResendOtpbutton = false;
                        $scope.forgetmobileOtpMatch = false;
                        $scope.forgetmobileOtpNotMatch = true;
                        $scope.forgetMobiledisable = false;
                    }
                })
            }
        }



        $scope.MobileOtp = false;
        $scope.mobileOtpbutton = true;
        $scope.mobileOtpMatch = false;
        $scope.mobileOtpNotMatch = false;

        $scope.mobileOtp = function (MobileNO) {
            if (MobileNO === "" || MobileNO === undefined) {
                swal("Please Enter Mobile Number!");
            }
            else {
                var mobno = {
                    "mobileNo": MobileNO
                }

                apiService.create("Login/getOTPForMobile", mobno).then(function (promise) {
                    swal(promise);

                    $scope.mobileOtpbutton = false;
                    $scope.MobileOtp = true;
                    $scope.sendmobotp = false;

                    $scope.mobileResendOtpbutton = true;
                })

            }
        }



        $scope.VerifymobileOtp = function (data) {
            if (data === "" || data === undefined) {
                swal("Please Enter OTP Number!");
            }
            else {

                var mobno = {
                    "MOBILEOTP": data
                }

                apiService.create("Login/VerifymobileOtp", mobno).then(function (promise) {

                    if (promise === "Success") {

                        $scope.mobileOtpbutton = false;
                        $scope.MobileOtp = false;
                        $scope.mobileOtpMatch = true;
                        $scope.mobileOtpNotMatch = false;
                        $scope.mobileResendOtpbutton = false;
                        $scope.Mobiledisable = true;
                    }
                    else if (promise === "Fail") {
                        swal("OTP Mismatch!!");

                        $scope.mobileOtpbutton = false;
                        $scope.MobileOtp = true;
                        $scope.mobileOtpMatch = false;
                        $scope.mobileOtpNotMatch = true;
                        $scope.mobileResendOtpbutton = true;
                        $scope.Mobiledisable = false;
                    }
                    else {
                        swal("OTP Expired. Please resend OTP");
                        $scope.mobileOtpbutton = false;
                        $scope.MobileOtp = true;
                        $scope.mobileOtpMatch = false;
                        $scope.mobileOtpNotMatch = true;
                        $scope.mobileResendOtpbutton = true;
                        $scope.Mobiledisable = false;
                    }
                })
            }
        }



        $scope.EmailOtp = false;
        $scope.emailOtpbutton = true;
        $scope.emailOtpMatch = false;
        $scope.emailOtpNotMatch = false;

        $scope.emailOtp = function (EmailId) {
            if (EmailId === "" || EmailId === undefined) {
                swal("Please Enter Email Id!");
            }
            else {
                var mobno = {
                    "Email": EmailId
                }

                apiService.create("Login/getOTPForEmail", mobno).then(function (promise) {
                    swal(promise);

                    $scope.emailOtpbutton = false;
                    $scope.EmailOtp = true;
                    $scope.sendemailotp = false;
                    $scope.emailResendOtpbutton = true;
                })
            }
        }


        $scope.VerifyemailOtp = function (data) {
            if (data === "" || data === undefined) {
                swal("Please Enter OTP Number!");
            }
            else {

                var mobno = {
                    "EMAILOTP": data
                }

                apiService.create("Login/VerifyEmailOtp", mobno).then(function (promise) {

                    if (promise === "Success") {

                        $scope.emailOtpbutton = false;
                        $scope.EmailOtp = false;
                        $scope.emailOtpMatch = true;
                        $scope.emailOtpNotMatch = false;
                        $scope.emailResendOtpbutton = false;
                        $scope.EMaildisable = true;
                    }
                    else if (promise === "Fail") {
                        swal("OTP Mismatch!!");

                        $scope.emailOtpbutton = false;
                        $scope.EmailOtp = true;
                        $scope.emailOtpMatch = false;
                        $scope.emailOtpNotMatch = true;
                        $scope.emailResendOtpbutton = true;
                        $scope.EMaildisable = false;
                    }
                    else {
                        swal("OTP Expired. Please resend OTP");

                        $scope.emailOtpbutton = false;
                        $scope.EmailOtp = true;
                        $scope.emailOtpMatch = false;
                        $scope.emailOtpNotMatch = true;
                        $scope.emailResendOtpbutton = true;
                        $scope.EMaildisable = false;
                    }
                })
            }
        }


        $scope.submitted1 = false;
        vm.forgotpassword = function (data) {

            if ($scope.forgotpasswordForm.$valid) {

                if (data.password2 != data.Confirmpassword) {
                    swal('Confirm Password is Not Match with New Password');
                    return;
                }
                var dt = { "newPassword": data.password2, "UserName": data.userName, "ForgetPasswordSelection": $scope.Type }

                if (data.Confirmpassword == data.password2) {
                    apiService.create("Login/sendforgotpassword", dt).then(function (promise) {
                        if (promise.message == "Success") {
                            $scope.clearforgot();
                            swal("Successfully Reset Your Password, Please Login With Your New Password..!");
                            //$state.go('login' + promise.subDomainName)
                            $('#myModal3').modal('hide');
                            $scope.vm.getRegUser = "";
                            window.location.href = "http://localhost:57606/#/login/";
                        }
                        else {
                            swal(promise.message);
                        }
                    });
                }
                else {
                    swal("Password Mismatch..!");
                    return;
                }
            }
            else {
                $scope.submitted1 = true;
            }
        }

        $scope.clearforgot = function () {
            $scope.vm.getRegUser = "";
            $scope.EmailMobileConform = false;
            $scope.ForgetEmailInput = false;
            $scope.forgetEmailOtp = false;
            $scope.UserNameSubmit = true;
            $scope.forgetUserName = false;
            $scope.Type = false;
            $scope.forgetEmail_id = "";
            $scope.forgetEmailOTP = "";
            $scope.forgetEMaildisable = false;
            $scope.forgetmobileInput = false;
            $scope.usermore = false;
            $scope.usermorebefore = true;
            $scope.resetpassword = false;
            var mobno = {
                "EMAILOTP": 1
            }
            apiService.create("Login/getsubdomain", mobno).
                then(function (promise) {
                    if (promise.subDomainName != null) {
                        window.location.href = "http://localhost:57606/#/login/" + promise.subDomainName;
                    }
                    else {
                        window.location.href = "http://localhost:57606/#/login/";
                    }
                })
        };
        // forgot password added on 10-11-2016
        //vm.DrawCaptcha = function () {
        //    var a = Math.ceil(Math.random() * 10) + '';
        //    var b = Math.ceil(Math.random() * 10) + '';
        //    var c = Math.ceil(Math.random() * 10) + '';
        //    var d = Math.ceil(Math.random() * 10) + '';
        //    var e = Math.ceil(Math.random() * 10) + '';
        //    var f = Math.ceil(Math.random() * 10) + '';
        //    var g = Math.ceil(Math.random() * 10) + '';
        //    var code = a + ' ' + b + ' ' + ' ' + c + ' ' + d + ' ' + e + ' ' + f + ' ' + g;
        //    $scope.vm.getUser.txtCaptcha = code;
        //}


        vm.loginNEW = function (Username, Password) {
            if ($scope.vm.loginForm != undefined) {
                if ($scope.vm.loginForm.$valid) {
                    // txtCaptcha = txtCaptcha.split(' ').join('');

                    // txtInput = txtInput.split(' ').join('');

                    //  if (txtCaptcha == txtInput) {
                    var data = {
                        "Username": Username,
                        "password": Password
                    }
                    apiService.get("Login/getMIdata", data).then(function (promise) {

                        if (promise.message != '' && promise.message != null) {
                            swal(promise.message);
                        }
                        else {
                            $scope.viewpopup = "false";

                            $scope.MIdata = promise.mIdata;

                            if (promise.mIdata.length > 1) {
                                $('#institution').modal('show');
                            }
                            else if (promise.mIdata.length == 1) {
                                vm.login(Username, Password, promise.mIdata[0].mI_Id);
                            }
                            else {
                                $scope.institution = '#';
                            }
                        }
                    });
                }
                else {
                    swal("Please Enter Correct Captcha Number..!");
                }
            }            
        };

        ////access login
        vm.login = function (Username, Password, auth) {
            //  txtCaptcha = txtCaptcha.split(' ').join('');
            // txtInput = txtInput.split(' ').join('');

            $.getJSON('https://ipinfo.io/json?token=a28739c42d2f25', function (data_ip) {
                ip = data_ip;

                $scope.iposbrowser = ip.ip + "," + ip.city + "," + ip.region + "," + ip.country + "," + Detect.browser + "," + Detect.version + "," + Detect.OS;
            });

            if (ip != "" && ip != undefined && $scope.iposbrowser=="") {
                $scope.iposbrowser = ip.ip + "," + ip.city + "," + ip.region + "," + ip.country + "," + Detect.browser + "," + Detect.version + "," + Detect.OS;
                //$scope.iposbrowser.push({ ip: ip.ip, city: ip.city, region: ip.region, country: ip.country, browser: Detect.browser, version: Detect.version, os: Detect.OS })
            }
            $timeout(function () { $scope.login_details(Username, Password, auth); }, 1000);
        };


        $scope.login_details = function (Username, Password, auth) {

            if (Username != undefined || Username != null) {
                if (Password != undefined || Password != null) {
                    //if (txtCaptcha == txtInput) {
                    var data = {
                        "Username": Username,
                        "password": Password,
                        "ClientId": mi_id,
                        "Logintype": "Web",
                        "twofactorcheck": auth,
                        "logindetails": $scope.iposbrowser
                    }
                    apiService.get("Login", data).then(function (promise) {
                        if (promise.error != "invalid_grant") {
                            if (promise.roleId != 0) {
                                localStorage.setItem('testObject', JSON.stringify());

                                var data = {
                                    "Username": Username,
                                    "password": Password,
                                    "ClientId": mi_id,
                                    "Logintype": "Web"
                                }
                                apiService.create("Login/getrolewisepage", data).then(function (promise) {
                                    $scope.twoFactorEnabled = promise.twoFactorEnabled;
                                    if (promise.filldashpagemap.length > 0) {
                                        if (promise.pageexists.length > 0) {
                                            $scope.daspgenme = promise.filldashpagemap[0].ivrmP_Dasboard_PageName
                                        }
                                        else {
                                            $scope.daspgenme = "app.homepage";
                                        }
                                    }
                                    else {
                                        $scope.daspgenme = "app.homepage";
                                    }

                                    $state.go($scope.daspgenme);

                                })
                                // $state.go($scope.dashhmepage);
                            }
                            else {
                                grecaptcha.reset();
                                swal(promise.message);
                            }
                        }
                        else {
                            if (promise.error_description == "expired") {
                                $scope.curpwd = "";
                                $scope.newpwd = "";
                                $scope.cnfpwd = "";
                                $scope.submitted = false;
                                if (Username != 'vaps super admin' && Username != 'sadmin') {
                                    $('#changepassword').modal('show');
                                    //var modal = document.getElementById("changepassword");
                                    //modal.style.display = "block";
                                }
                            }
                            else if (promise.error_description == "Alumni") {
                                swal("Alumni Membership Not Yet Approved..!!!")
                            }
                            else if (promise.error_description == "towfactor") {
                                $scope.twofactorloginOtp(Username, Password);
                            }
                            else {
                                swal(promise.error_description)
                            }
                        }
                    });
                }
                //else {
                //    swal("Please Enter Correct Captcha Number..!");
                //}
                else {
                    swal("Please Enter Password..!");
                }
            }
            else {
                swal("Please Enter Username..!");
            }


        };





        $scope.submitted = false;
        $scope.SubmitForm = function (Username) {
            $scope.submitted = true;
            if ($scope.myForm2.$valid) {

                if ($scope.newpwd != $scope.cnfpwd) {
                    swal('Confirm Password is Not Match with New Password!!');
                    return;
                }
                else if ($scope.curpwd == $scope.newpwd) {
                    swal('New Password should not be same as old password!!');
                    return;
                }

                var data = {
                    "password": $scope.curpwd,
                    "new_password": $scope.newpwd,
                    "username": Username,
                }
                apiService.create("Changepswd/", data).
                    then(function (promise) {
                        debugger;
                        if (promise.returnMsg === "Success") {

                            swal('Password changed Successfully', 'success');
                            $('#changepassword').modal('hide');
                            //$state.reload();
                        }
                        else if (promise.returnMsg === "fail") {
                            swal('Kindly enter valid Current password', 'Password is incorrect');
                            return;
                        }
                        else if (promise.returnMsg != "") {
                            swal(promise.returnMsg);
                            return;
                        }
                        else if (promise.returnMsg == "Error") {
                            swal('Password Not Changed', 'Failed');
                            $state.reload();
                        }
                    })
            }
        };

        $scope.submitted = false;
        $scope.SubmitForm1 = function (Username) {
            $scope.submitted = true;
            if ($scope.myForm2.$valid) {

                if ($scope.newpwd != $scope.cnfpwd) {
                    swal('Confirm Password is Not Match with New Password!!');
                    return;
                }
                else if ($scope.curpwd == $scope.newpwd) {
                    swal('New Password should not be same as old password!!');
                    return;
                }

                var data = {
                    "password": $scope.curpwd,
                    "new_password": $scope.newpwd,
                    "username": Username,
                }
                apiService.create("Changepswd/saveexppassword", data).
                    then(function (promise) {

                        if (promise.returnMsg === "Success") {

                            swal('Password changed Successfully', 'success');
                            $('#changepassword').modal('hide');
                            //$state.reload();
                        }
                        else if (promise.returnMsg === "fail") {
                            swal('Kindly enter valid Current password', 'Password is incorrect');
                            return;
                        }
                        else if (promise.returnMsg != "") {
                            swal(promise.returnMsg);
                            return;
                        }
                        else if (promise.returnMsg == "Error") {
                            swal('Password Not Changed', 'Failed');
                            $state.reload();
                        }
                    })
            }
        };


        //var d;
        //var now = new Date();
        //d = new Date(now);
        //alert(d.getMinutes() + ':' + d.getSeconds()); //11:55
        //d.setSeconds(d.getSeconds() + 1800);
        //alert(d.getMinutes() + ':0' + d.getSeconds()); //12:05

        //var now2 = new Date();

        //(now2.getMinutes() + ':' + now2.getSeconds());
        //alert("cur", d);
        //alert("exp", now2);



        //var retrievedObject = localStorage.getItem('testObject');

        //if (retrievedObject == "0" || retrievedObject == "" || retrievedObject == undefined || retrievedObject === "undefined") {
        //    grecaptcha.reset();
        //    swal("You can't proceed..first Need to clear Captcha..!");
        //} else {



        // }


        //access login
        //$scope.getAuthCredential = function (data) {

        //    var authdata = "username=" + data.Username +
        //                   "&password=" + data.Password +
        //                   "&grant_type=password" +
        //                   "&scope=offline_access";

        //    //var data = jsonToURI(authdata);
        //    apiService.createAuth("Authorization/connect/token", authdata).then(function (promise) {
        //        console.log(promise);
        //        var error = promise.error;
        //        if (promise.error === undefined) {
        //            var accesstoken = promise.access_token;
        //            var token_type = promise.token_type;
        //            var expires_in = promise.expires_in;
        //            var refresh_token = promise.refresh_token;
        //            localStorage.setItem('testObject', JSON.stringify());
        //            localStorage.setItem('accesstoken', accesstoken);
        //            localStorage.setItem('refresh_token', refresh_token);

        //            var timestamp = Math.round(new Date() / 1000) + expires_in;

        //            localStorage.setItem('session_expiresin', refresh_token);

        //            var session_expiresin = localStorage.getItem('session_expiresin');

        //            //if (Math.round(new Date() / 1000) > timestamp) {
        //            //    alert("Session has expired");
        //            //}


        //            $state.go('app.homepage');
        //        }
        //        else {
        //            grecaptcha.reset();
        //            swal(promise.error_description);
        //        }

        //        //var error_description = promise.error_description;


        //        //  var accesstokenObject = localStorage.getItem('testObject');

        //    });
        //};

        vm.lock_disable = function (data) {

            var data = {
                "Username": $scope.uName,
                "password": data.pwd
            }
            apiService.get("Login", data).then(function (promise) {
                if (promise.userId != 0) {

                    $state.go('app.homepage');
                }
                else {
                    swal(promise.message);
                    //$state.reload();
                    $scope.vm.getLockUser.pwd = "";
                }

            });
        };


        $scope.SelectedFileForUploadz = [];
        $scope.selectFileforUploadz = function (input) {
            $scope.SelectedFileForUploadz = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#blahh')
                        .attr('src', e.target.result)
                        .width(133)
                        .height(120);
                };
                reader.readAsDataURL(input.files[0]);
                Uploadprofile();
            } else if (input.files[0].type != "image/jpeg") {
                swal("Please Upload the JPEG file");
                return;
            } else if (input.files[0].size > 2097152) {
                swal("Image size should be less than 2MB");
                return;

            }
        }

        function Uploadprofile() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadz.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadz[i]);
            }

            //We can send more data to server using append         
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadprofilepicpreadmission", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    // swal(d);

                    onlineprofilepicpath = d;
                    //$scope.reg.PASR_Student_Pic_Path = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }

        function Uploads() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadz.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadz[i]);
            }
            formData.append("Id", pasR_Id);

        };


        $scope.getOTP = function (data) {

            var mobno = {
                "mobileNo": data.mobileNo,
                "UserName": data.userName
            }

            apiService.create("Login/getOTP", mobno).then(function (promise) {
                swal(promise);
            })
        }

        $scope.pamentsave = function () {

            var payu_URL = $scope.payu_URL;
            var url = payu_URL;
            var method = 'POST';
            var params = {
                "Key": $scope.key,
                "txnid": $scope.txnid,
                "amount": $scope.amount,
                "productinfo": $scope.productinfo,
                "firstname": $scope.firstname,
                "email": $scope.email,
                "phone": $scope.phone,
                "surl": $scope.surl,
                "furl": $scope.furl,
                "hash": $scope.hash,
                "udf1": $scope.udf1,
                "udf2": $scope.udf2,
                "udf3": $scope.udf3,
                "udf4": $scope.udf4,
                "udf5": $scope.udf5,
                "udf6": $scope.udf6,
                "service_provider": $scope.service_provider,
                "hash_string": $scope.hash_string

            }
            FormSubmitter.submit(url, method, params);
        }

        $scope.submitted = false;
        vm.register = function (data) {


            if ($scope.vm.registrationForm.$valid) {

                var vm = this;
                vm.getUser = {};
                vm.setUser = {};

                if (data.Confirmpassword == data.Password) {

                    var formData = new FormData();

                    formData.append("onlinepreadminfilepath", onlineprofilepicpath);

                    formData.append("Name", data.Name);
                    formData.append("Email_id", data.Email_id);
                    formData.append("Mobileno", data.Mobile);
                    formData.append("Username", data.Username);
                    formData.append("Password", data.Password);
                    formData.append("Otpmobl", $scope.moboto);
                    formData.append("Otpemail", $scope.emailboto);
                    //formData.append("transnumbconfigurationsettingsss", $scope.transnumbconfigurationsettingsassign[0]);

                    $http.post("/api/Login/paynow", formData,
                        {
                            withCredentials: true,
                            headers: { 'Content-Type': undefined },
                            transformRequest: angular.identity
                        }).then(function (response) {
                            //if (response.data != "" && response.data != "created") {
                            //    swal(response.data);
                            //}
                            //else if (response.data == "created") {
                            //    swal("Account created successfully");
                            //    vm.signIn = true;
                            //    $scope.vm.getRegUser = "";
                            //    $scope.vm.getRegUser.Name = "";
                            //    $scope.vm.getRegUser.Email_id = "";
                            //    $scope.vm.getRegUser.Mobile = "";
                            //    $scope.vm.getRegUser.Username = "";
                            //    $scope.vm.getRegUser.Password = "";
                            //    $scope.vm.getRegUser.Confirmpassword = "";
                            //    vm.DrawCaptcha();
                            //    $state.go('Login');

                            //}
                            //else {
                            //    swal(response.data);
                            //}

                            if (response.data.description == "Username Already exist .. !!" || response.data.description == "Email id Already exist .. !!") {
                                swal(response.data.description);
                                //$state.reload();

                            }
                            else {

                                if (response.data.paymentDetailsList != null) {



                                    $scope.PASR_FirstName = response.data.paymentDetailsList[0].firstname;
                                    $scope.PASR_emailId = response.data.paymentDetailsList[0].email;
                                    $scope.PASR_MobileNo = response.data.paymentDetailsList[0].phone;
                                    $scope.Pasr_amount = response.data.paymentDetailsList[0].amount;

                                    $scope.key = response.data.paymentDetailsList[0].marchanT_ID;
                                    $scope.txnid = response.data.paymentDetailsList[0].trans_id;
                                    $scope.amount = response.data.paymentDetailsList[0].amount;
                                    $scope.productinfo = response.data.paymentDetailsList[0].productinfo;
                                    $scope.firstname = response.data.paymentDetailsList[0].firstname;
                                    $scope.email = response.data.paymentDetailsList[0].email;
                                    $scope.phone = response.data.paymentDetailsList[0].phone;
                                    $scope.surl = response.data.paymentDetailsList[0].surl;
                                    $scope.furl = response.data.paymentDetailsList[0].furl;
                                    $scope.hash = response.data.paymentDetailsList[0].hash;
                                    $scope.udf1 = response.data.paymentDetailsList[0].udf1;
                                    $scope.udf2 = response.data.paymentDetailsList[0].udf2;
                                    $scope.udf3 = response.data.paymentDetailsList[0].udf3;
                                    $scope.udf4 = response.data.paymentDetailsList[0].udf4;
                                    $scope.udf5 = response.data.paymentDetailsList[0].udf5;
                                    $scope.udf6 = response.data.paymentDetailsList[0].udf6;
                                    $scope.service_provider = response.data.paymentDetailsList[0].service_provider;

                                    $scope.hash_string = response.data.paymentDetailsList[0].hash_string;
                                    $scope.payu_URL = response.data.paymentDetailsList[0].payu_URL;

                                    $scope.PaymentMode = true;
                                    $scope.ProspectuseScreen = false;
                                    swal.close();
                                    showConfirmButton: false
                                }
                                else {
                                    swal("Please Contact Administrator!!");
                                    $state.reload();
                                }

                            }


                        })

                }

                else {
                    swal("Password Mismatch..!");
                    return;
                }
            }

            else {
                $scope.submitted = true;
            }
        };



        $scope.submitted = false;
        vm.registeralumni = function (data) {


            if ($scope.vm.registrationForm.$valid) {

                var vm = this;
                vm.getUser = {};
                vm.setUser = {};

                if (data.Confirmpassword == data.Password) {

                    var formData = new FormData();

                    formData.append("onlinepreadminfilepath", onlineprofilepicpath);

                    formData.append("Name", data.Name);
                    formData.append("Email_id", data.Email_id);
                    formData.append("Mobileno", data.Mobile);
                    formData.append("Username", data.Username);
                    formData.append("admittedyer", data.asmaY_Id);
                    formData.append("leftyear", data.asmaY_Idleft);
                    formData.append("leftclassid", data.classid);
                    formData.append("Password", data.Password);
                    formData.append("Otpmobl", $scope.moboto);
                    formData.append("Otpemail", $scope.emailboto);
                    formData.append("Alumni", "Alumni");
                    //formData.append("transnumbconfigurationsettingsss", $scope.transnumbconfigurationsettingsassign[0]);

                    $http.post("/api/Login/Regis", formData,
                        {
                            withCredentials: true,
                            headers: { 'Content-Type': undefined },
                            transformRequest: angular.identity
                        }).then(function (response) {
                            if (response.data != "" && response.data != "created") {
                                swal(response.data);
                            }
                            else if (response.data == "created") {
                                swal("Registered successfully");
                                //vm.signIn = true;
                                //$scope.vm.getRegUser = "";
                                //$scope.vm.getRegUser.Name = "";
                                //$scope.vm.getRegUser.Email_id = "";
                                //$scope.vm.getRegUser.Mobile = "";
                                //$scope.vm.getRegUser.Username = "";
                                //$scope.vm.getRegUser.Password = "";
                                //$scope.vm.getRegUser.Confirmpassword = "";
                                //vm.DrawCaptcha();
                                //$scope.alumni = false;

                                //$state.go('Login');
                                $state.reload();

                            }
                            else {
                                swal(response.data);
                            }

                        })

                }

                else {
                    swal("Password Mismatch..!");
                    return;
                }
            }

            else {
                $scope.submitted = true;
            }
        };

        $scope.submitted = false;
        vm.registernotpay = function (data) {


            if ($scope.vm.registrationForm.$valid) {

                var vm = this;
                vm.getUser = {};
                vm.setUser = {};

                if (data.Confirmpassword == data.Password) {

                    var formData = new FormData();

                    formData.append("onlinepreadminfilepath", onlineprofilepicpath);

                    formData.append("Name", data.Name);
                    formData.append("Email_id", data.Email_id);
                    formData.append("Mobileno", data.Mobile);
                    formData.append("Username", data.Username);
                    formData.append("Password", data.Password);
                    formData.append("Otpmobl", $scope.moboto);
                    formData.append("Otpemail", $scope.emailboto);
                    //formData.append("transnumbconfigurationsettingsss", $scope.transnumbconfigurationsettingsassign[0]);

                    $http.post("/api/Login/Regis", formData,
                        {
                            withCredentials: true,
                            headers: { 'Content-Type': undefined },
                            transformRequest: angular.identity
                        }).then(function (response) {
                            if (response.data != "" && response.data != "created") {
                                swal(response.data);
                            }
                            else if (response.data == "created") {
                                swal("Account created successfully");
                                vm.signIn = true;
                                $scope.vm.getRegUser = "";
                                $scope.vm.getRegUser.Name = "";
                                $scope.vm.getRegUser.Email_id = "";
                                $scope.vm.getRegUser.Mobile = "";
                                $scope.vm.getRegUser.Username = "";
                                $scope.vm.getRegUser.Password = "";
                                $scope.vm.getRegUser.Confirmpassword = "";
                                vm.DrawCaptcha();
                                $state.go('Login');

                            }
                            else {
                                swal(response.data);
                            }

                        })

                }

                else {
                    swal("Password Mismatch..!");
                    return;
                }
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
        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };



        //$scope.GetUser = function () {
        //    apiService.get("Login/getRole/").
        //  then(function (promise) {
        //     
        //      if (promise != "") {
        //          $scope.uName = promise.userData[0].userName;
        //          $scope.rName = promise.userData[0].roleName;
        //          $scope.imgname = promise.userData[0].userImagePath;
        //      }
        //  });
        //}

        $scope.resetalldata = function () {
            $state.reload();
        }

        //Added on 02-02-2018.
        $scope.forgotpwd = function () {
            $scope.clickedlinkname = "forgotpwd";
            $scope.ForgetEmailInput = false;
            $scope.EmailMobileConform = false;
        }
        $scope.forgotname = function () {
            $scope.clickedlinkname = "forgotname";
            $scope.ForgetEmailInput = true;
            $scope.EmailMobileConform = true;
        };

        $scope.forgortpwd = function () {
            $scope.clickedlinkname = "forgotpwd";
            $scope.ForgetEmailInput = false;
            $scope.EmailMobileConform = false;
            $scope.forgetUserName = false;
            $scope.UserNameSubmit = true;
            $scope.vm.getRegUser = "";
            $scope.vm.getRegUser.userName = "";
            $scope.vm.getRegUser.password2 = "";
            $scope.vm.getRegUser.Confirmpassword = "";
            $('#myModal3').modal('show');
        };

    }]);
