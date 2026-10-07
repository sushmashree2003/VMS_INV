(function () {
    'use strict';
    angular
        .module('app')
        .controller('TADA_AdavanceApply_PaymentController', TADA_AdavanceApply_PaymentController)
    TADA_AdavanceApply_PaymentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile']
    function TADA_AdavanceApply_PaymentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile) {      
        $scope.usrname = localStorage.getItem('username');      
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        $scope.searchValue = '';
        $scope.searchValuee = "";
        $scope.editflag = false;
        $scope.editflagtwo = true;
        $scope.Ecashcard = false;
        $scope.flagone = false;
        $scope.flagtwo = false;
        $scope.newdate = new Date();
        $scope.currentPage = 1;
        $scope.currentPage1 = 1;
        $scope.VTADAAAP_Date = new Date();
        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        };
        $scope.onpageload = function () {
            var pageid = 1;
            apiService.getURI("TADA_AdavanceApply_Payment/getalldata", pageid).
                then(function (promise) {
                    $scope.get_Report = promise.getReport;
                    $scope.presentCountgrid = $scope.get_Report.length;
                    $scope.Adavance = promise.adavance;
                    $scope.presentCountgridf = $scope.Adavance.length;

                    if ($scope.get_Report.length > 0) {
                        $scope.pppfl = false;
                    }
                    else {
                        $scope.pppfl = true;
                    }
                })
        };
        $scope.payament = function () {
            $scope.VTADAAAP_AmountPaid = "";
            if ($scope.VTADAAAP_PaymentMode == "Bank" || $scope.VTADAAAP_PaymentMode == "Cheque" || $scope.VTADAAAP_PaymentMode == "Online") {
                $scope.Ecashcard = true;
            }
            else {
                $scope.Ecashcard = false;
            }
            if ($scope.VTADAAAP_PaymentMode == "Online") {
                $scope.VTADAAAP_PaymentRefNo = "";
            }
            if ($scope.VTADAAAP_PaymentMode == "Rejected") {
                $scope.VTADAAAP_AmountPaid = 0;
            }
        };
        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.submitted = true;        
            if ($scope.myForm.$valid) {
                if (Number($scope.sanction) >= Number($scope.VTADAAAP_AmountPaid)) {


                    var data = {
                        "VTADAAAP_PaymentMode": $scope.VTADAAAP_PaymentMode,
                        "VTADAAAP_AmountPaid": $scope.VTADAAAP_AmountPaid,
                        "VTADAAAP_PaymentRefNo": $scope.VTADAAAP_PaymentRefNo,
                        "VTADAAAP_Remarks": $scope.VTADAAAP_Remarks,
                        "VTADAAAP_Date": new Date($scope.VTADAAAP_Date).toDateString(),
                        "VTADAAA_Id": $scope.VTADAAA_Id,
                        "VTADAAAP_Id": $scope.VTADAAAP_Id,
                       // "VTADASLEAP_Id": $scope.VTADASLEAP_Id
                    };
                    apiService.create("TADA_AdavanceApply_Payment/savedta", data).then(function (promise) {
                        if (promise.returnvalue === true) {
                            if (promise.returnval === "Insert") {
                                swal("Record Saved Successfully");
                                $state.reload();
                            }
                            if (promise.returnval === "Failed") {
                                swal("Record Not saved");
                            }
                            if (promise.returnval === "Duplicate") {
                                swal("Record Already Exist");
                            }
                            if (promise.returnval === "Update") {
                                swal("Record Update Successfully");
                                $state.reload();
                            }
                            if (promise.returnval === "UpdateFailed") {
                                swal("Record Not Update");
                            }
                            if (promise.returnval === "") {
                                swal("Please contact Administrator !");
                                $state.reload();
                            }
                            if (promise.returnval === "RecordExit") {
                                swal(" Record Already Exist !");
                                $state.reload();
                            }
                        }
                        if (promise.returnvalue === false) {
                            if (promise.returnval === "") {
                                swal("please contact administrator !");
                                $state.reload();
                            }
                        }
                    });
                }
                else {
                    swal("Sanction amount Must Be Below Number of Total Amount Paid ! ");
                }
                }
        };
        $scope.totalAmount = function () {
            var total = 0;
            for (count = 0; count < $scope.data.length; count++) {
                total += parseInt($scope.data[count].total_amount, 10);
            }
            return total;
        };
        //========get.......
        $scope.getView = function (user) {

            var VTADAAA_Id = user.vtadaaA_Id;
            $scope.emplYoeeName = user.hrmE_EmployeeFirstName;
            $scope.hrmE_EmployeeCodeee = user.hrmE_EmployeeCode;
            if (VTADAAA_Id > 0) {
                var data = {
                    "VTADAAA_Id": VTADAAA_Id
                };
                apiService.create("TADA_AdavanceApply_Payment/Viewdetails", data).then(function (promise) {
                    $scope.ViewDetails = promise.veiwdetails;
                    if ($scope.ViewDetails != null && $scope.ViewDetails.length > 0) {
                        $scope.fromdate = $scope.ViewDetails[0].VTADAAA_FromDate;
                        $scope.todate = $scope.ViewDetails[0].VTADAAA_ToDate;
                        $scope.departuretime = moment($scope.ViewDetails[0].VTADAAA_DepartureTime, 'HH:mm a').format();
                        $scope.ArrivalTime = moment($scope.ViewDetails[0].VTADAAA_ArrivalTime, 'HH:mm a').format();
                        var date2 = new Date($scope.fromdate);
                        var date1 = new Date($scope.todate);
                        var timeDiff = Math.abs(date1.getTime() - date2.getTime());
                        var DateDifference = Math.ceil(timeDiff / (1000 * 3600 * 24) + 1);
                        $scope.datediff = DateDifference;
                        if ($scope.ViewDetails[0].VTADAAAD_ExpenditureHead != "" && $scope.ViewDetails[0].VTADAAAD_ExpenditureHead != null) {
                            $scope.flagone = true;
                            $scope.flagtwo = false;
                        }
                        else {

                            $scope.flagtwo = true;
                            $scope.flagone = false;
                        }
                        $scope.Getsancationed_List = [];
                        angular.forEach($scope.ViewDetails, function (dd) {
                            if ($scope.Getsancationed_List.length === 0) {
                                $scope.Getsancationed_List.push({
                                    VTADAAAA_SanctioningLevel: dd.VTADAAAA_SanctioningLevel,
                                    ApprovedBy: dd.ApprovedBy
                                });
                            } else if ($scope.Getsancationed_List.length > 0) {
                                var count = 0;

                                angular.forEach($scope.Getsancationed_List, function (d) {
                                    if (d.VTADAAAA_SanctioningLevel === dd.VTADAAAA_SanctioningLevel) {
                                        count += 1;
                                    }
                                });
                                if (count === 0) {
                                    $scope.Getsancationed_List.push({
                                        VTADAAAA_SanctioningLevel: dd.VTADAAAA_SanctioningLevel,
                                        ApprovedBy: dd.ApprovedBy
                                    });
                                }
                            }
                        });
                        $scope.getapproveddetails = [];
                        angular.forEach($scope.Getsancationed_List, function (dd) {
                            $scope.getapproveddetails = [];
                            angular.forEach($scope.ViewDetails, function (d) {
                                if (dd.VTADAAAA_SanctioningLevel === d.VTADAAAA_SanctioningLevel) {
                                    $scope.getapproveddetails.push(d);
                                }
                            });
                            dd.reportdetails = $scope.getapproveddetails;
                        });
                        $('#viewDeatils').modal('show');

                    }
                    else {
                        swal("TADA Authrization Request Is Not Availble For Approval");
                    }
                });

            }
        };
        //edittt
        $scope.edittt = function (item) {
           // $scope.VTADASLEAP_Id = "";
            $scope.ViewDetails = [];
            $scope.VTADAAAP_Id = item.vtadaaaP_Id;
            $scope.VTADAAA_Id = item.vtadaaA_Id;
            $scope.emplYoeeName = item.hrmE_EmployeeFirstName;
            $scope.hrmE_EmployeeCodeee = item.hrmE_EmployeeCode;
           // $scope.VTADASLEAP_Id = item.VTADASLEAP_Id;
            var data = {
                "VTADAAAP_Id": $scope.VTADAAAP_Id,
                "VTADAAA_Id": $scope.VTADAAA_Id
            };
            apiService.create("TADA_AdavanceApply_Payment/edittt", data).then(function (promise) {
                if (promise.edit_Two.length > 0 && promise.editArray.length > 0) {
                    $scope.transrowsedit = [];
                    $scope.staffname = promise.editArray[0].hrmE_EmployeeFirstName;
                    $scope.cityname = promise.editArray[0].ivrmmcT_Name;
                    $scope.applieamt = promise.editArray[0].vtadaaA_TotalAppliedAmount;
                    $scope.sanction = promise.editArray[0].vtadaaA_TotalSactionedAmount;
                    $scope.fromdate = promise.editArray[0].vtadaaA_FromDate;
                    $scope.todate = promise.editArray[0].vtadaaA_ToDate;

                    $scope.VTADAAAP_PaymentMode = promise.edit_Two[0].vtadaaaP_PaymentMode;
                    $scope.VTADAAAP_PaymentRefNo = promise.edit_Two[0].vtadaaaP_PaymentRefNo;
                    $scope.VTADAAAP_AmountPaid = promise.edit_Two[0].vtadaaaP_AmountPaid;
                    $scope.VTADAAAP_Remarks = promise.edit_Two[0].vtadaaaP_Remarks;
                    $scope.VTADAAAP_Date = promise.edit_Two[0].vtadaaaP_Date;
                    $scope.editflag = true;
                    $scope.editflagtwo = false;
                    if ($scope.VTADAAAP_PaymentMode == "Bank" || $scope.VTADAAAP_PaymentMode == "Cheque" || $scope.VTADAAAP_PaymentMode == "Online") {
                        $scope.Ecashcard = true;
                    }
                    else {
                        $scope.Ecashcard = false;
                    }
                }
                else {
                    swal("Record Not Found !!!");
                }
               
                $scope.ViewDetails = promise.veiwdetails;
                if ($scope.ViewDetails != null && $scope.ViewDetails.length > 0) {
                    $scope.fromdate = $scope.ViewDetails[0].VTADAAA_FromDate;
                    $scope.todate = $scope.ViewDetails[0].VTADAAA_ToDate;
                    $scope.departuretime = moment($scope.ViewDetails[0].VTADAAA_DepartureTime, 'HH:mm a').format();
                    $scope.ArrivalTime = moment($scope.ViewDetails[0].VTADAAA_ArrivalTime, 'HH:mm a').format();
                    var date2 = new Date($scope.fromdate);
                    var date1 = new Date($scope.todate);
                    var timeDiff = Math.abs(date1.getTime() - date2.getTime());
                    var DateDifference = Math.ceil(timeDiff / (1000 * 3600 * 24) + 1);
                    $scope.datediff = DateDifference;
                    if ($scope.ViewDetails[0].VTADAAAD_ExpenditureHead != "" && $scope.ViewDetails[0].VTADAAAD_ExpenditureHead != null) {
                        $scope.flagone = true;
                        $scope.flagtwo = false;
                    }
                    else {

                        $scope.flagtwo = true;
                        $scope.flagone = false;
                    }
                    $scope.Getsancationed_List = [];
                    angular.forEach($scope.ViewDetails, function (dd) {
                        if ($scope.Getsancationed_List.length === 0) {
                            $scope.Getsancationed_List.push({
                                VTADAAAA_SanctioningLevel: dd.VTADAAAA_SanctioningLevel,
                                ApprovedBy: dd.ApprovedBy
                            });
                        } else if ($scope.Getsancationed_List.length > 0) {
                            var count = 0;

                            angular.forEach($scope.Getsancationed_List, function (d) {
                                if (d.VTADAAAA_SanctioningLevel === dd.VTADAAAA_SanctioningLevel) {
                                    count += 1;
                                }
                            });
                            if (count === 0) {
                                $scope.Getsancationed_List.push({
                                    VTADAAAA_SanctioningLevel: dd.VTADAAAA_SanctioningLevel,
                                    ApprovedBy: dd.ApprovedBy
                                });
                            }
                        }
                    });
                    $scope.getapproveddetails = [];
                    angular.forEach($scope.Getsancationed_List, function (dd) {
                        $scope.getapproveddetails = [];
                        angular.forEach($scope.ViewDetails, function (d) {
                            if (dd.VTADAAAA_SanctioningLevel === d.VTADAAAA_SanctioningLevel) {
                                $scope.getapproveddetails.push(d);
                            }
                        });
                        dd.reportdetails = $scope.getapproveddetails;
                    });
                    // $('#viewDeatils').modal('show');

                }
                else {
                    swal("TADA Authrization Request Is Not Availble For Approval");
                }
            });
            $scope.scroll();
        };
        //TADAApply
        $scope.edit = function (editid) {           
            $scope.transrowsedit = [];
            $scope.staffname = editid.hrmE_EmployeeFirstName;
            $scope.VTADAAA_Id = editid.vtadaaA_Id;
            $scope.cityname = editid.ivrmmcT_Name;
            $scope.applieamt = editid.vtadaaA_TotalAppliedAmount;
            $scope.sanction = editid.vtadaaA_TotalSactionedAmount;
            $scope.fromdate = editid.vtadaaA_FromDate;
            $scope.todate = editid.vtadaaA_ToDate;
            //$scope.VTADASLEAP_Id = item.VTADASLEAP_Id;
            $scope.editflagtwo = false;
            $scope.editflag = true;
            var VTADAAA_Id = editid.vtadaaA_Id;;
            $scope.emplYoeeName =editid.hrmE_EmployeeFirstName;
            $scope.hrmE_EmployeeCodeee = editid.hrmE_EmployeeCode;
            if (VTADAAA_Id > 0) {
                var data = {

                    "VTADAAA_Id": VTADAAA_Id
                };
                apiService.create("TADA_AdavanceApply_Payment/Viewdetails", data).then(function (promise) {
                    $scope.ViewDetails = promise.veiwdetails;
                    if ($scope.ViewDetails != null && $scope.ViewDetails.length > 0) {
                        $scope.fromdate = $scope.ViewDetails[0].VTADAAA_FromDate;
                        $scope.todate = $scope.ViewDetails[0].VTADAAA_ToDate;
                        $scope.departuretime = moment($scope.ViewDetails[0].VTADAAA_DepartureTime, 'HH:mm a').format();
                        $scope.ArrivalTime = moment($scope.ViewDetails[0].VTADAAA_ArrivalTime, 'HH:mm a').format();
                        var date2 = new Date($scope.fromdate);
                        var date1 = new Date($scope.todate);
                        var timeDiff = Math.abs(date1.getTime() - date2.getTime());
                        var DateDifference = Math.ceil(timeDiff / (1000 * 3600 * 24) + 1);
                        $scope.datediff = DateDifference;
                        if ($scope.ViewDetails[0].VTADAAAD_ExpenditureHead != "" && $scope.ViewDetails[0].VTADAAAD_ExpenditureHead != null) {
                            $scope.flagone = true;
                            $scope.flagtwo = false;
                        }
                        else {

                            $scope.flagtwo = true;
                            $scope.flagone = false;
                        }
                        $scope.Getsancationed_List = [];
                        angular.forEach($scope.ViewDetails, function (dd) {
                            if ($scope.Getsancationed_List.length === 0) {
                                $scope.Getsancationed_List.push({
                                    VTADAAAA_SanctioningLevel: dd.VTADAAAA_SanctioningLevel,
                                    ApprovedBy: dd.ApprovedBy
                                });
                            } else if ($scope.Getsancationed_List.length > 0) {
                                var count = 0;

                                angular.forEach($scope.Getsancationed_List, function (d) {
                                    if (d.VTADAAAA_SanctioningLevel === dd.VTADAAAA_SanctioningLevel) {
                                        count += 1;
                                    }
                                });
                                if (count === 0) {
                                    $scope.Getsancationed_List.push({
                                        VTADAAAA_SanctioningLevel: dd.VTADAAAA_SanctioningLevel,
                                        ApprovedBy: dd.ApprovedBy
                                    });
                                }
                            }
                        });
                        $scope.getapproveddetails = [];
                        angular.forEach($scope.Getsancationed_List, function (dd) {
                            $scope.getapproveddetails = [];
                            angular.forEach($scope.ViewDetails, function (d) {
                                if (dd.VTADAAAA_SanctioningLevel === d.VTADAAAA_SanctioningLevel) {
                                    $scope.getapproveddetails.push(d);
                                }
                            });
                            dd.reportdetails = $scope.getapproveddetails;
                        });
                       

                    }
                    else {
                        swal("TADA Authrization Request Is Not Availble For Approval");
                    }
                });
            }
            $scope.scroll();
        };
        $scope.Deletedata = function (item, SweetAlert) {
            $scope.VTADAAAP_Id = item.vtadaaaP_Id;
            $scope.VTADAAA_Id = item.vtadaaA_Id;
            var data = {
                "VTADAAAP_Id": $scope.VTADAAAP_Id,
                "VTADAAA_Id": $scope.VTADAAA_Id
            }
            var dystring = "";
            if (item.vtadaaaP_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.vtadaaaP_ActiveFlg == false) {
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
                        apiService.create("TADA_AdavanceApply_Payment/deactive", data).
                            then(function (promise) {
                                if (promise.returnvalue == true) {
                                    if (promise.returnval === "Delete") {
                                        swal("Record " + dystring + " Successfully !!!");
                                    }
                                    if (promise.returnval === "NotDelete") {
                                        swal("Record Not " + dystring + " Delete !!!");
                                    }
                                }
                                else {
                                    swal("Please Contact Administrator !");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }
        $scope.Clearid = function () {
            $state.reload();
            $scope.tablediv = false;
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.totalAmountcal = function () {
            if ($scope.VTADAAAP_AmountPaid <= $scope.sanction) {
                
            }
            else {
                swal("Sanctioned Amount Not Greater Than  Paid Amount");
                $scope.VTADAAAP_AmountPaid = "";
                return;
            }
        }
        
    }
})();

