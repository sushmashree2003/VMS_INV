(function () {
    'use strict';
    angular
.module('app')
.controller('masterProfessionalTaxController', masterProfessionalTaxController)

    masterProfessionalTaxController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function masterProfessionalTaxController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

        // form Object
        $scope.PTax = {};

        // Datatable display
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                        {
                            name: 'SlNo', field: 'name', enableColumnMenu: false, enableHiding: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>'
                        },
                        {
                            name: 'hrmpT_SalaryFrom', displayName: 'Salary From', enableHiding: false
                        },
                        {
                            name: 'hrmpT_SalaryTo', displayName: 'Salary To', enableHiding: false
                        },
                        {

                            name: 'hrmpT_PTax', displayName: 'Professional Tax', enableHiding: false
                        },
                        {
                            field: 'id', name: '',
                            displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);" data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.hrmpT_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Activate"> Activate</a>' +
                        '<span ng-if="row.entity.hrmpT_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);" data-toggle="tooltip" title="Deactivate">  Deactivate</a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }

        };

        // Get form Details at onload 
        $scope.onloadGetData = function () {
            var pageid = 2;
            apiService.getURI("MasterProfessionalTax/getalldetails", pageid).then(function (promise) {
                if (promise.pTaxrList !== null && promise.pTaxrList.length > 0) {
                    $scope.gridOptions.data = promise.pTaxrList;
                }
            })
        }

        //compare both Amount
        $scope.checkErr = function (FromAmount, ToAmount) {
            if (parseFloat(FromAmount) > parseFloat(ToAmount)) {
                swal("Salary From should not be greater than Salary To", 'Please Change Your amount!');
                $scope.PTax.HRMPT_SalaryFrom = "";
                return false;
            }
        };

        $scope.checkErr1 = function (FromAmount, ToAmount) {
            if (parseFloat(FromAmount) > parseFloat(ToAmount)) {
                swal("Salary To should be greater than Salary From", 'Please Change Your amount!');
                $scope.PTax.HRMPT_SalaryTo = "";
                return false;
            }
        };

        // clear form data
        $scope.cancel = function () {
            $scope.PTax = {};
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.gridApi.grid.clearAllFilters();
        }
        //saving/updating Record
        $scope.submitted = false;
        $scope.saveData = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = $scope.PTax;
                apiService.create("MasterProfessionalTax/", data).
                then(function (promise) {

                    if (promise.retrunMsg !== "") {

                        if (promise.retrunMsg == "Duplicate") {
                            swal("Record already exist..!!");
                            return;
                        }

                        else if (promise.retrunMsg == "false") {
                            swal("Record Not saved / Updated..", 'Fail');

                        }
                        else if (promise.retrunMsg == "Add") {
                            swal("Record Saved Successfully..");
                        }
                        else if (promise.retrunMsg == "Update") {
                            swal("Record Updated Successfully..");
                        }
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                        }

                        if (promise.pTaxrList !== null && promise.pTaxrList.length > 0) {

                            $scope.gridOptions.data = promise.pTaxrList;
                        }
                        $scope.cancel();
                    }
                })
            }

        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


        // Edit Single Record
        $scope.EditData = function (record) {
            var id = record.hrmpT_Id;
            apiService.getURI("MasterProfessionalTax/editRecord", id).
                then(function (promise) {

                    if (promise.pTaxrList != null && promise.pTaxrList.length > 0) {
                        $scope.PTax = promise.pTaxrList[0];
                    }
                    
                })
        }
        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            if (data.hrmpT_ActiveFlag == false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }

            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Record..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
           function (isConfirm) {
               if (isConfirm) {
                   apiService.DeleteURI("MasterProfessionalTax/ActiveDeactiveRecord", data.hrmpT_Id).
                   then(function (promise) {


                       if (promise.retrunMsg !== "") {

                           if (promise.retrunMsg === "Activated") {
                               swal("Record Activated successfully");
                           }
                           else if (promise.retrunMsg === "Deactivated") {
                               swal("Record Deactivated successfully");
                           }
                           else {
                               swal("Record Not Activated/Deactivated", 'Fail');
                           }

                           if (promise.pTaxrList !== null && promise.pTaxrList.length > 0) {

                               $scope.gridOptions.data = promise.pTaxrList;
                           }
                       }

                   })
               }
               else {
                   swal(" Cancelled", "Ok");
               }
           }

           );
        }



    }

})();