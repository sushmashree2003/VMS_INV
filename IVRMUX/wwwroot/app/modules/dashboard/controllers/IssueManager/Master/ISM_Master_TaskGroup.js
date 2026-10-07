(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_Master_TaskGroupController', ISM_Master_TaskGroupController);
    ISM_Master_TaskGroupController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function ISM_Master_TaskGroupController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache) {
        var paginationformasters;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.obj = {};
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();
        $scope.depdes = false;

        //==============task load
        $scope.loaddata = function () {
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("MastersModule/getdetails_taskgroup", pageid).then(function (promise) {

                $scope.searchValue1 = "";
                $scope.searchValue = "";
                $scope.get_groupname = promise.get_groupname;
                $scope.task_grop_list = promise.task_grop_list;
                $scope.presentCountgrid1 = promise.task_grop_list.length;
                $scope.etr = false;
            });
        };

        //==============task grop
        $scope.onprojectChange = function () {
            var data = {
                "ISMMTGRP_Id": $scope.ISMMTGRP_Id
            }
            apiService.create("MastersModule/getdept", data).then(function (promise) {
                $scope.dept_list = promise.dept_list;
                $scope.usercheck = "";
            })
        }
        //==============task view
        $scope.task_view = function (qq) {
            var data = {
                "ISMMTGRP_Id": qq.ismmtgrP_Id
            }
            apiService.create("MastersModule/task_view", data).then(function (promise) {
                $scope.task_view_list = promise.task_view_list;
            })
        }
        //==============task show
        $scope.show_data = function () {
            $scope.deptarray = [];
            angular.forEach($scope.dept_list, function (aa) {
                if (aa.selectedd2 == true) {
                    $scope.deptarray.push({ HRMD_Id: aa.hrmD_Id })
                }
            })
            if ($scope.myForm.$valid) {

                var data = {
                    deptarray: $scope.deptarray
                }
                apiService.create("MastersModule/show_tasklist", data).then(function (promise) {
                    $scope.task_list = promise.task_list;
                    $scope.presentCountgrid = promise.task_list.length;
                    $scope.all = "";
                })
            }
            else {

                $scope.submitted = true;
            }
        }
        //==============check
        $scope.all_check = function () {
            var checkStatus2 = $scope.usercheck;
            angular.forEach($scope.dept_list, function (itm) {
                itm.selectedd2 = checkStatus2;
            });
        };
        $scope.toggleAll = function () {
            var checkStatus2 = $scope.all;
            angular.forEach($scope.task_list, function (itm) {
                itm.checkedvalue = checkStatus2;
            });
        };

        $scope.optionToggled = function () {
            $scope.all = $scope.task_list.every(function (itm) { return itm.checkedvalue; });
        };

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.dept_list.every(function (itm) { return itm.selectedd2; });
        };
        //===============cancel
        $scope.cancel = function () {
            $state.reload();
        }
        //===================edit
        $scope.edit = function (qq) {
            var data = {
                "ISMMTGRP_Id": qq.ismmtgrP_Id
            }
            apiService.create("MastersModule/task_edit", data).then(function (promise) {
                $scope.etr = true;
                $scope.ISMMTGRP_TaskGroupName = promise.task_edit[0].ismmtgrP_TaskGroupName;
                $scope.ISMMTGRP_Id = promise.task_edit[0].ismmtgrP_Id;
                $scope.task_edit_dept = promise.task_edit_dept;
                $scope.dept_list = promise.dept_list;
                $scope.task_list_temp = promise.task_list_temp;
                $scope.task_list = promise.task_list;
                $scope.presentCountgrid = promise.task_list.length;

                if ($scope.task_edit_dept.length > 0) {
                    angular.forEach($scope.task_edit_dept, function (ss) {
                        angular.forEach($scope.dept_list, function (qq) {
                            if (ss.hrmD_Id === qq.hrmD_Id) {
                                qq.selectedd2 = true;
                            }
                        });
                    });
                }
                if ($scope.task_list_temp.length > 0) {

                    if ($scope.task_list.length > 0) {
                        angular.forEach($scope.task_list_temp, function (ss) {
                            angular.forEach($scope.task_list, function (qq) {
                                if (ss.ISMTCR_Id === qq.ISMTCR_Id) {
                                    qq.checkedvalue = true;
                                }
                            });
                        });
                    }

                    else {
                        $scope.task_list = $scope.task_list_temp;
                        $scope.checkedvalue = true;
                    }





                }



            })
        }

        //==============task save
        $scope.save_data = function () {
            $scope.task_listarray = [];
            angular.forEach($scope.task_list, function (aa) {
                if (aa.checkedvalue == true) {
                    $scope.task_listarray.push({ ISMTCR_Id: aa.ISMTCR_Id, ISMTCR_TGOrder: aa.ISMTCR_TGOrder, HRMD_Id: aa.HRMD_Id })
                }
            });
            var data = {
                task_listarray: $scope.task_listarray,
                "ISMMTGRP_Id": $scope.ISMMTGRP_Id
            }
            apiService.create("MastersModule/save_taskgrpdata", data).then(function (promise) {
                if (promise.returnval == "Success") {
                    swal('Record Saved Successfully.')
                    $state.reload();
                }
                else {
                    swal('Record Not Saved Successfully.')
                    $state.reload();
                }

            })

        }
        //===================
        $scope.exportToExcel = function () {
            debugger;
            var blob = new Blob([document.getElementById('printP').innerHTML], {
                type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
            });

            saveAs(blob, "Approval Report.xls");
        };


        $scope.togchkbxuu = function () {
            $scope.checkalluu = $scope.get_userEmplist.every(function (options) {
                return options.empck;
            });

            $scope.onEmployeechange();

        };


        $scope.isOptionsRequired = function () {
            return !$scope.dept_list.some(function (options) {
                return options.selectedd2;
            });
        };

        //*************************** Approval Report *********************//        
        $scope.approvalReport = function () {
            $scope.plannerid = [];
            if ($scope.myForm.$valid) {
                //if ($scope.obj.HRME_Id != null && $scope.obj.HRME_Id != "" && $scope.obj.HRME_Id != undefined)
                //{
                $scope.temp_Planner = [];
                $scope.searchValue = "";
                $scope.get_emp_dailyreport = "";

                $scope.arrayuserEmp = [];
                angular.forEach($scope.get_userEmplist, function (em) {
                    if (em.empck === true) {
                        $scope.arrayuserEmp.push(em);
                    }
                });

                if ($scope.end_Date == undefined) {
                    $scope.end_Date = $scope.start_Date;
                }
                var data = {
                    arrayuserEmp: $scope.arrayuserEmp,
                    "Fromdate": new Date($scope.start_Date).toDateString(),
                    "Todate": new Date($scope.end_Date).toDateString(),
                    "feedbackflg": $scope.feedbackflg,

                };
                apiService.create("MastersModule/getempdetailsReport", data).then(function (promise) {
                    $scope.plannerid = [];
                    $scope.hide_n = false;
                    $scope.feed = false;
                    if (promise.get_emp_dailyreport.length > 0) {
                        $scope.get_emp_dailyreport = promise.get_emp_dailyreport;
                        angular.forEach($scope.get_emp_dailyreport, function (dev) {
                            if ($scope.plannerid.length === 0) {
                                $scope.plannerid.push({ plannedby: dev.Employee_name, HRME_Id: dev.HRME_Id, MI_Id: dev.MI_Id });
                            }
                            else if ($scope.plannerid.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.plannerid, function (emp) {
                                    if (emp.HRME_Id === dev.HRME_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.plannerid.push({ plannedby: dev.Employee_name, HRME_Id: dev.HRME_Id, MI_Id: dev.MI_Id });
                                }
                            }
                        });

                        angular.forEach($scope.plannerid, function (ddd) {
                            $scope.templist = [];
                            angular.forEach($scope.get_emp_dailyreport, function (dd) {
                                if (dd.HRME_Id === ddd.HRME_Id) {
                                    $scope.templist.push(dd);
                                }
                            });
                            ddd.plannerdetails = $scope.templist;
                        });
                    }

                });

            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.printData = function () {
            $scope.hide_n = false;
            var innerContents = "";
            innerContents = document.getElementById("printP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.Clear = function () {
            $state.reload();
            //$scope.departmentselectedAll = false;
            //$scope.designationselectedAll = false;
            //$scope.get_approvalreport = "";
            //$scope.get_Plannerlist = "";
            //$scope.checkall = "";
            //$scope.start_Date = "";
            //$scope.end_Date = "";
            //$scope.obj.HRME_Id = "";
            //$scope.loaddata();
        };

        //$scope.feedbackclick = function () {
        //    if ($scope.feedbact == '') {
        //        $scope.feed = false;
        //    }
        //    else {
        //        $scope.feed = true;
        //    }
        //}

        $scope.Sendfeedback = function (aa) {
            $scope.feedbackdtl = [];
            $scope.plannerid1 = $scope.plannerid;
            angular.forEach($scope.plannerid, function (qq) {
                angular.forEach(qq.plannerdetails, function (ss) {
                    if (ss.feedbact !== undefined) {
                        $scope.feedbackdtl.push({ ISMDRF_RCV_HRME_Id: ss.HRME_Id, ISMTCR_Id: ss.ISMTCR_Id, ISMDRF_FeedBack: ss.feedbact, MI_Id_r: ss.MI_Id })
                    }
                })

            })

            var data = {
                feedbackdtl: $scope.feedbackdtl
            }
            apiService.create("MastersModule/send_feedback", data).then(function (fromise) {
                if (fromise.send_flg == "Success") {
                    swal("Feedback send successfully...")
                    $scope.plnrtemp = [];
                    $scope.templist = [];
                    $scope.plnrtemp = $scope.plannerid;


                    angular.forEach($scope.plannerid, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.get_emp_dailyreport, function (dd) {
                            if (dd.HRME_Id === ddd.HRME_Id) {
                                $scope.templist.push({ Employee_name: dd.Employee_name, HRME_Id: dd.HRME_Id, ISMDRPT_Remarks: dd.ISMDRPT_Remarks, ISMDRPT_Status: dd.ISMDRPT_Status, ISMTCR_Id: dd.ISMTCR_Id, ISMTCR_TaskNo: dd.ISMTCR_TaskNo, ISMTCR_Title: dd.ISMTCR_Title, ISMTPL_Id: dd.ISMTPL_Id, feedbact: "" });
                            }
                        });
                        ddd.plannerdetails = $scope.templist;
                    });

                }
                else {
                    swal("Feedback Not send successfully...")
                    $scope.feedbact = '';

                }
            });
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

    }
})();

