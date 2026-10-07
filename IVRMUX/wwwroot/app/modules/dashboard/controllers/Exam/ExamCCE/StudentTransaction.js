(function () {
    'use strict';

    angular
        .module('app')
        .controller('StudentTransaction', StudentTransaction);

    StudentTransaction.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter'];

    function StudentTransaction($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {
        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.Onload = function () {
            apiService.getURI("StudentTransaction/Onload/", 1).then(function (promise) {
                $scope.academicYear = promise.academicYear;
                $scope.classList = promise.classList;
                $scope.sectionList = promise.sectionList;

                $scope.examTerms = promise.examTerms;
                $scope.skillsList = promise.skillsList;
                $scope.activitiesList = promise.activitiesList;
            });
        }
        $scope.isOptionsRequired = function () {

            return !$scope.StudentScoreData.some(function (options) {
                return options.checked;
            });
        }
        $scope.submitted = false;
        $scope.StudentScoreData = [];

        $scope.getStudentList = function () {
            
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "ECT_Id": $scope.ECT_Id,
                    "ECACT_Id": $scope.ECACT_Id,
                    "ECS_Id": $scope.ECS_Id,
                    "Skills": $scope.Skills
                }
                apiService.create("StudentTransaction/getStudentList", data).then(function (promise) {
                    if (promise.studentList != null) {
                        $scope.studentList = promise.studentList;
                       
                        $scope.StudentScoreData.length = 0;
                        if ($scope.Skills == 'Skills') {
                            for (var i = 0; i < $scope.studentList.length; i++) {
                                if ($scope.StudentScoreData.length > 0) {
                                    
                                    var rs = $filter('filter')($scope.StudentScoreData, { 'amsT_Id': $scope.studentList[i].amsT_Id })
                                    if (rs.length == 0) {
                                        $scope.StudentScoreData.push({
                                            "amsT_Id": $scope.studentList[i].amsT_Id,
                                            "score": $scope.studentList[i].ecsT_Score,
                                            "areaName": $scope.studentList[i].areaName,
                                            "studentName": $scope.studentList[i].studentName,
                                            "regNumber": $scope.studentList[i].regNumber,
                                            "rollNumber": $scope.studentList[i].rollNumber
                                        });
                                    }
                                }
                                else {
                                    $scope.StudentScoreData.push({
                                        "amsT_Id": $scope.studentList[i].amsT_Id,
                                        "score": $scope.studentList[i].ecsT_Score,
                                        "areaName": $scope.studentList[i].areaName,
                                        "studentName": $scope.studentList[i].studentName,
                                        "regNumber": $scope.studentList[i].regNumber,
                                        "rollNumber": $scope.studentList[i].rollNumber
                                    });
                                }
                            }
                            for (var u = 0; u < $scope.studentList.length; u++) {
                                $scope.studentList[u].ECSACTT_Score = $scope.studentList[u].ecsT_Score;
                            }

                        }
                        else if ($scope.Skills == 'Activities') {
                            for (var i = 0; i < $scope.studentList.length; i++) {
                                if ($scope.StudentScoreData.length > 0) {
                                    
                                    var rs = $filter('filter')($scope.StudentScoreData, { 'amsT_Id': $scope.studentList[i].amsT_Id })
                                    if (rs.length == 0) {
                                        $scope.StudentScoreData.push({
                                            "amsT_Id": $scope.studentList[i].amsT_Id,
                                            "score": $scope.studentList[i].ecsactT_Score,
                                            "areaName": $scope.studentList[i].areaName,
                                            "studentName": $scope.studentList[i].studentName,
                                            "regNumber": $scope.studentList[i].regNumber,
                                            "rollNumber": $scope.studentList[i].rollNumber
                                        });
                                    }
                                }
                                else {
                                    $scope.StudentScoreData.push({
                                        "amsT_Id": $scope.studentList[i].amsT_Id,
                                        "score": $scope.studentList[i].ecsactT_Score,
                                        "areaName": $scope.studentList[i].areaName,
                                        "studentName": $scope.studentList[i].studentName,
                                        "regNumber": $scope.studentList[i].regNumber,
                                        "rollNumber": $scope.studentList[i].rollNumber
                                    });
                                }
                            }
                            for (var u = 0; u < $scope.studentList.length; u++) {
                                $scope.studentList[u].ECSACTT_Score = $scope.studentList[u].ecsactT_Score;
                            }
                        }
                      
                        $scope.presentCountgrid = $scope.StudentScoreData.length;
                       
                        
                        if (promise.areasList != null) {
                            $scope.areasList = promise.areasList;
                       }
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        }
     
        $scope.submitted1 = false;
        $scope.SelectedStudentScore = [];
        $scope.save = function () {
            

            if ($scope.myForm1.$valid) {
                $scope.SelectedStudentScore.length = 0;
                if ($scope.Skills == 'Skills') {
                    for (var i = 0; i < $scope.StudentScoreData.length; i++) {
                        if ($scope.StudentScoreData[i].checked == true) {
                            for (var j = 0; j < $scope.studentList.length; j++) {
                                if ($scope.StudentScoreData[i].amsT_Id == $scope.studentList[j].amsT_Id) {
                                    $scope.SelectedStudentScore.push({ "AMST_Id": $scope.studentList[j].amsT_Id, "ECST_Score": $scope.studentList[j].ECSACTT_Score, "ECSA_Id": $scope.studentList[j].ecsA_Id, "areaName": $scope.studentList[j].areaName, "EMGR_Id": $scope.studentList[j].emgR_Id, "ECST_Id": $scope.studentList[j].ecsT_Id })
                                }
                            }
                        }
                    }
                }
                else if ($scope.Skills == 'Activities') {
                    for (var i = 0; i < $scope.StudentScoreData.length; i++) {
                        if ($scope.StudentScoreData[i].checked == true) {
                            for (var j = 0; j < $scope.studentList.length; j++) {
                                if ($scope.StudentScoreData[i].amsT_Id == $scope.studentList[j].amsT_Id) {
                                    $scope.SelectedStudentScore.push({ "AMST_Id": $scope.studentList[j].amsT_Id, "ECSACTT_Score": $scope.studentList[j].ECSACTT_Score, "ECACTA_Id": $scope.studentList[j].ecactA_Id, "areaName": $scope.studentList[j].areaName, "EMGR_Id": $scope.studentList[j].emgR_Id, "ECSACTT_Id": $scope.studentList[j].ecsactT_Id })
                                }
                            }
                        }
                    }

                    //for (var i = 0; i < $scope.studentList.length; i++) {
                    //    if ($scope.studentList[i].checked == true) {
                    //        for (var j = 0; j < $scope.areasList.length; j++) {
                    //            $scope.SelectedStudentScore.push({ "AMST_Id": $scope.studentList[i].amsT_Id, "ECSACTT_Score": $scope.studentList[i].area[j].ECSACTT_Score, "ECACTA_Id": $scope.areasList[j].ecactA_Id, "areaName": $scope.areasList[j].areaName, "EMGR_Id": $scope.areasList[j].emgR_Id })
                    //        }
                    //    }
                    //}
                }

                var obj = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "ECT_Id": $scope.ECT_Id,
                    "Skills": $scope.Skills,
                    "ECACT_Id": $scope.ECACT_Id,
                    "ECS_Id": $scope.ECS_Id,
                    "SelectedStudentScore": $scope.SelectedStudentScore
                }
                apiService.create("StudentTransaction/saveRecord", obj).then(function (promise) {
                    if (promise.returnVal == 'saved') {
                        swal("Record Saved Successfully");
                        $state.reload();
                    }
                    else if (promise.returnVal == "savingFailed") {
                        swal("Failed to save record");
                    }
                    else {
                        swal("Sorry...something went wrong");
                    }
                });
            }
            else {

                $scope.submitted1 = true;
            }
        }
        $scope.edit = function (data) {
            $scope.SPCCE_Id = data;
            apiService.getURI("StudentTransaction/Edit/", $scope.SPCCE_Id).then(function (promise) {
                $scope.editDetails = promise.editDetails;
                $scope.ASMAY_Id = promise.editDetails[0].asmaY_Id;
                $scope.SPCCME_Id = promise.editDetails[0].spccmE_Id;

                $scope.SPCCMEV_Id = promise.editDetails[0].spccmeV_Id;
                $scope.SPCCE_StartDate = new Date(promise.editDetails[0].spccE_StartDate);
                $scope.SPCCE_StartTime = moment(promise.editDetails[0].spccE_StartTime, 'h:mm a').format();

                $scope.SPCCE_EndDate = new Date(promise.editDetails[0].spccE_EndDate);
                $scope.SPCCE_EndTime = moment(promise.editDetails[0].spccE_EndTime, 'h:mm a').format();
                $scope.SPCCE_Remarks = promise.editDetails[0].spccE_Remarks;
            });
        }
        $scope.deactive = function (data) {
            
            if (data.spccE_ActiveFlag == false) {
                var obj = {
                    "SPCCE_Id": data.spccE_Id,
                    "SPCCE_ActiveFlag": true
                }
            }
            else if (data.spccE_ActiveFlag == true) {
                var obj = {
                    "SPCCE_Id": data.spccE_Id,
                    "SPCCE_ActiveFlag": false
                }
            }
            apiService.create("StudentTransaction/deactivate/", obj).then(function (promise) {
                if (promise.returnVal != '' && promise != null) {
                    swal(promise.returnVal);
                    $state.reload();
                }
                else {
                    swal("Something went wrong");
                }
            });

        }
        $scope.cancel = function () {
            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted1 = function (field1) {
            return $scope.submitted1;
        };

    }
})();




