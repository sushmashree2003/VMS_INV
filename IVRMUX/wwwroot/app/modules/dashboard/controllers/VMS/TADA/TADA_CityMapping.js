(function () {
    'use strict';
    angular
        .module('app')
        .controller('TADA_CityMappingController', TADA_CityMappingController)
    TADA_CityMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function TADA_CityMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {
        $scope.currentPage = 1;
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        else {
            paginationformasters = 10;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings!==null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }           
        $scope.Disable_Designation = false;
        $scope.Citymap = false;
        $scope.searchValue = "";
        $scope.isOptionsRequired1 = function () {
            return !$scope.CityType.some(function (options) {
                return options.selected;
            });
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.onpageload = function () {
            var pageid = 1;
            apiService.getURI("TADA_CityMapping/loaddata", pageid).
                then(function (promise) {
                    $scope.grneTLIst = promise.designation_types;                   
                    $scope.getstate = promise.state;                                 
                })
        }
        $scope.Dropdown_State = function () {
            $scope.get_CityMapping = [];
            if ($scope.IVRMMS_Id > 0) {
                if ($scope.hrmdeS_Id > 0) {
                    var data = {
                        "IVRMMS_Id": $scope.IVRMMS_Id,
                        "HRMDES_Id": $scope.hrmdeS_Id,
                         "message": "Other"
                    };
                    apiService.create("TADA_CityMapping/getcity", data).then(function (promise) {
                        if (promise.city_Type.length > 0) {
                            $scope.CityType = promise.city_Type;
                            $scope.get_CityMapping = promise.get_CityMapping;
                            $scope.presentCountgrid = $scope.get_CityMapping.length;
                            $scope.descity = $scope.get_CityMapping[0].hrmdeS_DesignationName;

                            //extra amaount 
                            //$scope.employee = [];
                            //$scope.employee = $scope.get_CityMapping[0].hrmdeS_DesignationName;
                            //$scope.employeeid = [];
                            //angular.forEach($scope.get_CityMapping, function (dev) {
                            //    if ($scope.employeeid.length === 0) {
                            //        $scope.employeeid.push({ hrmdeS_Id: dev.hrmdeS_Id, vtadacM_Id: dev.vtadacM_Id, hrmdeS_DesignationName: dev.hrmdeS_DesignationName, ivrmmcT_Name: dev.ivrmmcT_Name, vtadacM_FoodAmt: dev.vtadacM_FoodAmt, vtadacM_AccommodationAmt: dev.vtadacM_AccommodationAmt, vtadacM_TransportAmt: dev.vtadacM_TransportAmt, vtadacM_ActiveFlag: dev.vtadacM_ActiveFlag });
                            //    }
                            //    else if ($scope.employeeid.length > 0) {
                            //        var intcount = 0;
                            //        angular.forEach($scope.employeeid, function (emp) {
                            //            if (emp.hrmdeS_Id === dev.hrmdeS_Id) {
                            //                intcount += 1;
                            //            }
                            //        });
                            //        if (intcount === 0) {
                            //            $scope.employeeid.push({ hrmdeS_Id: dev.hrmdeS_Id, vtadacM_Id: dev.vtadacM_Id, hrmdeS_DesignationName: dev.hrmdeS_DesignationName, ivrmmcT_Name: dev.ivrmmcT_Name, vtadacM_FoodAmt: dev.vtadacM_FoodAmt, vtadacM_AccommodationAmt: dev.vtadacM_AccommodationAmt, vtadacM_TransportAmt: dev.vtadacM_TransportAmt, vtadacM_ActiveFlag: dev.vtadacM_ActiveFlag });
                            //        }
                            //    }
                            //});

                            //console.log($scope.employeeid);
                            //angular.forEach($scope.employeeid, function (ddd) {
                            //    $scope.templist = [];
                            //    angular.forEach($scope.get_CityMapping, function (dd) {
                            //        if (dd.hrmdeS_Id === ddd.hrmdeS_Id) {
                            //            $scope.templist.push(dd);
                            //        }
                            //    });
                            //    ddd.plannerdetails = $scope.templist;
                            //});
                            //console.log($scope.employeeid);
                        }
                        else {
                            swal("City Is Not Mapped !Please contact administrator");
                            $state.reload();
                        }
                    });
                }
            }               
        };            
        $scope.obj = {};
        $scope.submitted = false;
        $scope.savejob = function () {
            $scope.submitted = true;
            $scope.employeeArray = [];                      
                if ($scope.myForm.$valid) {                    
                    if ($scope.VTADACM_FoodAmt > 0 || $scope.VTADACM_AccommodationAmt > 0 || $scope.VTADACM_TransportAmt > 0) {
                        angular.forEach($scope.CityType, function (comp) {
                            if (comp.selected == true) {
                                $scope.employeeArray.push({ IVRMMCT_Id: comp.ivrmmcT_Id });
                            }
                        });
                        var VTacmId = 0;
                        if ($scope.vtadacM_Id > 0) {
                            VTacmId = $scope.vtadacM_Id;
                        }
                        else {
                            VTacmId = 0;
                        }
                        var data = {
                            "city_array": $scope.employeeArray,
                            "HRMDES_Id": $scope.hrmdeS_Id,
                            "VTADACM_FoodAmt": $scope.VTADACM_FoodAmt,
                            "VTADACM_AccommodationAmt": $scope.VTADACM_AccommodationAmt,
                            "VTADACM_TransportAmt": $scope.VTADACM_TransportAmt,
                            "VTADACM_Id": VTacmId,
                            "VTADACM_TravelHour": $scope.VTADACM_TravelHour,
                            "message": "Other"

                        };
                        apiService.create("TADA_CityMapping/saverecord", data).then(function (promise) {
                            if (promise.returnval === "Add") {
                                if (promise.returnvalue === true) {
                                    if (promise.message === "") {
                                        swal('Record Saved successfully');
                                        $scope.Dropdown_State();
                                        $scope.VTADACM_AccommodationAmt = "";
                                        $scope.VTADACM_TransportAmt = "";
                                        $scope.VTADACM_FoodAmt = "";
                                        $scope.Disable_Designation = false;
                                        $scope.Citymap = false;
                                        $scope.vtadacM_Id = "";
                                    }
                                    else {
                                        swal('Record Saved successfully', "These Cites " + promise.message + " Duplicate");

                                      

                                    }
                                } if (promise.returnvalue === false) {
                                    swal("Record Already Exsist");
                                    $scope.vtadacM_Id = "";
                                }

                            }
                            else if (promise.returnval === 'Update') {
                                if (promise.returnvalue === true) {
                                    swal('Record Updated successfully');
                                    $scope.Dropdown_State();
                                    $scope.VTADACM_AccommodationAmt = "";
                                    $scope.VTADACM_TransportAmt = "";
                                    $scope.VTADACM_FoodAmt = "";
                                    $scope.Disable_Designation = false;
                                    $scope.Citymap = false;
                                    $scope.vtadacM_Id = "";

                                } if (promise.returnvalue === false) {
                                    swal("Failed To Update Record");
                                }

                            }
                            else if (promise.returnval === 'Duplicate') {
                                swal('Record already exist');
                                $scope.vtadacM_Id = "";

                            }
                            else {
                                swal('Failed, please contact administrator');
                                $state.reload();
                            }
                        });
                        $state.reload();
                    }

                    else {
                        swal("Please Fill Atleast One ExpenditureHead Amount ");
                    }
                }                                       
        };
        //Deactive
        $scope.Deletedata = function (item, SweetAlert) {
            $scope.vtadacM_Id = item.vtadacM_Id;
            var data = {
                "VTADACM_Id": item.vtadacM_Id,
                "message": "Other"
            }
            var dystring = "";
            if (item.vtadacM_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.vtadacM_ActiveFlag == false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("TADA_CityMapping/deactive", data).
                            then(function (promise) {
                                if (promise.returnvalue == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $scope.Dropdown_State(); 
                              
                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }      
        $scope.Clearid = function () {
            $scope.submitted = false;
            $state.reload();
            $scope.tablediv = false;
           
        }    
        $scope.edit = function (task) {
            $scope.vtadacM_Id = task.vtadacM_Id;
            angular.forEach($scope.CityType, function (city) {
                if (city.ivrmmcT_Id === $scope.ivrmmcT_Id) {
                    city.selected = false;
                }
            })
            var data = {
                "VTADACM_Id": task.vtadacM_Id,
                "message": "Other"
            };
            apiService.create("TADA_CityMapping/edit", data).then(function (promise) {
                if (promise.editfiles.length > 0) {
                    $scope.hrmdeS_Id = promise.editfiles[0].hrmdeS_Id;
                    $scope.vtadacM_Id = promise.editfiles[0].vtadacM_Id;
                    $scope.ivrmmcT_Id = promise.editfiles[0].ivrmmC_Id;                   
                    $scope.selected = promise.editfiles[0].ivrmmC_Id;
                    $scope.VTADACM_FoodAmt = promise.editfiles[0].vtadacM_FoodAmt;
                    $scope.VTADACM_AccommodationAmt = promise.editfiles[0].vtadacM_AccommodationAmt;
                    $scope.VTADACM_TransportAmt = promise.editfiles[0].vtadacM_TransportAmt;    
                    $scope.VTADACM_TravelHour = promise.editfiles[0].vtadacM_TravelHour;    
                    $scope.Disable_Designation = true;
                    $scope.Citymap = true;

                    angular.forEach($scope.CityType, function (city) {
                        if (city.ivrmmcT_Id === $scope.ivrmmcT_Id) {
                            city.selected = true;
                        }
                    })


                }
            });
        };
        //extra added
        $scope.all_checkCCCC = function () {
            var ivrmmcT_Id = $scope.ivrmmcT_Id;
            var count = 0;
            angular.forEach($scope.CityType, function (itm) {
                itm.selected = ivrmmcT_Id;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        };
        $scope.togchkbxCCCC = function () {
            $scope.ivrmmcT_Id = $scope.CityType.every(function (options) {
                return options.selected;
            });
        };
        $scope.isOptionsRequiredtwo = function () {
            return !$scope.CityType.some(function (options) {
                return options.selected;
            });
        };
    }
})();

