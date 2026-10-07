(function () {
    'use strict';
    angular.module('app').controller('AppointmentStatusController', AppointmentStatusController)

    AppointmentStatusController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache', 'myFactoryvisitor']
    function AppointmentStatusController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache, myFactoryvisitor) {

        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.SPCCESTR_Id = 0;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };


        var miid = myFactoryvisitor.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }

        $scope.radiomodal = 'int_dept';
        $scope.visitorlist = [];
        $scope.loadgrid = function () {
            if ($scope.mI_Id === undefined || $scope.mI_Id === null || $scope.mI_Id === '') {
                $scope.mI_Id = 0;
            }
            apiService.getURI("AppointmentStatus/getDetails/", $scope.mI_Id).then(function (promise) {
                $scope.mI_Id = promise.mI_Id;
                $scope.institutionlist = promise.institutionlist;
                if (promise.visitorlist.length > 0) {
                    $scope.visitorlist = promise.visitorlist;
                }
            });
        };

        $scope.onselectname = function () {
            if ($scope.radiomodal === 'int_dept') {
                $scope.type = 'int_dept';
            }
            else if ($scope.radiomodal === 'out_dept') {
                $scope.type = 'out_dept';
            }
            var obj = {
                "VMMV_Id": $scope.vmmV_Id,
                "type": $scope.type,
                "MI_Id": $scope.mI_Id
            };

            apiService.create("AppointmentStatus/Edit", obj).then(function (promise) {
                if (promise.editDetails != null && promise.editDetails.length > 0) {
                $scope.VMMV_Id = promise.editDetails[0].vmmV_Id;
                $scope.empname = promise.editDetails[0].empname;
                $scope.VMMV_MeetingDateTime = new Date(promise.editDetails[0].vmmV_MeetingDateTime);
                $scope.VMMV_EntryDateTime = moment(promise.editDetails[0].vmmV_EntryDateTime, 'HH:mm').format();
                $scope.VMMV_Remarks = promise.editDetails[0].vmmV_Remarks;
                $scope.mI_Id = promise.editDetails[0].mI_Id;
            }
            });
        };

        $scope.hstep = 1;
        $scope.mstep = 1;


        $scope.submitted = false;
        $scope.saveData = function () {
            if ($scope.radiomodal === 'int_dept') {
                $scope.type = 'int_dept';
            }
            else if ($scope.radiomodal === 'out_dept') {
                $scope.type = 'out_dept';
            }
            if ($scope.myForm.$valid) {

                var obj = {
                    "VMMV_Id": $scope.VMMV_Id,
                    "VMMV_Remarks": $scope.VMMV_Remarks,
                    "VMMV_CkeckedInOutStatus": $scope.VMMV_CkeckedInOutStatus,
                    "VMMV_ExitDateTime": $filter('date')($scope.VMMV_ExitDateTime, "HH:mm"),
                    "type": $scope.type,
                    "MI_Id": $scope.mI_Id
                };
                apiService.create("AppointmentStatus/saveData", obj).then(function (promise) {

                    if (promise.returnVal === 'saved') {
                        swal("Record Saved Successfully");
                        $state.reload();
                    }
                    else if (promise.returnVal === 'updated') {
                        swal("SMS & Mail Send Successfully");
                        $state.reload();
                    }
                    else if (promise.returnVal === 'duplicate') {
                        swal("Record already exist");
                    }
                    else if (promise.returnVal === "savingFailed") {
                        swal("Failed to save record");
                    }
                    else if (promise.returnVal === "updateFailed") {
                        swal("Failed to update record");
                    }
                    else {
                        swal("Sorry...something went wrong");
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.onChangeradio = function (radiomodal) {
            $scope.visitorlist = [];
            if (radiomodal === 'int_dept') {
                $scope.type = 'int_dept';
            }
            else if (radiomodal === 'out_dept') {
                $scope.type = 'out_dept';
            }

            var data = {
                "type": $scope.type,
                "MI_Id": $scope.mI_Id
            };
            apiService.create("AppointmentStatus/change_radiobutton", data).then(function (promise) {
                if (promise.visitorlist.length > 0) {
                    $scope.visitorlist = promise.visitorlist;
                }
            });
        }
    }
})();