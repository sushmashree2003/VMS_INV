(function () {
    'use strict';
    angular
        .module('app')
        .controller('TransferCertificateController', TransferCertificateController)

    TransferCertificateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$timeout', 'Excel']
    function TransferCertificateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $timeout, Excel) {

        $scope.submitted = false;
        $scope.submitted2 = false;

      
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
      
       
        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;
        $scope.search2 = "";

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse2 = !$scope.reverse2; //if true make it false and vice versa
        }


        $scope.currentPage3 = 1;
        $scope.itemsPerPage3 = 10;
        $scope.search3 = "";

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse3 = !$scope.reverse3; //if true make it false and vice versa
        }



        $scope.showflag = false;
        $scope.showflag_stud = true;
        
        $scope.ascA_ApplyDate = new Date();

        $scope.loaddata = function () {
           
            var pageid = 2;
            apiService.getURI("TransferCertificate/getdetails", pageid).then(function (promise) {
                $scope.roletype = promise.roletype;
                if ($scope.roletype != undefined || $scope.roletype != null || $scope.roletype != "") {
                    if ($scope.roletype == "Student") {

                        $scope.studentdetails = promise.studentdetails;
                        if ($scope.studentdetails.length > 0) {
                            $scope.amsT_FirstName = promise.studentdetails[0].amsT_FirstName;
                            $scope.asmcL_ClassName = promise.studentdetails[0].asmcL_ClassName;
                            $scope.asmC_SectionName = promise.studentdetails[0].asmC_SectionName;
                            $scope.amsT_RegistrationNo = promise.studentdetails[0].amsT_RegistrationNo;
                            $scope.amsT_MobileNo = promise.studentdetails[0].amsT_MobileNo;
                            $scope.amsT_emailId = promise.studentdetails[0].amsT_emailId;
                            $scope.asmcL_Id = promise.studentdetails[0].asmcL_Id;
                            $scope.asmS_Id = promise.studentdetails[0].asmS_Id;
                            $scope.amsT_Id = promise.studentdetails[0].amsT_Id;
                        }
                        $scope.studlist = promise.studlist;
                    }
                    else if ($scope.roletype == "Principal") {
                       
                        $scope.applylist = promise.applylist;
                        $scope.aply_aprvlist = promise.aply_aprvlist;
                    }
                }
                else {

                }
              

            })
        }


        //====================================TC Apply.
        $scope.tcApply = function () {
           
            if ($scope.myForm.$valid) {
                var data = {
                    "ASCA_Id": $scope.ascA_Id,
                    "AMST_Id": $scope.amsT_Id,
                    "ASCA_CertificateType": $scope.ascA_CertificateType,
                    "ASCA_Reason": $scope.ascA_Reason,
                    "ASCA_ApplyDate": $scope.ascA_ApplyDate,
                }
                apiService.create("TransferCertificate/tcApply", data).
                    then(function (promise) {
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.ascA_Id > 0) {
                                        swal('Certificate Apply Updated Successfully!');
                                    }
                                    else {

                                        swal('Certificate Apply Successfully!');
                                    }
                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.ascA_Id > 0) {
                                            swal('Certificate Apply Not Update Successfully!');
                                        }
                                        else {
                                            swal('Certificate Not Apply Successfully!');
                                        }
                                    }
                                }
                            }
                            else {
                                swal("Record already exist");
                            }
                            $state.reload();
                        }
                        else {
                            swal("Kindly Contact Administrator!!!");
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        }
        //=====================================end!


        //=====================================Editdata
        $scope.editdata = function (user) {
                     

            $scope.amsT_FirstName = user.amsT_FirstName;
            $scope.asmcL_ClassName = user.asmcL_ClassName;
            $scope.asmC_SectionName = user.asmC_SectionName;
            $scope.amsT_RegistrationNo = user.amsT_RegistrationNo;
            $scope.amsT_MobileNo = user.amsT_MobileNo;
            $scope.amsT_emailId = user.amsT_emailId;
            $scope.asmcL_Id = user.asmcL_Id;
            $scope.asmS_Id = user.asmS_Id;
            $scope.amsT_Id = user.amsT_Id;
            $scope.ascA_CertificateType = user.ascA_CertificateType;
            $scope.ascA_ApplyDate = new Date(user.ascA_ApplyDate);
            $scope.ascA_Status = user.ascA_Status;
            $scope.ascA_Id = user.ascA_Id;
            $scope.ascA_Reason = user.ascA_Reason;
        }
        //========================================End



        //=================Activation/Deactivation--Record.
        $scope.deactiveY = function (user, SweetAlert) {
           
            $scope.ascA_Id = user.ascA_Id;

            var dystring = "";
            if (user.ascA_ActiveFlg == 1) {
                dystring = "Deactivate";
            }
            else if (user.ascA_ActiveFlg == 0) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("TransferCertificate/deactiveY", user).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }

                });
        }
        //================End----Activation/Deactivation--Record.


        //====================================editdatastud
        $scope.studeditdata = function (user) {
           
            $scope.showflag = true;
            $scope.showflag_stud = true;

            $scope.amsT_FirstName = user.amsT_FirstName;
            $scope.asmcL_ClassName = user.asmcL_ClassName;
            $scope.asmC_SectionName = user.asmC_SectionName;
            $scope.amsT_RegistrationNo = user.amsT_RegistrationNo;
            $scope.amsT_MobileNo = user.amsT_MobileNo;
            $scope.amsT_emailId = user.amsT_emailId;
            $scope.asmcL_Id = user.asmcL_Id;
            $scope.asmS_Id = user.asmS_Id;
            $scope.amsT_Id = user.amsT_Id;
            $scope.ascA_CertificateType = user.ascA_CertificateType;
            $scope.ascA_ApplyDate = new Date(user.ascA_ApplyDate);
            $scope.ascA_Status = user.ascA_Status;
            $scope.ascA_Id = user.ascA_Id;
            $scope.ascA_Reason = user.ascA_Reason;
        }
        //============================================end.



        //==============================Leave Approve
        $scope.certificateApproved = function () {
           
            if ($scope.myForm2.$valid) {
                var data = {
                    "ASCA_Id": $scope.ascA_Id,
                    "ASCAP_ApproveReason": $scope.ascaP_ApproveReason,
                }
                apiService.create("TransferCertificate/certificateApproved", data)
                    .then(function (promise) {
                        if (promise.returnval == true) {
                            swal('Certificate Approved Successfully!');
                        }
                        else {
                            swal('Certificate Not Approved Successfully!');
                        }
                        $state.reload();
                    });
            }
            else {
                $scope.submitted2 = true;
            }
        }
        //=======================================End



        //==============================Leave Reject
        $scope.certificateRejected = function () {
           
            if ($scope.myForm2.$valid) {
                var data = {
                    "ASCA_Id": $scope.ascA_Id,
                    "ASCAP_ApproveReason": $scope.ascaP_ApproveReason,
                }
                apiService.create("TransferCertificate/certificateRejected", data)
                    .then(function (promise) {
                        if (promise.returnval == true) {
                            swal('Certificate Rejected Successfully!');
                        }
                        else {
                            swal('Certificate Not Rejected Successfully!');
                        }
                        $state.reload();
                    });
            }
            else {
                $scope.submitted2 = true;
            }
        }
        //=======================================End



  
        $scope.Clearid = function () {
            $state.reload();
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        }; 
        $scope.interacted1 = function (field) {
            return $scope.submitted2;
        };


        ////////================================================================Print & Excel

        $scope.printPendingData = function () {
            if ($scope.filterValue2 !== null && $scope.filterValue2.length > 0) {
                var innerContents = document.getElementById("printpendinglst").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/RackReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
        }


        $scope.exportToExcelPending = function (printpendinglst) {
            debugger;
            var exportHref = Excel.tableToExcel(printpendinglst, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }


        $scope.print_Appr_RejLst = function () {
            if ($scope.filterValue3 !== null && $scope.filterValue3.length > 0) {
                var innerContents = document.getElementById("printApprRejclst").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/RackReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
        }


        $scope.exportToExcel_AppRejlst = function (printApprRejclst) {
            debugger;
            var exportHref = Excel.tableToExcel(printApprRejclst, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }


        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;






    }
})();

