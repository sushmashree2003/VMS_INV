(function () {
    'use strict';
    angular
        .module('app')
        .controller('ConsolidatedSalaryReportController', ConsolidatedSalaryReportController)

    ConsolidatedSalaryReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'Excel', '$timeout', 'superCache']
    function ConsolidatedSalaryReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, Excel, $timeout, superCache) {
        //object

        //Employeee
        //$scope.currentPage = 1;
        //$scope.itemsPerPage = 10;
        $scope.employeeDetails = [];

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("ConsolidatedSalaryReport/getalldetails", pageid).then(function (promise) {

                if (promise.empnames !== null && promise.empnames.length > 0) {

                    $scope.employeedetils = promise.empnames;


                }
                $scope.fillmonth = promise.fillmonth;
                $scope.fillyear = promise.fillyear;
                $scope.hrmE_EmployeeFirstName = $scope.employeedetils[0].hrmE_EmployeeFirstName;
                $scope.hrmE_EmployeeCode = $scope.employeedetils[0].hrmE_EmployeeCode;
                $scope.hrmE_LocStreet = $scope.employeedetils[0].hrmE_LocStreet;
                $scope.hrmE_DOJ = $scope.employeedetils[0].hrmE_DOJ;
                $scope.hrmE_MobileNo = $scope.employeedetils[0].hrmE_MobileNo;
                $scope.hrmE_EmailId = $scope.employeedetils[0].hrmE_EmailId;
                $scope.hrmdeS_DesignationName = $scope.employeedetils[0].hrmdeS_DesignationName;
                $scope.hrmD_DepartmentName = $scope.employeedetils[0].hrmD_DepartmentName;
                $scope.imgname = $scope.employeedetils[0].hrmE_Photo;




            })
        }









        $scope.Modal = function () {
            $('#salarydetails').modal('show');
        }
        $scope.salaryreport = function () {
            var data = {
                "yearid": $scope.year,
                "month": $scope.month,
            };
            apiService.create("ConsolidatedSalaryReport/salaryreport", data).
                then(function (promise) {
                    if (promise.employeesalaryDetails != null && promise.employeesalaryDetails != undefined && promise.employeesalaryDetails.length > 0) {
                        $scope.employeesalaryDetails = promise.employeesalaryDetails;
                        $scope.employeeSalaryslipDetails = promise.employeeSalaryslipDetails;


                        $scope.earning = [];
                        $scope.deduction = [];
                        $scope.earning = $filter('filter')($scope.employeeSalaryslipDetails, function (d) {
                            return d.HRMED_EarnDedFlag == 'Earning';
                        });
                        $scope.deduction = $filter('filter')($scope.employeeSalaryslipDetails, function (d) {
                            return d.HRMED_EarnDedFlag == 'Deduction';
                        });

                        var earningtotal = 0;
                        for (var i = 0; i < $scope.earning.length; i++) {
                            earningtotal = earningtotal + $scope.earning[i].Amount;
                        }
                        var deductiontotal = 0;
                        for (var i = 0; i < $scope.deduction.length; i++) {
                            deductiontotal = deductiontotal + $scope.deduction[i].Amount;
                        }
                        $scope.earningttl = earningtotal;
                        $scope.deductionttl = deductiontotal;

                        $scope.netamount = earningtotal - deductiontotal;

                        $scope.Gross = $scope.employeesalaryDetails[0].Gross;
                        $scope.LopDays = $scope.employeesalaryDetails[0].LopDays;
                        $scope.HRES_WorkingDays = $scope.employeesalaryDetails[0].HRES_WorkingDays;

                    }
                    else {
                        swal("Salary Slip Not Generated");
                    }
                });
        }




    }
})();