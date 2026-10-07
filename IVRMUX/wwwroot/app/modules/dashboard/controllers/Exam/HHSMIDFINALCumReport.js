(function () {
    'use strict';
    angular
        .module('app')
        .controller('HHSMIDFINALCumReportController', HHSMIDFINALCumReportController)
    HHSMIDFINALCumReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function HHSMIDFINALCumReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

        $scope.printbtn = false;
        $scope.exclbtn = false;



        $scope.colarraytmp = [];
        $scope.repoershow = false;

        // create the list of themes  
        $scope.fonts = [
            { name: '10px', size: '10px', class: 'font10' },
            { name: '11px', size: '11px', class: 'font11' },
            { name: '12px', size: '12px', class: 'font12' },
            { name: '13px', size: '13px', class: 'font13' },
            { name: '14px', size: '14px', class: 'font14' },
            { name: '15px', size: '15px', class: 'font15' },
            { name: '16px', size: '16px', class: 'font16' },
            { name: '17px', size: '17px', class: 'font17' },
            { name: '18px', size: '18px', class: 'font18' },
            { name: '25px', size: '25px', class: 'font25' }
        ];
        $scope.font_size = 'font25';
        $scope.size = '10px';


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;

        //TO  GEt The Values iN Grid
        $scope.BindData = function () {

            apiService.getDATA("HHSMIDFINALCumReport/Getdetails").
                then(function (promise) {
                    $scope.yearlt = promise.yearlist;
                    $scope.clslist = promise.classlist;
                    $scope.seclist = promise.seclist;
                    $scope.amstlt = promise.amstlist;
                    $scope.exsplt = promise.exmstdlist;
                    //$scope.gridOptions.data = promise.studmaplist;
                });
        };

        //Record pop up 
        $scope.viewrecordspopup = function (employee, SweetAlert) {

            $scope.editEmployee = employee.amsT_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("HHSMIDFINALCumReport/getalldetailsviewrecords", pageid).
                then(function (promise) {
                    $scope.viewrecordspopupdisplay = promise.gtdetailsview
                    //$scope.Group_Name = promise.view_group_subjects[0].emG_GroupName;
                    //$scope.viewrecordspopupdisplay = promise.view_group_subjects;
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

        $scope.subchkbx = function (column, obj, user) {
            //$scope.gridview2 = true;                
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
            $scope.exmrank = [];
            $scope.printbtn = false;
            $scope.exclbtn = false;
            $scope.repoershow = false;
            $scope.electivestd = [];
            $scope.electivesub = [];
            $scope.submitted = true;
            $scope.colarrayall = [];

            if ($scope.myForm.$valid) {
                var data = {
                    "EME_ID": $scope.emE_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("HHSMIDFINALCumReport/savedetails", data).
                    then(function (promise) {

                        $scope.masterinst = promise.configuration;
                        var count = 0;
                        if ($scope.masterinst.length > 0) {
                            $scope.colarrayall = [{ title: "SLNO", template: "<span class='row-number'></span>", width: 100 },
                            { name: 'amsT_FirstName', field: 'amsT_FirstName', title: 'Student Name', width: 200 }];

                            if ($scope.masterinst[0].exmConfig_RegnoColumnDisplay === true) {

                                $scope.colarrayall.push({ name: 'amsT_RegistrationNo', field: 'amsT_RegistrationNo', title: 'Reg.NO', width: 100 });

                                $scope.regno = true;
                                count = count + 1;

                            } else {
                                $scope.regno = false;
                            }

                            if ($scope.masterinst[0].exmConfig_AdmnoColumnDisplay === true) {

                                $scope.colarrayall.push({ name: 'amsT_AdmNo', field: 'amsT_AdmNo', title: 'Adm.NO', width: 100 });

                                $scope.admno = true;
                                count = count + 1;

                            } else {
                                $scope.admno = false;
                            }

                            if ($scope.masterinst[0].exmConfig_RollnoColumnDisplay === true) {

                                $scope.colarrayall.push({ name: 'amaY_RollNo', field: 'amaY_RollNo', title: 'Roll.NO', width: 100 });

                                $scope.rollno = true;
                                count = count + 1;

                            } else {
                                $scope.rollno = false;
                            }

                            if (count === 0) {

                                $scope.colarrayall.push({ name: 'amsT_AdmNo', field: 'amsT_AdmNo', title: 'Adm.NO', width: 100 });
                                $scope.colarrayall.push({ name: 'amaY_RollNo', field: 'amaY_RollNo', title: 'Roll.NO', width: 100 });

                                $scope.admno = true;
                                $scope.rollno = true;
                            }

                        } else {

                            $scope.colarrayall.push({ name: 'amsT_AdmNo', field: 'amsT_AdmNo', title: 'Adm.NO', width: 100 });
                            $scope.colarrayall.push({ name: 'amaY_RollNo', field: 'amaY_RollNo', title: 'Roll.NO', width: 100 });

                            $scope.admno = true;
                            $scope.rollno = true;
                        }

                        if (promise.savelist !== null) {
                            $scope.repoershow = true;
                            angular.forEach($scope.clslist, function (itm) {
                                if (itm.asmcL_Id == $scope.asmcL_Id) {
                                    $scope.cla = itm.asmcL_ClassName;
                                }
                            });
                            angular.forEach($scope.yearlt, function (itm) {
                                if (itm.asmaY_Id == $scope.asmaY_Id) {
                                    $scope.yr = itm.asmaY_Year;
                                }
                            });
                            angular.forEach($scope.seclist, function (itm) {
                                if (itm.asmS_Id == $scope.asmS_Id) {
                                    $scope.sec = itm.asmC_SectionName;
                                }
                            });
                            angular.forEach($scope.exsplt, function (itm) {
                                if (itm.emE_Id == $scope.emE_Id) {
                                    $scope.exmmid = itm.emE_ExamName;
                                }
                            });

                            $scope.printbtn = true;
                            $scope.exclbtn = true;
                            //inst name binding
                            $scope.instname = promise.instname;
                            $scope.inst_name = $scope.instname[0].mI_Name;
                            $scope.add = $scope.instname[0].mI_Address1;
                            $scope.city = $scope.instname[0].ivrmmcT_Name;
                            $scope.pin = $scope.instname[0].mI_Pincode;

                            $scope.studlist = promise.studlist;
                            $scope.savelist = promise.savelist;
                            $scope.subwithsubexm = promise.subwithsubexm;
                            $scope.electivemarks = promise.electivemarks;
                            $scope.electivestd = promise.savenonsubjlist;
                            $scope.electivesub = promise.nonsubjlist;
                            $scope.exmrank = promise.exmrank;


                            console.log($scope.exmrank);

                            $scope.details = [];
                            angular.forEach($scope.studlist, function (std) {
                                var mrklist = [];
                                var ellist = [];
                                angular.forEach($scope.subwithsubexm, function (exm) {
                                    if (exm.eyceS_AplResultFlg == true) {
                                        var obtn = '';
                                        var max = '';
                                        var fff = '';
                                        var cnttt = 0;
                                        var obtngrade = '';
                                        angular.forEach($scope.savelist, function (mrk) {
                                            if (mrk.amsT_Id == std.amsT_Id && exm.ismS_Id == mrk.ismS_Id && exm.emsE_Id == mrk.emsE_Id) {
                                                cnttt += 1;
                                                obtn = mrk.estmpS_ObtainedMarks;
                                                max = mrk.estmpS_MaxMarks;
                                                fff = mrk.estmpS_PassFailFlg;
                                                obtngrade = mrk.estmpS_ObtainedGrade;
                                            }
                                        });
                                        if (cnttt > 0) {
                                            mrklist.push({ ismS_Id: exm.ismS_Id, emsE_Id: exm.emsE_Id, emsE_SubExamName: exm.emsE_SubExamName, estmpS_ObtainedMarks: obtn, estmpS_MaxMarks: max, estmpS_PassFailFlg: fff, flag: true, eyceS_GradeDisplayFlg: exm.eyceS_GradeDisplayFlg, eyceS_MarksDisplayFlg: exm.eyceS_MarksDisplayFlg, estmpS_ObtainedGrade: obtngrade })
                                        }
                                        if (cnttt == 0) {
                                            mrklist.push({ ismS_Id: exm.ismS_Id, emsE_Id: exm.emsE_Id, emsE_SubExamName: exm.emsE_SubExamName, estmpS_ObtainedMarks: obtn, estmpS_MaxMarks: max, estmpS_PassFailFlg: fff, flag: true, eyceS_GradeDisplayFlg: exm.eyceS_GradeDisplayFlg, eyceS_MarksDisplayFlg: exm.eyceS_MarksDisplayFlg, estmpS_ObtainedGrade: obtngrade })
                                        }
                                    }
                                });
                                var bb = 0;
                                angular.forEach($scope.subwithsubexm, function (exm1) {
                                    if (exm1.eyceS_AplResultFlg == false) {
                                        angular.forEach($scope.electivemarks, function (el) {
                                            if (el.amsT_Id == std.amsT_Id) {
                                                bb += 1;
                                            }
                                            if (el.amsT_Id == std.amsT_Id && exm1.ismS_Id == el.ismS_Id) {
                                                ellist.push({ ismS_Id: el.ismS_Id, ismS_SubjectName: el.ismS_SubjectName, emsE_Id: el.emsE_Id, estmpS_ObtainedMarks: el.estmpS_ObtainedMarks, estmpS_MaxMarks: el.estmpS_MaxMarks, estmpS_PassFailFlg: el.estmpS_PassFailFlg, flag: false, eyceS_GradeDisplayFlg: exm1.eyceS_GradeDisplayFlg, eyceS_MarksDisplayFlg: exm1.eyceS_MarksDisplayFlg, estmpS_ObtainedGrade: el.estmpS_ObtainedGrade });
                                            }
                                        });
                                        if (bb == 0) {
                                            ellist.push({ ismS_Id: exm1.ismS_Id, ismS_SubjectName: exm1.ismS_SubjectName, emsE_Id: '', estmpS_ObtainedMarks: '', estmpS_MaxMarks: '', estmpS_PassFailFlg: '', flag: false, eyceS_GradeDisplayFlg: exm1.eyceS_GradeDisplayFlg, eyceS_MarksDisplayFlg: exm1.eyceS_MarksDisplayFlg, estmpS_ObtainedGrade: '' });
                                        }
                                    }
                                });
                                var estmP_TotalMaxMarks = '';
                                var estmP_TotalObtMarks = '';
                                var estmP_Percentage = '';
                                var estmP_SectionRank = '';
                                var estmP_Result = '';
                                var estmP_TotalGrade = '';

                                if ($scope.exmrank.length === 0) {
                                    $scope.details.push({
                                        amsT_Id: std.amsT_Id, amsT_FirstName: std.amsT_FirstName, amsT_AdmNo: std.amsT_AdmNo,
                                        amaY_RollNo: std.amaY_RollNo, amsT_RegistrationNo: std.amsT_RegistrationNo,
                                        stmrklist: mrklist, elmrklist: ellist, estmP_TotalObtMarks: estmP_TotalObtMarks, estmP_TotalMaxMarks: estmP_TotalMaxMarks, estmP_Percentage: estmP_Percentage, estmP_SectionRank: estmP_SectionRank, estmP_Result: estmP_Result, estmP_TotalGrade: estmP_TotalGrade
                                    });
                                }
                                //$scope.details.push({ amsT_FirstName: std.amsT_FirstName, amsT_AdmNo: std.amsT_AdmNo, amaY_RollNo: std.amaY_RollNo, stmrklist: mrklist, elmrklist: ellist })

                                var cmm = 0;
                                angular.forEach($scope.exmrank, function (rr) {
                                    if (rr.amsT_Id == std.amsT_Id) {
                                        cmm += 1;
                                        $scope.details.push({
                                            amsT_Id: rr.amsT_Id, amsT_FirstName: std.amsT_FirstName, amsT_AdmNo: std.amsT_AdmNo,
                                            amaY_RollNo: std.amaY_RollNo, amsT_RegistrationNo: std.amsT_RegistrationNo,
                                            stmrklist: mrklist, elmrklist: ellist, estmP_TotalObtMarks: rr.estmP_TotalObtMarks, estmP_TotalMaxMarks: rr.estmP_TotalMaxMarks, estmP_Percentage: rr.estmP_Percentage, estmP_SectionRank: rr.estmP_SectionRank, estmP_Result: rr.estmP_Result, estmP_TotalGrade: rr.estmP_TotalGrade
                                        });
                                    }
                                });
                                if (cmm === 0) {
                                    $scope.details.push({
                                        amsT_Id: std.amsT_Id, amsT_FirstName: std.amsT_FirstName, amsT_AdmNo: std.amsT_AdmNo,
                                        amaY_RollNo: std.amaY_RollNo, amsT_RegistrationNo: std.amsT_RegistrationNo,
                                        stmrklist: mrklist, elmrklist: ellist, estmP_TotalObtMarks: estmP_TotalObtMarks, estmP_TotalMaxMarks: estmP_TotalMaxMarks, estmP_Percentage: estmP_Percentage, estmP_SectionRank: estmP_SectionRank, estmP_Result: estmP_Result, estmP_TotalGrade: estmP_TotalGrade
                                    });
                                }
                            });

                            //getting all subjects
                            $scope.mainsubjects = [];
                            angular.forEach($scope.subwithsubexm, function (stu2) {
                                if ($scope.mainsubjects.length == 0) {
                                    $scope.mainsubjects.push({ ismS_Id: stu2.ismS_Id, ismS_SubjectName: stu2.ismS_SubjectName, eyceS_AplResultFlg: stu2.eyceS_AplResultFlg, eyceS_MarksDisplayFlg: stu2.eyceS_MarksDisplayFlg, eyceS_GradeDisplayFlg: stu2.eyceS_GradeDisplayFlg });
                                }
                                else if ($scope.mainsubjects.length > 0) {
                                    var al_ct = 0;
                                    angular.forEach($scope.mainsubjects, function (uf) {
                                        if (uf.ismS_Id == stu2.ismS_Id) {
                                            al_ct += 1;
                                        }
                                    });
                                    if (al_ct == 0) {
                                        $scope.mainsubjects.push({ ismS_Id: stu2.ismS_Id, ismS_SubjectName: stu2.ismS_SubjectName, eyceS_AplResultFlg: stu2.eyceS_AplResultFlg, eyceS_MarksDisplayFlg: stu2.eyceS_MarksDisplayFlg, eyceS_GradeDisplayFlg: stu2.eyceS_GradeDisplayFlg });
                                    }
                                }
                            });
                            //getting sub exam for all subjects
                            $scope.headersub = [];
                            angular.forEach($scope.mainsubjects, function (s1) {
                                var temp1 = [];
                                angular.forEach($scope.subwithsubexm, function (e1) {

                                    if (s1.ismS_Id == e1.ismS_Id) {
                                        temp1.push({ emsE_Id: e1.emsE_Id, emsE_SubExamName: e1.emsE_SubExamName, ismS_Id: e1.ismS_Id, ismS_SubjectName: e1.ismS_SubjectName });
                                    }

                                });
                                $scope.headersub.push({ ismS_Id: s1.ismS_Id, ismS_SubjectName: s1.ismS_SubjectName, eyceS_AplResultFlg: s1.eyceS_AplResultFlg, eyceS_MarksDisplayFlg: s1.eyceS_MarksDisplayFlg, eyceS_GradeDisplayFlg: s1.eyceS_GradeDisplayFlg, subexmlist: temp1 });

                            });
                            $scope.colarraytmp = [];
                            //kendogrid start
                            angular.forEach($scope.headersub, function (obj1) {
                                if (obj1.eyceS_AplResultFlg === true) {
                                    //obj.title = obj1.ismS_SubjectName;
                                    $scope.colarraytmp.push({ title: obj1.ismS_SubjectName, field: obj1.ismS_SubjectName, idd: obj1.ismS_Id, marksflg: obj1.eyceS_MarksDisplayFlg, grdflag: obj1.eyceS_GradeDisplayFlg })
                                }
                            });

                            angular.forEach($scope.colarraytmp, function (qwe) {
                                $scope.nwtmpary = [];
                                angular.forEach($scope.subwithsubexm, function (obj2) {
                                    if (obj2.eyceS_AplResultFlg == true && qwe.title == obj2.ismS_SubjectName) {
                                        // obj.title = obj2.emsE_SubExamName;
                                        $scope.nwtmpary.push({
                                            title: obj2.emsE_SubExamName, field: "unique" + qwe.idd + obj2.emsE_SubExamName, width: 200
                                        });
                                    }
                                });
                                qwe.columns = $scope.nwtmpary;
                            });

                            //angular.forEach($scope.colarraytmp, function (qwe) {
                            //    $scope.colarrayall.push(qwe);                               
                            //});
                            $scope.grade = [
                                { id: 1, name: 'M' },
                                { id: 2, name: 'G' }
                            ]
                            console.log("Gradeeeeeeeeeeeee");
                            console.log($scope.colarraytmp);
                            angular.forEach($scope.colarraytmp, function (cols) {
                                angular.forEach(cols.columns, function (cobs) {
                                    $scope.marksgrade = [];
                                    angular.forEach($scope.grade, function (grd) {

                                        if (cols.marksflg == true) {
                                            if (grd.name == 'M') {
                                                $scope.marksgrade.push({
                                                    title: grd.name, field: cobs.field + grd.name, width: 100
                                                });
                                            }
                                        }
                                        if (cols.grdflag == true) {
                                            if (grd.name == 'G') {
                                                $scope.marksgrade.push({
                                                    title: grd.name, field: cobs.field + grd.name, width: 100
                                                });
                                            }
                                        }

                                        cobs.columns = $scope.marksgrade;
                                    })
                                });
                            })
                            angular.forEach($scope.colarraytmp, function (qwe) {
                                $scope.colarrayall.push(qwe);
                            });
                            console.log("Graddddddddddddddddddddddddddddddd");
                            console.log($scope.colarraytmp);

                            $scope.colarrayall.push({ title: 'Over All Total', name: 'estmP_TotalObtMarks', field: 'estmP_TotalObtMarks', width: 100 },
                                { title: 'Over All Grade', name: 'estmP_TotalGrade', field: 'estmP_TotalGrade', width: 100 },
                                { title: 'Max.Total', name: 'estmP_TotalMaxMarks', field: 'estmP_TotalMaxMarks', width: 100 },
                                { title: 'PER.(%)', name: 'estmP_Percentage', field: 'estmP_Percentage', width: 100 },
                                { title: 'Rank', name: 'estmP_SectionRank', field: 'estmP_SectionRank', width: 100 },
                                { title: 'PASS/FAIL', name: 'estmP_Result', field: 'estmP_Result', width: 100 });

                            angular.forEach($scope.headersub, function (obj1) {
                                if (obj1.eyceS_AplResultFlg === false) {
                                    var strstr = '["' + obj1.ismS_SubjectName + '"]';
                                    $scope.colarrayall.push({ title: obj1.ismS_SubjectName, field: strstr, name: strstr, width: 100 });
                                }
                            });

                            var abc = 'M';
                            var abc1 = 'G';
                            angular.forEach($scope.details, function (qqq) {
                                angular.forEach(qqq.stmrklist, function (qqq1) {
                                    var s = "unique" + qqq1.ismS_Id + qqq1.emsE_SubExamName;
                                    var s1 = "unique" + qqq1.ismS_Id + qqq1.emsE_SubExamName;
                                    if ((qqq1.estmpS_PassFailFlg === 'AB' || qqq1.estmpS_PassFailFlg === 'M' || qqq1.estmpS_PassFailFlg === 'L') && qqq1.emsE_SubExamName !== 'Total') {


                                        if (qqq1.eyceS_MarksDisplayFlg === true && qqq1.eyceS_GradeDisplayFlg === false) {
                                            if (abc == 'M') {
                                                s = s + 'M';
                                                qqq[s] = qqq1.estmpS_PassFailFlg;
                                            }
                                        }
                                        else if (qqq1.eyceS_MarksDisplayFlg === false && qqq1.eyceS_GradeDisplayFlg === true) {
                                            if (abc1 == 'G') {
                                                s1 = s1 + 'G';
                                                qqq[s1] = qqq1.estmpS_PassFailFlg;
                                            }
                                        }
                                        else if (qqq1.eyceS_MarksDisplayFlg === true && qqq1.eyceS_GradeDisplayFlg === true) {
                                            if (abc == 'M') {
                                                s = s + 'M';
                                                qqq[s] = qqq1.estmpS_PassFailFlg;
                                            }
                                            if (abc1 == 'G') {
                                                s1 = s1 + 'G';
                                                qqq[s1] = qqq1.estmpS_PassFailFlg;
                                            }
                                        }




                                    }
                                    else if ((qqq1.estmpS_PassFailFlg === 'AB' || qqq1.estmpS_PassFailFlg === 'M' || qqq1.estmpS_PassFailFlg === 'L') && qqq1.emsE_SubExamName === 'Total') {


                                        if (qqq1.eyceS_MarksDisplayFlg === true && qqq1.eyceS_GradeDisplayFlg === false) {
                                            if (abc == 'M') {
                                                s = s + 'M';
                                                qqq[s] = qqq1.estmpS_PassFailFlg + "(" + qqq1.estmpS_ObtainedMarks + ")";
                                            }
                                        }
                                        else if (qqq1.eyceS_MarksDisplayFlg === false && qqq1.eyceS_GradeDisplayFlg === true) {
                                            if (abc1 == 'G') {
                                                s1 = s1 + 'G';
                                                qqq[s1] = qqq1.estmpS_PassFailFlg + "(" + qqq1.estmpS_ObtainedGrade + ")";
                                            }
                                        }
                                        else if (qqq1.eyceS_MarksDisplayFlg === true && qqq1.eyceS_GradeDisplayFlg === true) {
                                            if (abc == 'M') {
                                                s = s + 'M';
                                                qqq[s] = qqq1.estmpS_PassFailFlg + "(" + qqq1.estmpS_ObtainedMarks + ")";
                                            }
                                            if (abc1 == 'G') {
                                                s1 = s1 + 'G';
                                                qqq[s1] = qqq1.estmpS_PassFailFlg + "(" + qqq1.estmpS_ObtainedGrade + ")";
                                            }
                                        }





                                    }
                                    else if (qqq1.estmpS_PassFailFlg !== 'AB' && qqq1.estmpS_PassFailFlg !== 'M' && qqq1.estmpS_PassFailFlg !== 'L') {

                                        if (qqq1.estmpS_PassFailFlg === 'Fail') {

                                            if (qqq1.eyceS_MarksDisplayFlg === true && qqq1.eyceS_GradeDisplayFlg === false) {
                                                if (abc == 'M') {
                                                    s = s + 'M';
                                                    qqq[s] = qqq1.estmpS_ObtainedMarks + " * ";
                                                }
                                                //if (abc1 == 'G') {
                                                //    s1 = s1 + 'G';
                                                //    qqq[s1] = qqq1.estmpS_ObtainedGrade + " * ";
                                                //}


                                            } else if (qqq1.eyceS_MarksDisplayFlg === false && qqq1.eyceS_GradeDisplayFlg === true) {
                                                //if (abc == 'M') {
                                                //    s = s + 'M';
                                                //    qqq[s] = qqq1.estmpS_ObtainedGrade + " * ";
                                                //}
                                                if (abc1 == 'G') {
                                                    s1 = s1 + 'G';
                                                    qqq[s1] = qqq1.estmpS_ObtainedGrade + " * ";
                                                }

                                            } else if (qqq1.eyceS_MarksDisplayFlg === false && qqq1.eyceS_GradeDisplayFlg === false) {
                                                //if (abc == 'M') {
                                                //    s = s + 'M';
                                                //    qqq[s] = qqq1.estmpS_ObtainedMarks + " * ";
                                                //}
                                                //if (abc1 == 'G') {
                                                //    s1 = s1 + 'G';
                                                //    qqq[s1] = qqq1.estmpS_ObtainedMarks + " * ";
                                                //}


                                            } else {
                                                if (qqq1.estmpS_ObtainedMarks !== "" && qqq1.estmpS_ObtainedGrade !== "") {
                                                    if (abc == 'M') {
                                                        s = s + 'M';
                                                        qqq[s] = qqq1.estmpS_ObtainedMarks + " " + " | " + " " + qqq1.estmpS_ObtainedGrade + " * ";
                                                    }
                                                    if (abc1 == 'G') {
                                                        s1 = s1 + 'G';
                                                        qqq[s1] = qqq1.estmpS_ObtainedMarks + " " + " | " + " " + qqq1.estmpS_ObtainedGrade + " * ";
                                                    }

                                                }
                                                //qqq[s] = qqq1.estmpS_ObtainedMarks + '|' +  qqq1.estmpS_ObtainedGrade + "*";
                                            }
                                        }
                                        if (qqq1.estmpS_PassFailFlg !== 'Fail') {

                                            if (qqq1.eyceS_MarksDisplayFlg === true && qqq1.eyceS_GradeDisplayFlg === false) {
                                                if (abc == 'M') {
                                                    s = s + 'M';
                                                    qqq[s] = qqq1.estmpS_ObtainedMarks;
                                                }
                                                //if (abc1 == 'G') {
                                                //    s1 = s1 + 'G';
                                                //    qqq[s1] = qqq1.estmpS_ObtainedMarks;
                                                //}

                                            } else if (qqq1.eyceS_MarksDisplayFlg === false && qqq1.eyceS_GradeDisplayFlg === true) {
                                                //if (abc == 'M') {
                                                //    s = s + 'M';
                                                //    qqq[s] = qqq1.estmpS_ObtainedGrade;
                                                //}
                                                if (abc1 == 'G') {
                                                    s1 = s1 + 'G';
                                                    qqq[s1] = qqq1.estmpS_ObtainedGrade;
                                                }

                                            } else if (qqq1.eyceS_MarksDisplayFlg === false && qqq1.eyceS_GradeDisplayFlg === false) {
                                                //if (abc == 'M') {
                                                //    s = s + 'M';
                                                //    qqq[s] = qqq1.estmpS_ObtainedMarks;
                                                //}
                                                //if (abc1 == 'G') {
                                                //    s1 = s1 + 'G';
                                                //    qqq[s1] = qqq1.estmpS_ObtainedMarks;
                                                //}


                                            } else {
                                                debugger;
                                                if (qqq1.estmpS_ObtainedMarks !== "" && qqq1.estmpS_ObtainedGrade !== "") {
                                                    if (abc == 'M') {
                                                        s = s + 'M';
                                                        qqq[s] = qqq1.estmpS_ObtainedMarks;
                                                    }
                                                }

                                                if (qqq1.estmpS_ObtainedMarks !== "" && qqq1.estmpS_ObtainedGrade !== "") {
                                                    if (abc1 == 'G') {
                                                        s1 = s1 + 'G';
                                                        qqq[s1] = qqq1.estmpS_ObtainedGrade;
                                                    }

                                                }
                                            }
                                        }


                                        //if (qqq1.estmpS_PassFailFlg === 'Fail' && qqq1.eyceS_MarksDisplayFlg == true && qqq1.eyceS_GradeDisplayFlg == false) {
                                        //    qqq[s] = qqq1.estmpS_ObtainedMarks + "*";
                                        //}
                                        //if (qqq1.estmpS_PassFailFlg !== 'Fail' && qqq1.eyceS_MarksDisplayFlg == true && qqq1.eyceS_GradeDisplayFlg == true) {
                                        //    qqq[s] = qqq1.estmpS_ObtainedMarks + '(' + qqq[s] = qqq1.estmpS_ObtainedGrade + ')';
                                        //}
                                        //if (qqq1.estmpS_PassFailFlg === 'Fail' && qqq1.eyceS_GradeDisplayFlg == true) {
                                        //    qqq[s] = qqq1.estmpS_ObtainedMarks + "*";
                                        //}
                                        //if (qqq1.estmpS_PassFailFlg !== 'Fail' && qqq1.eyceS_GradeDisplayFlg == true) {
                                        //    qqq[s] = qqq1.estmpS_ObtainedGrade;
                                        //}
                                    }
                                    else {
                                        qqq[s] = qqq1.estmpS_ObtainedMarks + "|" + qqq1.estmpS_ObtainedGrade;
                                        //  qqq[s] = qqq1.estmpS_ObtainedGrade;
                                    }
                                });
                                angular.forEach(qqq.elmrklist, function (qqq1) {
                                   // var q = qqq1.ismS_SubjectName;
                                    var q = "unique" + qqq1.ismS_Id + qqq1.ismS_SubjectName;
                                    var q1 = "unique" + qqq1.ismS_Id + qqq1.ismS_SubjectName;
                                    if (qqq1.estmpS_PassFailFlg !== 'AB' && qqq1.estmpS_PassFailFlg !== 'M' && qqq1.estmpS_PassFailFlg !== 'L') {
                                        if (qqq1.estmpS_PassFailFlg === 'Fail' && qqq1.eyceS_MarksDisplayFlg === true) {

                                            if (abc == 'M') {
                                                q = q + 'M';
                                                qqq[q] = qqq1.estmpS_ObtainedMarks + "*";
                                            }
                                          //  qqq[q] = qqq1.estmpS_ObtainedMarks + "*";
                                        }
                                        if (qqq1.estmpS_PassFailFlg !== 'Fail' && qqq1.eyceS_MarksDisplayFlg === true) {
                                            if (abc == 'M') {
                                                q = q + 'M';
                                                qqq[q] = qqq1.estmpS_ObtainedMarks + "*";
                                            }
                                            //qqq[q] = qqq1.estmpS_ObtainedMarks;
                                        }
                                        if (qqq1.estmpS_PassFailFlg === 'Fail' && qqq1.eyceS_GradeDisplayFlg === true) {
                                            if (abc == 'G') {
                                                q = q + 'G';
                                                qqq[q] = qqq1.estmpS_ObtainedGrade + "*";
                                            }
                                            //qqq[q] = qqq1.estmpS_ObtainedGrade + "*";
                                        }
                                        if (qqq1.estmpS_PassFailFlg !== 'Fail' && qqq1.eyceS_GradeDisplayFlg === true) {
                                            if (abc == 'G') {
                                                q = q + 'G';
                                                qqq[q] = qqq1.estmpS_ObtainedGrade;
                                            }

                                            //qqq[q] = qqq1.estmpS_ObtainedGrade;
                                        }
                                    }
                                    else if (qqq1.estmpS_PassFailFlg === 'AB' || qqq1.estmpS_PassFailFlg === 'M' || qqq1.estmpS_PassFailFlg === 'L') {

                                        if (qqq1.eyceS_GradeDisplayFlg === true) {
                                    if (abc == 'G') {
                                        q = q + 'G';
                                        qqq[q] = qqq1.estmpS_PassFailFlg;
                                    }
                                        }

                                        if (qqq1.eyceS_MarksDisplayFlg === true) {
                                            if (abc == 'M') {
                                                q = q + 'M';
                                                qqq[q] = qqq1.estmpS_PassFailFlg;
                                            }
                                        }
                                       // qqq[q] = qqq1.estmpS_PassFailFlg;
                                    }
                                });
                            });


                            console.log($scope.details);
                            console.log("dfsd");
                            console.log($scope.colarrayall);

                            $(document).ready(function () {
                                $('#gridhhs').empty();
                                $("#gridhhs").kendoGrid({
                                    toolbar: ["excel"],
                                    excel: {
                                        fileName: "inddExport.xlsx",
                                        //allPages: true,
                                        filterable: true,
                                        allPages: true
                                    },
                                    dataSource: {
                                        data: $scope.details,
                                        pageSize: 100
                                    },

                                    groupable: false,
                                    sortable: true,
                                    pageable: true,
                                    columnMenu: true,
                                    columns: $scope.colarrayall,
                                    dataBound: function () {
                                        var pagenum = this.dataSource.page();
                                        var pageitms = this.dataSource.pageSize();
                                        var rows = this.items();
                                        $(rows).each(function () {
                                            var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                            var rowLabel = $(this).find(".row-number");
                                            $(rowLabel).html(index);
                                        });
                                    }
                                });
                            });
                            //kendigrid end
                        }
                        else if (promise.savenonsubjlist === null && promise.savelist === null) {
                            swal('No record Found');
                        }
                    });
            }
        };

        $scope.printToCart = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/CumulativeReporthhsPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
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

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.subjectlt1, function (itm) {
                itm.xyz = toggleStatus;
            });
        };

        $scope.optionToggled = function (chk_box) {
            $scope.all = $scope.subjectlt1.every(function (itm) { return itm.xyz; });
        };

        $scope.get_cls_sections = function (cls_id) {

            if ($scope.asmaY_Id !== "" && $scope.asmaY_Id !== undefined && $scope.asmaY_Id !== null) {

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    //clssids: $scope.selectedclasses,
                };

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
                        if (promise.seclist === null || promise.seclist === "") {
                            swal("Sections are Not Mapped To Selected Class!!!");
                        }
                        //else if (promise.exmstdlist == null || promise.exmstdlist == "") {
                        //    swal("Exams are Not Mapped To Selected Class !!!");
                        //}
                    });
            }
            else {

                swal("Please Select Academic Year  First !!!");
                $scope.asmcL_Id = "";

            }
        };

        $scope.get_classes = function (ASMAY_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("ExamCalculation_SSSE/get_classes", data).
                then(function (promise) {
                    $scope.clslist = promise.classlist;
                    $scope.asmcL_Id = "";
                    $scope.asmS_Id = "";
                    $scope.emE_Id = "";
                    $scope.seclist = [];
                    $scope.exsplt = [];
                    if (promise.classlist === null || promise.classlist === "") {
                        swal("Classes are Not Mapped To Selected Academic Year!!!");
                    }
                });
        };

        $scope.get_exams = function (ASMS_Id, ASMCL_Id, ASMAY_Id) {
            if (ASMAY_Id !== null && ASMAY_Id !== undefined && ASMCL_Id !== null && ASMCL_Id !== undefined) {
                var data = {
                    "ASMS_Id": ASMS_Id,
                    "ASMCL_Id": ASMCL_Id,
                    "ASMAY_Id": ASMAY_Id
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("ExamCalculation_SSSE/get_exams", data).
                    then(function (promise) {
                        $scope.emE_Id = "";
                        $scope.exsplt = promise.exmstdlist;
                        if (promise.exmstdlist === null || promise.exmstdlist === "") {
                            swal("Exams are Not Mapped To Selected Class And Section!!!");
                        }
                    });
            }
            else {
                swal("Please Select Academic Year  And Class First !!!");
                $scope.asmS_Id = "";
            }
        };
    }
})();