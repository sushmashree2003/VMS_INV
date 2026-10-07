(function () {
    'use strict';
    angular
        .module('app')
        .controller('Lead_mou_Controller', Lead_mou_Controller);

    Lead_mou_Controller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce', '$timeout', 'Excel', '$window'];
    function Lead_mou_Controller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce, $timeout, Excel, $window) {


        //=============image
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.maxDatemf = new Date();
        //loading start

        $scope.athflag = false;
        $scope.loadData = function () {
            var id = 2;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            apiService.getURI("Lead_mou_/OnloadGetDetails", id).
                then(function (promise) {
                    $scope.grid_view = false;
                    $scope.mompendinglist = promise.mompendinglist;

                    if (promise.mousavedeatils != null && promise.mousavedeatils) {
                        $scope.mousavedeatils = promise.mousavedeatils;
                    }
                    
                });
        };



        $scope.get_lead_product = function () {

            var data = {
                "ISMSLE_Id": $scope.ISMSLE_Id.ISMSLE_Id
            }
            apiService.create("Lead_mou_/lead_product", data).
                then(function (promise) {
                    $scope.momproductpendinglist = promise.momproductpendinglist;
                    $scope.ISMSMOUA_ProductName = $scope.momproductpendinglist[0].ISMSMPR_ProductName;
                    $scope.ISMSMOU_Studentstrength = $scope.momproductpendinglist[0].ISMSLE_StudentStrength;
                    $scope.ISMSMOU_Amount = $scope.momproductpendinglist[0].Negotiation_Amount;
                    $scope.ISMSMOU_PackageName = $scope.momproductpendinglist[0].ISMSBOM_BOMName;
                })

        }


        //$scope.togchkbxdept = function (groupType) {
        //    $scope.ISMSLEDMPR_Id = $scope.Mompendinglist.every(function (itm) {
        //        return itm.selected;
        //    });

        //};

        //$scope.all_checkdept = function (ISMSLEDMPR_Id) {
        //    var toggleStatus = ISMSLEDMPR_Id;
        //    angular.forEach($scope.Mompendinglist, function (itm) {
        //        itm.selected = toggleStatus;
        //    });

        //};








        //===========================ADD==================================
        $scope.documentListOtherDetails = [{ id: 'document' }];
        $scope.addNewDocumentOtherDetail = function () {
            var newItemNo = $scope.documentListOtherDetails.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListOtherDetails.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentOtherDetail = function (index, data) {
            var newItemNo = $scope.documentListOtherDetails.length - 1;
            $scope.documentListOtherDetails.splice(index, 1);
            if (data.hreothdeT_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
        };
        //==============================

        //========================upload files===========
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

                        data.file = d[0].path;
                        data.FileName = d[0].name;

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



        $scope.savedata = function (obj) {
            var data = {};
            $scope.files_paths = [];
            if ($scope.myForm.$valid) {
                if ($scope.documentListOtherDetails !== null) {
                    angular.forEach($scope.documentListOtherDetails, function (qq) {
                        if (qq.file !== undefined && qq.file !== null) {
                            $scope.files_paths.push({ ISMSMOUA_AttachmentName: qq.file, ISMSMOUA_AttachmentPath: qq.FileName });
                        }
                    });
                }
                else {
                    $scope.files_paths = undefined;
                }


                data = {
                    "ISMSLE_Id": $scope.ISMSLE_Id.ISMSLE_Id,
                    "ISMSMOUA_ProductName": $scope.ISMSMOUA_ProductName,
                    "ISMSMOU_Studentstrength": $scope.ISMSMOU_Studentstrength,
                    "ISMSMOU_Amount": $scope.ISMSMOU_Amount,
                    "ISMSMOU_Packagename": $scope.ISMSMOU_PackageName,
                    ISM_Sales_Lead_MOU_Attachments: $scope.files_paths,
                };
                apiService.create("Lead_mou_/savedata", data).then(function (promise) {
                    if (promise.msg === "Update") {
                        swal('Record Updated Successfully');
                        $state.reload();
                    }
                    else if (promise.msg === "save") {
                        swal('Record Saved Successfully');
                        $state.reload();
                    }
                    else {
                        swal('Operation Failed');
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };







        $scope.Viewdata = function (user) {
            var data = {
                "ISMSMOU_Id": user.ISMSMOU_Id
            }
            apiService.create("Lead_mou_/MOUViewdata", data).
                then(function (promise) {



                    if (promise.mouattachmentsdeatils != null && promise.mouattachmentsdeatils.length > 0) {
                        $scope.mouattachmentsdeatils = promise.mouattachmentsdeatils;
                        $('#ViewDetails').modal('show');

                    }
                    else {
                        swal("File Is Not Attached !");
                        $('#ViewDetails').modal('hide');
                    }
                })
        }
       

        $scope.previewimg_new_att = function (img) {
            $scope.vbabrsF_FilePath = img;
            var img = $scope.vbabrsF_FilePath;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.vbabrsF_FilePath);
                    $('#myimagePreview').modal('show');

                }
                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                    $window.open($scope.vbabrsF_FilePath);
                }
                else if (lastelement == 'pdf') {
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.vbabrsF_FilePath;
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
            }
            else {
                $window.open($scope.vbabrsF_FilePath)
            }
     

        };



        $scope.toggleAllstd = function () {
            $scope.sendsmsemail = false;
            var toggleStatus = $scope.stdall;
            angular.forEach($scope.mousavedeatils, function (itm) {
                itm.stdselected = toggleStatus;
            });
            angular.forEach($scope.mousavedeatils, function (itm) {
                if (itm.stdselected == true) {
                    $scope.sendsmsemail = true;
                }

            });

        };
        $scope.optionToggledstd = function () {
            $scope.sendsmsemail = false;
            $scope.stdall = $scope.mousavedeatils.every(function (itm) {
                return itm.stdselected;
            });
            angular.forEach($scope.mousavedeatils, function (itm) {
                if (itm.stdselected == true) {
                    $scope.sendsmsemail = true;
                }

            });

        };

        $scope.SendEmail = function () {

            $scope.noticeid = [];

            angular.forEach($scope.mousavedeatils, function (ty) {
                if (ty.stdselected == true) {

                    $scope.noticeid.push({
                        ISMSMOU_Id: ty.ISMSMOU_Id
                    });
                }
            })
            var data =
            {
                "ISMSMOU_Ids": $scope.noticeid
            };


            apiService.create("Lead_mou_/sendmail", data).
                then(function (promise) {
                    if (promise.message == "success") {
                        swal("Email Sent..!!!");
                        $state.reload();
                    }
                    else if (promise.message == "notFound") {
                        swal("Email Not sent..!!!", 'Default Email-Id is Not Found.. !!!');
                        $state.reload();
                    }
                    else if (promise.message == "Error") {
                        swal("Something went wrong", 'Try After some time..!!');
                        $state.reload();

                    } else {
                        swal("Something went wrong", 'Try After some time..!!');
                        $state.reload();
                    }
                });


        };






        $scope.clearid = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }

})();