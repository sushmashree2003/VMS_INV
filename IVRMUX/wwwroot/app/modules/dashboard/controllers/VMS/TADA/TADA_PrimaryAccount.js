
(function () {
    'use strict';
    angular
        .module('app')
        .controller('TADARoot_CategoryController', TADARoot_CategoryController)

    TADARoot_CategoryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'superCache']
    function TADARoot_CategoryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, superCache) {

        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;

        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("TADARoot_Category/getalldata", pageid).then(function (promise) {

                $scope.tADA_PrimaryAccountList = promise.tADA_PrimaryAccountList;
                $scope.Route_PrimaryDetails = promise.route_PrimaryDetails;
                $scope.GetEmployeeList = promise.geatLoadCategory;
                $scope.primarytypeCount = promise.primarytypeCount;
            })
        }
        $scope.searchchkbx1 = "";
        $scope.all_checkC = function () {
            var checkStatus = $scope.usercheckC;
            angular.forEach($scope.tADA_PrimaryAccountList, function (itm) {
                itm.selected = checkStatus;
            });
        };
        $scope.togchkbxC = function () {
            $scope.usercheckC = $scope.tADA_PrimaryAccountList.every(function (options) {
                return options.selected;
            });
        };
        $scope.isOptionsRequired = function () {
            return !$scope.tADA_PrimaryAccountList.some(function (options) {
                return options.selected;
            });
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted;
        };
        $scope.clear = function () {
            $scope.ISMSMSO_Id = "";
        };

        $scope.submitted = false;
        $scope.savedata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.arrayPrimaryAccount = [];
                angular.forEach($scope.tADA_PrimaryAccountList, function (em) {
                    if (em.selected === true) {
                        $scope.arrayPrimaryAccount.push(em);
                    }
                });
                var data =
                {
                    //"ISMSMSO_ID": $scope.ISMSMSO_Id
                    "HRME_Id": $scope.HRME_Id.HRME_Id,
                    "ArrayPrimaryAccount": $scope.arrayPrimaryAccount

                };
                apiService.create("TADARoot_Category/Saveprimary", data).then(function (promise) {

                    if (promise.message == "success") {
                        swal("Saved successfully");
                    }
                    else {
                        swal("Saved Failed");
                    }
                    $state.reload();

                });
            }
            else {
                $scope.submitted = true;
            }

        }
        $scope.SetOrder = function (dd) {
            $scope.eycE_Id_temp = dd.eycE_Id;
            var data = {
                "HRME_Id": dd.HRME_Id
            };

            apiService.create("TADARoot_Category/SetOrder", data).then(function (promise) {
                if (promise !== null) {
                    $scope.primarytypelist = promise.primarytypelist;
                }
            });
        };
        $scope.ActivatePrimary = function (primary, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (primary.IRPA_status === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: true,
                closeOnCancel: true
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("TADARoot_Category/ActivatePrimary/", primary).then(function (promise) {
                            if (promise.message != null) {
                                swal(promise.message);
                            }
                            else {
                                if (promise.returnval == true) {
                                    swal(confirmmgs + " " + "Successfully");
                                }
                                else {
                                    swal(confirmmgs + " " + " Successfully");
                                }
                            }
                            $scope.onLoadGetData();
                            $('#myModal').modal('hide');
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.primarytypelist) {
                    $scope.primarytypelist[index].IRPA_order = Number(index) + 1;

                }
            }
        };


        $scope.resetLists = function () {
            $scope.configA = {

                onUpdate: function (evt) {
                    var itemEl = evt.item;

                }
            };
        };
        $scope.init = function () {

            $scope.resetLists();

        };

        $scope.init();
        $scope.getOrder = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.IRPA_ID !== 0) {
                    orderarray[key].IRPA_order = key + 1;
                }
            });
            var data = {
                Primaryaccount: orderarray,
            }
            apiService.create("TADARoot_Category/validateordernumber", data).
                then(function (promise) {
                    if (promise.returnvalue === true) {
                        swal('Record Saved/Updated Successfully');

                    }
                    else {
                        swal('Record not Saved/Updated Successfully');
                    }
                    $scope.onLoadGetData()
                });

        }


        $scope.clear = function () {
            $state.reload();
          
        }

    }
})();