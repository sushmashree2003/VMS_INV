(function () {
    'use strict';
    angular
        .module('app')
        .controller('Booking_Details', Booking_Details)
    Booking_Details.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter', 'Excel', '$timeout']
    function Booking_Details($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter, Excel, $timeout) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //---------------loaddata
        $scope.loaddata = function () {           
            apiService.getURI("Booking_Details/getdetails", 2).then(function (promise) {              
                $scope.roomlist = promise.bookingList;
                $scope.enqlist_Report = promise.enqlist_Report;
            });
        }
        $scope.printflag = false;
        $scope.reportlist = false;
        $scope.submitted = false;
        $scope.hrmR_Id = 0;
        $scope.get_report = function () {
         
            $scope.bookinglist_Report = [];
            if ($scope.myForm.$valid) {
                var data = {
                    "HRMR_Id": $scope.hrmR_Id,
                    "fromdate1": new Date($scope.testfromdate).toDateString(),
                    "fromdate2": new Date($scope.testtodate).toDateString(),   
                    "OSSPBOOK_Status": $scope.OSSPBOOK_Status,                   
                    "OSSPBOOK_DurationType": $scope.OSSPBOOK_DurationType,                   
                }
                apiService.create("Booking_Details/booking_report", data)
                    .then(function (promise) {                      
                        if (promise.enqlist_Report.length > 0) {
                            $scope.enqlist_Report = promise.enqlist_Report;                          
                            $scope.printflag = true;
                            $scope.reportlist = true;
                        }
                        else {
                            swal('Record is not Available!!');
                            $scope.printflag = false;
                            $scope.reportlist = false;
                        }
                    })
            }
            else {
                $scope.submitted = true;
            }
        }        
        //===========print===========//
        $scope.printData = function () {
            var innerContents = '';
            innerContents = document.getElementById("printtable").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/SpaceBookingReport/SBReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        //==============End==============//
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //--------------Clear-field.....
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.ExportToExcel = function (table) {           
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }        
        $scope.get_listdata = function () {
            $scope.bookinglist_Report = [];
            $scope.printflag = false;
            $scope.reportlist = false;
        }
      }
})();

