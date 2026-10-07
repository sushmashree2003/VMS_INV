
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_ItemReportController', INV_ItemReportController);
    INV_ItemReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter'];
    function INV_ItemReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {
        $scope.obj = {};
        $scope.optionflag = "Item";
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
        //==========================================================
        $scope.onclickchange = function () {
            $scope.optionflag = '';
            $scope.get_itemreport = [];
        }
        //=====================
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";

            var data = {
                "optionflag": $scope.optionflag,
                "MI_Id_new": $scope.MI_Id
            };
            apiService.create("INV_ItemReport/getloaddata", data).
                then(function (promise) {
                    $scope.get_itemreportdetails = promise.get_itemreportdetails;
                    $scope.staffcheck = promise.staffcheck;
                    if ($scope.MI_Id == undefined) {
                        $scope.get_mi_list = promise.get_mi_list;
                    }
                    if ($scope.staffcheck == 'Staff') {
                        $scope.MI_Id = promise.get_mi_list[0].MI_Id;
                    }

                });
        };
        $scope.onrdochange = function (optionflag) {
            $scope.get_itemreportdetails = "";
            $scope.get_itemreport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag, $scope.MI_Id);
        };

        //========================ITEM
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.get_itemreportdetails.every(function (itm) {
                return itm.itemck;
            });
        };
        $scope.isOptionsRequiredI = function () {
            if ($scope.optionflag === 'Item') {
                return !$scope.get_itemreportdetails.some(function (options) {
                    return options.itemck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkI = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.get_itemreportdetails, function (itm) {
                itm.itemck = toggleStatus;
            });
        };
        //==================================Item Report
        $scope.submitted = false;
        $scope.onreport = function () {
            $("#grid123").kendoGrid();
            $('#grid123').data('kendoGrid').dataSource.read();
            $('#grid123').data('kendoGrid').refresh();

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
                $scope.itemsArray = [];
                $scope.grnArray = [];
                if ($scope.optionflag === null || $scope.optionflag === undefined) {
                    $scope.optionflag = "";
                }
                var data = {};
                if ($scope.optionflag === "Item") {
                    angular.forEach($scope.get_itemreportdetails, function (itm) {
                        if (itm.itemck === true) {
                            $scope.itemsArray.push(itm);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "itemsArray": $scope.itemsArray,
                        "INVMG_Id": 0,
                        "bwdateflag": $scope.bw_dates,
                        "MI_Id_new": $scope.MI_Id
                    };
                }
                else {

                    angular.forEach($scope.get_itemreportdetails, function (itm) {
                        if (itm.itemck === true) {
                            $scope.grnArray.push(itm);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "grnArray": $scope.grnArray,

                        "bwdateflag": $scope.bw_dates,
                        "MI_Id_new": $scope.MI_Id
                    };
                }
                apiService.create("INV_ItemReport/onreport", data).
                    then(function (promise) {
                        if (promise.get_itemreport != null && promise.get_itemreport.length > 0) {
                            var count = 0;
                            if ($scope.optionflag == "Group") {
                                $scope.get_itemreport = true;
                                $scope.tempaggary = [];

                                //INVMI_DepreciationPercentage
                                $scope.insarray = [{ name: "rowNumber" },
                                { name: "INVMG_GroupName" },
                                { name: "INVMI_ItemName" },
                                { name: "INVMI_ItemType" },
                                { name: "INVMI_ItemAssetType" },
                                { name: "INVMI_DepreciationPercentage" },
                                { name: "INVMI_ItemCode" },
                                { name: "INVMUOM_UOMName" },
                                { name: "INVMI_MaxStock" },
                                { name: "INVMI_ReorderStock" },
                                { name: "INVMI_HSNCode" },
                                { name: "INVMI_TaxAplFlg" },
                                { name: "INVMI_ForSaleFlg" },
                                { name: "INVMI_RawMatFlg" }];

                                $scope.columns1 = [
                                    { feild: "rowNumber", name: "rowNumber" },
                                    { field: "INVMG_GroupName", name: "INVMG_GroupName", title: "Group Name" },
                                    { field: "INVMI_ItemName", name: "INVMI_ItemName", title: "Item Name" },
                                    { field: "INVMI_ItemType", name: "INVMI_ItemType", title: "Item Type" },
                                    { field: "INVMI_ItemAssetType", name: "INVMI_ItemAssetType", title: "Asset Type" },
                                    { field: "INVMI_DepreciationPercentage", name: "INVMI_DepreciationPercentage", title: "Depreciation Percentage" },
                                    { field: "INVMI_ItemCode", name: "INVMI_ItemCode", title: "Item Code" },
                                    { field: "INVMUOM_UOMName", name: "INVMUOM_UOMName", title: "UOM" },
                                    { field: "INVMI_MaxStock", name: "INVMI_MaxStock", title: "Max Stock" },
                                    { field: "INVMI_ReorderStock", name: "INVMI_ReorderStock", title: "Re-Order Stock" },
                                    { field: "INVMI_HSNCode", name: "INVMI_HSNCode", title: "HSN Code" },
                                    { field: "INVMI_TaxAplFlg", name: "INVMI_TaxAplFlg", title: "Tax" },
                                    { field: "INVMI_ForSaleFlg", name: "INVMI_ForSaleFlg", title: "For Sale" },
                                    {
                                        field: "INVMI_RawMatFlg", name: "INVMI_RawMatFlg", title: "Raw Material"
                                    }];
                                $scope.newarray = [];

                                $scope.newarray1 = $scope.newarray;
                                $scope.data = promise.get_itemreport;
                                if (promise.get_itemreport.length > 0) {
                                    angular.forEach($scope.get_itemreport, function (po) {
                                    });
                                    for (var i in $scope.data) {
                                        $scope.data[i].rowNumber = count + 1;
                                        count = count + 1;
                                    }

                                    angular.forEach($scope.header_list, function (op) {
                                        op.columns = [];
                                        angular.forEach($scope.columns1, function (op123) {

                                            op.columns.push({ field: "hema" + op.ftI_Id + op123.field, name: "hema" + op.ftI_Id + op123.name, title: op123.title, width: 100 });

                                        })
                                        op.title = op.ftI_Name;
                                    })

                                    $scope.colarrayall = [{
                                        name: 'rowNumber',
                                        field: 'rowNumber',
                                        title: "SLNO",
                                        template: "<span class='row-number'></span>", width: 75

                                    },
                                    {
                                        name: 'INVMG_GroupName', field: 'INVMG_GroupName', title: 'Group Name', width: 150
                                    },
                                    {
                                        name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item Name', width: 150
                                    },
                                    {
                                        name: 'INVMI_ItemType', field: 'INVMI_ItemType', title: 'Item Type', width: 150
                                    },
                                    {
                                        name: 'INVMI_ItemAssetType', field: 'INVMI_ItemAssetType', title: 'Asset Type', width: 150
                                    },
                                    {
                                        name: 'INVMI_DepreciationPercentage', field: 'INVMI_DepreciationPercentage', title: 'Depreciation Percentage', width: 150
                                    },
                                    {
                                        name: 'INVMI_ItemCode', field: 'INVMI_ItemCode', title: 'Item Code', width: 150
                                    },
                                    {
                                        name: 'INVMUOM_UOMName', field: 'INVMUOM_UOMName', title: 'UOM', width: 150
                                    },

                                    {
                                        name: 'INVMI_MaxStock', field: 'INVMI_MaxStock', title: 'Max Stock', width: 200
                                    },
                                    {
                                        name: 'INVMI_ReorderStock', field: 'INVMI_ReorderStock', title: 'Re-Order Stock', width: 200
                                    },
                                    {
                                        name: 'INVMI_HSNCode', field: 'INVMI_HSNCode', title: 'HSN Code', width: 250
                                    },

                                    {
                                        name: 'INVMI_TaxAplFlg', field: 'INVMI_TaxAplFlg', title: 'Tax', width: 100
                                    },
                                    {
                                        name: 'INVMI_ForSaleFlg', field: 'INVMI_ForSaleFlg', title: 'For Sale', width: 150
                                    },
                                    {
                                        name: 'INVMI_RawMatFlg', field: 'INVMI_RawMatFlg', title: 'Raw Material', width: 100
                                    }
                                    ];

                                    angular.forEach($scope.header_list, function (obj) {
                                        $scope.colarrayall.push(obj);
                                    })

                                    console.log($scope.header_list);

                                    console.log($scope.colarrayall);

                                    $scope.txtdata = "TIME :" + " " + $filter('date')(new Date(), 'HH:mm:ss') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname;

                                    console.log($scope.txtdata);
                                    $scope.aaaa = [{
                                        title: $scope.txtdata,
                                        columns: $scope.colarrayall
                                    }]


                                    console.log($scope.data);
                                    var gridall;

                                    $(document).ready(function () {
                                        initGridall();

                                    });



                                    function initGridall() {
                                        $('#grid123').empty();
                                        gridall = $("#grid123").kendoGrid({
                                            toolbar: ["excel", "pdf"],
                                            excel: {
                                                fileName: "ItemReport.xlsx",
                                                proxyURL: "",
                                                filterable: true,
                                                allPages: true
                                            },
                                            pdf: {
                                                avoidLinks: true,
                                                landscape: true,
                                                repeatHeaders: true,
                                                fileName: "ItemReport.pdf",
                                                allPages: true
                                            },
                                            dataSource: {

                                                //type: "odata",
                                                //transport: {
                                                //    read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Products"
                                                //},
                                                data: $scope.data,
                                                aggregate: $scope.tempaggary,
                                                pageSize: 10

                                            },
                                            sortable: true,
                                            pageable: true,
                                            groupable: false,
                                            filterable: true,
                                            columnMenu: true,
                                            reorderable: true,
                                            resizable: true,
                                            columns: $scope.aaaa,
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
                                        gridall.setOptions({
                                            sortable: true
                                        });
                                    }
                                }
                            }
                            else {
                                $scope.get_itemreport = true;
                                $scope.tempaggary = [];


                                $scope.insarray = [{ name: "rowNumber" },
                                { name: "INVMI_ItemName" },
                                { name: "INVMI_ItemCode" },
                                { name: "INVMI_ItemType" },
                                { name: "INVMI_ItemAssetType" },
                                { name: "INVMI_DepreciationPercentage" },
                                { name: "INVMUOM_UOMName" },
                                { name: "INVMI_MaxStock" },
                                { name: "INVMI_ReorderStock" },
                                { name: "INVMI_HSNCode" },
                                { name: "INVMI_TaxAplFlg" },
                                { name: "INVMI_ForSaleFlg" },
                                { name: "INVMI_RawMatFlg" }];

                                $scope.columns1 = [{ field: "rowNumber", name: "rowNumber" },
                                { field: "INVMI_ItemName", name: "INVMI_ItemName", title: "Item Name" },
                                { field: "INVMI_ItemCode", name: "INVMI_ItemCode", title: "Item Code" },
                                { field: "INVMI_ItemType", name: "INVMI_ItemType", title: "Item Type" },
                                { field: "INVMI_ItemAssetType", name: "INVMI_ItemAssetType", title: "Asset Type" },
                                { field: "INVMI_DepreciationPercentage", name: "INVMI_DepreciationPercentage", title: "Depreciation Percentage" },
                                { field: "INVMUOM_UOMName", name: "INVMUOM_UOMName", title: "UOM" },
                                { field: "INVMI_MaxStock", name: "INVMI_MaxStock", title: "Max Stock" },
                                { field: "INVMI_ReorderStock", name: "INVMI_ReorderStock", title: "Re-Order Stock" },
                                { field: "INVMI_HSNCode", name: "INVMI_HSNCode", title: "HSN Code" },
                                { field: "INVMI_TaxAplFlg", name: "INVMI_TaxAplFlg", title: "Tax" },
                                { field: "INVMI_ForSaleFlg", name: "INVMI_ForSaleFlg", title: "For Sale" }, {
                                    field: "INVMI_RawMatFlg", name: "INVMI_RawMatFlg", title: "Raw Material"
                                }];
                                $scope.newarray = [];

                                $scope.newarray1 = $scope.newarray;
                                $scope.data = promise.get_itemreport;
                                if (promise.get_itemreport.length > 0) {
                                    angular.forEach($scope.get_itemreport, function (po) {
                                    });

                                    angular.forEach($scope.header_list, function (op) {
                                        op.columns = [];
                                        angular.forEach($scope.columns1, function (op123) {

                                            op.columns.push({ field: "hema" + op.ftI_Id + op123.field, name: "hema" + op.ftI_Id + op123.name, title: op123.title, width: 100 });

                                        })
                                        op.title = op.ftI_Name;
                                    })
                                    for (var i in $scope.data) {
                                        $scope.data[i].rowNumber = count + 1;
                                        count = count + 1;
                                    }
                                    $scope.colarrayall = [{
                                        name: 'rowNumber',
                                        field: 'rowNumber',
                                        title: "SLNO",
                                        template: "<span class='row-number'></span>", width: 75

                                    },

                                    {
                                        name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item Name', width: 150
                                    },

                                    {
                                        name: 'INVMI_ItemCode', field: 'INVMI_ItemCode', title: 'Item Code', width: 150
                                    },
                                    {
                                        name: 'INVMI_ItemType', field: 'INVMI_ItemType', title: 'Item Type', width: 150
                                    },
                                    {
                                        name: 'INVMI_ItemAssetType', field: 'INVMI_ItemAssetType', title: 'Asset Type', width: 150
                                    },
                                    {
                                        name: 'INVMI_DepreciationPercentage', field: 'INVMI_DepreciationPercentage', title: 'Depreciation Percentage', width: 150
                                    },
                                    {
                                        name: 'INVMUOM_UOMName', field: 'INVMUOM_UOMName', title: 'UOM', width: 150
                                    },

                                    {
                                        name: 'INVMI_MaxStock', field: 'INVMI_MaxStock', title: 'Max Stock', width: 200
                                    },
                                    {
                                        name: 'INVMI_ReorderStock', field: 'INVMI_ReorderStock', title: 'Re-Order Stock', width: 200
                                    },
                                    {
                                        name: 'INVMI_HSNCode', field: 'INVMI_HSNCode', title: 'HSN Code', width: 250
                                    },

                                    {
                                        name: 'INVMI_TaxAplFlg', field: 'INVMI_TaxAplFlg', title: 'Tax', width: 100
                                    },
                                    {
                                        name: 'INVMI_ForSaleFlg', field: 'INVMI_ForSaleFlg', title: 'For Sale', width: 150
                                    },
                                    {
                                        name: 'INVMI_RawMatFlg', field: 'INVMI_RawMatFlg', title: 'Raw Material', width: 100
                                    }
                                    ];

                                    angular.forEach($scope.header_list, function (obj) {
                                        $scope.colarrayall.push(obj);
                                    })

                                    console.log($scope.header_list);

                                    console.log($scope.colarrayall);

                                    $scope.txtdata = "TIME :" + " " + $filter('date')(new Date(), 'HH:mm:ss') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname;

                                    console.log($scope.txtdata);
                                    $scope.aaaa = [{
                                        title: $scope.txtdata,
                                        columns: $scope.colarrayall
                                    }]


                                    console.log($scope.data);
                                    var gridall;

                                    $(document).ready(function () {
                                        initGridall();

                                    });

                                    function initGridall() {
                                        $('#grid123').empty();
                                        gridall = $("#grid123").kendoGrid({
                                            toolbar: ["excel", "pdf"],
                                            excel: {
                                                fileName: "ItemReport.xlsx",
                                                proxyURL: "",
                                                filterable: true,
                                                allPages: true
                                            },
                                            pdf: {
                                                avoidLinks: true,
                                                landscape: true,
                                                repeatHeaders: true,
                                                fileName: "ItemReport.pdf",
                                                allPages: true
                                            },
                                            dataSource: {

                                                //type: "odata",
                                                //transport: {
                                                //    read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Products"
                                                //},
                                                data: $scope.data,
                                                aggregate: $scope.tempaggary,
                                                pageSize: 10

                                            },
                                            sortable: true,
                                            pageable: true,
                                            groupable: false,
                                            filterable: true,
                                            columnMenu: true,
                                            reorderable: true,
                                            resizable: true,
                                            columns: $scope.aaaa,
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
                                        gridall.setOptions({
                                            sortable: true
                                        });
                                    }
                                }
                            }
                        }
                        else {
                            swal("Record Not Found")
                        }
                      
                    });
            }
            else {
                $scope.submitted = true;
                $scope.get_itemreport = "";
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
        //    var exportHref = Excel.tableToExcel(export_id, 'printexcel');
        //    $timeout(function () {
        //        location.href = exportHref;
        //    }, 100);
        //};
        $scope.exportToExcel = function () {
            var exportHref = Excel.tableToExcel(printexcel, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };

        $scope.exportToPdf = function (export_id) {
            var exportHref = Excel.tableTopdf(export_id, 'sheet name');
            $timeout(function () {
                location.href = exportHref;
            }, 100);

        };
        $scope.printData = function () {
            var innerContents = document.getElementById("printItem").innerHTML;
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