

(function () {
    'use strict';
    angular
        .module('app')
        .controller('RoomBankDeatilsController', RoomBankDeatilsController)
    RoomBankDeatilsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile']
    function RoomBankDeatilsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile) {
        $scope.editflag = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0
        }
        $scope.obj = {};
        $scope.stdall = false;
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;

            apiService.getURI("RoomBankDeatils/getloaddata", pageid).then(function (promise) {

                    $scope.rooms = promise.rooms;
                    $scope.banklist = promise.banklist;
                    $scope.get_levl = promise.get_levl;
                    $scope.presentCountgrid = $scope.get_levl.length;

            });
        };



        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HRMRBD_Id": $scope.HRMRBD_Id,
                    "HRMR_Id": $scope.HRMR_Id,
                    "HRMBD_Id": $scope.HRMBD_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("RoomBankDeatils/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.hrmrbD_Id == 0 || promise.hrmrbD_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.hrmrbD_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnval == false) {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.hrmrbD_Id == 0 || promise.hrmrbD_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.tenantagrowN_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.deactive = function (item, SweetAlert) {            $scope.HRMRBD_Id = item.hrmrbD_Id;            var dystring = "";            if (item.hrmrbD_ActiveFlag == true) {                dystring = "Deactivate";            }            else if (item.hrmrbD_ActiveFlag == false) {                dystring = "Activate";            }            swal({                title: "Are you sure?",                text: "Do You Want To " + dystring + " Record?",                type: "warning",                showCancelButton: true,                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",                cancelButtonText: "Cancel",                closeOnConfirm: false,                closeOnCancel: false            },                function (isConfirm) {                    if (isConfirm) {                        apiService.create("RoomBankDeatils/deactive", item).                            then(function (promise) {                                if (promise.returnval == true) {                                    swal("Record " + dystring + "d Successfully!!!");                                }                                else {                                    swal("Record Not " + dystring + "d !!!");                                }                                $state.reload();                            })                    }                    else {                        swal("Record " + dystring + " Cancelled!!!");                    }                });        }


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';


        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.Clearid = function () {
            $state.reload();
        };


        $scope.edit = function (item) {
            $scope.HRMRBD_Id = item.hrmrbD_Id;
            $scope.HRMR_Id = item.hrmR_Id;        
            $scope.HRMBD_Id = item.hrmbD_Id;     
            $scope.editflag = true;
        };
    }
})();
