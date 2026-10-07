(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmployeewiseFeedbackController', EmployeewiseFeedbackController);
    EmployeewiseFeedbackController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache', 'myFactoryvisitor', '$q']
    function EmployeewiseFeedbackController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache, myFactoryvisitor, $q) {
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.itemsPerPage1 = 10;
        $scope.feedback = false;
        $scope.feedbacksave = false;
        //----------onload function----------//
        $scope.BindData = function () {

            var pageid = 2;
            apiService.getURI("Employee_Wise_VMS_FeedBack/GetMonthYear", pageid).then(function (promise) {

                if (promise !== null) {
                    $scope.months = promise.monthnames;
                    $scope.years = promise.masteryear;
                    $scope.feedback = false;
                    $scope.feedbacksave = false;
                }
                else {
                    swal("No Data Found !");
                }
            })


        
        };        $scope.clientdata = function () {            var pageid = 10;
            var data = {                "pageid": 10,                "year": $scope.f_Year,                "monthname": $scope.r_month            }
            apiService.create("Employee_Wise_VMS_FeedBack/employeefeedback", data).then(function (promise) {
                if (promise.getloaddata.length > 0 && promise.getloaddata != null ) {

                    $scope.feedback = true;
                    $scope.feedbacksave = true;
                    $scope.getloaddata = promise.getloaddata; 
                    $scope.feedbacklist = promise.feedbacklist;

                }
                else if (promise.feedbacklist.length > 0 && promise.feedbacklist != null)
                {
                    if (!promise.getloaddata || promise.getloaddata.length === 0)
                    {
                        $scope.feedback = false;
                        swal("No HealthCheckUp Data To Give Feedback");
                    }
                    $scope.feedbacklist = promise.feedbacklist;
                    $scope.feedbacksave = true;
                }
                else {
                    $scope.feedback = false;
                    $scope.feedbacksave = false;
                    swal("No Details Found !", "", "warning");
                }
            })
        }                //save call center data        $scope.submitted = false;        $scope.savecallcenter = function () {            $scope.CallFeedback_List = [];            if ($scope.myForm.$valid) {                if ($scope.getloaddata != null && $scope.getloaddata.length > 0) {                    angular.forEach($scope.getloaddata, function (ss) {                        if (ss.usercheck == true) {                            $scope.CallFeedback_List.push({                                HRME_Id: ss.HRME_Id,                                VEF_Remarks: ss.remarks,                                VEF_NegativeFlag: ss.VEF_NegativeFlag,                                ISMMCLT_ClientName: ss.ISMMCLT_ClientName,                                ISMMCLT_Id :ss.ISMMCLT_Id                            });                        }                    })                }                var data = {                    "CallFeedback_List": $scope.CallFeedback_List,                }                apiService.create("Employee_Wise_VMS_FeedBack/savecallcenter", data).then(function (promise) {                    if (promise.returnmsg == "saved") {                        swal('FeedBack Saved Successfully', "", "success");                    }                    else if (promise.returnmsg == "") {                        swal("failed to save", "", "error");                    }                    else {                        swal('Please Contcat Administrator');                    }                    $state.reload();                });            }            else {                $scope.submitted = true;            }            $scope.interacted = function (field) {
                return $scope.submitted;
            };        };    }
})();