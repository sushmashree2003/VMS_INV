(function () {
    'use strict';
    angular.module('app').controller('VmsAuditReportsController', VmsAuditReportsController)
    VmsAuditReportsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function VmsAuditReportsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {
        
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        if (transnumconfigsettings != null && transnumconfigsettings.length > 0) {
            for (var i = 0; i < transnumconfigsettings.length; i++) {
                if (transnumconfigsettings[i].imN_Flag === "PMTV") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length) {

            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;

        } else {
            paginationformasters = 10;
        }
        var configsettingsnew = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.totalgrid = [];
        $scope.submitted = false;

        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };


        $scope.loaddata = function () {

            var pageid = 2;
            apiService.getURI("VmsAuditReports/getalldata", pageid).then(function (promise) {            
                $scope.roleflag = promise.roleflag;
                $scope.companydropdown = promise.mI_List;
                //if ($scope.companydropdown != null && $scope.companydropdown.length > 0) {
                //    angular.forEach($scope.companydropdown, function (uem) {
                //        uem.selected = true;
                //    });
                //    $scope.companyselectedAll = true;
                //}

            });
        };

        /* Company SELECTION */
        $scope.all_checkMIname = function (companyselectedAll) {
            var toggleStatus = companyselectedAll;
            angular.forEach($scope.companydropdown, function (itm) {
                itm.selected = toggleStatus;
            });
          
            $scope.designationselectedAll = false;
            if ($scope.companyselectedAll == true) {
                $scope.togchkbxminame();
            }
            else {
                $scope.departmentdropdown = [];
            }
         
        };
        $scope.togchkbxminame = function (groupType) {
          
            $scope.companyselectedAll = $scope.companydropdown.every(function (itm) {
                return itm.selected;
            });
            $scope.compNylist = [];
            $scope.departmentdropdown = [];

            if ($scope.companydropdown != null && $scope.companydropdown.length > 0) {
                $scope.companyselectedAll = $scope.companydropdown.every(function (em) {
                    if (em.selected === true) {
                        $scope.compNylist.push({
                            MI_Id: em.MI_Id
                        });
                    }
                });
                var data = {
                    "comapanylist": $scope.compNylist
                };
                apiService.create("VmsAuditReports/Getdata", data).then(function (promise) {
                    $scope.departmentdropdown = [];
                    if (promise.departmentdropdown != null && promise.departmentdropdown.length > 0) {
                        $scope.departmentdropdown = promise.departmentdropdown;
                        //angular.forEach($scope.departmentdropdown, function (uem) {
                        //    uem.selected = true;
                        //});
                        
                    }

                });
            }

           
        };
        /* Company SELECTION */

        $scope.all_checkdept = function (departmentselectedAll) {
            var toggleStatus = departmentselectedAll;
            angular.forEach($scope.filterValue5, function (itm) {
                itm.selected = toggleStatus;
            });
        }
        $scope.togchkbxdept = function (groupType) {
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {
                return itm.selected;
            });
           
        };
        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("asdf").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 10000);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.savedata = function () {
            $scope.submitted = true;

            var data = {
                "ISMCLTPRP_Id": dd.ismcltprP_Id
            };
            apiService.create("CheckBookEntry/viewdetails", data).then(function (promise) {


            });
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };



    }
})();
