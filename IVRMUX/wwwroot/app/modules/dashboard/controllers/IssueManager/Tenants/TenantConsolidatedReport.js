(function () {
    'use strict';
    angular
        .module('app')
        .controller('TenantConsolidatedReportController', TenantConsolidatedReportController);
    TenantConsolidatedReportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter'];
    function TenantConsolidatedReportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter) {

        $scope.start_Date = new Date();
        $scope.end_Date = new Date();
        var paginationformasters;
        $scope.multidepartment = false;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.obj = {};
        $scope.stdall = false;
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();

        $scope.submitted = false;
        angular.forEach($scope.get_printdata, function (obj) {
            obj.stdselected = false;
        });

        $scope.start_Date = new Date();
        $scope.end_Date = new Date();

        $scope.all_checkC = function () {
            var checkStatus = $scope.usercheckC;
            var count = 0;
            angular.forEach($scope.building, function (itm) {
                itm.selected = checkStatus;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }

        $scope.all_checkC1 = function () {
            var checkStatus = $scope.usercheckC1;
            var count = 0;
            angular.forEach($scope.getfloordetails, function (itm) {
                itm.selected1 = checkStatus;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
            $scope.togchkbxC1();
        }

        
        $scope.all_checkC2 = function (all) {
        
            $scope.usercheckC2 = all;

            var toggleStatus = $scope.usercheckC2;
            angular.forEach($scope.rooms, function (role) {
                role.selected2 = toggleStatus;
            });
         
            $scope.TenantDetails();
        }

        $scope.togchkbxC = function () {
            var HRMBID = [];
            angular.forEach($scope.building, function (ty) {
                if (ty.selected) {
                    HRMBID.push(ty.hrmB_Id);
                }
            })
            var data = {

                "buldid": HRMBID,

            }

            apiService.create("TenantPaymentReport/buildingchange", data).
                then(function (promise) {
                    $scope.rooms = promise.rooms;

                    $scope.getfloordetails = promise.getfloordetails;


                })
        }
        $scope.togchkbxC1 = function () {
            var HRMBID = [];
            //$scope.getfloordetails = [];
            angular.forEach($scope.building, function (ty) {
                if (ty.selected) {
                    HRMBID.push(ty.hrmB_Id);
                }
            })
            var HRMFID = [];
            angular.forEach($scope.getfloordetails, function (ty) {
                if (ty.selected1) {
                    HRMFID.push(ty.hrmF_Id);
                }
            })
            var data = {
                buldid: HRMBID,
                floorid: HRMFID,


            }

            apiService.create("TenantPaymentReport/floorchange", data).
                then(function (promise) {


                    $scope.rooms = promise.rooms;


                })
        }

        $scope.isOptionsRequired = function () {
            return !$scope.building.some(function (options) {
                return options.selected;
            });
        }
        $scope.isOptionsRequired1 = function () {
            return !$scope.getfloordetails.some(function (options) {
                return options.selected1;
            });
        }
        $scope.isOptionsRequired2 = function () {
            return !$scope.rooms.some(function (options) {
                return options.selected2;
            });
        }
        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            //$scope.itemsPerPage = 5;
            $scope.search = "";
            var pageid = 2;

            apiService.getURI("TenantPaymentReport/getdetails", pageid).then(function (promise) {

                $scope.mI_Id = promise.mI_Id;
                $scope.institutionlist = promise.getinstitution;
                $scope.building = promise.building;
                $scope.rooms = promise.rooms;

                $scope.getfloordetails = promise.getfloordetails;


                //$scope.hrmD_Id ="4";


            });
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;
                $scope.reverse = !$scope.reverse;
            };
        };





        $scope.institutionchange = function () {
                var data = {
                    "MI_Id": $scope.mI_Id,
                }
                apiService.create("TenantPaymentReport/institutionchange", data).then(function (promise) {

                    if (promise.building.length > 0) {
                        $scope.building = promise.building;
                    }
                    else {
                        swal("Building is not found..... !!");
                    }

                });
        };

        $scope.rentmonthlypayment = function (user) {
            $scope.paymenttype = "Rent";
            $scope.TenantMonthlyPaymentDetails(user)
        }
        $scope.watermonthlypayment = function (user) {
            $scope.paymenttype = "WaterBill";
            $scope.TenantMonthlyPaymentDetails(user)
        }
        $scope.electricitymonthlypayment = function (user) {
            $scope.paymenttype = "Electrical";
            $scope.TenantMonthlyPaymentDetails(user)
        }



        $scope.TenantMonthlyPaymentDetails = function (user) {
                var data = {
                    "MI_Id": $scope.mI_Id,
                    "TENANTAGR_Id": user.TENANTAGR_Id,
                    "type": $scope.paymenttype
                }
            apiService.create("TenantPaymentReport/TenantMonthlyPaymentDetails", data).then(function (promise) {

                if (promise.tenant_array.length > 0) {
                    $scope.tenant_array = promise.tenant_array;
                    $('#GetTenantMontlyData').modal('show');
                    }
                    else {
                        swal("Building is not found..... !!");
                    }

                });
        };


        $scope.TenantDetails = function () {
            $scope.submitted = true;
            $scope.tenantdetails = [];
            if ($scope.myForm.$valid) {
                var HRMBID = [];
                angular.forEach($scope.building, function (ty) {
                    if (ty.selected) {
                        HRMBID.push(ty.hrmB_Id);
                    }
                })
                var HRMRID = [];
                angular.forEach($scope.rooms, function (ty) {
                    if (ty.selected2) {
                        HRMRID.push(ty.hrmR_Id);
                    }
                })
                var data = {
                    "buldid": HRMBID,
                    "roomid": HRMRID
                }
                apiService.create("TenantPaymentReport/TenantDetails", data).then(function (promise) {

                    if (promise.tenantDetails.length > 0) {
                        $scope.tenantdetails = promise.tenantDetails;
                    }
                    else {
                        swal("Tenants are not mapped..... !!");
                    }

                });

            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.getreport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var HRMBID = [];
                angular.forEach($scope.building, function (ty) {
                    if (ty.selected) {
                        HRMBID.push(ty.hrmB_Id);
                    }
                })
                var HRMFID = [];
                angular.forEach($scope.getfloordetails, function (ty) {
                    if (ty.selected1) {
                        HRMFID.push(ty.hrmF_Id);
                    }
                })
                var HRMRID = [];
                angular.forEach($scope.rooms, function (ty) {
                    if (ty.selected2) {
                        HRMRID.push(ty.hrmR_Id);
                    }
                })

                if ($scope.allindividual == "all") {
                    $scope.tenantagR_Id = 0;
                } 

                var data = {

                    "buldid": HRMBID,
                    "floorid": HRMFID,
                    "roomid": HRMRID,
                    "flag": $scope.activevacant,
                    "TENANTAGR_Id": $scope.tenantagR_Id.tenantagR_Id,
                    "type": $scope.allindividual,
                    "MI_Id": $scope.mI_Id
                    //"TENANTAGR_AgrFromDate": new Date($scope.start_Date).toDateString(),
                    //"TENANTAGR_AgrToDate": new Date($scope.end_Date).toDateString(),

                }



                apiService.create("TenantPaymentReport/TenantConsolidatedReport", data).then(function (promise) {

                    if (promise.tenantDetails.length > 0) {

                        $scope.getloaddata = promise.tenantDetails;
                        $scope.presentCountgrid = promise.tenantDetails.length;
                    }
                    else {
                        swal("No Record Found..... !!");
                        $scope.get_printdata = "";
                        $state.reload();
                    }

                });

            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printDeviation").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.exportToExcel = function (export_id) {
            var exportHref = Excel.tableToExcel(export_id, 'get_printdetail');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.Clear = function () {
            $state.reload();
            //$scope.get_report = "";

            //$scope.get_printdata = "";
            //$scope.HRMD_Id = "";
            //$scope.start_Date = new Date();
            //$scope.end_Date = new Date();
            //$scope.checkboxval = false;
            //$scope.checkboxval1 = false;
            //$scope.loaddata();
        };



    }
})();

