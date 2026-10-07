
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CumulativeReportSRKVSController', CumulativeReportSRKVSController)

    CumulativeReportSRKVSController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function CumulativeReportSRKVSController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

        $scope.applicable = false;
        $scope.nonapplicable = false;
        $scope.attendancedispaly = false;
        $scope.classrank = true;

        $scope.exportToExcel = function (tableIds) {
            var excelname = $scope.exmmid + ' - Cumulative Report - ' + $scope.yr + '-' + $scope.cla + '-' + $scope.sec + '.xls';
            var exportHref = Excel.tableToExcel(tableIds, 'Cumulative Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.exportToExceld = function (tableIds) { // ex: '#my-table'
            var exportHref = Excel.tableToExcel(tableIds, 'sheet name');

            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };


        $scope.report = false;
        $scope.applicable = false;
        $scope.nonapplicable = false;

        // create the list of themes  
        $scope.fonts = [
            { name: '10px', size: '10px ', class: 'font10' },
            { name: '11px', size: '11px ', class: 'font11' },
            { name: '12px', size: '12px ', class: 'font12' },
            { name: '13px', size: '13px ', class: 'font13' },
            { name: '14px', size: '14px ', class: 'font14' },
            { name: '15px', size: '15px', class: 'font15' },
            { name: '16px', size: '16px', class: 'font16' },
            { name: '17px', size: '17px', class: 'font17' },
            { name: '18px', size: '18px', class: 'font18' },
            { name: '25px', size: '25px', class: 'font25' }
        ];
        $scope.font_size = 'font25';


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;

        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            apiService.getDATA("CumulativeReport/Getdetails").
                then(function (promise) {

                    $scope.yearlt = promise.yearlist;
                    $scope.clslist = promise.classlist;
                    $scope.seclist = promise.seclist;
                    $scope.amstlt = promise.amstlist;
                    $scope.exsplt = promise.exmstdlist;
                });
        };

        //Record pop up 
        $scope.viewrecordspopup = function (employee, SweetAlert) {

            $scope.editEmployee = employee.amsT_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("CumulativeReport/getalldetailsviewrecords", pageid).
                then(function (promise) {

                    $scope.viewrecordspopupdisplay = promise.gtdetailsview;
                });

        };
        $scope.clearpopupgrid = function () {
            $scope.viewrecordspopupdisplay = "";
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        //to active or deactive 
        $scope.deactive = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.estsU_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        var config = {
                            headers: {
                                'Content-Type': 'application/json;'
                            }
                        };

                        apiService.create("CumulativeReport/deactivate", deactiveRecord).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal(confirmmgs + ' Successfully');
                                }
                                else {
                                    swal('Record Not  Activated/Deactivated');
                                }
                                $scope.BindData();
                                $scope.clearid1();
                            });
                    }
                    else {
                        swal("Record" + mgs + "Cancelled");
                    }
                });
        };




        // to Edit Data
        $scope.getorgvalue = function (EditRecord) {
            var MEditId = EditRecord.amsT_Id;
            apiService.getURI("CumulativeReport/editdetails", MEditId).
                then(function (promise) {
                    if (promise.editlist.length > 0) {
                        $scope.te_sub_ls = promise.editlist;
                        $scope.AMST_Id = promise.editlist[0].amsT_Id;
                        $scope.asmaY_Id = promise.editlist[0].asmaY_Id;
                        $scope.asmcL_Id = promise.editlist[0].asmcL_Id;
                        $scope.asmS_Id = promise.editlist[0].asmS_Id;
                        $scope.emcA_Id = promise.edclasslist[0].emcA_Id;
                        $scope.emG_Id = promise.editlist[0].emG_Id;
                        $scope.Onsubjectchange($scope.emG_Id);
                        $scope.valid_Section($scope.asmcL_Id, $scope.emcA_Id, $scope.asmaY_Id, $scope.asmS_Id);
                        //  $scope.Onsubjectchange($scope.emG_Id);

                        $scope.studentlist = true;
                    } else {
                        swal('No Record Found');
                    }
                });
        };

        $scope.sortKey = 'amsT_FirstName';

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //$scope.subchkbx = function (record1,record,mainary) {
        $scope.subchkbx = function (column, obj, user) {
            //  $scope.gridview2 = true;

            if (obj.abc == true) {
                angular.forEach($scope.subjectlt1, function (itm1) {
                    if (itm1.amsT_Id == user.amsT_Id) {
                        angular.forEach($scope.subjectlt, function (itm2) {
                            if (itm2.ismS_Id == column.ismS_Id) {
                                itm1.sub_list.push({ id: itm2.ismS_Id, name: itm2.ismS_SubjectName });
                            }
                        });
                    }
                });
            }
            else {
                angular.forEach($scope.subjectlt1, function (itm1) {
                    if (itm1.amsT_Id == user.amsT_Id) {
                        for (var i = 0; i < itm1.sub_list.length; i++) {
                            if (itm1.sub_list[i].id == column.ismS_Id) {
                                itm1.sub_list.splice(i, 1);
                            }
                        }
                    }
                });
            }
        };


        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.marksgradedisplay = [];
                $scope.marksgradedisplaynon = [];
                $scope.studentslt = [];
                $scope.studentslt1 = [];
                $scope.electivesub = [];
                $scope.electivestd = [];
                var data = {
                    "EME_ID": $scope.emE_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "applicableflag": $scope.applicable,
                    "nonapplicableflag": $scope.nonapplicable,
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("CumulativeReport/savedetails", data).
                    then(function (promise) {

                        $scope.masterinst = promise.configuration;
                        var count = 0;
                        if ($scope.masterinst.length > 0) {
                            if ($scope.masterinst[0].exmConfig_RegnoColumnDisplay === true) {
                                $scope.regno = true;
                                count = count + 1;

                            } else {
                                $scope.regno = false;
                            }

                            if ($scope.masterinst[0].exmConfig_AdmnoColumnDisplay === true) {
                                $scope.admno = true;
                                count = count + 1;
                            } else {
                                $scope.admno = false;
                            }

                            if ($scope.masterinst[0].exmConfig_RollnoColumnDisplay === true) {
                                $scope.rollno = true;
                                count = count + 1;
                            } else {
                                $scope.rollno = false;
                            }

                            if (count === 0) {
                                $scope.admno = true;
                                $scope.rollno = true;
                            }


                        } else {
                            $scope.admno = true;
                            $scope.rollno = true;
                        }

                        if ($scope.applicable === true && $scope.nonapplicable === false) {
                            $scope.nonapplicables = false;
                            $scope.applicables = true;

                        } else if ($scope.applicable === false && $scope.nonapplicable === true) {
                            $scope.nonapplicables = true;
                            $scope.applicables = false;
                        } else {
                            $scope.nonapplicables = true;
                            $scope.applicables = true;
                        }



                        $scope.examsubjectwise_details = promise.examsubjectwise_details;

                        if (promise.savelist !== null) {
                            $scope.report = true;
                            angular.forEach($scope.clslist, function (itm) {
                                if (parseInt(itm.asmcL_Id) === parseInt($scope.asmcL_Id)) {
                                    $scope.cla = itm.asmcL_ClassName;
                                }
                            });
                            angular.forEach($scope.yearlt, function (itm) {
                                if (parseInt(itm.asmaY_Id) === parseInt($scope.asmaY_Id)) {
                                    $scope.yr = itm.asmaY_Year;
                                }
                            });
                            angular.forEach($scope.seclist, function (itm) {
                                if (parseInt(itm.asmS_Id) == parseInt($scope.asmS_Id)) {
                                    $scope.sec = itm.asmC_SectionName;
                                }
                            });
                            angular.forEach($scope.exsplt, function (itm) {
                                if (parseInt(itm.emE_Id) === parseInt($scope.emE_Id)) {
                                    $scope.exmmid = itm.emE_ExamName;
                                }
                            });



                            //inst name binding
                            $scope.instname = promise.instname;
                            $scope.inst_name = $scope.instname[0].mI_Name;

                            //alert($scope.inst_name);
                            $scope.studentslt = promise.savelist;
                            $scope.studentslt1 = promise.subjlist;

                            console.log("*****************");
                            console.log($scope.studentslt1);

                            console.log("&&&&&&&&&&&&&&&&&&&&");
                            console.log($scope.examsubjectwise_details);

                            angular.forEach($scope.studentslt1, function (dub) {
                                angular.forEach($scope.examsubjectwise_details, function (dd) {
                                    if (parseInt(dub.ismS_Id) === parseInt(dd.ismS_Id)) {
                                        dub.marksdisplay = dd.eyceS_MarksDisplayFlg;
                                        dub.gradedisplay = dd.eyceS_GradeDisplayFlg;
                                    }
                                });
                            });
                            console.log("%%%%%%%%%%%%%%");
                            console.log($scope.studentslt1);

                            $scope.marksgradedisplay = [];

                            angular.forEach($scope.studentslt1, function (sub) {
                                var count = 0;
                                if (sub.marksdisplay === true) {
                                    count += 1;
                                    $scope.marksgradedisplay.push({ subjectid: sub.ismS_Id, columns: "M" });
                                } if (sub.gradedisplay === true) {
                                    count += 1;
                                    $scope.marksgradedisplay.push({ subjectid: sub.ismS_Id, columns: "G" });
                                }
                                sub.colspancount = count;
                            });
                            console.log("^^^^^^^^^^^");
                            console.log($scope.marksgradedisplay);


                            var temp_list = [];
                            for (var x = 0; x < promise.savelist.length; x++) {
                                var stu_id = promise.savelist[x].amsT_Id;
                                var stu_subj_list = [];
                                angular.forEach(promise.savelist, function (opq) {
                                    if (opq.amsT_Id == stu_id) {
                                        stu_subj_list.push({
                                            amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id, ismS_SubjectName: opq.amsT_Id.ismS_SubjectName,
                                            estmpS_MaxMarks: opq.estmpS_MaxMarks, estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks,
                                            estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade, estmpS_PassFailFlg: opq.estmpS_PassFailFlg
                                        });
                                    }
                                });
                                if (temp_list.length === 0) {
                                    temp_list.push({
                                        student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName,
                                        amsT_AdmNo: promise.savelist[x].amsT_AdmNo, classheld: promise.savelist[x].classheld,
                                        amsT_RegistrationNo: promise.savelist[x].amsT_RegistrationNo, amaY_RollNo: promise.savelist[x].amaY_RollNo,
                                        classatt: promise.savelist[x].classatt,
                                        estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_Percentage: promise.savelist[x].estmP_Percentage,
                                        estmP_TotalGrade: promise.savelist[x].estmP_TotalGrade,
                                        estmP_SectionRank: promise.savelist[x].estmP_SectionRank, estmP_ClassRank: promise.savelist[x].estmP_ClassRank,
                                        sub_list: stu_subj_list
                                    });
                                }
                                else if (temp_list.length > 0) {
                                    var already_cnt = 0;
                                    angular.forEach(temp_list, function (opq1) {
                                        if (opq1.student_id == stu_id) {
                                            already_cnt += 1;
                                        }
                                    });
                                    if (already_cnt === 0) {
                                        temp_list.push({
                                            student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName,
                                            amsT_LastName: promise.savelist[x].amsT_LastName, amsT_AdmNo: promise.savelist[x].amsT_AdmNo,
                                            amsT_RegistrationNo: promise.savelist[x].amsT_RegistrationNo, amaY_RollNo: promise.savelist[x].amaY_RollNo,
                                            classheld: promise.savelist[x].classheld, classatt: promise.savelist[x].classatt,
                                            estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_Percentage: promise.savelist[x].estmP_Percentage,
                                            estmP_TotalGrade: promise.savelist[x].estmP_TotalGrade,
                                            estmP_SectionRank: promise.savelist[x].estmP_SectionRank, estmP_ClassRank: promise.savelist[x].estmP_ClassRank,
                                            sub_list: stu_subj_list
                                        });
                                    }
                                }

                            }
                            $scope.studentslt = temp_list;

                            angular.forEach($scope.studentslt, function (oobj) {

                                $scope.tmparrry = [];
                                angular.forEach($scope.studentslt1, function (oobj1) {
                                    var ccount = 0;
                                    angular.forEach(oobj.sub_list, function (oobj2) {
                                        if (oobj1.ismS_Id == oobj2.ismS_Id) {
                                            angular.forEach($scope.examsubjectwise_details, function (sub_det) {
                                                if (sub_det.ismS_Id == oobj2.ismS_Id) {
                                                    oobj2.eyceS_MarksDisplayFlg = sub_det.eyceS_MarksDisplayFlg;
                                                    oobj2.eyceS_GradeDisplayFlg = sub_det.eyceS_GradeDisplayFlg;
                                                }
                                            });
                                            ccount += 1;
                                            if (oobj2.estmpS_PassFailFlg !== 'AB') {

                                                if (oobj2.estmpS_PassFailFlg === 'Fail') {

                                                    if ($scope.masterinst[0].exmConfig_FailBoldFlg === true) {
                                                        oobj2.styleweigh = 'bold';
                                                    } else {
                                                        oobj2.styleweigh = 'normal';
                                                    }

                                                    if ($scope.masterinst[0].exmConfig_FailItalicFlg === true) {
                                                        oobj2.styleitalic = 'italic';
                                                    } else {
                                                        oobj2.styleitalic = 'normal';
                                                    }
                                                    if ($scope.masterinst[0].exmConfig_FailUnderscoreFlg === true) {
                                                        oobj2.styleunderline = 'underline';
                                                    } else {
                                                        oobj2.styleunderline = 'normal';
                                                    }

                                                    if ($scope.masterinst[0].exmConfig_FailColorFlg !== "" && $scope.masterinst[0].exmConfig_FailColorFlg !== undefined && $scope.masterinst[0].exmConfig_FailColorFlg !== null) {
                                                        oobj2.stylecolor = $scope.masterinst[0].exmConfig_FailColorFlg;
                                                    } else {
                                                        oobj2.stylecolor = 'black';
                                                    }
                                                    oobj2.hema_estmpS_ObtainedMarks = oobj2.estmpS_ObtainedMarks;
                                                    oobj2.hema_estmpS_ObtainedGrade = oobj2.estmpS_ObtainedGrade;

                                                } else {
                                                    oobj2.hema_estmpS_ObtainedMarks = oobj2.estmpS_ObtainedMarks;
                                                    oobj2.hema_estmpS_ObtainedGrade = oobj2.estmpS_ObtainedGrade;
                                                    oobj2.styleunderline = 'normal';
                                                    oobj2.styleitalic = 'normal';
                                                    oobj2.styleweigh = 'normal';
                                                    oobj2.stylecolor = 'black';
                                                }
                                            }
                                            else if (oobj2.estmpS_PassFailFlg === 'AB') {
                                                oobj2.hema_estmpS_ObtainedMarks = oobj2.estmpS_PassFailFlg;
                                                oobj2.hema_estmpS_ObtainedGrade = oobj2.estmpS_PassFailFlg;
                                            }
                                            $scope.tmparrry.push(oobj2);
                                        }
                                    });
                                    if (ccount === 0) {
                                        var obj = {};
                                        obj.hema_estmpS_ObtainedMarks = "";
                                        obj.hema_estmpS_ObtainedGrade = "";
                                        $scope.tmparrry.push(obj);
                                    }
                                });
                                oobj.sub_list = $scope.tmparrry;
                            });



                            if (promise.savenonsubjlist !== null && promise.savenonsubjlist.length > 0) {

                                angular.forEach($scope.clslist, function (itm) {
                                    if (parseInt(itm.asmcL_Id) === parseInt($scope.asmcL_Id)) {
                                        $scope.cla = itm.asmcL_ClassName;
                                    }
                                });
                                angular.forEach($scope.yearlt, function (itm) {
                                    if (itm.asmaY_Id == $scope.asmaY_Id) {
                                        $scope.yr = itm.asmaY_Year;
                                    }
                                });
                                angular.forEach($scope.seclist, function (itm) {
                                    if (parseInt(itm.asmS_Id) === parseInt($scope.asmS_Id)) {
                                        $scope.sec = itm.asmC_SectionName;
                                    }
                                });
                                angular.forEach($scope.exsplt, function (itm) {
                                    if (parseInt(itm.emE_Id) === parseInt($scope.emE_Id)) {
                                        $scope.exmmid = itm.emE_ExamName;
                                    }
                                });


                                $scope.electivestd = promise.savenonsubjlist;
                                $scope.electivesub = promise.nonsubjlist;

                                console.log($scope.electivesub);


                                angular.forEach($scope.electivesub, function (dub) {
                                    angular.forEach($scope.examsubjectwise_details, function (dd) {
                                        if (parseInt(dub.ismS_Id) === parseInt(dd.ismS_Id)) {
                                            dub.marksdisplay = dd.eyceS_MarksDisplayFlg;
                                            dub.gradedisplay = dd.eyceS_GradeDisplayFlg;
                                        }
                                    });
                                });

                                console.log("%%%%%%%%%%%%%% Non");
                                console.log($scope.electivesub);

                                $scope.marksgradedisplaynon = [];

                                angular.forEach($scope.electivesub, function (sub) {
                                    var count = 0;
                                    if (sub.marksdisplay === true) {
                                        count += 1;
                                        $scope.marksgradedisplaynon.push({ subjectid: sub.ismS_Id, columns: "M" });
                                    } if (sub.gradedisplay === true) {
                                        count += 1;
                                        $scope.marksgradedisplaynon.push({ subjectid: sub.ismS_Id, columns: "G" });
                                    }
                                    sub.colspancountnon = count;
                                });
                                console.log("^^^^^^^^^^^ NOn");
                                console.log($scope.marksgradedisplaynon);





                                var temp_list = [];
                                for (var x = 0; x < promise.savenonsubjlist.length; x++) {
                                    var stu_id = promise.savenonsubjlist[x].amsT_Id;
                                    var stu_subj_list = [];
                                    angular.forEach(promise.savenonsubjlist, function (opq) {
                                        if (opq.amsT_Id == stu_id) {
                                            stu_subj_list.push({
                                                amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id, ismS_SubjectName: opq.amsT_Id.ismS_SubjectName,
                                                estmpS_MaxMarks: opq.estmpS_MaxMarks, estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks,
                                                estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade, estmpS_PassFailFlg: opq.estmpS_PassFailFlg
                                            });
                                        }
                                    })
                                    if (temp_list.length === 0) {
                                        temp_list.push({
                                            student_id: promise.savenonsubjlist[x].amsT_Id, amsT_FirstName: promise.savenonsubjlist[x].amsT_FirstName,
                                            amsT_LastName: promise.savenonsubjlist[x].amsT_LastName, amsT_AdmNo: promise.savenonsubjlist[x].amsT_AdmNo,
                                            amsT_RegistrationNo: promise.savenonsubjlist[x].amsT_RegistrationNo, amaY_RollNo: promise.savenonsubjlist[x].amaY_RollNo,
                                            classheld: promise.savenonsubjlist[x].classheld, classatt: promise.savenonsubjlist[x].classatt,
                                            estmP_TotalObtMarks: promise.savenonsubjlist[x].estmP_TotalObtMarks, estmP_SectionRank: promise.savenonsubjlist[x].estmP_SectionRank, estmP_ClassRank: promise.savenonsubjlist[x].estmP_ClassRank,
                                            sub_list_e: stu_subj_list
                                        });
                                    }
                                    else if (temp_list.length > 0) {
                                        var already_cnt = 0;
                                        angular.forEach(temp_list, function (opq1) {
                                            if (opq1.student_id == stu_id) {
                                                already_cnt += 1;
                                            }
                                        });
                                        if (already_cnt === 0) {
                                            temp_list.push({
                                                student_id: promise.savenonsubjlist[x].amsT_Id, amsT_FirstName: promise.savenonsubjlist[x].amsT_FirstName,
                                                amsT_LastName: promise.savenonsubjlist[x].amsT_LastName, amsT_AdmNo: promise.savenonsubjlist[x].amsT_AdmNo,
                                                amsT_RegistrationNo: promise.savenonsubjlist[x].amsT_RegistrationNo, amaY_RollNo: promise.savenonsubjlist[x].amaY_RollNo,
                                                classheld: promise.savenonsubjlist[x].classheld, classatt: promise.savenonsubjlist[x].classatt,
                                                estmP_TotalObtMarks: promise.savenonsubjlist[x].estmP_TotalObtMarks, estmP_SectionRank: promise.savenonsubjlist[x].estmP_SectionRank, estmP_ClassRank: promise.savenonsubjlist[x].estmP_ClassRank,
                                                sub_list_e: stu_subj_list
                                            });
                                        }
                                    }
                                }

                                $scope.nonstudentslt = temp_list;
                                $scope.newarray1 = [];

                                angular.forEach($scope.studentslt, function (testobj) {
                                    angular.forEach($scope.electivesub, function (testobjele) {
                                        angular.forEach($scope.nonstudentslt, function (testobj1) {
                                            if (testobj.student_id == testobj1.student_id) {
                                                angular.forEach(testobj1.sub_list, function (testobj2) {
                                                    if (testobjele.ismS_Id == testobj2.ismS_Id) {
                                                        $scope.newarray1.push({
                                                            stuid: testobj.student_id, subid: testobjele.ismS_Id,
                                                            abc: testobj2.estmpS_ObtainedMarks + ' ' + testobj2.estmpS_ObtainedGrade
                                                        });
                                                    }

                                                });
                                            }
                                        });
                                    });
                                });
                            }
                            console.log($scope.studentslt);
                        }


                        else if (promise.savenonsubjlist !== null) {
                            $scope.report = true;

                            angular.forEach($scope.clslist, function (itm) {
                                if (parseInt(itm.asmcL_Id) === parseInt($scope.asmcL_Id)) {
                                    $scope.cla = itm.asmcL_ClassName;
                                }
                            });
                            angular.forEach($scope.yearlt, function (itm) {
                                if (parseInt(itm.asmaY_Id) === parseInt($scope.asmaY_Id)) {
                                    $scope.yr = itm.asmaY_Year;
                                }
                            });
                            angular.forEach($scope.seclist, function (itm) {
                                if (parseInt(itm.asmS_Id) === parseInt($scope.asmS_Id)) {
                                    $scope.sec = itm.asmC_SectionName;
                                }
                            });
                            angular.forEach($scope.exsplt, function (itm) {
                                if (parseInt(itm.emE_Id) === parseInt($scope.emE_Id)) {
                                    $scope.exmmid = itm.emE_ExamName;
                                }
                            });


                            $scope.electivestd = promise.savenonsubjlist;
                            $scope.electivesub = promise.nonsubjlist;

                            angular.forEach($scope.electivesub, function (dub) {
                                angular.forEach($scope.examsubjectwise_details, function (dd) {
                                    if (parseInt(dub.ismS_Id) === parseInt(dd.ismS_Id)) {
                                        dub.marksdisplay = dd.eyceS_MarksDisplayFlg;
                                        dub.gradedisplay = dd.eyceS_GradeDisplayFlg;
                                    }
                                });
                            });

                            $scope.marksgradedisplaynon = [];

                            angular.forEach($scope.electivesub, function (sub) {
                                var count = 0;
                                if (sub.marksdisplay === true) {
                                    count += 1;
                                    $scope.marksgradedisplaynon.push({ subjectid: sub.ismS_Id, columns: "M" });
                                } if (sub.gradedisplay === true) {
                                    count += 1;
                                    $scope.marksgradedisplaynon.push({ subjectid: sub.ismS_Id, columns: "G" });
                                }
                                sub.colspancountnon = count;
                            });
                            console.log("^^^^^^^^^^^ NOn");
                            console.log($scope.marksgradedisplaynon);

                            var temp_list = [];
                            for (var x = 0; x < promise.savenonsubjlist.length; x++) {
                                var stu_id = promise.savenonsubjlist[x].amsT_Id;
                                var stu_subj_list = [];
                                angular.forEach(promise.savenonsubjlist, function (opq) {
                                    if (opq.amsT_Id == stu_id) {
                                        stu_subj_list.push({
                                            amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id, ismS_SubjectName: opq.amsT_Id.ismS_SubjectName,
                                            estmpS_MaxMarks: opq.estmpS_MaxMarks, estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks,
                                            estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade, estmpS_PassFailFlg: opq.estmpS_PassFailFlg
                                        });
                                    }
                                });
                                if (temp_list.length === 0) {
                                    temp_list.push({
                                        student_id: promise.savenonsubjlist[x].amsT_Id, amsT_FirstName: promise.savenonsubjlist[x].amsT_FirstName,
                                        amsT_LastName: promise.savenonsubjlist[x].amsT_LastName, amsT_AdmNo: promise.savenonsubjlist[x].amsT_AdmNo,
                                        amsT_RegistrationNo: promise.savenonsubjlist[x].amsT_RegistrationNo, amaY_RollNo: promise.savenonsubjlist[x].amaY_RollNo,
                                        classheld: promise.savenonsubjlist[x].classheld, classatt: promise.savenonsubjlist[x].classatt,
                                        estmP_TotalObtMarks: promise.savenonsubjlist[x].estmP_TotalObtMarks, estmP_SectionRank: promise.savenonsubjlist[x].estmP_SectionRank, estmP_ClassRank: promise.savenonsubjlist[x].estmP_ClassRank,
                                        sub_list_e: stu_subj_list
                                    });
                                }
                                else if (temp_list.length > 0) {
                                    var already_cnt = 0;
                                    angular.forEach(temp_list, function (opq1) {
                                        if (opq1.student_id == stu_id) {
                                            already_cnt += 1;
                                        }
                                    });
                                    if (already_cnt === 0) {
                                        temp_list.push({
                                            student_id: promise.savenonsubjlist[x].amsT_Id, amsT_FirstName: promise.savenonsubjlist[x].amsT_FirstName,
                                            amsT_LastName: promise.savenonsubjlist[x].amsT_LastName, amsT_AdmNo: promise.savenonsubjlist[x].amsT_AdmNo,
                                            amsT_RegistrationNo: promise.savenonsubjlist[x].amsT_RegistrationNo, amaY_RollNo: promise.savenonsubjlist[x].amaY_RollNo,
                                            classheld: promise.savenonsubjlist[x].classheld, classatt: promise.savenonsubjlist[x].classatt,
                                            estmP_TotalObtMarks: promise.savenonsubjlist[x].estmP_TotalObtMarks, estmP_SectionRank: promise.savenonsubjlist[x].estmP_SectionRank, estmP_ClassRank: promise.savenonsubjlist[x].estmP_ClassRank,
                                            sub_list_e: stu_subj_list
                                        });
                                    }
                                }

                            }
                            $scope.nonstudentslt = temp_list;
                            $scope.newarray1 = [];
                            $scope.studentslt = $scope.nonstudentslt;
                            angular.forEach($scope.studentslt, function (testobj) {
                                angular.forEach($scope.electivesub, function (testobjele) {
                                    angular.forEach($scope.nonstudentslt, function (testobj1) {
                                        if (testobj.student_id == testobj1.student_id) {
                                            angular.forEach(testobj1.sub_list_e, function (testobj2) {
                                                if (testobjele.ismS_Id == testobj2.ismS_Id) {
                                                    $scope.newarray1.push({
                                                        stuid: testobj.student_id, subid: testobjele.ismS_Id,
                                                        abc: testobj2.estmpS_ObtainedMarks + ' ' + testobj2.estmpS_ObtainedGrade
                                                    });
                                                }

                                            });
                                        }
                                    });
                                });
                            });
                        }

                        else if (promise.savenonsubjlist === null && promise.savelist === null) {
                            swal('No record Found');
                        }

                        angular.forEach($scope.studentslt, function (oobj) {
                            angular.forEach($scope.nonstudentslt, function (oobj2) {
                                if (oobj2.student_id == oobj.student_id) {
                                    $scope.tmparrry1 = [];
                                    angular.forEach($scope.electivesub, function (oobj1) {
                                        var ccount1 = 0;
                                        angular.forEach(oobj2.sub_list_e, function (oobj3) {
                                            // ccount1 += 1;
                                            if (oobj1.ismS_Id == oobj3.ismS_Id) {
                                                angular.forEach($scope.examsubjectwise_details, function (sub_det) {
                                                    if (sub_det.ismS_Id == oobj3.ismS_Id) {
                                                        oobj3.eyceS_MarksDisplayFlg = sub_det.eyceS_MarksDisplayFlg;
                                                        oobj3.eyceS_GradeDisplayFlg = sub_det.eyceS_GradeDisplayFlg;
                                                    }
                                                });
                                                ccount1 += 1;
                                                if (oobj3.estmpS_PassFailFlg !== 'AB') {


                                                    if (oobj3.estmpS_PassFailFlg === 'Fail') {

                                                        if ($scope.masterinst[0].exmConfig_FailBoldFlg === true) {
                                                            oobj3.styleweigh1 = 'bold';
                                                        } else {
                                                            oobj3.styleweigh1 = 'normal';
                                                        }

                                                        if ($scope.masterinst[0].exmConfig_FailItalicFlg === true) {
                                                            oobj3.styleitalic1 = 'italic';
                                                        } else {
                                                            oobj3.styleitalic1 = 'normal';
                                                        }
                                                        if ($scope.masterinst[0].exmConfig_FailUnderscoreFlg === true) {
                                                            oobj3.styleunderline1 = 'underline';
                                                        } else {
                                                            oobj3.styleunderline1 = 'normal';
                                                        }

                                                        if ($scope.masterinst[0].exmConfig_FailColorFlg !== ""
                                                            && $scope.masterinst[0].exmConfig_FailColorFlg !== undefined
                                                            && $scope.masterinst[0].exmConfig_FailColorFlg !== null) {
                                                            oobj3.stylecolor1 = $scope.masterinst[0].exmConfig_FailColorFlg;
                                                        } else {
                                                            oobj3.stylecolor1 = 'black';
                                                        }
                                                        oobj3.hema_estmpS_ObtainedMarks = oobj3.estmpS_ObtainedMarks;
                                                        oobj3.hema_estmpS_ObtainedGrade = oobj3.estmpS_ObtainedGrade;
                                                    } else {
                                                        oobj3.hema_estmpS_ObtainedMarks = oobj3.estmpS_ObtainedMarks;
                                                        oobj3.hema_estmpS_ObtainedGrade = oobj3.estmpS_ObtainedGrade;
                                                        oobj3.styleunderline1 = 'normal';
                                                        oobj3.styleitalic1 = 'normal';
                                                        oobj3.styleweigh1 = 'normal';
                                                        oobj3.stylecolor1 = 'black';
                                                    }

                                                }
                                                else if (oobj3.estmpS_PassFailFlg === 'AB') {
                                                    oobj3.hema_estmpS_ObtainedMarks = oobj3.estmpS_PassFailFlg;
                                                    oobj3.hema_estmpS_ObtainedGrade = oobj3.estmpS_PassFailFlg;
                                                }
                                                $scope.tmparrry1.push(oobj3);
                                            }
                                        });

                                        if (ccount1 === 0) {
                                            var obj1 = {};
                                            obj1.hema_estmpS_ObtainedMarks = "";
                                            obj1.hema_estmpS_ObtainedGrade = "";
                                            $scope.tmparrry1.push(obj1);
                                        }
                                    });
                                    oobj.sub_list_e = $scope.tmparrry1;
                                }
                            });
                        });
                    });
            }

            console.log($scope.studentslt);
        };

        $scope.printToCart = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportBBPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.cancel = function () {
            $scope.asmcL_Id = ""
            $scope.emcA_Id = ""
            $scope.asmaY_Id = ""
            $scope.emG_Id = ""
            $scope.asmS_Id = ""
            $scope.subjectlt = ""
            $scope.subjectlt1 = ""
            $scope.studentlist = false;
            $state.reload();
        }

        $scope.toggleAll = function () {

            var toggleStatus = $scope.all;
            angular.forEach($scope.subjectlt1, function (itm) {
                itm.xyz = toggleStatus;

            });
        }

        $scope.optionToggled = function (chk_box) {
            $scope.all = $scope.subjectlt1.every(function (itm) { return itm.xyz; })
        }

        //$scope.OnAcdyear = function () {

        //    $scope.asmcL_Id = "";
        //    $scope.asmS_Id = "";
        //    $scope.emE_Id = "";
        //    $scope.report = false;

        //    var data = {
        //        "ASMAY_Id": $scope.asmaY_Id
        //    };

        //    apiService.create("CumulativeReport/onchangeyear", data).then(function (promise) {

        //        if (promise != null) {
        //            $scope.clslist = promise.classlist;
        //            if ($scope.clslist.length == 0) {
        //                swal("No Class Is Mapped For Selected Academic Year");
        //            }
        //        } else {
        //            swal("No Class Is Mapped For Selected Academic Year");
        //        }

        //    });
        //};

        //$scope.onchangeclass = function () {

        //    $scope.asmS_Id = "";
        //    $scope.emE_Id = "";
        //    $scope.report = false;

        //    var data = {
        //        "ASMAY_Id": $scope.asmaY_Id,
        //        "ASMCL_Id": $scope.asmcL_Id
        //    };

        //    apiService.create("CumulativeReport/onchangeclass", data).then(function (promise) {

        //        if (promise != null) {
        //            $scope.seclist = promise.seclist;
        //            if ($scope.seclist.length == 0) {
        //                swal("No Section Is Mapped For Selected Class");
        //            }

        //        } else {
        //            swal("No Section Is Mapped For Selected Class");
        //        }

        //    });
        //};

        //$scope.onchangesection = function () {

        //    $scope.emE_Id = "";
        //    $scope.report = false;

        //    var data = {
        //        "ASMAY_Id": $scope.asmaY_Id,
        //        "ASMCL_Id": $scope.asmcL_Id,
        //        "ASMS_Id": $scope.asmS_Id
        //    };

        //    apiService.create("CumulativeReport/onchangesection", data).then(function (promise) {

        //        if (promise != null) {
        //            $scope.exsplt = promise.exmstdlist;
        //            if ($scope.exsplt.length == 0) {
        //                swal("No Exam Is Mapped For Selected Details ");
        //            }
        //        } else {
        //            swal("No Exam Is Mapped For Selected Details");
        //        }

        //    });
        //};

        $scope.onchangeclass = function () {

            if ($scope.asmaY_Id != "" && $scope.asmaY_Id != undefined && $scope.asmaY_Id != null) {

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    //clssids: $scope.selectedclasses,
                }

                // apiService.getURI("CategoryClassMapp/getdetails", pageid).

                apiService.create("ExamCalculation_SSSE/get_cls_sections", data).
                    then(function (promise) {


                        $scope.seclist = promise.seclist;
                        $scope.asmS_Id = "";
                        $scope.emE_Id = "";
                        $scope.exsplt = [];
                        // $scope.exsplt = promise.exmstdlist;


                        // if ((promise.seclist == null || promise.seclist == "") && (promise.exmstdlist == null || promise.exmstdlist == "")) {
                        //     swal("Sections And Exams are Not Mapped To Selected Class!!!");
                        // }

                        //else 
                        if (promise.seclist == null || promise.seclist == "") {
                            swal("Sections are Not Mapped To Selected Class!!!");
                        }
                        //else if (promise.exmstdlist == null || promise.exmstdlist == "") {
                        //    swal("Exams are Not Mapped To Selected Class !!!");
                        //}

                    })
            }
            else {

                swal("Please Select Academic Year  First !!!");
                $scope.asmcL_Id = "";

            }
        }

        $scope.OnAcdyear = function (ASMAY_Id) {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ExamCalculation_SSSE/get_classes", data).
                then(function (promise) {
                    $scope.clslist = promise.classlist;
                    $scope.asmcL_Id = "";
                    $scope.asmS_Id = "";
                    $scope.emE_Id = "";
                    $scope.seclist = [];
                    $scope.exsplt = [];
                    if (promise.classlist == null || promise.classlist == "") {
                        swal("Classes are Not Mapped To Selected Academic Year!!!");
                    }

                })
        };

        $scope.onchangesection = function (ASMAY_Id, ASMCL_Id, ASMS_Id) {
            var data = {
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ExamCalculation_SSSE/get_exams", data).
                then(function (promise) {

                    $scope.emE_Id = "";
                    $scope.exsplt = promise.exmstdlist;
                    if (promise.exmstdlist == null || promise.exmstdlist == "") {
                        swal("Exams are Not Mapped To Selected Class And Section!!!");
                    }
                })


        };
    }

})();