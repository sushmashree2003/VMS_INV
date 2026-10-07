
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CheckInAssetsDCController', CheckInAssetsDCController);
    CheckInAssetsDCController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function CheckInAssetsDCController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 10;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //========================================== PAGE LOAD
        $scope.obj = {};
        $scope.invacI_CheckInDate = new Date();



        $scope.printstudents = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.get_checkin, function (itm) {
                itm.selected = toggleStatus;

            });
        }


        $scope.optionToggled1 = function (SelectedStudentRecord, index) {

            $scope.all = $scope.get_checkin.every(function (options) {

                return options.selected;
            });
        }
        //===================================== VIEW TEMPLATE
        //====================== Auto Generated Number
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        if (transnumconfigsettings != null && transnumconfigsettings.length > 0) {
            for (var i = 0; i < transnumconfigsettings.length; i++) {
                if (transnumconfigsettings.length > 0) {
                    if (transnumconfigsettings[i].imN_Flag === "CHECKOUTDC") {
                        $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                    }
                }
            }

        }
        //===========================
        $scope.reschedule = function (obj) {

            $scope.ttl = 0;
            $scope.imagename = $scope.instlist[0].mI_Logo;
            $scope.miname = $scope.instlist[0].mI_Name;
            $scope.migst = $scope.instlist[0].mI_GSTNO;
            //$('#popup111').modal('show');

            $scope.printstudents = [];
            angular.forEach(obj.LIST, function (dd) {
                $scope.ttl = $scope.ttl + dd.invacI_CheckInQty;
                $scope.printstudents.push(dd);
                $scope.dcdate = dd.updatedDate;

            })

            $scope.tempto = [];
            angular.forEach($scope.printstudents, function (ddd) {

                if ($scope.tempto.length == 0) {
                    $scope.tempto.push({ invmsT_Id: ddd.invmsT_Id, invmS_StoreName: ddd.invmS_StoreName });
                }
                else if ($scope.tempto.length > 0) {
                    var already_cnt = 0;
                    angular.forEach($scope.tempto, function (opq1) {
                        if (opq1.invmsT_Id == ddd.invmsT_Id) {
                            already_cnt += 1;
                        }
                    })
                    if (already_cnt == 0) {
                        $scope.tempto.push({ invmsT_Id: ddd.invmsT_Id, invmS_StoreName: ddd.invmS_StoreName });
                    }
                }
            })
            $scope.temptorecby = [];
            $scope.temptorecbyname = '';
            angular.forEach($scope.printstudents, function (ddd) {
                if ($scope.temptorecby.length == 0) {
                    $scope.temptorecby.push({ invacI_ReceivedBy: ddd.invacI_ReceivedBy });
                    $scope.temptorecbyname = ddd.invacI_ReceivedBy;
                }
                else if ($scope.temptorecby.length > 0) {
                    var already_cnt = 0;
                    angular.forEach($scope.temptorecby, function (opq1) {
                        if (opq1.invacI_ReceivedBy == ddd.invacI_ReceivedBy) {
                            already_cnt += 1;
                        }
                    })
                    if (already_cnt == 0) {
                        $scope.temptorecby.push({ invacI_ReceivedBy: ddd.invacI_ReceivedBy });
                        $scope.temptorecbyname = $scope.temptorecbyname + ' , ' + ddd.invacI_ReceivedBy;
                    }
                }
            })
            $scope.decno = obj.INVACI_No;
            $('#popup111').modal('show');
        }
        //=============================
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = paginationformasters;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.loaddata = function () {
           
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("CheckInAssetsDC/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_locations = promise.get_locations;
                    $scope.get_employee = promise.get_employee;
                    $scope.get_checkin = promise.get_checkin;
                    $scope.get_checkindc = promise.get_checkindc;
                    $scope.instlist = promise.instlist;
                    $scope.presentCountgrid = $scope.get_checkin.length;



                    $scope.tempdc = [];
                    angular.forEach($scope.get_checkindc, function (ddd) {

                        if ($scope.tempdc.length == 0) {
                            $scope.tempdc.push({ INVACI_No: ddd.invacI_No });
                        }
                        else if ($scope.tempdc.length > 0) {
                            var already_cnt = 0;
                            angular.forEach($scope.tempdc, function (opq1) {
                                if (opq1.INVACI_No == ddd.invacI_No) {
                                    already_cnt += 1;
                                }
                            })

                            if (already_cnt == 0) {
                                $scope.tempdc.push({ INVACI_No: ddd.invacI_No });
                            }

                        }
                    })

                    $scope.mainarray = [];
                    angular.forEach($scope.tempdc, function (ss) {
                        var listdat = [];
                        angular.forEach($scope.get_checkindc, function (ff) {
                            if (ss.INVACI_No == ff.invacI_No) {
                                listdat.push(ff);
                            }

                        })
                        ss.LIST = listdat;
                    })
                });
        };
        //============================================ 
        $scope.onlocationchange = function () {
            $scope.id = $scope.obj.invmlO_Id.invmlO_Id;
            var data = {
                "INVMLO_Id": $scope.id
            };
            apiService.create("CheckInAssetsDC/getStore", data).
                then(function (promise) {
                    $scope.get_store = promise.get_store;
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
        $scope.onstorechange = function () {
            $scope.storeid = $scope.obj.invmsT_Id.invmsT_Id;
            $scope.locationid = $scope.obj.invmlO_Id.invmlO_Id;
            var data = {
                "INVMST_Id": $scope.storeid,
                "INVMLO_Id": $scope.locationid
            };
            apiService.create("CheckInAssetsDC/getitems", data).
                then(function (promise) {
                    $scope.get_items = promise.get_items;
                    // $scope.checkoutqty = $scope.get_items[0].invacI_CheckInQty;
                });
        };
        $scope.onitemchange = function () {
            var avqty = 0;
            $scope.storeid = $scope.obj.invmsT_Id.invmsT_Id;
            $scope.locationid = $scope.obj.invmlO_Id.invmlO_Id;
            $scope.itemid = $scope.obj.invmI_Id.invmI_Id;
            $scope.salerate = $scope.obj.invmI_Id.invstO_SalesRate;
            var data = {
                "INVMST_Id": $scope.storeid,
                "INVMLO_Id": $scope.locationid,
                "INVMI_Id": $scope.itemid,
                "INVSTO_SalesRate": $scope.obj.invmI_Id.invstO_SalesRate
            };
            apiService.create("CheckInAssetsDC/getdetails", data).
                then(function (promise) {
                    $scope.get_details = promise.get_details;
                    angular.forEach($scope.get_details, function (ast) {
                        avqty += ast.invstO_AvaiableStock;
                    });
                    $scope.checkoutqty = avqty;
                });
        };
        $scope.checkStock = function () {
            var totlqty = 0;
            var checkoutqty = 0;
            totlqty = parseInt($scope.invacI_CheckInQty);
            checkoutqty = parseInt($scope.checkoutqty);
            if (totlqty > checkoutqty) {
                swal("Please Check Check-out Quantity...!!");
                $scope.invacI_CheckInQty = "";
            }
        };       
        $scope.generatedc = function (SweetAlert) {
            swal({
                title: "Are you sure?",
                text: "Do You Want To Generate DC?",
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
                        $scope.migst = $scope.instlist[0].mI_GSTNO;
                        $scope.printstudents = [];
                        angular.forEach($scope.get_checkin, function (dd) {
                            if (dd.selected == true) {
                                $scope.ttl = $scope.ttl + dd.invacI_CheckInQty;
                                $scope.printstudents.push(dd);
                            }

                        })
                        $scope.tempto = [];
                        angular.forEach($scope.printstudents, function (ddd) {
                            if ($scope.tempto.length == 0) {
                                $scope.tempto.push({ invmsT_Id: ddd.invmsT_Id, invmS_StoreName: ddd.invmS_StoreName });
                            }
                            else if ($scope.tempto.length > 0) {
                                var already_cnt = 0;
                                angular.forEach($scope.tempto, function (opq1) {
                                    if (opq1.invmsT_Id == ddd.invmsT_Id) {
                                        already_cnt += 1;
                                    }
                                })

                                if (already_cnt == 0) {
                                    $scope.tempto.push({ invmsT_Id: ddd.invmsT_Id, invmS_StoreName: ddd.invmS_StoreName });
                                }
                            }
                        })
                        $scope.temptorecby = [];
                        $scope.temptorecbyname = '';
                        angular.forEach($scope.printstudents, function (ddd) {

                            if ($scope.temptorecby.length == 0) {
                                $scope.temptorecby.push({ invacI_ReceivedBy: ddd.invacI_ReceivedBy });
                                $scope.temptorecbyname = ddd.invacI_ReceivedBy;
                            }
                            else if ($scope.temptorecby.length > 0) {
                                var already_cnt = 0;
                                angular.forEach($scope.temptorecby, function (opq1) {
                                    if (opq1.invacI_ReceivedBy == ddd.invacI_ReceivedBy) {
                                        already_cnt += 1;
                                    }
                                })

                                if (already_cnt == 0) {
                                    $scope.temptorecby.push({ invacI_ReceivedBy: ddd.invacI_ReceivedBy });
                                    $scope.temptorecbyname = $scope.temptorecbyname + ' , ' + ddd.invacI_ReceivedBy;
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
                            var config = {
                                headers: {
                                    'Content-Type': 'application/json;'
                                }
                            };
                            apiService.create("CheckInAssetsDC/savedetails", data).then(function (promise) {
                                if (promise.returnval === true) {
                                    $scope.decno = promise.trans_id;
                                    $scope.dcdate = promise.updatedDate;
                                    $scope.get_checkin = promise.get_checkin;
                                    $scope.get_checkindc = promise.get_checkindc;
                                    $scope.tempdc = [];
                                    angular.forEach($scope.get_checkindc, function (ddd) {
                                        if ($scope.tempdc.length == 0) {
                                            $scope.tempdc.push({ INVACI_No: ddd.invacI_No });
                                        }
                                        else if ($scope.tempdc.length > 0) {
                                            var already_cnt = 0;
                                            angular.forEach($scope.tempdc, function (opq1) {
                                                if (opq1.INVACI_No == ddd.invacI_No) {
                                                    already_cnt += 1;
                                                }
                                            })
                                            if (already_cnt == 0) {
                                                $scope.tempdc.push({ INVACI_No: ddd.invacI_No });
                                            }
                                        }
                                    })
                                    $scope.mainarray = [];
                                    angular.forEach($scope.tempdc, function (ss) {
                                        var listdat = [];
                                        angular.forEach($scope.get_checkindc, function (ff) {
                                            if (ss.INVACI_No == ff.invacI_No) {
                                                listdat.push(ff);
                                            }

                                        })
                                        ss.LIST = listdat;
                                    })

                                    $('#popup111').modal('show');
                                    swal(" CheckIn DC Generated Successfully!!! !");
                                }
                                else {

                                }

                            });
                        }
                    }
                    else {
                        swal("CheckIn DC generation Cancelled !");
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



        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("asdf").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.deactive = function (item, SweetAlert) {
            $scope.INVACI_Id = item.invacI_Id;
            var dystring = "";
            if (item.invacI_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invacI_ActiveFlg === false) {
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
                        apiService.create("CheckInAssetsDC/deactive", item).
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

        //$scope.edit = function (item) {
        //    obj.invmsT_Id = item.invmsT_Id;
        //    $scope.obj.invmI_Id = item.invmI_Id;
        //    $scope.obj.invmlO_Id = item.invmlO_Id;
        //    $scope.obj.invmS_StoreName = item.invmS_StoreName;
        //    $scope.obj.invmI_ItemName = item.invmI_ItemName;
        //    $scope.obj.invmlO_LocationRoomName = item.invmlO_LocationRoomName;
        //    $scope.invacI_CheckInDate = item.invacI_CheckInDate;
        //    $scope.invacI_CheckInQty = item.invacI_CheckInQty;
        //    $scope.invacI_ReceivedBy = item.invacI_ReceivedBy;
        //    $scope.obj.hrmE_Id = item.hrmE_Id;
        //    $scope.invacO_CheckOutRemarks = item.invacO_CheckOutRemarks;
        //    $scope.invacO_Id = item.invacO_Id;

        //}
        $scope.edit = function (item) {
            $scope.editS = true;
            $scope.obj.invmsT_Id = item;
            $scope.obj.invmI_Id = item;
            $scope.obj.invmlO_Id = item;
            $scope.hrmE_Id = item.hrmE_Id;
            $scope.employeename = item.invacI_ReceivedBy;
            $scope.invacI_CheckInDate = new Date(item.invacI_CheckInDate);
            $scope.invacI_CheckInQty = item.invacI_CheckInQty;
            $scope.invacI_CheckInRemarks = item.invacI_CheckInRemarks;
            $scope.invacI_Id = item.invacI_Id;
            $scope.onitemchange();
        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
        $scope.searchValue1 = '';
    }
})();