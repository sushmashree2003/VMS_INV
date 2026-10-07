(function () {
    'use strict';
    angular.module('app').controller('PC_Expenditure_Indent_MappingController', PC_Expenditure_Indent_MappingController)

    PC_Expenditure_Indent_MappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function PC_Expenditure_Indent_MappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

        $scope.submitted = false;
        $scope.maxdate = new Date();
        $scope.PCEXPTR_Date = new Date();
        $scope.obj = {};

        $scope.indentdetails = 0;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.divdetails = false;
        $scope.imgname = logopath;
        $scope.submitted = false;

        $scope.showdetails = false;

        $scope.ExpenditureIndentMappingLoaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";

            var pageid = 2;
            apiService.getURI("PC_Indent_Approval/ExpenditureIndentMappingLoaddata", pageid).then(function (promise) {
                $scope.getuserinstitution = promise.getuserinstitution;
                $scope.getindentapprovaldetails = promise.getindentapprovaldetails;
                $scope.getparticularsdetails = promise.getparticularsdetails;
                $scope.MI_Id = promise.mI_Id;
            });
        };
        /* On Change Institution */
        $scope.OnChangeExpenditureIndentMappingInstitution = function () {            
            
            $scope.getparticularsdetails = [];
            $scope.getindentapprovaldetails = [];        
            $scope.PCINDENTAP_Id = "";
            $scope.PCEXPTR_Id = "";
            $scope.showdetails = false;
            $scope.submitted = false;
            var data = {
                "MI_Id": $scope.MI_Id
            };
            apiService.create("PC_Indent_Approval/OnChangeExpenditureIndentMappingInstitution", data).then(function (promise) {
                $scope.getindentapprovaldetails = promise.getindentapprovaldetails;
                $scope.getparticularsdetails = promise.getparticularsdetails;
            });
        };

        /* On Change Partiuclar */
        $scope.OnChangeExpenditureIndentMappingParticular = function () {
            $scope.PCINDENTAP_Id = "";           
            $scope.showdetails = false;
            $scope.submitted = false;
        };

        /* On Change Indent */
        $scope.OnChangeExpenditureIndentMapping = function () {
            $scope.showdetails = false;
            $scope.submitted = false;
        };

        /* Search  */
        $scope.SearchExpenditureMappingDetails = function () {
            if ($scope.myForm.$valid) {
                $scope.getselectedindentdetails = [];
                $scope.getselectedindentparticulardetails = [];
                $scope.getselectedexpenparticularsdetails = [];
                $scope.showdetails = false;
                $scope.submitted = false;

                angular.forEach($scope.getparticularsdetails, function (dd) {
                    if (dd.pcexptR_Id === parseInt($scope.PCEXPTR_Id)) {
                        $scope.PCMPART_Id = dd.pcmparT_Id;
                    }
                });
                var data = {
                    "MI_Id": $scope.MI_Id,
                    "PCINDENTAP_Id": $scope.PCINDENTAP_Id,
                    "PCMPART_Id": $scope.PCMPART_Id,
                    "PCEXPTR_Id": $scope.PCEXPTR_Id
                };

                apiService.create("PC_Indent_Approval/SearchExpenditureMappingDetails", data).then(function (promise) {
                    $scope.getselectedindentdetails = promise.getselectedindentdetails;
                    $scope.getselectedindentparticulardetails = promise.getselectedindentparticulardetails;
                    var count = 0;
                    if ($scope.getselectedindentdetails !== null && $scope.getselectedindentdetails.length > 0) {
                        count += 1;
                        $scope.PCINDENTAPT_IndentNo = $scope.getselectedindentdetails[0].pcindentapT_IndentNo;
                        $scope.PCINDENTAPT_Date = $scope.getselectedindentdetails[0].pcindentapT_Date;
                        $scope.PCINDENTAPT_SanctionedAmt = $scope.getselectedindentdetails[0].pcindentapT_SanctionedAmt;
                        $scope.PCINDENTAPT_AmountSpent = $scope.getselectedindentdetails[0].pcindentapT_AmountSpent;
                        $scope.PCINDENTAPT_BalanceAmount = $scope.getselectedindentdetails[0].pcindentapT_BalanceAmount;
                    }

                    if ($scope.getselectedindentparticulardetails !== null && $scope.getselectedindentparticulardetails.length > 0) {
                        count += 1;
                        $scope.PCMPART_ParticularName = $scope.getselectedindentparticulardetails[0].pcmparT_ParticularName;
                        $scope.PCINDENTAPDT_SanctionedAmt = $scope.getselectedindentparticulardetails[0].pcindentapdT_SanctionedAmt;
                        $scope.PCINDENTAPDT_AmountSpent = $scope.getselectedindentparticulardetails[0].pcindentapdT_AmountSpent;
                        $scope.PCINDENTAPDT_BalanceAmount = $scope.getselectedindentparticulardetails[0].pcindentapdT_BalanceAmount;
                        $scope.PCINDENTAPDT_Remarks = $scope.getselectedindentparticulardetails[0].pcindentapdT_Remarks;
                    }

                    $scope.getselectedexpenparticularsdetails = promise.getselectedexpenparticularsdetails;
                    if ($scope.getselectedexpenparticularsdetails !== null && $scope.getselectedexpenparticularsdetails.length > 0) {
                        count += 1;
                        $scope.expenditureno = $scope.getselectedexpenparticularsdetails[0].pcexptR_ExpenditureNo;
                        $scope.expenditurerefno = $scope.getselectedexpenparticularsdetails[0].pcexptR_ReferenceNo;
                        $scope.employeename = $scope.getselectedexpenparticularsdetails[0].employeename;
                        $scope.PCMPART_ParticularName = $scope.getselectedexpenparticularsdetails[0].pcmparT_ParticularName;
                        $scope.PCEXPTR_Amount = $scope.getselectedexpenparticularsdetails[0].pcexptR_Amount;
                        $scope.PCEXPTR_Date = $scope.getselectedexpenparticularsdetails[0].pcexptR_Date;
                    }

                    if (count > 0) {
                        $scope.showdetails = true;
                    } else {
                        swal("No Details Found");
                    }
                });


            } else {
                $scope.submitted = true;
            }
        };

        $scope.SaveExpenditureMappingDetails = function () {

            if ($scope.myForm.$valid) {

                angular.forEach($scope.getparticularsdetails, function (dd) {
                    if (dd.pcexptR_Id === parseInt($scope.PCEXPTR_Id)) {
                        $scope.PCMPART_Id = dd.pcmparT_Id;
                    }
                });

                var data = {
                    "MI_Id": $scope.MI_Id,
                    "PCEXPTR_Id": $scope.PCEXPTR_Id,
                    "PCINDENTAP_Id": $scope.PCINDENTAP_Id,
                    "PCMPART_Id": $scope.PCMPART_Id                    
                };

                swal({
                    title: "Are you sure?",
                    text: "Do You Want To Update This Record Record?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("PC_Indent_Approval/SaveExpenditureMappingDetails", data).then(function (promise) {
                                if (promise.saveorupdate === "ReferenceNotGenerated") {
                                    swal("Reference No. Not Generated");
                                } else {
                                    if (promise.returnval === true) {
                                        swal("Record Saved Successfully");
                                    } else {
                                        swal("Failed To Save Record");
                                    }
                                }
                                $scope.clearData();
                            });
                        }
                        else {
                            swal("Update Cancelled");
                        }
                    });                
            } else {
                $scope.submitted = true;
            }
        };         

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.clearData = function () {          
            $scope.MI_Id = "";
            $scope.getuserinstitution = [];

            $scope.getselectedindentdetails = [];
            $scope.getselectedindentparticulardetails = [];
            $scope.getselectedexpenparticularsdetails = [];
            $scope.showdetails = false;
            $scope.submitted = false;

            $scope.PCINDENTAPT_IndentNo = "";
            $scope.PCINDENTAPT_Date = "";
            $scope.PCINDENTAPT_SanctionedAmt = "";
            $scope.PCINDENTAPT_AmountSpent = "";
            $scope.PCINDENTAPT_BalanceAmount = "";

            $scope.PCMPART_ParticularName = "";
            $scope.PCINDENTAPDT_SanctionedAmt = "";
            $scope.PCINDENTAPDT_AmountSpent = "";
            $scope.PCINDENTAPDT_BalanceAmount = "";
            $scope.PCINDENTAPDT_Remarks = "";

            $scope.expenditureno = "";
            $scope.expenditurerefno = "";
            $scope.employeename = "";
            $scope.PCMPART_ParticularName = "";
            $scope.PCEXPTR_Amount = "";
            $scope.PCEXPTR_Date = "";

            $scope.ExpenditureIndentMappingLoaddata();
        };

        $scope.search = '';

        $scope.filterValue1 = function (obj) {
            return ($filter('date')(obj.pcexptR_Date, 'dd/MM/yyyy').indexOf($scope.search) >= 0) ||
                (angular.lowercase(obj.employeename)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.pcexptR_ExpenditureNo)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.pcexptR_ModeOfPayment)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.pcmparT_ParticularName)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (JSON.stringify(obj.pcexptR_Amount)).indexOf($scope.search) >= 0;
        };

        $scope.Print = function () {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        };
    }
})();
