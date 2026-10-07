(function () {
    'use strict';
    angular
.module('app')
.controller('BankCashReportController', BankCashReportController)
    BankCashReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache', 'uiGridExporterService', 'uiGridExporterConstants', 'Excel', '$timeout']
    function BankCashReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache, uiGridExporterService, uiGridExporterConstants, Excel, $timeout) {
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.SalaryFromDay = "";
        $scope.SalaryToDay = "";
        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("BankCashReport/getalldetails", pageid).then(function (promise) {
                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {
                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }
                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;
                }

                //if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                //    $scope.employeedropdown = promise.employeedropdown;
                //}

                if (promise.groupTypedropdown !== null && promise.groupTypedropdown.length > 0) {
                    $scope.groupTypedropdown = promise.groupTypedropdown;
                    $scope.groupTypeselectedAll = true;
                    $scope.GetEmployeeBygroupTypeAll($scope.groupTypeselectedAll);
                }

                if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                    $scope.departmentdropdown = promise.departmentdropdown;
                    $scope.departmentselectedAll = true;
                    $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                }

                //if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                //    $scope.designationdropdown = promise.designationdropdown;
                //    $scope.designationselectedAll = true;
                //    $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                //}
            });
        };

        $scope.employeeDetails = [];
        $scope.EmployeeDis = false;
        $scope.institutionDetails = {};
        //Search employee
        $scope.submitted = false;
        $scope.SearchEmployee = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.institutionDetails = {};
                var groupTypeselected = [];
                var departmentselected = [];
                // var designationselected = [];
                var data = {};

                angular.forEach($scope.groupTypedropdown, function (itm) {
                    if (itm.selected) {
                        groupTypeselected.push(itm.hrmgT_Id);
                    }
                });

                angular.forEach($scope.departmentdropdown, function (itm) {
                    if (itm.selected) {
                        departmentselected.push(itm.hrmD_Id);
                    }
                });

                //angular.forEach($scope.designationdropdown, function (itm) {
                //    if (itm.selected) {
                //        designationselected.push(itm.hrmdeS_Id);
                //    }
                //});

                if (groupTypeselected.length == 0 && departmentselected.length == 0) {
                    swal('Kindly select atleast one record');
                    return;
                }

                $scope.Employee.groupTypeselected = groupTypeselected;
                $scope.Employee.departmentselected = departmentselected;
                // $scope.Employee.designationselected = designationselected;
                $scope.Employee.BankCash = "Bank";
                data = $scope.Employee;

                apiService.create("BankCashReport/getEmployeedetailsBySelection", data).
                    then(function (promise) {

                        if (promise.institutionDetails != null) {
                            $scope.institutionDetails = promise.institutionDetails;
                            //  $('#blah').attr('src', 'https://bdcampusstrg.blob.core.windows.net/files/' + $scope.institutionDetails.mi_id + "/" + "EmployeeProfilePics" + "/" + $scope.institutionDetails.mI_Logo);
                            $scope.selectedMonth = promise.hreS_Month;
                            $scope.selectedYear = promise.hreS_Year;

                            var instuteAddress = "";
                            if ($scope.institutionDetails.mI_Address1 != null && $scope.institutionDetails.mI_Address1 != "") {
                                instuteAddress = $scope.institutionDetails.mI_Address1;
                            }
                            if ($scope.institutionDetails.mI_Address2 != null && $scope.institutionDetails.mI_Address2 != "") {
                                instuteAddress = instuteAddress + ',' + $scope.institutionDetails.mI_Address2;
                            }

                            if ($scope.institutionDetails.mI_Address3 != null && $scope.institutionDetails.mI_Address3 != "") {
                                instuteAddress = instuteAddress + ',' + $scope.institutionDetails.mI_Address3;
                            }
                            $scope.CurrentInstuteAddress = instuteAddress;
                        }

                        if (promise.employeeDetails !== null && promise.employeeDetails.length > 0) {

                            $scope.EmployeeDis = true;
                            $scope.employeeDetails = promise.employeeDetails;

                            angular.forEach($scope.employeeDetails, function (arr) {
                                arr.abc = "C";
                                arr.hreS_Month = $scope.selectedMonth + $scope.selectedYear + " Salary";
                                arr.bankAcNumber = Number(arr.bankAcNumber);
                            });

                            $scope.gridOptions.data = $scope.employeeDetails;

                            var totalgrp = 0;
                            var not_paid_list = [];
                            var temp = [];
                            angular.forEach($scope.employeeDetails, function (cls) {
                                if (cls != null) {
                                    not_paid_list.push(cls);
                                    //  $scope.totalgrp += parseInt(cls.netSalary);
                                }
                                else {
                                    temp.push(cls);
                                }
                            });
                            $scope.employeeDetails = not_paid_list;
                            angular.forEach($scope.employeeDetails, function (ty) {
                                totalgrp += parseInt(ty.netSalary);
                            });

                            var x = totalgrp;
                            x = x.toString();
                            var lastThree = x.substring(x.length - 3);
                            var otherNumbers = x.substring(0, x.length - 3);
                            if (otherNumbers != '')
                                lastThree = ',' + lastThree;
                            var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
                            // alert(res);
                            $scope.totalgrp = res;
                            // $scope.NetAmountInwords = toWords($scope.totalgrp) + 'only)';


                            //KINDO IMPLEMENT
                            $scope.coltotalarray = [];
                            $scope.coltotalarray.push({ name: 'netSalary', field: 'netSalary', aggregate: "sum" });

                            $scope.colarrayall = [];
                            $scope.colarrayall = [
                                { title: 'Sl.No', template: "<span class='row-number'></span>", width: 100 },
                                { name: 'bankAcNumber', field: 'bankAcNumber', title: 'Account #', width: 200 },
                                { name: 'abc', field: 'abc', title: 'Credit', width: 200 },
                                { name: 'netSalary', field: 'netSalary', title: 'Amount', width: 200, aggregates: ["sum"], footerTemplate: "#=sum#", groupFooterTemplate: " #=sum#" },
                                { name: 'hreS_Month', field: 'hreS_Month', title: 'Naration', width: 200 }
                            ];

                            $(document).ready(function () {
                                $('#kindogridBankCash').empty();
                                $("#kindogridBankCash").kendoGrid({
                                    toolbar: ["excel"],

                                    excel: {
                                        fileName: "BankCashReport.csv",
                                        //allPages: true,
                                        proxyURL: "",
                                        filterable: true,
                                        allPages: true
                                    },

                                    excelExport: function (e) {
                                        var sheet = e.workbook.sheets[0];
                                        sheet.frozenRows = 2;
                                        sheet.name = "BankCashReport";

                                        var myHeaders = [{
                                            value: "BankCashReport",
                                            fontSize: 20,
                                            textAlign: "center",
                                            background: "#ffffff",
                                            color: "black"
                                        }];

                                        sheet.rows.splice(0, 0, { cells: myHeaders, type: "header", height: 70 });
                                    },

                                    dataSource: {
                                        data: $scope.employeeDetails,
                                        pageSize: 500,
                                        aggregate: $scope.coltotalarray
                                    },

                                    sortable: true,
                                    // pageable: true,
                                    groupable: false,
                                    filterable: true,
                                    columnMenu: true,
                                    reorderable: true,
                                    resizable: true,

                                    columns: $scope.colarrayall,
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
                                });
                            });
                            //KINDO IMPLEMENT

                        }
                        else {
                            $scope.EmployeeDis = false;
                            swal('No Record found to display .. !');
                            return;
                        }
                    });
            }
        };

        //By Employee Type
        $scope.GetEmployeeBygroupTypeAll = function (groupTypeselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.groupTypeselectedAll;
            angular.forEach($scope.groupTypedropdown, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_depts();
        };

        //single
        $scope.GetEmployeeBygroupType = function (groupType) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.groupTypeselectedAll = $scope.groupTypedropdown.every(function (itm) {

                return itm.selected;
            });
            $scope.get_depts();
        };

        $scope.get_depts = function () {
            var ids = [];
            angular.forEach($scope.groupTypedropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.hrmgT_Id);
                }
            });
            var data = {
                hrmgT_IdList: ids
            };
            apiService.create("CumulativeSalaryReport/get_depts", data).
                then(function (promise) {
                    if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                        $scope.departmentdropdown = promise.departmentdropdown;
                        $scope.departmentselectedAll = true;
                        $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                    }
                });
        };

        //By Department
        $scope.GetEmployeeByDepartmentAll = function (departmentselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.departmentselectedAll;
            angular.forEach($scope.departmentdropdown, function (itm) {
                itm.selected = toggleStatus;

            });
        };

        //By Department Single
        $scope.GetEmployeeByDepartment = function (department) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {

                return itm.selected;
            });
        };

        //By Designation
        $scope.GetEmployeeByDesignationAll = function (designationselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.designationselectedAll;
            angular.forEach($scope.designationdropdown, function (itm) {
                itm.selected = toggleStatus;
            });
        };

        //By Designation Single
        $scope.GetEmployeeByDesignation = function (designation) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {

                return itm.selected;
            });
        };

        //Clear data
        $scope.Employee = {};
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.Employee.AllOrIndividual = "All";
            $scope.employeeDetails = [];
            $scope.submitted = false;
            $scope.groupTypeselectedAll = false;
            $scope.departmentselectedAll = false;
            $scope.designationselectedAll = false;
            $scope.employeeSelectedAll = false;
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.search = "";
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.onLoadGetData();
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.disableGrid = function () {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
        };

        //$scope.printData = function () {
        //    var divToPrint = document.getElementById("Table");
        //        var newWin = window.open();
        //        newWin.document.write(divToPrint.outerHTML);
        //        newWin.print();
        //        newWin.close();
        //        // $state.reload();
        //    }

        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function () {
            var divToPrint = document.getElementById("BankCash");
            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            saveAs(blob, "Report.xls");
        };

        $scope.gridOptions = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'Sl.No.', field: 'name', enableColumnMenu: false, enableFiltering: false, enableHiding: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'bankAcNumber', displayName: 'Account #', enableHiding: false },
                { name: 'abc', displayName: 'Credit', enableHiding: false },
                { name: 'netSalary', displayName: 'Amount', enableHiding: false },
                { name: 'hreS_Month', displayName: 'Naration', enableHiding: false }
            ],
            exporterCsvFilename: 'BankReport.csv',
            exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        $scope.exportCsv = function () {
            var grid = $scope.gridApi.grid;
            var rowTypes = uiGridExporterConstants.ALL;
            var colTypes = uiGridExporterConstants.ALL;
            uiGridExporterService.csvExport(grid, rowTypes, colTypes);
        };
    }
})();