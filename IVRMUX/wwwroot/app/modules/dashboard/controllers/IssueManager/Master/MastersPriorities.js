(function () {
    'use strict';
    angular
        .module('app')
        .controller('MastersPrioritiesController', MastersPrioritiesController)

    MastersPrioritiesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MastersPrioritiesController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;

        //=====================Load--data.............
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            
            var pageid = 2;
            apiService.getURI("MastersPriorities/getdetails", pageid).then(function (promise) {
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
                    "HRMPR_Id": $scope.hrmpR_Id,
                    "HRMP_Name": $scope.HRMP_Name,
                    "HRMP_Order": $scope.HRMP_Order,
                }
                apiService.create("MastersPriorities/saverecord", data).then(function (promise) {
                    if (promise.returnval != null && promise.duplicate != null) {
                        if (promise.duplicate == false) {
                            if (promise.returnval == true) {
                                if ($scope.hrmpR_Id > 0) {
                                    swal('Record Updated Successfully!!!');
                                    $state.reload();
                                }
                                else {
                                    swal('Record Saved Successfully!!!');
                                    $state.reload();
                                }
                            }
                            else {
                                if (promise.returnval == false) {
                                    if ($scope.hrmpR_Id > 0) {
                                        swal('Record Not Updated Successfully!!!');
                                        
                                    }
                                    else {
                                        swal('Record Not Saved Successfully!!!');
                                    }
                                }
                            }
                        }
                        else {
                            swal("You Can't  Mapp Same Order!");
                        }
                        
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
            
            $scope.hrmpR_Id = user.hrmpR_Id;
            $scope.HRMP_Name = user.hrmP_Name;
            $scope.HRMP_Order = user.hrmP_Order


        }
        //====================End---edit-record....


        //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            
            $scope.hrmpR_Id = user.hrmpR_Id;

            var dystring = "";
            if (user.hrmP_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.hrmP_ActiveFlag == 0) {
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
                        apiService.create("MastersPriorities/deactiveY", user).
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

