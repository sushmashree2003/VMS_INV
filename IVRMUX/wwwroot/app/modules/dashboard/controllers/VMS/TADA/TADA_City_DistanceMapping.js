
(function () {
    'use strict';
    angular
        .module('app')
        .controller('TADA_City_DistanceMappingController', TADA_City_DistanceMappingController)

    TADA_City_DistanceMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'superCache']
    function TADA_City_DistanceMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, superCache) {

        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;

        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("TADA_City_DistanceMapping/getalldata", pageid).then(function (promise) {
                $scope.FromState = promise.stateList;
                $scope.Statelist = promise.stateList;
                $scope.companylist = promise.companylist;
                $scope.Travel_data = promise.travel_data;
                $scope.Travelstatedetails = promise.travelstatedetails;
                $scope.Travel_Tier = promise.travel_Tier;

            });
        }

        $scope.changeTravelDetails = function (Travel) {

            var data = {
                "VTADAMT_Id": Travel
            };
            apiService.create("TADA_City_DistanceMapping/TravelDetails", data).then(function (promise) {
                if (promise.travel_Details != null && promise.travel_Details.length > 0) {
                    $scope.Travel_Details = promise.travel_Details;
                }
            });
        };

        $scope.GetDesignation = function (MI_Id) {

            var data = {
                "MI_Id": MI_Id
            };
            apiService.create("TADA_City_DistanceMapping/GetDesignation", data).then(function (promise) {
                if (promise.designation != null && promise.designation.length > 0) {
                    $scope.Designation = promise.designation;
                }


            });

        };

        $scope.searchchkbx = "";
        $scope.all_checkC = function () {
            var checkStatus = $scope.usercheckC;
            var count = 0;
            angular.forEach($scope.Designation, function (itm) {
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

            $scope.usercheckC = $scope.Designation.every(function (options) {
                return options.selected;
            });
        }

        $scope.GetCityList = function (Distance) {
            Distance.CityList1 = [];
            Distance.IVRMMCT_Id = "";
            Distance.IVRMMS_IdNew = "";
            Distance.CityList2 = [];
            Distance.IVRMMCT_IdNew = "";
            var data = {
                "IVRMMS_Id": Distance.IVRMMS_Id
            };
            apiService.create("TADA_City_DistanceMapping/OnchangeCity", data).then(function (promise) {
                if (promise.cityList != null && promise.cityList.length > 0) {
                    Distance.CityList1 = promise.cityList;
                }


            });

        };
        $scope.OnchageCity = function (Distance) {
            Distance.CityList2 = [];
            Distance.IVRMMCT_IdNew = ""; Distance.IVRMMS_IdNew = "";
        }
        //GetCityList
        $scope.GetCityListNew = function (Distance) {
            Distance.CityList2 = [];
            Distance.IVRMMCT_IdNew = "";
            var data = {
                "IVRMMS_Id": Distance.IVRMMS_IdNew
            };
            apiService.create("TADA_City_DistanceMapping/OnchangeCity", data).then(function (promise) {
                if (promise.cityList != null && promise.cityList.length > 0) {
                    //Distance.CityList2 = promise.cityList;
                    angular.forEach(promise.cityList, function (ob) {
                        if (ob.IVRMMCT_Id != Distance.IVRMMCT_Id) {
                            Distance.CityList2.push(ob);
                        }
                    });

                }
            });
        };


        $scope.CityDistance = [{ itrS_Id1: 'trans12' }];
        $scope.addgrnrowsclient = function () {
            if ($scope.CityDistance.length > 1) {
                for (var x = 0; x === $scope.CityDistance.length; x++) {
                    var id1 = $scope.CityDistance[x].itrS_Id1;
                    var lastChar1 = id1.substr(id1.length - 1);
                    $scope.cnt1 = parseInt(lastChar1);
                }
            }
            $scope.cnt1 = $scope.cnt1 + 1;
            $scope.tet = 'trans' + $scope.cnt1;
            var newItemNo1 = $scope.cnt1;
            $scope.CityDistance.push({ 'itrS_Id1': 'trans12' + newItemNo1 });
        };
        $scope.removeclient = function (index, data) {
            var newItemNo = $scope.CityDistance.length - 1;
            $scope.CityDistance.splice(index, 1);

        };


        $scope.savedata = function () {
            $scope.selecteddetails = [];
            $scope.TADACityDistance = [];

            if ($scope.myForm.$valid) {

                if ($scope.CityDistance.length > 0) {
                    angular.forEach($scope.CityDistance, function (itmx) {
                        if (true) {
                            $scope.TADACityDistance.push({
                                VTADAMC_City1: itmx.IVRMMCT_Id,
                                VTADAMC_City2: itmx.IVRMMCT_IdNew,
                                VTADAMC_State1: itmx.IVRMMS_Id,
                                VTADAMC_State2: itmx.IVRMMS_IdNew,
                                VTADAMC_DistanceKM: itmx.KM

                            });
                        }
                    });
                }
                var data = {

                    TADACityDistanceDTO: $scope.TADACityDistance
                };
                apiService.create("TADA_City_DistanceMapping/savedata", data).then(function (promise) {

                    if (promise.message == "Saved") {
                        swal("Record Saved Successfully", "", "success");
                    }

                    if (promise.message == "Failed") {
                        swal("Record Not saved", "", "info");
                    }
                    if (promise.message == "Duplicate") {
                        swal("Record Already Exist", "", "warning");
                    }

                    $state.reload();

                });
            }



        };

        $scope.EditData = function (user) {
            $scope.CityDistance = [];
            var data = {
                "VTADAMC_Id": user.VTADAMC_Id
            };
            apiService.create("TADA_City_DistanceMapping/EditData", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.editDetails !== null && promise.editDetails.length > 0) {
                        $scope.EditDetails = promise.editDetails;


                        angular.forEach($scope.EditDetails, function (dd) {
                            $scope.CityDistance.push({
                                IVRMMS_Id: dd.vtadamC_State1, IVRMMCT_Id: dd.vtadamC_City1, IVRMMS_IdNew: dd.vtadamC_State2,
                                IVRMMCT_IdNew: dd.vtadamC_City2, KM: dd.vtadamC_DistanceKM
                            });
                            $scope.GetCityList(dd.vtadamC_State1);
                            $scope.GetCityListNew(dd.vtadamC_State2);

                        });
                    }
                }

            });
        };
        $scope.submitted = false;
        $scope.Savedetails = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var Designationlist = [];
                angular.forEach($scope.Designation, function (rr) {
                    if (rr.selected === true) {
                        Designationlist.push(rr);
                    }
                });
                var Traveldata = [];
                angular.forEach($scope.Travel_Details, function (aa) {
                    if (aa.Amount > 0) {
                        Traveldata.push(aa);
                    }
                });
                var data = {
                    "MI_Id": $scope.MI_Id,
                    // "amount": $scope.Amount,
                    "VTADAMC_FromState": $scope.From_State,
                    "VTADAMC_ToState": $scope.To_State,
                    "VTADAMT_Id": $scope.Travel,
                    "Traveldatalist": Traveldata,
                    "Designationlist": Designationlist,
                    "VTADAMC_Id": $scope.VTADAMC_Id,
                    "VRMMDT_Id": $scope.VRMMDT_Id

                };
                apiService.create("TADA_City_DistanceMapping/Savedetails", data).then(function (promise) {
                    if (promise.message == 'saved') {
                        swal("Data Saved Successfully...!!!");
                    }
                    else if (promise.message == 'Failed') {
                        swal("Data Not Saved Successfully...!!!");
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



        $scope.EditDetails = function (user, aa) {
            var name = ""; $scope.Travel_Details = []; $scope.Designation = [];
            var data = {
                "VTADAMC_Id": user.VTADAMC_Id,
                "MI_Id": user.MI_Id
            };
            apiService.create("TADA_City_DistanceMapping/EditDetails", data).then(function (promise) {
                if (promise.tadaeditdetails != null && promise.tadaeditdetails.length > 0) {
                    $scope.edit = promise.tadaeditdetails;
                    $scope.Designation = promise.designation;
                    $scope.Travel_Details = promise.editdesign;
                  
                    $scope.MI_Id = $scope.edit[0].MI_Id;
                
                    $scope.VTADAMC_Id = $scope.edit[0].VTADAMC_Id;
                    $scope.From_State = $scope.edit[0].Fromtate;

                    $scope.To_State = $scope.edit[0].ToState;
                    $scope.Travel = $scope.edit[0].VTADAMT_Id;
                    $scope.VRMMDT_Id = $scope.edit[0].VRMMDT_Id;
            
                    $scope.TDetails = $scope.edit[0].VTADAMD_Id;    
                    $scope.Amount = $scope.edit[0].amount;
                    angular.forEach($scope.Designation, function (qq) {
                        if (promise.tadaeditdetails[0].HRMDES_Id == qq.hrmdeS_Id) {
                            qq.selected = true;
                            $scope.selected = true;
                            $scope.usercheckC = true;
                        }
                    });

                }
            });
        };
        $scope.interacted = function (field) {
            return $scope.submitted;

        };

        $scope.searchchkbx = "";
        $scope.all_checkCk1 = function () {
            var checkStatus1 = $scope.usercheckC1;
            var count = 0;
            angular.forEach($scope.Travel_Details, function (itm) {
                itm.select = checkStatus1;
                if (itm.select == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }

        $scope.togchkbxCk1 = function () {

            $scope.usercheckC1 = $scope.Travel_Details.every(function (options) {
                return options.select;
            });
        }


    }
})();