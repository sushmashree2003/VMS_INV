(function () {
    'use strict';
    angular
        .module('app')
        .controller('employeeMonthEndController', employeeMonthEndController);

    employeeMonthEndController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', '$timeout'];
    function employeeMonthEndController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, $timeout) {

        $scope.EarningDet = {};
        $scope.addjob = {};
        $scope.obj = {};
        $scope.addjob.hrjD_Id = 0;
        $scope.submitted = true;
        $scope.grid_view = false;


        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("EmployeeMonthEnd/getemplist", pageid).then(function (promise) {
                $scope.employeelist = promise.employeelist;
                $scope.fillmonth = promise.monthlist;
                $scope.fillyear = promise.yearlist;
            });
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.submitted = false;
        $scope.interacted1 = function (field) {
            return $scope.submitted;
        };

        $scope.findreport = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "HRME_ID": $scope.hrmE_Id,
                    "IVRM_Month_Id": $scope.month,
                    "HRMLY_Id": $scope.year
                };

                apiService.create("EmployeeMonthEnd/getempdetails", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {
                            $scope.currentdate = new Date();

                            if (promise.employeedetails.length > 0 && promise.employeedetails != null) {
                                $scope.candidateDetails = promise.employeedetails[0];
                                $scope.grid_view = true;
                            }

                            $scope.monthname = promise.monthname;
                            $scope.companydetails = promise.companydetails[0];

                            if (promise.rndcount.length > 0) { $scope.rndcount = promise.rndcount[0].OpenIssuesCount; } else
                            { $scope.rndcount = 0; }

                            $scope.plannedtaskcount = promise.plannedtaskcount;

                            if (promise.criticalCount.length > 0) { $scope.criticalCount = promise.criticalCount[0].OpenIssuesCount; } else
                            { $scope.criticalCount = 0; }

                            if (promise.majortaskCount.length > 0) {
                            $scope.majortaskCount = promise.majortaskCount[0].CompletedTaskCount; } else
                            { $scope.majortaskCount = 0; }

                            if (promise.minortaskCount.length > 0) {
                                $scope.minortaskCount = promise.minortaskCount[0].CompletedTaskCount;
                            } else { $scope.minortaskCount = 0; }

                            if (promise.completedtaskCount.length > 0) {
                                $scope.completedtaskCount = promise.completedtaskCount[0].CompletedTaskCount;
                            } else { $scope.completedtaskCount = 0; }

                            if (promise.issueOpenCount.length > 0) {
                                $scope.issueOpenCount = promise.issueOpenCount[0].CompletedTaskCount;
                            } else { $scope.issueOpenCount = 0; }

                            if (promise.issuecompleteCount.length > 0) {
                                $scope.issuecompleteCount = promise.issuecompleteCount[0].CompletedTaskCount;
                            } else { $scope.issuecompleteCount = 0; }

                            if (promise.enhancementOpenCount.length > 0) {
                                $scope.enhancementOpenCount = promise.enhancementOpenCount[0].CompletedTaskCount;
                            } else { $scope.enhancementOpenCount = 0; }

                            if (promise.enhancementcompltCount.length > 0) {
                                $scope.enhancementcompltCount = promise.enhancementcompltCount[0].CompletedTaskCount;
                            } else { $scope.enhancementcompltCount = 0; }

                            if (promise.monthlyDeviation.length > 0) {
                                $scope.monthlyDeviation = promise.monthlyDeviation[0].devPer;
                            } else { $scope.monthlyDeviation = 0; }

                            $scope.drgeneratedcount = promise.drgeneratedcount;
                            $scope.dailyreptnotsent = promise.dailyreptnotsent.length;
                            $scope.drgeneratedcount = promise.drgeneratedcount;
                            $scope.drhoursplanned = promise.drhoursplanned;
                            $scope.drhoursworked = promise.drhoursworked;

                            if (promise.drhoursapproved == null) { $scope.drhoursapproved = 0; } else { $scope.drhoursapproved = promise.drhoursapproved; }
                            //$scope.drhoursapproved = promise.drhoursapproved;

                            if (promise.drhoursarejected == null) { $scope.drhoursarejected = 0;} else { $scope.drhoursarejected = promise.drhoursarejected; }
                            
                            $scope.workingdayscount = promise.workingdayscount;
                            $scope.presentdayscount = promise.presentdayscount;
                            $scope.staffleaveDetails = promise.presentdayscount; //promise.staffleaveDetails;
                            $scope.leavescount = promise.leavescount;
                            $scope.appliedleavescount = promise.appliedleavescount;
                            $scope.approvedleavescount = promise.approvedleavescount;
                            $scope.rejectedleavescount = promise.rejectedleavescount;
                        }
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.printToCart = function (vapsappointment) {
            var innerContents = document.getElementById("printdata").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.Savedata = function () {

            var data = {
                "HRME_ID": $scope.hrmE_Id,
                "ISMEWMED_MonthName": $scope.month,
                "ISMEWMED_Year": $scope.year,
                "ISMEWMED_NoOfResDev": $scope.rndcount,
                "ISMEWMED_PtsAsPerPlannerCount": $scope.plannedtaskcount,
                "ISMEWMED_NoOfCriticalTasksOpen": $scope.criticalCount,
                "ISMEWMED_NoOfMajorTasksOpen": $scope.majortaskCount,
                "ISMEWMED_NoOfMinorTasksOpen": $scope.minortaskCount,
                "ISMEWMED_NoOfTasksCompleted": $scope.completedtaskCount,
                "ISMEWMED_NoOfIssuesOpen": $scope.issueOpenCount,
                "ISMEWMED_NoOfIssuesCompleted": $scope.issuecompleteCount,
                "ISMEWMED_NoOfEnhancementsOpen": $scope.enhancementOpenCount,
                "ISMEWMED_NoOfEnhancementsCompleted": $scope.enhancementcompltCount,
                "ISMEWMED_MonthlyDeviation": $scope.monthlyDeviation,
                "ISMEWMED_NoOfDaysDrGenerated": $scope.drgeneratedcount,
                "ISMEWMED_NoOfDaysDrNotGenerated": $scope.dailyreptnotsent,
                "ISMEWMED_NoOfWeeksPlCreated": $scope.drgeneratedcount,
                "ISMEWMED_NoOfWeeksPlNotCreated": $scope.drgeneratedcount,
                "ISMEWMED_NoOfActualhrsPlanned": $scope.drhoursplanned,
                "ISMEWMED_NoOfHrsWorked": $scope.drhoursworked,
                "ISMEWMED_NoOfHrsApproved": $scope.drhoursapproved,
                "ISMEWMED_NoOfHrsRejected": $scope.drhoursarejected,
                "ISMEWMED_NoOfWorkingDays": $scope.workingdayscount,
                "ISMEWMED_NoOfDaysPresent": $scope.presentdayscount,
                "ISMEWMED_NoOfDaysAbsent": $scope.year,
                "ISMEWMED_TotalNoOfLeaves": $scope.leavescount,
                "ISMEWMED_NoOfLeavesApplied": $scope.appliedleavescount,
                "ISMEWMED_NoOfLeavesApproved": $scope.approvedleavescount,
                "ISMEWMED_NoOfLeavesRejected": $scope.rejectedleavescount,
                "ISMEWMED_NoOfSods": $scope.ismewmeD_NoOfSods,
                "ISMEWMED_Rating": $scope.ismewmeD_Rating,
                "ISMEWMED_EscalationsFromClient": $scope.ismewmeD_EscalationsFromClient,
                "ISMEWMED_NoOfMemo": $scope.ismewmeD_NoOfMemo,
                "ISMEWMED_NoOfNotices": $scope.ismewmeD_NoOfNotices,
                "ISMEWMED_AppeciationsfromClients": $scope.ismewmeD_AppeciationsfromClients,
                "ISMEWMED_Remarks": $scope.obj.ismewmeD_Remarks,
                "ISMEWMED_NoOfResDevRemarks": $scope.ismewmeD_NoOfResDevRemarks,
                "ISMEWMED_PtsAsPerPlannerCountRemarks": $scope.ismewmeD_PtsAsPerPlannerCountRemarks,
                "ISMEWMED_NoOfCriticalTasksOpenRemarks": $scope.ismewmeD_NoOfCriticalTasksOpenRemarks,
                "ISMEWMED_NoOfMajorTasksOpenRemarks": $scope.ismewmeD_NoOfMajorTasksOpenRemarks,
                "ISMEWMED_NoOfMinorTasksOpenRemarks": $scope.ismewmeD_NoOfMinorTasksOpenRemarks,
                "ISMEWMED_NoOfTasksCompletedRemarks": $scope.ismewmeD_NoOfTasksCompletedRemarks,
                "ISMEWMED_NoOfIssuesOpenRemarks": $scope.ismewmeD_NoOfIssuesOpenRemarks,
                "ISMEWMED_NoOfIssuesCompletedRemarks": $scope.ismewmeD_NoOfIssuesCompletedRemarks,
                "ISMEWMED_NoOfEnhancementsOpenRemarks": $scope.ismewmeD_NoOfEnhancementsOpenRemarks,
                "ISMEWMED_NoOfEnhancementsCompletedRemarks": $scope.ismewmeD_NoOfEnhancementsCompletedRemarks,
                "ISMEWMED_MonthlyDeviationRemarks": $scope.ismewmeD_MonthlyDeviationRemarks,
                "ISMEWMED_NoOfDaysDrGeneratedRemarks": $scope.ismewmeD_NoOfDaysDrGeneratedRemarks,
                "ISMEWMED_NoOfDaysDrNotGeneratedRemarks": $scope.ismewmeD_NoOfDaysDrNotGeneratedRemarks,
                "ISMEWMED_NoOfWeeksPlCreatedRemarks": $scope.ismewmeD_NoOfWeeksPlCreatedRemarks,
                "ISMEWMED_NoOfWeeksPlNotCreatedRemarks": $scope.ismewmeD_NoOfWeeksPlNotCreatedRemarks,
                "ISMEWMED_NoOfActualhrsPlannedRemarks": $scope.ismewmeD_NoOfActualhrsPlannedRemarks,
                "ISMEWMED_NoOfHrsWorkedRemarks": $scope.ismewmeD_NoOfHrsWorkedRemarks,
                "ISMEWMED_NoOfHrsApprovedRemarks": $scope.ismewmeD_NoOfHrsApprovedRemarks,
                "ISMEWMED_NoOfHrsRejectedRemarks": $scope.ismewmeD_NoOfHrsRejectedRemarks,
                "ISMEWMED_NoOfWorkingDaysRemarks": $scope.ismewmeD_NoOfWorkingDaysRemarks,
                "ISMEWMED_NoOfDaysPresentRemarks": $scope.ismewmeD_NoOfDaysPresentRemarks,
                "ISMEWMED_NoOfDaysAbsentRemarks": $scope.ismewmeD_NoOfDaysAbsentRemarks,
                "ISMEWMED_TotalNoOfLeavesRemarks": $scope.ismewmeD_TotalNoOfLeavesRemarks,
                "ISMEWMED_NoOfLeavesAppliedRemarks": $scope.ismewmeD_NoOfLeavesAppliedRemarks,
                "ISMEWMED_NoOfLeavesApprovedRemarks": $scope.ismewmeD_NoOfLeavesApprovedRemarks,
                "ISMEWMED_NoOfLeavesRejectedRemarks": $scope.ismewmeD_NoOfLeavesRejectedRemarks,
                "ISMEWMED_NoOfSodsRemarks": $scope.ismewmeD_NoOfSodsRemarks,
                "ISMEWMED_RatingRemarks": $scope.ismewmeD_RatingRemarks,
                "ISMEWMED_EscalationsFromClientRemarks": $scope.ismewmeD_EscalationsFromClientRemarks,
                "ISMEWMED_NoOfMemoRemarks": $scope.ismewmeD_NoOfMemoRemarks,
                "ISMEWMED_NoOfNoticesRemarks": $scope.ismewmeD_NoOfNoticesRemarks,
                "ISMEWMED_AppeciationsfromClientsRemarks": $scope.ismewmeD_AppeciationsfromClientsRemarks
            };

            apiService.create("EmployeeMonthEnd/savereport", data).
                then(function (promise) {
                    if (promise.message === 'False') {
                        swal("Failed to Save/Update");
                    }
                    if (promise.message === 'Update') {
                        swal("Record Updated Successfully.");
                        $state.reload();
                    }                    
                    else if (promise.message === 'Add') {
                        swal('Record Saved Successfully.');
                        $state.reload();
                    }
                });

        };
    }
})();