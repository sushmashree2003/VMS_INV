(function () {
    'use strict';
    angular
        .module('app')
        .controller('vmsAluminiEmployeeBGVerificationController', vmsAluminiEmployeeBGVerificationController)
    vmsAluminiEmployeeBGVerificationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function vmsAluminiEmployeeBGVerificationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.PTax = {};
        // Get form Details at onload 
        $scope.onloadGetData = function () {
            var pageid = 2;
            apiService.getURI("HRAluminiEmployeeBGVerification/getalldetails", pageid).then(function (promise) {
                if (promise.employees !== null && promise.employees.length > 0) {
                    $scope.employeedropdown = promise.employees;

                }
                $scope.griddata = promise.griddata;
            });
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //=========//$scope.obj = {};==========Active/Deactive=====================////////////////////////////
        $scope.deactive = function (obj) {

            var mgs = "";
            var confirmmgs = "";
            if (obj.hrmeveR_ActiveFlg === true) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";

            }

            var data = {
                "HRMEVER_Id": obj.hrmeveR_Id,
                "HRME_Id": obj.hrmE_Id,

            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("HRAluminiEmployeeBGVerification/deactivate", data).then(function (promise) {

                            if (promise.retrunMsg === 'true') {
                                swal("Record " + confirmmgs + " " + "successfully", "", "success");
                            }
                            else {
                                swal("Record " + mgs + " Failed", "", "error");
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        $scope.start_Date = new Date();
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        $scope.Editdata = function (obj) {
            var data = {
                "HRMEVER_Id": obj.hrmeveR_Id,
                "HRME_Id": obj.hrmE_Id

            };

            apiService.create("HRAluminiEmployeeBGVerification/Editdata", data).then(function (promise) {
                
                if (promise !== null) {
                    $scope.editflag = true;
                    $scope.getdetails = promise.getdetails;

                    angular.forEach($scope.employeedropdown, function (dd) {
                        if (dd.hrmE_Id == obj.hrmE_Id) {
                            $scope.hrmE_Id = obj
                            dd.hrmE_Id = obj.hrmE_Id   
                            dd.select = true;                           
                        }
                    });

                    $scope.PTax.HRMEMNO_RequestedBy = promise.getdetails[0].hrmemnO_RequestedBy
                        $scope.PTax.HRMEVER_EmailId = promise.getdetails[0].hrmeveR_EmailId
                        $scope.PTax.HRMEVER_ContactNo = promise.getdetails[0].hrmeveR_ContactNo
                        $scope.PTax.HRMEVER_RequestedCompanyName = promise.getdetails[0].hrmeveR_RequestedCompanyName                    
                        $scope.start_Date = new Date(promise.getdetails[0].hrmeveR_GeneratedDate)
                        $scope.end_Date = new Date(promise.getdetails[0].hrmeveR_ExpiryDate)
                        //$scope.hrmE_Id = promise.getdetails[0].hrmE_Id
                       // $scope.HRME_EmployeeFirstName = promise.getdetails[0].hrmE_EmployeeFirstName
                    
                }
            });           
        };

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //send mail
        $scope.SendMail = function () {
            //   $scope.validationfordocument = true;
            $scope.tmplt = [];
            $scope.doc_list2 = [];
            //var Template = document.getElementById("calltemplate").innerHTML;
            //$scope.tmplt.push({ hrmE_EmployeeCode: "1", TemplateString: Template });

            if ($scope.myForm.$valid) {
                //angular.forEach($scope.documentList, function (emp) {
                //    if (emp.selectedd2 === true || emp.document_Path != null) {
                //        if (emp.document_Path.length > 0) {
                //            $scope.doc_list2.push({ document_Path: emp.document_Path, ISMRESGCL_FileName: emp.ismresgmcL_CheckListName, ISMRESGMCL_Id: emp.ismresgmcL_Id });
                //        }
                //        else {
                //            $scope.doc_list2.push({ document_Path: emp.ISMRESGMCL_Template, ISMRESGCL_FileName: emp.ismresgmcL_CheckListName, ISMRESGMCL_Id: emp.ismresgmcL_Id });
                //        }
                //    }
                //});

                var data = {
                    //"HRMEMNO_RequestedBy": $scope.PTax.HRMEMNO_RequestedBy,
                    "HRMEVER_EmailId": $scope.PTax.HRMEVER_EmailId,
                    "HRMEVER_ContactNo": $scope.PTax.HRMEVER_ContactNo,
                    "HRMEVER_RequestedCompanyName": $scope.PTax.HRMEVER_RequestedCompanyName,
                    "HRME_Id": $scope.hrmE_Id.hrmE_Id,
                    //"hrmE_EmployeeCode": $scope.PTax.hrmE_EmployeeCode,
                    "HRMEVER_GeneratedDate": new Date($scope.start_Date).toDateString(),
                    "HRMEVER_ExpiryDate": new Date($scope.end_Date).toDateString(),
                    "HRMEVER_Id": $scope.hrmE_Id.hrmeveR_Id,
                    //doc_list2: $scope.doc_list2,
                    //Template: Template
                }; 
                apiService.create("HRAluminiEmployeeBGVerification/sendResignationMail", data).
                    then(function (promise) {
                        if (promise.retrunMsg == "Success") {
                            swal('Mail Sent Successfully.');
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
                            swal('Operation Failed!!');
                        }
                    });
            }
            else { $scope.submitted = true; }
        };


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //save added by Rohan
        $scope.submitted = false;
        $scope.saveData = function (user) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HRMEMNO_RequestedBy": $scope.PTax.HRMEMNO_RequestedBy,
                    "HRMEVER_EmailId": $scope.PTax.HRMEVER_EmailId,
                    "HRMEVER_ContactNo": $scope.PTax.HRMEVER_ContactNo,
                    "HRMEVER_RequestedCompanyName": $scope.PTax.HRMEVER_RequestedCompanyName,
                    "HRME_Id": $scope.hrmE_Id.hrmE_Id,
                    "hrmE_EmployeeCode": $scope.PTax.hrmE_EmployeeCode,
                    "HRMEVER_GeneratedDate": new Date($scope.start_Date).toDateString(),
                    "HRMEVER_ExpiryDate": new Date($scope.end_Date).toDateString(),
                    "HRMEVER_Id": $scope.hrmE_Id.hrmeveR_Id
                };
                apiService.create("HRAluminiEmployeeBGVerification/", data).
                    then(function (promise) {
                        if (promise.returnvales != "") {
                            if (promise.returnvales == "Update") {
                                swal('Record Updated Successfully');
                            }
                            else if (promise.returnvales == "Add") {
                                swal('Record Saved Successfully');
                            }
                            else if (promise.returnvales == "false") {
                                swal('Record Not Saved/Updated successfully');
                            }
                            else if (promise.returnvales === "Duplicate") {
                                swal('Category Already Exist');
                            }
                            $state.reload();
                        }
                        else {
                            swal('Operation Failed');
                            $state.reload();
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.Clearid = function () {
            $state.reload();
        };



    }

})();