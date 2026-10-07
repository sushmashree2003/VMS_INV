(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClientHRMSOverallReportController', ClientHRMSOverallReportController)

    ClientHRMSOverallReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function ClientHRMSOverallReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        
        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("EmployeeStrengthReport/getalldetails", pageid).then(function (promise) {


        //        if (promise.groupTypedropdown !== null && promise.groupTypedropdown.length > 0) {
        //            $scope.groupTypedropdown = promise.groupTypedropdown;
        //            $scope.groupTypeselectedAll = true;
        //            $scope.GetEmployeeBygroupTypeAll($scope.groupTypeselectedAll);

        //        }


        //        if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
        //            $scope.departmentdropdown = promise.departmentdropdown;
        //            $scope.departmentselectedAll = true;
        //            $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
        //        }

        //        if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
        //            $scope.designationdropdown = promise.designationdropdown;

        //            $scope.designationselectedAll = true;
        //            $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
        //        }


        //        //Set From date and To date
        //        $scope.Employee.FromDate = new Date();

        //        $scope.Employee.ToDate = $scope.Employee.FromDate;
        //        $scope.minDateTo = new Date(
        //            $scope.Employee.ToDate.getFullYear(),
        //            $scope.Employee.ToDate.getMonth(),
        //            $scope.Employee.ToDate.getDate());


        //    })
        //}

        

    }


})();