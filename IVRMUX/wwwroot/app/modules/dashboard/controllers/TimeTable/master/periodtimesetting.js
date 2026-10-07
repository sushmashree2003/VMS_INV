
(function () {
    'use strict';
    angular
.module('app')
.controller('PeriodTimeSettingController', PeriodTimeSettingController)

    PeriodTimeSettingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter']
    function PeriodTimeSettingController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter) {
        $scope.editEmployee = {};

        // Time picker starts
        //$scope.timedis = true;
        //$scope.ScheduleTime = new Date();

        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        // Time picker end here

        //Ui Grid view rendering data from data base
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                 { name: 'Sl No', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'asmaY_Year', displayName: 'Academic Year' },
              { name: 'ttmC_CategoryName', displayName: 'Category Name' },
            { name: 'ttmP_PeriodName', displayName: ' Period' },
            { name: 'ttmD_DayName', displayName: 'Day' },
             { name: 'ttmdpT_StartTime', displayName: 'Start Time' },
             { name: 'ttmdpT_EndTime', displayName: 'End Time' },

            {
                field: 'id', name: '',
                displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                '<div class="grid-action-cell">' +
                '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>' +
               '<a ng-if="row.entity.ttmdpT_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive1(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
          '<span ng-if="row.entity.ttmdpT_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive1(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                '</div>'
            }
            ]

        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "TTMDPT_Id": $scope.TTMDPT_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "TTMC_Id": $scope.ttmC_Id,
                    "TTMP_Id": $scope.ttmP_Id,
                    "TTMD_Id": $scope.ttmD_Id,
                    "TTMDPT_StartTime": $filter('date')($scope.StartTime, "h:mm a"),
                    "TTMDPT_EndTime": $filter('date')($scope.EndTime, "h:mm a"),

                }
                apiService.create("PeriodTimeSetting/savedetail", data).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            swal('Data Updated successfully');                           
                            $scope.clearid();
                            $scope.BindData();
                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                            swal('Records Already Exist !');

                        }
                        else {
                            swal('Data Not Saved !');

                        }
                    })
            }
            else {
                $scope.submitted = true;

            }

        };

        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("PeriodTimeSetting/getalldetails").
       then(function (promise) {
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;
           $scope.AYear = promise.academicdrp;
           $scope.Category = promise.categorylist;
           $scope.Day = promise.daydrp;
           $scope.Period = promise.perioddrp;
           $scope.gridOptions.data = promise.gridview;
       })
        };
        //TO clear  data
        $scope.clearid = function () {
            
            $scope.TTMC_CategoryName = "";
            $scope.TTMC_Id = 0;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
            $scope.StartTime = "";
            $scope.EndTime = "";
            $scope.asmaY_Id = "";
            $scope.ttmD_Id = "";
            $scope.ttmC_Id = "";
            $scope.ttmP_Id = "";
            $scope.TTMDPT_Id =0;
        };
        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
            
            $scope.editperiod = employee.ttmdpT_Id;
            var editid = $scope.editperiod;
            angular.forEach($scope.classlist, function (role) {
                role.class = false;
            })
            apiService.getURI("PeriodTimeSetting/getpagedetails", editid).
            then(function (promise) {
                $scope.timing = moment(promise.periodlistedit[0].ttmdpT_StartTime, 'h:mm a').format();
                $scope.timing1 = moment(promise.periodlistedit[0].ttmdpT_EndTime, 'h:mm a').format();
                $scope.TTMDPT_Id = promise.periodlistedit[0].ttmdpT_Id;
                $scope.asmaY_Id = promise.periodlistedit[0].asmaY_Id;
                $scope.ttmC_Id = promise.periodlistedit[0].ttmC_Id;
                $scope.ttmD_Id = promise.periodlistedit[0].ttmD_Id;
                $scope.ttmP_Id = promise.periodlistedit[0].ttmP_Id;
                $scope.StartTime = $scope.timing;
                $scope.EndTime = $scope.timing1;
            })
        }

        $scope.deactive1 = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ttmdpT_ActiveFlag === true) {
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
                apiService.create("PeriodTimeSetting/deactive", employee).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal(confirmmgs + " " + "successfully.");
                    }
                    else {
                        swal(confirmmgs + " " + " successfully");
                    }
                    $scope.BindData();
                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

    }

})();