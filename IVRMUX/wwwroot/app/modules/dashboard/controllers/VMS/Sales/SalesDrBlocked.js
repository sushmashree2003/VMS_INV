(function () {
    'use strict';
    angular.module('app').controller('SalesDrBlockedController', SalesDrBlockedController)
    SalesDrBlockedController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function SalesDrBlockedController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {


        //$scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;


        $scope.objques = {};
        
        $scope.OnLoadEmpName = function () {
            $scope.getuserName = [];
            var pageid = 1;

            apiService.getURI("SalesDrBlocked/OnLoadData",pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.getuserName = promise.empDetails;
                    $scope.getSalesDrData = promise.masterData;

                }
            });
        };	
       
        //save
        $scope.submitted = false;
        $scope.SaveData = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HRME_ID": $scope.HRME_Id.hrme_id,
                    "Scenario": $scope.selfassign
            
                }
                apiService.create("SalesDrBlocked/Save", data).
                    then(function (promise) {

                        if (promise.returndata === "Add") {
                            swal('Record Saved Successfully !');

                        }
                        else if (promise.returndata === "Notsave") {
                            swal('Record Not Saved !');

                        }
                        else if (promise.returndata === "Duplicate") {
                            swal("Record Already Exists !");

                        }
                        else {
                            swal("Please Contact Administrator !");
                        }
                        $state.reload();
                    })

            }
            else {

                $scope.submitted = true;
            }

        };



        //deactivate
        $scope.deactiveY = function (flr, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var data = {
                "MDS_id": flr.MDS_id
            }
            var mgs = "";
            if (flr.ActiveFlag === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " The Selected Row! ",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " The Selected Row!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("SalesDrBlocked/Deactivate", data).
                            then(function (promise) {


                                if (promise.returndata === 'false') {
                                    swal('Record ' + mgs +' Successfully.');
                                }

                                else if (promise.returndata === 'true') {
                                    swal('Activated Successfully.');
                                }

                                else if (promise.returndata === 'Error') {
                                    swal('Operation Failed!!!');
                                }


                                $state.reload();
                            });
                    } else {
                        swal("Cancelled");
                        $state.reload();
                    }

                });
        };


        //edit
        $scope.edibl = {}
        $scope.edit = function (bil) {
            $scope.edibl = bil.ISMCLTPRBOM_Id;
            var pageid = $scope.edibl;
            apiService.getURI("SalesDrBlocked/Edit", pageid).
                then(function (promise) {
                    $scope.Id = promise.bom_details[0].ISMCLTPRBOM_Id;
                    $scope.client_project_name = promise.bom_details[0].client_project_name;
                })
        };



        $scope.cancel = function () {
            $scope.ISMMCLTPR_Id = "";
            $scope.ISMCLTC_Id = "";
            $scope.ISMCLTPRBOM_Qty = "";
            $scope.ISMCLTPRBOM_Remarks = "";
        }

        $scope.cancel = function () {
            $state.reload();
        };

        // clear form data
        $scope.cancel = function () {
            $scope.HRME_Id = null;
           // $state.reload();
        };


        $scope.interacted2 = function (field) {
            return $scope.submitted;
        };
    }
})();
