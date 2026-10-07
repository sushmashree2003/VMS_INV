(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_ProductReportController', INV_ProductReportController)
    INV_ProductReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', 'superCache']
    function INV_ProductReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, superCache) {


        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        //if (admfigsettings.length != null && admfigsettings.length > 0) {
        //    $scope.imgname = admfigsettings[0].asC_Logo_Path;
        //}

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        // $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //=================
        //==========
        $scope.onclickchange = function () {
            $scope.invmP_Id='';
            $scope.loaddata($scope.MI_Id);
        }
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            var data ={
                "MI_Id_new": $scope.MI_Id
            }
            apiService.create("INV_ProductReport/getalldetails", data).
                then(function (promise) {
                    $scope.get_product = promise.get_product;

                    $scope.staffcheck = promise.staffcheck;
                    if ($scope.MI_Id == undefined) {
                        $scope.get_mi_list = promise.get_mi_list;
                    }


                    if ($scope.staffcheck == 'Staff') {
                        $scope.MI_Id = promise.get_mi_list[0].MI_Id;
                    }

                })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        }




        $scope.getdata = function () {

            var data = {
                "INVMP_Id": $scope.invmP_Id,
            }
            apiService.create("INV_ProductReport/getdata", data).
                then(function (promise) {
                  
                    $scope.get_productlist = promise.get_productlist;
                    
                    $scope.get_item = promise.get_item;
                    $scope.earnlen = $scope.get_item.length;
                    $scope.dedlen = $scope.get_productlist.length;
                    console.log($scope.get_productlist);
                    $scope.imgname = promise.mI_logo;
                });
        };




        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.submitted = false;


        $scope.cancel = function () {
            $state.reload();
        }



        //$scope.printToCart = function () {
        //    var innerContents = document.getElementById("printtag").innerHTML;
        //    var popupWinindow = window.open('');
        //    popupWinindow.document.open();
        //    popupWinindow.document.write('<html><head>' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
        //        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
        //    popupWinindow.document.close();
        //}

        $scope.printData = function () {
            var innerContents = document.getElementById("printtag").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.getreport = function () {

            if ($scope.myForm.$valid) {

                var data = {
                    "INVMP_Id": $scope.invmP_Id,
                    "MI_Id_new": $scope.MI_Id
                }

                apiService.create("INV_ProductReport/radiobtndata", data).
                    then(function (promise) {
                        if ((promise.get_productTax !== null && promise.get_productTax !== "" && promise.get_productTax.length > 0) || (promise.gridproductTax !== null && promise.gridproductTax !== "" && promise.gridproductTax.length>0)) {
                            $scope.imgname = promise.mI_logo;
                            $scope.itemlst = promise.get_productTax;
                            $scope.stglst = promise.gridproductTax;
                            $scope.Grid_view = true;
                            //$scope.print_flag = false;


                            console.log($scope.itemlst);

                            $scope.templist = [];

                            angular.forEach($scope.itemlst, function (dd) {
                                if ($scope.templist.length === 0) {
                                    $scope.templist.push(dd);
                                } else if ($scope.templist.length > 0) {
                                    var count = 0;
                                    angular.forEach($scope.templist, function (ee) {
                                        if (ee.INVMP_Id === dd.INVMP_Id) {
                                            count += 1;
                                        }
                                    });
                                    if (count === 0) {
                                        $scope.templist.push(dd);
                                    }
                                }

                               
                            });

                            console.log($scope.stglst);
                            console.log($scope.templist);

                            angular.forEach($scope.templist, function (ddd) {
                                $scope.templistnew = [];
                                $scope.stglstnew = [];
                                angular.forEach($scope.itemlst, function (d) {
                                    if (ddd.INVMP_Id === d.INVMP_Id) {
                                        $scope.templistnew.push(d);
                                    }
                                });
                                ddd.itemlistnew = $scope.templistnew;

                                angular.forEach($scope.stglst, function (d) {
                                    if (ddd.INVMP_Id === d.INVMP_Id) {
                                        $scope.stglstnew.push(d);
                                    }
                                });

                                ddd.stglstnewnew = $scope.stglstnew;
                            });
                            console.log("DDDDDDDDDDD");
                            console.log($scope.templist);

                            $scope.totallist = [];

                            //angular.forEach($scope.itemlst, function (stuw1) {
                            //    var p = 0;

                            //    for (var q = 0; q < $scope.get_item.length; q++) {
                            //            angular.forEach(stuw1, function (x, y) {
                            //                    var a = x;
                            //                    var b = y;
                            //                if (b == $scope.get_item[q].INVMPI_ItemQty) {
                            //                        p = p+a;

                            //                        console.log(p);
                            //                    }
                            //                });
                            //            }

                            //    angular.forEach($scope.stglst, function (pdast) {
                            //                if (pdast.AMST_Id == stuw1.AMST_Id) {

                            //                    for (var i = 0; i < $scope.get_productlist.length; i++) {
                            //                        angular.forEach(pdast, function (x, y) {
                            //                            var c = x;
                            //                            var d = y;
                            //                            if (d == $scope.get_productlist[i].INVMPSS_Status) {
                            //                                p = p + c;

                            //                                console.log(p);
                            //                            }
                            //                        });
                            //                    }

                            //                }
                            //            })
                            //    $scope.totallist.push({ INVMP_Id: stuw1.INVMP_Id, totexp: p });
                            //            console.log($scope.totallist);

                            //})
                            $scope.presentCountgrid = $scope.templist.length;
                        }
                        else {
                            swal("No Record Found");
                            //$scope.Grid_view = false;
                            $scope.templist = ""
                        }
                    });
            }
            else {
                $scope.submitted = true;

            }
        };

    }
})();