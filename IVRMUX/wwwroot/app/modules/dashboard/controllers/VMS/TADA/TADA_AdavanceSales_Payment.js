(function () {
    'use strict';
    angular
        .module('app')
        .controller('TADA_AdavanceSales_PaymentController', TADA_AdavanceApply_PaymentController)
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
        $scope.VTADASLEAP_Id = "";
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
            apiService.getURI("TADA_AdavanceApply_Payment/getOnload", pageid).
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
                        "VTADASLEAP_Id": $scope.VTADASLEAP_Id
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
            $scope.ViewDetails = [];
            $scope.VTADAAAP_Id = item.vtadaaaP_Id;
            $scope.VTADAAA_Id = item.vtadaaA_Id;
            $scope.emplYoeeName = item.hrmE_EmployeeFirstName;
            $scope.hrmE_EmployeeCodeee = item.hrmE_EmployeeCode;
            
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
            //City
            $scope.transrowsedit = [];
            $scope.staffname = editid.hrmE_EmployeeFirstName;
            $scope.VTADAAA_Id = editid.vtadaaA_Id;
            $scope.cityname = editid.ivrmmcT_Name;
            $scope.applieamt = editid.vtadaaA_TotalAppliedAmount;
            $scope.sanction = editid.vtadaaA_TotalSactionedAmount;
            $scope.fromdate = editid.vtadaaA_FromDate;
            $scope.todate = editid.vtadaaA_ToDate;
            $scope.VTADASLEAP_Id = editid.VTADASLEAP_Id;        
            $scope.editflagtwo = false;
            $scope.editflag = true;
            var VTADAAA_Id = editid.vtadaaA_Id;;
            $scope.emplYoeeName = editid.hrmE_EmployeeFirstName;
            $scope.hrmE_EmployeeCodeee = editid.hrmE_EmployeeCode;
            $scope.VTADAAA_Remarks = "";
            $scope.IER_ID = "";
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
                        $scope.VTADAAA_Remarks = $scope.ViewDetails[0].VTADAAA_Remarks;
                        $scope.ApprovalRemarks(promise.veiwdetails[0].IER_ID);
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
        $scope.ViewFiles = function (user) {
            $scope.getloadarray = [];
            var data = {                "IER_ID": user.IER_ID,            };            apiService.create("TADA_Advance_Tour/FinalSubMission", data).then(function (promise) {                if (promise.getloadarray != null && promise.getloadarray.length > 0) {                    $scope.getloadarray = promise.getloadarray;
                    $('#myModal_TADA').modal('show');
                }                            });
        }

        $scope.previewimg_new = function (img) {            $scope.ilrT_FilePath = img;            var img = $scope.ilrT_FilePath;            if (img != null) {                var imagarr = img.split('.');                var lastelement = imagarr[imagarr.length - 1];                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {                    $('#tadapreviewX').attr('src', $scope.ilrT_FilePath);                    $('#myimagePreviewtada').modal('show');                }                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {                    $window.open($scope.ilrT_FilePath);                }                else if (lastelement == 'pdf') {                    $('#showpdf').modal('hide');                    var imagedownload1 = "";                    imagedownload1 = $scope.ilrT_FilePath;                    $http.get(imagedownload1, { responseType: 'arraybuffer' })                        .success(function (response) {                            var fileURL = "";                            var file = "";                            var embed = "";                            var pdfId = "";                            file = new Blob([(response)], { type: 'application/pdf' });                            fileURL = URL.createObjectURL(file);                            pdfId = document.getElementById("pdfIdzz");                            pdfId.removeChild(pdfId.childNodes[0]);                            embed = document.createElement('embed');                            embed.setAttribute('src', fileURL);                            embed.setAttribute('type', 'application/pdf');                            embed.setAttribute('width', '100%');                            embed.setAttribute('height', '1000');                            pdfId.appendChild(embed);                            $('#showpdf').modal('show');                        });                }            }            else {                $window.open($scope.ilrT_FilePath)            }        };
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

