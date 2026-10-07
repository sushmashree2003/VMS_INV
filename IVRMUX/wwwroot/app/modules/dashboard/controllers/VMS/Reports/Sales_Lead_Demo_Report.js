(function () {
    'use strict';
    angular
        .module('app')
        .controller('ism_sales_lead_demo_ReportController', ism_sales_lead_demo_ReportController);

    ism_sales_lead_demo_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce', '$timeout', 'Excel'];
    function ism_sales_lead_demo_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce, $timeout, Excel) {


        //=============image
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.maxDatemf = new Date();
        //loading start

        $scope.athflag = false;
        $scope.loadData = function () {
            var id = 2;

            apiService.getURI("Sales_Lead/getalllead", id).
                then(function (promise) {
                    $scope.grid_view = false;
                    $scope.leadlist = promise.leadlist;
                    $scope.roomlist = promise.roomlist
                });
        };

        $scope.togchkbxdept = function (groupType) {
            $scope.ISMSLE_Id = $scope.leadlist.every(function (itm) {
                return itm.selected;
            });


        };
        $scope.all_checkdept = function (ISMSLE_Id) {
            var toggleStatus = ISMSLE_Id;
            angular.forEach($scope.leadlist, function (itm) {
                itm.selected = toggleStatus;
            });

        };
        $scope.togchkbxdept1 = function (groupType) {
            $scope.HRMR_Id = $scope.roomlist.every(function (itm1) {
                return itm1.selected;
            });


        };
        $scope.all_checkdept1 = function (HRMR_Id) {
            var toggleStatus1 = HRMR_Id;
            angular.forEach($scope.roomlist, function (itm1) {
                itm1.selected = toggleStatus1;
            });
        };

        $scope.searchleadvalue = function (searchleadvalue) {
            $scope.searchleadvalue1 = searchleadvalue.length;
            if (searchleadvalue.length > 1) {
                var data = {
                    "searchleadvalue": searchleadvalue,
                    "flag": "Search"
                }
                apiService.create("ISM_Client_Project/searchleadvalue", data).
                    then(function (promise) {
                        $scope.leadlist = promise.leadlist;

                    })
            }
        };

        $scope.searchValue = '';
        $scope.searchValue1 = '';
        $scope.itemsPerPage = 15;
        $scope.currentPage = 1;


        $scope.grid_view = false;
        $scope.submitted = false;
        // $scope.leadlist = [];



        $scope.submitted = false;
        $scope.showreport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.leaddatalist = [];
                $scope.roomdatalist = [];
                $scope.leaddemoreport = [];
                angular.forEach($scope.leadlist, function (d) {
                    if (d.selected === true) {
                        $scope.leaddatalist.push({ ISMSLE_Id: d.ismslE_Id });
                    }
                });
                angular.forEach($scope.roomlist, function (dd) {
                    if (dd.selected === true) {
                        $scope.roomdatalist.push({ HRMR_Id: dd.hrmR_Id });
                    }
                });
                //if ($scope.leaddatalist.length == 0) {
                //    swal('Search Lead And Select Any One ..')
                //    return;
                //}

                $scope.fromdate = "";
                $scope.todate = "";
                $scope.fromdate = $filter('date')($scope.startdate, 'yyyy-MM-dd');
                $scope.todate = $filter('date')($scope.enddate, 'yyyy-MM-dd');
                if ($scope.Online == undefined) {
                    $scope.Online = 0;
                }
                if ($scope.OffLine == undefined) {
                    $scope.OffLine = 0;
                }

                if ($scope.Online == 0 && $scope.OffLine == 0) {
                    swal("Please Select Checkbox OffLine or Online Demo...");
                    return;
                } else {
                    var data = {
                        "fromdate": $scope.fromdate,
                        "todate": $scope.todate,
                        //"leaddatalist": $scope.leaddatalist,
                        //"roomdatalist": $scope.roomdatalist,
                        "Online": $scope.Online,
                        "OffLine": $scope.OffLine
                    }
                    apiService.create("Sales_Lead/demo_report/", data).
                        then(function (promise) {
                            if (promise.leaddemoreport.length > 0 && promise.leaddemoreport != null) {
                                $scope.leaddemoreport = promise.leaddemoreport;

                                $scope.colarrayall = [{
                                    title: "SLNO",
                                    template: "<span class='row-number'></span>", width: "40px"
                                },
                                {
                                    name: 'ISMSLE_LeadName', field: 'ISMSLE_LeadName', title: 'Lead Name', width: "150px"
                                },
                                {
                                    name: 'HRMR_RoomName', field: 'HRMR_RoomName', title: 'Room Name', width: "150px", hidden: !$scope.Online
                                },
                                {
                                    name: 'ISMSLEDM_DemoType', field: 'ISMSLEDM_DemoType', title: 'Demo Type', width: "80px"
                                },
                                {
                                    name: 'ISMSLEDM_DemoDate', field: 'ISMSLEDM_DemoDate', title: 'Demo Date', width: "100px", template: "#= kendo.toString(kendo.parseDate(ISMSLEDM_DemoDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                                },
                                {
                                    name: 'ISMSLEDM_ContactPerson', field: 'ISMSLEDM_ContactPerson', title: 'Contact Person', width: "100px"
                                },
                                {
                                    name: 'ISMSLEDM_Remarks', field: 'ISMSLEDM_Remarks', title: 'Remarks', width: "100px"
                                },

                                {
                                    name: 'employeename', field: 'employeename', title: 'Demo Given By', width: "100px"
                                }
                                ];

                                $(document).ready(function () {
                                    initGridall();
                                });
                                function initGridall() {
                                    $('#gridall').empty();
                                    //gridall =
                                    $("#gridall").kendoGrid({
                                        toolbar: ["excel"],
                                        toolbar: ["pdf"],
                                        excel: {
                                            fileName: "sales_lead_demo.xlsx",
                                            proxyURL: "",
                                            filterable: true,
                                            allPages: true
                                        },
                                        pdf: {
                                            avoidLinks: true,
                                            landscape: true,
                                            repeatHeaders: true,
                                            fileName: "sales_lead_demo.pdf",
                                            allPages: true
                                        },
                                        dataSource: {
                                            data: $scope.leaddemoreport,
                                            pageSize: 10,
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
                                            var pageitms = this.dataSource.pageSize()
                                            var rows = this.items();
                                            $(rows).each(function () {
                                                var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                                var rowLabel = $(this).find(".row-number");
                                                $(rowLabel).html(index);
                                            });
                                        }

                                    }).data("kendoGrid");
                                }

                                //$scope.taskid5 = [];
                                //angular.forEach($scope.leaddemoreport, function (dev) {
                                //    if ($scope.taskid5.length === 0) {
                                //        $scope.taskid5.push({ ISMSLE_Id: dev.ISMSLE_Id, ISMSLE_LeadName: dev.ISMSLE_LeadName });
                                //    } else if ($scope.taskid5.length > 0) {
                                //        var intcount = 0;
                                //        angular.forEach($scope.taskid5, function (emp) {
                                //            if (emp.ISMSLE_Id === dev.ISMSLE_Id) {
                                //                intcount += 1;
                                //            }
                                //        });

                                //        if (intcount === 0) {
                                //            $scope.taskid5.push({ ISMSLE_Id: dev.ISMSLE_Id, ISMSLE_LeadName: dev.ISMSLE_LeadName });
                                //        }
                                //    }
                                //});

                                //angular.forEach($scope.taskid5, function (ddd) {
                                //    $scope.templist = [];
                                //    angular.forEach($scope.leaddemoreport, function (dd) {
                                //        if (dd.ISMSLE_Id === ddd.ISMSLE_Id) {
                                //            $scope.templist.push(dd);
                                //        }
                                //    });
                                //    ddd.leaddemoreport = $scope.templist;
                                //});

                            }
                            else {
                                swal('No Data Found!!!')
                            }

                        })
                }
               
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.printData = function () {
            var innerContents = document.getElementById("printExcel").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/Sports/HouseReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();

        }


        $scope.exportToExcel = function (tableId) {
            var excelname = "LEAD DEMO REPORT";
            excelname = excelname.toUpperCase() + '.xls';

            if ($scope.leaddemoreport !== null && $scope.leaddemoreport.length > 0) {
                var exportHref = Excel.tableToExcel(printExcel, 'LEAD DEMO REPORT');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = excelname;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
                //$state.reload();
            }
            else {
                swal("Please Select Records to be Exported");
            }

        };




        $scope.clearid = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }

})();