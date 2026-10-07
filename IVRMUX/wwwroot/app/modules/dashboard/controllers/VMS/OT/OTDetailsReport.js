(function () {
    'use strict';
    angular.module('app').controller('OTDetailsReportController', OTDetailsReportController)

    OTDetailsReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter', '$sce', '$q']
    function OTDetailsReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter, $sce, $q) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.obj = {};
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');

        var copty;

        $scope.maxdate = new Date();

        $scope.itemsPerPage = paginationformasters;
        $scope.searchValue = '';
        if ($scope.itemsPerPage === undefined || $scope.itemsPerPage === null) {
            $scope.itemsPerPage = 15;
        }
        $scope.currentPage = 1;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        $scope.reportbtn = true;

        $scope.loaddata = function () {
            var pageid = 4;
            apiService.getURI("OnlineTestCandidate/loadDetailsRpt", pageid).then(function (promise) {
                $scope.milist = promise.milist;
            });
        };

        $scope.oncmpchange = function () {


            var data = {
                "MI_Id": $scope.MI_Id,

            };
            apiService.create("OnlineTestCandidate/oncmpchange", data).then(function (promise) {
                $scope.positionlist = promise.positionlist;
            });

        };
        $scope.printData = function () {

            var innerContents = document.getElementById("printareaId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); },900);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();

        }

        $scope.OnClickRadioBtn = function () {
            $scope.ASMAY_Id = "";
            // $scope.getyearlist = [];
            $scope.ASMCL_Id = "";
            $scope.getclasslist = [];
            $scope.ASMS_Id = "";
            $scope.getsectionlist = [];
            $scope.getstudentdetails = [];
            $scope.ISMS_Id = "";
            $scope.getsubjectlist = [];
            $scope.submitted1 = false;
            $scope.LPMOEEX_Id = "";
            $scope.getexamlist = [];
            $scope.result = [];
            $scope.reportbtn = true;
            $scope.submitted11 = false;
        };

        $scope.onchangeyear = function () {
            $scope.ASMCL_Id = "";
            $scope.getclasslist = [];
            $scope.ASMS_Id = "";
            $scope.getsectionlist = [];
            $scope.getstudentdetails = [];
            $scope.ISMS_Id = "";
            $scope.getsubjectlist = [];
            $scope.submitted1 = false;
            $scope.LPMOEEX_Id = "";
            $scope.getexamlist = [];
            $scope.result = [];
            $scope.reportbtn = true;
            $scope.submitted11 = false;

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "Flag": $scope.entry
            };
            apiService.create("OnlineTestCandidate/onchangeyear", data).then(function (promise) {
                $scope.getclasslist = promise.getclasslist;
            });

        };

        $scope.onchangeclass = function () {
            $scope.ISMS_Id = "";
            $scope.getsubjectlist = [];
            $scope.ASMS_Id = "";
            $scope.getsectionlist = [];
            $scope.getstudentdetails = [];
            $scope.LPMOEEX_Id = "";
            $scope.getexamlist = [];
            $scope.result = [];
            $scope.reportbtn = true;
            $scope.submitted1 = false;
            $scope.submitted11 = false;
            $scope.checkall = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "Flag": $scope.entry
            };
            apiService.create("OnlineTestCandidate/onchangeclass", data).then(function (promise) {
                $scope.getsectionlist = promise.getsectionlist;
            });

        };
        $scope.grnlist = [];
        /* SEARCH DETAILS  */
        $scope.GetSearchDetails = function () {
            $scope.grnlist = [];
            $scope.submitted11 = false;
            if ($scope.myForm.$valid) {

                var frmdate = $scope.FMCB_fromDATE === null ? "" : $filter('date')($scope.FMCB_fromDATE, "yyyy-MM-dd");
                var todate = $scope.FMCB_toDATE === null ? "" : $filter('date')($scope.FMCB_toDATE, "yyyy-MM-dd");

                $scope.getstudentdetails = [];
                var data = {
                    "MI_Id": $scope.MI_Id,
                    "HRMP_Id": $scope.HRMP_Id,
                    "fromdate": frmdate,
                    "todate": todate
                };

                apiService.create("OnlineTestCandidate/GetReport", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.getstudentdetails = promise.getcandidatedetails;
                        if ($scope.getstudentdetails === null || $scope.getstudentdetails.length === 0) {
                            swal("No Records Found");
                        } else {
                            $scope.getstudentdetails = promise.getcandidatedetails;

                            $scope.grnlist = [];
                          

                            angular.forEach($scope.getstudentdetails, function (ss) {

                                if ($scope.grnlist.length === 0) {
                                    $scope.grnlist.push({
                                        HRCD_Id: ss.HRCD_Id, CandidateName: ss.CandidateName, InterviewDate: ss.InterviewDate, HRCD_EmailId: ss.HRCD_EmailId
                                    });
                                }
                                else if ($scope.grnlist.length > 0) {
                                    var al_exm_cnt = 0;
                                    angular.forEach($scope.grnlist, function (exm) {
                                        if (exm.HRCD_Id === ss.HRCD_Id && exm.InterviewDate === ss.InterviewDate ) {
                                            al_exm_cnt += 1;
                                        }
                                    });
                                    if (al_exm_cnt === 0) {
                                        $scope.grnlist.push({
                                            HRCD_Id: ss.HRCD_Id, CandidateName: ss.CandidateName, InterviewDate: ss.InterviewDate, HRCD_EmailId: ss.HRCD_EmailId
                                        });
                                    }
                                }


                            });


                            console.log($scope.grnlist);


                            angular.forEach($scope.grnlist, function (vv) {
                                var examlist = [];
                                angular.forEach($scope.getstudentdetails, function (bb) {
                                    if (vv.HRCD_Id === bb.HRCD_Id && vv.InterviewDate === bb.InterviewDate) {
                                        examlist.push(bb);
                                    }

                                });

                                vv.LIST = examlist;

                            });

                        }
                    }
                });
            } else {
                $scope.submitted1 = true;
            }
        };

        $scope.exmids = [];
        $scope.listarray = [];
        /* View Student Wise Question List Or Documents */
        $scope.ViewQuestion = function (dd) {
            $scope.listarray = [];
            $scope.listarray = dd.LIST;

           
            $scope.studentname = dd.CandidateName;
            $scope.scheduledd = dd.InterviewDate;



            angular.forEach($scope.listarray, function (cc) {

                    $scope.exmids.push({ OTCANDEX_Id: cc.OTCANDEX_Id });

                });


          
            console.log($scope.exmids);

            var data = {
                "MI_Id": $scope.MI_Id,
                "HRCD_Id": dd.HRCD_Id,
                "type": 'S',
               exmids: $scope.exmids
            };
            debugger
            apiService.create("OnlineTestCandidate/ViewQuestionanswers", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getcandidatequesansdetails = promise.getcandidatequesansdetails;
                    console.log($scope.getcandidatequesansdetails);
                    var count = 0;
                    if ($scope.getcandidatequesansdetails !== null && $scope.getcandidatequesansdetails.length > 0) {
                        //if ($scope.obj.UploadExamFlag === false) {
                        angular.forEach($scope.getcandidatequesansdetails, function (dd) {
                            dd.SubjectiveOrObjective = 'Subjective';
                                if (dd.SubjectiveOrObjective === 'Subjective') {
                                    count += 1;
                                }
                            dd.marks = dd.OTCANDEXSANS_Marks;
                           // dd.questionname = dd.OTMOEQ_Question.replace(/\r?\n/g, '<br/>');
                            dd.questionname = dd.OTMOEQ_Question;
                            dd.OTCANDEXSANS_Answer = dd.OTCANDEXSANS_Answer;
                            });
                            if (count > 0) {
                                $scope.btnsubject = true;
                            }
                            $('#questionanswermodal').modal('show');
                        //} else {
                        //    angular.forEach($scope.getstudentquesansdetails, function (dd) {
                        //        var img = dd.lpstuexaS_AnswerSheetPath;
                        //        var imagarr = img.split('.');
                        //        var lastelement = imagarr[imagarr.length - 1];
                        //        dd.quesfiletypeview1 = lastelement;
                        //        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        //            dd.quesdocument_Pathnewview1 = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpstuexaS_AnswerSheetPath;
                        //        }
                        //    });
                        //    $scope.btnsubject = false;
                        //    $('#ViewUploadedFilesStudentwise').modal('show');
                        //}
                    } else {
                        swal("No Records Found");
                    }
                }

            });
        };


        $scope.ViewQuestion1 = function (dd) {
            $scope.listarray = [];
            $scope.listarray = dd.LIST;

            $scope.mainarray = [];
            $scope.studentname = dd.CandidateName;
            $scope.scheduledd = dd.InterviewDate;
            $scope.EmailId = dd.HRCD_EmailId;

            $scope.ttlqsn = 0;
            $scope.ttlcrctans = 0;
            $scope.ttmxmarks = 0;
            $scope.ttobtnmarks = 0;
            $scope.ttduration = 0;
            $scope.ttper = 0;


            $scope.min = 0;
            $scope.sec = 0;

            angular.forEach($scope.listarray, function (yy) {

                $scope.ttlqsn += yy.OTCANDEX_TotalQnsAnswered;
                $scope.ttlcrctans += yy.OTCANDEX_TotalCorrectAns;
                $scope.ttmxmarks += yy.OTCANDEX_TotalMaxMarks;
                $scope.ttobtnmarks += yy.OTCANDEX_TotalMarks;
                $scope.ttper += yy.OTCANDEX_Percentage;
                if (yy.OTQPTYP_MaxDuration === null || yy.OTQPTYP_MaxDuration === '' || yy.OTQPTYP_MaxDuration === undefined) {
                    yy.OTQPTYP_MaxDuration = 0;
                }
                $scope.ttduration = parseFloat($scope.ttduration) + parseFloat(yy.OTQPTYP_MaxDuration);

                var names = yy.OTCANDEX_TotalTime;
                var nameArr = names.split(':');
               

                $scope.min = parseFloat($scope.min) + parseFloat(nameArr[0]);
                $scope.sec = parseFloat($scope.sec) + parseFloat(nameArr[1]);
                console.log(nameArr);

            });
            debugger;
            var mmmm = 0;
            if ($scope.sec > 60) {


                var minuteValue = parseInt($scope.sec / 60);
                var secondsValue = $scope.sec % 60;


                $scope.sec = secondsValue;
                $scope.min = $scope.min + minuteValue;

            }
      
            angular.forEach($scope.listarray, function (cc) {

                $scope.exmids.push({ OTCANDEX_Id: cc.OTCANDEX_Id });

            });



            console.log($scope.exmids);

            var data = {
                "MI_Id": $scope.MI_Id,
                "HRCD_Id": dd.HRCD_Id,
                "type": 'O',
                exmids: $scope.exmids
            };
            debugger
            apiService.create("OnlineTestCandidate/ViewResultRpt", data).then(function (promise) {
                if (promise !== null) {
                    $scope.objectveans = promise.getcandidatequesansdetailsobj;
                    $scope.subjectiveans = promise.getcandidatequesansdetails;
                    $scope.papertypelist = promise.papertypelist;

                    $scope.mainarray = [];
                    angular.forEach($scope.papertypelist, function (pt) {

                        angular.forEach($scope.listarray, function (ls) {
                            if (pt.otqptyP_Id === ls.OTQPTYP_Id) {
                                var araylist = [];
                                angular.forEach($scope.objectveans, function (objj) {
                                    if (objj.OTQPTYP_Id === ls.OTQPTYP_Id && ls.OTCANDEX_Id === objj.OTCANDEX_Id) {
                                        var obmrk = 0;
                                        if (objj.OTCANDEXANS_CorrectAnsFlg === true) {
                                            obmrk = objj.OTMOEQ_Marks;
                                        }

                                        araylist.push({ QID: objj.OTMOEQ_Id, QSN: objj.OTMOEQ_Question, TYPE: objj.OTMOEQ_SubjectiveFlg, MAXM: objj.OTMOEQ_Marks, OBM: obmrk, CANS: objj.QOption, GANS: objj.COption, CFLAG: objj.OTCANDEXANS_CorrectAnsFlg });
                                        
                                    }
                                    
                                })


                                angular.forEach($scope.subjectiveans, function (objj1) {

                                    $scope.empname = objj1.EmpName;
                                    $scope.usernname = objj1.UserName;
                                    if (objj1.OTQPTYP_Id === ls.OTQPTYP_Id && ls.OTCANDEX_Id === objj1.OTCANDEX_Id) {
                                        var obmrk = 0;
                                   
                                        obmrk = objj1.OTCANDEXSANS_Marks;
                                 

                                        araylist.push({ QID: objj1.OTMOEQ_Id, QSN: objj1.OTMOEQ_Question, TYPE: objj1.OTMOEQ_SubjectiveFlg, MAXM: objj1.OTMOEQ_Marks, OBM: obmrk, CANS: objj1.OTMOEQ_Answer, GANS: objj1.OTCANDEXSANS_Answer, CFLAG: '' });

                                    }

                                })

                                $scope.mainarray.push({ PPRT: ls.OTQPTYP_QuestionPaperName, EDATE: ls.OTCANDEX_Date, TQSN: ls.OTCANDEX_TotalQnsAnswered, TOTALTIME: ls.OTQPTYP_MaxDuration, TMARKS: ls.OTCANDEX_TotalMaxMarks, TKTIME: ls.OTCANDEX_TotalTime, TCNS: ls.OTCANDEX_TotalCorrectAns, TOBM: ls.OTCANDEX_TotalMarks, PER: ls.OTCANDEX_Percentage, LIST: araylist})

                            }



                        })






                    })


                    console.log($scope.mainarray);

                    $('#myModalViewresult').modal('show');
                }

            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.getstudentdetails.some(function (options) {
                return options.checked;
            });
        };

        $scope.OnChangeMarks = function (dd, string) {

            if (string === "upload") {
                $scope.marksnew = parseFloat(dd.marks);

                if ($scope.marksnew > $scope.obj.Maxmarks) {
                    swal("Marks Should Be Less Than Equal To Total Marks");
                    dd.marks = "";
                }
            } else if (string === "question") {
                $scope.marksnewquestion = parseFloat(dd.marks);
                $scope.maxmarksnew = parseFloat(dd.OTMOEQ_Marks);
                if ($scope.marksnewquestion > $scope.maxmarksnew) {
                    swal("Marks Should Be Less Than Equal To Total Marks " + $scope.maxmarksnew);
                    dd.marks = "";
                }
            }
        };

        $scope.printData1 = function () {
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

        $scope.SaveMarks = function (objd) {

            if (objd.$valid) {

                $scope.selectedstudents = [];

                angular.forEach($scope.getstudentdetails, function (dd) {
                    if (dd.checked === true) {
                        $scope.selectedstudents.push({
                            AMST_Id: dd.AMST_Id, marks: dd.marks, LPSTUEX_Id: dd.LPSTUEX_Id,
                            LPSTUEX_CorrectedAnswerSheetPath: dd.LPSTUEX_CorrectedAnswerSheetPath,
                            LPSTUEX_CorrectedAnswerSheetFile: dd.LPSTUEX_CorrectedAnswerSheetFile
                        });
                    }
                });

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ISMS_Id": $scope.ISMS_Id,
                    "LPMOEEX_Id": $scope.LPMOEEX_Id,
                    "savemarks": $scope.selectedstudents,
                    "ASMS_Id": $scope.ASMS_Id,
                    "Flag": 'Marks'
                };
                apiService.create("OnlineTestCandidate/SaveMarks", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message === "Save") {
                            swal("Record Saved/Updated Successfully");
                            $state.reload();
                        } else {
                            swal("Failed To Save/Update Record");
                        }
                    } else {
                        swal("Failed To Save/Update Record");
                    }
                });

            } else {
                $scope.submitted11 = true;
            }
        };

        $scope.SaveSubjectiveMarks = function () {

            if ($scope.myFormquestionans.$valid) {

                $scope.savedetails = [];

                angular.forEach($scope.getcandidatequesansdetails, function (dd) {
                    if (dd.SubjectiveOrObjective === 'Subjective') {
                        $scope.savedetails.push({
                            OTQPTYP_Id: dd.OTQPTYP_Id, OTCANDEX_Id: dd.OTCANDEX_Id, OTMOEQ_Id: dd.OTMOEQ_Id,
                            OTCANDEXSANS_Marks: dd.marks, OTMOEQ_Marks: dd.OTMOEQ_Marks
                        });
                    }
                });

                var data = {
                    "savedetails": $scope.savedetails,
                    exmids: $scope.exmids
                };
                apiService.create("OnlineTestCandidate/SaveSubjectiveMarks", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message === "Add") {
                            swal("Record Saved/Updated Successfully");
                            $('#questionanswermodal').modal('hide');
                        } else {
                            swal("Failed To Save/Update Record");
                        }
                    } else {
                        swal("Failed To Save/Update Record");
                    }
                });

            } else {
                $scope.submitted123 = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted1;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted11;
        };

        $scope.interacted123 = function (field) {
            return $scope.submitted123;
        };

        $scope.cleartabl1 = function (field) {
            $state.reload();
        };

        $scope.toggleAll_S = function (checkall) {
            var toggleStatus = checkall;
            angular.forEach($scope.getstudentdetails, function (itm) {
                itm.checked = toggleStatus;
            });
        };

        $scope.optionToggled_S = function () {
            $scope.checkall = $scope.getstudentdetails.every(function (itm) { return itm.checked; });
        };

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
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


        $scope.uploadtecherdocuments1 = [];

        $scope.uploadtecherdocuments = function (input, document) {

            $scope.uploadtecherdocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {
                //$scope.size = input.files[0].size;
                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg") { UploaddianPhoto(document); }
                else if (input.files[0].type === "application/pdf") { UploaddianPhoto(document); }
                else if (input.files[0].type === "application/msword") { UploaddianPhoto(document); }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { UploaddianPhoto(document); }
                else if (input.files[0].type === "application/vnd.ms-excel") { UploaddianPhoto(document); }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document,") { UploaddianPhoto(document); }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation") { UploaddianPhoto(document); }
                else if (input.files[0].type === "application/vnd.ms-powerpoint") { UploaddianPhoto(document); }
                else { swal("Upload MP4, Pdf, Doc, Image Files Only"); }
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
                    headers: {
                        'Content-Type': undefined
                    },
                    transformRequest: angular.identity,

                })
                .success(function (d) {
                    defer.resolve(d);
                    data.LPSTUEX_CorrectedAnswerSheetPath = d;
                    data.LPSTUEX_CorrectedAnswerSheetFile = $scope.filename;
                    $('#').attr('src', data.lpmtR_Resources);
                    var img = data.lpmtR_Resources;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.quesfiletypeview1 = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.quesdocument_Pathnewview1 = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpmtR_Resources;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
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
                        scope.model += newVal[0];
                        var editor = $("#editor").data("kendoEditor");
                        editor.value(scope.model);
                    }
                });
            }
        };
    });
})();