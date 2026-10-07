(function () {
    'use strict';
    angular
        .module('app')
        .controller('DRWavedOffReportController', DRWavedOffReportController)

    DRWavedOffReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter']
    function DRWavedOffReportController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter) {
        $scope.maxDatemf = new Date();
        //loading start hrmlY_Id = 2 hrmlY_LeaveYear = "2017"


        $scope.loadData = function () {

            var id = 2;
            apiService.getURI("FrontOffice/", id).
                then(function (promise) {
                    $scope.fillmonth = promise.getmonth;
                    $scope.fillyear = promise.getyear;
                });
        };
       
   

        $scope.isOptionsRequired = function () {
            return !$scope.fillmonth.some(function (options) {
                return options.selected;
            });
        }

        $scope.all_check = function () {

            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.fillmonth, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_departments();
        }

        //fill employee end
        //get report start
        $scope.submitted = false;
        $scope.GetReport = function () {
            var data = {};
            $scope.montharray = [];
            $scope.monthnames = [];
            angular.forEach($scope.fillmonth, function (aa) {
                if (aa.selected == true) {
                    $scope.montharray.push({ IVRM_Month_Id: aa.ivrM_Month_Id })
                    $scope.monthnames.push({ IVRM_Month_Name: "[" + aa.ivrM_Month_Name.substring(0, 3)+ "-Totalblockeddays"+"]" })
                    //$scope.monthnames.push({ IVRM_Month_Name: "[" + aa.monthname.substring(0, 3) + "-EmpFinalLOP" + "]" }) ivrM_Month_Name = "February"
                    $scope.monthnames.push({ IVRM_Month_Name: "[" + aa.ivrM_Month_Name.substring(0, 3) + "-EmpSanctioned" + "]" })
                }
            })

            $scope.arraycount = $scope.monthnames.length + 3;

            if ($scope.myForm.$valid) {
                    data = {
                        "year":  $scope.year,
                        "Monthlist": $scope.montharray
                    }
                    apiService.create("FrontOffice/DRWaveofReport", data).then(function (promise) {
                        if (promise.drblockedReport.length > 0) {
                            $scope.grid_view = true;
                            $scope.drblockedReport = promise.drblockedReport

                            }else {
                                $scope.grid_view = false;
                                swal("Record not found.");
                            }
                        })
            }
            else {
                $scope.submitted = true;
            }
        };


        var cars = ["Jan", "Feb", "March","April","May","Jun","July","Aug","A"];
        //get report end
        //print start
        $scope.printData = function () {
            var divToPrint = document.getElementById("table");
            var newWin = window.open("");
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();
        }
        //print end
        //export start

        $scope.exptoex = function () {
            var divToPrint = document.getElementById("table");
            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            saveAs(blob, "DR-Blocked Waved-Off Report.xls");
        };
        //export end
        //TO clear  data
        $scope.clearid = function () {
            $state.reload();
            }
       
        //clear end
    }
})();