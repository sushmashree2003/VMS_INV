(function () {
    'use strict';
    angular.module('app').controller('ISMInventoryDashboardController', ISMInventoryDashboardController);
    ISMInventoryDashboardController.$inject = ['$compile', '$rootScope', '$scope', '$http', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', '$window', '$timeout', 'Excel'];
    function ISMInventoryDashboardController($compile, $rootScope, $scope, $state, $http, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, $timeout, Excel) {

        $scope.employeedetails = [];
        $scope.Departmentname = [];
        $scope.HRMDC_ID = 0;
        $scope.Activate = "";
        $scope.loaddata = function () {
            $scope.employeedetails = [];
            $scope.currentPage = 1;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("ISMDashboard/getdetails", pageid).then(function (promise) {
                $scope.Departmentname = promise.departmentname;
                $scope.TotalActive = promise.departmentcount[0].TotalActive;
                $scope.TotalDective = promise.departmentcount[0].TotalDective;
                $scope.HRMDC_ID = $scope.Departmentname[0].HRMDC_ID;
              
            });
        };

      
        $scope.departments = function () {

            $('#departments').modal('show');
        }

        $scope.getActiveMembers = function (item) {
            $scope.Activate = "";
            $scope.employeedetails = [];
            $scope.Activate = item;
          
            $scope.employeedetails = [];
            var data = {
                "HRMDC_ID": $scope.Departmentname[0].HRMDC_ID,                     
                "Flags": $scope.Activate,
            }
            apiService.create("ISMDashboard/getemployeedetail", data).then(function (promise) {
                $scope.employeedetails = promise.employeedetails;

                $scope.employeeid = [];
                if ($scope.employeedetails != null && $scope.employeedetails.length > 0) {
                    angular.forEach($scope.employeedetails, function (dev) {
                        if ($scope.employeeid.length === 0) {
                            $scope.employeeid.push(dev);
                        }

                        else if ($scope.employeeid.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.employeeid, function (emp) {
                                if (emp.HRME_Id === dev.HRME_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.employeeid.push(dev);
                            }
                        }
                    });

                    console.log($scope.employeeid);
                    angular.forEach($scope.employeeid, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.employeedetails, function (dd) {
                            if (dd.HRME_Id === ddd.HRME_Id) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.RecordDetails = $scope.templist;

                    });
                }

            });
            $('#departments').modal('show');
        }
        

        $scope.OnClickemployeedetails = function (item) {
            $scope.employeedetails = [];
            var data = {
                "HRMDC_ID": item.HRMDC_ID,
                "Flags": $scope.Activate,
            }
            apiService.create("ISMDashboard/getemployeedetail", data).then(function (promise) {
                $scope.employeedetails = promise.employeedetails;

                $scope.employeeid = [];
                if ($scope.employeedetails != null && $scope.employeedetails.length > 0) {
                    angular.forEach($scope.employeedetails, function (dev) {
                        if ($scope.employeeid.length === 0) {
                            $scope.employeeid.push(dev);
                        }

                        else if ($scope.employeeid.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.employeeid, function (emp) {
                                if (emp.HRME_Id === dev.HRME_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.employeeid.push(dev);
                            }
                        }
                    });
                    
                    console.log($scope.employeeid);
                    angular.forEach($scope.employeeid, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.employeedetails, function (dd) {
                            if (dd.HRME_Id === ddd.HRME_Id) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.RecordDetails = $scope.templist;

                    });
                }

            });


        };

    }
})();


