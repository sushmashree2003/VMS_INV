(function () {
    'use strict';
    angular.module('app').controller('FMS_Search_ReportController', FMS_Search_ReportController)
    FMS_Search_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$sce', 'Excel','$timeout']
    function FMS_Search_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $sce, Excel, $timeout) {

        $scope.getreport = [];
        var configsettings = JSON.parse(localStorage.getItem("configsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        $scope.onchnageradiobtnsfile = function () {
            $scope.getreport = [];
            $scope.name = $scope.ClientSupplierFlgflag;
            if ($scope.name === 'Client') {
                $scope.columnname = "ISMMCLT_ClientName";
            }
            if ($scope.name === 'Supplier') {
                $scope.columnname = "INVMS_SupplierName";
            }
            if ($scope.name === 'Sales Lead') {
                $scope.columnname = "ISMSLE_LeadName";
            }
        };

        $scope.SearchReport = function () {
            $scope.getreport = [];
            if ($scope.myForm2.$valid) {
                var data = "";

                data = {
                    "ClientSupplierFlgflag": $scope.ClientSupplierFlgflag,
                    "SearchValue": $scope.searchdetails
                };

                apiService.create("FMS_Correspondance_Report/SearchReport", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.getreport = promise.getreport;
                        if ($scope.getreport === null || $scope.getreport.length === 0) {
                            swal("No Records Found");
                        }
                    }
                });

            } else {
                $scope.submitted2 = true;
            }
        };

        $scope.viewreferencedetais = function (userd) {
            var data = {
                "FMSCOR_Id": userd.FMSCOR_Id
            };

            apiService.create("FMS_Correspondance_Report/viewreferencedetais", data).then(function (promise) {

                if (promise !== null) {
                    $scope.getcabintdetails = promise.getcabintdetails;
                    $scope.getfilesdetails = promise.getfilesdetails;
                    $scope.getmodedetails = promise.getmodedetails;

                    var count = 0;
                    if ($scope.getmodedetails === null || $scope.getmodedetails.length === 0) {
                        count += 0;
                    } else {
                        count +=1;
                    }
                    if ($scope.getfilesdetails === null || $scope.getfilesdetails.length === 0) {
                        count += 0;
                    } else {
                        count += 1;
                    }
                    if ($scope.getcabintdetails === null || $scope.getcabintdetails.length === 0) {
                        count += 0;
                    } else {
                        count += 1;
                    }

                    angular.forEach($scope.getfilesdetails, function (dd) {
                        if (dd.FMSCORFL_SoftCopyFilePath !== null && dd.FMSCORFL_SoftCopyFilePath !== '') {
                            var img = dd.FMSCORFL_SoftCopyFilePath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            console.log("dd.filetype : " + dd.filetype);
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.FMSCORFL_SoftCopyFilePath;
                            }
                        }
                    });

                    angular.forEach($scope.getmodedetails, function (dd) {
                        if (dd.FMSCORMD_FilePath !== null && dd.FMSCORMD_FilePath !== '') {
                            var img = dd.FMSCORMD_FilePath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            console.log("dd.filetype : " + dd.filetype);
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.FMSCORMD_FilePath;
                            }
                        }
                    });

                    $scope.refernceno = userd.FMSCOR_RefernceNo;

                    if (count === 0) {
                        swal("No Records Found");
                    } else {
                        $('#mymodalviewdetailsfirsttab').modal('show');
                    }
                }
            });
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted2;
        };

        $scope.exportToExceldetails = function () {

            var excelname = 'FMS Report';
            excelname = excelname.toUpperCase() + '.xls';
            //var printSectionId = export_id;s
            var exportHref = Excel.tableToExcel('#exceldetail', 'FMS Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.printDatadetails = function () {
            var innerContents = "";
            innerContents = document.getElementById("printdetail").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExceldetails1 = function () {

            var excelname = 'FMS Report';
            excelname = excelname.toUpperCase() + '.xls';
            //var printSectionId = export_id;s
            var exportHref = Excel.tableToExcel('#excelreferncenode', 'FMS Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.printDatadetails1 = function () {
            var innerContents = "";
            innerContents = document.getElementById("printreferncenode").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        var imagedownload = "";
        var docname = "";
        $scope.downloaddirectimage = function (data, idd, typeofphoto, type) {

            var studentreg = idd;

            $scope.imagedownload = data;
            imagedownload = data;
            docname = typeofphoto;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: idd
                    })[0].click();
                });
        };

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.fmscorfL_SoftCopyFilePath;
            $scope.videdfd = data.fmscorfL_SoftCopyFilePath;
            $scope.movie = { src: data.fmscorfL_SoftCopyFilePath };
            $scope.movie1 = { src: data.fmscorfL_SoftCopyFilePath };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.fmscorfL_SoftCopyFilePath });
            console.log($scope.view_videos);
        };

        $scope.onview = function (filepath, filename) {
            //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            document.getElementById("pdfviewdd").innerHTML = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var htmlElements = '<embed id="pdfview" src="' + $scope.content + '"  style="width: 100%;" height="1000" type="application/pdf" />';
                    document.getElementById("pdfviewdd").innerHTML = htmlElements;
                    $('#showpdf').modal('show');
                });
        };

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };
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