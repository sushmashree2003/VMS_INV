(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_PlannerCreationController', ISM_PlannerCreationController);
    ISM_PlannerCreationController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$window'];
    function ISM_PlannerCreationController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter, $window) {
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.getserverdate = function () {
            var xmlHttp;
            function srvTime() {
                try {
                    //FF, Opera, Safari, Chrome
                    xmlHttp = new XMLHttpRequest();
                }
                catch (err1) {
                    //IE
                    try {
                        xmlHttp = new ActiveXObject('Msxml2.XMLHTTP');
                    }
                    catch (err2) {
                        try {
                            xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
                        }
                        catch (eerr3) {
                            //AJAX not supported, use CPU time.
                            alert("AJAX not supported");
                        }
                    }
                }
                xmlHttp.open('HEAD', window.location.href.toString(), false);
                xmlHttp.setRequestHeader("Content-Type", "text/html");
                xmlHttp.send('');
                return xmlHttp.getResponseHeader("Date");
            }
            $scope.today = srvTime();
        }
        $scope.getserverdate();

        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        $scope.page = "page";
        $scope.page2 = "page2";
        $scope.plannerflg = false;
        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.typeFlg = "Planner";
        $scope.ismtcrastO_AssignedDate = new Date($scope.today);
        $scope.ismtcrtrtO_TransferredDate = new Date($scope.today);
        var today = (new Date($scope.today)).getDay();

        $scope.loaddata = function () {
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            var data = {
                "typeFlg": $scope.typeFlg,
                "ISMTPL_StartDate": new Date($scope.ismtpL_StartDate).toDateString(),
                "ISMTPL_EndDate": new Date($scope.ismtpL_EndDate).toDateString()
            };
            $scope.assignedtasklist = [];
            apiService.create("ISM_PlannerCreation/getdetails", data).then(function (promise) {
              
                $scope.pending = false;
                $scope.plannerextapproval = promise.plannerextapproval;
                $scope.plannerflg = $scope.plannerextapproval;
                // $scope.plannerflg = true;
                $scope.get_Status = promise.get_Status;
                $scope.get_employeelist = promise.get_employeelist;
                $scope.get_department = promise.get_department;
                $scope.holidaylist = promise.holidaylist;
                $scope.get_plannerlist = promise.get_plannerlist;
                $scope.plannerMaxdate = promise.plannerMaxdate;
                $scope.plMaxdate = new Date($scope.plannerMaxdate);
                $scope.plMaxdate.setDate($scope.plMaxdate.getDate());
                $scope.plannercategory = promise.categorylist;

                angular.forEach($scope.get_plannerlist, function (tf) {
                    var decimaltimeold = tf.ISMTPL_TotalHrs;
                    var hrsold = parseInt(Number(decimaltimeold));
                    var minold = Math.round((Number(decimaltimeold) - hrsold) * 60);
                    if (hrsold.toString().length === 1) {
                        hrsold = '0' + hrsold;
                    }
                    if (minold.toString().length === 1) {
                        minold = '0' + minold;
                    }
                    tf.effhrsold = hrsold;
                    tf.effminold = minold;
                });

                if (promise.rtO_VehcalList != null && promise.rtO_VehcalList.length > 0) {
                    $scope.rtO_VehcalList = promise.rtO_VehcalList;
                    $('#popup111').modal('show');
                }
                else {
                    $('#popup111').modal('hide');
                }
                if (promise.get_plannercategory != null && promise.get_plannercategory.length > 0) {
                    $scope.generate = false;
                    swal({
                        title: "You Can Not Generate Daily Report Because Still You Did Not Reached They weekly Target " + promise.get_plannercategory[0].TaskCategoryOutput + " ",
                        text: "Do You Want To update..?",
                        type: "warning",
                        // showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, update!",
                        //   cancelButtonText: "Cancel",
                        closeOnConfirm: true,
                        // closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                intracontroller.viewcompleted = true;
                                $('.modal-backdrop').remove();
                                $('.modal').on('hide.bs.modal', function (e) {
                                    e.stopPropagation();
                                    $('body').css('padding-right', '');
                                });
                                $window.location.href = 'https://' + HostName + '/#/app/MasterChannelPartner/12';
                            }

                        });
                }

            }).finally(function () {
                // Ensures this is called after the promise is resolved
                $scope.getcenteenfeedback();

            })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;
                $scope.reverse = !$scope.reverse;
            };

        };


        //Get Feedback
        $scope.getcenteenfeedback = function () {

            var data = {
                //"FMTY_Id": $scope.FMTY_Id
                //"CanteenFeedback": ["Canteen", "HouseKeeping"]
                "FMTY_FeedbackTypeName": "HouseKeeping"
            };
            apiService.create("FeedbackSchoolGeneralTransaction/getfeedback1", data).then(function (promise) {
                if (promise !== null) {
                    if ((promise.typelist !== null && promise.typelist.length > 0)
                        && (promise.mappedquestiondeetails !== null && promise.mappedquestiondeetails.length > 0)
                        && (promise.mappedoptiondeetails !== null && promise.mappedoptiondeetails.length > 0)) {
                        if ($scope.plannerflg == true) {
                            if (promise.given == "Given") {
                                $('#CanteenFeedback').modal('hide');
                            }
                            else {
                                $('#CanteenFeedback').modal('show');
                                $scope.canteentemparray = [];
                                $scope.tempquestionarray = [];
                                $scope.tempoptionarray = [];
                                $scope.typelist = promise.typelist;
                                $scope.mappedquestiondeetails = promise.mappedquestiondeetails;
                                $scope.mappedoptiondeetails = promise.mappedoptiondeetails;
                                if ($scope.typelist[0].fmtY_QuestionwiseOptionFlg === false) {
                                    angular.forEach($scope.typelist, function (dd) {

                                        $scope.tempquestionarray = [];
                                        $scope.tempoptionarray = [];
                                        angular.forEach($scope.mappedquestiondeetails, function (ddd) {
                                            if (dd.fmtY_Id === ddd.fmtY_Id && !ddd.fmqE_ManualEntryFlg) {
                                                $scope.tempoptionarray = [];
                                                angular.forEach($scope.mappedoptiondeetails, function (dddd) {
                                                    if (dd.fmtY_Id === dddd.fmtY_Id) {
                                                        $scope.tempoptionarray.push({ FMOP_Id: dddd.fmoP_Id, FMOP_FeedbackOptions: dddd.fmoP_FeedbackOptions });
                                                    }
                                                });

                                                $scope.tempquestionarray.push({
                                                    FMQE_Id: ddd.fmqE_Id, FMQE_FeedbackQuestions: ddd.fmqE_FeedbackQuestions,
                                                    opt: $scope.tempoptionarray, manualflg: ddd.fmqE_ManualEntryFlg
                                                }); task
                                            } else {
                                                $scope.tempquestionarray.push({
                                                    FMQE_Id: ddd.fmqE_Id, FMQE_FeedbackQuestions: ddd.fmqE_FeedbackQuestions,
                                                    manualflg: ddd.fmqE_ManualEntryFlg
                                                });
                                            }
                                        });
                                        $scope.canteentemparray.push({ FMTY_Id: dd.fmtY_Id, FMTY_FeedbackTypeName: dd.fmtY_FeedbackTypeName, ques: $scope.tempquestionarray });

                                    });
                                } else {
                                    angular.forEach($scope.typelist, function (dd) {

                                        $scope.tempquestionarray = [];
                                        $scope.tempoptionarray = [];
                                        angular.forEach($scope.mappedquestiondeetails, function (ddd) {
                                            if (dd.fmtY_Id === ddd.fmtY_Id && !ddd.fmqE_ManualEntryFlg) {
                                                $scope.tempoptionarray = [];
                                                angular.forEach($scope.mappedoptiondeetails, function (dddd) {
                                                    if (dd.fmtY_Id === dddd.fmtY_Id && ddd.fmqE_Id === dddd.fmqE_Id) {
                                                        $scope.tempoptionarray.push({ FMOP_Id: dddd.fmoP_Id, FMOP_FeedbackOptions: dddd.fmoP_FeedbackOptions });
                                                    }
                                                });

                                                $scope.tempquestionarray.push({
                                                    FMQE_Id: ddd.fmqE_Id, FMQE_FeedbackQuestions: ddd.fmqE_FeedbackQuestions,
                                                    opt: $scope.tempoptionarray, manualflg: ddd.fmqE_ManualEntryFlg
                                                });
                                            } else {
                                                $scope.tempquestionarray.push({
                                                    FMQE_Id: ddd.fmqE_Id, FMQE_FeedbackQuestions: ddd.fmqE_FeedbackQuestions,
                                                    manualflg: ddd.fmqE_ManualEntryFlg
                                                });
                                            }
                                        });
                                        $scope.canteentemparray.push({ FMTY_Id: dd.fmtY_Id, FMTY_FeedbackTypeName: dd.fmtY_FeedbackTypeName, ques: $scope.tempquestionarray });
                                    });
                                }
                            }
                        }



                        //$('#CanteenFeedback').modal('show');
                    } else {
                        $scope.canteentemparray = [];
                        $scope.tempquestionarray = [];
                        $scope.tempoptionarray = [];
                        //swal("No Records Found");
                    }
                } else {
                    $scope.canteentemparray = [];
                    $scope.tempquestionarray = [];
                    $scope.tempoptionarray = [];
                    swal("Something Went Wrong Kidndly Contact Administrator");
                }

            });
        };
        $scope.submitted123 = false;
        $scope.savecanteenfeedback = function (user) {
            if ($scope.myForm123.$valid) {
                var data = {
                    temp: $scope.canteentemparray,
                    "Flag": 'HouseKeeping'
                };
                apiService.create("FeedbackSchoolGeneralTransaction/savecanteen", data).
                    then(function (promise) {
                        if (promise !== null) {
                            if (promise.returnval === true) {
                                swal("Feed Back Saved Successfully", "", "success");
                                $('#CanteenFeedback').modal('hide');
                            } else {
                                swal("Failed To Save Feed Back");
                            }
                        } else {
                            swal("Something Went Wrong Kindly Contact Administrator");
                        }
                        // $state.reload();
                    });

            }
            else {
                $scope.submitted123 = true;
            }
        };
        $scope.interactedcanteen = function (field) {
            return $scope.submitted123;
        };
        //========================================= Add task Model
        $scope.addtask = function () {
            $scope.search = "";
            var data = {
                "typeFlg": $scope.typeFlg,
                "ISMTPL_StartDate": new Date($scope.ismtpL_StartDate).toDateString(),
                "ISMTPL_EndDate": new Date($scope.ismtpL_EndDate).toDateString()
            };
            apiService.create("ISM_ComplaintsTaskList/getdetails", data).then(function (promise) {
                $scope.taskplanner = promise.get_comptasklist;
                if ($scope.taskplanner.length > 0) {
                    //$('#myModalAddtoPlanner').modal('show');
                }
                else {
                    swal("No Task Found..... !!");
                }
            });
        };
        $scope.effertcalclist = [];
        $scope.categorylist = [];
        $scope.currentweekefrt = 0;
        $scope.categoryidlist = [];
        // ======================================== End Date selection
        $scope.ondatechange = function () {
            $scope.categoryidlist = [];
            var mineffort = 0;
            var twdays = 0;
            $scope.currentweekefrt = 0;
            $scope.alltask = "";
            $scope.assignedtasklist = [];
            $scope.effertcalclist = [];
            $scope.categorylist = [];

            var data = {
                "ISMTPL_StartDate": new Date($scope.ismtpL_StartDate).toDateString(),
                "ISMTPL_EndDate": new Date($scope.ismtpL_EndDate).toDateString()
            };
            apiService.create("ISM_PlannerCreation/geteffortdetails", data).then(function (promise) {
                if (promise.get_taskproject != null && promise.get_taskproject.length>0) {
                    $scope.YearPlannerReject();
                    return;
                }
                $scope.weekStart = promise.weekStart;
                $scope.weekEnd = promise.weekEnd;
                $scope.weekStart = $filter('date')(new Date($scope.weekStart), "dd-MM-yyyy");
                $scope.weekEnd = $filter('date')(new Date($scope.weekEnd), "dd-MM-yyyy");
                $scope.get_effortdetails = promise.get_effortdetails;
                $scope.categorylist = promise.categorylist;
                if ($scope.get_effortdetails.length > 0) {
                    angular.forEach($scope.get_effortdetails, function (tf) {
                        //mineffort += parseInt(tf.WORKINGHOURS);
                        mineffort += parseFloat(tf.WORKINGHOURS);
                        twdays += parseInt(tf.NOOFDAYS);
                    });
                }
                // $scope.mineffort = mineffort;
                $scope.mineffort = Math.round(Number(mineffort));
                var decimaltimemin = $scope.mineffort;
                var hrsmin = parseInt(Number(decimaltimemin));
                var minmin = Math.round((Number(decimaltimemin) - hrsmin) * 60);
                if (hrsmin.toString().length === 1) {
                    hrsmin = '0' + hrsmin;
                }
                if (minmin.toString().length === 1) {
                    minmin = '0' + minmin;
                }
                $scope.effhrsmin = hrsmin;
                $scope.effminmin = minmin;

                $scope.get_Assignedtasklist = promise.get_Assignedtasklist;
                if (twdays > 0) {
                    if ($scope.get_Assignedtasklist.length > 0) {
                        angular.forEach($scope.get_Assignedtasklist, function (tt) {
                            var taskseffort = tt.ISMTCRASTO_EffortInHrs;
                            var hrseff = parseInt(Number(taskseffort));
                            var mineff = Math.round((Number(taskseffort) - hrseff) * 60);
                            if (hrseff.toString().length === 1) {
                                hrseff = '0' + hrseff;
                            }
                            if (mineff.toString().length === 1) {
                                mineff = '0' + mineff;
                            }
                            tt.effhrs = hrseff;
                            tt.effmin = mineff;

                            if (tt.ISMTCRASTO_StartDate === null || tt.ISMTCRASTO_StartDate === '') {
                                $scope.startDate = $filter('date')(new Daget_Assignedtasklistte(), "dd-MM-yyyy");
                            }
                            else {
                                $scope.startDate = $filter('date')(tt.ISMTCRASTO_StartDate, "dd-MM-yyyy");
                            }
                            if (tt.ISMTCRASTO_EndDate === null || tt.ISMTCRASTO_EndDate === '') {
                                $scope.endDate = $filter('date')(new Date($scope.today), "dd-MM-yyyy");
                            }
                            else {
                                $scope.endDate = $filter('date')(tt.ISMTCRASTO_EndDate, "dd-MM-yyyy");
                            }
                            if ((tt.ISMTPLTA_PreviousTask === 1 || tt.ISMTPLTA_PreviousTask === '1')) {
                                $scope.assignedtasklist.push({
                                    ismtcrastO_Id: tt.ISMTCRASTO_Id, ISMTCR_Id: tt.ISMTCR_Id, HRMP_Name: tt.HRMP_Name, ISMTCR_BugOREnhancementFlg: tt.ISMTCR_BugOREnhancementFlg, ISMTCR_CreationDate: tt.ISMTCR_CreationDate, ISMTCR_Title: tt.ISMTCR_Title, ISMTCR_Status: tt.ISMTCR_Status, ISMTCR_TaskNo: tt.ISMTCR_TaskNo, ISMMCLT_ClientName: tt.ISMMCLT_ClientName, ISMTCRASTO_AssignedDate: tt.ISMTCRASTO_AssignedDate,
                                    ISMTCRASTO_StartDate: $scope.startDate,
                                    ISMTCRASTO_EndDate: $scope.endDate,
                                    ISMTCRASTO_EffortInHrs: tt.ISMTCRASTO_EffortInHrs, assignedby: tt.assignedby, tstartdate: tt.ISMTCRASTO_StartDate, tenddate: tt.ISMTCRASTO_EndDate,
                                    Periodicity: tt.Periodicity, ISMTPLTA_Id: tt.ISMTPLTA_Id, ISMMTCAT_Id: tt.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: tt.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: tt.ISMMTCAT_CompulsoryFlg, ISMTPLTA_Status: tt.ISMTPLTA_Status,
                                    ISMTPLTA_PreviousTask: tt.ISMTPLTA_PreviousTask, ISMTPLTA_ApprovalFlg: tt.ISMTPLTA_ApprovalFlg, effhrs: tt.effhrs, effmin: tt.effmin, ISMTPLTA_StartDate: tt.ISMTPLTA_StartDate, ISMTPLTA_EndDate: tt.ISMTPLTA_EndDate, yrplan: tt.yrplan
                                });
                            }
                            else if ((tt.Periodicity == " " || tt.Periodicity == null || tt.Periodicity == undefined || tt.Periodicity == '' || angular.lowercase(tt.Periodicity) == 'once') && (tt.ISMTPLTA_PreviousTask === 0 || tt.ISMTPLTA_PreviousTask === null || tt.ISMTPLTA_PreviousTask === '' || tt.ISMTPLTA_PreviousTask === '0')) {
                                $scope.assignedtasklist.push({
                                    ismtcrastO_Id: tt.ISMTCRASTO_Id, ISMTCR_Id: tt.ISMTCR_Id, HRMP_Name: tt.HRMP_Name, ISMTCR_BugOREnhancementFlg: tt.ISMTCR_BugOREnhancementFlg, ISMTCR_CreationDate: tt.ISMTCR_CreationDate, ISMTCR_Title: tt.ISMTCR_Title, ISMTCR_Status: tt.ISMTCR_Status, ISMTCR_TaskNo: tt.ISMTCR_TaskNo, ISMMCLT_ClientName: tt.ISMMCLT_ClientName, ISMTCRASTO_AssignedDate: tt.ISMTCRASTO_AssignedDate,
                                    ISMTCRASTO_StartDate: $scope.startDate,
                                    ISMTCRASTO_EndDate: $scope.endDate,
                                    ISMTCRASTO_EffortInHrs: tt.ISMTCRASTO_EffortInHrs, assignedby: tt.assignedby, tstartdate: tt.ISMTCRASTO_StartDate, tenddate: tt.ISMTCRASTO_EndDate,
                                    Periodicity: tt.Periodicity, ISMTPLTA_Id: tt.ISMTPLTA_Id, ISMMTCAT_Id: tt.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: tt.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: tt.ISMMTCAT_CompulsoryFlg, ISMTPLTA_Status: tt.ISMTPLTA_Status,
                                    ISMTPLTA_PreviousTask: tt.ISMTPLTA_PreviousTask, ISMTPLTA_ApprovalFlg: tt.ISMTPLTA_ApprovalFlg, effhrs: tt.effhrs, effmin: tt.effmin, ISMTPLTA_StartDate: tt.ISMTPLTA_StartDate, ISMTPLTA_EndDate: tt.ISMTPLTA_EndDate, yrplan: tt.yrplan
                                });
                            }
                            else if (angular.lowercase(tt.Periodicity) == 'daily' && (tt.ISMTPLTA_PreviousTask === 0 || tt.ISMTPLTA_PreviousTask === null || tt.ISMTPLTA_PreviousTask === '' || tt.ISMTPLTA_PreviousTask === '0')) {
                                angular.forEach($scope.get_effortdetails, function (mm) {
                                    if (mm.PDates === null || mm.PDates === '') {
                                        $scope.startDate = $filter('date')(new Date($scope.today), "dd-MM-yyyy");
                                    }
                                    else {
                                        $scope.startDate = $filter('date')(mm.PDates, "dd-MM-yyyy");
                                    }
                                    if (mm.PDates === null || mm.PDates === '') {
                                        $scope.endDate = $filter('date')(new Date($scope.today), "dd-MM-yyyy");
                                    }
                                    else {
                                        $scope.endDate = $filter('date')(mm.PDates, "dd-MM-yyyy");
                                    }
                                    if (mm.PDates >= tt.ISMTCRASTO_StartDate && mm.PDates <= tt.ISMTCRASTO_EndDate) {
                                        $scope.assignedtasklist.push({
                                            ismtcrastO_Id: tt.ISMTCRASTO_Id, ISMTCR_Id: tt.ISMTCR_Id, HRMP_Name: tt.HRMP_Name, ISMTCR_BugOREnhancementFlg: tt.ISMTCR_BugOREnhancementFlg, ISMTCR_CreationDate: tt.ISMTCR_CreationDate, ISMTCR_Title: tt.ISMTCR_Title, ISMTCR_Status: tt.ISMTCR_Status, ISMTCR_TaskNo: tt.ISMTCR_TaskNo, ISMMCLT_ClientName: tt.ISMMCLT_ClientName, ISMTCRASTO_AssignedDate: tt.ISMTCRASTO_AssignedDate,
                                            ISMTCRASTO_StartDate: $scope.startDate,
                                            ISMTCRASTO_EndDate: $scope.endDate,
                                            ISMTCRASTO_EffortInHrs: tt.ISMTCRASTO_EffortInHrs, assignedby: tt.assignedby, tstartdate: mm.PDates, tenddate: mm.PDates,
                                            Periodicity: tt.Periodicity, ISMTPLTA_Id: tt.ISMTPLTA_Id, ISMMTCAT_Id: tt.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: tt.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: tt.ISMMTCAT_CompulsoryFlg, ISMTPLTA_Status: tt.ISMTPLTA_Status,
                                            ISMTPLTA_PreviousTask: tt.ISMTPLTA_PreviousTask, ISMTPLTA_ApprovalFlg: tt.ISMTPLTA_ApprovalFlg, effhrs: tt.effhrs, effmin: tt.effmin, ISMTPLTA_StartDate: tt.ISMTPLTA_StartDate, ISMTPLTA_EndDate: tt.ISMTPLTA_EndDate, yrplan: tt.yrplan
                                        });
                                    }
                                });
                            }
                            else if (angular.lowercase(tt.Periodicity) == 'weekly' && (tt.ISMTPLTA_PreviousTask === 0 || tt.ISMTPLTA_PreviousTask === null || tt.ISMTPLTA_PreviousTask === '' || tt.ISMTPLTA_PreviousTask === '0')) {
                                var wrkd = $scope.get_effortdetails.length;
                                if (parseInt(wrkd) >= parseInt(tt.ISMTAPL_Day)) {
                                    var cnr = 0;
                                    angular.forEach($scope.get_effortdetails, function (mm) {
                                        cnr += 1;
                                        if (cnr == parseInt(tt.ISMTAPL_Day)) {
                                            if (mm.PDates === null || mm.PDates === '') {
                                                $scope.startDate = $filter('date')(new Date($scope.today), "dd-MM-yyyy");
                                            }
                                            else {
                                                $scope.startDate = $filter('date')(mm.PDates, "dd-MM-yyyy");
                                            }
                                            if (mm.PDates === null || mm.PDates === '') {
                                                $scope.endDate = $filter('date')(new Date($scope.today), "dd-MM-yyyy");
                                            }
                                            else {
                                                $scope.endDate = $filter('date')(mm.PDates, "dd-MM-yyyy");
                                            }

                                            if (mm.PDates >= tt.ISMTCRASTO_StartDate && mm.PDates <= tt.ISMTCRASTO_EndDate) {
                                                $scope.assignedtasklist.push({
                                                    ismtcrastO_Id: tt.ISMTCRASTO_Id, ISMTCR_Id: tt.ISMTCR_Id, HRMP_Name: tt.HRMP_Name, ISMTCR_BugOREnhancementFlg: tt.ISMTCR_BugOREnhancementFlg, ISMTCR_CreationDate: tt.ISMTCR_CreationDate, ISMTCR_Title: tt.ISMTCR_Title, ISMTCR_Status: tt.ISMTCR_Status, ISMTCR_TaskNo: tt.ISMTCR_TaskNo, ISMMCLT_ClientName: tt.ISMMCLT_ClientName, ISMTCRASTO_AssignedDate: tt.ISMTCRASTO_AssignedDate,
                                                    ISMTCRASTO_StartDate: $scope.startDate,
                                                    ISMTCRASTO_EndDate: $scope.endDate,
                                                    ISMTCRASTO_EffortInHrs: tt.ISMTCRASTO_EffortInHrs, assignedby: tt.assignedby, tstartdate: mm.PDates, tenddate: mm.PDates,
                                                    Periodicity: tt.Periodicity, ISMTPLTA_Id: tt.ISMTPLTA_Id, ISMMTCAT_Id: tt.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: tt.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: tt.ISMMTCAT_CompulsoryFlg, ISMTPLTA_Status: tt.ISMTPLTA_Status,
                                                    ISMTPLTA_PreviousTask: tt.ISMTPLTA_PreviousTask, ISMTPLTA_ApprovalFlg: tt.ISMTPLTA_ApprovalFlg, effhrs: tt.effhrs, effmin: tt.effmin, ISMTPLTA_StartDate: tt.ISMTPLTA_StartDate, ISMTPLTA_EndDate: tt.ISMTPLTA_EndDate, yrplan: tt.yrplan
                                                });
                                            }
                                        }
                                    });
                                }
                                else if (parseInt(wrkd) < parseInt(tt.ISMTAPL_Day)) {
                                    var cnr = 0;
                                    angular.forEach($scope.get_effortdetails, function (mm) {
                                        cnr += 1;
                                        if (cnr == parseInt(wrkd)) {
                                            if (mm.PDates === null || mm.PDates === '') {
                                                $scope.startDate = $filter('date')(new Date($scope.today), "dd-MM-yyyy");
                                            }
                                            else {
                                                $scope.startDate = $filter('date')(mm.PDates, "dd-MM-yyyy");
                                            }
                                            if (mm.PDates === null || mm.PDates === '') {
                                                $scope.endDate = $filter('date')(new Date($scope.today), "dd-MM-yyyy");
                                            }
                                            else {
                                                $scope.endDate = $filter('date')(mm.PDates, "dd-MM-yyyy");
                                            }
                                            if (mm.PDates >= tt.ISMTCRASTO_StartDate && mm.PDates <= tt.ISMTCRASTO_EndDate) {
                                                $scope.assignedtasklist.push({
                                                    ismtcrastO_Id: tt.ISMTCRASTO_Id, ISMTCR_Id: tt.ISMTCR_Id, HRMP_Name: tt.HRMP_Name, ISMTCR_BugOREnhancementFlg: tt.ISMTCR_BugOREnhancementFlg, ISMTCR_CreationDate: tt.ISMTCR_CreationDate, ISMTCR_Title: tt.ISMTCR_Title, ISMTCR_Status: tt.ISMTCR_Status, ISMTCR_TaskNo: tt.ISMTCR_TaskNo, ISMMCLT_ClientName: tt.ISMMCLT_ClientName, ISMTCRASTO_AssignedDate: tt.ISMTCRASTO_AssignedDate,
                                                    ISMTCRASTO_StartDate: $scope.startDate,
                                                    ISMTCRASTO_EndDate: $scope.endDate,
                                                    ISMTCRASTO_EffortInHrs: tt.ISMTCRASTO_EffortInHrs, assignedby: tt.assignedby, tstartdate: mm.PDates, tenddate: mm.PDates,
                                                    Periodicity: tt.Periodicity, ISMTPLTA_Id: tt.ISMTPLTA_Id, ISMMTCAT_Id: tt.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: tt.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: tt.ISMMTCAT_CompulsoryFlg, ISMTPLTA_Status: tt.ISMTPLTA_Status,
                                                    ISMTPLTA_PreviousTask: tt.ISMTPLTA_PreviousTask, ISMTPLTA_ApprovalFlg: tt.ISMTPLTA_ApprovalFlg, effhrs: tt.effhrs, effmin: tt.effmin, ISMTPLTA_StartDate: tt.ISMTPLTA_StartDate, ISMTPLTA_EndDate: tt.ISMTPLTA_EndDate, yrplan: tt.yrplan
                                                });
                                            }
                                        }
                                    })
                                }
                            }
                            else if (angular.lowercase(tt.Periodicity) == 'once in fortnight' && (tt.ISMTPLTA_PreviousTask === 0 || tt.ISMTPLTA_PreviousTask === null || tt.ISMTPLTA_PreviousTask === '' || tt.ISMTPLTA_PreviousTask === '0')) {
                                var ddcnt = parseInt(tt.ISMTAPL_Day) - 1;
                                var ddcnt = parseInt(tt.ISMTAPL_Day) - 1;
                                var wrkd = $scope.get_effortdetails.length;
                                var date = new Date($scope.today);
                                var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
                                var firstDay1 = new Date(date.getFullYear(), date.getMonth(), 1);
                                $scope.newdate = firstDay.setDate(firstDay.getDate() + ddcnt);
                                $scope.anewdate = firstDay1.setDate(firstDay1.getDate() + ddcnt + 15);
                                $scope.newdate1 = $filter('date')($scope.newdate, "dd-MM-yyyy");
                                $scope.anewdate1 = $filter('date')($scope.anewdate, "dd-MM-yyyy");
                                var cnr = 0;
                                angular.forEach($scope.get_effortdetails, function (mm) {
                                    if (mm.PDates === null || mm.PDates === '') {
                                        $scope.startDate = $filter('date')(new Date($scope.today), "dd-MM-yyyy");
                                    }
                                    else {
                                        $scope.startDate = $filter('date')(mm.PDates, "dd-MM-yyyy");
                                    }
                                    if (mm.PDates === null || mm.PDates === '') {
                                        $scope.endDate = $filter('date')(new Date($scope.today), "dd-MM-yyyy");
                                    }
                                    else {
                                        $scope.endDate = $filter('date')(mm.PDates, "dd-MM-yyyy");
                                    }
                                    if ($scope.newdate1 == $scope.endDate) {
                                        $scope.assignedtasklist.push({
                                            ismtcrastO_Id: tt.ISMTCRASTO_Id, ISMTCR_Id: tt.ISMTCR_Id, HRMP_Name: tt.HRMP_Name, ISMTCR_BugOREnhancementFlg: tt.ISMTCR_BugOREnhancementFlg, ISMTCR_CreationDate: tt.ISMTCR_CreationDate, ISMTCR_Title: tt.ISMTCR_Title, ISMTCR_Status: tt.ISMTCR_Status, ISMTCR_TaskNo: tt.ISMTCR_TaskNo, ISMMCLT_ClientName: tt.ISMMCLT_ClientName, ISMTCRASTO_AssignedDate: tt.ISMTCRASTO_AssignedDate,
                                            ISMTCRASTO_StartDate: $scope.startDate,
                                            ISMTCRASTO_EndDate: $scope.endDate,
                                            ISMTCRASTO_EffortInHrs: tt.ISMTCRASTO_EffortInHrs, assignedby: tt.assignedby, tstartdate: mm.PDates, tenddate: mm.PDates,
                                            Periodicity: tt.Periodicity, ISMTPLTA_Id: tt.ISMTPLTA_Id, ISMMTCAT_Id: tt.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: tt.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: tt.ISMMTCAT_CompulsoryFlg, ISMTPLTA_Status: tt.ISMTPLTA_Status,
                                            ISMTPLTA_PreviousTask: tt.ISMTPLTA_PreviousTask, ISMTPLTA_ApprovalFlg: tt.ISMTPLTA_ApprovalFlg, effhrs: tt.effhrs, effmin: tt.effmin, ISMTPLTA_StartDate: tt.ISMTPLTA_StartDate, ISMTPLTA_EndDate: tt.ISMTPLTA_EndDate, yrplan: tt.yrplan
                                        });
                                    }
                                    if ($scope.anewdate1 == $scope.endDate) {
                                        $scope.assignedtasklist.push({
                                            ismtcrastO_Id: tt.ISMTCRASTO_Id, ISMTCR_Id: tt.ISMTCR_Id, HRMP_Name: tt.HRMP_Name, ISMTCR_BugOREnhancementFlg: tt.ISMTCR_BugOREnhancementFlg, ISMTCR_CreationDate: tt.ISMTCR_CreationDate, ISMTCR_Title: tt.ISMTCR_Title, ISMTCR_Status: tt.ISMTCR_Status, ISMTCR_TaskNo: tt.ISMTCR_TaskNo, ISMMCLT_ClientName: tt.ISMMCLT_ClientName, ISMTCRASTO_AssignedDate: tt.ISMTCRASTO_AssignedDate,
                                            ISMTCRASTO_StartDate: $scope.startDate,
                                            ISMTCRASTO_EndDate: $scope.endDate,
                                            ISMTCRASTO_EffortInHrs: tt.ISMTCRASTO_EffortInHrs, assignedby: tt.assignedby, tstartdate: mm.PDates, tenddate: mm.PDates,
                                            Periodicity: tt.Periodicity, ISMTPLTA_Id: tt.ISMTPLTA_Id, ISMMTCAT_Id: tt.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: tt.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: tt.ISMMTCAT_CompulsoryFlg, ISMTPLTA_Status: tt.ISMTPLTA_Status,
                                            ISMTPLTA_PreviousTask: tt.ISMTPLTA_PreviousTask, ISMTPLTA_ApprovalFlg: tt.ISMTPLTA_ApprovalFlg, effhrs: tt.effhrs, effmin: tt.effmin, yrplan: tt.yrplan
                                        });
                                    }
                                });
                            }
                            else if (angular.lowercase(tt.Periodicity) == 'monthly' && (tt.ISMTPLTA_PreviousTask === 0 || tt.ISMTPLTA_PreviousTask === null || tt.ISMTPLTA_PreviousTask === '' || tt.ISMTPLTA_PreviousTask === '0')) {
                                $scope.ttdate1 = $filter('date')(tt.addeddate1, "dd-MM-yyyy");
                                $scope.ttdate2 = $filter('date')(tt.addeddate2, "dd-MM-yyyy");
                                if (tt.Weekfirsttrue) {
                                    if (tt.firsttrue) {
                                        $scope.assignedtasklist.push({
                                            ismtcrastO_Id: tt.ISMTCRASTO_Id, ISMTCR_Id: tt.ISMTCR_Id, HRMP_Name: tt.HRMP_Name, ISMTCR_BugOREnhancementFlg: tt.ISMTCR_BugOREnhancementFlg, ISMTCR_CreationDate: tt.ISMTCR_CreationDate, ISMTCR_Title: tt.ISMTCR_Title, ISMTCR_Status: tt.ISMTCR_Status, ISMTCR_TaskNo: tt.ISMTCR_TaskNo, ISMMCLT_ClientName: tt.ISMMCLT_ClientName, ISMTCRASTO_AssignedDate: tt.ISMTCRASTO_AssignedDate,
                                            ISMTCRASTO_StartDate: $scope.ttdate1,
                                            ISMTCRASTO_EndDate: $scope.ttdate1,
                                            ISMTCRASTO_EffortInHrs: tt.ISMTCRASTO_EffortInHrs, assignedby: tt.assignedby, tstartdate: tt.addeddate1, tenddate: tt.addeddate1,
                                            Periodicity: tt.Periodicity, ISMTPLTA_Id: tt.ISMTPLTA_Id, ISMMTCAT_Id: tt.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: tt.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: tt.ISMMTCAT_CompulsoryFlg, ISMTPLTA_Status: tt.ISMTPLTA_Status,
                                            ISMTPLTA_PreviousTask: tt.ISMTPLTA_PreviousTask, ISMTPLTA_ApprovalFlg: tt.ISMTPLTA_ApprovalFlg, effhrs: tt.effhrs, effmin: tt.effmin, ISMTPLTA_StartDate: tt.ISMTPLTA_StartDate, ISMTPLTA_EndDate: tt.ISMTPLTA_EndDate, yrplan: tt.yrplan
                                        });
                                    };
                                }
                                else if (tt.Weeksecondtrue) {
                                    if (tt.secondtrue) {
                                        $scope.assignedtasklist.push({
                                            ismtcrastO_Id: tt.ISMTCRASTO_Id, ISMTCR_Id: tt.ISMTCR_Id, HRMP_Name: tt.HRMP_Name, ISMTCR_BugOREnhancementFlg: tt.ISMTCR_BugOREnhancementFlg, ISMTCR_CreationDate: tt.ISMTCR_CreationDate, ISMTCR_Title: tt.ISMTCR_Title, ISMTCR_Status: tt.ISMTCR_Status, ISMTCR_TaskNo: tt.ISMTCR_TaskNo, ISMMCLT_ClientName: tt.ISMMCLT_ClientName, ISMTCRASTO_AssignedDate: tt.ISMTCRASTO_AssignedDate,
                                            ISMTCRASTO_StartDate: $scope.ttdate2,
                                            ISMTCRASTO_EndDate: $scope.ttdate2,
                                            ISMTCRASTO_EffortInHrs: tt.ISMTCRASTO_EffortInHrs, assignedby: tt.assignedby, tstartdate: tt.addeddate2, tenddate: tt.addeddate2,
                                            Periodicity: tt.Periodicity, ISMTPLTA_Id: tt.ISMTPLTA_Id, ISMMTCAT_Id: tt.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: tt.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: tt.ISMMTCAT_CompulsoryFlg, ISMTPLTA_Status: tt.ISMTPLTA_Status,
                                            ISMTPLTA_PreviousTask: tt.ISMTPLTA_PreviousTask, ISMTPLTA_ApprovalFlg: tt.ISMTPLTA_ApprovalFlg, effhrs: tt.effhrs, effmin: tt.effmin, ISMTPLTA_StartDate: tt.ISMTPLTA_StartDate, ISMTPLTA_EndDate: tt.ISMTPLTA_EndDate, yrplan: tt.yrplan
                                        });
                                    };
                                }
                                //var wrkd = $scope.get_effortdetails.length;
                                // var date = new Date($scope.today);
                                //var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
                                //$scope.newdate = firstDay.setDate(firstDay.getDate() + ddcnt);
                                //$scope.newdate1 = $filter('date')($scope.newdate, "dd-MM-yyyy");
                                //var cnr = 0;
                                //angular.forEach($scope.get_effortdetails, function (mm) {
                                //    if (mm.PDates === null || mm.PDates === '') {
                                //        $scope.startDate = $filter('date')(new Date($scope.today), "dd-MM-yyyy");
                                //    }
                                //    else {
                                //        $scope.startDate = $filter('date')(mm.PDates, "dd-MM-yyyy");
                                //    }
                                //    if (mm.PDates === null || mm.PDates === '') {
                                //        $scope.endDate = $filter('date')(new Date($scope.today), "dd-MM-yyyy");
                                //    }
                                //    else {
                                //        $scope.endDate = $filter('date')(mm.PDates, "dd-MM-yyyy");
                                //    }
                                //    if (mm.PDates >= tt.ISMTCRASTO_StartDate && mm.PDates <= tt.ISMTCRASTO_EndDate) {
                                //        if ($scope.getnewdate1 == $scope.endDate || $scope.getnewdate2 == $scope.endDate) {
                                //            $scope.assignedtasklist.push({
                                //                ismtcrastO_Id: tt.ISMTCRASTO_Id, ISMTCR_Id: tt.ISMTCR_Id, HRMP_Name: tt.HRMP_Name, ISMTCR_BugOREnhancementFlg: tt.ISMTCR_BugOREnhancementFlg, ISMTCR_CreationDate: tt.ISMTCR_CreationDate, ISMTCR_Title: tt.ISMTCR_Title, ISMTCR_Status: tt.ISMTCR_Status, ISMTCR_TaskNo: tt.ISMTCR_TaskNo, ISMMCLT_ClientName: tt.ISMMCLT_ClientName, ISMTCRASTO_AssignedDate: tt.ISMTCRASTO_AssignedDate,
                                //                ISMTCRASTO_StartDate: $scope.startDate,
                                //                ISMTCRASTO_EndDate: $scope.endDate,

                                //                ISMTCRASTO_EffortInHrs: tt.ISMTCRASTO_EffortInHrs, assignedby: tt.assignedby, tstartdate: mm.PDates, tenddate: mm.PDates,
                                //                Periodicity: tt.Periodicity, ISMTPLTA_Id: tt.ISMTPLTA_Id, ISMMTCAT_Id: tt.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: tt.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: tt.ISMMTCAT_CompulsoryFlg, ISMTPLTA_Status: tt.ISMTPLTA_Status,
                                //                ISMTPLTA_PreviousTask: tt.ISMTPLTA_PreviousTask, ISMTPLTA_ApprovalFlg: tt.ISMTPLTA_ApprovalFlg, effhrs: tt.effhrs, effmin: tt.effmin, ISMTPLTA_StartDate: tt.ISMTPLTA_StartDate, ISMTPLTA_EndDate: tt.ISMTPLTA_EndDate
                                //            });
                                //        }
                                //    };
                                //});
                            }
                            else if ((angular.lowercase(tt.Periodicity) == 'yearly once' || angular.lowercase(tt.Periodicity) == 'specific day') && (tt.ISMTPLTA_PreviousTask === 0 || tt.ISMTPLTA_PreviousTask === null || tt.ISMTPLTA_PreviousTask === '' || tt.ISMTPLTA_PreviousTask === '0')) {
                                $scope.newdate1 = $filter('date')(tt.OFFDate, "dd-MM-yyyy");
                                var cnr = 0;
                                angular.forEach($scope.get_effortdetails, function (mm) {
                                    if (mm.PDates === null || mm.PDates === '') {
                                        $scope.startDate = $filter('date')(new Date($scope.today), "dd-MM-yyyy");
                                    }
                                    else {
                                        $scope.startDate = $filter('date')(mm.PDates, "dd-MM-yyyy");
                                    }
                                    if (mm.PDates === null || mm.PDates === '') {
                                        $scope.endDate = $filter('date')(new Date($scope.today), "dd-MM-yyyy");
                                    }
                                    else {
                                        $scope.endDate = $filter('date')(mm.PDates, "dd-MM-yyyy");
                                    }
                                    if ($scope.newdate1 == $scope.endDate) {
                                        $scope.assignedtasklist.push({
                                            ismtcrastO_Id: tt.ISMTCRASTO_Id, ISMTCR_Id: tt.ISMTCR_Id, HRMP_Name: tt.HRMP_Name, ISMTCR_BugOREnhancementFlg: tt.ISMTCR_BugOREnhancementFlg, ISMTCR_CreationDate: tt.ISMTCR_CreationDate, ISMTCR_Title: tt.ISMTCR_Title, ISMTCR_Status: tt.ISMTCR_Status, ISMTCR_TaskNo: tt.ISMTCR_TaskNo, ISMMCLT_ClientName: tt.ISMMCLT_ClientName, ISMTCRASTO_AssignedDate: tt.ISMTCRASTO_AssignedDate,
                                            ISMTCRASTO_StartDate: $scope.startDate, ISMTCRASTO_EndDate: $scope.endDate,
                                            ISMTCRASTO_EffortInHrs: tt.ISMTCRASTO_EffortInHrs, assignedby: tt.assignedby, tstartdate: mm.PDates, tenddate: mm.PDates,
                                            Periodicity: tt.Periodicity, ISMTPLTA_Id: tt.ISMTPLTA_Id, ISMMTCAT_Id: tt.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: tt.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: tt.ISMMTCAT_CompulsoryFlg, ISMTPLTA_Status: tt.ISMTPLTA_Status,
                                            ISMTPLTA_PreviousTask: tt.ISMTPLTA_PreviousTask, ISMTPLTA_ApprovalFlg: tt.ISMTPLTA_ApprovalFlg, effhrs: tt.effhrs, effmin: tt.effmin, ISMTPLTA_StartDate: tt.ISMTPLTA_StartDate, ISMTPLTA_EndDate: tt.ISMTPLTA_EndDate, yrplan: tt.yrplan
                                        });
                                    }
                                })
                            }
                        });
                        if ($scope.assignedtasklist.length > 0) {
                            $scope.effertcalclist = [];
                            $scope.currentweekefrt = 0;
                            angular.forEach($scope.assignedtasklist, function (ss) {
                                ss.checkedvalue = true;
                                if (ss.ISMTPLTA_Id == 0 || ss.ISMTPLTA_Id == '' || ss.ISMTPLTA_Id == undefined || ss.ISMTPLTA_Id == null) {
                                    if (new Date(ss.tenddate) > $scope.ismtpL_EndDate) {
                                        ss.dis = false;
                                    }
                                    else {
                                        ss.dis = true;
                                    }
                                    ss.curr = true;
                                    $scope.effertcalclist.push(ss);
                                    $scope.currentweekefrt = $scope.currentweekefrt + parseFloat(ss.ISMTCRASTO_EffortInHrs);
                                }
                                else {
                                    if (ss.ISMTPLTA_PreviousTask == null || ss.ISMTPLTA_PreviousTask == 0 || ss.ISMTPLTA_PreviousTask == "0") {
                                        if (ss.ISMTPLTA_ApprovalFlg == "1") {
                                            ss.cls = "backkk";
                                            ss.dis = true;
                                            ss.curr = false;
                                        }
                                        else {
                                            ss.curr = true;
                                            $scope.effertcalclist.push(ss);
                                            $scope.currentweekefrt = $scope.currentweekefrt + parseFloat(ss.ISMTCRASTO_EffortInHrs);

                                            if (new Date(ss.tenddate) > $scope.ismtpL_EndDate) {
                                                ss.dis = false;
                                            }
                                            else {
                                                ss.dis = true;
                                            }
                                        }
                                    }
                                    else {
                                        ss.cls = "backkk";
                                        ss.dis = true;
                                        ss.curr = false;
                                    }
                                }
                            });
                            var decimaltime = $scope.currentweekefrt;
                            var hrscu = parseInt(Number(decimaltime));
                            var mincu = Math.round((Number(decimaltime) - hrscu) * 60);
                            if (hrscu.toString().length === 1) {
                                hrscu = '0' + hrscu;
                            }
                            if (mincu.toString().length === 1) {
                                mincu = '0' + mincu;
                            }
                            $scope.effhrscu = hrscu;
                            $scope.effmincu = mincu;
                        }
                        if ($scope.categorylist.length > 0) {
                            var pp = 0;
                            var msg = '';
                            $scope.remcategory = [];
                            angular.forEach($scope.categorylist, function (tt) {
                                var ddd = 0;
                                angular.forEach($scope.effertcalclist, function (dd) {
                                    if (parseInt(tt.ismmtcaT_Id) == parseInt(dd.ISMMTCAT_Id)) {
                                        ddd += parseFloat(dd.ISMTCRASTO_EffortInHrs);
                                    }
                                })
                                if (ddd != 0) {
                                    pp = (parseFloat(tt.ismmtcaT_TaskPercentage) / 100) * parseFloat($scope.mineffort)
                                }
                                else {
                                    pp = (parseFloat(tt.ismmtcaT_TaskPercentage) / 100) * parseFloat($scope.mineffort);
                                    $scope.remcategory.push({ CATNAME: tt.ismmtcaT_TaskCategoryName, RP: tt.ismmtcaT_TaskPercentage, CWP: 0, CRE: pp, CCE: 0 })
                                }
                                //pp = pp.toFixed(2);
                                //ddd = ddd.toFixed(2);
                                if (parseFloat(pp.toFixed(2)) > parseFloat(ddd.toFixed(2)) && ddd != 0) {
                                    $scope.remcategory.push({ CATNAME: tt.ismmtcaT_TaskCategoryName, RP: tt.ismmtcaT_TaskPercentage, CRE: pp, CCE: ddd })
                                }
                            })
                        }
                        angular.forEach($scope.remcategory, function (dd) {
                            var cateff = dd.CRE;
                            var hrscat = parseInt(Number(cateff));
                            var mincat = Math.round((Number(cateff) - hrscat) * 60);
                            if (hrscat.toString().length === 1) {
                                hrscat = '0' + hrscat;
                            }
                            if (mincat.toString().length === 1) {
                                mincat = '0' + mincat;
                            }
                            dd.effhrcat = hrscat;
                            dd.effmicat = mincat;
                            var cateffCCE = dd.CCE;
                            var hrscatCCE = parseInt(Number(cateffCCE));
                            var mincatCCE = Math.round((Number(cateffCCE) - hrscatCCE) * 60);
                            if (hrscatCCE.toString().length === 1) {
                                hrscatCCE = '0' + hrscatCCE;
                            }
                            if (mincatCCE.toString().length === 1) {
                                mincatCCE = '0' + mincatCCE;
                            }
                            dd.effhrcatCCE = hrscatCCE;
                            dd.effmicatCCE = mincatCCE;
                            var catefffin = dd.CRE - dd.CCE;
                            var hrscatfin = parseInt(Number(catefffin));
                            var mincatfin = Math.round((Number(catefffin) - hrscatfin) * 60);
                            if (hrscatfin.toString().length === 1) {
                                hrscatfin = '0' + hrscatfin;
                            }
                            if (mincatfin.toString().length === 1) {
                                mincatfin = '0' + mincatfin;
                            }
                            dd.effhrcatfin = hrscatfin;
                            dd.effmicatfin = mincatfin;
                        });
                        $scope.onassignCheck();
                        //$scope.presentCountgrid = $scope.assignedtasklist.length;
                        $scope.categoryidlist = [];
                        var snocount = 0;
                        var countsubgrp = 0;
                        angular.forEach($scope.assignedtasklist, function (sub) {
                            if ($scope.categoryidlist.length === 0) {
                                snocount += 1;
                                $scope.categoryidlist.push({ ISMMTCAT_Id: sub.ISMMTCAT_Id });
                            } else if ($scope.categoryidlist.length > 0) {
                                countsubgrp = 0;
                                angular.forEach($scope.categoryidlist, function (subgrp) {
                                    if (subgrp.ISMMTCAT_Id === sub.ISMMTCAT_Id) {
                                        countsubgrp += 1;
                                    }
                                });
                                if (countsubgrp === 0) {
                                    snocount += 1;
                                    $scope.categoryidlist.push({ ISMMTCAT_Id: sub.ISMMTCAT_Id });
                                }
                            }
                        });
                    }
                    else {
                        swal("No Record Found....!!");
                    }
                }
                else {
                    swal("Working days is not Available");
                }
                $scope.workingdays = twdays;
            });
        };

        //========================================= Checkbox Selections
        $scope.all_task = function (atk) {
            var totcheck = 0;
            var toteffort = 0;
            $scope.alltask = atk;
            var toggleStatus = $scope.alltask;
            angular.forEach($scope.assignedtasklist, function (atck) {
                atck.checkedvalue = toggleStatus;
                if (atck.checkedvalue === true) {
                    totcheck = totcheck + 1;
                    toteffort += parseFloat(atck.ISMTCRASTO_EffortInHrs);
                }
            });
            $scope.totalcheck = totcheck;
            $scope.totaleffort = toteffort;
            var decimaltimetot = $scope.totaleffort;
            var hrstot = parseInt(Number(decimaltimetot));
            var mintot = Math.round((Number(decimaltimetot) - hrstot) * 60);
            if (hrstot.toString().length === 1) {
                hrstot = '0' + hrstot;
            }
            if (mintot.toString().length === 1) {
                mintot = '0' + mintot;
            }
            $scope.effhrstot = hrstot;
            $scope.effmintot = mintot;
        };

        $scope.onassignCheck = function () {
            var totcheck = 0;
            var toteffort = 0;
            $scope.effertcalclist = [];
            $scope.currentweekefrt = 0;
            angular.forEach($scope.assignedtasklist, function (ack) {
                if (ack.checkedvalue === true) {
                    totcheck = totcheck + 1;
                    toteffort += parseFloat(ack.ISMTCRASTO_EffortInHrs);
                    if (ack.curr == true) {
                        $scope.effertcalclist.push(ack);
                        $scope.currentweekefrt += parseFloat(ack.ISMTCRASTO_EffortInHrs);
                    }
                }
            });

            var decimaltime = $scope.currentweekefrt;
            var hrscu = parseInt(Number(decimaltime));
            var mincu = Math.round((Number(decimaltime) - hrscu) * 60);
            if (hrscu.toString().length === 1) {
                hrscu = '0' + hrscu;
            }
            if (mincu.toString().length === 1) {
                mincu = '0' + mincu;
            }
            $scope.effhrscu = hrscu;
            $scope.effmincu = mincu;

            $scope.totalcheck = totcheck;
            $scope.totaleffort = toteffort;
            var decimaltimetot = $scope.totaleffort;
            var hrstot = parseInt(Number(decimaltimetot));
            var mintot = Math.round((Number(decimaltimetot) - hrstot) * 60);
            if (hrstot.toString().length === 1) {
                hrstot = '0' + hrstot;
            }
            if (mintot.toString().length === 1) {
                mintot = '0' + mintot;
            }
            $scope.effhrstot = hrstot;
            $scope.effmintot = mintot;
            $scope.alltask = $scope.assignedtasklist.every(function (options) {
                return options.checkedvalue;
            });
            if ($scope.categorylist.length > 0) {
                var pp = 0;
                var msg = '';
                $scope.remcategory = [];
                angular.forEach($scope.categorylist, function (tt) {
                    var ddd = 0;
                    angular.forEach($scope.effertcalclist, function (dd) {
                        if (parseInt(tt.ismmtcaT_Id) == parseInt(dd.ISMMTCAT_Id)) {
                            ddd += parseFloat(dd.ISMTCRASTO_EffortInHrs);
                        }
                    });
                    if (ddd !== 0) {
                        pp = (parseFloat(tt.ismmtcaT_TaskPercentage) / 100) * parseFloat($scope.mineffort);
                    }
                    else {
                        pp = (parseFloat(tt.ismmtcaT_TaskPercentage) / 100) * parseFloat($scope.mineffort);
                        $scope.remcategory.push({ CATNAME: tt.ismmtcaT_TaskCategoryName, RP: tt.ismmtcaT_TaskPercentage, CWP: 0, CRE: pp, CCE: 0 });
                    }
                    //pp = pp.toFixed(2);
                    //ddd = ddd.toFixed(2);
                    if (parseFloat(pp.toFixed(2)) > parseFloat(ddd.toFixed(2)) && ddd !== 0) {
                        $scope.remcategory.push({ CATNAME: tt.ismmtcaT_TaskCategoryName, RP: tt.ismmtcaT_TaskPercentage, CRE: pp, CCE: ddd });
                    }
                });
                angular.forEach($scope.remcategory, function (dd) {
                    var cateff = dd.CRE;
                    var hrscat = parseInt(Number(cateff));
                    var mincat = Math.round((Number(cateff) - hrscat) * 60);
                    if (hrscat.toString().length === 1) {
                        hrscat = '0' + hrscat;
                    }
                    if (mincat.toString().length === 1) {
                        mincat = '0' + mincat;
                    }
                    dd.effhrcat = hrscat;
                    dd.effmicat = mincat;

                    var cateffCCE = dd.CCE;
                    var hrscatCCE = parseInt(Number(cateffCCE));
                    var mincatCCE = Math.round((Number(cateffCCE) - hrscatCCE) * 60);
                    if (hrscatCCE.toString().length === 1) {
                        hrscatCCE = '0' + hrscatCCE;
                    }
                    if (mincatCCE.toString().length === 1) {
                        mincatCCE = '0' + mincatCCE;
                    }
                    dd.effhrcatCCE = hrscatCCE;
                    dd.effmicatCCE = mincatCCE;

                    var catefffin = dd.CRE - dd.CCE;
                    var hrscatfin = parseInt(Number(catefffin));
                    var mincatfin = Math.round((Number(catefffin) - hrscatfin) * 60);
                    if (hrscatfin.toString().length === 1) {
                        hrscatfin = '0' + hrscatfin;
                    }
                    if (mincatfin.toString().length === 1) {
                        mincatfin = '0' + mincatfin;
                    }
                    dd.effhrcatfin = hrscatfin;
                    dd.effmicatfin = mincatfin;
                });
            }
        };

        $scope.counteffort = function () {
            var toteffort = 0;
            var entereffort = 0;
            angular.forEach($scope.assignedtasklist, function (ack) {
                if (ack.checkedvalue === true) {
                    entereffort = parseFloat(ack.ISMTCRASTO_EffortInHrs);
                    toteffort += parseFloat(ack.ISMTCRASTO_EffortInHrs);
                    //if (entereffort > 12) {
                    //    swal("Effort must be less than or equal to 12 Hours ... !!");
                    //    ack.ISMTCRASTO_EffortInHrs = 0;
                    //}
                }
            });
            $scope.totaleffort = toteffort;
            var decimaltimetot = $scope.totaleffort;
            var hrstot = parseInt(Number(decimaltimetot));
            var mintot = Math.round((Number(decimaltimetot) - hrstot) * 60);
            if (hrstot.toString().length === 1) {
                hrstot = '0' + hrstot;
            }
            if (mintot.toString().length === 1) {
                mintot = '0' + mintot;
            }
            $scope.effhrstot = hrstot;
            $scope.effmintot = mintot;
        };

        //========================================= Add Task
        $scope.saveAddTask = function () {
            if ($scope.myForm1.$valid) {
                $scope.addTaskArray = [];
                angular.forEach($scope.taskplanner, function (tp) {
                    if (tp.checkedTask === true) {
                        $scope.addTaskArray.push(tp);
                    }
                });
                if ($scope.addTaskArray.length > 0) {
                    var data = {
                        "ISMTCRASTO_Id": $scope.ismtcrastO_Id,
                        "ISMTCRASTO_AssignedDate": new Date($scope.ismtcrastO_AssignedDate).toDateString(),
                        "addTaskArray": $scope.addTaskArray
                    };
                }
                else {
                    swal("Please Select Any Task ..... !!");
                }
                apiService.create("ISM_PlannerCreation/saveAddTask", data).then(function (promise) {
                    if (promise.returnval === true) {
                        if (promise.ismtcrastO_Id === 0 || promise.ismtcrastO_Id < 0) {
                            $('#myModalAddtoPlanner').modal('hide');
                            swal('Task added to planner successfully....!!');
                            $state.reload();
                        }
                        else if (promise.ismtcrastO_Id > 0) {
                            $('#myModalAddtoPlanner').modal('hide');
                            swal('Record updated successfully');
                            $state.reload();
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.ismtcrastO_Id === 0 || promise.ismtcrastO_Id < 0) {
                            swal('Failed to add, please contact administrator');
                        }
                        else if (promise.ismtcrastO_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                });
            }
            else {
                $scope.submitted1 = true;
                $('#myModalAddtoPlanner').modal('show');
            }
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        //========================================= Save Planner
        $scope.savePlanner = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.taskplannerArray = [];
                var minieffort = $scope.mineffort;
                if ($scope.currentweekefrt == null || $scope.currentweekefrt == undefined || $scope.currentweekefrt == '') {
                    $scope.currentweekefrt = 0;
                }
                $scope.currentweekefrt = parseFloat($scope.currentweekefrt);
                $scope.currentweekefrt = $scope.currentweekefrt.toFixed(2);
                if ($scope.currentweekefrt >= $scope.mineffort) {
                    angular.forEach($scope.assignedtasklist, function (tp) {
                        if (tp.checkedvalue === true) {
                            if (tp.ISMTPLTA_PreviousTask === 0) {
                                $scope.taskplannerArray.push({ ISMTCR_Id: tp.ISMTCR_Id, ISMTPLTA_StartDate: new Date(tp.tstartdate).toDateString(), ISMTPLTA_EndDate: new Date(tp.tenddate).toDateString(), ISMTPLTA_EffortInHrs: tp.ISMTCRASTO_EffortInHrs, ISMTPLTA_Remarks: tp.ISMTPLTA_Remarks, ISMTPLTA_Id: tp.ISMTPLTA_Id, ISMTPLTA_Status: tp.ISMTPLTA_Status, ISMTPLTA_PreviousTask: tp.ISMTPLTA_PreviousTask, ISMTPLTA_ApprovalFlg: tp.ISMTPLTA_ApprovalFlg });
                            }
                            else {
                                $scope.taskplannerArray.push({ ISMTCR_Id: tp.ISMTCR_Id, ISMTPLTA_StartDate: tp.ISMTPLTA_StartDate, ISMTPLTA_EndDate: tp.ISMTPLTA_EndDate, ISMTPLTA_EffortInHrs: tp.ISMTCRASTO_EffortInHrs, ISMTPLTA_Remarks: tp.ISMTPLTA_Remarks, ISMTPLTA_Id: tp.ISMTPLTA_Id, ISMTPLTA_Status: tp.ISMTPLTA_Status, ISMTPLTA_PreviousTask: tp.ISMTPLTA_PreviousTask, ISMTPLTA_ApprovalFlg: tp.ISMTPLTA_ApprovalFlg });
                            }
                        }
                    });
                    var msg = '';
                    if ($scope.categorylist.length > 0) {
                        var pp = 0;
                        angular.forEach($scope.categorylist, function (tt) {
                            var ddd = 0;
                            if (msg == '') {
                                angular.forEach($scope.effertcalclist, function (dd) {
                                    if (parseInt(tt.ismmtcaT_Id) == parseInt(dd.ISMMTCAT_Id)) {
                                        ddd += parseFloat(dd.ISMTCRASTO_EffortInHrs);
                                    }
                                })
                                if (ddd != 0) {
                                    pp = (parseFloat(tt.ismmtcaT_TaskPercentage) / 100) * parseFloat($scope.mineffort)
                                }
                                else {
                                    msg = "Category--" + tt.ismmtcaT_TaskCategoryName + ' Should Contain ' + tt.ismmtcaT_TaskPercentage + '% Of Your Minimum Effort';
                                }
                                if (parseFloat(pp.toFixed(2)) > parseFloat(ddd.toFixed(2))) {
                                    if (msg == '') {
                                        msg = "Category--" + tt.ismmtcaT_TaskCategoryName + ' Should Contain ' + tt.ismmtcaT_TaskPercentage + '% Of Your Minimum Effort';
                                    }
                                }
                            }
                        })
                    }
                    if (msg == '') {
                        var data = {
                            //"ISMTPL_Id": $scope.ismtpL_Id,
                            "ISMTPL_PlannerName": $scope.ismtpL_PlannerName,
                            "ISMTPL_StartDate": new Date($scope.ismtpL_StartDate).toDateString(),
                            "ISMTPL_EndDate": new Date($scope.ismtpL_EndDate).toDateString(),
                            "ISMTPL_TotalHrs": $scope.totaleffort,
                            "ISMTPL_Remarks": $scope.ismtpL_Remarks,
                            "taskplannerArray": $scope.taskplannerArray,
                            "catlistid": $scope.categoryidlist
                        };
                        var dystring = "Generate";
                        var plannerdatesft = new Date($scope.ismtpL_StartDate).toDateString() + " - " + new Date($scope.ismtpL_EndDate).toDateString();
                        swal({
                            title: "Are you sure?",
                            text: "Do You Want To " + dystring + " Planner from \n  " + plannerdatesft + " ....?",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                            cancelButtonText: "Cancel",
                            closeOnConfirm: false,
                            closeOnCancel: false
                        },
                            function (isConfirm) {
                                if (isConfirm) {
                                    apiService.create("ISM_PlannerCreation/savePlanner", data).then(function (promise) {
                                        if (promise.returnval === true) {
                                            if (promise.ismtpL_Id === 0 || promise.ismtpL_Id < 0) {
                                                swal('Planner Saved successfully ... !!', 'Approval has to be given by your department head');
                                                $state.reload();
                                            }
                                            else if (promise.ismtpL_Id > 0) {
                                                swal('Record updated successfully');
                                                $state.reload();
                                            }
                                        }
                                        else if (promise.returnduplicatestatus === 'Duplicate') {
                                            swal('Record already exist');
                                        }
                                        else {
                                            if (promise.ismtpL_Id === 0 || promise.ismtpL_Id < 0) {
                                                swal('Failed to Save, please contact administrator');
                                            }
                                            else if (promise.ismtpL_Id > 0) {
                                                swal('Failed to update, please contact administrator');
                                            }
                                        }
                                    });
                                }
                                else {
                                    swal("Planner " + dystring + " Cancelled!!!");
                                }
                            });
                    }
                    else {
                        swal(msg);
                    }
                }
                else {
                    swal("Total Effort must be greater than or equal to " + minieffort + " Hours ... !!");
                }
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.removetask = function (objtask) {
            $scope.ISMTCRASTO_Id = objtask.ismtcrastO_Id;
            var tasknum = objtask.ISMTCR_TaskNo;
            var dystring = "Remove";
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Task " + tasknum + "..?",

                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ISM_PlannerCreation/removetask", objtask).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Task " + dystring + "d Successfully..");
                                    $state.reload();
                                }
                                else {
                                    swal("Falied to " + dystring + " Task....!!!");
                                }

                            });
                    }
                    else {
                        swal("Task " + dystring + " Cancelled!!!");
                    }
                });
        };
        // ****** View The Task Description ****** //
        //$scope.viewresponse = function (objs) {
        //    $('#myModalResponse').modal('show');

        //    var data = {
        //        "ISMTCR_Id": objs.ISMTCR_Id
        //    };
        //    apiService.create("ISM_DailyReportGeneration/viewresponse", data).then(function (promise) {

        //        if (promise !== null) {
        //            $scope.gettaskdetails = promise.gettaskdetails;
        //            if ($scope.gettaskdetails !== null && $scope.gettaskdetails.length > 0) {
        //                $scope.tasktitle = $scope.gettaskdetails[0].ISMTCR_Title;
        //                $scope.taskno = $scope.gettaskdetails[0].ISMTCR_TaskNo;
        //                $scope.created_by = $scope.gettaskdetails[0].createdby;
        //                $scope.taskclient = $scope.gettaskdetails[0].ISMMCLT_ClientName;
        //                $scope.taskdate = $scope.gettaskdetails[0].ISMTCR_CreationDate;
        //                $scope.tasktype = $scope.gettaskdetails[0].ISMTCR_BugOREnhancementFlg;
        //                $scope.taskpriority = $scope.gettaskdetails[0].HRMP_Name;
        //                $scope.taskdescription = $scope.gettaskdetails[0].ISMTCR_Desc;
        //                $scope.taskstatus = $scope.gettaskdetails[0].ISMTCR_Status;
        //                $scope.assignedby = $scope.gettaskdetails[0].AssignedEmployee;
        //            }
        //        }
        //    });
        //};
        $scope.viewresponse = function (taskobj) {
            //$('#myModalResponse').modal('show');            
            $scope.ismtcrreS_Response = "";
            $scope.files_paths = "";
            $scope.ismmistS_StatusName = "";
            $scope.taskAttatchment = [];
            $scope.get_responseattachments = [];
            var data = {
                "ISMTCR_Id": taskobj.ISMTCR_Id
            };
            apiService.create("ISM_DailyReportGeneration/viewresponse", data).then(function (promise) {
                $scope.get_taskdetails = promise.get_taskdetails;
                $scope.get_taskresponse = promise.get_taskresponse;
                if ($scope.get_taskdetails !== null && $scope.get_taskdetails.length > 0) {
                    $scope.tasktitle = $scope.get_taskdetails[0].ISMTCR_Title;
                    $scope.taskno = $scope.get_taskdetails[0].ISMTCR_TaskNo;
                    $scope.taskclient = $scope.get_taskdetails[0].ISMMCLT_ClientName;
                    $scope.taskdate = $scope.get_taskdetails[0].ISMTCR_CreationDate;
                    $scope.tasktype = $scope.get_taskdetails[0].tasktype;
                    $scope.tsakpriority = $scope.get_taskdetails[0].HRMP_Name;
                    $scope.taskdescription = $scope.get_taskdetails[0].ISMTCR_Desc;
                    $scope.taskstatus = $scope.get_taskdetails[0].ISMTCR_Status;
                    $scope.createdby = $scope.get_taskdetails[0].createdby;
                    $scope.startdate = new Date($scope.get_taskdetails[0].ISMTCRASTO_StartDate).toDateString();
                    $scope.enddate = new Date($scope.get_taskdetails[0].ISMTCRASTO_EndDate).toDateString();
                    $scope.taskAttatchment = promise.get_taskattachments;
                    $scope.get_responseattachments = promise.get_responseattachments;
                    if ($scope.taskAttatchment.length > 0) {
                        angular.forEach($scope.taskAttatchment, function (at) {
                            if (at.ISMTCRAT_Attatchment !== null && at.ISMTCRAT_Attatchment !== "") {
                                var img = at.ISMTCRAT_Attatchment;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                at.filetype = lastelement;
                            }
                        });
                    }
                    if ($scope.get_responseattachments.length > 0) {
                        angular.forEach($scope.get_responseattachments, function (at) {
                            if (at.ISMTCRRES_ResponseAttachment !== null && at.ISMTCRRES_ResponseAttachment !== "") {
                                var img1 = at.ISMTCRRES_ResponseAttachment;
                                var imagarr1 = img1.split('.');
                                var lastelement1 = imagarr1[imagarr1.length - 1];

                                at.filetype = lastelement1;
                            }
                        });
                    }
                    angular.forEach($scope.get_plannerlist, function (dd) {
                        if (dd.ISMTPLTA_Id === taskobj.ISMTPLTA_Id) {
                            $scope.taskstartdate1 = taskobj.ISMTPLTA_StartDate;
                            $scope.taskenddate1 = taskobj.ISMTPLTA_EndDate;
                        }
                    });
                    $('#myModalResponse').modal('show');
                }
            });
        };
        $scope.previewimg = function (img) {
            $scope.imagepreview = img;
            $('#preview').attr('src', $scope.imagepreview);
            $('#myModalPreview').modal('show');
            $('.modal').on('hide.bs.modal', function (e) {
                e.stopPropagation();
                $('body').css('padding-right', '');
            });
        };
        //=========== Close Image Model
        $scope.closeimg = function () {
            $('#myModalPreview').modal('hide');
            $('.modal-backdrop').remove();
        };
        //========================================= View Mode
        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("asdf").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.ename = '';
        $scope.plnn = '';
        $scope.overalleffort = 0;
        $scope.overalleffortC = 0;
        $scope.overalleffortCP = 0;
        $scope.overalleffortD = 0;
        $scope.overalleffortDP = 0;
        //$scope.showgraph = function () {
        //    $('#myModalViewPlanner').modal('hide');
        //    $('#myModalViewchart').modal('show');
        //};
        $scope.getplannerdetails = function (objp) {
            $scope.ename = '';
            $scope.overalleffortC = 0;
            $scope.overalleffortCP = 0;
            $scope.overalleffortD = 0;
            $scope.overalleffortDP = 0;
            $scope.overalleffort = parseFloat(objp.ISMTPL_TotalHrs);
            var decimaltime = $scope.overalleffort;
            var hrs = parseInt(Number(decimaltime));
            var min = Math.round((Number(decimaltime) - hrs) * 60);
            if (hrs.toString().length === 1) {
                hrs = '0' + hrs;
            }
            if (min.toString().length === 1) {
                min = '0' + min;
            }
            $scope.effhrsov = hrs;
            $scope.effminov = min;
            $scope.plnn = objp.ISMTPL_PlannerName;
            $scope.appflag = objp.ISMTPL_ApprovalFlg;
            $scope.sdate = objp.ISMTPL_StartDate;
            $scope.edate = objp.ISMTPL_EndDate;
            $scope.appchart_extratask = [];
            $scope.appchart_deviation = [];
            $scope.appchart_category = [];
            $('#myModalViewchart').modal('hide');
            $scope.get_plannerdetails = [];
            var data = {
                "ISMTPL_Id": objp.ISMTPL_Id
            };
            apiService.create("ISM_PlannerCreation/getplannerdetails", data).then(function (promise) {
                $scope.plannerdetailsall = [];
                if (promise.get_plannerdetails.length > 0) {
                    $scope.plannerdetails = promise.get_plannerdetails;
                    $scope.totaltaskcount = promise.get_plannerdetails.length;
                    var cntt = 0;
                    angular.forEach($scope.plannerdetails, function (pl) {
                        var effrt11 = 0;
                        var status = '';
                        cntt += 1;
                        if (cntt === 1) {
                            $scope.ename = pl.ENAME;
                            $scope.approvedby = pl.approvedby;
                        }
                        var cls = '';
                        if (pl.ISMTPLTA_PreviousTask !== null && pl.ISMTPLTA_PreviousTask == true) {
                            cls = 'backkk';
                        }
                        if (pl.ISMTPLAP_Id === null) {

                            if (pl.ISMTPL_ApprovalFlg === true) {
                                status = "APPROVED";
                            }
                            else {
                                status = "PENDING";
                            }
                        }
                        else {
                            if (pl.ISMTPLTA_ApprovalFlg === false) {

                                status = "REJECTED";
                            }
                            else {

                                status = "APPROVED";
                            }
                        }
                        $scope.get_plannerdetails.push({
                            ISMTPL_Id: pl.ISMTPL_Id, ISMTCR_Id: pl.ISMTCR_Id, HRMP_Name: pl.HRMP_Name, ISMTCR_BugOREnhancementFlg: pl.ISMTCR_BugOREnhancementFlg, ISMTCR_Title: pl.ISMTCR_Title, ISMTCR_Status: pl.ISMTCR_Status, ISMTCR_TaskNo: pl.ISMTCR_TaskNo, ISMMCLT_ClientName: pl.ISMMCLT_ClientName, ISMTPL_CreatedBy: pl.ISMTPL_CreatedBy, plannedby: pl.plannedby, ISMTPL_PlannerName: pl.ISMTPL_PlannerName, ISMTPLTA_Remarks: pl.ISMTPLTA_Remarks, CreatedDate: pl.CreatedDate, ISMTCR_Desc: pl.ISMTCR_Desc, ISMTPLTA_Status: pl.ISMTPLTA_Status,
                            ISMTCR_CreationDate: pl.ISMTCR_CreationDate, ISMTPLTA_StartDate: new Date(pl.ISMTPLTA_StartDate), ISMTPLTA_EndDate: new Date(pl.ISMTPLTA_EndDate), ISMTPLTA_EffortInHrs: pl.ISMTPLTA_EffortInHrs, ISMMTCAT_Id: pl.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: pl.ISMMTCAT_TaskCategoryName, ISMMTCAT_TaskPercentage: pl.ISMMTCAT_TaskPercentage, ISMMTCAT_DurationFlg: pl.ISMMTCAT_DurationFlg, ISMMTCAT_CompulsoryFlg: pl.ISMMTCAT_CompulsoryFlg, ISMMTCAT_EachTaskMaxDuration: pl.ISMMTCAT_EachTaskMaxDuration, Flg: pl.ISMTPLTA_PreviousTask, cls: cls, status: status, ISMTPLTA_ExtraFlg: pl.ISMTPLTA_ExtraFlg, Periodicity: pl.Periodicity
                        });
                        $scope.plannerdetailsall = $scope.get_plannerdetails;
                    });
                    //============Shivu start========
                    $scope.get_plannerdetails_approval = [];
                    $scope.get_plannerdetails_reject = [];
                    $scope.get_plannerdetails_previous = [];
                    $scope.get_plannerdetails_extratask = [];
                    angular.forEach($scope.get_plannerdetails, function (dev) {
                        if (dev.ISMTPLTA_ExtraFlg === false) {
                            if (dev.cls !== "backkk") {
                                if (dev.status === "APPROVED") {
                                    $scope.get_plannerdetails_approval.push(dev);
                                    $scope.pending = false;
                                }
                                else if (dev.status === "PENDING") {
                                    $scope.get_plannerdetails_approval.push(dev);
                                    $scope.pending = true;
                                }
                                else {
                                    $scope.get_plannerdetails_reject.push(dev);
                                }
                            }
                            else {
                                $scope.get_plannerdetails_previous.push(dev);
                            }
                        }
                        else {
                            $scope.get_plannerdetails_extratask.push(dev);
                        }
                    });
                    $scope.get_plannerdetails = $scope.get_plannerdetails_approval
                    if ($scope.get_plannerdetails_extratask.length > 0) {
                        $scope.extratask_one = $scope.get_plannerdetails_extratask.length
                    }
                    $scope.extratask = [];
                    $scope.extratask.push({ extracount: $scope.extratask_one })
                    //-------------------Approval/Pending
                    $scope.taskid = [];
                    angular.forEach($scope.get_plannerdetails, function (dev) {
                        if ($scope.taskid.length === 0) {
                            $scope.taskid.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                        } else if ($scope.taskid.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.taskid, function (emp) {
                                if (emp.ISMMTCAT_Id === dev.ISMMTCAT_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.taskid.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                            }
                        }
                    });
                    console.log($scope.taskid);
                    angular.forEach($scope.taskid, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.get_plannerdetails, function (dd) {
                            if (dd.ISMMTCAT_Id === ddd.ISMMTCAT_Id) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.get_plannerdetails = $scope.templist;
                    });
                    //------------------Reject
                    $scope.taskid1 = [];
                    angular.forEach($scope.get_plannerdetails_reject, function (dev) {
                        if ($scope.taskid1.length === 0) {
                            $scope.taskid1.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                        } else if ($scope.taskid1.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.taskid1, function (emp) {
                                if (emp.ISMMTCAT_Id === dev.ISMMTCAT_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.taskid1.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                            }
                        }
                    });
                    console.log($scope.taskid1);
                    angular.forEach($scope.taskid1, function (ddd) {
                        $scope.templist1 = [];
                        angular.forEach($scope.get_plannerdetails_reject, function (dd) {
                            if (dd.ISMMTCAT_Id === ddd.ISMMTCAT_Id) {
                                $scope.templist1.push(dd);
                            }
                        });
                        ddd.get_plannerdetails_reject = $scope.templist1;
                    });
                    //------------------extra task
                    $scope.taskid3 = [];
                    angular.forEach($scope.get_plannerdetails_extratask, function (dev) {
                        if ($scope.taskid3.length === 0) {
                            $scope.taskid3.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                        } else if ($scope.taskid3.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.taskid3, function (emp) {
                                if (emp.ISMMTCAT_Id === dev.ISMMTCAT_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.taskid3.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                            }
                        }
                    });
                    console.log($scope.taskid3);
                    angular.forEach($scope.taskid3, function (ddd) {
                        $scope.templist3 = [];
                        angular.forEach($scope.get_plannerdetails_extratask, function (dd) {
                            if (dd.ISMMTCAT_Id === ddd.ISMMTCAT_Id) {
                                $scope.templist3.push(dd);
                            }
                        });
                        ddd.get_plannerdetails_extratask = $scope.templist3;
                    });
                    //-------------------Deviated
                    $scope.taskid5 = [];
                    angular.forEach($scope.get_plannerdetails_previous, function (dev) {
                        if ($scope.taskid5.length === 0) {
                            $scope.taskid5.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                        } else if ($scope.taskid5.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.taskid5, function (emp) {
                                if (emp.ISMMTCAT_Id === dev.ISMMTCAT_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.taskid5.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                            }
                        }
                    });
                    console.log($scope.taskid5);
                    angular.forEach($scope.taskid5, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.get_plannerdetails_previous, function (dd) {
                            if (dd.ISMMTCAT_Id === ddd.ISMMTCAT_Id) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.get_plannerdetails_previous = $scope.templist;
                    });
                    //-------------------------------------
                    angular.forEach($scope.get_plannerdetails_previous, function (dd) {
                        var decimaltime = dd.ISMTPLTA_EffortInHrs;
                        var hrs = parseInt(Number(decimaltime));
                        var min = Math.round((Number(decimaltime) - hrs) * 60);
                        if (hrs.toString().length === 1) {
                            hrs = '0' + hrs;
                        }
                        if (min.toString().length === 1) {
                            min = '0' + min;
                        }
                        dd.effhrs = hrs;
                        dd.effmin = min;
                    });
                    angular.forEach($scope.get_plannerdetails, function (dd) {
                        var decimaltime = dd.ISMTPLTA_EffortInHrs;
                        var hrs = parseInt(Number(decimaltime));
                        var min = Math.round((Number(decimaltime) - hrs) * 60);
                        if (hrs.toString().length === 1) {
                            hrs = '0' + hrs;
                        }
                        if (min.toString().length === 1) {
                            min = '0' + min;
                        }
                        dd.effhrs = hrs;
                        dd.effmin = min;
                    });
                    angular.forEach($scope.get_plannerdetails_reject, function (dd) {
                        var decimaltime = dd.ISMTPLTA_EffortInHrs;
                        var hrs = parseInt(Number(decimaltime));
                        var min = Math.round((Number(decimaltime) - hrs) * 60);
                        if (hrs.toString().length === 1) {
                            hrs = '0' + hrs;
                        }
                        if (min.toString().length === 1) {
                            min = '0' + min;
                        }
                        dd.effhrs = hrs;
                        dd.effmin = min;
                    });
                    angular.forEach($scope.get_plannerdetails_extratask, function (dd) {
                        var decimaltime = dd.ISMTPLTA_EffortInHrs;
                        var hrs = parseInt(Number(decimaltime));
                        var min = Math.round((Number(decimaltime) - hrs) * 60);
                        if (hrs.toString().length === 1) {
                            hrs = '0' + hrs;
                        }
                        if (min.toString().length === 1) {
                            min = '0' + min;
                        }
                        dd.effhrs = hrs;
                        dd.effmin = min;
                    });

                    //============Shivu End========


                    $scope.categoryidlist1 = [];
                    var snocount = 0;
                    var countsubgrp = 0;
                    angular.forEach($scope.plannerdetailsall, function (sub) {
                        if ($scope.categoryidlist1.length === 0) {
                            snocount += 1;
                            $scope.categoryidlist1.push({ ISMMTCAT_Id: sub.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: sub.ISMMTCAT_TaskCategoryName, ISMMTCAT_TaskPercentage: sub.ISMMTCAT_TaskPercentage, ISMMTCAT_DurationFlg: sub.ISMMTCAT_DurationFlg, ISMMTCAT_CompulsoryFlg: sub.ISMMTCAT_CompulsoryFlg, ISMMTCAT_EachTaskMaxDuration: sub.ISMMTCAT_EachTaskMaxDuration });
                        } else if ($scope.categoryidlist1.length > 0) {
                            countsubgrp = 0;
                            angular.forEach($scope.categoryidlist1, function (subgrp) {
                                if (subgrp.ISMMTCAT_Id === sub.ISMMTCAT_Id) {
                                    countsubgrp += 1;
                                }
                            });
                            if (countsubgrp === 0) {
                                snocount += 1;
                                $scope.categoryidlist1.push({ ISMMTCAT_Id: sub.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: sub.ISMMTCAT_TaskCategoryName, ISMMTCAT_TaskPercentage: sub.ISMMTCAT_TaskPercentage, ISMMTCAT_DurationFlg: sub.ISMMTCAT_DurationFlg, ISMMTCAT_CompulsoryFlg: sub.ISMMTCAT_CompulsoryFlg, ISMMTCAT_EachTaskMaxDuration: sub.ISMMTCAT_EachTaskMaxDuration });
                            }
                        }
                    });
                    $scope.finalcategoryidlist = [];
                    $scope.overalleffortC = 0;
                    $scope.overalleffortCP = 0;
                    $scope.overalleffortD = 0;
                    $scope.overalleffortDP = 0;
                    angular.forEach($scope.plannerdetailsall, function (ww) {
                        if (ww.status != 'REJECTED') {
                            if (ww.Flg == true && ww.Flg != null) {
                                $scope.overalleffortD = parseFloat($scope.overalleffortD) + parseFloat(ww.ISMTPLTA_EffortInHrs)
                                var decimaltime1 = ($scope.overalleffortD);
                                var hrsc = parseInt(Number(decimaltime1));
                                var minc = Math.round((Number(decimaltime1) - hrsc) * 60);
                                if (hrsc.toString().length === 1) {
                                    hrsc = '0' + hrsc;
                                }
                                if (minc.toString().length === 1) {
                                    minc = '0' + minc;
                                }
                                $scope.effhrsovdc = hrsc;
                                $scope.effminovdc = minc;
                            }
                        }
                    });
                    if ($scope.overalleffort > 0) {
                        $scope.overalleffortCP = (($scope.overalleffort - $scope.overalleffortD) / $scope.overalleffort) * 100;
                        $scope.overalleffortDP = ($scope.overalleffortD / $scope.overalleffort) * 100;
                        var decimaltime = ($scope.overalleffort - $scope.overalleffortD);
                        var hrs = parseInt(Number(decimaltime));
                        var min = Math.round((Number(decimaltime) - hrs) * 60);
                        if (hrs.toString().length === 1) {
                            hrs = '0' + hrs;
                        }
                        if (min.toString().length === 1) {
                            min = '0' + min;
                        }
                        $scope.effhrsovd = hrs;
                        $scope.effminovd = min;
                    }
                    angular.forEach($scope.categoryidlist1, function (rr) {
                        var cateffort = 0;
                        var devcateffort = 0;
                        var curper = 0;
                        var devper = 0;
                        angular.forEach($scope.plannerdetailsall, function (ff) {
                            if (ff.ISMMTCAT_Id != null && ff.ISMMTCAT_Id != undefined) {
                                if (parseInt(rr.ISMMTCAT_Id) == parseInt(ff.ISMMTCAT_Id)) {
                                    if (ff.status != 'REJECTED') {
                                        if (ff.Flg == true) {
                                            devcateffort = devcateffort + parseFloat(ff.ISMTPLTA_EffortInHrs);
                                        }
                                        if (ff.status != 'REJECTED') {
                                            cateffort = cateffort + parseFloat(ff.ISMTPLTA_EffortInHrs);
                                        }
                                    }
                                }
                            }
                        });
                        if (cateffort != 0) {
                            curper = (parseFloat(cateffort - devcateffort) / cateffort) * 100;
                            devper = (parseFloat(devcateffort) / cateffort) * 100;
                            $scope.finalcategoryidlist.push({
                                ISMMTCAT_Id: rr.ISMMTCAT_Id,
                                CNAME: rr.ISMMTCAT_TaskCategoryName,
                                MINP: rr.ISMMTCAT_TaskPercentage,
                                TE: cateffort, DE: devcateffort, CWP: curper, DP: devper, flg: rr.ISMMTCAT_CompulsoryFlg
                            })
                            $scope.finalcategory = [];
                            $scope.finalcategory = $scope.finalcategoryidlist
                        }
                        angular.forEach($scope.finalcategoryidlist, function (dd) {
                            var cateff = dd.TE;
                            var hrscat = parseInt(Number(cateff));
                            var mincat = Math.round((Number(cateff) - hrscat) * 60);
                            if (hrscat.toString().length === 1) {
                                hrscat = '0' + hrscat;
                            }
                            if (mincat.toString().length === 1) {
                                mincat = '0' + mincat;
                            }
                            dd.effhrcat = hrscat;
                            dd.effmicat = mincat;
                            var cateffCCE = dd.TE - dd.DE;
                            var hrscatCCE = parseInt(Number(cateffCCE));
                            var mincatCCE = Math.round((Number(cateffCCE) - hrscatCCE) * 60);
                            if (hrscatCCE.toString().length === 1) {
                                hrscatCCE = '0' + hrscatCCE;
                            }
                            if (mincatCCE.toString().length === 1) {
                                mincatCCE = '0' + mincatCCE;
                            }
                            dd.effhrcatCCE = hrscatCCE;
                            dd.effmicatCCE = mincatCCE;
                            var catefffin = dd.DE;
                            var hrscatfin = parseInt(Number(catefffin));
                            var mincatfin = Math.round((Number(catefffin) - hrscatfin) * 60);
                            if (hrscatfin.toString().length === 1) {
                                hrscatfin = '0' + hrscatfin;
                            }
                            if (mincatfin.toString().length === 1) {
                                mincatfin = '0' + mincatfin;
                            }
                            dd.effhrcatfin = hrscatfin;
                            dd.effmicatfin = mincatfin;
                        });
                    })
                    //====================
                    $scope.get_category = [];
                    angular.forEach($scope.finalcategory, function (qq) {
                        $scope.appname = qq.CNAME;
                        $scope.appdata = qq.MINP;
                        $scope.get_category.push({ Id: $scope.appname, Data: $scope.appdata });
                    })
                    $scope.appchart_category = [];
                    angular.forEach($scope.get_category, function (value, index) {
                        $scope.appchart_category.push({ 'label': value.Id, 'y': value.Data });
                    });
                    var chart = new CanvasJS.Chart("appchartcategory", {
                        title: {
                            text: " Chart Representation Of Category "
                        },
                        data: [
                            {
                                // Change type to "doughnut", "line", "splineArea", etc.
                                type: "line",
                                dataPoints: $scope.appchart_category
                            }
                        ]
                    });
                    chart.render();
                    //=====================
                    $scope.get_deviation = [];
                    $scope.get_deviationtotal = [];
                    $scope.appdatanew = 0;
                    $scope.appname = "Current Week Effort";
                    $scope.appdata = $scope.overalleffortCP / 100;
                    $scope.get_deviation.push({ Id: $scope.appname, Data: $scope.appdata });
                    $scope.appname = "Deviated Effort";
                    $scope.appdata = $scope.overalleffortDP / 100;
                    $scope.get_deviation.push({ Id: $scope.appname, Data: $scope.appdata });
                    $scope.appchart_deviation = [];
                    angular.forEach($scope.get_deviation, function (value, index) {
                        $scope.appchart_deviation.push({ 'label': value.Id, 'y': value.Data });
                    });
                    var chart = new CanvasJS.Chart("appchartdeviation", {
                        animationEnabled: true,
                        title: {
                            text: " Chart Representation Of Last Week Deviation "
                        },
                        data: [
                            {
                                // Change type to "doughnut", "line", "splineArea", etc.
                                type: "pie",
                                startAngle: 240,
                                yValueFormatString: "#,##0.########%",
                                indexLabel: "{label} {y}",
                                dataPoints: $scope.appchart_deviation
                            }
                        ]
                    });
                    chart.render();
                    //=======================================
                    //=====================
                    $scope.get_extratask = [];
                    $scope.appname = "Total Task";
                    $scope.appdata = $scope.totaltaskcount;
                    $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });
                    $scope.appname = "Deviated Task";
                    $scope.appdata = $scope.get_plannerdetails_previous.length;
                    $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });
                    if ($scope.pending === true) {
                        $scope.appname = "Approval Pending Task";
                        $scope.appdata = $scope.get_plannerdetails.length;
                        $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });
                    }
                    else {
                        $scope.appname = "Approved Task";
                        $scope.appdata = $scope.get_plannerdetails.length;
                        $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });
                    }
                    if ($scope.pending === false) {
                        $scope.appname = "Rejected Task";
                        $scope.appdata = $scope.get_plannerdetails_reject.length;
                        $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });
                        $scope.appname = "Extra Task";
                        $scope.appdata = $scope.get_plannerdetails_extratask.length;
                        $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });
                    }
                    $scope.appchart_extratask = [];
                    angular.forEach($scope.get_extratask, function (value, index) {
                        $scope.appchart_extratask.push({ 'label': value.Id, 'y': value.Data });
                    });
                    var chart = new CanvasJS.Chart("appcharttotaltask", {
                        title: {
                            text: " Chart Representation Of Total Task"
                        },
                        data: [
                            {
                                // Change type to "doughnut", "line", "splineArea", etc.
                                type: "column",
                                dataPoints: $scope.appchart_extratask
                            }
                        ]
                    });
                    chart.render();
                    //=======================================
                    console.log($scope.finalcategoryidlist);
                }
                else {
                    swal("No Record  Found..... !!");
                    $scope.get_plannerdetails = "";
                }
            });
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //===========Clear Field
        $scope.cancel = function () {
            $state.reload();
        };




        $scope.YearPlannerReject = function (obj) {
            swal({
                title: "You Can't Create Planner",
                text: `
                    <div>
                          <span style="color:red;">Note : </span><b>Your Year Planner Report tasks have been rejected. Kindly create the necessary tasks for approval. Once the approval is complete, you may proceed to create the planner.</b> 
                    </div>`,
                html: true,
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "CREATE",
                cancelButtonText: "CANCEL",
                closeOnConfirm: true,
                closeOnCancel: true
            },
                function (isConfirm) {
                    $scope.reporttype = isConfirm ? 'Yes' : 'No';
                    if ($scope.reporttype == "Yes") {
                        $window.location.href = 'http://localhost:57606/#/app/ISM_AdvanceplannerTaskcreation/';
                    }
                    else {
                        $window.location.href = 'http://localhost:57606/#/app/ISMDashboard/';
                    }
                 
                });
        };


    }
})();

