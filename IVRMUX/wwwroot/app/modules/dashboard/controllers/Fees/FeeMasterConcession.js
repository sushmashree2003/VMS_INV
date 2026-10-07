
(function () {
    'use strict';
    angular
.module('app')
.controller('FeeMasterConcessionController', FeeMasterConcessionController)

    FeeMasterConcessionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function FeeMasterConcessionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.masterlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.searchValue = "";


        $scope.BindData = function () {
            
            var pageid = 2;
            apiService.getURI("FeeMasterConcession/getdata", pageid).then(function (promise) {
                if (promise != null) {

                    if (promise.savedata.length > 0) {
                        $scope.mastervehicle = promise.savedata;
                        $scope.totcountfirst = $scope.mastervehicle.length;
                        $scope.masterlist = true;
                    }
                    else {
                        swal("No Records Found")
                        $scope.masterlist = false;
                    }

                }
                else {
                    swal("No Records Found")
                }
            })

        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.limit = function () {
            if ($scope.fmcC_ConcessionApplLimit != null && $scope.fmcC_ConcessionApplLimit != '' && $scope.fmcC_ConcessionApplLimit != undefined) {
                if (parseInt($scope.fmcC_ConcessionApplLimit) > 100) {
                    swal("Number Should be less than or equal to 100");
                    $scope.fmcC_ConcessionApplLimit = '';
                }             
            }

          
        }

        //---Save--//
        $scope.savedata = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "FMCC_Id": $scope.fmcC_Id,
                    "FMCC_ConcessionName": $scope.fmcC_ConcessionName,
                    "FMCC_ConcessionApplLimit": $scope.fmcC_ConcessionApplLimit
                }
                apiService.create("FeeMasterConcession/savedata", data).then(function (promise) {
                    if (promise.message == "Add") {
                        if (promise.retrunval == true) {
                            swal("Record Saved Successfully");
                        }
                        else {
                            swal("Record Not Saved");
                        }
                    }
                    else if (promise.message == "Update") {
                        if (promise.retrunval == true) {
                            swal("Record Updated Successfully");
                        }
                        else {
                            swal("Record Not Updated");
                        }
                    }
                    else if (promise.message == "Mapped") {
                        swal("You Can Not Edit This Record It Already Mapped");
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exists");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        }



        //--Sorting--//     
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.fmcC_ConcessionName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.fmcC_ConcessionApplLimit)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }


        //---Edit Data--//
        $scope.edit = function (user) {
            var data = {
                "FMCC_Id": user.fmcC_Id
            }

            apiService.create("FeeMasterConcession/editdata", data).then(function (Promise) {
                if (Promise != null) {
                    $scope.fmcC_Id = Promise.editdata[0].fmcC_Id;
                    $scope.fmcC_ConcessionName = Promise.editdata[0].fmcC_ConcessionName;
                    $scope.fmcC_ConcessionApplLimit = Promise.editdata[0].fmcC_ConcessionApplLimit;
                }
            })
        }
        //--Active Deactive--//
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.fmcC_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {
                apiService.create("FeeMasterConcession/activedeactive/", user).
                then(function (promise) {
                    if (promise.message != null) {
                        swal(promise.message);
                    }
                    else {
                        if (promise.returnval == true) {
                            swal(confirmmgs + " " + "Successfully");
                            $state.reload();
                        }
                        else {
                            swal(confirmmgs + " " + " Successfully");
                            $state.reload();
                        }
                    }
                })
            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
            $state.reload();
        });
        }

        //-Clear-//
        $scope.clearid = function () {
            $state.reload();
        };

        //--Sorting--//     
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }




    };



})();













