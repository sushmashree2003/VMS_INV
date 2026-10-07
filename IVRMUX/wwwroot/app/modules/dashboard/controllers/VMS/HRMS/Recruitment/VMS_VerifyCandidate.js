(function () {
    'use strict';
    angular
        .module('app')
        .controller('vmsVerifyCandidateController', vmsVerifyCandidateController)
    vmsVerifyCandidateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache', '$window']
    function vmsVerifyCandidateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache, $window) {

        $scope.lead_list2 = [];
        $scope.obj = {};
        $scope.obj.all = false;
        $scope.Loaddata = function () {

            var pageid = 2;
            apiService.getURI("AddCandidateVMS/getalldetailsDocVerfy", pageid).then(function (promise) {
                //  $scope.lead_list2 = promise.candidateDetailsverfy;
                $scope.lead_list2 = [];
                if (promise.candidateDetailsverfy != null && promise.candidateDetailsverfy.length > 0) {
                    $scope.lead_list2 = promise.candidateDetailsverfy;
                    angular.forEach(promise.candidateDetailsverfy, function (dd) {
                        dd.checked = false;
                    });
                    //angular.forEach(promise.candidateDetailsverfy, function (dev) {
                    //    if ($scope.lead_list2.length === 0) {
                    //        $scope.lead_list2.push(dev);
                    //    }
                    //    else if ($scope.lead_list2.length > 0) {
                    //        var intcount = 0;
                    //        angular.forEach($scope.lead_list2, function (emp) {
                    //            if (emp.HRME_Id === dev.HRME_Id) {
                    //                intcount += 1;
                    //            }
                    //        });
                    //        if (intcount === 0) {
                    //            $scope.lead_list2.push(dev);
                    //        }
                    //    }
                    //});
                    ////
                    //angular.forEach($scope.lead_list2, function (ddd) {
                    //    ddd.selected = false;
                    //    $scope.templist = [];                     
                    //    angular.forEach(promise.candidateDetailsverfy, function (dd) {
                    //        if (dd.HRME_Id === ddd.HRME_Id) {                              
                    //            $scope.templist.push(dd);
                    //        }
                    //    });
                    //    ddd.plannerdetails = $scope.templist;

                    //});

                }


            });
        };
        $scope.toggleAll = function () {
            var toglestatus = $scope.obj.all;
            angular.forEach($scope.lead_list2, function (user) {
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

        $scope.saveData = function () {
            $scope.HR_Candidate_Documents_VerifiedByDTO = [];
            angular.forEach($scope.lead_list2, function (planner) {
                if (planner.checked == true) {
                    $scope.HR_Candidate_Documents_VerifiedByDTO.push({
                        "HRMEDS_Id": planner.HRMEDS_Id,
                        "HRCDDV_Remarks": planner.HRCDDV_Remarks,
                        "HRCDDV_RejectedFlg": planner.flag,

                    });
                }
               
            });
            if ($scope.HR_Candidate_Documents_VerifiedByDTO != null && $scope.HR_Candidate_Documents_VerifiedByDTO.length > 0) {
                var dataToSave = {
                    "HR_Candidate_Documents_VerifiedByDTO": $scope.HR_Candidate_Documents_VerifiedByDTO
                };


                apiService.create("AddCandidateVMS/saveVerify", dataToSave).then(function (response) {

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

