(function () {
    'use strict';
    angular
.module('app')
.controller('PercentagewiseDetailsReportController', PercentagewiseDetailsReportController)

    PercentagewiseDetailsReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function PercentagewiseDetailsReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.BindData = function () {
            apiService.getDATA("PercentagewiseDetailsReport/getdetails").
            then(function (promise) {
                $scope.qualification_type = 'individual';
                $scope.acdlist = promise.acdlist;
                $scope.catlist = promise.catlist;
                $scope.ctlist = promise.ctlist;
                $scope.seclist = promise.seclist;     
                $scope.examlist = promise.examlist;
                $scope.studentlist = promise.studentlist;
            })
        };

        $scope.onselectCategory = function (ASMAY_Id, EMCA_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id,
                "EMCA_Id": EMCA_Id
            }
            apiService.create("PercentagewiseDetailsReport/onselectCategory", data).
            then(function (promise) {
                $scope.ctlist = promise.ctlist;

            })
        };

        $scope.onselectclass = function (ASMCL_Id, ASMAY_Id, EMCA_Id) {
            var data = {
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id,
                "EMCA_Id": EMCA_Id
            }
            apiService.create("PercentagewiseDetailsReport/onselectclass", data).
            then(function (promise) {
                $scope.seclist = promise.seclist;
            })
        };

        $scope.onselectSection = function () {
            var data = {
                "ASMS_Id": $scope.ASMS_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("PercentagewiseDetailsReport/onselectSection", data).
            then(function (promise) {
                $scope.examlist = promise.examlist;
                $scope.studentlist = promise.studentlist;
            })
        };

        $scope.onselectradio = function () {
            if ($scope.qualification_type == 'all') {
                $scope.stud_disable = true;
                $scope.AMST_Id = "";
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.search = "";
            }
            else {
                $scope.stud_disable = false;
            }
        };

        $scope.submitted = false;
        $scope.onreport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.qualification_type == 'all') {
                    $scope.AMST_Id = 0;
                }

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "EMCA_Id": $scope.EMCA_Id,
                    "EME_Id": $scope.EME_Id,
                    "AMST_Id": $scope.AMST_Id,
                    "report_type": $scope.qualification_type,
                    "percent":$scope.percent
                }

                apiService.create("PercentagewiseDetailsReport/onreport", data).
                then(function (promise) {

                    $scope.main_list = promise.datareport;

                })
            }
        };

        $scope.printData = function () {
            
            var innerContents = document.getElementById("table").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                  '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.cancel = function () {
            $scope.ASMAY_Id = '';
            $scope.ASMCL_Id = '';
            $scope.ASMS_Id = '';
            $scope.EMCA_Id = '';
            $scope.EME_Id = '';
            $scope.AMST_Id = '';
            $scope.Main_list = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

    }

})();