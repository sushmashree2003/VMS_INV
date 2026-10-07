(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterSystemController', MasterSystemController);

    MasterSystemController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function MasterSystemController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;

        //=====================Load--data.............
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
          
            var pageid = 2;
            apiService.getURI("MasterSystem/Getdetails", pageid).then(function (promise) {
              
                $scope.alldata = promise.alldata;
             
            });

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   // set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; // if true make it false and vice versa
            };
        };
        //=====================End-----Load--data----//


        //=====================save--record....
        //=====================save--record....
        $scope.saverecord = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "MS_Id": $scope.mS_Id ? $scope.mS_Id : 0,
                    "System": $scope.System
                };

                apiService.create("MasterSystem/saverecord", data).then(function (promise) {

                    // Handle property casing differences (returnval vs returnValue)
                    var returnval = promise.returnval !== undefined ? promise.returnval : promise.returnValue;
                    var duplicate = promise.duplicate !== undefined ? promise.duplicate : false;

                    if (duplicate === true) {
                        swal("Record Already Exists!");
                    }
                    else if (returnval === true) {
                        if ($scope.mS_Id > 0) {
                            swal('Record Updated Successfully!!!');
                        } else {
                            swal('Record Saved Successfully!!!');
                        }

                        // Clear inputs and reload grid data without state reload bugs
                        $scope.mS_Id = 0;
                        $scope.System = "";
                        $scope.myForm.$setPristine();
                        $scope.myForm.$setUntouched();
                        $scope.submitted = false;

                        // Reload data grid
                        $scope.Loaddata();
                    }
                    else if (returnval === false) {
                        if ($scope.mS_Id > 0) {
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
        //=====================End---save--record....
        //=====================End---save--record....


        //=====================Editrecord....
        $scope.EditData = function (user) {
            $scope.mS_Id = user.mS_Id;
            $scope.System = user.system;
            $scope.Loaddata();
        };
        //====================End---edit-record....


        //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user) {

            $scope.mS_Id = user.mS_Id;

            var dystring = "";
            if (user.mS_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.mS_ActiveFlag == 0) {
                dystring = "Activate";
            }

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
                        apiService.create("MasterSystem/deactiveY", user).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }
                });
        };
        //================End----Activation/Deactivation--Record.........


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        //===========----Clear Field
        $scope.Clearid = function () {
            $state.reload();
        };

    }
})();
