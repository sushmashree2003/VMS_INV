
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_R_GRNController', INV_R_GRNController);
    INV_R_GRNController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter'];
    function INV_R_GRNController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {
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


        //==============================
        $scope.all_miid = function (ac) {
            $scope.allmi = ac;
            var toggleStatus = $scope.allmi;
            angular.forEach($scope.get_mi_list, function (itm) {
                itm.miid = toggleStatus;
            });
        };

        $scope.mi_click = function () {
            $scope.allmi = $scope.get_mi_list.every(function (itm) {
                return itm.miid;
            });
            $scope.get_mi_arraylist1 = [];
            angular.forEach($scope.get_mi_list, function (qq) {
                if (qq.miid == true) {
                    $scope.get_mi_arraylist1.push({ MI_Id: qq.MI_Id })
                }

            })
            $scope.flag = $scope.optionflag;
            $scope.loaddata($scope.flag, $scope.get_mi_arraylist1);

        };


        $scope.isRequiredmi = function () {

            return !$scope.get_mi_list.some(function (options) {
                return options.miid;
            });

        };

        //==========================================================

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var data = {
                "optionflag": $scope.optionflag,
                "get_mi_arraylist": $scope.get_mi_arraylist1,

            };
            apiService.create("INV_R_GRN/getloaddata", data).
                then(function (promise) {
                    $scope.staffcheck = promise.staffcheck;
                    if ($scope.staffcheck == 'Staff') {
                        $scope.MI_Name = promise.get_mi_list[0].MI_Name;
                        $scope.MI_Id = promise.get_mi_list[0].MI_Id;
                        angular.forEach($scope.get_mi_list, function (a) {
                            a.miid = true;
                        })
                        $scope.get_grn_item_supplier = promise.get_grn_item_supplier;
                    }
                    else {
                        $scope.get_grn_item_supplier = promise.get_grn_item_supplier;
                        if ($scope.get_mi_list == undefined) {
                            $scope.get_mi_list = promise.get_mi_list;

                            angular.forEach($scope.get_mi_list, function (qq) {
                                qq.miid = true;
                            })
                            $scope.allmi = true;
                        }
                    }

                });
        };
        $scope.typerdochange = function () {
            $scope.tablediv = false;
            $scope.optionflag = '';
        }

        $scope.onrdochange = function (optionflag) {
            $scope.tablediv = false;
            $scope.get_mi_arraylist1 = [];
            angular.forEach($scope.get_mi_list, function (qq) {
                if (qq.miid == true) {
                    $scope.get_mi_arraylist1.push({ MI_Id: qq.MI_Id })
                }

            })
            $scope.get_grn_item_supplier = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag, $scope.get_mi_arraylist1);
        };
        $scope.onrdochange1 = function (optionflag) {
            $scope.tablediv = false;
            $scope.get_mi_arraylist1 = [];
            angular.forEach($scope.get_mi_list, function (qq) {
                if (qq.miid == true) {
                    $scope.get_mi_arraylist1.push({ MI_Id: qq.MI_Id })
                }

            })
            $scope.get_grn_item_supplier = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag, $scope.get_mi_arraylist1);
        };
        //===================================GRN Number Select
        $scope.togchkbxG = function () {
            $scope.grnall = $scope.get_grn_item_supplier.every(function (itm) {
                return itm.grnck;
            });
        };
        $scope.isOptionsRequired = function () {
            return !$scope.get_grn_item_supplier.some(function (options) {
                return options.grnck;
            });
        };
        $scope.all_check = function (grnal) {
            $scope.grnall = grnal;
            var toggleStatus = $scope.grnall;
            angular.forEach($scope.get_grn_item_supplier, function (grn) {
                grn.grnck = toggleStatus;
            });
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
                $scope.start_Date = $filter('date')($scope.startdate, "yyyy-MM-dd");
                $scope.end_Date = $filter('date')($scope.enddate, "yyyy-MM-dd");
                if ($scope.bw_dates === true) {
                    $scope.sDate = $scope.start_Date;
                    $scope.eDate = $scope.end_Date;
                }
                else {
                    $scope.sDate = "";
                    $scope.eDate = "";
                }

                $scope.grnArray = [];
                $scope.itemArray = [];
                $scope.supplierArray = [];
                $scope.get_mi_arraylist = [];
                if ($scope.staffcheck == 'Staff') {
                    $scope.get_mi_arraylist.push({ MI_Id: $scope.MI_Id })
                }
                else {
                    angular.forEach($scope.get_mi_list, function (qq) {
                        if (qq.miid == true) {
                            $scope.get_mi_arraylist.push({ MI_Id: qq.MI_Id })
                        }

                    })
                }

                if ($scope.optionflag === null || $scope.optionflag === undefined) {
                    $scope.optionflag = "";
                }
                var data = {};
                if ($scope.typeflag === "All") {
                    data = {
                        "optionflag": $scope.optionflag,
                        "typeflag": $scope.typeflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,
                        "AssetFlag": $scope.obj.Aseetflag
                    };
                }
                else if ($scope.optionflag === "Individual") {
                    angular.forEach($scope.get_grn_item_supplier, function (gn) {
                        if (gn.grnck === true) {
                            $scope.grnArray.push(gn);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "typeflag": $scope.typeflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "grnArray": $scope.grnArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,
                        "AssetFlag": $scope.obj.Aseetflag
                    };
                }
                else if ($scope.optionflag === "Item") {
                    angular.forEach($scope.get_grn_item_supplier, function (itm) {
                        if (itm.itemck === true) {
                            $scope.itemArray.push(itm);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "typeflag": $scope.typeflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "itemArray": $scope.itemArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,
                        "AssetFlag": $scope.obj.Aseetflag
                    };
                }
                else if ($scope.optionflag === "Supplier") {
                    angular.forEach($scope.get_grn_item_supplier, function (sup) {
                        if (sup.suplierck === true) {
                            $scope.supplierArray.push(sup);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "typeflag": $scope.typeflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "supplierArray": $scope.supplierArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,
                        "AssetFlag": $scope.obj.Aseetflag
                    };
                }
                apiService.create("INV_R_GRN/onreport", data).
                    then(function (promise) {
                        if (promise.get_grnreport.length > 0) {
                            var totalamt = 0.00;
                            var totaltax = 0.00;
                            var totaldiscount = 0.00;
                            var finalamt = 0.00;
                            var count = 0;
                            $scope.get_grnreport = promise.get_grnreport;
                            $scope.presentCountgrid = $scope.get_grnreport.length;


                            $scope.tablediv = true;
                            $scope.printgrn = true;
                            $scope.tempaggary = [];
                            for (var i in $scope.get_grnreport) {
                                $scope.get_grnreport[i].rowNumber = count + 1;
                                var myDate = $filter('date')($scope.get_grnreport[i].INVMGRN_PurchaseDate, 'dd-MM-yyyy');
                                $scope.get_grnreport[i].INVMGRN_PurchaseDate = myDate;
                                count = count + 1;
                            }
                            if ($scope.typeflag === "All" || $scope.typeflag === "Detailed") {
                                angular.forEach($scope.get_grnreport, function (grn) {
                                    totalamt += parseFloat(grn.INVTGRN_Amount);
                                    $scope.totalamt = totalamt;
                                    $scope.totalamt = parseFloat($scope.totalamt);
                                    $scope.totalamt = $scope.totalamt.toFixed(2);
                                    totaltax += parseFloat(grn.INVTGRN_TaxAmt);
                                    $scope.totaltax = totaltax;
                                    $scope.totaltax = parseFloat($scope.totaltax);
                                    $scope.totaltax = $scope.totaltax.toFixed(2);
                                    totaldiscount += parseFloat(grn.INVTGRN_DiscountAmt);
                                    $scope.totaldiscount = totaldiscount;
                                    $scope.totaldiscount = parseFloat($scope.totaldiscount);
                                    $scope.totaldiscount = $scope.totaldiscount.toFixed(2);
                                });
                                finalamt = parseFloat($scope.totalamt) + parseFloat($scope.totaltax) - parseFloat($scope.totaldiscount);
                                $scope.finalamt = parseFloat(finalamt);
                                $scope.finalamt = $scope.finalamt.toFixed(2);

                                $scope.colarrayall = [{
                                    name: 'rowNumber',
                                    field: 'rowNumber',
                                    title: "SLNO",
                                    template: "<span class='row-number'></span>", width: "80px"
                                },
                                {
                                    name: 'INVMG_GroupName', field: 'INVMG_GroupName', title: 'Group Name', width: "150px"
                                },
                                {
                                    name: 'INVMGRN_PurchaseDate', field: 'INVMGRN_PurchaseDate', title: 'Purchase Date', width: "130px"
                                    //, template: "#= kendo.toString(kendo.parseDate(INVMGRN_PurchaseDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                                },
                                {
                                    name: 'INVMS_SupplierName', field: 'INVMS_SupplierName', title: 'Supplier', width: "150px"
                                },
                                {
                                    name: 'INVMGRN_GRNNo', field: 'INVMGRN_GRNNo', title: 'GRN No', width: "130px"
                                },
                                {
                                    name: 'INVMGRN_InvoiceNo', field: 'INVMGRN_InvoiceNo', title: 'Invoice No', width: "130px"
                                },
                                {
                                    name: 'INVTGRN_AssetTypeFlag', field: 'INVTGRN_AssetTypeFlag', title: 'Asset Type', width: "130px"
                                },
                                //
                                {
                                    name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item', width: "130px"
                                },
                                {
                                    name: 'INVMUOM_UOMName', field: 'INVMUOM_UOMName', title: 'UOM', width: "100px"
                                },
                                {
                                    name: 'INVTGRN_Qty', field: 'INVTGRN_Qty', title: 'Quantity', width: "100px"
                                },
                                {
                                    name: 'INVTGRN_PurchaseRate', field: 'INVTGRN_PurchaseRate', title: 'Rate', width: "100px"
                                },
                                {
                                    name: 'INVTGRN_MRP', field: 'INVTGRN_MRP', title: 'MRP', width: "100px"
                                },
                                {
                                    name: 'INVTGRN_SalesPrice', field: 'INVTGRN_SalesPrice', title: 'Sale Rate', width: "100px"
                                },
                                {
                                    name: 'INVTGRN_DiscountAmt', field: 'INVTGRN_DiscountAmt', title: 'Discount', width: "100px"
                                },
                                {
                                    name: 'INVTGRN_TaxAmt', field: 'INVTGRN_TaxAmt', title: 'Tax', width: "100px", footerTemplate: "Total:",
                                    groupFooterTemplate: "Total: "
                                },
                                {
                                    name: 'INVTGRN_Amount', field: 'INVTGRN_Amount', title: 'Total Amount', width: "100px", aggregates: ["sum"], footerTemplate: "#=sum#",
                                    groupFooterTemplate: " #=sum#"
                                }


                                ];
                                $scope.tempaggary.push({ field: 'INVTGRN_Amount', name: 'INVTGRN_Amount', aggregate: "sum" });
                            }


                            else if ($scope.typeflag === "Overall") {
                                angular.forEach($scope.get_grnreport, function (grn) {
                                    totalamt += parseFloat(grn.grnAmount);
                                    $scope.totalamt = totalamt;
                                    $scope.totalamt = parseFloat($scope.totalamt);
                                    $scope.totalamt = $scope.totalamt.toFixed(2);
                                    totaltax += parseFloat(grn.grnTax);
                                    $scope.totaltax = totaltax;
                                    $scope.totaltax = parseFloat($scope.totaltax);
                                    $scope.totaltax = $scope.totaltax.toFixed(2);
                                    totaldiscount += parseFloat(grn.grnDiscount);
                                    $scope.totaldiscount = totaldiscount;
                                    $scope.totaldiscount = parseFloat($scope.totaldiscount);
                                    $scope.totaldiscount = $scope.totaldiscount.toFixed(2);
                                });
                                finalamt = parseFloat($scope.totalamt) + parseFloat($scope.totaltax) - parseFloat($scope.totaldiscount);
                                $scope.finalamt = parseFloat(finalamt);
                                $scope.finalamt = $scope.finalamt.toFixed(2);

                                if ($scope.optionflag === "Item") {
                                    $scope.colarrayall = [{
                                        name: 'rowNumber',
                                        field: 'rowNumber',
                                        title: "SLNO",
                                        template: "<span class='row-number'></span>", width: "80px"
                                    },
                                    {
                                        name: 'INVMG_GroupName', field: 'INVMG_GroupName', title: 'Group Name', width: "150px"
                                    },
                                    {
                                        name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item', width: "150px"
                                    },
                                    {
                                        name: 'INVTGRN_AssetTypeFlag', field: 'INVTGRN_AssetTypeFlag', title: 'Asset Type', width: "130px"
                                    },
                                    {
                                        name: 'INVMI_ItemCode', field: 'INVMI_ItemCode', title: 'Code', width: "100px"
                                    },
                                    {
                                        name: 'grnQuantity', field: 'grnQuantity', title: 'Quantity', width: "100px"
                                    },
                                    {
                                        name: 'grnRate', field: 'grnRate', title: 'Purchase Rate', width: "100px"
                                    },
                                    {
                                        name: 'grnMRP', field: 'grnMRP', title: 'MRP', width: "100px"
                                    },
                                    {
                                        name: 'grnSalePrice', field: 'grnSalePrice', title: 'Sale Price', width: "100px"
                                    },
                                    {
                                        name: 'grnDiscount', field: 'grnDiscount', title: 'Discount', width: "100px"
                                    },
                                    {
                                        name: 'grnTax', field: 'grnTax', title: 'Tax', width: "100px", footerTemplate: "Total:",
                                        groupFooterTemplate: "Total: "
                                    },
                                    {
                                        name: 'grnAmount', field: 'grnAmount', title: 'Amount', width: "100px", aggregates: ["sum"], footerTemplate: "#=sum#",
                                        groupFooterTemplate: " #=sum#"
                                    }

                                    ];
                                    $scope.tempaggary.push({ field: 'grnAmount', name: 'grnAmount', aggregate: "sum" });
                                }
                                else if ($scope.optionflag === "Supplier") {
                                    $scope.colarrayall = [{
                                        name: 'rowNumber',
                                        field: 'rowNumber',
                                        title: "SLNO",
                                        template: "<span class='row-number'></span>", width: "80px"
                                    },
                                    {
                                        name: 'INVMG_GroupName', field: 'INVMG_GroupName', title: 'Group Name', width: "150px"
                                    },
                                    {
                                        name: 'INVMS_SupplierName', field: 'INVMS_SupplierName', title: 'Supplier Name', width: "150px"
                                    },
                                    {
                                        name: 'INVTGRN_AssetTypeFlag', field: 'INVTGRN_AssetTypeFlag', title: 'Asset Type', width: "130px"
                                    },
                                    {
                                        name: 'INVMS_SupplierCode', field: 'INVMS_SupplierCode', title: 'Code', width: "100px"
                                    },
                                    {
                                        name: 'INVMS_SupplierConatctNo', field: 'INVMS_SupplierConatctNo', title: 'Contact No.', width: "100px"
                                    },
                                    {
                                        name: 'INVMS_EmailId', field: 'INVMS_EmailId', title: 'Email_Id', width: "100px"
                                    },
                                    {
                                        name: 'grnQuantity', field: 'grnQuantity', title: 'Quantity', width: "100px"
                                    },
                                    {
                                        name: 'grnRate', field: 'grnRate', title: 'Purchase Rate', width: "100px"
                                    },
                                    {
                                        name: 'grnMRP', field: 'grnMRP', title: 'MRP', width: "100px"
                                    },
                                    {
                                        name: 'grnSalePrice', field: 'grnSalePrice', title: 'Sale Price', width: "100px"
                                    },
                                    {
                                        name: 'grnDiscount', field: 'grnDiscount', title: 'Discount', width: "100px"
                                    },
                                    {
                                        name: 'grnTax', field: 'grnTax', title: 'Tax', width: "100px", footerTemplate: "Total:",
                                        groupFooterTemplate: "Total: "
                                    },
                                    {
                                        name: 'grnAmount', field: 'grnAmount', title: 'Amount', width: "100px", aggregates: ["sum"], footerTemplate: "#=sum#",
                                        groupFooterTemplate: " #=sum#"
                                    }
                                    ];
                                    $scope.tempaggary.push({ field: 'grnAmount', name: 'grnAmount', aggregate: "sum" });
                                }
                            }
                            $(document).ready(function () {
                                initGridall();
                            });

                            function initGridall() {
                                $('#gridall').empty();

                                $("#gridall").kendoGrid({
                                    toolbar: ["excel"],
                                    excel: {
                                        fileName: "GRNReport.xlsx",
                                        proxyURL: "",
                                        filterable: true,
                                        allPages: true
                                    },
                                    pdf: {
                                        avoidLinks: true,
                                        landscape: true,
                                        repeatHeaders: true,
                                        fileName: "GRNReport.pdf",
                                        allPages: true
                                    },
                                    dataSource: {
                                        data: $scope.get_grnreport,
                                        pageSize: 10,
                                        aggregate: $scope.tempaggary

                                    },

                                    sortable: true,
                                    pageable: true,
                                    groupable: false,
                                    filterable: true,
                                    columnMenu: true,
                                    reorderable: true,
                                    resizable: true,

                                    columns: $scope.colarrayall,
                                    dataBound: function () {
                                        var pagenum = this.dataSource.page();
                                        var pageitms = this.dataSource.pageSize();
                                        var rows = this.items();
                                        $(rows).each(function () {
                                            var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                            var rowLabel = $(this).find(".row-number");
                                            $(rowLabel).html(index);
                                        });
                                    }

                                }).data("kendoGrid");
                            }
                        }
                        else {
                            swal("No Record Found...!!");
                            $scope.get_grnreport = "";
                        }
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
        //======================================Print & Export to Excel
        //$scope.exportToExcel = function (export_id) {

        //    var exportHref = Excel.tableToExcel(export_id, 'printgrn');
        //    $timeout(function () {
        //        location.href = exportHref;
        //    }, 100);
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