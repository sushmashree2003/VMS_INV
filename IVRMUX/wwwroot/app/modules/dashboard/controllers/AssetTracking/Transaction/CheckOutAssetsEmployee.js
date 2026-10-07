
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CheckOutAssetsEmployeeController', CheckOutAssetsController);
    CheckOutAssetsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function CheckOutAssetsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.obj = {};
        $scope.editS = false;
        $scope.searchValue1 = '';
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
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        //if (transnumconfigsettings != null && transnumconfigsettings.length > 0) {
        //    for (var i = 0; i < transnumconfigsettings.length; i++) {
        //        if (transnumconfigsettings[i].imN_Flag === "CHECKOUTDC") {
        //            $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
        //        }

        //    }
        //}

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
                $scope.obj.MI_Id = promise.allcompany[0].mI_Id;
                $scope.onstorechange();
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
            apiService.create("INV_T_Sales/GetDcItemsemployee", data).then(function (promise) {
               
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
                                INVACO_ISSUED: ddd.invacO_ISSUED,
                                INVACO_Vehicalno: ddd.invacO_Vehicalno,
                                INVACO_Time: ddd.invacO_Time,
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

                else {
                    swal("Check-Out Assets DC Not Generated ! ");
                }
            });
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