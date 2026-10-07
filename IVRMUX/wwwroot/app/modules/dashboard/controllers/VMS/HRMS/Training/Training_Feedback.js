(function () {
    'use strict';
    angular
        .module('app')
        .controller('TrainingFeedbackController', TrainingFeedbackController)

    TrainingFeedbackController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function TrainingFeedbackController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.sortKey = 'HRTRQNS_Id';
        $scope.sortReverse = true;
        $scope.obj = {};

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }


        $scope.agendadetails = function (item) {
            $scope.hrmetrtytopM_StudyMaterialsDesc = item.hrmetrtytopM_StudyMaterialsDesc
            document.getElementById("ada").innerHTML = $scope.hrmetrtytopM_StudyMaterialsDesc;
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted || field.$dirty;
        };

        //get data
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("Training_Feedback/loaddata", pageid)
                .then(function (promise) {

                    //$scope.traninglisttrainer = promise.traninglisttrainee;
                    $scope.getmasterdata = promise.getmasterdata;

                    $scope.traninglisttrainer = promise.traninglisttrainer;
                    $scope.traninglisttrainee = promise.traninglisttrainee;
                    if (promise.traineeprogdetails != null && promise.traineeprogdetails.length > 0) {
                        $scope.traineeprogdetails = promise.traineeprogdetails;
                    }
                    if (promise.trainerprogdetails != null && promise.trainerprogdetails.length > 0) {
                        $scope.trainerprogdetails = promise.trainerprogdetails;
                    }
                });
        };

        $scope.getQuestions = function () {
            $scope.Type = "";
            $scope.mappedquestionlist = [];
            $scope.mappedoptionlist = [];
            $scope.trainerlist = [];

            var data = {
                "HRTCR_Id": $scope.hrtcR_Id,
                "HRMTT_Id": $scope.hrmtT_Id,
                "HRMTT_Topic": $scope.hrmtT_Topic,
                "Type": $scope.feedbacktype
            };
            apiService.create("Training_Feedback/getQuestions", data).
                then(function (promise) {

                    if (promise !== null) {
                        if ((promise.mappedquestionlist !== null && promise.mappedquestionlist.length > 0) && (promise.mappedoptionlist !== null && promise.mappedoptionlist.length > 0)) {

                            $scope.tempoptionarray = [];

                            if (promise.traineelist != null && promise.traineelist.length > 0) {
                                $scope.traineelist = promise.traineelist;
                            }
                            if (promise.trainerlist != null && promise.trainerlist.length > 0) {
                                $scope.trainerlist = promise.trainerlist;
                            }
                            if (promise.mappedquestionlist != null && promise.mappedquestionlist.length > 0) {
                                $scope.mappedquestionlist = promise.mappedquestionlist;
                            }
                            if (promise.mappedoptionlist != null && promise.mappedoptionlist.length > 0) {
                                $scope.mappedoptionlist = promise.mappedoptionlist;

                            }
                            $scope.mappedquestionlist = promise.mappedquestionlist;
                            $scope.mappedoptionlist = promise.mappedoptionlist;

                            angular.forEach($scope.mappedquestionlist, function (ddd) {

                                $scope.tempoptionarray = [];
                                angular.forEach($scope.mappedoptionlist, function (dddd) {
                                    if (ddd.hrmfqnS_Id == dddd.hrmfqnS_Id) {
                                        $scope.tempoptionarray.push({ hrmfqnS_Id: dddd.hrmfqnS_Id, hrmfopT_Id: dddd.hrmfopT_Id, hrmfopT_OptionName: dddd.hrmfopT_OptionName });
                                    }
                                    else if (ddd.hrmfqnS_QuestionTypeFlg != "Objective") {
                                        $scope.tempoptionarray.push({
                                            hrmfqns_id: ddd.hrmfqns_id, hrmfqns_questionname: ddd.hrmfqns_questionname,
                                        });
                                    }
                                });
                                ddd.tempoptionarray = $scope.tempoptionarray;
                            });

                            console.log($scope.tempoptionarray);
                        }
                        else {
                            $scope.tempoptionarray = [];
                            swal("No Records Found");
                            $state.reload();
                        }




                    } else {
                        $scope.tempoptionarray = [];
                        swal("Something Went Wrong Kidndly Contact Administrator");
                        $state.reload();
                    }
                });
        };

        $scope.getFeedbackTrainer = function () {
            $scope.trainingFeedbacklist = [];
            var data = {
                "HRTCR_Id": $scope.hrtcR_Id,
                "HRMTT_Id": $scope.hrmtT_Id,
                "HRTFEED_TrainerId": $scope.obj.hrtfeeD_TrainerId,
                "Type": $scope.feedbacktype
            };
            apiService.create("Training_Feedback/getFeedbackTrainer", data).
                then(function (promise) {
                    $scope.checkenable = false;
                    if (promise !== null) {
                        $scope.trainingFeedbacklist = promise.trainingFeedbacklist;
                        if ($scope.trainingFeedbacklist.length > 0) {
                            swal("Feedback Is Already Given");
                            $scope.temparray = [];
                            $scope.checkenable = true;
                            return;
                        }
                    } else {
                        $scope.tempoptionarray = [];
                        swal("Something Went Wrong Kidndly Contact Administrator");
                    }
                });
        };

        $scope.getFeedbackTrainee = function () {
            $scope.trainingFeedbacklist = [];
            var data = {
                "HRTCR_Id": $scope.hrtcR_Id,
                "HRMTT_Id": $scope.hrmtT_Id,
                "HRME_Id": $scope.hrmE_Id,
                "Type": $scope.feedbacktype
            };
            apiService.create("Training_Feedback/getFeedbackTrainer", data).then(function (promise) {
                $scope.checkenable = false;
                if (promise !== null) {
                    $scope.trainingFeedbacklist = promise.trainingFeedbacklist;
                    if ($scope.trainingFeedbacklist.length > 0) {
                        swal("Feedback Is Already Given");
                        $scope.temparray = [];
                        $scope.checkenable = true;
                        return;
                    }
                } else {
                    $scope.tempoptionarray = [];
                    swal("Something Went Wrong Kidndly Contact Administrator");
                }
            });
        };

        //save
        $scope.submitted = false;
        $scope.savetrainerfeedback = function () {
            $scope.question_Option = [];

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HRTCR_Id": $scope.hrtcR_Id,
                    "HRMTT_Id": $scope.hrmtT_Id,
                    "HRTFEED_TrainerId": $scope.obj.hrtfeeD_TrainerId,
                    "question_Answer": $scope.mappedquestionlist
                };
                apiService.create("Training_Feedback/savetrainerfeedback", data).then(function (promise) {
                    if (promise.returnvales !== "") {
                        if (promise.returnvales == "Update") {
                            swal('Record Updated Successfully');
                        }
                        else if (promise.returnvales == "Add") {
                            swal('Record Saved Successfully');
                        }
                        else if (promise.returnvales == "false") {
                            swal('Record Not Saved/Updated successfully');
                        }
                        else if (promise.returnvales === "Duplicate") {
                            swal('Category Already Exist');
                        }
                        $state.reload();
                    }
                    else {
                        swal('Operation Failed');
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        //save
        $scope.submitted = false;
        $scope.savetraineefeedback = function () {
            $scope.question_Option = [];
            angular.forEach($scope.question_list, function (cls) {
                if (cls.selectedd2 === true) {
                    $scope.question_Option.push(cls);
                }
            });
            $scope.submitted = true;
            if ($scope.myForm2.$valid) {
                var data = {
                    "HRTCR_Id": $scope.hrtcR_Id,
                    "HRMTT_Id": $scope.hrmtT_Id,
                    "HRME_Id": $scope.hrmE_Id,
                    "HRTFEED_Verified": $scope.HRTFEED_VerifiedFlag,
                    "question_Answer": $scope.mappedquestionlist
                };
                apiService.create("Training_Feedback/savetraineefeedback", data).
                    then(function (promise) {
                        if (promise.returnvales !== "") {
                            if (promise.returnvales == "Update") {
                                swal('Record Updated Successfully');
                            }
                            else if (promise.returnvales == "Add") {
                                swal('Record Saved Successfully');
                            }
                            else if (promise.returnvales == "false") {
                                swal('Record Not Saved/Updated successfully');
                            }
                            else if (promise.returnvales === "Duplicate") {
                                swal('Category Already Exist');
                            }
                            $state.reload();
                        }
                        else {
                            swal('Operation Failed');
                            $state.reload();
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.previewimg_new = function (img) {
            $scope.imagepreview = img;
            $scope.view_videos = [];
            var img = $scope.imagepreview;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myvideoPreview').modal('show');
            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {
                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');
            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
            }
            else if ($scope.filetype2 == 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');
            }
            else if ($scope.filetype2 == 'pdf') {
                ///=====================show pdf, img
                $('#showpdf').modal('hide');
                var imagedownload1 = "";
                imagedownload1 = $scope.imagepreview;
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
            else {
                $window.open($scope.imagepreview)
            }
        };

        $scope.Clearid = function () {
            $state.reload();
        };
    }

})();



