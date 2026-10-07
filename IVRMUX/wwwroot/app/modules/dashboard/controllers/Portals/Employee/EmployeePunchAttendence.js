(function () {
    'use strict';
    angular
.module('app')
.controller('EmployeePunchAttendenceController', EmployeePunchAttendenceController)

    EmployeePunchAttendenceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter']
    function EmployeePunchAttendenceController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter) {

        $scope.maxDatemf = new Date();
        //loading start

        $scope.loadData = function () {
            
            var id = 2;
            apiService.getURI("EmployeePunchAttendence/getalldetails/", id).
        then(function (promise) {
            

            $scope.empDetails = promise.filldepartment;
            $scope.Fname = $scope.empDetails[0].empFname;
            $scope.Mname = $scope.empDetails[0].empMname;
            $scope.Lname = $scope.empDetails[0].empLname;
            $scope.HRME_DOJ = $scope.empDetails[0].hrmE_DOJ
            $scope.HRMD_DepartmentName = $scope.empDetails[0].hrmD_DepartmentName;
            $scope.HRMDES_DesignationName = $scope.empDetails[0].hrmdeS_DesignationName;
            $scope.grid_show = false;
        })

        };
        //loading end
        $scope.gettodate = function () {
            
            //var Datepic1 = $scope.fromdate == null ? "" : $filter('date')($scope.fromdate, "yyyy-MM-dd");
            var fromdate1 = $scope.fromdate == null ? "" : $filter('date')($scope.fromdate, "yyyy-MM-dd");
            var todate1 = $scope.todate == null ? "" : $filter('date')($scope.todate, "yyyy-MM-dd");
            //var month1 =  new Date($scope.fromdate).getMonth();
            //var year1 = new Date($scope.fromdate).getFullYear();
            //var groupidss = $scope.hrmE_Id;
            var data = {
                //"selectdate": Datepic1,
                //"selectmonth": month1,
                //"selectyear": year1,
                "fromdate": fromdate1,
                "todate": todate1,
                //"multiplehrmeid": groupidss,
                //"punchtype": 'punch',
            }
           

            apiService.create("EmployeePunchAttendence/getrpt/", data).
                      then(function (promise) {
                          
                         
                      $scope.filldata = promise.emp_punchDetails;
                      if ($scope.filldata !== null && $scope.filldata.length > 0) {
                          $scope.grid_show = true;
                          var mainlist = [];
                          angular.forEach(promise.emp_punchDetails, function (po) {
                              var date = new Date(po.punchdate).toDateString();
                              var i_o_list = [];
                              angular.forEach(promise.emp_punchDetails, function (po1) {
                                  var date1 = new Date(po1.punchdate).toDateString();
                                  if (date == date1) {
                                      i_o_list.push(po1);
                                  }
                              })
                              if (mainlist.length == 0) {
                                  mainlist.push({ punchdate: po.punchdate, io_list: i_o_list });
                              }
                              else if (mainlist.length > 0) {
                                  var al_ct = 0;
                                  angular.forEach(mainlist, function (it) {
                                      var date_MEW = new Date(it.punchdate).toDateString();
                                      if (date_MEW == date) {
                                          al_ct += 1;
                                      }
                                  })
                                  if (al_ct == 0) {
                                      mainlist.push({ punchdate: po.punchdate, io_list: i_o_list });
                                  }
                              }
                          })

                          $scope.filldata = mainlist;
                      } 
                    else {
                              $scope.grid_show = false;
                              swal("No Record Found")
                         }

                          ////$scope.intime = [];
                          ////$scope.outtime = [];
                          ////for (var i = 0; i <= $scope.filldata.length; i++)
                          ////{
                          ////    if ($scope.filldata[i].inOutFlg=='I')
                          ////    {
                          ////        $scope.intime.push({ "intime": $scope.filldata[i].punchtime, "punchdate": $scope.filldata[i].punchdate })
                          ////    }else if($scope.filldata[i].inOutFlg=='O')
                          ////    {
                          ////        if ($scope.intime.length > 0!= null && $scope.intime != "") {
                          ////            for (var j = 0; j < $scope.intime.length; j++) {
                          ////                if ($scope.intime[j].punchdate == $scope.filldata[i].punchdate) {
                          ////                    $scope.outtime.push({ "outtime": $scope.filldata[i].punchtime, "punchdate": $scope.filldata[i].punchdate })
                          ////                }
                          ////                else {
                          ////                    $scope.outtime.push({ "outtime": "-"})
                          ////                }
                          ////            }
                          ////        }
                          ////    }
                          ////}
                          
                          ////console.log($scope.intime);
                          ////console.log($scope.outtime);
                        
                      }
                   )
        };
       

    }
})();