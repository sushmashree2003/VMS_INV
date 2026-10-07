(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_VendorCategoryMasterController', INV_VendorCategoryMasterController);
    INV_VendorCategoryMasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function INV_VendorCategoryMasterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //-------------------------------------------------------------------

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            $scope.vcM_Id = 0;
            var pageid = 2;
            apiService.getURI("INV_VendorCategoryMaster/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_vendorcategory = promise.vendorCategoryMasterArray;
                    $scope.presentCountgrid = $scope.get_vendorcategory ? $scope.get_vendorcategory.length : 0;
                });
        };
        //---------------------------------Save--------------------------------------------
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.vcM_Vendorname) {
                    $scope.vcM_Vendorname = $scope.vcM_Vendorname.trim();
                }
                if (!$scope.vcM_Vendorname || $scope.vcM_Vendorname === '') {
                    swal('Please Enter Valid Vendor Category Name');
                    return;
                }
                var data = {
                    "VCM_Vendorname": $scope.vcM_Vendorname,
                    "VCM_Id": $scope.vcM_Id || 0
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("INV_VendorCategoryMaster/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.vcM_Id == 0 || promise.vcM_Id < 0 || promise.vcM_Id == undefined) {
                            swal('Record saved successfully');
                        }
                        else if (promise.vcM_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate' || promise.saveorupdate == 'Vendor Name Already Exists') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.vcM_Id == 0 || promise.vcM_Id < 0 || promise.vcM_Id == undefined) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.vcM_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.deactive = function (item) {
            var activeFlag = (item.vcM_Activeflag !== undefined && item.vcM_Activeflag !== null) ? item.vcM_Activeflag :
                             (item.VCM_Activeflag !== undefined && item.VCM_Activeflag !== null) ? item.VCM_Activeflag : item.vcm_Activeflag;
            var dystring = activeFlag ? "Deactivate" : "Activate";

            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        var data = {
                            "VCM_Id": item.vcM_Id || item.VCM_Id || item.vcm_Id,
                            "VCM_Activeflag": activeFlag
                        };
                        apiService.create("INV_VendorCategoryMaster/deactive", data).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else if (promise.saveorupdate) {
                                    swal(promise.saveorupdate);
                                }
                                else {
                                    swal("Failed to " + dystring + " Record");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.edit = function (item) {
            $scope.vcM_Vendorname = item.vcM_Vendorname;
            $scope.vcM_Id = item.vcM_Id;
        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

    }
})();
