
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CheckOutAssetsVerficationController', CheckOutAssetsController);
    CheckOutAssetsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function CheckOutAssetsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.obj = {};
        $scope.editS = false;
        $scope.searchValue1 = '';
        $scope.obj.INVACO_Time = "";

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null &&  ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.itemsPerPage1 = paginationformasters;
        $scope.currentPage1 = 1;
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //-------------------------------------------------------------------
        $scope.obj = {};
        $scope.obj.returnval = false;
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        $scope.obj.INVACO_No = "";
        $scope.searchValue = "";
        $scope.searchValuetwo = "";
        $scope.materaldocuupload = [{ id: 'materal' }];
        $scope.obj.start_Date = new Date();
        $scope.obj.end_Date = new Date();
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("CheckBankBalance/loaddata", pageid).then(function (promise) {
                $scope.allcompany = promise.allcompany;
                $scope.obj.MI_Id = "";               
                angular.forEach($scope.materaldocuupload, function (itm) {
                    itm.INVACO_CheckoutDate = new Date();
                });
            });
        };

        $scope.onstorechange = function () {
            $scope.transnumbconfigurationsettingsassign = {};
            $scope.get_items = [];
            $scope.get_employee = [];
            $scope.materaldocuupload = [];
            $scope.get_checkoutdclist = [];
            $scope.instlist = [];
            $scope.tempdc = []; $scope.instlist = [];
            $scope.materaldocuupload = [{ id: 'materal' }];
            var data = {
                "MI_Id": $scope.obj.MI_Id,
                "INVMSL_SalesDate": $scope.obj.start_Date,
                "INVMSL_DCDate": $scope.obj.end_Date,
            };
            apiService.create("INV_T_Sales/getsectionlist", data).then(function (promise) {               
                if (promise.get_checkoutdclist != null && promise.get_checkoutdclist.length > 0) {
                    $scope.get_checkoutdclist = promise.get_checkoutdclist;
                    $scope.instlist = promise.get_mi_list;
                    angular.forEach($scope.get_checkoutdclist, function (ddd) {
                        if ($scope.tempdc.length == 0) {
                            $scope.tempdc.push({
                                INVACO_No: ddd.invacO_No,
                                invacO_Tolocation: ddd.invacO_Tolocation,
                                invacO_CheckoutDate: new Date(ddd.invacO_CheckoutDate),
                                invacO_ReceivedBy: ddd.invacO_ReceivedBy,
                                INVACO_CheckOutRemarks: ddd.invacO_CheckOutRemarks,
                                INVACO_MODE: ddd.invacO_MODE,
                                INVACO_Vehicalno: ddd.invacO_Vehicalno,
                                INVACO_Time: ddd.invacO_Time,
                                INVACO_ISSUED: ddd.invacO_ISSUED,
                                invmlO_Status: ddd.invmlO_Status,
                                invacO_CallCenterRemarks: ddd.invacO_CallCenterRemarks,
                                invacO_Id: ddd.invacO_Id
                            });
                        }
                        else if ($scope.tempdc.length > 0) {
                            var already_cnt = 0;
                            angular.forEach($scope.tempdc, function (opq1) {
                                if (opq1.INVACO_No == ddd.invacO_No) {
                                    already_cnt += 1;
                                }
                            })
                            if (already_cnt == 0) {
                                $scope.tempdc.push({
                                    INVACO_No: ddd.invacO_No,
                                    invacO_ReceivedBy: ddd.invacO_ReceivedBy,
                                    invacO_Tolocation: ddd.invacO_Tolocation,
                                    invacO_CheckoutDate: new Date(ddd.invacO_CheckoutDate),
                                    INVACO_CheckOutRemarks: ddd.invacO_CheckOutRemarks,
                                    INVACO_MODE: ddd.invacO_MODE,
                                    INVACO_ISSUED: ddd.invacO_ISSUED,
                                    INVACO_Vehicalno: ddd.invacO_Vehicalno,
                                    INVACO_Time: ddd.invacO_Time,
                                    invmlO_Status: ddd.invmlO_Status,
                                    invacO_CallCenterRemarks: ddd.invacO_CallCenterRemarks,
                                     invacO_Id: ddd.invacO_Id
                                });
                            }
                        }
                    })

                    $scope.mainarray = [];
                    angular.forEach($scope.tempdc, function (ss) {
                        var listdat = [];
                        angular.forEach($scope.get_checkoutdclist, function (ff) {
                            if (ss.INVACO_No == ff.invacO_No) {
                                listdat.push(ff);
                            }

                        })
                        ss.LIST = listdat;
                    })
                }
                else {
                    swal("Record Not Found !");
                }

            });
        };
        $scope.SaveCallCenter = function () {
            $scope.SaleItem = [];
            if ($scope.tempdc != null && $scope.tempdc.length > 0) {
                angular.forEach($scope.tempdc, function (itm) {
                    if (itm.selected == true ) {                        
                        angular.forEach(itm.LIST, function (check) {
                            if (check.invacO_CallCenterRemarks != null && check.invacO_CallCenterRemarks != "") {
                                $scope.SaleItem.push({
                                    INVTSL_Id: check.invacO_Id,
                                    INVTSL_Naration: check.invacO_CallCenterRemarks
                                });
                            }
                        });
                    }
                });
                if ($scope.SaleItem != null && $scope.SaleItem.length > 0) {
                    var data = {
                        "MI_Id": $scope.obj.MI_Id,
                        "SaleItem": $scope.SaleItem
                    };
                    apiService.create("INV_T_Sales/getStudentClsSec", data).then(function (promise) {
                        if (promise.message == "save") {
                            swal("Record  Updated Successfully !");
                        }
                        else {
                            swal("Record Not Updated !");
                        }
                        //$state.reload();
                    });
                }
                else {
                    swal("Please Select At Least One Check Box  !");
                    return;
                }
                
            }
            
        }
       
        $scope.OnItemchange = function (materal) {
            angular.forEach($scope.get_items, function (itm) {
                // if (materal.INVMI_Id.INVMI_Id == itm.INVMI_Id) {
                if (materal.INVMI_Id.INVSTO_Id == itm.INVSTO_Id) {
                    materal.INVSTO_AvaiableStock = "";
                    materal.INVMI_Id_new = "";
                    materal.INVMS_StoreName = "";
                    materal.INVMS_StoreName = itm.INVMS_StoreName;
                    materal.INVSTO_PurchaseDate = new Date(itm.INVSTO_PurchaseDate);
                    materal.INVSTO_SalesRate = itm.INVSTO_SalesRate;
                    materal.INVSTO_AvaiableStock = itm.INVSTO_AvaiableStock;
                    materal.INVMI_Id_new = itm.INVMI_Id;
                    materal.INVMST_Id = itm.INVMST_Id;
                    materal.INVACO_CheckoutDate = new Date();
                    return;
                }

            });

        }             
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cancel = function () {
            $state.reload();
        };            
        //added       
        $scope.reschedule = function (obj) {
            $scope.ttl = 0;
            $scope.imagename = $scope.instlist[0].mI_Logo;
            $scope.miname = $scope.instlist[0].mI_Name;
            $scope.miname = angular.uppercase($scope.miname);
            $scope.migst = $scope.instlist[0].mI_GSTNO;
            $scope.invaO_Tolocation = "";
            $scope.decno = "";
            $scope.Notes = "";
            $scope.Mode = "";
            $scope.Issued = "";
            $scope.printstudents = [];
            angular.forEach(obj.LIST, function (dd) {
                $scope.ttl = $scope.ttl + dd.invacO_CheckOutQty;
                $scope.printstudents.push(dd);
                $scope.dcdate = dd.updatedDate;
            })
            console.log($scope.printstudents);
            $scope.tempto = [];
            $scope.temptorecby = [];
            $scope.temptorecbyname = '';
            angular.forEach($scope.printstudents, function (ddd) {
                if ($scope.temptorecby.length == 0) {
                    $scope.temptorecby.push({ invacO_ReceivedBy: ddd.invacO_ReceivedBy });
                    $scope.temptorecbyname = ddd.invacO_ReceivedBy;
                }
                else if ($scope.temptorecby.length > 0) {
                    var already_cnt = 0;
                    angular.forEach($scope.temptorecby, function (opq1) {
                        if (opq1.invacO_ReceivedBy == ddd.invacO_ReceivedBy) {
                            already_cnt += 1;
                        }
                    })

                    if (already_cnt == 0) {
                        $scope.temptorecby.push({ invacO_ReceivedBy: ddd.invacO_ReceivedBy });
                        $scope.temptorecbyname = $scope.temptorecbyname + ' , ' + ddd.invacO_ReceivedBy;
                    }

                }
            })

            $scope.decno = obj.INVACO_No;
            $scope.invaO_Tolocation = obj.invacO_Tolocation;
            document.getElementById("invaO_Tolocation").innerText = $scope.invaO_Tolocation;
            $scope.Note = obj.INVACO_CheckOutRemarks;
            $scope.Mode = obj.INVACO_MODE;
            $scope.Issued = obj.INVACO_ISSUED;
            $scope.Vehicalno = obj.INVACO_Vehicalno;
            $scope.Time = obj.INVACO_Time;

            $('#popup111').modal('show');
        }

        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("asdf").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
    }
})();