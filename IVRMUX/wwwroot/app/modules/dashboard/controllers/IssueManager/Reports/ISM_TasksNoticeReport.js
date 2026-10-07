(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_TasksNoticeReportController', ISM_TasksNoticeReportController);
    ISM_TasksNoticeReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter', '$compile'];
    function ISM_TasksNoticeReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter, $compile) {

        $scope.gridview = false;
        var paginationformasters;
        $scope.multidepartment = false;
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

        $scope.disableemp = false;
        $scope.checkallemp = false;
        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();
        $scope.EmpD = true;
        $scope.page = "page";
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 10;
        $scope.loaddata = function () {
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("ISM_MemoNoticeReport/getdetails", pageid).then(function (promise) {
                $scope.get_userEmplist = promise.get_userEmplist;
                $scope.departmentdropdown = promise.departmentList;
                $scope.designationdropdown = promise.designation;
                $scope.empdepartment = promise.empdepartment;
                $scope.empname = promise.empname;
                $scope.roleflag = promise.roleflag;
                if ($scope.departmentdropdown.length > 0) {
                    angular.forEach($scope.departmentdropdown, function (uem) {
                        uem.selected = true;
                    });
                    $scope.departmentselectedAll = true;
                }
                if ($scope.designationdropdown.length > 0) {
                    angular.forEach($scope.designationdropdown, function (uem) {
                        uem.selected = true;
                    });
                    $scope.designationselectedAll = true;
                }
                if ($scope.get_userEmplist.length > 0) {
                    angular.forEach($scope.get_userEmplist, function (uem) {
                        uem.empck = true;
                    });
                    $scope.checkall = true;
                }
            });
        };

        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.filterValue3, function (uem) {
                uem.empck = toggleStatus;
            });
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };
        $scope.togchkbx = function () {
            $scope.checkall = $scope.get_userEmplist.every(function (options) {
                return options.empck;
            });
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };

        //=============================for grid check box selection
        $scope.togglecheckboxAllDept = function (checkallemp) {
            var toggleStatus = checkallemp;
            angular.forEach($scope.DeptdeviationRemarksReport, function (slc) {
                slc.checked = toggleStatus;
            });
            $scope.DeptselectionHeaddetails(checkallemp);
        };

        $scope.togglecheckboxAll = function (checkallemp) {
            var toggleStatus = checkallemp;
            angular.forEach($scope.employeeid, function (slc) {
                slc.checked = toggleStatus;
            });
        };
        $scope.optionToggled = function () {
            $scope.checkallemp = $scope.employeeid.every(function (options) {
                return options.checked;
            });
        };
        $scope.optionToggleddept = function (user) {
            $scope.checkalldept = $scope.DeptdeviationRemarksReport.every(function (options) {
                return options.checked;
            });
            $scope.DeptselectionHeaddetails(user);
        };
        //*************************** Department REPORT *********************//        
        $scope.all_checkdept = function (departmentselectedAll) {
            var toggleStatus = departmentselectedAll;
            angular.forEach($scope.filterValue5, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.searchValueDesg = '';
            $scope.designationselectedAll = false;
            $scope.Deptselectiondetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };

        $scope.togchkbxdept = function (groupType) {
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {
                return itm.selected;
            });
            $scope.searchValueDesg == '';
            $scope.designationselectedAll = false;
            $scope.Deptselectiondetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.searchValueUEM = '';
            $scope.employeeid = [];
            $scope.checkall = false;
            $scope.get_deviationreport = [];
        };

        //*************************** Designation REPORT *********************//        
        $scope.all_checkdesg = function (designationselectedAll) {
            var toggleStatus = designationselectedAll;
            angular.forEach($scope.filterValue1, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.searchValueUEM = '';
            $scope.checkall = false;
            $scope.Desgselectiondetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };

        $scope.togchkbxdesg = function (groupType) {
            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {
                return itm.selected;
            });
            $scope.searchValueUEM = '';
            $scope.checkall = false;
            $scope.Desgselectiondetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };

        $scope.viewresponsetask = function (taskobj) {
            var data = {
                "HRME_Id": taskobj.hrmE_ID,
                "ISMEMN_No": taskobj.ismemN_No
            };
            apiService.create("ISM_PlannerDeptDeviationReport/gettaskdetailsreport", data).
                then(function (promise) {
                    if (promise.get_taskdetails !== null && promise.get_taskdetails.length > 0) {
                        $('#myModalResponse').modal('show');
                        $scope.get_taskdetails = promise.get_taskdetails;
                    }
                    else {
                        $('#myModalResponse').modal('hide');
                        $scope.get_taskdetails = [];
                        swal("No issues pednding for these dates!!");
                    }
                });
        };

        $scope.viewresponsememo = function (taskobj) {
            var data = {
                "enddate": new Date($scope.end_Date).toDateString(),
                "HRME_Id": taskobj.HRME_Id,
                "type": false
            };
            apiService.create("ISM_MemoNoticeReport/gettaskdetails", data).
                then(function (promise) {
                    if (promise.get_taskdetails !== null && promise.get_taskdetails.length > 0) {
                        $scope.get_taskdetails = promise.get_taskdetails;
                    }
                });
        };

        $scope.closeimg = function () {
            $('#myModalResponse').modal('hide');
            $('.modal-backdrop').remove();
        };

        $scope.isOptionsRequired = function () {
            return !$scope.get_userEmplist.some(function (options) {
                return options.empck;
            });
        };

        $scope.searchValueDesg = '';
        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.HRMDES_DesignationName).indexOf(angular.lowercase($scope.searchValueDesg)) >= 0 || angular.lowercase(obj.MI_Name).indexOf(angular.lowercase($scope.searchValueDesg)) >= 0;
        };

        $scope.searchValueDept = '';
        $scope.filterValue4 = function (obj) {
            return angular.lowercase(obj.HRMDC_Name).indexOf(angular.lowercase($scope.searchValueDept)) >= 0;
        };

        $scope.searchValueUEM = '';
        $scope.filterValue2 = function (obj) {
            return angular.lowercase(obj.userEmpName).indexOf(angular.lowercase($scope.searchValueUEM)) >= 0 || angular.lowercase(obj.HRMD_DepartmentName).indexOf(angular.lowercase($scope.searchValueUEM)) >= 0;
        };

        $scope.Depatdeviation = function () {
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            if ($scope.DepD === true) {
                $scope.EmpD = false;
                if ($scope.designationdropdown.length > 0) {
                    angular.forEach($scope.designationdropdown, function (uem) {
                        uem.selected = true;
                    });
                    $scope.designationselectedAll = true;
                }
                $scope.Desgselectiondetails();
                $scope.disableemp = true;
                $scope.employeeid = [];
            }
            else {
                $scope.DeptdeviationRemarksReport = [];
                $scope.disableemp = false;
            }
        };

        $scope.Empdeviation = function () {
            if ($scope.EmpD === true) {
                $scope.DepD = false;
                $scope.disableemp = false;
            }
        };

        $scope.DeptselectionHeaddetails = function (user) {
            $scope.detailedemp = [];
            if (user.checked === true) {
                angular.forEach($scope.DeptdeviationRemarksReport, function (em) {
                    if (em.HRMDC_ID === user.HRMDC_ID) {
                        $scope.detailedemp.push(em);
                    }
                });
            }
            var data = {
                "departmentlist": $scope.detailedemp
            };
            apiService.create("ISM_MemoNoticeReport/DeptselectionHeaddetails", data).then(function (promise) {
                if (promise.departmnthead != null && promise.departmnthead.length > 0) {
                    $scope.departmntheadselect = [];
                    if (promise.departmnthead.length > 0) {
                        $scope.departmnthead = promise.departmnthead;
                        if (promise.departmnthead.length > 1) {
                            $scope.departmntheadselect = promise.departmnthead;
                            $('#myModalswal').modal('show');
                        }
                    }
                }
            });
        };

        //*************************** Deptselectiondetails REPORT *********************//
        $scope.Deptselectiondetails = function () {
            $scope.arrayuserEmp = [];
            $scope.designationdropdown = [];
            $scope.get_userEmplist = [];
            angular.forEach($scope.departmentdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserEmp.push(em);
                }
            });
            var data = {
                "departmentlist": $scope.arrayuserEmp
            };
            apiService.create("ISM_MemoNoticeReport/Deptselectiondetails", data).then(function (promise) {
                if (promise.designation != null && promise.designation.length > 0) {
                    $scope.designationdropdown = promise.designation;
                    if ($scope.designationdropdown.length > 0) {
                        angular.forEach($scope.designationdropdown, function (uem) {
                            uem.selected = true;
                        });
                        $scope.designationselectedAll = true;
                    }
                    $scope.Desgselectiondetails();
                }
                else {
                    $scope.searchValueUEM = '';
                    $scope.employeeid = [];
                    $scope.checkall = false;
                    swal("No Record  Found..... !!");
                }
            });
        };

        //*************************** Desgselectiondetails REPORT *********************//
        $scope.Desgselectiondetails = function () {
            $scope.arrayuserEmp = [];
            $scope.get_userEmplist = [];
            angular.forEach($scope.designationdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserEmp.push(em);
                }
            });
            var data = {
                "designationlist": $scope.arrayuserEmp
            };
            apiService.create("ISM_MemoNoticeReport/Desgselectiondetails", data).then(function (promise) {
                if (promise.get_userEmplist.length > 0) {
                    $scope.get_userEmplist = promise.get_userEmplist;
                    if ($scope.get_userEmplist.length > 0) {
                        angular.forEach($scope.get_userEmplist, function (uem) {
                            uem.empck = true;
                        });
                        $scope.checkall = true;
                    }
                }
                else {
                    $scope.searchValueUEM = '';
                    $scope.employeeid = [];
                    $scope.checkall = false;
                    swal("No Record  Found..... !!");
                }
            });
        };

        $scope.Report = function () {
            if ($scope.myForm.$valid) {
                $scope.arrayuserEmp = [];
                $scope.arraydepartment = []; $scope.employeeid = [];
                $scope.arraydesignation = []; $scope.get_deviationreport = []; 
                angular.forEach($scope.get_userEmplist, function (em) {
                    if (em.empck === true) {
                        $scope.arrayuserEmp.push(em.HRME_Id);
                    }
                });
                angular.forEach($scope.departmentdropdown, function (em) {
                    if (em.selected === true) {
                        $scope.arraydepartment.push(em.HRMDC_ID);
                    }
                });
                angular.forEach($scope.designationdropdown, function (em) {
                    if (em.selected === true) {
                        $scope.arraydesignation.push(em.HRMDES_Id);
                    }
                });

                var data = {
                    "startdate": new Date($scope.start_Date).toDateString(),
                    "enddate": new Date($scope.end_Date).toDateString(),
                    "employeelist": $scope.arrayuserEmp,
                    "departmentlist": $scope.arraydepartment,
                    "designationlist": $scope.arraydesignation,
                    "Type": "Task_Notice"
                };

                apiService.create("ISM_MemoNoticeReport/getReport", data).then(function (promise) {
                    if (promise.memonoticereport != null && promise.memonoticereport.length > 0) {
                        //
                        $scope.get_deviationreport = promise.memonoticereport;
                        angular.forEach($scope.get_deviationreport, function (dev) {
                            if ($scope.employeeid.length === 0) {
                                $scope.employeeid.push(dev);
                            } else if ($scope.employeeid.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.employeeid, function (emp) {
                                    if (emp.HRME_Id === dev.HRME_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.employeeid.push(dev);
                                }
                            }
                        });                      
                        angular.forEach($scope.employeeid, function (ddd) {
                            $scope.templist = [];                           
                            angular.forEach($scope.get_deviationreport, function (dd) {
                                if (dd.HRME_Id === ddd.HRME_Id) {                                   
                                    $scope.templist.push(dd);
                                }
                            });
                            ddd.plannerdetails = $scope.templist;
                           
                        });



                        //

                    }
                    else {
                        
                        $scope.get_deviationreport = [];
                        swal("No Record  Found..... !!");

                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.clearreport = function () {
            $scope.get_deviationreport = [];
            $scope.employeeid = [];
            $scope.DeptdeviationRemarksReport = [];
        };

        //*************************** MEMO,NOTICE,NOTIFICATION,EMAIL SENDING *********************//
        $scope.empemailmemo = function (type) {
            $scope.arrayuserEmp = [];
            var completeddate = true;
            if ($scope.DepD === true) {
                if ($scope.departmnthead.length == 1) {
                    angular.forEach($scope.departmnthead, function (em) {
                        $scope.arrayuserEmp.push(em);
                    });
                }
                else if ($scope.departmntheadselect.length > 1) {
                    angular.forEach($scope.departmntheadselect, function (em) {
                        if (em.checked == true) {
                            $scope.arrayuserEmp.push(em);
                        }
                    });
                }
            }
            else if ($scope.EmpD === true) {
                angular.forEach($scope.employeeid, function (em) {
                    if (em.checked === true) {
                        if (em.comp_Date === null) {
                            completeddate = false;
                            swal("Please Select Complete By Date for the Employee " + em.employeename);
                        }
                        else {
                            em.comp_Date = new Date(em.comp_Date).toDateString(),
                                $scope.arrayuserEmp.push(em);
                        }
                    }
                });
            }
            var Deviationtype = " ";
            if ($scope.DepD === true) {
                Deviationtype = "Dept";
            }
            else if ($scope.EmpD === true) {
                Deviationtype = "Emp";
            }
            if (completeddate === true) {
                if ($scope.arrayuserEmp.length > 0) {
                    var commutype = "";
                    if (type === "Memo") {
                        commutype = "Memo";
                    }
                    else if (type === "Notice") {
                        commutype = "Notice";
                    }
                    else if ($scope.Email === true) {
                        commutype = "Email";
                    }
                    else if ($scope.Notif === true) {
                        commutype = "Notif";
                    }
                    swal({
                        title: "Are you sure",
                        text: "Do you want to send " + commutype + " ?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes",
                        cancelButtonText: "Cancel!",
                        closeOnConfirm: true,
                        closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                var data = {
                                    "communicationfalg": commutype,
                                    "communicationmsg": $scope.messgaeboxdata,
                                    "EmplistforMNNE": $scope.arrayuserEmp,
                                    "startdate": new Date($scope.start_Date).toDateString(),
                                    "enddate": new Date($scope.end_Date).toDateString(),
                                    "Deviationreporttype": Deviationtype,
                                    "noofdays": $scope.noofdays,
                                    "type": true
                                };
                                apiService.create("ISM_MemoNoticeReport/empcommunication", data).then(function (promise) {
                                    if (promise.message === "success") {
                                        if ($scope.Email === true || $scope.Notif === true) {
                                            $('#myModal').modal('hide');
                                            $scope.messgaeboxdata = "";
                                        }
                                        if ($scope.Notif === true) {
                                            swal("Notification Sent Successfully!!");
                                        }
                                        else {
                                            $state.reload();
                                            swal(commutype + " Sent Successfully!!");
                                        }
                                    }
                                    else {
                                        swal("No Record  Found..... !!");
                                    }
                                });
                            }
                            else {
                                //$state.reload();
                            }

                        });

                }
                else {
                    swal("Select Any Employee..!!");
                }
            };
        };

        //VIEW MEMO NOTICE
        $scope.viewempemailmemo = function (ismemN_ID) {
            var data = {
                "ISMTPLTA_Id": ismemN_ID
            };

            apiService.create("ISM_MemoNoticeReport/gettaskdetails", data).then(function (promise) {
                if (promise.viewdetails != null && promise.viewdetails > 0) {
                    $state.viewdetails = promise.viewdetails;
                    $('#myModal').modal('show');
                }
                else {
                    $('#myModal').modal('hide');
                    swal("No Details..... !!");
                }
            });


            $scope.arrayuserEmp = [];
            angular.forEach($scope.employeeid, function (em) {
                if (em.checked === true && em.HRME_Id === empid) {
                    em.comp_Date = new Date(em.comp_Date).toDateString(),
                        $scope.arrayuserEmp.push(em);
                }
            });
            var Deviationtype = " ";
            if ($scope.DepD === true) {
                Deviationtype = "Dept";
            }
            else if ($scope.EmpD === true) {
                Deviationtype = "Emp";
            }
            var commutype = "";
            if (type === "Memo") {
                commutype = "Memo";
            }
            else if (type === "Notice") {
                commutype = "Notice";
            }
            var data = {
                "communicationfalg": commutype,
                "communicationmsg": $scope.messgaeboxdata,
                "EmplistforMNNE": $scope.arrayuserEmp,
                "startdate": new Date($scope.start_Date).toDateString(),
                "enddate": new Date($scope.end_Date).toDateString(),
                "Deviationreporttype": Deviationtype,
                "noofdays": $scope.noofdays,
                "type": false
            };
            apiService.create("ISM_MemoNoticeReport/gettaskdetails", data).then(function (promise) {
                if (promise.message !== "") {
                    $('#myModalmemo').modal('show');
                    var e1 = angular.element(document.getElementById("test"));
                    $compile(e1.html(promise.html))(($scope));
                }
                else {
                    var e1 = angular.element(document.getElementById("test"));
                    $compile(e1.html(""))(($scope));
                    $('#myModalmemo').modal('hide');
                    swal("No Pending Issues Found..... !!");
                }
            });
        };

        $scope.closeviewmodal = function () {
            var e1 = angular.element(document.getElementById("test"));
            $compile(e1.html(""))(($scope));
        };

        $scope.printData = function () {
            var innerContents = document.getElementById("ExcellDeviation").innerHTML;           
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        //$scope.exportToExcel = function (export_id) {
        //    var exportHref = Excel.tableToExcel(export_id, 'printDeviation');
        //    $timeout(function () {
        //        location.href = exportHref;
        //    }, 100);
        //};



        $scope.exportToExcel = function (export_id) {
            $scope.stdate = $filter('date')($scope.start_Date, 'dd-MM-yyyy');
            $scope.end = $filter('date')($scope.end_Date, 'dd-MM-yyyy');
            var excelname = 'DEVIATION REPORT ' + $scope.stdate + ' TO ' + $scope.end;
            excelname = excelname.toUpperCase() + '.xls';
            var printSectionId = export_id;
            var exportHref = Excel.tableToExcel(printSectionId, $scope.routename);
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.Clear = function () {
            //$scope.get_deviationreport = "";
            //$scope.get_userEmplist = "";
            //$scope.checkall = "";
            //$scope.start_Date = "";
            //$scope.end_Date = "";
            //$scope.loaddata();
            $state.reload();
        };

        (function () {
            $("span.fa").on("click", function () {
                var $span = $(this),
                    $parentTr = $span.closest("tr");

                //remove
                if ($span.hasClass("fa-rotate-90")) {
                    $span.removeClass("fa-rotate-90");
                    $parentTr

                        .next()
                        .children("td")
                        .animate({ padding: 0 })
                        .wrapInner("<div />")
                        .children()
                        .slideUp(function () {
                            var $tr = $(this).closest("tr");

                            // $tr
                            // 	.prev("tr")
                            // 	.find(".active")
                            // 	.removeClass("active");
                            $tr.remove();
                            $parentTr.removeClass("tr-selected");
                        });

                    return;
                }

                //add
                $span.addClass("fa-rotate-90");
                if ($parentTr.next().hasClass("tr-detail")) return;
                $parentTr.addClass("tr-selected");
                // 	.eq(1)
                // 	.addClass("active");
                $("#tr-detail")
                    .clone()
                    .removeClass("hidden")
                    .insertAfter($parentTr)
                    .children("td")
                    .animate()
                    .wrapInner("<div style='display:none'/>")
                    .children()
                    .slideDown();
            });
        })();
    }
})();

