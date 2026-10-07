(function () {
    'use strict';
    angular.module('app').controller('ModuleFeedbackController', ModuleFeedbackController)
    ModuleFeedbackController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$cookieStore', '$q', 'superCache', '$sce', 'Excel', '$timeout', '$window']
    function ModuleFeedbackController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $cookieStore, $q, superCache, $sce, Excel, $timeout, $window) {
        $scope.submitted = false;
        //$scope.ModuleName = '';
        $scope.moudleid = '';
        $scope.useronloadfeedback = [];
        $scope.modulefeedback = null;
        $scope.loadmoudulename = function () {
            $scope.moudule = $cookieStore.get("ModuleName");
        }
        $scope.onloadfeedback = function () {
            var info = {
                "pageid": 10
            }
            apiService.create("ModuleFeedback/getfeedback", info).then(function (promise) {
                if (promise.useronloadfeedback.length > 0 && promise.useronloadfeedback != null) {
                    $scope.useronloadfeedback = promise.useronloadfeedback;
                    $scope.useronloadfeedback = $scope.useronloadfeedback.reverse();
                }

            })
        }
        $scope.submitted = false;
        $scope.obj = {};
        $scope.submitfeedbackx = function () {
            if ($scope.myForm2.$valid) {
                $scope.moudule = $cookieStore.get("ModuleName");
                $scope.moudleid = $cookieStore.get("mouduleid");
                if (($scope.ModuleName == " " && $scope.ModuleName == null) || $scope.ModuleName == "Dashboard") {

                    swal("Kindly Go Inside Module And Then Give Your FeedBack.", "", "info");
                }
                else {
                    var info = {
                        "feedbacktype": $scope.modulefeedback,
                        "VMF_Remarks": $scope.suggestion,
                        "VMF_ModuleName": $scope.moudule,
                        "IVRMM_Id": $scope.moudleid,
                        "createtask": $scope.createtask
                    }
                    if ($scope.createtask) {
                        info.BugOREnh = $scope.obj.BugOREnh;
                    }

                    apiService.create("ModuleFeedback/submitfeedback", info).then(function (promise) {
                        if (promise.saved == "saved") {
                            if ($scope.createtask) {

                                swal("Feed Back Saved Successfully", "Task Was Generated For The FeedBack", "success");
                                $scope.onloadfeedback();
                                $window.location.href = 'http://localhost:57606/#/app/ISMDashboard/';
                                return;
                            }
                            else {
                                swal("Feed Back Saved Successfully", "", "success");
                                $state.onloadfeedback();
                                $window.location.href = 'http://localhost:57606/#/app/ISMDashboard/';
                                $scope.onloadfeedback();
                                return;
                            }

                        }
                        else if (promise.existfeedback == "exist") {
                            swal("Feedback has already been submitted for the current month.", "Kindly give feedback for other Modules.", "info");
                            // $state.onloadfeedback();
                        }
                        else if (promise.wrongfeedback == "cannotgivefeedback") {
                            swal("You Cannot Give Feedback To Selected Module.", "", "error");
                            $scope.loadmoudulename();
                        }
                        else {
                            swal("Failed To Save !", "", "error");
                            $state.reload();

                        }
                    })
                }
            }
            else {
                $scope.submitted = true;
            }

        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();