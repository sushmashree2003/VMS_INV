(function () {
    'use strict';
    angular.module('app').controller('PC_Indent_ReportController', PC_Indent_ReportController)

    PC_Indent_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$timeout', 'Excel']
    function PC_Indent_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $timeout, Excel) {

        $scope.submitted = false;
        $scope.totalgrid = [];
        $scope.PCREQTN_Date = new Date();
        $scope.PCINDENT_Date = new Date();
        $scope.editflag = false;
        $scope.maxdate = new Date();
        $scope.obj = {};
        $scope.indentdetails = [];

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
            $scope.totalgrid = [];
            $scope.totalgrid = [{ id: 'pcindent1' }];
            $scope.indentdetails = [];
            $scope.indentparticulardetais = [];
            $scope.geteditparticularsdata = [];
            $scope.geteditdata = [];
            $scope.obj.all2 = false;
            $scope.obj.all21 = false;
            $scope.obj.all22 = false;

            $scope.PCINDENT_Id = 0;
            $scope.editflag = false;
        };

        $scope.onchangedate = function () {
            $scope.totalgrid = [];
            $scope.totalgrid = [{ id: 'pcindent1' }];
            $scope.indentdetails = [];
            $scope.indentparticulardetais = [];
            $scope.geteditparticularsdata = [];
            $scope.geteditdata = [];
            $scope.PCINDENT_Id = 0;
            $scope.obj.all2 = false;
            $scope.obj.all22 = false;
            $scope.obj.all21 = false;
             
        };

        $scope.getreport = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "MI_Id": $scope.MI_Id,
                    "Fromdate": new Date($scope.PCINDENT_Date_From).toDateString(),
                    "Todate": new Date($scope.PCINDENT_Date_To).toDateString()
                };
                apiService.create("PC_Report/getindentreport", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.getindentreportdetails = promise.getindentreportdetails;
                        if ($scope.getindentreportdetails !== null && $scope.getindentreportdetails.length > 0) {
                            $scope.indentdetails = $scope.getindentreportdetails;
                            $scope.indentparticulardetais = [];
                            $scope.fromdated = new Date($scope.PCINDENT_Date_From).toDateString();
                            $scope.todated = new Date($scope.PCINDENT_Date_To).toDateString();
                            $scope.institutiondetails = promise.institutiondetails;
                            $scope.imagenew = $scope.institutiondetails[0].mI_Logo;
                            $scope.institutioname = $scope.institutiondetails[0].mI_Name;
                            console.log($scope.indentdetails);

                            $scope.indentid = [];

                            angular.forEach($scope.indentdetails, function (dev) {
                                if ($scope.indentid.length === 0) {
                                    $scope.indentid.push({
                                        PCINDENT_Id: dev.PCINDENT_Id,
                                        PCINDENT_IndentNo: dev.PCINDENT_IndentNo,
                                        EMPLOYEENAME: dev.EMPLOYEENAME,
                                        REQDATE: dev.REQDATE,
                                        REQTOTALAMOUNT: dev.REQTOTALAMOUNT,
                                        SANCAMT: dev.SANCAMT
                                    });
                                } else if ($scope.indentid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.indentid, function (emp) {
                                        if (emp.PCINDENT_Id === dev.PCINDENT_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.indentid.push({
                                            PCINDENT_Id: dev.PCINDENT_Id,
                                            PCINDENT_IndentNo: dev.PCINDENT_IndentNo,
                                            EMPLOYEENAME: dev.EMPLOYEENAME,
                                            REQDATE: dev.REQDATE,
                                            REQTOTALAMOUNT: dev.REQTOTALAMOUNT,
                                            SANCAMT: dev.SANCAMT
                                        });
                                    }
                                }
                            });

                            console.log($scope.indentid);

                            angular.forEach($scope.indentid, function (ddd) {
                                $scope.templist = [];
                                angular.forEach($scope.indentdetails, function (dd) {
                                    if (dd.PCINDENT_Id === ddd.PCINDENT_Id) {
                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.templistdata = $scope.templist;
                            });

                            console.log($scope.indentid);

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

        $scope.Print = function () {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            //popupWinindow.document.write('<html><head>' +
            //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
            //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
            //    '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
            //    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            //);
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 10000);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExceldetails = function () {

            var excelname = 'Indent Details Report';
            excelname = excelname.toUpperCase() + '.xls';           
            var exportHref = Excel.tableToExcel('#exceltopd', 'Indent Details Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();
