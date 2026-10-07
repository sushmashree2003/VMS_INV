(function () {
    'use strict';
    angular
        .module('app')
        .controller('VMS_SalaryCheck_ApprovalController', VMS_SalaryCheck_ApprovalController);

    VMS_SalaryCheck_ApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter'];
    function VMS_SalaryCheck_ApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter) {

        $scope.addjob = {};
        $scope.EmployeeDis = false;
        $scope.submitted = false;

        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("VMS_SalaryCheck_Approval/approvalloaddata", pageid).then(function (promise) {
                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {
                    $scope.yearList = promise.leaveyeardropdown;
                }

                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthList = promise.monthdropdown;
                }
            });
        };

        $scope.cancel = function () {
            $scope.chklst = "";
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted;
        };

        $scope.cleardata = function () {
            $scope.EmployeeDis = false;
        };

        $scope.EditData = function (record) {
            var id = record.hrcisC_Id;
            apiService.getURI("VMS_SalaryCheck_Approval/editRecord", id).
                then(function (promise) {

                });
        };

        $scope.savejob = function () {
            if ($scope.myForm1.$valid) {
                var data = {
                    "HRMLY_LeaveYear": $scope.hrmlY_LeaveYear,
                    "IVRM_Month_Name": $scope.ivrM_Month_Name
                };
                apiService.create("VMS_SalaryCheck_Approval/findchecklist", data).
                    then(function (promise) {
                        if (promise.main_list !== null && promise.main_list.length > 0) {
                            $scope.employeeDetails = promise.main_list;
                            $scope.EmployeeDis = true;

                            angular.forEach($scope.employeeDetails, function (ver) {
                                if (ver.employeeEarningsDeductionsDetails != null && ver.employeeEarningsDeductionsDetails.length != 0) {
                                    var items = ver.employeeEarningsDeductionsDetails;

                                    var getPortion = function (label) {
                                        var sum = 0;
                                        var out = items.filter(function (x) {
                                            var match = x.HRMED_EarnDedFlag == label;
                                            if (match)
                                                sum += x.Amount;
                                            return match;
                                        });
                                        return { out, sum };
                                    };

                                    var es = getPortion('Earning');
                                    var ds = getPortion('Deduction');
                                    ver.esary = es;
                                    ver.dsary = ds;
                                    if (es.out.length >= ds.out.length) {
                                        for (var item of ds.out)
                                            es.out[ds.out.indexOf(item)].ds = item;
                                        ver.items = es.out;
                                    }
                                    else {
                                        for (var item of es.out)
                                            ds.out[es.out.indexOf(item)].ds = item;
                                        ver.items = ds.out;
                                    }

                                    ver.totals = [es.sum, ds.sum];

                                    var LopAmount = 0;
                                    //Employee Salary Details
                                    ver.empsaldetail = ver.empsaldetail;
                                    ver.hrmE_Id = ver.employeedetailList[0].hrmE_Id;

                                    ver.hrmE_EmployeeFirstName = ver.employeedetailList[0].hrmE_EmployeeFirstName;
                                    ver.hrmD_DepartmentName = ver.employeedetailList[0].hrmD_DepartmentName;
                                    ver.hrmdeS_DesignationName = ver.employeedetailList[0].hrmdeS_DesignationName;

                                    ver.selected = true;
                                    ver.hrespdeT_Percentage = 100;
                                    ver.NetSalary = Math.round((Math.ceil(parseFloat(ver.totals[0]) - parseFloat(ver.totals[1]))) - LopAmount);
                                    ver.revisedSalary = ver.NetSalary;
                                }
                                //ver.selected = true;
                                //ver.hrespdeT_Percentage = 100;
                                //ver.hrespdeT_Remarks = ver.hrespdeT_Remarks;
                            });

                            console.log($scope.employeeDetails);
                        }
                        else {
                            swal("No Item Found !!");
                            return;
                        }
                    });
            }
            else { $scope.submitted = true; }
        };

        $scope.saveapproval = function () {
            if ($scope.myForm.$valid) {

                $scope.temp_array2 = [];
                for (var x = 0; x < $scope.employeeDetails.length; x++) {
                    if ($scope.employeeDetails[x].selected == true) {
                        var newCol = { HRME_Id: $scope.employeeDetails[x].hrmE_Id, revisedSalary: $scope.employeeDetails[x].revisedSalary, HRESPDET_Remarks: $scope.employeeDetails[x].hrespdeT_Remarks, HRESPDET_Percentage: $scope.employeeDetails[x].hrespdeT_Percentage};
                        $scope.temp_array2.push(newCol);
                    }
                }

                var data = {
                    "HRMLY_LeaveYear": $scope.hrmlY_LeaveYear,
                    "IVRM_Month_Name": $scope.ivrM_Month_Name,
                    "ApprovalTempDTO": $scope.temp_array2
                };

                apiService.create("VMS_SalaryCheck_Approval/saveapproval", data).
                    then(function (promise) {

                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                                $state.reload();
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                                $scope.cancel();
                            }
                            else if (promise.retrunMsg === "Notification") {
                                swal("Notification!");
                                $scope.cancel();
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                            }
                        }
                    });
            }            
        };

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

        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'CheckListDetails');
            $timeout(function () { location.href = exportHref; }, 100);
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };

        $scope.revisedSalary = function (ff) {
            ff.revisedSalary = Math.round((ff.hrespdeT_Percentage / 100) * ff.NetSalary);
        };
    }

})();