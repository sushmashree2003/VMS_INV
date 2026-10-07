(function () {
    'use strict';
    angular.module('app').controller('ChcekClearenceDateController', ChcekClearenceDateController)
    ChcekClearenceDateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function ChcekClearenceDateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag === "CLIINV") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.VBABRS_Dates = false;
        $scope.roleflag = false;
        $scope.loaddata = function () {
            var pageid = 2;
            $scope.MI_Id = "";
            $scope.HRMBD_Id = "";
            apiService.getURI("CheckBankBalance/loaddata", pageid).then(function (promise) {
                $scope.allcompany = promise.allcompany;
            });
        };
        $scope.Between = function () {
            $scope.trasactionReport = [];
        }
        $scope.oncmpchange = function () {
            $scope.HRMBD_Id = "";
            var data = {
                "MI_Id": $scope.MI_Id
            };
            apiService.create("CheckBankBalance/companychange", data).then(function (promise) {
                $scope.banklist = promise.banklist;

            });
        }
        $scope.submitted = false;
        $scope.RDIchaNge = function () {
            $scope.trasactionReport = [];
        }
        $scope.savedata = function () {
            var INVMS_Id = 0;
            if ($scope.banklist != null && $scope.banklist.length > 0) {
                for (var i = 0; i < $scope.banklist.length; i++) {
                    if ($scope.HRMBD_Id == $scope.banklist[i].hrmbD_Id) {
                        $scope.BankName = $scope.banklist[i].hrmbD_BankName;
                        $scope.AccountNumber = $scope.banklist[i].hrmbD_BankAccountNo;
                    }
                }
            }
            var fromdate = "";
            var todate = "";
            if ($scope.fromdate != null && $scope.todate != null && $scope.fromdate != "" && $scope.todate != "") {
                fromdate = new Date($scope.fromdate).toDateString();
                todate = new Date($scope.todate).toDateString()
            }

            if ($scope.myForm.$valid) {
                var data = {
                    "MI_Id": $scope.MI_Id,
                    "HRMBD_Id": $scope.HRMBD_Id,
                    "INVMS_Id": INVMS_Id,
                    "fromdate": fromdate,
                    "todate": todate,
                    "optionflag": $scope.optionflag
                };
                apiService.create("CheckBankBalance/ClearenceDate", data).then(function (promise) {
                    if (promise.trasactionReport != null && promise.trasactionReport.length > 0) {
                        $scope.trasactionReport = promise.trasactionReport;
                        $scope.imagenew = promise.fmI_Name;
                        angular.forEach($scope.trasactionReport, function (comp) {
                            comp.VBABRS_ClearedFlgone = false;
                        })
                    }
                    else {
                        swal("Record Not Found");
                    }


                });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("asdftt").innerHTML;         
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.bankchange = function () {
            $scope.trasactionReport = "";
            $scope.trasactionReport = [];
            var data = {
                "HRMBD_Id": $scope.HRMBD_Id,
                "MI_Id": $scope.MI_Id
            };
            apiService.create("CheckBookEntry/getbom", data).then(function (promise) {
                $scope.BanKBalance = promise.projectlist[0].vbabrS_BankBalalnce;
            });
        }

        $scope.CountBank = function () {
            var BankBalnce = 0;
            $scope.roleflag = false;
            if ($scope.trasactionReport != null && $scope.trasactionReport.length > 0) {
                for (var i = 0; i < $scope.trasactionReport.length; i++) {
                    if ($scope.trasactionReport[i].VBABRS_ClearedFlgone == true) {                      
                        BankBalnce += (Number($scope.trasactionReport[i].VPAYVOU_ApprovedAmount));
                    }
                }
            }
            if (BankBalnce > 0) {
                $scope.ApprovedAmount = BankBalnce;
            }
            else {
                $scope.ApprovedAmount = 0;
            }
            if ($scope.BanKBalance < $scope.ApprovedAmount) {
                $scope.ApprovedAmount = 0;
                angular.forEach($scope.trasactionReport, function (comp) {
                    comp.VBABRS_ClearedFlgone = false;
                })
                swal('Approved Amount Should Not be Greater (' + $scope.BankName + ' / ' + $scope.AccountNumber + ' ) --  Bank Balance !');
                $scope.roleflag = true;
               
               
            }
        }
        $scope.viewdetailsTWO = function (dd) {
            var rdopunch = "All";
            $scope.checkDetails = [];
            $scope.GRnDetails = [];
            var data = {
                "VPAYVOU_Id": dd.VPAYVOU_Id,
                "optionflag": rdopunch
            };
            apiService.create("CheckBankBalance/Editdata", data).then(function (promise) {
                if (promise.checkDetails != null && promise.checkDetails.length > 0) {
                    $scope.checkDetails = promise.checkDetails;
                    $scope.GRnDetails = promise.checkbookgrn;
                    $scope.VPAYVOU_ChequeNo = $scope.checkDetails[0].VPAYVOU_ChequeNo;
                    if ($scope.checkDetails[0].INVMS_SupplierName != null && $scope.checkDetails[0].INVMS_SupplierName != "") {
                        $scope.partyname = $scope.checkDetails[0].INVMS_SupplierName;
                    }
                    else {
                        $scope.partyname = $scope.checkDetails[0].VPAYVOU_PaymentTo;
                    }
                    $scope.VoucherCreateby = $scope.checkDetails[0].createdby;
                    $scope.VPAYVOU_PaymentDate = new Date($scope.checkDetails[0].VPAYVOU_PaymentDate);
                    $scope.VPAYVOU_Remarks = $scope.checkDetails[0].VPAYVOU_Remarks;
                    $scope.VPAYVOU_StatusFlg = $scope.checkDetails[0].VPAYVOU_StatusFlg;
                    $scope.HRMBD_BankName = $scope.checkDetails[0].HRMBD_BankName;
                    $scope.HRMBD_BankAccountNo = $scope.checkDetails[0].HRMBD_BankAccountNo;
                    $scope.projectlistTWO = promise.projectlistTWO;
                    $('#popup123').modal('show');
                }
                else {
                    swal("Record Not found !");

                }

            });
        }
        //excel export
        $scope.submitted = false;
        $scope.savedataClearFlag = function () {
            $scope.cleararrflag = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.trasactionReport != null && $scope.trasactionReport.length > 0 && $scope.optionflag =="PAYMENT") {
                    angular.forEach($scope.trasactionReport, function (comp) {
                        if (comp.VBABRS_ClearedFlgone == true && comp.VBABRS_ClearedDateone != null) {
                            $scope.cleararrflag.push({

                                VBABRS_ClearedDateone: new Date(comp.VBABRS_ClearedDateone).toDateString(),
                                VPAYVOU_Id: comp.VPAYVOU_Id
                            });
                        }
                    });
                }
                if ($scope.trasactionReport != null && $scope.trasactionReport.length > 0 && $scope.optionflag == "CONTRA") {
                    angular.forEach($scope.trasactionReport, function (comp) {
                        if (comp.VBABRS_ClearedFlgone == true && comp.VBABRS_ClearedDateone != null) {
                            $scope.cleararrflag.push({

                                VBABRS_ClearedDateone: new Date(comp.VBABRS_ClearedDateone).toDateString(),
                                VPAYVOU_Id: comp.VPAYVOU_Id
                            });
                        }
                    });
                }
                if ($scope.trasactionReport != null && $scope.trasactionReport.length > 0 && $scope.optionflag == "RECEIPT") {
                    angular.forEach($scope.trasactionReport, function (comp) {
                        if (comp.VBABRS_ClearedFlgone == true && comp.VBABRS_ClearedDateone != null) {
                            $scope.cleararrflag.push({

                                VBABRS_ClearedDateone: new Date(comp.VBABRS_ClearedDateone).toDateString(),
                                VPAYVOU_Id: comp.VRECVOU_Id
                            });
                        }
                    });
                }
                if ($scope.cleararrflag != null && $scope.cleararrflag.length > 0) {
                    var data = {
                        "cleararrflag": $scope.cleararrflag,
                        "optionflag": $scope.optionflag
                    };

                    apiService.create("CheckBankBalance/getreport", data).then(function (promise) {
                        $scope.cleararrflag = [];
                        if (promise.msg == "saved") {
                            swal("Data Saved/Updated Successfully");
                        } else {
                            swal("Contact Adminstrator");
                        }
                        $state.reload();
                    });
                }
                else {
                    swal("Select Atleast One Record  !");
                }
            }
           

        }
    }
})();
