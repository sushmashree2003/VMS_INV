(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterHolidayWorkingTypeController', MasterHolidayWorkingTypeController)

    MasterHolidayWorkingTypeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter']
    function MasterHolidayWorkingTypeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter) {

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            apiService.getURI("MasterHolidayWorkingType/getalldetails", pageid).then(
                function (promise) {

                    $scope.getarray = promise.getarray;
                });
        };

        $scope.savedata = function () {
            var data = {
                "FOHWDT_Id": $scope.fohwdT_Id,
                "FOHTWD_HolidayWDType": $scope.fohtwD_HolidayWDType,
                "FOHTWD_HolidayWDTypeFlag": $scope.fohtwD_HolidayWDTypeFlag,
                "FOHTWD_HolidayFlag": $scope.fohtwD_HolidayFlag,
            }
            apiService.create("MasterHolidayWorkingType/savedetails", data).then(function (promise) {
                if (promise.returnmsg == "Saved") {
                    swal("Record Saved Successfully");
                }
                else if (promise.returnmsg == "Failed") {
                    swal("Record NOt Saved");
                }
                else if (promise.returnmsg == "Updated") {
                    swal("Record Updated Successfully");
                }
                else if (promise.returnmsg == "notupdated") {
                    swal("Record Not Updated");
                }
            })
        };

        $scope.edit = function (user)
        {
            $scope.fohtwD_HolidayWDType = user.fohtwD_HolidayWDType;
            $scope.fohtwD_HolidayWDTypeFlag = user.fohtwD_HolidayWDTypeFlag;
            $scope.fohtwD_HolidayFlag = user.fohtwD_HolidayFlag;
            $scope.fohwdT_Id = user.fohwdT_Id;

        }
       





    }
})();

