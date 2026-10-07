(function () {
    'use strict';
    angular
        .module('app')
        .controller('TADATravelTierController', TADATravelTierController)

    TADATravelTierController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'superCache']
    function TADATravelTierController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, superCache) {

        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.search = "";
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("TADA_City_DistanceMapping/getalldata", pageid).then(function (promise) {

                $scope.Statedatalist = promise.statedatalist;
                $scope.TADA_Traveltier = promise.tadA_Traveltier
                $scope.Travel_Tier = promise.travel_Tier
                
            });
        }
        $scope.search = "";
        $scope.Tocities = function (IVRMMS_Id) {

            var data = {
                "IVRMMS_Id": IVRMMS_Id
            };
            apiService.create("TADA_City_DistanceMapping/Getcities", data).then(function (promise) {
                if (promise.cityList != null && promise.cityList.length > 0) {
                    $scope.CityList = promise.cityList;
                    //$scope.Citytier=promise.citytier; 
                }
            });
        };
        $scope.searchchkbx = "";
        $scope.all_checkC = function () {
            var checkStatus = $scope.usercheckC;
            var count = 0;
            angular.forEach($scope.CityList, function (itm) {
                itm.selected = checkStatus;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }

        $scope.togchkbxC = function () {

            $scope.usercheckC = $scope.CityList.every(function (options) {
                return options.selected;
            });
        }
        $scope.isOptionsRequiredtwo = function () {
            return !$scope.CityList.some(function (options) {
                return options.selected;
            });
        };
        $scope.search = "";
        $scope.submitted = false;
        $scope.SaveTierdata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var TravelCity = [];
                angular.forEach($scope.CityList, function (rr) {
                    if (rr.selected === true) {
                        TravelCity.push(rr);
                    }
                });
                var data = {
                   
                    "IVRMMS_Id": $scope.ivrmmS_Id,
                    "TravelCity": TravelCity,
                    "VRMMDT_Id": $scope.vrmmdT_Id
                   

                };
                apiService.create("TADA_City_DistanceMapping/SaveTierdata", data).then(function (promise) {
                    if (promise.message == 'saved') {
                        swal("Data Saved Successfully...!!!");
                    }
                    else if (promise.message == 'Failed') {
                        swal("Data Not Saved ");
                    }
                    else if (promise.message == 'updated') {
                        swal("Data Successfully Updated");
                    }
                    else {

                        swal("Data Already Exists");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;

        };

    }
})();
