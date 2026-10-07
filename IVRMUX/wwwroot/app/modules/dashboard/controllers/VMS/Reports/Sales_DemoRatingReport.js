(function () {
    'use strict';
    angular
        .module('app')
        .controller('SalesDrBlockedController', SalesDrBlockedController);

    SalesDrBlockedController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce', '$timeout', 'Excel'];
    function SalesDrBlockedController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce, $timeout, Excel) {


        //=============image
        $scope.SalesQuatationReport = [];
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;



        $scope.searchValue = '';
        $scope.searchValue1 = '';
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;


        $scope.yerlist = [{ yearid: 1, name: '2016', start: new Date('01/01/1016'), end: new Date('12/31/1016') },
        { yearid: 2, name: '2017', start: new Date('01/01/2017'), end: new Date('12/31/2017') },
        { yearid: 3, name: '2018', start: new Date('01/01/2018'), end: new Date('12/31/2018') },
        { yearid: 4, name: '2019', start: new Date('01/01/2019'), end: new Date('12/31/2019') },
        { yearid: 5, name: '2020', start: new Date('01/01/2020'), end: new Date('12/31/2020') },
        { yearid: 6, name: '2021', start: new Date('01/01/2021'), end: new Date('12/31/2021') },
        { yearid: 7, name: '2022', start: new Date('01/01/2022'), end: new Date('12/31/2022') },
        { yearid: 8, name: '2023', start: new Date('01/01/2023'), end: new Date('12/31/2023') },
        { yearid: 9, name: '2024', start: new Date('01/01/2024'), end: new Date('12/31/2024') },
        { yearid: 10, name: '2025', start: new Date('01/01/2025'), end: new Date('12/31/2025') },
        { yearid: 11, name: '2026', start: new Date('01/01/2026'), end: new Date('12/31/2026') },
        { yearid: 12, name: '2027', start: new Date('01/01/2027'), end: new Date('12/31/2027') },
        { yearid: 13, name: '2028', start: new Date('01/01/2028'), end: new Date('12/31/2028') },
        { yearid: 14, name: '2029', start: new Date('01/01/2029'), end: new Date('12/31/2029') },
        { yearid: 15, name: '2030', start: new Date('01/01/2030'), end: new Date('12/31/2030') }]

        $scope.grid_view = false;
        $scope.submitted = false;
        // $scope.leadlist = [];

        //get data
        $scope.OnLoadDetails = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("SalesDrBlocked/OnLoadDataReport", pageid).then(function (promise) {
                $scope.EmployeeData = promise.employeeData;
                $scope.EmployeeName = $scope.EmployeeData[0].EmployeeName;
            });
        };


        $scope.onchageradiobtn = function () {

            $scope.SalesQuatationReport = [];

        }

        $scope.submitted = false;
        $scope.showreport = function () {
            $scope.SalesQuatationReport = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "Year": $scope.ASMAY_YEAR
                }
                apiService.create("SalesDrBlocked/Employee_Ind_Detail/", data).
                    then(function (promise) {
                        if (promise.employee_Ind_Details.length > 0 && promise.employee_Ind_Details != null) {
                            $scope.Employee_Ind_Details = promise.employee_Ind_Details;

                            //for displaying grid
                            $scope.EmpList = [];

                            angular.forEach($scope.Employee_Ind_Details, function (dev) {
                                if ($scope.EmpList.length === 0) {
                                    $scope.EmpList.push(dev);
                                } else if ($scope.EmpList.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.EmpList, function (emp) {
                                        if (emp.HRME_ID === dev.HRME_ID) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.EmpList.push(dev
                                        );
                                    }
                                }
                            });


                            console.log($scope.EmpList);

                            angular.forEach($scope.EmpList, function (ddd) {
                                $scope.templist = [];

                                angular.forEach($scope.Employee_Ind_Details, function (dd) {
                                    if (dd.HRME_ID === ddd.HRME_ID) {

                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.datadetails = $scope.templist;

                            });


                            console.log($scope.EmpList);
                        }
                        else {
                            swal('No Data Found!!!');
                            $state.reload();
                        }

                    })
            }
            else {
                $scope.submitted = true;
            }
        };
        //Print
        $scope.printData = function () {
            var innerContents = document.getElementById("tableexcell").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


        $scope.exportToExcel = function (tableemployee) {
            var exportHref = Excel.tableToExcel(tableemployee, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }




        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }

})();