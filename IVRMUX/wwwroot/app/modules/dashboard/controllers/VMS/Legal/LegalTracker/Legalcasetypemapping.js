(function () {
    'use strict';
    angular
        .module('app')
        .controller('LegalcasetypemappingController', LegalcasetypemappingController)
    LegalcasetypemappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter', '$q']
    function LegalcasetypemappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter, $q) {
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

        $scope.Onloadcasetypemapping = function () {
            var pageid = 1;
            
            apiService.getURI("ivrm_LegalProcess/Onloadcasetypemapping", pageid).then(function (promise) {
                $scope.CourtType = promise.category1;
                $scope.courtcomplexdropdown = promise.courtcomplex;
                $scope.courtestablishmentdropdown = promise.courtestablishment;
                $scope.casetypedropdown = promise.casetype;
                $scope.gridcasetypemapping = promise.gridcasetypemapping;
            });
        }     

        $scope.Legalcasetypemappingsave = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {                
                var data = {
                    "CCM_Court": $scope.obj1.ccM_Court.mC_Name,
                    "CCM_Case": $scope.obj1.ccM_Case.mcsT_Name,
                    "CCM_Id": $scope.obj1.ccM_Id,
                    "CCM_ActiveFlag": $scope.ccM_ActiveFlag
                };
                apiService.create("ivrm_LegalProcess/Legalcasetypemappingsave", data).then(function (promise) {
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
         

        $scope.editcasedata = function (employee) { 
            //$scope.obj.MCT_Id = employee.obj.mcT_Id;
            $scope.CCM_Court = employee.ccM_Court;
            $scope.obj1.CCM_Case = employee.ccM_Case;
            $scope.CCM_ActiveFlag = employee.ccM_ActiveFlag;
            $scope.obj1.CCM_Id = employee.ccM_Id;
        }

        $scope.Clearid = function () {
            $state.reload();
        };
    }


})();