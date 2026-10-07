(function () {
    'use strict';
    angular.module('app').controller('Rating_DressCodeController', Rating_DressCodeController)
    Rating_DressCodeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function Rating_DressCodeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {

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

        //$scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.submitted = false;
        $scope.submitted1 = false;
        $scope.submitted2 = false;

        $scope.ratinggrid = false;
        $scope.empflag = true;
        $scope.objques = {};



        // Rating DressCode
        $scope.OnLoadRatingDressCode = function () {

            $scope.searchmastertype = "";
            $scope.selectedDate = "";
            $scope.getuserinstitution = [];
            $scope.GetEmployeeDetails = [];
            $scope.GetTypeQuestions = [];
            $scope.EditFlagMasterFlag = false;
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnLoad"
            };
            apiService.create("Rating_Transaction/OnLoadRatingDressCode", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getuserinstitution = promise.getuserinstitution;
                    $scope.DresscodeMI_Id = promise.mI_Id;
                    $scope.GetTypeQuestions = promise.getTypeQuestions;
                    $scope.getDresscodetransaction = promise.getDresscodetransaction;
                }
            });
        };

        $scope.OnChangeMasterInsitutionApperence = function () {

            $scope.GetEmployeeDetails = [];
            $scope.GetTypeQuestions = [];
            $scope.RATTR_FeedbackDate = "";
        };


        $scope.checkSelectedDate = function () {

            $scope.employeeprofile = [];
            $scope.GetEmployeeDetails = [];

            var day = $scope.selectedDate.getDay(); // Get the selected day of the week (0-6, where 0 is Sunday)
            var dayOfMonth = $scope.selectedDate.getDate();

            if (day === 1 || (day === 6 && dayOfMonth > 7 && dayOfMonth < 15)) { // If the selected day is not Monday (1)

                var data = {
                    "MI_Id": $scope.DresscodeMI_Id,
                    "RATTR_FeedbackDate": new Date($scope.selectedDate).toDateString(),
                };

                apiService.create("Rating_Transaction/checkratingDresscode", data).then(function (promise) {

                    if (promise.getEmployeeDetails.length > 0 && promise.getEmployeeDetails != null) {
                        $scope.GetEmployeeDetails = promise.getEmployeeDetails;
                    }

                });
            }
            else if (day !== 1 || (day !== 6 && dayOfMonth > 7 && dayOfMonth < 15)) { // If the selected day is not Monday (1)
                $scope.selectedDate = null; // Clear the selected date
                swal("Select Date Only Mondays and Second Saturday");
            }
        };


        $scope.OnChangeMasterInsitution = function () {
            $scope.EditFlagMasterFlag = false;
            $scope.GetTypeQuestions = [];
            $scope.GetEmployeeDetails = [];
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnChange",
                "MI_Id": $scope.DresscodeMI_Id
            };
            apiService.create("Rating_Transaction/OnLoadRatingDressCode", data).then(function (promise) {
                if (promise !== null) {
                    $scope.GetTypeQuestions = promise.getTypeQuestions;
                    $scope.getDresscodetransaction = promise.getDresscodetransaction;
                }
            });
        };

        $scope.SaveRatingDresscode = function () {
            $scope.employeelist = [];

            angular.forEach($scope.GetEmployeeDetails, function (em) {
                if (em.selected === true) {
                    $scope.employeelist.push({ HRME_Id: em.hrmE_Id });
                }
            });

            if ($scope.myForm3.$valid) {
               if ($scope.employeelist.length > 0) {
                var data = {
                    "RATMT_Id": $scope.GetTypeQuestions[0].ratmT_Id,
                    "RATMQE_Id": $scope.GetTypeQuestions[0].ratmqE_Id,
                    "RATTR_FeedbackDate": new Date($scope.selectedDate).toDateString(),
                    "employeedata": $scope.employeelist,
                    "MI_Id": $scope.DresscodeMI_Id
                };
                apiService.create("Rating_Transaction/SaveRatingDresscode", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Record Saved/Updated Successfully");
                        }
                        else {
                            swal("Failed Saved/Updated Record");
                        }
                        $scope.OnLoadRatingDressCode();
                    }
                });
               }
               else {
                    swal("Please select atleast one checkbox...");
               }

            } else {
                $scope.submitted = true;
            }
        };

        $scope.interacted3 = function (field) {
            return $scope.submitted;
        };

        $scope.clear_first_tab = function () {
            //$scope.OnChangeMasterInsitution();
            $state.reload();
        };

        // Master Appearance 

        $scope.OnClickRatingAppearance = function () {
            $scope.Ratingclassification_MI_Id = "";
            $scope.objques.HRME_Id = "";
            $scope.getuserinstitution = [];
            $scope.employeeprofile = [];
            $scope.GetEmployeeDetails = [];
            $scope.GetTypeQuestions = [];
            $scope.RATTR_FeedbackDate = "";
            $scope.search6 = "";
            $scope.submitted2 = false;
            $scope.EditFlagMasterquestion = false;
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnLoad"
            };
            apiService.create("Rating_Transaction/OnClickRatingAppearance", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getuserinstitution = promise.getuserinstitution;
                    $scope.Master_Type_AppearanceMI_Id = promise.mI_Id;
                    $scope.getAppearancetransaction = promise.getAppearancetransaction;
                }
            });

        };

        $scope.toggleAll = function (allcheck) {

            var toggleStatus = allcheck;
            if (toggleStatus == true) {
                angular.forEach($scope.GetEmployeeDetails, function (itm) {
                    itm.isSelected = true;
                });
            }
            else {
                angular.forEach($scope.GetEmployeeDetails, function (itm) {
                    itm.isSelected = false;
                });
            }

        }


        $scope.amtdetails = function () {

            $scope.objques.allch = $scope.GetEmployeeDetails.every(function (itm) {
                return itm.isSelected;
            });

        };

        $scope.checkratingAppearance = function () {
            $scope.GetTypeQuestions = [];
            $scope.employeeprofile = [];
            $scope.GetEmployeeDetails = [];
            $scope.GetTypeQuestionOptions = [];
            $scope.ratinggrid = true;
            $scope.empflag = true;
            $scope.objques.allch = false;
            $scope.objques.HRME_Id = "";
            var type = "Regular";

            if ($scope.RATTR_FeedbackDate !== undefined) {
                $scope.empflag = false;
            }
            else {
                $scope.empflag = true;
            }

            var data = {
                "MI_Id": $scope.Master_Type_AppearanceMI_Id,
                "RATTR_FeedbackDate": new Date($scope.RATTR_FeedbackDate).toDateString(),
            };

            apiService.create("Rating_Transaction/checkratingAppearance", data).then(function (promise) {

                if (promise.getEmployeeDetails.length > 0 && promise.getEmployeeDetails != null) {
                    $scope.GetEmployeeDetails = promise.getEmployeeDetails;
                }
                if (promise.getTypeQuestions !== undefined && promise.getTypeQuestions !== null && promise.getTypeQuestions.length > 0) {

                    $scope.GetTypeQuestions = promise.getTypeQuestions;
                    $scope.GetTypeQuestionOptions = promise.getTypeQuestionOptions;
                }

                if ($scope.GetEmployeeDetails.length > 0 && $scope.GetTypeQuestionOptions.length > 0) {

                    angular.forEach($scope.GetEmployeeDetails, function (dd) {
                        $scope.TempOptions = [];
                        angular.forEach($scope.GetTypeQuestionOptions, function (d) {
                            if (dd.ratmqE_Id === d.ratmqE_Id) {
                                $scope.TempOptions.push(d);
                            }
                        });
                        dd.optionslist = $scope.TempOptions;
                    });
                }

            });
        }

        $scope.OnChangeRatingClassificationInstitution = function () {
            $scope.RATMQECL_QuestionsClassification = "";
            $scope.search6 = "";
            $scope.submitted2 = false;

            $scope.getRatingTypeData = [];

            $scope.RATMQECL_Id = 0;
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnChange",
                "MI_Id": $scope.Ratingclassification_MI_Id
            };
            apiService.create("Rating_Master/OnClickRatingClassification", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getRatingTypeData = promise.getRatingTypeData;
                }
            });
        };

        $scope.SaveRatingAppearance = function () {
            var temparr = [];
            if ($scope.myForm2.$valid) {
                angular.forEach($scope.GetEmployeeDetails, function (ty) {
                    if (ty.isSelected == true) {
                        temparr.push({ RATMT_Id: $scope.GetTypeQuestions[0].ratmT_Id,RATMQE_Id: ty.ratmqE_Id, HRME_Id: ty.hrmE_Id, name: ty.name })
                    }

                })
                if (temparr.length > 0) {
                var data = {
                    "RATTR_FeedbackDate": new Date($scope.RATTR_FeedbackDate).toDateString(),
                    "Save_Behaviour_Rating_Detail": temparr,
                    "MI_Id": $scope.Master_Type_AppearanceMI_Id
                };
                apiService.create("Rating_Transaction/SaveBehaviourRating", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Record Saved/Updated Successfully");
                        }
                        else {
                            swal("Failed Saved/Updated Record");
                        }
                        $scope.OnClickRatingAppearance();
                    }
                    });
                }
                else {
                    swal("Please select Atleast One Check Box...");
                    // $state.reload();
                }
            } else {
                $scope.submitted2 = true;
            }
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };

        $scope.clear_second_tab = function () {
           // $scope.OnClickRatingAppearance();
            $state.reload();
        };

        // Master Behaviours

        $scope.OnClickRatingBehaviour = function () {
            $scope.Master_Appearance_MI_Id = "";
            $scope.objques.HRME_Id = "";
            $scope.getuserinstitution = [];
            $scope.employeeprofile = [];
            $scope.GetEmployeeDetails = [];
            $scope.GetTypeQuestions = [];
            $scope.RATTR_FeedbackDate = "";
            $scope.search6 = "";
            $scope.submitted1 = false;
            $scope.EditFlagMasterquestion = false;
            $scope.objques.allch = false;

            $scope.RATMQECL_Id = 0;
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnLoad"
            };
            apiService.create("Rating_Transaction/OnClickRatingBehaviour", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getuserinstitution = promise.getuserinstitution;
                    $scope.Master_Behaviour_MI_Id = promise.mI_Id;
                    $scope.GetEmployeeDetails = promise.getEmployeeDetails;
                    $scope.getbehaviourtransaction = promise.getbehaviourtransaction;
                }
            });

        };



        $scope.OnChangeBehaviourInsitution = function () {
            $scope.RATMQECL_QuestionsClassification = "";
            $scope.search7 = "";
            $scope.submitted1 = false;

            $scope.getQuestionClassificationData = [];

            $scope.RATMQECLSUB_Id = 0;
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnChange",
                "MI_Id": $scope.Master_Behaviour_MI_Id
            };
            apiService.create("Rating_Master/OnClickRatingBehaviour", data).then(function (promise) {
                if (promise !== null) {

                    $scope.GetEmployeeDetails = promise.getEmployeeDetails;
                }
            });
        };

        $scope.checkratingbehaviour = function () {
            $scope.GetTypeQuestions = [];
            $scope.employeeprofile = [];
            $scope.GetEmployeeDetails = [];
            $scope.GetTypeQuestionOptions = [];
            $scope.ratinggrid = true;
            $scope.empflag = true;
            $scope.objques.HRME_Id = "";
            $scope.objques.allch = false;
            var type = "Regular";

            if ($scope.RATTR_FeedbackDate !== undefined) {
                $scope.empflag = false;
            }
            else {
                $scope.empflag = true;
            }

            var data = {
                "MI_Id": $scope.Master_Behaviour_MI_Id,
                "RATTR_FeedbackDate": new Date($scope.RATTR_FeedbackDate).toDateString(),
            };

            apiService.create("Rating_Transaction/checkratingbehaviour", data).then(function (promise) {

                if (promise.getEmployeeDetails.length > 0 && promise.getEmployeeDetails != null) {
                    $scope.GetEmployeeDetails = promise.getEmployeeDetails;
                }

                if (promise.getTypeQuestions !== undefined && promise.getTypeQuestions !== null && promise.getTypeQuestions.length > 0) {

                    $scope.GetTypeQuestions = promise.getTypeQuestions;
                    $scope.GetTypeQuestionOptions = promise.getTypeQuestionOptions;
                }

                if ($scope.GetEmployeeDetails.length > 0 && $scope.GetTypeQuestionOptions.length > 0) {

                    angular.forEach($scope.GetEmployeeDetails, function (dd) {
                        $scope.TempOptions = [];
                        angular.forEach($scope.GetTypeQuestionOptions, function (d) {
                            if (dd.ratmqE_Id === d.ratmqE_Id) {
                                $scope.TempOptions.push(d);
                            }
                        });
                        dd.optionslist = $scope.TempOptions;
                    });
                }

            });
        }

        $scope.OnChangeBehaviourEmployee = function (flag) {
            $scope.GetTypeQuestions = [];
            $scope.ratinggrid = true;
            $scope.submitted1 = false;
            $scope.GetRatingTypeTransaction = [];
            $scope.GetRatingTypedata = [];
            $scope.GetRatingMasterType = [];
            $scope.GetTypeQuestions = [];
            $scope.GetTypeQuestionOptions = [];

            $scope.RATMT_Id = "";
            var data = {

                "HRME_Id": $scope.objques.HRME_Id.hrmE_Id,
                "RATTR_FeedbackDate": new Date($scope.RATTR_FeedbackDate).toDateString(),
                "flag": flag
            };

            apiService.create("Rating_Transaction/OnChangeBehaviourEmployee", data).then(function (promise) {


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


                //if (promise.prioritylist.length > 0 && promise.prioritylist != null) {
                //    $scope.prioritylist = promise.prioritylist;
                //}

                if (promise.getTypeQuestions !== undefined && promise.getTypeQuestions !== null && promise.getTypeQuestions.length > 0) {

                    $scope.GetTypeQuestions = promise.getTypeQuestions;
                    $scope.GetTypeQuestionOptions = promise.getTypeQuestionOptions;

                    $scope.TempOptions = [];
                    $scope.TempGetTypeQuestions = [];

                    angular.forEach($scope.GetTypeQuestions, function (dd) {
                        $scope.TempOptions = [];
                        angular.forEach($scope.GetTypeQuestionOptions, function (d) {
                            if (dd.ratmqE_Id === d.ratmqE_Id) {
                                $scope.TempOptions.push(d);
                            }
                        });
                        dd.optionslist = $scope.TempOptions;
                    });



                }


            });
        };


        $scope.SaveRating = function () {
            var temparr = [];
            if ($scope.myForm1.$valid) {
                angular.forEach($scope.GetEmployeeDetails, function (ty) {
                    if (ty.isSelected == true) {
                        temparr.push({ RATMT_Id: $scope.GetTypeQuestions[0].ratmT_Id, RATMQE_Id: ty.ratmqE_Id, HRME_Id: ty.hrmE_Id, name: ty.name })
                    }

                })
                if (temparr.length > 0) {
                var data = {
                    "RATTR_FeedbackDate": new Date($scope.RATTR_FeedbackDate).toDateString(),
                    "Save_Behaviour_Rating_Detail": temparr,
                    "MI_Id": $scope.Master_Behaviour_MI_Id
                };
                apiService.create("Rating_Transaction/SaveBehaviourRating", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Record Saved/Updated Successfully");
                        }
                        else {
                            swal("Failed Saved/Updated Record");
                        }
                        $scope.OnClickRatingBehaviour();
                    }
                    });
                }
                else {
                    swal("Please select atleast One check Box...");
                }
            } else {
                $scope.submitted1 = true;
            }
        };


        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.clear_third_tab = function () {
           // $scope.OnClickRatingBehaviour();
            $state.reload();
        };

        $scope.isOptionsRequireddesignation = function () {
            return !$scope.getDesginationData.some(function (item) {
                return item.selected;
            });
        };


    }
})();
