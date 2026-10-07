(function () {
    'use strict';
    angular
        .module('app')
        .controller('Reason_MasterController', reason_MasterController)
    reason_MasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$stateParams', 'Excel', '$timeout', '$q']
    function reason_MasterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $stateParams, Excel, $timeout, $q) {
        //====================================================================================
        $scope.sortKey = 'ismresgmrE_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.obj = {};
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        //===================Load data=======================
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("Exit_Employee/Get_Reason", pageid).then(function (promise) {
                $scope.company_list_dd = promise.company_list_dd;
                $scope.get_reason_list = promise.get_reason_list;
                $scope.presentCountgrid = $scope.get_reason_list.length;
            });
        }
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.all_checkS = function (ck) {
            $scope.checkallS = ck;
            var toggleStatus = $scope.checkallS;
            angular.forEach($scope.company_list_dd, function (st) {
                st.statusck = toggleStatus;
            });
        };
        $scope.togchkbxS = function () {
            $scope.checkallS = $scope.company_list_dd.every(function (options) {
                return options.statusck;
            });
        };
        $scope.isOptionsRequiredS = function () {
            return !$scope.company_list_dd.some(function (options) {
                return options.statusck;
            });
        };


        //=============Save/Edit========================
        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.submitted = true;
            $scope.ActivitynameArray = [];
            var ISMRESGMRE_Id = 0;
            if ($scope.obj.ismresgmrE_Id > 0) {
                ISMRESGMRE_Id = $scope.obj.ismresgmrE_Id;
                angular.forEach($scope.company_list_dd, function (role) {
                    if (role.statusck == true) {

                        $scope.ActivitynameArray.push({
                            MI_Id: role.mI_Id,
                            ISMRESGMRE_Id: role.ismresgmrE_Id,
                        });
                    }
                })
            }
            else {
                angular.forEach($scope.company_list_dd, function (role) {
                    if (role.statusck == true) {
                        $scope.ActivitynameArray.push({ MI_Id: role.mI_Id });
                    }
                })

            }
            $scope.notice1 = "";
            if ($scope.notice != undefined && $scope.notice.length > 0) {
                $scope.notice1 = $scope.notice[0];
            }
            else if ($scope.id > 0) {
                $scope.notice1 = $scope.noticeedit;
            }

            if ($scope.myForm.$valid) {
                var data = {
                    "ISMRESGMRE_Id": ISMRESGMRE_Id,
                    "ISMRESGMRE_ResignationReasons": $scope.ismresgmrE_ResignationReasons,
                    "ISMRESGMRE_Flg": $scope.ISMRESGMRE_Flg,
                    "ISMRESGMRE_NoticePeriodDays": $scope.obj.noticeperiod,
                    "ISMRESGMRE_Template": $scope.notice1,
                    "ActivitynameArray": $scope.ActivitynameArray
                };
                apiService.create("Exit_Employee/Save_Edit_Reason", data).then(function (promise) {
                    if (promise.returnval === "Add") {
                        swal('Record Saved  Successfully , Save Count ' + promise.savecount + ' , Duplicate Count ' + promise.duplicatecount + '');
                        $state.reload();
                    }

                    else if (promise.returnval === "Notadd") {
                        swal('Record Already Exist , Duplicate Count  ' + promise.duplicatecount + '');

                    }
                    else {
                        swal('Operation Failed');
                    }

                });
            }
            else {

                $scope.submitted = true;
            }
        }

        //============================getdetailsReasonmaster================================
        $scope.edit = {};
        $scope.Edit = function (ss) {

            var pageid = ss.ismresgmrE_Id
            apiService.getURI("Exit_Employee/get_details_reason", pageid).then(function (promise) {

                $scope.ismresgmrE_ResignationReasons = promise.get_details_reason_list[0].ismresgmrE_ResignationReasons;
                $scope.id = promise.get_details_reason_list[0].ismresgmrE_Id
                $scope.noticeedit = promise.get_details_reason_list[0].ismresgmrE_Template
                $scope.ISMRESGMRE_Flg = promise.get_details_reason_list[0].ismresgmrE_Flg
                $scope.file_detail = promise.get_details_reason_list[0].ismresgmrE_Template
                $scope.obj.noticeperiod = promise.get_details_reason_list[0].ismresgmrE_NoticePeriodDays
                $scope.obj.ismresgmrE_Id = promise.get_details_reason_list[0].ismresgmrE_Id

                $scope.ActivitynameArray = [];
                if ($scope.get_reason_list != null && $scope.get_reason_list.length > 0) {
                    angular.forEach($scope.get_reason_list, function (role) {

                        angular.forEach($scope.company_list_dd, function (ggg) {

                            if (role.mI_Id === ggg.mI_Id && role.ismresgmrE_ResignationReasons === promise.get_details_reason_list[0].ismresgmrE_ResignationReasons) {
                                ggg.statusck = true;
                                ggg.ismresgmrE_Id = role.ismresgmrE_Id;
                            }
                        })
                    })
                }
            })
        }

        $scope.deactive = function (flr, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (flr.ismresgmrE_ActiveFlag === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Exit_Employee/active_deactive_reason", flr).
                            then(function (promise) {

                                if (promise.returnval === "false") {
                                    swal('Master Reason Deactivated Successfully');
                                }

                                else if (promise.returnval === "true") {
                                    swal('Master Reason Activated Successfully');
                                }
                                else {
                                    swal("Cancelled");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Cancelled");
                        $state.reload();
                    }
                });
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

    }
})();
