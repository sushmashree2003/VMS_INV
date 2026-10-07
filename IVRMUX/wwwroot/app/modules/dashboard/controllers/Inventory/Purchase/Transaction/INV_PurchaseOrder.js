
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_PurchaseOrderController', INV_PurchaseOrderController);
    INV_PurchaseOrderController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$compile'];
    function INV_PurchaseOrderController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $compile) {

        var date = new Date();
        $scope.invmpO_PODate = date;
        $scope.qtype = 0;
        $scope.obj = {};
        var paginationformasters;
        $scope.supplieraddress = "";
        $scope.suppliername = "";
        $scope.suppliercontactno = "";
        $scope.emailid = "";
        $scope.miaddress = "";
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.asmaY_Id = 0;
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
            $scope.imagename = logopath;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        //===================================== VIEW TEMPLATE
        $scope.columdetails = [];
        var count = 0;

        $scope.addgrnfile = function () {
            var newItemNo = $scope.materaldocuupload.length + 1;
            if (newItemNo <= 50) {
                $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
            }
        };
        $scope.materaldocuupload = [];
        //$scope.materaldocuupload = [{ id: 'materal' }];

        $scope.materaldocuupload.push({
            ccmail: "acts2@vapstech.com"
        })
        $scope.materaldocuupload.push({
            ccmail: "acts@vapstech.com"
        })

        $scope.removegrnfiles = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);
        };
        $scope.viewtemplate = function () {

            $scope.columdetails = [];
            angular.forEach($scope.columnlist, function (dd) {

                if (dd.checkedvalue === true) {
                    $scope.columdetails.push(dd);
                }

            });
            $scope.arrayPOtax = [];
            $scope.arrayPO = [];

            angular.forEach($scope.potax_list, function (tx) {
                $scope.arrayPOtax.push({
                    invmiT_Id: tx.invmiT_Id, invtpoT_TaxPercent: tx.invmiT_TaxValue, invtpoT_TaxAmount: tx.inv_TaxAmount
                });
            })

            if ($scope.selectiontype === 'P') {
                angular.forEach($scope.pidetails, function (polist) {
                    $scope.arrayPO.push({
                        invmpI_Id: polist.invmpI_Id, invtpI_Id: polist.invtpI_Id, invmI_Id: polist.invmI_Id, invmuoM_Id: polist.invmuoM_Id, invmI_ItemName: polist.invmI_ItemName,
                        invmuoM_UOMName: polist.invmuoM_UOMName, invtpO_POQty: polist.invtpO_POQty, invtpO_RatePerUnit: polist.invtpI_PIUnitRate,
                        invtpO_TaxAmount: polist.invtpO_TaxAmount, invtpO_Amount: polist.invtpO_Amount, invtpO_Remarks: polist.invtpO_Remarks, arrayPOtax: $scope.arrayPOtax
                    });
                });
            }
            else {
                angular.forEach($scope.get_qtdetails, function (polist) {
                    $scope.arrayPO.push({
                        invmpI_Id: polist.invmpI_Id, invtpI_Id: polist.invtpI_Id, invmI_Id: polist.invmI_Id, invmuoM_Id: polist.invmuoM_Id, invmI_ItemName: polist.invmI_ItemName,
                        invmuoM_UOMName: polist.invmuoM_UOMName, invtpO_POQty: polist.invtpO_POQty, invtpO_RatePerUnit: polist.invtsQ_NegotiatedRate,
                        invtpO_TaxAmount: polist.invtpO_TaxAmount, invtpO_Amount: polist.invtpO_Amount, invtpO_Remarks: polist.invtpO_Remarks, arrayPOtax: $scope.arrayPOtax
                    });
                });
            }

            var data = {
                "ISES_Id": $scope.ISES_Id,
                "INVMS_Id": $scope.obj.invmS_Id.invmS_Id,
                "INVMPO_PODate": new Date($scope.invmpO_PODate),
                "INVMPO_Remarks": $scope.invmpO_Remarks,
                "INVMPO_ReferenceNo": $scope.invmpO_ReferenceNo,
                "INVMPO_TotRate": $scope.invmpO_TotRate,
                "INVMPO_TotTax": $scope.invmpO_TotTax,
                "INVMPO_TotAmount": $scope.invmpO_TotAmount,
                "arrayPO": $scope.arrayPO,
                "INVMPO_Id": $scope.invmpO_Id,
                "columnids": $scope.columnlist
            }
            apiService.create("INV_PurchaseOrder/viewtemplatedetails", data).
                then(function (promise) {

                    if (promise.mailmsg !== null) {
                        $scope.htmldata = promise.mailmsg;

                        var e1 = angular.element(document.getElementById("test"));
                        $compile(e1.html($scope.htmldata))(($scope));
                        $("#templemodel").modal('show');
                    }

                });

        }
        //added by adarsh to fetch the template from the data-base
        $scope.bindtemplate = [];
        $scope.loadtemplate = function() {
            var items = {
                "pending": $scope.Template,
                "message": "FetchTemp"
            }
            apiService.create("INV_PurchaseOrder/viewtemplatedetails", items).then(function (response) {
                var e1 = angular.element(document.getElementById("bindtemplate"));
                if (response.templatelist != null && response.templatelist.length > 0) {
                    $scope.bindtemplate = response.templatelist;
                    $scope.xtra = $scope.bindtemplate[0].VMSMT_Template;
                    //var e1 = angular.element(document.getElementById("bindtemplate"));
                    e1.html($scope.xtra);
                    $compile(e1.contents())($scope);
                }
                else {
                    e1.html(''); //make null if the template was not found 
                    swal("No Template Found");
                }
            })
        }
        //Added by Praveen gouda
        $scope.Viewtemplateone = function() {
            if ($scope.Template != null && $scope.Template != "") {
                $scope.invmpO_TotAmount = 0
                angular.forEach($scope.pidetails, function (obj) {
                    obj.totalamount = parseFloat(obj.invtpO_Amount) + parseFloat(obj.invtpO_TaxAmount);
                    $scope.invmpO_TotAmount += obj.totalamount
                })
                $scope.invmS_GSTNo = "";
                $scope.supplieraddress = "";
                $scope.suppliername = "";
                $scope.suppliercontactno = "";
                $scope.emailid = "";
                $scope.ConatctPerson = "";
                $scope.INVMS_BankAccountNo = ""; $scope.INVMS_AccountName = "";
                $scope.INVMS_BankName = ""; $scope.INVMS_IFSCCode = ""; $scope.INVMS_BranchName = "";
                $scope.INVMS_BankAddress = ""; $scope.invmS_GSTNo = "";
                $("#templatemodel").modal('show');
                angular.forEach($scope.get_supplier, function (dd) {
                    if (dd.invmS_Id == $scope.obj.invmS_Id.invmS_Id) {
                        $scope.supplieraddress = dd.invmS_SupplierAddress;
                        $scope.suppliername = dd.invmS_SupplierName;
                        $scope.suppliercontactno = dd.invmS_SupplierConatctNo;
                        $scope.emailid = dd.invmS_EmailId;
                        $scope.ConatctPerson = dd.invmS_SupplierConatctPerson;
                        $scope.invmS_GSTNo = dd.invmS_GSTNo;
                        $scope.INVMS_BankAccountNo = dd.invmS_BankAccountNo;
                        $scope.INVMS_AccountName = dd.invmS_AccountName;
                        $scope.INVMS_BankName = dd.invmS_BankName;
                        $scope.INVMS_IFSCCode = dd.invmS_IFSCCode;
                        $scope.INVMS_BranchName = dd.invmS_BranchName;
                        $scope.INVMS_BankAddress = dd.invmS_BankAddress;
                        $scope.INVMS_AccountType = dd.invmS_AccountType;
                        $scope.invmS_GSTNo = dd.invmS_GSTNo;
                        return;
                    }
                });

            }
            else {
                swal("Pls Select  Template name !");
            }

        }

        //Added by praveen gouda
        $scope.printDatas = function () {
            var innerContents = "";
            innerContents = document.getElementById("temp").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<style type="text/css">' +
                '@page :first {' +
                'margin-top: 150px;' +
                'margin-bottom: 150px;' +
                '}' +
                '</style>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        //Ended-------------------

        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("test").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.printoreder = function () {
            var innerContents = "";
            innerContents = document.getElementById("ordertemplate").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<style type="text/css">' +
                '@page :first {' +
                'margin-top: 150px;' +
                'margin-bottom: 150px;' +
                '}' +
                '</style>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.viewsavedteple = function (objj) {


            var data = {
                "INVMS_Id": objj.invmS_Id,
                "INVMPO_Id": objj.invmpO_Id,
                "columnids": $scope.columnlist,
                "INVMPO_POTemplate": objj.invmpO_POTemplate,
            }
            apiService.create("INV_PurchaseOrder/viewsavedteple", data).
                then(function (promise) {

                    if (promise.mailmsg !== null) {
                        $scope.htmldata = promise.mailmsg;

                        var e1 = angular.element(document.getElementById("test"));
                        $compile(e1.html($scope.htmldata))(($scope));
                        $("#templemodel").modal('show');
                    }





                });

        }
        //===================================== VIEW TEMPLATE
        //====================== Auto Generated Number
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag === "INVPO") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }
        //===========================


        $scope.columnlist = [];

        $scope.columnlist.push({ ID: 1, coulmn: 'ITEMNAME', title: 'ITEM NAME', checkedvalue: true });
        //$scope.columnlist.push({ ID: 7, coulmn: 'REMARKS', title: 'REMARKS', checkedvalue: true });
        $scope.columnlist.push({ ID: 2, coulmn: 'QUANTITY', title: 'QUANTITY', checkedvalue: true });
        $scope.columnlist.push({ ID: 3, coulmn: 'PRICE/UNIT', title: 'PRICE/UNIT', checkedvalue: true });
        $scope.columnlist.push({ ID: 4, coulmn: 'AMOUNT', title: 'AMOUNT', checkedvalue: true });
        $scope.columnlist.push({ ID: 5, coulmn: 'TAX', title: 'TAX', checkedvalue: true });
        $scope.columnlist.push({ ID: 6, coulmn: 'TOTALAMOUNT', title: 'TOTAL AMOUNT', checkedvalue: true });
        $scope.MI_Pincode = "";
        $scope.mI_name = "";
        //====================================== Page Load
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("INV_PurchaseOrder/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_quotationno = promise.get_quotationno;
                    $scope.get_supplier = promise.get_supplier;
                    $scope.get_PI = promise.get_PI;
                    $scope.miaddress = promise.get_mi_list[0].mI_Address1;
                    $scope.mI_name = promise.get_mi_list[0].mI_Name;
                    $scope.MI_Pincode = promise.get_mi_list[0].mI_Pincode;
                    $scope.imagenamenew = promise.get_mi_list[0].mI_Logo;
                    $scope.MI_Pincode = promise.get_mi_list[0].mI_Pincode;
                    $scope.get_comparequotationno = promise.get_comparequotationno;
                    $scope.get_purchaseorder = [];
                    if (promise.get_purchaseorder != null && promise.get_purchaseorder.length > 0) {                       
                        angular.forEach(promise.get_purchaseorder, function (dddd) {
                            if (dddd.invmpO_Status == "PO") {
                                $scope.get_purchaseorder.push(dddd)
                            }

                        });
                    }
                    
                    $scope.templatelist = promise.templatelist;
                    $scope.Template = "";
                    if ($scope.get_purchaseorder != null && $scope.get_purchaseorder.length > 0) {
                        angular.forEach($scope.get_purchaseorder, function (dd) {
                            angular.forEach($scope.get_supplier, function (dddd) {
                                if (dd.invmS_Id == dddd.invmS_Id) {
                                    dd.invmS_SupplierName = dddd.invmS_SupplierName;
                                }

                            });
                        });
                    }
                    //  $scope.Template = "TempTwo";
                });
        };

        //Added by Praveen Gouda
        $scope.supplieraddress = "";
        $scope.suppliername = "";
        $scope.suppliercontactno = "";
        $scope.emailid = "";
        $scope.ConatctPerson = "";
        $scope.addressed = function () {
            $scope.supplieraddress = "";
            $scope.suppliername = "";
            $scope.suppliercontactno = "";
            $scope.emailid = "";
            $scope.ConatctPerson = "";
            $scope.INVMS_BankAccountNo = ""; $scope.INVMS_AccountName = "";
            $scope.INVMS_BankName = ""; $scope.INVMS_IFSCCode = ""; $scope.INVMS_BranchName = "";
            $scope.INVMS_BankAddress = ""; $scope.invmS_GSTNo ="";
            angular.forEach($scope.get_supplier, function (dd) {
                if (dd.invmS_Id == $scope.obj.invmS_Id.invmS_Id) {
                    $scope.supplieraddress = dd.invmS_SupplierAddress;
                    $scope.suppliername = dd.invmS_SupplierName;
                    $scope.suppliercontactno = dd.invmS_SupplierConatctNo;
                    $scope.emailid = dd.invmS_EmailId;
                    $scope.ConatctPerson = dd.invmS_SupplierConatctPerson;
                    $scope.INVMS_BankAccountNo = dd.invmS_BankAccountNo;
                    $scope.INVMS_AccountName = dd.invmS_AccountName;
                    $scope.INVMS_BankName = dd.invmS_BankName;
                    $scope.INVMS_IFSCCode = dd.invmS_IFSCCode;
                    $scope.INVMS_BranchName = dd.invmS_BranchName;
                    $scope.INVMS_BankAddress = dd.invmS_BankAddress;
                    $scope.INVMS_AccountType = dd.invmS_AccountType;
                    $scope.invmS_GSTNo = dd.invmS_GSTNo;
                    return;
                }
            });


        }
        //Ended-------------------

        //===================================== Radio Change
        $scope.radiochange = function () {
            $scope.get_qtdetails = "";
            $scope.loaddata();
        };
        $scope.radioSelection = function () {
            $scope.get_qtdetails = [];
            $scope.pidetails = [];
        }

        //viewtemplatedetails

        //===================================== Qt Change
        $scope.onQtselect = function (qoute) {
            $scope.get_qtdetails = "";
            $scope.pidetails = "";
            var data = {};
            $scope.invmpO_ReferenceNo = "";

            if ($scope.selectiontype === 'P') {
                data = {
                    "INVMPI_Id": qoute.invmpI_Id,
                    "Selectionflag": $scope.selectiontype,
                    "quotationflag": $scope.qtype,
                    transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                    //"ASMAY_Id": $scope.asmaY_Id
                };
            }
            else {
                data = {
                    "INVMSQ_Id": qoute.invmsQ_Id,
                    "Selectionflag": $scope.selectiontype,
                    "quotationflag": $scope.qtype,
                    transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                    //  "ASMAY_Id": $scope.asmaY_Id
                };
            }


            apiService.create("INV_PurchaseOrder/getqtDetail", data).
                then(function (promise) {

                    if ($scope.selectiontype === 'P') {
                        $scope.pidetails = promise.pidetails;
                        $scope.countrate($scope.pidetails);


                        angular.forEach($scope.pidetails, function (obj) {
                            $scope.rate = parseFloat(obj.invtpI_PIUnitRate);
                            $scope.qty = parseFloat(obj.invtpO_POQty);
                            obj.invtpO_Amount = $scope.rate * $scope.qty;
                            obj.flag = "A";
                        })


                        $scope.countPOAmt($scope.pidetails);
                    }
                    else {
                        $scope.get_qtdetails = promise.get_qtdetails;
                        $scope.countrate($scope.get_qtdetails);
                        $scope.countPOAmt($scope.get_qtdetails);
                    }
                    //
                    $scope.invmpO_ReferenceNo = promise.trans_id;
                });
        };

        //==========================Tax View and Add To Cart
        $('#myModal').modal('hide');
        $scope.viewtx = function (objc) {
            $scope.get_itemTax = "";
            var item_id = 0;
            var qt_id = 0;
            var qtrate = 0.00;
            var pi_id = 0;
            var pirate = 0.00;
            var data = {};
            if ($scope.selectiontype === 'P') {
                item_id = objc.invmI_Id;
                pi_id = objc.invtpI_Id;
                pirate = objc.invtpI_PIUnitRate;
                data = {
                    "INVMI_Id": item_id,
                    "INVTPI_Id": pi_id,
                    "Selectionflag": $scope.selectiontype,
                    "INVTPI_PIUnitRate": pirate
                };
            }
            else {
                item_id = objc.invmI_Id;
                qt_id = objc.invtsQ_Id;
                qtrate = objc.invtsQ_NegotiatedRate;

                data = {
                    "INVMI_Id": item_id,
                    "INVTSQ_Id": qt_id,
                    "Selectionflag": $scope.selectiontype,
                    "INVTSQ_NegotiatedRate": qtrate
                };
            }

            apiService.create("INV_PurchaseOrder/getitemtax", data).
                then(function (promise) {
                    $scope.get_itemTax = promise.get_itemTax;
                    if ($scope.selectiontype === 'P') {
                        $scope.rate = parseFloat(objc.invtpI_PIUnitRate);
                    }
                    else {
                        $scope.rate = parseFloat(objc.invtsQ_NegotiatedRate);
                    }
                    $scope.qty = parseFloat(objc.invtpO_POQty);
                    $('#myModal').modal('show');
                    var cnt = 0;
                    $scope.cntax = 0;
                    angular.forEach($scope.get_itemTax, function (wer) {
                        $scope.totalpo = $scope.rate * $scope.qty;
                        $scope.finalpo = $scope.totalpo;
                        var taxamt = ($scope.finalpo / 100) * wer.invmiT_TaxValue;
                        //var taxamt = ($scope.rate * wer.invmiT_TaxValue) / 100;
                        wer.inv_TaxAmount = taxamt;
                        wer.inv_TaxAmount = parseFloat(wer.inv_TaxAmount);
                        wer.inv_TaxAmount = wer.inv_TaxAmount.toFixed(2);
                        cnt = parseFloat(cnt) + parseFloat(wer.inv_TaxAmount);
                        $scope.cntax = cnt;
                    });
                });
        };
        //=========== ADD TO Cart
        $scope.potax_list = [];
        $scope.addtocart = function (objtx) {
            if ($scope.selectiontype === 'P') {
                var invmiT_TaxValue = 0;
                angular.forEach(objtx, function (wr1) {
                    invmiT_TaxValue = (Number(invmiT_TaxValue) + (Number(wr1.invmiT_TaxValue)));
                })
                angular.forEach(objtx, function (wr1) {
                    angular.forEach($scope.pidetails, function (wr) {
                        if (wr1.invmI_Id == wr.invmI_Id && wr1.invtpI_PIUnitRate == wr.invtpI_PIUnitRate) {
                            wr.invtpO_TaxAmount = $scope.cntax;
                            wr.invtpO_TaxAmount = parseFloat(wr.invtpO_TaxAmount);
                            wr.invtpO_TaxAmount = wr.invtpO_TaxAmount.toFixed(2);
                            wr.invmiT_TaxValue = invmiT_TaxValue
                            $scope.potax_list.push(wr1);
                        }
                    })
                    //invmiT_TaxValue
                })
            }
            else {
                angular.forEach(objtx, function (wr1) {
                    angular.forEach($scope.get_qtdetails, function (wr) {
                        if (wr1.invmI_Id == wr.invmI_Id && wr1.invtsQ_NegotiatedRate == wr.invtsQ_NegotiatedRate) {
                            wr.invtpO_TaxAmount = $scope.cntax;
                            wr.invtpO_TaxAmount = parseFloat(wr.invtpO_TaxAmount);
                            wr.invtpO_TaxAmount = wr.invtpO_TaxAmount.toFixed(2);
                            $scope.potax_list.push(wr1);
                        }
                    })
                })
            }
            $scope.taxchange();

            angular.forEach($scope.pidetails, function (obj) {
                obj.totalamount = parseFloat(obj.invtpO_Amount) + parseFloat(obj.invtpO_TaxAmount);

            })

        }
        $scope.taxchange = function (sobj) {
            var a = 0;
            if ($scope.selectiontype === 'P') {
                angular.forEach($scope.pidetails, function (obj) {
                    a += parseFloat(obj.invtpO_TaxAmount);
                })
            }
            else {
                angular.forEach($scope.get_qtdetails, function (obj) {
                    a += parseFloat(obj.invtpO_TaxAmount);
                })
            }

            $scope.invmpO_TotTax = a;
            $scope.invmpO_TotTax = parseFloat($scope.invmpO_TotTax);
            $scope.invmpO_TotTax = $scope.invmpO_TotTax.toFixed(2);
        }

        //================== Count Rate
        $scope.countrate = function (objpo) {

            var a = 0;
            var r = 0;
            if ($scope.selectiontype === 'P') {
                $scope.rate = parseFloat(objpo.invtpI_PIUnitRate);
                $scope.qty = parseFloat(objpo.invtpO_POQty);
                objpo.invtpO_Amount = $scope.rate * $scope.qty;
                angular.forEach($scope.pidetails, function (obj) {
                    a += parseFloat(obj.invtpO_Amount);
                    r += parseFloat(obj.invtpI_PIUnitRate);
                })
            }
            else {
                $scope.rate = parseFloat(objpo.invtsQ_NegotiatedRate);
                $scope.qty = parseFloat(objpo.invtpO_POQty);
                objpo.invtpO_Amount = $scope.rate * $scope.qty;
                angular.forEach($scope.get_qtdetails, function (obj) {
                    a += parseFloat(obj.invtpO_Amount);
                    r += parseFloat(obj.invtsQ_NegotiatedRate);
                })
            }
            var totamt = a;
            $scope.invmpO_TotAmount = totamt;
            $scope.invmpO_TotAmount = parseFloat($scope.invmpO_TotAmount);
            $scope.invmpO_TotAmount = $scope.invmpO_TotAmount.toFixed(2);

            var ramt = r;
            $scope.invmpO_TotRate = ramt;
            $scope.invmpO_TotRate = parseFloat($scope.invmpO_TotRate);
            $scope.invmpO_TotRate = $scope.invmpO_TotRate.toFixed(2);


        }


        //================== Count Amount
        $scope.countPOAmt = function (objpo) {
            angular.forEach($scope.pidetails, function (obj) {
                if (obj.invtpO_POQty > obj.invtpI_ApproveQty) {
                    swal('Quantity Should Not Be More Than Approved Qty.');
                    obj.invtpO_POQty = "";

                }
                else {
                    var a = 0;

                    if ($scope.selectiontype === 'P') {
                        $scope.rate = parseFloat(objpo.invtpI_PIUnitRate);
                        $scope.qty = parseFloat(objpo.invtpO_POQty);
                        objpo.invtpO_Amount = $scope.rate * $scope.qty;
                        angular.forEach($scope.pidetails, function (obj) {
                            a += parseFloat(obj.invtpO_Amount);
                        })
                    }
                    else {
                        $scope.rate = parseFloat(objpo.invtsQ_NegotiatedRate);
                        $scope.qty = parseFloat(objpo.invtpO_POQty);
                        objpo.invtpO_Amount = $scope.rate * $scope.qty;
                        angular.forEach($scope.get_qtdetails, function (obj) {
                            a += parseFloat(obj.invtpO_Amount);
                        })
                    }
                    var totamt = a;
                    $scope.invmpO_TotAmount = totamt;
                    $scope.invmpO_TotAmount = parseFloat($scope.invmpO_TotAmount);
                    $scope.invmpO_TotAmount = $scope.invmpO_TotAmount.toFixed(2);
                }
            })



        }

        //======== Supplier validation
        $scope.isOptionsRequired = function () {
            return !$scope.get_pisupplier.some(function (sp) {
                return sp.spck;
            });
        }

        //===================================== SAVE DATA
        $scope.tempname = '';
        $scope.savedata = function () {
            $scope.submitted = true;
            $scope.tempname = '';
            if ($scope.ISES_Id > 0) {
                angular.forEach($scope.templatelist, function (ff) {

                    if (ff.iseS_Id == $scope.ISES_Id) {
                        $scope.tempname = ff.iseS_Template_Name;

                    }
                })
            }

            var data = {};
            if ($scope.myForm.$valid) {
                $scope.arrayPOtax = [];
                $scope.arrayPO = [];

                angular.forEach($scope.potax_list, function (tx) {
                    $scope.arrayPOtax.push({
                        invmiT_Id: tx.invmiT_Id, invtpoT_TaxPercent: tx.invmiT_TaxValue, invtpoT_TaxAmount: tx.inv_TaxAmount
                    });
                })

                if ($scope.selectiontype === 'P') {
                    angular.forEach($scope.pidetails, function (polist) {
                        $scope.arrayPO.push({
                            invmpI_Id: polist.invmpI_Id, invtpI_Id: polist.invtpI_Id, invmI_Id: polist.invmI_Id, invmuoM_Id: polist.invmuoM_Id, invmI_ItemName: polist.invmI_ItemName,
                            invmuoM_UOMName: polist.invmuoM_UOMName, invtpO_POQty: polist.invtpO_POQty, invtpO_RatePerUnit: polist.invtpI_PIUnitRate,
                            invtpO_TaxAmount: polist.invtpO_TaxAmount, invtpO_Amount: polist.invtpO_Amount, invtpO_Remarks: polist.invtpO_Remarks,
                            arrayPOtax: $scope.arrayPOtax
                        });
                    });
                }
                else {
                    angular.forEach($scope.get_qtdetails, function (polist) {
                        $scope.arrayPO.push({
                            invmpI_Id: polist.invmpI_Id, invtpI_Id: polist.invtpI_Id, invmI_Id: polist.invmI_Id, invmuoM_Id: polist.invmuoM_Id, invmI_ItemName: polist.invmI_ItemName,
                            invmuoM_UOMName: polist.invmuoM_UOMName, invtpO_POQty: polist.invtpO_POQty, invtpO_RatePerUnit: polist.invtsQ_NegotiatedRate,
                            invtpO_TaxAmount: polist.invtpO_TaxAmount, invtpO_Amount: polist.invtpO_Amount, invtpO_Remarks: polist.invtpO_Remarks, arrayPOtax: $scope.arrayPOtax
                        });
                    });
                }


                if ($scope.Template != null && $scope.Template != "") {

                    var tempname = document.getElementById("temp").innerHTML;
                    data = {
                        "INVMS_Id": $scope.obj.invmS_Id.invmS_Id,
                        "INVMPO_PODate": new Date($scope.invmpO_PODate).toDateString(),
                        "INVMPO_Remarks": $scope.invmpO_Remarks,
                        "INVMPO_ReferenceNo": $scope.invmpO_ReferenceNo,
                        "INVMPO_TotRate": $scope.invmpO_TotRate,
                        "INVMPO_TotTax": $scope.invmpO_TotTax,
                        "INVMPO_TotAmount": $scope.invmpO_TotAmount,
                        "arrayPO": $scope.arrayPO,
                        "INVMPO_Id": $scope.invmpO_Id,
                        //  "INVMPO_POTemplate": $scope.tempname,
                        //"INVMPO_POTemplate": $scope.Template,
                        "INVMPO_POTemplate": tempname,
                        transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                        "INVMPO_Status": "PO"
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("INV_PurchaseOrder/savedetails", data).then(function (promise) {
                        if (promise.returnval == true) {
                            if (promise.invmpO_Id == 0 || promise.invmpO_Id < 0) {
                                swal('Record saved successfully');
                            }
                            else if (promise.invmpO_Id > 0) {
                                swal('Record updated successfully');
                            }
                        }
                        else if (promise.returnduplicatestatus == 'Duplicate') {
                            swal('Record already exist');
                        }
                        else {
                            if (promise.invmpO_Id == 0 || promise.invmpO_Id < 0) {
                                swal('Failed to save, please contact administrator');
                            }
                            else if (promise.invmpO_Id > 0) {
                                swal('Failed to update, please contact administrator');
                            }
                        }
                        $state.reload();
                    })
                }
                else {
                    swal("Template Name Is Not Select ");
                }

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
            $scope.invmpO_Id = item.invmpO_Id;
            var dystring = "";
            if (item.invmpO_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invmpO_ActiveFlg == false) {
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
                        apiService.create("INV_PurchaseOrder/deactiveM", item).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
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

        $scope.deactive = function (item, SweetAlert) {
            $scope.invtpO_Id = item.invtpO_Id;
            var dystring = "";
            if (item.invtpO_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invtpO_ActiveFlg == false) {
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
                        apiService.create("INV_PurchaseOrder/deactive", item).
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

        $scope.deactiveTx = function (item, SweetAlert) {
            $scope.invtpoT_Id = item.invtpoT_Id;
            var dystring = "";
            if (item.invtpoT_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invtpoT_ActiveFlg == false) {
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
                        apiService.create("INV_PurchaseOrder/deactiveTx", item).
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
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        //to validate approved quntity



        $scope.searchValue = '';
        $scope.editflag = '';
        $scope.edit = function (item) {
            $scope.editflag = true;
            $scope.get_qtdetails = [];
            $scope.pidetails = [];
            var quoteid = 0;
            $scope.editS = true;
            $scope.invmpO_ReferenceNo = item.invmpO_ReferenceNo;
            $scope.invmpO_PODate = new Date(item.invmpO_PODate);
            $scope.invmpO_TotRate = item.invmpO_TotRate;
            $scope.invmpO_TotTax = item.invmpO_TotTax;
            $scope.invmpO_TotAmount = item.invmpO_TotAmount;
            $scope.invmpO_Id = item.invmpO_Id;
            $scope.obj.invmS_Id = item;
            $scope.obj.invmsQ_Id = item;

            var invmpO_Id = item.invmpO_Id;
            var invtpO_Id = item.invtpO_Id;
            var invmS_Id = item.invmS_Id;

            var data = {
                "INVMPO_Id": invmpO_Id,
                "INVTPO_Id": invtpO_Id,
                "INVMS_Id": invmS_Id
            }
            apiService.create("INV_PurchaseOrder/get_modeldetails", data).
                then(function (promise) {
                    $scope.get_editDetail = promise.get_poDetail;
                    $scope.get_edittaxDetail = promise.get_potax;
                    $scope.get_supdata = promise.get_supdata;
                    quoteid = parseInt($scope.get_editDetail[0].invmsQ_Id);
                    if ($scope.get_supdata.length > 0) {
                        $scope.supname = $scope.get_supdata[0].invmS_SupplierName;
                    }
                    if (quoteid === 0) {
                        $scope.selectiontype = "P";
                        angular.forEach($scope.get_editDetail, function (obj) {
                            $scope.pidetails.push(obj);
                        });
                    }
                    else {
                        $scope.selectiontype = "Q";
                        angular.forEach($scope.get_editDetail, function (obj) {
                            $scope.get_qtdetails.push(obj);
                        });
                    }
                    angular.forEach($scope.get_edittaxDetail, function (objtx) {
                        $scope.get_itemTax.push(objtx);
                    });

                    $scope.newcaste = {};
                    $scope.get_PI = [];
                    $scope.get_PI = promise.get_PI;
                    if ($scope.get_PI != null && $scope.get_PI.length > 0) {
                        for (var i = 0; i < $scope.get_PI.length; i++) {
                            if ($scope.pidetails[0].invmpI_Id == $scope.get_PI[i].invmpI_Id) {
                                $scope.get_PI[i].Selected = true;
                                $scope.obj.invmpI_Id = $scope.get_PI[i];
                                $scope.newcaste = $scope.get_PI[0].invmpI_Id;
                                return;
                            }
                        }
                    }
                });
        }


        $scope.edittx = function (itemtx) {
            $scope.get_itemTax = [];
            var invtpO_Id = itemtx.invtpO_Id;
            var data = {
                "INVTPO_Id": invtpO_Id
            }
            apiService.create("INV_PurchaseOrder/get_modeldetails", data).
                then(function (promise) {
                    $scope.get_edittaxDetail = promise.get_potax;
                    angular.forEach($scope.get_edittaxDetail, function (objtx) {
                        $scope.get_itemTax.push(objtx);
                    })

                })
        }

        $scope.onformclick = function (itemid) {
            $scope.get_poDetail = "";
            var poid = itemid.invmpO_Id;
            var potid = itemid.invmpO_Id;
            var data = {
                "INVMPO_Id": poid,
                "INVTPO_Id": potid
            }
            apiService.create("INV_PurchaseOrder/get_modeldetails", data).
                then(function (promise) {
                    $scope.get_poDetail = promise.get_poDetail;
                    $scope.ponum = $scope.get_poDetail[0].invmpO_PONo;
                    $scope.get_potax = promise.get_potax;
                    $scope.ponbr = $scope.get_potax[0].invmpO_PONo;
                    $scope.itm = $scope.get_potax[0].invmI_ItemName;
                })
        }
        //viewsavedtemplate
        $scope.viewsavedtemplate = function (user) {
            $scope.invmpO_ReferenceNo = "";
            $scope.pidetailstwo = [];
            $scope.invmpO_IdNew = "";
            $scope.emailid = "";
            $scope.invmpO_IdNew = user.invmpO_Id;

            document.getElementById("ordertemplate").innerHTML = user.invmpO_POTemplate;
            $("#popup11134").modal('show');
            angular.forEach($scope.get_supplier, function (dd) {
                if (dd.invmS_Id == user.invmS_Id) {
                    $scope.emailid = dd.invmS_EmailId;

                    return;
                }
            });
        }

        $scope.closemodal = function () {
            $scope.pidetails = [];
        }
        $scope.invmpO_IdNew = "";
        $scope.SendDataview = function () {
            var FHEAD = "";
            var msg = document.getElementById("ordertemplate").innerHTML;;
            if ($scope.materaldocuupload != null && $scope.materaldocuupload.length > 0) {
                angular.forEach($scope.materaldocuupload, function (objmail) {
                    if (objmail.ccmail != "") {
                        if (FHEAD != "") {
                            FHEAD = FHEAD + ',' + objmail.ccmail;
                        }
                        else {
                            FHEAD = objmail.ccmail;
                        }

                    }

                })
            }


            var data = {
                ISMMCLT_ClientName: "",
                "esubject": "Purchase Order",
                "msg": msg,
                "ISMMPR_Id": $scope.invmpO_IdNew,
                "ClientMail": $scope.emailid,
                "FHEAD": FHEAD,
                "ISMINC_WorkOrder": ""
            };
            apiService.create("ClientInvoice/getreport", data).then(function (promise) {
                swal("Email  Sent SucessFully ");
                $scope.esubjec = "";
                $scope.MSG = "";
                $scope.FHEAD = "";
                $scope.Footer = "";
                $scope.cfilepath = "";
                $scope.clemail = "";
                $('#popup11134').modal('hide');
                $scope.pidetails = [];
                $scope.loaddata();

            });
        }

        //SendDataSave
        $scope.SendDataSave = function () {          
            var msg = document.getElementById("ordertemplate").innerHTML;;            
            var data = {                             
                "msg": msg,
                "ISMMPR_Id": $scope.invmpO_IdNew,               
                "ISMINC_WorkOrder": "SAVE"
            };
            apiService.create("ClientInvoice/getreport", data).then(function (promise) {
                swal("Template Save SucessFully ");    
                $scope.esubjec = "";
                $scope.MSG = "";
                $scope.FHEAD = "";
                $scope.Footer = "";
                $scope.cfilepath = "";
                $scope.clemail = "";
                $('#popup11134').modal('hide');
                $scope.pidetails = [];
                $scope.loaddata();
            });
        }

    }
})();