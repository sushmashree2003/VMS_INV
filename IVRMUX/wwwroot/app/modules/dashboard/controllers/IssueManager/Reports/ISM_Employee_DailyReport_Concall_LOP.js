(function () {
    'use strict';
    angular.module('app').controller('ISM_Employee_DailyReport_Concall_LOPController', ISM_Employee_DailyReport_Concall_LOPController);
    ISM_Employee_DailyReport_Concall_LOPController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout'];
    function ISM_Employee_DailyReport_Concall_LOPController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        $scope.submitted = false;

        $scope.maxdate = new Date();

        $scope.loaddata = function () {
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            $scope.dateflag = 0;
            var pageid = 2;
            apiService.getURI("ISM_ApprovalReport/getcatempdetails", pageid).then(function (promise) {
                $scope.hide_n = false;
                if (promise.ivrmrT_Role === 'admin') {
                    $scope.depdes = true;
                    $scope.get_userEmplist = promise.get_userEmplist;
                }
                else {
                    $scope.depdes = false;
                    $scope.get_userEmplist = promise.get_userEmplist;
                }
                $scope.griddata = promise.griddata;
            });
        };

        $scope.all_checkuu = function (ack) {
            $scope.checkalluu = ack;
            var toggleStatus = $scope.checkalluu;
            angular.forEach($scope.get_userEmplist, function (uem) {
                uem.empck = toggleStatus;
            });
            $scope.onEmployeechange();
        };

        $scope.togchkbxuu = function () {
            $scope.checkalluu = $scope.get_userEmplist.every(function (options) {
                return options.empck;
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.get_userEmplist.some(function (options) {
                return options.empck;
            });
        };

        $scope.approvalReport = function () {
            $scope.plannerid = [];
            if ($scope.myForm.$valid) {

                $scope.temp_Planner = [];
                $scope.searchValue = "";
                $scope.get_emp_dailyreport = "";

                $scope.arrayuserEmp = [];

                angular.forEach($scope.get_userEmplist, function (em) {
                    if (em.empck === true) {
                        $scope.arrayuserEmp.push(em);
                    }
                });

                var data = {
                    arrayuserEmp: $scope.arrayuserEmp,
                    "Fromdate": new Date($scope.start_Date).toDateString(),
                    "Todate": new Date($scope.end_Date).toDateString(),
                    "feedbackflg": true

                };
                apiService.create("ISM_ApprovalReport/getempcatedetails", data).then(function (promise) {
                    $scope.plannerid = [];
                    $scope.hide_n = false;
                    $scope.feed = false;

                    if (promise.get_emp_dailyreport.length > 0) {
                        $scope.get_emp_dailyreport = promise.get_emp_dailyreport;
                        
                    }
                    else
                    {
                        swal("No Records Found");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.feedbackflgclr = function () {
            if ($scope.feedbackflg === false) {
                $scope.start_Date = new Date();
            }
            else {
                $scope.start_Date = '';
                $scope.end_Date = '';
            }
        };

        $scope.datechange = function () {
            $scope.end_Date='';
        };

        $scope.deactive = function (obj) {

            var mgs = "";
            var confirmmgs = "";
            if (obj.HRALD_Activeflag === true) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";

            }

            var data = {
                "HRALD_ID": obj.HRALD_ID,
                "HRME_Id": obj.HRME_Id,

            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ISM_ApprovalReport/deactivate", data).then(function (promise) {

                            if (promise.returnmsg === "true") {
                                swal("Record " + confirmmgs + " " + "successfully", "", "success");
                            }
                            else {
                                swal("Record " + mgs + " Failed", "", "error");
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };


        $scope.SaveEmpLop = function () {            $scope.AdditionLOPDetails = [];            if ($scope.myForm.$valid) {                if ($scope.get_emp_dailyreport != null && $scope.get_emp_dailyreport.length > 0) {                    angular.forEach($scope.get_emp_dailyreport, function (ss) {                        if (ss.usercheck == true) {                            $scope.AdditionLOPDetails.push({                                HRME_Id: ss.HRME_Id,                                HRALD_Remarks: ss.ISMDRPT_Remarks,                                VEF_NegativeFlag: ss.VEF_NegativeFlag,                                ISMTCR_Id: ss.ISMTCR_Id                            });                        }                    })                }                if ($scope.AdditionLOPDetails != null && $scope.AdditionLOPDetails.length > 0) {
                    var data = {                        "AdditionLOPDetails": $scope.AdditionLOPDetails,                    }                    apiService.create("ISM_ApprovalReport/saveAddLop", data).then(function (promise) {                        if (promise.message == "saved") {                            swal('Con call Lop Saved Succesfully', "", "success");                        }                        else if (promise.message == "") {                            swal("failed to save", "", "error");                        }                        else {                            swal('Please Contcat Administrator');                        }                        $state.reload();                    });                }                else {                    swal('Please Select  Atleast one CheckBox');                }            }            else {                $scope.submitted = true;            }        };



        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clear = function () {
            $state.reload();
        };

        $scope.printData = function () {
            $scope.hide_n = false;
            var innerContents = "";
            innerContents = document.getElementById("excelid").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function () {
            //var blob = new Blob([document.getElementById('printP').innerHTML], {
            //    type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
            //});
            //saveAs(blob, "Approval Report.xls");


            var excelname = 'Employee Con Call Daily Report Details';
            excelname = excelname.toUpperCase() + '.xls';
            //var printSectionId = export_id;s
            var exportHref = Excel.tableToExcel('#excelid', 'Employee Con Call Daily Report Details');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };
    }
})();