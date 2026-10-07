
(function () {
    'use strict';
    angular
        .module('app')
        .controller('LeaveMonthEndReportController', LeaveMonthEndReportController);

    LeaveMonthEndReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout'];
    function LeaveMonthEndReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, Excel, $timeout) {

        $scope.currentPage = 1;

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        //$scope.page = "page";

        $scope.search = '';
        $scope.grid_view = false;
        $scope.chk_All = true;
        $scope.DisaBleemp = true;
        $scope.loadData = function () {
            var id = 2;

            apiService.getURI("LeaveOverallReport/getleavereport", id).
                then(function (promise) {
                 
                    $scope.leave_name = promise.leave_name;
                    $scope.get_emp = promise.get_emp;
                    $scope.institutionlist = promise.institutionlist;
                

                });
        };


        //Onchange Institution, Leavetype Should Load

        $scope.getleavetype = function (companyid) {
            var data = {
                "MI_Id": companyid.mI_Id,
            };
            apiService.create("LeaveOverallReport/getleavetype", data).
                then(function (promise) {
                    if (promise.leave_name !== null && promise.leave_name.length > 0) {
                        $scope.leave_name = promise.leave_name;
                        $scope.getmonth = promise.getmonth;
                        $scope.getyear = promise.getyear;
                    } 
                    else {
                        swal(" No Record Found !");
                    }
                });
        };


        $scope.viewfunction = function () {

            $scope.getmonthReport = [];

        };


        $scope.submitted = false;
        $scope.getReport = function () { 
            if ($scope.myForm.$valid) {

                $scope.submitted = true;

                var data = {
                    "HRML_Id": $scope.hrmL_Id,                   
                    "Institute_Id": $scope.mI_Id.mI_Id,                    
                    "year": $scope.year,
                    "IVRM_Month_Id": $scope.ivrM_Month_Id,
                    "Rdotype": $scope.rdopunch,                   
                }
                apiService.create("LeaveOverallReport/getmonthendreport", data).then(function (promise) {

                    if (promise.getmonthReport.length !== 0) {
                        $scope.getmonthReport = promise.getmonthReport;
                        $scope.grid_view = true;                    
                    }
                    else {
                        swal("Record not found.");
                        $scope.grid_view = false;
                    }
                    if (promise === "") {
                        swal("please contact administrator !");
                    }
                });
            }


            else {
                $scope.submitted = false;
            }
        }
        //// to sum a leavecount
        $scope.sumofleave = function () {
            var total = 0;
            if ($scope.getmonthReport != null) {
                for (var i = 0; i < $scope.getmonthReport.length; i++) {
                    var add = $scope.getmonthReport[i];
                    total += add.leave;
                }
            }

            return total;
        }
        //check Function
        $scope.All_Chk = function () {
            if ($scope.chk_All === true) {
                $scope.DisaBleemp = true;
            }
            else {
                $scope.DisaBleemp = false;

            }
        }


        //validation start
        $scope.interacted = function (field) {

            return $scope.submitted;
        };


        //print start
        $scope.printData = function () {
            var id = "table1";

            var innerContents = document.getElementById(id).innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        //print end
        //export start

        $scope.exptoex = function () {
            var divToPrint = document.getElementById("table1");
            var exportHref = Excel.tableToExcel(divToPrint, divToPrint);
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };

        //export end
        //TO clear  data
        $scope.clearid = function () {
            $scope.submitted = false;
            $state.reload();
        };
        //clear end
        //$scope.getchanged = function (yr, mon) {
        //    $scope.report = false;
        //    $scope.monthname = "";
        //    angular.forEach($scope.fillmonth, function (check) {
        //        if (check.ivrM_Month_Id == mon) {
        //            $scope.monthname = check.monthname;
        //        }
        //    });
        //    $scope.yearname = "";
        //    angular.forEach($scope.fillyear, function (check) {
        //        if (check.hrmlY_LeaveYear == yr) {
        //            $scope.yearname = check.hrmlY_LeaveYear;
        //        }
        //    });
        //};

    }

})();
