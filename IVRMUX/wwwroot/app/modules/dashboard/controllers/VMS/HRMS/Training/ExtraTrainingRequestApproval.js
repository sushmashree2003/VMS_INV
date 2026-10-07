(function () {
    'use strict';
    angular.module('app').controller('TrainingController', TrainingController)

    TrainingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache', '$filter', 'uiGridConstants', 'appSettings', '$sce']
    function TrainingController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache, $filter, $uiGridConstants, appSettings, $sce) {
        var copty = "";
        $scope.rowsSelected = [];



        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;

        $scope.currentPage = 1;
        $scope.currentPage2 = 1;
        $scope.coptyright = copty;

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.hrmE_EmployeeFirstName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrmL_LeaveName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrelT_Status)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        };


        $scope.training_details_list = [];
        $scope.training_details_Grid = [];

        $scope.HRDTA_Date = new Date();
        $scope.loadData = function () {
            $scope.startdate1 = new Date($scope.HRDTA_Date).toDateString();
            var data = {
                "HRTCRINTTR_StartDate": $scope.startdate1
            };
            apiService.create("Induction_Training/loadinitialdata", data).
                then(function (promise) {
                    $scope.training_details_list1 = promise.training_details_list;

                    angular.forEach($scope.training_details_list1, function (row) {
                        if (row.HRETR_Status == 'Pending') {
                            $scope.training_details_list.push(row);
                        }
                    });

                    angular.forEach($scope.training_details_list1, function (row) {
                        if (row.HRETR_Status != 'Pending') {
                            $scope.training_details_Grid.push(row);
                        }
                    });
                });
        };



        $scope.submitted = false;

        $scope.get_status = function (leavestatus) {
            var temp_array = [];
            $scope.approveorreject = "";
            if ($scope.myForm.$valid) {
                var statusswal = "";
                if (leavestatus == "Approved") {
                    statusswal = "Approved";
                }
                else {
                    statusswal = "Rejected";
                }

                swal({
                    title: "Are you sure",
                    text: "Do you want to" + statusswal,
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + statusswal + " it!",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            angular.forEach($scope.training_details_list, function (row) {
                                if (row.selected == true) {
                                    temp_array.push({
                                        HRMTT_Id: row.HRETR_Id,
                                        HRMTT_Topic: row.HRETR_Remarks,
                                    });
                                }
                            });
                            var data = {
                                trainingTopicdetails: temp_array,
                                "returnvalue": statusswal,
                                "gradename": "Approval"
                            };

                            apiService.create("Induction_Training/SaveExtraTrngRqst", data).then(function (promise) {

                                if (promise.message === "Save") {
                                    swal('Record Saved Successfully');
                                    $state.reload();
                                    return;
                                }
                                else if (promise.message === "NotSaved") {
                                    swal('Record Not Saved successfully');
                                    return;
                                }
                                else {
                                    swal("Something went wrong!!");
                                    $state.reload();
                                }
                            });
                        }
                        else {
                            swal("Something went wrong!!");
                            $state.reload();
                        }
                    });
            }
            else {
                $scope.approveorreject = "Approve";
                $scope.submitted = true;
            }
        };






        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.GetLeave = function (leave) {
            if (leave.selected) {
                $scope.rowsSelected.push(leave);
            }
            else {
                const ind = $scope.rowsSelected.indexOf(leave);
                if (ind > -1) {
                    $scope.rowsSelected.splice(ind, 1);
                }
            }
        }
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.training_details_list, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.rowsSelected.indexOf(itm) === -1) {
                        $scope.rowsSelected.push(itm);
                    }
                }
                else {
                    $scope.rowsSelected.splice(itm);
                }
            });
        }

        $scope.isOptionsRequired = function () {
            return !$scope.training_details_list.some(function (options) {
                return options.selected;
            });
        };




    }
})();