(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_VendorSignOffController', INV_VendorSignOffController);

    INV_VendorSignOffController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$window', '$sce', '$timeout'];

    function INV_VendorSignOffController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $window, $sce, $timeout) {

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
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.invmsO_Id = 0;
        $scope.vendorList = [];
        $scope.signoff_list = [];
        $scope.selectedVendorName = '';
        $scope.selectedCompanyName = '';

        //---------------------------------- Load Initial Data ----------------------------------
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.searchValue = "";
            $scope.clearFields();

            var pageid = 2;
            apiService.getURI("INV_VendorSignOff/getloaddata", pageid).then(function (promise) {
                if (promise) {
                    $scope.vendorList = promise.vendorList || promise.vendroNameDropdownArray || promise.VendroNameDropdownArray || [];
                    $scope.signoff_list = promise.signOffList || promise.signoff_list || promise.vendrosignoffGridArray || promise.VendrosignoffGridArray || [];
                    $scope.presentCountgrid = $scope.signoff_list ? $scope.signoff_list.length : 0;
                }
            });
        };

        //---------------------------------- Vendor Selection Change ----------------------------------
        $scope.onVendorChange = function () {
            if ($scope.vendorList && $scope.vendorList.length > 0 && $scope.mve_Id) {
                var selectedObj = $scope.vendorList.find(function (item) {
                    return (item.mvE_Id || item.mve_Id || item.MVE_Id) == $scope.mve_Id;
                });
                if (selectedObj) {
                    $scope.selectedVendorName = selectedObj.mvE_VendorName || selectedObj.mve_VendorName || selectedObj.MVE_VendorName || '';
                    $scope.selectedCompanyName = selectedObj.mvE_CompanyName || selectedObj.mve_CompanyName || selectedObj.MVE_CompanyName || '';
                } else {
                    $scope.selectedVendorName = '';
                    $scope.selectedCompanyName = '';
                }
            } else {
                $scope.selectedVendorName = '';
                $scope.selectedCompanyName = '';
            }
        };

        //---------------------------------- Save / Update Record ----------------------------------
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if (!$scope.invmsO_MOU_FilePath) {
                    swal('Please Upload MOU File');
                    return;
                }
                if (!$scope.invmsO_WorkOrder_FilePath) {
                    swal('Please Upload Workorder File');
                    return;
                }

                $scope.onVendorChange();

                $timeout(function () {
                    var msg = document.getElementById("VendorSignOffMail") ? document.getElementById("VendorSignOffMail").innerHTML : "";

                    var data = {
                        "INVMSO_Id": $scope.invmsO_Id || 0,
                        "MVE_Id": $scope.mve_Id,
                        "INVMSO_SignOffDate": $scope.invmsO_SignOffDate,
                        "INVMSO_MOU_FilePath": $scope.invmsO_MOU_FilePath || "",
                        "INVMSO_WorkOrder_FilePath": $scope.invmsO_WorkOrder_FilePath || "",
                        "htmldata": msg,
                        "msg": msg
                    };

                    apiService.create("INV_VendorSignOff/savedetails", data).then(function (promise) {
                        if (promise.returnval === true) {
                            swal(promise.saveorupdate || 'Record Saved Successfully');
                            $scope.loaddata();
                        } else {
                            swal(promise.saveorupdate || 'Failed to Save / Update Record');
                        }
                    });
                }, 0);
            } else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //---------------------------------- Cancel / Reset Form ----------------------------------
        $scope.cancel = function () {
            $scope.clearFields();
        };

        $scope.clearFields = function () {
            $scope.invmsO_Id = 0;
            $scope.mve_Id = "";
            $scope.invmsO_SignOffDate = null;
            $scope.invmsO_MOU_FilePath = "";
            $scope.invmsO_WorkOrder_FilePath = "";
            $scope.mouFileName = "";
            $scope.workOrderFileName = "";
            $scope.selectedVendorName = "";
            $scope.selectedCompanyName = "";
            $scope.submitted = false;
            if ($scope.myForm) {
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
        };

        //---------------------------------- Edit Record ----------------------------------
        $scope.edit = function (item) {
            $scope.invmsO_Id = item.invmsO_Id || item.INVMSO_Id;
            $scope.mve_Id = item.mvE_Id || item.MVE_Id;
            var dateVal = item.invmsO_SignOffDate || item.INVMSO_SignOffDate;
            if (dateVal) {
                $scope.invmsO_SignOffDate = new Date(dateVal);
            }
            $scope.invmsO_MOU_FilePath = item.invmsO_MOU_FilePath || item.INVMSO_MOU_FilePath || "";
            $scope.invmsO_WorkOrder_FilePath = item.invmsO_WorkOrder_FilePath || item.INVMSO_WorkOrder_FilePath || "";
            $scope.mouFileName = $scope.invmsO_MOU_FilePath ? $scope.invmsO_MOU_FilePath.split('/').pop() : "";
            $scope.workOrderFileName = $scope.invmsO_WorkOrder_FilePath ? $scope.invmsO_WorkOrder_FilePath.split('/').pop() : "";
            $scope.selectedVendorName = item.mvE_VendorName || item.mve_VendorName || item.MVE_VendorName || "";
            $scope.selectedCompanyName = item.mvE_CompanyName || item.mve_CompanyName || item.MVE_CompanyName || "";
        };

        //---------------------------------- Deactive / Activate ----------------------------------
        $scope.deactive = function (item) {
            var id = item.invmsO_Id || item.INVMSO_Id;
            var flag = item.invmsO_ActiveFlg !== undefined ? item.invmsO_ActiveFlg : item.INVMSO_ActiveFlg;
            var dystring = flag ? "Deactivate" : "Activate";

            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + dystring + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            }, function (isConfirm) {
                if (isConfirm) {
                    var data = {
                        "INVMSO_Id": id,
                        "INVMSO_ActiveFlg": flag
                    };
                    apiService.create("INV_VendorSignOff/deactive", data).then(function (promise) {
                        if (promise.returnval === true) {
                            swal("Record " + dystring + "d Successfully!!!");
                        } else {
                            swal("Failed to " + dystring + " Record");
                        }
                        $scope.loaddata();
                    });
                } else {
                    swal("Record " + dystring + " Cancelled!!!");
                }
            });
        };

        //---------------------------------- File Upload Logic ----------------------------------
        $scope.uploadFile = function (input, fieldName) {
            if (input.files && input.files[0]) {
                var file = input.files[0];
                var validTypes = [
                    "image/jpeg", "image/png", "image/jpg", "application/pdf",
                    "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                ];

                if (file.size > 5242880) { // 5MB limit
                    swal("File size should be less than 5 MB");
                    return;
                }

                var formData = new FormData();
                formData.append("File", file);

                $http.post("/api/ImageUpload/Uploadtrnsportdocuments", formData, {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).then(function (response) {
                    if (response.data) {
                        $scope[fieldName] = response.data;
                        if (fieldName === 'invmsO_MOU_FilePath') {
                            $scope.mouFileName = file.name;
                        } else if (fieldName === 'invmsO_WorkOrder_FilePath') {
                            $scope.workOrderFileName = file.name;
                        }
                        swal("File uploaded successfully!");
                    }
                }, function () {
                    swal("File Upload Failed!");
                });
            }
        };

        //---------------------------------- Preview Document Modal ----------------------------------
        $scope.previewDocument = function (filepath) {
            if (!filepath) {
                swal("No file available to preview.");
                return;
            }
            var ext = filepath.split('.').pop().toLowerCase();
            if (ext.indexOf('?') !== -1) {
                ext = ext.split('?')[0];
            }

            if (['jpg', 'jpeg', 'png', 'gif', 'bmp'].indexOf(ext) !== -1) {
                $scope.imgpath = filepath;
                $('#preview').attr('src', filepath);
                $('#myimagePreview').modal('show');
            } else if (ext === 'pdf') {
                $scope.imgpath = filepath;
                var pdfId = document.getElementById("pdfIdzz");
                if (pdfId) {
                    pdfId.innerHTML = '<iframe src="' + filepath + '" style="width:100%; height:600px;" frameborder="0"></iframe>';
                }
                $('#showpdf').modal('show');
            } else {
                $window.open(filepath, '_blank');
            }
        };

        $scope.previewimg_new = $scope.previewDocument;

        //---------------------------------- Sorting Helper ----------------------------------
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

    }
})();
