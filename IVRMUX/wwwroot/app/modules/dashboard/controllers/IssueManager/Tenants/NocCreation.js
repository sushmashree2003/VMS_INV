

(function () {
    'use strict';
    angular
        .module('app')
        .controller('NocCreationController', NocCreationController);
    NocCreationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$compile'];
    function NocCreationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $compile) {


        $scope.submitted = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0
        }

        //$scope.usrname = localStorage.getItem('username');
        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        //if (admfigsettings.length > 0) {
        //    var logopath = admfigsettings[0].asC_Logo_Path;
        //}
        //$scope.imgname = logopath;
        //var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //-------------------------------------------------------------------
        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";

            var pageid = 2;
            apiService.getURI("NocCreation/getloaddata", pageid).
                then(function (promise) {
                    $scope.staffmappinglist = promise.staffmappinglist;
                    $scope.tname = promise.tname;
         
                    $scope.get_buildingmap = promise.get_building;
           
                    $scope.get_getAreaName = promise.get_Area;

                   // $scope.get_report = promise.get_report

                    $scope.presentCountgridI = $scope.staffmappinglist.length;
                   
                    $scope.presentCountgridU = $scope.get_Area.length;

                    //$scope.get_AreaActivity = promise.get_AreaActivity;
                    //$scope.presentCountgridI = $scope.get_AreaActivity.length;

                    $scope.fillroletype = promise.fillroletype;

                    $scope.institutionname = promise.fillinstitution;

                    $scope.staffmappinglistTemp = [];
                   // $scope.staffmappinglist = [];
                    $scope.staffmappinglistTemp = promise.staffmappinglist;
                    //if ($scope.staffmappinglistTemp != null && $scope.staffmappinglistTemp.length > 0) {
                    //    for (var t = 0; t < $scope.staffmappinglistTemp.length; t++) {
                    //        $scope.staffmappinglist.push({
                    //           // hrmaractT_Time: moment($scope.staffmappinglistTemp[t].hrmaractT_Time, 'HH:mm a').format(),
                    //            //hrmB_BuildingName: $scope.staffmappinglistTemp[t].hrmB_BuildingName,
                    //            //hrmF_FloorName: $scope.staffmappinglistTemp[t].hrmF_FloorName,

                    //            tenantagR_TenantName: $scope.staffmappinglistTemp[t].tenantagR_TenantName,
                    //            hrmaR_AreaName: $scope.staffmappinglistTemp[t].hrmaR_AreaName,
                    //            tenantnoC_Remarks: $scope.staffmappinglistTemp[t].tenantnoC_Remarks,       
                    //            tenantnoC_NOCDate: $scope.staffmappinglistTemp[t].tenantnoC_NOCDate,
                    //        });
                    //    }
                    //}
             
                });
        };



        // edit----------
        $scope.editdata = function (item) {
            $scope.hrmmmB_Id = item.hrmB_Id;  
           // $scope.hrmmmF_Id = item.hrmF_Id;
            $scope.hrmmaR_Id = item.hrmR_Id;
            $scope.tenantagR_Id = item.tenantagR_Id;
            $scope.tenantnoC_Remarks = item.remarks;
           $scope.tenantnoC_Id = item.tenantnoC_Id;
            //$scope.tenantnoC_CreatedDate = new date(item.date).toDateString();
            //  $scope.floordetailschange($scope.hrmmmB_Id) ;
            // $scope.areadetailschangetab($scope.hrmmmF_Id );
        }


        //-----------Floordeatils
        $scope.floordetailschange = function (tenant_id) {
         
            var data = {
                "TENANTAGR_Id": tenant_id.tenantagR_Id,
                "HRMR_Id": tenant_id.hrmR_Id,
                "HRMB_Id": tenant_id.hrmB_Id
                
            }
            apiService.create("NocCreation/getfloors", data).
                then(function (promise) {
                    if (promise.get_getAreaName != null && promise.get_getAreaName.length > 0) {
                        $scope.hrmmaR_Id = promise.get_getAreaName[0].hrmR_Id;
                        //$scope.get_flooramap = promise.get_floor;
                    }
                    else {
                        swal("No Record Found....")
                   
                    }
                })
        }

        //------------ On select Building, Loading Tenant

        $scope.loadtenant = function (hrmB_Id) {

            var data = {
                "HRMB_Id": hrmB_Id
            }
            apiService.create("NocCreation/gettenantname", data).
                then(function (promise) {
                    if (promise.tname != null && promise.tname.length > 0) {
                        $scope.tname = promise.tname;

                    }
                    else {
                        swal("No Record Found....")

                    }
                })
        }



        //-----------Areadeatils
        $scope.Activitydetailschange = function (hrmB_Id) {
        
            var data = {
                "HRMB_Id": $scope.hrmmmB_Id,
          
                "HRMR_Id": $scope.hrmmaR_Id

            }
            apiService.create("NocCreation/getAreaName", data).
                then(function (promise) {
                    if (promise.tenantlist != null && promise.tenantlist.length > 0) {
                        $scope.tname = promise.tenantlist;
                    }
                    else {
                        swal("No Record Found....")
                    }

                })
        }

        //----------usernames 
        $scope.tenantchnage = function (TENANTNOC_Id) {
            $scope.tenantlist = [];
            var data = {
                "TENANTNOC_Id": $scope.role_id,
                "MI_Id": $scope.MIId,
            }
            apiService.create("NocCreation/gettenantname", data).
                then(function (promise) {
                    if (promise.tenantlist != null && promise.tenantlist.length > 0) {
                        $scope.tenantlist = promise.tenantlist;
                    }
                    else {
                        swal("No Record Found....")
                    }
                })
        }

        //---------------------------------Save--------------------------------------------
        //Master  Activity_Time 
        $scope.savedataAREAActivityTime = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "TENANTNOC_Id": $scope.tenantnoC_Id,
                    "TENANTAGR_Id": $scope.tenantagR_Id.tenantagR_Id,
             
                    "TENANTNOC_Remarks": $scope.tenantnoC_Remarks,
                    "TENANTNOC_NOCDate": new Date($scope.tenantnoC_CreatedDate).toDateString()
                   
                    
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("NocCreation/savedetailsStaffMapping", data).then(function (promise) {

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

     

     
        //-------------- deactive  --------------------
        $scope.deactive = function (item) {
            $scope.tenantnoC_Id = item.tenantnoC_Id;
            var dystring = "";
            if (item.tenantnOC_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.tenantnOC_ActiveFlag == false) {
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
                        apiService.create("NocCreation/deactive", item).
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
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
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


