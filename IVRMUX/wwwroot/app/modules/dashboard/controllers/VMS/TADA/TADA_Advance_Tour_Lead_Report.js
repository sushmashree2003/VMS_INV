(function () {
    'use strict';
    angular
        .module('app')
        .controller('TADA_Advance_Tour_LeadController', TADA_Advance_Tour_LeadController)
    TADA_Advance_Tour_LeadController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function TADA_Advance_Tour_LeadController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

        $scope.usrname = localStorage.getItem('username');
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.exportsheet = false;
        $scope.IsHiddenup = true;
        $scope.IsHiddendown = true;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.file_disable = true;
        $scope.grid_flag = false;
        $scope.export_flag = false;
        $scope.print_flag = false;
        $scope.printdatatable = [];
        $scope.searchString = "";


        $scope.loadtadadata = function () {
            apiService.get("TADA_Advance_Tour/getalldata/").then(function (promise) {
                if (promise.designationArray != null && promise.designationArray.length > 0) {
                    $scope.designationArray = promise.designationArray;
                }
            });
        }

        $scope.submitted = false;
        $scope.showreport = function () {
            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_Id": $scope.obj.Amay_id,
                    "FMCC_Id": $scope.fmcC_Id
                }

                apiService.create("TADA_Advance_Tour/", data).then(function (promise) {
                    if (promise.fhwR_searchdatalist != null && promise.fhwR_searchdatalist != "") {
                        $scope.export_flag = true;
                        $scope.print_flag = true;
                        $scope.searchdatalist = promise.fhwR_searchdatalist;
                        $scope.totcountfirst = promise.fhwR_searchdatalist.length;
                        $scope.file_disable = false;
                        $scope.grid_flag = true;
                    }

                    else {
                        swal("No Records Found");
                        $scope.file_disable = true;
                        $scope.grid_flag = false;
                    }
                })
            }
            else {
                $scope.submitted = true;

            }
        }

        //Cancel
        $scope.clear_details = function () {
            $state.reload();
        }


        //Check All in Report
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.filterValue1, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all = $scope.searchdatalist.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }

        }

        $scope.ShowHidedown = function () {
            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        }

        //Export To excel
        $scope.exportToExcel = function (tableId) {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }
        }

        //Print Data
        $scope.printData = function () {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
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







