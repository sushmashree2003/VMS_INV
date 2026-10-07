(function () {
    'use strict';
    angular.module('app').controller('TypeQuestionOptionMappingController', TypeQuestionOptionMappingController)
    TypeQuestionOptionMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'Excel', 'apiService', '$timeout','$http', 'superCache', '$filter', '$q']
    function TypeQuestionOptionMappingController($rootScope, $scope, $state, $location, Flash, appSettings, Excel, apiService, $timeout, $http, superCache, $filter, $q) {

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

        $scope.OnLoadRatingMasterType = function () {
           
            $scope.Master_Type_MI_Id = "";
            $scope.getuserinstitution = [];
            $scope.GetMasterTypeLoadData = [];
            $scope.RATMT_Id = 0;
        
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnLoad"
            };
            apiService.create("TypeQuestionOptionMapping/OnLoadRatingMasterType", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getuserinstitution = promise.getuserinstitution;
                    $scope.Master_Type_MI_Id = promise.mI_Id;
                    $scope.getmastertype = promise.getmastertype;
                    $scope.GetMasterTypeLoadData = promise.getMasterTypeLoadData;
                }
            });
        };

        $scope.OnChangeMasteTypeInsitution = function () {
           
            $scope.gridflag = true;
            $scope.searchmastertype = "";
            $scope.GetMasterTypeLoadData = [];
           
        
            $scope.RATMT_Id = 0;
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnChange",
                "MI_Id": $scope.Master_Type_MI_Id
            };
            apiService.create("TypeQuestionOptionMapping/OnLoadRatingMasterType", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getmastertype = promise.getmastertype;
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

            $scope.departmentselectedAll = false;

            $scope.details = "";
            $scope.detailsOne = "";
            $scope.checkall = "";
            $scope.Master_Type_MI_Id = "";
            $scope.ratmt_Id = "";

            $scope.OnLoadRatingMasterType();
        };


        $scope.clearreport = function () {
            $scope.details = [];

        };



      
      
    }
})();