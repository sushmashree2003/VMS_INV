
(function () {
    'use strict';
    angular
        .module('app')
        .controller('IVRM_InteractionsController', IVRM_InteractionsController);

    IVRM_InteractionsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$interval', '$timeout'];
    function IVRM_InteractionsController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, $interval, $timeout) {

        $scope.isminT_DateTime = new Date();
        $scope.iintsS_Date = new Date();
        $scope.composetab = false;
        $scope.inboxtab = true;
        $scope.composehead = false;
        //Auto Generate Number
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        $scope.obj = {};

        //===================Compose Message
        //$scope.oncompose = function () {
        //    $scope.composetab = true;
        //    $scope.composehead = true;
        //    $scope.inboxtab = false;

        //};
        $scope.oninbox = function () {
            $scope.composetab = false;
            $scope.composehead = false;
            $scope.inboxtab = true;
        };
        $scope.typechangeG = function () {
            $scope.ISMINT_GroupOrIndFlg = "Group";
            $scope.groupindvalue = true;
            $scope.asmcL_Id = "";
            $scope.get_student = "";
            $scope.composetab = true;
            $scope.composehead = true;
            $scope.inboxtab = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };
        $scope.typechangeI = function () {
            $scope.ISMINT_GroupOrIndFlg = "Individual";
            $scope.groupindvalue = false;
            $scope.asmcL_Id = "";
            $scope.get_student = "";
            $scope.composetab = true;
            $scope.composehead = true;
            $scope.inboxtab = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();

        };
        $scope.refresh = function () {
            $scope.loaddata();
        };
        //===================Page Load
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("IVRM_Interactions/getloaddata", pageid).
                then(function (promise) {
                    $scope.roletype = promise.roletype;
                    $scope.roleflg = $scope.roletype[0].ivrmrT_Role;
                    $scope.configflag = promise.configflag;
                    $scope.stflag = $scope.configflag[0].ivrmgC_EnableSTIntFlg;
                    $scope.ctflag = $scope.configflag[0].ivrmgC_EnableCTIntFlg;
                    $scope.hodflag = $scope.configflag[0].ivrmgC_EnableHODIntFlg;
                    $scope.principalflag = $scope.configflag[0].ivrmgC_EnablePrincipalIntFlg;
                    $scope.asflag = $scope.configflag[0].ivrmgC_EnableASIntFlg;
                    $scope.ecflag = $scope.configflag[0].ivrmgC_EnableECIntFlg;
                    $scope.getinboxmsg = promise.getinboxmsg;

                    $scope.message_list = [];
                    angular.forEach($scope.getinboxmsg, function (mg) {
                        if ($scope.message_list.length === 0) {
                            $scope.message_list.push({ ISMINT_Id: mg.ISMINT_Id, ISTINT_Id: mg.ISTINT_Id, ISMINT_InteractionId: mg.ISMINT_InteractionId, ISMINT_Subject: mg.ISMINT_Subject, ISMINT_Interaction: mg.ISMINT_Interaction, ISMINT_GroupOrIndFlg: mg.ISMINT_GroupOrIndFlg, ISMINT_ComposedByFlg: mg.ISMINT_ComposedByFlg, Sender: mg.Sender, Receiver: mg.Receiver, ISMINT_DateTime: mg.ISMINT_DateTime });
                        }
                        else if ($scope.message_list.length > 0) {
                            var al_cnt = 0;
                            angular.forEach($scope.message_list, function (sl) {
                                if (sl.ISMINT_Id === mg.ISMINT_Id) {
                                    al_cnt += 1;
                                }
                            });
                            if (al_cnt === 0) {
                                $scope.message_list.push({ ISMINT_Id: mg.ISMINT_Id, ISTINT_Id: mg.ISTINT_Id, ISMINT_InteractionId: mg.ISMINT_InteractionId, ISMINT_Subject: mg.ISMINT_Subject, ISMINT_Interaction: mg.ISMINT_Interaction, ISMINT_GroupOrIndFlg: mg.ISMINT_GroupOrIndFlg, ISMINT_ComposedByFlg: mg.ISMINT_ComposedByFlg, Sender: mg.Sender, Receiver: mg.Receiver, ISMINT_DateTime: mg.ISMINT_DateTime });
                            }
                        }
                        $scope.inboxtotal = $scope.message_list.length;
                    });

                });
        };


        //===================On Radio Change
        $scope.getdetail = function () {
            $scope.empName = "";
            $scope.hrmE_Id = "";
            $scope.usercheckSTU = "";
            $scope.usercheckST = "";
            $scope.usercheckT = "";
            $scope.sectionList = "";
            $scope.getdetails = "";
            var data = {
                "roleflg": $scope.roleflg,
                "userflag": $scope.userflag
            };

            apiService.create("IVRM_Interactions/getdetails", data).
                then(function (promise) {

                    if (promise.getdetails.length > 0) {
                        $scope.getdetails = promise.getdetails;
                        $scope.empName = promise.getdetails[0].EmpName;
                        $scope.hrmE_Id = promise.getdetails[0].HRME_Id;
                        $scope.asmay_id = promise.getdetails[0].ASMAY_Id;
                    }
                    else {
                        swal("No Record Found....");
                        $scope.getdetails.length = 0;
                    }
                });
        };

        //========================= Class Change
        $scope.getsection = function (asmcL_Id) {
            $scope.get_student = "";
            $scope.asmcL_Id = asmcL_Id;
            var data = {
                "asmcL_Id": asmcL_Id,
                "ASMAY_Id": $scope.asmay_id
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("EmployeeStudentAttendenceDetails/Getsection", data).
                then(function (promise) {
                    $scope.sectionList = promise.sectionList;
                });
        };
        //=================== getstudent on Class Change
        $scope.getstudent = function () {
            $scope.get_student = "";
            var data = {
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id
            };
            apiService.create("IVRM_Interactions/getstudent", data).
                then(function (promise) {
                    if (promise.get_student !== null) {
                        $scope.get_student = promise.get_student;
                    }
                    else if ($scope.get_student.length === "0") {
                        swal("No Record Found....");
                    }
                });
        };

        //======================== Select All and multiselect validation
        //==== Student
        $scope.togchkbxSTU = function () {
            $scope.usercheckSTU = $scope.get_student.every(function (itm) {
                return itm.stuck;
            });
        };
        $scope.isOptionsRequiredSTU = function () {
            if ($scope.userflag === "Student" && $scope.groupindvalue === true) {
                return !$scope.get_student.some(function (options) {
                    return options.stuck;
                });
            }
        };
        $scope.all_checkSTU = function (subj) {
            $scope.usercheckSTU = subj;
            var toggleStatus = $scope.usercheckSTU;
            angular.forEach($scope.get_student, function (st) {
                st.stuck = toggleStatus;
            });
        };
        //==== Teacher
        $scope.togchkbxT = function () {
            $scope.usercheckT = $scope.getdetails.every(function (itm) {
                return itm.tck;
            });
        };
        $scope.isOptionsRequiredT = function () {
            if ($scope.userflag === "Teachers" && $scope.groupindvalue === true) {
                return !$scope.getdetails.some(function (options) {
                    return options.tck;
                });
            }
        };
        $scope.all_checkT = function (subj) {
            $scope.usercheckT = subj;
            var toggleStatus = $scope.usercheckT;
            angular.forEach($scope.getdetails, function (gd) {
                gd.tck = toggleStatus;
            });
        };
        //==== Subject Teacher
        $scope.togchkbxST = function () {
            $scope.usercheckST = $scope.getdetails.every(function (itm) {
                return itm.stck;
            });
        };
        $scope.isOptionsRequiredST = function () {
            if ($scope.userflag === "SubjectTeacher" && $scope.groupindvalue === true) {
                return !$scope.getdetails.some(function (options) {
                    return options.stck;
                });
            }
        };
        $scope.all_checkST = function (subj) {
            $scope.usercheckST = subj;
            var toggleStatus = $scope.usercheckST;
            angular.forEach($scope.getdetails, function (gd) {
                gd.stck = toggleStatus;
            });
        };

        //===================Save Data / Send Message     
        $scope.savedata = function () {
            $scope.submitted = true;
            var data = {};
            if ($scope.myForm.$valid) {
                if ($scope.roleflg === "Student") {
                    if ($scope.userflag === "SubjectTeacher") {
                        $scope.arrayST = [];
                        angular.forEach($scope.getdetails, function (st) {
                            if (st.stck === true) {
                                $scope.arrayST.push(st);
                            }
                        });
                        if ($scope.ISMINT_GroupOrIndFlg === "Group") {
                            data = {
                                "roleflg": $scope.roleflg,
                                "userflag": $scope.userflag,
                                "arrayST": $scope.arrayST,
                                "ISMINT_ComposedByFlg": $scope.roleflg,
                                "ISMINT_GroupOrIndFlg": $scope.ISMINT_GroupOrIndFlg,
                                "ISMINT_Subject": $scope.isminT_Subject,
                                "ISMINT_Interaction": $scope.isminT_Interaction,
                                "ISMINT_Id": $scope.isminT_Id
                            };
                        }
                        else {
                            data = {
                                "roleflg": $scope.roleflg,
                                "userflag": $scope.userflag,
                                "HRME_Id": $scope.hrmE_Id,
                                "ISMINT_ComposedByFlg": $scope.roleflg,
                                "ISMINT_GroupOrIndFlg": $scope.ISMINT_GroupOrIndFlg,
                                "ISMINT_Subject": $scope.isminT_Subject,
                                "ISMINT_Interaction": $scope.isminT_Interaction,
                                "ISMINT_Id": $scope.isminT_Id
                            };
                        }


                    }
                    else if ($scope.userflag === "ClassTeacher" || $scope.userflag === "HOD" || $scope.userflag === "Principal" || $scope.userflag === "AS" || $scope.userflag === "EC") {
                        data = {
                            "roleflg": $scope.roleflg,
                            "userflag": $scope.userflag,
                            "ISTINT_ToId": $scope.hrmE_Id,
                            "ISMINT_ComposedByFlg": $scope.roleflg,
                            "ISMINT_GroupOrIndFlg": $scope.ISMINT_GroupOrIndFlg,
                            "ISMINT_Subject": $scope.isminT_Subject,
                            "ISMINT_Interaction": $scope.isminT_Interaction,
                            "ISMINT_Id": $scope.isminT_Id
                        };
                    }
                }
                else if ($scope.roleflg === "Staff") {

                    if ($scope.userflag === "Student") {
                        $scope.arrayStudent = [];
                        angular.forEach($scope.get_student, function (stu) {
                            if (stu.stuck === true) {
                                $scope.arrayStudent.push(stu);
                            }
                        });
                        if ($scope.ISMINT_GroupOrIndFlg === "Group") {
                            data = {
                                "roleflg": $scope.roleflg,
                                "userflag": $scope.userflag,
                                "arrayStudent": $scope.arrayStudent,
                                "ISMINT_ComposedByFlg": $scope.roleflg,
                                "ISMINT_GroupOrIndFlg": $scope.ISMINT_GroupOrIndFlg,
                                "ISMINT_Subject": $scope.isminT_Subject,
                                "ISMINT_Interaction": $scope.isminT_Interaction,
                                "ISMINT_Id": $scope.isminT_Id
                            };
                        }
                        else {
                            data = {
                                "roleflg": $scope.roleflg,
                                "userflag": $scope.userflag,
                                "student_Id": $scope.obj.amsT_Id.amsT_Id,
                                "ISMINT_ComposedByFlg": $scope.roleflg,
                                "ISMINT_GroupOrIndFlg": $scope.ISMINT_GroupOrIndFlg,
                                "ISMINT_Subject": $scope.isminT_Subject,
                                "ISMINT_Interaction": $scope.isminT_Interaction,
                                "ISMINT_Id": $scope.isminT_Id
                            };
                        }
                    }
                    else if ($scope.userflag === "Teachers") {
                        $scope.arrayTeachers = [];
                        angular.forEach($scope.getdetails, function (tec) {
                            if (tec.tck === true) {
                                $scope.arrayTeachers.push(tec);
                            }
                        });
                        if ($scope.ISMINT_GroupOrIndFlg === "Group") {
                            data = {
                                "roleflg": $scope.roleflg,
                                "userflag": $scope.userflag,
                                "arrayTeachers": $scope.arrayTeachers,
                                "ISMINT_ComposedByFlg": $scope.roleflg,
                                "ISMINT_GroupOrIndFlg": $scope.ISMINT_GroupOrIndFlg,
                                "ISMINT_Subject": $scope.isminT_Subject,
                                "ISMINT_Interaction": $scope.isminT_Interaction,
                                "ISMINT_Id": $scope.isminT_Id
                            };
                        }
                        else {
                            data = {
                                "roleflg": $scope.roleflg,
                                "userflag": $scope.userflag,
                                "employee_Id": $scope.obj.hrmE_Id.HRME_Id,
                                "ISMINT_ComposedByFlg": $scope.roleflg,
                                "ISMINT_GroupOrIndFlg": $scope.ISMINT_GroupOrIndFlg,
                                "ISMINT_Subject": $scope.isminT_Subject,
                                "ISMINT_Interaction": $scope.isminT_Interaction,
                                "ISMINT_Id": $scope.isminT_Id
                            };
                        }
                    }
                    else if ($scope.userflag === "HOD" || $scope.userflag === "Principal" || $scope.userflag === "AS" || $scope.userflag === "EC") {
                        data = {
                            "roleflg": $scope.roleflg,
                            "userflag": $scope.userflag,
                            "ISTINT_ToId": $scope.hrmE_Id,
                            "ISMINT_ComposedByFlg": $scope.roleflg,
                            "ISMINT_GroupOrIndFlg": $scope.ISMINT_GroupOrIndFlg,
                            "ISMINT_Subject": $scope.isminT_Subject,
                            "ISMINT_Interaction": $scope.isminT_Interaction,
                            "ISMINT_Id": $scope.isminT_Id
                        };
                    }
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("IVRM_Interactions/savedetails", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.isminT_Id === 0 || promise.isminT_Id < 0) {
                            swal('Message sent successfully');
                        }
                    }
                    else {
                        if (promise.isminT_Id === 0 || promise.isminT_Id < 0) {
                            swal('Failed to send, please contact administrator');
                        }
                        else if (promise.isminT_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //===================
        $scope.reply = function (objs) {
            $scope.id = objs.ISMINT_Id;
            var data = {
                "ISMINT_Id": $scope.id
            };
            apiService.create("IVRM_Interactions/reply", data).
                then(function (promise) {
                    if (promise.viewmessage !== null) {
                        $scope.viewmessage = promise.viewmessage;
                        $scope.subject = objs.ISMINT_Subject;
                        $scope.subjdetails = objs.ISMINT_Interaction;
                    }
                });
        };

        $scope.replymsg = function () {
            angular.element('#viewrplyModal').modal('hide');
        };

        //===================Save Staff/Student Reply       
        $scope.savereply = function (rply) {
            $scope.submittedR = true;
            if ($scope.myFormR.$valid) {
                var data = {
                    "ISMINT_Id": $scope.id,
                    "ISTINT_ComposedByFlg": $scope.roleflg,
                    "ISTINT_Interaction": $scope.istinT_Interaction,
                    "ISTINT_Id": $scope.istinT_Id
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };

                apiService.create("IVRM_Interactions/savereply", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.istinT_Id === 0 || promise.istinT_Id < 0) {
                            swal('Message sent successfully');
                        }
                    }
                    else {
                        if (promise.istinT_Id === 0 || promise.istinT_Id < 0) {
                            swal('Failed to send, please contact administrator');
                        }
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submittedR = true;
            }
        };


    }
})();