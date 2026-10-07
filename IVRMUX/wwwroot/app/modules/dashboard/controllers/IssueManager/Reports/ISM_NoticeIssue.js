(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_NoticeIssueController', ISM_NoticeIssueController);
    ISM_NoticeIssueController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$compile'];
    function ISM_NoticeIssueController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $compile) {

        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        $scope.obj = {};
        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.multi_mi = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();
        $scope.currentdate = new Date();
        $scope.get_plannerlist = [];
        $scope.loaddata = function () {
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            $scope.currentPage2 = 1;
            $scope.itemsPerPage2 = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.page = "page";
            $scope.page1 = "page1";
            $scope.page2 = "page2";
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("ISM_NoticeIssue/getdetails", pageid).then(function (promise) {
                if (promise.get_Memolist !== null && promise.get_Memolist.length > 0) {
                    $scope.get_Memolist = promise.get_Memolist;
                }
                else {
                    swal("No Records Found !!");
                }
            });
        };

        //******************************* Check Selection ****************************//
        $scope.all_check = function (ck) {
            $scope.checkall = ck;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.get_userEmplist, function (st) {
                st.empck = toggleStatus;
            });
        };
        $scope.togchkbx = function () {
            $scope.checkall = $scope.get_userEmplist.every(function (options) {
                return options.empck;
            });
        };
        $scope.isOptionsRequiredE = function () {
            return !$scope.get_userEmplist.some(function (options) {
                return options.empck;
            });
        };


        $scope.togglecheckboxAll = function (checkallemp) {
            var toggleStatus = checkallemp;
            angular.forEach($scope.get_Memolist, function (slc) {
                slc.checked = toggleStatus;
            });
        };
        $scope.optionToggled = function () {
            $scope.checkallemp = $scope.get_Memolist.every(function (options) {
                return options.checked;
            });
        };

        $scope.editplannerdetails = function (objedit) {
            $scope.editflg = 1;
            objedit.editflg = $scope.editflg;
            $scope.getplannerdetails(objedit);
        };


        $scope.getplannerdetailsviewcat = function (objp) {
            var plannerid = 0;
            $scope.alltask = "";
            $scope.get_plannerdetails = [];

            if (objp.editflg === 1) {
                angular.forEach($scope.get_updatedplannerlist, function (objpl) {
                    if (objp.ISMTPL_Id === objpl.ISMTPL_Id) {
                        plannerid = objpl.ISMTPL_Id;
                    }
                });
            }
            else {
                $scope.editflg = 0;
                angular.forEach($scope.get_plannerlist, function (objpl) {
                    if (objp.ISMTPL_Id === objpl.ISMTPL_Id) {
                        plannerid = objpl.ISMTPL_Id;
                    }
                });
            }
            $scope.plannerid = plannerid;
            var data = {
                "ISMTPL_Id": $scope.plannerid,
                "HRMD_Id": $scope.hrmD_Id,
                "miarray": $scope.miarray
            };
            apiService.create("ISM_NoticeIssue/getplannerdetails", data).then(function (promise) {
                $scope.plannercategory = promise.get_plannercategory;
            });
        };
        $scope.statusAll = function (allsts) {
            if (allsts === "1") {
                angular.forEach($scope.get_plannerdetails, function (pck) {
                    //if (pck.ISMTPLTA_Status === 'Open') {
                    pck.plannerStatus = 1;
                    //}
                });
            }
            else {
                angular.forEach($scope.get_plannerdetails, function (pck) {
                    //if (pck.ISMTPLTA_Status === 'Open') {
                    pck.plannerStatus = 0;
                    //}
                });
            }
            $scope.onassignCheck();
            angular.forEach($scope.get_comptasklist, function (atck) {
                if (atck.assignedflg === false) {
                    atck.checkedvalue = toggleStatus;
                }
            });
        };

        $scope.all_task = function (atk) {
            var totcheck = 0;
            var toteffort = 0;
            $scope.alltask = atk;
            var toggleStatus = $scope.alltask;
            angular.forEach($scope.get_plannerdetails, function (atck) {
                //if (atck.ISMTPLTA_Status === 'Open') {
                atck.checkedPlan = toggleStatus;
                //}
                if (atck.checkedPlan === true && (atck.plannerStatus === "1" || atck.plannerStatus === 1)) {
                    totcheck = totcheck + 1;
                    toteffort += parseFloat(atck.ISMTPLTA_EffortInHrs);
                }
                //totcheck = totcheck + 1;
                //toteffort += parseFloat(atck.ISMTPLTA_EffortInHrs);
            });
            $scope.totalcheck = totcheck;
            $scope.totaleffort = toteffort;
        };

        $scope.counteffort = function () {
            var finaleffort = 0;
            angular.forEach($scope.get_plannerdetails, function (ack) {
                finaleffort += parseFloat(ack.ISMTPLTA_EffortInHrs);
            });
            $scope.totaleffort = finaleffort;
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.cancel = function () {
            $state.reload();
        };

        //VIEW MEMO NOTICE
        $scope.viewempemailmemo = function (empid) {
            $scope.arrayuserEmp = [];
            angular.forEach($scope.get_Memolist, function (em) {
                if (em.checked === true && em.ISMEMN_No === empid) {
                    $scope.arrayuserEmp = [];
                    em.comp_Date = new Date(em.comp_Date).toDateString(),
                        $scope.start_Date = em.ISMTPL_StartDate,
                        $scope.end_Date = em.ISMTPL_EndDate,
                        $scope.arrayuserEmp.push(em);
                }
            });

            var Deviationtype = "Emp";
            var commutype = "Notice";

            var data = {
                "communicationfalg": commutype,
                "communicationmsg": $scope.messgaeboxdata,
                "EmplistforMNNE": $scope.arrayuserEmp,
                "startdate": new Date($scope.start_Date).toDateString(),
                "enddate": new Date($scope.end_Date).toDateString(),
                "Deviationreporttype": Deviationtype,
                "noofdays": $scope.noofdays,
                "type": false
            };
            apiService.create("ISM_PlannerDeptDeviationReport/gettaskdetails", data).then(function (promise) {
                if (promise.message !== "") {
                    $('#myModalmemo').modal('show');
                    var e1 = angular.element(document.getElementById("test"));
                    $compile(e1.html(promise.html))(($scope));
                }
                else {
                    var e1 = angular.element(document.getElementById("test"));
                    $compile(e1.html(""))(($scope));
                    $('#myModalmemo').modal('hide');
                    swal("No Pending Issues Found..... !!");
                }
            });
        };

        $scope.viewresponsetask = function (taskobj) {
            var data = {
                "HRME_Id": taskobj.HRME_Id,
                "ISMEMN_No": taskobj.ISMEMN_No,
                "communicationfalg": "Notice",
                "type": true
            };
            apiService.create("ISM_PlannerDeptDeviationReport/gettaskdetails", data).
                then(function (promise) {
                    if (promise.get_taskdetails !== null && promise.get_taskdetails.length > 0) {
                        $('#myModalResponse').modal('show');
                        $scope.get_taskdetails = promise.get_taskdetails;
                    }
                    else {
                        $('#myModalResponse').modal('hide');
                        $scope.get_taskdetails = [];
                        swal("No issues pednding for these dates!!");
                    }
                });
        };

        $scope.closeimg = function () {
            $scope.searchValue = '';
            $('#myModalResponse').modal('hide');
            $('.modal-backdrop').remove();

        };

        //*************************** MEMO,NOTICE,NOTIFICATION,EMAIL SENDING *********************//
        $scope.empemailmemo = function (type) {
            $scope.arrayuserEmp = [];
            var completeddate = true;

            angular.forEach($scope.get_Memolist, function (em) {
                if (em.checked === true) {
                    if (em.comp_Date === null || em.comp_Date === 'Invalid Date' || em.comp_Date === undefined) {
                        completeddate = false;
                        swal("Please Select Complete By Date for the Employee " + em.employeename);
                    }
                    else {
                        em.comp_Date = new Date(em.comp_Date).toDateString(),
                            em.start_Date = em.ISMTPL_StartDate,
                            em.end_Date = em.ISMTPL_EndDate,
                            $scope.arrayuserEmp.push(em);
                    }
                }
            });

            var Deviationtype = "Emp";

            if (completeddate === true) {
                if ($scope.arrayuserEmp.length > 0) {
                    var  commutype = "Notice";
                    swal({
                        title: "Are you sure",
                        text: "Do you want to send " + commutype + " ?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes",
                        cancelButtonText: "Cancel!",
                        closeOnConfirm: true,
                        closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                var data = {
                                    "communicationfalg": commutype,
                                    "communicationmsg": $scope.messgaeboxdata,
                                    "EmplistforMNNE": $scope.arrayuserEmp,                                    
                                    "Deviationreporttype": Deviationtype,
                                    "noofdays": 0,
                                    "type": true
                                };
                                apiService.create("ISM_PlannerDeptDeviationReport/empcommunication", data).then(function (promise) {
                                    if (promise.message === "success") {
                                        if ($scope.Email === true || $scope.Notif === true) {
                                            $('#myModal').modal('hide');
                                            $scope.messgaeboxdata = "";
                                        }
                                        if ($scope.Notif === true) {
                                            swal("Notification Sent Successfully!!");
                                        }
                                        else {
                                            $state.reload();
                                            swal(commutype + " Sent Successfully!!");
                                        }
                                    }
                                    else {
                                        swal("No Record  Found..... !!");
                                    }
                                });
                            }
                            else {
                                //$state.reload();
                            }

                        });

                }
                else {
                    swal("Select Any Employee..!!");
                }
            };

        };

    }
})();

