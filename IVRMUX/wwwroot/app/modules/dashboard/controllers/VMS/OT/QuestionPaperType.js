
(function () {
    'use strict';
    angular
.module('app')
        .controller('QuestionPaperTypeController', QuestionPaperTypeController)

    QuestionPaperTypeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function QuestionPaperTypeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.sortKey = 'trmA_Id';
        $scope.sortReverse = true;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.searchValue = "";
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("QuestionPaperType/getalldetails", pageid).then(function (promise) {
                if (promise !== null) {
 
                    $scope.milist = promise.milist;
                    
                }
                else {
                    swal("No Records Found");
                }
            })

        }



        $scope.oncmpchange = function () {
            var data = {
                "MI_Id": $scope.MI_Id,

            };
            apiService.create("QuestionPaperType/loadcdata", data).then(function (promise) {
                $scope.positionlist = promise.positionlist;
                $scope.qnstypelist = promise.qnstypelist;
            });

        };

        $scope.submitted = false;
      

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        var counthrs = 0.00;
        var totalhrs = 0.00;
        //-------Save Data-------//
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {

                //counthrs = (parseFloat($scope.effmin) * 0.0166667).toFixed(2);
                //counthrs = parseFloat($scope.effhrs) + parseFloat(counthrs);

                var data = {
                    "HRMP_Id": $scope.HRMP_Id,
                    "OTQPTYP_Id": $scope.OTQPTYP_Id,
                    "OTQPTYP_QuestionPaperName": $scope.OTQPTYP_QuestionPaperName,
                    "OTQPTYP_QuestionPaperDesc": $scope.OTQPTYP_QuestionPaperDesc,
                    "OTQPTYP_MaxQuestionsPerExam": $scope.OTQPTYP_MaxQuestionsPerExam,
                    "OTQPTYP_MaxDuration": $scope.effmin,
                    "MI_Id": $scope.MI_Id,
                }
                apiService.create("QuestionPaperType/savedata", data).then(function (promise) {
                    if (promise != null) {
                        if (promise.retrunMsg == "Duplicate") {
                            swal("Record Already Exists");

                        }
                        else if (promise.retrunMsg == "Error") {
                            swal("Error");
                        }
                        else {
                            if (promise.retrval == true) {
                                swal("Record Saved Successfully");
                            }
                            else {
                                swal("Failed To Save Record");
                            }
                        }
                    }
                    $state.reload();
                });
            }
            else
            {
                $scope.submitted = true;
            }           
        }
       
        //--Cancel--//
        $scope.cancle = function () {
            $scope.TRMA_Id = 0;
            $state.reload();
        }

        $scope.searchValue = '';
      

        //---Edit Data--//
        $scope.edit = function (user) {
            //var data = {
            //    "OTQPTYP_Id": user.otqptyP_Id
            //}
            apiService.getURI("QuestionPaperType/editData", user.otqptyP_Id).then(function (Promise) {
                if (Promise != null) {
                    $scope.OTQPTYP_QuestionPaperName = Promise.editlist[0].otqptyP_QuestionPaperName;
                    $scope.OTQPTYP_QuestionPaperDesc = Promise.editlist[0].otqptyP_QuestionPaperDesc;
                    $scope.OTQPTYP_MaxQuestionsPerExam = Promise.editlist[0].otqptyP_MaxQuestionsPerExam;
                    if (Promise.editlist[0].otqptyP_MaxDuration != null && Promise.editlist[0].otqptyP_MaxDuration != '' && Promise.editlist[0].otqptyP_MaxDuration != undefined) {
                        $scope.effmin = parseInt(Promise.editlist[0].otqptyP_MaxDuration);
                    }
                   
                    $scope.HRMP_Id = Promise.editlist[0].hrmP_Id;
                    $scope.OTQPTYP_Id = Promise.editlist[0].otqptyP_Id;
                }
            })
        }

        //--Active Deactive---//
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.otqptyP_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";

            }
            else {

                mgs = "Active";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {
                apiService.create("QuestionPaperType/deactivate/", user).
                then(function (promise) {
                    if (promise.retrval == true) {
                            swal(confirmmgs + " " + "Successfully.");
                            $state.reload();
                        }
                        else {
                            swal(confirmmgs + " " + " Successfully");
                            $state.reload();
                        }
                   
                })
            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
            $state.reload();
        });
        }
 
        //--Sorting--//     
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        
    };
})();


