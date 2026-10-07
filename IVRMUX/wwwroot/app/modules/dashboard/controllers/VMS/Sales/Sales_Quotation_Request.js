(function () {
    'use strict';
    angular
        .module('app')
        .controller('SalesQuotationRequestController', SalesQuotationRequestController)

    SalesQuotationRequestController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache', 'blockUI', '$compile']
    function SalesQuotationRequestController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache, blockUI, $compile) {
        var paginationformasters;
        $scope.saveupdate = true;
        $scope.emailbtn = false;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        $scope.loaddata = function () {
            var pageid = 1;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.lead_details = false;
            apiService.getURI("Sales_Lead/getverifyleaddetails", pageid).then(function (promise) {
                if (promise.leadlist != null && promise.leadlist.length > 0) {
                    $scope.leadlist = promise.leadlist;
                    $scope.presentCountgrid = promise.finalleadlist.length;
                    $scope.finalleadlist = promise.finalleadlist
                    
                }

            })
        }



        $scope.getLeaddetails = function (id) {
            var data = {
                "ISMSLE_Id": id.ISMSLE_Id
            }
            apiService.create("Sales_Lead/getleaddetails", data).then(function (promise) {
                if (promise.get_lead_details != null && promise.get_lead_details.length > 0) {
                    $scope.get_lead_details = promise.get_lead_details;
                    $scope.FMSCOR_RefernceNo = $scope.get_lead_details[0].FMSCOR_RefernceNo;
                    $scope.ISMSLE_LeadAddress1 = $scope.get_lead_details[0].ISMSLE_LeadAddress1;
                    $scope.ISMSLE_ContactPerson = $scope.get_lead_details[0].ISMSLE_ContactPerson;
                    $scope.ISMSLE_ContactNo = $scope.get_lead_details[0].ISMSLE_ContactNo;
                    $scope.ISMSLE_EmailId = $scope.get_lead_details[0].ISMSLE_EmailId;
                    $scope.FMSCOR_Subject = $scope.get_lead_details[0].FMSCOR_Subject;
                    $scope.ISMSLE_ContactDesignation = $scope.get_lead_details[0].ISMSLE_ContactDesignation;
                    $scope.ISMSMPR_ProductName = $scope.get_lead_details[0].ISMSMPR_ProductName;
                    $scope.ISMSLE_Id = $scope.get_lead_details[0].ISMSLE_Id;
                } else {
                    swal('Quotation Value already generated !')
                    $state.reload();
                }
            });
        };


        $scope.obj = {};
        $scope.submitted = false;
        $scope.Update = function (dd) {
            //var Template = document.getElementById("asdf").innerHTML;

            if ($scope.myForm.$valid) {
                var data = {
                    "ISMSLE_Id": $scope.get_lead_details[0].ISMSLE_Id,
                    "ISMSLE_DedicatedHostingFlg": $scope.obj.ismslE_DedicatedHostingFlg,
                    "ISMSLE_DedicatedMobileAppFlg": $scope.obj.ismslE_DedicatedMobileAppFlg,
                    "ISMSLE_HostingServer": $scope.obj.ismslE_HostingServer,
                    "ISMSLE_QuotationValue": $scope.obj.ismslE_QuotationValue,
                    "ISMSLE_EmailId": $scope.get_lead_details[0].ismslE_EmailId,
                  //  "TemplateName": Template,
                   
                };
                apiService.create("Sales_Lead/updatedetails", data).then(function (promise) {
                    if (promise.msg === "Update") {
                        swal('Update Successfully')
                        $state.reload();
                    }
                    else {
                        swal('Opeartion Failed !')
                        $state.reload();
                    }

                });
            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.onselectyr = function (IMFY_Id) {
            var data = {
                "IMFY_Id": $scope.financialyr
            };

            apiService.create("Sales_Lead/get_leads", data).then(function (promise) {
                if (promise.leadlist !== null && promise.leadlist.length > 0) {
                    $scope.leadlist = promise.leadlist;
                }
                else {
                    swal('No Data Found..!!');
                }
            });
        };


        $scope.getLeaddetailsfinal = function (id) {
            var data = {
                "ISMSLE_Id": id.ISMSLE_Id
            }
            apiService.create("Sales_Lead/getleaddetailsfinal", data).then(function (promise) {
                if (promise.get_lead_detailsfinal != null && promise.get_lead_detailsfinal.length > 0) {
                    $scope.get_lead_detailsfinal = promise.get_lead_detailsfinal;
                    $scope.FMSCOR_RefernceNo = $scope.get_lead_detailsfinal[0].FMSCOR_RefernceNo;
                    $scope.ISMSLE_LeadAddress1 = $scope.get_lead_detailsfinal[0].ISMSLE_LeadAddress1;
                    $scope.ISMSLE_ContactPerson = $scope.get_lead_detailsfinal[0].ISMSLE_ContactPerson;
                    $scope.ISMSLE_ContactNo = $scope.get_lead_detailsfinal[0].ISMSLE_ContactNo;
                    $scope.ISMSLE_EmailId = $scope.get_lead_detailsfinal[0].ISMSLE_EmailId;
                    $scope.FMSCOR_Subject = $scope.get_lead_detailsfinal[0].FMSCOR_Subject;
                    $scope.ISMSLE_ContactDesignation = $scope.get_lead_detailsfinal[0].ISMSLE_ContactDesignation;
                    $scope.ISMSLEDM_DemoDate = $scope.get_lead_detailsfinal[0].ISMSLEDM_DemoDate;
                   
                } else {
                    swal('Quotation Value already generated !')
                    $state.reload();
                }
            });
        };

        $scope.CCEmail = "rajkumar@vapstech.com";

        $scope.SendMail = function (Restype) {
            $scope.validationfordocument = true;
            $scope.tmplt = [];
            $scope.doc_list2 = [];
            var Template = document.getElementById("calltemplate").innerHTML;
            $scope.tmplt.push({ hrmE_EmployeeCode: "1", TemplateString: Template });

                var data = {
                    "ManagerMailid": $scope.ISMSLE_EmailId,
                    "TeamLeadMailid": $scope.CCEmail,
                    Template: Template
                };
                apiService.create("Sales_Lead/sendmailDetails", data).
                    then(function (promise) {
                        if (promise.retrunMsg == "Success") {
                            swal('Email  Sent Successfully.');
                            $state.reload();
                        }
                        else if (promise.returnval == "Exist") {
                            swal('Resignation Already Applied.');
                        }
                        else if (promise.returnval == "AddedChecklist") {
                            swal('Checklist Uploaded Successfully..');
                            $state.reload();
                        }
                        else {
                            swal("Email  Sent Successfully!!");
                        }
                    });
            }
       
        
        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();