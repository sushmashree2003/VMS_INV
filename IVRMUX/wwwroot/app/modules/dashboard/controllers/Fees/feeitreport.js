

(function () {
    'use strict';
    angular
.module('app')
.controller('feeitreportController', feeitreportController)

    feeitreportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache', 'blockUI']
    function feeitreportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache, blockUI) {

        $scope.indrpt = false;
        $scope.report = false;
        var institutionid, automanualreceiptnotranum
        $scope.htmldata = "";

        $scope.totalpayable = 0;
        $scope.totalconcession = 0;
        $scope.totalpaid = 0;

        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        // $scope.Mi_Id = configsettings[0].mI_Id;
        institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        var headtot = 0;
        var contot = 0;
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        var transactionnumbering = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transactionnumbering.length; i++) {
            if (transactionnumbering.length > 0) {
                if (transactionnumbering[i].imN_Flag == "Online") {
                    $scope.transnumbconfigurationsettingsassign = transactionnumbering[i];
                }
            }
        }

        if (configsettings.length > 0) {
            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
            receiptformat = configsettings[0].fmC_Receipt_Format;
            mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
        }
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.obj = {};
        $scope.print_flag = false;
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
           
            apiService.getURI("feeitreport/getalldetails", pageid).
                then(function (promise) {
                    $scope.onclickloaddataclass();
            $scope.yearlist = promise.adcyear;
            $scope.classlist = promise.fillclass;

            $scope.fillmasterhead = promise.fillmasterhead;
        })
        }
        
        //adding section 
        $scope.onselectclass = function (clsobj) {

            var data = {
                "ASMAY_Id": clsobj.asmaY_Id,
                "ASMCL_Id": clsobj.asmcL_Id,
            }


            apiService.create("feeitreport/getsection", data).
               then(function (promise) {
                   $scope.sectioncount = promise.fillsection;
                   //  $scope.arrlstinst1 = promise.fillinstallment;
               })
        }
      

        $scope.onclickloaddataclass = function () {
            if ($scope.Selectionrd == "Indi") {
                $scope.rcpt = true;
                $scope.feehd = false;
                $scope.obj.asmaY_Id = "";
                $scope.obj.asmcL_Id = "";
                $scope.obj.asmC_Id = "";
                $scope.obj.amst_Id = "";
                $scope.obj.fyp_id = "";

                if ($scope.obj.asmaY_Id == "") {
                    $scope.Grid_view = true;
                }
                else {
                    $scope.Grid_view = false;
                }
                
            }
            else if ($scope.Selectionrd == "allr") {
                $scope.rcpt = false;
                $scope.feehd = true;
                $scope.obj.asmaY_Id = "";
                $scope.obj.asmcL_Id = "";
                $scope.obj.asmC_Id = "";
                $scope.obj.amst_Id = "";
                $scope.obj.fyp_id = "";

                if ($scope.obj.asmaY_Id == "") {
                    $scope.Grid_view = true;
                }
                else {
                    $scope.Grid_view = false;
                }

            }
        };




        $scope.onselectsection = function (secobj) {

            var data = {
                "ASMAY_Id": secobj.asmaY_Id,
                "ASMCL_Id": secobj.asmcL_Id,
                "AMSC_Id": secobj.asmC_Id,
            }
           

            debugger;
            apiService.create("feeitreport/getstudent", data).
               then(function (promise) {
                   $scope.studentlst = promise.fillstudent;
               })
        }



        $scope.onselectstudent = function (std) {

            var data = {
                "ASMAY_Id": std.asmaY_Id,
                "ASMCL_Id": std.asmcL_Id,
                "AMSC_Id": std.asmC_Id,
                "AMST_Id": std.amst_Id,

            }


            apiService.create("feeitreport/getreceipt", data).
               then(function (promise) {
                   $scope.receiptno = promise.fillterms;
               })
        }



        //$scope.adyr = true;
        $scope.Clearid = function () {
            $state.reload();
            $scope.loaddata();
        }
 


        $scope.is_optionrequired_groupterm_grp = function () {

            // if ($scope.group_check==true) {
            return !$scope.fillmasterhead.some(function (options) {
                return options.fmH_Id_chk;
            });
            // }

        }


     


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.showmodaldetail = function (obj) {

            $scope.totalpayable = 0;
            $scope.totalconcession = 0;
            $scope.totalpaid = 0;

            if ($scope.myForm.$valid) {

                $scope.ASMCL_ClassName = "";
                $scope.ASMC_SectionName = "";
                $scope.AMAY_RollNo = "";
                $scope.AMST_AdmNo = "";
                $scope.AMST_FirstName = "";

                $scope.curdate = "";
                $scope.asmaY_Year = "";
                $scope.AMST_FatherName = "";
                $scope.AMST_MotherName = "";
                $scope.FMCC_ConcessionName = "";

                var FMH_ID="0";
                $scope.albumNameArray = [];
                if ($scope.Selectionrd == "allr") {
                    angular.forEach($scope.fillmasterhead, function (ty) {
                        if (ty.fmH_Id_chk) {
                            FMH_ID = FMH_ID + ',' + ty.fmH_Id;
                        }
                    })
                }
                if (obj.fyp_id == "") {
                    obj.fyp_id = "0";
                }

                var data = {

                    "ASMAY_Id": obj.asmaY_Id,
                    "ASMCL_Id": obj.asmcL_Id,
                    "AMSC_Id": obj.asmC_Id,
                    "AMST_Id": obj.amst_Id,
                    "FYP_Id": obj.fyp_id,
                    "minstall": mergeinstallment,
                    "returnval": $scope.Selectionrd,
                    "multiplegroups": FMH_ID
                }

                apiService.create("feeitreport/printreceipt", data).
           then(function (promise) {

               if (promise.fillstudentviewdetails.length > 0) {

                   $scope.print_flag = true;

                   $scope.fillstudentdetails = promise.fillstudentviewdetails;
                   $scope.specialfeehead = promise.receiptformathead;

                       $scope.Paid_Date = $filter('date')(new Date(), "dd-MM-yyyy");
                       $scope.AMST_AdmNo = promise.fillstudentviewdetails[0].AMST_AdmNo;
                       $scope.asmaY_Year = promise.fillstudentviewdetails[0].ASMAY_Year;
                       $scope.AMST_FirstName = promise.fillstudentviewdetails[0].amsT_FirstName;

                       $scope.ASMCL_ClassName = promise.fillstudentviewdetails[0].classname;
                       $scope.ASMC_SectionName = promise.fillstudentviewdetails[0].ASMC_SectionName;

                       $scope.AMST_FatherName = promise.fillstudentviewdetails[0].fathername;
                       $scope.FMCC_ConcessionName = promise.fillstudentviewdetails[0].fmcC_ConcessionName;

                       $scope.AMST_MotherName = promise.fillstudentviewdetails[0].mothername;

                       $scope.period = promise.duration;

                       $scope.showdetailsreceipt = promise.fillstudentviewdetails;

                       angular.forEach(promise.fillstudentviewdetails, function (e) {
                           $scope.totalpayable += e.fmA_Amount;
                           $scope.totalconcession += e.ftP_Concession_Amt;
                           $scope.totalpaid += e.ftP_Paid_Amt;
                       });

                       $scope.totalpayable = Number($scope.totalpayable);
                       $scope.totalconcession = Number($scope.totalconcession);
                       $scope.totalpaid = Number($scope.totalpaid);

                       $scope.words = $scope.amountinwords(Number($scope.totalpaid));

                   }
           })

            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.ctotalC = function (int) {
            var total = 0;
            angular.forEach($scope.showdetailsreceipt, function (e) {
                total += e.totalcharges;
            });
            return total;
        };

        $scope.printToCart = function () {
          
            var pdss = "";
              
                pdss = 'printrcp'

            var innerContents = document.getElementById(pdss).innerHTML;
            var popupWinindow = window.open('_blank', 'padding-top=50%;');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
 '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
 '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BArrearReport/itreportpdf.css" />' +
 '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.amountinwords = function convertNumberToWords(atotalc) {
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
                for (var i = 9 - n_length, j = 0; i < 9; i++, j++) {
                    n_array[i] = received_n_array[j];
                }
                for (var i = 0, j = 1; i < 9; i++, j++) {
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

