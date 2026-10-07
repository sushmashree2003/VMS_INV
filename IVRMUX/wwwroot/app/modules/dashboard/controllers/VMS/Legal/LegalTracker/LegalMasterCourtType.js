(function () {
    'use strict';
    angular
        .module('app')
        .controller('LegalMasterCourtTypeController', LegalMasterCourtTypeController)
    LegalMasterCourtTypeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter', '$q']
    function LegalMasterCourtTypeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter, $q) {
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
       
        $scope.Onloadcasetype = function () {
            var pageid = 1;
            apiService.getURI("ivrm_LegalProcess/Onloadcasetype", pageid).then(function (promise) {
                $scope.CourtType = promise.category1;                
                $scope.gridcasetype = promise.gridcasetype;
            });
        }
       
        //Save for casemastertype

        $scope.legalcasetypesave = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {               
                var data = {
                    "MCST_Name": $scope.obj1.mcsT_Name,
                    "MCST_Id": $scope.obj1.mcsT_Id,
                    "MCST_ActiveFlag": $scope.mcsT_ActiveFlag
                };
                apiService.create("ivrm_LegalProcess/legalcasetypesave", data).then(function (promise) {
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
     
        
        $scope.editcasetype = function (employee1) {
            $scope.obj1.mcsT_Name = employee1.mcsT_Name;
            $scope.mcsT_ActiveFlag = employee1.mcsT_ActiveFlag;
            $scope.obj1.mcsT_Id = employee1.mcsT_Id;         
        }        

        $scope.Clearid = function () {
            $state.reload();
        };
    }


})();