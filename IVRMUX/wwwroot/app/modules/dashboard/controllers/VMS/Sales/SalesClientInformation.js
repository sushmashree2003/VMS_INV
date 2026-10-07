(function () {
    'use strict';
    angular.module('app').controller('SalesClientInformationController', SalesClientInformationController)
    SalesClientInformationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function SalesClientInformationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {


        //$scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;

        $scope.productsonload = [];
        $scope.objques = {};

        $scope.OnLoad = function () {
           
            var pageid = 1;

            apiService.getURI("SalesClientInformation/OnLoadData", pageid).then(function (promise) {
                if (promise.products !== null && promise.products.length > 0) {
                    $scope.productsonload = promise.products;
                }
            });
        };
        $scope.temparray = [];
        //save
        $scope.submitted = false;
        $scope.SaveData = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.temparray = [];
                angular.forEach($scope.productsonload, function (itm) {
                    if (itm.selected == true) {
                        $scope.temparray.push({ ISMSMPR_Id: itm.ISMSMPR_Id })
                    }
                });

                var data = {
                    "ClientName": $scope.ClientName,
                    "ContactNo": $scope.ContactNo,
                    "EmailId": $scope.EmailId,
                    "status": $scope.purchaseStatus,
                    "Products": $scope.temparray,
                    "Institution": $scope.Institution,
                    "Address": $scope.Address,
                    "Pincode": $scope.Pincode
                }
                $state.reload();
                //apiService.create("SalesDrBlocked/Save", data).
                //    then(function (promise) {

                //        if (promise.returndata === "Add") {
                //            swal('Record Saved Successfully !');

                //        }
                //        else if (promise.returndata === "Notsave") {
                //            swal('Record Not Saved !');

                //        }
                //        else if (promise.returndata === "Duplicate") {
                //            swal("Record Already Exists !");

                //        }
                //        else {
                //            swal("Please Contact Administrator !");
                //        }
                //        $state.reload();
                //    })

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

        $scope.all_checkm = function (checkallm) {
            var toggleStatus = checkallm;
            angular.forEach($scope.productsonload, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.searchValueDesg = '';

        };


        $scope.togchkbxm = function () {

            $scope.checkallm = $scope.productsonload.every(function (itm) {
                return itm.selected;
            });

            $scope.searchValueDesg == '';

        };

        $scope.isOptionsRequired = function () {
            return !$scope.productsonload.some(function (options) {
                return options.selected;
            });
        };

        //$scope.cancel = function () {
        //    $scope.ClientName = "";
        //    $scope.ContactNo = "";
        //    $scope.EmailId = "";
        //    $scope.purchaseStatus = "";
        //    $scope.temparray = [];
        //    $state.reload();
        //}

        $scope.cancel = function () {
            $state.reload();
        };

        //// clear form data
        //$scope.cancel = function () {
        //    $scope.HRME_Id = null;
        //    // $state.reload();
        //};


        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();
