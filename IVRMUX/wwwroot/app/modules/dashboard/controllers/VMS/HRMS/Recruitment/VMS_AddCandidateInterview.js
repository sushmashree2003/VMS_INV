(function () {
    'use strict';
    angular
        .module('app')
        .controller('vmsAddCandidateInterviewController', vmsAddCandidateInterviewController);

    vmsAddCandidateInterviewController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter'];
    function vmsAddCandidateInterviewController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter) {

        $scope.addjob = {};

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("AddCandidateInterviewVMS/getalldetails", pageid).then(function (promise) {

                if (promise.candidateDetailsList !== null && promise.candidateDetailsList.length > 0) {
                    $scope.candidateDetailsList = promise.candidateDetailsList;
                }

                if (promise.interviewerList !== null && promise.interviewerList.length > 0) {
                    $scope.interviewerList = promise.interviewerList;
                }
            });            
        };

        // clear form data
        $scope.cancel = function () {
            $scope.mrfReq = "";
        };

        // Edit Single Record
        $scope.EditData = function (record) {
            var id = record.hrcisC_Id;
            apiService.getURI("AddCandidateInterviewVMS/editRecord", id).
                then(function (promise) {

                });
        };

        $scope.savejob = function () {
            if ($scope.myForm.$valid) {

                var datevar = $scope.mrfReq.hrcisC_InterviewDateTime == null ? "" : $filter('date')($scope.mrfReq.hrcisC_InterviewDateTime, "yyyy-MM-dd");
                $scope.mrfReq.hrcisC_InterviewDateTime = datevar;
                $scope.mrfReq.hrcD_Id = $scope.hrcD_Id.hrcD_Id;
                $scope.mrfReq.hrcisC_Interviewer = $scope.hrcisC_Interviewer.id;
                var data = $scope.mrfReq;


                if ($scope.mrfReq.hrcisC_InterviewRounds ==='Online Test') {
                    var data1 = {
                        "HRCD_Id": $scope.hrcD_Id.hrcD_Id
                    }
                    apiService.create("registration/candidateusercreation", data1).
                        then(function (promise) {
                            if (promise.returnMsg === "created") {

                            $scope.mrfReq.hrcisC_OnlineTestUserName = promise.username;
                            $scope.mrfReq.hrcisC_OnlineTestPassword = promise.password;
                            var data2 = $scope.mrfReq;
                            apiService.create("AddCandidateInterviewVMS/", data2).
                                then(function (promise) {
                                    if (promise.retrunMsg !== "") {
                                        if (promise.retrunMsg === "Duplicate") {
                                            swal("Record already exist..!!");
                                            return;
                                        }
                                        else if (promise.retrunMsg === "NQ") {
                                            swal("Question Paper is Not set to the Candidate Position ");
                                            $scope.cancel();
                                        }
                                        else if (promise.retrunMsg === "false") {
                                            swal("Record Not saved / Updated..", 'Fail');
                                        }
                                        else if (promise.retrunMsg === "Add") {
                                            swal("Interview Scheduled Successfully.");
                                            $scope.cancel();
                                        }
                                        else if (promise.retrunMsg === "Update") {
                                            swal("Interview Updated Successfully.");
                                            $scope.cancel();
                                        }
                                        else if (promise.retrunMsg === "Notification") {
                                            swal("Notification!");
                                            $scope.cancel();
                                        }
                                        else {
                                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                                        }
                                        $state.reload();
                                    }
                                });
                        }
                           
                        });
                }
               
                else {
 apiService.create("AddCandidateInterviewVMS/", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg === "NQ") {
                                swal("Question Paper is Not set to the Candidate Position ");
                                $scope.cancel();
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Interview Scheduled Successfully.");
                                $scope.cancel();
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Interview Updated Successfully.");
                                $scope.cancel();
                            }
                            else if (promise.retrunMsg === "Notification") {
                                swal("Notification!");
                                $scope.cancel();
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                            }
                            $state.reload();
                        }
                    });
                }


               

               
            }            
        };
    }

})();