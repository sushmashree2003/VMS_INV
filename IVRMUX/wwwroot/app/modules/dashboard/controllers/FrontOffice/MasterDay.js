(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterDayController', MasterDayController)

    MasterDayController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter']
    function MasterDayController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter) {

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            apiService.getURI("MasterDay/getalldetails", pageid).then(function (promise) {

                $scope.getarray = promise.getarray;
                $scope.presentCountgrid = promise.getarray.length;

                });
        };


        $scope.savedata = function () {
            var data = {
                "FOMD_Id": $scope.fomD_Id,
                "FOMD_DayName": $scope.fomD_DayName,
                "FOMD_DayCode": $scope.fomD_DayCode,
            }
            apiService.create("MasterDay/savedetails", data).then(function (promise) {

                if (promise.returnmsg == "Saved") {
                    swal("Record Saved");
                }
                else if (promise.returnmsg == "Faild") {
                    swal("Record Not Saved");
                }
                else if (promise.returnmsg == "Updated") {
                    swal("Record Updated Successfully");
                }
                else if (promise.returnmsg == "notupdated") {
                    swal("Record Not Updated");
                }
            })
        };

        $scope.edit = function (user) {
            $scope.fomD_DayName = user.fomD_DayName;
            $scope.fomD_DayCode = user.fomD_DayCode;
            $scope.fomD_Id = user.fomD_Id;
        }



        

        $scope.deactive = function (item, SweetAlert) {
            $scope.FOMD_Id = item.fomD_Id;
            var dystring = "";
            if (item.fomD_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.fomD_ActiveFlag == false) {
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
                        apiService.create("MasterDay/deactive", item).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        }











       

    }
})();

