(function () {
    'use strict';
    angular
        .module('app')
        .controller('Training_Cerificate_frFloorHRController', Training_Cerificate_frFloorHRController)
    Training_Cerificate_frFloorHRController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache', '$window']
    function Training_Cerificate_frFloorHRController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache, $window) {

        $scope.lead_list2 = [];
        $scope.obj = {};
        $scope.obj.all = false;
        $scope.Loaddata = function () {
            $scope.itemsPerPage = 10;
            $scope.currentPage = 1;
            var pageid = 2;
            apiService.getURI("Appointment/getalldetails", pageid)
                .then(function (promise) {
                    // $scope.AllDataTraningHO1 = promise.allDataTraningHO
                    $scope.AllDataTraningHO1 = [];
                    $scope.AllDataTraningHR1 = [];

                    if (promise.allDataTraningHO != null && promise.allDataTraningHO.length > 0) {
                        $scope.AllDataTraningHO1 = promise.allDataTraningHO;

                        angular.forEach(promise.allDataTraningHO, function (dd) {
                            dd.checked = false;
                        });
                    }

                    if (promise.allDataTraningHR != null && promise.allDataTraningHR.length > 0) {
                        $scope.AllDataTraningHR2 = promise.allDataTraningHR;

                        angular.forEach($scope.AllDataTraningHR2, function (emp) {
                            if (emp.HRETF_Status == "Pending") {
                                $scope.AllDataTraningHR1.push(emp);
                            }
                        });

                        angular.forEach(promise.allDataTraningHR, function (dd) {
                            dd.checked = false;
                        });
                    }


                })


        };
        $scope.toggleAll = function () {
            var toglestatus = $scope.obj.all;
            angular.forEach($scope.AllDataTraningHR1, function (user) {
                user.checked = toglestatus;
            });

        }
        $scope.previewimg_new = function (img) {
            $scope.ismclT_FilePath = img;
            var img = $scope.ismclT_FilePath;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.ismclT_FilePath);
                    $('#myimagePreview').modal('show');
                }
                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                    $window.open($scope.ismclT_FilePath);
                }
                else if (lastelement == 'pdf') {
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.ismclT_FilePath;
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
                $window.open($scope.ismclT_FilePath)
            }
        }


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


        $scope.radiochange = function (ee) {
            if (ee.flag === 'A') {

                ee.classname = 'neww';
            }
            else {

                ee.classname = 'oldd';
            }
        }

        $scope.cleardata = function () {
            $scope.usercheckCC = "";
            $scope.usercheckC = "";
            $state.reload();
        }

        //SaveData

        $scope.saveAppointmentsMD = function () {
            $scope.HR_Candidate_DetailsDTO = [];
            angular.forEach($scope.AllDataTraningHR1, function (planner) {
                if (planner.checked == true) {
                    $scope.HR_Candidate_DetailsDTO.push({
                        "HRETF_Id": planner.HRETF_Id,
                        "HRETF_Remarks": planner.AL_Remarks,
                        "HRETF_Status": planner.AL_Flg,
                        "HRME_ID": planner.HRME_Id,
                        "HRMD_Id": planner.HRMD_Id,


                    });
                }

            });
            if ($scope.HR_Candidate_DetailsDTO != null && $scope.HR_Candidate_DetailsDTO.length > 0) {
                var data = {
                    "approvalHRTranning": $scope.HR_Candidate_DetailsDTO
                };


                apiService.create("Appointment/SaveHRDetails", data).then(function (response) {

                    if (response.retrunMsg == "Add") {
                        swal("Recod Save Sucessfully  !");
                    }
                    if (response.retrunMsg == "notsave") {
                        swal("Recod Not saved !");
                    }
                    if (response.retrunMsg == "admin") {
                        swal("Please concat Administrtor !");
                    }
                    $state.reload();

                })
            }
            else {
                swal("Please Select Atleast One CheckBox !");
            }

        };


    }
})();

