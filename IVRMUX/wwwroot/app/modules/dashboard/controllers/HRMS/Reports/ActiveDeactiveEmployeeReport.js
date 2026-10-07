(function () {
    'use strict';
    angular
        .module('app')
        .controller('ActiveDeactiveEmployeeReportController', ActiveDeactiveEmployeeReportController)

    ActiveDeactiveEmployeeReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache']
    function ActiveDeactiveEmployeeReportController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache) {


        $scope.loadData = function () {
            var id = 2;
            apiService.getURI("ActiveDeactiveEmployeeReport/getalldetails", id).then(function (promise) {

                $scope.institutionlist = promise.companydetails;


            });

        };

        //Select All

        $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.institutionlist, function (itm) {
                itm.select = checkStatus;
            });
        }        $scope.isOptionsRequired = function () {
            return !$scope.institutionlist.some(function (options) {
                return options.select;
            });
        }        $scope.togchkbx = function () {
            $scope.institutionlist.every(function (options) {
                return options.select;
            });
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


            
            //cancel
            $scope.cleardata = function () {
                $state.reload();
            };

            //Print
            $scope.printToCart = function (Baldwin) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            };

            //Execl
            $scope.exportToExcel = function () {
                var divToPrint = document.getElementById("BankCash");
                var blob = new Blob([divToPrint.outerHTML], {
                    type: "application/vnd.ms-excel;charset=utf-8"
                });
                saveAs(blob, "Report.xls");
            };        //Report        $scope.submitted = false;
        $scope.report = function () {
            $scope.griddata = [];
            $scope.submitted = true;
            $scope.selected_Inst = [];

            angular.forEach($scope.institutionlist, function (mm) {
                if (mm.select) {
                    $scope.selected_Inst.push(mm);
                }
            })
            if ($scope.myForm.$valid) {

                var data = {
                    
                    selected_Inst: $scope.selected_Inst,
                    "radiotype": $scope.radiotype,
                }

                apiService.create("ActiveDeactiveEmployeeReport/report", data).
                    then(function (promise) {

                        $scope.GetReport = promise.getReport;
                        if ($scope.GetReport.length > 0) {

                            $scope.griddata = promise.viewlist;
                            $scope.Cumureport = true;
                            $scope.Cumureport1 = false;
                            $scope.screport = true;
                            $scope.export = true;
                        }
                        else {
                            swal("No Records Found");

                            $scope.screport = false;
                            $scope.export = false;
                            $scope.Cumureport = false;
                            $scope.Cumureport1 = false;
                        }

                    })
            }

            $scope.institutionlist = promise.institutionlist;
         

        };


        }
    

})();