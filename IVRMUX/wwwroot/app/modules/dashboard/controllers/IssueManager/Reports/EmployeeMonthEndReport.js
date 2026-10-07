(function () {
    'use strict';
    angular
        .module('app')
        .controller('employeeMonthEndController', employeeMonthEndController);

    employeeMonthEndController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', '$timeout'];
    function employeeMonthEndController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, $timeout) {

        $scope.EarningDet = {};
        $scope.addjob = {};
        $scope.obj = {};
        $scope.addjob.hrjD_Id = 0;
        $scope.submitted = true;
        $scope.grid_view = false;
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("EmployeeMonthEnd/getemplist", pageid).then(function (promise) {
                $scope.employeelist = promise.employeelist;
                $scope.fillmonth = promise.monthlist;
                $scope.fillyear = promise.yearlist;
            });
        };
        $scope.cancel = function () {
            $state.reload();
        };
        $scope.submitted = false;
        $scope.interacted1 = function (field) {
            return $scope.submitted;
        };
        $scope.viewdata = [];
        $scope.findreport = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "HRME_ID": $scope.hrmE_Id,
                    "IVRM_Month_Id": $scope.month,
                    "HRMLY_Id": $scope.year
                };
                apiService.create("EmployeeMonthEnd/getempdetails", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {
                            $scope.viewdata = [];
                            // $scope.str_team_lead = promise.str_team_lead[0].hrmE_EmployeeFirstNametwo;
                            if (promise.names[0].DepartmentHead != null && promise.names[0].DepartmentHead !== undefined) {
                               

                                var DepartmentHead = promise.names[0].DepartmentHead;
                                if (DepartmentHead == 0) {
                                    $scope.DepartmentHead = " ";
                                }
                                else {
                                    $scope.DepartmentHead = DepartmentHead;
                                }

                            }
                            if (promise.names[0].TechnicalDirector != null && promise.names[0].TechnicalDirector !== undefined) {
                                var TechnicalDirector = promise.names[0].TechnicalDirector;
                                if (TechnicalDirector == 0) {
                                    $scope.TechnicalDirector = " ";
                                }
                                else {
                                    $scope.TechnicalDirector = TechnicalDirector;
                                }
                              
                            }
                            if (promise.names[0].VerifiedBy != null && promise.names[0].VerifiedBy !== undefined) {
                                var verified = promise.names[0].VerifiedBy;
                                if (verified == 0) {
                                    $scope.VerifiedBy = " ";
                                }
                                else {
                                    $scope.VerifiedBy = verified;
                                }
                               
                            }
                          
                            if (promise.names[0].ReportingManager != null && promise.names[0].ReportingManager !== undefined) {
                                var ReportingManager = promise.names[0].ReportingManager;
                                if (ReportingManager == 0) {
                                    $scope.ReportingManager = " ";
                                }
                                else {
                                    $scope.ReportingManager = ReportingManager;
                                }
                               
                            }
                            
                           
                           // $scope.mgrName = promise.rpt_name;
                            $scope.currentdate = new Date();
                            $scope.allheadvalues = promise.allheadvalues;
                            $scope.overallDeveloperCounts = [];
                            $scope.found = false;
                            if (promise.valueexist == true) {
                                angular.forEach($scope.allheadvalues, function (d) {
                                    $scope.viewdata.push({ label: d.vmsmmedpT_Description, values: d.vmsewmeR_Value, vmsmmedpT_ActiveFlag: d.vmsmmedpT_ActiveFlag, vmsmmedpT_ManualFlag: d.vmsmmedpT_ManualFlag, vmsmmedpT_Id: d.vmsmmedpT_Id, remark: d.vmsewmeR_Remarks });
                                });
                            }
                            else {
                                angular.forEach($scope.allheadvalues, function (d) {
                                    $scope.found = false;
                                    angular.forEach(promise.overallDeveloperCounts[0], function (key, value) {
                                        if (d.vmsmmedpT_Description === value) {
                                            $scope.found = true;
                                            $scope.viewdata.push({ label: value, values: key, vmsmmedpT_ActiveFlag: d.vmsmmedpT_ActiveFlag, vmsmmedpT_ManualFlag: d.vmsmmedpT_ManualFlag, vmsmmedpT_Id: d.vmsmmedpT_Id });
                                        }
                                    });
                                    if ($scope.found == false) {
                                        $scope.viewdata.push({ label: d.vmsmmedpT_Description, values: 0, vmsmmedpT_ActiveFlag: d.vmsmmedpT_ActiveFlag, vmsmmedpT_ManualFlag: d.vmsmmedpT_ManualFlag, vmsmmedpT_Id: d.vmsmmedpT_Id });
                                    }
                                });
                            }

                            $scope.overallDeveloperCounts = $scope.viewdata;

                            if (promise.employeedetails.length > 0 && promise.employeedetails != null) {
                                $scope.candidateDetails = promise.employeedetails[0];
                                $scope.grid_view = true;
                                 
                            }

                            $scope.monthname = promise.monthname;
                            $scope.companydetails = promise.companydetails[0];
                        }
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.printToCart = function () {
            //console.log($scope.overallDeveloperCounts);
            var innerContents = document.getElementById("printdataforprint").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.Savedata = function () {
            if ($scope.overallDeveloperCounts.length > 0) {
                $scope.temp_array2 = [];
                for (var x = 0; x < $scope.overallDeveloperCounts.length; x++) {
                    if ($scope.overallDeveloperCounts[x].vmsmmedpT_ActiveFlag == true || $scope.overallDeveloperCounts[x].vmsmmedpT_ManualFlag == true) {
                        $scope.temp_array2.push($scope.overallDeveloperCounts[x]);
                    }
                }
            }

            var data = {
                "HRME_ID": $scope.hrmE_Id,
                "VMSEWMER_Month": $scope.month,
                "VMSEWMER_Year": $scope.year,
                "TempDTO": $scope.temp_array2
            };

            apiService.create("EmployeeMonthEnd/savereport", data).
                then(function (promise) {
                    if (promise.message === 'False') {
                        swal("Failed to Save/Update");
                    }
                    else if (promise.message === 'Update') {
                        swal("Record Updated Successfully.");
                        $state.reload();
                    }
                    else if (promise.message === 'Add') {
                        swal('Record Saved Successfully.');
                        $state.reload();
                    }
                });

        };
    }
})();