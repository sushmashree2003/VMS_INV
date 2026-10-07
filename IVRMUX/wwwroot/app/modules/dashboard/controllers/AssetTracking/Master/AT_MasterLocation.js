
(function () {
    'use strict';
    angular
        .module('app')
        .controller('AT_MasterLocationController', AT_MasterLocationController);
    AT_MasterLocationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function AT_MasterLocationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {



        var paginationformasters;
        $scope.obj = {};
        $scope.editS = false;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //-------------------------------------------------------------------
        $scope.invmlO_OutsideFlg = false

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("AT_MasterLocation/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_sites = promise.get_sites;
                    $scope.get_employee = promise.get_employee;
                    $scope.get_locations = promise.get_locations;
                    $scope.presentCountgrid = $scope.get_locations.length;
                });
        };
        $scope.oncontactpersonchange = function () {
            if ($scope.contactpersonflag === 'C') {
                $scope.invmlO_InchargeName = "Common";
                $scope.obj = {};
            }
            else if ($scope.contactpersonflag === 'O') {
                if ($scope.invmlO_InchargeName && $scope.invmlO_InchargeName.toUpperCase() === 'COMMON') {
                    $scope.invmlO_InchargeName = "";
                }
                $scope.obj = {};
            }
            else if ($scope.contactpersonflag === 'E') {
                if ($scope.invmlO_InchargeName && $scope.invmlO_InchargeName.toUpperCase() === 'COMMON') {
                    $scope.invmlO_InchargeName = "";
                }
                $scope.obj = {};
            }
        };

        //============================================ SAVE EDIT

        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {};
                var hrme_id = 0;
                var inchargeName = "";

                if ($scope.contactpersonflag === "E") {
                    if ($scope.obj && $scope.obj.hrmE_Id && $scope.obj.hrmE_Id.hrmE_Id > 0) {
                        hrme_id = $scope.obj.hrmE_Id.hrmE_Id;
                        inchargeName = $scope.obj.hrmE_Id.employeename;
                    }
                    data = {
                        "INVMSI_Id": $scope.invmsI_Id,
                        "INVMLO_LocationRoomName": $scope.invmlO_LocationRoomName,
                        "INVMLO_LocationRemarks": $scope.invmlO_LocationRemarks,
                        "HRME_Id": hrme_id,
                        "INVMLO_InchargeName": inchargeName,
                        "INVMLO_Id": $scope.invmlO_Id,
                        "INVMLO_OutsideFlg": $scope.invmlO_OutsideFlg
                    };
                }
                else if ($scope.contactpersonflag === "C") {
                    data = {
                        "INVMSI_Id": $scope.invmsI_Id,
                        "INVMLO_LocationRoomName": $scope.invmlO_LocationRoomName,
                        "INVMLO_LocationRemarks": $scope.invmlO_LocationRemarks,
                        "HRME_Id": 0,
                        "INVMLO_InchargeName": "Common",
                        "INVMLO_Id": $scope.invmlO_Id,
                        "INVMLO_OutsideFlg": $scope.invmlO_OutsideFlg
                    };
                }
                else {
                    data = {
                        "INVMSI_Id": $scope.invmsI_Id,
                        "INVMLO_LocationRoomName": $scope.invmlO_LocationRoomName,
                        "INVMLO_LocationRemarks": $scope.invmlO_LocationRemarks,
                        "HRME_Id": 0,
                        "INVMLO_InchargeName": $scope.invmlO_InchargeName,
                        "INVMLO_Id": $scope.invmlO_Id,
                        "INVMLO_OutsideFlg": $scope.invmlO_OutsideFlg
                    };
                }

                apiService.create("AT_MasterLocation/savedetails", data).then(function (promise) {
                    if (promise.returnval === true) {
                        if ($scope.invmlO_Id > 0) {
                            swal('Record updated successfully');
                        }
                        else {
                            swal('Record saved successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if ($scope.invmlO_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                        else {
                            swal('Failed to save, please contact administrator');
                        }
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.deactive = function (item, SweetAlert) {
            $scope.INVMLO_Id = item.invmlO_Id;
            var dystring = "";
            if (item.invmlO_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invmlO_ActiveFlg === false) {
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
                        apiService.create("AT_MasterLocation/deactive", item).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.edit = function (item) {
            $scope.editS = true;
            $scope.invmsI_Id = item.invmsI_Id;
            $scope.invmsI_SiteBuildingName = item.invmsI_SiteBuildingName;
            $scope.invmlO_LocationRoomName = item.invmlO_LocationRoomName;
            $scope.invmlO_LocationRemarks = item.invmlO_LocationRemarks;
            $scope.invmlO_OutsideFlg = item.invmlO_OutsideFlg;
            $scope.invmlO_Id = item.invmlO_Id;
            $scope.obj = {};

            if (item.hrmE_Id !== null && item.hrmE_Id > 0) {
                $scope.contactpersonflag = 'E';
                $scope.invmlO_InchargeName = "";
                angular.forEach($scope.get_employee, function (emp) {
                    if (emp.hrmE_Id === item.hrmE_Id) {
                        $scope.obj.hrmE_Id = emp;
                    }
                });
            }
            else if (item.invmlO_InchargeName && item.invmlO_InchargeName.toUpperCase() === "COMMON") {
                $scope.contactpersonflag = 'C';
                $scope.invmlO_InchargeName = item.invmlO_InchargeName;
            }
            else {
                $scope.contactpersonflag = 'O';
                $scope.invmlO_InchargeName = item.invmlO_InchargeName;
            }
        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
    }
})();