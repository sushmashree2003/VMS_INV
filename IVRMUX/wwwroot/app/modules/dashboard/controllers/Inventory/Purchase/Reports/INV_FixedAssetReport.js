
(function () {
    'use strict';
    angular
        .module('app')
        .controller('Fixed_Asset_ReportController', Fixed_Asset_ReportController);
    Fixed_Asset_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter']
    function Fixed_Asset_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {


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
            $scope.get_Fixedassetdetails = [];
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

            $scope.get_Fixedassetdetails = [];

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
            apiService.create("Fixed_Asset_Report/getloaddata", data).
                then(function (promise) {
                    $scope.staffcheck = promise.staffcheck;
                    if ($scope.staffcheck == 'Staff') {
                        $scope.MI_Name = promise.get_mi_list[0].MI_Name;
                        $scope.MI_Id = promise.get_mi_list[0].MI_Id;
                        angular.forEach($scope.get_mi_list, function (a) {
                            a.miid = true;
                        })
                        $scope.get_Fixedassetdetails = promise.get_Fixedassetdetails;

                    }
                    else {
                        $scope.get_Fixedassetdetails = promise.get_Fixedassetdetails;
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
            $scope.get_Fixedassetdetails = "";
            $scope.get_fixedassetreport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag, $scope.get_mi_arraylist1);
        };
       
        //========================AssetID
        $scope.togchkbxA = function () {
            $scope.Assetall = $scope.get_Fixedassetdetails.every(function (itm) {
                return itm.Assetck;
            });
        };
        $scope.togchkbxSP = function () {
            $scope.spall = $scope.get_Fixedassetdetails.every(function (itm) {
                return itm.spck;
            });
        };
        $scope.isOptionsRequiredA = function () {
            if ($scope.optionflag === 'AssetId-Wise') {
                return !$scope.get_Fixedassetdetails.some(function (options) {
                    return options.Assetck;
                });
            } else {
                return false;
            }
        };
        $scope.isOptionsRequiredSP = function () {
            if ($scope.optionflag === 'Supplier') {
                return !$scope.get_Fixedassetdetails.some(function (options) {
                    return options.spck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkA = function (itmi) {
            $scope.Assetall = itmi;
            var toggleStatus = $scope.Assetall;
            angular.forEach($scope.get_Fixedassetdetails, function (itm) {
                itm.Assetck = toggleStatus;
            });
        };
        $scope.all_checkSP = function (itmi) {
            $scope.spall = itmi;
            var toggleStatus = $scope.spall;
            angular.forEach($scope.get_Fixedassetdetails, function (itm) {
                itm.spck = toggleStatus;
            });
        };

        //==================================Fixed Asset Report
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
              

                $scope.grnoArray = [];
                $scope.groupArray = [];
                $scope.grnsuplierArray = [];
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
                    angular.forEach($scope.get_Fixedassetdetails, function (Asset) {
                        if (Asset.Assetck === true) {
                            $scope.grnoArray.push(Asset);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "grnoArray": $scope.grnoArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,

                    };
                }
                else if ($scope.optionflag === "Supplier") {
                    angular.forEach($scope.get_Fixedassetdetails, function (sp) {
                        if (sp.spck === true) {
                            $scope.grnsuplierArray.push(sp);
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "grnsuplierArray": $scope.grnsuplierArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,

                    };
                }
                //
                else if ($scope.optionflag === "Group") {
                    angular.forEach($scope.get_Fixedassetdetails, function (sp) {
                        if (sp.spck === true) {
                            $scope.groupArray.push({
                                INVMS_Id: sp.INVMG_Id
                            });
                        }
                    });
                    data = {
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "grnsuplierArray": $scope.groupArray,
                        "bwdateflag": $scope.bw_dates,
                        "get_mi_arraylist": $scope.get_mi_arraylist,

                    };
                }
                apiService.create("Fixed_Asset_Report/onreport", data).
                    then(function (promise) {
                        var count = 0;                                             
                        $scope.get_fixedassetreport = true;
                        $scope.tempaggary = [];
                        $scope.insarray = [{ name: "INVMG_GroupName" },
                            { name: "INVAAT_AssetDescription" },
                            { name: "INVMGRN_PurchaseDate" },
                            { name: "INVMGRN_InvoiceNo" },
                            { name: "INVMS_SupplierName" },
                            { name: "INVMGRN_GRNNo" },
                            { name: "GRNDate" },
                            { name: "INVMI_HSNCode" },
                            { name: "INVAAT_AssetId" },
                            { name: "INVMLO_LocationRoomName" },
                            { name: "INVTGRN_PurchaseRate" },
                            { name: "INVTGRN_TaxAmt" },
                            { name: "INVT_Otherchrges" },
                            { name: "INVTGRN_Amount" }];

                        $scope.columns1 = [{ field: "INVMG_GroupName", name: "INVMG_GroupName", title: "Group Name" },
                            { field: "INVAAT_AssetDescription", name: "INVAAT_AssetDescription", title: "Asset Description" },
                            { field: "INVMGRN_PurchaseDate", name: "INVMGRN_PurchaseDate", title: "Date of supplier Invoice" },
                            { field: "INVMGRN_InvoiceNo", name: "INVMGRN_InvoiceNo", title: "Supplier Invoice No" },
                            { field: "INVMS_SupplierName", name: "INVMS_SupplierName", title: "Supplier Name" },
                            { field: "INVMGRN_GRNNo", name: "INVMGRN_GRNNo", title: "GRN No" },
                            { field: "INVMGRN_PurchaseDate", name: "INVMGRN_PurchaseDate", title: "GRN Date" },
                            { field: "INVMI_HSNCode", name: "INVMI_HSNCode", title: "HSN Code" },
                            { field: "INVAAT_AssetId", name: "INVAAT_AssetId", title: "Asset Id" },
                            { field: "INVMLO_LocationRoomName", name: "INVMLO_LocationRoomName", title: "Asset Location" },
                            { field: "INVTGRN_PurchaseRate", name: "INVTGRN_PurchaseRate", title: "Basic Price", aggregates: ["sum"], footerTemplate: "Sum: #=sum#", groupFooterTemplate: "Sum: #=sum#" },
                            { field: "INVTGRN_TaxAmt", name: "INVTGRN_TaxAmt", title: "GST Price", aggregates: ["sum"], footerTemplate: "Sum: #=sum#", groupFooterTemplate: "Sum: #=sum#" },
                            { field: "INVT_Otherchrges", name: "INVT_Otherchrges", title: "Other Chargers"},
                            {
                            field: "INVTGRN_Amount", name: "INVTGRN_Amount", title: "Total value", aggregates: ["sum"], footerTemplate: "Sum: #=sum#", groupFooterTemplate: "Sum: #=sum#"
                        }
                        ];
                        $scope.newarray = [];
                        $scope.newarray1 = $scope.newarray;
                        
                        $scope.data = promise.get_fixedassetreport;
                        for (var i in $scope.data) {
                            $scope.data[i].rowNumber = count + 1;
                            $scope.data[i].INVMGRN_PurchaseDate = $filter('date')($scope.data[i].INVMGRN_PurchaseDate, "yyyy-MM-dd");
                            count = count + 1;
                        }
                        if (promise.get_fixedassetreport.length > 0) {
                            var totalrate = 0.00;
                            var totaltax = 0.00;
                            var totalamt = 0.00;                         
                            //$scope.get_fixedassetreport = promise.get_fixedassetreport;
                            //$scope.presentCountgrid = $scope.get_fixedassetreport.length;

                            angular.forEach($scope.get_fixedassetreport, function (po) {
                              
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
                                name: 'INVMG_GroupName', field: 'INVMG_GroupName', title: 'Group Name', width: 150

                           },
                           {
                                    name: 'INVAAT_AssetDescription', field: 'INVAAT_AssetDescription', title: 'Asset Description', width: 150
                            },
                            {
                                name: 'INVMGRN_PurchaseDate', field: 'INVMGRN_PurchaseDate', title: 'Date of Supplier Invoice', width: 150, template: "#= kendo.toString(kendo.parseDate(INVMGRN_PurchaseDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                            },
                            
                            {
                                name: 'INVMGRN_InvoiceNo', field: 'INVMGRN_InvoiceNo', title: 'Supplier InvoiceNo', width: 200
                            },
                            {
                                name: 'INVMS_SupplierName', field: 'INVMS_SupplierName', title: 'Supplier Name', width: 250
                            },
                            
                            {
                                name: 'INVMGRN_GRNNo', field: 'INVMGRN_GRNNo', title: 'GRN No', width: 200

                            },
                            {
                                name: 'INVMGRN_PurchaseDate', field: 'INVMGRN_PurchaseDate', title: 'GRN date', width: 150, template: "#= kendo.toString(kendo.parseDate(INVMGRN_PurchaseDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                             },
                            {
                                name: 'INVMI_HSNCode', field: 'INVMI_HSNCode', title: 'HSN Code', width: 200
                            },

                            {
                                name: 'INVAAT_AssetId', field: 'INVAAT_AssetId', title: 'Asset Id', width: 100
                            },
                            {
                                name: 'INVMLO_LocationRoomName', field: 'INVMLO_LocationRoomName', title: 'Location', width: 200
                            },
                            
                            {
                                name: 'INVTGRN_PurchaseRate', field: 'INVTGRN_PurchaseRate', title: 'Basic Price', width: 100, format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                            },
                            {
                                name: 'INVTGRN_TaxAmt', field: 'INVTGRN_TaxAmt', title: 'GST price', width: 160, format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                            },
                            {
                                name: 'INVT_Otherchrges', field: 'INVT_Otherchrges', title: 'Other Chargers', width: 150
                            },
                            {
                                name: 'INVTGRN_Amount', field: 'INVTGRN_Amount', title: 'Total Amount', width: 160, format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"

                            }



                            ];
                          
                            //$scope.tempaggary.push({ field: 'INVTGRN_PurchaseRate', name: 'INVTGRN_PurchaseRate', format: "{0:n2}", aggregate: "sum" });
                          //  $scope.tempaggary.push({ field: 'INVTGRN_TaxAmt', name: 'INVTGRN_TaxAmt', format: "{0:n2}", aggregate: "sum" });
                           // $scope.tempaggary.push({ field: 'INVT_Otherchrges', name: 'INVT_Otherchrges', format: "{0:n2}", aggregate: "sum" });

                            $scope.tempaggary.push(
                                {
                                    field: 'INVTGRN_PurchaseRate', name: 'INVTGRN_PurchaseRate', format: "{0:n2}", aggregate: "sum"
                                },
                                {
                                    field: 'INVTGRN_TaxAmt', name: 'INVTGRN_TaxAmt', format: "{0:n2}", aggregate: "sum"
                                },
                                //{
                                //    field: 'INVT_Otherchrges', name: 'INVT_Otherchrges', format: "{0:n2}", aggregate: "sum"
                                //},

                                {
                                    field: 'INVTGRN_Amount', name: 'INVTGRN_Amount', format: "{0:n2}", aggregate: "sum",
                                
                                }
                            );

                            ///function (e) { return e.userDetails.UserTypeId == 1 ? false : true; }
                            angular.forEach($scope.header_list, function (obj) {
                                $scope.colarrayall.push(obj);
                            })
                            
                            console.log($scope.header_list);
                            console.log($scope.colarrayall);

                            //$scope.txtdata = "TIME :" + " " + $filter('date')(new Date(), 'HH:mm:ss') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname;

                            $scope.txtdata = "Fixed Asset Report";

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
                                        fileName: "FixedAssetReport.xlsx",
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
                                            fileName: "FixedAssetReport.pdf",
                                            allPages: true
                                    },
                                    dataSource: {

                                        //type: "odata",
                                        //transport: {
                                        //    read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Products"
                                        //},
                                        data: $scope.data,
                                        pageSize: 20,
                                        aggregate: $scope.tempaggary

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
                                //gridall.setOptions({
                                //    sortable: true
                                //});
                            }


                        }
                        else {
                            swal("No Record Found...!!");
                            $scope.get_fixedassetreport = "";
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
            var exportHref = Excel.tableTopdf(export_id, 'printpo');
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