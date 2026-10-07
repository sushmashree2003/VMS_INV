(function () {
    'use strict';
    angular
        .module('app')
        .controller('employeeadvanceapplyController', employeeadvanceapplyController)
    employeeadvanceapplyController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function employeeadvanceapplyController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

        // form Object
        $scope.saladvance = {};
        $scope.editflag = false;
        $scope.EntryDate = new Date();
        $scope.yearDis = true;
        $scope.monthDis = true;
        $scope.configurationDetails = {};
        $scope.empGrossSalDis = true;
        $scope.obj = {};


        // Get form Details at onload 
        $scope.onLoadGetData = function () {

            var pageid = 2;
            apiService.getURI("employeeadvanceApply/getalldetails", pageid).then(function (promise) {


                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {
                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }

                if (promise.empadvaList !== null && promise.empadvaList.length > 0) {
                    $scope.gridOptions = promise.empadvaList;
                }



                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;
                }

                if (promise.modeOfPaymentdropdown !== null && promise.modeOfPaymentdropdown.length > 0) {
                    $scope.modeOfPaymentdropdown = promise.modeOfPaymentdropdown;
                }
                if (promise.empGrossSal != 0 && promise.empGrossSal != null) {
                    $scope.saladvance.empGrossSal = promise.empGrossSal;
                }

                angular.forEach($scope.gridOptions, function (value, key) {
                    var fdate = value.hresA_EntryDate.split('T');
                    value.hresA_EntryDate = fdate[0];
                });

                if (promise.configurationDetails !== null) {
                    $scope.configurationDetails = promise.configurationDetails;
                    var date = new Date();
                    var year = date.getFullYear();
                    var montha = date.getMonth();

                    var out = $scope.monthdropdown.filter(function (x) {
                        var match = x.ivrM_Month_Id == montha;

                        return match;
                    });


                    var yearId = parseInt(year);
                    var monthId = parseInt(out[0].ivrM_Month_Id);
                    var startDay = parseInt($scope.configurationDetails.hrC_SalaryFromDay);
                    var endDay = parseInt($scope.configurationDetails.hrC_SalaryToDay);


                    if (startDay > 1 && monthId < 12) {

                        $scope.minDatedoi = new Date(
                            yearId,
                            monthId - 1,
                            startDay);

                        $scope.maxDatedoi = new Date(
                            yearId,
                            monthId,
                            endDay);


                    } else if (startDay > 1 && monthId == 12) {

                        $scope.minDatedoi = new Date(
                            yearId,
                            monthId - 1,
                            startDay);

                        $scope.maxDatedoi = new Date(
                            yearId + 1,
                            0,
                            endDay);


                    } else {

                        var days = getNumberOfDays(yearId, monthId);

                        $scope.minDatedoi = new Date(
                            yearId,
                            monthId - 1,
                            startDay);

                        $scope.maxDatedoi = new Date(
                            yearId,
                            monthId - 1,
                            days);
                    }
                }
            })
        }


        // clear form data
        $scope.cancel = function () {
            $state.reload();

        }
        

        //saving/updating Record
        $scope.submitted = false;
        $scope.saveData = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {

                if ($scope.saladvance.hresA_EntryDate != undefined && $scope.saladvance.hresA_EntryDate != "") {
                    $scope.saladvance.hresA_EntryDate = new Date($scope.saladvance.hresA_EntryDate).toDateString();
                }

                else {
                    $scope.saladvance.hresA_EntryDate = null;
                }

                var data = {
                    "HRESA_Id": $scope.saladvance.hresA_Id,
                    "HRESA_EntryDate": $scope.saladvance.hresA_EntryDate,
                    "HRESA_AdvMonth": $scope.saladvance.hresA_AdvMonth,
                    "HRESA_AdvYear": $scope.saladvance.hresA_AdvYear,
                    "HRESA_AppliedAmount": $scope.saladvance.hresA_AppliedAmount,
                    "HRESA_ModeOfPayment": $scope.saladvance.hresA_ModeOfPayment,
                    "HRESA_Remarks": $scope.saladvance.hresA_Remarks,
                    empGrossSal: $scope.saladvance.empGrossSal
                }
                apiService.create("employeeadvanceApply/", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {

                            if (promise.retrunMsg == "Duplicate") {
                                swal("Advance already applied...!", 'Kindly contact Administrator');
                                return;
                            }

                            else if (promise.retrunMsg == "false") {
                                swal("Record Not saved / Updated..", 'Fail');

                            }
                            else if (promise.retrunMsg == "Add") {
                                swal("Record Saved Successfully..");
                                $state.reload();
                            }
                            else if (promise.retrunMsg == "Update") {
                                swal("Record Updated Successfully..");
                                $state.reload();
                            }
                            else if (promise.retrunMsg == "acc") {
                                swal("Account No. is already exist..");
                                return;
                            } else if (promise.retrunMsg == "Not Mapped") {
                                swal("Salary Advance not mapped");
                                return;
                            }
                            else if (promise.retrunMsg == "AdvancePending") {
                                swal("Pervious advance pending...!",'So Your Not Eligible To Apply Advance');
                                return;
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }

                            angular.forEach($scope.gridOptions, function (value, key) {
                                var fdate = value.hresA_EntryDate.split('T');
                                value.hresA_EntryDate = fdate[0];

                            });

                            $scope.cancel();
                        }
                    })
            }
            else {
                swal("You can't apply advance for pervious date ..!", 'Kindly Check Request date');
                return;
            }
        };
        $scope.Viewapprovaldetails = function (item) {
            var data = {
                "HRESA_Id": item.hresA_Id,
            };

            apiService.create("employeeadvanceApply/Edit", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.advanceapproval !== null && promise.advanceapproval.length > 0) {
                        $scope.advanceapproval = promise.advanceapproval;
                    }
                }

            })
        }
        $scope.Edit = function (item) {
            var data = {
                "HRESA_Id": item.hresA_Id,
            };

            apiService.create("employeeadvanceApply/Edit", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.advanceapproval !== null && promise.advanceapproval.length > 0) {
                        $scope.advanceapproval = promise.advanceapproval;
                    }
                    $scope.empadvaList = promise.empadvaList;
                    $scope.saladvance.totalAppliedAmount = promise.totalAppliedAmount;
                    $scope.editflag = true;
                    $scope.saladvance.hresA_Id = promise.empadvaList[0].hresA_Id;
                    $scope.saladvance.hresA_AdvYear = promise.empadvaList[0].hresA_AdvYear;
                    $scope.saladvance.hresA_EntryDate = new Date(promise.empadvaList[0].hresA_EntryDate);
                    $scope.saladvance.hresA_AdvMonth = promise.empadvaList[0].hresA_AdvMonth;
                    $scope.saladvance.hresA_AppliedAmount = promise.empadvaList[0].hresA_AppliedAmount;
                    $scope.saladvance.hresA_ModeOfPayment = promise.empadvaList[0].hresA_ModeOfPayment;
                    $scope.saladvance.hresA_Remarks = promise.empadvaList[0].hresA_Remarks;
                    $scope.saladvance.hresA_ReferenceNo = promise.empadvaList[0].hresA_ReferenceNo;
                }
            })
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.setMonthAndyear = function (date) {
            $scope.saladvance.hresA_AdvYear = "";
            $scope.saladvance.hresA_AdvMonth = "";
            var year = date.getFullYear();
            var montha = date.getMonth() + 1;
            var month = $filter('filter')($scope.monthdropdown, function (a) {
                return a.ivrM_Month_Id === montha;
            })[0].ivrM_Month_Name;
            $scope.saladvance.hresA_AdvYear = year;
            $scope.saladvance.hresA_AdvMonth = month;
            $scope.GetDetailsByEmployee();
        }

        $scope.GetDetailsByEmployee = function () {

            var data = $scope.saladvance;

            apiService.create("employeeadvanceApply/getDetailsByEmployee", data).
                then(function (promise) {

                    $scope.saladvance.totalAppliedAmount = promise.totalAppliedAmount;

                    if ($scope.saladvance.hresA_AppliedAmount != "" && $scope.saladvance.hresA_AppliedAmount != undefined) {

                        var maxminAmout = parseFloat($scope.saladvance.empGrossSal).toFixed(2) - parseFloat($scope.saladvance.totalAppliedAmount).toFixed(2);
                        if (maxminAmout > 0) {
                            if ($scope.saladvance.hresA_AppliedAmount > maxminAmout) {

                                swal('Already Issued Advance of ' + $scope.saladvance.totalAppliedAmount + ' Rupees. So Advance Amount Should Be Less than ' + maxminAmout + ' Rupees. ');
                                $scope.saladvance.hresA_AppliedAmount = "";
                            }
                        } else {
                            swal('Already Issued Advance of ' + $scope.saladvance.totalAppliedAmount + ' Rupees. So Advance Amount Should Be Less than ' + maxminAmout + ' Rupees. ');
                            $scope.saladvance.hresA_AppliedAmount = "";
                        }

                    }
                })

        };
        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hresA_ActiveFlag == false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Record..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("employeeadvanceApply/ActiveDeactiveRecord", data.hresA_Id).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Activated") {
                                        swal("Record Activated successfully");
                                        $state.reload();
                                    }
                                    else if (promise.retrunMsg === "Deactivated") {
                                        swal("Record Deactivated successfully");
                                        $state.reload();
                                    }
                                    else {
                                        swal("Record Not Activated/Deactivated", 'Fail');
                                    }
                                    if (promise.empadvaList !== null && promise.empadvaList.length > 0) {
                                        $scope.gridOptions.data = promise.empadvaList;
                                        angular.forEach($scope.gridOptions.data, function (value, key) {
                                            var fdate = value.hresA_EntryDate.split('T');
                                            value.hresA_EntryDate = fdate[0];

                                        });
                                    }
                                }

                            })
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }

            );
        }
    }
})();