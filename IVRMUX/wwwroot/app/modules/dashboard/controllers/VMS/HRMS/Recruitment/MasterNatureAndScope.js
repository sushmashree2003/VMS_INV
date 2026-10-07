(function () {
    'use strict';
    angular
        .module('app')
        .controller('appointmentController', appointmentController);

    appointmentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter'];
    function appointmentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter) {

        $scope.search = "";

        //------------for sorting.........
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.onLoadGetData = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            var pageid = 2;
            apiService.getURI("Appointment/getalldetails", pageid).then(function (promise) {
                $scope.desgnationlist = promise.desgnationlist;
                $scope.departmentlist = promise.departmentlist;
                $scope.get_scopenature = promise.get_scopenature;
                
            });
        };

        // clear form data
        $scope.cancel = function () {
            $state.reload();
        };

        $scope.submitted = false;
        $scope.interacted1 = function (field) {
            return $scope.submitted;
        };

        //==============================save data
        $scope.savedata = function () {
            if ($scope.myForm1.$valid) {
                var data = {
                    //Employeedto: $scope.Candidate,
                    "HRMNS_Id": $scope.HRMNS_Id,
                    "HRMD_Id": $scope.hrmD_Id,
                    "HRMDES_Id":$scope.hrmdeS_Id,
                    "HRMNS_NatureWork":$scope.HRMNS_NatureWork,
                    "HRMNS_ScopeService": $scope.HRMNS_ScopeService,                   
                    "HRMNS_ApprovedStatus":"Pending"
                };

                apiService.create("Appointment/savescopenature", data).then(function (promise) {
                    if (promise.retrunMsg !== "")
                        if (promise.retrunMsg === "Add") {
                            swal("Record Saved Successfully..");
                            $state.reload();
                        }
                        else if (promise.retrunMsg === "Update") {
                            swal("Record Updated Successfully..");
                            $state.reload();
                        }
                        else {
                            swal("Something went wrong ..!");
                            $state.reload();
                        }                   
                });
            }
            else {
                $scope.submitted = true;
            }
        };



        $scope.editnotice = function (user) {
            $scope.HRMNS_Id = user.HRMNS_Id
            $scope.HRMNS_NatureWork = user.HRMNS_NatureWork
            $scope.HRMNS_ScopeService = user.HRMNS_ScopeService
            $scope.hrmD_Id = user.HRMD_Id
            $scope.hrmdeS_Id = user.HRMDES_Id
        }



        $scope.deactiveY = function (user) {
            var mgs = "";
            var confirmmgs = "";
            if (user.HRMNS_Activeflag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("Appointment/deactivate1", user).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }

    }
})();