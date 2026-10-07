
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_VendorPayment_ReportDateWiseController', INV_VendorPayment_ReportDateWiseController);
    INV_VendorPayment_ReportDateWiseController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter']
    function INV_VendorPayment_ReportDateWiseController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {


        $scope.obj = {};
        var paginationformasters;
        $scope.optionflag = "Supplier";
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
       // $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        $scope.ngchange = function () {
            $scope.optionflag = false;
            $scope.loaddata($scope.MI_Id);
            
        }
        //==========================================================
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var data = {
                "optionflag": $scope.optionflag,
                "MI_Id_new":$scope.MI_Id
            };
            apiService.create("INV_VendorPayment_Report/getloaddata", data).
                then(function (promise) {
                    $scope.staffcheck = promise.staffcheck;
                    if ($scope.staffcheck == 'Staff') {
                        $scope.MI_Id = promise.get_mi_list[0].MI_Id;
                        $scope.get_VPReport_Details = promise.get_VPReport_Details;

                    }
                    else {
                        $scope.get_VPReport_Details = promise.get_VPReport_Details;
                        if ($scope.get_mi_list == null) {
                            $scope.get_mi_list = promise.get_mi_list;
                        }
                      
                    }
                    
                });
        };

        $scope.onrdochange = function (optionflag) {
            $scope.getallreport = [];
            $scope.supplierreport = [];
            $scope.grnreport = [];
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag);
        };

        //==================================PI Report
        $scope.submitted = false;
        $scope.onreport = function () {

            if ($scope.myForm.$valid) {

                $scope.from_date = new Date($scope.FMCB_fromDATE).toDateString();
                $scope.to_date = new Date($scope.FMCB_toDATE).toDateString();

                var data = {};               
                if ($scope.optionflag === "All") {   
                    data = {
                        "optionflag": $scope.optionflag,
                        "INVMS_Id":0,
                        "INVMGRN_Id": 0,
                        "MI_Id_new": $scope.MI_Id,
                        "Fromdate": $scope.from_date,
                        "Todate": $scope.to_date,
                    };                   
                }
                else if ($scope.optionflag === "Supplier") {
                    data = {
                        "optionflag": $scope.optionflag,
                        "INVMS_Id": $scope.obj.invmS_Id.INVMS_Id,
                        "INVMGRN_Id": 0,
                        "MI_Id_new": $scope.MI_Id,
                        "Fromdate": $scope.from_date,
                        "Todate": $scope.to_date,
                    };                    
                }
                else if ($scope.optionflag === "GRN") {
                    data = {
                        "optionflag": $scope.optionflag,
                        "INVMS_Id": 0,
                        "INVMGRN_Id": $scope.obj.invmgrN_Id.INVMGRN_Id,
                        "MI_Id_new": $scope.MI_Id,
                        "Fromdate": $scope.from_date,
                        "Todate": $scope.to_date,
                    };                    
                }
                
                apiService.create("INV_VendorPayment_Report/ondatereport", data).
                    then(function (promise) {
                        $scope.imgname = promise.logopath;
                        if ($scope.optionflag == "All") {
                            if (promise.getallreport != null && promise.getallreport.length > 0) {
                                $scope.getallreport = promise.getallreport;
                                $scope.presentCountgrid = $scope.getallreport.length;
                                $scope.Grid_view = true;
                                $scope.all = true;
                                $scope.supplier = false;
                                $scope.grn = false;

                                var totalamt = 0;
                                var totalapproved = 0;
                                var totalbalance = 0;
                                angular.forEach($scope.getallreport, function (pi) {
                                    totalamt += parseFloat(pi.VPAYVOU_AppliedAmount);
                                    totalapproved += parseFloat(pi.VPAYVOU_ApprovedAmount);
                                    totalbalance += parseFloat(pi.VPAYVOU_BankBalalnce);
                                    $scope.totalamt = totalamt;
                                    $scope.totalapproved = totalapproved;
                                    $scope.totalbalance = totalbalance;
                                });
                            }
                            else {
                                swal("No Record Found...!!");
                                $scope.Grid_view = false;
                            }
                        }
                        else if ($scope.optionflag == "Supplier") {
                            if (promise.supplierreport != null && promise.supplierreport.length > 0) {
                                $scope.supplierreport = promise.supplierreport;
                                $scope.presentCountgrid = $scope.supplierreport.length;
                                $scope.Grid_view = true;
                                $scope.all = false;
                                $scope.supplier = true;
                                $scope.grn = false;    

                                var totalamt = 0;
                                var totalapproved = 0;
                                var totalbalance = 0;
                                angular.forEach($scope.supplierreport, function (pi) {
                                    totalamt += parseFloat(pi.VPAYVOU_AppliedAmount);
                                    totalapproved += parseFloat(pi.VPAYVOU_ApprovedAmount);
                                    totalbalance += parseFloat(pi.VPAYVOU_BankBalalnce);
                                    $scope.totalamt = totalamt;
                                    $scope.totalapproved = totalapproved;
                                    $scope.totalbalance = totalbalance;
                                });
                            }
                            else {
                                swal("No Record Found...!!");
                                $scope.Grid_view = false;
                            }
                        }
                        else if ($scope.optionflag == "GRN") {
                            if (promise.grnreport != null && promise.grnreport.length > 0) {
                                $scope.grnreport = promise.grnreport;
                                $scope.presentCountgrid = $scope.grnreport.length;
                                $scope.Grid_view = true;
                                $scope.all = false;
                                $scope.supplier = false;
                                $scope.grn = true;

                                var totalamt = 0;
                                var totalapproved = 0;
                                var totalbalance = 0;
                                angular.forEach($scope.grnreport, function (pi) {
                                    totalamt += parseFloat(pi.VPAYVOU_AppliedAmount);
                                    totalapproved += parseFloat(pi.VPAYVOU_ApprovedAmount);
                                    totalbalance += parseFloat(pi.VPAYVOU_BankBalalnce);
                                    $scope.totalamt = totalamt;
                                    $scope.totalapproved = totalapproved;
                                    $scope.totalbalance = totalbalance;
                                });
                            }
                            else {
                                swal("No Record Found...!!");
                                $scope.Grid_view = false;
                            }
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cancel = function () {
            $state.reload();
        };
        //======================================Print & Export to Excel
        //$scope.exportToExcel = function (export_id) {

        //    var exportHref = Excel.tableToExcel(export_id, 'printpr');
        //    $timeout(function () {
        //        location.href = exportHref;
        //    }, 100);
        //};
        $scope.exportToExcel = function (export_id) {

            var exportHref = Excel.tableToExcel(export_id, 'sheet name');
            $timeout(function () {
                location.href = exportHref;
            }, 100);

        };
        $scope.printData = function () {

            var innerContents = document.getElementById("printpm").innerHTML;
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
    }
})();