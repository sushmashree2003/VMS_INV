(function () {
    'use strict';
    angular
        .module('app')
        .controller('Channel_PatnerController', Channel_PatnerController)
    Channel_PatnerController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$stateParams', 'Excel', '$timeout', '$q']
    function Channel_PatnerController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $stateParams, Excel, $timeout, $q) {
        //======================================================
        $scope.yearlist = [];
        $scope.UploadFile = [];
        $scope.updshow = false;
        $scope.images_temp = [];
        $scope.checklink = false;
        var paginationformasters;
        $scope.saveupdate = true;
        //$scope.emailbtn = false;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.checklink = false;


        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }


        //Serch Grid Value===========================================================
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 10;

        //Grid======================================================================
        $scope.Loaddata = function () {
       
            var pageid = 2;
            apiService.getURI("Sales_Lead/load_all_lead", pageid).then(function (promise) {
                $scope.channalPatner_List = promise.channalPatner_List;

            });
        };

        $scope.Loaddata_mapping = function () {
       
            var pageid = 2;
            apiService.getURI("Sales_Lead/load_all_lead_mapping", pageid).then(function (promise) {
                $scope.yearlist = promise.yearlist;
                $scope.lead_list = promise.lead_list;



            });
        };
        $scope.IMFY_Id = 0;
        

        $scope.getChannelPatner = function (IMFY_Id) {

            var data = {
                "IMFY_Id": $scope.IMFY_Id,
              
            }
            apiService.create("Sales_Lead/getChannelPatner", data).then(function (promise) {

                if (promise.channalPatnerList != null && promise.channalPatnerList.length > 0) {
                    $scope.channalPatnerList = promise.channalPatnerList;
                    $scope.leadlists = promise.leadlists;
                    if ($scope.leadlists != null && $scope.leadlists.length > 0) {
                        angular.forEach($scope.leadlists, function (le) {
                            le.selected = false;
                        });
                    }
                  
                }
               // $scope.yearlist = promise.yearlist;
               
                

            });
        }

        var imagedownload = "";
        var docname = "";
        $scope.downloaddirectpdf = function (data, idd) {

            var studentreg = idd;

            $scope.imagedownload = data;
            imagedownload = data;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg + '.pdf'
                    })[0].click();
                });
        };

        var imagedownloadppt = "";
        var docname = "";
        $scope.downloaddirectppt = function (data, idd) {

            var studentreg = idd;

            $scope.imagedownloadppt = data;
            imagedownloadppt = data;

            $http.get(imagedownloadppt, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg + '.ppt || .pptx || .ppsx'
                    })[0].click();
                });
        };
        $scope.downloaddirectimage = function (data, idd) {

            var studentreg = idd;

            $scope.imagedownload = data;
            imagedownload = data;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg + '.jpg'
                    })[0].click();
                });
        };


        //-----------upload file/photo.............
        $scope.uploadFile = function (input, document) {

            $scope.UploadFile = input.files;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.ms-excel" || input.files[0].type === "application/pptx" || input.files[0].type === "application/ppsx" || input.files[0].type === "application/vnd.ms-powerpoint" || input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation" || input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" || input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.slideshow" || input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation") {

                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $('#blahD')
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();
                }

            }
        };
        function Uploadprofile() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadFile.length; i++) {
                formData.append("File", $scope.UploadFile[i]);
                $scope.file_detail = $scope.UploadFile[0].name;
            }
            //We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadChannelPartnerdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    $scope.notice = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }
        //----------End..........!
        $scope.content1 = "";
        ///=====================show pdf, img
        $scope.previewpdf = function (filepath1, filename) {
            $('#showpdf').modal('hide');
            var imagedownload1 = "";
            imagedownload1 = filepath1;


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
        };


        $scope.previewimg = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };
        ///=====================show pdf, img end

        ///=========clear upload field data......
        $scope.remove_file = function () {
            $scope.file_detail = "";
            $scope.notice = "";
        };



        //Save======================================================================

        $scope.ISMSMCP_Id = 0;
        $scope.submitted = false;
        $scope.channelList = [];
        $scope.saverecord = function () {


                $scope.ldrnew = true;

                //-------file upload
                $scope.filedoc = [];
                $scope.filedoc1 = [];
                $scope.filedoc2 = [];
                $scope.documentListOtherDetails11 = [];
                if ($scope.documentListOtherDetails != null) {
                    angular.forEach($scope.documentListOtherDetails, function (qq) {
                        if (qq.ISMSMCPD_DocumentPath != null) {
                            $scope.documentListOtherDetails11.push({ ISMSMCPD_DocumentPath: qq.ISMSMCPD_DocumentPath, FileName: qq.FileName });
                        }
                    })
                    $scope.filedoc = $scope.documentListOtherDetails11;
                }

                if ($scope.checklink == true) {
                    angular.forEach($scope.urldocumentlist, function (ee) {
                        if (ee.ISMSMCPD_DocumentPath != null) {
                            $scope.filedoc1.push({ ISMSMCPD_DocumentPath: ee.ISMSMCPD_DocumentPath, FileName: ee.ISMSMCPD_DocumentPath });
                        }
                    })
                }
                if ($scope.filedoc1 != null || $scope.filedoc1 > 0) {
                    angular.forEach($scope.filedoc1, function (ww) {
                        $scope.filedoc.push(ww);
                    })
                }

             

                $scope.removeNewDocumentOtherDetail = function (index, data) {
                    var newItemNo = $scope.documentListOtherDetails.length - 1;
                    $scope.documentListOtherDetails.splice(index, 1);
                    if (data.ISMSMCPD_Id > 0) {
                        $scope.DeleteDocumentDataOthers(data);
                    }
                };

            if ($scope.myForm.$valid) {
                var ISMSMCP_AgreementStartDate = null;
                var ISMSMCP_ActiveFromDate = null;
                var ISMSMCP_ActiveToDate = null;

                var data = {};
                    if ($scope.checkoruncheck == true) {
                        $scope.checkoruncheck = true;
                    }
                    else {
                        $scope.checkoruncheck = false;

                }
                if ($scope.ISMSMCP_AgreementStartDate != null && $scope.ISMSMCP_AgreementStartDate.length > 0) {
                    $scope.ISMSMCP_AgreementStartDate = new Date($scope.ISMSMCP_AgreementStartDate).toDateString();

                }

                if ($scope.ISMSMCP_ActiveFromDate != null && $scope.ISMSMCP_ActiveFromDate.length > 0) {
                    $scope.ISMSMCP_ActiveFromDate = new Date($scope.ISMSMCP_ActiveFromDate).toDateString();

                }
                if ($scope.ISMSMCP_ActiveToDate != null && $scope.ISMSMCP_ActiveToDate.length > 0) {
                    $scope.ISMSMCP_ActiveToDate = new Date($scope.ISMSMCP_ActiveToDate).toDateString();

                }
                     data = {
                        "ISMSMCP_Id": $scope.ISMSMCP_Id,
                        "ISMSMCP_ChannelPartnerName": $scope.ISMSMCP_ChannelPartnerName,
                        "ISMSMCP_CPAddress": $scope.ISMSMCP_CPAddress,
                        "ISMSMCP_CPContactNo": $scope.ISMSMCP_CPContactNo,
                        "ISMSMCP_CPEmailId": $scope.ISMSMCP_CPEmailId,
                        "ISMSMCP_AgreementDuration": $scope.ISMSMCP_AgreementDuration,
                         "ISMSMCP_AgreementStartDate": $scope.ISMSMCP_AgreementStartDate,
                         "ISMSMCP_ActiveFromDate": $scope.ISMSMCP_ActiveFromDate,
                         "ISMSMCP_ActiveToDate": $scope.ISMSMCP_ActiveToDate,
                        "ISMSMCP_RenewedFlg": $scope.checkoruncheck,
                        "ISMSMCPD_DocumentPath_Array": $scope.filedoc,
                    }
                    apiService.create("Sales_Lead/SaveChannel", data).
                        then(function (promise) {

                            if (promise.msg == 'Add') {
                                swal("Record Saved Successfully!");
                                $state.reload();
                            }
                            else if (promise.msg == 'Updated') {
                                swal("Record Updated Successfully!");
                                $state.reload();
                            }
                            else if (promise.msg == 'duplicate') {
                                swal("Record already exist Contact-No. OR Email-ID");
                            }
                            else if (promise.msg == "savingFailed") {
                                swal("Failed to save record");
                            }
                            else if (promise.msg == "updateFailed") {
                                swal("Failed to update/Duplicate Contact-No. OR Email-ID");
                            }
                            else {
                                swal("Sorry...something went wrong");
                            }

                        })
                }
                else {
                    $scope.submitted = true;
                }
         

        }




        //channel partner save
        $scope.submitted11 = false;
        $scope.save_ChannelPatnerMapping = function () {

            if ($scope.IMFY_Id != null && $scope.IMFY_Id != "" && $scope.IMFY_Id != undefined) {
                if ($scope.ISMSMCP_Id != null && $scope.ISMSMCP_Id != "" && $scope.ISMSMCP_Id != undefined) {
                    $scope.leadtdata = [];
                    angular.forEach($scope.leadlists, function (le) {
                        if (le.selected == true) {
                            $scope.leadtdata.push(le);
                        }
                    });

                    if ($scope.leadtdata.length > 0 && $scope.leadtdata != null && $scope.leadtdata != undefined) {
                        if ($scope.myForm11.$valid) {
                            $scope.submitted11 = true;
                            if ($scope.checkoruncheck == true) {
                                $scope.checkoruncheck = true;
                            }
                            else {
                                $scope.checkoruncheck = false;

                            }
                            var data = {
                                "IMFY_Id": $scope.IMFY_Id,
                                "ISMSMCP_Id": $scope.ISMSMCP_Id,
                                "leadlsttwo": $scope.leadtdata,
                                "ISMSLE_Id": $scope.ISMSLE_Id
                            }
                            apiService.create("Sales_Lead/save_ChannelPatnerMapping", data).
                                then(function (promise) {

                                    if (promise.msg == 'Add') {
                                        swal("Record Saved Successfully!");
                                        $state.reload();
                                    }
                                    else if (promise.msg == 'Updated') {
                                        swal("Record Updated Successfully!");
                                        $state.reload();
                                    }
                                    else if (promise.msg == 'duplicate') {
                                        swal("Record already exist Contact-No. OR Email-ID");
                                    }
                                    else if (promise.msg == "savingFailed") {
                                        swal("Failed to save record");
                                    }
                                    else if (promise.msg == "updateFailed") {
                                        swal("Failed to update/Duplicate Contact-No. OR Email-ID");
                                    }
                                    else {
                                        swal("Sorry...something went wrong");
                                    }

                                })
                        }
                        else {
                            $scope.submitted1 = true;
                        }
                    }
                    else {
                        swal("Please Select Atleast One Lead..");
                    }
                
                } else {
                    swal("Please Select The Channel Partner..");
                }

            } else {
                swal("Please Select The Financial Year..");
            }
       



        }
        //


        //==============view data=========

        $scope.viewData = function (option) {
            $scope.attachementlist = [];
            var data = {
                "INTB_Id": option.intB_Id

            };
            apiService.create("Sales_Lead/viewData", data)
                .then(function (promise) {

                    if (promise.attachementlist.length > 0) {
                        $scope.attachementlist1 = [];
                        angular.forEach(promise.attachementlist, function (qq) {
                            $scope.img = qq.ismsmcpD_DocumentPath;
                            if ($scope.img != null) {
                                var imagarr = $scope.img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];

                                $scope.filetype2 = lastelement;
                            }

                            if ($scope.filetype2 == 'mp4' || $scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp3'
                                || $scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' ||
                                $scope.filetype2 == 'pdf' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx'
                                || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'ppsx'
                                || $scope.filetype2 == 'doc' || $scope.filetype2 == 'docx') {
                                $scope.attachementlist1.push({
                                    ISMSMCPD_DocumentName: qq.ismsmcpD_DocumentName,
                                    ISMSMCPD_DocumentPath: qq.ismsmcpD_DocumentPath,
                                    ISMSMCP_Id: qq.ISMSMCP_Id
                                })
                            }
                            else {
                                $scope.attachementlist1.push({

                                    ISMSMCPD_DocumentName: qq.ismsmcpD_DocumentName,
                                    ISMSMCPD_DocumentPath: qq.ismsmcpD_DocumentPath,
                                    ISMSMCP_Id: qq.ismsmcP_Id

                                })
                            }
                        })

                        $scope.attachementlist = $scope.attachementlist1

                        $('#myModalCoverview').modal('show');
                        $scope.docshowary = true;
                        $scope.docshow = false;
                    }
                    else {
                        swal("No Data Found.")

                    }

                });
        };

        //Cancel==============================================================
        $scope.cancel = function () {
            $state.reload();
        }




        //Edit================================================================
        $scope.EditData = function (ss) {
            $scope.saveupdate = false;
            var pageid = {
                "ISMSMCP_Id": ss.ismsmcP_Id,
                "MI_Id": ss.mI_Id
            }
            apiService.create("Sales_Lead/Sales_lead_edit", pageid).then(function (promise) {
                $scope.MI_Id = promise.channel_patner_edit[0].mI_Id;
                $scope.ISMSMCP_Id = promise.channel_patner_edit[0].ismsmcP_Id;
                $scope.ISMSMCP_ChannelPartnerName = promise.channel_patner_edit[0].ismsmcP_ChannelPartnerName;
                $scope.ISMSMCP_CPAddress = promise.channel_patner_edit[0].ismsmcP_CPAddress;
                $scope.ISMSMCP_CPEmailId = promise.channel_patner_edit[0].ismsmcP_CPEmailId;
                $scope.ISMSMCP_CPContactNo = promise.channel_patner_edit[0].ismsmcP_CPContactNo;
                $scope.ISMSMCP_AgreementDuration = promise.channel_patner_edit[0].ismsmcP_AgreementDuration;
                $scope.ISMSMCP_AgreementStartDate = new Date(promise.channel_patner_edit[0].ismsmcP_AgreementStartDate);
                $scope.ISMSMCP_ActiveFromDate = new Date(promise.channel_patner_edit[0].ismsmcP_ActiveFromDate);
                $scope.ISMSMCP_ActiveToDate = new Date(promise.channel_patner_edit[0].ismsmcP_ActiveToDate);
                $scope.checkoruncheck = promise.channel_patner_edit[0].ismsmcP_RenewedFlg;
                $scope.urldocumentlist = [];
                $scope.documentListOtherDetails = [];
                if (promise.attachementlist != null && promise.attachementlist.length > 0) {
                    angular.forEach(promise.attachementlist, function (aa) {
                        $scope.img = aa.ismsmcpD_DocumentPath;
                        if ($scope.img != null) {
                            var imagarr = $scope.img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            $scope.filetype2 = lastelement;
                        }

                        if ($scope.filetype2 == 'mp4' || $scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp3'
                            || $scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' ||
                            $scope.filetype2 == 'pdf' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx'
                            || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'ppsx'
                            || $scope.filetype2 == 'doc' || $scope.filetype2 == 'docx') {
                            $scope.documentListOtherDetails.push({
                                id: 1, ISMSMCPD_DocumentPath: aa.ismsmcpD_DocumentPath,
                                FileName: aa.ismsmcpD_DocumentName
                            });
                        }
                        else {
                            $scope.urldocumentlist.push({
                                id: 1, ISMSMCPD_DocumentPath: aa.ismsmcpD_DocumentPath,
                                FileName: aa.ismsmcpD_DocumentName
                            });

                        }
                    });
                }
                else {
                    $scope.documentListOtherDetails.push({
                        id: 1, ISMSMCPD_DocumentPath: '',
                        FileName: ''
                    });
                }

                if ($scope.urldocumentlist.length > 0) {
                    $scope.checklink = true;
                } else {
                    $scope.urldocumentlist = [{ id: 'document' }];
                }

            });
        };


        //edit mapping data

        $scope.EditMappingData = function (ss) {
            $scope.saveupdate = false;
            var pageid = {
                "ISMSLE_Id": ss.ismslE_Id
            }
            apiService.create("Sales_Lead/leadChannelmapping_edit", pageid).then(function (promise) {
                if (promise.lead_list != null && promise.lead_list.length > 0) {

                    $scope.channalPatnerList = promise.channalPatnerList;
                    $scope.leadlists = promise.leadlists;

                    $scope.IMFY_Id = promise.lead_list[0].imfY_Id;
                    $scope.ISMSLE_Id = promise.lead_list[0].ismslE_Id;
                    $scope.ISMSMCP_Id = promise.lead_list[0].ismsmcP_Id;
          
                    if ($scope.leadlists != null && $scope.leadlists.length > 0) {
                        angular.forEach($scope.leadlists, function (le) {
                            if ($scope.ISMSLE_Id == le.ismslE_Id) {
                                le.selected = true;
                            }
                            else {
                                le.selected = false;
                            }
                           
                        });
                    }
                   
                }
               

            });
        };

        //




        //Deactivate=============================================
        $scope.deactivate = function (user, SweetAlertt) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            var mgs = "";
            if (user.ismsmcP_ActiveFlag == false) {

                mgs = "Activate";

            }
            else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " Record?",
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
                        apiService.create("Sales_Lead/deactivate", user).
                            then(function (promise) {

                                if (promise.returnVal == true) {
                                    if (promise.msg != null) {
                                        swal(promise.msg);
                                        $state.reload();
                                    }
                                }
                                else {
                                    swal('Failed to Activate/Deactivate the Record');
                                }
                            })
                    } else {
                        swal("Cancelled");
                    }
                })
        }


        //Requierd message=======================================================
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted11;
        };

        $scope.all_checkL = function (all) {

            $scope.leadid = all;
            var toggleStatus = $scope.leadid;
            angular.forEach($scope.leadlists, function (role) {
                role.selected = toggleStatus;
            });
        }
        $scope.isOptionsRequired = function () {
            return !$scope.leadlists.some(function (item) {
                return item.selected;
            });
        }

        $scope.togchkbxL = function () {

            $scope.leadid = $scope.leadlists.every(function (role) {
                return role.selected;
            });
        }

        //============Multiple file upload===========
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
        //-------------------ADD------------------
        $scope.urldocumentlist = [{ id: 'document' }];
        $scope.addNewurl = function () {
            var newItemNo = $scope.urldocumentlist.length + 1;
            if (newItemNo <= 30) {
                $scope.urldocumentlist.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewurl = function (index, data) {
            var newItemNo = $scope.urldocumentlist.length - 1;
            $scope.urldocumentlist.splice(index, 1);
            if (data.hreothdeT_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
        };
        //======================= file upload
        $scope.selectFileforUploadzdOtherDetail = function (input, document) {
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
            $http.post("/api/ImageUpload/UploadChannelPartnerdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {

                        data.ISMSMCPD_DocumentPath = d[0].path;
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

        $scope.previewimg_url = function (url) {
            $scope.urlnew = url;
            $window.open($scope.urlnew)
        }




                //========================end===========================
    }
    
})();