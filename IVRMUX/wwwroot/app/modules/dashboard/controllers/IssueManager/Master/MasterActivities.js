(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterActivitiesController', MasterActivitiesController)

    MasterActivitiesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterActivitiesController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;

        //=====================Load--data.............
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            $scope.alldata = [];
            $scope.deptlist = [];
            $scope.designationlist = [];
            var pageid = 2;
            apiService.getURI("MasterActivities/getdetails", pageid).then(function (promise) {
                $scope.alldata = promise.alldata;
                $scope.deptlist = promise.deptlist;
                $scope.designationlist = promise.designationlist;
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
                    "ismmaC_Id": $scope.ismmaC_Id,
                    "ISMMAC_ActivityName": $scope.ISMMAC_ActivityName,
                    "ISMMAC_Desc": $scope.ISMMAC_Desc,
                    "HRMD_Id": $scope.HRMD_Id,
                    "HRMDES_Id": $scope.HRMDES_Id

                }
                apiService.create("MasterActivities/saverecord1", data).then(function (promise) {
                    if (promise.returnval != null && promise.duplicate != null) {
                        if (promise.duplicate == false) {
                            if (promise.returnval == true) {
                                if ($scope.ismmaC_Id > 0) {
                                    swal('Record Updated Successfully!!!', "", "success");
                                 
                                    $scope.Loaddata();
                                }
                                else {
                                    swal('Record Saved Successfully!!!', "", "success");
                                   
                                    $scope.Loaddata();
                                }
                            }
                            else {
                                if (promise.returnval == false) {
                                    if ($scope.ismmaC_Id > 0) {
                                        swal('Failed To Update !!!', "", "warning");
                                    }
                                    else {
                                        swal('Record Not Saved Successfully!!!', "", "success");
                                    }
                                }
                            }
                        }
                        else {
                            swal("Record already exist","","info");
                        }
                 
                        $scope.Loaddata();
                    }
                    else {
                        swal("Kindly Contact Administrator!!!", "", "info");
                    }

                })
            }
            else {
                $scope.submitted = true;
            }
        };
        //=====================End...............................


        //=====================Editrecord....
        $scope.EditData = function (user) {

            $scope.ismmaC_Id = user.ismmaC_Id;
            $scope.ISMMAC_ActivityName = user.ismmaC_ActivityName;
            $scope.ISMMAC_Desc = user.ismmaC_Desc,
                $scope.HRMD_Id = user.hrmD_Id,
                $scope.HRMDES_Id = user.hrmdeS_Id


        }
        //====================End---edit-record....


        //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {

            $scope.ismmaC_Id = user.ismmaC_Id;

            var dystring = "";
            if (user.ismmaC_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.ismmaC_ActiveFlag == 0) {
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
                        apiService.create("MasterActivities/deactiveY", user).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");

                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");

                                }
                              //  $state.reload();
                                $scope.Loaddata();
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

