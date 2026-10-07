
(function () {
    'use strict';
    angular
        .module('app')
        .controller('AssetTagCheckout_ReportController', AssetTagCheckout_ReportController);
    AssetTagCheckout_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter'];
    function AssetTagCheckout_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {


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
            apiService.create("AssetTagCheckout_Report/getloaddata", data).
                then(function (promise) {
                    $scope.staffcheck = promise.staffcheck;
                    if ($scope.staffcheck == 'Staff') {
                        $scope.get_ckoutdetails = promise.get_ckoutdetails;
                          $scope.MI_Name = promise.get_mi_list[0].MI_Name;
                            $scope.MI_Id = promise.get_mi_list[0].MI_Id;
                        angular.forEach($scope.get_mi_list, function (a) {
                            a.miid = true;
                        }) 
                    }
                    else {
                        $scope.get_ckoutdetails = promise.get_ckoutdetails;
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
            angular.forEach($scope.get_mi_list, function (qq) {
                if (qq.miid == true) {
                    $scope.get_mi_arraylist1.push({ MI_Id: qq.MI_Id })
                }

            })
            $scope.get_ckoutdetails = "";
            $scope.get_ckoutreport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag, $scope.get_mi_arraylist1);
        };
        //========================Tag
        $scope.togchkbxtg = function () {
            $scope.tgall = $scope.get_ckoutdetails.every(function (tg) {
                return tg.tgck;
            });
        };
        $scope.isOptionsRequiredtg = function () {
            if ($scope.optionflag === 'Tag') {
                return !$scope.get_ckoutdetails.some(function (options) {
                    return options.tgck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checktg = function (tg) {
            $scope.tgall = tg;
            var toggleStatus = $scope.tgall;
            angular.forEach($scope.get_ckoutdetails, function (tg) {
                tg.tgck = toggleStatus;
            });
        };
        //========================ITEM
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.get_ckoutdetails.every(function (itm) {
                return itm.itemck;
            });
        };
        $scope.isOptionsRequiredI = function () {
            if ($scope.optionflag === 'Item') {
                return !$scope.get_ckoutdetails.some(function (options) {
                    return options.itemck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkI = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.get_ckoutdetails, function (itm) {
                itm.itemck = toggleStatus;
            });
        };
        //========================Store
        $scope.togchkbxST = function () {
            $scope.stall = $scope.get_ckoutdetails.every(function (str) {
                return str.stck;
            });
        };
        $scope.isOptionsRequiredST = function () {
            if ($scope.optionflag === 'Store') {
                return !$scope.get_ckoutdetails.some(function (options) {
                    return options.stck;
                });
            } else {
                return false;
            }
        };
        $scope.isOptionsRequiredE = function () {
            if ($scope.optionflag === 'Employee') {
                return !$scope.get_ckoutdetails.some(function (options) {
                    return options.HRME;
                });
            } else {
                return false;
            }
        };
         $scope.isOptionsRequiredsup = function () {
            if ($scope.optionflag === 'Supplier') {
                return !$scope.get_ckoutdetails.some(function (options) {
                    return options.sup;
                });
            } else {
                return false;
            }
        };

        $scope.all_checkST = function (stri) {
            $scope.stall = stri;
            var toggleStatus = $scope.stall;
            angular.forEach($scope.get_ckoutdetails, function (st) {
                st.stck = toggleStatus;
            });
        };
        //======================== Location
        $scope.togchkbxlo = function () {
            $scope.loall = $scope.get_ckoutdetails.every(function (loc) {
                return loc.lock;
            });
        };
        $scope.togchkbxem = function () {
            $scope.loallem = $scope.get_ckoutdetails.every(function (loc) {
                return loc.HRME;
            });
        };
        $scope.togchkbxsup = function () {
            $scope.loallsup = $scope.get_ckoutdetails.every(function (loc) {
                return loc.sup;
            });
        };
        $scope.isOptionsRequiredlo = function () {
            if ($scope.optionflag === 'Location') {
                return !$scope.get_ckoutdetails.some(function (options) {
                    return options.lock;
                });
            } else {
                return false;
            }
        };
        $scope.all_checklo = function (loct) {
            $scope.loall = loct;
            var toggleStatus = $scope.loall;
            angular.forEach($scope.get_ckoutdetails, function (loca) {
                loca.lock = toggleStatus;
            });
        };
        $scope.all_checkem = function (loct) {
            $scope.loallem = loct;
            var toggleStatus = $scope.loallem;
            angular.forEach($scope.get_ckoutdetails, function (loca) {
                loca.HRME = toggleStatus;
            });
        };
         $scope.all_checksup = function (loct) {
            $scope.loallsup = loct;
            var toggleStatus = $scope.loallsup;
            angular.forEach($scope.get_ckoutdetails, function (loca) {
                loca.sup = toggleStatus;
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

                $scope.coItemArray = [];
                $scope.coStoreArray = [];
                $scope.coLocationArray = [];
                $scope.employeeArray = [];
                $scope.coTagArray = [];
                $scope.get_mi_arraylist = [];
                if ($scope.staffcheck == 'Staff') {
                    $scope.get_mi_arraylist.push({ MI_Id: $scope.MI_Id })
                }
                else
                {
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
                    angular.forEach($scope.get_ckoutdetails, function (itm) {
                        if (itm.itemck === true) {
                            $scope.coItemArray.push(itm);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "coItemArray": $scope.coItemArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist
                    };
                }
                else if ($scope.optionflag === "Store") {
                    angular.forEach($scope.get_ckoutdetails, function (st) {
                        if (st.stck === true) {
                            $scope.coStoreArray.push(st);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "coStoreArray": $scope.coStoreArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist
                    };
                }
                else if ($scope.optionflag === "Location") {
                    angular.forEach($scope.get_ckoutdetails, function (lo) {
                        if (lo.lock === true) {
                            $scope.coLocationArray.push(lo);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "coLocationArray": $scope.coLocationArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist
                    };
                }
                else if ($scope.optionflag === "Tag") {
                    angular.forEach($scope.get_ckoutdetails, function (tg) {
                        if (tg.tgck === true) {
                            $scope.coTagArray.push(tg);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "coTagArray": $scope.coTagArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist
                    };
                }

                else if ($scope.optionflag === "Employee") {
                    angular.forEach($scope.get_ckoutdetails, function (tg) {
                        if (tg.HRME === true) {
                            $scope.employeeArray.push(tg);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "employeeArray": $scope.employeeArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist
                    };
                }

                apiService.create("AssetTagCheckout_Report/onreport", data).
                    then(function (promise) {
                        //if (promise.get_ckoutreport.length > 0) {
                        //    $scope.get_ckoutreport = promise.get_ckoutreport;
                        //    $scope.presentCountgrid = $scope.get_ckoutreport.length;
                        //}
                        //else {
                        //    swal("No Record Found...!!");
                        //    $scope.get_ckoutreport = "";
                        //}
                        $scope.get_ckoutreport = true;
                        $scope.tempaggary = [];


                        $scope.insarray = [{ name: "MI_Name" }, { name: "INVMS_StoreName" }, { name: "INVMI_ItemName" }, { name: "INVAAT_AssetId" }, { name: "INVAAT_ModelNo" }, { name: "INVAAT_SerialNo" }, { name: "INVMLO_LocationRoomName" }, { name: "INVATCO_CheckoutDate" }, { name: "INVATCO_ReceivedBy" }, { name: "INVATCO_CheckOutRemarks" }];

                        $scope.columns1 = [{ field: "MI_Name", name: "MI_Name", title: "Company Name" }, { field: "INVMS_StoreName", name: "INVMS_StoreName", title: "Store" }, { field: "INVMI_ItemName", name: "INVMI_ItemName", title: "Item Name" }, { field: "INVAAT_AssetId", name: "INVAAT_AssetId", title: "Tag" }, { field: "INVAAT_ModelNo", name: "INVAAT_ModelNo", title: "Model No." }, { field: "INVAAT_SerialNo", name: "INVAAT_SerialNo", title: "Serial No." }, { field: "INVMLO_LocationRoomName", name: "INVMLO_LocationRoomName", title: "Location" }, { field: "INVATCO_CheckoutDate", name: "INVATCO_CheckoutDate", title: "Date" }, { field: "INVATCO_ReceivedBy", name: "INVATCO_ReceivedBy", title: "ReceivedBy" }, {
                            field: "INVATCO_CheckOutRemarks", name: "INVATCO_CheckOutRemarks", title: "Remarks"
                        }];
                        $scope.newarray = [];

                        $scope.newarray1 = $scope.newarray;
                        $scope.data = promise.get_ckoutreport;
                        if (promise.get_ckoutreport.length > 0) {
                            angular.forEach($scope.get_ckoutreport, function (po) {
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
                                name: 'MI_Name', field: 'MI_Name', title: 'Company Name', width: 150
                            },
                            {
                                name: 'INVMS_StoreName', field: 'INVMS_StoreName', title: 'Store', width: 150
                            },

                            {
                                name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item Name', width: 150
                            },
                            {
                                name: 'INVAAT_AssetId', field: 'INVAAT_AssetId', title: 'Tag', width: 150
                            },

                            {
                                name: 'INVAAT_ModelNo', field: 'INVAAT_ModelNo', title: 'Model No.', width: 200
                            },
                            {
                                name: 'INVAAT_SerialNo', field: 'INVAAT_SerialNo', title: 'Serial No.', width: 200
                            },
                            {
                                name: 'INVMLO_LocationRoomName', field: 'INVMLO_LocationRoomName', title: 'Location', width: 250
                            },

                            {
                                name: 'INVATCO_CheckoutDate', field: 'INVATCO_CheckoutDate', title: 'Date', width: 100, template: "#= kendo.toString(kendo.parseDate(INVATCO_CheckoutDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                            },
                            {
                                name: 'INVATCO_ReceivedBy', field: 'INVATCO_ReceivedBy', title: 'ReceivedBy', width: 150
                            },
                            {
                                name: 'INVATCO_CheckOutRemarks', field: 'INVATCO_CheckOutRemarks', title: 'Remarks', width: 100
                            }
                            ];

                            angular.forEach($scope.header_list, function (obj) {
                                $scope.colarrayall.push(obj);
                            })

                            console.log($scope.header_list);

                            console.log($scope.colarrayall);

                            //$scope.txtdata = "TIME :" + " " + $filter('date')(new Date(), 'HH:mm:ss') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname;

                            $scope.txtdata = "Asset Tag Checkout Report";
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
                                        fileName: "CheckoutReport.xlsx",
                                        proxyURL: "",
                                        filterable: true,
                                        allPages: true
                                    },
                                    pdf: {
                                        avoidLinks: true,
                                        landscape: true,
                                        repeatHeaders: true,
                                        fileName: "CheckoutReport.pdf",
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
        $scope.exportToExcel = function (export_id) {

            var exportHref = Excel.tableToExcel(export_id, 'sheet name');
            $timeout(function () {
                location.href = exportHref;
            }, 100);

        };
        $scope.printData = function () {

            var innerContents = document.getElementById("printagco").innerHTML;
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