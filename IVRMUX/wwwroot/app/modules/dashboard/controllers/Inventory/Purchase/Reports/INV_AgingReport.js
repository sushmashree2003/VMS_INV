
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_Aging_ReportController', INV_Aging_ReportController);
    INV_Aging_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter']
    function INV_Aging_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {
        
        $scope.obj = {};
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        $scope.companyname = localStorage.getItem('MIName');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        $scope.coptyright = copty;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
       

     

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
            apiService.create("INV_Aging_Report/getloaddata", data).
                then(function (promise) {
                    $scope.staffcheck = promise.staffcheck;
                    if ($scope.staffcheck == 'Staff') {
                        $scope.MI_Name = promise.get_mi_list[0].MI_Name;
                        $scope.MI_Id = promise.get_mi_list[0].MI_Id;
                        angular.forEach($scope.get_mi_list, function (a) {
                            a.miid = true;
                        })
                        $scope.get_Agingdetails = promise.get_Agingdetails;

                    }
                    else {
                        $scope.get_Agingdetails = promise.get_Agingdetails;
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
            $scope.get_Agingdetails = "";
            $scope.get_Agingreport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag, $scope.get_mi_arraylist1);
        };
       
        //========================ITEM
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.get_Agingdetails.every(function (itm) {
                return itm.itemck;
            });
        };

       

        $scope.isOptionsRequiredI = function () {
            if ($scope.optionflag === 'Item-wise') {
                return !$scope.get_Agingdetails.some(function (options) {
                    return options.itemck;
                });
            } else {
                return false;
            }
        };
       
        $scope.all_checkI = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.get_Agingdetails, function (itm) {
                itm.itemck = toggleStatus;
            });
        };
      
        $scope.all_miid = function (ac) {
            $scope.get_Agingdetails = [];
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
            $scope.get_Agingdetails = [];
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

        //==================================Aging Report
        $scope.submitted = false;
        $scope.onreport = function ()
        {

            $("#grid123").kendoGrid();
            $('#grid123').data('kendoGrid').dataSource.read();
            $('#grid123').data('kendoGrid').refresh();

            if ($scope.myForm.$valid) {
                var data = {};
                //$scope.start_Date = $filter('date')($scope.startdate, "yyyy-MM-dd");
                $scope.end_Date = $filter('date')($scope.enddate, "yyyy-MM-dd");
                //if ($scope.bw_dates === true) {
                    //$scope.sDate = $scope.start_Date;
                    $scope.eDate = $scope.end_Date;
                //}
                //else {
                //    //$scope.sDate = "";
                //    $scope.eDate = "";
                //}
              

                $scope.grnoArray = [];
                $scope.poitemArray = [];
                $scope.posuplierArray = [];
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
                      
                        "enddate": $scope.eDate,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,

                    };
                }
               
                else if ($scope.optionflag === "Item-wise") {
                    angular.forEach($scope.get_Agingdetails, function (itm) {
                        if (itm.itemck === true) {
                            $scope.poitemArray.push(itm);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        //"startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "poitemArray": $scope.poitemArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,

                    };
                }
              
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("INV_Aging_Report/onreport", data).
                    then(function (promise) {
                        $scope.data = [];
                        if (promise.get_Agingreport != null && promise.get_Agingreport.length > 0) {
                            var count = 0;
                            $scope.get_Agingreport = true;
                            $scope.tempaggary = [];

                            $scope.insarray = [{ name: "INVMI_Itemcode" },
                            { name: "INVMI_ItemName" },
                            { name: "INVMI_HSNCode" },
                            { name: "INVTGRN_Qty" },
                            { name: "INVOB_PurchaseDate" },
                            { name: "LessThen90days" },
                            { name: "LessThen180days" },
                            { name: "LessThen360days" },
                            { name: "MoreThen360days" },
                            { name: "INVMGRN_InvoiceNo" },
                            { name: "INVMGRN_PurchaseDate" },
                            { name: "INVMGRN_Remarks" }];

                            $scope.columns1 = [{ field: "INVMI_Itemcode", name: "INVMI_Itemcode", title: "Item Code" },
                            { field: "INVMI_ItemName", name: "INVMI_ItemName", title: "Item Description" },
                            { field: "INVMI_HSNCode", name: "INVMI_HSNCode", title: "HSN Code" },
                            { field: "INVTGRN_Qty", name: "INVTGRN_Qty", title: "Qty" },
                            { field: "INVOB_PurchaseDate", name: "INVOB_PurchaseDate", title: "Date of Inward" },
                            { field: "LessThen90days", name: "LessThen90days", title: "Less Then 90 days" },
                            { field: "LessThen180days", name: "LessThen180days", title: "Less Then 180 days" },
                            { field: "LessThen360days", name: "LessThen360days", title: "Less Then 360 days" },
                            { field: "MoreThen360days", name: "MoreThen360days", title: "More Then 360 days" },
                            { field: "INVMGRN_InvoiceNo", name: "INVMGRN_InvoiceNo", title: "Supplier InvoiceNo" },
                            { field: "INVMGRN_PurchaseDate", name: "INVMGRN_PurchaseDate", title: "Supplier Invoice Date" },
                            { field: "INVMGRN_Remarks", name: "INVMGRN_Remarks", title: "Remarks" }];
                            $scope.newarray = [];

                            $scope.newarray1 = $scope.newarray;
                            $scope.data = promise.get_Agingreport;
                            for (var i in $scope.data) {
                                $scope.data[i].rowNumber = count + 1;
                                $scope.data[i].INVOB_PurchaseDate = $filter('date')($scope.data[i].INVOB_PurchaseDate, "yyyy-MM-dd");
                                count = count + 1;
                            }
                            if (promise.get_Agingreport.length > 0) {
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
                                    name: 'INVMI_Itemcode', field: 'INVMI_Itemcode', title: 'Item Code', width: 150
                                },
                                {
                                    name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item Description', width: 250
                                },
                                {
                                    name: 'INVMI_HSNCode', field: 'INVMI_HSNCode', title: 'HSN Code', width: 100
                                },

                                {
                                    name: 'INVTGRN_Qty', field: 'INVTGRN_Qty', title: 'Qty', width: 150
                                },

                                {
                                    name: 'INVOB_PurchaseDate', field: 'INVOB_PurchaseDate', title: 'Date of Inward', width: 150

                                },
                                {
                                    name: 'LessThen90days', field: 'LessThen90days', title: 'Less Then 90 days', width: 200
                                },

                                {
                                    name: 'LessThen180days', field: 'LessThen180days', title: 'Less Then 180 days', width: 200
                                },
                                {
                                    name: 'LessThen360days', field: 'LessThen360days', title: 'Less Then 360 days', width: 200
                                },
                                {
                                    name: 'MoreThen360days', field: 'MoreThen360days', title: 'More Then 360 days', width: 200
                                },

                                {
                                    name: 'INVMGRN_InvoiceNo', field: 'INVMGRN_InvoiceNo', title: 'Supplier InvoiceNo', width: 200
                                },

                                {
                                    name: 'INVMGRN_PurchaseDate', field: 'INVMGRN_PurchaseDate', title: 'Supplier Invoice Date', width: 200
                                },


                                {
                                    name: 'INVMGRN_Remarks', field: 'INVMGRN_Remarks', title: 'Remarks', width: 100
                                }


                                ];


                                angular.forEach($scope.header_list, function (obj) {
                                    $scope.colarrayall.push(obj);
                                })

                                console.log($scope.header_list);

                                console.log($scope.colarrayall);

                                //$scope.txtdata = "TIME :" + " " + $filter('date')(new Date(), 'HH:mm:ss') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname;
                                $scope.txtdata = "Aging Report";
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
                                            fileName: "AgingReport.xlsx",
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
                                            fileName: "AgingReport.pdf",
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

                        }
                        else {
                            swal("Record Not Found");
                        }
                     
                     
                    })
            }
            else {
                swal("No Records Found");
                $scope.submitted = true;
                //$scope.get_Agingreport = "";
            }    
                        //   $scope.mainsubhdr = [];
                       
        }

        
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

            var innerContents = document.getElementById("printpo").innerHTML;
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