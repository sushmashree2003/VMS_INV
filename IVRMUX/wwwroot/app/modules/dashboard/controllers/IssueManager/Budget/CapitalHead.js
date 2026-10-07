
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CapitalHeadController', CapitalHeadController);
    CapitalHeadController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function CapitalHeadController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;

        }
        else {
            paginationformasters = 12;
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
        $scope.obj.totalamount = 0;


        $scope.loaddata = function () {

            //GET ALL
            $scope.obj.MI_Id = "";
            var pageid = 2;
            apiService.getURI("Vms_Budget/getalldata1", pageid).
                then(function (promise) {
                    $scope.allcompany = promise.allcompany;
                    $scope.FinancialYear = promise.financialYear;

                    $scope.month_List = promise.month_List;
                    if ($scope.month_List != null && $scope.month_List.length > 0) {
                        angular.forEach($scope.month_List, function (itm) {
                            itm.selected = true;
                        });
                    }

                });
        };

        $scope.GetEmployeeAll = function (employeeSelectedAll) {

            var toggleStatus = $scope.employeeSelectedAll;
            angular.forEach($scope.month_List, function (itm) {
                itm.selected = toggleStatus;

            });

        }

        $scope.GetEmployee = function (designation) {

            $scope.employeeSelectedAll = $scope.month_List.every(function (itm) {

                return itm.selected;
            });
        }

        // Function to select/unselect all months
        $scope.all_checkMonths = function (checkall, item) {
            angular.forEach($scope.month_List, function (month) {
                item.selectedMonths[month.ivrM_Month_Id] = checkall;
            });
        };


        $scope.hasSelectedMonth = function (item) {
            return Object.values(item.selectedMonths || {}).some(val => val);
        };


        // Function to check if at least one checkbox is selected
        $scope.isOptionsRequiredMonth = function () {
            return $scope.budgetItems.some(item =>
                Object.values(item.selectedMonths).some(val => val)
            );
        };





        // Stores selected months
        // Initialize budget items
        $scope.budgetItems = [];

        $scope.addNewRow = function () {
            $scope.budgetItems.push({
                selectedMonth: '',
                amount: '',
                remark: ''
            });
        };

        $scope.removeRow = function (index) {
            $scope.budgetItems.splice(index, 1);
        };

        // Add an initial row on load
        $scope.addNewRow();

        $scope.$apply();

        //BUDGET HEAD  TAB

        $scope.edit = function (user) {
            $scope.obj.VMBDGHD_Id = user.VMBDGHDN_Id;
            $scope.obj.MI_Id = user.MI_Id;
            $scope.obj.hrmD_Id = user.HRMD_Id;
            $scope.obj.VMBDGHD_BudgetHead = user.VMBDGHDN_BudgetHead;
            $scope.obj.VMBDGHD_Remarks = user.VMBDGHDN_Remarks;
            $scope.obj.VMBDGHD_MonthwiseBudgetFlg = user.VMBDGHDN_MonthwiseBudgetFlg;
        }


        //ACTIVE - DEACTIVE TAB1
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
                            "VMBDGHD_Id": usersem.VMBDGHDN_Id,

                        };

                        //ACTIVE - DEACTIVE

                        apiService.create("Vms_Budget/deactive1", data).then(function (promise) {
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





        //----SaveData----------------//


        //Main Group 1st tab
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var VMBDGHD_Id = 0;
                if ($scope.obj.VMBDGHD_Id > 0) {
                    VMBDGHD_Id = $scope.obj.VMBDGHD_Id;
                }
                var data = {
                    "VMBDGHD_Id": VMBDGHD_Id,
                    "HRMD_Id": $scope.obj.hrmD_Id,
                    "MI_Id": $scope.obj.MI_IdTwo,
                    "VMBDGHD_BudgetHead": $scope.obj.VMBDGHD_BudgetHead,
                    "VMBDGHD_MonthwiseBudgetFlg": $scope.obj.VMBDGHDN_MonthwiseBudgetFlg,
                    "VMBDGHD_Remarks": $scope.obj.VMBDGHD_Remarks,
                    "VMBDGHDN_SatuswiseFlg": "Capital"
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("Vms_Budget/Savedata", data).then(function (promise) {
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

            if (!$scope.obj.MI_Id) {
                console.error('MI_Id is missing');
                return;
            }
            var data = {
                "MI_Id": $scope.obj.MI_Id,
                "Flag": "Budget"

            }
            apiService.create("Vms_Budget/Getdata1", data).then(function (promise) {
                if (promise.alldata != null && promise.alldata.length > 0) {
                    $scope.alldata = promise.alldata;
                }
            })
        };




        //Budget-Head-Mapping tab

        //BudgetChange

        $scope.editU = function (userU) {
            $scope.obj.MI_IdTwo = userU.MI_Id;
            $scope.obj.hrmD_Id = userU.HRMD_Id;
            $scope.obj.VMBDGHD_IdTwo = userU.VMBDGHDN_Id;
            $scope.obj.IMFY_Id = userU.IMFY_ID;
            $scope.obj.VMBDGHD_IdTwo = userU.VMBDGHDN_Id,
                $scope.obj.VMBDGHDAMT_Amount = userU.VMBDGHDAMTN_Amount;
            $scope.obj.VMBDGHDAMT_MonthwiseAmount = userU.VMBDGHDAMTN_MonthwiseAmount;
            $scope.obj.VMBDGHDAMT_Remarks = userU.VMBDGHDAMTN_Remarks;
            $scope.obj.VMBDGHDAMT_Id = userU.VMBDGHDAMTN_Id;
            $scope.obj.VMBDGHDAMTN_Name = userU.VMBDGHDAMTN_Name;
            var data = {
                "VMBDGHDAMTN_Id": userU.VMBDGHDAMTN_Id,

            }
            apiService.create("Vms_Budget/editBudget", data).then(function (promise) {
                if (promise.month_List != null && promise.month_List.length > 0) {
                    angular.forEach($scope.month_List, function (dev) {
                        dev.selected = false; dev.amount = "";
                        angular.forEach(promise.month_List, function (month) {
                            if (dev.ivrM_Month_Id == month.VMBDGHDAMTNM_MonthId) {
                                dev.selected = true;
                                dev.amount = month.VMBDGHDAMTNM_Amount;
                            }
                        });
                    });
                    $scope.obj.totalamount = 0; var totalamount = 0;
                    angular.forEach($scope.month_List, function (employee) {
                        if (employee.selected == true) {
                            totalamount = (Number(totalamount) + (Number(employee.amount)));
                        }
                    });

                    $scope.obj.totalamount = totalamount
                }

            })
        }






        $scope.oncmpchangeDept2 = function (flag) {
            $scope.deptList = []; // Reset the department list
            $scope.allBudgetHead = []; // Reset the budget head list
            var data = {
                "MI_Id": $scope.obj.MI_IdTwo,
                "Flag": flag

            };




            apiService.create("Vms_Budget/Getdept", data).then(function (response) {
                $scope.allBudgetHead = response.budgetArray;
                if (response && response.deptList && response.deptList.length > 0) {
                    console.log('Dept List:', response.deptList);
                    $scope.deptList = response.deptList;
                } else {
                    console.warn('No departments found for the selected company.');
                }
                if (response.alldata != null && response.alldata.length > 0) {
                    $scope.alldatatwo = response.alldata;
                }
            }).catch(function (error) {
                console.error('Error fetching departments:', error);
            });
        };

        $scope.oncmpchangeDept = function (flag) {
            $scope.deptList = [];
            // $scope.allBudgetHead = []; 
            var data = {
                "MI_Id": $scope.obj.MI_IdTwo,
                "Flag": flag

            };

            apiService.create("Vms_Budget/Getdept", data).then(function (response) {

                if (response && response.deptList && response.deptList.length > 0) {
                    console.log('Dept List:', response.deptList);
                    $scope.deptList = response.deptList;
                } else {
                    console.warn('No departments found for the selected company.');
                }
                if (response.alldata != null && response.alldata.length > 0) {
                    $scope.alldata = response.alldata;
                }
            }).catch(function (error) {
                console.error('Error fetching departments:', error);
            });
        };




        //2nd tab

        $scope.oncmpchangeTwo = function () {
            $scope.obj.VMBDGHDAMT_MonthwiseAmount = "";
            $scope.obj.VMBDGHDAMT_Amount = "";
            $scope.alldatatwo = [];
            $scope.BudgetArray = [];
            $scope.MonthwiseBudgetFlg = false;
            $scope.obj.VMBDGHD_IdTwo = "";
            var data = {
                "MI_Id": $scope.obj.MI_IdTwo,
                "Flag": "BudgetAmount",
                "HRMD_Id": $scope.obj.hrmD_Id
            }
            apiService.create("Vms_Budget/Getdata1", data).then(function (promise) {
                $scope.allBudgetHead = promise.budgetArray;
                if (promise.alldata != null && promise.alldatatwo.length > 0) {
                    $scope.alldatatwo = promise.alldatatwo;
                }

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


        //Approval Data2
        $scope.submittedUG = false;
        $scope.savedataUG = function () {
            $scope.employeeselected = [];
            if ($scope.myForm1.$valid) {
                if ((Number($scope.obj.totalamount)) === (Number($scope.obj.VMBDGHDAMT_Amount))) {

                }
                else {
                    swal("Total Amount Not Matched Moth Wise Amount ! ");
                    return;
                }

                var VMBDGHDAMT_Id = 0;
                var VMBDGHDAMT_MonthwiseAmount = 0;
                if ($scope.obj.VMBDGHDAMT_Id > 0) {
                    VMBDGHDAMT_Id = $scope.obj.VMBDGHDAMT_Id;
                }
                if ($scope.obj.VMBDGHDAMT_MonthwiseAmount > 0) {
                    VMBDGHDAMT_MonthwiseAmount = $scope.obj.VMBDGHDAMTN_MonthwiseAmount;
                }


                angular.forEach($scope.month_List, function (itm) {
                    if (itm.selected == true) {
                        $scope.employeeselected.push({

                            VMBDGHDAMTNM_MonthId: itm.ivrM_Month_Id,
                            VMBDGHDAMTNM_Amount: itm.amount

                        });
                    }
                });

                var data = {
                    "VMBDGHD_Id": $scope.obj.VMBDGHD_IdTwo,
                    "VMBDGHDAMT_Id": VMBDGHDAMT_Id,
                    "HRMD_Id": $scope.obj.hrmD_Id,
                    "VMBDGHDAMTN_Name": $scope.obj.VMBDGHDAMTN_Name,
                    "MI_Id": $scope.obj.MI_IdTwo,
                    "IMFY_ID": $scope.obj.IMFY_Id,
                    "VMBDGHDAMT_Amount": $scope.obj.VMBDGHDAMT_Amount,
                    "VMBDGHDAMT_Remarks": $scope.obj.VMBDGHDAMT_Remarks,
                    //"VMBDGHDAMTNM_MonthId": $scope.obj.ivrM_Month_Id,
                    //"VMBDGHDAMTNM_Amount": $scope.amount,
                    "Amount_List": $scope.employeeselected
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("Vms_Budget/ApprovalData1", data).then(function (promise) {
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
            else {                $scope.submittedUG = true;            }

        };




        //Budget Head Mapping Deactive  grid2

        $scope.onchangeclick = function () {
            $scope.obj.totalamount = 0; var totalamount = 0;
            angular.forEach($scope.month_List, function (employee) {
                employee.selected = $scope.employeeSelectedAll;
            });

            if ($scope.obj.VMBDGHDAMT_Amount > 0) {
                angular.forEach($scope.month_List, function (employee) {
                    if (employee.selected == true) {
                        totalamount = (Number(totalamount) + (Number(employee.amount)));
                    }
                    else {
                        employee.amount = "";
                    }
                });
            }
            $scope.obj.totalamount = totalamount
        }
        $scope.onchangeamount = function () {
            $scope.obj.totalamount = 0; var totalamount = 0;
            var monthcount = 0;
            angular.forEach($scope.month_List, function (employee) {
                if (employee.selected == true) {
                    monthcount = monthcount + 1;
                }
                else {
                    employee.amount = 0;
                    $scope.employeeSelectedAll = false;
                }
            });
            angular.forEach($scope.month_List, function (employee) {
                if (employee.selected == true) {
                    employee.amount = (Math.round(Number($scope.obj.VMBDGHDAMT_Amount) / monthcount));
                    totalamount = (Number(totalamount) + (Number(employee.amount)));
                }

            });
            $scope.obj.totalamount = totalamount;
        };
        $scope.onchangeamountNew = function () {
            $scope.obj.totalamount = 0; var totalamount = 0;
            angular.forEach($scope.month_List, function (employee) {
                if (employee.selected == true && employee.amount > 0) {
                    totalamount = (Number(totalamount) + (Number(employee.amount)));
                }
                else {
                    $scope.employeeSelectedAll = false;
                }

            });
            $scope.obj.totalamount = totalamount;
        }
        $scope.Budgettable = function () {
            $scope.obj.totalamount = 0; var totalamount = 0;
            if ($scope.obj.VMBDGHDAMT_Amount > 0) {
                angular.forEach($scope.month_List, function (employee) {
                    employee.selected = true; employee.amount = 0;
                    employee.amount = (Math.round(Number($scope.obj.VMBDGHDAMT_Amount) / 12));
                    totalamount = (Number(totalamount) + (Number(employee.amount)));
                });
            }
            else {
                angular.forEach($scope.month_List, function (employee) {
                    employee.amount = 0;
                    employee.selected = false;
                });
            }
            $scope.obj.totalamount = totalamount

        }



        //Budget Head Mapping Deactive  grid2

        //Budget Head Mapping Deactive  grid2
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
                            "VMBDGHDAMT_Id": usersem.VMBDGHDAMTN_Id,

                        };
                        apiService.create("Vms_Budget/edit1", data).then(function (promise) {
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