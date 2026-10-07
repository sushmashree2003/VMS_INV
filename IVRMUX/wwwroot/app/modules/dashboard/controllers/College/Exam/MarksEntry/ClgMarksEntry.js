(function () {
    'use strict';
    angular.module('app').controller('ClgMarksEntryController', ClgMarksEntryController)
    ClgMarksEntryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function ClgMarksEntryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.BindData = function () {
            apiService.getDATA("ClgMarksEntry/getalldetails").
                then(function (promise) {
                    $scope.yearlist = promise.getyear;
                    $scope.course_list = promise.courseslist;
                    $scope.branch_list = promise.branchlist;
                    $scope.semisters_list = promise.semisters;
                    $scope.subject_list = promise.subjectlist;
                    $scope.subjectgrp_list = promise.subjectgrplist;
                    $scope.section_List = promise.sectionlist;
                    $scope.exam_list = promise.examlist;
                    $scope.gridOptions.data = "";
                });
        };

        $scope.onchangeyear = function () {
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.ISMS_ID = "";
            $scope.EME_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("ClgMarksEntry/onchangeyear", data).then(function (promise) {

                $scope.course_list = promise.courseslist;

            });

        };

        $scope.onchangecourse = function () {
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.ISMS_ID = "";
            $scope.EME_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            };
            apiService.create("ClgMarksEntry/onchangecourse", data).then(function (promise) {
                $scope.branch_list = promise.branchlist;
            });
        };


        $scope.onchangebranch = function () {
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.ISMS_ID = "";
            $scope.EME_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };
            apiService.create("ClgMarksEntry/onchangebranch", data).then(function (promise) {
                $scope.semisters_list = promise.semisters;
            });
        };

        $scope.get_exams = function () {
            $scope.ACMS_Id = "";
            $scope.ISMS_ID = "";
            $scope.EME_Id = "";
            if ($scope.AMCO_Id !== "" && $scope.AMCO_Id !== undefined && $scope.AMSE_Id !== "" && $scope.AMSE_Id !== undefined && $scope.AMB_Id !== ""
                && $scope.AMB_Id !== undefined) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "AMB_Id": $scope.AMB_Id
                };
                apiService.create("ClgMarksEntry/get_exams", data).
                    then(function (promise) {
                        $scope.exam_list = promise.examlist;
                        $scope.section_List = promise.sectionlist;
                        $scope.subjectgrp_list = promise.subjectgrplist;
                    });
            }
        };



        $scope.get_subjects = function () {
            $scope.ISMS_ID = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                //"EMG_ID": $scope.EMG_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "AMB_Id": $scope.AMB_Id,
                "ACMS_Id": $scope.ACMS_Id

            };
            apiService.create("ClgMarksEntry/get_subjects", data).
                then(function (promise) {
                    $scope.subject_list = promise.subjectgroups;
                });

        };



        $scope.onsearch = function (ACMS_Id, AMCO_Id, AMSE_Id, AMB_Id, EME_Id, ISMS_Id) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": AMCO_Id,
                    "AMSE_Id": AMSE_Id,
                    "ACMS_Id": ACMS_Id,
                    "AMB_Id": AMB_Id,
                    "EME_Id": EME_Id,
                    "ISMS_Id": ISMS_Id
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("ClgMarksEntry/onsearch", data).
                    then(function (promise) {

                        $scope.gridOptions.data = promise.studentList;
                        $scope.subMorGFlag = promise.subMorGFlag;
                        $scope.gradname = promise.gradname;
                        // $scope.marksdeleteflag = promise.marksdeleteflag;
                        $scope.marksdeleteflag = false;
                    });

            }
        };
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            columnDefs: [
                { name: 'SLNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'studentname', displayName: 'Student Name' },
                { name: 'amcsT_AdmNo', displayName: 'Admission No' },
                { name: 'acysT_RollNo', displayName: 'Roll No', type: 'number' },
                { name: 'totalMarks', displayName: 'Max Marks' },
                { name: 'minMarks', displayName: 'Min Marks' },
                { name: 'marksEnterFor', displayName: 'Marks Enter For' },
                { name: 'obtainmarks', displayName: 'Obtain Marks', cellTemplate: '<div class="ui-grid-cell-contents"><input type="text" ng-model="row.entity.obtainmarks"  style="text-align:center;" allow-pattern="[A-Z0-9+-]" ng-blur="grid.appScope.changemarks(row.entity.marksEnterFor,row.entity.obtainmarks,row)"  class="form-control" value="0"  min="0" </input></div>' }

            ]

        };
        $scope.gridOptions.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;
        };
        $scope.submitted = false;

        $scope.SaveMarks = function (ACMS_Id, AMCO_Id, AMSE_Id, AMB_Id, EME_Id, ISMS_Id) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var flag = "false";

                if ($scope.marksdeleteflag == "true") {
                    swal({
                        title: "Are You Sure?",
                        text: "Marks Already Calculated, If You Update The Marks You Need To Recalculate Marks Again..Do You Want To Continue ..!?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Update It!",
                        cancelButtonText: "Cancel..!",
                        closeOnConfirm: false,
                        closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                $scope.save(ACMS_Id, AMCO_Id, AMSE_Id, AMB_Id, EME_Id, ISMS_Id);
                            }
                            else {
                                flag = "false"

                            }

                        });
                }
                else {
                    $scope.save(ACMS_Id, AMCO_Id, AMSE_Id, AMB_Id, EME_Id, ISMS_Id);
                }


            }
            else {
                swal('Please select required field / Entered Marks are not in correct format....!');
            }
        };

        $scope.save = function (ACMS_Id, AMCO_Id, AMSE_Id, AMB_Id, EME_Id, ISMS_Id) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.savedata = $scope.gridOptions.data;
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ACMS_Id": ACMS_Id,
                    "AMCO_Id": AMCO_Id,
                    "AMSE_Id": AMSE_Id,
                    "AMB_Id": AMB_Id,
                    "EME_Id": EME_Id,
                    "ISMS_Id": ISMS_Id,
                    "marksdeleteflag": $scope.marksdeleteflag,
                    "detailsList": $scope.savedata
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("ClgMarksEntry/SaveMarks", data).
                    then(function (promise) {

                        if (promise.messagesaveupdate == "true") {
                            $scope.cancle();
                            swal('Data Saved Successfully');
                        }
                        else {
                            swal('Failed to Save/Update Data');
                        }

                    })
            }
        };
        $scope.cancle = function () {
            $state.reload();
        };










        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        //TO  GEt The Values iN Grid
        $scope.changemarks = function (totalMarks, obtainmarks, row) {
            if ($scope.subMorGFlag == "G") {

                var flag = "false";

                for (var i = 0; i < $scope.gradname.length; i++) {
                    if ($scope.gradname[i] == obtainmarks) {
                        flag = "true";
                    }
                }

                if (obtainmarks == "AB" || obtainmarks == "ab" || obtainmarks == "L" || obtainmarks == "l" || obtainmarks == "M" || obtainmarks == "m") {
                    flag = "true";
                }

                if (flag == "false") {
                    row.entity.obtainmarks = 0;
                    swal('Entered Grade cant be out of master setting...!');
                }
            }
            else {
                var flag = "false";

                if (obtainmarks.match(/[a-z]/i)) {
                    if (obtainmarks == "AB" || obtainmarks == "ab" || obtainmarks == "L" || obtainmarks == "l" || obtainmarks == "M" || obtainmarks == "m") {
                        flag = "true";
                    }
                    if (flag == "false") {
                        row.entity.obtainmarks = 0;
                        swal('Entered value cant be out of master setting...!');
                    }
                }
                else {
                    if (totalMarks < obtainmarks) {
                        row.entity.obtainmarks = 0;
                        swal('Entered marks cant be more than Marks Enter For...!');
                    }
                    if (obtainmarks < 0) {
                        row.entity.obtainmarks = 0;
                        swal('Entered marks cant be in nagative values...!');
                    }
                }

            }
        };

        $scope.onselectAcdYear = function (ASMAY_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ExamEntry/onselectAcdYear", data).
                then(function (promise) {
                    $scope.ctlist = promise.ctlist;
                    $scope.ASMCL_Id = 'classdefualt';
                    $scope.gridOptions.data = "";
                })
        };

        $scope.onselectclass = function (ASMCL_Id, ASMAY_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id,
                "ASMCL_Id": ASMCL_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ExamEntry/onselectclass", data).
                then(function (promise) {
                    $scope.seclist = promise.seclist;
                    $scope.ASMS_Id = 'sectiondefualt';
                    $scope.gridOptions.data = "";
                })
        };



        $scope.onselectSection = function (ASMS_Id, ASMCL_Id, ASMAY_Id) {
            var data = {
                "ASMS_Id": ASMS_Id,
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ExamEntry/onselectSection", data).
                then(function (promise) {

                    $scope.EME_Id = "";
                    $scope.gridOptions.data = "";
                    $scope.examlist = promise.examlist;

                    $scope.EME_Id = 'examdefualt';

                    $scope.subjectlist = promise.subjectlist;

                    $scope.ISMS_Id = 'subjectdefualt';
                })
        };

        $scope.onselectExam = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id) {
            var data = {
                "ASMS_Id": ASMS_Id,
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id,
                "EME_Id": EME_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ExamEntry/onselectExam", data).
                then(function (promise) {
                    $scope.subjectlist = promise.subjectlist;
                    $scope.gridOptions.data = "";
                })
        };

        $scope.onselectSubject = function () {
            $scope.gridOptions.data = "";
        };






        $scope.cancel = function () {
            $state.reload();
            $scope.BindData();
        };
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


    }

})();