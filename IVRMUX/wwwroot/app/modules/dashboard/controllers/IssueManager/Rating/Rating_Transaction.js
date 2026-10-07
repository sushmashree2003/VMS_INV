(function () {
    'use strict';
    angular.module('app').controller('Rating_TransactionController', Rating_TransactionController)
    Rating_TransactionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function Rating_TransactionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {

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
        $scope.empflag = true;
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.submitted1 = false;
        $scope.objques = {};
        //$scope.maxdate = new Date();
        $scope.ratinggrid = false;
        // $scope.RATTR_FeedbackDate = new Date();

        //$scope.maxdate = new Date(currentDate.getFullYear(), currentDate.getMonth(), 0);


        var currentDate = new Date();
        // $scope.RATTR_FeedbackDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), 0); // Set to the last day of the previous month
        $scope.format = 'dd-MM-yyyy';
        $scope.datePickerOpened = false;

        $scope.maxdate = new Date(currentDate.getFullYear(), currentDate.getMonth(), 0);

        $scope.dateOptions = {
            startingDay: 1
        };

        $scope.openDatePicker = function () {
            $scope.datePickerOpened = true;
        };

        $scope.isDateDisabled = function (RATTR_FeedbackDate, mode) {
            // Enable only the last day of the previous month and disable other dates
            return mode === 'day' && !(RATTR_FeedbackDate.getMonth() === currentDate.getMonth() && RATTR_FeedbackDate.getDate() === 0);
        };

        $scope.OnLoad = function () {
            $scope.GetRatingMasterType = [];
            $scope.GetEmployeeDetails = [];
            $scope.objques.HRME_Id = "";
            $scope.submitted1 = false;
            $scope.GetTypeQuestions = [];
            $scope.RATMT_Id = "";
            var type = "Regular";

            var data = {
                "FlagOnLoadOrOnChangeInst": "OnLoad",
                "flag": type
            };
            apiService.create("Rating_Transaction/OnLoadRatingTransaction", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getuserinstitution = promise.getuserinstitution;
                    $scope.GetEmployeeDetails = promise.getEmployeeDetails;
                    $scope.GetSavedDataOnLoad = promise.getSavedDataOnLoad;
                    $scope.GetEmployeeUnratedDetails = promise.getEmployeeUnratedDetails;

                    $scope.MI_Id = promise.mI_Id;
                }
            });
        };

        $scope.UnratedEmployee = function () {
            if ($scope.GetEmployeeUnratedDetails.length > 0 && $scope.GetEmployeeUnratedDetails != null) {
                $scope.EmployeeUnratedDetails = $scope.GetEmployeeUnratedDetails;
            }
        }

        //$scope.onradiobuttonSelect = function () {
        //    $scope.GetRatingMasterType = [];
        //    $scope.GetEmployeeDetails = [];
        //    $scope.objques.HRME_Id = "";
        //    $scope.submitted1 = false;
        //    $scope.GetTypeQuestions = [];
        //    $scope.RATMT_Id = "";

        //    var type = "Regular";
        //    if ($scope.all1 == 'Left') {
        //         type = "Left";
        //    }
        //    else {
        //         type = "Regular";
        //    }

        //    var data = {
        //        "FlagOnLoadOrOnChangeInst": "OnChange",
        //        "flag": type
        //    };
        //    apiService.create("Rating_Transaction/OnLoadRatingTransaction", data).then(function (promise) {
        //        if (promise !== null) {
        //            $scope.getuserinstitution = promise.getuserinstitution;
        //            $scope.GetEmployeeDetails = promise.getEmployeeDetails;
        //            $scope.GetSavedDataOnLoad = promise.getSavedDataOnLoad;
        //            $scope.GetEmployeeUnratedDetails = promise.getEmployeeUnratedDetails;

        //            $scope.MI_Id = promise.mI_Id;
        //        }
        //    });
        //};



        $scope.OnChangeInsitution = function () {
            $scope.GetRatingMasterType = [];
            $scope.GetEmployeeDetails = [];
            $scope.GetTypeQuestions = [];
            $scope.objques.HRME_Id = "";
            $scope.RATMT_Id = "";
            $scope.submitted1 = false;
            var type = "Regular";
            if ($scope.all1 == "Left") {
                type = "Left";
            }
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnChange",
                "MI_Id": $scope.MI_Id,
                "flag": type
            };
            apiService.create("Rating_Transaction/OnLoadRatingTransaction", data).then(function (promise) {
                if (promise !== null) {
                    $scope.GetRatingMasterType = promise.getRatingMasterType;
                    $scope.GetEmployeeDetails = promise.getEmployeeDetails;
                }
            });
        };

        $scope.checkrating = function () {
            $scope.GetTypeQuestions = [];
            $scope.employeeprofile = [];
            $scope.GetEmployeeDetails = [];
            $scope.GetRatingMasterType = [];
            $scope.prioritylist = [];
            $scope.empflag = true;
            $scope.objques.HRME_Id = "";
            var type = "Regular";

            if ($scope.RATTR_FeedbackDate !== undefined) {
                $scope.empflag = false;
            }
            else {
                $scope.empflag = true;
            }

            var data = {
                "flag": $scope.all1,
                "RATTR_FeedbackDate": new Date($scope.RATTR_FeedbackDate).toDateString(),
            };

            apiService.create("Rating_Transaction/checkrating", data).then(function (promise) {

                if (promise.getEmployeeDetails.length > 0 && promise.getEmployeeDetails != null) {
                    $scope.GetEmployeeDetails = promise.getEmployeeDetails;
                }


            });
        }

        $scope.viewautomaticrating = function (questionflag) {
            $scope.LateinEarlyoutdetail = [];
            $scope.autoratingdetail_ul = [];
            $scope.autoratingdetail_deviation = [];
            $scope.auto_reopencount = [];
            $scope.auto_rejectedcount = [];
            $scope.auto_Overallrating = [];
            $scope.autoratingnoticecount = [];
            $scope.autoratingworkholiday = [];
            $scope.autoratingnonearned = [];
            $scope.autoratingemployee_deviation = [];
            $scope.autoratingdresscode = [];
            $scope.autoratingbehaviour = [];
            $scope.autoratingappearance = [];
            $scope.auto_Enhancementcount = [];
            $scope.auto_ProxyRolecount = [];
            $scope.questionflag = questionflag;
            var data = {
                "RATMQE_RatingsQRemarks": questionflag,
                "HRME_Id": $scope.objques.HRME_Id.hrmE_Id,
                "RATTR_FeedbackDate": new Date($scope.RATTR_FeedbackDate).toDateString(),
            }
            apiService.create("Rating_Transaction/viewautomaticrating", data).then(function (promise) {
                if (promise.autoratingdetail_lieo != null && promise.autoratingdetail_lieo.length > 0) {
                    $scope.LateinEarlyoutdetail = promise.autoratingdetail_lieo;
                    $scope.LateinEarlyoutmins = promise.autoratingdetail_lieo[0].LateinEarlyoutmins;
                    $('#autolateinearlyout').modal('show');
                }
                else if (promise.autoratingdetail_ul != null && promise.autoratingdetail_ul.length > 0) {
                    $scope.autoratingdetail_ul = promise.autoratingdetail_ul;
                    $scope.unouthorisedcount = promise.autoratingdetail_ul[0].UAleavecount;
                    $('#autolateinearlyout').modal('show');
                }
                else if (questionflag == "Notice") {
                    $scope.autoratingnoticecount = promise.autoratingdetail_deviation;
                    $scope.noticecount = $scope.autoratingnoticecount[0].NoticeCount;
                    $('#autolateinearlyout').modal('show');
                }
                else if (questionflag == "DressCode") {
                    $scope.autoratingdresscode = promise.autoratingdetail_deviation;
                    $scope.dresscode = $scope.autoratingdresscode[0].result;
                    $('#autolateinearlyout').modal('show');
                }
                else if (questionflag == "Appearance") {
                    $scope.autoratingappearance = promise.autoratingdetail_deviation;
                    $scope.ratingappearance = $scope.autoratingappearance[0].result;
                    $('#autolateinearlyout').modal('show');
                }
                else if (questionflag == "Behaviour") {
                    $scope.autoratingbehaviour = promise.autoratingdetail_deviation;
                    $scope.ratingbehaviour = $scope.autoratingbehaviour[0].result;
                    $('#autolateinearlyout').modal('show');
                }
                else if (questionflag == "Holiday") {
                    $scope.autoratingworkholiday = promise.autoratingdetail_deviation;
                    $scope.holidaycount = $scope.autoratingworkholiday[0].EmpHolidayWorkingDay;
                    $('#autolateinearlyout').modal('show');
                }
                else if (questionflag == "NonEarned") {
                    $scope.autoratingnonearned = promise.autoratingdetail_deviation;
                    $scope.nonearned = $scope.autoratingnonearned[0].nonearned;
                    $('#autolateinearlyout').modal('show');
                }
                else if (questionflag == "Deviate") {
                    $scope.autoratingdetail_deviation = promise.autoratingdetail_deviation;
                    $scope.deviationcount = promise.autoratingdetail_deviation[0].DevPer;
                    $('#autolateinearlyout').modal('show');
                }
                else if (questionflag == "Deviation") {
                    $scope.autoratingemployee_deviation = promise.autoratingdetail_deviation;
                    $scope.deviationofemployee = $scope.autoratingemployee_deviation[0].DeviationPer;
                    $('#autolateinearlyout').modal('show');
                }
                else if (questionflag == "Reopen") {
                    $scope.auto_reopencount = promise.autoratingdetail_deviation;
                    $scope.reopeoncount = $scope.auto_reopencount[0].reopencount;
                    $('#autolateinearlyout').modal('show');
                }
                else if (questionflag == "RejectedCount") {
                    $scope.auto_rejectedcount = promise.autoratingdetail_deviation;
                    $scope.rejectedcount = $scope.auto_rejectedcount[0].rejectedcount;
                    $('#autolateinearlyout').modal('show');
                }
                else if (questionflag == "Enhancement") {
                    $scope.auto_Enhancementcount = promise.autoratingdetail_deviation;
                    $scope.enhancementcount = $scope.auto_Enhancementcount[0].result;
                    $('#autolateinearlyout').modal('show');
                }
                else if (questionflag == "Proxy Role") {
                    $scope.auto_ProxyRolecount = promise.autoratingdetail_deviation;
                    $scope.proxyrolecount = $scope.auto_ProxyRolecount[0].result;
                    $('#autolateinearlyout').modal('show');
                }
                else if (questionflag == "OVERALLRATING") {
                    $scope.auto_Overallrating = promise.autoratingdetail_deviation;
                    $scope.overallrating = $scope.auto_Overallrating[0].overallrating;
                    $('#autolateinearlyout').modal('show');
                }
            });
        }

        $scope.onclearradiovalue = function () {
            $scope.RATTR_FeedbackDate = '';
            $scope.employeeprofile = [];
            $scope.GetEmployeeDetails = [];
            $scope.GetRatingMasterType = [];
            $scope.objques.HRME_Id = '';
            $scope.ratinggrid = false;
            $scope.GetRatingTypeTransaction = [];
            $scope.GetRatingTypedata = [];
        }

        $scope.OnChangeEmployee = function () {
            $scope.GetTypeQuestions = [];
            $scope.submitted1 = false;
            $scope.checkall123 = false;
            $scope.GetRatingTypeTransaction = [];
            $scope.GetRatingTypedata = [];
            $scope.GetRatingMasterType = [];
            $scope.GetTypeQuestions = [];
            $scope.RATMT_Id = "";
            $scope.prioritylist = [];

            var data = {

                "HRME_Id": $scope.objques.HRME_Id.hrmE_Id,
                "RATTR_FeedbackDate": new Date($scope.RATTR_FeedbackDate).toDateString(),
            };

            apiService.create("Rating_Transaction/OnChangeEmployee", data).then(function (promise) {
                //$scope.GetRatingMasterType = promise.getRatingMasterType;

                if (promise.employeeprofile != null && promise.employeeprofile.length > 0 && promise.employeeprofile != undefined) {

                    $scope.employeeprofile = promise.employeeprofile;

                    $scope.Department = promise.employeeprofile[0].HRMD_DepartmentName;
                    $scope.Designation = promise.employeeprofile[0].HRMDES_DesignationName;
                    $scope.mI_Name = promise.employeeprofile[0].MI_Name;
                    $scope.employeename = promise.employeeprofile[0].employeename;
                    //$scope.ismresG_RegRelFlag = promise.employeeprofile[0].ismresG_RegRelFlag;
                    // $scope.hrmE_Id = promise.employeeprofile[0].hrmE_Id;
                    $scope.HRME_LeftFlag = promise.employeeprofile[0].HRME_LeftFlag;
                    $scope.HRME_DOL = promise.employeeprofile[0].HRME_DOL;
                    $scope.DateOfJoining = promise.employeeprofile[0].HRME_DOJ;
                    $scope.HRME_LeavingReason = promise.employeeprofile[0].HRME_LeavingReason;
                    $scope.HRME_Photo = promise.employeeprofile[0].HRME_Photo;
                }
                if (promise.getRatingMasterType.length > 0 && promise.getRatingMasterType != null) {
                    $scope.GetRatingMasterType = promise.getRatingMasterType;
                }


                if (promise.prioritylist.length > 0 && promise.prioritylist != null) {
                    $scope.prioritylist = promise.prioritylist;
                }

            });
        };

        $scope.OnChangeDate = function () {
            $scope.GetTypeQuestions = [];
            $scope.submitted1 = false;
        };

        $scope.GetSearchDetails = function () {
            $scope.GetTypeQuestions = [];
            $scope.ratinggrid = true;
            $scope.ratingtypelist = [];
            angular.forEach($scope.GetRatingMasterType, function (em) {
                if (em.selected === true) {
                    $scope.ratingtypelist.push({ RATMT_Id: em.ratmT_Id });
                }
            });


            if ($scope.myForm1.$valid) {
                var data = {
                    "flag": $scope.all1,
                    "ratingtypelist": $scope.ratingtypelist,
                    "RATTR_FeedbackDate": new Date($scope.RATTR_FeedbackDate).toDateString(),
                    "HRME_Id": $scope.objques.HRME_Id.hrmE_Id
                };

                apiService.create("Rating_Transaction/GetSearchDetails", data).then(function (promise) {
                    if (promise !== null) {

                        $scope.GetRatingTypedata = promise.getRatingTypedata;
                        $scope.GetTypeClassificationQuestions = promise.getTypeClassificationQuestions;
                        $scope.GetTypeSubClassificationQuestions = promise.getTypeSubClassificationQuestions;

                        if (promise.getTypeQuestionTransaction !== undefined && promise.getTypeQuestionTransaction !== null && promise.getTypeQuestionTransaction.length > 0) {

                            $scope.GetRatingTypeTransaction = promise.getRatingTypeTransaction;
                            $scope.GetTypeQuestionTransaction = promise.getTypeQuestionTransaction;
                            $scope.GetTypeQuestionOptionTransaction = promise.getTypeQuestionOptionTransaction;
                            $scope.GetRatingClassificationTransaction = promise.getRatingClassificationTransaction;
                            $scope.GetRatingSubClassificationTransaction = promise.getRatingSubClassificationTransaction;

                            $scope.TempOptionauto = [];
                            $scope.TempGetTypeQuestionauto = [];
                            $scope.TempGetClassificationQuestionauto = [];

                            angular.forEach($scope.GetRatingTypeTransaction, function (rtt) {
                                $scope.TempGetClassificationQuestionauto = [];
                                angular.forEach($scope.GetRatingClassificationTransaction, function (rqq) {
                                    if (rtt.ratmT_Id == rqq.ratmT_Id) {
                                        $scope.TempGetClassificationQuestionauto.push(rqq)
                                    }
                                });

                                rtt.questionclassificationdata = $scope.TempGetClassificationQuestionauto;


                                angular.forEach(rtt.questionclassificationdata, function (dd) {
                                    $scope.Tempquestionsubclassification = [];
                                    angular.forEach($scope.GetRatingSubClassificationTransaction, function (d) {
                                        if (dd.ratmqecL_Id === d.ratmqecL_Id) {
                                            $scope.Tempquestionsubclassification.push(d);
                                        }
                                    });

                                    dd.questionsubclassificationdata = $scope.Tempquestionsubclassification;

                                    angular.forEach(dd.questionsubclassificationdata, function (dd) {
                                        $scope.Tempquestiodata = [];
                                        angular.forEach($scope.GetTypeQuestionTransaction, function (d) {
                                            if (dd.ratmqeclsuB_Id === d.RATMQECLSUB_Id) {
                                                $scope.Tempquestiodata.push(d);
                                            }
                                        });

                                        dd.questionlistdata = $scope.Tempquestiodata;


                                        angular.forEach(dd.questionlistdata, function (dd) {
                                            // dd.RATTR_Id = 0;
                                            $scope.TempOptionauto = [];
                                            if (dd.ratmqE_ManualEntryFlg === false) {
                                                angular.forEach($scope.GetTypeQuestionOptionTransaction, function (d) {
                                                    if (dd.ratmqE_Id === d.ratmqE_Id) {

                                                        if (d.ratmoP_OptionsValue != null && d.ratmoP_OptionsValue != 0) {
                                                            if (d.ratmoP_Id == dd.ratmoP_Id) {
                                                                $scope.TempOptionauto.push({
                                                                    name: true, ratmoP_RatingOptions: d.ratmoP_RatingOptions, ratmoP_OptionsValue: d.ratmoP_OptionsValue,
                                                                    ratmoP_ROOrder: d.ratmoP_ROOrder, ratmoP_Id: d.ratmoP_Id, ratmqE_Id: d.ratmqE_Id
                                                                });
                                                            }
                                                            else {
                                                                $scope.TempOptionauto.push({
                                                                    name: false, ratmoP_RatingOptions: d.ratmoP_RatingOptions, ratmoP_OptionsValue: d.ratmoP_OptionsValue,
                                                                    ratmoP_ROOrder: d.ratmoP_ROOrder, ratmoP_Id: d.ratmoP_Id, ratmqE_Id: d.ratmqE_Id
                                                                });
                                                            }

                                                        }

                                                    }
                                                });
                                            }
                                            dd.optionslistdata = $scope.TempOptionauto;
                                        });

                                    });
                                });

                            });


                        }


                        if (promise.getTypeQuestions !== undefined && promise.getTypeQuestions !== null && promise.getTypeQuestions.length > 0) {

                            $scope.GetTypeQuestions = promise.getTypeQuestions;
                            $scope.GetTypeQuestionOptions = promise.getTypeQuestionOptions;

                            $scope.TempOptions = [];
                            $scope.TempGetTypeQuestions = [];

                            angular.forEach($scope.GetRatingTypedata, function (tt) {
                                $scope.TempClassificationQuestions = [];
                                angular.forEach($scope.GetTypeClassificationQuestions, function (qq) {
                                    if (tt.ratmT_Id == qq.ratmT_Id) {
                                        $scope.TempClassificationQuestions.push(qq)
                                    }
                                });
                                tt.classificationlist = $scope.TempClassificationQuestions;

                                angular.forEach(tt.classificationlist, function (ttt) {
                                    $scope.TempSubClassificationQuestions = [];
                                    angular.forEach($scope.GetTypeSubClassificationQuestions, function (qq) {
                                        if (ttt.ratmqecL_Id == qq.ratmqecL_Id) {
                                            $scope.TempSubClassificationQuestions.push(qq)
                                        }
                                    });
                                    ttt.subclassificationlist = $scope.TempSubClassificationQuestions;

                                    angular.forEach(ttt.subclassificationlist, function (scl) {
                                        $scope.TempGetTypeQuestions = [];
                                        angular.forEach($scope.GetTypeQuestions, function (qq) {
                                            if (scl.ratmqeclsuB_Id == qq.ratmqeclsuB_Id) {
                                                $scope.TempGetTypeQuestions.push(qq)
                                            }
                                        });
                                        scl.questionlist = $scope.TempGetTypeQuestions;

                                        angular.forEach(scl.questionlist, function (dd) {
                                            dd.RATTR_Id = 0;
                                            $scope.TempOptions = [];
                                            if (dd.ratmqE_ManualEntryFlg === false) {
                                                angular.forEach($scope.GetTypeQuestionOptions, function (d) {
                                                    if (dd.ratmqE_Id === d.ratmqE_Id) {
                                                        $scope.TempOptions.push(d);
                                                    }
                                                });
                                            }
                                            dd.optionslist = $scope.TempOptions;
                                        });
                                    });
                                });

                            });


                            $scope.GetSavedDetails = promise.getSavedDetails;

                            angular.forEach($scope.GetTypeQuestions, function (dd) {
                                angular.forEach($scope.GetSavedDetails, function (d) {
                                    if (dd.ratmqE_Id === d.ratmqE_Id) {
                                        dd.name = dd.ratmqE_ManualEntryFlg === true ? d.rattR_FeedBack : d.ratmoP_Id;
                                        dd.RATTR_Id = d.rattR_Id;
                                    }
                                });
                            });

                            console.log($scope.GetTypeQuestions);
                            console.log($scope.GetTypeQuestionOptions);
                        }
                    }
                    else {
                        swal("No Questions Mapped For Selected Type , Department And Designation");
                    }
                });
            } else {
                $scope.submitted1 = true;
            }
        };

        $scope.SaveRating = function () {
            if ($scope.myForm1.$valid) {
                $scope.TempsaveDetails = [];

                $scope.ratingtypelist = [];
                angular.forEach($scope.GetRatingMasterType, function (em) {
                    if (em.selected === true) {
                        $scope.ratingtypelist.push({ RATMT_Id: em.ratmT_Id });
                    }
                });

                var data = {
                    "ratingtypelist": $scope.ratingtypelist,
                    "HRME_Id": $scope.objques.HRME_Id.hrmE_Id,
                    "RATTR_FeedbackDate": new Date($scope.RATTR_FeedbackDate).toDateString(),
                    "Save_Rating_Details": $scope.GetRatingTypedata,
                    "Save_AutoRating_Details": $scope.GetRatingTypeTransaction,
                    "MI_Id": $scope.MI_Id
                };
                apiService.create("Rating_Transaction/SaveRating", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Record Saved/Updated Successfully");
                        }
                        else {
                            swal("Failed Saved/Updated Record");
                        }
                        $state.reload();
                    }
                });
            } else {
                $scope.submitted1 = true;
            }
        };


        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.clear_first_tab = function () {
            $state.reload();
        };

        $scope.viewratinglist = function (item) {
            var data = {
                "HRME_Id": item.HRME_Id,
                "RATMT_Id": item.RATMT_Id,
                "RATTR_FeedbackDate": new Date(item.RATTR_FeedbackDate).toDateString(),
            }
            apiService.create("Rating_Transaction/viewratinglist", data).then(function (promise) {

                if (promise.viewEmployeeRating != null && promise.viewEmployeeRating.length > 0) {
                    $scope.viewEmployeeRating = promise.viewEmployeeRating;
                    $scope.ratingtypeid = [];
                    angular.forEach($scope.viewEmployeeRating, function (dev) {
                        if ($scope.ratingtypeid.length === 0) {
                            $scope.ratingtypeid.push({
                                RATMT_Id: dev.RATMT_Id,
                                RATMT_RatingTypeName: dev.RATMT_RatingTypeName,
                            });
                        } else if ($scope.ratingtypeid.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.ratingtypeid, function (emp) {
                                if (emp.RATMT_Id === dev.RATMT_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.ratingtypeid.push({
                                    RATMT_Id: dev.RATMT_Id,
                                    RATMT_RatingTypeName: dev.RATMT_RatingTypeName,
                                });
                            }
                        }
                    });

                    angular.forEach($scope.ratingtypeid, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.viewEmployeeRating, function (dd) {
                            if (dd.RATMT_Id === ddd.RATMT_Id) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.Ratingdetails = $scope.templist;
                    });

                    $("#viewrating").modal('show');

                }
                else {
                    swal("No Record Found");
                    $("#viewrating").modal('hide');
                }


            });
        }


        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.rattR_FeedbackDate)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.MI_Name)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrmE_EmployeeFirstName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.ratmT_RatingTypeName)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        };



        $scope.all_check123 = function () {            var checkStatus = $scope.checkall123;            var count = 0;            angular.forEach($scope.GetRatingMasterType, function (itm) {                itm.selected = checkStatus;                if (itm.selected == true) {                    count += 1;                }                else {                    count = 0;                }            });        }        $scope.isOptionsRequired1223 = function () {            return !$scope.GetRatingMasterType.some(function (options) {                return options.selected;            });        }        $scope.togchkbx123 = function () {            $scope.checkall123 = $scope.GetRatingMasterType.every(function (options) {                return options.selected;            });        }



    }
})();