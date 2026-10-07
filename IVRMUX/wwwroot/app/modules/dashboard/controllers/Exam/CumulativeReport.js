
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CumulativeReportController', CumulativeReportController)

    CumulativeReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', 'blockUI']
    function CumulativeReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, blockUI) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.report = false;

        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            apiService.getDATA("CumulativeReport/Getdetails").
                then(function (promise) {

                    $scope.yearlt = promise.yearlist;
                    $scope.clslist = promise.classlist;
                    $scope.seclist = promise.seclist;
                    $scope.amstlt = promise.amstlist;
                    $scope.exsplt = promise.exmstdlist;
                    //  $scope.gridOptions.data = promise.studmaplist;
                })
        };



        //Table 
        //$scope.gridOptions = {
        //    enableColumnMenus: false,
        //    enableFiltering: true,
        //    paginationPageSizes: [5, 10, 15],
        //    paginationPageSize: 5,

        //    columnDefs: [
        //           { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
        //            { name: 'amsT_FirstName', displayName: 'Student Name' },
        //            { name: 'asmcL_ClassName', displayName: 'Class Name' },
        //            { name: 'asmC_SectionName', displayName: 'Section Name' },
        //       {
        //           field: 'id', name: '',
        //           displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
        //               '<div class="grid-action-cell">' +
        //                '<a ng-if="row.entity.estsU_ElecetiveFlag === true" href="javascript:void(0)" data-toggle="modal" data-target="#popup" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
        //         '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +

        //        '<a ng-if="row.entity.estsU_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
        //      '<span ng-if="row.entity.estsU_ActiveFlg === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
        //        '</div>'
        //       }
        //    ]

        //};

        //Record pop up 
        $scope.viewrecordspopup = function (employee, SweetAlert) {

            $scope.editEmployee = employee.amsT_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("CumulativeReport/getalldetailsviewrecords", pageid).
                then(function (promise) {

                    $scope.viewrecordspopupdisplay = promise.gtdetailsview
                    //$scope.Group_Name = promise.view_group_subjects[0].emG_GroupName;
                    //$scope.viewrecordspopupdisplay = promise.view_group_subjects;

                })

        };
        $scope.clearpopupgrid = function () {
            $scope.viewrecordspopupdisplay = "";
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

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
                        }

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
                            })
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
                        swal('No Record Found')
                    }
                })
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

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
                        })
                    }
                })
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
                })

            }
        };


        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "EME_ID": $scope.emE_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("CumulativeReport/savedetails", data).
                    then(function (promise) {
                        $scope.report = true;
                        $scope.masterinst = promise.configuration;
                        if (promise.savelist != null) {

                          
                            var count = 0;
                            if ($scope.masterinst.length > 0) {
                                if ($scope.masterinst[0].exmConfig_RegnoColumnDisplay == true) {
                                    $scope.regno = true;
                                    count = count + 1;

                                } else {
                                    $scope.regno = false;
                                }

                                if ($scope.masterinst[0].exmConfig_AdmnoColumnDisplay == true) {
                                    $scope.admno = true;
                                    count = count + 1;
                                } else {
                                    $scope.admno = false;
                                }

                                if ($scope.masterinst[0].exmConfig_RollnoColumnDisplay == true) {
                                    $scope.rollno = true;
                                    count = count + 1;
                                } else {
                                    $scope.rollno = false;
                                }

                                if (count == 0) {
                                    $scope.admno = true;
                                    $scope.rollno = true;
                                }


                            } else {
                                $scope.admno = true;
                                $scope.rollno = true;
                            }



                            if (promise.savelist.length > 0) {
                                $scope.report = true;

                                $scope.mI_name = promise.savelist[0].mI_name;

                                angular.forEach($scope.clslist, function (itm) {
                                    if (itm.asmcL_Id == $scope.asmcL_Id) {
                                        $scope.cla = itm.asmcL_ClassName;
                                    }
                                })
                                angular.forEach($scope.yearlt, function (itm) {
                                    if (itm.asmaY_Id == $scope.asmaY_Id) {
                                        $scope.yr = itm.asmaY_Year;
                                    }
                                })
                                angular.forEach($scope.seclist, function (itm) {
                                    if (itm.asmS_Id == $scope.asmS_Id) {
                                        $scope.sec = itm.asmC_SectionName;
                                    }
                                })
                                angular.forEach($scope.exsplt, function (itm) {
                                    if (itm.emE_Id == $scope.emE_Id) {
                                        $scope.exmmid = itm.emE_ExamName;
                                    }
                                })


                                $scope.studentslt = promise.savelist;
                                $scope.studentslt1 = promise.subjlist;



                                var temp_list = [];
                                for (var x = 0; x < promise.savelist.length; x++) {
                                    var stu_id = promise.savelist[x].amsT_Id;
                                    var stu_subj_list = [];
                                    angular.forEach(promise.savelist, function (opq) {
                                        if (opq.amsT_Id == stu_id) {
                                            stu_subj_list.push({
                                                amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id,
                                                ismS_SubjectName: opq.amsT_Id.ismS_SubjectName, estmpS_MaxMarks: opq.estmpS_MaxMarks,
                                                estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks, estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade,
                                                estmpS_PassFailFlg: opq.estmpS_PassFailFlg, eyceS_SubjectOrder: opq.eyceS_SubjectOrder
                                            });
                                        }
                                    })

                                    if (temp_list.length == 0) {
                                        temp_list.push({
                                            student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName,
                                            amsT_AdmNo: promise.savelist[x].amsT_AdmNo,
                                            amaY_RollNo: promise.savelist[x].amaY_RollNo,
                                            amsT_RegistrationNo: promise.savelist[x].amsT_RegistrationNo,
                                            estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_SectionRank: promise.savelist[x].estmP_SectionRank,
                                            classheld: promise.savelist[x].classheld, classatt: promise.savelist[x].classatt, sub_list: stu_subj_list
                                        });
                                    }
                                    else if (temp_list.length > 0) {
                                        var already_cnt = 0;
                                        angular.forEach(temp_list, function (opq1) {
                                            if (opq1.student_id == stu_id) {
                                                already_cnt += 1;
                                            }
                                        })
                                        if (already_cnt == 0) {
                                            temp_list.push({
                                                student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName,
                                                amsT_LastName: promise.savelist[x].amsT_LastName,
                                                amsT_AdmNo: promise.savelist[x].amsT_AdmNo,
                                                amaY_RollNo: promise.savelist[x].amaY_RollNo,
                                                amsT_RegistrationNo: promise.savelist[x].amsT_RegistrationNo,
                                                estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks,
                                                estmP_SectionRank: promise.savelist[x].estmP_SectionRank, classheld: promise.savelist[x].classheld,
                                                classatt: promise.savelist[x].classatt, sub_list: stu_subj_list
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
                                                ccount += 1;
                                                if (oobj2.estmpS_PassFailFlg != 'AB') {

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
                                                else if (oobj2.estmpS_PassFailFlg == 'AB') {
                                                    oobj2.hema_estmpS_ObtainedMarks = oobj2.estmpS_PassFailFlg;
                                                    // oobj2.hema_estmpS_ObtainedGrade = oobj2.estmpS_PassFailFlg;
                                                }
                                                $scope.tmparrry.push(oobj2);
                                            }
                                        });

                                        if (ccount == 0) {
                                            var obj = {};
                                            obj.hema_estmpS_ObtainedMarks = "";
                                            obj.hema_estmpS_ObtainedGrade = "";
                                            $scope.tmparrry.push(obj);
                                        }
                                    })
                                    oobj.sub_list = $scope.tmparrry;
                                })

                                if (promise.savenonsubjlist != null) {

                                    if (promise.savenonsubjlist.length > 0) {
                                        angular.forEach($scope.clslist, function (itm) {
                                            if (itm.asmcL_Id == $scope.asmcL_Id) {
                                                $scope.cla = itm.asmcL_ClassName;
                                            }
                                        })
                                        angular.forEach($scope.yearlt, function (itm) {
                                            if (itm.asmaY_Id == $scope.asmaY_Id) {
                                                $scope.yr = itm.asmaY_Year;
                                            }
                                        })
                                        angular.forEach($scope.seclist, function (itm) {
                                            if (itm.asmS_Id == $scope.asmS_Id) {
                                                $scope.sec = itm.asmC_SectionName;
                                            }
                                        })
                                        angular.forEach($scope.exsplt, function (itm) {
                                            if (itm.emE_Id == $scope.emE_Id) {
                                                $scope.exmmid = itm.emE_ExamName;
                                            }
                                        })


                                        $scope.electivestd = promise.savenonsubjlist;
                                        $scope.electivesub = promise.nonsubjlist;

                                        var temp_list = [];
                                        for (var x = 0; x < promise.savenonsubjlist.length; x++) {
                                            var stu_id = promise.savenonsubjlist[x].amsT_Id;
                                            var stu_subj_list = [];
                                            angular.forEach(promise.savenonsubjlist, function (opq) {
                                                if (opq.amsT_Id == stu_id) {
                                                    stu_subj_list.push({
                                                        amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id, ismS_SubjectName: opq.amsT_Id.ismS_SubjectName,
                                                        estmpS_MaxMarks: opq.estmpS_MaxMarks, estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks,
                                                        estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade, estmpS_PassFailFlg: opq.estmpS_PassFailFlg,
                                                        eyceS_SubjectOrder: opq.eyceS_SubjectOrder
                                                    });
                                                }
                                            })
                                            if (temp_list.length == 0) {
                                                temp_list.push({
                                                    student_id: promise.savenonsubjlist[x].amsT_Id, amsT_FirstName: promise.savenonsubjlist[x].amsT_FirstName,
                                                    amsT_LastName: promise.savenonsubjlist[x].amsT_LastName,
                                                    amsT_AdmNo: promise.savenonsubjlist[x].amsT_AdmNo,
                                                    amaY_RollNo: promise.savenonsubjlist[x].amaY_RollNo,
                                                    amsT_RegistrationNo: promise.savenonsubjlist[x].amsT_RegistrationNo,
                                                    estmP_TotalObtMarks: promise.savenonsubjlist[x].estmP_TotalObtMarks,
                                                    estmP_SectionRank: promise.savelist[x].estmP_SectionRank, classheld: promise.savelist[x].classheld,
                                                    classatt: promise.savelist[x].classatt, sub_list: stu_subj_list
                                                });
                                            }
                                            else if (temp_list.length > 0) {
                                                var already_cnt = 0;
                                                angular.forEach(temp_list, function (opq1) {
                                                    if (opq1.student_id == stu_id) {
                                                        already_cnt += 1;
                                                    }
                                                })
                                                if (already_cnt == 0) {
                                                    temp_list.push({
                                                        student_id: promise.savenonsubjlist[x].amsT_Id,
                                                        amsT_FirstName: promise.savenonsubjlist[x].amsT_FirstName,
                                                        amsT_LastName: promise.savenonsubjlist[x].amsT_LastName,
                                                        amsT_AdmNo: promise.savenonsubjlist[x].amsT_AdmNo,
                                                        amaY_RollNo: promise.savenonsubjlist[x].amaY_RollNo,
                                                        amsT_RegistrationNo: promise.savenonsubjlist[x].amsT_RegistrationNo,
                                                        estmP_TotalObtMarks: promise.savenonsubjlist[x].estmP_TotalObtMarks,
                                                        estmP_SectionRank: promise.savelist[x].estmP_SectionRank, classheld: promise.savelist[x].classheld,
                                                        classatt: promise.savelist[x].classatt, sub_list: stu_subj_list
                                                    });
                                                }
                                            }

                                        }
                                        $scope.nonstudentslt = temp_list;
                                        //$scope.studentslt = [];
                                        //angular.forEach(temp_list, function (xyz) {
                                        //    $scope.studentslt.push({ student_id: xyz.student_id, amsT_FirstName: xyz.amsT_FirstName, amsT_LastName: xyz.amsT_LastName });
                                        //})

                                    }
                                }

                            }
                        }
                        else if (promise.savenonsubjlist != null) {
                            $scope.report = true;
                            if (promise.savenonsubjlist.length > 0) {
                                angular.forEach($scope.clslist, function (itm) {
                                    if (itm.asmcL_Id == $scope.asmcL_Id) {
                                        $scope.cla = itm.asmcL_ClassName;
                                    }
                                })
                                angular.forEach($scope.yearlt, function (itm) {
                                    if (itm.asmaY_Id == $scope.asmaY_Id) {
                                        $scope.yr = itm.asmaY_Year;
                                    }
                                })
                                angular.forEach($scope.seclist, function (itm) {
                                    if (itm.asmS_Id == $scope.asmS_Id) {
                                        $scope.sec = itm.asmC_SectionName;
                                    }
                                })
                                angular.forEach($scope.exsplt, function (itm) {
                                    if (itm.emE_Id == $scope.emE_Id) {
                                        $scope.exmmid = itm.emE_ExamName;
                                    }
                                })


                                $scope.electivestd = promise.savenonsubjlist;
                                $scope.electivesub = promise.nonsubjlist;

                                var temp_list = [];
                                for (var x = 0; x < promise.savenonsubjlist.length; x++) {

                                    $scope.mI_name = promise.savenonsubjlist[0].mI_name;

                                    var stu_id = promise.savenonsubjlist[x].amsT_Id;
                                    var stu_subj_list = [];
                                    angular.forEach(promise.savenonsubjlist, function (opq) {
                                        if (opq.amsT_Id == stu_id) {
                                            stu_subj_list.push({ amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id, ismS_SubjectName: opq.amsT_Id.ismS_SubjectName, estmpS_MaxMarks: opq.estmpS_MaxMarks, estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks, estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade, estmpS_PassFailFlg: opq.estmpS_PassFailFlg });
                                        }
                                    })
                                    if (temp_list.length == 0) {
                                        temp_list.push({
                                            student_id: promise.savenonsubjlist[x].amsT_Id, amsT_FirstName: promise.savenonsubjlist[x].amsT_FirstName, amsT_LastName: promise.savenonsubjlist[x].amsT_LastName, estmP_TotalObtMarks: promise.savenonsubjlist[x].estmP_TotalObtMarks, classheld: promise.savenonsubjlist[x].classheld, classatt: promise.savenonsubjlist[x].classatt, sub_list: stu_subj_list
                                        });
                                    }
                                    else if (temp_list.length > 0) {
                                        var already_cnt = 0;
                                        angular.forEach(temp_list, function (opq1) {
                                            if (opq1.student_id == stu_id) {
                                                already_cnt += 1;
                                            }
                                        })
                                        if (already_cnt == 0) {
                                            temp_list.push({ student_id: promise.savenonsubjlist[x].amsT_Id, amsT_FirstName: promise.savenonsubjlist[x].amsT_FirstName, amsT_LastName: promise.savenonsubjlist[x].amsT_LastName, estmP_TotalObtMarks: promise.savenonsubjlist[x].estmP_TotalObtMarks, classheld: promise.savenonsubjlist[x].classheld, classatt: promise.savenonsubjlist[x].classatt, sub_list: stu_subj_list });
                                        }
                                    }

                                }
                                $scope.nonstudentslt = temp_list;
                                $scope.studentslt = [];
                                angular.forEach(temp_list, function (xyz) {
                                    $scope.studentslt.push({ student_id: xyz.student_id, amsT_FirstName: xyz.amsT_FirstName, amsT_LastName: xyz.amsT_LastName, classheld: xyz.classheld, classatt: xyz.classatt });
                                })


                            }
                            else if (promise.savenonsubjlist == 0 || null && promise.savelist == 0 || null) {
                                swal('No record Found');
                            }
                        }
                        console.log($scope.electivesub);
                        angular.forEach($scope.studentslt, function (oobj) {
                            angular.forEach($scope.nonstudentslt, function (oobj2) {
                                if (oobj2.student_id == oobj.student_id) {
                                    $scope.tmparrry1 = [];
                                    angular.forEach($scope.electivesub, function (oobj1) {
                                        var ccount1 = 0;
                                        angular.forEach(oobj2.sub_list, function (oobj3) {
                                            // ccount1 += 1;
                                            if (oobj1.ismS_Id == oobj3.ismS_Id) {
                                                ccount1 += 1;
                                                if (oobj3.estmpS_PassFailFlg != 'AB') {

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

                                                        if ($scope.masterinst[0].exmConfig_FailColorFlg !== "" && $scope.masterinst[0].exmConfig_FailColorFlg !== undefined && $scope.masterinst[0].exmConfig_FailColorFlg !== null) {
                                                            oobj2.stylecolor1 = $scope.masterinst[0].exmConfig_FailColorFlg;
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
                                                else if (oobj3.estmpS_PassFailFlg == 'AB') {
                                                    oobj3.hema_estmpS_ObtainedMarks = oobj3.estmpS_PassFailFlg;
                                                    // oobj3.hema_estmpS_ObtainedGrade = oobj3.estmpS_PassFailFlg;
                                                }
                                                $scope.tmparrry1.push(oobj3);
                                            }

                                        })
                                        if (ccount1 == 0) {
                                            var obj1 = {};
                                            obj1.hema_estmpS_ObtainedMarks = "";
                                            obj1.hema_estmpS_ObtainedGrade = "";
                                            $scope.tmparrry1.push(obj1);
                                        }
                                    })
                                    oobj.sub_list_e = $scope.tmparrry1;
                                }
                            })

                        })

                    })
            }
        };

        $scope.exportToExcel = function (tableId) {


            var exportHref = Excel.tableToExcel(tableId, 'sheet name');

            $timeout(function () {
                location.href = exportHref;
            }, 100);


        }


        //$scope.exportExcel = function (tableId) {
        //    
        //    var exportHref = Excel.tableToExcel(tableId, 'CumulativeReport');

        //    $timeout(function () {
        //        location.href = exportHref;
        //    }, 100);


        //    //var worksheetName = 'CumulativeReport';
        //    //var table=$(tableId),
        //    //    ctx={worksheet:worksheetName,table:table.html()},
        //    //    href=base64(format(template,ctx));
        //    //return href;
        //};



        $scope.printToCart = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
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