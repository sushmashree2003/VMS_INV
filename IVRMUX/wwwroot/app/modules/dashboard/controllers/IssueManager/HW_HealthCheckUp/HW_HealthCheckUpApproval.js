(function () {
    'use strict';
    angular.module('app').controller('HW_HealthCheckUpApprovalController', HW_HealthCheckUpApprovalController)
    HW_HealthCheckUpApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', '$sce']
    function HW_HealthCheckUpApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, $sce) {

        var paginationformasters;
        var copty;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            paginationformasters = 10;
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.submitted1 = false;
        $scope.temptaskreponsefiles = [];
        $scope.temptaskreponsefiles.push({ 'id': 'Teacher1' });
        $scope.objques = {};

        $scope.VisitedDate = new Date();
        $scope.visitedmaxdate = new Date();

        $scope.LoadHWHealthCheckUpdataApproval = function () {
            $scope.getuserinstitution = [];
            $scope.MI_Id = "";
            $scope.getapprovaldetails = [];
            $scope.getloaddata = [];
            $scope.submitted1 = false;
            var data = {
                "OnLoadOrOnChange": "OnLoad"
            };

            apiService.create("HW_HeathCheckUp/LoadHWHealthCheckUpdataApproval", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getuserinstitution = promise.getuserinstitution;
                    $scope.MI_Id = promise.mI_Id;
                    $scope.getapprovaldetails = promise.getapprovaldetails;
                    $scope.getloaddata = promise.getloaddata;

                    if (promise.returnmsg === "NotMapped") {
                        swal("You Don't Have Privileges To Approve The Details So Contact Administrator");
                    }
                }
            });
        };

        $scope.OnChangeHwHealthCheckUpInst = function () {
            $scope.submitted1 = false;
            $scope.getloaddata = [];
            $scope.getapprovaldetails = [];
            var data = {
                "OnLoadOrOnChange": "OnChange",
                "MI_Id": $scope.MI_Id
            };

            apiService.create("HW_HeathCheckUp/LoadHWHealthCheckUpdataApproval", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getapprovaldetails = promise.getapprovaldetails;
                    $scope.getloaddata = promise.getloaddata;
                    if (promise.returnmsg === "NotMapped") {
                        swal("You Don't Have Privileges To Approve The Details So Contact Administrator");
                    }
                }
            });
        };

        $scope.SaveHWHealthChekUpApproval = function () {
            $scope.submitted1 = false;
            if ($scope.myForm1.$valid) {
                $scope.SelectedItems = [];

                angular.forEach($scope.getapprovaldetails, function (dd) {
                    if (dd.selectedvalue) {
                        $scope.SelectedItems.push({
                            HWHCHUP_Id: dd.HWHCHUP_Id, Approved_Level: Number(dd.Approved_Level),
                            ISMMCLT_Id: dd.ISMMCLT_Id, HRME_Id: dd.HRME_Id, FinalFlag: dd.FinalFlag,
                            apprvlremarks: dd.apprvlremarks, approvalstatus: dd.approvalstatus
                        });
                    }
                });

                var data = {
                    "MI_Id": $scope.MI_Id,
                    "ApprovalDetails_Temp": $scope.SelectedItems
                };
                apiService.create("HW_HeathCheckUp/SaveHWHealthChekUpApproval", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Record Saved/Updated Successfully");
                        } else {
                            swal("Failed To Save/Updated Record");
                        }

                        $scope.OnChangeHwHealthCheckUpInst();
                    }
                });
            } else {
                $scope.submitted1 = true;
            }
        };

        $scope.ViewHealthCheckUpFiles = function (obj) {

            var data = {
                "HWHCHUP_Id": obj.HWHCHUP_Id,
                "MI_Id": $scope.MI_Id
            };

            apiService.create("HW_HeathCheckUp/ViewHealthCheckUpFiles", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getviewfilesdetails !== null && promise.getviewfilesdetails.length > 0) {
                        $scope.getviewfilesdetails = promise.getviewfilesdetails;
                        angular.forEach($scope.getviewfilesdetails, function (dd) {
                            if (dd.filepath !== null && dd.filepath !== "") {
                                var img = dd.filepath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("dd.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
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

        $scope.ViewHealthCheckUpComments = function (obj) {

            var data = {
                "HWHCHUP_Id": obj.HWHCHUP_Id,
                "MI_Id": $scope.MI_Id
            };

            apiService.create("HW_HeathCheckUp/ViewHealthCheckUpComments", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.viewcommens !== null && promise.viewcommens.length > 0) {
                        $scope.viewcommens = promise.viewcommens;                       
                        $('#modalviewcomments').modal('show');
                    } else {
                        swal("No Records Found");
                        $('#modalviewcomments').modal('hide');
                    }
                }
            });
        };

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalPreview').modal('show');
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

        $scope.ViewDocExcelfles = function (docpath, filename, filetype) {
            docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + docpath;
            $scope.detailFrame = $sce.trustAsResourceUrl(docpath);
            $('#myModaldocx').modal('show');
        };

        $scope.downloaddirectimage = function (data, idd, type) {
            var studentreg = idd;
            $scope.imagedownload = data;
            var imagedownload = data;
            $http.get(imagedownload, { responseType: "arraybuffer" }).success(function (data) {
                var anchor = angular.element('<a/>');
                var blob = new Blob([data]);
                anchor.attr({
                    href: window.URL.createObjectURL(blob),
                    target: '_blank',
                    download: studentreg
                })[0].click();
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.getapprovaldetails.some(function (options) {
                return options.selectedvalue;
            });
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.Clearid = function () {
            $scope.OnChangeHwHealthCheckUpInst();
        };

    }
})();