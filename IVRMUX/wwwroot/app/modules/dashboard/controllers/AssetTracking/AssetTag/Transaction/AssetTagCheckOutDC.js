
(function () {
    'use strict';
    angular
        .module('app')
        .controller('AssetTagCheckOutDCController', AssetTagCheckOutDCController);
    AssetTagCheckOutDCController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function AssetTagCheckOutDCController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

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
        //===========================
        if ( transnumconfigsettings.length > 0) {
            for (var i = 0; i < transnumconfigsettings.length; i++) {
                if (transnumconfigsettings[i].imN_Flag === "AssetDc") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }
       
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
        //========================================== PAGE LOAD
        $scope.instlist = [];
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage = 10;
            $scope.itemsPerPage1 = 10;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("AssetTagCheckOutDC/getloaddata", pageid).
                then(function (promise) {                 
                    $scope.get_checkout = promise.get_checkout;
                    $scope.instlist = promise.instlist;
                    $scope.presentCountgrid = $scope.get_checkout.length;
                    $scope.get_checkoutdclist = promise.get_checkoutdclist;
                    $scope.tempdc = [];                                                       
                    angular.forEach($scope.get_checkoutdclist, function (ddd) {
                        if ($scope.tempdc.length == 0) {
                            $scope.tempdc.push({ INVMLO_No: ddd.invmlO_No });
                        }
                        else if ($scope.tempdc.length > 0) {
                            var already_cnt = 0;
                            angular.forEach($scope.tempdc, function (opq1) {
                                if (opq1.INVMLO_No == ddd.invmlO_No) {
                                    already_cnt += 1;
                                }
                            })

                            if (already_cnt == 0) {
                                $scope.tempdc.push({ INVMLO_No: ddd.invmlO_No });
                            }
                        }
                    })
                    $scope.mainarray = [];
                    angular.forEach($scope.tempdc, function (ss) {
                        var listdat = [];
                        angular.forEach($scope.get_checkoutdclist, function (ff) {
                            if (ss.INVMLO_No == ff.invmlO_No) {
                                listdat.push(ff);
                            }

                        })
                        ss.LIST = listdat;
                    })

                });
            if (transnumconfigsettings != null && transnumconfigsettings.length > 0) {
                for (var i = 0; i < transnumconfigsettings.length; i++) {
                    if (transnumconfigsettings[i].imN_Flag === "AssetDc") {
                        $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                    }
                }
            }
        };      
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.get_checkout, function (itm) {
                itm.selected = toggleStatus;

            });
        }
        $scope.optionToggled1 = function () {
            $scope.all = $scope.get_checkout.every(function (options) {
                return options.selected;
            });
        };
        //=====================================  SAVE DATA
        $scope.generatedc = function () {
            swal({
                title: "Are you sure?",
                text: "Do You Want Generate Aseet tag check-out DC ??",
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
                                $scope.ttl = $scope.ttl + dd.invatcO_CheckOutQty;
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
                                $scope.temptorecby.push({ invacO_ReceivedBy: ddd.invatcO_ReceivedBy });
                                $scope.temptorecbyname = ddd.invatcO_ReceivedBy;
                            }
                            else if ($scope.temptorecby.length > 0) {
                                var already_cnt = 0;
                                angular.forEach($scope.temptorecby, function (opq1) {
                                    if (opq1.invatcO_ReceivedBy == ddd.invatcO_ReceivedBy) {
                                        already_cnt += 1;
                                    }
                                })

                                if (already_cnt == 0) {
                                    $scope.temptorecby.push({ invacO_ReceivedBy: ddd.invatcO_ReceivedBy });
                                    $scope.temptorecbyname = $scope.temptorecbyname + ' , ' + ddd.invatcO_ReceivedBy;
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
                            apiService.create("AssetTagCheckOutDC/savedata", data).then(function (promise) {

                                if (promise.returnval === true) {
                                    $scope.decno = promise.trans_id;
                                    $scope.dcdate = promise.updatedDate;
                                    $scope.get_checkout = promise.get_checkout;
                                    $scope.get_checkoutdclist = promise.get_checkoutdclist;
                                    $scope.tempdc = [];
                                    angular.forEach($scope.get_checkoutdclist, function (ddd) {
                                        if ($scope.tempdc.length == 0) {
                                            $scope.tempdc.push({ INVMLO_No: ddd.invmlO_No });
                                        }
                                        else if ($scope.tempdc.length > 0) {
                                            var already_cnt = 0;
                                            angular.forEach($scope.tempdc, function (opq1) {
                                                if (opq1.INVMLO_No == ddd.invmlO_No) {
                                                    already_cnt += 1;
                                                }
                                            })

                                            if (already_cnt == 0) {
                                                $scope.tempdc.push({ INVMLO_No: ddd.invmlO_No });
                                            }
                                        }
                                    })
                                    $scope.mainarray = [];
                                    angular.forEach($scope.tempdc, function (ss) {
                                        var listdat = [];
                                        angular.forEach($scope.get_checkoutdclist, function (ff) {
                                            if (ss.INVMLO_No == ff.invmlO_No) {
                                                listdat.push(ff);
                                            }

                                        })
                                        ss.LIST = listdat;
                                    })
                                    $('#popup111').modal('show');
                                    swal("Aseet tag check-out DC Generated Successfully!!! !");

                                }
                                else {
                                    swal('Error in Aseet tag check-out  DC generation');
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
          
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
        $scope.searchValue1 = '';
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
                $scope.ttl = $scope.ttl + dd.invatcO_CheckOutQty;
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
                        if (opq1.invatcO_ReceivedBy == ddd.invatcO_ReceivedBy) {
                            already_cnt += 1;
                        }
                    })

                    if (already_cnt == 0) {
                        $scope.temptorecby.push({ invatcO_ReceivedBy: ddd.invatcO_ReceivedBy });
                        $scope.temptorecbyname = $scope.temptorecbyname + ' , ' + ddd.invatcO_ReceivedBy;
                    }

                }
            })



            $scope.decno = obj.INVMLO_No;


            $('#popup111').modal('show');

        }
        $scope.CloseDc = function () {
            $('#popup111').modal('hide');
           // $state.reload();
            $scope.loaddata(); 
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
        //===================== Get Item on store Change
    }
})();