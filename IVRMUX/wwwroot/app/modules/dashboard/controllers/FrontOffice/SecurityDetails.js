(function () {
    'use strict';
    angular
        .module('app')
        .controller('SecurityDetailsController', SecurityDetailsController)

    SecurityDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce'];
    function SecurityDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce) {

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.itemsPerPage1 = 10;
        $scope.currentPage1 = 1;
        $scope.page1 = "page1";
        $scope.search = " ";

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.submitted = false;
        $scope.getdata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "ISMSD_Id": $scope.ISMSD_Id,
                    "ISMSD_FirstName": $scope.ismsD_FirstName,
                    "ISMSD_MiddleName": $scope.ismsD_MiddleName,
                    "ISMSD_LastName": $scope.ismsD_LastName,
                    "ISMSD_MobileNo": $scope.ismsD_MobileNo,
                    "ISMSD_EmailId": $scope.ismsD_EmailId,
                    "ISMSD_PerStreet": $scope.ismsD_PerStreet,
                    "ISMSD_PerArea": $scope.ismsD_PerArea,
                    "ISMSD_PerCity": $scope.ismsD_PerCity,
                    "ISMSD_PerPincode": $scope.ismsD_PerPincode,
                    "IVRMMC_Id": $scope.ivrmmC_Id,
                    "IVRMMS_Id": $scope.ivrmmS_Id,
                    "ISMSD_Photo": $scope.ismsD_Photo,
                    "ISMSD_Aadhar": $scope.ismsD_Aadhar,
                    "ISMSD_Designation": $scope.ismsD_Designation,
                    "ISMSD_PANNo": $scope.ismsD_PANNo

                };
                apiService.create("SecurityDetails/getdata", data).then(function (promise) {
                    debugger;
                    if (promise.msg === 'Saved') {
                        swal("Data Saved Successfully.....!!!!!");
                        $state.reload();
                    }
                    else if (promise.msg === 'Failed') {
                        swal("Data Not Saved Successfully.....!!!!!");
                        $state.reload();
                    }
                    else if (promise.msg === 'updated') {
                        swal("Data Updated.....!!!!!");
                        $state.reload();
                    }
                    else if (promise.msg === 'failed') {
                        swal("Data Not Updated Successfully.....!!!!!");
                        $state.reload();
                    }
                    else if (promise.duplicate === true) {
                        swal("Data already Exists.....!!!!!");
                    }
                    else {
                        swal("Something is Wrong...");
                    }
                });

            }
            else {
                $scope.submitted = true;
            }

        };
        $scope.search = '';
        $scope.filtervalue1 = function (user) {
            //return (angular.lowercase(user.name)).indexOf(angular.lowercase($scope.search)) >= 0 ||
            //    (angular.lowercase(user.number)).indexOf(angular.lowercase($scope.search)) >= 0 ||
            //    (angular.lowercase(user.email)).indexOf(angular.lowercase($scope.search)) >= 0;
        };
        $scope.getalldetails = function () {

            var data = {
                // pageid = 2
            }
            apiService.create("SecurityDetails/getalldetails", data).then(function (promise) {
                $scope.alldata = promise.alldata;
                $scope.countrylist = promise.countrylist;//country
                //$scope.statelist = promise.statelist;//state

            });
        };

        $scope.edittab1 = function (user) {

            var data = {
                "ISMSD_Id": user.ismsD_Id

            }
            apiService.create("SecurityDetails/edittab1", data).then(function (promise) {

                $scope.statelistdata = promise.statelistdata;
                $scope.ISMSD_Id = promise.editlist[0].ismsD_Id;
                $scope.ismsD_FirstName = promise.editlist[0].ismsD_FirstName;
                $scope.ismsD_MiddleName = promise.editlist[0].ismsD_MiddleName;
                $scope.ismsD_LastName = promise.editlist[0].ismsD_LastName;
                $scope.ismsD_MobileNo = promise.editlist[0].ismsD_MobileNo;
                $scope.ismsD_EmailId = promise.editlist[0].ismsD_EmailId;
                $scope.ivrmmC_Id = promise.editlist[0].ivrmmC_Id;
                $scope.ivrmmS_Id = promise.editlist[0].ivrmmS_Id;
                
                $scope.ismsD_PerStreet = promise.editlist[0].ismsD_PerStreet;
                $scope.ismsD_PerArea = promise.editlist[0].ismsD_PerArea;
                $scope.ismsD_PerCity = promise.editlist[0].ismsD_PerCity;
                $scope.ismsD_PerPincode = promise.editlist[0].ismsD_PerPincode;
                $scope.ismsD_Aadhar = promise.editlist[0].ismsD_Aadhar;
                $scope.ismsD_Designation = promise.editlist[0].ismsD_Designation;
                $scope.ismsD_PANNo = promise.editlist[0].ismsD_PANNo; 
                $scope.ivrmmC_CountryName = promise.editlist[0].countryname;
                $scope.ivrmmS_Name = promise.editlist[0].statename;

                $scope.ismsD_Photo = promise.editlist[0].ismsD_Photo;

                $('#blah').attr('src', promise.editlist[0].ismsD_Photo);


            });
        };
        $scope.deactivYTab1 = function (usersem, SweetAlert) {

            var dystring = "";
            if (usersem.ismsD_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (usersem.ismsD_ActiveFlag === false) {
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
                        apiService.create("SecurityDetails/deactive", usersem).
                            then(function (promise) {
                                if (promise.ret === true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                                else {

                                    swal("Record Not " + dystring + "d " + "Successfully!!!");
                                    $state.reload();
                                }
                            });
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }

                });
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        //==================== Get State List Data
        $scope.get_statelist = function () {
            var data = {
                "IVRMMC_Id": $scope.ivrmmC_Id,
            }
            apiService.create("SecurityDetails/Get_StateData", data).then(function (promise) {
                $scope.statelistdata = promise.statelistdata;
            })
        }

        //upload Employee Profile pic
        $scope.UploadEmployeeProfilePic = [];
        $scope.uploadEmployeeProfilePic = function (input, document) {
            $scope.UploadEmployeeProfilePic = input.files;
            $('#blah').removeAttr('src');
            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blah')
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeprofile(document);
                }
                else if (input.files[0].type !== "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    angular.element("input[type='file']").val(null);
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    angular.element("input[type='file']").val(null);
                    return;
                }
            }
        };

        function UploadEmployeeprofile(data) {

            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadEmployeeProfilePic.length; i++) {
                formData.append("File", $scope.UploadEmployeeProfilePic[i]);
            }

            //We can send more data to server using append         
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/EmployeeDocumentUpload/UploadEmployeeprofilepic", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {
                        $scope.ismsD_Photo = d;
                    } else {
                        swal('File Storage Path Not Found ..!!');
                        angular.element("input[type='file']").val(null);
                    }
                })
                .error(function () {
                    $('#blah').removeAttr('src');
                    defer.reject("File Upload Failed!");
                    angular.element("input[type='file']").val(null);
                });
        }
    }

    
})();