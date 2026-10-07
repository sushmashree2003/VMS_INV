(function () {
    'use strict';
    angular.module('app').controller('PC_Expenditure_Balance_ReportController', PC_Expenditure_Balance_ReportController)

    PC_Expenditure_Balance_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$timeout', 'Excel']
    function PC_Expenditure_Balance_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $timeout, Excel) {

        $scope.submitted = false;
        $scope.indentapproveddetais = [];
        $scope.maxdate = new Date();

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";

            var pageid = 2;
            apiService.getURI("PC_Report/onloaddata", pageid).then(function (promise) {
                $scope.getuserinstitution = promise.getuserinstitution;
                $scope.MI_Id = promise.mI_Id;
            });
        };

        $scope.OnChangeExpenditureInstitution = function () {
            $scope.indentapproveddetais = [];
            $scope.getindenapprovedreportdetails = [];
        };

        $scope.onchangefromdate = function () {
            $scope.PCINDENT_Date_To = null;
            $scope.indentapproveddetais = [];
            $scope.getindenapprovedreportdetails = [];
        };

        $scope.onchangedate = function () {
            $scope.indentapproveddetais = [];
            $scope.getindenapprovedreportdetails = [];
        };

        $scope.getreport = function (PCINDENT_Idss) {
            if ($scope.myForm.$valid) {
                $scope.indentapproveddetais = [];
                $scope.getindenapprovedreportdetails = [];
                $scope.getexpendaturebalancereport = [];

                var data = {
                    "MI_Id": $scope.MI_Id,
                    "Fromdate": new Date($scope.PCINDENT_Date_From).toDateString(),
                    "Todate": new Date($scope.PCINDENT_Date_To).toDateString()
                };
                apiService.create("PC_Report/getexpenditurebalancereports", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.getexpendaturebalancereport = promise.getexpendaturebalancereport;
                        if ($scope.getexpendaturebalancereport != null && $scope.getexpendaturebalancereport.length > 0) {

                            $scope.fromdated = new Date($scope.PCINDENT_Date_From).toDateString();
                            $scope.todated = new Date($scope.PCINDENT_Date_To).toDateString();
                            $scope.institutiondetails = promise.institutiondetails;
                            $scope.imagenew = $scope.institutiondetails[0].mI_Logo;
                            $scope.institutioname = $scope.institutiondetails[0].mI_Name;
                            //$scope.employee = $scope.getexpendaturebalancereport[0].employeename;

                            $scope.employeeid = [];
                            var total_Amount = 0;
                            var Balance = 0;
                            angular.forEach($scope.getexpendaturebalancereport, function (dev) {
                                if ($scope.employeeid.length === 0) {
                                    $scope.employeeid.push({ PCINDENT_Id: dev.PCINDENT_Id, PCINDENTAPT_IndentNo: dev.PCINDENTAPT_IndentNo, PCINDENTAPT_SanctionedAmt: dev.PCINDENTAPT_SanctionedAmt, total_Amount: total_Amount, Balance: Balance });
                                }
                                else if ($scope.employeeid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.employeeid, function (emp) {
                                        if (emp.PCINDENT_Id === dev.PCINDENT_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        total_Amount = 0;
                                        Balance = 0;
                                        $scope.employeeid.push({
                                            PCINDENT_Id: dev.PCINDENT_Id,
                                            PCINDENTAPT_IndentNo: dev.PCINDENTAPT_IndentNo,
                                            PCINDENTAPT_SanctionedAmt: dev.PCINDENTAPT_SanctionedAmt, total_Amount: total_Amount,
                                            Balance: Balance
                                        });
                                    }
                                }
                            });
                            console.log($scope.employeeid);

                            angular.forEach($scope.employeeid, function (ddd) {
                                $scope.templist = [];
                                total_Amount = 0;
                                Balance = 0;
                                $scope.balanceflag = "Balance";
                                angular.forEach($scope.getexpendaturebalancereport, function (dd) {
                                    if (dd.PCINDENT_Id === ddd.PCINDENT_Id) {
                                        total_Amount += dd.PCEXPTR_Amount;
                                        $scope.templist.push(dd);
                                    }
                                });

                                //Excess Amount
                                if (total_Amount > ddd.PCINDENTAPT_SanctionedAmt) {
                                    Balance = (total_Amount - ddd.PCINDENTAPT_SanctionedAmt);
                                    $scope.balanceflag = "Excess";
                                }
                                //Balance Amount
                                else {
                                    Balance = (ddd.PCINDENTAPT_SanctionedAmt - total_Amount);
                                }
                                ddd.total_Amount = total_Amount;
                                ddd.Balance = Balance;
                                ddd.balanceflag = $scope.balanceflag;
                                ddd.plannerdetails = $scope.templist;
                            });
                            console.log($scope.employeeid);
                        }
                        else {
                            swal("No Record  Found..... !!");
                            $scope.getexpendaturebalancereport = [];
                        }
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.exportToExceldetails = function () {

            var excelname = 'Expenditure Details Report';
            excelname = excelname.toUpperCase() + '.xls';
            var exportHref = Excel.tableToExcel('#exceltopd', 'Expenditure Details Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };
        //$scope.exportToExcel = function () {
        //        var exportHref = Excel.tableToExcel(tablegrp, 'sheet name');
        //        $timeout(function () { location.href = exportHref; }, 100);
        //};

        $scope.printData = function () {

            var innerContents = document.getElementById("tablegrp").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };


        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();
