
(function () {
    'use strict';
    angular
        .module('app')
        .controller('StaffAreaActivityRecordController', StaffAreaActivityRecordController);
    StaffAreaActivityRecordController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$compile'];
    function StaffAreaActivityRecordController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $compile) {

      
        $scope.searchValueI = "";

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;

        }
        else {
            paginationformasters = 10;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //-------------------------------------------------------------------
        $scope.currentPageI = 1;
        $scope.itemsPerPageI = paginationformasters;
        $scope.search = "";
        $scope.presentCountgridI = "";
        $scope.loaddata = function () {

            //this is for live 
            var newURL = window.location.href;            var splitURL = newURL.split("?Id=");            var URLS = splitURL[1];            let decoded = decodeURIComponent(URLS);
            $scope.HRMA_QRCODEPath = decoded;
            //  $scope.HRMA_QRCODEPath = 'VapsCorporateHouse/GroundFloor/FrontOfficeDesk/01';
            var data = {
                "HRMA_QRCODEPath": $scope.HRMA_QRCODEPath,
            }



            // this for testing 
            //$scope.HRMA_QRCODEPath = 'VapsCorporateOffice/FirstFloor/commonfloor/FCF2';
            //var data = {
            //    "HRMA_QRCODEPath": $scope.HRMA_QRCODEPath,
            //}
           
            apiService.create("StaffAreaActivityRecord/savedetails", data).
                then(function (promise) {

                    if (promise.qRcodeActivityDetails.length > 0 && promise.qRcodeActivityDetails != null) {
                        $scope.QRcodeActivityDetails = promise.qRcodeActivityDetails;
                        $scope.BuildingName = $scope.QRcodeActivityDetails[0].hrmB_BuildingName;
                        $scope.FloorName = $scope.QRcodeActivityDetails[0].hrmF_FloorName;
                        $scope.AreaName = $scope.QRcodeActivityDetails[0].HRMAR_AreaName;
                        $scope.ActivityName = $scope.QRcodeActivityDetails[0].hrmaracT_ActivityName;
                        $scope.HRMARACTT_Time = $scope.QRcodeActivityDetails[0].hrmaractT_Time;
                        $scope.ActivityDuration = $scope.QRcodeActivityDetails[0].hrmaractT_ActivityDuration;
                        $scope.HRMARACTSTMAP_Date = $scope.QRcodeActivityDetails[0].hrmaractstmaP_Date;
                    }
                });
        };

        $scope.toggleAll = function (all) {
            var toggleStatus = all;
            angular.forEach($scope.QRcodeActivityDetails, function (itm) {
                itm.selected = toggleStatus;

            });
        }
        $scope.optionToggled1 = function (SelectedStudentRecord, index) {

            $scope.all = $scope.QRcodeActivityDetails.every(function (options) {

                return options.selected;
            });
        }
        //save
        $scope.QRclick = function () {

            $scope.tasklistarray = [];
            angular.forEach($scope.QRcodeActivityDetails, function (qq) {
                if (qq.selected == true) {
                    $scope.tasklistarray.push(qq)
                }
            });
         
            var data = {
                "HRMA_QRCODEPath": $scope.HRMA_QRCODEPath,
                "tasklistarray": $scope.tasklistarray
            }
            apiService.create("StaffAreaActivityRecord/Activitysavedetails", data).
                then(function (promise) {
                    if (promise.returnval == "Add") {
                        swal("Activity saved Successfully....")
                        $state.reload();
                    }
                    else {
                        swal("Not SActivity saved Successfully...., Try Again.")
                    }
                   
                })
        }
        
        $scope.interacted = function (field) {
            return $scope.submittedUG;
        };
      
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        
        $scope.searchValue = '';
        $scope.cancel = function () {
            $state.reload();
        }
        
    }
})();