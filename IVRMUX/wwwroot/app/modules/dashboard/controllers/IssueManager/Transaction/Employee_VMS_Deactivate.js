(function () {
    'use strict';
    angular
        .module('app')
        .controller('Employee_VMS_DeactivateController', Employee_VMS_DeactivateController);
    Employee_VMS_DeactivateController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function Employee_VMS_DeactivateController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache) {

        //var paginationformasters;
        //var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings.length > 0) {
        //    paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        //}
        //else {
        //    paginationformasters = 0;
        //}
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        $scope.obj = {};
        $scope.responsetrans = 0;
        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.typeFlg = "Default";
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();
        $scope.ismtcrreS_ResponseDate = new Date();
        $scope.currentDate = new Date();

        var options = [];


        $scope.loaddata = function () {
            $scope.get_employeelist = [];
            var pageid = 2;
            apiService.getURI("Employee_VMS_Deactivate/Onloaddata", pageid).then(function (promise) {           

                $scope.get_employeelist = promise.get_employeelist;               


            });           
        };



        //======================================== Search List
        $scope.Savedata = function () {
            var temp_array = [];
            angular.forEach($scope.get_employeelist, function (row) {                
                    temp_array.push({
                        HRME_Id: row.HRME_ID,                       
                        Activeflag: row.Activeflag,                       
                    });              
            });

            if ($scope.myForm.$valid) {

                debugger;
                var data = {
                    employeedetails: temp_array                   
                };
                apiService.create("Employee_VMS_Deactivate/SaveData", data).
                    then(function (promise) {
                        if (promise.returnmsg == "Add") {
                            swal("Record Updated Succussfully")
                            $state.reload();
                        }
                        else {
                            swal("Record Not Updated Succussfully")
                        }
                    });
            }
            else {
                $scope.submitted1 = true;
            }
        };
        $scope.rowsSelected = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.getemployeedetails, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.rowsSelected.indexOf(itm) === -1) {
                        $scope.rowsSelected.push(itm);
                    }
                }
                else {
                    $scope.rowsSelected.splice(itm);
                }
            });
        }

        $scope.isOptionsRequired = function () {
            return !$scope.getemployeedetails.some(function (options) {
                return options.selected;
            });
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.getemp = function (leave) {
            if (leave.selected) {
                $scope.rowsSelected.push(leave);
            }
            else {
                const ind = $scope.rowsSelected.indexOf(leave);
                if (ind > -1) {
                    $scope.rowsSelected.splice(ind, 1);
                }
            }
        }

      

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
        //===========Clear Field
        $scope.cancel = function () {
            $state.reload();
        };

    }
})();

