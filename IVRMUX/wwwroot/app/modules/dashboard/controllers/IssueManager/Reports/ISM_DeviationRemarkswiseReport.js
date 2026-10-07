(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_PlannerDeviationReportController', ISM_PlannerDeviationReportController);
    ISM_PlannerDeviationReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter'];
    function ISM_PlannerDeviationReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter) {

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
            apiService.getURI("ISM_PlannerDeviationReport/getdetails", pageid).then(function (promise) {

                $scope.get_Status = promise.get_Status;                
                $scope.get_userEmplist = promise.get_userEmplist;
                $scope.empdepartment = promise.empdepartment;
                $scope.empname = promise.empname;
                $scope.empname = promise.empname;
            });
        };

        //*************************CHECK Box Selection****************************//
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
        $scope.all_checkS = function (ack) {
            $scope.checkallS = ack;
            var toggleStatus = $scope.checkallS;
            angular.forEach($scope.get_Status, function (sts) {
                sts.stck = toggleStatus;
            });
        };
        $scope.togchkbxS = function () {
            $scope.checkallS = $scope.get_Status.every(function (options) {
                return options.stck;
            });
        };
        $scope.isOptionsRequiredS = function () {
            return !$scope.get_Status.some(function (options) {
                return options.stck;
            });
        };
        //*************************** DEVIATION REMARKS-WISE REPORT *********************//        
        $scope.deviationremarksReport = function () {
            if ($scope.myForm.$valid) {
                $scope.get_deviationRemarksReport = [];
                $scope.deviationtype = "Remarks";
                $scope.arrayuserEmp = [];
                $scope.arraydeviationStatus = [];
                angular.forEach($scope.get_userEmplist, function (em) {
                    if (em.empck === true) {
                        $scope.arrayuserEmp.push(em);
                    }
                });
                angular.forEach($scope.get_Status, function (sts) {
                    if (sts.stck === true) {
                        $scope.arraydeviationStatus.push({ deviationStatus: sts.ISMMISTS_StatusName });
                    }
                });
                var data = {
                    "deviationtype": $scope.deviationtype,
                    "startdate": new Date($scope.start_Date).toDateString(),
                    "enddate": new Date($scope.end_Date).toDateString(),
                    "NoOfDays": $scope.noofdays,
                    "arrayuserEmp": $scope.arrayuserEmp,
                    "arraydeviationStatus": $scope.arraydeviationStatus
                };
                apiService.create("ISM_PlannerDeviationReport/getdeviationReport", data).then(function (promise) {
                    if (promise.get_deviationRemarksReport !== null && promise.get_deviationRemarksReport.length > 0) {

                        $scope.columnsheader = [];                        
                        $scope.columnsheader.push({ title: 'Task No.', field: 'ISMTCR_TaskNo', name: 'ISMTCR_TaskNo', width: 150 });
                        $scope.columnsheader.push({ title: 'Task Title', field: 'ISMTCR_Title', name: 'ISMTCR_Title', width: 150 });
                        $scope.columnsheader.push({ title: 'Task Description', field: 'ISMTCR_Desc', name: 'ISMTCR_Desc', width: 150 });                       
                        $scope.columnsheader.push({ title: 'Status', field: 'ISMTCR_Status', name: 'ISMTCR_Status', width: 100 });
                        $scope.columnsheader.push({ title: 'Employee', field: 'EmpName', name: 'EmpName', width: 100 });                      
                        $scope.columnsheader.push({ title: 'From Date', field: 'ISMTAPL_FromDate', name: 'ISMTAPL_FromDate', width: 100 });
                        $scope.columnsheader.push({ title: 'To Date', field: 'ISMTAPL_ToDate', name: 'ISMTAPL_ToDate', width: 100 });
                        $scope.columnsheader.push({ title: 'Deviation Days', field: 'DiffDays', name: 'DiffDays', width: 100 });

                        $(document).ready(function () {
                            initGridall();
                        });

                        function initGridall() {
                            $('#gridall').empty();
                            $("#gridall").kendoGrid({
                                toolbar: ["excel"],
                                excel: {
                                    fileName: "Deviation Consolidated Report.xlsx",
                                    proxyURL: "",
                                    filterable: true,
                                    allPages: true
                                },
                                dataSource: {
                                    data: promise.get_deviationRemarksReport,
                                    pageSize: 10
                                },
                                sortable: true,
                                pageable: true,
                                groupable: false,
                                filterable: true,
                                columnMenu: true,
                                reorderable: true,
                                resizable: true,

                                columns: $scope.columnsheader,
                                dataBound: function () {
                                    var pagenum = this.dataSource.page();
                                    var pageitms = this.dataSource.pageSize();
                                    var rows = this.items();
                                    $(rows).each(function () {
                                        var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                        var rowLabel = $(this).find(".row-number");
                                        $(rowLabel).html(index);
                                    });
                                }
                            }).data("kendoGrid");
                        }
                    }
                    else {
                        swal("No Record  Found..... !!");
                        $scope.get_deviationRemarksReport = "";
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printDRR").innerHTML;
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
            var exportHref = Excel.tableToExcel(export_id, 'printDRR');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //===========Clear Field
        $scope.Clear = function () {
            $scope.get_deviationRemarksReport = "";
            $scope.get_userEmplist = "";
            $scope.checkall = "";
            $scope.checkallS = "";
            $scope.start_Date = "";
            $scope.end_Date = "";
            $scope.loaddata();
        };

        $scope.searchValueS = '';
        $scope.searchValueSD = function (obj) {
            return angular.lowercase(obj.ISMMISTS_StatusName).indexOf(angular.lowercase($scope.searchValueS)) >= 0;
        };

        $scope.searchValueUEM = '';
        $scope.searchValueUEMD = function (obj) {
            return angular.lowercase(obj.userEmpName).indexOf(angular.lowercase($scope.searchValueUEM)) >= 0 || angular.lowercase(obj.HRMD_DepartmentName).indexOf(angular.lowercase($scope.searchValueUEM)) >= 0;
        };
    }
})();

