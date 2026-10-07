(function () {
    'use strict';
    angular.module('app').controller('CheckBook_BudgetHeadReportController', CheckBook_BudgetHeadReportController)
    CheckBook_BudgetHeadReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', 'Excel','$timeout']
    function CheckBook_BudgetHeadReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, Excel, $timeout) {
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.obj = {};
        $scope.submitted = false;

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }
        var configsettingsnew = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        $scope.itemsPerPage = paginationformasters;

        $scope.currentPage = 1;
        //-----------Tax Grid Select All
        $scope.allmi = false;
        $scope.loaddata = function () {
            $scope.obj.MI_Id = "";
            var pageid = 2;
            $scope.Month_List = [];
            apiService.getURI("CheckBook_BudgetHeads/getalldata", pageid).
                then(function (promise) {
                    $scope.allcompany = promise.allcompany;
                    $scope.FinancialYear = promise.financialYear;
                    $scope.Month_List = promise.month_List;
                });
        };
        $scope.all_miid = function (ac) {
            $scope.allmi = ac;
            var toggleStatus = $scope.allmi;
            angular.forEach($scope.CheckBookBudget, function (itm) {
                itm.miid = toggleStatus;
            });

        };
        //allmidd
        $scope.allmidd = function (ac) {
            $scope.obj.IMFY_Id = "";
            $scope.allmiddff = ac;
            var toggleStatus = $scope.allmiddff;
            angular.forEach($scope.allcompany, function (itm) {
                itm.miiddd = toggleStatus;
            });

        };
        $scope.allmiddd = function (allmiddffpo) {
            var toggleStatus = allmiddffpo;
            angular.forEach($scope.Month_List, function (itm) {
                itm.miiddfgd = toggleStatus;
            });

        };
        $scope.isRequiredmithree = function () {

            return !$scope.Month_List.some(function (options) {
                return options.miiddfgd;
            });

        };
        //isRequiredmithree
        $scope.isRequiredmitwo = function () {

            return !$scope.allcompany.some(function (options) {
                return options.miiddd;
            });

        };
        $scope.isRequiredmi = function () {

            return !$scope.CheckBookBudget.some(function (options) {
                return options.miid;
            });

        };
        $scope.FianciBudget = function () {
            $scope.CheckBookBudget = [];
            $scope.obj.VMBDGHD_Id = "";
            $scope.CheckbookSource = [];
            $scope.CheckBookBudgetlist = [];
            $scope.temp_MI_ID = [];
            if ($scope.allcompany != null && $scope.rdopunch == "Consolidated"  && $scope.allcompany.length > 0) {
                angular.forEach($scope.allcompany, function (itm) {
                    if (itm.miiddd == true) {
                        $scope.temp_MI_ID.push({
                            MI_Id: itm.mI_Id,
                        });
                    }
                });
            }
            else {
                $scope.temp_MI_ID.push({
                    MI_Id: $scope.obj.MI_Id,
                });
            }
            var data = {
                //"MI_Id": $scope.obj.MI_Id,
                "IMFY_Id": $scope.obj.IMFY_Id,
                "MultipleComapnyBudget": $scope.temp_MI_ID

            };
            apiService.create("CheckBook_BudgetHeads/TadaAllwence", data).then(function (promise) {
                if (promise.checkBookBudget != null && promise.checkBookBudget.length > 0) {
                    $scope.CheckBookBudget = promise.checkBookBudget;
                }
                else {
                    $scope.obj.IMFY_Id = "";
                    swal("Check Book Budget Not Found !");
                }



            });
        }

        $scope.Uploaddata = function (user) {
            $scope.CheckBookBudgetlist = [];
          
            var data = {
                "MI_Id": user.MI_ID,
                "VMBDGHD_Id": user.VMBDGHD_Id,
                "IMFY_Id": user.IMFY_Id,

            };
            apiService.create("CheckBookEntry/getreport", data).then(function (promise) {
                if (promise.alldata != null && promise.alldata.length > 0) {
                    $scope.CheckBookBudgetlist = promise.alldata;
                    $scope.imagename = promise.mI_logo;
                    $scope.VMBDGHD_BudgetHead = promise.alldata[0].VMBDGHD_BudgetHead;
                    $scope.VMBDGHDAMT_Amount = promise.alldata[0].VMBDGHDAMT_Amount;
                    $scope.IMFY_FinancialYear = promise.alldata[0].IMFY_FinancialYear;
                    $scope.VMBDGHDAMT_MonthwiseAmount = promise.alldata[0].VMBDGHDAMT_MonthwiseAmount;
                    var TotalApproved = 0;
                    if ($scope.CheckBookBudgetlist != null && $scope.CheckBookBudgetlist.length > 0) {
                        for (var i = 0; i < $scope.CheckBookBudgetlist.length; i++) {
                            if ($scope.CheckBookBudgetlist[i].VPAYVOU_ApprovedAmount > 0) {
                                TotalApproved += (Number($scope.CheckBookBudgetlist[i].VPAYVOU_ApprovedAmount));
                            }
                            else {
                                TotalApproved += (Number($scope.CheckBookBudgetlist[i].VPAYVOU_AppliedAmount));
                            }
                        }
                    }

                    $scope.TotalApproved = TotalApproved;
                    $('#CheckBookBudget').modal('show');

                }
                else {
                    swal("Check Book Budget Not Found !");
                }
            });
        }
        $scope.savedata = function () {
            $scope.CheckbookSource = [];
            $scope.BudgetHeadlist = [];
            $scope.CheckBookBudgetlist = [];
            $scope.temp_MI_ID = [];
            $scope.BudgetReportotal = [];
            if ($scope.CheckBookBudget != null && $scope.CheckBookBudget.length > 0 && ($scope.rdopunch == "Consolidated" || $scope.rdopunch == "Monthwise")) {
                angular.forEach($scope.CheckBookBudget, function (itm) {
                    if (itm.miid == true) {
                        $scope.BudgetHeadlist.push({
                            VMBDGHD_Id: itm.VMBDGHD_Id,
                        });
                    }
                });
            }
            else if ($scope.CheckBookBudget != null && $scope.CheckBookBudget.length > 0 && $scope.rdopunch == "Detailed") {
                $scope.BudgetHeadlist.push({
                    VMBDGHD_Id: $scope.obj.VMBDGHD_Id,
                });
            }

            $scope.temp_MI_ID = [];
            if ($scope.allcompany != null && $scope.rdopunch == "Consolidated" && $scope.allcompany.length > 0) {
                angular.forEach($scope.allcompany, function (itm) {
                    if (itm.miiddd == true) {
                        $scope.temp_MI_ID.push({
                            MI_Id: itm.mI_Id,
                        });
                    }
                });
            }
           
            else {
                $scope.temp_MI_ID.push({
                    MI_Id: $scope.obj.MI_Id,
                });
            }
            $scope.temp_MonthList = [];
            if ($scope.Month_List != null && $scope.Month_List.length > 0 && $scope.rdopunch == "Monthwise") {
                angular.forEach($scope.Month_List, function (itm) {
                    if (itm.miiddfgd == true) {
                        $scope.temp_MonthList.push({
                            IVRM_Month_Id: itm.ivrM_Month_Id,
                            IVRM_Month_Name: itm.ivrM_Month_Name,
                        });
                    }
                });
            }
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "BudgetHeadlist": $scope.BudgetHeadlist,
                    "IMFY_Id": $scope.obj.IMFY_Id,
                   // "MI_Id": $scope.obj.MI_Id,
                    "MultipleComapnyBudget": $scope.temp_MI_ID,
                    "returnval": $scope.rdopunch,
                    "temMonth_List": $scope.temp_MonthList,
                };
                apiService.create("CheckBook_BudgetHeads/BudgetReport", data).then(function (promise) {
                    if (promise.budgetReport != null && promise.budgetReport.length > 0) {
                        $scope.CheckbookSource = promise.budgetReport;
                        $scope.CheckBookBudgetlist = promise.budgetReport;
                        $scope.imagename = promise.mi_logo;
                        if ($scope.rdopunch == "Detailed" && $scope.CheckBookBudgetlist.length > 0) {
                            $scope.imagename = promise.mi_logo;
                            $scope.VMBDGHD_BudgetHead = promise.budgetReport[0].VMBDGHD_BudgetHead;
                            $scope.VMBDGHDAMT_Amount = promise.budgetReport[0].VMBDGHDAMT_Amount;
                            $scope.IMFY_FinancialYear = promise.budgetReport[0].IMFY_FinancialYear;
                            $scope.VMBDGHDAMT_MonthwiseAmount = promise.budgetReport[0].VMBDGHDAMT_MonthwiseAmount;
                            var TotalApproved = 0;
                            if ($scope.CheckBookBudgetlist != null && $scope.CheckBookBudgetlist.length > 0) {
                                for (var i = 0; i < $scope.CheckBookBudgetlist.length; i++) {
                                    if ($scope.CheckBookBudgetlist[i].VPAYVOU_ApprovedAmount > 0) {
                                        TotalApproved += (Number($scope.CheckBookBudgetlist[i].VPAYVOU_ApprovedAmount));
                                    }
                                    else {
                                        TotalApproved += (Number($scope.CheckBookBudgetlist[i].VPAYVOU_AppliedAmount));
                                    }
                                }
                            }

                            $scope.TotalApproved = TotalApproved;
                        }
                        $scope.BudgetReportotal = promise.budgetReportotal;
                    }
                    else {
                        swal("Record Not Found !")
                    }
                    
                });
            }

        };
        $scope.exportToExcel = function (export_id) {

            var exportHref = Excel.tableToExcel(export_id, 'sheet name');
            $timeout(function () {
                location.href = exportHref;
            }, 100);

        };
        $scope.printData = function () {

            var innerContents = document.getElementById("printpi").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
       //printpiP
        $scope.printDatatwo = function () {

            var innerContents = document.getElementById("printpiP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.otherdaydatechange = function () {
            $scope.CheckbookSource = [];
            $scope.CheckBookBudgetlist = [];

        }
       
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();
