(function () {
    'use strict';
    angular.module('app').controller('ISM_AutoPlannerApprovalController', ISM_AutoPlannerApprovalController)
    ISM_AutoPlannerApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', 'Excel', '$timeout']
    function ISM_AutoPlannerApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, Excel, $timeout) {
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        $scope.obj = {};
        $scope.searchValue = "";
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
      
        $scope.loaddata = function () {
            var pageid = 2;                  
            apiService.getURI("ISM_PlannerApproval/loaddata", pageid).then(function (promise) {
                $scope.get_userEmplist = promise.get_userEmplist;
                $scope.get_employeelist = promise.get_employeelist;
            });
        };
       
        $scope.isRequiredmi = function () {

            return !$scope.get_userEmplist.some(function (options) {
                return options.checked;
            });

        };
        //
        $scope.all_miid = function (ac) {
            $scope.allmi = ac;
            var toggleStatus = $scope.allmi;
            angular.forEach($scope.get_userEmplist, function (itm) {
                itm.checked = toggleStatus;
            });
            
        };
       
        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.emparray = [];
            var dystring = "Auto Planer Approved";
            if ($scope.myForm.$valid) {
             
                angular.forEach($scope.get_userEmplist, function (itm) {
                    if (itm.checked == true) {
                        $scope.emparray.push({
                            HRME_Id: itm.HRME_Id
                        })
                    }
                   
                });
                var data = {
                    "emparray": $scope.emparray
                };
                swal({
                    title: "Are you sure?",
                    text: "Do You Want To " + dystring + " Record?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Approved it",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                   
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("ISM_PlannerApproval/saveemployee", data).
                                then(function (promise) {
                                    if (promise.sound == "save") {
                                        swal("Record Saved Sucessfully !");
                                    }
                                    else if (promise.sound == "notsave") {
                                        swal("Record Not Saved");
                                    }
                                    else if (promise.sound == "admin") {
                                        swal("Please Contact Administrator !");
                                    }
                                    $state.reload();
                                });
                        }

                    });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.Clearidtwo = function () {
            angular.forEach($scope.get_employeelist, function (itmp) {
                itmp.checked = false;

            });
            $scope.obj.alll = false;
        };
        $scope.AllAutoplaner = function () {
            angular.forEach($scope.get_employeelist, function (itmp) {
                if ($scope.obj.alll == true) {
                    itmp.checked = true;
                }
                else {
                    itmp.checked = false;
                }

            });
        }
        $scope.deactive = function (item) {
            $scope.emparray = [];
            angular.forEach($scope.get_employeelist, function (itmp) {
                if (itmp.checked == true) {
                    $scope.emparray.push({
                        HRME_Id: itmp.HRME_Id
                    })
                }

            });
            if ($scope.emparray != null && $scope.emparray.length > 0) {
                var dystring = "Removed Planer";
                var data = {
                    "emparray": $scope.emparray
                };
                swal({
                    title: "Are you sure?",
                    text: "Do You Want To " + dystring + " Record?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Remove it",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("ISM_PlannerApproval/deactiveemployee", data).
                                then(function (promise) {
                                    if (promise.sound == "save") {
                                        swal("Record Saved Sucessfully !");
                                    }
                                    else if (promise.sound == "notsave") {
                                        swal("Record Not Saved");
                                    }
                                    else if (promise.sound == "admin") {
                                        swal("Please Contact Administrator !");
                                    }
                                    $state.reload();
                                });
                        }

                    });
            }
            else {
                swal("Select At Least One CheckBox !");
            }
           
        };
        //excek
      
        //excel export

    }
})();
