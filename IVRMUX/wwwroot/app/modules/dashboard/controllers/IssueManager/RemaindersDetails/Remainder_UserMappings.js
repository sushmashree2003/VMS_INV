(function () {
    'use strict';
    angular.module('app').controller('Remainder_UserMappingsController', Remainder_UserMappingsController)
    Remainder_UserMappingsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function Remainder_UserMappingsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {
      
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
        $scope.submitted = false;
        $scope.TempRemoveUser = [];
        $scope.TempAddUser = [];
        $scope.ISMS_TemplateContent = "";

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("RemaindersDetails/GetDetails", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.GetMasterTemplateDetails = promise.getMasterTemplateDetails;
                    $scope.GetInstitutionDetails = promise.getInstitutionDetails;
                }
            });
        };

        $scope.OnChangeClient = function (VMS_IVRMTemplate_Name) {
            $scope.GetUserDetails = [];
            $scope.GetMappedUserDetails = [];
            $scope.getTemplateDetails = [];
            $scope.TempAddUser = [];
            $scope.UserId = "";
            $scope.ISMS_TemplateContent = "";
            var data = {
                "ISMMCLT_Id": $scope.ISMMCLT_Id.ismmclT_Id,
                "ISES_Template_Name": VMS_IVRMTemplate_Name
            };

            apiService.create("RemaindersDetails/OnChangeClient", data).then(function (promise) {
                if (promise !== null) {
                    $scope.ISES_Id = 0;
                    $scope.GetUserDetails = promise.getUserDetails;
                    $scope.GetMappedUserDetails = promise.getMappedUserDetails;
                    $scope.getTemplateDetails = promise.getTemplateDetails;

                    if ($scope.getTemplateDetails !== null && $scope.getTemplateDetails.length > 0) {
                        $scope.ISMS_TemplateContent = $scope.getTemplateDetails[0].iseS_SMSMessage;
                        $scope.ISES_Id = $scope.getTemplateDetails[0].iseS_Id;
                    }
                }
            });
        };


        $scope.optionToggled = function (dd) {
            if (dd.selected) {                 
                $scope.TempRemoveUser.push({ ISESUSR_Id: dd.isesusR_Id, UserId: dd.userId });
            } else {
                angular.forEach($scope.TempRemoveUser, function (d, i) {
                    if (dd.isesusR_Id === d.ISESUSR_Id) {
                        $scope.TempRemoveUser.splice(i,1);
                    }
                });
            }
        };

        $scope.AddToCart = function (aa) {
            var count1 = 0;
            angular.forEach($scope.GetMappedUserDetails, function (dd) {
                if (dd.userId === Number(aa.Id)) {
                    count1 += 1;
                }
            });

            if (count1 === 0) {
                if ($scope.TempAddUser.length === 0) {
                    $scope.TempAddUser.push({ UserId: aa.Id, UserName: aa.UserName });
                } else {
                    var count = 0;
                    angular.forEach($scope.TempAddUser, function (d, i) {
                        if (d.UserId === aa.Id) {
                            count += 1;
                        }
                    });

                    if (count == 0) {
                        $scope.TempAddUser.push({ UserId: aa.Id, UserName: aa.UserName });
                    }
                }
            } else {
                swal("Already Added");
            }
            aa = "";
        };

        $scope.DeleteRecord = function (dd, i) {
            $scope.TempAddUser.splice(i, 1);
        };

        $scope.savedata = function (ISMS_TemplateContent, VMS_IVRMTemplate_Name) {
             
            var data = {
                "ISMMCLT_Id": $scope.ISMMCLT_Id.ismmclT_Id,
                "ISES_Template_Name":VMS_IVRMTemplate_Name,
                "ISES_TemplateContent":ISMS_TemplateContent,
                "ISES_Id": $scope.ISES_Id,
                "Temp_UserMapping_Add": $scope.TempAddUser,
                "Temp_UserMapping_Remove": $scope.TempRemoveUser
            };

            apiService.create("RemaindersDetails/SaveData", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.returnvalue === "Save") {
                        swal("Record Saved Successfully");
                    } else {
                        swal("Failed To Save Record");
                    }
                    $state.reload();
                }
            });
        };       

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();