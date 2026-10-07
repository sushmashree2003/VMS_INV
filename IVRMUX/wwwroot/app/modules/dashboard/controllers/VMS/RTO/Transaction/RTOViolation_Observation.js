(function () {
    'use strict';
    angular.module('app').controller('RTOViolation_ObservationController', RTOViolation_ObservationController)
    RTOViolation_ObservationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function RTOViolation_ObservationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };


        $scope.submitted = false;

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
        $scope.RTOVOB_AnyViolationFlg = false;
        $scope.currentPage = 1;
        //-----------Tax Grid Select All
        $scope.ismeridian = true;
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.RTOVOB_Date = new Date();
        $scope.RTOVOB_FineAmountPaidFlg = false;
        $scope.RToVialationtemp = [];
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian; todate
        };
        $scope.vehicalListTemp = [];
        $scope.CreatedBy = "";
        $scope.RToVialation = [];
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("RTOViolation/loaddata", pageid).then(function (promise) {
                $scope.vehicalArray = promise.vehicalArray;
                $scope.emplist = promise.emplist;
                $scope.fineArray = promise.fineArray;
                $scope.RToVialationtemp = promise.rtO_VialationList;
                if ($scope.RToVialationtemp != null && $scope.RToVialationtemp.length > 0) {
                    for (var t = 0; t < $scope.RToVialationtemp.length; t++) {
                        $scope.RToVialation.push({
                            RTOVOB_DateTimeOfViolation: moment($scope.RToVialationtemp[t].RTOVOB_DateTimeOfViolation, 'HH:mm a').format(),                                                    
                            RTOVOB_Date: new Date($scope.RToVialationtemp[t].RTOVOB_Date),
                            HRME_Id: $scope.RToVialationtemp[t].HRME_Id,
                            TRMV_VehicleName: $scope.RToVialationtemp[t].TRMV_VehicleName,                           
                            TRMV_VehicleNo: $scope.RToVialationtemp[t].TRMV_VehicleNo,
                            EMPNAME: $scope.RToVialationtemp[t].EMPNAME,
                            Applicationuser: $scope.RToVialationtemp[t].Applicationuser,
                            TRMV_Id: $scope.RToVialationtemp[t].TRMV_Id,
                            RTOMFP_Id: $scope.RToVialationtemp[t].RTOMFP_Id,
                            RTOVOB_ViolationDescription: $scope.RToVialationtemp[t].RTOVOB_ViolationDescription,
                            RTOVOB_FineAmount: $scope.RToVialationtemp[t].RTOVOB_FineAmount,
                            RTOVOB_FineAmountPaidFlg: $scope.RToVialationtemp[t].RTOVOB_FineAmountPaidFlg,
                            RTOVOB_AnyViolationFlg: $scope.RToVialationtemp[t].RTOVOB_AnyViolationFlg,
                            RTOVOB_ActiveFlag: $scope.RToVialationtemp[t].RTOVOB_ActiveFlag,
                            RTOVOB_Id: $scope.RToVialationtemp[t].RTOVOB_Id,
                           
                        });
                    }
                }
                // $scope.RToVialation = promise.rtO_VialationList;
                $scope.CreatedBy = promise.createdBy;

            });
        };
        // RTOViolation
        $scope.Parmeter = function (RTOMFP_Id) {
            $scope.RTOListTemplate();   
            if (RTOMFP_Id != null && RTOMFP_Id > 0) {

                if ($scope.fineArray != null && $scope.fineArray.length > 0) {
                    for (var i = 0; i < $scope.fineArray.length; i++) {
                        if (RTOMFP_Id == $scope.fineArray[i].rtomfP_Id) {
                            $scope.RTOVOB_FineAmount = $scope.fineArray[i].rtomfP_FineAmount;
                            return;
                        }
                    }
                }
            }
            else {
                $scope.RTOMFP_Id = "";
                $scope.RTOVOB_FineAmount = "";
                
            }
            
        }
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
            //fineArray
            if ($scope.fineArray != null && $scope.fineArray.length > 0) {
                for (var i = 0; i < $scope.fineArray.length; i++) {
                    if ($scope.RTOMFP_Id == $scope.fineArray[i].rtomfP_Id) {
                        $scope.ParameterName = $scope.fineArray[i].rtomfP_FineParameterName;

                    }
                }
            }
            
           
        }
        $scope.savedata = function () {
            $scope.submitted = true;
            $scope.uploadarray = [];
            if ($scope.myForm.$valid) {
                if ($scope.materaldocuupload.length > 0) {
                    angular.forEach($scope.materaldocuupload, function (itm) {
                        if (itm.vbabrsF_FilePath !== undefined && itm.vbabrsF_FilePath !== null && itm.vbabrsF_FilePath !== "") {
                            $scope.uploadarray.push({
                                RTOVOBFL_FileName: itm.vbabrsF_FileName,
                                RTOVOBFL_FilePath: itm.vbabrsF_FilePath,
                                RTOVOBFL_Remarks: itm.vbabrsF_Filedesc
                            });
                        }
                    });
                }
                var RTOVOB_Id = 0;
                if ($scope.RTOVOB_Id > 0) {
                    RTOVOB_Id = $scope.RTOVOB_Id;
                }
                var RTOVOB_DateTimeOfViolation = $scope.RTOVOB_DateTimeOfViolation;

                var Template = document.getElementById("empmailPP").innerHTML;

                $scope.sresult = $filter('date')(RTOVOB_DateTimeOfViolation, 'HH:mm');
                var data = {
                    "HRME_Id": $scope.HRME_Id,
                    "TRMV_Id": $scope.TRMV_Id,
                    "RTOVOB_Id": RTOVOB_Id,
                    "RTOVOB_Date": new Date($scope.RTOVOB_Date).toDateString(),
                    "RTOVOB_AnyViolationFlg": $scope.RTOVOB_AnyViolationFlg,
                    "VMSFOVOB_DamageDescription": $scope.VMSFOVOB_DamageDescription,
                    "RTOVOB_DateTimeOfViolation": $scope.sresult,
                    "RTOMFP_Id": $scope.RTOMFP_Id,
                    "RTOVOB_ViolationDescription": $scope.RTOVOB_ViolationDescription,
                    "RTOVOB_FineAmount": $scope.RTOVOB_FineAmount,
                    "ViolationFiles": $scope.uploadarray,
                    "RTOVOB_FineAmountPaidFlg": $scope.RTOVOB_FineAmountPaidFlg,
                    Template: Template,
                };
                apiService.create("RTOViolation/savedata", data).then(function (promise) {
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
        $scope.edit = function (user) {
            var data = {
                "RTOVOB_Id": user.RTOVOB_Id
            }
            apiService.create("RTOViolation/RecieptReport", data).
                then(function (promise) {
                    if (promise.editlist != null && promise.editlist.length > 0) {
                        $scope.RTOVOB_Id = promise.editlist[0].rtovoB_Id;
                        $scope.HRME_Id = promise.editlist[0].hrmE_Id;
                        $scope.TRMV_Id = promise.editlist[0].trmV_Id;
                        $scope.RTOVOB_FineAmount = promise.editlist[0].rtovoB_FineAmount;
                        $scope.RTOMFP_Id = promise.editlist[0].rtomfP_Id;
                        $scope.RTOVOB_Date = new Date(promise.editlist[0].rtovoB_Date);
                        $scope.RTOVOB_AnyViolationFlg = promise.editlist[0].rtovoB_AnyViolationFlg;
                        $scope.RTOVOB_ViolationDescription = promise.editlist[0].rtovoB_ViolationDescription;
                        $scope.RTOVOB_DateTimeOfViolation = moment(promise.editlist[0].rtovoB_DateTimeOfViolation, 'HH:mm a').format();
                        if (promise.filist != null && promise.filist.length > 0) {
                            $scope.materaldocuupload = promise.filist;
                            angular.forEach($scope.materaldocuupload, function (dd) {
                                dd.vbabrsF_Filedesc = dd.rtovobfL_Remarks;
                                dd.vbabrsF_FilePath = dd.rtovobfL_FilePath;
                                dd.ismclT_FileName = dd.rtovobfL_FileName;
                            });
                        }
                    }

                })

        }
        $scope.Deletedata = function (item, SweetAlert) {
            var data = {
                "RTOVOB_Id": item.RTOVOB_Id
            }
            var dystring = "";
            if (item.RTOVOB_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.RTOVOB_ActiveFlag == false) {
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
                        apiService.create("RTOViolation/onchange", data).
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
                //else if (input.files[0].type === "application/vnd.ms-excel" && input.files[0].size <= 5242880) {
                //    UploaddianmateralPhoto(document);
                //}
                //else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && input.files[0].size <= 5242880) {
                //    UploaddianmateralPhoto(document);
                //}
                //else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 5242880) {
                //    UploaddianmateralPhoto(document);
                //}
                //else if (input.files[0].type === "application/msword" && input.files[0].size <= 5242880) {
                //    UploaddianmateralPhoto(document);
                //}
                
                //else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 5242880) {
                //    UploaddianmateralPhoto(document);
                //}
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
        $scope.View = function (user) {
            var data = {
                "RTOVOB_Id": user.RTOVOB_Id
            }
            apiService.create("RTOViolation/RecieptReport", data).
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
