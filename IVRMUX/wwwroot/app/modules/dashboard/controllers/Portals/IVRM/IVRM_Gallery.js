

(function () {
    'use strict';
    angular
        .module('app')
        .controller('IVRM_GalleryController', IVRM_GalleryController);

    IVRM_GalleryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', '$http', '$q', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$interval'];
    function IVRM_GalleryController($rootScope, $scope, $state, $location, dashboardService, Flash, $http, $q, apiService, $stateParams, $filter, superCache, $window, $interval) {


        $scope.coverflg = false;
        var date = new Date();
        $scope.igA_Date = date;
        $scope.igA_Time = date;
        //===================Page Load
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("IVRM_Gallery/getloaddata", pageid).
                then(function (promise) {
                    $scope.classlist = promise.classlist;
                    $scope.clasname = $scope.classlist[0].asmcL_ClassName;
                    $scope.secname = $scope.classlist[0].asmC_SectionName;
                    $scope.roleflg = promise.roleflg;
                    $scope.get_galleryimg = promise.get_galleryimg;
                    $scope.presentCountgrid = $scope.get_galleryimg.length;
                });
        };


        //================================ Section Selection
        $scope.classchange = function () {
            var data = {
                "ASMCL_Id": $scope.asmcL_Id
            };
            apiService.create("IVRM_Gallery/get_section", data).then(function (promise) {
                $scope.sectionlist = promise.sectionlist;
            });
        };

        $scope.stepsModel = [];
        $scope.imageUpload = function (event) {
            $scope.files = event.files;
            for (var i = 0; i < $scope.files.length; i++) {
                var file = $scope.files[i];
                $scope.fileimg = file;
                var reader = new FileReader();
                reader.onload = $scope.imageIsLoaded;
                reader.readAsDataURL(file);
            }
        };
        $scope.imageIsLoaded = function (e) {
            $scope.$apply(function () {
                $scope.stepsModel.push(e.target.result);
            });
        };

        $scope.remove_img = function (reimg) {
            for (var i = 0; i < $scope.files.length; i++) {

                var imgt1 = $scope.files[i];
                if (imgt1.name === reimg.name) {
                    $scope.stepsModel.splice(i, 1);
                }
            }
        };
        function UploadImgs() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.files.length; i++) {
                formData.append("File", $scope.files[i]);
                //  $scope.filenames = $scope.files[i].name;
            }
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_GalleryImgVideos", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    swal(d);
                    $scope.images_paths = d;
                    if ($scope.images_paths.length > 0) {
                        $scope.savedata();
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

        }

        //=============================== Preview Image
        $scope.previewimg = function (img) {
            $scope.imagepreview = img;
            $('#preview').attr('src', $scope.imagepreview);
            $('#myModalPreview').modal('show');
        };

        $scope.checkCover = function (ig) {
            $scope.imgcover = "";
            $scope.imgcover = ig.coverid;
            
        };

        //================================ Set as Cover 
        $scope.covermodel = function (img) {
            var data = {
                "IGA_Id": img
            };
            apiService.create("IVRM_Gallery/getcovermodel", data).then(function (promise) {
                $scope.covermodel = promise.covermodel;
                $('#myModalCover').modal('show');
            });
        };

        //================================== Select Class
        $scope.all_checkCls = function (studt) {
            $scope.usercheckCls = studt;
            var toggleStatus = $scope.usercheckCls;
            angular.forEach($scope.sectionlist, function (cl) {
                cl.clsck = toggleStatus;
            });
        };
        $scope.onselect = function () {
            $scope.usercheckCls = $scope.sectionlist.every(function (options) {
                return options.clsck = toggleStatus;
            });
        };
        $scope.isOptionsRequiredCls = function () {
            return !$scope.sectionlist.some(function (options) {
                return options.clsck;
            });
        };
        //================================== Save  
        $scope.uploaddata = function () {
            if ($scope.myForm.$valid) {
                UploadImgs();
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.images_paths === null || $scope.images_paths === undefined || $scope.images_paths === "") {
                    $scope.images_paths = [];
                }
                //if ($scope.videos_paths == null || $scope.videos_paths == undefined || $scope.videos_paths == "") {
                //    $scope.videos_paths = [];
                //}
                $scope.arraySection = [];
                if ($scope.typeflag === 'CO') {
                    $scope.commonflag = true;
                    $scope.arraySection = [];
                    $scope.ASMCL_Id = $scope.asmcL_Id;
                    $scope.ASMS_Id = $scope.asmS_Id;
                }
                else {
                    $scope.commonflag = false;
                    angular.forEach($scope.sectionlist, function (cls) {
                        if (cls.clsck === true) {
                            $scope.arraySection.push(cls);
                        }
                    });
                }
                var data = {
                    "IGA_Id": $scope.igA_Id,
                    "IGA_GalleryName": $scope.igA_GalleryName,
                    "IGA_Date": $scope.igA_Date,
                    "IGA_CommonGalleryFlg": $scope.commonflag,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "arraySection": $scope.arraySection,
                    "IGA_Time": $filter('date')($scope.igA_Time, "h:mm a"),
                    images_list: $scope.images_paths
                    // videos_list: $scope.videos_paths
                };
                apiService.create("IVRM_Gallery/savedata", data).then(function (promise) {
                    $scope.covermodel(promise.igaId);
                });
                $scope.images_paths = [];
                $scope.videos_paths = [];
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cleardata = function () {
            $state.reload();
        };

        $scope.savecover = function () {
            if ($scope.imgcover === "" || $scope.imgcover === null || $scope.imgcover === 0 || $scope.imgcover === undefined) {
                swal("Select any Image for set Cover image....!!");
            }
            else {
                var data = {
                    "IGAP_Id": $scope.imgcover
                };
            }
            apiService.create("IVRM_Gallery/savecover", data).then(function (promise) {
                if (promise.returnval === true) {
                    swal('Record Saved successfully');
                }
                else if (promise.returnduplicatestatus === 'Duplicate') {
                    swal('Record already exist');
                }
                else {
                    swal('Failed to Upload, please contact administrator');
                }
                angular.element('#myModalCover').model('hide');
                $scope.loaddata();
            });

        };
    }
})();