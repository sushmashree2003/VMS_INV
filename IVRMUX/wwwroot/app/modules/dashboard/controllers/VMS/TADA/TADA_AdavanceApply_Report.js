(function () {
    'use strict';
    angular.module('app').controller('TADA_AdavanceApply_ReportController', TADA_AdavanceApply_ReportController);
    TADA_AdavanceApply_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', 'Excel', '$timeout'];
    function TADA_AdavanceApply_ReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter, Excel, $timeout) {

        var paginationformasters = 10;
        $scope.obj = {};
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();
        $scope.report = false;
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian; todate
        };
        $scope.onpageload = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.searchValue = "";
            var pageid = 2;
            $scope.flagone = false;
            $scope.flagtwo = false;
            $scope.flagtwos = false;
            $scope.flagones = false;            
            apiService.getURI("TADA_Reports/getalldata", pageid).then(function (promise) {
                $scope.get_userEmplist = promise.get_userEmplist;
                $scope.departmentdropdown = promise.departmentList;
                $scope.designationdropdown = promise.designation;
                $scope.empdepartment = promise.empdepartment;
                $scope.deviationpercentage = promise.deviationpercentage;
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
        /* DEPARTMENT SELECTION */
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
            $scope.searchValueDesg = '';
            $scope.designationselectedAll = false;
            
            $scope.DeptdeviationRemarksReport = [];
            $scope.searchValueUEM = '';
            $scope.employeeid = [];
            $scope.checkall = false;
            $scope.get_deviationreport = [];
            $scope.Deptselectiondetails();
        };
        /* DESIGNATION SELECTION */
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
        $scope.otherdaydatechange = function () {

            $scope.get_Report = null;
        };
        /* EMPLOYEE SELECTION */
        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.get_userEmplist, function (uem) {
                uem.empck = toggleStatus;
            });
        };
        $scope.togchkbx = function () {
            $scope.checkall = $scope.get_userEmplist.every(function (options) {
                return options.empck;
            });
        };
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

            apiService.create("TADA_Reports/Deptselectiondetails", data).then(function (promise) {
                if (promise.designation !== null && promise.designation.length > 0) {
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
                    $scope.get_Report = null;
                }
            });
        };

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
            apiService.create("TADA_Reports/Desgselectiondetails", data).then(function (promise) {
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
                    $scope.get_Report = null;
                }
            });
        };
        /* DEVIATION REPORT */
        $scope.getReport = function () {
            if ($scope.myForm.$valid) {
                $scope.headreport = [];
                var data = "";
                $scope.deptlist = [];
                $scope.desglist = [];
                $scope.emplist = [];
                angular.forEach($scope.departmentdropdown, function (d) {
                    if (d.selected === true) {
                        $scope.deptlist.push({ HRMDC_ID: d.HRMDC_ID });
                    }
                });
                angular.forEach($scope.designationdropdown, function (d) {
                    if (d.selected === true) {
                        $scope.desglist.push({ HRMDES_Id: d.HRMDES_Id });
                    }
                });

                angular.forEach($scope.get_userEmplist, function (d) {
                    if (d.empck === true) {
                        $scope.emplist.push({ HRME_Id: d.HRME_Id });
                    }
                });

                data = {
                    "From_Date": new Date($scope.start_Date).toDateString(),
                    "To_Date": new Date($scope.end_Date).toDateString(),
                    "RdoPunch": $scope.rdopunch,
                    "departmentlist": $scope.deptlist,
                    "designationlist": $scope.desglist,
                    "emplist": $scope.emplist,
                    "optionflag": $scope.optionflag
                };
                apiService.create("TADA_Reports/getReport", data).then(function (promise) {
                    $scope.imagenew = promise.mI_Name;
                    if (promise.getReport !== null && promise.getReport.length > 0) {
                        $scope.get_Report = promise.getReport;
                        $scope.presentCountgrid = $scope.get_Report.length;
                    }
                    else {
                        swal("No Record Found");
                        $scope.get_Report = null;
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.getView = function (user) {
            $scope.emplYoeeName = user.emplYoeeName;
            $scope.hrmE_EmployeeCode = user.hrmE_EmployeeCode;
            var VTADAA_Id = 0; var VTADAAA_Id = 0;
            if (user.vtadaA_Id != null &&  user.vtadaA_Id > 0) {
                VTADAA_Id = user.vtadaA_Id;
            }
            if (user.vtadaaA_Id !=null && user.vtadaaA_Id > 0) {
                VTADAAA_Id = user.vtadaaA_Id;
            }                       
                var data = {
                    "VTADAAA_Id": VTADAAA_Id,
                    "VTADAA_Id": VTADAA_Id,
                    "RdoPunch": $scope.rdopunch
                };
                apiService.create("TADA_Reports/Viewdetails", data).then(function (promise) {
                    $scope.ViewDetails = promise.veiwdetails;
                    if ($scope.ViewDetails.length !== null && $scope.ViewDetails.length > 0) {
                        $scope.fromdate = $scope.ViewDetails[0].VTADAA_FromDate;
                        $scope.todate = $scope.ViewDetails[0].VTADAA_ToDate;
                        $scope.departuretime = moment($scope.ViewDetails[0].VTADAA_DepartureTime, 'HH:mm a').format();
                        $scope.ArrivalTime = moment($scope.ViewDetails[0].VTADAA_ArrivalTime, 'HH:mm a').format();
                        var date2 = new Date($scope.fromdate);
                        var date1 = new Date($scope.todate);
                        var timeDiff = Math.abs(date1.getTime() - date2.getTime());
                        var DateDifference = Math.ceil(timeDiff / (1000 * 3600 * 24) + 1);
                        $scope.datediff = DateDifference;
                        if ($scope.ViewDetails[0].VTADAAD_ExpenditureHead != "" && $scope.ViewDetails[0].VTADAAD_ExpenditureHead != null) {
                            $scope.flagone = true;
                            $scope.flagtwo = false
                        }
                        else {

                            $scope.flagtwo = true;
                            $scope.flagone = false;
                        }

                        $scope.Getsancationed_List = [];

                        angular.forEach($scope.ViewDetails, function (dd) {
                            if ($scope.Getsancationed_List.length === 0) {
                                $scope.Getsancationed_List.push({
                                    VTADAAA_SanctioningLevel: dd.VTADAAA_SanctioningLevel,
                                    ApprovedBy: dd.ApprovedBy
                                });
                            } else if ($scope.Getsancationed_List.length > 0) {
                                var count = 0;

                                angular.forEach($scope.Getsancationed_List, function (d) {
                                    if (d.VTADAAA_SanctioningLevel === dd.VTADAAA_SanctioningLevel) {
                                        count += 1;
                                    }
                                });
                                if (count === 0) {
                                    $scope.Getsancationed_List.push({
                                        VTADAAA_SanctioningLevel: dd.VTADAAA_SanctioningLevel,
                                        ApprovedBy: dd.ApprovedBy
                                    });
                                }
                            }
                        });

                        $scope.getapproveddetails = [];
                        angular.forEach($scope.Getsancationed_List, function (dd) {
                            $scope.getapproveddetails = [];
                            angular.forEach($scope.ViewDetails, function (d) {
                                if (dd.VTADAAA_SanctioningLevel === d.VTADAAA_SanctioningLevel) {
                                    $scope.getapproveddetails.push(d);
                                }
                            });
                            dd.reportdetails = $scope.getapproveddetails;
                        });
                        $('#viewDeatils').modal('show');

                    }
                    else {
                       
                        swal("TADA  Authorization Request Is Not  Approved !");
                    }
                    //===ADAvance Details In  Tada application===
                    $scope.ViewDetailss=[]
                    $scope.ViewDetailss = promise.veiwdetailstt
                    if ($scope.ViewDetailss.length != null && $scope.ViewDetailss.length > 0) {
                        if ($scope.ViewDetailss[0].VTADAAAD_ExpenditureHead != null && $scope.ViewDetailss[0].VTADAAAD_ExpenditureHead != "") {
                            $scope.flagones = true;
                            $scope.flagtwos = false;
                        }
                        else {

                            $scope.flagtwos = true;
                            $scope.flagones = false;
                        }

                        $scope.Getsancationed_Listss = [];

                        angular.forEach($scope.ViewDetailss, function (dd) {
                            if ($scope.Getsancationed_Listss.length === 0) {
                                $scope.Getsancationed_Listss.push({
                                    VTADAAAA_SanctioningLevel: dd.VTADAAAA_SanctioningLevel,
                                    ApprovedBy: dd.ApprovedBy
                                });
                            } else if ($scope.Getsancationed_Listss.length > 0) {
                                var count = 0;

                                angular.forEach($scope.Getsancationed_Listss, function (d) {
                                    if (d.VTADAAAA_SanctioningLevel === dd.VTADAAAA_SanctioningLevel) {
                                        count += 1;
                                    }
                                });
                                if (count === 0) {
                                    $scope.Getsancationed_Listss.push({
                                        VTADAAAA_SanctioningLevel: dd.VTADAAAA_SanctioningLevel,
                                        ApprovedBy: dd.ApprovedBy
                                    });
                                }
                            }
                        });

                        $scope.getapproveddetailsss = [];
                        angular.forEach($scope.Getsancationed_Listss, function (dd) {
                            $scope.getapproveddetailsss = [];
                            angular.forEach($scope.ViewDetailss, function (d) {
                                if (dd.VTADAAAA_SanctioningLevel === d.VTADAAAA_SanctioningLevel) {
                                    $scope.getapproveddetailsss.push(d);
                                }
                            });
                           
                            dd.reportdetailss = $scope.getapproveddetailsss;
                        });
                       

                    }
                    
                    ///======consolited
                    $scope.consolited = promise.consolited;

                });                      
        }       //adavance
        $scope.getViewA = function (user) {
            $scope.emplYoeeName = user.emplYoeeName;
            $scope.hrmE_EmployeeCode = user.hrmE_EmployeeCode;
            var VTADAAA_Id = user.vtadaaA_Id
            if (VTADAAA_Id > 0) {
                var data = {
                    "RdoPunch": $scope.rdopunch,
                    "VTADAAA_Id": VTADAAA_Id
                };
                apiService.create("TADA_Reports/Viewdetails", data).then(function (promise) {
                    $scope.ViewDetails = promise.veiwdetailstt;
                    if ($scope.ViewDetails.length != null && $scope.ViewDetails.length > 0) {
                        $scope.fromdate = $scope.ViewDetails[0].VTADAAA_FromDate;
                        $scope.todate = $scope.ViewDetails[0].VTADAAA_ToDate;
                             $scope.departuretime = moment($scope.ViewDetails[0].VTADAAA_DepartureTime, 'HH:mm a').format();
                        $scope.ArrivalTime = moment($scope.ViewDetails[0].VTADAAA_ArrivalTime, 'HH:mm a').format();
                        var date2 = new Date($scope.fromdate);
                        var date1 = new Date($scope.todate);
                        var timeDiff = Math.abs(date1.getTime() - date2.getTime());
                        var DateDifference = Math.ceil(timeDiff / (1000 * 3600 * 24) + 1);
                        $scope.datediff = DateDifference;

                        if ($scope.ViewDetails[0].VTADAAAD_ExpenditureHead != null && $scope.ViewDetails[0].VTADAAAD_ExpenditureHead != "") {
                            $scope.flagone = true;
                            $scope.flagtwo = false;
                        }
                        else {

                            $scope.flagtwo = true;
                            $scope.flagone = false;
                        }

                        $scope.Getsancationed_List = [];

                        angular.forEach($scope.ViewDetails, function (dd) {
                            if ($scope.Getsancationed_List.length === 0) {
                                $scope.Getsancationed_List.push({
                                    VTADAAAA_SanctioningLevel: dd.VTADAAAA_SanctioningLevel,
                                    ApprovedBy: dd.ApprovedBy
                                });
                            } else if ($scope.Getsancationed_List.length > 0) {
                                var count = 0;

                                angular.forEach($scope.Getsancationed_List, function (d) {
                                    if (d.VTADAAAA_SanctioningLevel === dd.VTADAAAA_SanctioningLevel) {
                                        count += 1;
                                    }
                                });
                                if (count === 0) {
                                    $scope.Getsancationed_List.push({
                                        VTADAAAA_SanctioningLevel: dd.VTADAAAA_SanctioningLevel,
                                        ApprovedBy: dd.ApprovedBy
                                    });
                                }
                            }
                        });

                        $scope.getapproveddetails = [];
                        angular.forEach($scope.Getsancationed_List, function (dd) {
                            $scope.getapproveddetails = [];
                            angular.forEach($scope.ViewDetails, function (d) {
                                if (dd.VTADAAAA_SanctioningLevel === d.VTADAAAA_SanctioningLevel) {
                                    $scope.getapproveddetails.push(d);
                                }
                            });
                            dd.reportdetails = $scope.getapproveddetails;
                        });
                        $('#viewDeatils').modal('show');

                    }
                    else {
                        swal("TADA Authorization Request Is Not  Approved !");
                    }

                });
            }
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };      
        //===========Clear Field
        $scope.Clear = function () {
            $state.reload();
        };
        $scope.isOptionsRequired = function () {
            return !$scope.get_userEmplist.some(function (options) {
                return options.empck;
            });
        };
        $scope.obj.searchValueDeptd = '';
        $scope.filterValue4 = function (obj) {
            return angular.lowercase(obj.HRMDC_Name).indexOf(angular.lowercase($scope.obj.searchValueDeptd)) >= 0;
        };
        $scope.obj.searchValueDesgd = '';
        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.HRMDES_DesignationName).indexOf(angular.lowercase($scope.obj.searchValueDesgd)) >= 0 || angular.lowercase(obj.MI_Name).indexOf(angular.lowercase($scope.obj.searchValueDesgd)) >= 0;
        };

        $scope.obj.searchValueUEMd = '';
        $scope.filterValue2 = function (obj) {
            return angular.lowercase(obj.userEmpName).indexOf(angular.lowercase($scope.obj.searchValueUEMd)) >= 0 || angular.lowercase(obj.HRMD_DepartmentName).indexOf(angular.lowercase($scope.obj.searchValueUEMd)) >= 0;
        };
        $scope.exportToExcel = function (export_id) {
            $scope.stdate = $filter('date')($scope.start_Date, 'dd-MM-yyyy');
            $scope.end = $filter('date')($scope.end_Date, 'dd-MM-yyyy');
            var excelname = "";
            var exportHref = "";
            var excelsheetname = "";
            var reportname = "";
            if ($scope.rdopunch === "TADAAdavance") {
                reportname = "#TADAAdavanceReport";
                excelsheetname = "TADAAdavance";
            }
            else if ($scope.rdopunch === "TADAApply") {
                reportname = "#TADAApplyReport";
                excelsheetname = "TADAApply";
            }
            else if ($scope.rdopunch === "consolidatereport") {
                reportname = "#consolidatereport";
                excelsheetname = "consolidatereport";
            }
            excelname = excelsheetname + ' REPORT ' + $scope.stdate + ' TO ' + $scope.end;
            excelname = excelname.toUpperCase() + '.xls';
            exportHref = Excel.tableToExcel(reportname, excelsheetname);
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };
        
        $scope.printData = function () {
            var innerContents = "";
            var name = "";
            if ($scope.rdopunch === "TADAAdavance") {

                name = "TADAAdavanceReport";
            }
            else if ($scope.rdopunch === "TADAApply") {

                name = "TADAApplyReport";
            }
            else if ($scope.rdopunch === "consolidatereport") {

                name = "consolidatereport";
            }
            innerContents = document.getElementById(name).innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.printDataModal = function () {
            var innerContents = "";
            var name = "";
            if ($scope.rdopunch === "TADAAdavance") {

                name = "printDataModalAD";
            }
            else if ($scope.rdopunch === "TADAApply") {

                name = "printDataModalTA";
            }
            else if ($scope.rdopunch === "consolidatereport") {

                name = "printDataModalC";
            }
            innerContents = document.getElementById(name).innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
    }
})();

