


(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterSchoolTopicController', MasterSchoolTopicController)

    MasterSchoolTopicController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function MasterSchoolTopicController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            $scope.ISMS_IdNew = "";
            $scope.getsubjecttopicdetails = [];
            var id = 2;
            apiService.getURI("MasterSchoolTopic/Getdetails", id).
                then(function (promise) {
                    $scope.gridOptions.data = promise.getdetails;
                    $scope.grouptypeListOrder = promise.getdetails;

                    $scope.subjectlist = promise.getsubject;

                    angular.forEach($scope.subjectlist, function (dd) {
                        dd.ismS_SubjectName = dd.ismS_SubjectName + " : " + dd.ismS_SubjectCode;
                    });

                    //$scope.subjectlistnew = promise.getsubject;

                    //angular.forEach($scope.subjectlistnew, function (ddd) {
                    //    ddd.ismS_SubjectName = ddd.ismS_SubjectName + " : " + ddd.ismS_SubjectCode;
                    //});
                });
        };

        //Table 
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'ismS_SubjectName', displayName: 'Subject Name' ,width:200},
                { name: 'lpmmT_TopicName', displayName: 'Topic Name' },
                { name: 'lpmmT_TopicDescription', displayName: 'Topic Description' },
                { name: 'lpmmT_TotalPeriods', displayName: 'No of Periods' },
                { name: 'lpmmT_TotalHrs', displayName: 'No of Hours' },
                { name: 'lpmmT_Order', displayName: 'Order', type: 'number' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip><i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +

                        '<a ng-if="row.entity.lpmmT_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.lpmmT_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }

        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "LPMMT_Id": $scope.LPMMT_Id,
                    "LPMMT_TopicName": $scope.LPMMT_TopicName,
                    "LPMMT_TopicDescription": $scope.LPMMT_TopicDescription,
                    "LPMMT_TotalPeriods": $scope.LPMMT_TotalPeriods,
                    "LPMMT_TotalHrs": $scope.LPMMT_TotalHrs,
                    "ISMS_Id": $scope.ISMS_Id.ismS_Id
                };

                apiService.create("MasterSchoolTopic/savedetails", data).
                    then(function (promise) {
                        // $scope.newuser = promise.mastersubexam;

                        if (promise.message === "Add") {
                            if (promise.returnval === true) {
                                swal('Record saved successfully');
                            } else {
                                swal('Failed To save Record');
                            }
                        } else if (promise.message === "Update") {
                            if (promise.returnval === true) {
                                swal('Record updated successfully');
                            } else {
                                swal('Failed To Update Record');
                            }
                        }

                        else if (promise.message === 'Duplicate') {
                            swal('Record already exist');
                        }
                        else {
                            swal("Failed To Save / Update Record");
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
            apiService.create("MasterSchoolTopic/editdeatils", data).
                then(function (promise) {
                    $scope.LPMMT_Id = promise.editdetails[0].lpmmT_Id;
                    $scope.LPMMT_TopicName = promise.editdetails[0].lpmmT_TopicName;
                    $scope.LPMMT_TopicDescription = promise.editdetails[0].lpmmT_TopicDescription;
                    $scope.LPMMT_TotalPeriods = promise.editdetails[0].lpmmT_TotalPeriods;
                    $scope.LPMMT_TotalHrs = promise.editdetails[0].lpmmT_TotalHrs;
                    $scope.ISMS_Id = promise.editdetails[0];
                });
        };

        //to active or deactive 
        $scope.deactive = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.lpmmT_ActiveFlag === true) {
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

                        apiService.create("MasterSchoolTopic/deactivate", deactiveRecord).
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


        $scope.gettopicdetails = function () {
            $scope.getsubjecttopicdetails = [];
            $scope.btn = false;
            var data = {
                "ISMS_Id": $scope.ISMS_IdNew.ismS_Id
            };

            apiService.create("MasterSchoolTopic/gettopicdetails", data).then(function (promise) {

                if (promise.getsubjecttopicdetails !== null) {
                    if (promise.getsubjecttopicdetails.length > 0) {
                        $scope.getsubjecttopicdetails = promise.getsubjecttopicdetails;
                        $scope.btn = true;
                    } else {
                        swal("No Data Found");
                    }
                   
                } else {
                    swal("No Data Found");
                }
            });
        };




        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.getsubjecttopicdetails) {
                    $scope.getsubjecttopicdetails[index].lpmmT_Order = Number(index) + 1;

                }
            }
        };

        $scope.getOrder = function (orderarray) {
            var data = {
                MasterSchoolTopicOrderDTO: orderarray
            };

            apiService.create("MasterSchoolTopic/validateordernumber", data).
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

    }

})();