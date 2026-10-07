(function () {
    'use strict';
    angular
        .module('app')
        .controller('AT_Master_CheckPointController', AT_Master_CheckPointController);

    AT_Master_CheckPointController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function AT_Master_CheckPointController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;

        //===================== Load Data =====================//
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";

            var pageid = 2;
            apiService.getURI("AT_Master_CheckPoint/getdetails", pageid).then(function (promise) {
                if (promise.alldata != null && promise.alldata.length > 0) {
                    $scope.alldata = promise.alldata;
                } else {
                    $scope.alldata = [];
                }
            });

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;
                $scope.reverse = !$scope.reverse;
            };
        };

        //===================== Save Record =====================//
        $scope.saverecord = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "MCP_Id": $scope.mcP_Id ? $scope.mcP_Id : 0,
                    "CheckPoint": $scope.CheckPoint
                };

                apiService.create("AT_Master_CheckPoint/saverecord", data).then(function (promise) {
                    var returnval = promise.returnValue !== undefined ? promise.returnValue : promise.returnval;
                    var duplicate = promise.duplicate !== undefined ? promise.duplicate : false;

                    if (duplicate === true) {
                        swal("Record Already Exists!");
                    }
                    else if (returnval === true) {
                        if ($scope.mcP_Id > 0) {
                            swal('Record Updated Successfully!!!');
                        } else {
                            swal('Record Saved Successfully!!!');
                        }

                        $scope.mcP_Id = 0;
                        $scope.CheckPoint = "";
                        $scope.myForm.$setPristine();
                        $scope.myForm.$setUntouched();
                        $scope.submitted = false;

                        $scope.Loaddata();
                    }
                    else if (returnval === false) {
                        if ($scope.mcP_Id > 0) {
                            swal('Record Not Updated Successfully!!!');
                        } else {
                            swal('Record Not Saved Successfully!!!');
                        }
                    }
                    else {
                        swal("Kindly Contact Administrator!!!");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        //===================== Edit Record =====================//
        $scope.EditData = function (user) {
            $scope.mcP_Id = user.mcP_Id || user.mcp_Id;
            $scope.CheckPoint = user.checkPoint;
        };

        //===================== Deactivate / Activate Record =====================//
        $scope.deactiveY = function (user) {
            var activeStatus = user.activeFlag !== undefined ? user.activeFlag : user.mCP_ActiveFlag;
            var dystring = (activeStatus === true || activeStatus === 1) ? "Deactivate" : "Activate";

            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("AT_Master_CheckPoint/deactiveY", user).then(function (promise) {
                            var returnval = promise.returnValue !== undefined ? promise.returnValue : promise.returnval;
                            if (returnval === true) {
                                swal("Record " + dystring + "d Successfully!!!");
                            } else {
                                swal("Record Not " + dystring + "d Successfully!!!");
                            }
                            $scope.Loaddata();
                        });
                    } else {
                        swal("Record " + dystring + " Cancelled");
                    }
                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();