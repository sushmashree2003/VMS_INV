(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_PlannerApprovalController', ISM_PlannerApprovalController);
    ISM_PlannerApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$window', 'intracontroller'];
    function ISM_PlannerApprovalController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter, $window, intracontroller) {

        var HostName = location.host;
        $scope.showview = true;
        $scope.disablesave = false;
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        $scope.obj = {};
        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.multi_mi = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();
        $scope.currentdate = new Date();
        $scope.get_plannerlist = [];
        $scope.showapproval = false;
        $scope.loaddata = function () {
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            $scope.currentPage2 = 1;
            $scope.itemsPerPage2 = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.currentPageapp = 1;
            $scope.itemsPerPageapp = 10;
            $scope.page = "page";
            $scope.page1 = "page1";
            $scope.page2 = "page2";
            $scope.pageapp = "pageapp";
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("ISM_PlannerApproval/getdetails", pageid).then(function (promise) {

                $scope.get_updatedplannerlist = promise.get_updatedplannerlist; 
                angular.forEach($scope.get_updatedplannerlist, function (plm) {
                    var decimaltimeapp = plm.ISMTPL_TotalHrs;
                    var hrsapp = parseInt(Number(decimaltimeapp));
                    var minapp = Math.round((Number(decimaltimeapp) - hrsapp) * 60);
                    if (hrsapp.toString().length === 1) {
                        hrsapp = '0' + hrsapp;
                    }
                    if (minapp.toString().length === 1) {
                        minapp = '0' + minapp;
                    }
                    plm.effhrsapp = hrsapp;
                    plm.effminapp = minapp;
                });

                if (promise.get_leaveDetails != null && promise.get_leaveDetails.length > 0) {
                    $scope.get_leaveDetails = promise.get_leaveDetails;
                    $('#LeaveApproval').modal('show');
                }
                else {
                    $('#LeaveApproval').modal('hide');
                }


                if (promise.cerfificatelist != null && promise.cerfificatelist.length > 0) {
                    $scope.Cerfificatelist = promise.cerfificatelist;
                    $('#certificateApproval').modal('show');
                }
                else {
                    $('#certificateApproval').modal('hide');
                }

                $scope.monthlist = promise.monthlist;
               
                $scope.plannerlist = promise.get_plannerlist;
                $scope.get_plannermax = promise.get_plannermax;
                $scope.drnotapprovedmessage = promise.drnotapprovedmessage;
                angular.forEach($scope.plannerlist, function (pl) {
                    angular.forEach($scope.get_plannermax, function (plm) {
                        if (pl.ISMTPL_Id === plm.ISMTPL_Id) {
                            var decimaltime = pl.ISMTPL_TotalHrs;
                            var hrs = parseInt(Number(decimaltime));
                            var min = Math.round((Number(decimaltime) - hrs) * 60);
                            if (hrs.toString().length === 1) {
                                hrs = '0' + hrs;
                            }
                            if (min.toString().length === 1) {
                                min = '0' + min;
                            }
                            pl.effhrs = hrs;
                            pl.effmin = min;

                            $scope.get_plannerlist.push(pl);
                        }
                    });
                });

                if ($scope.drnotapprovedmessage != null && $scope.drnotapprovedmessage.length > 0) {
                    angular.forEach($scope.get_plannerlist, function (drapp) {
                        var tempdata = [];
                        tempdata = $filter('filter')($scope.drnotapprovedmessage, function (d) {
                            return d.HRME_Id == drapp.HRME_Id;
                        });
                        drapp.DrappDatecount = tempdata.length;
                    });
                }

                if (promise.completedcreatedbyme != null && promise.completedcreatedbyme.length > 0) {
                    $scope.compbyme = promise.completedcreatedbyme[0].ISMEDWTCC_TaskCount;
                    if ($scope.compbyme > 0) {
                        swal({
                            title: $scope.compbyme + " Task are pending to validate status which are completed & are created by you, without validating those you cant approve planner!!",
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
                                //else {
                                //    //$state.go('app.homepage');
                                //    swal("Task Update Cancelled!!!");
                                //}
                            });
                    }                   
                }
            });
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;
                $scope.reverse = !$scope.reverse;
            };
        };

        $scope.RedirectToMakerChecker = function () {
            //window.location.href = "http://localhost:57606/#/app/ISM_DailyReport_Approval/884";
            $window.open("http://localhost:57606/#/app/ISM_DailyReport_Approval/884");
            //$('.modal-backdrop').remove();
        };
        $scope.RedirectTotasklist = function () {
            intracontroller.viewcompleted = true;
            $window.location.href = 'http://' + HostName + '/#/app/ISM_ComplaintsTaskList/12';
        };

        $scope.onchange = function (view) {

            angular.forEach($scope.get_plannerlist, function (plan) {
                if (plan.ISMTPL_Id === $scope.plannerid) {
                    $scope.getplannerdetails(plan, view);
                };
            });

            if (view == 1) {
                $scope.showview = true;
                $scope.viewtemp = 1;
            }
            else {
                $scope.showview = false;
                $scope.viewtemp = 2;
            }
        };
        //******************************* Check Selection ****************************//
        $scope.all_check = function (ck) {
            $scope.checkall = ck;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.get_userEmplist, function (st) {
                st.empck = toggleStatus;
            });
        };
        $scope.togchkbx = function () {
            $scope.checkall = $scope.get_userEmplist.every(function (options) {
                return options.empck;
            });
        };
        $scope.isOptionsRequiredE = function () {
            return !$scope.get_userEmplist.some(function (options) {
                return options.empck;
            });
        };
        //******************************* Get Planner List ****************************//
        $scope.onView = function () {
            $scope.checkallemp = false;
            if ($scope.myForm.$valid) {
                $scope.emparray = [];
                $scope.get_plannerlist = [];
                angular.forEach($scope.get_userEmplist, function (em) {
                    if (em.empck === true) {
                        $scope.emparray.push(em);
                    }
                });
                var data = {
                    //  "IVRM_Month_Id": $scope.ivrM_Month_Id,
                    "emparray": $scope.emparray
                };
                apiService.create("ISM_PlannerApproval/getplannerlist", data).then(function (promise) {
                    if (promise.get_plannerlist.length > 0 || promise.get_updatedplannerlist.length > 0) {
                        $scope.plannerlist = promise.get_plannerlist;
                        $scope.get_plannermax = promise.get_plannermax;
                        $scope.get_updatedplannerlist = promise.get_updatedplannerlist;

                        angular.forEach($scope.plannerlist, function (pl) {
                            angular.forEach($scope.get_plannermax, function (plm) {
                                if (pl.ISMTPL_Id === plm.ISMTPL_Id) {
                                    $scope.get_plannerlist.push(pl);
                                }
                            });
                        });
                    }
                    else {
                        swal("No Record  Found..... !!");
                        $scope.get_plannerlist = "";
                        $scope.get_plannerdetails = "";
                        $scope.get_updatedplannerlist = "";
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.togglecheckboxAll = function (checkallemp) {
            var toggleStatus = checkallemp;
            angular.forEach($scope.get_plannerlist, function (slc) {
                slc.checked = toggleStatus;
            });
        };
        $scope.optionToggled = function () {
            $scope.checkallemp = $scope.get_plannerlist.every(function (options) {
                return options.checked;
            });
        };

        $scope.editplannerdetails = function (objedit) {
            $scope.editflg = 1;
            objedit.editflg = $scope.editflg;
            $scope.getplannerdetails(objedit);
        };

        
        $scope.getplannerdetailsviewcat = function (objp) {
            var plannerid = 0;
            $scope.alltask = "";
            $scope.nmineffort = 0;
            $scope.totminhr = 0;
            $scope.totminmin = 0;
            var nmineffort = 0;
            var ntwdays = 0;
            $scope.get_plannerdetails = [];

            if (objp.editflg === 1) {
                angular.forEach($scope.get_updatedplannerlist, function (objpl) {
                    if (objp.ISMTPL_Id === objpl.ISMTPL_Id) {
                        plannerid = objpl.ISMTPL_Id;
                    }
                });
            }
            else {
                $scope.editflg = 0;
                angular.forEach($scope.get_plannerlist, function (objpl) {
                    if (objp.ISMTPL_Id === objpl.ISMTPL_Id) {
                        plannerid = objpl.ISMTPL_Id;
                    }
                });
            }
            $scope.plannerid = plannerid;
            var data = {
                "ISMTPL_Id": $scope.plannerid,
                "HRMD_Id": $scope.hrmD_Id,
                "miarray": $scope.miarray
            };
            apiService.create("ISM_PlannerApproval/getplannerdetails", data).then(function (promise) {
                $scope.plannercategory = promise.get_plannercategory;

                $scope.nget_effortdetails = promise.get_effortdetails;              
                if ($scope.nget_effortdetails!=null && $scope.nget_effortdetails.length > 0) {
                    angular.forEach($scope.nget_effortdetails, function (tf) {
                        nmineffort += parseInt(tf.WORKINGHOURS);
                        ntwdays += parseInt(tf.NOOFDAYS);
                    });
                }
                $scope.nmineffort = nmineffort;
                var ndecimaltimemin = $scope.nmineffort;
                var nhrsmin = parseInt(Number(ndecimaltimemin));
                var nminmin = Math.round((Number(ndecimaltimemin) - nhrsmin) * 60);
                if (nhrsmin.toString().length === 1) {
                    nhrsmin = '0' + nhrsmin;
                }
                if (nminmin.toString().length === 1) {
                    nminmin = '0' + nminmin;
                }
                $scope.totminhr = nhrsmin;
                $scope.totminmin = nminmin;

                angular.forEach($scope.plannercategory, function (tt) {
                    tt.total = ((parseFloat(tt.ISMMTCAT_TaskPercentage) / 100) * parseFloat(48));
                    var decimaltimecat = tt.total;
                    var hrscat = parseInt(Number(decimaltimecat));
                    var mincat = Math.round((Number(decimaltimecat) - hrscat) * 60);
                    if (hrscat.toString().length === 1) {
                        hrscat = '0' + hrscat;
                    }
                    if (mincat.toString().length === 1) {
                        mincat = '0' + mincat;
                    }
                    tt.hrscat = hrscat;
                    tt.mincat = mincat;
                });
            });
        };

        $scope.getplannerdetailsviewcatnew = function (objp) { 
            $scope.nmineffort = 0;
            $scope.totminhr = 0;
            $scope.totminmin = 0;
            var nmineffort = 0;
            var ntwdays = 0;
            var data = {
                "ISMTPL_Id": $scope.plannerid,
                "HRMD_Id": $scope.hrmD_Id,
                "miarray": $scope.miarray
            };
            apiService.create("ISM_PlannerApproval/getplannerdetails", data).then(function (promise) {
                $scope.plannercategory = promise.get_plannercategory;

                $scope.nget_effortdetails = promise.get_effortdetails;
                if ($scope.nget_effortdetails!=null && $scope.nget_effortdetails.length > 0) {
                    angular.forEach($scope.nget_effortdetails, function (tf) {
                        nmineffort += parseInt(tf.WORKINGHOURS);
                        ntwdays += parseInt(tf.NOOFDAYS);
                    });
                }
                $scope.nmineffort = nmineffort;
                var ndecimaltimemin = $scope.nmineffort;
                var nhrsmin = parseInt(Number(ndecimaltimemin));
                var nminmin = Math.round((Number(ndecimaltimemin) - nhrsmin) * 60);
                if (nhrsmin.toString().length === 1) {
                    nhrsmin = '0' + nhrsmin;
                }
                if (nminmin.toString().length === 1) {
                    nminmin = '0' + nminmin;
                }
                $scope.totminhr = nhrsmin;
                $scope.totminmin = nminmin;

                angular.forEach($scope.plannercategory, function (tt) {
                    tt.total = ((parseFloat(tt.ISMMTCAT_TaskPercentage) / 100) * parseFloat(48));
                    var decimaltimecat = tt.total;
                    var hrscat = parseInt(Number(decimaltimecat));
                    var mincat = Math.round((Number(decimaltimecat) - hrscat) * 60);
                    if (hrscat.toString().length === 1) {
                        hrscat = '0' + hrscat;
                    }
                    if (mincat.toString().length === 1) {
                        mincat = '0' + mincat;
                    }
                    tt.hrscat = hrscat;
                    tt.mincat = mincat;
                });
            });
        };

        //*************************************** Planner Details ***********************************//
        $scope.getplannerdetails = function (objp, view) {
            $('.modal-backdrop').remove();
            $('.modal').on('hide.bs.modal', function (e) {
                e.stopPropagation();
                $('body').css('padding-right', '');
            });
            $('#dailyreportnotapp').modal('hide');
            if (objp.DrappDatecount == 0) {
                var plannerid = 0;
                $scope.nmineffort = 0;
                $scope.totminhr = 0;
                $scope.totminmin = 0;
                var nmineffort = 0;
                var ntwdays = 0;
                $scope.disablesave = true;
                if (view == undefined || view < 0) {
                    $scope.showapproval = false;
                    document.getElementById("hideshowbutton").click();
                    document.getElementById("hideshowbutton2").click();
                }
                $scope.alltask = "";
                $scope.get_plannerdetailsapp = [];
                $scope.totaleffort = 0;
                $scope.mineffort = 0;

                $scope.statusall = 1;
                $scope.totalcheck = 0;
                $scope.mineffort = 0;
                $scope.currentweekeffort = 0;
                $scope.deviatedeffort = 0;
                $scope.effhrsdev = 0;
                $scope.mindev = 0;
                $scope.sdateview = objp.ISMTPL_StartDate;
                $scope.edateview = objp.ISMTPL_EndDate;
                if (objp.editflg === 1) {
                    angular.forEach($scope.get_updatedplannerlist, function (objpl) {
                        if (objp.ISMTPL_Id === objpl.ISMTPL_Id) {
                            plannerid = objpl.ISMTPL_Id;
                        }
                    });
                }
                else {
                    $scope.editflg = 0;
                    angular.forEach($scope.get_plannerlist, function (objpl) {
                        if (objp.ISMTPL_Id === objpl.ISMTPL_Id) {
                            plannerid = objpl.ISMTPL_Id;
                        }
                    });
                }
                var nmineffort = 0;
                var ntwdays = 0;
                $scope.nmineffort = 0;
                $scope.totminhr = 0;
                $scope.totminmin = 0;
                $scope.getplannerdetailsviewNewcat(objp);
                $scope.plannerid = plannerid;

                if (view == 2) {
                    $scope.viewtemp = 2;
                } else {
                    $scope.viewtemp = 1;
                    $scope.view = "1";
                }
                var data = {
                    "ISMTPL_Id": $scope.plannerid,
                    "HRMD_Id": $scope.hrmD_Id,
                    "miarray": $scope.miarray,
                    "view": $scope.viewtemp,
                    "HRME_Id": objp.HRME_Id
                };
                apiService.create("ISM_PlannerApproval/getplannerdetails", data).then(function (promise) {
                    if (promise.get_plannerdetails.length > 0 || promise.get_plannerApprovaldetails.length > 0) {
                        //document.getElementById("hideshowbutton").click();
                        //document.getElementById("hideshowbutton2").click();
                        $scope.showapproval = true;
                        $scope.plannerdetailsapp = promise.get_plannerdetails;
                        $scope.get_plannerdetails_daily = promise.get_plannerdetails_daily;
                        $scope.plannercategory = promise.get_plannercategory;
                        $scope.plannerApprovaldetails = promise.get_plannerApprovaldetails;
                        $scope.totalplannertask = $scope.plannerdetailsapp.length;

                        $scope.nget_effortdetails = promise.get_effortdetails;
                        if ($scope.nget_effortdetails != null && $scope.nget_effortdetails.length > 0) {
                            angular.forEach($scope.nget_effortdetails, function (tf) {
                                nmineffort += parseInt(tf.WORKINGHOURS);
                                ntwdays += parseInt(tf.NOOFDAYS);
                            });
                        }
                        $scope.nmineffort = nmineffort;
                        var ndecimaltimemin = $scope.nmineffort;
                        var nhrsmin = parseInt(Number(ndecimaltimemin));
                        var nminmin = Math.round((Number(ndecimaltimemin) - nhrsmin) * 60);
                        if (nhrsmin.toString().length === 1) {
                            nhrsmin = '0' + nhrsmin;
                        }
                        if (nminmin.toString().length === 1) {
                            nminmin = '0' + nminmin;
                        }
                        $scope.totminhr = nhrsmin;
                        $scope.totminmin = nminmin;

                        $scope.ismtplaP_Id = $scope.plannerdetailsapp[0].ISMTPLAP_Id;
                        angular.forEach($scope.plannerdetailsapp, function (pl) {
                            if (pl.ISMTPLTA_PreviousTask === 1 || pl.ISMTPLTA_PreviousTask === "1" || pl.ISMTPLTA_PreviousTask === true) {
                                //pl.plannerStatus = 1;
                                pl.cls = "backkk";
                            }
                            var decimaltime = pl.ISMTPLTA_EffortInHrs;
                            var hrs = parseInt(Number(decimaltime));
                            var min = Math.round((Number(decimaltime) - hrs) * 60);
                            //if (hrs.toString().length === 1) {
                            //    hrs = '0' + hrs;
                            //}
                            //if (min.toString().length === 1) {
                            //    min = '0' + min;
                            //}
                            var effhrs = hrs;
                            var effmin = min;

                            var eachdecimaltime = pl.eachtaskeff;
                            var eachhrs = parseInt(Number(eachdecimaltime));
                            var eachmin = Math.round((Number(eachdecimaltime) - eachhrs) * 60);
                            if (eachhrs.toString().length === 1) {
                                eachhrs = '0' + eachhrs;
                            }
                            if (eachmin.toString().length === 1) {
                                eachmin = '0' + eachmin;
                            }
                            var eachhrs = eachhrs;
                            var eachmin = eachmin;

                            if (pl.ISMTPLTA_StartDate == null || pl.ISMTPLTA_StartDate == "") {
                                if ($scope.get_plannerdetails_daily != null && $scope.get_plannerdetails_daily.length > 0) {
                                    angular.forEach($scope.plannerdetailsapp, function (pln) {
                                        angular.forEach($scope.get_plannerdetails_daily, function (dat) {
                                            if (pln.ISMTCR_Id === dat.ISMTCR_Id) {
                                                pln.ISMTPLTA_StartDate = dat.ISMTPLTA_StartDate;
                                                pln.ISMTPLTA_EndDate = dat.ISMTPLTA_EndDate;
                                            };
                                        });
                                    });
                                }
                            };


                            pl.plannerStatus = 1;
                            $scope.get_plannerdetailsapp.push({
                                ISMTPLAPTA_Id: pl.ISMTPLAPTA_Id, ISMTPL_Id: pl.ISMTPL_Id, ISMTCR_Id: pl.ISMTCR_Id, HRMP_Name: pl.HRMP_Name, ISMTCR_BugOREnhancement: pl.ISMTCR_BugOREnhancement, ISMTCR_Title: pl.ISMTCR_Title, ISMTCR_Status: pl.ISMTCR_Status, ISMTCR_TaskNo: pl.ISMTCR_TaskNo, ISMMCLT_ClientName: pl.ISMMCLT_ClientName, ISMTPL_CreatedBy: pl.ISMTPL_CreatedBy, plannedby: pl.plannedby, ISMTPL_PlannerName: pl.ISMTPL_PlannerName, ISMTPLTA_Remarks: pl.ISMTPLTA_Remarks, plannerStatus: pl.plannerStatus, ISMTPLTA_Status: pl.ISMTPLTA_Status, ISMTPLTA_StartDate: new Date(pl.ISMTPLTA_StartDate), ISMTPLTA_EndDate: new Date(pl.ISMTPLTA_EndDate), plannerDate: new Date(pl.plannerDate), ISMTPLTA_EffortInHrs: pl.ISMTPLTA_EffortInHrs, ISMTPLTA_PreviousTask: pl.ISMTPLTA_PreviousTask, ISMTPLTA_Id: pl.ISMTPLTA_Id, CreatedDate: pl.CreatedDate, ISMMTCAT_TaskCategoryName: pl.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: pl.ISMMTCAT_CompulsoryFlg, createdby: pl.createdby, cls: pl.cls, Periodicity: pl.Periodicity, effhrs: effhrs, effmin: effmin, PTSCount: pl.PTSCount, eachhrs: eachhrs, eachmin: eachmin
                            });
                            $scope.plannerDate = pl.plannerDate;
                            $scope.plannedby = pl.plannedby;
                            $scope.plannername = pl.ISMTPL_PlannerName;
                            $scope.mineffort = pl.ISMTPL_TotalHrs;

                            var decimaltimeMin = pl.ISMTPL_TotalHrs;
                            var hrsmin = parseInt(Number(decimaltimeMin));
                            var minmin = Math.round((Number(decimaltimeMin) - hrsmin) * 60);
                            if (hrsmin.toString().length === 1) {
                                hrsmin = '0' + hrsmin;
                            }
                            if (minmin.toString().length === 1) {
                                minmin = '0' + minmin;
                            }
                            $scope.effhrsmin = hrsmin;
                            $scope.minminmin = minmin;
                        });
                        if ($scope.get_plannerdetailsapp.length > 0) {
                            angular.forEach($scope.get_plannerdetailsapp, function (ww) {
                                if (ww.ISMTPLTA_PreviousTask === false) {
                                    $scope.currentweekeffort = parseFloat($scope.currentweekeffort) + parseFloat(ww.ISMTPLTA_EffortInHrs);
                                    var decimaltimecur = $scope.currentweekeffort;
                                    var hrscur = parseInt(Number(decimaltimecur));
                                    var mincur = Math.round((Number(decimaltimecur) - hrscur) * 60);
                                    if (hrscur.toString().length === 1) {
                                        hrscur = '0' + hrscur;
                                    }
                                    if (mincur.toString().length === 1) {
                                        mincur = '0' + mincur;
                                    }

                                    $scope.effhrscur = hrscur;
                                    $scope.mincur = mincur;
                                }
                                else if (ww.ISMTPLTA_PreviousTask === true) {
                                    $scope.deviatedeffort = parseFloat($scope.deviatedeffort) + parseFloat(ww.ISMTPLTA_EffortInHrs);

                                    var decimaltimedev = $scope.deviatedeffort;
                                    var hrsdev = parseInt(Number(decimaltimedev));
                                    var mindev = Math.round((Number(decimaltimedev) - hrsdev) * 60);
                                    if (hrsdev.toString().length === 1) {
                                        hrsdev = '0' + hrsdev;
                                    }
                                    if (mindev.toString().length === 1) {
                                        mindev = '0' + mindev;
                                    }
                                    $scope.effhrsdev = hrsdev;
                                    $scope.mindev = mindev;
                                };
                            });
                        }

                    }
                    else {
                        swal("No Record  Found..... !!");
                        $scope.showapproval = false;
                        $scope.get_plannerdetailsapp = "";
                    }
                });
            }
            else {
                $scope.showapproval = false;
                $scope.get_plannerdetailsapp = "";
                $scope.drnotapprovedlist = [];
                $scope.drappname = "";
               var tempdatamew = $filter('filter')($scope.drnotapprovedmessage, function (d) {
                   return d.HRME_Id == objp.HRME_Id;
                });
                $scope.drnotapprovedlist = tempdatamew;
                $scope.drappname = objp.plannedby;
                $('.modal-backdrop').remove();
                $('.modal').on('hide.bs.modal', function (e) {
                    e.stopPropagation();
                    $('body').css('padding-right', '');
                });
                $('#dailyreportnotapp').modal('show');
            }
           
        };

        $scope.getplannerdetailsview = function (objp) {
            $scope.get_plannerdetailsview = [];
            $scope.nmineffort = 0;
            $scope.totminhr = 0;
            $scope.totminmin = 0;
            var nmineffort = 0;
            var ntwdays = 0;
            var data = {
                "ISMTPL_Id": objp.ISMTPL_Id
            };
            apiService.create("ISM_PlannerCreation/getplannerdetails", data).then(function (promise) {
                if (promise.get_plannerdetails.length > 0) {
                    $scope.plannerdetailsview = promise.get_plannerdetails;
                    $scope.totalplannertask = promise.get_plannerdetails.length;

                    $scope.nget_effortdetails = promise.get_effortdetails;
                    if ($scope.nget_effortdetails!=null && $scope.nget_effortdetails.length > 0) {
                        angular.forEach($scope.nget_effortdetails, function (tf) {
                            nmineffort += parseInt(tf.WORKINGHOURS);
                            ntwdays += parseInt(tf.NOOFDAYS);
                        });
                    }
                    $scope.nmineffort = nmineffort;
                    var ndecimaltimemin = $scope.nmineffort;
                    var nhrsmin = parseInt(Number(ndecimaltimemin));
                    var nminmin = Math.round((Number(ndecimaltimemin) - nhrsmin) * 60);
                    if (nhrsmin.toString().length === 1) {
                        nhrsmin = '0' + nhrsmin;
                    }
                    if (nminmin.toString().length === 1) {
                        nminmin = '0' + nminmin;
                    }
                    $scope.totminhr = nhrsmin;
                    $scope.totminmin = nminmin;


                    var decimaltime = pl.ISMTPLTA_EffortInHrs;
                    var hrs = parseInt(Number(decimaltime));
                    var min = Math.round((Number(decimaltime) - hrs) * 60);
                    if (hrs.toString().length === 1) {
                        hrs = '0' + hrs;
                    }
                    if (min.toString().length === 1) {
                        min = '0' + min;
                    }
                    pl.effhrs = hrs;
                    pl.effmin = min;

                    angular.forEach($scope.plannerdetailsview, function (pl) {
                        $scope.get_plannerdetailsview.push({
                            ISMTPL_Id: pl.ISMTPL_Id, ISMTCR_Id: pl.ISMTCR_Id, HRMP_Name: pl.HRMP_Name, ISMTCR_BugOREnhancement: pl.ISMTCR_BugOREnhancement, ISMTCR_Title: pl.ISMTCR_Title, ISMTCR_Status: pl.ISMTCR_Status, ISMTCR_TaskNo: pl.ISMTCR_TaskNo, ISMMCLT_ClientName: pl.ISMMCLT_ClientName, ISMTPL_CreatedBy: pl.ISMTPL_CreatedBy, plannedby: pl.plannedby, ISMTPL_PlannerName: pl.ISMTPL_PlannerName, ISMTPLTA_Remarks: pl.ISMTPLTA_Remarks, CreatedDate: pl.CreatedDate, ISMTCR_Desc: pl.ISMTCR_Desc, ISMTPLTA_Status: pl.ISMTPLTA_Status,
                            ISMTCR_CreationDate: pl.ISMTCR_CreationDate, ISMTPLTA_StartDate: new Date(pl.ISMTPLTA_StartDate), ISMTPLTA_EndDate: new Date(pl.ISMTPLTA_EndDate), ISMTPLTA_EffortInHrs: pl.ISMTPLTA_EffortInHrs, ISMTPLTA_PreviousTask: pl.ISMTPLTA_PreviousTask, ISMTPLTA_Id: pl.ISMTPLTA_Id, effhrs: effhrs, effmin: effmin
                        });
                        $scope.mineffort = pl.ISMTPL_TotalHrs;

                        var decimaltimeMin = pl.ISMTPL_TotalHrs;
                        var hrsmin = parseInt(Number(decimaltimeMin));
                        var minmin = Math.round((Number(decimaltimeMin) - hrsmin) * 60);
                        if (hrsmin.toString().length === 1) {
                            hrsmin = '0' + hrsmin;
                        }
                        if (minmin.toString().length === 1) {
                            minmin = '0' + minmin;
                        }
                        $scope.effhrsmin = hrsmin;
                        $scope.minminmin = minmin;

                    });
                }
                else {
                    swal("No Record  Found..... !!");
                    $scope.get_plannerdetailsview = "";
                }
            });
        };

        $scope.appRejChange = function () {
            angular.forEach($scope.get_plannerdetails, function (pl) {
                var finaleffort = 0;
                if (pl.plannerStatus === "0" || pl.plannerStatus === 0) {
                    angular.forEach($scope.get_plannerdetails, function (ack) {

                        finaleffort += parseFloat(parseInt(ack.effhrs, 10) + parseInt(ack.effmin, 10) / 60);
                    });
                    $scope.totaleffort = finaleffort;

                    var decimaltimetot = $scope.totaleffort;
                    var hrstot = parseInt(Number(decimaltimetot));
                    var mintot = Math.round((Number(decimaltimetot) - hrstot) * 60);
                    if (hrstot.toString().length === 1) {
                        hrstot = '0' + hrstot;
                    }
                    if (mintot.toString().length === 1) {
                        mintot = '0' + mintot;
                    }
                    if (mintot === 60) {
                        hrstot = hrstot + 1;
                        mintot = 0;
                    }
                    $scope.effhrstot = hrstot;
                    $scope.minmintot = mintot;
                }
                else {
                    angular.forEach($scope.get_plannerdetails, function (ack) {
                        finaleffort += parseFloat(parseInt(ack.effhrs, 10) + parseInt(ack.effmin, 10) / 60);
                    });
                    $scope.totaleffort = finaleffort;

                    var decimaltimetotf = $scope.totaleffort;
                    var hrstotf = parseInt(Number(decimaltimetotf));
                    var mintotf = Math.round((Number(decimaltimetotf) - hrstot) * 60);
                    if (hrstotf.toString().length === 1) {
                        hrstotf = '0' + hrstotf;
                    }
                    if (mintotf.toString().length === 1) {
                        mintotf = '0' + mintotf;
                    }

                    if (mintotf === 60) {
                        hrstotf = hrstotf + 1;
                        mintotf = 0;
                    }
                    $scope.effhrstot = hrstotf;
                    $scope.minmintot = mintotf;

                    pl.ISMTPLTA_EffortInHrs = parseFloat(parseInt(ack.effhrs, 10) + parseInt(ack.effmin, 10) / 60);
                }
            });
        };

        $scope.statusAll = function (allsts) {
            if (allsts === "1") {
                angular.forEach($scope.get_plannerdetailsapp, function (pck) {
                    //if (pck.ISMTPLTA_Status === 'Open') {
                    pck.plannerStatus = 1;
                    //}
                });
            }
            else {
                angular.forEach($scope.get_plannerdetailsapp, function (pck) {
                    //if (pck.ISMTPLTA_Status === 'Open') {
                    pck.plannerStatus = 0;
                    //}
                });
            }
            $scope.onassignCheck();
            angular.forEach($scope.get_comptasklist, function (atck) {
                if (atck.assignedflg === false) {
                    atck.checkedvalue = toggleStatus;
                }
            });
        };

        $scope.selectpartiularpage = function (alltaskc, pagev, itemsv) {
            $scope.selected = [];
            for (var i = ((pagev - 1) * itemsv); i < ((pagev) * itemsv); i++) {
                if ($scope.get_plannerdetailsapp.length > i) {
                    $scope.get_plannerdetailsapp[i].checkedPlan = true;
                }
            }
            $scope.onassignCheck();

        };
        $scope.deselectpartiularpage = function (alltaskc, pagev, itemsv) {
            $scope.selected = [];
            for (var i = ((pagev - 1) * itemsv); i < ((pagev) * itemsv); i++) {
                if ($scope.get_plannerdetailsapp.length > i) {
                    $scope.get_plannerdetailsapp[i].checkedPlan = false;
                }
            }
            $scope.onassignCheck();

        };

        $scope.all_task = function (atk) {
            var totcheck = 0;
            var toteffort = 0;
            $scope.alltask = atk;
            var toggleStatus = $scope.alltask;
            angular.forEach($scope.get_plannerdetailsapp, function (atck) {
                //if (atck.ISMTPLTA_Status === 'Open') {
                atck.checkedPlan = toggleStatus;
                //}
                if (atck.checkedPlan === true && (atck.plannerStatus === "1" || atck.plannerStatus === 1)) {
                    totcheck = totcheck + 1;
                    toteffort += parseFloat(parseInt(atck.effhrs, 10) + parseInt(atck.effmin, 10) / 60);
                }
                //totcheck = totcheck + 1;
                //toteffort += parseFloat(atck.ISMTPLTA_EffortInHrs);
            });
            $scope.totalcheck = totcheck;
            $scope.totaleffort = toteffort;

            var decimaltimetot = $scope.totaleffort;
            var hrstot = parseInt(Number(decimaltimetot));
            var mintot = Math.round((Number(decimaltimetot) - hrstot) * 60);
            if (hrstot.toString().length === 1) {
                hrstot = '0' + hrstot;
            }
            if (mintot.toString().length === 1) {
                mintot = '0' + mintot;
            }

            if (mintot === 60) {
                hrstot = hrstot + 1;
                mintot = 0;
            }

            $scope.effhrstot = hrstot;
            $scope.minmintot = mintot;
        };

        //========================================= View Mode
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

        $scope.onassignCheck = function () {
            var totcheck = 0;
            var toteffort = 0;
            angular.forEach($scope.get_plannerdetailsapp, function (ack) {
                if (ack.checkedPlan === true && (ack.plannerStatus === "1" || ack.plannerStatus === 1)) {
                    totcheck = totcheck + 1;
                    if (ack.effhrs === null || ack.effhrs === undefined) {
                        ack.effhrs = 0;
                    }
                    if (ack.effmin === null || ack.effmin === undefined) {
                        ack.effmin = 0;
                    }
                    toteffort += parseFloat(parseInt(ack.effhrs, 10) + parseInt(ack.effmin, 10) / 60);
                }
                else if (ack.checkedPlan === true) {
                    totcheck = totcheck + 1;
                }
            });
            $scope.totalcheck = totcheck;
            $scope.totaleffort = toteffort;
            var decimaltimetot = $scope.totaleffort;
            var hrstot = parseInt(Number(decimaltimetot));
            var mintot = Math.round((Number(decimaltimetot) - hrstot) * 60);
            if (hrstot.toString().length === 1) {
                hrstot = '0' + hrstot;
            }
            if (mintot.toString().length === 1) {
                mintot = '0' + mintot;
            }
            if (mintot === 60) {
                hrstot = hrstot + 1;
                mintot = 0;
            }
            $scope.effhrstot = hrstot;
            $scope.minmintot = mintot;
            $scope.alltask = $scope.get_plannerdetailsapp.every(function (options) {
                return options.checkedPlan;
            });

            var userTotal = $scope.get_plannerdetailsapp.length;
            var count = 0;
            angular.forEach($scope.get_plannerdetailsapp, function (item) {
                if (item.checkedPlan) {
                    count++;
                }
            });

            if (userTotal === count) {
                $scope.alltask = true;
            } else {
                $scope.alltask = false;
            }
        };

        $scope.onassignCheckstatus = function () {
            //var totcheck = 0;
            var toteffort = 0;
            angular.forEach($scope.get_plannerdetailsapp, function (ack) {
                //if (ack.ISMTPLTA_Status !== 'Open') {
                //    //totcheck = totcheck + 1;
                //    toteffort += parseFloat(ack.ISMTPLTA_EffortInHrs);
                //}
                if (ack.checkedPlan === true && (ack.plannerStatus === "1" || ack.plannerStatus === 1)) {
                    //totcheck = totcheck + 1;
                    toteffort += parseFloat(parseInt(ack.effhrs, 10) + parseInt(ack.effmin, 10) / 60);
                }
            });
            //$scope.totalcheck = totcheck;
            $scope.totaleffort = toteffort;
            var decimaltimetot = $scope.totaleffort;
            var hrstot = parseInt(Number(decimaltimetot));
            var mintot = Math.round((Number(decimaltimetot) - hrstot) * 60);
            if (hrstot.toString().length === 1) {
                hrstot = '0' + hrstot;
            }
            if (mintot.toString().length === 1) {
                mintot = '0' + mintot;
            }
            if (mintot === 60) {
                hrstot = hrstot + 1;
                mintot = 0;
            }
            $scope.effhrstot = hrstot;
            $scope.minmintot = mintot;
            $scope.alltask = $scope.get_plannerdetailsapp.every(function (options) {
                return options.checkedPlan;
            });
        };

        $scope.counteffort = function () {
            var finaleffort = 0;
            angular.forEach($scope.get_plannerdetails, function (ack) {
                finaleffort += parseFloat(parseInt(ack.effhrs, 10) + parseInt(ack.effmin, 10) / 60);
            });
            $scope.totaleffort = finaleffort;
            var decimaltimetot = $scope.totaleffort;
            var hrstot = parseInt(Number(decimaltimetot));
            var mintot = Math.round((Number(decimaltimetot) - hrstot) * 60);
            if (hrstot.toString().length === 1) {
                hrstot = '0' + hrstot;
            }
            if (mintot.toString().length === 1) {
                mintot = '0' + mintot;
            }
            if (mintot === 60) {
                hrstot = hrstot + 1;
                mintot = 0;
            }
            $scope.effhrstot = hrstot;
            $scope.minmintot = mintot;
        };

        // ****** View The Task Description ****** //
        //$scope.viewresponse = function (objs) {
        //     $('#myModalResponse').modal('show');

        //    var data = {
        //        "ISMTCR_Id": objs.ISMTCR_Id
        //    };
        //    apiService.create("ISM_DailyReportGeneration/viewresponse", data).then(function (promise) {

        //        if (promise !== null) {
        //            $scope.gettaskdetails = promise.gettaskdetails;
        //            if ($scope.gettaskdetails !== null && $scope.gettaskdetails.length > 0) {
        //                $scope.tasktitle = $scope.gettaskdetails[0].ISMTCR_Title;
        //                $scope.taskno = $scope.gettaskdetails[0].ISMTCR_TaskNo;
        //                $scope.created_by = $scope.gettaskdetails[0].createdby;
        //                $scope.taskclient = $scope.gettaskdetails[0].ISMMCLT_ClientName;
        //                $scope.taskdate = $scope.gettaskdetails[0].ISMTCR_CreationDate;
        //                $scope.tasktype = $scope.gettaskdetails[0].ISMTCR_BugOREnhancementFlg;
        //                $scope.taskpriority = $scope.gettaskdetails[0].HRMP_Name;
        //                $scope.taskdescription = $scope.gettaskdetails[0].ISMTCR_Desc;
        //                $scope.taskstatus = $scope.gettaskdetails[0].ISMTCR_Status;
        //                $scope.assignedby = $scope.gettaskdetails[0].AssignedEmployee;
        //            }
        //        }
        //    });
        //};

        $scope.viewresponse = function (taskobj) {
            //$('#myModalResponse').modal('show');            
            $scope.ismtcrreS_Response = "";
            $scope.files_paths = "";
            $scope.ismmistS_StatusName = "";
            $scope.taskAttatchment = [];
            $scope.get_responseattachments = [];

            var data = {
                "ISMTCR_Id": taskobj.ISMTCR_Id
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
                    if ($scope.get_responseattachments.length > 0) {
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
                    $('#myModalResponse').modal('show');
                }
            });
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
        //=========== Close Image Model
        $scope.closeimg = function () {
            $('#myModalPreview').modal('hide');
            $('.modal-backdrop').remove();
        };
        //======================================== Add task Model
        $scope.addtask = function () {
            var data = {
                "ISMTPL_Id": $scope.plannerid
            };
            apiService.create("ISM_PlannerApproval/getextratask", data).then(function (promise) {
                $scope.get_extratasklist = promise.get_extratasklist;

                if ($scope.get_extratasklist.length > 0) {
                    // $('#myModalExtraTask').modal('show');
                }
                else {
                    swal("No Task Found.....!!");
                }
            });
        };

        //========================================= Add Extra Task    
        $scope.saveAddTask = function () {

            if ($scope.myForm1.$valid) {
                angular.forEach($scope.get_extratasklist, function (et) {
                    var count = 0;
                    var checkestra = false;
                    if (et.checkedTask === true) {
                        angular.forEach($scope.get_plannerdetailsapp, function (itm) {
                            if (itm.ISMTCR_Id === et.ISMTCR_Id) {
                                checkestra = true;
                            }
                        });
                        if (checkestra === false) {
                            et.ISMTPL_Id = $scope.get_plannerdetailsapp[0].ISMTPL_Id;
                            et.extraflag = 1;
                            et.ISMTCR_Id = et.ISMTCR_Id;
                            et.ISMTPLTA_StartDate = new Date(et.ISMTPLAPTA_StartDate);
                            et.ISMTPLTA_EndDate = new Date(et.ISMTPLAPTA_EndDate);
                            et.ISMTPLTA_EffortInHrs = et.ISMTPLAPTA_EffortInHrs;

                            et.ISMTCRASTO_StartDate = new Date(et.ISMTPLAPTA_StartDate).toDateString();
                            et.ISMTCRASTO_EndDate = new Date(et.ISMTPLAPTA_EndDate).toDateString();
                            et.ISMTCRASTO_EffortInHrs = et.ISMTPLAPTA_EffortInHrs;
                            et.ISMTPLTA_Status = et.ISMTCR_Status;
                            et.ISMTPLTA_Remarks = et.ISMTPLAPTA_Remarks;

                            et.deviationflag = false;
                            et.plannerStatus = 1;
                            $scope.get_plannerdetailsapp.push(et);
                            $scope.totalplannertask = $scope.totalplannertask + 1;
                        }
                        else {
                            swal("Task alreaday added to planner !!");
                        }
                    }
                });
                //  $('#myModalExtraTask').modal('hide');
                $('.modal-backdrop').remove();
            } else {
                $scope.submitted1 = true;
                //  $('#myModalExtraTask').modal('show');
            }
            console.log($scope.get_plannerlist);
        };


        $scope.AllupdateplannerStatus = function () {
            $scope.submitted = true;
            var data = {};
            $scope.employee = [];
            if ($scope.myForm.$valid) {
                angular.forEach($scope.get_plannerlist, function (itm) {
                    if (itm.checked === true) {
                        $scope.employee.push(itm);
                    }
                });
                data = {
                    "Emplist": $scope.employee
                };
                apiService.create("ISM_PlannerApproval/allupdateplannerStatus", data).then(function (promise) {
                    if (promise.returnval === true) {
                        swal('Record updated successfully');
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.ismtpL_Id === 0 || promise.ismtpL_Id < 0) {
                            swal('Failed to Save, please contact administrator');
                        }
                        else if (promise.ismtpL_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }

                });
            }
            else {
                $scope.submitted = true;
            }
        };
        //========================================= Approv Planner      
        $scope.updateplannerStatus = function () {
            $scope.submitted = true;
            var data = {};
            var minieffort = 0;
            if ($scope.totaleffort >= $scope.nmineffort || $scope.totaleffort==0) {
                if ($scope.myForm.$valid) {
                    $scope.approveplannerArray = [];
                    $scope.plannercount = $scope.get_plannerdetailsapp.length;
                    var rejetedcount = 0;
                    var appcount = 0;
                    var rejcount = 0;
                    var checkedcount = 0;
                    angular.forEach($scope.get_plannerdetailsapp, function (pl) {
                        if (pl.plannerStatus === "0" || pl.plannerStatus === 0) {
                            rejetedcount += 1;
                        }
                        if (pl.checkedPlan === true) {
                            checkedcount += 1;
                        }
                    });
                    //if ($scope.plannercount === rejetedcount && $scope.totaleffort === 0) {
                    //    $scope.totaleffort = $scope.mineffort;
                    //}
                    if (checkedcount !== 0) {
                        minieffort = $scope.mineffort;
                        if ($scope.obj.plannerRemarks !== null && $scope.obj.plannerRemarks !== undefined) {
                            //if ($scope.totaleffort >= $scope.mineffort) {
                            angular.forEach($scope.get_plannerdetailsapp, function (tp) {
                                var counthrs = 0.00;
                                var totalhrs = 0.00;

                                if (tp.checkedPlan !== true && (tp.plannerStatus === "1" || tp.plannerStatus === 1)) {
                                    tp.plannerStatus = "0";
                                }
                                counthrs = (parseFloat(tp.effmin) * 0.0166667).toFixed(2);
                                counthrs = parseFloat(tp.effhrs) + parseFloat(counthrs);

                                $scope.approveplannerArray.push({
                                    ISMTCR_Id: tp.ISMTCR_Id, ISMTPLAPTA_StartDate: new Date(tp.ISMTPLTA_StartDate).toDateString(), ISMTPLAPTA_EndDate: new Date(tp.ISMTPLTA_EndDate).toDateString(), ISMTPLAPTA_Status: tp.ISMTPLTA_Status,
                                    ISMTPLAPTA_EffortInHrs: counthrs, ISMTPLAPTA_Remarks: tp.ISMTPLAP_Remarks, plannerStatus: tp.plannerStatus, ISMTPLAPTA_Id: tp.ISMTPLAPTA_Id, ISMTCRASTO_StartDate: new Date(tp.ISMTPLTA_StartDate).toDateString(), ISMTCRASTO_EndDate: new Date(tp.ISMTPLTA_EndDate).toDateString(), ISMTCRASTO_EffortInHrs: tp.ISMTPLTA_EffortInHrs, deviationflag: tp.deviationflag, extraflag: tp.extraflag, ISMTPLTA_Id: tp.ISMTPLTA_Id, PTSCount: tp.PTSCount
                                });
                            });

                            if ($scope.editflg === 1) {
                                data = {
                                    "ISMTPL_Id": $scope.plannerid,
                                    "ISMTPLAP_Remarks": $scope.obj.plannerRemarks,
                                    "approveplannerArray": $scope.approveplannerArray,
                                    "ISMTPLAP_Id": $scope.ismtplaP_Id,
                                    "totaleffort": $scope.totaleffort,
                                    "view": $scope.viewtemp
                                };
                            }
                            else {
                                data = {
                                    "ISMTPL_Id": $scope.plannerid,
                                    "ISMTPLAP_Remarks": $scope.obj.plannerRemarks,
                                    "approveplannerArray": $scope.approveplannerArray,
                                    "totaleffort": $scope.totaleffort,
                                    "view": $scope.viewtemp,
                                    "ISMTPLAP_Id": $scope.ismtplaP_Id,
                                };
                            }
                            //}
                            //else {
                            //    swal("Total Effort must be greater than or equal to " + minieffort + " Hours ... !!");
                            //    return false;
                            //}

                            angular.forEach($scope.get_plannerdetailsapp, function (pl) {
                                if (pl.plannerStatus === "0" || pl.plannerStatus === 0) {
                                    rejcount += 1;
                                }
                                else {
                                    appcount += 1;
                                }
                            });

                            var swalmsg = "";
                            if (appcount == 0) {
                                swalmsg = "Do you want to reject planner?";
                            }
                            else {
                                swalmsg = "You are Approving " + appcount + " and Rejecting " + rejcount + " Tasks?";
                            }
                            swal({
                                title: "Are you sure?",                               
                                text: swalmsg,
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Approve",
                                cancelButtonText: "Cancel",
                                closeOnConfirm: false,
                                closeOnCancel: false
                            },
                                function (isConfirm) {
                                    if (isConfirm) {
                                        apiService.create("ISM_PlannerApproval/updateplannerStatus", data).then(function (promise) {
                                            if (promise.returnval === true) {
                                                if (promise.ismtpL_Id === 0 || promise.ismtpL_Id < 0) {
                                                    swal('Planner Saved successfully ... !!');
                                                    $state.reload();
                                                }
                                                else if (promise.ismtpL_Id > 0) {
                                                    if (promise.ismtpL_ApprovalFlg === false) {
                                                        swal('Record updated successfully', 'Planner extention given to regenerate!');
                                                    }
                                                    else {
                                                        swal('Record updated successfully');
                                                    }
                                                    $state.reload();
                                                }
                                            }
                                            else if (promise.returnduplicatestatus === 'Duplicate') {
                                                swal('Record already exist');
                                            }
                                            else {
                                                if (promise.ismtpL_Id === 0 || promise.ismtpL_Id < 0) {
                                                    swal('Failed to Save, please contact administrator');
                                                }
                                                else if (promise.ismtpL_Id > 0) {
                                                    swal('Failed to update, please contact administrator');
                                                }
                                            }
                                        });
                                    }
                                    else {
                                        swal("Planner Approval Cancelled!!!");
                                    }
                                });
                        }
                        else {
                            swal("Please Enter Remarks");
                        }
                    }
                    else {
                        swal("Please Select Atleast One Task");
                    }
                }
                else {
                    $scope.submitted = true;
                }
            }
            else {
                swal("Approving effort must be greater than " + $scope.nmineffort+" Hrs.");
            }
           
        };
        $scope.removetask = function (objtask) {
            $scope.ISMTCRASTO_Id = objtask.ismtcrastO_Id;
            var tasknum = objtask.ISMTCR_TaskNo;

            var dystring = "Remove";
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Task " + tasknum + "..?",

                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ISM_PlannerCreation/removetask", objtask).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Task " + dystring + "Successfully..");
                                    $state.reload();
                                }
                                else {
                                    swal("Falied to " + dystring + " Task....!!!");
                                }

                            });
                    }
                    else {
                        swal("Task " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //======================================== View Approved Planner Details
        $scope.getApprovedPlannerDetail = function (objp) {
            var data = {
                "HRMD_Id": $scope.hrmD_Id,
                "ISMTPLAP_Id": objp.ISMTPLAP_Id
            };
            apiService.create("ISM_PlannerApproval/getApprovedPlannerDetail", data).then(function (promise) {
                $scope.get_ApprovedPlannerDetail = promise.get_ApprovedPlannerDetail;

                if ($scope.get_ApprovedPlannerDetail.length > 0) {
                    $scope.plannername = $scope.get_ApprovedPlannerDetail[0].ISMTPL_PlannerName;
                }
                else {
                    swal("No Task Found.....!!");
                }
            });
        };


        //VIEW NEW PLANNER
        $scope.ename = '';
        $scope.plnn = '';
        $scope.overalleffort = 0;
        $scope.overalleffortC = 0;
        $scope.overalleffortCP = 0;
        $scope.overalleffortD = 0;
        $scope.overalleffortDP = 0;
        $scope.getplannerdetailsviewNew = function (objp) {
            $scope.nmineffort = 0;
            $scope.totminhr = 0;
            $scope.totminmin = 0;
            var nmineffort = 0;
            var ntwdays = 0;
            $scope.ename = '';
            $scope.HRME_Id1 = objp.HRME_Id;
            $scope.startdate1 = objp.ISMTPL_StartDate;
            $scope.enddate1 = objp.ISMTPL_EndDate;
            $scope.overalleffortC = 0;
            $scope.overalleffortCP = 0;
            $scope.overalleffortD = 0;
            $scope.overalleffortDP = 0;
            $scope.overalleffort = parseFloat(objp.ISMTPL_TotalHrs);

            var decimaltime = $scope.overalleffort;
            var hrs = parseInt(Number(decimaltime));
            var min = Math.round((Number(decimaltime) - hrs) * 60);
            if (hrs.toString().length === 1) {
                hrs = '0' + hrs;
            }
            if (min.toString().length === 1) {
                min = '0' + min;
            }
            $scope.effhrsov = hrs;
            $scope.effminov = min;

            $scope.plnn = objp.ISMTPL_PlannerName;
            $scope.appflag = objp.ISMTPL_ApprovalFlg;

            $scope.sdate = objp.ISMTPL_StartDate;
            $scope.edate = objp.ISMTPL_EndDate;
            $scope.appchart_extratask = [];
            $scope.appchart_deviation = [];
            $scope.appchart_category = [];

            $scope.get_plannerdetails = [];
            var data = {
                "ISMTPL_Id": objp.ISMTPL_Id
            };
            apiService.create("ISM_PlannerCreation/getplannerdetails", data).then(function (promise) {

                $scope.plannerdetailsall = [];
                if (promise.get_plannerdetails.length > 0) {
                    $scope.plannerdetails = promise.get_plannerdetails;
                    $scope.totaltaskcount = promise.get_plannerdetails.length;

                    $scope.nget_effortdetails = promise.get_effortdetails;
                    if ($scope.nget_effortdetails!=null && $scope.nget_effortdetails.length > 0) {
                        angular.forEach($scope.nget_effortdetails, function (tf) {
                            nmineffort += parseInt(tf.WORKINGHOURS);
                            ntwdays += parseInt(tf.NOOFDAYS);
                        });
                    }
                    $scope.nmineffort = nmineffort;
                    var ndecimaltimemin = $scope.nmineffort;
                    var nhrsmin = parseInt(Number(ndecimaltimemin));
                    var nminmin = Math.round((Number(ndecimaltimemin) - nhrsmin) * 60);
                    if (nhrsmin.toString().length === 1) {
                        nhrsmin = '0' + nhrsmin;
                    }
                    if (nminmin.toString().length === 1) {
                        nminmin = '0' + nminmin;
                    }
                    $scope.totminhr = nhrsmin;
                    $scope.totminmin = nminmin;

                    var cntt = 0;
                    angular.forEach($scope.plannerdetails, function (pl) {
                        var effrt11 = 0;
                        var status = '';
                        cntt += 1;
                        if (cntt === 1) {
                            $scope.ename = pl.ENAME;
                            $scope.approvedby = pl.approvedby;
                        }
                        var cls = '';
                        if (pl.ISMTPLTA_PreviousTask !== null && pl.ISMTPLTA_PreviousTask == true) {
                            cls = 'backkk';
                        }
                        if (pl.ISMTPLAP_Id === null) {

                            if (pl.ISMTPL_ApprovalFlg === true) {
                                status = "APPROVED";
                            }
                            else {
                                status = "PENDING";
                            }
                        }
                        else {
                            if (pl.ISMTPLTA_ApprovalFlg === false) {

                                status = "REJECTED";
                            }
                            else {

                                status = "APPROVED";
                            }
                        }



                        $scope.get_plannerdetails.push({
                            ISMTPL_Id: pl.ISMTPL_Id, ISMTCR_Id: pl.ISMTCR_Id, HRMP_Name: pl.HRMP_Name, ISMTCR_BugOREnhancementFlg: pl.ISMTCR_BugOREnhancementFlg, ISMTCR_Title: pl.ISMTCR_Title, ISMTCR_Status: pl.ISMTCR_Status, ISMTCR_TaskNo: pl.ISMTCR_TaskNo, ISMMCLT_ClientName: pl.ISMMCLT_ClientName, ISMTPL_CreatedBy: pl.ISMTPL_CreatedBy, plannedby: pl.plannedby, ISMTPL_PlannerName: pl.ISMTPL_PlannerName, ISMTPLTA_Remarks: pl.ISMTPLTA_Remarks, CreatedDate: pl.CreatedDate, ISMTCR_Desc: pl.ISMTCR_Desc, ISMTPLTA_Status: pl.ISMTPLTA_Status,
                            ISMTCR_CreationDate: pl.ISMTCR_CreationDate, ISMTPLTA_StartDate: new Date(pl.ISMTPLTA_StartDate), ISMTPLTA_EndDate: new Date(pl.ISMTPLTA_EndDate), ISMTPLTA_EffortInHrs: pl.ISMTPLTA_EffortInHrs, ISMMTCAT_Id: pl.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: pl.ISMMTCAT_TaskCategoryName, ISMMTCAT_TaskPercentage: pl.ISMMTCAT_TaskPercentage, ISMMTCAT_DurationFlg: pl.ISMMTCAT_DurationFlg, ISMMTCAT_CompulsoryFlg: pl.ISMMTCAT_CompulsoryFlg, ISMMTCAT_EachTaskMaxDuration: pl.ISMMTCAT_EachTaskMaxDuration, Flg: pl.ISMTPLTA_PreviousTask, cls: cls, status: status, ISMTPLTA_ExtraFlg: pl.ISMTPLTA_ExtraFlg, Periodicity: pl.Periodicity
                        });


                        $scope.plannerdetailsall = $scope.get_plannerdetails;
                    });
                    //============Shivu start========

                    $scope.get_plannerdetails_approval = [];
                    $scope.get_plannerdetails_reject = [];
                    $scope.get_plannerdetails_previous = [];
                    $scope.get_plannerdetails_extratask = [];

                    angular.forEach($scope.get_plannerdetails, function (dev) {
                        if (dev.ISMTPLTA_ExtraFlg === false) {
                            if (dev.cls !== "backkk") {
                                if (dev.status === "APPROVED") {
                                    $scope.get_plannerdetails_approval.push(dev);
                                    $scope.pending = false;
                                }
                                else if (dev.status === "PENDING") {
                                    $scope.get_plannerdetails_approval.push(dev);
                                    $scope.pending = true;
                                }
                                else {
                                    $scope.get_plannerdetails_reject.push(dev);
                                }
                            }
                            else {
                                $scope.get_plannerdetails_previous.push(dev);
                            }
                        }
                        else {
                            $scope.get_plannerdetails_extratask.push(dev);
                        }
                    });

                    $scope.get_plannerdetails = $scope.get_plannerdetails_approval
                    if ($scope.get_plannerdetails_extratask.length > 0) {
                        $scope.extratask_one = $scope.get_plannerdetails_extratask.length
                    }
                    $scope.extratask = [];
                    $scope.extratask.push({ extracount: $scope.extratask_one })
                    //-------------------Approval/Pending
                    $scope.taskid = [];
                    angular.forEach($scope.get_plannerdetails, function (dev) {
                        if ($scope.taskid.length === 0) {
                            $scope.taskid.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                        } else if ($scope.taskid.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.taskid, function (emp) {
                                if (emp.ISMMTCAT_Id === dev.ISMMTCAT_Id) {
                                    intcount += 1;
                                }
                            });

                            if (intcount === 0) {
                                $scope.taskid.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                            }
                        }
                    });
                    console.log($scope.taskid);
                    angular.forEach($scope.taskid, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.get_plannerdetails, function (dd) {
                            if (dd.ISMMTCAT_Id === ddd.ISMMTCAT_Id) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.get_plannerdetails = $scope.templist;
                    });

                    //------------------Reject

                    $scope.taskid1 = [];
                    angular.forEach($scope.get_plannerdetails_reject, function (dev) {
                        if ($scope.taskid1.length === 0) {
                            $scope.taskid1.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                        } else if ($scope.taskid1.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.taskid1, function (emp) {
                                if (emp.ISMMTCAT_Id === dev.ISMMTCAT_Id) {
                                    intcount += 1;
                                }
                            });

                            if (intcount === 0) {
                                $scope.taskid1.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                            }
                        }
                    });
                    console.log($scope.taskid1);
                    angular.forEach($scope.taskid1, function (ddd) {
                        $scope.templist1 = [];
                        angular.forEach($scope.get_plannerdetails_reject, function (dd) {
                            if (dd.ISMMTCAT_Id === ddd.ISMMTCAT_Id) {
                                $scope.templist1.push(dd);
                            }
                        });
                        ddd.get_plannerdetails_reject = $scope.templist1;
                    });


                    //------------------extra task

                    $scope.taskid3 = [];
                    angular.forEach($scope.get_plannerdetails_extratask, function (dev) {

                        if ($scope.taskid3.length === 0) {
                            $scope.taskid3.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                        } else if ($scope.taskid3.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.taskid3, function (emp) {
                                if (emp.ISMMTCAT_Id === dev.ISMMTCAT_Id) {
                                    intcount += 1;
                                }
                            });

                            if (intcount === 0) {
                                $scope.taskid3.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                            }
                        }
                    });
                    console.log($scope.taskid3);
                    angular.forEach($scope.taskid3, function (ddd) {
                        $scope.templist3 = [];
                        angular.forEach($scope.get_plannerdetails_extratask, function (dd) {
                            if (dd.ISMMTCAT_Id === ddd.ISMMTCAT_Id) {
                                $scope.templist3.push(dd);
                            }
                        });
                        ddd.get_plannerdetails_extratask = $scope.templist3;
                    });
                    //-------------------Deviated
                    $scope.taskid5 = [];
                    angular.forEach($scope.get_plannerdetails_previous, function (dev) {
                        if ($scope.taskid5.length === 0) {
                            $scope.taskid5.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                        } else if ($scope.taskid5.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.taskid5, function (emp) {
                                if (emp.ISMMTCAT_Id === dev.ISMMTCAT_Id) {
                                    intcount += 1;
                                }
                            });

                            if (intcount === 0) {
                                $scope.taskid5.push({ ISMMTCAT_Id: dev.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: dev.ISMMTCAT_TaskCategoryName, ISMMTCAT_CompulsoryFlg: dev.ISMMTCAT_CompulsoryFlg });
                            }
                        }
                    });
                    console.log($scope.taskid5);
                    angular.forEach($scope.taskid5, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.get_plannerdetails_previous, function (dd) {
                            if (dd.ISMMTCAT_Id === ddd.ISMMTCAT_Id) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.get_plannerdetails_previous = $scope.templist;
                    });
                    //-------------------------------------

                    angular.forEach($scope.get_plannerdetails_previous, function (dd) {
                        var decimaltime = dd.ISMTPLTA_EffortInHrs;
                        var hrs = parseInt(Number(decimaltime));
                        var min = Math.round((Number(decimaltime) - hrs) * 60);
                        if (hrs.toString().length === 1) {
                            hrs = '0' + hrs;
                        }
                        if (min.toString().length === 1) {
                            min = '0' + min;
                        }
                        dd.effhrs = hrs;
                        dd.effmin = min;
                    });

                    angular.forEach($scope.get_plannerdetails, function (dd) {
                        var decimaltime = dd.ISMTPLTA_EffortInHrs;
                        var hrs = parseInt(Number(decimaltime));
                        var min = Math.round((Number(decimaltime) - hrs) * 60);
                        if (hrs.toString().length === 1) {
                            hrs = '0' + hrs;
                        }
                        if (min.toString().length === 1) {
                            min = '0' + min;
                        }
                        dd.effhrs = hrs;
                        dd.effmin = min;
                    });

                    angular.forEach($scope.get_plannerdetails_reject, function (dd) {
                        var decimaltime = dd.ISMTPLTA_EffortInHrs;
                        var hrs = parseInt(Number(decimaltime));
                        var min = Math.round((Number(decimaltime) - hrs) * 60);
                        if (hrs.toString().length === 1) {
                            hrs = '0' + hrs;
                        }
                        if (min.toString().length === 1) {
                            min = '0' + min;
                        }
                        dd.effhrs = hrs;
                        dd.effmin = min;
                    });

                    angular.forEach($scope.get_plannerdetails_extratask, function (dd) {
                        var decimaltime = dd.ISMTPLTA_EffortInHrs;
                        var hrs = parseInt(Number(decimaltime));
                        var min = Math.round((Number(decimaltime) - hrs) * 60);
                        if (hrs.toString().length === 1) {
                            hrs = '0' + hrs;
                        }
                        if (min.toString().length === 1) {
                            min = '0' + min;
                        }
                        dd.effhrs = hrs;
                        dd.effmin = min;
                    });


                    //============Shivu End========


                    $scope.categoryidlist1 = [];
                    var snocount = 0;
                    var countsubgrp = 0;
                    angular.forEach($scope.plannerdetailsall, function (sub) {
                        if ($scope.categoryidlist1.length === 0) {
                            snocount += 1;
                            $scope.categoryidlist1.push({ ISMMTCAT_Id: sub.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: sub.ISMMTCAT_TaskCategoryName, ISMMTCAT_TaskPercentage: sub.ISMMTCAT_TaskPercentage, ISMMTCAT_DurationFlg: sub.ISMMTCAT_DurationFlg, ISMMTCAT_CompulsoryFlg: sub.ISMMTCAT_CompulsoryFlg, ISMMTCAT_EachTaskMaxDuration: sub.ISMMTCAT_EachTaskMaxDuration });
                        } else if ($scope.categoryidlist1.length > 0) {
                            countsubgrp = 0;
                            angular.forEach($scope.categoryidlist1, function (subgrp) {
                                if (subgrp.ISMMTCAT_Id === sub.ISMMTCAT_Id) {
                                    countsubgrp += 1;
                                }
                            });
                            if (countsubgrp === 0) {
                                snocount += 1;
                                $scope.categoryidlist1.push({ ISMMTCAT_Id: sub.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: sub.ISMMTCAT_TaskCategoryName, ISMMTCAT_TaskPercentage: sub.ISMMTCAT_TaskPercentage, ISMMTCAT_DurationFlg: sub.ISMMTCAT_DurationFlg, ISMMTCAT_CompulsoryFlg: sub.ISMMTCAT_CompulsoryFlg, ISMMTCAT_EachTaskMaxDuration: sub.ISMMTCAT_EachTaskMaxDuration });
                            }
                        }
                    });



                    $scope.finalcategoryidlist = [];


                    $scope.overalleffortC = 0;
                    $scope.overalleffortCP = 0;
                    $scope.overalleffortD = 0;
                    $scope.overalleffortDP = 0;

                    angular.forEach($scope.plannerdetailsall, function (ww) {
                        if (ww.status != 'REJECTED') {
                            if (ww.Flg == true && ww.Flg != null) {
                                $scope.overalleffortD = parseFloat($scope.overalleffortD) + parseFloat(ww.ISMTPLTA_EffortInHrs)
                                var decimaltime1 = ($scope.overalleffortD);
                                var hrsc = parseInt(Number(decimaltime1));
                                var minc = Math.round((Number(decimaltime1) - hrsc) * 60);
                                if (hrsc.toString().length === 1) {
                                    hrsc = '0' + hrsc;
                                }
                                if (minc.toString().length === 1) {
                                    minc = '0' + minc;
                                }
                                $scope.effhrsovdc = hrsc;
                                $scope.effminovdc = minc;
                            }
                        }
                    });

                    if ($scope.overalleffort > 0) {
                        $scope.overalleffortCP = (($scope.overalleffort - $scope.overalleffortD) / $scope.overalleffort) * 100;
                        $scope.overalleffortDP = ($scope.overalleffortD / $scope.overalleffort) * 100;

                        var decimaltime = ($scope.overalleffort - $scope.overalleffortD);
                        var hrs = parseInt(Number(decimaltime));
                        var min = Math.round((Number(decimaltime) - hrs) * 60);
                        if (hrs.toString().length === 1) {
                            hrs = '0' + hrs;
                        }
                        if (min.toString().length === 1) {
                            min = '0' + min;
                        }
                        $scope.effhrsovd = hrs;
                        $scope.effminovd = min;



                    }
                    angular.forEach($scope.categoryidlist1, function (rr) {
                        var cateffort = 0;
                        var devcateffort = 0;
                        var curper = 0;
                        var devper = 0;
                        angular.forEach($scope.plannerdetailsall, function (ff) {
                            if (ff.ISMMTCAT_Id != null && ff.ISMMTCAT_Id != undefined) {
                                if (parseInt(rr.ISMMTCAT_Id) == parseInt(ff.ISMMTCAT_Id)) {
                                    if (ff.status != 'REJECTED') {
                                        if (ff.Flg == true) {
                                            devcateffort = devcateffort + parseFloat(ff.ISMTPLTA_EffortInHrs);
                                        }
                                        if (ff.status != 'REJECTED') {
                                            cateffort = cateffort + parseFloat(ff.ISMTPLTA_EffortInHrs);
                                        }
                                    }
                                }
                            }
                        });

                        if (cateffort != 0) {
                            curper = (parseFloat(cateffort - devcateffort) / cateffort) * 100;
                            devper = (parseFloat(devcateffort) / cateffort) * 100;

                            $scope.finalcategoryidlist.push({
                                ISMMTCAT_Id: rr.ISMMTCAT_Id,
                                CNAME: rr.ISMMTCAT_TaskCategoryName,
                                MINP: rr.ISMMTCAT_TaskPercentage,
                                TE: cateffort, DE: devcateffort, CWP: curper, DP: devper, flg: rr.ISMMTCAT_CompulsoryFlg
                            })
                            $scope.finalcategory = [];
                            $scope.finalcategory = $scope.finalcategoryidlist
                        }
                        angular.forEach($scope.finalcategoryidlist, function (dd) {
                            var cateff = dd.TE;
                            var hrscat = parseInt(Number(cateff));
                            var mincat = Math.round((Number(cateff) - hrscat) * 60);
                            if (hrscat.toString().length === 1) {
                                hrscat = '0' + hrscat;
                            }
                            if (mincat.toString().length === 1) {
                                mincat = '0' + mincat;
                            }
                            dd.effhrcat = hrscat;
                            dd.effmicat = mincat;

                            var cateffCCE = dd.TE - dd.DE;
                            var hrscatCCE = parseInt(Number(cateffCCE));
                            var mincatCCE = Math.round((Number(cateffCCE) - hrscatCCE) * 60);
                            if (hrscatCCE.toString().length === 1) {
                                hrscatCCE = '0' + hrscatCCE;
                            }
                            if (mincatCCE.toString().length === 1) {
                                mincatCCE = '0' + mincatCCE;
                            }
                            dd.effhrcatCCE = hrscatCCE;
                            dd.effmicatCCE = mincatCCE;

                            var catefffin = dd.DE;
                            var hrscatfin = parseInt(Number(catefffin));
                            var mincatfin = Math.round((Number(catefffin) - hrscatfin) * 60);
                            if (hrscatfin.toString().length === 1) {
                                hrscatfin = '0' + hrscatfin;
                            }
                            if (mincatfin.toString().length === 1) {
                                mincatfin = '0' + mincatfin;
                            }
                            dd.effhrcatfin = hrscatfin;
                            dd.effmicatfin = mincatfin;
                        });

                    })


                    //====================
                    $scope.get_category = [];
                    angular.forEach($scope.finalcategory, function (qq) {

                        $scope.appname = qq.CNAME;
                        $scope.appdata = qq.MINP;
                        $scope.get_category.push({ Id: $scope.appname, Data: $scope.appdata });
                    })

                    $scope.appchart_category = [];
                    angular.forEach($scope.get_category, function (value, index) {
                        $scope.appchart_category.push({ 'label': value.Id, 'y': value.Data });
                    });
                    var chart = new CanvasJS.Chart("appchartcategory", {
                        title: {
                            text: " Chart Representation Of Category "
                        },
                        data: [
                            {
                                // Change type to "doughnut", "line", "splineArea", etc.
                                type: "line",
                                dataPoints: $scope.appchart_category
                            }
                        ]
                    });
                    chart.render();


                    //=====================
                    $scope.get_deviation = [];
                    $scope.get_deviationtotal = [];
                    $scope.appdatanew = 0;

                    $scope.appname = "Current Week Effort";
                    $scope.appdata = $scope.overalleffortCP / 100;
                    $scope.get_deviation.push({ Id: $scope.appname, Data: $scope.appdata });
                    $scope.appname = "Deviated Effort";
                    $scope.appdata = $scope.overalleffortDP / 100;
                    $scope.get_deviation.push({ Id: $scope.appname, Data: $scope.appdata });

                    $scope.appchart_deviation = [];
                    angular.forEach($scope.get_deviation, function (value, index) {
                        $scope.appchart_deviation.push({ 'label': value.Id, 'y': value.Data });
                    });
                    var chart = new CanvasJS.Chart("appchartdeviation", {
                        animationEnabled: true,
                        title: {
                            text: " Chart Representation Of Last Week Deviation "
                        },
                        data: [
                            {
                                // Change type to "doughnut", "line", "splineArea", etc.
                                type: "pie",
                                startAngle: 240,
                                yValueFormatString: "#,##0.########%",
                                indexLabel: "{label} {y}",
                                dataPoints: $scope.appchart_deviation
                            }
                        ]
                    });
                    chart.render();

                    //=======================================
                    //=====================
                    $scope.get_extratask = [];

                    $scope.appname = "Total Task";
                    $scope.appdata = $scope.totaltaskcount;
                    $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });

                    $scope.appname = "Deviated Task";
                    $scope.appdata = $scope.get_plannerdetails_previous.length;
                    $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });

                    if ($scope.pending === true) {
                        $scope.appname = "Approval Pending Task";
                        $scope.appdata = $scope.get_plannerdetails.length;
                        $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });
                    }
                    else {
                        $scope.appname = "Approved Task";
                        $scope.appdata = $scope.get_plannerdetails.length;
                        $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });
                    }
                    if ($scope.pending === false) {

                        $scope.appname = "Rejected Task";
                        $scope.appdata = $scope.get_plannerdetails_reject.length;
                        $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });

                        $scope.appname = "Extra Task";
                        $scope.appdata = $scope.get_plannerdetails_extratask.length;
                        $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });
                    }

                    //angular.forEach($scope.extratask, function (qq) {
                    //    $scope.appname = "Extra Task";
                    //    $scope.appdata = qq.extracount;
                    //    $scope.get_extratask.push({ Id: $scope.appname, Data: $scope.appdata });
                    //})

                    $scope.appchart_extratask = [];
                    angular.forEach($scope.get_extratask, function (value, index) {
                        $scope.appchart_extratask.push({ 'label': value.Id, 'y': value.Data });
                    });
                    var chart = new CanvasJS.Chart("appcharttotaltask", {
                        title: {
                            text: " Chart Representation Of Total Task"
                        },
                        data: [
                            {
                                // Change type to "doughnut", "line", "splineArea", etc.
                                type: "column",
                                dataPoints: $scope.appchart_extratask
                            }
                        ]
                    });
                    chart.render();

                    //=======================================

                    console.log($scope.finalcategoryidlist);
                }
                else {
                    swal("No Record  Found..... !!");
                    $scope.get_plannerdetails = "";
                }

            });
        };

        $scope.getplannerdetailsviewNewcat = function (objp) {
            $scope.ename = '';
            $scope.overalleffortCx = 0;
            $scope.overalleffortCPx = 0;
            $scope.overalleffortDx = 0;
            $scope.overalleffortDPx = 0;
            $scope.overalleffortx = parseFloat(objp.ISMTPL_TotalHrs);
            var nmineffort = 0;
            var ntwdays = 0;
            $scope.nmineffort = 0;
            $scope.totminhr = 0;
            $scope.totminmin = 0;
            $scope.get_plannerdetailsx = [];
            var data = {
                "ISMTPL_Id": objp.ISMTPL_Id
            };
            apiService.create("ISM_PlannerCreation/getplannerdetails", data).then(function (promise) {

                $scope.plannerdetailsall = [];
                if (promise.get_plannerdetails.length > 0) {
                    $scope.plannerdetailsx = promise.get_plannerdetails;

                    $scope.nget_effortdetails = promise.get_effortdetails;
                    if ($scope.nget_effortdetails!=null && $scope.nget_effortdetails.length > 0) {
                        angular.forEach($scope.nget_effortdetails, function (tf) {
                            nmineffort += parseInt(tf.WORKINGHOURS);
                            ntwdays += parseInt(tf.NOOFDAYS);
                        });
                    }
                    $scope.nmineffort = nmineffort;
                    var ndecimaltimemin = $scope.nmineffort;
                    var nhrsmin = parseInt(Number(ndecimaltimemin));
                    var nminmin = Math.round((Number(ndecimaltimemin) - nhrsmin) * 60);
                    if (nhrsmin.toString().length === 1) {
                        nhrsmin = '0' + nhrsmin;
                    }
                    if (nminmin.toString().length === 1) {
                        nminmin = '0' + nminmin;
                    }
                    $scope.totminhr = nhrsmin;
                    $scope.totminmin = nminmin;

                    var cntt = 0;
                    angular.forEach($scope.plannerdetailsx, function (pl) {
                        var effrt11 = 0;
                        var status = '';
                        cntt += 1;
                        var cls = '';
                        if (pl.ISMTPLTA_PreviousTask !== null && pl.ISMTPLTA_PreviousTask == true) {
                            cls = 'backkk';
                        }
                        if (pl.ISMTPLAP_Id === null) {

                            if (pl.ISMTPL_ApprovalFlg === true) {
                                status = "APPROVED";
                            }
                            else {
                                status = "PENDING";
                            }
                        }
                        else {
                            if (pl.ISMTPLTA_ApprovalFlg === false) {

                                status = "REJECTED";
                            }
                            else {

                                status = "APPROVED";
                            }
                        }



                        $scope.get_plannerdetailsx.push({
                            ISMTPL_Id: pl.ISMTPL_Id, ISMTCR_Id: pl.ISMTCR_Id, HRMP_Name: pl.HRMP_Name, ISMTCR_BugOREnhancementFlg: pl.ISMTCR_BugOREnhancementFlg, ISMTCR_Title: pl.ISMTCR_Title, ISMTCR_Status: pl.ISMTCR_Status, ISMTCR_TaskNo: pl.ISMTCR_TaskNo, ISMMCLT_ClientName: pl.ISMMCLT_ClientName, ISMTPL_CreatedBy: pl.ISMTPL_CreatedBy, plannedby: pl.plannedby, ISMTPL_PlannerName: pl.ISMTPL_PlannerName, ISMTPLTA_Remarks: pl.ISMTPLTA_Remarks, CreatedDate: pl.CreatedDate, ISMTCR_Desc: pl.ISMTCR_Desc, ISMTPLTA_Status: pl.ISMTPLTA_Status,
                            ISMTCR_CreationDate: pl.ISMTCR_CreationDate, ISMTPLTA_StartDate: new Date(pl.ISMTPLTA_StartDate), ISMTPLTA_EndDate: new Date(pl.ISMTPLTA_EndDate), ISMTPLTA_EffortInHrs: pl.ISMTPLTA_EffortInHrs, ISMMTCAT_Id: pl.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: pl.ISMMTCAT_TaskCategoryName, ISMMTCAT_TaskPercentage: pl.ISMMTCAT_TaskPercentage, ISMMTCAT_DurationFlg: pl.ISMMTCAT_DurationFlg, ISMMTCAT_CompulsoryFlg: pl.ISMMTCAT_CompulsoryFlg, ISMMTCAT_EachTaskMaxDuration: pl.ISMMTCAT_EachTaskMaxDuration, Flg: pl.ISMTPLTA_PreviousTask, cls: cls, status: status, ISMTPLTA_ExtraFlg: pl.ISMTPLTA_ExtraFlg, Periodicity: pl.Periodicity
                        });
                        $scope.plannerdetailsallx = $scope.get_plannerdetailsx;
                    });


                    $scope.categoryidlist1x = [];
                    var snocount = 0;
                    var countsubgrp = 0;
                    angular.forEach($scope.plannerdetailsallx, function (sub) {
                        if ($scope.categoryidlist1x.length === 0) {
                            snocount += 1;
                            $scope.categoryidlist1x.push({ ISMMTCAT_Id: sub.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: sub.ISMMTCAT_TaskCategoryName, ISMMTCAT_TaskPercentage: sub.ISMMTCAT_TaskPercentage, ISMMTCAT_DurationFlg: sub.ISMMTCAT_DurationFlg, ISMMTCAT_CompulsoryFlg: sub.ISMMTCAT_CompulsoryFlg, ISMMTCAT_EachTaskMaxDuration: sub.ISMMTCAT_EachTaskMaxDuration });
                        } else if ($scope.categoryidlist1x.length > 0) {
                            countsubgrp = 0;
                            angular.forEach($scope.categoryidlist1x, function (subgrp) {
                                if (subgrp.ISMMTCAT_Id === sub.ISMMTCAT_Id) {
                                    countsubgrp += 1;
                                }
                            });
                            if (countsubgrp === 0) {
                                snocount += 1;
                                $scope.categoryidlist1x.push({ ISMMTCAT_Id: sub.ISMMTCAT_Id, ISMMTCAT_TaskCategoryName: sub.ISMMTCAT_TaskCategoryName, ISMMTCAT_TaskPercentage: sub.ISMMTCAT_TaskPercentage, ISMMTCAT_DurationFlg: sub.ISMMTCAT_DurationFlg, ISMMTCAT_CompulsoryFlg: sub.ISMMTCAT_CompulsoryFlg, ISMMTCAT_EachTaskMaxDuration: sub.ISMMTCAT_EachTaskMaxDuration });
                            }
                        }
                    });



                    $scope.finalcategoryidlistx = [];


                    $scope.overalleffortCx = 0;
                    $scope.overalleffortCPx = 0;
                    $scope.overalleffortDx = 0;
                    $scope.overalleffortDPx = 0;

                    angular.forEach($scope.plannerdetailsallx, function (ww) {
                        if (ww.status != 'REJECTED') {
                            if (ww.Flg == true && ww.Flg != null) {
                                $scope.overalleffortDx = parseFloat($scope.overalleffortDx) + parseFloat(ww.ISMTPLTA_EffortInHrs)
                            }
                        }
                    });

                    if ($scope.overalleffortx > 0) {
                        $scope.overalleffortCPx = (($scope.overalleffortx - $scope.overalleffortDx) / $scope.overalleffortx) * 100;
                        $scope.overalleffortDPx = ($scope.overalleffortDx / $scope.overalleffortx) * 100;
                    }
                    angular.forEach($scope.categoryidlist1x, function (rr) {
                        var cateffortx = 0;
                        var devcateffortx = 0;
                        var curperx = 0;
                        var devperx = 0;
                        var actual = 0;
                        angular.forEach($scope.plannerdetailsallx, function (ff) {
                            if (ff.ISMMTCAT_Id != null && ff.ISMMTCAT_Id != undefined) {
                                if (parseInt(rr.ISMMTCAT_Id) == parseInt(ff.ISMMTCAT_Id)) {
                                    if (ff.status != 'REJECTED') {
                                        if (ff.Flg == true) {
                                            devcateffortx = devcateffortx + parseFloat(ff.ISMTPLTA_EffortInHrs);
                                        }
                                        if (ff.status != 'REJECTED') {
                                            cateffortx = cateffortx + parseFloat(ff.ISMTPLTA_EffortInHrs);
                                        }
                                    }
                                }
                            }
                        });

                        if (cateffortx != 0) {
                            curperx = (parseFloat(cateffortx - devcateffortx) / cateffortx) * 100;
                            devperx = (parseFloat(devcateffortx) / cateffortx) * 100;
                            actual = ((parseFloat(rr.ISMMTCAT_TaskPercentage) / 100) * parseFloat(48));
                            $scope.finalcategoryidlistx.push({
                                ISMMTCAT_Id: rr.ISMMTCAT_Id,
                                CNAME: rr.ISMMTCAT_TaskCategoryName,
                                MINP: rr.ISMMTCAT_TaskPercentage,
                                TE: cateffortx, DE: devcateffortx, CWP: curperx, DP: devperx, flg: rr.ISMMTCAT_CompulsoryFlg, actual: actual
                            });
                            $scope.finalcategory = [];
                            $scope.finalcategory = $scope.finalcategoryidlistx;
                        };
                        angular.forEach($scope.finalcategoryidlistx, function (dd) {
                            var cateff = dd.TE;
                            var hrscat = parseInt(Number(cateff));
                            var mincat = Math.round((Number(cateff) - hrscat) * 60);
                            if (hrscat.toString().length === 1) {
                                hrscat = '0' + hrscat;
                            }
                            if (mincat.toString().length === 1) {
                                mincat = '0' + mincat;
                            }
                            dd.effhrcat = hrscat;
                            dd.effmicat = mincat;

                            var cateffCCE = dd.TE - dd.DE;
                            var hrscatCCE = parseInt(Number(cateffCCE));
                            var mincatCCE = Math.round((Number(cateffCCE) - hrscatCCE) * 60);
                            if (hrscatCCE.toString().length === 1) {
                                hrscatCCE = '0' + hrscatCCE;
                            }
                            if (mincatCCE.toString().length === 1) {
                                mincatCCE = '0' + mincatCCE;
                            }
                            dd.effhrcatCCE = hrscatCCE;
                            dd.effmicatCCE = mincatCCE;

                            var catefffin = dd.DE;
                            var hrscatfin = parseInt(Number(catefffin));
                            var mincatfin = Math.round((Number(catefffin) - hrscatfin) * 60);
                            if (hrscatfin.toString().length === 1) {
                                hrscatfin = '0' + hrscatfin;
                            }
                            if (mincatfin.toString().length === 1) {
                                mincatfin = '0' + mincatfin;
                            }
                            dd.effhrcatfin = hrscatfin;
                            dd.effmicatfin = mincatfin;
                        });
                    });
                }
                else {
                    swal("No Record  Found..... !!");
                    $scope.get_plannerdetailsx = "";
                }
            });
        };
        //=========================
        $scope.getpdailyreportdetails = function (objp) {
            $scope.get_emp_dailyreport = [];
            $scope.arrayuserEmp = [];
            $scope.arrayuserEmp.push({ HRME_Id: $scope.HRME_Id1 });

            var data = {
                arrayuserEmp: $scope.arrayuserEmp,
                "Fromdate": new Date($scope.startdate1).toDateString(),
                "Todate": new Date($scope.enddate1).toDateString(),
                "feedbackflg": 1,

            };
            apiService.create("ISM_ApprovalReport/getempdetailsReport", data).then(function (promise) {
                $scope.plannerid = [];
                $scope.hide_n = false;
                $scope.feed = false;
                if (promise.get_emp_dailyreport.length > 0) {
                    $scope.get_emp_dailyreport = promise.get_emp_dailyreport;
                    angular.forEach($scope.get_emp_dailyreport, function (dev) {
                        if ($scope.plannerid.length === 0) {
                            $scope.plannerid.push({ ISMDRPT_Date: dev.ISMDRPT_Date });
                        }
                        else if ($scope.plannerid.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.plannerid, function (emp) {
                                if (emp.ISMDRPT_Date === dev.ISMDRPT_Date) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.plannerid.push({ ISMDRPT_Date: dev.ISMDRPT_Date });
                            }
                        }
                    });

                    angular.forEach($scope.plannerid, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.get_emp_dailyreport, function (dd) {
                            if (dd.ISMDRPT_Date === ddd.ISMDRPT_Date) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.plannerdetails = $scope.templist;
                        $scope.empname = $scope.templist[0].Employee_name;
                    });
                }

            });


        };
        //==============send feedback========
        $scope.Sendfeedback = function (aa) {
            $scope.feedbackdtl = [];
            $scope.plannerid1 = $scope.plannerid;
            angular.forEach($scope.plannerid, function (qq) {
                angular.forEach(qq.plannerdetails, function (ss) {
                    if (ss.feedbact !== undefined && ss.feedbact !== null && ss.feedbact !== "") {
                        $scope.feedbackdtl.push({
                            ISMDRF_RCV_HRME_Id: ss.HRME_Id, ISMTCR_Id: ss.ISMTCR_Id, ISMDRF_FeedBack: ss.feedbact, MI_Id_r: ss.MI_Id,
                            ISMDRF_Feedback_DR_Date: ss.ISMDRPT_Date
                        });
                    }
                });
            });

            if ($scope.feedbackdtl !== null && $scope.feedbackdtl.length > 0) {
                var data = {
                    feedbackdtl: $scope.feedbackdtl
                };

                apiService.create("ISM_ApprovalReport/send_feedback", data).then(function (fromise) {
                    if (fromise.send_flg === "Success") {
                        swal("Feedback Sent Successfully...");
                        $scope.plnrtemp = [];
                        $scope.templist = [];
                        $scope.plnrtemp = $scope.plannerid;

                        angular.forEach($scope.plannerid, function (ddd) {
                            $scope.templist = [];
                            angular.forEach($scope.get_emp_dailyreport, function (dd) {
                                if (dd.HRME_Id === ddd.HRME_Id) {
                                    $scope.templist.push({
                                        Employee_name: dd.Employee_name, HRME_Id: dd.HRME_Id, ISMDRPT_Remarks: dd.ISMDRPT_Remarks, ISMDRPT_Status: dd.ISMDRPT_Status, ISMTCR_Id: dd.ISMTCR_Id, ISMTCR_TaskNo: dd.ISMTCR_TaskNo, ISMTCR_Title: dd.ISMTCR_Title,
                                        ISMTPL_Id: dd.ISMTPL_Id, feedbact: "", ISMDRPT_Date: dd.ISMDRPT_Date
                                    });
                                }
                            });
                            ddd.plannerdetails = $scope.templist;

                        });
                    }
                    else {
                        swal("Feedback Not sent successfully...");
                        $scope.feedbact = '';
                    }
                });
            } else {
                swal("Enter Feedback");
            }
        };
        //===========Clear Field
        $scope.cancel = function () {
            $state.reload();
        };

    }
})();

