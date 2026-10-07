(function () {
    'use strict';
    angular.module('app').controller('Rating_TransactionController', Rating_TransactionController)

    Rating_TransactionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$timeout', 'Excel']
    function Rating_TransactionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $timeout, Excel) {

        $scope.obj = {};
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";

            var data = {
                "FlagOnLoadOrOnChangeInst": "OnLoad"
            };
            apiService.create("Rating_Transaction/OnLoadRatingTransaction", data).then(function (promise) {

                
                $scope.getRatingType = promise.getRatingType;
                $scope.OverAllRatingGrid = promise.overAllTypeGrid;

            });
        };

        //getquestion
        $scope.getquestion = function () {
            $scope.typeid = 0;
            
            //angular.forEach($scope.getRatingType, function (d) {
            //    if ($scope.RATMT_Id == d.ratmT_Id) {
            //        $scope.typeid = d.ratmT_Id;
                   
            //    }
            //});

            var data = {
                "RATMT_Id": $scope.RATMT_Id.ratmT_Id
            };
            apiService.create("Rating_Transaction/getQuestions", data).then(function (promise) {

                
                $scope.getRatingQuestion = promise.getRatingQuestion;
               

            });
        }

        //gettypequestion
        $scope.gettypequestion = function () {

            var data = {
                "RATMT_Id": $scope.RATMT_Id.ratmT_Id,
                "RATMQE_Id": $scope.RATMQE_Id.ratmqE_Id
            };
            apiService.create("Rating_Transaction/getTypeQuestions", data).then(function (promise) {


                $scope.getRatingTypeQuestion = promise.getRatingTypeQuestion;


            });
        }




        

        $scope.totext = function () {
            $scope.tovalidsshow = 0;
            //if ($scope.from == 0) {
            //    $scope.tovalidshow = false;
            //    $scope.to = '';
            //}
            if ($scope.to.length >= $scope.from.length) {

                if ($scope.to >= $scope.from) {

                    scope.tovalidsshow = 0;

                } else if ($scope.to <= $scope.from) {
                    $scope.tovalidsshow = 1;
                    $scope.to = '';

                }

            }
            

        }




        $scope.submitted = false;
        $scope.savedata = function () {

            //$scope.RATMT_Id = 0;
            //$scope.RATMQE_Id = 0;
            //angular.forEach($scope.getRatingTypeQuestion, function (d) {
            //    if ($scope.RATMTQ_Id == d.ratmtQ_Id) {
            //        $scope.RATMT_Id = d.ratmT_Id;
            //        $scope.RATMQE_Id = d.ratmqE_Id
            //    }
            //});


            if ($scope.myForm.$valid) {
                var data = {
                    "RATMTQR_FromValue": $scope.from,
                    "RATMTQR_ToValue": $scope.to,
                    "RATMTQR_Rating": $scope.rating,
                    "RATMTQR_Id": $scope.RATMTQR_Id,
                    "RATMT_Id": $scope.RATMT_Id.ratmT_Id,
                    "RATMQE_Id": $scope.RATMQE_Id.ratmqE_Id,
                    "RATMTQ_Id": $scope.RATMTQ_Id.ratmtQ_Id
                };

                apiService.create("Rating_Transaction/SaveOverAllRating", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnmsg === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.returnmsg === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else {
                            swal("Failed To Update/Save Record");
                        }

                    }

                });
                $scope.Loaddata();
                $scope.from = ''
                $scope.to = ''
                $scope.RATMTQR_Id = ''
                $scope.rating = ''
                $state.reload();
            } else {
                $scope.submitted = true;
            }

        };





        $scope.interacted = function (field) {
            return $scope.submitted;
        };



        $scope.Clearid = function () {
            $state.reload();
        };



        $scope.EditOverAll = function (obj) {
            $scope.getRatingQuestion = []; $scope.getRatingTypeQuestion = [];
            var data = {
                "RATMTQR_Id": obj.ratmtqR_Id,
                "RATMT_Id": obj.ratmT_Id,
                "RATMQE_Id": obj.ratmqE_Id,
                "RATMTQ_Id": obj.ratmtQ_Id
            };

            apiService.create("Rating_Transaction/OverAllEdit", data).then(function (promise) {
                if (promise !== null) {
                    
                    $scope.RATMTQ_Id = {};
                    $scope.RATMT_Id = {};
                    $scope.RATMQE_Id = {};

                    $scope.getRatingType = promise.getRatingType;
                    $scope.getRatingQuestion = promise.getRatingQuestion;
                    $scope.getRatingTypeQuestion = promise.getRatingTypeQuestion;
                    
                    $scope.from = promise.editDataOverAll[0].ratmtqR_FromValue;
                    $scope.to = promise.editDataOverAll[0].ratmtqR_ToValue;
                    $scope.RATMTQR_Id = promise.editDataOverAll[0].ratmtqR_Id;
                    $scope.rating = promise.editDataOverAll[0].ratmtqR_Rating;

                   // $scope.RATMT_Id.ratmT_Id = promise.editDataOverAll[0].ratmT_Id;
                    //$scope.RATMQE_Id.ratmqE_Id = promise.editDataOverAll[0].ratmqE_Id;
                    //$scope.RATMTQ_Id.ratmtQ_Id = promise.editDataOverAll[0].ratmtQ_Id;
                    $scope.newcaste = {};
                    $scope.newcastwo = {};
                    $scope.newcasthree = {};
                    if ($scope.getRatingType != null && $scope.getRatingType.length > 0) {
                        for (var i = 0; i < $scope.getRatingType.length; i++) {
                            if (promise.editDataOverAll[0].ratmT_Id == $scope.getRatingType[i].ratmT_Id) {
                                $scope.getRatingQuestion[i].Selected = true;
                                $scope.RATMT_Id = $scope.getRatingType[i];
                                $scope.newcasthree = $scope.getRatingType[0].ratmT_Id;
                            }
                        }
                    }
                  
                    if ($scope.getRatingQuestion != null && $scope.getRatingQuestion.length > 0) {
                        for (var i = 0; i < $scope.getRatingQuestion.length; i++) {
                            if (promise.editDataOverAll[0].ratmqE_Id == $scope.getRatingQuestion[i].ratmqE_Id) {
                                $scope.getRatingQuestion[i].Selected = true;
                                $scope.RATMQE_Id = $scope.getRatingQuestion[i];
                                $scope.newcastwo = $scope.getRatingQuestion[0].ratmqE_Id;
                            }
                        }
                    }
                  
                    if ($scope.getRatingTypeQuestion != null && $scope.getRatingTypeQuestion.length > 0) {
                        for (var i = 0; i < $scope.getRatingTypeQuestion.length; i++) {
                            if (promise.editDataOverAll[0].ratmtQ_Id == $scope.getRatingTypeQuestion[i].ratmtQ_Id) {
                                $scope.getRatingTypeQuestion[i].Selected = true;
                                $scope.RATMTQ_Id = $scope.getRatingTypeQuestion[i];
                                $scope.newcaste = $scope.getRatingTypeQuestion[0].ratmtQ_Id;
                            }
                        }
                    }
                   
                   
                }
            });
        };

        $scope.OverAllActiveDeactive = function (obj) {

            var mgs = "";
            var confirmmgs = "";
            if (obj.ratmtqR_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }

            var data = {
                "RATMTQR_Id": obj.ratmtqR_Id,
            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Rating_Transaction/OverAllRatingActiveDeactive", data).then(function (promise) {
                            if (promise.returnmsg === "Mapped") {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                            }
                            $scope.Loaddata();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };



    }
})();
