(function () {
    'use strict';
    angular
        .module('app')
        .controller('Referral_Lead_management_reportController', Referral_Lead_management_reportController);
    Referral_Lead_management_reportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', 'Excel', '$timeout'];
    function Referral_Lead_management_reportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter, Excel, $timeout) {

        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag === "ISMTASK") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }

        $scope.filename = '';
        $scope.tblsh = false;
        var vm = this;
        vm.gridOptions = {};
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.reverse = false;
        var path = '../../../../../../DEMOSAMPLE.xls';
        var paginationformasters = 10;
        $scope.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=C:\Users\praveen\Downloads/DEMOSAMPLE.xls";
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.components_list = [];
            $scope.Master_Employee_List = [];
            $scope.itemsPerPage = paginationformasters;
            var id = 2;
            var pageid = 2;
            apiService.getURI("ISM_Client_Project/getcmtrpt_data", pageid).
                then(function (promise) {


                    if (promise.closurestatus !== null && promise.closurestatus.length > 0) {
                        $scope.Closurestatus = promise.closurestatus;
                    }

                    //if (promise.bdElist !== null && promise.bdElist.length > 0) {
                    //    $scope.BDElist = promise.bdElist;
                    //}

                    if (promise.clientAcquisitionlist !== null && promise.clientAcquisitionlist.length > 0) {
                        $scope.ClientAcquisitionlist = promise.clientAcquisitionlist;
                    }

                    //if (promise.telecallerlist !== null && promise.telecallerlist.length > 0) {
                    //    $scope.Telecallerlist = promise.telecallerlist;
                    //}
                    //if (promise.regionlist !== null && promise.regionlist.length > 0) {
                    //    $scope.Regionlist = promise.regionlist;
                    //}

                    //if (promise.revenue !== null && promise.revenue.length > 0) {
                    //    $scope.Revenue = promise.revenue;
                    //}


                })
        };

        $scope.order = function (keyname) {

            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input) {
            $scope.SelectedFileForUploadzd = input.files;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    //once return success 
                    var reader = new FileReader();
                    reader.readAsDataURL(input.files[0]);
                    $scope.filename = input.files[0].name;
                    // alert($scope.filename);
                }
                else {
                    //swal("Please Upload the .xlsx file");
                    //return;
                    $scope.filename = input.files[0].name;
                }
            }
        };


   
    






        $scope.exportToExcel1 = function () {
            if ($scope.DuplicateDataArray.length > 0) {
                var exportHref = Excel.tableToExcel(exceldata, 'Duplicate Data');
                $timeout(function () { location.href = exportHref; }, 100);
            }
        };


        $scope.exportToExcelInvalidData = function () {
            if ($scope.InvalidData.length > 0) {
                var exportHref = Excel.tableToExcel(InvalidDataExcelData, 'Invalid Data');
                $timeout(function () { location.href = exportHref; }, 100);
            }
        };

        $scope.exportToExcel = function () {

            swal({
                title: "Are you sure?",
                text: "Do you want to Download the Duplicates Data into Excel",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        if ($scope.DuplicateDataArray.length > 0) {

                            var exportHref = Excel.tableToExcel(exceldata, 'Duplicate Data');
                            $timeout(function () { location.href = exportHref; }, 100);
                            swal("Downloded Successfully.");
                            // swal.close();
                            // $state.reload();

                        }

                    }
                    else {
                        swal("Cancelled");
                        // $state.reload();
                    }

                });
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //rawdata manual save//
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacteds = function (field) {
            return $scope.submitted11;
        };
      

        $scope.isOptionsRequiredCA = function () {
            return !$scope.ClientAcquisitionlist.some(function (options) {
                return options.selected;
            });
        };

        $scope.isOptionsRequiredclosure = function () {
            return !$scope.Closurestatus.some(function (options) {
                return options.selected;
            });
        };


        $scope.setmonth = function () {
            $scope.obj.month = 0;
        }
        $scope.showreport = function () {

            $scope.obj.givenflg = "";
            if ($scope.myForm.$valid) {

                if ($scope.obj.month == 0 && $scope.all1 != "CA" && $scope.all1 != "CLOSURE" && $scope.all1 != "Daily" && $scope.all1 != "Weekly") {
                    swal("Please Select Month....");
                    return;
                }



                $scope.flag = $scope.all1;
                var BDElists = [];
                var Revenues = [];
                var Closurestatuss = [];
                var Telecallerlists = [];
                var ClientAcquisitionlists = [];
                var RegionlistLists = [];


                ////BDE
                //angular.forEach($scope.BDElist, function (grp_t) {
                //    if (grp_t.selected) {
                //        BDElists.push(grp_t.HRME_Id);
                //    }
                //});

                ////Revenue
                //angular.forEach($scope.Revenue, function (grp_t) {
                //    if (grp_t.selected) {
                //        Revenues.push(grp_t.revenue_range);
                //    }
                //});
                //closure
                angular.forEach($scope.Closurestatus, function (grp_t) {
                    if (grp_t.selected) {

                        Closurestatuss.push(grp_t.ClosureStatus);
                    }
                });
                ////TELECALLER
                //angular.forEach($scope.Telecallerlist, function (grp_t) {
                //    if (grp_t.selected) {
                //        Telecallerlists.push(grp_t.HRME_Id);
                //    }
                //});



                //CA
                angular.forEach($scope.ClientAcquisitionlist, function (grp_t) {
                    if (grp_t.selected) {
                        ClientAcquisitionlists.push(grp_t.HRME_Id);
                    }
                });
                ////region
                //angular.forEach($scope.Regionlist, function (grp_t) {
                //    if (grp_t.selected) {
                //        RegionlistLists.push(grp_t.IVRMMS_Id);
                //    }
                //});



                $scope.fromdate11 = "";
                $scope.todate11 = "";
                $scope.fromdate11 = $filter('date')($scope.obj.fromdate, 'yyyy-MM-dd');
                $scope.todate11 = $filter('date')($scope.obj.todate, 'yyyy-MM-dd');
                $scope.submitted = true;

                if ($scope.obj.month == null || $scope.obj.month == undefined || $scope.obj.month || "") {
                    $scope.obj.month = 0;
                }
        

                var data = {
                    "flag": $scope.all1,
                    "fromdateNew": $scope.fromdate11,
                    "todateNew": $scope.todate11,
                    //"BDElists": BDElists,
                    //"Revenues": Revenues,
                    "Closurestatuss": Closurestatuss,
                    "Telecallerlists": Telecallerlists,
                    "ClientAcquisitionlists": ClientAcquisitionlists,
                    "RegionlistLists": RegionlistLists,
                    "month": Number($scope.obj.month)


                }
                //leaddatabasereportdetails
                apiService.create("ISM_Client_Project/SalesReferralLeadReport/", data).
                    then(function (promise) {
                        if (promise.reffralLeadReport != null && promise.reffralLeadReport.length > 0) {

                            $scope.ReffralLeadReport = promise.reffralLeadReport;

                        }
                        else {
                            swal("No Records Found");
                            $state.reload();

                        }
                    })
            }
            else {

                $scope.submitted = true;
            }
        };

        $scope.exportToExcel = function () {
            var excelnamemain = "Referral Sales Lead Report";
            var printSectionId = '#tableexcell';
            $scope.start_Date = new Date();
            $scope.end_Date = new Date();
            $scope.from_dateex = $filter('date')($scope.start_Date, 'yyyy-MM-dd');
            $scope.to_dateex = $filter('date')($scope.end_Date, 'yyyy-MM-dd');
            //excelnamemain = excelnamemain + '-' + $scope.from_dateex + ' To ' + $scope.to_dateex + '.xls';

            excelnamemain = excelnamemain + '.xls';

            var exportHref = Excel.tableToExcel(printSectionId, 'Referral Sales Lead Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelnamemain;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.exportToExcel11 = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };
   
        $scope.printData = function () {
            var innerContents = document.getElementById("tableexcell").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

    }
})();




