(function () {
    'use strict';
    angular
        .module('app')
        .controller('SubjectsubjexamasubsubjectCumulativeController', SubjectsubjexamasubsubjectCumulativeController)
    SubjectsubjexamasubsubjectCumulativeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function SubjectsubjexamasubsubjectCumulativeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

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
        $scope.font_size = 'font14';
        $scope.size = '14px';


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
            if (obj.abc === true) {
                angular.forEach($scope.subjectlt1, function (itm1) {
                    if (itm1.amsT_Id === user.amsT_Id) {
                        angular.forEach($scop = e.subjectlt, function (itm2) {
                            if (itm2.ismS_Id === column.ismS_Id) {
                                itm1.sub_list.push({ id: itm2.ismS_Id, name: itm2.ismS_SubjectName });
                            }
                        });
                    }
                });
            }
            else {
                angular.forEach($scope.subjectlt1, function (itm1) {
                    if (itm1.amsT_Id === user.amsT_Id) {
                        for (var i = 0; i < itm1.sub_list.length; i++) {
                            if (itm1.sub_list[i].id === column.ismS_Id) {
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
                apiService.create("HHSMIDFINALCumReport/savedetailsnew", data).
                    then(function (promise) {

                        $scope.masterinst = promise.configuration;
                        var count = 0;

                        if (promise.savelistnew !== null) {

                            $scope.printbtn = true;
                            $scope.exclbtn = true;


                            $scope.repoershow = true;
                            angular.forEach($scope.clslist, function (itm) {
                                if (itm.asmcL_Id === parseInt($scope.asmcL_Id)) {
                                    $scope.cla = itm.asmcL_ClassName;
                                }
                            });
                            angular.forEach($scope.yearlt, function (itm) {
                                if (itm.asmaY_Id === parseInt($scope.asmaY_Id)) {
                                    $scope.yr = itm.asmaY_Year;
                                }
                            });
                            angular.forEach($scope.seclist, function (itm) {
                                if (itm.asmS_Id === parseInt($scope.asmS_Id)) {
                                    $scope.sec = itm.asmC_SectionName;
                                }
                            });
                            angular.forEach($scope.exsplt, function (itm) {
                                if (itm.emE_Id === parseInt($scope.emE_Id)) {
                                    $scope.exmmid = itm.emE_ExamName;
                                }
                            });

                            $scope.printbtn = true;
                            $scope.exclbtn = true;

                            $scope.instname = promise.instname;
                            $scope.inst_name = $scope.instname[0].mI_Name;
                            $scope.add = $scope.instname[0].mI_Address1;
                            $scope.city = $scope.instname[0].ivrmmcT_Name;
                            $scope.pin = $scope.instname[0].mI_Pincode;

                            $scope.studlist = promise.studlist;
                            $scope.savelist = promise.savelistnew;
                            $scope.subjectlistwithdetails = promise.subjectlistwithdetails;

                            //$scope.electivemarks = promise.electivemarks;
                            //$scope.electivestd = promise.savenonsubjlist;

                            $scope.subjectlistnews = promise.subjectlistnew;
                            $scope.exmrank = promise.exmrank;

                            angular.forEach($scope.studlist, function (dd) {
                                $scope.tempsubjectlist = [];
                                angular.forEach($scope.subjectlistnews, function (sub) {
                                    $scope.tempdetails = [];
                                    angular.forEach($scope.savelist, function (ds) {

                                        if (ds.AMST_Id === dd.amsT_Id) {
                                            if (sub.ISMS_Id === ds.ISMS_Id) {
                                                $scope.tempdetails.push({
                                                    subjid: ds.ISMS_Id, subexamname: ds.EMSE_SubExamName, subsubjectname: ds.EMSS_SubSubjectName,
                                                    marksobtained: ds.subsubjectmarks, gradeobtained: ds.subsubjectgrade
                                                });

                                            }
                                        }
                                    });

                                    $scope.tempsubjectlist.push({ subid: sub.ISMS_Id, subjectname: sub.ISMS_SubjectName, templist: $scope.tempdetails });
                                });
                                dd.details = $scope.tempsubjectlist;
                            });

                            console.log($scope.studlist);

                            $scope.lastmainlist = [];

                            angular.forEach($scope.studlist, function (tt) {
                                $scope.submainlist = [];
                                angular.forEach(tt.details, function (mm) {

                                    $scope.exam_list = [];
                                    $scope.overalltotalmax = 0;
                                    $scope.mainheading1 = [];
                                    angular.forEach(mm.templist, function (st) {

                                        if (st.subsubjectname !== '') {
                                            if ($scope.exam_list.length === 0) {

                                                $scope.exam_list.push({ subjid: st.subjid, subsubjectname: st.subsubjectname });
                                                // $scope.mainheading1.push({ id: st.id, categoryname: st.categoryname, ACQC_Id: st.ACQC_Id });
                                            }
                                            else if ($scope.exam_list.length > 0) {
                                                var al_exm_cnt = 0;
                                                angular.forEach($scope.exam_list, function (exm) {
                                                    if (exm.subsubjectname === st.subsubjectname) {
                                                        al_exm_cnt += 1;
                                                    }
                                                });
                                                if (al_exm_cnt === 0) {

                                                    $scope.exam_list.push({ subjid: st.subjid, subsubjectname: st.subsubjectname });
                                                }
                                            }
                                        };
                                        if (st.subexamname !== '') {
                                            if ($scope.mainheading1.length === 0) {

                                                $scope.mainheading1.push({ subjid: st.subjid, subexamname: st.subexamname });

                                            }
                                            else if ($scope.mainheading1.length > 0) {
                                                var al_exm_cnt1 = 0;
                                                angular.forEach($scope.mainheading1, function (exm) {
                                                    if (exm.subexamname === st.subexamname) {
                                                        al_exm_cnt1 += 1;
                                                    }
                                                });
                                                if (al_exm_cnt1 === 0) {

                                                    $scope.mainheading1.push({ subjid: st.subjid, subexamname: st.subexamname });

                                                }
                                            }
                                        }


                                    });

                                    $scope.submainlist.push({ subid: mm.subid, subjectname: mm.subjectname, subsub: $scope.exam_list, subexm: $scope.mainheading1 });

                                });


                                $scope.lastmainlist.push({ amsT_Id: tt.amsT_Id, sublist: $scope.submainlist });
                                //st.tempnew = $scope.mainheading;
                            });


                            console.log($scope.lastmainlist);

                            $scope.lastmainlistnew = [];
                            $scope.submainlistnew = [];
                            $scope.exam_listnew = [];
                            $scope.overalltotalmax = 0;
                            $scope.mainheading1new = [];
                            $scope.ssss = [];
                            $scope.eeee = [];

                            angular.forEach($scope.subjectlistnews, function (dd) {
                                $scope.exam_listnew = [];
                                $scope.mainheading1new = [];
                                angular.forEach($scope.subjectlistwithdetails, function (dde) {
                                    if (dd.ISMS_Id === dde.ISMS_Id) {
                                        if (dde.EMSS_SubSubjectName !== '') {
                                            if ($scope.exam_listnew.length === 0) {
                                                $scope.exam_listnew.push({ subjid: dd.ISMS_Id, subsubjectname: dde.EMSS_SubSubjectName });
                                                $scope.ssss.push({ subjid: dd.ISMS_Id, subsubjectname: dde.EMSS_SubSubjectName });
                                            }
                                            else if ($scope.exam_listnew.length > 0) {
                                                var al_exm_cnt = 0;
                                                angular.forEach($scope.exam_listnew, function (exm) {
                                                    if (exm.subsubjectname === dde.EMSS_SubSubjectName) {
                                                        al_exm_cnt += 1;
                                                    }
                                                });
                                                if (al_exm_cnt === 0) {
                                                    $scope.ssss.push({ subjid: dd.ISMS_Id, subsubjectname: dde.EMSS_SubSubjectName });
                                                    $scope.exam_listnew.push({ subjid: dd.ISMS_Id, subsubjectname: dde.EMSS_SubSubjectName });
                                                }
                                            }
                                        }


                                        if (dde.EMSE_SubExamName !== '') {
                                            if ($scope.mainheading1new.length === 0) {
                                                $scope.mainheading1new.push({ subjid: dd.ISMS_Id, subexamname: dde.EMSE_SubExamName });
                                                $scope.eeee.push({ subjid: dd.ISMS_Id, subexamname: dde.EMSE_SubExamName });
                                            }
                                            else if ($scope.mainheading1new.length > 0) {
                                                var al_exm_cnt1 = 0;
                                                angular.forEach($scope.mainheading1new, function (exm) {
                                                    if (exm.subexamname === dde.EMSE_SubExamName) {
                                                        al_exm_cnt1 += 1;
                                                    }
                                                });
                                                if (al_exm_cnt1 === 0) {
                                                    $scope.mainheading1new.push({ subjid: dd.ISMS_Id, subexamname: dde.EMSE_SubExamName });
                                                    $scope.eeee.push({ subjid: dd.ISMS_Id, subexamname: dde.EMSE_SubExamName });
                                                }
                                            }
                                        }

                                    }
                                });

                                $scope.submainlistnew.push({
                                    subid: dd.ISMS_Id, subjectname: dd.ISMS_SubjectName,
                                    subsubnew: $scope.exam_listnew, subexmnew: $scope.mainheading1new
                                });
                            });

                            angular.forEach($scope.submainlistnew, function (fff) {
                                if (fff.subsubnew.length === 0) {
                                    fff.rowspancnt = 2;
                                } else {
                                    fff.rowspancnt = 1;
                                }
                            });


                            $scope.abcd = [];
                            angular.forEach($scope.submainlistnew, function (dd) {
                                if (dd.subsubnew.length > 0) {
                                    angular.forEach(dd.subsubnew, function (ll) {
                                        $scope.abcd.push({ subid: dd.subid, subjectname: dd.subjectname, subsubjectname: ll.subsubjectname });
                                    });
                                }
                                else {
                                    angular.forEach(dd.subexmnew, function (cc) {
                                        $scope.abcd.push({ subid: dd.subid, subjectname: dd.subjectname, subsubjectname: '' });
                                    });
                                }
                            });

                            angular.forEach($scope.abcd, function (lk) {
                                var cnt = 0;
                                angular.forEach($scope.subjectlistwithdetails, function (hh) {
                                    if (lk.subid === hh.ISMS_Id && lk.subsubjectname === hh.EMSS_SubSubjectName && hh.EMSS_SubSubjectName !== '') {
                                        cnt += 1;
                                    }
                                });
                                lk.clspncnt = cnt;
                            });

                            // final total max marks display

                            angular.forEach($scope.studlist, function (std) {
                                angular.forEach($scope.exmrank, function (marks) {
                                    if (std.amsT_Id === marks.amsT_Id) {
                                        std.total = marks.estmP_TotalObtMarks;
                                        std.maxtotal = marks.estmP_TotalMaxMarks;
                                        std.percentage = marks.estmP_Percentage;
                                        std.estmP_SectionRank = marks.estmP_SectionRank;
                                        std.estmP_Result = marks.estmP_Result;
                                    }
                                });
                            });


                            // $scope.lastmainlistnew.push({ sublist: $scope.submainlistnew });
                            console.log("Final");
                            console.log($scope.submainlistnew);
                            console.log($scope.subjectlistwithdetails);
                            console.log($scope.ssss);
                            console.log($scope.eeee);
                            console.log($scope.abcd);
                            console.log($scope.savelist);

                            angular.forEach($scope.studlist, function (dd) {
                                $scope.bindarray = [];
                                angular.forEach($scope.savelist, function (ddd) {

                                    if (dd.amsT_Id === ddd.AMST_Id) {
                                        $scope.bindarray.push(ddd);
                                    }
                                });
                                dd.arraybind = $scope.bindarray;
                            });
                            console.log("VISHNU");
                            console.log($scope.studlist);


                            $scope.grade = [
                                { id: 1, name: 'M' },
                                { id: 2, name: 'G' }
                            ];
                            $scope.tempmarksgrade = [];
                            angular.forEach($scope.subjectlistwithdetails, function (dd) {

                              

                                angular.forEach($scope.grade, function (ddd) {
                                    $scope.tempmarksgrade.push({ subjidnew: dd.ISMS_Id, name: ddd.name, EMSE_SubExamName: dd.EMSE_SubExamName, EMSS_SubSubjectName: dd.EMSS_SubSubjectName });
                                });


                            });

                            angular.forEach($scope.submainlistnew, function (TT) {
                                var cnt = 0;
                                angular.forEach($scope.tempmarksgrade, function (PP) {
                                    if (PP.subjidnew == TT.subid) {
                                        debugger;
                                        cnt += 1;
                                    }


                                })
                                TT.clmcnt = cnt;
                            })
                            console.log("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
                            console.log($scope.tempmarksgrade);
                            console.log($scope.submainlistnew);
                        }
                        else if (promise.savenonsubjlist === null && promise.savelist === null) {
                            swal('No record Found');
                        }
                    });
            }
        };

        $scope.printToCart = function () {
            var innerContents = document.getElementById("printnew").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportBBPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 1000);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function (tableIds) {
            var exportHref = Excel.tableToExcel(tableIds, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
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