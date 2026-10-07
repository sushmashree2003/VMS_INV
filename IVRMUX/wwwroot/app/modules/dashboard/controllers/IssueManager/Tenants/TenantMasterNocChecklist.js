(function () {
    'use strict';
    angular.module('app').controller('TenantMasterNocChecklistController', TenantMasterNocChecklistController);
    TenantMasterNocChecklistController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$filter', '$window', 'intracontroller'];
    function TenantMasterNocChecklistController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, Excel, $filter, $window, intracontroller) {

        $scope.submitted = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag === "ISMTASK") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }
        $scope.VMSPOD_Type = 'Color';
        $scope.submitted = false;
        $scope.VMSPOD_ToDate = new Date();
        $scope.VMSPOD_FromDate = new Date();
       // $scope.tenantnoccL_CheckedDate = new Date();

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;

            apiService.getURI("TenantMasterNocChecklist/getloaddata", pageid).then(function (promise) {

                $scope.get_tenant = promise.get_tenant;
                $scope.get_amenities = promise.get_amenities;


                $scope.staffmappinglistTemp = promise.staffmappinglist;
            })
        };
        //-----------amenities details change
        $scope.amenitiesdetailschange = function (tenantagR_Id) {
            $scope.get_amenities = [];
            $scope.get_amenitiesmap = [];
            $scope.hrmaM_Id = "";

            var data = {
                "TENANTAGR_Id": $scope.tenantagR_Id
            }
            apiService.create("TenantMasterNocChecklist/getamenity", data).
                then(function (promise) {
                    if (promise.get_amenities != null && promise.get_amenities.length > 0) {
                        $scope.get_amenities = promise.get_amenities;

                    }
                    else {
                        swal("No Record Found....")
                    }
                })
        }

        var filename = "";

        $scope.selectFileforUploadzdBOSBOE = function (input) {
            $scope.SelectedFileForUploadzdBOSBOE = input.files;
            //$('#blah').removeAttr('src');
            if (input.files && input.files[0]) {
                filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                $scope.extention = nameArray[nameArray.length - 1];
                if (($scope.extention === "JPEG" || $scope.extention === "jpg" || $scope.extention === "JPG" || $scope.extention === "png" || $scope.extention === "pdf") && input.files[0].size <= 5242880)  // 2097152 bytes = 2MB , 5242880 bytes = 5MB
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blah')
                            .attr('src', e.target.result);

                        $('#documentid') //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentBOSBOE();
                }
                else if ($scope.extention !== "JPEG" && $scope.extention !== "png" && $scope.extention !== "jpg" && $scope.extention !== "JPG" && $scope.extention !== "pdf") {
                    $('#documentid').removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 5242880) {
                    $('#documentid').removeAttr('src');
                    swal("Document size should be less than 5MB");
                    return;
                }
            }
        };

        function UploadEmployeeDocumentBOSBOE() {

            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadzdBOSBOE.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdBOSBOE[i]);
            }

            formData.append("Id", 0);

            var sample1 = Object.fromEntries(formData.entries());

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadnaacdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {
                        $scope.TENANTNOCCL_FilePath = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#documentid').removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.showmodaldetailsBOSBOE = function () {
            //$('#preview').removeAttr('src');
            var filename = $scope.TENANTNOCCL_FilePath.toString();
            var nameArray = filename.split('.');
            $scope.extention = nameArray[nameArray.length - 1];
            if ($scope.extention === "jpg" || $scope.extention === "jpeg" || $scope.extention === "JPG" || $scope.extention === "JPEG" || $scope.extention === "png"
                || $scope.extention === "PNG") {
                $('#preview').attr('src', $scope.TENANTNOCCL_FilePath);
                $('#myModal').modal('show');
            }
            else if ($scope.extention === "doc" || $scope.extention === "docx" || $scope.extention === "xls" || $scope.extention === "xlsx") {
                //$('#preview').removeAttr('src');
                $('#preview').attr('src', $scope.TENANTNOCCL_FilePath);
            }
            else if ($scope.extention === "pdf") {
                var imagedownload = $scope.TENANTNOCCL_FilePath;
                $scope.content = "";
                var fileURL = "";
                var file = "";
                $http.get(imagedownload, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        $scope.content = $sce.trustAsResourceUrl(fileURL);
                        $('#showpdf').modal('show');
                    });
            }
        };

        $scope.viewdocument = function (item) {

            if (item != null) {
                var img = item.tenantnoccL_FilePath;
                var imagarr = img.split('.');
                var extention = imagarr[imagarr.length - 1];
                item.filetype = extention;
                if (extention === 'doc' || extention === 'docx' || extention === 'ppt' || extention === 'pptx' || extention === 'xlsx' || extention === 'xls') {
                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + item.tenantnoccL_FilePath;
                }
                else if (extention === "jpg" || extention === "JPG" || extention === "jpeg" || extention === "JPEG" || extention === "png" || extention === "PNG") {
                    $('#preview').removeAttr('src');
                    $('#preview').attr('src', item.tenantnoccL_FilePath);
                    $('#myModal').modal();
                    $('#myModal').modal({ keyboard: false });
                    $('#myModal').modal('show');
                }
                else if (extention === "pdf") {
                    var imagedownload = item.tenantnoccL_FilePath;
                    $scope.content = "";
                    var fileURL = "";
                    var file = "";
                    $http.get(imagedownload, { responseType: 'arraybuffer' })
                        .success(function (response) {
                            file = new Blob([(response)], { type: 'application/pdf' });
                            fileURL = URL.createObjectURL(file);
                            $scope.content = $sce.trustAsResourceUrl(fileURL);
                            $('#showpdf1').modal('show');
                        });
                }
            }
        };

        $scope.zoomin = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth >= 750) {
                swal("Maximum zoom-in level reached.");
            } else {
                myImg.style.width = (currWidth + 50) + "px";
            }
        };

        $scope.zoomout = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth <= 400) {
                swal("Maximum zoom-out level reached.");
            } else {
                myImg.style.width = (currWidth - 50) + "px";
            }
        };

        //---------------------------------Save--------------------------------------------
        //Master  Activity_Time 
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "TENANTNOCCL_Id": $scope.tenantnoccL_Id,
                    "TENANTAGR_Id": $scope.tenantagR_Id,
                    "HRMAM_Id": $scope.hrmaM_Id,
                    "TENANTNOCCL_WorkingFlg": $scope.tenantnoccL_WorkingFlg,
                    "TENANTNOCCL_DeductingAmount": $scope.tenantnoccL_DeductingAmount,

                    "TENANTNOCCL_Remarks": $scope.tenantnoccL_Remarks,
                    "TENANTNOCCL_CheckedDate": new Date($scope.tenantnoccL_CheckedDate).toDateString(),
                    "TENANTNOCCL_FilePath": $scope.TENANTNOCCL_FilePath,
                    "TENANTNOCCL_FileName": filename
                    //"TENANTNOCCL_ActiveFlag": $scope.tenantnoccL_ActiveFlag,

                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("TenantMasterNocChecklist/savedetails", data).then(function (promise) {

                    if (promise.returnduplicatestatus == 'Saved') {
                        swal('Record Saved Successfuly');
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus == 'Not Saved') {
                        swal('Record Not Saved Successfuly');
                    }
                    else if (promise.returnduplicatestatus == 'Update') {
                        swal('Record Update Successfuly');
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus == 'Not Update') {
                        swal('Record Not Update Successfuly');
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.hrmaractT_Id == 0 || promise.hrmaractT_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.hrmaractT_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }

                    $scope.submittedUG = false;
                })
            }
            else {
                $scope.submittedUG = true;
            }
        };

        $scope.edit = function (item) {

            var readable_date = new Date(item.date);

            $scope.tenantnoccL_Id = item.tenantnoccL_Id;
            $scope.tenantagR_Id = item.tenantagR_Id;
            $scope.hrmaM_Id = item.hrmaM_Id;
            $scope.tenantnoccL_DeductingAmount = item.damount;
            $scope.tenantnoccL_Remarks = item.remarks;
            $scope.tenantnoccL_CheckedDate = readable_date ;
            $scope.tenantnoccL_WorkingFlg = item.tenantnoccL_WorkingFlg;
            $scope.TENANTNOCCL_FilePath = item.tenantnoccL_FilePath;
            $scope.showmodaldetailsBOSBOE()
            {
                $('#myModal').modal('hide');
            }

        }
        //-------------- deactive  --------------------
        $scope.deactive = function (item) {            $scope.tenantnoccL_Id = item.tenantnoccL_Id;            var dystring = "";            if (item.tenantnoccL_ActiveFlag == true) {                dystring = "Deactivate";            }            else if (item.hrmcoN_ActiveFlg == false) {                dystring = "Activate";            }            swal({                title: "Are you sure?",                text: "Do You Want To " + dystring + " Record?",                type: "warning",                showCancelButton: true,                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",                cancelButtonText: "Cancel",                closeOnConfirm: false,                closeOnCancel: false            },                function (isConfirm) {                    if (isConfirm) {                        apiService.create("TenantMasterNocChecklist/activedeactive", item).                            then(function (promise) {                                if (promise.returnval == true) {                                    swal("Record " + dystring + "d Successfully!!!");                                }                                else {                                    swal("Record Not " + dystring + "d Successfully!!!");                                }                                $state.reload();                            })                    }                    else {                        swal("Record " + dystring + " Cancelled!!!");                    }                });        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        //===========----Clear Field
        $scope.cancel = function () {
            $state.reload();
        };
    }

})();

