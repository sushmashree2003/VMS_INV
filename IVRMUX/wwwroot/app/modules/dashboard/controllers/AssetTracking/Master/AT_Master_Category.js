(function () {
    'use strict';
    angular
        .module('app')
        .controller('AT_Master_CategoryController', AT_Master_CategoryController);

    AT_Master_CategoryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function AT_Master_CategoryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;

        //===================== Load Data =====================//
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";

            // Calls /api/AT_Master_Category/getdetails/2
            var pageid = 2;
            apiService.getURI("AT_Master_Category/getdetails", pageid).then(function (promise) {
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
                    "MC_Id": $scope.mC_Id ? $scope.mC_Id : 0,
                    "Category": $scope.Category
                };

                apiService.create("AT_Master_Category/saverecord", data).then(function (promise) {
                    var returnval = promise.returnValue !== undefined ? promise.returnValue : promise.returnval;
                    var duplicate = promise.duplicate !== undefined ? promise.duplicate : false;

                    if (duplicate === true) {
                        swal("Record Already Exists!");
                    }
                    else if (returnval === true) {
                        if ($scope.mC_Id > 0) {
                            swal('Record Updated Successfully!!!');
                        } else {
                            swal('Record Saved Successfully!!!');
                        }

                        $scope.mC_Id = 0;
                        $scope.Category = "";
                        $scope.myForm.$setPristine();
                        $scope.myForm.$setUntouched();
                        $scope.submitted = false;

                        $scope.Loaddata();
                    }
                    else if (returnval === false) {
                        if ($scope.mC_Id > 0) {
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
            $scope.mC_Id = user.mC_Id || user.mc_Id;
            $scope.Category = user.category;
        };

        //===================== Deactivate / Activate Record =====================//
        $scope.deactiveY = function (user) {
            var activeStatus = user.activeFlag !== undefined ? user.activeFlag : user.mC_ActiveFlag;
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
                        apiService.create("AT_Master_Category/deactiveY", user).then(function (promise) {
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