(function () {
    'use strict';
    angular
        .module('app')
        .controller('TenantViewNoticesController', TenantViewNoticesController);
    TenantViewNoticesController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$window' , '$sce', 'Excel', '$timeout', '$filter'];
    function TenantViewNoticesController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $window , $sce, Excel, $timeout, $filter) {

        $scope.start_Date = new Date();
        $scope.end_Date = new Date();
        var paginationformasters;
        $scope.multidepartment = false;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.obj = {};
        $scope.stdall = false;
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();

        $scope.submitted = false;
       

    
        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            //$scope.itemsPerPage = 5;
            $scope.search = "";
            var pageid = 2;

        
            apiService.getURI("TenantViewNotices/getdetails", pageid).then(function (promise) {

                if (promise.getloaddata.length > 0) {
                    //$scope.getloaddata = promise.getloaddata;

                    //$scope.presentCountgrid = promise.getloaddata.length;
                    $scope.TENANTAGR_Id = promise.tenantagR_Id;
                }
                else {
                    // swal("No Record Found..... !!");
                    $scope.get_printdata = "";
                }
            });
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;
                $scope.reverse = !$scope.reverse;
            };
        };

        $scope.getreport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
              

                var data = {

                        "TENANTAGR_AgrFromDate": new Date($scope.start_Date).toDateString(),
                    "TENANTAGR_AgrToDate": new Date($scope.end_Date).toDateString(),
                    "TENANTAGR_Id": $scope.TENANTAGR_Id
                      

                   

                }



                apiService.create("TenantViewNotices/get_report", data).then(function (promise) {
                    $scope.plannerid = [];
                    if (promise.getloaddata.length > 0) {
                        $scope.getloaddata = promise.getloaddata;

                        $scope.presentCountgrid = promise.getloaddata.length;
                    }
                    else {
                        swal("No Record Found..... !!");
                        $scope.get_printdata = "";
                        $state.reload();
                    }

                });

            }
            else {
                $scope.submitted = true;
            }
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
        $scope.exportToExcel = function (export_id) {
            var exportHref = Excel.tableToExcel(export_id, 'get_printdetail');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.Clear = function () {
            $state.reload();
            //$scope.get_report = "";

            //$scope.get_printdata = "";
            //$scope.HRMD_Id = "";
            //$scope.start_Date = new Date();
            //$scope.end_Date = new Date();
            //$scope.checkboxval = false;
            //$scope.checkboxval1 = false;
            //$scope.loaddata();
        };


        $scope.ViewHealthCheckUpFiles = function (obj) {

            var data = {
                "TENENTNTB_Id": obj.TENENTNTB_Id,
                "TENANTAGR_Id": $scope.TENANTAGR_Id

            };

            apiService.create("TenantViewNotices/ViewFiles", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getviewfilesdetails !== null && promise.getviewfilesdetails.length > 0) {
                        $scope.getviewfilesdetails = promise.getviewfilesdetails;
                        angular.forEach($scope.getviewfilesdetails, function (dd) {
                            if (dd.tenentntbfL_FilePath !== null && dd.tenentntbfL_FilePath !== "") {
                                var img = dd.tenentntbfL_FilePath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("dd.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.tenentntbfL_FilePath;
                                }
                            }
                        });

                        
                        $('#modalviewfiles').modal('show');
                    } else {
                        swal("No Records Found");
                        $('#modalviewfiles').modal('hide');
                    }
                }
            });
        };

        $scope.onviewpdf = function (filepath, filename) {
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

        $scope.downloaddirectimage = function (data, idd, type) {            var studentreg = idd;            $scope.imagedownload = data;            var imagedownload = data;            $http.get(imagedownload, { responseType: "arraybuffer" }).success(function (data) {                var anchor = angular.element('<a/>');                var blob = new Blob([data]);                anchor.attr({                    href: window.URL.createObjectURL(blob),                    target: '_blank',                    download: studentreg                })[0].click();            });        };

        $scope.showmothersign = function (img) {
            //$('#preview').attr('src', path);

            //$('#myModalimg').modal('show');

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
                $('#myModalimg').modal('show');

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

    }
})();

