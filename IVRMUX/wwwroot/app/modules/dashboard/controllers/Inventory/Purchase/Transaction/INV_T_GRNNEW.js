
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_T_GRNNEWController', INV_T_GRNNEWController);
    INV_T_GRNNEWController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache','$q'];
    function INV_T_GRNNEWController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q) {


        $scope.objg = {};
        var date = new Date();
        $scope.discounttype = "A";
        $scope.invmgrN_PurchaseDate = date;
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
        //  $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        //====================== Auto Generated Number
        $scope.transnumbconfigurationsettings = [];
        $scope.serivicenumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        $scope.editS = false;
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        $scope.getiteDetails = [];
        if (transnumconfigsettings != null && transnumconfigsettings.length > 0) {
            for (var i = 0; i < transnumconfigsettings.length; i++) {
                if (transnumconfigsettings[i].imN_Flag === "INVGRN") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
                if (transnumconfigsettings[i].imN_Flag === "INVSRN") {
                    $scope.serivicenumbconfigurationsettings = serivicenumbconfigurationsettings[i];
                }

            }
        }
        $scope.search_box = function () {
            if ($scope.searchValue !== "" || $scope.searchValue !== null) {
                $scope.searc_button = false;
            }
            else {
                $scope.searc_button = true;
            }
        };

        $scope.obj.taxappcable = false;
        //=====================Adding and removing new row in transcation Grid============      
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        if ($scope.transrows.length === 1) {
            $scope.cnt = 1;
        }
        $scope.materaldocuupload = [];
        $scope.materaldocuupload = [{ id: 'materal' }];
        $scope.addgrnfile = function () {
            var newItemNo = $scope.materaldocuupload.length + 1;
            if (newItemNo <= 50) {
                $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
            }
        };
        $scope.addgrnrows = function () {
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
        $scope.Deletegrnrows = function () {
            var invmgrN_TotalAmount = 0; var invmgrN_TotDiscount = 0; var invmgrN_TotTaxAmt = 0;
            if ($scope.transrows != null && $scope.transrows.length > 0) {
                for (var i = 0; i < $scope.transrows.length; i++) {
                    invmgrN_TotalAmount += (Number($scope.transrows[i].invtgrN_PurchaseRate) * (Number($scope.transrows[i].invtgrN_Qty)));
                    // invmgrN_TotalAmount += (Number($scope.transrows[i].invtgrN_Amount));
                    invmgrN_TotDiscount += (Number($scope.transrows[i].invtgrN_DiscountAmt));
                    invmgrN_TotTaxAmt += (Number($scope.transrows[i].invtgrN_TaxAmt));
                }
            }
            $scope.invmgrN_TotalAmount = invmgrN_TotalAmount.toFixed(2);
            $scope.invmgrN_TotTaxAmt = invmgrN_TotTaxAmt.toFixed(2);
            $scope.invmgrN_TotDiscount = invmgrN_TotDiscount.toFixed(2);
            $scope.invmgrN_PurchaseValue = (Number($scope.invmgrN_TotalAmount) + (Number($scope.invmgrN_TotTaxAmt)) - (Number($scope.invmgrN_TotDiscount))).toFixed(2);


        }
        $scope.removegrnrows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);

            $scope.Deletegrnrows(data);

        };
        $scope.removegrnfiles = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

            if ($scope.materaldocuupload.length === 0) {               //data
            }
        };
        //==============================Page Load========================

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.searchvalue = "";
            var pageid = 2;
            apiService.getURI("INV_T_GRN/getloaddata", pageid).
                then(function (promise) {
                    $scope.getiteDetails = promise.get_item;


                    $scope.get_Store = promise.get_Store;
                    $scope.get_supplier = promise.get_supplier;
                    //$scope.get_tax = promise.get_tax;
                    $scope.get_taxlist = promise.get_tax;

                    $scope.get_GRN = promise.get_GRN;
                    $scope.presentCountgrid = $scope.get_GRN.length;
                    $scope.edit1 = false;
                    // $scope.get_tax = promise.taxlist;
                    $scope.getgrroup = promise.group;
                    $scope.RDIchaNge();
                });
        };
        $scope.GraupIdFilter = function (invmG_Id) {
            if (invmG_Id != null && invmG_Id > 0) {
                angular.forEach($scope.transrows, function (item) {
                    item.invmG_Id = invmG_Id;
                });
            }
            else {
                angular.forEach($scope.transrows, function (item) {
                    item.invmG_Id = "";
                });
            }
        }
        $scope.FireAsset = function (All_AssetTypeFlag) {
            if (All_AssetTypeFlag != null && All_AssetTypeFlag != "") {
                if ($scope.transrows != null && $scope.transrows.length > 0) {
                    angular.forEach($scope.transrows, function (item) {
                        item.invtgrN_AssetTypeFlag = All_AssetTypeFlag;
                    });
                }
            }
            else {
                if ($scope.transrows != null && $scope.transrows.length > 0) {
                    angular.forEach($scope.transrows, function (item) {
                        item.invtgrN_AssetTypeFlag = "";
                    });
                }
            }
        }
        $scope.RDIchaNge = function () {
            $scope.get_item = [];
            if ($scope.getiteDetails != null && $scope.getiteDetails.length > 0) {
                angular.forEach($scope.getiteDetails, function (item) {
                    if ($scope.obj.optionflag == item.invmI_ItemType) {
                        $scope.get_item.push(item);
                    }
                });

            }
        }

        //===========Item Change Transcation Grid
        $scope.onitemchange = function (itemid, objid) {
            $scope.get_itemTax1 = [];
            var item_id = itemid.invmI_Id;
            var data = {
                "INVMI_Id": item_id
            };
            apiService.create("INV_T_GRN/getitemDetail", data).
                then(function (promise) {
                    $scope.get_itemDetail = promise.get_itemDetail;
                    $scope.get_itemTax1 = promise.get_itemTax;
                    angular.forEach($scope.transrows, function (obj) {
                        if (obj.itrS_Id === objid.itrS_Id) {
                            obj.invmuoM_Id = $scope.get_itemDetail[0].invmuoM_UOMName;
                            obj.get_itemTax = $scope.get_itemTax1;
                            $scope.get_itemTax = $scope.get_itemTax1;
                            obj.invtgrN_DiscountAmt = 0.00;
                            obj.invtgrN_PurchaseRate = 0.00;
                            obj.invtgrN_MRP = 0.00;
                            obj.invtgrN_SalesPrice = 0.00;
                            obj.invtgrN_Qty = 0.00;
                            obj.invtgrN_Amount = 0.00;
                            obj.invtgrN_TaxAmt = 0.00;
                            obj.discountper = 0.00;
                        }
                    });
                    if ($scope.invmgrN_TotalAmount === 0 || $scope.invmgrN_TotalAmount === "" || $scope.invmgrN_TotalAmount === undefined || $scope.invmgrN_TotalAmount === "NaN") {
                        $scope.invmgrN_TotalAmount = 0.00;
                    }
                    if ($scope.invmgrN_TotDiscount === 0 || $scope.invmgrN_TotDiscount === "" || $scope.invmgrN_TotDiscount === undefined || $scope.invmgrN_TotDiscount === "NaN") {
                        $scope.invmgrN_TotDiscount = 0.00;
                    }
                    if ($scope.invmgrN_TotTaxAmt === 0 || $scope.invmgrN_TotTaxAmt === "" || $scope.invmgrN_TotTaxAmt === undefined || $scope.invmgrN_TotTaxAmt === "NaN") {
                        $scope.invmgrN_TotTaxAmt = 0.00;
                    }

                });
        };
        // View Tax Model
        $scope.index = 0;
        $('#myModal').modal('hide');
        $scope.viewtxfn = function (qwe, ind) {
            $scope.index = ind; $scope.get_itemTax = []; $scope.get_itemTaxtemp = [];
            $scope.purrate = parseFloat(qwe.invtgrN_PurchaseRate);
            $scope.qty = parseFloat(qwe.invtgrN_Qty);
            $scope.totdiscout = parseFloat(qwe.invtgrN_DiscountAmt);
            $scope.get_itemTaxtemp = qwe.get_itemTax;
            $('#myModal').modal('show');
            if ($scope.editS === true) {
                for (var i = 0; i < $scope.get_itemTaxtemp.length; i++) {
                    $scope.get_itemTax.push({
                        invmI_ItemName: $scope.get_itemTaxtemp[i].invmI_ItemName,
                        invmT_TaxName: $scope.get_itemTaxtemp[i].invmT_TaxName,
                        invmT_TaxAliasName: $scope.get_itemTaxtemp[i].invmT_TaxAliasName,
                        invmiT_TaxValue: $scope.get_itemTaxtemp[i].invmiT_TaxValue,
                        invtgrN_TaxAmt: $scope.get_itemTaxtemp[i].invtgrN_TaxAmt,
                        pindex: ind

                    });
                }
            }
            else {
                // $scope.get_itemTaxtemp = qwe.get_itemTax;
                $scope.get_itemTax = qwe.get_itemTax;
            }



            var cnt = 0;
            $scope.cntax = 0;
            angular.forEach($scope.get_itemTax, function (wer) {
                wer.itrS_Id = qwe.itrS_Id;
                $scope.totpurrate = $scope.purrate * $scope.qty;
                if (qwe.invtgrN_DiscountAmt === undefined || qwe.invtgrN_DiscountAmt === "0" || qwe.invtgrN_DiscountAmt === "" || qwe.invtgrN_DiscountAmt === "NaN") {
                    $scope.finalpurrate = $scope.totpurrate;
                }
                else {
                    $scope.finalpurrate = $scope.totpurrate - $scope.totdiscout;
                }
                var taxamt = ($scope.finalpurrate / 100) * wer.invmiT_TaxValue;
                wer.invtgrN_TaxAmt = taxamt;//.toFixed(2);
                wer.invtgrN_TaxAmt = parseFloat(wer.invtgrN_TaxAmt);
                wer.invtgrN_TaxAmt = wer.invtgrN_TaxAmt.toFixed(2);

                cnt = cnt + wer.invtgrN_TaxAmt;
                $scope.cntax = cnt;

            });

            //  $scope.get_itemTax = qwe.get_itemTax;
        };
        //==========================Tax Add To Cart
        $scope.arrayGRNtax = [];

        $scope.gettax = function (objg, user) {
            $scope.objgUU = [];
            $scope.objgUU = objg;
            angular.forEach($scope.get_taxlist, function (ppp) {
                ppp.xyz = false;
                ppp.invmiT_TaxValue = "";

            });

            if ($scope.editS == true) {


                var data = {
                    "INVTGRN_Id": user.invtgrN_Id
                };
                apiService.create("INV_T_GRN/getitemDetail", data).
                    then(function (promise) {
                        //get_itemTax
                        if (promise.get_itemTax != null && promise.get_itemTax.length > 0) {
                            angular.forEach($scope.get_taxlist, function (ppp) {
                                angular.forEach(promise.get_itemTax, function (pppp) {
                                    if (pppp.invmT_Id == ppp.invmT_Id) {
                                        ppp.xyz = true;
                                        ppp.invmiT_TaxValue = pppp.invtgrnT_TaxPer;
                                    }

                                });
                            });
                        }

                    });
            }

            $('#myModalTax').modal('show');
        }
        $scope.addtocart = function () {
            var TotalTax = 0;
            $scope.arrayGRNtax = [];

            if ($scope.objgUU != null) {
                if ($scope.transrows != null && $scope.transrows.length > 0) {
                    angular.forEach($scope.get_taxlist, function (ppp) {
                        if (ppp.xyz == true) {
                            TotalTax = (Number(TotalTax) + Number(ppp.invmiT_TaxValue)).toFixed(2);
                            $scope.arrayGRNtax.push({
                                invmT_Id: ppp.invmT_Id,
                                invtgrnT_TaxPer: ppp.invmiT_TaxValue,

                            });
                        }
                    });

                    for (var i = 0; i < $scope.transrows.length; i++) {
                        if (i == $scope.objgUU) {
                            $scope.transrows[i].invtgrN_TaxAmt = (Number((TotalTax) * (Number($scope.transrows[i].invtgrN_Amount))) / 100);
                            $scope.transrows[i].txxxx = $scope.arrayGRNtax;
                            $scope.transrows[i].taxpercent = TotalTax;
                            $scope.transrows[i].Totall_Amount = (Number($scope.transrows[i].invtgrN_Amount) + (Number($scope.transrows[i].invtgrN_TaxAmt)));

                        }

                    }



                    $scope.Deletegrnrows();
                }
                else {
                    swal("Enter Amount !");
                }
            }



        }


        $scope.taxchange = function (grnobj) {
            var Totall_Amount = 0;

            var tamt = parseFloat($scope.invmgrN_TotalAmount);
            var tdiscout = parseFloat($scope.invmgrN_TotDiscount);
            var ttax = parseFloat($scope.invmgrN_TotTaxAmt);
            if ($scope.invmgrN_TotTaxAmt === undefined || $scope.invmgrN_TotTaxAmt === "") {
                ttax = 0;
            }
            var purvalur = tamt - tdiscout + ttax;
            $scope.invmgrN_PurchaseValue = purvalur;//.toFixed(2);
            $scope.invmgrN_PurchaseValue = parseFloat($scope.invmgrN_PurchaseValue);
            $scope.invmgrN_PurchaseValue = $scope.invmgrN_PurchaseValue.toFixed(2);

        };

        $scope.countDiscount = function (grnobj, items) {
            var d = 0;
            var tm = 0;
            $scope.purrate = parseFloat(grnobj.invtgrN_PurchaseRate);
            $scope.qty = parseFloat(grnobj.invtgrN_Qty);
            tm = $scope.purrate * $scope.qty;
            $scope.totamt = tm.toFixed(2);
            if ($scope.discounttype === 'A') {
                $scope.totdiscout = parseFloat(grnobj.invtgrN_DiscountAmt);
            }
            else if ($scope.discounttype === 'P') {
                var discotamt = ($scope.totamt / 100) * grnobj.discountper;
                var discountAmt = parseFloat(discotamt);
                grnobj.invtgrN_DiscountAmt = discountAmt.toFixed(2);
                $scope.totdiscout = grnobj.invtgrN_DiscountAmt;
            }
            else {
                $scope.totdiscout = parseFloat(grnobj.invtgrN_DiscountAmt);
            }
            $scope.finalpurrate = $scope.totamt - $scope.totdiscout;
            grnobj.invtgrN_Amount = $scope.finalpurrate.toFixed(2);
            angular.forEach($scope.transrows, function (obj) {
                if (obj.invtgrN_DiscountAmt === "") {
                    obj.invtgrN_DiscountAmt = 0;
                }
                d += parseFloat(obj.invtgrN_DiscountAmt);
            });
            var discamt = d;
            $scope.invmgrN_TotDiscount = discamt;
            $scope.invmgrN_TotDiscount = parseFloat($scope.invmgrN_TotDiscount);
            $scope.invmgrN_TotDiscount = $scope.invmgrN_TotDiscount.toFixed(2);
            if ($scope.transrows != null && $scope.transrows.length > 0) {
                angular.forEach($scope.transrows, function (obj) {
                    obj.invtgrTotalAmount = (Number(obj.taxamt) + Number(obj.invtgrN_Amount)).toFixed(2);
                });
            }

        };

        $scope.countitemAmt = function (grnobj, items) {
            var a = 0;
            var ab = 0;
            var ds = 0;

            $scope.invmgrN_TotTaxAmt = parseFloat($scope.invmgrN_TotTaxAmt) - parseFloat(grnobj.invtgrN_TaxAmt);
            grnobj.invtgrN_TaxAmt = 0;
            if (grnobj.invtgrN_DiscountAmt != null && grnobj.invtgrN_DiscountAmt != undefined) {
                $scope.invmgrN_TotDiscount = parseFloat($scope.invmgrN_TotDiscount) - parseFloat(grnobj.invtgrN_DiscountAmt);
            }

            grnobj.discountper = 0;
            grnobj.invtgrN_DiscountAmt = 0;
            $scope.purrate = parseFloat(grnobj.invtgrN_PurchaseRate);
            $scope.qty = parseFloat(grnobj.invtgrN_Qty);
            $scope.totdiscout = parseFloat(grnobj.invtgrN_DiscountAmt);
            ab = $scope.purrate * $scope.qty;
            grnobj.invtgrN_Amount = parseFloat(ab);
            grnobj.invtgrN_Amount = grnobj.invtgrN_Amount.toFixed(2);

            angular.forEach($scope.transrows, function (obj) {
                ds += parseFloat(obj.invtgrN_Amount) + parseFloat(obj.invtgrN_DiscountAmt);
                a = ds.toFixed(2);
            });


            var totamt = a;
            $scope.invmgrN_TotalAmount = totamt;
            $scope.invmgrN_TotalAmount = parseFloat($scope.invmgrN_TotalAmount);
            $scope.invmgrN_TotalAmount = $scope.invmgrN_TotalAmount.toFixed(2);
            $scope.grnToatalcalculate();
        };

        $scope.grnToatalcalculate = function () {
            if ($scope.transrows != null && $scope.transrows.length > 0) {
                angular.forEach($scope.transrows, function (obj) {
                    var taxamt = 0; var invtgrN_Amount = 0;
                    if (obj.invtgrN_TaxAmt > 0) {
                        taxamt = obj.invtgrN_TaxAmt;
                    }
                    if (obj.invtgrN_Amount > 0) {
                        invtgrN_Amount = obj.invtgrN_Amount;
                    }
                    obj.invtgrTotalAmount = (Number(taxamt) + Number(invtgrN_Amount)).toFixed(2);
                });
            }
        }

        $scope.$watch('invmgrN_TotalAmount', function (totalamoutchange) {
            $scope.invmgrN_PurchaseValue = $scope.invmgrN_TotalAmount;
            $scope.invmgrN_PurchaseValue = parseFloat($scope.invmgrN_PurchaseValue);
            $scope.invmgrN_PurchaseValue = $scope.invmgrN_PurchaseValue.toFixed(2);
        });

        $scope.$watch('invmgrN_TotDiscount', function (totalDiscountchange) {

            var tamt = parseFloat($scope.invmgrN_TotalAmount);
            var tdiscout = parseFloat($scope.invmgrN_TotDiscount);
            var tottx = parseFloat($scope.invmgrN_TotTaxAmt);
            var purvalur = tamt - tdiscout + tottx;
            $scope.invmgrN_PurchaseValue = purvalur;//.toFixed(2);
            $scope.invmgrN_PurchaseValue = parseFloat($scope.invmgrN_PurchaseValue);
            $scope.invmgrN_PurchaseValue = $scope.invmgrN_PurchaseValue.toFixed(2);

        });

        $scope.$watch('invmgrN_TotTaxAmt', function (totalTaxchange) {
            var tamt = parseFloat($scope.invmgrN_TotalAmount);
            var tottx = parseFloat($scope.invmgrN_TotTaxAmt);
            var tdiscout = parseFloat($scope.invmgrN_TotDiscount);
            var purvalur = tamt + tottx - tdiscout;
            $scope.invmgrN_PurchaseValue = purvalur;//.toFixed(2);
            $scope.invmgrN_PurchaseValue = parseFloat($scope.invmgrN_PurchaseValue);
            $scope.invmgrN_PurchaseValue = $scope.invmgrN_PurchaseValue.toFixed(2);

        });
        $scope.$watch('obj.invtgrN_TaxAmt', function (objg) {
            var tx = 0.00;
            angular.forEach($scope.transrows, function (tobj) {
                if (tobj.itrS_Id === objg.itrS_Id) {
                    tx += parseFloat(tobj.invtgrN_TaxAmt);
                }
            });
            var totttx = tx;
            $scope.invmgrN_TotTaxAmt = totttx;//.toFixed(2);
            $scope.invmgrN_TotTaxAmt = parseFloat($scope.invmgrN_TotTaxAmt);
            $scope.invmgrN_TotTaxAmt = $scope.invmgrN_TotTaxAmt.toFixed(2);

        });
        //=======================Save Data
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.arrayGRN = [];
                $scope.grntemTax = [];
                $scope.get_taxlisttemp = [];


                if ($scope.invmgrN_InvoiceNo === null) {
                    $scope.invmgrN_InvoiceNo = undefined;
                }
                if ($scope.invmgrN_TotTaxAmt === null) {
                    $scope.invmgrN_TotTaxAmt = undefined;
                }
                if ($scope.invmgrN_Remarks === null) {
                    $scope.invmgrN_Remarks = undefined;
                }
                if ($scope.invmgrN_TotalAmount === 0 || $scope.invmgrN_TotalAmount === "" || $scope.invmgrN_TotalAmount === undefined || $scope.invmgrN_TotalAmount === "NaN") {
                    $scope.invmgrN_TotalAmount = 0.00;
                }
                if ($scope.invmgrN_TotDiscount === 0 || $scope.invmgrN_TotDiscount === "" || $scope.invmgrN_TotDiscount === undefined || $scope.invmgrN_TotDiscount === "NaN") {
                    $scope.invmgrN_TotDiscount = 0.00;
                }
                if ($scope.invmgrN_TotTaxAmt === 0 || $scope.invmgrN_TotTaxAmt === "" || $scope.invmgrN_TotTaxAmt === undefined || $scope.invmgrN_TotTaxAmt === "NaN") {
                    $scope.invmgrN_TotTaxAmt = 0.00;
                }
                if ($scope.invmgrN_PurchaseValue === 0 || $scope.invmgrN_PurchaseValue === "" || $scope.invmgrN_PurchaseValue === undefined || $scope.invmgrN_PurchaseValue === "NaN") {
                    $scope.invmgrN_PurchaseValue = $scope.invmgrNPurchaseValue;
                }
                if ($scope.edit1 === true) {
                    angular.forEach($scope.transrows, function (grn) {
                        $scope.arrayGRN.push({
                            invmI_Id: grn.invmI_Id.invmI_Id, invmuoM_Id: grn.invmI_Id.invmuoM_Id, invtgrN_BatchNo: grn.invtgrN_BatchNo, invtgrN_Qty: grn.invtgrN_Qty,
                            invtgrN_MfgDate: grn.invtgrN_MfgDate, invtgrN_ExpDate: grn.invtgrN_ExpDate, invtgrN_PurchaseRate: grn.invtgrN_PurchaseRate, invtgrN_MRP: grn.invtgrN_MRP,
                            invtgrN_DiscountAmt: grn.invtgrN_DiscountAmt, invtgrN_SalesPrice: grn.invtgrN_SalesPrice, invtgrN_Amount: grn.invtgrN_Amount, invtgrN_Naration: grn.invtgrN_Naration,
                            invtgrN_Id: grn.invtgrN_Id, invtgrN_TaxAmt: grn.invtgrN_TaxAmt,

                            GRNItemTax: grn.txxxx,
                            invtgrN_AssetTypeFlag: grn.invtgrN_AssetTypeFlag,
                            INVMG_Id: grn.invmG_Id,
                            INVTGRN_WithTaxFlg: grn.invtgrN_WithTaxFlg
                            //  INVMGRN_Id: grn.invmG_Id
                        });
                    });
                }
                else {
                    angular.forEach($scope.transrows, function (grn) {
                        $scope.arrayGRN.push({
                            invmI_Id: grn.invmI_Id.invmI_Id, invmuoM_Id: grn.invmI_Id.invmuoM_Id, invtgrN_BatchNo: grn.invtgrN_BatchNo, invtgrN_Qty: grn.invtgrN_Qty,
                            invtgrN_MfgDate: grn.invtgrN_MfgDate, invtgrN_ExpDate: grn.invtgrN_ExpDate, invtgrN_PurchaseRate: grn.invtgrN_PurchaseRate, invtgrN_MRP: grn.invtgrN_MRP,
                            invtgrN_DiscountAmt: grn.invtgrN_DiscountAmt, invtgrN_SalesPrice: grn.invtgrN_SalesPrice, invtgrN_Amount: grn.invtgrN_Amount, invtgrN_Naration: grn.invtgrN_Naration,
                            invtgrN_Id: grn.invtgrN_Id, invtgrN_TaxAmt: grn.invtgrN_TaxAmt,

                            GRNItemTax: grn.txxxx,
                            invtgrN_AssetTypeFlag: grn.invtgrN_AssetTypeFlag,
                            INVMG_Id: grn.invmG_Id,
                            INVTGRN_WithTaxFlg: grn.invtgrN_WithTaxFlg
                            // INVMGRN_Id: grn.invmG_Id

                        });
                    });

                }

                $scope.uploadarray = [];
                if ($scope.materaldocuupload != null && $scope.materaldocuupload.length > 0) {
                    angular.forEach($scope.materaldocuupload, function (itm) {
                        if (itm.vbabrsF_FilePath !== undefined && itm.vbabrsF_FilePath !== null && itm.vbabrsF_FilePath !== "") {
                            $scope.uploadarray.push({
                                VBABRSF_FileName: itm.vbabrsF_FileName,
                                VBABRSF_FilePath: itm.vbabrsF_FilePath,
                                VBABRSF_Filedesc: itm.vbabrsF_Filedesc
                            });
                        }
                    });
                }
                var data = {
                    "INVMGRN_GRNNo": $scope.invmgrN_GRNNo,
                    "INVMGRN_InvoiceNo": $scope.invmgrN_InvoiceNo,
                    "INVMGRN_PurchaseDate": $scope.invmgrN_PurchaseDate,
                    "INVMGRN_PurchaseValue": $scope.invmgrN_PurchaseValue,
                    "INVMGRN_TotDiscount": $scope.invmgrN_TotDiscount,
                    "INVMGRN_TotTaxAmt": $scope.invmgrN_TotTaxAmt,
                    "INVMGRN_TotalAmount": $scope.invmgrN_TotalAmount,
                    "INVMGRN_Remarks": $scope.invmgrN_Remarks,
                    "INVMGRN_POReference": $scope.invmgrN_POReference,
                    "INVMGRN_EWayBillRef": $scope.invmgrN_EWayBillRef,
                    "INVMGRN_TowardsProjects": $scope.invmgrN_TowardsProjects,
                    "INVMGRN_ReturnFlg": $scope.invmgrN_ReturnFlg,
                    "INVMGRN_PaidFlg": $scope.invmgrN_PaidFlg,
                    "INVMGRN_CreditFlg": $scope.invmgrN_CreditFlg,
                    "INVMGRN_Id": $scope.invmgrN_Id,
                    "INVMST_Id": $scope.invmsT_Id,
                    "INVMS_Id": $scope.invmS_Id,
                    "GRNItem": $scope.arrayGRN,
                    "optionflag": $scope.obj.optionflag,
                    "message": "GRNNEW",
                    filelist: $scope.uploadarray,
                    transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,

                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("INV_T_GRN/savedetails", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.invmgrN_Id === 0 || promise.invmgrN_Id < 0) {
                            swal('Record saved successfully');
                            $scope.edit1 = false;
                        }
                        else if (promise.invmgrN_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmgrN_Id === 0 || promise.invmgrN_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmgrN_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $scope.edit1 = false;
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

        $scope.taxModel = function () {

            if ($scope.transrows != null && $scope.transrows.length > 0) {
                if ($scope.obj.taxappcable == true) {
                    angular.forEach($scope.transrows, function (grn) {
                        grn.invtgrN_WithTaxFlg = true;
                    });
                }
                else {
                    angular.forEach($scope.transrows, function (grn) {
                        grn.invtgrN_WithTaxFlg = false;
                    });
                }
                
                
            }
        }


        //=======================GRN Grid Activate and Deactivate 
        $scope.deactive = function (item, SweetAlert) {
            $scope.INVMGRN_Id = item.invmgrN_Id;
            var dystring = "";
            if (item.invmgrN_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invmgrN_ActiveFlg === false) {
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
                        apiService.create("INV_T_GRN/deactive", item).
                            then(function (promise) {
                                if (item.invmgrN_ActiveFlg === true) {
                                    swal("Record " + dystring + " Successfully!!!");
                                }
                                else {
                                    swal("Record " + dystring + " Successfully!!!");
                                }


                                $state.reload();
                            });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        };
        $scope.deactiveG = function (item, SweetAlert) {
            $scope.INVTGRN_Id = item.invtgrN_Id;
            var dystring = "";
            if (item.invtgrN_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invtgrN_ActiveFlg === false) {
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
                        apiService.create("INV_T_GRN/deactiveg", item).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                angular.element('#myModalG').modal('hide');
                                // $state.reload();
                            });

                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        };
        $scope.deactivet = function (item, SweetAlert) {
            $scope.INVTGRNT_Id = item.invtgrnT_Id;
            var dystring = "";
            if (item.invtgrnT_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invtgrnT_ActiveFlg === false) {
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
                        apiService.create("INV_T_GRN/deactivet", item).
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
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        };

        //=======================GRN Grid Model Data
        $scope.transmodelclick = function (id) {
            var data = {
                "INVTGRN_Id": id
            };
            apiService.create("INV_T_GRN/get_itemtax", data).
                then(function (promise) {
                    $scope.get_GRNItemTax = promise.get_GRNItemTax;

                });
        };

        $scope.mainmodelclick = function (id) {
            var data = {
                "INVMGRN_Id": id
            };
            apiService.create("INV_T_GRN/get_GRNitemDetails", data).
                then(function (promise) {
                    $scope.get_GRNItemDetails = promise.get_GRNItemDetails;
                    $scope.grnnumber = $scope.get_GRNItemDetails[0].invmgrN_GRNNo;
                });
        };
        $scope.objgUU = [];

        $scope.edit = function (item) {
            $scope.transrows = [];

            var id = item.invmgrN_Id;
            var data = {
                "INVMGRN_Id": id
            };
            apiService.create("INV_T_GRN/Edit_GRN_details", data).
                then(function (promise) {
                    if (promise.edit_GRN_Master_Details.length > 0) {
                        $scope.editS = true;
                        $scope.edit1 = true;
                        $scope.invmgrN_PurchaseDate = promise.edit_GRN_Master_Details[0].invmgrN_PurchaseDate;
                        $scope.invmS_Id = promise.edit_GRN_Master_Details[0].invmS_Id;
                        $scope.invmS_SupplierName = promise.edit_GRN_Master_Details[0].invmS_SupplierName;
                        $scope.invmgrN_InvoiceNo = promise.edit_GRN_Master_Details[0].invmgrN_InvoiceNo;
                        $scope.invmsT_Id = promise.edit_GRN_Master_Details[0].invmsT_Id;
                        $scope.invmS_StoreName = promise.edit_GRN_Master_Details[0].invmS_StoreName;
                        $scope.invmgrN_TotalAmount = promise.edit_GRN_Master_Details[0].invmgrN_TotalAmount;
                        $scope.invmgrN_TotDiscount = promise.edit_GRN_Master_Details[0].invmgrN_TotDiscount;
                        $scope.invmgrN_TotTaxAmt = promise.edit_GRN_Master_Details[0].invtgrnT_TaxAmt;
                        $scope.invmgrNPurchaseValue = parseFloat(promise.edit_GRN_Master_Details[0].invmgrN_PurchaseValue);
                        $scope.invmgrN_Remarks = promise.edit_GRN_Master_Details[0].invmgrN_Remarks;
                        $scope.invmgrN_POReference = promise.edit_GRN_Master_Details[0].invmgrN_POReference;
                        $scope.invmgrN_EWayBillRef = promise.edit_GRN_Master_Details[0].invmgrN_EWayBillRef;
                        $scope.invmgrN_TowardsProjects = promise.edit_GRN_Master_Details[0].invmgrN_TowardsProjects;
                        $scope.invmgrN_Id = promise.edit_GRN_Master_Details[0].invmgrN_Id;
                        $scope.differentarray = [];
                        $scope.differentarray = promise.edit_GRN_Details_List;
                        $scope.saveditemtx = promise.savedget_itemTax;
                        $scope.ittax = promise.get_itemTax;
                        if ($scope.differentarray.length > 0) {
                            angular.forEach($scope.differentarray, function (tt) {

                                $scope.transrows.push({
                                    invtgrN_Id: tt.invtgrN_Id,
                                    invmI_Id: tt,
                                    invmG_Id: tt.invmG_Id,
                                    invmI_ItemName: tt.invmI_ItemName,
                                    invtgrN_BatchNo: tt.invtgrN_BatchNo,
                                    invtgrN_TaxAmt: tt.invtgrN_TaxAmt,
                                    invmuoM_Id: tt.invmuoM_Id,
                                    invtgrN_Qty: tt.invtgrN_Qty,
                                    invtgrN_PurchaseRate: tt.invtgrN_PurchaseRate,
                                    invtgrN_MRP: tt.invtgrN_MRP,
                                    invtgrN_DiscountAmt: tt.invtgrN_DiscountAmt,
                                    invtgrN_SalesPrice: tt.invtgrN_SalesPrice,
                                    invtgrN_Amount: tt.invtgrN_Amount,
                                    invtgrN_Naration: tt.invtgrN_Naration,
                                    itmid: tt.invmI_Id,
                                    invtgrN_AssetTypeFlag: tt.invtgrN_AssetTypeFlag,
                                    invtgrN_WithTaxFlg: tt.invtgrN_WithTaxFlg
                                });

                            })


                            angular.forEach($scope.transrows, function (xx) {
                                var cnt = 0;
                                var taxx = [];
                                var taxx1 = [];
                                var totper = 0;
                                angular.forEach($scope.saveditemtx, function (tax) {
                                    if (xx.invtgrN_Id == tax.invtgrN_Id && xx.itmid == tax.invmI_Id) {
                                        cnt += 1;
                                        taxx.push(tax);
                                        taxx1.push({
                                            invmT_Id: tax.invmT_Id,
                                            invtgrnT_TaxPer: tax.invmiT_TaxValue,
                                            invtgrnT_TaxAmt: tax.invtgrN_TaxAmt,
                                            invtgrnT_Id: tax.invtgrnT_Id
                                        });
                                        totper += Number(tax.invmiT_TaxValue);
                                    }

                                });
                                xx.txxxx = taxx1;
                                xx.taxpercent = totper > 0 ? totper.toFixed(2) : "";
                                xx.get_itemTax = taxx;
                                //$scope.arrayGRNtax1.push({ invmT_Id: tax.invmT_Id, invtgrnT_TaxPer: tax.invmiT_TaxValue, invtgrnT_TaxAmt: tax.invtgrN_TaxAmt, invtgrnT_Id: tax.invtgrnT_Id })
                                if (cnt == 0) {
                                    var taxx1 = [];
                                    angular.forEach($scope.ittax, function (ss) {
                                        if (xx.invmI_Id == ss.invmI_Id) {
                                            taxx1.push(ss);
                                        }
                                    })

                                    xx.get_itemTax = taxx1;
                                }

                            })

                        }
                        $scope.grnToatalcalculate();

                    }

                    if ($scope.get_taxlist != null && promise.get_itemTaxtemp != null && $scope.get_taxlist.length > 0 && promise.get_itemTaxtemp.length > 0) {
                        angular.forEach($scope.get_taxlist, function (KK) {
                            angular.forEach(promise.get_itemTaxtemp, function (PP) {
                                if (KK.invmT_Id == PP.invmT_Id) {
                                    KK.invmiT_TaxValue = PP.invmiT_TaxValue;
                                    KK.xyz = true;
                                }
                            })
                        })
                    }

                    if (promise.grnFiles != null && promise.grnFiles.length > 0) {
                        $scope.materaldocuupload = promise.grnFiles;
                        angular.forEach($scope.materaldocuupload, function (dd) {
                            dd.vbabrsF_Filedesc = dd.invmgrnF_Filedesc;
                            dd.vbabrsF_FilePath = dd.invmgrnF_FilePath;
                            dd.vbabrsF_FileName = dd.invmgrnF_FileName;                         
                        });
                    }


                });
        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
        $scope.selectFileforUploadzd = function (input, document) {
            $scope.SelectedFileForUploadzd = input.files;
            $scope.vbabrsF_FileName = input.files[0].name;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 5242880)  // 2097152 bytes = 2MB 
                {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/png" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/jpg" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/JPG" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/pdf" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/msword" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                //5242880
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].size > 5242880) {
                    swal("File size should be less than 5 MB");
                    return;
                }
            }
        };
        function UploaddianmateralPhoto(data) {
            console.log("check Book upload  Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= 1; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
            }

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadtrnsportdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.vbabrsF_FilePath = d;
                    data.vbabrsF_FileName = $scope.vbabrsF_FileName;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        };

        $scope.previewimg_new = function (img) {
            $scope.vbabrsF_FilePath = img;
            var img = $scope.vbabrsF_FilePath;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.vbabrsF_FilePath);
                    $('#myimagePreview').modal('show');
                    //$('#myModalfile').modal('show');
                }
                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                    $window.open($scope.vbabrsF_FilePath);
                }
                else if (lastelement == 'pdf') {
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.vbabrsF_FilePath;
                    $http.get(imagedownload1, { responseType: 'arraybuffer' })
                        .success(function (response) {
                            var fileURL = "";
                            var file = "";
                            var embed = "";
                            var pdfId = "";
                            file = new Blob([(response)], { type: 'application/pdf' });
                            fileURL = URL.createObjectURL(file);
                            pdfId = document.getElementById("pdfIdzz");
                            pdfId.removeChild(pdfId.childNodes[0]);
                            embed = document.createElement('embed');
                            embed.setAttribute('src', fileURL);
                            embed.setAttribute('type', 'application/pdf');
                            embed.setAttribute('width', '100%');
                            embed.setAttribute('height', '1000');
                            pdfId.appendChild(embed);
                            $('#showpdf').modal('show');
                        });
                }
            }
            else {
                $window.open($scope.vbabrsF_FilePath)
            }
        };
    }
})();