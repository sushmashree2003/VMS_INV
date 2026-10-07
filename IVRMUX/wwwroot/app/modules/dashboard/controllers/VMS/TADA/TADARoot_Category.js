
(function () {
    'use strict';
    angular
        .module('app')
        .controller('TADARoot_CategoryController', TADARoot_CategoryController)

    TADARoot_CategoryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'superCache']
    function TADARoot_CategoryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, superCache) {
        $scope.search = "";
        $scope.obj = {};
        $scope.Cat = {};
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.itemsPerPage12 = 10;
        $scope.currentPage12 = 1;
        $scope.stateList = [];
        $scope.sortReverse = true;
        $scope.onpageload = function () {
            $scope.statenamelists = [];
            var pageid = 1;
            apiService.getURI("TADARoot_Category/getalldata", pageid).then(function (promise) {
                $scope.categorylist = promise.categorynamelists;
             
                $scope.stateList = promise.stateList;
                $scope.statenamelists = promise.statenamelists;
                $scope.getEmployeeList = promise.getEmployeeList;
                $scope.getEmployeeList1 = promise.getEmployeeList;

                if ($scope.statenamelists != null && $scope.statenamelists.length > 0) {
                    angular.forEach($scope.statenamelists, function (itm) {
                        itm.checked = false;

                    });
                }

                if ($scope.categorylist != null && $scope.categorylist.length > 0) {
                    angular.forEach($scope.categorylist, function (itm) {
                        itm.checked = false;

                    });
                }
                $scope.GeatLoadCategory = promise.geatLoadCategory;

                $scope.GetstateList = promise.getstateList;
                $scope.Cat.Min = 2;
                $scope.Cat.Max = 5;
            });

        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };
        $scope.clear1 = function () {
            $scope.onpageload();
            $scope.Categoryname = "";
        }
        $scope.clear2 = function () {
            $scope.onpageload();
            $scope.obj.HRME_Id = "";
            $scope.Cat.Percentage = "";
            $scope.Cat.Min = "";
            $scope.Cat.Max = "";
        }
        $scope.clear3 = function () {
            $scope.onpageload();
            $scope.obj.HRME_Id = "";
        }


        $scope.filterValue_S = "";
        $scope.All_S = false;

        $scope.toggleAll_E = function () {
            angular.forEach($scope.getEmployeeList, function (subj) {
                subj.checked = $scope.All_E;
            })
        };

        $scope.optiontoggled_S = function () {
            $scope.All_E = $scope.getEmployeeList.every(function (itm) { return itm.checked; });
        };

        $scope.isOptionsRequired_S = function () {
            return !$scope.getEmployeeList.some(function (options) {
                return options.checked;
            });
        };
        $scope.submitted = false;
        $scope.savecategoryddata = function () {
            if ($scope.myForm1.$valid) {

                var data = {
                    "Categoryname": $scope.Categoryname,
                   

                    //"Flag": $scope.Flag
                };
                apiService.create("TADARoot_Category/SaveCategory", data).then(function (promise) {
                    if (promise.returnval == "saved") {
                        swal("Records saved successfully");

                    }
                    else if (promise.returnval == "Notsaved") {
                        swal("Record Not saved !");

                    }
                    else if (promise.returnval == "Duplicate") {
                        swal("Record Already Exist..!");
                    }
                    else if (promise.returnval == "admin") {
                        swal("Please Contact Administror !");
                    }
                    $state.reload();
                });
            }

            else {
                $scope.submitted = true;
            }

        }
        $scope.submitted1 = false;
        $scope.SaveEmprouteCategory = function () {
            $scope.submitted1 = true;
            if ($scope.myForm2.$valid) {
                if ($scope.categorylist != null && $scope.categorylist.length > 0) {
                    $scope.Getlist = [];
                    angular.forEach($scope.categorylist, function (itm) {
                        if (itm.checked === true) {
                            $scope.Getlist.push({ categoryId: itm.categoryId, Percentage: itm.Percentage, IMECM_MinTarget: itm.Min, IMECM_MaxTarget:itm.Max })
                        }


                    });

                };
                $scope.MI_Id = 0;
                angular.forEach($scope.getEmployeeList, function (itm) {
                    if (itm.HRME_Id === $scope.obj.HRME_Id.hrmE_Id) {
                        $scope.MI_Id = itm.MI_Id
                    }

                });

                var data = {

                    "GetList": $scope.Getlist,
                    "MI_Id": $scope.MI_Id,
                    "HRME_Id": $scope.obj.HRME_Id.hrmE_Id,
                   
                };

                apiService.create("TADARoot_Category/SaveEmprouteCategory", data).then(function (promise) {
                    if (promise.returnval == "saved") {

                        swal("Records saved successfully");

                    }
                    else if (promise.returnval == "Notsaved") {
                        swal("Record Not saved !");
                    }
                    else if (promise.returnval == "admin") {
                        swal("Please Contact Administror !");
                    }
                    $scope.onpageload();
                });

            }
            else {
                $scope.submitted1 = true;

            }

        }
        $scope.submitted2 = false;
        $scope.SaveEmpregionCategory = function () {
            if ($scope.myForm122.$valid) {
                if ($scope.statenamelists != null && $scope.statenamelists.length > 0) {
                    $scope.regionList = [];
                    angular.forEach($scope.statenamelists, function (state) {
                        if (state.checked === true) {
                            $scope.regionList.push({ IVRMMS_Id: state.stateId })
                        }

                    });
                }
                $scope.MI_Id = 0;
                angular.forEach($scope.GeatLoadCategory, function (itm) {
                    if (itm.HRME_Id === $scope.obj.HRME_Id.HRME_Id) {
                        $scope.MI_Id = itm.MI_Id
                    }

                });
                if ($scope.Checked1 == true) {
                    $scope.Checked1 = 1
                }

                else {
                    $scope.Checked1 = 0

                }

                var data = {
                    "MI_Id": $scope.MI_Id,
                    "HRME_Id": $scope.obj.HRME_Id.HRME_Id,
                    "regionList": $scope.regionList,
                    "IMER_ApplicableRegion": $scope.Checked1

                };
                apiService.create("TADARoot_Category/saveRegionMapping", data).then(function (promise) {
                    if (promise.returnval == "saved") {
                        swal("Records saved successfully");
                    }
                    else if (promise.returnval == "Notsaved") {
                        swal("Record Not saved !");
                    }
                    else if (promise.returnval == "admin") {
                        swal("Please Contact Administror !");
                    }
                    $scope.onpageload();
                });
            }
            else {
                $scope.submitted2 = true;
            }

        }
        //Tab2
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            //var user = {
            //    "IMECM_id": $scope.IMECM_id,

            //};
            if (user.IMECM_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("TADARoot_Category/deactiveY/", user).
                            then(function (promise) {
                                if (promise.message != null) {
                                    swal(promise.message);
                                }
                                else {
                                    if (promise.returnval == true) {
                                        swal(confirmmgs + " " + "Successfully");
                                    }
                                    else {
                                        swal(confirmmgs + " " + " Successfully");
                                    }
                                }
                                $scope.onpageload();
                                $('#popup123').modal('hide');
                            });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }
        $scope.deactiveRegion = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.IMER_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("TADARoot_Category/deactiveRegion/", user).
                            then(function (promise) {
                                if (promise.message != null) {
                                    swal(promise.message);
                                }
                                else {
                                    if (promise.returnval == true) {
                                        swal(confirmmgs + " " + "Successfully");

                                    }
                                    else {
                                        swal(confirmmgs + " " + " Successfully");

                                    }
                                }
                                $scope.onpageload();
                                $('#popup12').modal('hide');
                            });

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }


                });


        }



        //tab1
        $scope.deactiveCat = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.imrC_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: true,
                closeOnCancel: true
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("TADARoot_Category/deactiveCat/", user).then(function (promise) {
                            if (promise.message != null) {
                                swal(promise.message);
                            }
                            else {
                                if (promise.returnval == true) {
                                    swal(confirmmgs + " " + "Successfully");

                                }
                                else {
                                    swal(confirmmgs + " " + " Successfully");

                                }
                            }
                            $scope.onpageload();

                        });

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }


                });


        }


        $scope.viewrecordspopup = function (user) {

            var data = {
                "HRME_Id": user.HRME_Id,
                //"Flag": $scope.Flag
            };

            apiService.create("TADARoot_Category/viewrecordspopup", data).then(function (promise) {

                if (promise.categorylist != null && promise.categorylist.length > 0) {
                    $scope.viewrecordspopupdisplay = promise.categorylist;
                    $('#popup123').modal('show');
                }
                else {
                    swal("Employee Category Not Found !");
                }
                $scope.onpageload();

            });
        };

        $scope.viewrecordspopupstate = function (user) {

            var data = {
                "HRME_Id": user.HRME_Id,
                //"Flag": $scope.Flag
            };
            apiService.create("TADARoot_Category/viewrecordspopup", data).then(function (promise) {

                if (promise.stateList != null && promise.stateList.length > 0) {
                    $scope.viewrecordspopupstate1 = promise.stateList;
                    $('#popup12').modal('show');
                }
                else {
                    swal("Employee state Not Found !");
                }
                $scope.onpageload();
            });
        };

        $scope.PercenatgeEdit = function () {


            if ($scope.viewrecordspopupdisplay != null && $scope.viewrecordspopupdisplay.length > 0) {
                $scope.Getlist = [];
                angular.forEach($scope.viewrecordspopupdisplay, function (itm1) {
                    if (itm1.IMECM_ActiveFlag === true) {
                        $scope.Getlist.push({
                            categoryId: itm1.IMRC_CategoryId, Percentage: itm1.IMRC_Percentage, HRME_Id: itm1.HRME_Id, IMECM_id: itm1.IMECM_id,
                            IMECM_MinTarget: itm1.IMECM_MinTarget, IMECM_MaxTarget: itm1.IMECM_MaxTarget
                        })
                    }


                });

            };
            //$scope.MI_Id = 0;
            //angular.forEach($scope.getEmployeeList, function (itm) {
            //    if (itm.HRME_Id === $scope.obj.HRME_Id.hrmE_Id) {
            //        $scope.MI_Id = itm.MI_Id
            //    }

            //});

            var data = {

                "GetList": $scope.Getlist,
                //"MI_Id": $scope.MI_Id,
                //"HRME_Id": $scope.obj.HRME_Id.hrmE_Id
            };

            apiService.create("TADARoot_Category/SaveEmprouteCategory", data).then(function (promise) {
                if (promise.returnval == "saved") {

                    swal("Records saved successfully");

                }
                else if (promise.returnval == "Notsaved") {
                    swal("Record Not saved !");
                }
                else if (promise.returnval == "admin") {
                    swal("Please Contact Administror !");
                }
                $scope.onpageload();
                $('#popup123').modal('hide');
            });



        }



    }
})();