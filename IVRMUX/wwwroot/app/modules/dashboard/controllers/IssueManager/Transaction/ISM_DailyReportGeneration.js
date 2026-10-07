(function () {
    'use strict';
    angular.module('app').controller('ISM_DailyReportGenerationController', ISM_DailyReportGenerationController);
    ISM_DailyReportGenerationController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$sce', 'intracontroller', '$window'];
    function ISM_DailyReportGenerationController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter, $sce, intracontroller, $window) {
        
        var imagedownload = "";
        var docname = "";
        var studentreg = "";
        var HostName = location.host;
        $scope.generate = true;
        $scope.generateothers = false;
        $scope.Approvalflag = true;
        $scope.halfday = false;
        $scope.submitted6 = false;

        this.isOpen = false;

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        $scope.getserverdate = function () {
            var xmlHttp;
            function srvTime() {
                try {
                    //FF, Opera, Safari, Chrome
                    xmlHttp = new XMLHttpRequest();
                }
                catch (err1) {
                    //IE
                    try {
                        xmlHttp = new ActiveXObject('Msxml2.XMLHTTP');
                    }
                    catch (err2) {
                        try {
                            xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
                        }
                        catch (eerr3) {
                            //AJAX not supported, use CPU time.
                            alert("AJAX not supported");
                        }
                    }
                }
                xmlHttp.open('HEAD', window.location.href.toString(), false);
                xmlHttp.setRequestHeader("Content-Type", "text/html");
                xmlHttp.send('');
                return xmlHttp.getResponseHeader("Date");
            }
            $scope.today = srvTime();
        }

        $scope.getserverdate();

        $scope.documentFlag = false;
        $scope.ISMDRPT_Date = new Date($scope.today);
        $scope.maxDatedof = new Date($scope.today);

        $scope.ISMDRPT_Date_others = new Date($scope.today);

        var fullyear = $scope.ISMDRPT_Date_others.getFullYear();
        $scope.maxDatefP = new Date(fullyear, "11", "31");


        $scope.datedisable = true;

        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date($scope.today);
        $scope.ismtcrtrtO_TransferredDate = new Date($scope.today);
        var today = (new Date($scope.today)).getDay();
        $scope.startweek = "";
        $scope.endweek = "";
        $scope.deviation_id = "";

        if (today === 6) {
            $scope.deviation_id = 1;
            $scope.startweek = new Date(new Date($scope.today).setDate(new Date($scope.today).getDate() - 5));
            $scope.endweek = new Date($scope.today);
        }
        else {
            $scope.deviation_id = 2;
            $scope.startweek = new Date($scope.today);
            $scope.endweek = new Date($scope.today);
        }

        // ******* On Load  *********//
        $scope.loaddata = function () {
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.genrateflag = false;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;

            $scope.get_plannerlist = [];

            apiService.getURI("ISM_DailyReportGeneration/loaddata", pageid).then(function (promise) {
                $scope.get_plannerlist = promise.gettasklist;
                $scope.get_Status = promise.get_Status;
                $scope.getfiledetails = promise.getfiledetails;
               $scope.get_userEmplist = promise.get_userEmplist;
                $scope.daywise_drreport = promise.daywise_drreport;
                if (promise.rtO_VehcalList != null && promise.rtO_VehcalList.length > 0) {
                    $scope.rtO_VehcalList = promise.rtO_VehcalList;
                    $('#popup111').modal('show');
                }
                else {
                    $('#popup111').modal('hide');
                }
                if ($scope.daywise_drreport !== null && $scope.daywise_drreport.length > 0) {
                    $scope.halfday = $scope.daywise_drreport[0].ismdrptdW_HalfDayDRFlg;
                }

                if ($scope.get_plannerlist !== null && $scope.get_plannerlist.length > 0) {
                    $scope.genrateflag = true;
                }
                if (promise.getbda_Report != null && promise.getbda_Report.length > 0) {
                    $('#tadaBdm').modal('show');
                    $scope.TadaDdM = [];
                    angular.forEach(promise.getbda_Report, function (dev) {
                        if ($scope.TadaDdM.length === 0) {
                            $scope.TadaDdM.push(dev);
                        }
                        else if ($scope.TadaDdM.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.TadaDdM, function (emp) {
                                if (emp.IER_ID === dev.IER_ID) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.TadaDdM.push(dev);
                            }
                        }
                    })
                    angular.forEach($scope.TadaDdM, function (ddd) {
                        $scope.templist = [];
                        angular.forEach(promise.getbda_Report, function (dd) {
                            if (dd.IER_ID === ddd.IER_ID) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.tadaCategory = $scope.templist;

                    });
                   
                    angular.forEach(promise.TadaDdM, function (dd) {
                        if (dd.Blcokflag == true) {
                            $scope.generate = false;
                        }
                    });
                }
                else {
                    $('#tadaBdm').modal('hide');
                }
                if (promise.drnotapprovedmessage !== undefined && promise.drnotapprovedmessage !== null && promise.drnotapprovedmessage.length > 0) {                    
                    $scope.generate = false;
                    $scope.drnotapprovedmessage = promise.drnotapprovedmessage;
                    $scope.TempEmployee = [];

                    angular.forEach($scope.drnotapprovedmessage, function (dd) {
                        if ($scope.TempEmployee.length === 0) {
                            $scope.TempEmployee.push(dd);
                        } else if ($scope.TempEmployee.length > 0) {
                            var countemp = 0;
                            angular.forEach($scope.TempEmployee, function (d) {
                                if (d.HRME_Id === dd.HRME_Id) {
                                    countemp += 1;
                                }
                            });
                            if (countemp === 0) {
                                $scope.TempEmployee.push(dd);
                            }
                        }
                    });

                    angular.forEach($scope.TempEmployee, function (dd) {
                        $scope.TempDates_Details = [];
                        angular.forEach($scope.drnotapprovedmessage, function (d) {
                            if (dd.HRME_Id === d.HRME_Id) {
                                $scope.TempDates_Details.push(d);
                            }
                        });
                        dd.DatesDetails = $scope.TempDates_Details;
                    });

                    $('#viewdrnotapproveddetails').modal('show');
                    //return;
                }               

                $scope.TempEmployee = [];

                // Check if data exists
                if (promise.isM_RegeneratedDR !== undefined && promise.isM_RegeneratedDR !== null && promise.isM_RegeneratedDR.length > 0) {
                    $scope.generate = false;
                    $scope.RegeneratedDR = promise.isM_RegeneratedDR;
                    $scope.TempEmployee = [];
                    //angular.forEach($scope.RegeneratedDR, function (dd) {
                    //    var found = false;
                    //    angular.forEach($scope.TempEmployee, function (tempEmp) {
                    //        if (tempEmp.HRME_Id === dd.HRME_Id) {
                    //            found = true;
                    //        } 
                    //    });

                    //    if (!found) {
                    //        $scope.TempEmployee.push(dd);
                    //    }
                    //});

                    angular.forEach($scope.RegeneratedDR, function (dd) {
                        if ($scope.TempEmployee.length === 0) {
                            $scope.TempEmployee.push(dd);
                        } else if ($scope.TempEmployee.length > 0) {
                            var countemp = 0;
                            angular.forEach($scope.TempEmployee, function (d) {
                                if (d.HRME_Id === dd.HRME_Id) {
                                    countemp += 1;
                                }
                            });
                            if (countemp === 0) {
                                $scope.TempEmployee.push(dd);
                            }
                        }
                    });




                    // Now group DatesDetails by HRME_Id
                    angular.forEach($scope.TempEmployee, function (dd) {
                        $scope.TempDates_Details = [];
                        angular.forEach($scope.RegeneratedDR, function (d) {
                            if (dd.HRME_Id === d.HRME_Id) {
                                $scope.TempDates_Details.push(d);
                            }
                        });
                        dd.DatesDetails = $scope.TempDates_Details;
                    });

                    // Open the modal to show the data
                    $('#DrRechecking').modal('show');
                }



                angular.forEach($scope.get_plannerlist, function (dd) {
                    dd.deviationflag = false;
                    dd.effhrs = 0;
                    dd.effmin = 0;

                    if (dd.dr_flag === 1) {
                        dd.checkedvalue = true;
                        dd.deviationflag = dd.ISMDRPT_DeviationFlg;
                        if (dd.ISMDRPT_ExtraFlg === true) {
                            dd.extraflag = 1;
                        } 

                        if (dd.ISMDRPT_TimeTakenInHrs !== null && dd.ISMDRPT_TimeTakenInHrs !== "") {
                            var decimaltimetakenhrs = dd.ISMDRPT_TimeTakenInHrs;
                            var hrstimetaken = parseInt(Number(decimaltimetakenhrs));
                            var mintimetaken = Math.round((Number(decimaltimetakenhrs) - hrstimetaken) * 60);
                            dd.effhrs = hrstimetaken;
                            dd.effmin = mintimetaken;
                        }
                        $scope.tempfiles_list = [];
                        if ($scope.getfiledetails !== null && $scope.getfiledetails.length > 0) {

                            angular.forEach($scope.getfiledetails, function (files) {
                                if (files.ismtcR_Id == dd.ISMTCR_Id && dd.ISMTPLTA_Id == files.ismtpltA_Id) {
                                    $scope.tempfiles_list.push({
                                        checklistname: files.ismmtcatcL_CheckListName, filepath: files.ismdrptfL_FilePath, filename: files.ismdrptfL_FileName,
                                        comments: files.ismdrptfL_Remarks, refno: files.ismdrptfL_RefNo, ISMMTCATCL_Id: files.ismmtcatcL_Id,
                                        ISMDRPTFL_Id: files.ismdrptfL_Id
                                    });
                                }
                            });

                            dd.categorychecklist = $scope.tempfiles_list;
                        }
                    }
                    // Task Wise Status List
                    $scope.tempstatus_Temp = [];
                    $scope.tempstatus = [];
                    angular.forEach($scope.get_Status, function (dd) {
                        if (dd.ismmistS_StatusName.toLowerCase() !== 'onhold-department' && dd.ismmistS_StatusName.toLowerCase() !== "onhold"
                            && dd.ismmistS_StatusName.toLowerCase() !== "management blocker") {
                            $scope.tempstatus_Temp.push(dd);
                        }
                    });

                    if (dd.CreatedFlag === 1) {
                        dd.get_Status = $scope.tempstatus_Temp;
                    } else {
                        angular.forEach($scope.tempstatus_Temp, function (task) {
                            if (task.ismmistS_StatusName != "Close" && task.ismmistS_StatusName != "close") {
                                $scope.tempstatus.push(task);
                            };
                        });
                        dd.get_Status = $scope.tempstatus;
                    }


                    if (dd.ISMTAPL_Periodicity === 'Daily' && dd.periodicitydailyflag === 1) {
                        //dd.checkedvalue = true;
                        dd.disableflag = 1;
                    }

                    if (dd.ISMTAPL_Periodicity === 'Daily' && dd.periodicitydailyflag === 2) {
                        dd.disableflag = 1;
                    }

                    if (dd.ISMTAPL_Periodicity === 'Weekly' && dd.periodicityweeklyflag === 0) {
                        dd.disableflag = 1;
                    }

                    if (dd.ISMTAPL_Periodicity === 'Weekly' && dd.periodicityweeklyflag === 1) {
                        dd.disableflag = 1;
                        //dd.checkedvalue = true;
                    }

                    if (dd.checkedvalue === true) {
                        $scope.taskenddate = new Date(dd.ISMTPL_EndDate);
                        $scope.todaydate = new Date($scope.today);
                        var enddate = $scope.curdate === null ? "" : $filter('date')($scope.taskenddate, "yyyy-MM-dd");
                        var curntdate = $scope.todaydate === null ? "" : $filter('date')($scope.todaydate, "yyyy-MM-dd");
                        if (curntdate > enddate) {
                            dd.deviationflag = true;
                        } else {
                            dd.deviationflag = false;
                        }
                    }


                    if (dd.ISMDRPT_CompoffApplicableflag === 1) {
                        dd.ISMDRPT_CompoffApplicableflag = true;
                    }
                    else {
                        dd.ISMDRPT_CompoffApplicableflag = false;
                    }



                    var decimaltimetot = dd.ISMTPLTA_EffortInHrs;

                    var hrstot = parseInt(Number(decimaltimetot));
                    var mintot = Math.round((Number(decimaltimetot) - hrstot) * 60);

                    if (hrstot.toString().length === 1) {
                        hrstot = '0' + hrstot;
                    }

                    if (mintot.toString().length === 1) {
                        mintot = '0' + mintot;
                    }

                    dd.actualeffortinhrs = hrstot;
                    dd.actualeffortinmins = mintot;


                });

                //$scope.totaltask = $scope.get_plannerlist.length;

                $scope.blockedcount = promise.blockedcount;

                //$scope.blockeddetails = promise.blockeddetails;
                //$scope.blockeddetailsNew = []; //by ganesh 12-09-2024
                //if ($scope.blockeddetails != null && $scope.blockeddetails.length > 0) {

                //    angular.forEach($scope.blockeddetails, function (task) {
                //        if (task.ISM_PlannerBlockFlg == true) {
                //            $scope.blockeddetailsNew.push(task);
                //        };
                //    });
                //    if ($scope.blockeddetailsNew !== undefined && $scope.blockeddetailsNew !== null && $scope.blockeddetailsNew.length > 0) {
                //        $('#blockedHRreason').modal('show');
                //        $scope.generate = false;
                //    }

                //}
                //Created By AMAN
                if (promise.dailyreportextensiondotherdate !=null && promise.dailyreportextensiondotherdate.length > 0) {

                    if (promise.dailyreportextensiondotherdate != null && promise.dailyreportextensiondotherdate.length > 0) {
                        $scope.appointmentgenerationblock = promise.dailyreportextensiondotherdate;
                        if ($scope.appointmentgenerationblock.length > 0) {
                            $('#appointmentgenerationblock').modal('show');
                        }
                       
                    }
                    else {
                        $('#appointmentgenerationblock').modal('hide');
                    }

                }
                if (promise.proxyNotMapped != null && promise.proxyNotMapped.length > 0) {
                    $scope.proxyNotMapped = promise.proxyNotMapped;
                    $('#proxyNotMappedEmp').modal('show');
                } 
                if (promise.dR_Generationreject != null && promise.dR_Generationreject.length > 0) {
                    $scope.NatureOfScopeNotGiven = promise.dR_Generationreject;
                    $('#naturefoscope').modal('show');
                }
                //Created By AMAN Assets mapping
                if (promise.assetsmappingdrblock != null && promise.assetsmappingdrblock.length > 0) {

                    //if (promise.assetsmappingdrblock != null && promise.assetsmappingdrblock.length > 0) {
                    //    $scope.assetsmapdrblock = promise.assetsmappingdrblock;
                    //    if ($scope.assetsmapdrblock.length > 0) {
                    //        $('#assetDRblock').modal('show');
                    //    }

                    //}
                    //else {
                    //    $('#assetDRblock').modal('hide');
                    //}


                    if (promise.assetsmappingdrblock != null && promise.assetsmappingdrblock.length > 0) {
                        // Initialize the arrays for HR and IT assets
                        $scope.HR_assets = [];
                        $scope.IT_assets = [];

                        // Loop through the assetsmappingdrblock array
                        for (let i = 0; i < promise.assetsmappingdrblock.length; i++) {
                            let asset = promise.assetsmappingdrblock[i];

                            // Check if the asset should be added to HR_assets
                            if (asset.Name && asset.HRCD_MobileNo && asset.HRME_DOJ) {
                                $scope.HR_assets.push({
                                    Name: asset.Name,
                                    HRCD_MobileNo: asset.HRCD_MobileNo,
                                    HRME_DOJ: asset.HRME_DOJ
                                });
                            }

                            // Check if the asset should be added to IT_assets
                            if (asset.Name && asset.HRMF_FloorName && asset.INVMLO_LocationRoomName) {
                                $scope.IT_assets.push({
                                    Name: asset.Name,
                                    HRMF_FloorName: asset.HRMF_FloorName,
                                    INVMLO_LocationRoomName: asset.INVMLO_LocationRoomName
                                });
                            }
                        }

                        // Show the modal if there are HR or IT assets
                        if ($scope.HR_assets.length > 0 || $scope.IT_assets.length > 0) {
                            $('#assetDRblock').modal('show');
                        }
                    } else {
                        // If no assets are present, hide the modal
                        $('#assetDRblock').modal('hide');
                    }


                }

                $scope.RegeneratedTasks = function () {
                    window.location.href = "http://localhost:57606/#/app/ISM_DailyReportRejection/1178";
                    $('.modal-backdrop').remove();
                };

                $scope.Redirect = function () {
                    $window.location.href = 'http://localhost:57606/#/app/ISMDashboard/';
                };
                //end

                       
                //    if ($scope.blockeddetails !== undefined && $scope.blockeddetails !== null && $scope.blockeddetails.length > 0) {
                //        $('#blockedHRreason').modal('show');
                //        }
                    
                //}


                $scope.getprioritylist = promise.getprioritylist;
                $scope.getstatuslist = promise.getstatuslist;

                $scope.lblplannername = promise.plannername;
                $scope.plannerdetails = promise.plannerdetails;

                $scope.totaltask = promise.plannerlength;
                $scope.getplannerdetails = promise.getplannerdetails;

                $scope.dailyreportcount = promise.dailyreportcount;
                $scope.dailyreportothersdatecount = promise.dailyreportothersdatecount;
                $scope.dailyreportblockedcount = promise.dailyreportblockedcount;
                $scope.extentiondays = promise.extensiondays;
                $scope.othersdayenable = promise.othersdayenable;
                $scope.dailyreportcountload = promise.dailyreportothersdatecount;
                $scope.extentiondaysload = promise.extensiondays;

                //$scope.gettaskcategorychecklist = promise.gettaskcategorychecklist;

                $scope.getdeviationreport = promise.getdeviationreport;
                if ($scope.getdeviationreport !== null && $scope.getdeviationreport.length > 0) {
                    $scope.completedpercentage = $scope.getdeviationreport[0].per;
                    $scope.deviationpercentage = $scope.getdeviationreport[0].deviation;
                } else {
                    $scope.completedpercentage = 0;
                    $scope.deviationpercentage = 100;
                }

                $scope.getdepartwisedeviationremrks = promise.getdepartwisedeviationremrks;
                $scope.getdepartwisecategory = promise.getdepartwisecategory;

                if (promise.getemployeedetails !== null && promise.getemployeedetails !== undefined && promise.getemployeedetails.length > 0) {
                    $scope.getemployeedetails = promise.getemployeedetails;
                    if ($scope.getemployeedetails[0].hrmE_DOJ !== null && $scope.getemployeedetails[0].hrmE_DOJ !== "") {
                        $scope.minDatedof = new Date($scope.getemployeedetails[0].hrmE_DOJ);
                    }
                }

                $scope.get_effortdetails = promise.get_effortdetails;

                if ($scope.get_effortdetails !== null && $scope.get_effortdetails.length > 0) {
                    $scope.workingefforts = $scope.get_effortdetails[0].WORKINGHOURS;
                }
                if ($scope.getplannerdetails !== null && $scope.getplannerdetails.length > 0) {

                    $scope.getapprovedfalg = $scope.getplannerdetails[0].ismtpL_ApprovalFlg;
                    $scope.getapprovedby = $scope.getplannerdetails[0].ismtpL_ApprovedBy;
                    
                    if ($scope.getapprovedfalg === false && $scope.getapprovedby === 0 ) {
                        $scope.Approvalflag = false;
                        $scope.totaltask = 0;
                        swal("Still Your Planner Is Pending , So Kindly Contact Department Head", "", "info");
                        return;
                    }
                   

                    else if ($scope.getapprovedfalg === false && $scope.getapprovedby > 0) {
                        $scope.Approvalflag = false;
                        $scope.totaltask = 0;
                        swal("Your Planner Is Rejected , So Kindly Contact Department Head", "", "info");
                        return;
                    }
                    else {
                        $scope.Approvalflag = true;
                        $scope.totaltask = promise.plannerlength;
                        if ($scope.ISMTPL_ApprovedTo == 0) {
                            swal("Still Your Planner Is Pending ,But Still You can Generete DR","","info");
                        }
                        //to get canteenfeed back on approval of Planner
                       // $scope.getcenteenfeedback();
                        //if ($scope.get_userEmplist != null && $scope.get_userEmplist.length > 0 && $scope.get_userEmplist[0].DRConcall != 1) {
                        //    $scope.ConcallLOP();
                            
                        //}
                    }
                    $scope.ISMTPL_ApprovedTo = $scope.getplannerdetails[0].ismtpL_ApprovedTo;
                    $scope.getcenteenfeedback();
                    $scope.getscounts();

                    if ($scope.get_userEmplist != null && $scope.get_userEmplist.length > 0 && $scope.get_userEmplist[0].DRConcall != 1) {
                        $scope.ConcallLOP();

                    }

                    $scope.get_userEmplistDemoRating = promise.get_userEmplistDemoRating;
                    if ($scope.get_userEmplistDemoRating != null && $scope.get_userEmplistDemoRating.length > 0 && $scope.get_userEmplistDemoRating[0].FLAG == "0") {


                        //$('#DemoratingmodelDrblock').modal('show');
                        $('#Demoratingmodel').modal('show');

                       
                    }
                    else if ($scope.get_userEmplistDemoRating != null && $scope.get_userEmplistDemoRating.length > 0 && $scope.get_userEmplistDemoRating[0].FLAG == "1"){
                        $('#DemoratingmodelDrblock').modal('show');

                         // //if ($scope.get_userEmplistDemoRating == null) {
                        //// Get the current date
                        //var currentDate = new Date();

                        //// Get the last date of the current month
                        //var lastDayOfMonth = new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 0);

                        //// Get the day of the month (1-31)
                        //var dayOfMonth = currentDate.getDate();
                        // $('#Demoratingmodel').modal('show');


                        // $('#DemoratingmodelDrblock').modal('show');

                        //// Check if we're in the last 10 days of the month, but NOT on the last day
                        // if (dayOfMonth >= lastDayOfMonth.getDate() - 9 && dayOfMonth < lastDayOfMonth.getDate()) {
                        //     $('#Demoratingmodel').modal('show');
                        // } else {
                        //     $('#DemoratingmodelDrblock').modal('show');
                        // }
                    }


                } else {
                    swal("Planner Not Created For This Week!!");
                    return;
                   
                   // $scope.getcenteenfeedback();
                }

                

                //HR Planner Details
                $scope.hrplannerdetails = [];
                $scope.overduehrplannerdetails = [];
                $scope.duehrplannerdetails = [];
                if (promise.hrplannerdetails !== undefined && promise.hrplannerdetails !== null && promise.hrplannerdetails.length > 0) {
                    $scope.hrplannerdetails = promise.hrplannerdetails;
                    $scope.itemsPerPageoverduereminder = 10;
                    $scope.itemsPerPageduereminder = 10;
                    $scope.currentPageoverduereminder = 1;
                    $scope.currentPagereminder = 1;
                    angular.forEach($scope.hrplannerdetails, function (dd) {
                        if (dd.HRPDSD_RepeatFlag === "OVER-DUE REMINDER") {
                            $scope.overduehrplannerdetails.push(dd);
                        } else if (dd.HRPDSD_RepeatFlag === "REMINDER") {
                            $scope.duehrplannerdetails.push(dd);
                        }
                    });
                    $('#myModalhrplanner').modal('show');
                }


                $scope.telecallerblock = promise.telecallerblock;
                if ($scope.telecallerblock != null && $scope.telecallerblock.length > 0) {
                    if ($scope.telecallerblock[0].drblock == "OOPS! YOUR NOT ACHIVED DAILY LEAD TARGET") {
                        $scope.generate = false;
                        swal({
                            title: "OOPS! YOUR NOT ACHIVED DAILY LEAD TARGET",
                            text: "Do You Want To update..?",
                            type: "warning",
                            showCancelButton: false,
                            confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, update!",
                            cancelButtonText: "Cancel",
                            closeOnConfirm: true,
                            closeOnCancel: true
                        },
                            function (isConfirm) {
                                if (isConfirm) {
                                    intracontroller.viewcompleted = true;
                                    $('.modal-backdrop').remove();
                                    $('.modal').on('hide.bs.modal', function (e) {
                                        e.stopPropagation();
                                        $('body').css('padding-right', '');
                                    });
                                    $window.location.href = 'http://' + HostName + '/#/app/Sales_Lead_Creation/792';
                                }

                            });
                        //swal("You Can Not Generate Daily Report Because Still You Did Not Closed The Completed Task. Kindly Close The Completed Task");
                    }
                }
               
                $scope.CloseTaskCoutnDetails = promise.closeTaskCoutnDetails;
                $scope.compbyme = $scope.CloseTaskCoutnDetails != null && $scope.CloseTaskCoutnDetails.length > 0 ? $scope.CloseTaskCoutnDetails[0].ISMEDWTCC_TaskCount : 0;

                if ($scope.compbyme > 0) {
                    $scope.generate = false;
                    swal({
                        title: "You Can Not Generate Daily Report Because Still You Did Not Closed The Completed Task. Kindly Close The Completed Task",
                        text: "Do You Want To update..?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, update!",
                        cancelButtonText: "Cancel",
                        closeOnConfirm: true,
                        closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                intracontroller.viewcompleted = true;
                                $('.modal-backdrop').remove();
                                $('.modal').on('hide.bs.modal', function (e) {
                                    e.stopPropagation();
                                    $('body').css('padding-right', '');
                                });
                                $window.location.href = 'http://' + HostName + '/#/app/ISM_ComplaintsTaskList/12';
                            }
                           
                        });
                    
                }

                //
                $scope.demo_list = [];
                if (promise.demo_list != null && promise.demo_list.length > 0) {
                    $scope.demo_list = promise.demo_list;
                    if ($scope.demo_list[0].drblockDemo == "Please Update Demo Status") {
                        console.log($scope.demo_list[0].drblockDemo);
                        swal({
                            title: "Please Update Demo Status ",
                            text: "Do You Want To update..?",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, update!",
                            cancelButtonText: "Cancel",
                            closeOnConfirm: true,
                            closeOnCancel: true
                        },
                            function (isConfirm) {
                                if (isConfirm) {
                                    intracontroller.viewcompleted = true;
                                    $('.modal-backdrop').remove();
                                    $('.modal').on('hide.bs.modal', function (e) {
                                        e.stopPropagation();
                                        $('body').css('padding-right', '');
                                    });
                                    $window.location.href = 'http://' + HostName + '/#/app/ISM_Sales_Lead_Demo_Update/';
                                }

                            });

                    }
                }
                //gettaskdetails
                if (promise.gettaskdetails != null && promise.gettaskdetails.length > 0) {
                    $scope.generate = false;
                    swal({
                        title: "You Can Not Generate Daily Report Because Still You Did Not Reached They Daily Target " + promise.gettaskdetails[0].TaskCategoryOutput+" ",
                        text: "Do You Want To update..?",
                        type: "warning",
                       // showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, update!",
                     //   cancelButtonText: "Cancel",
                        closeOnConfirm: true,
                       // closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                intracontroller.viewcompleted = true;
                                $('.modal-backdrop').remove();
                                $('.modal').on('hide.bs.modal', function (e) {
                                    e.stopPropagation();
                                    $('body').css('padding-right', '');
                                });
                                $window.location.href = 'http://' + HostName + '/#/app/SalesLeadRawImport/12';
                            }

                        });
                }

                //deviation_create
                if (promise.deviation_create != null && promise.deviation_create.length > 0) {
                    $scope.deviation_create = promise.deviation_create;
                    $('#orderacceptance').modal('show');
                    //$scope.generate = false;
                    //swal({
                    //    title: "You Can Not Generate Daily Report Because Still You Did Not Approved  Order Acepatance !  ",
                    //    text: "Do You Want To Approved..?",
                    //    type: "warning",
                    //    showCancelButton: true,
                    //    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Approved!",
                    //    cancelButtonText: "Cancel",
                    //    closeOnConfirm: true,
                    //    closeOnCancel: true
                    //},
                    //    function (isConfirm) {
                    //        if (isConfirm) {
                    //            intracontroller.viewcompleted = true;
                    //            $('.modal-backdrop').remove();
                    //            $('.modal').on('hide.bs.modal', function (e) {
                    //                e.stopPropagation();
                    //                $('body').css('padding-right', '');
                    //            });
                    //            $window.location.href = 'http://' + HostName + '/#/app/ISM_LeadOA_Approval/12';
                    //        }

                    //    });
                }



                //You Can Not Generate Daily Report
                //if (promise.clientdropdown != null && promise.clientdropdown.length > 0) {
                //    $scope.generate = false;
                //    swal({
                //        title: "You Can Not Generate Daily Report Because Still You Did Not Approved Task. Kindly Approved The  Tasks",
                //        text: "Do You Want To update..?",
                //        type: "warning",
                //        showCancelButton: true,
                //        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, update!",
                //        cancelButtonText: "Cancel",
                //        closeOnConfirm: true,
                //        closeOnCancel: true
                //    },
                //        function (isConfirm) {
                //            if (isConfirm) {
                //                intracontroller.notcompleted = true;
                //                $('.modal-backdrop').remove();
                //                $('.modal').on('hide.bs.modal', function (e) {
                //                    e.stopPropagation();
                //                    $('body').css('padding-right', '');
                //                });
                //                $window.location.href = 'http://' + HostName + '/#/app/ISM_ComplaintsTaskList/12';
                //            }

                //        });
                //}


                // YesterDay Date Report Generated Or Not Details
                if (promise.dR_Notification === 0) {
                    if ($scope.Approvalflag === true) {
                        $scope.getdrnotsentdetails = promise.getdrnotsentdetails;
                        if ($scope.getdrnotsentdetails !== undefined && $scope.getdrnotsentdetails !== null && $scope.getdrnotsentdetails.length > 0) {
                            $('#viewdrnotsentdetails').modal('show');
                        }
                    }
                }
                //AdavanceList
                $scope.AdavanceList = promise.adavanceList;
                $scope.ApplyList = promise.applyList;
                if ($scope.AdavanceList != null && $scope.AdavanceList.length > 0 || $scope.ApplyList != null && $scope.ApplyList.length > 0) {
                    $('#TadaReport').modal('show');
                }
                else {
                    $('#TadaReport').modal('hide');
                }

                //HR_Plan //TA_Reminder
                if (promise.reminderList != null && promise.reminderList.length > 0) {
                    $scope.HR_PlanerReminder = promise.reminderList;
                    $('#HR_PlanerReminder').modal('show');
                }
                else {
                    $('#HR_PlanerReminder').modal('hide');
                }
                //droveralldetailsdatewise
                if (promise.droveralldetailsdatewise != null && promise.droveralldetailsdatewise.length > 0) {
                    if (promise.drnotapprovedmessageindividual != null && promise.drnotapprovedmessageindividual.length > 0) {

                    }
                    else {
                        $scope.generate = false;
                        swal({
                            title: "You Can Not Generate Daily Report Because Still You Did Not Update TADA Tour Status !",
                            text: "Do You Want To update..?",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, update!",
                            cancelButtonText: "Cancel",
                            closeOnConfirm: true,
                            closeOnCancel: true
                        },
                            function (isConfirm) {
                                if (isConfirm) {
                                    intracontroller.viewcompleted = true;
                                    $('.modal-backdrop').remove();
                                    $('.modal').on('hide.bs.modal', function (e) {
                                        e.stopPropagation();
                                        $('body').css('padding-right', '');
                                    });
                                    $window.location.href = 'http://' + HostName + '/#/app/TADA_Advance_Tour_Lead/12';
                                }

                            });
                    }
                }
                $scope.get_report = [];
                if (promise.get_report != null && promise.get_report.length > 0) {
                    $scope.Datacategorynew = promise.get_report;
                    $('#TA_Reminder').modal('show');
                    $scope.employeeidNew = [];
                    angular.forEach($scope.Datacategorynew, function (dev) {
                        if ($scope.employeeidNew.length === 0) {
                            $scope.employeeidNew.push(dev);
                        }
                        else if ($scope.employeeidNew.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.employeeidNew, function (emp) {
                                if (emp.HRME_Id === dev.HRME_Id) {
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
                        angular.forEach($scope.Datacategorynew, function (dd) {
                            if (dd.HRME_Id === ddd.HRME_Id) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.Datacategorynew = $scope.templist;

                    });
                }
                else {
                    $('#TA_Reminder').modal('hide');
                }
                
                //geTada_Report
                //get_report
                $scope.HRME_IdTour = "";
                $scope.notificationUser = []; $scope.notificationUserNew = [];
                if (promise.notificationUser != null && promise.notificationUser.length > 0) {
                    angular.forEach(promise.notificationUser, function (dd) {
                        if (dd.ISMSMPR_ProductName === "Qutation Not Sent") {
                            $scope.notificationUserNew.push(dd);
                        }
                        else {
                            $scope.notificationUser.push(dd);
                        }
                    });
                   // $scope.notificationUser = promise.notificationUser;
                    $scope.HRME_IdTour = promise.hrmE_Id;
                    if ($scope.HRME_IdTour == 1735) {
                        $scope.generate = true;
                    }
                    else {
                        $scope.generate = false;
                    }
                    
                    //TADA_FollowUp
                    $('#TADA_FollowUp').modal('show');
                }

                // termsandconditionagree
                $scope.probationperiod_15days = [];
                if (promise.get_feedback!=null && promise.get_feedback.length > 0) {
                    $scope.probationperiod_15days = promise.get_feedback;
                    $scope.probationperiod_threeMonth = promise.getchecklistarray;
                    $('#termsandconditionagree').modal('show');
                }
                else {
                    $('#termsandconditionagree').modal('hide');
                }               
                // termsandconditionagree
                $scope.momdrblock = [];
                if (promise.get_report_effort !=null  && promise.get_report_effort.length > 0) {
                    $scope.momdrblock = promise.get_report_effort;
                    $('#momdrblock').modal('show');
                }
                else {
                    $('#momdrblock').modal('hide');
                }
                // pendingfeedback

                if (promise.get_feedbackTrainee!=null && promise.get_feedbackTrainee.length > 0) {
                    $scope.get_feedback = promise.get_feedbackTrainee;
                    $scope.traineefeedback = promise.get_feedbackTrainer;
                    $('#feedbackpending').modal('show');
                    return;
                }
                else {
                    $('#feedbackpending').modal('hide');
                }

                if (promise.trainingMonitorPopUp != null && promise.trainingMonitorPopUp.length > 0) {

                    swal({
                        title: "Please confirm if there is a gap or issue observed in the training process.",
                        text: "Do you have any concerns to report..? ",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes",
                        cancelButtonText: "No",
                        closeOnConfirm: true,
                        closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                intracontroller.viewcompleted = true;
                                $('.modal-backdrop').remove();
                                $('.modal').on('hide.bs.modal', function (e) {
                                    e.stopPropagation();
                                    $('body').css('padding-right', '');
                                });
                                window.location.href = 'http://' + HostName + '/#/app/TrainingMonitoring/';
                            }

                        });
                }

            });           
        };

        //save DR Remarks ORDER ACCEPTANCE CAPTURED 

        $scope.saveremarks = function () {
            if (true) {
                var data = {
                    "Remarks": Remarks
                };
                apiService.create("ISM_ClientProject_Mapping/savedataLead", data).then(function (promise) {
                    if (promise.msg == 'saved') {
                        swal("Data Saved Successfully");
                    }
                    else if (promise.msg == 'Failed') {
                        swal("Data Not Saved");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        }
  
        // to get the not assign task list -- adarsh ms 
        $scope.getscounts = function () {
            $scope.currentPage = 1;
            $scope.get_comptasklist = [];
            $scope.get_tasks = [];
            var data = {
                "countflag": "NotAssign"
            };
            apiService.create("ISM_ComplaintsTaskList/getcount", data).then(function (promise) {
                if (promise.get_tasks.length > 0) {
                    $scope.get_tasks = promise.get_tasks;
                    $scope.get_tasks_count = $scope.get_tasks.length;

                    swal({
                        title: "You Have Not Assigned Tasks List. Please Assign To Generate DR!",
                        text: "Pending To Assign Tasks " + " : " + $scope.get_tasks_count,
                        type: "warning",
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Yes, Assign Tasks"  ,
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                intracontroller.viewnotassigned = true;
                                $('.modal-backdrop').remove();
                                $('.modal').on('hide.bs.modal', function (e) {
                                    e.stopPropagation();
                                    $('body').css('padding-right', '');
                                });
                                $window.location.href = 'http://localhost:57606/#/app/ISM_ComplaintsTaskList/';
                            } else {
                                swal("Cancelled");
                            }
                        });
                } else {
                    // If no tasks are found, proceed with the next function
                   // $scope.getnotapprovedcount();
                }
            }).catch(function (error) {
                console.error("Error in getscounts:", error);
            });
        };
         // to get the not approved task list -- adarsh ms 
        $scope.getnotapprovedcount = function () {
            var data = {
                "typeFlg": "Default"
            };
            apiService.create("ISM_ComplaintsTaskList/getdetails", data).then(function (promise) {
                if (promise.get_pendingcount !== null && promise.get_pendingcount.length > 0) {
                    $scope.get_pendingcount = promise.get_pendingcount.length;
                    if ($scope.get_pendingcount > 0) {
                        swal({
                            title: "You Have Not Approved Task List. Kindly Approve To Generate DR!",
                            text: "Approve Tasks",
                            type: "warning",
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "Yes, Approve Tasks",
                        },
                            function (isConfirm) {
                                if (isConfirm) {
                                    
                                    intracontroller.viewnotapproved = true;
                                    $window.location.href = 'http://localhost:57606/#/app/ISM_ComplaintsTaskList/';
                                } else {
                                    swal("Cancelled");
                                }
                            });
                        return;
                    }
                } else {
                    $scope.get_pendingcount = 0;
                }
            }).catch(function (error) {
                console.error("Error in getnotapprovedcount:", error);
            });
        };
    


        //FEED BACK FOR THE CANTEEN ON DIALY BASIS -- adarsh ms
        //Get Feedback
        $scope.getcenteenfeedback = function () {
            
            var data = {
                //"CanteenFeedback": ["Canteen", "HouseKeeping"]
                "FMTY_FeedbackTypeName": "Canteen"
            };
            apiService.create("FeedbackSchoolGeneralTransaction/getfeedback1", data).then(function (promise) {
                if (promise !== null) {
                    if ((promise.typelist !== null && promise.typelist.length > 0)
                        && (promise.mappedquestiondeetails !== null && promise.mappedquestiondeetails.length > 0)
                        && (promise.mappedoptiondeetails !== null && promise.mappedoptiondeetails.length > 0)) {
                        if (promise.given == "Given") {
                            $('#CanteenFeedback').modal('hide');
                            return;
                        }
                        else {
                            $('#CanteenFeedback').modal('show');
                        }
                        $scope.canteentemparray = [];
                        $scope.tempquestionarray = [];
                        $scope.tempoptionarray = [];
                        $scope.typelist = promise.typelist;
                        $scope.mappedquestiondeetails = promise.mappedquestiondeetails;
                        $scope.mappedoptiondeetails = promise.mappedoptiondeetails;
                        if ($scope.typelist[0].fmtY_QuestionwiseOptionFlg === false) {
                            angular.forEach($scope.typelist, function (dd) {

                                $scope.tempquestionarray = [];
                                $scope.tempoptionarray = [];
                                angular.forEach($scope.mappedquestiondeetails, function (ddd) {
                                    if (dd.fmtY_Id === ddd.fmtY_Id && !ddd.fmqE_ManualEntryFlg) {
                                        $scope.tempoptionarray = [];
                                        angular.forEach($scope.mappedoptiondeetails, function (dddd) {
                                            if (dd.fmtY_Id === dddd.fmtY_Id) {
                                                $scope.tempoptionarray.push({ FMOP_Id: dddd.fmoP_Id, FMOP_FeedbackOptions: dddd.fmoP_FeedbackOptions });
                                            }
                                        });

                                        $scope.tempquestionarray.push({
                                            FMQE_Id: ddd.fmqE_Id, FMQE_FeedbackQuestions: ddd.fmqE_FeedbackQuestions,
                                            opt: $scope.tempoptionarray, manualflg: ddd.fmqE_ManualEntryFlg
                                        });
                                    } else {
                                        $scope.tempquestionarray.push({
                                            FMQE_Id: ddd.fmqE_Id, FMQE_FeedbackQuestions: ddd.fmqE_FeedbackQuestions,
                                            manualflg: ddd.fmqE_ManualEntryFlg
                                        });
                                    }
                                });
                                $scope.canteentemparray.push({ FMTY_Id: dd.fmtY_Id, FMTY_FeedbackTypeName: dd.fmtY_FeedbackTypeName, ques: $scope.tempquestionarray });

                            });
                        } else {
                            angular.forEach($scope.typelist, function (dd) {

                                $scope.tempquestionarray = [];
                                $scope.tempoptionarray = [];
                                angular.forEach($scope.mappedquestiondeetails, function (ddd) {
                                    if (dd.fmtY_Id === ddd.fmtY_Id && !ddd.fmqE_ManualEntryFlg) {
                                        $scope.tempoptionarray = [];
                                        angular.forEach($scope.mappedoptiondeetails, function (dddd) {
                                            if (dd.fmtY_Id === dddd.fmtY_Id && ddd.fmqE_Id === dddd.fmqE_Id) {
                                                $scope.tempoptionarray.push({ FMOP_Id: dddd.fmoP_Id, FMOP_FeedbackOptions: dddd.fmoP_FeedbackOptions });
                                            }
                                        });

                                        $scope.tempquestionarray.push({
                                            FMQE_Id: ddd.fmqE_Id, FMQE_FeedbackQuestions: ddd.fmqE_FeedbackQuestions,
                                            opt: $scope.tempoptionarray, manualflg: ddd.fmqE_ManualEntryFlg
                                        });
                                    } else {
                                        $scope.tempquestionarray.push({
                                            FMQE_Id: ddd.fmqE_Id, FMQE_FeedbackQuestions: ddd.fmqE_FeedbackQuestions,
                                            manualflg: ddd.fmqE_ManualEntryFlg
                                        });
                                    }
                                });
                                $scope.canteentemparray.push({ FMTY_Id: dd.fmtY_Id, FMTY_FeedbackTypeName: dd.fmtY_FeedbackTypeName, ques: $scope.tempquestionarray });
                            });
                        }

                        console.log($scope.canteentemparray);
                        //$('#CanteenFeedback').modal('show');
                    } else {
                        $scope.canteentemparray = [];
                        $scope.tempquestionarray = [];
                        $scope.tempoptionarray = [];
                        //swal("No Records Found");
                    }
                } else {
                    $scope.canteentemparray = [];
                    $scope.tempquestionarray = [];
                    $scope.tempoptionarray = [];
                    swal("Something Went Wrong Kidndly Contact Administrator");
                }

            });
        };
        //SAVE FEED BACK FOR THE CANTEEN ON DIALY BASIS -- adarsh ms
        $scope.submitted123 = false;
        $scope.savecanteenfeedback = function (user) {
            if ($scope.myForm123.$valid) {
                var data = {
                    temp: $scope.canteentemparray,
                    "Flag": 'canteen'
                };
                apiService.create("FeedbackSchoolGeneralTransaction/savecanteen", data).
                    then(function (promise) {
                        if (promise !== null) {
                            if (promise.returnval === true) {
                                swal("Feed Back Saved Successfully", "", "success");
                                $('#CanteenFeedback').modal('hide');
                            } else {
                                swal("Failed To Save Feed Back");
                            }
                        } else {
                            swal("Something Went Wrong Kindly Contact Administrator");
                        }
                       // $state.reload();
                    });
                
            }
            else {
                $scope.submitted123 = true;
            }
          
        };
        $scope.interactedcanteen = function (field) {
            return $scope.submitted123;
        };
        // ------------END CANTEEN SAVE----------------- -adarsh ms

        // ------------CONCALL SAVE SAVE----------------- -manohar nm
        $scope.reporttype = "";
        $scope.ConcallLOP = function (obj) {
            $scope.reporttype = "";
            var DRDate = $filter('date')($scope.ISMDRPT_Date, "dd-MM-yyyy");
            swal({
                title: "Have You Attended The Concall On  " + DRDate +  " ?",
                text: `
                    <div>
                          <span style="color:red;">Note 1: </span><b>If you click on <b>No Button</b>,  you have to submit con call task in DR by the end of the day or else it will lead to LOP.</b> <br> <br>
                     <span style="color:red;">Note 2: </span><b>If you click on <b>Yes Button</b>,  you must submit the con call category task in DR otherwise you won’t be able to generate DR!</b>
                    </div>`,
                html: true, 
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "YES",
                cancelButtonText: "NO",
                closeOnConfirm: true,
                closeOnCancel: true
            },
                function (isConfirm) {   
                    $scope.reporttype = isConfirm ? 'Yes' : 'No';
                    if ($scope.reporttype == "No") {
                        var data = {
                            "ISMDRPT_Date": new Date($scope.ISMDRPT_Date).toDateString(),
                            "reporttype": $scope.reporttype
                        };
                        apiService.create("ISM_ApprovalReport/deactivate", data).then(function (promise) {
                            if (promise.message != null && promise.message != "") {
                                if (promise.message === "saved") {
                                    swal("Con Call Attendance Updated", "", "success");
                                }
                                else if (promise.message === "failed") {
                                    swal("Failed To Update Con Call Attendance");
                                }
                                else {
                                    swal("Please Contact Administrator");
                                }

                            }
                        });
                    }
                    //else
                    //{
                    //    swal("Noted!", "You have to submit con call category task in DR!");
                    //}
                        
                });
        };

        // ------------END CONCALL SAVE SAVE----------------- -manohar nm

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };

        //Save DR NOT Sent Remarks

        $scope.SaveDRNotSentRemarks = function (drform) {
            if (drform.$valid) {
                $scope.SaveDRNotSentRemarksDetails = [];

                angular.forEach($scope.getdrnotsentdetails, function (dd) {
                    $scope.SaveDRNotSentRemarksDetails.push({ drnotsentdate: new Date(dd.FromDate).toDateString(), remarks: dd.Remarks, Template: 'DRNotSentRemarks' });
                });
                $scope.yesterdaysdated = new Date(new Date($scope.today).setDate(new Date($scope.today).getDate() - 1));
                var data = {
                    "SaveDRNotSentRemarksDetails": $scope.SaveDRNotSentRemarksDetails,
                    "ISMDRNSDR_RemarksDate": new Date($scope.today).toDateString()
                };

                apiService.create("ISM_DailyReportGeneration/SaveDRNotSentRemarks", data).then(function (promise) {
                    if (promise.returnval === true) {
                        swal("Remarks Captured");
                        $('#viewdrnotsentdetails').modal('hide');
                    }
                });
            } else {
                $scope.submitteddrdates = true;
            }
        };

        // Save HR Planner Remarks

        $scope.SaveHRPlannerRemarks = function (dd) {

            if (dd !== undefined && dd !== null && dd !== "") {
                $scope.SaveDRNotSentRemarksDetails = [];
                $scope.SaveDRNotSentRemarksDetails.push({ drnotsentdate: new Date().toDateString(), remarks: dd, Template: 'HRPlannerRemarks' });
                $scope.yesterdaysdated = new Date(new Date($scope.today).setDate(new Date($scope.today).getDate() - 1));

                var data = {
                    "SaveDRNotSentRemarksDetails": $scope.SaveDRNotSentRemarksDetails,
                    "ISMDRNSDR_RemarksDate": new Date($scope.today).toDateString()
                };

                apiService.create("ISM_DailyReportGeneration/SaveDRNotSentRemarks", data).then(function (promise) {
                    if (promise.returnval === true) {
                        swal("Remarks Captured");
                        $('#myModalhrplanner').modal('hide');
                    }
                });
            } else {
                swal("Enter Remarks");
            }
           
        };

        $scope.DeletRecord = function (stringdisplay, SweetAlert) {
            swal({
                title: "Daily Report Notification !",
                text: stringdisplay,
                type: "input",
                showCancelButton: false,
                closeOnConfirm: false,
                allowEscapeKey: false,
                closeOnClickOutside: false,
                inputPlaceholder: "Enter Remarks",
                confirmButtonText: "OK"
            }, function (inputValue) {
                if (inputValue === false) return false;
                if (inputValue === "") {
                    swal.showInputError("You need to write something!");
                    return false;
                }
                if (inputValue !== "") {
                    $scope.yesterdaysdated = new Date(new Date($scope.today).setDate(new Date($scope.today).getDate() - 1));
                    var data = {
                        "DRNotSentRemarks": inputValue,
                        "ISMDRNSDR_RemarksDate": new Date($scope.today).toDateString(),
                        "ISMDRNSDR_DRNotSentDate": new Date($scope.yesterdaysdated).toDateString()
                    };
                    apiService.create("ISM_DailyReportGeneration/SaveRemarks", data).then(function (promise) {
                        if (promise !== null) {
                            swal("OK");
                        }
                    });
                }
            });
        };

        //************* View The Blocked Reponse **************//
        $scope.viewblockresponse = function () {
            $scope.blockedreasond = $scope.blockeddetails[0].ismbE_Reason;
        };


        // *********** View Blocked Details List *********//
        $scope.saveDRremarks = function (DRRemarks) {
            if (DRRemarks !== undefined && DRRemarks !== null && DRRemarks !== "") {
                //$scope.RegeneratedDR

                var data = {
                    "ISMTRR_Remarks": $scope.DRRemarks,
                    //"ISMDRNSDR_RemarksDate": new Date($scope.today).toDateString()
                };

                apiService.create("ISM_DailyReportGeneration/saveDRremarks", data).then(function (promise) {
                    if (promise.returnval === true) {
                        swal("Remarks Captured");
                        $('#DrRechecking').modal('hide');
                    }
                });
            } else {
                swal("Enter Remarks");
            }
        };
        $scope.ViewBlockedDetails = function (dd) {

            var data = {
                "ISMEMN_ID": dd.ISMEMN_ID,
                "ISMBE_Id": dd.ISMBE_Id,
                "ISMBE_Reason": dd.ISMBE_Reason,
            };

            apiService.create("ISM_DailyReportGeneration/ViewBlockedDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.drblockeddetailslist = promise.drblockeddetailslist;
                    if ($scope.drblockeddetailslist !== null && $scope.drblockeddetailslist.length > 0) {
                        $('#blockeddetailslist').modal('show');
                    }


                }
            });
        };



        //************** On Change Of Radio Buttons *****************//
        $scope.onchangetodayorothersddate = function () {
            if ($scope.todayorothers === 'today') {
                $scope.datedisable = true;
                $scope.ISMDRPT_Date = new Date($scope.today);
                $scope.maxDatedof = new Date($scope.today);
                $scope.generate = true;
                $scope.generateothers = false;
                $scope.genrateflag = true;
            }
            else {
                if ($scope.othersdayenable === false) {
                    swal("Others Date Report Generation Is Not Available");
                    $scope.generate = false;
                    $scope.genrateflag = false;
                    $scope.generateothers = false;
                    $scope.datedisable = false;
                } else {
                    $scope.datedisable = false;
                    $scope.genrateflag = true;
                    $scope.generate = true;
                    $scope.generateothers = true;
                }
            }
        };


        //************** On Change Of Other Day date *****************//
        $scope.otherdaydatechange = function (flag) {
          
            var fullyear = $scope.ISMDRPT_Date.getFullYear();
            $scope.maxDatefP = new Date(fullyear, "11", "31");
            $scope.flag_othersdate = 0;
            if ($scope.todayorothers === 'today') {
                $scope.datedisable = true;
                $scope.ISMDRPT_Date = new Date($scope.today);
                $scope.maxDatedof = new Date($scope.today);
                $scope.generate = true;
                $scope.genrateflag = true;
                $scope.generateothers = false;
                $scope.extentiondays = $scope.extentiondaysload;
                $scope.dailyreportothersdatecount = $scope.dailyreportcountload;
            } else {
                if (flag === '1') {
                    var selecteddate = new Date($scope.ISMDRPT_Date).toDateString();
                    var today_date = new Date($scope.today).toDateString();
                    if (selecteddate === today_date) {
                        $scope.flag_othersdate = 1;
                        $scope.todayorothers = 'today';
                        $scope.datedisable = true;
                    }
                    
                }
            }
            //else {
            var data = {
                "ISMDRPT_Date": new Date($scope.ISMDRPT_Date).toDateString()
            };
            apiService.create("ISM_DailyReportGeneration/Otherdaycount", data).then(function (promise) {
                $scope.dailyreportothersdatecount = promise.dailyreportothersdatecount;
                $scope.dailyreportblockedcount = promise.dailyreportblockedcount;
                $scope.othersdayenable = promise.othersdayenable;
                $scope.extentiondays = 0;
                if (promise.dailyreportextensiondotherdate !== null && promise.dailyreportextensiondotherdate.length > 0) {
                    $scope.dailyreportextensiondotherdate = promise.dailyreportextensiondotherdate;
                    var totext = 0;
                    angular.forEach($scope.dailyreportextensiondotherdate, function (ext) {
                        totext += parseInt(ext.ismodE_Noofdays);
                    });
                    if (totext > 0) {
                        $scope.totalext = totext;
                    }
                    else {
                        $scope.totalext = 0;
                    }
                    $scope.extentiondays = promise.extensiondays;

                }
                else {
                    $scope.dailyreportextensiondotherdate = [];
                }
                if ($scope.othersdayenable === false) {

                    if ($scope.todayorothers !== 'today') {
                        swal("Others Date Report Generation Is Not Available");
                        $scope.generate = false;
                        $scope.genrateflag = false;
                        $scope.generateothers = false;
                        $scope.datedisable = false;
                        return;
                    }
                } else {
                    if ($scope.flag_othersdate === 1) {
                        $scope.datedisable = true;
                    } else {
                        $scope.datedisable = false;
                    }

                    $scope.genrateflag = true;
                    $scope.generate = true;
                    $scope.generateothers = true;
                }

                //if (flag === '1')
                //{
                //    if (promise.get_userEmplist != null && promise.get_userEmplist.length > 0 && promise.get_userEmplist[0].DRConcall != 1) {
                //        $scope.ConcallLOP();

                //    }
                //}

                var countdrapproved = 0;

                if (promise.drnotapprovedmessage !== undefined && promise.drnotapprovedmessage !== null && promise.drnotapprovedmessage.length > 0) {


                    $scope.generate = false;
                    countdrapproved = 1;

                    $scope.drnotapprovedmessage = promise.drnotapprovedmessage;
                    $scope.TempEmployee = [];

                    angular.forEach($scope.drnotapprovedmessage, function (dd) {
                        if ($scope.TempEmployee.length === 0) {
                            $scope.TempEmployee.push(dd);
                        } else if ($scope.TempEmployee.length > 0) {
                            var countemp = 0;
                            angular.forEach($scope.TempEmployee, function (d) {
                                if (d.HRME_Id === dd.HRME_Id) {
                                    countemp += 1;
                                }
                            });
                            if (countemp === 0) {
                                $scope.TempEmployee.push(dd);
                            }
                        }
                    });

                    angular.forEach($scope.TempEmployee, function (dd) {
                        $scope.TempDates_Details = [];
                        angular.forEach($scope.drnotapprovedmessage, function (d) {
                            if (dd.HRME_Id === d.HRME_Id) {
                                $scope.TempDates_Details.push(d);
                            }
                        });
                        dd.DatesDetails = $scope.TempDates_Details;
                    });

                    $('#viewdrnotapproveddetails').modal('show');

                    //$scope.statusdrapproved = promise.drnotapprovedmessage[0].Stauts;
                    //$scope.datedrapproved = promise.drnotapprovedmessage[0].Date;
                    //if ($scope.statusdrapproved === "Pending") {
                    //    $scope.generate = false;
                    //    countdrapproved = 1;
                    //    swal("Daily Report Is Not Apporved For Your Team On " + $scope.datedrapproved + " So, You Can Not Generated Daily Report");
                    //}
                    //return;
                }

                //if (promise.message === "Generated") {
                //    $scope.generate = false;
                //    swal("Already Daily Report Generated For This Date");
                //} else {
                    if (countdrapproved === 0) {
                        $scope.generate = true;
                    } else {
                        $scope.generate = false;
                    }
                    $scope.get_effortdetails = promise.get_effortdetails;

                    if ($scope.get_effortdetails !== null && $scope.get_effortdetails.length > 0) {
                        $scope.workingefforts = $scope.get_effortdetails[0].WORKINGHOURS;
                    }

                    $scope.get_plannerlist = promise.gettasklist;

                    angular.forEach($scope.get_plannerlist, function (dd) {

                        dd.deviationflag = false;
                        dd.effhrs = 0;
                        dd.effmin = 0;

                        if (dd.dr_flag === 1) {
                            dd.checkedvalue = true;
                            dd.deviationflag = dd.ISMDRPT_DeviationFlg;
                            if (dd.ISMDRPT_ExtraFlg === true) {
                                dd.extraflag = 1;
                            }


                            if (dd.ISMDRPT_TimeTakenInHrs !== null && dd.ISMDRPT_TimeTakenInHrs !== "") {
                                var decimaltimetakenhrs = dd.ISMDRPT_TimeTakenInHrs;
                                var hrstimetaken = parseInt(Number(decimaltimetakenhrs));
                                var mintimetaken = Math.round((Number(decimaltimetakenhrs) - hrstimetaken) * 60);
                                dd.effhrs = hrstimetaken;
                                dd.effmin = mintimetaken;
                            }
                            $scope.tempfiles_list = [];
                            if ($scope.getfiledetails !== null && $scope.getfiledetails.length > 0) {

                                angular.forEach($scope.getfiledetails, function (files) {
                                    if (files.ismtcR_Id == dd.ISMTCR_Id && dd.ISMTPLTA_Id == files.ismtpltA_Id) {
                                        $scope.tempfiles_list.push({
                                            checklistname: files.ismmtcatcL_CheckListName, filepath: files.ismdrptfL_FilePath, filename: files.ismdrptfL_FileName,
                                            comments: files.ismdrptfL_Remarks, refno: files.ismdrptfL_RefNo, ISMMTCATCL_Id: files.ismmtcatcL_Id,
                                            ISMDRPTFL_Id: files.ismdrptfL_Id
                                        });
                                    }
                                });

                                dd.categorychecklist = $scope.tempfiles_list;
                            }
                        }

                        // Task Wise Status List
                        $scope.tempstatus_Temp = [];
                        $scope.tempstatus = [];
                        angular.forEach($scope.get_Status, function (dd) {
                            if (dd.ismmistS_StatusName.toLowerCase() !== 'onhold-department' && dd.ismmistS_StatusName.toLowerCase() !== "onhold"
                                && dd.ismmistS_StatusName.toLowerCase() !== "management blocker") {
                                $scope.tempstatus_Temp.push(dd);
                            }
                        });
                        if (dd.CreatedFlag === 1) {
                            dd.get_Status = $scope.tempstatus_Temp;
                        } else {
                            angular.forEach($scope.tempstatus_Temp, function (task) {
                                if (task.ismmistS_StatusName != "Close" && task.ismmistS_StatusName != "close") {
                                    $scope.tempstatus.push(task);
                                };
                            });
                            dd.get_Status = $scope.tempstatus;
                        }


                        if (dd.ISMTAPL_Periodicity === 'Daily' && dd.periodicitydailyflag === 1) {
                            //dd.checkedvalue = true;
                            dd.disableflag = 1;
                        }

                        if (dd.ISMTAPL_Periodicity === 'Daily' && dd.periodicitydailyflag === 2) {
                            dd.disableflag = 1;
                        }

                        if (dd.ISMTAPL_Periodicity === 'Weekly' && dd.periodicityweeklyflag === 0) {
                            dd.disableflag = 1;
                        }

                        if (dd.ISMTAPL_Periodicity === 'Weekly' && dd.periodicityweeklyflag === 1) {
                            dd.disableflag = 1;
                            //dd.checkedvalue = true;
                        }

                        if (dd.checkedvalue === true) {
                            $scope.taskenddate = new Date(dd.ISMTPL_EndDate);
                            $scope.todaydate = new Date($scope.ISMDRPT_Date);
                            var enddate = $scope.curdate === null ? "" : $filter('date')($scope.taskenddate, "yyyy-MM-dd");
                            var curntdate = $scope.todaydate === null ? "" : $filter('date')($scope.todaydate, "yyyy-MM-dd");
                            if (curntdate > enddate) {
                                dd.deviationflag = true;
                            } else {
                                dd.deviationflag = false;
                            }
                        }

                        var decimaltimetot = dd.ISMTPLTA_EffortInHrs;

                        var hrstot = parseInt(Number(decimaltimetot));
                        var mintot = Math.round((Number(decimaltimetot) - hrstot) * 60);

                        if (hrstot.toString().length === 1) {
                            hrstot = '0' + hrstot;
                        }

                        if (mintot.toString().length === 1) {
                            mintot = '0' + mintot;
                        }

                        dd.actualeffortinhrs = hrstot;
                        dd.actualeffortinmins = mintot;

                        //dd.effhrs = 0;
                        //dd.effmin = 0;

                    });

                    $scope.lblplannername = promise.plannername;
                    $scope.plannerdetails = promise.plannerdetails;

                    $scope.totaltask = promise.plannerlength;
                    $scope.getplannerdetails = promise.getplannerdetails;

                    if ($scope.getplannerdetails !== null && $scope.getplannerdetails.length > 0) {

                        $scope.getapprovedfalg = $scope.getplannerdetails[0].ismtpL_ApprovalFlg;
                        $scope.getapprovedby = $scope.getplannerdetails[0].ismtpL_ApprovedBy;
                        $scope.ISMTPL_ApprovedTo = $scope.getplannerdetails[0].ismtpL_ApprovedTo;
                        if ($scope.getapprovedfalg === false && $scope.getapprovedby === 0 ) {
                            $scope.Approvalflag = false;
                            $scope.totaltask = 0;
                            swal("Still Your Planner Is Pending , So Kindly Contact Department Head");
                            return;
                        }

                        else if ($scope.getapprovedfalg === false && $scope.getapprovedby > 0) {
                            $scope.Approvalflag = false;
                            $scope.totaltask = 0;
                            swal("Your Planner Is Rejected , So Kindly Contact Department Head");
                            return;
                        } else {
                            $scope.Approvalflag = true;
                            $scope.totaltask = promise.plannerlength;
                            if (flag === '1') {
                                if (promise.get_userEmplist != null && promise.get_userEmplist.length > 0 && promise.get_userEmplist[0].DRConcall != 1) {
                                    $scope.ConcallLOP();

                                }
                                $scope.get_userEmplistDemoRating = promise.get_userEmplistDemoRating;
                                if ($scope.get_userEmplistDemoRating != null && $scope.get_userEmplistDemoRating.length > 0 && $scope.get_userEmplistDemoRating[0].FLAG == "0") {
                                    $('#Demoratingmodel').modal('show');
                                }
                                else if ($scope.get_userEmplistDemoRating != null && $scope.get_userEmplistDemoRating.length > 0 && $scope.get_userEmplistDemoRating[0].FLAG == "1") {
                                    $('#DemoratingmodelDrblock').modal('show');
                                }
                            }
                        }
                    } else {
                        swal("Planner Not Created For This Week!!");
                        return;
                      
                    }
                //}
            });
            //}
        };

        //**** Check Required Check Box ****//
        $scope.isOptionsRequired = function () {
            return !$scope.get_plannerlist.some(function (options) {
                return options.checkedvalue;
            });
        };

        //******* Save Daily Report ****** //
        $scope.generatedailyreport = function () {
            $scope.selectedlist = [];

            if ($scope.myForm.$valid) {

                if ($scope.todayorothers === 'others') {
                    $scope.othersselecteddate = new Date($scope.ISMDRPT_Date);
                    $scope.todaydatenew = new Date($scope.today);

                    var enddatenew = $filter('date')($scope.othersselecteddate, "yyyy-MM-dd");
                    var curntdatenew = $filter('date')($scope.todaydatenew, "yyyy-MM-dd");

                    if (curntdatenew === enddatenew) {
                        swal("Other's Date Should Not Be Equal To Today's Date");
                        return;
                    }
                }


                angular.forEach($scope.get_plannerlist, function (selected) {
                    if (selected.checkedvalue === true) {
                        $scope.selectedlist.push(selected);
                    }
                });

                var counthrs = 0.00;
                var totalhrs = 0.00;
                angular.forEach($scope.selectedlist, function (countefforts) {

                    //if (countefforts.ISMDRPT_TimeTakenInHrsmin === "Hours") {
                    //    counthrs = parseFloat(countefforts.efforts);
                    //} else if (countefforts.ISMDRPT_TimeTakenInHrsmin === "Minutes") {
                    //    counthrs = (parseFloat(countefforts.efforts) * 0.0166667).toFixed(2);
                    //}
                    //totalhrs += parseFloat(counthrs);

                    var counthrs = 0.00;
                    counthrs = (parseFloat(countefforts.effmin) * 0.0166667).toFixed(2);

                    counthrs = parseFloat(countefforts.effhrs) + parseFloat(counthrs);

                    countefforts.ISMDRPT_TimeTakenInHrs = counthrs;

                    totalhrs += parseFloat(counthrs);
                    countefforts.ISMDRPT_TimeTakenInHrsmin = "Hours";
                });

                if (totalhrs <= 0) {
                    swal("Daily Report Can Not Be Generate For Zero Hours", "Total Efforts Selected " + totalhrs + " Hours");
                    return;
                }

                var totalworkinghrsflag = 0;
                if ($scope.halfday === false) {
                    var effortss = parseFloat($scope.workingefforts);

                    if (totalhrs >= effortss) {
                        totalworkinghrsflag = 1;
                    }

                    if (totalhrs > 24) {
                        swal("Effort Should Be  Less Than Equal To 24 Hours Per Day ", "Total Efforts Selected " + totalhrs + " Hours");
                        return;
                    }
                } else if ($scope.halfday === true) {
                    var efforts = parseFloat($scope.workingefforts) / 2;
                    if (totalhrs === efforts) {
                        totalworkinghrsflag = 1;
                    }
                }
                var ConcalRoleFlag = false;
                //generatedailyreport
                if ($scope.reporttype == "Yes") {
                    angular.forEach($scope.selectedlist, function (selected) {
                        if (selected.taskcategoryname.toUpperCase() == "CON CALL") {
                            ConcalRoleFlag = true;
                        }
                    });
                    if (ConcalRoleFlag == false) {
                        swal("You Have Attended a Con Call Today. Please Select The Con Call Task Category For DR !");
                        return;
                    }
                }
                console.log($scope.selectedlist);
                var data = {
                    "Temp_ISM_DailyReportGenerationDTO": $scope.selectedlist,
                    "ISMDRPT_Date": new Date($scope.ISMDRPT_Date).toDateString(),
                    "todayorothers": $scope.todayorothers,
                    "reason": $scope.reason,
                    "ISMTPL_Id": $scope.plannerdetails[0].ismtpL_Id,
                    "ISMDRPT_HalfDayFlag": $scope.halfday,

                    "deviation_id": $scope.deviation_id,
                    "startweek": new Date($scope.startweek).toDateString(),
                    "endweek": new Date($scope.endweek).toDateString(),
                    "totalworkinghrsflag": totalworkinghrsflag
                };

                swal({
                    title: "Are you sure?",
                    text: "Total No. Of Hours For Daily Report : " + totalhrs + " Hrs",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Send Daily Report",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("ISM_DailyReportGeneration/generatedailyreport", data).then(function (promise) {
                                if (promise !== null) {
                                    if (promise.message === "Generated") {
                                        swal("Already Daily Report Generated For This Date", "", "info");
                                    } else {
                                        if (promise.returnval === true) {
                                            swal("Daily Report Generated Successfully", "", "success");
                                        } else {
                                            swal("Failed To Save Daily Report", "", "warning");
                                        }
                                    }
                                } else {
                                    swal("Failed To Save Daily Report", "", "warning");
                                }
                                $state.reload();
                               
                            });
                        }
                        else {
                            swal("Cancelled");
                        }
                    });
            } else {
                $scope.submitted = true;
            }
        };


        //***** Search  Previous Others Date Saved Daily Report ****//
        $scope.searchpreviousdailyreport = function () {
            $scope.getsearchdailyreport = [];
            if ($scope.ISMDRPT_Date_others !== undefined && $scope.ISMDRPT_Date_others !== null && $scope.ISMDRPT_Date_others !== '') {

                var data = {
                    "ISMDRPT_Date": new Date($scope.ISMDRPT_Date_others).toDateString()
                };
                apiService.create("ISM_DailyReportGeneration/searchpreviousdailyreport", data).then(function (promise) {
                    if (promise.getsearchdailyreport !== null && promise.getsearchdailyreport.length > 0) {

                        $scope.getsearchdailyreport = promise.getsearchdailyreport;
                        $scope.droveralldetailsdatewise = promise.droveralldetailsdatewise;

                        var total_DrHrs = 0;
                        var total_ApprovedHrs = 0;

                        angular.forEach($scope.getsearchdailyreport, function (dd) {

                            var decimaltimetot = dd.efforts;
                            total_DrHrs += decimaltimetot;

                            var hrstot = parseInt(Number(decimaltimetot));
                            var mintot = Math.round((Number(decimaltimetot) - hrstot) * 60);

                            if (hrstot.toString().length === 1) {
                                hrstot = '0' + hrstot;
                            }

                            if (mintot.toString().length === 1) {
                                mintot = '0' + mintot;
                            }

                            dd.actualeffortinhrs = hrstot;
                            dd.actualeffortinmins = mintot;

                            if (dd.ISMDRPT_ApprovedTime !== null && dd.ISMDRPT_ApprovedTime !== "") {

                                var decimaltimetotapproved = dd.ISMDRPT_ApprovedTime;
                                total_ApprovedHrs += decimaltimetotapproved;

                                var hrstotapproved = parseInt(Number(decimaltimetotapproved));
                                var mintotapproved = Math.round((Number(decimaltimetotapproved) - hrstotapproved) * 60);

                                if (hrstotapproved.toString().length === 1) {
                                    hrstotapproved = '0' + hrstotapproved;
                                }

                                if (mintotapproved.toString().length === 1) {
                                    mintotapproved = '0' + mintotapproved;
                                }

                                dd.actualeffortinhrsapproved = hrstotapproved;
                                dd.actualeffortinminsapproved = mintotapproved;
                            }
                        });

                        // Total DR Hours 
                        var total_drhr = parseInt(Number(total_DrHrs));
                        var total_drmins = Math.round((Number(total_DrHrs) - total_drhr) * 60);

                        if (total_drhr.toString().length === 1) {
                            total_drhr = '0' + total_drhr;
                        }

                        if (total_drmins.toString().length === 1) {
                            total_drmins = '0' + total_drmins;
                        }

                        // Total DR Approved Hours 
                        if (total_ApprovedHrs !== null) {
                            var total_drapprovedhr = parseInt(Number(total_ApprovedHrs));
                            var total_drapprovedmins = Math.round((Number(total_ApprovedHrs) - total_drapprovedhr) * 60);

                            if (total_drapprovedhr.toString().length === 1) {
                                total_drapprovedhr = '0' + total_drapprovedhr;
                            }

                            if (total_drapprovedmins.toString().length === 1) {
                                total_drapprovedmins = '0' + total_drapprovedmins;
                            }

                            $scope.TotalDrApproveHrs = total_drapprovedhr + ':' + total_drapprovedmins + ' Hrs';
                            $scope.TotalDrHrs = total_drhr + ':' + total_drmins + ' Hrs';


                            //Rejected Hours 
                            var RejectedHrs = Number(total_DrHrs) - Number(total_ApprovedHrs);
                            var total_drrejectedhr = parseInt(Number(RejectedHrs));
                            var total_drrejectedmins = Math.round((Number(RejectedHrs) - total_drrejectedhr) * 60);


                            if (total_drrejectedmins.toString().length === 1) {
                                total_drrejectedmins = '0' + total_drrejectedmins;
                            }

                            if (total_drrejectedhr.toString().length === 1) {
                                total_drrejectedhr = '0' + total_drrejectedhr;
                            }

                            $scope.TotalDrRejectedHrs = total_drrejectedhr + ':' + total_drrejectedmins + ' Hrs';

                        }

                        $scope.DROverallcomments = $scope.droveralldetailsdatewise[0].ismdrptdW_OverallComments;

                        if ($scope.DROverallcomments === null || $scope.DROverallcomments === '') {
                            $scope.TotalDrRejectedHrs = "00:00 Hrs";
                        }

                        //$("#myModalreadmitview").modal("show");
                    }
                    else {
                        $scope.getsearchdailyreport = [];
                        swal("No Records Found");
                        $("#myModalreadmitview").modal("hide");
                        $('.modal-backdrop').remove();
                    }
                });
            } else {
                swal("Select The Date First");
            }
        };


        // ****** Add Extra Task To Planner ****** //
        $scope.addextratask = function () {
            $scope.search = "";
            var pageid = 2;

            var data = {
                "ISMDRPT_Date": new Date($scope.ISMDRPT_Date).toDateString()
            };

            apiService.create("ISM_DailyReportGeneration/addextratask", data).then(function (promise) {
                $scope.taskplanner = promise.getextratasks;
                $scope.temparray = [];
                $scope.temparraynew = [];
                angular.forEach($scope.taskplanner, function (cc) {
                    var count = 0;
                    angular.forEach($scope.get_plannerlist, function (dd) {
                        if (dd.ISMTCR_Id === cc.ISMTCR_Id)
                            count += 1;
                    });
                    if (count === 0) {
                        $scope.temparray.push(cc);
                    }
                });

                console.log($scope.temparray);

                $scope.temparraynew = $scope.temparray;

                angular.forEach($scope.temparraynew, function (tas) {
                    tas.ISMTPLTA_StartDate = new Date(tas.startdatenew);
                    tas.ISMTPLTA_EndDate = new Date(tas.enddatenew);

                    //angular.forEach($scope.temparray, function (temp) {
                    //    if (tas.ISMTCR_Id === temp.ISMTCR_Id)
                    //        $scope.temparraynew.push(tas);
                    //});
                });

                if ($scope.taskplanner.length > 0) {
                    //$('#myModalAddtoPlanner').modal('show');
                }
                else {
                    swal("Task List Not Found");
                    $('#myModalAddtoPlanner').modal('hide');
                }
            });
        };

        $scope.saveAddTask = function () {
            if ($scope.myForm1.$valid) {
                angular.forEach($scope.taskplanner, function (cc) {
                    var count = 0;
                    if (cc.checkedTask === true) {
                        cc.ISMTPL_Id = $scope.plannerdetails[0].ismtpL_Id;
                        cc.extraflag = 1;

                        $scope.tempstatus_Temp = [];
                        $scope.tempstatus = [];
                        angular.forEach($scope.get_Status, function (dd) {
                            if (dd.ismmistS_StatusName.toLowerCase() !== 'onhold-department' && dd.ismmistS_StatusName.toLowerCase() !== "onhold"
                                && dd.ismmistS_StatusName.toLowerCase() !== "management blocker") {
                                $scope.tempstatus_Temp.push(dd);
                            }
                        });

                        if (cc.CreatedFlag === 1) {
                            cc.get_Status = $scope.tempstatus_Temp;
                        } else {
                            angular.forEach($scope.tempstatus_Temp, function (task) {
                                if (task.ismmistS_StatusName != "Close" && task.ismmistS_StatusName != "close") {
                                    $scope.tempstatus.push(task);
                                };
                            });
                            cc.get_Status = $scope.tempstatus;
                        }


                        //cc.ISMTPLTA_StartDate = new Date(cc.ISMTPLTA_StartDate).toDateString();
                        //cc.ISMTPLTA_EndDate = new Date(cc.ISMTPLTA_EndDate).toDateString();                       

                        cc.ISMTPL_StartDate = new Date(cc.ISMTPLTA_StartDate).toDateString();
                        cc.ISMTPL_EndDate = new Date(cc.ISMTPLTA_EndDate).toDateString();

                        cc.ISMTPLTA_StartDate = new Date(cc.ISMTPLTA_StartDate).toDateString();
                        cc.ISMTPLTA_EndDate = new Date(cc.ISMTPLTA_EndDate).toDateString();

                        cc.deviationflag = false;

                        var decimaltimetot = cc.ISMTPLTA_EffortInHrs;

                        var hrstot = parseInt(Number(decimaltimetot));
                        var mintot = Math.round((Number(decimaltimetot) - hrstot) * 60);

                        if (hrstot.toString().length === 1) {
                            hrstot = '0' + hrstot;
                        }

                        if (mintot.toString().length === 1) {
                            mintot = '0' + mintot;
                        }

                        cc.actualeffortinhrs = hrstot;
                        cc.actualeffortinmins = mintot;

                        cc.effhrs = 0;
                        cc.effmin = 0;

                        $scope.get_plannerlist.push(cc);
                    }
                });
                $('#myModalAddtoPlanner').modal('hide');
                $('.modal-backdrop').remove();
            } else {
                $scope.submitted1 = true;
                //$('#myModalAddtoPlanner').modal('show');
            }
            console.log($scope.get_plannerlist);
        };

        // ****** View The Task Description ****** //
        $scope.viewresponse = function (taskobj) {
            //$('#myModalResponse').modal('show');
            $scope.temptaskreponsefiles = [];
            $scope.temptaskreponsefiles.push({ 'id': 'Teacher1' });
            $scope.ismtcrreS_Response = "";
            $scope.files_paths = "";
            $scope.ismmistS_StatusName = "";
            $scope.taskAttatchment = [];
            $scope.get_responseattachments = [];
            $scope.taskid = taskobj.ISMTCR_Id;
            $scope.plannertaskid = taskobj.ISMMTCAT_Id;
            $scope.created_by = taskobj.createdemp;
            $scope.files = [];
            var data = {
                "ISMTCR_Id": $scope.taskid
            };
            apiService.create("ISM_DailyReportGeneration/viewresponse", data).then(function (promise) {
                $scope.get_taskdetails = promise.get_taskdetails;
                $scope.get_taskresponse = promise.get_taskresponse;
                if ($scope.get_taskdetails !== null && $scope.get_taskdetails.length > 0) {
                    $scope.tasktitle = $scope.get_taskdetails[0].ISMTCR_Title;
                    $scope.taskno = $scope.get_taskdetails[0].ISMTCR_TaskNo;
                    $scope.taskclient = $scope.get_taskdetails[0].ISMMCLT_ClientName;
                    $scope.taskdate = $scope.get_taskdetails[0].ISMTCR_CreationDate;
                    $scope.tasktype = $scope.get_taskdetails[0].tasktype;
                    $scope.tsakpriority = $scope.get_taskdetails[0].HRMP_Name;
                    $scope.taskdescription = $scope.get_taskdetails[0].ISMTCR_Desc;
                    $scope.taskstatus = $scope.get_taskdetails[0].ISMTCR_Status;
                    $scope.createdby = $scope.get_taskdetails[0].createdby;
                    $scope.startdate = new Date($scope.get_taskdetails[0].ISMTCRASTO_StartDate).toDateString();
                    $scope.enddate = new Date($scope.get_taskdetails[0].ISMTCRASTO_EndDate).toDateString();
                    $scope.taskAttatchment = promise.get_taskattachments;
                    $scope.get_responseattachments = promise.get_responseattachments;

                    if ($scope.taskAttatchment.length > 0) {
                        angular.forEach($scope.taskAttatchment, function (at) {
                            if (at.ISMTCRAT_Attatchment !== null && at.ISMTCRAT_Attatchment !== "") {
                                var img = at.ISMTCRAT_Attatchment;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                at.filetype = lastelement;
                            }
                        });
                    }
                    if ($scope.get_responseattachments !== null && $scope.get_responseattachments.length > 0) {
                        angular.forEach($scope.get_responseattachments, function (at) {
                            if (at.ISMTCRRES_ResponseAttachment !== null && at.ISMTCRRES_ResponseAttachment !== "") {
                                var img1 = at.ISMTCRRES_ResponseAttachment;
                                var imagarr1 = img1.split('.');
                                var lastelement1 = imagarr1[imagarr1.length - 1];

                                at.filetype = lastelement1;
                            }
                        });
                    }

                    angular.forEach($scope.get_plannerlist, function (dd) {
                        if (dd.ISMTPLTA_Id === taskobj.ISMTPLTA_Id) {
                            $scope.taskstartdate1 = taskobj.ISMTPLTA_StartDate;
                            $scope.taskenddate1 = taskobj.ISMTPLTA_EndDate;
                        }
                    });

                    angular.forEach($scope.get_plannerlist, function (obj) {



                        if ($scope.taskid === obj.ISMTCR_Id && $scope.plannertaskid == obj.ISMMTCAT_Id) {

                            if (obj.Temp_TaskReponseList !== undefined && obj.Temp_TaskReponseList !== null && obj.Temp_TaskReponseList.length > 0) {
                                $scope.temptaskreponsefiles = obj.Temp_TaskReponseList;
                                angular.forEach($scope.temptaskreponsefiles, function (dd) {
                                    if (obj.ISMDRPT_CompoffApplicableflag==1) {
                                        dd.attachmentmadflag = true;
                                    }
                                    else {
                                        dd.attachmentmadflag = false;
                                    }
                                    var img = dd.filepath;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];
                                    dd.filetype = lastelement;
                                    console.log("dd.filetype : " + dd.filetype);
                                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                        dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
                                    }
                                });
                            }
                        }
                    });
                }
            });
        };

        $scope.addNewviewresponse = function () {
            var newItemNo = $scope.temptaskreponsefiles.length + 1;

            if (newItemNo <= 10) {
                $scope.temptaskreponsefiles.push({ 'id': 'Teacher' + newItemNo });
            }
            console.log($scope.temptaskreponsefiles);
        };

        $scope.removeNewviewresponse = function (index) {
            var newItemNo = $scope.temptaskreponsefiles.length - 1;
            $scope.temptaskreponsefiles.splice(index, 1);

            if ($scope.temptaskreponsefiles.length === 0) {
                //data
            }
        };

        $scope.uploadviewresponsedocuments1 = [];

        $scope.uploadviewresponsfile = function (input, document) {

            $scope.uploadviewresponsedocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/jpg" || input.files[0].type === "image/png") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-powerpoint") {
                    UploaddianPhoto(document);
                }
                else {
                    swal("Upload MP4, Pdf, Doc, Image Files Only");
                }
            }
        };

        function UploaddianPhoto(data) {
            console.log("Teacher Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadviewresponsedocuments1.length; i++) {
                formData.append("File", $scope.uploadviewresponsedocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/DRReportCheckList", formData,
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': undefined
                    },
                    transformRequest: angular.identity,

                })
                .success(function (d) {
                    defer.resolve(d);
                    data.filepath = d;
                    data.filename = $scope.filename;
                    $('#').attr('src', data.filepath);
                    var img = data.filepath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.filepath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        // ****** ADD FILES TO PARTICLAR TASK ********** //
        $scope.responsetask = function () {
            if ($scope.temptaskreponsefiles !== null && $scope.temptaskreponsefiles !== undefined && $scope.temptaskreponsefiles.length > 0) {
                // UploadFiles();

                $scope.tempfiles = [];

                angular.forEach($scope.temptaskreponsefiles, function (dd) {
                    if (dd.filepath !== undefined && dd.filepath !== null && dd.filepath !== "") {
                        $scope.tempfiles.push({ filepath: dd.filepath, filename: dd.filename });
                    }
                });

                if ($scope.tempfiles != null && $scope.tempfiles.length > 0) {

                    angular.forEach($scope.get_plannerlist, function (obj) {
                        if ($scope.taskid === obj.ISMTCR_Id && $scope.plannertaskid == obj.ISMMTCAT_Id) {
                            obj.Temp_TaskReponseList = $scope.tempfiles;
                            console.log(obj);
                        }
                    });
                    $('#myModalResponse').modal('hide');
                }
                else {
                    swal("Please Select Atleast One Attachment !");
                    $('#myModalResponse').modal('show');
                }
            }
           
        };

        // ****** On Change Of Efforts *******//
        $scope.oneffortchange = function (obja) {
            if (parseFloat(obja.efforts) === 0) {
                angular.forEach($scope.get_plannerlist, function (ext) {
                    if (obja.ISMTPLTA_Id === ext.ISMTPLTA_Id) {
                        swal('Efforts Should Be Greather Than 0 For Task :' + obja.ISMTCR_TaskNo);
                        obja.efforts = "";
                    }
                });
            } else {
                if (parseFloat(obja.efforts) > 60 && obja.ISMDRPT_TimeTakenInHrsmin === 'Minutes') {
                    angular.forEach($scope.get_plannerlist, function (ext) {
                        if (obja.ISMTPLTA_Id === ext.ISMTPLTA_Id) {
                            swal('You Can Not Give More Than 60 Min For Task :' + obja.ISMTCR_TaskNo);
                            obja.efforts = "";
                        }
                    });
                }
            }
        };
       
        // ****** Checking Deviation Remarks Required ****** //
        $scope.CheckDeviationRemarks = function (objs, index) {
            $scope.documentFlag = false;
            if (objs.checkedvalue === true) {
                $scope.savedfiles = [];

                $scope.checklist_taskplannerid = objs.ISMTPLTA_Id;
                $scope.checklist_categoryid = objs.ISMMTCAT_Id;
                $scope.checklist_arrayindex = index;
                $scope.checklist_taskid = objs.ISMTCR_Id;

                $scope.taskenddate = new Date(objs.ISMTPL_EndDate);
                $scope.todaydate = new Date($scope.ISMDRPT_Date);

                var enddate = $scope.curdate === null ? "" : $filter('date')($scope.taskenddate, "yyyy-MM-dd");
                var curntdate = $scope.todaydate === null ? "" : $filter('date')($scope.todaydate, "yyyy-MM-dd");

                $scope.tempcategorychecklist = [];
                $scope.tempcategorychecklistfiles = [];
                $scope.ISMMTCATCL_Id = "";

                if (objs.ISMTAPL_Periodicity === "Daily") {

                    var taskdailystartdate = $filter('date')(new Date(objs.ISMTPLTA_StartDate), "yyyy-MM-dd");
                    var taskdailycurrentdate = $filter('date')(new Date($scope.ISMDRPT_Date), "yyyy-MM-dd");
                }

                if (curntdate > enddate) {
                    objs.deviationflag = true;
                    swal("Task End Date Is  " + enddate + "  Kindly Select Remarks ");
                } else {
                    objs.deviationflag = false;
                }

                var data = {
                    "ISMTCR_Id": objs.ISMTCR_Id,
                    "ISMMTCAT_Id": objs.ISMMTCAT_Id
                };

                apiService.create("ISM_DailyReportGeneration/GetCategoryChecklistDetails", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.gettaskcategorychecklist = promise.gettaskcategorychecklist;
                        $scope.tempcategorychecklist = $scope.gettaskcategorychecklist;
                        if ($scope.tempcategorychecklist !== null && $scope.tempcategorychecklist.length > 0) {
                            $('#categorychecklist').modal('show');                            
                            $('#myModalResponse').modal('hide');
                        }
                        else {
                            //
                            if (objs.Temp_TaskReponseList != null && objs.Temp_TaskReponseList.length > 0) {

                            }
                            else {
                                if (objs.effhrs >= 1) {
                                    $scope.documentFlag = true;
                                    $scope.viewresponse(objs);
                                    $('#myModalResponse').modal('show');
                                }
                            }
                           
                        }
                    }
                });
            } else {
                //if (objs.dr_flag === 1) {
                //    objs.deviationflag = true;
                //    swal("You Can't Un Check This Task, Daily Report Is Already Sent For This Task");
                //} else {
                objs.deviationflag = false;
                //}
            }
        };

        $scope.previewimg = function (img) {
            $scope.imagepreview = img;
            $('#preview').attr('src', $scope.imagepreview);
            $('#myModalPreview').modal('show');
            $('.modal').on('hide.bs.modal', function (e) {
                e.stopPropagation();
                $('body').css('padding-right', '');
            });
        };

        // ****** Close Image Model ********** //
        $scope.closeimg = function () {
            $('#myModalPreview').modal('hide');
            $('.modal-backdrop').remove();
        };

        // ******* File Upload *************//        
        $scope.stepsModel = [];
        $scope.filedata = [];

        $scope.selectFileforUpload = function (event) {
            $scope.files = event.files;
            for (var i = 0; i < $scope.files.length; i++) {
                var file = $scope.files[i].name;
                $scope.filedata = $scope.files;
                $scope.fileimg = file;
                var reader = new FileReader();
                reader.onload = $scope.imageIsLoaded;
                reader.readAsDataURL($scope.files[i]);
            }
        };

        // ******* Remove Selected File *************//  
        $scope.remove_file = function (index) {
            $scope.file_detail = "";
            $scope.files = "";
            $scope.files_paths = "";
        };

        $scope.imageIsLoaded = function (e) {
            $scope.$apply(function () {
                $scope.stepsModel.push(e.target.result);
            });
        };

        function UploadFiles() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.files.length; i++) {
                formData.append("File", $scope.files[i]);
                //   $scope.file_detail = $scope.files[i].name;
            }
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_ISMAttachment", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    swal(d);
                    $scope.files_paths = d;
                    if ($scope.files_paths.length > 0) {
                        //DD NEED TO CHANGE                      

                        angular.forEach($scope.get_plannerlist, function (obj) {
                            if ($scope.taskid === obj.ISMTCR_Id && $scope.plannertaskid == obj.ISMMTCAT_Id) {
                                obj.attachmentArray = $scope.files_paths;
                                console.log(obj);
                            }
                        });
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.interacted6 = function (field) {
            return $scope.submitted6;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacteddrdates = function (field) {
            return $scope.submitteddrdates;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.onviewpdf = function (filepath, filename) {
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            document.getElementById("pdfviewdd").innerHTML = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var htmlElements = '<embed id="pdfview" src="' + $scope.content + '"  style="width: 100%;" height="1000" type="application/pdf" />';
                    document.getElementById("pdfviewdd").innerHTML = htmlElements;
                    $('#showpdf').modal('show');
                });
        };

        $scope.downloaddirectimage = function (data, idd) {
            var studentreg = idd;
            $scope.imagedownload = data;
            imagedownload = data;
            $http.get(imagedownload, { responseType: "arraybuffer" }).success(function (data) {
                var anchor = angular.element('<a/>');
                var blob = new Blob([data]);
                anchor.attr({
                    href: window.URL.createObjectURL(blob),
                    target: '_blank',
                    download: studentreg
                })[0].click();
            });
        };

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalPreview').modal('show');
        };

        $scope.uploadtecherdocuments1 = [];

        $scope.uploadtecherdocuments = function (input, document) {

            $scope.uploadtecherdocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/jpg" || input.files[0].type === "image/png") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-powerpoint") {
                    UploaddianPhoto(document);
                }
                else {
                    swal("Upload MP4, Pdf, Doc, Image Files Only");
                }
            }
        };

        function UploaddianPhoto(data) {
            console.log("Teacher Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadtecherdocuments1.length; i++) {
                formData.append("File", $scope.uploadtecherdocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/DRReportCheckList", formData,
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': undefined
                    },
                    transformRequest: angular.identity,

                })
                .success(function (d) {
                    defer.resolve(d);
                    data.filepath = d;
                    data.filename = $scope.filename;
                    $('#').attr('src', data.filepath);
                    var img = data.filepath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.filepath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.addNewsiblingguard = function () {
            var newItemNo = $scope.tempcategorychecklistfiles.length + 1;

            angular.forEach($scope.tempcategorychecklist, function (dd) {
                if (dd.ismmtcatcL_Id === parseInt($scope.ISMMTCATCL_Id)) {
                    $scope.CheckListName = dd.ismmtcatcL_CheckListName;
                    $scope.CheckListFlag = dd.ismmtcatcL_UploadEnterFlg;
                }
            });

            if (newItemNo <= 20) {
                $scope.tempcategorychecklistfiles.push({
                    'id': 'Teacher' + newItemNo, ismmtcatcL_Id: parseInt($scope.ISMMTCATCL_Id),
                    ismmtcatcL_CheckListName: $scope.CheckListName,
                    ismmtcatcL_UploadEnterFlg: $scope.CheckListFlag
                });
            }
            console.log($scope.tempcategorychecklistfiles);
        };

        $scope.removeNewsiblingguard = function (index) {
            var newItemNo = $scope.tempcategorychecklistfiles.length - 1;
            $scope.tempcategorychecklistfiles.splice(index, 1);

            if ($scope.tempcategorychecklistfiles.length === 0) {
                //data
            }
        };

        $scope.onCheckListChange = function (ISMMTCATCL_Id) {
            $scope.tempcategorychecklistfiles = [];
            
            angular.forEach($scope.tempcategorychecklist, function (dd) {
                if (dd.ismmtcatcL_Id === parseInt($scope.ISMMTCATCL_Id)) {
                    $scope.CheckListName = dd.ismmtcatcL_CheckListName;
                    $scope.CheckListFlag = dd.ismmtcatcL_UploadEnterFlg;
                }
            });

            angular.forEach($scope.get_plannerlist, function (ddd, index1) {
                //ddd.ISMTPLTA_Id === $scope.checklist_taskplannerid &&
                if ($scope.checklist_taskplannerid !== undefined && $scope.checklist_taskplannerid !== null && $scope.checklist_taskplannerid > 0) {
                    if (ddd.ISMTPLTA_Id === $scope.checklist_taskplannerid && ddd.ISMTCR_Id === $scope.checklist_taskid && ddd.ISMMTCAT_Id === $scope.checklist_categoryid) {
                        angular.forEach(ddd.categorychecklist, function (dd) {
                            if (parseInt(dd.ISMMTCATCL_Id) === parseInt(ISMMTCATCL_Id)) {
                                if (dd.filepath !== undefined && dd.filepath !== null && dd.filepath !== "") {
                                    var img = dd.filepath;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];
                                    dd.filetype = lastelement;
                                    console.log("dd.filetype : " + dd.filetype);
                                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                        dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
                                    }
                                }

                                $scope.tempcategorychecklistfiles.push({
                                    ismmtcatcL_CheckListName: dd.checklistname, checklistremarks: dd.comments,
                                    refno: dd.refno, filepath: dd.filepath, filetype: dd.filetype, filename: dd.filename, ismmtcatcL_Id: dd.ISMMTCATCL_Id,
                                    ismmtcatcL_UploadEnterFlg: $scope.CheckListFlag
                                });
                            }
                        });
                    }
                } else {
                    var taskplannerid = ddd.ISMTPLTA_Id === undefined || ddd.ISMTPLTA_Id === null || ddd.ISMTPLTA_Id === 0 ? "A" : "B";
                    if (ddd.ISMTCR_Id === $scope.checklist_taskid && ddd.ISMMTCAT_Id === $scope.checklist_categoryid && taskplannerid==="A") {
                        angular.forEach(ddd.categorychecklist, function (dd) {
                            if (parseInt(dd.ISMMTCATCL_Id) === parseInt(ISMMTCATCL_Id)) {
                                if (dd.filepath !== undefined && dd.filepath !== null && dd.filepath !== "") {
                                    var img = dd.filepath;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];
                                    dd.filetype = lastelement;
                                    console.log("dd.filetype : " + dd.filetype);
                                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                        dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
                                    }
                                }

                                $scope.tempcategorychecklistfiles.push({
                                    ismmtcatcL_CheckListName: dd.checklistname, checklistremarks: dd.comments,
                                    refno: dd.refno, filepath: dd.filepath, filetype: dd.filetype, filename: dd.filename, ismmtcatcL_Id: dd.ISMMTCATCL_Id,
                                    ismmtcatcL_UploadEnterFlg: $scope.CheckListFlag
                                });
                            }
                        });
                    }
                }               
            });

            if ($scope.tempcategorychecklistfiles.length === 0) {
                $scope.tempcategorychecklistfiles.push({
                    'id': 'Teacher', ismmtcatcL_Id: parseInt($scope.ISMMTCATCL_Id),
                    ismmtcatcL_CheckListName: $scope.CheckListName,
                    ismmtcatcL_UploadEnterFlg: $scope.CheckListFlag
                });
            }
        };

        $scope.AddFiles = function (tempcategorychecklistfiles) {
            if ($scope.myForm6.$valid) {
                angular.forEach($scope.savedfiles, function (dd, i) {
                    if (dd.ismmtcatcL_Id === parseInt($scope.ISMMTCATCL_Id)) {
                        $scope.savedfiles.splice(i, 1);
                    }
                });

                angular.forEach(tempcategorychecklistfiles, function (dd) {
                    var filepathnew = dd.filepath !== undefined && dd.filepath !== null && dd.filepath !== "" ? dd.filepath : "";
                    var filenamenew = dd.filename !== undefined && dd.filename !== null && dd.filename !== "" ? dd.filename : "";
                    var document_Pathnew = dd.document_Pathnew !== undefined && dd.document_Pathnew !== null && dd.document_Pathnew !== "" ? dd.document_Pathnew : "";
                    var refnonew = dd.refno !== undefined && dd.refno !== null && dd.refno !== "" ? dd.refno : "";
                    var filetypenew = dd.filetypenew !== undefined && dd.filetypenew !== null && dd.filetypenew !== "" ? dd.filetypenew : "";
                    var checklistremarksnew = dd.checklistremarks !== undefined && dd.checklistremarks !== null && dd.checklistremarks !== "" ? dd.checklistremarks : "";

                    $scope.savedfiles.push({
                        filepathnew: filepathnew, filenamenew: filenamenew, document_Pathnew1: document_Pathnew,
                        remrks: checklistremarksnew, refno: refnonew, filetypenew: filetypenew, ismmtcatcL_Id: dd.ismmtcatcL_Id,
                        ismmtcatcL_CheckListName: dd.ismmtcatcL_CheckListName
                    });
                });

                $scope.ISMMTCATCL_Id = "";
                $scope.tempcategorychecklistfiles = [];
                $scope.submitted6 = false;
                console.log($scope.savedfiles);
            } else {
                $scope.submitted6 = true;
            }
        };

        // ****** Saving Checklist Files Or  Remarks  ****** //
        $scope.SaveCheckListDeails = function (dd) {
            $scope.submitted6 = false;

            $scope.savedchecklist = [];

            angular.forEach(dd, function (d) {
                $scope.uploadfilepath = d.filepathnew !== undefined && d.filepathnew !== null ? d.filepathnew : "";
                $scope.uploadfilename = d.filenamenew !== undefined && d.filenamenew !== null ? d.filenamenew : "";
                $scope.checklistrmks = d.remrks !== undefined && d.remrks !== null ? d.remrks : "";
                $scope.checklistrefno = d.refno !== undefined && d.refno !== null ? d.refno : "";

                $scope.savedchecklist.push({
                    checklistname: d.ismmtcatcL_CheckListName, filepath: $scope.uploadfilepath, filename: $scope.uploadfilename,
                    comments: $scope.checklistrmks, refno: $scope.checklistrefno, ISMMTCATCL_Id: d.ismmtcatcL_Id
                });
            });

            angular.forEach($scope.get_plannerlist, function (ddd, index1) {
                if ($scope.checklist_taskplannerid === undefined || $scope.checklist_taskplannerid === null || $scope.checklist_taskplannerid === 0) {
                    if (ddd.ISMTCR_Id === $scope.checklist_taskid && ddd.ISMMTCAT_Id === $scope.checklist_categoryid) {
                        ddd.categorychecklist = $scope.savedchecklist;
                    }
                } else {
                    if (ddd.ISMTCR_Id === $scope.checklist_taskid && ddd.ISMMTCAT_Id === $scope.checklist_categoryid
                        && ddd.ISMTPLTA_Id === $scope.checklist_taskplannerid) {
                        ddd.categorychecklist = $scope.savedchecklist;
                    }
                }              
            });

            console.log($scope.get_plannerlist);
            $('#categorychecklist').modal('hide');

        };

        // ****** View Added Checklist Details ***** //
        $scope.ViewAddChecklistDetails = function (ddd) {


            $scope.addedchecklist = ddd.categorychecklist;

            if ($scope.addedchecklist !== null && $scope.addedchecklist.length > 0) {
                angular.forEach($scope.addedchecklist, function (dd) {
                    if (dd.filepath !== null && dd.filepath !== "") {
                        var img = dd.filepath;
                        var imagarr = img.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        dd.filetype = lastelement;
                        console.log("dd.filetype : " + dd.filetype);
                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                            dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
                        }
                    }
                });

                $('#viewaddedchecklist').modal('show');
            }
        };

        // ****** View Check List Saved Details ****** //
        $scope.ViewCheckListSavedDetails = function (objs) {

            var data = {
                "ISMDRPT_Id": objs.ISMDRPT_Id
            };

            apiService.create("ISM_DailyReportGeneration/ViewCheckListSavedDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getchecklistarray = promise.getchecklistarray;
                    if ($scope.getchecklistarray === null || $scope.getchecklistarray.length === 0) {
                        swal("No Records Found");
                    } else {
                        angular.forEach($scope.getchecklistarray, function (dd) {
                            if (dd.ismdrptfL_FilePath !== null && dd.ismdrptfL_FilePath !== "") {
                                var img = dd.ismdrptfL_FilePath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("dd.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.ismdrptfL_FilePath;
                                }
                            }
                        });
                        $('#viewfiles').modal('show');
                    }
                }
            });
        };

        $scope.CloseCategoryCheckList = function () {

            angular.forEach($scope.get_plannerlist, function (ddd, index1) {
                //ddd.ISMTPLTA_Id === $scope.checklist_taskplannerid
                if (ddd.ISMTCR_Id === $scope.checklist_taskid && ddd.ISMMTCAT_Id === $scope.checklist_categoryid) {
                    if (ddd.categorychecklist !== undefined && ddd.categorychecklist !== null && ddd.categorychecklist.length > 0) {
                        ddd.checkedvalue = true;
                    } else {
                        ddd.checkedvalue = false;
                    }
                }
            });
        };

        $scope.ViewDRNotApprovedList = function () {
            $scope.drnotapprovedmessageindividual = [];
            $scope.TempEmployee = [];
            var data = {
                "ISMDRPT_Date": new Date($scope.ISMDRPT_Date).toDateString()
            };

            apiService.create("ISM_DailyReportGeneration/ViewDRNotApprovedList", data).then(function (promise) {

                if (promise.drnotapprovedmessageindividual !== undefined && promise.drnotapprovedmessageindividual !== null
                    && promise.drnotapprovedmessageindividual.length > 0) {

                    $scope.drnotapprovedmessageindividual = promise.drnotapprovedmessageindividual;
                    $scope.TempEmployee = [];
                    angular.forEach($scope.drnotapprovedmessageindividual, function (dd) {
                        if ($scope.TempEmployee.length === 0) {
                            $scope.TempEmployee.push(dd);
                        } else if ($scope.TempEmployee.length > 0) {
                            var countemp = 0;
                            angular.forEach($scope.TempEmployee, function (d) {
                                if (d.HRME_Id === dd.HRME_Id) {
                                    countemp += 1;
                                }
                            });
                            if (countemp === 0) {
                                $scope.TempEmployee.push(dd);
                            }
                        }
                    });

                    angular.forEach($scope.TempEmployee, function (dd) {
                        $scope.TempDates_Details = [];
                        angular.forEach($scope.drnotapprovedmessageindividual, function (d) {
                            if (dd.HRME_Id === d.HRME_Id) {
                                $scope.TempDates_Details.push(d);
                            }
                        });
                        dd.DatesDetails = $scope.TempDates_Details;
                    });

                    $('#viewdrnotapproveddetails').modal('show');
                } else {
                    swal("No Records Found");
                }
            });
        };

    }

    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });

    angular.module('app').directive('txtArea', function () {
        return {
            restrict: 'AE',
            replace: 'true',
            scope: { data: '=', model: '=ngModel' },
            template: "<textarea></textarea>",
            link: function (scope, elem) {
                scope.$watch('data', function (newVal) {
                    if (newVal) {
                        scope.model += newVal[0];
                    }
                });
            }
        };
    });

})();

