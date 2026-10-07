(function () {
    'use strict';
    angular.module('app').controller('LegalUpdateMasterController', LegalUpdateMasterController)

    LegalUpdateMasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter', '$q', '$sce', '$window', '$compile']
    function LegalUpdateMasterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter, $q, $sce, $window, $compile) {

        var paginationformasters;

        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        //***** MASTER QUESTION ******//
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("LegalUpdateMaster/loaddata", pageid).then(function (promise) {
                $scope.CourtType = promise.category1;
                $scope.gridcasedetails = promise.gridcasedetails;
            });
        };
     



        //$scope.TenantTabFlag = false;
        //$scope.tabChanged = function () {

        //    $scope.TenantTabFlag = true;
        //}    

        $scope.interacted = function (field) {
            return $scope.submitted;
        }

        //save added by Rohan
        $scope.submitted = false;
        $scope.savelegalcasedetails = function (user) {
            $scope.submitted = true;
            if ($scope.myFormCD.$valid) {
                var data = {
                    "LCD_CaseType": $scope.LCD_CaseType,
                    "LCD_FillingNumber": $scope.LCD_FillingNumber,
                    "LCD_FillingDate": $scope.LCD_FillingDate,
                    "LCD_RegistrationNumber": $scope.LCD_RegistrationNumber,
                    "LCD_RegistrationDate": $scope.LCD_RegistrationDate,
                    "LCD_CNRNumber": $scope.LCD_CNRNumber,
                    "LCD_NextHearingDate": $scope.LCD_NextHearingDate,
                    "LCD_PartyName": $scope.LCD_PartyName
                };
                apiService.create("LegalUpdateMaster/savelegalcasedetails", data).
                    then(function (promise) {
                        if (promise.returnvales != "") {
                            if (promise.returnvales == "Update") {
                                swal('Record Updated Successfully');
                                $state.reload();
                                $('#mymodalviewlegalcasedetails').modal('hide');
                                //  $scope.cleartabl1();
                            }
                            else if (promise.returnvales == "Add") {
                                swal('Record Saved Successfully');
                                $state.reload();
                                $('#mymodalviewlegalcasedetails').modal('hide');
                                //  $scope.cleartabl1();
                            }
                            else if (promise.returnvales == "false") {
                                swal('Record Not Saved/Updated successfully');
                                $('#mymodalviewlegalcasedetails').modal('hide');
                                // $scope.cleartabl1();
                            }
                            else if (promise.returnvales === "Duplicate") {
                                swal('Category Already Exist');
                                $('#mymodalviewlegalcasedetails').modal('hide');
                                //  $scope.cleartabl1();
                            }

                        }
                        else {
                            swal('Operation Failed');
                        }
                    });

            }
            //else {
            $scope.submitted = true;
            //}
        };


        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        }
        $scope.load6 = function () {
            $scope.documentslist = [];
            $scope.Remarks = "";
            $scope.Ld_DocumentPath = "";
            $scope.LD_DocumentName = "";
        }

        $scope.submitted1 = false;
        $scope.savecasestatus = function () {
            $scope.submitted1 = true;
            if ($scope.myFormCS.$valid) {
                if ($scope.LCS_Id == "") {
                    $scope.LCS_Id = 0;
                }
                var data = {
                    "LCD_Id": $scope.LCD_ID,
                    "LCS_Id": $scope.LCS_Id,
                    "LCS_CaseStatus": $scope.LCS_CaseStatus,
                    "LCS_FirstHearingDate": $scope.LCS_FirstHearingDate,
                    "LCS_DecisionDate": $scope.LCS_DecisionDate,
                    "LCS_NatureofDisposal": $scope.LCS_NatureofDisposal,
                    "LCS_CourtNumberandJudge": $scope.LCS_CourtNumberandJudge
                };
                apiService.create("LegalUpdateMaster/savecasestatus", data).
                    then(function (promise) {
                        if (promise.returnvales != "") {
                            if (promise.returnvales == "Update") {
                                swal('Record Updated Successfully');
                                $scope.cleartabl2();
                                $('#mymodalviewlegalcasedetails').modal('hide');
                                $scope.cleartabl2();
                            }
                            else if (promise.returnvales == "Add") {
                                swal('Record Saved Successfully');
                                $scope.cleartabl2();
                                $('#mymodalviewlegalcasedetails').modal('hide');
                                $scope.cleartabl2();
                            }
                            else if (promise.returnvales == "false") {
                                swal('Record Not Saved/Updated successfully');
                                $scope.cleartabl2();
                                $('#mymodalviewlegalcasedetails').modal('hide');
                                $scope.cleartabl2();
                            }
                            else if (promise.returnvales === "Duplicate") {
                                swal('Category Already Exist');
                                $scope.cleartabl2();
                                $('#mymodalviewlegalcasedetails').modal('hide');

                                $scope.cleartabl2();
                            }
                        }
                        else {
                            swal('Operation Failed');

                        }
                    });
            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.interacted3 = function (field) {
            return $scope.submitted3;
        }

        $scope.submitted3 = false;
        $scope.saveadvocate = function (user1) {
            if ($scope.LA_Id == "") {
                $scope.LA_Id = 0;
            }
            $scope.submitted3 = true;
            if ($scope.myFormAD.$valid) {
                var data = {
                    "LCD_Id": $scope.LCD_ID,
                    "LA_Id": $scope.LA_Id,
                    "LA_PetitionerandAdvocate": $scope.LA_PetitionerandAdvocate,
                    "LA_RspondentandAdvocate": $scope.LA_RspondentandAdvocate
                };
                apiService.create("LegalUpdateMaster/saveadvocate", data).
                    then(function (promise) {
                        if (promise.returnvales != "") {
                            if (promise.returnvales == "Update") {
                                swal('Record Updated Successfully');
                                $('#mymodalviewlegalcasedetails').modal('hide');
                                //  $scope.cleartabl3();
                            }
                            else if (promise.returnvales == "Add") {
                                swal('Record Saved Successfully');
                                $('#mymodalviewlegalcasedetails').modal('hide');
                                $scope.cleartabl3();
                            }
                            else if (promise.returnvales == "false") {
                                swal('Record Not Saved/Updated successfully');
                                $('#mymodalviewlegalcasedetails').modal('hide');
                                //  $scope.cleartabl3();
                            }
                            else if (promise.returnvales === "Duplicate") {
                                swal('Category Already Exist');
                                $('#mymodalviewlegalcasedetails').modal('hide');
                                //$scope.cleartabl3();
                            }
                        }
                        else {
                            swal('Operation Failed');

                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted4 = function (field) {
            return $scope.submitted4;
        }


        $scope.submitted4 = false;
        $scope.saveacts = function () {
            $scope.submitted4 = true;
            if ($scope.myFormACT.$valid) {
                var data = {
                    "LCD_Id": $scope.LCD_ID,
                    "LACT_Id": $scope.LACT_Id,
                    "LACT_UnderActs": $scope.LACT_UnderActs,
                    "LACT_UnderSection": $scope.LACT_UnderSection
                };
                apiService.create("LegalUpdateMaster/saveacts", data).
                    then(function (promise) {
                        if (promise.returnvales != "") {
                            if (promise.returnvales == "Update") {
                                swal('Record Updated Successfully');
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                                $('#mymodalviewlegalcasedetails').modal('hide');

                            }
                            else if (promise.returnvales == "Add") {
                                swal('Record Saved Successfully');
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                                $('#mymodalviewlegalcasedetails').modal('hide');

                                //  $state.reload();
                            }
                            else if (promise.returnvales == "false") {
                                swal('Record Not Saved/Updated successfully');
                            }
                            else if (promise.returnvales === "Duplicate") {
                                swal('Category Already Exist');
                                $('#mymodalviewlegalcasedetails').modal('hide');
                            }
                        }
                        else {
                            swal('Operation Failed');

                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.interacted5 = function (field) {
            return $scope.submitted5;
        }

        $scope.submitted5 = false;
        $scope.savecasehistory = function () {
            $scope.submitted5 = true;
            if ($scope.myFormHist.$valid) {
                var data = {
                    "LCD_Id": $scope.LCD_ID,
                    "LCH_Id": $scope.LCH_Id,
                    "LCH_RegistrationNumber": $scope.LCH_RegistrationNumber,
                    "LCH_Judge": $scope.LCH_Judge,
                    "LCH_BusinessonDate": $scope.LCH_BusinessonDate,
                    "LCH_PurposeofHearing": $scope.LCH_PurposeofHearing,
                    "LCH_HearingDate": $scope.LCH_HearingDate,
                    "LCD_NextHearingDate": $scope.LCD_NextHearingDate,
                    "LCD_PartyName": $scope.LCD_PartyName
                };
                apiService.create("LegalUpdateMaster/savecasehistory", data).
                    then(function (promise) {
                        if (promise.returnvales != "") {
                            if (promise.returnvales == "Update") {
                                swal('Record Updated Successfully');
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                                $('#mymodalviewlegalcasedetails').modal('hide');
                            }
                            else if (promise.returnvales == "Add") {
                                swal('Record Saved Successfully');
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                                $('#mymodalviewlegalcasedetails').modal('hide');
                            }
                            else if (promise.returnvales == "false") {
                                swal('Record Not Saved/Updated successfully');
                            }
                            else if (promise.returnvales === "Duplicate") {
                                swal('Category Already Exist');
                                $('#mymodalviewlegalcasedetails').modal('hide');
                            }
                        }
                        else {
                            swal('Operation Failed');

                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted6 = function (field) {
            return $scope.submitted6;
        }


        $scope.submitted6 = false;
        $scope.savefinalorder = function () {
            $scope.submitted6 = true;
            if ($scope.myFormFinal.$valid) {
                var data = {
                    "LCD_Id": $scope.LCD_ID,
                    "LFO_Id": $scope.LFO_Id,
                    "LFO_OrderDetails": $scope.LFO_OrderDetails,
                    "LFO_OrderNumber": $scope.LFO_OrderNumber,
                    "LFO_Date": $scope.LFO_Date
                };
                apiService.create("LegalUpdateMaster/savefinalorder", data).
                    then(function (promise) {
                        if (promise.returnvales != "") {
                            if (promise.returnvales == "Update") {
                                swal('Record Updated Successfully');
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else if (promise.returnvales == "Add") {
                                swal('Record Saved Successfully');
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                            }
                            else if (promise.returnvales == "false") {
                                swal('Record Not Saved/Updated successfully');
                            }
                            else if (promise.returnvales === "Duplicate") {
                                swal('Category Already Exist');
                            }
                        }
                        else {
                            swal('Operation Failed');
                            // $state.reload();
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interactedDOC = function (field) {
            return $scope.submitted7;
        }
        $scope.submitted7 = false;
        $scope.SaveDocuments = function () {
            $scope.submitted7 = true;
            if ($scope.myFormDOC.$valid) {
                var data = {
                    "LCD_Id": $scope.LCD_ID,
                    "LD_Id": $scope.LD_Id,
                    "LD_DocumentName": $scope.LD_DocumentName,
                    "Ld_DocumentPath": $scope.Ld_DocumentPath,
                    "LD_Remarks": $scope.Remarks
                };
                apiService.create("LegalUpdateMaster/SaveDocuments", data).
                    then(function (promise) {
                        if (promise.returnvales != "") {
                            if (promise.returnvales == "Update") {
                                swal('Record Updated Successfully');
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                                $('#mymodalviewlegalcasedetails').modal('hide');

                            }
                            else if (promise.returnvales == "Add") {
                                swal('Record Saved Successfully');
                                $scope.myTabIndex = $scope.myTabIndex + 1;
                                $('#mymodalviewlegalcasedetails').modal('hide');
                            }
                            else if (promise.returnvales == "false") {
                                swal('Record Not Saved/Updated successfully');

                            }
                            else if (promise.returnvales === "Duplicate") {
                                swal('Category Already Exist');

                            }
                        }
                        else {
                            swal('Operation Failed');
                            // $state.reload();
                        }
                        $('#mymodalviewlegalcasedetails').modal('hide');
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.editdata = function (employee) {
            $scope.gridcasestatus = [];
            $scope.gridadvocate = [];
            $scope.gridacts = [];
            $scope.gridcasehistory = [];
            $scope.documentslist = [];
            $scope.gridfinalorder = [];
            $scope.LCS_FirstHearingDate = "";
            $scope.LCS_CaseStatus = "";
            $scope.LCS_CourtNumberandJudge = "";
            $scope.LCS_DecisionDate = "";
            $scope.LCS_NatureofDisposal = "";
            $scope.LA_PetitionerandAdvocate = "";
            $scope.LA_RspondentandAdvocate = "";
            $scope.LACT_UnderActs = "";
            $scope.LACT_UnderSection = "";
            $scope.LCH_RegistrationNumber = "";
            $scope.LCH_BusinessonDate = "";
            $scope.LCH_HearingDate = "";
            $scope.LCD_NextHearingDate = "";
            $scope.LCD_PartyName = "";
            $scope.LCH_Judge = "";
            $scope.LCH_PurposeofHearing = "";
            $scope.LFO_OrderNumber = "";
            $scope.LFO_OrderDetails = "";
            $scope.LFO_Date = "";
            $scope.Ld_DocumentPath = "";
            $scope.LCD_ID = "";
            $scope.LD_DocumentName = "";
            $scope.Remarks = "";
            $scope.LD_Id = "";
            $scope.LCS_Id = "";
            $scope.LA_Id = "";
            $scope.LACT_Id = "";
            $scope.LCH_Id = "";

            $scope.LCD_ID = employee.lcD_Id;
            var data = {
                "LCD_Id": employee.lcD_Id
            };
            apiService.create("LegalUpdateMaster/editdata", data).then(function (promise) {
                if (promise !== null) {
                    $scope.gridcasestatus = promise.gridcasestatus;
                    if (true) {
                        $('#mymodalviewlegalcasedetails').modal('show');
                    } else {
                        swal("No Records Found for Legal Case");
                    }

                    $scope.gridadvocate = promise.gridadvocate;
                    if (true) {
                        $('#mymodalviewlegalcasedetails').modal('show');
                    } else {
                        swal("No Records Found for Legal Case");
                    }

                    $scope.gridacts = promise.gridacts;
                    if (true) {
                        $('#mymodalviewlegalcasedetails').modal('show');
                    } else {
                        swal("No Records Found for Legal Case");
                    }

                    $scope.gridcasehistory = promise.gridcasehistory;
                    if (true) {
                        $('#mymodalviewlegalcasedetails').modal('show');
                    } else {
                        swal("No Records Found for Legal Case");
                    }

                    $scope.gridfinalorder = promise.gridfinalorder;
                    if (true) {
                        $('#mymodalviewlegalcasedetails').modal('show');
                    } else {
                        swal("No Records Found for Legal Case");
                    }
                    $scope.documentslist = promise.documentslist;
                    if (true) {
                        $('#mymodalviewlegalcasedetails').modal('show');
                    } else {
                        swal("No Records Found for Legal Case");
                    }
                }
            });
        };

        $scope.ViewLegalCaseDetails = function (record) {
            var data = {
                "LCD_Id": record.lcD_Id
            };
            apiService.create("LegalUpdateMaster/ViewLegalCaseDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.gridcasestatus = promise.gridcasestatus;
                    if ($scope.gridcasestatus !== null && $scope.gridcasestatus.length > 0) {
                        $('#mymodalviewlegalcase').modal('show');
                    } else {
                        swal("No Records Found for gridcasestatus");
                    }

                    $scope.gridadvocate = promise.gridadvocate;
                    if ($scope.gridadvocate !== null && $scope.gridadvocate.length > 0) {
                        $('#mymodalviewlegalcase').modal('show');
                    } else {
                        swal("No Records Found for gridadvocate");
                    }

                    $scope.gridacts = promise.gridacts;
                    if ($scope.gridacts !== null && $scope.gridacts.length > 0) {
                        $('#mymodalviewlegalcase').modal('show');
                    } else {
                        swal("No Records Found for gridacts");
                    }

                    $scope.gridcasehistory = promise.gridcasehistory;
                    if ($scope.gridcasehistory !== null && $scope.gridcasehistory.length > 0) {
                        $('#mymodalviewlegalcase').modal('show');
                    } else {
                        swal("No Records Found for gridcasehistory");
                    }

                    $scope.gridfinalorder = promise.gridfinalorder;
                    if ($scope.gridfinalorder !== null && $scope.gridfinalorder.length > 0) {
                        $('#mymodalviewlegalcase').modal('show');
                    } else {
                        swal("No Records Found for gridfinalorder");
                    }
                    $scope.documentslist = promise.documentslist;
                    if ($scope.documentslist !== null && $scope.documentslist.length > 0) {
                        $('#mymodalviewlegalcase').modal('show');
                    } else {
                        swal("No Records Found for documentslist");
                    }
                }

            });
        };

        $scope.editcasestatus = function (user) {
            $scope.LCS_FirstHearingDate = new Date(user.lcS_FirstHearingDate);
            $scope.LCS_DecisionDate = new Date(user.lcS_DecisionDate);
            $scope.LCS_CaseStatus = user.lcS_CaseStatus;
            $scope.LCS_NatureofDisposal = user.lcS_NatureofDisposal;
            $scope.LCS_CourtNumberandJudge = user.lcS_CourtNumberandJudge;
            $scope.LCS_Id = user.lcS_Id;
        }

        $scope.editadvocate = function (user1) {
            $scope.LA_PetitionerandAdvocate = user1.lA_PetitionerandAdvocate;
            $scope.LA_RspondentandAdvocate = user1.lA_RespondentandAdvocate;
            $scope.LA_Id = user1.lA_Id;
        }

        $scope.editacts = function (user2) {

            $scope.LACT_UnderActs = user2.lacT_UnderActs;
            $scope.LACT_UnderSection = user2.lacT_UnderSection;
            $scope.LACT_Id = user2.lacT_Id;
        }

        $scope.editcasehist = function (user3) {
            $scope.LCH_RegistrationNumber = user3.lcH_RegistrationNumber;
            $scope.LCH_BusinessonDate = new Date(user3.lcH_BusinessonDate);
            $scope.LCH_HearingDate = new Date(user3.lcH_HearingDate);
            $scope.LCH_Judge = user3.lcH_Judge;
            $scope.LCH_PurposeofHearing = user3.lcH_PurposeofHearing;
            $scope.LCH_Id = user3.lcH_Id;
            $scope.LCD_PartyName = user3.lcD_PartyName;
            $scope.LCD_NextHearingDate = new Date(user3.lcD_NextHearingDate);
        }
        $scope.previewimg = function (img) {
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
            else if ($scope.filetype2 == 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');
            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {
                $('#preview12').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');
            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
            }
            else if ($scope.filetype2 == 'pdf') {
                //$('#showpdf12').modal('show');
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
                        $('#showpdf12').modal('show');
                    });
            }
            else {
                $window.open($scope.imagepreview)
            }
        };
        $scope.editfinalorder = function (user4) {
            $scope.LFO_OrderNumber = user4.lfO_OrderNumber;
            $scope.LFO_OrderDetails = user4.lfO_OrderDetails;
            $scope.LFO_Date = new Date(user4.lfO_Date);
            $scope.LFO_Id = user4.lfO_Id;
        }

        $scope.editDocument = function (user5) {

            $scope.LD_DocumentName = user5.lD_DocumentName;
            $scope.Remarks = user5.lD_Remarks;
            $scope.LD_Id = user5.lD_Id;
            $scope.Ld_DocumentPath = user5.ld_DocumentPath;

            if ($scope.Ld_DocumentPath !== null && $scope.Ld_DocumentPath !== "") {
                var img = $scope.Ld_DocumentPath;
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype = lastelement;
                console.log("data.filetype : " + $scope.filetype);
                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                    $scope.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.Ld_DocumentPath;
                }
            }

        }

        $scope.cleartabl1 = function () {
            $state.reload();
        }

        $scope.cleartabl21 = function () {
            $state.reload();
        }

        $scope.cleartabl2 = function () {
            $scope.LCS_FirstHearingDate = "";
            $scope.LCS_DecisionDate = "";
            $scope.LCS_CaseStatus = "";
            $scope.LCS_NatureofDisposal = "";
            $scope.LCS_CourtNumberandJudge = "";
            $scope.LCS_Id = "";
        }

        $scope.cleartabl3 = function () {
            $state.reload();
        }

        $scope.cleartabl3 = function () {
            $scope.LA_PetitionerandAdvocate = "";
            $scope.LA_RspondentandAdvocate = "";
            $scope.LA_Id = "";
        }

        $scope.cleartabl4 = function () {
            $state.reload();
        }

        $scope.cleartabl4 = function () {
            $scope.LACT_UnderActs = "";
            $scope.LACT_UnderSection = "";
            $scope.LACT_Id = "";
        }

        $scope.cleartabl5 = function () {
            $state.reload();
        }

        $scope.cleartabl5 = function () {
            $scope.LCH_RegistrationNumber = "";
            $scope.LCH_BusinessonDate = "";
            $scope.LCH_HearingDate = "";
            $scope.LCH_Judge = "";
            $scope.LCH_PurposeofHearing = "";
            $scope.LCH_Id = "";
        }

        $scope.cleartabl6 = function () {
            $state.reload();
        }

        $scope.cleartabl6 = function () {
            $scope.LFO_OrderNumber = "";
            $scope.LFO_OrderDetails = "";
            $scope.LFO_Date = "";
            $scope.LFO_Id = "";
        }
        //$scope.cleartabl7 = function () {
        //    $state.reload();
        //}
        $scope.cleartabl7 = function () {
            $scope.LD_DocumentName = "";
            $scope.Remarks = "";
            $scope.Ld_DocumentPath = "";

        }

        $scope.uploadtecherdocuments = function (input, document) {

            $scope.uploadtecherdocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {
                //$scope.size = input.files[0].size;
                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" ||
                    input.files[0].type === "image/jpg") {
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
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document,") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-powerpoint") {
                    UploaddianPhoto(document);
                }
                else {
                    swal("Upload  Pdf, Doc, Image Files Only");
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
            $http.post("/api/ImageUpload/UploadStudentDocuments", formData,
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': undefined
                    },
                    transformRequest: angular.identity,

                })
                .success(function (d) {
                    defer.resolve(d);
                    $scope.Ld_DocumentPath = d;
                    $scope.LD_DocumentName = $scope.filename;
                    $('#').attr('src', $scope.Ld_DocumentPath);
                    var img = $scope.Ld_DocumentPath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    $scope.filetype = lastelement;
                    console.log("data.filetype : " + $scope.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        $scope.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.ASCOMP_FilePath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };


        $scope.onview = function (filepath, filename) {
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


        $scope.downloaddirectimage = function (data) {
            $scope.imagedownload = data;
            $http.get($scope.imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: "Image" + '.jpg'
                    })[0].click();
                })
        }


        //$scope.downloaddirectimage = function (data) {
        //    $scope.fileurl = data;

        //    $http.get($scope.fileurl, {
        //        responseType: 'arraybuffer'
        //    }).then(function (response) {
        //        var fileType = response.headers('Content-Type');
        //        var blob = new Blob([response.data], { type: fileType });
        //        var fileExtension = getFileExtension(fileType);
        //        var anchor = angular.element('<a/>');
            
        //        anchor.attr({
        //            href: window.URL.createObjectURL(blob),
        //            target: '_blank',
        //            download: "FILE" + fileExtension
        //        })[0].click();
        //    }).catch(function (error) {
        //        console.error('Error downloading file:', error);
        //    });
        //};

        // Utility function to get file extension based on MIME type
        function getFileExtension(mimeType) {
            var mimeTypes = {
                'application/pdf': '.pdf',
                'image/jpeg': '.jpg',
                'image/png': '.png',
                'application/octet-stream':'pdf'
           
                // Add more MIME types and their extensions here if needed
            };

            /*return mimeTypes[mimeType] || '.bin';*/ // Default to .bin if MIME type is unknown
        }


    }



})();