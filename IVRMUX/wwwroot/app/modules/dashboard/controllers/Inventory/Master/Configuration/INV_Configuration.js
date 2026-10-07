
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_ConfigurationController', INV_ConfigurationController);
    INV_ConfigurationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function INV_ConfigurationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //-------------------------------------------------------------------

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("INV_Configuration/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_store = promise.get_store;
                    $scope.get_configdetails = promise.get_configdetails;
                });
        };


        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.get_store, function (uem) {
                uem.storeck = toggleStatus;
            });
        };
        $scope.togchkbx = function () {
            $scope.checkall = $scope.get_store.every(function (options) {
                return options.storeck;
            });
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.isOptionsRequired = function () {
            return !$scope.get_store.some(function (options) {
                return options.storeck;
            });
        };
        $scope.submitted = false;
        //********************************** SAVE DATA
        $scope.savedata = function () {
            $scope.submitted = true;
            $scope.selectedStore = [];
            if ($scope.myForm.$valid) {                
                angular.forEach($scope.get_store, function (str) {
                    if (str.storeck === true) {
                        $scope.selectedStore.push(str);
                    }
                });
                var INVC_Id = 0;
                if ($scope.INVC_Id > 0) {
                    INVC_Id = $scope.INVC_Id;
                }
                var data = {
                    "INVC_Id": INVC_Id,                 
                    "selectedStore": $scope.selectedStore,
                    "INVC_LIFOFIFOFlg": $scope.INVC_LIFOFIFOFlg,
                    "INVC_ProcessApplFlg": $scope.invC_ProcessApplFlg,
                    "INVC_PRApplicableFlg": $scope.invC_PRApplicableFlg
                };             
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("INV_Configuration/savedetails", data).then(function (promise) {

                    if (promise.returnval == "save") {
                        swal("Record Saved Successfully duplicate count " + promise.invC_AlertsBeforeDays +"");
                    }
                     if (promise.returnval == "notsave") {
                         swal("Record Not Saved ! duplicate count " + promise.invC_AlertsBeforeDays +" ");
                    }
                    if (promise.returnval == "update") {
                        swal("Record update Successfully duplicate count " + promise.invC_AlertsBeforeDays +" ");
                    }
                    if (promise.returnval == "notupdate") {
                        swal("Record Not update   ! duplicate count " + promise.invC_AlertsBeforeDays +"");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.Clearid = function () {
            $state.reload();
        }

    }
})();