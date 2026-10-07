(function () {
    'use strict';
    angular.module('app').controller('SalesDrBlockedController', SalesDrBlockedController)
    SalesDrBlockedController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function SalesDrBlockedController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {


        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.editflag = false;

        $scope.obj = {};
        $scope.objques = {};

        $scope.OnLoadEmpName = function () {
            $scope.getuserName = [];
            var pageid = 1;

            apiService.getURI("SalesDrBlocked/OnLoadData", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.getuserName = promise.empDetails;
                    $scope.getSalesNegaotionData = promise.salesNegaotionData;

                }
            });
        };

        //save
        $scope.submitted = false;
        $scope.SaveData = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ISLIDN_ID": $scope.ISLIDN_ID,
                    "HRME_Id": $scope.obj.HRME_Id.hrme_id,
                    "ISLIDN_Value": $scope.ISLIDN_Value,
                    "ISLIDN_Remark": $scope.ISLIDN_Remark

                }
                apiService.create("SalesDrBlocked/SalesNegaotionSave", data).
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
                        else if (promise.returndata == "Update") {
                                swal("Record Update Successfully");
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
                "ISLIDN_ID": flr.ISLIDN_ID
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
                        apiService.create("SalesDrBlocked/SalesNegaotionDeactivate", data).
                            then(function (promise) {


                                if (promise.returndata === 'false') {
                                    swal('Record ' + mgs + ' Successfully.');
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
       
        $scope.EditData = function (user) {
            $scope.editflag = true;
            $scope.ISLIDN_ID = user.ISLIDN_ID;
            $scope.HRME_Id = user.HRME_Id;
            $scope.ISLIDN_Value = user.ISLIDN_Value;
            $scope.ISLIDN_Remark = user.ISLIDN_Remark;
            $scope.newcaste = {};
            for (var i = 0; i < $scope.getuserName.length; i++) {
                if ($scope.getuserName[i].hrme_id == user.HRME_Id) {
                    $scope.getuserName[i].Selected = true;
                    $scope.obj.HRME_Id = $scope.getuserName[i]; 
                    $scope.newcaste = $scope.getuserName[i].hrme_id;
                }
            }

        }



        // clear form data
        $scope.cancel = function () {
            $scope.HRME_Id = null;
            $scope.ISLIDN_Value = null;
            $scope.ISLIDN_Remark = null;
            $state.reload();
        };


        $scope.interacted2 = function (field) {
            return $scope.submitted;
        };
    }
})();
