(function () {
    'use strict';
    angular.module('app').controller('RTO_Master_FineController', RTO_Master_FineController)
    RTO_Master_FineController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function RTO_Master_FineController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };


        $scope.submitted = false;

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }
        var configsettingsnew = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        $scope.itemsPerPage = paginationformasters;

        $scope.currentPage = 1;
        //-----------Tax Grid Select All

        $scope.loaddata = function () {
            $scope.obj.MI_Id = "";
            var pageid = 2;
            apiService.getURI("RTO_Master_Fine/loaddata", pageid).then(function (promise) {

                $scope.RTOArray = promise.rtoArray;
            });
        };

        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var RTOMFP_Id = 0;
                if ($scope.RTOMFP_Id > 0) {
                    RTOMFP_Id = $scope.RTOMFP_Id;
                }
                var data = {
                    "RTOMFP_FineParameterName": $scope.RTOMFP_FineParameterName,
                    "RTOMFP_FineAmount": $scope.RTOMFP_FineAmount,
                    "RTOMFP_MaxViolationPermitted": $scope.RTOMFP_MaxViolationPermitted,
                    "RTOMFP_ParameterDescription": $scope.RTOMFP_ParameterDescription,
                    "RTOMFP_Id": RTOMFP_Id
                };
                apiService.create("RTO_Master_Fine/savedataSource", data).then(function (promise) {
                    if (promise.returnval == "save") {
                        swal("Record Saved Sucessfully !");
                    }
                    else if (promise.returnval == "Notsave") {
                        swal("Record Not Saved !");
                    }
                    else if (promise.returnval == "duplicate") {
                        swal("Record Not Saved !");
                    }
                    if (promise.returnval == "update") {
                        swal("Record Update Sucessfully !");
                    }
                    else if (promise.returnval == "Notupdate") {
                        swal("Record Not Update !");
                    }
                    else if (promise.returnval == "admin") {
                        swal("Please Contact Administrator !");
                    }
                    $state.reload();
                });
            }

        };
        $scope.edit = function (user) {
            $scope.RTOMFP_FineParameterName = user.rtomfP_FineParameterName;
            $scope.RTOMFP_FineAmount = user.rtomfP_FineAmount;
            $scope.RTOMFP_MaxViolationPermitted = user.rtomfP_MaxViolationPermitted;
            $scope.RTOMFP_ParameterDescription = user.rtomfP_ParameterDescription;
            $scope.RTOMFP_Id = user.rtomfP_Id;
        }
        $scope.Deletedata = function (item, SweetAlert) {
            var data = {
                "RTOMFP_Id": item.rtomfP_Id
            }
            var dystring = "";
            if (item.rtomfP_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.rtomfP_ActiveFlag == false) {
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
                        apiService.create("RTO_Master_Fine/clientDecative", data).
                            then(function (promise) {
                                if (promise.returnval == "active") {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else if (promise.returnval == "notactive")  {
                                    swal("Record Not Deactivate !");
                                }
                                else if (promise.returnval == "admin") {
                                    swal("Please Contact Administrator !");
                                }
                                $state.reload();

                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();
