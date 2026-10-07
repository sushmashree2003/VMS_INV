

(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISMAluminiDashboardController', ISMAluminiDashboardController);
    ISMAluminiDashboardController.$inject = ['$rootScope', '$scope', '$http', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', '$window'];
    function ISMAluminiDashboardController($rootScope, $scope, $state, $http, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {


        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("ISMDashboard/getdetails", pageid).then(function (promise) {


                if (promise.emp_deatils.length > 0 && promise.emp_deatils != null) {
                    $scope.emp_deatils = promise.emp_deatils;
                    $scope.HRME_EmailId = $scope.emp_deatils[0].HRME_EmailId;
                    $scope.employeename = $scope.emp_deatils[0].employeename;
                    $scope.HRMD_DepartmentName = $scope.emp_deatils[0].HRMD_DepartmentName;
                    $scope.HRMDES_DesignationName = $scope.emp_deatils[0].HRMDES_DesignationName;
                    $scope.MI_Name = $scope.emp_deatils[0].MI_Name;
                    $scope.HRME_EmployeeCode = $scope.emp_deatils[0].HRME_EmployeeCode;
                    $scope.HRME_MobileNo = $scope.emp_deatils[0].HRME_MobileNo;
                    $scope.HRME_DOJ = $scope.emp_deatils[0].HRME_DOJ;
                    $scope.HRME_DOL = $scope.emp_deatils[0].HRME_DOL;
                    $scope.HRME_AadharCardNo = $scope.emp_deatils[0].HRME_AadharCardNo;
                    $scope.HRME_PANCardNo = $scope.emp_deatils[0].HRME_PANCardNo;
                    $scope.HRME_LeavingReason = $scope.emp_deatils[0].HRME_LeavingReason;

                }

            });

        };



      
    }
})();




