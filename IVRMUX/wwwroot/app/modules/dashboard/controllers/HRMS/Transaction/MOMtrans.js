(function () {
    'use strict';
    angular.module('app').controller('MOM_TransactionController', MOM_TransactionController)
    MOM_TransactionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache', 'myFactoryvisitor', '$q']
    function MOM_TransactionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache, myFactoryvisitor, $q) {
        $scope.Vendornames = [];
        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.obj1 = {};
        $scope.obsupdate = {};
        $scope.editflag = false;
        $scope.edit = false;
        $scope.plMaxdate = new Date();
        $scope.MOM_Date = new Date();
        $scope.plMaxdate.setDate($scope.plMaxdate.getDate());
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };
        var miid = myFactoryvisitor.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }
        $scope.radiomodal = 'InternalDept';
        $scope.radiomodal1 = 'Internal';
        $scope.meetingmodal = 'meeting1';
        $scope.loadgrid = function () {
            $scope.radiomodal1 = 'Internal';
            $scope.radiomodal = 'InternalDept';
            apiService.getURI("MOMUpdate/getonloaddata/", $scope.mI_Id).then(function (promise) {
                $scope.griddata = promise.griddata;
                $scope.venue = promise.venue;
                $scope.meetingname = promise.meetingname;
                $scope.departmentdropdown = promise.departmentdropdown;
                $scope.empname = promise.approvedata;
                $scope.Vendornames = promise.vendornames;
                $scope.GetDetailsByDepartment();
            });
        };


        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        $scope.submitted = false;

        //===========================ADD==================================
        $scope.employeenameschange = [{ id: 'sell11' }];

        $scope.addNewDetail = function () {
            var newItemNo = $scope.employeenameschange.length + 1;
            if (newItemNo <= 100) {
                $scope.employeenameschange.push({ 'id': 'sell11' + newItemNo });
            }
        };

        $scope.removeNewDetail = function (index, data) {
            var newItemNo = $scope.employeenameschange.length - 1;
            $scope.employeenameschange.splice(index, 1);
            if (data.hrmE_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
        };
        //==============================

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //===================Active/Deactive=====================////////////////////////////
        $scope.deactive = function (obj) {

            var mgs = "";
            var confirmmgs = "";
            if (obj.moM_ActiveFlag === true) {
                mgs = "Cancel";
                confirmmgs = "Cancel";
            }
            else {
                mgs = "Schedule";
                confirmmgs = "Schedule";

            }

            var data = {
                "MOM_Id": obj.moM_Id,
                "MI_Id": obj.mI_Id,

            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To " + mgs + " Meeting ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("MOMUpdate/deactivate", data).then(function (promise) {

                            if (promise.retrunMsg === 'true') {
                                swal("Meeting " + confirmmgs + " " + "successfully", "", "success");
                            }
                            else {
                                swal("Meeting " + mgs + " Failed", "", "error");
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Meeting " + mgs + " Cancelled");
                    }
                });
        };



        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
        //$scope.selectedVendor = "";
        $scope.vendorslist = [];
        $scope.other = {}; 
        $scope.saveData = function () {
            if ($scope.myForm.$valid) {
                $scope.vendorslist = $scope.other.selectedVendor1;
                if ($scope.editflag === true) {
                    $scope.employeenameschange = [];
                }
                else {

                }
                $scope.newemp = [];
                if ($scope.employeenameschange.length > 0) {
                    angular.forEach($scope.employeenameschange, function (rrr) {
                        var hrme_id = 0;
                        if ((rrr.radiomodal1 == "External")) {
                            $scope.newemp.push({
                                HRME_Id: hrme_id,
                                MOMPRT_ParticipantsName: rrr.Particularname,
                                MOMPRT_MobileNo: rrr.MobileNo,
                                MOMPRT_Email: rrr.EmailId,
                                MOMPRT_Designation: rrr.DesignationName,
                                MOMPRT_EXTINTFlag: rrr.radiomodal1,
                                INVMS_Id: rrr.selectedVendor1.INVMS_Id,
                                INVMS_SupplierName: rrr.selectedVendor1.INVMS_SupplierName
                            });
                        }
                        else if (rrr.radiomodal1 == "Internal") {
                            $scope.editDetails = [];
                            if ($scope.selectedemp.length > 0) {
                                angular.forEach($scope.selectedemp, function (dd) {
                                    var hrmeid = 0;
                                    if (dd.hrmE_Id != undefined && dd.hrmE_Id > 0 && dd.hrmE_Id != null) {
                                        hrmeid = dd.hrmE_Id;
                                    }
                                    if ((dd.hrmE_Id !== undefined && dd.hrmE_Id !== null && dd.hrmE_Id !== "")) {

                                        $scope.editDetails.push({
                                            HRME_Id: hrmeid,
                                            MOMPRT_ParticipantsName: dd.Particularname,
                                            MOMPRT_MobileNo: dd.MobileNo,
                                            MOMPRT_Email: dd.EmailId,
                                            MOMPRT_Designation: dd.DesignationName,
                                            MOMPRT_EXTINTFlag: dd.radiomodal1
                                        });
                                    }
                                });
                            }
                        }
                        //if (rrr.MOMPRT_EXTINTFlag == "Internal") {
                        //    $scope.editDetails = [];
                        //    if ($scope.employeenameschange.length > 0) {
                        //        angular.forEach($scope.employeenameschange, function (dd) {                                   
                        //            if ((dd.MOMPRT_EXTINTFlag == "Internal" && dd.HRME_Id !== null && dd.HRME_Id !== "")) {
                        //                $scope.editDetails.push({
                        //                    HRME_Id: dd.HRME_Id,
                        //                    MOMPRT_ParticipantsName: dd.MOMPRT_ParticipantsName,
                        //                    MOMPRT_MobileNo: dd.MOMPRT_MobileNo,
                        //                    MOMPRT_Email: dd.MOMPRT_Email,
                        //                    MOMPRT_Designation: dd.MOMPRT_Designation,
                        //                    MOMPRT_EXTINTFlag: dd.MOMPRT_EXTINTFlag
                        //                });
                        //            }
                        //        });
                        //    }
                        //    else if ((rrr.MOMPRT_EXTINTFlag == "External")) {
                        //        $scope.newemp.push({
                        //            HRME_Id: rrr.HRME_Id,
                        //            MOMPRT_ParticipantsName: rrr.MOMPRT_ParticipantsName,
                        //            MOMPRT_MobileNo: rrr.MOMPRT_MobileNo,
                        //            MOMPRT_Email: rrr.MOMPRT_Email,
                        //            MOMPRT_Designation: rrr.MOMPRT_Designation,
                        //            MOMPRT_EXTINTFlag: rrr.MOMPRT_EXTINTFlag
                        //        });
                        //    }
                        //}
                        
                    });
                }




                $scope.uploadarray = [];
                if ($scope.documentListOtherDetails.length > 0) {
                    angular.forEach($scope.documentListOtherDetails, function (itm) {
                        if (itm.FilePath !== undefined && itm.FilePath !== null && itm.FilePath !== "") {
                            $scope.uploadarray.push({
                                FilePath: itm.FilePath,
                                FileName: itm.FileName
                            });
                        }
                    });
                }

                $scope.MOM_Agenda = document.getElementById("myP").innerHTML;                var obj = {
                    "MOM_Agenda": $scope.MOM_Agenda,
                    "MOM_Name": $scope.MOM_Name,
                    "MOM_ID": $scope.MOM_Id,
                    "MOM_Venue": $scope.MOM_Venue,
                    "MOM_Status": $scope.MOM_Status,
                    "MOM_StartTime": $filter('date')($scope.MOM_StartTime, "hh:mma"),
                    "MOM_EndTime": $filter('date')($scope.MOM_EndTime, "hh:mma"),
                    "MOM_Date": new Date($scope.MOM_Date).toDateString(),
                    "MI_Id": $scope.MI_Id,
                    "MOM_EXTINTFlag": $scope.radiomodal,
                    "Participent_Array": $scope.editDetails,//internal
                    "ParticipentsArray": $scope.newemp,//external
                    "FilePath_Array": $scope.uploadarray,
                };
                apiService.create("MOMUpdate/savetransaction/", obj).then(function (promise) {

                    if (promise.retrunMsg === 'Add') {
                        swal("Record Saved Successfully");
                        $state.reload();
                    }
                    else if (promise.retrunMsg === 'duplicate') {
                        swal("Record already exist");
                        $state.reload();
                    }
                    else if (promise.retrunMsg === "false") {
                        swal("Failed to save record");
                        $state.reload();
                    }
                    else {
                        swal("Email not sent");
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.minutesdetails = function (item) {
            $scope.MOM_Minutes = item.moM_Minutes
        };
        $scope.agendadetails = function (item) {
            $scope.moM_Agenda = item.moM_Agenda
            document.getElementById("ada").innerHTML = $scope.moM_Agenda;
        };


        //==============================
        $scope.get_employeedetails = function () {
            $scope.selectedemp = [];
            angular.forEach($scope.employeenameschange, function (uem) {
                angular.forEach(uem.staffData, function (rr) {
                    if (rr.empck) $scope.selectedemp.push({ hrmE_Id: rr.hrmE_Id, EmailId: rr.hrmE_EmailId, Particularname: rr.hrmE_EmployeeFirstName, DesignationName: rr.hrmdeS_DesignationName, MobileNo: rr.hrmE_MobileNo, radiomodal1: uem.radiomodal1 });
                });
            });
        }
        //===========================ADD files==================================
        $scope.documentListOtherDetails = [{ id: 'sell11' }];
        $scope.addNewDocumentOtherDetail = function () {
            var newItemNo = $scope.documentListOtherDetails.length + 1;
            if (newItemNo <= 100) {
                $scope.documentListOtherDetails.push({ 'id': 'sell11' + newItemNo });
            }
        };
        $scope.removeNewDocumentOtherDetail = function (index, data) {
            var newItemNo = $scope.documentListOtherDetails.length - 1;
            $scope.documentListOtherDetails.splice(index, 1);
            if (data.hrmE_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
        };
        $scope.selectFileforUploadzdOtherDetail = function (input, document) {
            $scope.ldr = true;
            $scope.SelectedFileForUploadzdOtherDetail = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetail(document);

            }
        };
        function UploadEmployeeDocumentOtherDetail(data) {
            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetail.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetail[i]);
            }
            // We can send more data to server using append         
            //formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_ISMAttachment_new", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {
                        data.FilePath = d[0].path;
                        data.FileName = d[0].name;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
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
        $scope.objdata = [];
        $scope.viewdocument = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.objdata = obj;

            apiService.create("MOMUpdate/viewflies", obj).then(function (promise) {
                if (promise !== null) {

                    $scope.getfiledetails = promise.getfiledetails;
                    $scope.staffData = promise.emplist;
                    if (promise.getfiledetails !== null && promise.getfiledetails.length > 0) {
                        $scope.uploaddocfiles = promise.getfiledetails;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.momfL_FilePath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.momfL_FilePath;
                            }
                        });
                    } else {

                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        //=====================  for alignment=============
        //$scope.store=function () {
        //    var txt = document.getElementById("text-area-first").value;

        //    var txttostore = '<p>' + txt.replace(/\n/g, "</p>\n<p>") + '</p>';

        //    console.log(txttostore);
        //}
        //==============================


        $scope.ViewDetails = function (user) {

            var data = {
                "MOM_Id": user.moM_Id
            };

            apiService.create("MOMUpdate/viewflies/", data).then(function (promise) {

                if (promise.getParticipentsdetails.length > 0 && promise.getParticipentsdetails !== null) {
                    $scope.getParticipentsdetails = promise.getParticipentsdetails;
                    $('#participants').modal('show');
                }
            });
        };
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //By Department 

        $scope.GetDetailsByDepartment = function (obj) {
            $scope.staffData = [];

            var data = {
                "MI_Id": $scope.mI_Id
            };

            apiService.create("MOMUpdate/viewflies", data).
                then(function (promise) {
                    if (promise.emplist != 0 && promise.emplist != null) {
                        //$scope.staffData = promise.emplist;        

                        angular.forEach($scope.employeenameschange, function (itm) {
                            itm.staffData = promise.emplist;
                        });



                    }
                })


        }
        ///////////////////////////////////////////////////////////////



        $scope.get_employeedetails = function () {
            $scope.selectedemp = [];
            angular.forEach($scope.employeenameschange, function (uem) {
                angular.forEach(uem.staffData, function (rr) {
                    if (rr.empck) $scope.selectedemp.push({ hrmE_Id: rr.hrmE_Id, EmailId: rr.hrmE_EmailId, Particularname: rr.hrmE_EmployeeFirstName, DesignationName: rr.hrmdeS_DesignationName, MobileNo: rr.hrmE_MobileNo, radiomodal1: uem.radiomodal1 });
                });
            });
        }

        ////////////////////////////////////////////////////////////////////
        // ============================ng_click function for employee (ALL)========================//$scope.employeenameschange[0].staffData

        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.employeenameschange, function (uem) {
                angular.forEach(uem.staffData, function (staff) {
                    if (uem.usercheckC == toggleStatus) {
                        staff.empck = toggleStatus;
                    }
                });
            });
            $scope.get_employeedetails();
        };
        //$scope.togchkbx = function () {
        //    $scope.checkall = $scope.employeenameschange[0].staffData.every(function (options) {
        //        return options.empck;
        //    });
        //    $scope.get_employeedetails();
        //};

        $scope.togchkbx = function (emp) {
            $scope.checkall = emp;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.employeenameschange, function (uem) {
                angular.forEach(uem.staffData, function (options) {
                    if (options.empck == toggleStatus) {
                        return options.empck;
                    }
                });
            });

            $scope.get_employeedetails();
        };



        //$scope.isOptionsRequired = function () {
        //    return !$scope.employeenameschange[0].staffData.some(function (options) {
        //        return options.empck;
        //    });
        //};

        $scope.isOptionsRequired = function () {

            for (var i = 0; i <= $scope.employeenameschange.length; i++) {

                return !$scope.employeenameschange[i].staffData.some(function (options) {
                    return options.empck;
                });
            }
        };

        ///==================edit/===================//////////////////////////////////////////////////////////////////////
        $scope.Edit = function (item) {
            $scope.editflag = true;
          

            var data = {
                "MOM_Id": item.moM_Id,
            };        
      
            apiService.create("MOMUpdate/Edittrans", data).then(function (promise) {
                if (promise !== null) {
                    $scope.edittransdata = promise.edittransdata;
                    $scope.editempdata = promise.editempdata;
                    $scope.editemployeedata = promise.approvedata;
                    $scope.editflag = true;
                    $scope.radiomodal = promise.edittransdata[0].moM_EXTINTFlag;
                    $scope.MOM_Date = new Date(promise.edittransdata[0].moM_Date);
                    $scope.MOM_Agenda = promise.edittransdata[0].moM_Agenda;
                    $scope.MOM_Venue = promise.edittransdata[0].moM_Venue;
                    $scope.meetingmodal = "meeting1";
                    $scope.MOM_Name = promise.edittransdata[0].moM_Name;
                    $scope.MOM_Id = promise.edittransdata[0].moM_Id;
                    $scope.MOM_StartTime = moment(promise.edittransdata[0].moM_StartTime, 'h:mm a').format();
                    $scope.MOM_EndTime = moment(promise.edittransdata[0].moM_EndTime, 'h:mm a').format();
                    $scope.IVRMSTAUL_UserName = promise.approvedata[0].ivrmstauL_UserName;
                    document.getElementById("myP").innerHTML = $scope.MOM_Agenda;
                    $scope.employeenameschange = [];
                    angular.forEach($scope.editempdata, function (rr) {
                        $scope.employeenameschange.push({
                            HRME_Id: rr.hrmE_Id,
                            MOMPRT_ParticipantsName: rr.momprT_ParticipantsName,
                            MOMPRT_MobileNo: rr.momprT_MobileNo,
                            MOMPRT_Email: rr.momprT_Email,
                            MOMPRT_Designation: rr.momprT_Designation,
                            MOMPRT_EXTINTFlag: rr.momprT_EXTINTFlag
                        });                        
                    });

                }
            })




        }


        $scope.isAnyExternal = function () {
            return $scope.employeenameschange.some(function (other) {
                return other.radiomodal1 === 'External';
            });
        };
    }
})();