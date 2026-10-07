(function () {
    'use strict';
    angular
        .module('app')
        .controller('TenantPaidandPendinglistController', TenantPaidandPendinglistController)

    TenantPaidandPendinglistController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function TenantPaidandPendinglistController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {


        var paginationformasters;
        $scope.multidepartment = false;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.sendsmsemail = false;
        $scope.obj = {};

        $scope.onLoadGetData = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            //$scope.itemsPerPage = 5;
            $scope.search = "";

            var pageid = 2;
            apiService.getURI("TenantPaidandPendinglist/getalldetails", pageid).then(function (promise) {
                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {
                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }

                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;
                }
            });
        };
        $scope.getreport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

              
                var type = "";
                var tenantagrpaY_Flag = "";

                //if ($scope.monthdate == 'datewise') {
                //    $scope.obj.month = " "
                //    $scope.obj.year = " "

                //}
                //else if ($scope.monthdate == 'month&yearwise') {
                //    $scope.obj.start_Date = " "
                //    $scope.obj.end_Date = " "
                //}
                var data = {
                   
                    //"start_Date": $scope.obj.start_Date,
                    //"end_Date": $scope.obj.end_Date,
                    "monthid": $scope.obj.month,
                    "HRMLY_LeaveYear": $scope.obj.year,
                    "type": $scope.tenantpaidpending,
                    //"flag": $scope.monthdate,

                }
                apiService.create("TenantPaidandPendinglist/get_report", data).then(function (promise) {

                    if (promise.tenantpendingreport != null && promise.tenantpendingreport.length > 0) {

                        $scope.tenantpendingreport = promise.tenantpendingreport;
                        $scope.presentCountgrid = promise.tenantpendingreport.length;
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


        $scope.toggleAll = function () {

            $scope.sendsmsemail = false;

            var toggleStatus = $scope.all;
            if (toggleStatus == true) {
                angular.forEach($scope.tenantpendingreport, function (itm) {
                    itm.isSelected = true;
                });
                $scope.sendsmsemail = true;
            }
            else {
                angular.forEach($scope.tenantpendingreport, function (itm) {
                    itm.isSelected = false;
                });
            }

        }

        $scope.amtdetails = function () {
            $scope.sendsmsemail = false;
            $scope.all = $scope.tenantpendingreport.every(function (itm) {
                return itm.isSelected;

            });
            angular.forEach($scope.tenantpendingreport, function (itm) {
                if (itm.isSelected == true) {
                    $scope.sendsmsemail = true;
                }

            });
            
        };

        $scope.SendEmail = function () {

            var tenantpendingarr = [];

            angular.forEach($scope.tenantpendingreport, function (ty) {
                if (ty.isSelected == true) {

                    tenantpendingarr.push(ty);
                }
            })
            var data =
            {
                tenantpendingpaymentlistarr: tenantpendingarr
            };


            apiService.create("TenantPaidandPendinglist/sendmail", data).
                then(function (promise) {
                    if (promise.returnmsg == "success") {
                        swal("Email Sent..!!!");
                        $state.reload();
                    }
                    else if (promise.returnmsg == "notFound") {
                        swal("Email Not sent..!!!", 'Default Email-Id is Not Found.. !!!');
                        $state.reload();
                    }
                    else if (promise.returnmsg == "Error") {
                        swal("Something went wrong", 'Try After some time..!!');
                        $state.reload();

                    } else {
                        swal("Something went wrong", 'Try After some time..!!');
                        $state.reload();
                    }
                });


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



        $scope.interacted = function (field) {
            return $scope.submitted;
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

        //===========Clear Field
        $scope.Clear = function () {
            $state.reload();

        };



    }

})();