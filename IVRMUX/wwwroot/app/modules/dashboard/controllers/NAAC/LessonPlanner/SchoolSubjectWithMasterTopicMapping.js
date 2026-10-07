


(function () {
    'use strict';
    angular
        .module('app')
        .controller('SchoolSubjecWithtMasterTopicMappingController', SchoolSubjecWithtMasterTopicMappingController)

    SchoolSubjecWithtMasterTopicMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function SchoolSubjecWithtMasterTopicMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.showbtn = false;

        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            var id = 2;
            $scope.ISMS_IdNew = "";
            $scope.subjectlist = [];
            $scope.LPMMT_IdNew = "";
            $scope.topicdetailsnews = [];
            $scope.grouptypeListOrder = [];
            $scope.topicdetailsnew = [];

            apiService.getURI("SchoolSubjectWithMasterTopicMapping/Getdetails", id).
                then(function (promise) {

                    $scope.subjectlist = promise.getsubjectlist;
                    angular.forEach($scope.subjectlist, function (dd) {
                        dd.ismS_SubjectName = dd.ismS_SubjectName + " : " + dd.ismS_SubjectCode;
                    });
                    $scope.gridOptions.data = promise.getdetails;

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
                { name: 'ismS_SubjectName', displayName: 'Subject Name' },
                { name: 'lpmmT_TopicName', displayName: 'Topic Name' },
                { name: 'lpmT_TopicName', displayName: 'Sub Topic' },
                { name: 'lpmT_TotalHrs', displayName: 'Total Hours' },
                { name: 'lpmT_TotalPeriods', displayName: 'Total Periods' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip><i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.lpmT_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.lpmT_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }

        };


        $scope.onchangesubject = function () {
            $scope.topicdetailsnew = [];
            var data = {
                "ISMS_Id": $scope.ISMS_Id.ismS_Id
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/onchangesubject", data).then(function (promise) {
                if (promise !== null) {
                    $scope.showbtn = true;
                    $scope.topicdetailsnew = promise.topicdetails;
                    $scope.grouptypeListOrder = promise.getorderdetails;
                    if ($scope.topicdetailsnew.length > 0) {
                        $scope.topicdetails = $scope.topicdetailsnew;

                    } else {
                        swal("No Topics Mapped To This Subjects");
                    }
                } else {
                    swal("Something Went Wrong , Kindly Contact Administrator");
                }
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.topicdetails.some(function (options) {
                return options.Selected;
            });
        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {

                $scope.arraydetails = [];
                var LPMT_TeacherGuide = "";
                if ($scope.LPMT_TeacherGuide !== undefined && $scope.LPMT_TeacherGuide !== null) {
                    LPMT_TeacherGuide = $scope.LPMT_TeacherGuide;
                } else {
                    LPMT_TeacherGuide = "";
                }

                var LPMT_MaterialNeeded = "";
                if ($scope.LPMT_MaterialNeeded !== undefined && $scope.LPMT_MaterialNeeded !== null) {
                    LPMT_MaterialNeeded = $scope.LPMT_MaterialNeeded;
                } else {
                    LPMT_MaterialNeeded = "";
                }

                var LPMT_References = "";
                if ($scope.LPMT_References !== undefined && $scope.LPMT_References !== null) {
                    LPMT_References = $scope.LPMT_References;
                } else {
                    LPMT_References = "";
                }

                var LPMT_Homework = "";
                if ($scope.LPMT_Homework !== undefined && $scope.LPMT_Homework !== null) {
                    LPMT_Homework = $scope.LPMT_Homework;
                } else {
                    LPMT_Homework = "";
                }

                var LPMT_StudentGuide = "";
                if ($scope.LPMT_StudentGuide !== undefined && $scope.LPMT_StudentGuide !== null) {
                    LPMT_StudentGuide = $scope.LPMT_StudentGuide;
                } else {
                    LPMT_StudentGuide = "";
                }


                var data = {
                    "ISMS_Id": $scope.ISMS_Id.ismS_Id,
                    "LPMMT_Id": $scope.LPMMT_Id,
                    "LPMT_TopicName": $scope.LPMT_TopicName,
                    "LPMT_LessonPlan": $scope.LPMT_LessonPlan,
                    "LPMT_TotalHrs": $scope.LPMT_TotalHrs,
                    "LPMT_TotalPeriods": $scope.LPMT_TotalPeriods,
                    "LPMT_TeacherGuide": LPMT_TeacherGuide,
                    "LPMT_StudentGuide": LPMT_StudentGuide,
                    "LPMT_MaterialNeeded": LPMT_MaterialNeeded,
                    "LPMT_References": LPMT_References,
                    "LPMT_Homework": LPMT_Homework,
                    "LPMT_Id": $scope.LPMT_Id
                };

                apiService.create("SchoolSubjectWithMasterTopicMapping/savedetails", data).
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
            apiService.create("SchoolSubjectWithMasterTopicMapping/editdeatils", data).
                then(function (promise) {
                    $scope.topicdetailsnew = promise.topicdetails;
                    $scope.ISMS_Id = promise.geteditdetials[0];
                    $scope.LPMMT_Id = promise.geteditdetials[0].lpmmT_Id;
                    $scope.LPMT_TopicName = promise.geteditdetials[0].lpmT_TopicName;
                    $scope.LPMT_LessonPlan = promise.geteditdetials[0].lpmmT_TopicName;
                    $scope.LPMT_TotalHrs = promise.geteditdetials[0].lpmT_TotalHrs;
                    $scope.LPMT_TotalPeriods = promise.geteditdetials[0].lpmT_TotalPeriods;
                    $scope.LPMT_TeacherGuide = promise.geteditdetials[0].lpmT_TeacherGuide;
                    $scope.LPMT_StudentGuide = promise.geteditdetials[0].lpmT_StudentGuide;
                    $scope.LPMT_MaterialNeeded = promise.geteditdetials[0].lpmT_MaterialNeeded;
                    $scope.LPMT_References = promise.geteditdetials[0].lpmT_References;
                    $scope.LPMT_Homework = promise.geteditdetials[0].lpmT_Homework;
                    $scope.LPMT_Id = promise.geteditdetials[0].lpmT_Id;
                });
        };

        //to active or deactive 
        $scope.deactive = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.lpmT_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do You Want To " + mgs + " Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("SchoolSubjectWithMasterTopicMapping/deactivate", deactiveRecord).
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


        // Order 

        $scope.onchangesubjectnew = function () {
            $scope.topicdetailsnews = [];
            $scope.LPMMT_IdNew = "";
            $scope.grouptypeListOrder = [];
            var data = {
                "ISMS_Id": $scope.ISMS_IdNew.ismS_Id
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/onchangesubject", data).then(function (promise) {
                if (promise !== null) {
                    $scope.showbtn = true;
                    $scope.topicdetailsnew = promise.topicdetails;
                    if ($scope.topicdetailsnew !== null) {
                        if ($scope.topicdetailsnew.length > 0) {
                            $scope.topicdetailsnews = $scope.topicdetailsnew;

                        } else {
                            swal("No Topics Mapped To This Subjects");
                        }
                    } else {
                        swal("No Topics Mapped To This Subjects");
                    }
                } else {
                    swal("Something Went Wrong , Kindly Contact Administrator");
                }
            });

        };

        $scope.onselecttopic = function () {
            $scope.grouptypeListOrder = [];
            var data = {
                "ISMS_Id": $scope.ISMS_IdNew.ismS_Id,
                "LPMMT_Id": $scope.LPMMT_IdNew
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/onselecttopic", data).then(function (promise) {

                $scope.grouptypeListOrder = promise.gettopicdetailsorder;
                if ($scope.grouptypeListOrder !== null) {
                    if ($scope.grouptypeListOrder.length > 0) {
                        $scope.btn = true;
                    } else {
                        $scope.btn = false;
                        swal("No Records Found");
                    }
                } else {
                    $scope.btn = false;
                    swal("No Records Found");
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
                SchoolSubjectWithMasterTopicMappingTemporderDTO: orderarray
            };

            apiService.create("SchoolSubjectWithMasterTopicMapping/validateordernumber", data).
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

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.grouptypeListOrder) {
                    $scope.grouptypeListOrder[index].lpmT_TopicOrder = Number(index) + 1;

                }
            }
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