
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_R_GRN_NewController', INV_R_GRN_NewController);
    INV_R_GRN_NewController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter'];
    function INV_R_GRN_NewController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {
        $scope.tablediv = false;
        $scope.printgrn = false;
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
        // $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        $scope.obj = {};
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.search = "";
        $scope.loaddata = function () {
            $scope.obj.MI_Id = "";
            var pageid = 2;
            apiService.getURI("CheckBankBalance/loaddata", pageid).then(function (promise) {
                $scope.get_mi_list = promise.allcompany;


            })
        };
        $scope.typerdochange = function () {
            $scope.tablediv = false;
            $scope.optionflag = '';
            $scope.get_grnreport = [];
        }

        $scope.onrdochange = function (optionflag) {
            $scope.get_grn_item_supplier = [];
            $scope.get_mi_listTemp = [];
            $scope.get_grnreport = [];
            if ($scope.get_mi_list != null && $scope.get_mi_list.length > 0) {
                angular.forEach($scope.get_mi_list, function (itm) {
                    if (itm.miid == true) {
                        $scope.get_mi_listTemp.push({
                           
                            MI_Id: itm.mI_Id
                        });
                    }
                    
                });
            }
            var data = {
                "optionflag": $scope.obj.flagP,             
                "INVMGRN_InvoiceNo": "GRNNew",
                "get_mi_arraylist": $scope.get_mi_listTemp

            };
            apiService.create("INV_R_GRN/getloaddata", data).
                then(function (promise) {
                    if (promise.get_grn_item_supplier != null && promise.get_grn_item_supplier.length > 0) {
                        $scope.get_grn_item_supplier = promise.get_grn_item_supplier;
                    }
                   
                  
                   

                });
        };
        $scope.isRequiredmi = function () {

            return !$scope.get_mi_list.some(function (options) {
                return options.miid;
            });

        };
        $scope.all_miid = function (ac) {
            $scope.allmi = ac;
            var toggleStatus = $scope.allmi;
            angular.forEach($scope.get_mi_list, function (itm) {
                itm.miid = toggleStatus;
            });
            $scope.onrdochange();
        };
        $scope.mi_click = function () {
            $scope.onrdochange();
        }
        //===================================GRN Number Select
      
        $scope.isOptionsRequired = function () {
            return !$scope.get_grn_item_supplier.some(function (options) {
                return options.grnck;
            });
        };
        //togchkbxG
        $scope.togchkbxG = function () {
            $scope.get_grnreport = [];
        }
        $scope.all_check = function (grnal) {
            $scope.grnall = grnal;
            var toggleStatus = $scope.grnall;
            angular.forEach($scope.get_grn_item_supplier, function (grn) {
                grn.grnck = toggleStatus;
            });
            $scope.get_grnreport = [];
        };
        //========================ITEM
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.get_grn_item_supplier.every(function (itm) {
                return itm.itemck;
            });
        };
        $scope.isOptionsRequiredI = function () {
            if ($scope.individualflag === 'Item') {
                return !$scope.get_grn_item_supplier.some(function (options) {
                    return options.itemck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkI = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.get_grn_item_supplier, function (itm) {
                itm.itemck = toggleStatus;
            });
        };
        //========================SUPPLIER
        $scope.togchkbxS = function () {
            $scope.suplierall = $scope.get_grn_item_supplier.every(function (sup) {
                return sup.suplierck;
            });
        };
        $scope.isOptionsRequiredS = function () {
            if ($scope.individualflag === 'Supplier') {
                return !$scope.get_grn_item_supplier.some(function (options) {
                    return options.suplierck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkS = function (itmi) {
            $scope.suplierall = itmi;
            var toggleStatus = $scope.suplierall;
            angular.forEach($scope.get_grn_item_supplier, function (sup) {
                sup.suplierck = toggleStatus;
            });
        };
        $scope.AssetchaNge = function () {
            $scope.get_grnreport = [];
        }
        //==================================Grn Report
        $scope.submitted = false;
        $scope.onreport = function () {
            if ($scope.myForm.$valid) {
                $scope.sDate = "";
                $scope.eDate = "";
                $scope.get_grnreport = [];
                $scope.get_mi_listTemp = [];
            $scope.grnArrayTemp = [];
                $scope.obj.start_Date = $filter('date')($scope.obj.startdate, "yyyy-MM-dd");
                $scope.obj.end_Date = $filter('date')($scope.obj.enddate, "yyyy-MM-dd");
                if ($scope.bw_dates === true) {
                    
                    $scope.sDate = $scope.obj.start_Date ;
                    $scope.eDate = $scope.obj.end_Date;
                }
                if ($scope.get_mi_list != null && $scope.get_mi_list.length > 0) {
                    angular.forEach($scope.get_mi_list, function (itm) {
                        if (itm.miid == true) {
                            $scope.get_mi_listTemp.push({

                                MI_Id: itm.mI_Id
                            });
                        }

                    });

                }
              
                if ($scope.get_grn_item_supplier != null && $scope.get_grn_item_supplier.length > 0 && $scope.obj.flagP == "Group" ) {
                    angular.forEach($scope.get_grn_item_supplier, function (itm) {
                        if (itm.grnck == true) {
                            $scope.grnArrayTemp.push({

                                INVMGRN_Id: itm.INVMG_Id
                            });
                        }

                    });
                }
                if ($scope.get_grn_item_supplier != null && $scope.get_grn_item_supplier.length > 0 && $scope.obj.flagP == "invoice") {
                    angular.forEach($scope.get_grn_item_supplier, function (itm) {
                        if (itm.grnck == true) {
                            $scope.grnArrayTemp.push({

                                INVMGRN_Id: itm.INVMGRN_Id
                            });
                        }

                    });
                }
                if ($scope.get_grn_item_supplier != null && $scope.get_grn_item_supplier.length > 0 && $scope.obj.flagP == "Item") {
                    angular.forEach($scope.get_grn_item_supplier, function (itm) {
                        if (itm.grnck == true) {
                            $scope.grnArrayTemp.push({

                                INVMGRN_Id: itm.INVMI_Id
                            });
                        }

                    });
                }
                if ($scope.get_grn_item_supplier != null && $scope.get_grn_item_supplier.length > 0 && $scope.obj.flagP == "Supplier") {
                    angular.forEach($scope.get_grn_item_supplier, function (itm) {
                        if (itm.grnck == true) {
                            $scope.grnArrayTemp.push({

                                INVMGRN_Id: itm.INVMS_Id
                            });
                        }

                    });
                }

                var data = {};
                
                data = {
                   
                    "AssetFlag": $scope.obj.Aseetflag,
                    "optionflag": "New",
                    "typeflag": $scope.obj.flagP,
                    "trans_id": $scope.obj.MI_Id,
                    "startdate": $scope.sDate,
                    "enddate": $scope.eDate,
                    "get_mi_arraylist": $scope.get_mi_listTemp,
                    "grnArray": $scope.grnArrayTemp
                   
                };
                apiService.create("INV_R_GRN/onreport", data).
                    then(function (promise) {
                        var TotalQty = 0; var purchaserate = 0; var TotalBasics = 0; var Totalsales = 0; var TotalDiscount = 0;
                        var Totaltax = 0; var totalAmount = 0; $scope.groupwiseexamlist_temp = [];
                        var counttotal = 0;
                        if (promise.get_grnreport != null && promise.get_grnreport.length > 0 && $scope.obj.flagP =="ALLG") {
                            $scope.get_grnreport = promise.get_grnreport;
                            $scope.employeeidTemp = promise.get_GRN;
                            $scope.presentCountgrid = $scope.get_grnreport.length;
                           
                            angular.forEach($scope.get_grnreport, function (user) {
                                user.INVTGRTotal = (Number(user.INVTGRN_PurchaseRate * user.INVTGRN_Qty) - (Number(user.INVTGRN_DiscountAmt)) + (Number(user.INVTGRN_TaxAmt))).toFixed(2);
                                user.TotalBasic = (Number(user.INVTGRN_PurchaseRate * user.INVTGRN_Qty)).toFixed(2);
                                TotalQty += user.INVTGRN_Qty;
                                purchaserate += user.INVTGRN_PurchaseRate;
                                TotalBasics += (Number(user.TotalBasic));
                                Totalsales += user.INVTGRN_SalesPrice;
                                TotalDiscount += user.INVTGRN_DiscountAmt;
                                Totaltax += user.INVTGRN_TaxAmt;
                                totalAmount +=(Number(user.INVTGRTotal));

                            });
                           


                        }
                        else if (promise.get_grnreport != null && promise.get_grnreport.length > 0 && $scope.obj.flagP == "Group") {
                            $scope.get_grnreport = promise.get_grnreport;
                            $scope.employeeidTemp = [];
                            $scope.employeeidTemp = promise.get_GRN;
                            $scope.presentCountgrid = promise.get_grnreport.length;
                            $scope.employeeid = [];
                            angular.forEach($scope.get_grnreport, function (dev) {
                                if ($scope.employeeid.length === 0) {
                                    $scope.employeeid.push({
                                        INVMG_ParentId: dev.INVMG_ParentId,
                                        INVMG_GroupName: dev.INVMG_GroupName,
                                        
                                    });
                                } else if ($scope.employeeid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.employeeid, function (emp) {
                                        if (emp.INVMG_ParentId === dev.INVMG_ParentId) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.employeeid.push({
                                            INVMG_ParentId: dev.INVMG_ParentId,
                                            INVMG_GroupName: dev.INVMG_GroupName,
                                        });
                                    }
                                }
                            });

                            angular.forEach($scope.employeeid, function (ddd) {
                                $scope.templist = [];
                                angular.forEach($scope.get_grnreport, function (dd) {
                                    if (dd.INVMG_ParentId === ddd.INVMG_ParentId) {
                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.grndetails = $scope.templist;
                            });
                            angular.forEach($scope.get_grnreport, function (user) {
                                var INVTGRTotal = (Number(user.INVTGRN_PurchaseRate * user.INVTGRN_Qty) - (Number(user.INVTGRN_DiscountAmt)) + (Number(user.INVTGRN_TaxAmt))).toFixed(2);
                                var TotalBasic = (Number(user.INVTGRN_PurchaseRate * user.INVTGRN_Qty)).toFixed(2);
                                TotalQty += user.INVTGRN_Qty;
                                purchaserate += user.INVTGRN_PurchaseRate;
                                TotalBasics += (Number(TotalBasic));
                                Totalsales += user.INVTGRN_SalesPrice;
                                TotalDiscount += user.INVTGRN_DiscountAmt;
                                Totaltax += user.INVTGRN_TaxAmt;
                                totalAmount += (Number(INVTGRTotal));

                            });
                        }
                        else if (promise.get_grnreport != null && promise.get_grnreport.length > 0 && $scope.obj.flagP == "invoice") {
                            $scope.employeeidTemp = [];
                            $scope.employeeidTemp = promise.get_GRN;
                            $scope.get_grnreport = promise.get_grnreport;
                       
                            $scope.presentCountgrid = promise.get_grnreport.length;
                            $scope.employeeid = [];
                            angular.forEach($scope.get_grnreport, function (dev) {
                                
                                if ($scope.employeeid.length === 0) {
                                    $scope.employeeid.push({
                                        INVMGRN_Id: dev.INVMGRN_Id,
                                        INVMGRN_InvoiceNo: dev.INVMGRN_InvoiceNo,
                                        PurchaseRate: dev.PurchaseRate,

                                    });
                                    
                                }
                                else if ($scope.employeeid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.employeeid, function (emp) {
                                        if (emp.INVMGRN_Id === dev.INVMGRN_Id) {
                                            intcount += 1;
                                           // counttotal += dev.INVTGRN_PurchaseRate;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.employeeid.push({
                                            INVMGRN_Id: dev.INVMGRN_Id,
                                            INVMGRN_InvoiceNo: dev.INVMGRN_InvoiceNo,
                                            PurchaseRate: dev.PurchaseRate,
                                        });
                                    }
                                }

                               
                            });
                           
                            //groupwiseexamlist_temp
                            angular.forEach($scope.employeeid, function (ddd) {
                                $scope.templist = [];
                                angular.forEach($scope.get_grnreport, function (dd) {
                                    if (dd.INVMGRN_Id === ddd.INVMGRN_Id) {
                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.grndetails = $scope.templist;
                            });
                          
                            angular.forEach($scope.get_grnreport, function (user) {
                                var INVTGRTotal = (Number(user.INVTGRN_PurchaseRate * user.INVTGRN_Qty) - (Number(user.INVTGRN_DiscountAmt)) + (Number(user.INVTGRN_TaxAmt))).toFixed(2);
                                var TotalBasic = (Number(user.INVTGRN_PurchaseRate * user.INVTGRN_Qty)).toFixed(2);
                                TotalQty += user.INVTGRN_Qty;
                                purchaserate += user.INVTGRN_PurchaseRate;
                                TotalBasics += (Number(TotalBasic));
                                Totalsales += user.INVTGRN_SalesPrice;
                                TotalDiscount += user.INVTGRN_DiscountAmt;
                                Totaltax += user.INVTGRN_TaxAmt;
                                totalAmount += (Number(INVTGRTotal));

                            });
                        }
                        else if (promise.get_grnreport != null && promise.get_grnreport.length > 0 && $scope.obj.flagP == "Supplier") {
                            $scope.get_grnreport = promise.get_grnreport;
                            $scope.employeeidTemp = [];
                            $scope.employeeidTemp = promise.get_GRN;
                            $scope.presentCountgrid = promise.get_grnreport.length;
                            $scope.employeeid = [];
                            angular.forEach($scope.get_grnreport, function (dev) {
                                if ($scope.employeeid.length === 0) {
                                    $scope.employeeid.push({
                                        INVMS_Id: dev.INVMS_Id,
                                        INVMS_SupplierName: dev.INVMS_SupplierName,

                                    });
                                } else if ($scope.employeeid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.employeeid, function (emp) {
                                        if (emp.INVMS_Id === dev.INVMS_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.employeeid.push({
                                            INVMS_Id: dev.INVMS_Id,
                                            INVMS_SupplierName: dev.INVMS_SupplierName,

                                        });
                                    }
                                }
                            });

                            angular.forEach($scope.employeeid, function (ddd) {
                                $scope.templist = [];
                                angular.forEach($scope.get_grnreport, function (dd) {
                                    if (dd.INVMS_Id === ddd.INVMS_Id) {
                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.grndetails = $scope.templist;
                            });
                            //dd
                            angular.forEach($scope.get_grnreport, function (user) {
                                var INVTGRTotal = (Number(user.INVTGRN_PurchaseRate * user.INVTGRN_Qty) - (Number(user.INVTGRN_DiscountAmt)) + (Number(user.INVTGRN_TaxAmt))).toFixed(2);
                                var TotalBasic = (Number(user.INVTGRN_PurchaseRate * user.INVTGRN_Qty)).toFixed(2);
                                TotalQty += user.INVTGRN_Qty;
                                purchaserate += user.INVTGRN_PurchaseRate;
                                TotalBasics += (Number(TotalBasic));
                                Totalsales += user.INVTGRN_SalesPrice;
                                TotalDiscount += user.INVTGRN_DiscountAmt;
                                Totaltax += user.INVTGRN_TaxAmt;
                                totalAmount += (Number(INVTGRTotal));

                            });
                        }
                        else if (promise.get_grnreport != null && promise.get_grnreport.length > 0 && $scope.obj.flagP == "Item") {
                            $scope.get_grnreport = promise.get_grnreport;
                            $scope.presentCountgrid = $scope.get_grnreport.length;
                            $scope.employeeidTemp = [];
                            $scope.employeeidTemp = promise.get_GRN;
                            angular.forEach($scope.get_grnreport, function (user) {
                                user.INVTGRTotal = (Number(user.INVTGRN_PurchaseRate * user.INVTGRN_Qty) - (Number(user.INVTGRN_DiscountAmt)) + (Number(user.INVTGRN_TaxAmt))).toFixed(2);
                                user.TotalBasic = (Number(user.INVTGRN_PurchaseRate * user.INVTGRN_Qty)).toFixed(2);
                                TotalQty += user.INVTGRN_Qty;
                                purchaserate += user.INVTGRN_PurchaseRate;
                                TotalBasics += (Number(user.TotalBasic));
                                Totalsales += user.INVTGRN_SalesPrice;
                                TotalDiscount += user.INVTGRN_DiscountAmt;
                                Totaltax += user.INVTGRN_TaxAmt;
                                totalAmount += (Number(user.INVTGRTotal));

                            });
                        }
                        else {
                            swal("Record Not Found !");
                        }                       
                         $scope.TotalQty = Math.round(Number(TotalQty));
                         $scope.purchaserate = Math.round(Number(purchaserate));
                         $scope.TotalBasics = Math.round(Number(TotalBasics));
                         $scope.Totalsales = Math.round(Number(Totalsales));
                         $scope.TotalDiscount = Math.round(Number(TotalDiscount));
                         $scope.Totaltax = Math.round(Number(Totaltax));
                         $scope.totalAmount = Math.round(Number(totalAmount));
                        //$scope.TotalQty = TotalQty.toFixed(2);
                        //$scope.purchaserate = purchaserate.toFixed(2);
                        //$scope.TotalBasics = TotalBasics.toFixed(2);
                        //$scope.Totalsales = Totalsales.toFixed(2);
                        //$scope.TotalDiscount = TotalDiscount.toFixed(2);
                        //$scope.Totaltax = Totaltax.toFixed(2);
                        //$scope.totalAmount = totalAmount.toFixed(2);
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
        $scope.exportToExcel = function () {    
            var excelnamemain = "GRN Report";
            var printSectionId = '#printgrn';
            $scope.start_Date = new Date();
            $scope.end_Date = new Date();
            $scope.from_dateex = $filter('date')($scope.start_Date, 'yyyy-MM-dd');
            $scope.to_dateex = $filter('date')($scope.end_Date, 'yyyy-MM-dd');
            excelnamemain = excelnamemain + '-' + $scope.from_dateex + ' To ' + $scope.to_dateex + '.xls';
            var exportHref = Excel.tableToExcel(printSectionId, 'GRN Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelnamemain;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };
        //======================================Print & Export to Excel
        //$scope.exportToExcel = function (export_id) {

        //    var exportHref = Excel.tableToExcel(export_id, 'printgrn');
        //    $timeout(function () {
        //        location.href = exportHref;
        //    }, 3000);
        //};
        $scope.printData = function () {

            var innerContents = document.getElementById("printgrn").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
    }
})();