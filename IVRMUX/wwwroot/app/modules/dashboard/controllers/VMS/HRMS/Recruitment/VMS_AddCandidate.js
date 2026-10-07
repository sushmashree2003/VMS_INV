(function () {
    'use strict';
    angular
        .module('app')
        .controller('vmsAddCandidateController', vmsAddCandidateController);

    vmsAddCandidateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$timeout','$stateParams', '$filter', '$q', '$sce'];
    function vmsAddCandidateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $timeout, $stateParams, $filter, $q, $sce) {

        $scope.EarningDet = {};
        $scope.addjob = {};
        $scope.addjob.hrjD_Id = 0;
        $scope.hrcD_Resume = "";

        $scope.Qualification = true;
        $scope.Experiance = true;
        $scope.Family = true;
        $scope.Language = true;

        $scope.Candidate = {};
        $scope.Candidate.hrcD_Id = 0;
        $scope.Candidate.mI_Id = 0;

        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("AddCandidateVMS/getalldetails", pageid).then(function (promise) {
             //   $('#termsandconditionagree').modal('show');
                $scope.candidateflage = promise.candidateflage;
                if (promise.masterPosTypeList !== null && promise.masterPosTypeList.length > 0) {
                    $scope.masterPosTypeList = promise.masterPosTypeList;
                }

                if (promise.masterPositionList !== null && promise.masterPositionList.length > 0) {
                    $scope.masterPositionList = promise.masterPositionList;
                }

                if (promise.masterQualification !== null && promise.masterQualification.length > 0) {
                    $scope.masterQualification = promise.masterQualification;
                }

                if (promise.masterGender !== null && promise.masterGender.length > 0) {
                    $scope.masterGender = promise.masterGender;
                }

                if (promise.masterjob !== null && promise.masterjob.length > 0) {
                    $scope.masterjob = promise.masterjob;
                }

                if (promise.mastercountry !== null && promise.mastercountry.length > 0) {
                    $scope.mastercountry = promise.mastercountry;
                }

                if (promise.masterReligion !== null && promise.masterReligion.length > 0) {
                    $scope.masterReligion = promise.masterReligion;
                }

                if (promise.masterCaste !== null && promise.masterCaste.length > 0) {
                    $scope.masterCaste = promise.masterCaste;
                }

                if (promise.mastermaritalstatus !== null && promise.mastermaritalstatus.length > 0) {
                    $scope.mastermaritalstatus = promise.mastermaritalstatus;
                }
                if (promise.candidatedetailsgrid !== null && promise.candidatedetailsgrid.length > 0) {
                    $scope.candidatedetailsgrid = promise.candidatedetailsgrid;
                }
                if (promise.cousultencydetail !== null && promise.cousultencydetail.length > 0) {
                    $scope.cousultencydetail = promise.cousultencydetail;
                }

            });
            $scope.frmdate = new Date();
            $scope.todate = $scope.Employee.frmdate;
            $scope.minDateTo = new Date(
                $scope.todate.getFullYear(),
                $scope.todate.getMonth(),
                $scope.todate.getDate());
        };

        $scope.FormatType = "Format1";

        //setToDate
        $scope.setToDate = function (frmdate) {

            $scope.todate = frmdate;
            $scope.minDateTo = new Date(
                $scope.todate.getFullYear(),
                $scope.todate.getMonth(),
                $scope.todate.getDate());
            $scope.todate = "";
            if ($scope.griddeatails) {
                $scope.griddeatails = false;
            }
        }
        $scope.OnchageToDate = function () {

            if ($scope.griddeatails) {
                $scope.griddeatails = false;
            }
        }
        // clear form data
        $scope.cancel = function () {
            $state.reload();
        };

        $scope.getaddress = function () {
            if ($scope.mrfCand.copyaddress == true) {
                $scope.mrfCand.hrcD_AddressPermanent = $scope.mrfCand.hrcD_AddressLocal;
                $scope.mrfCand.hrcD_AddressPermanent2 = $scope.mrfCand.hrcD_AddressLocal2;
                $scope.mrfCand.hrcD_AddPermanentPlace = $scope.mrfCand.hrcD_AddLocalPlace;
                $scope.mrfCand.hrcD_AddPermanentPIN = $scope.mrfCand.hrcD_AddLocalPIN;
            }
        };

        $scope.submitted = false;
        $scope.submitted3 = false;
        $scope.submitted4 = false;
        $scope.submitted2 = false;
        $scope.submitted5 = false;

        $scope.interacted = function (field) {
            return $scope.submitted && field.$invalid;
        };
        $scope.interacted2 = function (field) {
            return $scope.submitted2 || field.$dirty;
        };
        $scope.interacted3 = function (field) {
            return $scope.submitted3 || field.$dirty;
        };
        $scope.interacted4 = function (field) {
            return $scope.submitted4 || field.$dirty;
        };
        $scope.interacted5 = function (field) {
            return $scope.submitted5 || field.$dirty;
        };

        $scope.qualificationDetails = [{ id: 'qualification' }];
        $scope.addNewQualification = function () {
            var newItemNo = $scope.qualificationDetails.length + 1;
            if (newItemNo <= 10) {
                $scope.qualificationDetails.push({ 'id': 'qualification' + newItemNo });
            }
        };

        $scope.removeNewQualification = function (index, data) {
            var newItemNo = $scope.qualificationDetails.length - 1;
            $scope.qualificationDetails.splice(index, 1);
            if (data.hrmC_Id > 0) {
                $scope.DeleteQualificationData(data);
            }
        };


        $scope.Qualification_previous = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };


        $scope.edit1 = function (obj) {
            $scope.mrfCand = obj
            $scope.Qualification = false;
            $scope.Family = false;
            $scope.Language = false;
            $scope.Experiance = false;
            $scope.HR_Candidate_Documents = false;

            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_FirstName;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_MiddleName;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_LastName;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_Skills;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_DOB;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_MobileNo;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_EmailId;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_ExpFrom;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_ExpTo;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_CurrentCompany;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_LastCTC;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_ExpectedCTC;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_AppDate;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_InterviewDate;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_RefCode;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_Remarks;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_Resume;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_ActiveFlg;
            //$scope.mrfCand.hrcD_FirstName = obj.HRMPT_Id;
            //$scope.mrfCand.hrcD_FirstName = obj.HRMPT_Name;
            //$scope.mrfCand.hrcD_FirstName = obj.HRMPT_Desc;
            //$scope.mrfCand.hrcD_FirstName = obj.HRMC_Id;
            //$scope.mrfCand.hrcD_FirstName = obj.HRMC_QulaificationName;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_RecruitmentStatus;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_ResumeSource;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_JobPortalName;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_Photo;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_FatherName;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_AadharNo;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_ScopeOfService;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_SHName;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_SHAddress;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_SHGender;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_SHContactNo;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_Place;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_PINCode;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_Department;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_NoticePeriod;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_NationalityId;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_Religion;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_MaritalStatus;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_BloodGroup;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_AddPermanentPIN;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_SHAddress2;
            //$scope.mrfCand.hrcD_FirstName = obj.HRMP_Id;
            //$scope.mrfCand.hrcD_FirstName = obj.HRMP_Position;
            //$scope.mrfCand.hrcD_FirstName = obj.HRMP_Skills;
            //$scope.mrfCand.hrcD_FirstName = obj.HRMP_Desc;
            //$scope.mrfCand.hrcD_FirstName = obj.HRCD_DocSubmitFlag;
        }


        $scope.edit = function (obj) {
            $scope.mrfCand = obj
            $('#blah').removeAttr('src');
            $('#blah').attr('src', $scope.mrfCand.hrcD_Photo);
            $scope.Qualification = false;
            $scope.Family = false;
            $scope.Language = false;
            $scope.Experiance = false;
            $scope.HR_Candidate_Documents = false;

            var data = {
                "HRCD_Id": obj.hrcD_Id
            };
            apiService.create("AddCandidateVMS/EditRecord", data).then(function (promise) {
                $scope.hrcD_Id = 0;
                $scope.hrcD_Id = promise.hrcD_Id;
                if (promise.qualificationlist !== null && promise.qualificationlist.length > 0) {
                    $scope.qualificationDetails = promise.qualificationlist;
                }
                if (promise.experiencelist !== null && promise.experiencelist.length > 0) {
                    $scope.experienceDetails = promise.experiencelist;
                }
                if (promise.languagelist !== null && promise.languagelist.length > 0) {
                    $scope.languageDetails = promise.languagelist;
                }
                if (promise.familylist !== null && promise.familylist.length > 0) {
                    $scope.familyDetails = promise.familylist;
                }

            });

        };

        $scope.activedeactive = function (obj) {
            $scope.mrfCand = obj
            $scope.Qualification = false;
            $scope.Family = false;
            $scope.Language = false;
            $scope.Experiance = false;
            var data = {
                "HRCD_Id": obj.hrcD_Id,
                "userMessage": ""
            };
            apiService.create("AddCandidateVMS/activedeactive", data).then(function (promise) {
                if (promise.retrunMsg === "Activated") {
                    swal("Record Activated Successfully");
                    $state.reload();
                }
                else if (promise.retrunMsg === "Not Activated") {
                    swal("Record Not Activated Successfully");
                    $state.reload();
                }
                else if (promise.retrunMsg === "Deactivated") {
                    swal("Record Deactivated Successfully");
                    $state.reload();
                }
                else if (promise.retrunMsg === "Not Deactivated") {
                    swal("Record Not Deactivated Successfully");
                    $state.reload();
                }

            });

        };


        $scope.validateQualificationDetails = function () {
            if ($scope.myForm3.$valid) {
                var data = {
                    Employeedto: $scope.Candidate,
                    QualificationsDTO: $scope.qualificationDetails,
                    "HRCD_Id": $scope.hrcD_Id
                };
                apiService.create("AddCandidateVMS/addQualification", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                                return;
                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                                $scope.Experiance = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                                $scope.Experiance = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }
                        }
                    });
            }
            else {
                $scope.submitted3 = true;
            }
        };

        $scope.clear_third_tab = function (data) {
            $scope.qualificationDetails = [{ id: 'qualification' }];
            $scope.submitted3 = false;
            $scope.myForm3.$setPristine();
            $scope.myForm3.$setUntouched();
        };

        $scope.experienceDetails = [{ id: 'experience' }];
        $scope.addNewExperience = function () {
            var newItemNo = $scope.experienceDetails.length + 1;
            if (newItemNo <= 10) {
                $scope.experienceDetails.push({ 'id': 'experience' + newItemNo });
                $scope.experienceDetails[newItemNo - 1].exitDateDis = true;
            }
        };

        $scope.removeNewExperience = function (index, data) {
            var newItemNo = $scope.experienceDetails.length - 1;
            $scope.experienceDetails.splice(index, 1);
            if (data.hrmeE_Id > 0) {
                $scope.DeleteExperienceData(data);
            }
        };

        $scope.EmployeeExperience = 0;

        $scope.validateExperiencePrevious = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };

        $scope.validateExperiencedetails = function () {
            if ($scope.myForm4.$valid) {
                var data = {
                    Employeedto: $scope.Candidate,
                    ExperienceDTO: $scope.experienceDetails,
                    "HRCD_Id": $scope.hrcD_Id
                };
                apiService.create("AddCandidateVMS/addexperience", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                                return;
                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                                $scope.Family = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                                $scope.Family = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }
                        }
                    });
            }
            else {
                $scope.submitted4 = true;
            }
        };

        $scope.clear_fourth_tab = function () {
            $scope.experienceDetails = [{ id: 'experience' }];
            $scope.submitted4 = false;
            $scope.myForm4.$setPristine();
            $scope.myForm4.$setUntouched();
        };

        //FAMILY DETAILS
        $scope.familyDetails = [{ id: 'family' }];
        $scope.addNewfamily = function () {
            var newItemNo = $scope.familyDetails.length + 1;
            if (newItemNo <= 10) {
                $scope.familyDetails.push({ 'id': 'family' + newItemNo });
            }
        };

        $scope.removeNewfamily = function (index, data) {
            var newItemNo = $scope.familyDetails.length - 1;
            $scope.familyDetails.splice(index, 1);
            if (data.hrmC_Id > 0) {
                $scope.DeleteQualificationData(data);
            }
        };

        $scope.family_previous = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };

        $scope.validatefamilyDetails = function () {
            if ($scope.myForm2.$valid) {
                var data = {
                    Employeedto: $scope.Candidate,
                    FamilyDTO: $scope.familyDetails,
                    "HRCD_Id": $scope.hrcD_Id
                };
                apiService.create("AddCandidateVMS/addfamily", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                                return;
                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                                $scope.Language = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                                $scope.Language = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }
                        }
                    });
            }
            else {
                $scope.submitted2 = true;
            }
        };

        $scope.clear_second_tab = function (data) {
            $scope.familyDetails = [{ id: 'family' }];
            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
        };
        //FAMILY DETAILS






        $scope.documents_previous = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };

        $scope.onFileChange = function (files, index) {
            if (files.length > 0) {
                var file = files[0];
                $scope.documentDetails[index].file = file;
                $scope.documentDetails[index].documentPath = file.name; 
                
            }
        };

        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd1 = function (input, document) {
            $scope.SelectedFileForUploadzd = input.files;
            $scope.vtadaA_FileName = input.files[0].name;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 5242880)  // 2097152 bytes = 2MB 
                {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/png" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/jpg" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/JPG" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/pdf" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/msword" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                //5242880
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].size > 5242880) {
                    swal("File size should be less than 5 MB");
                    return;
                }
            }
        };
        function UploaddianmateralPhoto(data) {
            console.log("TADA  Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= 1; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadtrnsportdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.ismclT_FilePath = d;
                    data.vtadaaF_FileName = $scope.vtadaA_FileName;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        };
        $scope.materaldocuupload = [{ itrS_Id: 'trans1' }];
        if ($scope.materaldocuupload.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addgrnrows = function () {
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
        //$scope.previewimg_new = function (img) {
        //    scope.ismclT_FilePath = img;
        //    var img = $scope.ismclT_FilePath;
        //    if (img != null) {
        //        var imagarr = img.split('.');
        //        var lastelement = imagarr[imagarr.length - 1];
        //        if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
        //            $('#preview').attr('src', $scope.ismclT_FilePath);
        //            $('#myimagePreview').modal('show');
        //        }
        //        else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
        //            $window.open($scope.ismclT_FilePath);
        //        }
        //        else if (lastelement == 'pdf') {
        //            $('#showpdf').modal('hide');
        //            var imagedownload1 = "";
        //            imagedownload1 = $scope.ismclT_FilePath;
        //            $http.get(imagedownload1, { responseType: 'arraybuffer' })
        //                .success(function (response) {
        //                    var fileURL = "";
        //                    var file = "";
        //                    var embed = "";
        //                    var pdfId = "";
        //                    file = new Blob([(response)], { type: 'application/pdf' });
        //                    fileURL = URL.createObjectURL(file);
        //                    pdfId = document.getElementById("pdfIdzz");
        //                    pdfId.removeChild(pdfId.childNodes[0]);
        //                    embed = document.createElement('embed');
        //                    embed.setAttribute('src', fileURL);
        //                    embed.setAttribute('type', 'application/pdf');
        //                    embed.setAttribute('width', '100%');
        //                    embed.setAttribute('height', '1000');
        //                    pdfId.appendChild(embed);
        //                    $('#showpdf').modal('show');
        //                });
        //        }
        //    }
        //    else {
        //        $window.open($scope.ismclT_FilePath)
        //    }
        //};

        $scope.previewimg_new = function (img) {
            if (!img) return;

            $scope.ismclT_FilePath = img;
            $scope.view_videos = [];

            // Get file extension in lowercase
            var extension = img.split('.').pop().toLowerCase();
            $scope.filetype2 = extension;

            // ==== Video Preview ====
            if (['wmv', 'mp4'].includes(extension)) {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myvideoPreview').modal('show');
            }

            // ==== Image Preview ====
            else if (['jpg', 'jpeg', 'png'].includes(extension)) {
                $('#preview').attr('src', img);
                $('#myimagePreview').modal('show');
            }

            // ==== Office Document Preview (using Microsoft Viewer) ====
            else if (['doc', 'docx', 'xls', 'xlsx', 'ppt', 'pptx'].includes(extension)) {
                var officeUrl = "https://view.officeapps.live.com/op/embed.aspx?src=" + encodeURIComponent(img);
                $http.get(img).then(function () {
                    $('#officeIframe').attr('src', officeUrl);
                    $('#showOffice').modal('show');
                }, function () {
                    // Optional fallback to Google Docs
                    var gviewUrl = "https://docs.google.com/gview?url=" + encodeURIComponent(img) + "&embedded=true";
                    $('#officeIframe').attr('src', gviewUrl);
                    $('#showOffice').modal('show');

                    // Optional alert
                    // alert("Microsoft viewer failed. Trying Google Docs viewer as fallback.");
                });
            }


            // ==== Audio Preview ====
            else if (extension === 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');
            }

            // ==== PDF Preview ====
            else if (extension === 'pdf') {
                $('#showpdf').modal('hide');
                $http.get(img, { responseType: 'arraybuffer' }).success(function (response) {
                    var file = new Blob([response], { type: 'application/pdf' });
                    var fileURL = URL.createObjectURL(file);
                    $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var embed = '<embed id="pdfview" src="' + $scope.content + '" style="width: 100%;" height="1000" type="application/pdf" />';
                    document.getElementById("pdfIdzz").innerHTML = embed;
                    $('#showpdf').modal('show');
                });
            }

            // ==== Fallback: Open in new tab ====
            else {
                $window.open(img, '_blank');
            }
        };


        $scope.validateDocumentDetails = function () {

            //if ($scope.validateDocumentDetails.$valid) {
            var data = {
                Employeedto: $scope.Candidate,
                "HRCD_Id": $scope.Candidate.hrcD_Id,
                "HR_Candidate_DocumentsDTO": $scope.materaldocuupload
            };

            apiService.create("AddCandidateVMS/AddCandidateDocuments", data)
                .then(function (promise) {
                    if (promise.returnMsg) {
                        switch (promise.returnMsg) {
                            case "Duplicate":
                                swal("Record already exists..!!");
                                break;
                            case "false":
                                swal("Record Not saved / Updated..", 'Fail');
                                break;
                            case "Add":
                                swal("Record Saved Successfully..");
                                $scope.Language = false;
                                $scope.myTabIndex += 1;
                                break;
                            case "Update":
                                swal("Record Updated Successfully..");
                                $scope.Language = false;
                                $scope.myTabIndex += 1;
                                break;
                            default:
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                break;
                        }
                    }
                });
        
            //} else {
            //    $scope.submitted2 = true;
            //}
        };



        $scope.clearDocumentsTab = function () {
            $scope.documentDetails = [{ id: 'document', documentPath: '' }]; 
            $scope.submitted2 = false;
            $scope.documentsForm.$setPristine();
            $scope.documentsForm.$setUntouched();
        };

// DOCUMENTS DETAILS


        //LANGUAGE DETAILS
        $scope.languageDetails = [{ id: 'language' }];
        $scope.addNewLanguage = function () {
            var newItemNo = $scope.languageDetails.length + 1;
            if (newItemNo <= 10) {
                $scope.languageDetails.push({ 'id': 'language' + newItemNo });
            }
        };

        $scope.removeNewLanguage = function (index, data) {
            var newItemNo = $scope.languageDetails.length - 1;
            $scope.languageDetails.splice(index, 1);
            if (data.hrmC_Id > 0) {
                $scope.DeleteQualificationData(data);
            }
        };

        $scope.language_previous = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };

        $scope.validatelanguageDetails = function () {
            if ($scope.myForm5.$valid) {
                var data = {
                    Employeedto: $scope.Candidate,
                    LanguagesDTO: $scope.languageDetails,
                    "HRCD_Id": $scope.hrcD_Id
                };
                apiService.create("AddCandidateVMS/addlanguage", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                                return;
                            }
                            else if (promise.retrunMsg === "Add") {
                                $scope.Qualification = true;
                                $scope.Experiance = true;
                                $scope.Family = true;
                                $scope.Language = true;
                                swal("Record Saved Successfully..");
                                $state.reload;
                            }
                            else if (promise.retrunMsg === "Update") {
                                $scope.Qualification = true;
                                $scope.Experiance = true;
                                $scope.Family = true;
                                $scope.Language = true;
                                swal("Record Updated Successfully..");
                                $state.reload;
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }
                        }
                    });
            }
            else {
                $scope.submitted5 = true;
            }
        };

        $scope.clear_fifth_tab = function (data) {
            $scope.languageDetails = [{ id: 'language' }];
            $scope.submitted5 = false;
            $scope.myForm5.$setPristine();
            $scope.myForm5.$setUntouched();
        };
        //LANGUAGE DETAILS
        $scope.hrcD_IdJNew = 0;
        $scope.AgreeTearmsandconditions = function (data) {
            $scope.hrcD_IdJNew = 0;
            $('#termsandconditionagree').modal('show');
            $scope.hrcD_IdJNew = data.hrcD_Id;           

        }
        //
       
        $scope.Tearmsandconditions = function () {
            $('#termsandconditionagree').modal('hide');                         
            var data = {
                "HRCD_Id": $scope.hrcD_IdJNew, 
                "userMessage":"Concert"
            };
            apiService.create("AddCandidateVMS/activedeactive", data).then(function (promise) {
                if (promise.retrunMsg === "Update") {
                    swal("Record Update Successfully");
                    $state.reload();
                }
                else if (promise.retrunMsg === "Not Update") {
                    swal("Record Not Update Successfully");
                  
                }
               
                $state.reload();
            });
        }


        $scope.savejob = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.mrfCand.hrcD_RefCode != null)
                    $scope.mrfCand.hrcD_RefCode = String($scope.mrfCand.hrcD_RefCode);

                if ($scope.mrfCand.hrcD_Remarks != null)
                    $scope.mrfCand.hrcD_Remarks = String($scope.mrfCand.hrcD_Remarks);

                if ($scope.mrfCand.hrcD_Resume != null)
                    $scope.mrfCand.hrcD_Resume = String($scope.mrfCand.hrcD_Resume);

                $scope.mrfCand.hrcD_RecruitmentStatus = "New";
                var data = $scope.mrfCand;
                console.log(data)
                apiService.create("AddCandidateVMS/", data).
                    then(function (promise) {

                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate") {
                                $('#termsandconditionagree').modal('hide');
                                swal("Record already exist..!!");

                                return;
                            }
                            else if (promise.retrunMsg === "false") {
                                $('#termsandconditionagree').modal('hide');
                                swal("Record Not saved / Updated..", 'Fail');
                                return;
                            }
                            else if (promise.retrunMsg === "Add") {
                                $('#termsandconditionagree').modal('hide');
                                swal("Record Saved Successfully..");

                                $scope.Candidate.hrcD_Id = promise.hrcD_Id;
                                $scope.Candidate.mI_Id = promise.mI_Id;
                                $scope.Qualification = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else if (promise.retrunMsg === "Update") {
                                $('#termsandconditionagree').modal('hide');
                                swal("Record Updated Successfully..");
                                $scope.Candidate.hrcD_Id = promise.hrcD_Id;
                                $scope.Candidate.mI_Id = promise.mI_Id;
                                $scope.Qualification = false;
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else {
                                $('#termsandconditionagree').modal('hide');
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }
                            //if (promise.employeeTypeList !== null && promise.employeeTypeList.length > 0) {
                            //    $scope.gridOptionsEmployeeType.data = promise.employeeTypeList;
                            //}
                            //$scope.cancel();
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }

        };
        $scope.mrfCand = {};

        $scope.redirectpage = function () {
            $state.go('app.CandidateDashboard');
        };

        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input, document) {
            $('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzd = input.files;
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                var extention = nameArray[nameArray.length - 1];
                if ((extention === "JPEG" || extention === "jpg" || extention === "docx" || extention === "doc" || extention === "pdf")
                    && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.id) //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocument(document);
                }
                else if (extention !== "JPEG" && extention !== "jpg" && extention !== "docx" && extention !== "doc" && extention !== "pdf") {
                    $('#' + document.id).removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    $('#' + document.id).removeAttr('src');
                    swal("Document size should be less than 2MB");
                    return;
                }
            }
        };

        function UploadEmployeeDocument(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzd.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
            }
            // We can send more data to server using append         
            //formData.append("Id", 0);
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
                        data.hrcD_Resume = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        //COMMENT
        $scope.uploadmateraldocuments1 = [];
        $scope.uploadmateraldocuments = function (input, document) {

            $scope.uploadmateraldocuments1 = input.files;
            $scope.filename = input.files[0].name;
            if (input.files && input.files[0]) {

                if ((input.files[0].type === "image/jpeg" || input.files[0].type === "image/jpg") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "video/mp4" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/pdf" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/msword" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;

                }
            };

            function UploaddianmateralPhoto(data) {
                console.log("Student Upload  :" + data);
                var formData = new FormData();
                for (var i = 0; i <= $scope.uploadmateraldocuments1.length; i++) {
                    formData.append("File", $scope.uploadmateraldocuments1[i]);
                }
                var defer = $q.defer();
                $http.post("/api/ImageUpload/Addcandidate", formData,
                    {
                        withCredentials: true,
                        headers: { 'Content-Type': undefined },
                        transformRequest: angular.identity
                    })
                    .success(function (d) {
                        defer.resolve(d);
                        data.hrcD_Resume = d;
                        data.cfilename = $scope.filename;
                    })
                    .error(function () {
                        defer.reject("File Upload Failed!");
                    });
            }
            //COMMENT

            

            $timeout(function () {
                if ($scope.myForm && $scope.myForm.doc) {
                    $scope.myForm.doc.$setValidity('required', true);
                }
            });
        }
            //upload Employee Profile pic
            $scope.UploadEmpProfilePic = [];
            $scope.uploadEmployeeProfilePic = function (input) {
                $scope.UploadEmpProfilePic = input.files;
                $('#blah').removeAttr('src');
                if (input.files && input.files[0]) {
                    if ((input.files[0].type == "image/jpeg" || input.files[0].type == "image/png") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                    {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $('#blah')
                                .attr('src', e.target.result);
                        };
                        reader.readAsDataURL(input.files[0]);
                        UploadEmployeeprofile();
                    }
                    else if (input.files[0].type != "image/jpeg") {
                        swal("Please Upload the JPEG file");
                        angular.element("input[type='file']").val(null);
                        return;
                    } else if (input.files[0].size > 2097152) {
                        swal("Image size should be less than 2MB");
                        angular.element("input[type='file']").val(null);
                        return;
                    }
                }
            };

            function UploadEmployeeprofile() {

                var formData = new FormData();

                for (var i = 0; i <= $scope.UploadEmpProfilePic.length; i++) {
                    formData.append("File", $scope.UploadEmpProfilePic[i]);
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
                            $scope.mrfCand.HRCD_Photo = d;
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
    }
})();