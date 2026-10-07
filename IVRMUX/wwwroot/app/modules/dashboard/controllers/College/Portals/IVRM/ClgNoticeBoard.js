(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgNoticeBoardController', ClgNoticeBoardController)

    ClgNoticeBoardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$q', '$http', '$filter', 'superCache', '$window']
    function ClgNoticeBoardController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $q, $http, $filter, superCache, $window) {

        //====================================== Upload

        //-----------upload file/photo.............
        $scope.uploadFile = function (input, document) {

            $scope.UploadFile = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" || input.files[0].type == "image/png" || input.files[0].type == "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/doc" || input.files[0].type === "application/docx" || input.files[0].type === "application/vnd.ms-excel" && input.files[0].size <= 2097152) {
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

        ///=========clear upload field data......
        $scope.remove_file = function () {
            $scope.file_detail = "";
            $scope.notice = "";
        }
        //====================LOAD Data
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            apiService.getDATA("ClgNoticeBoard/getloaddata").
                then(function (promise) {
                    $scope.course_list = promise.course_list;
                    $scope.noticelist = promise.noticelist;
                    $scope.presentCountgrid = $scope.noticelist.length;

                })
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        }

        //=========================Course all Check
        //$scope.toggleAll = function () {
        //    angular.forEach($scope.course_list, function (cors) {
        //        cors.course = $scope.all;
        //    })
        //    $scope.oncoursechange();
        //};

        //=========================Course Selection Change
        $scope.oncoursechange = function () {
            var data = {
                "AMCO_Id": $scope.amcO_Id,
            }
            apiService.create("ClgNoticeBoard/getbranchdata", data).
                then(function (promise) {
                    $scope.branch_list = promise.branch_list;
                })
        }

        //=========================Branch All Check
        $scope.toggleAllB = function () {
            angular.forEach($scope.branch_list, function (brch) {
                brch.branch = $scope.allB;
            })
            $scope.onbranchchange();
        };

        $scope.isOptionsRequiredb = function () {
            return !$scope.branch_list.some(function (options) {
                return options.branch;
            });
        }
        //=========================Branch Selection Change
        $scope.onbranchchange = function () {
            $scope.brancharray = [];
            $scope.sem_list = "";
            $scope.allB = $scope.branch_list.every(function (brh) { return brh.branch; });
            angular.forEach($scope.branch_list, function (brch) {
                if (brch.branch == true) {
                    $scope.brancharray.push(brch);
                }
            })
            var data = {
                "branchArray": $scope.brancharray,
            }
            apiService.create("ClgNoticeBoard/getsemdata", data).
                then(function (promise) {
                    $scope.sem_list = promise.sem_list;
                })
        }

        //=========================Semester All Check
        $scope.toggleAllS = function () {
            angular.forEach($scope.sem_list, function (sm) {
                sm.sem = $scope.allS;
            })
        };

        $scope.isOptionsRequiredS = function () {
            return !$scope.sem_list.some(function (options) {
                return options.sem;
            });
        }

        //========================= Save Notice
        $scope.savedata = function () {
            $scope.courseArray = [];
            $scope.branchArray = [];
            $scope.semesterArray = [];

            if ($scope.myForm.$valid) {
                //======================== Date Time Format
                if ($scope.intB_DispalyDisableFlg == false) {
                    var displaydate = null;
                }
                else if ($scope.intB_DispalyDisableFlg == true) {
                    var displaydate = $scope.intB_DisplayDate == null ? "" : $filter('date')($scope.intB_DisplayDate, "yyyy-MM-dd");
                }

                var startdate = $scope.intB_StartDate == null ? "" : $filter('date')($scope.intB_StartDate, "yyyy-MM-dd");
                var enddate = $scope.intB_EndDate == null ? "" : $filter('date')($scope.intB_EndDate, "yyyy-MM-dd");

                //===========Branch / Semester Selected values              
                angular.forEach($scope.branch_list, function (brh) {
                    if (brh.branch == true) {
                        $scope.branchArray.push(brh);
                    }
                });
                angular.forEach($scope.sem_list, function (sm) {
                    if (sm.sem == true) {
                        $scope.semesterArray.push(sm);
                    }
                });


                if ($scope.notice == undefined || $scope.notice == "") {
                    var data = {
                        "INTB_Id": $scope.intB_Id,
                        "INTB_Title": $scope.intB_Title,
                        "INTB_Description": $scope.intB_Description,
                        "INTB_StartDate": startdate,
                        "INTB_EndDate": enddate,
                        "INTB_DisplayDate": displaydate,
                        "NTB_TTSylabusFlg": $scope.ntB_TTSylabusFlg,
                        "INTB_DispalyDisableFlg": $scope.intB_DispalyDisableFlg,
                        "AMCO_Id": $scope.amcO_Id,
                        "branchArray": $scope.branchArray,
                        "semesterArray": $scope.semesterArray,
                    }
                }
                else {
                    var att_file = "";
                    $scope.filedoc = [];
                    $scope.filedoc = [$scope.notice];
                    if ($scope.filedoc.length > 0) {
                        for (var i = 0; i < $scope.filedoc.length; i++) {
                            att_file = $scope.filedoc[0];
                        }
                    }
                    var att_file11 = att_file.toString();
                    var data = {
                        "INTB_Id": $scope.intB_Id,
                        "INTB_Title": $scope.intB_Title,
                        "INTB_Description": $scope.intB_Description,
                        "INTB_StartDate": startdate,
                        "INTB_EndDate": enddate,
                        "INTB_DisplayDate": displaydate,
                        "NTB_TTSylabusFlg": $scope.ntB_TTSylabusFlg,
                        "INTB_DispalyDisableFlg": $scope.intB_DispalyDisableFlg,
                        "AMCO_Id": $scope.amcO_Id,
                        "branchArray": $scope.branchArray,
                        "semesterArray": $scope.semesterArray,
                        "INTB_Attachment": $scope.file_detail,
                        "INTB_FilePath": att_file11,

                    }
                }
                apiService.create("ClgNoticeBoard/savedata", data).
                    then(function (promise) {

                        if (promise.returnval != null && promise.already_cnt != null) {
                            if (promise.already_cnt == false) {
                                if (promise.returnval == true) {
                                    if ($scope.intB_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }
                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.intB_Id > 0) {
                                            swal('Record Not Update Successfully!!!');
                                        }
                                        else {
                                            swal('Record Not Saved Successfully!!!');
                                        }
                                    }
                                }
                            }
                            else {
                                swal("Record already exist");
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

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.noticemodel = function (id) {
            var data = {
                "INTB_Id": id,
            }
            apiService.create("ClgNoticeBoard/getNoticedata", data).
                then(function (promise) {
                    $scope.noticedetails = promise.noticedetails;
                    $scope.notice = $scope.noticedetails[0].intB_Title;
                })
        }

        $scope.deactive = function (item, SweetAlert) {
            $scope.INTB_Id = item.intB_Id;
            var dystring = "";
            if (item.intB_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.intB_ActiveFlag == false) {
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
                        apiService.create("ClgNoticeBoard/deactive", item).
                            then(function (promise) {
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
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.deactivedetails = function (item, SweetAlert) {
            $scope.INTBCB_Id = item.intbcB_Id;
            var dystring = "";
            if (item.intbcB_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.intbcB_ActiveFlag == false) {
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
                        apiService.create("ClgNoticeBoard/deactivedetails", item).
                            then(function (promise) {
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
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.edit = function (id) {
            var data = {
                "INTB_Id": id
            }
            apiService.create("ClgNoticeBoard/editdetails", data).then(function (promise) {

                $scope.editdetails = [];
                if (promise.editdetails.length > 0) {
                    $scope.intB_Id = promise.editdetails[0].intB_Id;
                    $scope.intB_Title = promise.editdetails[0].intB_Title;
                    $scope.intB_Description = promise.editdetails[0].intB_Description;
                    $scope.file_detail = promise.editdetails[0].intB_Attachment;
                    $scope.intB_StartDate = new Date(promise.editdetails[0].intB_StartDate);
                    $scope.intB_EndDate = new Date(promise.editdetails[0].intB_EndDate);
                    $scope.intB_FilePath = promise.editdetails[0].intB_FilePath
                    $scope.notice = promise.editdetails[0].intB_FilePath
                    $scope.ntB_TTSylabusFlg = promise.editdetails[0].ntB_TTSylabusFlg;
                    $scope.intB_DispalyDisableFlg = promise.editdetails[0].intB_DispalyDisableFlg;
                    $scope.amcO_Id = promise.editdetails[0].amcO_Id;

                    if (promise.editdetails[0].intB_DispalyDisableFlg == false) {
                        $scope.intB_DisplayDate = ""
                    }
                    else {
                        $scope.intB_DispalyDisableFlg = true;
                        $scope.intB_DisplayDate = new Date(promise.editdetails[0].intB_DisplayDate);
                    }
                }

                $scope.editdetails = promise.editdetails;
                $scope.coursebranchsem = promise.coursebranchsem
                angular.forEach($scope.coursebranchsem, function (b) {
                    b.branch = false;
                    angular.forEach($scope.editdetails, function (ed) {
                        if (b.amB_Id == ed.amB_Id) {
                            b.branch = true;
                        }
                    })
                })
                $scope.allB = $scope.coursebranchsem.every(function (role) {
                    return role.branch;
                });

                angular.forEach($scope.coursebranchsem, function (s) {
                    b.branch = false;
                    angular.forEach($scope.editdetails, function (ed) {
                        if (s.amsE_Id == ed.amsE_Id) {
                            s.sem = true;
                        }
                    })
                })
                $scope.allS = $scope.coursebranchsem.every(function (role) {
                    return role.sem;
                });
            })

        }


    };
})();

