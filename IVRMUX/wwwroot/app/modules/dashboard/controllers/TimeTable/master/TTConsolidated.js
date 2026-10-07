
(function () {
    'use strict';
    angular
.module('app')
.controller('TTConsolidatedController', TTConsolidatedController)

    TTConsolidatedController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout']
    function TTConsolidatedController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, Excel, $timeout) {
      
        $scope.editEmployee = {};
        $scope.hstep = 1;
        $scope.mstep = 1;      

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.dayslst_fix = []; 
        //TO get data on load
        $scope.LoadData = function () {            
            apiService.getDATA("TTConsolidated/getalldetails").
       then(function (promise) {
           $scope.year_list = promise.acayear;
           $scope.category_list = promise.categorylist;
           $scope.class_list = promise.classlist;
           $scope.temp_classlist = promise.classlist;
           $scope.cls_List = promise.gridcls;
           $scope.dayslst_fix = promise.dayslst_fix;
       })
        };

        //----------TO Get Repot
        $scope.submitted = false;
        $scope.GetReport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                for (var i = 0; i < $scope.dayslst_fix.length; i++) {
                    $scope.GetReportNew($scope.dayslst_fix[i].ttmD_Id);
                }




                //var data = {
                //    "ASMAY_Id": $scope.asmaY_Id,
                //    "rpttypairods": $scope.rpttypai,
                //    "TTMD_Id": 14
                //}
                //apiService.create("TTConsolidated/getrpt", data).
                //    then(function (promise) {
                //        var count = promise.time_Table;
                //        if (count != 0) {
                //            if ($scope.rpttypai == "teac") {                             
                //                $scope.teacher_grid = true;
                //                $scope.stafflist = promise.stafflst;
                //                $scope.period_list = promise.periodslst; 
                //                $scope.timetablelist = promise.tt;
                //                $scope.daysList = promise.dayslst;
                //                $scope.GetReportNew(15);
                //                $scope.GetReportNew(16);
                //            }                         
                //        }
                //        else {                     
                //            $scope.teacher_grid = false;
                //            $scope.submitted = false;
                //            $scope.myForm.$setPristine();
                //            $scope.myForm.$setUntouched();
                //            swal("No Record found....!!")
                //        }
                //    })
            };
        }

        $scope.GetReportNew = function (ttmdid) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "rpttypairods": $scope.rpttypai,
                    "TTMD_Id": ttmdid
                }
                apiService.create("TTConsolidated/getrpt", data).
                    then(function (promise) {
                        var count = promise.time_Table;
                        if (count != 0) {
                            if ($scope.rpttypai == "teac") {

                                if (ttmdid == 14) {
                                    $scope.stafflist = promise.stafflst;
                                    $scope.period_list = promise.periodslst;
                                    $scope.timetablelist = promise.tt;
                                    $scope.daysList = promise.dayslst;
                                }
                                else if (ttmdid == 15) {
                                    $scope.stafflist2 = promise.stafflst;
                                    $scope.period_list2 = promise.periodslst;
                                    $scope.timetablelist2 = promise.tt;
                                    $scope.daysList2 = promise.dayslst;
                                }
                                else if (ttmdid == 16) {
                                    $scope.stafflist3 = promise.stafflst;
                                    $scope.period_list3 = promise.periodslst;
                                    $scope.timetablelist3 = promise.tt;
                                    $scope.daysList3 = promise.dayslst;
                                }
                                else if (ttmdid == 17) {
                                    $scope.stafflist4 = promise.stafflst;
                                    $scope.period_list4 = promise.periodslst;
                                    $scope.timetablelist4 = promise.tt;
                                    $scope.daysList4 = promise.dayslst;
                                } 
                                else if (ttmdid == 18) {
                                    $scope.stafflist5 = promise.stafflst;
                                    $scope.period_list5 = promise.periodslst;
                                    $scope.timetablelist5 = promise.tt;
                                    $scope.daysList5 = promise.dayslst;
                                }   
                            }
                        }
                        else {
                            $scope.teacher_grid = false;
                            $scope.submitted = false;
                            $scope.myForm.$setPristine();
                            $scope.myForm.$setUntouched();
                            swal("No Record found....!!")
                        }
                    })
            };
        }
       
        //---export to excel   
        var exportHref = "";
        $scope.exptoex = function () {         
            exportHref = Excel.tableToExcel(tabelexport, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);            
        };
        $scope.exptoex1 = function () {
            exportHref = Excel.tableToExcel(tablediv1, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };
        $scope.exptoex2 = function () {
            exportHref = Excel.tableToExcel(tablediv2, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };
        //------TO clear  data
        $scope.clearid = function () {       
            $scope.teacher_grid = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };

    }

})();