(function () {
    'use strict';
    angular
        .module('app')
        .controller('AT_System_Category_Checkpoint_MappingController', AT_System_Category_Checkpoint_MappingController);

    AT_System_Category_Checkpoint_MappingController.$inject = ['$scope', '$state', 'apiService'];
    function AT_System_Category_Checkpoint_MappingController($scope, $state, apiService) {

        $scope.submitted = false;
        $scope.TotalItems = 0;
        $scope.projectlist = [];
        $scope.systemlist = [];
        $scope.categorylist = [];
        $scope.checkpointlist = [];
        $scope.alldata = [];

        // Load dropdowns, checklists, and grid data
        $scope.Loaddata = function () {
            var pageid = 2;

            apiService.getURI("AT_System_Category_Checkpoint_Mapping/getdetails", pageid)
                .then(function (promise) {

                    console.log("Promise :", promise);

                    function getArray(obj) {
                        if (!obj) return [];
                        if (Array.isArray(obj)) return obj;
                        if (obj.$values) return obj.$values;
                        return [];
                    }

                    $scope.projectlist = getArray(promise.projectlist || promise.Projectlist);
                    $scope.systemlist = getArray(promise.systemlist || promise.Systemlist);
                    $scope.categorylist = getArray(promise.categorylist || promise.Categorylist);
                    $scope.checkpointlist = getArray(promise.checkpointlist || promise.Checkpointlist);

                    // Bind Grid Data
                    $scope.alldata = getArray(promise.alldata || promise.Alldata);

                    console.log("Projects:", $scope.projectlist);
                    console.log("Grid Data:", $scope.alldata);
                });
        };

        // Checkbox All / Select Individual Count logic
        $scope.isAllSelected = false;

        $scope.toggleAllCheckpoints = function () {
            var toggleStatus = $scope.isAllSelected;
            angular.forEach($scope.checkpointlist, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.countSelectedCheckpoints();
        };

        // Calculates Total Items automatically and updates the disabled input field
        $scope.countSelectedCheckpoints = function () {
            var count = 0;
            angular.forEach($scope.checkpointlist, function (itm) {
                if (itm.selected) {
                    count++;
                }
            });
            $scope.TotalItems = count;
            $scope.isAllSelected = $scope.checkpointlist.length > 0 && count === $scope.checkpointlist.length;
        };

        // Save Record
        $scope.saverecord = function () {
            if ($scope.myForm.$valid) {
                var selected_cp = [];
                angular.forEach($scope.checkpointlist, function (itm) {
                    if (itm.selected) {
                        selected_cp.push(itm);
                    }
                });

                if (selected_cp.length === 0) {
                    swal("Please select at least one Checkpoint!");
                    return;
                }

                var data = {
                    "ISMMPR_Id": $scope.ISMMPR_Id,
                    "MS_Id": $scope.MS_Id,
                    "MC_Id": $scope.MC_Id,
                    "TotalItems": $scope.TotalItems,
                    "selectedCheckpoints": selected_cp
                };

                apiService.create("AT_System_Category_Checkpoint_Mapping/saverecord", data).then(function (promise) {
                    var returnval = promise.returnValue !== undefined ? promise.returnValue : promise.returnval;
                    if (returnval === true) {
                        swal("Saved Successfully!");
                        $state.reload();
                    } else {
                        swal("Save Failed!");
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        // Activate / Deactivate Toggle Logic
        $scope.deactive = function (user) {
            var activeStatus = (user.activeFlag !== undefined) ? user.activeFlag : user.ActiveFlag;
            var confirmMessage = activeStatus === true ? "Deactivate" : "Activate";

            swal({
                title: "Are you sure?",
                text: "Do you want to " + confirmMessage + " this record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + confirmMessage + " it!",
                closeOnConfirm: false
            }, function (isConfirm) {
                if (isConfirm) {
                    var data = {
                        "ATSCCM_Id": user.atsccm_Id || user.ATSCCM_Id || user.sccc_Id || user.SCCC_Id
                    };

                    apiService.create("AT_System_Category_Checkpoint_Mapping/deactivate", data).then(function (promise) {
                        var returnval = promise.returnValue !== undefined ? promise.returnValue : promise.returnval;
                        if (returnval === true) {
                            swal("Record " + confirmMessage + "d Successfully!");
                            $state.reload();
                        } else {
                            swal("Operation Failed!");
                        }
                    });
                }
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();