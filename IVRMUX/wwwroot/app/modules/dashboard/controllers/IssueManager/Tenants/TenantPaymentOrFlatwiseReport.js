(function () {
    'use strict';
    angular
        .module('app')
        .controller('TenantPaymentOrFlatwiseReportController', TenantPaymentOrFlatwiseReportController);
    TenantPaymentOrFlatwiseReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter'];
    function TenantPaymentOrFlatwiseReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter) {

        var paginationformasters;
        $scope.multidepartment = false;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.obj = {};
        $scope.stdall = false;
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));


        $scope.editflag = false;
        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();
        $scope.obj.stdalPPl = false;
        $scope.stdall = false;
        $scope.submitted = false;
        angular.forEach($scope.get_printdata, function (obj) {
            obj.stdselected = false;
        });
        $scope.obj.TENENTNTB_PaymentFlg = true;
        $scope.obj.TENENTNTB_PaymentFlgPaid = true;
        $scope.obj.TENENTNTB_PaymentFlgPending = true;

        $scope.obj.TENENTNTB_FlatFlg = true;
        $scope.obj.TENENTNTB_FlgOccupied = true;
        $scope.obj.TENENTNTB_FlgVacant = true;

        $scope.start_Date = new Date();
        $scope.end_Date = new Date();

        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;

            $scope.all_checkC3 = function () {
                var checkStatus = $scope.mI_Id;
                var count = 0;
                angular.forEach($scope.getinstitution, function (itm) {
                    itm.selected = checkStatus;
                    if (itm.selected == true) {
                        count += 1;
                    }
                    else {
                        count = 0;
                    }
                });
                $scope.togchkbxC2();
            }

            $scope.all_checkC = function () {
                var checkStatus = $scope.hrmB_Id;
                var count = 0;
                angular.forEach($scope.building, function (itm) {
                    itm.selected = checkStatus;
                    if (itm.selected == true) {
                        count += 1;
                    }
                    else {
                        count = 0;
                    }
                });
                $scope.togchkbxC();
            }

            $scope.all_checkC1 = function () {
                var checkStatus = $scope.hrmF_Id;
                var count = 0;
                angular.forEach($scope.getfloordetails, function (itm) {
                    itm.selected = checkStatus;
                    if (itm.selected == true) {
                        count += 1;
                    }
                    else {
                        count = 0;
                    }
                });
                $scope.togchkbxC1();
            }

            $scope.all_checkC2 = function () {
                var checkStatus = $scope.obj.hrmR_Id;
                var count = 0;
                angular.forEach($scope.rooms, function (itm) {
                    itm.selected = checkStatus;
                    if (itm.selected == true) {
                        count += 1;
                    }
                    else {
                        count = 0;
                    }
                });
            }
            $scope.all_checkC4 = function () {
                var checkStatus = $scope.obj.ivrM_Month_Id;
                var count = 0;
                angular.forEach($scope.getmonth, function (itm) {
                    itm.selected = checkStatus;
                    if (itm.selected == true) {
                        count += 1;
                    }
                    else {
                        count = 0;
                    }
                });

            }




            $scope.obj.TENENTNTB_FlgAll = false;
            $scope.Clickall = function () {
                if ($scope.obj.TENENTNTB_PaymentFlgPaid == false) {
                    $scope.obj.TENENTNTB_PaymentFlg = false;
                }
                else if ($scope.obj.TENENTNTB_PaymentFlgPaid == true && $scope.obj.TENENTNTB_PaymentFlgPending == true) {
                    $scope.obj.TENENTNTB_PaymentFlg = true;
                }
            }
            //Clickalld
            $scope.Clickalld = function () {
                if ($scope.obj.TENENTNTB_PaymentFlgPending == false) {
                    $scope.obj.TENENTNTB_PaymentFlg = false;
                }
                else if ($scope.obj.TENENTNTB_PaymentFlgPaid == true && $scope.obj.TENENTNTB_PaymentFlgPending == true) {
                    $scope.obj.TENENTNTB_PaymentFlg = true;
                }
            }
            $scope.OnclivkEventAll = function () {
                if ($scope.obj.TENENTNTB_PaymentFlg == true) {
                    $scope.obj.TENENTNTB_PaymentFlgPaid = true;
                    $scope.obj.TENENTNTB_PaymentFlgPending = true;
                }
                else {
                    $scope.obj.TENENTNTB_PaymentFlgPaid = false;
                    $scope.obj.TENENTNTB_PaymentFlgPending = false;
                }
            }

            $scope.obj.TENENTNTB_FlgAll1 = false;
            $scope.Clickall1 = function () {
                $scope.getloaddata = [];
                if ($scope.obj.TENENTNTB_FlgOccupied == true && $scope.obj.TENENTNTB_FlgVacant == true) {
                    $scope.obj.TENENTNTB_FlatFlg = true;
                }
                else if ($scope.obj.TENENTNTB_FlgOccupied == false ) {
                    $scope.obj.TENENTNTB_FlatFlg = false;
                }
            }

            //Clickalld
            $scope.Clickalld1 = function () {
                $scope.getloaddata = [];
                if ($scope.obj.TENENTNTB_FlgVacant == false) {
                    $scope.obj.TENENTNTB_FlatFlg = false;
                }
                else if ($scope.obj.TENENTNTB_FlgOccupied == true && $scope.obj.TENENTNTB_FlgVacant == true) {
                    $scope.obj.TENENTNTB_FlatFlg = true;
                }
            }

            $scope.OnclivkEventAll1 = function () {
                $scope.getloaddata = [];
                if ($scope.obj.TENENTNTB_FlatFlg == true) {
                    $scope.obj.TENENTNTB_FlgOccupied = true;
                    $scope.obj.TENENTNTB_FlgVacant = true;
                }
                else {
                    $scope.obj.TENENTNTB_FlgOccupied = false;
                    $scope.obj.TENENTNTB_FlgVacant = false;
                }
            }

            $scope.togchkbxC2 = function () {

                $scope.mI_Id = $scope.getinstitution.every(function (options) {
                    return options.selected;
                });

                var MIID = [];
                angular.forEach($scope.getinstitution, function (ty) {
                    if (ty.selected) {
                        MIID.push(ty.mI_Id);
                    }
                })
                var data = {

                    "company_array": MIID,

                }

                apiService.create("TenantPaymentOrFlatwiseReport/companychange", data).
                    then(function (promise) {

                        $scope.building = promise.building;
                        $scope.getfloordetails = promise.getfloordetails;
                        $scope.rooms = promise.rooms;

                    })
            }
            $scope.togchkbxC = function () {

                $scope.hrmB_Id = $scope.building.every(function (options) {
                    return options.selected;
                });
                var MIID = [];
                angular.forEach($scope.getinstitution, function (ty) {
                    if (ty.selected) {
                        MIID.push(ty.mI_Id);
                    }
                })
                var HRMBID = [];
                angular.forEach($scope.building, function (ty) {
                    if (ty.selected) {
                        HRMBID.push(ty.hrmB_Id);
                    }
                })
                var data = {

                    "company_array": MIID,
                    "buldid": HRMBID,


                }

                apiService.create("TenantPaymentOrFlatwiseReport/buildingchange", data).
                    then(function (promise) {

                        $scope.getfloordetails = promise.getfloordetails;
                        $scope.rooms = promise.rooms;


                    })
            }


            $scope.togchkbxC1 = function () {

                $scope.hrmF_Id = $scope.getfloordetails.every(function (options) {
                    return options.selected;
                });
                var MIID = [];
                angular.forEach($scope.getinstitution, function (ty) {
                    if (ty.selected == true) {
                        MIID.push(ty.mI_Id);
                    }
                })
                var HRMBID = [];
                angular.forEach($scope.building, function (ty) {
                    if (ty.selected == true) {
                        HRMBID.push(ty.hrmB_Id);
                    }
                })
                var HRMFID = [];
                angular.forEach($scope.getfloordetails, function (ty) {
                    if (ty.selected == true) {
                        HRMFID.push(ty.hrmF_Id);
                    }
                })
                var data = {
                    "company_array": MIID,
                    "buldid": HRMBID,
                    "floorid": HRMFID,
                }

                apiService.create("TenantPaymentOrFlatwiseReport/floorchange", data).
                    then(function (promise) {


                        $scope.rooms = promise.rooms;


                    })
            }

            $scope.togchkbxC3 = function () {
                $scope.hrmR_Id = $scope.rooms.every(function (options) {
                    return options.selected;
                });
            }

            $scope.isOptionsRequired3 = function () {
                return !$scope.getinstitution.some(function (options) {
                    return options.selected;
                });
            }
            $scope.isOptionsRequired = function () {
                return !$scope.building.some(function (options) {
                    return options.selected;
                });
            }
            $scope.isOptionsRequired1 = function () {
                return !$scope.getfloordetails.some(function (options) {
                    return options.selected;
                });
            }
            $scope.isOptionsRequired2 = function () {
                return !$scope.rooms.some(function (options) {
                    return options.selected;
                });
            }
            $scope.isOptionsRequired4 = function () {
                return !$scope.getmonth.some(function (options) {
                    return options.selected;
                });
            }

            apiService.getURI("TenantPaymentOrFlatwiseReport/getdetails", pageid).then(function (promise) {
                $scope.getinstitution = promise.getinstitution;
                $scope.building = promise.building;
                $scope.getfloordetails = promise.getfloordetails;
                $scope.rooms = promise.rooms;
                $scope.getyear = promise.getyear;
                $scope.getmonth = promise.getmonth;



                //$scope.hrmD_Id ="4";


            });
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;
                $scope.reverse = !$scope.reverse;
            };
        };

        $scope.getreport = function () {
            $scope.submitted = true;
            $scope.getloaddata = [];
            if ($scope.myForm.$valid) {
                var MIID = [];
                angular.forEach($scope.getinstitution, function (ty) {
                    if (ty.selected) {
                        MIID.push(ty.mI_Id);
                    }
                })
                var HRMBID = [];
                angular.forEach($scope.building, function (ty) {
                    if (ty.selected) {
                        HRMBID.push(ty.hrmB_Id);
                    }
                })
                var HRMFID = [];
                angular.forEach($scope.getfloordetails, function (ty) {
                    if (ty.selected) {
                        HRMFID.push(ty.hrmF_Id);
                    }
                })
                var HRMRID = [];
                angular.forEach($scope.rooms, function (ty) {
                    if (ty.selected) {
                        HRMRID.push(ty.hrmR_Id);
                    }
                })
                var MONTHID = [];
                angular.forEach($scope.getmonth, function (ty) {
                    if (ty.selected) {
                        MONTHID.push(ty.ivrM_Month_Id);
                    }
                })
                var type = "";
                var maintype = "";

                if ($scope.obj.TENENTNT_Flag == 'Payment') {
                    maintype = $scope.obj.TENENTNT_Flag;
                    if ($scope.obj.TENENTNTB_PaymentFlg == true) {
                        type = "All";
                    }
                    else {
                        if ($scope.obj.TENENTNTB_PaymentFlgPaid == true) {
                            type = "paid";
                        }
                        else if ($scope.obj.TENENTNTB_PaymentFlgPending == true) {
                            type = "NonPaid";
                        }
                    }
                    
                }
                else if ($scope.obj.TENENTNT_Flag == 'Flatwise') {
                    maintype = $scope.obj.TENENTNT_Flag;
                    if ($scope.obj.TENENTNTB_FlatFlg == true) {
                        type = "All"
                    }
                    else if ($scope.obj.TENENTNTB_FlgOccupied == true) {
                        type = "Occupied"
                    }
                    else if ($scope.obj.TENENTNTB_FlgVacant == true){
                        type = "Vacant"
                    }
                }

                if ($scope.obj.TENENTNT_Flag == 'Flatwise') {

                    var data = {
                        "company_array": MIID,
                        "TENANTAGR_TenantName": $scope.tenantagR_TenantName,
                        "HRMB_BuildingName": $scope.hrmB_BuildingName,
                        "HRMR_RoomName": $scope.hrmR_RoomName,
                        "TENANTAGR_ContactNo": $scope.tenantagR_ContactNo,
                        "TENANTAGRPAY_Amount": $scope.tenantagrpaY_Amount,
                        "TENANTAGRPAY_Bankname": $scope.tenantagrpaY_Bankname,
                        "TENANTAGRPAY_Chequedate": $scope.tenantagrpaY_Chequedate,
                        "TENANTAGRPAY_Chequeno": $scope.tenantagrpaY_Chequeno,
                        "TENANTAGRPAY_ModeOfPayment": $scope.tenantagrpaY_ModeOfPayment,
                        "TENANTAGRPAY_PaymentDate": $scope.tenantagrpaY_PaymentDate,
                        "TENANTAGRPAY_ReceiptNo": $scope.tenantagrpaY_ReceiptNo,
                        "TENANTAGRPAY_Remarks": $scope.tenantagrpaY_Remarks,
                        "TENANTAGR_Id": $scope.tenantagR_Id,
                        "buldid": HRMBID,
                        "floorid": HRMFID,
                        "roomid": HRMRID,
                        "monthid": MONTHID,
                        "HRMLY_LeaveYear": $scope.obj.HRMLY_LeaveYear,
                        //"TENANTAGR_AgrFromDate": new Date($scope.start_Date).toDateString(),
                        //"TENANTAGR_AgrToDate": new Date($scope.end_Date).toDateString(),
                        "type": type,
                        "maintype": maintype

                    }
                }
                else if ($scope.obj.TENENTNT_Flag == 'NotOccupied') {
                    var data = {
                        "company_array": MIID,
                        "buldid": HRMBID,
                        "floorid": HRMFID,
                        "maintype": $scope.obj.TENENTNT_Flag
                    }
                }

                apiService.create("TenantPaymentOrFlatwiseReport/get_report", data).then(function (promise) {

                    if (promise.getloaddata != null && promise.getloaddata.length > 0) {

                        $scope.getloaddata = promise.getloaddata;
                        $scope.presentCountgrid = promise.getloaddata.length;
                    }
                    else {
                        swal("No Record Found..... !!");
                        $scope.get_printdata = "";
                        $state.reload();
                    }

                });

            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printDeviation").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        //$scope.exportToExcel = function (export_id) {
        //    var exportHref = Excel.tableToExcel(export_id, 'get_printdetail');
        //    $timeout(function () {
        //        location.href = exportHref;
        //    }, 100);
        //};

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.exportToExcel = function (export_id) {
            var an = $scope.stdall;
            if ($scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(export_id, 'printDeviation');
                $timeout(function () {
                    location.href = exportHref;
                }, 100);
            }

            else {

                swal("select atleast one record!");
            }
        };

        $scope.printdatatable = [];
        $scope.optionToggledstd = function (SelectedStudentRecord, index) {

            $scope.stdall = $scope.getloaddata.every(function (itm) {
                return itm.stdselected;
            });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }

            if ($scope.printdatatable.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
        }

        $scope.toggleAllstd = function (stdall) {

            var toggleStatus = stdall;

            angular.forEach($scope.getloaddata, function (itm) {


                itm.stdselected = toggleStatus;
                if (itm.stdselected == true) {
                    $scope.printdatatable.push(itm);
                    //$scope.getloaddata.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                    //$scope.getloaddata.splice(itm);
                }
            });

            if ($scope.printdatatable.length != null) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }

        }

        //===========Clear Field
        $scope.Clear = function () {
            $state.reload();

        };



    }
})();

