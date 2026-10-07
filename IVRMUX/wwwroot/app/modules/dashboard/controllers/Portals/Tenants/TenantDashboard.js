
(function () {
    'use strict';
    angular
        .module('app')
        .controller('TenantDashboardController', TenantDashboardController);
    TenantDashboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', '$http', '$q', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$interval', '$sce', 'uiCalendarConfig', 'appSettings'];

    function TenantDashboardController($rootScope, $scope, $state, $location, dashboardService, Flash, $http, $q, apiService, $stateParams, $filter, superCache, $window, $interval, $sce, uiCalendarConfig, appSettings) {
        var miid = "";
       
        $scope.closeupdate = false;
        $scope.conformflag = true;

        $scope.RedirectToNoticeboard = function () {
            window.location.href = "http://localhost:57606/#/app/TenantViewNoticeBoardReport/";
          //  window.location.href = "https://vapsmanagementsoftware.azurewebsites.net/#/app/TenantViewNoticeBoardReport/";

        };
        $scope.RedirectTopaymentreport = function () {
            window.location.href = "http://localhost:57606/#/app/TenantIndividualOfflinePaymentReport/";
           // window.location.href = "https://vapsmanagementsoftware.azurewebsites.net/#/app/TenantIndividualOfflinePaymentReport/";
        };
        $scope.RedirectTopayment = function () {
            window.location.href = "http://localhost:57606/#/app/TenantOnlinePayment/";
          // window.location.href = "https://vapsmanagementsoftware.azurewebsites.net/#/app/TenantOnlinePayment/";
        };
        // View Student Profile
        $scope.RedirectToDocument = function () {

            var data = {
                "TENANTAGR_Id": $scope.TENANTAGR_Id

            };

            apiService.create("TenantAgreement/ViewFiles", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getviewfilesdetails !== null && promise.getviewfilesdetails.length > 0) {
                        $scope.getviewfilesdetails = promise.getviewfilesdetails;
                        angular.forEach($scope.getviewfilesdetails, function (dd) {
                            if (dd.tenantagrfL_FileLocation !== null && dd.tenantagrfL_FileLocation !== "") {
                                var img = dd.tenantagrfL_FileLocation;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("dd.filetype : " + dd.filetype);
                                if (lastelement === 'PDF' || lastelement === 'Pdf' || lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.tenantagrfL_FileLocation;
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

        $scope.downloaddirectimage = function (data, idd, type) {            var studentreg = idd;            $scope.imagedownload = data;            var imagedownload = data;            $http.get(imagedownload, { responseType: "arraybuffer" }).success(function (data) {                var anchor = angular.element('<a/>');                var blob = new Blob([data]);                anchor.attr({                    href: window.URL.createObjectURL(blob),                    target: '_blank',                    download: studentreg                })[0].click();            });        };

        $scope.ViewDocExcelfles = function (docpath, filename, filetype) {
            docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + docpath;
            $scope.detailFrame = $sce.trustAsResourceUrl(docpath);
            $('#myModaldocx').modal('show');
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


        $scope.ViewTenantProfile = function () {
            var pageid = 1;

            apiService.create("TenantDashboard/ViewTenantProfile", pageid).then(function (promise) {

                if (promise !== null) {


                    if (promise.tenants_details != null ) {
                        $scope.tenants_details = promise.tenants_details;

                        $scope.tenant_name = $scope.tenants_details[0].tenantagR_TenantName;
                        $scope.TENANTAGR_EmailId = $scope.tenants_details[0].tenantagR_EmailId;
                        $scope.TENANTAGR_ContactNo = $scope.tenants_details[0].tenantagR_ContactNo;
                        $scope.TENANTAGR_Address = $scope.tenants_details[0].tenantagR_Address;
                        $scope.TENANTAGR_AgrFromDate = $scope.tenants_details[0].tenantagR_AgrFromDate;
                        $scope.TENANTAGR_AgrToDate = $scope.tenants_details[0].tenantagR_AgrToDate;
                        $scope.TENANTAGR_AdvanceAmount = $scope.tenants_details[0].tenantagR_AdvanceAmount;
                        $scope.TENANTAGR_RentDueDay = $scope.tenants_details[0].tenantagR_RentDueDay;
                        $scope.TENANTAGR_MonthlyRent = $scope.tenants_details[0].tenantagR_MonthlyRent;
                        $scope.TENANTAGR_Id = $scope.tenants_details[0].tenantagR_Id;
                        // $scope.TENANTAGR_Imagepath = promise.get_tenant_details[0].TENANTAGRFL_FileLocation;

                    }

                    if (promise.recenocol.length > 0 && promise.recenocol != null) {
                        $scope.tenant_paid_details = promise.recenocol;

                    } 

                    if (promise.get_tenant_details.length > 0) {
                        $scope.TENANTAGR_Imagepath = promise.get_tenant_details[0].TENANTAGRFL_FileLocation;
                    }

                   


                }
            });
        };


        $scope.viewmybalanceamount = function () {
            if ($scope.tenant_paid_details.length > 0 && $scope.tenant_paid_details != null) {
                $scope.get_tenant_paid_details = $scope.tenant_paid_details;
                $('#myModalMonthlyDetails').modal('show');
            } else {
                $('#myModalMonthlyDetails').modal('hide');
            }
        }


        $scope.getMonthlyBalance_details = function (type) {
            var data = {
                "type":type
            }
            apiService.create("TenantDashboard/getMonthlyBalance_details", data).then(function (promise) {

                if (promise.get_month_details.length > 0 && promise.get_month_details != null) {
                    $scope.get_month_details = promise.get_month_details;
                    $('#myModalMonthlyBalanceDetails').modal('show');
                } else {
                    $('#myModalMonthlyBalanceDetails').modal('hide');
                }

            });

        }


        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };

        //===================
    }
    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });

})();
