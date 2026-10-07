(function () {
    'use strict';
    angular
        .module('app')
        .controller('TADA_MasterPageReportController', TADA_MasterPageReportController)
    TADA_MasterPageReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function TADA_MasterPageReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {
        $scope.currentPage = 1;
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        else {
            paginationformasters = 10;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }                   
        $scope.searchValue = "";
        $scope.report = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.onpageload = function () {
            var pageid = 1;
            apiService.getURI("TADA_CityMapping/loaddata", pageid).
                then(function (promise) {
                    $scope.grneTLIst = promise.designation_types;
                    $scope.getstate = promise.state;
                })
        }
        //====drodowncity====
        $scope.submitted = false;
        $scope.getReport = function () {
            $scope.submitted = true;           
            if ($scope.myForm.$valid) {
                $scope.Desgighna_Institutionsss = [];
                angular.forEach($scope.grneTLIst, function (comp) {
                    if (comp.selected == true) {
                        $scope.Desgighna_Institutionsss.push({ HRMDES_IdS: comp.hrmdeS_Id });
                    }
                });
                    var data = {
                        "IVRMMS_Id": $scope.IVRMMS_Id,
                        "List": $scope.Desgighna_Institutionsss
                    };
                apiService.create("TADA_CityMapping/getMasterReport", data).then(function (promise) {
                    if (promise.get_MasterReport.length > 0) {
                        $scope.imagenew = promise.mI_Logo;
                            $scope.get_CityMapping = promise.get_MasterReport;
                            $scope.presentCountgrid = $scope.get_CityMapping.length;
                            $scope.company_name = promise.company_name;
                            $scope.report = true;                            
                            //getstate
                            $scope.employee = [];
                            $scope.employee = $scope.get_CityMapping[0].hrmdeS_DesignationName;
                            $scope.employeeid = [];
                            angular.forEach($scope.get_CityMapping, function (dev) {
                                if ($scope.employeeid.length === 0) {
                                    $scope.employeeid.push({ hrmdeS_Id: dev.hrmdeS_Id, vtadacM_Id: dev.vtadacM_Id, hrmdeS_DesignationName: dev.hrmdeS_DesignationName, ivrmmcT_Name: dev.ivrmmcT_Name, vtadacM_FoodAmt: dev.vtadacM_FoodAmt, vtadacM_AccommodationAmt: dev.vtadacM_AccommodationAmt, vtadacM_TransportAmt: dev.vtadacM_TransportAmt, vtadacM_TravelHour: dev.vtadacM_TravelHour });
                                }
                                else if ($scope.employeeid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.employeeid, function (emp) {
                                        if (emp.hrmdeS_Id === dev.hrmdeS_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.employeeid.push({ hrmdeS_Id: dev.hrmdeS_Id, vtadacM_Id: dev.vtadacM_Id, hrmdeS_DesignationName: dev.hrmdeS_DesignationName, ivrmmcT_Name: dev.ivrmmcT_Name, vtadacM_FoodAmt: dev.vtadacM_FoodAmt, vtadacM_AccommodationAmt: dev.vtadacM_AccommodationAmt, vtadacM_TransportAmt: dev.vtadacM_TransportAmt, vtadacM_TravelHour: dev.vtadacM_TravelHour });
                                    }
                                }
                            });
                            console.log($scope.employeeid);
                            angular.forEach($scope.employeeid, function (ddd) {
                                $scope.templist = [];
                                angular.forEach($scope.get_CityMapping, function (dd) {
                                    if (dd.hrmdeS_Id === ddd.hrmdeS_Id) {
                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.plannerdetails = $scope.templist;
                            });
                            console.log($scope.employeeid);
                           
                        }
                        else {
                            swal("No Record Found !");
                            $state.reload();
                        }
                    });
            }          
        };
        $scope.exportToExcel = function () {
            if ($scope.get_CityMapping.length > 0) {
                var exportHref = Excel.tableToExcel(tablegrp, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
        };
        $scope.printData = function () {
            if ($scope.get_CityMapping.length > 0) {
                var innerContents = document.getElementById("tablegrp").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                // $scope.imgdiv = true;
                $scope.headerimg = true;
                popupWinindow.document.close();
            }
        };
        $scope.Clearid = function () {           
            $state.reload();                      
        }      
        $scope.all_checkCCCC = function () {
            var hrmdeS_Id = $scope.hrmdeS_Id;
            var count = 0;
            angular.forEach($scope.grneTLIst, function (itm) {
                itm.selected = hrmdeS_Id;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }
        $scope.togchkbxCCCC = function () {
            $scope.hrmdeS_Id = $scope.grneTLIst.every(function (options) {
                return options.selected;
            });
        }
        $scope.isOptionsRequiredtwo = function () {
            return !$scope.grneTLIst.some(function (options) {
                return options.selected;
            });
        }
    }
})();

