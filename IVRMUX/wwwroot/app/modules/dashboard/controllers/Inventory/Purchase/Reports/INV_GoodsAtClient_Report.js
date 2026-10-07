
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_GoodsAtClient_ReportController', INV_GoodsAtClient_ReportController);
    INV_GoodsAtClient_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter']
    function INV_GoodsAtClient_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {


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


        //==============================
        $scope.all_miid = function (ac) {
            $scope.get_GoodsAtCldetails = [];
            $scope.allmi = ac;
            var toggleStatus = $scope.allmi;
            angular.forEach($scope.get_mi_list, function (itm) {
                itm.miid = toggleStatus;
            });
            if ($scope.allmi == true) {
                $scope.loaddata($scope.flag, $scope.get_mi_arraylist1);
            }
        };

        $scope.mi_click = function () {
            $scope.get_GoodsAtCldetails = [];
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
                "get_mi_arraylist": $scope.get_mi_arraylist1
            };
            apiService.create("INV_GoodsAtClient_Report/getloaddata", data).
                then(function (promise) {
                    $scope.staffcheck = promise.staffcheck;
                    if ($scope.staffcheck == 'Staff') {
                        $scope.MI_Name = promise.get_mi_list[0].MI_Name;
                        $scope.MI_Id = promise.get_mi_list[0].MI_Id;
                        angular.forEach($scope.get_mi_list, function (a) {
                            a.miid = true;
                        })
                        $scope.get_GoodsAtCldetails = promise.get_GoodsAtCldetails;

                    }
                    else {
                        $scope.get_GoodsAtCldetails = promise.get_GoodsAtCldetails;
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
            $scope.get_GoodsAtCldetails = "";
            $scope.get_GoodsATClreport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag, $scope.get_mi_arraylist1);
          
        };
        $scope.togchkbxA = function () {
            $scope.Assetall = $scope.get_GoodsAtCldetails.every(function (itm) {
                return itm.Assetck;
            });
        };
        $scope.isOptionsRequiredA = function () {
            if ($scope.optionflag === 'AssetId-Wise') {
                return !$scope.get_GoodsAtCldetails.some(function (options) {
                    return options.Assetck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkA = function (itmi) {
            $scope.Assetall = itmi;
            var toggleStatus = $scope.Assetall;
            angular.forEach($scope.get_GoodsAtCldetails, function (itm) {
                itm.Assetck = toggleStatus;
            });
        };
        //========================ITEM
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.get_GoodsAtCldetails.every(function (itm) {
                return itm.itemck;
            });
        };
        $scope.togchkbxST = function () {
            $scope.stall = $scope.get_GoodsAtCldetails.every(function (itm) {
                return itm.stck;
            });
        };
       
        $scope.isOptionsRequiredI = function () {
            if ($scope.optionflag === 'Item-Wise') {
                return !$scope.get_GoodsAtCldetails.some(function (options) {
                    return options.itemck;
                });
            } else {
                return false;
            }
        };
    
        $scope.isOptionsRequiredst = function () {
            if ($scope.optionflag === 'Store') {
                return !$scope.get_GoodsAtCldetails.some(function (options) {
                    return options.stck;
                });
            } else {
                return false;
            }
        };
      
        $scope.all_checkI = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.get_GoodsAtCldetails, function (itm) {
                itm.itemck = toggleStatus;
            });
        };
        $scope.all_checkST = function (itmi) {
            $scope.stall = itmi;
            var toggleStatus = $scope.stall;
            angular.forEach($scope.get_GoodsAtCldetails, function (itm) {
                itm.stck = toggleStatus;
            });
        };

        //==================================PO Report
        $scope.submitted = false;
        $scope.onreport = function () {

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
                if ($scope.accept == undefined) {
                    $scope.accept = 0;
                }
                if ($scope.reject == undefined) {
                    $scope.reject = 0;
                }
                if ($scope.pending == undefined) {
                    $scope.pending = 0;
                }
                $scope.AssetIDArray = [];
          
                $scope.GoodsitemArray = [];
                $scope.GoodsStoreArray = [];
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
                        "get_mi_arraylist": $scope.get_mi_arraylist,
                    
                    };
                }

                else if ($scope.optionflag === "AssetId-Wise") {
                    angular.forEach($scope.get_GoodsAtCldetails, function (Asset) {
                        if (Asset.Assetck === true) {
                            $scope.AssetIDArray.push(Asset);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "AssetIDArray": $scope.AssetIDArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,

                    };
                }
            
                else if ($scope.optionflag === "Item-Wise") {
                    angular.forEach($scope.get_GoodsAtCldetails, function (itm) {
                        if (itm.itemck === true) {
                            $scope.GoodsitemArray.push(itm);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "GoodsitemArray": $scope.GoodsitemArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,
                      
                    };
                }
                else if ($scope.optionflag === "Store") {
                    angular.forEach($scope.get_GoodsAtCldetails, function (st) {
                        if (st.stck === true) {
                            $scope.GoodsStoreArray.push(st);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "GoodsStoreArray": $scope.GoodsStoreArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,
                       
                    };
                }
                apiService.create("INV_GoodsAtClient_Report/onreport", data).
                    then(function (promise) {


                      
                        $scope.get_GoodsATClreport = true;
                        $scope.tempaggary = [];
                        var count = 0;
                        $scope.insarray = [{ name: "INVMS_StoreName" },
                            { name: "INVMI_ItemName" },
                            { name: "INVAAT_AssetId" },
                            { name: "INVMGRN_InvoiceNo" },
                            { name: "INVMLO_LocationRoomName" },
                            { name: "INVACO_CheckoutDate" },
                            { name: "INVACO_CheckOutQty" },
                            { name: "INVTGRN_PurchaseRate" },
                            { name: "INVTGRN_Amount" },
                            { name: "INVACO_ReceivedBy" }];
                        $scope.columns1 = [{ field: "INVMS_StoreName", name: "INVMS_StoreName", title: "From store" },
                            { field: "INVMI_ItemName", name: "INVMI_ItemName", title: "Item " },
                            { field: "INVAAT_AssetId", name: "INVAAT_AssetId", title: "AssetID" },
                            { field: "INVMGRN_InvoiceNo", name: "INVMGRN_InvoiceNo", title: "Supplier Invoice No" },
                            { field: "INVMLO_LocationRoomName", name: "INVMLO_LocationRoomName", title: "To Location" },
                            { field: "INVACO_CheckoutDate", name: "INVACO_CheckoutDate", title: "Check-out Date", width: 150, template: "#= kendo.toString(kendo.parseDate(INVACO_CheckoutDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #" },
                            { field: "INVACO_CheckOutQty", name: "INVACO_CheckOutQty", title: "Check-out Qty" },
                            { field: "INVTGRN_PurchaseRate", name: "INVTGRN_PurchaseRate", title: "Rate" },
                            { field: "INVTGRN_Amount", name: "INVTGRN_Amount", title: "Amount", aggregates: ["sum"], footerTemplate: "Sum: #=sum#", groupFooterTemplate: "Sum: #=sum#" },
                            { field: "INVACO_ReceivedBy", name: "INVACO_ReceivedBy", title: "Received By" }
                        ];
                        $scope.newarray = [];
                        $scope.newarray1 = $scope.newarray;
                        
                        $scope.data = promise.get_GoodsATClreport;
                        for (var i in $scope.data) {
                            $scope.data[i].rowNumber = count + 1;
                            $scope.data[i].INVACO_CheckoutDate = $filter('date')($scope.data[i].INVACO_CheckoutDate, "yyyy-MM-dd");
                         
                            count = count + 1;
                        }
                        if (promise.get_GoodsATClreport.length > 0) {
                            var totalrate = 0.00;
                            var totaltax = 0.00;
                            var totalamt = 0.00;                         
                          

                            angular.forEach($scope.get_GoodsATClreport, function (po) {
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
                                    name: 'INVMS_StoreName', field: 'INVMS_StoreName', title: 'From Store', width: 150
                                    },
                                    {
                                        name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item', width: 150
                                    },
                                    {
                                        name: 'INVAAT_AssetId', field: 'INVAAT_AssetId', title: 'AssetID', width: 150
                                    },
                                    {
                                        name: 'INVMGRN_InvoiceNo', field: 'INVMGRN_InvoiceNo', title: 'Supplier Invoice No', width: 150
                                    },
                                    {
                                        name: 'INVMLO_LocationRoomName', field: 'INVMLO_LocationRoomName', title: 'To Location', width: 150
                                    },
                                   


                                {
                                    name: 'INVACO_CheckoutDate', field: 'INVACO_CheckoutDate', title: 'Check-out Date', width: 150, template: "#= kendo.toString(kendo.parseDate(INVACO_CheckoutDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                                },
                                  {
                                      name: 'INVACO_CheckOutQty', field: 'INVACO_CheckOutQty', title: 'Check-out Qty', width: 150
                                    },
                                    {
                                        name: 'INVTGRN_PurchaseRate', field: 'INVTGRN_PurchaseRate', title: 'Rate', width: 250, footerTemplate: "Total:",
                                        groupFooterTemplate: "Total: "
                                    },
                               
                                    
                                    {
                                        name: 'INVTGRN_Amount', field: 'INVTGRN_Amount', title: 'Amount', width: 160, format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"

                                    },
                                    {
                                        name: 'INVACO_ReceivedBy', field: 'INVACO_ReceivedBy', title: 'Received By', width: 150
                                    }
                                   
                              
                                  
                               

                            ];
                          
                           
                                $scope.tempaggary.push({ field: 'INVTGRN_Amount', name: 'INVTGRN_Amount', format: "{0:n2}", aggregate: "sum" });

                            ///function (e) { return e.userDetails.UserTypeId == 1 ? false : true; }
                                angular.forEach($scope.header_list, function (obj) {
                                    $scope.colarrayall.push(obj);
                            })
                           
                          

                                console.log($scope.header_list);

                            console.log($scope.colarrayall);

                            //$scope.txtdata = "TIME :" + " " + $filter('date')(new Date(), 'HH:mm:ss') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname;
                            $scope.txtdata = "Goods At Client Report";
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
                                            fileName: "GoodsAtClientReport.xlsx",
                                            proxyURL: "",
                                            filterable: true,
                                            allPages: true
                                             
                                        },
                                    

                                        pdf: {
                                            paperSize: "A4",
                                            scale: 0.35,
                                            margin: {
                                                top: "2cm",
                                                left: "1cm",
                                                right: "1cm",
                                                bottom: "1cm"
                                            },
                                            avoidLinks: true,
                                            landscape: true,
                                            repeatHeaders: true,
                                            fileName: "GoodsAtClientReport.pdf",
                                            allPages: true
                                            
                                        },
                                        dataSource: {

                                            //type: "odata",
                                            //transport: {
                                            //    read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Products"
                                            //},
                                            data: $scope.data,
                                            aggregate: $scope.tempaggary,
                                             pageSize: 20
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
                        else {
                            swal("No Record Found...!!");
                            $scope.get_GoodsATClreport = "";
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
        };
        //======================================Print & Export to Excel
        //$scope.exportToExcel = function (export_id) {

        //    var exportHref = Excel.tableToExcel(export_id, 'printpr');
        //    $timeout(function () {
        //        location.href = exportHref;
        //    }, 100);
        //};
        $scope.exportToExcel = function (export_id) {

            var exportHref = Excel.tableToExcel(export_id, 'sheet name');
            $timeout(function () {
                location.href = exportHref;
            }, 100);

        };
        $scope.exportToPdf = function (export_id) {
            var exportHref = Excel.tableTopdf(export_id, 'sheet name');
            $timeout(function () {
                location.href = exportHref;
            }, 100);

        };
        $scope.printData = function () {

            var innerContents = document.getElementById("printInward").innerHTML;
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