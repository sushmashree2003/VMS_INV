
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_LocationReportController', INV_LocationReportController);
    INV_LocationReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter'];
    function INV_LocationReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {             
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null &&  ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 10;
        }
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null &&   admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.search = "";      
        $scope.loaddata = function () {           
            var data = {
                "optionflag": $scope.optionflag              
            };
            apiService.create("INV_LocationReport/getloaddata", data).then(function (promise) {
                    $scope.get_mi_list = promise.milist;
                });
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cancel = function () {
            $state.reload();
        };
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.get_itemreportdetails.every(function (itm) {
                return itm.itemck;
            });
        };
        //item
        $scope.all_checkI = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.get_itemreportdetails, function (itm) {
                itm.itemck = toggleStatus;
            });
        };
        //location
        $scope.all_checkII = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.get_itemreportdetails, function (itm) {
                itm.itemck = toggleStatus;
            });
        };
        //strore
        $scope.all_checkIII = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.get_itemreportdetails, function (itm) {
                itm.itemck = toggleStatus;
            });
        };
        //group
        $scope.all_checkIIII = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.get_itemreportdetails, function (itm) {
                itm.itemck = toggleStatus;
            });
        };
        //onrdochangeTwo
        $scope.onrdochangeTwo = function () {
           
            $scope.get_itemreport = "";
        };
        $scope.onrdochange = function () {
            $scope.get_itemreportdetails = "";
            $scope.get_itemreport = "";
            if ($scope.MI_Id != null && $scope.optionflag != null && $scope.MI_Id != "" && $scope.optionflag != "") {
                var data = {
                    "optionflag": $scope.optionflag,
                    "MI_Id": $scope.MI_Id
                };
                apiService.create("INV_LocationReport/onchangeloaddata", data).then(function (promise) {
                    if (promise.get_itemreportdetails.length != null && promise.get_itemreportdetails.length > 0) {
                        $scope.get_itemreportdetails = promise.get_itemreportdetails;
                    }
                    else {
                        swal("Record Not Found...!!");                        
                    }
                });
            }
            else {
                swal("Please Select Any One Institutions ...!!");
            }           
        };
        $scope.onreport = function () {
            if ($scope.myForm.$valid) {
                $scope.start_Date = $filter('date')($scope.startdate, "yyyy-MM-dd");
                $scope.end_Date = $filter('date')($scope.enddate, "yyyy-MM-dd");
                if ($scope.bw_dates === true) {
                    $scope.sDate = $scope.start_Date;
                    $scope.eDate = $scope.end_Date;
                }
                else {
                    $scope.sDate = "";
                    $scope.eDate = "";
                }
                $scope.itemsArray = [];
                if ($scope.optionflag === null || $scope.optionflag === undefined) {
                    $scope.optionflag = "";
                }
                var data = {};
                if ($scope.optionflag === "Item") {
                    angular.forEach($scope.get_itemreportdetails, function (itm) {
                        if (itm.itemck === true) {
                            $scope.itemsArray.push(itm);
                        }
                    });
                   
                }    
                if ($scope.optionflag === "Group") {
                    angular.forEach($scope.get_itemreportdetails, function (itm) {
                        if (itm.itemck === true) {
                            $scope.itemsArray.push(itm);
                        }
                    });

                }  
                if ($scope.optionflag === "Location") {
                    angular.forEach($scope.get_itemreportdetails, function (itm) {
                        if (itm.itemck === true) {
                            $scope.itemsArray.push(itm);
                        }
                    });

                }  
                if ($scope.optionflag === "Store") {
                    angular.forEach($scope.get_itemreportdetails, function (itm) {
                        if (itm.itemck === true) {
                            $scope.itemsArray.push(itm);
                        }
                    });
                }  
                data = {
                    "optionflag": $scope.optionflag,
                    "startdate": $scope.sDate,
                    "enddate": $scope.eDate,
                    "itemsArray": $scope.itemsArray,
                    "MI_Id": $scope.MI_Id,
                    "optionflagtwo": $scope.optionflagtwo
                };
                apiService.create("INV_LocationReport/onreport", data).
                    then(function (promise) {
                        if (promise.get_itemreport.length != null &&  promise.get_itemreport.length > 0) {
                            $scope.get_itemreport = promise.get_itemreport;
                            $scope.presentCountgrid = $scope.get_itemreport.length;
                        }
                        else {
                            swal("No Record Found...!!");
                            $scope.get_itemreport = "";
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.isOptionsRequiredI = function () {
            if ($scope.optionflag === 'Item') {
                return !$scope.get_itemreportdetails.some(function (options) {
                    return options.itemck;
                });
            } else {
                return false;
            }
        };
        $scope.isOptionsRequiredII = function () {
            if ($scope.optionflag === 'Location') {
                return !$scope.get_itemreportdetails.some(function (options) {
                    return options.itemck;
                });
            } else {
                return false;
            }
        };
        $scope.isOptionsRequiredIII = function () {
            if ($scope.optionflag === 'Store') {
                return !$scope.get_itemreportdetails.some(function (options) {
                    return options.itemck;
                });
            } else {
                return false;
            }
        };
        //isOptionsRequiredI
        $scope.isOptionsRequiredIIII = function () {
            if ($scope.optionflag === 'Group') {
                return !$scope.get_itemreportdetails.some(function (options) {
                    return options.itemck;
                });
            } else {
                return false;
            }
        };
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.exportToExcel = function (export_id) {
            var exportHref = Excel.tableToExcel(export_id, 'printItem');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };
        $scope.printData = function () {
            var innerContents = document.getElementById("printItem").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
    }
})();