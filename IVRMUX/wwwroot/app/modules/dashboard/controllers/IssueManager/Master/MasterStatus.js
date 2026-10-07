(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterStatusController', MasterStatusController)

    MasterStatusController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterStatusController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;

        //=====================Load--data.............
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            
            var pageid = 2;
            apiService.getURI("MasterStatus/getdetails", pageid).then(function (promise) {
                $scope.deptlist = promise.deptlist;
                $scope.getStatusdetails = promise.getStatusdetails;
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
                    "ISMMISTS_Id": $scope.ismmistS_Id,
                    "HRMD_Id": $scope.HRMD_Id,
                    "ISMMISTS_Order": $scope.ISMMISTS_Order,
                    "ISMMISTS_StatusName": $scope.ISMMISTS_StatusName,
                    "ISMMISTS_StatusFlag": $scope.ISMMISTS_StatusFlag,
                    "ISMMISTS_Color": $scope.ISMMISTS_Color,

                }
                apiService.create("MasterStatus/saverecord", data).then(function (promise) {
                    if (promise.returnval != null && promise.duplicate != null) {
                        if (promise.duplicate == false) {
                            if (promise.returnval == true) {
                                if ($scope.ismmistS_Id > 0) {
                                    swal('Record Updated Successfully!!!');
                                }
                                else {
                                    swal('Record Saved Successfully!!!');
                                }
                            }
                            else {
                                if (promise.returnval == false) {
                                    if ($scope.ismmistS_Id > 0) {
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
            
            $scope.ismmistS_Id = user.ismmistS_Id;
            $scope.HRMD_Id = user.hrmD_Id;
            $scope.ISMMISTS_Order = user.ismmistS_Order,
                $scope.ISMMISTS_StatusName = user.ismmistS_StatusName,
                $scope.ISMMISTS_StatusFlag = user.ismmistS_StatusFlag,
                $scope.ISMMISTS_Color = user.ismmistS_Color



        }
        //====================End---edit-record....


        //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            
            $scope.ismmistS_Id = user.ismmistS_Id;

            var dystring = "";
            if (user.ismmistS_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.ismmistS_ActiveFlag == 0) {
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
                        apiService.create("MasterStatus/deactiveY", user).
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
            return $scope.submitted;
        };



        //===========----Clear Field
        $scope.Clearid = function () {
            $state.reload();
        }



    }
})();

