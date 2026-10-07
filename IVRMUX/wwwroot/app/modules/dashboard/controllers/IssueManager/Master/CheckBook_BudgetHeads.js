
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CheckBook_BudgetHeadsController', CheckBook_BudgetHeadsController);
    CheckBook_BudgetHeadsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function CheckBook_BudgetHeadsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;

        }
        else {
            paginationformasters = 10;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //-------------------------------------------------------------------
        $scope.obj = {};
        $scope.obj.VMBDGHD_MonthwiseBudgetFlg = false;
        $scope.searchValue = "";
        $scope.allBudgetHead = [];
        $scope.obj.VMBDGHD_IdTwo = "";
        $scope.MonthwiseBudgetFlg = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.search = "";
        $scope.currentPageI = 1;
        $scope.currentPageU = 1;
        $scope.itemsPerPageI = paginationformasters;
        $scope.itemsPerPageU = paginationformasters;
        $scope.search = "";
        $scope.searchValueU = "";
        $scope.loaddata = function () {
           
            $scope.obj.MI_Id = "";
            var pageid = 2;
            apiService.getURI("CheckBook_BudgetHeads/getalldata", pageid).
                then(function (promise) {
                    $scope.allcompany = promise.allcompany;
                    $scope.FinancialYear = promise.financialYear;
                });
        };
        $scope.edit = function (user) {
            $scope.obj.VMBDGHD_Id = user.VMBDGHD_Id;
            $scope.obj.MI_Id = user.MI_Id;
            $scope.obj.VMBDGHD_BudgetHead = user.VMBDGHD_BudgetHead;
            $scope.obj.VMBDGHD_Remarks = user.VMBDGHD_Remarks;
            $scope.obj.VMBDGHD_MonthwiseBudgetFlg = user.VMBDGHD_MonthwiseBudgetFlg;
        }
        $scope.clientDecative = function (usersem, SweetAlert) {

            var dystring = "";
            if (usersem.VMBDGHD_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (usersem.VMBDGHD_ActiveFlg === false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        var data = {
                            "VMBDGHD_Id": usersem.VMBDGHD_Id,

                        };
                        apiService.create("CheckBook_BudgetHeads/deactive", data).then(function (promise) {
                            if (promise.returnval == "active") {
                                swal("Record Activate Sucessfully !");
                            }
                            else if (promise.returnval == "Notactive") {
                                swal("Record Not Notactive   !");
                            }
                            else if (promise.returnval == "admin") {
                                swal("Please Contact Administartor   !");
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        };
        //---------------------------------Save--------------------------------------------
        //Main Group
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var VMBDGHD_Id = 0;
                if ($scope.obj.VMBDGHD_Id > 0) {
                    VMBDGHD_Id = $scope.obj.VMBDGHD_Id;
                }
                var data = {
                    "VMBDGHD_Id": VMBDGHD_Id,
                    "MI_Id": $scope.obj.MI_Id,
                    "VMBDGHD_BudgetHead": $scope.obj.VMBDGHD_BudgetHead,
                    "VMBDGHD_MonthwiseBudgetFlg": $scope.obj.VMBDGHD_MonthwiseBudgetFlg,
                    "VMBDGHD_Remarks": $scope.obj.VMBDGHD_Remarks,

                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("CheckBook_BudgetHeads/savedta", data).then(function (promise) {
                    if (promise.returnval == "Save") {
                        swal("Record Save Sucessfully !");
                    }
                    else if (promise.returnval == "Notsave") {
                        swal("Record Not Saved !");
                    }
                    else if (promise.returnval == "Update") {
                        swal("Record Update  Sucessfully  !");
                    }
                    else if (promise.returnval == "NotUpdate") {
                        swal("Record Not Update    !");
                    }
                    else if (promise.returnval == "admin") {
                        swal("Please Contact Administartor   !");
                    }
                    else if (promise.returnval == "duplicate") {
                        swal("Record Already exist !");
                    }
                    $state.reload();

                })
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.oncmpchange = function () {
            $scope.alldata = [];
            $scope.MonthwiseBudgetFlg = false;
            var data = {
                "MI_Id": $scope.obj.MI_Id,
                "Flag": "Budget"

            }
            apiService.create("CheckBook_BudgetHeads/Getdata", data).then(function (promise) {
                if (promise.alldata != null && promise.alldata.length > 0) {
                    $scope.alldata = promise.alldata;
                }
            })
        };
        //BudgetChange
        $scope.BudgetChange = function () {
            $scope.obj.VMBDGHDAMT_MonthwiseAmount = "";
            $scope.obj.VMBDGHDAMT_Amount = "";
            $scope.MonthwiseBudgetFlg = false;
            if ($scope.allBudgetHead != null && $scope.allBudgetHead.length > 0) {
                for (var i = 0; i < $scope.allBudgetHead.length; i++) {
                    if ($scope.allBudgetHead[i].vmbdghD_Id == $scope.obj.VMBDGHD_IdTwo && $scope.allBudgetHead[i].vmbdghD_MonthwiseBudgetFlg == true) {
                        $scope.MonthwiseBudgetFlg = true;
                        return;
                    }
                    else {
                        $scope.MonthwiseBudgetFlg = false;
                    }

                }
            }

        }
        $scope.editU = function (userU) {
            $scope.obj.MI_IdTwo = userU.MI_Id;
            $scope.obj.VMBDGHD_IdTwo = userU.VMBDGHD_Id;
            $scope.obj.IMFY_Id = userU.IMFY_ID;
            $scope.obj.VMBDGHDAMT_Amount = userU.VMBDGHDAMT_Amount;
            $scope.obj.VMBDGHDAMT_MonthwiseAmount = userU.VMBDGHDAMT_MonthwiseAmount;
            $scope.obj.VMBDGHDAMT_Remarks = userU.VMBDGHDAMT_Remarks;
            $scope.obj.VMBDGHDAMT_Id = userU.VMBDGHDAMT_Id;
        }
        //oncmpchangeTwo
        $scope.oncmpchangeTwo = function () {
            $scope.obj.VMBDGHDAMT_MonthwiseAmount = "";
            $scope.obj.VMBDGHDAMT_Amount = "";
            $scope.alldatatwo = [];
            $scope.BudgetArray = [];
            $scope.MonthwiseBudgetFlg = false;
            $scope.obj.VMBDGHD_IdTwo = "";
            var data = {
                "MI_Id": $scope.obj.MI_IdTwo,
                "Flag": "BudgetAmount"

            }
            apiService.create("CheckBook_BudgetHeads/Getdata", data).then(function (promise) {
                if (promise.alldata != null && promise.alldata.length > 0) {
                    $scope.alldatatwo = promise.alldata;
                }
                $scope.allBudgetHead = promise.budgetArray;
            })
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted1 = function (field) {
            return $scope.submittedUG;
        };
        $scope.cancel = function () {
            $state.reload();
        }
        $scope.cancelUG = function () {
            $state.reload();
        }
        $scope.savedataUG = function () {
            $scope.submittedUG = true;
            if ($scope.myForm1.$valid) {
                var VMBDGHDAMT_Id = 0;
                var VMBDGHDAMT_MonthwiseAmount = 0;
                if ($scope.obj.VMBDGHDAMT_Id > 0) {
                    VMBDGHDAMT_Id = $scope.obj.VMBDGHDAMT_Id;
                }
                if ($scope.obj.VMBDGHDAMT_MonthwiseAmount > 0) {
                    VMBDGHDAMT_MonthwiseAmount = $scope.obj.VMBDGHDAMT_MonthwiseAmount;
                }
                var data = {
                    "VMBDGHD_Id": $scope.obj.VMBDGHD_IdTwo,
                    "VMBDGHDAMT_Id": VMBDGHDAMT_Id,
                    "MI_Id": $scope.obj.MI_IdTwo,
                    "IMFY_ID": $scope.obj.IMFY_Id,
                    "VMBDGHDAMT_Amount": $scope.obj.VMBDGHDAMT_Amount,
                    "VMBDGHDAMT_MonthwiseAmount": VMBDGHDAMT_MonthwiseAmount,
                    "VMBDGHDAMT_Remarks": $scope.obj.VMBDGHDAMT_Remarks,


                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("CheckBook_BudgetHeads/ApprovalData", data).then(function (promise) {
                    if (promise.returnval == "Save") {
                        swal("Record Save Sucessfully !");
                    }
                    else if (promise.returnval == "Notsave") {
                        swal("Record Not Saved !");
                    }
                    else if (promise.returnval == "update") {
                        swal("Record Update  Sucessfully  !");
                    }
                    else if (promise.returnval == "Notupdate") {
                        swal("Record Not Update    !");
                    }
                    else if (promise.returnval == "admin") {
                        swal("Please Contact Administartor   !");
                    }
                    else if (promise.returnval == "duplicate") {
                        swal("Record Already exist !");
                    }
                    $state.reload();

                })
            }

        };
        $scope.Budgettable = function () {
            var VMBDGHDAMT_Amount = 0;
            var VMBDGHDAMT_MonthwiseAmount = 0;
            if ($scope.obj.VMBDGHDAMT_Amount > 0) {
                VMBDGHDAMT_Amount = $scope.obj.VMBDGHDAMT_Amount;
            }
            if ($scope.obj.VMBDGHDAMT_MonthwiseAmount > 0) {
                VMBDGHDAMT_MonthwiseAmount = $scope.obj.VMBDGHDAMT_MonthwiseAmount;
            }
            if ((Number(VMBDGHDAMT_Amount)) < (Number(VMBDGHDAMT_MonthwiseAmount))) {
                swal("Budget Amount Not Greater Than Monthwise Amount !");
                $scope.obj.VMBDGHDAMT_MonthwiseAmount = "";
               
            }
          
          
           
        }
        $scope.deactiveU = function (usersem, SweetAlert) {

            var dystring = "";
            if (usersem.VMBDGHDAMT_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (usersem.VMBDGHDAMT_ActiveFlg === false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        var data = {
                            "VMBDGHDAMT_Id": usersem.VMBDGHDAMT_Id,

                        };
                        apiService.create("CheckBook_BudgetHeads/edit", data).then(function (promise) {
                            if (promise.returnval == "active") {
                                swal("Record Activate Sucessfully !");
                            }
                            else if (promise.returnval == "Notactive") {
                                swal("Record Not Notactive   !");
                            }
                            else if (promise.returnval == "admin") {
                                swal("Please Contact Administartor   !");
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        };
    }
})();