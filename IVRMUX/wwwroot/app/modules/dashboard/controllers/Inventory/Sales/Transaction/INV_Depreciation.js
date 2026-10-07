
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_DepreciationController', INV_DepreciationController);
    INV_DepreciationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function INV_DepreciationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 20;
        }
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.searchValue = "";
        //====================================== Page Load
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("INV_Depreciation/getloaddata", pageid).then(function (promise) {
                $scope.IMFY_YEAR = promise.imfY_YEAR;
                $scope.Institution = promise.institution;
                $scope.get_Report = promise.getReport;
                $scope.presentCountgrid = $scope.get_Report.length;
            });
        };
        //get
        $scope.Getreport = function () {
            if ($scope.myForm.$valid) {
                $scope.Arraystwo = [];
                $scope.getdata = [];
               
                $scope.Arraystwo.push({ MI_Id: $scope.mI_Id });
                var data = {
                    "IMFY_Id": $scope.imfY_Id,
                    //"INVMSTSS_Id": $scope.invmsT_Id,
                    "companyNames": $scope.Arraystwo,
                };
                apiService.create("INV_Depreciation/Itemchanges", data).then(function (promise) {
                    if (promise.getdata !== null && promise.getdata.length > 0) {
                        $scope.getdata = promise.getdata;
                        angular.forEach($scope.getdata, function (dd) {
                            dd.chkitem = false;
                        });
                    }
                    else {
                        swal("Record Not Found !");
                    }
                });
            } else {
                $scope.submitted = true;
            }       
        };
        //===========Get Item deatils On item Change
        $scope.all_checkCCCC = function () {
            var mI_Id = $scope.mI_Id;
            $scope.getdata = [];           
            angular.forEach($scope.Institution, function (itm) {
                itm.selected = mI_Id;
            });
            $scope.storechanges();
        };
        $scope.togchkbxCCCC = function () {
            $scope.getdata = [];
            $scope.mI_Id = $scope.Institution.every(function (options) {
                return options.selected;
            });
        };
        $scope.isOptionsRequired = function () {
            return !$scope.getdata.some(function (options) {
                return options.chkitem;
            });
        };

        $scope.isOptionsRequiredtwo = function () {
            return !$scope.Institution.some(function (options) {
                return options.selected;
            });
        };
        //===================================== SAVE DATA

        $scope.onchangeyear = function () {
            $scope.getdata = [];
        };

        $scope.storeChangestwo = function () {
            $scope.imfY_Id = "";
            $scope.getdata = [];
        };

        $scope.storechanges = function () {            
            $scope.Arrays = [];
            $scope.get_Store = [];
            $scope.getdata = [];
            //angular.forEach($scope.Institution, function (comp) {
            //    if (comp.selected == true) {
            //        $scope.Arrays.push({ MI_Id: comp.mI_Id });
            //    }
            //});

            $scope.Arrays.push({ MI_Id: $scope.mI_Id });

            if ($scope.Arrays !== null && $scope.Arrays.length > 0) {
                var data = {
                    "companyNames": $scope.Arrays,
                };
                apiService.create("INV_Depreciation/storechanges", data).then(function (promise) {
                    $scope.get_Store = promise.get_Store;
                });
            }
            else {
                swal("Please select Compny !");
            }
        };        

        $scope.savedata = function () {          
            if ($scope.myform2.$valid) {
                $scope.Arraysthree = [];
                var INVDEP_Id = 0;
                if ($scope.INVDEP_Id != null &&  $scope.INVDEP_Id > 0) {
                    INVDEP_Id = $scope.INVDEP_Id;
                }
                angular.forEach($scope.getdata, function (comp) {
                    if (comp.chkitem == true) {
                        $scope.Arraysthree.push({
                            MI_Id: comp.MI_Id,
                            INVMST_Id: comp.INVMST_Id,
                            INVMI_Id: comp.INVMI_Id,
                            INVDEP_OpeningValue: comp.INVDEP_OpeningValue,
                            INVDEP_OpeningValueAsOnDate: new Date(comp.INVDEP_OpeningValueAsOnDate).toDateString(),
                            INVDEP_DepreciationPer: comp.INVDEP_DepreciationPer,
                            INVDEP_ClosingValue: comp.INVDEP_ClosingValue,
                            INVDEP_ClosingValueAsOnDate: new Date(comp.INVDEP_ClosingValueAsOnDate).toDateString(),
                            INVDEP_ReValuationValue: comp.INVDEP_ReValuationValue,
                            INVMI_ItemName: comp.INVMI_ItemName,
                            INVMI_ItemMoreThan180:comp.INVMI_ItemMoreThan180,
                            INVMI_ItemLessThan180:comp.INVMI_ItemLessThan180,
                            INVMI_DeletedItemAmount:comp.INVMI_DeletedItemAmount
                        });
                    }
                });               
                var data = {
                    "INV_Depreciation": $scope.Arraysthree,
                    "IMFY_Id": $scope.imfY_Id,
                    "INVDEP_Id": INVDEP_Id
                };

                apiService.create("INV_Depreciation/savedetails", data).then(function (promise) {
                    if (promise.returnstatus === "Insert") {
                        swal("Record Saved Successfully !");
                        $state.reload();
                    }
                    if (promise.returnstatus === "Failed") {
                        swal("Record Not saved !");
                    }
                    if (promise.returnstatus === "Update") {
                        swal("Record Update Successfully !");
                    }
                    if (promise.returnstatus === "Notupdate") {
                        swal("Record Not Update !");
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted2 = true;
            }
        };       

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };

        $scope.cancel = function () {
            $state.reload();
        };
        $scope.edit = function (task) {
            $scope.getdata = [];            
            $scope.INVDEP_Id = task.invdeP_Id;
            $scope.mI_Id = task.mI_Id;
            $scope.storechanges();
            $scope.invmsT_Id = task.invmsT_Id;
            $scope.imfY_Id = task.imfY_Id;
            if ($scope.INVDEP_Id !== null && $scope.INVDEP_Id > 0) {
                $scope.getdata.push({
                    MI_Id: task.mI_Id,
                    INVMST_Id: task.invmsT_Id,
                    INVMI_Id: task.invmI_Id,
                    INVDEP_OpeningValue: task.invdeP_OpeningValue,
                    INVDEP_OpeningValueAsOnDate: new Date(task.invdeP_OpeningValueAsOnDate),
                    INVDEP_DepreciationPer: task.invdeP_DepreciationPer,
                    INVDEP_ClosingValue: task.invdeP_ClosingValue,
                    INVDEP_ClosingValueAsOnDate: new Date(task.invdeP_ClosingValueAsOnDate),
                    INVDEP_ReValuationValue: task.invdeP_ReValuationValue,
                    INVMI_ItemName: task.invmI_ItemName,
                    INVOB_PurchaseRate: task.invoB_PurchaseRate,
                    INVMI_ItemMoreThan180:task.invmI_ItemMoreThan180,
                    INVMI_ItemLessThan180:task.invmI_ItemLessThan180,
                    INVMI_DeletedItemAmount:task.invmI_DeletedItemAmount,
                    chkitem: true                  
                });
            }
            else {
                swal("Record Not Found !");
            }
        };
        $scope.Deletedata = function (item, SweetAlert) {
            $scope.INVDEP_Id = item.invdeP_Id;           
            var data = {
                "INVDEP_Id": $scope.INVDEP_Id
            }
            var dystring = "";
            if (item.invdeP_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invdeP_ActiveFlg == false) {
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
                        apiService.create("INV_Depreciation/deactive", data).
                            then(function (promise) {
                                if (promise.returnstatus === "Delete") {
                                    swal("Record " + dystring + "d Successfully!!!");

                                }
                                if (promise.returnstatus === "NotDelete") {
                                    swal("Record Not " + dystring + "d Successfully!!!");                                    
                                }

                                $state.reload();

                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }      
    }
})();

