(function () {
    'use strict';
    angular.module('app').controller('FO_Vehicle_Ob_reportController', FO_Vehicle_Ob_reportController)
   FO_Vehicle_Ob_reportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function FO_Vehicle_Ob_reportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };


        $scope.submitted = false;

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
        var configsettingsnew = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        $scope.itemsPerPage = paginationformasters;

        $scope.currentPage = 1;
        //-----------Tax Grid Select All
        $scope.ismeridian = true;
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.VMSVKEYS_DateTime = new Date();
        $scope.allmi = false;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian; todate
        };
        $scope.fo_ob_report_list = [];
        $scope.loaddata = function () {

            var pageid = 2;
            apiService.getURI("FO_Vehicle_Ob_report/loaddata1", pageid).then(function (promise) {

                $scope.fovehicale = promise.fovehicales;

            });
        };
        $scope.all_miid = function () {
            if ($scope.allmi == true) {
                angular.forEach($scope.fovehicale, function (sne) {
                    sne.miid = true;
                });
            }
            else {
                angular.forEach($scope.fovehicale, function (sne) {
                    sne.miid = false;
                });
            }
        }
        $scope.getreportt = function () {
            $scope.fo_ob_report_list = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                angular.forEach($scope.fovehicale, function (sne) {
                    // sne.miid = true
                    if (sne.miid == true) {
                        $scope.fo_ob_report_list.push({
                            VMSFOVOB_Id: sne.trmV_Id
                        });
                    }

                });

                var data = {

                    "FromDate": new Date($scope.FromDate).toDateString(),
                    "ToDate": new Date($scope.ToDate).toDateString(),
                    "fo_ob_report_list": $scope.fo_ob_report_list,
                };
                apiService.create("FO_Vehicle_Ob_report/RecieptReport1", data).then(function (promise) {
                    if (promise.getReportt != null && promise.getReportt.length > 0) {
                        $scope.raj = promise.getReportt;

                    }
                    else {
                        swal("Record Not Found  !");
                    }


                });
            }
        };
        $scope.View = function (user) {
            var data = {
                "VMSFOVOB_Id": user.VMSFOVOB_Id
            }
            apiService.create("Vehicle_Observation/RecieptReport", data).
                then(function (promise) {
                    if (promise.filist != null && promise.filist.length > 0) {
                        $scope.GetFile = promise.filist;
                        $('#ViewDetails').modal('show');

                    }
                    else {
                        swal("File Is Not Attached !");
                        $('#ViewDetails').modal('hide');
                    }

                })
        }
        $scope.previewimg_new = function (img) {
            $scope.vbabrsF_FilePath = img;
            var img = $scope.vbabrsF_FilePath;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.vbabrsF_FilePath);
                    $('#myimagePreview').modal('show');

                }
                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                    $window.open($scope.vbabrsF_FilePath);
                }
                else if (lastelement == 'pdf') {
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.vbabrsF_FilePath;
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
            }
            else {
                $window.open($scope.vbabrsF_FilePath)
            }
        };


        $scope.isRequiredmi = function () {

            return !$scope.fovehicale.some(function (options) {
                return options.miid;
            });

        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };
    }



})();
