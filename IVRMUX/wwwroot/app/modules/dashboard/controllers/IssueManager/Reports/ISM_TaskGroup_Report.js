(function () {
    'use strict';
    angular.module('app').controller('ISM_TaskGroup_ReportController', ISM_TaskGroup_ReportController);
    ISM_TaskGroup_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter'];
    function ISM_TaskGroup_ReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter) {
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

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
        $scope.all_checktype = function () {

            var checkStatus = $scope.userchecktype;
            angular.forEach($scope.department_list, function (itmtype) {
                itmtype.select1 = checkStatus;
            });
        };
        $scope.togchkbxtype = function () {

            $scope.userchecktype = $scope.department_list.every(function (optionstype) {
                return optionstype.select1;
            });
        };
        $scope.isOptionsRequiredtype = function () {
            return !$scope.department_list.some(function (options) {
                return options.select1;
            });
        };
       
        $scope.all_check = function () {

            var checkStatus = $scope.usercheck;
            angular.forEach($scope.grouplist, function (itm) {
                itm.select = checkStatus;
            });
        };

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.grouplist.every(function (options) {
                return options.select;
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.grouplist.some(function (options) {
                return options.select;
            });
        };

        $scope.submitted = false;
        $scope.searchchkbx = "";
        $scope.searchchkbxtype = "";

        $scope.Clear = function () {
            $state.reload();
        };

       //===========================load

        $scope.loaddata = function () {
            
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("ISM_TaskGroup_Report/getdetails", pageid).then(function (promise) {
               
                $scope.grouplist = promise.grouplist;
                $scope.department_list = promise.department_list;
                $scope.userdetails = promise.userdetails;               
            });
        };        
      
        //*************************** Group REPORT *********************//        
        $scope.taskgroupReport = function () {
            
            $scope.selectedgrouplist = [];
            $scope.selecteddepartment_list = [];           
            if ($scope.myForm.$valid) {
                angular.forEach($scope.grouplist, function (group) {
                    if (group.select === true) {
                        $scope.selectedgrouplist.push({ ismmtgrP_Id: group.ismmtgrP_Id });
                    }
                });
                angular.forEach($scope.department_list, function (dept) {
                    if (dept.select1 === true) {
                        $scope.selecteddepartment_list.push({ hrmD_Id: dept.hrmD_Id });
                    }
                });

                var data = {                    
                    selectedgrouplist: $scope.selectedgrouplist,              
                    selecteddepartment_list: $scope.selecteddepartment_list,
                    "startdate": new Date($scope.start_Date).toDateString(),
                    "enddate": new Date($scope.end_Date).toDateString(),
                };
                apiService.create("ISM_TaskGroup_Report/getplannerReport", data).then(function (promise) {                   
                    if (promise.userdetails.length > 0) {
                        $scope.userdetails = promise.userdetails;                       
                    }
                    else {
                        swal("No Record Found..... !!");
                        $scope.userdetails = "";
                        $state.reload();
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
        };

        $scope.exportToExcel = function (table) {
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };       
    }
})();