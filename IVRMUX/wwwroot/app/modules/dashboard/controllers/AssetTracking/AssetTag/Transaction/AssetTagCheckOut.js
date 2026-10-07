
(function () {
    'use strict';
    angular
        .module('app')
        .controller('AssetTagCheckOutController', AssetTagCheckOutController);
    AssetTagCheckOutController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function AssetTagCheckOutController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.obj = {};
        var date = new Date();
        $scope.invatcO_CheckoutDate = date;
        var paginationformasters;
        $scope.transflag = false;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //========================================== PAGE LOAD
        $scope.instlist = [];
        $scope.commonlocation = false;
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage = 10;
            $scope.itemsPerPage1 = 10;
            $scope.search = "";
            $scope.commonlocation = false;
            var pageid = 2;
            apiService.getURI("AssetTagCheckOut/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_store = promise.get_store;
                    $scope.get_employee = promise.get_employee;
                    $scope.get_locations = promise.get_locations;
                    $scope.get_ATcheckout = promise.get_ATcheckout;
                    $scope.transflag1 = false;
                });
        };
        //========================================= Drop-downs  CHANGE
        $scope.onstorechange = function () {
            $scope.storeid = "";
            $scope.obj.invmI_Id = "";
            $scope.obj.invmlO_Id = "";
            $scope.commonlocation = false;
            $scope.storeid = $scope.obj.invmsT_Id.invmsT_Id;
            var data = {
                "INVMST_Id": $scope.storeid
            };
            apiService.create("AssetTagCheckOut/getitems", data).
                then(function (promise) {
                    if (promise.get_items.length > 0) {
                        $scope.get_items = promise.get_items;
                    }
                    else {
                        swal("No Record Found.... !!");
                        $scope.get_items = "";
                        $scope.obj.invmI_Id = "";
                        $scope.obj.invmlO_Id = "";
                    }
                });
        };

        $scope.onitemchange = function () {

            $scope.storeid = "";
            $scope.itemid = "";
            $scope.obj.invmlO_Id = ""; $scope.invstO_Id = "";
            $scope.commonlocation = false;
            $scope.storeid = $scope.obj.invmsT_Id.invmsT_Id;
            $scope.itemid = $scope.obj.invmI_Id.invmI_Id;
            $scope.salerate = $scope.obj.invmI_Id.invstO_SalesRate;
            $scope.invstO_Id = $scope.obj.invmI_Id.invstO_Id;
            var data = {
                "INVMST_Id": $scope.storeid,
                "INVMI_Id": $scope.itemid,
                "INVSTO_SalesRate": $scope.salerate,
                "INVATCO_Id": $scope.invstO_Id

            };
            apiService.create("AssetTagCheckOut/getitemtagdata", data).
                then(function (promise) {
                    if (promise.get_itemtagdata.length > 0) {
                        $scope.transflag = true;
                        $scope.transflag12 = true;
                        $scope.get_itemtagdata = promise.get_itemtagdata;
                        $scope.presentCountgrid = $scope.get_itemtagdata.length;
                        console.log($scope.get_itemtagdata);
                       
                    }
                    else {
                        swal("No Record Found.... !!");
                        $scope.get_itemtagdata = [];
                        $scope.transflag = false;
                        $scope.obj.invmlO_Id = "";
                        $scope.transflag1 = false;


                    }
                });
        };

        $scope.onlocationchange = function () {
            $scope.commonlocation = false;
            var loc = null;
            if ($scope.obj && $scope.obj.invmlO_Id) {
                if (typeof $scope.obj.invmlO_Id === 'object' && $scope.obj.invmlO_Id !== null) {
                    loc = $scope.obj.invmlO_Id;
                } else {
                    var targetId = $scope.obj.invmlO_Id;
                    angular.forEach($scope.get_locations, function (item) {
                        if (item.invmlO_Id == targetId || item.INVMLO_Id == targetId) {
                            loc = item;
                        }
                    });
                }
            }

            var locId = loc ? (loc.invmlO_Id || loc.INVMLO_Id) : null;
            if (locId && $scope.get_locations) {
                angular.forEach($scope.get_locations, function (item) {
                    if (item.invmlO_Id == locId || item.INVMLO_Id == locId) {
                        loc = angular.extend({}, item, loc);
                    }
                });
            }

            if (loc) {
                var remarks = ((loc.invmlO_LocationRemarks || loc.INVMLO_LocationRemarks) || '').toString().trim().toLowerCase();
                var incharge = ((loc.invmlO_InchargeName || loc.INVMLO_InchargeName) || '').toString().trim().toLowerCase();
                var roomName = ((loc.invmlO_LocationRoomName || loc.INVMLO_LocationRoomName) || '').toString().trim().toLowerCase();

                if (remarks.indexOf('common') !== -1 || incharge.indexOf('common') !== -1 || roomName.indexOf('common') !== -1) {
                    $scope.commonlocation = true;
                    angular.forEach($scope.get_itemtagdata, function (item) {
                        item.hrmE_Id = "";
                    });
                }
            }
        };
        //======================================= Grid Check box Selection
        $scope.toggleAll = function () {
            angular.forEach($scope.get_itemtagdata, function (subj) {
                subj.xyz = $scope.all;
            });
        };
        $scope.optionToggled = function () {
            $scope.all = $scope.get_itemtagdata.every(function (itm) { return itm.xyz; });
        };
        //=====================================  SAVE DATA
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.tagckoutArray = [];
                var qty = 1.00;
                angular.forEach($scope.get_itemtagdata, function (itag) {
                    if (itag.xyz) {
                        var hrmeId = null;
                        if (!$scope.commonlocation) {
                            hrmeId = itag.hrmE_Id ? itag.hrmE_Id : ($scope.empHRME_Id ? $scope.empHRME_Id : null);
                        }
                        $scope.tagckoutArray.push({
                            invaaT_Id: itag.INVAAT_Id, invatcO_CheckOutRemarks: itag.invatcO_CheckOutRemarks, HRME_Id: hrmeId, INVSTO_SalesRate: itag.INVSTO_SalesRate
                        });
                    }
                });
                if ($scope.tagckoutArray.length > 0) {
                    var data = {
                        "INVATCO_Id": $scope.invatcO_Id,
                        "INVMST_Id": $scope.obj.invmsT_Id.invmsT_Id,
                        "INVMI_Id": $scope.obj.invmI_Id.invmI_Id,
                        "INVMLO_Id": $scope.obj.invmlO_Id.invmlO_Id,
                        "INVATCO_CheckoutDate": $scope.invatcO_CheckoutDate,
                        "INVATCO_CheckOutQty": qty,
                        "tagckoutArray": $scope.tagckoutArray,
                        "INVSTO_Id": $scope.obj.invmI_Id.invstO_Id
                    };
                }
                else {
                    swal("Select Atleast One checkbox....!!");
                }

                apiService.create("AssetTagCheckOut/savedata", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.invatcO_Id === 0 || promise.invatcO_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invatcO_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invatcO_Id === 0 || promise.invatcO_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invatcO_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };
        ////====================== emp_change
        //$scope.get_emp_change = function () {
        //    $scope.get_employee12 = [];
        //    angular.forEach($scope.get_itemtagdata, function (itag1) {
        //        if (itag1.xyz) {
        //            angular.forEach($scope.get_employee, function (ff) {
        //                if (itag1.hrmE_Id != ff.hrmE_Id) {

        //                    $scope.get_employee12.push(ff);
        //                }
        //            })
        //        }
        //    });

        //    $scope.get_employee = $scope.get_employee12;
        //}

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };
        $scope.deactive = function (item) {
            $scope.INVATCO_Id = item.invatcO_Id;
            var dystring = "";
            if (item.invatcO_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invatcO_ActiveFlg === false) {
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
                        apiService.create("AssetTagCheckOut/deactive", item).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.edit = function (item) {
            $scope.obj.invmsT_Id = item;
            $scope.obj.invmI_Id = item;
            $scope.obj.invmlO_Id = item;
            $scope.invatcO_CheckoutDate = new Date(item.invatcO_CheckoutDate);
            $scope.edits = true;
            $scope.onlocationchange();
            $scope.get_itemtagdata = [];
            $scope.taglist = [];
            $scope.taglist.push(item);
            angular.forEach($scope.taglist, function (ltag) {
                $scope.get_itemtagdata.push({
                    INVAAT_Id: ltag.invaaT_Id, INVAAT_AssetId: ltag.invaaT_AssetId, INVMI_Id: ltag.invmI_Id,
                    INVMI_ItemName: ltag.invmI_ItemName,
                    INVAAT_AssetDescription: ltag.invaaT_AssetDescription,
                    INVAAT_ModelNo: ltag.invaaT_ModelNo, INVAAT_SerialNo: ltag.invaaT_SerialNo,
                    // hrmE_Id: ltag.hrmE_Id, invatcO_ReceivedBy: ltag.invatcO_ReceivedBy,
                    invatcO_CheckOutRemarks: ltag.invatcO_CheckOutRemarks, xyz: true
                });

                angular.forEach($scope.get_employee, function (emp) {
                    if (ltag.hrmE_Id === emp.hrmE_Id) {
                        $scope.obj.hrmE_Id = ltag.hrmE_Id;
                        $scope.obj.invatcO_ReceivedBy = ltag.employeename;
                    }
                });
            });

        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
        $scope.searchValue1 = '';


    }
})();