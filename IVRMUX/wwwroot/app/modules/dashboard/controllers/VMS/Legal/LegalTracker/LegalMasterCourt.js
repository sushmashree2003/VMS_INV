(function () {
    'use strict';
    angular
        .module('app')
        .controller('LegalProcessCourtController', LegalProcessCourtController)
    LegalProcessCourtController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter', '$q']
    function LegalProcessCourtController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter, $q) {
        $scope.obj = {};
        $scope.obj1 = {};
        $scope.cat = {};

        

        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        //$scope.Clearid = function () {
        //    $state.reload();
        //    $scope.loaddata();
        //}

        $scope.Onloaddata = function () {
            var pageid = 1;
            apiService.getURI("ivrm_LegalProcess/onloaddata", pageid).then(function (promise) {              
                $scope.dashdata = promise.admindata;
                $scope.CourtType = promise.category1;                               
                $scope.griddata = promise.griddata;

            });
        }

        $scope.Onloadcourtdata = function () {
            var pageid = 1;
            apiService.getURI("ivrm_LegalProcess/Onloadcourtdata", pageid).then(function (promise) {               
                $scope.CourtType = promise.category1;
                $scope.gridcourt = promise.gridcourt;
            });
        }

        //Added by Rohan

        //$scope.onradiobuttonclick = function (data) {
        //    var input = {
        //        "LNC_Id": data.lnC_Id
        //    }
        //    apiService.create("ivrm_LegalProcess/onclickloaddata1", input).then(function (promise) {

        //        $scope.admindata = promise.admindata;

        //    });
        //} 

        $scope.legalcourtsave = function () {
            $scope.submitted = true;                       

            if ($scope.myForm.$valid) {
                              
                var data = {
                    "MC_Id": $scope.obj1.MC_Id,
                    "MC_Name": $scope.obj1.mC_Name,
                    "MCT_Id": $scope.obj.MCT_Id,                  
                    "MC_ActiveFlag": $scope.mC_ActiveFlag
                };
                apiService.create("ivrm_LegalProcess/legalcourtsave", data).then(function (promise) {
                    if (promise.returnvalue !== "") {
                        if (promise.returnvales == "Update") {
                            swal("Record Updated Successfully");
                        } else if (promise.returnvales == "Add") {
                            swal("Record Saved Successfully");
                        } else if (promise.returnvales == "false") {
                            swal("Record Not Saved/Updated successfully");
                        } else if (promise.returnvales === "Duplicate") {
                            swal("Category Already Exist");
                        }
                        else if (promise.update == "updated") {
                            swal("Record Saved Successfully", "", "success");
                        }
                        $state.reload();
                    } else {
                        swal("Operation Failed");
                        $state.reload();
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.editdata = function (employee) {
            $scope.obj1.mC_Name = employee.mC_Name;
            $scope.MC_ActiveFlag = employee.mC_ActiveFlag;
            $scope.obj1.MCT_Id = employee.mcT_Id; 
            $scope.obj1.MC_Id = employee.mC_Id;
        }
                     
        $scope.Clearid = function () {
            $state.reload();
        };
    }


})();