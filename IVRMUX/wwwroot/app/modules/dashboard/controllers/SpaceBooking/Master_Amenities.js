(function () {
    'use strict';
    angular
        .module('app')
        .controller('Master_AmenitiesController', master_AmenitiesController)

    master_AmenitiesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function master_AmenitiesController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.sortKey = 'MA_ID';
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



        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        //get data
        $scope.load_data = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("Space_Amenities/get_master_amenities", pageid)
                .then(function (promise) {
                    $scope.master_amenities_list = promise.master_amenities_list;
                    $scope.presentCountgrid = $scope.master_amenities_list.length;

                })
        }





        // edit
        $scope.edibl = {}
        $scope.edit = function (bil) {
            $scope.edibl = bil.hrmaM_Id;
            var pageid = $scope.edibl;
            apiService.getURI("Space_Amenities/Edit_MasterAmenities", pageid).
                then(function (promise) {
                    $scope.Id = promise.master_amenities_details[0].hrmaM_Id;
                    $scope.HRMAM_AmenitiesName = promise.master_amenities_details[0].hrmaM_AmenitiesName;
                    $scope.HRMAM_AmenitiesDes = promise.master_amenities_details[0].hrmaM_AmenitiesDes;
                    $scope.HRMAM_Price = promise.master_amenities_details[0].hrmaM_Price;
                    $scope.cost_check = promise.master_amenities_details[0].hrmaM_PriceApplFlg;
                    

                })
        };


        //save
        $scope.submitted = false;
        $scope.savedata = function () {

            $scope.submitted = true;
            $scope.HRMAM_Price1=0
            if ($scope.cost_check == true) {
                $scope.HRMAM_Price1 = $scope.HRMAM_Price
            }
            else {
                $scope.cost_check = false;
            }
            if ($scope.myForm.$valid) {
                var data = {
                    "HRMAM_AmenitiesName": $scope.HRMAM_AmenitiesName,
                    "HRMAM_AmenitiesDes": $scope.HRMAM_AmenitiesDes,
                    "HRMAM_Price": $scope.HRMAM_Price1,
                    "HRMAM_PriceApplFlg": $scope.cost_check,
                    "HRMAM_Id": $scope.Id,
                }
                apiService.create("Space_Amenities/SaveEdit_MasterAmenities", data).
                    then(function (promise) {
                        if (promise.returnvalue !== "") {

                            if (promise.returnvalue === "Update") {
                                swal('Record Updated Successfully');
                                $state.reload();
                            }
                            else if (promise.returnvalue === "Add") {
                                swal('Record Saved Successfully');
                                $state.reload();
                            }


                        }

                        else {
                            swal('Operation Failed');
                            $state.reload();
                        }
                    })

            }
            else {

                $scope.submitted = true;
            }

        };
        $scope.Clearid = function () {
            $state.reload();
        }
        //deactive
        $scope.deactive = function (bL, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (bL.hrmaM_ActiveFlag === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Category?",
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
                        apiService.create("Space_Amenities/deactivate_MasterAmenities", bL).
                            then(function (promise) {

                                if (promise.returnvalue ==="false") {
                                    swal('Master Amenities Deactivated Successfully');
                                }
                                else if (promise.returnvalue === "true") {
                                    swal('Master Amenities Activated Successfully');
                                }


                                $state.reload();
                            });
                    } else {
                        swal("Cancelled");

                    }

                });
        };



        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


    }

})();



