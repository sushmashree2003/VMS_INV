(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterVenuesController', MasterVenuesController)

    MasterVenuesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterVenuesController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;

        //=====================Load--data.............
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            
            var pageid = 2;
            apiService.getURI("MasterVenues/getdetails", pageid).then(function (promise) {
                $scope.alldata = promise.alldata;
            })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
        //=====================End-----Load--data----//


        //=====================save--record....
        $scope.saverecord = function () {
            
            if ($scope.myForm.$valid) {
                var data = {
                    "ISMMVNE_Id": $scope.ismmvnE_Id,
                    "ISMMVNE_VenueName": $scope.ISMMVNE_VenueName,
                    "ISMMVNE_VenueAddress": $scope.ISMMVNE_VenueAddress,
                }
                apiService.create("MasterVenues/saverecord", data).then(function (promise) {
                    if (promise.returnval != null && promise.duplicate != null) {
                        if (promise.duplicate == false) {
                            if (promise.returnval == true) {
                                if ($scope.ismmvnE_Id > 0) {
                                    swal('Record Updated Successfully!!!');
                                }
                                else {
                                    swal('Record Saved Successfully!!!');
                                }
                            }
                            else {
                                if (promise.returnval == false) {
                                    if ($scope.ismmvnE_Id > 0) {
                                        swal('Record Not Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Not Saved Successfully!!!');
                                    }
                                }
                            }
                        }
                        else {
                            swal("Record already exist");
                        }
                        $state.reload();
                    }
                    else {
                        swal("Kindly Contact Administrator!!!");
                    }

                })
            }
            else {
                $scope.submitted = true;
            }
        };
        //=====================End---save--record....


        //=====================Editrecord....
        $scope.EditData = function (user) {
            
            $scope.ismmvnE_Id = user.ismmvnE_Id;
            $scope.ISMMVNE_VenueName = user.ismmvnE_VenueName;
            $scope.ISMMVNE_VenueAddress = user.ismmvnE_VenueAddress


        }
        //====================End---edit-record....


        //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            
            $scope.ismmvnE_Id = user.ismmvnE_Id;

            var dystring = "";
            if (user.ismmvnE_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.ismmvnE_ActiveFlag == 0) {
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
                        apiService.create("MasterVenues/deactiveY", user).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");

                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");

                                }
                                $state.reload();
                            })

                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }

                });
        }
        //================End----Activation/Deactivation--Record.........


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };



        //===========----Clear Field
        $scope.Clearid = function () {
            $state.reload();
        }



    }
})();

