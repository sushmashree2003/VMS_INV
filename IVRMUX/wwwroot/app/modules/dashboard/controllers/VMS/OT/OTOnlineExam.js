(function () {
    'use strict';
    angular.module('app').controller('OTOnlineExamController', OTOnlineExamController);

    OTOnlineExamController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter', '$sce', '$q', '$window', '$compile','$cookieStore']
    function OTOnlineExamController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter, $sce, $q, $window, $compile, $cookieStore) {
        $(document).on('keydown', function (e) {
            if (e.keyCode === 27) { // ESC
                if ($scope.textoverall === "End" && $scope.OTQPTYP_Id > 0) {
                    debugger;
                    $scope.LP_OnlineFinalDetails = [];
                    var optionid = 0;
                    var optioncorrectflag = false;
                    var answerdesc = "";
                    angular.forEach($scope.q_rpt1, function (d) {
                        answerdesc = "";
                        optionid = 0;
                        optioncorrectflag = false;
                        if (d.otmoeQ_SubjectiveFlg === false) {
                            if (d.QuizeQuastions !== undefined && d.QuizeQuastions !== null && d.QuizeQuastions !== "") {
                                optionid = parseInt(d.QuizeQuastions);
                                angular.forEach(d.qst_opt, function (dd) {
                                    if (parseInt(d.QuizeQuastions) === dd.OTMOEQOA_Id) {
                                        optioncorrectflag = dd.OTMOEQOA_AnswerFlag;
                                    }
                                });
                            }
                        } else {
                            if (d.answeredques !== undefined && d.answeredques !== null && d.answeredques !== "") {
                                answerdesc = d.answeredques;
                            }
                        }
                        $scope.LP_OnlineFinalDetails.push({
                            OTMOEQ_Id: d.otmoeQ_Id, QuizeQuastions: optionid, OTMOEQOA_AnswerFlag: optioncorrectflag,
                            answer: answerdesc, OTMOEQ_SubjectiveFlg: d.otmoeQ_SubjectiveFlg
                        });
                    });
              var data = {
                        "OTCANDEX_TotalTime": $scope.Upresultd,
                        "OTCANDEX_Id": $scope.OTCANDEX_Id,
                        "OTQPTYP_Id": $scope.OTQPTYP_Id,
                        "HRCD_Id": $scope.HRCD_Id,
                  "LP_OnlineFinalDetails": $scope.LP_OnlineFinalDetails,
                  "MI_Id": $scope.MI_Id,
                    };


                    $scope.timeforoverallquestion = 0;
                    apiService.create("OnlineTestCandidate/submitexam", data).then(function (promise) {
                        $('#QuizQuastions').modal('hide');
                        swal("Exam Is Submitted");
                    });

                    if (document.exitFullscreen) {
                        document.exitFullscreen();
                    } else if (document.mozCancelFullScreen) {
                        /* Firefox */
                        document.mozCancelFullScreen();
                    } else if (document.webkitExitFullscreen) {
                        /* Chrome, Safari and Opera */
                        document.webkitExitFullscreen();
                    } else if (document.msExitFullscreen) {
                        /* IE/Edge */
                        document.msExitFullscreen();
                    }
                    $state.reload();
                }
            }
        });
        $scope.rolef = '';
        function handleVisibilityChange() {
            debugger;
            if (document.hidden) {
                if ($scope.rolef ==='Online Exam Candidate') {
                    //$scope.alertdata();
                }
              
            }
        }
        document.addEventListener("visibilitychange", handleVisibilityChange, false);

        var alertcnt = 0;
        //$scope.alertdata = function () {
        //    alertcnt += 1;
        //    alert("You are Not allowed to open new tab while writing online exam!!!!!");
        //    if (alertcnt===3) {
        //       // alert("Exam Submited");
        //    }

        //}



        $scope.logout = function () {
            $cookieStore.remove('pagetitle');
            $cookieStore.put("pagetitle", "Home");
            $cookieStore.remove('IsLogged');
            apiService.getURI("Login/clearsession", 1).
                then(function (promise) {
                    //$state.go("login");
                    if (promise.subDomainNamelogout != null && promise.subDomainNamelogout != "") {
                        window.location.href = "http://localhost:57606/#/login/" + promise.subDomainNamelogout;
                    }
                    else {
                        window.location.href = "http://localhost:57606/#/login/";
                    }
                });
        };

        $scope.submitted = false;
        $scope.submitted3 = false;
        $scope.btnsave = false;
        var t;

        $scope.start = true;
        $scope.textht = '350px';
        $scope.startexam = false;

        $scope.teacherdocuupload = {};
        $scope.teacherdocuupload = [{ id: 'Teacher1' }];

        $scope.startTimer = function () {
            var countDownTime = 0;
            $scope.minutes = 0;
            $scope.seconds = 0;
            var result = "";
            clearInterval(countDownTime);
            document.getElementById("demo").innerHTML = "";
            countDownTime = parseInt($scope.timeforeachquestion);
            countDownTime = countDownTime * 60;
            var count = 0;
            function countDownTimer() {
                $scope.minutes = parseInt(countDownTime / 60) % 60;
                $scope.seconds = countDownTime % 60;
                result = ($scope.minutes < 10 ? "0" + $scope.minutes : $scope.minutes) + ":" + ($scope.seconds < 10 ? "0" + $scope.seconds : $scope.seconds);
                document.getElementById("demo").innerHTML = result;
                if (countDownTime !== 0 && count === 0) {
                    countDownTime = countDownTime - 1;
                }
                else if (count === 0) {
                    count += 1;
                    clearInterval(countDownTime);
                    document.getElementById("demo").innerHTML = "TIME EXPIRED";
                    $scope.text = "End";
                }
                setTimeout(function () { countDownTimer(); }, 1000);
            }
            countDownTimer();
        };







        // Down Timer from given time
        $scope.startTimeroverall = function () {
            countDownTimeoverall = 0;
            $scope.minutesoverall = 0;
            $scope.secondsoverall = 0;
            var result = "";
            document.getElementById("demooverall").innerHTML = "";

            var countDownTimeoverall = parseInt($scope.timeforoverallquestion);
            countDownTimeoverall = countDownTimeoverall * 60;
            var countoverall = 0;
            clearInterval(countDownTimeoverall);

            function countDownTimeroverall() {
                $scope.minutesoverall = parseInt(countDownTimeoverall / 60) % 60;
                $scope.secondsoverall = countDownTimeoverall % 60;
                result = ($scope.minutesoverall < 10 ? "0" + $scope.minutesoverall : $scope.minutesoverall) + ":" + ($scope.secondsoverall < 10 ? "0" + $scope.secondsoverall : $scope.secondsoverall);
                document.getElementById("demooverall").innerHTML = "Total Time Left :<b style='color:red'>" + result +"</b>";
                if (countDownTimeoverall !== 0 && countoverall === 0) {
                    countDownTimeoverall = countDownTimeoverall - 1;
                }
                else if (countoverall === 0) {
                    countoverall += 1;
                    clearInterval(countDownTimeoverall);
                    document.getElementById("demooverall").innerHTML = "TIME EXPIRED";
                    $scope.textoverall = "End";
                    $scope.Quit();
                }
                t = setTimeout(function () { countDownTimeroverall(); }, 1000);
            }
            countDownTimeroverall();
        };

        // Up Timer from 0
        $scope.UpstartTimeroverall = function () {
            UpcountDownTimeoverall = 0;
            $scope.Upminutesoverall = 0;
            $scope.Upsecondsoverall = 0;
            var Upresult = "";
            document.getElementById("Updemooverall").innerHTML = "";

            var UpcountDownTimeoverall = 0;
            UpcountDownTimeoverall = UpcountDownTimeoverall * 60;
            var Upcountoverall = 0;


            var minutesLabel = "";
            var secondsLabel = "";
            var totalSeconds = 0;

            $scope.Upresultd = "";
            document.getElementById("Updemooverall").innerHTML = "";


            function setTime() {
                ++totalSeconds;
                secondsLabel = pad(totalSeconds % 60);
                minutesLabel = pad(parseInt(totalSeconds / 60));
                $scope.Upresultd = minutesLabel + ":" + secondsLabel;

                if (minutesLabel === $scope.timeforoverallquestion) {
                    clearInterval(totalSeconds);
                }
                setTimeout(function () { setTime(); }, 1000);
            }
            setTime();
            function pad(val) {
                var valString = val + "";
                if (valString.length < 2) {
                    return "0" + valString;
                }
                else {
                    return valString;
                }
            }
        };

        $scope.examsubmittedlist = [];
        $scope.examnotsubmittedlist = [];
        $scope.examtodayslist = [];
        $scope.examnotstartedlist = [];
        $scope.html = "";
        //-------------------Load Data
        $scope.loaddata = function () {
            var pageid = 2;
            $scope.count = 0;
            $scope.subject = false;
            apiService.getURI("OnlineTestCandidate/loadExamdata", pageid).then(function (promise) {
                if (promise !== null) {

                    $scope.rolef = promise.rolename;
                    if (promise.message === null || promise.message === "") {
                        $scope.getsubjectdetails = promise.getsubjectdetails;
                        $scope.gettodaysexamdetails = promise.gettodaysexamdetails;
                        $scope.getallexamdetails = promise.getallexamdetails;
                        $scope.getexamcompleteddetails = promise.getexamcompleteddetails;
                        $scope.getexamsubmitteddetails = promise.getexamsubmitteddetails;

                        $scope.MI_Id = $scope.getallexamdetails[0].MI_Id;
                        var examdateflag = 0;

                        angular.forEach($scope.getallexamdetails, function (dd) {
                            examdateflag = 0;
                            angular.forEach($scope.gettodaysexamdetails, function (d) {
                                if (dd.otqptyP_Id === d.otqptyP_Id) {
                                    examdateflag = 1;
                                }
                            });
                            dd.examdateflag = examdateflag;
                        });

                        //angular.forEach($scope.getallexamdetails, function (dd) {
                        //    angular.forEach($scope.getexamsubmitteddetails, function (d) {
                        //        if (dd.lpmoeeX_Id === d.lpmoeeX_Id) {
                        //            dd.examdateflag = -2;
                        //            dd.viewmarkscount = d.viewmarkscount;
                        //            dd.viewmarkscountnew = d.viewmarkscountnew;
                        //        }
                        //    });
                        //});

                        //angular.forEach($scope.getallexamdetails, function (dd) {
                        //    angular.forEach($scope.getexamcompleteddetails, function (d) {
                        //        if (dd.lpmoeeX_Id === d.lpmoeeX_Id) {
                        //            dd.examdateflag = -1;
                        //        }
                        //    });
                        //});

                        $scope.examsubmittedlist = [];
                        $scope.examnotsubmittedlist = [];
                        $scope.examtodayslist = [];
                        $scope.examnotstartedlist = [];

                        angular.forEach($scope.getallexamdetails, function (dd) {


                            if (dd.OTCANDEX_Status === 'Completed') {
                                $scope.subject = true;
                            }
                            if (dd.examdateflag === 0) {
                                $scope.examnotstartedlist.push(dd);
                            }

                            else if (dd.examdateflag === 1) {
                                $scope.examtodayslist.push(dd);
                            }

                            else if (dd.examdateflag === -1) {
                                $scope.examnotsubmittedlist.push(dd);
                            }

                            else if (dd.examdateflag === -2) {
                                $scope.examsubmittedlist.push(dd);
                                console.log($scope.examsubmittedlist);
                            }
                        });

                        //if ($scope.getsubjectdetails === null || $scope.getsubjectdetails.length === 0) {
                        //    swal("Subject Are Not Mapped, Kindly Contact Administrator");
                        //}
                    } else {
                        swal("Candidate/Exam Details Not Found, Kindly Contact Administrator");
                    }
                }
            });
        };

        var elem = "";

        function onPaste(e) {
            kendo.htmlEncode(e.html);
        }

        function editor_change() {
            $scope.datass = this.value();
            document.getElementById("deff").value = $scope.datass;
        }

        $scope.onselectsubject = function () {
            var data = {
                "ISMS_Id": $scope.ISMS_Id
            };
            apiService.create("LP_OnlineStudentExam/onselectsubject", data).then(function (promise) {
                $scope.getexamlist = promise.getexamlist;
                if ($scope.getexamlist === null || $scope.getexamlist.length === 0) {
                    swal("No Exam Is Mapped For Selected Subject, Kindly Contact Administrator");
                }
            });
        };

        $scope.Submit1 = function () {
            $scope.subject = true;

        };

        $scope.UploadFiles = function (objupload, index) {
            $scope.QSNID = objupload.otmoeQ_Id
            $scope.optionsindex = index;
            if (objupload.tempoptionsfiles !== undefined && objupload.tempoptionsfiles !== null && objupload.tempoptionsfiles.length > 0) {
                $scope.teacherdocuuploadopts = objupload.tempoptionsfiles;

                angular.forEach($scope.teacherdocuuploadopts, function (d) {
                    var img = d.otcandexaS_AnswerSheetPath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    d.filetype = lastelement;
                    console.log("data.filetype : " + d.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        d.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + d.otcandexaS_AnswerSheetPath;
                    }
                });
            } else {
                $scope.teacherdocuuploadopts = [{ id: 'Teacheropts1' }];
            }
            $('#optsfilesupload').modal('show');
        };

        $scope.OnClickOptsFilesUpload = function () {
            $scope.tempoptsfiles = [];
            angular.forEach($scope.teacherdocuuploadopts, function (dd) {
                var id = 0;
                if (dd.otcandexaS_AnswerSheetPath !== undefined && dd.otcandexaS_AnswerSheetPath !== null && dd.otcandexaS_AnswerSheetPath !== "") {
                    if (dd.otcandexaS_Id !== undefined && dd.otcandexaS_Id !== null) {
                        id = dd.otcandexaS_Id;
                    }
                    $scope.tempoptsfiles.push({
                        otcandexaS_AnswerSheetPath: dd.otcandexaS_AnswerSheetPath, otcandexaS_AnswerSheetFile: dd.otcandexaS_AnswerSheetFile,
                        otcandexaS_Id: id
                    });
                }
            });

            if ($scope.tempoptsfiles.length > 0) {
                angular.forEach($scope.q_rpt1, function (dd, index) {
                    if (dd.otmoeQ_Id == $scope.QSNID) {
                        dd.tempoptionsfiles = $scope.tempoptsfiles;
                    }
                });
            }
        };
        $scope.uploadtecheroptsdocuments1 = [];
        $scope.uploadtecheroptsdocuments = function (input, document) {

            $scope.uploadtecheroptsdocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg")  // 2097152 bytes = 2MB 
                {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload  Pdf, Doc, Image Files Only");
                }
            }
        };
        function UploaddianPhotoopts(data) {
            console.log("Teacher Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadtecheroptsdocuments1.length; i++) {
                formData.append("File", $scope.uploadtecheroptsdocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/lessonplannerdoc", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.otcandexaS_AnswerSheetPath = d;
                    data.otcandexaS_AnswerSheetFile = $scope.filename;
                    $('#').attr('src', data.otcandexaS_AnswerSheetPath);
                    var img = data.otcandexaS_AnswerSheetPath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.otcandexaS_AnswerSheetPath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.addNewsiblingguardopts = function () {
            var newItemNo = $scope.teacherdocuuploadopts.length + 1;

            if (newItemNo <= 10) {
                $scope.teacherdocuuploadopts.push({ 'id': 'Teacheropts1' + newItemNo });
            }
            console.log($scope.teacherdocuuploadopts);
        };

        $scope.removeNewsiblingguardopts = function (index) {
            var newItemNo = $scope.teacherdocuuploadopts.length - 1;
            $scope.teacherdocuuploadopts.splice(index, 1);

            if ($scope.teacherdocuuploadopts.length === 0) {
                //data
            }
        };
        // Click Function For To Start Exam
        $scope.Submit = function (details) {
            $('#textareaid').html('ddd'); 
            //if ($scope.myForm.$valid) {
            debugger;
            $scope.HRCD_Id = details.HRCD_Id;
            $scope.HRMP_Id = details.hrmP_Id;
            $scope.OTQPTYP_Id = details.otqptyP_Id;

            $scope.examdetailsstudent = "";
            $scope.examdetailsstudent = "Exam : " + details.otqptyP_QuestionPaperName;

            var data = {
                "OTQPTYP_Id": details.otqptyP_Id,
                "HRMP_Id": details.hrmP_Id,
                "HRCD_Id": details.HRCD_Id,
                "MI_Id": $scope.MI_Id,
            };
            apiService.create("OnlineTestCandidate/getQuestion", data).then(function (promise) {
                $scope.abcde = 'ddddd';

             
                $scope.getexamdetails = promise.getexamdetails;

                //if ($scope.getexamdetails !== null && $scope.getexamdetails.length > 0) {


                    //if ($scope.getexamdetails[0].lpmoeeX_UploadExamPaperFlg === false) {
                        $scope.start = false;
                        $scope.subject = false;

                        if (promise.getexamquestionlist !== null && promise.getexamquestionlist.length > 0) {

                            if (promise.message === "Time Crossed") {
                                //swal("You Can Not Take Exam Now Already Time Crossed To Start Exam");
                                //return;
                            }

                            elem = document.documentElement;
                            var methodToBeInvoked = elem.requestFullscreen || elem.webkitRequestFullScreen || elem['mozRequestFullscreen']
                                || elem['msRequestFullscreen'];
                            if (methodToBeInvoked) methodToBeInvoked.call(elem);

                            $scope.getQuestion = promise.getexamquestionlist;
                            $scope.getconnfig = promise.getconnfig;

                            //$scope.timeforeachquestion = $scope.getconnfig[0].lpmoeS_EachQnsDuration;
                            $scope.timeforoverallquestion = details.OTQPTYP_MaxDuration;

                            $scope.test = $scope.getQuestion;
                            $scope.count = 0;
                            $scope.q_list1 = [];
                            $scope.temp_recp_list1 = [];
                            $scope.q_rpt1 = $scope.getQuestion;

                            $scope.getquestionoptionlist = promise.getquestionoptionlist;
                            $scope.getQuestiondocuments = promise.getquestiondoclist;
                            $scope.getoptionwisefiles = promise.getoptionwisefiles;

                            angular.forEach($scope.q_rpt1, function (dd, index) {

                                //attaching question filed
                                $scope.qst_opttemp = [];
                                $scope.tempdoc = [];
                                angular.forEach($scope.getQuestiondocuments, function (o) {
                                    if (o.otmoeQ_Id === dd.otmoeQ_Id) {


                                        $scope.tempdoc.push(o);
                                    }
                                });

                                angular.forEach($scope.tempdoc, function (d) {
                                    var img = d.otmoeqF_FilePath;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];
                                    d.filetype = lastelement;
                                    
                                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                        d.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + d.otmoeqF_FilePath;
                                    }
                                });

                                dd.files = $scope.tempdoc;


                                if (index === 0) {
                                    debugger;
                                   
                                    dd.class = "current";
                                }

                                $scope.qst_opttemp = [];
                                angular.forEach($scope.getquestionoptionlist, function (d) {
                                    if (dd.otmoeQ_Id === d.otmoeQ_Id) {

                                        $scope.tempdocoptions = [];
                                        angular.forEach($scope.getoptionwisefiles, function (o) {
                                            if (o.otmoeqoA_Id === d.otmoeqoA_Id) {
                                                $scope.tempdocoptions.push(o);
                                            }
                                        });

                                        angular.forEach($scope.tempdocoptions, function (mm) {
                                            var img = mm.otmoeqoaF_FilePath;
                                            var imagarr = img.split('.');
                                            var lastelement = imagarr[imagarr.length - 1];
                                            mm.filetype = lastelement;

                                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                                mm.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + mm.otmoeqoaF_FilePath;
                                            }
                                        });

                                        $scope.qst_opttemp.push({
                                            OTMOEQOA_Id: d.otmoeqoA_Id, OTMOEQOA_Option: d.otmoeqoA_Option, OTMOEQ_Id: d.otmoeQ_Id,
                                            OTMOEQOA_OptionCode: d.otmoeqoA_OptionCode, OTMOEQOA_AnswerFlag: d.otmoeqoA_AnswerFlag, opt_files: $scope.tempdocoptions
                                        });
                                    }
                                });
                                dd.qst_opt = $scope.qst_opttemp; 
                            });

                            console.log($scope.q_rpt1);
                            $('#content1').on('change keyup keydown paste cut', 'textarea', function () {
                                debugger;
                                $(this).height(0).height(this.scrollHeight);
                                var aaa = this.scrollHeight;
                                $scope.textht = aaa + 2;
                                $scope.textht = $scope.textht + 'px';
                            }).find('textarea').change();

                          //  $scope.tempdoc = [];




                            //$scope.getQuestiondocuments = promise.getquestiondoclist;

                            //angular.forEach($scope.q_rpt1, function (dd) {
                            //    $scope.tempdoc = [];
                            //    angular.forEach($scope.getQuestiondocuments, function (d) {
                            //        if (dd.lpmoeQ_Id === d.lpmoeQ_Id) {
                            //            $scope.tempdoc.push(d);
                            //        }
                            //    });
                            //    dd.files = $scope.tempdoc;
                            //});

                            console.log($scope.q_rpt1);
                            $scope.html = "";
                            $scope.startTimeroverall();
                            $scope.UpstartTimeroverall();
                            $scope.startexam = true;
                            //$('#QuizQuastions').modal('show');
                        }
                        else {
                            swal("No Questions Available for the Selected Subject");
                        }
                   // }

                    //else {
                    //    $scope.LPMOEEX_AnswerPapeFileNameView = $scope.getexamdetails[0].lpmoeeX_AnswerPapeFileName;
                    //    $scope.LPMOEEX_AnswerSheetView = $scope.getexamdetails[0].lpmoeeX_AnswerSheet;
                    //    $scope.LPMOEEX_QuestionPapeFileNameView = $scope.getexamdetails[0].lpmoeeX_QuestionPapeFileName;
                    //    $scope.LPMOEEX_QuestionPaperView = $scope.getexamdetails[0].lpmoeeX_QuestionPaper;

                    //    if ($scope.LPMOEEX_QuestionPaperView !== null && $scope.LPMOEEX_QuestionPaperView !== "") {
                    //        var img = $scope.LPMOEEX_QuestionPaperView;
                    //        var imagarr = img.split('.');
                    //        var lastelement = imagarr[imagarr.length - 1];
                    //        $scope.quesfiletypeview = lastelement;
                    //        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                    //            $scope.quesdocument_Pathnewview = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.LPMOEEX_QuestionPaperView;
                    //        }
                    //    }

                    //    //if ($scope.LPMOEEX_AnswerSheetView !== null && $scope.LPMOEEX_AnswerSheetView !== "") {
                    //    //    var img1 = $scope.LPMOEEX_AnswerSheetView;
                    //    //    var imagarr1 = img1.split('.');
                    //    //    var lastelement1 = imagarr1[imagarr.length - 1];
                    //    //    $scope.ansfiletypeview = lastelement1;
                    //    //    if (lastelement1 === 'doc' || lastelement1 === 'docx' || lastelement1 === 'ppt' || lastelement1 === 'pptx' || lastelement1 === 'xlsx'
                    //    //        || lastelement1 === 'xls') {
                    //    //        $scope.ansdocument_Pathnewview = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.LPMOEEX_AnswerSheetView;
                    //    //    }
                    //    //}

                    //    $scope.startTimeroverall();
                    //    $scope.UpstartTimeroverall();
                    //    $('#myModalView').modal('show');
                    //}
                //}
            });
        };

        $scope.nextqst = function () {
            $scope.submitted3 = false;
            $scope.count += 1;
            var pageid = 2;
            apiService.getURI("LP_OnlineStudentExam/savedanswers", pageid).then(function (promise) {
                $scope.savedanswer = promise.savedanswer;
                var PAMOEQ_Id = $scope.q_list1[$scope.count].PAMOEQ_Id;
                angular.forEach($scope.savedanswer, function (itm1) {
                    if (itm1.pamoeQ_Id === PAMOEQ_Id) {
                        // $scope.QuizeQuastions = itm1.LMSMOEQOA_Id;
                        $scope.q_list1[$scope.count].QuizeQuastions = itm1.pamoeqoA_Id;
                    }
                });
                //$scope.startTimer();
            });
        };

        // Click Function For Previous 
        $scope.prevqst = function () {

            if ($scope.count === $scope.q_rpt1.length) {

                //$scope.q_rpt1[$scope.count].class = "skipped";
                $scope.submitted3 = false;
                $scope.count -= 1;
                $scope.q_list_answrd = $scope.q_rpt1[$scope.count];
                var out = $scope.q_list_answrd;
                $scope.q_rpt1[$scope.count].class = "skipped";
                if (out.QuizeQuastions !== undefined && out.QuizeQuastions !== null) {
                    $scope.q_rpt1[$scope.count].class = "answered";
                } else if (out.answeredques !== undefined && out.answeredques !== null) {
                    $scope.q_rpt1[$scope.count].class = "answered";
                } else {
                    $scope.q_rpt1[$scope.count].class = "correct";
                }
                $scope.btnsave = false;
                //$scope.count -= 1;
                var pageid = 2;
                var data = {
                    "LPMOEEX_Id": $scope.LPMOEEX_Id,
                    "ISMS_Id": $scope.ISMS_Id
                };
            }
            else {
                $scope.q_rpt1[$scope.count].class = "skipped";
                $scope.submitted3 = false;
                $scope.count -= 1;
                $scope.q_list_answrd = $scope.q_rpt1[$scope.count];
                var out = $scope.q_list_answrd;
                $scope.q_rpt1[$scope.count].class = "skipped";
                if (out.QuizeQuastions !== undefined && out.QuizeQuastions !== null) {
                    $scope.q_rpt1[$scope.count].class = "answered";
                } else if (out.answeredques !== undefined && out.answeredques !== null) {
                    $scope.q_rpt1[$scope.count].class = "answered";
                } else {
                    $scope.q_rpt1[$scope.count].class = "correct";
                }
                $scope.btnsave = false;
                //$scope.count -= 1;
              
            }

            

            //apiService.create("LP_OnlineStudentExam/savedanswers", data).then(function (promise) {
            //    $scope.btnsave = false;
            //    $scope.getsavedanswer = promise.getsavedanswer;
            //    var lpmoeQ_Id = $scope.q_rpt1[$scope.count].lpmoeQ_Id;
            //    var subjectiveflag = $scope.q_rpt1[$scope.count].lpmoeQ_SubjectiveFlg;
            //    if (subjectiveflag === false) {
            //        angular.forEach($scope.getsavedanswer, function (itm1) {
            //            if (itm1.lpmoeQ_Id === lpmoeQ_Id) {
            //                $scope.q_rpt1[$scope.count].QuizeQuastions = itm1.lpmoeqoA_Id;
            //                $scope.q_rpt1[$scope.count].class = "correct";
            //            }
            //        });
            //    } else {
            //        $scope.q_rpt1[$scope.count].answeredques = itm1.lpmoeqoA_Id;
            //        $scope.q_rpt1[$scope.count].class = "correct";
            //    }
            //});
        };

        //
        $scope.showQuestion = function (objin, obj) {
            debugger;
            $('#content1').on('change keyup keydown paste cut', 'textarea', function () {
                debugger;
                $(this).height(0).height(this.scrollHeight);
                var aaa = this.scrollHeight;
                $scope.textht = aaa + 10;
                $scope.textht = $scope.textht + 'px';
            }).find('textarea').change();
           // $scope.count = 0;
            $scope.count = objin;

            angular.forEach($scope.q_rpt1, function (gg) {
                if (gg.class === 'current') {
                gg.class = '';
                }
               
            });


            $scope.q_rpt1[$scope.count].class = "current";

            $('#content1').on('change keyup keydown paste cut', 'textarea', function () {
                debugger;
                $(this).height(0).height(this.scrollHeight);
                var aaa = this.scrollHeight;
                $scope.textht = aaa + 2;
                $scope.textht = $scope.textht + 'px';
            }).find('textarea').change();

           
        };

        // Click Function For Save And Next
        $scope.Saveanswer = function (objuser) {
            debugger;

          

            
            if (objuser.$valid) {
                $scope.q_list_answrd = $scope.q_rpt1[$scope.count];
                console.log($scope.q_list_answrd.tempoptionsfiles);
                var LMSMOEQOA_Id = 0;
                var QuizeQuastions = 0;
                var PAMOEQ_Id = 0;
                var q_name = '';
                $scope.term_list = [];
                var out = $scope.q_list_answrd;
                var data = "";
                $scope.q_rpt1[$scope.count].class = "answerd";
                data = {
                    "OTCANDEX_TotalTime": $scope.Upresultd,
                    "HRMP_Id": $scope.HRMP_Id,
                    "HRCD_Id": $scope.HRCD_Id,
                    "OTQPTYP_Id": $scope.OTQPTYP_Id,
                    "OTCANDEX_Id": $scope.OTCANDEX_Id,
                    "OTMOEQ_SubjectiveFlg": out.otmoeQ_SubjectiveFlg,
                    "OTMOEQ_Id": out.otmoeQ_Id,
                    "MI_Id": $scope.MI_Id,
                };

                if (out.otmoeQ_SubjectiveFlg === false) {

                    angular.forEach(out.qst_opt, function (dd) {
                        if (parseInt(out.QuizeQuastions) === dd.OTMOEQOA_Id) {
                            out.OTMOEQOA_AnswerFlag = dd.OTMOEQOA_AnswerFlag;
                        }
                    });

                    $scope.term_list.push({
                        OTMOEQ_Id: out.otmoeQ_Id, OTMOEQOA_Id: out.QuizeQuastions, QuizeQuastions: out.QuizeQuastions,
                        OTMOEQ_Question: out.otmoeQ_Question, OTMOEQOA_AnswerFlag: out.OTMOEQOA_AnswerFlag
                    });
                    data.saveanswerlsttemp = $scope.term_list;
                }

                else {
                    data.OTCANDEXSANS_Answer = out.answeredques;
                }

                if ($scope.q_list_answrd.tempoptionsfiles!=null && $scope.q_list_answrd.tempoptionsfiles.length>0) {
                    data.tempoptionsfiles = $scope.q_list_answrd.tempoptionsfiles;
                }


                apiService.create("OnlineTestCandidate/Saveanswer", data).then(function (promise) {
                    $scope.getsavedanswer = promise.getsavedanswer;
                    $scope.OTCANDEX_Id = promise.otcandeX_Id;
                    $scope.count += 1;                    
                    if ($scope.count === $scope.q_rpt1.length) {
                        $scope.btnsave = true;
                    } else {
                        $scope.q_rpt1[$scope.count].class = "current";
                        $scope.btnsave = false;
                    }

                    if ($scope.count !== $scope.q_rpt1.length) {
                        var otmoeQ_Id = $scope.q_rpt1[$scope.count].otmoeQ_Id;
                        var subjectiveflag = $scope.q_rpt1[$scope.count].otmoeQ_SubjectiveFlg;

                        angular.forEach($scope.getsavedanswer, function (itm1) {
                            if (itm1.otmoeQ_Id === otmoeQ_Id) {
                                if (subjectiveflag === false) {
                                    $scope.q_rpt1[$scope.count].QuizeQuastions = itm1.otmoeqoA_Id;
                                } else {
                                    $scope.q_rpt1[$scope.count].answeredques = itm1.otcandexsanS_Answer;
                                }
                            }
                        });
                    }
                    //alert($scope.btnsave);
                    $scope.submitted3 = false;
                });
            } else {
                $scope.submitted3 = true;
            }
        };

        $scope.uploadtecherdocuments1 = [];

        $scope.uploadtecherdocuments = function (input, document) {

            $scope.uploadtecherdocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg")  // 2097152 bytes = 2MB 
                {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload  Pdf, Doc, Image Files Only");
                }
            }
        };

        function UploaddianPhoto(data) {
            console.log("Teacher Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadtecherdocuments1.length; i++) {
                formData.append("File", $scope.uploadtecherdocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/lessonplannerdoc", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.otmoeqF_FilePath = d;
                    data.otmoeqF_FileName = $scope.filename;
                    $('#').attr('src', data.otmoeqF_FilePath);
                    var img = data.otmoeqF_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.otmoeqF_FilePath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }





        // Click Function For To Submit Exam
        $scope.Quit = function () {
            var data = "";

            $scope.LP_OnlineFinalDetails = [];
            var optionid = 0;
            var optioncorrectflag = false;
            var answerdesc = "";
            angular.forEach($scope.q_rpt1, function (d) {
                answerdesc = "";
                optionid = 0;
                optioncorrectflag = false;
                if (d.otmoeQ_SubjectiveFlg === false) {
                    if (d.QuizeQuastions !== undefined && d.QuizeQuastions !== null && d.QuizeQuastions !== "") {
                        optionid = parseInt(d.QuizeQuastions);
                        angular.forEach(d.qst_opt, function (dd) {
                            if (parseInt(d.QuizeQuastions) === dd.OTMOEQOA_Id) {
                                optioncorrectflag = dd.OTMOEQOA_AnswerFlag;
                            }
                        });
                    }
                } else {
                    if (d.answeredques !== undefined && d.answeredques !== null && d.answeredques !== "") {
                        answerdesc = d.answeredques;
                    }
                }

                



                $scope.LP_OnlineFinalDetails.push({
                    OTMOEQ_Id: d.otmoeQ_Id, QuizeQuastions: optionid, OTMOEQOA_AnswerFlag: optioncorrectflag,
                    answer: answerdesc, OTMOEQ_SubjectiveFlg: d.otmoeQ_SubjectiveFlg, tempoptionsfiles: d.tempoptionsfiles
                });
            });


            debugger;

            if ($scope.textoverall === "End") {
                data = {
                    "OTCANDEX_TotalTime": $scope.Upresultd,
                    "OTCANDEX_Id": $scope.OTCANDEX_Id,
                    "OTQPTYP_Id": $scope.OTQPTYP_Id,
                    "HRCD_Id": $scope.HRCD_Id,
                    "LP_OnlineFinalDetails": $scope.LP_OnlineFinalDetails,
                    "MI_Id": $scope.MI_Id,
                };
                swal({
                    title: "Are you sure?",
                    text: "Time Up !!!",
                    type: "warning",
                    showCancelButton: false,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            clearTimeout(t);
                            $scope.timeforoverallquestion = 0;
                            apiService.create("OnlineTestCandidate/submitexam", data).then(function (promise) {
                                $('#QuizQuastions').modal('hide');
                                swal("Exam Is Submitted");
                            });
                            if (document.exitFullscreen) {
                                document.exitFullscreen();
                            } else if (document.mozCancelFullScreen) {
                                /* Firefox */
                                document.mozCancelFullScreen();
                            } else if (document.webkitExitFullscreen) {
                                /* Chrome, Safari and Opera */
                                document.webkitExitFullscreen();
                            } else if (document.msExitFullscreen) {
                                /* IE/Edge */
                                document.msExitFullscreen();
                            }
                            $state.reload();
                        }
                        else {
                            swal("Exam Is Submitted Cancel");
                        }
                    });
            }
            else {
                var confirmmgs = "";
                data = {
                    "OTCANDEX_TotalTime": $scope.Upresultd,
                    "OTCANDEX_Id": $scope.OTCANDEX_Id,
                    "OTQPTYP_Id": $scope.OTQPTYP_Id,
                    "HRCD_Id": $scope.HRCD_Id,
                    "LP_OnlineFinalDetails": $scope.LP_OnlineFinalDetails,
                    "MI_Id": $scope.MI_Id,
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                swal({
                    title: "Are you sure?",
                    text: "Do You Want To Quit the Test, If yes only the Saved Answers will be Considered For Test?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            clearTimeout(t);
                            $scope.timeforoverallquestion = 0;

                            apiService.create("OnlineTestCandidate/submitexam", data).then(function (promise) {
                                if (promise.result.length > 0) {
                                    $('#QuizQuastions').modal('hide');
                                    swal("Exam Is Submitted");
                                }
                                else {
                                    $('#QuizQuastions').modal('hide');
                                    swal("Exam Is Submitted");
                                }

                                if (document.exitFullscreen) {
                                    document.exitFullscreen();
                                } else if (document.mozCancelFullScreen) {
                                    /* Firefox */
                                    document.mozCancelFullScreen();
                                } else if (document.webkitExitFullscreen) {
                                    /* Chrome, Safari and Opera */
                                    document.webkitExitFullscreen();
                                } else if (this.document.msExitFullscreen) {
                                    /* IE/Edge */
                                    document.msExitFullscreen();
                                }
                                $state.reload();
                            });
                        }
                        else {
                            swal("Exam Submission Cancelled");
                        }
                    });
            }
        };

        $scope.Ok = function () {
            $('#finalwindow').modal('hide');
        };

        $scope.OkViewMarks = function () {
            $('#viewmarks').modal('hide');
        };

        // View Marks
        $scope.GetViewMarks = function (detailss) {

            var data = {
                "LPMOEEX_Id": detailss.lpmoeeX_Id,
                "ISMS_Id": detailss.ismS_Id
            };

            apiService.create("LP_OnlineStudentExam/GetViewMarks", data).then(function (promise) {
                if (promise !== null) {

                    if (promise.getmarksdetails !== null && promise.getmarksdetails.length > 0) {

                        $scope.getmarksdetails = promise.getmarksdetails;

                        $scope.examname = detailss.lpmoeeX_ExamName;
                        $scope.subjectname = detailss.ismS_SubjectName;
                        $scope.maxmarks = $scope.getmarksdetails[0].lpstueX_TotalMaxMarks;
                        $scope.marksobtained = $scope.getmarksdetails[0].lpstueX_TotalMarks;
                        $scope.examdate = $scope.getmarksdetails[0].lpstueX_Date;
                        $scope.durationtake = $scope.getmarksdetails[0].otcandeX_TotalTime;
                        $scope.questionattempt = $scope.getmarksdetails[0].lpstueX_TotalQnsAnswered;
                        $scope.questioncorrect = $scope.getmarksdetails[0].lpstueX_TotalCorrectAns;
                        $scope.maxmarkspercentage = $scope.getmarksdetails[0].lpstueX_Percentage;

                        $scope.answersheetfilename = $scope.getmarksdetails[0].lpstueX_AnswerSheetFile;
                        $scope.answesheetfilepath = $scope.getmarksdetails[0].lpstueX_AnswerSheetPath;

                        $scope.correctedanswersheetfilename = $scope.getmarksdetails[0].lpstueX_CorrectedAnswerSheetFile;
                        $scope.correctedanswesheetfilepath = $scope.getmarksdetails[0].lpstueX_CorrectedAnswerSheetPath;

                        if ($scope.answesheetfilepath !== null && $scope.answesheetfilepath !== "") {
                            var img = $scope.answesheetfilepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            $scope.answerfiletypeview = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                $scope.answerdocument_Pathnewview = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.answesheetfilepath;
                            }
                        }

                        if ($scope.correctedanswesheetfilepath !== null && $scope.correctedanswesheetfilepath !== "") {
                            var img1 = $scope.correctedanswesheetfilepath;
                            var imagarr1 = img1.split('.');
                            var lastelement1 = imagarr1[imagarr1.length - 1];
                            $scope.correctedanswerfiletypeview = lastelement1;
                            if (lastelement1 === 'doc' || lastelement1 === 'docx' || lastelement1 === 'ppt' || lastelement1 === 'pptx' || lastelement1 === 'xlsx' || lastelement1 === 'xls') {
                                $scope.correctedanswerdocument_Pathnewview = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.correctedanswesheetfilepath;
                            }
                        }

                        $scope.getexamdetails = promise.getexamdetails;
                        $scope.lpmoeeX_UploadExamPaperFlg = $scope.getexamdetails[0].lpmoeeX_UploadExamPaperFlg;
                        if ($scope.lpmoeeX_UploadExamPaperFlg === false) {
                            $scope.getallmarksdetails = promise.getallmarksdetails;
                        }
                        $('#viewmarks').modal('show');
                    } else {
                        swal("No Records Found");
                    }
                }
            });
        };

        $scope.cancel = function () {
            clearTimeout(t);
            $scope.timeforoverallquestion = 0;
            $state.reload();
        };

        $scope.cancel1 = function () {
            clearTimeout(t);
            $scope.timeforoverallquestion = 0;
            $state.reload();
        };

        $scope.interacted3 = function (field) {
            return $scope.submitted3;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.showmothersign = function (path) {
            $scope.imagepreview = path;
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });

            var docElm = document.getElementById('myModalimg');

           
                //if (docElm.requestFullscreen) {
                //    docElm.requestFullscreen();
                //} else if (docElm.mozRequestFullScreen) {
                //    docElm.mozRequestFullScreen();
                //} else if (docElm.webkitRequestFullScreen) {
                //    docElm.webkitRequestFullScreen();
                //} else if (docElm.msRequestFullscreen) {
                //    docElm.msRequestFullscreen();
                //}
         
                
           

            $('#myModalimg').modal('show');


            //if (docElm.exitFullscreen) {
            //    docElm.exitFullscreen();
            //} else if (docElm.webkitExitFullscreen) {
            //    docElm.webkitExitFullscreen();
            //} else if (docElm.mozCancelFullScreen) {
            //    docElm.mozCancelFullScreen();
            //} else if (docElm.msExitFullscreen) {
            //    docElm.msExitFullscreen();
            //}
        };

        $scope.pauseOrPlay = function (ele) {
            $('#popup15').modal({ show: false }).on('hidden.bs.modal', function () {
                $(this).find('video')[0].pause();
            });
        };

        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.lpmoeeX_QuestionPaperView;
            $scope.videdfd = data.lpmoeeX_QuestionPaperView;
            $scope.movie = { src: data.lpmoeeX_QuestionPaperView };
            $scope.movie1 = { src: data.lpmoeeX_QuestionPaperView };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.lpmoeeX_QuestionPaperView });
            console.log($scope.view_videos);
        };

        $scope.onview = function (filepath, filename) {
            //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            document.getElementById("pdfviewdd").innerHTML = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var htmlElements = '<embed id="pdfview" src="' + $scope.content + '"  style="width: 100%;" height="1000" type="application/pdf" />';
                    document.getElementById("pdfviewdd").innerHTML = htmlElements;
                    $('#showpdf').modal('show');
                });
        };

        var imagedownload = "";
        $scope.downloaddirectimage = function (data, idd) {

            var studentreg = idd;

            $scope.imagedownload = data;
            imagedownload = data;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg
                    })[0].click();
                });
        };

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };

        //Timmer 
        $scope.clicktimmer = function () {
            $scope.timeforoverallquestion = 1;
            $scope.startTimeroverall();
        };

        $scope.clicktimmerstop = function () {
            clearTimeout(t);
            $scope.timeforoverallquestion = 0;
        };

        $scope.stoptimmer = function (t1) {
            swal("OK", t1);
        };

        $scope.uploadtecherdocuments1 = [];

        $scope.uploadtecherdocuments = function (input, document) {

            $scope.uploadtecherdocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg")  // 2097152 bytes = 2MB 
                {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                    UploaddianPhoto(document);
                }
                else {
                    swal("Upload  Pdf, Doc, Image Files Only");
                }
            }
        };

        function UploaddianPhoto(data) {
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadtecherdocuments1.length; i++) {
                formData.append("File", $scope.uploadtecherdocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/lessonplannerdoc", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    $scope.LPSTUEX_AnswerSheetPath = d;
                    $scope.LPSTUEX_AnswerSheetFile = $scope.filename;
                    $('#').attr('src', $scope.LPSTUEX_AnswerSheetPath);
                    var img = $scope.LPSTUEX_AnswerSheetPath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    $scope.quesfiletype = lastelement;
                    console.log("data.filetype : " + $scope.quesfiletype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        $scope.quesdocument_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.LPSTUEX_AnswerSheetPath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.OnClickHomePage = function () {
            var HostName = location.host;
            $window.location.href = 'http://' + HostName + '/#/app/StudentDashboard';
        };

        $scope.SaveAnswerSheet = function () {
            var confirmmgs = "";
            if ($scope.LPSTUEX_AnswerSheetPath !== undefined && $scope.LPSTUEX_AnswerSheetPath !== null && $scope.LPSTUEX_AnswerSheetPath !== "") {
                var data = {
                    "OTCANDEX_TotalTime": $scope.Upresultd,
                    "LPMOEEX_Id": $scope.LPMOEEX_Id,
                    "ISMS_Id": $scope.ISMS_Id,
                    "LPSTUEX_AnswerSheetPath": $scope.LPSTUEX_AnswerSheetPath,
                    "LPSTUEX_AnswerSheetFile": $scope.LPSTUEX_AnswerSheetFile
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                swal({
                    title: "Are you sure?",
                    text: "Do You Want To Submit the Test?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            clearTimeout(t);
                            $scope.timeforoverallquestion = 0;

                            apiService.create("LP_OnlineStudentExam/SaveAnswerSheet", data).then(function (promise) {

                                if (promise.message === "Saved") {
                                    swal("Exam Is Submitted");
                                } else if (promise.message === "Updated") {
                                    swal("Exam Is Submitted");
                                } else {
                                    swal("Failed To Submit");
                                }
                                $('#myModalView').modal('hide');

                                $state.reload();
                            });
                        }
                        else {
                            swal("Exam Is Submittion Cancelled");
                        }
                    });
            } else {
                swal("Upload File To Save The Exam");
            }
        };
    }

    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });

    angular.module('app').directive('txtArea', function () {
        return {
            restrict: 'AE',
            replace: 'true',
            scope: { data: '=', model: '=ngModel' },
            template: "<textarea></textarea>",
            link: function (scope, elem) {
                scope.$watch('data', function (newVal) {
                    if (newVal) {
                        debugger;
                        scope.model += newVal[0];
                        var editor = $("#editor").data("kendoEditor");

                        editor.value(scope.model);

                    }
                });
            }
        };
    });
})();