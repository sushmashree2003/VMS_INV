(function () {
    'use strict';
    angular
        .module('app')
        .controller('AppointmentLaterverifyApproval_MDController', AppointmentLaterverifyApproval_MDController)
    AppointmentLaterverifyApproval_MDController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache', '$window']
    function AppointmentLaterverifyApproval_MDController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache, $window) {

        $scope.lead_list2 = [];
        $scope.obj = {};
        $scope.obj.all = false;
        $scope.Loaddata = function () {

            var pageid = 2;
            apiService.getURI("Appointment/getalldetails", pageid)
                .then(function (promise) {
                    //$scope.AllAppointmentLetters1 = promise.allAppointmentLetters;
                    $scope.AllAppointmentLetters1 = [];
                    if (promise.allAppointmentLetters != null && promise.allAppointmentLetters.length > 0) {
                        $scope.AllAppointmentLetters1 = promise.allAppointmentLetters;
                        angular.forEach(promise.allAppointmentLetters, function (dd) {
                            dd.checked = false;
                        });
                       

                    }

                })
        };
        $scope.toggleAll = function () {
            var toglestatus = $scope.obj.all;
            angular.forEach($scope.AllAppointmentLetters1, function (user) {
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
            angular.forEach($scope.AllAppointmentLetters1, function (planner) {
                if (planner.checked == true) {
                    $scope.HR_Candidate_DetailsDTO.push({
                        "AL_Id": planner.AL_Id,
                        "AL_Remarks": planner.AL_Remarks,
                        "AL_Flg": planner.AL_Flg,

                    });
                }

            });
            if ($scope.HR_Candidate_DetailsDTO != null && $scope.HR_Candidate_DetailsDTO.length > 0) {
                var dataToSave = {
                    "HR_Candidate_DetailsDTO": $scope.HR_Candidate_DetailsDTO
                };


                apiService.create("Appointment/SaVeAppointmentsHead", dataToSave).then(function (response) {

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

