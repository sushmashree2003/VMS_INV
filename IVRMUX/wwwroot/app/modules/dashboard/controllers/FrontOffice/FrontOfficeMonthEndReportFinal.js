(function () {
    'use strict';

    angular
        .module('app')
        .controller('FrontOfficeMonthEndFinalReport', FrontOfficeMonthEndFinalReport);

    FrontOfficeMonthEndFinalReport.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']

    function FrontOfficeMonthEndFinalReport($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {
        
        $scope.ddate = new Date();
        $scope.maxDatemf = new Date();
        $scope.earlyoutcount = 0;
        $scope.latecomingcount = 0;
        $scope.summinutes = 0;
        $scope.lateindates = "";
        $scope.lateindateafter = "";
        $scope.earlyoutdates = "";
        $scope.earlyoutdateafter = "";
        $scope.emptotlop = 0;
        $scope.dailyrptncount = 0;
        $scope.dailyrptndates = 0;

        $scope.gettodate = function () {
            $scope.minDatemf = $scope.fromdate;
        };

        $scope.loadData = function () {
            var id = 2;
            apiService.getURI("FrontOffice/", id).
                then(function (promise) {
                    $scope.fillmonth = promise.fillmonth;
                    $scope.fillyear = promise.fillyear;
                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clear_Details = function () {
            $state.reload();
        };

        $scope.getchanged = function () {
            $scope.report = false;
        };

        $scope.makeminutes = function (time1) {
            var hour1 = 0;
            var minute1 = 0;
            var totmin = 0;

            hour1 = time1.split(':')[0];
            minute1 = time1.split(':')[1];

            if (hour1 > 0) {
                totmin = parseInt(hour1) * 60;
            }
            totmin = parseInt(totmin) + parseInt(minute1);
            return totmin;
        };

        $scope.user_check = function () {
            if ($scope.user_check == 1) {
                $scope.userl = 1;
            }
            else {
                $scope.userl = 0;
            }
        };

        $scope.GetFinalReport = function () {
            if ($scope.myForm.$valid) {

                var data = {
                    "yearid": $scope.year,
                    "month": $scope.month
                };

                apiService.create("FrontOffice/GetFinalReport", data).
                    then(function (promise) {

                        $scope.monthendreport = promise.consolidateReportTempDTO;
                        $scope.salaryReport = promise.consolidateSalaryReport;
                        var temp_array1 = [];

                        $scope.numberofdays = new Date($scope.year, $scope.month, 0).getDate();

                        angular.forEach($scope.monthendreport, function (ff) {
                            //APPROVED LEAVE DETAILS
                            var leave_array = [];
                            var approvedleave = $filter('filter')(promise.leaveDetails, function (d) {
                                return d.HRME_Id == ff.hrmE_Id;
                            });
                            var newleave = {};

                            for (var i = 0; i < approvedleave.length; i++) {
                                newleave = { approvedleavetype: approvedleave[i].HRML_LeaveName, approvedleavedays: approvedleave[i].HRELT_TotDays, leavebalance: approvedleave[i].RunningBalLeaves };
                                leave_array.push(newleave);
                                newleave = {};
                            }
                            //APPROVED LEAVE DETAILS

                            //SALARY DETAILS
                            var salary_array = [];
                            var newsalary = {};
                            var salarydetails = $filter('filter')(promise.currentsalary, function (d) {
                                return d.hrmE_Id == ff.hrmE_Id;
                            });

                            for (var i = 0; i < salarydetails.length; i++) {
                                newsalary = { salaryhead: salarydetails[i].hrmeD_Name, headamount: salarydetails[i].hresD_Amount };
                                salary_array.push(newsalary);
                                newsalary = {};
                            }
                            //SALARY DETAILS

                            var newCol = {
                                emailid: ff.emailid, hrmE_Id: ff.hrmE_Id, vmscrT_DesigCode: ff.vmscrT_DesigCode, vmscrT_EName: ff.vmscrT_EName, approvedleavedetails: leave_array, salarydetails: salary_array, vmscrT_Id: ff.vmscrT_Id, vmscrT_Year: ff.vmscrT_Year, vmscrT_Month: ff.vmscrT_Month, vmscrT_EMPCode: ff.vmscrT_EMPCode, vmscrT_Earlyout: ff.vmscrT_Earlyout, vmscrT_DailyReportnotsend: ff.vmscrT_DailyReportnotsend, vmscrT_Latecoming: ff.vmscrT_Latecoming, vmscrT_unauthorizedleave: ff.vmscrT_unauthorizedleave, vmscrT_DRDetails: ff.vmscrT_DRDetails, vmscrT_ULDetails: ff.vmscrT_ULDetails, vmscrT_EODetails: ff.vmscrT_EODetails, vmscrT_LCDetails: ff.vmscrT_LCDetails, vmscrT_EOU180: ff.vmscrT_EOU180, vmscrT_LCU180: ff.vmscrT_LCU180, vmscrT_HREO: ff.vmscrT_HREO, vmscrT_HRLC: ff.vmscrT_HRLC, vmscrT_HRUL: ff.vmscrT_HRUL, vmscrT_HRDR: ff.vmscrT_HRDR, vmscrT_HRTLOP: ff.vmscrT_HRTLOP, vmscrT_Remarks: ff.vmscrT_Remarks, vmscrT_DOJ: ff.vmscrT_DOJ, vmscrT_TLOP: ff.vmscrT_TLOP, vmscrT_HDailyReport: ff.vmscrT_HDailyReport, vmscrT_HDRDetails: ff.vmscrT_HDRDetails, vmscrT_BDailyReport: ff.vmscrT_BDailyReport, vmscrT_BDRDetails: ff.vmscrT_BDRDetails };
                            temp_array1.push(newCol);
                            $scope.monthendreport = temp_array1;
                        });
                        
                        $scope.institutename = promise.institutename;
                        $scope.monthname = promise.monthname;
                        $scope.report = true;
                        if (promise.retrunMsg === 'False') {
                            swal("No Data Found !");
                            $scope.report = false;
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.sendmailMD = function () {
            var data = {
                "yearid": $scope.year,
                "months": $scope.month,
                "ConsolidateReportTempDTO": $scope.monthendreport
            };

            apiService.create("FrontOffice/SendFinalMailMD", data).
                then(function (promise) {
                    if (promise.retrunMsg === 'True') {
                        swal("Mail sent Successfully !");
                    }
                    else if (promise.retrunMsg === 'False') {
                        swal("Mail Not sent!");
                    }
                });
        };

        $scope.exptoex = function () {
            var divToPrint = document.getElementById("tablegrp");
            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            saveAs(blob, $scope.institutename + " – Attendance Report for Month of " +$scope.monthname+ " "+$scope.year + ".xls");
        };

        $scope.printData = function () {
            var innerContents = document.getElementById("tablegrp").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
    }
})();

