(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_ClientProject_Mapping', ISM_ClientProject_Mapping)

    ISM_ClientProject_Mapping.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', '$compile']
    function ISM_ClientProject_Mapping($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, $compile) {

        $scope.itemsPerPage = 5;
        $scope.currentPage = 1;
        $scope.search = "";
        $scope.currentDate = new Date();
        $scope.obj = {};
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   
            $scope.reverse = !$scope.reverse; 
        }
        // ============================ng_click function for module (ALL)========================//
        $scope.searchchkbx = "";
        $scope.ISMSLE_Id = 0;
    
        $scope.radiochange = function (ee) {
            if (ee.flag === 'A') {

                ee.classname = 'neww';
            }
            else {

                ee.classname = 'oldd';
            }
        }
        //====================================clear data====================================//

        $scope.get_lead_product = function () {
            $scope.bomListNew = []; $scope.ModuleName = [];
            $scope.ISMSLE_LeadName = ""; $scope.ISMSLE_Id = 0;
            angular.forEach($scope.mompendinglist, function (d) {
                if (d.ISMSLET_Id == $scope.ISMSLET_Id.ISMSLET_Id && d.Flag == $scope.ISMSLET_Id.Flag) {

                    $scope.ISMSLE_Id = d.ISMSLE_Id

                    $scope.ISMMCLTPR_NodalOfficerName = d.ISMSLE_ContactPerson;
                    $scope.ISMMCLTPR_NodalOfficerEmailId = d.ISMSLE_EmailId;
                    $scope.ISMMCLTPR_NodalOfficerContactNo = d.ISMSLE_ContactNo;
                    // $scope.ISMMCLTPR_NodalOfficerContactNo = d.ISMSLE_ContactNo;
                    $scope.ISMSMOUA_ProductName = d.ISMSMPR_ProductName;
                    $scope.ISMMCLTPR_TotalStudent = d.ISMSLE_StudentStrength;
                    $scope.ISMMCLTPR_CostPerStudent = d.Negotiation_Amount;
                    $scope.ISMSMOU_PackageName = d.ISMSBOM_BOMName;
                    $scope.ISMMCLTPR_ProposalRefNo = d.FMSCOR_RefernceNo;
                    $scope.ISMMCLTPR_CostPerStudent = d.Negotiation_Amount;
                    $scope.ISMSLE_LeadName = d.ISMSLE_LeadNameNew;
                    $scope.ISMSLE_LeadAddress1 = d.ISMSLE_LeadAddress1;
                    $scope.ISMSLE_LeadAddress2 = d.ISMSLE_LeadAddress2;
                    $scope.ISMSLE_LeadAddress3 = d.ISMSLE_LeadAddress3;
                    $scope.ISMSLE_Pincode = d.ISMSLE_Pincode;
                    $scope.ISMSLE_ContactDesignation = d.ISMSLE_ContactDesignation;

                }
            });
            var data = {
                "ISMSLE_Id": $scope.ISMSLE_Id,
                "Type": "M"
            }
            apiService.create("Lead_mou_/lead_product", data).
                then(function (promise) {
                    $scope.ModuleName = promise.moduleName;

                    if (promise.momproductpendinglist != null && promise.momproductpendinglist.length > 0) {
                        angular.forEach(promise.momproductpendinglist, function (d) {
                            $scope.bomListNew.push({ ISMSBOM_Id: d.ISMSBOM_Id });
                        });
                        $scope.OnChangeBOMList();
                    }
                })
        }
        //added by praveenkumkar 
        $scope.submitted = false;
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.isOptionsRequired = function () {
            return !$scope.module_list.some(function (options) {
                return options.selected;
            });
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.leaddata = [];
        //==================load data=========================//
        $scope.submitted = false;
        $scope.loaddata = function () {
            var id = 2;
            $scope.search = "";
            apiService.getURI("Lead_mou_/OnloadGetDetails", id).
                then(function (promise) {
                    $scope.dataapproval = promise.leadtargetMapping_lts;
                    if (promise.mouApproval != null && promise.mouApproval.length > 0) {
                        $scope.leaddata = promise.mouApproval;  
                        angular.forEach($scope.leaddata, function (dd) {
                            dd.flag = "Approved";
                            dd.checked = false;
                        });
                    }

                });
        }
        $scope.savedata = function () {
            $scope.arrayList = [];
            if ($scope.leaddata != null && $scope.leaddata.length > 0) {           
                angular.forEach($scope.leaddata, function (d) {
                    if (d.checked == true) {
                        $scope.arrayList.push({
                            ISMMCLT_Id: d.ISMMCLT_Id,
                            OrderConfirm_Flg:"Approved",
                            ISMCL_ID: d.ISMCL_ID,
                            ISMOC__Remark: d.ISMOC__Remark
                        })
                    }
                });
            }
            var data = {
                "arrayLead": $scope.arrayList
            };
            swal({
                title: "Are you sure?",
                text:  "You are Approving Mou",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            }, function (isConfirm) {
                if (isConfirm) {
                    apiService.create("ISM_ClientProject_Mapping/SaveApprovalVerify", data).then(function (promise) {
                        if (promise.returnval) {
                            $('#popup111').modal('show');
                            swal('MOU Approved Successfully');

                        } else {
                            swal('Failed to save, please contact administrator');
                        }
                      
                        $state.reload();
                    });
                } else {
                    swal("Approval Cancelled!");
                }    
               
                });
           
        };  


        $scope.savedataReject = function () {
            $scope.arrayList = [];
            if ($scope.leaddata != null && $scope.leaddata.length > 0) {
                angular.forEach($scope.leaddata, function (d) {
                    if (d.checked == true) {
                        $scope.arrayList.push({
                            ISMMCLT_Id: d.ISMMCLT_Id,
                            OrderConfirm_Flg: "Hold",
                            ISMCL_ID: d.ISMCL_ID,
                            ISMOC__Remark: d.ISMOC__Remark
                        })
                    }
                });
            }
            var data = {
                "arrayLead": $scope.arrayList
            };
            swal({
                title: "Are you sure?",
                text: "You are Approving Mou",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            }, function (isConfirm) {
                if (isConfirm) {
                    apiService.create("ISM_ClientProject_Mapping/SaveApprovalVerify", data).then(function (promise) {
                        if (promise.returnval) {
                            $('#popup111').modal('show');
                            swal('MOU Rejected Successfully');

                        } else {
                            swal('Failed to save, please contact administrator');
                        }

                        $state.reload();
                    });
                } else {
                    swal("Approval Cancelled!");
                }

            });

        };

        

        //added by view templeate
        $scope.Viewtemplateone = function (itm) {
          
            $scope.ISMSLE_Id = 0;
            $scope.ISMMCLTPR_NodalOfficerName = ""; $scope.ISMMCLTPR_NodalOfficerEmailId = ""; $scope.ISMMCLTPR_NodalOfficerContactNo = ""; $scope.ISMMCLTPR_NodalOfficerContactNo = "";
            $scope.ISMSMOUA_ProductName = ""; $scope.ISMMCLTPR_TotalStudent = ""; $scope.ISMSMOU_PackageName = ""; $scope.ISMMCLTPR_ProposalRefNo = "";
            /*$scope.ISMMCLTPR_CostPerStudent = "";*/ $scope.ISMMCLTPR_CostPerStudent = ""; $scope.ISMSLE_LeadName = ""; $scope.ISMSLE_LeadAddress1 = "";
            $scope.ISMSLE_LeadAddress2 = ""; $scope.ISMSLE_LeadAddress3 = ""; $scope.ISMSLE_Pincode = ""; $scope.ISMSLE_ContactDesignation = ""; $scope.ISMSLE_EmailId = "";
            var items = {
                "pending": itm.Template,
                "msg": "FetchTemp",
                "ISMSBOM_Id": itm.ISMSBOM_Id,
                "ISMSLE_Id": itm.ISMSLE_ID
            }
            apiService.create("ISM_ClientProject_Mapping/viewtemplatedetails", items).then(function (promise) {
                var e1 = angular.element(document.getElementById("bindtemplate"));
                if (promise.templatelist != null && promise.templatelist.length > 0) {
                    $scope.ISMSLE_Id = itm.ISMSLE_ID;
                    $scope.bindtemplate = promise.templatelist;
                    $scope.xtra = $scope.bindtemplate[0].VMSMOUT_Template;
                    e1.html($scope.xtra);
                    $compile(e1.contents())($scope);

                    $scope.ModuleName = promise.moduleName;

                    $scope.leadprojectpayment = promise.leadprojectpayment;

                    $scope.groupedData = [];
                    $scope.leadprojectpayment.forEach(item => {
                        // Check if the YearFlag already exists in groupedData
                        let yearFlagGroup = $scope.groupedData.find(group => group.YearFlag === item.YearFlag);

                        if (!yearFlagGroup) {
                            // If not found, create a new group for this YearFlag
                            yearFlagGroup = {
                                YearFlag: item.YearFlag,
                                records: []
                            };
                            $scope.groupedData.push(yearFlagGroup);
                        }
                        // Push the current item into the appropriate yearFlagGroup
                        yearFlagGroup.records.push(item);
                    });
                    if (promise.momproductpendinglist1 != null && promise.momproductpendinglist1.length > 0) {
                        angular.forEach(promise.momproductpendinglist1, function (d) {
                            $scope.bomListNew.push({ ISMSBOM_Id: d.ISMSBOM_Id });
                        });
                        angular.forEach(promise.momproductpendinglist1, function (d) {
                            if (d.ISMSLE_Id == itm.ISMSLE_ID) {
                                $scope.ISMSLE_EmailId = d.ISMSLE_EmailId;
                                $scope.ISMMCLTPR_ProjectDuration = d.ISMMCLTPR_ProjectDuration;
                                $scope.ISMMCLTPR_MOURefNo = d.ISMMCLTPR_MOURefNo;
                                $scope.ISMMCLTPR_MOUDate = d.ISMMCLTPR_MOUDate;
                                $scope.ISMMCLTPR_ProposalRefNo = d.ISMMCLTPR_ProposalRefNo;
                                $scope.ISMMCLTPR_EnhancementPerYr = d.ISMMCLTPR_EnhancementPerYr;
                                $scope.ISMMCLTPR_MOUStartDate = d.ISMMCLTPR_MOUStartDate;
                                $scope.ISMMCLTPR_MOUEndDate = d.ISMMCLTPR_MOUEndDate;
                                $scope.Contract_Year_To = d.ISMMCLT_Contract_YearTo;
                                $scope.Contract_Year_From = d.ISMMCLT_Contract_YearFrom;
                                $scope.ISMMCLTPR_NodalOfficerName = d.ISMSLE_ContactPerson;
                                $scope.ISMMCLTPR_NodalOfficerEmailId = d.ISMSLE_EmailId;
                                $scope.ISMMCLTPR_NodalOfficerContactNo = d.ISMSLE_ContactNo;
                                $scope.ISMMCLTPR_NodalOfficerContactNo = d.ISMSLE_ContactNo;
                                $scope.ISMSMOUA_ProductName = d.ISMSMPR_ProductName;
                                $scope.ISMMCLTPR_TotalStudent = d.ISMSLE_StudentStrength;
                               // $scope.ISMMCLTPR_CostPerStudent = d.Negotiation_Amount;
                                $scope.ISMSMOU_PackageName = d.ISMSBOM_BOMName;
                                $scope.ISMMCLTPR_ProposalRefNo = d.FMSCOR_RefernceNo;
                                $scope.ISMMCLTPR_CostStudent = d.Negotiation_Amount;
                                $scope.ISMSLE_LeadName = d.ISMSLE_LeadNameNew;
                                $scope.ISMSLE_LeadAddress1 = d.ISMSLE_LeadAddress1;
                                $scope.ISMSLE_LeadAddress2 = d.ISMSLE_LeadAddress2;
                                $scope.ISMSLE_LeadAddress3 = d.ISMSLE_LeadAddress3;
                                $scope.ISMSLE_Pincode = d.ISMSLE_Pincode;
                                $scope.ISMSLE_ContactDesignation = d.ISMSLE_ContactDesignation;
                                if (d.ISPSIF_ItemsName == 'SMS Package') {
                                    $scope.ISPSIF_Quantity = d.ISPSIF_Quantity
                                }
                              
                            }
                        });
                        $scope.OnChangeBOMList();
                    }
                    $("#templatemodel").modal('show');
                  
                }
                else {
                    e1.html('');
                    swal("No Template Found");
                }
            })
        }

        //order acceptance
        $scope.Viewtemplateone1 = function (itm) {
        
            $scope.bomListNew = []; $scope.ModuleName = [];
            $scope.ISMSLE_LeadName = ""; $scope.ISMSLE_Id = 0; $scope.ISMSLET_Id = 0;
            $scope.services = [];
           
            var data = {
                "ISMSLE_Id": itm.ISMSLE_ID,
                "Type": "M",
                  "ISMSLET_Id": itm.ISMSLET_Id
            }
            apiService.create("Lead_mou_/lead_product", data).
                then(function (promise) {
                    $scope.ModuleName = promise.moduleName;
                    $scope.services = promise.approvedBy;

                    $scope.leadprojectpayment = promise.leadprojectpayment;

                    $scope.groupedData = [];
                    $scope.leadprojectpayment.forEach(item => {
                        // Check if the YearFlag already exists in groupedData
                        let yearFlagGroup = $scope.groupedData.find(group => group.YearFlag === item.YearFlag);

                        if (!yearFlagGroup) {
                            // If not found, create a new group for this YearFlag
                            yearFlagGroup = {
                                YearFlag: item.YearFlag,
                                records: []
                            };
                            $scope.groupedData.push(yearFlagGroup);
                        }
                        // Push the current item into the appropriate yearFlagGroup
                        yearFlagGroup.records.push(item);
                    });
                    

                    if (promise.momproductpendinglist1 != null && promise.momproductpendinglist1.length > 0) {
                        angular.forEach(promise.momproductpendinglist1, function (d) {
                            $scope.ISMSLE_Id = d.ISMSLE_Id;
                           
                            $scope.ISMMCLTPR_NodalOfficerContactNo = d.ISMSLE_ContactNo;
                            $scope.ISMMCLTPR_NodalOfficerName = d.ISMSLE_ContactPerson;
                            $scope.ISMMCLTPR_NodalOfficerEmailId = d.ISMSLE_EmailId;
                            $scope.ISMSMOUA_ProductName = d.ISMSMPR_ProductName;
                            $scope.ISMMCLTPR_TotalStudent = d.ISMSLE_StudentStrength;
                            $scope.ISMMCLTPR_CostPerStudent = d.Negotiation_Amount;
                            $scope.ISMSMOU_PackageName = d.ISMSBOM_BOMName;
                            $scope.ISMMCLTPR_ProposalRefNo = d.FMSCOR_RefernceNo;
                            $scope.ISMMCLTPR_CostPerStudent = d.Negotiation_Amount;
                            $scope.ISMSLE_LeadName = d.ISMSLE_LeadName;
                            $scope.ISMSLE_LeadAddress1 = d.ISMSLE_LeadAddress1;
                            $scope.ISMSLE_LeadAddress2 = d.ISMSLE_LeadAddress2;
                            $scope.ISMSLE_LeadAddress3 = d.ISMSLE_LeadAddress3;
                            $scope.ISMMCLTPR_ProjectDuration = d.ISMMCLTPR_ProjectDuration;
                            $scope.ISMSLE_ContactDesignation = d.ISMSLE_ContactDesignation;
                            $scope.ISMSLE_Pincode = d.ISMSLE_Pincode;
                            $scope.ISMSLE_ContactDesignation = d.ISMSLE_ContactDesignation;
                            $scope.ISMMCLTPR_MOURefNo = d.ISMMCLTPR_MOURefNo;
                      
                            if (d.ISPSIF_ItemsName == 'SMS Package') {
                                $scope.ISPSIF_Quantity = d.ISPSIF_Quantity
                            }
                        });
                      
                    }
                    $('#popup111').modal('show');
                })
        }

        //viewNDA
        $scope.ViewNDA = function (itm) {
            //  itm.Template = "MET";
            $scope.ISMSLE_Id = 0;
            $scope.ISMMCLTPR_NodalOfficerName = ""; $scope.ISMMCLTPR_NodalOfficerEmailId = ""; $scope.ISMMCLTPR_NodalOfficerContactNo = ""; $scope.ISMMCLTPR_NodalOfficerContactNo = "";
            $scope.ISMSMOUA_ProductName = ""; $scope.ISMMCLTPR_TotalStudent = ""; $scope.ISMSMOU_PackageName = ""; $scope.ISMMCLTPR_ProposalRefNo = "";
            $scope.ISMMCLTPR_CostPerStudent = ""; $scope.ISMMCLTPR_CostPerStudent = ""; $scope.ISMSLE_LeadName = ""; $scope.ISMSLE_LeadAddress1 = "";
            $scope.ISMSLE_LeadAddress2 = ""; $scope.ISMSLE_LeadAddress3 = ""; $scope.ISMSLE_Pincode = ""; $scope.ISMSLE_ContactDesignation = ""; $scope.ISMSLE_EmailId = "";
            var items = {
                "pending": itm.Template,
                "msg": "FetchTemp",
                "ISMSBOM_Id": itm.ISMSBOM_Id,
                "ISMSLE_Id": itm.ISMSLE_ID
            }
            apiService.create("ISM_ClientProject_Mapping/viewtemplatedetails", items).then(function (promise) {
                var e1 = angular.element(document.getElementById("bindtemplate"));
                if (promise.templatelist != null && promise.templatelist.length > 0) {
                    $scope.ISMSLE_Id = itm.ISMSLE_ID;
                    $scope.bindtemplate = promise.templatelist;
                    $scope.xtra = $scope.bindtemplate[0].VMSMOUT_Template;
                    e1.html($scope.xtra);
                    $compile(e1.contents())($scope);

                    $scope.ModuleName = promise.moduleName;
                    if (promise.momproductpendinglist1 != null && promise.momproductpendinglist1.length > 0) {
                        angular.forEach(promise.momproductpendinglist1, function (d) {
                            $scope.bomListNew.push({ ISMSBOM_Id: d.ISMSBOM_Id });
                        });
                        angular.forEach(promise.momproductpendinglist1, function (d) {
                            if (d.ISMSLE_Id == itm.ISMSLE_ID) {
                                $scope.ISMSLE_EmailId = d.ISMSLE_EmailId;
                                $scope.ISMMCLTPR_ProjectDuration = d.ISMMCLTPR_ProjectDuration;
                                $scope.ISMMCLTPR_MOURefNo = d.ISMMCLTPR_MOURefNo;
                                $scope.ISMMCLTPR_MOUDate = d.ISMMCLTPR_MOUDate;
                                $scope.ISMMCLTPR_ProposalRefNo = d.ISMMCLTPR_ProposalRefNo;
                                $scope.ISMMCLTPR_EnhancementPerYr = d.ISMMCLTPR_EnhancementPerYr;
                                $scope.ISMMCLTPR_MOUStartDate = d.ISMMCLTPR_MOUStartDate;
                                $scope.ISMMCLTPR_MOUEndDate = d.ISMMCLTPR_MOUEndDate;
                                $scope.ISMMCLT_Contract_YearTo = d.ISMMCLT_Contract_YearTo;
                                $scope.ISMMCLT_Contract_YearFrom = d.ISMMCLT_Contract_YearFrom;
                                $scope.ISMMCLTPR_NodalOfficerName = d.ISMSLE_ContactPerson;
                                $scope.ISMMCLTPR_NodalOfficerEmailId = d.ISMSLE_EmailId;
                                $scope.ISMMCLTPR_NodalOfficerContactNo = d.ISMSLE_ContactNo;
                                $scope.ISMMCLTPR_NodalOfficerContactNo = d.ISMSLE_ContactNo;
                                $scope.ISMSMOUA_ProductName = d.ISMSMPR_ProductName;
                                $scope.ISMMCLTPR_TotalStudent = d.ISMSLE_StudentStrength;
                                $scope.ISMMCLTPR_CostPerStudent = d.Negotiation_Amount;
                                $scope.ISMSMOU_PackageName = d.ISMSBOM_BOMName;
                                $scope.ISMMCLTPR_ProposalRefNo = d.FMSCOR_RefernceNo;
                                //$scope.ISMMCLTPR_CostPerStudent = d.Negotiation_Amount;
                                $scope.ISMSLE_LeadName = d.ISMSLE_LeadNameNew;
                                $scope.ISMSLE_LeadAddress1 = d.ISMSLE_LeadAddress1;
                                $scope.ISMSLE_LeadAddress2 = d.ISMSLE_LeadAddress2;
                                $scope.ISMSLE_LeadAddress3 = d.ISMSLE_LeadAddress3;
                                $scope.ISMSLE_Pincode = d.ISMSLE_Pincode;
                                $scope.ISMSLE_ContactDesignation = d.ISMSLE_ContactDesignation;

                            }
                        });

                        $scope.OnChangeBOMList();
                    }
                    $('#viewNDA').modal('show');
                }
                else {
                    e1.html('');
                    swal("No Template Found");
                }
            })

        }



        //after saving view order aceptance leaddata
        $scope.Viewtemplateone2 = function (itm) {
            $scope.ISMSLE_LeadName = ""; $scope.ISMSLE_Id = 0;
            $scope.ISMSLET_Id = 0;
            $scope.services = [];

            var data = {
                "ISMSLE_Id": itm.ISMSLE_ID,
                "Type": "E",
                "ISMSLET_Id": itm.ISMSLET_Id
            }
            apiService.create("Lead_mou_/lead_product", data).
                then(function (promise) {
                    $scope.ModuleName = promise.moduleName1;
                    $scope.services = promise.approvedBy;

                    $scope.leadprojectpayment = promise.leadprojectpayment;

                    $scope.groupedData = [];
                    $scope.leadprojectpayment.forEach(item => {
                      
                        let yearFlagGroup = $scope.groupedData.find(group => group.YearFlag === item.YearFlag);

                        if (!yearFlagGroup) {
                            // If not found, create a new group for this YearFlag
                            yearFlagGroup = {
                                YearFlag: item.YearFlag,
                                records: []
                            };
                            $scope.groupedData.push(yearFlagGroup);
                        }
                        // Push the current item into the appropriate yearFlagGroup
                        yearFlagGroup.records.push(item);
                    });
                    if (promise.momproductpendinglist1 != null && promise.momproductpendinglist1.length > 0) {
                        angular.forEach(promise.momproductpendinglist1, function (d) {
                            $scope.ISMSLE_Id = d.ISMSLE_Id;
                            $scope.ISMMCLTPR_NodalOfficerContactNo = d.ISMSLE_ContactNo;
                            $scope.ISMMCLTPR_NodalOfficerName = d.ISMSLE_ContactPerson;
                            $scope.ISMMCLTPR_NodalOfficerEmailId = d.ISMSLE_EmailId;
                            $scope.ISMSMOUA_ProductName = d.ISMSMPR_ProductName;
                            $scope.ISMMCLTPR_TotalStudent = d.ISMSLE_StudentStrength;
                            $scope.ISMMCLTPR_CostPerStudent = d.Negotiation_Amount;
                            $scope.ISMSMOU_PackageName = d.ISMSBOM_BOMName;
                            $scope.ISMMCLTPR_ProposalRefNo = d.FMSCOR_RefernceNo;
                            $scope.ISMMCLTPR_CostPerStudent = d.Negotiation_Amount;
                            $scope.ISMSLE_LeadName = d.ISMSLE_LeadName;
                            $scope.ISMSLE_LeadAddress1 = d.ISMSLE_LeadAddress1;
                            $scope.ISMSLE_LeadAddress2 = d.ISMSLE_LeadAddress2;
                            $scope.ISMSLE_LeadAddress3 = d.ISMSLE_LeadAddress3;
                            $scope.ISMSLE_Pincode = d.ISMSLE_Pincode;
                            $scope.ISMMCLTPR_EnhancementPerYr = d.ISMMCLTPR_EnhancementPerYr;
                            $scope.ISMMCLTPR_ProjectDuration = d.ISMMCLTPR_ProjectDuration;
                            $scope.ISMSLE_ContactDesignation = d.ISMSLE_ContactDesignation;
                            $scope.ISMMCLTPR_MOURefNo = d.ISMMCLTPR_MOURefNo;
                            if (d.ISPSIF_ItemsName == 'SMS Package') {
                                $scope.ISPSIF_Quantity = d.ISPSIF_Quantity
                            }
                        });
                    
                    }
                    $('#popup111').modal('show');
                })
        }

        //added by praveenkumar 
        $scope.loadtemplate = function () {
            $scope.ISMSLE_Id = 0;
            //$scope.ISMMCLTPR_NodalOfficerName = ""; $scope.ISMMCLTPR_NodalOfficerEmailId = ""; $scope.ISMMCLTPR_NodalOfficerContactNo = ""; $scope.ISMMCLTPR_NodalOfficerContactNo = "";
            //$scope.ISMSMOUA_ProductName = ""; $scope.ISMMCLTPR_TotalStudent = ""; $scope.ISMSMOU_PackageName = ""; $scope.ISMMCLTPR_ProposalRefNo = "";
            //$scope.ISMMCLTPR_CostPerStudent = ""; $scope.ISMMCLTPR_CostPerStudent = ""; $scope.ISMSLE_LeadName = ""; $scope.ISMSLE_LeadAddress1 = "";
            //$scope.ISMSLE_LeadAddress2 = ""; $scope.ISMSLE_LeadAddress3 = ""; $scope.ISMSLE_Pincode = ""; $scope.ISMSLE_ContactDesignation = "";
            var ISMSBOM_Id = 0; var ISMSLE_ID = 0;
            angular.forEach($scope.mompendinglist, function (d) {
                if (d.ISMSLET_Id == $scope.ISMSLET_Id.ISMSLET_Id) {
                    ISMSBOM_Id = d.ISMSBOM_Id;
                    ISMSLE_ID = d.ISMSLE_Id;
                }
            });

            $scope.ISMSLE_Id = ISMSLE_ID;
            var items = {
                "pending": $scope.Template,
                "msg": "FetchTemp",
                "ISMSBOM_Id": ISMSBOM_Id,
                "ISMSLE_Id": ISMSLE_ID
            }
            apiService.create("ISM_ClientProject_Mapping/viewtemplatedetails", items).then(function (promise) {
                var e1 = angular.element(document.getElementById("bindtemplate"));
                if (promise.templatelist != null && promise.templatelist.length > 0) {
                    $scope.bindtemplate = promise.templatelist;
                    $scope.xtra = $scope.bindtemplate[0].VMSMOUT_Template;
                    e1.html($scope.xtra);
                    $compile(e1.contents())($scope);
                    $scope.ModuleName = promise.moduleName;
                    if (promise.momproductpendinglist != null && promise.momproductpendinglist.length > 0) {
                        angular.forEach(promise.momproductpendinglist, function (d) {
                            $scope.bomListNew.push({ ISMSBOM_Id: d.ISMSBOM_Id });
                        });
                        angular.forEach(promise.momproductpendinglist, function (d) {
                            if (d.ISMSLE_Id == ISMSLE_ID) {
                                $scope.ISMMCLTPR_NodalOfficerName = d.ISMSLE_ContactPerson;
                                $scope.ISMMCLTPR_NodalOfficerEmailId = d.ISMSLE_EmailId;
                                $scope.ISMMCLTPR_NodalOfficerContactNo = d.ISMSLE_ContactNo;
                                $scope.ISMMCLTPR_NodalOfficerContactNo = d.ISMSLE_ContactNo;
                                $scope.ISMSMOUA_ProductName = d.ISMSMPR_ProductName;
                                $scope.ISMMCLTPR_TotalStudent = d.ISMSLE_StudentStrength;
                                $scope.ISMMCLTPR_CostPerStudent = d.Negotiation_Amount;
                                $scope.ISMSMOU_PackageName = d.ISMSBOM_BOMName;
                                $scope.ISMMCLTPR_ProposalRefNo = d.FMSCOR_RefernceNo;
                                $scope.ISMMCLTPR_CostPerStudent = d.Negotiation_Amount;
                                $scope.ISMSLE_LeadName = d.ISMSLE_LeadNameNew;
                                $scope.ISMSLE_LeadAddress1 = d.ISMSLE_LeadAddress1;
                                $scope.ISMSLE_LeadAddress2 = d.ISMSLE_LeadAddress2;
                                $scope.ISMSLE_LeadAddress3 = d.ISMSLE_LeadAddress3;
                                $scope.ISMSLE_Pincode = d.ISMSLE_Pincode;
                                $scope.ISMSLE_ContactDesignation = d.ISMSLE_ContactDesignation;

                            }
                        });

                        $scope.OnChangeBOMList();
                    }
                    $("#templatemodel").modal('show');
                }
                else {
                    e1.html('');
                    swal("No Template Found");
                }
            })
        }

        //Print Order Acceptance
        $scope.printData11 = function () {
            var innerContents = "";
            innerContents = document.getElementById("asdf").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
              
                '<style>@media print { img { border: none !important; } }</style>' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' +
                innerContents +
                '</body></html>');
            popupWinindow.document.close();
        };

        //added by praveenkumar 
        $scope.clearFunction = function () {
            $scope.ISMSLE_Id = 0;
            $scope.ISMMCLTPR_NodalOfficerName = ""; $scope.ISMMCLTPR_NodalOfficerEmailId = ""; $scope.ISMMCLTPR_NodalOfficerContactNo = ""; $scope.ISMMCLTPR_NodalOfficerContactNo = "";
            $scope.ISMSMOUA_ProductName = ""; $scope.ISMMCLTPR_TotalStudent = ""; $scope.ISMSMOU_PackageName = ""; $scope.ISMMCLTPR_ProposalRefNo = "";
            $scope.ISMMCLTPR_CostPerStudent = ""; $scope.ISMMCLTPR_CostPerStudent = ""; $scope.ISMSLE_LeadName = ""; $scope.ISMSLE_LeadAddress1 = "";
            $scope.ISMSLE_LeadAddress2 = ""; $scope.ISMSLE_LeadAddress3 = ""; $scope.ISMSLE_Pincode = ""; $scope.ISMSLE_ContactDesignation = "";
        }

        $scope.bomListNew = [];
        $scope.OnChangeBOMList = function (edits) {
            // $scope.bomList = [];
            $scope.amt = "";
            $scope.amt_4 = "";
            $scope.obj.ISMSLET_Amount_I = "";
            $scope.obj.ISMSLET_Amount_Final = "";
            var BOMLists = []; $scope.sales_CostPerMonth = []; $scope.bomTemplateList = [];
            if ($scope.bomListNew != null && $scope.bomListNew.length > 0) {
                angular.forEach($scope.bomListNew, function (d) {
                    BOMLists.push(d.ISMSBOM_Id);
                });
            }
            var data = {
                "BOMListt": BOMLists,
                "ISMSLE_Id": $scope.ISMSLE_Id,
            };
            apiService.create("ISM_ClientProject_Mapping/OnChangeOnChangeBOMList", data).then(function (promise) {
                if (promise.bomTemplateList != null && promise.bomTemplateList.length > 0) {
                    $scope.bomTemplateList = promise.bomTemplateList;
                    angular.forEach($scope.bomTemplateList, function (d) {
                        if (d.ISPSIF_ItemsName == 'SMS Package') {
                            $scope.ISPSIF_Quantity = d.ISPSIF_Quantity
                        }
                    });
                    // $scope.smsamt = promise.bomTemplateList[3].ISPSIF_Quantity;
                    $scope.sales_CostPerMonth = promise.sales_CostPerMonth;
                    $scope.taskid5 = [];
                    angular.forEach($scope.bomTemplateList, function (dev) {
                        if ($scope.taskid5.length === 0) {
                            $scope.taskid5.push({
                                ISMSPS_ID: dev.ISMSPS_ID,
                                ISMSPS_Service_Name: dev.ISMSPS_Service_Name,
                                ISMSPSI_ID: dev.ISMSPSI_ID,
                            });
                        }
                        else if ($scope.taskid5.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.taskid5, function (emp) {
                                if (emp.ISMSPS_ID === dev.ISMSPS_ID) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.taskid5.push({
                                    ISMSPS_ID: dev.ISMSPS_ID,
                                    ISMSPS_Service_Name: dev.ISMSPS_Service_Name,
                                    ISMSPSI_ID: dev.ISMSPSI_ID,
                                });
                            }
                        }
                    });
                    angular.forEach($scope.taskid5, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.bomTemplateList, function (dd) {
                            if (dd.ISMSPS_ID === ddd.ISMSPS_ID) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.bomTemplateList = $scope.templist;
                    });
                    $scope.asmaY_Year = $scope.bomTemplateList[0].ASMAY_Year;
                    //added By sanjeev 
                    if ($scope.ismsboM_BOMName == "HARDWARE") {
                        $scope.amt = 361200;
                        $scope.obj.ISMSLET_Amount_I = 361200;
                    }
                    else {
                        $scope.amt = $scope.sales_CostPerMonth[0].AMt;

                        if ($scope.ismsmoU_PackageName != 'Delite  Package') {
                            $scope.amt_4 = $scope.sales_CostPerMonth[0].AMt_4;

                        }
                        else {
                            $scope.amt_4 = 0;
                            $scope.obj.ISMSLET_Amount_Final = 0;
                        }
                    }
                } else {
                    swal('No Data Found ');
                    $state.reload();
                }
            });
        };
    }
})();
//======================================================================================================//
