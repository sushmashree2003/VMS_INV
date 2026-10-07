(function () {
    'use strict';
    angular
        .module('app')
        .controller('NoticeBoardController', NoticeBoardController);
    NoticeBoardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce']
    function NoticeBoardController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce) {

        $scope.UploadFile = [];
        // $scope.file_temp = [];
        $scope.search = "";
        $scope.intB_DispalyDisableFlg = false;
        //$scope.minDate = Date.now();

        //------------for sorting.........
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }



        //----------load data into page.............
        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            var id = 1;
            apiService.getURI("NoticeBoard/Getdetails", id).
                then(function (promise) {

                    $scope.notice_details = promise.notice_details;

                    if (promise.classlist.length > 0) {

                        $scope.classlist = promise.classlist;

                    }

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

        //-------check date
        $scope.display_Date = function () {
            if ($scope.intB_DisplayDate > $scope.intB_EndDate) {
                swal("Display Date Cannot Greater than End date ")
                $scope.intB_DisplayDate = "";
            }
            else if ($scope.intB_StartDate > $scope.intB_EndDate) {
                swal("Start Date Cannot Greater than End date ")
                $scope.intB_EndDate = "";
            }
        }


        //-------------search date
        $scope.filterValue = function (obj) {
            return
            angular.lowercase(obj.intB_Title).indexOf(angular.lowercase($scope.search)) >= 0 ||
                angular.lowercase(obj.intB_Description).indexOf(angular.lowercase($scope.search)) >= 0 ||
                angular.lowercase(obj.intB_Attachment).indexOf(angular.lowercase($scope.search)) >= 0 ||
                ($filter('date')(obj.intB_StartDate, 'dd/MM/yyyy').indexOf($scope.search) > 0) ||
                ($filter('date')(obj.intB_EndDate, 'dd/MM/yyyy').indexOf($scope.search) > 0) ||
                ($filter('date')(obj.intB_DisplayDate, 'dd/MM/yyyy').indexOf($scope.search) > 0)
        }
        $scope.filterValue = function (obj) {
            return $filter('date')(obj.OrderDate, 'MM/dd/yyyy') == $filter('date')($scope.searchValue, 'MM/dd/yyyy')
        }



        //========classlist CheckBox Field Validation===========//
        $scope.isOptionsRequired = function () {
            return !$scope.classlist.some(function (item) {
                return item.selected;
            });
        }


        //=======selection of checkbox....
        $scope.togchkbxC = function () {

            $scope.usercheckC = $scope.classlist.every(function (role) {
                return role.selected;
            });
        }


        //---------all checkbox Select...
        $scope.all_checkC = function (all) {

            $scope.usercheckC = all;
            var toggleStatus = $scope.usercheckC;
            angular.forEach($scope.classlist, function (role) {
                role.selected = toggleStatus;
            });
        }



        //------------save data.....
        $scope.classlstdata = [];
        $scope.savedata = function () {
            $scope.classlstdata = [];

            if ($scope.myForm.$valid) {
                if ($scope.intB_DispalyDisableFlg == false) {
                    var displaydate = null;
                }
                else if ($scope.intB_DispalyDisableFlg == true) {
                    var displaydate = $scope.intB_DisplayDate == null ? "" : $filter('date')($scope.intB_DisplayDate, "yyyy-MM-dd");
                }

                var startdate = $scope.intB_StartDate == null ? "" : $filter('date')($scope.intB_StartDate, "yyyy-MM-dd");
                var enddate = $scope.intB_EndDate == null ? "" : $filter('date')($scope.intB_EndDate, "yyyy-MM-dd");

                $scope.classlstdata = [];
                angular.forEach($scope.classlist, function (cls) {
                    if (cls.selected == true) {
                        $scope.classlstdata.push(cls);
                    }
                });

                //-------file upload

                if ($scope.notice == undefined || $scope.notice == "") {
                    var data = {
                        "INTB_Id": $scope.intB_Id,
                        "INTB_Title": $scope.intB_Title,
                        "INTB_Description": $scope.intB_Description,
                        "INTB_StartDate": startdate,
                        "INTB_EndDate": enddate,
                        "INTB_DisplayDate": displaydate,
                        //"INTB_Attachment": $scope.file_detail,
                        // "INTB_FilePath": att_file11,
                        "NTB_TTSylabusFlg": $scope.ntB_TTSylabusFlg,
                        "INTB_DispalyDisableFlg": $scope.intB_DispalyDisableFlg,
                        "classlst": $scope.classlstdata,

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
                        "INTB_Attachment": $scope.file_detail,
                        "INTB_FilePath": att_file11,
                        "NTB_TTSylabusFlg": $scope.ntB_TTSylabusFlg,
                        "INTB_DispalyDisableFlg": $scope.intB_DispalyDisableFlg,
                        "classlst": $scope.classlstdata,

                    }
                }



                apiService.create("NoticeBoard/savedetail", data).
                    then(function (promise) {

                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
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

        $scope.interacted = function (field) {
            return $scope.submitted;
        };



        //----------------edit data.......
        $scope.editnotice = function (id) {

            var data = {
                "INTB_Id": id.intB_Id
            }

            apiService.create("NoticeBoard/editdetails", data).then(function (promise) {

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

                    if (promise.editdetails[0].intB_DispalyDisableFlg == false) {

                        //$scope.intB_DisplayDate = ""
                    }
                    else {
                        $scope.intB_DispalyDisableFlg = true;
                        $scope.intB_DisplayDate = new Date(promise.editdetails[0].intB_DisplayDate);
                    }
                }

                $scope.editdetails = promise.editdetails;
                angular.forEach($scope.classlist, function (tt) {
                    tt.selected = false;
                    angular.forEach($scope.editdetails, function (xx) {

                        if (tt.asmcL_Id == xx.asmcL_Id) {

                            tt.selected = true;
                        }
                    })
                })
                $scope.usercheckC = $scope.classlist.every(function (role) {
                    return role.selected;
                });
            })

        }

        //-----------------------------------


        //$scope.editnotice = function (item) {
        //   
        //    $scope.intb_id = item.intb_id;
        //    $scope.intb_title = item.intb_title;
        //    $scope.intb_description = item.intb_description;
        //    $scope.file_detail = item.intb_attachment;
        //    $scope.intb_filepath = item.intb_filepath;
        //    $scope.notice = item.intb_filepath;
        //    $scope.intb_startdate = new date(item.intb_startdate);
        //    $scope.intb_enddate = new date(item.intb_enddate);
        //    $scope.intb_displaydate = new date(item.intb_displaydate);
        //}


        //-------------for active and deactive
        $scope.deactiveY = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.intB_ActiveFlag === true) {
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

                        apiService.create("NoticeBoard/deactivate", employee).
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

        $scope.clear = function () {
            $state.reload();
        };
    }
})();
