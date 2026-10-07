(function () {
    'use strict';
    angular
.module('app')
        .controller('FeeTallyTransactionController', FeeTallyTransactionController)
    FeeTallyTransactionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function FeeTallyTransactionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.cfg = {};
        $scope.stuwiseorheadwise = "";
        $scope.formload = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters
            var pageid = 1;
            apiService.create("FeeTallyTransaction/getalldetails", pageid).
                then(function (promise) {

                    $scope.yearlst = promise.academicdrp;

                    $scope.cfg.ASMAY_Id = academicyrlst[0].asmaY_Id;

                    $scope.classcount = promise.allclsdata;

                    $scope.inscount = promise.allinsdata;

                })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.submitted = false;
        $scope.SHOWSTU = function (inscount) {

            $scope.albumNameArray = [];
            angular.forEach($scope.inscount, function (option) {
                if (!!option.selected) $scope.albumNameArray.push(option);
            })

            $scope.submitted = true;
            if ($scope.myform.$valid) {

                if ($scope.albumNameArray.length>0) {

                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id
                    //TempararyArrayList: $scope.albumNameArray,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeTallyTransaction/SHOWSTUDENT", data).
                    then(function (promise) {
                        if (promise.fillstudent.length > 0) {
                            $scope.totcountfirst = promise.fillstudent.length;
                            $scope.totalgrid = promise.fillstudent;
                        }

                    })
            }
            else {
                swal("Atleast Select any one Term")
            }
            }
            else {
                $scope.submitted = true;
            }
        }


        $scope.cleardata = function () {
            $state.reload();
        }


        $scope.toggleAll = function () {
            debugger;
            $scope.printdatatable = [];
            var toggleStatus = $scope.all;
            angular.forEach($scope.totalgrid, function (itm) {
                itm.isSelected = toggleStatus;
                if ($scope.all == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
          

        }
       




        //$scope.GENERATERV = function () {

        //    $scope.abc = 1;
        //    $scope.submitted = true;

        //    if ($scope.myform.$valid) {
        //        var data = {
        //            "ASMAY_Id": $scope.cfg.ASMAY_Id,
        //            "From_Date": new Date($scope.fromDate).toDateString(),
        //            "To_Date": new Date($scope.todate).toDateString(),
        //        }

        //        var config = {
        //            headers: {
        //                'Content-Type': 'application/json;'
        //            }
        //        }
        //        apiService.create("FeeTallyTransaction/savedataRV", data).
        //            then(function (promise) {

        //                if (promise.returnval == "true") {
        //                    swal("Record Saved Sucessfully");
        //                }
        //                else {
        //                    swal("Record Not Saved Sucessfully")
        //                }

        //            })
        //    }
        //    else {
        //        $scope.submitted = true;
        //    }
        //}








        $scope.submitted = false;
        $scope.GENERATEJV = function (totalgrid) {
            
            $scope.abc = 1;
            $scope.submitted = true;

            $scope.albumNameArray = [];
            var ftiidss = "0";
            angular.forEach($scope.inscount, function (option) {
                if (!!option.selected) {
                    ftiidss = ftiidss + "," + option.fmT_Id
                    $scope.albumNameArray.push(option);
                } 
            })


            var AMST_id = [];
            angular.forEach($scope.totalgrid, function (ty) {
                if (ty.isSelected) {
                    AMST_id.push(ty.amsT_Id);
                }
            })
          

            if ($scope.myform.$valid) {
                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ftiidss": ftiidss,
                    Amst_Ids: AMST_id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeTallyTransaction/savedata", data).
                    then(function (promise) {

                        if (promise.returnval == "true") {
                            swal("Journal Vouchers Created Sucessfully");
                        }
                        else {
                            swal("Journal Vouchers Not Created Sucessfully")
                        }
                        
                        $state.reload();

                    })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.GENERATERV = function () {
            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                "From_Date": new Date($scope.FMCB_fromDATE).toDateString(),
                "To_Date": new Date($scope.FMCB_toDATE).toDateString(),
            }
            apiService.create("FeeTallyTransaction/deletedata", data).
                then(function (promise) {

                    if (promise.returnval == "true") {
                        swal("Receipt Vouchers Generated Sucessfully");
                    }
                    else {
                        swal("Receipt Vouchers Not Generated Sucessfully")
                    }

                    $state.reload();

                })
        }


        $scope.GENERATECONCESSIONJV = function (totalgrid) {

            $scope.abc = 1;
            $scope.submitted = true;

            $scope.albumNameArray = [];
            var ftiidss = "0";
            angular.forEach($scope.inscount, function (option) {
                if (!!option.selected) {
                    ftiidss = ftiidss + "," + option.fmT_Id
                    $scope.albumNameArray.push(option);
                }
            })


            var AMST_id = [];
            angular.forEach($scope.totalgrid, function (ty) {
                if (ty.isSelected) {
                    AMST_id.push(ty.amsT_Id);
                }
            })


            if ($scope.myform.$valid) {
                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ftiidss": ftiidss,
                    Amst_Ids: AMST_id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeTallyTransaction/Concessiondata", data).
                    then(function (promise) {

                        if (promise.returnval == "true") {
                            swal("Concession Vouchers Created Sucessfully");
                        }
                        else {
                            swal("Concession Vouchers Not Created Sucessfully")
                        }

                        $state.reload();

                    })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.GENERATEPAYMENTRV = function () {
            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                "From_Date": new Date($scope.FMCB_fromDATE).toDateString(),
                "To_Date": new Date($scope.FMCB_toDATE).toDateString(),
            }
            apiService.create("FeeTallyTransaction/Paymentdata", data).
                then(function (promise) {

                    if (promise.returnval == "true") {
                        swal("Payment Vouchers Generated Sucessfully");
                    }
                    else {
                        swal("Payment Vouchers Not Generated Sucessfully")
                    }

                    $state.reload();

                })
        }


        //$scope.Import = function () {

        //    $scope.currentPage = 1;
        //    $scope.itemsPerPage = 5
        //    var pageid = 1;
        //    $scope.tabel12 = false;
        //    $scope.tabel123 = false;
        //    apiService.getURI("FeeTallyTransaction/getalldetails", pageid).
        //        then(function (promise) {
        //            $scope.yearlst = promise.fillyear;
                  
        //        })
        //}

        $scope.Import = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            var pageid = 1;
            $scope.tabel12 = false;
            $scope.tabel123 = false;
            apiService.getURI("FeeTallyTransaction/getalldetails", pageid).
                then(function (promise) {
                    $scope.yearlst = promise.fillyear;

                })
        }






    }

})();



