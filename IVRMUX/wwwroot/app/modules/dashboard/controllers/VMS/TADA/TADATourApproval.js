(function () {
    'use strict';
    angular
        .module('app')
        .controller('TADATourApprovalController', TADAAdvanceApprovalController)
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
        $scope.itemsPerPage4 = 8;
        $scope.currentPage3 = 1;
        $scope.itemsPerPage3 = 5;

        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.getReport = [];
        $scope.txt_approxTotAmount = 0;
        $scope.searchValue = "";
        $scope.searchtwo = "";
        $scope.searchthree = "";
        $scope.flag = false;
        $scope.currentPage4 = 1;
        $scope.maxDatedof = new Date($scope.today);
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian; todate
        };
        $scope.ismeridian = true;
        $scope.AddLeadFlag = false;

        $scope.hstep = 1;
        $scope.mstep = 1
        $scope.loaddata = function () {
            var pageid = 1;
            apiService.getURI("TADAAdvanceApproval/getTourApproval", pageid).
                then(function (promise) {
                    $scope.getadvancetada = promise.getadvancetada;
                    if ($scope.getadvancetada != null && $scope.getadvancetada.length > 0) {
                        $scope.prflag = true;
                        $scope.pppfl = false;
                        angular.forEach($scope.getadvancetada, function (li) {
                            angular.forEach(promise.tourPlanDetails, function (lii) {
                                if (li.IER_ID == lii.ieR_ID) {
                                    li.IER_Plan_StartDate = lii.ieR_Plan_StartDate;
                                    li.IER_Plan_EndDate = lii.ieR_Plan_EndDate;
                                    li.IER_PlanName = lii.ieR_PlanName;
                                    li.IER_Remarks = lii.ieR_Remarks;

                                }

                            });
                        });
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
                            if (li.flag === 'A' && $scope.VTADAAA_TotalSactionedAmount > 0) {
                                approvecnt += 1;
                            }
                            else {
                                li.flag = "R";

                            }
                        }

                    });
                }
                $scope.Datacategorysave = [];
                if ($scope.employeeid != null && $scope.employeeid.length > 0 && $scope.AddLeadFlag == true) {
                    angular.forEach($scope.employeeid, function (dev) {
                        angular.forEach(dev.Datacategorynew, function (dd) {
                            if (dev.Datacategorynew != null && dev.Datacategorynew.length > 0) {
                                $scope.Datacategorysave.push({
                                    ClientId: dd.ISMSLE_Id,
                                    CategoryId: dev.IMRC_CategoryId,
                                    ISMSMPR_Id: dd.ISMSMPR_Id
                                })
                            }

                        })
                    })
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
                    "PlanStartDate": new Date($scope.planstartdate).toDateString(),
                    "PlanEndDate": new Date($scope.planenddate).toDateString(),
                    "listEmployeeSales": $scope.Datacategorysave,
                    "IER_ID": $scope.IER_IDNew,
                    "PlanName": "",
                    "TourRemark": "",
                    "AdminFlag": $scope.AddLeadFlag
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                //TourApproval
                apiService.create("TADAAdvanceApproval/TourApproval", data).then(function (promise) {
                    if (promise.returnvalue === true) {
                        if ($scope.VTADAAA_TotalSactionedAmount > 0) {
                            swal('Tour  Approved Successfully');
                        }
                        else {
                            swal('Tour Rejected Successfully');
                        }

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
            $scope.transrowsedit = []; $scope.Totaltravelamt = ""; $scope.Totalaplamt = "";
            $scope.getArrayDc = [];
            $scope.sourcelist = []; $scope.getReport = [];
            $scope.PlanerName = "";
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
            $scope.prflag = false;
            $scope.editS = true;
            var pi_id = editid.INVMPI_Id;
            $scope.IER_Remarks = "";
            $scope.PlanerName = editid.IER_PlanName;
            $scope.IER_Remarks = editid.IER_Remarks;
            var data = {
                "VTADAAA_Id": editid.VTADAAA_Id,
                "MI_Id": editid.MI_Id,
                "IER_ID": editid.IER_ID,
                "HRME_Id": editid.HRME_Id
            };
            apiService.create("TADAAdvanceApproval/editTour", data).then(function (promise) {

                $scope.editarray = promise.editArray;
                if (promise.multipleclientlist != null && promise.multipleclientlist.length > 0) {
                    $scope.multipleclientlist = promise.multipleclientlist;
                }
                if (promise.getReport != null && promise.getReport.length > 0) {
                    $scope.getReport = promise.getReport;
                    $scope.Totaltravelamt = 0; $scope.Totalaplamt = 0;
                    angular.forEach($scope.getReport, function (d) {
                        $scope.Totaltravelamt = (Number($scope.Totaltravelamt) + (Number(d.Totalamount)));
                        $scope.Totalaplamt = (Number($scope.Totalaplamt) + (Number(d.Appliedamount)));
                    });
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
                                    // VTADAAAAH_SactionedAmount: rr.VTADAAAAH_SactionedAmount,
                                    classname: classname, flag: flag,
                                    VTADACM_FoodAmt: rr.VTADACM_FoodAmt,
                                    VTADACM_AccommodationAmt: rr.VTADACM_AccommodationAmt,
                                    VTADACM_TransportAmt: rr.VTADACM_TransportAmt,
                                    VTADAAAD_Slots: rr.VTADAAAD_Slots,
                                    VTADAAAD_TotalSlots: rr.VTADAAAD_TotalSlots,
                                    vtdaA_Percentage: 80,
                                    VTADAAAAH_SactionedAmount: ((Number(rr.VTADAAAD_Amount) / 100) * 80).toFixed(2)
                                });
                            }

                        })
                        $scope.flag = false;
                    }
                    else {
                        $scope.flag = true;
                    }
                    // $scope.TravelHour = promise.editArray[0].VTADACM_TravelHour;
                    //$scope.countitemAmt(promise.editArray);
                    var totamt = 0;
                    angular.forEach($scope.transrowsedit, function (obj) {
                        totamt += parseFloat(obj.VTADAAAAH_SactionedAmount);
                    });

                    $scope.VTADAAA_TotalSactionedAmount = totamt.toFixed(2);
                }
                //EditArrayTwo
                if (promise.editArrayTwo != null && promise.editArrayTwo.length > 0) {
                    $scope.EditArrayTwo = promise.editArrayTwo;
                }
                if (promise.getadvancetada != null && promise.getadvancetada.length > 0) {
                    $scope.CategoryParameters = promise.getadvancetada;
                    $scope.CatParameters = [];
                    angular.forEach($scope.CategoryParameters, function (dev) {
                        if ($scope.CatParameters.length === 0) {
                            $scope.CatParameters.push(dev);
                        }
                        else if ($scope.CatParameters.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.CatParameters, function (emp) {
                                if (emp.IMRC_CategoryId === dev.IMRC_CategoryId) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.CatParameters.push(dev);
                            }
                        }
                    })
                    angular.forEach($scope.CatParameters, function (ddd) {
                        $scope.tempPARA = [];
                        angular.forEach($scope.CategoryParameters, function (dd) {
                            if (dd.IMRC_CategoryId === ddd.IMRC_CategoryId) {
                                $scope.tempPARA.push(dd);
                            }
                        });
                        ddd.DaTAPARA = $scope.tempPARA;
                    });
                }


                if (promise.getArray != null && promise.getArray.length > 0) {
                    $scope.getArrayDc = promise.getArray;
                }
                if (promise.client_Master != null && promise.client_Master.length > 0) {
                    $scope.sourcelist = promise.client_Master;                 
                }
                $scope.concalLopdata = []; $scope.VTADAA_BalanceConcal = ""; $scope.GpsLocationTada = [];
                if (promise.concalLopdata != null && promise.concalLopdata.length > 0) {
                    $scope.concalLopdata = promise.concalLopdata;
                }                if (promise.gpsLocationTada != null && promise.gpsLocationTada.length > 0) {                    $scope.GpsLocationTada = promise.gpsLocationTada;
                }
                //sourcelist

                //$scope.VTADAAA_TotalSactionedAmount = $scope.aplamount;

            });

            //added 
            $scope.getView(editid);
        };

        $scope.cancel = function () {
            $scope.submitted = false;
            $state.reload();
            $scope.tablediv = false;
        }
        $scope.clientnames_rem = [{ itrS_Id1: 'trans12' }];
        $scope.addgrnrowsclient = function () {
            if ($scope.clientnames_rem.length > 1) {
                for (var x = 0; x === $scope.clientnames_rem.length; x++) {
                    var id1 = $scope.clientnames_rem[x].itrS_Id1;
                    var lastChar1 = id1.substr(id1.length - 1);
                    $scope.cnt1 = parseInt(lastChar1);
                }
            }
            $scope.cnt1 = $scope.cnt1 + 1;
            $scope.tet = 'trans' + $scope.cnt1;
            var newItemNo1 = $scope.cnt1;
            $scope.clientnames_rem.push({ 'itrS_Id1': 'trans12' + newItemNo1 });
        };

        $scope.removeclient = function (index, data) {
            var newItemNo = $scope.clientnames_rem.length - 1;
            $scope.clientnames_rem.splice(index, 1);

        };
        //extra added
        $scope.getView = function (item) {
            $scope.getClient = [];
            $scope.RepeatDetails = [];
            $scope.IER_IDNew = "";
            $scope.HRME_IdNew = "";
            $scope.orderFlag = false;
            $scope.Datacategorynew = [];
            $scope.IER_IDNew = item.IER_ID;
            $scope.HRME_IdNew = item.HRME_Id;
            var data = {
                "IER_ID": item.IER_ID,
                "VTADAAA_Id": item.VTADAAA_Id,
                "HRME_Id": item.HRME_Id
            };
            apiService.create("TADAAdvanceApproval/getView", data).then(function (promise) {
                if (promise.editdataOthelist != null && promise.editdataOthelist.length > 0) {
                    angular.forEach(promise.editdataOthelist, function (dd) {
                        $scope.RepeatDetails.push({
                            IER_ID: dd.IER_ID,
                            IVRMMS_Id: dd.IVRMMS_Id,
                            IVRMMS_Name: dd.IVRMMS_Name,
                            IRPD_DepartureDate: new Date(dd.IRPD_DepartureDate),
                            IRPD_ArrivalDate: new Date(dd.IRPD_ArrivalDate),
                            IRPD_ArrivalTime: moment(dd.IRPD_ArrivalTime, 'HH:mm a').format(),
                            IRPD_DepartureTime: moment(dd.IRPD_DepartureTime, 'HH:mm a').format(),
                            IRPD_Order: dd.IRPD_Order
                        })

                    })
                    //  $('#tadadocument').modal('show');
                    //
                    $scope.GetCategoryArray = promise.getCategoryArray;
                    $scope.Datacategorynew = promise.getPlanerdata;
                    if (promise.categorywise_MinMax != null && promise.categorywise_MinMax.length > 0) {
                        $scope.Categorywise_MinMax = promise.categorywise_MinMax;
                    }
                    $scope.employeeid = [];
                    angular.forEach($scope.Datacategorynew, function (dev) {
                        if ($scope.employeeid.length === 0) {
                            $scope.employeeid.push(dev);
                        }
                        else if ($scope.employeeid.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.employeeid, function (emp) {
                                if (emp.IMRC_CategoryId === dev.IMRC_CategoryId) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.employeeid.push(dev);
                            }
                        }
                    })
                    angular.forEach($scope.employeeid, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.Datacategorynew, function (dd) {
                            if (dd.IMRC_CategoryId === ddd.IMRC_CategoryId) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.Datacategorynew = $scope.templist;

                    });
                    $scope.GetMandatary = promise.getCategoryArray;
                    $scope.getClient = promise.clientlist;
                    if (promise.multipleclientlist != null && promise.multipleclientlist.length > 0) {

                        angular.forEach(promise.multipleclientlist, function (dev) {
                            angular.forEach($scope.clientnames_rem, function (dd) {
                                dev.Client_Id = dd.ismmclT_Id;
                            });
                        });
                    }
                    if (promise.priority != null && promise.priority.length > 0) {
                        $scope.Priority = promise.priority;
                    }

                    $scope.IMRC_CategoryId = $scope.GetCategoryArray[0].IMRC_CategoryId;
                    $scope.Onchangecategory();
                    $scope.Chatagraphs = []; $scope.categoriesNames = [];
                    if ($scope.employeeid != null && $scope.employeeid.length > 0) {
                        angular.forEach($scope.employeeid, function (dev) {
                            $scope.Chatagraphs.push({
                                category: dev.IMRC_CategoryName,
                                value: dev.Datacategorynew.length,
                                color: "#c2800b"
                            })
                            $scope.categoriesNames.push([dev.IMRC_CategoryName])
                        });
                    }


                    $scope.GrpahsData();
                }
                else {
                    swal("Records Not  Found !");
                }

                $scope.planstartdate = new Date(item.IER_Plan_StartDate);
                $scope.planenddate = new Date(item.IER_Plan_EndDate);

            });


        }
        $scope.Chatagraphs = []; $scope.categoriesNames = [];
        $scope.GrpahsData = function () {


            $("#donutchart").kendoChart({
                title: {
                    position: "bottom",
                    text: "Category Details"
                },
                legend: {
                    visible: false
                },
                chartArea: {
                    background: ""
                },
                seriesDefaults: {
                    type: "column",
                    startAngle: 60
                },
                series: [{
                    data: $scope.Chatagraphs
                },

                {
                    labels: {
                        visible: true,
                        background: "transparent",
                        position: "outsideEnd",

                    }
                    }],
                categoryAxis: {
                  //  categories: ["OpenCount", "ClosedCount", "InprogressCount ", "CompletedCount",],
                    categories: $scope.categoriesNames,
                    line: {
                        visible: false
                    },
                    labels: {
                        padding: { top: 5 }
                    }
                },
                tooltip: {
                    visible: true,
                    template: "#= category # : #= value #"
                }
            });

        }
        $scope.sortKey = 'ismS_OrderFlag';
        $scope.sortReverse = false;
        $scope.sortableOptions = {

            stop: function (e, ui) {
                for (var index in $scope.RepeatDetails) {
                    $scope.RepeatDetails[index].ismS_OrderFlag = Number(index) + 1;
                    $scope.RepeatDetails[index].IRPD_Order = Number(index) + 1;
                }
                $scope.orderFlag = false;
            }
            //


        };
        $scope.orderFlag = false;
        $scope.sortableNumber = function () {
            var count = 0;
            $scope.orderFlag = true;
            angular.forEach($scope.RepeatDetails, function (dd) {
                dd.IRPD_Order = "";
                count = count + 1;
                dd.IRPD_Order = count;
            })

        }
        //mailView

        $scope.Onchangecategory = function () {
            $scope.LeadList = [];
            if ($scope.IMRC_CategoryId > 0) {
                var data = {
                    "IMRC_CategoryId": $scope.IMRC_CategoryId,
                    "HRME_Id": $scope.HRME_IdNew
                };
                apiService.create("TADAAdvanceApproval/TadaAllwence", data).then(function (promise) {
                    $scope.LeadList = promise.leadList;

                    angular.forEach($scope.LeadList, function (dd) {
                        dd.classname = "";
                        dd.verfied = false;
                    });

                });
            }

        };
        //AddToCart
        $scope.ISMSLE_IdNew = "";
        $scope.AddToCart = function () {
            $scope.Datacategorytemp = [];
            angular.forEach($scope.LeadList, function (dd) {
                if (dd.checked == true && dd.verfied == false) {
                    //  dd.verfied = true;
                    $scope.Datacategorytemp.push({
                        ISMSLE_Id: dd.ISMSLE_Id,
                        ISMSLE_LeadName: dd.ISMSLE_LeadName,
                        ISMSLE_StudentStrength: dd.ISMSLE_StudentStrength,
                        ISMSLE_StaffStrength: dd.ISMSLE_StaffStrength,
                        ISMSMPR_ProductName: dd.ISMSMPR_ProductName,
                        SourceName: dd.SourceName,
                        IMRC_CategoryId: $scope.IMRC_CategoryId,
                        IVRMMS_Name: dd.IVRMMS_Name,
                        IMRC_CategoryName: $scope.IMRC_CategoryName,
                        ISMSMPR_Id: dd.ISMSMPR_Id,
                        IVRMMS_Id: dd.IVRMMS_Id,
                        NewFlagaddtion: true,
                        classname: 'oldd',

                    });
                }
            });

            //Datacategorynew
            if ($scope.Datacategorytemp != null && $scope.Datacategorytemp.length > 0) {
                angular.forEach($scope.employeeid, function (dd) {
                    angular.forEach($scope.Datacategorytemp, function (dev) {
                        if (dd.IMRC_CategoryId == dev.IMRC_CategoryId) {
                            dd.Datacategorynew.push(dev);
                        }
                    });
                });
                $scope.AddLeadFlag = true;
                $scope.LeadList = [];
                $scope.IMRC_CategoryId = "";
                $('#salesView').modal('hide');
            }
            else {
                swal("Record Not Found / Already Added Lead Name !");
            }

        }
        //$('#salesView').modal('hide');
        $scope.OnChangeBOMList = function () {
            $scope.bomTemplateList = []; $scope.ismslE_EmailId = "";
            $scope.ismslE_LeadName = ""; $scope.ismsmpR_ProductName = "";
            $scope.ismslE_ContactDesignation = ""; $scope.addres = ""; $scope.ismslE_ContactNo = ""; $scope.ismslE_ContactNo = "";
            $scope.ismslE_ContactPerson = ""; $scope.ismslE_EmailId = ""; $scope.fmscoR_RefernceNo = ""; $scope.hrmE_EmployeeFirstName = "";
            $scope.ismspS_Service_Name = ""; $scope.ismsboM_BOMName = ""; $scope.ismslE_StudentStrength = ""; $scope.asmaY_Year = "";
            $scope.ismspsI_ItemsDescription = ""; $scope.amt = ""; $scope.amt_4 = ""; $scope.ismslE_Id = "";
            var BOMLists = [];
            angular.forEach($scope.bomList, function (d) {
                if (d.ISMSBOM_Id == $scope.ISMSBOM_Id) {
                    //d.ISMSBOM_Id = 50;
                    BOMLists.push(d.ISMSBOM_Id);
                }
            });

            var data = {
                "BOMListt": BOMLists,
                "ISMSLE_Id": $scope.ISMSLE_IdNew,
                // "ISMSLE_Id": 8041,
            };
            apiService.create("Lead_Target_Mapping/OnChangeOnChangeBOMList", data).then(function (promise) {

                if (promise.bomTemplateList != null && promise.bomTemplateList.length > 0) {
                    $scope.bomTemplateList = promise.bomTemplateList;
                    $scope.sales_CostPerMonth = promise.sales_CostPerMonth;
                    $scope.taskid5 = [];
                    angular.forEach($scope.bomTemplateList, function (dev) {
                        if ($scope.taskid5.length === 0) {
                            $scope.taskid5.push({
                                ISMSPS_ID: dev.ISMSPS_ID,
                                ISMSPS_Service_Name: dev.ISMSPS_Service_Name,
                                ISMSPSI_ID: dev.ISMSPSI_ID,

                            });
                        }
                        else if ($scope.taskid5.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.taskid5, function (emp) {
                                if (emp.ISMSPS_ID === dev.ISMSPS_ID) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.taskid5.push({
                                    ISMSPS_ID: dev.ISMSPS_ID,
                                    ISMSPS_Service_Name: dev.ISMSPS_Service_Name,
                                    ISMSPSI_ID: dev.ISMSPSI_ID,
                                });
                            }
                        }
                    });
                    angular.forEach($scope.taskid5, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.bomTemplateList, function (dd) {
                            if (dd.ISMSPS_ID === ddd.ISMSPS_ID) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.bomTemplateList = $scope.templist;

                    });
                    $scope.ismslE_LeadName = $scope.bomTemplateList[0].ISMSLE_LeadName;
                    $scope.ismsmpR_ProductName = $scope.bomTemplateList[0].ISMSMPR_ProductName;
                    $scope.ismslE_ContactDesignation = $scope.bomTemplateList[0].ISMSLE_ContactDesignation;
                    $scope.addres = $scope.bomTemplateList[0].addres;
                    $scope.ismslE_ContactNo = $scope.bomTemplateList[0].ISMSLE_ContactNo;
                    $scope.ismslE_ContactPerson = $scope.bomTemplateList[0].ISMSLE_ContactPerson;
                    $scope.ismslE_EmailId = $scope.bomTemplateList[0].ISMSLE_EmailId;
                    $scope.fmscoR_RefernceNo = $scope.bomTemplateList[0].FMSCOR_RefernceNo;
                    $scope.hrmE_EmployeeFirstName = $scope.bomTemplateList[0].HRME_EmployeeFirstName;
                    $scope.ismspS_Service_Name = $scope.bomTemplateList[0].ISMSPS_Service_Name;
                    $scope.ismsboM_BOMName = $scope.bomTemplateList[0].ISMSBOM_BOMName;
                    $scope.ismslE_StudentStrength = $scope.bomTemplateList[0].ISMSLE_StudentStrength;
                    $scope.asmaY_Year = $scope.bomTemplateList[0].ASMAY_Year;
                    $scope.ismspsI_ItemsDescription = $scope.bomTemplateList[0].ISMSPSI_ItemsDescription;
                    $scope.ismspsI_ItemsDescription = $scope.bomTemplateList[0].ISMSPSI_ItemsDescription;

                    $scope.amt = $scope.sales_CostPerMonth[0].AMt;
                    $scope.amt_4 = $scope.sales_CostPerMonth[0].AMt_4;
                    $scope.ismslE_Id = $scope.bomTemplateList[0].ISMSLE_Id;


                }

                else {
                    swal('Praposal Details Not Updated ! ');

                }


            });
        };

        $scope.OnChangelead = function (user) {

            $scope.bomList = [];
            $scope.ISMSBOM_Id = "";
            var products = [];
            angular.forEach($scope.employeeid, function (d) {
                if (d.ISMSLE_Id == user.ISMSLE_Id) {
                    products.push(d.ISMSMPR_Id);
                }
            });
            $scope.ISMSLE_IdNew = user.ISMSLE_Id;
            var data = {
                "product": products,
                "ISMSLE_Id" : 0
            };
            apiService.create("Lead_Target_Mapping/OnChangeOnChangeproductlist", data).then(function (promise) {

                if (promise.bomList != null && promise.bomList.length > 0) {
                    $scope.bomList = promise.bomList;
                    $('#mailView').modal('show');
                    $scope.ISMSBOM_Id = $scope.bomList[0].ISMSBOM_Id;
                    $scope.OnChangeBOMList();
                }

                else {
                    swal('No Data Found ');

                }

            });

        };
        //OnChangelead

        $scope.Update = function () {
            if ($scope.IER_IDNew > 0) {
                $scope.Datacategorysave = [];


                angular.forEach($scope.employeeid, function (dev) {
                    angular.forEach(dev.Datacategorynew, function (dd) {
                        if (dev.Datacategorynew != null && dev.Datacategorynew.length > 0) {
                            $scope.Datacategorysave.push({
                                ClientId: dd.ISMSLE_Id,
                                CategoryId: dev.IMRC_CategoryId,
                                ISMSMPR_Id: dd.ISMSMPR_Id
                            })
                        }

                    })
                })
                if ($scope.Datacategorysave != null && $scope.Datacategorysave.length > 0) {

                    var data = {
                        "PlanStartDate": new Date($scope.planstartdate).toDateString(),
                        "PlanEndDate": new Date($scope.planenddate).toDateString(),
                        "listEmployeeSales": $scope.Datacategorysave,
                        "IER_ID": $scope.IER_IDNew,
                        "HRME_Id": $scope.HRME_IdNew,
                        "PlanName": $scope.PlanerName,
                        "TourRemark": $scope.IER_Remarks
                    };
                    apiService.create("TADAAdvanceApproval/updateallowence", data).then(function (promise) {
                        if (promise.message == "save") {
                            swal("Records Updated Sucessfully !");
                            $scope.RepeatDetails = [];

                            angular.forEach(promise.editdataOthelist, function (dd) {
                                $scope.RepeatDetails.push({
                                    IER_ID: dd.IER_ID,
                                    IVRMMS_Id: dd.IVRMMS_Id,
                                    IVRMMS_Name: dd.IVRMMS_Name,
                                    IRPD_DepartureDate: new Date(dd.IRPD_DepartureDate),
                                    IRPD_ArrivalDate: new Date(dd.IRPD_ArrivalDate),
                                    IRPD_ArrivalTime: moment(dd.IRPD_ArrivalTime, 'HH:mm a').format(),
                                    IRPD_DepartureTime: moment(dd.IRPD_DepartureTime, 'HH:mm a').format(),
                                    IRPD_Order: dd.IRPD_Order
                                })

                            })
                        }
                        else if (promise.message == "notsave") {
                            swal("Records Not Saved  !");
                        }
                        else if (promise.message == "admin") {
                            swal("Please Contact Administrator !");
                        }
                        else {
                            swal("Records Not Saved  !");
                        }

                    });
                }
                else {
                    swal("Plaese Select Category !");
                }
                //

            }

        }
        $scope.RemoveY = function (index, user) {
            angular.forEach($scope.employeeid, function (dev) {
                if (user.IMRC_CategoryId === dev.IMRC_CategoryId) {
                    var newItemNo = dev.Datacategorynew.length - 1;
                    dev.Datacategorynew.splice(index, 1);
                    return;
                }
            })

            //var newItemNo = $scope.Datacategory.length - 1;
            //$scope.Datacategory.splice(index, 1);
        }

        $scope.Calculate = function () {
            $scope.submitted1 = true;
            if ($scope.myForm2.$valid) {
                $scope.RepeatDetailsTemp = [];
                if ($scope.RepeatDetails != null && $scope.RepeatDetails.length > 0) {
                    angular.forEach($scope.RepeatDetails, function (dd) {
                        $scope.srresult = [];
                        $scope.erresult = [];
                        var IRPD_DepartureTime = dd.IRPD_DepartureTime;
                        var IRPD_ArrivalTime = dd.IRPD_ArrivalTime;

                        $scope.sresult = $filter('date')(IRPD_DepartureTime, 'HH:mm');
                        $scope.eresult = $filter('date')(IRPD_ArrivalTime, 'HH:mm');

                        $scope.RepeatDetailsTemp.push({
                            IER_ID: dd.IER_ID,
                            IVRMMS_Id: dd.IVRMMS_Id,
                            IRPD_DepartureDate: new Date(dd.IRPD_DepartureDate).toDateString(),
                            IRPD_ArrivalDate: new Date(dd.IRPD_ArrivalDate).toDateString(),
                            IRPD_DepartureTime: $scope.sresult,
                            IRPD_ArrivalTime: $scope.eresult,
                            IRPD_Order: dd.IRPD_Order
                        })

                    })
                }
                var data = {
                    "Plan_DepartureDetails": $scope.RepeatDetailsTemp,
                    "IER_ID": $scope.IER_IDNew

                };
                apiService.create("TADAAdvanceApproval/CalculateTada", data).then(function (promise) {

                    if (promise.message == "save") {
                        swal("Record Save Successfully  !");
                        $scope.RepeatDetails = [];
                        angular.forEach(promise.editdataOthelist, function (dd) {
                            $scope.RepeatDetails.push({
                                IER_ID: dd.IER_ID,
                                IVRMMS_Id: dd.IVRMMS_Id,
                                IVRMMS_Name: dd.IVRMMS_Name,
                                IRPD_DepartureDate: new Date(dd.IRPD_DepartureDate),
                                IRPD_ArrivalDate: new Date(dd.IRPD_ArrivalDate),
                                IRPD_ArrivalTime: moment(dd.IRPD_ArrivalTime, 'HH:mm a').format(),
                                IRPD_DepartureTime: moment(dd.IRPD_DepartureTime, 'HH:mm a').format(),
                                IRPD_Order: dd.IRPD_Order
                            })

                        })
                        //
                    }
                    else if (promise.message == "notsave") {
                        swal("Record  Not saved !");
                    }
                    else if (promise.message == "admin") {
                        swal("Plaese Conact Administrtor !");
                    }
                });
            }
        }

        // $('#salesView').modal('show');
        //added Graphs

        $scope.previewimg_new1 = function (img) {
            $scope.ISMERCP_Filepath = img;
            var img = $scope.ISMERCP_Filepath;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.ISMERCP_Filepath);
                    $('#myimagePreview').modal('show');
                }
                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {

                    $window.open($scope.ISMERCP_Filepath);
                }
                else if (lastelement == 'pdf') {
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.ISMERCP_Filepath;
                    $http.get(imagedownload1, { responseType: 'arraybuffer' })
                        .success(function (response) {
                            var fileURL = "";
                            var file = "";
                            var embed = "";
                            var pdfId = "";
                            file = new Blob([(response)], { type: 'application/pdf' });
                            fileURL = URL.createObjectURL(file);
                            pdfId = document.getElementById("pdfIdzz");
                            pdfId.removeChild(pdfId.childNodes[0]);
                            embed = document.createElement('embed');
                            embed.setAttribute('src', fileURL);
                            embed.setAttribute('type', 'application/pdf');
                            embed.setAttribute('width', '100%');
                            embed.setAttribute('height', '1000');
                            pdfId.appendChild(embed);
                            $('#showpdf').modal('show');
                        });
                }
            }
            else {
                $window.open($scope.ISMERCP_Filepath)
            }
        };


    }
})();

