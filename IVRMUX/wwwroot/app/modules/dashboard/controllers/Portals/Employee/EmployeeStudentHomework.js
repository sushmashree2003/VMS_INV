(function () {
    'use strict';

    angular
        .module('app')
        .controller('EmployeeStudentHomeworkController', HomeworkController);

    HomeworkController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce'];

    function HomeworkController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce) {

        $scope.UploadFile = [];
        $scope.images_temp = [];
        $scope.minDatemf = new Date();
        var id = 1;
        $scope.search = "";
        $scope.itemsPerPage = 10;
        $scope.cls = false;

        //--------for sorting....
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        //==========Load data
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            apiService.getURI("EmployeeStudentHomework/Getdetails", id).
                then(function (promise) {
                    $scope.hrmE_Id = promise.hrmE_Id;
                    $scope.studetiallist = promise.yearlist;
                    $scope.classwork = promise.classwork;
                });
        };

        //-----------upload file/photo.............
        $scope.uploadFile = function (input, document) {
            $scope.UploadFile = input.files;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.ms-excel" && input.files[0].size <= 2097152) {

                    var reader = new FileReader();

                    reader.onload = function (e) {

                        $('#blahD')
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
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
            $http.post("/api/ImageUpload/Upload_Noticefiles", formData,
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



        //========Remove Selected File & Image!
        $scope.remove_file = function () {

            $scope.file_detail = "";
            $scope.notice = "";
        };



        //=============For Save Data!

        $scope.selectedImages = [];
        $scope.submitted = false;
        $scope.savedata = function () {

            var todaysdate = Date.now();
            var date = todaysdate === null ? "" : $filter('date')(todaysdate, "yyyy-MM-dd");
            var hwdate = $scope.ihW_Date === null ? "" : $filter('date')($scope.ihW_Date, "yyyy-MM-dd");
            if (hwdate !== date) {
                swal("Please Select Today's Date");
            }
            else {
                if ($scope.myForm1.$valid) {

                    var data = {};
                    //-------file upload
                    if ($scope.notice === undefined || $scope.notice === "") {
                        data = {
                            "IHW_Id": $scope.ihW_Id,
                            "ASMAY_Id": $scope.asmaY_Id,
                            "ASMCL_Id": $scope.asmcL_Id,
                            "ASMS_Id": $scope.asmS_Id,
                            "ISMS_Id": $scope.ismS_Id,
                            "IHW_Topic": $scope.ihW_Topic,
                            "IHW_Assignment": $scope.ihW_Assignment,
                            "IHW_AssignmentNo": $scope.ihW_AssignmentNo,
                            "IHW_Date": hwdate
                            //"IHW_Attachment": $scope.file_detail,
                            //"IHW_FilePath": att_file11
                        };
                    }
                    else {
                        $scope.filedoc = [];
                        $scope.filedoc = [$scope.notice];
                        var att_file = "";

                        if ($scope.filedoc.length > 0) {
                            for (var i = 0; i < $scope.filedoc.length; i++) {
                                att_file = $scope.filedoc[0];
                            }
                        }
                        var att_file11 = att_file.toString();
                        data = {
                            "IHW_Id": $scope.ihW_Id,
                            "ASMAY_Id": $scope.asmaY_Id,
                            "ASMCL_Id": $scope.asmcL_Id,
                            "ASMS_Id": $scope.asmS_Id,
                            "ISMS_Id": $scope.ismS_Id,
                            "IHW_Topic": $scope.ihW_Topic,
                            "IHW_Assignment": $scope.ihW_Assignment,
                            "IHW_AssignmentNo": $scope.ihW_AssignmentNo,
                            "IHW_Date": hwdate,
                            "IHW_Attachment": $scope.file_detail,
                            "IHW_FilePath": att_file11
                        };
                    }

                    apiService.create("EmployeeStudentHomework/savedetail", data).
                        then(function (promise) {

                            if (promise.returnval !== null && promise.dulicate !== null) {
                                if (promise.dulicate === false) {
                                    if (promise.returnval === true) {
                                        if ($scope.ihW_Id > 0) {
                                            swal('Record Update Successfully!');
                                        }
                                        else {
                                            swal('Record Saved Successfully!!!');
                                        }
                                    }
                                    else {
                                        if (promise.returnval === false) {
                                            if ($scope.ihW_Id > 0) {
                                                swal('Record Not Update Successfully!');
                                            }
                                            else {
                                                swal('Record Not Saved Successfully!!!');
                                            }
                                        }
                                    }
                                }
                                else {
                                    swal("Record already Exist!");
                                }
                                $state.reload();
                            }
                            else {
                                swal("Kindly Contact Administrator!!!");
                            }
                        });
                }
                else {
                    $scope.submitted = true;
                }
            }

        };


        //======Edit Record =====//
        $scope.editData = function (option) {

            var data = {
                "IHW_Id": option.ihW_Id,
                "ASMAY_Id": option.asmaY_Id
            };
            apiService.create("EmployeeStudentHomework/editData", data)
                .then(function (promise) {

                    $scope.editlist = promise.editlist;
                    if (promise.editlist.length > 0) {

                        $scope.ihW_Id = promise.editlist[0].ihW_Id;

                        $scope.asmaY_Id = promise.editlist[0].asmaY_Id;
                        $scope.onyearchange($scope.asmaY_Id);

                        $scope.asmcL_Id = promise.editlist[0].asmcL_Id;
                        $scope.onclasschange($scope.asmcL_Id);

                        $scope.asmS_Id = promise.editlist[0].asmS_Id;
                        $scope.sectionchange($scope.asmS_Id);

                        $scope.ismS_Id = promise.editlist[0].ismS_Id;

                        $scope.ihW_Date = new Date(promise.editlist[0].ihW_Date);
                        $scope.file_detail = promise.editlist[0].ihW_Attachment;
                        $scope.ihW_Topic = promise.editlist[0].ihW_Topic;
                        $scope.ihW_Assignment = promise.editlist[0].ihW_Assignment;
                        $scope.ihW_AssignmentNo = promise.editlist[0].ihW_AssignmentNo;
                        $scope.ihW_FilePath = promise.editlist[0].ihW_FilePath;

                        $scope.notice = promise.editlist[0].ihW_FilePath;
                    }
                    else {
                        swal('Data Not Available For Edit!');
                    }
                });
        };
        //===========End Edit Record =======//



        $scope.interacted2 = function (field) {

            return $scope.submitted;
        };


        //==========selected year!
        $scope.onyearchange = function (ASMAY_Id) {
            $scope.sectionlist = [];
            $scope.asmcL_Id = "";
            $scope.sectionlist = [];
            $scope.asmS_Id = "";
            var data = {
                "HRME_Id": $scope.hrmE_Id,
                "ASMAY_Id": $scope.asmaY_Id
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("EmployeeStudentHomework/get_classes", data).then(function (promise) {
                if (promise.classlist.length > 0) {
                    if (promise.roleflg === 'COORDINATOR' || promise.roleflg === 'Coordinator' || promise.roleflg === 'coordinator') {
                        $scope.cls = true;
                        $scope.classlist = promise.classlist;
                    }
                    else {
                        $scope.cls = false;
                        $scope.classlist = promise.classlist;
                    }
                }
                else {
                    swal('Data Not Available');
                    $scope.asmcL_Id = "";
                }
            });
        };

        $scope.all_checkCls = function (allcls) {
            $scope.usercheckCls = allcls;
            var toggleStatus = $scope.usercheckCls;
            angular.forEach($scope.classlist, function (cls) {
                cls.clsck = toggleStatus;
            });
            $scope.onclasschange();
        };

        //==========selected class!
        $scope.onclasschange = function () {
            $scope.usercheckCls = $scope.classlist.every(function (options) {
                return options.clsck;
            });
            $scope.sectionlist = [];
            $scope.asmS_Id = "";
            $scope.subjectlist = [];
            $scope.ismS_Id = "";

            $scope.clsarray = [];
            angular.forEach($scope.classlist, function (cl) {
                if (cl.clsck === true) {
                    $scope.clsarray.push({ ASMCL_Id: cl.ASMCL_Id, ASMCL_ClassName: cl.ASMCL_ClassName });
                }
            });
            var data = {
                "HRME_Id": $scope.hrmE_Id,
                "clsarray": $scope.clsarray,
                "ASMAY_Id": $scope.asmaY_Id
            };
            apiService.create("EmployeeStudentHomework/getsectiondata", data).
                then(function (promise) {
                    if (promise.sectionlist.length > 0) {
                        $scope.sectionlist = promise.sectionlist;
                    }
                    else {
                        swal('Data Not Available!');
                        $scope.asmS_Id = "";
                    }
                });
        };



        //==========selected setion.!
        $scope.sectionchange = function () {
            $scope.usercheckSec = $scope.sectionlist.every(function (options) {
                return options.secck;
            });
            $scope.subjectlist = [];
            $scope.ismS_Id = "";
            $scope.secarray = [];
            $scope.clsarray = [];
            angular.forEach($scope.classlist, function (cl) {
                if (cl.clsck === true) {
                    $scope.clsarray.push({ ASMCL_Id: cl.ASMCL_Id, ASMCL_ClassName: cl.ASMCL_ClassName });
                }
            });
            angular.forEach($scope.sectionlist, function (se) {
                if (se.secck === true) {
                    $scope.secarray.push({ asmS_Id: se.asmS_Id, asmC_SectionName: se.asmC_SectionName });
                }
            });
            var data = {
                "HRME_Id": $scope.hrmE_Id,
                "secarray": $scope.secarray,
                "clsarray": $scope.clsarray,
                "ASMAY_Id": $scope.asmaY_Id
            };
            apiService.create("EmployeeStudentHomework/getsubject", data).
                then(function (promise) {

                    if (promise.subjectlist.length > 0) {
                        $scope.subjectlist = promise.subjectlist;
                    }
                    else {
                        swal('Data Not Available!');
                        $scope.ismS_Id = "";
                    }

                });
        };


        //=====for activation & deactivation
        $scope.Deactivate = function (data, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            if (data.ihW_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("EmployeeStudentHomework/deactivate", data).
                            then(function (promise) {

                                if (promise.returnval === true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $state.reload();
                            });

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };


        $scope.clear_data = function () {
            $state.reload();
        };

    }
})();
