(function () {
    'use strict';
    angular
        .module('app')
        .controller('Master_DepartmentHead_MappingController', Master_DepartmentHead_MappingController)

    Master_DepartmentHead_MappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function Master_DepartmentHead_MappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
        $scope.edit = false;
        $scope.obj = {};
        $scope.displaydata = [];
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        // ============================ng_click function for module (ALL)========================//
        $scope.searchchkbx = "";
        $scope.all_checkC = function () {
            var checkStatus = $scope.usercheckC;
            var count = 0;
            angular.forEach($scope.module_list, function (itm) {
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

            $scope.usercheckC = $scope.module_list.every(function (options) {
                return options.selected;
            });
        }

        //====================================clear data====================================//
        $scope.submitted = false;
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.isOptionsRequired = function () {
            return !$scope.module_list.some(function (options) {
                return options.selected;
            });
        }

        $scope.itemsPerPage = 5;
        $scope.currentPage = 1;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.alldepartmentcodetemp = [];
        //==================load data=========================//
        $scope.submitted = false;
        $scope.loaddata = function () {
            $scope.allcompany = [];
            $scope.obj.MI_Id = "";
            var pageid = 2;

            apiService.getURI("Master_DepartmentHead_Mapping/loaddata", pageid).then(function (promise) {
                $scope.allcompany = promise.allcompany;
                $scope.alldepartmentcodetemp = promise.alldepartmentcode;
                $scope.obj.MI_Id = promise.mI_Id;
                $scope.displaydata = promise.displaydata;
            });
            $scope.OnCompanyChange();

        }
        $scope.OnCompanyChange = function () {
            $scope.alldepartmentcode = [];
            //$scope.obj.HRMDC_ID = "";
            $scope.allEmployeename = [];
            $scope.displaydata = [];
            if ($scope.alldepartmentcodetemp != null && $scope.alldepartmentcodetemp.length > 0) {
                for (var i = 0; i < $scope.alldepartmentcodetemp.length; i++) {
                    if ($scope.obj.MI_Id == $scope.alldepartmentcodetemp[i].mI_Id) {
                        $scope.alldepartmentcode.push({
                            hrmdC_ID: $scope.alldepartmentcodetemp[i].hrmdC_ID,
                            hrmdC_Name: $scope.alldepartmentcodetemp[i].hrmdC_Name,
                            hrmdC_Code: $scope.alldepartmentcodetemp[i].hrmdC_Code,

                        })
                    }
                }
            }
            // $scope.ondepchange();
            if ($scope.obj.MI_Id > 0) {
                var data = {
                    "MI_Id": $scope.obj.MI_Id,
                    "HRMDC_ID": 0
                };
                apiService.create("Master_DepartmentHead_Mapping/depchange", data).then(function (promise) {

                    if (promise.displaydata != null && promise.displaydata.length > 0) {
                        $scope.displaydata = promise.displaydata;

                    }
                    else {
                        swal("Record Not Found !")
                    }
                });
            }
            else {
                $scope.obj.MI_Id = "";
            }
        }
        //=====================ondepchange=====================//
        $scope.ondepchange = function () {
            $scope.allEmployeename = [];
           
            var data = {
                "MI_Id": $scope.obj.MI_Id,
                "HRMDC_ID": $scope.obj.HRMDC_ID
            };
            apiService.create("Master_DepartmentHead_Mapping/depchange", data).then(function (promise) {
                if (promise.alldepartmentcode != null && promise.alldepartmentcode.length > 0) {
                    $scope.allEmployeename = promise.alldepartmentcode;
                }
                else {
                    swal("Record Not Found !");
                }
                

            });
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var HRMDCH_ID = 0;
                if ($scope.obj.hrmdcH_ID > 0) {
                    HRMDCH_ID = $scope.obj.hrmdcH_ID;
                }
                var data = {
                    "HRMDC_ID": $scope.obj.HRMDC_ID,
                    "HRME_Id": $scope.obj.HRME_Id.HRME_Id,
                    "HRMDCH_ID": HRMDCH_ID
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("Master_DepartmentHead_Mapping/savedata", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.hrmdcH_ID == 0 || promise.hrmdcH_ID < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.hrmdcH_ID > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else if (promise.returnduplicatestatus == 'Allreadymapped') {
                        swal('All Ready User is Mapped For this Department!!!');
                    }
                    else {
                        if (promise.hrmdcH_ID == 0 || promise.hrmdcH_ID < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.hrmdcH_ID > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.Editdata = function (obj) {
            $scope.newcaste = {};
            $scope.allEmployeename = [];
            var data = {
                "HRMDCH_ID": obj.HRMDCH_ID,
                "MI_Id": obj.MI_Id,
                "HRMDC_ID": obj.HRMDC_ID
          

            };

        apiService.create("Master_DepartmentHead_Mapping/Editdata", data).then(function (promise) {
            if (promise !== null ) {
                $scope.getdetails = promise.getdetails;
                $scope.allEmployeename = promise.alldepartmentcode;
                $scope.obj.HRMDC_ID = promise.getdetails[0].hrmdC_ID,
                    $scope.obj.hrmdcH_ID = promise.getdetails[0].hrmdcH_ID;
                if ($scope.allEmployeename != null && $scope.allEmployeename.length > 0) {
                    for (var i = 0; i < $scope.allEmployeename.length; i++) {
                        if (promise.getdetails[0].hrmE_ID == $scope.allEmployeename[i].HRME_Id) {
                            $scope.allEmployeename[i].Selected = true;
                            $scope.obj.HRME_Id = $scope.allEmployeename[i];
                            $scope.newcaste = $scope.allEmployeename[0].HRME_Id;
                        }
                    }
                }
                }
            });
        };
        $scope.Decative = function (user, SweetAlert) {
            var dystring = "";
            if (user.PlannerApproval === true) {
                dystring = "Deactivate";
            }
            else if (user.PlannerApproval === false) {
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
                        var data = {
                            "HRMDCH_ID": user.HRMDCH_ID
                        };

                        apiService.create("Master_DepartmentHead_Mapping/Decative", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record " + dystring + "d" + " Successfully!!!");


                            }
                            else {
                                swal("Record Not " + dystring + "d" + " Successfully!!!");
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        };
    
    }
})();

