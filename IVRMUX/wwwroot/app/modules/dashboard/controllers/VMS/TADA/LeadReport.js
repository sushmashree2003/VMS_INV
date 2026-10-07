(function () {
    'use strict';
    angular.module('app').controller('ISM_ExtensionController', ISM_ExtensionController);
    ISM_ExtensionController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter'];
    function ISM_ExtensionController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
      
        $scope.page3 = "page3";
        $scope.currentPage3 = 1;
        $scope.itemsPerPage3 = 10

        $scope.submitted = false;
        $scope.dflg = false;
        
        $scope.obj = {};
       
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.currentPageP = 1;
            $scope.page1 = "page1";
            $scope.page2 = "page2";
            $scope.itemsPerPage = 10;
            $scope.itemsPerPagep = 10;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("TADAApproval/getalldata", pageid).then(function (promise) {
                $scope.get_Emplist = promise.getReport;
            });

        };

       
        //======================================== On Month Change

        $scope.obj.start_Date = new Date();

        $scope.obj.end_Date = new Date();

        $scope.report = function () {
            $scope.Datacategorynew = [];
            angular.forEach($scope.get_deptlist, function (em) {
                if (em.empck === true) {
                    $scope.deptlistarray.push(em);
                }
            });
            var data = {
                "HRME_Id": $scope.obj.HRME_Id.HRME_Id,
                "VTADAA_FromDate": new Date($scope.obj.start_Date).toDateString(),
                "VTADAA_ToDate": new Date($scope.obj.end_Date).toDateString(),
            };
            apiService.create("TADAApproval/getfile", data).then(function (promise) {
                if (promise.getReport != null &&  promise.getReport.length > 0) {
                    $scope.Datacategorynew = promise.getReport;
                    $scope.employeeid = [];
                    angular.forEach($scope.Datacategorynew, function (dev) {
                        if ($scope.employeeid.length === 0) {
                            $scope.employeeid.push(dev);
                        }
                        else if ($scope.employeeid.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.employeeid, function (emp) {
                                if (emp.IERCLE_Id === dev.IERCLE_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.employeeid.push(dev);
                            }
                        }
                    })
                    angular.forEach($scope.employeeid, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.Datacategorynew, function (dd) {
                            if (dd.IERCLE_Id === ddd.IERCLE_Id) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.Datacategorynew = $scope.templist;

                    });
                }
                else {
                    swal("No Record  Found..... !!");
                }
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        //===========Clear Field
        $scope.cancel = function () {
            $state.reload();
        };

    }
})();

