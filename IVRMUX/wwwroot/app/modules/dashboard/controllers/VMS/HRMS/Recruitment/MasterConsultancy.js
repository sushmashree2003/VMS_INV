(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterConsultancyController', MasterConsultancyController);

    MasterConsultancyController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter'];
    function MasterConsultancyController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter) {




        var paginationformasters;
        var copty;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        // form Object
        $scope.EarningDet = {};
        $scope.addjob = {};
        $scope.mrfReq = {};
        $scope.addjob.hrjD_Id = 0;
        $scope.submitted = true;




        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("AddJobVMS/getalldetails", pageid).then(function (promise) {

                if (promise.masterPositionList !== null && promise.masterPositionList.length > 0) {
                    $scope.masterPositionList = promise.masterPositionList;
                }

                if (promise.masterLocation !== null && promise.masterLocation.length > 0) {
                    $scope.masterLocation = promise.masterLocation;
                }

                if (promise.clientlist !== null && promise.clientlist.length > 0) {
                    $scope.clientlist = promise.clientlist;
                }

                if (promise.masterDepartmentList !== null && promise.masterDepartmentList.length > 0) {
                    $scope.masterDepartmentList = promise.masterDepartmentList;
                }

                if (promise.masterPriorityList !== null && promise.masterPriorityList.length > 0) {
                    $scope.masterPriorityList = promise.masterPriorityList;
                }

                if (promise.masterPosTypeList !== null && promise.masterPosTypeList.length > 0) {
                    $scope.masterPosTypeList = promise.masterPosTypeList;
                }

                if (promise.masterQualification !== null && promise.masterQualification.length > 0) {
                    $scope.masterQualification = promise.masterQualification;
                }

                if (promise.masterGender !== null && promise.masterGender.length > 0) {
                    $scope.masterGender = promise.masterGender;
                }
                if (promise.countrylist !== null && promise.countrylist.length > 0) {
                    $scope.countrylist = promise.countrylist;

                    angular.forEach($scope.countrylist, function (head) {
                        if (head.ivrmmC_Default == 1) {
                            $scope.mrfReq.ivrmmC_Id = head.ivrmmC_Id;
                            $scope.staterecord()
                        }

                    })
                }

                if (promise.gridrecordMRF !== null && promise.gridrecordMRF.length > 0) {
                    $scope.gridrecordMRF = promise.gridrecordMRF;
                }
                if (promise.consulancygriddata !== null && promise.consulancygriddata.length > 0) {
                    $scope.consulancygriddata = promise.consulancygriddata;
                }



                if (promise.hrmrfR_Id !== 0) {
                    $scope.mrfReq = promise.vmsmrfList[0];
                }

            });

        };

        // clear form data
        $scope.comment = function (user) {
            $scope.hrmrfR_MDComment = user.hrmrfR_MDComment
            $scope.hrmrfR_UpdatedBy = user.hrmrfR_UpdatedBy
            $scope.PayScaleFrom = user.hrmrfR_PayScaleFrom
            $scope.PayScaleTo = user.hrmrfR_PayScaleTo
            $('#approvedcomment').modal('show');
        };

        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $scope.mrfReq = {};
            $state.reload();
        };
        $scope.staterecord = function (ivrmmC_Id) {
            $scope.statelist = [];


            var dataED = {
                "IVRMMC_Id": $scope.mrfReq.ivrmmC_Id,
            };

            apiService.create("AddJobVMS/staterecord", dataED).then(function (promise) {
                if (promise.statelist !== null && promise.statelist.length > 0) {
                    $scope.statelist = promise.statelist;
                }
            });
        };

        $scope.edit = function (item) {
            $scope.mrfReq.HRMCON_ConsultancyName = item.hrmcoN_ConsultancyName;
            $scope.mrfReq.HRMCON_ContactPersonName = item.hrmcoN_ContactPersonName;
            $scope.mrfReq.HRMCON_Address = item.hrmcoN_Address;
            $scope.mrfReq.HRMCON_ContactNo = item.hrmcoN_ContactNo;
            $scope.mrfReq.HRMCON_EmailId = item.hrmcoN_EmailId;
            $scope.mrfReq.HRMCON_City = item.hrmcoN_City;
            $scope.mrfReq.HRMCON_State = item.hrmcoN_State;
            $scope.mrfReq.HRMCON_PINCode = item.hrmcoN_PINCode;
            $scope.mrfReq.HRMCON_Id = item.hrmcoN_Id;
            $scope.mrfReq.HRMCON_State = item.ivrmmS_Id;

        };


        //Delete 
        $scope.deletedataY = function (user, SweetAlert) {

            var data = {
                "HRMRFR_Id": user.hrmrfR_Id,
            }

            swal({
                title: "Are you sure",
                text: "Do you want to delete record????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("AddJobVMS/deleterecord", data).then(function (promise) {
                            $scope.cancel();
                            if (promise.retrunMsg !== "") {

                                if (promise.retrunMsg === "Deleted") {
                                    swal("Record Deleted successfully");
                                    $state.reload();
                                }
                                else if (promise.retrunMsg === "Failed") {
                                    swal("Record Not Failed");
                                }
                                else {
                                    swal("Record Not Failed", 'Fail');
                                }

                                $scope.cancel();
                            }
                        });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };

        $scope.deactive = function (item) {
            $scope.HRMCON_Id = item.hrmcoN_Id;
            var dystring = "";
            if (item.hrmcoN_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.hrmcoN_ActiveFlg == false) {
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
                        apiService.create("AddJobVMS/activedeactive", item).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        }


        $scope.Savedata = function (obj) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = $scope.mrfReq;

                apiService.create("AddJobVMS/saverecord", data).then(function (promise) {
                    if (promise.returnval == true) {
                        if ($scope.mrfReq.HRMCON_Id == 0 || $scope.mrfReq.HRMCON_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if ($scope.mrfReq.HRMCON_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnval == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if ($scope.mrfReq.hrmcoN_Id == 0 || $scope.mrfReq.hrmcoN_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if ($scope.mrfReq.hrmcoN_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.SelectedFileForUploadzdd = [];
        $scope.selectFileforUploadzd = function (input, document) {

            $scope.SelectedFileForUploadzdd = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.ismresgmcL_Id).attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofiled(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.ismresgmcL_Id).attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofiled(document);
                }
                else if (input.files[0].type === "application/msword") {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.ismresgmcL_Id).attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofiled(document);
                }

                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload MP4, Pdf, Image Files Only");
                }
            }
        };

        $scope.showmodaldetailscheck = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];

            if (extention == "jpg" || extention == "jpeg" || extention == "png") {
                $('#preview').attr('src', data);
                $('#myModal').modal('show');
            }
            else if (extention == "pdf" || extention == "xlsx" || extention == "ppt" || extention == "zip" || extention == "doc" || extention == "docx" || extention == "txt") {
                var imagedownload = data;
                $scope.content = "";
                var fileURL = "";
                var file = "";
                $http.get(imagedownload, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        var pdfId = document.getElementById("pdfIdzz");
                        pdfId.removeChild(pdfId.childNodes[0]);
                        var embed = document.createElement('embed');
                        embed.setAttribute('src', fileURL);
                        embed.setAttribute('type', 'application/pdf');
                        embed.setAttribute('width', '100%');
                        embed.setAttribute('height', '1000');
                        pdfId.appendChild(embed);
                        $('#showpdf').modal('show');

                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        $scope.content = $sce.trustAsResourceUrl(fileURL);
                        $('#showpdf').modal('show');
                    });
            }
        };

        function UploadEmployeeDocument(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzd.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
            }
            // We can send more data to server using append         
            formData.append("Id", 0);
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
                        data.hrmrfR_Attachment = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.getposition = function (data) {
            angular.forEach($scope.masterPositionList, function (value, key) {
                if (value.hrmP_Id === parseInt(data)) {
                    $scope.mrfReq.hrmrfR_Skills = value.hrmP_Skills;
                    $scope.mrfReq.hrmrfR_JobDesc = value.hrmP_Desc;
                }
            });
        };
    }

})();