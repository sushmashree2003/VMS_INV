(function () {
    'use strict';
    angular.module('app').controller('EmailDepartmentMappingController', EmailDepartmentMappingController)

    EmailDepartmentMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function EmailDepartmentMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;
        $scope.searchValueU = "";
        $scope.emailsstd = [{ id: 'emailsstd' }];
        //=====================Load--data.............
        $scope.Loaddata = function () {
            $scope.currentPageU = 1;
            $scope.itemsPerPageU = 10;
            $scope.search = "";

            var pageid = 2;
            apiService.getURI("EmailDepartmentMapping/getdetails", pageid).then(function (promise) {
                $scope.deptartmentlist = promise.getdepartment;
                $scope.newuser = promise.getsavelist;

            });
        };

        $scope.onchangedept = function () {
            var data = {
                "HRMD_Id": $scope.HRMD_Id
            };
            apiService.create("EmailDepartmentMapping/onchangedept", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getonchangedata !== null && promise.getonchangedata.length > 0) {

                        $scope.emailsstdedits = promise.getonchangedata;
                        $scope.ISM_EId = $scope.emailsstdedits[0].isM_EId;
                        $scope.HRMD_Id = $scope.emailsstdedits[0].hrmD_Id;

                        $scope.emailsstdedit = $scope.emailsstdedits[0].emailids;

                        var res = $scope.emailsstdedit.split(",");
                        $scope.temparray = [];
                        angular.forEach(res, function (dd) {
                            $scope.temparray.push({ emaild: dd });
                        });

                        $scope.emailsstd = $scope.temparray;

                    }
                }
            });
        };


        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.savedata = function () {
            if ($scope.frmmodule.$valid) {
                var data = {
                    "HRMD_Id": $scope.HRMD_Id,
                    "ISM_EId": $scope.ISM_EId,
                    "multipleemailid": $scope.emailsstd
                };

                apiService.create("EmailDepartmentMapping/savedata", data).then(function (promise) {
                    if (promise !== null) {

                        if (promise.message === "Save") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.message === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else {
                            swal("Failed To Save / Update Record");
                        }
                        $state.reload();
                    }

                });
            }
            else {
                $scope.submitted = true;
            }
        };




        //=====================Editrecord....

        $scope.Editdata = function (user) {
            var data = {
                "HRMD_Id": user.hrmD_Id,
                "ISM_EId": user.isM_EId
            };
            apiService.create("EmailDepartmentMapping/Editdata", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.geteditdata !== null && promise.geteditdata.length > 0) {

                        $scope.emailsstdeditsnew = promise.geteditdata;
                        $scope.ISM_EId = $scope.emailsstdeditsnew[0].isM_EId;
                        $scope.HRMD_Id = $scope.emailsstdeditsnew[0].hrmD_Id;

                        $scope.emailsstdeditnew = $scope.emailsstdeditsnew[0].emailids;

                        var res = $scope.emailsstdeditnew.split(",");

                        $scope.temparraynew = [];
                        angular.forEach(res, function (dd) {
                            $scope.temparraynew.push({ emaild: dd });
                        });


                        $scope.emailsstd = $scope.temparraynew;
                        //$scope.emailsstd = res;

                    }
                }
            });
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.Clearid = function () {
            $state.reload();
        };


        // add new row
        $scope.showAddEmail1std = function (email) {
            return email.id === $scope.emailsstd[$scope.emailsstd.length - 1].id;
        };

        //adding email id student

        $scope.emailsstd = [{ id: 'emailsstd1' }];
        $scope.addNewEmail1std = function () {
            var newItemNostd2 = $scope.emailsstd.length + 1;
            if (newItemNostd2 <= 5) {
                $scope.emailsstd.push({ 'id': 'emailsstd1' + newItemNostd2 });
            }
        };

        $scope.removeNewEmail1std = function (index) {
            var newItemNostd = $scope.emailsstd.length - 1;
            if (newItemNostd !== 0) {
                $scope.delmsrd = $scope.emailsstd.splice(index, 1);
            }
        };


        $scope.addtopdays = function (user, index) {

            for (var k = 0; k < $scope.emailsstd.length; k++) {
                var roll = user.emaild;
                var arryind = $scope.emailsstd.indexOf($scope.emailsstd[k]);
                console.log(arryind);
                if (parseInt(arryind) !== parseInt(index)) {
                    if ($scope.emailsstd[k].emaild === roll) {
                        swal("Already Exist");
                        $scope.emailsstd[index].emaild = "";
                        break;
                    }
                }
            }
        };


    }
})();

