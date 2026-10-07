(function () {
    'use strict';
    angular
        .module('app')
        .controller('HealthCheckFeedbackController', HealthCheckFeedbackController);
    HealthCheckFeedbackController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache', 'myFactoryvisitor', '$q']
    function HealthCheckFeedbackController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache, myFactoryvisitor, $q) {
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.itemsPerPage1 = 10;
        $scope.feedback = false;
        $scope.feedbacksave = false;
        //----------onload function----------//
        $scope.BindData = function () {

            var pageid = 2;
            apiService.getURI("Employee_Wise_VMS_FeedBack/GetMonthYear", pageid).then(function (promise) {

                if (promise !== null) {
                    $scope.months = promise.monthnames;
                    $scope.years = promise.masteryear;
                    $scope.feedback = false;
                    $scope.feedbacksave = false;
                }
                else {
                    swal("No Data Found !");
                }
            })


        
        };        $scope.clientdata = function () {            var pageid = 10;
            var data = {                "pageid": 10,                "year": $scope.f_Year,                "monthname": $scope.r_month            }
            apiService.create("Employee_Wise_VMS_FeedBack/employeefeedback", data).then(function (promise) {
                if (promise.getloaddata.length > 0 && promise.getloaddata != null ) {

                    $scope.feedback = true;
                    $scope.feedbacksave = true;
                    $scope.getloaddata = promise.getloaddata; 
                    $scope.feedbacklist = promise.feedbacklist;

                }
                else if (promise.feedbacklist.length > 0 && promise.feedbacklist != null)
                {
                    if (!promise.getloaddata || promise.getloaddata.length === 0) {
                        $scope.feedback = false;
                        swal("No Health Check-Up Data Available for Feedback");
                    }
                    $scope.feedbacklist = promise.feedbacklist;
                    $scope.feedbacksave = true;
                }
                else {
                    $scope.feedback = false;
                    $scope.feedbacksave = false;
                    swal("No Details Found !", "", "warning");
                }
            })
        }                //save call center data        $scope.submitted = false;        $scope.savecallcenter = function () {            $scope.CallFeedback_List = [];            if ($scope.myForm.$valid) {                if ($scope.getloaddata != null && $scope.getloaddata.length > 0) {                    angular.forEach($scope.getloaddata, function (ss) {                        if (ss.usercheck == true) {                            $scope.CallFeedback_List.push({                                HRME_Id: ss.HRME_Id,                                VEF_Remarks: ss.remarks,                                VEF_NegativeFlag: ss.VEF_NegativeFlag,                                ISMMCLT_ClientName: ss.ISMMCLT_ClientName,                                ISMMCLT_Id: ss.ISMMCLT_Id,                                HWHCHUP_Id: ss.HWHCHUP_Id                            });                        }                    })                }                if ($scope.CallFeedback_List != null && $scope.CallFeedback_List.length > 0) {
                    var data = {                        "CallFeedback_List": $scope.CallFeedback_List,                    }                    apiService.create("Employee_Wise_VMS_FeedBack/savecallcenter", data).then(function (promise) {                        if (promise.returnmsg == "saved") {                            swal('FeedBack Saved Successfully', "", "success");                        }                        else if (promise.returnmsg == "") {                            swal("failed to save", "", "error");                        }                        else {                            swal('Please Contcat Administrator');                        }                        $state.reload();                    });                }                else {                    swal('Please Select  Atleast one CheckBox');                }                            }                       else {                $scope.submitted = true;            }            $scope.interacted = function (field) {
                return $scope.submitted;
            };        };        //View Files        $scope.ViewHealthCheckUpFiles = function (obj) {

            var data = {
                "HWHCHUP_Id": obj.HWHCHUP_Id,
                
            };

            apiService.create("Employee_Wise_VMS_FeedBack/ViewHealthCheckUpFiles", data).then(function (promise) {
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
        };        //view image        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalPreview').modal('show');
        };        //view Pdf        $scope.onviewpdf = function (filepath, filename) {
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
        };        //view excel files        $scope.ViewDocExcelfles = function (docpath, filename, filetype) {
            docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + docpath;
            $scope.detailFrame = $sce.trustAsResourceUrl(docpath);
            $('#myModaldocx').modal('show');
        };        $scope.printData = function () {
            var innerContents = document.getElementById("checkupreport").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();

            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();

        }        $scope.refresh = function () {
            $scope.r_month = "";
            $scope.getloaddata = [];
            $scope.feedbacklist =[];
            
        };    }
})();