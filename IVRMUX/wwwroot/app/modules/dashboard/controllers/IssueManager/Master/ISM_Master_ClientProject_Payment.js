(function () {
    'use strict';
    angular.module('app').controller('ISM_Master_ClientProject_PaymentController', ISM_Master_ClientProject_PaymentController)
    ISM_Master_ClientProject_PaymentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function ISM_Master_ClientProject_PaymentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.searchchkbx = "";
        $scope.editflag = false;
        $scope.submitted = false;
        $scope.maxdate = new Date();
        var MI_Id = 0;
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
        $scope.totalgrid = [];

        $scope.submitted = false;

        $scope.loaddata = function () {

            $scope.ISMMCLT_Id = "";
            $scope.ISMMPR_Id = "";
            $scope.ISMCLTPRP_Year = "";
            $scope.FTI_Id = "";
            $scope.ISMCLTPRP_InstallmentAmt = "";
            $scope.ISMCLTPRP_PaymentDate = null;
            $scope.ismcltprP_Id = 0;
            $scope.projectlist = [];
            $scope.editflag = false;

            var pageid = 2;
            apiService.getURI("ISM_Master_ClientProject_Payment/loaddata", pageid).then(function (promise) {
                $scope.clientlist = promise.clientlist;
                $scope.yearlist = promise.yearlist;
                $scope.alldata = promise.alldata;
                $scope.getinstallment = promise.getinstallment;
                $scope.master_bankdetails = promise.master_bankdetails;
                MI_Id = promise.mI_Id;
            });
        };

        $scope.onchangeProject = function (ISMMCLT_Id) {
            var data = {
                "ISMMCLT_Id": ISMMCLT_Id.ismmclT_Id
            };
            apiService.create("ISM_Master_ClientProject_Payment/getProject", data).then(function (promise) {
                $scope.projectlist = promise.projectlist;
            });
        };

        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                angular.forEach($scope.getinstallment, function (d) {
                    if (d.ftI_Id === parseInt($scope.FTI_Id)) {
                        $scope.ISMCLTPRP_InstallmentName = d.ftI_Name;
                    }
                });

                var data = {
                    "ISMCLTPRP_Id": $scope.ismcltprP_Id,
                    "ismmclT_Id": $scope.ISMMCLT_Id.ismmclT_Id,
                    "ISMMPR_Id": $scope.ISMMPR_Id,
                    "asmaY_Id": $scope.ISMCLTPRP_Year,
                    "ISMCLTPRP_InstallmentName": $scope.ISMCLTPRP_InstallmentName,
                    "ISMCLTPRP_InstallmentAmt": $scope.ISMCLTPRP_InstallmentAmt,
                    "ISMCLTPRP_PaymentDate": new Date($scope.ISMCLTPRP_PaymentDate).toDateString(),
                    "FTI_Id": $scope.FTI_Id
                };

                apiService.create("ISM_Master_ClientProject_Payment/savedata", data).then(function (promise) {
                    if (promise.duplicate === true) {
                        swal("Data Already Exist!");
                    }
                    else if (promise.msg === 'saved') {
                        swal("Data Saved Successfully");
                    }
                    else if (promise.msg === 'Failed') {
                        swal("Data not Saved Successfully");
                    }
                    else if (promise.msg === 'updated') {
                        swal("Data Updated Successfully ");
                    } else {
                        swal("Failed To Save/Update Record");
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.Editdata = function (user) {
            var data = {
                "ISMCLTPRP_Id": user.ismcltprP_Id
            };
            apiService.create("ISM_Master_ClientProject_Payment/Editdata", data).then(function (promise) {
                if (promise.editlist !== null && promise.editlist.length > 0) {

                    $scope.editflag = true;
                    $scope.geteditclient = promise.geteditclient;
                    $scope.projectlist = promise.projectlist;

                    $scope.clientid = promise.geteditclientprojectid[0].ismmclT_Id;
                    $scope.ISMMPR_Id = promise.geteditclientprojectid[0].ismmpR_Id;

                    $scope.ismcltprP_Id = promise.editlist[0].ismcltprP_Id;
                    $scope.ISMMCLTPR_Id = promise.editlist[0].ismmcltpR_Id;
                    $scope.ISMCLTPRP_Year = promise.editlist[0].ismcltprP_Year;
                    $scope.ISMCLTPRP_InstallmentName = promise.editlist[0].ismcltprP_InstallmentName;
                    $scope.ISMCLTPRP_InstallmentAmt = promise.editlist[0].ismcltprP_InstallmentAmt;
                    $scope.ISMCLTPRP_PaymentDate = new Date(promise.editlist[0].ismcltprP_PaymentDate);
                    $scope.FTI_Id = promise.editlist[0].ftI_Id;

                    angular.forEach($scope.clientlist, function (dd) {
                        if (dd.ismmclT_Id === $scope.clientid) {
                            $scope.ISMMCLT_Id = $scope.geteditclient[0];
                        }
                    });
                }
            });
        };

        $scope.clientDecative = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.ismcltprP_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (usersem.ismcltprP_ActiveFlag === false) {
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
                        apiService.create("ISM_Master_ClientProject_Payment/clientDecative", usersem).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record " + dystring + "d" + " Successfully!!!");
                                $state.reload();
                            }
                            else {
                                swal("Record Not " + dystring + "d" + " Successfully!!!");
                                $state.reload();
                            }
                        });
                    }
                    else {
                        swal("Record" + dystring + "Cancelled!!!");
                    }
                });
        };

        $scope.viewdetails = function (dd) {
            $scope.submitted1 = false;
            $scope.totalgrid = [];
            $scope.totalgrid = [{ id: 'viewdetails1' }];

            $scope.ISMCLTPRP_Id_New = 0;
            var data = {
                "ISMCLTPRP_Id": dd.ismcltprP_Id
            };
            apiService.create("ISM_Master_ClientProject_Payment/viewdetails", data).then(function (promise) {
                $scope.clientname = dd.ismmclT_ClientName;
                $scope.clientid = dd.ismmclT_Id;
                $scope.projectname = dd.ismmpR_ProjectName;
                $scope.yearname = dd.asmaY_Year;
                $scope.installmentname = dd.ismcltprP_InstallmentName;
                $scope.paymentstatusflagnew = dd.paymentstatusflag;
                $scope.installmentamount = dd.ismcltprP_InstallmentAmt;
                $scope.ISMCLTPRP_Id_New = dd.ismcltprP_Id;
                $scope.duedate = new Date(dd.ismcltprP_PaymentDate);

                $scope.paymentmodedetails = promise.paymentmodedetails;
                $scope.getpaymentdetails = promise.getpaymentdetails;
                if ($scope.getpaymentdetails !== null && $scope.getpaymentdetails.length > 0) {
                    $scope.totalgrid = $scope.getpaymentdetails;

                    angular.forEach($scope.totalgrid, function (dd) {
                        dd.ismcppD_ReceivedDate = new Date(dd.ismcppD_ReceivedDate);
                        if (dd.ismcppD_ChequeDate !== null) {
                            dd.ismcppD_ChequeDate = new Date(dd.ismcppD_ChequeDate);
                        }

                        angular.forEach($scope.paymentmodedetails, function (ddd) {
                            if (ddd.ivrmmoD_Id === parseInt(dd.ivrmmoD_Id)) {
                                if (ddd.ivrmmoD_Flag === 'C' || ddd.ivrmmoD_Flag === 'c') {
                                    dd.flag = 0;
                                } else {
                                    dd.flag = 1;
                                }
                            }
                        });
                    });
                }
                $('#myModalreadmitview').modal('show');

            });
        };

        $scope.onchangepaymentmode = function (user) {
            angular.forEach($scope.paymentmodedetails, function (dd) {
                if (dd.ivrmmoD_Id === parseInt(user.ivrmmoD_Id)) {
                    if (dd.ivrmmoD_Flag === 'C' || dd.ivrmmoD_Flag === 'c') {
                        user.flag = 0;
                    } else {
                        user.flag = 1;
                    }
                }
            });
        };
        $scope.bankchange = function (user) {
            var Message = "";
           
           
            if ($scope.master_bankdetails != null && $scope.master_bankdetails.length > 0) {
                for (var i = 0; i < $scope.master_bankdetails.length; i++) {
                    if ($scope.master_bankdetails[i].hrmbD_Id == user.HRMBD_Id) {
                        Message = $scope.master_bankdetails[i].hrmbD_BankName + '/' + $scope.master_bankdetails[i].hrmbD_BankAccountNo;
                    }
                }
            }
            var data = {
                "HRMBD_Id": user.HRMBD_Id,
                "MI_Id": MI_Id
            };
            apiService.create("CheckBookEntry/getbom", data).then(function (promise) {
                if (promise.msg != null && promise.msg != "" && promise.msg == "OB") {
                   


                }
                else {
                    if (Message != null && Message != "") {
                        swal("Must Be Entered  : " + Message + " - Opening Balnace !");
                    }
                    user.HRMBD_Id = "";
                }


            });
        }
        $scope.savepaymentdetailsrecord = function () {
            $scope.submitted1 = false;
            if (true) {
                $scope.savepaymentdetailstemp = [];
                angular.forEach($scope.totalgrid, function (dd) {
                    var date = "";
                    if (dd.flag === 1) {
                        date = new Date(dd.ismcppD_ChequeDate).toDateString();
                    } else {
                        date = new Date().toDateString();
                    }

                    $scope.savepaymentdetailstemp.push({
                        ISMCPPD_Id: dd.ismcppD_Id,
                        ISMCPPD_ReceivedAmount: dd.ismcppD_ReceivedAmount,
                        ISMCPPD_ReceivedDate: new Date(dd.ismcppD_ReceivedDate).toDateString(),
                        IVRMMOD_Id: dd.ivrmmoD_Id,
                        ISMCPPD_PaymentRefNo: dd.ismcppD_PaymentRefNo,
                        ISMCPPD_TDSAmount: dd.ismcppD_TDSAmount,
                        ISMCPPD_Remarks: dd.ismcppD_Remarks,
                        ISMCPPD_ChequeDate: date,
                        flag: dd.flag,
                        HRMBD_Id: dd.HRMBD_Id                        
                    });
                });

                var data = {
                    "ISM_Client_Project_Payment_Details": $scope.savepaymentdetailstemp,
                    "ISMCLTPRP_Id": $scope.ISMCLTPRP_Id_New,
                    "ISMCLTPRP_InstallmentAmt": $scope.installmentamount,
                    "ISMMCLT_Id": $scope.clientid,
                    "ISMMCLT_ClientName": $scope.clientname,

                };

                apiService.create("ISM_Master_ClientProject_Payment/savepaymentdetailsrecord", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Record Saved/Updated Successfully");
                        } else {
                            swal("Failed To Save/Update Record");
                        }
                        $('#myModalreadmitview').modal('hide');
                        $scope.loaddata();
                    }
                });

            } else {
                $scope.submitted1 = true;
            }
        };

        $scope.totalgrid = [{ id: 'viewdetails1' }];

        $scope.addNewsiblingguard = function () {

            var newItemNo = $scope.totalgrid.length + 1;
            if (newItemNo <= 100) {
                $scope.totalgrid.push({ 'id': 'viewdetails' + newItemNo, editfalguser: false, editadduser: false });
            }

            console.log($scope.totalgrid);
            if ($scope.editflag === true) {
                $scope.editadd = false;
            }
        };

        $scope.removeNewsiblingguard = function (index) {
            var newItemNo = $scope.totalgrid.length - 1;
            $scope.totalgrid.splice(index, 1);

            if ($scope.totalgrid.length === 0) {
                //data
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.clearpayment = function () {
            $scope.clientname = "";
            $scope.projectname = "";
            $scope.yearname = "";
            $scope.installmentname = "";
            $scope.installmentamount = "";
            $scope.ISMCLTPRP_Id_New = 0;
            $scope.duedate = "";
            $scope.totalgrid = [];
            $scope.totalgrid = [{ id: 'viewdetails1' }];
            $('#myModalreadmitview').modal('hide');
            $scope.loaddata();
        };

        $scope.search = '';
        $scope.filterValue = function (obj) {
            return ($filter('date')(obj.ismcltprP_PaymentDate, 'dd/MM/yyyy').indexOf($scope.search) >= 0) ||
                (JSON.stringify(obj.ismcltprP_InstallmentAmt)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.ismmclT_ClientName)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.ismmpR_ProjectName)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.asmaY_Year)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.ismcltprP_InstallmentName)).indexOf(angular.lowercase($scope.search)) >= 0;
        };
    }
})();
