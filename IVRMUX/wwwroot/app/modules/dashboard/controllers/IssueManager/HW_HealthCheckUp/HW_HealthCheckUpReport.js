(function () {
    'use strict';
    angular
        .module('app')
        .controller('HW_HeathCheckUpController', HW_HeathCheckUpController);
    HW_HeathCheckUpController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function HW_HeathCheckUpController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache) {
        var paginationformasters;


        $scope.max_Date = new Date();

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !=null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.obj = {};
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();
        $scope.depdes = false;

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.submitted = false;
        $scope.objques = {};
        $scope.gridflag = false;


        $scope.startdate = new Date($scope.start_Date).toDateString();
        $scope.enddate = new Date($scope.end_Date).toDateString();
        if ($scope.start_Date !== null && $scope.end_Date) {
            $scope.dateflag = 1;
        }
        else {
            $scope.dateflag = 0;
        }

        $scope.loaddata = function () {
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            $scope.dateflag = 0;
            var pageid = 2;
            $scope.details = [];
            $scope.detailsOne = [];
            var data = {
                "OnLoadOrOnChange": "OnLoad"
            };
            apiService.create("HW_HeathCheckUp/HealthCheckUpInstitution", data).then(function (promise) {
                $scope.getmastertype = promise.getuserinstitution;
                $scope.Master_Type_MI_Id = promise.mI_Id;
            });
        };

        $scope.all_checkuu = function (ack) {
            $scope.checkalluu = ack;
            var toggleStatus = $scope.checkalluu;
            angular.forEach($scope.get_userEmplist, function (uem) {
                uem.empck = toggleStatus;
            });


            $scope.onEmployeechange();
        };

        $scope.exportToExcel = function () {          
            var blob = new Blob([document.getElementById('printP').innerHTML], {
                type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
            });

            saveAs(blob, "HWHealthCheckupReport.xls");
        };

        $scope.exportToExcelOne = function () {
            var blob = new Blob([document.getElementById('printC').innerHTML], {
                type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
            });

            saveAs(blob, "HWHealthCheckupReport.xls");
        };

        $scope.searchValueDept = '';
        $scope.filterValue4 = function (obj) {
            return angular.lowercase(obj.mI_Name).indexOf(angular.lowercase($scope.searchValueDept)) >= 0;
        };

        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.namme)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.clientName)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.visitedName)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.ApprovedBy)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.ApprovedStatus)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.visitedDate)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.remarks)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.classes)).indexOf($scope.searchValue) >= 0 

        };

        $scope.filterValueOne = function (obj) {
            return (angular.lowercase(obj.namme)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.clientName)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.visitedDate)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.visitedName)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.remarks)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.classes)).indexOf($scope.searchValue) >= 0
        };     
      
       
        //***************************** Date Change
        $scope.onenddatechange = function () {           
            $scope.searchValue = "";
            $scope.searchValueOne = "";
            $scope.details = [];
            $scope.detailsOne = [];
        };

        $scope.OnChangeFromDate = function () {           
            $scope.searchValue = "";
            $scope.searchValueOne = "";
            $scope.details = [];
            $scope.detailsOne = [];
            $scope.end_Date = null;
        };       

        //*************************** Approval Report *********************//        
        $scope.approvalReport = function () {
            $scope.plannerid = [];
            if ($scope.myForm.$valid) {            
               
                $scope.searchValue = "";
                $scope.searchValueOne = "";
                $scope.details = [];
                $scope.detailsOne = [];
               
                
                var data = {
                  
                    "MI_Id": $scope.Master_Type_MI_Id,
                    "FromDate": $scope.start_Date,
                    "ToDate": $scope.end_Date,
                    "HWHCHUP_Stauts": $scope.HWHCHUP_Stauts
                };
                apiService.create("HW_HeathCheckUp/getdetails", data).then(function (promise) {
                    $scope.plannerid = [];
                    if (promise.get_HWreport !=null && promise.get_HWreport.length > 0 ) {                       
                        $scope.details = promise.get_HWreport;                       
                        $scope.presentCountgrid = $scope.details.length;                        
                    }
                    else if (promise.get_HWreportOne != null && promise.get_HWreportOne.length > 0)
                    {
                        $scope.detailsOne = promise.get_HWreportOne;
                        $scope.presentCountgridOne = $scope.detailsOne.length;
                    }
                    else {
                        swal("No Records Found!!");
                    }
                    $scope.imagenew = promise.mI_Logo;
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            //popupWinindow.document.write('<html><head>' +
            //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
            //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
            //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
            //    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');

            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.printDataOne = function () {
            var innerContents = "";
            innerContents = document.getElementById("printC").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            //popupWinindow.document.write('<html><head>' +
            //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
            //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
            //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
            //    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');

            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.Clear = function () {
            $state.reload();
            //$scope.departmentselectedAll = false;          
            //$scope.details =[];
            //$scope.detailsOne = [];
            //$scope.get_Plannerlist = "";
            //$scope.checkall = "";
            //$scope.start_Date = "";
            //$scope.end_Date = "";         
            //$scope.loaddata();
        };

        $scope.clearreport = function () {
            $scope.details = [];
            $scope.detailsOne = [];
            $scope.plannerid = [];
        };

    }
})();

