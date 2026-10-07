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

                   

                    $scope.ststuslist = promise.channelParter_lead_Reportonloadstatus;
                    $scope.Regionlist = promise.channelParter_lead_Reportonloadregion;
                    $scope.cplist = promise.channelParter_lead_ReportonloadCP;





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




        $scope.Getcpll = function (cpAll) {
            var toggleStatus = $scope.obj.cpAll;
            angular.forEach($scope.cplist, function (itm) {
                itm.selected = toggleStatus;
            });

        };


        $scope.togchkbxmcp= function () {

            $scope.obj.cpAll = $scope.cplist.every(function (itm) {
                return itm.selected;
            });

            $scope.searchValueDesg == '';

        };


        $scope.Getclosureall = function (closurestatuslistAll) {
            var toggleStatus = $scope.obj.closurestatuslistAll;
            angular.forEach($scope.ststuslist, function (itm) {
                itm.selected = toggleStatus;
            });

        };


        $scope.togchkbxmclosure = function () {

            $scope.obj.closurestatuslistAll = $scope.ststuslist.every(function (itm) {
                return itm.selected;
            });

            $scope.searchValueDesg == '';

        };



        $scope.Getregionall = function (regionAll) {
            var toggleStatus = $scope.obj.regionAll;
            angular.forEach($scope.Regionlist, function (itm) {
                itm.selected = toggleStatus;
            });

        };


        $scope.togchkbxmregion = function () {

            $scope.obj.regionAll = $scope.Regionlist.every(function (itm) {
                return itm.selected;
            });

            $scope.searchValueDesg == '';

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



        $scope.isOptionsRequiredCp = function () {
            return !$scope.cplist.some(function (options) {
                return options.selected;
            });
        };

        $scope.isOptionsRequiredregion = function () {
            return !$scope.Regionlist.some(function (options) {
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


                $scope.flag = $scope.all1;
                var BDElists = [];
                var Revenues = [];
                var Closurestatuss = [];
                var Telecallerlists = [];
                var cplists = [];
                var RegionlistLists = [];


                angular.forEach($scope.Regionlist, function (grp_t) {
                    if (grp_t.selected) {

                        RegionlistLists.push(grp_t.IVRMMS_Id);
                    }
                });



                angular.forEach($scope.ststuslist, function (grp_t) {
                    if (grp_t.selected) {

                        Closurestatuss.push(grp_t.ClosureStatus);
                    }
                });
         


         
                angular.forEach($scope.cplist, function (grp_t) {
                    if (grp_t.selected) {
                        cplists.push(grp_t.ISMSMCP_Id);
                    }
                });
      


                $scope.fromdate11 = "";
                $scope.todate11 = "";
                $scope.fromdate11 = $filter('date')($scope.obj.fromdate, 'yyyy-MM-dd');
                $scope.todate11 = $filter('date')($scope.obj.todate, 'yyyy-MM-dd');
                $scope.submitted = true;



                var data = {
                    "flag": $scope.all1,
                    "fromdateNew": $scope.fromdate11,
                    "todateNew": $scope.todate11,
                  
                    "Closurestatuss": Closurestatuss,
                    
                    "cplists": cplists,
                    "RegionlistLists": RegionlistLists
                   


                }
                //leaddatabasereportdetails
                apiService.create("ISM_Client_Project/Ism_Sales_ChannelParter_lead_Report/", data).
                    then(function (promise) {
                        if (promise.ism_Sales_ChannelParter_lead_Report != null && promise.ism_Sales_ChannelParter_lead_Report.length > 0) {

                            $scope.ReffralLeadReport = promise.ism_Sales_ChannelParter_lead_Report;

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




