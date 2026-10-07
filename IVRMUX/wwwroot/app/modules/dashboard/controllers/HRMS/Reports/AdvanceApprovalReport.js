(function () {
    'use strict';

    angular
        .module('app')
        .controller('AdvanceApprovalReportController', AdvanceApprovalReportController);

    AdvanceApprovalReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter', '$q']

    function AdvanceApprovalReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter, $q) {

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
                var checkStatus = $scope.hrmE_Id;
                var count = 0;
                angular.forEach($scope.employees, function (itm) {
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
                return !$scope.employees.some(function (options) {
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

                $scope.hrmE_Id = $scope.employees.every(function (options) {
                    return options.selected;
                });
            }

            apiService.getURI("SalaryAdvanceApproval/onloaddata", pageid).then(function (promise) {
                $scope.fillmonth = promise.fillmonth;
                $scope.fillyear = promise.fillyear;
                $scope.employees = promise.employees;
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clear_Details = function () {
            $state.reload();
        };

        $scope.ShowReportdata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var MONTHID = [];
                var HRMEID = [];



               angular.forEach($scope.employees, function (ty) {
                    if (ty.selected) {
                        HRMEID.push({ HRME_Id: ty.hrmE_Id });
                    }
                })

                if ($scope.allind == "monthwise") {
                    $scope.fromdate = new Date().toDateString();
                    $scope.todate = new Date().toDateString();
                    angular.forEach($scope.fillmonth, function (ty) {
                        if (ty.selected == true) {
                            MONTHID.push({ IVRM_Month_Name: ty.ivrM_Month_Name });
                        }
                    })
                }
                else {
                    MONTHID.push({ IVRM_Month_Id: "0" });
                    $scope.fromdate = new Date($scope.fromdate).toDateString();
                    $scope.todate = new Date($scope.todate).toDateString();
                    $scope.ASMAY_Id = 0;
                }

                var data = {
                    "HRMEID": HRMEID,
                    "yearid": $scope.ASMAY_Id,
                    "MONTHID": MONTHID,
                    "Fromdate": $scope.fromdate,
                    "Todate": $scope.todate,
                    "rdbbutton": $scope.allind,
                    "Status_Type": $scope.status,
                   
                   
                };


                apiService.create("SalaryAdvanceApproval/getapprovalreport", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.approvalReport = promise.approvalReport;
                        if ($scope.approvalReport !== null && $scope.approvalReport.length > 0) {
                            $scope.approvaldetails = $scope.approvalReport;

                            console.log($scope.approvaldetails);


                            $scope.certificaterequestid = [];

                            angular.forEach($scope.approvaldetails, function (dev) {
                                if ($scope.certificaterequestid.length === 0) {
                                    $scope.certificaterequestid.push({
                                        HRESA_Id: dev.HRESA_Id,
                                        EMPLOYEENAME: dev.Employee_Name,
                                        REQDATE: dev.HRESA_EntryDate,
                                        HRESA_Remarks: dev.HRESA_Remarks,
                                        HRESA_ModeOfPayment: dev.HRESA_ModeOfPayment,
                                        HRESA_AdvYear: dev.HRESA_AdvYear,
                                        HRESA_AdvMonth: dev.HRESA_AdvMonth,
                                        HRESAA_Date: dev.HRESAA_Date,
                                        HRESA_AppliedAmount: dev.HRESA_AppliedAmount,
                                        HRESA_AdvStatus: dev.HRESA_AdvStatus,
                                        HRESAA_Status: dev.HRESAA_Status,
                                        HRESAA_Amount: dev.HRESAA_Amount,

                                    });
                                } else if ($scope.certificaterequestid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.certificaterequestid, function (emp) {
                                        if (emp.HRESA_Id === dev.HRESA_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.certificaterequestid.push({
                                            HRESA_Id: dev.HRESA_Id,
                                            EMPLOYEENAME: dev.Employee_Name,
                                            REQDATE: dev.HRESA_EntryDate,
                                            HRESA_Remarks: dev.HRESA_Remarks,
                                            HRESA_ModeOfPayment: dev.HRESA_ModeOfPayment,
                                            HRESA_AdvYear: dev.HRESA_AdvYear,
                                            HRESA_AdvMonth: dev.HRESA_AdvMonth,
                                            HRESAA_Date: dev.HRESAA_Date,
                                            HRESA_AppliedAmount: dev.HRESA_AppliedAmount,
                                            HRESA_AdvStatus: dev.HRESA_AdvStatus,
                                            HRESAA_Status: dev.HRESAA_Status,
                                            HRESAA_Amount: dev.HRESAA_Amount,
                                        });
                                    }
                                }
                            });

                            console.log($scope.certificaterequestid);

                            angular.forEach($scope.certificaterequestid, function (ddd) {
                                $scope.templist = [];
                                angular.forEach($scope.approvaldetails, function (dd) {
                                    if (dd.HRESA_Id === ddd.HRESA_Id) {
                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.templistdata = $scope.templist;
                            });

                            console.log($scope.certificaterequestid);

                        }

                        else {
                            swal("No Records Found");
                            $scope.approvaldetails = []
                        }
                    }

                });
            }

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
        $scope.Print = function () {

            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExceldetails = function () {

            var excelname = 'SALARY ADVANCE APPROVAL REPORT';
            excelname = excelname.toUpperCase() + '.xls';
            var exportHref = Excel.tableToExcel('#exceltopd', 'SALARY ADVANCE APPROVAL REPORT');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

    }
})();


