(function () {
    'use strict';
    angular
        .module('app')
        .controller('GeneralSendSMSController', GeneralSendSMSController)

    GeneralSendSMSController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', '$http', 'Flash', 'apiService', '$stateParams', '$filter', '$q', 'superCache']
    function GeneralSendSMSController($rootScope, $scope, $state, $location, dashboardService, $http, Flash, apiService, $stateParams, $filter, $q, superCache) {


        //dashboard.controller("General_SendSMSController", ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache',
        //function ($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache) {
        $scope.searchValue = '';
        $scope.stsearch = '';
        $scope.sortReverse = true;
        $scope.snd_email = false;
        $scope.snd_sms = false;
        $scope.stfsnd_email = false;
        $scope.stfsnd_sms = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.loadradioval = true;
        $scope.exm_radioval = 'mark';
        $scope.attend_radioval = 'between';
        $scope.stdmsg = '';
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 5;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        var configsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));

        if (configsettings !== null && configsettings.length > 0) {
            var emailotp = configsettings[0].ivrmgC_emailValOTPFlag;
            var mobileotp = configsettings[0].ivrmgC_MobileValOTPFlag;

            $scope.emailotp = configsettings[0].ivrmgC_emailValOTPFlag;
            $scope.mobileotp = configsettings[0].ivrmgC_MobileValOTPFlag;
            //hard code
            //emailotp = 1;
            //mobileotp = 1;
            //$scope.emailotp =1;
            //$scope.mobileotp = 1;

            if ($scope.emailotp == 1) {
                $scope.emailotpshow = true;
            }
            else {
                $scope.emailotpshow = false;
            }

            if ($scope.mobileotp == 1) {
                $scope.mobileotpshow = true;
            }
            else {
                $scope.mobileotpshow = false;
            }


            var mobilenofull = configsettings[0].ivrmgC_OTPMobileNo.toString();
            //  mobilenofull = '9591081840';
            if (mobilenofull != '0') {
                var otpmobile = mobilenofull.substring(6, 10);
                $scope.mobileno = otpmobile;
            }
            var emailidforotp = configsettings[0].ivrmgC_OTPMailId;
            // emailidforotp = 'praveend114@gmail.com';
            if (emailidforotp != null || emailidforotp != undefined) {
                $scope.emailid = emailidforotp.substring(0, 4);
            }


            if ($scope.emailotp == 1 || $scope.mobileotp == 1) {
                $scope.otpcheck = true;

            }
            else {
                $scope.otpcheck = false;
            }
        }
        else {
            var emailotp = 0;
            var mobileotp = 0;

        }

        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.changeradio1 = function () {
            $scope.stdmsg = 'Hi';
            if ($scope.selradioval == 'exam') {

                $scope.stdmsg = 'Hi';
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    // "ASMS_Id": $scope.asmS_Id
                };
                if ($scope.count > 0) {
                    apiService.create("General_SendSMS/Getexam/", data).
                        then(function (promise) {

                            $scope.exmstdlist = promise.exmstdlist;

                        });
                }
            }
        };

        $scope.otponclickloaddata = function () {
            $scope.buttonotp = true;
            if ($scope.otptype == 'M') {
                $scope.otpmobile = true;
                $scope.otpemail = false;
            }
            else if ($scope.otptype == 'E') {
                $scope.otpmobile = false;
                $scope.otpemail = true;
            }
        };



        $scope.VerifyforgetEmailOtp = function (data) {

            if (data === "" || data === undefined) {
                swal("Please Enter OTP Number!");
            }
            else {

                var mobno = {
                    "EMAILOTP": data
                };

                apiService.create("Login/VerifyEmailOtpgen", mobno).then(function (promise) {
                    if (promise === "Success") {
                        $scope.otpcheck = false;
                    }

                    else if (promise === "Fail") {
                        $scope.otpcheck = true;
                        $scope.resendotpbutton = true;
                        swal("OTP Mismatch!");
                    }
                    else {
                        $('#myModalswal').modal('hide');
                        $('#myModalotp').modal('show');
                        swal("OTP Expired. Please resend OTP");

                    }
                });
            }
        };



        $scope.resendotp = function () {
            $('#myModalswal').modal('show');
            //  $('#myModalotp').modal('show');
        };

        $scope.stfsearchValue = '';
        $scope.sendotpsms = function (forgetEmail) {

            $("#myModalswal").modal({ backdrop: false });
            $scope.resendotpbutton = false;
            $scope.forgetEmailOTP = "";

            if (emailidforotp == null || emailidforotp == undefined) {
                emailidforotp = "test@mail";
            }
            // $('#myModalotp').modal('hide');
            var mobno = {
                "clickedlinkname": forgetEmail,
                "Mobile": mobilenofull.toString(),
                "Email": emailidforotp.toString(),
            };

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };

            apiService.create("Login/ForgotOTPForEmailval", mobno).then(function (promise) {
                if ($scope.radioval == 'Student') {
                    $scope.clslst2 = $scope.albumNameArray;
                    $("#myModalswal").modal({ backdrop: false });
                }
                if ($scope.radioval == 'Staff') {
                    $scope.clslst21 = $scope.albumNameArray;
                    $("#myModalswal").modal({ backdrop: false });
                }

                console.log($scope.clslst2);
                $('#myModalswal').modal('show');
                $("#myModalswal").modal({ backdrop: false });
            });

        };


        $scope.submitted = true;
        $scope.email = true;
        $scope.sms = true;

        $scope.saveUseronce = function () {

            $scope.otpmobile = false;
            $scope.otpemail = false;
            $scope.buttonotp = false;
            if ($scope.emailotp == 1 || $scope.mobileotp == 1) {
                if ($scope.email == true || $scope.sms == true) {
                    $scope.otpcheck = true;
                }
                else {
                    $scope.otpcheck = false;
                }
            }

            else {
                $scope.otpcheck = false;
            }

            if ($scope.radioval == 'Student') {
                if ($scope.exm_radioval == 'mark' && $scope.selradioval == 'exam') {
                    $scope.examdd = 'examddd';
                } else if ($scope.exm_radioval == 'mark' && $scope.selradioval == 'grade') {
                    $scope.examdd = 'gradeddd';
                } else if ($scope.selradioval == 'attendance') {
                    $scope.examdd = 'attendanceddd';
                } else {
                    $scope.examdd = 'ggddd';
                }


                $scope.albumNameArray = [];
                angular.forEach($scope.studentList, function (user) {
                    if (!!user.selected) {
                        $scope.albumNameArray.push(user);
                    }
                });
                if ($scope.albumNameArray.length > 0) {


                    if ((emailotp != 0 && ($scope.email == true || $scope.sms == true))) {
                        $("#myModalotp").modal({ backdrop: false });
                        var emailidforotp = configsettings[0].ivrmgC_OTPMailId;
                        if (emailidforotp != null || emailidforotp != undefined) {
                            $scope.emailid = emailidforotp.substring(0, 4);
                            $('#myModalotp').modal('show');
                        }
                        else {
                            swal("Authorized Email ID Not Found!!");
                        }

                    }
                    else if ((mobileotp != 0 && ($scope.email == true || $scope.sms == true))) {
                        $("#myModalotp").modal({ backdrop: false });
                        if (mobilenofull != '0') {
                            var otpmobile = mobilenofull.substring(6, 10);
                            $scope.mobileno = otpmobile;
                            $('#myModalotp').modal('show');
                        }
                        else {
                            swal("Authorized Mobile Number Not Found!!");
                        }
                    }
                    else {
                        $scope.stdmsg = $scope.stdmsg;
                        $scope.clslst2 = $scope.albumNameArray;
                        $('#myModalswal').modal('show');
                        $("#myModalswal").modal({ backdrop: false });
                    }
                }
                else {
                    swal('Kindly select atleast one student..!');
                    return;
                }
            }

            if ($scope.radioval == 'General') {
                if ($scope.myForm.$valid) {

                    if ((emailotp != 0 && ($scope.email == true || $scope.sms == true))) {

                        $("#myModalotp").modal({ backdrop: false });

                        var emailidforotp = configsettings[0].ivrmgC_OTPMailId;
                        // emailidforotp = 'praveend114@gmail.com';
                        if (emailidforotp != null || emailidforotp != undefined) {
                            $scope.emailid = emailidforotp.substring(0, 4);
                            $('#myModalotp').modal('show');
                        }
                        else {
                            swal("Authorized Email ID Not Found!!");
                        }

                    }
                    else
                        if ((mobileotp != 0 && ($scope.email == true || $scope.sms == true))) {

                            $("#myModalotp").modal({ backdrop: false });

                            if (mobilenofull != '0') {
                                var otpmobile = mobilenofull.substring(6, 10);
                                $scope.mobileno = otpmobile;
                                $('#myModalotp').modal('show');
                            }
                            else {
                                swal("Authorized Mobile Number Not Found!!");
                            }
                        }
                        else {
                            $scope.clslst2 = $scope.albumNameArray;
                            $('#myModalswal').modal('show');
                            $("#myModalswal").modal({ backdrop: false });
                        }
                }
                else {
                    $scope.submitted = true;
                }
            }


            if ($scope.radioval == 'Staff') {

                $scope.albumNameArray = [];

                angular.forEach($scope.employeelst, function (user) {
                    if (user.selected === true) {
                        $scope.albumNameArray.push(user);
                    }
                });
                console.log($scope.albumNameArray);

                if ($scope.albumNameArray.length > 0) {

                    if ((emailotp != 0 && ($scope.email == true || $scope.sms == true))) {

                        $("#myModalotp").modal({ backdrop: false });

                        var emailidforotp = configsettings[0].ivrmgC_OTPMailId;
                        // emailidforotp = 'praveend114@gmail.com';

                        if (emailidforotp != null || emailidforotp != undefined) {

                            $scope.emailid = emailidforotp.substring(0, 4);
                            $('#myModalotp').modal('show');
                        }
                        else {
                            swal("Authorized Email ID Not Found!!");
                        }

                    }
                    else if ((mobileotp != 0 && ($scope.email == true || $scope.sms == true))) {
                        $("#myModalotp").modal({ backdrop: false });
                        if (mobilenofull != '0') {
                            var otpmobile = mobilenofull.substring(6, 10);
                            $scope.mobileno = otpmobile;
                            $('#myModalotp').modal('show');
                        }
                        else {
                            swal("Authorized Mobile Number Not Found!!");
                        }
                    }
                    else {
                        $scope.clslst21 = $scope.albumNameArray;
                        console.log($scope.clslst2);
                        $('#myModalswal').modal('show');
                        $("#myModalswal").modal({ backdrop: false });
                    }
                }
                else {

                    swal('Kindly select atleast one Employee..!');
                    return;
                }
            }
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.changeradio = function () {
            //alert('a')
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.radiovalnewregular = "Regular";
            if ($scope.loadradioval == true) {
                $scope.radioval = 'General';
                $scope.loadradioval = false;
            }

            if ($scope.radioval == 'General') {
                $scope.mobno = "";
                $scope.mes = "";
            }

            if ($scope.radioval == 'Student') {


                $scope.snd_email = true;
                $scope.snd_sms = true;
                $scope.selradioval = '';
                $scope.emE_Id = '';
                $scope.exmstdlist = [];


                $scope.stdmsg = "";
                var data = {
                    "selectedRadiobtn": $scope.radioval
                }

                $scope.studentList = [];
                apiService.create("General_SendSMS/Getdetails/", data).
                    then(function (promise) {

                        $scope.fillacademiyear = promise.yearlist;
                        $scope.currentYear = promise.currentYear;
                        for (var i = 0; i < $scope.fillacademiyear.length; i++) {
                            if ($scope.currentYear[0].asmaY_Id == $scope.fillacademiyear[i].asmaY_Id) {
                                $scope.ASMAY_Id = $scope.currentYear[0].asmaY_Id;
                            }
                            $scope.classlist = promise.classlist;
                            $scope.ASMCL_Id = $scope.classlist[0].asmcL_Id;
                            $scope.sectionlist = promise.sectionlist;
                            $scope.asmS_Id = $scope.sectionlist[0].asmS_Id;

                            if (promise.studentCount > 0) {
                                $scope.count = promise.studentCount;
                                $scope.studentList = promise.studentlist;
                                //  console.log($scope.studentList);
                            }
                            else {
                                swal("No records found for selected academicYear,class and section");
                                $scope.count = 0;
                            }
                        }
                    });
            }
            if ($scope.radioval == 'Staff') {
                var id = 1;
                $scope.departmentdropdown = [];
                $scope.Designation_types = [];
                $scope.stfsnd_email = true;
                $scope.stfsnd_sms = true;
                $scope.deptcheck = false;

                $scope.employeelst = [];
                $scope.stfmsg = "";
                apiService.getURI("General_SendSMS/Getdepartment", id).
                    then(function (promise) {
                        $scope.departmentdropdown = promise.departmentdropdown;
                        if (promise.departmentdropdown != null) {
                            $scope.deptcheck = true;

                            $scope.all_checkdep();
                        }
                    });
            }
        };

        $scope.all_checkdep = function () {

            $scope.employeelst = [];
            var toggleStatus = $scope.deptcheck;
            angular.forEach($scope.departmentdropdown, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_designation();
        };

        $scope.get_designation = function () {
            $scope.employeelst = [];
            $scope.deptcheck = $scope.departmentdropdown.every(function (options) {
                return options.selected;
            });

            $scope.get_designationnew();
        };

        $scope.get_designationnew = function () {
            $scope.desgcheck = "";
            var groupidss;
            for (var i = 0; i < $scope.departmentdropdown.length; i++) {
                if ($scope.departmentdropdown[i].selected == true) {

                    if (groupidss == undefined)
                        groupidss = $scope.departmentdropdown[i].hrmD_Id;
                    else
                        groupidss = groupidss + "," + $scope.departmentdropdown[i].hrmD_Id;
                }
            }

            if (groupidss != undefined) {
                var data = {
                    "multipledep": groupidss,
                };
                apiService.create("General_SendSMS/get_designation", data).
                    then(function (promise) {

                        $scope.Designation_types = promise.designationdropdown;
                        if (promise.designationdropdown != null) {
                            $scope.desgcheck = true;
                            $scope.all_checkdesg();
                        }
                    });
            }
            else {
                $scope.Designation_types = "";
                $scope.Employeelst = "";
            }
        };

        $scope.all_checkdesg = function () {

            var toggleStatus = $scope.desgcheck;
            angular.forEach($scope.Designation_types, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_employee();
        };

        //fill desg end
        //fill employee start
        $scope.get_employee = function () {
            $scope.employeelst = [];
            $scope.desgcheck = $scope.Designation_types.every(function (options) {

                return options.selected;
            });

            $scope.get_employeenew();
        };

        $scope.get_employeenew = function () {
            $scope.stf = false;
            $scope.Stflist = false;
            var deptIds;
            for (var i = 0; i < $scope.departmentdropdown.length; i++) {
                if ($scope.departmentdropdown[i].selected == true) {

                    if (deptIds == undefined)
                        deptIds = $scope.departmentdropdown[i].hrmD_Id;
                    else
                        deptIds = deptIds + "," + $scope.departmentdropdown[i].hrmD_Id;
                }
            }
            var groupidss;
            for (var i = 0; i < $scope.Designation_types.length; i++) {
                if ($scope.Designation_types[i].selected == true) {

                    if (groupidss == undefined)
                        groupidss = $scope.Designation_types[i].hrmdeS_Id;
                    else
                        groupidss = groupidss + "," + $scope.Designation_types[i].hrmdeS_Id;
                }
            }
            if (groupidss != undefined) {
                var data = {
                    "multipledes": groupidss,
                    "multipledep": deptIds
                };
                apiService.create("General_SendSMS/get_employee", data).
                    then(function (promise) {

                        $scope.employeelst = promise.stafflist;
                        if ($scope.employeelst.length > 0) {
                            $scope.stf = true;
                        }
                    });
            }
            else {
                $scope.employeelst = "";
            }
        };

        $scope.GetStudentDetails = function () {
            $scope.selradioval = '';
            $scope.emE_Id = '';
            $scope.all = false;
            $scope.exmstdlist = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.asmS_Id,
                "neworregular": $scope.radiovalnewregular
            };
            apiService.create("General_SendSMS/GetStudentDetails/", data).
                then(function (promise) {

                    if (promise.studentCount > 0) {

                        if ($scope.selradioval == 'exam') {

                            $scope.changeradio1();

                        }
                        $scope.count = promise.studentCount;
                        $scope.studentList = promise.studentlist;
                    } else {
                        swal("No records found for selected academicYear,class and section");
                        $scope.studentList = "";
                        $scope.count = 0;
                    }
                });
        };

        $scope.exname = '';
        $scope.stumarkdetails = [];
        $scope.at_details = [];
        $scope.searchstddetails = function () {
            $scope.studentList = [];

            $scope.all = false;
            $scope.submitted1 = true;
            if ($scope.myForm1.$valid) {

                var data = {};
                if ($scope.selradioval == 'exam') {
                    data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "ASMS_Id": $scope.asmS_Id,
                        "EME_Id": $scope.emE_Id,
                        "selradioval": $scope.selradioval
                    };
                }
                if ($scope.selradioval == 'attendance') {

                    if ($scope.attend_radioval == 'between') {
                        $scope.curnt_date = new Date();
                    } else {
                        $scope.frm_date = new Date();
                        $scope.to_date = new Date();
                    }

                    data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "ASMS_Id": $scope.asmS_Id,
                        "selradioval": $scope.selradioval,
                        "attend_radioval": $scope.attend_radioval,
                        "fr_date": new Date($scope.frm_date).toDateString(),
                        "to_date": new Date($scope.to_date).toDateString(),
                        "crnt_date": new Date($scope.curnt_date).toDateString(),
                    };
                }

                apiService.create("General_SendSMS/searchstddetails/", data).
                    then(function (promise) {

                        if (promise.studentCount > 0) {
                            $scope.count = promise.studentCount;
                            $scope.studentList = promise.studentlist;

                            if ($scope.selradioval == 'exam') {
                                $scope.stumarkdetails = promise.stumarkdetails;
                                if (promise.stumarkdetails != null) {
                                    $scope.temparray = [];
                                    angular.forEach($scope.studentList, function (stu) {
                                        var marks = '';
                                        var grade = '';
                                        angular.forEach($scope.stumarkdetails, function (mrk) {
                                            if (stu.amsT_Id == mrk.amsT_Id) {

                                                if (marks == '') {
                                                    if (mrk.estmpS_PassFailFlg == 'AB' || mrk.estmpS_PassFailFlg == 'M' || mrk.estmpS_PassFailFlg == 'L') {
                                                        marks = mrk.result;
                                                    }
                                                    else {
                                                        marks = mrk.marksDetails;
                                                    }

                                                }
                                                else {
                                                    if (mrk.estmpS_PassFailFlg == 'AB' || mrk.estmpS_PassFailFlg == 'M' || mrk.estmpS_PassFailFlg == 'L') {
                                                        marks = marks + ',' + mrk.result;
                                                    }
                                                    else {
                                                        marks = marks + ',' + mrk.marksDetails;
                                                    }

                                                }
                                                if (grade == '') {

                                                    if (mrk.estmpS_PassFailFlg == 'AB' || mrk.estmpS_PassFailFlg == 'M' || mrk.estmpS_PassFailFlg == 'L') {
                                                        grade = mrk.result;
                                                    }
                                                    else {
                                                        grade = mrk.gradeDetails;
                                                    }

                                                }
                                                else {
                                                    if (mrk.estmpS_PassFailFlg == 'AB' || mrk.estmpS_PassFailFlg == 'M' || mrk.estmpS_PassFailFlg == 'L') {

                                                        grade = grade + ',' + mrk.result;

                                                    }
                                                    else {
                                                        grade = grade + ',' + mrk.gradeDetails;
                                                    }
                                                }
                                            }
                                        });

                                        var finaltotal = '';
                                        var finalgrade = '';
                                        angular.forEach($scope.stumarkdetails, function (fnl) {
                                            if (stu.amsT_Id == fnl.amsT_Id) {
                                                finaltotal = fnl.totalMarks;
                                                finalgrade = fnl.totalGrade;
                                            }
                                        });

                                        grade = grade + ',' + finalgrade;
                                        marks = marks + ',' + finaltotal;

                                        $scope.temparray.push({ amsT_Id: stu.amsT_Id, studentName: stu.studentName, amsT_AdmNo: stu.amsT_AdmNo, amsT_MobileNo: stu.amsT_MobileNo, amsT_emailId: stu.amsT_emailId, marksDetails: marks, gradeDetails: grade });
                                    });

                                    $scope.studentList = $scope.temparray;
                                    $scope.temparraynew = [];
                                    if ($scope.exm_radioval === 'grade') {
                                        angular.forEach($scope.studentList, function (grad) {
                                            if (grad.gradeDetails !== ",") {
                                                $scope.temparraynew.push(grad);
                                            }
                                        });
                                    }
                                    if ($scope.exm_radioval === 'mark') {
                                        angular.forEach($scope.studentList, function (grad) {
                                            if (grad.marksDetails !== ",") {
                                                $scope.temparraynew.push(grad);
                                            }
                                        });
                                    }
                                    $scope.studentList = $scope.temparraynew;
                                    console.log($scope.studentList);
                                }
                            }

                            if ($scope.selradioval == 'attendance') {

                                $scope.at_details = promise.attdetails;
                                if ($scope.at_details.length != 0) {

                                    $scope.temparray1 = [];
                                    var lb = '';
                                    if ($scope.attend_radioval == 'between') {
                                        var fromdate1 = $filter('date')($scope.frm_date, 'dd-MM-yyyy');
                                        var todate1 = $filter('date')($scope.to_date, 'dd-MM-yyyy');
                                        //$scope.frm_date = $filter('date')($scope.frm_date, 'dd-MM-yyyy');
                                        //  $scope.to_date = $filter('date')($scope.to_date, 'dd-MM-yyyy');

                                        lb = 'Student Attendence From' + ' ' + ' ' + fromdate1 + ' ' + 'To' + ' ' + ' ' + todate1 + ' ' + ' ' + '-' + ' ';
                                    }
                                    if ($scope.attend_radioval == 'current') {
                                        var curdate = $filter('date')($scope.curnt_date, 'dd-MM-yyyy');
                                        // $scope.curnt_date = $filter('date')($scope.curnt_date, 'dd-MM-yyyy');
                                        lb = 'Student Attendence On' + ' ' + ' ' + curdate + ' ' + '-' + ' ';
                                    }

                                    angular.forEach($scope.studentList, function (stu1) {
                                        var attt = '';
                                        //attt = lb + 'Total Class Held :' + 100 + '/' + 'Total Class Present :' + 80;
                                        angular.forEach($scope.at_details, function (at1) {
                                            if (stu1.amsT_Id == at1.amsT_Id) {
                                                attt = lb + ' ' + 'Total Class Held :' + at1.totalworkingday + ' ' + '/' + ' ' + 'Total Class Present :' + at1.totalpresentday + '   ' + 'Percentage :' + at1.attper;
                                            }

                                        });
                                        $scope.temparray1.push({ amsT_Id: stu1.amsT_Id, studentName: stu1.studentName, amsT_AdmNo: stu1.amsT_AdmNo, amsT_MobileNo: stu1.amsT_MobileNo, amsT_emailId: stu1.amsT_emailId, atndetails: attt });

                                    });
                                    $scope.studentList = $scope.temparray1;
                                }
                            }

                        } else {
                            swal("No records found for selected academicYear,class and section");
                            $scope.studentList = "";
                            $scope.count = 0;
                        }
                    });

            }
            else {
                $scope.submitted1 = true;
            }
        };


        $scope.order1 = function (keyname) {
            $scope.sortKey1 = keyname;   //set the sortKey to the param passed
            $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
        };


        $scope.order = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };
        $scope.submitted = false;

        $scope.SendData = function () {

            if ($scope.myForm.$valid) {
                //    var Send = {
                //        "Mobno": $scope.mobno,
                //        "mes": $scope.mes,
                //        "radiotype": $scope.radioval
                //    }
                //    apiService.create("General_SendSMS/savedetail", Send).then(function (promise) {
                //        if (promise.smsStatus == 'sent') {
                //            swal("SMS Sent Successfully.");
                //            $state.reload();
                //        }
                //        else {
                //            swal("SMS Sending Cancelled.");
                //        }
                //    });

                $('#myModalswal').modal('hide');
                swal({
                    title: "Are you sure?",
                    text: "Do you want to Send SMS ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            var Send = {
                                "Mobno": $scope.mobno,
                                "mes": $scope.mes,
                                "radiotype": $scope.radioval
                            };

                            apiService.create("General_SendSMS/savedetail", Send).then(function (promise) {
                                if (promise.smsStatus == 'sent') {
                                    swal("SMS Sent Successfully.");
                                    $state.reload();
                                }
                                else {
                                    swal("SMS Sending Cancelled.");
                                }
                            });
                        }
                        else {
                            //dd
                        }
                    });

            }
            else {
                $scope.submitted = true;
            }
        };




        $scope.SendMSG = function (stdmsg) {
            //alert($scope.radioval)

            $('#myModalswal').modal('hide');
            $scope.printstudents = [];
            angular.forEach($scope.studentList, function (user) {
                if (!!user.selected) $scope.printstudents.push(user);
            });

            if ($scope.printstudents.length > 0) {
                swal({
                    title: "Are you sure?",
                    text: "Do you want to Send SMS?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Send it!",
                    cancelButtonText: "Cancel!!!!!!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            if ($scope.selradioval == 'exam') {

                                angular.forEach($scope.exmstdlist, function (ex) {
                                    if ($scope.emE_Id == ex.emE_Id) {
                                        $scope.exname = ex.emE_ExamName;
                                    }
                                });

                                $scope.curnt_date = new Date();
                                $scope.frm_date = new Date();
                                $scope.to_date = new Date();
                            }

                            else if ($scope.selradioval == 'attendance') {
                                $scope.emE_Id = 0;
                                if ($scope.attend_radioval == 'between') {
                                    $scope.curnt_date = new Date();
                                } else {
                                    $scope.frm_date = new Date();
                                    $scope.to_date = new Date();
                                }
                            }

                            else {
                                $scope.frm_date = new Date();
                                $scope.to_date = new Date();
                                $scope.curnt_date = new Date();
                                $scope.emE_Id = 0;
                            }
                            var data = {
                                studentlistdto: $scope.printstudents,
                                "SmsMailText": stdmsg,
                                "radiotype": $scope.radioval,
                                "exm_radioval": $scope.exm_radioval,
                                "selradioval": $scope.selradioval,
                                "snd_email": $scope.snd_email,
                                "snd_sms": $scope.snd_sms,
                                "exmname": $scope.exname,
                                "ASMAY_Id": $scope.ASMAY_Id,
                                "ASMCL_Id": $scope.ASMCL_Id,
                                "ASMS_Id": $scope.asmS_Id,
                                "EME_Id": $scope.emE_Id,
                                "fr_date": new Date($scope.frm_date).toDateString(),
                                "to_date": new Date($scope.to_date).toDateString(),
                                "crnt_date": new Date($scope.curnt_date).toDateString(),
                                "attend_radioval": $scope.attend_radioval
                            };
                            apiService.create("General_SendSMS/savedetail", data).then(function (promise) {
                                if (promise.emailStatus === 'sent') {

                                    if ($scope.snd_email === true && $scope.snd_sms === false) {
                                        swal("Email Sent Successfully.");
                                    } else if ($scope.snd_email === false && $scope.snd_sms === true) {
                                        swal("SMS Sent Successfully.");
                                    } else {
                                        swal("SMS And Mail Sent Successfully.");
                                    }
                                    $state.reload();
                                }

                            });
                        }
                        else {
                            swal("SMS And Mail Sending Cancelled.");
                        }
                    });

            } else {
                swal("Kindly select atleast a record to procced..!");
                return;
            }
        };


        $scope.SendStaffData = function () {
            //alert($scope.radioval)

            $scope.printstudents = [];
            angular.forEach($scope.employeelst, function (employee) {
                if (employee.hrmE_MobileNo != null) {
                    if (!!employee.selected) $scope.printstudents.push(employee);
                }
            });

            if ($scope.printstudents.length > 0) {
                swal({
                    title: "Are you sure?",
                    text: "Do you want to Send SMS?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Send it!",
                    cancelButtonText: "Cancel!!!!!!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {

                            var data = {
                                studentlistdto: $scope.printstudents,
                                "SmsMailText": $scope.stfmsg,
                                "radiotype": $scope.radioval,
                                "stfsnd_email": $scope.stfsnd_email,
                                "stfsnd_sms": $scope.stfsnd_sms
                            };
                            apiService.create("General_SendSMS/savedetail", data)
                                .then(function (promise) {
                                    if (promise.smsStatus === 'sent') {

                                        if ($scope.stfsnd_email === true && $scope.stfsnd_sms === false) {
                                            swal("Email Sent Successfully.");
                                        } else if ($scope.stfsnd_email === false && $scope.stfsnd_sms === true) {
                                            swal("SMS Sent Successfully.");
                                        } else {
                                            swal("SMS And Email Sent Successfully.");
                                        }
                                        $state.reload();
                                    }
                                    else {
                                        swal("SMS Not Sent for all selection.");
                                    }
                                });
                        }
                        else {
                            swal("SMS Sending Cancelled.");
                        }
                    });

            } else {
                swal("Kindly select atleast a record to having  mobile number to procced..!");
                return;
            }
        };

        $scope.onSelectclass = function (classId) {
            if ($scope.ASMAY_Id > 0 && $scope.ASMCL_Id >= 0 && $scope.asmS_Id >= 0) {
                $scope.GetStudentDetails();
            }
        };
        $scope.onSelectyear = function () {
            if ($scope.ASMAY_Id > 0 && $scope.ASMCL_Id >= 0 && $scope.asmS_Id >= 0) {
                $scope.GetStudentDetails();
            }
        };

        $scope.printstudents = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.studentList, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
        };

        $scope.printstudents = [];
        $scope.toggleAll1 = function () {
            var toggleStatus = $scope.Stflist;
            angular.forEach($scope.employeelst, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.Stflist == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
        };



        $scope.optionToggled1 = function (SelectedStudentRecord, index) {

            $scope.all = $scope.studentList.every(function (options) {

                return options.selected;
            });

            //$scope.all = $scope.employeelst.every(function (itm)
            //{ return itm.selected; });
            //if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
            //    $scope.printstudents.push(SelectedStudentRecord);
            //}
            //else {
            //    $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            //}
        };

        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.Stflist = $scope.employeelst.every(function (options) {

                return options.selected;
            });

            //$scope.all = $scope.employeelst.every(function (itm)
            //{ return itm.selected; });
            //if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
            //    $scope.printstudents.push(SelectedStudentRecord);
            //}
            //else {
            //    $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            //}
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted1 = function (field1) {
            return $scope.submitted1;
        };
        $scope.interacted2 = function (field2) {
            return $scope.submitted2;
        };

        $scope.cancel = function () {
            $scope.mobno = "";
            $scope.mes = "";
            $state.reload();
        };
        $scope.cancel1 = function () {

            $scope.stdmsg = "";

            $scope.all = false;
            angular.forEach($scope.studentList, function (user) {
                user.selected = false;
            });
        };
        $scope.cancel2 = function () {
            $scope.stfmsg = "";
            $scope.Stflist = false;
            angular.forEach($scope.departmentdropdown, function (user) {
                user.selected = false;
            });
            $scope.employeelst = [];
            $scope.desgcheck = false;
            $scope.stf = false;
            $scope.Designation_types = [];
        };


        //IVRS Code written by surya  
        $scope.objivr = {};
        $scope.IVRS_CHANGE_TYPES = function () {
            $scope.datagridviewIVR = true;
        };


        $scope.dataArray = [];
        $scope.BindData_ivr = function () {
            apiService.getDATA("IVRSOBD/getalldetails").
                then(function (promise) {
                    $scope.ivr_fillacademiyear = promise.acs_lst;
                    $scope.ivr_classlist = promise.clas_list;
                    $scope.ivr_sectionlist = promise.sect_list;
                    $scope.mi_id = promise.mI_ID;
                    $scope.schoolname = promise.schoolname;
                    $scope.datagridviewIVR = true;
                    $scope.gridOptions.data = promise.maindata;
                });
        };

        $scope.maketestcall = function (objivr) {

            var mobi = "";
            if ($scope.rndtype === '1000033778') {
                mobi = "+91" + $scope.ivr_test_mobile + "," + $scope.mi_id + "," + $scope.objivr.ttscontent +
                    ", " + $scope.schoolname + ",http://bdmobileapp.azurewebsites.net/api/IVRS/updatecredits";
            }
            else if ($scope.rndtype === '1000033928') {
                mobi = "+91" + $scope.ivr_test_mobile + "," + $scope.mi_id + "," + $scope.objivr.urlimage +
                    ", " + $scope.schoolname + ",http://bdmobileapp.azurewebsites.net/api/IVRS/updatecredits";
            }
            var data = {
                "phonebookname": "phonebook",
                "numbers": mobi
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json charset=UTF-8',
                    'x-api-key': 'lF4vZUSwA8Jab0ABWsITtxwM1ZwL6h2jZDdCTX30',
                    'Authorization': '964c52d4-016a-4d31-9b0a-86daaf43bbc5',
                    'Access-Control-Allow-Origin': '*',
                    'Access-Control-Allow-Methods': 'POST',
                    'cache-control': 'no-cache'
                }
            };
            $http.post('https://kpi.knowlarity.com/Basic/v1/account/contacts/phonebook', data, config)
                .success(function (data, status, config) {

                    //$scope.PostDataResponse = data.id;

                    $scope.getMessage(data.id);

                    //$scope.scheduleCall(data.id);

                })
                .error(function (data, status, config) {
                    $scope.PostDataResponse = "Data: " + data;
                });
            $('#myivrstestpop').modal('hide');
        };

        $scope.getMessage = function (id) {
            setTimeout(function () {
                $scope.$apply(function () {
                    //wrapped this within $apply
                    $scope.scheduleCall(id);
                });
            }, 2000);
        };


        $scope.scheduleCall = function (ph_id) {
            if ($scope.ph_id != "") {
                var date = new Date();
                var year = date.getFullYear();
                var month = date.getMonth() + 1;
                var day = date.getDate();
                if (month.toString().length == 1) {
                    month = "0" + month
                }
                if (day.toString().length == 1) {
                    day = "0" + day
                }
                var value = year + '-' + month + '-' + day;

                var hrs_get = date.getHours();
                var min_get = date.getMinutes();
                var ff = parseInt(Number(min_get)) + parseInt(Number(2));
                if (ff == 60) {
                    ff = "01";
                }
                var advancetime = value + ' ' + hrs_get + ':' + ff;
                var IVR_ID = $scope.rndtype.toString();
                var data =
                {
                    "ivr_id": IVR_ID,
                    "phonebook": ph_id.toString(),
                    "timezone": "Asia/Kolkata",
                    "priority": "8",
                    "order_throttling": "10",
                    "retry_duration": "15",
                    "start_time": advancetime.toString(),
                    "max_retry": "1",
                    "call_scheduling": "[1, 1, 1, 1, 1, 1, 1]",
                    "call_scheduling_start_time": "09:00",
                    "call_scheduling_stop_time": "21:00",
                    "k_number": "+911133035250",
                    "is_transactional": "False"
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json',
                        'x-api-key': 'lF4vZUSwA8Jab0ABWsITtxwM1ZwL6h2jZDdCTX30',
                        'Authorization': '964c52d4-016a-4d31-9b0a-86daaf43bbc5',
                        'cache-control': 'no-cache'
                    }
                };
                $http.post('https://kpi.knowlarity.com/Basic/v1/account/call/campaign', data, config)
                    .success(function (data, status, headers, config) {
                        $scope.PostDataResponse = data;

                        if (data.status_code == "1") {
                            swal("Call will initiated After next two minute !!");
                            $state.reload();
                        }
                        else {
                            swal('Please Try again Later !!!');
                        }
                    })
                    .error(function (data, status, header, config) {
                        $scope.ResponseDetails = "Data: " + data;
                    });
            }
        };

        $scope.gridOptions = {
            enableFiltering: true,
            paginationPageSizes: [10, 15, 20],
            paginationPageSize: 10,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'amsT_Id', displayName: 'AMST Id' },
                { name: 'studentName', displayName: 'Student Name' },
                { name: 'amsT_AdmNo', displayName: 'Admission No' },
                { name: 'amsT_emailId', displayName: 'E-Mail' },
                { name: 'amsT_MobileNo', displayName: 'MobileNo' }
            ]
        };
        $scope.Getstudentdetails_IVRS = function () {
            if ($scope.myFormnew1.$valid) {
                var data = {
                    "ASMAY_ID": $scope.ivrs_ASMAY_Id,
                    "ASMCL_ID": $scope.Ivrs_asmcL_Id,
                    "ASMS_ID": $scope.ivrs_asmS_Id
                };
                apiService.create("IVRSOBD/ivrgetstudetails", data).then(function (promise) {
                    $scope.currentPage = 1;
                    $scope.itemsPerPage = 10;
                    $scope.datagridviewIVR = false;
                    $scope.gridOptions.data = promise.maindata;
                    $scope.dataArray = promise.maindata;
                });
            }
        };
        $scope.maketestBulk_calls = function (objivr) {
            var mobi = "";

            if ($scope.rndtype === '1000033778') {
                angular.forEach($scope.dataArray, function (item) {
                    if (!!item.amsT_MobileNo) {
                        if (mobi === "") {
                            mobi = "+91" + item.amsT_MobileNo + "," + $scope.mi_id + "," + $scope.objivr.ttscontent +
                                ", " + $scope.schoolname + " , http://bdmobileapp.azurewebsites.net/api/IVRS/updatecredits";
                        }
                        else {
                            mobi += ";+91" + item.amsT_MobileNo + "," + $scope.mi_id + "," + $scope.objivr.ttscontent +
                                ", " + $scope.schoolname + " , http://bdmobileapp.azurewebsites.net/api/IVRS/updatecredits";
                        }
                    }
                });
            }
            else if ($scope.rndtype === '1000033928') {
                angular.forEach($scope.dataArray, function (item) {
                    if (!!item.amsT_MobileNo) {
                        if (mobi === "") {
                            mobi = "+91" + item.amsT_MobileNo + "," + $scope.mi_id + "," + $scope.objivr.urlimage +
                                ", " + $scope.schoolname + " , http://bdmobileapp.azurewebsites.net/api/IVRS/updatecredits";
                        }
                        else {
                            mobi += ";+91" + item.amsT_MobileNo + "," + $scope.mi_id + "," + $scope.objivr.urlimage +
                                ", " + $scope.schoolname + " , http://bdmobileapp.azurewebsites.net/api/IVRS/updatecredits";
                        }
                    }
                });
            }



            var data = {
                "phonebookname": "phonebook",
                "numbers": mobi
            };

            var config = {
                headers: {
                    'Content-Type': 'application/json charset=UTF-8',
                    'x-api-key': 'lF4vZUSwA8Jab0ABWsITtxwM1ZwL6h2jZDdCTX30',
                    'Authorization': '964c52d4-016a-4d31-9b0a-86daaf43bbc5',
                    'Access-Control-Allow-Origin': '*',
                    'Access-Control-Allow-Methods': 'POST',
                    'cache-control': 'no-cache'
                }
            };
            $http.post('https://kpi.knowlarity.com/Basic/v1/account/contacts/phonebook', data, config)
                .success(function (data, status, config) {


                    //$scope.PostDataResponse = data.id;
                    //$scope.scheduleCall(data.id);




                    //$scope.PostDataResponse = data.id;

                    $scope.getMessage(data.id);

                    //$scope.scheduleCall(data.id);

                })
                .error(function (data, status, config) {
                    $scope.PostDataResponse = "Data: " + data;
                });
        };


        $scope.UploadSoundfile = [];
        $scope.UploadSoundfile = function (input, document) {
            $scope.UploadSoundfile = input.files;
            if (input.files && input.files[0]) {
                if (input.files[0].type == "audio/mp3" && input.files[0].size <= 922097152) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blah')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadsoundfiel();
                }
                else if (input.files[0].type != "audio/mp3") {
                    swal("Please Upload the sound file");
                    return;
                } else if (input.files[0].size > 922097152) {
                    swal("sound size should be less than 922MB");
                    return;
                }
            }
        };
        function Uploadsoundfiel() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.UploadSoundfile.length; i++) {
                formData.append("File", $scope.UploadSoundfile[i]);
            }
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadsoundtrack", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    swal(d);
                    $scope.objivr.urlimage = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            Uploads1(miid);
        }


        $scope.changeradioregularnew = function () {
            $scope.count = 0;
            $scope.GetStudentDetails();


        };
    }
})();