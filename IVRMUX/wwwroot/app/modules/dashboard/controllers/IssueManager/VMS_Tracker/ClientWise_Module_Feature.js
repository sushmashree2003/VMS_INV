
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClientWise_Module_FeatureController', ClientWise_Module_FeatureController);
    ClientWise_Module_FeatureController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter'];
    function ClientWise_Module_FeatureController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {

        $scope.obj = {};
        $scope.optionflag = "Item";
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //===================================

        $scope.coladmission = [{
            title: "SLNO",
            template: "<span class='row-number'></span>", width: "70px"
        },
        {
            name: 'HeadName', field: 'HeadName', title: 'Head Name', width: "100px"
        },
        {
            name: 'counts', field: 'counts', title: 'Count', width: "100px"
        },
        //{
        //    name: 'TotalActiveStudents', field: 'TotalActiveStudents', title: 'Total Active Students', width: "100px"
        //},
        //{
        //    name: 'TotalDeactiveStudents', field: 'TotalDeactiveStudents', title: 'Total Deactive Students', width: "100px"
        //},

        //{
        //    name: 'TotalLeftStudents', field: 'TotalLeftStudents', title: 'Total Left Students', width: "170%"
        //},
        //{
        //    name: 'TransferCertificateCount', field: 'TransferCertificateCount', title: 'Transfer Certificate', width: "100px"
        //},
        //{
        //    name: 'StudyCertificateCount', field: 'StudyCertificateCount', title: 'SC Count', width: "150%"
        //},
        //{
        //    name: 'ConductCertificateCount', field: 'ConductCertificateCount', title: 'Conduct Certificate ', width: "100px"
        //},
        //{
        //    name: 'BonefiedCertificateCount', field: 'BonefiedCertificateCount', title: 'Bonefied Certificate', width: "100px"
        //},
        //{
        //    name: 'CharacterCertificateCount', field: 'CharacterCertificateCount', title: 'Character Certificate ', width: "100px"
        //},
        //{
        //    name: 'TotalMobNoMissionCount', field: 'TotalMobNoMissionCount', title: 'Total Mob No Mission ', width: "100px"
        //},
        //{
        //    name: 'TotalEmailNoMissionCount', field: 'TotalEmailNoMissionCount', title: 'Total Email No Mission', width: "100px"
        //},
        //{
        //    name: 'TotalusernamecreationMissionCount', field: 'TotalusernamecreationMissionCount', title: 'Total username creation Mission', width: "100px"
        //},
        //{
        //    name: 'DOBCertificateCount', field: 'DOBCertificateCount', title: 'DOB Certificate', width: "100px"
        //},
        //{
        //    name: 'TotalClassTeacher', field: 'TotalClassTeacher', title: 'Total Class Teacher', width: "100px"
        //},
        //{
        //    name: 'ClassTeachermappingcount', field: 'ClassTeachermappingcount', title: 'Class Teachermapping ', width: "100px"
        //}

        ];
        $scope.colvm = [{
            title: "SLNO",
            template: "<span class='row-number'></span>", width: "80px"
        },
        {
            name: 'totalVisitor', field: 'totalVisitor', title: 'Total Visitor', width: "100px"
        },
        {
            name: 'emailcount', field: 'emailcount', title: 'Email Count', width: "100px"
        }

        ];
        $scope.collib = [{
            title: "SLNO",
            template: "<span class='row-number'></span>", width: "80px"
        },
        {
            name: 'STDISSUECNT', field: 'STDISSUECNT', title: 'STD ISSUE', width: "100px"
        },
        {
            name: 'STDRETURNCNT', field: 'STDRETURNCNT', title: 'STD RETURN', width: "100px"
        },
        {
            name: 'STFISSUECNT', field: 'STFISSUECNT', title: 'STF ISSUE', width: "100px"
        },
        {
            name: 'STFRETURNCNT', field: 'STFRETURNCNT', title: 'STF RETURN', width: "100px"
        },
        {
            name: 'SMSCOUNT', field: 'SMSCOUNT', title: 'SMS COUNT', width: "100px"
        },
        {
            name: 'EMAILCOUNT', field: 'EMAILCOUNT', title: 'EMAIL COUNT', width: "100px"
        },

        ];

        $scope.colBirthday_Month_Stu = [{
            title: "SLNO",
            template: "<span class='row-number'></span>", width: "80px"
        },
        {
            name: 'CYear', field: 'CYear', title: 'Year', width: "100px"
        },
        {
            name: 'IVRM_Month_Name', field: 'IVRM_Month_Name', title: 'Month', width: "100px"
        },
        {
            name: 'STUBCount', field: 'STUBCount', title: 'STU BD Count', width: "120px"
        },
        {
            name: 'STUSMSSentCount', field: 'STUSMSSentCount', title: 'SMS Count', width: "100px"
        },
        {
            name: 'STUEmailSentCount', field: 'STUEmailSentCount', title: 'Email COUNT', width: "100px"
        },

        ];
        $scope.colBirthday_Month_Stf = [{
            title: "SLNO",
            template: "<span class='row-number'></span>", width: "80px"
        },
        {
            name: 'CYear', field: 'CYear', title: 'Year', width: "100px"
        },
        {
            name: 'IVRM_Month_Name', field: 'IVRM_Month_Name', title: 'Month', width: "100px"
        },
        {
            name: 'EMPBCount', field: 'EMPBCount', title: 'Emp BD Count', width: "120px"
        },
        {
            name: 'STUSMSSentCount', field: 'STUSMSSentCount', title: 'SMS Count', width: "100px"
        },
        {
            name: 'STUEmailSentCount', field: 'STUEmailSentCount', title: 'Email COUNT', width: "100px"
        },

        ];
        $scope.colBirthday_year_Stu = [{
            title: "SLNO",
            template: "<span class='row-number'></span>", width: "80px"
        },


        {
            name: 'STUBCount', field: 'STUBCount', title: 'STU BD Count', width: "120px"
        },
        {
            name: 'STUSMSSentCount', field: 'STUSMSSentCount', title: 'SMS Count', width: "100px"
        },
        {
            name: 'STUEmailSentCount', field: 'STUEmailSentCount', title: 'Email COUNT', width: "100px"
        },

        ];
        $scope.colBirthday_yesr_Stf = [{
            title: "SLNO",
            template: "<span class='row-number'></span>", width: "80px"
        },
        {
            name: 'CYear', field: 'CYear', title: 'Year', width: "100px"
        },

        {
            name: 'EMPBCount', field: 'EMPBCount', title: 'Emp BD Count', width: "120px"
        },
        {
            name: 'STUSMSSentCount', field: 'STUSMSSentCount', title: 'SMS Count', width: "100px"
        },
        {
            name: 'STUEmailSentCount', field: 'STUEmailSentCount', title: 'Email COUNT', width: "100px"
        },

        ];
        //---------------------
        $scope.colCOE_Month_Stu = [{
            title: "SLNO",
            template: "<span class='row-number'></span>", width: "80px"
        },
        {
            name: 'CYear', field: 'CYear', title: 'Year', width: "100px"
        },
        {
            name: 'IVRM_Month_Name', field: 'IVRM_Month_Name', title: 'Month', width: "100px"
        },
        {
            name: 'STUECount', field: 'STUECount', title: 'STUCount', width: "120px"
        },
        {
            name: 'STUCount', field: 'STUCount', title: 'STUCount', width: "100px"
        },
        {
            name: 'STUSMSSentCount', field: 'STUSMSSentCount', title: 'STUCount', width: "100px"
        },
        {
            name: 'STUEmailSentCount', field: 'STUEmailSentCount', title: 'Email COUNT', width: "100px"
        },

        ];
        $scope.colCOE_year_Stu = [{
            title: "SLNO",
            template: "<span class='row-number'></span>", width: "80px"
        },

        {
            name: 'CYear', field: 'CYear', title: 'Year', width: "100px"
        },
        {
            name: 'STUECount', field: 'STUECount', title: 'STUCount', width: "120px"
        },
        {
            name: 'STUCount', field: 'STUCount', title: 'STUCount', width: "100px"
        },
        {
            name: 'STUSMSSentCount', field: 'STUSMSSentCount', title: 'STUCount', width: "100px"
        },
        {
            name: 'STUEmailSentCount', field: 'STUEmailSentCount', title: 'Email COUNT', width: "100px"
        },


        ];
        $scope.colCOE_Month_Stf = [{
            title: "SLNO",
            template: "<span class='row-number'></span>", width: "80px"
        },
        {
            name: 'CYear', field: 'CYear', title: 'Year', width: "100px"
        },
        {
            name: 'IVRM_Month_Name', field: 'IVRM_Month_Name', title: 'Month', width: "100px"
        },
        {
            name: 'EMPCount', field: 'EMPCount', title: 'Emp Count', width: "120px"
        },
        {
            name: 'STSMSSentCount', field: 'STSMSSentCount', title: 'SMS Count', width: "100px"
        },
        {
            name: 'STEmailSentCount', field: 'STEmailSentCount', title: 'Email COUNT', width: "100px"
        },

        ];
        $scope.colCOE_yesr_Stf = [{
            title: "SLNO",
            template: "<span class='row-number'></span>", width: "80px"
        },
        {
            name: 'CYear', field: 'CYear', title: 'Year', width: "100px"
        },

        {
            name: 'EMPCount', field: 'EMPCount', title: 'Emp BD Count', width: "120px"
        },
        {
            name: 'STSMSSentCount', field: 'STSMSSentCount', title: 'SMS Count', width: "100px"
        },
        {
            name: 'STEmailSentCount', field: 'STEmailSentCount', title: 'Email COUNT', width: "100px"
        },

        ];
        $scope.colFO_yesr_Stf = [{
            title: "SLNO",
            template: "<span class='row-number'></span>", width: "80px"
        },
        {
            name: 'CYear', field: 'CYear', title: 'Year', width: "100px"
        },

        {
            name: 'IVRM_Month_Name', field: 'IVRM_Month_Name', title: 'Month', width: "120px"
        },
        {
            name: 'EMPCount', field: 'EMPCount', title: 'Emp Count', width: "100px"
        },
        {
            name: 'WorkingDaysCount', field: 'WorkingDaysCount', title: 'Working Days Count', width: "100px"
        },
        {
            name: 'PresentCount', field: 'PresentCount', title: 'Present Days Count', width: "100px"
        },

        ];

        $scope.colPreAdmission = [{
            title: "SLNO",
            template: "<span class='row-number'></span>", width: "80px"
        },
        {
            name: 'CYear', field: 'CYear', title: 'Year', width: "100px"
        },

        {
            name: 'IVRM_Month_Name', field: 'IVRM_Month_Name', title: 'Month', width: "120px"
        },
        {
            name: 'EMPCount', field: 'EMPCount', title: 'Emp Count', width: "100px"
        },
        {
            name: 'WorkingDaysCount', field: 'WorkingDaysCount', title: 'Working Days Count', width: "100px"
        },
        {
            name: 'PresentCount', field: 'PresentCount', title: 'Present Days Count', width: "100px"
        },

        ];
        $scope.colExam = [{
            title: "SLNO",
            template: "<span class='row-number'></span>", width: "80px"
        },
        {
            name: 'EME_Name', field: 'EME_Name', title: 'Exam Name', width: "100px"
        },

        {
            name: 'Passcount', field: 'Passcount', title: 'STU Pass', width: "120px"
        },
        {
            name: 'Failcount', field: 'Failcount', title: 'STU Fail', width: "100px"
        },
        {
            name: 'Absentcount', field: 'Absentcount', title: 'Absent count', width: "100px"
        },
        {
            name: 'Medicalcount', field: 'Medicalcount', title: 'Medical count', width: "100px"
        },
        {
            name: 'Sportscount', field: 'Sportscount', title: 'Sports count', width: "100px"
        },
        {
            name: 'ODcount', field: 'ODcount', title: 'On Duty Count', width: "100px"
        },
        {
            name: 'HallTicketCount', field: 'HallTicketCount', title: 'Hall Ticket Count', width: "100px"
        },
        {
            name: 'SMSCount', field: 'SMSCount', title: 'SMS Count', width: "100px"
        },
        {
            name: 'EmailCount', field: 'EmailCount', title: 'Email Count', width: "100px"
        },

        ];

        $scope.colTransport = [{
            title: "SLNO",
            template: "<span class='row-number'></span>", width: "80px"
        },
        {
            name: 'MFIELDS', field: 'MFIELDS', title: 'Head Name', width: "100px"
        },

        {
            name: 'MCOUNT', field: 'MCOUNT', title: 'Counts', width: "120px"
        },
       

        ];
        //==========================================================

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            $scope.moduleflg = false;
            var id = 1;
            apiService.getURI("ClientWise_Module_Feature/getloaddata", id).
                then(function (promise) {

                    $scope.clientlist = promise.clientlist;


                });
        };

        $scope.onrdochange = function (optionflag) {
            $scope.get_itemreportdetails = "";
            $scope.get_itemreport = "";
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag);
        };

        //========================ITEM
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.clientlist.every(function (itm) {
                return itm.itemck;
            });
        };
        $scope.isOptionsRequiredI = function () {

            return !$scope.clientlist.some(function (options) {
                return options.itemck;
            });

        };
        $scope.all_checkI = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.clientlist, function (itm) {
                itm.itemck = toggleStatus;
            });
        };


        $scope.getmodule = function () {
            var data = {
                "ISMMCLT_Id": $scope.IVRM_MI_Id.ismmclT_Id,
            };
            apiService.create("ClientWise_Module_Feature/getmodule", data).
                then(function (promise) {
                    $scope.modulelist = [];
                    if (promise.modulelist != null && promise.modulelist.length > 0) {
                        $scope.modulelist = promise.modulelist;
                    }
                    else {
                        swal("No Record Found...!!");
                        $scope.modulelist = "";
                    }
                });
        };

        $scope.getmoduleflag = function () {
            $scope.modulid = $scope.IVRMM_Id.ivrmM_Id;
            $scope.moduleflage = "";
            angular.forEach($scope.modulelist, function (qq) {
                if ($scope.modulid == qq.ivrmM_Id) {
                    $scope.moduleflage = qq.ivrmM_Flag;
                    $scope.getreportdetails = [];
                }
            });

            if ($scope.moduleflage == 'Birthday' || $scope.moduleflage == 'COE') {
                $scope.moduleflg = false;
            }
            else {
                $scope.moduleflg = true;
            }
        };


        //==================================Item Report
        $scope.submitted = false;
        $scope.onreport = function () {
            if ($scope.myForm.$valid) {
                $scope.getreportdetails = [];
                if ($scope.moduleflage == 'Birthday' || $scope.moduleflage == 'COE') {
                    var data = {
                        "ISMMCLT_Id": $scope.IVRM_MI_Id.ismmclT_Id,
                        "IVRMM_Id": $scope.IVRMM_Id.ivrmM_Id,
                        "Flag": $scope.Flag,
                        "Type": $scope.Type,
                        "moduleflage": $scope.moduleflage,
                    };
                }
                else if ($scope.moduleflage == 'Admission') {
                    $scope.fromdate1 = $filter('date')($scope.fromdate, 'yyyy-MM-dd');
                    $scope.todate1 = $filter('date')($scope.todate, 'yyyy-MM-dd');
                    var data = {
                        "ISMMCLT_Id": $scope.IVRM_MI_Id.ismmclT_Id,
                        "IVRMM_Id": $scope.IVRMM_Id.ivrmM_Id,
                        "fromdate": $scope.fromdate1,
                        "todate": $scope.todate1,
                        "moduleflage": $scope.moduleflage
                    }
                }
                else if ($scope.moduleflage == 'FO') {
                    var data = {
                        "ISMMCLT_Id": $scope.IVRM_MI_Id.ismmclT_Id,
                        "IVRMM_Id": $scope.IVRMM_Id.ivrmM_Id,
                        "Flag": 'Staff',
                        "Type": 'Monthly',
                        "moduleflage": $scope.moduleflage

                    }
                }
                else if ($scope.moduleflage == 'Library') {
                    $scope.fromdate1 = $filter('date')($scope.fromdate, 'yyyy-MM-dd');
                    $scope.todate1 = $filter('date')($scope.todate, 'yyyy-MM-dd');
                    var data = {
                        "ISMMCLT_Id": $scope.IVRM_MI_Id.ismmclT_Id,
                        "IVRMM_Id": $scope.IVRMM_Id.ivrmM_Id,
                        "fromdate": $scope.fromdate1,
                        "todate": $scope.todate1,
                        "moduleflage": $scope.moduleflage
                    }
                }
                else if ($scope.moduleflage == 'VM') {
                    $scope.fromdate1 = $filter('date')($scope.fromdate, 'yyyy-MM-dd');
                    $scope.todate1 = $filter('date')($scope.todate, 'yyyy-MM-dd');
                    var data = {
                        "ISMMCLT_Id": $scope.IVRM_MI_Id.ismmclT_Id,
                        "IVRMM_Id": $scope.IVRMM_Id.ivrmM_Id,
                        "fromdate": $scope.fromdate1,
                        "todate": $scope.todate1,
                        "moduleflage": $scope.moduleflage
                    }
                }
                else if ($scope.moduleflage == 'Preadmission') {
                    $scope.fromdate1 = $filter('date')($scope.fromdate, 'yyyy-MM-dd');
                    $scope.todate1 = $filter('date')($scope.todate, 'yyyy-MM-dd');
                    var data = {
                        "ISMMCLT_Id": $scope.IVRM_MI_Id.ismmclT_Id,
                        "IVRMM_Id": $scope.IVRMM_Id.ivrmM_Id,
                        "fromdate": $scope.fromdate1,
                        "todate": $scope.todate1,
                        "moduleflage": $scope.moduleflage
                    }
                }
                else if ($scope.moduleflage == 'Exam') {
                    $scope.fromdate1 = $filter('date')($scope.fromdate, 'yyyy-MM-dd');
                    $scope.todate1 = $filter('date')($scope.todate, 'yyyy-MM-dd');
                    var data = {
                        "ISMMCLT_Id": $scope.IVRM_MI_Id.ismmclT_Id,
                        "IVRMM_Id": $scope.IVRMM_Id.ivrmM_Id,
                        "fromdate": $scope.fromdate1,
                        "todate": $scope.todate1,
                        "moduleflage": $scope.moduleflage
                    }
                }
                else if ($scope.moduleflage == 'Fees') {
                    $scope.fromdate1 = $filter('date')($scope.fromdate, 'yyyy-MM-dd');
                    $scope.todate1 = $filter('date')($scope.todate, 'yyyy-MM-dd');
                    var data = {
                        "ISMMCLT_Id": $scope.IVRM_MI_Id.ismmclT_Id,
                        "IVRMM_Id": $scope.IVRMM_Id.ivrmM_Id,
                        "fromdate": $scope.fromdate1,
                        "todate": $scope.todate1,
                        "moduleflage": $scope.moduleflage
                    }
                }
                apiService.create("ClientWise_Module_Feature/getreport", data).
                    then(function (promise) {
                        if (promise.getreportdetails.length > 0) {
                            $scope.getreportdetails = promise.getreportdetails;
                            if ($scope.moduleflage == 'Admission') {
                                $scope.fileName = "admissioncount.xlsx";
                                $scope.fileNamepdf = "admissioncount.pdf";
                                $scope.col = $scope.coladmission;

                                $(document).ready(function () {
                                    initGridall();
                                });

                            }
                            else if ($scope.moduleflage == 'VM') {
                                $scope.fileName = "VisitorManagement.xlsx";
                                $scope.fileNamepdf = "VisitorManagement.pdf";
                                $scope.col = $scope.colvm;

                                $(document).ready(function () {
                                    initGridall();
                                });

                            }
                            else if ($scope.moduleflage == 'Library') {
                                $scope.fileName = "LibraryCount.xlsx";
                                $scope.fileNamepdf = "LibraryCount.pdf";
                                $scope.col = $scope.collib;

                                $(document).ready(function () {
                                    initGridall();
                                });

                            }
                            else if ($scope.moduleflage == 'Birthday') {
                                $scope.fileName = "BirthdayCount.xlsx";
                                $scope.fileNamepdf = "BirthdayCount.pdf";
                                if ($scope.Flag == 'Staff') {
                                    if ($scope.Type == 'Yearly') {
                                        $scope.col = $scope.colBirthday_yesr_Stf;
                                    }
                                    else if ($scope.Type == 'Monthly') {
                                        $scope.col = $scope.colBirthday_Month_Stf;
                                    }

                                }
                                else if ($scope.Flag == 'Student') {
                                    if ($scope.Type == 'Yearly') {
                                        $scope.col = $scope.colBirthday_year_Stu;
                                    }
                                    if ($scope.Type == 'Monthly') {
                                        $scope.col = $scope.colBirthday_Month_Stu;
                                    }

                                }


                                $(document).ready(function () {
                                    initGridall();
                                });

                            }
                            else if ($scope.moduleflage == 'COE') {
                                $scope.fileName = "COECount.xlsx";
                                $scope.fileNamepdf = "COECount.pdf";
                                if ($scope.Flag == 'Staff') {
                                    if ($scope.Type == 'Yearly') {
                                        $scope.col = $scope.colCOE_yesr_Stf;
                                    }
                                    else if ($scope.Type == 'Monthly') {
                                        $scope.col = $scope.colCOE_Month_Stf;
                                    }

                                }
                                else if ($scope.Flag == 'Student') {
                                    if ($scope.Type == 'Yearly') {
                                        $scope.col = $scope.colCOE_year_Stu;
                                    }
                                    if ($scope.Type == 'Monthly') {
                                        $scope.col = $scope.colCOE_Month_Stu;
                                    }

                                }


                                $(document).ready(function () {
                                    initGridall();
                                });

                            }
                            else if ($scope.moduleflage == 'FO') {
                                $scope.fileName = "FOCount.xlsx";
                                $scope.fileNamepdf = "FOCount.pdf";
                                if ($scope.Flag == 'Staff') {
                                    if ($scope.Type == 'Monthly') {
                                        $scope.col = $scope.colFO_yesr_Stf;
                                    }

                                }



                                $(document).ready(function () {
                                    initGridall();
                                });

                            }
                            else if ($scope.moduleflage == 'Preadmission') {
                                $scope.reportdatelist = [];
                                $scope.reportdatelist = promise.getreportdetails;
                                if ($scope.reportdatelist.length > 0 && $scope.reportdatelist != null) {

                                    $scope.grid_flag = true;
                                    $scope.IsHiddendown = true;
                                    $scope.total_Reg = $scope.reportdatelist[0].total_Reg;
                                    $scope.total_filled = $scope.reportdatelist[0].total_filled;
                                    $scope.total_notfilled = $scope.reportdatelist[0].total_notfilled;
                                    $scope.total_regpaid = $scope.reportdatelist[0].total_regpaid;
                                    $scope.offline_pay = $scope.reportdatelist[0].offline_pay;
                                    $scope.total_notregpaid = $scope.reportdatelist[0].total_notregpaid;
                                    $scope.total_transferd = $scope.reportdatelist[0].total_transferd;
                                    $scope.sent_email_count = $scope.reportdatelist[0].sent_email_count;
                                    $scope.sent_sms_count = $scope.reportdatelist[0].sent_sms_count;
                                    $scope.missing_photo = $scope.reportdatelist[0].missing_photo;
                                    $scope.missing_no = $scope.reportdatelist[0].missing_no;
                                    $scope.missing_email = $scope.reportdatelist[0].missing_email;

                                    //$scope.tot_absent = promise.reportdatelist[0].tot_absent;
                                    //$scope.DOB_Certificate_count = promise.reportdatelist[0].DOB_Certificate_count;
                                    $scope.designation = "Implementation Engineer";
                                    $scope.today = new Date();
                                    angular.forEach($scope.month_name, function (itm) {
                                        if (itm.ivrM_Month_Id == $scope.monthmodel) {
                                            $scope.monthmodelvalue = itm.ivrM_Month_Name
                                        }
                                    });
                                    angular.forEach($scope.acayyearbind, function (itm) {
                                        if (itm.asmaY_Id == $scope.academicyr) {
                                            $scope.acayearnow = itm.asmaY_Year
                                        }
                                    });

                                    //COUNT
                                    $scope.feegraphseries1 = [];
                                    $scope.feegraphseries2 = [];
                                    $scope.feegraphseries3 = [];
                                    $scope.feegraphseries4 = [];
                                    $scope.feegraphseries5 = [];

                                    //SMS
                                    $scope.feegraphseries6 = [];
                                    $scope.feegraphseries7 = [];
                                    $scope.feegraphseries8 = [];
                                    $scope.feegraphseries9 = [];
                                    $scope.feegraphseries10 = [];

                                    //Email
                                    $scope.feegraphseries11 = [];
                                    $scope.feegraphseries12 = [];
                                    $scope.feegraphseries13 = [];
                                    $scope.feegraphseries14 = [];
                                    $scope.feegraphseries15 = [];


                                    $scope.feegraphseries1.push({ label: 'Total Registered', "y": $scope.total_Reg })
                                    $scope.feegraphseries2.push({ label: 'Total Filled', "y": $scope.total_filled })
                                    $scope.feegraphseries3.push({ label: 'Reg. not Filled', "y": $scope.total_notfilled })
                                    $scope.feegraphseries4.push({ label: 'Reg Payment Done', "y": $scope.total_regpaid })
                                    $scope.feegraphseries5.push({ label: 'Reg Payment Not Done', "y": $scope.total_notregpaid })
                                    $scope.feegraphseries5.push({ label: 'Total Transferred', "y": $scope.total_transferd })




                                    console.log($scope.feegraphseries1);


                                    chart = new CanvasJS.Chart("rangeBarChat");

                                    chart.options.axisX = { interval: 1, labelFontSize: 12 };
                                    chart.options.axisY = { labelFontSize: 12 };

                                    // chart.options.title = { text: "Fruits sold in First & Second Quarter" };

                                    var series1 = { //dataSeries - first quarter
                                        type: "column",
                                        name: "Total Registered",
                                        showInLegend: true
                                    };



                                    var series2 = { //dataSeries - second quarter
                                        type: "column",
                                        name: "Total Filled",
                                        showInLegend: true
                                    };

                                    var series3 = { //dataSeries - third quarter
                                        type: "column",
                                        name: "Reg. not Filled",
                                        showInLegend: true
                                    };
                                    var series4 = { //dataSeries - fourth quarter
                                        type: "column",
                                        name: "Reg Payment Done",
                                        showInLegend: true
                                    };
                                    var series5 = { //dataSeries - fifth quarter
                                        type: "column",
                                        name: "Reg Payment Not Done",
                                        showInLegend: true
                                    };
                                    var series6 = { //dataSeries - fifth quarter
                                        type: "column",
                                        name: "Total Transferred",
                                        showInLegend: true
                                    };


                                    chart.options.data = [];
                                    chart.options.data.push(series1);
                                    chart.options.data.push(series2);
                                    chart.options.data.push(series3);
                                    chart.options.data.push(series4);
                                    chart.options.data.push(series5);
                                    chart.options.data.push(series6);


                                    series1.dataPoints = $scope.feegraphseries1;
                                    series2.dataPoints = $scope.feegraphseries2;
                                    series3.dataPoints = $scope.feegraphseries3;
                                    series4.dataPoints = $scope.feegraphseries4;
                                    series5.dataPoints = $scope.feegraphseries5;
                                    series6.dataPoints = $scope.feegraphseries6;
                                    chart.render();


                                    $scope.export_flag = true;
                                }
                                else {
                                    swal("Record Not Found");
                                    $scope.grid_flag = true;
                                }

                                //$scope.fileName = "PreadmissionCount.xlsx";
                                //$scope.fileNamepdf = "PreadmissionCount.pdf";
                                //$scope.col = $scope.colPreAdmission;



                                //$(document).ready(function () {
                                //    initGridall();
                                //});

                            }
                            else if ($scope.moduleflage == 'Exam') {
                                $scope.fileName = "ExamCount.xlsx";
                                $scope.fileNamepdf = "ExamCount.pdf";
                                $scope.col = $scope.colExam;



                                $(document).ready(function () {
                                    initGridall();
                                });

                            }
                            else if ($scope.moduleflage == 'Transport') {
                                $scope.fileName = "TransportCount.xlsx";
                                $scope.fileNamepdf = "TransportCount.pdf";
                                $scope.col = $scope.colTransport;



                                $(document).ready(function () {
                                    initGridall();
                                });

                            }
                        }
                        else {
                            swal("No Record Found...!!");
                            $scope.getreportdetails = "";
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        function initGridall() {
            $('#grid567').empty();
            //gridall =
            $("#grid567").kendoGrid({
                toolbar: ["excel", "pdf"],
                excel: {
                    fileName: $scope.fileName,
                    proxyURL: "",
                    filterable: true,
                    allPages: true
                },
                pdf: {
                    avoidLinks: true,
                    landscape: true,
                    repeatHeaders: true,
                    fileName: $scope.fileNamepdf,
                    allPages: true
                },
                dataSource: {
                    data: $scope.getreportdetails,
                    pageSize: 10,
                },
                sortable: true,
                pageable: true,
                groupable: false,
                filterable: true,
                columnMenu: true,
                reorderable: true,
                resizable: true,
                columns: $scope.col,
                dataBound: function () {
                    var pagenum = this.dataSource.page();
                    var pageitms = this.dataSource.pageSize()
                    var rows = this.items();
                    $(rows).each(function () {
                        var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                        var rowLabel = $(this).find(".row-number");
                        $(rowLabel).html(index);
                    });
                }

            }).data("kendoGrid");
        }
        //==========================

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cancel = function () {
            $state.reload();
        };
        //======================================Print & Export to Excel
        $scope.exportToExcel = function (export_id0, export_id1) {
            if ($scope.term === 1) {
                var exportHref = Excel.tableToExcel(export_id0, 'printexcel_0');
                $timeout(function () {
                    location.href = exportHref;
                }, 100);
            }
            else {
                var exportHref1 = Excel.tableToExcel(export_id1, 'printexcel_1');
                $timeout(function () {
                    location.href = exportHref1;
                }, 100);
            }

        };
        $scope.printData = function () {
            if ($scope.term === 1) {

                var innerContents = document.getElementById("printItem_0").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            else {

                var innerContents1 = document.getElementById("printItem_1").innerHTML;
                var popupWinindow1 = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents1 + '</html>');
                popupWinindow1.document.close();
            }
        };


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
    }
})();