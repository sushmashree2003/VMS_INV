
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ExamCalculation_SSSEController', ExamCalculation_SSSEController)

    ExamCalculation_SSSEController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function ExamCalculation_SSSEController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.publis = false;

        $scope.BindData = function () {
            apiService.getDATA("ExamCalculation_SSSE/Getdetails").
                then(function (promise) {

                    $scope.yearlt = promise.yearlist;

                });
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

        $scope.get_cls_sections = function (cls_id) {

            if ($scope.asmaY_Id !== "" && $scope.asmaY_Id !== undefined && $scope.asmaY_Id !== null) {

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id

                };

                apiService.create("ExamCalculation_SSSE/get_cls_sections", data).then(function (promise) {
                    $scope.seclist = promise.seclist;
                    $scope.asmS_Id = "";
                    $scope.emE_Id = "";
                    $scope.exsplt = [];
                    if (promise.seclist === null || promise.seclist === "") {
                        swal("Sections are Not Mapped To Selected Class!!!");
                    }
                });
            }
            else {

                swal("Please Select Academic Year  First !!!");
                $scope.asmcL_Id = "";

            }
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

        $scope.onchangeexam = function () {
            $scope.publis = false;
            var data = {
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "EME_Id": $scope.emE_Id
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("ExamCalculation_SSSE/onchangeexam", data).
                then(function (promise) {

                    if (promise.lastdateprocess > 0) {
                        swal("You Can Not Calculate The Marks Last Date For Calculation Is Completed");
                        $scope.processbtn = true;
                    } else {
                        $scope.processbtn = false;
                    }

                    if (promise.retuvrnvalflag === true) {
                        $scope.publis = true;
                        $scope.retuvrnvalflag = promise.retuvrnvalflag;
                        $scope.retuvrnvalpublishflag = promise.retuvrnvalpublishflag;
                    } else {
                        $scope.publis = false;
                    }

                });
        };

        $scope.submitted = false;

        $scope.saveddata = function (asmaY_Id, asmcL_Id, asmS_Id, emE_Id) {

            if ($scope.myForm.$valid) {


                if ($scope.retuvrnvalpublishflag === true) {

                    swal({

                        title: "Marks Are Already Updated To Student Portal",
                        text: "Do you want to Recalculate The Marks Again ??",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Calculate!",
                        cancelButtonText: "No, Cancel Calculation!",
                        closeOnConfirm: false,
                        closeOnCancel: false
                    },
                        function (isConfirm) {
                            if (isConfirm) {

                                var ESTMP_PublishToStudentFlg = 0;
                                var data = {
                                    "ASMAY_Id": asmaY_Id,
                                    "ASMCL_Id": asmcL_Id,
                                    "ASMS_Id": asmS_Id,
                                    "EME_Id": emE_Id,
                                    "ESTMP_PublishToStudentFlg": ESTMP_PublishToStudentFlg
                                };
                                apiService.create("ExamCalculation_SSSE/Calculation", data).
                                    then(function (promise) {

                                        if (promise.returnval === true) {

                                            swal('Marks Calculated Successfully');
                                        }
                                        else {

                                            swal('Marks Not Calculated !!!');
                                        }
                                    });

                            }
                            else {
                                swal("Marks Calculation Process Is Cancelled");

                            }
                        });
                } else {

                    var ESTMP_PublishToStudentFlg = 0;

                    var data = {
                        "ASMAY_Id": asmaY_Id,
                        "ASMCL_Id": asmcL_Id,
                        "ASMS_Id": asmS_Id,
                        "EME_Id": emE_Id,
                        "ESTMP_PublishToStudentFlg": ESTMP_PublishToStudentFlg
                    };
                    apiService.create("ExamCalculation_SSSE/Calculation", data).then(function (promise) {
                        if (promise.returnval === true) {
                            swal('Marks Calculated Successfully');
                        }
                        else {
                            swal('Marks Not Calculated !!!');
                        }
                        $scope.clear();
                    });
                }
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.saveapporvecal = function (asmaY_Id, asmcL_Id, asmS_Id, emE_Id) {

            if ($scope.myForm.$valid) {

                var ESTMP_PublishToStudentFlg = 0;
                var data = {
                    "ASMAY_Id": asmaY_Id,
                    "ASMCL_Id": asmcL_Id,
                    "ASMS_Id": asmS_Id,
                    "EME_Id": emE_Id
                };

                apiService.create("ExamCalculation_SSSE/saveapporvecal", data).
                    then(function (promise) {

                        if (promise.returnval === true) {

                            swal('Marks Published Successfully');
                        }
                        else {
                            swal('Failed To Published Marks');
                        }
                    });

            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.clear = function () {
            // $state.reload();
            //  $scope.BindData();
            $scope.asmaY_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.clslist = [];
            $scope.seclist = [];
            $scope.exsplt = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $state.reload();
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

        $scope.interacted = function (field) {

            return $scope.submitted;
        };
    }

})();