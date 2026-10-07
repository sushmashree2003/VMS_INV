(function () {
    'use strict';
    angular.module('app').controller('FO_Vehicle_ObservationController', FO_Vehicle_ObservationController)
    FO_Vehicle_ObservationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function FO_Vehicle_ObservationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };


        $scope.submitted = false;
        $scope.CreatedBy = "";
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
        var configsettingsnew = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        $scope.itemsPerPage = paginationformasters;
        $scope.VMSFOVOB_AnyDamageFlg = false;
        $scope.currentPage = 1;
        $scope.VMSFOVOB_DateTime = new Date();
        //-----------Tax Grid Select All
        $scope.ismeridian = true;
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.VMSVKEYS_DateTime = new Date();
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian; todate
        };
        $scope.vehicalListTemp = [];
        $scope.loaddata = function () {            
            var pageid = 2;
            apiService.getURI("Vehicle_Observation/loaddata", pageid).then(function (promise) {
                $scope.vehicalArray = promise.vehicalArray;
                $scope.emplist = promise.emplist;
                $scope.obserVationList = promise.obserVationList;
                $scope.CreatedBy = promise.createdBy;
            });
        };
        $scope.RTOListTemplate = function () {
            $scope.VehicaleName = ""; $scope.DriverName = ""; $scope.ParameterName = "";

            if ($scope.vehicalArray != null && $scope.vehicalArray.length > 0) {
                for (var i = 0; i < $scope.vehicalArray.length; i++) {
                    if ($scope.TRMV_Id == $scope.vehicalArray[i].trmV_Id) {
                        $scope.VehicaleName = $scope.vehicalArray[i].trmV_VehicleName + '/' + $scope.vehicalArray[i].trmV_VehicleNo;
                        //trmV_VehicleNo
                    }
                }
            }
            if ($scope.emplist != null && $scope.emplist.length > 0) {
                for (var i = 0; i < $scope.emplist.length; i++) {
                    if ($scope.HRME_Id == $scope.emplist[i].HRME_Id) {
                        $scope.DriverName = $scope.emplist[i].EMPNAME;

                    }
                }
            }

        }
        $scope.savedata = function () {
            $scope.submitted = true;
            $scope.uploadarray = [];
            if ($scope.myForm.$valid) {
                var Template = document.getElementById("empmailPP").innerHTML;

                if ($scope.materaldocuupload.length > 0) {
                    angular.forEach($scope.materaldocuupload, function (itm) {
                        if (itm.vbabrsF_FilePath !== undefined && itm.vbabrsF_FilePath !== null && itm.vbabrsF_FilePath !== "") {
                            $scope.uploadarray.push({
                                VMSFOVOBFL_FileName: itm.vbabrsF_FileName,
                                VMSFOVOBFL_FilePath: itm.vbabrsF_FilePath,
                                VMSFOVOBFL_Filedesc: itm.vbabrsF_Filedesc
                            });
                        }
                    });
                }
                if ($scope.VMSFOVOB_AnyDamageFlg == true) {
                    if ($scope.uploadarray != null && $scope.uploadarray.length) {

                    }
                    else {
                        swal("Please Uplaod Atleast One File !")
                        return;
                    }
                }
                var VMSFOVOB_Id = 0;
                if ($scope.VMSFOVOB_Id > 0) {
                    VMSFOVOB_Id = $scope.VMSFOVOB_Id;
                }          
                var data = {
                    "HRME_Id": $scope.HRME_Id,
                    "TRMV_Id": $scope.TRMV_Id,                   
                    "VMSFOVOB_Id": VMSFOVOB_Id,
                    "VMSFOVOB_DateTime": new Date($scope.VMSFOVOB_DateTime).toDateString(),
                    "VMSFOVOB_AnyDamageFlg": $scope.VMSFOVOB_AnyDamageFlg,
                    "VMSFOVOB_DamageDescription": $scope.VMSFOVOB_DamageDescription,
                    "Observation_Fileslist": $scope.uploadarray,
                    Template: Template,
                };
                apiService.create("Vehicle_Observation/savedata", data).then(function (promise) {
                    if (promise.returnval == "save") {
                        swal("Record Saved Sucessfully !");
                    }
                    else if (promise.returnval == "Notsave") {
                        swal("Record Not Saved !");
                    }
                    else if (promise.returnval == "duplicate") {
                        swal("Record duplicate !");
                    }
                    if (promise.returnval == "update") {
                        swal("Record Update Sucessfully !");
                    }
                    else if (promise.returnval == "Notupdate") {
                        swal("Record Not Update !");
                    }
                    else if (promise.returnval == "admin") {
                        swal("Please Contact Administrator !");
                    }
                    $state.reload();
                });
            }

        };
        $scope.View = function (user) {
            var data = {
                "VMSFOVOB_Id": user.VMSFOVOB_Id
            }
            apiService.create("Vehicle_Observation/RecieptReport", data).
                then(function (promise) {
                    if (promise.filist != null && promise.filist.length > 0) {
                        $scope.GetFile = promise.filist;
                        $('#ViewDetails').modal('show');
                       
                    }
                    else {
                        swal("File Is Not Attached !");
                        $('#ViewDetails').modal('hide');
                    }

                })
        }
        $scope.edit = function (user) {
            var data = {
                "VMSFOVOB_Id": user.VMSFOVOB_Id
            }
            apiService.create("Vehicle_Observation/RecieptReport", data).
                then(function (promise) {
                    if (promise.editlist != null && promise.editlist.length > 0) {
                        $scope.VMSFOVOB_Id = promise.editlist[0].vmsfovoB_Id;
                        $scope.HRME_Id = promise.editlist[0].hrmE_Id;
                        $scope.TRMV_Id = promise.editlist[0].trmV_Id;
                        $scope.VMSFOVOB_DateTime = new Date(promise.editlist[0].vmsfovoB_DateTime);
                        $scope.VMSFOVOB_AnyDamageFlg = promise.editlist[0].vmsfovoB_AnyDamageFlg;
                        $scope.VMSFOVOB_DamageDescription = promise.editlist[0].vmsfovoB_DamageDescription;                       
                        if (promise.filist != null && promise.filist.length > 0) {
                            $scope.materaldocuupload = promise.filist;
                            angular.forEach($scope.materaldocuupload, function (dd) {
                                dd.vbabrsF_Filedesc = dd.vmsfovobfL_Filedesc;
                                dd.vbabrsF_FilePath = dd.vmsfovobfL_FilePath;
                                dd.ismclT_FileName = dd.vmsfovobfL_FileName;
                            });
                        }
                    }

                })

        }
        $scope.Deletedata = function (item, SweetAlert) {
            var data = {
                "VMSFOVOB_Id": item.VMSFOVOB_Id
            }
            var dystring = "";
            if (item.VMSFOVOB_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.VMSFOVOB_ActiveFlag == false) {
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
                        apiService.create("Vehicle_Observation/onchange", data).
                            then(function (promise) {
                                if (promise.returnval == "active") {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else if (promise.returnval == "notactive") {
                                    swal("Record Not Deactivate !");
                                }
                                else if (promise.returnval == "admin") {
                                    swal("Please Contact Administrator !");
                                }
                                $state.reload();

                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };
        //addded by
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

          
        };



        $scope.SelectedFileForUploadzd = [];
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
