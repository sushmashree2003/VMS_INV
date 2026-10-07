(function () {
    'use strict';
    angular.module('app').controller('PC_ExpenditureController', PC_ExpenditureController)

    PC_ExpenditureController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function PC_ExpenditureController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

        $scope.submitted = false;
        $scope.editflag = false;
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

        $scope.ExpenditureLoaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";

            var pageid = 2;
            apiService.getURI("PC_Indent_Approval/ExpenditureLoaddata", pageid).then(function (promise) {
                $scope.getuserinstitution = promise.getuserinstitution;
                $scope.getindentapprovaldetails = promise.getindentapprovaldetails;
                $scope.getparticularsdetails = promise.getparticularsdetails;
                $scope.getexpenditureloaddata = promise.getexpenditureloaddata;
                $scope.MI_Id = promise.mI_Id;
               // $scope.getemp = promise.getemp;
                $scope.getinstitutiondetails = promise.getinstitutiondetails;
            });
        };

        $scope.OnChangeExpenditureInstitution = function () {
            $scope.divdetails = false;
            $scope.submitted = false;
            $scope.getparticularsdetails = [];
            $scope.getindentapprovaldetails = [];
            $scope.getexpenditureloaddata = [];
            $scope.getinstitutiondetails = [];
            $scope.getemp = [];
            $scope.PCINDENTAP_Id = "";
            $scope.HRME_Id = "";
            $scope.indentdetails = 0;
            var data = {
                "MI_Id": $scope.MI_Id
            };
            apiService.create("PC_Indent_Approval/OnChangeExpenditureInstitution", data).then(function (promise) {
                $scope.getindentapprovaldetails = promise.getindentapprovaldetails;
                $scope.getparticularsdetails = promise.getparticularsdetails;
                $scope.getexpenditureloaddata = promise.getexpenditureloaddata;
                $scope.getemp = promise.getemp;
                $scope.getinstitutiondetails = promise.getinstitutiondetails;

            });
        };

        $scope.OnChangeExpenditureIndent = function () {
            $scope.divdetails = false;
            $scope.submitted = false;
            $scope.getparticularsdetails = [];
            $scope.PCMPART_Id = "";
            $scope.indentdetails = 0;
            var data = {
                "MI_Id": $scope.MI_Id,
                "PCINDENTAP_Id": $scope.PCINDENTAP_Id
            };

            apiService.create("PC_Indent_Approval/OnChangeExpenditureIndent", data).then(function (promise) {
                $scope.getparticularsdetails = promise.getparticularsdetails;
                $scope.getemp = promise.getemp;
                $scope.getviewdata = promise.getviewdata;

            });
        };

        $scope.OnChangeExpenditureParticular = function () {
            $scope.indentdetails = 0;
            $scope.submitted = false;
            if ($scope.PCINDENTAP_Id !== undefined && $scope.PCINDENTAP_Id !== null && $scope.PCINDENTAP_Id !== "") {
                var data = {
                    "MI_Id": $scope.MI_Id,
                    "PCINDENTAP_Id": $scope.PCINDENTAP_Id,
                    "PCMPART_Id": $scope.PCMPART_Id
                };
                apiService.create("PC_Indent_Approval/OnChangeExpenditureParticular", data).then(function (promise) {
                    $scope.divdetails = true;
                    $scope.indentdetails = 1;

                    $scope.getparticularsindentdetails = promise.getparticularsindentdetails;
                    $scope.PCMPART_ParticularName = $scope.getparticularsindentdetails[0].pcmparT_ParticularName;
                    $scope.PCINDENTAPDT_RequestedAmount = $scope.getparticularsindentdetails[0].pcindentapdT_RequestedAmount;
                    $scope.PCINDENTAPDT_SanctionedAmt = $scope.getparticularsindentdetails[0].pcindentapdT_SanctionedAmt;
                    $scope.PCINDENTAPDT_AmountSpent = $scope.getparticularsindentdetails[0].pcindentapdT_AmountSpent;
                    $scope.PCINDENTAPDT_BalanceAmount = $scope.getparticularsindentdetails[0].pcindentapdT_BalanceAmount;

                    if ($scope.PCINDENTAPDT_BalanceAmount >= 0) {
                        $scope.balanceamt = "Balance Amount";
                    } else {
                        $scope.balanceamt = "Excess Amount";
                    }

                    $scope.getindentdetails = promise.getindentdetails;
                    $scope.PCINDENT_IndentNo = $scope.getindentdetails[0].pcindenT_IndentNo;
                    $scope.PCINDENTAPT_RequestedAmount = $scope.getindentdetails[0].pcindentapT_RequestedAmount;
                    $scope.PCINDENTAPT_SanctionedAmt = $scope.getindentdetails[0].pcindentapT_SanctionedAmt;
                    $scope.PCINDENTAPT_AmountSpent = $scope.getindentdetails[0].pcindentapT_AmountSpent;
                    $scope.PCINDENTAPT_BalanceAmount = $scope.getindentdetails[0].pcindentapT_BalanceAmount;

                    if ($scope.PCINDENTAPT_BalanceAmount >= 0) {
                        $scope.balanceamt1 = "Balance Amount";
                    } else {
                        $scope.balanceamt1 = "Excess Amount";
                    }
                });
            } else {
                $scope.divdetails = true;
            }
        };

        $scope.SaveExpenditure = function () {

            if ($scope.myForm.$valid) {

                $scope.PCEXPTR_Id = $scope.obj.PCEXPTR_Id !== undefined && $scope.obj.PCEXPTR_Id !== null && $scope.obj.PCEXPTR_Id !== "" ? $scope.obj.PCEXPTR_Id : 0;

                var PCINDENTAP_Id = $scope.PCINDENTAP_Id !== undefined && $scope.PCINDENTAP_Id !== null && $scope.PCINDENTAP_Id !== "" ? $scope.PCINDENTAP_Id : 0;

                $scope.PCEXPTR_ReferenceNo = $scope.PCEXPTR_ReferenceNo !== undefined && $scope.PCEXPTR_ReferenceNo !== null && $scope.PCEXPTR_ReferenceNo !== "" ? $scope.PCEXPTR_ReferenceNo : "";

                $scope.PCEXPTR_Desc = $scope.PCEXPTR_Desc !== undefined && $scope.PCEXPTR_Desc !== null && $scope.PCEXPTR_Desc !== "" ? $scope.PCEXPTR_Desc : "";

                var data = {
                    "MI_Id": $scope.MI_Id,
                    "PCEXPTR_Id": $scope.PCEXPTR_Id,
                    "HRME_Id": $scope.HRME_Id.hrmE_Id,
                    "PCINDENTAP_Id": PCINDENTAP_Id,
                    "PCMPART_Id": $scope.PCMPART_Id,
                    "PCEXPTR_Date": new Date($scope.PCEXPTR_Date).toDateString(),
                    "PCEXPTR_Amount": $scope.obj.PCEXPTR_Amount,
                    "PCEXPTR_ReferenceNo": $scope.obj.PCEXPTR_ReferenceNo,
                    "PCEXPTR_ModeOfPayment": $scope.obj.PCEXPTR_ModeOfPayment,
                    "PCEXPTR_Desc": $scope.obj.PCEXPTR_Desc
                };
                apiService.create("PC_Indent_Approval/SaveExpenditure", data).then(function (promise) {

                    if (promise.saveorupdate === "ReferenceNotGenerated") {
                        swal("Reference No. Not Generated");
                    } else if (promise.saveorupdate === "Add") {
                        if (promise.returnval === true) {
                            swal("Record Saved Successfully");
                        } else {
                            swal("Failed To Save Record");
                        }
                    } else if (promise.saveorupdate === "Update") {
                        if (promise.returnval === true) {
                            swal("Record Updated Successfully");
                        } else {
                            swal("Failed To Update Record");
                        }
                    } else {
                        swal("Failed To Save / Update Record");
                    }

                    $scope.getexpenditureloaddata = promise.getexpenditureloaddata;
                    
                    $scope.clearSaveData();
                    //$scope.clearData();
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.DeleteExpenditure = function (obj) {

            swal({
                title: "Delete Expenditure!",
                text: "Are You Sure You Want To Delete This Record",
                type: "input",
                showCancelButton: false,
                closeOnConfirm: false,
                inputPlaceholder: "Enter Remarks",
                confirmButtonText: "OK"
            }, function (inputValue) {
                if (inputValue === false) return false;
                if (inputValue === "") {
                    swal.showInputError("You need to write something!");
                    return false;
                }
                if (inputValue !== "") {                     
                    var data = {
                        "PCEXPTR_Id": obj.pcexptR_Id,
                        "MI_Id": $scope.MI_Id,
                        "PCEXPTR_DeletedRemarks": inputValue
                    };
                    apiService.create("PC_Indent_Approval/DeleteExpenditure", data).then(function (promise) {
                        if (promise !== null) {
                            if (promise.saveorupdate === "Error") {
                                swal("Failed To Delete Record");
                            } else {
                                if (promise.returnval === true) {
                                    swal("Record Deleted Successfully");
                                } else {
                                    swal("Failed To Delete Record");
                                }
                            }
                            $scope.clearData();
                        }
                    });
                }
            });
        };

        $scope.ViewPaymentVocher = function (obj) {

            $scope.pcexptRExpenditureNo = obj.pcexptR_ExpenditureNo;
            $scope.pcexptRParticularName = obj.pcmparT_ParticularName;
            $scope.employeenamedetails = obj.employeename;
            $scope.pcexptRDesc = obj.pcexptR_Desc;
            $scope.pcexptRAmount = obj.pcexptR_Amount;
            $scope.amountinwords = $scope.amountinwordss(Number($scope.pcexptRAmount));
            $scope.pcexptRModeOfPayment = obj.pcexptR_ModeOfPayment;
            $scope.pcexptRDate = new Date(obj.pcexptR_Date);
            $scope.companyname = $scope.getinstitutiondetails[0].mI_Name;
            $scope.imagenew = "";
            $scope.imagenew = $scope.getinstitutiondetails[0].mI_Logo;

            $('#mymodalviewpaymentvocher').modal('show');
        };

        $scope.EditExpenditure = function (obj) {

            var data = {
                "PCEXPTR_Id": obj.pcexptR_Id,
                "MI_Id": $scope.MI_Id
            };

            apiService.create("PC_Indent_Approval/EditExpenditure", data).then(function (promise) {

                if (promise !== null) {
                    $scope.EditExpenditureDetails = promise.editexpendituredetails;

                    $scope.obj.PCEXPTR_Amount = $scope.EditExpenditureDetails[0].pcexptR_Amount;
                    $scope.obj.PCEXPTR_ReferenceNo = $scope.EditExpenditureDetails[0].pcexptR_ReferenceNo;
                    $scope.obj.PCEXPTR_ModeOfPayment = $scope.EditExpenditureDetails[0].pcexptR_ModeOfPayment;
                    $scope.obj.PCEXPTR_Desc = $scope.EditExpenditureDetails[0].pcexptR_Desc;
                    $scope.obj.PCEXPTR_Id = $scope.EditExpenditureDetails[0].pcexptR_Id;

                    $scope.getparticularsdetails = promise.getparticularsdetails;

                    $scope.getparticularsindentdetails = promise.getparticularsindentdetails;
                    $scope.getindentdetails = promise.getindentdetails;


                    $scope.divdetails = true;
                    $scope.indentdetails = 1;

                    $scope.getparticularsindentdetails = promise.getparticularsindentdetails;
                    $scope.PCMPART_ParticularName = $scope.getparticularsindentdetails[0].pcmparT_ParticularName;
                    $scope.PCINDENTAPDT_RequestedAmount = $scope.getparticularsindentdetails[0].pcindentapdT_RequestedAmount;
                    $scope.PCINDENTAPDT_SanctionedAmt = $scope.getparticularsindentdetails[0].pcindentapdT_SanctionedAmt;
                    $scope.PCINDENTAPDT_AmountSpent = $scope.getparticularsindentdetails[0].pcindentapdT_AmountSpent;
                    $scope.PCINDENTAPDT_BalanceAmount = $scope.getparticularsindentdetails[0].pcindentapdT_BalanceAmount;

                    if ($scope.PCINDENTAPDT_BalanceAmount >= 0) {
                        $scope.balanceamt = "Balance Amount";
                    } else {
                        $scope.balanceamt = "Excess Amount";
                    }
                   
                    $scope.PCINDENT_IndentNo = $scope.getindentdetails[0].pcindenT_IndentNo;
                    $scope.PCINDENTAPT_RequestedAmount = $scope.getindentdetails[0].pcindentapT_RequestedAmount;
                    $scope.PCINDENTAPT_SanctionedAmt = $scope.getindentdetails[0].pcindentapT_SanctionedAmt;
                    $scope.PCINDENTAPT_AmountSpent = $scope.getindentdetails[0].pcindentapT_AmountSpent;
                    $scope.PCINDENTAPT_BalanceAmount = $scope.getindentdetails[0].pcindentapT_BalanceAmount;

                    if ($scope.PCINDENTAPT_BalanceAmount >= 0) {
                        $scope.balanceamt1 = "Balance Amount";
                    } else {
                        $scope.balanceamt1 = "Excess Amount";
                    }

                    $scope.PCINDENTAP_Id = promise.pcindentaP_Id;
                    $scope.PCMPART_Id = promise.pcmparT_Id;

                    $scope.geteditemp = promise.geteditemp;
                    $scope.HRME_Id = $scope.geteditemp[0];

                    $scope.editflag = true;
                }

            });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };      

        $scope.clearSaveData = function () {
            $scope.obj.PCEXPTR_Amount = "";
            $scope.obj.PCEXPTR_ReferenceNo = "";
            $scope.obj.PCEXPTR_ModeOfPayment = "";
            $scope.obj.PCEXPTR_Desc = "";
            $scope.divdetails = true;
            $scope.indentdetails = 1;
            $scope.PCEXPTR_Id = "";
            $scope.obj.PCEXPTR_Id = "";
        };

        $scope.clearData = function () {
            $scope.PCEXPTR_Date = new Date();
            $scope.MI_Id = "";
            $scope.HRME_Id = "";
            $scope.getuserinstitution = [];
            $scope.getemp = [];
            $scope.divdetails = false;
            $scope.submitted = false;
            $scope.getparticularsdetails = [];
            $scope.getindentapprovaldetails = [];
            $scope.getexpenditureloaddata = [];
            $scope.PCINDENTAP_Id = "";
            $scope.PCMPART_Id = "";
            $scope.indentdetails = 0;

            $scope.PCINDENT_IndentNo = "";
            $scope.PCINDENTAPT_RequestedAmount = "";
            $scope.PCINDENTAPT_SanctionedAmt = "";
            $scope.PCINDENTAPT_AmountSpent = "";
            $scope.PCINDENTAPT_BalanceAmount = "";

            $scope.PCMPART_ParticularName = "";
            $scope.PCINDENTAPDT_RequestedAmount = "";
            $scope.PCINDENTAPDT_SanctionedAmt = "";
            $scope.PCINDENTAPDT_AmountSpent = "";
            $scope.PCINDENTAPDT_BalanceAmount = "";

            $scope.obj.PCEXPTR_Amount = "";
            $scope.obj.PCEXPTR_ReferenceNo = "";
            $scope.obj.PCEXPTR_ModeOfPayment = "";
            $scope.obj.PCEXPTR_Desc = "";
            $scope.PCEXPTR_Id = "";
            $scope.obj.PCEXPTR_Id = "";

            $scope.ExpenditureLoaddata();
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
            var innerContents = document.getElementById("printexpenditure").innerHTML;
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

        $scope.amountinwordss = function convertNumberToWords(atotalc) {
            var words = new Array();
            words[0] = '';
            words[1] = 'One';
            words[2] = 'Two';
            words[3] = 'Three';
            words[4] = 'Four';
            words[5] = 'Five';
            words[6] = 'Six';
            words[7] = 'Seven';
            words[8] = 'Eight';
            words[9] = 'Nine';
            words[10] = 'Ten';
            words[11] = 'Eleven';
            words[12] = 'Twelve';
            words[13] = 'Thirteen';
            words[14] = 'Fourteen';
            words[15] = 'Fifteen';
            words[16] = 'Sixteen';
            words[17] = 'Seventeen';
            words[18] = 'Eighteen';
            words[19] = 'Nineteen';
            words[20] = 'Twenty';
            words[30] = 'Thirty';
            words[40] = 'Forty';
            words[50] = 'Fifty';
            words[60] = 'Sixty';
            words[70] = 'Seventy';
            words[80] = 'Eighty';
            words[90] = 'Ninety';
            atotalc = atotalc.toString();
            var atemp = atotalc.split(".");
            var number = atemp[0].split(",").join("");
            var n_length = number.length;
            var words_string = "";
            if (n_length <= 9) {
                var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
                var received_n_array = new Array();
                for (var i = 0; i < n_length; i++) {
                    received_n_array[i] = number.substr(i, 1);
                }
                for (var i = 9 - n_length, j = 0; i < 9; i++ , j++) {
                    n_array[i] = received_n_array[j];
                }
                for (var i = 0, j = 1; i < 9; i++ , j++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        if (n_array[i] == 1) {
                            n_array[j] = 10 + parseInt(n_array[j]);
                            n_array[i] = 0;
                        }
                    }
                }
                atotalc = "";
                for (var i = 0; i < 9; i++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        atotalc = n_array[i] * 10;
                    } else {
                        atotalc = n_array[i];
                    }
                    if (atotalc != 0) {
                        words_string += words[atotalc] + " ";
                    }
                    if ((i == 1 && atotalc != 0) || (i == 0 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Crores ";
                    }
                    if ((i == 3 && atotalc != 0) || (i == 2 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Lakhs ";
                    }
                    if ((i == 5 && atotalc != 0) || (i == 4 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Thousand ";
                    }
                    if (i == 6 && atotalc != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                        words_string += "Hundred and ";
                    } else if (i == 6 && atotalc != 0) {
                        words_string += "Hundred ";
                    }
                }
                words_string = words_string.split("  ").join(" ");
            }
            return words_string;
        }
    }
})();