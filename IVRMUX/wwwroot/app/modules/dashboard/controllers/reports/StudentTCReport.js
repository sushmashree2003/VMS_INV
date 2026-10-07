(function () {
    'use strict';
    angular
        .module('app')
        .controller('StudentTcController', StudentTcController)

    StudentTcController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function StudentTcController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {

        $scope.print_flag = true;
        $scope.excel_flag = true;
        $scope.printstudents = [];

        $scope.searchValue = '';
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        var _date = new Date();
        $scope.today_date = _date
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //$scope.userPrivileges = "";
        //var pageid = $stateParams.pageId;  

        $scope.sortKey = 'ASMCL_Order';

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

       


        $scope.submitted = false;
        $scope.albumNameArraycolumn = [];
        $scope.columnsTest = [];
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            //   $scope.itemsPerPage = 10;
            var pageid = 1;
            apiService.getURI("StudentTcReport/getalldetails", pageid).
                then(function (promise) {

                    $scope.yearlst = promise.fillyear;
                    $scope.arrclasslist = promise.fillclass;
                    // $scope.arrseclist = promise.fillsection;

                    //  $scope.arrlistchkhead = promise.alldata;
                    $scope.columnsTest = [];
                    $scope.headertest = [{ id: 'AMST_FirstName', checked: true, value: 'Student Name' },
                    { id: 'AMST_RegistrationNo', checked: true, value: 'Registration No.' },
                    { id: 'ASTC_TCNO', checked: true, value: 'TC No.' },
                    { id: 'ASTC_ASMCL_ID', checked: true, value: 'Leaving Class' },
                    { id: 'ASMCL_ClassName', checked: true, value: 'Admission Class Name' },

                    { id: 'ASTC_LeavingReason', checked: true, value: 'Leaving Reason' },
                    { id: 'ASTC_Remarks', checked: true, value: 'Remark' },
                    { id: 'AMST_AdmNo', checked: true, value: 'Adm. No.' },
                    //{ id: 'ASMCL_ClassName', checked: true, value: 'Class' },
                    { id: 'ASMC_SectionName', checked: true, value: 'Section' },
                    { id: 'AMST_Date', checked: true, value: 'Date Of Admission' },
                    { id: 'ASTC_TCDate', checked: true, value: 'TC Issue Date' },
                    { id: 'AMST_FatherName', checked: true, value: 'Father Name' },
                    { id: 'AMST_MotherName', checked: true, value: 'Mother Name' },
                    { id: 'AMST_MobileNo', checked: true, value: 'Contact No.' },
                    { id: 'AMST_emailId', checked: true, value: 'Email ID' },
                    { id: 'AMST_DOB', checked: true, value: 'Date Of Birth' },
                    { id: 'AMST_PerCity', checked: true, value: 'Place' },
                    { id: 'AMST_PerAdd3', checked: true, value: 'Permanent Address' },
                    { id: 'AMST_ConCity', checked: true, value: 'Present Address' },
                    { id: 'AMST_AadharNo', checked: true, value: 'Government ID' },
                    { id: 'AMST_BPLCardNo', checked: true, value: 'STS No.' }
                    ];
                });
        };

        $scope.report123 = false;
        $scope.clssec = true;
        $scope.clssec = true;
        $scope.albumNameArraycolumn = [];
        $scope.addColumn = function (role) {
            $scope.all_header = $scope.headertest.every(function (itm) { return itm.selected; });

            if (role.selected === true) {
                $scope.albumNameArraycolumn.push(role);
                //var newCol = { id: role.value, checked: true, value: role.id }
                //$scope.columnsTest.push(newCol);
                //return $filter('filter')($scope.newuser1, { checked: true });
            }
            else {
                var som = $scope.albumNameArraycolumn.indexOf(role);
                $scope.columnsTest.splice($scope.albumNameArraycolumn.indexOf(role), 1);
                $scope.albumNameArraycolumn.splice($scope.albumNameArraycolumn.indexOf(role), 1);
                //return $filter('filter')($scope.newuser1, { checked: true });
            }
        };

        $scope.Toggle_header = function () {
            var toggleStatus_header = $scope.all_header;
            angular.forEach($scope.headertest, function (itm) {
                itm.selected = toggleStatus_header;
            });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.toggleAll = function () {

            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all === true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
        };


        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }
        };

        //Exporting data(table id will be passed)
        $scope.exportToExcel = function (export_id) {
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var exportHref = Excel.tableToExcel(export_id, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        };

        //Printing the data (div id will be passed)
        $scope.printData = function (printSectionId) {
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        };

        $scope.onclickloaddata = function () {
            if ($scope.TC_allorind === "all") {
                $scope.clssec = true;
                $scope.clssec = true;
                $scope.submitted = false;
                $scope.report123 = false;
                $scope.ASMAY = "";
                for (var i = 0; i < $scope.headertest.length; i++) {
                    $scope.headertest[i].selected = false;
                }
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
            else if ($scope.TC_allorind === "indi") {
                $scope.clssec = false;
                $scope.clssec = false;
                $scope.submitted = false;
                $scope.report123 = false;
                $scope.ASMAY = "";
                for (var i1 = 0; i1 < $scope.headertest.length; i1++) {
                    $scope.headertest[i1].selected = false;
                }
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
        };

        $scope.isOptionsRequired = function () {
            return !$scope.headertest.some(function (options) {
                return options.selected;
            });
        };

        $scope.ShowReport = function (ASMAY, TC_allorind, Tc_teporper, ASMCL, ASMS) {
            $scope.students = [];
            $scope.sortKey = 'ASMCL_Order';
            $scope.printstudents = [];
            $scope.searchValue = '';
            if ($scope.myForm.$valid) {

                angular.forEach($scope.yearlst, function (dd) {

                    if (dd.asmaY_Id === parseInt($scope.ASMAY)) {

                        $scope.yearname = dd.asmaY_Year;
                    }

                });

                $scope.albumNameArraycolumn = [];
                angular.forEach($scope.headertest, function (role) {
                    if (!!role.selected) $scope.albumNameArraycolumn.push({
                        columnID: role.id,
                        columnName: role.value
                    });
                });
                var data = {
                    "ASMAY_Id": ASMAY,
                    "tcperortemp": Tc_teporper,
                    "ASMCL_Id": ASMCL,
                    "ASMC_Id": ASMS,
                    "tcallorindi": TC_allorind,
                    "TempararyArrayheadList": $scope.albumNameArraycolumn
                };

                if ($scope.TC_allorind === 'all') {
                    if ($scope.ASMAY === null || $scope.ASMAY === undefined || $scope.albumNameArraycolumn.length === 0) {
                        // swal("Please Select The Required Fields....");
                        $scope.report123 = false;
                    } else {
                        apiService.create("StudentTcReport/Getreportdetails", data).
                            then(function (promise) {

                                if (promise.alldatagridreport !== null && promise.alldatagridreport.length > 0) {
                                    $scope.students = promise.alldatagridreport;
                                    $scope.sortKey = 'ASMCL_Order';
                                    $scope.columnsTest = promise.tempararyArrayheadList;
                                    $scope.presentCountgrid = $scope.students.length;
                                    $scope.print_flag = false;
                                    $scope.excel_flag = false;
                                    $scope.report123 = true;
                                }
                                else {
                                    swal(" Record Not Found. !!");
                                    $scope.students = [];
                                    $scope.report123 = false;
                                    $scope.print_flag = true;
                                    $scope.excel_flag = true;
                                }
                            });
                    }
                } else if ($scope.TC_allorind === 'indi') {
                    if ($scope.ASMAY === null || $scope.ASMAY === undefined || $scope.albumNameArraycolumn.length === 0 || $scope.ASMCL === null || $scope.ASMCL === undefined || $scope.ASMC === null || $scope.ASMC === undefined) {
                        //swal("Please select the required fields....");
                        $scope.report123 = false;
                    } else {
                        apiService.create("StudentTcReport/Getreportdetails", data).
                            then(function (promise) {
                                if (promise.alldatagridreport !== null && promise.alldatagridreport.length > 0) {
                                    $scope.students = promise.alldatagridreport;
                                    $scope.columnsTest = promise.tempararyArrayheadList;
                                    $scope.sortKey = 'ASMCL_Order';
                                    $scope.presentCountgrid = $scope.students.length;
                                    $scope.print_flag = false;
                                    $scope.excel_flag = false;
                                    $scope.report123 = true;
                                }
                                else {
                                    swal(" Record Not Found. !!");
                                    $scope.report123 = false;
                                    $scope.print_flag = true;
                                    $scope.excel_flag = true;
                                }
                            });
                    }
                } else {
                    swal("Please Select The Required Fields");
                    $scope.report123 = false;
                }
            } else {
                $scope.submitted = true;
            }
        };

        $("#btnExport").click(function (e) {
            window.open('data:application/vnd.ms-excel,' + $('#Table').html());
            e.preventDefault();
        });

        $scope.clear = function () {
            $state.reload();
            $scope.grid_flag = false;
        };

        $scope.getsecton = function (ASMAY, ASMCL) {

            var data = {
                "ASMAY_Id": ASMAY,
                "ASMCL_Id": ASMCL,
            };

            apiService.create("StudentTcReport/getsecton", data).then(function (promise) {
                $scope.arrseclist = promise.fillsection;
            });

        };

    }
})();

