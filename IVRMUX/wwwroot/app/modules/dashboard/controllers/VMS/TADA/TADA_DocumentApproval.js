(function () {
    'use strict';
    angular
        .module('app')
        .controller('TADA_DocumentApprovalController', TADA_DocumentApprovalController)
    TADA_DocumentApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache', '$window']
    function TADA_DocumentApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache, $window) {
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
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.searchValue = '';
        $scope.maxDatedof = new Date($scope.today);
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian; todate
        };
        $scope.transrowsedit = [];
        $scope.VTADAA_BalanceHRMS = "";
        $scope.VTADAA_ApprovedFlg = false;
        // scope.tablediv = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.loaddata = function () {
            var pageid = 1;
            apiService.getURI("TADAApproval/getalldocument", pageid).
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

        //================== Count Amount

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
            $scope.transrowsedit = [];
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
            apiService.create("TADAApproval/getfileedit", data).then(function (promise) {
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
                    $scope.clientname = "";
                    $scope.clientname = promise.timeArray[0].vtadaaA_ClientMultiple;
                    if (promise.timeArray[0].vtadaA_BalanceHRMS != null && promise.timeArray[0].vtadaA_BalanceHRMS > 0) {
                        $scope.VTADAA_BalanceHRMS = promise.timeArray[0].vtadaA_BalanceHRMS;
                    }



                }
                if (promise.getFile != null && promise.getFile.length > 0) {
                    $scope.GetFile = promise.getFile;
                    angular.forEach($scope.GetFile, function (rr) {
                        rr.flag = "A";
                        rr.classname = 'neww';
                    });
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
            });
        };
        ///=================DeleteRecord-----
        //Deactive
        $scope.savedata = function () {
            $scope.submitted = true;
            $scope.uploadarray = [];
            $scope.VTADAA_ApprovedFlg = true;

            if ($scope.myForm.$valid) {
                if ($scope.GetFile.length > 0) {
                    angular.forEach($scope.GetFile, function (itm) {
                        if (itm.flag == "R") {
                            $scope.VTADAA_ApprovedFlg = false;
                        }
                        $scope.uploadarray.push({
                            VTADAAF_VerfieRemarks: itm.vtadaaF_VerfieRemarks,
                            VTADAAF_Id: itm.vtadaaF_Id,
                            Flag: itm.flag,

                        });

                    });
                    var msg = document.getElementById("documnetapproval").innerHTML;;
                    var data = {
                        "VTADAA_Id": $scope.VTADAA_Id,
                        "VTADAA_ApprovedFlg": $scope.VTADAA_ApprovedFlg,
                        filistarray: $scope.uploadarray,
                        "MailSubject": "TADA DOCUMENT",
                        "Mailmsg": msg
                    }
                    swal({
                        title: "Are you sure?",
                        text: "Do You Want To Save Record?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Save it",
                        cancelButtonText: "Cancel",
                        closeOnConfirm: false,
                        closeOnCancel: false
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                apiService.create("TADAApproval/Uploadfile", data).
                                    then(function (promise) {
                                        if (promise.returnval == "save") {
                                            swal("Record saved Sucessfully !");
                                        }
                                        else if (promise.returnval == "notsave") {
                                            swal("Record Not saved  !");
                                        }
                                        else if (promise.returnval == "admin") {
                                            swal("Please Contact Administrator  !");
                                        }
                                        else  {
                                            swal("Record Is Empty !");
                                        }
                                        $state.reload();

                                    })
                            }
                            else {
                                swal("Record  Cancelled!!!");
                            }
                        });
                }
                //$scope.VTADAA_ApprovedFlg
              
            }

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

        $scope.radiochange = function (ee) {

            if (ee.flag === 'A') {
                ee.classname = 'neww';

            }
            else {
                ee.classname = 'oldd';

            }
        }
    }
})();

