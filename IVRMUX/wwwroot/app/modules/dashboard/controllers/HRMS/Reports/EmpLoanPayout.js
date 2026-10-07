(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmpLoanPayoutController', EmpLoanPayoutController)

    EmpLoanPayoutController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function EmpLoanPayoutController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

       
        $scope.loan = {};
        $scope.employeedet = false;
        $scope.empchange = false;


     

        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("EmpLoanPayout/getalldetails", pageid).then(function (promise) {


                if (promise.gmasterloanList !== null && promise.gmasterloanList.length > 0) {
                
                    $scope.gmasterloanList = promise.gmasterloanList;
                }
                if (promise.employeelist !== null && promise.employeelist.length > 0) {
                    $scope.employeelist = promise.employeelist;
                }
                if (promise.modeOfPaymentdropdown !== null && promise.modeOfPaymentdropdown.length > 0) {
                    $scope.modeOfPaymentdropdown = promise.modeOfPaymentdropdown;
                }

            })
        }

        //// Sort table data
        //$scope.sort = function (keyname) {
        //    $scope.sortKey = keyname;   //set the sortKey to the param passed
        //    $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        //}

        // clear form data
        $scope.cancel = function () {
            // $scope.search = "";
            $state.reload();
        }

        //saving/updating Record
        $scope.submitted = false;
        $scope.saveData = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.loan.HRELDIS_DisbursedDate = new Date($scope.loan.HRELDIS_DisbursedDate);
                var data = $scope.loan;

                apiService.create("EmpLoanPayout/", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {

                            if (promise.retrunMsg == "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }

                            else if (promise.retrunMsg == "false") {
                                swal("Record Not saved / Updated..", 'Fail');

                            }
                            else if (promise.retrunMsg == "Add") {
                                swal("Record Saved Successfully..");
                                $state.reload();
                                $scope.employeedet = false;
                            }
                            else if (promise.retrunMsg == "Update") {
                                swal("Record Updated Successfully..");
                                $state.reload();
                                $scope.employeedet = false;
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                            }

                            if (promise.gmasterloanList !== null && promise.gmasterloanList.length > 0) {
                                //$scope.currentPage = 1;
                                //$scope.itemsPerPage = 10;

                                // $scope.employeeTypeList = promise.employeeTypeList;

                                $scope.gmasterloanList = promise.gmasterloanList;
                            }
                            $scope.cancel();
                        }
                    })
            }

        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.InterestFree = function () {
            $scope.loan.hrmL_InterestAmount = 0;
        };
        // Edit Single Record
        $scope.EditData = function (record) {

            var id = record.hreldiS_Id;
            apiService.getURI("EmpLoanPayout/editRecord", id).
                then(function (promise) {

                    if (promise.employeelist != null && promise.employeelist.length > 0) {

                        $scope.employeelist = promise.employeelist;

                    }
                    if (promise.editdata != null && promise.editdata.length > 0) {
                        $scope.loan.hreL_Id = promise.editdata[0].hreL_Id;
                        $scope.loan.HRELDIS_Remarks = promise.editdata[0].hreldiS_Remarks;
                        $scope.loan.HRELDIS_PaymentMode = promise.editdata[0].hreldiS_PaymentMode;
                        $scope.loan.HRME_Id = promise.editdata[0].hrmE_Id;
                        $scope.loan.HRELDIS_Id = promise.editdata[0].hreldiS_Id;
                     //   $scope.HRME_Id = promise.editdata[0].HRME_Id;
                        $scope.loan.HRELDIS_DisbursedDate = new Date(promise.editdata[0].hreldiS_DisbursedDate);
                        $scope.loan.HRELDIS_Amount = promise.editdata[0].hreldiS_Amount;
                        $scope.empchange = true;


                        
                        $scope.Employeechange();
                        

                    }

                })
        }


        //deactivate record
        $scope.deleterecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hreldiS_ActiveFlg == false) {
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
                        apiService.DeleteURI("EmpLoanPayout/ActiveDeactiveRecord", data.hreldiS_Id).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {

                                    if (promise.retrunMsg === "Activated") {
                                        swal("Record Activated successfully");
                                    }
                                    else if (promise.retrunMsg === "Deactivated") {
                                        swal("Record Deactivated successfully");
                                    }
                                    else {
                                        swal("Record Not Activated/Deactivated", 'Fail');
                                    }

                                    if (promise.gmasterloanList !== null && promise.gmasterloanList.length > 0) {
                                        // $scope.currentPage = 1;
                                        // $scope.itemsPerPage = 10;

                                        // $scope.employeeTypeList = promise.employeeTypeList;
                                        $scope.gmasterloanList = promise.gmasterloanList;
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



        $scope.Employeechange = function () {
            var id = $scope.loan.hreL_Id;
            apiService.getURI("EmpLoanPayout/employeechange", id).
                then(function (promise) {

                    if (promise.loandetails != null && promise.loandetails.length > 0) {
                        $scope.employeedet = true;
                        $scope.loan.hrmE_Id = promise.loandetails[0].hrmE_Id;
                        $scope.hrmE_EmployeeFirstName = promise.loandetails[0].hrmE_EmployeeFirstName;
                        $scope.HRML_LoanType = promise.loandetails[0].hrmL_LoanType;
                        $scope.HREL_SanctionedAmount = promise.loandetails[0].hreL_SanctionedAmount;
                        $scope.HREL_LoanAmount = promise.loandetails[0].hreL_LoanAmount;
                       // $scope.empchange = true;
                    }
                    else {
                        $scope.employeedet = false;
                        $scope.empchange = false;
                    }

                })
        }
    }

})();