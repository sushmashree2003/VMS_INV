(function () {
    'use strict';
    angular
        .module('app')
        .controller('HR_Admin_NOC_DH_VerifyController', HR_Admin_NOC_DH_VerifyController)
    HR_Admin_NOC_DH_VerifyController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache', '$window']
    function HR_Admin_NOC_DH_VerifyController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache, $window) {

        $scope.lead_list2 = [];
        $scope.obj = {};
        $scope.obj.all = false;
        //$scope.Loaddata = function () {

        //    var pageid = 2;
        //    apiService.getURI("HR_NOC_Process/getadminloaddata", pageid).then(function (promise) {
        //        //  $scope.lead_list2 = promise.candidateDetailsverfy;
        //        $scope.lead_list2 = [];
        //        if (promise.candidateDetailsverfy != null && promise.candidateDetailsverfy.length > 0) {
        //            $scope.lead_list2 = promise.candidateDetailsverfy;
        //            angular.forEach(promise.candidateDetailsverfy, function (dd) {
        //                dd.checked = false;
        //            });
        //            //angular.forEach(promise.candidateDetailsverfy, function (dev) {
        //            //    if ($scope.lead_list2.length === 0) {
        //            //        $scope.lead_list2.push(dev);
        //            //    }
        //            //    else if ($scope.lead_list2.length > 0) {
        //            //        var intcount = 0;
        //            //        angular.forEach($scope.lead_list2, function (emp) {
        //            //            if (emp.HRME_Id === dev.HRME_Id) {
        //            //                intcount += 1;
        //            //            }
        //            //        });
        //            //        if (intcount === 0) {
        //            //            $scope.lead_list2.push(dev);
        //            //        }
        //            //    }
        //            //});
        //            ////
        //            //angular.forEach($scope.lead_list2, function (ddd) {
        //            //    ddd.selected = false;
        //            //    $scope.templist = [];                     
        //            //    angular.forEach(promise.candidateDetailsverfy, function (dd) {
        //            //        if (dd.HRME_Id === ddd.HRME_Id) {                              
        //            //            $scope.templist.push(dd);
        //            //        }
        //            //    });
        //            //    ddd.plannerdetails = $scope.templist;

        //            //});

        //        }


        //    });
        //};



        $scope.ISMCERTREQ_RequestDate = new Date();


        $scope.obj = {};
        $scope.editflag = false;
        $scope.showemployeedetails = false;
        $scope.itemsPerPage = 10;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0
        }


        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("HR_NOC_Process/getadminloaddata", pageid).then(function (promise) {
                if (promise.employees != null && promise.employees.length > 0) {
                    $scope.employees = promise.employees;
                }

                if (promise.display_data != null && promise.display_data.length > 0) {
                    $scope.get_data = promise.display_data;
                    $scope.presentCountgrid = $scope.get_data.length;
                }
                if (promise.document != null && promise.document.length > 0) {
                    $scope.documentlist = promise.document;
                }
                $scope.ISMCERTREQAPP_ReceiningDate = 0;
                if (promise.ismcertreqapP_ReceiningDate != null && promise.ismcertreqapP_ReceiningDate.length > 0) {
                    $scope.ISMCERTREQAPP_ReceiningDate = promise.ismcertreqapP_ReceiningDate;
                }
                if (promise.ismcertreQ_FilePath != null && promise.ismcertreQ_FilePath.length > 0) {
                    $scope.ISMCERTREQAPP_ACK = promise.ismcertreQ_FilePath;
                }
                $scope.pagePrivilege = [];
                if (promise.pagePrivilege !== null && promise.pagePrivilege.length > 0) {
                    $scope.pagePrivilege = promise.pagePrivilege;
                    $scope.IVRMRP_AddFlag = $scope.pagePrivilege[0].IVRMRP_AddFlag;
                    $scope.IVRMRP_UpdateFlag = $scope.pagePrivilege[0].IVRMRP_UpdateFlag;
                    $scope.IVRMRP_DeleteFlag = $scope.pagePrivilege[0].IVRMRP_DeleteFlag;
                    $scope.IVRMRP_ReportFlag = $scope.pagePrivilege[0].IVRMRP_ReportFlag;
                    $scope.IVRMRP_ProcessFlag = $scope.pagePrivilege[0].IVRMRP_ProcessFlag;

                }
                if (promise.hrmE_DOJ != null && promise.hrmE_DOJ.length > 0 && promise.hrmE_DOJ != null) {
                    $scope.HRME_DOJ = promise.hrmE_DOJ;
                    var dt1 = new Date($scope.HRME_DOJ);
                    var dt2 = new Date();

                    //2 years condition is removed
                    $scope.eligibilitymonths = 25;

                    //var diff = (dt2.getTime() - dt1.getTime()) / 1000;
                    //diff /= (60 * 60 * 24 * 7 * 4);
                    //$scope.eligibilitymonths = Math.abs(Math.round(diff));

                }


                $scope.todate = true;

            })
        };

    

        $scope.getcertificate = function (empid) {
            var data = {
                "HRME_Id": empid.hrmE_Id,
            };
            apiService.create("HR_NOC_Process/getcertificate", data).
                then(function (promise) {
                    $scope.tasklist = promise.tasklist;

                     

                    $scope.todate = true;

                });
        }


        $scope.toggleAll = function () {
            var toglestatus = $scope.obj.all;
            angular.forEach($scope.tasklist, function (user) {
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

