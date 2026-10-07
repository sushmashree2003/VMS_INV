(function () {
    'use strict';
    angular.module('app').controller('clientIEmappingController', clientIEmappingController)

    clientIEmappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function clientIEmappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {

        $scope.submitted = false;

        $scope.obj = {};
        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        };

        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("clientIEmapping/onloaddata", pageid).then(function (promise) {
                $scope.emp_list = promise.employee_list;
                $scope.cli_list = promise.client_list;
                $scope.getloadgrid = promise.getloaddetails;
            });
        };

        $scope.onieChange = function (hrmeid) {
            angular.forEach($scope.emp_list, function (d) {
                if (d.fobvieM_HRMEId == hrmeid.fobvieM_HRMEId) {
                    if (d.bioMetricCode != null && d.bioMetricCode != undefined && d.bioMetricCode.length > 0) {
                        $scope.biometriccode = d.bioMetricCode
                    }
                    else {
                        swal("Please enter the Biometric Code in Employee Registration!!");
                        $scope.biometriccode = "";
                    }
                }

            });
        };
        $scope.oncliChange = function (ismmcltid) {
            angular.forEach($scope.cli_list, function (d) {
                if (d.ismmclT_Id == ismmcltid.ismmclT_Id) {
                    if (d.ivrM_MI_Id > 0 && d.ivrM_MI_Id != null && d.ivrM_MI_Id != undefined) {
                        $scope.ivrM_MI_Id = d.ivrM_MI_Id
                    }
                    else {
                        swal("Please enter the MI Code For Client !!");
                        $scope.ivrM_MI_Id = "";
                    }
                }

            });
        };

        $scope.saverecorded = function () {
            if ($scope.myForm.$valid) {
                //if (fobvieM_Insert_MI_Id != null && fobvieM_Insert_MI_Id != 0) {
                var data = {
                    "FOBVIEM_Id": $scope.FOBVIEM_Id,
                    "FOBVIEM_HRMEId": $scope.fobvieM_HRMEId.fobvieM_HRMEId,
                    "FOBVIEM_BiometricId": $scope.biometriccode,
                    "FOBVIEM_Insert_MI_Id": $scope.ivrM_MI_Id,
                    "ISMMCLT_Id": $scope.ismmclT_Id.ismmclT_Id,
                };

                apiService.create("clientIEmapping/saverecord", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message === "Duplicate") {
                            swal("Record Already Exists");
                        } else if (promise.message === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.message === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else if (promise.message === "Error") {
                            swal("Failed To Save/ Update Record");
                        } else {
                            swal("Something Went Wrong Contact Administrator");
                        }
                        $state.reload();
                    }
                });

            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.edit = function (item) {
            $scope.FOBVIEM_Id = item.FOBVIEM_Id;
            $scope.fobvieM_HRMEId = item.FOBVIEM_HRMEId;
            $scope.ismmclT_Id = item.ISMMCLT_Id;
            $scope.ivrM_MI_Id = item.IVRM_MI_Id;
            $scope.biometriccode = item.FOBVIEM_BiometricId;

            $scope.newEmp = {};
            if ($scope.emp_list != null && $scope.emp_list.length > 0) {                for (var i = 0; i < $scope.emp_list.length; i++) {                    if ($scope.fobvieM_HRMEId == $scope.emp_list[i].fobvieM_HRMEId) {                        $scope.emp_list[i].Selected = true;                        $scope.fobvieM_HRMEId = $scope.emp_list[i];                        $scope.newEmp = $scope.emp_list[0].fobvieM_HRMEId;                        // return;                    }                }            }
            $scope.newIe = {};
            if ($scope.cli_list != null && $scope.cli_list.length > 0) {                for (var i = 0; i < $scope.cli_list.length; i++) {                    if ($scope.ismmclT_Id == $scope.cli_list[i].ismmclT_Id) {                        $scope.cli_list[i].Selected = true;                        $scope.ismmclT_Id = $scope.cli_list[i];                        $scope.newIe = $scope.cli_list[0].ismmclT_Id;                        // return;                    }                }            }


        }


        $scope.deactiveY = function (user) {


            var dystring = "";
            if (user.FOBVIEM_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (user.FOBVIEM_ActiveFlg == false) {
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
                        var data = {
                            "FOBVIEM_Id": user.FOBVIEM_Id
                        };
                        apiService.create("clientIEmapping/deactive", data).
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
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };


    }
})();

