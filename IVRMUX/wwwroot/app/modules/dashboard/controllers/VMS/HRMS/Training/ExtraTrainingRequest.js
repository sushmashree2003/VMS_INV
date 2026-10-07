(function () {
    'use strict';
    angular
        .module('app')
        .controller('TrainingController', trainingController)

    trainingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter']
    function trainingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter) {

        $scope.sortKey = 'Id';
        $scope.sortReverse = true;
        $scope.donce = false;
        $scope.maxdate = new Date();

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

        $scope.all_check = function () {
            var checkStatus2 = $scope.usercheck;
            angular.forEach($scope.trainingEmpList, function (itm) {
                itm.selectedd2 = checkStatus2;
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.trainingEmpList.some(function (sec) {
                return sec.selectedd2;
            });
        };
        $scope.togchkbx = function () {
            $scope.usercheck = $scope.trainingEmpList.every(function (role) {
                return role.selectedd2;
            });
        };


        $scope.HRDTA_Date = new Date();
        $scope.getAllDetail = function () {
            $scope.searchValue = "";
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.startdate1 = new Date($scope.HRDTA_Date).toDateString();
            var data = {
                "HRTCRINTTR_StartDate": $scope.startdate1
            };
            apiService.create("Induction_Training/loadinitialdata", data).
                then(function (promise) {
                    $scope.trainingEmpList = promise.trainingEmpList;
                    $scope.training_details_list = promise.training_details_list;
                });
        };


        //save
        $scope.submitted = false;
        $scope.itemsPerPage = 10;
        $scope.currentPage = 10;
        $scope.savetmpldata = function () {

            var startdate1 = $scope.date_of_birth === null ? "" : $filter('date')($scope.startdate, "yyyy-MM-dd");
            var enddate1 = $scope.date_of_birth === null ? "" : $filter('date')($scope.enddate, "yyyy-MM-dd");


            //$scope.employeeAP = [];
            //angular.forEach($scope.trainingEmpList, function (itm) {
            //    if (itm.selectedd2 === true) {
            //        $scope.employeeAP.push(itm.Hrme_id);
            //    }
            //});
            //$scope.hrme_ids = $scope.employeeAP.join(',');
            $scope.employeeAP = [];
            angular.forEach($scope.trainingEmpList, function (itm) {
                if (itm.selectedd2 === true) {
                    $scope.employeeAP.push({
                        HRME_Id: itm.Hrme_id
                    });
                }
            });
 
            if ($scope.myForm.$valid) {
                var data = {
                    "HRTCRINTTR_Id": $scope.HRETR_Id,
                    "desgination": $scope.description,
                    "emplyee": $scope.employeeAP,
                    "START_DATE": startdate1,
                    "END_DATE": enddate1,
                    "gradename":"Save"
                };
                apiService.create("Induction_Training/SaveExtraTrngRqst", data).then(function (promise) {

                    if (promise.returnvalue !== "") {

                        if (promise.message === "Update") {
                            swal('Record Updated Successfully');
                            $state.reload();
                            return;
                        }
                        else if (promise.message === "Save") {
                            swal('Record Saved Successfully');
                            $state.reload();
                            return;
                        }
                        else if (promise.message === "NotSaved") {
                            swal('Record Not Saved successfully');
                            return;
                        }
                        else if (promise.returnvalue === "Duplicate") {
                            swal('Category Already Exist');
                            $state.reload();
                            return;
                        }
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


    
        $scope.interacted = function (field) {
            return $scope.submitted;
        };



        //ActiveDeactive
        $scope.deactive = function (flr, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (flr.hrmetR_ActiveFlag === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Academic Year?",
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
                        apiService.create("Training_Master/deactivate_T", flr).
                            then(function (promise) {

                                if (promise.hrmetR_ActiveFlag === true) {
                                    if (promise.returnval === false) {
                                        swal('Master Training Deactivated Successfully');
                                    }
                                }
                                else if (promise.hrmetR_ActiveFlag === false) {
                                    swal('Master Training Activated Successfully');
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



        //cancel
        $scope.cancel = function () {
            $state.reload();
        }



        //edit
        var DATEBIRTH = $scope.date_of_birth === null ? "" : $filter('date')($scope.date_of_birth, "yyyy-MM-dd");
        $scope.edibl = {}
        $scope.edit = function (bil) {
            $scope.edibl = bil.hrmetR_Id;
            var pageid = $scope.edibl;
            apiService.getURI("Training_Master/details_T", pageid).
                then(function (promise) {

                    $scope.id = promise.train_list[0].hrmetR_Id;
                    $scope.trainer_Name = promise.train_list[0].hrmetR_Name;
                    $scope.address = promise.train_list[0].hrmetR_Address;
                    $scope.date_of_birth = new Date(promise.train_list[0].hrmetR_DOB);
                    $scope.city = promise.train_list[0].hrmetR_City;
                    $scope.pin_code = promise.train_list[0].hrmetR_Pincode;
                    $scope.Email = promise.train_list[0].hrmetR_EmailId;
                    $scope.contact_no = promise.train_list[0].hrmetR_ContactNo;
                    $scope.Domain_exp = promise.train_list[0].hrmetR_DomainExp;
                    $scope.Training_Exp = promise.train_list[0].hrmetR_TrainingExp;
                    $scope.skills = promise.train_list[0].hrmetR_Skills;
                    $scope.hiringType = promise.train_list[0].hrmetR_ParttimeORFullTimeFlg;
                    $scope.Remark = promise.train_list[0].hrmetR_Remarks;
                    $scope.file_detail = promise.train_list[0].hrmetR_Image_Name;
                    $scope.ivrmmG_Id = promise.train_list[0].ivrmmG_Id;
                    $scope.hrmeQ_Id = promise.train_list[0].hrmeQ_Id;
                    $scope.ivrmmG_GenderName = promise.train_list[0].ivrmmG_GenderName;
                    $scope.hrmE_QualificationName = promise.train_list[0].hrmE_QualificationName;
                    $('#blah').attr('src', promise.train_list[0].hrmetR_ImagePath);
                    $scope.obj.image = promise.train_list[0].hrmetR_ImagePath;




                })
        };


    }

})();



