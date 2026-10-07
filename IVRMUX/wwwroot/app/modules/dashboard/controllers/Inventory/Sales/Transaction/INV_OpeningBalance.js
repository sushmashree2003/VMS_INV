
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_OpeningBalanceController', INV_OpeningBalanceController);
    INV_OpeningBalanceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function INV_OpeningBalanceController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var date = new Date();
        // $scope.invoB_PurchaseDate = date;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !=null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //=====================Adding and removing new row in transcation Grid============      
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        if ($scope.transrows.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addOBrows = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.transrows.length > 1) {
                    for (var i = 0; i === $scope.transrows.length; i++) {
                        var id = $scope.transrows[i].itrS_Id;
                        var lastChar = id.substr(id.length - 1);
                        $scope.cnt = parseInt(lastChar);
                    }
                }
                $scope.cnt = $scope.cnt + 1;
                $scope.tet = 'trans' + $scope.cnt;
                var newItemNo = $scope.cnt;
                $scope.transrows.push({ 'itrS_Id': 'trans' + newItemNo });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.removeOBrows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);
            if (data.amstB_Id > 0) {
                $scope.Deletegrnrows(data);
            }
        };
        //====================================== Page Load
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("INV_OpeningBalance/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_Store = promise.get_Store;
                    $scope.get_item = promise.get_item;
                    $scope.get_openingbalance = promise.get_openingbalance;
                    $scope.presentCountgrid = $scope.get_openingbalance.length;
                    $scope.get_obdetails = promise.get_obdetails;
                });
        };
        //===========Get Item deatils On item Change
        $scope.onitemchange = function (itemid, objid) {
            $scope.get_itemTax1 = [];
            var data = {
                "INVMI_Id": itemid.invmI_Id
            };
            apiService.create("INV_T_GRN/getitemDetail", data).
                then(function (promise) {
                    $scope.get_itemDetail = promise.get_itemDetail;
                    angular.forEach($scope.transrows, function (obj) {
                        if (obj.itrS_Id === objid.itrS_Id) {
                            obj.invmuoM_UOMName = $scope.get_itemDetail[0].invmuoM_UOMName;
                            obj.invmuoM_Id = $scope.get_itemDetail[0].invmuoM_Id;
                            var date = new Date();
                            obj.invoB_PurchaseDate = date;
                            obj.invoB_BatchNo = "0";
                            obj.invoB_Qty = 0.00;
                            obj.invstO_PurchaseRate = 0.00;
                            obj.invstO_SalesRate = 0.00;
                        }
                    });

                });
        };

        //===================================== SAVE DATA

        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.arrayOB = [];
                if ($scope.editS === true) {
                    angular.forEach($scope.transrows, function (ob) {
                        $scope.arrayOB.push({
                            invmI_Id: ob.invmI_Id, invoB_PurchaseDate: ob.invoB_PurchaseDate, invmuoM_Id: ob.invmuoM_Id, invoB_BatchNo: ob.invoB_BatchNo,
                            invoB_PurchaseRate: ob.invstO_PurchaseRate, invoB_SaleRate: ob.invstO_SalesRate, invoB_Qty: ob.invoB_Qty,
                            invoB_Naration: ob.invoB_Naration, invoB_DepreciatedValue: ob.invoB_DepreciatedValue, invoB_DepreciationAsOnDate: ob.invoB_DepreciationAsOnDate
                        });
                    });
                }
                else {
                    angular.forEach($scope.transrows, function (ob) {
                        $scope.arrayOB.push({
                            invmI_Id: ob.invmI_Id.invmI_Id, invoB_PurchaseDate: ob.invoB_PurchaseDate, invmuoM_Id: ob.invmuoM_Id, invoB_BatchNo: ob.invoB_BatchNo, invoB_MfgDate: ob.invoB_MfgDate,
                            invoB_ExpDate: ob.invoB_ExpDate, invoB_PurchaseRate: ob.invstO_PurchaseRate, invoB_SaleRate: ob.invstO_SalesRate, invoB_Qty: ob.invoB_Qty,
                            invoB_Naration: ob.invoB_Naration, invoB_DepreciatedValue: ob.invoB_DepreciatedValue, invoB_DepreciationAsOnDate: ob.invoB_DepreciationAsOnDate
                        });
                    });
                }
                var data = {
                    "INVMST_Id": $scope.invmsT_Id,
                    "INVOB_Id": $scope.invoB_Id,
                    "OBItem": $scope.arrayOB,
                    "IMFY_Id": $scope.IMFY_Id
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("INV_OpeningBalance/savedetails", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.invoB_Id === 0 || promise.invoB_Id < 0) {
                            if (promise.returnduplicatestatus === "Updated") {
                                swal('Record saved successfully / Stock Updated');
                            }
                            else if (promise.returnduplicatestatus === "notUpdated") {
                                swal('Record saved successfully / Failed to update Stock');
                            }

                        }
                        else if (promise.invoB_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invoB_Id === 0 || promise.invoB_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invoB_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $state.reload();
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

        $scope.deactive = function (item, SweetAlert) {
            $scope.INVOB_Id = item.invoB_Id;
            var dystring = "";
            if (item.invoB_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invoB_ActiveFlg === false) {
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
                        apiService.create("INV_OpeningBalance/deactive", item).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.edit = function (item) {
            $scope.get_obdetails = [];
            $scope.transrows = [];
            $scope.invmsT_Id = item.invmsT_Id;
            $scope.invoB_Id = item.invoB_Id;
            $scope.editS = true;
            var invoB_Id = item.invoB_Id;
            var data = {
                "INVOB_Id": invoB_Id
            };
            apiService.create("INV_OpeningBalance/getobdetails", data).
                then(function (promise) {
                    $scope.get_obdetails = promise.get_obdetails;
                    angular.forEach($scope.get_obdetails, function (objr) {
                        $scope.transrows.push({
                            invmI_Id: objr.invmI_Id, invmI_ItemName: objr.invmI_ItemName, invmuoM_Id: objr.invmuoM_Id, invmuoM_UOMName: objr.invmuoM_UOMName,
                            invoB_BatchNo: objr.invoB_BatchNo, invoB_PurchaseDate: new Date(objr.invoB_PurchaseDate), invoB_Qty: objr.invoB_Qty,
                            invoB_MfgDate: objr.invoB_MfgDate, invstO_PurchaseRate: objr.invoB_PurchaseRate, invoB_ExpDate: objr.invoB_ExpDate, invstO_SalesRate: objr.invoB_SaleRate,
                            invoB_Naration: objr.invoB_Naration, invoB_DepreciatedValue: objr.invoB_DepreciatedValue, invoB_DepreciationAsOnDate: objr.invoB_DepreciationAsOnDate
                        });
                    });

                });
        };


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
        $scope.getView = function (item) {
            $scope.invoBBB_Id = "";
            $scope.invmS_StoreName = item.invmS_StoreName;
            $scope.invmI_ItemName = item.invmI_ItemName;
            $scope.invoB_Qty = item.invoB_Qty;
            $scope.invoB_PurchaseDate = item.invoB_PurchaseDate;
            $scope.invoB_PurchaseRate = item.invoB_PurchaseRate;
            $scope.invoB_SaleRate = item.invoB_SaleRate;
            $scope.invoBBB_Id = item.invoB_Id;
            $scope.GroupName = item.invmG_GroupName;
            $('#viewDeatils').modal('show');
        }
        $scope.interactedone = function (field) {
            return $scope.submittedone;
        };
        $scope.closeDeparture = function () {
            $scope.submittedone = false;
        }
        $scope.submittedone = false;
        $scope.savedeparture = function () {
            $scope.submittedone = true;
            if ($scope.myFormtwo.$valid) {
                var data = {
                    "INVOB_Id": $scope.invoBBB_Id,
                    "INVOB_DepreciatedValue": $scope.INVOB_DepreciatedValue,
                    "INVOB_DepreciationAsOnDate": new Date($scope.INVOB_DepreciationAsOnDate).toDateString()
                };
                apiService.create("INV_OpeningBalance/savedeparture", data).
                    then(function (promise) {
                        if (promise.returnduplicatestatus === "Updated") {
                            swal('Record saved successfully / Stock Updated');
                        }
                        else if (promise.returnduplicatestatus === "notUpdated") {
                            swal('Failed to update Stock');
                        }
                        else if (promise.returnduplicatestatus === "adminstartor") {
                            swal('Failed to save, please contact administrator !');
                        }
                        $('#viewDeatils').modal('hide');
                        //$state.reload();
                        $scope.INVOB_DepreciatedValue = "";
                        $scope.invoBBB_Id = "";
                        $scope.loaddata();
                    })
            }
            //myFormtwo
           
        };
    }
})();