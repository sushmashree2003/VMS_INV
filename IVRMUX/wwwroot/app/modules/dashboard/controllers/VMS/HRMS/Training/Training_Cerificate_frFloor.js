(function () {
    'use strict';
    angular
        .module('app')
        .controller('Training_Cerificate_frFloorController', Training_Cerificate_frFloorController)

    Training_Cerificate_frFloorController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$sce', '$q']
    function Training_Cerificate_frFloorController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $sce, $q) {

        $scope.sortKey = 'ISMCLTPRDOC_Id';
        $scope.sortReverse = true;

        $scope.obj = {};

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        //$scope.materaldocuupload.push({
        //    cfilepath: '',          
        //    cfilename1: '',         
        //    cfilepath1: '',         
        //    filetype: '',          
        //    VTADAAF_Remarks: ''   
        //});

        $scope.addNewsiblingguard = function () {
            if ($scope.materaldocuupload.length > 1) {
                for (var i = 0; i === $scope.materaldocuupload.length; i++) {
                    var id = $scope.materaldocuupload[i].itrS_Id;
                    var lastChar = id.substr(id.length - 1);
                    $scope.cnt = parseInt(lastChar);
                }
            }
            $scope.cnt = $scope.cnt + 1;
            $scope.tet = 'trans' + $scope.cnt;
            var newItemNo = $scope.cnt;
            $scope.materaldocuupload.push({ 'itrS_Id': 'trans' + newItemNo });


        };



        $scope.removeRow = function (index) {
            var newItemNo = $scope.fmsfilesupload.length - 1;
            $scope.fmsfilesupload.splice(index, 1);

            if ($scope.fmsfilesupload.length === 0) {
                //data
            }
        };


        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));



        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
        //===========
        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };
        $scope.onview = function (filepath, filename) {
            //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
            var imagedownload = filepath;
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
        };
        //============upload photo

        $scope.uploadmateraldocuments1 = [];

        $scope.uploadmateraldocuments = function (input, document) {

            $scope.uploadmateraldocuments1 = input.files;
            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                // Check for file types and sizes
                var file = input.files[0];
                var fileType = file.type;
                var fileSize = file.size;

                // Image, PDF, and Video checks
                if (fileType === "image/jpeg" && fileSize <= 2097152) {  // 2MB limit for images
                    UploaddianmateralPhoto(document);
                }
                else if (fileType === "video/mp4") {
                    UploaddianmateralPhoto(document);
                }
                else if (fileType === "application/pdf") {
                    UploaddianmateralPhoto(document);
                }
                // Check for DOCX and other Microsoft Office files
                else if (fileType === "application/msword" || fileType === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                    UploaddianmateralPhoto(document);
                }
                // Check for Excel files (XLSX, XLS)
                else if (fileType === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || fileType === "application/vnd.ms-excel") {
                    UploaddianmateralPhoto(document);
                }
                // Check for DOCX files as a specific condition
                else if (fileType === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                    UploaddianmateralPhoto(document);
                }
                // If file size exceeds 2MB, show an error message
                else if (fileSize > 2097152) {
                    swal("File size should be less than 2MB");
                    return;
                }
                // If the file type is not supported, show an error message
                else {
                    swal("Upload MP4, PDF, DOCX, XLSX, and Image Files Only");
                }
            }
        };


        function UploaddianmateralPhoto(data) {
            console.log("Student Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadmateraldocuments1.length; i++) {
                formData.append("File", $scope.uploadmateraldocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadspaceBooking", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    $scope.cfilepath = d;
                    $scope.cfilepath1 = d;
                    $scope.cfilename = $scope.filename;
                    $scope.cfilename1 = $scope.filename;
                    //$('#').attr('src', data.cfilepath);
                    var img = $scope.cfilepath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    $scope.filetype = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        $scope.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.cfilepath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        //==============

        $scope.materaldocuupload = [{ id: 'Materal1' }];

        $scope.linkdata = [{ id: 'link1' }];
        //get data


        ////Multiple File Upload

        $scope.materaldocuupload = [{ id: 'Materal1' }];

        //$scope.addmateral = function () {
        //    var newItemNo = $scope.materaldocuupload.length + 1;

        //    if (newItemNo <= 10) {
        //        $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
        //    }
        //};
        $scope.addmateral = function () {
            var newItemNo = $scope.materaldocuupload.length + 1;

            if (newItemNo <= 10) {
                // Add a new row with empty data
                $scope.materaldocuupload.push({
                    id: 'Materal' + newItemNo,
                    VTADAAF_Remarks: '', // Empty value for file name or remarks
                    cfilepath: '', // Empty value for file path
                    cfilename: '', // Empty value for file name
                    filetype: '', // Empty file type
                    document_Pathnew: '' // Empty document path
                });
            }
        };


        $scope.removemateral = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

            if ($scope.materaldocuupload.length === 0) {
                //data
            }
        };

        //====================
        $scope.viewdocument = function (ss) {

            var img = ss.HRETF_FilePath;
            var imagarr = img.split('.');
            var lastelement = imagarr[imagarr.length - 1];

            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                $scope.xl = "";
                $scope.xl = "https://view.officeapps.live.com/op/view.aspx?src=" + ss.HRETF_FilePath;

                $('#myModalexl').modal('show');
            }
            else if (lastelement === 'jpg' || lastelement === 'jpeg') {
                $('#preview').attr('src', img);
                $('img').bind('contextmenu', function (e) {
                    return false;
                });
                $('#myModalimg').modal('show');
            }

            else if (lastelement === 'pdf') {
                $scope.content = "";
                var fileURL = "";
                var file = "";
                $http.get(img, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        $scope.content = $sce.trustAsResourceUrl(fileURL);
                        $('#showpdf').modal('show');
                    });
            }
        }
        //======================
        //get data
        $scope.getAllDetail = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            //apiService.getURI("INV_LocationDashboard/getalldetails", pageid)
            //    .then(function (promise) {
            //        $scope.AllEployess = promise.checkoutemployeelist;
                  
            //    })

            //apiService.getURI("INV_LocationDashboard/getalldetails", pageid)
            //    .then(function (promise) {
                   
            //        $scope.AllEployess = promise.checkoutemployeelist;
            //        return apiService.getURI("AddtoHRMS/getalldetails", pageid);
            //    }).then(function (promise) {
            //        $scope.Alldepartments = promise.employeedepartmentlist;
            //    })
            //    .catch(function (error) {
            //        console.error("Error during API calls:", error);
            //    }); 

            apiService.getURI("INV_LocationDashboard/getalldetails", pageid)
                .then(function (promise) {
                    $scope.AllEployess = promise.checkoutemployeelist;
                    return apiService.getURI("AddtoHRMS/getalldetails", pageid);
                })
                .then(function (promise) {
                    $scope.Alldepartments = promise.employeedepartmentlist;
                    return apiService.getURI("Appointment/getalldetails", pageid); // Add the third API call here
                })
                .then(function (promise) {
                    //$scope.AllDataTraningHR1 = promise.allDataTraningHR;
                    $scope.AllDataTraningHR1 = [];
                    if (promise.allDataTraningHR != null && promise.allDataTraningHR.length > 0) {
                        $scope.AllDataTraningHR1 = promise.allDataTraningHR;
                        angular.forEach(promise.allDataTraningHR, function (dd) {
                            dd.checked = false;
                        });
                    }
                })
                .catch(function (error) {
                    console.error("Error during API calls:", error);
                });


        }

        //save
        $scope.submitted = false;
        $scope.SaveTraning = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ISMMCLTPR_Id": $scope.ISMMCLTPR_Id,
                    "HRME_ID": $scope.HRME_Id,
                    "HRMD_Id": $scope.HRMD_Id,
                    "cfilepath": $scope.cfilepath,
                    "cfilename": $scope.cfilename,
                    "HRETF_FileType": $scope.obj.VMMV_IdentityCardTypes,
                    "HRRemarks": $scope.obj.HRRemarkss,
                    "ISMCLTPRDOC_Id": $scope.Id
                }
                apiService.create("Appointment/SaveFODetails", data).
                    then(function (promise) {


                        if (promise.Message === "Update") {
                            swal('Record Updated Successfully');
                        }
                        else if (promise.Message === "Add") {
                            swal('Record Saved Successfully');

                        }
                        else if (promise.Message === "Error") {
                            swal('Record Not Saved/Updated successfully');

                        }
                        $state.reload();
                    })

            }
            else {

                $scope.submitted = true;
            }

        };

        //edit
        $scope.edibl = {}
        $scope.edit = function (bil) {
            $scope.edibl = bil.ISMCLTPRDOC_Id;
            var pageid = $scope.edibl;
            apiService.getURI("ISM_Client_Project/details_DOC", pageid).
                then(function (promise) {
                    $scope.Id = promise.doc_details[0].ISMCLTPRDOC_Id;
                    $scope.client_project_name = promise.doc_details[0].client_project_name;
                    $scope.ISMMCLTPR_Id = promise.doc_details[0].ISMMCLTPR_Id;
                    $scope.ISMCLTPRMDOC_Id = promise.doc_details[0].ISMCLTPRMDOC_Id;
                    $scope.ismcltprmdoC_Name = promise.doc_details[0].ISMCLTPRMDOC_Name;
                    $scope.ISMCLTPRDOC_Date = new Date(promise.doc_details[0].ISMCLTPRDOC_Date);
                    $scope.cfilepath = promise.doc_details[0].ISMCLTPRDOC_FilePath;

                    var img = promise.doc_details[0].ISMCLTPRDOC_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    $scope.filetype = lastelement;

                })
        };



        $scope.cancel = function () {
            $state.reload();

        }

        //deactive
        $scope.deactive = function (flr, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (flr.ISMCLTPRMP_ActiveFlag === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Academic Year?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ISM_Client_Project/deactivate_DOC", flr).
                            then(function (promise) {


                                if (promise.returndata === 'false') {
                                    swal('Client Project Documents Deactivated Successfully.');
                                }

                                else if (promise.returndata === 'true') {
                                    swal('Client Project Documents Activated Successfully.');
                                }

                                else if (promise.returndata === 'Error') {
                                    swal('Operation Failed!!!');
                                }


                                $scope.getAllDetail();
                            });
                    } else {
                        swal("Cancelled");
                        $scope.getAllDetail();
                    }

                });
        };




        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


    }

})();



