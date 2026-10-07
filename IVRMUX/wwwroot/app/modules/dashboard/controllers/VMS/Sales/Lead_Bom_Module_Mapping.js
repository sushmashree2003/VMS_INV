(function () {
    'use strict';
    angular.module('app').controller('SalesDrBlockedController', SalesDrBlockedController)
    SalesDrBlockedController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function SalesDrBlockedController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {

        $scope.itemsPerPage = 1;
        $scope.currentPage = 1;

        $scope.objques = {};

        $scope.OnLoadModuleNames = function () {
            $scope.LeadMappedModules = [];
            $scope.BomList = [];
            $scope.LeadDetails = [];
            $scope.MasterModulesList = [];
            var pageid = 1;

            apiService.getURI("Lead_Target_Mappingnew/OnloadGetDetails", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.BomList = promise.employeedropdown;
                    $scope.LeadDetails = promise.status_dd;
                    $scope.MasterModulesList = promise.emp_list;

                    $scope.LeadMappedModules = promise.emp_target_count;


                    //for displaying grid
                    $scope.bomids = [];

                    angular.forEach($scope.LeadMappedModules, function (dev) {
                        if ($scope.bomids.length === 0) {
                            $scope.bomids.push(dev);
                        } else if ($scope.bomids.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.bomids, function (emp) {
                                if (emp.ISMSLE_Id === dev.ISMSLE_Id) {
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

                        angular.forEach($scope.LeadMappedModules, function (dd) {
                            if (dd.ISMSLE_Id === ddd.ISMSLE_Id) {
                               
                                    $scope.templist.push(dd);
                               

                               
                            }
                        });
                        ddd.datadetails = $scope.templist;

                    });


                    console.log($scope.bomids);

                }
                else {
                    swal("No Record  Found..... !!");
                  
                }
            });
        };

        $scope.selectedModules = [];
        // selection
        $scope.all_checkm = function (checkallm) {
            var toggleStatus = checkallm;
            angular.forEach($scope.MasterModulesList, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.searchValueDesg = '';

        };


        $scope.togchkbxm = function () {

            $scope.checkallm = $scope.MasterModulesList.every(function (itm) {
                return itm.selected;
            });

            $scope.searchValueDesg == '';

        };




      

        //save
        $scope.submitted = false;
        $scope.SaveData = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.tagtedit = [];
                angular.forEach($scope.MasterModulesList, function (itm) {
                    if (itm.selected == true) {
                        $scope.tagtedit.push({ ISMMTCAT_Id: itm.IVRMM_Id })
                    }
                });
                var data = {
                    "tagtedit": $scope.tagtedit,
                    "ISMSLETARM_Id": $scope.ISMSBOM_Id.ISMSBOM_Id,
                    "ISMSLETARM_Numbers": $scope.ISMSLE_Id.ISMSLE_Id
                }
                apiService.create("Lead_Target_Mappingnew/SaveTarget", data).
                    then(function (promise) {
                        if (promise.retrunMsg === "Add") {
                            swal('Record Saved Successfully !');
                        }
                        else if (promise.retrunMsg === "NotAdd") {
                            swal('Record Already Exists !');
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


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired = function () {
            return !$scope.MasterModulesList.some(function (options) {
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
                //"ISMSLETARM_Id": flr.ISMSLBOM_Id
                 "ISMMTCAT_Id": flr.ISMSLBOM_Id
                
            }
            var mgs = "";
            if (flr.ISMSLBOM_ActiveFlag === false) {
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
                        apiService.create("Lead_Target_Mappingnew/SaveTarget", data).
                            then(function (promise) {


                                if (promise.retrunMsg === 'false') {
                                    swal('Record ' + mgs + ' Successfully.');
                                }

                                else if (promise.retrunMsg === 'true') {
                                    swal('Activated Successfully.');
                                }

                                else if (promise.retrunMsg === 'Error') {
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
            return item.ISMSLE_LeadName.toLowerCase().includes(search.toLowerCase()) ||
                item.ISMSBOM_BOMName.toLowerCase().includes(search.toLowerCase());
        });
    };
});