
(function () {
    'use strict';
 angular
.module('app')
.controller('ConstraintReportController', ConstraintReportController)

    ConstraintReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout']
    function ConstraintReportController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, Excel, $timeout) {
                $scope.fix_res_print = true;
        $scope.conscon_print = true;
        $scope.bifcon_print = true;
        $scope.labcon_print = true;

        $scope.editEmployee = {};
      
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                 
            
            { name: 'ttmC_CategoryName', displayName: 'Category' },
            { name: 'asmcL_ClassName', displayName: 'Class' },
             { name: 'asmC_SectionName', displayName: 'Section' },
               { name: 'staffName', displayName: 'Staff Name' },
            { name: 'ismS_SubjectName', displayName: 'Subject Name' },
             { name: 'ttmD_DayName', displayName: 'Day' },
             { name: 'ttmP_PeriodName', displayName: 'Period' }
            ],          
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        $scope.gridOptions_cons = {
            enableColumnMenus: false,
            enableFiltering: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                 

            { name: 'categoryName', displayName: 'Category', editable: false },
            { name: 'className', displayName: 'Class', editable: false },
             { name: 'sectionName', displayName: 'Section', editable: false },
               { name: 'staffName', displayName: 'Staff Name', editable: false },
            { name: 'subjectName', displayName: 'Subject Name', editable: false },
             { name: 'noOfPeriods', displayName: 'Period', editable: false },
             
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;

            }

        };

        $scope.gridOptions_bif = {
            enableColumnMenus: false,
            enableFiltering: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                 
            { name: 'categoryName1', displayName: 'Category' },
            { name: 'bifricationName', displayName: 'Bifurcation Name' },
             { name: 'periodname', displayName: 'Period' }


            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }

        };

        $scope.gridOptions_lab = {
            enableColumnMenus: false,
            enableFiltering: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                 
            { name: 'asmayYear', displayName: 'Academic Year' },
            { name: 'categoryName2', displayName: 'Category Name' },
             { name: 'ttlaB_LABLIBName', displayName: 'Lab Name' }


            ], onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }

        };

        //TO get excel report
        $scope.exportToExcel = function () {
                $scope.gridApi.exporter.csvExport('all', 'all');
        };

        //TO print data
        $scope.printData = function () {
            

            //var divToPrint = document.getElementById("fixing");
            //var newWin = window.print();
            //newWin.document.write(divToPrint.outerHTML);
            //newWin.print();
            //newWin.close();
            var con;
            if ($scope.ttconstrep === "fix") {
                con = document.getElementById("fixing_print").innerHTML;
            }
            else if ($scope.ttconstrep === "restrict") {
                con = document.getElementById("restrict_print").innerHTML;
            }
            else if ($scope.ttconstrep === "cons") {
                con = document.getElementById("cons_print").innerHTML;
            }
            else if ($scope.ttconstrep === "bif") {
                con = document.getElementById("bif_print").innerHTML;
            }
            else if ($scope.ttconstrep === "lab") {
                con = document.getElementById("lab_print").innerHTML;
            }

            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
               '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
          '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);"><table border="1">' + con + '</table></html>');
            popupWinindow.document.close();

        }

        //TO get data on load
        $scope.LoadData = function () {
            
            apiService.getDATA("ConstraintReport/getalldetails").
       then(function (promise) {
           $scope.year_list = promise.acayear;
       })
        };

        //----------TO Get Repot
        $scope.header_display = true;
        $scope.header_display1 = true;
        $scope.submitted = false;
        $scope.GetReport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "constype": $scope.ttconstrep,
                }
                apiService.create("ConstraintReport/getpagedetails", data).
                  then(function (promise) {

                      if ($scope.ttconstrep === "fix") {
                        
                         if (promise.fix_day_period_list.length > 0)
                         {
                             $scope.header_display = false;
                             $scope.name = "Fixing";
                             $scope.gridOptions.data = promise.fix_day_period_list;
                             $scope.datafix = promise.fix_day_period_list;
                             $scope.fix_res = false;
                             $scope.conscon = true;
                             $scope.bifcon = true;
                             $scope.labcon = true;
                         }
                         else {
                             swal("No Data Found!!!");
                             $scope.clearid();
                         }                       
                      }
                      else if ($scope.ttconstrep === "restrict") {

                          if (promise.restrict_day_period_list.length > 0) {
                              $scope.header_display = false;
                              $scope.name = "Restrict";
                              $scope.gridOptions.data = promise.restrict_day_period_list;
                              $scope.datafix = promise.restrict_day_period_list;
                              $scope.fix_res = false;
                              $scope.conscon = true;
                              $scope.bifcon = true;
                              $scope.labcon = true;
                          }
                          else {
                              swal("No Data Found!!!");
                              $scope.clearid();

                          }
                      }
                      else if ($scope.ttconstrep === "cons") {

                          if (promise.consecutivelst.length > 0) {
                              $scope.header_display = false;
                              $scope.name = "Consecutive";
                              $scope.gridOptions_cons.data = promise.consecutivelst;
                              $scope.data_con = promise.consecutivelst;
                              $scope.conscon = false;
                              $scope.fix_res = true;
                              $scope.bifcon = true;
                              $scope.labcon = true;
                          }
                          else {
                              swal("No Data Found!!!");
                              $scope.clearid();

                          }
                      }
                      else if ($scope.ttconstrep === "bif") {

                          if (promise.bif_detailslist.length > 0) {
                              $scope.header_display = false;
                              $scope.name = "Bifurcation";
                              $scope.gridOptions_bif.data = promise.bif_detailslist;
                              $scope.data_bifer = promise.bif_detailslist;
                              $scope.bifcon = false;
                              $scope.fix_res = true;
                              $scope.labcon = true;
                              $scope.conscon = true;
                          }
                          else {
                              swal("No Data Found!!!");
                              $scope.clearid();

                          }
                      }
                      else if ($scope.ttconstrep === "lab") {

                          if (promise.labdetailsarray.length > 0) {
                              $scope.header_display = false;
                              $scope.name = "Lab";
                              $scope.gridOptions_lab.data = promise.labdetailsarray;
                              $scope.data_lab = promise.labdetailsarray;
                              $scope.labcon = false;
                              $scope.fix_res = true;
                              $scope.conscon = true;
                              $scope.bifcon = true;
                          }
                          else {
                              swal("No Data Found!!!");
                              $scope.clearid();

                          }
                      }
                      else {
                          swal("No Record found....!!")
                          $scope.submitted = false;
                      }

                  });
                }
        }

        //------TO clear  data
        $scope.clearid = function () {
            $scope.header_display = true;
            $scope.labcon = true;
            $scope.fix_res = true;
            $scope.conscon = true;
            $scope.bifcon = true;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };



    }

})();

