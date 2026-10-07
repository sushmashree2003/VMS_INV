(function () {
    'use strict';
    angular
        .module('app')
        .controller('SalarySlipController', SalarySlipController)

    SalarySlipController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function SalarySlipController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object


        $scope.EmployeeDis = false;
        $scope.Emp = {};
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.employeeLeaveDetails = [];

        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("SalarySlip/getalldetails", pageid).then(function (promise) {
                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {
                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }

                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;
                }
            })
        }


        //GetEmployeeDetailsByLeaveYearAndMonth
        $scope.submitted = false;
        $scope.GetEmployeeList = function () {

            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.showbutton = false;
            $scope.employeedropdown = [];
            $scope.Emp = {};
            if ($scope.Employee.hreS_Year != "" && $scope.Employee.hreS_Year != undefined && $scope.Employee.hreS_Month != "" && $scope.Employee.hreS_Month != undefined) {


                var data = {
                    "HRES_Year": $scope.Employee.hreS_Year,
                    "HRES_Month": $scope.Employee.hreS_Month,
                }

                apiService.create("SalarySlip/GetEmployeeDetailsByLeaveYearAndMonth", data).
                    then(function (promise) {

                        if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                            $scope.employeedropdown = promise.employeedropdown;
                            $scope.employeedropdown_mail = promise.employeedropdown;
                        }
                        else {
                            $scope.employeedropdown = [];
                            $scope.employeedropdown_mail = [];

                            swal("Employees salary Not generated for " + $scope.Employee.hreS_Month + "-" + $scope.Employee.hreS_Year);
                            $scope.showbutton = true;
                            $scope.EmployeeDis = false;
                        }
                    })
            }
        }

        $scope.submitted = false;
        $scope.employeeSalaryslipDetails = [];
        $scope.institutionDetails = {};
        $scope.submitted = false;
        $scope.GenerateSalarySlip = function () {

            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.esary = [];
            $scope.dsary = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.institutionDetails = {};
                $scope.earningDetails = [];
                $scope.deductionDetails = [];
                $scope.esary = [];
                $scope.dsary = [];
                $scope.items = [];

                var data =
                {
                    "HRES_Year": $scope.Employee.hreS_Year,
                    "HRES_Month": $scope.Employee.hreS_Month,
                };

                apiService.create("SalarySlip/GenerateEmployeeSalarySlip", data).
                    then(function (promise) {
                        if (promise.main_list.length > 0 && promise.main_list != null) {

                            for (var i = 0; i < promise.main_list.length; i++) {
                                if (promise.main_list[i].employeeSalaryslipDetails != null && promise.main_list[i].employeeSalaryslipDetails.length != 0) {

                                    var items = promise.main_list[i].employeeSalaryslipDetails;

                                    $scope.hrmE_CTC = promise.main_list[i].currentemployeeDetails.hrmE_CTC;

                                    var getPortion = function (label) {
                                        var sum = 0;
                                        var out = items.filter(function (x) {
                                            var match = x.HRMED_EarnDedFlag == label;
                                            if (match)
                                                sum += x.Amount;
                                            return match;
                                        });
                                        return { out, sum }
                                    };





                                    var es = getPortion('Earning');
                                    var ds = getPortion('Deduction');
                                    promise.main_list[i].esary = es;
                                    promise.main_list[i].dsary = ds;
                                    if (es.out.length >= ds.out.length) {
                                        for (var item of ds.out)
                                            es.out[ds.out.indexOf(item)].ds = item;
                                        promise.main_list[i].items = es.out;

                                    } else {
                                        for (var item of es.out)
                                            ds.out[es.out.indexOf(item)].ds = item;
                                        promise.main_list[i].items = ds.out;
                                    }



                                    promise.main_list[i].totals = [es.sum, ds.sum];

                                    var LopAmount = 0;
                                    if (promise.main_list[i].payrollStandard.hrC_PayMethodFlg == "Method1") {
                                        LopAmount = promise.main_list[i].lopAmount;
                                    }
                                    else {
                                        LopAmount = 0;
                                    }
                                    //Employee Salary Details
                                    promise.main_list[i].empsaldetail = promise.main_list[i].empsaldetail;

                                    promise.main_list[i].NetSalary = Math.round((Math.ceil(parseFloat(promise.main_list[i].totals[0]) - parseFloat(promise.main_list[i].totals[1]))) - LopAmount);

                                    promise.main_list[i].NetAmountInwords = toWords(promise.main_list[i].NetSalary) + 'Rupees only.';

                                    if (promise.main_list[i].institutionDetails != null) {


                                        promise.main_list[i].selectedMonth = promise.main_list[i].hreS_Month;
                                        promise.main_list[i].selectedYear = promise.main_list[i].hreS_Year;

                                        promise.main_list[i].institutionDetails = promise.main_list[i].institutionDetails;

                                        if (promise.main_list[i].institutionDetails.mI_Logo == "" || promise.main_list[i].institutionDetails.mI_Logo == null) {
                                            promise.main_list[i].institutionDetails.mI_Logo = "../../../../../../images/uploads/studentDocumnets/2/36ed82c1-b373-4358-af12-4397bacda396.jpg";
                                        }
                                        var instuteAddress = "";
                                        if (promise.main_list[i].institutionDetails.mI_Address1 != null && promise.main_list[i].institutionDetails.mI_Address1 != "") {
                                            instuteAddress = promise.main_list[i].institutionDetails.mI_Address1;
                                        }
                                        if (promise.main_list[i].institutionDetails.mI_Address2 != null && promise.main_list[i].institutionDetails.mI_Address2 != "") {

                                            instuteAddress = instuteAddress + ',' + promise.main_list[i].institutionDetails.mI_Address2;
                                        }

                                        if (promise.main_list[i].institutionDetails.mI_Address3 != null && promise.main_list[i].institutionDetails.mI_Address3 != "") {

                                            instuteAddress = instuteAddress + ',' + promise.main_list[i].institutionDetails.mI_Address3;
                                        }
                                        promise.main_list[i].CurrentInstuteAddress = instuteAddress;
                                    }
                                    $scope.all_employees = promise.main_list;
                                    $scope.EmployeeDis = true;

                                }
                                else {
                                    swal("Salary Not Generated")
                                }

                            }

                        }
                        else {
                            swal("Salary Not Generated")
                        }

                        if (promise.employeeLeaveDetails != null && promise.employeeLeaveDetails != "") {

                            $scope.employeeLeaveDetails = promise.employeeLeaveDetails;
                        }
                        if (promise.companydetails != null && promise.companydetails != "") {
                            $scope.lopDetails = promise.companydetails[0].vmscrT_Remarks;
                        }

                    })
            }

        }


        $scope.Employee = {};
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.submitted = false;
            $scope.groupTypeselectedAll = false;
            $scope.departmentselectedAll = false;
            $scope.designationselectedAll = false;


            if ($scope.EmployeeDis == true) {
                $scope.EmployeeDis = false;
            }
            $scope.search = "";
            $scope.Emp = {};
            $scope.employeeLeaveDetails = [];
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.onLoadGetData();
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        var th = ['', 'thousand', 'million', 'billion', 'trillion'];
        var dg = ['Zero', 'One', 'Two', 'Three', 'Four', 'Five', 'Six', 'Seven', 'Eight', 'Nine'];
        var tn = ['Ten', 'Eleven', 'Twelve', 'Thirteen', 'Fourteen', 'Fifteen', 'Sixteen', 'Seventeen', 'Eighteen', 'Nineteen'];
        var tw = ['Twenty', 'Thirty', 'Forty', 'Fifty', 'Sixty', 'Seventy', 'Eighty', 'Ninety'];


        function toWords(s) {
            s = s.toString();
            s = s.replace(/[\, ]/g, '');
            if (s != parseFloat(s)) return 'not a number';
            var x = s.indexOf('.');
            if (x == -1) x = s.length;
            if (x > 15) return 'too big';
            var n = s.split('');
            var str = '';
            var sk = 0;
            for (var i = 0; i < x; i++) {
                if ((x - i) % 3 == 2) {
                    if (n[i] == '1') {
                        str += tn[Number(n[i + 1])] + ' ';
                        i++;
                        sk = 1;
                    }
                    else if (n[i] != 0) {
                        str += tw[n[i] - 2] + ' ';
                        sk = 1;
                    }
                }
                else if (n[i] != 0) {
                    str += dg[n[i]] + ' ';
                    if ((x - i) % 3 == 0) str += 'hundred ';
                    sk = 1;
                }


                if ((x - i) % 3 == 1) {
                    if (sk) str += th[(x - i - 1) / 3] + ' ';
                    sk = 0;
                }
            }
            if (x != s.length) {
                var y = s.length;
                str += 'point ';
                for (var i = x + 1; i < y; i++) str += dg[n[i]] + ' ';
            }
            return str.replace(/\s+/g, ' ');
        }

        $scope.printData = function (Employee) {
            var innerContents = document.getElementById("EmpPaySlip").innerHTML;
            var popupWinindow = window.open('about:blank', '_blank');

            popupWinindow.document.open();
            popupWinindow.document.write(`<html><head>
            <title>Salary Slip</title>
            <link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />
            <link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/EmpPaySlipPdf.css" />
            <link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" /></head>
            <body onload="window.print();" onfocus="window.setTimeout(function() { window.close(); }, 100);">
            ${innerContents}</body></html>`);
            popupWinindow.document.close();
        }
    }


})();