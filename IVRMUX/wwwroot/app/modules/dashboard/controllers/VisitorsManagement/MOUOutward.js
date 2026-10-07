(function () {
    'use strict';
    angular
        .module('app')
        .controller('MOUOutwardController', MOUOutwardController)

    MOUOutwardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache', 'myFactoryvisitor', '$q', '$window']
    function MOUOutwardController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache, myFactoryvisitor, $q, $window) {

        $scope.sttud = false;
        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.FOOUT_DateTime = new Date();
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        var miid = myFactoryvisitor.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }

        //=========== page load
        //$scope.loadgrid = function () {
        //    if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
        //        $scope.mI_Id = 0;
        //    }  
        //    apiService.getURI("Outward/getDetails/", $scope.mI_Id).then(function (promise) {
        //        $scope.mI_Id = promise.mI_Id;
        //        $scope.emplist = promise.emplist;
        //        $scope.getdata = promise.getdata;
        //        $scope.clientproject_dd = promise.clientproject_dd;

        //        $scope.institutionlist = promise.institutionlist; 
        //    });
        //}


        //$scope.loadgrid123 = function () {
        //    angular.forEach($scope.institutionlist, function (cls) {
        //        if (cls.mI_Id == $scope.MI_Id) {
        //            $scope.mI_Name = cls.mI_Name;
        //        }
        //    })
        //}

        $scope.loadGridAndProcess = function () {
            $scope.loadgrid();  // Calls loadgrid()
            $scope.loadgrid123();  // Calls loadgrid123()$scope.institutionlist[0].mI_AboutInstitute
        }

        $scope.loadgrid = function () {
            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }
            apiService.getURI("Outward/getDetails/", $scope.mI_Id).then(function (promise) {
                $scope.mI_Id = promise.mI_Id;
                $scope.emplist = promise.emplist;
                $scope.getdata = promise.getdata;
                $scope.mougetdata = promise.mougetdata
                $scope.clientproject_dd = promise.clientproject_dd; //$scope.clientproject_dd[1].ISMMCLTPR_Id
                $scope.institutionlist = promise.institutionlist; 
            });
        };

        $scope.loadgrid123 = function () {
            angular.forEach($scope.institutionlist, function (cls) {
                if (cls.mI_Id == $scope.mI_Id) {
                    $scope.mI_Name = cls.mI_Name;
                }
            });
        };

        $scope.Alldata = function (ISMMCLTPR_Id) {
            angular.forEach($scope.clientproject_dd, function (cls) {
                if (cls.ISMMCLTPR_Id == ISMMCLTPR_Id.ISMMCLTPR_Id) {
                    $scope.ISMMCLT_ContactNo = cls.ISMMCLT_ContactNo;
                    $scope.ISMMCLT_EmailId = cls.ISMMCLT_EmailId;

                }
            });

        }


        //================== save data
        $scope.submitted = false;
        $scope.saveData = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.uploadarray = [];
                if ($scope.materaldocuupload.length > 0) {
                    angular.forEach($scope.materaldocuupload, function (itm) {
                        if (itm.ismclT_FilePath !== undefined && itm.ismclT_FilePath !== null && itm.ismclT_FilePath !== "") {
                            $scope.uploadarray.push({
                                FOOUTF_FileName: itm.vtadaaF_FileName,
                                FOOUTF_FilePath: itm.ismclT_FilePath,
                                FOOUTF_Filedesc: itm.VTADAAF_Remarks
                              
                            });
                        }
                    });
                }
                if ($scope.hrmE_Id != undefined) {
                    var date = new Date();
                    $scope.time = $filter('date')(new Date(), 'HH:mm:ss');

                    var obj = {
                        "FOOUT_Id": $scope.FOOUT_Id,
                        //"FOOUT_OutwardNo": $scope.FOOUT_OutwardNo,
                        "FOOUT_DateTime": new Date($scope.FOOUT_DateTime).toDateString(),
                        "FOOUT_Discription": $scope.FOOUT_Discription,
                        "FOOUT_From": $scope.mI_Name,
                        "FOOUT_To": $scope.ISMMCLTPR_Id.ISMMCLTPR_Id,
                        "FOOUT_Address": $scope.FOOUT_Address,
                        "FOOUT_PhoneNo": $scope.ISMMCLT_ContactNo,
                        "FOOUT_EmailId": $scope.ISMMCLT_EmailId,
                        "FOOUT_PostSalesFlag":"MOU",
                        "FOOUT_DispatachedBy": $scope.hrmE_Id.hrmE_Id,
                        "FOOUT_DispatchedThrough": $scope.FOOUT_DispatchedThrough,
                        "FOOUT_DispatchedDeatils": $scope.FOOUT_DispatchedDeatils,
                        "FilePath_Array": $scope.uploadarray,
                        "FOOUT_DispatchedPhNo": $scope.FOOUT_DispatchedPhNo,
                        "MI_Id": $scope.mI_Id,
                    }
                    apiService.create("Outward/saveData", obj).
                        then(function (promise) {
                            if (promise.returnval != null || promise.duplicate != null) {
                                if (promise.duplicate == false) {
                                    if (promise.returnval == "Save") {
                                        swal('Record Saved Successfully!!!');

                                        $scope.outward = promise.outward;
                                        $scope.printbutton = false;
                                        $scope.SchoolLogo = promise.institution[0].mI_Logo;
                                        $scope.SchollName = promise.institution[0].mI_Name;
                                        $scope.SchollAdd = promise.institution[0].mI_Address1;
                                        $scope.sttud = true;

                                        $scope.getdata.length = 0;
                                    }
                                    else if (promise.returnval == "Not Save") {
                                        swal('Record Not Saved Successfully!!!');
                                    }
                                    else if (promise.returnval == "Update") {
                                        swal('Record Updated Successfully!!!');
                                        $state.reload();
                                    }
                                    else if (promise.returnval == "Not Update") {
                                        swal('Record Not Updated Successfully!!!');
                                    }
                                }
                                else {
                                    swal("Record already exist");
                                }
                            }
                            else {
                                swal("Kindly Contact Administrator!!!");
                            }
                        });
                    $state.reload();
                }
                else {
                    swal('Please select Dispatached By!');
                }
            }
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cancel = function () {
            $state.reload();
        };       
        $scope.edit = function (obj) {
            $scope.materaldocuupload = [];
            var data = {
                "FOOUT_Id": obj.foouT_Id,
                "MI_Id": obj.mI_Id,
            }
            apiService.create("Outward/EditDetails/", data).then(function (promise) {
                $scope.attachementlist = promise.attachementlist;
                if ($scope.attachementlist != null && $scope.attachementlist.length > 0) {
                   
                    angular.forEach($scope.attachementlist, function (dd) {
                        $scope.materaldocuupload.push({ VTADAAF_Remarks: dd.fooutfU_Filedesc, ismclT_FilePath: dd.fooutfU_FilePath, ismclT_FileName: dd.fooutfU_FileName });
                    });
                }
                else {
                    $scope.materaldocuupload = [{ itrS_Id: 'trans1' }];
                    if ($scope.materaldocuupload.length === 1) {
                        $scope.cnt = 1;
                    }
                }               
                $scope.editDetails = promise.editDetails;
                $scope.mI_Id = promise.editDetails[0].mI_Id;
                $scope.FOOUT_Id = promise.editDetails[0].foouT_Id;
                $scope.FOOUT_OutwardNo = promise.editDetails[0].foouT_OutwardNo;
                $scope.FOOUT_PhoneNo = promise.editDetails[0].foouT_PhoneNo;
                $scope.FOOUT_From = promise.editDetails[0].foouT_From;
                $scope.FOOUT_To = promise.editDetails[0].foouT_To;
                $scope.FOOUT_DateTime = new Date(promise.editDetails[0].foouT_DateTime);
                $scope.FOOUT_Discription = promise.editDetails[0].foouT_Discription;
                $scope.FOOUT_DispatchedDeatils = promise.editDetails[0].foouT_DispatchedDeatils;
                $scope.FOOUT_DispatchedPhNo = promise.editDetails[0].foouT_DispatchedPhNo;
                $scope.FOOUT_DispatchedThrough = promise.editDetails[0].foouT_DispatchedThrough;
                $scope.FOOUT_EmailId = promise.editDetails[0].foouT_EmailId;
                $scope.FOOUT_Address = promise.editDetails[0].foouT_Address;
                $scope.hrmE_Id = promise.editDetails[0];              
                $scope.hrmE_Id.hrmE_Id = promise.editDetails[0].foouT_DispatachedBy;                
            });
        }
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
            console.log("Outward  Upload  :" + data);
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
            if ($scope.materaldocuupload != null && $scope.materaldocuupload.length < 5) {
                for (var i = 0; i === $scope.materaldocuupload.length; i++) {
                    var id = $scope.materaldocuupload[i].itrS_Id;
                    var lastChar = id.substr(id.length - 1);
                    $scope.cnt = parseInt(lastChar);
                }
            $scope.cnt = $scope.cnt + 1;
            $scope.tet = 'trans' + $scope.cnt;
            var newItemNo = $scope.cnt;
            $scope.materaldocuupload.push({ 'itrS_Id': 'trans' + newItemNo });
            }
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
               // $scope.filetype2 = lastelement;
            }
            if ($scope.lastelement == 'wmv' || $scope.lastelement == 'mp4') {

                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myvideoPreview').modal('show');

            }
            else if ($scope.lastelement == 'jpg' || $scope.lastelement == 'jpeg' || $scope.lastelement == 'png') {

                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');

            }
            else if ($scope.lastelement == 'doc' || $scope.lastelement == 'docx' || $scope.lastelement == 'xls' || $scope.lastelement == 'xlsx' || $scope.lastelement == 'ppt' || $scope.lastelement == 'pptx') {
                $window.open($scope.imagepreview);
            }
            else if ($scope.lastelement == 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');

            }
            else if ($scope.lastelement == 'pdf') {

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





        //============= Active/De-active record
        $scope.deactive = function (newuser1, SweetAlertt) {
            var mgs = "";
            if (newuser1.foouT_ActiveFlag == false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Outward/deactivate", newuser1).
                            then(function (promise) {
                                if (promise.returnval2 == true) {
                                    swal("Record " + mgs + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + mgs + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }


        //=============== print
        $scope.printbutton = true;
        $scope.Print = function () {
            var innerContents = '';
            innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/OutwardPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        }

       
        //=====================exportToExcel Record ==================//
        $scope.exportToExcel = function () {
            var printSectionId = '#table1';
            if ($scope.getdata !== null && $scope.getdata.length > 0) {
                var exportHref = Excel.tableToExcel(printSectionId, 'OUTWARD RECORD');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = "OUTWARD RECORD.xls";
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        };



    }

})();