(function () {
    'use strict';
    angular
        .module('app')
        .controller('TADAFinalApprovalController', TADAFinalApprovalController)
    TADAFinalApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache', '$window','intracontroller']
    function TADAFinalApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache, $window, intracontroller) {
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
        $scope.searchthree = "";
        $scope.itemsPerPage4 = 5;
        $scope.currentPage4 = 1;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.Chatagraphs = []; $scope.categoriesNames = [];
        $scope.searchValue = '';
        $scope.maxDatedof = new Date($scope.today);
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian; todate
        };
        $scope.transrowsedit = [];
        $scope.VTADAA_BalanceHRMS = "";
        // scope.tablediv = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.loaddata = function () {
            var pageid = 1;
            apiService.getURI("TADAApproval/getfinaldata", pageid).
                then(function (promise) {
                    $scope.getadvancetada = promise.getadvancetada;
                    if ($scope.getadvancetada != null && $scope.getadvancetada.length > 0) {
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
                $scope.VTADAA_TotalSactionedAmount = 0;

            }
            else if ($scope.VTADAA_BalanceHRMS > 0) {
                approvecnt = 1;


            }
            $scope.submitted = true;
            var data = {};
            if ($scope.myForm.$valid) {
                if ($scope.transrowsedit != null && $scope.transrowsedit.length > 0) {
                    angular.forEach($scope.transrowsedit, function (li) {
                        if (li.flag === 'A') {
                            approvecnt += 1;
                        }
                    });
                }

                data = {
                    "VTADAA_Remarks": $scope.VTADAA_Remarks,
                    "VTADAA_TotalSactionedAmount": $scope.VTADAA_TotalSactionedAmount,
                    "headarray": $scope.transrowsedit,
                    "VTADAA_Id": $scope.VTADAA_Id,
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
                apiService.create("TADAApproval/savefinaldta", data).then(function (promise) {

                    if (promise.returnvalue === true) {
                        if ($scope.VTADAA_BalanceHRMS > 0) {
                            swal('TADA  Approved Successfully');
                        }
                        else if ($scope.VTADAA_TotalSactionedAmount > 0) {
                            swal('TADA  Approved Successfully');
                        }

                        else {
                            swal('TADA  Rejected Successfully');
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
        $scope.radiochange = function (ee) {

            if (ee.flag === 'A') {
                ee.VTADAAAH_SactionedAmount = ee.VTADAAD_Amount;
                $scope.VTADAA_TotalSactionedAmount = parseFloat($scope.VTADAA_TotalSactionedAmount) + parseFloat(ee.VTADAAAH_SactionedAmount);
                ee.classname = 'neww';
            }
            else {
                $scope.VTADAA_TotalSactionedAmount = parseFloat($scope.VTADAA_TotalSactionedAmount) - parseFloat(ee.VTADAAAH_SactionedAmount);
                ee.VTADAAAH_SactionedAmount = 0;
                ee.classname = 'oldd';
            }
        }
        $scope.save_Finaltour = function (IER_ID) {
            //TADA_Advance_Tour
            $scope.VTADAA_TotalAppliedAmount = 0; $scope.VTADAA_BalanceHRMS = 0;
            $scope.leadLists = [];
            $scope.getArrayDc = []; $scope.EditArrayTwo = []; $scope.TourPlanDetails = []; $scope.FinalSumbmission = []
            $scope.ApprovalSubmission = [];
            var data = {                "IER_ID": IER_ID,            };            apiService.create("TADA_Advance_Tour/FinalSubMission", data).then(function (promise) {                if (promise.getArray != null && promise.getArray.length > 0) {                    $scope.getArrayDc = promise.getArray;
                }                if (promise.editArrayTwo != null && promise.editArrayTwo.length > 0) {
                    $scope.EditArrayTwo = promise.editArrayTwo;
                }                if (promise.tourPlanDetails != null && promise.tourPlanDetails.length > 0) {                    $scope.TourPlanDetails = promise.tourPlanDetails;
                }                //getloadarray                if (promise.getloadarray != null && promise.getloadarray.length > 0) {
                    $scope.materaldocuuploadNew = promise.getloadarray;
                    angular.forEach($scope.materaldocuuploadNew, function (dd) {
                        dd.ilrT_FileName = dd.ILRT_FileName;
                        dd.ilrT_FilePath = dd.ILRT_FilePath;
                        dd.ilrT_Remarks = dd.ILRT_Remarks;
                        dd.ilrT_Cost = dd.ILRT_Cost;
                        dd.editflag = true;
                    });
                }                $scope.concalLopdata = []; $scope.VTADAA_BalanceConcal = ""; $scope.GpsLocationTada = [];
                if (promise.concalLopdata != null && promise.concalLopdata.length > 0) {
                    $scope.concalLopdata = promise.concalLopdata;
                    $scope.VTADAA_BalanceConcal = (Number($scope.concalLopdata[0].TotalSactionedAmount) - (Number($scope.concalLopdata[0].TotalEligbleAmount)));
                }                if (promise.gpsLocationTada != null && promise.gpsLocationTada.length > 0) {                    $scope.GpsLocationTada = promise.gpsLocationTada;
                }                //LeadLists                if (promise.leadLists != null && promise.leadLists.length > 0) {                    $scope.leadLists = promise.leadLists;
                    $scope.employeeidNew = [];

                    angular.forEach($scope.leadLists, function (dev) {
                        if ($scope.employeeidNew.length === 0) {
                            $scope.employeeidNew.push(dev);
                        }
                        else if ($scope.employeeidNew.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.employeeidNew, function (emp) {
                                if (emp.IMRC_CategoryId === dev.IMRC_CategoryId) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.employeeidNew.push(dev);
                            }
                        }
                    })
                    angular.forEach($scope.employeeidNew, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.leadLists, function (dd) {
                            if (dd.IMRC_CategoryId === ddd.IMRC_CategoryId) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.Datacategorynew = $scope.templist;

                    });
                }                $scope.categoriesNames = []; $scope.Chatagraphs = [];                //FinalSumbmission //ApprovalSubmission                if (promise.finalSumbmission != null && promise.finalSumbmission.length > 0) {                    $scope.FinalSumbmission = promise.finalSumbmission;
                    angular.forEach(promise.finalSumbmission, function (dev) {
                        $scope.Chatagraphs.push({
                            category: dev.IMRC_CategoryName,
                            value: dev.ExecutionLeadCount,
                            color: "#3eaee2"
                        })
                        //#3eaee2
                        //$scope.Chatagraphs.push({
                        //    category: dev.IMRC_CategoryName,
                        //    value: dev.ExecutionLeadCount,
                        //    color: "#3eaee2"
                        //})
                        $scope.categoriesNames.push([dev.IMRC_CategoryName])
                    });
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
                }                if (promise.approvalSubmission != null && promise.approvalSubmission.length > 0) {                    $scope.ApprovalSubmission = promise.approvalSubmission;
                    $scope.VTADAA_TotalAppliedAmount = promise.approvalSubmission[0].EligibleAmtToApply;
                    $scope.VTADAA_BalanceHRMS = promise.approvalSubmission[0].SalaryCreditAmt;
                    if ($scope.VTADAA_BalanceHRMS > 0) {

                    }
                    else {
                        $scope.VTADAA_TotalAppliedAmount = (Number(promise.approvalSubmission[0].TotalAmt) - (Number(promise.approvalSubmission[0].SactionedAmt))).toFixed(2);
                    }

                }                var counttotal1 = 0;
                var counttotal2 = 0;
                var counttotal3 = 0;
                $scope.TotalAppliedAmount = ""; $scope.TotalSactionedAmount = ""; $scope.TotalEligbleAmount = "";
                if ($scope.FinalSumbmission != null && $scope.FinalSumbmission.length > 0) {
                    angular.forEach($scope.FinalSumbmission, function (d) {

                        counttotal1 += d.VTADAAA_TotalAppliedAmount;
                        counttotal2 += d.VTADAAA_TotalSactionedAmount;
                        counttotal3 += d.VTADAAA_TotalEligbleAmount;

                    });
                    //$scope.TotalAppliedAmount = counttotal1
                    //$scope.TotalSactionedAmount = counttotal2
                    //$scope.TotalEligbleAmount = counttotal3
                    $scope.TotalAppliedAmount = Math.round(Number(counttotal1));
                    $scope.TotalSactionedAmount = Math.round(Number(counttotal2));
                    $scope.TotalEligbleAmount = Math.round(Number(counttotal3));
                }                if (promise.getAdavancedata != null && promise.getAdavancedata.length > 0) {
                    $scope.CategoryParameters = promise.getAdavancedata;
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
                }            });

        }
        //================== Count Amount
        $scope.countitemAmt = function (rowobj) {
            if (parseFloat(rowobj.VTADAAD_Amount) > parseFloat(rowobj.VTADAAAH_SactionedAmount)) {
                var a = 0;
                angular.forEach($scope.transrowsedit, function (obj) {
                    a += parseFloat(obj.VTADAAAH_SactionedAmount);
                });
                var totamt = a;
                $scope.VTADAA_TotalSactionedAmount = totamt;
                $scope.VTADAA_TotalSactionedAmount = parseFloat($scope.VTADAA_TotalSactionedAmount);
                $scope.VTADAA_TotalSactionedAmount = $scope.VTADAA_TotalSactionedAmount.toFixed(2);
            }
            else {
                swal('Sanction Amount should not exceed Actual Amount');
                rowobj.VTADAAAH_SactionedAmount = parseFloat(rowobj.VTADAAD_Amount);
            }
        };
        //=====================Editrecord....
        $scope.edit = function (editid) {
            $scope.transrowsedit = [];
            $scope.MI_Id = editid.MI_Id;
            $scope.VTADAA_Id = editid.VTADAA_Id;
            $scope.cname = editid.MI_Name;
            $scope.staffn = editid.EmpName;
            $scope.cityname = editid.CityName;
            $scope.clientname = editid.ClientName;
            $scope.inddate = new Date(editid.VTADAA_AppliedDate);
            $scope.frmdate = new Date(editid.VTADAA_FromDate);
            $scope.todate = new Date(editid.VTADAA_ToDate);
            var date2 = new Date(editid.VTADAA_FromDate);
            var date1 = new Date(editid.VTADAA_ToDate);
            var timeDiff = Math.abs(date1.getTime() - date2.getTime());
            var DateDifference = Math.ceil(timeDiff / (1000 * 3600 * 24) + 1);
            $scope.daydiffrence = DateDifference;
            $scope.toadress = editid.VTADAA_ToAddress;
            $scope.remarks = editid.VTADAA_Remarks;
            $scope.aplamount = editid.VTADAA_TotalAppliedAmount;
            $scope.level = editid.SanctionLevelNo;
            $scope.HRME_Id = editid.HRME_Id;
            $scope.transrowsedit = []; $scope.getReport = [];
            // $scope.VTADAA_TotalSactionedAmount = editid.VTADAA_TotalAppliedAmount;   
            $scope.VTADAA_TotalSactionedAmount = editid.VTADAAAA_SactionedAmount;
            $scope.prflag = false;
            $scope.editS = true;
            var pi_id = editid.INVMPI_Id;
            var VTADAAA_Id = editid.VTADAAA_Id
            var VTADAAAAA_Id = 0;
            if (editid.VTADAAA_Id != null && editid.VTADAAA_Id > 0) {
                VTADAAAAA_Id = editid.VTADAAA_Id;
            }
            else {
                VTADAAAAA_Id = 0;
            }
            var data = {
                "VTADAA_Id": editid.VTADAA_Id,
                "MI_Id": editid.MI_Id,
                "VTADAAAAA_Id": VTADAAAAA_Id
            };
            apiService.create("TADAApproval/editTada", data).then(function (promise) {
                // getfiles
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
                    if (promise.timeArray[0].vtadaA_DepartureTime != null && promise.timeArray[0].vtadaA_ArrivalTime != null) {
                        $scope.Departure = moment(promise.timeArray[0].vtadaA_DepartureTime, 'HH:mm a').format();
                        $scope.Ariival = moment(promise.timeArray[0].vtadaA_ArrivalTime, 'HH:mm a').format();
                    }
                    else {
                        $scope.Departure = "";
                        $scope.Ariival = "";
                    }
                    $scope.clientname = "";
                    $scope.clientname = promise.timeArray[0].vtadaaA_ClientMultiple;
                    if (promise.timeArray[0].vtadaA_BalanceHRMS != null && promise.timeArray[0].vtadaA_BalanceHRMS > 0) {
                        $scope.VTADAA_BalanceHRMS = promise.timeArray[0].vtadaA_BalanceHRMS;
                    }



                }
                if (promise.getFile.length != null && promise.getFile.length > 0) {
                    $scope.GetFile = promise.getFile;
                }
                else {
                    swal("Files Not Attached");
                }
                $scope.editarray = promise.editArray;
                //edit
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
                            if (rr.VTADAAD_ExpenditureHead != null && rr.VTADAAD_ExpenditureHead != "") {
                                $scope.transrowsedit.push({
                                    VTADAAD_Id: rr.VTADAAD_Id,
                                    VTADAA_Id: rr.VTADAA_Id,
                                    VTADAAD_ExpenditureHead: rr.VTADAAD_ExpenditureHead,
                                    VTADAAD_Amount: rr.VTADAAD_Amount,
                                    //VTADAAAD_Remarks: rr.VTADAAAD_Remarks,
                                    VTADAAAH_SactionedAmount: rr.VTADAAAH_SactionedAmount,
                                    classname: classname, flag: flag,
                                    VTADAAD_Slots: rr.VTADAAD_Slots,
                                    VTADAAD_TotalSlots: rr.VTADAAD_Totalslots,
                                    VTADACM_FoodAmt: rr.VTADACM_FoodAmt,
                                    VTADACM_AccommodationAmt: rr.VTADACM_AccommodationAmt,
                                    VTADACM_TransportAmt: rr.VTADACM_TransportAmt
                                });
                            }



                        })

                    }
                }
                // $scope.TravelHour = promise.editArray[0].VTADACM_TravelHour;

                //adavanace Details
                $scope.ViewDetails = promise.veiwdetails
                if ($scope.ViewDetails != null && $scope.ViewDetails.length > 0) {
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
                    $scope.totalamount = $scope.ViewDetails[0].AdvancedAppliedAmount;
                    $scope.Paidamount = $scope.ViewDetails[0].Paid_Amount;
                    $scope.balanceamount = (Number($scope.totalamount) - Number($scope.Paidamount));
                    $scope.extramount = (Number($scope.VTADAA_TotalSactionedAmount) - Number($scope.balanceamount));
                    if ($scope.extramount > 0) {
                        //swal("Extra Amount Is " + $scope.extramount + " !");

                    }
                    else {
                        $scope.extramount = "";
                    }
                    //  $scope.TravelHour = promise.editArray[0].VTADACM_TravelHour;
                }
                if (editid.IER_ID > 0) {
                    $scope.save_Finaltour(editid.IER_ID);
                }
                 
                
                
            });
        };
        ///=================DeleteRecord-----
        //Deactive
        $scope.Deletedata = function (item, SweetAlert) {
            $scope.VTADAA_Id = item.vtadaA_Id;
            var data = {
                "VTADAA_Id": $scope.VTADAA_Id
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To  Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("TADAApproval/deactive", data).
                            then(function (promise) {
                                if (promise.returnvalue == true) {
                                    if (promise.returnval === "Delete") {
                                        swal("Record delete Successfully!!!");
                                    }
                                    if (promise.returnval === "NotDelete") {
                                        swal("Record Not delete !!!");
                                    }
                                }
                                else {
                                    swal(" please contact administrator!!!");
                                }
                                $state.reload();

                            })
                    }
                    else {
                        swal("Record  Cancelled!!!");
                    }
                });
        }

        $scope.cancel = function () {

            $state.reload();

        }
        $scope.previewimg_new = function (img) {
            $scope.vtadaaF_FilePath = img;
            var img = $scope.vtadaaF_FilePath;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.vtadaaF_FilePath);
                    $('#myimagePreview').modal('show');
                }
                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {

                    $window.open($scope.vtadaaF_FilePath);
                }
                else if (lastelement == 'pdf') {
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.vtadaaF_FilePath;
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
                $window.open($scope.vtadaaF_FilePath)
            }
        };

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

        //getView
        $scope.getView = function (VTADAA_Id) {
            $scope.headreport = [];
            $scope.empwise_statuslist = [];
            var data = "";

            data = {
                "DRApprovalTypeFlag": "DRApproval",
                "VTADAA_Id": $scope.VTADAA_Id,

            };

            apiService.create("TADAApproval/GetSavedDetails", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getsaveddetails !== null && promise.getsaveddetails.length > 0) {
                        $scope.getsaveddetails = promise.getsaveddetails;
                        $scope.empwise_statuslist = promise.empwise_statuslist;

                        $scope.emp_list = [];
                        var count = 0;

                        angular.forEach($scope.getsaveddetails, function (dd) {
                            if ($scope.emp_list.length === 0) {
                                $scope.emp_list.push({
                                    HRME_Id: dd.HRME_Id, employeename: dd.EMPLOYEENAME, employeecode: dd.EMPLOYEECODE,
                                    empdept: dd.DEPTNAME, empdesig: dd.DESGNAME
                                });
                            } else if ($scope.emp_list.length > 0) {
                                count = 0;
                                angular.forEach($scope.emp_list, function (ddd) {
                                    if (ddd.HRME_Id === dd.HRME_Id) {
                                        count += 1;
                                    }
                                });

                                if (count === 0) {
                                    $scope.emp_list.push({
                                        HRME_Id: dd.HRME_Id, employeename: dd.EMPLOYEENAME, employeecode: dd.EMPLOYEECODE,
                                        empdept: dd.DEPTNAME, empdesig: dd.DESGNAME, overallremarks: ""
                                    });
                                }
                            }
                        });

                        //Emp Wise Status
                        $scope.temp_emp_status = [];

                        angular.forEach($scope.emp_list, function (emp) {
                            $scope.temp_emp_status = [];
                            angular.forEach($scope.empwise_statuslist, function (emp_status) {
                                if (emp.HRME_Id === emp_status.HRME_Id) {
                                    $scope.temp_emp_status.push(emp_status);
                                }
                            });
                            emp.status_list = $scope.temp_emp_status;
                        });

                        $scope.emp_date = [];
                        var countdate = 0;

                        angular.forEach($scope.emp_list, function (dd, i) {
                            $scope.emp_drreport = [];
                            var totalworkinghrs = 0;
                            var counthrs = 0.00;

                            angular.forEach($scope.getsaveddetails, function (d, j) {
                                if (dd.HRME_Id === d.HRME_Id) {
                                    $scope.timetakenactual = "";
                                    $scope.timetaken = "";
                                    countdate = d.HRME_Id + "NAME" + i + "Date" + j;

                                    var decimaltimetot = d.ISMDRPT_TimeTakenInHrs;

                                    var hrstot = parseInt(Number(d.ISMDRPT_TimeTakenInHrs));

                                    var mintot = Math.round((Number(decimaltimetot) - hrstot) * 60);

                                    counthrs = (parseFloat(mintot) * 0.0166667).toFixed(2);

                                    counthrs = parseFloat(hrstot) + parseFloat(counthrs);

                                    totalworkinghrs += counthrs;

                                    if (hrstot.toString().length === 1) {
                                        hrstot = '0' + hrstot;
                                    }
                                    if (mintot.toString().length === 1) {
                                        mintot = '0' + mintot;
                                    }

                                    $scope.timetaken = hrstot + " Hrs " + mintot + " Mins";


                                    var decimaltimetot1 = d.ISMTPLTA_EffortInHrs;
                                    var hrstot1 = parseInt(Number(d.ISMTPLTA_EffortInHrs));
                                    var mintot1 = Math.round((Number(decimaltimetot1) - hrstot1) * 60);
                                    if (hrstot1.toString().length === 1) {
                                        hrstot1 = '0' + hrstot1;
                                    }
                                    if (mintot1.toString().length === 1) {
                                        mintot1 = '0' + mintot1;
                                    }
                                    $scope.timetakenactual = hrstot1 + " Hrs " + mintot1 + " Mins";
                                    var res = "";
                                    var countlength = 0;
                                    if (d.ISMDRPT_Remarks !== null && d.ISMDRPT_Remarks !== "") {
                                        if (d.ISMDRPT_Remarks.toString().length > 100) {
                                            res = d.ISMDRPT_Remarks.substring(0, 100);
                                            res = res + "....";
                                            countlength = 1;
                                        } else {
                                            res = d.ISMDRPT_Remarks;
                                        }
                                    }

                                    $scope.emp_drreport.push({
                                        ISMTCR_TaskNo: d.ISMTCR_TaskNo, ISMDRPT_Id: d.ISMDRPT_Id,
                                        ISMTCR_Title: d.ISMTCR_Title, ISMDRPT_Status: d.ISMDRPT_Status, ISMTPLTA_EffortInHrs: $scope.timetakenactual,
                                        ISMDRPT_TimeTakenInHrs: $scope.timetaken, ISMDRPT_Date: d.ISMDRPT_Date, COUNTCHECKLIST: d.COUNTCHECKLIST,
                                        HRME_Id: d.HRME_Id, Approvedflag: false, Remarks: "Approved", Efforts: 0, index: countdate, ISMTCR_Id: d.ISMTCR_Id,
                                        effhrs: parseInt(hrstot), effmin: parseInt(mintot), drremarks: d.ISMDRPT_Remarks, ISMTPLTA_Id: d.ISMTPLTA_Id,
                                        status: "1", countlength: countlength, drresponsedetails: res,
                                        ISMTCR_Id_rejected: d.ISMTCR_Id_rejected, ISMMTCAT_TaskCategoryName: d.ISMMTCAT_TaskCategoryName,
                                        TASK_CREATED_BY: d.TASK_CREATED_BY, periodicitydailyflag: d.periodicitydailyflag,
                                        ISMTAPL_Periodicity: d.ISMTAPL_Periodicity
                                    });
                                }
                            });

                            var totalhrstot = parseInt(Number(totalworkinghrs));

                            var totalmintot = Math.round((Number(totalworkinghrs) - totalhrstot) * 60);

                            if (totalhrstot.toString().length === 1) {
                                totalhrstot = '0' + totalhrstot;
                            }
                            if (totalmintot.toString().length === 1) {
                                totalmintot = '0' + totalmintot;
                            }
                            $scope.totaltimetaken = totalhrstot + " Hrs" + " " + totalmintot + " Mins";

                            dd.totalhrs = $scope.totaltimetaken;
                            dd.emp_drdates = $scope.emp_drreport;
                        });

                        $scope.filldata = promise.filldata;
                        angular.forEach($scope.emp_list, function (dd) {
                            dd.totalpunchedhrs = "";
                            angular.forEach($scope.filldata, function (d) {
                                if (dd.HRME_Id === d.HRME_Id) {
                                    if (d.WorkingHours != null && d.WorkingHours != "") {

                                        var PunchInOut = d.WorkingHours.split(':');
                                        var punchin = PunchInOut[0];
                                        var punchout = PunchInOut[1];

                                        if (punchin.toString().length === 1) {
                                            punchin = "0" + punchin;
                                        }

                                        dd.totalpunchedhrs = punchin + " Hrs " + punchout + " Mins";
                                        dd.starttime = d.StartTime;
                                        dd.endtime = d.EndTime;
                                        dd.titlehrs = "Punch In :" + d.StartTime + " Punch Out : " + d.EndTime;
                                    }
                                }
                            });
                        });

                        $scope.emp_drreport = [];
                        console.log($scope.emp_list);
                        $('#viewDrApproved').modal('show');
                        //viewDrApproved
                    } else {
                        swal("No Record Found.... !!");
                        $scope.getsaveddetails = [];
                    }
                }
            });

        }
        $scope.ISMSLE_IdNew = "";
        $scope.OnChangelead = function (user) {

            $scope.bomList = [];
            $scope.ISMSBOM_Id = "";
            var products = [];
            angular.forEach($scope.employeeidNew, function (d) {
                if (d.IERCLE_Id == user.IERCLE_Id) {
                    products.push(d.ISMSMPR_Id);
                }
            });
            $scope.ISMSLE_IdNew = user.IERCLE_Id;
            var data = {
                "product": products,
                "ISMSLE_Id": 0
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

        //added By sanjeev

        
        $scope.LeadsData = function (bil) {
            $scope.LeadComments = [];
            var data = {
                "IER_ID": bil.IER_ID,
            }
            apiService.create("TADA_Advance_Tour/LeadComments", data).then(function (promise) {
                if (promise.view_lead_demo != null && promise.view_lead_demo.length > 0) {
                    angular.forEach(promise.view_lead_demo, function (emcp) {
                        if (emcp.IERCLE_Id == bil.ISMSLE_Id) {
                            $scope.LeadComments.push(emcp);

                        }
                    });
                     
                }
                else {
                    swal("Comments Not Found !")
                }

            })
        }
        $scope.vi = 1;
        $scope.view = function (ss, vi) {
            if (vi == 1) {
                $scope.viewall = 1;
            }
            else {
                $scope.viewall = 0;
            }
            //$scope.edibl = ss.ismsledM_Id;
            //$scope.edib2 = ss.ismslE_Id;
            var data = {
                "viewall": $scope.viewall,
                "ISMSLEDM_Id": ss.ISMSLEDM_Id,
                "ISMSLE_Id": ss.ISMSLE_Id
            }
            apiService.create("TADA_Advance_Tour/view_lead_demo", data).then(function (promise) {
                $scope.ismslE_LeadName_s = promise.view_lead_demo[0].ismslE_LeadName;
                $scope.ISMSLEDM_ContactPerson_s = promise.view_lead_demo[0].ismsledM_ContactPerson;
                $scope.ISMSLEDM_DemoAddress_s = promise.view_lead_demo[0].ismsledM_DemoAddress;
                $scope.ISMSLEDM_Remarks_s = promise.view_lead_demo[0].ismsledM_Remarks;
                $scope.ISMSLE_ContactNo_s = promise.view_lead_demo[0].ismslE_ContactNo;

                $scope.product_dd_s = promise.product_dd_s;
                if ($scope.viewall == 1) {
                    $scope.demo_response_details = promise.demo_response_details;
                }


            });
        }
        //added By 
        $scope.LeadsDataview = function (bil,flag) {
            $scope.LeadReports = [];
            $scope.catenames = "";
            $scope.flagnew = "";
            $scope.catenames = bil.IMRC_CategoryName;
            $scope.flagnew = flag;
            $scope.ctcount = "";
           
            var data = {
                "IER_ID": $scope.employeeidNew[0].IER_ID,
                "IMRC_CategoryId": bil.IMRC_CategoryId,
                "message": flag

            }
            apiService.create("TADA_Advance_Tour/LeadComments", data).then(function (promise) {
                if (promise.getloadarray != null && promise.getloadarray.length > 0) {
                    $scope.LeadReports = promise.getloadarray;
                    $('#myModal_counts').modal('show');
                    if ($scope.flagnew == 'LC') {
                        $scope.ctcount = bil.LeadCount;
                    }
                    else if ($scope.flagnew == 'EC') {
                        $scope.ctcount = bil.ExecutionLeadCount;
                    }
                    else if ($scope.flagnew == 'BC') {
                        $scope.ctcount = (Number(bil.LeadCount) - (Number(bil.ExecutionLeadCount)));
                    }
                   
                    //angular.forEach(promise.getloadarray, function (dev) {
                    //    if ($scope.LeadReports.length === 0) {
                    //        $scope.LeadReports.push(dev);
                    //    }
                    //    else if ($scope.LeadReports.length > 0) {
                    //        var intcount = 0;
                    //        angular.forEach($scope.LeadReports, function (emp) {
                    //            if (emp.ISMSLE_Id === dev.ISMSLE_Id) {
                    //                intcount += 1;
                    //            }
                    //        });
                    //        if (intcount === 0) {
                    //            $scope.LeadReports.push(dev);
                    //        }
                    //    }
                    //})
                    //angular.forEach($scope.LeadReports, function (ddd) {
                    //    $scope.templist = [];
                    //    angular.forEach(promise.getloadarray, function (dd) {
                    //        if (dd.ISMSLE_Id === ddd.ISMSLE_Id) {
                    //            $scope.templist.push(dd);
                    //        }
                    //    });
                    //    ddd.Datacategorynew = $scope.templist;

                    //});
                }


            })
        }
    }
})();

