(function () {
    'use strict';
    angular.module('app').controller('INV_PendingAcknowledgementReportController', INV_PendingAcknowledgementReportController);

    INV_PendingAcknowledgementReportController.$inject = ['$compile', '$rootScope', '$scope', '$http', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$timeout', 'Excel'];

    function INV_PendingAcknowledgementReportController($compile, $rootScope, $scope, $state, $http, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, $timeout, Excel) {

        $scope.searchValue = "";
        $scope.pendingAssetList = [];
        $scope.locationRoomList = [];
        $scope.To_INVMLO_Id = "";
        $scope.allSelected = false;

        // Helper to unwrap $values array from .NET PreserveReferences JSON
        function extractArray(obj) {
            if (!obj) return [];
            if (angular.isArray(obj)) return obj;
            if (obj.$values && angular.isArray(obj.$values)) return obj.$values;
            if (typeof obj === 'object') {
                if (obj.$values && angular.isArray(obj.$values)) return obj.$values;
                var arr = [];
                angular.forEach(obj, function (val, key) {
                    if (key !== '$type' && key !== '$id' && key !== '$values') {
                        if (typeof val === 'object' && val !== null) {
                            arr.push(val);
                        }
                    }
                });
                if (arr.length > 0) return arr;
            }
            return [];
        }

        // Load all pending acknowledgement assets & locations
        $scope.loaddata = function () {
            $scope.pendingAssetList = [];
            $scope.locationRoomList = [];
            $scope.To_INVMLO_Id = "";
            $scope.allSelected = false;

            var data = {
                "Flag": "PendingAcknowledgement"
            };

            apiService.create("INV_LocationDashboard/GetPendingAcknowledgementReport", data).then(function (promise) {
                console.log("GetPendingAcknowledgementReport API Response:", promise);
                if (promise) {
                    // 1. Process Pending / Unselected Assets (Extract from $values if wrapped)
                    var listSource = promise.pendingAssetList || promise.pendingassetlist || promise.unselectedAssetList || promise.unselectedassetlist;
                    var rawList = extractArray(listSource);
                    $scope.pendingAssetList = [];

                    if (rawList && rawList.length > 0) {
                        angular.forEach(rawList, function (item) {
                            if (item && typeof item === 'object') {
                                var empName = item.EmployeeName || item.employeeName || item.EMPLOYEENAME || "Unassigned / Open";
                                var empCode = item.HRME_EmployeeCode || item.hrmE_EmployeeCode || item.employeeCode || "";
                                var deptName = item.HRMD_DepartmentName || item.hrmD_DepartmentName || item.departmentName || "";
                                var itemName = item.INVMI_ItemName || item.invmI_ItemName || item.itemName || "";
                                var assetId = item.INVAAT_AssetId || item.invaaT_AssetId || item.invaA_AssetId || item.assetId || "";
                                var serialNo = item.INVAAT_SerialNo || item.invaaT_SerialNo || item.serialNo || "-";
                                var purchaseRate = (item.INVTGRN_PurchaseRate !== undefined && item.INVTGRN_PurchaseRate !== null) ? item.INVTGRN_PurchaseRate : (item.invtgrN_PurchaseRate || item.purchaseRate || 0);
                                var miName = item.MI_Name || item.mI_Name || item.instituteName || "";
                                var qty = item.INVATCO_CheckOutQty || item.invatcO_CheckOutQty || item.checkOutQty || 1;
                                var roomName = item.INVMLO_LocationRoomName || item.invmlO_LocationRoomName || item.inVMLO_LocationRoomName || item.locationRoomName || item.Floor_Name || "";
                                var assignedDate = item.AssignedDate || item.assignedDate || item.INVATCO_CheckoutDate || item.invatcO_CheckoutDate || item.CreatedDate || item.createdDate || "";
                                var pendingDays = (item.PendingDays !== undefined && item.PendingDays !== null) ? item.PendingDays : (item.pendingDays || 0);
                                var ackStatus = item.AcknowledgementStatus || item.acknowledgementStatus || item.INVATCO_CheckOutRemarks || item.invatcO_CheckOutRemarks || "Pending Acknowledgement";
                                var filePath = item.INVAAT_FilePath || item.invaaT_FilePath || "";
                                var coId = item.INVATCO_Id || item.invatcO_Id || 0;
                                var aatId = item.INVAAT_Id || item.invaaT_Id || 0;
                                var miId = item.MI_Id || item.mI_Id || 0;
                                var invmiId = item.INVMI_Id || item.invmI_Id || 0;

                                $scope.pendingAssetList.push({
                                    selected: false,
                                    INVATCO_Id: coId,
                                    INVAAT_Id: aatId,
                                    MI_Id: miId,
                                    INVMI_Id: invmiId,
                                    EmployeeName: empName,
                                    HRME_EmployeeCode: empCode,
                                    HRMD_DepartmentName: deptName,
                                    INVMI_ItemName: itemName,
                                    INVAAT_AssetId: assetId,
                                    INVAAT_SerialNo: serialNo,
                                    INVTGRN_PurchaseRate: purchaseRate,
                                    MI_Name: miName,
                                    INVATCO_CheckOutQty: qty,
                                    INVMLO_LocationRoomName: roomName,
                                    AssignedDate: assignedDate,
                                    PendingDays: pendingDays,
                                    AcknowledgementStatus: ackStatus,
                                    INVAAT_FilePath: filePath
                                });
                            }
                        });
                    }

                    // 2. Process Location Rooms Dropdown (Extract from $values if wrapped)
                    var locSource = promise.locationRoomList || promise.locationroomlist || promise.locationdetails || promise.locationDetails;
                    var rawLocs = extractArray(locSource);
                    $scope.locationRoomList = [];

                    if (rawLocs && rawLocs.length > 0) {
                        angular.forEach(rawLocs, function (loc) {
                            if (loc && typeof loc === 'object') {
                                var id = loc.INVMLO_Id || loc.invmlO_Id || loc.inVMLO_Id || loc.invmlO_ID || loc.INVMLO_ID || loc.id;
                                var name = loc.INVMLO_LocationRoomName || loc.invmlO_LocationRoomName || loc.inVMLO_LocationRoomName || loc.invmlO_locationroomname || loc.locationRoomName || loc.Room_Name || loc.room_Name || loc.roomName || loc.name;
                                if (id) {
                                    $scope.locationRoomList.push({
                                        INVMLO_Id: id,
                                        INVMLO_LocationRoomName: name || ("Room #" + id)
                                    });
                                }
                            }
                        });
                    }

                    console.log("Processed $scope.pendingAssetList:", $scope.pendingAssetList);
                    console.log("Processed $scope.locationRoomList:", $scope.locationRoomList);
                }
            });
        };

        // Select / Deselect All Checkboxes
        $scope.selectAll = function (allSelected) {
            angular.forEach($scope.pendingAssetList, function (item) {
                item.selected = allSelected;
            });
        };

        // Check if all are selected
        $scope.optionToggled = function () {
            $scope.allSelected = $scope.pendingAssetList.every(function (item) {
                return item.selected;
            });
        };

        // Save Location Transfer
        $scope.saveLocationTransfer = function () {
            var selectedList = [];
            angular.forEach($scope.pendingAssetList, function (item) {
                if (item.selected === true) {
                    selectedList.push({
                        INVATCO_Id: item.INVATCO_Id,
                        INVAAT_Id: item.INVAAT_Id,
                        INVMI_Id: item.INVMI_Id,
                        MI_Id: item.MI_Id
                    });
                }
            });

            if (selectedList.length === 0) {
                swal("Please select at least one asset to transfer.");
                return;
            }

            if (!$scope.To_INVMLO_Id || $scope.To_INVMLO_Id === "" || $scope.To_INVMLO_Id === "0") {
                swal("Please select a target Location/Room.");
                return;
            }

            var data = {
                "To_INVMLO_Id": parseInt($scope.To_INVMLO_Id),
                "selectedPendingAssets": selectedList,
                "locationlistarrays": selectedList
            };

            apiService.create("INV_LocationDashboard/SavePendingAssetLocationTransfer", data).then(function (promise) {
                if (promise && (promise.returnval === true || promise.Flag === "save" || promise.message === "Location Transferred Successfully")) {
                    swal("Location Transferred Successfully!");
                    $scope.To_INVMLO_Id = "";
                    $scope.allSelected = false;
                    $scope.loaddata();
                } else {
                    swal(promise.message || "Failed to transfer location.");
                }
            });
        };

        // Total Gross Amount for filtered list
        $scope.TotalgrossAmount = function () {
            var total = 0;
            if ($scope.pendingAssetList && $scope.pendingAssetList.length > 0) {
                var list = $filter('filter')($scope.pendingAssetList, $scope.searchValue);
                if (list != null && list.length > 0) {
                    for (var i = 0; i < list.length; i++) {
                        total += parseFloat(list[i].INVTGRN_PurchaseRate) || 0;
                    }
                }
            }
            return total;
        };

        // Preview Image Modal
        $scope.previewimg_new = function (img) {
            $scope.imagepreview = img;
            $('#imagemodal').modal('show');
        };

        // Export to Excel
        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'Pending_Acknowledgement_Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = 'Pending_Acknowledgement_Report_' + $filter('date')(new Date(), 'ddMMyyyy_HHmmss') + '.xls';
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        // Print Data
        $scope.printData = function () {
            var innerContents = document.getElementById("printSection").innerHTML;
            var popupWin = window.open('', '_blank', 'width=1100,height=700,scrollbars=no,menubar=no,toolbar=no,location=no,status=no,titlebar=no');
            popupWin.document.open();
            popupWin.document.write('<html><head><title>Pending Acknowledgement Report</title><link rel="stylesheet" type="text/css" href="css/bootstrap.min.css" /><link rel="stylesheet" type="text/css" href="css/style.css" /><style>table { width: 100%; border-collapse: collapse; } th, td { border: 1px solid #ddd; padding: 8px; text-align: center; font-size: 12px; } th { background-color: #0066c0; color: white; }</style></head><body onload="window.print()">' + innerContents + '</body></html>');
            popupWin.document.close();
        };

    }
})();
