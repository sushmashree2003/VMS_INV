(function () {
    'use strict';
    angular.module('app').controller('masterActivitieController', masterActivitieController)

    masterActivitieController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function masterActivitieController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {

        $scope.submitted = false;

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        };

        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("MasterActivities/onloaddata", pageid).then(function (promise) {
                $scope.getloadgrid = promise.getloaddetails;
            });
        };

        $scope.saverecord = function () {
            if ($scope.myForm.$valid) {
                $scope.uploadarray = [];
                var data = {
                    "HR_PR_NAME": $scope.activities,
                    "HR_PR_DESC": $scope.discription,
                };

                apiService.create("MasterActivities/saverecord", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message === "Duplicate") {
                            swal("Record Already Exists");
                        } else if (promise.message === "Add") {
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
                        } else if (promise.message === "Error") {
                            swal("Failed To Save/ Update Record");
                        } else {
                            swal("Something Went Wrong Contact Administrator");
                        }
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.EditData = function (indl) {
            $scope.HREXTTRN_Id = indl.hrexttrN_Id;
            $scope.HRMETRCEN_Id = indl.hrmetrceN_Id;
            $scope.HRMETRTY_Id = indl.hrmetrtY_Id;
            $scope.HREXTTRN_TrainingTopic = indl.hrexttrN_TrainingTopic;
            $scope.HREXTTRN_StartTime = indl.hrexttrN_StartTime;
            $scope.HREXTTRN_EndTime = indl.hrexttrN_EndTime;
            $scope.HREXTTRN_TotalHrs = indl.hrexttrN_TotalHrs;
            $scope.HREXTTRN_StartDate = indl.hrexttrN_StartDate;
            $scope.HREXTTRN_EndDate = indl.hrexttrN_EndDate;
            $scope.FilePath_Array = $scope.uploadarray;

            $scope.scroll();
        };
        $scope.deactiveY = function (user, SweetAlert) {
            var data = {
                "HREXTTRN_Id": user.hrexttrN_Id
            };
            var dystring = "";
            if (user.hrexttrN_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (user.hrexttrN_ActiveFlag === false) {
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
                        apiService.create("MasterActivities/deactiveY", data).then(function (promise) {
                            if (promise.message === "Mapped") {
                                swal("Record Is Already Mapped, So You Can Not Deactive The Record");
                            } else {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            }
                        });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }
                });
        };








        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();

