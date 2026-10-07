(function () {
    'use strict';
    angular
        .module('app')
        .controller('ToppersListReportController', ToppersListReportController)

    ToppersListReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function ToppersListReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null) {

            if (admfigsettings.length > 0) {
                logopath = admfigsettings[0].asC_Logo_Path;
            } else {
                logopath = "";
            }

        } else {
            logopath = "";
        }

        $scope.imgname = logopath;


        $scope.BindData = function () {
            apiService.getDATA("ToppersListReport/getdetails").
                then(function (promise) {
                    $scope.acdlist = promise.acdlist;
                    $scope.catlist = promise.catlist;
                    $scope.ctlist = promise.ctlist;
                    $scope.seclist = promise.seclist;
                    $scope.sublist = promise.sublist;
                    $scope.examlist = promise.examlist;
                })
        };


        $scope.onselectCategory = function (ASMAY_Id, EMCA_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id,
                "EMCA_Id": EMCA_Id
            }
            apiService.create("ToppersListReport/onselectCategory", data).
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
            apiService.create("ToppersListReport/onselectclass", data).
                then(function (promise) {
                    $scope.seclist = promise.seclist;
                })
        };


        $scope.submitted = false;
        $scope.onreport = function () {
            $scope.main_list = [];
            $scope.main_list1 = [];
            $scope.submitted = true;
            $scope.exm_checked = true;
            if ($scope.myForm.$valid) {
                var asmsid = 0;
                var subid = 0;
                if ($scope.qualification_type == '1') {
                    asmsid = 0;
                }
                var exm_check = 0;
                if ($scope.exm_checked === true) {
                    exm_check = 1;
                } else {
                    exm_check = 0;
                }

                var sub_check = 0;
                if ($scope.sub_checked === 1) {
                    sub_check = 1;
                } else {
                    sub_check = 0;
                }

               
                if ($scope.ASMS_Id === "") {
                    asmsid = 0;
                } else {
                    asmsid = $scope.ASMS_Id;
                }

                if ($scope.ISMS_Id === "") {
                    subid = 0;
                } else {
                    subid = $scope.ISMS_Id;
                }

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": asmsid,
                    //"EMCA_Id": $scope.EMCA_Id,
                    "EME_Id": $scope.EME_Id,
                    "ISMS_Id": subid,
                    "report_type": $scope.qualification_type,
                    "sub_check_type": sub_check,
                    "exm_check_type": exm_check,
                    "topper": $scope.topper
                }

                apiService.create("ToppersListReport/onreport", data).
                    then(function (promise) {

                        angular.forEach($scope.acdlist, function (dd) {
                            if (dd.asmaY_Id == $scope.ASMAY_Id) {
                                $scope.year = dd.asmaY_Year;
                            }
                        });

                        angular.forEach($scope.ctlist, function (ddd) {
                            if (ddd.asmcL_Id == $scope.ASMCL_Id) {
                                $scope.class = ddd.asmcL_ClassName
                            }
                        });

                        angular.forEach($scope.examlist, function (d) {
                            if (d.emE_Id == $scope.EME_Id) {
                                $scope.exam = d.emE_ExamName
                            }
                        });

                        if ($scope.qualification_type === "1") {

                            if ($scope.sub_checked === 1) {

                                angular.forEach($scope.sublist, function (d1) {
                                    if (d1.ismS_Id === $scope.ISMS_Id) {
                                        $scope.subjectname = d1.ismS_SubjectName;
                                    }
                                });

                                $scope.details = "Year :" + $scope.year + ' Class :' + $scope.class + ' Exam : ' + $scope.exam + ' Subject : ' + $scope.subjectname
                            }
                            else {
                                $scope.details = "Year :" + $scope.year + ' Class :' + $scope.class + ' Exam : ' + $scope.exam
                            }                            
                        }

                        else if ($scope.qualification_type === "2") {

                            if ($scope.sub_checked === 1) {

                                angular.forEach($scope.sublist, function (d1) {
                                    if (d1.ismS_Id === $scope.ISMS_Id) {
                                        $scope.subjectname = d1.ismS_SubjectName;
                                    }
                                })


                                $scope.details = "Year :" + $scope.year + ' Class - Section :' + $scope.class + ' Exam : ' + $scope.exam + ' Subject : ' + $scope.subjectname
                            }
                            else {
                                $scope.details = "Year :" + $scope.year + ' Class :' + $scope.class + ' Exam : ' + $scope.exam
                            }
                        }



                        if ($scope.sub_checked === 1) {
                            $scope.main_list1 = promise.datareport;

                            if ($scope.qualification_type === 2) {

                                angular.forEach($scope.main_list1, function (dd2) {
                                    dd2.ASMCL_ClassName = dd2.asmcL_ClassName;
                                    dd2.ASMC_SectionName = dd2.asmC_SectionName;
                                });
                            }

                        } else {
                            $scope.main_list = promise.datareport;
                        }
                    });
            }
        };

        $scope.printData = function () {

            if ($scope.qualification_type == '1' || $scope.qualification_type == '2') {
                var data1 = "";
                if ($scope.sub_checked === 1) {
                    data1 = "tablesub";
                } else {
                    data1 = "table";
                }

                var innerContents = document.getElementById(data1).innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }

            else if ($scope.qualification_type == 'toppers') {
                var innerContents = document.getElementById("table1").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
        };

        $scope.cancel = function () {
            $scope.ASMAY_Id = '';
            $scope.ASMCL_Id = '';
            $scope.ASMS_Id = '';
            $scope.EMCA_Id = '';
            $scope.EME_Id = '';
            $scope.ISMS_Id = '';
            $scope.topper = '';
            $scope.main_list = [];
            $scope.main_list1 = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.get_sec_exam = function () {
            $scope.main_list = [];
            $scope.main_list1 = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
            }
            apiService.create("ToppersListReport/get_sec_exam", data).then(function (promise) {

                if (promise != null) {
                    $scope.seclist = promise.seclist;

                    if ($scope.seclist.length == 0) {
                        $scope.ASMS_Id == "";
                    }

                    $scope.examlist = promise.examlist;
                    if ($scope.examlist.length == 0) {
                        $scope.EME_Id = "";
                    }

                } else {
                    swal("No Records Found")
                }
            })
        };

        $scope.onselectexam = function () {
            $scope.main_list = [];
            $scope.main_list1 = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            }

            apiService.create("ToppersListReport/onselectexam", data).then(function (promise) {
                if (promise != null) {
                    
                    $scope.examlist = promise.examlist;
                    $scope.examlist = promise.examlist;
                    if ($scope.examlist.length == 0) {
                        $scope.EME_Id = "";
                    }

                } else {
                    swal("No Records Found")
                }
            })
        }

        $scope.get_subject = function () {
            $scope.main_list = [];
            $scope.main_list1 = [];
            var asms_id = 0;
            if ($scope.ASMS_Id === "") {
                asms_id = 0;
            } else {
                asms_id = $scope.ASMS_Id;
            }

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": asms_id,
                "EME_Id": $scope.EME_Id,
            }

            apiService.create("ToppersListReport/get_subject", data).then(function (promise) {
                if (promise != null) {
                    $scope.sublist = promise.sublist;
                    if ($scope.sublist.length == 0) {
                        $scope.ISMS_Id = "";
                    }

                } else {
                    swal("No Records Found");
                }
            });
        };

        $scope.onselectAcdYear = function () {
            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            $scope.EME_Id = "";
            $scope.ISMS_Id = "";
        }
    }

})();