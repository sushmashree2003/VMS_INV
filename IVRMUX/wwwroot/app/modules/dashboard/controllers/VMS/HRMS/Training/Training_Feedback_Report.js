(function () {
    'use strict';
    angular
        .module('app')
        .controller('TrainingFeedbackReportController', TrainingFeedbackReportController)

    TrainingFeedbackReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function TrainingFeedbackReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.sortKey = 'HRTRQNS_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        //get data
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("Training_Feedback/reportloaddata", pageid)
                .then(function (promise) {
                    $scope.traninglisttrainer = promise.traninglisttrainer;
                    $scope.traninglisttrainee = promise.traninglisttrainee;
                    if (promise.trainerprogdetails != null && promise.trainerprogdetails.length > 0) {
                        $scope.trainerprogdetails = promise.trainerprogdetails;
                    }

                    if (promise.traineeprogdetails != null && promise.traineeprogdetails.length > 0) {
                        $scope.traineeprogdetails = promise.traineeprogdetails;
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
                "Type": $scope.feedbacktype
            };
            apiService.create("Training_Feedback/getQuestions", data).
                then(function (promise) {

                    if (promise !== null) {                      
                            $scope.trainerlist = promise.trainerlist;
                            $scope.traineelist = promise.traineelist;
                      
                        } else {
                            $scope.tempoptionarray = [];
                            swal("No Records Found");
                        }
                    
                });
        };

        //Report
       
        $scope.submitted = false;
        $scope.trainerfeedbackreport = function () {
            $scope.feedbackreportlist = [];
            
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                var data = {
                    "HRTCR_Id": $scope.hrtcR_Id,
                    "HRMTT_Id": $scope.hrmtT_Id,
                    "HRTFEED_TrainerId": $scope.hrtfeeD_TrainerId,
                    "Type": $scope.feedbacktype
                };
                apiService.create("Training_Feedback/reportpagetrainerfeedbackreport", data).
                    then(function (promise) {
                        if (promise.getfeedbackquestions != null && promise.getfeedbackquestions.length > 0) {
                            $scope.getfeedbackquestions = promise.getfeedbackquestions;                                                                         
                        }
                        else{
                            swal("No Records Found");
                            $state.reload;
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        //save
        $scope.submitted = false;
        $scope.traineefeedbackreport = function () {
            $scope.submitted = true;
            if ($scope.myForm2.$valid) {
                var data = {
                    "HRTCR_Id": $scope.hrtcR_Id,
                    "HRME_Id": $scope.hrmE_Id,
                    "HRMTT_Id": $scope.hrmtT_Id,
                    "Type": $scope.feedbacktype
                };
                apiService.create("Training_Feedback/reportpagetrainerfeedbackreport", data).
                    then(function (promise) {
                        if (promise.getfeedbackquestions != null && promise.getfeedbackquestions.length > 0) {
                            $scope.getfeedbackquestions = promise.getfeedbackquestions;
                        }
                        else
                        {
                            swal("No Records Found");
                            $state.reload;
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.Clearid = function () {
            $state.reload();
        };
    }

})();



