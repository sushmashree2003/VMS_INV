(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_MasterVendorEntryController', INV_MasterVendorEntryController);

    INV_MasterVendorEntryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$window'];

    function INV_MasterVendorEntryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $window) {

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
        $scope.mve_Id = 0;

        // Helper to extract array from promise response (handles Newtonsoft $values wrapper)
        function extractArrayData(data) {
            if (!data) return [];
            if (Array.isArray(data)) return data;
            if (data.$values && Array.isArray(data.$values)) return data.$values;
            if (data.values && Array.isArray(data.values)) return data.values;
            return [];
        }

        //---------------------------------- Load Data ----------------------------------
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.searchValue = "";
            var pageid = 2;

            apiService.getURI("INV_MasterVendorEntry/getloaddata", pageid).then(function (promise) {
                if (promise) {
                    var catData = promise.vendorCategoryList || promise.VendorCategoryList || promise.vendorCategoryMasterArray || promise.VendorCategoryMasterArray || promise.vendorCategory || promise.get_vendorcategory;
                    $scope.get_vendorcategory = extractArrayData(catData);

                    var vendorData = promise.gridDataFetch || promise.GridDataFetch || promise.vendorEntryArray || promise.VendorEntryArray;
                    $scope.get_vendor = extractArrayData(vendorData);
                    $scope.presentCountgrid = $scope.get_vendor ? $scope.get_vendor.length : 0;
                }
            });
        };

        //---------------------------------- Save / Update ----------------------------------
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                // Mandatory Document Upload Checks
                if (!$scope.mvE_GST_Certificate_filepath) {
                    swal('Please Upload GST Certificate');
                    return;
                }
                if (!$scope.mvE_PAN_Upload_filepath) {
                    swal('Please Upload PAN Copy');
                    return;
                }
                if (!$scope.mvE_MOU_filepath) {
                    swal('Please Upload MOU');
                    return;
                }
                if (!$scope.mvE_Company_Register_Certificate_filepath) {
                    swal('Please Upload Company Register Certificate');
                    return;
                }
                if (!$scope.mvE_ISO_Certificate_filepath) {
                    swal('Please Upload ISO Certificate');
                    return;
                }

                if ($scope.mve_Agreement_Startdate && $scope.mve_Agreement_Enddate) {
                    if (new Date($scope.mve_Agreement_Enddate) < new Date($scope.mve_Agreement_Startdate)) {
                        swal('Agreement End Date cannot be earlier than Agreement Start Date');
                        return;
                    }
                }

                var data = {
                    "MVE_Id": $scope.mve_Id || 0,
                    "VCM_Id": $scope.vcM_Id,
                    "MVE_VendorName": $scope.mve_VendorName,
                    "MVE_CompanyName": $scope.mve_CompanyName,
                    "MVE_GST_Number": $scope.mve_GST_Number,
                    "MVE_PAN_Number": $scope.mve_PAN_Number,
                    "MVE_CIN_Number": $scope.mve_CIN_Number,
                    "MVE_MSME_Register": $scope.mve_MSME_Register,
                    "MVE_Contact_Person": $scope.mve_Contact_Person,
                    "MVE_MobileNumber": parseInt($scope.mve_MobileNumber, 10),
                    "MVE_Email_Address": $scope.mve_Email_Address,
                    "MVE_Register_OfficeAddress": $scope.mve_Register_OfficeAddress,
                    "MVE_Bank_Name": $scope.mve_Bank_Name,
                    "MVE_Account_name": $scope.mve_Account_name,
                    "MVE_Branch_Name": $scope.mve_Branch_Name,
                    "MVE_Account_Number": $scope.mve_Account_Number,
                    "MVE_IFSC_Code": $scope.mve_IFSC_Code,
                    "MVE_Service_Offered": $scope.mve_Service_Offered,
                    "MVE_Payment_Terms": $scope.mve_Payment_Terms,
                    "MVE_Agreement_Startdate": $scope.mve_Agreement_Startdate,
                    "MVE_Agreement_Enddate": $scope.mve_Agreement_Enddate,

                    // Upload Filepaths
                    "MVE_GST_Certificate_filepath": $scope.mvE_GST_Certificate_filepath || "",
                    "MVE_Company_Register_Certificate_filepath": $scope.mvE_Company_Register_Certificate_filepath || "",
                    "MVE_PAN_Upload_filepath": $scope.mvE_PAN_Upload_filepath || "",
                    "MVE_MSME_Certificate_filepath": $scope.mvE_MSME_Certificate_filepath || "",
                    "MVE_Bank_Details_filepath": $scope.mvE_Bank_Details_filepath || "",
                    "MVE_ISO_Certificate_filepath": $scope.mvE_ISO_Certificate_filepath || "",
                    "MVE_MOU_filepath": $scope.mvE_MOU_filepath || "",
                    "MVE_Workorder_filepath": $scope.mvE_Workorder_filepath || "",
                    "ApprovalStatus": $scope.approvalStatus || "Pending"
                };

                apiService.create("INV_MasterVendorEntry/savedetails", data).then(function (promise) {
                    if (promise.returnval === true) {
                        if (data.MVE_Id > 0) {
                            swal('Record Updated Successfully');
                        } else {
                            swal('Record Saved Successfully');
                        }
                        $state.reload();
                    } else if (promise.saveorupdate === 'Vendor PAN Number Already Exists') {
                        swal('Vendor PAN Number Already Exists');
                    } else {
                        swal(promise.saveorupdate || 'Failed to Save / Update Record');
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.setToDate = function (startDate) {
            if ($scope.mve_Agreement_Enddate && startDate) {
                if (new Date($scope.mve_Agreement_Enddate) < new Date(startDate)) {
                    $scope.mve_Agreement_Enddate = null;
                }
            }
        };

        //---------------------------------- Toggle Active/Deactive ----------------------------------
        $scope.deactive = function (item) {
            var activeFlag = (item.mvE_Activeflag !== undefined && item.mvE_Activeflag !== null) ? item.mvE_Activeflag :
                             (item.MVE_Activeflag !== undefined && item.MVE_Activeflag !== null) ? item.MVE_Activeflag :
                             (item.mve_Activeflag !== undefined && item.mve_Activeflag !== null) ? item.mve_Activeflag :
                             (item.Activeflag !== undefined && item.Activeflag !== null) ? item.Activeflag :
                             (item.activeflag !== undefined && item.activeflag !== null) ? item.activeflag : item.MVE_ActiveFlag;

            var mveId = item.mvE_Id || item.MVE_Id || item.mve_Id || item.MVE_ID || item.mve_id;

            var dystring = activeFlag ? "Deactivate" : "Activate";

            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            }, function (isConfirm) {
                if (isConfirm) {
                    var data = {
                        "MVE_Id": mveId,
                        "MVE_Activeflag": activeFlag
                    };
                    apiService.create("INV_MasterVendorEntry/deactive", data).then(function (promise) {
                        if (promise.returnval === true) {
                            swal("Record " + dystring + "d Successfully!!!");
                        } else {
                            swal("Failed to " + dystring + " Record");
                        }
                        $state.reload();
                    });
                } else {
                    swal("Record " + dystring + " Cancelled!!!");
                }
            });
        };

        //---------------------------------- Edit Record ----------------------------------
        $scope.edit = function (item) {
            $scope.mve_Id = item.mvE_Id || item.MVE_Id || 0;
            $scope.vcM_Id = item.vcM_Id || item.VCM_Id || item.vcm_Id;
            $scope.mve_VendorName = item.mvE_VendorName || item.MVE_VendorName;
            $scope.mve_CompanyName = item.mvE_CompanyName || item.MVE_CompanyName;
            $scope.mve_GST_Number = item.mvE_GST_Number || item.MVE_GST_Number;
            $scope.mve_PAN_Number = item.mvE_PAN_Number || item.MVE_PAN_Number;
            $scope.mve_CIN_Number = item.mvE_CIN_Number || item.MVE_CIN_Number;
            $scope.mve_MSME_Register = item.mvE_MSME_Register || item.MVE_MSME_Register;
            $scope.mve_Contact_Person = item.mvE_Contact_Person || item.MVE_Contact_Person;
            $scope.mve_MobileNumber = item.mvE_MobileNumber || item.MVE_MobileNumber;
            $scope.mve_Email_Address = item.mvE_Email_Address || item.MVE_Email_Address;
            $scope.mve_Register_OfficeAddress = item.mvE_Register_OfficeAddress || item.MVE_Register_OfficeAddress;
            $scope.mve_Bank_Name = item.mvE_Bank_Name || item.MVE_Bank_Name;
            $scope.mve_Account_name = item.mvE_Account_name || item.MVE_Account_name;
            $scope.mve_Branch_Name = item.mvE_Branch_Name || item.MVE_Branch_Name;
            $scope.mve_Account_Number = item.mvE_Account_Number || item.MVE_Account_Number;
            $scope.mve_IFSC_Code = item.mvE_IFSC_Code || item.MVE_IFSC_Code;
            $scope.mve_Service_Offered = item.mvE_Service_Offered || item.MVE_Service_Offered;
            $scope.mve_Payment_Terms = item.mvE_Payment_Terms || item.MVE_Payment_Terms;

            var startDate = item.mvE_Agreement_Startdate || item.MVE_Agreement_Startdate;
            if (startDate) {
                $scope.mve_Agreement_Startdate = new Date(startDate);
            }
            var endDate = item.mvE_Agreement_Enddate || item.MVE_Agreement_Enddate;
            if (endDate) {
                $scope.mve_Agreement_Enddate = new Date(endDate);
            }

            $scope.mvE_GST_Certificate_filepath = item.mvE_GST_Certificate_filepath || item.MVE_GST_Certificate_filepath;
            $scope.mvE_Company_Register_Certificate_filepath = item.mvE_Company_Register_Certificate_filepath || item.MVE_Company_Register_Certificate_filepath;
            $scope.mvE_PAN_Upload_filepath = item.mvE_PAN_Upload_filepath || item.MVE_PAN_Upload_filepath;
            $scope.mvE_MSME_Certificate_filepath = item.mvE_MSME_Certificate_filepath || item.MVE_MSME_Certificate_filepath;
            $scope.mvE_Bank_Details_filepath = item.mvE_Bank_Details_filepath || item.MVE_Bank_Details_filepath;
            $scope.mvE_ISO_Certificate_filepath = item.mvE_ISO_Certificate_filepath || item.MVE_ISO_Certificate_filepath;
            $scope.mvE_MOU_filepath = item.mvE_MOU_filepath || item.MVE_MOU_filepath;
            $scope.mvE_Workorder_filepath = item.mvE_Workorder_filepath || item.MVE_Workorder_filepath;
            $scope.approvalStatus = item.approvalStatus || item.ApprovalStatus || (item.mvE_Activeflag ? 'Approved' : 'Pending');
        };

        //---------------------------------- Helper to get Category Name in Grid ----------------------------------
        $scope.getCategoryName = function (vcmId) {
            if (!$scope.get_vendorcategory) return '';
            var list = extractArrayData($scope.get_vendorcategory);
            var found = list.find(function (cat) {
                var id = cat.vcM_Id || cat.vcm_Id || cat.VCM_Id;
                return id == vcmId;
            });
            return found ? (found.vcM_Vendorname || found.vcm_Vendorname || found.VCM_Vendorname || found.vcM_VendorName || found.VCM_VendorName) : '';
        };

        //---------------------------------- File Upload Logic ----------------------------------
        $scope.selectFileforUpload = function (input, fieldName) {
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
                        swal("File uploaded successfully!");
                    }
                }, function () {
                    swal("File Upload Failed!");
                });
            }
        };

        //---------------------------------- Document Preview ----------------------------------
        $scope.previewimg_new = function (filepath) {
            if (!filepath) {
                swal("No file available to preview.");
                return;
            }
            var ext = filepath.split('.').pop().toLowerCase();
            if (['jpg', 'jpeg', 'png', 'gif'].indexOf(ext) !== -1) {
                $('#preview').attr('src', filepath);
                $('#myimagePreview').modal('show');
            } else if (ext === 'pdf') {
                $http.get(filepath, { responseType: 'arraybuffer' }).then(function (response) {
                    var file = new Blob([response.data], { type: 'application/pdf' });
                    var fileURL = URL.createObjectURL(file);
                    var pdfId = document.getElementById("pdfIdzz");
                    pdfId.innerHTML = '';
                    var embed = document.createElement('embed');
                    embed.setAttribute('src', fileURL);
                    embed.setAttribute('type', 'application/pdf');
                    embed.setAttribute('width', '100%');
                    embed.setAttribute('height', '600');
                    pdfId.appendChild(embed);
                    $('#showpdf').modal('show');
                });
            } else {
                $window.open(filepath, '_blank');
            }
        };

        //---------------------------------- Sorting ----------------------------------
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

    }
})();
