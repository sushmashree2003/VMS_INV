(function () {
    'use strict';
    angular.module('app').controller('Employee_WeekOffController', Employee_WeekOffController)
    Employee_WeekOffController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function Employee_WeekOffController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {


        $scope.objques = {};

        $scope.OnLoad_EmpName = function () {
            $scope.getuserName = [];
            var pageid = 1;

            apiService.getURI("Employee_WeekOff/OnLoadData", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.getuserName = promise.empData;
                  

                }
            });
        };


        //save
        $scope.submitted = false;
        $scope.SaveData = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HRME_ID": $scope.HRME_Id.hrme_id,
                    "date": $scope.ismtpL_StartDate.toISOString()

                }
                apiService.create("Employee_WeekOff/Save", data).
                    then(function (promise) {
                        if (promise.returnData === null) {
                            swal('Record Saved Successfully !');
                         }
                         else if (promise.returnData != null) {
                            swal('Record Not Saved !');
                        }
                        //if (promise.returndata === "Add") {
                        //    swal('Record Saved Successfully !');

                        //}
                        //else if (promise.returndata === "Notsave") {
                        //    swal('Record Not Saved !');

                        //}
                        //else if (promise.returndata === "Duplicate") {
                        //    swal("Record Already Exists !");

                        //}
                        else {
                            swal("Please Contact Administrator !");
                        }
                        $state.reload();
                    })

            }
            else {

                $scope.submitted = true;
            }

        };

        // clear form data
        $scope.cancel = function () {
            $scope.ismtpL_StartDate = null;
            $scope.HRME_Id = null;
           // $state.reload();
        };


        $scope.interacted2 = function (field) {
            return $scope.submitted;
        };

        $scope.interacted = function (field) {
            return $scope.submitted || (field && field.$touched);
        };


        //$scope.interacted = function (field) {
        //    return $scope.submitted;
        //};
    }
})();
