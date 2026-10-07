
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CheckOutAssetsDCController', CheckOutAssetsDCController);
    CheckOutAssetsDCController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function CheckOutAssetsDCController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.obj = {};
        $scope.editS = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 10;
        }

        $scope.get_checkout = [];
        //===================================== VIEW TEMPLATE
        //====================== Auto Generated Number
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag === "CHECKOUTDC") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }
        //===========================

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //-------------------------------------------------------------------
        $scope.obj = {};
        $scope.invacO_CheckoutDate = new Date();
        $scope.currentPage = 1;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPage1 = paginationformasters;
        $scope.tempdc = [];
        $scope.loaddata = function () {

            $scope.search = "";
            var pageid = 2;
            apiService.getURI("CheckOutAssetsDC/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_store = promise.get_store;
                    console.log($scope.get_store);
                    $scope.get_locations = promise.get_locations;
                    $scope.instlist = promise.instlist;

                    $scope.get_checkout = promise.get_checkout;
                    $scope.get_checkoutdclist = promise.get_checkoutdclist;
                    $scope.presentCountgrid = $scope.get_checkout.length;

                    $scope.tempdc = [];
                    angular.forEach($scope.get_checkoutdclist, function (ddd) {

                        if ($scope.tempdc.length == 0) {
                            $scope.tempdc.push({ INVACO_No: ddd.invacO_No });
                        }
                        else if ($scope.tempdc.length > 0) {
                            var already_cnt = 0;
                            angular.forEach($scope.tempdc, function (opq1) {
                                if (opq1.INVACO_No == ddd.invacO_No) {
                                    already_cnt += 1;
                                }
                            })

                            if (already_cnt == 0) {
                                $scope.tempdc.push({ INVACO_No: ddd.invacO_No });
                            }

                        }
                    })

                    $scope.mainarray = [];
                    angular.forEach($scope.tempdc, function (ss) {
                        var listdat = [];
                        angular.forEach($scope.get_checkoutdclist, function (ff) {
                            if (ss.INVACO_No == ff.invacO_No) {
                                listdat.push(ff);
                            }

                        })
                        ss.LIST = listdat;
                    })


                });
        };
        //============================================ 
        $scope.reschedule = function (obj) {

            $scope.ttl = 0;
            $scope.imagename = $scope.instlist[0].mI_Logo;
            $scope.miname = $scope.instlist[0].mI_Name;
            $scope.miname = angular.uppercase($scope.miname);
            $scope.migst = $scope.instlist[0].mI_GSTNO;
            //$('#popup111').modal('show');

            $scope.printstudents = [];
            angular.forEach(obj.LIST, function (dd) {
                $scope.ttl = $scope.ttl + dd.invacO_CheckOutQty;
                $scope.printstudents.push(dd);
                $scope.dcdate = dd.updatedDate;

            })

            $scope.tempto = [];
            angular.forEach($scope.printstudents, function (ddd) {

                if ($scope.tempto.length == 0) {
                    $scope.tempto.push({ invmlO_Id: ddd.invmlO_Id, invmlO_LocationRoomName: ddd.invmlO_LocationRoomName });
                }
                else if ($scope.tempto.length > 0) {
                    var already_cnt = 0;
                    angular.forEach($scope.tempto, function (opq1) {
                        if (opq1.invmlO_Id == ddd.invmlO_Id) {
                            already_cnt += 1;
                        }
                    })

                    if (already_cnt == 0) {
                        $scope.tempto.push({ invmlO_Id: ddd.invmlO_Id, invmlO_LocationRoomName: ddd.invmlO_LocationRoomName });
                    }

                }
            })

            $scope.temptorecby = [];
            $scope.temptorecbyname = '';
            angular.forEach($scope.printstudents, function (ddd) {

                if ($scope.temptorecby.length == 0) {
                    $scope.temptorecby.push({ invacO_ReceivedBy: ddd.invacO_ReceivedBy });
                    $scope.temptorecbyname = ddd.invacO_ReceivedBy;
                }
                else if ($scope.temptorecby.length > 0) {
                    var already_cnt = 0;
                    angular.forEach($scope.temptorecby, function (opq1) {
                        if (opq1.invacO_ReceivedBy == ddd.invacO_ReceivedBy) {
                            already_cnt += 1;
                        }
                    })

                    if (already_cnt == 0) {
                        $scope.temptorecby.push({ invacO_ReceivedBy: ddd.invacO_ReceivedBy });
                        $scope.temptorecbyname = $scope.temptorecbyname + ' , ' + ddd.invacO_ReceivedBy;
                    }

                }
            })



            $scope.decno = obj.INVACO_No;


            $('#popup111').modal('show');

        }
        //===================== Get Item on store Change
        $scope.onstorechange = function () {
            $scope.id = $scope.obj.invmsT_Id.invmsT_Id;
            var data = {
                "INVMST_Id": $scope.id
            };
            apiService.create("INV_T_Sales/getitem", data).
                then(function (promise) {

                    if (promise.get_item.length > 0) {
                        $scope.get_items = promise.get_item;
                    }
                    else {
                        swal('For Selected Store, No Item found..!!');
                        $scope.get_items.length = 0;
                    }
                });
        };
        //$scope.onstorechange = function () {
        //    $scope.id = $scope.obj.invmsT_Id.invmsT_Id;
        //    var data = {
        //        "INVMST_Id": $scope.id
        //    };
        //    apiService.create("CheckOutAssetsDC/getitems", data).
        //        then(function (promise) {
        //            $scope.get_items = promise.get_items;
        //        });
        //};
        $scope.onlocationchange = function () {
            $scope.id = $scope.obj.invmlO_Id.invmlO_Id;
            var data = {
                "INVMLO_Id": $scope.id
            };
            apiService.create("CheckOutAssetsDC/getcontactperson", data).
                then(function (promise) {
                    $scope.get_contactperson = promise.get_contactperson;
                    $scope.contactflag = promise.contactflag;
                    if ($scope.contactflag === "E") {
                        $scope.hrmE_Id = $scope.get_contactperson[0].hrmE_Id;
                        $scope.employeename = $scope.get_contactperson[0].employeename;
                    }
                    else {
                        $scope.get_employee = promise.get_employee;
                        $scope.employeename = $scope.get_contactperson[0].invmlO_InchargeName;
                    }
                });
        };
        $scope.onitemchange = function () {
            var data = {};
            var avstock = 0;
            $scope.storeid = $scope.obj.invmsT_Id.invmsT_Id;
            $scope.itemid = $scope.obj.INVMI_Id.INVMI_Id;
            $scope.salerate = $scope.obj.INVMI_Id.INVSTO_SalesRate;
            data = {
                "INVMI_Id": $scope.itemid,
                "INVSTO_SalesRate": $scope.salerate,
                "INVMST_Id": $scope.storeid
            };
            apiService.create("INV_T_Sales/getitemDetail", data).
                then(function (promise) {
                    $scope.availablestock = promise.availablestock;
                    angular.forEach($scope.availablestock, function (ast) {
                        avstock += ast.AvaiableStock;
                    });
                    $scope.avstock = avstock;
                });
        };
        $scope.printstudents = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.get_checkout, function (itm) {
                itm.selected = toggleStatus;

            });
        }
        $scope.optionToggled1 = function (SelectedStudentRecord, index) {

            $scope.all = $scope.get_checkout.every(function (options) {

                return options.selected;
            });
        }
        $scope.checkStock = function () {
            var totlqty = 0;
            var availablestok = 0;
            totlqty = parseInt($scope.invacO_CheckOutQty);
            availablestok = parseInt($scope.avstock);
            if (totlqty > availablestok) {
                swal("Please Check Available Stock...!!");
                $scope.invacO_CheckOutQty = "";
            }
        };
        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("asdf").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.ttl = 0;
        $scope.generatedc = function () {
            swal({
                title: "Are you sure?",
                text: "Do You Want Generate DC??",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Generate DC",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        $scope.ttl = 0;
                        $scope.imagename = $scope.instlist[0].mI_Logo;
                        $scope.miname = $scope.instlist[0].mI_Name;
                        $scope.miname = angular.uppercase($scope.miname);
                        $scope.migst = $scope.instlist[0].mI_GSTNO;
                        $scope.printstudents = [];
                        angular.forEach($scope.get_checkout, function (dd) {
                            if (dd.selected == true) {
                                $scope.ttl = $scope.ttl + dd.invacO_CheckOutQty;
                                $scope.printstudents.push(dd);
                            }
                        })
                        $scope.tempto = [];
                        angular.forEach($scope.printstudents, function (ddd) {
                            if ($scope.tempto.length == 0) {
                                $scope.tempto.push({ invmlO_Id: ddd.invmlO_Id, invmlO_LocationRoomName: ddd.invmlO_LocationRoomName });
                            }
                            else if ($scope.tempto.length > 0) {
                                var already_cnt = 0;
                                angular.forEach($scope.tempto, function (opq1) {
                                    if (opq1.invmlO_Id == ddd.invmlO_Id) {
                                        already_cnt += 1;
                                    }
                                })

                                if (already_cnt == 0) {
                                    $scope.tempto.push({ invmlO_Id: ddd.invmlO_Id, invmlO_LocationRoomName: ddd.invmlO_LocationRoomName });
                                }

                            }
                        })
                        $scope.temptorecby = [];
                        $scope.temptorecbyname = '';
                        angular.forEach($scope.printstudents, function (ddd) {
                            if ($scope.temptorecby.length == 0) {
                                $scope.temptorecby.push({ invacO_ReceivedBy: ddd.invacO_ReceivedBy });
                                $scope.temptorecbyname = ddd.invacO_ReceivedBy;
                            }
                            else if ($scope.temptorecby.length > 0) {
                                var already_cnt = 0;
                                angular.forEach($scope.temptorecby, function (opq1) {
                                    if (opq1.invacO_ReceivedBy == ddd.invacO_ReceivedBy) {
                                        already_cnt += 1;
                                    }
                                })

                                if (already_cnt == 0) {
                                    $scope.temptorecby.push({ invacO_ReceivedBy: ddd.invacO_ReceivedBy });
                                    $scope.temptorecbyname = $scope.temptorecbyname + ' , ' + ddd.invacO_ReceivedBy;
                                }

                            }
                        })
                        if ($scope.printstudents.length == 0) {
                            swal('Select Items to generate Delivery Challan');
                        }
                        else {
                            var data = {};
                            data = {
                                selecteditems: $scope.printstudents,
                                transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                            };
                            var config = {
                                headers: {
                                    'Content-Type': 'application/json;'
                                }
                            };
                            apiService.create("CheckOutAssetsDC/savedetails", data).then(function (promise) {

                                if (promise.returnval === true) {
                                    $scope.decno = promise.trans_id;
                                    $scope.dcdate = promise.updatedDate;
                                    $scope.get_checkout = promise.get_checkout;
                                    $scope.get_checkoutdclist = promise.get_checkoutdclist;
                                    $scope.tempdc = [];
                                    angular.forEach($scope.get_checkoutdclist, function (ddd) {
                                        if ($scope.tempdc.length == 0) {
                                            $scope.tempdc.push({ INVACO_No: ddd.invacO_No });
                                        }
                                        else if ($scope.tempdc.length > 0) {
                                            var already_cnt = 0;
                                            angular.forEach($scope.tempdc, function (opq1) {
                                                if (opq1.INVACO_No == ddd.invacO_No) {
                                                    already_cnt += 1;
                                                }
                                            })

                                            if (already_cnt == 0) {
                                                $scope.tempdc.push({ INVACO_No: ddd.invacO_No });
                                            }
                                        }
                                    })
                                    $scope.mainarray = [];
                                    angular.forEach($scope.tempdc, function (ss) {
                                        var listdat = [];
                                        angular.forEach($scope.get_checkoutdclist, function (ff) {
                                            if (ss.INVACO_No == ff.invacO_No) {
                                                listdat.push(ff);
                                            }

                                        })
                                        ss.LIST = listdat;
                                    })
                                    $('#popup111').modal('show');
                                    swal("CheckOut DC Generated Successfully!!! !");

                                }
                                else {
                                    swal('Error in CheckOut  DC generation');
                                }

                            });
                        }

                    }
                    else {
                        swal("Check Out DC generation Cancelled !");
                    }
                }
            );
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.edit = function (item, salerate) {
            $scope.editS = true;
            $scope.invmsT_Id = item.invmsT_Id;
            $scope.invmS_StoreName = item.invmS_StoreName;
            $scope.invmI_Id = item.invmI_Id;
            $scope.invmI_ItemName = item.invmI_ItemName;
            $scope.invstO_SalesRate = item.invstO_SalesRate;
            $scope.obj.invmlO_Id = item;
            $scope.contactflag = "E";
            $scope.hrmE_Id = item.hrmE_Id;
            $scope.employeename = item.invacO_ReceivedBy;
            $scope.invacO_CheckoutDate = new Date(item.invacO_CheckoutDate);
            $scope.invacO_CheckOutQty = item.invacO_CheckOutQty;
            $scope.invacO_CheckOutRemarks = item.invacO_CheckOutRemarks;
            $scope.invacO_Id = item.invacO_Id;
            var avstock = 0;
            var data = {
                "INVMI_Id": $scope.invmI_Id,
                "INVSTO_SalesRate": $scope.invstO_SalesRate,
                "INVMST_Id": $scope.invmsT_Id
            };
            apiService.create("INV_T_Sales/getitemDetail", data).
                then(function (promise) {
                    $scope.availablestock = promise.availablestock;
                    angular.forEach($scope.availablestock, function (ast) {
                        avstock += ast.AvaiableStock;
                    });
                    $scope.avstock = avstock;
                });
            $scope.onitemchange();
        };

        $scope.deactive = function (item, SweetAlert) {
            $scope.INVACO_Id = item.invacO_Id;
            var dystring = "";
            if (item.invacO_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invacO_ActiveFlg === false) {
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
                        apiService.create("CheckOutAssetsDC/deactive", item).
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

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
        $scope.searchValue1 = '';
    }
})();