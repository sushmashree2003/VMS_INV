(function () {
    'use strict';
    angular
        .module('app')
        .controller('TrainingAssigoptController', TrainingAssigoptController)

    TrainingAssigoptController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function TrainingAssigoptController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.sortKey = 'HRTAOPT_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        };

        //get data
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("Training_Assignment_Option/getdata_MFO", pageid)
                .then(function (promise) {
                    $scope.question_list = promise.question_list;
                    $scope.presentCountgrid = $scope.question_list.length;
                });
        };

        //save
        $scope.submitted = false;
        $scope.savetmpldata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HRTAOPT_OptionName": $scope.HRTAOPT_OptionName,
                    "HRTAOPT_OptionOrder": $scope.HRTAOPT_OptionOrder,
                    "HRTAOPT_Id": $scope.Id
                };
                apiService.create("Training_Assignment_Option/SaveEdit_MFO", data).
                    then(function (promise) {
                        if (promise.returnvalue !== "") {
                            if (promise.returnvalue == "Update") {
                                swal('Record Updated Successfully');
                            }
                            else if (promise.returnvalue == "Add") {
                                swal('Record Saved Successfully');
                            }
                            else if (promise.returnvalue == "false") {
                                swal('Record Not Saved/Updated successfully');
                            }
                            else if (promise.returnvalue === "Duplicate") {
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
        $scope.Clearid = function () {
            $state.reload();
        };
        // edit
        $scope.edibl = {};
        $scope.edit = function (bil) {
            $scope.edibl = bil.hrtaopT_Id;
            var pageid = $scope.edibl;
            apiService.getURI("Training_Assignment_Option/details_MFO", pageid).
                then(function (promise) {
                    $scope.Id = promise.question_list[0].hrtaopT_Id;
                    $scope.HRTAOPT_OptionName = promise.question_list[0].hrtaopT_OptionName;
                    $scope.HRTAOPT_OptionOrder = promise.question_list[0].hrtaopT_OptionOrder;
                });
        };

        //deactive
        $scope.deactive = function (bL, SweetAlertt) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (bL.hrmfopT_ActiveFlg === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Feedback Option ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Training_Assignment_Option/deactivate_MFO", bL).
                            then(function (promise) {

                                if (promise.hrtaopT_ActiveFlg === true) {
                                    if (promise.returnval === false) {
                                        swal('Training Assignment Option Deactivated Successfully');
                                    }
                                }
                                else if (promise.hrtaopT_ActiveFlg === false) {
                                    swal('Training Assignment Option Activated Successfully');
                                }
                                //   }

                                $state.reload();
                            });
                    } else {
                        swal("Cancelled");
                        $state.reload();
                    }

                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


    }

})();






