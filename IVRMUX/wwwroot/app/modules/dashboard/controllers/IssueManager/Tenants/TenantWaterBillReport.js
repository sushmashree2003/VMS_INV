
(function () {
    'use strict';
    angular
        .module('app')
        .controller('TenantWaterBillReportController', TenantWaterBillReportController)
    TenantWaterBillReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', 'Excel', '$timeout' ,'$window', 'superCache', '$compile']
    function TenantWaterBillReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, Excel, $timeout, $window, superCache, $compile) {
      
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
        $scope.stdall = false;
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();
        $scope.submitted = false;
        $scope.start_Date = new Date();
        $scope.end_Date = new Date();

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;

            apiService.getURI("TenantWaterBillReport/getdetails", pageid).then(function (promise) {

                $scope.building = promise.building;
                $scope.monthname = promise.rooms;
                $scope.tenants = promise.tenants;
                $scope.arrlist8 = promise.year;

            });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };

        $scope.all_checkC = function () {
            var checkStatus = $scope.usercheckC;
            var count = 0;
            angular.forEach($scope.building, function (itm) {
                itm.selected = checkStatus;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }

        $scope.all_checkC1 = function () {
            var checkStatus = $scope.usercheckC1;
            var count = 0;
            angular.forEach($scope.tenants, function (itm) {
                itm.selected1 = checkStatus;
                if (itm.selected1 == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }

        $scope.all_checkC2 = function () {
            var checkStatus = $scope.usercheckC2;
            var count = 0;
            angular.forEach($scope.rooms, function (itm) {
                itm.selected2 = checkStatus;
                if (itm.selected2 == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }

        $scope.togchkbxC = function () {
            var HRMBID = [];

            $scope.usercheckC = $scope.building.every(function (options) {
                return options.selected;
            });

            angular.forEach($scope.building, function (ty) {
                if (ty.selected) {
                    HRMBID.push(ty.hrmB_Id);
                }
            })

            $scope.usercheckC1 = $scope.tenants.every(function (options) {
                return options.selected1;
            });
            var tenantname = [];
            angular.forEach($scope.tenants, function (ty) {
                if (ty.selected1) {
                    tenantname.push(ty.tenantagR_Id);
                }
            })
            var data = {

                "buldid": HRMBID,
                "year": $scope.hrmlY_LeaveYear,
                "tenant": tenantname,

            }

            apiService.create("TenantWaterBillReport/buildingchange", data).
                then(function (promise) {
                    //$scope.rooms = promise.rooms;

                    $scope.tenants = promise.tenants;
                   // $scope.arrlist8 = promise.yearlist;

                })
        }

        $scope.togchkbxC1 = function () {

            $scope.usercheckC1 = $scope.tenants.every(function (options) {
                return options.selected1;
            });

            var HRMBID = [];
            angular.forEach($scope.building, function (ty) {
                if (ty.selected) {
                    HRMBID.push(ty.hrmB_Id);
                }
            })
            var HRMFID = [];
            angular.forEach($scope.tenants, function (ty) {
                if (ty.selected1) {
                    TENANTAGRId.push(ty.TENANTAGR_Id);
                }
            })
            var data = {

                buldid: HRMBID,
                tenantid: TENANTAGRId,

            }

            apiService.create("TenantWaterBillReport/floorchange", data).
                then(function (promise) {

                    $scope.tenants = promise.tenants;
                    
                })
        }

        $scope.isOptionsRequired = function () {
            return !$scope.building.some(function (options) {
                return options.selected;
            });
        }

        $scope.isOptionsRequired1 = function () {
            return !$scope.tenants.some(function (options) {
                return options.selected1;
            });
        }

        $scope.isOptionsRequired2 = function () {
            return !$scope.rooms.some(function (options) {
                return options.selected2;
            });
        }
  

        $scope.getreport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var HRMBID = [];
                angular.forEach($scope.building, function (ty) {
                    if (ty.selected) {
                        HRMBID.push(ty.hrmB_Id);
                    }
                })
                var TENANTAGRID = [];
                angular.forEach($scope.tenants, function (ty) {
                    if (ty.selected1) {
                        TENANTAGRID.push(ty.tenantagR_Id);
                    }
                })
                var IMMId = [];
                angular.forEach($scope.monthname, function (ty) {
                    if (ty.selected3) {
                        IMMId.push(ty.ivrM_Month_Id);
                    }
                })
                
                var data = {

                    "buldid": HRMBID,
                    "tenant": TENANTAGRID,
                    "monthname": IMMId,
                    "yearname": $scope.fmtP_Years,
                  
                }
                
                apiService.create("TenantWaterBillReport/get_report", data).then(function (promise) {
                    $scope.plannerid = [];
                    if (promise.get_userEmplist.length > 0) {

                        $scope.getloaddata = promise.get_userEmplist;

                        $scope.presentCountgrid = promise.get_userEmplist.length;
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
        
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        
        $scope.Clear = function () {

            $state.reload();
        };

        $scope.change_year = function () {
            $scope.fillyear = [];
            angular.foreach($scope.arrlist8, function (dd) {
                if (dd.asmay_id === parseint($scope.asmay_id)) {
                    var yearname = dd.asmay_year;
                    var yearname1 = yearname.split('-');
                    $scope.fillyear.push({ fmtp_year: yearname1[0] });
                    $scope.fillyear.push({ fmtp_year: yearname1[1] });
                }
            });
        }  

        $scope.all_checkm = function (checkallm) {
            var togglestatus = checkallm;
            angular.forEach($scope.monthname, function (itm) {
                itm.selected3 = togglestatus;
            });
            
            $scope.searchvaluedesg = '';
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
            var an = $scope.stdall;
            if ($scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(export_id, 'printDeviation');
                $timeout(function () {
                    location.href = exportHref;
                }, 100);
            }

            else {

                swal("select atleast one record!");
            }
        };

        $scope.printdatatable = [];
        $scope.optionToggledstd = function (SelectedStudentRecord, index) {

            $scope.stdall = $scope.getloaddata.every(function (itm) {
                return itm.stdselected;
            });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }

            if ($scope.printdatatable.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
        }

        $scope.toggleAllstd = function (stdall) {

            var toggleStatus = stdall;

            angular.forEach($scope.getloaddata, function (itm) {


                itm.stdselected = toggleStatus;
                if (itm.stdselected == true) {
                    $scope.printdatatable.push(itm);
                    //$scope.getloaddata.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                    //$scope.getloaddata.splice(itm);
                }
            });

            if ($scope.printdatatable.length != null) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }

        }


        //$scope.SendEmail = function () {

        //    $scope.tenantid = [];
        //    angular.forEach($scope.getloaddata, function (ty) {
        //        if (ty.stdselected == true) {

        //            $scope.tenantid.push({
        //                TENANTAGR_Id: ty.TENANTAGR_Id,
        //                TENANTMWBILLRW_Id: ty.TENANTMWBILL_Id
        //            });
        //        }
        //    })
        //    var data =
        //    {

        //        "TENANTEBWB_Id": $scope.tenantid
              
        //    };


        //    apiService.create("TenantWaterBillReport/sendmail", data).
        //        then(function (promise) {
        //            if (promise.returnmsg == "success") {
        //                swal("Email Sent..!!!");
        //                $state.reload();
        //            }
        //            else if (promise.returnmsg == "notFound") {
        //                swal("Email Not sent..!!!", 'Default Email-Id is Not Found.. !!!');
        //                $state.reload();
        //            }
        //            else if (promise.returnmsg == "Error") {
        //                swal("Something went wrong", 'Try After some time..!!');
        //                $state.reload();

        //            } else {
        //                swal("Something went wrong", 'Try After some time..!!');
        //                $state.reload();
        //            }
        //        });
        //};
    }
})();
