(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_Employee_Absent_Present_ListController', ISM_Employee_Absent_Present_ListController)

    ISM_Employee_Absent_Present_ListController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function ISM_Employee_Absent_Present_ListController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {

  
        $scope.onLoadGetData = function () {

            $scope.getcurrentdate = new Date();
            
            $scope.getpresentabsentdetails();
            
        }




        $scope.getpresentabsentdetails = function () {            

            var data = {
                "FOEP_PunchDate": new Date($scope.getcurrentdate).toDateString()                  
                };

                apiService.create("ISMDashboard/getdatedetails", data).then(function (promise) {

                $scope.GetReport = promise.getReport;

                    if (promise !== null) {
                        if (promise.GetReport.length>0) {

                            
                        } else {
                            swal("Record Not Found");
                        }
                        
                    }
                });

           
        };


        $scope.approverName = function (obj) {

            var data = {
                "HRME_Id": $scope.hrmE_Id
            };
            apiService.create("OnlineLeaveApplication/Leaveapprover", data).then(function (promise) {
                if (promise.approverList != null && promise.approverList != undefined) {
                    $scope.approverList = promise.approverList;

                    $scope.employeeid = [];

                    angular.forEach($scope.approverList, function (dev) {
                        if ($scope.employeeid.length === 0) {
                            $scope.employeeid.push({
                                HRLA_Id: dev.HRLA_Id,
                                HRML_LeaveName: dev.HRML_LeaveName,
                            });
                        } else if ($scope.employeeid.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.employeeid, function (emp) {
                                if (emp.HRLA_Id === dev.HRLA_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.employeeid.push({
                                    HRLA_Id: dev.HRLA_Id,
                                    HRML_LeaveName: dev.HRML_LeaveName,
                                });
                            }
                        }
                    });

                    angular.forEach($scope.employeeid, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.approverList, function (dd) {
                            if (dd.HRLA_Id === ddd.HRLA_Id) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.plannerdetails = $scope.templist;
                    });

                }
            });
        };



    }
})();

