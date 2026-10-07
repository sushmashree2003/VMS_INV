(function () {
    'use strict';
    angular.module('app').controller('TenantOfflinePaymentReportController', TenantOfflinePaymentReportController)

    TenantOfflinePaymentReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$timeout', 'Excel']
    function TenantOfflinePaymentReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $timeout, Excel) {

        $scope.submitted = false;
      
      

     
       
     
        $scope.monthlist_temp = [];
        $scope.consolidata_id = [];
        $scope.get_deviationreport = [];
       
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.Loaddata = function () {
           
            $scope.search = "";

            var pageid = 2;
            apiService.getURI("TenantOfflinePaymentReport/onloaddata", pageid).then(function (promise) {

                $scope.getyear = promise.yeardata;
                $scope.grid_view = false;
                $scope.monthlist_temp = promise.monthdata;
                $scope.DisplayData1 = promise.displayTableOneDetails;
                $scope.advanceid = promise.displayTableOneDetails[0].tenantagR_Id;
                $scope.hrmb = promise.displayTableOneDetails[0].hrmB_ID;
                

            })
        };

        $scope.Clearid = function () {
            $state.reload();
        };


        //submit button
        $scope.submitted = false;
        $scope.get_paymentreport = function () {
            $scope.griddata = [];
            $scope.submitted = true;
            $scope.selected = [];
            
            angular.forEach($scope.monthlist_temp, function (mm) {
                if (mm.selected) {
                    $scope.selected.push({
                        IVRM_Month_Id: mm.ivrM_Month_Id
                           
                    });

                   // $scope.selected.push(mm);
                }
            })
            if ($scope.advanceid = true) {
                $scope.advanceid = true;
            }
            else {
                $scope.advanceid = false;
            }
            if ($scope.myForm.$valid) {
              
                if ($scope.allind == 'Rent') {
                    $scope.allind = 'Rent';
                }
                else if ($scope.allind == 'Electrical') {
                    $scope.allind = 'Electrical';
                }
                else if ($scope.allind == 'WaterBill') {
                    $scope.allind = 'WaterBill';
                }
                else {
                    $scope.allind = 'All';
                }
                
                var data = {
                    
                    "yrrrrs": $scope.fmtP_Years,
              //      selected_Inst: $scope.selected,
                    "monthlist": $scope.selected,
                    "type": $scope.allind,

                }
            }
            apiService.create("TenantOfflinePaymentReport/getpaymentreport", data).then(function (promise) {
                $scope.DisplayData1 = promise.displayTableOneDetails;
                if (promise.displayTableOneDetails.length > 0) {
                    $scope.grid_view = true;
                    $scope.TLength = promise.displayTableOneDetails.length;
                    $scope.DisplayData1 = promise.displayTableOneDetails;
             
                }
                else
                {
                    swal("No Records Found");


                    $scope.fmtP_Years = false;
                    $scope.selected = false;

                }

            })
        };

        $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.checkallm;
            angular.forEach($scope.monthlist_temp, function (itm) {
                itm.selected = checkStatus;
            });
        }


        $scope.togchkbx = function () {
            $scope.monthlist_temp.every(function (options) {
                return options.selected;
            });
        }

        $scope.isOptionsRequired = function () {
            return !$scope.monthlist_temp.some(function (options) {
                return options.selected;
            });
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.objdata = [];
        $scope.viewdocument = function (obj , obj1) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;
            $scope.objdata1 = obj1;

            var data = {
                "TENANTAGRPAY_Id": $scope.objdata,
                "IVRM_Month_Id": $scope.objdata1
            }
           
            apiService.create("TenantOfflinePaymentReport/viewuploadflies", data).then(function (promise) {
                if (promise !== null) {
                   
                    $scope.imagenew = promise.mI_Logo;
                    $scope.name = promise.editfiles[0].name;
                    $scope.email = promise.editfiles[0].email;
                    $scope.phonenumber = promise.editfiles[0].phonenumber;
                    $scope.agreementfromdate = promise.editfiles[0].agreementfromdate;
                    $scope.agreementtodate = promise.editfiles[0].agreementtodate;
                    $scope.tenantagrpaY_ReceiptNo = promise.editfiles[0].tenantagrpaY_ReceiptNo;
                    $scope.tenantagrpaY_Flag = promise.editfiles[0].tenantagrpaY_Flag;
                    $scope.ivrM_Month_Name = promise.editfiles[0].ivrm_month_Name;
                    $scope.tenantagrpaY_Amount = promise.editfiles[0].tenantagrpaY_Amount;
                    $scope.tenantagrpaY_PaymentDate = promise.editfiles[0].tenantagrpaY_PaymentDate;

                    $scope.disabledata = true;
                    

                    $scope.uploadfilesdetails = promise.editfiles;
                    if (promise.editfiles !== null && promise.editfiles.length > 0) {
                        $scope.uploaddocfiles = promise.editfiles;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.filepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
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

        $scope.All_Individual = function (allind) {
            $scope.DisplayData1 = [];
            if ($scope.allind === allind)
                $scope.disabledata = false;
            else
                // $scope.hrmE_Id = "";
                $scope.disabledata = true;
        };



    }
})();