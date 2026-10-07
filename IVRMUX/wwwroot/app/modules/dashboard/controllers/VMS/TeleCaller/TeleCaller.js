(function () {
    'use strict';
    angular
        .module('app')
        .controller('TeleCaller_DemoListController', TeleCaller_DemoListController)

    TeleCaller_DemoListController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function TeleCaller_DemoListController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.sortReverse = true;
        //  $scope.ISMSLEDM_DemoScheduledBy = 1860;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        $scope.From = new Date();
        $scope.to = new Date();
        //$scope.todate = new Date();

        $scope.gettodate = function () {

            $scope.minDatemf = new Date($scope.fromdate);
            $scope.maxDatemf = new Date();
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
        //============= + - row===============
        $scope.demonstration = [{ itrS_Id: 'trans1' }];
        if ($scope.demonstration.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addpirows = function () {
            if ($scope.demonstration.length > 1) {
                for (var i = 0; i === $scope.demonstration.length; i++) {
                    var id = $scope.demonstration[i].itrS_Id;
                    var lastChar = id.substr(id.length - 1);
                    $scope.cnt = parseInt(lastChar);
                }
            }
            $scope.cnt = $scope.cnt + 1;
            $scope.tet = 'trans' + $scope.cnt;
            var newItemNo = $scope.cnt;
            $scope.demonstration.push({ 'itrS_Id': 'trans' + newItemNo });

        };
        $scope.removepirows = function (index, data) {
            var newItemNo = $scope.demonstration.length - 1;
            $scope.demonstration.splice(index, 1);

            if (data.amstB_Id > 0) {
                $scope.Deletepirows(data);
            }

        };

        $scope.editsub = false;
       // $scope.disableRadioButtons = false
        $scope.editdata = function (caller) {
            $scope.editsub = true;
           
            $scope.ClientName = caller.ISMSLE_LeadName,
                //$scope.DemoDate = $scope.DemoDate,
                $scope.Fromdate = $scope.Fromdate,
                $scope.Todate = $scope.Todate,
                $scope.ISMSLE_Id = caller.ISMSLE_Id,
                $scope.ISMSLEDM_Id = caller.ISMSLEDM_Id,
                $scope.ISMSMPR_Id = caller.ISMSMPR_Id,
                $scope.RemarksFlag = caller.RemarksFlag
            if ($scope.RemarksFlag == 'Executed') {
                $scope.RemarksFlag = 1;
            }
            else if ($scope.RemarksFlag == 'NonExecuted') {
                $scope.RemarksFlag = 0;
            }
            else {
                $scope.RemarksFlag = "";
            }

            
            //if ($scope.RemarksFlag == 0 || $scope.RemarksFlag == 1) {
            //    $scope.disableRadioButtons = true;
            //}
            //else {
            //    $scope.disableRadioButtons = false;
            //}

            $scope.TelecallerRemarks = caller.TelecallerRemarks,
                $scope.ClientName = caller.ISMSLE_LeadName,
                $scope.contactPerson = caller.ISMSLE_ContactPerson,
                $scope.contactNumber = caller.ISMSLE_ContactNo
          

        };

        

        $scope.savetmpldata = function () {



            var data = {
                // "Flag": $scope.Flag,
                //"Remarks": $scope.Remarks,
                //"DemoDate": $scope.DemoDate,
                "FromDate": $scope.Fromdate,
                "Todate": $scope.Todate,
                "ISMSLE_Id": $scope.ISMSLE_Id,
                "ISMSLEDM_Id": $scope.ISMSLEDM_Id,
                "ISMSMPR_Id": $scope.ISMSMPR_Id,
                "RemarksFlag": $scope.RemarksFlag,
                "TelecallerRemarks": $scope.TelecallerRemarks,



            }
            apiService.create("TeleCaller_DemoList/UpdateTeleCaller_List", data).then(function (promise) {
                if (promise.message == "success") {
                    swal("Updated successfully");
                }
                else {
                    swal("Updated Failed");
                }

                $state.reload();
            });



        };

        $scope.swalcancel = function () {
            //$('#myModalswal').modal('hide');
            //$('.modal-backdrop').remove();
            //swal("Save Data Cancelled!");
            $state.reload();
        };
        $scope.search = function () {
            var data = {
                "FromDate": $scope.Fromdate,
                "Todate": $scope.Todate
            }
            apiService.create("TeleCaller_DemoList/TeleCaller_List", data).
                then(function (promise) {
                    $scope.presentationClientlist = promise.presentationClientlist;

                  
                })
        };


    }

})();



