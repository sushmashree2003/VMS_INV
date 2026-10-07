(function () {
    'use strict';
    angular
        .module('app')
        .controller('Sales_Lead_CreationController', sales_Lead_CreationController)
    sales_Lead_CreationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$stateParams', 'Excel', '$timeout', '$q']
    function sales_Lead_CreationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $stateParams, Excel, $timeout, $q) {
        //======================================================
        var paginationformasters;
        $scope.saveupdate = true;
        $scope.emailbtn = false;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.lead_list = [];
        //$scope.objques = {};
        //scope.obj = {};
        //$scope.HRME_IDS = "";
        var ISMSLERaw_Id = "";
        //$scope.leadTempList = [];
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));


        $scope.searchValueNew = "";
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
        $scope.all_check = function () {
            var checkStatus2 = $scope.usercheck;
            angular.forEach($scope.product_dd, function (itm) {
                itm.selectedd2 = checkStatus2;
            });
        };

        $scope.validate = false;
        //=====================================

        $scope.isOptionsRequired = function () {

            return !$scope.product_dd.some(function (sec) {
                return sec.selectedd2;
            });
        }
        $scope.select_state = function (ss) {
            var id = ss;
            apiService.getURI("Sales_Lead/select_state", id).then(function (promise) {
                $scope.state_dd = promise.state_dd;
            });
        };

        $scope.select_District = function (ss) {
            var id = ss;
            apiService.getURI("Sales_Lead/select_District", id).then(function (promise) {
                $scope.district_dd = promise.district_dd;
            });
        };
        //=====================================
        $scope.load_date = function () {
            $scope.currentPage = 1; $scope.finalleadlist = [];
            $scope.currentPage1 = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.itemsPerPage1 = paginationformasters;
            var pageid = 2;
            apiService.getURI("Sales_Lead/load_all_lead", pageid).then(function (promise) {

                $scope.companylist = promise.companylist;
                $scope.MI_Id = promise.mI_Id;
                $scope.lead_list = promise.lead_list;
                $scope.presentCountgrid = promise.lead_list.length;
                $scope.category_dd = promise.category_dd;
                $scope.source_dd = promise.source_dd;
                $scope.product_dd = promise.product_dd;
                $scope.status_dd = promise.status_dd;
                $scope.country_dd = promise.country_dd;
                $scope.channalPatnerList = promise.channalPatnerList;
                $scope.telecaller = promise.telecaller;
                $scope.lead_array = promise.leadlists;
                $scope.viewdetails = promise.viewdetails;
                $scope.finalleadlist = promise.finalleadlist;
                $scope.IVRMMC_Id = "";
                $scope.IVRMMC_Id = promise.country_dd[0].ivrmmC_Id;
                $scope.select_state($scope.IVRMMC_Id);
                $scope.telecallerstatus = 0;
                $scope.telecallerstatus = promise.telecaller[0].Counts;
                if ($scope.telecallerstatus == 1) {
                    angular.forEach($scope.telecaller, function (cls) {
                        if (cls.id === promise.user_id) {
                            $scope.HRME_Id = cls.hrmE_Id;
                        }
                    });
                }
                

            });
        };
        $scope.get_rawDetails = function () {
            $scope.ISMSMST_Id = ""; $scope.IVRMMS_Id = ""; $scope.ISMSLE_Remarks = "";
            $scope.ISMSLE_LeadName = ""; $scope.ISMSLE_EmailId = ""; $scope.ISMSMSO_Id = ""; $scope.ISMSMCA_Id = ""; $scope.ISMSLE_ContactDesignation = "";
            $scope.ISMSLE_LeadCode = ""; $scope.ISMSLE_ContactNo = ""; $scope.ISMSLE_LeadAddress1 = ""; $scope.ISMSLE_ContactPerson = ""; ISMSLERaw_Id = "";
            angular.forEach($scope.finalleadlist, function (cls) {
                if (cls.ISMSLERaw_Id === $scope.ISMSLERaw_Id.ISMSLERaw_Id) {
                    $scope.ISMSLE_LeadName = cls.ISMSLE_LeadName;
                    $scope.ISMSLE_EmailId = cls.ISMSLERaw_EmailId;
                    $scope.ISMSLE_LeadCode = cls.ISMSLERaw_LeadCode;
                    $scope.ISMSLE_ContactNo = cls.ISMSLERaw_ContactNo;
                    $scope.ISMSLE_LeadAddress1 = cls.ISMSLERaw_LeadAddress1;
                    //$scope.ISMSMSO_Id = cls.Source;
                    if (cls.Source != "" && cls.Source != undefined && cls.Source != null) {
                        $scope.ISMSMSO_Id = cls.Source;
                    } else {
                        $scope.ISMSMSO_Id = "";
                    }

                    $scope.ISMSMCA_Id = cls.Category;
                    $scope.ISMSMCA_Id = cls.Category;
                    $scope.ISMSLE_ContactPerson = cls.Contact_Person;
                    $scope.ISMSLE_ContactDesignation = cls.Designation;
                    //$scope.ISMSLE_ContactDesignation = cls.Health;
                    $scope.ISMSMST_Id = cls.Health;
                    $scope.IVRMMS_Id = cls.State;
                    $scope.ISMSLE_Remarks = cls.Remarks;

                }
            });
            if ($scope.IVRMMS_Id > 0) {
                $scope.select_District($scope.IVRMMS_Id);
            }
            ISMSLERaw_Id = $scope.ISMSLERaw_Id.ISMSLERaw_Id;
        }
        

        $scope.getproduct = function (MI_Id) {
            var data = {
                "MI_Id": MI_Id

            }
            apiService.create("Sales_Lead/getproduct", data).then(function (promise) {
                $scope.category_dd = promise.category_dd;
                $scope.source_dd = promise.source_dd;
                $scope.product_dd = promise.product_dd;
                $scope.status_dd = promise.status_dd;
                $scope.lead_list = promise.lead_list;
                $scope.presentCountgrid = promise.lead_list.length;
            });
        };


        $scope.getLead = function (MI_Id) {
            var data = {
                "MI_Id": MI_Id,
                "ISMSMCP_Id": $scope.ISMSMCP_Id
            }
            apiService.create("Sales_Lead/getLead", data).then(function (promise) {

                $scope.lead_array = promise.leadlists;

            });
        }; $scope.onchangesource = function (sourceid) {
           // $scope.ISMSMSO_Id = "";
            apiService.getURI("Sales_Lead/checkemailtemplet", sourceid).then(function (promise) {
                $scope.emailbtn = promise.returnvalue;
            });
        };

        //$scope.saveLead = function () {
        //    swal({
        //        title: "Are you sure?",
        //        text: "Do you want to save the data?",
        //        type: "warning",
        //        showCancelButton: true,
        //        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,Save it",
        //        cancelButtonText: "Cancel",
        //        closeOnConfirm: true,
        //        closeOnCancel: true
        //    },
        //        function (isConfirm) {
        //            if (isConfirm) {
        //                $('#myModalswal').modal('show');
        //            }
        //            else {
        //                swal("Save data Cancelled!!!");
        //            }
        //        });
        //};

        $scope.ISMSLE_QuotationValue = 0;
        $scope.ISMSLE_HostingServer = "";
        $scope.ISMSLE_DedicatedHostingFlg = 0;
        $scope.ISMSLE_DedicatedMobileAppFlg = 0;

        $scope.saveLeadnew = function (trfal) {
            //if ($scope.ISMSLE_LeadAddress1 == "" || $scope.ISMSLE_LeadAddress1 == null) {
            //    swal("Enter the Address1");
            //}
            $scope.product_list = [];
            angular.forEach($scope.product_dd, function (cls) {
                if (cls.selectedd2 === true) {
                    $scope.product_list.push(cls);
                }

            });
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.checkoruncheck == true) {
                    $scope.checkoruncheck = true;
                }
                else {
                    $scope.checkoruncheck = false;

                }
                //if ($scope.ISMSLERaw_Id.ISMSLERaw_Id !=null &&  $scope.ISMSLERaw_Id.ISMSLERaw_Id > 0) {
                //    ISMSLERaw_Id = $scope.ISMSLERaw_Id.ISMSLERaw_Id;
                //}

                //ISMSLERaw_Id = $scope.ISMSLERaw_Id.ISMSLERaw_Id === undefined || $scope.ISMSLERaw_Id.ISMSLERaw_Id === null ||
                //    $scope.ISMSLERaw_Id.ISMSLERaw_Id === "" ? 0 : $scope.ISMSLERaw_Id.ISMSLERaw_Id;
                var data = {
                    product_list_save: $scope.product_list,
                    "ISMSLE_Id": $scope.id,
                    "MI_Id": $scope.MI_Id,
                  
                    "ISMSLE_LeadName": $scope.ISMSLE_LeadName,
                    "ISMSLE_LeadCode": $scope.ISMSLE_LeadCode,
                    "ISMSLE_ContactPerson": $scope.ISMSLE_ContactPerson,
                    "ISMSLE_LeadAddress1": $scope.ISMSLE_LeadAddress1,
                    "ISMSLE_LeadAddress2": $scope.ISMSLE_LeadAddress2,
                    "ISMSLE_LeadAddress3": $scope.ISMSLE_LeadAddress3,
                    "ISMSLE_Pincode": $scope.ISMSLE_Pincode,
                    "ISMSLE_ContactNo": $scope.ISMSLE_ContactNo,
                    "IVRMMC_Id": $scope.IVRMMC_Id,
                    "ISMSMST_Id": $scope.ISMSMST_Id,
                    "ISMSLE_EmailId": $scope.ISMSLE_EmailId,
                    "ISMSMCA_Id": $scope.ISMSMCA_Id,
                    "ISMSMSO_Id": $scope.ISMSMSO_Id,
                    "ISMSLE_Reference": $scope.ISMSLE_Reference,
                    "ISMSLE_ContactDesignation": $scope.ISMSLE_ContactDesignation,
                    "IVRMMS_Id": $scope.IVRMMS_Id,
                    "IVRMMCT_Id": $scope.IVRMMCT_Id,
                    "ISMSLE_StudentStrength": $scope.ISMSLE_StudentStrength,
                    "ISMSLE_StaffStrength": $scope.ISMSLE_StaffStrength,
                    "ISMSLE_NoOfInstitutions": $scope.ISMSLE_NoOfInstitutions,
                    "ISMSLE_Remarks": $scope.ISMSLE_Remarks,
                    "ISMSMCP_Id": $scope.ISMSMCP_Id,
                    "HRME_Id": $scope.HRME_Id,
                   // "HRME_Id": $scope.HRME_IDS.hrmE_Id,
                    "ISMSMCP_ChannelPartnerFlg": $scope.checkoruncheck,
                    "ISMSLE_VisitedDate": new Date($scope.ISMSLE_VisitedDate).toDateString(),
                    "sendemail": trfal,
                    "ISMSLE_QuotationValue": $scope.ISMSLE_QuotationValue,
                    "ISMSLE_HostingServer": $scope.ISMSLE_HostingServer,
                    "ISMSLE_DedicatedHostingFlg": $scope.ISMSLE_DedicatedHostingFlg,
                    "ISMSLE_DedicatedMobileAppFlg": $scope.ISMSLE_DedicatedMobileAppFlg,
                    "ISMSLE_Board": $scope.ISMSLE_Board,
                    "ISMSLERaw_Id": ISMSLERaw_Id
                };
                var swaltext = "";
                var btntext = "";
                if ($scope.saveupdate === true) {
                    if (trfal === true) {
                       // swaltext = "Do you want to save and send mail?";
                      // btntext = "Yes,Save and send mail";
                        swaltext = "Do you want to save?";
                        btntext = "Yes,Save";
                    }
                    else {
                        //swaltext = "Do you want to save without mail?";
                        //btntext = "Yes,Save without mail";

                        swaltext = "Do you want to save?";
                        btntext = "Yes,Save";
                    }
                }
                else {
                    if (trfal === true) {
                        //swaltext = "Do you want to update data and send mail?";
                        //btntext = "Yes,Update and send mail";
                        swaltext = "Do you want to update data?";
                        btntext = "Yes,Update";
                    }
                    else {
                        swaltext = "Do you want to update data?";
                        btntext = "Yes,Update data";
                    }
                }

                swal({
                    title: "Are you sure?",
                    text: swaltext,
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: btntext,
                    cancelButtonText: "Cancel",
                    closeOnConfirm: true,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("Sales_Lead/Save_Edit_SaleLead", data).then(function (promise) {
                                if (promise.return_value === "Add") {
                                    swal('Record Saved Successfully');
                                    $state.reload();
                                }
                                else if (promise.return_value === "Update") {
                                    swal('Record Update Successfully');
                                    $state.reload();
                                }
                                else if (promise.return_value === "duplicate") {
                                    if (promise.ismslE_TypeFlg == "lead") {
                                        swal(' Leads Already Created !');

                                    }
                                    else if (promise.ismslE_TypeFlg == "email") {
                                        swal('Email  Already  Exist  !');

                                    }
                                    else if (promise.ismslE_TypeFlg == "mobile") {
                                        swal('Mobile Number Already  Exist  !');

                                    }
                                    else {
                                        swal('Duplicate Exist!!');
                                    }
                                    
                                }
                                else {
                                    //swal('Please Contact  Administartor');
                                    swal('Record With Same Data Already Exists....');
                                }
                            });

                        }
                        else {
                            swal("Save data Cancelled!!!");
                        }
                    });
            }
            else {

                $scope.submitted = true;
            }
        };
        $scope.swalcancel = function () {
            //$('#myModalswal').modal('hide');
            //$('.modal-backdrop').remove();
            //swal("Save Data Cancelled!");
            $state.reload();
        };
        $scope.checkoruncheck = false;
        $scope.oncheck = function () {
            if ($scope.checkoruncheck == true) {
                $scope.checkoruncheck = true;
            }
            else {
                $scope.checkoruncheck = false;
            }

        };
       // $scope.HRME_Id = 0;
        $scope.edit = function (ss) {
            $scope.leadTempList = [];
            $scope.HRME_Id = 0;
            $scope.ISMSMSO_Id = "";
            $scope.source_dd = [];
            $scope.status_dd = [];

            $scope.validate = false;
            if ($scope.checkoruncheck == true) {
                $scope.checkoruncheck = true;
            }
            else {
                $scope.checkoruncheck = false;
            }

            if (ss.ISMSLE_Id != undefined && ss.ISMSLE_Id > 0) {
                var pageid = {
                    "ISMSLE_Id": ss.ISMSLE_Id,
                    "MI_Id": ss.MI_Id
                }
            }
            else if (ss > 0) {
                var pageid = {
                    "ISMSLE_Id": ss.ISMSLE_Id,
                    "MI_Id": $scope.MI_Id
                }
                $scope.checkoruncheck = true;
            }
           
     
          

            $scope.saveupdate = false;
           
            apiService.create("Sales_Lead/Sales_lead_edit", pageid).then(function (promise) {
                $scope.validate = true;
                $scope.source_dd = promise.source_dd;
                $scope.status_dd = promise.status_dd;
                $scope.ISMSLE_LeadName = promise.sales_lead_edit[0].ISMSLE_LeadName;
                $scope.ISMSLE_LeadCode = promise.sales_lead_edit[0].ISMSLE_LeadCode;
                $scope.ISMSLE_ContactPerson = promise.sales_lead_edit[0].ISMSLE_ContactPerson;
                $scope.ISMSLE_LeadAddress1 = promise.sales_lead_edit[0].ISMSLE_LEADADDRESS1;
                $scope.ISMSLE_LeadAddress2 = promise.sales_lead_edit[0].ISMSLE_LeadAddress2;
                $scope.ISMSLE_LeadAddress3 = promise.sales_lead_edit[0].ISMSLE_LeadAddress3;
                $scope.ISMSLE_Pincode = promise.sales_lead_edit[0].ISMSLE_Pincode;
                $scope.ISMSLE_ContactNo = promise.sales_lead_edit[0].ISMSLE_ContactNo;
                $scope.ismsmsT_StatusName = promise.sales_lead_edit[0].ismsmsT_StatusName;
                $scope.ISMSMST_Id = promise.sales_lead_edit[0].ISMSMST_Id;
                $scope.ISMSLE_EmailId = promise.sales_lead_edit[0].ISMSLE_EmailId;
                $scope.ISMSMCA_Id = promise.sales_lead_edit[0].ISMSMCA_Id;
                $scope.ismsmcA_CategoryName = promise.sales_lead_edit[0].ismsmcA_CategoryName;
                $scope.ISMSMSO_Id = promise.sales_lead_edit[0].ISMSMSO_Id;
                $scope.ismsmsO_SourceName = promise.sales_lead_edit[0].ISMSMSO_SourceName;
                $scope.ISMSLE_Reference = promise.sales_lead_edit[0].ISMSLE_Reference;
                $scope.ISMSLE_ContactDesignation = promise.sales_lead_edit[0].ISMSLE_ContactDesignation;
                $scope.IVRMMS_Id = promise.sales_lead_edit[0].IVRMMS_Id;
                $scope.IVRMMCT_Id = promise.sales_lead_edit[0].IVRMMCT_Id;
                $scope.ivrmmS_Name = promise.sales_lead_edit[0].IVRMMS_Name;
                $scope.ivrmmcT_Name = promise.sales_lead_edit[0].IVRMMCT_Name;
                $scope.ISMSLE_StudentStrength = promise.sales_lead_edit[0].ISMSLE_StudentStrength;
                $scope.ISMSLE_StaffStrength = promise.sales_lead_edit[0].ISMSLE_StaffStrength;
                $scope.ISMSLE_NoOfInstitutions = promise.sales_lead_edit[0].ISMSLE_NoOfInstitutions;
                $scope.ISMSLE_Remarkspromise = promise.sales_lead_edit[0].ISMSLE_Remarkspromise;
                $scope.ivrmmC_CountryName = promise.sales_lead_edit[0].ivrmmC_CountryName;
                $scope.ISMSLE_Remarks = promise.sales_lead_edit[0].ISMSLE_Remarks;
                $scope.ISMSLE_VisitedDate = new Date(promise.sales_lead_edit[0].ISMSLE_VisitedDate);
                //$scope.IVRMMC_Id = promise.sales_lead_edit[0].ivrmmC_Id;
                $scope.id = promise.sales_lead_edit[0].ISMSLE_Id;
                $scope.product_dd_temp = [];
                $scope.product_dd_temp = promise.sales_lead_edit_product_dd;
                $scope.product_dd = promise.product_dd;

                $scope.state_dd = promise.state_dd;
                $scope.district_dd = promise.district_dd;

                $scope.IVRMMC_Id = promise.ivrmmC_Id;
                $scope.IVRMMS_Id = promise.ivrmmS_Id;
                $scope.IVRMMCT_Id = promise.ivrmmcT_Id;


                $scope.emailbtn = promise.returnvalue;
                $scope.companylist = promise.companylist;
                $scope.MI_Id = promise.sales_lead_edit[0].MI_Id;
                $scope.HRME_Id = promise.sales_lead_edit[0].HRME_Id;
                $scope.checkoruncheck = promise.sales_lead_edit[0].ISMSMCP_CHANNELPARTNERFLG;
                $scope.ISMSMCP_Id = promise.sales_lead_edit[0].ISMSMCP_Id;
                //$scope.HRME_Id = promise.sales_lead_edit[0].hrmE_Id;
                //$scope.country_dd = promise.country_dd;
                if ($scope.checkoruncheck == true) {
                    $scope.checkoruncheck = true;
                }
                else {
                    $scope.checkoruncheck = false;
                }
                if ($scope.product_dd_temp.length > 0) {
                    angular.forEach($scope.product_dd_temp, function (ss) {
                        angular.forEach($scope.product_dd, function (qq) {
                            if (ss.ismsmpR_Id === qq.ismsmpR_Id) {
                                qq.selectedd2 = true;
                            }
                        });
                    });
                }
                if (promise.finalleadlist != null && promise.finalleadlist.length > 0) {
                    $scope.newcaste = {}; $scope.finalleadlist = [];
                    $scope.finalleadlist = promise.finalleadlist;
                    for (var i = 0; i < $scope.finalleadlist.length; i++) {
                        if (promise.sales_lead_edit[0].ISMSLERaw_Id == $scope.finalleadlist[i].ISMSLERaw_Id) {
                            $scope.finalleadlist[i].Selected = true;
                            $scope.ISMSLERaw_Id = $scope.finalleadlist[i];               
                            $scope.newcaste = promise.sales_lead_edit[0].ismsleraw_Id;
                        }
                    }
                }
               //16-12-24
                $scope.leadTempList = promise.leadTempList;

            });
        };

        //$scope.Sib = true;
        //$scope.siblings = [{ id: 'sibling1' }];
        //$scope.addNewsibling = function () {
        //    var newItemNo = $scope.siblings.length + 1;

        //    if (newItemNo <= 5) {
        //        $scope.siblings.push({ 'id': 'sibling' + newItemNo });
        //    }
        //};

        $scope.EmaiAndMob = true;
        $scope.EmaiAndMobs = [{ id: 'EmaiAndMob1' }];
        $scope.addNewEmaiAndMob = function () {
            var newItemNo = $scope.EmaiAndMobs.length + 1;
            if (newItemNo <= 5) {
                $scope.EmaiAndMobs.push({ 'id': 'EmaiAndMob' + newItemNo });
            }
        };


        //$scope.removeNewsibling = function (index) {
        //    var newItemNo = $scope.siblings.length - 1;
        //    $scope.siblings.splice(index, 1);

        //    if ($scope.siblings.length == 0) {
        //        $scope.Sib = false;
        //    }
        //};

        $scope.removeNewEmaiAndMob = function (index) {
            var newItemNo = $scope.EmaiAndMobs.length - 1;
            $scope.EmaiAndMobs.splice(index, 1);

            if ($scope.EmaiAndMobs.length == 0) {
                $scope.EmaiAndMob = false;
            }
        };

        //$scope.showAddsibling = function (sibling) {
        //    return sibling.id === $scope.siblings[$scope.siblings.length - 1].id;
        //};

        $scope.showAddEmaiAndMob = function (EmaiAndMob) {
            return EmaiAndMob.id === $scope.EmaiAndMobs[$scope.EmaiAndMobs.length - 1].id;
        };


        $scope.updateEmail = function (fll) {
            if (fll.selected) {
                $scope.fll = fll;
                $scope.isModalVisibleSwap = true;
                $('#updatemail').modal('show');
            }
            //$scope.leadid = lead.ISMSLE_Id;
            //$('#updatemail').modal('show');
        }

        $scope.submitted1 = false;
        $scope.savemailmob = function (lead) {
            if ($scope.myForm1.$valid) {
                $scope.ISMSLE_Id = lead.ISMSLE_Id;
                $scope.submitted1 = true;
                //$scope.siblingsnew = [];
                //if ($scope.siblings.length > 0) {
                //    angular.forEach($scope.siblings, function (itmx) {
                //        if (true) {
                //            $scope.siblingsnew.push({
                //                email: itmx.email,
                //                mobnum: itmx.mobnum
                //            });
                //        }
                //    });
                //}
                $scope.EmaiAndMobsnew = [];
                if ($scope.EmaiAndMobs.length > 0) {
                    angular.forEach($scope.EmaiAndMobs, function (itmx) {
                        if (true) {
                            $scope.EmaiAndMobsnew.push({
                                email: itmx.email,
                                mobnum: itmx.mobnum
                            });
                        }
                    });
                }

                var data = {
                    "SaveEmailMobNum": $scope.EmaiAndMobsnew,
                    "ISMSLE_Id": $scope.ISMSLE_Id
                }
                apiService.create("Sales_Lead/Sales_Lead_EmailMobNumSave", data)
                    .then(function (promise) {
                        if (promise.retrunMsg === "Add") {
                            swal('Email And Mobile Number Saved Successfully !');
                        }
                        else if (promise.retrunMsg === "Duplicate") {
                            swal("Email And Mobile Number Already Exists !");
                        }
                        else {
                            swal("Please Contact Administrator !");
                        }
                        $state.reload();
                    });
            }
            else {
                $scope.submitted1 = true;
            }
           
        }




        $scope.view = function (ss) {
            //$scope.ISMSMSO_Id = "";
            var pageid = {
                "ISMSLE_Id": ss.ISMSLE_Id,
                "MI_Id": ss.MI_Id
            }
            apiService.create("Sales_Lead/Sales_lead_edit", pageid).then(function (promise) {

                $scope.ISMSLE_LeadName_s = promise.sales_lead_edit[0].ISMSLE_LeadName;
                $scope.ISMSLE_LeadName_Code_s = promise.sales_lead_edit[0].ISMSLE_LeadCode;
                $scope.ISMSLE_ContactPerson_s = promise.sales_lead_edit[0].ISMSLE_CONTACTPERSON;
                $scope.ISMSLE_LeadAddress_s = promise.sales_lead_edit[0].ISMSLE_LEADADDRESS1;
               // $scope.ISMSLE_LeadAddress_s = promise.sales_lead_edit[0].ISMSLE_LeadAddress1;
                $scope.ISMSLE_ContactNo_s = promise.sales_lead_edit[0].ISMSLE_CONTACTNO;
                $scope.ismsmsT_StatusName_s = promise.sales_lead_edit[0].ISMSMST_STATUSNAME;
                $scope.ISMSMST_Id_s = promise.sales_lead_edit[0].ISMSMST_ID;
                $scope.ISMSLE_EmailId_s = promise.sales_lead_edit[0].ISMSLE_EmailId;
                $scope.ISMSMCA_Id_s = promise.sales_lead_edit[0].ISMSMCA_ID;
                $scope.ismsmcA_CategoryName_s = promise.sales_lead_edit[0].ISMSMCA_CATEGORYNAME;
                $scope.ISMSMSO_Id_s = promise.sales_lead_edit[0].ISMSMSO_ID;
                $scope.ismsmsO_SourceName_s = promise.sales_lead_edit[0].ISMSMSO_SOURCENAME;
                $scope.ISMSLE_Reference_s = promise.sales_lead_edit[0].ISMSLE_Reference;
                $scope.ISMSLE_ContactDesignation_s = promise.sales_lead_edit[0].ISMSLE_CONTACTDESIGNATION;
                $scope.IVRMMS_Id_s = promise.sales_lead_edit[0].IVRMMS_ID;
                $scope.ivrmmS_Name_s = promise.sales_lead_edit[0].IVRMMS_Name;
                $scope.ivrmmcT_Name = promise.sales_lead_edit[0].IVRMMCT_NAME;
                $scope.ISMSLE_StudentStrength_s = promise.sales_lead_edit[0].ISMSLE_STUDENTSTRENGTH;
                $scope.ISMSLE_StaffStrength_s = promise.sales_lead_edit[0].ISMSLE_STAFFSTRENGTH;
                $scope.ISMSLE_NoOfInstitutions_s = promise.sales_lead_edit[0].ISMSLE_NOOFINSTITUTIONS;
                $scope.ISMSLE_Remarkspromise_s = promise.sales_lead_edit[0].ISMSLE_REMARKSPROMISE;
                $scope.ivrmmC_CountryName_s = promise.sales_lead_edit[0].IVRMMC_CountryName;
                $scope.ISMSLE_Remarks_s = promise.sales_lead_edit[0].ISMSLE_REMARKS;
                $scope.IVRMMC_Id_s = promise.sales_lead_edit[0].IVRMMC_Id;
                $scope.id_s = promise.sales_lead_edit[0].ISMSLE_ID;
                $scope.product_dd_s = promise.sales_lead_edit_product_dd_product;

            })
        }
        $scope.deactiv_prde = function (bL, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (bL.ismslepR_ActiveFlag === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Product?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Sales_Lead/deactiv_prde", bL).
                            then(function (promise) {

                                if (promise.retbool === false) {
                                    swal('Sales Product  Deactivated Successfully');
                                }
                                else if (promise.retbool === true) {
                                    swal('Sales Product Activated Successfully');
                                }


                                $state.reload();
                            });
                    } else {
                        swal("Cancelled");

                    }

                });
        };

        $scope.exportToExcel = function (tableId) {
            var excelname = "LEAD REPORT";
            excelname = excelname.toUpperCase() + '.xls';
            var printSectionId = tableId;
            if ($scope.lead_list !== null && $scope.lead_list.length > 0) {
                var exportHref = Excel.tableToExcel(printSectionId, 'LEAD REPORT');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = excelname;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
                //$state.reload();
            }
            else {
                swal("Please Select Records to be Exported");
            }

        };

        $scope.cancel = function () {
            $state.reload();
        }

        $scope.cancelNew = function () {
            $scope.EmaiAndMobs.forEach(function (emailandmob) {
                emailandmob.email = '';
                emailandmob.mobnum = '';
            });
            //// Optionally, you can also reset the form if required
            //myForm1.$setPristine();  // Reset form state
            //myForm1.$setUntouched();
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
          
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;

        };

        $scope.searchByColumn = function (search, searchColumn) {

            if (search !== "" && search !== null && search !== undefined) {
                if (searchColumn !== "" && searchColumn !== null && searchColumn !== undefined) {
                    //$scope.lead_list = [];
                    var data = {
                        "EnteredData": search,
                        "SearchColumnFlag": searchColumn
                    };
                    //var config = {
                    //    headers: {
                    //        'Content-Type': 'application/json;'
                    //    }
                    //};
                    apiService.create("Sales_Lead/LeadSearchByColumn", data)
                        .then(function (promise) {
                            if (promise.lead_list != "" && promise.lead_list != null && promise.lead_list.length > 0) {
                               // $scope.search = "";
                                $scope.lead_list = promise.lead_list;
                                //console.log($scope.lead_list);
                                $scope.presentCountgrid = promise.lead_list.length;
                            }
                            else {
                                swal("No Records Found");
                                $state.reload();
                            }
                        });
                }
                else {
                    swal("Please Select The column Name to Search the Data..");
                }
            }
            else {
                swal("Please Enter Data To Be Searched In Search here.....");
            }
        };

        $scope.onchange = function (search) {
            if (search === "") {
                //$state.reload();
                $scope.lead_list = $scope.lead_list;
            }
          
        }

    }
})();