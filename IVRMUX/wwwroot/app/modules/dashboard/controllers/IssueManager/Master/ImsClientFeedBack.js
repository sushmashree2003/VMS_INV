(function () {
    'use strict';
    angular.module('app').controller('ImsClientController', ImsClientController)

    ImsClientController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function ImsClientController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;
        $scope.search2 = "";

        $scope.sort = function (keyname) {
            $scope.sortKey2 = keyname;   //set the sortKey to the param passed
            $scope.reverse2 = !$scope.reverse2; //if true make it false and vice versa
        };

        $scope.clientgrid = [];
        //Load data
        $scope.Loaddata = function () {
            var pageid = 2;
            apiService.getURI("ImsClient/getdetails", pageid).then(function (promise) {

                $scope.get_clentlist = promise.arrayclientlist;

                if (promise.clientgrid != null && promise.clientgrid.length > 0) {
                    $scope.clientgrid = promise.clientgrid;
                }
            });
        };

        //CheckAll
        $scope.toggleAllgrp = function () {
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.get_clentlist, function (itm) {
                itm.checkselected = toggleStatus;                
            });           
        }
        $scope.optionToggledcheckall = function () {
            $scope.checkall = $scope.get_clentlist.every(function (itm) { return itm.grpselected; });                   
        }

        //SaveData
        $scope.submitted = false;
        $scope.saveClientfeedback = function () {
            $scope.multipleclientstemp = [];
            if ($scope.myForm.$valid) {
                if ($scope.get_clentlist != null && $scope.get_clentlist.length > 0) {
                    angular.forEach($scope.get_clentlist, function (ss) {
                        if (ss.checkselected == true) {
                            $scope.multipleclientstemp.push({
                                ISMMCLT_Id: ss.ISMMCLT_Id,
                                ISMMCF_Remarks: ss.ISMMCF_Remarks,
                                ISMMCF_Negative: ss.ISMMCF_Negative
                            });
                        }
                    })
                }                             

                if ($scope.multipleclientstemp != null && $scope.multipleclientstemp.length > 0) {
                    var ISMMCF_Id = 0;
                    if ($scope.ISMMCF_Id > 0) {
                        ISMMCF_Id = $scope.ISMMCF_Id;
                    }

                    var data = {

                        "multipleclients": $scope.multipleclientstemp,
                        "ISMMCF_Id": ISMMCF_Id
                    }
                }
                else {
                    swal("Please Select  Atleast One Check Box !");
                    return;
                }

                apiService.create("ImsClient/savefeedback", data).then(function (promise) {
                    if (promise.message == "Saved") {
                        swal('Record Saved Successfully ');
                    }
                    else if (promise.message == "FailedSave") {
                        swal('Record Failed ! ');
                    }
                    else if (promise.message == "admin") {
                        swal('Please Contcat Administrator');
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.ISMMCF_Id = "";
        //Editrecord
        $scope.editdata = function (user1) {                   
            $scope.ISMMCF_Id = "";
            $scope.ISMMCF_Id = user1.ISMMCF_Id;

            var data = {
                "ISMMCLT_Id": user1.ISMMCLT_Id,
                "ISMMCF_Id": user1.ISMMCF_Id,
            };

            apiService.create("ImsClient/editdata", data).then(function (promise) {              
                $scope.get_clentlist = [];
                $scope.get_clentlist = promise.editClient;
                $scope.checkall = true;
                angular.forEach($scope.get_clentlist, function (ss) {
                    ss.checkselected = true;
                    ss.ISMMCF_Negative = true;
                })     
            });
        };

        $scope.Clearid = function () {
            $state.reload();
            $scope.Loaddata();
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };
    }
})();