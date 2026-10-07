(function () {
    'use strict';
    angular
        .module('app')
        .controller('TenantUploadNoticeController', TenantUploadNoticeController);
    TenantUploadNoticeController.$inject = ['$window', '$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce'];
    function TenantUploadNoticeController($window, $rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce) {

        $scope.UploadFile = [];
        // $scope.file_temp = [];
        $scope.search = "";
        $scope.intB_DispalyDisableFlg = false;
        $scope.checklink = false;
        //$scope.minDate = Date.now();

        $scope.obj = {};

        //------------for sorting.........
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };

        $scope.editflag = false;


        //----------load data into page.............
        $scope.loaddata = function () {

            $scope.feegrplist1 = [];
            $scope.feedeflist1 = [];
            $scope.feegrplist = [];
            $scope.feedeflist = [];
            $scope.sectionlist = [];
            $scope.sectionlistarray = [];

            $scope.studentlist = [];
            $scope.studentarray = [];

            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            $scope.itemsPerPage = 10;
            $scope.check_student = true;
            $scope.getclass = false;
            $scope.getclasscheck = false;
            var id = 1;
            apiService.getURI("TenantUploadNotice/Getdetails", id).
                then(function (promise) {
                    $scope.notice_details = promise.notice_details;
                    $scope.building = promise.building;
                   // $scope.tenants = promise.tenants;


                    //TENENTNTBFL_FilePath

                    if (promise.notice_details != null && promise.notice_details.length > 0) {

                        angular.forEach($scope.notice_details, function (ddd) {
                            if (ddd.TENENTNTBFL_FilePath != undefined && ddd.TENENTNTBFL_FilePath != '' && ddd.TENENTNTBFL_FilePath != null) {
                                var img = ddd.TENENTNTBFL_FilePath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                ddd.filetype = lastelement;
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls' || lastelement === 'ppsx') {
                                    ddd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + ddd.TENENTNTBFL_FilePath;
                                }
                            }


                        })

                    }

                    console.log($scope.notice_details)
                });
        };


        $scope.onchangebuilding = function () {
            var data = {
                "HRMB_Id": $scope.obj.HRMB_Id,
            }
            apiService.create("TenantUploadNotice/gettenant", data).then(function (promise) {
              
                if (promise.tenants.length > 0 && promise.tenants !== null) {
                    $scope.tenants = promise.tenants;
                }
                else {
                    swal('No Data Found!!!')
                }
            })
        }

        var imagedownload = "";
        var docname = "";
        $scope.downloaddirectpdf = function (data, idd) {

            var studentreg = idd;

            $scope.imagedownload = data;
            imagedownload = data;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg + '.pdf'
                    })[0].click();
                });
        };

        var imagedownloadppt = "";
        var docname = "";
        $scope.downloaddirectppt = function (data, idd) {

            var studentreg = idd;

            $scope.imagedownloadppt = data;
            imagedownloadppt = data;

            $http.get(imagedownloadppt, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg + '.ppt || .pptx || .ppsx'
                    })[0].click();
                });
        };
        $scope.downloaddirectimage = function (data, idd) {

            var studentreg = idd;

            $scope.imagedownload = data;
            imagedownload = data;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg + '.jpg'
                    })[0].click();
                });
        };

        $scope.toggleAllstd = function () {

            $scope.sendsmsemail = false;
            var toggleStatus = $scope.stdall;
            angular.forEach($scope.notice_details, function (itm) {
                itm.stdselected = toggleStatus;
            });
            angular.forEach($scope.notice_details, function (itm) {
                if (itm.stdselected == true) {
                    $scope.sendsmsemail = true;
                }

            });

        };
        $scope.optionToggledstd = function () {
            $scope.sendsmsemail = false;
            $scope.stdall = $scope.notice_details.every(function (itm) {
                return itm.stdselected;
            });
            angular.forEach($scope.notice_details, function (itm) {
                if (itm.stdselected == true) {
                    $scope.sendsmsemail = true;
                }

            });

        };

        // Send Mail 
        $scope.SendEmail = function () {

            $scope.noticeid = [];

            angular.forEach($scope.notice_details, function (ty) {
                if (ty.stdselected == true) {

                    $scope.noticeid.push({
                        TENENTNTB_Id: ty.TENENTNTB_Id
                    });
                }
            })
            var data =
            {
                "TENANTNOTICE_Ids": $scope.noticeid
            };


            apiService.create("TenantUploadNotice/sendmail", data).
                then(function (promise) {
                    if (promise.message == "success") {
                        swal("Email Sent..!!!");
                        $state.reload();
                    }
                    else if (promise.message == "notFound") {
                        swal("Email Not sent..!!!", 'Default Email-Id is Not Found.. !!!');
                        $state.reload();
                    }
                    else if (promise.message == "Error") {
                        swal("Something went wrong", 'Try After some time..!!');
                        $state.reload();

                    } else {
                        swal("Something went wrong", 'Try After some time..!!');
                        $state.reload();
                    }
                });


        };


        //-----------upload file/photo.............
        $scope.uploadFile = function (input, document) {

            $scope.UploadFile = input.files;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.ms-excel" || input.files[0].type === "application/pptx" || input.files[0].type === "application/ppsx" || input.files[0].type === "application/vnd.ms-powerpoint" || input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation" || input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" || input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.slideshow" || input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation") {

                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $('#blahD')
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();
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
            $http.post("/api/ImageUpload/Uploadnaacdocuments", formData,
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

        ///=========clear upload field data......
        $scope.remove_file = function () {
            $scope.file_detail = "";
            $scope.notice = "";
        };
        //-------check date
        $scope.display_Date = function () {
            if ($scope.obj.intB_DisplayDate > $scope.obj.intB_EndDate) {
                swal("Display Date Cannot Greater than End date ");
                $scope.obj.intB_DisplayDate = "";
            }
            else if ($scope.obj.intB_StartDate > $scope.obj.intB_EndDate) {
                swal("Start Date Cannot Greater than End date ");
                $scope.obj.intB_EndDate = "";
            }
        };

        //-------------search date
        $scope.filterValue = function (obj) {
            angular.lowercase(obj.intB_Title).indexOf(angular.lowercase($scope.search)) >= 0 ||
                angular.lowercase(obj.intB_Description).indexOf(angular.lowercase($scope.search)) >= 0 ||
                angular.lowercase(obj.intB_Attachment).indexOf(angular.lowercase($scope.search)) >= 0 ||
                ($filter('date')(obj.intB_StartDate, 'dd/MM/yyyy').indexOf($scope.search) > 0) ||
                ($filter('date')(obj.intB_EndDate, 'dd/MM/yyyy').indexOf($scope.search) > 0) ||
                ($filter('date')(obj.intB_DisplayDate, 'dd/MM/yyyy').indexOf($scope.search) > 0);
        };
        $scope.filterValue = function (obj) {
            return $filter('date')(obj.OrderDate, 'MM/dd/yyyy') === $filter('date')($scope.searchValue, 'MM/dd/yyyy');
        };

        //========classlist CheckBox Field Validation===========//
        $scope.isOptionsRequireddesc = function () {
            return !$scope.designationdropdown.some(function (item) {
                return item.selected;
            });
        };
        $scope.isOptionsRequiredclass = function () {
            return !$scope.classlist.some(function (item) {
                return item.selected;
            });
        };
        $scope.isOptionsRequired = function () {
            return !$scope.sectionlist.some(function (item) {
                return item.selected;
            });
        };
        $scope.isOptionsRequiredgrp = function () {
            return !$scope.feegrplist.some(function (item) {
                return item.selected;
            });
        };
        $scope.isOptionsRequireddept = function () {
            return !$scope.departmentdropdown.some(function (item) {
                return item.selected;
            });
        };

        $scope.isOptionsRequiredemp = function () {
            return !$scope.get_userEmplist.some(function (item) {
                return item.empck;
            });
        };

        $scope.isOptionsRequiredgroup = function () {
            return !$scope.feegrplist.some(function (item) {
                return item.selected;
            });
        };

        $scope.isOptionsRequiredterms = function () {
            return !$scope.feedeflist.some(function (item) {
                return item.selected;
            });
        };

        $scope.isOptionsRequiredstu = function () {
            return !$scope.studentlist.some(function (item) {
                return item.selected;
            });
        };


        //$scope.isOptionsRequired1 = function () {
        //    return !$scope.studentlist.some(function (item) {
        //        return item.selected;
        //    });
        //};

        //=======selection of checkbox....
        //$scope.togchkbxC = function () {
        //    $scope.usercheckC = $scope.sectionlist.every(function (role) {
        //        return role.selected;
        //    });


        //};
        $scope.togchkbxS = function () {
            $scope.studentarray = [];
            angular.forEach($scope.studentlist, function (qq) {
                if (qq.selected == true) {
                    $scope.studentarray.push({ AMST_Id: qq.amsT_Id })
                }
            })
        }
        //fee group
        $scope.togchkbxG = function () {
            $scope.defgrparray = [];
            angular.forEach($scope.tenants, function (qq) {
                if (qq.selected == true) {
                    $scope.defgrparray.push({ TENANTAGR_Id: qq.tenantagR_Id })
                }
            })
        }

        $scope.togchkbxF = function () {
            $scope.deflistarray = [];
            angular.forEach($scope.feedeflist, function (qq) {
                if (qq.selected == true) {
                    $scope.deflistarray.push({ FMT_Id: qq.fmT_Id })
                }
            })
        }
        //
        $scope.all_checkC = function (all, ASMCL_Id) {
            $scope.sectionlistarray = [];
            $scope.obj.usercheckC = all;
            var toggleStatus = $scope.obj.usercheckC;
            angular.forEach($scope.sectionlist, function (role) {
                role.selected = toggleStatus;
            });

            $scope.sectionlistarray = [];
            angular.forEach($scope.sectionlist, function (qq) {
                if (qq.selected == true) {
                    $scope.sectionlistarray.push({ ASMS_Id: qq.asmS_Id, ASMCL_Id: qq.asmcL_Id })

                    //$scope.sectionlistarray.push({ ASMS_Id: qq.asmS_Id })
                }
            });
            $scope.getstudent();


        };

        $scope.getstudent = function () {
            $scope.studentlist = [];
            if ($scope.obj.fee_def == true) {
                $scope.defgrparray = [];
                angular.forEach($scope.feegrplist, function (qq) {
                    if (qq.selected == true) {
                        $scope.defgrparray.push({ FMG_Id: qq.fmG_Id })
                    }
                });

                $scope.deflistarray = [];
                angular.forEach($scope.feedeflist, function (qq) {
                    if (qq.selected == true) {
                        $scope.deflistarray.push({ FMT_Id: qq.fmT_Id })
                    }
                });
            }

            //added by roopa//
            $scope.classlistarray = [];

            if ($scope.classlistarray.length == 0 || $scope.classlistarray == null) {
                angular.forEach($scope.classlist, function (qq) {
                    if (qq.selected == true) {
                        $scope.classlistarray.push({ ASMCL_Id: qq.asmcL_Id })

                    }
                });
            }

            if ($scope.sectionlist != null && $scope.sectionlist.length > 0) {
                $scope.sectionlistarray = [];
                angular.forEach($scope.sectionlist, function (qq) {
                    if (qq.selected == true) {
                        $scope.sectionlistarray.push({ ASMS_Id: qq.asmS_Id, ASMCL_Id: qq.asmcL_Id })
                        // $scope.sectionlistarray.push({ ASMS_Id: qq.asmS_Id })
                    }
                });
            }

            //if ($scope.obj.select_student == true) {
            var data = {
                // "ASMCL_Id": ASMCL_Id,
                "classlsttwo": $scope.classlistarray,
                "sectionlistarray": $scope.sectionlistarray,
                "defarray": $scope.deflistarray,
                "defgrparray": $scope.defgrparray,
                "fee_def": $scope.obj.fee_def
            }
            apiService.create("TenantUploadNotice/getstudent", data).then(function (promise) {
                $scope.studentlist1 = [];
                $scope.studentlist = [];
                $scope.studentlist1 = promise.studentlist;
                if ($scope.studentlist1.length > 0 || $scope.studentlist1 != null) {
                    $scope.studentlist = $scope.studentlist1;
                }
                else {
                    swal('No Data Found!!!')
                }
            })
            // }
        }
        //==========dept =================
        $scope.all_checkdept = function (departmentselectedAll) {
            var toggleStatus = departmentselectedAll;
            angular.forEach($scope.departmentdropdown, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.arrayuserdept = [];
            angular.forEach($scope.departmentdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserdept.push(em);
                }
            });

            $scope.searchValueDesg = '';
            $scope.designationselectedAll = false;
            $scope.Deptselectiondetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };
        $scope.togchkbxdept = function (groupType) {
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {
                return itm.selected;
            });
            $scope.searchValueDesg == '';
            $scope.designationselectedAll = false;
            $scope.Deptselectiondetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.searchValueUEM = '';
            $scope.employeeid = [];
            $scope.checkall = false;
            $scope.get_deviationreport = [];
        };
        $scope.Deptselectiondetails = function () {
            $scope.arrayuserdept = [];
            $scope.designationdropdown = [];
            $scope.get_userEmplist = [];
            angular.forEach($scope.departmentdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserdept.push(em);
                }
            });
            var data = {
                "departmentlist": $scope.arrayuserdept
            };
            apiService.create("TenantUploadNotice/Deptselectiondetails", data).then(function (promise) {
                if (promise.designation != null && promise.designation.length > 0) {
                    $scope.designationdropdown = promise.designation;

                }
                else {

                    swal("No Record  Found..... !!");
                }
            });
        };
        //===========designation============
        $scope.all_checkdesg = function (designationselectedAll) {
            var toggleStatus = designationselectedAll;
            angular.forEach($scope.designationdropdown, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_userEmplist = [];
            $scope.arrayuserdesig = [];
            angular.forEach($scope.designationdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserdesig.push(em);
                }
            });
            $scope.des_test = true;
            $scope.togchkbxdesg();
        };

        $scope.togchkbxdesg = function (groupType) {
            if ($scope.des_test == true) {
                var data = {
                    "designationlist": $scope.arrayuserdesig

                };
            }
            else {
                $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {
                    return itm.selected;
                });
                $scope.arrayuserdesig = [];
                angular.forEach($scope.designationdropdown, function (em) {
                    if (em.selected === true) {
                        $scope.arrayuserdesig.push(em);
                    }
                });
                var data = {
                    "designationlist": $scope.arrayuserdesig

                };
            }

            apiService.create("TenantUploadNotice/Desgselectiondetails", data).then(function (promise) {
                if (promise.get_userEmplist.length > 0) {

                    $scope.des_test = false;
                    $scope.get_userEmplist = promise.get_userEmplist;
                    if ($scope.get_userEmplist.length > 0) {
                        angular.forEach($scope.get_userEmplist, function (uem) {
                            uem.empck = false;
                        });
                        $scope.checkall = false;
                    }
                }
                else {
                    $scope.searchValueUEM = '';
                    $scope.employeeid = [];
                    $scope.checkall = false;
                    swal("No Record  Found..... !!");
                }
            });
        };
        //---------all checkbox Select...
        $scope.all_check = function (checkall) {
            $scope.userc = checkall;
            var toggleStatus = $scope.userc;
            angular.forEach($scope.get_userEmplist, function (role) {
                role.empck = toggleStatus;
            });

            $scope.employeearraylist = [];
            angular.forEach($scope.get_userEmplist, function (qq) {
                if (qq.empck == true) {
                    $scope.employeearraylist.push({ HRME_Id: qq.HRME_Id })
                }
            })
        }



        $scope.all_checkS = function (all) {
            $scope.usercheckS = all;
            var toggleStatus = $scope.usercheckS;
            angular.forEach($scope.studentlist, function (role) {
                role.selected = toggleStatus;
            });

            $scope.studentarray = [];
            angular.forEach($scope.studentlist, function (qq) {
                if (qq.selected == true) {
                    $scope.studentarray.push({ AMST_Id: qq.amsT_Id })
                }
            })



        };
        // get section
        $scope.getsection = function (ASMCL_Id) {

            //$scope.asmclid = ASMCL_Id;
            //if ($scope.asmclid === "All") {
            //    $scope.classlistarray = [];
            //    $scope.getclass = true;
            //    angular.forEach($scope.classlist, function (aa) {
            //        $scope.classlistarray.push({ ASMCL_Id: aa.asmcL_Id })
            //    });
            //}
            //else {
            //    var data = {
            //        "ASMCL_Id": ASMCL_Id
            //    }
            //    apiService.create("TenantUploadNotice/getsection", data).then(function (promise) {
            //        if (promise.sectionlist.length > 0 || promise.sectionlist != null) {
            //            $scope.sectionlist = promise.sectionlist;
            //            $scope.getclass = false;
            //        }
            //        else {
            //            swal('No data Found!!!');
            //        }
            //    });
            //}


            //added by roopa//
            $scope.classlistarray = [];

            angular.forEach($scope.classlist, function (aa) {
                if (aa.selected == true) {
                    $scope.classlistarray.push({ ASMCL_Id: aa.asmcL_Id })
                }

            });

            if ($scope.classlistarray != null) {
                $scope.classflag = true;
            }

            var data = {
                "classlsttwo": $scope.classlistarray
            }
            apiService.create("TenantUploadNotice/getsection", data).then(function (promise) {
                if (promise.sectionlist.length > 0 || promise.sectionlist != null) {
                    $scope.sectionlist = promise.sectionlist;
                    $scope.getclass = false;
                }
                else {
                    swal('No data Found!!!');
                }
            });
            //
        }
        //============employee list======
        $scope.togchkbx = function () {
            $scope.employeearraylist = [];
            angular.forEach($scope.get_userEmplist, function (qq) {
                if (qq.empck == true) {
                    $scope.employeearraylist.push({ HRME_Id: qq.HRME_Id })
                }
            })
        }
        //------------save data.....
        $scope.classlstdata = [];
        $scope.submitted = false;
        $scope.savedata = function () {

            $scope.classlstdata = [];
            var data = {};
            var displaydate = "";
            var hrmbid = 0
            if ($scope.obj.HRMB_Id != 0 && $scope.obj.HRMB_Id != null) {
                hrmbid = parseInt($scope.obj.HRMB_Id);
            }


            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.intB_DispalyDisableFlg === false) {

                    displaydate = null;
                }
                else if ($scope.intB_DispalyDisableFlg === true) {
                    displaydate = $scope.obj.tenentntB_DisplayDate === null ? "" : $filter('date')($scope.obj.tenentntB_DisplayDate, "yyyy-MM-dd");
                }

                $scope.filedoc = [];

                $scope.documentListOtherDetails11 = [];
                if ($scope.documentListOtherDetails != null) {
                    angular.forEach($scope.documentListOtherDetails, function (qq) {
                        //if (qq.tenentntbfL_FilePath != null) {
                        $scope.documentListOtherDetails11.push({ TENENTNTBFL_FilePath: qq.tenentntbfL_FilePath, TENENTNTBFL_FileName: qq.FileName });
                        //}
                    })
                    $scope.filedoc = $scope.documentListOtherDetails11;
                }


                $scope.studentarraynew = [];
                //if ($scope.tenants != null || $scope.tenants > 0) {
                //    $scope.studentarraynew = $scope.tenants;
                //}
                //else {
                //    angular.forEach($scope.tenants, function (qq) {
                //        $scope.studentarraynew.push({ TENANTAGR_Id: qq.tenantagR_Id });
                //    })
                //}

                if ($scope.obj.TENENTNTB_AllOrIndFlg == 'Individual') {
                    angular.forEach($scope.tenants, function (em) {
                        if (em.selected === true) {
                            $scope.studentarraynew.push({ TENANTAGR_Id: em.tenantagR_Id });
                        }
                    });

                } else if ($scope.obj.TENENTNTB_AllOrIndFlg == 'All') {
                    $scope.studentarraynew = $scope.tenants;
                }


                var startdate = $scope.obj.tenentntB_StartDate === null ? "" : $filter('date')($scope.obj.tenentntB_StartDate, "yyyy-MM-dd");
                var enddate = $scope.obj.tenentntB_EndDate === null ? "" : $filter('date')($scope.obj.tenentntB_EndDate, "yyyy-MM-dd");





                data = {
                    "TENENTNTB_Id": $scope.tenentntB_Id,
                    "dd": hrmbid,
                    "TENENTNTB_Title": $scope.tenentntB_Title,
                    "TENENTNTB_Description": $scope.tenentntB_Description,
                    "TENENTNTB_DisplayDate": displaydate,

                    "TENENTNTB_StartDate": startdate,
                    "TENENTNTB_EndDate": enddate,
                    "TENENTNTB_AllOrIndFlg": $scope.obj.TENENTNTB_AllOrIndFlg,
                    "studentarray": $scope.studentarraynew,
                    "FilePath_Array": $scope.filedoc,



                };


                apiService.create("TenantUploadNotice/savedetail", data).
                    then(function (promise) {
                        if (promise.message === 'Duplicate') {
                            swal('Record Already Exist');
                        }
                        else if (promise.message === 'ex') {
                            swal('Exceeds the number of Questions');
                        }
                        else {
                            if (promise.message === "Add") {
                                if (promise.returnval === true) {
                                    swal("Record Saved Successfully");
                                } else {
                                    swal("Failed To Save Record");
                                }
                            } else if (promise.message === "Update") {
                                if (promise.returnval === true) {
                                    swal("Record Updated Successfully");
                                } else {
                                    swal("Failed To Update Record");
                                }
                            } else {
                                swal("Failed To Save/Update Record");
                            }
                        }
                        $state.reload();
                    });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //==============view data=========

        $scope.viewData = function (option) {
            $scope.attachementlist = [];
            var data = {
                "TENENTNTB_Id": option.TENENTNTB_Id

            };
            apiService.create("TenantUploadNotice/viewData", data)
                .then(function (promise) {

                    if (promise.attachementlist.length > 0) {
                        $scope.attachementlist1 = [];
                        angular.forEach(promise.attachementlist, function (qq) {
                            $scope.img = qq.TENENTNTBFL_FilePath;
                            if ($scope.img != null) {
                                var imagarr = $scope.img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];

                                $scope.filetype2 = lastelement;
                            }

                            if ($scope.filetype2 == 'mp4' || $scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp3'
                                || $scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' ||
                                $scope.filetype2 == 'pdf' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx'
                                || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'ppsx'
                                || $scope.filetype2 == 'doc' || $scope.filetype2 == 'docx') {
                                $scope.attachementlist1.push({
                                    tenantntbfL_FileName: qq.tenantntbfL_FileName,
                                    TENENTNTBFL_FilePath: qq.TENENTNTBFL_FilePath,
                                    INTB_Attachment: qq.intB_Attachment,
                                    INTB_Id: qq.intB_Id
                                })
                            }
                            else {
                                $scope.attachementlist1.push({

                                    tenantntbfL_FileName: qq.tenantntbfL_FileName,
                                    TENENTNTBFL_FilePath: qq.TENENTNTBFL_FilePath,
                                    INTB_Attachment: 'HyperLink',
                                    INTB_Id: qq.intB_Id

                                })
                            }
                        })

                        $scope.attachementlist = $scope.attachementlist1

                        $('#myModalCoverview').modal('show');
                        $scope.docshowary = true;
                        $scope.docshow = false;
                    }
                    else {
                        swal("No Data Found.")

                    }

                });
        };
        //============
        $scope.checkteru = function () {
            if ($scope.check_student == true && $scope.check_staff == false) {
                $scope.check_student = true;
            }
            else if ($scope.check_student == false && $scope.check_staff == true) {
                $scope.check_staff = true;
            }
            else if ($scope.check_student == true && $scope.check_staff == true) {
                $scope.check_staff = true;
                $scope.check_student = true;
            }
            else if ($scope.check_student == false && $scope.check_staff == false) {
                $scope.check_staff = false;
                $scope.check_student = true;
            }
        }
        //----------------edit data.......
        $scope.editnotice = function (id) {
            $scope.checklink = false;
            var data = {
                "TENENTNTB_Id": id.TENENTNTB_Id
                //"classlsttwo": $scope.classlistarray,
                //"sectionlistarray": $scope.sectionlistarray,
                //"defarray": $scope.deflistarray,
                //"defgrparray": $scope.defgrparray,
                //"fee_def": $scope.obj.fee_def

            };
            apiService.create("TenantUploadNotice/editdetails", data).then(function (promise) {
                $scope.editdetails = [];
                if (promise.editdetails.length > 0) {

                    $scope.editflag = true;
                    $scope.tenentntB_Id = promise.editdetails[0].tenentntB_Id;
                    $scope.obj.HRMB_Id = promise.editdetails[0].hrmB_Id;
                    $scope.tenentntB_Title = promise.editdetails[0].tenentntB_Title;
                    $scope.tenentntB_Description = promise.editdetails[0].tenentntB_Description;

                    $scope.obj.tenentntB_StartDate = new Date(promise.editdetails[0].tenentntB_StartDate);
                    $scope.obj.tenentntB_EndDate = new Date(promise.editdetails[0].tenentntB_EndDate);
                    $scope.obj.tenentntB_DisplayDate = new Date(promise.editdetails[0].tenentntB_DisplayDate);
                    if (promise.editdetails[0].tenentntB_AllOrIndFlg == "Individual") {
                        $scope.obj.TENENTNTB_AllOrIndFlg = "Individual";
                    } else {
                        $scope.obj.TENENTNTB_AllOrIndFlg = "All";
                    }


                    $scope.urldocumentlist = [];
                    $scope.documentListOtherDetails = [];
                    if (promise.attachementlist != null && promise.attachementlist.length > 0) {
                        angular.forEach(promise.attachementlist, function (aa) {
                            $scope.img = aa.tenentntbfL_FilePath;
                            if ($scope.img != null) {
                                var imagarr = $scope.img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                $scope.filetype2 = lastelement;
                            }

                            if ($scope.filetype2 == 'mp4' || $scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp3'
                                || $scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' ||
                                $scope.filetype2 == 'pdf' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx'
                                || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'ppsx'
                                || $scope.filetype2 == 'doc' || $scope.filetype2 == 'docx') {
                                $scope.documentListOtherDetails.push({
                                    id: 1, tenentntbfL_FilePath: aa.tenentntbfL_FilePath,
                                    FileName: aa.tenentntbfL_FileName
                                });
                            }
                            else {
                                $scope.urldocumentlist.push({
                                    id: 1, tenentntbfL_FilePath: aa.tenentntbfL_FilePath,
                                    FileName: aa.tenantntbfL_FileName
                                });

                            }
                        });
                    }
                    else {
                        $scope.documentListOtherDetails.push({
                            id: 1, TENENTNTBFL_FilePath: '',
                            FileName: ''
                        });

                    }


                    for (var i = 0; i < $scope.tenants.length; i++) {

                        var name = $scope.tenants[i].tenantagR_Id;
                        if (name == promise.editclasslist[i].tenantagR_Id) {
                            $scope.tenants[i].selected = true

                        }
                        else {
                            $scope.tenants[i].selected = false;

                        }

                    }


                }




            });
        };

        $scope.deactiveY = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            if (employee.TENENTNTB_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            } else if (employee.TENENTNTB_ActiveFlag === false) {
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
                        apiService.create("TenantUploadNotice/deactivate", employee).
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
        $scope.clear = function () {
            $state.reload();
        };

        $scope.clearcls = function () {



            if (($scope.obj.select_student == false && $scope.obj.fee_def == false) || ($scope.obj.select_student == true && $scope.obj.fee_def == true)) {


                $scope.obj.usercheckC = false;
                $scope.obj.usercheckCC = false;


                angular.forEach($scope.classlist, function (qq) {
                    if (qq.selected == true) {
                        qq.selected = false;


                    }

                });

                angular.forEach($scope.sectionlist, function (qq) {
                    if (qq.selected == true) {
                        qq.selected = false;

                    }
                    else {

                        qq.selected = true;
                    }
                });



            }
            if ($scope.obj.fee_def == true) {
                $scope.studentlist = [];

            }



        };

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
        $scope.urldocumentlist = [{ id: 'document' }];
        $scope.addNewurl = function () {
            var newItemNo = $scope.urldocumentlist.length + 1;
            if (newItemNo <= 30) {
                $scope.urldocumentlist.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewurl = function (index, data) {
            var newItemNo = $scope.urldocumentlist.length - 1;
            $scope.urldocumentlist.splice(index, 1);
            if (data.hreothdeT_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
        };
        //======================= file upload
        //$scope.selectFileforUploadzdOtherDetail = function (input, document) {
        //    $scope.SelectedFileForUploadzdOtherDetail = input.files;
        //    if (input.files && input.files[0]) {
        //        var reader = new FileReader();
        //        reader.onload = function (e) {
        //            $('#blahD')
        //                .attr('src', e.target.result);
        //        };
        //        reader.readAsDataURL(input.files[0]);
        //        UploadEmployeeDocumentOtherDetail(document);

        //    }
        //};
        $scope.selectFileforUploadzdOtherDetail = function (input, document) {

            $scope.uploadtecherdocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg")  // 2097152 bytes = 2MB 
                {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload  Pdf, Doc, Image Files Only");
                }
            }
        };

        function UploaddianPhoto(data) {
            console.log("Teacher Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadtecherdocuments1.length; i++) {
                formData.append("File", $scope.uploadtecherdocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/lessonplannerdoc", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);

                    data.tenentntbfL_FilePath = d;
                    data.FileName = $scope.filename;
                    $('#').attr('src', data.tenentntbfL_FilePath);
                    var img = data.tenentntbfL_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.tenentntbfL_FilePath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }


        function UploadEmployeeDocumentOtherDetail(data) {
            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetail.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetail[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/lessonplannerdoc", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.tenentntbfL_FilePath = d;
                    data.fileName = $scope.FileName;
                    $('#').attr('src', data.tenentntbfL_FilePath);
                    var img = data.tenentntbfL_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.tenentntbfL_FilePath;
                    }

                    //defer.resolve(d);
                    //if (d !== "PathNotFound") {

                    //    data.tenentntbfL_FilePath = d[0].path;
                    //    data.FileName = d[0].name;

                    //} else {
                    //    swal('Document Storage Path Not Found ..!!');
                    //}
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }


        $scope.previewimg_new = function (img) {
            $scope.imagepreview = img;
            $scope.view_videos = [];
            var img = $scope.imagepreview;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {

                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myvideoPreview').modal('show');

            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {

                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');

            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
            }
            else if ($scope.filetype2 == 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');

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
        $scope.flag = "";
        $scope.notice_name = "";
        $scope.viewrecordstwo = function (user) {

            var data = user;

            apiService.create("TenantUploadNotice/viewrecords", data).then(function (promise) {
                $scope.viewrecords = [];
                if (promise !== null) {
                    if (promise.viewlist != null && promise.viewlist.length > 0) {
                        $scope.flag = promise.flag;

                        $scope.notice_name = promise.notice_Name;

                        $scope.viewrecords = promise.viewlist;
                    }

                }

            });
        };


        $scope.al_checkclass = function (all, ASMCL_Id) {


            $scope.classlistarray = [];
            $scope.obj.usercheckCC = all;

            var toggleStatus = $scope.obj.usercheckCC;
            angular.forEach($scope.classlist, function (role) {
                role.selected = toggleStatus;
            });


            $scope.classlistarray = [];
            angular.forEach($scope.classlist, function (qq) {
                if (qq.selected == true) {
                    $scope.classlistarray.push({ ASMCL_Id: qq.asmcL_Id, ASMCL_Id: qq.asmcL_Id })
                }
            });


            if ($scope.obj.usercheckCC == true) {
                $scope.getsection();
                $scope.classflag = true;
            }
            else {
                $scope.sectionlist = [];

            }

        }

        $scope.all_checkG = function (all, TENANTAGR_Id) {

            $scope.defgrparray = [];
            $scope.usercheckG = all;
            var toggleStatus = $scope.usercheckG;
            angular.forEach($scope.tenants, function (role) {
                role.selected = toggleStatus;
            });


            $scope.defgrparray = [];
            angular.forEach($scope.tenants, function (qq) {
                if (qq.selected == true) {
                    $scope.defgrparray.push({ TENANTAGR_Id: qq.tenantagR_Id })
                }
            });


        }

        $scope.all_checkF = function (all, FMT_Id) {

            $scope.deflistarray = [];
            $scope.usercheckF = all;
            var toggleStatus = $scope.usercheckF;
            angular.forEach($scope.feedeflist, function (role) {
                role.selected = toggleStatus;
            });


            $scope.deflistarray = [];
            angular.forEach($scope.feedeflist, function (qq) {
                if (qq.selected == true) {
                    $scope.defgrparray.push({ FMT_Id: qq.fmT_Id })
                }
            });

        }

        $scope.onyearchange = function (ASMAY_Id) {
            $scope.sectionlist = [];
            $scope.asmcL_Id = "";
            $scope.sectionlist = [];
            $scope.asmS_Id = "";
            var data = {
                "HRME_Id": $scope.hrmE_Id,
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("TenantUploadNotice/get_noticelist", data).then(function (promise) {
                if (promise.classlist.length > 0) {

                    $scope.classwork = promise.classlist;

                }
                else {
                    swal('Data Not Available');
                    $scope.asmcL_Id = "";
                }
            });
        };
        //====================end==================
    }
    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });

    angular.module('app').directive('txtArea', function () {
        return {
            restrict: 'AE',
            replace: 'true',
            scope: { data: '=', model: '=ngModel' },
            template: "<textarea></textarea>",
            link: function (scope, elem) {
                scope.$watch('data', function (newVal) {
                    if (newVal) {
                        scope.model += newVal[0];
                    }
                });
            }
        };



    });



})();
