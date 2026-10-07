(function () {
    'use strict';
    angular.module('app').controller('FeeInstallmentController', feeMIController)

    feeMIController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache']
    function feeMIController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache) {

        $scope.sortKey = "fmI_Id";
        $scope.reverse = true;
        $scope.submitted = false;
        $scope.disableinstallmentnme = false;
        $scope.disabletypename = false;
        $scope.academicdisable = false;
        $scope.installmenttypedisable = false;
        $scope.grigview1 = true;
        $scope.noinstall = false;
        $scope.searchvalue = '';
        $scope.editEmployee = {}

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }
        
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;           
            $scope.page1 = "page1";            
            var pageid = 2;

            apiService.getURI("FeeInstallment/getalldetails", pageid).then(function (promise) {
                $scope.databindDB = promise.installmentData;
                $scope.totcountfirst = $scope.databindDB.length;
                $scope.fmI_Id = 0;
            });
        };

        $scope.TextBoxChanged1 = function (installment) {

            $scope.noinstall = false;
            $scope.FMI_No_Of_Installments = "";
            if (installment == 1) {
                $scope.noinstall = true;
            }
            else if (installment == 2) {
                $scope.noinstall = false;
                $scope.grigview2 = true;
                var valu = 1;
                $scope.albumNameArray = [];
                if (valu > 0) {
                    $scope.albumNameArray = [];
                    for (var i = 1; i <= valu; i++) {
                        $scope.albumNameArray.push({ 'ftI_Name': "" });
                    }
                    $scope.students = $scope.albumNameArray;
                }
            }
            else if (installment == 3) {
                $scope.noinstall = false;
                $scope.grigview2 = true;
                var valu = 12;
                $scope.albumNameArray = [];
                if (valu > 0) {
                    $scope.albumNameArray = [];
                    for (var i = 1; i <= valu; i++) {
                        $scope.albumNameArray.push({ 'ftI_Name': "" });
                    }
                    $scope.students = $scope.albumNameArray;
                }
            }
            else if (installment == 4) {
                $scope.noinstall = false;
                $scope.grigview2 = true;
                var valu = 4;
                $scope.albumNameArray = [];
                if (valu > 0) {
                    $scope.albumNameArray = [];
                    for (var i = 1; i <= valu; i++) {
                        $scope.albumNameArray.push({ 'ftI_Name': "" });
                    }
                    $scope.students = $scope.albumNameArray;
                }
            }
            else if (installment == 5) {
                $scope.noinstall = false;
                $scope.grigview2 = true;
                var valu = 1;
                $scope.albumNameArray = [];
                if (valu > 0) {
                    $scope.albumNameArray = [];
                    for (var i = 1; i <= valu; i++) {
                        $scope.albumNameArray.push({ 'ftI_Name': "" });
                    }
                    $scope.students = $scope.albumNameArray;
                }
            }
            else if (installment == 6) {
                $scope.noinstall = false;
                $scope.grigview2 = true;
                var valu = 2;
                $scope.albumNameArray = [];
                if (valu > 0) {
                    $scope.albumNameArray = [];
                    for (var i = 1; i <= valu; i++) {
                        $scope.albumNameArray.push({ 'ftI_Name': "" });
                    }
                    $scope.students = $scope.albumNameArray;
                }
            }
            else if (installment == 7) {
                $scope.noinstall = false;
                $scope.grigview2 = true;
                var valu = 24;
                $scope.albumNameArray = [];
                if (valu > 0) {
                    $scope.albumNameArray = [];
                    for (var i = 1; i <= valu; i++) {
                        $scope.albumNameArray.push({ 'ftI_Name': "" });
                    }
                    $scope.students = $scope.albumNameArray;
                }
            }
            else if (installment == 8) {
                $scope.noinstall = false;
                $scope.grigview2 = true;
                var valu = 1;
                $scope.albumNameArray = [];
                if (valu > 0) {
                    $scope.albumNameArray = [];
                    for (var i = 1; i <= valu; i++) {
                        $scope.albumNameArray.push({ 'ftI_Name': "" });
                    }
                    $scope.students = $scope.albumNameArray;
                }
            }
            else {
                $scope.noinstall = false;
            }
        };

        $scope.TextBoxChanged = function () {
            if ($scope.fmI_Id != 0) {
                $scope.grigview2 = true;
                if ($scope.forcompare < $scope.FMI_No_Of_Installments) {
                    var valu = $scope.FMI_No_Of_Installments;
                    $scope.albumNameArray = [];
                    $scope.albumNameArraynew = $scope.students;
                    if (valu > 0) {
                        for (var i = 1; i <= ($scope.FMI_No_Of_Installments - $scope.forcompare); i++) {
                            $scope.albumNameArraynew.push({ 'ftI_Name': "" });
                        }
                        $scope.students = $scope.albumNameArraynew;
                    }
                }
            }
            else {
                $scope.grigview2 = true;
                var valu = $scope.FMI_No_Of_Installments;
                $scope.albumNameArray = [];
                if (valu > 0) {
                    $scope.albumNameArray = [];
                    for (var i = 1; i <= valu; i++) {
                        $scope.albumNameArray.push({ 'ftI_Name': "" });
                    }
                    $scope.students = $scope.albumNameArray;
                }
                else if (valu == 0) {
                    $scope.grigview2 = false;
                    swal("No Of Installments Can't be Zero ");
                    $scope.FMI_No_Of_Installments = "";
                }
                else {
                    $scope.grigview2 = false;
                }
            }
        }

        $scope.savedata = function (students) {

            if ($scope.form1.$valid) {
                $scope.myArray = [];
                angular.forEach($scope.students, function (user) {
                    $scope.myArray.push(user);
                });

                var data = {
                    "FMI_Id": $scope.fmI_Id,
                    "FMI_Name": $scope.FMI_Name,
                    "FMI_No_Of_Installments": $scope.FMI_No_Of_Installments,
                    "FMI_Installment_Type": $scope.FMI_Installment_Type,
                    "FMI_ActiceFlag": true,
                    "fydto": $scope.students,
                };
                
                apiService.create("FeeInstallment/savedetail", data).then(function (promise) {
                    if (promise.msg != "" && promise.msg != null) {
                        swal(promise.msg);
                        return;
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate' && promise.returnval === false) {
                        swal('Record Already Exist');
                        $scope.loaddata();
                        $scope.Clear();
                    }
                    else if (promise.returnduplicatestatus === "Save") {
                        swal('Record Saved Successfully');
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus === "NotSave") {
                        swal('Record Not Saved Successfully');
                    }
                    else if (promise.returnduplicatestatus === "Update") {
                        swal('Record Updated Successfully');
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus === "NotUpdate") {
                        swal('Record Not Updated Successfully');
                    }
                    else {
                        swal('Record Not Saved/Updated Successfully');
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.getorgvalue = function (employee) {

            $scope.editEmployee = employee.fmI_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("FeeInstallment/getdetails", pageid).then(function (promise) {

                $scope.students = "";
                $scope.disableinstallmentnme = true;
                $scope.disabletypename = true;
                $scope.FMI_Installment_Type = promise.installmentData[0].fmI_Installment_Type;

                if ($scope.FMI_Installment_Type == 1) {
                    $scope.noinstall = true;
                }
                else {
                    $scope.noinstall = false;
                }

                $scope.FMI_Name = promise.installmentData[0].fmI_Name;
                $scope.FMI_No_Of_Installments = promise.installmentData[0].fmI_No_Of_Installments;
                $scope.forcompare = promise.installmentData[0].fmI_No_Of_Installments;
                $scope.FMI_Installment_Type = promise.installmentData[0].fmI_Installment_Type;
                if (promise.inTData.length > 0) {
                    $scope.grigview2 = true;
                    $scope.students = promise.inTData
                }
                else {
                    $scope.grigview2 = false;
                    $scope.disableinstallmentnme = false;
                    $scope.disabletypename = false;
                }
                $scope.fmI_Id = promise.installmentData[0].fmI_Id;
            })
        }

        $scope.deactive = function (installmentData, SweetAlert) {
            
            var mgs = "";
            var confirmmgs = "";
            if (installmentData.fmI_ActiceFlag == true) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("FeeInstallment/deactivate", installmentData).then(function (promise) {

                            $scope.loaddata();
                            $scope.MI_Id = "";
                            $scope.FMI_Name = "";
                            $scope.FMI_No_Of_Installments = "";
                            $scope.FMI_Installment_Type = "";
                            $scope.databindDB = promise.installmentData;
                            if (promise.returnvalue == "used") {
                                swal("Record already Used");
                            }
                            else {
                                if (promise.returnval == true) {
                                    swal("Record " + confirmmgs + " Successfully");
                                }

                                else {
                                    swal("Record " + confirmmgs + " Successfully");
                                }
                            }

                            $scope.loaddata();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }

        $scope.Clear = function () {
            $state.reload();
        };

        $scope.filtervalue = function (obj) {
            if (obj.fmI_ActiceFlag == true) {
                $scope.testSub = "Active";
            } else if (obj.fmI_ActiceFlag == false) {
                $scope.testSub = "Deactive";
            }
            return (angular.lowercase(obj.fmI_Name)).indexOf(angular.lowercase($scope.searchvalue)) >= 0 || JSON.stringify(obj.fmI_ActiceFlag).indexOf($scope.searchvalue) >= 0 || JSON.stringify(obj.fmI_No_Of_Installments).indexOf($scope.searchvalue) >= 0 || ($scope.testSub).indexOf($scope.searchvalue) >= 0;
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();