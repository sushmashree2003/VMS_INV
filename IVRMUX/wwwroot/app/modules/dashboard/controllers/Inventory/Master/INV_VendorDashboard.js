
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_DashController', INV_DashboardController);
    INV_DashboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$window'];
    function INV_DashboardController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $window) {
        var HostName = location.host;
        $scope.searchValue = '';
        $scope.typeflg = "Stock";
        $scope.expiredays = 0;
        $scope.CurrentDate = new Date();
        $scope.obj = {};
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }      
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage = 5;
            $scope.itemsPerPage1 = 5;
            $scope.search = "";
            $scope.Toatal_Count = "";
            $scope.Active_Count = "";
            $scope.Deactive_Count = "";
            $scope.ActiveGRN_Count = "";
            $scope.NVMGRN_TotalAmount = "";
            $scope.Total_VPAYVOUSGRN_ActualTotalAmount = "";
            $scope.Total_VPAYVOUSGRN_PaidAmount = "";
            $scope.Total_VPAYVOUSGRN_BalanceAmount = "";
            $scope.Supplier_Details = [];
            var pageid = 2;
            apiService.getURI("INV_LocationDashboard/getalldetails", pageid).then(function (promise) {
                if (promise.voucher_Details != null && promise.voucher_Details.length > 0) {
                    $scope.Toatal_Count = promise.voucher_Details[0].Toatal_Count;
                    $scope.Active_Count = promise.voucher_Details[0].Active_Count;
                    $scope.Deactive_Count = promise.voucher_Details[0].Deactive_Count;
                    $scope.Deactive_Count = promise.voucher_Details[0].Deactive_Count;
                    $scope.ActiveGRN_Count = promise.voucher_Details[0].ActiveGRN_Count;
                    $scope.NVMGRN_TotalAmount = promise.voucher_Details[0].NVMGRN_TotalAmount;
                    $scope.Total_VPAYVOUSGRN_ActualTotalAmount = promise.voucher_Details[0].Total_VPAYVOUSGRN_ActualTotalAmount;
                    $scope.Total_VPAYVOUSGRN_PaidAmount = promise.voucher_Details[0].Total_VPAYVOUSGRN_PaidAmount;
                    $scope.Total_VPAYVOUSGRN_BalanceAmount = promise.voucher_Details[0].Total_VPAYVOUSGRN_BalanceAmount;
                }
                else {
                    swal("Record  Not Found  !");
                }
                $scope.Supplier_Details = promise.supplier_Details;
               
            });

        };
        //added By sanjeev 
        $scope.OnLocationChange = function () {          
            $scope.employeedetails = [];
            $scope.Voucher_Details = [];
            $scope.locationdetails = [];
            $scope.SupplierDetails = [];
            $scope.suplierlist = [];
            $scope.SuplierName = "";
            $scope.AppliedAmount = 0;
            $scope.Approvedamount = 0;
            var data = {
                "HRME_Id": $scope.obj.INVMS_Id.INVMS_Id
            }
            apiService.create("INV_LocationDashboard/OnChange_Suplier", data).then(function (promise) {
                $scope.Voucher_Details = promise.voucher_Details;
                $scope.SupplierDetails = promise.supplier_Details;
                $scope.locationdetails = promise.locationdetails;
                angular.forEach($scope.Supplier_Details, function (dd) {
                    if (dd.INVMS_Id === $scope.obj.INVMS_Id.INVMS_Id) {
                        $scope.suplierlist.push(dd);
                        $scope.SuplierName = dd.INVMS_SupplierName;
                    }
                });
                if ($scope.Voucher_Details != null && $scope.Voucher_Details.length > 0) {
                    var AppliedAmount = 0;
                    var Approvedamount = 0;
                    angular.forEach($scope.Voucher_Details, function (dd) {
                        if (dd.INVMS_Id === $scope.obj.INVMS_Id.INVMS_Id) {
                            AppliedAmount = AppliedAmount + dd.VPAYVOU_AppliedAmount;
                            Approvedamount = Approvedamount + dd.VPAYVOU_ApprovedAmount;
                        }
                    });
                }
            });

        }
        $scope.printNew = function () {
            var innerContents = "";
            innerContents = document.getElementById("workorder").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.Viewtemplateone = function () {
            if ($scope.Template != null && $scope.Template != "") {
                
                $scope.invmS_GSTNo = "";
                $scope.supplieraddress = "";
                $scope.suppliername = "";
                $scope.suppliercontactno = "";
                $scope.emailid = "";
                $scope.ConatctPerson = "";
                $("#templatemodel").modal('show');
                angular.forEach($scope.get_supplier, function (dd) {
                    if (dd.invmS_Id == $scope.obj.invmS_Id.INVMS_Id) {
                        $scope.supplieraddress = dd.invmS_SupplierAddress;
                        $scope.suppliername = dd.invmS_SupplierName;
                        $scope.suppliercontactno = dd.invmS_SupplierConatctNo;
                        $scope.emailid = dd.invmS_EmailId;
                        $scope.ConatctPerson = dd.invmS_SupplierConatctPerson;
                        $scope.invmS_GSTNo = dd.invmS_GSTNo;
                        return;
                    }
                });

            }
            else {
                swal("Pls Select  Template name !");
            }

        }
        $scope.previewimg_new = function (img) {
            $scope.vbabrsF_FilePath = img;
            var img = $scope.vbabrsF_FilePath;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.vbabrsF_FilePath);
                    $('#myimagePreview').modal('show');
                    //$('#myModalfile').modal('show');
                }
                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                    $window.open($scope.vbabrsF_FilePath);
                }
                else if (lastelement == 'pdf') {
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.vbabrsF_FilePath;
                    $http.get(imagedownload1, { responseType: 'arraybuffer' })
                        .success(function (response) {
                            var fileURL = "";
                            var file = "";
                            var embed = "";
                            var pdfId = "";
                            file = new Blob([(response)], { type: 'application/pdf' });
                            fileURL = URL.createObjectURL(file);
                            pdfId = document.getElementById("pdfIdzz");
                            pdfId.removeChild(pdfId.childNodes[0]);
                            embed = document.createElement('embed');
                            embed.setAttribute('src', fileURL);
                            embed.setAttribute('type', 'application/pdf');
                            embed.setAttribute('width', '100%');
                            embed.setAttribute('height', '1000');
                            pdfId.appendChild(embed);
                            $('#showpdf').modal('show');
                        });
                }
            }
            else {
                $window.open($scope.vbabrsF_FilePath)
            }
        };
    }
})();