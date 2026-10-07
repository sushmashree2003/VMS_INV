(function () {
    'use strict';
    angular
        .module('app')
        .controller('FF_SettlementAdminApplyController', FF_SettlementAdminApplyController)
    FF_SettlementAdminApplyController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function FF_SettlementAdminApplyController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

  
        $scope.ffsetllement = {};
        $scope.ffsetllement.hreffS_EntryDate = new Date();
        $scope.yearDis = true;
        $scope.monthDis = true;
        $scope.configurationDetails = {};
        $scope.empGrossSalDis = true;    

        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("FF_SettlemetAdminApply/getalldetails", pageid).then(function (promise) {

                if (promise.empadvaList !== null && promise.empadvaList.length > 0) {
                    $scope.gridOptions = promise.empadvaList;
                }

                if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                    $scope.employeedropdown = promise.employeedropdown;
                }

                angular.forEach($scope.gridOptions, function (value, key) {
                    var fdate = value.hreffS_EntryDate.split('T');
                    value.hreffSEntryDate = fdate[0];
                });                                
            })
        }
    
        $scope.GetAllRelData = function (obj) {

            var data = {
                "HRME_Id": obj.hrmE_Id.hrmE_Id,

            };
            apiService.create("FF_SettlemetAdminApply/empdetails", data).then(function (promise) {
                $scope.getdata = promise.griddisplay;

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
                if ($scope.ffsetllement.hreffS_EntryDate != undefined && $scope.ffsetllement.hreffS_EntryDate != "") {
                    $scope.ffsetllement.hreffS_EntryDate = new Date($scope.ffsetllement.hreffS_EntryDate).toDateString();
                }
                else {
                    $scope.ffsetllement.hreffS_EntryDate = null;
                }

                var data = {
                    "HREFFS_EntryDate": $scope.ffsetllement.hreffS_EntryDate,
                    "HREFFS_EntryDate": $scope.ffsetllement.hreffS_EntryDate,
                }
                apiService.create("FF_SettlemetAdminApply/", data).
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
                            }
                            else if (promise.retrunMsg == "Update") {
                                swal("Record Updated Successfully..");
                                $state.reload();
                            }
                            else if (promise.retrunMsg == "acc") {
                                swal("Account No. is already exist..");
                                return;
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }
                            if (promise.empadvaList !== null && promise.empadvaList.length > 0) {
                                $scope.gridOptions = promise.empadvaList;
                                angular.forEach($scope.gridOptions, function (value, key) {
                                    var fdate = value.hreffS_EntryDate.split('T');
                                    value.hreffS_EntryDate = fdate[0];

                                });
                            }
                            $scope.cancel();
                        }
                    })
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.editflg = false;
        // Edit Single Record
        $scope.lablenamme = {};
        $scope.EditData = function (record) {
            $scope.ffsetllement = {};
            $scope.editflg = true;
            var id = record.hreffS_Id;
            apiService.getURI("FF_SettlemetAdminApply/editRecord", id).
                then(function (promise) {
                    if (promise.empadvaList != null && promise.empadvaList.length > 0) {
                        $scope.ffsetllement = promise.empadvaList[0];
                        $scope.lablenamme1 = promise.empadvaList[0].hrmE_Id;
                        $scope.ffsetllement.hreffS_EntryDate = new Date(promise.empadvaList[0].hreffS_EntryDate);

                        $scope.ffsetllement.empGrossSal = promise.empGrossSal;
                        $scope.ffsetllement.totalAppliedAmount = promise.totalAppliedAmount;
                        angular.forEach($scope.employeedropdown, function (wer) {
                            if (wer.hrmE_Id == $scope.lablenamme1) {
                                $scope.lablenamme = wer.hrmE_EmployeeFirstName;
                            }
                        })
                    }
                })
        }
       

       



        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hreffS_ActiveFlag == false) {
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
                        apiService.DeleteURI("FF_SettlemetAdminApply/ActiveDeactiveRecord", data.hresA_Id).
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
                                            var fdate = value.hreffS_EntryDate.split('T');
                                            value.hreffS_EntryDate = fdate[0];

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