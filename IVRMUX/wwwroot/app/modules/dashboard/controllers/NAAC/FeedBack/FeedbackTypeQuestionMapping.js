(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeedbacktypequestionmappingController', FeedbacktypequestionmappingController)

    FeedbacktypequestionmappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function FeedbacktypequestionmappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {

        $scope.report = false;
        $scope.catreport = false;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length !== 0 && ivrmcofigsettings.length !== null && ivrmcofigsettings.length !== undefined) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        $scope.BindData = function () {

            var data = {
                "MI_Id": 4
            };

            apiService.create("FeedbackTypeQuestionMapping/getdetails", data).
                then(function (promise) {
                    $scope.getdetails = promise.getdetails;
                    $scope.grouptypeListOrder = promise.getdetails;
                    $scope.feedbacktype = promise.feedbacktype;
                    if ($scope.getdetails.length > 0) {
                        $scope.catreport = true;
                    }
                });
        };

        $scope.onchnagetype = function () {
            var data = {
                "FMTY_Id": $scope.FMTY_Id
            };
            apiService.create("FeedbackTypeQuestionMapping/onchnagetype", data).then(function (promise) {
                if (promise !== null) {
                    $scope.feedbackquestion1 = promise.feedbackquestions;
                    if ($scope.feedbackquestion1.length > 0) {
                        $scope.feedbackquestion = promise.feedbackquestions;
                    } else {
                        swal("No Record Found");
                    }
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.searchValue = '';

        $scope.students = [];
        $scope.catreport = false;
        $scope.submitted = false;

        $scope.onreport = function (obj) {
            $scope.albumNameArraycolumn = [];
            $scope.all = false;
            if ($scope.myForm.$valid) {

                angular.forEach($scope.feedbackquestion, function (hi) {
                    if (hi.Selected) {
                        $scope.albumNameArraycolumn.push({ FMQE_Id: hi.fmqE_Id, FMQE_FeedbackQuestions: hi.fmqE_FeedbackQuestions });
                    }
                });

                var data = {
                    "FMTY_Id": $scope.FMTY_Id,
                    "FeedbackTypeQuestionMappingTempDTO": $scope.albumNameArraycolumn
                };

                apiService.create("FeedbackTypeQuestionMapping/savedata", data).
                    then(function (promise) {

                        if (promise !== null) {

                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else {
                            swal("Something Went Wrong Kindly Contact Administrator");
                        }
                        $state.reload();
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.editdata = function (user) {
            var data = user;
            apiService.create("FeedbackTypeQuestionMapping/editdata", data).then(function (promise) {

                $scope.editdetails = promise.editdetails;

                if ($scope.editdetails.length > 0) {
                    $scope.edit = true;
                    $scope.FMTY_Id = promise.editdetails[0].fmtY_Id;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };

        $scope.deactiveY = function (item, SweetAlert) {

            $scope.FMTQ_Id = item.fmtQ_Id;

            var dystring = "";
            if (item.fmtQ_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (item.fmtQ_ActiveFlag === false) {
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
                        apiService.create("FeedbackTypeQuestionMapping/activedeactive", item).
                            then(function (promise) {
                                if (promise.message === "Mapped") {
                                    swal("You Can Not Deactivate The Record Its Already Mapped");
                                } else {
                                    if (promise.returnval === true) {
                                        swal("Record " + dystring + "Successfully");
                                    }
                                    else {
                                        swal("Record Not " + dystring + "Successfully");
                                    }
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Cancel");
                    }
                });
        };

        $scope.getOrder = function (orderarray) {
            var data = {
                "FeedbackTypeQuestionMappingTemporderDTO": orderarray
            };

            apiService.create("FeedbackTypeQuestionMapping/getorder", data).
                then(function (promise) {
                    if (promise.returnval === true) {
                        swal("Record Updated Successfully");
                    } else {
                        swal("Failed To Update Record");
                    }
                    $state.reload();
                });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.feedbackquestion.some(function (options) {
                return options.Selected;
            });
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.grouptypeListOrder) {
                    $scope.grouptypeListOrder[index].fmoP_FOOrder = Number(index) + 1;

                }
            }
        };
    }

})();