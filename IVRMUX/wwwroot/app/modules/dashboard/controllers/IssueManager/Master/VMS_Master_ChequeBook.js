(function () {
    'use strict';
    angular.module('app').controller('VMS_Master_ChequeBookController', VMS_Master_ChequeBookController)
    VMS_Master_ChequeBookController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function VMS_Master_ChequeBookController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {
        $scope.search = "";
        $scope.newdate = new Date();
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
        $scope.itemsPerPage1 = paginationformasters;
        $scope.currentPage = 1;
        $scope.currentPageone = 1;
        $scope.submitted = false;
        $scope.obj = {};
        $scope.flagg = true;
        $scope.slnocheck = [];
        $scope.employeeArray = [];
        $scope.obj.VMCHEBK_NoOfChequeLeafs = 0;
        $scope.newdate = new Date();
        $scope.obj.VMCHEBK_ReceivedDate = new Date();
        $scope.sourcelist = [];
        $scope.editflag = false;
        $scope.loaddata = function () {
            $scope.obj.MI_Id = "";
            var pageid = 2;
            apiService.getURI("CheckBankBalance/loaddata", pageid).then(function (promise) {
                $scope.allcompany = promise.allcompany;
                $scope.sourcelist = promise.sourcelist;

            });
        };
        $scope.slFroamChange = function () {
            $scope.slnocheck = [];
            $scope.slnochecktwo = [];
            $scope.actions = [];
            $scope.obj.VMCHEBK_SLNoTo = "";
            $scope.obj.VMCHEBK_NoOfChequeLeafs = "";
           
            
        }
        $scope.slNotOChange = function () {
            $scope.slnocheck = [];
            $scope.obj.VMCHEBK_SLNoTo = "";
            $scope.actions = [];
            $scope.slnochecktwo = [];
            $scope.iu = "";
            if ($scope.obj.VMCHEBK_SLNoFrom != null) {
                for (var i = 0; i < $scope.obj.VMCHEBK_NoOfChequeLeafs; i++) {
                    $scope.slnochecktwo.push({
                        vmchebK_LeafNo: (Number($scope.obj.VMCHEBK_SLNoFrom) + Number(i))
                    });
                }
                if ($scope.slnochecktwo != null && $scope.slnochecktwo.length > 0) {
                    for (var t = 0; t < $scope.slnochecktwo.length; t++) {
                        $scope.iu = $scope.slnochecktwo[t].vmchebK_LeafNo.toString();
                        if (Number($scope.obj.VMCHEBK_SLNoFrom.length) == Number($scope.iu.length)) {
                            $scope.slnocheck = $scope.slnochecktwo;
                            if ($scope.slnocheck != null && $scope.slnocheck.length > 0) {
                                var i = $scope.slnocheck.length - 1;
                                for (var e = 0; e < $scope.slnocheck.length; e++) {
                                    if (e == i) {
                                        $scope.obj.VMCHEBK_SLNoTo = $scope.slnocheck[e].vmchebK_LeafNo;
                                    }
                                }
                            }
                            return;
                        }
                        else {
                            var data = (Number($scope.obj.VMCHEBK_SLNoFrom.length) - Number($scope.iu.length));
                            var M = "";
                            for (var e = 0; e < data; e++) {
                                M += "0"
                            }
                            $scope.slnocheck.push({
                                vmchebK_LeafNo: M + Number($scope.slnochecktwo[t].vmchebK_LeafNo)
                            });
                        }
                    }
                }
                if ($scope.slnocheck != null && $scope.slnocheck.length > 0) {
                    var i = $scope.slnocheck.length - 1;
                    for (var e = 0; e < $scope.slnocheck.length; e++) {
                        if (e == i) {
                            $scope.obj.VMCHEBK_SLNoTo = $scope.slnocheck[e].vmchebK_LeafNo;
                        }
                    }
                }

            }
            else {
                swal("Enter Cheque Leafs Total !");
                $scope.obj.VMCHEBK_SLNoFrom = "";
                $scope.slnochecktwo = [];
                $scope.slnocheck = [];
            }


        }

        $scope.oncmpchange = function () {
            $scope.obj.HRMBD_Id = "";
            var data = {
                "MI_Id": $scope.obj.MI_Id,
                "optionflag": "vmscheckbook"
            };
            apiService.create("CheckBankBalance/loaddataconfig", data).then(function (promise) {

                $scope.banklist = promise.banklist;
                $scope.MasterChecks = promise.masterChecks;


            });
        }
        $scope.viewdetails = function (dd) {
            $scope.ComapnyName = dd.mI_Name;
            $scope.hrmbD_BankNameone = dd.hrmbD_BankName;
            $scope.hrmbD_BankAccountNoone = dd.hrmbD_BankAccountNo;

            var data = {
                "VMCHEBK_Id": dd.vmchebK_Id
            };
            apiService.create("CheckBankBalance/viewdetails", data).then(function (promise) {
                $scope.edtilistP = promise.edtilistP;
                $('#popup111').modal('show');
            });
        }
        $scope.savedata = function () {

            angular.forEach($scope.slnocheck, function (comp) {
                $scope.employeeArray.push({
                    vmchebK_LeafNo: comp.vmchebK_LeafNo
                });

            });
            if ($scope.employeeArray != null && $scope.employeeArray.length > 0) {
                //slnocheck

                if ($scope.myForm.$valid) {
                    swal({
                        title: "Are you sure?",
                        text: "Do You Want To Update  Cheque  Book Numbers  ?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Update ?",
                        cancelButtonText: "Cancel",
                        closeOnConfirm: false,
                        closeOnCancel: false
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                var data = {
                                    "MI_Id": $scope.obj.MI_Id,
                                    "HRMBD_Id": $scope.obj.HRMBD_Id,
                                    "VMCHEBK_Id": $scope.obj.VMCHEBK_Id,
                                    "VMCHEBK_ReceivedDate": new Date($scope.obj.VMCHEBK_ReceivedDate).toDateString(),
                                    "VMCHEBK_NoOfChequeLeafs": $scope.obj.VMCHEBK_NoOfChequeLeafs,
                                    "VMCHEBK_SLNoFrom": $scope.obj.VMCHEBK_SLNoFrom,
                                    "VMCHEBK_SLNoTo": $scope.obj.VMCHEBK_SLNoTo,
                                    "VMCHEBK_Remarks": $scope.obj.VMCHEBK_Remarks,
                                    "slnocheck": $scope.employeeArray
                                };

                                apiService.create("CheckBankBalance/savepaymentdetailsrecord", data).then(function (promise) {
                                    if (promise.returnval == true) {
                                        swal("Data Saved/Updated Successfully");
                                    }
                                    else if (promise.vmchebK_Remarks == "duplicate") {
                                        swal("Cheque number is duplicate! !");
                                    }
                                    else if (promise.returnval == false) {
                                        swal("Failed To Save/Update Record");
                                    }
                                    $state.reload();
                                });
                            }
                            else {
                                swal("Checeque  Book Numbers Cancelled !");
                            }
                        }
                    );
                }
                else {
                    $scope.submitted = true;
                }
            }
            else {
                swal("Cheque Book Numbers Not Generated !");
            }


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
        $scope.clientDecative = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.vmchebK_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (usersem.vmchebK_ActiveFlg === false) {
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
                        var data = {
                            "VMCHEBK_Id": usersem.vmchebK_Id
                        };

                        apiService.create("CheckBankBalance/loaddatareport", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record " + dystring + "d" + " Successfully!!!");


                            }
                            else {
                                swal("Record Not " + dystring + "d" + " Successfully!!!");
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.Editdata = function (user) {

            $scope.slnocheck = [];
            $scope.employeeArray = [];
            var data = {
                "VMCHEBK_Id": user.vmchebK_Id,
                "MI_Id": user.mI_Id,
                "HRMBD_Id": user.hrmbD_Id

            };
            apiService.create("CheckBankBalance/paymentnotification", data).then(function (promise) {
                debugger;
                $scope.editflag = true;
                $scope.obj.MI_Id = promise.getpaymentnotificationdetails[0].mI_Id;
                $scope.obj.VMCHEBK_Id = promise.getpaymentnotificationdetails[0].vmchebK_Id;
                $scope.obj.VMCHEBK_SLNoFrom = promise.getpaymentnotificationdetails[0].vmchebK_SLNoFrom;
                $scope.obj.VMCHEBK_SLNoTo = promise.getpaymentnotificationdetails[0].vmchebK_SLNoTo;
                $scope.obj.VMCHEBK_ReceivedDate = new Date(promise.getpaymentnotificationdetails[0].vmchebK_ReceivedDate);
                $scope.obj.VMCHEBK_NoOfChequeLeafs = promise.getpaymentnotificationdetails[0].vmchebK_NoOfChequeLeafs;
                $scope.obj.VMCHEBK_Remarks = promise.getpaymentnotificationdetails[0].vmchebK_Remarks;

                $scope.banklist = promise.banklist;
                $scope.obj.HRMBD_Id = promise.getpaymentnotificationdetails[0].hrmbD_Id;
                $scope.slnocheck = promise.checkbookSource;
                $scope.obj.all = true;
                //  $scope.chkAll();

            });
        }

        //$scope.chkAll = function () {
        //    if ($scope.obj.all == true) {
        //        angular.forEach($scope.slnocheck, function (comp) {
        //            comp.Flgone = true;
        //        });
        //    }
        //    else {
        //        angular.forEach($scope.slnocheck, function (comp) {
        //            comp.Flgone = false;
        //        });
        //    }
        //}
    }
})();
