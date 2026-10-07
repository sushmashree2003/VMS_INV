(function () {
    'use strict';
    angular
.module('app')
.controller('HallTicketGenerationController', HallTicketGenerationController)

    HallTicketGenerationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function HallTicketGenerationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("HallTicketGeneration/getdetails",pageid).
            then(function (promise) {

                $scope.acdlist = promise.acdlist;
                $scope.ctlist = promise.ctlist;
                $scope.seclist = promise.seclist;      
                $scope.examlist = promise.examlist;            
            })
        };

        $scope.onselectAcdYear = function (ASMAY_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id
            }
            apiService.create("HallTicketGeneration/onselectAcdYear", data).
            then(function (promise) {
                $scope.ctlist = promise.ctlist;

            })
        };

        $scope.onselectclass = function (ASMCL_Id, ASMAY_Id) {

            var data = {
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id
            }
            apiService.create("HallTicketGeneration/onselectclass", data).
            then(function (promise) {
                $scope.seclist = promise.seclist;
            })
        };

        $scope.submitted = false;
        $scope.savedetail = function () {
            $scope.submitted = true;
            

            if ($scope.myForm.$valid) {
                $scope.checkseclist = [];
                angular.forEach($scope.seclist, function (option3) {
                    if (!!option3.section) $scope.checkseclist.push(option3);
                })

                var data = {
                    
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "section_Array": $scope.checkseclist,
                    "EME_Id": $scope.EME_Id,
                    "prefixstr": $scope.prefix,
                    "startno": $scope.startno,
                    "increment": $scope.increment,
                    "leadingzeros": $scope.preno
                   
                }
                apiService.create("HallTicketGeneration/savedetail", data).
                    then(function (promise) {
                        if (promise.returnval === true) {

                            swal('Data Saved successfully');
                            $scope.cancel();
                            $scope.BindData();
                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                            swal('Records Already Exist !');
                        }
                        else {
                            swal('Data Not Saved !');
                        }

                    })
            }

        };


        $scope.isOptionsRequired1 = function () {

            return !$scope.seclist.some(function (option3) {
                return option3.asmS_Id;
            });
        }

        $scope.cancel = function () {
            $scope.ASMAY_Id = '';
            $scope.ASMCL_Id = '';
            $scope.ASMS_Id = '';
            $scope.EME_Id = '';
            $scope.prefix = '';
            $scope.startno = '';
            $scope.increment = '';
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

    }

})();