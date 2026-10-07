(function () {
    'use strict';
    angular
        .module('app')
        .controller('VMS_ReportController', VMS_ReportController)

    VMS_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout',]
    function VMS_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, Excel, $timeout, ) {

        $scope.obj = {};
        $scope.loaddata = function () {

            $scope.currentPageU = 1;
            $scope.itemsPerPageU = 10;
            $scope.search = "";
            var pageid = 2;

            apiService.getURI("VMS_Report/onloadgetdetails", pageid).then(function (promise) {
                
                $scope.institutionlist = promise.companydetails;
                
                
                $scope.allind = "Institution";
                $scope.all_check();
            });
        };
        
        //cancel
        $scope.cleardata = function () {
            $state.reload();
     
        };

        // selecting all
        $scope.searchchkbx = ""; 
        $scope.all_check = function () {
            var toggleStatus = $scope.desgcheck;
            angular.forEach($scope.institutionlist, function (itm) {
                itm.select = toggleStatus;
            });
            $scope.get_employee();
        }

        $scope.isOptionsRequired = function () {
            return !$scope.institutionlist.some(function (com) {
                return com.select;
            });
        }

        $scope.togchkbx = function () {
            $scope.institutionlist.every(function (options) {
                return options.select;
            });
        }
 
        $scope.get_employee = function () {
            
            $scope.desgcheck = $scope.institutionlist.every(function (com) {
                return com.select; 
            });

            $scope.get_employeenew();
        }

        $scope.get_employeenew = function () {
            var groupidss;
            for (var i = 0; i < $scope.institutionlist.length; i++) {
                 if ($scope.institutionlist[i].select == true) {

                    if (groupidss == undefined)
                        groupidss = $scope.institutionlist[i].mI_Id;
                    else
                        groupidss = groupidss + "," + $scope.institutionlist[i].mI_Id;
                }
            }
            if (groupidss != undefined) {
                var dto = {
                    "multipledep": groupidss
                }
                apiService.create("VMS_Report/get_Employee", dto).
                    then(function (promise) {

                        if (promise.fillemployee.length != null && promise.fillemployee.length>0) {
                            $scope.Employeelst = promise.fillemployee;
                        } else {
                            swal("Record not found.");
                        }

                        if (promise.projectName.length != null && promise.projectName.length > 0) {
                        
                            $scope.projectName = promise.projectName;
                        } else {
                            swal("Record not found.");
                        }
                        
                    })
            }
            else {
                $scope.Employeelst = "";
            }
        }
        
        $scope.All_Individual = function () {      
            angular.forEach($scope.institutionlist, function (mm) {
                mm.select = false;
                $scope.desgcheck = false;
                $scope.submitted = false;

            })
        }

       
        //to get report
        $scope.submitted = false;
        $scope.report = function () {
            $scope.submitted = true;
            $scope.selected_Inst = [];

            if ($scope.myForm.$valid) {
                if ($scope.allind == "Institution") {
                    angular.forEach($scope.institutionlist, function (mm) {
                        if (mm.select) {
                            $scope.selected_Inst.push(mm);
                        }
                    })
                    var dto = {
                        "selected_Inst": $scope.selected_Inst,
                        "allind": $scope.allind
                    }

                    apiService.create("VMS_Report/report", dto).
                        then(function (promise) {
                            if (promise.getReport.length > 0 && promise.getReport != null) {
                                $scope.GetReport = promise.getReport;
                            }
                            else {
                                swal("No Record  Found..... !!");

                            }
                        })
                }
                else {
                    angular.forEach($scope.institutionlist, function (mm) {
                        if (mm.select) {
                            $scope.selected_Inst.push(mm);
                        }
                    })
                    var dto = {
                        "selected_Inst": $scope.selected_Inst,
                        "allind": $scope.allind,
                        "HRME_Id": $scope.obj.hrmE_Id.hrmE_Id
                        //"ISMMPR_Id": $scope.obj.ismmpR_Id.ismmpR_Id
                    }

                    apiService.create("VMS_Report/report", dto).
                        then(function (promise) {
                            if (promise.getReport.length > 0 && promise.getReport != null) {
                                $scope.GetReport = promise.getReport;
                            }
                            else {
                                swal("No Record  Found..... !!");
                            }
                        })
                }

               $scope.institutionlist = promise.institutionlist;

            }
            else {
                $scope.submitted = true;
            }
        };
        
        $scope.interacted = function () {
            return $scope.submitted;
        };

        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printDeviation").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function (export_id) {
            var exportHref = Excel.tableToExcel(export_id, 'Salary Approval Report');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };


    }
})();