(function () {
    'use strict';
    angular.module('app').controller('CheckBookSourceController', CheckBookSourceController)
    CheckBookSourceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', 'Excel', '$timeout']
    function CheckBookSourceController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, Excel, $timeout) {
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };


        $scope.submitted = false;

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {

            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;

        } else {
            paginationformasters = 10;
        }
        var configsettingsnew = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        $scope.itemsPerPage = paginationformasters;
        $scope.obj = {};
        $scope.currentPage = 1;
        $scope.INVMS_Id = false;
        $scope.VPAYVOU_Id = false;
        $scope.VPAYVOU_ChequeNo = false;
        //-----------Tax Grid Select All
        $scope.bankchange = [];
        $scope.emplist = [];
        $scope.hrmdlist = [];

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("CheckBankBalance/loaddata", pageid).then(function (promise) {
                $scope.allcompany = promise.allcompany;

            });
        };
        $scope.isRequiredmi = function () {

            return !$scope.allcompany.some(function (options) {
                return options.miid;
            });
            $scope.bankchange();
        };
        $scope.isRequiredmiBnk = function () {

            return !$scope.banklist.some(function (options) {
                return options.hrmbid;
            });

        };
        $scope.isRequiredmisup = function () {

            return !$scope.get_supplier.some(function (options) {
                return options.invmS_Idd;
            });

        };
        $scope.bankchange = function () {
            $scope.banklist = [];
            $scope.emplist = [];
            if ($scope.allcompany != null && $scope.allcompany.length > 0) {
                angular.forEach($scope.allcompany, function (comp) {
                    if (comp.miid == true) {
                        $scope.emplist.push({
                            MI_Id: comp.mI_Id
                        });
                    }
                });
            }
            var data = {
                "emplist": $scope.emplist,
            };
            apiService.create("CheckBankBalance/getProject", data).then(function (promise) {
                $scope.banklist = promise.banklist;
                $scope.get_supplier = promise.get_supplier;
                $scope.checkarray = promise.checkarray;

                if ($scope.banklist != null && $scope.banklist.length > 0) {
                    $scope.allhrmd = true;
                    angular.forEach($scope.banklist, function (itmee) {
                        itmee.hrmbid = true;
                    });
                }
                else {
                    $scope.allhrmd = false;
                    swal("Record Not Found !");
                }
            });
        }
        $scope.otherdaydatechange = function () {
            $scope.MasterChecks = [];
            $scope.checckleafTotal = [];
            angular.forEach($scope.allcompany, function (itm) {
                itm.miid = true;
            });
            $scope.bankchange();

        }
        //printDataTwo
        $scope.printDataTwo = function () {
            var innerContents = "";
            if ($scope.rdopunch == "Detailed") {
                innerContents = document.getElementById("detailed").innerHTML;
            }
            else {
                innerContents = document.getElementById("con").innerHTML;
            }
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.exportToExcel = function () {
            var innerContents;
            if ($scope.rdopunch == "Detailed") {
                innerContents = document.getElementById("detailed").innerHTML;
            }
            else {
                innerContents = document.getElementById("con").innerHTML;
            }
            var exportHref = Excel.tableToExcel(innerContents, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
           
        };
        $scope.printData = function () {
            var innerContents = "";
            if ($scope.rdopunch == "Detailed") {
                innerContents = document.getElementById("asdftt").innerHTML;
            }
            else {
                innerContents = document.getElementById("asdf").innerHTML;
            }
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.viewdetails = function (dd) {
            $scope.ComapnyName = dd.mI_Name;
            $scope.hrmbD_BankNameone = dd.hrmbD_BankName;
            $scope.hrmbD_BankAccountNoone = dd.hrmbD_BankAccountNo;

            var data = {
                "VMCHEBK_Id": dd.vmchebK_Id,

            };
            apiService.create("CheckBankBalance/viewdetails", data).then(function (promise) {
                $scope.edtilistP = promise.edtilistP;
                $('#popup111').modal('show');
            });
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
        $scope.INVMS_Idclick = function () {
            if ($scope.obj.INVMS == true) {
                $scope.obj.VPAYVOU_ChequeNo = false;
            }

        }
        $scope.VPAYVOU_ChequeNoclick = function () {
            if ($scope.obj.VPAYVOU_ChequeNo == true) {
                $scope.obj.INVMS = false;
            }
        }
        $scope.all_miid = function (ac) {

            var toggleStatus = $scope.allmi;
            angular.forEach($scope.allcompany, function (itm) {
                itm.miid = toggleStatus;
            });
            $scope.bankchange();
        };
        $scope.allhrmdTT = function (ac) {

            var toggleStatus = $scope.allhrmd;
            angular.forEach($scope.banklist, function (itm) {
                itm.hrmbid = toggleStatus;
            });

        };
        $scope.allinvmS_Idd = function (ac) {

            var toggleStatus = $scope.allinvmS_Id;
            angular.forEach($scope.get_supplier, function (itm) {
                itm.invmS_Idd = toggleStatus;
            });

        };

        $scope.mi_click = function () {
            $scope.allmi = $scope.allcompany.every(function (itm) {
                return itm.miid;
            });
            $scope.bankchange();
        };
        $scope.mi_clickhrmd = function () {
            $scope.allhrmd = $scope.banklist.every(function (itm) {
                return itm.hrmbid;
            });

        };
        $scope.mi_clickinvmS = function () {
            $scope.allinvmS_Id = $scope.get_supplier.every(function (itm) {
                return itm.invmS_Idd;
            });

        };
        $scope.Report = function () {
            $scope.MasterCheck = [];
            $scope.Inmvs_list = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.obj.VPAYVOU_ChequeNo == true) {
                    var rdopunch = "All";
                    $scope.checkDetails = [];
                    $scope.GRnDetails = [];
                    var data = {
                        "VPAYVOU_Id": $scope.obj.VPAYVOU_Id.vpayvoU_Id,
                        "optionflag": rdopunch
                    };
                    apiService.create("CheckBankBalance/Editdata", data).then(function (promise) {
                        if (promise.checkDetails != null && promise.checkDetails.length > 0) {
                            $scope.imagename = promise.fmI_Name;
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
                else {
                    angular.forEach($scope.allcompany, function (comp) {
                        if (comp.miid == true) {
                            $scope.emplist.push({
                                MI_Id: comp.mI_Id
                            });
                        }
                    });
                    if ($scope.banklist != null && $scope.banklist.length > 0) {
                        angular.forEach($scope.banklist, function (comp) {
                            if (comp.hrmbid == true) {
                                $scope.hrmdlist.push({
                                    HRMD_Id: comp.hrmbD_Id
                                });
                            }
                        });
                    }

                    if ($scope.get_supplier != null && $scope.get_supplier.length > 0) {
                        angular.forEach($scope.get_supplier, function (comp) {
                            if (comp.invmS_Idd == true) {
                                $scope.Inmvs_list.push({
                                    INVMS_Id: comp.invmS_Id
                                });
                            }
                        });
                    }
                    var VPAYVOU_Id = 0;
                    if ($scope.VPAYVOU_ChequeNo == true) {
                        VPAYVOU_Id = $scope.obj.VPAYVOU_Id.vpayvoU_Id;
                    }
                    var data = {
                        "emplist": $scope.emplist,
                        "hrmdlist": $scope.hrmdlist,
                        "optionflag": $scope.rdopunch,
                        "Inmvs_list": $scope.Inmvs_list,
                        "VPAYVOU_Idtwo": VPAYVOU_Id
                    };
                    apiService.create("CheckBankBalance/loaddataconfig", data).then(function (promise) {
                        if (promise.checckleafTotal != null && promise.checckleafTotal.length > 0 && $scope.rdopunch == "consolidated") {
                            $scope.checckleafTotal = promise.checckleafTotal;
                            $scope.employee = [];
                            $scope.employee = $scope.checckleafTotal[0].HRMBD_BankAccountNo;
                            $scope.employeeid = [];
                            angular.forEach($scope.checckleafTotal, function (dev) {
                                if ($scope.employeeid.length === 0) {
                                    $scope.employeeid.push({
                                        VMCHEBK_Id: dev.VMCHEBK_Id,
                                        VMCHEBK_SLNoFrom: dev.VMCHEBK_SLNoFrom,
                                        VMCHEBK_SLNoTo: dev.VMCHEBK_SLNoTo,
                                        VMCHEBK_NoOfChequeLeafs: dev.VMCHEBK_NoOfChequeLeafs,
                                        VMCHEBK_ReceivedDate: dev.VMCHEBK_ReceivedDate,
                                        VMCHEBKSLN_Remarks: dev.VMCHEBKSLN_Remarks,
                                        VMCHEBKSLN_CancelledFlg: dev.VMCHEBKSLN_CancelledFlg,
                                        HRMBD_BankName: dev.HRMBD_BankName,
                                        HRMBD_BankAccountNo: dev.HRMBD_BankAccountNo,
                                        VMCHEBK_LeafNo: dev.VMCHEBK_LeafNo,
                                        MI_Name: dev.MI_Name
                                    });
                                }
                                else if ($scope.employeeid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.employeeid, function (emp) {
                                        if (emp.VMCHEBK_Id === dev.VMCHEBK_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.employeeid.push({
                                            VMCHEBK_Id: dev.VMCHEBK_Id,
                                            VMCHEBK_SLNoFrom: dev.VMCHEBK_SLNoFrom,
                                            VMCHEBK_SLNoTo: dev.VMCHEBK_SLNoTo,
                                            VMCHEBK_NoOfChequeLeafs: dev.VMCHEBK_NoOfChequeLeafs,
                                            VMCHEBK_ReceivedDate: dev.VMCHEBK_ReceivedDate,
                                            VMCHEBKSLN_Remarks: dev.VMCHEBKSLN_Remarks,
                                            VMCHEBKSLN_CancelledFlg: dev.VMCHEBKSLN_CancelledFlg,
                                            HRMBD_BankName: dev.HRMBD_BankName,
                                            HRMBD_BankAccountNo: dev.HRMBD_BankAccountNo,
                                            VMCHEBK_LeafNo: dev.VMCHEBK_LeafNo,
                                            MI_Name: dev.MI_Name
                                        });
                                    }
                                }
                            });
                            console.log($scope.employeeid);
                            angular.forEach($scope.employeeid, function (ddd) {
                                $scope.templist = [];
                                angular.forEach($scope.checckleafTotal, function (dd) {
                                    if (dd.HRMBD_Id === ddd.HRMBD_Id) {
                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.plannerdetails = $scope.templist;
                            });
                            console.log($scope.employeeid);
                        }
                        if (promise.checckleafTotal != null && promise.checckleafTotal.length > 0 && $scope.rdopunch == "Detailed") {
                            $scope.checckleafTotal = promise.checckleafTotal;
                            $scope.employee = [];
                            $scope.employee = $scope.checckleafTotal[0].HRMBD_BankAccountNo;
                            $scope.employeeid = [];
                            angular.forEach($scope.checckleafTotal, function (dev) {
                                if ($scope.employeeid.length === 0) {
                                    $scope.employeeid.push({
                                        VPAYVOU_Id: dev.VPAYVOU_Id,
                                        HRMBD_BankName: dev.HRMBD_BankName,
                                        HRMBD_BankAccountNo: dev.HRMBD_BankAccountNo,
                                        INVMS_SupplierName: dev.INVMS_SupplierName,
                                        VPAYVOU_PaymentTo: dev.VPAYVOU_PaymentTo,
                                        VPAYVOU_PaymentDate: dev.VPAYVOU_PaymentDate,
                                        VPAYVOU_AppliedAmount: dev.VPAYVOU_AppliedAmount,
                                        VPAYVOU_ApprovedAmount: dev.VPAYVOU_ApprovedAmount,
                                        MI_Name: dev.MI_Name
                                    });
                                }
                                else if ($scope.employeeid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.employeeid, function (emp) {
                                        if (emp.HRMBD_Id === dev.HRMBD_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.employeeid.push({
                                            VPAYVOU_Id: dev.VPAYVOU_Id,
                                            HRMBD_BankName: dev.HRMBD_BankName,
                                            HRMBD_BankAccountNo: dev.HRMBD_BankAccountNo,
                                            INVMS_SupplierName: dev.INVMS_SupplierName,
                                            VPAYVOU_PaymentTo: dev.VPAYVOU_PaymentTo,
                                            VPAYVOU_PaymentDate: dev.VPAYVOU_PaymentDate,
                                            VPAYVOU_AppliedAmount: dev.VPAYVOU_AppliedAmount,
                                            VPAYVOU_ApprovedAmount: dev.VPAYVOU_ApprovedAmount,
                                            MI_Name: dev.MI_Name
                                        });
                                    }
                                }
                            });
                            console.log($scope.employeeid);
                            angular.forEach($scope.employeeid, function (ddd) {
                                $scope.templist = [];
                                angular.forEach($scope.checckleafTotal, function (dd) {
                                    if (dd.HRMBD_Id === ddd.HRMBD_Id) {
                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.plannerdetails = $scope.templist;
                            });
                            console.log($scope.employeeid);
                        }
                    });
                }

            }
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();
