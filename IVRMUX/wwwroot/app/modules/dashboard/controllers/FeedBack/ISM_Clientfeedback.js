(function () {
    'use strict';
    angular.module('app').controller('ISM_ClientfeedbackController', ISM_ClientfeedbackController)

    ISM_ClientfeedbackController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache', 'myFactoryvisitor', '$q', '$sce']
    function ISM_ClientfeedbackController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache, myFactoryvisitor, $q, $sce) {
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.loaddata = function () {
            $scope.PCstatus = 'Pending';
            var pageid = 2;
            apiService.getURI("ClientInvoice/ClientData", pageid).then(function (promise) {
                $scope.clientlist = promise.clientlist;
                $scope.getfeedbackdata = promise.getfeedbackdata;
                $scope.AcademicList = promise.academicList;
                $scope.Modulelist = promise.modulelist;
                $scope.MonthList = promise.monthList;
                $scope.getfeedbackdatabyus = promise.getfeedbackdatabyus;
                

            });
        };
        $scope.OnIEeemployee = function (ISMMCLT_Id) {

            var data = {
                "ISMMCLT_Id": ISMMCLT_Id,

            };
            apiService.create("ClientInvoice/GetIEName", data).then(function (promise) {
                $scope.GetallIEnames = promise.getallIEnames;
            });
        };
        $scope.removedata2 = function () {

            if ($scope.searchradio == 'byus') {
                $scope.ASMAY_Id = "";
                $scope.ivrM_Month_Id = "";
                $scope.IVRMM_Id = "";
                $scope.ISMMCLT_Id = "";
                $scope.EndUser_No = "";
                $scope.Mangement_No = "";
                $scope.ICCF_IE_HRMEID = "";
                $scope.ConfirmationRemark = "";
                $scope.ICCF_Date = "";
            
                $scope.NodalOfficerRemark = "";
                $scope.MangmentRemark = "";
            }
            else {
                $scope.ParentNumber = "";
                $scope.ParentsRemarks = "";
                $scope.ASMAY_Id = "";
                $scope.ivrM_Month_Id = "";
                $scope.IVRMM_Id = "";
                $scope.ISMMCLT_Id = "";
               
            }
        }
        $scope.savefeedbackdetails = function () {
            $scope.submitted = true;
           


            if ($scope.searchradio === "receiving") {
                var data = {
                    "ICCF_ID": $scope.ICCF_ID,
                    "ISMMCLT_Id": $scope.ISMMCLT_Id,
                    "ICCF_Date": new Date($scope.ICCF_Date).toDateString(),
                    "ICCF_Flag": $scope.searchradio,
                    "asmaY_Id": $scope.ASMAY_Id,
                    "IMM_Id": $scope.ivrM_Month_Id,
                    "IVRMM_Id": $scope.IVRMM_Id,
                    "ParentsRemarks": $scope.ParentsRemarks,
                    "ParentsNumber": $scope.ParentNumber,
                    "PCstatus": $scope.PCstatus,
                }
            }
            else {
                var data = {
                    "ICCF_ID": $scope.ICCF_ID,
                    "ISMMCLT_Id": $scope.ISMMCLT_Id,
                    "ICCF_IE_HRMEID": $scope.ICCF_IE_HRMEID,
                    "EndUser_No": $scope.EndUser_No,
                    "Mangement_No": $scope.Mangement_No,
                    "NodalOfficerRemark": $scope.NodalOfficerRemark,
                    "MangmentRemark": $scope.MangmentRemark,
                    "ConfirmationRemark": $scope.ConfirmationRemark,
                    "ICCF_Date": new Date($scope.ICCF_Date).toDateString(),
                    "ICCF_Flag": $scope.searchradio,
                    "asmaY_Id": $scope.ASMAY_Id,
                    "IMM_Id": $scope.ivrM_Month_Id,
                    "IVRMM_Id": $scope.IVRMM_Id,
                    "PCstatus": $scope.PCstatus,


                }
            }

            apiService.create("ClientInvoice/savefeedbackdetails", data).
                then(function (promise) {
                    if (promise.returnvalue === "Update") {
                        swal("Success!", "Record Updated Successfully", "success");
                    }
                    else if (promise.returnvalue === "Add") {
                        swal("Success!", "Record Saved Successfully", "success");
                    }
                    else if (promise.returnvalue === "Error") {
                        swal("Error", "Record Not Saved/Updated Successfully", "error");
                    }
                    $state.reload();
                })
          
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cancel = function () {
            $state.reload();
        };

        $scope.Editfeedback = function (user2) {
            $scope.clientlist = [];
            var data = {
                "ICCF_ID": user2.ICCF_ID
            }
            apiService.create("ClientInvoice/Editclientfeedback", data).
                then(function (promise) {
                    $scope.Editfeedbackdata = promise.editfeedbackdata;
                    $scope.clientlist = promise.clientlist;
                    $scope.ISMMCLT_Id = $scope.Editfeedbackdata[0].ISMMCLT_Id;
                    $scope.ICCF_ID = $scope.Editfeedbackdata[0].ICCF_ID;
                    $scope.OnIEeemployee($scope.ISMMCLT_Id);
                    $scope.GetallIEnames11 = promise.getallIEnames
                    $scope.GetallIEnames = [];
                    $scope.ICCF_IE_HRMEID = $scope.Editfeedbackdata[0].ICCF_IE_HRMEID;
                    $scope.EndUser_No = $scope.Editfeedbackdata[0].EndUser_No;
                    $scope.PCstatus = $scope.Editfeedbackdata[0].PCstatus;
                    
                    $scope.ASMAY_Id = $scope.Editfeedbackdata[0].ASMAY_Id;
                    angular.forEach($scope.AcademicList, function (yy) {
                        angular.forEach($scope.Editfeedbackdata, function (uu) {
                            if (yy.asmaY_Id == uu.ASMAY_Id) {
                                yy.selected = true;
                            }
                        })
                    })
                    $scope.ParentNumber = $scope.Editfeedbackdata[0].ParentsNumber;
                    $scope.ParentsRemarks = $scope.Editfeedbackdata[0].ParentsRemarks;
                    $scope.IVRMM_Id = $scope.Editfeedbackdata[0].IVRMM_Id;
                    $scope.ivrM_Month_Id = $scope.Editfeedbackdata[0].IMM_Id;
                    $scope.ICCF_Flag = $scope.Editfeedbackdata[0].ICCF_Flag;


                    $scope.Mangement_No = $scope.Editfeedbackdata[0].Mangement_No;
                    $scope.NodalOfficerRemark = $scope.Editfeedbackdata[0].NodalOfficerRemark;
                    $scope.MangmentRemark = $scope.Editfeedbackdata[0].MangmentRemark;
                    $scope.ConfirmationRemark = $scope.Editfeedbackdata[0].ConfirmationRemark;
                    $scope.ICCF_Date = new Date($scope.Editfeedbackdata[0].ICCF_Date);
                    angular.forEach($scope.clientlist, function (yy) {
                        angular.forEach($scope.Editfeedbackdata, function (uu) {
                            if (yy.ismmclT_Id == uu.ISMMCLT_Id) {
                                yy.selected = true;
                            }
                        })
                    })

                    angular.forEach($scope.GetallIEnames11, function (dd) {
                        if (dd.HRME_Id == $scope.ICCF_IE_HRMEID) {
                            $scope.GetallIEnames.push(dd);
                        }
                    })
                })

        };



        $scope.Editfeedback2 = function (user2) {
            $scope.clientlist = [];
            var data = {
                "ICCF_ID": user2.ICCF_ID
            }
            apiService.create("ClientInvoice/Editclientfeedback", data).
                then(function (promise) {
                    $scope.Editfeedbackdata = promise.editfeedbackdata;
                    $scope.clientlist = promise.clientlist;
                    $scope.ISMMCLT_Id = $scope.Editfeedbackdata[0].ISMMCLT_Id;
                    $scope.ICCF_ID = $scope.Editfeedbackdata[0].ICCF_ID;
                    $scope.PCstatus = $scope.Editfeedbackdata[0].PCstatus;
                    
                    $scope.ASMAY_Id = $scope.Editfeedbackdata[0].ASMAY_Id;
                    angular.forEach($scope.AcademicList, function (yy) {
                        angular.forEach($scope.Editfeedbackdata, function (uu) {
                            if (yy.asmaY_Id == uu.ASMAY_Id) {
                                yy.selected = true;
                            }
                        })
                    })
                    $scope.ParentNumber = $scope.Editfeedbackdata[0].ParentsNumber;
                    $scope.ParentsRemarks = $scope.Editfeedbackdata[0].ParentsRemarks;
                    $scope.IVRMM_Id = $scope.Editfeedbackdata[0].IVRMM_Id;
                    $scope.ivrM_Month_Id = $scope.Editfeedbackdata[0].IMM_Id;
                    $scope.ICCF_Flag = $scope.Editfeedbackdata[0].ICCF_Flag;

                    $scope.ICCF_Date = new Date($scope.Editfeedbackdata[0].ICCF_Date);
                    angular.forEach($scope.clientlist, function (yy) {
                        angular.forEach($scope.Editfeedbackdata, function (uu) {
                            if (yy.ismmclT_Id == uu.ISMMCLT_Id) {
                                yy.selected = true;
                            }
                        })
                    })
                })

        };

    }
})();
