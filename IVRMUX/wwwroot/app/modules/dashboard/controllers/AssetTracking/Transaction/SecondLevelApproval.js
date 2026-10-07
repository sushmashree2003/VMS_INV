(function () {
    'use strict';

    angular
        .module('app')
        .controller('AT_Second_Level_ApprovalController', AT_Second_Level_ApprovalController);

    AT_Second_Level_ApprovalController.$inject = ['$scope', '$state', 'apiService'];

    function AT_Second_Level_ApprovalController($scope, $state, apiService) {

        $scope.obj = {};
        $scope.submitted = false;
        $scope.selectAll = false;
        $scope.projectList = [];
        $scope.checklistGroups = [];

        // Initial Load Data Function
        $scope.loaddata = function () {
            $scope.obj = {
                MI_Id: 2,
                ISMMPR_Id: ""
            };

            var pageid = 2;
            apiService.getURI("AT_System_Category_Checkpoint_Mapping/getProjectClientdetails", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.projectList = promise.projectlist || [];
                }
            });
        };

        // Form Validation Helper
        $scope.interacted = function (field) {
            return $scope.submitted || (field && field.$dirty);
        };

        // Triggered on Project Dropdown Change
        $scope.onProjectChange = function () {
            $scope.checklistGroups = [];
            $scope.selectAll = false;

            if ($scope.obj.ISMMPR_Id) {
                var payload = {
                    "MI_Id": $scope.obj.MI_Id || 2,
                    "ISMMPR_Id": parseInt($scope.obj.ISMMPR_Id),
                    "Approval_Status": "SECOND LEVEL APPROVAL"
                };

                apiService.create("AT_System_Category_Checkpoint_Mapping/firstLevelApproval", payload).then(function (promise) {
                    if (promise !== null && promise.approvalDetailsList) {
                        var rawList = promise.approvalDetailsList || [];
                        var grouped = {};
                        var globalSlNo = 1;

                        angular.forEach(rawList, function (item) {
                            var headerKey = item.systemCategoryHeader || item.SystemCategoryHeader || "General Checklist";

                            if (!grouped[headerKey]) {
                                grouped[headerKey] = {
                                    categoryName: headerKey,
                                    items: []
                                };
                            }

                            // Capture HESCCM_Id with property casing fallback
                            var hesccmId = item.hesccm_Id || item.HESCCM_Id || item.hesccM_Id || 0;

                            grouped[headerKey].items.push({
                                selected: false,
                                hesccm_Id: hesccmId,
                                slNo: globalSlNo++,
                                system: item.system || item.System,
                                checkPoint: item.checkPoint || item.CheckPoint,
                                status: item.statusName || item.StatusName,
                                remarks: item.remarks || item.Remarks,
                                priority: item.priority || item.Priority,
                                checkedBy: item.checkedBy || item.CheckedBy,
                                checkedDate: item.checkedDate || item.CheckedDate
                            });
                        });

                        $scope.checklistGroups = Object.keys(grouped).map(function (key) {
                            return grouped[key];
                        });
                    }
                });
            }
        };

        // Toggle All Checkboxes
        $scope.toggleAll = function (selectAll) {
            angular.forEach($scope.checklistGroups, function (group) {
                angular.forEach(group.items, function (item) {
                    item.selected = selectAll;
                });
            });
        };

        // Sync Header Checkbox State
        $scope.searchValue = function () {
            var allItems = [];
            angular.forEach($scope.checklistGroups, function (group) {
                allItems = allItems.concat(group.items);
            });

            if (allItems.length > 0) {
                $scope.selectAll = allItems.every(function (item) {
                    return !!item.selected;
                });
            } else {
                $scope.selectAll = false;
            }
        };

        // Extract selected HESCCM_Ids safely
        function getSelectedHesccmIds() {
            var selectedIds = [];
            angular.forEach($scope.checklistGroups, function (group) {
                angular.forEach(group.items, function (item) {
                    if (item.selected) {
                        var id = item.hesccm_Id || item.HESCCM_Id || item.hesccM_Id;
                        if (id) {
                            selectedIds.push(parseInt(id));
                        }
                    }
                });
            });
            return selectedIds;
        }

        // Approve Action (Final Level Approval)
        $scope.approve = function () {
            if (!$scope.obj.ISMMPR_Id) {
                swal("Selection Required", "Please select a Project Name first.", "warning");
                return;
            }

            var selectedIds = getSelectedHesccmIds();
            if (selectedIds.length === 0) {
                swal("Selection Required", "Please select at least one checkpoint row to approve.", "warning");
                return;
            }

            swal({
                title: "Are you sure?",
                text: "Do you want to grant Final Approval for the selected Checkpoints?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                confirmButtonText: "Yes, Grant Final Approval!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false
            }, function (isConfirm) {
                if (isConfirm) {
                    var payload = {
                        "MI_Id": $scope.obj.MI_Id || 2,
                        "UserId": $scope.UserId || 1,
                        "ISMMPR_Id": parseInt($scope.obj.ISMMPR_Id),
                        "Approval_Status": "FINAL LEVEL APPROVAL",
                        "selectedHesccmIds": selectedIds
                    };

                    apiService.create("AT_System_Category_Checkpoint_Mapping/saveApproval", payload).then(function (promise) {
                        if (promise && promise.returnval === true) {
                            swal("Approved Successfully!", "Checklist marked as Final Level Approval.", "success");
                            $state.reload();
                        } else {
                            swal("Approval Failed!", "Unable to update approval status.", "error");
                        }
                    });
                }
            });
        };

        // Reject Action (Second Level Reject)
        $scope.reject = function () {
            if (!$scope.obj.ISMMPR_Id) {
                swal("Selection Required", "Please select a Project Name first.", "warning");
                return;
            }

            var selectedIds = getSelectedHesccmIds();
            if (selectedIds.length === 0) {
                swal("Selection Required", "Please select at least one checkpoint row to reject.", "warning");
                return;
            }

            swal({
                title: "Are you sure?",
                text: "Do you want to Reject the selected Checkpoints?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#dd4b39",
                confirmButtonText: "Yes, Reject!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false
            }, function (isConfirm) {
                if (isConfirm) {
                    var payload = {
                        "MI_Id": $scope.obj.MI_Id || 2,
                        "UserId": $scope.UserId || 1,
                        "ISMMPR_Id": parseInt($scope.obj.ISMMPR_Id),
                        "Approval_Status": "SECOND LEVEL REJECT",
                        "selectedHesccmIds": selectedIds
                    };

                    apiService.create("AT_System_Category_Checkpoint_Mapping/saveApproval", payload).then(function (promise) {
                        if (promise && promise.returnval === true) {
                            swal("Rejected Successfully!", "Checklist marked as Second Level Reject.", "success");
                            $state.reload();
                        } else {
                            swal("Action Failed!", "Unable to update approval status.", "error");
                        }
                    });
                }
            });
        };

        $scope.cancel = function () {
            $state.reload();
        };
    }
})();