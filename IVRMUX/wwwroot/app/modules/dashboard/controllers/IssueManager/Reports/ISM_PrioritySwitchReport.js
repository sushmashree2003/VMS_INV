(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_PrioritySwitchReportController', ISM_PrioritySwitchReportController);
    ISM_PrioritySwitchReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache' ,'Excel', '$timeout', '$filter'];
    function ISM_PrioritySwitchReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache,Excel, $timeout, $filter) {


        var paginationformasters;
        $scope.multidepartment = false;
        $scope.searchValue = '';
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
            apiService.getURI("ISM_PrioritySwitchReport/getdetails", pageid).then(function (promise) {
                $scope.get_userEmplist = promise.get_userEmplist;
                $scope.empdepartment = promise.empdepartment;
                $scope.empname = promise.empname;
            });
        };

        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.get_userEmplist, function (uem) {
                uem.empck = toggleStatus;
            });
        };
        $scope.togchkbx = function () {
            $scope.checkall = $scope.get_userEmplist.every(function (options) {
                return options.empck;
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.get_userEmplist.some(function (options) {
                return options.empck;
            });
        };
        //*************************** DEVIATION REPORT *********************//        
        $scope.deviationReport = function () {
            $scope.get_deviationRemarksReport = [];
            if ($scope.myForm.$valid) {
                $scope.arrayuserEmp = [];
                angular.forEach($scope.get_userEmplist, function (em) {
                    if (em.empck === true) {
                        $scope.arrayuserEmp.push(em);
                    }
                });
                var start_Date = $filter('date')($scope.start_Date, "yyyy-MM-dd");
                var end_Date = $filter('date')($scope.end_Date, "yyyy-MM-dd");
                var data = {
                    "startdate1": start_Date,
                    "enddate1": end_Date,
                    "arrayuserEmp": $scope.arrayuserEmp
                };
                apiService.create("ISM_PrioritySwitchReport/getdeviationReport", data).then(function (promise) {
                    if (promise.get_deviationRemarksReport.length > 0) {

                        $scope.get_deviationRemarksReport = promise.get_deviationRemarksReport;

                        $scope.tempto = [];
                        angular.forEach($scope.get_deviationRemarksReport, function (ddd) {

                            if ($scope.tempto.length == 0) {
                                $scope.tempto.push({
                                    ISMTCR_Id: ddd.ISMTCR_Id,
                                    ISMTCR_CreationDate: ddd.ISMTCR_CreationDate,
                                    ISMTCR_Title: ddd.ISMTCR_Title,
                                    ISMTCR_Desc: ddd.ISMTCR_Desc,
                                    AssignedBy: ddd.AssignedBy,
                                    AssignedTo: ddd.AssignedTo,
                                    ISMTCRASTO_Remarks: ddd.ISMTCRASTO_Remarks,
                                    ISMTCRASTO_StartDate: ddd.ISMTCRASTO_StartDate,
                                    ISMTCRASTO_EndDate: ddd.ISMTCRASTO_EndDate,
                                    ISMTCR_TaskNo: ddd.ISMTCR_TaskNo,
                                    ISMTCRASTO_AssignedDate: ddd.ISMTCRASTO_AssignedDate,
                                 

                                });
                            }
                            else if ($scope.tempto.length > 0) {
                                var already_cnt = 0;
                                angular.forEach($scope.tempto, function (opq1) {
                                    if (opq1.ISMTCR_Id == ddd.ISMTCR_Id) {
                                        already_cnt += 1;
                                    }
                                })

                                if (already_cnt == 0) {
                                    $scope.tempto.push({
                                        ISMTCR_Id: ddd.ISMTCR_Id,
                                        ISMTCR_CreationDate: ddd.ISMTCR_CreationDate,
                                        ISMTCR_Title: ddd.ISMTCR_Title,
                                        ISMTCR_Desc: ddd.ISMTCR_Desc,
                                        AssignedBy: ddd.AssignedBy,
                                        AssignedTo: ddd.AssignedTo,
                                        ISMTCRASTO_Remarks: ddd.ISMTCRASTO_Remarks,
                                        ISMTCRASTO_StartDate: ddd.ISMTCRASTO_StartDate,
                                        ISMTCRASTO_EndDate: ddd.ISMTCRASTO_EndDate,
                                        ISMTCR_TaskNo: ddd.ISMTCR_TaskNo,
                                        ISMTCRASTO_AssignedDate: ddd.ISMTCRASTO_AssignedDate,


                                    });
                                }

                            }
                        })


                        angular.forEach($scope.tempto, function (ff) {
                            var statuslist = [];
                            angular.forEach($scope.get_deviationRemarksReport, function (gg) {
                                if (ff.ISMTCR_Id == gg.ISMTCR_Id) {
                                    statuslist.push({ ISMTCR_Id: gg.ISMTCR_Id, TaskPriorityStatus: gg.TaskPriorityStatus, OldPriority: gg.OldPriority, NewPriority: gg.NewPriority, ISMTPSCD_CurrentDate: gg.ISMTPSCD_CurrentDate})
                                }


                            })

                            ff.LIST = statuslist;

                        })

                        console.log($scope.tempto);

                       
                    }
                    else {
                        swal("No Record  Found..... !!");
                     
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
            $state.reload();
        };

    }
})();

