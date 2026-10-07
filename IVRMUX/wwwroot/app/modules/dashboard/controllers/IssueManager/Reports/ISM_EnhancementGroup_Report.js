(function () {
    'use strict';
    angular.module('app').controller('ISM_EnhancementGroup_ReportController', ISM_CategoryGroup_ReportController);
    ISM_CategoryGroup_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter'];
    function ISM_CategoryGroup_ReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter) {
        $scope.search = "";

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        $scope.searchValue1 = "";
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

        // ============================ng_click function for module (ALL)========================//
        $scope.searchchkbx = "";
        $scope.start_Date = new Date();
        $scope.end_Date = new Date();
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = paginationformasters;
        $scope.userdetails = [];
        $scope.all_checktype = function () {

            var checkStatus = $scope.userchecktype;
            angular.forEach($scope.getemployee, function (itmtype) {
                itmtype.selectdemployee = checkStatus;
            });
        };

        $scope.togchkbxtype = function () {
            $scope.userchecktype = $scope.getemployee.every(function (optionstype) {
                return optionstype.selectdemployee;
            });
        };

        $scope.isOptionsRequiredtype = function () {
            return !$scope.getemployee.some(function (options) {
                return options.selectdemployee;
            });
        };

        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.getcategory, function (itm) {
                itm.selectdcaategory = checkStatus;
            });
        };

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.getcategory.every(function (options) {
                return options.selectdcaategory;
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.getcategory.some(function (options) {
                return options.selectdcaategory;
            });
        };

        $scope.filterchkbxsearch = '';
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.ismmtcaT_TaskCategoryName).indexOf(angular.lowercase($scope.filterchkbxsearch)) >= 0;
        };

        $scope.searchchkbxtype = '';
        $scope.filterchkbxtype = function (obj) {
            return angular.lowercase(obj.userEmpName).indexOf(angular.lowercase($scope.searchchkbxtype)) >= 0;

        };

        $scope.submitted = false;
        $scope.searchchkbx = "";
        $scope.searchchkbxtype = "";

        $scope.Clear = function () {
            $state.reload();
        };

        //===========================load

        $scope.loaddata = function () {

            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
          
            $scope.search = "";
            var pageid = 2;
            var data = {
                "MI_Id": 4
            };

            apiService.create("ISM_TaskGroup_Report/getcategorydetails", data).then(function (promise) {
                $scope.department_list = promise.department_list;
                $scope.userdetails = promise.userdetails;
            });
        };
        $scope.saveFeedBack = function () {
            $scope.selecteddepartment_list = [];
            if ($scope.get_report != null && $scope.get_report.length > 0) {
                angular.forEach($scope.get_report, function (d) {
                    if (d.selected == true && d.ISMTCCR_Remarks != null && d.ISMTCCR_Remarks !="") {
                        $scope.selecteddepartment_list.push({
                            ISMTCR_Id: d.ISMTCR_Id, 
                            ISMTPLAPTA_Remarks: d.ISMTCCR_Remarks
                        });
                    }
                });
                if ($scope.selecteddepartment_list != null && $scope.selecteddepartment_list.length > 0) {

                    var data = {
                        "selecteddepartment_list": $scope.selecteddepartment_list
                    };

                    apiService.create("ISM_TaskGroup_Report/SaveFeedBack", data).then(function (promise) {
                        if (promise.ismtplaptA_Status == "save") {
                            swal("Record  saved Successfully  !")
                        }
                        else {
                            swal("Record Not saved !")
                        }
                    });
                }
                else {
                    swal("Select Atlease One CheckBox and Enter Remark !");
                    return;
                }
               
            }
          
        }

        $scope.onchangedept = function () {

            var data = {
                "HRMDC_ID": $scope.Dept_Id,
                "ISMMPR_ProjectName":"Call_Center"
            }; apiService.create("ISM_TaskGroup_Report/onchangedept", data).then(function (promise) {
                $scope.getcategory = promise.getcategory;
                $scope.getemployee = promise.getemployee;

                if ($scope.getcategory !== null && $scope.getcategory.length > 0) {
                    $scope.usercheck = true;
                    angular.forEach($scope.getcategory, function (dd) {
                        dd.selectdcaategory = true;
                    });
                }

                if ($scope.getemployee !== null && $scope.getemployee.length > 0) {
                    $scope.userchecktype = true;
                    angular.forEach($scope.getemployee, function (dd) {
                        dd.selectdemployee = true;
                    });
                }

            });
        };


        //*************************** Group REPORT *********************//        
        $scope.categorygroupReport = function () {
            if ($scope.myForm.$valid) {
                //Close //Completed
                $scope.selectedemp = []; $scope.get_report = [];
                angular.forEach($scope.getemployee, function (d) {
                    if (d.selectdemployee === true) {
                        $scope.selectedemp.push({ HRME_Id: d.HRME_Id });
                    }
                });
                $scope.selectedcategory = [];
                angular.forEach($scope.getcategory, function (d) {
                    if (d.selectdcaategory === true) {
                        $scope.selectedcategory.push({ ISMMTCAT_Id: d.ismmtcaT_Id });
                    }
                });
                var data = {
                    "startdate": new Date($scope.start_Date).toDateString(),
                    "enddate": new Date($scope.end_Date).toDateString(),
                    "categorytempdto": $scope.selectedcategory,
                    "categoryemptempdto": $scope.selectedemp,
                    "HRMDC_ID": $scope.Dept_Id
                };
                apiService.create("ISM_TaskGroup_Report/categorygroupReport", data).then(function (promise) {
                    if (promise.get_report !== null && promise.get_report.length > 0) {
                        angular.forEach(promise.get_report, function (d) {
                            d.selected = false;
                            if (d.ISMTCR_Status === "Close" || d.ISMTCR_Status === "Completed") {
                                $scope.get_report.push(d);
                            }
                        });
                        //$scope.userdetails
                        if ($scope.get_report != null && $scope.get_report.length > 0) {
                            angular.forEach($scope.get_report, function (d) {
                                d.ISMTCCR_Remarks = "";
                                angular.forEach($scope.userdetails, function (dd) {
                                    if (d.ISMTCR_Id == dd.ismtcR_Id) {
                                        d.ISMTCCR_Remarks = dd.ismtccR_Remarks;
                                    }
                                });
                            });
                        }

                    }
                    else {
                        swal("No Record Found..... !!");
                        $scope.get_report = [];
                      //  $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.printData = function () {
            var innerContents = '';
            innerContents = document.getElementById("printareaId1").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); },900);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function (export_id) {
            var excelname = 'ENHANCEMENT  REPORT';
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
    }
})();

