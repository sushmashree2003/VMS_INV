(function () {
    'use strict';

    angular
        .module('app')
        .controller('AT_System_Category_Checkpoint_MappingController', AT_System_Category_Checkpoint_MappingController);

    AT_System_Category_Checkpoint_MappingController.$inject = [
        '$rootScope', '$scope', '$state', '$location', 'Flash',
        'appSettings', 'apiService', '$http', 'Excel', '$timeout',
        '$filter', 'superCache', '$q', '$sce'
    ];

    function AT_System_Category_Checkpoint_MappingController(
        $rootScope, $scope, $state, $location, Flash,
        appSettings, apiService, $http, Excel, $timeout,
        $filter, superCache, $q, $sce
    ) {

        $scope.obj = {};
        $scope.submitted = false;
        $scope.selectAll = false;
        $scope.projectList = [];
        $scope.clientList = [];
        $scope.statusList = [];
        $scope.checklistData = [];
        $scope.gridDetails = [];
        $scope.reportHeader = {};
        $scope.reportSummary = [];
        $scope.detailedChecklistData = [];

        // Initial Load Function
        $scope.OnLoad = function () {
            $scope.UserId = $scope.UserId || 1;

            $scope.obj = {
                MI_Id: 2,
                ISMMPR_Id: "",
                ISMMCLT_Id: "",
                NextDueDate: new Date(),
                TechnicianName: "",
                CompanyName: "",
                ContactNo: "",
                EmailId: "",
                Designation: ""
            };

            var pageId = 2;

            apiService.getURI("AT_System_Category_Checkpoint_Mapping/getProjectClientdetails", pageId).then(function (promise) {
                if (promise !== null) {
                    $scope.projectList = promise.projectlist || [];
                    $scope.clientList = promise.clientList || [];
                    
                    var rawStatusList = promise.statusList || promise.StatusList || [];
                    if (rawStatusList && rawStatusList.length > 0) {
                        $scope.statusList = rawStatusList.map(function(s) {
                            return {
                                health_Entry_Status_Id: parseInt(s.health_Entry_Status_Id || s.Health_Entry_Status_Id),
                                statusName: s.statusName || s.StatusName
                            };
                        });
                    } else {
                        $scope.statusList = [
                            { health_Entry_Status_Id: 1, statusName: "OK" },
                            { health_Entry_Status_Id: 2, statusName: "Attention" },
                            { health_Entry_Status_Id: 3, statusName: "Fail" },
                            { health_Entry_Status_Id: 4, statusName: "In Progress" },
                            { health_Entry_Status_Id: 5, statusName: "Not Checked" }
                        ];
                    }

                    $scope.gridDetails = promise.gridDataList || [];
                }
            });
        };

        // Project Dropdown Change Handler
        $scope.onProjectChange = function () {
            $scope.checklistData = [];
            $scope.selectAll = false;

            if ($scope.obj.ISMMPR_Id) {
                var payload = {
                    "MI_Id": $scope.obj.MI_Id,
                    "ISMMPR_Id": parseInt($scope.obj.ISMMPR_Id)
                };

                apiService.create("AT_System_Category_Checkpoint_Mapping/getcheckpoints", payload).then(function (promise) {
                    if (promise !== null) {
                        var rawChecklist = promise.systemCategoryCheckpointDtos || [];

                        if (promise.statusList || promise.StatusList) {
                            var rawSt = promise.statusList || promise.StatusList;
                            if (rawSt && rawSt.length > 0) {
                                $scope.statusList = rawSt.map(function(s) {
                                    return {
                                        health_Entry_Status_Id: parseInt(s.health_Entry_Status_Id || s.Health_Entry_Status_Id),
                                        statusName: s.statusName || s.StatusName
                                    };
                                });
                            }
                        }
                        if (!$scope.statusList || $scope.statusList.length === 0) {
                            $scope.statusList = [
                                { health_Entry_Status_Id: 1, statusName: "OK" },
                                { health_Entry_Status_Id: 2, statusName: "Attention" },
                                { health_Entry_Status_Id: 3, statusName: "Fail" },
                                { health_Entry_Status_Id: 4, statusName: "In Progress" },
                                { health_Entry_Status_Id: 5, statusName: "Not Checked" }
                            ];
                        }

                        $scope.checklistData = rawChecklist.map(function (item) {
                            var sId = (item.health_Entry_Status_Id && parseInt(item.health_Entry_Status_Id) > 0) ? parseInt(item.health_Entry_Status_Id) : null;
                            return {
                                selected: (sId !== null && sId > 0) ? true : false,
                                systemCategoryHeader: item.systemCategoryHeader || item.SystemCategoryHeader,
                                system: item.system || item.System,
                                category: item.category || item.Category,
                                checkPoint: item.checkPoint || item.CheckPoint,
                                sccc_Id: item.sccC_Id || item.SCCC_Id,
                                ms_Id: item.mS_Id || item.MS_Id,
                                mc_Id: item.mC_Id || item.MC_Id,
                                health_Entry_Status_Id: sId,
                                remarks: item.remarks || "",
                                priority: item.priority || "Medium"
                            };
                        });

                        if (promise.gridDataList) {
                            $scope.gridDetails = promise.gridDataList;
                        }
                    }
                });
            }
        };

        // Form Validation Helper
        $scope.interacted = function (field) {
            return $scope.submitted || (field && field.$dirty);
        };

        // Checkbox Handlers
        $scope.toggleAll = function (selectAll) {
            angular.forEach($scope.checklistData, function (item) {
                item.selected = selectAll;
            });
        };

        $scope.searchValue = function () {
            if ($scope.checklistData && $scope.checklistData.length > 0) {
                $scope.selectAll = $scope.checklistData.every(function (item) {
                    return item.selected === true;
                });
            } else {
                $scope.selectAll = false;
            }
        };

        // Save Checklist
        $scope.SaveChecklist = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                // Explicit 10-digit mobile number validation
                if ($scope.obj.ContactNo) {
                    var contactStr = $scope.obj.ContactNo.toString().trim();
                    var mobileRegex = /^[6-9]\d{9}$/;
                    if (!mobileRegex.test(contactStr)) {
                        swal("Invalid Contact Number", "Please enter a valid 10-digit mobile number starting with 6, 7, 8, or 9.", "warning");
                        return;
                    }
                }

                var selectedCheckpoints = [];
                angular.forEach($scope.checklistData, function (item) {
                    if (item.selected === true) {
                        selectedCheckpoints.push({
                            SCCC_Id: item.sccc_Id,
                            MS_Id: item.ms_Id,
                            MC_Id: item.mc_Id,
                            Health_Entry_Status_Id: item.health_Entry_Status_Id ? parseInt(item.health_Entry_Status_Id) : null,
                            Remarks: item.remarks || "",
                            Priority: item.priority || "Medium"
                        });
                    }
                });

                if (selectedCheckpoints.length === 0) {
                    swal("Selection Required", "Please select at least one checkpoint from the list.", "warning");
                    return;
                }

                var clientIdStr = $scope.obj.ISMMCLT_Id ? $scope.obj.ISMMCLT_Id.toString() : "";

                var payload = {
                    "UserId": $scope.UserId || 1,
                    "MI_Id": $scope.obj.MI_Id || 2,
                    "ISMMPR_Id": parseInt($scope.obj.ISMMPR_Id),
                    "ISMMCLT_Id": clientIdStr,
                    "Next_Due_Date": $scope.obj.NextDueDate,
                    "Technician_Name": $scope.obj.TechnicianName,
                    "Company_Name": $scope.obj.CompanyName,
                    "Contact_Number": $scope.obj.ContactNo,
                    "Emailid": $scope.obj.EmailId,
                    "Designation": $scope.obj.Designation,
                    "systemCategoryCheckpointDtos": selectedCheckpoints
                };

                apiService.create("AT_System_Category_Checkpoint_Mapping/saveHealthRecord", payload).then(function (promise) {
                    if (promise !== null && promise.returnval === true) {
                        swal("Success", "Record Saved Successfully", "success");
                        $state.reload();
                    } else {
                        swal("Error", "Failed to Save Record", "error");
                    }
                }, function (error) {
                    console.error("API Error:", error);
                    swal("Error", "Error occurred while saving the record.", "error");
                });

            } else {
                $scope.submitted = true;
            }
        };

        // 1. Dashboard Template Modal Handler
        $scope.openReportModal = function (row) {
            var projectId = row.ismmpR_Id || row.ismmpr_Id || row.ISMMPR_Id || $scope.obj.ISMMPR_Id;
            var hcueId = row.hcue_Id || row.hcuE_Id || row.HCUE_Id || 0;

            if (!projectId) {
                swal("Error", "Invalid Project ID associated with this record.", "error");
                return;
            }

            var payload = {
                "MI_Id": $scope.obj.MI_Id || 2,
                "ISMMPR_Id": parseInt(projectId),
                "HCUE_Id": parseInt(hcueId)
            };

            apiService.create("AT_System_Category_Checkpoint_Mapping/getProjectSummaryReport", payload).then(function (promise) {
                if (promise !== null && promise.returnval === true) {
                    var resolvedClient = promise.clientName || row.clientName || row.ismmclT_ClientName || row.ISMMCLT_ClientName;
                    if ((!resolvedClient || resolvedClient === 'N/A') && (promise.ismmclT_Id || row.ismmclT_Id) && $scope.clientList && $scope.clientList.length > 0) {
                        var cid = promise.ismmclT_Id || row.ismmclT_Id;
                        var matchedC = $scope.clientList.find(function(c) { return c.ismmclT_Id == cid; });
                        if (matchedC) { resolvedClient = matchedC.ismmclT_ClientName; }
                    }

                    $scope.reportHeader = {
                        projectName: promise.projectName || row.projectName || 'N/A',
                        technicianName: promise.technician_Name || row.technicianName || 'N/A',
                        inspectionDate: promise.createdDate || row.createdDate,
                        nextDueDate: promise.next_Due_Date || row.nextDueDate,
                        clientName: resolvedClient || 'N/A'
                    };

                    if (promise.summaryReportList && promise.summaryReportList.length > 0) {
                        $scope.reportSummary = promise.summaryReportList;
                        $('#summaryReportModal').modal('show');
                    } else {
                        swal("No Record Found ..... !!");
                    }
                } else {
                    swal("No Record Found ..... !!");
                }
            }, function (error) {
                console.error("API Error in getProjectSummaryReport:", error);
                swal("Error", "Failed to load project summary report.", "error");
            });
        };

        // 2. Checkup Template Modal Handler
        $scope.openDetailedCheckupModal = function (row) {
            var projectId = row.ismmpR_Id || row.ismmpr_Id || row.ISMMPR_Id || $scope.obj.ISMMPR_Id;
            var hcueId = row.hcue_Id || row.hcuE_Id || row.HCUE_Id || 0;

            if (!projectId) {
                swal("Error", "Invalid Project ID associated with this record.", "error");
                return;
            }

            var payload = {
                "MI_Id": $scope.obj.MI_Id || 2,
                "ISMMPR_Id": parseInt(projectId),
                "HCUE_Id": parseInt(hcueId)
            };

            apiService.create("AT_System_Category_Checkpoint_Mapping/getDetailedCheckupReport", payload).then(function (promise) {
                if (promise !== null && promise.returnval === true) {
                    var resolvedClient = promise.clientName || row.clientName || row.ismmclT_ClientName || row.ISMMCLT_ClientName;
                    if ((!resolvedClient || resolvedClient === 'N/A') && (promise.ismmclT_Id || row.ismmclT_Id) && $scope.clientList && $scope.clientList.length > 0) {
                        var cid = promise.ismmclT_Id || row.ismmclT_Id;
                        var matchedC = $scope.clientList.find(function(c) { return c.ismmclT_Id == cid; });
                        if (matchedC) { resolvedClient = matchedC.ismmclT_ClientName; }
                    }

                    $scope.reportHeader = {
                        projectName: promise.projectName || row.projectName || 'N/A',
                        technicianName: promise.technician_Name || row.technicianName || 'N/A',
                        inspectionDate: promise.createdDate || row.createdDate,
                        nextDueDate: promise.next_Due_Date || row.nextDueDate,
                        clientName: resolvedClient || 'N/A'
                    };

                    // Read the detailed checklist list array from backend
                    var rawDetailedList = promise.detailedReportList || promise.systemCategoryCheckpointDtos || [];

                    if (rawDetailedList && rawDetailedList.length > 0) {
                        $scope.detailedChecklistData = rawDetailedList.map(function(item) {
                            var statusName = item.statusName || item.StatusName || "";
                            if (!statusName && item.health_Entry_Status_Id && $scope.statusList && $scope.statusList.length > 0) {
                                var matchedStatus = $scope.statusList.find(function(s) {
                                    return s.health_Entry_Status_Id == item.health_Entry_Status_Id;
                                });
                                if (matchedStatus) {
                                    statusName = matchedStatus.statusName;
                                }
                            }
                            if (!statusName && item.health_Entry_Status_Id) {
                                statusName = "OK";
                            }
                            return {
                                systemCategoryHeader: item.systemCategoryHeader || item.SystemCategoryHeader,
                                system: item.system || item.System,
                                category: item.category || item.Category,
                                checkPoint: item.checkPoint || item.CheckPoint,
                                sccc_Id: item.sccc_Id || item.SCCC_Id,
                                ms_Id: item.ms_Id || item.MS_Id,
                                mc_Id: item.mc_Id || item.MC_Id,
                                health_Entry_Status_Id: item.health_Entry_Status_Id || item.Health_Entry_Status_Id,
                                statusName: statusName || "OK",
                                remarks: item.remarks || item.Remarks || "",
                                priority: item.priority || item.Priority || "Medium",
                                checkedBy: item.checkedBy || item.CheckedBy || promise.technician_Name || row.technicianName || "-",
                                verifiedBy: item.verifiedBy || item.VerifiedBy || "-",
                                checkedDate: item.checkedDate || item.CheckedDate || promise.createdDate || row.createdDate
                            };
                        });
                        $('#checkupTemplateModal').modal('show');
                    } else {
                        swal("No Detailed Checklist Records Found!");
                    }
                } else {
                    swal("No Record Found ..... !!");
                }
            }, function (error) {
                console.error("API Error in getDetailedCheckupReport:", error);
                swal("Error", "Failed to load detailed checkup report.", "error");
            });
        };

        // Total Sum Helper
        $scope.getTotalSum = function (column) {
            var total = 0;
            if ($scope.reportSummary && $scope.reportSummary.length > 0) {
                angular.forEach($scope.reportSummary, function (item) {
                    total += parseInt(item[column] || 0);
                });
            }
            return total;
        };
        $scope.signOffHeader = {};

        // 3. Sign Off Template Modal Handler
        $scope.openSignOffModal = function (row) {
            var projectId = row.ismmpR_Id || row.ismmpr_Id || row.ISMMPR_Id || $scope.obj.ISMMPR_Id;
            var hcueId = row.hcue_Id || row.hcuE_Id || row.HCUE_Id || 0;

            if (!projectId) {
                swal("Error", "Invalid Project ID associated with this record.", "error");
                return;
            }

            var payload = {
                "MI_Id": $scope.obj.MI_Id || 2,
                "ISMMPR_Id": parseInt(projectId),
                "HCUE_Id": parseInt(hcueId)
            };

            apiService.create("AT_System_Category_Checkpoint_Mapping/getSignOffReportDetails", payload).then(function (promise) {
                if (promise !== null && promise.returnval === true) {
                    $scope.details = promise.inspectionDetailsSignOff || {};
                    $scope.summary = promise.inspectionSummarySignOff || {};
                    $scope.tech = promise.technicalSignOff || {};
                    $scope.client = promise.clientOwnerVerification || {};

                    // Client Name resolution
                    var resolvedClient = $scope.client.clientName || promise.clientName || row.clientName || row.ismmclT_ClientName || row.ISMMCLT_ClientName;
                    if ((!resolvedClient || resolvedClient === 'N/A' || resolvedClient === '') && (row.ismmclT_Id || promise.ismmclT_Id || row.client_Id) && $scope.clientList && $scope.clientList.length > 0) {
                        var cid = row.ismmclT_Id || promise.ismmclT_Id || row.client_Id;
                        var matchedC = $scope.clientList.find(function(c) { return c.ismmclT_Id == cid; });
                        if (matchedC) { resolvedClient = matchedC.ismmclT_ClientName; }
                    }
                    $scope.client.clientName = resolvedClient || row.clientName || 'N/A';

                    // Ensure Technician Name strictly matches the entry record
                    if (!$scope.tech.technicianName || $scope.tech.technicianName === 'N/A' || $scope.tech.technicianName === '') {
                        $scope.tech.technicianName = row.technicianName || promise.technician_Name || 'N/A';
                    }
                    if (!$scope.details.projectName || $scope.details.projectName === 'N/A' || $scope.details.projectName === '') {
                        $scope.details.projectName = row.projectName || promise.projectName || 'N/A';
                    }
                    if (!$scope.details.inspectionDate) {
                        $scope.details.inspectionDate = row.createdDate || promise.createdDate;
                    }
                    if (!$scope.details.nextDueDate) {
                        $scope.details.nextDueDate = row.nextDueDate || promise.next_Due_Date;
                    }

                    $('#signOffTemplateModal').modal('show');
                } else {
                    swal("No Record Found ..... !!");
                }
            }, function (error) {
                console.error("API Error in getSignOffReportDetails:", error);
                swal("Error", "Failed to load Sign-off report details.", "error");
            });
        };

        // Print Sign Off Report Function
        $scope.printSignOffReportData = function () {
            var innerContents = document.getElementById("printSignOffReport").innerHTML;
            var popupWindow = window.open('');
            popupWindow.document.open();
            popupWindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' +
                innerContents + '</body></html>');
            popupWindow.document.close();
        };
        // Print Functions
        $scope.printReportData = function () {
            var innerContents = document.getElementById("printSummaryReport").innerHTML;
            var popupWindow = window.open('');
            popupWindow.document.open();
            popupWindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' +
                innerContents + '</body></html>');
            popupWindow.document.close();
        };

        $scope.printCheckupReportData = function () {
            var innerContents = document.getElementById("printCheckupReport").innerHTML;
            var popupWindow = window.open('');
            popupWindow.document.open();
            popupWindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' +
                innerContents + '</body></html>');
            popupWindow.document.close();
        };

        $scope.ClearForm = function () {
            $state.reload();
        };
    }
})();