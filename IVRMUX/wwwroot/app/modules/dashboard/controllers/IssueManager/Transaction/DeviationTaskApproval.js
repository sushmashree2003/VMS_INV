(function () {
    'use strict';
    angular.module('app').controller('DeviationTaskApprovalController', DeviationTaskApprovalController);
    DeviationTaskApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', 'Excel', '$timeout', '$sce'];
    function DeviationTaskApprovalController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter, Excel, $timeout, $sce) {


        var paginationformasters = 10;

        $scope.obj = {};
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};


        $scope.submitted = false;

        /* Load Data */
        $scope.loaddata = function () {
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            $scope.currentPage2 = 1;
            $scope.itemsPerPage2 = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.currentPageapp = 1;
            $scope.itemsPerPageapp = 10;
            $scope.page = "page";
            $scope.page1 = "page1";
            $scope.page2 = "page2";
            $scope.pageapp = "pageapp";
            $scope.search = "";
            var pageid = 2;
            var data = {
                "Id": pageid
            }
            apiService.create("ISM_Task_Deviation/OnloadApproval", data).then(function (promise) {

                $scope.get_userEmplist = promise.get_userEmplist;
                $scope.empwise_statuslist = promise.empwise_statuslist;
                

            });
        };

              $scope.Viewrequestedtask = function (objp) {            $scope.get_taskdetails = [];            var data = {
                "HRME_Id": objp.HRME_Id
            }
            apiService.create("ISM_Task_Deviation/Getrequestdetails", data).then(function (promise) {

                if (promise.get_taskdetails != null) {
                    $scope.get_taskdetails = promise.get_taskdetails;
                    $('#deviationtask').modal('show');
                }
            });        };





        // save task date extention        $scope.submitted1 = false;        $scope.SaveApproval = function (data) {            $scope.status = '';            if (data == 'Approved') {
                $scope.status = 'Approved';
            }
            else {
                $scope.status = 'Rejected';
            }
            $scope.DeviationtaskApprovalDetails = [];            //if ($scope.myForm1.$valid) {            if ($scope.get_taskdetails != null && $scope.get_taskdetails.length > 0) {                angular.forEach($scope.get_taskdetails, function (ss) {                    if (ss.checkedTask == true) {                        $scope.DeviationtaskApprovalDetails.push({                            ISMTCR_Id: ss.ISMTCR_Id,                            ISMTDE_Id: ss.ISMTDE_Id,                            ISMTDE_TaskExtendDate: new Date(ss.ISMTDE_TaskExtendDate).toDateString(),                            ISMTDEA_Remarks: ss.Extend_AppRemarks                        });                    }                })            }            if ($scope.DeviationtaskApprovalDetails != null && $scope.DeviationtaskApprovalDetails.length > 0) {                              var data = {                    "TaskExtentionApproval": $scope.DeviationtaskApprovalDetails,                    "Appstatus": $scope.status                }                apiService.create("ISM_Task_Deviation/SaveApprovalExtention", data).then(function (promise) {                                       if (promise.message == "save" && $scope.status == "Approved") {                        swal("Approved The Requested Tasks Successfully");                    }                    else if (promise.message == "notsave" && $scope.status == "Approved" ) {                        swal("Failed To Approved The Requested Tasks ");                    }                    else if (promise.message == "save" && $scope.status == "Rejected") {                        swal("Rejected The Requested Tasks Successfully");                    }                    else if (promise.message == "notsave" && $scope.status == "Rejected") {                        swal("Failed To Reject The Requested Tasks ");                    }                    else {                        swal('Please Contact Administrator');                    }                    $('#deviationtask').modal('hide');                    $scope.loaddata();                });            }            else {                swal('Please Select  Atleast one CheckBox');            }            //}            //else {            //    $scope.submitted1 = true;            //}        };


        $scope.ViewDetails = function (obj) {

            var data = {
                "HRME_Id": obj.HRME_ID, 
            };

            apiService.create("ISM_Task_Deviation/ViewApproveDetails", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getsaveddetails !== null && promise.getsaveddetails.length > 0) {
                        $scope.getsaveddetails = promise.getsaveddetails;
                        $('#viewdetails').modal('show');
                    } else {
                        swal("No Records Found");
                        $('#viewdetails').modal('hide');
                    }
                }
            });
        };






        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        //===========Clear Field
        $scope.Clear = function () {
            $state.reload();
        };

        $scope.isOptionsRequired = function () {
            return !$scope.get_userEmplist.some(function (options) {
                return options.empck;
            });
        };


    }
})();