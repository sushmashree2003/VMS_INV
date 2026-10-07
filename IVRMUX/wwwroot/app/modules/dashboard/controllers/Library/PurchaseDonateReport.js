(function () {
    'use strict';
    angular
        .module('app')
        .controller('PurchaseDonateReportController', PurchaseDonateReportController)

    PurchaseDonateReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function PurchaseDonateReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {



        //by deepak
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        // $scope.obj = {};

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        //End by 
        // $scope.IsHiddenup = true;
        $scope.printdatatable = [];
        $scope.printdatatablegrp = [];
        $scope.printdatatablehad = [];
        $scope.printdatatablecls = [];
        $scope.itemterm = [];
        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        // $scope.Mi_Id = configsettings[0].mI_Id;
        var institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));


        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }


        //=====================Loaddata
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            debugger;
            var pageid = 2;
            $scope.stf = false;
            $scope.std = false;
            $scope.book_Flag = "BP";
            $scope.gridview = false;
          
           
        }
        //=====================End-----Loaddata----//

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.Clearid = function () {
            debugger;
            $state.reload();
        }

        $scope.savedata = function () {
            if ($scope.myForm.$valid) {

                    var data = {
                        "LBCPA_Flg": $scope.LMB_BookType,
                        "Circ_Flag": $scope.book_Flag,
                    }

                    apiService.create("PurchaseDonateReport/getdata", data)
                        .then(function (promise) {
                            if (promise.alldata != null && promise.alldata.length > 0) {
                                $scope.alldata = promise.alldata;
                                $scope.result = true;
                                $scope.gridview = true;
                        }
                        else {
                                swal("No Records Found");
                                $scope.gridview = false;
                        }

                    })

            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.exportToExcel = function () {
            debugger;
         
            if ($scope.alldata !== null && $scope.alldata.length > 0) {
                var exportHref = Excel.tableToExcel(printSectionIdgrp1, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("No records Found to Export");
                }
        }
        
        $scope.printData = function () {
           
            if ($scope.alldata !== null && $scope.alldata.length > 0) {
                    var innerContents = document.getElementById("printSectionIdgrp1").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                  
                }
                else {
                    swal("Please Select Records to be Printed");
                }
        }
        
    }
})();

