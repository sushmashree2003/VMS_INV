(function () {
    'use strict';
    angular
        .module('app')
        .controller('AggreagtiveIntProcesscardReportController', AggreagtiveIntProcesscardReportController)

    AggreagtiveIntProcesscardReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function AggreagtiveIntProcesscardReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;

        $scope.amsT_Date = new Date();
        $scope.readmit = false;
        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("VikasaAssessment2Report/Getdetails/", pageid).
                then(function (promise) {

                    $scope.yearlt = promise.yearlist;
                    $scope.asmcL_Id = "";
                    $scope.asmS_Id = "";
                    $scope.amsT_Id = "";
                    $scope.emE_Id = "";
                    $scope.classDropdown = "";
                    $scope.sectionDropdown = "";
                    $scope.exsplt = "";
                });
        };

        $scope.get_class = function () {

            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.sectionDropdown = "";
            $scope.exsplt = "";

            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("VikasaAssessment2Report/get_class", data)
                .then(function (promise) {
                    $scope.classDropdown = promise.classlist;
                });
        };

        $scope.get_section = function () {
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.exsplt = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            };

            apiService.create("VikasaAssessment2Report/get_section", data)
                .then(function (promise) {
                    $scope.sectionDropdown = promise.sectionList;
                });
        };

        $scope.get_Exam = function () {


            $scope.emE_Id = "";

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id
            };

            apiService.create("VikasaAssessment2Report/get_exam", data)
                .then(function (promise) {
                    $scope.exsplt = promise.examList;
                });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.getcategory = function () {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id
            };
            apiService.create("VikasaAssessment2Report/getcategory", data).then(function (promise) {
                $scope.examList = promise.categoryList;
            });

        };


        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "EMCA_Id": $scope.EMCA_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id
                };

                var temp_list = [];
                apiService.create("VikasaProgressReportExam/aggregativereport", data).
                    then(function (promise) {
                        $scope.temp = promise.getsavedlist;
                        $scope.cbsesavelist = promise.getsavedlist;
                        $scope.cbsesubexamlist = promise.getexamlist;
                        $scope.cbsestudentlist = promise.getstudentdetails;
                        $scope.cbsesubjectlist = promise.getsubjectlist;
                        $scope.remarks = promise.remarks;

                        console.log("-");
                        console.log($scope.cbsesavelist);
                        console.log("--");
                        console.log($scope.cbsesubexamlist);
                        console.log("---");
                        console.log($scope.cbsestudentlist);
                        console.log("----");
                        console.log($scope.cbsesubjectlist);
                        console.log("-----");

                        if ($scope.cbsesavelist !== null && $scope.cbsesavelist.length > 0) {

                            angular.forEach($scope.cbsesavelist, function (dd) {
                                if (dd.aggavg === "A") {
                                    dd.color = 'Blue';
                                } else if (dd.aggavg === "A+") {
                                    dd.color = 'Purple';
                                } else if (dd.aggavg === "B") {
                                    dd.color = 'DarkGreen';
                                } else if (dd.aggavg === "C") {
                                    dd.color = 'Green';
                                } else if (dd.aggavg === "D") {
                                    dd.color = '#cc9900';
                                } else if (dd.aggavg === "E") {
                                    dd.color = 'Orange';
                                } else if (dd.aggavg === "F") {
                                    dd.color = 'Orange';
                                } else if (dd.aggavg === "G") {
                                    dd.color = 'RosyBrown';
                                } else if (dd.aggavg === "U") {
                                    dd.color = 'Brown';
                                } else {
                                    dd.color = 'Black';
                                }
                            });

                            $scope.studentdetails = [];
                            angular.forEach($scope.cbsestudentlist, function (stud) {
                                $scope.subjectlist = [];
                                $scope.finalgrade = [];
                                var gradefinal = "";
                                var gradeexam = "";
                                var gradeexamcolor = "";

                                angular.forEach($scope.cbsesubjectlist, function (subj) {
                                    $scope.examdetails = [];
                                    angular.forEach($scope.cbsesubexamlist, function (exam) {
                                        angular.forEach($scope.cbsesavelist, function (sav) {
                                            if (stud.amstid === parseInt(sav.amstid)) {
                                                if (parseInt(sav.ismsid) === subj.ismsid) {
                                                    if (sav.examname === exam.examname) {
                                                        $scope.examdetails.push(sav);
                                                    }
                                                }
                                                if (sav.examname === "GradeFinal") {
                                                    gradefinal = sav.aggavg;
                                                    gradeexam = sav.examname;
                                                    gradeexamcolor = sav.color;
                                                }
                                            }
                                        });
                                    });
                                    $scope.subjectlist.push({ ismsid: subj.ismsid, subjectname: subj.subjectname, examdetailsnew: $scope.examdetails });
                                });
                                $scope.studentdetails.push({
                                    amstid: stud.amstid, studentname: stud.studentname, admno: stud.admno, class: stud.classsectionname,
                                    yearname: stud.yearname, attendanceper: stud.attendanceper, finalexamname: gradeexam, finalgradename: gradefinal,
                                    finalcolor: gradeexamcolor,
                                    subjectlistnew: $scope.subjectlist
                                });
                            });

                            $scope.clstchname = promise.clstchname;
                            $scope.clastechname = $scope.clstchname[0].hrmE_EmployeeFirstName;
                            console.log("Final");
                            console.log($scope.studentdetails);


                            angular.forEach($scope.studentdetails, function (dd) {
                                angular.forEach($scope.remarks, function (re) {
                                    if (dd.amstid === re.amsT_Id) {
                                        dd.remarks = re.eprD_Remarks;
                                        dd.classpromoted = re.eprD_ClassPromoted;
                                        dd.promotionname = re.eprD_PromotionName;
                                    }
                                });
                            });
                            $scope.issuedate = new Date($scope.amsT_Date);

                        } else {
                            swal("No Records Found");
                        }

                    });
            }
        };

        $scope.cancel = function () {
            $scope.asmcL_Id = "";
            $scope.emcA_Id = "";
            $scope.asmaY_Id = "";
            $scope.emG_Id = "";
            $scope.asmS_Id = "";
            $scope.subjectlt = "";
            $scope.subjectlt1 = "";
            $scope.studentlist = false;
            $state.reload();
        };

        //for print

        // end for print

        $scope.get_totalmin = function (exm_subjs, stu_subjs) {
            $scope.stu_grandmin_marks = 0;
            angular.forEach(exm_subjs, function (itm) {
                if (itm.eyceS_AplResultFlg) {
                    angular.forEach(stu_subjs, function (itm1) {
                        if (itm1.ismS_Id === itm.ismS_Id) {
                            $scope.stu_grandmin_marks += itm.eyceS_MinMarks;
                        }
                    });
                }
            });
        };

        $scope.VIKASAProgressCardReport = function () {
            var innerContents = document.getElementById("VIKASAProgressCard").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Vikasa/ProgressCardReport/ProgressCardReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

    }

})();