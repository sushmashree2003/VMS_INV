(function () {
    'use strict';
    angular
        .module('app')
        .controller('Employee_Wise_VMS_FeedBackController', Employee_Wise_VMS_FeedBackController);
    Employee_Wise_VMS_FeedBackController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache', 'myFactoryvisitor', '$q']
    function Employee_Wise_VMS_FeedBackController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache, myFactoryvisitor, $q) {
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.itemsPerPage1 = 10;
        //----------onload function----------//
        $scope.BindData = function () {
            
            var pageid = 10;
            var data = {                "pageid":10,            }
            apiService.create("Employee_Wise_VMS_FeedBack/getfeedback", data).then(function (promise) {
                    if (promise.employee_list.length > 0 && promise.employee_list !=null ) {
                        $scope.employee_list = promise.employee_list;
                        $scope.feedbackdata = promise.feedbackdata;
                    }
                    else {
                        swal("No Employees Found !","","warning");
                    }
                })
        };        //SaveData        $scope.submitted = false;        $scope.savetmpldata = function () {            $scope.Feedback_list = [];            if ($scope.myForm.$valid) {                if ($scope.employee_list != null && $scope.employee_list.length > 0) {                    angular.forEach($scope.employee_list, function (ss) {                        if (ss.usercheck == true) {                            $scope.Feedback_list.push({                                HRME_Id: ss.HRME_Id,                                VEF_Remarks: ss.remarks,                                VEF_NegativeFlag: ss.VEF_NegativeFlag                            });                        }                    })                }                var data ={                    "Feedback_list": $scope.Feedback_list,                }                apiService.create("Employee_Wise_VMS_FeedBack/Savedata", data).then(function (promise) {                    if (promise.saved == "saved") {                        swal('FeedBack Saved Successfully', "", "success");                    }                    else if (promise.saved == "") {                        swal("failed to save","","error");                    }                    else   {                        swal('Please Contcat Administrator');                    }                    $state.reload();                });            }            else {                $scope.submitted = true;            }            $scope.interacted = function (field) {
                return $scope.submitted;
            };        };    }
})();