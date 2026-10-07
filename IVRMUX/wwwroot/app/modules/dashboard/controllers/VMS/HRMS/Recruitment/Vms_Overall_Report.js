(function () {
    'use strict';
    angular
        .module('app')
        .controller('VmsOverall_ReportController', VmsOverall_ReportController);
    VmsOverall_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout','$window'];
    function VmsOverall_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, Excel, $timeout, $window) {
        $scope.currentPage = 1;
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.newdate = new Date();
        $scope.grid_view = false;
        $scope.Employee = {};
        $scope.Employee.Qualification = false;
        $scope.Employee.PositionType = false;
        $scope.Employee.JobTitle = false;
        $scope.Employee.Religion = false;
        $scope.Employee.Experience = false;
        $scope.Employee.CTC = false;
        $scope.Employee.DateWise = false;
        $scope.searchValue = '';
        $scope.no_found = false;
        $scope.loadData = function () {
            var id = 2;
            apiService.getURI("VmsOverall_Report/getalldetails", id).
                then(function (promise) {
                    if (promise.masterQualification !== null && promise.masterQualification.length > 0) {
                        $scope.masterQualification = promise.masterQualification;
                    }
                    if (promise.masterPositionList !== null && promise.masterPositionList.length > 0) {
                        $scope.masterPositionList = promise.masterPositionList;
                    }
                    if (promise.masterjob !== null && promise.masterjob.length > 0) {
                        $scope.masterjob = promise.masterjob;
                    }
                    if (promise.masterReligion !== null && promise.masterReligion.length > 0) {
                        $scope.masterReligion = promise.masterReligion;
                    }
                    if (promise.gender !== null && promise.gender.length > 0) {
                        $scope.gender = promise.gender;
                    }
                });


            //Set From date and To date
            $scope.fromdate = new Date();
            $scope.todate = $scope.fromdate;
            $scope.minDateTo = new Date(
                $scope.todate.getFullYear(),
                $scope.todate.getMonth(),
                $scope.todate.getDate());
        };

        $scope.FormatType = "Format1";

        //setToDate
        $scope.setToDate = function (fromdate) {
            $scope.todate = fromdate;
            $scope.minDateTo = new Date(
                $scope.todate.getFullYear(),
                $scope.todate.getMonth(),
                $scope.todate.getDate());
            
            $scope.todate = "";
            if ($scope.griddeatails) {
                $scope.griddeatails = false;
            }
        }
        $scope.OnchageToDate = function () {

            if ($scope.griddeatails) {
                $scope.griddeatails = false;
            }
        }



        //ivrmmG_GenderName = "MALE"
        //ivrmmG_Id = 2
        //=============== view Resume==========
        $scope.previewimg_new = function (img) {
            $scope.hrcD_Resume = img;
            var img = $scope.hrcD_Resume;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.ismclT_FilePath);
                    $('#myimagePreview').modal('show');
                }
                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                    $window.open($scope.hrcD_Resume)
                }
                else if (lastelement == 'pdf') {
                    ///=====================show pdf, img
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.hrcD_Resume;
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
                $window.open($scope.hrcD_Resume)
            }
        };
        $scope.ngClick = function () {
            $scope.getDbArray = [];
            $scope.submitted = false;

        };
        //============View Photo======
        $scope.previewimgnew = function (img) {
            $scope.hrcD_Photo = img;

            var img = $scope.hrcD_Photo;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.hrcD_Photo);
                    $('#myimagePreview').modal('show');
                }
                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                    $window.open($scope.hrcD_Photo)
                }
                else if (lastelement == 'pdf') {
                    ///=====================show pdf, img
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.hrcD_Photo;
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
                $window.open($scope.hrcD_Photo)
            }
        };
        $scope.ogen = {};
        $scope.obj = {};
        $scope.objj = {};
        $scope.mrfCand = {};
        $scope.mrfCcand = {};
        $scope.objfrom = {};
        $scope.objto = {};
        $scope.mrCand = {};
        $scope.mfCand = {};
        $scope.fCand = {};
        $scope.Genn = {};
        $scope.sts = {};
        $scope.submitted = false;
        $scope.GetReport = function () {
            $scope.getDbArray = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var HRMC_Id = 0;
                var HRMPT_Id = 0;
                var HRMJ_Id = 0;
                var IVRMMG_Id = 0;
                var FROM_DATE = null;
                var TO_DATE = null;
                var HRCD_ExpFrom = 0;
                var BELOW_CTC = 0;
                var HIGH_CTC = 0;
                var HRCD_Religion = 0;
                var IVRMMG_Id = 0;
                var Recruitmentstatus = "";
                if ($scope.Employee.Qualification === true) {
                    HRMC_Id = $scope.obj.hrmC_Id;
                }
                if ($scope.Employee.PositionType === true) {
                    HRMPT_Id = $scope.objj.HRMPT_Id;
                }
                if ($scope.Employee.JobTitle === true) {
                    HRMJ_Id = $scope.mrfCand.hrmJ_Id;
                }
                if ($scope.Employee.Religion === true) {
                    HRCD_Religion = $scope.mrfCcand.hrcD_Religion;
                }
                if ($scope.Employee.gender === true) {
                    IVRMMG_Id = $scope.Genn.IVRMMG_Id;
                }
                if ($scope.Employee.DateWise === true) {
                    FROM_DATE = new Date($scope.objfrom.fromdate).toDateString();
                    TO_DATE = new Date($scope.objto.todate).toDateString();
                }
                if ($scope.Employee.Experience === true) {
                    HRCD_ExpFrom = $scope.mrCand.hrcD_ExpFrom;
                }
                if ($scope.Employee.CTC === true) {
                    HIGH_CTC = $scope.fCand.hrcD_highCTC;
                    BELOW_CTC = $scope.mfCand.hrcD_belowCTC;
                }
                if ($scope.Employee.status === true) {
                    Recruitmentstatus = $scope.sts.hrcD_RecruitmentStatus;
                }
                var data = {
                    "HRMC_Id": HRMC_Id,
                    "HRMPT_Id": HRMPT_Id,
                    "HRMJ_Id": HRMJ_Id,
                    "HRCD_Religion": HRCD_Religion,
                    "FROM_DATE": FROM_DATE,
                    "TO_DATE": TO_DATE,
                    "HRCD_ExpFrom": HRCD_ExpFrom,
                    "BELOW_CTC": BELOW_CTC,
                    "HIGH_CTC": HIGH_CTC,
                    "IVRMMG_Id": IVRMMG_Id,
                    "RDO_TyPETwo": $scope.obj.rdopunchTwo,
                    "Recruitmentstatus": Recruitmentstatus
                }
                apiService.create("VmsOverall_Report/getReport", data).then(function (promise) {
                    if (promise.get_DbArray.length !== 0) {
                        $scope.getDbArray = promise.get_DbArray;
                        $scope.grid_view = true;
                    }
                    else {
                        swal("Record not found.");
                    }
                });            
            }
        }
        //validation start
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        //print start
        $scope.printData = function () {
            var id = "CandiateOverallReport";
            var innerContents = document.getElementById(id).innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        //print end
        //export start
        $scope.exptoex = function (export_id) {
            var excelname = "CandiateOverallReport";
            var excelsheetname = "CandiateOverallReport";
            var reportname = "#CandiateOverallReport";
            excelname = excelname.toUpperCase() + '.xls';
            var exportHref = Excel.tableToExcel(reportname, excelsheetname);
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };       
        $scope.clearid = function () {
            $state.reload();
        };
        //clear end
        $scope.All_click = function () {
            $scope.getDbArray = [];
            $scope.submitted = false;
            if ($scope.Employee.All === true) {
                $scope.Employee.Qualification = true;
                $scope.Employee.PositionType = true;
                $scope.Employee.JobTitle = true;
                $scope.Employee.Religion = true;
                $scope.Employee.Experience = true;
                $scope.Employee.CTC = true;
                $scope.Employee.DateWise = true;
                $scope.Employee.gender = true;
                $scope.Employee.status = true;
            }
            else {
                $scope.Employee.Qualification = false;
                $scope.Employee.PositionType = false;
                $scope.Employee.JobTitle = false;
                $scope.Employee.Religion = false;
                $scope.Employee.Experience = false;
                $scope.Employee.CTC = false;
                $scope.Employee.DateWise = false;
                $scope.Employee.gender = false;
                $scope.Employee.status = false;
            }
        };
    }
})();

