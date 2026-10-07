(function () {
    'use strict';
    angular.module('app').controller('PC_Requisition_ReportController', PC_Requisition_ReportController)

    PC_Requisition_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$timeout', 'Excel']
    function PC_Requisition_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $timeout, Excel) {

        $scope.submitted = false;      
        $scope.maxdate = new Date();   
        $scope.requisitiondetais = [];

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
            $scope.getrequisitionreportdetails = [];
            $scope.requisitiondetais = [];
        };

        $scope.onchangefromdate = function () {
            $scope.PCINDENT_Date_To = null;
            $scope.getrequisitionreportdetails = [];
            $scope.requisitiondetais = [];
        };

        $scope.onchangedate = function () {           
            $scope.requisitiondetais = [];
            $scope.getrequisitionreportdetails = [];
          
        };

        $scope.getreport = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "MI_Id": $scope.MI_Id,
                    "Fromdate": new Date($scope.PCINDENT_Date_From).toDateString(),
                    "Todate": new Date($scope.PCINDENT_Date_To).toDateString()
                };
                apiService.create("PC_Report/getrequisitionreport", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.getrequisitionreportdetails = promise.getrequisitionreportdetails;
                        if ($scope.getrequisitionreportdetails !== null && $scope.getrequisitionreportdetails.length > 0) {
                            $scope.requisitiondetais = $scope.getrequisitionreportdetails;
                            $scope.fromdated = new Date($scope.PCINDENT_Date_From).toDateString();
                            $scope.todated = new Date($scope.PCINDENT_Date_To).toDateString();
                            $scope.institutiondetails = promise.institutiondetails;
                            $scope.imagenew = $scope.institutiondetails[0].mI_Logo;
                            $scope.institutioname = $scope.institutiondetails[0].mI_Name;
                            console.log($scope.requisitiondetais);


                            $scope.requisitionid = [];

                            angular.forEach($scope.getrequisitionreportdetails, function (dev) {
                                if ($scope.requisitionid.length === 0) {
                                    $scope.requisitionid.push({
                                        PCREQTN_Id: dev.PCREQTN_Id,
                                        PCREQTN_RequisitionNo: dev.PCREQTN_RequisitionNo,
                                        EMPLOYEENAME: dev.EMPLOYEENAME,
                                        REQDATE: dev.REQDATE,
                                        TOTALAMOUNT: dev.TOTALAMOUNT
                                    });
                                } else if ($scope.requisitionid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.requisitionid, function (emp) {
                                        if (emp.PCREQTN_Id === dev.PCREQTN_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.requisitionid.push({
                                            PCREQTN_Id: dev.PCREQTN_Id,
                                            PCREQTN_RequisitionNo: dev.PCREQTN_RequisitionNo,
                                            EMPLOYEENAME: dev.EMPLOYEENAME,
                                            REQDATE: dev.REQDATE,
                                            TOTALAMOUNT: dev.TOTALAMOUNT
                                        });
                                    }
                                }
                            });

                            console.log($scope.requisitionid);

                            angular.forEach($scope.requisitionid, function (ddd) {
                                $scope.templist = [];
                                angular.forEach($scope.getrequisitionreportdetails, function (dd) {
                                    if (dd.PCREQTN_Id === ddd.PCREQTN_Id) {
                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.templistdata = $scope.templist;
                            });

                            console.log($scope.requisitionid);

                        }
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.Print = function () {

            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExceldetails = function () {

            var excelname = 'Requisition Details Report';
            excelname = excelname.toUpperCase() + '.xls';
            var exportHref = Excel.tableToExcel('#exceltopd', 'Requisition Details Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };


        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.Clearid = function () {
            $state.reload();
        };
       
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

    }
})();