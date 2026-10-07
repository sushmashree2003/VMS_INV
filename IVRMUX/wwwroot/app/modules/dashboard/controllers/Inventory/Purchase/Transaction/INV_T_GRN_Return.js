
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_T_GRN_ReturnController', INV_T_GRN_ReturnController);
    INV_T_GRN_ReturnController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function INV_T_GRN_ReturnController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.objg = {};
        var date = new Date();
        $scope.discounttype = "A";
        $scope.invmgrnreT_ReturnDate = date;
        //$scope.invmgrN_PurchaseDate = date;
        $scope.invmgrnreT_DebitNoteDate = date;
        $scope.invmgrnreT_DCDate = date;
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

        //====================== Auto Generated Number
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        
        if (transnumconfigsettings!= null && transnumconfigsettings.length > 0) {
            for (var i = 0; i < transnumconfigsettings.length; i++) {
                if (transnumconfigsettings[i].imN_Flag === "INVGRNR") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
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


        //=====================Adding and removing new row in transcation Grid============      
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        if ($scope.transrows.length === 1) {
            $scope.cnt = 1;
        }
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
        $scope.removegrnrows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);

            if (data.amstB_Id > 0) {
                $scope.Deletegrnrows(data);
            }

        };

        //==============================Page Load========================

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.searchvalue = "";
            var pageid = 2;
            apiService.getURI("INV_T_GRN_Return/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_GRN = promise.get_GRN;
                    $scope.get_GRNReturnList = promise.get_GRNReturnList;
                    $scope.presentCountgrid = $scope.get_GRNReturnList.length;
                });
        };
        //===========Item Change Transcation Grid
        $scope.getitem1 = function (ite) {
            $scope.get_item = [];
            $scope.get_itemTax1 = [];
            $scope.item_id = ite.invmgrN_Id;
            var data = {
                "INVMGRN_Id": ite.invmgrN_Id
            };
            apiService.create("INV_T_GRN_Return/getitem", data).
                then(function (promise) {
                    $scope.get_item = promise.get_item;
                    $scope.INVMS_SupplierName = promise.getsupplier[0].invmS_SupplierName;
                    $scope.INVMS_Id = promise.getsupplier[0].invmS_Id;
                    $scope.INVMGRN_PurchaseDate = new Date(promise.getsupplier[0].invmgrN_PurchaseDate);
                    //$scope.INVMGRN_PurchaseDate = promise.getsupplier[0].invmgrN_PurchaseDate;

                });
        };

        $scope.onitemchange = function (itemid, objid) {
            $scope.get_itemTax1 = [];
            // var item_id = itemid.invmI_Id;
            var data = {
                "INVMI_Id": itemid.invmI_Id,
                "INVMGRN_Id": $scope.item_id
            };
            apiService.create("INV_T_GRN_Return/getitemDetail", data).
                then(function (promise) {
                    $scope.get_itemDetail = promise.get_itemDetail;
                    $scope.get_itemTax1 = promise.get_itemTax;

                    angular.forEach($scope.transrows, function (obj) {
                        if (obj.itrS_Id === objid.itrS_Id) {
                            obj.invmuoM_UOMName = $scope.get_itemDetail[0].invmuoM_UOMName;
                            obj.invmuoM_Id = $scope.get_itemDetail[0].invmuoM_Id;
                            obj.get_itemTax = $scope.get_itemTax1;
                            $scope.get_itemTax = $scope.get_itemTax1;
                            obj.invtgrN_DiscountAmt = 0.00;
                            obj.invtgrN_PurchaseRate = 0.00;
                            obj.invtgrN_PurchaseRatenew = $scope.get_itemDetail[0].invtgrN_PurchaseRate;
                            obj.invtgrN_Qty3 = $scope.get_itemDetail[0].invtgrN_Qty;
                            obj.invtgrN_Qty1 = $scope.get_itemDetail[0].INVTGRN_Qty1;
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
                    if ($scope.invmgrN_PurchaseValue === 0 || $scope.invmgrN_PurchaseValue === "" || $scope.invmgrN_PurchaseValue === undefined || $scope.invmgrN_PurchaseValue === "NaN") {
                        $scope.invmgrN_PurchaseValue = 0.00;
                    }

                });
        };
        // View Tax Model
        $('#myModal').modal('hide');
        $scope.viewtxfn = function (qwe) {
            $scope.purrate = parseFloat(qwe.invtgrN_PurchaseRate);
            $scope.qty = parseFloat(qwe.invtgrN_Qty);
            // $scope.totamtsl = qwe.invtgrN_Amount;
            $scope.totdiscout = parseFloat(qwe.invtgrN_DiscountAmt);
            $('#myModal').modal('show');
            $scope.get_itemTax = qwe.get_itemTax;
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
                //wer.invtgrN_TaxAmt = ($scope.finalpurrate / 100) * wer.invmiT_TaxValue;
                //cnt = cnt + wer.invtgrN_TaxAmt;
            });

            //  $scope.get_itemTax = qwe.get_itemTax;
        };
        //==========================Tax Add To Cart
        $scope.arrayGRNtax = [];
        $scope.addtocart = function (itemtax) {
            $scope.arraytax = [];
            var ccnt = 0;
            angular.forEach(itemtax, function (tax) {
                $scope.tsid = tax.itrS_Id;
                $scope.arrayGRNtax.push({ invmT_Id: tax.invmT_Id, invtgrnT_TaxPer: tax.invmiT_TaxValue, invtgrnT_TaxAmt: tax.invtgrN_TaxAmt, invtgrnT_Id: tax.invtgrnT_Id, INVMI_Id: tax.invmI_Id });
                ccnt = ccnt + tax.invtgrN_TaxAmt;

            });

            angular.forEach($scope.transrows, function (obj) {
                var tx = 0.00;
                angular.forEach(obj.get_itemTax, function (obj1) {
                    if (obj.itrS_Id === obj1.itrS_Id) {
                        tx += parseFloat(obj1.invtgrN_TaxAmt);
                    }
                });
                obj.invtgrN_TaxAmt = tx;
            });

            var t = 0;
            angular.forEach($scope.transrows, function (obj) {
                angular.forEach(obj.get_itemTax, function (obj1) {
                    t += parseFloat(obj1.invtgrN_TaxAmt);
                });
            });
            $scope.invmgrN_TotTaxAmt = t.toFixed(2);

            angular.forEach($scope.transrows, function (qwetax) {
                if (qwetax.itrS_Id === $scope.tsid) {
                    $scope.arraytax.push({ invmT_Id: qwetax.invmT_Id, invtgrnT_TaxPer: qwetax.invmiT_TaxValue, invtgrnT_TaxAmt: qwetax.invtgrN_TaxAmt, invtgrnT_Id: qwetax.invtgrnT_Id });
                    qwetax.get_itemTax = itemtax;

                }
            });
            $scope.taxchange($scope.invmgrN_TotTaxAmt);
        };

        $scope.taxchange = function (grnobj) {
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
        };

        $scope.countitemAmt = function (grnobj, items) {
            if (grnobj.invtgrN_Qty3 == 0) {
                swal('Remain Quantity is Zero..');
                angular.forEach($scope.transrows, function (obj) {
                    if (obj.itrS_Id === grnobj.itrS_Id) {
                        obj.invmuoM_Id = $scope.get_itemDetail[0].invmuoM_Id;
                        obj.invmuoM_UOMName = $scope.get_itemDetail[0].invmuoM_UOMName;
                        obj.get_itemTax = $scope.get_itemTax1;
                        $scope.get_itemTax = $scope.get_itemTax1;
                        obj.invtgrN_DiscountAmt = 0.00;
                        obj.invtgrN_PurchaseRate = 0.00;
                        obj.invtgrN_PurchaseRatenew = $scope.get_itemDetail[0].invtgrN_PurchaseRate;
                        obj.invtgrN_Qty3 = $scope.get_itemDetail[0].invtgrN_Qty;
                        obj.invtgrN_Qty1 = $scope.get_itemDetail[0].INVTGRN_Qty1;
                        obj.invtgrN_Qty = 0.00;
                        obj.invtgrN_Amount = 0.00;
                        obj.invtgrN_TaxAmt = 0.00;
                        obj.discountper = 0.00;
                    }
                });
                //$scope.invmgrN_TotalAmount = 0.00;
                //$scope.invmgrN_TotTaxAmt = 0.00;
                //$scope.invmgrN_PurchaseValue = 0.00;
            }
            else if (grnobj.invtgrN_Qty > grnobj.invtgrN_Qty3) {
                swal('Remain Quantity Less Than Return Quantity..');
                angular.forEach($scope.transrows, function (obj) {
                    if (obj.itrS_Id === grnobj.itrS_Id) {
                        obj.invmuoM_UOMName = $scope.get_itemDetail[0].invmuoM_UOMName;
                        obj.invmuoM_Id = $scope.get_itemDetail[0].invmuoM_Id;
                        obj.get_itemTax = $scope.get_itemTax1;
                        $scope.get_itemTax = $scope.get_itemTax1;
                        obj.invtgrN_DiscountAmt = 0.00;
                        obj.invtgrN_PurchaseRate = 0.00;
                        obj.invtgrN_PurchaseRatenew = $scope.get_itemDetail[0].invtgrN_PurchaseRate;
                        obj.invtgrN_Qty3 = $scope.get_itemDetail[0].invtgrN_Qty;
                        obj.invtgrN_Qty1 = $scope.get_itemDetail[0].INVTGRN_Qty1;
                        obj.invtgrN_Qty = 0.00;
                        obj.invtgrN_Amount = 0.00;
                        obj.invtgrN_TaxAmt = 0.00;
                        obj.discountper = 0.00;
                    }
                });

            }
            else {
                var a = 0;
                var ab = 0;
                var ds = 0;
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
            }

        };

        $scope.$watch('invmgrN_TotalAmount', function (totalamoutchange) {
            $scope.invmgrN_PurchaseValue = $scope.invmgrN_TotalAmount;
            $scope.invmgrN_PurchaseValue = parseFloat($scope.invmgrN_PurchaseValue);
            $scope.invmgrN_PurchaseValue = $scope.invmgrN_PurchaseValue.toFixed(2);

            if ($scope.invmgrN_TotTaxAmt != null && $scope.invmgrN_TotTaxAmt != '' && $scope.invmgrN_TotTaxAmt != undefined) {
                var tax = $scope.invmgrN_TotTaxAmt;

                $scope.invmgrN_PurchaseValue = parseFloat($scope.invmgrN_PurchaseValue) + parseFloat(tax);
                $scope.invmgrN_PurchaseValue = $scope.invmgrN_PurchaseValue.toFixed(2);
            }

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
                $scope.arrayGRN1 = [];
                $scope.arrayGRN = [];
                $scope.grntemTax = [];

                angular.forEach($scope.transrows, function (grn) {
                    $scope.arrayGRN.push({
                        invmI_Id: grn.invmI_Id.invmI_Id, invmuoM_Id: grn.invmuoM_Id, invtgrN_BatchNo: grn.invtgrN_BatchNo, INVTGRNRET_ReturnQty: grn.invtgrN_Qty,
                        invtgrN_MfgDate: grn.invtgrN_MfgDate, invtgrN_ExpDate: grn.invtgrN_ExpDate, INVTGRNRET_ReturnAmount: grn.invtgrN_PurchaseRate, invtgrN_MRP: grn.invtgrN_MRP,
                        invtgrN_DiscountAmt: grn.invtgrN_DiscountAmt, invtgrN_SalesPrice: grn.invtgrN_SalesPrice, invtgrN_Amount: grn.invtgrN_Amount, INVTGRN_Naration: grn.invtgrN_Naration,
                        invtgrN_Id: grn.invtgrN_Id, invtgrN_TaxAmt: grn.invtgrN_TaxAmt, GRNItemTax: $scope.arrayGRNtax
                    });
                });

                angular.forEach($scope.arrayGRN1, function (qq) {
                    if (qq.INVTGRNRET_ReturnQty != 0) {
                        $scope.arrayGRN.push(qq);
                    }
                });
                var data = {

                    "INVMGRNRET_ReturnDate": new Date($scope.invmgrnreT_ReturnDate).toDateString(),
                    "INVMGRNRET_DebitNoteNo": $scope.invmgrnreT_DebitNoteNo,
                    "INVMGRNRET_DebitNoteDate": new Date($scope.invmgrnreT_DebitNoteDate).toDateString(),
                    "INVMGRNRET_EWayBillNo": $scope.invmgrnreT_EWayBillNo,
                    "INVMGRNRET_DCNo": $scope.invmgrnreT_DCNo,
                    "INVMGRNRET_DCDate": new Date($scope.invmgrnreT_DCDate).toDateString(),
                   // "INVMGRN_InvoiceNo": $scope.invmgrN_InvoiceNo,
                    "INVMGRN_PurchaseDate": $scope.invmgrN_PurchaseDate,
                    "INVMGRN_PurchaseValue": $scope.invmgrN_PurchaseValue,
                    "INVMGRN_TotDiscount": $scope.invmgrN_TotDiscount,
                    "INVMGRN_TotTaxAmt": $scope.invmgrN_TotTaxAmt,
                    "INVMGRN_TotalAmount": $scope.invmgrN_TotalAmount,
                    "INVMGRNRET_Remarks": $scope.INVMGRNRET_Remarks,
                    "INVMGRN_ReturnFlg": $scope.invmgrN_ReturnFlg,
                    "INVMGRN_PaidFlg": $scope.invmgrN_PaidFlg,
                    "INVMGRN_CreditFlg": $scope.invmgrN_CreditFlg,
                    "INVMGRN_Id": $scope.item_id,
                    //"INVMGRN_Id": $scope.invmgrnreT_Id,
                    "INVMS_Id": $scope.INVMS_Id,
                    "GRNItem": $scope.arrayGRN,
                    transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign

                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("INV_T_GRN_Return/savedata", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.invmgrN_Id === 0 || promise.invmgrN_Id < 0) {
                            swal('Record saved successfully');
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
                "INVMGRNRET_Id": id.invmgrnreT_Id
            };
            apiService.create("INV_T_GRN_Return/get_GRNitemDetails", data).
                then(function (promise) {
                    $scope.get_GRNitemdetails = promise.get_GRNitemdetails;
                    //$scope.grnnumber = $scope.get_GRNItemDetails[0].invmgrN_GRNNo;
                });
        };
        $scope.deactive = function (item) {

            var dystring = "Delete";
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
                        apiService.create("INV_T_GRN_Return/deactive", item).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    if (promise.returnduplicatestatus === 'Updated') {
                                        swal("Record " + dystring + "d Successfully / Stock Updated");
                                    }
                                    else if (promise.returnduplicatestatus === 'notUpdated') {
                                        swal("Record " + dystring + "d Successfully / Failed to Update Stock");
                                    }
                                    else {
                                        swal("Record " + dystring + "d Successfully!!!");
                                    }
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

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
    }
})();