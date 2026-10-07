(function () {
    'use strict';
    angular
        .module('app')
        .controller('SecurityRoasterController', SecurityRoasterController)

    SecurityRoasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce'];
    function SecurityRoasterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce) {

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.itemsPerPage1 = 10;
        $scope.currentPage1 = 1;
        $scope.page1 = "page1";
        $scope.search = " ";
        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        $scope.isOptionsRequired = function () {
            if ($scope.group_check == "1") {
                return !$scope.floorlist.some(function (options) {
                    return options.dayy = $scope.group_check;
                });
            }
            else if ($scope.group_check == "0") {
                return false;
            }
        };

        $scope.load_group_check = function () {
            var toggleStatus = $scope.group_check;
            angular.forEach($scope.floorlist, function (val) {
                val.dayy = toggleStatus;
            });
        };

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.submitted = false;
        $scope.getdata = function () {
            var date1 = $scope.ismsR_Date == null ? "" : $filter('date')($scope.ismsR_Date, "yyyy-MM-dd");
          
            if ($scope.myForm.$valid) {
                $scope.flrlist = [];
                angular.forEach($scope.floorlist, function (val) {
                    if (val.dayy == true) { $scope.flrlist.push(val.hrmF_Id); }
                });

                var data = {
                    "ISMSR_Id": $scope.ISMSR_Id,
                    "ISMSD_Id": $scope.ismsD_Id,
                    "ISMSR_Date": date1,
                    "flrlist": $scope.flrlist,
                    "ISMSR_Status": $scope.ismsR_Status,  
                    "ISMSR_From": $filter('date')($scope.ismsR_From, "HH:mm"),
                    "ISMSR_To": $filter('date')($scope.ismsR_To, "HH:mm"),
                    "ISMSR_Remarks": $scope.ismsR_Remarks
                };
                apiService.create("SecurityRoaster/getdata", data).then(function (promise) {

                    if (promise.msg === 'Saved') {
                        swal("Data Saved Successfully.....!!!!!");
                        $state.reload();
                    }
                    else if (promise.msg === 'Failed') {
                        swal("Data Not Saved Successfully.....!!!!!");
                        $state.reload();
                    }
                    else if (promise.msg === 'updated') {
                        swal("Data Updated.....!!!!!");
                        $state.reload();
                    }
                    else if (promise.msg === 'failed') {
                        swal("Data Not Updated Successfully.....!!!!!");
                        $state.reload();
                    }
                    else if (promise.duplicate === true) {
                        swal("Data already Exists.....!!!!!");
                    }
                    else {
                        swal("Something is Wrong...");
                    }
                });

            }
            else {
                $scope.submitted = true;
            }

        };
        $scope.search = '';
        $scope.filtervalue1 = function (user) {
            //return (angular.lowercase(user.name)).indexOf(angular.lowercase($scope.search)) >= 0 ||
            //    (angular.lowercase(user.number)).indexOf(angular.lowercase($scope.search)) >= 0 ||
            //    (angular.lowercase(user.email)).indexOf(angular.lowercase($scope.search)) >= 0;
        };
        $scope.getalldetails = function () {
            var data = {
               
            }
            apiService.create("SecurityRoaster/getalldetails", data).then(function (promise) {
                $scope.alldata = promise.alldata;
                $scope.floorlist = promise.floorlist;//floor
                $scope.namelist = promise.namelist;//names
            });
        };
        $scope.edittab1 = function (user) {
            var data = {
                "ISMSR_Id": user.ismsR_Id
            };
            apiService.create("SecurityRoaster/edittab1", data).then(function (promise) {
                $scope.ISMSR_Id = promise.editlist[0].ismsR_Id;
                $scope.ismsD_Id = promise.editlist[0].ismsD_Id;
                $scope.ismsR_Date = new Date(promise.editlist[0].ismsR_Date);
                $scope.hrmF_Id = promise.editlist[0].hrmF_Id;
                $scope.ismsR_From = moment(promise.editlist[0].ismsR_From, 'HH:mm').format();
                $scope.ismsR_To = moment(promise.editlist[0].ismsR_To, 'HH:mm').format();
                $scope.ismsR_Remarks = promise.editlist[0].ismsR_Remarks;
                $scope.ismsR_Status = promise.editlist[0].ismsR_Status;
                $scope.ismsD_FirstName = promise.editlist[0].name;
                $scope.hrmF_FloorName = promise.editlist[0].floorname;
                
                 name = promise.editlist[0].hrmF_Id;

                for (var i = 0; i < $scope.floorlist.length; i++) {
                    $scope.floorlist[i].dayy = false;
                    if (name == $scope.floorlist[i].hrmF_Id) {
                        $scope.floorlist[i].dayy = true;
                        //$scope. arrlistchk[i].disablegroups = true;
                    }
                    else {
                        $scope.floorlist[i].dayy= false;
                        // $scope.arrlistchk[i].disablegroups = true;
                    }
                }
                
            });
        };
        $scope.deactivYTab1 = function (usersem, SweetAlert) {

            var dystring = "";
            if (usersem.ismsR_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (usersem.ismsR_ActiveFlag === false) {
                dystring = "Activate";
            }
            swal({

                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("SecurityRoaster/deactive", usersem).
                            then(function (promise) {
                                if (promise.ret === true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                                else {

                                    swal("Record Not " + dystring + "d " + "Successfully!!!");
                                    $state.reload();
                                }
                            });
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }

                });
        };

        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();
