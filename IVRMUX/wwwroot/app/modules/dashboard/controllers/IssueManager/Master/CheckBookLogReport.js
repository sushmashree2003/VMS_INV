(function () {
    'use strict';
    angular.module('app').controller('CheckBookLogReportController', CheckBookLogReportController)
    CheckBookLogReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', 'Excel','$timeout']
    function CheckBookLogReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, Excel, $timeout) {


        $scope.loaddata = function () {
            $scope.obj.MI_Id = "";

            var pageid = 2;
            apiService.getURI("CheckBookLogReport/loaddata", pageid).then(function (promise) {
                $scope.allcompany = promise.allcompany;


            });
        };

        $scope.optionToggled2 = function () {
            $scope.obj.checkallm1 = $scope.Cheque.every(function (itm) {
                return itm.selected1;
            });

        };
        $scope.all_check1 = function () {
            var toggleStatus = $scope.obj.checkallm1;
            angular.forEach($scope.Cheque, function (itm) {
                itm.selected1 = toggleStatus;
            });

        };
        $scope.isOptionsRequired = function () {
            return !$scope.Cheque.some(function (options) {
                return options.selected1;
            });

        };

       
        $scope.onrdochange = function () {
            $scope.allReport = [];
        };

        $scope.oncmpchange = function () {
            $scope.obj.HRMBD_Id = "";
            var data = {
                "MI_Id": $scope.obj.MI_Id,
            };

            apiService.create("CheckBookLogReport/onchange", data).then(function (promise) {
                $scope.banklist = promise.banklist;
            });
        }

        $scope.onbnkchange = function () {
            $scope.Cheque = [];
            $scope.obj.VPAYVOU_Id = "";
            var data = {
                "HRMBD_Id": $scope.obj.HRMBD_Id,
                "MI_Id": $scope.obj.MI_Id,
            };
            apiService.create("CheckBookLogReport/onchange", data).then(function (promise) {
                $scope.Cheque = promise.cheque;
            });
        }


        $scope.hrmdlist = [];
        $scope.chequelist = [];
        $scope.allReport = [];
        $scope.report = function () {
            $scope.chequelist = [];

            if ($scope.banklist != null && $scope.banklist.length > 0) {
                angular.forEach($scope.banklist, function (comp) {
                    if (comp.hrmbid == true) {
                        $scope.hrmdlist.push({
                            HRMD_Id: comp.hrmbD_Id
                        });
                    }
                });
            }
            
                if ($scope.Cheque != null && $scope.Cheque.length > 0) {
                    angular.forEach($scope.Cheque, function (comp) {
                        if (comp.selected1 == true) {
                            $scope.chequelist.push({
                                VPAYVOU_Id: comp.vpayvoU_Id
                            });
                        }
                    });
                }
                $scope.from_date = new Date($scope.Fromdate).toDateString();
            $scope.to_date = new Date($scope.Todate).toDateString();
            if ($scope.myForm.$valid) {
                var data = {
                    "HRMBD_Id": $scope.obj.HRMBD_Id,
                    "MI_Id": $scope.obj.MI_Id,
                    "chequedetails": $scope.chequelist,
                    "type": $scope.optionflag,                    "startdate": $scope.from_date,                    "enddate": $scope.to_date



                };
                apiService.create("CheckBookLogReport/Report", data).then(function (promise) {

                    if (promise.allReport != null && promise.allReport.length > 0) {
                        $scope.printflg = true;
                        $scope.tablegrid = promise.allReport;
                        $scope.imagenewtwo = promise.mI_Logo;
                        $scope.flag = promise.type;
                        angular.forEach($scope.tablegrid, function (itm) {
                            itm.ITAT_Time = moment(itm.ITAT_Time, 'HH:mm a').format();


                        });
                    }
                    else {
                        swal("Record Not Found !");
                    }



                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.exportToExcel = function (logreport) {
           
            var exportHref = Excel.tableToExcel(logreport, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };

      
        $scope.printDataREC = function () {
         
            var innerContents = innerContents = document.getElementById("chequebook").innerHTML;
            
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
            $scope.interacted = function (field) {
                return $scope.submitted;
            };

            $scope.Clearid = function () {
                $state.reload();
            };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
    }
    })();
