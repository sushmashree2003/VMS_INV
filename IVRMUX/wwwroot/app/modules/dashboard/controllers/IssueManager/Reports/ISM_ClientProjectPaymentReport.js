(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_TaskGroup_ReportController', ISM_TaskGroup_ReportController);
    ISM_TaskGroup_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter'];
    function ISM_TaskGroup_ReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter) {
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        var paginationformasters;
        $scope.multidepartment = false;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.obj = {};
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        // ============================ng_click function for module (ALL)========================//
        $scope.searchchkbx = "";
        

        $scope.all_check = function () {

            var checkStatus = $scope.usercheck;
            angular.forEach($scope.projectlist, function (itm) {
                itm.select = checkStatus;
            });
        }
        $scope.togchkbx = function () {

            $scope.usercheck = $scope.projectlist.every(function (options) {
                return options.select;
            });
        }
        $scope.isOptionsRequired = function () {
            return !$scope.projectlist.some(function (options) {
                return options.select;
            });
        }
        $scope.submitted = false;
        $scope.searchchkbx = "";       
        $scope.Clear = function () {
            $state.reload();
        }

        //===========================load

        $scope.loaddata = function () {

            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("ISM_TaskGroup_Report/getdetails", pageid).then(function (promise) {
               
                $scope.projectlist = promise.projectlist;               
                $scope.reportdetails = promise.reportdetails;
            });
        };

        //*************************** Group REPORT *********************//        
        $scope.Report = function () {
          
            $scope.selectedprojectlist = [];           
            if ($scope.myForm.$valid) {
                angular.forEach($scope.projectlist, function (group) {
                    if (group.select == true) {
                        $scope.selectedprojectlist.push({ ismmpR_Id: group.ismmpR_Id });
                    }
                });                
                var data = {
                    selectedprojectlist: $scope.selectedprojectlist,                  
                };
                apiService.create("ISM_TaskGroup_Report/getReport", data).then(function (promise) {
                 
                    if (promise.reportdetails.length > 0) {
                        $scope.reportdetails = promise.reportdetails;
                    }
                    else {
                        swal("No Record Found..... !!");
                        $scope.reportdetails = "";
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.printData = function () {
            var innerContents = '';
            innerContents = document.getElementById("printareaId1").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); },900);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        }

        $scope.exportToExcel = function (table) {
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();

