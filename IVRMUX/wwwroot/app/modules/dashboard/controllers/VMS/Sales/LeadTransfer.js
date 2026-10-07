(function () {
    'use strict';
    angular.module('app')
        .controller('LeadTransferController', LeadTransferController)
    LeadTransferController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
    function LeadTransferController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !=null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings!=null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.sortKey = "regno";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa
        $scope.presentCountgrid = 0;
        $scope.searchValue = "";
        $scope.tadprint = false;
        $scope.nameschedule = true;
        $scope.printdatatable = [];
        $scope.currentPage1 = 1; $scope.Serachvalues = "";
        $scope.itemsPerPage1 = 10; $scope.LeadTransfer = [];
        $scope.loaddata = function () {
            $scope.screport = false;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            var pageid = 2;
            apiService.getURI("ISM_Client_Project/Transferload", pageid).
                then(function (promise) {                 
                    $scope.yearlist = promise.yearlist;
                    $scope.allleadIncharge = promise.leadIncharge;
                    if (promise.leadInchargetList != null && promise.leadInchargetList.length > 0) {
                        $scope.inchargelist = promise.leadInchargetList;
                    } 
                })
        };
        $scope.onchangeIncharge = function () {
            $scope.inchargeName = "";
            angular.forEach($scope.inchargelist, function (itm) {
                if (itm.hrmE_Id == $scope.HRME_Id.hrmE_Id) {
                    $scope.inchargeName = itm.empname;
                    return;
                }
            });   
        }
        
        $scope.AddToCart = function () {          
            angular.forEach($scope.leadlist, function (itm) {               
                if (itm.checked == true) {
                    $scope.LeadTransfer.push(itm);
                    itm.checked = false;                  
                   // $scope.leadlist.splice(index, 1);
                }
            });    
        }
        $scope.RemoveY = function (index) {          
            $scope.LeadTransfer.splice(index, 1);
        }
        $scope.gettransferLeads = function () {
            $scope.leadlist = [];
            var data = {
                "IMFY_Id": $scope.IMFY_Id
            }
            apiService.create("ISM_Client_Project/gettransferLeads", data).
                then(function (promise) {
                    if (promise.leadlist != null && promise.leadlist.length > 0) {
                        $scope.leadlist = promise.leadlist;
                    }
                    else {
                        swal("Record Not Found !");

                    }
                   
                })
        }
        //$scope.getdetails = function () {
        //    var data = {
        //        "IMFY_Id": $scope.IMFY_Id,
        //        "ISMSLE_Id": $scope.ISMSLE_Id.ISMSLE_Id
        //    }
        //    apiService.create("ISM_Client_Project/getTransferDetails", data).
        //        then(function (promise) {                 
        //            if (promise.leadIncharge != null && promise.leadIncharge.length > 0) {
        //                $scope.leadincharge = promise.leadIncharge;
        //                $scope.inchargeName = $scope.leadincharge[0].employeeName;
        //                $scope.ISMSLE_Id = $scope.leadincharge[0].ISMSLE_Id;
        //            }
        //            if (promise.leadInchargetList != null && promise.leadInchargetList.length > 0) {
        //                $scope.inchargelist = promise.leadInchargetList;
        //            }                   
        //        })
        //}
        //$scope.ISMSLE_Id = 0;

        $scope.submitted = false;
        $scope.LeadTransferSave = function () {
            $scope.LeadTransferTemp = [];
            if ($scope.myForm.$valid) {        
                angular.forEach($scope.LeadTransfer, function (itm) {
                    $scope.LeadTransferTemp.push({
                        ISMSLE_Id: itm.ismslE_Id,
                        
                    })
                });  
                var data = {
                    "HRME_Id": $scope.HRME_Id.hrmE_Id, 
                    "leaddatalist": $scope.LeadTransferTemp 
                }
               
                apiService.create("ISM_Client_Project/LeadTransferSave/", data).
                    then(function (promise) {
                        if (promise.message !== null) {
                            swal(promise.message);
                            $state.reload();
                         
                        }
                        else {
                            swal('Please contact Administrator !')
                        }
                    })
            }
            else {

                $scope.submitted = true;
            }
        };
      //export start
        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };
        $scope.printData = function () {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };



    }
})();