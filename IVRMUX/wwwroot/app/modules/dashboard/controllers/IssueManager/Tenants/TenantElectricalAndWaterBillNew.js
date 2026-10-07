(function () {
    'use strict';
    angular
        .module('app')
        .controller('TenantElectricalAndWaterBillNewController', TenantElectricalAndWaterBillNewController)

    TenantElectricalAndWaterBillNewController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache', 'blockUI', '$compile', '$q']
    function TenantElectricalAndWaterBillNewController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache, blockUI, $compile, $q) {

        //  var studentphtoto = "https://jshsstorage.blob.core.windows.net/files/NoImage.jpg";

        var paginationformasters;
        $scope.multidepartment = false;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.asmaY_Year = "";
        $scope.editdataflag = false;
        $scope.submitted = false;
        $scope.submitted1 = false;
        $scope.sendsmsemail = false;
        $scope.obj = {};
        $scope.search = "";
        $scope.edit_flag = false;
        $scope.bankdetails = false;
        //added on 09102017
        $scope.place_flg = true;
        $scope.name_flg = false;
        $scope.otherdetails == false;
        $scope.tenant_details = false;
        $scope.rolenamelist = "";
        $scope.tenantagrpaY_PaymentDate = new Date();
        $scope.searchby = false;
        $scope.itemsPerPage = 5;
        $scope.currentPage = 1;
        $scope.RegistrationNumbering = [];


       
        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            //$scope.itemsPerPage = 5;
            $scope.search = "";

            var pageid = 1;

            apiService.getURI("TenantAgreementReport/getallofflinedetails", pageid).then(function (promise) {
              
                if (promise.tenant_array != null && promise.tenant_array.length > 0) {
                    $scope.tenant_array = promise.tenant_array;
                }
             
                if (promise.monthnames != null && promise.monthnames.length > 0) {
                    $scope.monthnames = promise.monthnames;
                   
                }
                if (promise.years != null && promise.years.length > 0) {
                    $scope.years = promise.years;
                }

                if (promise.tenants != null) {
                    $scope.tenants = promise.tenants;
                }

            })

        }


        $scope.toggleAll = function () {


            var toggleStatus = $scope.all;
            if (toggleStatus == true) {
                angular.forEach($scope.tenant_array, function (itm) {
                    itm.isSelected = true;
                });
            }
            else {
                angular.forEach($scope.tenant_array, function (itm) {
                    itm.isSelected = false;
                });
            }

        }

        $scope.amtdetails = function () {

            $scope.all = $scope.tenant_array.every(function (itm) {
                return itm.isSelected;
            });

        };

        $scope.Save = function () {
            var temparr = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                angular.forEach($scope.tenant_array, function (ty) {
                    if (ty.isSelected == true) {
                        temparr.push(ty)
                    }

                })

                var data = {
                    "TENANTEBWB_Id": $scope.TENANTEBWB_Id,
                    tenantbilltypearr: temparr,
                    "TENANTEBWB_Year": $scope.TENANTEBWB_Year,
                    "TENANTEBWB_Month": $scope.TENANTEBWB_Month,

                };
                apiService.create("TenantAgreementReport/Saveallbilltype", data).then(function (promise) {

                    if (promise != null) {
                        if ($scope.tenants != null) {
                            $scope.tenants = promise.tenants;
                        }

                        if (promise.returnmsg === "true") {


                            swal('Record Saved successfully');

                        }
                        else if (promise.returnmsg === "duplicate") {

                            swal("Record Duplicate");
                        }


                        else {
                            swal("Failed to Update the record");
                        }
                    }
                    else {
                        swal("Contact Administrator");
                    }
                    $state.reload();
                });

            }
            else {

            }

        }


        $scope.Saveupdate = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "TENANTEBWB_Id": $scope.TENANTEBWB_Id,
                    "TENANTEBWB_Amount": $scope.obj.TENANTEBWB_Amount,
                    "TENANTEBWB_Year": $scope.TENANTEBWB_Year,
                    "TENANTEBWB_Month": $scope.TENANTEBWB_Month,
                    "TENANTAGR_Id": $scope.tenantagR_Id.tenantagR_Id,
                    "TENANTEBWB_Flg": $scope.radioval,
                    "TENANTEBWB_Remarks": $scope.obj.tenantebwB_Remarks

                };
                apiService.create("TenantAgreementReport/SaveElectricalbill", data).then(function (promise) {

                    if (promise != null) {
                        if ($scope.tenants != null) {
                            $scope.tenants = promise.tenants;
                        }

                        if (promise.returnmsg === "true") {


                            swal('Record Saved successfully');

                        }
                        else if (promise.returnmsg === "duplicate") {

                            swal("Record Duplicate");
                        }


                        else {
                            swal("Failed to Update the record");
                        }
                    }
                    else {
                        swal("Contact Administrator");
                    }
                    $state.reload();
                });

            }
            else {

            }

        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cleartabl1 = function () {
            $state.reload();
        }
       


        //=====================Editrecord....
        $scope.editdetails = function (task) {

            $scope.editdataflag = true;
            var data = {
               
                    "TENANTEBWB_Id": task.tenantebwB_Id
            };
            apiService.create("TenantAgreementReport/editdetails", data).then(function (promise) {
                if (promise.get_printdetails.length > 0) {
                    // $scope.get_printdetails = promise.get_printdetails;
                    $scope.TENANTEBWB_Id = promise.get_printdetails[0].tenantebwB_Id;
                    $scope.obj.TENANTEBWB_Amount = promise.get_printdetails[0].tenantebwB_Amount;
                    $scope.TENANTEBWB_Year = promise.get_printdetails[0].tenantebwB_Year;
                    $scope.TENANTEBWB_Month = promise.get_printdetails[0].tenantebwB_Month;
                    $scope.obj.tenantebwB_Remarks = promise.get_printdetails[0].tenantebwB_Remarks;
                   // $scope.tenantagR_Id = promise.get_printdetails[0].tenantagR_Id;

                    $scope.radioval = promise.get_printdetails[0].tenantebwB_Flg;

                    if ($scope.tenant_array != null && $scope.tenant_array.length > 0) {
                        for (var i = 0; i < $scope.tenant_array.length; i++) {
                            if (promise.get_printdetails[0].tenantagR_Id == $scope.tenant_array[i].tenantagR_Id) {
                                $scope.tenant_array[i].Selected = true;
                                $scope.tenantagR_Id = $scope.tenant_array[i];
                                //$scope.newcaste = $scope.allEmployeename[0].HRME_Id;
                            }
                        }
                    }

                }
              
            });
        };

        //=================Active/Deactivate
        $scope.deleterecord = function (task) {

            $scope.TENANTAGR_Id = task.tenantebwB_Id;
            var dystring = "";

            if (task.tenantebwB_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (task.tenantebwB_ActiveFlag === false) {
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
                        apiService.create("TenantAgreementReport/deactive", task).
                            then(function (promise) {
                                if (promise.returnmsg == "true") {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        };


        $scope.loadTenants = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            //$scope.itemsPerPage = 5;
            $scope.tenantDetails = [];
            $scope.getloaddata = [];
            $scope.tenantrentamountmonthwise = [];
            $scope.search = "";

            var pageid = 1;

            apiService.getURI("TenantAgreementReport/loadtenants", pageid).then(function (promise) {

                if (promise.tenantDetails != null && promise.tenantDetails.length > 0) {
                    $scope.tenantDetails = promise.tenantDetails;
                }

            })

        }

        $scope.getTenantAmountDetails = function () {
            var data = {
                "TENANTAGR_Id": $scope.tenantagR_Id.tenantagR_Id,
            };
            apiService.create("TenantAgreementReport/getTenantAmountDetails", data).then(function (promise) {
                if (promise.getloaddata != null && promise.getloaddata.length > 0) {
                    $scope.getloaddata = promise.getloaddata;
                }
                if (promise.reference != null && promise.reference.length > 0) {
                    $scope.tenantrentamountmonthwise = promise.reference;
                }

            });
        }


        $scope.toggleAllMonthdata = function () {

            var toggleStatus = $scope.obj.stdalll;
            angular.forEach($scope.tenantrentamountmonthwise, function (itm) {
                itm.isSelected = toggleStatus;
            });
        };

        $scope.amtdetailsmonthwise = function () {

            $scope.obj.stdalll = $scope.tenantrentamountmonthwise.every(function (itm) {
                return itm.isSelected;
            });

        };

        $scope.Savefine = function () {
            var temparrr = [];
            $scope.submitted1 = true;
            if ($scope.myForm2.$valid) {

                angular.forEach($scope.tenantrentamountmonthwise, function (ty) {
                    if (ty.isSelected == true) {
                        temparrr.push({ TENANTAGRPAY_Id: ty.tenantagrpaY_Id, TENANTAGR_Id: ty.tenantagR_Id, TENANTAGRPAY_PaymentMonth: ty.tenantagrpaY_PaymentMonth, TENANTAGRPAY_PaymentYear: ty.tenantagrpaY_PaymentYear, TENANTAGRPAY_ActualFineAmount: ty.tenantagrpaY_ActualFineAmount})
                    }

                })

                if (temparrr.length > 0) {

                    var data = {
                        tenantfineentryarr: temparrr,
                    };
                    apiService.create("TenantAgreementReport/Savefine", data).then(function (promise) {

                        if (promise != null) {
                           
                            if (promise.returnmsg === "true") {
                                swal('Record Saved successfully');
                            }
                            else if (promise.returnmsg === "duplicate") {
                                swal("Record Duplicate");
                            }
                            else {
                                swal("Failed to Update the record");
                            }
                        }
                        else {
                            swal("Contact Administrator");
                        }
                        
                        $scope.loadTenants();
                    });
                } else {
                    swal("Select atleast one record.");
                }
            }
            else {

            }

        }

        $scope.cleartab2 = function () {
            $scope.tenantagR_Id.tenantagR_Id = "";
            $scope.tenantDetails = [];
            $scope.getloaddata = [];
            $scope.tenantrentamountmonthwise = [];
            $scope.loadTenants();
        }

        $scope.toggleAllstd = function () {

            $scope.sendsmsemail = false;
            var toggleStatus = $scope.stdall;
            angular.forEach($scope.tenants, function (itm) {
                itm.stdselected = toggleStatus;
            });
            angular.forEach($scope.tenants, function (itm) {
            if (itm.stdselected == true) {
                $scope.sendsmsemail = true;
            }
            
            });

        };
        
        $scope.optionToggledstd = function () {
            $scope.sendsmsemail = false;
            $scope.stdall = $scope.tenants.every(function (itm) {
                return itm.stdselected;
            });
            angular.forEach($scope.tenants, function (itm) {
                if (itm.stdselected == true) {
                    $scope.sendsmsemail = true;
                }
               
            });

        };


        $scope.SendEmail = function () {

            $scope.hrmdid = [];

            angular.forEach($scope.tenants, function (ty) {
                if (ty.stdselected == true) {
                   
                    $scope.hrmdid.push({
                        TENANTEBWB_Id: ty.tenantebwB_Id
                    });
                }
            })
            var data =
            {
                "TENANTEBWB_Ids": $scope.hrmdid
            };


            apiService.create("TenantAgreementReport/sendmail", data).
                then(function (promise) {
                    if (promise.returnmsg == "success") {
                        swal("Email Sent..!!!");
                        $state.reload();
                    }
                    else if (promise.returnmsg == "notFound") {
                        swal("Email Not sent..!!!", 'Default Email-Id is Not Found.. !!!');
                        $state.reload();
                    }
                    else if (promise.returnmsg == "Error") {
                        swal("Something went wrong", 'Try After some time..!!');
                        $state.reload();

                    } else {
                        swal("Something went wrong", 'Try After some time..!!');
                        $state.reload();
                    }
                });


        };
    }
})();