(function () {
    'use strict';
    angular
        .module('app')
        .controller('DriverIndentController', DriverIndentController);

    DriverIndentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', 'uiGridGroupingConstants'];
    function DriverIndentController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, uiGridGroupingConstants) {

        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        //$scope.imgname = logopath;

        $scope.search = '';
        $scope.submitted = false;
        $scope.filtervalue1 = function (user) {
        };

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("DriverIndent/loaddata", pageid).then(function (promise) {
                $scope.alldata = promise.alldata;
                
                $scope.vehicle_list = promise.vehicle_list;
                $scope.imgname = promise.mI_logo;
            });
        };

        $scope.ISMDIT_Date = new Date();// today's date
        $scope.checkedVehicleno = function () {
            $scope.ISMDIT_OpeningKM = "";
            var data = {
                "TRMV_Id": $scope.TRMV_Id
            };
            apiService.create("DriverIndent/checkedVehicleno", data).then(function (promise) {
                if (promise.vehicledetails !== null && promise.vehicledetails.length > 0) {
                    $scope.vehicledetails = promise.vehicledetails;
                    $scope.ISMDIT_OpeningKM = $scope.vehicledetails[0].ismdiT_ClosingKM;
                }

            });
        };
        //============== save
        $scope.save = function () {
            if ($scope.myForm.$valid) {
                var date = $scope.ISMDIT_Date === null ? "" : $filter('date')($scope.ISMDIT_Date, "yyyy-MM-dd");

                var data = {
                    "ISMDIT_Id": $scope.ISMDIT_Id,
                    "ISMDIT_Date": $scope.ISMDIT_Date,
                    "TRMV_Id": $scope.TRMV_Id,
                    "ISMDIT_BillNo": $scope.ISMDIT_BillNo,
                    "ISMDIT_Qty": $scope.ISMDIT_Qty,
                    "ISMDIT_Amount": $scope.ISMDIT_Amount,
                    "ISMDIT_Remarks": $scope.ISMDIT_Remarks,
                    "ISMDIT_OpeningKM": $scope.ISMDIT_OpeningKM,
                    "ISMDIT_ClosingKM": $scope.ISMDIT_ClosingKM,
                    "ISMDIT_BalanceDiesel": $scope.ISMDIT_BalanceDiesel
                };

                apiService.create("DriverIndent/save", data).then(function (promise) {
                    if (promise.msg === 'Saved') {
                        swal("Request Saved Successfully.....!!!!!");
                        $state.reload();
                    }
                    else if (promise.msg === 'updated') {
                        swal("Request Updated Successfully.....!!!!!");
                        $state.reload();
                    }
                    else if (promise.msg === 'Failed') {
                        swal("Request  Not Saved .....!!!!!");
                    }
                    else
                    {
                        swal("Request  Not Saved , Please Contact Administrator");
                    }
                   
                });
            }
            else {
                $scope.submitted = true;
            }  
        };

        $scope.edittab1 = function (user) {
            var data = {
                "ISMDIT_Id": user.ismdiT_Id                
            };

            apiService.create("DriverIndent/edittab1", data).then(function (promise) {
                $scope.imgname = promise.mI_logo;
                $scope.ISMDIT_Id = promise.editlist[0].ismdiT_Id;
                $scope.ISMDIT_Date = new Date(promise.editlist[0].ismdiT_Date);
                $scope.TRMV_Id = promise.editlist[0].trmV_Id;
                $scope.ISMDIT_BillNo = promise.editlist[0].ismdiT_BillNo;
                $scope.ISMDIT_Qty = promise.editlist[0].ismdiT_Qty;
                $scope.ISMDIT_Amount = promise.editlist[0].ismdiT_Amount;
                $scope.ISMDIT_Remarks = promise.editlist[0].ismdiT_Remarks;
                $scope.ISMDIT_OpeningKM = promise.editlist[0].ismdiT_OpeningKM;
                $scope.ISMDIT_ClosingKM = promise.editlist[0].ismdiT_ClosingKM;
                $scope.ISMDIT_BalanceDiesel = promise.editlist[0].ismdiT_BalanceDiesel;
                $scope.TrmV_VehicleNo = promise.editlist[0].vehicleno;
            });
        };

        $scope.printtab = function (user) {
            var data = {
                "ISMDIT_Id": user.ismdiT_Id
            };
            apiService.create("DriverIndent/edittab1", data).then(function (promise) {
                $scope.ISMDIT_Id = promise.editlist[0].ismdiT_Id;
                $scope.ISMDIT_Date = new Date(promise.editlist[0].ismdiT_Date);
                $scope.TRMV_Id = promise.editlist[0].trmV_Id;
                $scope.ISMDIT_BillNo = promise.editlist[0].ismdiT_BillNo;
                $scope.ISMDIT_Qty = promise.editlist[0].ismdiT_Qty;
                $scope.ISMDIT_Amount = promise.editlist[0].ismdiT_Amount;
                $scope.ISMDIT_Remarks = promise.editlist[0].ismdiT_Remarks;
                $scope.ISMDIT_OpeningKM = promise.editlist[0].ismdiT_OpeningKM;
                $scope.ISMDIT_ClosingKM = promise.editlist[0].ismdiT_ClosingKM;
                $scope.ISMDIT_BalanceDiesel = promise.editlist[0].ismdiT_BalanceDiesel;
                $scope.TrmV_VehicleNo = promise.editlist[0].vehicleno;
                $scope.hrmE_EmployeeFirstName = promise.editlist[0].hrmE_EmployeeFirstName;
                $scope.mI_Logo = promise.editlist[0].mI_Logo;
            });
        };

        $scope.Clearid = function () {
            $scope.ISMDIT_Id = "";
            $scope.ISMDIT_Date = "";
            $scope.TRMV_Id = "";
            $scope.ISMDIT_BillNo = "";
            $scope.ISMDIT_Qty = "";
            $scope.ISMDIT_Amount = "";
            $scope.ISMDIT_Remarks = "";
            $scope.ISMDIT_OpeningKM = "";
            $scope.ISMDIT_ClosingKM = "";
            $scope.ISMDIT_BalanceDiesel = "";
            $scope.TrmV_VehicleNo = "";
            $state.reload();
        };

        $scope.printData = function (Employee) {
            var innerContents = document.getElementById("indenttemplate").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/EmpPaySlipPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
    }   
})();


