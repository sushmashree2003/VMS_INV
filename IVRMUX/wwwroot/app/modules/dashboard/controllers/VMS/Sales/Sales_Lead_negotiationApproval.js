(function () {
    'use strict';
    angular
        .module('app')
        .controller('Sales_Lead_negotiationApprovalController', Sales_Lead_negotiationApprovalController);

    Sales_Lead_negotiationApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce', '$timeout', 'Excel','$compile'];
    function Sales_Lead_negotiationApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce, $timeout, Excel, $compile) {

        $scope.checkbox1 = false;
        $scope.checkbox2 = false;


        $scope.itemsPerPageNew = 15;
        $scope.currentPageNew = 1;
        $scope.LeadModuleData11 = [];
        $scope.LeadModuleData22 = [];
        $scope.obj = {};
        $scope.sortReverse = true;
        $scope.editflag = false;
        $scope.updateflag = false;
        $scope.date = new Date();
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.yearlist = []; $scope.getinstallment = [];
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
        $scope.searchValueDeptd = ""; $scope.transrows = [{ itrS_Id: 'trans1' }];
        $scope.getAllleadtarget = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var id = 2;
            apiService.getURI("Lead_Target_Mapping/OnloadGetDetailsnego", id).
                then(function (promise) {
                    if (promise.leadnegotiation_Approvaldetails != null && promise.leadnegotiation_Approvaldetails.length > 0) {
                        $scope.leadnegotiation_Approvaldetails = promise.leadnegotiation_Approvaldetails;
                        angular.forEach($scope.leadnegotiation_Approvaldetails, function (dev) {
                            dev.transrows = [];
                        });
                        $scope.yearlist = promise.yearlist;
                        $scope.getinstallment = promise.getinstallment;
                        //getinstallment
                       // yearlist
                    }
                    $scope.leadnegotiation_detailsmd = [];
                    $scope.leadnegotiation_details = promise.leadnegotiation_details;
                    angular.forEach($scope.leadnegotiation_details, function (dev) {
                        if (dev.ISMS_NegotiationFlag == true && dev.Negotiation_Amount > 0) {
                            $scope.leadnegotiation_detailsmd.push(dev);
                        }
                    });
                    
                });
        };
        
        
        $scope.addpayment = function (obj) {
            $scope.obj.LeadId = 0;
            $('#Payment').modal('show');
            $scope.obj.LeadId = obj.ISMSLE_Id;
            var data = {
                "ISMSLE_Id": obj.ISMSLE_Id,
            }
            apiService.create("Lead_Target_Mapping/OnChangeLTM_Institution", data).then(function (promise) {
                if (promise.leadlist != null && promise.leadlist.length > 0) {
                    $scope.transrows = promise.leadlist;
                    angular.forEach($scope.transrows, function (emp) {
                        emp.ISMCLTPRP_PaymentDate = new Date(emp.ISMCLTPRP_PaymentDate);
                    });
                }

            });
        }
        $scope.AddToPaymentMou = function () {
            //AddToPayment          
            angular.forEach($scope.leadnegotiation_Approvaldetails, function (emp) {
                if ($scope.obj.LeadId == emp.ISMSLE_Id) {
                    for (var i = 0; i < $scope.transrows.length; i++) {
                        var FMI_Id = 0;
                        angular.forEach($scope.getinstallment, function (dev) {
                            if ($scope.transrows[i].FTI_Id == dev.ftI_Id) {
                                FMI_Id = dev.fmI_Id;
                            }
                        });
                        emp.transrows.push({
                            FTI_Id: $scope.transrows[i].FTI_Id,
                            ISMCLTPRP_Year: $scope.transrows[i].ISMCLTPRP_Year,
                            ISMCLTPRP_InstallmentAmt: 0,
                            ISMCLTPRP_Year: $scope.transrows[i].ISMCLTPRP_Year,
                            ISMCLTPRP_PaymentDate: new Date($scope.transrows[i].ISMCLTPRP_PaymentDate).toDateString(),
                            ISMCLTPRP_BalanceAmt: 0,
                            ISMCLTPRP_ExcessAmt: 0,
                            ISMCLTPRP_InstallmentName: $scope.transrows[i].ftI_Name,
                            FMI_Id: FMI_Id,
                        })
                    }
                    // emp.transrows.push($scope.transrows);

                }
            });

            $('#Payment').modal('hide');
        }
        $scope.get_category1 = function () {
            var data = {
                "ISMSLE_Id": $scope.Sales.ISMSLE_Id
            };
            apiService.create("Lead_Target_Mapping/get_quotation", data).
                then(function (promise) {

                    if (promise.perstudent_QutationReq != null && promise.perstudent_QutationReq.length > 0) {
                        $scope.perstudent_QutationReq = promise.perstudent_QutationReq;
                        $scope.amt = $scope.perstudent_QutationReq[0].AMt;
                        $scope.amt_4 = $scope.perstudent_QutationReq[0].AMt_4;
                        $scope.ISMSLE_StudentStrength = $scope.perstudent_QutationReq[0].ISMSLE_StudentStrength;
                        $scope.ISMSBOM_BOMName = $scope.perstudent_QutationReq[0].ISMSBOM_BOMName;
                        $scope.ISMSMPR_ProductName = $scope.perstudent_QutationReq[0].ISMSMPR_ProductName;
                        $scope.ISMSLE_Id = $scope.perstudent_QutationReq[0].ISMSLE_Id;
                    }
                });
        };

        $scope.editDemo = function (lead) {

            var data = {
                "ISMSLE_Id": lead.ISMSLE_Id,

            }
            apiService.create("Lead_Target_Mapping/Saveboomlistpackage", data).then(function (promise) {
                if (promise.productlist != null && promise.productlist.length > 0) {
                    $scope.get_deviationreport = promise.productlist;
                    $scope.employeeid = [];
                    angular.forEach($scope.get_deviationreport, function (dev) {
                        if ($scope.employeeid.length === 0) {
                            $scope.employeeid.push(dev);
                        } else if ($scope.employeeid.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.employeeid, function (emp) {
                                if (emp.ISMSLE_Id === dev.ISMSLE_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.employeeid.push(dev);
                            }
                        }
                    });
                    angular.forEach($scope.employeeid, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.get_deviationreport, function (dd) {
                            if (dd.ISMSLE_Id === ddd.ISMSLE_Id) {

                                $scope.templist.push(dd);
                            }
                        });
                        ddd.plannerdetails = $scope.templist;

                    });
                    $('#editDemoN').modal('show');

                }
            });
        }

        $scope.showprintdata = function (temp) {

            $scope.ismsboM_BOMName = temp.ISMSLET_HtmlName;
            var e1 = angular.element(document.getElementById("LeadIntimate"));
            $compile(e1.html(temp.ISMSLET_HtmlTemplate))(($scope));
            $('#myModal1').modal('show');

        }
        //SaveNegotiation
        $scope.SaveNegotiation = function () {
            $scope.submitted = true; $scope.leadlistarray = [];
            if ($scope.myForm.$valid) {
                if ($scope.leadnegotiation_Approvaldetails != null && $scope.leadnegotiation_Approvaldetails.length > 0) {
                    angular.forEach($scope.leadnegotiation_Approvaldetails, function (dd) {
                        if (dd.checked == true) {
                            $scope.leadlistarray.push({
                                Negotiation_Amount: dd.Negotiation_Amount,
                                ISMSLE_Id: dd.ISMSLE_Id,
                                SanctionLevelNo: dd.SanctionLevelNo,
                                ISMSALENA_Remarks: dd.ISMSALENA_Remarks,
                                HRPAON_FinalFlg: dd.HRPAON_FinalFlg,
                                 Project_Payment_Lead: dd.transrows,
                                ISMSLET_Id: dd.ISMSLET_Id,
                                ISMSLET_HtmlTemplate: dd.ISMSLET_HtmlTemplate,
                                ISMSLET_HtmlName: dd.ISMSLET_HtmlName
                            })
                        }
                    });
                        var data = {                 
                            
                            "leadlistarray": $scope.leadlistarray,
                            "msg": "MOU",
                            "ISMS_NegoMDApprovedFlag": true,
                            "ISMS_MOMFlag": true,
                        }
                        apiService.create("Lead_Target_Mapping/saveNegotiationdetails", data).then(function (promise) {
                            swal('Record Update Successfuly');
                            $state.reload();

                        })

                   
                }
              
            } 
        };
        $scope.SaveNegotiationApproval = function () {
            $scope.submitted = true; $scope.leadlistarray = [];
            if ($scope.myForm.$valid) {
                if ($scope.leadnegotiation_Approvaldetails != null && $scope.leadnegotiation_Approvaldetails.length > 0) {

                    angular.forEach($scope.leadnegotiation_Approvaldetails, function (dd) {
                        if (dd.checked == true) {
                            $scope.leadlistarray.push({
                                Negotiation_Amount: dd.Negotiation_Amount,
                                ISMSLE_Id: dd.ISMSLE_Id,
                                SanctionLevelNo: dd.SanctionLevelNo,
                                ISMSALENA_Remarks: dd.ISMSALENA_Remarks,
                                HRPAON_FinalFlg: dd.HRPAON_FinalFlg,
                                ISMSLET_Id: dd.ISMSLET_Id
                            })
                        }
                    });
                    if ($scope.leadlistarray.length > 0 && $scope.leadlistarray != undefined && $scope.leadlistarray != null && $scope.leadlistarray != "") {
                        var data = {
                            "ISMS_NegoMDApprovedFlag": true,
                            "ISMS_MOMFlag": true,
                            "leadlistarray": $scope.leadlistarray,
                            "msg": ""
                        }
                        apiService.create("Lead_Target_Mapping/saveNegotiationdetails", data).then(function (promise) {
                            swal('Record Updated Successfuly');
                            $state.reload();

                        })
                    }
                    else {
                        swal("Please select atleast One checkbox to proceed..");
                    }
                 


                }

            } 
        }
        //SaveNegotiationApproval 

        
        $scope.calculateMinAmount = function (user) {
            if ($scope.ISMSLET_Amount_IMD <= user.Negotiation_Amount) {
                swal('You Con not Enter max value');
                user.Negotiation_Amount = '';
            }
        }

        $scope.clearid = function () {
            $state.reload();
        }

        $scope.leadmodules = function (fll) {

            $scope.LeadModuleData11 = [];
            $scope.LeadModuleData22 = [];
            $scope.LeadNameNew11 = "";
            $scope.BomNameNew = "";
            $scope.fll = fll;

            var data = {
                //"ISMSLE_Id": fll.ISMSLE_Id,
                //"ISMSLBOM_Id": fll.ISMSBOM_IdNew  

                "ISMSLE_Id": 28200,
                "ISMSLBOM_Id": 53   

            }

            apiService.create("Lead_Target_Mapping/get_quotation", data).then(function (promise) {
                if (promise.leadloaddata1 != null && promise.leadloaddata1.length > 0) {
                    $scope.LeadModuleData11 = promise.leadloaddata1;

                    $scope.LeadNameNew11 = promise.leadloaddata1[0].ISMSLE_LeadName;
                    $scope.BomNameNew = promise.leadloaddata1[0].ISMSBOM_BOMName;
                }
                //if (promise.leadnegotiation_details != null && promise.leadnegotiation_details.length > 0) {
                //    $scope.LeadModuleData22 = promise.leadnegotiation_details;

                //}
                if (promise.leadloaddata1 != null && promise.leadloaddata1.length > 0) {
                    $('#leadmodule').modal('show');
                }
                else {
                    swal("No Modules Data Found..!!!");
                }
            })


        }

    }

})();