
(function () {
    'use strict';
    angular
        .module('app')
        .controller('HR_Admin_NOC_ApplyController', HR_Admin_NOC_ApplyController);
    HR_Admin_NOC_ApplyController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce']
    function HR_Admin_NOC_ApplyController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce) {

        $scope.ISMCERTREQ_RequestDate = new Date();


        $scope.obj = {};
        $scope.editflag = false;
        $scope.showemployeedetails = false;
        $scope.itemsPerPage = 10;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings!=null && ivrmcofigsettings.length > 0) {
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
            apiService.getURI("HR_NOC_Process/getadminloaddata", pageid).then(function (promise) {
                if (promise.employees != null && promise.employees.length > 0) {
                    $scope.employees = promise.employees;
                }

                if (promise.display_data != null && promise.display_data.length > 0) {
                    $scope.get_data = promise.display_data;
                    $scope.presentCountgrid = $scope.get_data.length;
                }
                if (promise.document != null && promise.document.length > 0) {
                    $scope.documentlist = promise.document;
                }
                $scope.ISMCERTREQAPP_ReceiningDate = 0;
                if (promise.ismcertreqapP_ReceiningDate != null && promise.ismcertreqapP_ReceiningDate.length > 0) {
                    $scope.ISMCERTREQAPP_ReceiningDate = promise.ismcertreqapP_ReceiningDate;
                }
                if (promise.ismcertreQ_FilePath != null && promise.ismcertreQ_FilePath.length > 0) {
                    $scope.ISMCERTREQAPP_ACK = promise.ismcertreQ_FilePath;
                }
                $scope.pagePrivilege = [];
                if (promise.pagePrivilege !== null && promise.pagePrivilege.length > 0) {
                    $scope.pagePrivilege = promise.pagePrivilege;
                    $scope.IVRMRP_AddFlag = $scope.pagePrivilege[0].IVRMRP_AddFlag;
                    $scope.IVRMRP_UpdateFlag = $scope.pagePrivilege[0].IVRMRP_UpdateFlag;
                    $scope.IVRMRP_DeleteFlag = $scope.pagePrivilege[0].IVRMRP_DeleteFlag;
                    $scope.IVRMRP_ReportFlag = $scope.pagePrivilege[0].IVRMRP_ReportFlag;
                    $scope.IVRMRP_ProcessFlag = $scope.pagePrivilege[0].IVRMRP_ProcessFlag;

                }
                if (promise.hrmE_DOJ != null && promise.hrmE_DOJ.length > 0 && promise.hrmE_DOJ != null) {
                    $scope.HRME_DOJ = promise.hrmE_DOJ;
                    var dt1 = new Date($scope.HRME_DOJ);
                    var dt2 = new Date();

                    //2 years condition is removed
                    $scope.eligibilitymonths = 25;

                    //var diff = (dt2.getTime() - dt1.getTime()) / 1000;
                    //diff /= (60 * 60 * 24 * 7 * 4);
                    //$scope.eligibilitymonths = Math.abs(Math.round(diff));

                }


                $scope.todate = true;

            })
        };

        //------------------------------------------------------------------------------

        $scope.getcertificate = function (empid) {
            var data = {
                "HRME_Id": empid.hrmE_Id,
            };
            apiService.create("HR_NOC_Process/getcertificate", data).
                then(function (promise) {
                    if (promise.getloaddetails != null && promise.getloaddetails.length > 0 && promise.getloaddetails != undefined) {

                        $scope.Department = promise.getloaddetails[0].HRMD_DepartmentName;
                        $scope.Designation = promise.getloaddetails[0].HRMDES_DesignationName;
                        $scope.mI_Name = promise.getloaddetails[0].MI_Name;
                        //  $scope.ismresG_Remarks = promise.getloaddetails[0].ismresG_Remarks;
                        $scope.employeename = promise.getloaddetails[0].employeename;
                        //$scope.ismresG_RegRelFlag = promise.getloaddetails[0].ismresG_RegRelFlag;
                        // $scope.hrmE_Id = promise.getloaddetails[0].hrmE_Id;
                        $scope.HRME_LeftFlag = promise.getloaddetails[0].HRME_LeftFlag;
                        $scope.HRME_DOL = promise.getloaddetails[0].HRME_DOL;
                        $scope.HRME_Photo = promise.getloaddetails[0].HRME_Photo;
                        $scope.DateOfJoining = promise.getloaddetails[0].HRME_DOJ;
                        $scope.HRME_LeavingReason = promise.getloaddetails[0].HRME_LeavingReason;
                        $scope.HRME_EmailId = promise.getloaddetails[0].HRME_EmailId;
                        $scope.HRMEMNO_MobileNo = promise.getloaddetails[0].HRMEMNO_MobileNo;
                    }
                    else {
                        $state.reload();

                    }
                    if (promise.display_data != null && promise.display_data.length > 0 && promise.display_data != undefined) {
                        $scope.get_data = promise.display_data;
                        $scope.presentCountgrid = $scope.get_data.length;
                    }
                    else {
                        $scope.get_data = [];

                    }
                   
                    $scope.ISMCERTREQAPP_ReceiningDate = 0;
                    if (promise.ismcertreqapP_ReceiningDate != null && promise.ismcertreqapP_ReceiningDate.length > 0 && promise.ismcertreqapP_ReceiningDate != undefined) {
                        $scope.ISMCERTREQAPP_ReceiningDate = promise.ismcertreqapP_ReceiningDate;
                    }
                    if (promise.ismcertreQ_FilePath != null && promise.ismcertreQ_FilePath.length > 0 && promise.ismcertreQ_FilePath != undefined) {
                        $scope.ISMCERTREQAPP_ACK = promise.ismcertreQ_FilePath;
                    }
                    if (promise.hrmE_DOJ != null && promise.hrmE_DOJ.length > 0) {
                        $scope.HRME_DOJ = promise.hrmE_DOJ;
                        var dt1 = new Date($scope.HRME_DOJ);
                        var dt2 = new Date();

                         //2 years condition is removed
                        $scope.eligibilitymonths = 25;



                        //var diff = (dt2.getTime() - dt1.getTime()) / 1000;
                        //diff /= (60 * 60 * 24 * 7 * 4);
                        //$scope.eligibilitymonths = Math.abs(Math.round(diff));

                    }


                    $scope.todate = true;

                });
        }


        //---------------------------------Save--------------------------------------------

        $scope.savedata = function () {
            $scope.submitted = true;
            $scope.from_date = new Date($scope.ISMCERTREQ_RequestDate).toDateString();
            $scope.files_paths = [];
            var ismcertreQ_FileName = "";
            if ($scope.ismcertreQ_FileName != null && $scope.ismcertreQ_FileName != undefined) {
                ismcertreQ_FileName = $scope.ismcertreQ_FileName
            }
            var ismcertreQ_FilePath = "";
            if ($scope.ismcertreQ_FilePath != null && $scope.ismcertreQ_FilePath != undefined) {
                ismcertreQ_FilePath = $scope.ismcertreQ_FilePath
            }


            if ($scope.myForm.$valid) {
                var data = {
                    "ISMCERTREQ_Id": $scope.ismcertreQ_Id,
                    "HRME_Id": $scope.obj.hrmE_Id.hrmE_Id,
                    "ISMCERTREQ_RequestDate": $scope.from_date,
                    //"ISMCERTREQ_CertificateName": $scope.ISMCERTREQ_CertificateName,
                    "ISMCERTREQ_Remarks": $scope.obj.ISMCERTREQ_Remarks,
                    "ISMCERTREQ_ReceivingMode": $scope.obj.ISMCERTREQ_ReceivingMode,
                    "ISMCERTREQ_FileName": ismcertreQ_FileName,
                    "ISMCERTREQ_FilePath": ismcertreQ_FilePath,
                }
                apiService.create("HR_NOC_Process/saveadmindetails", data).then(function (promise) {
                    if (promise.returnval == true) {
                        if (promise.ismcertreQ_Id == 0 || promise.ismcertreQ_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.ismcertreQ_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else if (promise.returnduplicatestatus == 'Applied') {
                        swal('Applied already');
                    }
                    else if (promise.returnduplicatestatus == 'AuthoritySetting') {
                        swal('Approval Authorization Is Not Set To This Employee');
                    }
                    else {
                        if (promise.ismcertreQ_Id == 0 || promise.v < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.ismcertreQ_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $state.reload();
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

            apiService.create("HR_NOC_Process/Edit", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getloaddetails != null && promise.getloaddetails.length > 0 && promise.getloaddetails != undefined) {
                        $scope.getloaddetails = promise.getloaddetails;
                        $scope.editflag = true;
                        $scope.ismcertreQ_Id = promise.getloaddetails[0].ismcertreQ_Id;
                        $scope.ISMCERTREQ_CertificateName = promise.getloaddetails[0].ismcertreQ_CertificateName;
                        $scope.obj.ISMCERTREQ_ReceivingMode = promise.getloaddetails[0].ismcertreQ_ReceivingMode;
                        $scope.ISMCERTREQ_RequestDate = new Date(promise.getloaddetails[0].ismcertreQ_RequestDate);
                        $scope.obj.ISMCERTREQ_Remarks = promise.getloaddetails[0].ismcertreQ_Remarks;

                        if ($scope.employees != null && $scope.employees.length > 0) {                            for (var i = 0; i < $scope.employees.length; i++) {                                if (promise.getloaddetails[0].hrmE_Id == $scope.employees[i].hrmE_Id) {                                    $scope.employees.Selected = true;                                    $scope.obj.hrmE_Id = $scope.employees[i];                                    $scope.newcaste = $scope.employees[0].hrmE_Id;                                    // return;                                }                            }                        }


                        //$scope.obj.hrmE_Id = promise.getloaddetails[0].hrmE_Id;
                      
                        $scope.newcaste = {};
                        if ($scope.employees != null && $scope.employees.length > 0) {
                            for (var i = 0; i < $scope.employees.length; i++) {
                                if (promise.getloaddetails[0].hrmE_Id == $scope.employees[i].hrmE_Id) {
                                    $scope.employees[i].Selected = true;
                                    $scope.hrmE_Id = $scope.employees[i];
                                    $scope.newcaste = $scope.employees[0].hrmE_Id;
                                    // return;
                                }
                            }
                        }

                        $scope.showemployeedetails = false;
                        $scope.getcertificate(item);
                    }


                    if (promise.getloaddetails[0].ismcertreQ_FilePath != null && promise.getloaddetails[0].ismcertreQ_FilePath.length > 0) {
                        $scope.ismcertreQ_FilePath = promise.getloaddetails[0].ismcertreQ_FilePath;

                    }
                    

                    if (promise.getloaddetails[0].ismcertreQ_FileName.length != null || promise.getloaddetails[0].ismcertreQ_FileName.length != '') {
                        $scope.ismcertreQ_FileName = promise.getloaddetails[0].ismcertreQ_FileName;
                    }
                    



                }
            });
        }


        $scope.viewaproval = function (ismcertreQ_Id) {
            var data = {
                "ISMCERTREQ_Id": ismcertreQ_Id
            };
            apiService.create("HR_NOC_Process/Viewdata", data).
                then(function (promise) {
                    if (promise !== null && promise.modalview.length > 0) {
                        $scope.modalview = promise.modalview;
                        $('#mymodalviewdetailssecondtab').modal('show');
                    }
                    else {
                        swal(" First Level is Not Approved  !");
                        $('#mymodalviewdetailssecondtab').modal('hide');
                    }
                });
        };



        $scope.GetAllRelDatagrid = function (user) {
            $scope.getcertificatedata = [];
            $scope.aprovedlist = [];    
            $scope.display_data = [];    
            $scope.ISMCERTREQAPP_ACK = "";
            $scope.ISMCERTDISDET_ReceivingFromDate = "";
            $scope.ISMCERTDISDET_ReceivedateTime = "";
            $scope.HRME_Photo ="";
            $scope.ismcertreQ_AppRejFlag = user.ismcertreQ_AppRejFlag;
            $scope.hrmE_Id = user.hrmE_Id;
            var data = {
                "HRME_Id": user.hrmE_Id,
                "ISMCERTREQ_Id": user.ismcertreQ_Id,
            };
            apiService.create("HR_NOC_Process/viewuploadflies", data).then(function (promise) {
                $scope.ISMCERTREQ_Id = user.ismcertreQ_Id
                if (promise.document != null && promise.document.length > 0) {
                    $scope.getcertificatedata = promise.document;
                    //$scope.ISMCERTREQAPP_Remarks = promise.getcertificatedata[0].ISMCERTREQAPP_Remarks;
                    // $scope.ISMCERTREQAPP_AppRejFlag = promise.getcertificatedata[0].ISMCERTREQAPP_AppRejFlag;
                }
                if (promise.aprovedlist != null && promise.aprovedlist.length > 0) {
                    $scope.aprovedlist = promise.aprovedlist;
                }
                if (promise.ismcertreqapP_Remarks != null && promise.ismcertreqapP_Remarks.length > 0) {
                    $scope.ISMCERTREQ_Remarks = promise.ismcertreqapP_Remarks;
                }
                if (promise.roleType != null && promise.roleType.length > 0) {
                    $scope.roleType = promise.roleType;
                }
                if (promise.viewlist != null && promise.viewlist.length > 0) {
                    $scope.viewlist = promise.viewlist;
                    $scope.HRPAON_SanctionLevelNo = $scope.viewlist[0].hrpaoN_SanctionLevelNo;
                    $scope.HRPAON_FinalFlg = $scope.viewlist[0].hrpaoN_FinalFlg;
                }
                if (promise.display_data != null && promise.display_data.length > 0) {
                    $scope.display_data = promise.display_data;
                    $scope.ISMCERTDISDET_ReceivedMode = $scope.display_data[0].ISMCERTDISDET_ReceivedMode;
                    $scope.ISMCERTDISDET_ReceivedateTime = $scope.display_data[0].ISMCERTDISDET_ReceivedateTime;
                    $scope.ISMCERTDISDET_FineAmount = $scope.display_data[0].ISMCERTDISDET_FineAmount;
                    $scope.ISMCERTDISDET_FileName = $scope.display_data[0].ISMCERTDISDET_FileName;
                    $scope.ISMCERTDISDET_Remarks = $scope.display_data[0].ISMCERTDISDET_Remarks;
                    $scope.ISMCERTDISDET_ReceivingFromDate = $scope.display_data[0].ISMCERTDISDET_ReceivingFromDate;
                    $scope.ISMCERTDISDET_ReceivingToDate = $scope.display_data[0].ISMCERTDISDET_ReceivingToDate;
                    $scope.ISMCERTREQAPP_ApprovedDate = $scope.display_data[0].ISMCERTREQAPP_ApprovedDate;
                    $scope.ISMCERTREQAPP_ReceiningDate = $scope.display_data[0].ISMCERTREQAPP_ReceiningDate;
                    $scope.ISMCERTREQAPP_Remarks = $scope.display_data[0].ISMCERTREQAPP_Remarks;
                    $scope.ISMCERTREQAPP_FinalFlg = $scope.display_data[0].ISMCERTREQAPP_FinalFlg;
                    $scope.ISMCERTREQAPP_AppRejFlag = $scope.display_data[0].ISMCERTREQAPP_AppRejFlag;
                    $scope.ISMCERTREQAPP_RecivingTime = $scope.display_data[0].ISMCERTREQAPP_RecivingTime;
                    $scope.ISMCERTREQAPP_ACK = $scope.display_data[0].ISMCERTREQAPP_ACK;
                    $scope.ISMCERTREQ_RequestDate = $scope.display_data[0].ISMCERTREQ_RequestDate;
                    $scope.ISMCERTREQ_Remarks = $scope.display_data[0].ISMCERTREQ_Remarks;
                    $scope.ISMCERTREQ_ReceivingMode = $scope.display_data[0].ISMCERTREQ_ReceivingMode;
                    $scope.ISMCERTREQ_FileName = $scope.display_data[0].ISMCERTREQ_FileName;
                    $scope.ISMCERTREQ_FilePath = $scope.display_data[0].ISMCERTREQ_FilePath;
                    $scope.ISMCERTREQ_RecivingAddress = $scope.display_data[0].ISMCERTREQ_RecivingAddress;
                    $scope.AuthorisedEmployee = $scope.display_data[0].AuthorisedEmployee;
                }

                if (promise.employees != null && promise.employees.length > 0) {
                    $scope.employees = promise.employees;
                }

                if (promise.getloaddetails != null && promise.getloaddetails.length > 0) {
                    $scope.DateOfJoining = promise.getloaddetails[0].HRME_DOJ;
                    $scope.Department = promise.getloaddetails[0].HRMD_DepartmentName;
                    $scope.Designation = promise.getloaddetails[0].HRMDES_DesignationName;
                    $scope.mI_Name = promise.getloaddetails[0].MI_Name;
                    //  $scope.ismresG_Remarks = promise.getloaddetails[0].ismresG_Remarks;
                    $scope.employeename = promise.getloaddetails[0].employeename;
                    //$scope.ismresG_RegRelFlag = promise.getloaddetails[0].ismresG_RegRelFlag;
                    //$scope.hrmE_Id = promise.getloaddetails[0].hrmE_Id;
                    $scope.HRME_LeftFlag = promise.getloaddetails[0].HRME_LeftFlag;
                    $scope.HRME_DOL = promise.getloaddetails[0].HRME_DOL;
                    $scope.HRME_LeavingReason = promise.getloaddetails[0].HRME_LeavingReason;
                    $scope.HRME_Photo = promise.getloaddetails[0].HRME_Photo;

                    $scope.HRMEMNO_MobileNo = promise.getloaddetails[0].HRMEMNO_MobileNo;
                    $scope.HRME_EmailId = promise.getloaddetails[0].HRME_EmailId;
                    
                    
                }
                if (promise.ismcertreQ_FilePath != null && promise.ismcertreQ_FilePath.length > 0) {
                    $scope.ISMCERTREQAPP_ACK = promise.ismcertreQ_FilePath;
                }
                if (promise.maxmumlevel != null && promise.maxmumlevel > 0) {
                    $scope.maxmumlevel = promise.maxmumlevel - 1;
                }
                if (promise.ismcertreqapP_ReceiningDate != null && promise.ismcertreqapP_ReceiningDate.length > 0) {
                    $scope.ISMCERTREQAPP_ReceiningDate = promise.ismcertreqapP_ReceiningDate;
                }

                $scope.showemployeedetails = true;
              

            });
        };




        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };


        $scope.deactive = function (item, SweetAlert) {            $scope.ISMCERTREQ_Id = item.ismcertreQ_Id;            var dystring = "";            if (item.ismcertreQ_ActiveFlag == true) {                dystring = "Deactivate";            }            else if (item.ismcertreQ_ActiveFlag == false) {                dystring = "Activate";            }            swal({                title: "Are you sure?",                text: "Do You Want To " + dystring + " Record?",                type: "warning",                showCancelButton: true,                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",                cancelButtonText: "Cancel",                closeOnConfirm: false,                closeOnCancel: false            },                function (isConfirm) {                    if (isConfirm) {                        apiService.create("HR_NOC_Process/deactive", item).                            then(function (promise) {                                if (promise.returnval == true) {                                    swal("Record " + dystring + "d Successfully!!!");                                }                                else {                                    swal("Record could not be " + dystring + "d Please try again later!!!");                                }                                $state.reload();                            })                    }                    else {                        swal("Record " + dystring + " Cancelled!!!");                    }                });        }


        $scope.nocdetails = function () {
            var data = {
                "HRME_Id": $scope.hrmE_Id,
            };
            apiService.create("HR_NOC_Process/nocdetails", data).then(function (promise) {
                if (promise.aprovedlist != null && promise.aprovedlist != undefined && promise.aprovedlist.length > 0) {
                    $scope.nocapproveddetails = promise.aprovedlist;
                }
                if (promise.checklist != null && promise.checklist != undefined && promise.checklist.length > 0) {
                    $scope.checklistapprovedlist = promise.checklist;
                }
            });
        };
        $scope.printnoc = function (Baldwin) {
            var innerContents = document.getElementById("printnoccertificate").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Vikasa/VIKASATCREPORT/VIKASATCReportPdf1.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }



        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';




    }

})();