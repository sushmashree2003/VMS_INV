(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_Sales_Lead_CommentsController', ISM_Sales_Lead_CommentsController)

    ISM_Sales_Lead_CommentsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache', '$q']
    function ISM_Sales_Lead_CommentsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache, $q) {
        //$scope.sortKey = 'ISMCLTPRBOM_Id';
        $scope.sortKey = '';
        $scope.sortReverse = true;

        $scope.submitted = false;
        $scope.flag = 'HOT';

        $scope.ISMSLEDMPR_SendQuotationFlg = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
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
        // $scope.SelectedFileForUploadzd = [];
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        $scope.ismslecoM_SendQuotationFlg = false;
        $scope.ismeridian = true;
        $scope.hstep = 1;
        $scope.mstep = 1;
        //get data


        $scope.onradiobuttonSelect = function (optionflag) {
            $scope.flag = optionflag;
            $scope.getAllDetail($scope.flag);
        };

        $scope.getAllDetail = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var data = {
                "flag": $scope.flag
            }
            apiService.create("ISM_Client_Project/getdata_CMT", data)
                .then(function (promise) {

                    $scope.commentlist = promise.commentlist;
                    $scope.followuplead = promise.followuplead;
                    $scope.presentCountgrid = $scope.commentlist.length;
                });
        }
        //save
        $scope.submitted = false;
        $scope.savetmpldata = function () {
            $scope.uploadarray = [];
            var quotationFlg = false;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                //added by roop 
                if ($scope.materaldocuupload.length > 0) {
                    angular.forEach($scope.materaldocuupload, function (itm) {
                        if (itm.ismclT_FilePath !== undefined && itm.ismclT_FilePath !== null && itm.ismclT_FilePath !== "") {
                            $scope.uploadarray.push({
                                ISMSLECF_FileName: itm.vtadaaF_FileName,
                                ISMSLECF_FilePath: itm.ismclT_FilePath,
                                ISMSLECF_FileDesc: itm.VTADAAF_Remarks
                            });
                        }
                    });
                }

                var ISMSLECOM_RemainderTime = $scope.ismslecoM_RemainderTime;
                $scope.result = $filter('date')(ISMSLECOM_RemainderTime, 'HH:mm');



                //
                var data = {
                    "ISMSLECOM_Id": $scope.ISMSLECOM_Id,
                    "ISMSLE_Id": $scope.ISMSLE_Id.ISMSLE_Id,
                    "ISMSLECOM_Comments": $scope.ISMSLECOM_Comments,
                    "ISMSLECOM_RemainderDate": new Date($scope.ismslecoM_RemainderDate).toDateString(),
                    "ISMSLECOM_RemainderTime": $scope.result,
                    "ISMSLECOM_RemainderHrs": $scope.ismslecoM_RemainderHrs,
                    "ISMSLECOM_SendQuotationFlg": $scope.ismslecoM_SendQuotationFlg,
                    filelist: $scope.uploadarray
                };
                apiService.create("ISM_Client_Project/SaveEdit_CMT", data).
                    then(function (promise) {


                        if (promise.returndata === "Update") {
                            swal('Record Updated Successfully');
                            $state.reload();

                        }
                        else if (promise.returndata === "Add") {
                            swal('Record Saved Successfully');
                            $state.reload();

                        }
                        else if (promise.returndata === "Error") {
                            swal('Record Not Saved/Updated successfully');
                            return;
                        }

                    });

            }
            else {

                $scope.submitted = true;
            }

        };
        //edit

        $scope.edibl = {}
        $scope.edit = function (bil) {
            $scope.editflag = true;
            $scope.edibl = bil.ismslecoM_Id;
            var pageid = $scope.edibl;
            apiService.getURI("ISM_Client_Project/details_CMT", pageid).
                then(function (promise) {
                    $scope.ISMSLECOM_Id = promise.edit_comment[0].ismslecoM_Id;
                    // var ISMSLE_Id = promise.edit_comment[0].ismslE_Id;
                    //$scope.ISMSLE_Id = promise.edit_comment[0].ismslE_Id;
                    $scope.ISMSLECOM_Comments = promise.edit_comment[0].ismslecoM_Comments;
                    $scope.leadlist_dd = promise.leadlist_dd;
                    $scope.ismslE_FilePath = promise.edit_comment[0].ismslE_FilePath;
                    if (promise.editArryThree != null && promise.editArryThree.length > 0) {
                        $scope.materaldocuupload = promise.editArryThree;
                        angular.forEach($scope.materaldocuupload, function (dd) {
                            dd.VTADAAF_Remarks = dd.ismslecF_FileDesc;
                            dd.ismclT_FilePath = dd.ismslecF_FilePath;
                            dd.ismclT_FileName = dd.ismslecF_FileName;
                        });
                    }

                    $scope.ismslecoM_RemainderDate = new Date(promise.edit_comment[0].ismslecoM_RemainderDate);
                    $scope.ismslecoM_RemainderHrs = promise.edit_comment[0].ismslecoM_RemainderHrs;
                    $scope.ismslecoM_RemainderTime = moment(promise.edit_comment[0].ismslecoM_RemainderTime, 'HH:mm a').format();
                    if ($scope.followuplead != null && $scope.followuplead.length > 0) {
                        //angular.forEach($scope.followuplead, function (dd) {
                        //    if (dd.ismslE_Id === bil.ismslE_Id) {
                        //        $scope.ISMSLE_Id = promise.edit_comment[0];
                        //        //$scope.ISMSLE_Id = dd.ismslE_Id;
                        //    }
                        //});

                        //followuplead array is dropdown array
                        for (var i = 0; i < $scope.followuplead.length; i++) {
                            if (promise.edit_comment[0].ismslE_Id == $scope.followuplead[i].ISMSLE_Id) {

                                $scope.ISMSLE_Id = $scope.followuplead[i];

                            }
                        }

                    }


                });
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
            if (flr.ismslecoM_ActiveFlag === false) {
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
                        apiService.create("ISM_Client_Project/deactivate_CMT", flr).
                            then(function (promise) {


                                if (promise.returndata === 'false') {
                                    swal('Lead Comments Deactivated Successfully.');
                                }

                                else if (promise.returndata === 'true') {
                                    swal('Lead Comments Activated Successfully.');
                                }

                                else if (promise.returndata === 'Error') {
                                    swal('Operation Failed!!!');
                                }


                                $scope.getAllDetail();
                            });
                    } else {
                        swal("Cancelled");
                        $state.reload();
                    }

                });
        };
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        //$scope.selectFileforUploadzd = function (input) {
        //    debugger;
        //    $scope.SelectedFileForUploadzd = input.files;
        //    if (input.files && input.files[0]) {               
        //        if (input.files[0].size > 2097152) {
        //            swal("File size should be less than 2MB");
        //            return;
        //        }
        //        else {
        //            if (input.files[0].type === "application/pdf" && input.files[0].size <= 2097152) {
        //                UploaddianmateralPhoto();
        //            }
        //            else if (input.files[0].type === "application/vnd.ms-excel" && input.files[0].size <= 2097152) {
        //                UploaddianmateralPhoto();
        //            }
        //        }
        //    }
        //};
        //function UploaddianmateralPhoto() {

        //    var formData = new FormData();
        //    for (var i = 0; i <= 1; i++) {
        //        formData.append("File", $scope.SelectedFileForUploadzd[i]);
        //    }            
        //    var defer = $q.defer();
        //    $http.post("/api/ImageUpload/Uploadtrnsportdocuments", formData,
        //        {
        //            withCredentials: true,
        //            headers: { 'Content-Type': undefined },
        //            transformRequest: angular.identity
        //        })
        //        .success(function (d) {
        //            defer.resolve(d);
        //            $scope.ismslE_FilePath = d;

        //        })
        //        .error(function () {
        //            defer.reject("File Upload Failed!");
        //        });
        //}

        //$scope.previewimg_new = function (img) {
        //    $scope.ismslE_FilePath = img;
        //    var img = $scope.ismslE_FilePath;
        //    if (img != null && img != "") {
        //        var imagarr = img.split('.');
        //        var lastelement = imagarr[imagarr.length - 1];

        //        if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
        //            $window.open($scope.ismslE_FilePath)
        //        }
        //        else if (lastelement == 'pdf') {
        //            $('#showpdf').modal('hide');
        //            var imagedownload1 = "";
        //            imagedownload1 = $scope.ismslE_FilePath;
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


        //};                  

        //added by roop
        $scope.previewimg_new = function (img) {
            $scope.ismclT_FilePath = img;
            var img = $scope.ismclT_FilePath;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.ismclT_FilePath);
                    $('#myimagePreview').modal('show');
                }
                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                    $window.open($scope.ismclT_FilePath);
                }
                else if (lastelement == 'pdf') {
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.ismclT_FilePath;
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
            }
            else {
                $window.open($scope.ismclT_FilePath)
            }
        };
        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input, document) {
            $scope.SelectedFileForUploadzd = input.files;
            $scope.vtadaA_FileName = input.files[0].name;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/png" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/jpg" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/JPG" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/pdf" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/msword" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                //5242880
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("File size should be less than 5 MB");
                    return;
                }
            }
        };
        function UploaddianmateralPhoto(data) {
            console.log("Sales Lead  :" + data);
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
        $scope.removegrnrows = function (index, data) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

        };


        //nagaraj

        $scope.ISMSLEDMPR_SendQuotationFlg = false;
        $scope.updateCheckbox2 = function (evet) {
            if (evet.ISMSLEDMPR_SendQuotationFlg == true) {
                $scope.ISMSLEDMPR_SendQuotationFlg = true;
                return evet.ISMSLEDMPR_SendQuotationFlg;
            }
        };


        $scope.ddd = [];
        $scope.StatusUpdateOnLoad = [];
        $scope.StatusDropDown = [];
        $scope.viewdetails = function (dd) {
            $scope.dd = dd.ismslecoM_Id;
            $scope.ddd = dd;
            var dataStatus = {
                "ISMSLE_Id": dd.ismslE_Id,
                "flag": "2"
            }
            apiService.create("ISM_Client_Project/StatusDropDownData", dataStatus).
                then(function (promise) {
                    $scope.StatusDropDown = promise.statusDropDownData;
                });

            $scope.viewdetailsNew(dd);

            //var data = {
            // "ISMSLE_Id": dd.ismslE_Id,
            // "flag": "1"
            //};
            //apiService.create("ISM_Client_Project/StatusUpdateOnLoad", data).
            // then(function (promise) {
            //     $scope.StatusUpdateOnLoad = promise.statusUpdateOnLoad;

            // });

            //.finally(function () {

            //    var data = {
            //        "ISMSLE_Id": dd.ismslE_Id,
            //        "flag": "1"
            //    }
            //    apiService.create("ISM_Client_Project/StatusUpdateOnLoad", data).
            //        then(function (promise) {
            //            $scope.StatusUpdateOnLoad = promise.statusUpdateOnLoad;
            //            //if ($scope.StatusUpdateOnLoad.length > 0) {
            //            //    $scope.isModalVisible = true;
            //            //    $('#ViewStatus').modal('show');
            //            //} else {
            //            //    swal("No Data Found.");
            //            //}
            //        });
            //})
        }

        $scope.viewdetailsNew = function (dd) {
            var data = {
                "ISMSLE_Id": dd.ismslE_Id,
                "flag": "1"
            };
            apiService.create("ISM_Client_Project/StatusUpdateOnLoad", data).
                then(function (promise) {
                    $scope.StatusUpdateOnLoad = promise.statusUpdateOnLoad;
                    if ($scope.StatusUpdateOnLoad.length == 0) {
                        swal("No Data Found..");
                    }
                });
        }
        $scope.selectedEvent = null;

        $scope.selectEvent = function (evet) {
            $scope.selectedEvent = evet;
        };
        $scope.submitted = false;
        $scope.ISMSMST_Id = '';
        $scope.ISMSMST_StatusName = '';


        $scope.updatestatusname = function (evet) {
            var selectedStatusId = Number(evet);
            angular.forEach($scope.StatusDropDown, function (status) {
                if (status.ISMSMST_Id === selectedStatusId) {
                    $scope.ISMSMST_Id = status.ISMSMST_Id;
                    $scope.ISMSMST_StatusName = status.ISMSMST_StatusName;
                }
            });

        };

        $scope.submitted = false;
        $scope.Save_responseNew = function (ee) {
            $scope.submitted = true;
            $scope.selectedData = [];
            angular.forEach($scope.StatusUpdateOnLoad, function (aa) {

                if (aa.selected === true) {
                    $scope.selectedData.push({
                        ISMSLEDMPR_Id: aa.ISMSLEDMPR_Id,
                        ISMSMST_Id: aa.ISMSMST_Id,
                        ISMSLEDMPR_SendQuotationFlg: aa.ISMSLEDMPR_SendQuotationFlg,
                        ISMSLEDM_Id: aa.ISMSLEDM_Id
                    })
                }

            });


            //$scope.submitted = true;
            //$scope.ISMSMST_Id = ee[0].ISMSMST_Id;
            //$scope.ISMSMST_StatusName = ee[0].ISMSMST_StatusName;
            //$scope.remarks = ee[0].ISMSLEDMPR_Remarks;

            //var data = {
            //    "ISMSLEDMPR_Id" : ee[0].ISMSLEDMPR_Id,
            //    "ISMSMST_Id": $scope.ISMSMST_Id,
            //    "ISMSLEDMPR_SendQuotationFlg": $scope.ISMSLEDMPR_SendQuotationFlg,
            //    "ISMSLEDM_Id": ee[0].ISMSLEDM_Id
            //};

            var data = {
                "selectedDataArray": $scope.selectedData
            }

            if ($scope.selectedData != null && $scope.selectedData.length > 0) {
                apiService.create("ISM_Client_Project/StatusUpdate", data).
                    then(function (promise) {
                        if (promise.returndata === 'update') {
                            swal('Record Updated Successfully..');
                        }
                        else if (promise.returndata === 'Not Update') {
                            swal('Record Not Updated..');
                        }
                        else if (promise.returndata === 'Error') {
                            swal('Operation Failed!!!');
                        }
                        $state.reload();

                    });
            }
            else {
                swal("Please Select  Atleast one CheckBox");
            }

            $scope.submitted = false;
        }


        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.StatusUpdateOnLoad, function (itm) {

                itm.selected = toggleStatus;

            });
        }

        $scope.optionToggled = function () {

            $scope.all = $scope.StatusUpdateOnLoad.every(function (options) {

                return options.selected;
            });
        }

        $scope.Clearid = function () {
            $state.reload();
        }
    }

})();



