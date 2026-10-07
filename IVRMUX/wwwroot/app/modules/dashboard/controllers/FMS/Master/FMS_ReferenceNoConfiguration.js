(function () {
    'use strict';
    angular
        .module('app')
        .controller('FMS_RefNoConfgController', FMS_RefNoConfgController);

    FMS_RefNoConfgController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter'];
    function FMS_RefNoConfgController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter) {

        $scope.loadreferenceconfg = function () {
            var pageid = 2;
            apiService.getURI("FMS_Master_PhysicalFileName/loadreferenceconfg", pageid).then(function (promise) {

                if (promise.gridviewDetails !== null && promise.gridviewDetails.length > 0) {
                    $scope.gridReferenceNoConfiguration.data = promise.gridviewDetails;
                }
                
            });            
        };

        $scope.cancel = function () {
            $scope.fmsrncoN_Criterias = "";
            $scope.fmsrncoN_CriteriaOrder = "";
        };

        $scope.EditData = function (record) {
            var id = record.fmsrncoN_Id;
            apiService.getURI("FMS_Master_PhysicalFileName/editreferenceconfg", id).
                then(function (promise) {
                    if (promise.editDetails !== null && promise.editDetails.length > 0) {
                        $scope.fmsrncoN_Criterias = promise.editDetails[0].fmsrncoN_Criterias;
                        $scope.fmsrncoN_CriteriaOrder = promise.editDetails[0].fmsrncoN_CriteriaOrder;
                    }
                });
        };

        //Reference Number Configuration Grid
        $scope.gridReferenceNoConfiguration = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableHiding: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'fmsrncoN_Criterias', displayName: 'Criteria Name', enableHiding: false },
                { name: 'fmsrncoN_CriteriaOrder', displayName: 'Criteria Order', enableHiding: false },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);" data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.fmsrncoN_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.ActivateRecord(row.entity);" data-toggle="tooltip" title="Activate"> Activate</a>' +
                        '<span ng-if="row.entity.fmsrncoN_ActiveFlg === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.ActivateRecord(row.entity);" data-toggle="tooltip" title="Deactivate">  Deactivate</a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridReferenceNoConfiguration = gridApi;
            }
        };
        $scope.submitted = false;
        $scope.saveCriteria = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "FMSRNCON_Criterias": $scope.fmsrncoN_Criterias,
                    "FMSRNCON_CriteriaOrder": $scope.fmsrncoN_CriteriaOrder
                };

                apiService.create("FMS_Master_PhysicalFileName/savereferenceconfg", data).
                    then(function (promise) {

                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate Order") {
                                swal("Duplicate Order..!!");
                                return;
                            }
                            else if (promise.retrunMsg === "Duplicate Criteria") {
                                swal("Duplicate Criteria..!!");
                                return;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                                $state.reload();
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                                $scope.cancel();
                            }
                            else if (promise.retrunMsg === "Notification") {
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
        $scope.interacted = function (field) {
            return $scope.submitted;
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
                        apiService.getURI("FMS_Master_PhysicalFileName/ActiveDeactivereferenceconfg", data.fmsrncoN_Id).
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
    }

})();