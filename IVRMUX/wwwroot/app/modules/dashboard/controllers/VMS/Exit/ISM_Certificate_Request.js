
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_Certificate_RequestController', ISM_Certificate_RequestController);
    ISM_Certificate_RequestController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce']
    function ISM_Certificate_RequestController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce) {


        $scope.obj = {}

        $scope.obj.ISMCERTREQ_RequestDate = new Date();
        $scope.editflag = false;
        $scope.itemsPerPage = 10;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0
        }



        //-------------------------------------------------------------------

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("ISM_Certificate_Process/getloaddata", pageid).
                then(function (promise) {
                    if (promise.display_data != null && promise.display_data.length > 0) {
                        $scope.get_data = promise.display_data;
                        $scope.presentCountgrid = $scope.get_data.length;

                        $scope.desabledleave = false;
                        angular.forEach($scope.get_data, function (grdobj) {
                            if (grdobj.ismcertreQ_AppRejFlag == 'Pending' || grdobj.ismcertreQ_AppRejFlag == 'Partial Approved' || grdobj.ismcertreQ_AppRejFlag == 'Approved') {
                                $scope.desabledleave = true;
                            }
                        });
                    }
                    if (promise.document != null && promise.document.length > 0) {
                        $scope.documentlist = promise.document;
                    }

                    if (promise.getloaddetails != null && promise.getloaddetails.length > 0) {
                        $scope.getloaddetails = promise.getloaddetails;
                        $scope.ISMCERTDISDET_ReceivedMode = $scope.getloaddetails[0].ISMCERTDISDET_ReceivedMode;
                        $scope.ISMCERTDISDET_ReceivedateTime = $scope.getloaddetails[0].ISMCERTDISDET_ReceivedateTime;
                        $scope.ISMCERTDISDET_FineAmount = $scope.getloaddetails[0].ISMCERTDISDET_FineAmount;
                        $scope.ISMCERTDISDET_FileName = $scope.getloaddetails[0].ISMCERTDISDET_FileName;
                        $scope.ISMCERTDISDET_Remarks = $scope.getloaddetails[0].ISMCERTDISDET_Remarks;
                        $scope.ISMCERTDISDET_ReceivingFromDate = $scope.getloaddetails[0].ISMCERTDISDET_ReceivingFromDate;
                        $scope.ISMCERTDISDET_ReceivingToDate = $scope.getloaddetails[0].ISMCERTDISDET_ReceivingToDate;
                        $scope.ISMCERTREQAPP_ApprovedDate = $scope.getloaddetails[0].ISMCERTREQAPP_ApprovedDate;
                        $scope.ISMCERTREQAPP_ReceiningDate = $scope.getloaddetails[0].ISMCERTREQAPP_ReceiningDate;
                        $scope.ISMCERTREQAPP_Remarks = $scope.getloaddetails[0].ISMCERTREQAPP_Remarks;
                        $scope.ISMCERTREQAPP_FinalFlg = $scope.getloaddetails[0].ISMCERTREQAPP_FinalFlg;
                        $scope.ISMCERTREQAPP_AppRejFlag = $scope.getloaddetails[0].ISMCERTREQAPP_AppRejFlag;
                        $scope.ISMCERTREQAPP_RecivingTime = $scope.getloaddetails[0].ISMCERTREQAPP_RecivingTime;
                        $scope.ISMCERTREQAPP_ACK = $scope.getloaddetails[0].ISMCERTREQAPP_ACK;
                        $scope.ISMCERTREQ_RequestDate = $scope.getloaddetails[0].ISMCERTREQ_RequestDate;
                        $scope.ISMCERTREQ_Remarks = $scope.getloaddetails[0].ISMCERTREQ_Remarks;
                        $scope.ISMCERTREQ_ReceivingMode = $scope.getloaddetails[0].ISMCERTREQ_ReceivingMode;
                        $scope.ISMCERTREQ_FileName = $scope.getloaddetails[0].ISMCERTREQ_FileName;
                        $scope.ISMCERTREQ_FilePath = $scope.getloaddetails[0].ISMCERTREQ_FilePath;
                        $scope.ISMCERTREQ_FilePath = $scope.getloaddetails[0].ISMCERTREQ_AppRejFlag;
                        $scope.ISMCERTREQ_RecivingAddress = $scope.getloaddetails[0].ISMCERTREQ_RecivingAddress;
                        $scope.AuthorisedEmployee = $scope.getloaddetails[0].AuthorisedEmployee;
                    }


                    if (promise.ismcertreQ_FilePath != null && promise.ismcertreQ_FilePath.length > 0) {
                        $scope.ISMCERTREQAPP_ACK = promise.ismcertreQ_FilePath;
                    }
                    if (promise.hrmE_DOJ != null && promise.hrmE_DOJ.length > 0) {
                        $scope.HRME_DOJ = promise.hrmE_DOJ;
                        var dt1 = new Date($scope.HRME_DOJ);
                        var dt2 = new Date();

                        $scope.eligibilitymonths = 0;

                        var diff = (dt2.getTime() - dt1.getTime()) / 1000;
                        diff /= (60 * 60 * 24 * 7 * 4);
                        $scope.eligibilitymonths = Math.abs(Math.round(diff));

                    }



                    $scope.todate = true;



                })
        };





        //---------------------------------Save--------------------------------------------

        $scope.savedata = function () {
            $scope.submitted = true;
            $scope.from_date = new Date($scope.ISMCERTREQ_RequestDate).toDateString();
            $scope.files_paths = [];
            var ISMCERTREQ_RecivingAddress = "";
            if ($scope.obj.ISMCERTREQ_ReceivingMode == "Courier") {
                ISMCERTREQ_RecivingAddress = $scope.obj.ISMCERTREQ_RecivingAddress
            }
            if ($scope.myForm.$valid) {
                var data = {
                    "ISMCERTREQ_Id": $scope.ismcertreQ_Id,
                    // "HRME_Id": $scope.HRME_Id,
                    "ISMCERTREQ_RequestDate": $scope.obj.ISMCERTREQ_RequestDate,
                    // "ISMCERTREQ_CertificateName": $scope.ISMCERTREQ_CertificateName,
                    "ISMCERTREQ_Remarks": $scope.obj.ISMCERTREQ_Remarks,
                    "ISMCERTREQ_ReceivingMode": $scope.obj.ISMCERTREQ_ReceivingMode,
                    "ISMCERTREQ_FileName": $scope.ismcertreQ_FileName,
                    "ISMCERTREQ_FilePath": $scope.ismcertreQ_FilePath,
                    "ISMCERTREQ_RecivingAddress": ISMCERTREQ_RecivingAddress
                }
                apiService.create("ISM_Certificate_Process/savedetails", data).then(function (promise) {
                    if (promise.returnval == true) {
                        if (promise.ismcertreQ_Id == 0 || promise.ismcertreQ_Id < 0) {
                            swal('Record saved successfully');
                            $state.reload();
                        }
                        else if (promise.ismcertreQ_Id > 0) {
                            swal('Record updated successfully');
                            $state.reload();
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else if (promise.returnduplicatestatus == 'Applied') {
                        swal('Applied already');
                    }
                    else if (promise.returnduplicatestatus == 'AuthoritySetting') {
                        swal('Mapping Is Not Done For Approval');
                    }
                    else {
                        swal('Failed to save, please contact administrator');
                    }

                })
            }
            else {
                $scope.submitted = true;
            }
        };



        $scope.documentListOtherDetails = [{ id: 'document' }];
        $scope.selectFileforUploadzdOtherDetail = function (input, document) {
            $scope.ldr = true;
            //$('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdOtherDetail = input.files;
            if (input.files && input.files[0]) {


                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetail(document);

            }
        };
        function UploadEmployeeDocumentOtherDetail(data) {

            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetail.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetail[i]);
            }

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_ISMAttachment_new", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {

                        //data.file = d[0].path;
                        //data.ismcertreQ_FileName = d[0].name;

                        $scope.ismcertreQ_FilePath = d[0].path;;
                        $scope.ismcertreQ_FileName = d[0].name;



                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        //=============== view doc==========

        $scope.previewimg_new = function (img) {
            $scope.imagepreview = img;
            $scope.view_videos = [];
            var img = $scope.imagepreview;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }

            if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {
                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');
            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
            }

            else if ($scope.filetype2 == 'pdf') {
                ///=====================show pdf, img
                $('#showpdf').modal('hide');
                var imagedownload1 = "";
                imagedownload1 = $scope.imagepreview;
                $http.get(imagedownload1, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        var fileURL = "";
                        var file = "";
                        var embed = "";
                        var pdfId = "";
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        pdfId = document.getElementById("pdfIdzz");
                        pdfId.removeChild(pdfId.childNodes[0]);
                        embed = document.createElement('embed');
                        embed.setAttribute('src', fileURL);
                        embed.setAttribute('type', 'application/pdf');
                        embed.setAttribute('width', '100%');
                        embed.setAttribute('height', '1000');
                        pdfId.appendChild(embed);
                        $('#showpdf').modal('show');
                    });
            }
            else {
                $window.open($scope.imagepreview)
            }
        };

        $scope.Edit = function (item) {
            var data = {
                "ISMCERTREQ_Id": item.ismcertreQ_Id,
            };

            apiService.create("ISM_Certificate_Process/Edit", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getloaddetails = promise.getloaddetails;
                    $scope.editflag = true;



                    $scope.ismcertreQ_Id = promise.getloaddetails[0].ismcertreQ_Id;
                    //$scope.obj.ISMCERTREQ_CertificateName = promise.getloaddetails[0].ismcertreQ_CertificateName;
                    $scope.obj.ISMCERTREQ_ReceivingMode = promise.getloaddetails[0].ismcertreQ_ReceivingMode;
                    $scope.obj.ISMCERTREQ_RequestDate = promise.getloaddetails[0].ismcertreQ_RequestDate;
                    $scope.obj.ISMCERTREQ_Remarks = promise.getloaddetails[0].ismcertreQ_Remarks;
                    $scope.obj.ISMCERTREQ_RecivingAddress = promise.getloaddetails[0].ismcertreQ_RecivingAddress;

                    if (promise.getloaddetails[0].ismcertreQ_FilePath.length > 0) {
                        $scope.ismcertreQ_FilePath = promise.getloaddetails[0].ismcertreQ_FilePath;

                    }
                    else {

                        $scope.files_paths = [{ id: 'document' }];
                    }

                    if (promise.getloaddetails[0].ismcertreQ_FileName.length != null || promise.getloaddetails[0].ismcertreQ_FileName.length != '') {
                        $scope.ismcertreQ_FileName = promise.getloaddetails[0].ismcertreQ_FileName;
                    }
                    else {
                        $scope.documentListOtherDetails = [{ id: 'document' }];
                    }



                }
            });
        }


        $scope.viewaproval = function (user) {
            var data = {
                "ISMCERTREQ_Id": user.ismcertreQ_Id
            };
            apiService.create("ISM_Certificate_Process/Viewdata", data).
                then(function (promise) {
                    if (promise.modalview !== null && promise.modalview.length > 0) {
                        $scope.modalview = promise.modalview;
                    }
                    else {
                        swal(" First Level is Not Approved  !");
                        $('#mymodalviewdetailssecondtab').modal('hide');
                    }
                });
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };


        $scope.deactive = function (item, SweetAlert) {            $scope.ISMCERTREQ_Id = item.ismcertreQ_Id;            var dystring = "";            if (item.ismcertreQ_ActiveFlag == true) {                dystring = "Deactivate";            }            else if (item.ismcertreQ_ActiveFlag == false) {                dystring = "Activate";            }            swal({                title: "Are you sure?",                text: "Do You Want To " + dystring + " Record?",                type: "warning",                showCancelButton: true,                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",                cancelButtonText: "Cancel",                closeOnConfirm: false,                closeOnCancel: false            },                function (isConfirm) {                    if (isConfirm) {                        apiService.create("ISM_Certificate_Process/deactive", item).                            then(function (promise) {                                if (promise.returnval == true) {                                    swal("Record " + dystring + "d Successfully!!!");                                }                                else {                                    swal("Record Not " + dystring + "d Successfully!!!");                                }                                $state.reload();                            })                    }                    else {                        swal("Record " + dystring + " Cancelled!!!");                    }                });        }


        $scope.viewaddress = function (address) {
            $scope.ISMCERTREQ_RecivingAddress = address;
            $('#receivingaddress').modal('show');
           
        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';




    }

})();