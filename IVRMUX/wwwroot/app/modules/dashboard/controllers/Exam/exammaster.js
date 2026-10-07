(function () {
    'use strict';
    angular
        .module('app')
        .controller('ExamMasterController', exammasterController)

    exammasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function exammasterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        //TO  GEt The Values iN Grid
        // var temp_exam_list = [];
        $scope.EME_FinalExamFlag = false;
        $scope.EME_ActiveFlag = true;
        $scope.BindData = function () {
            apiService.getDATA("exammaster/Getdetails").
                then(function (promise) {


                    $scope.gridOptions.data = promise.exammastername;
                    $scope.grouptypeListOrder = promise.exammastername;
                    //temp_exam_list = promise.exammastername;
                    $scope.final_exm_count = 0;
                    angular.forEach(promise.exammastername, function (emd) {
                        if (emd.emE_FinalExamFlag == true) {
                            $scope.final_exm_count += 1;
                        }
                    })

                })
        };

        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'emE_ExamName', displayName: 'Exam Name' },
                { name: 'emE_IVRSExamName', displayName: 'IVRS Exam Name' },
                { name: 'emE_ExamCode', displayName: 'Exam Code' },
                { name: 'emE_ExamOrder', displayName: 'Exam Order', type: 'number' },
                {
                    name: 'emE_FinalExamFlag', displayName: 'Final-Exam Flag', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.emE_FinalExamFlag == true" href="javascript:void(0)" > <i class="fa fa-check  text-green"></i></a>' +
                        '<a ng-if="row.entity.emE_FinalExamFlag == false" href="javascript:void(0)" > <i class="fa fa-times  text-red"></i></i></a>' +
                        '</div>'
                },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.Editexammasterdata(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.emE_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.emE_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }

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
            angular.forEach(orderarray, function (value, key) {
                if (value.EME_ID !== 0) {
                    orderarray[key].emE_ExamOrder = key + 1;
                }
            });
            var data = {
                examDTO: orderarray,
            }
            apiService.create("exammaster/validateordernumber", data).
                then(function (promise) {
                    if (promise.retrunMsg != "" && promise.retrunMsg != undefined && promise.retrunMsg != null) {
                        swal(promise.retrunMsg);


                    }
                    $scope.cancel();
                    $scope.BindData();

                });
            // $state.BindData();
            // $scope.BindData();
        }

        //to deactive the data

        $scope.deactive = function (deactiveRecord) {
            if (deactiveRecord.emE_FinalExamFlag == true && deactiveRecord.emE_ActiveFlag == true) {
                swal("You Can Not Deactivate Final Exam Record");
                //$scope.cancel();
                //$scope.BindData();
            }
            else {
                var mgs = "";
                var confirmmgs = "";
                if (deactiveRecord.emE_ActiveFlag == true) {
                    //mgs = "Deactive";
                    mgs = "Deactivate";
                    confirmmgs = "De-activated";

                }
                else {
                    // mgs = "Active";
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

                            var config = {
                                headers: {
                                    'Content-Type': 'application/json;'
                                }
                            }

                            apiService.create("exammaster/deactivate", deactiveRecord).
                                then(function (promise) {
                                    if (promise.already_cnt == true) {
                                        swal("You Can Not Deactivate This Record,It Has Dependency");
                                    }
                                    else {
                                        if (promise.returnval == true) {
                                            swal("Record " + confirmmgs + " " + "successfully");
                                        }
                                        else {
                                            // swal(confirmmgs + " " + " successfully");
                                            swal("Record " + mgs + " Failed");
                                        }
                                    }
                                    //if (promise.returnval === true) {
                                    //    swal(confirmmgs + ' Successfully');
                                    //}
                                    //else {
                                    //    swal('Record Not  Activated/Deactivated');
                                    //}
                                    $scope.cancel();
                                    $scope.BindData();
                                    // $scope.clearid1();
                                })
                        }
                        else {
                            swal("Record " + mgs + " Cancelled");
                        }
                    });
            }
        };


        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }

        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        }

        // to Edit Data
        $scope.Editexammasterdata = function (EditRecord) {

            var MEditId = EditRecord.emE_Id;
            apiService.getURI("exammaster/editdetails/", MEditId).
                then(function (promise) {
                    $scope.EME_ID = promise.editlist[0].emE_Id;
                    $scope.excode = promise.editlist[0].emE_ExamCode;
                    $scope.exname = promise.editlist[0].emE_ExamName;
                    $scope.ivrsexname = promise.editlist[0].emE_IVRSExamName;
                    $scope.EME_FinalExamFlag = promise.editlist[0].emE_FinalExamFlag;
                    $scope.EME_ActiveFlag = promise.editlist[0].emE_ActiveFlag;
                    $scope.exorder = promise.editlist[0].emE_ExamOrder;
                    if (promise.editlist[0].emE_FinalExamFlag == true) {
                        $scope.final_exm_count = 0;
                    }
                })
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "EME_Id": $scope.EME_ID,
                    "EME_ExamCode": $scope.excode,
                    "EME_ExamName": $scope.exname,
                    "EME_IVRSExamName": $scope.ivrsexname,
                    "EME_FinalExamFlag": $scope.EME_FinalExamFlag,
                    //   "EME_ExamOrder": $scope.exorder,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("exammaster/savedetails", data).
                    then(function (promise) {
                        $scope.newuser = promise.exammastername;
                        //if (promise.returnval === true) {
                        //    //$scope.cancel();
                        //    //$scope.BindData();
                        //    swal('Record Saved/Updated Successfully', 'success');
                        //}
                        //else if (promise.returnduplicatestatus === true || promise.returnval === false) {
                        //    swal('Record is Duplicate', 'Failed')
                        //}
                        //else {
                        //    //$scope.cancel();
                        //    //$scope.BindData();
                        //    swal('Record Not Saved/Updated Successfully', 'Failed');
                        //}
                        if (promise.returnval === true) {
                            // swal('Data successfully Saved');
                            if (promise.emE_Id == 0 || promise.emE_Id < 0) {
                                swal('Record saved successfully');
                            }
                            // else if(promise.emcA_Id!="" && promise.emcA_Id>0 && promise.emcA_Id!=undefined)
                            else if (promise.emE_Id > 0) {
                                swal('Record updated successfully');
                            }

                        }
                        else if (promise.returnduplicatestatus === 'Duplicate') {
                            //  swal('Recards AlReady Exist !');
                            swal('Record already exist');
                        }
                        else {
                            //swal('Data Not Saved !');
                            if (promise.emE_Id == 0 || promise.emE_Id < 0) {
                                swal('Failed to save, please contact administrator');
                            }
                            else if (promise.emE_Id > 0) {
                                swal('Failed to update, please contact administrator');
                            }
                        }
                        $scope.cancel();
                        $scope.BindData();
                    })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.cancel = function () {
            $scope.EME_ID = 0;
            $scope.excode = "";
            $scope.exname = "";
            $scope.ivrsexname = "";
            $scope.exorder = "";
            $scope.EME_FinalExamFlag = false;
            $scope.EME_ActiveFlag = true;
            angular.forEach($scope.gridOptions.data, function (emd) {
                if (emd.emE_FinalExamFlag == true) {
                    $scope.final_exm_count += 1;
                }
            })
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.gridApi.grid.clearAllFilters();
            $scope.search = "";
            // $state.reload();
        }
        $scope.get_older = function () {

            $scope.BindData();
            //$scope.grouptypeListOrder = temp_exam_list;
        }

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.grouptypeListOrder) {
                    $scope.grouptypeListOrder[index].emE_ExamOrder = Number(index) + 1;

                }
            }
        };


    }

})();