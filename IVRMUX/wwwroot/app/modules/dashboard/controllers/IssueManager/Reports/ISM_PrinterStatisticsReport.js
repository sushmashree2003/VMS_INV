(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_PrinterStatisticsReportController', ISM_PrinterStatisticsReportController);
    ISM_PrinterStatisticsReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter'];
    function ISM_PrinterStatisticsReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter) {


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
        $scope.printerdet = false;
        $scope.stdall = false;
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        $scope.printdatatable = [];
        $scope.get_printdata1 = [];
        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();

        $scope.submitted = false;
        angular.forEach($scope.get_printdata, function (obj) {
            obj.stdselected = false;
        });

        $scope.start_Date = new Date();
        $scope.end_Date = new Date();
        $scope.loaddata = function () {

            $scope.currentPage = 1;
           $scope.itemsPerPage = paginationformasters;
            //$scope.itemsPerPage = 5;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("ISM_PrinterStatisticsReport/getdetails", pageid).then(function (promise) {

                $scope.get_department = promise.get_department;

                $scope.get_printdetails = promise.get_printdetails;
                $scope.presentCountgrid = $scope.get_printdetails.length;
                $scope.hrmD_Id = promise.dept_Id;
                
                //$scope.hrmD_Id ="4";


            });
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;
                $scope.reverse = !$scope.reverse;
            };
        };
        
        $scope.getreport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var color, BW;
                $scope.hrmdlist = [];
                if ($scope.hrmD_Id == "0") {

                }
                else {

                }
                if ($scope.checkboxval == true) {
                    color = 'Color';
                   
                }
                if ($scope.checkboxval1 == true) {
                    BW = 'BlackWhite';
                }

                if (color != null && BW != null) {
                    var data = {
                        "startdate": new Date($scope.start_Date).toDateString(),
                        "enddate": new Date($scope.end_Date).toDateString(),
                        "HRMD_Id": $scope.hrmD_Id,
                        "VMSPOD_Type_One": color,
                        "VMSPOD_Type_Two": BW,
                        "flag":1

                    };
                }
                else if (color == null && BW != null) {
                    var data = {
                        "startdate": new Date($scope.start_Date).toDateString(),
                        "enddate": new Date($scope.end_Date).toDateString(),
                        "HRMD_Id": $scope.hrmD_Id,

                        "VMSPOD_Type_Two": BW,
                        "flag": 2

                    };
                }
                else if (color != null && BW == null) {
                    var data = {
                        "startdate": new Date($scope.start_Date).toDateString(),
                        "enddate": new Date($scope.end_Date).toDateString(),
                        "HRMD_Id": $scope.hrmD_Id,
                        "VMSPOD_Type_One": color,
                         "flag": 3

                    };
                }
                else {
                    var data = {
                        "startdate": new Date($scope.start_Date).toDateString(),
                        "enddate": new Date($scope.end_Date).toDateString(),
                        "HRMD_Id": $scope.hrmD_Id,
                        "flag": 4

                    };
                }
                apiService.create("ISM_PrinterStatisticsReport/get_report", data).then(function (promise) {
                    $scope.plannerid = [];
                    if (promise.get_printdetails.length > 0) {

                        $scope.get_printdata = promise.get_printdetails;
                        $scope.presentCountgrid = promise.get_printdetails.length;
                    }
                    else {
                        swal("No Record Found..... !!");
                        $scope.get_printdata = "";
                        $state.reload();
                    }

                });

            }
            else {
                $scope.submitted = true;
            }
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
            var exportHref = Excel.tableToExcel(export_id, 'get_printdetail');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.Clear = function () {
            $state.reload();
            //$scope.get_report = "";
    
            //$scope.get_printdata = "";
            //$scope.HRMD_Id = "";
            //$scope.start_Date = new Date();
            //$scope.end_Date = new Date();
            //$scope.checkboxval = false;
            //$scope.checkboxval1 = false;
            //$scope.loaddata();
        };

        $scope.optionToggledstd = function (SelectedStudentRecord, index) {
         
            $scope.stdall = $scope.get_printdata.every(function (itm)
            {
                return itm.stdselected;
            });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }


            if ($scope.get_printdata1.indexOf(SelectedStudentRecord) === -1) {
                $scope.get_printdata1.push(SelectedStudentRecord);
            }
            else {
                $scope.get_printdata1.splice($scope.get_printdata1.indexOf(SelectedStudentRecord), 1);
            }

            if ($scope.get_printdata1.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }

           
        }


        $scope.btnclick = function () {
            if ($scope.printdatatable.length > 0) {
                $('#myModalswal').modal('show');
            }
            else {
                $('#myModalswal').modal('hide');
            }

        }
    
        $scope.toggleAllstd = function () {
        
            var toggleStatus = $scope.stdall;
            angular.forEach($scope.get_printdata, function (itm) {

          
                itm.stdselected = toggleStatus;
                if (itm.stdselected == true) {
                    $scope.printdatatable.push(itm);
                    $scope.get_printdata1.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                    $scope.get_printdata1.splice(itm);
                }
            });


            if ($scope.printdatatable.length != null) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
        
       

          
        }

        $scope.SendMail = function () {
            $scope.hrmdidsTemp = [];
            $scope.tmplt = [];
            var Template = document.getElementById("empmail").innerHTML;
            var hrmdid = [];
           
            angular.forEach($scope.get_printdata, function (ty) {
                if (ty.stdselected==true) {
                      //  hrmdid.push(ty.hrmD_Id);
                    $scope.hrmdidsTemp.push({
                        HRMD_Id: ty.hrmD_Id
                    })
                    }
                })
            var data =
            {
               
                Templatedata: Template,
                // hrmdids: hrmdid
                hrmdids: $scope.hrmdidsTemp
            };

            apiService.create("ISM_PrinterStatisticsReport/sendmail", data).
                then(function (promise) {
                    if (promise.retrunMsg == "success") {
                        swal("Email Sent..!!!");
                        $('#myModalswal').modal('hide');
                    }
                    else if (promise.retrunMsg == "notFound") {
                        swal("Email Not sent..!!!", 'Default Email-Id is Not Found.. !!!');
                        $('#myModalswal').modal('hide');
                    }
                    else if (promise.retrunMsg == "Error") {
                        swal("Something went wrong", 'Try After some time..!!');
                    
                    } else {
                        swal("Something went wrong", 'Try After some time..!!');
                    }
                });
        };

    }
})();

