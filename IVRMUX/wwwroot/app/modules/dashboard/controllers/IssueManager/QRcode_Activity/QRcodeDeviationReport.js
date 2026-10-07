
(function () {
    'use strict';
    angular
        .module('app')
        .controller('QRcodeDeviationReportController', QRcodeDeviationReportController);
    QRcodeDeviationReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$compile'];
    function QRcodeDeviationReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $compile) {

        var paginationformasters;
        $scope.multidepartment = false;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.obj = {};
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();

        $scope.loaddata = function () {
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("QRcodeDeviationReport/getloaddata", pageid).then(function (promise) {
             
                $scope.get_building = promise.get_building;

                $scope.fillroletype = promise.fillroletype;
            });
        };

       

        //-----------Employee list
        $scope.Employeelistchange = function (role_id) {
            $scope.emp_deatils = [];
            var data = {
                "IVRMRT_Id": role_id,
            }
            apiService.create("QRcodeDeviationReport/getEmployeeName", data).
                then(function (promise) {
                    if (promise.emp_deatils != null && promise.emp_deatils.length > 0) {
                        $scope.emp_deatils = promise.emp_deatils;
                    }
                    else {
                        swal("No Record Found....")
                    }

                })
        }
        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.emp_deatils, function (uem) {
                uem.id = toggleStatus;
            });
        };
        $scope.togchkbx = function () {
            $scope.checkall = $scope.emp_deatils.every(function (options) {
                return options.id;
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.emp_deatils.some(function (options) {
                return options.id;
            });
        };


        //*************************** DEVIATION REPORT *********************//        
        $scope.deviationReport = function () {
            if ($scope.myForm.$valid) {
                $scope.arrayuserEmp = [];
                angular.forEach($scope.emp_deatils, function (em) {
                    if (em.id === true) {
                        $scope.arrayuserEmp.push({ UserId: em.Id });
                    }
                });
                var data = {
                    "startdate": new Date($scope.start_Date).toDateString(),
                    "enddate": new Date($scope.end_Date).toDateString(),
                    "arrayuserEmp": $scope.arrayuserEmp,
                    "HRMB_Id": $scope.hrmB_Id
            
                };
                apiService.create("QRcodeDeviationReport/QRcodedeviation", data).then(function (promise) {

                    $scope.get_deviationreport_Temp = [];
                    $scope.get_deviationreport = [];
                    $scope.get_deviationreport_Temp = promise.get_deviationreport;
                    if ($scope.get_deviationreport_Temp != null && $scope.get_deviationreport_Temp.length > 0) {
                        for (var t = 0; t < $scope.get_deviationreport_Temp.length; t++) {
                            $scope.get_deviationreport.push({
                                HRMARACTT_Time: moment($scope.get_deviationreport_Temp[t].HRMARACTT_Time, 'HH:mm a').format(),
                                HRSTARACTRCD_Time: moment($scope.get_deviationreport_Temp[t].HRSTARACTRCD_Time, 'HH:mm a').format(),
                                HRMB_BuildingName: $scope.get_deviationreport_Temp[t].HRMB_BuildingName,
                                UserName: $scope.get_deviationreport_Temp[t].UserName,
                                HRMF_FloorName: $scope.get_deviationreport_Temp[t].HRMF_FloorName,
                                HRMAR_AreaName: $scope.get_deviationreport_Temp[t].HRMAR_AreaName,
                                HRMARACT_ActivityName: $scope.get_deviationreport_Temp[t].HRMARACT_ActivityName,
                                Deviation: $scope.get_deviationreport_Temp[t].Deviation,
                                status: $scope.get_deviationreport_Temp[t].status,
                                HRMARACTT_ActivityDuration: $scope.get_deviationreport_Temp[t].HRMARACTT_ActivityDuration,
                                HRMARACTSTMAP_Date: new Date($scope.get_deviationreport_Temp[t].HRMARACTSTMAP_Date),
                            });
                        }
                    }
                    else {
                        swal("No Record  Found..... !!");
                        $scope.get_deviationreport = "";
                    }

                });
            }
            else {
                $scope.submitted = true;
            }
        };

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
        };
        $scope.exportToExcel = function (export_id) {
            var exportHref = Excel.tableToExcel(export_id, 'printDeviation');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.Clear = function () {
            $scope.get_deviationreport = "";
            $scope.emp_deatils = "";
            $scope.checkall = "";
            $scope.start_Date = "";
            $scope.end_Date = "";
            $scope.loaddata();
        };
        
    }
})();