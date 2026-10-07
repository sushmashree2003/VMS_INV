(function () {
    'use strict';
    angular.module('app').controller('OTMasterQuestionController', OTMasterQuestionController)

    OTMasterQuestionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter', '$q', '$sce', '$window']
    function OTMasterQuestionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter, $q, $sce, $window) {

        $scope.groups = [{ title: 'Dynamic Group Header - 1', content: 'Dynamic Group Body - 1' },
        { title: 'Dynamic Group Header - 2', content: 'Dynamic Group Body - 2' },
        { title: 'Dynamic Group Header - 3', content: 'Dynamic Group Body - 3' },
        { title: 'Dynamic Group Header - 4', content: 'Dynamic Group Body - 4' }];

        $scope.searc_button = true;
        $scope.sortKey = 'OTMOEQ_Id';
        $scope.sortReverse = true;
        $scope.OTMOEQ_SubjectiveFlg = false;
        $scope.edit = false;
        $scope.getexamhappenedcounttemp = false;

        $scope.answer = "";
        $scope.show_ansOption = false;
        $scope.obj = {};
        $scope.obj.searchValueddd = "";
        $scope.searchValue22 = "";

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.currentPage1 = 1;
        $scope.currentPage2 = 1;
        $scope.currentPage3 = 1;
        $scope.btn = false;

        $scope.itemsPerPage1 = paginationformasters;
        $scope.itemsPerPage2 = paginationformasters;
        $scope.itemsPerPage3 = paginationformasters;

        $scope.teacherdocuupload = {};
        $scope.teacherdocuupload = [{ id: 'Teacher1' }];
        $scope.totalgrid = [];

        //***** MASTER QUESTION ******//
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("OnlineTest/getmasterquestionloaddata", pageid).then(function (promise) {
                $scope.milist = promise.milist;
                $scope.getarratcomplexities = promise.getarratcomplexities;
            });
        };


        $scope.UploadFiles = function (objupload, index) {

            $scope.optionsindex = index;
            if (objupload.tempoptionsfiles !== undefined && objupload.tempoptionsfiles !== null && objupload.tempoptionsfiles.length > 0) {
                $scope.teacherdocuuploadopts = objupload.tempoptionsfiles;

                angular.forEach($scope.teacherdocuuploadopts, function (d) {
                    var img = d.otmoeqoaF_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    d.filetype = lastelement;
                    console.log("data.filetype : " + d.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        d.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + d.otmoeqoaF_FilePath;
                    }
                });
            } else {
                $scope.teacherdocuuploadopts = [{ id: 'Teacheropts1' }];
            }
            $('#optsfilesupload').modal('show');
        };


        $scope.UploadFiles1 = function (objupload, index) {

            $scope.optionsindex = index;
            if (objupload.tempoptionsfiles !== undefined && objupload.tempoptionsfiles !== null && objupload.tempoptionsfiles.length > 0) {
                $scope.teacherdocuuploadopts = objupload.tempoptionsfiles;

                angular.forEach($scope.teacherdocuuploadopts, function (d) {
                    var img = d.otmoeqoaF_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    d.filetype = lastelement;
                    console.log("data.filetype : " + d.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        d.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + d.otmoeqoaF_FilePath;
                    }
                });
            } else {
                $scope.teacherdocuuploadopts = [{ id: 'Teacheropts1' }];
            }
            $('#optsfilesupload1').modal('show');
        };
        $scope.OnClickOptsFilesUpload = function () {
            $scope.tempoptsfiles = [];
            angular.forEach($scope.teacherdocuuploadopts, function (dd) {
                var id = 0;
                if (dd.otmoeqoaF_FilePath !== undefined && dd.otmoeqoaF_FilePath !== null && dd.otmoeqoaF_FilePath !== "") {
                    if (dd.otmoeqoaF_Id !== undefined && dd.otmoeqoaF_Id !== null) {
                        id = dd.otmoeqoaF_Id;
                    }
                    $scope.tempoptsfiles.push({
                        otmoeqoaF_FilePath: dd.otmoeqoaF_FilePath, otmoeqoaF_FileName: dd.otmoeqoaF_FileName,
                        otmoeqoaF_Id: id
                    });
                }
            });

            if ($scope.tempoptsfiles.length > 0) {
                angular.forEach($scope.totalgrid, function (dd, index) {
                    if (index === $scope.optionsindex) {
                        dd.tempoptionsfiles = $scope.tempoptsfiles;
                    }
                });
            }
        };


        $scope.OnClickOptsFilesUpload1 = function () {
            $scope.tempoptsfiles = [];
            angular.forEach($scope.teacherdocuuploadopts, function (dd) {
                var id = 0;
                if (dd.otmoeqoaF_FilePath !== undefined && dd.otmoeqoaF_FilePath !== null && dd.otmoeqoaF_FilePath !== "") {
                    if (dd.otmoeqoaF_Id !== undefined && dd.otmoeqoaF_Id !== null) {
                        id = dd.otmoeqoaF_Id;
                    }
                    $scope.tempoptsfiles.push({
                        otmoeqoaF_FilePath: dd.otmoeqoaF_FilePath, otmoeqoaF_FileName: dd.otmoeqoaF_FileName,
                        otmoeqoaF_Id: id
                    });
                }
            });

            if ($scope.tempoptsfiles.length > 0) {
                angular.forEach($scope.totalgrid1, function (dd, index) {
                    if (index === $scope.optionsindex) {
                        dd.tempoptionsfiles = $scope.tempoptsfiles;
                    }
                });
            }
        };

        /* Options Files */
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
                    data.otmoeqoaF_FilePath = d;
                    data.otmoeqoaF_FileName = $scope.filename;
                    $('#').attr('src', data.otmoeqoaF_FilePath);
                    var img = data.otmoeqoaF_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.otmoeqoaF_FilePath;
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



        $scope.oncmpchange = function () {


            var data = {
                "MI_Id": $scope.MI_Id,

            };
            apiService.create("OnlineTest/loadcdata", data).then(function (promise) {
                $scope.positionlist = promise.positionlist;
                $scope.getMasterQuestiondetails = promise.getMasterQuestiondetails;
                $scope.getConfigurationSettings = promise.getConfigurationSettings;
            });

        };



        $scope.getqnspapertype = function () {
            $scope.totalgrid = [];
            var data = {
                "HRMP_Id": $scope.HRMP_Id,
                "MI_Id": $scope.MI_Id,
            };

            apiService.create("OnlineTest/getqnspapertype", data).then(function (promise) {
                if (promise.papertypelist !== null && promise.papertypelist.length > 0) {
                    $scope.papertypelist = promise.papertypelist;
                }
            });

        };

        $scope.gettopiclist = function () {
            $scope.totalgrid = [];
            $scope.gettopiclist = [];
            $scope.LPMT_Id = "";

            var data = {
                //"ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ISMS_Id": $scope.ISMS_Id
            };

            apiService.create("OnlineTest/gettopiclist", data).then(function (promise) {
                if (promise.gettopiclist !== null && promise.gettopiclist.length > 0) {
                    $scope.gettopiclist = promise.gettopiclist;

                }
            });
        };

        $scope.OnchangeNoofOptions = function () {

            $scope.totalgrid = [];
            $scope.noofoptions = parseInt($scope.LPMOEQ_NoOfOptions);
            for (var i = 0; i < $scope.noofoptions; i++) {
                $scope.totalgrid.push({ OTMOEQOA_OptionCode: '', OTMOEQOA_Option: '', OTMOEQOA_AnswerDesc: '', OTMOEQOA_AnswerFlag: false, OTMOEQOA_Id: 0 });
            }
        };

        $scope.OnchangeNoofOptions1 = function (a) {
            $scope.totalgrid1 = [];
            $scope.noofoptions = parseInt(a);
            for (var i = 0; i < $scope.noofoptions; i++) {
                $scope.totalgrid1.push({ OTMOEQOA_OptionCode: '', OTMOEQOA_Option: '', OTMOEQOA_AnswerDesc: '', OTMOEQOA_AnswerFlag: false, OTMOEQOA_Id: 0 });
            }
        };


        $scope.clearoption = function () {
            $scope.totalgrid1 = [];
            $scope.extraoption = false;
            $scope.LPMOEQ_NoOfOptions1 = '';
            $('#sel92').val($scope.LPMOEQ_NoOfOptions1);
            angular.forEach($scope.getoptiondetails, function (zz) {

                if ($scope.correctans === zz.otmoeqoA_Id) {
                    zz.otmoeqoA_AnswerFlag = true;
                }

            });
        };


        $scope.extraoption = false;
        $scope.addnewww = function () {
            $scope.totalgrid1 = [];
            $scope.extraoption = true;
            $scope.LPMOEQ_NoOfOptions1 = '';
            $('#sel92').val($scope.LPMOEQ_NoOfOptions1);
            angular.forEach($scope.getoptiondetails, function (zz) {

                if ($scope.correctans === zz.otmoeqoA_Id) {
                    zz.otmoeqoA_AnswerFlag = true;
                }

            })
        }


        $scope.AddMoreOptions = function () {
            debugger;
            $scope.submitted1 = false;

            if ($scope.myForm2.$valid) {

                var data = {
                    "OTMOEQ_Id": $scope.OTMOEQ_Id1,
                    "tempoptionsDTO": $scope.totalgrid1,
                    "MI_Id": $scope.MI_Id,
                };
                apiService.create("OnlineTest/AddMoreOptions", data).then(function (promise) {
                    if (promise.returnval === true) {
                        swal('New Options added Successfully');
                    } else {

                        swal('Error');
                    }
                    $scope.getoptiondetails = promise.getViewSavedOptions;

                    $scope.totalgrid1 = [];
                    $scope.extraoption = false;
                    $scope.LPMOEQ_NoOfOptions1 = '';
                    $('#sel92').val($scope.LPMOEQ_NoOfOptions1);
                });

            }
            else {
                $scope.submitted4 = true;
            }
        };



        $scope.SaveMasterQuestionDetails = function () {
            $scope.submitted1 = true;
            $scope.submitted2 = false;
            $scope.submitted3 = false;

            if ($scope.myForm.$valid) {
                var desc = "";
                if ($scope.OTMOEQ_QuestionDesc !== undefined && $scope.OTMOEQ_QuestionDesc !== null && $scope.OTMOEQ_QuestionDesc !== '') {
                    desc = $scope.OTMOEQ_QuestionDesc;
                } else {
                    desc = "";
                }
                if ($scope.OTMOEQ_SubjectiveFlg === false) {
                    var checkansflag = 0;
                    angular.forEach($scope.totalgrid, function (d) {
                        if (d.OTMOEQOA_AnswerFlag === true) {
                            checkansflag += 1;
                        }
                    });

                    if (checkansflag === 0) {
                        swal("Select One Correct Answer Flag");
                        return;
                    }

                    if (checkansflag > 1) {
                        swal("Select Only One Correct Answer");
                        return;
                    }
                }

                var questionanswer = "";
                if ($scope.OTMOEQ_SubjectiveFlg === true) {
                    questionanswer = $scope.OTMOEQ_Answer !== undefined && $scope.OTMOEQ_Answer !== null && $scope.OTMOEQ_Answer !== "" ? $scope.OTMOEQ_Answer : "";
                }

                var noofoptionsnew = 0;
                if ($scope.OTMOEQ_SubjectiveFlg === false) {
                    noofoptionsnew = $scope.LPMOEQ_NoOfOptions !== undefined && $scope.LPMOEQ_NoOfOptions !== null && $scope.LPMOEQ_NoOfOptions !== "" ? $scope.LPMOEQ_NoOfOptions : 0;
                }

                var data = {
                    //"ASMAY_Id": $scope.ASMAY_Id,
                    //  "ASMCL_Id": $scope.ASMCL_Id,obj
                    "HRMP_Id": $scope.HRMP_Id,
                    "OTQPTYP_Id": $scope.OTQPTYP_Id,
                    "OTMOEQ_Question": $scope.OTMOEQ_Question,
                    "OTMOEQ_QuestionDesc": desc,
                    "OTMOEQ_Id": $scope.OTMOEQ_Id,
                    "OTMOEQ_Marks": $scope.OTMOEQ_Marks,
                    "OTMOEQ_Answer": questionanswer,
                    "OTMOEQ_SubjectiveFlg": $scope.OTMOEQ_SubjectiveFlg,
                    "LPMOEQ_NoOfOptions": noofoptionsnew,
                    "tempfilesDTO": $scope.teacherdocuupload,
                    "tempoptionsDTO": $scope.totalgrid,
                    "MI_Id": $scope.MI_Id,
                    "LPMCOMP_Id": $scope.LPMCOMP_Id,
                    "OTMOEQ_AnswerUploadFlag": $scope.OTMOEQ_AnswerUploadFlag,
                };

                apiService.create("OnlineTest/SaveMasterQuestionDetails", data).then(function (promise) {
                    debugger;
                    if (promise.message === 'Duplicate') {
                        swal('Record Already Exist');
                    }
                    else if (promise.message === 'ex') {
                        swal('Exceeds the number of Questions');
                    }
                    else {
                        if (promise.message === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.message === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else {
                            swal("Failed To Save/Update Record");
                        }
                    }
                    $scope.cleartabl1();
                });
            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.EditMasterQuestion = function (id) {
            $scope.totalgrid = [];
            $scope.teacherdocuupload = [{ id: 'Teacher1' }];

            var data = {
                "OTMOEQ_Id": id,
                "MI_Id": $scope.MI_Id,
            };

            apiService.create("OnlineTest/EditMasterQuestion", data).then(function (promise) {
                if (promise.geteditmasterquestion !== null && promise.geteditmasterquestion.length > 0) {
                    debugger;
                    $scope.papertypelist = promise.papertypelist;
                    $scope.HRMP_Id = promise.geteditmasterquestion[0].hrmP_Id;
                    $scope.OTQPTYP_Id = promise.geteditmasterquestion[0].otqptyP_Id;


                    $scope.OTMOEQ_Question = promise.geteditmasterquestion[0].otmoeQ_Question;
                    $scope.OTMOEQ_QuestionDesc = promise.geteditmasterquestion[0].otmoeQ_QuestionDesc;
                    $scope.OTMOEQ_Id = promise.geteditmasterquestion[0].otmoeQ_Id;
                    $scope.OTMOEQ_SubjectiveFlg = promise.geteditmasterquestion[0].otmoeQ_SubjectiveFlg;
                    $scope.OTMOEQ_Answer = promise.geteditmasterquestion[0].otmoeQ_Answer;
                    $scope.OTMOEQ_Marks = promise.geteditmasterquestion[0].otmoeQ_Marks;
                    $scope.LPMCOMP_Id = promise.geteditmasterquestion[0].lpmcomP_Id;
                    $scope.OTMOEQ_AnswerUploadFlag = promise.geteditmasterquestion[0].otmoeQ_AnswerUploadFlag;


                    $scope.teacherdocuupload = promise.geteditdocuments;
                    $scope.edit = true;



                    if ($scope.teacherdocuupload !== null && $scope.teacherdocuupload.length > 0) {
                        angular.forEach($scope.teacherdocuupload, function (dd) {
                            var img = dd.otmoeqF_FilePath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            console.log("data.filetype : " + dd.filetype);
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.otmoeqF_FilePath;
                            }
                        });
                    } else {
                        $scope.teacherdocuupload = [{ id: 'Teacher1' }];
                    }

                    if ($scope.OTMOEQ_SubjectiveFlg === false) {
                        $scope.getSavedOptions = promise.getSavedOptions;

                        if ($scope.getSavedOptions !== null && $scope.getSavedOptions.length > 0) {
                            $scope.LPMOEQ_NoOfOptions = $scope.getSavedOptions.length;

                            angular.forEach($scope.getSavedOptions, function (dd) {

                                $scope.totalgrid.push({ OTMOEQOA_OptionCode: dd.otmoeqoA_OptionCode, OTMOEQOA_Option: dd.otmoeqoA_Option, OTMOEQOA_AnswerDesc: dd.otmoeqoA_AnswerDesc, OTMOEQOA_AnswerFlag: dd.otmoeqoA_AnswerFlag, OTMOEQOA_Id: dd.otmoeqoA_Id });
                            });

                            $scope.getViewSavedOptionsFiles = promise.getViewSavedOptionsFiles;
                            $scope.tempoptsfiles = [];
                            angular.forEach($scope.totalgrid, function (ddd) {
                                $scope.tempoptsfiles = [];
                                angular.forEach($scope.getViewSavedOptionsFiles, function (df) {
                                    if (ddd.OTMOEQOA_Id === df.otmoeqoA_Id) {
                                        $scope.tempoptsfiles.push({
                                            otmoeqoaF_FilePath: df.otmoeqoaF_FilePath, otmoeqoaF_FileName: df.otmoeqoaF_FileName,
                                            otmoeqoaF_Id: df.otmoeqoaF_Id
                                        });
                                    }
                                });
                                if ($scope.tempoptsfiles.length > 0) {
                                    ddd.tempoptionsfiles = $scope.tempoptsfiles;
                                }
                            });

                        } else {
                            $scope.totalgrid = [];
                            for (var i = 0; i < $scope.noofoptions; i++) {
                                $scope.totalgrid.push({ OTMOEQOA_OptionCode: '', OTMOEQOA_Option: '', OTMOEQOA_AnswerDesc: '', OTMOEQOA_AnswerFlag: false, OTMOEQOA_Id: 0 });
                            }


                        }
                    }
                }
            });
        };

        $scope.ViewMasterQuesDoc = function (dd) {
            var data = {
                "OTMOEQ_Id": dd.otmoeQ_Id,
                "MI_Id": $scope.MI_Id,
            };
            apiService.create("OnlineTest/ViewMasterQuesDoc", data).then(function (promise) {
                if (promise !== null) {

                    $scope.viewdocarray = promise.getviedocarray;

                    angular.forEach($scope.viewdocarray, function (dd) {
                        var img = dd.otmoeqF_FilePath;
                        var imagarr = img.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        dd.filetype = lastelement;
                        console.log("data.filetype : " + dd.filetype);
                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                            dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.otmoeqF_FilePath;
                        }
                    });
                }
            });
        };
        $scope.correctans = 0;
        $scope.OTMOEQ_Id1 = 0;
        $scope.getoptiondetails = [];
        $scope.ViewMasterQuesOptions = function (dd) {
            debugger;
            $scope.getoptiondetails = [];
            $scope.totalgrid1 = [];
            $scope.extraoption = false;
            $scope.LPMOEQ_NoOfOptions1 = '';

            $('#sel92').val($scope.LPMOEQ_NoOfOptions1);

            $scope.correctans = 0;
            $scope.Question = dd.otmoeQ_Question;
            $scope.Questiondesc = dd.otmoeQ_QuestionDesc;
            $scope.Subject = dd.otqptyP_QuestionPaperName;
            $scope.ClassName = dd.hrmP_Position;
            $scope.lpmoeQ_SubjectiveFlgdetails = dd.otmoeQ_SubjectiveFlg;
            $scope.OTMOEQ_Id1 = dd.otmoeQ_Id;

            if (dd.otmoeQ_SubjectiveFlg === true) {
                $scope.answerdetails = dd.otmoeQ_Answer;
                return;
            }

            var data = {
                "OTMOEQ_Id": dd.otmoeQ_Id,
                "MI_Id": $scope.MI_Id,
            };

            apiService.create("OnlineTest/ViewMasterQuesOptions", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getoptiondetails = promise.getViewSavedOptions;
                    angular.forEach($scope.getoptiondetails, function (zz) {
                        if (zz.otmoeqoA_AnswerFlag === true) {
                            $scope.correctans = zz.otmoeqoA_Id;
                        }
                    })

                }
            });
        };

        $scope.ViewUploadOptionFiles = function (dd) {
            var data = {
                "OTMOEQOA_Id": dd.otmoeqoA_Id,
                "OTMOEQ_Id": dd.otmoeQ_Id
            };

            apiService.create("OnlineTest/ViewUploadOptionFiles", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getViewSavedOptionsFiles = promise.getViewSavedOptionsFiles;

                    angular.forEach($scope.getViewSavedOptionsFiles, function (dd) {
                        var img = dd.otmoeqoaF_FilePath;
                        var imagarr = img.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        dd.filetype = lastelement;
                        console.log("data.filetype : " + dd.filetype);
                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                            dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.otmoeqoaF_FilePath;
                        }
                    });
                }
            });
        };

        $scope.DeactivateActivateOptionsDocument = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.otmoeqoaF_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            var data = {
                "OTMOEQOA_Id": deactiveRecord.otmoeqoA_Id,
                "OTMOEQOAF_Id": deactiveRecord.otmoeqoaF_Id
            };
            swal({
                title: "Are you sure",
                text: "Do You Want To " + mgs + " Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("OnlineTest/DeactivateActivateOptionsDocument", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record " + confirmmgs + " " + "successfully");
                            }
                            else {
                                swal("Record " + mgs + " Failed");
                            }
                            $scope.getViewSavedOptionsFiles = promise.getViewSavedOptionsFiles;

                            angular.forEach($scope.getViewSavedOptionsFiles, function (dd) {
                                var img = dd.otmoeqoaF_FilePath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("data.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.otmoeqoaF_FilePath;
                                }
                            });
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.DeactivateActivateDocument = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.otmoeqF_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            var data = {
                "OTMOEQF_Id": deactiveRecord.otmoeqF_Id,
                "OTMOEQ_Id": deactiveRecord.otmoeQ_Id,
                "MI_Id": $scope.MI_Id,
            };
            swal({
                title: "Are you sure",
                text: "Do You Want To " + mgs + " Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("OnlineTest/DeactivateActivateDocument", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record " + confirmmgs + " " + "successfully");
                            }
                            else {
                                swal("Record " + mgs + " Failed");
                            }

                            $scope.viewdocarray = promise.getviedocarray;

                            angular.forEach($scope.viewdocarray, function (dd) {
                                var img = dd.otmoeqF_FilePath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("data.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.otmoeqF_FilePath;
                                }
                            });
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }


                });
        };

        $scope.DeactivateActivateQuesOption = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.otmoeqoA_ActiveFlg === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            var data = {
                "OTMOEQOA_Id": deactiveRecord.otmoeqoA_Id,
                "OTMOEQ_Id": deactiveRecord.otmoeQ_Id,
                "MI_Id": $scope.MI_Id,
            };
            swal({
                title: "Are you sure",
                text: "Do You Want To " + mgs + " Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("OnlineTest/DeactivateActivateQuesOption", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record " + confirmmgs + " " + "successfully");
                            }
                            else {
                                swal("Record " + mgs + " Failed");
                            }
                            $scope.getoptiondetails = promise.getViewSavedOptions;
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };



        $scope.textAreaAdjust = function (element) {

            element.style.height = "1px";
            element.style.height = (25 + element.scrollHeight) + "px";
            debugger;

        }






        $scope.DeactivateActivateQues = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.otmoeQ_ActiveFlg === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            var data = {
                "OTMOEQ_Id": deactiveRecord.otmoeQ_Id,
                "OTQPTYP_Id": deactiveRecord.otqptyP_Id,
                "HRMP_Id": deactiveRecord.hrmP_Id,
                "MI_Id": $scope.MI_Id,

            };
            swal({
                title: "Are you sure",
                text: "Do You Want To " + mgs + " Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("OnlineTest/DeactivateActivateQues", data).then(function (promise) {
                            if (promise.message === 'ex') {
                                swal('Not able to Activate/Exceeds the maximum number of Questions ');
                            }
                            else {

                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.cleartabl1 = function () {
            $state.reload();
        };

        $scope.searchValue1 = function (obj) {
            return (angular.lowercase(obj.otmoeQ_Question)).indexOf(angular.lowercase($scope.obj.searchValueddd)) >= 0 ||
                (angular.lowercase(obj.lpmoeQ_QuestionDesc)).indexOf(angular.lowercase($scope.obj.searchValueddd)) >= 0 ||
                (angular.lowercase(obj.asmcL_ClassName)).indexOf(angular.lowercase($scope.obj.searchValueddd)) >= 0 ||
                (angular.lowercase(obj.lpmT_TopicName)).indexOf(angular.lowercase($scope.obj.searchValueddd)) >= 0 ||
                (angular.lowercase(obj.ismS_SubjectName)).indexOf(angular.lowercase($scope.obj.searchValueddd)) >= 0 ||
                (JSON.stringify(obj.lpmoeQ_Marks)).indexOf($scope.searchValueddd) >= 0;
        };

        $scope.interacted = function (field) {
            return $scope.submitted1;
        };
        $scope.interactedw = function (field) {
            return $scope.submitted4;
        };

        $scope.showmodaldetails = function (data) {
            $('#preview').attr('src', data.document_Path);
        };

        $scope.addNewsiblingguard = function () {
            var newItemNo = $scope.teacherdocuupload.length + 1;

            if (newItemNo <= 10) {
                $scope.teacherdocuupload.push({ 'id': 'Teacher' + newItemNo });
            }
            console.log($scope.teacherdocuupload);
        };

        $scope.removeNewsiblingguard = function (index) {
            var newItemNo = $scope.teacherdocuupload.length - 1;
            $scope.teacherdocuupload.splice(index, 1);

            if ($scope.teacherdocuupload.length === 0) {
                //data
            }
        };

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.pauseOrPlay = function (ele) {
            $('#popup15').modal({
                show: false
            }).on('hidden.bs.modal', function () {
                $(this).find('video')[0].pause();
            });
        };

        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.otmoeqF_FilePath;
            $scope.videdfd = data.otmoeqF_FilePath;
            $scope.movie = { src: data.otmoeqF_FilePath };
            $scope.movie1 = { src: data.otmoeqF_FilePath };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.otmoeqF_FilePath });
            console.log($scope.view_videos);
        };

        $scope.onview = function (filepath, filename) {

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

        $scope.addColumn = function (role, indexx, totalgrid) {
            angular.forEach(totalgrid, function (subscription, index) {
                if (indexx !== index)
                    subscription.OTMOEQOA_AnswerFlag = false;
            });
        };

        $scope.addColumn1 = function (role, indexx, totalgrid) {
            debugger;
            var cnt = 0;
            angular.forEach(totalgrid, function (ss) {
                if (ss.OTMOEQOA_AnswerFlag === true) {
                    cnt += 1;
                }

            });
            if (cnt > 0) {
                angular.forEach(totalgrid, function (subscription, index) {
                    if (indexx !== index)
                        subscription.OTMOEQOA_AnswerFlag = false;
                });

                angular.forEach($scope.getoptiondetails, function (zz) {
                    zz.otmoeqoA_AnswerFlag = false;
                })

            }
            if (cnt === 0) {

                angular.forEach($scope.getoptiondetails, function (zz) {

                    if ($scope.correctans === zz.otmoeqoA_Id) {
                        zz.otmoeqoA_AnswerFlag = true;
                    }

                })

            }

        };


        $scope.checkcode = function (role, indexx, totalgrid) {
            debugger;
            var cn = 0
            var cn1 = 0
            if (role.OTMOEQOA_OptionCode != '' && role.OTMOEQOA_OptionCode != null && role.OTMOEQOA_OptionCode != undefined) {


                angular.forEach($scope.getoptiondetails, function (mm) {
                    if (mm.otmoeqoA_OptionCode != '' && mm.otmoeqoA_OptionCode != null && mm.otmoeqoA_OptionCode != undefined) {
                        if (role.OTMOEQOA_OptionCode.toLowerCase() === mm.otmoeqoA_OptionCode.toLowerCase()) {
                            cn += 1;
                        }
                    }

                })

                angular.forEach(totalgrid, function (ss, index) {
                    if (ss.OTMOEQOA_OptionCode != '' && ss.OTMOEQOA_OptionCode != null && ss.OTMOEQOA_OptionCode != undefined) {
                        if (role.OTMOEQOA_OptionCode.toLowerCase() === ss.OTMOEQOA_OptionCode.toLowerCase() && index !== indexx) {
                            cn += 1;
                        }
                    }

                })
                if (cn > 0) {
                    angular.forEach(totalgrid, function (subscription, index) {
                        if (indexx === index) {
                            subscription.OTMOEQOA_OptionCode = '';
                            swal('Option Code Should be Unique..!!!')
                        }

                    });
                }
            }

            if (role.OTMOEQOA_Option != '' && role.OTMOEQOA_Option != null && role.OTMOEQOA_Option != undefined) {
                angular.forEach($scope.getoptiondetails, function (mm) {
                    if (mm.otmoeqoA_Option != '' && mm.otmoeqoA_Option != null && mm.otmoeqoA_Option != undefined) {
                        if (role.OTMOEQOA_Option.toLowerCase() === mm.otmoeqoA_Option.toLowerCase()) {
                            cn1 += 1;
                        }
                    }

                })

                angular.forEach(totalgrid, function (ss, index) {
                    if (ss.OTMOEQOA_Option != '' && ss.OTMOEQOA_Option != null && ss.OTMOEQOA_Option != undefined) {
                        if (role.OTMOEQOA_Option.toLowerCase() === ss.OTMOEQOA_Option.toLowerCase() && index !== indexx) {
                            cn1 += 1;
                        }
                    }

                })
                if (cn1 > 0) {
                    angular.forEach(totalgrid, function (subscription, index) {
                        if (indexx === index) {
                            subscription.OTMOEQOA_Option = '';
                            swal('Options  Should be Unique..!!!');
                        }

                    });
                }
            }
        };

        $scope.checkcode1 = function (role, indexx, totalgrid) {
            debugger;
            var cn = 0
            var cn1 = 0
            if (role.OTMOEQOA_OptionCode != '' && role.OTMOEQOA_OptionCode != null && role.OTMOEQOA_OptionCode != undefined) {



                angular.forEach(totalgrid, function (ss, index) {

                    if (ss.OTMOEQOA_OptionCode != '' && ss.OTMOEQOA_OptionCode != null && ss.OTMOEQOA_OptionCode != undefined) {
                        if (role.OTMOEQOA_OptionCode.toLowerCase() === ss.OTMOEQOA_OptionCode.toLowerCase() && index !== indexx) {
                            cn += 1;
                        }
                    }

                })
                if (cn > 0) {
                    angular.forEach(totalgrid, function (subscription, index) {
                        if (indexx === index) {
                            subscription.OTMOEQOA_OptionCode = '';
                            swal('Option Code Should be Unique..!!!')
                        }

                    });
                }
            }

            if (role.OTMOEQOA_Option != '' && role.OTMOEQOA_Option != null && role.OTMOEQOA_Option != undefined) {


                angular.forEach(totalgrid, function (ss, index) {
                    if (ss.OTMOEQOA_Option != '' && ss.OTMOEQOA_Option != null && ss.OTMOEQOA_Option != undefined) {
                        if (role.OTMOEQOA_Option.toLowerCase() === ss.OTMOEQOA_Option.toLowerCase() && index !== indexx) {
                            cn1 += 1;
                        }
                    }

                })
                if (cn1 > 0) {
                    angular.forEach(totalgrid, function (subscription, index) {
                        if (indexx === index) {
                            subscription.OTMOEQOA_Option = '';
                            swal('Options  Should be Unique..!!!');
                        }

                    });
                }
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

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
        };

        $scope.sort = function (key) {
            $scope.sortKey = key;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

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
                    }
                });
            }
        };
    });

})();