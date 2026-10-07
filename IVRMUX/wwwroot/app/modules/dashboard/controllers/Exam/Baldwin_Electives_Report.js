(function () {
    'use strict';

    angular
        .module('app')
        .controller('Baldwin_Electives_ReportController', Baldwin_Electives_ReportController);

    Baldwin_Electives_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout'];

    function Baldwin_Electives_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'Baldwin_Electives_Report';

        activate();

        function activate() { }

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != undefined && admfigsettings != null) {
            var logopath = "";
            if (admfigsettings.length > 0) {
                logopath = admfigsettings[0].asC_Logo_Path;
            }
            $scope.imgname = logopath;
        }

        $scope.BindData = function () {
            
            apiService.getDATA("Baldwin_Electives_Report/Getdetails").
       then(function (promise) {

           $scope.year_list = promise.yearlist;
           // $scope.category_list = promise.categorylist;

       })
        };
        $scope.get_categories = function () {
            
            $scope.student_subjs_list = [];
            if ($scope.Type == 'Indi' || $scope.Type=='subj' || $scope.Type=='csw') {
                if ($scope.ASMAY_Id != undefined && $scope.ASMAY_Id != "") { 
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("Baldwin_Electives_Report/get_categories", data).then(function (promise) {
                    $scope.category_list = promise.categorylist;
                    $scope.EMCA_Id = "";
                    $scope.student_list = [];
                   // $scope.student_subjs_list = [];
                })
            }
            }
        };

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.get_groups = function () {
            
            $scope.student_subjs_list = [];
            if ($scope.Type == 'subj' || $scope.Type=='csw') {
                if ($scope.ASMAY_Id != undefined && $scope.ASMAY_Id != "" && $scope.EMCA_Id != undefined && $scope.EMCA_Id != "") {
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "EMCA_Id": $scope.EMCA_Id
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("Baldwin_Electives_Report/get_groups", data).then(function (promise) {
                        $scope.group_list_drp = promise.grouplist;
                        $scope.EMG_Id = "";
                        $scope.student_list = [];
                        //  $scope.student_subjs_list = [];
                        $scope.class_list = promise.classlist;
                        $scope.ASMCL_Id = "";
                    })
                }
            }
        };
        $scope.get_subjects = function () {
            
            $scope.student_subjs_list = [];
            if ($scope.Type == 'subj') {
                if ($scope.ASMAY_Id != undefined && $scope.ASMAY_Id != "" && $scope.EMCA_Id != undefined && $scope.EMCA_Id != "" && $scope.EMG_Id != undefined && $scope.EMG_Id != "")
                {
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "EMCA_Id": $scope.EMCA_Id,
                        "EMG_Id": $scope.EMG_Id
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("Baldwin_Electives_Report/get_subjects", data).then(function (promise) {
                        $scope.subject_list = promise.subjectlist;
                        $scope.ISMS_Id = "";
                        $scope.student_list = [];
                      //  $scope.student_subjs_list = [];
                    })
                }             
            }
        };

        $scope.get_sections = function () {
            
            $scope.student_subjs_list = [];
            if ($scope.Type == 'csw') {
                if ($scope.ASMAY_Id != undefined && $scope.ASMAY_Id != "" && $scope.EMCA_Id != undefined && $scope.EMCA_Id != "" && $scope.ASMCL_Id != undefined && $scope.ASMCL_Id != "") {
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "EMCA_Id": $scope.EMCA_Id,
                        "ASMCL_Id": $scope.ASMCL_Id
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("Baldwin_Electives_Report/get_sections", data).then(function (promise) {
                        $scope.section_list = promise.sectionlist;
                        $scope.ASMS_Id = "";
                        $scope.student_list = [];
                        //  $scope.student_subjs_list = [];
                    })
                }
            }
        };


        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        }
        $scope.get_report = function () {
            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "Type": $scope.Type,
                }
                if ($scope.Type == 'Indi' || $scope.Type == 'subj' ||$scope.Type=='csw') {
                    data.EMCA_Id = $scope.EMCA_Id;
                }
                if ($scope.Type == 'subj') {
                    data.EMG_Id = $scope.EMG_Id;
                    data.ISMS_Id = $scope.ISMS_Id;
                }
                if ($scope.Type == 'csw') {
                    data.ASMCL_Id = $scope.ASMCL_Id;
                    data.ASMS_Id = $scope.ASMS_Id;
                }
                apiService.create("Baldwin_Electives_Report/get_report", data).then(function (promise) {
                    if ($scope.Type == "subj") {
                        promise.grouplist = [];
                        angular.forEach($scope.group_list_drp, function (grp_dp) {
                            if (grp_dp.emG_Id == $scope.EMG_Id) {
                                promise.grouplist.push(grp_dp);
                            }
                        })
                    }
                    if (promise.studentlist != null && promise.grouplist != null && promise.studentlist.length > 0 && promise.grouplist.length > 0) {
                        $scope.student_list = promise.studentlist;
                        $scope.group_list = promise.grouplist;
                        $scope.student_subjs_list = promise.studentsubj_list;

                        angular.forEach($scope.student_list, function (stud) {
                            var temp_array = [];
                            angular.forEach($scope.group_list, function (grp) {
                                var cnt = 0;
                                angular.forEach($scope.student_subjs_list, function (stu_sub) {
                                    if (stud.amsT_Id == stu_sub.amsT_Id && stu_sub.emG_Id == grp.emG_Id) {
                                        cnt += 1;
                                        var alrdy_cnt = 0;
                                        angular.forEach(temp_array, function (subj) {
                                            if (stud.amsT_Id == subj.amsT_Id && subj.emG_Id == grp.emG_Id)
                                            {
                                                alrdy_cnt += 1;
                                                subj.ismS_SubjectName += '<br/>' + stu_sub.ismS_SubjectName;
                                            }
                                        })
                                        if (alrdy_cnt == 0)
                                        {
                                            temp_array.push(stu_sub);
                                        }                                       
                                    }
                                })
                                if (cnt == 0) {
                                    temp_array.push({});
                                }
                            })
                            stud.sub_list = temp_array;
                        })

                    }
                    else {
                        swal("For Selected Details Students Are Not Mapped with Elective Subjects");
                        $scope.clear1();
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.Print = function () {
            var innerContents = document.getElementById("Final_Report").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                  '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportBBPdf.css" />' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        $scope.clear = function () {
            $scope.Type = 'All';
            $scope.clear1();
            //$scope.ASMAY_Id = "";
            //$scope.EMCA_Id = "";
            //$scope.EMG_Id = "";
            //$scope.ISMS_Id = "";
            //$scope.ASMCL_Id = "";
            //$scope.ASMS_Id = "";
            //$scope.category_list = [];
            //$scope.group_list_drp = [];
            //$scope.subject_list = [];
            //$scope.class_list = [];
            //$scope.section_list = [];
            //$scope.student_list = [];
            //$scope.student_subjs_list = [];
            //$scope.group_list = [];
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();
        };
        $scope.clear1 = function () {
            $scope.ASMAY_Id = "";
            $scope.EMCA_Id = "";
            $scope.EMG_Id = "";
            $scope.ISMS_Id = "";
            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            $scope.category_list = [];
            $scope.group_list_drp = [];
            $scope.subject_list = [];
            $scope.class_list = [];
            $scope.section_list = [];
            $scope.student_list = [];
            $scope.student_subjs_list = [];
            $scope.group_list = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };

        $scope.exportToExcel = function (tableIds) {
            
            var exportHref = Excel.tableToExcel(tableIds, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        };


    }
})();
