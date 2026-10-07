
(function () {
    'use strict';
    angular.module('app').controller('Master_TrainingTopicMaterialController', Master_TrainingTopicMaterialController)
    Master_TrainingTopicMaterialController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache', 'myFactoryvisitor', '$q']
    function Master_TrainingTopicMaterialController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache, myFactoryvisitor, $q) {

        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.obj1 = {};
        $scope.obsupdate = {};
        $scope.edit = false;
        $scope.plMaxdate = new Date();
        $scope.MOM_Date = new Date();
        $scope.plMaxdate.setDate($scope.plMaxdate.getDate());
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };
        var miid = myFactoryvisitor.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }


        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("Training_Master/loaddata", pageid).then(function (promise) {
                $scope.training = promise.training;
                $scope.getmasterdata = promise.getmasterdata;
            });
        };
        $scope.saveData = function () {

            if ($scope.myForm.$valid) {
                $scope.uploadarray = [];
                if ($scope.documentListOtherDetails.length > 0) {
                    angular.forEach($scope.documentListOtherDetails, function (itm) {
                        if (itm.FilePath !== undefined && itm.FilePath !== null && itm.FilePath !== "") {
                            $scope.uploadarray.push({
                                FilePath: itm.FilePath,
                                FileName: itm.FileName
                            });
                        }
                    });
                }

                $scope.HRMETRTYTOPM_StudyMaterialsDesc = document.getElementById("myP").innerHTML;                var obj = {
                    "HRMETRTYTOPM_Id": $scope.HRMETRTYTOPM_Id,
                    "HRMETRTYTOPM_StudyMaterialsDesc": $scope.HRMETRTYTOPM_StudyMaterialsDesc,
                    "HRMTT_Id": $scope.hrmtT_Id,
                    "FilePath_Array": $scope.uploadarray,
                };
                apiService.create("Training_Master/SaveTopic_content", obj).then(function (promise) {

                    if (promise.returnvalue !== "") {

                        if (promise.returnvalue === "Update") {
                            $scope.getmasterdata = promise.getmasterdata;
                            swal('Topic content Updated Successfully');
                            $state.reload();
                        }
                        else if (promise.returnvalue === "Add") {
                            $scope.getmasterdata = promise.getmasterdata;
                            swal('Topic content Saved Successfully');
                            $state.reload();
                        }
                        else if (promise.returnvalue === "false") {
                            swal('Record Not Saved/Updated successfully');
                        }
                        else if (promise.returnvalue === "Duplicate") {
                            swal('Topic content Already Exist');
                        }
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

        $scope.submitted = false;

        //===========================ADD==================================
        $scope.employeenameschange = [{ id: 'sell11' }];



        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //===================Active/Deactive=====================////////////////////////////      

        $scope.agendadetails = function (item) {
            $scope.hrmetrtytopM_StudyMaterialsDesc = item.hrmetrtytopM_StudyMaterialsDesc
            document.getElementById("ada").innerHTML = $scope.hrmetrtytopM_StudyMaterialsDesc;
        };


        //===========================ADD files==================================
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
                        data.FilePath = d[0].path;
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

    }
})();