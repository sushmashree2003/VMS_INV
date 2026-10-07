

(function () {
    'use strict';
    angular
.module('app')
        .controller('OnlineExamConfigController', OnlineExamConfigController)

    OnlineExamConfigController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function OnlineExamConfigController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {


        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }


        $scope.submitted = false;

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        
     
        $scope.loaddata = function () {
            var pageid = 2;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            apiService.getURI("OnlineExamConfig/getloaddata", pageid).
            then(function (promise) {
                $scope.getQuestion = promise.getQdetails;
               
            })
        };


        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.pamoeS_Id;

            var data = {
                "PAMOES_Id": $scope.editEmployee
            }

            apiService.create("OnlineExamConfig/editQuestion", data).
                then(function (promise) {
                    $scope.pamoeS_Id = promise.editQus[0].pamoeS_Id;
                    $scope.Noofques = promise.editQus[0].pamoeS_NoofQns;
                    $scope.totmrks = promise.editQus[0].pamoeS_TotalMarks;
                    $scope.totdur = promise.editQus[0].pamoeS_TotalDuration;
                    $scope.echmrkques = promise.editQus[0].pamoeS_EachQnsMarks;
                    $scope.echquesdur = promise.editQus[0].pamoeS_EachQnsDuration;
                    $scope.noopt = promise.editQus[0].pamoeS_NoOfOptions;
              
                })
        }




        $scope.savedata = function () {
            debugger;
            var data = {
                "PAMOES_Id": $scope.pamoeS_Id,
                "Noofques": $scope.Noofques,
                "totmrks": $scope.totmrks,
                "totdur": $scope.totdur,
                "echmrkques": $scope.echmrkques,
                "echquesdur": $scope.echquesdur,
                "noopt": $scope.noopt
            }
            apiService.create("OnlineExamConfig/savedata", data).
                then(function (promise) {

                    $scope.getQuestion = promise.getQuestion;
                    if (promise.returnval === true) {
                        if (promise.returnval === true) {
                            if (promise.message != null) {
                                swal('Record Updated Successfully', 'success');
                                $state.reload();
                            }
                            else {
                                swal('Record Saved Successfully', 'success');
                                $state.reload();
                            }
                        }
                    }
                    else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record Already Exist');
                    }
                    else {
                        if (promise.message != null) {
                            swal('Record Not Updated', 'success');
                        }
                        else {
                            swal('Record Not Saved', 'success');
                        }
                    }
                    $state.reload();
        })
            
        };


        $scope.Clearid = function () {
            $state.reload();
        }


        $scope.DeletRecord = function (pamoeS_Id) {
            var data = {
                "PAMOES_Id": pamoeS_Id,
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("OnlineExamConfig/Deletedetails/", data).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal('Record Deleted Successfully');
                                }
                                else if (promise.returnval == false) {
                                    swal('Contact Administrator');
                                }
                                else if (promise.returnval == "RecordExists") {
                                    swal('Data has already been used So record cannot be deleted');
                                }
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                    $state.reload();
                });
        }
 }


})();