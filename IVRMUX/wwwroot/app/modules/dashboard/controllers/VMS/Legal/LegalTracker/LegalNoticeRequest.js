(function () {
    'use strict';
    angular
        .module('app')
        .controller('ivrm_LegalProcessController', ivrm_LegalProcessController)
    ivrm_LegalProcessController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter', '$q']
    function ivrm_LegalProcessController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter, $q) {
        $scope.obj = {};
        $scope.cat = {};

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        

        $scope.Clearid = function () {
            $state.reload();
            $scope.loaddata();
        }
      
        $scope.Onloaddata = function () {

            var pageid = 1;


            apiService.getURI("ivrm_LegalProcess/onloaddata", pageid).then(function (promise) {
                
                    if (promise.employees !== null && promise.employees.length > 0) {
                        $scope.employeedropdown = promise.employees;

                }
                $scope.dashdata = promise.admindata;
                $scope.category = promise.category;
                    $scope.clientlist = promise.clientlist;
                    $scope.vendorlist = promise.vendorlist;
                    $scope.griddata = promise.griddata;
                });
             
                    
        
        }

        //Added by Rohan

        $scope.onradiobuttonclick = function (data) {
            var input = {
                "LNC_Id": data.lnC_Id
            }
            apiService.create("ivrm_LegalProcess/onclickloaddata1",input).then(function (promise) {

                $scope.admindata = promise.admindata;

            });
        }

     
           
       
        $scope.interacted = function (field) {
            return $scope.submitted;
        };        

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
            console.log("LEGAL  Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= 1; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadlegaldocuments", formData,
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


        //$scope.SaveMasterType = function () {
        //    if ($scope.myForm1.$valid) {
        //        var data = {
        //            "HRME_Id": $scope.hrmE_Id.hrmE_Id,
        //            "ISMMCLT_Id": $scope.ismmclT_Id,
        //            "INVMS_Id": $scope.invmS_Id,
        //            "LNR_ActiveFlag": $scope.LNR_ActiveFlag,
        //            "LNR_Remarks": $scope.lnR_Remarks  
        //        };

        //        apiService.create("ivrm_LegalProcess/savedetail", data).then(function (promise) {
        //            if (promise !== null) {
        //                if (promise.returnmsg == "Duplicate") {
        //                    swal("Record Already Exists");
        //                } else if (promise.returnmsg === "Add") {
        //                    if (promise.returnval === true) {
        //                        swal("Record Saved Successfully");
        //                    } else {
        //                        swal("Failed To Save Record");
        //                    }
        //                } else if (promise.returnmsg === "Update") {
        //                    if (promise.returnval === true) {
        //                        swal("Record Updated Successfully");
        //                    } else {
        //                        swal("Failed To Update Record");
        //                    }
        //                } else {
        //                    swal("Failed To Update/Save Record");
        //                }
        //                $scope.OnChangeMasteTypeInsitution();
        //            }
        //        });
        //    } else {
        //        $scope.submitted1 = true;
        //    }
        //};

        $scope.obj1 = {};
        $scope.submitted = false;

        $scope.saveData = function (option) {
            $scope.submitted = true;
            $scope.uploadarray = [];

            if ($scope.materaldocuupload.length > 0) {
                angular.forEach($scope.materaldocuupload, function (itm) {
                    if (
                        itm.ismclT_FilePath !== undefined &&
                        itm.ismclT_FilePath !== null &&
                        itm.ismclT_FilePath !== ""
                    ) {
                        $scope.uploadarray.push({
                            LNRD_Document: itm.ismclT_FilePath,
                        });
                    }
                });
            }

            if ($scope.myForm.$valid) {
                var Id = "";

                if ($scope.obj.catg1 == 1) {
                    
                    $scope.lnr_id = option.catg12.lnR_Id ;
                    Id = $scope.obj1.catg12.hrmE_Id;


                } else if ($scope.obj.catg1 == 2) {
                    Id = $scope.obj1.catg13.ismmclT_Id;
                    $scope.lnr_id = option.catg13.lnR_Id;
                } else {
                    Id = $scope.obj1.catg14.invmS_Id;
                    $scope.lnr_id = option.catg14.lnR_Id;
                }
            

                var list = {
                    "LNC_Id": $scope.obj.catg1,
                    "RequestingFor_id": Id,
                    "LNR_Remarks": $scope.lnR_Remarks,
                    "FilePath_Array": $scope.uploadarray,
                    "Lnr_ID": $scope.lnr_id 
                };

                apiService.create("ivrm_LegalProcess/savedetail", list, Id).then(function (promise) {
                    if (promise.returnvalue !== "") {
                        if (promise.returnvales == "Update") {
                            swal("Record Updated Successfully");
                        } else if (promise.returnvales == "Add") {
                            swal("Record Saved Successfully");
                        } else if (promise.returnvales == "false") {
                            swal("Record Not Saved/Updated successfully");
                        } else if (promise.returnvales === "Duplicate") {
                            swal("Category Already Exist");
                        }
                        else if (promise.update == "updated"){
                            swal("Record Saved Successfully","","success");//this is for legal update
                        }
                        $state.reload();
                    } else {
                        swal("Operation Failed");
                        $state.reload();
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };



        


        $scope.Clearid = function () {
            $state.reload();
        };
    }


}) ();