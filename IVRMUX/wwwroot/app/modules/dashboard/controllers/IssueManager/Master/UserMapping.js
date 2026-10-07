(function () {
    'use strict';
    angular
        .module('app')
        .controller('UserMappingController', UserMappingController)

    UserMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function UserMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;
        $scope.search2 = "";
        $scope.sort = function (keyname) {
            $scope.sortKey2 = keyname;   //set the sortKey to the param passed
            $scope.reverse2 = !$scope.reverse2; //if true make it false and vice versa
        };
        //=========================================Load data
        $scope.Loaddata = function () {
            var pageid = 2;
            apiService.getURI("UserMapping/getdetails", pageid).then(function (promise) {
                $scope.clientlist = promise.clientlist;
                $scope.ieEmpList = promise.ieEmpList;
                $scope.allIeMappingdata = promise.allIeMappingdata;
            });
        };
        //===========================================================End
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };
        //===========----Clear Field
        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.searchchkbx1 = "";
        $scope.all_checkC = function () {
            var checkStatus = $scope.usercheckC;
            angular.forEach($scope.ieEmpList, function (itm) {
                itm.selected = checkStatus;
            });
        };
        $scope.togchkbxC = function () {
            $scope.usercheckC = $scope.ieEmpList.every(function (options) {
                return options.selected;
            });
        };
        $scope.isOptionsRequired = function () {
            return !$scope.ieEmpList.some(function (options) {
                return options.selected;
            });
        };
        /////////////////////////////....................Save2
        $scope.submitted2 = false;
        $scope.saveClientMappingdata = function () {
            
            $scope.ieEmpListdata = [];
            if ($scope.myForm2.$valid) {
                angular.forEach($scope.ieEmpList, function (list) {
                    if (list.selected === true) {
                        $scope.ieEmpListdata.push({ User_Id: list.userId });
                    }
                });

                var data = {
                    "ISMMCLTUS_Id": $scope.ismmcltuS_Id,
                    "ISMMCLT_Id": $scope.ismmclT_Id,
                    ieEmpListdata: $scope.ieEmpListdata,
                }

                apiService.create("UserMapping/saveClientMappingdata", data).then(function (promise) {
                    if (promise.returnval != null) {
                        if (promise.returnval == true) {
                            if ($scope.ismmcltuS_Id > 0) {
                                swal('Record Updated Successfully!!!');
                                $scope.allIeMappingdata = promise.allIeMappingdata;
                            }
                            else {
                                swal('Record Saved Successfully!!!');
                                $scope.allIeMappingdata = promise.allIeMappingdata;
                            }
                        }
                        else {
                            if (promise.returnval == false) {
                                if ($scope.ismmcltuS_Id > 0) {
                                    swal('Record Not Updated Successfully!!!');
                                }
                                else {
                                    swal('Record Not Saved Successfully!!!');
                                }
                            }
                        }

                        $scope.Clearid();
                    }
                    else {
                        swal("Kindly Contact Administrator!!!");
                    }

                })
            }
            else {
                $scope.submitted2 = true;
            }
        }

        //=====================Edit2  record
        $scope.editClientMappingdata = function (user) {
            
            var data = {
                "ISMMCLT_Id": user.ismmclT_Id
            }
            apiService.create("UserMapping/editClientMappingdata", user).then(function (promise) {


                $scope.ismmcltiE_Id = promise.editCltMappinglist[0].ismmcltiE_Id;
                $scope.ismmclT_Id = promise.editCltMappinglist[0].ismmclT_Id;
                $scope.hrmE_Id = promise.editCltMappinglist[0].ismciM_IEList;

                angular.forEach($scope.ieEmpList, function (tt) {
                    angular.forEach(promise.editCltMappinglist, function (ss) {
                        if (tt.hrmE_Id == ss.ismciM_IEList) {
                            tt.selected = true;
                        }
                    })
                })
            });
        }
        //====================End-Edit2


        //=================Activation 2 /Deactivation 2
        $scope.deactiveClientMappingdata = function (user, SweetAlert) {
            
            $scope.ismmcltuS_Id = user.ismmcltuS_Id;

            var dystring = "";
            if (user.ismmcltuS_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.ismmcltuS_ActiveFlag == 0) {
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
                        apiService.create("UserMapping/deactiveClientMappingdata", user).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                    $scope.modalUserMapingList = promise.modalUserMapingList;
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }

                            })

                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }

                });
        }
        //=================================End-Activation2/Deactivation2

        $scope.cancel = function () {
            $scope.reload();
        };

        $scope.get_MappedUserlist = function (user) {

            var data = {
                "ISMMCLT_Id": user.ismmclT_Id,
            }
            apiService.create("UserMapping/get_MappedUserlist", data).then(function (promise) {

                $scope.modalUserMapingList = promise.modalUserMapingList;
            })
        }


    }
})();

