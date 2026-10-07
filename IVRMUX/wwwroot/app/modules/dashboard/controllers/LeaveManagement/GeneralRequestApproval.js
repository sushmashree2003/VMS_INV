(function () {
    'use strict';
    angular
        .module('app')
        .controller('GeneralRequestApprovalController', GeneralRequestApprovalController)

    GeneralRequestApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache', '$filter', 'uiGridConstants', 'appSettings', '$sce']
    function GeneralRequestApprovalController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache, $filter, $uiGridConstants, appSettings, $sce) {

        var copty = "";
        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.loadData = function () {
            var id = 2;
            apiService.getURI("GeneralRequestProcess/getalldetails", id).then(function (promise) {                
                $scope.punchrequest = promise.punchrequest;              
                $scope.punchapproveddetails = promise.punchapproveddetails;              
                if ($scope.punchrequest.length === 0) {
                    swal("Data not Found !!");                  
                }
            });
        };
        $scope.punchdetail = function (item) {
            if ($scope.myForm.$valid) {
                var data = {
                    "FOEPAR_Id": item.FOEPAR_Id
                };
                apiService.create("GeneralRequestProcess/approvaldata", data).then(function (promise) {

                    if (promise.approvaldata.length > 0) {
                        $scope.temparraynew = promise.approvaldata;
                        $scope.ename = promise.approvaldata[0].ename;
                        $scope.FOEPAR_Remarks = promise.approvaldata[0].FOEPAR_Remarks;
                        $scope.FOEPAR_PunchDate = promise.approvaldata[0].FOEPAR_PunchDate;
                        $scope.FOEPARD_Time = promise.approvaldata[0].FOEPARD_Time;
                        $scope.FOEPARD_InOutFlg = promise.approvaldata[0].FOEPARD_InOutFlg;
                        $scope.FOEPAR_Id = promise.approvaldata[0].FOEPAR_Id;
                       
                    }
                    else {
                        swal("No Record  Found..... !!");

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
        $scope.save = function (obj) {
            if ($scope.myForm.$valid) {
                var data = {
                    "FOEPAR_Id": $scope.temparraynew[0].FOEPAR_Id,
                    "FOEPARA_ApprovalFlg": obj,
                    "FOEPARA_ApprovedTime": $filter('date')($scope.obj.FOEPARA_ApprovedTime, "hh:mma"),
                    "FOEPARA_Remarks": $scope.obj.FOEPARA_Remarks
                }
                apiService.create("GeneralRequestProcess/getstatus", data).then(function (promise) {

                    if (promise.retrunMsg == "Update") {
                        swal('Record saved successfully');
                        $state.reload();
                    }
                    else if (promise.retrunMsg == "false") {
                        swal('Record  Not saved successfully');
                    }                                    
                })
            }
            else {
                $scope.submitted = true;
            }

        };
      
        $scope.cancel = function () {
            $state.reload();
        };
    }
})();