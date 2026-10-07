(function () {
    'use strict';
    angular
        .module('app')
        .controller('AttendanceApprovalReportController', AttendanceApprovalReportController)

    AttendanceApprovalReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$timeout' , 'Excel']
    function AttendanceApprovalReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $timeout, Excel) {

        $scope.print_flag = true;
        $scope.excel_flag = true;

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            apiService.getURI("AttendanceApprovalReport/getalldetails", pageid).then(function (promise) {

                $scope.getmonth = promise.getmonth;
                $scope.getyear = promise.getyear;
                $scope.institutionlist = promise.institutionlist;
            });
        };

        //Report
        $scope.submitted = false;
        $scope.report = function () {


            $scope.submitted = true;
            $scope.selected_Inst = [];
            angular.forEach($scope.institutionlist, function (mm) {
                if (mm.select) {
                    $scope.selected_Inst.push(mm);
                }
            })

            if ($scope.myForm.$valid) {
                var data = {
                    "HRMLY_LeaveYear": $scope.HRMLY_LeaveYear,
                    "IVRM_Month_Name": $scope.IVRM_Month_Name,
                    "HREAA_ApprovalFlg": $scope.radiotype,

                    selected_Inst: $scope.selected_Inst,
                }

                apiService.create("AttendanceApprovalReport/report", data).
                    then(function (promise) {
                        $scope.getreport = promise.getreport;
                    })
            }
        };



        $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.institutionlist, function (itm) {
                itm.select = checkStatus;
            });
        }
        $scope.isOptionsRequired = function () {
            return !$scope.institutionlist.some(function (options) {
                return options.select;
            });
        }

        $scope.togchkbx = function () {
            $scope.institutionlist.every(function (options) {
                return options.select;
            });
        }

        $scope.Print = function () {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            //popupWinindow.document.write('<html><head>' +
            //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
            //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
            //    '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
            //    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            //);
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 10000);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        //$scope.exportToExcel = function () {

        //    var excelname = 'Attendance Approval Report List';
        //    excelname = excelname.toUpperCase() + '.xls';
        //    var exportHref = Excel.tableToExcel('#exceltopd', 'Attendance Approval Report List');
        //    $timeout(function () {
        //        var a = document.createElement('a');
        //        a.href = exportHref;
        //        a.download = excelname;
        //        document.body.appendChild(a);
        //        a.click();
        //        a.remove();
        //    }, 100);
        //};

        $scope.exportToExcel = function () {

            var excelname = 'Attendance Approval Report List';
            excelname = excelname.toUpperCase() + '.xls';
            var exportHref = Excel.tableToExcel('#printSectionId', 'Attendance Approval Report List');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.cancel = function () {
            $state.reload();
        };

    }
})();