
(function () {
    'use strict';
    angular
        .module('app')
        .controller('AssetTag_ReportController', AssetTag_ReportController);
    AssetTag_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter'];
    function AssetTag_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {


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

        //==========================
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
            apiService.create("AssetTag_Report/getloaddata", data).
                then(function (promise) {
                    $scope.staffcheck = promise.staffcheck;
                    if ($scope.staffcheck == 'Staff') {
                        $scope.get_tagdetails = promise.get_tagdetails;
                        $scope.MI_Name = promise.get_mi_list[0].MI_Name;
                        $scope.MI_Id = promise.get_mi_list[0].MI_Id;
                        angular.forEach($scope.get_mi_list, function (a) {
                            a.miid = true;
                        })
                    }
                    else {
                        $scope.get_tagdetails = promise.get_tagdetails;
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

        $scope.onrdochange = function (optionflag) {
            $scope.get_mi_arraylist1 = [];
            $scope.get_tagreport = [];
            angular.forEach($scope.get_mi_list, function (qq) {
                if (qq.miid == true) {
                    $scope.get_mi_arraylist1.push({ MI_Id: qq.MI_Id })
                }

            })
            $scope.get_tagdetails = "";
            $scope.get_tagreport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag, $scope.get_mi_arraylist1);
        };

        //========================ITEM
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.get_tagdetails.every(function (itm) {
                return itm.itemck;
            });
        };
        $scope.isOptionsRequiredI = function () {
            if ($scope.optionflag === 'Item') {
                return !$scope.get_tagdetails.some(function (options) {
                    return options.itemck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkI = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.get_tagdetails, function (itm) {
                itm.itemck = toggleStatus;
            });
        };
        //========================ITEM
        $scope.togchkbxST = function () {
            $scope.stall = $scope.get_tagdetails.every(function (str) {
                return str.stck;
            });
        };
        $scope.isOptionsRequiredST = function () {
            if ($scope.optionflag === 'Store') {
                return !$scope.get_tagdetails.some(function (options) {
                    return options.stck;
                });
            } else {
                return false;
            }
        };

        $scope.all_checkST = function (stri) {
            $scope.stall = stri;
            var toggleStatus = $scope.stall;
            angular.forEach($scope.get_tagdetails, function (st) {
                st.stck = toggleStatus;
            });
        };

        //==================================PO Report
        $scope.submitted = false;
        $scope.onreport = function () {

            $("#grid123").kendoGrid();
            $('#grid123').data('kendoGrid').dataSource.read();
            $('#grid123').data('kendoGrid').refresh();

            if ($scope.myForm.$valid) {
                var data = {};
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

                $scope.tagItemArray = [];
                $scope.tagStoreArray = [];
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
                if ($scope.optionflag === "All") {
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist
                    };
                }
                else if ($scope.optionflag === "Item") {
                    angular.forEach($scope.get_tagdetails, function (itm) {
                        if (itm.itemck === true) {
                            $scope.tagItemArray.push(itm);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "tagItemArray": $scope.tagItemArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist
                    };
                }
                else if ($scope.optionflag === "Store") {
                    angular.forEach($scope.get_tagdetails, function (st) {
                        if (st.stck === true) {
                            $scope.tagStoreArray.push(st);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "tagStoreArray": $scope.tagStoreArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist
                    };
                }
                apiService.create("AssetTag_Report/onreport", data).
                    then(function (promise) {
                        //$scope.get_tagreport = [];
                        //if (promise.get_tagreport.length > 0) {                         
                        //    $scope.get_tagreport = promise.get_tagreport;
                        //    $scope.presentCountgrid = $scope.get_tagreport.length;
                        //}
                        //else {
                        //    swal("No Record Found...!!");
                        //    $scope.get_tagreport = "";
                        //}

                        if ($scope.optionflag === "Store") {
                            $scope.get_tagreport = true;
                            $scope.tempaggary = [];


                            $scope.insarray = [{ name: "INVMS_StoreName" }, { name: "INVMI_ItemName" }, { name: "INVAAT_AssetId" }, { name: "INVAAT_AssetDescription" }, { name: "INVAAT_PurchaseDate" }, { name: "INVAAT_ModelNo" }, { name: "INVAAT_SerialNo" }, { name: "INVAAT_SKU" }, { name: "INVAAT_WarantyPeriod" }, { name: "INVAAT_WarantyExpiryDate" }, { name: "INVAAT_UnderAMCFlg" }, { name: "INVAAT_AMCExpiryDate" }];

                            $scope.columns1 = [{ field: "INVMS_StoreName", name: "INVMS_StoreName", title: "Store" }, { field: "INVMI_ItemName", name: "INVMI_ItemName", title: "Item" }, { field: "INVAAT_AssetId", name: "INVAAT_AssetId", title: "Tag" }, { field: "INVAAT_AssetDescription", name: "INVAAT_AssetDescription", title: "Description" }, { field: "INVAAT_PurchaseDate", name: "INVAAT_PurchaseDate", title: "Purchase Date" }, { field: "INVAAT_ModelNo", name: "INVAAT_ModelNo", title: "Model No." }, { field: "INVAAT_SerialNo", name: "INVAAT_SerialNo", title: "Serial No." }, { field: "INVAAT_SKU", name: "INVAAT_SKU", title: "SKU" }, { field: "INVAAT_WarantyPeriod", name: "INVAAT_WarantyPeriod", title: "Warranty Period" }, {
                                field: "INVAAT_WarantyExpiryDate", name: "INVAAT_WarantyExpiryDate", title: "Waranty Expiry"
                            }, { field: "INVAAT_UnderAMCFlg", name: "INVAAT_UnderAMCFlg", title: "AMC" }, {
                                field: "INVAAT_AMCExpiryDate", name: "INVAAT_AMCExpiryDate", title: "AMC Expiry"
                            }];
                            $scope.newarray = [];

                            $scope.newarray1 = $scope.newarray;
                            $scope.data = promise.get_tagreport;
                            if (promise.get_tagreport.length > 0) {
                                angular.forEach($scope.get_tagreport, function (po) {
                                });

                                angular.forEach($scope.header_list, function (op) {
                                    op.columns = [];
                                    angular.forEach($scope.columns1, function (op123) {

                                        op.columns.push({ field: "hema" + op.ftI_Id + op123.field, name: "hema" + op.ftI_Id + op123.name, title: op123.title, width: 100 });

                                    })
                                    op.title = op.ftI_Name;
                                })

                                $scope.colarrayall = [{

                                    title: "SLNO",
                                    template: "<span class='row-number'></span>", width: 75

                                },
                                {
                                    name: 'INVMS_StoreName', field: 'INVMS_StoreName', title: 'Store', width: 150
                                },
                                {
                                    name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item', width: 150
                                },

                                {
                                    name: 'INVAAT_AssetId', field: 'INVAAT_AssetId', title: 'Tag', width: 150
                                },
                                {
                                    name: 'INVAAT_AssetDescription', field: 'INVAAT_AssetDescription', title: 'Description', width: 150
                                },

                                {
                                    name: 'INVAAT_PurchaseDate', field: 'INVAAT_PurchaseDate', title: 'Purchase Date', width: 200
                                    /* template: "#= kendo.toString(kendo.parseDate(INVAAT_PurchaseDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"*/
                                },
                                {
                                    name: 'INVAAT_ModelNo', field: 'INVAAT_ModelNo', title: 'Model No.', width: 200
                                },
                                {
                                    name: 'INVAAT_SerialNo', field: 'INVAAT_SerialNo', title: 'Serial No.', width: 200
                                },
                                {
                                    name: 'INVAAT_SKU', field: 'INVAAT_SKU', title: 'SKU', width: 250
                                },

                                {
                                    name: 'INVAAT_WarantyPeriod', field: 'INVAAT_WarantyPeriod', title: 'Warranty Period', width: 100
                                },
                                {
                                    name: 'INVAAT_WarantyExpiryDate', field: 'INVAAT_WarantyExpiryDate', title: 'Waranty Expiry', width: 150
                                    /*, template: "#= kendo.toString(kendo.parseDate(INVAAT_WarantyExpiryDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"*/
                                },
                                {
                                    name: 'INVAAT_UnderAMCFlg', field: 'INVAAT_UnderAMCFlg', title: 'AMC', width: 100
                                },
                                {
                                    name: 'INVAAT_AMCExpiryDate', field: 'INVAAT_AMCExpiryDate', title: 'AMC Expiry', width: 100
                                    /*, template: "#= kendo.toString(kendo.parseDate(INVAAT_AMCExpiryDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"*/
                                }
                                ];

                                angular.forEach($scope.header_list, function (obj) {
                                    $scope.colarrayall.push(obj);
                                })

                                console.log($scope.header_list);

                                console.log($scope.colarrayall);

                                //$scope.txtdata = "TIME :" + " " + $filter('date')(new Date(), 'HH:mm:ss') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname;
                                $scope.txtdata = "Asset Tag Report";
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
                                            fileName: "AssetTagReport.xlsx",
                                            proxyURL: "",
                                            filterable: true,
                                            allPages: true
                                        },
                                        pdf: {
                                            avoidLinks: true,
                                            landscape: true,
                                            repeatHeaders: true,
                                            fileName: "AssetTagReport.pdf",
                                            allPages: true,
                                            paperSize: "A4",
                                            scale: 0.35,
                                            margin: {
                                                top: "2cm",
                                                left: "1cm",
                                                right: "1cm",
                                                bottom: "1cm"
                                            },
                                        },
                                        dataSource: {

                                            //type: "odata",
                                            //transport: {
                                            //    read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Products"
                                            //},
                                            data: $scope.data,
                                            aggregate: $scope.tempaggary,
                                            pageSize: 7

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
                        else
                        {
                            $scope.get_tagreport = true;
                            $scope.tempaggary = [];


                            $scope.insarray = [{ name: "INVMI_ItemName" }, { name: "INVAAT_AssetId" }, { name: "INVAAT_AssetDescription" }, { name: "INVAAT_PurchaseDate" }, { name: "INVAAT_ModelNo" }, { name: "INVAAT_SerialNo" }, { name: "INVAAT_SKU" }, { name: "INVAAT_WarantyPeriod" }, { name: "INVAAT_WarantyExpiryDate" }, { name: "INVAAT_UnderAMCFlg" }, { name: "INVAAT_AMCExpiryDate" }];

                            $scope.columns1 = [{ field: "INVMI_ItemName", name: "INVMI_ItemName", title: "Item" }, { field: "INVAAT_AssetId", name: "INVAAT_AssetId", title: "Tag" }, { field: "INVAAT_AssetDescription", name: "INVAAT_AssetDescription", title: "Description" }, { field: "INVAAT_PurchaseDate", name: "INVAAT_PurchaseDate", title: "Purchase Date" }, { field: "INVAAT_ModelNo", name: "INVAAT_ModelNo", title: "Model No." }, { field: "INVAAT_SerialNo", name: "INVAAT_SerialNo", title: "Serial No." }, { field: "INVAAT_SKU", name: "INVAAT_SKU", title: "SKU" }, { field: "INVAAT_WarantyPeriod", name: "INVAAT_WarantyPeriod", title: "Warranty Period" }, {
                                field: "INVAAT_WarantyExpiryDate", name: "INVAAT_WarantyExpiryDate", title: "Waranty Expiry"
                            }, { field: "INVAAT_UnderAMCFlg", name: "INVAAT_UnderAMCFlg", title: "AMC" }, {
                                field: "INVAAT_AMCExpiryDate", name: "INVAAT_AMCExpiryDate", title: "AMC Expiry"
                            }];
                            $scope.newarray = [];

                            $scope.newarray1 = $scope.newarray;
                            $scope.data = promise.get_tagreport;
                            if (promise.get_tagreport.length > 0) {
                                angular.forEach($scope.get_tagreport, function (po) {
                                });

                                angular.forEach($scope.header_list, function (op) {
                                    op.columns = [];
                                    angular.forEach($scope.columns1, function (op123) {

                                        op.columns.push({ field: "hema" + op.ftI_Id + op123.field, name: "hema" + op.ftI_Id + op123.name, title: op123.title, width: 100 });

                                    })
                                    op.title = op.ftI_Name;
                                })

                                $scope.colarrayall = [{

                                    title: "SLNO",
                                    template: "<span class='row-number'></span>", width: 75

                                },
                                {
                                    name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item', width: 150
                                },

                                {
                                    name: 'INVAAT_AssetId', field: 'INVAAT_AssetId', title: 'Tag', width: 150
                                },
                                {
                                    name: 'INVAAT_AssetDescription', field: 'INVAAT_AssetDescription', title: 'Description', width: 150
                                },

                                {
                                    name: 'INVAAT_PurchaseDate', field: 'INVAAT_PurchaseDate', title: 'Purchase Date', width: 200
                                    /* template: "#= kendo.toString(kendo.parseDate(INVAAT_PurchaseDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"*/
                                },
                                {
                                    name: 'INVAAT_ModelNo', field: 'INVAAT_ModelNo', title: 'Model No.', width: 200
                                },
                                {
                                    name: 'INVAAT_SerialNo', field: 'INVAAT_SerialNo', title: 'Serial No.', width: 200
                                },
                                {
                                    name: 'INVAAT_SKU', field: 'INVAAT_SKU', title: 'SKU', width: 250
                                },

                                {
                                    name: 'INVAAT_WarantyPeriod', field: 'INVAAT_WarantyPeriod', title: 'Warranty Period', width: 100
                                },
                                {
                                    name: 'INVAAT_WarantyExpiryDate', field: 'INVAAT_WarantyExpiryDate', title: 'Waranty Expiry', width: 150
                                    /*, template: "#= kendo.toString(kendo.parseDate(INVAAT_WarantyExpiryDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"*/
                                },
                                {
                                    name: 'INVAAT_UnderAMCFlg', field: 'INVAAT_UnderAMCFlg', title: 'AMC', width: 100
                                },
                                {
                                    name: 'INVAAT_AMCExpiryDate', field: 'INVAAT_AMCExpiryDate', title: 'AMC Expiry', width: 100
                                    /*, template: "#= kendo.toString(kendo.parseDate(INVAAT_AMCExpiryDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"*/
                                }
                                ];

                                angular.forEach($scope.header_list, function (obj) {
                                    $scope.colarrayall.push(obj);
                                })

                                console.log($scope.header_list);

                                console.log($scope.colarrayall);

                                //$scope.txtdata = "TIME :" + " " + $filter('date')(new Date(), 'HH:mm:ss') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname;
                                $scope.txtdata = "Asset Tag Report";
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
                                            fileName: "AssetTagReport.xlsx",
                                            proxyURL: "",
                                            filterable: true,
                                            allPages: true
                                        },
                                        pdf: {
                                            avoidLinks: true,
                                            landscape: true,
                                            repeatHeaders: true,
                                            fileName: "AssetTagReport.pdf",
                                            allPages: true,
                                            paperSize: "A4",
                                            scale: 0.35,
                                            margin: {
                                                top: "2cm",
                                                left: "1cm",
                                                right: "1cm",
                                                bottom: "1cm"
                                            },
                                        },
                                        dataSource: {

                                            //type: "odata",
                                            //transport: {
                                            //    read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Products"
                                            //},
                                            data: $scope.data,
                                            aggregate: $scope.tempaggary,
                                            pageSize: 7

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

        //    var exportHref = Excel.tableToExcel(export_id, 'sheet name');
        //    $timeout(function () {
        //        location.href = exportHref;
        //    }, 100);

        //};
        $scope.exportToExcel = function (export_id) {
            var exportHref = Excel.tableToExcel(export_id, 'printtag');
            $timeout(function () {
                location.href = exportHref;
            }, 1000);
        };

        $scope.exportToPdf = function (export_id) {
            var exportHref = Excel.tableTopdf(export_id, 'printtag');
            $timeout(function () {
                location.href = exportHref;
            }, 100);

        };
        $scope.printData = function () {

            var innerContents = document.getElementById("printtag").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 10000);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
    }
})();