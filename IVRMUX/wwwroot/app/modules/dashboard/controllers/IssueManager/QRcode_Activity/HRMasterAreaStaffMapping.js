
(function () {
    'use strict';
    angular
        .module('app')
        .controller('HRMasterAreaStaffMappingController', HRMasterAreaStaffMappingController);
    HRMasterAreaStaffMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$compile'];
    function HRMasterAreaStaffMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $compile) {

      
        $scope.searchValueI = "";
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;

        }
        else {
            paginationformasters = 10;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //-------------------------------------------------------------------
        
        $scope.loaddata = function () {

            $scope.currentPageI = 1;
            $scope.itemsPerPageI = paginationformasters;
            $scope.search = "";
            $scope.presentCountgridI = "";
           
            var pageid = 2;
            apiService.getURI("HRMasterAreaStaffMapping/getloaddata", pageid).
                then(function (promise) {

                    $scope.get_building = promise.get_building;
                    $scope.get_buildingmap = promise.get_building;

                    $scope.get_Area = promise.get_Area;
                    $scope.presentCountgridU = $scope.get_Area.length;

                    $scope.get_AreaActivity = promise.get_AreaActivity;
                    $scope.presentCountgridI = $scope.get_AreaActivity.length;

                    $scope.fillroletype = promise.fillroletype;

                    $scope.institutionname = promise.fillinstitution;
                    
                    $scope.staffmappinglistTemp = [];
                    $scope.staffmappinglist = [];
                    $scope.staffmappinglistTemp = promise.staffmappinglist;
                    if ($scope.staffmappinglistTemp != null && $scope.staffmappinglistTemp.length > 0) {
                        for (var t = 0; t < $scope.staffmappinglistTemp.length; t++) {
                            $scope.staffmappinglist.push({
                                hrmaractT_Time: moment($scope.staffmappinglistTemp[t].hrmaractT_Time, 'HH:mm a').format(),
                                hrmB_BuildingName: $scope.staffmappinglistTemp[t].hrmB_BuildingName,
                                hrmF_FloorName: $scope.staffmappinglistTemp[t].hrmF_FloorName,
                                hrmaR_AreaName: $scope.staffmappinglistTemp[t].hrmaR_AreaName,
                              
                                hrmaracT_ActivityName: $scope.staffmappinglistTemp[t].hrmaracT_ActivityName,
                                hrmaractstmaP_Date: $scope.staffmappinglistTemp[t].hrmaractstmaP_Date,
                                hrmaractstmaP_Id: $scope.staffmappinglistTemp[t].hrmaractstmaP_Id,
                                hrmaractstmaP_ActiveFlag: $scope.staffmappinglistTemp[t].hrmaractstmaP_ActiveFlag,
                                userName: $scope.staffmappinglistTemp[t].userName,
                            });
                        }
                    }
                    $scope.presentCountgridI = $scope.staffmappinglist.length;
                });
        };

        //-----------Floordeatils
        $scope.floordetailschange = function (hrmB_Id) {
            $scope.get_floor = [];
            $scope.get_flooramap = [];
            $scope.hrmF_Id = "";
            var data = {
                "HRMB_Id": hrmB_Id
            }
            apiService.create("HRMasterAreaStaffMapping/getfloors", data).
                then(function (promise) {
                    if (promise.get_floor != null && promise.get_floor.length > 0) {
                        $scope.get_floor = promise.get_floor;
                        $scope.get_flooramap = promise.get_floor;
                    }
                    else {
                        swal("No Record Found....")
                    }

                })
        }
        
        //-----------Areadeatils
        $scope.areadetailschangetab = function (hrmB_Id) {
            $scope.get_getAreaName = [];
            var data = {
                "HRMB_Id": $scope.hrmmmB_Id,
                "HRMF_Id": $scope.hrmmmF_Id
            }
            apiService.create("HRMasterAreaStaffMapping/getAreaName", data).
                then(function (promise) {
                    if (promise.get_getAreaName != null && promise.get_getAreaName.length > 0) {
                        $scope.get_getAreaName = promise.get_getAreaName;
                    }
                    else {
                        swal("No Record Found....")
                    }

                })
        }
        
        //-----------Activity Deatils
        $scope.Activitydetailschange = function (hrmaR_Id) {
            $scope.get_getActivity = [];
            var data = {
                "HRMAR_Id": hrmaR_Id,
            }
            apiService.create("HRMasterAreaStaffMapping/getActivityName", data).
                then(function (promise) {
                    if (promise.get_getActivity != null && promise.get_getActivity.length > 0) {
                        $scope.get_getActivity = promise.get_getActivity;
                    
                    }
                    else {
                        swal("No Record Found....")
                    }

                })
        }
        //-----------Employee list
        $scope.Employeelistchange = function (role_id) {
            $scope.emp_deatils = [];
            var data = {
                "IVRMRT_Id": role_id,
            }
            apiService.create("HRMasterAreaStaffMapping/getEmployeeName", data).
                then(function (promise) {
                    if (promise.emp_deatils != null && promise.emp_deatils.length > 0) {
                        $scope.emp_deatils = promise.emp_deatils;
                    }
                    else {
                        swal("No Record Found....")
                    }

                })
        }


        //----------usernames 
        $scope.Usernamechnage = function (ivrmrT_Id) {
            $scope.get_getusernames = [];
            var data = {
                "IVRMRT_Id": $scope.role_id,
                "MI_Id": $scope.MIId,
            }
            apiService.create("HRMasterAreaStaffMapping/getUsername", data).
                then(function (promise) {
                    if (promise.get_getusernames != null && promise.get_getusernames.length > 0) {
                        $scope.get_getusernames = promise.get_getusernames;
                    }
                    else {
                        swal("No Record Found....")
                    }

                })
        }
        //---------------------------------Save--------------------------------------------
        //Master  Activity_Time 
        $scope.savedataAREAActivityTime = function () {
            $scope.submittedUGActivityTime = true;

            $scope.ActivitynameArray = [];
            angular.forEach($scope.get_getActivity, function (role) {
             //   if (!!role.selected) $scope.ActivitynameArray.push(role);
                if (role.statusck == true) {
                    $scope.ActivitynameArray.push({ HRMARACT_Id:role.hrmaracT_Id });
                }
            })

            if ($scope.myForm.$valid) {
                var data = {
                    "HRMARACTSTMAP_Id": $scope.hrmaractstmaP_Id,
                    "HRMB_Id": $scope.hrmmmB_Id,
                    "HRMF_Id": $scope.hrmmmF_Id,
                    "HRMAR_Id": $scope.hrmmaR_Id,
                    "ActivitynameArray": $scope.ActivitynameArray,
                    "id": $scope.id,
                    "HRMARACTSTMAP_Date": new Date($scope.hrmaractstmaP_Date).toDateString(),
                    
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("HRMasterAreaStaffMapping/savedetailsStaffMapping", data).then(function (promise) {

                    if (promise.returnduplicatestatus == 'Saved') {
                        swal('Record Saved Successfuly');
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus == 'Not Saved') {
                        swal('Record Not Saved Successfuly');
                    }
                    else if (promise.returnduplicatestatus == 'Update') {
                        swal('Record Update Successfuly');
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus == 'Not Update') {
                        swal('Record Not Update Successfuly');
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.hrmaractT_Id == 0 || promise.hrmaractT_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.hrmaractT_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }

                    $scope.submittedUG = false;
                })
            }
            else {
                $scope.submittedUG = true;
            }
        };

        
        $scope.deactivemapping = function (item, SweetAlert) {
            $scope.hrmaractstmaP_ActiveFlag = item.hrmaractstmaP_ActiveFlag;
            var dystring = "";
            if (item.hrmaractstmaP_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.hrmaractstmaP_ActiveFlag == false) {
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
                        apiService.create("HRMasterAreaStaffMapping/deactive", item).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + dystring + "d Cancelled!!!");
                    }
                });
        }
        //=====================Editrecord....
        $scope.edit = function (user) {

            var data = {
                "HRMARACTSTMAP_Id": user.hrmaractstmaP_Id
            };
            apiService.create("HRMasterAreaStaffMapping/edit", data).then(function (promise) {
                $scope.editarrey = promise.editarrey; 
                $scope.hrmmmB_Id = $scope.editarrey[0].HRMB_Id; 
                $scope.hrmmmF_Id = $scope.editarrey[0].HRMF_Id; 
                $scope.hrmmaR_Id = $scope.editarrey[0].HRMAR_Id; 
                $scope.hrmaracT_Id = $scope.editarrey[0].HRMARACT_Id; 
                $scope.hrmaractstmaP_Id = $scope.editarrey[0].HRMARACTSTMAP_Id; 
                $scope.hrmaractstmaP_Date = $scope.editarrey[0].HRMARACTSTMAP_Date; 


            });
        };


        $scope.all_checkS = function (ck) {
            $scope.checkallS = ck;
            var toggleStatus = $scope.checkallS;
            angular.forEach($scope.get_getActivity, function (st) {
                st.statusck = toggleStatus;
            });
        };
        $scope.togchkbxS = function () {
            $scope.checkallS = $scope.get_getActivity.every(function (options) {
                return options.statusck;
            });
        };
        $scope.isOptionsRequiredS = function () {
            return !$scope.get_getActivity.some(function (options) {
                return options.statusck;
            });
        };




        $scope.interacted = function (field) {
            return $scope.submittedUG;
        };
      
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        
        $scope.searchValue = '';
        $scope.cancel = function () {
            $state.reload();
        }
        
    }
})();