(function () {
    'use strict';

    angular
        .module('app')
        .controller('IEMonthEndSubmissionAndApprovalReportController', IEMonthEndSubmissionAndApprovalReportController);

    IEMonthEndSubmissionAndApprovalReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache' ,'Excel', '$timeout', '$filter', '$q']

    function IEMonthEndSubmissionAndApprovalReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter, $q) {

        $scope.takescreenshot = false;
        $scope.hrcommentflag = false;
        $scope.disablemonth = false;
        $scope.ddate = new Date();
    
        $scope.submitted = false;
       
        $scope.loadData = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;

            $scope.all_checkC = function () {
                var checkStatus = $scope.ismmclT_Id;
                var count = 0;
                angular.forEach($scope.intistutename, function (itm) {
                    itm.selected = checkStatus;
                    if (itm.selected == true) {
                        count += 1;
                    }
                    else {
                        count = 0;
                    }
                });

            }

            $scope.all_checkC4 = function () {
                var checkStatus = $scope.ivrM_Month_Id;
                var count = 0;
                angular.forEach($scope.fillmonth, function (itm) {
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
                return !$scope.intistutename.some(function (options) {
                    return options.selected;
                });


            }

            $scope.isOptionsRequired4 = function () {
                return !$scope.fillmonth.some(function (options) {
                    return options.selected;
                });
            }

            $scope.togchkbxC = function () {

                $scope.ivrM_Month_Id = $scope.fillmonth.every(function (options) {
                    return options.selected;
                });
            }

            $scope.togchkbxC1 = function () {

                $scope.ismmclT_Id = $scope.intistutename.every(function (options) {
                    return options.selected;
                });
            }
  
            apiService.getURI("IEMonthEndSubmissionAndApprovalReport/onloaddata", pageid).then(function (promise) {
                $scope.fillmonth = promise.fillmonth;
                $scope.fillyear = promise.fillyear;
                $scope.intistutename = promise.intistutename;
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clear_Details = function () {
            $state.reload();
        };

        $scope.getIEDetails = function() {
            var data = {
                "ISMMCLT_Id":$scope.ismmclT_Id,
            };
            apiService.create("IEMonthEndSubmissionAndApprovalReport/getIEDetails", data).
                then(function (promise) {
                    if (promise.ielist.length > 0 ) {
                        $scope.ielist = promise.ielist;
                    }
                    else {
                        swal("No Record Found")
                    }

                })
        };

        $scope.ShowReportdata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var MONTHID = [];
                var CLIENTID = [];
                if ($scope.allind == "IE") {
                    angular.forEach($scope.fillmonth, function (ty) {
                        if (ty.selected) {
                            MONTHID.push({ IVRM_Month_Id: ty.ivrM_Month_Id });
                        }
                    })
                }
                else {
                    MONTHID.push({ IVRM_Month_Id: $scope.ivrM_Month_Id });
                }

                if ($scope.allind == "Client") {
                    $scope.IE_Id = 0;
                    angular.forEach($scope.fillmonth, function (ty) {
                        if (ty.selected) {
                            MONTHID.push({ IVRM_Month_Id: ty.ivrM_Month_Id });
                        }
                    })
                }
               
                if ($scope.allind == "All") {
                   $scope.IE_Id = 0;
                    angular.forEach($scope.intistutename, function (ty) {
                        if (ty.selected) {
                            CLIENTID.push({ ISMMCLT_Id: ty.ismmclT_Id });
                        }
                    })
                }
                else {
                    CLIENTID.push({ ISMMCLT_Id: $scope.ismmclT_Id });
                }
               
                    var data = {
                        "CLIENTID": CLIENTID,
                        "HRME_Id": $scope.IE_Id,
                        "yearid": $scope.ASMAY_Id,
                        "MONTHID": MONTHID,
                        "rdbbutton": $scope.allind
                    };
                

                apiService.create("IEMonthEndSubmissionAndApprovalReport/getmonthreport", data).then(function (promise) {
                    if (promise.iemonthendReport.length > 0) {
                        $scope.iemonthendReport = promise.iemonthendReport;
                        $scope.presentCountgrid = $scope.iemonthendReport.length;
                    }
                    else {
                        swal("No Records Found");
                    }
                });
            }
            
        };

        $scope.ApprovalView = function (indl) {
            var data = {
                "ISMCLMER_Id": indl.ISMCLMER_Id,
            };
            apiService.create("IEMonthEndSubmissionAndApprovalReport/DocView", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.multidocView != null) {
                        $('#MultiDocumentView').modal('show');
                        $scope.multidocView = promise.multidocView;
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
            if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {

                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');

            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
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

        $scope.printData = function () {
            var innerContents = document.getElementById("printDeviation").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function (export_id) {
            var exportHref = Excel.tableToExcel(export_id, 'printDeviation');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };

        $scope.exporttoexcelpopup = function () {
            swal({
                title: "Records Updated Successfully.!!",
                text: "Do you want to export data to excel?",
                type: "success",
                showCancelButton: true,
                confirmButtonColor: "green", confirmButtonText: "Yes, export excel!",
                cancelButtonText: "No",
                closeOnConfirm: true,
                closeOnCancel: true
            },
                function (isConfirm) {
                    if (isConfirm) {
                        $scope.exptoex();
                    }
                    else {

                    }
                });
        };
    }
}) ();

