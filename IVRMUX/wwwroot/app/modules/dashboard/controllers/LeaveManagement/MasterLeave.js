(function () {
    'use strict';
    angular
.module('app')
.controller('MasterLeaveController', MasterLeaveController)

    MasterLeaveController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache']
    function MasterLeaveController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache) {

        var paginationformasters;
        $scope.multidepartment = false;
        $scope.editflag = false;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 10;
        }

        $scope.editEmployee = {};
        $scope.selected = {};
        $scope.obj = {};
        //Ui Grid view rendering data from data base
        $scope.gridOptions = {
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SlNo', field: 'name', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                    { name: 'hrmL_LeaveName', displayName: 'LeaveName' },
                    { name: 'hrmL_LeaveCode', displayName: 'LeaveCode' },
                    {
                        field: 'id', name: '',
                        displayName: 'Actions', enableFiltering: false, enableSorting: false,
                        cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);" data-toggle="tooltip" title="Edit" > <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.deletedataY(row.entity);" data-placement="bottom" data-toggle="tooltip" title="Delete"> <i class="fa fa-trash"></i></a> &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.HRML_LeaveCreditFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.HRML_LeaveCreditFlg === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                    }
            ]
        };

        $scope.resetLists = function () {
            $scope.configA = {
                onUpdate: function (evt) {
                    var itemEl = evt.item;
                }
            };
        };
        $scope.disablefield = false;
        $scope.getorgvalue = function (employee) {
            $scope.disablefield = true;
            $scope.editEmployee = employee.hrmL_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("MasterLeave/getdetails", pageid).then(function (promise) {
                if (promise.edit_m_event !== null && promise.edit_m_event.length > 0) {
                    $scope.HRML_Id = promise.edit_m_event[0].hrmL_Id;
                    $scope.obj.HRML_LeaveName = promise.edit_m_event[0].hrmL_LeaveName;
                    $scope.obj.HRML_LeaveCode = promise.edit_m_event[0].hrmL_LeaveCode;
                    $scope.obj.HRML_LeaveDetails = promise.edit_m_event[0].hrmL_LeaveDetails;
                    $scope.obj.ltmodel = promise.edit_m_event[0].hrmL_LeaveType;
                    $scope.desig = promise.edit_m_event[0].hrmL_LeaveCreditFlg;
                    $scope.lateflag = promise.edit_m_event[0].hrmL_LateDeductFlag;
                    $scope.probationflg = promise.edit_m_event[0].hrmL_ProbationaryNAFlg
                    $scope.obj.hrmL_WhenToApplyFlg = promise.edit_m_event[0].hrmL_WhenToApplyFlg;
                    $scope.obj.hrmL_NoOfDays = promise.edit_m_event[0].hrmL_NoOfDays;
                    $scope.obj.hrmL_MaxLeavesApplyPerMonth = promise.edit_m_event[0].hrmL_MaxLeavesApplyPerMonth;
                }
                });
        };

        $scope.deletedataY = function (employee, SweetAlert) {
            $scope.editEmployeeY = employee.hrmL_Id;
            var id = $scope.editEmployeeY;
            swal({
                title: "Are you sure",
                text: "Do you want to delete record????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.getURI("MasterLeave/deletepages", id).
                        then(function (promise) {
                            $scope.BindData();
                            $scope.gridOptions.data = promise.master_eventlist;
                            if (promise.returnval === true) {
                                swal('Record Deleted Successfully');
                            }
                            else {
                                swal('Mapping is Already defined');
                            }
                        });
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        };

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        //Load Page
        $scope.BindData = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.itemsPerPage = 10;
            $scope.search = "";

            apiService.getDATA("MasterLeave/GetLeave").
                then(function (promise) {
                    if (promise.count == 0) {
                        swal("No Records Found.....!!");
                        return;
                    }
                    else {

                        $scope.pagePrivilege = [];
                        if (promise.pagePrivilege !== null && promise.pagePrivilege.length > 0) {
                            $scope.pagePrivilege = promise.pagePrivilege;
                            $scope.IVRMRP_AddFlag = $scope.pagePrivilege[0].IVRMRP_AddFlag;
                            $scope.IVRMRP_UpdateFlag = $scope.pagePrivilege[0].IVRMRP_UpdateFlag;
                            $scope.IVRMRP_DeleteFlag = $scope.pagePrivilege[0].IVRMRP_DeleteFlag;
                            $scope.IVRMRP_ReportFlag = $scope.pagePrivilege[0].IVRMRP_ReportFlag;
                            $scope.IVRMRP_ProcessFlag = $scope.pagePrivilege[0].IVRMRP_ProcessFlag;

                        }


                        $scope.gridOptions.data = promise.master_eventlist;
                        $scope.vargrid = promise.master_eventlist;
                        $scope.edit_m_event = promise.leaveorderlist;
                    }
                });
        };

        $scope.submitted = false;
        $scope.savedata = function () {
            if ($scope.myForm.$valid)
            {
                var str = $scope.value;
                var data = {
                    "HRML_Id": $scope.HRML_Id,
                    "HRML_LeaveName": $scope.obj.HRML_LeaveName,
                    "HRML_LeaveCode": $scope.obj.HRML_LeaveCode,
                    "HRML_LeaveDetails": $scope.obj.HRML_LeaveDetails,
                    "HRML_LeaveType": $scope.obj.ltmodel,
                    "HRML_LeaveCreditFlg": $scope.obj.desig,
                    "HRML_LateDeductFlag": $scope.obj.lateflag,
                    "HRML_ProbationaryNAFlg": $scope.obj.probationflg,
                    "HRML_WhenToApplyFlg": $scope.obj.hrmL_WhenToApplyFlg,
                    "HRML_NoOfDays": $scope.obj.hrmL_NoOfDays,
                    "HRML_MaxLeavesApplyPerMonth": $scope.obj.hrmL_MaxLeavesApplyPerMonth
                };

                apiService.create("MasterLeave/savedetail1", data).then(function (promise) {
                    $scope.gridOptions.data = promise.master_eventlist;
                    $scope.edit_m_event = promise.leaveorderlist;
                    if (promise.returnval_add == true && promise.returnval_Update == false) {
                        swal('Record Saved Successfully');
                        $state.reload();
                    }
                    else if (promise.returnval_Update == true && promise.returnval_add == false) {
                        swal('Record Updated Successfully');
                        $state.reload();
                    }
                    else if (promise.returnval_add == false) {
                        swal('Failed to Save record');
                    }
                    else if (promise.returnval_Update == false) {
                        swal('Failed to  Update record');
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.deactive = function (obj) {

            var mgs = "";
            var confirmmgs = "";
            if (obj.hrmL_LeaveCreditFlg === false) {
   mgs = "Activate";
                confirmmgs = "Activated";
              
            }
            else {
               mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }

            var data = {
                "HRML_Id": obj.hrmL_Id,
                "MI_ID": obj.MI_ID,

            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("MasterLeave/deactivate", data).then(function (promise) {

                            if (promise.returnval === true) {
                                swal("Record " + confirmmgs + " " + "successfully");
                            }
                            else {
                                swal("Record " + mgs + " Failed");
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };  



        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $scope.HRML_LeaveName = "";
            $scope.HRML_LeaveCode = "";
            $scope.HRML_LeaveDetails = "";
            $scope.probationflg = "";
            $scope.ltmodel = "";
            $scope.desig = "";
            $scope.obj.hrmL_NoOfDays = "";
            $scope.hrmL_MaxLeavesApplyPerMonth = "";
            $scope.obj.hrmL_WhenToApplyFlg = "";
            $scope.lateflag = "";
        };

        $scope.getOrder = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.hrmL_Id !== 0) {
                    orderarray[key].hrmL_LateDeductOrder = key + 1;
                    orderarray[key].hrmL_LeaveOrder = key + 1;
                }
            });
            var data = {
                MasterLeaveDTOO: orderarray
            };
            apiService.create("MasterLeave/validateordernumber", data).
                then(function (promise) {
                    if (promise.retrunMsg != "" && promise.retrunMsg != undefined && promise.retrunMsg != null) {
                        swal(promise.retrunMsg);
                        $scope.BindData();
                    }
                });
        };

        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        };

        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        };
    }

})();