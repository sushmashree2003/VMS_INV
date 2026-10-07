(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeStaffperiodtransactionController', CollegeStaffperiodtransactionController)

    CollegeStaffperiodtransactionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function CollegeStaffperiodtransactionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.showbtn = false;
        $scope.details = false;
        //TO  GEt The Values iN Grid
        $scope.currentPage = 1;
        // $scope.itemsPerPage = 1;

        $scope.BindData = function () {
            var id = 2;
            apiService.getURI("CollegeStaffPeriodMapping/Getdetailstransaction", id).
                then(function (promise) {
                    $scope.masteryear = promise.masteryear;
                    $scope.employeedetails = promise.employeedetails;
                    $scope.HRME_Id = $scope.employeedetails[0].hrmE_Id;
                });
        };

        $scope.obj = {};

        $scope.getemployeedetails = function () {
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.ISMS_Id = "";
            var data = {
                "HRME_Id": $scope.HRME_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("CollegeStaffPeriodMapping/getemployeedetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.mastercourse = promise.mastercourse;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };

        $scope.onchangecourse = function () {
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.ISMS_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "HRME_Id": $scope.HRME_Id,
                "AMCO_Id": $scope.AMCO_Id
            };

            apiService.create("CollegeStaffPeriodMapping/onchangecourse", data).then(function (promise) {
                if (promise !== null) {
                    $scope.masterbranch = promise.masterbranch;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };

        $scope.onchangebranch = function () {
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.ISMS_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "HRME_Id": $scope.HRME_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };

            apiService.create("CollegeStaffPeriodMapping/onchangebranch", data).then(function (promise) {
                if (promise !== null) {
                    $scope.mastersemester = promise.mastersemester;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };

        $scope.onchangesemster = function () {

            $scope.ACMS_Id = "";
            $scope.ISMS_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "HRME_Id": $scope.HRME_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id
            };

            apiService.create("CollegeStaffPeriodMapping/onchangesemster", data).then(function (promise) {
                if (promise !== null) {
                    $scope.mastersection = promise.mastersection;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };

        $scope.onchangesection = function () {
            $scope.ISMS_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "HRME_Id": $scope.HRME_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACMS_Id": $scope.ACMS_Id
            };

            apiService.create("CollegeStaffPeriodMapping/onchangesection", data).then(function (promise) {
                if (promise !== null) {
                    $scope.mastersubjects = promise.mastersubjects;
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };

        $scope.getsearchdetailstransaction = function () {

            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "HRME_Id": $scope.HRME_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "ISMS_Id": $scope.ISMS_Id
                };
                apiService.create("CollegeStaffPeriodMapping/getsearchdetailstransaction", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.getalldates = promise.getalldates;

                        angular.forEach($scope.getalldates, function (dd) {

                            dd.minDatedof = new Date(
                               new Date(dd.alldates).getFullYear(),
                                new Date(dd.alldates).getMonth(),
                                new Date(dd.alldates).getDate());
                        });


                        console.log($scope.getalldates);

                        $scope.details = true;
                        $scope.gettopicdetails = promise.gettopicdetails;
                    } else {
                        swal("Something Went Wrong Kindly Contact Administrator");
                    }
                });

            } else {
                $scope.submitted = true;
            }
        };



        $scope.isOptionsRequired = function () {
            return !$scope.topicdetails.some(function (options) {
                return options.Selected;
            });
        };

        // TO Save The Data
        $scope.submitted = false;

        $scope.savedata = function () {

            $scope.submitted = true;

            if ($scope.myForm.$valid) {

                $scope.arraydetails = [];

                angular.forEach($scope.getalldates, function (hi) {
                    if (hi.Selected) {
                        $scope.arraydetails.push({ LPCSWA_AllocatedDate: new Date(hi.alldates).toDateString(), LPMT_Id: hi.lpmT_Id, LPCSWA_Id: hi.lpcswa_id, LPCSWA_TakenDate: new Date(hi.lpcswA_TakenDate).toDateString() });
                    }
                });


                console.log($scope.arraydetails);

                var data = {
                    "ISMS_Id": $scope.ISMS_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "HRME_Id": $scope.HRME_Id,
                    "CollegeStaffPeriodMappingsavingTempDTO": $scope.arraydetails
                };

                apiService.create("CollegeStaffPeriodMapping/savedatatransaction", data).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            swal('Record saved / Updated successfully');
                        } else {
                            swal('Failed To save / Update Record');
                        }
                        $scope.cancel();
                        //$scope.BindData();
                    });
            } else {
                $scope.submitted = true;
            }

        };

        // to Edit Data
        $scope.getorgvalue = function (EditRecord) {
            var data = EditRecord;
            apiService.create("CollegeSubjetMasterTopicMapping/editdeatils", data).
                then(function (promise) {
                    $scope.LPMT_Id = promise.editdetails[0].lpmT_Id;
                    $scope.LPMT_TopicName = promise.editdetails[0].lpmT_TopicName;
                    $scope.LPMT_TopicDescription = promise.editdetails[0].lpmT_TopicDescription;
                    $scope.LPMT_TopicPeriods = promise.editdetails[0].lpmT_TopicPeriods;
                });
        };

        //to active or deactive 
        $scope.deactive = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.lpsmtM_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
                mgs = "Activate";
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

                        apiService.create("CollegeSubjetMasterTopicMapping/deactivate", deactiveRecord).
                            then(function (promise) {
                                if (promise.already_cnt === true) {
                                    swal("You Can Not Deactivate This Record,It Has Dependency");
                                }
                                else {
                                    if (promise.returnval === true) {
                                        swal("Record " + confirmmgs + " " + "successfully");
                                    }
                                    else {
                                        swal("Record " + mgs + " Failed");
                                    }
                                }
                                $scope.cancel();
                                // $scope.BindData();

                            });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        //fix the order drag
        //ConfigA is an Items
        $scope.resetLists = function () {
            $scope.configA = {
                // Changed sorting within list
                onUpdate: function (evt) {
                    var itemEl = evt.item;  // dragged HTMLElement
                    // + indexes from onEnd
                }
            };
        };
        $scope.init = function () {
            $scope.resetLists();
        };
        $scope.init();

        $scope.getOrder = function (orderarray) {
            var data = {
                CollegeSubjectWithMasterTopicMappingTemporderDTO: orderarray
            };

            apiService.create("CollegeSubjetMasterTopicMapping/validateordernumber", data).
                then(function (promise) {
                    if (promise.returnval === true) {
                        swal("order Updated Successfully");
                    } else {
                        swal("Failed To Update order");
                    }
                    $scope.cancel();
                    // $scope.BindData();
                });
        };


        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return JSON.stringify(obj.alldates).indexOf($scope.searchValue) >= 0;
        };

        //Table 
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'ismS_SubjectName', displayName: 'Subject Name' },
                { name: 'lpmT_TopicName', displayName: 'Topic Name' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.lpsmtM_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.lpsmtM_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }

        };
    }

})();