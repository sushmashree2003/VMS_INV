
(function () {
    'use strict';
    angular
.module('app')
.controller('AreaGroupMappingController', AreaGroupMapping)

    AreaGroupMapping.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function AreaGroupMapping($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.dselectAll1 = false;
        $scope.sortKey = "fgaM_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa
        $scope.searchValue = "";
        $scope.loaddata = function () {
            
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("AreaGroupMapping/getalldetails", pageid).
        then(function (promise) {
            
            $scope.totcountfirst = promise.fillareagroup.length;
            $scope.fillareagroup = promise.fillareagroup;
            $scope.fillarea = promise.fillarea;
            $scope.fillgroup = promise.fillgroup;           
        })
        }

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.toggleAll1 = function () {
            var toggleStatus = $scope.selectAll1;
            angular.forEach($scope.fillarea, function (role) { role.selected = toggleStatus; });

        }

        $scope.optionToggled1 = function (selectedrole) {
            
            if ($scope.fgaM_Id > 0)
            {
                for (var i = 0; i < $scope.fillarea.length; i++) {
                    if (selectedrole.trmA_Id == $scope.fillarea[i].trmA_Id) {
                        if (selectedrole.selected == true)
                            $scope.fillarea[i].selected = true
                        else
                            $scope.fillarea[i].selected = true
                    }
                    else {
                        $scope.fillarea[i].selected = false;
                    }
                }
                
            }
            else
            {
                $scope.selectAll1 = $scope.fillarea.every(function (role) { return role.selected; })
            }
            
        }
        $scope.isOptionsRequired1 = function () {
            return !$scope.fillarea.some(function (options) {
                return options.selected;
            });
        }
      
        $scope.submitted = false;
        $scope.savedata = function (arrlistchk) {
            
            if ($scope.myForm.$valid) {
                $scope.albumNameArray = [];
                angular.forEach(arrlistchk, function (role) {
                    if (!!role.selected) $scope.albumNameArray.push(role);
                })

                if ($scope.albumNameArray.length === 0) {
                    swal('Please Select Area From List Atleast One....!')
                }
                else {
                    if ($scope.fgaM_Id > 0)
                    {
                        var TRMA_Id1 = 0;
                        for (var i = 0; i < $scope.albumNameArray.length; i++) {
                            TRMA_Id1 = $scope.albumNameArray[i].trmA_Id;
                        }                      
                        var data = {
                            "FGAM_Id": $scope.fgaM_Id,
                            "FMG_Id": $scope.FMG_GroupName,
                            "FGAM_ActiveFlag": true,
                            "FGAM_WayFlag": "1",
                            "TRMA_Id": TRMA_Id1
                            //"TempararyArrayList": $scope.albumNameArray,
                        }
                    }
                    else
                    {
                        var data = {
                            "FGAM_Id": $scope.fgaM_Id,
                            "FMG_Id": $scope.FMG_GroupName,
                            "FGAM_ActiveFlag": true,
                            "FGAM_WayFlag": "1",
                            "TempararyArrayList": $scope.albumNameArray,
                        }
                    }
                   
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("AreaGroupMapping/", data).
                    then(function (promise) {
                        if (promise.returnduplicatestatus == 'Duplicate') {
                            swal("Record Already Exist");
                            $scope.cance1($scope.arrlistchk);
                            $scope.loaddata();
                            $scope.cance();
                        }

                        else if (promise.returnduplicatestatus == "Save") {
                            $scope.students = promise.groupData;
                            swal('Record Saved Successfully');                            
                            $scope.loaddata();
                            $scope.cance();
                        }
                        else if (promise.returnduplicatestatus == "NotSave") {
                            swal('Record Not Saved');
                        }
                        else if (promise.returnduplicatestatus == "Update") {
                            swal('Record Updated Successfully');
                            $scope.loaddata();
                            $scope.cance();
                            
                        }
                        else if (promise.returnduplicatestatus == "NotUpdate") {
                            swal('Record Not Updated');                                                      
                        }
                        else {
                            swal('Error');
                        }
                    })
                }
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.interacted = function (field) {

            return $scope.submitted;
        };
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.fgaM_ActiveFlag == true) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {

                apiService.create("AreaGroupMapping/deactivate", user).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal("Record " + confirmmgs + " Successfully");
                    }
                    else {
                        swal("Record " + confirmmgs + " Successfully");
                    }
                    $scope.loaddata();
                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }
        $scope.cance = function () {
            
            $state.reload();
        };
        $scope.getorgvalue = function (employee, fillarea) {
            
            $scope.editEmployee = employee.fgaM_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("AreaGroupMapping/getdetails", pageid).
            then(function (promise) {
                for (var i = 0; i < fillarea.length; i++) {
                    name = fillarea[i].trmA_Id;
                    if (name == promise.fillareagroup[0].trmA_Id) {
                        fillarea[i].selected = true
                    }
                    else {
                        fillarea[i].selected = false;
                    }
                }
                $scope.fgaM_Id = promise.fillareagroup[0].fgaM_Id;
                $scope.FMG_GroupName = promise.fillareagroup[0].fmG_Id;
                $scope.fgaM_WayFlag = promise.fillareagroup[0].fgaM_WayFlag;
                $scope.dselectAll1 = true;
                if (promise.fillareagroup[0].fgaM_ActiveFlag == "1") {
                    $scope.fgaM_ActiveFlag = true;
                }
                else {
                    $scope.fgaM_ActiveFlag = false;
                }                
            })
        }
    };
})();


