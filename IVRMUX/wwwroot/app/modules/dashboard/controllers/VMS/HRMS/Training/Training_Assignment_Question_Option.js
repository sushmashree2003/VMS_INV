(function () {
    'use strict';
    angular
        .module('app')
        .controller('TrainingAssigqnsoptController', TrainingAssigqnsoptController)

    TrainingAssigqnsoptController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function TrainingAssigqnsoptController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.sortKey = 'HRTAQOPT_Id';
        $scope.sortReverse = true;


        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        };

        $scope.all_check = function () {
            var checkStatus2 = $scope.usercheck;
            angular.forEach($scope.option_list, function (itm) {
                itm.selectedd2 = checkStatus2;
            });
        };

        $scope.isOptionsRequired = function () {

            return !$scope.option_list.some(function (sec) {
                return sec.selectedd2;
            });
        };
        $scope.togchkbx = function () {

            $scope.usercheck = $scope.option_list.every(function (role) {
                return role.selectedd2;
            });
        };
        $scope.option_view = function (ev) {

            $scope.eve = ev.hrtaqnS_Id;
            var pageid = $scope.eve;
            apiService.getURI("Training_Assignment_Question_Option/option_view_TAQO", pageid).then(function (promise) {

                $scope.option_view_list = promise.option_view_list;

            });

        };
        //get data
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            var pageid = 2;
            apiService.getURI("Training_Assignment_Question_Option/getdata_TAQO", pageid)
                .then(function (promise) {
                    $scope.question = false;
                    $scope.question_list = promise.question_list;
                    $scope.option_list = promise.option_list;
                    $scope.question_option_list = promise.question_option_list;
                    $scope.presentCountgrid = $scope.question_option_list.length;

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
            if (bL.hrtaqopT_ActiveFlg === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Question Option ?",
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
                        apiService.create("Training_Assignment_Question_Option/deactivate_TAQO", bL).
                            then(function (promise) {


                                if (promise.returnval === false) {
                                    swal('Training Assignment Question Option Deactivated Successfully');
                                    $state.reload();
                                }


                                else if (promise.returnval === true) {
                                    swal('Training Assignment Question Option Activated Successfully');
                                    $state.reload();
                                }



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

        // edit
        $scope.edibl = {};
        $scope.edit = function (bil) {
            $scope.edibl = bil.hrtaqnS_Id;
            var pageid = $scope.edibl;
            apiService.getURI("Training_Assignment_Question_Option/details_TAQO", pageid).
                then(function (promise) {
                    $scope.option_list1 = [];
                    $scope.question = true;
                    //$scope.question_list = promise.question_details_list;
                    $scope.hrtaqnS_QuestionName = promise.question_details_list[0].hrtaqnS_QuestionName;
                    $scope.Id = promise.question_details_list[0].hrtaqnS_Id;
                    $scope.HRTAQNS_Id = promise.question_details_list[0].hrtaqnS_Id;
                    $scope.option_list1 = promise.option_details_list;
                    $scope.option_list = promise.option_list;

                    if ($scope.option_list1.length > 0) {
                        angular.forEach($scope.option_list, function (ss) {
                            angular.forEach($scope.option_list1, function (qq) {
                                if (ss.hrtaopT_Id == qq.hrtaopT_Id)
                                    ss.selectedd2 = true;
                            });//HRTAOPT_Id


                        });
                    }
                });
        };



    
        //save
        $scope.submitted = false;
        $scope.savetmpldata = function () {

            $scope.Feedback_Option = [];
            angular.forEach($scope.option_list, function (cls) {
                if (cls.selectedd2 === true) {
                    $scope.Feedback_Option.push(cls);
                }
            });
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HRTAQNS_Id": $scope.HRTAQNS_Id,
                    Feedback_Option: $scope.Feedback_Option,
                    "hqn_id": $scope.Id
                };
                apiService.create("Training_Assignment_Question_Option/SaveEdit_TAQO", data).
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

    }
})();