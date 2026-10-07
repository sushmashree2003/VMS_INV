(function () {
    'use strict';
    angular.module('app').controller('Rating_MasterController', Rating_MasterController)
    Rating_MasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function Rating_MasterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {

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
        $scope.objques = {};

        // Master Type
        $scope.GetMasterQuestionDatalist = [];
        $scope.OnLoadRatingMasterType = function () {
            $scope.RATMT_RatingTypeName = "";
            $scope.RATMT_PeriodicityFlg = "";
            $scope.Master_Type_MI_Id = "";
            $scope.RATMT_DepartmentwiseFlg = false;
            $scope.RATMT_QuestionwiseOptionFlg = false;
            $scope.searchmastertype = "";
            $scope.getuserinstitution = [];
            $scope.GetMasterTypeLoadData = [];
            $scope.RATMT_Id = 0;
            $scope.EditFlagMasterFlag = false;
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnLoad"
            };
            apiService.create("Rating_Master/OnLoadRatingMasterType", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getuserinstitution = promise.getuserinstitution;
                    $scope.Master_Type_MI_Id = promise.mI_Id;
                    $scope.GetMasterTypeLoadData = promise.getMasterTypeLoadData;
                }
            });
        };

        $scope.OnChangeMasteTypeInsitution = function () {
            $scope.RATMT_RatingTypeName = "";
            $scope.RATMT_PeriodicityFlg = "";
            $scope.RATMT_DepartmentwiseFlg = false;
            $scope.RATMT_QuestionwiseOptionFlg = false;
            $scope.searchmastertype = "";
            $scope.GetMasterTypeLoadData = [];
            $scope.RATMT_LInkedToRelievingFlg = false;
            $scope.EditFlagMasterFlag = false;
            $scope.RATMT_Id = 0;
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnChange",
                "MI_Id": $scope.Master_Type_MI_Id
            };
            apiService.create("Rating_Master/OnLoadRatingMasterType", data).then(function (promise) {
                if (promise !== null) {
                    $scope.GetMasterTypeLoadData = promise.getMasterTypeLoadData;
                }
            });
        };

        $scope.SaveMasterType = function () {
            if ($scope.myForm1.$valid) {
                var data = {
                    "RATMT_RatingTypeName": $scope.RATMT_RatingTypeName,
                    "RATMT_PeriodicityFlg": $scope.RATMT_PeriodicityFlg,
                    "RATMT_DepartmentwiseFlg": $scope.RATMT_DepartmentwiseFlg,
                    "RATMT_QuestionwiseOptionFlg": $scope.RATMT_QuestionwiseOptionFlg,
                    "RATMT_Id": $scope.RATMT_Id,
                    "MI_Id": $scope.Master_Type_MI_Id,
                    "RATMT_LInkedToRelievingFlg": $scope.RATMT_LInkedToRelievingFlg
                };

                apiService.create("Rating_Master/SaveMasterType", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnmsg == "Duplicate") {
                            swal("Record Already Exists");
                        } else if (promise.returnmsg === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.returnmsg === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else {
                            swal("Failed To Update/Save Record");
                        }
                        $scope.OnChangeMasteTypeInsitution();
                    }
                });
            } else {
                $scope.submitted1 = true;
            }
        };

        $scope.EditMasterType = function (obj) {
            var data = {
                "RATMT_Id": obj.ratmT_Id,
                "MI_Id": $scope.Master_Type_MI_Id
            };

            apiService.create("Rating_Master/EditMasterType", data).then(function (promise) {
                if (promise !== null) {

                    if (promise.checkRatingTypeMapped !== null && promise.checkRatingTypeMapped.length > 0) {
                        swal("Record Already Mapped To Employee So You Can Not Edit This Record");
                    } else {
                        $scope.EditFlagMasterFlag = true;
                        $scope.GetMaterTypeEditData = promise.getMaterTypeEditData;
                        $scope.RATMT_RatingTypeName = $scope.GetMaterTypeEditData[0].ratmT_RatingTypeName;
                        $scope.RATMT_DepartmentwiseFlg = $scope.GetMaterTypeEditData[0].ratmT_DepartmentwiseFlg;
                        $scope.RATMT_PeriodicityFlg = $scope.GetMaterTypeEditData[0].ratmT_PeriodicityFlg;
                        $scope.RATMT_QuestionwiseOptionFlg = $scope.GetMaterTypeEditData[0].ratmT_QuestionwiseOptionFlg;
                        $scope.RATMT_Id = $scope.GetMaterTypeEditData[0].ratmT_Id;
                        $scope.RATMT_LInkedToRelievingFlg = $scope.GetMaterTypeEditData[0].ratmT_LInkedToRelievingFlg;
                    }
                }
            });
        };

        $scope.MastetTypeActiveDeactive = function (obj) {

            var mgs = "";
            var confirmmgs = "";
            if (obj.ratmT_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }

            var data = {
                "RATMT_Id": obj.ratmT_Id,
                "MI_Id": $scope.Master_Type_MI_Id
            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Rating_Master/MastetTypeActiveDeactive", data).then(function (promise) {
                            if (promise.returnmsg === "Mapped") {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                            }
                            $scope.OnChangeMasteTypeInsitution();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.MastetTypeQuestionActiveDeactive = function (obj) {

            var mgs = "";
            var confirmmgs = "";
            if (obj.ratmT_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }

            var data = {
                "RATMT_Id": obj.ratmT_Id,
                "MI_Id": $scope.Master_Type_MI_Id
            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Rating_Master/MastetTypeQuestionActiveDeactive", data).then(function (promise) {
                            if (promise.returnmsg === "Mapped") {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                            }
                            $scope.OnChangeMasteTypeInsitution();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };


        $scope.MasterClassificationQuestionActiveDeactive = function (obj) {

            var mgs = "";
            var confirmmgs = "";
            if (obj.ratmqecL_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }

            var data = {
                "RATMQECL_Id": obj.ratmqecL_Id,
                "MI_Id": obj.mI_Id
            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Rating_Master/MasterClassificationQuestionActiveDeactive", data).then(function (promise) {
                            if (promise.returnmsg === "Mapped") {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                            }
                            $scope.OnClickRatingClassification();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.clear_first_tab = function () {
            $scope.OnChangeMasteTypeInsitution();
        };

        // Master Classification 

        $scope.OnClickRatingClassification = function () {
            $scope.Ratingclassification_MI_Id = "";
            $scope.RATMQECL_QuestionsClassification = "";
            $scope.search6 = "";
            $scope.submitted6 = false;
            $scope.EditFlagMasterquestion = false;
            $scope.getRatingTypeData = [];
            $scope.RATMQECL_Id = 0;
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnLoad"
            };
            apiService.create("Rating_Master/OnClickRatingClassification", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getuserinstitution_classification = promise.getuserinstitution;
                    $scope.Ratingclassification_MI_Id = promise.mI_Id;
                    $scope.getRatingTypeData = promise.getRatingTypeData;
                    $scope.getMasterQuestionClassificationLoadData = promise.getMasterQuestionClassificationLoadData;
                }
            });

        };

        $scope.OnChangeRatingClassificationInstitution = function () {
            $scope.RATMQECL_QuestionsClassification = "";
            $scope.RatingClassification_RATMT_Id = "";
            $scope.search6 = "";
            $scope.submitted6 = false;

            $scope.getRatingTypeData = [];

            $scope.RATMQECL_Id = 0;
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnChange",
                "MI_Id": $scope.Ratingclassification_MI_Id
            };
            apiService.create("Rating_Master/OnClickRatingClassification", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getRatingTypeData = promise.getRatingTypeData;
                    $scope.getMasterQuestionClassificationLoadData = promise.getMasterQuestionClassificationLoadData;
                }
            });
        };

        $scope.SaveRatingClassification = function () {

            if ($scope.myForm6.$valid) {


                var data = {
                    "RATMQECL_Id": $scope.RATMQECL_Id,
                    "MI_Id": $scope.Ratingclassification_MI_Id,
                    "RATMT_Id": $scope.RatingClassification_RATMT_Id,
                    "RATMQECL_QuestionsClassification": $scope.RATMQECL_QuestionsClassification,
                };
                apiService.create("Rating_Master/SaveRatingClassification", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnmsg == "Duplicate") {
                            swal("Record Already Exists");
                        } else if (promise.returnmsg === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.returnmsg === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else {
                            swal("Failed To Update/Save Record");
                        }
                        $scope.OnChangeRatingClassificationInstitution();
                    }
                });
            } else {
                $scope.submitted6 = true;
            }
        };

        $scope.interacted6 = function (field) {
            return $scope.submitted6;
        };

        $scope.EditMasterQuestionClassification = function (obj) {
            var data = {
                "RATMQECL_Id": obj.ratmqecL_Id,
                "MI_Id": obj.mI_Id,
                "RATMT_Id": obj.ratmT_Id
            };

            apiService.create("Rating_Master/EditMasterQuestionClassification", data).then(function (promise) {
                if (promise !== null) {

                    if (promise.checkRatingQuestionClassificationMapped !== null && promise.checkRatingQuestionClassificationMapped.length > 0) {
                        swal("Record Already Mapped To Employee So You Can Not Edit This Record");
                    } else {
                        $scope.EditFlagMasterFlag = true;
                        $scope.GetQuestionClassificationEditData = promise.getQuestionClassificationEditData;
                        $scope.Ratingclassification_MI_Id = $scope.GetQuestionClassificationEditData[0].mI_Id;
                        $scope.RatingClassification_RATMT_Id = $scope.GetQuestionClassificationEditData[0].ratmT_Id;
                        $scope.RATMQECL_QuestionsClassification = $scope.GetQuestionClassificationEditData[0].ratmqecL_QuestionsClassification;
                        $scope.RATMQECL_Id = $scope.GetQuestionClassificationEditData[0].ratmqecL_Id;
                    }
                }
            });
        };

        // Master Sub-Classification

        $scope.OnClickRatingSubClassification = function () {
            $scope.Ratingsubclassification_MI_Id = "";
            $scope.RATMQECL_QuestionsClassification = "";
            $scope.search6 = "";
            $scope.submitted6 = false;
            $scope.EditFlagMasterquestion = false;
            $scope.getRatingTypeData = [];
            $scope.RATMQECL_Id = 0;
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnLoad"
            };
            apiService.create("Rating_Master/OnClickRatingSubClassification", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getuserinstitution_subclassification = promise.getuserinstitution;
                    $scope.Ratingsubclassification_MI_Id = promise.mI_Id;
                    $scope.getQuestionClassificationData = promise.getQuestionClassificationData;
                    $scope.getMasterQuestionsubClassificationLoadData = promise.getMasterQuestionsubClassificationLoadData;
                }
            });

        };

        $scope.OnChangeRatingSubClassificationInstitution = function () {
            $scope.RATMQECLSUB_QuestionsSubclassification = "";
            $scope.RATMQECL_Id = "";
            $scope.search7 = "";
            $scope.submitted7 = false;

            $scope.getQuestionClassificationData = [];

            $scope.RATMQECLSUB_Id = 0;
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnChange",
                "MI_Id": $scope.Ratingsubclassification_MI_Id
            };
            apiService.create("Rating_Master/OnClickRatingSubClassification", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getQuestionClassificationData = promise.getQuestionClassificationData;
                    $scope.getMasterQuestionsubClassificationLoadData = promise.getMasterQuestionsubClassificationLoadData;
                }
            });
        };

        $scope.SaveRatingsubClassification = function () {

            if ($scope.myForm7.$valid) {


                var data = {
                    "RATMQECLSUB_Id": $scope.RATMQECLSUB_Id,
                    "RATMQECL_Id": $scope.RATMQECL_Id,
                    "RATMQECLSUB_QuestionsSubclassification": $scope.RATMQECLSUB_QuestionsSubclassification,
                };
                apiService.create("Rating_Master/SaveRatingsubClassification", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnmsg == "Duplicate") {
                            swal("Record Already Exists");
                        } else if (promise.returnmsg === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.returnmsg === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else {
                            swal("Failed To Update/Save Record");
                        }
                        $scope.OnChangeRatingSubClassificationInstitution();
                    }
                });
            } else {
                $scope.submitted7 = true;
            }
        };

        $scope.interacted7 = function (field) {
            return $scope.submitted7;
        };

        $scope.clear_seventh_tab = function () {
            $scope.OnChangeRatingSubClassificationInstitution();
        };

        $scope.EditMasterQuestionSubClassification = function (obj) {
            var data = {
                "RATMQECLSUB_Id": obj.ratmqeclsuB_Id,
                "MI_Id": obj.mI_Id
            };

            apiService.create("Rating_Master/EditMasterQuestionSubClassification", data).then(function (promise) {
                if (promise !== null) {

                    if (promise.checkRatingQuestionSubClassificationMapped !== null && promise.checkRatingQuestionSubClassificationMapped.length > 0) {
                        swal("Record Already Mapped To Employee So You Can Not Edit This Record");
                    } else {
                        $scope.EditFlagMasterFlag = true;
                        $scope.GetQuestionSubClassificationEditData = promise.getQuestionSubClassificationEditData;
                        $scope.ratmqeclsuB_Id = $scope.GetQuestionSubClassificationEditData[0].ratmqeclsuB_Id;
                        $scope.RATMQECL_Id = $scope.GetQuestionSubClassificationEditData[0].ratmqecL_Id;
                        $scope.RATMQECLSUB_QuestionsSubclassification = $scope.GetQuestionClassificationEditData[0].ratmqeclsuB_QuestionsSubclassification;
                    }
                }
            });
        };

        $scope.MasterSubClassificationQuestionActiveDeactive = function (obj) {

            var mgs = "";
            var confirmmgs = "";
            if (obj.ratmqeclsuB_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }

            var data = {
                "RATMQECLSUB_Id": obj.ratmqeclsuB_Id,
                "MI_Id": obj.mI_Id
            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Rating_Master/MasterSubClassificationQuestionActiveDeactive", data).then(function (promise) {
                            if (promise.returnmsg === "Mapped") {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                            }
                            $scope.OnClickRatingSubClassification();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        // Master Question
        $scope.OnClickMasterQuestions = function () {
            $scope.Master_Question_MI_Id = "";
            $scope.RATMQE_RatingQuestions = "";
            $scope.search2 = "";
            $scope.RATMQE_RatingsQRemarks = "";
            $scope.RATMQE_ManualEntryFlg = false;
            $scope.submitted2 = false;
            $scope.EditFlagMasterquestion = false;
            $scope.GetQuestionSubClassificationdata = [];
            $scope.GetMasterQuestionLoadData = [];
            $scope.GetMasterQuestionOrderData = [];
            $scope.RATMQE_Id = 0;
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnLoad"
            };
            apiService.create("Rating_Master/OnClickMasterQuestions", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getuserinstitution_questions = promise.getuserinstitution;
                    $scope.Master_Question_MI_Id = promise.mI_Id;
                    $scope.GetMasterQuestionLoadData = promise.getMasterQuestionLoadData;
                    $scope.GetMasterQuestionOrderData = promise.getMasterQuestionOrderData;
                    $scope.GetQuestionSubClassificationdata = promise.getQuestionSubClassificationdata;
                }
            });
        };

        $scope.OnChangeMasteQuesInsitution = function () {

            $scope.RATMQE_RatingQuestions = "";
            $scope.RATMQE_RatingsQRemarks = "";
            $scope.RATMQE_QuestionFlg = "";
            $scope.search2 = "";
            $scope.RATMQE_ManualEntryFlg = false;
            $scope.RATMQE_AutoEntryFlg = false;
            $scope.submitted2 = false;
            $scope.EditFlagMasterquestion = false;
            $scope.GetQuestionSubClassificationdata = [];
            $scope.GetMasterQuestionLoadData = [];
            $scope.GetMasterQuestionOrderData = [];
            $scope.RATMQE_Id = 0;
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnChange",
                "MI_Id": $scope.Master_Question_MI_Id
            };
            apiService.create("Rating_Master/OnClickMasterQuestions", data).then(function (promise) {
                if (promise !== null) {
                    $scope.GetMasterQuestionLoadData = promise.getMasterQuestionLoadData;
                    $scope.GetMasterQuestionOrderData = promise.getMasterQuestionOrderData;
                    $scope.GetQuestionSubClassificationdata = promise.getQuestionSubClassificationdata;
                }
            });
        };

        $scope.SaveMasterQuestion = function () {

            if ($scope.myForm2.$valid) {

                if ($scope.RATMQE_AutoEntryFlg == false || $scope.RATMQE_AutoEntryFlg == undefined) {

                    $scope.RATMQE_AutoEntryFlg = false;
                }

                var data = {
                    "RATMQE_Id": $scope.RATMQE_Id,
                    "MI_Id": $scope.Master_Question_MI_Id,
                    "RATMQECLSUB_Id": $scope.RATMQECLSUBClassification_Id,
                    "RATMQE_RatingQuestions": $scope.RATMQE_RatingQuestions,
                    "RATMQE_ManualEntryFlg": $scope.RATMQE_ManualEntryFlg,
                    "RATMQE_AutoEntryFlg": $scope.RATMQE_AutoEntryFlg,
                    "RATMQE_RatingsQRemarks": $scope.RATMQE_RatingsQRemarks,
                    "RATMQE_QuestionFlg": $scope.RATMQE_QuestionFlg
                };
                apiService.create("Rating_Master/SaveMasterQuestion", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnmsg == "Duplicate") {
                            swal("Record Already Exists");
                        } else if (promise.returnmsg === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.returnmsg === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else {
                            swal("Failed To Update/Save Record");
                        }
                        $scope.OnChangeMasteQuesInsitution();
                    }
                });
            } else {
                $scope.submitted2 = true;
            }
        };

        $scope.EditMasterQuestion = function (obj2) {
            var data = {
                "RATMQE_Id": obj2.ratmqE_Id,
                "MI_Id": $scope.Master_Question_MI_Id
            };
            apiService.create("Rating_Master/EditMasterQuestions", data).then(function (promise) {
                if (promise !== null) {
                    $scope.EditFlagMasterquestion = true;
                    $scope.GetMaterQuestionEditData = promise.getMaterQuestionEditData;
                    $scope.RATMQE_Id = $scope.GetMaterQuestionEditData[0].ratmqE_Id;
                    $scope.RATMQE_RatingQuestions = $scope.GetMaterQuestionEditData[0].ratmqE_RatingQuestions;
                    $scope.RATMQE_ManualEntryFlg = $scope.GetMaterQuestionEditData[0].ratmqE_ManualEntryFlg;
                    $scope.RATMQE_RatingsQRemarks = $scope.GetMaterQuestionEditData[0].ratmqE_RatingsQRemarks;
                    $scope.RATMQE_AutoEntryFlg = $scope.GetMaterQuestionEditData[0].ratmqE_AutomaticFlg;
                    $scope.RATMQE_QuestionFlg = $scope.GetMaterQuestionEditData[0].ratmqE_QuestionFlg;
                    $scope.RATMQECLSUBClassification_Id = $scope.GetMaterQuestionEditData[0].ratmqeclsuB_Id;
                }
            });
        };

        $scope.MasteQuestionActiveDeactive = function (obj) {

            var mgs = "";
            var confirmmgs = "";
            if (obj.ratmqE_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }

            var data = {
                "RATMQE_Id": obj.ratmqE_Id,
                "MI_Id": $scope.Master_Question_MI_Id
            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Rating_Master/MasteQuestionActiveDeactive", data).then(function (promise) {
                            if (promise.returnmsg === "Mapped") {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                            }
                            $scope.OnChangeMasteQuesInsitution();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.UpdateMasterQuestionOrder = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                orderarray[key].ratmqE_RQOrder = key + 1;
            });

            var data = {
                "MI_Id": $scope.Master_Question_MI_Id,
                "Temp_Question_Order": orderarray
            }

            apiService.create("Rating_Master/UpdateMasterQuestionOrder", data).then(function (promise) {
                if (promise.returnval === true) {
                    swal("Order Updated Successfuuly");
                    $('#masterquestionorder').modal('hide');
                    $scope.OnChangeMasteQuesInsitution();
                } else {
                    swal("Fail To Update Order");
                }
            });
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };

        $scope.clear_second_tab = function () {
            $scope.OnChangeMasteQuesInsitution();
        };


        // Master Option
        $scope.OnClickMasterOption = function () {
            $scope.Master_Options_MI_Id = "";
            $scope.RATMOP_RatingOptions = "";
            $scope.search3 = "";
            $scope.RATMOP_OptionsValue = "";
            $scope.RATMOP_RatingORemarks = "";
            $scope.getuserinstitution_options = [];
            $scope.GetMasterOptionLoadData = [];
            $scope.GetMasterOptionOrderData = [];
            $scope.submitted3 = false;
            $scope.EditFlagMasterOption = false;
            $scope.RATMOP_Id = 0;
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnLoad"
            };
            apiService.create("Rating_Master/OnClickMasterOption", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getuserinstitution_options = promise.getuserinstitution;
                    $scope.Master_Options_MI_Id = promise.mI_Id;
                    $scope.GetMasterOptionLoadData = promise.getMasterOptionLoadData;
                    $scope.GetMasterOptionOrderData = promise.getMasterOptionOrderData;
                }
            });
        };

        //OnClickRatingtypeQuestionMapping
        $scope.OnClickRatingtypeQuestionMapping = function () {
            $scope.Master_Options_MI_Id = "";
            $scope.RATMOP_RatingOptions = "";
            $scope.search3 = "";
            $scope.RATMOP_OptionsValue = "";
            $scope.RATMOP_RatingORemarks = "";
            $scope.getuserinstitution_options = [];
            $scope.GetMasterOptionLoadData = [];
            $scope.GetMasterOptionOrderData = [];
            $scope.submitted3 = false;
            $scope.EditFlagMasterOption = false;
            $scope.RATMOP_Id = 0;
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnLoad"
            };
            apiService.create("Rating_Master/OnClickRatingtypeQuestionMapping", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getuserinstitution_ratingQuestionMapping = promise.getuserinstitution;
                    $scope.GetRatingTypeQuestionMappingLoadData = promise.getRatingTypeQuestionMappingLoadData;
                    $scope.GetRatingTypeQuestionMappingLoadDataflag = promise.getRatingTypeQuestionMappingLoadData[0].ratmT_ActiveFlag;
                    $scope.Master_Options_MI_Id = promise.mI_Id;
                    $scope.GetMasterOptionLoadData = promise.getMasterOptionLoadData;
                    $scope.GetMasterOptionOrderData = promise.getMasterOptionOrderData;
                }
            });
        };



        $scope.OnChangeMasteOptionInsitution = function () {

            $scope.RATMOP_RatingOptions = "";
            $scope.RATMOP_OptionsValue = "";
            $scope.RATMOP_RatingORemarks = "";
            $scope.search3 = "";
            $scope.submitted3 = false;
            $scope.GetMasterOptionLoadData = [];
            $scope.GetMasterOptionOrderData = [];
            $scope.EditFlagMasterOption = false;
            $scope.RATMOP_Id = 0;
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnChange",
                "MI_Id": $scope.Master_Options_MI_Id
            };
            apiService.create("Rating_Master/OnClickMasterOption", data).then(function (promise) {
                if (promise !== null) {
                    $scope.GetMasterOptionLoadData = promise.getMasterOptionLoadData;
                    $scope.GetMasterOptionOrderData = promise.getMasterOptionOrderData;
                }
            });
        };

        $scope.SaveMasterOption = function () {
            if ($scope.myForm3.$valid) {
                var data = {
                    "RATMOP_Id": $scope.RATMOP_Id,
                    "MI_Id": $scope.Master_Options_MI_Id,
                    "RATMOP_RatingOptions": $scope.RATMOP_RatingOptions,
                    "RATMOP_RatingORemarks": $scope.RATMOP_RatingORemarks,
                    "RATMOP_OptionsValue": $scope.RATMOP_OptionsValue
                };
                apiService.create("Rating_Master/SaveMasterOption", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnmsg == "Duplicate") {
                            swal("Record Already Exists");
                        } else if (promise.returnmsg === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.returnmsg === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else {
                            swal("Failed To Update/Save Record");
                        }
                        $scope.OnChangeMasteOptionInsitution();
                    }
                });
            } else {
                $scope.submitted3 = true;
            }
        };

        $scope.EditMasterOption = function (obj3) {
            var data = {
                "RATMOP_Id": obj3.ratmoP_Id,
                "MI_Id": $scope.Master_Options_MI_Id
            };
            apiService.create("Rating_Master/EditMasterOption", data).then(function (promise) {
                if (promise !== null) {
                    $scope.GetMasterOptionEditData = promise.getMasterOptionEditData;
                    $scope.RATMOP_Id = $scope.GetMasterOptionEditData[0].ratmoP_Id;
                    $scope.RATMOP_RatingOptions = $scope.GetMasterOptionEditData[0].ratmoP_RatingOptions;
                    $scope.RATMOP_OptionsValue = $scope.GetMasterOptionEditData[0].ratmoP_OptionsValue;
                    $scope.RATMOP_RatingORemarks = $scope.GetMasterOptionEditData[0].ratmoP_RatingORemarks;
                    $scope.EditFlagMasterOption = true;
                }
            });
        };

        $scope.MasteOptionActiveDeactive = function (obj) {

            var mgs = "";
            var confirmmgs = "";
            if (obj.ratmoP_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }

            var data = {
                "RATMOP_Id": obj.ratmoP_Id,
                "MI_Id": $scope.Master_Options_MI_Id
            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Rating_Master/MasteOptionActiveDeactive", data).then(function (promise) {
                            if (promise.returnmsg === "Mapped") {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                            }
                            $scope.OnChangeMasteOptionInsitution();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.UpdateMasterOptionOrder = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                orderarray[key].ratmoP_ROOrder = key + 1;
            });

            var data = {
                "MI_Id": $scope.Master_Options_MI_Id,
                "Temp_Option_Order": orderarray
            }

            apiService.create("Rating_Master/UpdateMasterOptionOrder", data).then(function (promise) {

                if (promise.returnval === true) {
                    swal("Order Updated Successfuuly");
                    $('#masteroptionorder').modal('hide');
                    $scope.OnChangeMasteOptionInsitution();
                } else {
                    swal("Fail To Update Order");
                }
            });
        };

        $scope.interacted3 = function (field) {
            return $scope.submitted3;
        };

        $scope.clear_third_tab = function () {
            $scope.OnChangeMasteOptionInsitution();
        };

        // Rating Authority

        $scope.OnClickRatingAuthority = function () {
            $scope.Ratingauthority_MI_Id = "";
            $scope.RATMQECL_QuestionsClassification = "";
            $scope.search8 = "";
            $scope.submitted8 = false;
            $scope.EditFlagMasterquestion = false;
            $scope.getMasterAuthority = [];
            $scope.getDepartmentData = [];
            $scope.RATMAUT_Id = 0;
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnLoad"
            };
            apiService.create("Rating_Master/OnClickRatingAuthority", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getuserinstitution_authority = promise.getuserinstitution;
                    $scope.Ratingauthority_MI_Id = promise.mI_Id;
                    $scope.getDepartmentData = promise.getDepartmentData;
                    $scope.getMasterAuthority = promise.getMasterAuthority;
                }
            });

        };

        $scope.OnChangeRatingauthorityInstitution = function () {

            $scope.search8 = "";
            $scope.submitted8 = false;

            $scope.getDepartmentData = [];
            $scope.getMasterAuthority = [];
            $scope.getDesginationData = [];
            $scope.HRMD_id = "";

            $scope.RATMQECLSUB_Id = 0;
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnChange",
                "MI_Id": $scope.Ratingauthority_MI_Id
            };
            apiService.create("Rating_Master/OnClickRatingAuthority", data).then(function (promise) {
                if (promise !== null) {

                    $scope.getDepartmentData = promise.getDepartmentData;
                    $scope.getMasterAuthority = promise.getMasterAuthority;
                }
            });
        };

        $scope.OnChangeRatingDepartment = function () {
            $scope.getDesginationData = [];
            var data = {
                "HRMD_Id": $scope.HRMD_id,
            }
            apiService.create("Rating_Master/OnChangeRatingDepartment", data).then(function (promise) {
                if (promise != null) {
                    $scope.getDesginationData = promise.getDesginationData;
                }

            });
        }

        $scope.SaveRatingAuthority = function () {

            $scope.designationlistarray = [];

            angular.forEach($scope.getDesginationData, function (aa) {
                if (aa.selected == true) {
                    $scope.designationlistarray.push({ HRMDES_Id: aa.hrmdeS_Id })
                }
            });

            if ($scope.myForm8.$valid) {
                var data = {
                    "RATMAUT_Id": $scope.RATMAUT_Id,
                    "MI_Id": $scope.Ratingauthority_MI_Id,
                    "HRMD_Id": $scope.HRMD_id,
                    "designationlistarray": $scope.designationlistarray
                };
                apiService.create("Rating_Master/SaveRatingAuthority", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnmsg == "Duplicate") {
                            swal("Record Already Exists");
                        } else if (promise.returnmsg === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.returnmsg === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else {
                            swal("Failed To Update/Save Record");
                        }
                        $scope.OnChangeRatingauthorityInstitution();
                    }
                });
            } else {
                $scope.submitted8 = true;
            }
        };
        $scope.interacted8 = function (field) {
            return $scope.submitted8;
        };

        $scope.clear_eight_tab = function () {
            $scope.OnChangeRatingauthorityInstitution();
        };

        $scope.MasterAuthorityActiveDeactive = function (obj) {

            var mgs = "";
            var confirmmgs = "";
            if (obj.ratmauT_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }

            var data = {
                "RATMAUT_Id": obj.ratmauT_Id,
                "MI_Id": obj.mI_Id
            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Rating_Master/MasterAuthorityActiveDeactive", data).then(function (promise) {
                            if (promise.returnmsg === "Mapped") {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                            }
                            $scope.OnChangeMasteTypeInsitution();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };


        //Rating type and question mapping
        $scope.SaveRatingQuestionMapping = function () {

            $scope.questionlistarray = [];
            angular.forEach($scope.getRatingTypeQuestionData, function (qq) {
                if (qq.selected == true) {
                    $scope.questionlistarray.push({ RATMQE_Id: qq.ratmqE_Id })
                }
            });


            if ($scope.myForm5.$valid) {
                var data = {
                    "RATMTQ_Id": $scope.RATMTQ_Id,
                    "MI_Id": $scope.RatingQuestionMapping_MI_Id,
                    "questionlistarray": $scope.questionlistarray,
                    "RATMT_Id": $scope.RatingQuestionMapping_RATMT_Id
                };
                apiService.create("Rating_Master/SaveRatingQuestionMapping", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnmsg == "Duplicate") {
                            swal("Record Already Exists");
                        } else if (promise.returnmsg === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.returnmsg === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else {
                            swal("Failed To Update/Save Record");
                        }
                        $scope.OnClickRatingtypeQuestionMapping();
                    }
                });
            } else {
                $scope.submitted5 = true;
            }
        };

        $scope.interacted5 = function (field) {
            return $scope.submitted5;
        };

        $scope.OnChangeRatingQuestionMappingInsitution = function () {

            $scope.getRatingTypeQuestionData = [];
            $scope.RatingQuestionMapping_RATMT_Id = "";
            $scope.obj.usercheckCC = false;

            $scope.search5 = "";
            $scope.submitted5 = false;
            $scope.EditFlagMasterOption = false;
            $scope.RATMTQ_Id = 0;
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnChange",
                "MI_Id": $scope.RatingQuestionMapping_MI_Id
            };
            apiService.create("Rating_Master/OnClickRatingtypeQuestionMapping", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getRatingTypeData = promise.getRatingTypeData;
                    $scope.GetRatingTypeQuestionMappingLoadData = promise.getRatingTypeQuestionMappingLoadData;
                    $scope.GetRatingTypeQuestionMappingLoadDataflag = promise.getRatingTypeQuestionMappingLoadData[0].ratmT_ActiveFlag;
                }
            });
        };

        $scope.OnChangeRatingQuestion_Rating = function () {
            $scope.search3 = "";
            $scope.submitted3 = false;
            $scope.getRatingTypeQuestionData = [];
            $scope.EditFlagMasterOption = false;
            $scope.RATMOP_Id = 0;
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnChange",
                "MI_Id": $scope.RatingQuestionMapping_MI_Id

            };
            apiService.create("Rating_Master/OnChangeRatingQuestion_Rating", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getRatingTypeQuestionData = promise.getRatingTypeQuestionData;
                    // $scope.GetMasterOptionOrderData = promise.getMasterOptionOrderData;
                }
            });
        };


        $scope.al_checkdesignation = function (all, hrmdeS_Id) {
            $scope.obj.usercheck = all;

            var toggleStatus = $scope.obj.usercheck;
            angular.forEach($scope.getDesginationData, function (role) {
                role.selected = toggleStatus;
            });
        }


        $scope.getdesignation = function (hrmdeS_Id) {

            $scope.obj.checkdesignation = $scope.getDesginationData.every(function (options) {
                return options.selected;
            });
        }


        $scope.isOptionsRequireddesignation = function () {
            return !$scope.getDesginationData.some(function (item) {
                return item.selected;
            });
        };


        $scope.al_checkQuestion = function (all, RATMQE_Id) {
            $scope.obj.usercheck = all;

            var toggleStatus = $scope.obj.usercheck;
            angular.forEach($scope.getRatingTypeQuestionData, function (role) {
                role.selected = toggleStatus;
            });


            $scope.questionlistarray = [];
            angular.forEach($scope.getRatingTypeQuestionData, function (qq) {
                if (qq.selected == true) {
                    $scope.questionlistarray.push({ RATMQE_Id: qq.ratmqE_Id })
                }
            });

        }

        var ratingtypearray = [];

        $scope.al_checkRatingType = function (all, RATMT_Id) {
            $scope.obj.userratingtypecheck = all;

            var toggleStatus = $scope.obj.userratingtypecheck;
            angular.forEach($scope.GetMasterTypeData, function (role) {
                role.selected = toggleStatus;
            });

            angular.forEach($scope.GetMasterTypeData, function (qq) {
                if (qq.selected == true) {
                    ratingtypearray.push(qq.ratmT_Id)
                }
            });
            $scope.OnChangeRatingQuestionOption_Rating();
        }

        var departmentarray = [];
        $scope.al_checkDepartment = function (all, HRMD_Id) {
            $scope.obj.userdeptcheck = all;

            var toggleStatus = $scope.obj.userdeptcheck;
            angular.forEach($scope.GetDepartmentData, function (role) {
                role.selected = toggleStatus;
            });


            angular.forEach($scope.GetDepartmentData, function (qq) {
                if (qq.selected == true) {
                    departmentarray.push({ HRMD_Id: qq.hrmD_Id })
                }
            });
            $scope.OnChangeDepartment();
        }




        $scope.getquestion = function (getquestion) {
            $scope.questionlistarray = [];

            angular.forEach($scope.getRatingTypeQuestionData, function (aa) {
                if (aa.selected == true) {
                    $scope.questionlistarray.push({ RATMQE_Id: aa.ratmqE_Id })
                }

            });


        }

        $scope.OnChangeRatingType = function (RATMT_Id) {
            ratingtypearray = [];

            angular.forEach($scope.GetMasterTypeData, function (ty) {
                if (ty.selected) {
                    ratingtypearray.push(ty.ratmT_Id);
                }

            })
            $scope.obj.userratingtypecheck = $scope.GetMasterTypeData.every(function (options) {
                return options.selected;
            });


            $scope.OnChangeRatingQuestionOption_Rating();

        }


        $scope.OnChangeDesignation = function (HRMD_Id) {

            departmentarray = [];

            angular.forEach($scope.GetDepartmentData, function (tyy) {
                if (tyy.selected) {
                    departmentarray.push({ HRMD_Id: tyy.hrmD_Id })
                }
            })
            $scope.OnChangeDepartment();

        }




        $scope.isOptionsRequiredquestiontype = function () {
            return !$scope.GetMasterTypeData.some(function (option) {
                return option.selected;
            });
        }


        $scope.isOptionsRequiredquestion = function () {
            return !$scope.getRatingTypeQuestionData.some(function (item) {
                return item.selected;
            });
        };

        $scope.isOptionsRequireddepartment = function () {
            return !$scope.GetDepartmentData.some(function (item) {
                return item.selected;
            });
        };

        $scope.viewdocument = function (ratmT_Id) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];


            var data = {
                "RATMT_Id": ratmT_Id


            }

            apiService.create("Rating_Master/viewquestions", data).then(function (promise) {
                if (promise !== null) {
                    $scope.RATMT_RatingTypeName = promise.viewarray[0].ratmT_RatingTypeName;
                    $scope.uploadfilesdetails = promise.viewarray;
                    if (promise.viewarray !== null && promise.viewarray.length > 0) {
                        $scope.uploaddocfiles = promise.viewarray;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.filepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');

                        swal("No Documents Found");
                    }
                }
            });
        };

        //



        $scope.OnChangeRatingQuestionOption_Rating = function () {
            $scope.search4 = "";
            $scope.submitted4 = false;
            $scope.EditFlagMasterOption = false;
            $scope.RATMOP_Id = 0;
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnChange",
                "MI_Id": $scope.Master_TypeQues_MI_Id,
                "RATMT_Array": ratingtypearray

            };
            apiService.create("Rating_Master/OnChangeRatingQuestionOption_Rating", data).then(function (promise) {
                if (promise.getMasterQuestionData != null && promise.getMasterQuestionData.length > 0) {

                    $scope.GetMasterQuestionData = promise.getMasterQuestionData;

                }
            });
        };


        $scope.al_checkQuestionfourthtab = function (all, RATMQE_Id) {
            $scope.obj.usercheck = all;

            var toggleStatus = $scope.obj.usercheck;
            angular.forEach($scope.GetMasterQuestionData, function (role) {
                role.selected = toggleStatus;
            });


            $scope.questionlistarrayForthtab = [];
            angular.forEach($scope.GetMasterQuestionData, function (qq) {
                if (qq.selected == true) {
                    $scope.questionlistarrayForthtab.push({ RATMQE_Id: qq.ratmqE_Id })
                }
            });

            $scope.OnChangeQuestion();

        }


        $scope.al_checkOptiontab = function (all) {
            $scope.obj.useroptioncheck = all;
            var checkStatus = $scope.obj.useroptioncheck;
            var count = 0;
            angular.forEach($scope.GetMasterOptionData, function (itm) {
                itm.checkedsuboption = checkStatus;
                if (itm.checkedsuboption == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }


        $scope.OnChangeOption = function () {

            $scope.obj.useroptioncheck = $scope.GetMasterOptionData.every(function (options) {
                return options.checkedsuboption;
            });
        }


        $scope.al_checkDesignation = function (all) {
            $scope.obj.userdesignationcheck = all;
            var checkStatus = $scope.obj.userdesignationcheck;
            var count = 0;
            angular.forEach($scope.GetDesginationData, function (itm) {
                itm.checkedsub = checkStatus;
                if (itm.checkedsub == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }

        $scope.OnChangeDesignation = function () {

            $scope.obj.userdesignationcheck = $scope.GetDesginationData.every(function (options) {
                return options.checkedsub;
            });
        }


        $scope.getForthtabquestion = function (RATMQE_Id) {
            $scope.questionlistarrayForthtab = [];

            angular.forEach($scope.GetMasterQuestionData, function (aa) {
                if (aa.selected == true) {
                    $scope.questionlistarrayForthtab.push({ RATMQE_Id: aa.ratmqE_Id })
                }

            });




        }

        //Type Question Mapping

        $scope.OnClickTypeQuestionMapping = function () {
            $scope.GetMasterTypeData = [];
            $scope.GetTypeMasterQuestionMapping = [];
            $scope.GetMasterQuestionData = [];
            $scope.getuserinstitution_typeques = [];
            $scope.GetDesginationData = [];
            $scope.GetQuestionDeptDesigData = [];
            $scope.Temp_AddUser = [];
            $scope.Master_TypeQues_MI_Id = "";
            $scope.searchchkbx = "";
            $scope.searchchkbx1 = "";
            $scope.RATMT_Id_Type = "";
            $scope.search4 = "";
            $scope.objques.RATMQE_Id = "";
            $scope.objques.HRMD_Id = "";
            $scope.Addflag = false;
            $scope.submitted4 = false;
            $scope.GetMasterOptionData = [];
            $scope.GetMasterTypeQuestionOptionMapping = [];
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnLoad"
            };

            apiService.create("Rating_Master/OnClickTypeQuestionMapping", data).then(function (promise) {
                if (promise !== null) {
                    $scope.GetMasterTypeData = promise.getMasterTypeData;
                    $scope.GetDepartmentData = promise.getDepartmentData;
                    // $scope.GetDepartmentAuthority = promise.getDepartmentData;
                    //$scope.GetMasterQuestionData = promise.getMasterQuestionData;
                    $scope.GetTypeMasterQuestionMapping = promise.getTypeMasterQuestionMapping;
                    $scope.getuserinstitution_typeques = promise.getuserinstitution;
                    $scope.Master_TypeQues_MI_Id = promise.mI_Id;
                    $scope.getAuthority = promise.getAuthority;
                }
            });
        };

        $scope.OnChangeMasteTypeQuesInsitution = function () {
            $scope.GetMasterTypeData = [];
            $scope.GetTypeMasterQuestionMapping = [];
            $scope.getAuthority = [];
            $scope.GetMasterQuestionData = [];
            $scope.GetDesginationData = [];
            $scope.GetQuestionDeptDesigData = [];
            $scope.Temp_AddUser = [];
            $scope.RATMT_Id_Type = "";
            $scope.searchchkbx = "";
            $scope.searchchkbx1 = "";
            $scope.search4 = "";
            $scope.objques.RATMQE_Id = "";
            $scope.objques.HRMD_Id = "";
            $scope.GetMasterOptionData = [];
            $scope.GetMasterTypeQuestionOptionMapping = [];
            $scope.submitted4 = false;
            $scope.Addflag = false;
            var data = {
                "FlagOnLoadOrOnChangeInst": "OnChange",
                "MI_Id": $scope.Master_TypeQues_MI_Id
            };

            apiService.create("Rating_Master/OnChangeMasteTypeQuesInsitution", data).then(function (promise) {
                if (promise !== null) {
                    $scope.GetMasterTypeData = promise.getMasterTypeData;
                    $scope.GetDepartmentData = promise.getDepartmentData;
                    //$scope.GetMasterQuestionData = promise.getMasterQuestionData;
                    $scope.GetTypeMasterQuestionMapping = promise.getTypeMasterQuestionMapping;
                    $scope.getAuthority = promise.getAuthority;
                }
            });
        };

        $scope.OnChangeQuestion = function () {
            $scope.Temp_AddUser = [];

            $scope.GetMasterOptionData = [];
            $scope.GetMasterTypeQuestionOptionMapping = [];

            $scope.questionoptionflag = 0;
            //if (ques.ratmqE_ManualEntryFlg === true) {
            //    $scope.questionoptionflag = 1;
            //}

            $scope.GetMasterQuestionDatalist = [];
            angular.forEach($scope.GetMasterQuestionData, function (em) {
                if (em.selected === true) {
                    $scope.GetMasterQuestionDatalist.push({ RATMQE_Id: em.ratmqE_Id });
                }
            });


            var data = {
                "GetMasterQuestionDatalist": $scope.GetMasterQuestionDatalist,
                "MI_Id": $scope.Master_TypeQues_MI_Id,
                "RATMT_Array": ratingtypearray
            };

            apiService.create("Rating_Master/OnChangeQuestion", data).then(function (promise) {

                if (promise !== null) {
                    $scope.GetQuestionDeptDesigData = promise.getQuestionDeptDesigData;



                    //angular.forEach($scope.GetQuestionDeptDesigData, function (dd) {
                    //    if ($scope.Temp_AddUser.length === 0) {
                    //        $scope.Temp_AddUser.push({ HRMD_Id: dd.hrmD_Id, HRMD_DepartmentName: dd.hrmD_DepartmentName });
                    //    } else if ($scope.Temp_AddUser.length > 0) {
                    //        var count = 0;
                    //        angular.forEach($scope.Temp_AddUser, function (d) {
                    //            if (d.HRMD_Id === dd.hrmD_Id) {
                    //                count += 1;
                    //            }
                    //        });
                    //        if (count === 0) {
                    //            $scope.Temp_AddUser.push({ HRMD_Id: dd.hrmD_Id, HRMD_DepartmentName: dd.hrmD_DepartmentName });
                    //        }
                    //    }
                    //});

                    //$scope.Temp_Dept_Desgi = [];
                    //angular.forEach($scope.Temp_AddUser, function (dd) {
                    //    $scope.Temp_Dept_Desgi = [];
                    //    angular.forEach($scope.GetQuestionDeptDesigData, function (d) {
                    //        if (dd.HRMD_Id === d.hrmD_Id) {
                    //            $scope.Temp_Dept_Desgi.push({ HRMDES_Id: d.hrmdeS_Id, HRMDES_DesignationName: d.hrmdeS_DesignationName });
                    //        }
                    //    });
                    //    dd.TempDesgin = $scope.Temp_Dept_Desgi;
                    //});

                    if ($scope.questionoptionflag === 0) {

                        $scope.GetMasterOptionData = promise.getMasterOptionData;
                        $scope.GetMasterTypeQuestionOptionMapping = promise.getMasterTypeQuestionOptionMapping;

                        //angular.forEach($scope.GetMasterOptionData, function (dd) {
                        //    dd.checkedsuboption = false;
                        //    angular.forEach($scope.GetMasterTypeQuestionOptionMapping, function (d) {
                        //        if (dd.ratmoP_Id === d.ratmoP_Id) {
                        //            dd.checkedsuboption = true;
                        //        }
                        //    });
                        //});
                    }
                }
            });
        };

        $scope.OnChangeDepartment = function (objques) {
            var data = {

                "MI_Id": $scope.Master_TypeQues_MI_Id,

                "HRMD_Id": objques.hrmD_Id,
            };
            apiService.create("Rating_Master/OnChangeDepartment", data).then(function (promise) {
                if (promise.getDesginationData != null && promise.getDesginationData.length > 0) {
                    $scope.GetDesginationData = promise.getDesginationData;
                }
            })
        };

        // OnChange Company Loading Department
        $scope.OnChangeComapnyAuthoritys = function (objj) {
            var data = {

                "MI_Id": $scope.Authority_MI_Id
            };
            apiService.create("Rating_Master/OnChangeComapnyAuthoritys", data).then(function (promise) {
                if (promise !== null) {
                    $scope.GetDepartmentAuthority = promise.getDepartmentData;
                }
            });
        };

        //OnChangeDepartmentAuthority
        $scope.OnChangeDepartmentAuthoritys = function (objj) {
            var data = {
                "MI_Id": $scope.Authority_MI_Id,
                "HRMD_Id": $scope.AuthoritiesDept
            };
            apiService.create("Rating_Master/OnChangeDepartmentAuthority", data).then(function (promise) {
                if (promise !== null) {
                    $scope.GetDesginationDataAuthority = promise.getDesginationDataAuthority;
                }
            });
        };


        $scope.isOptionsRequired3 = function () {
            return !$scope.GetDesginationData.some(function (options) {
                return options.checkedsub;
            });
        };

        $scope.isOptionsRequired4 = function () {
            return !$scope.GetMasterOptionData.some(function (options) {
                return options.checkedsuboption;
            });
        };

        $scope.AddToCart = function () {
            if ($scope.myForm4.$valid) {
                $scope.Addflag = true;
                var count = 0;
                $scope.TempDesgind = [];

                if ($scope.Temp_AddUser.length > 0) {
                    angular.forEach($scope.Temp_AddUser, function (d) {
                        if (Number(d.HRMD_Id) === Number($scope.objques.HRMD_Id.hrmD_Id)) {
                            count += 1;
                            angular.forEach($scope.GetDesginationData, function (des) {
                                if (des.checkedsub) {
                                    var count2 = 0;
                                    angular.forEach(d.TempDesgin, function (dd) {
                                        if (Number(dd.HRMDES_Id) === Number(des.hrmdeS_Id)) {
                                            count2 += 1;
                                        }
                                    });

                                    if (count2 === 0) {
                                        d.TempDesgin.push({ HRMDES_Id: des.hrmdeS_Id, HRMDES_DesignationName: des.hrmdeS_DesignationName });
                                    }
                                }
                            });
                        }
                    });

                }

                angular.forEach($scope.GetMasterQuestionData, function (em) {
                    if (em.selected === true) {
                        $scope.GetMasterQuestionDatalist.push({ RATMQE_Id: em.ratmqE_Id });
                    }
                });

                if (count === 0) {
                    angular.forEach($scope.GetDesginationData, function (dd) {
                        if (dd.checkedsub) {
                            $scope.TempDesgind.push({ HRMDES_Id: dd.hrmdeS_Id, HRMDES_DesignationName: dd.hrmdeS_DesignationName });
                        }
                    });
                    $scope.Temp_AddUser.push({
                        HRMD_Id: $scope.objques.HRMD_Id.hrmD_Id,
                        HRMD_DepartmentName: $scope.objques.HRMD_Id.hrmD_DepartmentName,
                        TempDesgin: $scope.TempDesgind
                    });
                }
                $scope.objques.HRMD_Id = "";
                $scope.searchchkbx = "";
                $scope.searchchkbx1 = "";
                angular.forEach($scope.GetDesginationData, function (desg) {
                    desg.checkedsub = false;
                })
            } else {
                $scope.submitted4 = true;
            }
        };

        $scope.DeleteTypeQuestionDeptDes = function (desgd, deptdd, index) {
            angular.forEach(deptdd.TempDesgin, function (dd, i) {
                if (Number(dd.HRMDES_Id) === Number(desgd.HRMDES_Id)) {
                    deptdd.TempDesgin.splice(i, 1);
                }
            });
        };

        $scope.DeleteTypeQuestionDept = function (deptd, index) {
            angular.forEach($scope.Temp_AddUser, function (dd, i) {
                if (Number(dd.HRMD_Id) === Number(deptd.HRMD_Id)) {
                    $scope.Temp_AddUser.splice(i, 1);
                }
            });
        };

        $scope.SaveTypeQuestion = function () {

            if ($scope.myForm4.$valid) {

                $scope.TempOption = [];

                angular.forEach($scope.GetMasterOptionData, function (dd) {
                    if (dd.checkedsuboption === true) {
                        $scope.TempOption.push(dd);
                    }
                });

                $scope.questionlistarrayForthtab = [];
                angular.forEach($scope.GetMasterQuestionData, function (qq) {
                    if (qq.selected == true) {
                        $scope.questionlistarrayForthtab.push({ RATMQE_Id: qq.ratmqE_Id, RATMT_Id: qq.ratmT_Id, RATMTQ_Id: qq.ratmtQ_Id })
                    }
                });


                var data = {};
                if ($scope.CheckBoxDeptDes == true) {
                    var data = {
                        //"RATMQE_Id": $scope.objques.RATMQE_Id.ratmqE_Id,
                        "MI_Id": $scope.Master_TypeQues_MI_Id,
                        "RATMT_Array": ratingtypearray,
                        "Temp_Department": $scope.Temp_AddUser,
                        "GetMasterQuestionDatalist": $scope.GetMasterQuestionDatalist,
                        //"Deptid": $scope.AuthoritiesDept,
                        //"Designationid": $scope.AuthoritiesDes,
                        "RATMAUT_Id": $scope.RATMAUT_Id.ratmauT_Id,
                        "CheckBoxDeptDes": $scope.CheckBoxDeptDes,
                        "questionlistarray": $scope.questionlistarrayForthtab
                    };
                } else {
                    var data = {
                        "MI_Id": $scope.Master_TypeQues_MI_Id,
                        "RATMT_Array": ratingtypearray,
                        "Temp_Department": $scope.Temp_AddUser,
                        "GetMasterQuestionDatalist": $scope.GetMasterQuestionDatalist,
                        "CheckBoxDeptDes": $scope.CheckBoxDeptDes,
                        "questionlistarray": $scope.questionlistarrayForthtab
                    };

                }


                // if ($scope.objques.RATMQE_Id.ratmqE_ManualEntryFlg === false) {
                //     data.Temp_Option = $scope.TempOption;
                //}

                angular.forEach($scope.GetMasterQuestionData, function (qq) {
                    if (qq.ratmqE_ManualEntryFlg === false) {
                        data.Temp_Option = $scope.TempOption;
                    }
                });



                //if ($scope.GetMasterQuestionData[0].ratmqE_ManualEntryFlg === false) {
                //    data.Temp_Option = $scope.TempOption;
                //}

                apiService.create("Rating_Master/SaveTypeQuestion", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Record Saved Successfully");
                        } else {
                            swal("Failed To Save Record");
                        }
                        $scope.OnClickTypeQuestionMapping();
                        $state.reload();
                    }
                });
            } else {
                $scope.submitted4 = true;
            }
        };

        $scope.MasteTypeQuesOptionActiveDeactive = function (obj) {

            var mgs = "";
            var confirmmgs = "";
            if (obj.ratmtQ_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }

            var data = {
                "RATMTQ_Id": obj.ratmtQ_Id,
                "MI_Id": $scope.Master_TypeQues_MI_Id
            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Rating_Master/MasteTypeQuesOptionActiveDeactive", data).then(function (promise) {
                            if (promise.returnmsg === "Mapped") {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                            }
                            $scope.OnChangeMasteTypeQuesInsitution();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.ViewTypeQuestionDeptMapping = function (dd) {
            $scope.ViewQuestionDeptDesigData = [];
            $scope.Temp_AddUser1 = [];
            $scope.Temp_RATMTQ_Id = dd.ratmtQ_Id;
            var data = {
                "RATMTQ_Id": dd.ratmtQ_Id,
                "MI_Id": $scope.Master_TypeQues_MI_Id
            };

            apiService.create("Rating_Master/ViewTypeQuestionDeptMapping", data).then(function (promise) {
                if (promise !== null) {
                    $scope.Temp_AddUser1 = [];
                    $scope.ViewQuestionDeptDesigData = promise.viewQuestionDeptDesigData;
                    if ($scope.ViewQuestionDeptDesigData === null || $scope.ViewQuestionDeptDesigData.length === 0) {
                        swal("No Records Found");
                    } else {
                        angular.forEach($scope.ViewQuestionDeptDesigData, function (dd) {
                            if ($scope.Temp_AddUser1.length === 0) {
                                $scope.Temp_AddUser1.push({ HRMD_Id: dd.hrmD_Id, HEMDC_ID: dd.hrmdC_ID, HRMD_DepartmentName: dd.hrmD_DepartmentName });
                            } else if ($scope.Temp_AddUser1.length > 0) {
                                var count = 0;
                                angular.forEach($scope.Temp_AddUser1, function (d) {
                                    if (d.HRMD_Id === dd.hrmD_Id) {
                                        count += 1;
                                    }
                                });
                                if (count === 0) {
                                    $scope.Temp_AddUser1.push({ HRMD_Id: dd.hrmD_Id, HEMDC_ID: dd.hrmdC_ID, HRMD_DepartmentName: dd.hrmD_DepartmentName });
                                }
                            }
                        });

                        $scope.Temp_Dept_Desgi1 = [];
                        angular.forEach($scope.Temp_AddUser1, function (dd) {
                            $scope.Temp_Dept_Desgi1 = [];
                            angular.forEach($scope.ViewQuestionDeptDesigData, function (d) {
                                if (dd.HRMD_Id === d.hrmD_Id) {
                                    $scope.Temp_Dept_Desgi1.push({ HRMDES_Id: d.hrmdeS_Id, HRMDES_DesignationName: d.hrmdeS_DesignationName });
                                }
                            });
                            dd.TempDesgin = $scope.Temp_Dept_Desgi1;
                        });

                        if ($scope.Temp_AddUser1.length > 0) {
                            $('#viewdeptdesgindetails').modal('show');
                            $('#ViewAuthoritydataa').modal('hide');
                        }
                    }
                }
            });
        };

        //Anusha R - 18/02/2023




        $scope.ViewAuthoritydataff = function (dd) {
            $scope.ViewAuthoritydata = [];
            $('#ViewAuthoritydataa').modal('hide');
            $scope.ViewAuthoritydata = [];
            var data = {
                "HRMDC_ID": dd,
                "MI_Id": $scope.Master_TypeQues_MI_Id
            };

            apiService.create("Rating_Master/ViewAuthoritydata", data).then(function (promise) {
                if (promise.viewAuthoritydata != null && promise.viewAuthoritydata.length > 0) {
                    $scope.ViewAuthoritydata = promise.viewAuthoritydata;
                    $('#ViewAuthoritydataa').modal('show');
                }



            });
        };


        $scope.ViewTypeQuestionOptionMapping = function (dd) {
            $scope.ViewMasterTypeQuestionOptionMapping = [];
            var data = {
                "RATMTQ_Id": dd.ratmtQ_Id,
                "MI_Id": $scope.Master_TypeQues_MI_Id
            };

            apiService.create("Rating_Master/ViewTypeQuestionOptionMapping", data).then(function (promise) {
                if (promise !== null) {
                    $scope.ViewMasterTypeQuestionOptionMapping = promise.viewMasterTypeQuestionOptionMapping;
                    if ($scope.ViewMasterTypeQuestionOptionMapping === null || $scope.ViewMasterTypeQuestionOptionMapping.length === 0) {
                        swal("No Records Found");
                    } else {
                        $('#viewdoptions').modal('show');
                    }
                }
            });
        };
        //Akash
        $scope.ViewTypeAuthority = function (dd) {
            $scope.ViewMasterTypeQuestionOptionMapping = [];
            var data = {
                "RATMQEAUT_Id": dd.ratmqeauT_Id,

            };

            apiService.create("Rating_Master/ViewTypeAuthorityData", data).then(function (promise) {
                if (promise !== null) {
                    $scope.viewAuthorities = promise.viewAuthorities;
                    if ($scope.viewAuthorities === null || $scope.viewAuthorities.length === 0) {
                        swal("No Records Found");
                    } else {
                        $('#viewAuthority').modal('show');
                    }
                }
            });
        };


        $scope.DeleteViewTypeQuestionDept = function (d) {

            var data = {
                "HRMD_Id": d.HRMD_Id,
                "RATMTQ_Id": $scope.Temp_RATMTQ_Id,
                "MI_Id": $scope.Master_TypeQues_MI_Id
            }

            swal({
                title: "Are You Sure",
                text: "Do You Want To Delete This Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Rating_Master/DeleteViewTypeQuestionDept", data).then(function (promise) {
                            if (promise.returnmsg === "Mapped") {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Record  Deleted successfully");
                                }
                                else {
                                    swal("Failed To Delete Record");
                                }
                                $state.reload();
                            }

                            $scope.ViewDeptDesginationModal(promise.viewQuestionDeptDesigData);
                        });
                        $state.reload();
                    }
                    else {
                        swal("Record deletion Cancelled");
                    }
                });

        };

        $scope.DeleteViewTypeQuestionDeptDes = function (d, dd) {

            var data = {
                "HRMD_Id": dd.HRMD_Id,
                "RATMTQ_Id": $scope.Temp_RATMTQ_Id,
                "MI_Id": $scope.Master_TypeQues_MI_Id,
                "HRMDES_Id": d.HRMDES_Id,
            }

            swal({
                title: "Are You Sure",
                text: "Do You Want To Delete This Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Rating_Master/DeleteViewTypeQuestionDeptDes", data).then(function (promise) {
                            if (promise.returnmsg === "Mapped") {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Record  Deleted successfully");
                                    $('#viewdeptdesgindetails').modal('hide');
                                }
                                else {
                                    swal("Failed To Delete Record");
                                    $('#viewdeptdesgindetails').modal('hide');
                                }

                            }
                            // $scope.ViewDeptDesginationModal(promise.viewQuestionDeptDesigData);

                        });

                    }
                    else {
                        swal("Record deletion Cancelled");
                        $('#viewdeptdesgindetails').modal('hide');
                    }
                });

        };

        $scope.DeleteOptionRecord = function (dd) {

            var data = {
                "RATMTQO_Id": dd.ratmtqO_Id,
                "RATMTQ_Id": dd.ratmtQ_Id,
                "MI_Id": $scope.Master_TypeQues_MI_Id
            }

            swal({
                title: "Are You Sure",
                text: "Do You Want To Delete This Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Rating_Master/DeleteOptionRecord", data).then(function (promise) {
                            if (promise.returnmsg === "Mapped") {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Record  Deleted successfully");
                                }
                                else {
                                    swal("Failed To Delete Record");
                                }
                                $state.reload();
                            }
                            $scope.ViewMasterTypeQuestionOptionMapping = promise.viewMasterTypeQuestionOptionMapping;
                            //if ($scope.ViewMasterTypeQuestionOptionMapping === null || $scope.ViewMasterTypeQuestionOptionMapping.length === 0) {
                            //    swal("No Records Found");
                            //}
                            $state.reload();
                        });
                    }
                    else {
                        swal("Record deletion Cancelled");
                    }
                });
        };

        $scope.interacted4 = function (field) {
            return $scope.submitted4;
        };


        $scope.clear_fourth_tab = function () {
            $scope.OnChangeMasteTypeQuesInsitution();
        };

        $scope.clear_fifth_tab = function () {
            $scope.OnChangeRatingQuestionMappingInsitution();
        };

        $scope.clear_sixth_tab = function () {
            $scope.OnChangeRatingClassificationInstitution();
        };

        $scope.ViewDeptDesginationModal = function (ViewQuestionDeptDesigData) {
            $scope.Temp_AddUser1 = [];
            $scope.ViewQuestionDeptDesigData = ViewQuestionDeptDesigData;
            if ($scope.ViewQuestionDeptDesigData === null || $scope.ViewQuestionDeptDesigData.length === 0) {
                swal("No Records Found");
            } else {
                angular.forEach($scope.ViewQuestionDeptDesigData, function (dd) {
                    if ($scope.Temp_AddUser1.length === 0) {
                        $scope.Temp_AddUser1.push({ HRMD_Id: dd.hrmD_Id, HRMD_DepartmentName: dd.hrmD_DepartmentName });
                    } else if ($scope.Temp_AddUser1.length > 0) {
                        var count = 0;
                        angular.forEach($scope.Temp_AddUser1, function (d) {
                            if (d.HRMD_Id === dd.hrmD_Id) {
                                count += 1;
                            }
                        });
                        if (count === 0) {
                            $scope.Temp_AddUser1.push({ HRMD_Id: dd.hrmD_Id, HRMD_DepartmentName: dd.hrmD_DepartmentName });
                        }
                    }
                });

                $scope.Temp_Dept_Desgi1 = [];
                angular.forEach($scope.Temp_AddUser1, function (dd) {
                    $scope.Temp_Dept_Desgi1 = [];
                    angular.forEach($scope.ViewQuestionDeptDesigData, function (d) {
                        if (dd.HRMD_Id === d.hrmD_Id) {
                            $scope.Temp_Dept_Desgi1.push({ HRMDES_Id: d.hrmdeS_Id, HRMDES_DesignationName: d.hrmdeS_DesignationName });
                        }
                    });
                    dd.TempDesgin = $scope.Temp_Dept_Desgi1;
                });

                if ($scope.Temp_AddUser1.length > 0) {
                    $('#viewdeptdesgindetails').modal('show');
                    $('#ViewAuthoritydataa').modal('hide');
                }
            }
        }

        //Akash

        $scope.all_check123 = function () {            var checkStatus = $scope.checkall123;            var count = 0;            angular.forEach($scope.GetMasterQuestionData, function (itm) {                itm.selected = checkStatus;                if (itm.selected == true) {                    count += 1;                }                else {                    count = 0;                }            });            $scope.OnChangeQuestion();        }        $scope.isOptionsRequired1223 = function () {            return !$scope.GetMasterQuestionData.some(function (options) {                return options.selected;            });        }        $scope.togchkbx123 = function () {            $scope.checkall123 = $scope.GetMasterQuestionData.every(function (options) {                return options.selected;            });            $scope.OnChangeQuestion();        }




    }
})();
