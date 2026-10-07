(function () {
    'use strict';
    angular.module('app').controller('RTO_VoilationReportController', RTO_VoilationReportController)
    RTO_VoilationReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', '$timeout','Excel']
    function RTO_VoilationReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, $timeout, Excel) {
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
        $scope.FromDate = new Date();
        $scope.ToDate = new Date();
        $scope.allmi = false;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian; todate
        };        
        $scope.rto_ob_report_list = [];
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("RTO_VoilationReport/loaddata1", pageid).then(function (promise) {               
                $scope.rtoviolation = promise.rtoviolations;
                if ($scope.rtoviolation != null && $scope.rtoviolation.length > 0) {
                    $scope.allmi = true;
                    angular.forEach($scope.rtoviolation, function (sne) {
                        sne.miid = true;
                    });
                }
            });
        };
        $scope.all_miid = function () {
            if ($scope.allmi == true) {
               
                angular.forEach($scope.rtoviolation, function (sne) {
                    sne.miid = true;
                });
            }
            else {
                
                angular.forEach($scope.rtoviolation, function (sne) {
                    sne.miid = false;
                });
            }
        }
        $scope.View = function (user) {
            var data = {
                "RTOVOB_Id": user.RTOVOB_Id
            }
            apiService.create("RTOViolation/RecieptReport", data).
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


        $scope.Clear = function () {
            $state.reload();

        };
        $scope.getreportt = function () {
            
            $scope.rto_ob_report_list = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                
                angular.forEach($scope.rtoviolation, function (sne) {
                    if (sne.miid == true) {
                        $scope.rto_ob_report_list.push({
                            VMSFOVOB_Id: sne.trmV_Id
                        });
                    }

                });

                var data = {

                    "FromDate": new Date($scope.FromDate).toDateString(),
                    "ToDate": new Date($scope.ToDate).toDateString(),
                    "rto_ob_report_list": $scope.rto_ob_report_list,
                };
                apiService.create("RTO_VoilationReport/RecieptReport1", data).then(function (promise) {
                    if (promise.getReportt != null && promise.getReportt.length > 0) {
                        $scope.raj = promise.getReportt;
                        angular.forEach($scope.raj, function (itm) {
                            itm.RTOVOB_DateTimeOfViolation = moment(itm.RTOVOB_DateTimeOfViolation, 'HH:mm a').format();
                           

                        });
                    }
                    else {
                        swal("Record Not Found  !");
                    }


                });
            }
        };

        $scope.isRequiredmi = function () {

            return !$scope.rtoviolation.some(function (options) {
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