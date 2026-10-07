(function () {
    'use strict';
    angular
        .module('app')
        .controller('TADAApply_PaymentController', TADAApply_PaymentController)
    TADAApply_PaymentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile']
    function TADAApply_PaymentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile) {       
        $scope.usrname = localStorage.getItem('username');       
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !==null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        else {
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
        $scope.newdate = new Date();
        $scope.currentPage = 1;
        $scope.currentPage1 = 1;
        $scope.VTADAA_BalanceHRMS = "";
        $scope.VTADAAP_Date = new Date();
        $scope.onpageload = function () {
            var pageid = 1;
            apiService.getURI("TADAApply_Payment/getalldata", pageid).
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
            if ($scope.VTADAAP_PaymentMode == "Bank" || $scope.VTADAAP_PaymentMode == "Cheque" || $scope.VTADAAP_PaymentMode == "Online") {
                $scope.Ecashcard = true;
            }
            else {
                $scope.Ecashcard = false;
            }
            if ($scope.VTADAAP_PaymentMode == "Online") {
                $scope.VTADAAP_PaymentRefNo = "";
            }
        };
        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var VTADAAP_AmountPaidNew = "";
                if ($scope.VTADAAP_AmountPaidNew > 0) {
                    VTADAAP_AmountPaidNew = $scope.VTADAAP_AmountPaidNew;
                }

                var data = {
                    "VTADAAP_PaymentMode": $scope.VTADAAP_PaymentMode,
                    "VTADAAP_AmountPaid": $scope.VTADAAP_AmountPaid,
                    "VTADAAP_PaymentRefNo": $scope.VTADAAP_PaymentRefNo,
                    "VTADAAP_Remarks": $scope.VTADAAP_Remarks,
                    "VTADAAP_Date": new Date($scope.VTADAAP_Date).toDateString(),
                    "VTADAA_Id": $scope.VTADAA_Id,
                    "VTADAAP_Id": $scope.VTADAAP_Id,
                    "VTADAA_BalanceHRMS": VTADAAP_AmountPaidNew
                };
                apiService.create("TADAApply_Payment/savedta", data).then(function (promise) {
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
                            swal("Please Contact Administrator !");
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
        };
        //=====================Editrecord....
        $scope.edit = function (editid) {
            $scope.IER_Id = "";
            $scope.transrowsedit = [];
            $scope.VTADAA_BalanceHRMS = "";
            $scope.staffname = editid.hrmE_EmployeeFirstName;
            $scope.VTADAA_Id = editid.vtadaA_Id;
            $scope.cityname = editid.ivrmmcT_Name;
            $scope.applieamt = editid.vtadaA_TotalAppliedAmount;
            $scope.sanction = editid.vtadaA_TotalSactionedAmount;
            $scope.fromdate = editid.vtadaA_FromDate;
            $scope.todate = editid.vtadaA_ToDate;
            $scope.editflag = true;
            $scope.editflagtwo = false;
           // $scope.materaldocuupload = [];
            $scope.filarry(editid.vtadaA_Id)


           var VTADAAAAA_Id = 0;
            if (editid.vtadaaA_Id != null && editid.vtadaaA_Id > 0) {
                VTADAAAAA_Id = editid.vtadaaA_Id
            }
            else {
                VTADAAAAA_Id = 0;
            }           
            var data = {
                "VTADAA_Id": editid.vtadaA_Id,
                "MI_Id": editid.mI_Id,
                "VTADAAAAA_Id": VTADAAAAA_Id
            };
            apiService.create("TADAApproval/edit", data).then(function (promise) {
                // getfiles
                if (promise.timeArray != null && promise.timeArray.length > 0) {
                    if (promise.timeArray[0].vtadaA_DepartureTime != null && promise.timeArray[0].vtadaA_ArrivalTime != null) {
                        $scope.Departure = moment(promise.timeArray[0].vtadaA_DepartureTime, 'HH:mm a').format();
                        $scope.Ariival = moment(promise.timeArray[0].vtadaA_ArrivalTime, 'HH:mm a').format();
                    }
                    else {
                        $scope.Departure = "";
                        $scope.Ariival = "";
                    }
                    if (promise.timeArray[0].ieR_ID > 0) {
                        $scope.IER_Id = promise.timeArray[0].ieR_ID;
                        $scope.ApprovalRemarks($scope.IER_Id);
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
                if (promise.editArray != null && promise.editArray.length>0) {
                    if (promise.editArray[0].HCNT > 0) {
                        angular.forEach($scope.editarray, function (rr) {
                            var flag = '';
                            var classname = '';
                            if (rr.VTADAAAAH_StatusFlg =='Rejected') {
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
                                    VTADAAD_TotalSlots: rr.VTADAAD_TotalSlots,
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
                if (editid.vtadaA_BalanceHRMS != null && editid.vtadaA_BalanceHRMS > 0) {
                    $scope.VTADAA_BalanceHRMS = editid.vtadaA_BalanceHRMS;
                    $scope.VTADAAP_AmountPaid = 0;
                    $scope.VTADAAP_AmountPaidNew = editid.vtadaA_BalanceHRMS;
                    $scope.VTADAAP_PaymentMode = "Self";
                }
                if ($scope.IER_Id > 0) {
                   // $scope.save_Finaltour($scope.IER_Id);
                }
               // $scope.save_Finaltour();
                
            }); 
        };

        $scope.save_Finaltour = function (IER_ID) {
            //TADA_Advance_Tour
            $scope.VTADAA_TotalAppliedAmount = 0; $scope.VTADAA_BalanceHRMS = 0;
            $scope.leadLists = [];
            $scope.getArrayDc = []; $scope.EditArrayTwo = []; $scope.TourPlanDetails = []; $scope.FinalSumbmission = []
            $scope.ApprovalSubmission = [];
            var data = {                "IER_ID": 6,            };            apiService.create("TADA_Advance_Tour/FinalSubMission", data).then(function (promise) {                if (promise.getArray != null && promise.getArray.length > 0) {                    $scope.getArrayDc = promise.getArray;
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
                    $scope.TotalAppliedAmount = counttotal1
                    $scope.TotalSactionedAmount = counttotal2
                    $scope.TotalEligbleAmount = counttotal3
                }            });

        }
        $scope.Validation = function () {
            if ($scope.VTADAAP_AmountPaid <= $scope.sanction) {

            }
            else {
                swal("Sanaction Amount Not greater Than  Paid Amount !");
                $scope.VTADAAP_AmountPaid = "";
            }
        }
        $scope.filarry = function (vtadaA_Id) {
            $scope.materaldocuupload = [];
            var data = {
                "VTADAA_Id": vtadaA_Id   
            };
            apiService.create("TADA_Apply/Getdata", data).then(function (promise) {
                if (promise.editArryThree != null && promise.editArryThree.length > 0) {
                    $scope.materaldocuupload = promise.editArryThree;
                    angular.forEach($scope.materaldocuupload, function (dd) {
                        dd.VTADAAF_Remarks = dd.vtadaaF_Remarks;
                        dd.ismclT_FilePath = dd.vtadaaF_FilePath;
                        dd.ismclT_FileName = dd.vtadaaF_FileName;
                    });
                }

            });
        }
        $scope.previewimg_new = function (img) {
            $scope.ismclT_FilePath = img;
            var img = $scope.ismclT_FilePath;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.ismclT_FilePath);
                    $('#myimagePreview').modal('show');
                }
                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                    $window.open($scope.ismclT_FilePath);
                }
                else if (lastelement == 'pdf') {
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.ismclT_FilePath;
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
                $window.open($scope.ismclT_FilePath)
            }
        };
        //Deactive
        $scope.Deletedata = function (item, SweetAlert) {
            $scope.VTADAAP_Id = item.vtadaaP_Id;
            $scope.VTADAA_Id = item.vtadaA_Id;
            var data = {
                "VTADAAP_Id": $scope.VTADAAP_Id,
                "VTADAA_Id": $scope.VTADAA_Id
            }
            var dystring = "";
            if (item.vtadaaP_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.vtadaaP_ActiveFlg == false) {
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
                        apiService.create("TADAApply_Payment/deactive", data).
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
        }        //view
        $scope.getView = function (user) {
            var VTADAA_Id = user.vtadaA_Id;
            var VTADAAA_Id = user.vtadaaA_Id;
            $scope.emplYoeeName = user.hrmE_EmployeeFirstName;
            $scope.hrmE_EmployeeCode = user.hrmE_EmployeeCode;

            if (VTADAA_Id > 0) {
                var data = {
                    "VTADAAA_Id": VTADAAA_Id,
                    "VTADAA_Id": VTADAA_Id,
                    "RdoPunch": $scope.rdopunch
                };
                apiService.create("TADAApply_Payment/Viewdetails", data).then(function (promise) {
                    $scope.ViewDetails = promise.veiwdetails;
                    if ($scope.ViewDetails.length !== null && $scope.ViewDetails.length > 0) {
                        $scope.fromdate = $scope.ViewDetails[0].VTADAA_FromDate;
                        $scope.todate = $scope.ViewDetails[0].VTADAA_ToDate;
                        $scope.departuretime = moment($scope.ViewDetails[0].VTADAA_DepartureTime, 'HH:mm a').format();
                        $scope.ArrivalTime = moment($scope.ViewDetails[0].VTADAA_ArrivalTime, 'HH:mm a').format();
                        var date2 = new Date($scope.fromdate);
                        var date1 = new Date($scope.todate);
                        var timeDiff = Math.abs(date1.getTime() - date2.getTime());
                        var DateDifference = Math.ceil(timeDiff / (1000 * 3600 * 24) + 1);
                        $scope.datediff = DateDifference;
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
                                    VTADAAA_SanctioningLevel: dd.VTADAAA_SanctioningLevel,
                                    ApprovedBy: dd.ApprovedBy
                                });
                            } else if ($scope.Getsancationed_List.length > 0) {
                                var count = 0;
                                angular.forEach($scope.Getsancationed_List, function (d) {
                                    if (d.VTADAAA_SanctioningLevel === dd.VTADAAA_SanctioningLevel) {
                                        count += 1;
                                    }
                                });
                                if (count === 0) {
                                    $scope.Getsancationed_List.push({
                                        VTADAAA_SanctioningLevel: dd.VTADAAA_SanctioningLevel,
                                        ApprovedBy: dd.ApprovedBy
                                    });
                                }
                            }
                        });
                        $scope.getapproveddetails = [];
                        angular.forEach($scope.Getsancationed_List, function (dd) {
                            $scope.getapproveddetails = [];
                            angular.forEach($scope.ViewDetails, function (d) {
                                if (dd.VTADAAA_SanctioningLevel === d.VTADAAA_SanctioningLevel) {
                                    $scope.getapproveddetails.push(d);
                                }
                            });
                            dd.reportdetails = $scope.getapproveddetails;
                        });
                        $('#viewDeatils').modal('show');

                    }                    
                });
            }
        }
        //edittt
        $scope.edittt = function (item) {
            $scope.VTADAAP_Id = item.vtadaaP_Id;
            $scope.VTADAA_Id = item.vtadaA_Id;
            var data = {
                "VTADAAP_Id": $scope.VTADAAP_Id,
                "VTADAA_Id": $scope.VTADAA_Id
            };
            apiService.create("TADAApply_Payment/edittt", data).then(function (promise) {
                if (promise.edit_Two.length > 0 && promise.editArray.length > 0) {
                    $scope.transrowsedit = [];
                    $scope.staffname = promise.editArray[0].hrmE_EmployeeFirstName;
                    $scope.cityname = promise.editArray[0].ivrmmcT_Name;
                    $scope.applieamt = promise.editArray[0].vtadaA_TotalAppliedAmount;
                    $scope.sanction = promise.editArray[0].vtadaA_TotalSactionedAmount;
                    $scope.fromdate = promise.editArray[0].vtadaA_FromDate;
                    $scope.todate = promise.editArray[0].vtadaA_ToDate;
                    $scope.VTADAAP_PaymentMode = promise.edit_Two[0].vtadaaP_PaymentMode;
                    $scope.VTADAAP_PaymentRefNo = promise.edit_Two[0].vtadaaP_PaymentRefNo;
                    $scope.VTADAAP_AmountPaid = promise.edit_Two[0].vtadaaP_AmountPaid;
                    $scope.VTADAAP_Remarks = promise.edit_Two[0].vtadaaP_Remarks;
                    $scope.VTADAAP_Date = promise.edit_Two[0].vtadaaP_Date;
                    $scope.editflag = true;
                    $scope.editflagtwo = false;
                    if ($scope.VTADAAP_PaymentMode == "Bank" || $scope.VTADAAP_PaymentMode == "Cheque" || $scope.VTADAAP_PaymentMode == "Online") {
                        $scope.Ecashcard = true;
                    }
                    else {
                        $scope.Ecashcard = false;
                    }
                    $scope.filarry(item.vtadaA_Id)
                }
                else {
                    swal("Record Not Found !!!");
                }
            });
        };
        $scope.Clearid = function () {
            $state.reload();
            $scope.tablediv = false;
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.ApprovalRemarks = function (IER_ID) {
            $scope.allowense = [];
            $scope.txt_approxTotAmountNew = "";
            $scope.RepeatDetails = [];
            $scope.RepeatDetailsNew = [];
            $scope.Tour_Details = [];
            $scope.Datacategorynew = [];
            var data = {
                "IER_ID": IER_ID
            };
            apiService.create("TADA_Advance_Tour/getView", data).then(function (promise) {
                if (promise.editdataOthelist != null && promise.editdataOthelist.length > 0) {
                    angular.forEach(promise.editdataOthelist, function (dd) {
                        $scope.RepeatDetailsNew.push({
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
                    $scope.maxstartdateNew = new Date(promise.editdataOthelist[0].IER_Plan_StartDate);
                    $scope.maxEndDateNew = new Date(promise.editdataOthelist[0].IER_Plan_EndDate);
                    $scope.Datacategorynew = promise.getPlanerdata;
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

                    if (promise.tour_Details != null && promise.tour_Details.length > 0) {
                        $scope.allowense = promise.tour_Details;
                        var Totalamount = 0;

                    }


                }


            });
        }
    }
})();

