
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_ItemConsumptionReportController', INV_ItemConsumptionReportController);
    INV_ItemConsumptionReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter'];
    function INV_ItemConsumptionReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {
        $scope.obj = {};
        $scope.tablediv = false;
        $scope.printIC = false;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));


        //==========================================================

        $scope.onclickchange = function () {
            $scope.typeflag = 'All';
        }
        //===========================
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var data = {
                "optionflag": $scope.optionflag,
                "MI_Id_new": $scope.MI_Id
            };
            apiService.create("INV_ItemConsumptionReport/getloaddata", data).
                then(function (promise) {
                    $scope.get_ICreportdetails = promise.get_ICreportdetails;
                   
                    $scope.staffcheck = promise.staffcheck;
                    if ($scope.MI_Id == undefined) {
                        $scope.get_mi_list = promise.get_mi_list;
                    }
                   

                    if ($scope.staffcheck == 'Staff') {
                        $scope.MI_Id = promise.get_mi_list[0].MI_Id;
                    }

                  
                });
        };

        $scope.typerdochange = function (typeflag) {
            $scope.optionflag = "Item";            
            $scope.get_allReport = "";
            $scope.get_supplierReport = "";
            $scope.get_itemReport = "";
            $scope.get_staffReport = "";
            $scope.get_departmentReport = "";
            $scope.get_studentReport = "";
            $scope.startdate = "";
            $scope.enddate = "";
            $scope.tflag = typeflag;
            $scope.loaddata($scope.tflag,$scope.MI_Id);
        };
        $scope.onrdochange = function (optionflag) {
            $scope.get_allReport = "";
            $scope.get_supplierReport = "";
            $scope.get_itemReport = "";
            $scope.get_staffReport = "";
            $scope.get_departmentReport = "";
            $scope.get_studentReport = "";
            $scope.startdate = "";
            $scope.enddate = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag);
        };

        //========================STAFF
        $scope.togchkbxST = function () {
            $scope.stall = $scope.get_ICreportdetails.every(function (str) {
                return str.stck;
            });
        };
        $scope.isOptionsRequired = function () {
            if ($scope.optionflag === 'Staff') {
                return !$scope.get_ICreportdetails.some(function (options) {
                    return options.stck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkST = function (stri) {
            $scope.stall = stri;
            var toggleStatus = $scope.stall;
            angular.forEach($scope.get_ICreportdetails, function (st) {
                st.stck = toggleStatus;
            });
        };

        //========================SUPPLIER
        $scope.togchkbxSupp = function () {
            $scope.supplier = $scope.get_ICreportdetails.every(function (str) {
                return str.supp;
            });
        };
        $scope.isOptionsRequired = function () {
            if ($scope.optionflag === 'Supplier') {
                return !$scope.get_ICreportdetails.some(function (options) {
                    return options.supp;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkSupp = function (stri) {
            $scope.supplier = stri;
            var toggleStatus = $scope.supplier;
            angular.forEach($scope.get_ICreportdetails, function (st) {
                st.supp = toggleStatus;
            });
        };

        //==================================IC Report
        $scope.submitted = false;
        $scope.onreport = function () {
            var data = {};
           // if ($scope.myForm.$valid) {
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

                if ($scope.typeflag === "All") {
                    data = {
                        "typeflag": $scope.typeflag,
                        "optionflag": "",
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "INVMI_Id": 0,
                        "HRME_Id": 0,
                        "HRMD_Id": 0,
                        "AMST_Id": 0,
                        "bwdateflag": $scope.bw_dates,
                        "MI_Id_new": $scope.MI_Id
                    };
                }
                else if ($scope.optionflag === "Supplier") {
                    $scope.supparray = [];
                    angular.forEach($scope.get_ICreportdetails, function (sup) {
                        if (sup.supp === true) {
                            $scope.supparray.push(sup);
                        }
                    });
                    data = {
                        "typeflag": $scope.typeflag,
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "supparray": $scope.supparray,
                        "HRME_Id": 0,
                        "HRMD_Id": 0,
                        "AMST_Id": 0,
                        "bwdateflag": $scope.bw_dates,
                        "MI_Id_new": $scope.MI_Id
                    };
                }
                else if ($scope.optionflag === "Item") {
                    data = {
                        "typeflag": $scope.typeflag,
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "INVMI_Id": $scope.obj.INVMI_Id.INVMI_Id,
                        "HRME_Id": 0,
                        "HRMD_Id": 0,
                        "AMST_Id": 0,
                        "bwdateflag": $scope.bw_dates,
                        "MI_Id_new": $scope.MI_Id
                    };
                }
                else if ($scope.optionflag === "Staff") {
                    $scope.staffarray = [];
                    angular.forEach($scope.get_ICreportdetails, function (stf) {
                        if (stf.stck === true) {
                            $scope.staffarray.push(stf);
                        }
                    });
                    data = {
                        "typeflag": $scope.typeflag,
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "staffarray": $scope.staffarray,
                        "INVMI_Id": 0,
                        "HRME_Id": 0,
                        "HRMD_Id": 0,
                        "AMST_Id": 0,
                        "bwdateflag": $scope.bw_dates,
                        "MI_Id_new": $scope.MI_Id
                    };
                }

                else if ($scope.optionflag === "Department") {
                    data = {
                        "typeflag": $scope.typeflag,
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "INVMI_Id": 0,
                        "HRME_Id": 0,
                        "HRMD_Id": $scope.obj.HRMD_ID.HRMD_Id,
                        "AMST_Id": 0,
                        "bwdateflag": $scope.bw_dates,
                        "MI_Id_new": $scope.MI_Id
                    };
                }
                else if ($scope.optionflag === "Student") {
                    data = {
                        "typeflag": $scope.typeflag,
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "INVMI_Id": 0,
                        "HRME_Id": 0,
                        "HRMD_Id": 0,
                        "AMST_Id": $scope.obj.AMST_Id.AMST_Id,
                        "bwdateflag": $scope.bw_dates,
                        "MI_Id_new": $scope.MI_Id
                    };
                }
                apiService.create("INV_ItemConsumptionReport/onreport", data).
                    then(function (promise) {
                        if (promise.typeflag == "All") {
                            if (promise.get_allReport != null && promise.get_allReport.length > 0) {
                                $scope.get_allReport = promise.get_allReport;
                                $scope.presentCountgrid = $scope.get_allReport.length;
                                $scope.Grid_view = true;
                                $scope.all = true;
                                $scope.supplier = false;
                                $scope.item = false;
                                $scope.staff = false;
                                $scope.department = false; 
                                $scope.student = false;
                            }
                            else {
                                swal("No Records Found");
                                $scope.Grid_view = false;
                            }
                        }
                        else if ($scope.optionflag == "Supplier") {
                            if (promise.get_supplierReport != null && promise.get_supplierReport.length > 0) {
                                $scope.get_supplierReport = promise.get_supplierReport;
                                $scope.presentCountgrid = $scope.get_supplierReport.length;
                                $scope.Grid_view = true;
                                $scope.all = false;
                                $scope.supplier = true;
                                $scope.item = false;
                                $scope.staff = false;
                                $scope.department = false;
                                $scope.student = false;
                                
                                $scope.employeeid = [];

                                angular.forEach($scope.get_supplierReport, function (dev) {
                                    if ($scope.employeeid.length === 0) {
                                        $scope.employeeid.push({
                                            INVMS_Id: dev.INVMS_Id,
                                            INVMS_SupplierName: dev.INVMS_SupplierName,
                                            INVMI_ItemName: dev.INVMI_ItemName
                                        });
                                    } else if ($scope.employeeid.length > 0) {
                                        var intcount = 0;
                                        angular.forEach($scope.employeeid, function (emp) {
                                            if (emp.INVMS_Id === dev.INVMS_Id) {
                                                intcount += 1;
                                            }
                                        });
                                        if (intcount === 0) {
                                            $scope.employeeid.push({
                                                INVMS_Id: dev.INVMS_Id, INVMS_SupplierName: dev.INVMS_SupplierName,
                                                INVMI_ItemName: dev.INVMI_ItemName
                                            });
                                        }
                                    }
                                });

                                console.log($scope.employeeid);

                                angular.forEach($scope.employeeid, function (ddd) {
                                    $scope.templist = [];
                                    angular.forEach($scope.get_supplierReport, function (dd) {
                                        if (dd.INVMS_Id === ddd.INVMS_Id) {
                                            $scope.templist.push(dd);
                                        }
                                    });
                                    ddd.plannerdetails = $scope.templist;
                                });

                                console.log($scope.employeeid);

                                //Total Amount
                                var totamount = 0;
                                angular.forEach($scope.get_supplierReport, function (pi) {
                                    totamount += parseFloat(pi.INVMGRN_TotalAmount);
                                    $scope.totamount = totamount;
                                    $scope.totamount = parseFloat($scope.totamount);
                                });

                            }
                            else {
                                swal("No Records Found");
                                $scope.Grid_view = false;
                            }
                        }
                        else if ($scope.optionflag == "Item") {
                            if (promise.get_itemReport != null && promise.get_itemReport.length > 0) {
                                $scope.get_itemReport = promise.get_itemReport;
                                $scope.presentCountgrid = $scope.get_itemReport.length;
                                $scope.Grid_view = true;
                                $scope.all = false;
                                $scope.supplier = false;
                                $scope.item = true;
                                $scope.staff = false;
                                $scope.department = false;
                                $scope.student = false;
                            }
                            else {
                                swal("No Records Found");
                                $scope.Grid_view = false;
                            }
                        }
                        else if ($scope.optionflag == "Staff") {
                            if (promise.get_staffReport != null && promise.get_staffReport.length > 0) {
                                $scope.get_staffReport = promise.get_staffReport;
                                $scope.presentCountgrid = $scope.get_staffReport.length;
                                $scope.Grid_view = true;
                                $scope.all = false;
                                $scope.supplier = false;
                                $scope.item = false;
                                $scope.staff = true;
                                $scope.department = false;
                                $scope.student = false;
                            }
                            else {
                                swal("No Records Found");
                                $scope.Grid_view = false;
                            }
                        }
                        else if ($scope.optionflag == "Department") {
                            if (promise.get_departmentReport != null && promise.get_departmentReport.length > 0) {
                                $scope.get_departmentReport = promise.get_departmentReport;
                                $scope.presentCountgrid = $scope.get_departmentReport.length;
                                $scope.Grid_view = true;
                                $scope.all = false;
                                $scope.supplier = false;
                                $scope.item = false;
                                $scope.staff = false;
                                $scope.department = true;
                                $scope.student = false;
                            }
                            else {
                                swal("No Records Found");
                                $scope.Grid_view = false;
                            }
                        }
                        else if ($scope.optionflag == "Student") {
                            if (promise.get_studentReport != null && promise.get_studentReport.length > 0) {
                                $scope.get_studentReport = promise.get_studentReport;
                                $scope.presentCountgrid = $scope.get_studentReport.length;
                                $scope.Grid_view = true;
                                $scope.all = false;
                                $scope.supplier = false;
                                $scope.item = false;
                                $scope.staff = false;
                                $scope.department = false;
                                $scope.student = true;
                            }
                            else {
                                swal("No Records Found");
                                $scope.Grid_view = false;
                            }
                        }                      
                    });            
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cancel = function () {
            $state.reload();
        };
        //======================================Print & Export to Excel        
        $scope.exportToExcel = function (export_id) {

            var exportHref = Excel.tableToExcel(export_id, 'sheet name');
            $timeout(function () {
                location.href = exportHref;
            }, 100);

        };
        $scope.printData = function () {

            var innerContents = document.getElementById("printIC").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };       
        $scope.searchValue = '';
        $scope.searchValueSup = '';
        $scope.searchValuestaf = '';
    }
})();