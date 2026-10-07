(function () {
    'use strict';
    angular
        .module('app')
        .controller('ExamLoginPrivilegesReportController', ExamLoginPrivilegesReportController)

    ExamLoginPrivilegesReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function ExamLoginPrivilegesReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.BindData = function () {
            apiService.getDATA("ExamLoginPrivilegesReport/getdetails").
                then(function (promise) {
                    $scope.qualification_type = 'individual';
                    $scope.emp_checked = 1;
                    $scope.acdlist = promise.acdlist;
                    $scope.ctlist = promise.ctlist;
                    $scope.seclist = promise.seclist;
                    $scope.catlist = promise.catlist;
                    $scope.stafflist = promise.stafflist;

                    $scope.albumNameArray1 = [];
                    for (var i = 0; i < promise.stafflist.length; i++) {
                        if (promise.stafflist[i].firstName != '') {
                            if (promise.stafflist[i].middleName !== null) {
                                if (promise.stafflist[i].lastName !== null) {

                                    $scope.albumNameArray1.push({ staffName: promise.stafflist[i].firstName + ' ' + promise.stafflist[i].middleName + ' ' + promise.stafflist[i].lastName, hrmE_Id: promise.stafflist[i].hrmE_Id });
                                }
                                else {
                                    $scope.albumNameArray1.push({ staffName: promise.stafflist[i].firstName + ' ' + promise.stafflist[i].middleName, hrmE_Id: promise.stafflist[i].hrmE_Id });
                                }
                            }
                            else {
                                if (promise.stafflist[i].lastName !== null) {

                                    $scope.albumNameArray1.push({ staffName: promise.stafflist[i].firstName + ' ' + promise.stafflist[i].lastName, hrmE_Id: promise.stafflist[i].hrmE_Id });
                                }
                                else {
                                    $scope.albumNameArray1.push({ staffName: promise.stafflist[i].firstName, hrmE_Id: promise.stafflist[i].hrmE_Id });

                                }
                            }
                        }
                    }
                    $scope.stafflist = $scope.albumNameArray1;


                })
        };

        $scope.onselectAcdYear = function (ASMAY_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id
            }
            apiService.create("ExamLoginPrivilegesReport/onselectAcdYear", data).
                then(function (promise) {
                    $scope.ctlist = promise.ctlist;
                })
        };

        $scope.onselectclass = function (ASMCL_Id, ASMAY_Id) {
            var data = {
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id
            }
            apiService.create("ExamLoginPrivilegesReport/onselectclass", data).
                then(function (promise) {
                    $scope.seclist = promise.seclist;

                })
        };

        $scope.submitted = false;
        $scope.onreport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var classid = 0;
                var sectionid = 0;

                if ($scope.qualification_type == 'all') {
                    classid = 0;
                    sectionid = 0;
                } else {
                    classid = $scope.ASMCL_Id;
                    sectionid = $scope.ASMS_Id;
                }
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": classid,
                    "ASMS_Id": sectionid,
                    "EMCA_Id": $scope.EMCA_Id,
                    "HRME_Id": $scope.HRME_Id,
                    "report_type": $scope.qualification_type,
                    "check_type": $scope.emp_checked
                }

                apiService.create("ExamLoginPrivilegesReport/onreport", data).
                    then(function (promise) {
                        $scope.main_list = promise.datareport;
                    })
            }
        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.onselectradio = function () {
            if ($scope.qualification_type == 'all') {

                $scope.class_disable = true;
                $scope.sec_disable = true;
                $scope.ASMCL_Id = '';
                $scope.ASMS_Id = '';
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.search = "";
            }
            else {
                $scope.class_disable = false;
                $scope.sec_disable = false;
            }


        };

        $scope.cancel = function () {
            $scope.ASMAY_Id = '';
            $scope.ASMCL_Id = '';
            $scope.ASMS_Id = '';
            $scope.EMCA_Id = '';
            $scope.HRME_Id = '';
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
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
        }

    }

})();