
(function () {
    'use strict';
    angular
        .module('app')
        .controller('EnquiryReportController', EnquiryReportController)

    EnquiryReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter', 'Excel', '$timeout']
    function EnquiryReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter, Excel, $timeout) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //---------------Load --data
        $scope.loaddata = function () {
          
            apiService.getURI("SBReport/getdetails", 2).then(function (promise) {
                if (promise.milist != null || promise.milist > 0) {
                    $scope.milist = promise.milist;
                    $scope.MIId = promise.miid;
                    $scope.userflag = promise.userflag;
                }
            });

        }
        //--------------End-Load --data
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        //---------------Get-Report   
        $scope.printflag = false;
        $scope.submitted = false;
        $scope.reportlist = false;
        $scope.get_report = function () {
            
            $scope.enqlist_Report = [];
            if ($scope.myForm.$valid) {
                //var entrydate = $scope.testfromdate == null ? "" : $filter('date')($scope.testfromdate, "yyyy-MM-dd");
                //var entrydate2 = $scope.testtodate == null ? "" : $filter('date')($scope.testtodate, "yyyy-MM-dd");
                var data = {
                    "fromdate1": new Date($scope.testfromdate).toDateString(),
                    "fromdate2": new Date($scope.testtodate).toDateString(),
                    //"fromdate1": entrydate,                    
                    //"fromdate2": entrydate2,
                    "enqstatus": $scope.enqstatus,
                    "enqtype": $scope.enqtype,
                    "MI_Id_new": $scope.MIId,
                }
                
                apiService.create("SBReport/enquary_report", data)
                    .then(function (promise) {

                        if (promise.enqlist_Report.length > 0) {
                            $scope.enqlist_Report = promise.enqlist_Report;
                            $scope.company_name = promise.company_name;
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
        //-------------End--Get-Report



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
            debugger;
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }

        $scope.get_listdata = function () {

            $scope.enqlist_Report = [];
            $scope.printflag = false;
            $scope.reportlist = false;
        }

    }
})();

