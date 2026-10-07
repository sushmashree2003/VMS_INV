
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_RequisitionNotificationController', INV_RequisitionNotificationController)

    INV_RequisitionNotificationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel','$timeout']
    function INV_RequisitionNotificationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

        var date = new Date();
        $scope.invmpR_PRDate = date;
        $scope.addro = false;
        $scope.obj = {};
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        //=====================Adding and removing new row in transcation Grid============      
       
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
        $scope.obj = {};
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = paginationformasters;
        $scope.searchValue = "";
        //====================================== Page Load
        $scope.loaddata = function () {         
            var pageid = 2;
            apiService.getURI("INV_RequisitionNotification/getloaddata", pageid).then(function (promise) {
                $scope.get_mi_list = promise.get_mi_list;
                $scope.obj.MI_Id = promise.mI_Id;                                                     
                $scope.get_purchaserequisition = promise.get_purchaserequisition;
                $scope.rolflag = promise.rolflag;
            });
        };
        $scope.all_checkCCCC = function () {
            var invmpR_IdAll = $scope.obj.invmpR_IdAll;            
            angular.forEach($scope.get_purchaserequisition, function (itm) {
                itm.selected = invmpR_IdAll;                
            });
        };
        $scope.isOptionsRequiredtwo = function () {
            return !$scope.get_purchaserequisition.some(function (options) {
                return options.selected;
            });
        };
        $scope.exportToExcel = function () {
            var exportHref = Excel.tableToExcel(asdf, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };
        $scope.printDataV = function () {
            var innerContents = "";
            innerContents = document.getElementById("asdf").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 10000);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.OnChangeinst = function (mi_id) {           
            $scope.get_purchaserequisition = [];
            var data = {
                "MI_IdNew": mi_id
            }
            apiService.create("INV_PurchaseRequisition/GetOnchangeData", data).then(function (promise) {              
                $scope.get_purchaserequisition = promise.get_purchaserequisition;
            });
        }       
        $scope.savedata = function () {
            $scope.arrayPR = [];
            $scope.get_Details = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var INVMPR_Id = 0;
                if ($scope.get_purchaserequisition != null && $scope.get_purchaserequisition.length > 0 && $scope.rdopunch == "Detailed") {                   
                        angular.forEach($scope.get_purchaserequisition, function (itm) {
                            if (itm.selected === true) {
                                $scope.arrayPR.push({
                                    INVMPR_Id: itm.invmpR_Id
                                })
                            }                          
                        });                                                        
                }
                else {
                    INVMPR_Id = $scope.obj.invmpR_Id.invmpR_Id;
                }
                var data = {
                    "MI_Id": $scope.obj.MI_Id,
                    "INVMPR_Id": INVMPR_Id ,
                    "arrayPR": $scope.arrayPR,
                    "Flag": "MPurchase_Req"

                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("INV_RequisitionNotification/savedetails", data).then(function (promise) {
                    if (promise.get_Details != null && promise.get_Details.length > 0) {
                        $scope.get_Details = promise.get_Details;
                        $scope.logo = promise.logo;
                    }
                    else {
                        Swal("Record Not Found !");
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
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };      
        //printItem
        $scope.printItems = function () {
            var innerContents = "";
            innerContents = document.getElementById("printItem").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 10000);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        //printApproval
        $scope.printApproval = function () {
            var innerContents = "";
            innerContents = document.getElementById("printAp").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 10000);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.printremark = function () {
            var innerContents = "";
            innerContents = document.getElementById("printApPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 10000);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.printApremark = function () {
            var innerContents = "";
            innerContents = document.getElementById("printApPPPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 10000);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.onformclick = function (itemid) {
            $scope.get_prDetail = [];
            $scope.INVMPR_PRNo = itemid.INVMPR_PRNo;
            var item_id = itemid.INVMPR_Id;
            var data = {
                "INVMPR_Id": item_id,
                "MI_IdNew": $scope.obj.MI_Id
            }
            apiService.create("INV_PurchaseRequisition/get_prdetails", data).
                then(function (promise) {
                    $scope.get_prDetail = promise.get_prDetail;
                    $scope.prnum = $scope.get_prDetail[0].invmpR_PRNo;
                    $scope.reqname = $scope.get_prDetail[0].employeename;
                    console.log($scope.get_prDetail);
                })
        }
        //otherdaydatechange
        $scope.otherdaydatechange = function () {
        }
        //IndentRemark
        $scope.IndentRemark = function (user) {
            //$('#myModal').modal('hide');
            $('#myModalfile').modal('hide');
            $scope.get_IndentDetail = [];
            $scope.get_prDetail = [];
          
            var data = {
                "MI_Id": $scope.obj.MI_Id,
                "INVMPR_Id": user.INVMPR_Id,                
                "Flag": "Purchase_Indent"
            }
            apiService.create("INV_RequisitionNotification/savedetails", data).
                then(function (promise) {
                    if (promise.get_Details != null && promise.get_Details.length > 0) {
                        $scope.get_IndentDetail = promise.get_Details;
                        $scope.prnum = user.INVMPR_PRNo;
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
                "MI_Id": $scope.obj.MI_Id,
                "INVMPR_Id": user.INVMPR_Id,
                "Flag": "OrderAp_Remark"
            }
            apiService.create("INV_RequisitionNotification/savedetails", data).
                then(function (promise) {
                    if (promise.get_Details != null && promise.get_Details.length > 0) {
                        $scope.get_ApprovalDetail = promise.get_Details;
                        $scope.prnum = user.INVMPR_PRNo;
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
                "MI_Id": $scope.obj.MI_Id,
                "INVMPR_Id": user.INVMPR_Id,
                "Flag": "Order_Remark"
            }
            apiService.create("INV_RequisitionNotification/savedetails", data).
                then(function (promise) {
                    if (promise.get_Details != null && promise.get_Details.length > 0) {
                        $scope.get_ApprovalDetail = promise.get_Details;
                        $scope.prnum = user.INVMPR_PRNo;                                           
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
                "MI_Id": $scope.obj.MI_Id,
                "INVMPR_Id": user.INVMPR_Id,
                "Flag": "Purchase_Approved"
            }
            apiService.create("INV_RequisitionNotification/savedetails", data).
                then(function (promise) {
                    if (promise.get_Details != null && promise.get_Details.length > 0) {
                        $scope.get_ApprovalDetail = promise.get_Details;
                        $scope.prnum = user.INVMPR_PRNo;
                        $scope.logo = promise.logo;
                        $('#myApproval').modal('show');

                    }
                    else {
                       // Swal("Record Not Found !");
                        swal("Not Approved First Level  !");
                    }
                })
        }

    }
})();