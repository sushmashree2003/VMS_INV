(function () {
    'use strict';
    angular
        .module('app')
        .controller('IVRM_VersionReportController', IVRM_VersionReportController)

    IVRM_VersionReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce', 'Excel', '$timeout']
    function IVRM_VersionReportController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce, Excel, $timeout) {
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 10;
        $scope.submitted = false;
        $scope.setremind = false;
        $scope.disabledata = false;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("IVRM_VersionControlProcess/ReportOnload", pageid).then(function (promise) {
                //$scope.versionlist = promise.versionlist;
                $scope.clientHosting = promise.clientHosting;


            });
        };
        $scope.getversionlistforhost = function () {
            $scope.versionlist = [];
            var data = {
                "IVRMCH_Id": $scope.obj.ivrmcH_Id.ivrmcH_Id,
                "flag": "gethostversions"
            }
            apiService.create("IVRM_VersionControlProcess/GetReport", data).then(function (promise) {
                $scope.versionlist = promise.versionlist;
            });
        };

        $scope.cancel = function () {
            $state.reload();
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.GetReport = function () {
            if ($scope.myForm.$valid) {
                angular.forEach($scope.versionlist, function (dev) {
                    if (dev.ivrmvT_Id == $scope.obj.ivrmvT_Id.ivrmvT_Id) {
                        $scope.currentversion = dev.ivrmvT_VersionName;
                    }

                });
                angular.forEach($scope.clientHosting, function (x) {
                    if (x.ivrmcH_Id == $scope.obj.ivrmcH_Id.ivrmcH_Id) {
                        $scope.hostname = x.ivrmcH_HostName;
                    }

                });

                var ivrmcH_Id = 0;
                var ivrmvT_Id = 0;
                //if ($scope.obj.ivrmcH_Id != undefined && $scope.obj.ivrmcH_Id.ivrmcH_Id > 0) {
                //    ivrmcH_Id = $scope.obj.ivrmcH_Id.ivrmcH_Id
                //}
                //else if ($scope.obj.ivrmvT_Id != undefined && $scope.obj.ivrmvT_Id.ivrmvT_Id > 0) {
                //    ivrmvT_Id = $scope.obj.ivrmvT_Id.ivrmvT_Id
                //}

                var data = {
                    "IVRMCH_Id": $scope.obj.ivrmcH_Id.ivrmcH_Id,
                    "IVRMVT_Id": $scope.obj.ivrmvT_Id.ivrmvT_Id,
                    // "Fromdate": $scope.Fromdate,
                    // "Todate": $scope.Todate
                };
                apiService.create("IVRM_VersionControlProcess/GetReport", data).then(function (promise) {
                    if (promise.getgriddata.length > 0) {
                        $scope.grid_view = true;
                        $scope.griddata = promise.getgriddata;
                        $scope.presentCountgrid = $scope.griddata.length;

                        $scope.employeeid = [];

                        if ($scope.radiomodal == "hostingwise") {

                            angular.forEach($scope.griddata, function (dev) {
                                if ($scope.employeeid.length === 0) {
                                    $scope.employeeid.push({
                                        IVRMVT_Id: dev.IVRMVT_Id,
                                        IVRMVT_VersionName: dev.IVRMVT_VersionName,
                                    });
                                } else if ($scope.employeeid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.employeeid, function (emp) {
                                        if (emp.IVRMVT_Id === dev.IVRMVT_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.employeeid.push({
                                            IVRMVT_Id: dev.IVRMVT_Id, IVRMVT_VersionName: dev.IVRMVT_VersionName

                                        });
                                    }
                                }
                            });

                            angular.forEach($scope.employeeid, function (ddd) {
                                $scope.templist = [];
                                angular.forEach($scope.griddata, function (dd) {
                                    if (dd.IVRMVT_Id === ddd.IVRMVT_Id) {
                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.griddatadetails = $scope.templist;
                            });


                        }
                        else {

                            angular.forEach($scope.griddata, function (dev) {
                                if ($scope.employeeid.length === 0) {
                                    $scope.employeeid.push({
                                        IVRMCH_Id: dev.IVRMCH_Id,
                                        IVRMCH_HostName: dev.IVRMCH_HostName,
                                    });
                                } else if ($scope.employeeid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.employeeid, function (emp) {
                                        if (emp.IVRMCH_Id === dev.IVRMCH_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.employeeid.push({
                                            IVRMCH_Id: dev.IVRMCH_Id, IVRMCH_HostName: dev.IVRMCH_HostName

                                        });
                                    }
                                }
                            });

                            angular.forEach($scope.employeeid, function (ddd) {
                                $scope.templist = [];
                                var sum_deviation = 0;
                                angular.forEach($scope.griddata, function (dd) {
                                    if (dd.IVRMCH_Id === ddd.IVRMCH_Id) {
                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.griddatadetails = $scope.templist;
                            });

                        }
                    }
                    else {
                        swal("Data not Found !!");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.exptoex = function (tabel1) {
            var excelnamemain = "VERSION_CONTROL_REPORT" + " " + $scope.hostname + " " + $scope.currentversion + " " ;
            var printSectionId = '#table1';
            excelnamemain = excelnamemain + '' + '.xls';
            var exportHref = Excel.tableToExcel(printSectionId, 'Student Register Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelnamemain;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);

        };

        $scope.onChangeradio = function () {
            $scope.griddata = [];
            $scope.employeeid = [];
        }
        //$scope.exptoex = function () {
        //    var divToPrint = document.getElementById("table1");
        //    var exportHref = Excel.tableToExcel(divToPrint, divToPrint);
        //    $timeout(function () {
        //        location.href = exportHref;
        //    }, 100);
        //};


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




    }
})();
