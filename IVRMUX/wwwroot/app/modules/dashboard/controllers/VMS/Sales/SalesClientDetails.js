(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_Client_Project_BOMrController', ISM_Client_Project_BOMrController)


      ISM_Client_Project_BOMrController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce', '$timeout', 'Excel'];
      function ISM_Client_Project_BOMrController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce, $timeout, Excel) {




        $scope.objques = {};
        $scope.sortKey = '';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }



        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));



        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        //get data
        $scope.getAllDetail = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("ISM_Client_Project/getClientData", pageid)
                .then(function (promise) {
                    $scope.ClientDataOnload = promise.clientDataOnload;
                    $scope.ProductListOnload = promise.productListOnload;
                    $scope.StatusListOnload = promise.statusListOnload;
                   
                })
        }

        //save
        $scope.submitted = false;
        $scope.Report = function () {

            //$scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.ISMSLE_IdNew = "";
                $scope.ISMSMPR_IdNew = "";
                $scope.ISMSMST_IdNew = "";
                if ($scope.ISMSLE_Id == undefined) {
                    $scope.ISMSLE_IdNew = "0";
                }
                else {
                    $scope.ISMSLE_IdNew = String($scope.ISMSLE_Id.ismslE_Id);
                }
                if ($scope.ISMSMPR_Id == undefined) {
                    $scope.ISMSMPR_IdNew = "0";
                }
                else {
                    $scope.ISMSMPR_IdNew = String($scope.ISMSMPR_Id.ismsmpR_Id);
                }
                if ($scope.ISMSMST_Id == undefined) {
                    $scope.ISMSMST_IdNew = "0";
                }
                else {
                    $scope.ISMSMST_IdNew = String($scope.ISMSMST_Id.ismsmsT_Id)
                }
                var data = {
                    "ISMSLE_IdNew": $scope.ISMSLE_IdNew,
                    "ISMSMPR_IdNew": $scope.ISMSMPR_IdNew,
                    "ISMSMST_IdNew": $scope.ISMSMST_IdNew
                   

                }
                apiService.create("ISM_Client_Project/getClientReport", data)
                    .then(function (promise) {
                        if (promise.clientReport.length > 0 && promise.clientReport != undefined && promise.clientReport != null) {
                            $scope.ClientReport = promise.clientReport;
                        }
                        else {
                            swal("No Data Found..");
                            $state.reload();
                        }
                    })

            }
            else {

                $scope.submitted = true;
            }

        };

       

        $scope.cancel = function () {
            //$scope.ISMSLE_Id = "";
            //$scope.ISMSMPR_Id = "";
            //$scope.ISMSMST_Id = "";
            $state.reload();
        }

        //Print
        $scope.printData = function () {
            var innerContents = document.getElementById("tableexcell").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }



        $scope.exportToExcel = function () {
            var excelnamemain = "Client Details";
            var printSectionId = '#tableexcell';
            var exportHref = Excel.tableToExcel(printSectionId, 'sheet name');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelnamemain;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };
      

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


    }

})();



