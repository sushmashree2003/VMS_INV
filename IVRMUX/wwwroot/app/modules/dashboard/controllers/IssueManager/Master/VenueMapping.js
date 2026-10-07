(function () {
    'use strict';
    angular
        .module('app')
        .controller('VenueMappingController', VenueMappingController)

    VenueMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function VenueMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //=====================Load--data.............
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            
            var pageid = 2;
            apiService.getURI("VenueMapping/getdetails", pageid).then(function (promise) {
                $scope.alldata = promise.alldata;
                $scope.venuelist = promise.venuelist;

            })

        }
        //=====================End-----Load--data----//


        //=====================save--record....
        $scope.saverecord = function () {
            
            if ($scope.myForm.$valid) {
                var data = {
                    "ISMMMER_Id": $scope.ismmmeR_Id,
                    "ISMMMER_Facilities": $scope.ISMMMER_Facilities,
                    "ISMMMER_MeetingRoomName": $scope.ISMMMER_MeetingRoomName,
                    "ISMMMER_FloorName": $scope.ISMMMER_FloorName,
                    "ISMMVNE_Id": $scope.ISMMVNE_Id
                   
                }
                apiService.create("VenueMapping/saverecord", data).then(function (promise) {
                    if (promise.returnval != null && promise.duplicate != null) {
                        if (promise.duplicate == false) {
                            if (promise.returnval == true) {
                                if ($scope.ismmmeR_Id > 0) {
                                    swal('Record Updated Successfully!!!');
                                }
                                else {
                                    swal('Record Saved Successfully!!!');
                                }
                            }
                            else {
                                if (promise.returnval == false) {
                                    if ($scope.ismmmeR_Id > 0) {
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
            
            $scope.ismmmeR_Id = user.ismmmeR_Id;
            $scope.ISMMMER_Facilities = user.ismmmeR_Facilities;
            $scope.ISMMMER_MeetingRoomName = user.ismmmeR_MeetingRoomName,
            $scope.ISMMMER_FloorName = user.ismmmeR_FloorName,
            $scope.ISMMVNE_Id = user.ismmvnE_Id


        }
        //====================End---edit-record....


        //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            
            $scope.ismmmeR_Id = user.ismmmeR_Id;

            var dystring = "";
            if (user.ismmmeR_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.ismmmeR_ActiveFlag == 0) {
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
                        apiService.create("VenueMapping/deactiveY", user).
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

