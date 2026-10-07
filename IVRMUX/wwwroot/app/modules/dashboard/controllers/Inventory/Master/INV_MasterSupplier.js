
(function () {
    'use strict';
    angular.module('app').controller('INV_MasterSupplierController', INV_MasterSupplierController);
    INV_MasterSupplierController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q','$window']
    function INV_MasterSupplierController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $window) {


        $scope.editflag = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //-------------------------------------------------------------------

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("INV_MasterSupplier/getloaddata", pageid).then(function (promise) {
                $scope.get_supplier = promise.get_supplier;
                $scope.getinstitution = promise.getinstitution;
                $scope.MI_Id = promise.mI_Id;
                $scope.presentCountgrid = $scope.get_supplier.length;
            })
        };

        $scope.OnChangeInstitution = function () {
            $scope.get_supplier = [];
            var data = {
                "MI_Id": $scope.MI_Id
            };
            apiService.create("INV_MasterSupplier/OnChangeInstitution", data).then(function (promise) {
                $scope.get_supplier = promise.get_supplier;
                $scope.presentCountgrid = $scope.get_supplier.length;
            });
        };

        //---------------------------------Save--------------------------------------------
        //Tax
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.uploadarray = [];
                if ($scope.materaldocuupload.length > 0) {
                    angular.forEach($scope.materaldocuupload, function (itm) {
                        if (itm.vbabrsF_FilePath !== undefined && itm.vbabrsF_FilePath !== null && itm.vbabrsF_FilePath !== "") {
                            $scope.uploadarray.push({
                                VBABRSF_FileName: itm.vbabrsF_FileName,
                                VBABRSF_FilePath: itm.vbabrsF_FilePath,
                                VBABRSF_Filedesc: itm.vbabrsF_Filedesc
                            });
                        }
                    });
                }
                var data = {
                    "INVMS_SupplierName": $scope.invmS_SupplierName,
                    "INVMS_SupplierCode": $scope.invmS_SupplierCode,
                    "INVMS_SupplierConatctPerson": $scope.invmS_SupplierConatctPerson,
                    "INVMS_SupplierConatctNo": $scope.invmS_SupplierConatctNo,
                    "INVMS_EmailId": $scope.invmS_EmailId,
                    "INVMS_GSTNo": $scope.invmS_GSTNo,
                    "INVMS_SupplierAddress": $scope.invmS_SupplierAddress,
                    "INVMS_Id": $scope.invmS_Id,
                    "MI_Id": $scope.MI_Id,
                    "INVMS_BankAccountNo": $scope.invmS_BankAccountNo,
                    "INVMS_AccountName": $scope.invmS_AccountName,
                    "INVMS_BankName": $scope.invmS_BankName,
                    "INVMS_IFSCCode": $scope.invmS_IFSCCode,
                    "INVMS_BranchName": $scope.invmS_BranchName,
                    "INVMS_BankAddress": $scope.invmS_BankAddress,
                    "INVMS_AccountType": $scope.invmS_AccountType,
                    filelist: $scope.uploadarray,

                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };

                apiService.create("INV_MasterSupplier/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.invmS_Id == 0 || promise.invmS_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invmS_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmS_Id == 0 || promise.invmS_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmS_Id > 0) {
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

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        }

        $scope.deactive = function (item, SweetAlert) {
            $scope.INVMS_Id = item.invmS_Id;
            var dystring = "";
            if (item.invmS_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invmS_ActiveFlg == false) {
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
                        var data = {
                            "INVMS_Id": item.invmS_Id,
                            "message": ""
                        }
                        apiService.create("INV_MasterSupplier/deactive", data).then(function (promise) {
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
        };

        $scope.edit = function (item) {
            $scope.materaldocuupload = [];
            $scope.materaldocuupload = [{ id: 'materal' }];
            $scope.invmS_SupplierName = item.invmS_SupplierName;
            $scope.invmS_SupplierCode = item.invmS_SupplierCode;
            $scope.invmS_SupplierConatctPerson = item.invmS_SupplierConatctPerson;
            $scope.invmS_SupplierConatctNo = item.invmS_SupplierConatctNo;
            $scope.invmS_EmailId = item.invmS_EmailId;
            $scope.invmS_GSTNo = item.invmS_GSTNo;
            $scope.invmS_SupplierAddress = item.invmS_SupplierAddress;
            $scope.invmS_Id = item.invmS_Id;
            $scope.MI_Id = item.mI_Id;
            $scope.invmS_BankAccountNo = item.invmS_BankAccountNo;
            $scope.invmS_AccountName = item.invmS_AccountName;
            $scope.invmS_BankName = item.invmS_BankName;
            $scope.invmS_IFSCCode = item.invmS_IFSCCode;
            $scope.invmS_BranchName = item.invmS_BranchName;
            $scope.invmS_BankAddress = item.invmS_BankAddress;
            $scope.invmS_AccountType = item.invmS_AccountType;
            $scope.editflag = true;
            var data = {                           
                "INVMS_Id": item.invmS_Id,
                "message":"edit"
            }
            apiService.create("INV_MasterSupplier/deactive", data).then(function (promise) {
               
                if (promise.getFilesArray != null && promise.getFilesArray.length > 0) {
                    $scope.materaldocuupload = promise.getFilesArray;
                    angular.forEach($scope.materaldocuupload, function (dd) {
                        dd.vbabrsF_Filedesc = dd.invmsF_Filedesc;
                        dd.vbabrsF_FilePath = dd.invmsF_FilePath;
                        dd.vbabrsF_FileName = dd.invmsF_FileName;

                        //dd.invmsF_Filedesc = dd.vbabrsF_Filedesc;
                        //dd.invmsF_FilePath = dd.vbabrsF_FilePath;
                        //dd.invmsF_FileName = dd.vbabrsF_FileName;
                    });
                }
            })

        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.searchValue = '';
        //
        $scope.materaldocuupload = [{ id: 'materal' }];

        $scope.addgrnrows = function () {
            var newItemNo = $scope.materaldocuupload.length + 1;
            if (newItemNo <= 10) {
                $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
            }
        };
        $scope.removegrnrows = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

            if ($scope.materaldocuupload.length === 0) {               //data
            }
        };
        //
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
        };

        $scope.previewimg_new = function (img) {
            $scope.vbabrsF_FilePath = img;
            var img = $scope.vbabrsF_FilePath;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.vbabrsF_FilePath);
                    $('#myimagePreview').modal('show');
                    //$('#myModalfile').modal('show');
                }
                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                    $window.open($scope.vbabrsF_FilePath);
                }
                else if (lastelement == 'pdf') {
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.vbabrsF_FilePath;
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
                $window.open($scope.vbabrsF_FilePath)
            }
        };

    }
})();