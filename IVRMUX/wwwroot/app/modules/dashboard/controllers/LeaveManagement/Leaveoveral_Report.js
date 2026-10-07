
(function () {
    'use strict';
    angular
        .module('app')
        .controller('LeaveOverallReportController', LeaveOverallReportController);

    LeaveOverallReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout'];
    function LeaveOverallReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, Excel, $timeout) {

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

        $scope.page = "page";
       
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
                    }
                    else {
                        swal(" No Record Found !");
                    }
                });
        };


        $scope.viewfunction = function () {

            $scope.getReport = [];

        };


        $scope.submitted = false;
        $scope.GetReport = function () {

           
            var HRME_Id = 0;
            if ($scope.radioval == 'Employeewise') {
                if ($scope.chk_All === true) {
                    HRME_Id = 0;
                    $scope.hrmL_Id = 999; 
                    $scope.mI_Id = 0;
                }
                else {
                    HRME_Id = $scope.hrmE_Id.hrmE_Id;
                    $scope.mI_Id = 0;
                }
            }
          
            if ($scope.myForm.$valid) {

                $scope.submitted = true;

                var data = {
                    "HRML_Id": $scope.hrmL_Id,
                    "HRME_Id": HRME_Id,
                    "Institute_Id": $scope.mI_Id.mI_Id,
                    "FROM_DATE": new Date($scope.myDate_from).toDateString(),
                    "TO_DATE": new Date($scope.myDate_to).toDateString(),
                    "Rdotype": $scope.rdopunch,
                    "Radioflag": $scope.radioval
                }
                apiService.create("LeaveOverallReport/get_report", data).then(function (promise) {

                    if (promise.getReport.length !== 0) {
                        $scope.getReport = promise.getReport;
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


    }

})();
