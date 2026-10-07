

(function () {
    'use strict';
    angular
        .module('app')
        .controller('MarksEntryController', MarksEntryController)

    MarksEntryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http','$stateParams']
    function MarksEntryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams) {

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }

        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            enableHorizontalScrollbar:0,
            //paginationPageSizes: [10, 20, 30],
            //paginationPageSize: 20,
            enableVerticalScrollbar: 0,
            columnDefs: [
                { name: 'SLNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'studentname', displayName: 'Student Name', width: 300 , cellClass:'textleft' },
                { name: 'amsT_AdmNo', displayName: 'Admission No', width: 120  },
                { name: 'amaY_RollNo', displayName: 'Roll No', type: 'number', width: 75   },
                { name: 'totalMarks', displayName: 'Max Marks' , width: 100 },
                { name: 'minMarks', displayName: 'Min Marks', width: 100 },
                { name: 'marksEnterFor', displayName: 'Marks Enter For', width: 120 },
                { name: 'obtainmarks', displayName: 'Obtain Marks', cellTemplate: '<div class="ui-grid-cell-contents"><input type="text" ng-model="row.entity.obtainmarks"  style="text-align:center;" allow-pattern="[A-Z0-9+-.]" ng-blur="grid.appScope.changemarks(row.entity.marksEnterFor,row.entity.obtainmarks,row)"  class="form-control" value="0"  min="0" </input></div>' }
            ]
        };
        

        $scope.gridOptions.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;
        };
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            apiService.getDATA("ExamEntry/Getdetails").
                then(function (promise) {
                    $scope.acdlist = promise.acdlist;

                    $scope.ASMCL_Id = 'classdefualt';
                    $scope.ASMS_Id = 'sectiondefualt';
                    $scope.EME_Id = 'examdefualt';
                    $scope.ISMS_Id = 'subjectdefualt';

                    $scope.gridOptions.data = "";
                });
        };

        $scope.changemarks = function (totalMarks, obtainmarks, row) {
            var flag = "";
            if ($scope.subMorGFlag === "G") {

                 flag = "false";

                for (var i = 0; i < $scope.gradname.length; i++) {
                    if ($scope.gradname[i] == obtainmarks) {
                        flag = "true";
                    }
                }

                if (obtainmarks === "AB" || obtainmarks === "ab" || obtainmarks === "L" || obtainmarks === "l" || obtainmarks === "M" || obtainmarks === "m") {
                    flag = "true";
                }

                if (flag === "false") {
                    row.entity.obtainmarks = 0;
                    swal('Entered Grade cant be out of master setting...!');
                }
            }
            else {
                 flag = "false";
                if (obtainmarks.match(/[a-z]/i)) {
                    if (obtainmarks === "AB" || obtainmarks === "ab" || obtainmarks === "L" || obtainmarks === "l" || obtainmarks === "M" || obtainmarks === "m") {
                        flag = "true";
                    }
                    if (flag === "false") {
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
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("ExamEntry/onselectAcdYear", data).
                then(function (promise) {
                    $scope.ctlist = promise.ctlist;
                    $scope.ASMCL_Id = 'classdefualt';
                    $scope.gridOptions.data = "";
                });
        };

        $scope.onselectclass = function (ASMCL_Id, ASMAY_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id,
                "ASMCL_Id": ASMCL_Id
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("ExamEntry/onselectclass", data).
                then(function (promise) {
                    $scope.seclist = promise.seclist;
                    $scope.ASMS_Id = 'sectiondefualt';
                    $scope.gridOptions.data = "";
                });
        };

        $scope.cancle = function () {
            $scope.acdlist = $scope.acdlist;
            $scope.ctlist = $scope.ctlist;
            $scope.seclist = $scope.seclist;
            $scope.examlist = $scope.examlist;
            $scope.subjectlist = $scope.subjectlist;
            $scope.gridOptions.data = "";            
        };

        $scope.onselectSection = function (ASMS_Id, ASMCL_Id, ASMAY_Id) {
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
            apiService.create("ExamEntry/onselectSection", data).
                then(function (promise) {

                    $scope.EME_Id = "";
                    $scope.gridOptions.data = "";
                    $scope.examlist = promise.examlist;

                    $scope.EME_Id = 'examdefualt';

                    $scope.subjectlist = promise.subjectlist;

                    $scope.ISMS_Id = 'subjectdefualt';
                });
        };

        $scope.onselectExam = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id) {
            var data = {
                "ASMS_Id": ASMS_Id,
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id,
                "EME_Id": EME_Id
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("ExamEntry/onselectExam", data).
                then(function (promise) {
                    $scope.subjectlist = promise.subjectlist;
                    $scope.gridOptions.data = "";
                });
        };

        $scope.onselectSubject = function () {
            $scope.gridOptions.data = "";
        };

        $scope.onsearch = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.ASMCL_Id === 'classdefualt' || $scope.ASMS_Id === 'sectiondefualt' || $scope.EME_Id === 'examdefualt' || $scope.ISMS_Id === 'subjectdefualt') {

                    if ($scope.ASMCL_Id === 'classdefualt') {
                        swal('Please Select Class');
                        return;
                    }
                    else if ($scope.ASMS_Id === 'sectiondefualt') {
                        swal('Please Select Section');
                        return;
                    }
                    else if ($scope.EME_Id === 'examdefualt') {
                        swal('Please Select Exam');
                        return;
                    }
                    else if ($scope.ISMS_Id === 'subjectdefualt') {
                        swal('Please Select Subject');
                        return;
                    }

                }
                else {
                    var data = {
                        "ASMS_Id": ASMS_Id,
                        "ASMCL_Id": ASMCL_Id,
                        "ASMAY_Id": ASMAY_Id,
                        "EME_Id": EME_Id,
                        "ISMS_Id": ISMS_Id
                    };
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    };
                    apiService.create("ExamEntry/onsearch", data).
                        then(function (promise) {

                            $scope.student = promise.studentList;
                            if (promise.subMorGFlag === "G") {
                                angular.forEach(promise.studentList, function (dd) {
                                    dd.obtainmarks = dd.estM_Grade;
                                });
                            }

                            $scope.gridOptions.data = promise.studentList;
                            $scope.subMorGFlag = promise.subMorGFlag;
                            $scope.gradname = promise.gradname;
                            $scope.marksdeleteflag = promise.marksdeleteflag;

                            $scope.update = promise.saveupdatecount;

                            $scope.eyceidlastdateentry = promise.lastdateentry;
                            if ($scope.eyceidlastdateentry === 0) {
                                swal("Marks Entry Last Date Completed You Can Not Enter Marks");
                                $scope.savemarksbutton = false;
                            } else {
                                $scope.savemarksbutton = true;
                            }

                        });
                }
            }
        };


        $scope.save = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.savedata = $scope.gridOptions.data;
                var data = {
                    "ASMS_Id": ASMS_Id,
                    "ASMCL_Id": ASMCL_Id,
                    "ASMAY_Id": ASMAY_Id,
                    "EME_Id": EME_Id,
                    "ISMS_Id": ISMS_Id,
                    "marksdeleteflag": $scope.marksdeleteflag,
                    "detailsList": $scope.savedata
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("ExamEntry/SaveMarks", data).
                    then(function (promise) {

                        if (promise.messagesaveupdate == "true") {
                            $scope.cancle();
                            //$scope.BindData();
                            swal('Data Saved Successfully');
                        }
                        else {
                            swal('Failed to Save/Update Data');
                        }
                    });
            }
        };

        $scope.cancel = function () {
            $state.reload();
            $scope.BindData();
        };
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.submitted = false;

        $scope.SaveMarks = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id) {
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
                                $scope.save(ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id);
                            }
                            else {
                                flag = "false";
                            }
                        });
                }
                else {
                    $scope.save(ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id);
                }
            }
            else {
                swal('Please select required field / Entered Marks are not in correct format....!');
            }
        };
    }
})();