(function () {
    'use strict';
    angular.module('app').controller('HW_HealthCheckUpController', HW_HealthCheckUpController)
    HW_HealthCheckUpController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', '$sce']
    function HW_HealthCheckUpController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, $sce) {

        var paginationformasters;
        var copty;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            paginationformasters = 10;
        } else {
            paginationformasters = 10;
        }
        $scope.obj = {};
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.submitted1 = false;
        $scope.temptaskreponsefiles = [];
        $scope.temptaskreponsefiles.push({ 'id': 'Teacher1' });
        $scope.objques = {};

        $scope.VisitedDate = new Date();
        $scope.visitedmaxdate = new Date();

        $scope.LoadHWHealthCheckUpdata = function () {
            $scope.getuserinstitution = [];
            $scope.MI_Id = "";
            $scope.Remarks = "";
            $scope.search4 = "";
            $scope.getclientlist = [];
            $scope.getloaddata = [];
            $scope.objques.ISMMCLT_Id = "";
            $scope.objques.HRME_Id = "";
            $scope.getemployeedetails = [];
            $scope.submitted1 = false;
            $scope.editable = false;
            $scope.HWHCHUP_Id = 0;

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;

            var data = {
                "OnLoadOrOnChange": "OnLoad"
            };

            apiService.create("HW_HeathCheckUp/OnLoadHWHealthCheckUp", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getuserinstitution = promise.getuserinstitution;
                    $scope.MI_Id = promise.mI_Id;
                    $scope.getclientlist = promise.getclientlist;
                    $scope.getloaddata = promise.getloaddata;
                    $scope.getemployeedetails = promise.getemployeedetails;
                }
            });
        };

        $scope.OnChangeHwHealthCheckUpInst = function () {
            $scope.getclientlist = [];
            $scope.objques.ISMMCLT_Id = "";
            $scope.search4 = "";
            $scope.Remarks = "";
            $scope.editable = false;
            $scope.submitted1 = false;
            $scope.HWHCHUP_Id = 0;
            $scope.temptaskreponsefiles = [];
            $scope.getloaddata = [];
            $scope.temptaskreponsefiles.push({ 'id': 'Teacher1' });
            $scope.VisitedDate = new Date();
            $scope.visitedmaxdate = new Date();
            $scope.objques.HRME_Id = "";
            $scope.getemployeedetails = [];
            var data = {
                "OnLoadOrOnChange": "OnChange",
                "MI_Id": $scope.MI_Id
            };

            apiService.create("HW_HeathCheckUp/OnLoadHWHealthCheckUp", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getclientlist = promise.getclientlist;
                    $scope.getloaddata = promise.getloaddata;
                    $scope.getemployeedetails = promise.getemployeedetails;
                }
            });
        };

        $scope.SaveHWHealthChekUp = function () {
            $scope.submitted1 = false;
            if ($scope.myForm1.$valid) {    
                var data = {
                    "MI_Id": $scope.MI_Id,
                    "HWHCHUP_Id": $scope.HWHCHUP_Id,
                    "ISMMCLT_Id": $scope.objques.ISMMCLT_Id.ismmclT_Id,
                    "HRME_Id": $scope.objques.HRME_Id.hrmE_Id,
                    "VisitedDate": new Date($scope.VisitedDate).toDateString(),
                    "Remarks": $scope.Remarks,
                    "UploadedFiles": $scope.temptaskreponsefiles
                };
                apiService.create("HW_HeathCheckUp/SaveHWHealthChekUp", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Record Saved/Updated Successfully");
                        } else {
                            swal("Failed To Save/Updated Record");
                        }

                        $scope.OnChangeHwHealthCheckUpInst();
                    }
                });
            } else {
                $scope.submitted1 = true;
            }
        };

        $scope.EditHealthCheckUp = function (obj) {
            $scope.temptaskreponsefiles = [];
            $scope.temptaskreponsefiles.push({ 'id': 'Teacher1' });
            var data = {
                "HWHCHUP_Id": obj.hwhchuP_Id,
                "MI_Id": $scope.MI_Id
            };

            apiService.create("HW_HeathCheckUp/EditHealthCheckUp", data).then(function (promise) {
                if (promise !== null) {
                    $scope.geteditdetails = promise.geteditdetails;
                    $scope.editable = true;
                    $scope.HWHCHUP_Id = $scope.geteditdetails[0].hwhchuP_Id;
                    $scope.Remarks = $scope.geteditdetails[0].remarks;
                    $scope.VisitedDate = new Date($scope.geteditdetails[0].visitedDate);

                    $scope.geteditclientdetails = promise.geteditclientdetails;
                    $scope.geteditemployeedetails = promise.geteditemployeedetails;
                    $scope.objques.ISMMCLT_Id = $scope.geteditclientdetails[0];

                    $scope.objques.HRME_Id = $scope.geteditemployeedetails[0];


                    if ($scope.geteditemployeedetails != null) {
                        for (var i = 0; i < $scope.geteditdetails.length; i++) {
                            if ($scope.geteditemployeedetails[0].HRME_Id == $scope.geteditdetails[i].HRME_Id) {
                                $scope.getemployeedetails[i].Selected = true;
                                $scope.objques.HRME_Id = $scope.getemployeedetails[i];

                            }
                        }
                    }


                    if (promise.geteditfilesdetails !== null && promise.geteditfilesdetails.length > 0) {
                        $scope.temptaskreponsefiles = promise.geteditfilesdetails;
                        angular.forEach($scope.temptaskreponsefiles, function (dd) {
                            if (dd.filepath !== null && dd.filepath !== "") {
                                var img = dd.filepath;
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

        $scope.ViewHealthCheckUpFiles = function (obj) {

            var data = {
                "HWHCHUP_Id": obj.hwhchuP_Id,
                "MI_Id": $scope.MI_Id
            };

            apiService.create("HW_HeathCheckUp/ViewHealthCheckUpFiles", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getviewfilesdetails !== null && promise.getviewfilesdetails.length > 0) {
                        $scope.getviewfilesdetails = promise.getviewfilesdetails;
                        angular.forEach($scope.getviewfilesdetails, function (dd) {
                            if (dd.filepath !== null && dd.filepath !== "") {
                                var img = dd.filepath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("dd.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
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

        $scope.ActiveDeactiveHealthCheckUp = function (obj) {

            var mgs = "";
            var confirmmgs = "";
            if (obj.activeFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }

            var data = {
                "HWHCHUP_Id": obj.hwhchuP_Id,
                "MI_Id": $scope.MI_Id
            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("HW_HeathCheckUp/ActiveDeactiveHealthCheckUp", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record " + confirmmgs + " " + "successfully");
                            }
                            else {
                                swal("Record " + mgs + " Failed");
                            }
                            $scope.OnChangeHwHealthCheckUpInst();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.DeleteHWHealthCheckUpFiles = function (obj) {

            var mgs = "";
            var confirmmgs = "";

            var data = {
                "HWHCHUP_Id": obj.hwhchuP_Id,
                "HWHCHUPF_Id": obj.hwhchupF_Id,
                "MI_Id": $scope.MI_Id
            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To Delete This Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("HW_HeathCheckUp/DeleteHWHealthCheckUpFiles", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record Deleted successfully");
                            }
                            else {
                                swal("Failed To Delete Record");
                            }
                            if (promise.getviewfilesdetails !== null && promise.getviewfilesdetails.length > 0) {
                                $scope.getviewfilesdetails = promise.getviewfilesdetails;
                                angular.forEach($scope.getviewfilesdetails, function (dd) {
                                    if (dd.filepath !== null && dd.filepath !== "") {
                                        var img = dd.filepath;
                                        var imagarr = img.split('.');
                                        var lastelement = imagarr[imagarr.length - 1];
                                        dd.filetype = lastelement;
                                        console.log("dd.filetype : " + dd.filetype);
                                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                            dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
                                        }
                                    }
                                });
                                $('#modalviewfiles').modal('show');
                            } else {
                                $scope.OnChangeHwHealthCheckUpInst();
                                //swal("No Records Found");
                                $('#modalviewfiles').modal('hide');
                            }
                        });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };

        $scope.ViewHealthCheckUpComments = function (obj) {

            var data = {
                "HWHCHUP_Id": obj.hwhchuP_Id,
                "MI_Id": $scope.MI_Id
            };

            apiService.create("HW_HeathCheckUp/ViewHealthCheckUpComments", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.viewcommens !== null && promise.viewcommens.length > 0) {
                        $scope.viewcommens = promise.viewcommens;
                        $('#modalviewcomments').modal('show');
                    } else {
                        swal("No Records Found");
                        $('#modalviewcomments').modal('hide');
                    }
                }
            });
        };

        $scope.ViewFile = function (docpath, filename, filetype) {

            if (filetype === 'Excel') {
                docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + docpath;
                $scope.detailFrame = $sce.trustAsResourceUrl(docpath);
                $('#myModaldocx').modal('show');
            } else {
                var imagedownload = docpath;
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
            }
        };

        $scope.DownloadFile = function (data, idd, filetype) {
            var file_type = "";
            if (filetype === "Pdf") {
                file_type = '.pdf';
            } else {
                file_type = '.xls';
            }
            var studentreg = idd;
            $scope.imagedownload = data;
            var imagedownload = data;
            $http.get(imagedownload, { responseType: "arraybuffer" }).success(function (data) {
                var anchor = angular.element('<a/>');
                var blob = new Blob([data]);
                anchor.attr({
                    href: window.URL.createObjectURL(blob),
                    target: '_blank',
                    download: studentreg + file_type
                })[0].click();
            });
        };

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
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

        $scope.ViewDocExcelfles = function (docpath, filename, filetype) {
            docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + docpath;
            $scope.detailFrame = $sce.trustAsResourceUrl(docpath);
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

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.Clearid = function () {
            $scope.OnChangeHwHealthCheckUpInst();
        };

        $scope.addNewviewresponse = function () {
            var newItemNo = $scope.temptaskreponsefiles.length + 1;

            if (newItemNo <= 10) {
                $scope.temptaskreponsefiles.push({ 'id': 'Teacher' + newItemNo });
            }
            console.log($scope.temptaskreponsefiles);
        };

        $scope.removeNewviewresponse = function (index) {
            var newItemNo = $scope.temptaskreponsefiles.length - 1;
            $scope.temptaskreponsefiles.splice(index, 1);

            if ($scope.temptaskreponsefiles.length === 0) {
                //data
            }
        };

        $scope.uploadtecherdocuments1 = [];

        $scope.uploadtecherdocuments = function (input, document) {

            $scope.uploadtecherdocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/jpg" || input.files[0].type === "image/png") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-powerpoint") {
                    UploaddianPhoto(document);
                }
                else {
                    swal("Upload MP4, Pdf, Doc, Image Files Only");
                }
            }
        };

        function UploaddianPhoto(data) {
            console.log("Teacher Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadtecherdocuments1.length; i++) {
                formData.append("File", $scope.uploadtecherdocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/HWHealthCheckUpFiles", formData,
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': undefined
                    },
                    transformRequest: angular.identity,

                })
                .success(function (d) {
                    defer.resolve(d);
                    data.filepath = d;
                    data.filename = $scope.filename;
                    $('#').attr('src', data.filepath);
                    var img = data.filepath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.filepath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }
    }



})();