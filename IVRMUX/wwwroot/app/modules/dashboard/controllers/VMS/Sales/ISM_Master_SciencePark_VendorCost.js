(function () {
    'use strict';
    angular.module('app').controller('ISM_Master_SciencePark_VendorCostController', ISM_Master_SciencePark_VendorCostController)
    ISM_Master_SciencePark_VendorCostController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function ISM_Master_SciencePark_VendorCostController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {


        //$scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;


        $scope.objques = {};

        $scope.OnLoadData = function () {
            $scope.getuserName = [];
            var pageid = 1;

            apiService.getURI("ISM_Master_SciencePark_VendorCost/OnLoadData", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.getsupplierNames = promise.onLoadSupplierList;
                    $scope.OnLoadVendorCostList = promise.onLoadVendorCostList;

                    //for displaying grid
                    $scope.OnLoadVendorCostListNew = [];

                    angular.forEach($scope.OnLoadVendorCostList, function (dev) {
                        if ($scope.OnLoadVendorCostListNew.length === 0) {
                            $scope.OnLoadVendorCostListNew.push(dev);
                        } else if ($scope.OnLoadVendorCostListNew.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.OnLoadVendorCostListNew, function (emp) {
                                if (emp.INVMS_ID === dev.INVMS_ID) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.OnLoadVendorCostListNew.push(dev
                                );
                            }
                        }
                    });


                   // console.log($scope.OnLoadVendorCostListNew);

                    angular.forEach($scope.OnLoadVendorCostListNew, function (ddd) {
                        $scope.templist = [];

                        angular.forEach($scope.OnLoadVendorCostList, function (dd) {
                            if (dd.INVMS_ID === ddd.INVMS_ID) {

                                $scope.templist.push(dd);
                            }
                        });
                        ddd.datadetails = $scope.templist;

                    });

                }
            });
        };

        //save
        $scope.submitted = false;
        $scope.SaveData = function () {
            if ($scope.INVMS_Id != null && $scope.INVMS_Id != 0 && $scope.INVMS_Id != "" && $scope.INVMS_Id != undefined) {

                $scope.albumNameArray = [];
                if ($scope.materaldocuupload != null && $scope.materaldocuupload.length > 0) {
                    angular.forEach($scope.materaldocuupload, function (emp) {
                        $scope.albumNameArray.push({
                            IMSV_PackageName: emp.Package,
                            IMSV_Quantity: emp.Quantity,
                            IMSV_Price: emp.Price,
                            IMSV_Percentage: emp.Percentage,
                            IMSV_TotalCost: emp.TotalCost,
                            IMSV_TotalCostPerQty: emp.TotalCostPerQty,
                            IMSV_PackageFlag: emp.PackageFlag
                        })
                    });
                }


                if ($scope.albumNameArray.length > 0 && $scope.albumNameArray != null && $scope.albumNameArray != undefined) {
                    let isValid = true;  // Flag to check if any value is invalid

                    angular.forEach($scope.albumNameArray, function (emp) {
                        if (!emp.IMSV_PackageName || !emp.IMSV_Quantity || !emp.IMSV_Price ||
                            !emp.IMSV_Percentage || !emp.IMSV_TotalCost ||
                            !emp.IMSV_TotalCostPerQty || !emp.IMSV_PackageFlag) {
                            isValid = false;
                        }
                    });

                    if (!isValid) {
                        swal('Please Enter The Valid data');
                    } else {
                        $scope.submitted = true;
                        var data = {
                            "VendorCostSave": $scope.albumNameArray,
                            "INVMS_ID": $scope.INVMS_Id.INVMS_Id
                        }
                        apiService.create("ISM_Master_SciencePark_VendorCost/Save", data).
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
                }
                

            }
            else {
                swal('Please select the Supplier..');
            }
            //$scope.submitted = true;
            ////if ($scope.myForm.$valid) {
            //    var data = {
            //        "VendorCostSave": $scope.albumNameArray,
            //        "INVMS_ID": $scope.INVMS_Id.INVMS_Id
            //    }
            //    apiService.create("ISM_Master_SciencePark_VendorCost/Save", data).
            //        then(function (promise) {

            //            if (promise.returndata === "Add") {
            //                swal('Record Saved Successfully !');

            //            }
            //            else if (promise.returndata === "Notsave") {
            //                swal('Record Not Saved !');

            //            }
            //            else if (promise.returndata === "Duplicate") {
            //                swal("Record Already Exists !");

            //            }
            //            else {
            //                swal("Please Contact Administrator !");
            //            }
            //            $state.reload();
            //        })

            ////}
            ////else {

            ////    $scope.submitted = true;
            ////}

        };



        $scope.deactiveY = function (flr, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var data = {
                "IMSV_ID": flr.IMSV_ID
                
            }
            var mgs = "";
            if (flr.IMMB_ActiveFlag === false) {
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
                        apiService.create("ISM_Master_SciencePark_VendorCost/Deactivate", data).
                            then(function (promise) {
                                if (promise.returndata === 'false') {
                                    swal('Record ' + mgs + ' Successfully.');
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

        $scope.materaldocuupload = [{ id: 'Materal1' }];
        $scope.addmateral = function () {
            var newItemNo = $scope.materaldocuupload.length + 1;

            if (newItemNo <= 10) {
                $scope.materaldocuupload.push({
                    'id': 'Materal' + newItemNo,
                    ISMSLERaw_ExibitionDate: new Date()
                });
            }
        };
        $scope.removemateral = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

            if ($scope.materaldocuupload.length === 0) {
                //data
            }
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

        //// clear form data
        //$scope.cancel = function () {
        //    $scope.HRME_Id = null;
        //    // $state.reload();
        //};


        $scope.interacted2 = function (field) {
            return $scope.submitted;
        };
    }
})();
