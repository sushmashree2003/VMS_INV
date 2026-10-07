(function () {
    'use strict';
    angular
        .module('app')
        .controller('AttendanceGPSLocationController', AttendanceGPSLocationController)
    AttendanceGPSLocationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce', '$timeout', '$window','intracontroller']
    function AttendanceGPSLocationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce, $timeout, $window, intracontroller) {

        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 5;
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.albumNameArraycolumn = [];
        $scope.ismeridian = true;
        $scope.punchflag = false;
        $scope.vmsatgpsL_LATLONGValue = "";
        var HostName = location.host;
        $scope.nearme = function () {
            var d = 0;
            var dd = 0;
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(function (position) {
                    $timeout(function () {
                        d = position.coords.latitude;
                        dd = position.coords.longitude;
                        $scope.vmsatgpsL_LATLONGValue = d.toString() + "+" + dd.toString();
                        //var localAPI = "http://maps.googleapis.com/maps/api/geocode/json?latlong=" + d + "," + dd + "&sensor=true";
                        //console.log(d + dd);
                    });
                });
            }

        };

        $scope.loadData = function () {
            var d = 0;
            var dd = 0;
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(function (position) {
                    $timeout(function () {
                        d = position.coords.latitude;
                        dd = position.coords.longitude;
                        $scope.vmsatgpsL_LATLONGValue = d + "+" + dd;

                        if ($scope.vmsatgpsL_LATLONGValue != null) {
                            var date = { "VMSATGPSL_LATLONGValue": $scope.vmsatgpsL_LATLONGValue };

                            apiService.create("GPSLocation/getalldetails", date).then(function (promise) {
                                $scope.online_leave = promise.employee_name;
                                $scope.clientlist = promise.clientlist;
                                $scope.saleslist = promise.saleslist;
                                $scope.vmsatgpsL_Address = promise.vmsatgpsL_Address;
                                $scope.empdetails = promise.empdetails;
                                if (promise.employee_name != null && promise.employee_name.length > 0) {
                                    $scope.TadaAllwence(promise.employee_name[0].hrmE_Id);
                                }
                               
                            });
                        }

                    });
                });
            }
        };
        //TadaAllwence

        $scope.TadaAllwence = function (HRME_Id) {
            var date = {
                "HRME_Id": HRME_Id
                    
            };

            apiService.create("TADAAdvanceApproval/TadaAllwence", date).then(function (promise) {
                if (promise.getReport != null && promise.getReport.length > 0) {
                    if (promise.getArray != null && promise.getArray.length > 0) {

                    }
                    else {
                        
                        swal({
                            title: "You Can Not do GPS !  Because Still You Did Not Update TADA Tour Status !",
                            text: "Do You Want To update..?",
                            type: "warning",
                            showCancelButton: false,
                            confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, update!",
                         //   cancelButtonText: "Cancel",
                            closeOnConfirm: true,
                            closeOnCancel: true
                        },
                            function (isConfirm) {
                                if (isConfirm) {
                                    intracontroller.viewcompleted = true;
                                    $('.modal-backdrop').remove();
                                    $('.modal').on('hide.bs.modal', function (e) {
                                        e.stopPropagation();
                                        $('body').css('padding-right', '');
                                    });
                                    $window.location.href = 'https://' + HostName + '/#/app/TADA_Advance_Tour_Lead/12';
                                }

                            });
                    }
                }
            });
        }
        //save
        $scope.submitted = false;
        $scope.save = function () {
            if ($scope.myForm.$valid) {

                if ($scope.ismmclT_Id.ismmclT_Id == undefined && $scope.ismslE_Id.ismslE_Id == undefined) {
                    swal('Please select Client Name or Sales Lead Name');
                    return;
                }
                var ISMSLE_Id = 0
                if ($scope.ismslE_Id != null && $scope.ismslE_Id != undefined) {
                    ISMSLE_Id = $scope.ismslE_Id.ismslE_Id;
                }
                else {
                    ISMSLE_Id = 0;
                }

                var data = {
                    "ISMMCLT_Id": $scope.ismmclT_Id.ismmclT_Id,
                    "ISMSLE_Id": ISMSLE_Id,
                    "VMSATGPSL_Address": $scope.vmsatgpsL_Address,
                    "VMSATGPSL_LATLONGValue": $scope.vmsatgpsL_LATLONGValue,
                    "VMSATGPSL_Remarks": $scope.vmsatgpsL_Remarks,
                    "punchflag": $scope.punchflag
                };

                apiService.create("GPSLocation/save", data).
                    then(function (promise) {
                        if (promise.returnmsg === 'Add') {
                            swal('Data successfully Saved');
                            $state.reload();
                        }
                        else if (promise.returnduplicatestatus === 'Failed') {
                            swal('Failed..!!');
                        }
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
    }
})();