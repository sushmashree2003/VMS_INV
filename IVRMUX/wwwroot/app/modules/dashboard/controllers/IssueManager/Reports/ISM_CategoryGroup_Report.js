(function () {
    'use strict';
    angular.module('app').controller('ISM_CategoryGroup_ReportController', ISM_CategoryGroup_ReportController);
    ISM_CategoryGroup_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter'];
    function ISM_CategoryGroup_ReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter) {
        $scope.search = "";

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

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

        // ============================ng_click function for module (ALL)========================//
        $scope.searchchkbx = "";
        $scope.all_checktype = function () {

            var checkStatus = $scope.userchecktype;
            angular.forEach($scope.getemployee, function (itmtype) {
                itmtype.selectdemployee = checkStatus;
            });
        };

        $scope.togchkbxtype = function () {
            $scope.userchecktype = $scope.getemployee.every(function (optionstype) {
                return optionstype.selectdemployee;
            });
        };

        $scope.isOptionsRequiredtype = function () {
            return !$scope.getemployee.some(function (options) {
                return options.selectdemployee;
            });
        };

        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.getcategory, function (itm) {
                itm.selectdcaategory = checkStatus;
            });
        };

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.getcategory.every(function (options) {
                return options.selectdcaategory;
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.getcategory.some(function (options) {
                return options.selectdcaategory;
            });
        };

        $scope.filterchkbxsearch = '';
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.ismmtcaT_TaskCategoryName).indexOf(angular.lowercase($scope.filterchkbxsearch)) >= 0;
        };

        $scope.searchchkbxtype = '';
        $scope.filterchkbxtype = function (obj) {
            return angular.lowercase(obj.userEmpName).indexOf(angular.lowercase($scope.searchchkbxtype)) >= 0;

        };

        $scope.submitted = false;
        $scope.searchchkbx = "";
        $scope.searchchkbxtype = "";

        $scope.Clear = function () {
            $state.reload();
        };

        //===========================load
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.itemsPerPage1 = paginationformasters;
        $scope.currentPage1 = 1;
        $scope.loaddata = function () {

            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            var data = {
                "MI_Id": 4
            };

            apiService.create("ISM_TaskGroup_Report/getcategorydetails", data).then(function (promise) {
                $scope.department_list = promise.departmentList;
            });
        };

        $scope.onchangedept = function () {

            var data = {
                "HRMDC_ID": $scope.Dept_Id,
                "ISMMPR_ProjectName": ""
            }; apiService.create("ISM_TaskGroup_Report/onchangedept", data).then(function (promise) {
                $scope.getcategory = promise.getcategory;
                $scope.getemployee = promise.getemployee;

                if ($scope.getcategory !== null && $scope.getcategory.length > 0) {
                    $scope.usercheck = true;
                    angular.forEach($scope.getcategory, function (dd) {
                        dd.selectdcaategory = true;
                    });
                }

                if ($scope.getemployee !== null && $scope.getemployee.length > 0) {
                    $scope.userchecktype = true;
                    angular.forEach($scope.getemployee, function (dd) {
                        dd.selectdemployee = true;
                    });
                }

            });
        };


        //*************************** Group REPORT *********************//        
        $scope.categorygroupReport = function () {
            if ($scope.myForm.$valid) {
                $scope.selectedemp = [];
                angular.forEach($scope.getemployee, function (d) {
                    if (d.selectdemployee === true) {
                        $scope.selectedemp.push({ HRME_Id: d.HRME_Id });
                    }
                });

                $scope.selectedcategory = [];
                angular.forEach($scope.getcategory, function (d) {
                    if (d.selectdcaategory === true) {
                        $scope.selectedcategory.push({ ISMMTCAT_Id: d.ismmtcaT_Id });
                    }
                });

                var data = {
                    "startdate": new Date($scope.start_Date).toDateString(),
                    "enddate": new Date($scope.end_Date).toDateString(),
                    "categorytempdto": $scope.selectedcategory,
                    "categoryemptempdto": $scope.selectedemp,
                    "HRMDC_ID": $scope.Dept_Id
                };

                apiService.create("ISM_TaskGroup_Report/categorygroupReport", data).then(function (promise) {

                    if (promise.get_report !== null && promise.get_report.length > 0) {
                        $scope.get_report = promise.get_report;



                        $scope.columnsheader = [];
                        $scope.columnsheader.push({ title: 'Department', field: 'HRMD_DepartmentName', name: 'HRMD_DepartmentName', width: 150 });
                        $scope.columnsheader.push({ title: 'Task No.', field: 'ISMTCR_TaskNo', name: 'ISMTCR_TaskNo', width: 150 });
                        $scope.columnsheader.push({ title: 'Task Title', field: 'ISMTCR_Title', name: 'ISMTCR_Title', width: 150 });
                        $scope.columnsheader.push({ title: 'Task Description', field: 'ISMTCR_Desc', name: 'ISMTCR_Desc', width: 150 });
                        $scope.columnsheader.push({ title: 'Client', field: 'ISMMCLT_ClientName', name: 'ISMMCLT_ClientName', width: 150 });
                        $scope.columnsheader.push({ title: 'Module', field: 'IVRMM_ModuleName', name: 'IVRMM_ModuleName', width: 150 });
                        $scope.columnsheader.push({ title: 'Status', field: 'ISMTCR_Status', name: 'ISMTCR_Status', width: 100 });
                        $scope.columnsheader.push({ title: 'Category', field: 'ISMMTCAT_TaskCategoryName', name: 'ISMMTCAT_TaskCategoryName', width: 100 });
                        $scope.columnsheader.push({ title: 'Created By', field: 'createdby', name: 'createdby', width: 100 });
                        $scope.columnsheader.push({ title: 'Assigned By', field: 'assignedby', name: 'assignedby', width: 100 });
                        $scope.columnsheader.push({ title: 'Assigned To', field: 'assignedto', name: 'assignedto', width: 100 });
                        $scope.columnsheader.push({ title: 'Created Date', field: 'crdate', name: 'crdate', width: 100 });

                        $(document).ready(function () {
                            initGridall();
                        });

                        function initGridall() {
                            $('#gridall').empty();
                            $("#gridall").kendoGrid({
                                toolbar: ["excel"],
                                excel: {
                                    fileName: "Category Group Report.xlsx",
                                    proxyURL: "",
                                    filterable: true,
                                    allPages: true
                                },
                                dataSource: {
                                    data: $scope.get_report,
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
                        swal("No Record Found..... !!");
                        $scope.get_report = [];
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.printData = function () {
            var innerContents = '';
            innerContents = document.getElementById("printareaId1").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); },900);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function (table) {
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();

