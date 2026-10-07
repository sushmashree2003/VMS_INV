(function () {
    'use strict';
    angular.module('app').controller('MOMUpdateController', MOMUpdateController)

    MOMUpdateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache', 'myFactoryvisitor', '$q', '$sce']
    function MOMUpdateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache, myFactoryvisitor, $q, $sce) {

        $scope.showgrd = false;
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.search = "";
        $scope.searchemp = "";
        $scope.search1 = "";
        $scope.currentPage = 1;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage = 15;
        $scope.itemsPerPage1 = 10;
        $scope.itemsPerPage11 = 5;
        $scope.currentPage11 = 1;
        //previous date block
        $scope.entry_date = new Date();
        $scope.entry_date.setDate($scope.entry_date.getDate());
        $scope.obj = {};
        $scope.excel = false;
        var vm = this;
        vm.gridOptions = {};

        //====== loadadata 
        $scope.loadgrid = function () {
            var pageid = 2;
            apiService.getURI("MOMUpdate/getDetails/", pageid).then(function (promise) {
                $scope.aprovedlist = promise.aprovedlist;
                $scope.griddata = promise.griddata;
                $scope.emplist = promise.emplist;
                $scope.empdata = promise.empdata;
            });
        };
        //promise.empdata[0].hrmE_EmployeeLastName
        //$scope.empdata.hrmE_Designation
        $scope.minutesdetails = function (item) {
            $scope.MOM_Minutes = item.moM_Minutes
        };
        $scope.agendadetails = function (item) {
            $scope.moM_Agenda = item.moM_Agenda
            document.getElementById("ada").innerHTML = $scope.moM_Agenda;
        };
        //===========================ADD==================================
        $scope.employeenameschange = [{ id: 'sell1' }];
        $scope.addNewDetail = function () {
            var newItemNo = $scope.employeenameschange.length + 1;
            if (newItemNo <= 1030) {
                $scope.employeenameschange.push({ 'id': 'sell1' + newItemNo });
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
        //===========================ADD files==================================
        $scope.documentListOtherDetails = [{ id: 'sell11' }];
        $scope.addNewDocumentOtherDetail = function () {
            var newItemNo = $scope.documentListOtherDetails.length + 1;
            if (newItemNo <= 1030) {
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



        $scope.exptoex = function () {

            // window.open("https://dcampusstrg.blob.core.windows.net/files/17/ISM_Attachments/e7ec5bd7-eac2-494b-92ff-23a98194b12b.xlsx");
            window.open("https://dcampusstrg.blob.core.windows.net/files/17/Trnsportdocuments/3f0f96c8-835b-48ae-9cd1-1360b39272a2.xlsx");
        };



        //==============================
        $scope.visitorlist = function (obj, hrmE_Id) {
            obj.MOMPRT_ParticipantsName = "";
            obj.MOMPRT_Designation = "";
            obj.MOMPRT_Email = "";
            obj.MOMPRT_MobileNo = "";
            angular.forEach($scope.emplist, function (rr) {
                if (obj.hrmE_Id.hrmE_Id == rr.hrmE_Id) {
                    obj.MOMPRT_ParticipantsName = rr.hrmE_EmployeeFirstName;
                    obj.MOMPRT_Designation = rr.hrmdeS_DesignationName;
                    obj.MOMPRT_Email = rr.hrmE_EmailId;
                    obj.MOMPRT_MobileNo = rr.hrmE_MobileNo;
                }
            })
        }

        //============================================update=======================================//////////////////////////////////////////
        $scope.allvisitor = [];
        $scope.update = function (obj) {
            $scope.MOM_Date = "";
            $scope.MOM_StartTime = "";
            $scope.MOM_EndTime = "";
            $scope.MOM_Agenda = obj.MOM_Agenda;
            $scope.MOM_Venue = obj.MOM_Venue;
            $scope.MOM_Status = obj.MOM_Status;
            $scope.MOM_Id = obj.MOM_Id;
            $scope.MI_Id = obj.MI_Id;
            $scope.HRME_Id = obj.HRME_Id;
            $scope.MOM_StartTime = obj.MOM_StartTime;
            $scope.MOM_EndTime = obj.MOM_EndTime;
            $scope.MOM_Date = new Date(obj.MOM_Date);
            $scope.MOM_StartTime = moment(obj.MOM_StartTime, 'h:mm a').format();
            $scope.MOM_EndTime = moment(obj.MOM_EndTime, 'h:mm a').format();

            document.getElementById("van").innerHTML = $scope.MOM_Agenda;

            apiService.create("MOMUpdate/UpdateDetails/", obj).then(function (promise) {

                $scope.appliedlist = promise.appliedlist;
                $scope.emplist = promise.emplist;
                $scope.updateDetails = promise.updateDetails;
                $scope.getfiledetails = promise.getfiledetails;
                $scope.allvisitor = promise.updateDetails;

                angular.forEach($scope.visitorlist, function (rr) {

                    $scope.allvisitor.push({
                        HRME_Id: rr.hrmE_Id,
                        MOMPRT_ParticipantsName: rr.MOMPRT_ParticipantsName,
                        MOMPRT_MobileNo: rr.MOMPRT_MobileNo,
                        MOMPRT_Email: rr.MOMPRT_Email,
                        MOMPRT_Designation: rr.MOMPRT_Designation,
                        MOMPRT_EXTINTFlag: rr.MOMPRT_EXTINTFlag
                    })

                })

            })
            angular.forEach($scope.employeenameschange, function (rr) {

                $scope.allvisitor.push({
                    HRME_Id: rr.hrmE_Id,
                    MOMPRT_ParticipantsName: rr.MOMPRT_ParticipantsName,
                    MOMPRT_MobileNo: rr.MOMPRT_MobileNo,
                    MOMPRT_Email: rr.MOMPRT_Email,
                    MOMPRT_Designation: rr.MOMPRT_Designation,
                    MOMPRT_EXTINTFlag: rr.MOMPRT_EXTINTFlag,

                })

                $scope.showgrd = true;
            });
        };
        ///==================edit/===================//////////////////////////////////////////////////////////////////////
        $scope.Edit = function (item) {
            $scope.excel = true;
            var data = {
                "MOM_Id": item.moM_Id,
            };

            apiService.create("MOMUpdate/Edit", data).then(function (promise) {
                if (promise !== null) {

                    $scope.appliedlist = promise.appliedlist;
                    $scope.updateDetails = promise.editupdateDetails;
                    $scope.editflag = true;
                    $scope.MOMMNT_TargetDate = new Date(promise.editupdateDetails[0].mommnT_TargetDate);
                    $scope.MOM_Date = new Date(promise.appliedlist[0].moM_Date);
                    $scope.MOM_Agenda = promise.appliedlist[0].moM_Agenda;
                    $scope.MOM_Venue = promise.appliedlist[0].moM_Venue;
                    $scope.MOM_Minutes = promise.appliedlist[0].moM_Minutes;
                    $scope.MOM_Id = promise.appliedlist[0].moM_Id;
                    $scope.MOM_Name = promise.appliedlist[0].moM_Name;
                    $scope.MOM_StartTime = moment(promise.appliedlist[0].moM_StartTime, 'h:mm a').format();
                    $scope.MOM_EndTime = moment(promise.appliedlist[0].moM_EndTime, 'h:mm a').format();

                    document.getElementById("van").innerHTML = $scope.MOM_Agenda;



                    //angular.forEach($scope.updateDetails, function (dddd) {

                    //    $scope.updateDetails.push({

                    //        HRME_Id: dddd.hrmE_Id,
                    //        MOMPRT_ParticipantsName: dddd.momprT_ParticipantsName,
                    //        MOMPRT_MobileNo: dddd.momprT_MobileNo,
                    //        MOMPRT_Email: dddd.momprT_Email,
                    //        MOMPRT_Designation: dddd.momprT_Designation,
                    //        MOMPRT_EXTINTFlag: dddd.momprT_EXTINTFlag,
                    //        MOMMNT_TargetDate: new Date(dddd.mommnT_TargetDate),
                    //        MOMMNT_ActionPlans: dddd.mommnT_ActionPlans,
                    //        MOMMNT_Names: dddd.mommnT_Names,
                    //    })


                    //})


                    $scope.updateDetails = $filter('filter')($scope.updateDetails, function (d) {
                        d.MOMMNT_TargetDate = new Date(d.MOMMNT_TargetDate)
                        return d
                    });


                    $scope.emplist = promise.emplist;
                    //$scope.allvisitor = promise.updateDetails;
                    //angular.forEach($scope.emplist, function (rr) {

                    //    $scope.employeenameschange.push({
                    //        HRME_Id: rr.hrmE_Id,
                    //        MOMPRT_ParticipantsName: rr.MOMPRT_ParticipantsName,
                    //        MOMPRT_MobileNo: rr.MOMPRT_MobileNo,
                    //        MOMPRT_Email: rr.MOMPRT_Email,
                    //        MOMPRT_Designation: rr.MOMPRT_Designation,
                    //        MOMPRT_EXTINTFlag: rr.MOMPRT_EXTINTFlag,

                    //    })
                    //})

                }
            })
        }




        //////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\


        $scope.validatemax = function (maxdata) {

            var dsttimee = $scope.MOM_StartTime;
            $scope.sresult = $filter('date')(dsttimee, 'HH:mm:ss a');
            $scope.eresult = $filter('date')(maxdata, 'HH:mm:ss a');
            var startTime = moment($scope.sresult, "HH:mm:ss a");
            var endTime = moment($scope.eresult, "HH:mm:ss a");
            var duration = moment.duration(endTime.diff(startTime));
            var hours = parseInt(duration.asHours());
            var minutes = parseInt(duration.asMinutes()) - hours * 60;
            var finlrst = hours + ":" + minutes;

            $scope.tmin = new Date($scope.today);
            $scope.tmin.setHours(hours);
            $scope.tmin.setMinutes(minutes);
            $scope.tmax = new Date($scope.today);
            $scope.tmax.setHours(hours);
            $scope.tmax.setMinutes(minutes);

            $scope.ttst = new Date($scope.today);
            $scope.ttst.setHours(hours);
            $scope.ttst.setMinutes(minutes);

            $scope.htmax = new Date($scope.today);
            $scope.htmax.setHours(hours);
            $scope.htmax.setMinutes(minutes);

            if (maxdata >= new Date($scope.MOM_StartTime)) {
                $scope.totimemax = maxdata;
                var hh = $scope.totimemax.getHours();
                var mm = $scope.totimemax.getMinutes();
                $scope.max = maxdata;

                $scope.max.setMinutes(hh);
                $scope.max.setMinutes(mm);
            }
            else {
                $scope.MOM_EndTime = 0;
            }
        };
        $scope.validateTomintime = function (timedata) {

            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;
            $scope.min.setHours(hh);
            $scope.min.setMinutes(mm);
            $scope.minlnc = timedata;
            $scope.minlnc.setHours(hh);
            $scope.minlnc.setMinutes(mm);
            $scope.MOM_EndTime = 0;
        };
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.mindate = new Date();
        $scope.submitted = false;
        $scope.submitted1 = false;

        $scope.deactive = function (obj) {
            var data = {
                "MOM_Id": obj.MOM_Id,
                "MOMPRT_Id": obj.MOMPRT_Id,
                "MOMPRT_ActiveFlag": obj.MOMPRT_ActiveFlag,
            };
            apiService.create("MOMUpdate/approvaldata", data).then(function (promise) {
                if (promise.retrunMsg === 'true') {

                    angular.forEach($scope.updateDetails, function (rrr) {
                        if (rrr.MOMPRT_Id === obj.MOMPRT_Id) {
                            if (rrr.MOMPRT_ActiveFlag == true) {
                                rrr.MOMPRT_ActiveFlag = false;
                            }
                            else {
                                rrr.MOMPRT_ActiveFlag = true;
                            }


                            swal("Attendance added successfully.");
                        }

                    });


                }
                else {
                    swal("Failed to add attendance.");
                }
            });
        };

        ///SAve//////edit&save///////////////////////////////////////////////////////////////
        $scope.saveData = function () {

            if ($scope.myForm.$valid) {


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


                if ($scope.excel1 == true) {
                    var valu = $scope.vm.gridOptions.data;
                    $scope.albumNameArray = [];
                    if (valu.length > 0) {
                        $scope.albumNameArray = [];

                        for (var i = 0; i < valu.length; i++) {
                            $scope.albumNameArray.push(valu[i]);
                        }

                    }
                }
                else {
                    if ($scope.myForm12.$valid) {

                        $scope.emppdetail = [];
                        if ($scope.updateDetails.length > 0) {
                            angular.forEach($scope.updateDetails, function (itm) {
                                if (itm.MOMPRT_Id !== undefined && itm.MOMPRT_Id !== null && itm.MOMPRT_Id !== "") {
                                    $scope.emppdetail.push({
                                        HRME_Id: itm.HRME_Id,
                                        MOM_Id: itm.MOM_Id,
                                        MOMPRT_Id: itm.MOMPRT_Id,
                                        MOMPRT_ParticipantsName: itm.MOMPRT_ParticipantsName,
                                        MOMPRT_MobileNo: itm.MOMPRT_MobileNo,
                                        MOMPRT_Email: itm.MOMPRT_Email,
                                        MOMPRT_Designation: itm.MOMPRT_Designation,
                                        MOMPRT_EXTINTFlag: itm.MOMPRT_EXTINTFlag,
                                        MOMMNT_Names: itm.MOMMNT_Names,
                                        MOMMNT_ActionPlans: itm.MOMMNT_ActionPlans,
                                        MOMMNT_TargetDate: itm.MOMMNT_TargetDate,
                                        MOMMNT_Id: itm.MOMMNT_Id,
                                    });

                                }
                                //else {
                                //    MOMMNT_Names: itm.MOMMNT_Names,
                                //        MOMMNT_ActionPlans: itm.MOMMNT_ActionPlans,
                                //            MOMMNT_TargetDate: itm.MOMMNT_TargetDate,
                                //}
                            });
                        }

                        $scope.empdetail = [];

                        if ($scope.employeenameschange.length > 0) {
                            angular.forEach($scope.employeenameschange, function (rr) {
                                var hrmeid = 0;
                                if (rr.hrmE_Id != undefined && rr.hrmE_Id.hrmE_Id > 0 && rr.hrmE_Id.hrmE_Id != null) {
                                    hrmeid = rr.hrmE_Id.hrmE_Id;
                                }
                                else {
                                    hrmeid = 0;
                                }
                                if ((rr.MOMPRT_ParticipantsName !== undefined && rr.MOMPRT_ParticipantsName !== null && rr.MOMPRT_ParticipantsName !== "") || hrmeid > 0) {
                                    $scope.empdetail.push({
                                        HRME_Id: hrmeid,
                                        MOM_Id: rr.moM_Id,
                                        MOMPRT_Id: rr.momprT_Id,
                                        MOMPRT_ParticipantsName: rr.MOMPRT_ParticipantsName,
                                        MOMPRT_MobileNo: rr.MOMPRT_MobileNo,
                                        MOMPRT_Email: rr.MOMPRT_Email,
                                        MOMPRT_Designation: rr.MOMPRT_Designation,
                                        MOMPRT_EXTINTFlag: rr.radiomodal1,
                                        MOMMNT_Names: rr.MOMMNT_Names,
                                        MOMMNT_ActionPlans: rr.MOMMNT_ActionPlans,
                                        MOMMNT_TargetDate: rr.MOMMNT_TargetDate,
                                        MOMMNT_Id: rr.mommnT_Id,
                                        //MOMPRT_ParticipantsName: rr.MOMPRT_ParticipantsName,
                                        //MOMPRT_MobileNo: rr.MOMPRT_MobileNo,
                                        //MOMPRT_Email: rr.MOMPRT_Email,
                                        //MOMPRT_Designation: rr.MOMPRT_Designation,
                                        //MOMPRT_EXTINTFlag: rr.radiomodal1,
                                        //MOMMNT_Names: rr.MOMMNT_Names,
                                        //MOMMNT_ActionPlans: rr.MOMMNT_ActionPlans,
                                        //MOMMNT_TargetDate: rr.MOMMNT_TargetDate,
                                        //MOMMNT_Id: rr.mommnT_Id,
                                    });
                                }

                            });
                        }

                        //$scope.mommnT_ActionPlans = "";
                        // $scope.mommnT_Names = "";
                    }
                    else {
                        swal("Kindly fill mandatory field Task/Group");
                        return;
                    }
                }




                var data = {
                    "MOM_Id": $scope.MOM_Id,
                    "MOM_Status": $scope.MOM_Status,
                    "MOM_StartTime": $filter('date')($scope.MOM_StartTime, "HH:mma"),
                    "MOM_Date": $scope.MOM_Date,
                    "MOM_Minutes": $scope.MOM_Minutes,
                    "MOM_Flag": $scope.MOM_Flag,
                    "MI_Id": $scope.MI_Id,
                    "MOM_EndTime": $filter('date')($scope.MOM_EndTime, "HH:mma"),
                    "Participent_Array": $scope.empdetail,  //newParticipants
                    "ParticipentsArray": $scope.emppdetail, //ExisitsParticipants from trans
                    "FilePath_Array": $scope.uploadarray,
                    "newlstget1": $scope.albumNameArray
                };

                apiService.create("MOMUpdate/saveData", data).then(function (promise) {

                    if (promise.retrunMsg === "Update") {
                        swal("Record Saved Successfully");
                        $state.reload();
                    }

                    else if (promise.retrunMsg === "duplicate") {
                        swal("Record already exist");
                        $state.reload();
                    }
                    else if (promise.retrunMsg === "false") {
                        swal("Failed to save record");
                    }
                    else {
                        swal("Kindly contact administrator for more information");
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

        $scope.interacted12 = function (field) {
            return $scope.submitted;
        };
        $scope.interacted21 = function (field) {
            return $scope.submitted;
        };

        // /////////////////////import to excel///////////////////////////

        $scope.excel1 = false;
        $scope.excelhide1 = function () {
            $scope.excel1 = true;
        }
        $scope.excel = false;
        $scope.excelhide = function () {
            $scope.excel = true;
        }
        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input) {

            $scope.SelectedFileForUploadzd = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {

                    //once return success 
                    var reader = new FileReader();
                    reader.readAsDataURL(input.files[0]);

                }
                else {
                    swal("Please Upload the .xlsx file");
                    return;
                }
            }

        }
        $rootScope.validateForm = function () {

            console.log($scope.opts.data);
            var data1 = {
                "newlstget1": $scope.opts.data
            }
            apiService.create("MOMUpdate/checkvalidation", data1).then(function (promise) {
                if (promise.stuStatus != "" && promise.stuStatus != null) {
                    swal(promise.stuStatus);
                    $elm.val(null);
                    $scope.opts.data = null;
                } else {
                    $scope.opts.data = $scope.opts.data;
                }
            });
        }


        // /////////////////////import to excel///////end////////////////////
        $scope.objdata = [];
        $scope.viewdocument = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.objdata = obj;

            apiService.create("MOMUpdate/viewuploadflies", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.getfiledetails = promise.getfiledetails;
                    if (promise.getfiledetails !== null && promise.getfiledetails.length > 0) {
                        $scope.uploaddocfiles = promise.getfiledetails;
                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            if (dd.momfL_FilePath !== null) {
                                var img = dd.momfL_FilePath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.momfL_FilePath;
                                }
                            }

                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        $scope.ViewDetails = function (user) {

            var data = {
                "MOM_Id": user.moM_Id,
            };

            apiService.create("MOMUpdate/viewuploadflies/", data).then(function (promise) {

                if (promise.getParticipentsdetails.length > 0 && promise.getParticipentsdetails !== null) {
                    $scope.getParticipentsdetails = promise.getParticipentsdetails;

                    $scope.employeeid = [];

                    angular.forEach($scope.getParticipentsdetails, function (dev) {
                        if ($scope.employeeid.length === 0) {
                            $scope.employeeid.push({
                                MOMMNT_Id: dev.MOMMNT_Id,
                                MOMMNT_Names: dev.MOMMNT_Names
                            });
                        } else if ($scope.employeeid.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.employeeid, function (emp) {
                                if (emp.MOMMNT_Names === dev.MOMMNT_Names) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.employeeid.push({
                                    MOMMNT_Id: dev.MOMMNT_Id, MOMMNT_Names: dev.MOMMNT_Names,

                                });
                            }
                        }
                    });
                    angular.forEach($scope.employeeid, function (ddd) {
                        $scope.templist = [];
                        var sum_deviation = 0;
                        angular.forEach($scope.getParticipentsdetails, function (dd) {
                            if (dd.MOMMNT_Names === ddd.MOMMNT_Names) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.plannerdetails = $scope.templist;
                    });
                    $('#participants').modal('show');


                }
            });
        };


        //////end////

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



angular
    .module('app').filter('keys', function () {

        return function (input) {
            if (!input) {
                return [];
            }
            delete input.$$hashKey;
            return Object.keys(input);
        }

    })

angular
    .module('app').directive("fileread", ['$rootScope', 'apiService', function ($rootScope, apiService) {

        return {
            scope: {
                opts: '='

            },
            link: function ($scope, $elm, $attrs) {

                $elm.on('change', function (changeEvent) {

                    var reader = new FileReader();

                    reader.onload = function (evt) {
                        $scope.$apply(function () {
                            var data = evt.target.result;

                            var workbook = XLSX.read(data, { type: 'binary' });

                            var headerNames = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]], { header: 1 })[0];

                            var data = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]]);

                            if (data.length == 0) {

                                swal("Excel Sheet is Empty");
                                $elm.val(null);
                                $scope.opts.data = null;
                                return;

                            }
                            else {
                                $scope.opts = {};
                                $scope.opts.columnDefs = [];
                                headerNames.forEach(function (h) {
                                    $scope.opts.columnDefs.push({ field: h });
                                });
                                //dfsd
                                $scope.opts.data = data;
                                // $rootScope.validateForm();

                                console.log($scope.opts.data);
                                var data1 = {
                                    "newlstget1": $scope.opts.data
                                }
                                apiService.create("MOMUpdate/checkvalidation", data1).then(function (promise) {
                                    if (promise.stuStatus != "" && promise.stuStatus != null) {
                                        swal(promise.stuStatus);
                                        $elm.val(null);
                                        $scope.opts.data = null;
                                    } else {
                                        $scope.opts.data = $scope.opts.data;
                                    }
                                });


                                //$elm.val(null);
                            }
                        });
                    };
                    reader.readAsBinaryString(changeEvent.target.files[0]);

                });
            }
        }
    }]);