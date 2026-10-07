(function () {
    'use strict';

    angular
        .module('app')
        .controller('IVRM_ClassWorkController', IVRM_ClassWorkController);

    IVRM_ClassWorkController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce']

    function IVRM_ClassWorkController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce) {

        $scope.UploadFile = [];
        $scope.search = "";
        $scope.itemsPerPage = 10;
        var id = 1;

        //--------for sorting....
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            apiService.getURI("IVRM_ClassWork/Getdetails", id).
                then(function (promise) {
                    $scope.studetiallist = promise.yearlist;
                    $scope.hrmE_Id = promise.hrmE_Id;

                    $scope.classworklst = promise.classwork;
                })
        }


        //-----------upload file/photo.............
        $scope.uploadFile = function (input, document) {

            $scope.UploadFile = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" || input.files[0].type == "image/png" || input.files[0].type == "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.ms-excel" &&
                    input.files[0].size <= 2097152) {

                    var reader = new FileReader();

                    reader.onload = function (e) {

                        $('#blahD')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        }
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



        ///=========clear upload field data......
        $scope.remove_file = function () {

            $scope.file_detail = "";
            $scope.notice = "";
        }



        $scope.submitted = false;
        $scope.savedata = function () {

            if ($scope.myForm1.$valid) {

                //-------file upload
                var att_file = "";
                $scope.filedoc = [];
                $scope.filedoc = [$scope.notice];

                if ($scope.filedoc.length > 0) {
                    for (var i = 0; i < $scope.filedoc.length; i++) {
                        att_file = $scope.filedoc[0];
                    }
                }
                var att_file11 = "";
                if (att_file != undefined) {
                     att_file11 = att_file.toString();
                }

                var data = {
                    "ICW_Id": $scope.icW_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "ISMS_Id": $scope.ismS_Id,
                    "ICW_Topic": $scope.work_topic,
                    "ICW_SubTopic": $scope.work_subtopic,
                    "ICW_Content": $scope.work_content,
                    "ICW_TeachingAid": $scope.icW_TeachingAid,
                    "ICW_Assignment": $scope.icW_Assignment,
                    "ICW_Evaluation": $scope.icW_Evaluation,
                    "ICW_FromDate": new Date($scope.notice_EStartDate).toDateString(),
                    "ICW_ToDate": new Date($scope.notice_EndDate).toDateString(),
                    "ICW_Attachment": $scope.file_detail,
                    "ICW_FilePath": att_file11,
                }

                apiService.create("IVRM_ClassWork/savedetail", data).
                    then(function (promise) {
                        if (promise.returnval != null && promise.dulicate != null) {
                            if (promise.dulicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.icW_Id > 0) {
                                        swal('Record Update Successfully!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }
                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.icW_Id > 0) {
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

        };
        $scope.clear1 = function () {
            $state.reload();
        };



        $scope.interacted2 = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.onyearchange = function (ASMAY_Id) {
            $scope.sectionlist = [];
            $scope.asmcL_Id = "";
            $scope.sectionlist = [];
            $scope.asmS_Id = "";

            var data = {
                "HRME_Id": $scope.hrmE_Id,
                "ASMAY_Id": $scope.asmaY_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("IVRM_ClassWork/get_classes", data).then(function (promise) {
                if (promise.classlist.length > 0) {
                    $scope.classlist = promise.classlist;
                }
                else {

                    $scope.asmcL_Id = "";
                    swal('Data Not Available..!');

                }
            })

        }

        $scope.onclasschange = function (asmcL_Id) {
            $scope.sectionlist = [];
            $scope.asmS_Id = "";
            $scope.subjectlist = [];
            $scope.ismS_Id = "";

            var data = {
                "HRME_Id": $scope.hrmE_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
            }
            apiService.create("IVRM_ClassWork/getsectiondata", data).
                then(function (promise) {

                    if (promise.sectionlist.length > 0) {
                        $scope.sectionlist = promise.sectionlist;
                    }
                    else {
                        swal('Records are not available..!');
                        $scope.asmS_Id = "";
                    }
                })
        }
        $scope.sectionchange = function () {
            $scope.subjectlist = [];
            $scope.ismS_Id = "";


            var data = {
                "HRME_Id": $scope.hrmE_Id,
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
            }
            apiService.create("IVRM_ClassWork/getsubject", data).
                then(function (promise) {

                    if (promise.subjectlist.length > 0) {
                        $scope.subjectlist = promise.subjectlist;
                    }
                    else {
                        swal('Subjects are not available for This Section..!');
                        $scope.ismS_Id = "";
                    }

                })
        }


        //======Edit Record =====//
        $scope.editData = function (option) {

            var data = {
                "ICW_Id": option.icW_Id,
                "ASMAY_Id": option.asmaY_Id,
            }
            apiService.create("IVRM_ClassWork/editData", data)
                .then(function (promise) {

                    $scope.editlist = promise.editlist;
                    if (promise.editlist.length > 0) {

                        $scope.icW_Id = promise.editlist[0].icW_Id;

                        $scope.asmaY_Id = promise.editlist[0].asmaY_Id;
                        $scope.onyearchange($scope.asmaY_Id);

                        $scope.asmcL_Id = promise.editlist[0].asmcL_Id;
                        $scope.onclasschange($scope.asmcL_Id);

                        $scope.asmS_Id = promise.editlist[0].asmS_Id;
                        $scope.sectionchange($scope.asmS_Id);

                        $scope.ismS_Id = promise.editlist[0].ismS_Id;

                        $scope.notice_EStartDate = new Date(promise.editlist[0].icW_FromDate);

                        $scope.notice_EndDate = new Date(promise.editlist[0].icW_ToDate);

                        $scope.file_detail = promise.editlist[0].icW_Attachment;
                        $scope.work_topic = promise.editlist[0].icW_Topic;
                        $scope.work_subtopic = promise.editlist[0].icW_SubTopic;
                        $scope.icW_Assignment = promise.editlist[0].icW_Assignment;
                        $scope.icW_Evaluation = promise.editlist[0].icW_Evaluation;
                        $scope.icW_TeachingAid = promise.editlist[0].icW_TeachingAid;
                        $scope.work_content = promise.editlist[0].icW_Content;
                        $scope.icW_FilePath = promise.editlist[0].icW_FilePath;

                        $scope.notice = promise.editlist[0].icW_FilePath;



                    }

                })

        }
        //===========End Edit Record =======//


        //===============activate/deactivate
        $scope.Deactivate = function (employee, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.icW_ActiveFlag === true) {
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

                        apiService.create("IVRM_ClassWork/deactivate", employee).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $state.reload();
                            })

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }

    }
})();
