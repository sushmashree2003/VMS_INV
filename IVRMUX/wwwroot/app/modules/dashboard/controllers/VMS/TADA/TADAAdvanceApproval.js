(function () {
    'use strict';
    angular
        .module('app')
        .controller('TADAAdvanceApprovalController', TADAAdvanceApprovalController)
    TADAAdvanceApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function TADAAdvanceApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {
        $scope.currentPage = 1;
        $scope.currentPage2 = 1;
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        else {
            paginationformasters = 10;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPage2 = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.searchValue = "";
        $scope.flag = false;
        $scope.maxDatedof = new Date($scope.today);
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian; todate
        };
        $scope.loaddata = function () {
            var pageid = 1;
            apiService.getURI("TADAAdvanceApproval/getalldata", pageid).
                then(function (promise) {
                    $scope.getadvancetada = promise.getadvancetada;
                    if ($scope.getadvancetada.length > 0) {
                        $scope.prflag = true;
                        $scope.pppfl = false;
                    }
                    else {
                        $scope.pppfl = true;
                    }
                })
        }
        $scope.submitted = false;
        var approvecnt = 0;
        $scope.savedata = function (tp) {
            approvecnt = 0;
            if (tp == 'AR') {
                approvecnt = 0;
            }
            else if (tp == 'A') {
                approvecnt = 1;
            }
            else if (tp == 'R') {
                approvecnt = 0;
                $scope.VTADAAA_TotalSactionedAmount = 0;
            }
            $scope.submitted = true;
            var data = {};
            if ($scope.myForm.$valid) {
                if ($scope.transrowsedit != null && $scope.transrowsedit.length > 0) {
                    angular.forEach($scope.transrowsedit, function (li) {
                        if (li.VTADAAAD_ExpenditureHead != null && li.VTADAAAD_ExpenditureHead != "") {
                            if (li.flag === 'A') {
                                approvecnt += 1;
                            }
                        }

                    });
                }

                data = {
                    "VTADAAA_Remarks": $scope.VTADAAA_Remarks,
                    "VTADAAA_TotalSactionedAmount": $scope.VTADAAA_TotalSactionedAmount,
                    "headarray": $scope.transrowsedit,
                    "VTADAAA_Id": $scope.VTADAAA_Id,
                    "MI_Id": $scope.MI_Id,
                    "approvecnt": approvecnt,
                    "level": $scope.level,
                    "HRME_Id": $scope.HRME_Id,
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("TADAAdvanceApproval/savedta", data).then(function (promise) {
                    if (promise.returnvalue === true) {
                        if ($scope.VTADAAA_TotalSactionedAmount > 0) {
                            swal('TADA Advance Approved Successfully');
                        }
                        else {
                            swal('TADA Advance Rejected Successfully');
                        }
                        //if (tp == 'AR') {
                        //    if ($scope.VTADAAA_TotalSactionedAmount > 0) {
                        //        swal('TADA Advance Approved Successfully');
                        //    }
                        //    else {
                        //        swal('TADA Advance Rejected Successfully');
                        //    }

                        //}
                        //else if (tp == 'A') {
                        //    if ($scope.VTADAAA_TotalSactionedAmount > 0) {
                        //        swal('TADA Advance Approved Successfully');
                        //    }
                        //    else {
                        //        swal('TADA Advance Rejected Successfully');
                        //    }
                        //}
                        //else if (tp == 'R') {
                        //    swal('TADA Advance Rejected Successfully');
                        //}

                    }
                    else {
                        swal('Failed to save, please contact administrator');

                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };
        //=======================twoPercentage====
        $scope.perceNtagetwo = function () {
            if ($scope.Percenage_Two != null && $scope.Percenage_Two != "" && $scope.Percenage_Two > 0) {
                var a = 0; var b = 0;
                a = (Number($scope.Percenage_Two / 100));
                b = (Number($scope.aplamount) * a);
                $scope.VTADAAA_TotalSactionedAmount = Number(b).toFixed();
                $scope.countitemAmt(rowobj);
            }
            else {
                $scope.VTADAAA_TotalSactionedAmount = $scope.aplamount;
            }
        };
        //========================PercentCaculate
        $scope.perceNtage = function (rowobj) {
            if (rowobj.vtdaA_Percentage != null && rowobj.vtdaA_Percentage != "" && rowobj.vtdaA_Percentage > 0) {
                var a = 0; var b = 0;
                a = (Number(rowobj.vtdaA_Percentage / 100));
                b = (Number(rowobj.VTADAAAD_Amount) * a);
                rowobj.VTADAAAAH_SactionedAmount = Number(b).toFixed();
                $scope.countitemAmt(rowobj);
            }
            else {
                rowobj.VTADAAAAH_SactionedAmount = rowobj.VTADAAAD_Amount;
                var a = 0;
                angular.forEach($scope.transrowsedit, function (obj) {
                    a += parseFloat(obj.VTADAAAAH_SactionedAmount);
                });
                var totamt = a;
                $scope.VTADAAA_TotalSactionedAmount = totamt;
                $scope.VTADAAA_TotalSactionedAmount = parseFloat($scope.VTADAAA_TotalSactionedAmount);
                $scope.VTADAAA_TotalSactionedAmount = $scope.VTADAAA_TotalSactionedAmount.toFixed(2);
            }
        };
        $scope.radiochange = function (ee) {
            if (ee.flag === 'A') {
                ee.VTADAAAAH_SactionedAmount = ee.VTADAAAD_Amount;
                $scope.VTADAAA_TotalSactionedAmount = parseFloat($scope.VTADAAA_TotalSactionedAmount) + parseFloat(ee.VTADAAAAH_SactionedAmount);
                ee.classname = 'neww';
            }
            else {
                $scope.VTADAAA_TotalSactionedAmount = parseFloat($scope.VTADAAA_TotalSactionedAmount) - parseFloat(ee.VTADAAAAH_SactionedAmount);
                ee.VTADAAAAH_SactionedAmount = 0;
                ee.classname = 'oldd';
            }
        }
        //================== Count Amount
        $scope.countitemAmt = function (rowobj) {
            if (parseFloat(rowobj.VTADAAAD_Amount) > parseFloat(rowobj.VTADAAAAH_SactionedAmount)) {
                var a = 0;
                angular.forEach($scope.transrowsedit, function (obj) {
                    a += parseFloat(obj.VTADAAAAH_SactionedAmount);
                });
                var totamt = a;
                $scope.VTADAAA_TotalSactionedAmount = totamt;
                $scope.VTADAAA_TotalSactionedAmount = parseFloat($scope.VTADAAA_TotalSactionedAmount);
                $scope.VTADAAA_TotalSactionedAmount = $scope.VTADAAA_TotalSactionedAmount.toFixed(2);
            }
            else {
                swal('Sanction Amount should not exceed Actual Amount');
                rowobj.VTADAAAAH_SactionedAmount = parseFloat(rowobj.VTADAAAD_Amount);
            }
        };
        //=====================Editrecord....
        $scope.edit = function (editid) {
            $scope.transrowsedit = [];
            $scope.MI_Id = editid.MI_Id;
            $scope.VTADAAA_Id = editid.VTADAAA_Id;
            $scope.cname = editid.MI_Name;
            $scope.staffn = editid.EmpName;
            $scope.cityname = editid.CityName;
            $scope.clientname = editid.ClientName;
            $scope.inddate = new Date(editid.VTADAAA_AppliedDate);
            $scope.frmdate = new Date(editid.VTADAAA_FromDate);
            $scope.todate = new Date(editid.VTADAAA_ToDate);
            var date2 = new Date(editid.VTADAAA_FromDate);
            var date1 = new Date(editid.VTADAAA_ToDate);
            var timeDiff = Math.abs(date1.getTime() - date2.getTime());
            var DateDifference = Math.ceil(timeDiff / (1000 * 3600 * 24) + 1);
            $scope.daydiffrence = DateDifference;
            $scope.toadress = editid.VTADAAA_ToAddress;
            $scope.remarks = editid.VTADAAA_Remarks;
            $scope.aplamount = editid.VTADAAA_TotalAppliedAmount;
            $scope.level = editid.SanctionLevelNo;
            $scope.HRME_Id = editid.HRME_Id;
            $scope.transrowsedit = [];
            $scope.VTADAAA_TotalSactionedAmount = editid.VTADAAAA_SactionedAmount;
            $scope.prflag = false;
            $scope.editS = true;
            var pi_id = editid.INVMPI_Id;
            var data = {
                "VTADAAA_Id": editid.VTADAAA_Id,
                "MI_Id": editid.MI_Id,
            };
            apiService.create("TADAAdvanceApproval/edit", data).then(function (promise) {
                //  = 60               
                $scope.editarray = promise.editArray;
                if (promise.multipleclientlist != null && promise.multipleclientlist.length > 0) {
                    $scope.multipleclientlist = promise.multipleclientlist;
                }
                if (promise.timeArray != null && promise.timeArray.length > 0) {
                    if (promise.timeArray[0].vtadaaA_DepartureTime != null && promise.timeArray[0].vtadaaA_ArrivalTime != null) {
                        $scope.Departure = moment(promise.timeArray[0].vtadaaA_DepartureTime, 'HH:mm a').format();
                        $scope.Ariival = moment(promise.timeArray[0].vtadaaA_ArrivalTime, 'HH:mm a').format();
                    }
                    else {
                        $scope.Departure = "";
                        $scope.Ariival = "";
                    }
                    $scope.clientname = promise.timeArray[0].vtadaaA_ClientMultiple;

                }
                else {
                    $scope.Departure = "";
                    $scope.Ariival = "";
                }
                if (promise.editArray != null && promise.editArray.length > 0) {
                    if (promise.editArray[0].HCNT > 0) {
                        angular.forEach($scope.editarray, function (rr) {
                            var flag = '';
                            var classname = '';
                            if (rr.VTADAAAAH_StatusFlg == 'Rejected') {
                                flag = 'R';
                                classname = 'oldd';
                            } else {
                                flag = 'A';
                                classname = 'neww';
                            }
                            if (rr.VTADAAAD_ExpenditureHead != null && rr.VTADAAAD_ExpenditureHead != "") {
                                $scope.transrowsedit.push({
                                    VTADAAAD_Id: rr.VTADAAAD_Id,
                                    VTADAAA_Id: rr.VTADAAA_Id,
                                    VTADAAAD_ExpenditureHead: rr.VTADAAAD_ExpenditureHead,
                                    VTADAAAD_Amount: rr.VTADAAAD_Amount,
                                    //VTADAAAD_Remarks: rr.VTADAAAD_Remarks,
                                    VTADAAAAH_SactionedAmount: rr.VTADAAAAH_SactionedAmount,
                                    classname: classname, flag: flag,
                                    VTADACM_FoodAmt: rr.VTADACM_FoodAmt,
                                    VTADACM_AccommodationAmt: rr.VTADACM_AccommodationAmt,
                                    VTADACM_TransportAmt: rr.VTADACM_TransportAmt,
                                    VTADAAAD_Slots: rr.VTADAAAD_Slots,
                                    VTADAAAD_TotalSlots: rr.VTADAAAD_TotalSlots
                                });
                            }


                        })
                        $scope.flag = false;
                    }
                    else {
                        $scope.flag = true;
                    }
                    $scope.TravelHour = promise.editArray[0].VTADACM_TravelHour;

                }
                
            });
        };
        $scope.cancel = function () {
            $scope.submitted = false;
            $state.reload();
            $scope.tablediv = false;
        }
        //extra added
        $scope.getView = function (VTADAAA_Id) {
            var data = {
                // "VTADAAA_Id": VTADAAA_Id
                "VTADAAA_Id": $scope.VTADAAA_Id,
            };
            apiService.create("TADA_AdavanceApply_Payment/Viewdetails", data).then(function (promise) {
                $scope.ViewDetails = promise.veiwdetails;
                if ($scope.ViewDetails != null && $scope.ViewDetails.length > 0) {
                    if ($scope.ViewDetails[0].VTADAAD_ExpenditureHead != "" && $scope.ViewDetails[0].VTADAAD_ExpenditureHead != null) {
                        $scope.flagone = true;
                        $scope.flagtwo = false
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
                                ApprovedBy: dd.ApprovedBy,
                                VTADAAAA_Remarks: dd.VTADAAAA_Remarks
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
                                    ApprovedBy: dd.ApprovedBy,
                                    VTADAAAA_Remarks: dd.VTADAAAA_Remarks
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


        };
    }
})();

