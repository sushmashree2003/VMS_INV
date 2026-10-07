(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_VenderApprovalController', INV_VenderApprovalController);

    INV_VenderApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$window', '$sce'];

    function INV_VenderApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $window, $sce) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.searchValue = '';
        $scope.searchValue2 = '';
        $scope.currentPage = 1;
        $scope.currentPage2 = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPage2 = paginationformasters;
        $scope.selectAll = false;
        $scope.pending_vendor_list = [];
        $scope.approved_vendor_list = [];
        $scope.selectedVendorDoc = {};

        //---------------------------------- Load Data (Grid 1 & Grid 2) ----------------------------------
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.currentPage2 = 1;
            $scope.searchValue = "";
            $scope.searchValue2 = "";
            $scope.selectAll = false;

            // Fetch Grid 1 (Pending Vendor Approval - SP: Vender_Approval)
            apiService.create("INV_VenderApproval/getloaddataOne", {}).then(function (promise) {
                if (promise) {
                    $scope.pending_vendor_list = promise.inV_MasterVendorApprovalArray || promise.pending_vendor_list || [];
                    angular.forEach($scope.pending_vendor_list, function (item) {
                        item.selected = false;
                    });
                    $scope.presentCountgrid = $scope.pending_vendor_list ? $scope.pending_vendor_list.length : 0;
                }
            });

            // Fetch Grid 2 (Approved/Processed Vendor Status - SP: fetch_Vendor_UpdateApproval)
            apiService.create("INV_VenderApproval/getloaddataTwo", {}).then(function (promise) {
                if (promise) {
                    $scope.approved_vendor_list = promise.approved_vendor_list || [];
                    $scope.presentCountgrid2 = $scope.approved_vendor_list ? $scope.approved_vendor_list.length : 0;
                }
            });
        };

        //---------------------------------- Checkbox Selection ----------------------------------
        $scope.toggleAll = function () {
            var toggleStatus = $scope.selectAll;
            angular.forEach($scope.pending_vendor_list, function (item) {
                item.selected = toggleStatus;
            });
        };

        $scope.toggleRow = function () {
            $scope.selectAll = $scope.pending_vendor_list.every(function (item) {
                return item.selected;
            });
        };

        //---------------------------------- Approve Vendors (SP: Vendor_UpdateApproval) ----------------------------------
        $scope.approveVendor = function () {
            var selectedVendors = [];
            angular.forEach($scope.pending_vendor_list, function (item) {
                if (item.selected) {
                    selectedVendors.push(item);
                }
            });

            if (selectedVendors.length === 0) {
                swal('Please select at least one vendor to Approve.');
                return;
            }

            swal({
                title: "Are you sure?",
                text: "Do you want to Approve the selected vendor(s)?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Yes, Approve!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: true
            }, function (isConfirm) {
                if (isConfirm) {
                    var promises = [];
                    angular.forEach(selectedVendors, function (item) {
                        var payload = {
                            "MVE_Id": item.mvE_Id || item.MVE_Id || 0,
                            "VAS_Id": item.vaS_Id || item.VAS_Id || item.vaS_ID || item.VAS_ID || 0,
                            "Action": "Approve"
                        };
                        promises.push(apiService.create("INV_VenderApproval/UpdateSP", payload));
                    });

                    $q.all(promises).then(function (results) {
                        swal("Approved!", "Selected vendor(s) approved successfully.", "success");
                        $state.reload();
                    }, function (error) {
                        swal("Error", "An error occurred while approving vendor(s).", "error");
                        $state.reload();
                    });
                }
            });
        };

        //---------------------------------- Reject Vendors (SP: Vendor_UpdateApproval) ----------------------------------
        $scope.rejectVendor = function () {
            var selectedVendors = [];
            angular.forEach($scope.pending_vendor_list, function (item) {
                if (item.selected) {
                    selectedVendors.push(item);
                }
            });

            if (selectedVendors.length === 0) {
                swal('Please select at least one vendor to Reject.');
                return;
            }

            swal({
                title: "Are you sure?",
                text: "Do you want to Reject the selected vendor(s)?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#dd6b55",
                cancelButtonColor: "#3085d6",
                confirmButtonText: "Yes, Reject!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: true
            }, function (isConfirm) {
                if (isConfirm) {
                    var promises = [];
                    angular.forEach(selectedVendors, function (item) {
                        var payload = {
                            "MVE_Id": item.mvE_Id || item.MVE_Id || 0,
                            "VAS_Id": item.vaS_Id || item.VAS_Id || item.vaS_ID || item.VAS_ID || 0,
                            "Action": "Reject"
                        };
                        promises.push(apiService.create("INV_VenderApproval/UpdateSP", payload));
                    });

                    $q.all(promises).then(function (results) {
                        swal("Rejected!", "Selected vendor(s) rejected successfully.", "success");
                        $state.reload();
                    }, function (error) {
                        swal("Error", "An error occurred while rejecting vendor(s).", "error");
                        $state.reload();
                    });
                }
            });
        };

        //---------------------------------- Document Viewing ----------------------------------
        $scope.viewDocument = function (filePath) {
            if (!filePath || filePath.trim() === "" || filePath === "null" || filePath === "undefined") {
                swal("No Document", "No document available to view.", "info");
                return;
            }

            $scope.documentUrl = filePath;
            var cleanPath = filePath.split('?')[0];
            var ext = cleanPath.substring(cleanPath.lastIndexOf('.') + 1).toLowerCase();

            if (ext === 'pdf') {
                $http.get(filePath, { responseType: 'arraybuffer' }).then(function (response) {
                    if (!response || !response.data || response.data.byteLength === 0) {
                        swal("File Not Found", "The document file is empty or missing on the server.", "warning");
                        return;
                    }
                    var file = new Blob([response.data], { type: 'application/pdf' });
                    var fileURL = URL.createObjectURL(file);
                    var container = document.getElementById("pdfContainer");
                    if (container) {
                        container.innerHTML = '';
                        var embed = document.createElement('embed');
                        embed.setAttribute('src', fileURL);
                        embed.setAttribute('type', 'application/pdf');
                        embed.setAttribute('width', '100%');
                        embed.setAttribute('height', '550');
                        container.appendChild(embed);
                    }
                    $('#pdfModal').modal('show');
                }, function (error) {
                    swal("File Not Found", "The requested document was not found on the server.", "warning");
                });
            } else if (ext === 'jpg' || ext === 'jpeg' || ext === 'png' || ext === 'gif' || ext === 'bmp') {
                $http.get(filePath, { responseType: 'blob' }).then(function (response) {
                    if (!response || !response.data || response.data.size === 0) {
                        swal("File Not Found", "The image file is missing on the server.", "warning");
                        return;
                    }
                    var fileURL = URL.createObjectURL(response.data);
                    $scope.imagePreviewUrl = fileURL;
                    $('#imgpreview').attr('src', fileURL);
                    $('#imageModal').modal('show');
                }, function (error) {
                    swal("File Not Found", "The requested image was not found on the server.", "warning");
                });
            } else {
                $window.open(filePath, '_blank');
            }
        };

        $scope.viewAllDocuments = function (row) {
            $scope.selectedVendorDoc = row;
            $('#allDocsModal').modal('show');
        };

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };
    }
})();
