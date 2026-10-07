(function () {
    'use strict';
    angular.module('app').controller('MOMApprovalController', MOMApprovalController)

    MOMApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache', 'myFactoryvisitor', '$q', '$sce']
    function MOMApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache, myFactoryvisitor, $q, $sce) {

        var paginationformasters;
        $scope.multidepartment = false;
        $scope.milogo = "";
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            $scope.milogo = ivrmcofigsettings[0].asC_Logo_Path;
        }
        else {
            paginationformasters = 0;
        }
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 10;
        $scope.loadData = function () {

            var pageid = 2;
            apiService.getURI("MOMUpdate/getdata", pageid).then(function (promise) {
                $scope.getapproval = promise.getapproval;
                $scope.getapproveddata = promise.getapproveddata;
            })
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.minutesdetails = function (item) {
            $scope.MOM_Minutes = item.moM_Minutes
        };
        $scope.agendadetailssold = function (item) {
            $scope.moM_Agenda = item.moM_Agenda
            document.getElementById("adaa").innerHTML = $scope.moM_Agenda;
        };
        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printDeviation").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
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

        $scope.statusupdate = function (item) {
            if ($scope.myForm.$valid) {

                var data = {
                    "MOM_Id": item.moM_Id,
                    "MOM_ParentId": item.moM_ParentId,

                };
                apiService.create("MOMUpdate/approvaldata", data).then(function (promise) {

                    if (promise.approvaldata.length > 0) {
                        $scope.temparraynew = promise.approvaldata;
                        $scope.meetname = promise.approvaldata[0].MOM_Name;
                        $scope.meetdate = promise.approvaldata[0].MOM_Date;
                        $scope.meettime = promise.approvaldata[0].MOM_StartTime;
                        $scope.meetVenue = promise.approvaldata[0].Venue;
                        $scope.activeflag = promise.approvaldata[0].activeflag;
                        $scope.MOM_ParentId = promise.approvaldata[0].MOM_ParentId;
                        // $scope.len = temparraynew.length;






                        $scope.employeeid = [];

                        angular.forEach($scope.temparraynew, function (dev) {
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
                            angular.forEach($scope.temparraynew, function (dd) {
                                if (dd.MOMMNT_Names === ddd.MOMMNT_Names && dd.activeflag === null) {
                                    $scope.templist.push(dd);
                                }
                            });
                            ddd.plannerdetails = $scope.templist;
                        });






                    }
                    else {
                        swal("No Record  Found..... !!");

                    }
                    //// $scope.absentdata = promise.absentdata;

                    

                    //$scope.approvaldata = promise.approvaldata;
                    $scope.empabsent = [];
                    $scope.emppresent = [];
                    $scope.approvaldata = [];
                    if (promise.approvaldata != null) {
                        for (var i = 0; i < promise.approvaldata.length; i++) {
                            var dublicatdublicateabsent = [];
                            var dublicatdublicateabsent = $filter('filter')($scope.approvaldata, function (d) {
                                return d.ParticipantsName === promise.approvaldata[i].ParticipantsName;//&& d.activeflag == promise.approvaldata[i].activeflag;
                            });
                            if (dublicatdublicateabsent.length == 0) {
                                $scope.approvaldata.push(promise.approvaldata[i]);


                                if (promise.approvaldata[i].activeflag === false) {

                                    $scope.empabsent.push({ aname: promise.approvaldata[i].ParticipantsName, activeflag: promise.approvaldata[i].activeflag })

                                }
                                else if (promise.approvaldata[i].activeflag === true)  {

                                    $scope.emppresent.push({ pname: promise.approvaldata[i].ParticipantsName, activeflag: promise.approvaldata[i].activeflag })
                                }

                            }
                        }
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.agendadetails = function (item) {
            $scope.moM_Agenda = item.moM_Agenda
            document.getElementById("ada").innerHTML = $scope.moM_Agenda;
        };
        $scope.clear = function () {
            $state.reload();
        };
        $scope.OldmeetingDetails = function () {
            if ($scope.myForm.$valid) {

                var data = {
                    "MOM_ParentId": $scope.MOM_ParentId,
                    "MOM_Id": $scope.MOM_Id

                };
                apiService.create("MOMUpdate/oldmeeting", data).then(function (promise) {

                    if (promise.oldmeetdetail.length > 0) {
                        $scope.oldmeetdetail = promise.oldmeetdetail;
                        $('#perviousmeetingdetails').modal('show');
                    }
                    else {
                        swal("No Record  Found..... !!");
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.get_status = function () {
            if ($scope.myForm.$valid) {
                var Template = document.getElementById("momexcel").innerHTML;
                var data = {
                    Template: Template,
                    "MOM_Id": $scope.temparraynew[0].MOM_Id
                }
                apiService.create("MOMUpdate/getstatus", data).then(function (promise) {

                    if (promise.retrunMsg == "Update") {
                        swal('Record saved successfully');
                        $state.reload();
                    }
                    else if (promise.retrunMsg == "false") {
                        swal('Record  Not saved successfully');
                    }
                })
            }
            else {
                $scope.submitted = true;
            }

        };

        $scope.ViewDetails = function (user) {

            var data = {
                "MOM_Id": user.moM_Id
            };

            apiService.create("MOMUpdate/viewuploadflies/", data).then(function (promise) {

                if (promise.getParticipentsdetails.length > 0 && promise.getParticipentsdetails !== null) {
                    $scope.getParticipentsdetails = promise.getParticipentsdetails;
                    $('#participants').modal('show');


                }
            });
        };


        //MOMApproval emp
        //$scope.SendMail = function (momapproval) {
        //    var Template = document.getElementById("momexcel").innerHTML;

        //    if ($scope.myForm.$valid) {
        //        var data = {
        //            Template: Template,
        //            "MOM_Id": $scope.temparraynew[0].MOM_Id,
        //        };
        //        apiService.create("MOMUpdate/sendapprovalMail", data).
        //            then(function (promise) {
        //                if (promise.retrunMsg == "Add") {
        //                    swal('Email Sent Successfully.');
        //                    $state.reload();
        //                }
        //                else if (promise.retrunMsg == "Exist") {
        //                    swal('Email not Sent.');
        //                }
        //                else {
        //                    swal('Email Failed!!');
        //                }
        //            });
        //    }
        //    else { $scope.submitted = true; }
        //};

    }
})();
