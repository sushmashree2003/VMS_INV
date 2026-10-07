(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_PDpntSDpntMappingController', ISM_PDpntSDpntMappingController);

    ISM_PDpntSDpntMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', '$window', '$q', '$timeout'];
    function ISM_PDpntSDpntMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, $window, $q, $timeout) {

        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 5;

        $scope.nearme = function ($scope) {
            var d = 0;
            var dd = 0;
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(function (position) {
                    $timeout(function () {
                        d = position.coords.latitude;
                        dd = position.coords.longitude;

                        var localAPI = "http://maps.googleapis.com/maps/api/geocode/json?latlong=" + d + "," + dd + "&sensor=true";
                        console.log(localAPI);
                    });
                });
            }

        };

        $scope.loaddata = function () {
            var pageid = 2;
            //$scope.nearme();
            apiService.getURI("ISMPDpntSDpntMapping/loaddata", pageid).then(function (promise) {

                if (promise.gridviewdetails !== null && promise.gridviewdetails.length > 0) {
                    $scope.gridviewdetails = promise.gridviewdetails;
                }

                $scope.pmi_name = promise.pmi_name;
                $scope.pdnamelist = promise.departmentlist;
                $scope.sdnamelist = promise.departmentlist;
                $scope.institutionlist = promise.institutionlist;
            });
        };

        $scope.cancel = function () {
            $scope.ismpdsdmaP_SHRMD_Id = "";
            $scope.smI_Id = "";
            $scope.hrmD_Id = "";
        };

        $scope.Editdata = function (record) {
            var id = record.ISMPDSDMAP_Id;
            apiService.getURI("ISMPDpntSDpntMapping/editdata", id).
                then(function (promise) {

                    if (promise.departmentlist !== null && promise.departmentlist.length > 0) {
                        $scope.sdnamelist = promise.departmentlist;
                    }

                    if (promise.editlist !== null && promise.editlist.length > 0) {
                        $scope.hrmD_Id = promise.editlist[0].hrmD_Id;
                        $scope.smI_Id = promise.editlist[0].ismpdsdmaP_SMI_Id;
                        $scope.ismpdsdmaP_SHRMD_Id = promise.editlist[0].ismpdsdmaP_SHRMD_Id;
                    }
                });
        };

        $scope.Deletedata = function (record) {
            var id = record.ISMPDSDMAP_Id;
            apiService.getURI("ISMPDpntSDpntMapping/deletedata", id).
                then(function (promise) {
                    if (promise.msg !== "") {
                        if (promise.msg === "Success") {
                            swal("Record Removed Successfully..!!");
                            $scope.loaddata();
                        }
                        else if (promise.msg === "Failed") {
                            swal("Record Not Removed..", 'Fail');
                        }
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                        }
                    }
                });
        };

        $scope.saveData = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "HRMD_Id": $scope.hrmD_Id,
                    "ISMPDSDMAP_SMI_Id": $scope.smI_Id,
                    "ISMPDSDMAP_SHRMD_Id": $scope.ismpdsdmaP_SHRMD_Id
                };

                apiService.create("ISMPDpntSDpntMapping/savedata", data).
                    then(function (promise) {

                        if (promise.msg !== "") {
                            if (promise.msg === "Duplicate") {
                                swal("Duplicate Record..!!");
                                return;
                            }
                            else if (promise.msg === "Failed") {
                                swal("Record Not saved / Updated..", 'Fail');
                            }
                            else if (promise.msg === "Saved") {
                                swal("Record Saved Successfully..");
                                $state.reload();
                            }
                            else if (promise.msg === "updated") {
                                swal("Record Updated Successfully..");
                                $state.reload();
                            }
                            else if (promise.msg === "Notification") {
                                swal("Notification!");
                                $scope.cancel();
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                            }
                        }
                    });
            }
        };

        $scope.ActivateRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.fmsrncoN_ActiveFlg == false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }

            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Record..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.getURI("ISMPDpntSDpntMapping/ActiveDeactivereferenceconfg", data.fmsrncoN_Id).
                            then(function (promise) {

                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Activated") {
                                        swal("Record Activated successfully");
                                    }
                                    else if (promise.retrunMsg === "Deactivated") {
                                        swal("Record Deactivated successfully");
                                    }
                                    else {
                                        swal("Record Not Activated/Deactivated", 'Fail');
                                    }

                                    if (promise.gridviewDetails !== null && promise.gridviewDetails.length > 0) {
                                        $scope.gridReferenceNoConfiguration.data = promise.gridviewDetails;
                                    }
                                }

                            });
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }
            );
        };

        $scope.loaddepartments = function (id) {
            $scope.sdnamelist = [];
           // $scope.smI_Id = "";
            apiService.getURI("ISMPDpntSDpntMapping/loaddepartment", id).then(function (promise) {
                if (promise.departmentlist !== null && promise.departmentlist.length > 0) {
                    $scope.sdnamelist = promise.departmentlist;
                }
            });
        };
    }
})();