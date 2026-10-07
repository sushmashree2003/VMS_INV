(function () {
    'use strict';
    angular
        .module('app')
        .controller('PostSalesReportController', PostSalesReportController)

    PostSalesReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', '$compile']
    function PostSalesReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, $compile) {

        $scope.itemsPerPage = 5;
        $scope.currentPage = 1;
        $scope.search = "";
        $scope.currentDate = new Date();
        $scope.obj = {}; $scope.clemail = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        }
        // ============================ng_click function for module (ALL)========================//
        $scope.searchchkbx = "";
        $scope.ISMSLE_Id = 0;
        $scope.all_checkC = function () {
            var checkStatus = $scope.usercheckC;
            var count = 0;
            angular.forEach($scope.module_list, function (itm) {
                itm.selected = checkStatus;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }
        $scope.togchkbxC = function () {
            $scope.usercheckC = $scope.module_list.every(function (options) {
                return options.selected;
            });
        }

        //====================================clear data====================================//
        $scope.submitted = false;
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.isOptionsRequired = function () {
            return !$scope.module_list.some(function (options) {
                return options.selected;
            });
        }
      
        $scope.MOUDATES = function () {
            if ($scope.ISMMCLTPR_MOUStartDate) {
                $scope.ISMMCLTPR_MOUDate = new Date($scope.ISMMCLTPR_MOUStartDate);
                $scope.ISMMCLTPR_DealClosureDate = new Date($scope.ISMMCLTPR_MOUStartDate);
            }
        };


        $scope.DateFunction  = function () {
            $scope.MOUDATES();
            $scope.updateEndDate();
        };

        $scope.handleChange = function () {
            $scope.updateEndYear();
            $scope.updateEndYear();
        };


        $scope.updateEndDate = function () {
            if ($scope.ISMMCLTPR_MOUStartDate && $scope.ISMMCLTPR_ProjectDuration) {
                var startDate = new Date($scope.ISMMCLTPR_MOUStartDate);
                var endDate = new Date(startDate);
                endDate.setFullYear(startDate.getFullYear() + $scope.ISMMCLTPR_ProjectDuration);
                $scope.ISMMCLTPR_MOUEndDate = endDate;
            }
        };



        $scope.Contract_Year_From = null;
        $scope.ISMMCLTPR_ProjectDuration = null;
        $scope.Contract_Year_To = null;
        $scope.updateEndYear = function () {
            if ($scope.Contract_Year_From && $scope.ISMMCLTPR_ProjectDuration) {
                var startYear = parseInt($scope.Contract_Year_From);
                var duration = parseInt($scope.ISMMCLTPR_ProjectDuration);
                var endYear = startYear + duration;
                $scope.Contract_Year_To = endYear;
            } else {
                $scope.Contract_Year_To = null;
            }
        };




        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //==================load data=========================//
        $scope.submitted = false;
        $scope.loaddata = function () {
            var id = 2;
            $scope.search = "";
            apiService.getURI("Lead_mou_/OnloadGetDetails", id).
                then(function (promise) {
                    $scope.grid_view = false;
                    $scope.mompendinglist = promise.mompendinglist;
                    $scope.leaddata = promise.leadloaddata;
                    if (promise.mousavedeatils != null && promise.mousavedeatils) {
                        $scope.mousavedeatils = promise.mousavedeatils;
                    }
                    $scope.yearlist = promise.yearlist;
                    $scope.getinstallment = promise.getinstallment;
                });
        }

        //=====================project=====================//
        $scope.getproject = function () {
            var data = {
                "ISMMCLT_Id": $scope.obj.ISMMCLT_Id.ismmclT_Id,
                "HRMD_Id": $scope.hrmD_Id
            }
            apiService.create("ISM_ClientProject_Mapping/getproject", data).then(function (promise) {
                $scope.module_list = "";
                $scope.project_list = promise.project_list;
            })
        }
        $scope.interacted2 = function (field) {
            return $scope.submitted;
        };
        //======module============================//
        $scope.getmodule = function (projectid, deptid) {
            var data = {
                "ISMMCLT_Id": $scope.obj.ISMMCLT_Id.ismmclT_Id,
                "ISMMPR_Id": $scope.obj.ISMMPR_Id.ismmpR_Id,
                "HRMD_Id": deptid
            }
            apiService.create("ISM_ClientProject_Mapping/getmodule", data).then(function (promise) {
                $scope.module_list = promise.module_list;
            })
        }
        //================================================save================================================//
        $scope.savedata = function (clint, proj) {
            if ($scope.myForm.$valid) {
                var modulesss = [];
                angular.forEach($scope.module_list, function (rr) {
                    if (rr.selected === true) {
                        modulesss.push(rr);
                    }
                });
                var ISMMCLTPR_CostPerYearStudent = 0;
                if ($scope.ISMMCLTPR_CostPerYearStudent > 0) {
                    ISMMCLTPR_CostPerYearStudent = $scope.ISMMCLTPR_CostPerYearStudent;
                }
                var data = {
                    "ISMMCLTPR_Id": $scope.ismmcltpR_Id,
                    "ISMMCLT_Id": $scope.obj.ISMMCLT_Id.ismmclT_Id,
                    "ISMMPR_Id": $scope.obj.ISMMPR_Id.ismmpR_Id,
                    "moduleidss": modulesss,
                    "ISMMCLTPR_ProposalRefNo": $scope.ISMMCLTPR_ProposalRefNo,
                    "ISMMCLTPR_ProposalSentDate": $scope.ISMMCLTPR_ProposalSentDate,
                    "ISMMCLTPR_DealClosureDate": $scope.ISMMCLTPR_DealClosureDate,
                    "ISMMCLTPR_MOURefNo": $scope.ISMMCLTPR_MOURefNo,
                    "ISMMCLTPR_MOUDate": $scope.ISMMCLTPR_MOUDate,
                    "ISMMCLTPR_MOURepresentedBy": $scope.ISMMCLTPR_MOURepresentedBy,
                    "ISMMCLTPR_MOUStartDate": $scope.ISMMCLTPR_MOUStartDate,
                    "ISMMCLTPR_MOUEndDate": $scope.ISMMCLTPR_MOUEndDate,
                    "ISMMCLTPR_NodalOfficerName": $scope.ISMMCLTPR_NodalOfficerName,
                    "ISMMCLTPR_NodalOfficerContactNo": $scope.ISMMCLTPR_NodalOfficerContactNo,
                    "ISMMCLTPR_NodalOfficerEmailId": $scope.ISMMCLTPR_NodalOfficerEmailId,
                    "ISMMCLTPR_ProjectDuration": $scope.ISMMCLTPR_ProjectDuration,
                    "ISMMCLTPR_TotalStudent": $scope.ISMMCLTPR_TotalStudent,
                    "ISMMCLTPR_CostPerStudent": $scope.ISMMCLTPR_CostPerStudent,
                    "ISMMCLTPR_EnhancementPerYr": $scope.ISMMCLTPR_EnhancementPerYr,
                    "ISMMCLTPR_WorkOrder": $scope.ISMMCLTPR_WorkOrder,
                    "ISMSLET_Id": $scope.ISMSLET_Id.ISMSLET_Id,
                    "Template": $scope.Template,
                    "Contract_Year_From": $scope.Contract_Year_From,
                    "ISMMCLTPR_IEcharge": $scope.ISMMCLTPR_IEcharge,
                    "ISMMCLTPR_OperatorCost": $scope.ISMMCLTPR_OperatorCost,
                    "Contract_Year_To": $scope.Contract_Year_To,
                    "ISMMCLTPR_Percentage": $scope.ISMMCLTPR_Percentage,
                    "ISMMCLTPR_CostPerYearStudent": ISMMCLTPR_CostPerYearStudent
                };
                apiService.create("ISM_ClientProject_Mapping/savedata", data).then(function (promise) {
                    if (promise.msg == 'saved') {
                        swal("Data Saved Successfully...!!!");
                    }
                    else if (promise.msg == 'Failed') {
                        swal("Data Not Saved Successfully...!!!");
                    }
                    else if (promise.msg == 'updated') {
                        swal("Data Successfully Updated");
                    }
                    else {

                        swal("Data Already Exists");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        }
        //=====================================================edit data================================//
        $scope.Editdata = function (user) {
            debugger;
            var data = {
                "ISMMCLTPR_Id": user.ismmcltpR_Id
            }
            apiService.create("ISM_ClientProject_Mapping/Editdata", data).then(function (promise) {
                debugger;
                if (promise.editlist.length > 0) {
                    $scope.module_list = promise.module_list;
                    $scope.client_list = promise.client_list;
                    $scope.project_list = promise.project_list;
                    $scope.hrmD_Id = promise.editlist[0].hrmD_Id;
                    $scope.ismmcltpR_Id = promise.editlist[0].ismmcltpR_Id;
                    $scope.obj = promise.editlist[0];
                    $scope.ISMMCLTPR_ProposalRefNo = promise.editlist[0].ismmcltpR_ProposalRefNo;
                    $scope.ISMMCLTPR_ProposalSentDate = new Date(promise.editlist[0].ismmcltpR_ProposalSentDate);
                    $scope.ISMMCLTPR_DealClosureDate = new Date(promise.editlist[0].ismmcltpR_DealClosureDate);
                    $scope.ISMMCLTPR_MOURefNo = promise.editlist[0].ismmcltpR_MOURefNo;
                    $scope.ISMMCLTPR_MOUDate = new Date(promise.editlist[0].ismmcltpR_MOUDate);
                    $scope.ISMMCLTPR_MOURepresentedBy = promise.editlist[0].ismmcltpR_MOURepresentedBy;
                    $scope.ISMMCLTPR_MOUStartDate = new Date(promise.editlist[0].ismmcltpR_MOUStartDate);
                    $scope.ISMMCLTPR_MOUEndDate = new Date(promise.editlist[0].ismmcltpR_MOUEndDate);
                    $scope.ISMMCLTPR_NodalOfficerName = promise.editlist[0].ismmcltpR_NodalOfficerName;
                    $scope.ISMMCLTPR_NodalOfficerContactNo = promise.editlist[0].ismmcltpR_NodalOfficerContactNo;
                    $scope.ISMMCLTPR_NodalOfficerEmailId = promise.editlist[0].ismmcltpR_NodalOfficerEmailId;
                    $scope.ISMMCLTPR_ProjectDuration = promise.editlist[0].ismmcltpR_ProjectDuration;
                    $scope.ISMMCLTPR_TotalStudent = promise.editlist[0].ismmcltpR_TotalStudent;
                    $scope.ISMMCLTPR_CostPerStudent = promise.editlist[0].ismmcltpR_CostPerStudent;
                    $scope.ISMMCLTPR_EnhancementPerYr = promise.editlist[0].ismmcltpR_EnhancementPerYr;
                    $scope.ISMMCLTPR_WorkOrder = promise.editlist[0].ismmcltpR_WorkOrder;


                    angular.forEach($scope.module_list, function (yy) {
                        angular.forEach(promise.editlist, function (uu) {
                            if (yy.ivrmM_Id == uu.ivrmM_Id) {
                                yy.selected = true;
                            }
                        })
                    })
                }
            })
        }
        //==========================================deactivate=====================================//
        $scope.clientDecative = function (usersem, SweetAlert) {

            var dystring = "";

            if (usersem.ismmcltpR_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (usersem.ismmcltpR_ActiveFlag == false) {
                dystring = "Activate"
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
                        apiService.create("ISM_ClientProject_Mapping/clientDecative", usersem).
                            then(function (promise) {

                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                                else {
                                    swal("Record Not " + dystring + "d" + " Successfull!!!");
                                    $state.reload();
                                }
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }
        $scope.SelectedFileForUploadzdBOSBOE = [];
        $scope.selectFileforUploadzdBOSBOE = function (input) {
            $scope.SelectedFileForUploadzdBOSBOE = input.files;

            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                $scope.extention = nameArray[nameArray.length - 1];
                if (($scope.extention === "JPEG" || $scope.extention === "jpg" || $scope.extention === "JPG" || $scope.extention === "png" || $scope.extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blah')
                            .attr('src', e.target.result);

                        $('#documentid')
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentBOSBOE();
                }
                else if ($scope.extention !== "JPEG" && $scope.extention !== "png" && $scope.extention !== "jpg" && $scope.extention !== "JPG" && $scope.extention !== "pdf") {
                    $('#documentid').removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    $('#documentid').removeAttr('src');
                    swal("Document size should be less than 2MB");
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
                        $scope.hrelT_SupportingDocument = d;
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
            var filename = $scope.hrelT_SupportingDocument.toString();
            var nameArray = filename.split('.');
            $scope.extention = nameArray[nameArray.length - 1];
            if ($scope.extention === "jpg" || $scope.extention === "jpeg" || $scope.extention === "JPG" || $scope.extention === "JPEG" || $scope.extention === "png"
                || $scope.extention === "PNG") {
                $('#preview').attr('src', $scope.hrelT_SupportingDocument);
                //$('#myModal').modal('show');
            }
            else if ($scope.extention === "doc" || $scope.extention === "docx" || $scope.extention === "xls" || $scope.extention === "xlsx") {
                //$('#preview').removeAttr('src');
                $('#preview').attr('src', $scope.hrelT_SupportingDocument);
            }
            else if ($scope.extention === "pdf") {
                var imagedownload = $scope.hrelT_SupportingDocument;
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
        //added
        $scope.get_lead_product = function () {
            $scope.bomListNew = []; $scope.ModuleName = [];
            $scope.ISMSLE_LeadName = ""; $scope.ISMSLE_Id = 0;
            angular.forEach($scope.mompendinglist, function (d) {
                if (d.ISMSLET_Id == $scope.ISMSLET_Id.ISMSLET_Id && d.Flag == $scope.ISMSLET_Id.Flag) {
                    $scope.ISMSLE_Id = d.ISMSLE_Id
                    $scope.ISMMCLTPR_EnhancementPerYr = 15;
                    $scope.ISMMCLTPR_NodalOfficerName = d.ISMSLE_ContactPerson;
                    $scope.ISMMCLTPR_NodalOfficerEmailId = d.ISMSLE_EmailId;
                    $scope.ISMMCLTPR_NodalOfficerContactNo = d.ISMSLE_ContactNo;
                    // $scope.ISMMCLTPR_NodalOfficerContactNo = d.ISMSLE_ContactNo;
                    $scope.ISMSMOUA_ProductName = d.ISMSMPR_ProductName;
                    $scope.ISMMCLTPR_TotalStudent = d.ISMSLE_StudentStrength;
                    $scope.ISMMCLTPR_CostPerStudent = d.Negotiation_Amount;
                    $scope.ISMSMOU_PackageName = d.ISMSBOM_BOMName;
                    $scope.ISMMCLTPR_ProposalRefNo = d.FMSCOR_RefernceNo;
                    $scope.ISMMCLTPR_CostPerStudent = d.Negotiation_Amount;
                    $scope.ISMSLE_LeadName = d.ISMSLE_LeadNameNew;
                    $scope.ISMSLE_LeadAddress1 = d.ISMSLE_LeadAddress1;
                    $scope.ISMSLE_LeadAddress2 = d.ISMSLE_LeadAddress2;
                    $scope.ISMSLE_LeadAddress3 = d.ISMSLE_LeadAddress3;
                    $scope.ISMSLE_Pincode = d.ISMSLE_Pincode;
                    $scope.ISMSLE_ContactDesignation = d.ISMSLE_ContactDesignation;
                    $scope.ISMMCLTPR_ProposalSentDate = new Date(d.ISMSLET_CreatedDate);
                                    }
            });
            var data = {
                "ISMSLE_Id": $scope.ISMSLE_Id,
                "Type": "M"
            }
            apiService.create("Lead_mou_/lead_product", data).
                then(function (promise) {
                    $scope.ModuleName = promise.moduleName;

                    if (promise.momproductpendinglist != null && promise.momproductpendinglist.length > 0) {
                        angular.forEach(promise.momproductpendinglist, function (d) {
                            $scope.bomListNew.push({ ISMSBOM_Id: d.ISMSBOM_Id });
                        });
                        $scope.OnChangeBOMList();
                    }


                })

        }
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

        $scope.obj1 = {};

        $scope.savedataLead = function (clint, proj) {
            if (true) {
                $scope.files_paths = [];
                $scope.arrayMOU = [];
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
                angular.forEach($scope.transrows, function (ob) {
                    $scope.arrayMOU.push({
                        FTI_Id: ob.FTI_Id, ASMAY_Id: ob.ISMCLTPRP_Year, ISMCLTPRP_InstallmentAmt: ob.ISMCLTPRP_InstallmentAmt, ISMCLTPRP_PaymentDate: ob.ISMCLTPRP_PaymentDate
                    });
                });
                var ISMMCLTPR_CostPerYearStudent = 0;
                if ($scope.ISMMCLTPR_CostPerYearStudent > 0) {
                    ISMMCLTPR_CostPerYearStudent = $scope.ISMMCLTPR_CostPerYearStudent;
                }

                var data = {
                    "ISMSLE_Id": $scope.ISMSLE_Id,
                    "ISMMCLTPR_ProposalRefNo": $scope.ISMMCLTPR_ProposalRefNo,
                    "ISMMCLTPR_ProposalSentDate": $scope.ISMMCLTPR_ProposalSentDate,
                    "ISMMCLTPR_DealClosureDate": $scope.ISMMCLTPR_DealClosureDate,

                    "ISMMCLTPR_MOURefNo": $scope.ISMMCLTPR_MOURefNo,
                    "ISMMCLTPR_MOUDate": new Date($scope.ISMMCLTPR_MOUStartDate).toDateString(),
                    "ISMMCLTPR_MOURepresentedBy": $scope.ISMMCLTPR_MOURepresentedBy,
                    "ISMMCLTPR_MOUStartDate": new Date($scope.ISMMCLTPR_MOUStartDate).toDateString(),
                    "ISMMCLTPR_MOUEndDate": new Date($scope.ISMMCLTPR_MOUEndDate).toDateString(),
                    "ISMMCLTPR_NodalOfficerName": $scope.ISMMCLTPR_NodalOfficerName,
                    "ISMMCLTPR_NodalOfficerContactNo": $scope.ISMMCLTPR_NodalOfficerContactNo,
                    "ISMMCLTPR_NodalOfficerEmailId": $scope.ISMMCLTPR_NodalOfficerEmailId,
                    "ISMMCLTPR_ProjectDuration": $scope.ISMMCLTPR_ProjectDuration,
                    "ISMMCLTPR_TotalStudent": $scope.ISMMCLTPR_TotalStudent,
                    "ISMMCLTPR_CostPerStudent": $scope.ISMMCLTPR_CostPerStudent,
                    "ISMMCLTPR_EnhancementPerYr": $scope.ISMMCLTPR_EnhancementPerYr,
                    "ISMMCLTPR_WorkOrder": $scope.ISMMCLTPR_WorkOrder,
                    "ISMSMOUA_ProductName": $scope.ISMSMOUA_ProductName,
                    "ISMSMOU_PackageName": $scope.ISMSMOU_PackageName,
                    "ISMSLET_Id": $scope.ISMSLET_Id.ISMSLET_Id,
                    "Template": $scope.Template,
                    "Contract_Year_From": $scope.Contract_Year_From,
                    "ISMMCLTPR_IEcharge": $scope.ISMMCLTPR_IEcharge,
                    "ISMMCLTPR_OperatorCost": $scope.ISMMCLTPR_OperatorCost,
                    "Contract_Year_To": $scope.Contract_Year_To,
                    "ISMMCLTPR_Percentage": $scope.ISMMCLTPR_Percentage,
                    "ISMMCLT_Host": $scope.ISMMCLT_Host,
                    "ISMMCLTPR_CostPerYearStudent": ISMMCLTPR_CostPerYearStudent

                };
                apiService.create("ISM_ClientProject_Mapping/savedataLead", data).then(function (promise) {
                    if (promise.msg == 'saved') {
                        swal("Data Saved Successfully");
                    }
                    else if (promise.msg == 'Failed') {
                        swal("Data Not Saved");
                    }
                    else if (promise.msg == 'updated') {
                        swal("Data Updated Successfully ");
                    }
                    else {

                        swal("Data not Exists");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.showTemplateDropdown = false;

        $scope.toggleTemplateDropdown = function () {
            $scope.showTemplateDropdown = !$scope.showTemplateDropdown;
        };
        $scope.Viewtemplateone = function (itm) {
            //  itm.Template = "MET";
            $scope.ISMSLE_Id = 0;
            $scope.ISMMCLTPR_NodalOfficerName = ""; $scope.ISMMCLTPR_NodalOfficerEmailId = ""; $scope.ISMMCLTPR_NodalOfficerContactNo = ""; $scope.ISMMCLTPR_NodalOfficerContactNo = "";
            $scope.ISMSMOUA_ProductName = ""; $scope.ISMMCLTPR_TotalStudent = ""; $scope.ISMSMOU_PackageName = ""; $scope.ISMMCLTPR_ProposalRefNo = "";
            $scope.ISMMCLTPR_CostPerStudent = ""; $scope.ISMMCLTPR_CostPerStudent = ""; $scope.ISMSLE_LeadName = ""; $scope.ISMSLE_LeadAddress1 = "";
            $scope.ISMSLE_LeadAddress2 = ""; $scope.ISMSLE_LeadAddress3 = ""; $scope.ISMSLE_Pincode = ""; $scope.ISMSLE_ContactDesignation = ""; $scope.ISMSLE_EmailId = "";
            var items = {
                "pending": itm.Template,
                "msg": "FetchTemp",
                "ISMSBOM_Id": itm.ISMSBOM_Id,
                "ISMSLE_Id": itm.ISMSLE_ID
            }
            apiService.create("ISM_ClientProject_Mapping/viewtemplatedetails", items).then(function (promise) {
                var e1 = angular.element(document.getElementById("bindtemplate"));
                if (promise.templatelist != null && promise.templatelist.length > 0) {
                    $scope.ISMSLE_Id = itm.ISMSLE_ID;
                    $scope.bindtemplate = promise.templatelist;
                    $scope.xtra = $scope.bindtemplate[0].VMSMOUT_Template;
                    e1.html($scope.xtra);
                    $compile(e1.contents())($scope);
                    $scope.ModuleName = promise.moduleName;
                    $scope.leadprojectpayment = promise.leadprojectpayment;
                    $scope.groupedData = [];
                    $scope.leadprojectpayment.forEach(item => {
                        let yearFlagGroup = $scope.groupedData.find(group => group.YearFlag === item.YearFlag);
                        if (!yearFlagGroup) {
                            yearFlagGroup = {
                                YearFlag: item.YearFlag,
                                records: []
                            };
                            $scope.groupedData.push(yearFlagGroup);
                        }
                        yearFlagGroup.records.push(item);
                    });
                    if (promise.momproductpendinglist1 != null && promise.momproductpendinglist1.length > 0) {
                        angular.forEach(promise.momproductpendinglist1, function (d) {
                            $scope.bomListNew.push({ ISMSBOM_Id: d.ISMSBOM_Id });
                        });
                        angular.forEach(promise.momproductpendinglist1, function (d) {
                            if (d.ISMSLE_Id == itm.ISMSLE_ID) {
                                $scope.ISMSLE_EmailId = d.ISMSLE_EmailId;
                                $scope.ISMMCLTPR_ProjectDuration = d.ISMMCLTPR_ProjectDuration;
                                $scope.ISMMCLTPR_MOURefNo = d.ISMMCLTPR_MOURefNo;
                                $scope.ISMMCLTPR_MOUDate = d.ISMMCLTPR_MOUDate;
                                $scope.ISMMCLTPR_ProposalRefNo = d.ISMMCLTPR_ProposalRefNo;
                                $scope.ISMMCLTPR_EnhancementPerYr = d.ISMMCLTPR_EnhancementPerYr;
                                $scope.ISMMCLTPR_MOUStartDate = d.ISMMCLTPR_MOUStartDate;
                                $scope.ISMMCLTPR_MOUEndDate = d.ISMMCLTPR_MOUEndDate;
                                $scope.Contract_Year_To = d.ISMMCLT_Contract_YearTo;
                                $scope.Contract_Year_From = d.ISMMCLT_Contract_YearFrom;
                                $scope.ISMMCLTPR_NodalOfficerName = d.ISMSLE_ContactPerson;
                                $scope.ISMMCLTPR_NodalOfficerEmailId = d.ISMSLE_EmailId;
                                $scope.ISMMCLTPR_NodalOfficerContactNo = d.ISMSLE_ContactNo;
                                $scope.ISMMCLTPR_NodalOfficerContactNo = d.ISMSLE_ContactNo;
                                $scope.ISMSMOUA_ProductName = d.ISMSMPR_ProductName;
                                $scope.ISMMCLTPR_TotalStudent = d.ISMSLE_StudentStrength;
                                $scope.ISMMCLTPR_CostStudent = d.Negotiation_Amount;
                                $scope.ISMSMOU_PackageName = d.ISMSBOM_BOMName;
                                $scope.ISMMCLTPR_ProposalRefNo = d.FMSCOR_RefernceNo;
                                //$scope.ISMMCLTPR_CostPerStudent = d.Negotiation_Amount;
                                $scope.ISMSLE_LeadName = d.ISMSLE_LeadNameNew;
                                $scope.ISMSLE_LeadAddress1 = d.ISMSLE_LeadAddress1;
                                $scope.ISMSLE_LeadAddress2 = d.ISMSLE_LeadAddress2;
                                $scope.ISMSLE_LeadAddress3 = d.ISMSLE_LeadAddress3;
                                $scope.ISMSLE_Pincode = d.ISMSLE_Pincode;
                                $scope.ISMSLE_ContactDesignation = d.ISMSLE_ContactDesignation;
                                if (d.ISPSIF_ItemsName == 'SMS Pack') {
                                    $scope.ISPSIF_Quantity = d.ISPSIF_Quantity
                                }

                            }
                        });
                        $scope.OnChangeBOMList();
                    }
                    $("#templatemodel").modal('show');
                   
                    //$scope.clearFunction();
                   
                }
                else {
                    e1.html('');
                    swal("No Template Found");
                }
            })
        }
        //order acceptance 
        $scope.Viewtemplateone2 = function (itm) {
            $scope.ISMSLE_LeadName = ""; $scope.ISMSLE_Id = 0;
            $scope.services = [];
            var data = {
                "ISMSLE_Id": itm.ISMSLE_ID,
                "Type": "E",
                "ISMSLET_Id":itm.ISMSLET_Id
            }
            apiService.create("Lead_mou_/lead_product", data).
                then(function (promise) {
                    $scope.ModuleName = promise.moduleName1;
                    $scope.services = promise.approvedBy;
                    $scope.leadprojectpayment = promise.leadprojectpayment;
                    $scope.groupedData = [];
                    $scope.leadprojectpayment.forEach(item => {
                        let yearFlagGroup = $scope.groupedData.find(group => group.YearFlag === item.YearFlag);
                        if (!yearFlagGroup) {
                            // If not found, create a new group for this YearFlag
                            yearFlagGroup = {
                                YearFlag: item.YearFlag,
                                records: []
                            };
                            $scope.groupedData.push(yearFlagGroup);
                        }
                        // Push the current item into the appropriate yearFlagGroup
                        yearFlagGroup.records.push(item);
                    });

                    if (promise.momproductpendinglist1 != null && promise.momproductpendinglist1.length > 0) {
                        angular.forEach(promise.momproductpendinglist1, function (d) {
                            $scope.ISMSLE_Id = d.ISMSLE_Id;
                            $scope.ISMMCLTPR_NodalOfficerContactNo = d.ISMSLE_ContactNo;
                            $scope.ISMMCLTPR_NodalOfficerName = d.ISMSLE_ContactPerson;
                            $scope.ISMMCLTPR_NodalOfficerEmailId = d.ISMSLE_EmailId;
                            $scope.ISMSMOUA_ProductName = d.ISMSMPR_ProductName;
                            $scope.ISMMCLTPR_TotalStudent = d.ISMSLE_StudentStrength;
                            $scope.ISMMCLTPR_CostPerStudent = d.Negotiation_Amount;
                            $scope.ISMSMOU_PackageName = d.ISMSBOM_BOMName;
                            $scope.ISMMCLTPR_ProposalRefNo = d.FMSCOR_RefernceNo;
                            $scope.ISMMCLTPR_CostPerStudent = d.Negotiation_Amount;
                            $scope.ISMSLE_LeadName = d.ISMSLE_LeadName;
                            $scope.ISMSLE_LeadAddress1 = d.ISMSLE_LeadAddress1;
                            $scope.ISMSLE_LeadAddress2 = d.ISMSLE_LeadAddress2;
                            $scope.ISMSLE_LeadAddress3 = d.ISMSLE_LeadAddress3;
                            $scope.ISMMCLTPR_ProjectDuration = d.ISMMCLTPR_ProjectDuration;
                            $scope.ISMMCLTPR_EnhancementPerYr = d.ISMMCLTPR_EnhancementPerYr;
                            $scope.ISMSLE_ContactDesignation = d.ISMSLE_ContactDesignation;
                            $scope.ISMSLE_Pincode = d.ISMSLE_Pincode;
                            $scope.ISMMCLTPR_MOUStartDate = d.ISMMCLTPR_MOUStartDate;
                            $scope.ISMSLE_ContactDesignation = d.ISMSLE_ContactDesignation;
                            $scope.ISMMCLTPR_MOURefNo = d.ISMMCLTPR_MOURefNo;
                            if (d.ISPSIF_ItemsName == 'SMS Pack') {
                                $scope.ISPSIF_Quantity = d.ISPSIF_Quantity
                            }
                        });
                    }
                    $('#OrderAcceptance').modal('show');
                })
        }
        $scope.Viewtemplateone1 = function (itm) {
            //  itm.Template = "MET";
            $scope.ISMSLE_Id = 0;
            $scope.ISMMCLTPR_NodalOfficerName = ""; $scope.ISMMCLTPR_NodalOfficerEmailId = ""; $scope.ISMMCLTPR_NodalOfficerContactNo = ""; $scope.ISMMCLTPR_NodalOfficerContactNo = "";
            $scope.ISMSMOUA_ProductName = ""; $scope.ISMMCLTPR_TotalStudent = ""; $scope.ISMSMOU_PackageName = ""; $scope.ISMMCLTPR_ProposalRefNo = "";
            $scope.ISMMCLTPR_CostPerStudent = ""; $scope.ISMMCLTPR_CostPerStudent = ""; $scope.ISMSLE_LeadName = ""; $scope.ISMSLE_LeadAddress1 = "";
            $scope.ISMSLE_LeadAddress2 = ""; $scope.ISMSLE_LeadAddress3 = ""; $scope.ISMSLE_Pincode = ""; $scope.ISMSLE_ContactDesignation = ""; $scope.ISMSLE_EmailId = "";
            var items = {
                "pending": itm.Template,
                "msg": "FetchTemp",
                "ISMSBOM_Id": itm.ISMSBOM_Id,
                "ISMSLE_Id": itm.ISMSLE_ID
            }
            apiService.create("ISM_ClientProject_Mapping/viewtemplatedetails", items).then(function (promise) {
                var e1 = angular.element(document.getElementById("bindtemplate"));
                if (promise.templatelist != null && promise.templatelist.length > 0) {
                    $scope.ISMSLE_Id = itm.ISMSLE_ID;
                    $scope.bindtemplate = promise.templatelist;
                    $scope.xtra = $scope.bindtemplate[0].VMSMOUT_Template;
                    e1.html($scope.xtra);
                    $compile(e1.contents())($scope);
                    $scope.ModuleName = promise.moduleName;
                    if (promise.momproductpendinglist1 != null && promise.momproductpendinglist1.length > 0) {
                        angular.forEach(promise.momproductpendinglist1, function (d) {
                            $scope.bomListNew.push({ ISMSBOM_Id: d.ISMSBOM_Id });
                        });
                        angular.forEach(promise.momproductpendinglist1, function (d) {
                            if (d.ISMSLE_Id == itm.ISMSLE_ID) {
                                $scope.ISMSLE_EmailId = d.ISMSLE_EmailId;
                                $scope.ISMMCLTPR_ProjectDuration = d.ISMMCLTPR_ProjectDuration;
                                $scope.ISMMCLTPR_MOURefNo = d.ISMMCLTPR_MOURefNo;
                                $scope.ISMMCLTPR_MOUDate = d.ISMMCLTPR_MOUDate;
                                $scope.ISMMCLTPR_ProposalRefNo = d.ISMMCLTPR_ProposalRefNo;
                                $scope.ISMMCLTPR_EnhancementPerYr = d.ISMMCLTPR_EnhancementPerYr;
                                $scope.ISMMCLTPR_MOUStartDate = d.ISMMCLTPR_MOUStartDate;
                                $scope.ISMMCLTPR_MOUEndDate = d.ISMMCLTPR_MOUEndDate;
                                $scope.ISMMCLT_Contract_YearTo = d.ISMMCLT_Contract_YearTo;
                                $scope.ISMMCLT_Contract_YearFrom = d.ISMMCLT_Contract_YearFrom;
                                $scope.ISMMCLTPR_NodalOfficerName = d.ISMSLE_ContactPerson;
                                $scope.ISMMCLTPR_NodalOfficerEmailId = d.ISMSLE_EmailId;
                                $scope.ISMMCLTPR_NodalOfficerContactNo = d.ISMSLE_ContactNo;
                                $scope.ISMMCLTPR_NodalOfficerContactNo = d.ISMSLE_ContactNo;
                                $scope.ISMSMOUA_ProductName = d.ISMSMPR_ProductName;
                                $scope.ISMMCLTPR_TotalStudent = d.ISMSLE_StudentStrength;
                                $scope.ISMMCLTPR_CostStudent = d.Negotiation_Amount;
                                $scope.ISMSMOU_PackageName = d.ISMSBOM_BOMName;
                                $scope.ISMMCLTPR_ProposalRefNo = d.FMSCOR_RefernceNo;
                                //$scope.ISMMCLTPR_CostPerStudent = d.Negotiation_Amount;
                                $scope.ISMSLE_LeadName = d.ISMSLE_LeadNameNew;
                                $scope.ISMSLE_LeadAddress1 = d.ISMSLE_LeadAddress1;
                                $scope.ISMSLE_LeadAddress2 = d.ISMSLE_LeadAddress2;
                                $scope.ISMSLE_LeadAddress3 = d.ISMSLE_LeadAddress3;
                                $scope.ISMSLE_Pincode = d.ISMSLE_Pincode;
                                $scope.ISMSLE_ContactDesignation = d.ISMSLE_ContactDesignation;
                            }
                        });

                        $scope.OnChangeBOMList();
                    }
                    $('#popup111').modal('show');
                }
                else {
                    e1.html('');
                    swal("No Template Found");
                }
            })

        }
        $scope.bindtemplate = []; $scope.xtra = "";

        $scope.loadtemplate = function () {

            $scope.ISMSLE_Id = 0;
            //$scope.ISMMCLTPR_NodalOfficerName = ""; $scope.ISMMCLTPR_NodalOfficerEmailId = ""; $scope.ISMMCLTPR_NodalOfficerContactNo = ""; $scope.ISMMCLTPR_NodalOfficerContactNo = "";
            //$scope.ISMSMOUA_ProductName = ""; $scope.ISMMCLTPR_TotalStudent = ""; $scope.ISMSMOU_PackageName = ""; $scope.ISMMCLTPR_ProposalRefNo = "";
            //$scope.ISMMCLTPR_CostPerStudent = ""; $scope.ISMMCLTPR_CostPerStudent = ""; $scope.ISMSLE_LeadName = ""; $scope.ISMSLE_LeadAddress1 = "";
            //$scope.ISMSLE_LeadAddress2 = ""; $scope.ISMSLE_LeadAddress3 = ""; $scope.ISMSLE_Pincode = ""; $scope.ISMSLE_ContactDesignation = "";
            var ISMSBOM_Id = 0; var ISMSLE_ID = 0;

            angular.forEach($scope.mompendinglist, function (d) {
                if (d.ISMSLET_Id == $scope.ISMSLET_Id.ISMSLET_Id) {
                    ISMSBOM_Id = d.ISMSBOM_Id;
                    ISMSLE_ID = d.ISMSLE_Id;
                }

            });
            $scope.ISMSLE_Id = ISMSLE_ID;
            var items = {
                "pending": $scope.Template,
                "msg": "FetchTemp",
                "ISMSBOM_Id": ISMSBOM_Id,
                "ISMSLE_Id": ISMSLE_ID
            }
            apiService.create("ISM_ClientProject_Mapping/viewtemplatedetails", items).then(function (promise) {
                var e1 = angular.element(document.getElementById("bindtemplate"));
                if (promise.templatelist != null && promise.templatelist.length > 0) {

                    $scope.bindtemplate = promise.templatelist;
                    $scope.xtra = $scope.bindtemplate[0].VMSMOUT_Template;
                    e1.html($scope.xtra);
                    $compile(e1.contents())($scope);

                    $scope.ModuleName = promise.moduleName;

                    $scope.leadprojectpayment = promise.leadprojectpayment;

                    $scope.groupedData = [];
                    $scope.leadprojectpayment.forEach(item => {
                        // Check if the YearFlag already exists in groupedData
                        let yearFlagGroup = $scope.groupedData.find(group => group.YearFlag === item.YearFlag);

                        if (!yearFlagGroup) {
                            // If not found, create a new group for this YearFlag
                            yearFlagGroup = {
                                YearFlag: item.YearFlag,
                                records: []
                            };
                            $scope.groupedData.push(yearFlagGroup);
                        }
                        // Push the current item into the appropriate yearFlagGroup
                        yearFlagGroup.records.push(item);
                    });

                    if (promise.momproductpendinglist != null && promise.momproductpendinglist.length > 0) {
                        angular.forEach(promise.momproductpendinglist, function (d) {
                            $scope.bomListNew.push({ ISMSBOM_Id: d.ISMSBOM_Id });
                        });
                        angular.forEach(promise.momproductpendinglist, function (d) {
                            if (d.ISMSLE_Id == ISMSLE_ID) {

                                $scope.ISMMCLTPR_NodalOfficerName = d.ISMSLE_ContactPerson;
                                $scope.ISMMCLTPR_NodalOfficerEmailId = d.ISMSLE_EmailId;
                                $scope.ISMMCLTPR_NodalOfficerContactNo = d.ISMSLE_ContactNo;
                                $scope.ISMMCLTPR_NodalOfficerContactNo = d.ISMSLE_ContactNo;
                                $scope.ISMSMOUA_ProductName = d.ISMSMPR_ProductName;
                                $scope.ISMMCLTPR_TotalStudent = d.ISMSLE_StudentStrength;
                                $scope.ISMMCLTPR_CostStudent = d.Negotiation_Amount;
                                $scope.ISMSMOU_PackageName = d.ISMSBOM_BOMName;
                                $scope.ISMMCLTPR_ProposalRefNo = d.FMSCOR_RefernceNo;
                                // $scope.ISMMCLTPR_CostPerStudent = d.Negotiation_Amount;
                                $scope.ISMSLE_LeadName = d.ISMSLE_LeadNameNew;
                                $scope.ISMSLE_LeadAddress1 = d.ISMSLE_LeadAddress1;
                                $scope.ISMSLE_LeadAddress2 = d.ISMSLE_LeadAddress2;
                                $scope.ISMSLE_LeadAddress3 = d.ISMSLE_LeadAddress3;
                                $scope.ISMSLE_Pincode = d.ISMSLE_Pincode;
                                $scope.ISMSLE_ContactDesignation = d.ISMSLE_ContactDesignation;

                            }
                        });

                      
                    }
                    $scope.OnChangeBOMList();
                    $("#templatemodel").modal('show');
                }
                else {
                    e1.html('');
                    swal("No Template Found");
                }
            })
        }
        //$scope.print = function () {
        //    var innercontents = "";
        //    innercontents = document.getelementbyid("Baldwin12").innerhtml;
        //    var popupwinindow = window.open('');
        //    popupwinindow.document.open();
        //    popupwinindow.document.write('<html><head>' +
        //        '<style type="text/css">' +
        //        '@page  {' +
        //        'margin: 70px;' +
        //        'margin-top: 150px;' + 
        //        'margin-bottom: 125px;' +
        //        'content: counter(page);' +
        //        '}' +

        //        'body {' +
        //        'text-align: justify;' + 
        //        '}' +
        //        'ol li{' +
        //        'line-height: 1;' + 
        //        '}' +
        //        '</style>' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf_bootstrap.css" />' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/cumulativereportpdf.css" />' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
        //        '</head><body onload="window.print()" onfocus="window.settimeout(function() { window.close(); }, 100);">' + innercontents + '</body></html>');
        //    popupwinindow.document.close();
        //};


        //$scope.print = function () {
        //    var innerContents = "";
        //    innerContents = document.getElementById("Baldwin12").innerHTML;
        //    var popupWinindow = window.open('');
        //    popupWinindow.document.open();
        //    popupWinindow.document.write('<html><head>' +
        //        '<style type="text/css">' +
        //        '@page {' +
        //        'margin: 70px;' +
        //        '}' +
        //        '@page :first {' +
        //        'margin-top: 150px;' +
        //        'margin-bottom: 150px;' +
        //        'content: counter(page);' +
        //        '}' +
        //        'body {' +
        //        'text-align: justify;' + 
        //        '}' +
        //        'ol li{' +
        //        'line-height: 1.5;' + 
        //        '}' +
        //        '</style>' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
        //        '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</body></html>');
        //    popupWinindow.document.close();
        //};


        $scope.printDatas = function () {
            var innerContents = "";
            innerContents = document.getElementById("bindtemplate").innerHTML;
            var ISMSLE_LeadName = $scope.ISMSLE_LeadName;
            var ISMSLE_ContactDesignation = $scope.ISMSLE_ContactDesignation;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<style type="text/css">' +
                '@page {' +
                '    margin-top: 150px;' +
                '    margin-bottom: 220px;' +
                '    margin: 100px;' +
                '}' +
                '@page :first {' +
                '    margin-top: 150px;' +
                '    margin-bottom: 180px;' +
                '    content: counter(page);' +
                '}' +
                'body {' +
                '    text-align: justify;' +
                '}' +
                'ol li {' +
                '    line-height: 1.5;' +
                '}' +
                '@page {' +
                '    @top-right {' +
                '        content: counter(page);' +
                '        white-space: pre;' +
                '        font-size: 12px;' +
                '        font-weight: normal;' +
                '    }' +
                '    @bottom-left {' +
                '         content: "\\A\\A For VAPS Technosoft Pvt. Ltd\\AManaging Director (Sign & Seal)\\A\\A\\A\\A"; ' +
                '        white-space: pre;' +
                '        font-size: 11px;' +
                '        line-height: 2.5;' +
                '        font-weight: bold;' +
                '    }' +
                // Dynamically insert ISMSLE_LeadName and ISMSLE_ContactDesignation  here using template literals:
                '    @bottom-right {' +
                '          content: "\\A\\A For ' + ISMSLE_LeadName + '\\A ' + ISMSLE_ContactDesignation + '(Sign & Seal)\\A\\A\\A\\A";' +
                '        white-space: pre;' +
                '        font-size: 11px;' +
                '        line-height: 2.5;' +
                '        font-weight: bold;' +
                '    }' +
                '}' +
                '</style>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</body></html>');
            popupWinindow.document.close();
        };


        $scope.printData11 = function () {
            var innerContents = "";
            innerContents = document.getElementById("asdf").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '<style>@media print { img { border: none !important; } }</style>' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' +
                innerContents +
                '</body></html>');
            popupWinindow.document.close();
        };

        $scope.printOrderAcceptance = function () {
            var innerContents = "";
            innerContents = document.getElementById("asdfMou").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '<style>@media print { img { border: none !important; } }</style>' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' +
                innerContents +
                '</body></html>');
            popupWinindow.document.close();
        };

        $scope.print = function () {
            var innerContents = "";
            innerContents = document.getElementById("Baldwin12").innerHTML;
            var ISMSLE_LeadName = $scope.ISMSLE_LeadName;
            var ISMSLE_ContactDesignation = $scope.ISMSLE_ContactDesignation;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<style type="text/css">' +
                '@page {' +
                '    margin-top: 150px;' +
                '    margin-bottom: 220px;' +
                '    margin: 100px;' +
                '}' +

                'body {' +
                '    text-align: justify;' +
                '}' +
                'ol li {' +
                '    line-height: 1.5;' +
                '}' +
                '@page {' +
                '    @top-right {' +
                '        content: counter(page);' +
                '        white-space: pre;' +
                '        font-size: 12px;' +
                '        font-weight: normal;' +
                '    }' +
                '    @bottom-left {' +
                '         content: "\\A\\A For VAPS Technosoft Pvt. Ltd\\AManaging Director (Sign & Seal)\\A\\A\\A\\A"; ' +
                '        white-space: pre;' +
                '        font-size: 15px;' +
                '        line-height: 2.5;' +
                '        font-weight: bold;' +
                '    }' +
                // Dynamically insert ISMSLE_LeadName and ISMSLE_ContactDesignation  here using template literals:
                '    @bottom-right {' +
                '          content: "\\A\\A For ' + ISMSLE_LeadName + '\\A ' + ISMSLE_ContactDesignation + '(Sign & Seal)\\A\\A\\A\\A";' +
                '        white-space: pre;' +
                '        font-size: 15px;' +
                '        line-height: 2.5;' +
                '        font-weight: bold;' +
                '    }' +
                '}' +
                '</style>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</body></html>');
            popupWinindow.document.close();
        };


        //=====================Adding and removing new row in transcation Grid============      
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        if ($scope.transrows.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addOBrows = function () {
            $scope.submitted = true;
            //if ($scope.myForm.$valid) {
            if ($scope.transrows.length > 1) {
                for (var i = 0; i === $scope.transrows.length; i++) {
                    var id = $scope.transrows[i].itrS_Id;
                    var lastChar = id.substr(id.length - 1);
                    $scope.cnt = parseInt(lastChar);
                }
            }
            $scope.cnt = $scope.cnt + 1;
            $scope.tet = 'trans' + $scope.cnt;
            var newItemNo = $scope.cnt;
            $scope.transrows.push({ 'itrS_Id': 'trans' + newItemNo });

        };
        $scope.removeOBrows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);
            if (data.amstB_Id > 0) {
                $scope.Deletegrnrows(data);
            }
        };
        $scope.bomListNew = [];
        //added  By 
        $scope.clearFunction = function () {
            $scope.ISMSLE_Id = 0;
            $scope.ISMMCLTPR_EnhancementPerYr = "";
            $scope.ISMMCLTPR_NodalOfficerName = ""; $scope.ISMMCLTPR_NodalOfficerEmailId = ""; $scope.ISMMCLTPR_NodalOfficerContactNo = ""; $scope.ISMMCLTPR_NodalOfficerContactNo = "";
            $scope.ISMSMOUA_ProductName = ""; $scope.ISMMCLTPR_TotalStudent = ""; $scope.ISMSMOU_PackageName = ""; $scope.ISMMCLTPR_ProposalRefNo = "";
            $scope.ISMMCLTPR_CostPerStudent = ""; $scope.ISMMCLTPR_CostPerStudent = ""; $scope.ISMSLE_LeadName = ""; $scope.ISMSLE_LeadAddress1 = "";
            $scope.ISMSLE_LeadAddress2 = ""; $scope.ISMSLE_LeadAddress3 = ""; $scope.ISMSLE_Pincode = ""; $scope.ISMSLE_ContactDesignation = ""; $scope.ISMMCLTPR_MOURefNo = "";
        }
        $scope.OnChangeBOMList = function (edits) {
            // $scope.bomList = [];
            $scope.amt = "";
            $scope.amt_4 = "";
            $scope.obj.ISMSLET_Amount_I = "";
            $scope.obj.ISMSLET_Amount_Final = "";
            var BOMLists = []; $scope.sales_CostPerMonth = []; $scope.bomTemplateList = [];
            if ($scope.bomListNew != null && $scope.bomListNew.length > 0) {
                angular.forEach($scope.bomListNew, function (d) {
                    BOMLists.push(d.ISMSBOM_Id);
                });
            }
            var data = {
                "BOMListt": BOMLists,
                "ISMSLE_Id": $scope.ISMSLE_Id,
            };
            apiService.create("ISM_ClientProject_Mapping/OnChangeOnChangeBOMList", data).then(function (promise) {

                if (promise.bomTemplateList != null && promise.bomTemplateList.length > 0) {
                    $scope.bomTemplateList = promise.bomTemplateList;
                    angular.forEach($scope.bomTemplateList, function (d) {
                        if (d.ISPSIF_ItemsName === 'SMS Pack') {
                            $scope.ISPSIF_Quantity = d.ISPSIF_Quantity;
                        } else if (d.ISPSIF_ItemsName === 'Smart Cards') {
                            $scope.ISPSIF_CARDQuantity = d.ISPSIF_Quantity;
                        }
                        else if (d.ISPSIF_ItemsName === 'Biometric Face Reader  and Fingerprint Machine') {
                            $scope.ISPSIF_FaceQuantity = d.ISPSIF_Quantity;
                        }
                        else if (d.ISPSIF_ItemsName === 'Touch Screen Kiosk Machine') {
                            $scope.ISPSIF_kioskQuantity = d.ISPSIF_Quantity;
                        }
                        else if (d.ISPSIF_ItemsName === 'Vertical Digital Signage') {
                            $scope.ISPSIF_VDSkQuantity = d.ISPSIF_Quantity;
                        }

                    });
                    // $scope.smsamt = promise.bomTemplateList[3].ISPSIF_Quantity;
                    $scope.sales_CostPerMonth = promise.sales_CostPerMonth;
                    $scope.taskid5 = [];

                    angular.forEach($scope.bomTemplateList, function (dev) {

                        if ($scope.taskid5.length === 0) {
                            $scope.taskid5.push({
                                ISMSPS_ID: dev.ISMSPS_ID,
                                ISMSPS_Service_Name: dev.ISMSPS_Service_Name,
                                ISMSPSI_ID: dev.ISMSPSI_ID,
                            });
                        }
                        else if ($scope.taskid5.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.taskid5, function (emp) {
                                if (emp.ISMSPS_ID === dev.ISMSPS_ID) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.taskid5.push({
                                    ISMSPS_ID: dev.ISMSPS_ID,
                                    ISMSPS_Service_Name: dev.ISMSPS_Service_Name,
                                    ISMSPSI_ID: dev.ISMSPSI_ID,
                                });
                            }
                        }
                    });
                    angular.forEach($scope.taskid5, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.bomTemplateList, function (dd) {
                            if (dd.ISMSPS_ID === ddd.ISMSPS_ID) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.bomTemplateList = $scope.templist;
                    });
                    $scope.asmaY_Year = $scope.bomTemplateList[0].ASMAY_Year;
                    //added By sanjeev 
                    if ($scope.ismsboM_BOMName == "HARDWARE") {
                        $scope.amt = 361200;
                        $scope.obj.ISMSLET_Amount_I = 361200;
                    }
                    else {
                        $scope.amt = $scope.sales_CostPerMonth[0].AMt;

                        if ($scope.ismsmoU_PackageName != 'Delite  Package') {
                            $scope.amt_4 = $scope.sales_CostPerMonth[0].AMt_4;

                        }
                        else {
                            $scope.amt_4 = 0;
                            $scope.obj.ISMSLET_Amount_Final = 0;
                        }
                        if ($scope.bomTemplateList[0].Negotiation_Amount > 0) {
                            $scope.ISMMCLTPR_CostPerStudent = $scope.bomTemplateList[0].Negotiation_Amount;
                        }
                        else {
                            $scope.ISMMCLTPR_CostPerStudent = $scope.amt;
                        }

                    }
                } else {
                    swal('No Data Found ');
                    $state.reload();
                }
            });
        };
        $scope.materaldocuupload = [];
        $scope.materaldocuupload = [{ id: 'materal' }];
        //added by 
        $scope.SendDataview = function () {
            $scope.taxdto = [];//$scope.ISMMCLTPR_NodalOfficerContactNo
            var msg = document.getElementById("asdfMou").innerHTML;          
            var data = {        
                "msg": msg, 
              //  "ISMMCLTPR_NodalOfficerEmailId": $scope.ISMMCLTPR_NodalOfficerEmailId,
               // "ISMMCLTPR_NodalOfficerContactNo": $scope.ISMMCLTPR_NodalOfficerContactNo,
                "ISMMCLTPR_NodalOfficerEmailId": "praveenkumar.b@vapstech.com",
                "ISMMCLTPR_NodalOfficerContactNo": 6383077734,
            };
            apiService.create("ISM_ClientProject_Mapping/sendMail", data).then(function (promise) {
                swal("Email Sent SucessFully ! ");
                $('#OrderAcceptance').modal('hide');               
               $scope.clearFunction();

            });
           
        }
        //added By
        $scope.selectFileforUploadzd = function (input, document) {
            $scope.SelectedFileForUploadzd = input.files;
            $scope.vbabrsF_FileName = input.files[0].name;
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
            console.log("check Book upload  Upload  :" + data);
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
                    data.vbabrsF_FilePath = d;
                    data.vbabrsF_FileName = $scope.vbabrsF_FileName;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }
    }
})();
//======================================================================================================//
