(function () {
    'use strict';
    angular.module('app').controller('ModuleWiseFeedbackReportController', ModuleWiseFeedbackReportController)
    ModuleWiseFeedbackReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'Excel', 'apiService', '$timeout','$http', 'superCache', '$filter', '$q']
    function ModuleWiseFeedbackReportController($rootScope, $scope, $state, $location, Flash, appSettings, Excel, apiService, $timeout, $http, superCache, $filter, $q) {

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

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.submitted = false;
        $scope.objques = {};
        $scope.gridflag = false;
        // Master Type



        $scope.OnLoadMasterClient = function () {
           
            $scope.gridflag = true;

            var pageid = 2;

            apiService.getURI("ModuleWiseFeedbackReport/OnLoadClient", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.masterclientlist = promise.masterclientlist;
                }l
            });
        };



        $scope.OnChangeMasterClient = function () {


            $scope.templist = [];
            angular.forEach($scope.masterclientlist, function (dd) {
                if (dd.ISMMCLT_Id == Number($scope.Client_Id)) {
                    $scope.templist.push({ IVRM_MI_Id: dd.IVRM_MI_Id, Institute_code: dd.ISMMCLT_Code});
                }
            });

            var data = {
                
                //"clientlistarr": $scope.templist

                "fromdate": new Date($scope.FromDate).toDateString(),
                "todate": new Date($scope.ToDate).toDateString(),

            };

            apiService.create("ModuleWiseFeedbackReport/getmodulewisefeedbackreport", data).then(function (promise) {
                $scope.getfeedbackdata = [];
                if (promise.getfeedbackdata !== null && promise.getfeedbackdata.length > 0) {
                    $scope.getfeedbackdata = promise.getfeedbackdata;

                    $scope.presentCountgrid = $scope.getfeedbackdata.length;
                } 
            });
        };




        $scope.interacted = function (field) {
            return $scope.submitted;
        };

      


        // Master Question
       

      

        $scope.Report = function () {
            $scope.get_report = [];
           
           $scope.details = "";
            if ($scope.myForm.$valid) {
               
                var data = {
                    "MI_Id": $scope.Master_Type_MI_Id,
                    "RATMT_Id": $scope.ratmt_Id

                };
                apiService.create("TypeQuestionOptionMapping/getReport", data).then(function (promise) {
                    $scope.details = [];
                   
                    if (promise.get_report != null && promise.get_report.length > 0) {

                        $scope.details = promise.get_report;
                        $scope.presentCountgrid = $scope.details.length;
                       //// $scope.printdata = $scope.details;
                        $scope.gridflag = true;
                    }
                    else {
                        swal("No Records Found!!");
                    }

                });
             
            }
            else {
                $scope.submitted = true;
            }
        };
        

      

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.namme)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.ratmqE_RatingQuestions)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.ratmoP_RatingOptions)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.classes)).indexOf($scope.searchValue) >= 0
        };

       


        $scope.printData = function () {

                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();

                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');

               
                popupWinindow.document.close();
           
        }

        $scope.exportToExcel = function () {

            
                var exportHref = Excel.tableToExcel(printSectionId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
          

        }



        $scope.hide = function () {
            $scope.gridflag = false;
        };

        $scope.Clear = function () {

            $state.reload();
         
        };


        $scope.clearreport = function () {
            $scope.details = [];

        };



      
      
    }
})();