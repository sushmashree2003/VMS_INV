(function () {
    'use strict';
    angular
.module('app')
        .controller('categoryController', categoryController)

    categoryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http','superCache']
    function categoryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.sortKey = 'amC_Id';
        $scope.sortReverse = true;

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



        $scope.getAllDetails = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("MasterCategory/getalldetails", pageid).
            then(function (promise) {
                $scope.students = promise.categoryList;
                $scope.institution = promise.institutionDrpdwn;
                $scope.MI_Id = promise.institutionDrpdwn[0].mI_Id;
                $scope.presentCountgrid = $scope.students.length;
                $scope.typelist = [
               { id: '1', name: "School" },
               { id: '2', name: "College" }
                ];
            })
            //$scope.sort = function (keyname) {
            //    $scope.sortKey = keyname;   //set the sortKey to the param passed
            //    $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            //}
        }

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }


        $scope.edit = function (employee) {
            $scope.editEmployee = employee.amC_Id;
            var templId = $scope.editEmployee;

            apiService.getURI("MasterCategory/getdetails", templId).
            then(function (promise) {
                $scope.address = promise.categoryList[0].amC_Address.split(',');
                $scope.MI_Id = promise.categoryList[0].mI_Id;
                $scope.AMC_Id = promise.categoryList[0].amC_Id;
                $scope.AMC_Name = promise.categoryList[0].amC_Name;
                $scope.Line1 = $scope.address[0];
                $scope.Line2 = $scope.address[1];
                $scope.Line3 = $scope.address[2];
                $scope.Line4 = $scope.address[3];
                $scope.AMC_RegNoPrefix = promise.categoryList[0].amC_RegNoPrefix;
                $scope.AMC_Type = promise.categoryList[0].amC_Type;
            })
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


        $scope.submitted = false;
        $scope.savetmpldata = function () {
            
           
            if ($scope.myForm.$valid) {
                var data = {
                    "MI_Id": $scope.MI_Id,
                    "AMC_Id": $scope.AMC_Id,
                    "AMC_Name": $scope.AMC_Name,
                    "Line1": $scope.Line1,
                    "Line2": $scope.Line2,
                    "Line3": $scope.Line3,
                    "Line4": $scope.Line4,
                    "AMC_Type": $scope.AMC_Type,
                    "AMC_RegNoPrefix": $scope.AMC_RegNoPrefix
                }
                apiService.create("MasterCategory/", data)

                .then(function (promise) {
                    if (promise.returnval == "Add") {
                        swal('Record Saved Successfully');
                        $state.reload();
                    }
                    else if (promise.returnval == "Update") {
                        swal('Record Updated Successfully');
                        $state.reload();
                    }
                    else if (promise.returnval == "false") {
                        swal('Record Not Saved/Updated successfully');
                    }
                    else if (promise.returnval == "Duplicate") {
                        swal('Category Already Exist');
                       // $state.reload();
                        return;
                    }
                    else {
                        swal('Operation Failed');
                       // $state.reload();
                    }

                })
            }
            else {
               
                $scope.submitted = true;
            }
            //else {
            //    swal("please Select the Mandatiory Fields..")
            //}
        };

        $scope.deactive = function (category) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            
            var mgs = "";
            if (category.amC_ActiveFlag == false) {

                mgs = "Activate";

            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " Category?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
             function (isConfirm) {
                 if (isConfirm) {
                     apiService.create("MasterCategory/deactivate", category).
                     then(function (promise) {
                         
                         swal(promise.returnval);
                         $state.reload();
                     })
                 }
                 else {
                     swal("Cancelled");
                 }
             })
        }

        $scope.Clearid = function () {
            $state.reload();
        }

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            
            return (angular.lowercase(obj.amC_Name)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.mI_Name)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.amC_Type)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
       
    
        }
  
})();

