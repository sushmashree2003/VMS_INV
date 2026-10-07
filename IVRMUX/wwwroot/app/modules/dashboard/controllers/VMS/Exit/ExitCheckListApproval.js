(function () {
    'use strict';
    angular.module('app').controller('ExitCheckListApprovalController', ExitCheckListApprovalController)

    ExitCheckListApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$filter', '$timeout']
    function ExitCheckListApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $filter, $timeout) {

        $scope.submitted = false;
        $scope.totalgrid = [];
        $scope.editflag = false;
        $scope.obj = {};
        $scope.hstep = 0;
        $scope.mstep = 0;
        $scope.ReceivingTime = "";

        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("Exit_Employee/loaddata", pageid).
                then(function (promise) {

                    $scope.certificate = promise.cerfificatelist;
                    $scope.getloaddata = promise.aprovedlist;

                })
        };



        $scope.toggleAll = function (all2) {
            var toggleStatus = all2;
            angular.forEach($scope.certificate, function (itm) {
                itm.selected = toggleStatus;
            });
        };


        $scope.viewaproval = function (user) {

            var data = {
                "ISMCERTREQ_Id": user.ISMCERTREQ_Id,
                "HRME_Id": user.HRME_Id
            };
            apiService.create("Exit_Employee/viewuploadflies", data).then(function (promise) {

                $scope.getcertificatedata = promise.document;
            });
        };

        $scope.viewaprovals = function (user) {
            var data = {
                "ISMCERTREQ_Id": user.ismcertreQ_Id
            };
            apiService.create("Exit_Employee/viewuploadflies", data).then(function (promise) {
                $scope.getviewdata = promise.attachment;
            });
        };

        $scope.ShowApprovedDetails = false;

        $scope.GetAllRelDatagrid = function (user) {
            $scope.ShowApprovedDetails = true;
            $scope.GetAllRelData(user);

        }

        $scope.GetAllRelData = function (user) {
            $scope.getcertificatedata = [];
            $scope.aprovedlist = [];
            var data = {
                "HRME_Id": user.HRME_Id,
                "ISMCERTREQ_Id": user.ISMCERTREQ_Id,
            };
            apiService.create("Exit_Employee/viewuploadflies", data).then(function (promise) {
                $scope.ISMCERTREQ_Id = user.ISMCERTREQ_Id
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
                if (promise.getloaddetails != null && promise.getloaddetails.length > 0) {
                    $scope.DateOfJoining = promise.getloaddetails[0].HRME_DOJ;
                    $scope.Department = promise.getloaddetails[0].HRMD_DepartmentName;
                    $scope.Designation = promise.getloaddetails[0].HRMDES_DesignationName;
                    $scope.mI_Name = promise.getloaddetails[0].MI_Name;
                    //  $scope.ismresG_Remarks = promise.getloaddetails[0].ismresG_Remarks;
                    $scope.employeename = promise.getloaddetails[0].employeename;
                    //$scope.ismresG_RegRelFlag = promise.getloaddetails[0].ismresG_RegRelFlag;
                    $scope.hrmE_Id = promise.getloaddetails[0].hrmE_Id;
                    $scope.HRME_LeftFlag = promise.getloaddetails[0].HRME_LeftFlag;
                    $scope.HRME_DOL = promise.getloaddetails[0].HRME_DOL;
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



                //$scope.getloaddata = promise.document;



            });
        };

        $scope.Viewdocumentdata = function (user) {

            var data = {
                "HRMEDS_Id": user.hrmedS_Id
            };
            apiService.create("Exit_Employee/viewuploadflies", data).then(function (promise) {

                $scope.getdocumentdata = promise.viewattachment;
            });
        };

        $scope.approvedreject = function (objjj) {
            if ($scope.myForm.$valid) {
                //var recceivingdate = new Date($scope.obj.ISMCERTREQAPP_ReceiningDate).toString("dd/MM/yyyy");
                var recceivingdate = $scope.obj.ISMCERTREQAPP_ReceiningDate == null ? "" : $filter('date')($scope.obj.ISMCERTREQAPP_ReceiningDate, "dd-MM-yyyy");
                $scope.typevalue = true;
                $scope.approveddetails = [];



                var dsttimee = $scope.obj.ReceivingTime;
                $scope.sresult = $filter('date')(dsttimee, 'HH:mm a');




                var data = {
                    "ISMCERTREQ_Id": $scope.ISMCERTREQ_Id,
                    "ISMCERTREQAPP_ReceiningDate": recceivingdate,
                    "ISMCERTREQAPP_ReceivingTime": $scope.sresult,
                    "ISMCERTREQAPP_Remarks": $scope.obj.ISMCERTREQAPP_Remarks,
                    "ISMCERTREQAPP_AppRejFlag": objjj,
                    "ISMCERTREQ_FilePath": $scope.ismcertreQ_FilePath,
                    "maxmumlevel": $scope.maxmumlevel
                }
                apiService.create("Exit_Employee/approvedrecord", data).then(function (promise) {

                    if (promise.returnval == true) {
                        swal('Record saved successfully');
                        $state.reload();
                    }
                    else if (promise.returnval == false) {
                        swal('Record  Not saved successfully');
                    }


                    //$state.reload();
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

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };


        $scope.search = '';




    }
})();
