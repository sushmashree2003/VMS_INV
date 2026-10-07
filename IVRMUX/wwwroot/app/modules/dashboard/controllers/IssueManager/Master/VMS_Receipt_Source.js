(function () {
    'use strict';
    angular.module('app').controller('VMS_Receipt_SourceController', VMS_Receipt_SourceController)
    VMS_Receipt_SourceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function VMS_Receipt_SourceController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
     

        $scope.submitted = false;

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
        var configsettingsnew = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        $scope.itemsPerPage = paginationformasters;

        $scope.currentPage = 1;
        //-----------Tax Grid Select All

        $scope.loaddata = function () {
            $scope.obj.MI_Id = "";
            var pageid = 2;
            apiService.getURI("VMS_Receipt_Voucher/loaddata", pageid).then(function (promise) {
                
                $scope.CheckbookSource = promise.checkbookSource;
            });
        };
      
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var VCBSRC_Id = 0;
                if ($scope.VCBSRC_Id > 0) {
                    VCBSRC_Id = $scope.VCBSRC_Id;
                }
                var data = {
                    "VCBSRC_SourceName": $scope.VCBSRC_SourceName,
                    "VCBSRC_SourceMobileNo": $scope.VCBSRC_SourceMobileNo,
                    "VCBSRC_EmailId": $scope.VCBSRC_EmailId,
                    "VCBSRC_Address": $scope.VCBSRC_Address,
                    "VCBSRC_Id": VCBSRC_Id
                };
                apiService.create("VMS_Receipt_Voucher/savedataSource", data).then(function (promise) {
                    if (promise.returnval == true) {
                        if (promise.returnvalTwo === "exit") {
                            swal("Record Already Exists !");
                        }
                        else {
                            swal("Data Saved/Updated Successfully ");
                        }
                    } else {
                        swal("Failed To Save/Update Record ");
                    }
                    $state.reload();
                });
            }

        };
        $scope.edit = function (user) {
            $scope.VCBSRC_SourceName = user.vcbsrC_SourceName;
            $scope.VCBSRC_SourceMobileNo = user.vcbsrC_SourceMobileNo;
            $scope.VCBSRC_EmailId = user.vcbsrC_EmailId;
            $scope.VCBSRC_Id = user.vcbsrC_Id;
            $scope.VCBSRC_Address = user.vcbsrC_Address;
        }     
        $scope.Deletedata = function (item, SweetAlert) {
            var data = {
                "VCBSRC_Id": item.vcbsrC_Id
            }
            var dystring = "";
            if (item.vcbsrC_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.vcbsrC_ActiveFlg == false) {
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
                        apiService.create("VMS_Receipt_Voucher/clientDecative", data).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }


                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();
