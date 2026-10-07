(function () {
    'use strict';
    angular
        .module('app')
        .controller('ApprovalInOutStatus', ApprovalInOutStatus)

    ApprovalInOutStatus.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache', 'myFactoryvisitor']
    function ApprovalInOutStatus($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache, myFactoryvisitor) {



        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.SPCCESTR_Id = 0;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }


        var miid = myFactoryvisitor.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }

        $scope.radiomodal = 'int_dept';
        $scope.visitorlist = [];
        $scope.loadgrid = function () {
            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }
            apiService.getURI("ApprovalInOutStatus/getDetails/", $scope.mI_Id).then(function (promise) {
                $scope.mI_Id = promise.mI_Id;
                $scope.institutionlist = promise.institutionlist;
                if (promise.visitorlist.length > 0) {
                    $scope.visitorlist = promise.visitorlist;
                }
            });
        }

        $scope.onselectname = function () {
           
            if ($scope.radiomodal === 'int_dept') {
                $scope.type = 'int_dept'
            }
            else if ($scope.radiomodal === 'out_dept') {
                $scope.type = 'out_dept'
            }
            var obj = {
                "VMAP_Id": $scope.vmaP_Id,
                "type": $scope.type,
                "MI_Id": $scope.mI_Id,

            }

            apiService.create("ApprovalInOutStatus/Edit", obj).then(function (promise) {

                $scope.VMAP_Id = promise.editDetails[0].vmaP_Id;
                $scope.empname = promise.editDetails[0].empname;
                $scope.VMAP_MeetingDateTime = new Date(promise.editDetails[0].vmaP_MeetingDateTime);
                $scope.VMAP_EntryDateTime = moment(promise.editDetails[0].vmaP_EntryDateTime, 'HH:mm').format();
                $scope.VMAP_Remarks = promise.editDetails[0].vmaP_Remarks;
                $scope.mI_Id = promise.editDetails[0].mI_Id;
            });
        }

        $scope.hstep = 1;
        $scope.mstep = 1;


        $scope.submitted = false;
        $scope.saveData = function () {
            debugger;
            if ($scope.radiomodal === 'int_dept') {
                $scope.type = 'int_dept'
            }
            else if ($scope.radiomodal === 'out_dept') {
                $scope.type = 'out_dept'
            }
            if ($scope.myForm.$valid) {

                var obj = {
                    "VMAP_Id": $scope.vmaP_Id,
                    "VMAP_Remarks": $scope.VMAP_Remarks,
                    "VMAP_ChekInOutStatus": $scope.VMAP_ChekInOutStatus,
                    "VMAP_ExitDateTime": $filter('date')($scope.VMAP_ExitDateTime, "HH:mm"),
                    //"VMMV_EntryDateTime": $filter('date')($scope.VMMV_EntryDateTime, "HH:mm")
                    "type": $scope.type,
                    "MI_Id": $scope.mI_Id,
                }
                apiService.create("ApprovalInOutStatus/saveData", obj).
                    then(function (promise) {

                        if (promise.returnVal == 'updated') {
                            swal("SMS & Mail Send Successfully");

                            $state.reload();
                        }                        
                        else if (promise.returnVal == "updateFailed") {
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
        }

        $scope.cancel = function () {

            $state.reload();

        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.onChangeradio = function (radiomodal) {
            $scope.visitorlist = [];
            debugger;
            if (radiomodal === 'int_dept') {
                $scope.type = 'int_dept'
            }
            else if (radiomodal === 'out_dept') {
                $scope.type = 'out_dept'
            }

            var data = {
                "type": $scope.type,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("ApprovalInOutStatus/change_radiobutton", data).then(function (promise) {
                if (promise.visitorlist.length > 0) {
                    $scope.visitorlist = promise.visitorlist;
                }
            });
        }



    }
})();