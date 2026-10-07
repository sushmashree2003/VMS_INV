(function () {
    'use strict';
    angular
        .module('app')
        .controller('PassFailReportController', PassFailReportController)

    PassFailReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function PassFailReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.ddate = {};
        $scope.ddate = new Date();

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

        $scope.imgname = logopath;




        $scope.BindData = function () {
            apiService.getDATA("PassFailReport/getdetails").
                then(function (promise) {

                    $scope.acdlist = promise.acdlist;
                    $scope.catlist = promise.catlist;
                    $scope.ctlist = promise.ctlist;
                    $scope.seclist = promise.seclist;
                    $scope.examlist = promise.examlist;
                    $scope.studentlist = promise.studentlist;
                })
        };

        $scope.onselectCategory = function (ASMAY_Id, EMCA_Id) {
            $scope.main_list = [];
            $scope.main_list2 = [];
            $scope.main_list3 = [];
            $scope.main_list4 = [];
            var data = {
                "ASMAY_Id": ASMAY_Id,
                "EMCA_Id": EMCA_Id
            }
            apiService.create("PassFailReport/onselectCategory", data).
                then(function (promise) {
                    $scope.ctlist = promise.ctlist;
                    $scope.examlist = promise.examlist;

                })
        };


        $scope.onselectclass = function (ASMCL_Id, ASMAY_Id, EMCA_Id) {
            $scope.main_list = [];
            $scope.main_list2 = [];
            $scope.main_list3 = [];
            $scope.main_list4 = [];
            var data = {
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id,
                "EMCA_Id": EMCA_Id
            }
            apiService.create("PassFailReport/onselectclass", data).
                then(function (promise) {
                    $scope.seclist = promise.seclist;
                })
        };


        $scope.onselectSection = function () {
            $scope.main_list = [];
            $scope.main_list2 = [];
            $scope.main_list3 = [];
            $scope.main_list4 = [];
            var data = {
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMS_Id": $scope.ASMS_Id
            }
            apiService.create("PassFailReport/onselectSection", data
            ).then(function (promise) {
                $scope.studentlist = promise.studentlist;
            })
        }


        $scope.submitted = false;

        $scope.onreport = function () {

            $scope.main_list = [];
            $scope.main_list2 = [];
            $scope.main_list3 = [];
            $scope.main_list4 = [];


            $scope.submitted = true;

            if ($scope.myForm.$valid) {

                var classid = 0;
                var sectionid = 0;
                var stdid = 0;
                var emeid = 0;

                if ($scope.qualification_type == 'all') {
                    classid = 0;
                    sectionid = 0;
                    stdid = 0;
                    emeid = $scope.EME_Id
                }
                else if ($scope.qualification_type == 'individual' || $scope.qualification_type == 'Absent' || $scope.qualification_type == 'Medical' || $scope.qualification_type == 'onDuty') {
                    stdid = 0;
                    classid = $scope.ASMCL_Id;
                    sectionid = $scope.ASMS_Id;
                    emeid = $scope.EME_Id;
                }
                else if ($scope.qualification_type == 'Studentwise') {
                    emeid = 0;
                    stdid = $scope.AMST_Id;
                    classid = $scope.ASMCL_Id;
                    sectionid = $scope.ASMS_Id;
                }

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": classid,
                    "ASMS_Id": sectionid,
                    "EMCA_Id": $scope.EMCA_Id,
                    "EME_Id": emeid,
                    "AMST_Id": stdid,
                    "report_type": $scope.qualification_type
                }

                apiService.create("PassFailReport/onreport", data).
                    then(function (promise) {


                        $scope.main_list = promise.datareport;
                        $scope.main_list2 = promise.datareport2;
                        $scope.main_list3 = promise.datareport3;
                        $scope.main_list4 = promise.datareport4;


                        angular.forEach($scope.acdlist, function (dd) {
                            if (dd.asmaY_Id == $scope.ASMAY_Id) {
                                $scope.year = dd.asmaY_Year;
                            }
                        });

                        angular.forEach($scope.examlist, function (dde) {
                            if (dde.emE_Id == $scope.EME_Id) {
                                $scope.examname = dde.emE_ExamName;
                            }
                        })


                        if ($scope.qualification_type == 'individual' || $scope.qualification_type == 'all') {
                            if ($scope.main_list == 0 || $scope.main_list == null || $scope.main_list == undefined) {
                                $scope.main_list = "";
                                swal('No Records Found!!!');
                            }
                            else {
                                $scope.details = "For Year : " + $scope.year + " " + "Exam  : " + $scope.examname;

                                $scope.main_list = promise.datareport;
                                $scope.main_temp_list = $scope.main_list;
                                var main_temp_list = [];
                                angular.forEach($scope.main_list, function (obj) {
                                    var count = 0;
                                    angular.forEach(main_temp_list, function (obj2) {
                                        if (obj2.asmcL_ClassName == obj.asmcL_ClassName) {
                                            count++;
                                        }
                                    })

                                    if (count == 0) {
                                        var temp_array = [];
                                        angular.forEach($scope.main_list, function (obj1) {
                                            if (obj1.asmcL_ClassName == obj.asmcL_ClassName) {
                                                temp_array.push(obj1);
                                            }
                                        })
                                        main_temp_list.push({ asmcL_Id: obj.asmcL_Id, asmcL_ClassName: obj.asmcL_ClassName, temp_array: temp_array });
                                    }
                                })
                                $scope.main_temp_list = main_temp_list;
                                console.log(main_temp_list);
                            }
                        }

                        else
                            if ($scope.qualification_type == 'Studentwise') {
                                if (promise.datareport2 == 0 || promise.datareport2 == null || promise.datareport2 == undefined) {
                                    $scope.main_list2 = "";
                                    swal('No Records Found!!!');
                                }
                                else {

                                    angular.forEach($scope.ctlist, function (dd) {
                                        if ($scope.ASMCL_Id == dd.asmcL_Id) {
                                            $scope.classname = dd.asmcL_ClassName;
                                        }
                                    })

                                    angular.forEach($scope.seclist, function (dd) {
                                        if ($scope.ASMS_Id == dd.asmS_Id) {
                                            $scope.sectionname = dd.asmC_SectionName;
                                        }
                                    })

                                    $scope.details = "For Year : " + $scope.year + " " + "Class - Section  : " + $scope.classname + "-" + $scope.sectionname;
                                    $scope.main_list2 = promise.datareport2;
                                }

                            }
                            else if ($scope.qualification_type == 'Medical') {
                                if ($scope.main_list3 == 0 || $scope.main_list3 == null || $scope.main_list3 == undefined) {
                                    $scope.main_list3 = "";
                                    swal('No Records Found!!!');
                                }
                                else {
                                    $scope.main_list3 = promise.datareport3;
                                }
                            }

                            else if ($scope.qualification_type == 'Absent' || $scope.qualification_type == 'onDuty') {
                                if (promise.datareport4 == 0 || promise.datareport4 == null || promise.datareport4 == undefined) {
                                    $scope.main_list4 = "";
                                    swal('No Records Found!!!');
                                }
                                else {

                                    angular.forEach($scope.ctlist, function (dd) {
                                        if ($scope.ASMCL_Id == dd.asmcL_Id) {
                                            $scope.classname = dd.asmcL_ClassName;
                                        }
                                    })

                                    angular.forEach($scope.seclist, function (dd) {
                                        if ($scope.ASMS_Id == dd.asmS_Id) {
                                            $scope.sectionname = dd.asmC_SectionName;
                                        }
                                    })

                                    if ($scope.qualification_type == 'onDuty') {

                                        $scope.type = "On Duty";
                                    } else {
                                        $scope.type = "Absent";
                                    }

                                    $scope.details =" - " + $scope.type + " For Year : " + $scope.year + " " + "Exam  : " + $scope.examname;
                                    $scope.main_list4 = promise.datareport4;
                                }
                            }
                    });
            }
        };

        $scope.onselectradio = function () {
            if ($scope.qualification_type == 'all') {
                $scope.class_disable = true;
                $scope.sec_disable = true;
                $scope.stud_disable = true;
                $scope.ASMCL_Id = '';
                $scope.ASMS_Id = '';
                $scope.AMST_Id = '';
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.search = "";

                $scope.main_list = [];
                $scope.main_list2 = [];
                $scope.main_list3 = [];
                $scope.main_list4 = [];

            } else {
                $scope.class_disable = false;
                $scope.sec_disable = false;
                $scope.stud_disable = false;
            }
            if ($scope.qualification_type == 'individual' || $scope.qualification_type == 'Absent' || $scope.qualification_type == 'Medical' || $scope.qualification_type == 'onDuty') {
                $scope.stud_disable = true;
                $scope.AMST_Id = '';
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.search = "";

                $scope.main_list = [];
                $scope.main_list2 = [];
                $scope.main_list3 = [];
                $scope.main_list4 = [];

            } if ($scope.qualification_type == 'Studentwise') {
                $scope.exam_disable = true;
                $scope.EME_Id = '';
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.search = "";

                $scope.main_list = [];
                $scope.main_list2 = [];
                $scope.main_list3 = [];
                $scope.main_list4 = [];
            }
            else {
                $scope.exam_disable = false;
            }
        };



        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.printData = function () {

            if ($scope.qualification_type == 'all' || $scope.qualification_type == 'individual') {
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
            else if ($scope.qualification_type == 'Studentwise') {
                var innerContents = document.getElementById("table2").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }

            else if ($scope.qualification_type == 'Medical') {
                var innerContents = document.getElementById("table3").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            else if ($scope.qualification_type == 'Absent' || $scope.qualification_type == 'onDuty') {
                var innerContents = document.getElementById("table4").innerHTML;
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

    }

})();