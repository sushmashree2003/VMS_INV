
(function () {
    'use strict';
    angular
        .module('app')
        .controller('StatutoryTaxComplianceController', StatutoryTaxComplianceController)

    StatutoryTaxComplianceController.$inject = ['$rootScope', '$scope', '$state', '$location', '$sce', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'superCache']
    function StatutoryTaxComplianceController($rootScope, $scope, $state, $location, Flash, appSettings, $sce, apiService, $http, $q, $cookieStore, $stateParams, $filter, superCache) {


        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            var pageid = 1;
            $scope.VMSSC_Status = 'Pending';
            $scope.VMSSC_Statustwo = 'Pending';
            $scope.VMSSC_Statusthree = 'Pending';
            $scope.VMSSC_Statusfour = 'Pending';
            $scope.VMSSC_Statusfive = 'Pending';
            $scope.VMSSC_Statussix = 'Pending';
            $scope.VMSSC_Statusseven = 'Pending';

            apiService.getURI("StatutoryTaxCompliance/getdetails", pageid).
                then(function (promise) {
                    $scope.AllCompany = promise.allCompany;
                    $scope.AllCompanytwo = promise.allCompany;
                    $scope.AllCompanythree = promise.allCompany;
                    $scope.AllCompanyfour = promise.allCompany;
                    $scope.AllCompanyfive = promise.allCompany;
                    $scope.AllCompanysix = promise.allCompany;
                    $scope.AllCompanyseven = promise.allCompany;
                    $scope.year = promise.year;
                    $scope.yeartwo = promise.year;
                    $scope.yearthree = promise.year;
                    $scope.yearfour = promise.year;
                    $scope.yearfive = promise.year;
                    $scope.yearsix = promise.year;
                    $scope.yearseven = promise.year;
                    $scope.Getincome('IncomeTax');

                });

        };
        $scope.GetReference = function (imfY_Id, mI_Id) {

            var data = {
                "MI_Id": mI_Id,
                "IMFY_Id": imfY_Id
            };

            apiService.create("StatutoryTaxCompliance/GetReference", data).
                then(function (promise) {
                    $scope.ReferenceNo = promise.referenceNo;
                    $scope.ReferenceNumber = $scope.ReferenceNo[0].VMSSC_ReferenceNo;
                });

        };
        $scope.ViewUploadFiles = function () {
            //$scope.Flagdata = [];
            $('#Statutorydetails').modal('show');

        }
        $scope.Getincome = function (tax) {
            $scope.Flagdata = [];
            var data = {
                "VMSSC_Flag": tax,

            };

            apiService.create("StatutoryTaxCompliance/Getincome", data).
                then(function (promise) {
                    if (promise.flagdata.length > 0 && promise.flagdata != null) {
                        $scope.Flagdata = promise.flagdata;
                    }


                });

        };
        $scope.viewData = function (option) {
            $scope.attachementlist = [];
            var data = {
                "VMSSC_Id": option.VMSSC_Id,

            };
            apiService.create("StatutoryTaxCompliance/viewData", data).then(function (promise) {

                if (promise.viewData.length > 0) {
                    $scope.viewData1 = [];
                    angular.forEach(promise.viewData, function (qq) {
                        $scope.img = qq.VMSSCD_Filepath;
                        if ($scope.img != null) {
                            var imagarr = $scope.img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];

                            $scope.filetype2 = lastelement;
                        }

                        if ($scope.filetype2 == 'mp4' || $scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp3'
                            || $scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' ||
                            $scope.filetype2 == 'pdf' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx'
                            || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'ppsx'
                            || $scope.filetype2 == 'doc' || $scope.filetype2 == 'docx') {
                            $scope.viewData1.push({
                                VMSSCD_Filename: qq.VMSSCD_Filename,
                                VMSSC_Id: qq.VMSSC_Id,
                                VMSSCD_Filepath: qq.VMSSCD_Filepath
                            })
                        }
                        else {
                            $scope.viewData1.push({
                                VMSSCD_Filename: qq.VMSSCD_Filename,
                                VMSSC_Id: qq.VMSSC_Id,
                                VMSSCD_Filepath: qq.VMSSCD_Filepath
                            })
                        }
                    });

                    $scope.attachementlist = $scope.viewData1

                    $('#myModalCoverview').modal('show');
                    //$scope.docshowary = true;
                    //$scope.docshow = false;
                }
                else {
                    swal("No Data Found.");
                }
            });
        };

        // First Tab
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.submitted = false;;
        $scope.SaveTabOne = function () {
            $scope.submitted = true;
            if ($scope.myForm1.$valid) {

                $scope.FILESLIST = [];
                $scope.documentListOtherDetails11 = [];
                if ($scope.documentListOtherDetails != null) {
                    angular.forEach($scope.documentListOtherDetails, function (qq) {
                        if (qq.vmsscD_Filepath != null) {
                            $scope.documentListOtherDetails11.push({ VMSSCD_Filepath: qq.vmsscD_Filepath, VMSSCD_Filename: qq.vmsscD_Filename });
                        }
                    })
                    $scope.FILESLIST = $scope.documentListOtherDetails11;
                }
                if ($scope.vmssC_ConsultantFlag === false) {
                    $scope.consultantNameone = "NA";
                    $scope.ConsultantAddressone = "NA";
                    $scope.ConsultantRemarksone = "NA";
                } else {

                    $scope.consultantNameone = $scope.consultantName;
                    $scope.ConsultantAddressone = $scope.ConsultantAddress;
                    $scope.ConsultantRemarksone = $scope.ConsultantRemarks;
                }

                if ($scope.vmssC_CaseFlag === false) {
                    $scope.VMSSC_Statusflag = "NA";
                    $scope.vmssC_CaseDtails = "NA";
                } else {

                    $scope.VMSSC_Statusflag = $scope.VMSSC_Status;
                }

                var data = {
                    "VMSSC_Id": $scope.VMSSC_Id,
                    "MI_Id": $scope.MI_ID,
                    "IMFY_Id": $scope.imfY_Id,
                    "VMSSC_Flag": "IncomeTax",
                    "VMSSC_ReferenceNo": $scope.ReferenceNumber,
                    "VMSSC_Remarks": $scope.vmssC_Remarks,
                    //"VMSSC_FromDate": $scope.vmssC_FromDate,
                    //"VMSSC_ToDate": $scope.vmssC_ToDate,
                    "VMSSC_OtherRemarks": $scope.vmssC_OtherRemarks,
                    "VMSSC_CaseFlag": $scope.vmssC_CaseFlag,
                    "VMSSC_CaseDtails": $scope.vmssC_CaseDtails,
                    "FILESLISTdata": $scope.FILESLIST,
                    "VMSSC_Status": $scope.VMSSC_Statusflag,
                    "VMSSCT_consultantName": $scope.consultantNameone,
                    "VMSSCT_Address": $scope.ConsultantAddressone,
                    "VMSSCT_Remarks": $scope.ConsultantRemarksone,
                    "VMSSCT_Flag": $scope.vmssC_ConsultantFlag,

                };
                apiService.create("StatutoryTaxCompliance/SaveTabOne", data).then(function (promise) {
                    if (promise.message == 'saved') {
                        swal("", "Data Saved Successfully!", "success");
                    }
                    else if (promise.message == 'notsaved') {
                        swal("", "Data Not Saved ", "error");
                    }
                    else if (promise.message == 'Updated') {
                        swal("", "Data Updated Successfully!", "success");
                    }

                    else if (promise.message == 'notupdated') {
                        swal("", "Data Not Updated ", "error");
                    }
                    else {

                        swal("Data Not Saved ", "error");
                    }
                    $state.reload();
                })
            }
        }

        $scope.documentListOtherDetails = [{ id: 'sell11' }];
        $scope.addNewDocumentOtherDetail = function () {
            var newItemNo = $scope.documentListOtherDetails.length + 1;
            if (newItemNo <= 100) {
                $scope.documentListOtherDetails.push({ 'id': 'sell11' + newItemNo });
            }
        };
        $scope.removeNewDocumentOtherDetail = function (index, data) {
            var newItemNo = $scope.documentListOtherDetails.length - 1;
            $scope.documentListOtherDetails.splice(index, 1);
            if (data.hrmE_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
        };
        $scope.selectFileforUploadzdOtherDetail = function (input, document) {
            $scope.ldr = true;
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
            // We can send more data to server using append         
            //formData.append("Id", 0);
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
                        data.vmsscD_Filepath = d[0].path;
                        data.vmsscD_Filename = d[0].name;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }
        $scope.previewimg_new = function (img) {
            $scope.imagepreview = img;
            $scope.view_videos = [];
            var img = $scope.imagepreview;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myvideoPreview').modal('show');
            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {
                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');
            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
            }
            else if ($scope.filetype2 == 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');
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
        $scope.Edit1 = function (obj) {
            $scope.temptaskreponsefiles = [];
            $scope.temptaskreponsefiles.push({ 'id': 'sell11' });
            var data = {
                "VMSSC_Id": obj.VMSSC_Id,

            };

            apiService.create("StatutoryTaxCompliance/Edit", data).then(function (promise) {
                if (promise !== null) {
                    $scope.geteditdetails = promise.geteditdetails;
                    $scope.editable = true;
                    $scope.VMSSC_Id = $scope.geteditdetails[0].VMSSC_Id;
                    $scope.MI_ID = $scope.geteditdetails[0].MI_Id;
                    $scope.imfY_Id = $scope.geteditdetails[0].IMFY_Id;
                    $scope.ReferenceNumber = $scope.geteditdetails[0].VMSSC_ReferenceNo;
                    $scope.vmssC_Remarks = $scope.geteditdetails[0].VMSSC_Remarks;
                    $scope.VMSSC_Flag = $scope.geteditdetails[0].VMSSC_Flag;
                    $scope.MI_Name = $scope.geteditdetails[0].MI_Name;
                    $scope.vmssC_CaseFlag = $scope.geteditdetails[0].VMSSC_CaseFlag;
                    $scope.VMSSC_FromDate = $scope.geteditdetails[0].VMSSC_FromDate;
                    $scope.VMSSC_ToDate = $scope.geteditdetails[0].VMSSC_ToDate;
                    $scope.vmssC_CaseDtails = $scope.geteditdetails[0].VMSSC_CaseDtails;
                    $scope.vmssC_OtherRemarks = $scope.geteditdetails[0].VMSSC_OtherRemarks;
                    $scope.VMSSC_Status = $scope.geteditdetails[0].VMSSC_Status;
                    $scope.vmssC_ConsultantFlag = $scope.geteditdetails[0].VMSSCT_Flag;
                    $scope.consultantName = $scope.geteditdetails[0].VMSSCT_consultantName;
                    $scope.ConsultantAddress = $scope.geteditdetails[0].VMSSCT_Address;
                    $scope.ConsultantRemarks = $scope.geteditdetails[0].VMSSCT_Remarks;

                    if (promise.geteditfilesdetails !== null && promise.geteditfilesdetails.length > 0) {
                        $scope.documentListOtherDetails = [];
                        $scope.documentListOtherDetails = promise.geteditfilesdetails;
                        angular.forEach($scope.documentListOtherDetails, function (dd) {
                            if (dd.vmsscD_Filepath !== null && dd.vmsscD_Filepath !== "") {
                                var img = dd.vmsscD_Filepath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("dd.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
                                }
                            }
                        });
                    }
                }
            });
        };
        $scope.clear1 = function () {
            $state.reload();
        }

        //Second tab

        $scope.documentListOtherDetailstwo = [{ id: 'sell11' }];
        $scope.addNewDocumentOtherDetailtwo = function () {
            var newItemNo = $scope.documentListOtherDetailstwo.length + 1;
            if (newItemNo <= 100) {
                $scope.documentListOtherDetailstwo.push({ 'id': 'sell11' + newItemNo });
            }
        };
        $scope.removeNewDocumentOtherDetailtwo = function (index, data) {
            var newItemNo = $scope.documentListOtherDetailstwo.length - 1;
            $scope.documentListOtherDetailstwo.splice(index, 1);
            if (data.hrmE_Id > 0) {
                $scope.DeleteDocumentDataOtherstwo(data);
            }
        };
        $scope.selectFileforUploadzdOtherDetailtwo = function (input, document) {
            $scope.ldr = true;
            $scope.SelectedFileForUploadzdOtherDetailtwo = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetailtwo(document);

            }
        };
        function UploadEmployeeDocumentOtherDetailtwo(data) {
            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetailtwo.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetailtwo[i]);
            }
            // We can send more data to server using append         
            //formData.append("Id", 0);
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
                        data.vmsscD_Filepathtwo = d[0].path;
                        data.vmsscD_Filenametwo = d[0].name;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }
        $scope.previewimg_newtwo = function (img) {
            $scope.imagepreviewtwo = img;
            $scope.view_videos = [];
            var img = $scope.imagepreviewtwo;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myvideoPreview').modal('show');
            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {
                $('#preview').attr('src', $scope.imagepreviewtwo);
                $('#myimagePreview').modal('show');
            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreviewtwo)
            }
            else if ($scope.filetype2 == 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');
            }
            else if ($scope.filetype2 == 'pdf') {
                ///=====================show pdf, img
                $('#showpdf').modal('hide');
                var imagedownload1 = "";
                imagedownload1 = $scope.imagepreviewtwo;
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
                $window.open($scope.imagepreviewtwo)
            }
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };
        $scope.submitted2 = false;;
        $scope.SaveTabtwo = function () {
            $scope.submitted2 = true;
            if ($scope.myForm2.$valid) {

                $scope.FILESLISTtwo = [];
                $scope.documentListOtherDetails11two = [];
                if ($scope.documentListOtherDetailstwo != null) {
                    angular.forEach($scope.documentListOtherDetailstwo, function (qq) {
                        if (qq.vmsscD_Filepathtwo != null) {
                            $scope.documentListOtherDetails11two.push({ VMSSCD_Filepath: qq.vmsscD_Filepathtwo, VMSSCD_Filename: qq.vmsscD_Filenametwo });
                        }
                    })
                    $scope.FILESLISTtwo = $scope.documentListOtherDetails11two;
                }
                if ($scope.vmssC_CaseFlagtwo === false) {
                    $scope.VMSSC_Statustwoflag = "NA";
                    $scope.vmssC_CaseDtailstwo = "NA";
                } else {

                    $scope.VMSSC_Statustwoflag = $scope.VMSSC_Statustwo;
                }
                var data = {
                    "VMSSC_Id": $scope.VMSSC_Id,
                    "MI_Id": $scope.mI_Idtwo,
                    "IMFY_Id": $scope.imfY_Idtwo,
                    "VMSSC_Flag": "GST",
                    "VMSSC_ReferenceNo": $scope.ReferenceNumbertwo,
                    "VMSSC_Remarks": $scope.vmssC_Remarkstwo,
                    //"VMSSC_FromDate": $scope.vmssC_FromDate,
                    //"VMSSC_ToDate": $scope.vmssC_ToDate,
                    "VMSSC_OtherRemarks": $scope.vmssC_OtherRemarkstwo,
                    "VMSSC_CaseFlag": $scope.vmssC_CaseFlagtwo,
                    "VMSSC_CaseDtails": $scope.vmssC_CaseDtailstwo,
                    "FILESLISTdata": $scope.FILESLISTtwo,
                    "VMSSC_Status": $scope.VMSSC_Statustwoflag,
                };
                apiService.create("StatutoryTaxCompliance/SaveTabtwo", data).then(function (promise) {
                    if (promise.message == 'saved') {
                        swal("", "Data Saved Successfully!", "success");
                    }
                    else if (promise.message == 'notsaved') {
                        swal("", "Data Not Saved ", "error");
                    }
                    else if (promise.message == 'Updated') {
                        swal("", "Data Updated Successfully!", "success");
                    }

                    else if (promise.message == 'notupdated') {
                        swal("", "Data Not Updated ", "error");
                    }
                    else {

                        swal("Data Not Saved ", "error");
                    }
                    $state.reload();
                })
            }
        }

        $scope.GetReferencetwo = function (imfY_Id, mI_Id) {

            var data = {
                "MI_Id": mI_Id,
                "IMFY_Id": imfY_Id
            };

            apiService.create("StatutoryTaxCompliance/GetReference", data).
                then(function (promise) {
                    $scope.ReferenceNo = promise.referenceNo;
                    $scope.ReferenceNumbertwo = $scope.ReferenceNo[0].VMSSC_ReferenceNo;
                });


        };
        $scope.Edit2 = function (obj) {
            $scope.temptaskreponsefiles = [];
            $scope.temptaskreponsefiles.push({ 'id': 'sell11' });
            var data = {
                "VMSSC_Id": obj.VMSSC_Id,

            };

            apiService.create("StatutoryTaxCompliance/Edit", data).then(function (promise) {
                if (promise !== null) {
                    $scope.geteditdetails = [];
                    $scope.geteditdetails = promise.geteditdetails;
                    $scope.documentListOtherDetailstwo = promise.geteditfilesdetails;
                    //$scope.editable = true;
                    $scope.VMSSC_Id = $scope.geteditdetails[0].VMSSC_Id;
                    $scope.mI_Idtwo = $scope.geteditdetails[0].MI_Id;
                    $scope.imfY_Idtwo = $scope.geteditdetails[0].IMFY_Id;
                    $scope.ReferenceNumbertwo = $scope.geteditdetails[0].VMSSC_ReferenceNo;
                    $scope.vmssC_Remarkstwo = $scope.geteditdetails[0].VMSSC_Remarks;
                    $scope.VMSSC_Flag = $scope.geteditdetails[0].VMSSC_Flag;
                    $scope.MI_Name = $scope.geteditdetails[0].MI_Name;
                    $scope.vmssC_CaseFlagtwo = $scope.geteditdetails[0].VMSSC_CaseFlag;
                    $scope.VMSSC_FromDate = $scope.geteditdetails[0].VMSSC_FromDate;
                    $scope.VMSSC_ToDate = $scope.geteditdetails[0].VMSSC_ToDate;
                    $scope.vmssC_CaseDtailstwo = $scope.geteditdetails[0].VMSSC_CaseDtails;
                    $scope.vmssC_OtherRemarkstwo = $scope.geteditdetails[0].VMSSC_OtherRemarks;
                    $scope.VMSSC_Statustwo = $scope.geteditdetails[0].VMSSC_Status;
                    if (promise.geteditfilesdetailstwo !== null && promise.geteditfilesdetailstwo.length > 0) {
                        $scope.documentListOtherDetailstwo = [];
                        $scope.documentListOtherDetailstwo = promise.geteditfilesdetailstwo;
                        angular.forEach($scope.documentListOtherDetailstwo, function (dd) {
                            if (dd.vmsscD_Filepathtwo !== null && dd.vmsscD_Filepathtwo !== "") {
                                var img = dd.vmsscD_Filepathtwo;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("dd.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
                                }
                            }
                        });
                    }
                }
            });
        };
        $scope.clear2 = function () {
            $state.reload();
        }
        //Third tab

        $scope.documentListOtherDetailsthree = [{ id: 'sell11' }];
        $scope.addNewDocumentOtherDetailthree = function () {
            var newItemNo = $scope.documentListOtherDetailsthree.length + 1;
            if (newItemNo <= 100) {
                $scope.documentListOtherDetailsthree.push({ 'id': 'sell11' + newItemNo });
            }
        };
        $scope.removeNewDocumentOtherDetailthree = function (index, data) {
            var newItemNo = $scope.documentListOtherDetailsthree.length - 1;
            $scope.documentListOtherDetailsthree.splice(index, 1);
            if (data.hrmE_Id > 0) {
                $scope.DeleteDocumentDataOthersthree(data);
            }
        };
        $scope.selectFileforUploadzdOtherDetailthree = function (input, document) {
            $scope.ldr = true;
            $scope.SelectedFileForUploadzdOtherDetailthree = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetailthree(document);

            }
        };
        function UploadEmployeeDocumentOtherDetailthree(data) {
            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetailthree.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetailthree[i]);
            }
            // We can send more data to server using append         
            //formData.append("Id", 0);
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
                        data.vmsscD_Filepaththree = d[0].path;
                        data.vmsscD_Filenamethree = d[0].name;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }
        $scope.previewimg_newthree = function (img) {
            $scope.imagepreviewthree = img;
            $scope.view_videos = [];
            var img = $scope.imagepreviewthree;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myvideoPreview').modal('show');
            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {
                $('#preview').attr('src', $scope.imagepreviewthree);
                $('#myimagePreview').modal('show');
            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreviewthree)
            }
            else if ($scope.filetype2 == 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');
            }
            else if ($scope.filetype2 == 'pdf') {
                ///=====================show pdf, img
                $('#showpdf').modal('hide');
                var imagedownload1 = "";
                imagedownload1 = $scope.imagepreviewthree;
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
                $window.open($scope.imagepreviewthree)
            }
        };

        $scope.interacted3 = function (field) {
            return $scope.submitted3;
        };
        $scope.submitted3 = false;;
        $scope.SaveTabthree = function () {
            $scope.submitted3 = true;
            if ($scope.myForm3.$valid) {

                $scope.FILESLISTthree = [];
                $scope.documentListOtherDetails11three = [];
                if ($scope.documentListOtherDetailsthree != null) {
                    angular.forEach($scope.documentListOtherDetailsthree, function (qq) {
                        if (qq.vmsscD_Filepaththree != null) {
                            $scope.documentListOtherDetails11three.push({ VMSSCD_Filepath: qq.vmsscD_Filepaththree, VMSSCD_Filename: qq.vmsscD_Filenamethree });
                        }
                    })
                    $scope.FILESLISTthree = $scope.documentListOtherDetails11three;
                }
                if ($scope.vmssC_CaseFlagthree === false) {
                    $scope.VMSSC_Statusthreeflag = "NA";
                    $scope.vmssC_CaseDtailsthree = "NA";
                } else {

                    $scope.VMSSC_Statusthreeflag = $scope.VMSSC_Statusthree;
                }
                var data = {
                    "VMSSC_Id": $scope.VMSSC_Id,
                    "MI_Id": $scope.mI_Idthree,
                    "IMFY_Id": $scope.imfY_Idthree,
                    "VMSSC_Flag": "ESICPF",
                    "VMSSC_ReferenceNo": $scope.ReferenceNumberthree,
                    "VMSSC_Remarks": $scope.vmssC_Remarksthree,
                    //"VMSSC_FromDate": $scope.vmssC_FromDate,
                    //"VMSSC_ToDate": $scope.vmssC_ToDate,
                    "VMSSC_OtherRemarks": $scope.vmssC_OtherRemarksthree,
                    "VMSSC_CaseFlag": $scope.vmssC_CaseFlagthree,
                    "VMSSC_CaseDtails": $scope.vmssC_CaseDtailsthree,
                    "FILESLISTdata": $scope.FILESLISTthree,
                    "VMSSC_Status": $scope.VMSSC_Statusthreeflag,
                };
                apiService.create("StatutoryTaxCompliance/SaveTabThree", data).then(function (promise) {
                    if (promise.message == 'saved') {
                        swal("", "Data Saved Successfully!", "success");
                    }
                    else if (promise.message == 'notsaved') {
                        swal("", "Data Not Saved ", "error");
                    }
                    else if (promise.message == 'Updated') {
                        swal("", "Data Updated Successfully!", "success");
                    }

                    else if (promise.message == 'notupdated') {
                        swal("", "Data Not Updated ", "error");
                    }
                    else {

                        swal("Data Not Saved ", "error");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted3 = true;
            }
        }

        $scope.GetReferencethree = function (imfY_Id, mI_Id) {

            var data = {
                "MI_Id": mI_Id,
                "IMFY_Id": imfY_Id
            };

            apiService.create("StatutoryTaxCompliance/GetReference", data).
                then(function (promise) {
                    $scope.ReferenceNo = promise.referenceNo;
                    $scope.ReferenceNumberthree = $scope.ReferenceNo[0].VMSSC_ReferenceNo;
                });


        };

        $scope.Edit3 = function (obj) {
            $scope.temptaskreponsefiles = [];
            $scope.temptaskreponsefiles.push({ 'id': 'sell11' });
            var data = {
                "VMSSC_Id": obj.VMSSC_Id,

            };

            apiService.create("StatutoryTaxCompliance/Edit", data).then(function (promise) {
                if (promise !== null) {
                    $scope.geteditdetails = promise.geteditdetails;
                    $scope.editable = true;
                    $scope.VMSSC_Id = $scope.geteditdetails[0].VMSSC_Id;
                    $scope.mI_Idthree = $scope.geteditdetails[0].MI_Id;
                    $scope.imfY_Idthree = $scope.geteditdetails[0].IMFY_Id;
                    $scope.ReferenceNumberthree = $scope.geteditdetails[0].VMSSC_ReferenceNo;
                    $scope.vmssC_Remarksthree = $scope.geteditdetails[0].VMSSC_Remarks;
                    $scope.VMSSC_Flag = $scope.geteditdetails[0].VMSSC_Flag;
                    $scope.MI_Name = $scope.geteditdetails[0].MI_Name;
                    $scope.vmssC_CaseFlagthree = $scope.geteditdetails[0].VMSSC_CaseFlag;
                    $scope.VMSSC_FromDate = $scope.geteditdetails[0].VMSSC_FromDate;
                    $scope.VMSSC_ToDate = $scope.geteditdetails[0].VMSSC_ToDate;
                    $scope.vmssC_CaseDtailsthree = $scope.geteditdetails[0].VMSSC_CaseDtails;
                    $scope.VMSSC_Statusthree = $scope.geteditdetails[0].VMSSC_Status;

                    if (promise.geteditfilesdetailsthree !== null && promise.geteditfilesdetailsthree.length > 0) {
                        $scope.documentListOtherDetailsthree = promise.geteditfilesdetailsthree;
                        angular.forEach($scope.documentListOtherDetailsthree, function (dd) {
                            if (dd.vmsscD_Filepath !== null && dd.vmsscD_Filepath !== "") {
                                var img = dd.vmsscD_Filepath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("dd.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
                                }
                            }
                        });
                    }
                }
            });
        };
        $scope.clear3 = function () {
            $state.reload();
        }

        // Fourth Tab
        $scope.documentListOtherDetailsfour = [{ id: 'sell11' }];
        $scope.addNewDocumentOtherDetailfour = function () {
            var newItemNo = $scope.documentListOtherDetailsfour.length + 1;
            if (newItemNo <= 100) {
                $scope.documentListOtherDetailsfour.push({ 'id': 'sell11' + newItemNo });
            }
        };
        $scope.removeNewDocumentOtherDetailfour = function (index, data) {
            var newItemNo = $scope.documentListOtherDetailsfour.length - 1;
            $scope.documentListOtherDetailsfour.splice(index, 1);
            if (data.hrmE_Id > 0) {
                $scope.DeleteDocumentDataOthersfour(data);
            }
        };
        $scope.selectFileforUploadzdOtherDetailfour = function (input, document) {
            $scope.ldr = true;
            $scope.SelectedFileForUploadzdOtherDetailfour = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetailfour(document);

            }
        };
        function UploadEmployeeDocumentOtherDetailfour(data) {
            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetailfour.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetailfour[i]);
            }
            // We can send more data to server using append         
            //formData.append("Id", 0);
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
                        data.vmsscD_Filepathfour = d[0].path;
                        data.vmsscD_Filenamefour = d[0].name;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }
        $scope.previewimg_newfour = function (img) {
            $scope.imagepreviewfour = img;
            $scope.view_videos = [];
            var img = $scope.imagepreviewfour;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myvideoPreview').modal('show');
            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {
                $('#preview').attr('src', $scope.imagepreviewfour);
                $('#myimagePreview').modal('show');
            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreviewfour)
            }
            else if ($scope.filetype2 == 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');
            }
            else if ($scope.filetype2 == 'pdf') {
                ///=====================show pdf, img
                $('#showpdf').modal('hide');
                var imagedownload1 = "";
                imagedownload1 = $scope.imagepreviewfour;
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
                $window.open($scope.imagepreviewfour)
            }
        };

        $scope.interacted4 = function (field) {
            return $scope.submitted4;
        };
        $scope.submitted4 = false;;
        $scope.SaveTabfour = function () {
            $scope.submitted4 = true;
            if ($scope.myForm4.$valid) {

                $scope.FILESLISTfour = [];
                $scope.documentListOtherDetails11four = [];
                if ($scope.documentListOtherDetailsfour != null) {
                    angular.forEach($scope.documentListOtherDetailsfour, function (qq) {
                        if (qq.vmsscD_Filepathfour != null) {
                            $scope.documentListOtherDetails11four.push({ VMSSCD_Filepath: qq.vmsscD_Filepathfour, VMSSCD_Filename: qq.vmsscD_Filenamefour });
                        }
                    })
                    $scope.FILESLISTfour = $scope.documentListOtherDetails11four;
                }
                if ($scope.vmssC_CaseFlagfour === false) {
                    $scope.VMSSC_Statusfourflag = "NA";
                    $scope.vmssC_CaseDtailsfour = "NA";
                } else {

                    $scope.VMSSC_Statusfourflag = $scope.VMSSC_Statusfour;
                }
                var data = {
                    "VMSSC_Id": $scope.VMSSC_Id,
                    "MI_Id": $scope.mI_Idfour,
                    "IMFY_Id": $scope.imfY_Idfour,
                    "VMSSC_Flag": "PT",
                    "VMSSC_ReferenceNo": $scope.ReferenceNumberfour,
                    "VMSSC_Remarks": $scope.vmssC_Remarksfour,
                    "VMSSC_FromDate": $scope.vmssC_FromDatefour,
                    "VMSSC_ToDate": $scope.vmssC_ToDatefour,
                    "VMSSC_OtherRemarks": $scope.vmssC_OtherRemarksfour,
                    "VMSSC_CaseFlag": $scope.vmssC_CaseFlagfour,
                    "VMSSC_CaseDtails": $scope.vmssC_CaseDtailsfour,
                    "FILESLISTdata": $scope.FILESLISTfour,
                    "VMSSC_Status": $scope.VMSSC_Statusfourflag,
                };
                apiService.create("StatutoryTaxCompliance/SaveTabFour", data).then(function (promise) {
                    if (promise.message == 'saved') {
                        swal("", "Data Saved Successfully!", "success");
                    }
                    else if (promise.message == 'notsaved') {
                        swal("", "Data Not Saved ", "error");
                    }
                    else if (promise.message == 'Updated') {
                        swal("", "Data Updated Successfully!", "success");
                    }

                    else if (promise.message == 'notupdated') {
                        swal("", "Data Not Updated ", "error");
                    }
                    else {

                        swal("Data Not Saved ", "error");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted4 = true;
            }
        }

        $scope.GetReferencefour = function (imfY_Id, mI_Id) {

            var data = {
                "MI_Id": mI_Id,
                "IMFY_Id": imfY_Id
            };

            apiService.create("StatutoryTaxCompliance/GetReference", data).
                then(function (promise) {
                    $scope.ReferenceNo = promise.referenceNo;
                    $scope.ReferenceNumberfour = $scope.ReferenceNo[0].VMSSC_ReferenceNo;
                });


        };
        $scope.Edit4 = function (obj) {
            $scope.temptaskreponsefiles = [];
            $scope.temptaskreponsefiles.push({ 'id': 'sell11' });
            var data = {
                "VMSSC_Id": obj.VMSSC_Id,

            };

            apiService.create("StatutoryTaxCompliance/Edit", data).then(function (promise) {
                if (promise !== null) {
                    $scope.geteditdetails = promise.geteditdetails;
                    $scope.editable = true;
                    $scope.VMSSC_Id = $scope.geteditdetails[0].VMSSC_Id;
                    $scope.mI_Idfour = $scope.geteditdetails[0].MI_Id;
                    $scope.imfY_Idfour = $scope.geteditdetails[0].IMFY_Id;
                    $scope.ReferenceNumberfour = $scope.geteditdetails[0].VMSSC_ReferenceNo;
                    $scope.vmssC_Remarksfour = $scope.geteditdetails[0].VMSSC_Remarks;
                    $scope.VMSSC_Flagfour = $scope.geteditdetails[0].VMSSC_Flag;
                    $scope.MI_Namefour = $scope.geteditdetails[0].MI_Name;
                    $scope.vmssC_CaseFlagfour = $scope.geteditdetails[0].VMSSC_CaseFlag;
                    $scope.vmssC_FromDatefour = new Date($scope.geteditdetails[0].VMSSC_FromDate);
                    $scope.vmssC_ToDatefour = new Date($scope.geteditdetails[0].VMSSC_ToDate);
                    $scope.vmssC_CaseDtailsfour = $scope.geteditdetails[0].VMSSC_CaseDtails;
                    $scope.VMSSC_Statusfour = $scope.geteditdetails[0].VMSSC_Status;

                    if (promise.geteditfilesdetailsfour !== null && promise.geteditfilesdetailsfour.length > 0) {
                        $scope.documentListOtherDetailsfour = promise.geteditfilesdetailsfour;
                        angular.forEach($scope.documentListOtherDetailsfour, function (dd) {
                            if (dd.vmsscD_Filepath !== null && dd.vmsscD_Filepath !== "") {
                                var img = dd.vmsscD_Filepath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("dd.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
                                }
                            }
                        });
                    }
                }
            });
        };
        $scope.clear4 = function () {
            $state.reload();
        }
        //Save fifth tab

        $scope.documentListOtherDetailsfive = [{ id: 'sell11' }];
        $scope.addNewDocumentOtherDetailfive = function () {
            var newItemNo = $scope.documentListOtherDetailsfive.length + 1;
            if (newItemNo <= 100) {
                $scope.documentListOtherDetailsfive.push({ 'id': 'sell11' + newItemNo });
            }
        };
        $scope.removeNewDocumentOtherDetailfive = function (index, data) {
            var newItemNo = $scope.documentListOtherDetailsfive.length - 1;
            $scope.documentListOtherDetailsfive.splice(index, 1);
            if (data.hrmE_Id > 0) {
                $scope.DeleteDocumentDataOthersfive(data);
            }
        };
        $scope.selectFileforUploadzdOtherDetailfive = function (input, document) {
            $scope.ldr = true;
            $scope.SelectedFileForUploadzdOtherDetailfive = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetailfive(document);

            }
        };
        function UploadEmployeeDocumentOtherDetailfive(data) {
            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetailfive.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetailfive[i]);
            }
            // We can send more data to server using append         
            //formData.append("Id", 0);
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
                        data.vmsscD_Filepathfive = d[0].path;
                        data.vmsscD_Filenamefive = d[0].name;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }
        $scope.previewimg_newfive = function (img) {
            $scope.imagepreviewfive = img;
            $scope.view_videos = [];
            var img = $scope.imagepreviewfive;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myvideoPreview').modal('show');
            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {
                $('#preview').attr('src', $scope.imagepreviewfive);
                $('#myimagePreview').modal('show');
            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreviewfive)
            }
            else if ($scope.filetype2 == 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');
            }
            else if ($scope.filetype2 == 'pdf') {
                ///=====================show pdf, img
                $('#showpdf').modal('hide');
                var imagedownload1 = "";
                imagedownload1 = $scope.imagepreviewfive;
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
                $window.open($scope.imagepreviewfive)
            }
        };

        $scope.interacted5 = function (field) {
            return $scope.submitted5;
        };
        $scope.submitted5 = false;;
        $scope.SaveTabfive = function () {
            $scope.submitted5 = true;
            if ($scope.myForm5.$valid) {

                $scope.FILESLISTfive = [];
                $scope.documentListOtherDetails11five = [];
                if ($scope.documentListOtherDetailsfive != null) {
                    angular.forEach($scope.documentListOtherDetailsfive, function (qq) {
                        if (qq.vmsscD_Filepathfive != null) {
                            $scope.documentListOtherDetails11five.push({ VMSSCD_Filepath: qq.vmsscD_Filepathfive, VMSSCD_Filename: qq.vmsscD_Filenamefive });
                        }
                    })
                    $scope.FILESLISTfive = $scope.documentListOtherDetails11five;
                }

                if ($scope.vmssC_CaseFlagfive === false) {
                    $scope.VMSSC_Statusfiveflag = "NA";
                    $scope.vmssC_CaseDtailsfive = "NA";
                } else {

                    $scope.VMSSC_Statusfiveflag = $scope.VMSSC_Statusfive;
                }
                var data = {
                    "VMSSC_Id": $scope.VMSSC_Id,
                    "MI_Id": $scope.mI_Idfive,
                    "IMFY_Id": $scope.imfY_Idfive,
                    "VMSSC_Flag": "SEAC",
                    "VMSSC_ReferenceNo": $scope.ReferenceNumberfive,
                    "VMSSC_Remarks": $scope.vmssC_Remarksfive,
                    "VMSSC_FromDate": $scope.vmssC_FromDatefive,
                    "VMSSC_ToDate": $scope.vmssC_ToDatefive,
                    "VMSSC_OtherRemarks": $scope.vmssC_OtherRemarksfive,
                    "VMSSC_CaseFlag": $scope.vmssC_CaseFlagfive,
                    "VMSSC_CaseDtails": $scope.vmssC_CaseDtailsfive,
                    "FILESLISTdata": $scope.FILESLISTfive,
                    "VMSSC_Status": $scope.VMSSC_Statusfiveflag,
                };
                apiService.create("StatutoryTaxCompliance/SaveTabFive", data).then(function (promise) {
                    if (promise.message == 'saved') {
                        swal("", "Data Saved Successfully!", "success");
                    }
                    else if (promise.message == 'notsaved') {
                        swal("", "Data Not Saved ", "error");
                    }
                    else if (promise.message == 'Updated') {
                        swal("", "Data Updated Successfully!", "success");
                    }

                    else if (promise.message == 'notupdated') {
                        swal("", "Data Not Updated ", "error");
                    }
                    else {

                        swal("Data Not Saved ", "error");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted5 = true;
            }
        }

        $scope.GetReferencefive = function (imfY_Id, mI_Id) {

            var data = {
                "MI_Id": mI_Id,
                "IMFY_Id": imfY_Id
            };

            apiService.create("StatutoryTaxCompliance/GetReference", data).
                then(function (promise) {
                    $scope.ReferenceNo = promise.referenceNo;
                    $scope.ReferenceNumberfive = $scope.ReferenceNo[0].VMSSC_ReferenceNo;
                });


        };
        $scope.Edit5 = function (obj) {
            $scope.temptaskreponsefiles = [];
            $scope.temptaskreponsefiles.push({ 'id': 'sell11' });
            var data = {
                "VMSSC_Id": obj.VMSSC_Id,

            };

            apiService.create("StatutoryTaxCompliance/Edit", data).then(function (promise) {
                if (promise !== null) {
                    $scope.geteditdetails = promise.geteditdetails;
                    $scope.editable = true;
                    $scope.VMSSC_Id = $scope.geteditdetails[0].VMSSC_Id;
                    $scope.mI_Idfive = $scope.geteditdetails[0].MI_Id;
                    $scope.imfY_Idfive = $scope.geteditdetails[0].IMFY_Id;
                    $scope.ReferenceNumberfive = $scope.geteditdetails[0].VMSSC_ReferenceNo;
                    $scope.vmssC_Remarksfive = $scope.geteditdetails[0].VMSSC_Remarks;
                    $scope.VMSSC_Flag = $scope.geteditdetails[0].VMSSC_Flag;
                    $scope.MI_Name = $scope.geteditdetails[0].MI_Name;
                    $scope.vmssC_CaseFlagfive = $scope.geteditdetails[0].VMSSC_CaseFlag;
                    $scope.vmssC_FromDatefive = new Date($scope.geteditdetails[0].VMSSC_FromDate);
                    $scope.vmssC_ToDatefive = new Date($scope.geteditdetails[0].VMSSC_ToDate);
                    $scope.vmssC_CaseDtailsfive = $scope.geteditdetails[0].VMSSC_CaseDtails;

                    if (promise.geteditfilesdetailsfive !== null && promise.geteditfilesdetailsfive.length > 0) {
                        $scope.documentListOtherDetailsfive = promise.geteditfilesdetailsfive;
                        angular.forEach($scope.documentListOtherDetailsfive, function (dd) {
                            if (dd.vmsscD_Filepath !== null && dd.vmsscD_Filepath !== "") {
                                var img = dd.vmsscD_Filepath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("dd.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
                                }
                            }
                        });
                    }
                }
            });
        };
        $scope.clear5 = function () {
            $state.reload();
        }
        //Save sixth tab

        $scope.documentListOtherDetailssix = [{ id: 'sell11' }];
        $scope.addNewDocumentOtherDetailsix = function () {
            var newItemNo = $scope.documentListOtherDetailssix.length + 1;
            if (newItemNo <= 100) {
                $scope.documentListOtherDetailssix.push({ 'id': 'sell11' + newItemNo });
            }
        };
        $scope.removeNewDocumentOtherDetailsix = function (index, data) {
            var newItemNo = $scope.documentListOtherDetailssix.length - 1;
            $scope.documentListOtherDetailssix.splice(index, 1);
            if (data.hrmE_Id > 0) {
                $scope.DeleteDocumentDataOtherssix(data);
            }
        };
        $scope.selectFileforUploadzdOtherDetailsix = function (input, document) {
            $scope.ldr = true;
            $scope.SelectedFileForUploadzdOtherDetailsix = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetailsix(document);

            }
        };
        function UploadEmployeeDocumentOtherDetailsix(data) {
            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetailsix.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetailsix[i]);
            }
            // We can send more data to server using append         
            //formData.append("Id", 0);
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
                        data.vmsscD_Filepathsix = d[0].path;
                        data.vmsscD_Filenamesix = d[0].name;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }
        $scope.previewimg_newsix = function (img) {
            $scope.imagepreviewsix = img;
            $scope.view_videos = [];
            var img = $scope.imagepreviewsix;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myvideoPreview').modal('show');
            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {
                $('#preview').attr('src', $scope.imagepreviewsix);
                $('#myimagePreview').modal('show');
            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreviewsix)
            }
            else if ($scope.filetype2 == 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');
            }
            else if ($scope.filetype2 == 'pdf') {
                ///=====================show pdf, img
                $('#showpdf').modal('hide');
                var imagedownload1 = "";
                imagedownload1 = $scope.imagepreviewsix;
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
                $window.open($scope.imagepreviewsix)
            }
        };

        $scope.interacted6 = function (field) {
            return $scope.submitted6;
        };
        $scope.submitted6 = false;;
        $scope.SaveTabsix = function () {
            $scope.submitted6 = true;
            if ($scope.myForm6.$valid) {

                $scope.FILESLISTsix = [];
                $scope.documentListOtherDetails11six = [];
                if ($scope.documentListOtherDetailssix != null) {
                    angular.forEach($scope.documentListOtherDetailssix, function (qq) {
                        if (qq.vmsscD_Filepathsix != null) {
                            $scope.documentListOtherDetails11six.push({ VMSSCD_Filepath: qq.vmsscD_Filepathsix, VMSSCD_Filename: qq.vmsscD_Filenamesix });
                        }
                    })
                    $scope.FILESLISTsix = $scope.documentListOtherDetails11six;
                }

                if ($scope.vmssC_CaseFlagsix === false) {
                    $scope.VMSSC_Statussixflag = "NA";
                    $scope.vmssC_CaseDtailssix = "NA";
                } else {

                    $scope.VMSSC_Statussixflag = $scope.VMSSC_Statussix;
                }

                var data = {
                    "VMSSC_Id": $scope.VMSSC_Id,
                    "MI_Id": $scope.mI_Idsix,
                    "IMFY_Id": $scope.imfY_Idsix,
                    "VMSSC_Flag": "OrderNotice",
                    "VMSSC_ReferenceNo": $scope.ReferenceNumbersix,
                    "VMSSC_Remarks": $scope.vmssC_Remarkssix,
                    "VMSSC_FromDate": $scope.vmssC_FromDatesix,
                    "VMSSC_ToDate": $scope.vmssC_ToDatesix,
                    "VMSSC_OtherRemarks": $scope.vmssC_OtherRemarkssix,
                    "VMSSC_CaseFlag": $scope.vmssC_CaseFlagsix,
                    "VMSSC_CaseDtails": $scope.vmssC_CaseDtailssix,
                    "FILESLISTdata": $scope.FILESLISTsix,
                    "VMSSC_Status": $scope.VMSSC_Statussixflag,
                };
                apiService.create("StatutoryTaxCompliance/SaveTabSix", data).then(function (promise) {
                    if (promise.message == 'saved') {
                        swal("", "Data Saved Successfully!", "success");
                    }
                    else if (promise.message == 'notsaved') {
                        swal("", "Data Not Saved ", "error");
                    }
                    else if (promise.message == 'Updated') {
                        swal("", "Data Updated Successfully!", "success");
                    }

                    else if (promise.message == 'notupdated') {
                        swal("", "Data Not Updated ", "error");
                    }
                    else {

                        swal("Data Not Saved ", "error");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted6 = true;
            }
        }

        $scope.GetReferencesix = function (imfY_Id, mI_Id) {

            var data = {
                "MI_Id": mI_Id,
                "IMFY_Id": imfY_Id
            };

            apiService.create("StatutoryTaxCompliance/GetReference", data).
                then(function (promise) {
                    $scope.ReferenceNo = promise.referenceNo;
                    $scope.ReferenceNumbersix = $scope.ReferenceNo[0].VMSSC_ReferenceNo;
                });


        };
        $scope.Edit6 = function (obj) {
            $scope.temptaskreponsefiles = [];
            $scope.temptaskreponsefiles.push({ 'id': 'sell11' });
            var data = {
                "VMSSC_Id": obj.VMSSC_Id,

            };

            apiService.create("StatutoryTaxCompliance/Edit", data).then(function (promise) {
                if (promise !== null) {
                    $scope.geteditdetails = promise.geteditdetails;
                    $scope.editable = true;
                    $scope.VMSSC_Id = $scope.geteditdetails[0].VMSSC_Id;
                    $scope.mI_Idsix = $scope.geteditdetails[0].MI_Id;
                    $scope.imfY_Idsix = $scope.geteditdetails[0].IMFY_Id;
                    $scope.ReferenceNumbersix = $scope.geteditdetails[0].VMSSC_ReferenceNo;
                    $scope.vmssC_Remarkssix = $scope.geteditdetails[0].VMSSC_Remarks;
                    $scope.VMSSC_Flag = $scope.geteditdetails[0].VMSSC_Flag;
                    $scope.MI_Name = $scope.geteditdetails[0].MI_Name;
                    $scope.vmssC_CaseFlagsix = $scope.geteditdetails[0].VMSSC_CaseFlag;
                    $scope.vmssC_FromDatesix = new Date($scope.geteditdetails[0].VMSSC_FromDate);
                    $scope.vmssC_ToDatesix = new Date($scope.geteditdetails[0].VMSSC_ToDate);
                    $scope.vmssC_CaseDtailssix = $scope.geteditdetails[0].VMSSC_CaseDtails;

                    if (promise.geteditfilesdetailssix !== null && promise.geteditfilesdetailssix.length > 0) {
                        $scope.documentListOtherDetailssix = promise.geteditfilesdetailssix;
                        angular.forEach($scope.documentListOtherDetailssix, function (dd) {
                            if (dd.vmsscD_Filepath !== null && dd.vmsscD_Filepath !== "") {
                                var img = dd.vmsscD_Filepath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement; S
                                console.log("dd.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
                                }
                            }
                        });
                    }
                }
            });
        };
        $scope.clear7 = function () {
            $state.reload();
        }
        //Save seventh tab

        $scope.documentListOtherDetailsseven = [{ id: 'sell11' }];
        $scope.addNewDocumentOtherDetailseven = function () {
            var newItemNo = $scope.documentListOtherDetailsseven.length + 1;
            if (newItemNo <= 100) {
                $scope.documentListOtherDetailsseven.push({ 'id': 'sell11' + newItemNo });
            }
        };
        $scope.removeNewDocumentOtherDetailseven = function (index, data) {
            var newItemNo = $scope.documentListOtherDetailsseven.length - 1;
            $scope.documentListOtherDetailsseven.splice(index, 1);
            if (data.hrmE_Id > 0) {
                $scope.DeleteDocumentDataOthersseven(data);
            }
        };
        $scope.selectFileforUploadzdOtherDetailseven = function (input, document) {
            $scope.ldr = true;
            $scope.SelectedFileForUploadzdOtherDetailseven = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetailseven(document);

            }
        };
        function UploadEmployeeDocumentOtherDetailseven(data) {
            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetailseven.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetailseven[i]);
            }
            // We can send more data to server using append         
            //formData.append("Id", 0);
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
                        data.vmsscD_Filepathseven = d[0].path;
                        data.vmsscD_Filenameseven = d[0].name;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }
        $scope.previewimg_newseven = function (img) {
            $scope.imagepreviewseven = img;
            $scope.view_videos = [];
            var img = $scope.imagepreviewseven;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myvideoPreview').modal('show');
            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {
                $('#preview').attr('src', $scope.imagepreviewseven);
                $('#myimagePreview').modal('show');
            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreviewseven)
            }
            else if ($scope.filetype2 == 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');
            }
            else if ($scope.filetype2 == 'pdf') {
                ///=====================show pdf, img
                $('#showpdf').modal('hide');
                var imagedownload1 = "";
                imagedownload1 = $scope.imagepreviewseven;
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
                $window.open($scope.imagepreviewseven)
            }
        };

        $scope.interacted7 = function (field) {
            return $scope.submitted7;
        };
        $scope.submitted7 = false;
        $scope.SaveTabseven = function () {
            $scope.submitted7 = true;
            if ($scope.myForm7.$valid) {

                $scope.FILESLISTseven = [];
                $scope.documentListOtherDetails11seven = [];
                if ($scope.documentListOtherDetailsseven != null) {
                    angular.forEach($scope.documentListOtherDetailsseven, function (qq) {
                        if (qq.vmsscD_Filepathseven != null) {
                            $scope.documentListOtherDetails11seven.push({ VMSSCD_Filepath: qq.vmsscD_Filepathseven, VMSSCD_Filename: qq.vmsscD_Filenameseven });
                        }
                    })
                    $scope.FILESLISTseven = $scope.documentListOtherDetails11seven;
                }
                if ($scope.vmssC_CaseFlagseven === false) {
                    $scope.VMSSC_Statussevenflag = "NA";
                    $scope.vmssC_CaseDtailsseven = "NA";
                } else {
                    // This will be executed if $scope.vmssC_CaseFlagseven is true
                    $scope.VMSSC_Statussevenflag = $scope.VMSSC_Statusseven;
                }


                var data = {
                    "VMSSC_Id": $scope.VMSSC_Id,
                    "MI_Id": $scope.mI_Idseven,
                    "IMFY_Id": $scope.imfY_Idseven,
                    "VMSSC_Flag": "TLC",
                    "VMSSC_ReferenceNo": $scope.ReferenceNumberseven,
                    "VMSSC_Remarks": $scope.vmssC_Remarksseven,
                    "VMSSC_FromDate": $scope.vmssC_FromDateseven,
                    "VMSSC_ToDate": $scope.vmssC_ToDateseven,
                    "VMSSC_OtherRemarks": $scope.vmssC_OtherRemarksseven,
                    "VMSSC_CaseFlag": $scope.vmssC_CaseFlagseven,
                    "VMSSC_CaseDtails": $scope.vmssC_CaseDtailsseven,
                    "FILESLISTdata": $scope.FILESLISTseven,
                    "VMSSC_Status": $scope.VMSSC_Statussevenflag,
                };
                apiService.create("StatutoryTaxCompliance/SaveTabSeven", data).then(function (promise) {
                    if (promise.message == 'saved') {
                        swal("", "Data Saved Successfully!", "success");
                    }
                    else if (promise.message == 'notsaved') {
                        swal("", "Data Not Saved ", "error");
                    }
                    else if (promise.message == 'Updated') {
                        swal("", "Data Updated Successfully!", "success");
                    }

                    else if (promise.message == 'notupdated') {
                        swal("", "Data Not Updated ", "error");
                    }
                    else {

                        swal("Data Not Saved ", "error");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted7 = true;
            }
        }

        $scope.GetReferenceseven = function (imfY_Id, mI_Id) {

            var data = {
                "MI_Id": mI_Id,
                "IMFY_Id": imfY_Id
            };

            apiService.create("StatutoryTaxCompliance/GetReference", data).
                then(function (promise) {
                    $scope.ReferenceNo = promise.referenceNo;
                    $scope.ReferenceNumberseven = $scope.ReferenceNo[0].VMSSC_ReferenceNo;
                });


        };
        $scope.ViewUploadFiles = function (obj) {

            var data = {
                "VMSSC_Id": obj.VMSSC_Id,

            };

            apiService.create("StatutoryTaxCompliance/ViewFiles", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getviewfilesdetails !== null && promise.getviewfilesdetails.length > 0) {
                        $scope.getviewfilesdetails = promise.getviewfilesdetails;
                        angular.forEach($scope.getviewfilesdetails, function (dd) {
                            if (dd.vmsscD_Filepath !== null && dd.vmsscD_Filepath !== "") {
                                var img = dd.vmsscD_Filepath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("dd.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.VMSSCD_Filepath;
                                }
                            }
                        });
                        $('#modalviewfiles').modal('show');
                    } else {
                        swal("No Records Found");
                        $('#modalviewfiles').modal('hide');
                    }
                }
            });
        };
        $scope.showmothersign = function (path) {
            $('#preview1').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalPreview').modal('show');
        };

        $scope.onviewpdf = function (filepath, filename) {
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            document.getElementById("pdfviewdd").innerHTML = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var htmlElements = '<embed id="pdfview" src="' + $scope.content + '"  style="width: 100%;" height="1000" type="application/pdf" />';
                    document.getElementById("pdfviewdd").innerHTML = htmlElements;
                    $('#showpdf').modal('show');
                });
        };

        $scope.ViewDocExcelfles = function (docpath, filename) {
            docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + docpath;
            $scope.detailFrame123 = $sce.trustAsResourceUrl(docpath);
            $('#myModaldocx').modal('show');
        };

        $scope.downloaddirectimage = function (data, idd, type) {
            var studentreg = idd;
            $scope.imagedownload = data;
            var imagedownload = data;
            $http.get(imagedownload, { responseType: "arraybuffer" }).success(function (data) {
                var anchor = angular.element('<a/>');
                var blob = new Blob([data]);
                anchor.attr({
                    href: window.URL.createObjectURL(blob),
                    target: '_blank',
                    download: studentreg
                })[0].click();
            });
        };


        $scope.Edit7 = function (obj) {
            $scope.temptaskreponsefiles = [];
            $scope.temptaskreponsefiles.push({ 'id': 'sell11' });
            var data = {
                "VMSSC_Id": obj.VMSSC_Id,

            };

            apiService.create("StatutoryTaxCompliance/Edit", data).then(function (promise) {
                if (promise !== null) {
                    $scope.geteditdetails = promise.geteditdetails;
                    $scope.editable = true;
                    $scope.VMSSC_Id = $scope.geteditdetails[0].VMSSC_Id;
                    $scope.mI_Idseven = $scope.geteditdetails[0].MI_Id;
                    $scope.imfY_Idseven = $scope.geteditdetails[0].IMFY_Id;
                    $scope.ReferenceNumberseven = $scope.geteditdetails[0].VMSSC_ReferenceNo;
                    $scope.vmssC_OtherRemarksseven = $scope.geteditdetails[0].VMSSC_OtherRemarks;
                    $scope.vmssC_Remarksseven = $scope.geteditdetails[0].VMSSC_Remarks;
                    $scope.VMSSC_Statusseven = $scope.geteditdetails[0].VMSSC_Status;
                    $scope.VMSSC_Flagseven = $scope.geteditdetails[0].VMSSC_Flag;

                    $scope.MI_Name = $scope.geteditdetails[0].MI_Name;
                    $scope.vmssC_CaseFlagseven = $scope.geteditdetails[0].VMSSC_CaseFlag;
                    $scope.vmssC_FromDateseven = new Date($scope.geteditdetails[0].VMSSC_FromDate);
                    $scope.vmssC_ToDateseven = new Date($scope.geteditdetails[0].VMSSC_ToDate);
                    $scope.vmssC_CaseDtailsseven = $scope.geteditdetails[0].VMSSC_CaseDtails;

                    if (promise.geteditfilesdetailsseven !== null && promise.geteditfilesdetailsseven.length > 0) {
                        $scope.documentListOtherDetailsseven = promise.geteditfilesdetailsseven;
                        angular.forEach($scope.documentListOtherDetailsseven, function (dd) {
                            if (dd.vmsscD_Filepath !== null && dd.vmsscD_Filepath !== "") {
                                var img = dd.vmsscD_Filepath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("dd.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
                                }
                            }
                        });
                    }
                }
            });
        };

        $scope.clear7 = function () {
            $state.reload();
        }

    }
})();