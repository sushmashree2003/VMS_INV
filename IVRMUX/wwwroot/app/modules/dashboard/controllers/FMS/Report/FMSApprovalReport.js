(function () {
    'use strict';
    angular.module('app').controller('FMSApprovalReportController', FMSApprovalReportController)

    FMSApprovalReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$timeout', 'Excel']
    function FMSApprovalReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $timeout, Excel) {



        $scope.submitted = false;
        $scope.indentapproveddetais = [];
        $scope.maxdate = new Date();

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        var temp = [];
        var year = "";
        $scope.yearfromdate = "";
        $scope.monthlist_temp = [];
        $scope.consolidata_id = [];
        $scope.get_deviationreport = [];
        $scope.imgname = logopath;
        $scope.Fromdate = new Date();
        $scope.Todate = new Date();
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";


            var pageid = 2;
            apiService.getURI("FMSApprovalReport/loaddata", pageid).then(function (promise) {

                $scope.filename = promise.filename;
                $scope.institution = promise.institution;

                if (promise.get_tax != null && promise.get_tax.length > 0) {
                    $scope.get_tax = promise.get_tax;
                    $scope.registration = promise.registration;
                }


            });
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.all_check = function () {
            var toggleStatus = $scope.checkallm;
            angular.forEach($scope.institution, function (itm) {
                itm.selected = toggleStatus;
            });
        }

        $scope.optionToggled1 = function (x) {
            $scope.checkallm = $scope.institution.every(function (itm) { return itm.selected; })

        }


        $scope.all_check1 = function () {
            var toggleStatus = $scope.checkallm1;
            angular.forEach($scope.filename, function (itm) {
                itm.selected1 = toggleStatus;
            });
        }

        $scope.optionToggled2 = function (x) {
            $scope.checkallm1 = $scope.filename.every(function (itm) { return itm.selected1; })

        }



        $scope.selected_Inst = [];
        $scope.approveddetails = [];
        $scope.get_Report = function () {


            angular.forEach($scope.institution, function (role) {
                if (role.selected == true) {
                    $scope.selected_Inst.push({
                        Id: role.mI_Id
                    });
                }
            })

            angular.forEach($scope.filename, function (role) {
                if (role.selected1 == true) {
                    $scope.approveddetails.push({
                        fnid: role.fmsmpfN_Id
                    });
                }
            })

            
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.flag == 1) {
                    $scope.flag = 1;
                } else if ($scope.flag == 0) {
                    $scope.flag = 0
                } else {
                    swal('Select one of the Status Flag');
                }

                $scope.from_date = new Date($scope.Fromdate).toDateString();
                $scope.to_date = new Date($scope.Todate).toDateString();

                var data = {
                    "grid_mid": $scope.selected_Inst,
                    "grid_arraydatas": $scope.approveddetails,
                    "fromdate": $scope.from_date,
                    "todate": $scope.to_date,
                    "FMSFLREQ_ApprovedFlg": $scope.flag
                }

                apiService.create("FMSApprovalReport/report", data).then(function (promise) {
                    if (promise.griddata != null && promise.griddata.length > 0) {
                        $scope.DisplayApprovalTable = promise.griddata;
                        $scope.physicalfilename = $scope.DisplayApprovalTable[0].physicalname;
                        $scope.presentCountgrid = $scope.DisplayApprovalTable.length;


                        $scope.employeeid = [];

                        angular.forEach($scope.DisplayApprovalTable, function (dev) {
                            if ($scope.employeeid.length === 0) {
                                $scope.employeeid.push({
                                    fmsflreQ_Id: dev.fmsflreQ_Id,
                                    physicalname: dev.physicalname,
                                    createdby: dev.createdby

                                });
                            } else if ($scope.employeeid.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.employeeid, function (emp) {
                                    if (emp.fmsflreQ_Id === dev.fmsflreQ_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.employeeid.push({
                                        fmsflreQ_Id: dev.fmsflreQ_Id, physicalname: dev.physicalname,
                                        createdby: dev.createdby

                                    });
                                }
                            }
                        });

                        angular.forEach($scope.employeeid, function (ddd) {
                            $scope.templist = [];
                            angular.forEach($scope.DisplayApprovalTable, function (dd) {
                                if (dd.fmsflreQ_Id === ddd.fmsflreQ_Id) {
                                    $scope.templist.push(dd);
                                }
                            });
                            ddd.plannerdetails = $scope.templist;
                        });




                    }

                    else {
                        swal("No Record  Found..... !!");

                    }

                });

            }
            
                else {
                    $scope.submitted = true;
                }

        }



        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printDeviation").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };        $scope.exportToExceldetails = function (export_id) {
            var exportHref = Excel.tableToExcel(export_id, 'Salary Approval Report');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };


        $scope.Clearid = function () {
            $state.reload();
        };


    }
 })();