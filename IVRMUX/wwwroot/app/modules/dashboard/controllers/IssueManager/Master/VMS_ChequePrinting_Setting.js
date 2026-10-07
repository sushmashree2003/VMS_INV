
(function () {
    'use strict';
    angular
        .module('app')
        .controller('VMS_ChequePrinting_SettingController', VMS_ChequePrinting_SettingController);
    VMS_ChequePrinting_SettingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function VMS_ChequePrinting_SettingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        
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

        $scope.obj = [];
        $scope.bankchange =function (){
            $scope.alldata = [];
           
            var data = {
                "MI_Id": $scope.obj.MI_Id,
                "HRMBD_Id": $scope.obj.HRMBD_Id,
                "optionflag": "ChequePrinting"
            };
            apiService.create("VMS_ChequePrinting_Setting/companychange", data).then(function (promise) {
                $scope.alldata = promise.alldata;
               
            });
        }
        $scope.oncmpchange = function () {
            $scope.alldata = [];
            $scope.obj.HRMBD_Id = "";
            var data = {
                "MI_Id": $scope.obj.MI_Id,
                "optionflag":  "ChequePrinting"

            };
            apiService.create("VMS_ChequePrinting_Setting/companychange", data).then(function (promise) {
               
                $scope.banklist = promise.banklist;
            });
        }


        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("VMS_ChequePrinting_Setting/getloaddata", pageid).
                then(function (promise) {
                    $scope.allcompany = promise.allcompany;
                })
        };
        //---------------------------------Save vms checkbook--------------------------------------------
       
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "VCPSET_Id": $scope.vcpseT_Id,
                    "VCPSET_Remarks": $scope.vcpseT_Remarks,
                    "VCPSET_ChequeImageFilePath": $scope.vcpseT_ChequeImageFilePath,
                    "VCPSET_ChequeImageFileName": $scope.vcpseT_ChequeImageFileName,
                    "VCPSET_PrintingTemplateId": $scope.vcpseT_PrintingTemplateId,
                    "HRMBD_Id": $scope.obj.HRMBD_Id,

                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("VMS_ChequePrinting_Setting/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.vcpseT_Id == 0 || promise.vcpseT_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.vcpseT_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.vcpseT_Id == 0 || promise.vcpseT_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.vcpseT_Id > 0) {
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

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        }

        $scope.deactive = function (item, SweetAlert) {
            $scope.VCPSET_Id = item.vcpseT_Id;
            var dystring = "";
            if (item.vcpseT_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.vcpseT_ActiveFlg == false) {
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
                        apiService.create("VMS_ChequePrinting_Setting/deactive", item).
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

        $scope.edit = function (item) {
            $scope.vcpseT_Remarks = item.vcpseT_Remarks;
            $scope.vcpseT_ChequeImageFilePath = item.vcpseT_ChequeImageFilePath;
            $scope.vcpseT_ChequeImageFileName = item.vcpseT_ChequeImageFileName;
            $scope.vcpseT_PrintingTemplateId = item.vcpseT_PrintingTemplateId;
            $scope.vcpseT_Id = item.vcpseT_Id;
        }
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
    }
})();