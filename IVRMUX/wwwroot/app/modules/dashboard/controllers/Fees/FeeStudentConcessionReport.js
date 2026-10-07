

(function () {
    'use strict';
    angular
.module('app')
.controller('FeeStudentConcessionReportController', FeeStudentConcessionReport123)

    FeeStudentConcessionReport123.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function FeeStudentConcessionReport123($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {
        $scope.IsHiddenup = true;
        $scope.Selectionrd = "allr";

        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');


        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        $scope.searchString = "";
        $scope.file_disable = true;

        $scope.get_total_student_print_con = function () {
            var totA_p = 0;
            var totB_p = 0;
            var totC_p = 0;
            angular.forEach($scope.printdatatable, function (gp) {
                totA_p += gp.Netamount;
                totB_p += gp.Concession;
                totC_p += gp.Paid;
            })
            $scope.totA_p = totA_p;
            $scope.totB_p = totB_p;
            $scope.totC_p = totC_p;
        }



        $scope.get_total_student_print = function () {
            var totA_p = 0;
            var totB_p = 0;
            angular.forEach($scope.printdatatable, function (gp) {
                totA_p += gp.Netamount;
                totB_p += gp.Concession;
            })
            $scope.totA_p = totA_p;
            $scope.totB_p = totB_p;

        }







        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (configsettings.length > 0) {
            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
            receiptformat = configsettings[0].fmC_Receipt_Format;
            mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
        }
      
        if (grouporterm == "T") {
            $scope.grouportername = "Term Name"
            $scope.term = true;
            $scope.groupterm = false;
        }
        else if (grouporterm == "G") {
            $scope.grouportername = "Group Name"
            $scope.groupterm = true;
            $scope.term = false;
        }


        var temp_grp_list = [];
        $scope.loaddata = function () {
            var pageid = 2;
            $scope.rndindCLSS = false;
            $scope.custom_check_flag = true;
            $scope.group_check_flag = true;
            $scope.custom_check = "0";
            $scope.group_check = "0";
            $scope.load_group_check();
            $scope.load_custom_check();
            var data = {
                "reporttype": grouporterm,
            }
            apiService.create("FeeStudentConcessionReport/getalldetails123", data).
        then(function (promise) {
            debugger;
            $scope.acayyearbind = promise.acayear;
            //$scope.arrlistchk = promise.fgrp;
            //$scope.groupcount = promise.fgrp;
            $scope.clsdrpdown = promise.classlist;
            $scope.sectiondrpre = promise.sectionlist;
            $scope.concategory = promise.concategory;
            //$scope.individual_drpdis1();
            //$scope.export_flag = false;
            //$scope.print_flag = false;
            $scope.onclickloaddata();
            $scope.groupcount = promise.fillmastergroup;
            if (grouporterm == "T") {
                angular.forEach(promise.customlist, function (tr) {
                    tr.fmgG_Id_chk = true;
                })
            }
            else if (grouporterm == "G") {
                angular.forEach(promise.customlist, function (tr) {
                    tr.fmgG_Id_chk1 = true;
                })
            }

            $scope.custom = promise.customlist;

            if (grouporterm == "T") {
                angular.forEach(promise.grouplist, function (tr) {
                    tr.fmG_Id_chk = true;
                })
            }
            else if (grouporterm == "G") {
                angular.forEach(promise.grouplist, function (tr) {
                    tr.fmG_Id_chk1 = true;
                })
            }


            $scope.group = promise.grouplist;
            temp_grp_list = promise.grouplist;
          
                        
        })
        }


        $scope.load_custom_check = function () {

            if ($scope.custom_check == "1") {
                $scope.custom_check_flag = false;

            }
            else if ($scope.custom_check == "0") {
                $scope.custom_check_flag = true;

            }
        }
        $scope.load_group_check = function () {

            if ($scope.group_check == "1") {
                $scope.group_check_flag = false;
            }
            else if ($scope.group_check == "0") {
                $scope.group_check_flag = true;

            }
        }


        $scope.onclickloaddata = function () {
            if ($scope.Selectionrd == "allr") {
                $scope.rndindACC = true;
                $scope.rndindSECC = false;
                $scope.rndindCLSS = false;
                $scope.rndindStff = false;
                $scope.rndindGeneral = false;
                $scope.rndindSpecialFee = false;
                $scope.rndindTotal = true;
                $scope.rndindCDAdj = false;
                $scope.individual_drdd = true;
                $scope.individual_cat = false;
                $scope.fee_group_valid = true;
                $scope.Grid_view = false;
                $scope.export_flag = false;
                $scope.print_flag = false;
                $scope.submitted = false;
                $scope.load_group_check();
                $scope.load_custom_check();

               
            }
            else if ($scope.Selectionrd == "Indi") {
                $scope.rndindACC = true;
                $scope.rndindSECC = true;
                $scope.rndindCLSS = true;
                $scope.rndindStff = false;
                $scope.rndindGeneral = false;
                $scope.rndindSpecialFee = true;
                $scope.rndindTotal = true;
                $scope.rndindCDAdj = true;
                $scope.individual_drdd = true;
                $scope.individual_cat = false;
                $scope.fee_group_valid = true;
                $scope.Grid_view = false;
                $scope.export_flag = false;
                $scope.print_flag = false;
                $scope.clsdrp = "";
                $scope.submitted = false;
                $scope.load_group_check();
                $scope.load_custom_check();
            }
          
        };
       // $scope.fee_group = true;
        $scope.individual_drdd = false;
        $scope.fee_group_valid = false;
        $scope.fmG_Individual_Flag = false;
        $scope.individual_drpdis = function () {
            debugger;
            if ($scope.individual == "1") {
                $scope.fmG_Individual_Flag = true;
                $scope.fee_group_valid = false;

            }
            else if ($scope.individual == "0") {
                $scope.fmG_Individual_Flag = false;
                $scope.fee_group_valid = true;
            }
        }
        $scope.individual1 = "0";
        $scope.individual_drpdis1 = function () {
            debugger;
            if ($scope.individual1 == "1") {
                angular.forEach($scope.concategory, function (role) {
                    role.selected = true;
                        })
                $scope.fmcC_Individual_Flag = true;
            //    $scope.binddatagrp();
            }
            else if ($scope.individual1 == "0") {
                angular.forEach($scope.concategory, function (role) {
                    role.selected = false;
                })
                $scope.fmcC_Individual_Flag = false;
             //   $scope.binddatagrp();
            }
        }

        $scope.ShowHideup = function () {

            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted1 = function (field) {

            return $scope.submitted1;
        };
        $scope.total1 = function (e) {
            var totalc = 0;
            angular.forEach($scope.students, function (e) {
                totalc += e.Netamount;
            });
            return totalc;
        };
        $scope.total2 = function (e) {
            var totalc = 0;
            angular.forEach($scope.students, function (e) {
                totalc += e.Concession;
            });
            return totalc;
        };

        $scope.total3 = function (e) {
            var totalc = 0;
            angular.forEach($scope.students, function (e) {
                totalc += e.Netamount;
            });
            return totalc;
        };
        $scope.total4 = function (e) {
            var totalc = 0;
            angular.forEach($scope.students, function (e) {
                totalc += e.Concession;
            });
            return totalc;
        };

        $scope.total5 = function (e) {
            var totalc = 0;
            angular.forEach($scope.students, function (e) {
                totalc += e.Paid;
            });
            return totalc;
        };

        $scope.clear_stucon = function () {  
            $state.reload();
            $scope.loaddata();
        }
        $scope.individual = "0";
        $scope.individual1 = "0";
        $scope.Grid_view = false;
        $scope.print_flag = true;
        $scope.submitted = false;

        $scope.ShowReportdata = function () {
            // if($scope.all)
         
            if ($scope.myForm.$valid) {
                debugger;



                if ($scope.reporttype == 'Student') {
                    $scope.clsdrp =  $scope.clsdrp,
                    $scope.sectiondrp = $scope.sectiondrp
                }
                else
                {
                    $scope.clsdrp = 0,
                    $scope.sectiondrp =0
                }




                $scope.totA_p = '';
                $scope.totB_p = '';
                $scope.totC_p = '';
                $scope.printdatatable = [];
                $scope.all = false;
                $scope.ind = false;
                $scope.cons = false;
                $scope.albumNameArraycolumn1 = [];
                angular.forEach($scope.custom, function (custom1) {
                    if (!!custom1.selected) $scope.albumNameArraycolumn1.push({
                        columnID1: custom1.fmgG_Id

                    });
                })

                $scope.albumNameArraycolumn2 = [];
                angular.forEach($scope.group, function (group) {
                    if (!!group.selected) $scope.albumNameArraycolumn2.push({
                        columnID2: group.fmG_Id

                    });
                })

                $scope.albumNameArraycolumn3 = [];
                angular.forEach($scope.groupcount, function (groupcount) {
                    if (!!groupcount.selected) $scope.albumNameArraycolumn3.push({
                        columnID3: groupcount.fmT_Id

                    });
                })



                var FMG_Ids = [];
                var FMT_Ids = [];
                if (grouporterm == "T") {
                    angular.forEach($scope.group, function (ty) {
                        if (ty.fmG_Id_chk) {
                            FMG_Ids.push(ty.fmG_Id);
                        }
                    })
                    angular.forEach($scope.groupcount, function (ty) {
                        if (ty.fmT_Id_chk) {
                            FMT_Ids.push(ty.fmT_Id);
                        }
                    })
                }
                else if (grouporterm == "G") {
                    angular.forEach($scope.group, function (ty) {
                        if (ty.fmG_Id_chk1) {
                            FMG_Ids.push(ty.fmG_Id);
                        }
                    })
                }
                    //var data = {

                    //    "typeofrpt": $scope.Selectionrd,
                    //    "asmyid": $scope.academicyr,
                    //    "clsid": $scope.clsdrp,
                    //    "setid": $scope.sectiondrp,
                    //    "FMG_Id": $scope.fmG_Id,
                    //    "Type": $scope.individual1,
                    //    fmcC_ConcessionName: $scope.albumNameArray1,    // Fee concession array list.
                    //}

                if (($scope.Selectionrd == "allr") || ($scope.Selectionrd == "con")){

                    var data = {
                        "typeofrpt": $scope.Selectionrd,
                        "asmyid": $scope.academicyr,
                        //"clsid": $scope.clsdrp,
                        //"setid": $scope.sectiondrp,
                        "FMG_Id": $scope.fmG_Id,
                        "FMGG_Id": $scope.fmgG_Id,
                        "FMT_Id": $scope.fmT_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "radioval": $scope.result,
                        "reporttype": $scope.rpttyp,
                        "studenttype": $scope.status,
                        "customflag": $scope.custom_check,
                        "groupflag": $scope.group_check,
                        FMG_Ids: FMG_Ids,
                        FMT_Ids: FMT_Ids,
                        "term_group": grouporterm,
                        "FMCC_Id": $scope.fmcC_Id,
                        "report": $scope.reporttype
                    }

                }
                else if ($scope.Selectionrd == "Indi") {

                    var data = {
                        "typeofrpt": $scope.Selectionrd,
                        "asmyid": $scope.academicyr,
                        "clsid": $scope.clsdrp,
                        "setid": $scope.sectiondrp,
                        "FMG_Id": $scope.fmG_Id,
                        "FMGG_Id": $scope.fmgG_Id,
                        "FMT_Id": $scope.fmT_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "radioval": $scope.result,
                        "reporttype": $scope.rpttyp,
                        "studenttype": $scope.status,
                        "customflag": $scope.custom_check,
                        "groupflag": $scope.group_check,
                        FMG_Ids: FMG_Ids,
                        FMT_Ids: FMT_Ids,
                        "term_group": grouporterm,
                        "FMCC_Id": $scope.fmcC_Id,
                        "report": $scope.reporttype
                    }

                }
                
                apiService.create("FeeStudentConcessionReport/getreport", data).
            then(function (promise) {
                debugger;
               
                if ($scope.Selectionrd == "allr") {
                    if (promise.reportdatelist != null && promise.reportdatelist !="") {
                        $scope.Grid_view = true;
                        $scope.export_flag = true;
                        $scope.print_flag = true;
                    $scope.students123 = [];
                    $scope.students = "";
                    $scope.totcountfirst = promise.reportdatelist.length;
                    $scope.students = promise.reportdatelist;
                    $scope.totA = $scope.total1(promise.reportdatelist);
                    $scope.totB = $scope.total2(promise.reportdatelist);
                    $scope.rndindALLGrid = true;
                    $scope.file_disable = true;
                        $scope.rndindconsol = false;
                     $scope.rndindIndivisual = false;
                     $scope.IsHiddendown = true;
                   
                    }
                    else {
                        swal("No Record Found");
                        $scope.Grid_view = false;
                        $scope.export_flag = false;
                        $scope.print_flag = false;
                        $scope.file_disable = true;
                    }
                }
               
                else if ($scope.Selectionrd == "Indi") {
                    debugger;
                    if (promise.reportdatelist != null && promise.reportdatelist !="") {
                        $scope.Grid_view = true;
                        $scope.export_flag = true;
                        $scope.print_flag = true;
                    $scope.students = "";
                    $scope.students123 = "";
                    $scope.students = promise.reportdatelist;
                    $scope.totcountfirst = promise.reportdatelist.length;
                    $scope.totA = $scope.total3(promise.reportdatelist);
                    $scope.totB = $scope.total4(promise.reportdatelist);
                     $scope.rndindALLGrid = false;
                        $scope.rndindconsol = false;
                    $scope.rndindIndivisual = true;
                          $scope.IsHiddendown = true;
                        //$scope.file_disable = false;
                        
                    }
                    else {
                        swal("No Record Found");
                        $scope.Grid_view = false;
                        $scope.export_flag = false;
                        $scope.print_flag = false;
                        $scope.file_disable = true;
                    }
                }



                else if ($scope.Selectionrd == "con") {
                    debugger;
                    if (promise.reportdatelist != null && promise.reportdatelist != "") {
                        $scope.Grid_view = true;
                        $scope.export_flag = true;
                        $scope.print_flag = true;
                        $scope.students = "";
                        $scope.students123 = "";
                        $scope.students = promise.reportdatelist;
                        $scope.totcountfirst = promise.reportdatelist.length;
                        $scope.totA = $scope.total3(promise.reportdatelist);
                        $scope.totB = $scope.total4(promise.reportdatelist);
                        $scope.totc = $scope.total5(promise.reportdatelist);
                        $scope.rndindALLGrid = false;
                        $scope.rndindconsol = true;
                        $scope.rndindIndivisual = false;
                        $scope.IsHiddendown = true;
                        //$scope.file_disable = false;

                    }
                    else {
                        swal("No Record Found");
                        $scope.Grid_view = false;
                        $scope.export_flag = false;
                        $scope.print_flag = false;
                        $scope.file_disable = true;
                    }
                }









            })
            }
            else {
                $scope.submitted = true;

            }
        }
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }


        $scope.get_groups = function () {
            debugger;
            var FMGG_Ids = [];
            if (grouporterm == "T") {
                angular.forEach($scope.custom, function (ty) {
                    if (ty.fmgG_Id_chk) {
                        FMGG_Ids.push(ty.fmgG_Id);
                    }
                })
            }
            else if (grouporterm == "G") {
                angular.forEach($scope.custom, function (ty) {
                    if (ty.fmgG_Id_chk1) {
                        FMGG_Ids.push(ty.fmgG_Id);
                    }
                })
            }

            if (FMGG_Ids.length > 0) {
                var data = {
                    "reporttype": grouporterm,
                    FMGG_Ids: FMGG_Ids
                }



                apiService.create("FeeStudentConcessionReport/get_groups", data).
                 then(function (promise) {


                     if (grouporterm == "T") {
                         angular.forEach(promise.grouplist, function (tr) {
                             tr.fmG_Id_chk = true;
                         })
                     }
                     else if (grouporterm == "G") {
                         angular.forEach(promise.grouplist, function (tr) {
                             tr.fmG_Id_chk1 = true;
                         })
                     }
                     $scope.group = promise.grouplist;
                 });
            }
            else if (FMGG_Ids.length == 0) {
                //$scope.group = temp_grp_list;
                $scope.group = [];
            }


        }


        $scope.is_optionrequired_trm_cg = function () {

            return !$scope.custom.some(function (options) {
                return options.fmgG_Id_chk;
            });
        }
        $scope.is_optionrequired_trm_grp = function () {

            return !$scope.group.some(function (options) {
                return options.fmG_Id_chk;
            });
        }
        $scope.is_optionrequired_trm_trm = function () {

            return !$scope.groupcount.some(function (options) {
                return options.fmT_Id_chk;
            });
        }
        $scope.is_optionrequired_groupterm_cg = function () {

            //if ($scope.custom_check == true) {
            return !$scope.custom.some(function (options) {
                return options.fmgG_Id_chk1;
            });
            // }

        }
        $scope.is_optionrequired_groupterm_grp = function () {

            // if ($scope.group_check==true) {
            return !$scope.group.some(function (options) {
                return options.fmG_Id_chk1;
            });
            // }

        }




        $scope.clear_feeconss= function () {
            debugger;
            $state.reload();
            

        }

        $scope.printdatatable = [];
        $scope.exportToExcel = function (table1) {
            debugger;
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                if ($scope.reporttype == 'Student') {

                    if (($scope.Selectionrd == "allr") || ($scope.Selectionrd == "Indi")) {
                        var exportHref = Excel.tableToExcel(table1, 'WireWorkbenchDataExport');
                    }
                    else {
                        var exportHref = Excel.tableToExcel(table3, 'WireWorkbenchDataExport');
                    }



                }
                else {

                    var exportHref = Excel.tableToExcel(table2, 'WireWorkbenchDataExport');
                }
                
               

                $timeout(function () { location.href = exportHref; }, 100);
                $timeout(function () {
                    location.href = exportHref;
                }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }
           // $state.reload();

        }


      
        $scope.printData = function (printSectionId) {
            debugger;
            var pdss = "";
            if ($scope.reporttype == 'Student') {

                if (($scope.Selectionrd == "allr") || ($scope.Selectionrd == "Indi")) {
                    pdss = 'printSectionId'
                }
                else {
                    pdss = 'printSectioncon'
                }
            }
         
            else {
             
                pdss = 'printSectionIdstf'
            }
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
               
                var innerContents = document.getElementById(pdss).innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                 '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
               // $state.reload();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }
  


        $scope.toggleAll = function () {
            debugger;
            $scope.printdatatable = [];
            var toggleStatus = $scope.all;
            angular.forEach($scope.students1, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
            $scope.get_total_student_print();

        }

       
      
        $scope.optionToggled = function (SelectedStudentRecord, index) {
            debugger;
            $scope.all = $scope.students.every(function (itm)
            { return itm.selected; });

            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatable.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_student_print();
        }



        



        $scope.toggleAllind = function () {
            debugger;
            $scope.printdatatable = [];
            var toggleStatus = $scope.ind;
            angular.forEach($scope.students1, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.ind == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
            $scope.get_total_student_print();

        }



        $scope.toggleAllcon = function () {
            debugger;
            $scope.printdatatable = [];
            var toggleStatus = $scope.cons;
            angular.forEach($scope.students1, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.cons == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
            $scope.get_total_student_print_con();

        }


        $scope.optionToggind = function (SelectedStudentRecord, index) {
            debugger;
            $scope.ind = $scope.students.every(function (itm)
            { return itm.selected; });
           // $scope.printdatatable = [];
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatable.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_student_print();
        }

        
       
        $scope.optionToggcon = function (SelectedStudentRecord, index) {
            debugger;
            $scope.ind = $scope.students.every(function (itm) { return itm.selected; });
            // $scope.printdatatable = [];
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatable.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_student_print_con();
        }


    

        $scope.ShowHideup = function () {

            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        }



        $scope.ShowHidedown = function () {


            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        }

    }
})();

