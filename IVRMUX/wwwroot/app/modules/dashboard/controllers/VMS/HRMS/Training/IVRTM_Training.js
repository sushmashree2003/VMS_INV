(function () {
    'use strict';
    angular.module('app').controller('IVRTM_TrainingController', IVRTM_TrainingController)

    IVRTM_TrainingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter','$q']
    function IVRTM_TrainingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {

        $scope.vapstrainign = false;
        $scope.submitted = false;
        $scope.editflag = false;
        $scope.ddate = {};
        $scope.Trainingdate = new Date();
        $scope.obj = {};

        var weburl;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            weburl =  ivrmcofigsettings[0].ivrmgC_Webiste;
        } else {
            paginationformasters = 10;
            weburl = "";
        }




       
        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        };

        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("IVRTM_Training/onloaddata", pageid).then(function (promise) {

                if (promise.emp_deatils != null && promise.emp_deatils.length > 0 ) {
                    $scope.emp_deatils = promise.emp_deatils;
                    $scope.email = $scope.emp_deatils[0].HRME_EmailId;
                    $scope.emp_code = $scope.emp_deatils[0].HRME_EmployeeCode;
                    $scope.mobile_num = $scope.emp_deatils[0].HRME_MobileNo;
                    $scope.doj = $scope.emp_deatils[0].HRME_DOJ;
                    $scope.aadhar_num = $scope.emp_deatils[0].HRME_AadharCardNo;
                    $scope.pan_num = $scope.emp_deatils[0].HRME_PANCardNo;
                    $scope.empname = $scope.emp_deatils[0].employeename;

                }

                if (promise.getloaddetails != null && promise.getloaddetails.length > 0) {
                    $scope.getloaddetails = promise.getloaddetails;
                    $scope.presentCountgrid = promise.getloaddetails.length;

                    angular.forEach($scope.getloaddetails, function (itm) {
                        if (itm.ivrmtT_Status =="Completed") {
                           // $('#updatetrainingfeedback').modal('show');
                            //$scope.ivrmtT_Id = item.ivrmtT_Id
                            //$scope.IVRMTT_Remarks = item.ivrmtT_Remarks
                            //$scope.IVRMTT_TrainingMode = item.ivrmtT_TrainingMode
                            //$scope.ivrmtT_TentetiveStartTime = moment(item.ivrmtT_TentetiveStartTime, 'HH:mm a').format();
                            //$scope.ivrmtT_TentetiveEndTime = moment(item.ivrmtT_TentetiveEndTime, 'HH:mm a').format();
                            //$scope.Trainingdate = new Date(item.ivrmtT_TentetiveDate); 



                        }
                    });

                }

                
            });
        };

        $scope.saverecord = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "IVRMTT_Id": $scope.ivrmtT_Id,
                    "IVRMTT_EmployeeName": $scope.empname,
                    "IVRMTT_EmployeeEmail": $scope.email,
                    "IVRMTT_EmployeePhone": $scope.mobile_num,
                    "IVRMTT_Remarks": $scope.IVRMTT_Remarks,
                    "IVRMTT_TrainingMode": $scope.IVRMTT_TrainingMode,
                    "IVRMTT_TentetiveStartTime": $filter('date')($scope.ivrmtT_TentetiveStartTime, "HH:mm"),
                    "IVRMTT_TentetiveEndTime": $filter('date')($scope.ivrmtT_TentetiveEndTime, "HH:mm"),
                    "TrainingDate": new Date($scope.Trainingdate).toDateString(),
                    "ClientURL": weburl
                    //"HREXTTRN_EndDate": new Date($scope.hrexttrN_EndDate).toDateString()
                };
                apiService.create("IVRTM_Training/saverecord", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message === "Duplicate") {
                            swal("Record Already Exists");
                        } else if (promise.message === "Add") {                        
                            swal("Record Saved Successfully"); 
                            $state.reload();
                        }
                        else if (promise.message === "Update") {
                            swal("Record Updated Successfully");   
                            $state.reload();
                        } else if (promise.message === "Error") {
                            swal("Failed To Save/ Update Record");
                        } else {
                            swal("Something Went Wrong Contact Administrator");
                        }
                        
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.EditData = function (item) {                   
                    
            $scope.ivrmtT_Id = item.ivrmtT_Id
            $scope.IVRMTT_Remarks = item.ivrmtT_Remarks
            $scope.IVRMTT_TrainingMode = item.ivrmtT_TrainingMode
            $scope.ivrmtT_TentetiveStartTime = moment(item.ivrmtT_TentetiveStartTime, 'HH:mm a').format();
            $scope.ivrmtT_TentetiveEndTime = moment(item.ivrmtT_TentetiveEndTime, 'HH:mm a').format();
            $scope.Trainingdate = new Date(item.ivrmtT_TentetiveDate);              
                    

        }


        $scope.deactiveY = function (user, SweetAlert) {   
            $scope.HREXTTRN_Id = user.hrexttrN_Id;

           var data = {
               "IVRMTT_Id": user.ivrmtT_Id,
          };
            var dystring = "";
            if (user.ivrmtT_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (user.ivrmtT_ActiveFlag === false) {
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
                        apiService.create("IVRTM_Training/deactiveY", data).then(function (promise) {
                        
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            
                        });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }
                });
        };




        $scope.gettrainer = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "IVRMTT_Id": $scope.ivrmtT_Id,                  
                };
                apiService.create("IVRTM_Training/gettrainer", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.gettrainer = promise.gettrainer

                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };











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
        console.log("TADA  Upload  :" + data);
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
    ///addd
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
    $scope.previewimg_url = function (url) {
        $scope.urlnew = url;
        $window.open($scope.urlnew)
    }


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
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

        //totalhourscal
        $scope.Caltotalhours = function () {
            $scope.srresult = [];
            $scope.erresult = [];
            $scope.sresult = $filter('date')($scope.hrexttrN_StartTime, 'HH:mm:ss a');
            $scope.eresult = $filter('date')($scope.hrexttrN_EndTime, 'HH:mm:ss a');
            var startTime = moment($scope.sresult, "HH:mm:ss a");
            var endTime = moment($scope.eresult, "HH:mm:ss a");
            var duration = moment.duration(endTime.diff(startTime));
            var hours = parseInt(duration.asHours());
            $scope.hrexttrN_TotalHrs = hours;
        
        }


    }
})();

