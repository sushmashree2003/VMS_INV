(function () {
    'use strict';
    angular.module('app').controller('TenantAgreementController', TenantAgreementController)

    TenantAgreementController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter', '$q', '$sce', '$window', '$compile']
    function TenantAgreementController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter, $q, $sce, $window,  $compile) {

        var paginationformasters;
        $scope.multidepartment = false;
        $scope.editflag = false;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 10;
        }
        $scope.obj = {};
        $scope.transnumbconfigurationsettings = [];
        $scope.getloaddata = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        $scope.MonthlyRent = 0;
        $scope.MonthlyRentAmount = 0;
       
        $scope.groups = [{ title: 'Dynamic Group Header - 1', content: 'Dynamic Group Body - 1' },
        { title: 'Dynamic Group Header - 2', content: 'Dynamic Group Body - 2' },
        { title: 'Dynamic Group Header - 3', content: 'Dynamic Group Body - 3' },
        { title: 'Dynamic Group Header - 4', content: 'Dynamic Group Body - 4' }];

        $scope.searc_button = true;
        $scope.sortKey = 'OTMOEQ_Id';
        $scope.sortReverse = true;
        $scope.OTMOEQ_SubjectiveFlg = false;
        $scope.edit = false;
        $scope.getexamhappenedcounttemp = false;
        $scope.submitted1 = false;
        $scope.submitted3 = false;
        $scope.duration = true;
        $scope.aggstartdate = false;
        $scope.aggenddate = false;

        $scope.showgridsave = true;
        $scope.searchchkbx = "";
        $scope.searchchkbxtype = "";
        $scope.search = "";

        $scope.answer = "";
        $scope.show_ansOption = false;
        $scope.obj = {};
        $scope.obj.searchValueddd = "";
        $scope.searchValue22 = "";
        $scope.btn = false;
        $scope.teacherdocuupload = [];
        $scope.teacherdocuupload = [{ id: 'Teacher1' }];
        $scope.totalgrid = [];
        $scope.TENANTAGR_AgrFromDate = new Date();
        $scope.TENANTAGR_LeftFlg = false;
        //***** MASTER QUESTION ******//
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;
           
            apiService.getURI("TenantAgreement/getdata/", pageid).then(function (promise) {
                
                $scope.building = promise.building;
                $scope.rooms = promise.rooms;
                
                angular.forEach(promise.getloaddata, function (dd) {
                    if (dd.tenantagR_LeftFlg==true) {
                        $scope.getloaddata.push({
                            tenantagR_Id:dd.tenantagR_Id,
                            tenantagR_TenantName:dd.tenantagR_TenantName,
                            tenantagR_NoOfOccupants:dd.tenantagR_NoOfOccupants,
                            hrmB_BuildingName:dd.hrmB_BuildingName,
                            hrmR_RoomName:dd.hrmR_RoomName,
                            tenantagR_PANCardNo:dd.tenantagR_PANCardNo,
                            tenantagR_Aadhar:dd.tenantagR_Aadhar,
                            tenantagR_ContactNo:dd.tenantagR_ContactNo,
                            tenantagR_EmailId:dd.tenantagR_EmailId,
                            tenantagR_AdvanceAmount:dd.tenantagR_AdvanceAmount,
                            tenantagR_MonthlyRent:dd.tenantagR_MonthlyRent,
                            tenantagR_AgrFromDate:dd.tenantagR_AgrFromDate,
                            tenantagR_AgrToDate:dd.tenantagR_AgrToDate,
                            tenantagR_Remarks: dd.tenantagR_Remarks,
                            tenantagR_RentDueDay:dd.tenantagR_RentDueDay,
                            cls: 'backkk',
                            tenantagR_UserId: dd.tenantagR_UserId,
                            tenantagR_RefundedAmount: dd.tenantagR_RefundedAmount,
                            tenantagR_RefundModeOfPayment: dd.tenantagR_RefundModeOfPayment
                        });
                    }
                    else {
                        $scope.getloaddata.push({
                            tenantagR_Id: dd.tenantagR_Id,
                            tenantagR_TenantName: dd.tenantagR_TenantName,
                            tenantagR_NoOfOccupants: dd.tenantagR_NoOfOccupants,
                            hrmB_BuildingName: dd.hrmB_BuildingName,
                            hrmR_RoomName: dd.hrmR_RoomName,
                            tenantagR_PANCardNo: dd.tenantagR_PANCardNo,
                            tenantagR_Aadhar: dd.tenantagR_Aadhar,
                            tenantagR_ContactNo: dd.tenantagR_ContactNo,
                            tenantagR_EmailId: dd.tenantagR_EmailId,
                            tenantagR_AdvanceAmount: dd.tenantagR_AdvanceAmount,
                            tenantagR_MonthlyRent: dd.tenantagR_MonthlyRent,
                            tenantagR_AgrFromDate: dd.tenantagR_AgrFromDate,
                            tenantagR_AgrToDate: dd.tenantagR_AgrToDate,
                            tenantagR_Remarks: dd.tenantagR_Remarks,
                            tenantagR_RentDueDay: dd.tenantagR_RentDueDay,
                            tenantagR_UserId: dd.tenantagR_UserId,
                            tenantagR_RefundedAmount: dd.tenantagR_RefundedAmount,
                            tenantagR_RefundModeOfPayment: dd.tenantagR_RefundModeOfPayment

                        });
                    }
                });

                $scope.tenants = promise.tenants;
                $scope.tenantrenewal = promise.tenantrenewal;
                $scope.getfloordetails = promise.getfloordetails;
                $scope.reference = promise.reference;
                $scope.getownerdetails = promise.getownerdetails;
                //$scope.milist = promise.milist;
                //$scope.getarratcomplexities = promise.getarratcomplexities;
            });
        };

        $scope.monthcalculation = function () {

            $scope.TENANTAGR_AgreementDuration = $scope.obj.TENANTAGR_AgrToDate.getMonth() - $scope.TENANTAGR_AgrFromDate.getMonth() + (12 * ($scope.obj.TENANTAGR_AgrToDate.getFullYear() - $scope.TENANTAGR_AgrFromDate.getFullYear()))

            //var months;


            //months = ($scope.TENANTAGR_AgrToDate.getFullYear() - $scope.TENANTAGR_AgrFromDate.getFullYear()) * 12;
            //months -= $scope.TENANTAGR_AgrToDate.getMonth();

            //$scope.TENANTAGR_AgreementDuration = months;

        }

        $scope.UploadFiles = function (objupload, index) {

            $scope.optionsindex = index;
            if (objupload.tempoptionsfiles !== undefined && objupload.tempoptionsfiles !== null && objupload.tempoptionsfiles.length > 0) {
                $scope.teacherdocuuploadopts = objupload.tempoptionsfiles;

                angular.forEach($scope.teacherdocuuploadopts, function (d) {
                    var img = d.tenantagrfL_FileLocation;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    d.filetype = lastelement;
                    console.log("data.filetype : " + d.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        d.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + d.tenantagrfL_FileLocation;
                    }
                });
            } else {
                $scope.teacherdocuuploadopts = [{ id: 'Teacheropts1' }];
            }
            $('#optsfilesupload').modal('show');
        };


        $scope.UploadFiles1 = function (objupload, index) {

            $scope.optionsindex = index;
            if (objupload.tempoptionsfiles !== undefined && objupload.tempoptionsfiles !== null && objupload.tempoptionsfiles.length > 0) {
                $scope.teacherdocuuploadopts = objupload.tempoptionsfiles;

                angular.forEach($scope.teacherdocuuploadopts, function (d) {
                    var img = d.tenantagrfL_FileLocation;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    d.filetype = lastelement;
                    console.log("data.filetype : " + d.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        d.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + d.tenantagrfL_FileLocation;
                    }
                });
            } else {
                $scope.teacherdocuuploadopts = [{ id: 'Teacheropts1' }];
            }
            $('#optsfilesupload1').modal('show');
        };
        $scope.OnClickOptsFilesUpload = function () {
            $scope.tempoptsfiles = [];
            angular.forEach($scope.teacherdocuuploadopts, function (dd) {
                var id = 0;
                if (dd.tenantagrfL_FileLocation !== undefined && dd.tenantagrfL_FileLocation !== null && dd.tenantagrfL_FileLocation !== "") {
                    if (dd.tenantagrfL_Id !== undefined && dd.tenantagrfL_Id !== null) {
                        id = dd.tenantagrfL_Id;
                    }
                    $scope.tempoptsfiles.push({
                        tenantagrfL_FileLocation: dd.tenantagrfL_FileLocation, tenantagrfL_FileName: dd.tenantagrfL_FileName,
                        tenantagrfL_Id: id
                    });
                }
            });

            if ($scope.tempoptsfiles.length > 0) {
                angular.forEach($scope.totalgrid, function (dd, index) {
                    if (index === $scope.optionsindex) {
                        dd.tempoptionsfiles = $scope.tempoptsfiles;
                    }
                });
            }
        };


        $scope.OnClickOptsFilesUpload1 = function () {
            $scope.tempoptsfiles = [];
            angular.forEach($scope.teacherdocuuploadopts, function (dd) {
                var id = 0;
                if (dd.tenantagrfL_FileLocation !== undefined && dd.tenantagrfL_FileLocation !== null && dd.tenantagrfL_FileLocation !== "") {
                    if (dd.tenantagrfL_Id !== undefined && dd.tenantagrfL_Id !== null) {
                        id = dd.tenantagrfL_Id;
                    }
                    $scope.tempoptsfiles.push({
                        tenantagrfL_FileLocation: dd.tenantagrfL_FileLocation, tenantagrfL_FileName: dd.tenantagrfL_FileName,
                        tenantagrfL_Id: id
                    });
                }
            });

            if ($scope.tempoptsfiles.length > 0) {
                angular.forEach($scope.totalgrid1, function (dd, index) {
                    if (index === $scope.optionsindex) {
                        dd.tempoptionsfiles = $scope.tempoptsfiles;
                    }
                });
            }
        };

        /* Options Files */
        $scope.uploadtecheroptsdocuments1 = [];
        $scope.uploadtecheroptsdocuments = function (input, document) {

            $scope.uploadtecheroptsdocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg")  // 2097152 bytes = 2MB 
                {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload  Pdf, Doc, Image Files Only");
                }
            }
        };
        function UploaddianPhotoopts(data) {
            console.log("Teacher Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadtecheroptsdocuments1.length; i++) {
                formData.append("File", $scope.uploadtecheroptsdocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/lessonplannerdoc", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.tenantagrfL_FileLocation = d;
                    data.tenantagrfL_FileName = $scope.filename;
                    $('#').attr('src', data.tenantagrfL_FileLocation);
                    var img = data.tenantagrfL_FileLocation;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.tenantagrfL_FileLocation;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.addNewsiblingguardopts = function () {
            var newItemNo = $scope.teacherdocuuploadopts.length + 1;

            if (newItemNo <= 10) {
                $scope.teacherdocuuploadopts.push({ 'id': 'Teacheropts1' + newItemNo });
            }
            console.log($scope.teacherdocuuploadopts);
        };

        $scope.removeNewsiblingguardopts = function (index) {
            var newItemNo = $scope.teacherdocuuploadopts.length - 1;
            $scope.teacherdocuuploadopts.splice(index, 1);

            if ($scope.teacherdocuuploadopts.length === 0) {
                //data
            }
        };

        function UploaddianPhoto(data) {
            console.log("Teacher Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadtecherdocuments1.length; i++) {
                formData.append("File", $scope.uploadtecherdocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/lessonplannerdoc", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);

                    data.TENANTAGRFL_FileLocation = d;
                    data.TENANTAGRFL_FileName = $scope.filename;
                    $('#').attr('src', data.TENANTAGRFL_FileLocation);
                    var img = data.TENANTAGRFL_FileLocation;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.TENANTAGRFL_FileLocation;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        


        $scope.showmothersign = function (img) {
            //$('#preview').attr('src', path);

            //$('#myModalimg').modal('show');

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
                $('#myModalimg').modal('show');

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

        $scope.addNewsiblingguard = function () {
            var newItemNo = $scope.teacherdocuupload.length + 1;

            if (newItemNo <= 10) {
                $scope.teacherdocuupload.push({ 'id': 'Teacher' + newItemNo });
            }
            console.log($scope.teacherdocuupload);
        };

        $scope.removeNewsiblingguard = function (index) {
            var newItemNo = $scope.teacherdocuupload.length - 1;
            $scope.teacherdocuupload.splice(index, 1);

            if ($scope.teacherdocuupload.length === 0) {
                //data
            }
        };
        $scope.uploadtecherdocuments1 = [];

        $scope.uploadtecherdocuments = function (input, document) {

            $scope.uploadtecherdocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg")  // 2097152 bytes = 2MB 
                {
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
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload  Pdf, Doc, Image Files Only");
                }
            }
        };

        $scope.amenitiescharges = [];
        
        $scope.getrefundableadvance = function () {
            var data = {
                "TENANTAGR_Id": $scope.tenantagR_Id.tenantagR_Id,
            };
            apiService.create("TenantAgreement/getrefundableadvance", data).then(function (promise) {
                $scope.TENANTAGR_AdvanceAmount = promise.advanceamount;
                $scope.Refundable_Advance = (promise.advanceamount - promise.deductingamount);
                $scope.amenitiescharges = promise.amenitiescharges;

                $scope.TotalAmenitiesAmount = 0;
                angular.forEach($scope.amenitiescharges, function (em) {
                    $scope.TotalAmenitiesAmount = $scope.TotalAmenitiesAmount + em.tenantnoccL_DeductingAmount;
                });
            
            });
        };
        




        $scope.interacted = function (field) {
            return $scope.submitted1 || field.$dirty;
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted2;
        };

        $scope.buildingchange = function () {

            var data = {
                "HRMB_Id": $scope.HRMB_Id,

            };

            apiService.create("TenantAgreement/buildingchange", data).then(function (promise) {
                if (promise !== null) {
                    $scope.rooms = promise.rooms;
                    $scope.getfloordetails = promise.getfloordetails;
                }
            });
        };

        $scope.roomchange = function () {

            var data = {
                "HRMB_Id": $scope.HRMB_Id,
                "HRMR_Id": $scope.HRMR_Id,

            };

            apiService.create("TenantAgreement/roomchange", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getamenitiesdetails = promise.getamenitiesdetails;

                }
            });
        };
        $scope.Floorchange = function () {

            var data = {
                "HRMB_Id": $scope.HRMB_Id,
                "HRMF_Id": $scope.HRMF_Id,


            };

            apiService.create("TenantAgreement/Floorchange", data).then(function (promise) {
                if (promise !== null) {
                    $scope.rooms = promise.rooms;

                }
            });
        };



        $scope.ViewHealthCheckUpFiles = function (obj) {

           
            var data = {
                "TENANTAGR_Id": obj.tenantagR_Id,

            };

            apiService.create("TenantAgreement/ViewFiles", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getviewfilesdetails !== null && promise.getviewfilesdetails.length > 0) {
                        $scope.getviewfilesdetails = promise.getviewfilesdetails;
                        angular.forEach($scope.getviewfilesdetails, function (dd) {
                            if (dd.tenantagrfL_FileLocation !== null && dd.tenantagrfL_FileLocation !== "") {
                                var img = dd.tenantagrfL_FileLocation;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("dd.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.tenantagrfL_FileLocation;
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

        $scope.TenantFullDetails = function (obj) {

            var data = {
                "TENANTAGR_Id": obj.tenantagR_Id,
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("TenantAgreement/TenantDetails", data).then(function (promise) {
               // $('#Tenantdetails').modal('show');
                if (promise.tenantDetails.length > 0 && promise.tenantDetails != null) {
                    $scope.Template = promise.tenantDetails[0].HRMB_Agrement_Template;
                    var e1 = angular.element(document.getElementById("test"));
                    $compile(e1.html($scope.Template))(($scope));

                    $scope.tenantDetails = promise.tenantDetails;
                   
                    $scope.TenantName = promise.tenantDetails[0].TENANTAGR_TenantName;
                    $scope.Address = promise.tenantDetails[0].TENANTAGR_Address;
                    $scope.MI_Name = promise.tenantDetails[0].MI_Name;
                    $scope.MobileNo = promise.tenantDetails[0].TENANTAGR_ContactNo;
                    $scope.AggstartDate = new Date(promise.tenantDetails[0].TENANTAGR_AgrFromDate);
                    $scope.AggToDate = new Date(promise.tenantDetails[0].TENANTAGR_AgrToDate);
                    $scope.duration = promise.tenantDetails[0].TENANTAGR_AgreementDuration;
                    $scope.Advance = promise.tenantDetails[0].TENANTAGR_AdvanceAmount;
                    $scope.advanceinwords = $scope.advanceamountinwords($scope.Advance);
                    $scope.MonthlyRent = promise.tenantDetails[0].TENANTAGR_MonthlyRent;
                    $scope.monthlyrentwords = $scope.amountinwords($scope.MonthlyRent);
                    $scope.BankName = promise.tenantDetails[0].HRMBD_BankName;
                    $scope.AccountNo = promise.tenantDetails[0].HRMBD_BankAccountNo;
                    $scope.IFSCCode = promise.tenantDetails[0].HRMBD_IFSCCode;
                    $scope.BranchName = promise.tenantDetails[0].HRMBD_BranchName;
                    $scope.email = promise.tenantDetails[0].TENANTAGR_EmailId;
                    $scope.pancard = promise.tenantDetails[0].TENANTAGR_PANCardNo;
                    $scope.AccountHolderName = promise.tenantDetails[0].HRMBD_AccHolderName;
                    $scope.AccountType = promise.tenantDetails[0].HRMBD_AccType;
                    $scope.TenantOwnerName = promise.tenantDetails[0].HRMR_Ownername;
                    $scope.BuildingName = promise.tenantDetails[0].HRMB_BuildingName;
                    $scope.FloorName = promise.tenantDetails[0].HRMF_FloorName;
                    $scope.RoomName = promise.tenantDetails[0].HRMR_RoomName;
                    $scope.WaterTaps = promise.tenantDetails[0].HRMR_NoOfTaps;
                    $scope.NoOfBHK = promise.tenantDetails[0].HRMR_NoOfBHK;
                    $scope.HRMB_Id = promise.tenantDetails[0].HRMB_Id;
                    $scope.HRMR_Id = promise.tenantDetails[0].HRMR_Id;
                    $scope.OwnerAdsress = promise.tenantDetails[0].OwnerAddress;
                    $scope.ownerAge = promise.tenantDetails[0].ownerage;
                    
                    $scope.roomchange();
                }
            });
        };

        $scope.advanceamountinwords = function convertNumberToWords(atotalc) {
            var words = new Array();
            words[0] = '';
            words[1] = 'One';
            words[2] = 'Two';
            words[3] = 'Three';
            words[4] = 'Four';
            words[5] = 'Five';
            words[6] = 'Six';
            words[7] = 'Seven';
            words[8] = 'Eight';
            words[9] = 'Nine';
            words[10] = 'Ten';
            words[11] = 'Eleven';
            words[12] = 'Twelve';
            words[13] = 'Thirteen';
            words[14] = 'Fourteen';
            words[15] = 'Fifteen';
            words[16] = 'Sixteen';
            words[17] = 'Seventeen';
            words[18] = 'Eighteen';
            words[19] = 'Nineteen';
            words[20] = 'Twenty';
            words[30] = 'Thirty';
            words[40] = 'Forty';
            words[50] = 'Fifty';
            words[60] = 'Sixty';
            words[70] = 'Seventy';
            words[80] = 'Eighty';
            words[90] = 'Ninety';
            atotalc = atotalc.toString();
            var atemp = atotalc.split(".");
            var number = atemp[0].split(",").join("");
            var n_length = number.length;
            var words_string = "";
            if (n_length <= 9) {
                var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
                var received_n_array = new Array();
                for (var i = 0; i < n_length; i++) {
                    received_n_array[i] = number.substr(i, 1);
                }
                for (var i = 9 - n_length, j = 0; i < 9; i++ , j++) {
                    n_array[i] = received_n_array[j];
                }
                for (var i = 0, j = 1; i < 9; i++ , j++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        if (n_array[i] == 1) {
                            n_array[j] = 10 + parseInt(n_array[j]);
                            n_array[i] = 0;
                        }
                    }
                }
                atotalc = "";
                for (var i = 0; i < 9; i++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        atotalc = n_array[i] * 10;
                    } else {
                        atotalc = n_array[i];
                    }
                    if (atotalc != 0) {
                        words_string += words[atotalc] + " ";
                    }
                    if ((i == 1 && atotalc != 0) || (i == 0 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Crores ";
                    }
                    if ((i == 3 && atotalc != 0) || (i == 2 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Lakhs ";
                    }
                    if ((i == 5 && atotalc != 0) || (i == 4 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Thousand ";
                    }
                    if (i == 6 && atotalc != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                        words_string += "Hundred and ";
                    } else if (i == 6 && atotalc != 0) {
                        words_string += "Hundred ";
                    }
                }
                words_string = words_string.split("  ").join(" ");
            }
            return words_string;
        }

        $scope.amountinwords = function convertNumberToWords(atotalc) {
            var words = new Array();
            words[0] = '';
            words[1] = 'One';
            words[2] = 'Two';
            words[3] = 'Three';
            words[4] = 'Four';
            words[5] = 'Five';
            words[6] = 'Six';
            words[7] = 'Seven';
            words[8] = 'Eight';
            words[9] = 'Nine';
            words[10] = 'Ten';
            words[11] = 'Eleven';
            words[12] = 'Twelve';
            words[13] = 'Thirteen';
            words[14] = 'Fourteen';
            words[15] = 'Fifteen';
            words[16] = 'Sixteen';
            words[17] = 'Seventeen';
            words[18] = 'Eighteen';
            words[19] = 'Nineteen';
            words[20] = 'Twenty';
            words[30] = 'Thirty';
            words[40] = 'Forty';
            words[50] = 'Fifty';
            words[60] = 'Sixty';
            words[70] = 'Seventy';
            words[80] = 'Eighty';
            words[90] = 'Ninety';
            atotalc = atotalc.toString();
            var atemp = atotalc.split(".");
            var number = atemp[0].split(",").join("");
            var n_length = number.length;
            var words_string = "";
            if (n_length <= 9) {
                var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
                var received_n_array = new Array();
                for (var i = 0; i < n_length; i++) {
                    received_n_array[i] = number.substr(i, 1);
                }
                for (var i = 9 - n_length, j = 0; i < 9; i++ , j++) {
                    n_array[i] = received_n_array[j];
                }
                for (var i = 0, j = 1; i < 9; i++ , j++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        if (n_array[i] == 1) {
                            n_array[j] = 10 + parseInt(n_array[j]);
                            n_array[i] = 0;
                        }
                    }
                }
                atotalc = "";
                for (var i = 0; i < 9; i++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        atotalc = n_array[i] * 10;
                    } else {
                        atotalc = n_array[i];
                    }
                    if (atotalc != 0) {
                        words_string += words[atotalc] + " ";
                    }
                    if ((i == 1 && atotalc != 0) || (i == 0 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Crores ";
                    }
                    if ((i == 3 && atotalc != 0) || (i == 2 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Lakhs ";
                    }
                    if ((i == 5 && atotalc != 0) || (i == 4 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Thousand ";
                    }
                    if (i == 6 && atotalc != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                        words_string += "Hundred and ";
                    } else if (i == 6 && atotalc != 0) {
                        words_string += "Hundred ";
                    }
                }
                words_string = words_string.split("  ").join(" ");
            }
            return words_string;
        }

        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printTenant").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/TenantAgreement_css.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.TenantTabFlag = false;
        $scope.tabChanged = function () {

            $scope.TenantTabFlag = true;
        }




        
        $scope.Edit = function (obj) {

            var data = {
                "TENANTAGR_Id": obj.tenantagR_Id,

            };

            apiService.create("TenantAgreement/Edit", data).then(function (promise) {
                if (promise !== null) {
                    $('#blah').removeAttr('src');
                    $scope.geteditdetails = promise.geteditdetails;
                    $scope.editable = true;
                    $scope.editflag = true;
                    $scope.TENANTAGR_Id = promise.geteditdetails[0].tenantagR_Id;
                    $scope.TENANTAGR_TenantName = promise.geteditdetails[0].tenantagR_TenantName;
                    $scope.TENANTAGR_Address = promise.geteditdetails[0].tenantagR_Address;

                    $scope.HRMR_Id = promise.geteditdetails[0].hrmR_Id;
                    $scope.HRMF_Id = promise.geteditdetails[0].hrmF_Id;
                    $scope.TENANTAGR_ContactNo = promise.geteditdetails[0].tenantagR_ContactNo;
                    $scope.TENANTAGR_AgrFromDate = new Date(promise.geteditdetails[0].tenantagR_AgrFromDate);
                    $scope.obj.TENANTAGR_AgrToDate = new Date(promise.geteditdetails[0].tenantagR_AgrToDate);

                    $scope.plMaxdate = new Date(promise.geteditdetails[0].tenantagR_AgrToDate);
                    $scope.plMaxdate.setDate($scope.plMaxdate.getDate());

                    $scope.TENANTAGR_AdvanceAmount = promise.geteditdetails[0].tenantagR_AdvanceAmount;

                    $scope.TENANTAGR_Remarks = promise.geteditdetails[0].tenantagR_Remarks;
                    $scope.TENANTAGR_NoOfOccupants = promise.geteditdetails[0].tenantagR_NoOfOccupants;
                    $scope.TENANTAGR_EmailId = promise.geteditdetails[0].tenantagR_EmailId;
                    $scope.TENANTAGR_PANCardNo = promise.geteditdetails[0].tenantagR_PANCardNo;
                    $scope.TENANTAGR_AgreementDuration = promise.geteditdetails[0].tenantagR_AgreementDuration;
                    $scope.TENANTAGR_RenewalPercentage = promise.geteditdetails[0].tenantagR_RenewalPercentage;
                    $scope.TENANTAGR_MonthlyRent = promise.geteditdetails[0].tenantagR_MonthlyRent;
                    $scope.HRMB_Id = promise.geteditdetails[0].hrmB_Id;
                    $scope.PAMS_Id = promise.geteditdetails[0].pamS_Id;
                    $scope.TENANTAGR_RentDueDay = promise.geteditdetails[0].tenantagR_RentDueDay;
                    $scope.TENANTAGR_LeftRemarks = promise.geteditdetails[0].tenantagR_LeftRemarks;
                    $scope.TENANTAGR_LeftFlg = promise.geteditdetails[0].tenantagR_LeftFlg;
                    $scope.obj.TENANTAGR_Imagepath = promise.tenantagrfL_FileName;
                    $scope.TENANTAGR_Aadhar = promise.geteditdetails[0].tenantagR_Aadhar;
                    if (promise.tenantagrfL_FileName != null && promise.tenantagrfL_FileName != "") {
                        $('#blah').attr('src', promise.tenantagrfL_FileName);
                    }

                    if (promise.geteditfilesdetails !== null && promise.geteditfilesdetails.length > 0) {
                        $scope.teacherdocuupload = promise.geteditfilesdetails;
                        angular.forEach($scope.teacherdocuupload, function (dd) {
                            if (dd.tenantagrfL_FileLocation !== null && dd.tenantagrfL_FileLocation !== "") {
                                var img = dd.tenantagrfL_FileLocation;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("dd.filetype : " + dd.filetype);
                                dd.TENANTAGRFL_FileLocation = dd.tenantagrfL_FileLocation;
                                dd.TENANTAGRFL_FileName = dd.tenantagrfL_FileName;
                                dd.TENANTAGRFL_Remarks = dd.tenantagrfL_Remarks;
                                if (lastelement === 'pdf' || lastelement === 'PDF' || lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.tenantagrfL_FileLocation;
                                    //dd.document_Pathnew =  dd.tenantagrfL_FileLocation;
                                }
                            }
                        });
                    }
                    $scope.roomchange();
                    $scope.teacherdocuupload;
                }
            });
        };

        $scope.deactive = function (item, SweetAlert) {
            $scope.HRMRAM_Id = item.hrmraM_Id;
            var dystring = "";
            if (item.HRMRAM_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.HRMRAM_ActiveFlag == false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("RoomWiseAmenitiesMapping/deactive", item).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "deleted Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $scope.roomchange();
                            })
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        }



        $scope.delete = function (obj) {

            var mgs = "Delete";

            var data = {
                "TENANTAGR_Id": obj.tenantagR_Id,

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
                        apiService.create("TenantAgreement/delete", data).then(function (promise) {
                            if (promise.message == "Deleted") {
                                swal("Record Deleted Sucessfully");
                                $state.reload();
                            }
                            else if (promise.message == "Not Deleted") {
                                swal("Record Not Deleted");
                                $state.reload();
                            }
                            else {
                                swal("Request Failed");
                                $state.reload();
                            }

                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };


        $scope.save = function () {

            $scope.submitted2 = true;
      //      if ($scope.myForm2.$valid) {
                $scope.TENANTAGR_AgreementDuration = $scope.obj.tenantagrreN_PreAgrToDate.getMonth() - $scope.obj.tenantagrreN_PreAgrFromDate.getMonth() + (12 * ($scope.obj.tenantagrreN_PreAgrToDate.getFullYear() - $scope.obj.tenantagrreN_PreAgrFromDate.getFullYear()))

                var data = {

                    "TENANTAGRREN_Id": $scope.obj.tenantagrreN_Id,
                    "TENANTAGR_Id": $scope.obj.TENANTAGRId,
                    "TENANTAGRREN_PreAgrFromDate": new Date($scope.obj.tenantagrreN_PreAgrFromDate).toDateString(),
                    "TENANTAGRREN_PreAgrToDate": new Date($scope.obj.tenantagrreN_PreAgrToDate).toDateString(),

                    "TENANTAGRREN_Remarks": $scope.obj.TENANTAGRREN_Remarks,
                    "TENANTAGR_AgrFromDate": new Date($scope.obj.tenantagrreN_PreAgrFromDate).toDateString(),
                    "TENANTAGR_AgrToDate": new Date($scope.obj.tenantagrreN_PreAgrToDate).toDateString(),
                  

                    "TENANTAGR_AgreementDuration": $scope.TENANTAGR_AgreementDuration,

                    "TENANTAGRREN_AggreedEnhancement": $scope.TENANTAGR_RenewalPercentage,
                    "TENANTAGRREN_AmtOrPercentFlg": $scope.AmountPercentage,
                    "TENANTAGRREN_Rent": $scope.obj.tenantagR_MonthlyRent

                };

            apiService.create("TenantAgreement/saverenewal", data).then(function (promise) {
                    debugger;
                    if (promise.message === 'Duplicate') {
                        swal('Record Already Exist');
                    }
                    else if (promise.message === 'ex') {
                        swal('Exceeds the number of Questions');
                    }
                    else {
                        if (promise.message === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");

                            } else {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.message === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else {
                            swal("Failed To Save/Update Record");
                        }
                    }
                    $scope.tenantrenewal = promise.tenantrenewal;
                    $scope.cleartabl2();

                    // $state.reload();
                });
            //}
            //else {
            //    $scope.submitted1 = true;
            //}
        };

        $scope.saverefundadvance = function () {

            $scope.submitted3 = true;

            var data = {

                "TENANTAGR_Id": $scope.tenantagR_Id.tenantagR_Id,
                "TENANTAGR_RefundedAmount": $scope.Refundable_Advance,
                "TENANTAGR_RefundModeOfPayment": $scope.paymentmode
                
            };

            apiService.create("TenantAgreement/saverefundadvance", data).then(function (promise) {
                debugger;
                if (promise.message === 'Duplicate') {
                    swal('Record Already Exist');
                }
                else if (promise.message === 'ex') {
                    swal('Exceeds the number of Questions');
                }
                else {
                    if (promise.message === "Add") {
                        if (promise.returnval === true) {
                           
                            swal("Record Saved Successfully");
                            $scope.loaddata();

                        } else {
                            swal("Failed To Save Record");
                        }
                    } else if (promise.message === "Update") {
                        if (promise.returnval === true) {
                            swal("Record Updated Successfully");
                            $scope.loaddata();
                        } else {
                            swal("Failed To Update Record");
                        }
                    } else {
                        swal("Failed To Save/Update Record");
                    }
                }
                //$scope.tenantrenewal = promise.tenantrenewal;

                $scope.cleartabl3();

                // $state.reload();
            });
            
        };


        $scope.tenantchange = function () {

            var data = {

                "TENANTAGR_Id": $scope.obj.TENANTAGRId,

            };

            apiService.create("TenantAgreement/tenantchange", data).then(function (promise) {
                if (promise.tenantchange.length != null) {
                    $scope.obj.tenantagrreN_PreAgrFromDate = new Date(promise.tenantchange[0].tenantagR_AgrFromDate);
                    $scope.obj.tenantagrreN_PreAgrToDate = new Date(promise.tenantchange[0].tenantagR_AgrToDate);

                    $scope.obj.tenantagrreN_AgrFromDate = new Date(promise.tenantchange[0].tenantagR_AgrFromDate);
                    $scope.obj.tenantagrreN_AgrToDate = new Date(promise.tenantchange[0].tenantagR_AgrToDate);
                    $scope.obj.tenantagR_MonthlyRent = promise.tenantchange[0].tenantagR_MonthlyRent;
                    $scope.MonthlyRent = promise.tenantchange[0].tenantagR_MonthlyRent;
                    $scope.MonthlyRentAmount = promise.tenantchange[0].tenantagR_MonthlyRent;
                }
            });
        };

        
        $scope.amountcaluclation = function ()
        {
            if ($scope.AmountPercentage == "Amount") {
                
                $scope.obj.tenantagR_MonthlyRent = $scope.MonthlyRent + Number($scope.TENANTAGR_RenewalPercentage);

            }
            else {
                $scope.obj.tenantagR_MonthlyRent = $scope.MonthlyRent + ($scope.MonthlyRent * $scope.TENANTAGR_RenewalPercentage) / 100;
                
            }

        };
        
        $scope.amountorpercentagechange = function () {
            if ($scope.AmountPercentage == "Amount") {
                $scope.TENANTAGR_RenewalPercentage = null;
                $scope.obj.tenantagR_MonthlyRent = $scope.MonthlyRentAmount;
            }
            else {
                $scope.TENANTAGR_RenewalPercentage = null;
                $scope.obj.tenantagR_MonthlyRent = $scope.MonthlyRentAmount;
            }
        };


        $scope.Editrenewal = function (obj) {

            var data = {
                "TENANTAGRREN_Id": obj.tenantagrreN_Id,
                "TENANTAGR_Id": obj.tenantagR_Id
            };

            apiService.create("TenantAgreement/Editrenewal", data).then(function (promise) {
                if (promise !== null) {

                    $scope.obj.tenantagrreN_Id = promise.edittenantrenewal[0].tenantagrreN_Id;
                    $scope.obj.TENANTAGRId = promise.edittenantrenewal[0].tenantagR_Id;
                    $scope.obj.TENANTAGRREN_Remarks = promise.edittenantrenewal[0].tenantagrreN_Remarks;
                    
                    $scope.obj.tenantagrreN_PreAgrFromDate = new Date(promise.edittenantrenewal[0].tenantagrreN_PreAgrFromDate);
                    $scope.obj.tenantagrreN_PreAgrToDate = new Date(promise.edittenantrenewal[0].tenantagrreN_PreAgrToDate);

                    $scope.obj.tenantagrreNAgrFromDate = new Date(promise.edittenantrenewal[0].tenantagrreN_PreAgrFromDate);
                    $scope.obj.tenantagrreNAgrToDate = new Date(promise.edittenantrenewal[0].tenantagrreN_PreAgrToDate);
                    $scope.AmountPercentage = promise.edittenantrenewal[0].tenantagrreN_AmtOrPercentFlg;
                    $scope.TENANTAGR_RenewalPercentage = promise.edittenantrenewal[0].tenantagrreN_AggreedEnhancement;
                    $scope.obj.tenantagR_MonthlyRent = promise.edittenantrenewal[0].tenantagrreN_Rent;

                }
            });
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

        $scope.deleterenewal = function (obj) {



            var mgs = "Delete";



            var data = {
                "TENANTAGRREN_Id": obj.tenantagrreN_Id,
                "TENANTAGR_Id": obj.tenantagR_Id

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
                        apiService.create("TenantAgreement/deleterenewal", data).then(function (promise) {
                            if (promise.message == "Deleted") {
                                swal("Record Deleted Sucessfully");
                                $state.reload();
                            }
                            else if (promise.message == "Not Deleted") {
                                swal("Record Not Deleted");
                                $state.reload();
                            }
                            else {
                                swal("Request Failed");
                                $state.reload();
                            }

                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.cleartabl1 = function () {
            $state.reload();
        }

        $scope.cleartabl2 = function () {

            $scope.obj.TENANTAGRREN_Id = "";
            $scope.obj.TENANTAGRId = "";
            $scope.obj.tenantagrreN_PreAgrFromDate = "";
            $scope.obj.tenantagrreN_PreAgrToDate = "";
            $scope.obj.TENANTAGRREN_Remarks = "";
            $scope.obj.tenantagR_MonthlyRent = "";
            $scope.TENANTAGR_RenewalPercentage = "";
            $scope.AmountPercentage = "";
           
        }

        $scope.cleartabl3 = function () {
            $scope.tenantagR_Id = "";
            $scope.TENANTAGR_AdvanceAmount = "";
            $scope.Refundable_Advance = "";
            $scope.paymentmode = "";
            $scope.amenitiescharges = [];
        }

        $scope.SaveMasterQuestionDetails = function () {

            $scope.submitted1 = true;

            if ($scope.TENANTAGR_LeftRemarks == undefined) {
                $scope.TENANTAGR_LeftRemarks = "";
            }

            if ($scope.myForm.$valid) {

                var data = {
                    "TENANTAGR_Id": $scope.TENANTAGR_Id,
                    "TENANTAGR_TenantName": $scope.TENANTAGR_TenantName,
                    "TENANTAGR_Address": $scope.TENANTAGR_Address,
                    "HRMR_Id": $scope.HRMR_Id,
                    "TENANTAGR_ContactNo": $scope.TENANTAGR_ContactNo,
                    "TENANTAGR_AdvanceAmount": $scope.TENANTAGR_AdvanceAmount,
                    "TENANTAGR_AgrFromDate": new Date($scope.TENANTAGR_AgrFromDate).toDateString(),
                    "TENANTAGR_AgrToDate": new Date($scope.obj.TENANTAGR_AgrToDate).toDateString(),
                    "TENANTAGR_RentDueDay": $scope.TENANTAGR_RentDueDay,
                    "TENANTAGR_Remarks": $scope.TENANTAGR_Remarks,
                    "TENANTAGR_NoOfOccupants": $scope.TENANTAGR_NoOfOccupants,
                    "HRMB_Id": $scope.HRMB_Id,
                    "TENANTAGR_EmailId": $scope.TENANTAGR_EmailId,
                    "TENANTAGR_PANCardNo": $scope.TENANTAGR_PANCardNo,
                    "TENANTAGR_AgreementDuration": $scope.TENANTAGR_AgreementDuration,
                    "TENANTAGR_RenewalPercentage": $scope.TENANTAGR_RenewalPercentage,
                    "TENANTAGR_MonthlyRent": $scope.TENANTAGR_MonthlyRent,
                    "tempfilesDTO": $scope.teacherdocuupload,
                    "TENANTAGR_LeftRemarks": $scope.TENANTAGR_LeftRemarks,
                    "TENANTAGR_LeftFlg": $scope.TENANTAGR_LeftFlg,
                    "TENANTAGR_Aadhar": $scope.TENANTAGR_Aadhar,
                    "PAMS_Id": $scope.PAMS_Id,
                    "TENANTAGR_Imagepath": $scope.TENANTAGR_Imagepath
                };

                apiService.create("TenantAgreement/SaveMasterType", data).then(function (promise) {
                    var fromdate = new Date($scope.TENANTAGR_AgrFromDate).toDateString();
                    var todate = new Date($scope.TENANTAGR_AgrToDate).toDateString();
                    if (promise.returnmsg == null || promise.returnmsg == '') {
                        if (promise.message === 'Duplicate') {
                            swal('Record Already Exist');
                        }
                        else if (promise.message === 'ex') {
                            swal('Exceeds the number of Questions');
                        }
                        else {
                            if (promise.message === "Add") {
                                if (promise.returnval === true) {
                                    swal("Record Saved Successfully");
                                } else {
                                    swal("Failed To Save Record");
                                }
                            } else if (promise.message === "Update") {
                                if (promise.returnval === true) {
                                    swal("Record Updated Successfully");
                                } else {
                                    swal("Failed To Update Record");
                                }
                            } else {
                                swal("Failed To Save/Update Record");
                            }
                        }
                        $scope.cleartabl1();
                        angular.forEach(promise.getloaddata, function (dd) {
                            if (dd.tenantagR_LeftFlg == true) {
                                $scope.getloaddata.push({
                                    tenantagR_Id: dd.tenantagR_Id,
                                    tenantagR_TenantName: dd.tenantagR_TenantName,
                                    tenantagR_NoOfOccupants: dd.tenantagR_NoOfOccupants,
                                    hrmB_BuildingName: dd.hrmB_BuildingName,
                                    hrmR_RoomName: dd.hrmR_RoomName,
                                    tenantagR_PANCardNo: dd.tenantagR_PANCardNo,
                                    tenantagR_Aadhar: dd.tenantagR_Aadhar,
                                    tenantagR_ContactNo: dd.tenantagR_ContactNo,
                                    tenantagR_EmailId: dd.tenantagR_EmailId,
                                    tenantagR_AdvanceAmount: dd.tenantagR_AdvanceAmount,
                                    tenantagR_MonthlyRent: dd.tenantagR_MonthlyRent,
                                    tenantagR_AgrFromDate: dd.tenantagR_AgrFromDate,
                                    tenantagR_AgrToDate: dd.tenantagR_AgrToDate,
                                    tenantagR_Remarks: dd.tenantagR_Remarks,
                                    cls: 'backkk',
                                    tenantagR_RentDueDay: dd.tenantagR_RentDueDay,
                                    tenantagR_UserId: dd.tenantagR_UserId

                                });
                            }
                            else {
                                $scope.getloaddata.push({
                                    tenantagR_Id: dd.tenantagR_Id,
                                    tenantagR_TenantName: dd.tenantagR_TenantName,
                                    tenantagR_NoOfOccupants: dd.tenantagR_NoOfOccupants,
                                    hrmB_BuildingName: dd.hrmB_BuildingName,
                                    hrmR_RoomName: dd.hrmR_RoomName,
                                    tenantagR_PANCardNo: dd.tenantagR_PANCardNo,
                                    tenantagR_Aadhar: dd.tenantagR_Aadhar,
                                    tenantagR_ContactNo: dd.tenantagR_ContactNo,
                                    tenantagR_EmailId: dd.tenantagR_EmailId,
                                    tenantagR_AdvanceAmount: dd.tenantagR_AdvanceAmount,
                                    tenantagR_MonthlyRent: dd.tenantagR_MonthlyRent,
                                    tenantagR_AgrFromDate: dd.tenantagR_AgrFromDate,
                                    tenantagR_AgrToDate: dd.tenantagR_AgrToDate,
                                    tenantagR_Remarks: dd.tenantagR_Remarks,
                                    tenantagR_RentDueDay: dd.tenantagR_RentDueDay,
                                    tenantagR_UserId: dd.tenantagR_UserId

                                });
                            }
                        });
                    }
                    else {
                        swal(promise.returnmsg + " is in the Selected Flat , Agreement " + fromdate + " to " + todate)
                    }
                   
                 

                    // $state.reload();
                });
            }
            else {
                $scope.submitted1 = true;
            }

          
        };


        $scope.UploadEmployeeProfilePic = [];

        $scope.uploadEmployeeProfilePic = function (input, document) {
            $scope.UploadEmployeeProfilePic = input.files;
            $('#blah').removeAttr('src');

            if (input.files && input.files[0]) {

                if ((input.files[0].type == "image/jpeg" || input.files[0].type == "image/png")  && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {

                    var reader = new FileReader();

                    reader.onload = function (e) {

                        $('#blah')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeprofile();

                }
                else if (input.files[0].type != "image/jpeg" && input.files[0].type == "image/png" && input.files[0].type == "image/PNG" ) {
                    swal("Please Upload the JPEG file");
                    angular.element("input[type='file']").val(null);
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    angular.element("input[type='file']").val(null);
                    return;
                }

            }
        }

        function UploadEmployeeprofile() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadEmployeeProfilePic.length; i++) {
                formData.append("File", $scope.UploadEmployeeProfilePic[i]);
            }

            //We can send more data to server using append         
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/EmployeeDocumentUpload/UploadEmployeeprofilepic", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    if (d != "PathNotFound") {
                        $scope.TENANTAGR_Imagepath = d;
                    } else {
                        swal('File Storage Path Not Found ..!!');
                        angular.element("input[type='file']").val(null);
                    }

                })
                .error(function () {
                    $('#blah').removeAttr('src');
                    defer.reject("File Upload Failed!");
                    angular.element("input[type='file']").val(null);
                });
        }




        $scope.CreateUser = function (obj) {

            var mgs = "Create User";

            var data = {
                "TENANTAGR_Id": obj.tenantagR_Id,

            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To " + mgs ,
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("TenantAgreement/CreateUser", data).then(function (promise) {
                            if (promise.message == "Duplicate") {
                                swal("User Already Exists");
                                $state.reload();
                            }
                            else if (promise.message == "Add") {
                                swal("Record Saved Sucessfully");
                                $state.reload();
                            }
                            else if (promise.message == "False") {
                                swal("Record Not Saved Sucessfully");
                                $state.reload();
                            }
                            else {
                                swal("Request Failed");
                                $state.reload();
                            }

                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };


        ///Zip File  download

        (function (global, factory) {
            if (typeof define === "function" && define.amd) {
                define([], factory);
            } else if (typeof exports !== "undefined") {
                factory();
            } else {
                var mod = {
                    exports: {}
                };
                factory();
                global.FileSaver = mod.exports;
            }
        })(this, function () {
            "use strict";

            var _global = typeof window === 'object' && window.window === window ? window : typeof self === 'object' && self.self === self ? self : typeof global === 'object' && global.global === global ? global : void 0;

            function bom(blob, opts) {
                if (typeof opts === 'undefined') opts = {
                    autoBom: false
                }; else if (typeof opts !== 'object') {
                    console.warn('Deprecated: Expected third argument to be a object');
                    opts = {
                        autoBom: !opts
                    };
                }

                if (opts.autoBom && /^\s*(?:text\/\S*|application\/xml|\S*\/\S*\+xml)\s*;.*charset\s*=\s*utf-8/i.test(blob.type)) {
                    return new Blob([String.fromCharCode(0xFEFF), blob], {
                        type: blob.type
                    });
                }

                return blob;
            }

            function download(url, name, opts) {
                var xhr = new XMLHttpRequest();
                xhr.open('GET', url);
                xhr.responseType = 'blob';

                xhr.onload = function () {
                    saveAs(xhr.response, name, opts);
                };

                xhr.onerror = function () {
                    console.error('could not download file');
                };

                xhr.send();
            }

            function corsEnabled(url) {
                var xhr = new XMLHttpRequest();
                xhr.open('HEAD', url, false);

                try {
                    xhr.send();
                } catch (e) { }

                return xhr.status >= 200 && xhr.status <= 299;
            }


            function click(node) {
                try {
                    node.dispatchEvent(new MouseEvent('click'));
                } catch (e) {
                    var evt = document.createEvent('MouseEvents');
                    evt.initMouseEvent('click', true, true, window, 0, 0, 0, 80, 20, false, false, false, false, 0, null);
                    node.dispatchEvent(evt);
                }
            }

            var saveAs = _global.saveAs || ( // probably in some web worker
                typeof window !== 'object' || window !== _global ? function saveAs() { }
                    /* noop */
                    // Use download attribute first if possible (#193 Lumia mobile)
                    : 'download' in HTMLAnchorElement.prototype ? function saveAs(blob, name, opts) {
                        var URL = _global.URL || _global.webkitURL;
                        var a = document.createElement('a');
                        name = name || blob.name || 'download';
                        a.download = name;
                        a.rel = 'noopener';

                        if (typeof blob === 'string') {

                            a.href = blob;

                            if (a.origin !== location.origin) {
                                corsEnabled(a.href) ? download(blob, name, opts) : click(a, a.target = '_blank');
                            } else {
                                click(a);
                            }
                        } else {
                            // Support blobs
                            a.href = URL.createObjectURL(blob);
                            setTimeout(function () {
                                URL.revokeObjectURL(a.href);
                            }, 4E4); // 40s

                            setTimeout(function () {
                                click(a);
                            }, 0);
                        }
                    } // Use msSaveOrOpenBlob as a second approach
                        : 'msSaveOrOpenBlob' in navigator ? function saveAs(blob, name, opts) {
                            name = name || blob.name || 'download';

                            if (typeof blob === 'string') {
                                if (corsEnabled(blob)) {
                                    download(blob, name, opts);
                                } else {
                                    var a = document.createElement('a');
                                    a.href = blob;
                                    a.target = '_blank';
                                    setTimeout(function () {
                                        click(a);
                                    });
                                }
                            } else {
                                navigator.msSaveOrOpenBlob(bom(blob, opts), name);
                            }
                        } // Fallback to using FileReader and a popup
                            : function saveAs(blob, name, opts, popup) {

                                popup = popup || open('', '_blank');

                                if (popup) {
                                    popup.document.title = popup.document.body.innerText = 'downloading...';
                                }

                                if (typeof blob === 'string') return download(blob, name, opts);
                                var force = blob.type === 'application/octet-stream';

                                var isSafari = /constructor/i.test(_global.HTMLElement) || _global.safari;

                                var isChromeIOS = /CriOS\/[\d]+/.test(navigator.userAgent);

                                if ((isChromeIOS || force && isSafari) && typeof FileReader !== 'undefined') {
                                    // Safari doesn't allow downloading of blob URLs
                                    var reader = new FileReader();

                                    reader.onloadend = function () {
                                        var url = reader.result;
                                        url = isChromeIOS ? url : url.replace(/^data:[^;]*;/, 'data:attachment/file;');
                                        if (popup) popup.location.href = url; else location = url;
                                        popup = null; // reverse-tabnabbing #460
                                    };

                                    reader.readAsDataURL(blob);
                                } else {
                                    var URL = _global.URL || _global.webkitURL;
                                    var url = URL.createObjectURL(blob);
                                    if (popup) popup.location = url; else location.href = url;
                                    popup = null; // reverse-tabnabbing #460

                                    setTimeout(function () {
                                        URL.revokeObjectURL(url);
                                    }, 4E4); // 40s
                                }
                            });
            _global.saveAs = saveAs.saveAs = saveAs;

            if (typeof module !== 'undefined') {
                module.exports = saveAs;
            }
        });

        var JSZipUtils = {};

        JSZipUtils._getBinaryFromXHR = function (xhr) {
            // for xhr.responseText, the 0xFF mask is applied by JSZip
            return xhr.response || xhr.responseText;
        };

        // taken from jQuery
        function createStandardXHR() {
            try {
                return new window.XMLHttpRequest();
            }
            catch (e) { }
        }

        function createActiveXHR() {
            try {
                return new window.ActiveXObject("Microsoft.XMLHTTP");
            }
            catch (e) { }
        }

        // Create the request object
        var createXHR = window.ActiveXObject ?

            function () {
                return createStandardXHR() || createActiveXHR();
            } :
            // For all other browsers, use the standard XMLHttpRequest object
            createStandardXHR;



        JSZipUtils.getBinaryContent = function (path, callback, progress) {

            try {

                var xhr = createXHR();

                xhr.open('GET', path, true);

                // recent browsers
                if ("responseType" in xhr) {
                    xhr.responseType = "arraybuffer";
                }

                // older browser
                if (xhr.overrideMimeType) {
                    xhr.overrideMimeType("text/plain; charset=x-user-defined");
                }

                xhr.onreadystatechange = function (evt) {
                    var file, err;
                    // use `xhr` and not `this`... thanks IE
                    if (xhr.readyState === 4) {
                        if (xhr.status === 200 || xhr.status === 0) {
                            file = null;
                            err = null;
                            try {
                                file = JSZipUtils._getBinaryFromXHR(xhr);
                            }
                            catch (e) {
                                err = new Error(e);
                            }
                            callback(err, file);
                        }
                        else {
                            callback(new Error("Ajax error for " + path + " : " + this.status + " " + this.statusText), null);
                        }
                    }
                };
                if (progress) xhr.onprogress = progress;
                xhr.send();


                return xhr;

            }
            catch (e) {
                callback(new Error(e), null);
            }
        };


        $scope.downloadzipfile = function (getviewfilesdetails) {

            var urls = [];


            for (var n = 0; n < getviewfilesdetails.length; n++) {
                urls.push(getviewfilesdetails[n].tenantagrfL_FileLocation);

            }


            var filename = "TenantAgreementFiles";

            compressed_img(urls, filename);
        }

        function compressed_img(urls, filename) {
            var zip = new JSZip();
            var count = 0;
            var name = filename + ".zip";
            urls.forEach(function (url, i) {

                JSZipUtils.getBinaryContent(url, function (err, data) {
                    if (err) {
                        throw err;
                    }
                    //var fname = "file_"+i+".jpg";
                    var img = url;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];

                    var fname = "file_" + i + "." + lastelement;
                    zip.file(fname, data, {
                        binary: true
                    });
                    count++;
                    if (count == urls.length) {
                        zip.generateAsync({
                            type: 'blob'
                        }).then(function (content) {
                            saveAs(content, name);
                        });
                    }
                }, function (e) {
                    p[e.target.responseURL] = e;
                    var total = 0, prog = 0;

                });
            });
        }

        var p = {};



        
    }


})();