(function () {
    'use strict';
    angular
        .module('app')
        .controller('masterEmployeeDetailsController', masterEmployeeDetailsController)

    masterEmployeeDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function masterEmployeeDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {


        //#region Master Employee Type Form Data starts


        // form Object
        $scope.Employee = {};


        // Datatable display
        $scope.gridOptionsEmployeeType = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableHiding: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmeT_EmployeeType', displayName: 'Employee Type', enableHiding: false },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditDataEmployeeType(row.entity);" data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.hrmeT_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecordEmployeeType(row.entity);" data-toggle="tooltip" title="Activate"> Activate</a>' +
                        '<span ng-if="row.entity.hrmeT_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecordEmployeeType(row.entity);" data-toggle="tooltip" title="Deactivate">  Deactivate</a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApiEmployeeType = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptionsEmployeeType.data);
            }

        };

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.departmentListOrder) {
                    $scope.departmentListOrder[index].hrmD_DepartmentName;

                }
            }
        };


        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.designationListOrder) {
                    $scope.designationListOrder[index].hrmdeS_DesignationName;

                }
            }
        };
        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.gradeListOrder) {
                    $scope.gradeListOrder[index].hrmG_GradeName;

                }
            }
        };

        // Get form Details at Initial  
        $scope.onLoadGetDataEmployeeTypeInitial = function () {

            var pageid = 2;
            apiService.getURI("MasterEmployeeType/getalldetails", pageid).then(function (promise) {


                if (promise.employeeTypeList !== null && promise.employeeTypeList.length > 0) {

                    $scope.gridOptionsEmployeeType.data = promise.employeeTypeList;
                }
            })
        }


        // Get form Details at onload  
        $scope.onLoadGetDataEmployeeType = function () {

            $scope.cancelEmployeeType();

            var pageid = 2;
            apiService.getURI("MasterEmployeeType/getalldetails", pageid).then(function (promise) {


                if (promise.employeeTypeList !== null && promise.employeeTypeList.length > 0) {

                    $scope.gridOptionsEmployeeType.data = promise.employeeTypeList;
                }
            })
        }


        // clear form data
        $scope.cancelEmployeeType = function () {
            $scope.Employee = {};
            $scope.submittedEmployeeType = false;
            $scope.myFormEmployeeType.$setPristine();
            $scope.myFormEmployeeType.$setUntouched();
            $scope.gridApiEmployeeType.grid.clearAllFilters();
        }

        //saving/updating Record
        $scope.submittedEmployeeType = false;
        $scope.saveDataEmployeeType = function () {
            $scope.submittedEmployeeType = true;
            if ($scope.myFormEmployeeType.$valid) {
                var data = $scope.Employee;

                apiService.create("MasterEmployeeType/", data).
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

                            if (promise.employeeTypeList !== null && promise.employeeTypeList.length > 0) {

                                $scope.gridOptionsEmployeeType.data = promise.employeeTypeList;
                            }
                            $scope.cancelEmployeeType();
                        }
                    })
            }

        };

        $scope.interactedEmployeeType = function (field) {
            return $scope.submittedEmployeeType || field.$dirty;
        };


        // Edit Single Record
        $scope.EditDataEmployeeType = function (record) {

            var id = record.hrmeT_Id;
            apiService.getURI("MasterEmployeeType/editRecord", id).
                then(function (promise) {

                    if (promise.employeeTypeList != null && promise.employeeTypeList.length > 0) {
                        $scope.Employee = promise.employeeTypeList[0];
                    }

                    //angular.forEach(promise.employeeTypeList, function (value, key) {
                    //    if (value.hrmeT_Id !== 0) {
                    //        $scope.Employee.HRMET_EmployeeType = value.hrmeT_EmployeeType;
                    //        $scope.Employee.HRMET_Id = value.hrmeT_Id;
                    //    }
                    //});
                })
        }


        //deactivate record
        $scope.DeletRecordEmployeeType = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hrmeT_ActiveFlag == false) {
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
                        apiService.getURI("MasterEmployeeType/ActiveDeactiveRecord", data.hrmeT_Id).
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

                                    if (promise.employeeTypeList !== null && promise.employeeTypeList.length > 0) {

                                        $scope.gridOptionsEmployeeType.data = promise.employeeTypeList;
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

        //#endregion Master Employee Type Form Data Ends


        //#region Master Group Type Form Data Starts

        // form Object
        $scope.Group = {};


        // Datatable display
        $scope.gridOptionsGroupType = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableHiding: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmgT_EmployeeGroupType', displayName: 'Group Type', enableHiding: false },
                { name: 'hrmgT_Code', displayName: 'Group Code', enableHiding: false },
                { name: 'hrmgT_Order', displayName: 'Group Order', enableHiding: false },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditDataGroupType(row.entity);" data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.hrmgT_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecordGroupType(row.entity);" data-toggle="tooltip" title="Activate"> Activate</a>' +
                        '<span ng-if="row.entity.hrmgT_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecordGroupType(row.entity);" data-toggle="tooltip" title="Deactivate"">  Deactivate</a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApiGroupType = gridApi;
            }

        };

        // Get form Details at onload 
        $scope.onLoadGetDataGroupType = function () {

            $scope.cancelGroupType();

            var pageid = 2;
            apiService.getURI("MasterGroupType/getalldetails", pageid).then(function (promise) {


                if (promise.grouptypeList !== null && promise.grouptypeList.length > 0) {

                    $scope.gridOptionsGroupType.data = promise.grouptypeList;

                    $scope.grouptypeListOrder = promise.grouptypeList;
                }
            })
        }



        // clear form data
        $scope.cancelGroupType = function () {
            // $scope.search = "";
            $scope.Group = {};
            $scope.submittedGroupType = false;
            $scope.myFormGroupType.$setPristine();
            $scope.myFormGroupType.$setUntouched();
            $scope.gridApiGroupType.grid.clearAllFilters();
        }

        $scope.Group.hrmgT_Order = 0;
        //saving/updating Record
        $scope.submittedGroupType = false;
        $scope.saveDataGroupType = function () {
            $scope.submittedGroupType = true;
            if ($scope.myFormGroupType.$valid) {
                var data = $scope.Group;

                apiService.create("MasterGroupType/", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {

                            if (promise.retrunMsg == "AllDuplicate") {
                                swal("Record already exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg == "Duplicate") {
                                swal("Group Type is Already Exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg == "Code") {
                                swal("Group Code is Already Exist..!!");
                                return;
                            }

                            else if (promise.retrunMsg == "false") {
                                swal("Record Not saved / Updated..", 'Fail');

                            }
                            else if (promise.retrunMsg == "Add") {
                                swal("Record Saved Successfully.");
                            }
                            else if (promise.retrunMsg == "Update") {
                                swal("Record Updated Successfully.");
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                            }

                            $scope.onLoadGetDataGroupType();
                        }
                    })
            }

        };
        $scope.interactedGroupType = function (field) {
            return $scope.submittedGroupType || field.$dirty;
        };

        // Edit Single Record
        $scope.EditDataGroupType = function (record) {

            var id = record.hrmgT_Id;
            apiService.getURI("MasterGroupType/editRecord", id).
                then(function (promise) {

                    if (promise.grouptypeList != null && promise.grouptypeList.length > 0) {
                        $scope.Group = promise.grouptypeList[0];
                    }
                })
        }


        //deactivate record
        $scope.DeletRecordGroupType = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hrmgT_ActiveFlag == false) {
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
                        apiService.getURI("MasterGroupType/ActiveDeactiveRecord", data.hrmgT_Id).
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

                                    if (promise.grouptypeList !== null && promise.grouptypeList.length > 0) {

                                        $scope.gridOptionsGroupType.data = promise.grouptypeList;
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


        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.grouptypeListOrder) {
                    $scope.grouptypeListOrder[index].hrmgT_Order = Number(index) + 1;

                }
            }
        };

        //set order
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

        $scope.getOrderGroupType = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.hrmgT_Id !== 0) {
                    orderarray[key].hrmgT_Order = key + 1;
                }
            });

            var data = {
                GroupTypeDTO: orderarray,
            }
            apiService.create("MasterGroupType/validateordernumber", data).
                then(function (promise) {
                    if (promise.retrunMsg != "" && promise.retrunMsg != undefined && promise.retrunMsg != null) {
                        swal(promise.retrunMsg);
                        $scope.onLoadGetDataGroupType();
                    }


                });
        }

        //#endregion Master Group Type Form Data Ends



        //#region Master Department Form Data Starts

        // form Object
        $scope.Department = {};


        // Datatable display
        $scope.gridOptionsDepartment = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableHiding: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmD_DepartmentName', displayName: 'Department Name', enableHiding: false },
                { name: 'hrmD_Order', displayName: 'Order', enableHiding: false },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditDataDepartment(row.entity);" data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.hrmD_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecordDepartment(row.entity);" data-toggle="tooltip" title="Activate"> Activate</a>' +
                        '<span ng-if="row.entity.hrmD_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecordDepartment(row.entity);" data-toggle="tooltip" title="Deactivate">  Deactivate</a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApiDepartment = gridApi;
            }

        };

        // Get form Details at onload 

        $scope.onLoadGetDataDepartment = function () {
            $scope.cancelDepartment();
            var pageid = 2;
            apiService.getURI("MasterDepartment/getalldetails", pageid).then(function (promise) {
                if (promise.departmentList !== null && promise.departmentList.length > 0) {
                    $scope.gridOptionsDepartment.data = promise.departmentList;
                    $scope.departmentListOrder = promise.departmentList;
          
                }
                $scope.departmencodetList = promise.departmencodetList;
            })
        }


        // clear form data
        $scope.cancelDepartment = function () {
            // $scope.search = "";
            $scope.Department = {};
            $scope.submittedDepartment = false;
            $scope.myFormDepartment.$setPristine();
            $scope.myFormDepartment.$setUntouched();
            $scope.gridApiDepartment.grid.clearAllFilters();
        }

        //saving/updating Record
        $scope.submittedDepartment = false;
        $scope.saveDataDepartment = function () {
            $scope.submittedDepartment = true;
            if ($scope.myFormDepartment.$valid) {
                //var data = $scope.Department;
                var data = {
                    "HRMD_Id": $scope.hrmdid,
                    "HRMD_DepartmentName": $scope.Department.hrmD_DepartmentName,
                    "HRMDC_ID": $scope.Department.hrmdC_ID
                };
                apiService.create("MasterDepartment/", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {

                            if (promise.retrunMsg == "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg == "Order") {
                                swal("Department Name is Already Exist..");
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

                            if (promise.departmentList !== null && promise.departmentList.length > 0) {

                                $scope.gridOptionsDepartment.data = promise.departmentList;
                            }
                            $scope.cancelDepartment();
                            $scope.onLoadGetDataDepartment();
                        }
                    })
            }

        };

        $scope.interactedDepartment = function (field) {
            return $scope.submittedDepartment || field.$dirty;
        };


        // Edit Single Record
        $scope.EditDataDepartment = function (record) {

            var id = record.hrmD_Id;
            apiService.getURI("MasterDepartment/editRecord", id).
                then(function (promise) {

                    if (promise.departmentList != null && promise.departmentList.length > 0) {
                        $scope.Department = promise.departmentList[0];
                        $scope.hrmdid = promise.departmentList[0].hrmD_Id;
                    }

                })
        }


        //deactivate record
        $scope.DeletRecordDepartment = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hrmD_ActiveFlag == false) {
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
                        apiService.getURI("MasterDepartment/ActiveDeactiveRecord", data.hrmD_Id).
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

                                    if (promise.departmentList !== null && promise.departmentList.length > 0) {

                                        $scope.gridOptionsDepartment.data = promise.departmentList;
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

        //Department Set Order

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



        $scope.getdepartmentOrder = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.hrmD_Id !== 0) {
                    orderarray[key].hrmD_Order = key + 1;
                }
            });
            var data = {
                DeraptmentDTO: orderarray,
            }
            apiService.create("MasterDepartment/validateordernumber", data).
                then(function (promise) {
                    if (promise.retrunMsg != "" && promise.retrunMsg != undefined && promise.retrunMsg != null) {
                        swal(promise.retrunMsg);
                        $scope.onLoadGetDataDepartment();
                    }
                });
        }


        //#endregion Master Department Form Data Ends


        //#region Master Designation Form Data Starts

        // form Object
        $scope.Designation = {};


        // Datatable display
        $scope.gridOptionsDesignation = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableHiding: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmdeS_DesignationName', displayName: 'Designation Name', enableHiding: false },
                { name: 'hrmdeS_BasicAmount', displayName: 'Basic Amount', enableHiding: false },
                { name: 'hrmdeS_SanctionedSeats', displayName: 'Sanctioned Seats', enableHiding: false },
                {
                    name: 'hrmdeS_DisplaySanctionedSeatsFlag', displayName: 'Display Sanctioned Seats ?', enableHiding: false, cellTemplate:
                        '<div>' +
                        '<span ng-if="row.entity.hrmdeS_DisplaySanctionedSeatsFlag === false"> No</span>' +
                        '<span ng-if="row.entity.hrmdeS_DisplaySanctionedSeatsFlag === true">Yes<span>' +
                        '</div>'
                },
                { name: 'hrmdeS_Order', displayName: 'Order', enableHiding: false },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditDataDesignation(row.entity);" data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.hrmdeS_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecordDesignation(row.entity);" data-toggle="tooltip" title="Activate"> Activate</a>' +
                        '<span ng-if="row.entity.hrmdeS_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecordDesignation(row.entity);"  data-toggle="tooltip" title="Deactivate">  Deactivate</a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApiDesignation = gridApi;
            }

        };

        // Get form Details at onload 

        $scope.onLoadGetDataDesignation = function () {
            $scope.cancelDesignation();
            var pageid = 2;
            apiService.getURI("MasterDesignation/getalldetails", pageid).then(function (promise) {


                if (promise.designationlList !== null && promise.designationlList.length > 0) {

                    $scope.gridOptionsDesignation.data = promise.designationlList;

                    $scope.designationListOrder = promise.designationlList;
                    
                }
                $scope.departmencodetList = promise.departmencodetList;
            })
        }


        // clear form data
        $scope.cancelDesignation = function () {
            // $scope.search = "";
            $scope.Designation = {};
            $scope.submittedDesignation = false;
            $scope.myFormDesignation.$setPristine();
            $scope.myFormDesignation.$setUntouched();
            $scope.gridApiDesignation.grid.clearAllFilters();
        };

        //saving/updating Record
        $scope.submittedDesignation = false;
        $scope.saveDataDesignation = function () {
            $scope.submittedDesignation = true;
            if ($scope.myFormDesignation.$valid) {
                //var data = $scope.Designation;
                var data = {
                    "HRMDES_DesignationName": $scope.Designation.hrmdeS_DesignationName,
                    "HRMDC_ID": $scope.Designation.hrmdC_ID,
                    "HRMDES_BasicAmount": $scope.Designation.hrmdeS_BasicAmount,
                    "HRMDES_SanctionedSeats": $scope.Designation.hrmdeS_SanctionedSeats,
                    "HRMDES_DisplaySanctionedSeatsFlag": $scope.Designation.hrmdeS_DisplaySanctionedSeatsFlag
                };
                apiService.create("MasterDesignation/", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {

                            if (promise.retrunMsg == "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg == "Order") {
                                swal("Designation Name is Already Exist..");
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

                            if (promise.designationlList !== null && promise.designationlList.length > 0) {

                                $scope.gridOptionsDesignation.data = promise.designationlList;
                            }
                            $scope.onLoadGetDataDesignation();
                        }
                    })
            }

        };

        $scope.TempData = [];
        $scope.TempDataHRAuthorisation = [];
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.submitted = false;
        $scope.onloadgroupdeptdesignationmapping = function () {
            var pageid = 2;
            apiService.getURI("GroupDeptDessg/loaddata", pageid).then(function (promise) {
                $scope.group_list = promise.groupTypedropdown;
                $scope.department_list = promise.departmentdropdown;
                $scope.designation_list = promise.designationdropdown;
                $scope.gridviewdata = promise.gridviewdata;
                $scope.leave_name = promise.leave_name;
                $scope.get_emp = promise.get_emp;
                $scope.allActivity = promise.privalue;
                $scope.approve = promise.approveid;
            });
        };

        $scope.searchchkbx1 = "";
        $scope.searchchkbx = "";
        $scope.all_checkC = function () {
            var checkStatus = $scope.usercheckC;
            var count = 0;
            angular.forEach($scope.designation_list, function (itm) {
                itm.selected = checkStatus;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        };

        //===ng_click function for cast (ALL voice versa)
        $scope.togchkbxC = function () {
            $scope.usercheck = $scope.designation_list.every(function (options) {
                return options.selected;
            });
        };
        $scope.isOptionsRequired2 = function () {
            return !$scope.group_list.some(function (options) {
                return options.selected;
            });
        };
        $scope.isOptionsRequired1 = function () {
            return !$scope.department_list.some(function (options) {
                return options.selected;
            });
        };
        $scope.isOptionsRequired = function () {
            return !$scope.designation_list.some(function (options) {
                return options.selected;
            });
        };


        $scope.AddToCart = function () {
            if ($scope.myForm.$valid) {

                $scope.empname = "";
                $scope.empid = "";
                $scope.empapprovallevel = "";
                $scope.addtocartflag = true;
                angular.forEach($scope.get_emp, function (dd) {
                    if (dd.hrmE_Id === Number($scope.ivrmuL_Id)) {
                        $scope.empname = dd.hrmE_EmployeeFirstName;
                        $scope.empid = dd.hrmE_Id;
                    }
                });

                if ($scope.TempData.length === 0) {
                    $scope.TempData.push({
                        ApprovalEmpName: $scope.empname, Approval_HRME_Id: $scope.empid, ApprovalLevelNo: Number($scope.HRLAON_SanctionLevelNo),
                        ApprovalFinalFlag: $scope.finalflag
                    });
                    $scope.ivrmuL_Id = "";
                    $scope.HRLAON_SanctionLevelNo = "";
                    $scope.finalflag = false;
                }
                else if ($scope.TempData.length > 0) {
                    $scope.levelcount = 0;
                    angular.forEach($scope.TempData, function (d) {
                        if (d.ApprovalLevelNo === Number($scope.HRLAON_SanctionLevelNo)) {
                            $scope.levelcount += 1;
                        }
                    });
                    if ($scope.levelcount === 0) {
                        $scope.empcount = 0;
                        angular.forEach($scope.TempData, function (d) {
                            if (d.Approval_HRME_Id === Number($scope.ivrmuL_Id)) {
                                $scope.empcount += 1;
                            }
                        });

                        if ($scope.empcount === 0) {
                            $scope.TempData.push({
                                ApprovalEmpName: $scope.empname, Approval_HRME_Id: $scope.empid, ApprovalLevelNo: Number($scope.HRLAON_SanctionLevelNo),
                                ApprovalFinalFlag: $scope.finalflag
                            });
                        } else {
                            swal("Approval Person Already Added");
                        }
                        $scope.ivrmuL_Id = "";
                        $scope.HRLAON_SanctionLevelNo = "";
                        $scope.finalflag = false;
                    } else {
                        swal("Sanction Level Order Already Added");
                    }
                }
            } else {
                $scope.submitted = true;
            }
        };

        $scope.DeleteTempdata = function (dd, index) {
            $scope.TempData.splice(index, 1);

            if ($scope.TempData.length === 0) {
                $scope.addtocartflag = false;
            }
        };




        $scope.AddToCartHRAuthorisation = function () {

            if ($scope.myForm.$valid) {
                $scope.empname = "";
                $scope.empid = "";
                $scope.empapprovallevel = "";
                $scope.addtocartflaghrauthorisation = true;

                angular.forEach($scope.allActivity, function (activ) {
                    if (activ.Selected == true) {
                        angular.forEach($scope.approve, function (dd) {
                            if (dd.id === Number($scope.id)) {
                                $scope.empname = dd.ivrmstauL_UserName;
                                $scope.empid = dd.id;
                            }
                        });

                        if ($scope.TempDataHRAuthorisation.length === 0) {
                            $scope.TempDataHRAuthorisation.push({
                                ApprovalEmpName: $scope.empname, Approval_HRME_Id: $scope.empid, ApprovalLevelNo: Number($scope.httapproveidorder), hR_PR_NAME: activ.hR_PR_NAME,
                                ApprovalFinalFlag: $scope.HRPAON_FinalFlg
                            });
                            $scope.id = "";
                            // $scope.httapproveidorder = "";
                            // $scope.HRPAON_FinalFlg = false;
                        }
                        else if ($scope.TempDataHRAuthorisation.length > 0) {
                            $scope.levelcount = 0;
                            angular.forEach($scope.TempDataHRAuthorisation, function (d) {
                                if (d.ApprovalLevelNo === Number($scope.httapproveidorder) && d.hR_PR_NAME == activ.hR_PR_NAME) {
                                    $scope.levelcount += 1;
                                }
                            });
                            if ($scope.levelcount === 0) {
                                $scope.empcount = 0;
                                angular.forEach($scope.TempDataHRAuthorisation, function (d) {
                                    if (d.Approval_HRME_Id === Number($scope.id) && d.hR_PR_NAME == activ.hR_PR_NAME) {
                                        $scope.empcount += 1;
                                    }
                                });

                                if ($scope.empcount === 0) {
                                    $scope.TempDataHRAuthorisation.push({
                                        ApprovalEmpName: $scope.empname, Approval_HRME_Id: $scope.empid, ApprovalLevelNo: Number($scope.httapproveidorder), hR_PR_NAME: activ.hR_PR_NAME,
                                        ApprovalFinalFlag: $scope.HRPAON_FinalFlg
                                    });
                                } else {
                                    swal("Approval Person Already Added");
                                }
                                $scope.id = "";
                                //$scope.httapproveidorder = "";
                                // $scope.HRPAON_FinalFlg = false;
                            } else {
                                swal("Sanction Level Order Already Added");
                            }
                        }
                    }
                });




            } else {
                $scope.submitted = true;
            }
        };

        $scope.DeleteTempdataHRAuthorisation = function (dd, index) {
            $scope.TempDataHRAuthorisation.splice(index, 1);

            if ($scope.TempDataHRAuthorisation.length === 0) {
                $scope.addtocartflaghrauthorisation = false;
            }
        };



        $scope.Savegroupdeptdesimapping = function () {

            $scope.hrfinalflag = false;
            $scope.leavefinalflag = false;

            var checkfinalflag = 0;
            angular.forEach($scope.TempDataHRAuthorisation, function (obj) {
                if (obj.ApprovalFinalFlag) {
                    checkfinalflag += 1;
                }
            });

            if (checkfinalflag === 0) {
                swal({
                    title: "Final Approval User Not Mapped For This Details",
                    text: "Do you want continue",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue it!",
                    cancelButtonText: "Cancel!!!!!!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            $scope.hrfinalflag = true;
                            //$scope.saveauthdata();
                        }
                        else {
                            swal("Cancelled");
                        }
                    });
            } else {
                $scope.hrfinalflag = true;
                //$scope.saveauthdata();
            }



            var checkfinalflagLeave = 0;
            angular.forEach($scope.TempData, function (obj) {
                if (obj.ApprovalFinalFlag) {
                    checkfinalflagLeave += 1;
                }
            });

            if (checkfinalflagLeave === 0) {
                swal({
                    title: "Final Approval User Not Mapped For This Details",
                    text: "Do you want continue",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, continue it!",
                    cancelButtonText: "Cancel!!!!!!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                           // $scope.saveauthdata();
                            $scope.leavefinalflag = true;
                            $scope.hrfinalflag = true

                            if ($scope.leavefinalflag && $scope.hrfinalflag) {
                                $scope.saveauthdata();
                            }
                        }
                        else {
                            swal("Cancelled");
                        }
                    });
            } else {
                $scope.leavefinalflag = true;
                //$scope.saveauthdata();
            }

            //if ($scope.leavefinalflag == true && $scope.hrfinalflag == true) {
            //    $scope.saveauthdata();
            //}



        };



        $scope.saveauthdata = function () {
            //if ($scope.myFormgroupdeptdesignationmapping.$valid)

            //this line code i giving direct valid true
            //$scope.myFormgroupdeptdesignationmapping.$valid = true;
            if ($scope.myFormgroupdeptdesignationmapping.$valid)
            {
                $scope.designations = [];
                angular.forEach($scope.designation_list, function (rr) {
                    if (rr.selected == true) {
                        $scope.designations.push(rr.hrmdeS_Id);
                    }
                });
                var data = {
                    "HRMGT_Id": $scope.hrmgT_Id.hrmgT_Id,
                    "HRMD_Id": $scope.hrmD_Id.hrmD_Id,
                    "designationids": $scope.designations
                };
                apiService.create("GroupDeptDessg/savedata", data).then(function (promise) {
                    if (promise.msg == 'Saved') {
                        swal("Record saved Successfully");
                        $state.reload();
                    }
                    else if (promise.msg == 'Failed') {
                        swal("Failed to Save the Record");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.interactedDesignation = function (field) {
            return $scope.submittedDesignation || field.$dirty;
        };
        
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        // Edit Single Record
        $scope.EditDataDesignation = function (record) {

            var id = record.hrmdeS_Id;
            apiService.getURI("MasterDesignation/editRecord", id).
                then(function (promise) {

                    if (promise.designationlList != null && promise.designationlList.length > 0) {
                        $scope.Designation = promise.designationlList[0];
                    }


                })
        }


        //deactivate record
        $scope.DeletRecordDesignation = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hrmdeS_ActiveFlag == false) {
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
                        apiService.getURI("MasterDesignation/ActiveDeactiveRecord", data.hrmdeS_Id).
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

                                    if (promise.designationlList !== null && promise.designationlList.length > 0) {

                                        $scope.gridOptionsDesignation.data = promise.designationlList;
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


        //Designation Set Order

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



        $scope.getdesignationOrder = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.hrmdeS_Id !== 0) {
                    orderarray[key].hrmdeS_Order = key + 1;
                }
            });
            var data = {
                DesignationDTO: orderarray,
            }
            apiService.create("MasterDesignation/validateordernumber", data).
                then(function (promise) {
                    if (promise.retrunMsg != "" && promise.retrunMsg != undefined && promise.retrunMsg != null) {
                        swal(promise.retrunMsg);
                        $scope.onLoadGetDataDesignation();
                    }
                });
        }

        //#endregion Master Designation Form Data Ends


        //#region Master Grade Form Data Starts

        // form Object
        $scope.Grade = {};


        // Datatable display
        $scope.gridOptionsGrade = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableHiding: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmG_GradeName', displayName: 'Grade Name', enableHiding: false },
                { name: 'hrmG_GradeDisplayName', displayName: 'Grade Display Name', enableHiding: false },
                { name: 'hrmG_PayScaleFrom', displayName: 'Pay Scale From', enableHiding: false },
                { name: 'hrmG_IncrementOf', displayName: 'Increment Of', enableHiding: false },
                { name: 'hrmG_PayScaleTo', displayName: 'Pay Scale To', enableHiding: false },
                { name: 'hrmG_PayScaleRange', displayName: 'Pay Scale Range', enableHiding: false },
                { name: 'hrmG_Order', displayName: 'Order', enableHiding: false },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditDataGrade(row.entity);" data-toggle="tooltip" title="Edit"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.hrmG_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecordGrade(row.entity);" data-toggle="tooltip" title="Activate"> Activate</a>' +
                        '<span ng-if="row.entity.hrmG_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecordGrade(row.entity);" data-toggle="tooltip" title="Deactivate">  Deactivate</a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }

        };

        // Get form Details at onload 

        $scope.onLoadGetDataGrade = function () {

            $scope.cancelGrade();

            var pageid = 2;
            apiService.getURI("MasterGrade/getalldetails", pageid).then(function (promise) {


                if (promise.gradelList !== null && promise.gradelList.length > 0) {

                    $scope.gridOptionsGrade.data = promise.gradelList;

                    $scope.gradeListOrder = promise.gradelList;
                }
            })
        }



        //Compare to VAlue
        $scope.FromVal = function (FromAmount, ToAmount) {
            if (parseFloat(FromAmount) > parseFloat(ToAmount)) {
                swal("Pay Scale From should not be greater than Pay Scale To", 'Please Change Your Pay Scale Range!');
                $scope.Grade.hrmG_PayScaleFrom = "";
                return false;
            } else {
                $scope.setPayScaleRange();
            }
        };
        $scope.Toval = function (FromAmount, ToAmount) {
            if (parseFloat(FromAmount) > parseFloat(ToAmount)) {
                swal("Range  Pay Scale To should be greater than Pay Scale From", 'Please Change Your Pay Scale Range!');
                $scope.Grade.hrmG_PayScaleTo = "";
                return false;
            } else {
                $scope.setPayScaleRange();
            }
        };

        $scope.disPayScaleRange = true;

        $scope.setPayScaleRange = function () {

            if ($scope.Grade.hrmG_PayScaleFrom != "" && $scope.Grade.hrmG_PayScaleFrom != undefined && $scope.Grade.hrmG_IncrementOf != "" && $scope.Grade.hrmG_IncrementOf != undefined && $scope.Grade.hrmG_PayScaleTo != "" && $scope.Grade.hrmG_PayScaleTo != undefined) {

                $scope.Grade.hrmG_PayScaleRange = $scope.Grade.hrmG_PayScaleFrom + "-" + $scope.Grade.hrmG_IncrementOf + "-" + $scope.Grade.hrmG_PayScaleTo;
            } else {
                $scope.Grade.hrmG_PayScaleRange = "";
            }


        }

        // clear form data
        $scope.cancelGrade = function () {
            // $scope.search = "";
            $scope.Grade = {};
            $scope.submittedGrade = false;
            $scope.myFormGrade.$setPristine();
            $scope.myFormGrade.$setUntouched();
            $scope.gridApi.grid.clearAllFilters();
        }

        //saving/updating Record
        $scope.submittedGrade = false;
        $scope.saveDataGrade = function () {
            $scope.submittedGrade = true;
            if ($scope.myFormGrade.$valid) {
                var data = $scope.Grade;

                apiService.create("MasterGrade/", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {

                            if (promise.retrunMsg == "Duplicate") {
                                swal("Grade Name is Already Exist..!!");
                                return;
                            } else if (promise.retrunMsg == "AllDuplicate") {
                                swal("Record Already Exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg == "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                            }
                            else if (promise.retrunMsg == "Add") {
                                swal("Record Saved Successfully.");
                            }
                            else if (promise.retrunMsg == "Update") {
                                swal("Record Updated Successfully.");
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                            }

                            if (promise.gradelList !== null && promise.gradelList.length > 0) {

                                $scope.gridOptionsGrade.data = promise.gradelList;

                            }
                            $scope.onLoadGetDataGrade();
                        }
                    })
            }

        };

        $scope.interactedGrade = function (field) {
            return $scope.submittedGrade || field.$dirty;
        };


        // Edit Single Record
        $scope.EditDataGrade = function (record) {

            var id = record.hrmG_Id;
            apiService.getURI("MasterGrade/editRecord", id).
                then(function (promise) {

                    if (promise.gradelList != null && promise.gradelList.length > 0) {
                        $scope.Grade = promise.gradelList[0];
                    }

                })
        }


        //deactivate record
        $scope.DeletRecordGrade = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hrmG_ActiveFlag == false) {
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
                        apiService.getURI("MasterGrade/ActiveDeactiveRecord", data.hrmG_Id).
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

                                    if (promise.gradelList !== null && promise.gradelList.length > 0) {
                                        // $scope.currentPage = 1;
                                        // $scope.itemsPerPage = 10;

                                        // $scope.employeeTypeList = promise.employeeTypeList;
                                        $scope.gridOptionsGrade.data = promise.gradelList;
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

        //#endregion Master Grade Form Data Ends


        //Designation Set Order


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


        $scope.getgradeOrder = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.hrmG_Id !== 0) {
                    orderarray[key].hrmG_Order = key + 1;
                }
            });
            var data = {
                GradeDTO: orderarray,
            }
            apiService.create("MasterGrade/validateordernumber", data).
                then(function (promise) {
                    if (promise.retrunMsg != "" && promise.retrunMsg != undefined && promise.retrunMsg != null) {
                        swal(promise.retrunMsg);
                        $scope.onLoadGetDataGrade();
                    }
                });
        }
        $scope.Editdata = function (del) {
            var data = {
                "HRGTDDS_Id": del.hrgtddS_Id
            }

            swal({
                title: "Are you sure",
                text: "Do you want to delete record????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("GroupDeptDessg/Editdata", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal('Record Deleted Successfully');
                                $scope.onloadgroupdeptdesignationmapping();
                            }
                            else {
                                swal('Record Not Deleted Successfully!');
                            }
                        });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };
        $scope.masterDecative = function (user) {
            var mgs = "";
            var confirmmgs = "";
            if (user.hrgtddS_ActiveFlg == false) {
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
                        var data = {
                            "HRGTDDS_Id": user.hrgtddS_Id
                        }
                        apiService.create("GroupDeptDessg/masterDecative", data).
                            then(function (promise) {


                                if (promise.msg !== "") {

                                    if (promise.msg === "Activated") {
                                        swal("Record Activated successfully");
                                        $scope.onloadgroupdeptdesignationmapping();
                                    }
                                    else if (promise.msg === "Deactivated") {
                                        swal("Record Deactivated successfully");
                                        $scope.onloadgroupdeptdesignationmapping();
                                    }
                                    else {
                                        swal("Record Not Activated/Deactivated", 'Fail');
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