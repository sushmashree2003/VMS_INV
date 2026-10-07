
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CheckOutAssetsNewController', CheckOutAssetsController);
    CheckOutAssetsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function CheckOutAssetsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.obj = {};
        $scope.editS = false;
        $scope.searchValue1 = '';
        $scope.obj.INVACO_Time = "";

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.itemsPerPage1 = paginationformasters;
        $scope.currentPage1 = 1;
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //-------------------------------------------------------------------
        $scope.obj = {};
        $scope.obj.returnval = false;
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        $scope.obj.INVACO_No = "";
        $scope.searchValue = "";
        $scope.searchValuetwo = "";
        $scope.materaldocuupload = [{ id: 'materal' }];
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("CheckBankBalance/loaddata", pageid).then(function (promise) {
                $scope.allcompany = promise.allcompany;
                $scope.obj.MI_Id = "";
                // $scope.obj.MI_Id = promise.allcompany[0].mI_Id;
                // $scope.onstorechange();
                angular.forEach($scope.materaldocuupload, function (itm) {
                    itm.INVACO_CheckoutDate = new Date();
                });
            });
        };

        $scope.onstorechange = function () {
            $scope.transnumbconfigurationsettingsassign = {};
            $scope.get_items = [];
            $scope.get_employee = [];
            $scope.materaldocuupload = [];
            $scope.get_checkoutdclist = [];
            $scope.instlist = [];
            $scope.tempdc = [];
            $scope.materaldocuupload = [{ id: 'materal' }];
            var data = {
                "MI_Id": $scope.obj.MI_Id
            };
            apiService.create("INV_T_Sales/GetDcItems", data).then(function (promise) {
                if (promise.get_item != null && promise.get_item.length > 0) {
                    $scope.get_items = promise.get_item;
                    $scope.get_employee = promise.get_employee;
                    $scope.get_checkoutdclist = promise.get_checkoutdclist;
                }
                if (promise.get_checkoutdclist != null && promise.get_checkoutdclist.length > 0) {
                    $scope.get_checkoutdclist = promise.get_checkoutdclist;
                    $scope.instlist = promise.get_mi_list;

                    angular.forEach($scope.get_checkoutdclist, function (ddd) {
                        if ($scope.tempdc.length == 0) {
                            $scope.tempdc.push({
                                INVACO_No: ddd.invacO_No,
                                invacO_Tolocation: ddd.invacO_Tolocation,
                                invacO_CheckoutDate: new Date(ddd.invacO_CheckoutDate),
                                invacO_ReceivedBy: ddd.invacO_ReceivedBy,
                                INVACO_CheckOutRemarks: ddd.invacO_CheckOutRemarks,
                                INVACO_MODE: ddd.invacO_MODE,
                                INVACO_Vehicalno: ddd.invacO_Vehicalno,
                                INVACO_Time: ddd.invacO_Time,
                                INVACO_ISSUED: ddd.invacO_ISSUED,
                                invmlO_Status: ddd.invmlO_Status,
                                invacO_CallCenterRemarks: ddd.invacO_CallCenterRemarks,
                            });
                        }
                        else if ($scope.tempdc.length > 0) {
                            var already_cnt = 0;
                            angular.forEach($scope.tempdc, function (opq1) {
                                if (opq1.INVACO_No == ddd.invacO_No) {
                                    already_cnt += 1;
                                }
                            })
                            if (already_cnt == 0) {
                                $scope.tempdc.push({
                                    INVACO_No: ddd.invacO_No,
                                    invacO_ReceivedBy: ddd.invacO_ReceivedBy,
                                    invacO_Tolocation: ddd.invacO_Tolocation,
                                    invacO_CheckoutDate: new Date(ddd.invacO_CheckoutDate),
                                    INVACO_CheckOutRemarks: ddd.invacO_CheckOutRemarks,
                                    INVACO_MODE: ddd.invacO_MODE,
                                    INVACO_ISSUED: ddd.invacO_ISSUED,
                                    INVACO_Vehicalno: ddd.invacO_Vehicalno,
                                    INVACO_Time: ddd.invacO_Time,
                                    invmlO_Status: ddd.invmlO_Status,
                                    invacO_CallCenterRemarks: ddd.invacO_CallCenterRemarks,
                                });
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
                }
                if (promise.get_editmember != null && promise.get_editmember.length > 0) {
                    $scope.transnumbconfigurationsettingsassign = promise.get_editmember[0];
                }

            });
        };
        //$scope.OnItemchange = function (materal) {
        //    angular.forEach($scope.get_items, function (itm) {
        //        if (materal.INVMI_Id.INVMI_Id == itm.INVMI_Id) {

        //            materal.INVMS_StoreName = itm.INVMS_StoreName;
        //            materal.INVSTO_PurchaseDate = new Date(itm.INVSTO_PurchaseDate);
        //            materal.INVSTO_SalesRate = itm.INVSTO_SalesRate;
        //            materal.INVSTO_AvaiableStock = itm.INVSTO_AvaiableStock;
        //            materal.INVMI_Id_new = itm.INVMI_Id;
        //            materal.INVMST_Id = itm.INVMST_Id;
        //            materal.INVACO_CheckoutDate = new Date();
        //        }

        //    });

        //}
        $scope.OnItemchange = function (materal) {
            angular.forEach($scope.get_items, function (itm) {
                // if (materal.INVMI_Id.INVMI_Id == itm.INVMI_Id) {
                if (materal.INVMI_Id.INVSTO_Id == itm.INVSTO_Id) {
                    materal.INVSTO_AvaiableStock = "";
                    materal.INVMI_Id_new = "";
                    materal.INVMS_StoreName = "";
                    materal.INVMS_StoreName = itm.INVMS_StoreName;
                    materal.INVSTO_PurchaseDate = new Date(itm.INVSTO_PurchaseDate);
                    materal.INVSTO_SalesRate = itm.INVSTO_SalesRate;
                    materal.INVSTO_AvaiableStock = itm.INVSTO_AvaiableStock;
                    materal.INVMI_Id_new = itm.INVMI_Id;
                    materal.INVMST_Id = itm.INVMST_Id;
                    materal.INVACO_CheckoutDate = new Date();
                    return;
                }

            });

        }
        $scope.addgrnrows = function (materal) {
            if (materal.INVACO_CheckOutQty > 0 && materal.INVMI_Id_new > 0) {
                var totlqty = 0;
                var availablestok = 0;
                totlqty = parseInt(materal.INVACO_CheckOutQty);
                availablestok = parseInt(materal.INVSTO_AvaiableStock);
                if (totlqty > availablestok) {
                    swal("Please Check Available Stock...!!");
                    // materal.INVACO_CheckOutQty = "";
                    return;
                }
                var newItemNo = $scope.materaldocuupload.length + 1;
                if (newItemNo <= 100) {
                    $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
                }
            }
            else {
                swal("Please Select  Qunatity / Item  !")
            }

        };
        $scope.removegrnrows = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

            if ($scope.materaldocuupload.length === 0) {               //data
            }
        };



        var minutes;
        var meridiem;
        var hours
        $scope.checkOut = function () {
            //to get time in 12 hr format added by adarsh
            var currentDate = new Date();
            hours = currentDate.getHours();
            minutes = currentDate.getMinutes();
            meridiem = hours >= 12 ? 'PM' : 'AM';
            hours = hours % 12 || 12;
            var cuurtimee = hours + ':' + minutes + ' ' + meridiem;
            $scope.obj.INVACO_Time = cuurtimee;

            $scope.INVACO_Tolocation = document.getElementById("INVACO_Tolocation").innerText
            //time function ends------------------//
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {};
                $scope.employeename_two = "";
                for (var i = 0; i < $scope.get_employee.length; i++) {
                    if ($scope.get_employee[i].hrmE_Id == $scope.obj.HRME_Id.hrmE_Id) {
                        $scope.employeename_two = $scope.get_employee[i].employeename;
                    }
                }
                $scope.itemarray = [];
                if ($scope.materaldocuupload != null && $scope.materaldocuupload.length > 0) {
                    angular.forEach($scope.materaldocuupload, function (materal) {
                        if (materal.INVMI_Id_new > 0 && materal.INVACO_CheckOutQty > 0) {
                            $scope.itemarray.push({
                                INVMI_Id: materal.INVMI_Id_new,
                                INVACO_CheckoutDate: new Date(materal.INVACO_CheckoutDate).toDateString(),
                                INVACO_CheckOutQty: materal.INVACO_CheckOutQty,
                                INVSTO_SalesRate: materal.INVSTO_SalesRate,
                                INVMST_Id: materal.INVMST_Id,
                                INVACO_ItemRemarks: materal.INVACO_ItemRemarks,
                                INVACO_QuantityRemark: materal.INVACO_QuantityRemark,

                            })
                        }

                    });

                    if ($scope.itemarray != null && $scope.itemarray.length > 0) {

                        data = {

                            "INVACO_ReceivedBy": $scope.employeename_two,
                            "HRME_Id": $scope.obj.HRME_Id.hrmE_Id,
                            "itemarray": $scope.itemarray,
                            "MI_Id": $scope.obj.MI_Id,
                            "INVACO_MODE": $scope.obj.INVACO_MODE,
                            "INVACO_ISSUED": $scope.obj.INVACO_ISSUED,
                            "INVACO_Tolocation": $scope.INVACO_Tolocation,
                            "INVACO_CheckOutRemarks": $scope.obj.INVACO_CheckOutRemarks,
                            "INVACO_Vehicalno": $scope.obj.INVACO_Vehicalno,
                            "INVACO_Time": $scope.obj.INVACO_Time,
                            "INVACO_No": $scope.obj.INVACO_No,
                            transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                           "returnval": $scope.obj.returnval
                        };
                        var config = {
                            headers: {
                                'Content-Type': 'application/json;'
                            }
                        };
                        apiService.create("CheckOutAssets/GenarateDc", data).then(function (promise) {
                            if (promise.message == "save") {
                                swal("Record Save SuceesFully !");
                            }
                            else if (promise.message == "Notsave") {
                                swal("Record Not Saved !");
                            }
                            else if (promise.message == "admin") {
                                swal("Please Contact Administrtor  !");
                            }

                            $state.reload();
                        });
                    }
                    else {
                        swal("Items  Not Found ")
                        return;
                    }

                }
                else {
                    swal("Items  Not Found ")
                    return;
                }

            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cancel = function () {
            $state.reload();
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
                        apiService.create("CheckOutAssets/deactiveNew", item).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + " ");
                                }
                                else {
                                    swal("Record Not " + dystring + " ");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.editdc = function (item) {
            $scope.materaldocuupload = []
            $scope.obj.INVACO_No = "";
            var values = {
                "INVACO_No": item.INVACO_No,
                "MI_Id": $scope.obj.MI_Id
            }

            apiService.create("CheckOutAssets/editdcNew", values).then(function (promise) {
                if (promise.editdata != null && promise.editdata.length > 0) {
                    $scope.obj.INVACO_MODE = promise.editdata[0].invacO_MODE;
                    $scope.obj.returnval = true;
                    //$scope.obj.INVACO_Tolocation = promise.editdata[0].invacO_Tolocation
                    //to update ocation and address
                    document.getElementById("INVACO_Tolocation").innerText = promise.editdata[0].invacO_Tolocation;
                    $scope.obj.INVACO_CheckOutRemarks = promise.editdata[0].invacO_CheckOutRemarks
                    $scope.obj.INVACO_Vehicalno = promise.editdata[0].invacO_Vehicalno;
                    $scope.HRME_Idnew = promise.editdata[0].hrmE_Id;
                    $scope.obj.INVACO_No = item.INVACO_No;
                    $scope.obj.INVACO_ISSUED = promise.editdata[0].invacO_ISSUED;


                    angular.forEach(promise.editdata, function (materal) {
                        $scope.materaldocuupload.push({
                            INVMI_Id: materal.invmI_Id,
                            INVACO_CheckoutDate: new Date(materal.invacO_CheckoutDate),
                            INVACO_CheckOutQty: materal.invacO_CheckOutQty,
                            INVSTO_SalesRate: materal.invstO_SalesRate,
                            INVACO_ItemRemarks: materal.invacO_ItemRemarks,
                            INVACO_QuantityRemark: materal.invacO_QuantityRemark,
                            INVMST_Id: materal.invmsT_Id
                        })

                    });
                    if (promise.get_items != null && promise.get_items.length > 0) {
                        angular.forEach($scope.materaldocuupload, function (materal) {
                            for (var i = 0; i < promise.get_items.length; i++) {
                                if (materal.INVMI_Id == promise.get_items[i].INVMI_Id) {
                                    materal.INVMS_StoreName = promise.get_items[i].INVMS_StoreName;
                                    materal.INVSTO_PurchaseDate = new Date(promise.get_items[i].INVSTO_PurchaseDate);
                                    materal.INVSTO_SalesRate = promise.get_items[i].INVSTO_SalesRate;
                                    materal.INVSTO_AvaiableStock = promise.get_items[i].INVSTO_AvaiableStock;
                                    materal.INVMI_Id_new = promise.get_items[i].INVMI_Id;
                                    materal.INVACO_CheckoutDate = new Date();
                                    materal.INVMI_Id = promise.get_items[i];
                                }

                            }

                        });
                    }
                    $scope.newcaste = {};
                    if ($scope.get_employee != null && $scope.get_employee.length > 0) {
                        for (var i = 0; i < $scope.get_employee.length; i++) {
                            if (promise.editdata[0].hrmE_Id == $scope.get_employee[i].hrmE_Id) {
                                $scope.get_employee[i].Selected = true;
                                $scope.obj.HRME_Id = $scope.get_employee[i];
                                $scope.newcaste = $scope.get_employee[0].hrmE_Id;
                                return;
                            }
                        }
                    }

                    //get_items

                }
                //  $scope.editdata = promise.editdata;



            });

        }

        //added
        $scope.deactivestatus = function (item, SweetAlert) {

            var dystring = "";
            if (item.invmlO_Status === "Received") {
                dystring = "Received";
            }
            else if (item.invmlO_Status === "Issue") {
                dystring = "Issue";
            }
            if (dystring != "") {
                swal({
                    title: "Are you sure?",
                    text: "Do You Want To " + dystring + "  Item?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            var data = {
                                "INVMLO_Status": item.invmlO_Status,
                                "INVACO_Id": item.invacO_Id

                            }
                            apiService.create("CheckOutAssets/deactiveStatus", data).
                                then(function (promise) {
                                    if (promise.returnval === true) {
                                        swal("Record " + dystring + "d Successfully!!!");
                                    }
                                    else {
                                        swal("Record Not " + dystring + "d Successfully!!!");
                                    }
                                    $scope.onstorechange();
                                });
                        }
                        else {
                            swal("Record " + dystring + " Cancelled!!!");

                            $scope.onstorechange();
                        }
                    });
            }

        };
        $scope.reschedule = function (obj) {
            $scope.ttl = 0;
            $scope.imagename = $scope.instlist[0].mI_Logo;
            $scope.miname = $scope.instlist[0].mI_Name;
            $scope.miname = angular.uppercase($scope.miname);
            $scope.migst = $scope.instlist[0].mI_GSTNO;
            $scope.invaO_Tolocation = "";
            $scope.decno = "";
            $scope.Notes = "";
            $scope.Mode = "";
            $scope.Issued = "";
            $scope.printstudents = [];
            angular.forEach(obj.LIST, function (dd) {
                $scope.ttl = $scope.ttl + dd.invacO_CheckOutQty;
                $scope.printstudents.push(dd);
                $scope.dcdate = dd.updatedDate;
            })
            console.log($scope.printstudents);
            $scope.tempto = [];
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
            $scope.invaO_Tolocation = obj.invacO_Tolocation;
            document.getElementById("invaO_Tolocation").innerText = $scope.invaO_Tolocation;
            $scope.Note = obj.INVACO_CheckOutRemarks;
            $scope.Mode = obj.INVACO_MODE;
            $scope.Issued = obj.INVACO_ISSUED;
            $scope.Vehicalno = obj.INVACO_Vehicalno;
            $scope.Time = obj.INVACO_Time;

            $('#popup111').modal('show');
        }

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
    }
})();