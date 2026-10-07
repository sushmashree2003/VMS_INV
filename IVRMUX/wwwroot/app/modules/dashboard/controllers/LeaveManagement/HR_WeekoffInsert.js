(function () {
    'use strict';
    angular
        .module('app')
        .controller('Employee_WeekOffController', Employee_WeekOffController)

    Employee_WeekOffController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$filter']
    function Employee_WeekOffController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $filter) {


        $scope.loaddata = function () {
            $scope.page1 = "page1";
            $scope.page2 = "page2";
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            var pageid = 1;
            apiService.getURI("MasterHoliday/advloaddata", pageid).
                then(function (promise) {
                    $scope.employeelist = promise.employeelist;
                    $scope.griddata = promise.gridviewdetails;

                });
        };


        $scope.search = '';


        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        $scope.sort1 = function (keyname) {
            $scope.sortKey1 = keyname;   //set the sortKey to the param passed
            $scope.reverse1 = !$scope.reverse; //if true make it false and vice versa
        };



        $scope.savedata = function () {

            if ($scope.myForm.$valid) {

                var data = {
                    "HRME_Id": $scope.obj.HRME_Id.hrmE_Id,
                    "FOMEH_Date": new Date($scope.FromDate).toDateString()
                };

                apiService.create("MasterHoliday/savedataWeekoff", data).
                    then(function (promise) {
                        if (promise.message === "Success") {
                            swal('Record saved successfully.');
                            $state.reload();
                        }
                        else if (promise.message === "NotSuccess") {
                            swal('Failed to save.');
                            $state.reload();
                        }
                    })
            }
            else {
                $scope.submitted = true;

            }

        };

        $scope.clear1 = function () {
            $state.reload();
            $scope.loaddata();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };



        $scope.Editdata = function (employee) {
            $scope.editEmployee = employee.vmsmdsH_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("MasterHoliday/EditData", pageid).
                then(function (promise) {
                    $scope.VMSMDSH_Id = promise.edit_array[0].vmsmdsH_Id;
                    $scope.VMSMDSH_Remarks = promise.edit_array[0].vmsmdsH_Remarks;

                })
        }


    }
})();