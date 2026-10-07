(function () {
    'use strict';
    angular
        .module('app')
        .controller('trainingevaluationController', trainingevaluationController)

    trainingevaluationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function trainingevaluationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {

        $scope.report = false;
        $scope.catreport = false;
        $scope.obj = {};
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length !== 0 && ivrmcofigsettings.length !== null && ivrmcofigsettings.length !== undefined) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        $scope.typelist = [];
        $scope.mappedquestiondeetails = [];
        $scope.mappedoptiondeetails = [];

        $scope.temparray = [];
        $scope.tempquestionarray = [];
        $scope.tempoptionarray = [];

        $scope.BindData = function () {

            var data = {
                "Flag": 'Staff'
            };

            apiService.create("FeedbackSchoolGeneralTransaction/getFeedback_Staff_Transactiondetails", data).then(function (promise) {
                $scope.count = promise.count;
                $scope.emplist = promise.emplist;
                $scope.griddata = promise.getstaffdetails;
                if (promise.typelistload !== null && promise.typelistload.length > 0) {
                    $scope.typelistload = promise.typelistload;
                }
            });
        };

        $scope.getfeedback = function () {

            var data = {
                "FMTY_Id": $scope.FMTY_Id
            };

            apiService.create("FeedbackSchoolGeneralTransaction/getfeedback", data).then(function (promise) {
                if (promise !== null) {
                    $scope.count = promise.count;

                    if ($scope.count > 0) {
                        $scope.temparray = [];
                        $scope.tempquestionarray = [];
                        $scope.tempoptionarray = [];
                        swal("Feedback Is Already Given");
                        return;
                    }

                    if ((promise.typelist !== null && promise.typelist.length > 0)
                        && (promise.mappedquestiondeetails !== null && promise.mappedquestiondeetails.length > 0)
                        && (promise.mappedoptiondeetails !== null && promise.mappedoptiondeetails.length > 0)) {

                        $scope.temparray = [];
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
                                $scope.temparray.push({ FMTY_Id: dd.fmtY_Id, FMTY_FeedbackTypeName: dd.fmtY_FeedbackTypeName, ques: $scope.tempquestionarray, opt: $scope.tempoptionarray });

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
                                $scope.temparray.push({ FMTY_Id: dd.fmtY_Id, FMTY_FeedbackTypeName: dd.fmtY_FeedbackTypeName, ques: $scope.tempquestionarray, opt: $scope.tempoptionarray});
                            });
                        }

                        console.log($scope.temparray);

                    } else {
                        $scope.temparray = [];
                        $scope.tempquestionarray = [];
                        $scope.tempoptionarray = [];
                        swal("No Records Found");
                    }
                } else {
                    $scope.temparray = [];
                    $scope.tempquestionarray = [];
                    $scope.tempoptionarray = [];
                    swal("Something Went Wrong Kidndly Contact Administrator");
                }

            });
        };



        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.searchValue = '';

        $scope.students = [];
        $scope.catreport = false;
        $scope.submitted = false;

        // $scope.user = {};

        $scope.onreport = function (user) {
            if ($scope.myForm.$valid) {

                var data = {
                    temp: $scope.temparray,
                    "HRME_Id": $scope.obj.hrmE_Id.hrmE_Id
                };
                if ($scope.count > 0) {

                    swal({
                        title: "For Today's Date Already Feedback Is Submitted?",
                        text: "Do You Want To Submit Again?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Submit It Again",
                        cancelButtonText: "Cancel",
                        closeOnConfirm: false,
                        closeOnCancel: false
                    },

                        function (isConfirm) {
                            if (isConfirm) {
                                apiService.create("FeedbackSchoolGeneralTransaction/saveFeedback_Staff_Transactiondetails", data).
                                    then(function (promise) {

                                        if (promise !== null) {
                                            if (promise.returnval === true) {
                                                swal("Record Saved Successfully");
                                            } else {
                                                swal("Failed To Save Record");
                                            }
                                        } else {
                                            swal("Something Went Wrong Kindly Contact Administrator");
                                        }
                                        $state.reload();
                                    });
                            }
                            else {
                                swal("Cancel");
                                $state.reload();
                            }
                        });

                } else {

                    apiService.create("FeedbackSchoolGeneralTransaction/saveFeedback_Staff_Transactiondetails", data).
                        then(function (promise) {

                            if (promise !== null) {

                                if (promise.returnval === true) {

                                    swal("Record Saved Successfully");
                                } else {
                                    swal("Failed To Save Record");
                                }
                            } else {
                                swal("Something Went Wrong Kindly Contact Administrator");
                            }
                            $state.reload();
                        });
                }
            } else {
                $scope.submitted = true;
            }
        };

        $scope.editdata = function (user) {

            var data = {
                "FSTTR_Id": user.FSTTR_Id
            };


            apiService.create("FeedbackSchoolGeneralTransaction/certificateFeedback_Staff_Transactiondetails/", data).then(function (promise) {

                if (promise.getstaffdetails.length > 0) {
                    $scope.editdetails = promise.getstaffdetails;
                    $('#feedbackdetails').modal('show');
                    $scope.FSSTR_FeedbackDate = promise.getstaffdetails[0].FSSTR_FeedbackDate;
                    $scope.HRME_EmployeeFirstName = promise.getstaffdetails[0].HRME_EmployeeFirstName;                
                    $scope.HRMD_DepartmentName = promise.getstaffdetails[0].HRMD_DepartmentName;              
                    $scope.MI_Logo = promise.getstaffdetails[0].MI_Logo;
                } else {
                    swal("No Records Found");
                }
            });
        };

        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printDeviation").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
    }

})();