
(function () {
    'use strict';
    angular
        .module('app')
        .controller('RoomWiseAmenitiesMappingController', RoomWiseAmenitiesMappingController);
    RoomWiseAmenitiesMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce']
    function RoomWiseAmenitiesMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce) {


        $scope.editflag = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0
        }
        $scope.Tenantflg = 'Tenant';


        //-------------------------------------------------------------------

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;


            $scope.all_checkC = function () {
                var checkStatus = $scope.hrmaM_Id;
                var count = 0;
                angular.forEach($scope.get_org, function (itm) {
                    itm.selected = checkStatus;
                    if (itm.selected == true) {
                        count += 1;
                    }
                    else {
                        count = 0;
                    }
                });
            }

            $scope.isOptionsRequired = function () {
                return !$scope.get_org.some(function (options) {
                    return options.selected;
                });
            }

            $scope.togchkbxC = function () {

                $scope.hrmaM_Id = $scope.get_org.every(function (options) {
                    return options.selected;
                });
            }


            apiService.getURI("RoomWiseAmenitiesMapping/getloaddata", pageid).
                then(function (promise) {
                    $scope.rooms = promise.rooms;
                    $scope.get_org = promise.get_org;
                    $scope.get_levl = promise.get_levl;
                    $scope.presentCountgrid = $scope.get_levl.length;
                })
        };





        //---------------------------------Save--------------------------------------------

        $scope.savedata = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {

                var HRMAMID = [];
                angular.forEach($scope.get_org, function (ty) {
                    if (ty.selected) {
                        HRMAMID.push(ty.hrmaM_Id);
                    }
                })

                var data = {

                    "HRMRAM_Id": $scope.HRMRAM_Id,
                    "HRMR_Id": $scope.HRMR_Id,
                    "HRMRAM_Array": HRMAMID,
                    "HRMRAM_RentPerDay": $scope.perday,
                    "HRMRAM_NoOfHrs": $scope.hrs,
                    "HRMRAM_RentPerHour": $scope.perhrs,
                    "HRMRAM_filepath": $scope.HRMRAM_filepath

                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };

                apiService.create("RoomWiseAmenitiesMapping/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.hrmraM_Id == 0 || promise.hrmraM_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.hrmraM_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.hrmraM_Id == 0 || promise.v < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.hrmraM_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        };




        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };





        $scope.selectFileforUploadzdBOSBOE = function (input) {
            $scope.SelectedFileForUploadzdBOSBOE = input.files;
            //$('#blah').removeAttr('src');
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                $scope.extention = nameArray[nameArray.length - 1];
                if (($scope.extention === "JPEG" || $scope.extention === "jpeg" || $scope.extention === "jpg" || $scope.extention === "JPG" || $scope.extention === "png" || $scope.extention === "pdf" || $scope.extention === "jfif" || $scope.extention === "JFIF") && input.files[0].size <= 5242880)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blah')
                            .attr('src', e.target.result);

                        $('#documentid') //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentBOSBOE();
                }
                else if ($scope.extention !== "JPEG" && $scope.extention !== "png" && $scope.extention !== "jpg" && $scope.extention !== "JPG" && $scope.extention !== "pdf") {
                    $('#documentid').removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 5242880) {
                    $('#documentid').removeAttr('src');
                    swal("Document size should be less than 5MB");
                    return;
                }
            }
        };

        function UploadEmployeeDocumentBOSBOE() {

            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadzdBOSBOE.length; i++) {
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
                        $scope.HRMRAM_filepath = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#documentid').removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }
        $scope.deactive = function (item, SweetAlert) {
            $scope.HRMRAM_Id = item.hrmraM_Id;
            var dystring = "";
            if (item.HRMRAM_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.HRMRAM_ActiveFlag == false) {
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
                        apiService.create("RoomWiseAmenitiesMapping/deactive", item).
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
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        }

        //$scope.edit = function (item) {
        //    $('#documentid').removeAttr('src');

        //    $scope.HRMRAM_Id = item.HRMRAM_Id;
        //    $scope.HRMR_Id = item.HRMR_Id;
        //    $scope.hrmaM_Id = item.HRMAM_Id;
        //    $scope.perday = item.HRMRAM_RentPerDay;
        //    $scope.hrs = item.HRMRAM_NoOfHrs;
        //    $scope.perhrs = item.HRMRAM_RentPerHour;
        //    $scope.HRMRAM_filepath = item.HRMRAM_filepath;

        //    if (item.HRMRAM_filepath != null && item.HRMRAM_filepath != "") {
        //        $('#documentid').attr('src', item.HRMRAM_filepath);
        //    }

        //    $scope.editflag = true;

        //};

        $scope.Edit = function (item, get_org) {
            var data = {
                "HRMRAM_Id": item.HRMRAM_Id,
            };
            apiService.create("RoomWiseAmenitiesMapping/Edit", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getloaddetails = promise.getloaddetails;
                    $scope.editflag = true;

                    for (var i = 0; i < get_org.length; i++) {

                        name = get_org[i].hrmaM_Id;
                        if (name == promise.getloaddetails[0].hrmaM_Id) {
                            get_org[i].selected = true

                        }
                        else {
                            get_org[i].selected = false;

                        }

                    }

                    $scope.HRMRAM_Id = promise.getloaddetails[0].hrmraM_Id;
                    $scope.HRMR_Id = promise.getloaddetails[0].hrmR_Id;
                    $scope.perday = promise.getloaddetails[0].hrmraM_RentPerDay;
                    $scope.hrs = promise.getloaddetails[0].hrmraM_NoOfHrs;
                    $scope.perhrs = promise.getloaddetails[0].hrmraM_RentPerHour;
                    $scope.HRMRAM_filepath = promise.getloaddetails[0].hrmraM_filepath;

                    $scope.showmodaldetailsBOSBOE()
                    {
                        $('#myModal').modal('hide');
                    }
                }
            });
        }

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

        $scope.showmodaldetailsBOSBOE = function () {
            //$('#preview').removeAttr('src');
            var filename = $scope.HRMRAM_filepath.toString();
            var nameArray = filename.split('.');
            $scope.extention = nameArray[nameArray.length - 1];
            if ($scope.extention === "jpg" || $scope.extention === "jpeg" || $scope.extention === "JPG" || $scope.extention === "JPEG" || $scope.extention === "png"
                || $scope.extention === "PNG" || $scope.extention === "jfif" || $scope.extention === "JFIF") {
                $('#preview').attr('src', $scope.HRMRAM_filepath);
                $('#myModal').modal('show');
            }
            else if ($scope.extention === "doc" || $scope.extention === "docx" || $scope.extention === "xls" || $scope.extention === "xlsx") {
                //$('#preview').removeAttr('src');
                $('#preview').attr('src', $scope.HRMRAM_filepath);
            }
            else if ($scope.extention === "pdf") {
                var imagedownload = $scope.HRMRAM_filepath;
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




        $scope.viewdocument = function (item) {

            if (item != null) {
                var img = item.HRMRAM_filepath;
                var imagarr = img.split('.');
                var extention = imagarr[imagarr.length - 1];
                item.filetype = extention;
                if (extention === 'doc' || extention === 'docx' || extention === 'ppt' || extention === 'pptx' || extention === 'xlsx' || extention === 'xls') {
                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + item.HRMRAM_filepath;
                }
                else if (extention === "jpg" || extention === "JPG" || extention === "jpeg" || extention === "JPEG" || extention === "png" || extention === "PNG" || extention == "jfif" || extension == "JFIF") {
                    $('#preview').removeAttr('src');
                    $('#preview').attr('src', item.HRMRAM_filepath);
                    $('#myModal').modal();
                    $('#myModal').modal({ keyboard: false });
                    $('#myModal').modal('show');
                }
                else if (extention === "pdf") {
                    var imagedownload = item.HRMRAM_filepath;
                    $scope.content = "";
                    var fileURL = "";
                    var file = "";
                    $http.get(imagedownload, { responseType: 'arraybuffer' })
                        .success(function (response) {
                            file = new Blob([(response)], { type: 'application/pdf' });
                            fileURL = URL.createObjectURL(file);
                            $scope.content = $sce.trustAsResourceUrl(fileURL);
                            $('#showpdf1').modal('show');
                        });
                }
            }
        };


        $scope.zoomin = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth >= 750) {
                swal("Maximum zoom-in level reached.");
            } else {
                myImg.style.width = (currWidth + 50) + "px";
            }
        };

        $scope.zoomout = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth <= 400) {
                swal("Maximum zoom-out level reached.");
            } else {
                myImg.style.width = (currWidth - 50) + "px";
            }
        };

    }

})();