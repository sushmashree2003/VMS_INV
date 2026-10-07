(function () {
    'use strict';
    angular
        .module('app')
        .controller('Lead_To_ClientController', Lead_To_ClientController)

    Lead_To_ClientController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function Lead_To_ClientController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        //$scope.sortKey = 'ISMCLTPRBOM_Id';
        $scope.sortKey = '';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
        //get data
        $scope.module_list = "";
        $scope.components_dd = [];
        $scope.materaldocuupload = [{ itrS_Id: 'trans1' }];
        if ($scope.materaldocuupload.length === 1) {
            $scope.cnt = 1;
        }
        $scope.getAllDetail = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("Lead_To_Client/getdetails", pageid)
                .then(function (promise) {
                    $scope.leadlist_dd = promise.leadlist_dd;
                    // $scope.get_department = promise.get_department;
                    $scope.project_list = promise.project_list;
                   
                    //$scope.components_dd = promise.components_dd;
                    $scope.components_dddd = promise.components_dd;
                   // materaldocuupload
                  //  $scope.materaldocuupload[0].components_dd = $scope.components_dddd;
                })
        }

        //save
        $scope.isOptionsRequired = function () {
            return !$scope.module_list.some(function (options) {
                return options.selected;
            });
        }
        $scope.togchkbxC = function () {

            $scope.usercheckC = $scope.module_list.every(function (options) {
                return options.selected;
            });
        }
        $scope.all_checkC = function () {
            var checkStatus = $scope.usercheckC;
            var count = 0;
            angular.forEach($scope.module_list, function (itm) {
                itm.selected = checkStatus;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }
        $scope.submitted = false;


        //edit
        $scope.getproject = function () {
            var data = {
                "ISMMCLT_Id": $scope.obj.ISMMCLT_Id.ismmclT_Id,
                "HRMD_Id": $scope.hrmD_Id
            }
            apiService.create("Lead_To_Client/modalListdata", data).then(function (promise) {
                $scope.module_list = "";
                $scope.project_list = promise.project_list;
            })
        }
        $scope.cancel = function () {
            $state.reload();
        }
        //deactive

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        //getmodule
        $scope.getmodule = function (projectid, deptid) {
            $scope.module_list = [];
            var data = {

                "ISMMPR_Id": $scope.obj.ISMMPR_Id.ismmpR_Id,

            }
            apiService.create("Lead_To_Client/OnChangeTab2Inst", data).then(function (promise) {
                if (promise.module_list != null && promise.module_list.length > 0) {
                    $scope.module_list = promise.module_list;
                }
                else {
                    swal("Modules Not Found");
                }


            })
        }

       
        $scope.addgrnrows = function (matrial) {
            //$scope.submitted = true;
           // matrial.ISMCLTC_Id = "";
            // $scope.OnChangeTab1Inst = [];
           // if ($scope.myForm.$valid) {              
                if ($scope.materaldocuupload.length > 1) {
                    for (var i = 0; i === $scope.materaldocuupload.length; i++) {
                        var id = $scope.materaldocuupload[i].itrS_Id;
                        var lastChar = id.substr(id.length - 1);
                        $scope.cnt = parseInt(lastChar);
                    }
                }
                $scope.cnt = $scope.cnt + 1;
                var ct = $scope.cnt;
                $scope.tet = 'trans' + $scope.cnt;
                var newItemNo = $scope.cnt;
                $scope.materaldocuupload.push({ 'itrS_Id': 'trans' + newItemNo });
                angular.forEach(matrial, function (itmddd) {
                    $scope.complist.push({
                        ISMCLTC_Id: itmddd.ISMCLTC_Id
                    })

                });
                var data = {
                    "complist": $scope.complist
                }
                apiService.create("Lead_To_Client/OnChangeTab1Inst", data).then(function (promise) {
                    $scope.materaldocuupload[ct].components_dddd = promise.components_dd;
                })
         //   }



        };
       
        $scope.removegrnrows = function (index, data, matrial) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

            angular.forEach(matrial, function (itmddd) {
                $scope.complist.push({
                    ISMCLTC_Id: itmddd.ISMCLTC_Id
                })

            });
            var data = {
                "complist": $scope.complist
            }
            apiService.create("Lead_To_Client/OnChangeTab1Inst", data).then(function (promise) {
                $scope.materaldocuupload.components_dddd = promise.components_dd;
              //  $scope.components_dddd = promise.components_dd
            })
        };
    }

})();



