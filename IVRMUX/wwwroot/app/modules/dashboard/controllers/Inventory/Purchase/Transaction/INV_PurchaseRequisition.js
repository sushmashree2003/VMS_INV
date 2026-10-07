
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_PurchaseRequisitionController', INV_PurchaseRequisitionController)

    INV_PurchaseRequisitionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function INV_PurchaseRequisitionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var date = new Date();
        $scope.invmpR_PRDate = date;
        $scope.addro = false;
        $scope.obj = {};
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
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        //=====================Adding and removing new row in transcation Grid============      
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        if ($scope.transrows.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addprrows = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.transrows.length > 1) {
                    for (var i = 0; i == $scope.transrows.length; i++) {
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
        $scope.removeprrows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);

            if (data.amstB_Id > 0) {
                $scope.Deleteprrows(data);
            }
            if ($scope.transrows.length == 0) {
            }
        };

        //add multiple client names
        $scope.clientnames_rem = [{ itrS_Id1: 'trans12' }];
        if ($scope.clientnames_rem.length === 1) {
            $scope.cnt1 = 1;
        }
        $scope.addgrnrowsclient = function () {
            if ($scope.clientnames_rem.length > 1) {
                for (var x = 0; x === $scope.clientnames_rem.length; x++) {
                    var id1 = $scope.clientnames_rem[x].itrS_Id1;
                    var lastChar1 = id1.substr(id1.length - 1);
                    $scope.cnt1 = parseInt(lastChar1);
                }
            }
            $scope.cnt1 = $scope.cnt1 + 1;
            $scope.tet = 'trans' + $scope.cnt1;
            var newItemNo1 = $scope.cnt1;
            $scope.clientnames_rem.push({ 'itrS_Id1': 'trans12' + newItemNo1 });
        };

        $scope.removeclient = function (index, data) {
            var newItemNo = $scope.clientnames_rem.length - 1;
            $scope.clientnames_rem.splice(index, 1);

        };


        //====================== Auto Generated Number
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};

        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag == "INVPR") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = paginationformasters;
        //====================================== Page Load
        $scope.loaddata = function () {

            $scope.search = "";
            var pageid = 2;
            apiService.getURI("INV_PurchaseRequisition/getloaddata", pageid).then(function (promise) {
                $scope.get_mi_list = promise.get_mi_list;
                $scope.MI_Id = promise.mI_Id;
                $scope.get_Store = promise.get_Store;
                $scope.get_supplier = promise.get_supplier;
                $scope.get_item = promise.get_item;
                $scope.get_employee = promise.get_employee;
                $scope.get_purchaserequisition = promise.get_purchaserequisition;
                $scope.getClient = promise.client_Master;

                $scope.hrmE_Id_temp = promise.hrmE_Id;
                if (promise.hrmE_Id === undefined || promise.hrmE_Id === null || promise.hrmE_Id === 0) {
                    swal("This User Don't Have privilages To Enter Details");
                }

            });
        };
        $scope.OnChangeinst = function (mi_id) {
            $scope.get_Store = [];
            $scope.get_supplier = [];
            $scope.get_item = [];
            $scope.get_employee = [];
            $scope.get_purchaserequisition = [];
            var data = {
                "MI_IdNew": mi_id
            }

            apiService.create("INV_PurchaseRequisition/GetOnchangeData", data).then(function (promise) {

                var transnumconfigsettings = promise.transnumconfig;
                for (var i = 0; i < transnumconfigsettings.length; i++) {
                    if (transnumconfigsettings.length > 0) {
                        if (transnumconfigsettings[i].imN_Flag == "INVPR") {
                            $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                        }
                    }
                }



                $scope.get_Store = promise.get_Store;
                $scope.get_supplier = promise.get_supplier;
                $scope.get_item = promise.get_item;
                $scope.get_employee = promise.get_employee;
                $scope.get_purchaserequisition = promise.get_purchaserequisition;
            });
        }
        //====================================== On Item Change
        $scope.onitemchange = function (itemid, objid) {
            var item_id = itemid.invmI_Id;
            var data = {
                "INVMI_Id": item_id
            }
            apiService.create("INV_PurchaseRequisition/getitemDetail", data).
                then(function (promise) {
                    $scope.get_itemDetail = promise.get_itemDetail;
                    angular.forEach($scope.transrows, function (obj) {
                        if (obj.itrS_Id == objid.itrS_Id) {
                            obj.invmuoM_UOMName = $scope.get_itemDetail[0].invmuoM_UOMName;

                        }
                    })
                })
        }

        //================== Count Amount
        $scope.countitemAmt = function (probj, items) {
            var a = 0;
            $scope.purrate = parseFloat(probj.invtpR_PRUnitRate);
            $scope.qty = parseFloat(probj.invtpR_PRQty);
            probj.invtpR_ApproxAmount = $scope.purrate * $scope.qty;
            angular.forEach($scope.transrows, function (obj) {
                a += parseFloat(obj.invtpR_ApproxAmount);
            })
            var totamt = a;
            $scope.invmpR_ApproxTotAmount = totamt;
            $scope.invmpR_ApproxTotAmount = parseFloat($scope.invmpR_ApproxTotAmount);
            $scope.invmpR_ApproxTotAmount = $scope.invmpR_ApproxTotAmount.toFixed(2);
        }

        //===================================== SAVE DATA

        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.arrayPR = [];
                angular.forEach($scope.transrows, function (pr) {
                    $scope.arrayPR.push({
                        invtpR_Id: pr.invtpR_Id, invmI_Id: pr.invmI_Id.invmI_Id, invmuoM_Id: pr.invmI_Id.invmuoM_Id, invtpR_PRQty: pr.invtpR_PRQty, invtpR_PRUnitRate: pr.invtpR_PRUnitRate,
                        invtpR_ApproxAmount: pr.invtpR_ApproxAmount, invtpR_Remarks: pr.invtpR_Remarks
                    });
                })

                $scope.clientnames = [];
                angular.forEach($scope.clientnames_rem, function (itmx) {
                    $scope.clientnames.push({
                        ISMMCLT_Id: itmx.Client_Id.ismmclT_Id, INVMPRCLT_Remarks: itmx.INVMPRCLT_Remarks
                    });
                })

                var data = {
                    "INVMPR_Remarks": $scope.invmpR_Remarks,
                    "INVMPR_PRDate": $scope.invmpR_PRDate,
                    "INVMPR_ApproxTotAmount": $scope.invmpR_ApproxTotAmount,
                    "arrayPR": $scope.arrayPR,
                    "INVMPR_Id": $scope.invmpR_Id,
                    "MI_IdNew": $scope.MI_Id,
                    "clientnames": $scope.clientnames,
                    transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("INV_PurchaseRequisition/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.invmpR_Id == 0 || promise.invmpR_Id < 0) {
                            swal('Record saved successfully');
                            $state.reload();
                        }
                        else if (promise.invmpR_Id > 0) {
                            swal('Record updated successfully');
                            $state.reload();
                        }
                        $scope.OnChangeinst($scope.MI_Id);
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                        $scope.OnChangeinst($scope.MI_Id);
                    }
                    else {
                        if (promise.invmpR_Id == 0 || promise.invmpR_Id < 0) {
                            swal('Failed to save, please contact administrator');
                            $scope.OnChangeinst($scope.MI_Id);
                        }
                        else if (promise.invmpR_Id > 0) {
                            swal('Failed to update, please contact administrator');
                            $state.reload();
                        }
                    }


                })
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
        }

        $scope.deactiveM = function (item, SweetAlert) {
            $scope.invmpR_Id = item.invmpR_Id;
            var data = {
                "INVMPR_Id": item.invmpR_Id,
                "MI_IdNew": $scope.MI_Id,
            }
            var dystring = "";
            if (item.invmpR_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invmpR_ActiveFlg == false) {
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
                        apiService.create("INV_PurchaseRequisition/deactiveM", data).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }

                                $scope.OnChangeinst($scope.MI_Id);
                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.deactive = function (item, SweetAlert) {
            $scope.invtpR_Id = item.invtpR_Id;
            var data = {
                "INVTPR_Id": item.invtpR_Id
            }
            var dystring = "";
            if (item.invtpR_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invtpR_ActiveFlg == false) {
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
                        apiService.create("INV_PurchaseRequisition/deactive", data).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }

                                $scope.onformclick(item);
                                $scope.loaddata();
                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }




        $scope.edit = function (item, row) {
            $scope.transrows = [];

            $scope.invmpR_ApproxTotAmount = item.invmpR_ApproxTotAmount;
            $scope.invmpR_Remarks = item.invmpR_Remarks;
            $scope.invmpR_PRDate = new Date(item.invmpR_PRDate);
            $scope.invmpR_Id = item.invmpR_Id;
            $scope.invtpR_Id = item.invtpR_Id;
            $scope.edits = true;
            var invmpR_Id = item.invmpR_Id;
            var data = {
                "INVMPR_Id": invmpR_Id,
                "MI_IdNew": $scope.MI_Id
            }
            apiService.create("INV_PurchaseRequisition/edit", data).
                then(function (promise) {
                    $scope.editPR = promise.editPR;
                    angular.forEach($scope.editPR, function (obje) {
                        $scope.transrows.push(obje);
                    })
                })
        }

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
        $scope.searchValue2 = '';

        //IndentRemark
        $scope.IndentRemark = function (user) {
            //$('#myModal').modal('hide');
            $('#myModalfile').modal('hide');
            $scope.get_IndentDetail = [];
            $scope.get_prDetail = [];

            var data = {
                "MI_Id": $scope.MI_Id,
                "INVMPR_Id": user.invmpR_Id,
                "Flag": "Purchase_Indent"
            }
            apiService.create("INV_RequisitionNotification/savedetails", data).
                then(function (promise) {
                    if (promise.get_Details != null && promise.get_Details.length > 0) {
                        $scope.get_IndentDetail = promise.get_Details;
                        $scope.prnum = user.invmpR_PRNo;
                        $scope.logo = promise.logo;
                        $('#myModalfile').modal('show');

                    }
                    else {
                        Swal("Record Not Found !");
                    }
                })
        }
        //OrderAp_Remark
        $scope.OrderAp_Remark = function (user) {
            $('#OrderAp_Remark').modal('hide');
            $scope.get_IndentDetail = [];
            $scope.get_prDetail = [];

            var data = {
                "MI_Id": $scope.MI_Id,
                "INVMPR_Id": user.invmpR_Id,
                "Flag": "OrderAp_Remark"
            }
            apiService.create("INV_RequisitionNotification/savedetails", data).
                then(function (promise) {
                    if (promise.get_Details != null && promise.get_Details.length > 0) {
                        $scope.get_ApprovalDetail = promise.get_Details;
                        $scope.prnum = user.invmpR_PRNo;
                        $scope.reqname = $scope.get_ApprovalDetail[0].INVMPO_PONo;
                        $scope.INVMPO_PODate = $scope.get_ApprovalDetail[0].INVMPO_PODate;


                        $scope.logo = promise.logo;
                        $('#OrderAp_Remark').modal('show');

                    }
                    else {

                        swal("Order Not Approved  !");
                    }
                })
        }
        $scope.OrderRemark = function (user) {
            $('#Order_Remark').modal('hide');
            $scope.get_IndentDetail = [];
            $scope.get_prDetail = [];

            var data = {
                "MI_Id": $scope.MI_Id,
                "INVMPR_Id": user.invmpR_Id,
                "Flag": "Order_Remark"
            }
            apiService.create("INV_RequisitionNotification/savedetails", data).
                then(function (promise) {
                    if (promise.get_Details != null && promise.get_Details.length > 0) {
                        $scope.get_ApprovalDetail = promise.get_Details;
                        $scope.prnum = user.invmpR_PRNo;
                        $scope.logo = promise.logo;
                        $('#Order_Remark').modal('show');

                    }
                    else {

                        swal(" Order Not Apply  !");
                    }
                })
        }
        //get_ApprovalDetail
        $scope.ApprovalRemark = function (user) {
            //$('#myModal').modal('hide');
            $('#myApproval').modal('hide');
            $scope.get_IndentDetail = [];
            $scope.get_prDetail = [];

            var data = {
                "MI_Id": $scope.MI_Id,
                "INVMPR_Id": user.invmpR_Id,
                "Flag": "Purchase_Approved"
            }
            apiService.create("INV_RequisitionNotification/savedetails", data).
                then(function (promise) {
                    if (promise.get_Details != null && promise.get_Details.length > 0) {
                        $scope.get_ApprovalDetail = promise.get_Details;
                        $scope.prnum = user.invmpR_PRNo;
                        $scope.logo = promise.logo;
                        $('#myApproval').modal('show');

                    }
                    else {
                        // Swal("Record Not Found !");
                        swal("Not Approved First Level  !");
                    }
                })
        }
        $scope.onformclick = function (itemid) {
            $scope.get_prDetail = "";
            var item_id = itemid.invmpR_Id;
            var data = {
                "INVMPR_Id": item_id,
                "MI_IdNew": $scope.MI_Id
            }
            apiService.create("INV_PurchaseRequisition/get_prdetails", data).
                then(function (promise) {
                    $scope.get_prDetail = promise.get_prDetail;
                    $scope.prnum = $scope.get_prDetail[0].invmpR_PRNo;
                    $scope.reqname = $scope.get_prDetail[0].employeename;
                    console.log($scope.get_prDetail);
                })
        }

    }
})();