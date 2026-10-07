(function () {
    'use strict';
    angular
        .module('app')
        .controller('FrontOfficeMonthEndReport', FrontOfficeMonthEndReport)
    FrontOfficeMonthEndReport.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache','$q']
    function FrontOfficeMonthEndReport($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache, $q) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.PTax = {};
        // Get form Details at onload 
        $scope.onloadGetData = function () {
            var pageid = 2;
            apiService.getURI("FrontOffice/getalldetails1", pageid).then(function (promise) {
                if (promise.employees !== null && promise.employees.length > 0) {
                    $scope.employeedropdown = promise.employees;

                }
                $scope.griddata = promise.griddata;
            });
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //=========//$scope.obj = {};==========Active/Deactive=====================////////////////////////////
        $scope.deactive = function (obj) {

            var mgs = "";
            var confirmmgs = "";
            if (obj.HRALD_Activeflag === true) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";

            }

            var data = {
                "HRALD_ID": obj.HRALD_ID,
                "HRME_Id": obj.HRME_Id,

            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("FrontOffice/deactivate", data).then(function (promise) {

                            if (promise.returnmsg === "true") {
                                swal("Record " + confirmmgs + " " + "successfully", "", "success");
                            }
                            else {
                                swal("Record " + mgs + " Failed", "", "error");
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        $scope.start_Date = new Date();
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////      

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //save added by Rohan
        $scope.submitted = false;
        $scope.saveData = function (user) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HRALD_Type": $scope.loptype,
                    "HRALD_Remarks": $scope.remarks,
                    "HRME_Id": $scope.hrmE_Id.hrmE_Id,
                    "lopdate": new Date($scope.start_Date).toDateString(),
                    "HRALD_ID": $scope.HRALD_ID,                   
                    "HRALD_File": $scope.filepath
                };
                apiService.create("FrontOffice/Lopsave", data).
                    then(function (promise) {
                        if (promise.returnmsg != "") {
                            if (promise.returnmsg == "Update") {
                                swal('Record Updated Successfully');
                                $state.reload();
                            }
                            else if (promise.returnmsg == "Add") {
                                swal('Record Saved Successfully');
                                $state.reload();
                            }
                            else if (promise.returnmsg == "false") {
                                swal('Record Not Saved/Updated successfully');
                            }
                            else if (promise.returnmsg === "Duplicate") {
                                swal('Category Already Exist');
                            }
                           
                        }
                        else {
                            swal('Operation Failed');
                            $state.reload();
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.Clearid = function () {
            $state.reload();
        };       







        $scope.SelectedFileForUploadzdBOSBOE = [];
        $scope.selectFileforUploadzdBOSBOE1 = function (input) {
            $scope.SelectedFileForUploadzdBOSBOE = input.files;
            //$('#blah').removeAttr('src');
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                $scope.extention = nameArray[nameArray.length - 1];
                if (($scope.extention === "JPEG" || $scope.extention === "jpg" || $scope.extention === "JPG" || $scope.extention === "png" || $scope.extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    //reader.onload = function (e) {
                    //    $('#blah')
                    //        .attr('src', e.target.result);

                    //    $('#documentid') //hrmedS_Id
                    //        .attr('src', e.target.result);
                    //};
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentBOSBOE();
                }
                else if ($scope.extention !== "JPEG" && $scope.extention !== "png" && $scope.extention !== "jpg" && $scope.extention !== "JPG" && $scope.extention !== "pdf") {
                    $('#documentid').removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    $('#documentid').removeAttr('src');
                    swal("Document size should be less than 2MB");
                    return;
                }
            }
        };

        function UploadEmployeeDocumentBOSBOE() {

            var formData = new FormData();
            for (var i = 0; i <= $scope.SelectedFileForUploadzdBOSBOE.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdBOSBOE[i]);
            }

            formData.append("Id", 0);

            var sample1 = Object.fromEntries(formData.entries());

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
                        $scope.filepath = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#documentid').removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.showmodaldetailsBOSBOE = function (filepath) {
            //$('#preview').removeAttr('src');
            var filename = filepath.toString();
            var nameArray = filename.split('.');
            $scope.extention = nameArray[nameArray.length - 1];
            if ($scope.extention === "jpg" || $scope.extention === "jpeg" || $scope.extention === "JPG" || $scope.extention === "JPEG" || $scope.extention === "png"
                || $scope.extention === "PNG") {
                $('#preview').attr('src', filepath);
                $('#myimagePreview').modal('show');
            }
            else if ($scope.extention === "doc" || $scope.extention === "docx" || $scope.extention === "xls" || $scope.extention === "xlsx") {
                //$('#preview').removeAttr('src');
                $('#preview').attr('src', filepath);
            }
            else if ($scope.extention === "pdf") {
                var imagedownload = filepath;
                $scope.content = "";
                var fileURL = "";
                var file = "";
                $http.get(imagedownload, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        $scope.content = $sce.trustAsResourceUrl(fileURL);
                        $('#showpdf').modal('show');
                    });
            }
        };






    }

    

})();