(function () {
    'use strict';
    angular
.module('app')
.controller('FeeDetailsReportController', FeeDetailsReportController)
    FeeDetailsReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function FeeDetailsReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {
        $scope.route = false;
        $scope.print = false;

        $scope.search = "";
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var copty;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }


        $scope.filterValue = function (obj) {
            
            return (angular.lowercase(obj.fsS_OBArrearAmount)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.FSS_RefundAmount)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.FSS_AdjustedAmount)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.FSS_OBExcessAmount)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (JSON.stringify(obj.FSS_WaivedAmount)).indexOf($scope.searchValue) >= 0 ||
                ($filter('date')(obj.trmV_PurchaseDate, 'dd/MM/yyyy').indexOf($scope.searchValue) >= 0)
        }



        $scope.imgname = logopath;
        $scope.loaddata = function () {
            $scope.grid_view = false;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            var pageid = 1;
            $scope.reporttype = false;
            $scope.wise = true;



            $scope.get_total_class_print = function () {
                var totA_p = 0;
                var totB_p = 0;
                var totC_p = 0;
                var totD_p = 0;
                var totE_p = 0;
                angular.forEach($scope.printdatatable, function (gp) {
                    totA_p += gp.FSS_OBArrearAmount;
                    totB_p += gp.FSS_RefundAmount;
                    totC_p += gp.FSS_AdjustedAmount;
                    totD_p += gp.FSS_OBExcessAmount;
                    totE_p += gp.FSS_WaivedAmount;
                })
                $scope.totA_p = totA_p;
                $scope.totB_p = totB_p;
                $scope.totC_p = totC_p;
                $scope.totD_p = totD_p;
                $scope.totE_p = totE_p;
            }

            $scope.get_total_waived = function () {
                var totA_w = 0;
                var totB_w = 0;
                var totC_w = 0;
                var totD_w = 0;
                angular.forEach($scope.printdatatable, function (gp) {
                    totA_w += gp.FSS_WaivedAmount;
                    totB_w += gp.FSS_TotalToBePaid;
                    totC_w += gp.FSS_PaidAmount;
                    totD_w += gp.FSS_ToBePaid;
                  
                })
                $scope.totA_w = totA_w;
                $scope.totB_w = totB_w;
                $scope.totC_w = totC_w;
                $scope.totD_w = totD_w;
               
            }



            $scope.get_total_OB = function () {
                var totA_w = 0;
                var totB_w = 0;
                var totC_w = 0;
                var totD_w = 0;
                var totE_w = 0;
                angular.forEach($scope.printdatatable, function (gp) {
                    totA_w += gp.FMOB_Student_Due;
                    totB_w += gp.FMOB_Institution_Due;
                    totC_w += gp.FSS_TotalToBePaid;
                    totD_w += gp.FSS_PaidAmount;
                    totE_w += gp.FSS_ToBePaid;
                })
                $scope.totA_w = totA_w;
                $scope.totB_w = totB_w;
                $scope.totC_w = totC_w;
                $scope.totD_w = totD_w;
                $scope.totE_w = totE_w;
            }
            

            $scope.get_total_adjstd = function () {
                var totA_w = 0;
                angular.forEach($scope.printdatatable, function (gp) {
                    totA_w += gp.adjamt;
                })
                $scope.totA_w = totA_w;
               
            }

            apiService.getURI("FeeDetailsReport/getalldetails", pageid).
            then(function (promise) {

                $scope.arrlist6 = promise.adcyear;
                $scope.groupcount = promise.fillterms;
                $scope.classcount = promise.fillclass;
                //$scope.sectioncount = promise.fillsection;
                $scope.headcount = promise.fillmasterhead;
                $scope.installcount = promise.fillinstallment;

            })
        }

        $scope.route_id = function () {
            
            if ($scope.route == "1") {
                $scope.trmR_Id = true;
            }
            else if ($scope.route == "0") {
                $scope.trmR_Id = false;

            }
        }



        $scope.allorconsorall = function () {

            if ($scope.allorindiorcon == "all") {
                
                $scope.reporttype = false;
                $scope.wise = true;

            }
            else if ($scope.allorindiorcon == "indi") {
              
                $scope.reporttype = true;
                $scope.wise = false;
            }
           
        }





        $scope.onselectyear = function () {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            
            apiService.create("FeeDetailsReport/getheadisbygrpid", data).
               then(function (promise) {
                   $scope.headlist = promise.fillfeehead;

                   //  $scope.arrlstinst1 = promise.fillinstallment;

               })
        }

        $scope.onselectclass = function () {
            
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            
            apiService.create("FeeDetailsReport/getsection", data).
               then(function (promise) {
                   $scope.sectioncount = promise.fillsection;
               })
        }


        $scope.onclickloaddata = function () {

            if ($scope.ctg == true) {

                $scope.checked = true;

                $scope.frmdt = true;



            }
            else if ($scope.ctg == false) {
                $scope.checked = false;

                $scope.frmdt = false;
            }

        }



        $scope.toggleAllstd = function () {
            
            $scope.printdatatable = [];
            var toggleStatus = $scope.stdall;
            angular.forEach($scope.students, function (itm) {
                itm.stdselected = toggleStatus;
                if (itm.stdselected == true) {
                    $scope.printdatatable.push(itm);
                }
            });

            $scope.get_total_class_print();
        }

        $scope.optionToggledstd = function () {
            
            $scope.printdatatable = [];
            angular.forEach($scope.students, function (itm) {
                if (itm.stdselected == true) {
                    $scope.printdatatable.push(itm);
                }
            });
            $scope.get_total_class_print();

        }
        $scope.toggleAllwaive = function () {
            
            $scope.printdatatable = [];
            var toggleStatus = $scope.waive;
            angular.forEach($scope.waived, function (itm) {
                itm.waiveselected = toggleStatus;
                if (itm.waiveselected == true) {
                    $scope.printdatatable.push(itm);
                }
            });

            $scope.get_total_waived();
        }

        $scope.optionToggledwaive = function () {
            
            $scope.printdatatable = [];
            angular.forEach($scope.waived, function (itm) {
                if (itm.waiveselected == true) {
                    $scope.printdatatable.push(itm);
                }
            });
            $scope.get_total_waived();

        }


        $scope.toggleAllgrp = function () {
            
            $scope.printdatatable = [];
            var toggleStatus = $scope.grpall;
            angular.forEach($scope.groups, function (itm) {
                if (itm.grpselected == true) {
                    $scope.printdatatable.push(itm);
                }            });
            $scope.get_total_class_print();

        }

        $scope.optionToggledgrp = function () {
            
            $scope.printdatatable = [];
            angular.forEach($scope.groups, function (itm) {
                if (itm.grpselected == true) {
                    $scope.printdatatable.push(itm);
                }
            });
            $scope.get_total_class_print();

        }

        $scope.toggleAllhad = function () {
            
            $scope.printdatatable = [];
            var toggleStatus = $scope.hadall;
            angular.forEach($scope.heads, function (itm) {
                itm.hadselected = toggleStatus;
                if (itm.hadselected == true) {
                    $scope.printdatatable.push(itm);
                }

            });

            $scope.get_total_class_print();
        }
        $scope.optionToggledhad = function () {
            
            $scope.printdatatable = [];
            angular.forEach($scope.heads, function (itm) {
                if (itm.hadselected == true) {
                    $scope.printdatatable.push(itm);
                }
            });
            $scope.get_total_class_print();

        }



        $scope.toggleAllcls = function () {
            
            $scope.printdatatable = [];
            var toggleStatus = $scope.clsall;
            angular.forEach($scope.classes, function (itm) {
                itm.clsselected = toggleStatus;
                if (itm.clsselected == true) {
                    $scope.printdatatable.push(itm);
                }

            });

            $scope.get_total_class_print();
        }
        $scope.optionToggledcls = function () {
            
            $scope.printdatatable = [];
            angular.forEach($scope.classes, function (itm) {
                if (itm.clsselected == true) {
                    $scope.printdatatable.push(itm);
                }
            });
            $scope.get_total_class_print();

        }




        $scope.toggleAlladj = function () {

            $scope.printdatatable = [];
            var toggleStatus = $scope.adj;
            angular.forEach($scope.admsudentslist, function (itm) {
                itm.adjselected = toggleStatus;
                if (itm.adjselected == true) {
                    $scope.printdatatable.push(itm);
                }
            });

            $scope.get_total_adjstd();
        }

        $scope.optionToggledadj = function () {

            $scope.printdatatable = [];
            angular.forEach($scope.admsudentslist, function (itm) {
                if (itm.adjselected == true) {
                    $scope.printdatatable.push(itm);
                }
            });
            $scope.get_total_adjstd();

        }


        $scope.toggleAllOB = function () {

            $scope.printdatatable = [];
            var toggleStatus = $scope.OBrs;
            angular.forEach($scope.opnbln, function (itm) {
                itm.OBselected = toggleStatus;
                if (itm.OBselected == true) {
                    $scope.printdatatable.push(itm);
                }
            });

            $scope.get_total_OB();
        }

        $scope.optionToggledOB = function () {

            $scope.printdatatable = [];
            angular.forEach($scope.opnbln, function (itm) {
                if (itm.OBselected == true) {
                    $scope.printdatatable.push(itm);
                }
            });
            $scope.get_total_OB();

        }





        $scope.submitted = false;
        $scope.ShowReport = function () {
            
            if ($scope.myForm.$valid) {

                var FMT_Ids = [];
                $scope.albumNameArraycolumn3 = [];
                angular.forEach($scope.groupcount, function (groupcount) {
                    if (!!groupcount.selected) $scope.albumNameArraycolumn3.push({
                        columnID3: groupcount.fmT_Id

                    });
                })

                angular.forEach($scope.groupcount, function (ty) {
                    if (ty.fmT_Id_chk) {
                        FMT_Ids.push(ty.fmT_Id);
                    }
                })


                var data = {
                    "active": $scope.active,
                    "deactive": $scope.deactive,
                    "left": $scope.left,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "FMH_Id": $scope.fmh_id,
                    "AMSC_Id": $scope.asmC_Id,
                    "TRMR_Id": $scope.trmR_Id,
                    "radioval": $scope.result,
                    "reporttype": $scope.allorindiorcon,
                    "type":$scope.report,
                    FMT_Ids: FMT_Ids,

                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeDetailsReport/radiobtndata", data).
              then(function (promise) {
                  

                  if (promise.reporttype == "all") {
                      if (promise.radioval == "FRW") {
                          if (promise.groupalldata != null && promise.groupalldata != "") {
                              $scope.grid_view = true;
                              $scope.print = true;
                              $scope.groups = promise.groupalldata;
                              $scope.Grid_view = true;
                              $scope.print_flag = false;
                              $scope.std = false;
                              $scope.cls = false;
                              $scope.had = false;
                              $scope.grp = true;
                              $scope.totcountfirst = promise.groupalldata.length;
                              $scope.tot = $scope.getTotalgrp(promise.groupalldata);

                          }
                          else {
                              swal("No Record Found");
                              $scope.Grid_view = false;
                              $scope.print_flag = true;
                          }

                      }
                      else if (promise.radioval == "FHW") {
                          if (promise.headalldata != null && promise.headalldata != "") {
                              $scope.heads = promise.headalldata;
                              $scope.Grid_view = true;
                              $scope.print_flag = false;
                              $scope.std = false;
                              $scope.cls = false;
                              $scope.had = true;
                              $scope.grp = false;
                              $scope.totcountfirst = $scope.heads.length;
                              $scope.tot = $scope.getTotalhd(promise.headalldata);

                          }
                          else {
                              swal("No Record Found");
                              $scope.Grid_view = false;
                              $scope.print_flag = true;
                          }

                      }
                      else if (promise.radioval == "FCW") {
                          if (promise.classalldata != null && promise.classalldata != "") {
                              $scope.classes = promise.classalldata;
                              
                              if ($scope.classes.length != 0) {
                                  $scope.Grid_view = true;
                                  $scope.print_flag = false;
                                  $scope.std = false;
                                  $scope.cls = true;
                                  $scope.had = false;
                                  $scope.grp = false;
                                  $scope.totcountfirst = $scope.classes.length;
                                  $scope.tot = $scope.getTotalcls(promise.classalldata);

                              }
                          }
                          else {
                              swal("No Record Found");
                              $scope.Grid_view = false;
                              $scope.print_flag = true;
                          }

                      }
                      else if (promise.radioval == "FSW") {
                          if (promise.studentalldata != null && promise.studentalldata != "") {
                              $scope.students = promise.studentalldata;
                              if ($scope.students.length != 0) {
                                  $scope.Grid_view = true;
                                  $scope.print_flag = false;
                                  $scope.std = true;
                                  $scope.cls = false;
                                  $scope.had = false;
                                  $scope.grp = false;
                                  $scope.totcountfirst = $scope.students.length;
                                  $scope.tot = $scope.getTotalstd(promise.studentalldata);

                              }
                          }
                          else {
                              swal("No Record Found");
                              $scope.Grid_view = false;
                              $scope.print_flag = true;
                          }

                      }
                  }
                  else {
                      if (promise.type == "WO") {
                          if (promise.duration != null && promise.duration != "") {
                              $scope.grid_view = true;
                              $scope.print = true;
                              $scope.waived = promise.duration;
                              $scope.Grid_view = true;
                              $scope.print_flag = false;
                              $scope.waiveoff = true;
                              $scope.false = true;
                              $scope.totcountfirst = promise.duration.length;
                             // $scope.tot = $scope.getTotalwaived(promise.duration);

                          }
                          else {
                              swal("No Record Found");
                              $scope.Grid_view = false;
                              $scope.print_flag = true;
                          }

                      }
                      else if (promise.type == "OB") {
                          if (promise.categorydata != null && promise.categorydata != "") {
                              $scope.grid_view = true;
                              $scope.print = true;
                              $scope.adjamt1 = false;
                              $scope.opnbln = promise.categorydata;
                              $scope.Grid_view = true;
                              $scope.print_flag = false;
                              $scope.opnblnd = true;
                              $scope.waiveoff = false;
                              $scope.totcountfirst = promise.categorydata.length;
                              // $scope.tot = $scope.getTotalwaived(promise.duration);

                          }
                          else {
                              swal("No Record Found");
                              $scope.Grid_view = false;
                              $scope.print_flag = true;
                          }

                      }

                      else {
                          if (promise.fillstudent != null && promise.fillstudent != "") {


                              $scope.adjamt1 = true;
                              $scope.grid_view = true;
                              $scope.print = true;
                              $scope.Grid_view = true;
                              $scope.print_flag = false;
                              $scope.opnblnd = false;
                              $scope.waiveoff = false;
                              $scope.term_list1 = [];
                              $scope.term_list = [];
                              $scope.adjusted = promise.fillstudent;
                              var temp_recp_list1 = promise.fillstudent;
                            
                           
                              for (var m = 0; m < promise.fillstudent.length; m++) {

                                  var stu_id = promise.fillstudent[m].AMST_Id;
                                  var FSA_Id = promise.fillstudent[m].FSA_Id;
                                  var class_n = promise.fillstudent[m].ASMCL_ClassName;
                                  var adj_amt = promise.fillstudent[m].FSA_AdjustedAmount;
                                  var adj_dt = promise.fillstudent[m].FSA_Date;
                                  var stu_nm = promise.fillstudent[m].StudentName;
                                  var already_cnt = 0;

                                  angular.forEach($scope.term_list1, function (itm1) {
                                      if (itm1.stu_id == stu_id && itm1.stu_rpt.FSA_Id == FSA_Id) {
                                          already_cnt += 1;
                                      }
                                  })

                                  if (already_cnt == 0) {
                                      $scope.temp_stu_rpt1 = [];
                                      var test1 = [];
                                      angular.forEach(temp_recp_list1, function (itm) {
                                          if (itm.AMST_Id == stu_id && itm.FSA_Id == FSA_Id) {
                                              $scope.temp_stu_rpt1.push(itm);
                                          }
                                      })
                                      angular.forEach($scope.temp_stu_rpt1, function (itm1) {
                                          var fsa_id = itm1.FSA_Id;
                                          var fmh_name = itm1.FMH_FeeName;
                                          if (itm1.FSA_Id == fsa_id) {
                                              test1.push({ FSA_Id: itm1.FSA_Id, FMH_FeeName: itm1.FMH_FeeName });
                                          }
                                      })
                                   
                                      $scope.term_list1.push({ stu_id: stu_id, FSA_Id: FSA_Id ,class_name: class_n, adjamt: adj_amt, adjdate: adj_dt, stuname: stu_nm, stu_rpt: test1 });
                                  }
                              }
                              var final_list = [];
                              $scope.admsudentslist = [];
                              for (var l = 0; l < $scope.term_list1.length; l++) {
                                  var st_id = $scope.term_list1[l].stu_id;
                                  var fsa_id = $scope.term_list1[l].FSA_Id;
                                  var st_id_cnt = 0;
                                  angular.forEach($scope.admsudentslist, function (itm1) {
                                      if (itm1.stu_id == st_id && itm1.FSA_Id == fsa_id) {
                                          st_id_cnt += 1;
                                      }
                                  })
                                  if (st_id_cnt == 0) {
                                      $scope.admsudentslist.push($scope.term_list1[l]);
                                  }
                              }
                              $scope.totcountfirst = $scope.admsudentslist.length;
                          }
                          else {
                              swal("No Record Found");
                              $scope.Grid_view = false;
                              $scope.print_flag = true;
                          }

                      }

                  }
                  

              })
            }
            else {
                $scope.submitted = true;

            }
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
            //|| field.$dirty;
        };



        $scope.printdatatable = [];
        $scope.exportToExcel = function () {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var tableid = '';
                if ($scope.allorindiorcon == "all") {
                    if ($scope.result == 'FHW') {
                        tableid = '#table2';
                    }
                    else if ($scope.result == 'FSW') {
                        tableid = '#table1';
                    }
                    else if ($scope.result == 'FCW') {
                        tableid = '#table3';
                    }
                    else if ($scope.result == 'FRW') {
                        tableid = '#table4';
                    }
                   
                }
                else {
                 if ($scope.report == 'WO') {
                    tableid = '#table5';
                    }
                 else if ($scope.report == 'AA') {
                     tableid = '#table6';
                 }
                 else{
                     tableid = '#table7';
                 }
                }

                var exportHref = Excel.tableToExcel(tableid, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }
          //  $state.reload();

        }


        $scope.printData = function () {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = "";
                if ($scope.allorindiorcon == "all") {
                    if ($scope.result == 'FHW') {
                        innerContents = document.getElementById("printSectionIdhad").innerHTML;
                    }
                    else if ($scope.result == 'FSW') {
                        innerContents = document.getElementById("printSectionIdstd1").innerHTML;
                    }

                    else if ($scope.result == 'FCW') {
                        innerContents = document.getElementById("printSectionIdcls").innerHTML;
                    }
                    else if ($scope.result == 'FRW') {
                        innerContents = document.getElementById("printSectionIdgrp").innerHTML;
                    }
                  
                }
                else {
                    if ($scope.report == 'WO') {
                        innerContents = document.getElementById("printwaive").innerHTML;
                    }
                   else if ($scope.report == 'AA') {
                        innerContents = document.getElementById("printadjust").innerHTML;
                    }
                   else {
                        innerContents = document.getElementById("printOB").innerHTML;
                    }
                }
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
          //  $state.reload();
        }

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }



        $scope.Clearid = function () {

            $state.reload();
        }

    }
})();
