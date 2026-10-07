(function () {
    'use strict';
    angular.module('app').controller('SalesDrBlockedController', SalesDrBlockedController)
    SalesDrBlockedController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function SalesDrBlockedController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {

        $scope.itemsPerPage = 1;
        $scope.currentPage = 1;

        $scope.objques = {};

        $scope.OnLoadModuleNames = function () {
            $scope.getModuleData = [];
            var pageid = 1;

            apiService.getURI("SalesDrBlocked/OnLoadIvrmData", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.getModuleData = promise.ivrmData;
                    $scope.getIsmSalesData = promise.ism_SalesData;
                    $scope.getMasterData = promise.masterData;


                    //for displaying grid
                    $scope.bomids = [];

                    angular.forEach($scope.getMasterData, function (dev) {
                        if ($scope.bomids.length === 0) {
                            $scope.bomids.push(dev);
                        } else if ($scope.bomids.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.bomids, function (emp) {
                                if (emp.IMMB_BOMID === dev.IMMB_BOMID) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.bomids.push(dev
                                );
                            }
                        }
                    });


                    console.log($scope.bomids);

                    angular.forEach($scope.bomids, function (ddd) {
                        $scope.templist = [];

                        angular.forEach($scope.getMasterData, function (dd) {
                            if (dd.IMMB_BOMID === ddd.IMMB_BOMID) {

                                $scope.templist.push(dd);
                            }
                        });
                        ddd.datadetails = $scope.templist;
                        
                    });


                    console.log($scope.bomids);

                }
                else {
                    swal("No Record  Found..... !!");
                    $scope.getMasterData = "";
                }
            });
        };

        $scope.selectedModules = [];
        // selection
        $scope.all_checkm = function (checkallm) {
            var toggleStatus = checkallm;
            angular.forEach($scope.getModuleData, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.searchValueDesg = '';

        };


        $scope.togchkbxm = function () {

            $scope.checkallm = $scope.getModuleData.every(function (itm) {
                return itm.selected;
            });

            $scope.searchValueDesg == '';
           
        };

        $scope.getView = function (user) {
            $scope.RepeatDetails = [];
            $scope.orderFlag = false;
            $scope.RepeatDetails = user.datadetails;
            $('#moduleordersetting').modal('show');

        }



        //$scope.sortKey = 'ismS_OrderFlag';
        //$scope.sortReverse = false;
        //$scope.sortableOptions = {
        //    stop: function (e, ui) {
        //        for (var index in $scope.RepeatDetails) {
        //            $scope.RepeatDetails[index].ismS_OrderFlag = Number(index) + 1;
        //            $scope.RepeatDetails[index].IMMB_IVRMMID_Order = Number(index) + 1;
        //        }
        //        $scope.orderFlag = false;
        //    }
        //};
       

        //$scope.orderFlag = false;
        //$scope.sortableNumber = function () {
        //    var count = 0;
        //    $scope.orderFlag = true;
        //    angular.forEach($scope.RepeatDetails, function (dd) {
        //        dd.IMMB_IVRMMID_Order = "";
        //        count = count + 1;
        //        dd.IMMB_IVRMMID_Order = count;
        //    })

        //}


        //$scope.sortableOptions = {
        //    stop: function (e, ui) {
        //        // After the sorting is done, update the order dynamically.
        //        angular.forEach($scope.RepeatDetails, function (dd, index) {
        //            dd.IMMB_IVRMMID_Order = index + 1; // Set the new order based on index
        //        });
        //        $scope.orderFlag = true;  // Mark that the order has been updated
        //    }
        //};



        //$scope.sortableNumber = function () {
        //    // This function is now just a placeholder in case you need to reset the order
        //    var count = 0;
        //    angular.forEach($scope.RepeatDetails, function (dd) {
        //        count++;
        //        dd.IMMB_IVRMMID_Order = count; // Sequential order
        //    });
        //};



        $scope.sortableOptions = {
            stop: function (e, ui) {
                // Only update the order when the "Set Order" button is clicked
                //console.log("Drag-and-drop finished. The order is updated, but we won't save it yet.");
            }
        };

        // Function to explicitly set the order after drag-and-drop
        $scope.sortableNumber = function () {
            var count = 0;
            angular.forEach($scope.RepeatDetails, function (dd) {
                count++;
                dd.IMMB_IVRMMID_Order = count; // Set sequential order based on current position
            });
            $scope.orderFlag = true;  // Mark that the order has been set
        };



        //SaveModuleOrder
        $scope.CityDistance = [];
        $scope.RepeatDetailsNew = [];
        $scope.SaveModuleOrder = function () {
            $scope.submitted1 = true;

           // if ($scope.myForm2.$valid) {
                $scope.RepeatDetailsTemp = [];
                if ($scope.RepeatDetails != null && $scope.RepeatDetails.length > 0) {
                    angular.forEach($scope.RepeatDetails, function (dd) {

                        $scope.RepeatDetailsTemp.push({
                            IMMB_ID: dd.IMMB_ID,
                            IMMB_IVRMMID: dd.IMMB_IVRMMID,
                            IMMB_BOMID: dd.IMMB_BOMID,
                            IMMB_IVRMMID_Order: dd.IMMB_IVRMMID_Order
                        })

                    })
                }
                var data = {
                    "ModuleDetails": $scope.RepeatDetailsTemp,

                };
                apiService.create("SalesDrBlocked/SaveModuleOrder", data).then(function (promise) {
                    if (promise.returndata == "update") {
                        swal("Module Order Updated Successfully  !");
                        $scope.OnLoadModuleNames();
                        //$state.reload();
                        $('#moduleordersetting').modal('hide');
                    }
                    else if (promise.returndata == "Notupdate") {
                        swal("Module Order Not Updated !");
                        $scope.OnLoadModuleNames();
                        $('#moduleordersetting').modal('hide');
                    }
                    else if (promise.returndata == "admin") {
                        swal("Plaese Conact Administrtor !");
                        $scope.OnLoadModuleNames();
                        $('#moduleordersetting').modal('hide');
                    }
                });
            //}
        }

        //save
        $scope.submitted = false;
        $scope.SaveData = function () {       
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.temparray = [];
                angular.forEach($scope.getModuleData, function (itm) {
                    if (itm.selected==true) {
                        $scope.temparray.push({ ivrmM_Id: itm.ivrmM_Id})
                    }
                });
                var data = {
                    "IVRMMIDS": $scope.temparray ,
                    "IMMB_BOMID": $scope.ismsboM_Id.ismsboM_Id,
                    "IMMB_InsType": $scope.selfassign
                }
                apiService.create("SalesDrBlocked/ISM_Master_Module_BOMSave", data).
                    then(function (promise) {
                        if (promise.returndata === "Add") {
                            swal('Record Saved Successfully !');
                        }
                        else if (promise.returndata === "Notsave") {
                            swal('Record Already Exists !');
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





        $scope.cancel = function () {
            $scope.ISMMCLTPR_Id = "";
            $scope.ISMCLTC_Id = "";
            $scope.ISMCLTPRBOM_Qty = "";
            $scope.ISMCLTPRBOM_Remarks = "";
        }

        $scope.cancel = function () {
            $scope.ismsboM_Id = null;
            $scope.getModuleData.forEach(function (user) {
                user.selected = false; // Uncheck the checkbox
            });
            // $state.reload();
        };

        //$scope.cancel = function () {
        //    $state.reload();
        //};

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired = function () {
            return !$scope.getModuleData.some(function (options) {
                return options.selected;
            });
        };


        $scope.deactiveY = function (flr, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var data = {
                "IMMB_ID": flr.IMMB_ID,
                "IMMB_BOMID": flr.IMMB_BOMID,
                "IMMB_IVRMMID": flr.IMMB_IVRMMID
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
                        apiService.create("SalesDrBlocked/MasterDataDeactivate", data).
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
    }
})();


angular.module('app').filter('customSearch', function () {
    return function (input, search) {
        if (!search) return input;
        return input.filter(function (item) {
            return item.ISMSBOM_BOMDescription && item.ISMSBOM_BOMDescription.toLowerCase().includes(search.toLowerCase()) ||
                item.IVRMM_ModuleName && item.IVRMM_ModuleName.toLowerCase().includes(search.toLowerCase())
                ;
        });
    };
});

 


