(function () {
    'use strict';
    angular
        .module('app')
        .controller('TenantWaterBillController', TenantWaterBillController)

    TenantWaterBillController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function TenantWaterBillController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.sortKey = 'HRTRQNS_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted || field.$dirty;
        };

        //get data
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("TenantWaterBill/loaddata", pageid)
                .then(function (promise) {
                    if (promise.building != null && promise.building.length > 0) {
                        $scope.tenant_array = promise.building;
                    }

                    if (promise.monthnames != null && promise.monthnames.length > 0) {
                        $scope.monthnames = promise.monthnames;

                    }
                    if (promise.years != null && promise.years.length > 0) {
                        $scope.years = promise.years;
                    }

                    if (promise.building != null) {
                        $scope.building = promise.building;
                    }
                    if (promise.tenantdata != null) {
                        $scope.tenantdata = promise.tenantdata;
                    }
                });

            $scope.submitted = false;
            $scope.savetrainerfeedback = function () {
          
                $scope.submitted = true;
                if ($scope.myForm.$valid) {
                    var data = {
                        "TENANTMWBILL_Id": $scope.TENANTMWBILL_Id,
                        "HRMB_ID": $scope.HRMB_ID,
                        "TENANTMWBILL_Year": $scope.TENANTEBWB_Year,
                        "TENANTMWBILL_Month": $scope.TENANTEBWB_Month,
                        "TENANTMWBILL_TotalAmount": $scope.TENANTEBWB_Amount

                    };
                    apiService.create("TenantWaterBill/savetrainerfeedback", data).
                        then(function (promise) {
                            if (promise.returnmsg !== "") {
                                if (promise.returnmsg == "true") {
                                    swal('Record Updated Successfully');
                                }
                                else if (promise.returnmsg == "false") {
                                    swal('Record Not Saved/Updated successfully');
                                }
                                else {
                                    swal("Request Failed");
                                }
                                
                                $state.reload();
                            }
                            else {
                                swal('Operation Failed');
                                $state.reload();
                            }
                        });
                }
                else {
                    $scope.submitted = true;
                }
            };

            // 
            $scope.viewroomwisewaterbill = function (item) {
                var data = {
                    "TENANTMWBILL_Id": item.tenantmwbilL_Id,
                    "HRMB_ID": item.hrmB_ID
                    
                }
                apiService.create("TenantWaterBill/viewroomwisewaterbill", data).then(function (promise) {
                    if (promise.tenant_array != null && promise.tenant_array.length > 0) {
                        $scope.viewWaterBillRoomWise = promise.tenant_array;
                        $("#viewwaterbill").modal('show');
                    }
                    else {
                        swal("No Record Found");
                        $("#viewwaterbill").modal('hide');
                    }
                });
            }

            //save
            $scope.submitted = false;
            $scope.savetraineefeedback = function () {
                $scope.question_Option = [];
                angular.forEach($scope.question_list, function (cls) {
                    if (cls.selectedd2 === true) {
                        $scope.question_Option.push(cls);
                    }
                });
                $scope.submitted = true;
                if ($scope.myForm2.$valid) {
                    var data = {
                        "HRTCR_Id": $scope.HRTCR_Id,
                        "HRME_Id": $scope.hrmE_Id,
                        "question_Answer": $scope.mappedquestionlist
                    };
                    apiService.create("TenantWaterBill/savetraineefeedback", data).
                        then(function (promise) {
                            if (promise.returnvales !== "") {
                                if (promise.returnvales == "Update") {
                                    swal('Record Updated Successfully');
                                }
                                else if (promise.returnvales == "Add") {
                                    swal('Record Saved Successfully');
                                }
                                else if (promise.returnvales == "false") {
                                    swal('Record Not Saved/Updated successfully');
                                }
                                else if (promise.returnvales === "Duplicate") {
                                    swal('Category Already Exist');
                                }
                                $state.reload();
                            }
                            else {
                                swal('Operation Failed');
                                $state.reload();
                            }
                        });
                }
                else {
                    $scope.submitted = true;
                }
            };


            $scope.deleterecord = function (task) {

                $scope.TENANTMWBILL_Id = task.tenantmwbilL_Id;
              

                swal({
                    title: "Are you sure?",
                    text: "Do You Want To Delete Record?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("TenantWaterBill/deactive", task).
                                then(function (promise) {
                                    if (promise.returnval == "true") {
                                        swal("Record Deleted Successfully!!!");
                                    }
                                    else {
                                        swal("Record Not Deleted Successfully!!!");
                                    }
                                    $state.reload();
                                });
                        }
                        else {
                            swal("Record Deletion Cancelled!!!");
                        }
                    });
            };


            $scope.Clearid = function () {
                $state.reload();
            };
        }
    }


})();



