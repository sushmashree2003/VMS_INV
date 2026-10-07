(function () {
    'use strict';
    angular
        .module('app')
        .controller('IVRMandVMSInterlinkingDashboardController', IVRMandVMSInterlinkingDashboardController)

    IVRMandVMSInterlinkingDashboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function IVRMandVMSInterlinkingDashboardController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.SalaryFromDay = "";
        $scope.SalaryToDay = "";
        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("IVRMandVMSInterlinkingDashboard/getalldetails", pageid).then(function (promise) {
                if (promise.clintlist !== null && promise.clintlist.length > 0) {
                    $scope.clintlist = promise.clintlist;
                    $scope.showclientdetails = false;
                    $scope.showpaymentdetails = false;
                    $scope.showmoduledetails = false;
                }
            });
        };

        $scope.showclientdetails = false;
        $scope.clientdetails = function () {
           
            if ($scope.showclientdetails == false) {
                $scope.showclientdetails = true;
                $scope.showpaymentdetails = false;
                $scope.showmoduledetails = false;
            }
            else if ($scope.showclientdetails == true) {
                $scope.showclientdetails = false;
            }

        };

        $scope.showpaymentdetails = false;
        $scope.paymentdetails = function () {
        
            if ($scope.showpaymentdetails == false) {
                $scope.showpaymentdetails = true;
                $scope.showclientdetails = false;
                $scope.showmoduledetails = false;
            }
            else if ($scope.showpaymentdetails == true) {
                $scope.showpaymentdetails = false;
            }
        };

        $scope.showmoduledetails = false;
        $scope.moduledetails = function () {

            if ($scope.showmoduledetails == false) {
                $scope.showmoduledetails = true;
                $scope.showclientdetails = false;
                $scope.showpaymentdetails = false;
            }
            else if ($scope.showmoduledetails == true) {
                $scope.showmoduledetails = false;
            }
        };





        




        $scope.employeeDetails = [];
        $scope.EmployeeDis = false;
        $scope.clientIEDetails = {};
        //Search employee
        $scope.submitted = false;
        $scope.getclientdetails = function (ismmclT_Id) {
            $scope.submitted = true;


            angular.forEach($scope.clintlist, function (itm) {
                if (itm.ismmclT_Id == ismmclT_Id.ismmclT_Id) {
                    $scope.ismmclT_ClientName = itm.ismmclT_ClientName;
                    $scope.ismmclT_Address = itm.ismmclT_Address;
                }
            });

            if ($scope.myForm.$valid) {

                var data = {
                    "ISMMCLT_Id": $scope.ismmclT_Id.ismmclT_Id
                };
                apiService.create("IVRMandVMSInterlinkingDashboard/getclientdetails", data).then(function (promise) {
                    if (promise.clientIEDetails !== null && promise.clientIEDetails.length > 0) {
                        $scope.clientIEDetails = promise.clientIEDetails;
                           
                    }
                    if (promise.clientpaymentdetails !== null && promise.clientpaymentdetails.length > 0) {
                        $scope.clientpaymentdetails = promise.clientpaymentdetails;
                           
                        }
                       
                    });
            }
        };





















        $scope.employeeDetails = [];
        $scope.EmployeeDis = false;
        $scope.institutionDetails = {};
        //Search employee
        $scope.submitted = false;
        $scope.SearchFReport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = { "HRME_Id": $scope.hrmE_Id };
                apiService.create("MonthEndReport/SearchFReport", data).
                    then(function (promise) {
                        if (promise.employeeDetailssingle !== null && promise.employeeDetailssingle.length > 0) {
                            $scope.EmployeeDis = true;
                            $scope.employeeDetailssingle = promise.employeeDetailssingle[0];
                            $scope.leavedetails = promise.leavedetails;
                            $scope.currentdate = new Date();
                        }
                        else {
                            $scope.EmployeeDis = false;
                            swal('No Record found to display .. !');
                            return;
                        }
                    });
            }
        };

        //By Employee Type
        $scope.GetEmployeeBygroupTypeAll = function (groupTypeselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.groupTypeselectedAll;
            angular.forEach($scope.groupTypedropdown, function (itm) {
                itm.selected = toggleStatus;

            });
            $scope.get_depts();
        }


        //single
        $scope.GetEmployeeBygroupType = function (groupType) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.groupTypeselectedAll = $scope.groupTypedropdown.every(function (itm) {

                return itm.selected;
            });
            $scope.get_depts();
        }

        $scope.get_depts = function () {
            var ids = [];
            angular.forEach($scope.groupTypedropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.hrmgT_Id);
                }
            })
            var data = {
                hrmgT_IdList: ids
            }
            apiService.create("CumulativeSalaryReport/get_depts", data).
                then(function (promise) {

                    if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                        $scope.departmentdropdown = promise.departmentdropdown;
                        $scope.departmentselectedAll = true;
                        $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                    }
                })
        };

        //By Department
        $scope.GetEmployeeByDepartmentAll = function (departmentselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.departmentselectedAll;
            angular.forEach($scope.departmentdropdown, function (itm) {
                itm.selected = toggleStatus;

            });


        }


        //By Department Single
        $scope.GetEmployeeByDepartment = function (department) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {

                return itm.selected;
            });
        }



        //By Designation
        $scope.GetEmployeeByDesignationAll = function (designationselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.designationselectedAll;
            angular.forEach($scope.designationdropdown, function (itm) {
                itm.selected = toggleStatus;

            });

        }


        //By Designation Single
        $scope.GetEmployeeByDesignation = function (designation) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {

                return itm.selected;
            });
        }


        //Clear data
        $scope.Employee = {};
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.employeeDetails = [];
            $scope.submitted = false;
            $scope.groupTypeselectedAll = false;
            $scope.departmentselectedAll = false;
            $scope.designationselectedAll = false;
            $scope.employeeSelectedAll = false;
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.search = "";

            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.onLoadGetData();

        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.disableGrid = function () {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
        }

        //$scope.printData = function () {
        //    var divToPrint = document.getElementById("Table");
        //        var newWin = window.open();
        //        newWin.document.write(divToPrint.outerHTML);
        //        newWin.print();
        //        newWin.close();
        //        // $state.reload();
        //    }

        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("PFChallan").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function () {
            var divToPrint = document.getElementById("BankCash");
            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            saveAs(blob, "Report.xls");
        };

    }

})();