(function () {
    'use strict';
    angular
        .module('app')
        .controller('FF_Settlement_RequestProcessController', FF_Settlement_RequestProcessController)
    FF_Settlement_RequestProcessController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function FF_Settlement_RequestProcessController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

        $scope.hreffS_EntryDate = new Date();
        $scope.obj = {};

        $scope.onLoadGetData = function ()
        {
            var pageid = 2;
             
            apiService.getURI("FF_Settlement_Process/getalldetails", pageid).then(function (promise) {
                if (promise.griddisplay !== null && promise.griddisplay.length > 0) {
                    $scope.getdata = promise.griddisplay;
                }   
                if (promise.empadvaList !== null && promise.empadvaList.length > 0) {
                    $scope.gridOptions = promise.empadvaList;
                }
            })
        };
        $scope.submitted = false;

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.saveData = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                var data = {
                    "HREFFS_Id": $scope.hreffS_Id,
                    "HREFFS_EntryDate": $scope.hreffS_EntryDate,                 
                    "HREFFS_Remarks": $scope.hreffS_Remarks,                    
                }
                apiService.create("FF_Settlement_Process/SaveUpdate", data).then(function (promise) {
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
                            $state.reload();
                        }
                        else if (promise.retrunMsg == "Update") {
                            swal("Record Updated Successfully..");
                            $state.reload();
                        }                                               
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                            return;
                        }
                        $scope.cancel();
                    }
                })
                       
            }
        };
        $scope.Edit = function (item) {
            var data = {
                "HREFFS_Id": item.hreffS_Id,
            };

            apiService.create("FF_Settlement_Process/Edit", data).then(function (promise) {
                if (promise !== null) {
                    $scope.empadvaList = promise.empadvaList;
                    $scope.editflag = true;
                    $scope.hreffS_Id = promise.empadvaList[0].hreffS_Id; 
                    $scope.hreffS_Remarks = promise.empadvaList[0].hreffS_Remarks;                  
                }
            })
        }
        // clear form data
        $scope.cancel = function () {
            $scope.search = "";
            $state.reload();
        };                
        
        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hresA_ActiveFlag == false) {
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
                        apiService.DeleteURI("FF_Settlement_Process/ActiveDeactiveRecord", data.hreffS_Id).
                            then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Activated") {
                                        swal("Record Activated successfully");
                                        $state.reload();
                                    }
                                    else if (promise.retrunMsg === "Deactivated") {
                                        swal("Record Deactivated successfully");
                                        $state.reload();
                                    }
                                    else {
                                        swal("Record Not Activated/Deactivated", 'Fail');
                                    }
                                    if (promise.empadvaList !== null && promise.empadvaList.length > 0) {
                                        $scope.gridOptions.data = promise.empadvaList;
                                        angular.forEach($scope.gridOptions.data, function (value, key) {
                                            var fdate = value.hresA_EntryDate.split('T');
                                            value.hresA_EntryDate = fdate[0];

                                        });
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