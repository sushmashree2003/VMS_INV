(function () {
    'use strict';
    angular
        .module('app')
        .controller('UnauthorizedLeaveCountReportController', UnauthorizedLeaveCountReportController)
    UnauthorizedLeaveCountReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter']
    function UnauthorizedLeaveCountReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter) {

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("FrontOffice/getalldetails", pageid).then(function (promise) {

                $scope.pagePrivilege = promise.pagePrivilege;
                $scope.getmonth = promise.getmonth;
                $scope.getyear = promise.getyear;
                $scope.institutionlist = promise.institutionlist;


                //$scope.usercheck = true;
                $scope.all_check();
            });
        };

        //Onchange Institution, Departmnet Should Load

        $scope.getdepartment = function (companyid) {
            var data = {
                "MI_Id": companyid.mI_Id,
            };
            apiService.create("FrontOffice/getdepartment", data).
                then(function (promise) {
                    if (promise.columnnames !== null && promise.columnnames.length > 0) {
                        $scope.columnnames = promise.columnnames;
                    }
                    else {
                        swal(" No Record Found !");
                    }
                });
        };


        //Report
        $scope.submitted = false;
        $scope.unauthorizedreport = function () {
            $scope.gettodate = [];
            $scope.Datewisecount = [];
            var from_daterp = $filter('date')($scope.from_date, 'yyyy-MM-dd');
            var to_daterp = $filter('date')($scope.to_date, 'yyyy-MM-dd');
            $scope.submitted = true;
            $scope.selected_Inst = [];
            $scope.selected_Dept = [];

            if ($scope.radioval == "Institutionwise") {
                angular.forEach($scope.institutionlist, function (mm) {
                    if (mm.select) {
                        $scope.selected_Inst.push(mm);
                    }

                })
            }
            else {
                $scope.selected_Inst.push({ MI_Id: $scope.mI_Id.mI_Id });
            }

            if ($scope.radioval == "Departmentwise") {
                angular.forEach($scope.columnnames, function (mm) {
                    if (mm.selected) {
                        $scope.selected_Dept.push(mm);
                    }

                })
            }
            else {
                $scope.selected_Dept.push({ HRMD_Id: 0 });
            }


            if ($scope.myForm.$valid) {
                var data = {
                    "HRMLY_LeaveYear": $scope.HRMLY_LeaveYear,
                    "IVRM_Month_Name": $scope.IVRM_Month_Name,
                    "IVRM_Month_Id": $scope.IVRM_Month_Id,
                    "from_date": from_daterp,
                    "to_date": to_daterp,
                    "Datetype": $scope.allind,
                    "flag": $scope.Type,
                    "radioflag": $scope.radioval,
                    selected_Inst: $scope.selected_Inst,
                    selected_Dept: $scope.selected_Dept

                }

                apiService.create("FrontOffice/unauthorizedreport", data).
                    then(function (promise) {
                        if (promise != null) {
                            $scope.gettodate = promise.gettodate;
                            $scope.IVRM_Month_Name = promise.ivrM_Month_Name;
                            $scope.Datewisecount = promise.gettodate;

                            $scope.departmentwise = promise.gettodate;


                            var departmentname = [];

                            angular.forEach($scope.departmentwise, function (value) {
                                departmentname.push(value.HRMD_DepartmentName);

                            });

                            var departmentid = departmentname.filter(function (value, index) { return departmentname.indexOf(value) == index });

                            $scope.departmentid = [];

                            angular.forEach(departmentid, function (val) {
                                var leavecount = 0;
                                angular.forEach($scope.departmentwise, function (deve) {
                                    if (deve.HRMD_DepartmentName === val) {
                                        leavecount = leavecount + deve.UAleavecount
                                    }
                                });

                                $scope.departmentid.push({
                                    HRMD_DepartmentName: val,
                                    leavecount: leavecount

                                });
                            });

                            angular.forEach($scope.departmentid, function (dept) {
                                $scope.templistdept = [];
                                angular.forEach($scope.departmentwise, function (dep) {
                                    if (dep.HRMD_DepartmentName === dept.HRMD_DepartmentName) {
                                        $scope.templistdept.push(dep);
                                    }
                                });
                                dept.tempdepartment = $scope.templistdept;
                            });



                            $scope.employeeid = [];

                            angular.forEach($scope.Datewisecount, function (dev) {
                                if ($scope.employeeid.length === 0) {
                                    $scope.employeeid.push({
                                        HRME_Id: dev.HRME_ID,
                                        HRME_EmployeeFirstName: dev.HRME_EmployeeFirstName,
                                        FOMHWDD_FromDate: dev.FOMHWDD_FromDate
                                    });
                                } else if ($scope.employeeid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.employeeid, function (emp) {
                                        if (emp.HRME_Id === dev.HRME_ID) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.employeeid.push({
                                            HRME_Id: dev.HRME_ID,
                                            HRME_EmployeeFirstName: dev.HRME_EmployeeFirstName,
                                            FOMHWDD_FromDate: dev.FOMHWDD_FromDate

                                        });
                                    }
                                }
                            });

                            angular.forEach($scope.employeeid, function (ddd) {
                                $scope.templist = [];
                                angular.forEach($scope.Datewisecount, function (dd) {
                                    if (dd.HRME_ID === ddd.HRME_Id) {
                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.plannerdetails = $scope.templist;
                            });



                        }
                        else {
                            swal("Record Not Found..");
                        }


                    })
            }
        };



        // $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            var count = 0;
            angular.forEach($scope.institutionlist, function (itm) {
                itm.select = checkStatus;
                if (itm.select == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }

        $scope.all_check1 = function () {
            var checkStatus = $scope.hrmD_Id;
            var count = 0;
            angular.forEach($scope.columnnames, function (itm) {
                itm.selected = checkStatus;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });

        }

        $scope.isOptionsRequired = function () {
            return !$scope.institutionlist.some(function (options) {
                return options.select;
            });
        }

        $scope.isOptionsRequired1 = function () {
            return !$scope.columnnames.some(function (options) {
                return options.selected;
            });
        }

        $scope.cancel = function () {
            $state.reload();
        }
        $scope.togchkbx = function () {
            $scope.usercheck = $scope.institutionlist.every(function (options) {
                return options.select;
            });
        }

        $scope.togchkbx1 = function () {

            $scope.hrmD_Id = $scope.columnnames.every(function (options) {
                return options.selected;
            });
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //print
        $scope.print = function (Baldwin) {
            var innerContents = document.getElementById("countPrint").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        //Export to Excel
        $scope.exportToExcel = function () {
            var divToPrint = document.getElementById("exportReport");
            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            saveAs(blob, "UnAuthorisedLeaveReport.xls");
        };

    }
})();