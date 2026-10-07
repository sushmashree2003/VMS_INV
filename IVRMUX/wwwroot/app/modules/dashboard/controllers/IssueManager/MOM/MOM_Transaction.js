(function () {
    'use strict';
    angular.module('app').controller('MOM_TransactionController', MOM_TransactionController)
    MOM_TransactionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache', 'myFactoryvisitor']
    function MOM_TransactionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache, myFactoryvisitor) {

        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.obj1 = {};
        $scope.obsupdate = {};
        $scope.edit = false;
        $scope.plMaxdate = new Date();
        $scope.IVRMMOMT_Date = new Date();
        $scope.plMaxdate.setDate($scope.plMaxdate.getDate());
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };
        var miid = myFactoryvisitor.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }
        $scope.radiomodal = 'int_dept';
        $scope.radiomodal1 = 'int_dep';

        $scope.loadgrid = function () {
            $scope.staffData = [];
            $scope.radiomodal1 = 'int_dep';
            $scope.radiomodal = 'int_dept';
            apiService.getURI("MOM_Transaction/getDetails/", $scope.mI_Id).then(function (promise) {
                $scope.staffData = promise.staffData;
            });
        };


        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        $scope.submitted = false;




        //===========================ADD==================================
        $scope.employeenameschange = [{ id: 'sell11' }];

        $scope.addNewDetail = function () {
            var newItemNo = $scope.employeenameschange.length + 1;
            if (newItemNo <= 30) {
                $scope.employeenameschange.push({ 'id': 'sell11' + newItemNo });
            }
        };

        $scope.removeNewDetail = function (index, data) {
            var newItemNo = $scope.employeenameschange.length - 1;
            $scope.employeenameschange.splice(index, 1);
            if (data.hrmE_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
        };
        //==============================


       



   

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
        $scope.meetingdetails = function (user) {
            var data = {
                "MI_Id": $scope.mI_Id,
            };

            apiService.create("AddVisitors/visitornameschange/", data).then(function (promise) {

                if (promise.emplist.length > 0 && promise.emplist !== null) {
                    $scope.emplist = promise.emplist;
                    $('#meetingdetails').modal('show');
                }
                else {
                    swal('No Data Found..!!');
                }

            });
        };


        $scope.edittab1 = function (user) {
            var data = {

            };

        };

    }
})();