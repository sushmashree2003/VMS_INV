(function () {
    'use strict';
    angular
        .module('app')
        .controller('MOMReportController', MOMReportController)

    MOMReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
    function MOMReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache) {

        $scope.searchValue = "";
        $scope.disablemonth = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.searchValue2 = "";

        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey2 == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey2 = key;
        }

        $scope.onselectradio = function () {
            $scope.Cumureport = false;
            $scope.Cumureport1 = false;
            $scope.screport = false;
            $scope.export = false;
        }
        $scope.momdetailreport = [];
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.getchanged = function (yr, mon) {
            $scope.report = false;
            $scope.monthname = "";
            angular.forEach($scope.fillmonth, function (check) {
                if (check.ivrM_Month_Id == mon) {
                    $scope.monthname = check.monthname;
                }
            });
            $scope.yearname = "";
            angular.forEach($scope.fillyear, function (check) {
                if (check.hrmlY_LeaveYear == yr) {
                    $scope.yearname = check.hrmlY_LeaveYear;
                }
            });
        };

        $scope.loadgrid = function () {
            var id = 2;
            apiService.getURI("MOMUpdate/getreport", id).
                then(function (promise) {
                    $scope.fillmonth = promise.fillmonth;
                    $scope.fillyear = promise.fillyear;
                });
        };
        $scope.ShowReportdata = function () {
            $scope.submitted = true;
            $scope.disablemonth = true;
            if ($scope.myForm.$valid) {

                if ($scope.allind == "Monthwise") {
                    $scope.from_date = new Date().toDateString();
                    $scope.to_date = new Date().toDateString();
                }
                else {
                    $scope.from_date = new Date($scope.fromdate).toDateString();
                    $scope.to_date = new Date($scope.todate).toDateString();
                    $scope.year = new Date($scope.fromdate).getFullYear();
                    $scope.month = (new Date($scope.fromdate).getMonth() + 1);
                }

                var data = {
                    "Fromdate": $scope.from_date,
                    "Todate": $scope.to_date,
                    "Year_id": $scope.year,
                    "Month_id": $scope.month,
                    "flg": $scope.allind,
                    //"userId": 0,
                };

                apiService.create("MOMUpdate/showreport", data).then(function (promise) {

                    if (promise.momdetailreport.length > 0) {
                        if ($scope.allind == 'Datewise') {
                            $scope.momdetailreport = promise.momdetailreport;
                        }
                        else if ($scope.allind == 'Monthwise') {
                            $scope.momdetailreport = promise.momdetailreport;
                        }
                    }
                    else {
                        swal("No Records Found");
                    }
                })
            }
        };
        $scope.cancel = function () {
            $state.reload();
        };
        $scope.agendadetails = function (obj) {
            $scope.MOM_Agenda = obj.MOM_Agenda
            document.getElementById("ada").innerHTML = $scope.MOM_Agenda;
        };
        $scope.minutesdetails = function (item) {
            $scope.MOM_Minutes = item.MOM_Minutes
        };
        $scope.objdata = [];
        $scope.viewdocument = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.objdata = obj;

            apiService.create("MOMUpdate/viewdetails", obj).then(function (promise) {
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

        $scope.minutesdetails = function (item) {
            $scope.MOM_Minutes = item.MOM_Minutes
        };

        $scope.ViewDetails = function (user) {
            var data = {
                "MOM_Id": user.MOM_Id
            }
            apiService.create("MOMUpdate/viewdetails/", data).then(function (promise) {

                if (promise.getParticipentsdetails.length > 0 && promise.getParticipentsdetails !== null) {
                    $scope.getParticipentsdetails = promise.getParticipentsdetails;
                    $('#participants').modal('show');
                }
            });
        };


    }
})();