(function () {
    'use strict';
    angular.module('app').controller('PC_Expenditure_Indent_ReportController', PC_Expenditure_Indent_ReportController)

    PC_Expenditure_Indent_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$timeout', 'Excel']
    function PC_Expenditure_Indent_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $timeout, Excel) {
        $scope.submitted = false;
        $scope.indentapproveddetais = [];
        $scope.maxdate = new Date();

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;       

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

        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";

            var pageid = 2;
            apiService.getURI("PC_Report/onloaddatabalancesheet", pageid).then(function (promise) {
                $scope.getuserinstitution = promise.getuserinstitution;
                $scope.MI_Id = promise.mI_Id;
                $scope.indentdetails = promise.indentdetails;
            });
        };

        $scope.OnChangeBalanceSheetInstitution = function () {
            $scope.indentapproveddetais = [];
            $scope.getindenapprovedreportdetails = [];
            $scope.indentdetails = [];
            $scope.PCINDENT_Id = "";

            var data = {
                "MI_Id": $scope.MI_Id
            }
            apiService.create("PC_Report/OnChangeBalanceSheetInstitution", data).then(function (promise) {
                $scope.indentdetails = promise.indentdetails;
            });
        };


        $scope.getreport = function (PCINDENT_Idss) {
            if ($scope.myForm.$valid) {
                $scope.indentapproveddetais = [];
                $scope.getindenapprovedreportdetails = [];

                var data = {
                    "MI_Id": $scope.MI_Id,
                    "PCINDENT_Id": $scope.PCINDENT_Id
                };
                apiService.create("PC_Report/getbalancesheetreports", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.getindenapprovedreportdetails = promise.getindenapprovedreportdetails;
                        if ($scope.getindenapprovedreportdetails !== null && $scope.getindenapprovedreportdetails.length > 0) {

                            $scope.indentapproveddetais = $scope.getindenapprovedreportdetails;
                            $scope.totalexpenditure = 0.00;

                            angular.forEach($scope.indentapproveddetais, function (dd) {
                                $scope.totalexpenditure += dd.PCEXPTR_Amount;
                            });

                            $scope.getindentdetails = promise.getindentdetails;
                            $scope.sanc_amt = $scope.getindentdetails[0].pcindentapT_SanctionedAmt;
                            $scope.balance_amt = $scope.sanc_amt - $scope.totalexpenditure;

                            if ($scope.balance_amt > 0) {
                                $scope.labelname = "Balance";
                            } else {
                                $scope.labelname = "Due";
                            }

                            angular.forEach($scope.indentdetails, function (dd) {
                                if (dd.pcindenT_Id === parseInt($scope.PCINDENT_Id)) {
                                    $scope.indentno = dd.pcindenT_IndentNo;
                                }
                            });

                            $scope.date = new Date($scope.today);
                            $scope.institutiondetails = promise.institutiondetails;
                            $scope.imagenew = $scope.institutiondetails[0].mI_Logo;
                            $scope.institutioname = $scope.institutiondetails[0].mI_Name;

                            console.log($scope.indentapproveddetais);
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

            var excelname = 'Indent Wise Expenditure Report';
            excelname = excelname.toUpperCase() + '.xls';
            var exportHref = Excel.tableToExcel('#exceltopd', 'Indent Wise Expenditure Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.Print = function () {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        };

        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();
