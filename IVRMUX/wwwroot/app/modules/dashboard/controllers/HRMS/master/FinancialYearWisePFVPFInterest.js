(function () {
    'use strict';
    angular
        .module('app')
        .controller('FinancialYearWisePFVPFInterestController', FinancialYearWisePFVPFInterestController);
    FinancialYearWisePFVPFInterestController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function FinancialYearWisePFVPFInterestController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0
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
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("FinancialYearWisePFVPFInterest/getdetails", pageid).
                then(function (promise) {
                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                    
                    $scope.get_store = promise.get_store;
                    $scope.presentCountgrid = $scope.get_store.length;
                   
                })
        };

        // SAVE =========================


        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "IMFY_Id": $scope.imfY_Id,
                    "HRMPFVPFINT_PFInterestRate": $scope.hrmpfvpfinT_PFInterestRate,
                    "HRMPFVPFINT_VPFInterestRate": $scope.hrmpfvpfinT_VPFInterestRate,
                    "HRMPFVPFINT_Id": $scope.hrmpfvpfinT_Id,
                }

                apiService.create("FinancialYearWisePFVPFInterest/Savedetails", data).then(function (promise) {

                    if (promise !== null) {
                        if (promise.returnduplicatestatus == "Duplicate") {
                            swal("Record Already Exists");

                        } else if (promise.returnduplicatestatus === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }

                        } else if (promise.returnduplicatestatus === "Update") {
                            if (promise.returnval === false) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else {
                            swal("Failed To Update/Save Record");
                        }
                        $state.reload();
                    }
                });
            }
                    else {
                        $scope.submitted = true;
                    }
          };
           

    
              



        //DEACTIVE ---------------------
        $scope.deactive = function (user, SweetAlert) {
            $scope.hrmpfvpfinT_Id = user.hrmpfvpfinT_Id;
            var dystring = "";
            if (user.hrmpfvpfinT_ActiveFlg === false) {
                dystring = "Deactivate";
            }
            else if (user.hrmpfvpfinT_ActiveFlg === true) {
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
                        apiService.create("FinancialYearWisePFVPFInterest/deactive", user).
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



        //--------------------------
        //edit
        $scope.edit = function (item) {
            var data = {
                "HRMPFVPFINT_Id": item.hrmpfvpfinT_Id,
            };
            apiService.create("FinancialYearWisePFVPFInterest/edit", data).then(function (promise) {


                if (promise.getMaterTypeEditData.length > 0 && promise.getMaterTypeEditData != null) {
                    $scope.imfY_Id = promise.getMaterTypeEditData[0].imfY_Id;
                    $scope.imfY_FinancialYear = "";
                    $scope.hrmpfvpfinT_PFInterestRate = promise.getMaterTypeEditData[0].hrmpfvpfinT_PFInterestRate;
                    $scope.hrmpfvpfinT_VPFInterestRate = promise.getMaterTypeEditData[0].hrmpfvpfinT_VPFInterestRate;
                    $scope.hrmpfvpfinT_Id = promise.getMaterTypeEditData[0].hrmpfvpfinT_Id;
                }
            });
        };

        
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        }




    }
})();