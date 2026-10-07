(function () {
    'use strict';
    angular
        .module('app')
        .controller('PurchaseReturn_ReportController', PurchaseReturn_ReportController)

    PurchaseReturn_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function PurchaseReturn_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {      
        $scope.currentPage = 1;
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
      //  $scope.imgname = logopath;
        $scope.tablediv = false;
        $scope.searchValue = '';
        $scope.report = false;
        $scope.report_Print = false;
        $scope.obj.AllD = true;    
       $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.onpageload = function () {
            var pageid = 1;
            apiService.getURI("PurchaseReturn_Report/getdataReport", pageid).
                then(function (promise) {
                    $scope.grneTLIst = promise.grnetlIst;                             
                    $scope.transrows = promise.grnetlIst;
                })
        }
        $scope.submitted = false;
        $scope.savejob = function () {
            $scope.submitted = true;
            var CheckValues;
            if ($scope.obj.EmpD === true) {
                CheckValues ="Approved";
            }
            if ($scope.obj.AllD === true) {
                CheckValues = "ALL";                
            }
            if ($scope.obj.NotappD === true)
                {

                CheckValues = "NotApproved";
                }
            if ($scope.myForm.$valid) {
                $scope.sectionselect = [];

                angular.forEach($scope.grneTLIst, function (itm) {
                    if (itm.selected) {
                        $scope.sectionselect.push({
                            invmgrnreT_Id: itm.invmgrnreT_Id
                        });
                    }
                });


                var data = {

                    "From_Date": new Date($scope.obj.startdate).toDateString(),
                    "To_Date": new Date($scope.obj.enddate).toDateString(),
                    //dd-MM-yyyy
                    "INVMGRNRET_Id_Filter": $scope.sectionselect,
                    "CheckValues": CheckValues

                }
                apiService.create("PurchaseReturn_Report/onreport", data).then(function (promise) {
                    $scope.getReport = promise.getReport;

                    if (promise.getReport.length > 0) {
                        $scope.report = true;
                        $scope.report_Print = true;
                        $scope.presentCountgrid = $scope.getReport.length;
                        $scope.tablediv = true;
                        //invmgrnretapP_Id   extra

                        $scope.employee = $scope.getReport[0].invmgrnreT_GRNRETNo;

                        $scope.employeeid = [];

                        angular.forEach($scope.getReport, function (dev) {
                            if ($scope.employeeid.length === 0) {
                                $scope.employeeid.push({ invmgrneT_Id: dev.invmgrneT_Id, invmgrnreT_GRNRETNo: dev.invmgrnreT_GRNRETNo, invmI_ItemName: dev.invmI_ItemName, invmgrnreT_TotalAmount: dev.invmgrnreT_TotalAmount, invtgrnreT_ReturnQty: dev.invtgrnreT_ReturnQty, invtgrnreT_ReturnAmount: dev.invtgrnreT_ReturnAmount, invmgrnreT_Remarks: dev.invmgrnreT_Remarks, invmgrnretapP_Id: dev.invmgrnretapP_Id });

                             
                            } else if ($scope.employeeid.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.employeeid, function (emp) {
                                    if (emp.invmgrneT_Id === dev.invmgrneT_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.employeeid.push({ invmgrneT_Id: dev.invmgrneT_Id, invmgrnreT_GRNRETNo: dev.invmgrnreT_GRNRETNo, invmI_ItemName: dev.invmI_ItemName, invmgrnreT_TotalAmount: dev.invmgrnreT_TotalAmount, invtgrnreT_ReturnQty: dev.invtgrnreT_ReturnQty, invtgrnreT_ReturnAmount: dev.invtgrnreT_ReturnAmount, invmgrnreT_Remarks: dev.invmgrnreT_Remarks, invmgrnretapP_Id: dev.invmgrnretapP_Id });
                                }
                            }
                        });


                        console.log($scope.employeeid);

                        angular.forEach($scope.employeeid, function (ddd) {
                            $scope.templist = [];
                            angular.forEach($scope.getReport, function (dd) {
                                if (dd.invmgrneT_Id === ddd.invmgrneT_Id) {
                                    $scope.templist.push(dd);
                                }
                            });
                            ddd.plannerdetails = $scope.templist;
                        });


                        console.log($scope.employeeid);



                    }
                    else {
                        swal("Record Not Found....!!");
                        $state.reload();
                    }

                });
            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.isOptionsRequired1 = function () {
            return !$scope.grneTLIst.some(function (options) {
                return options.selected;
            });
        };

        $scope.Empdeviation = function () {
            if ($scope.obj.EmpD === true) {
                $scope.obj.NotappD = false;
                $scope.obj.AllD = false;
                
            }
        };

        $scope.chkalls = function () {
            if ($scope.obj.AllD === true) {
                $scope.obj.NotappD = false;
               
                $scope.obj.EmpD = false;
            }
        };
        $scope.chkallNot = function () {
            if ($scope.obj.NotappD === true) {
                $scope.obj.AllD = false;
                $scope.obj.EmpD = false;
            }
        };


     
        $scope.exportToExcel = function () {
            if ($scope.getReport.length > 0) {
                var exportHref = Excel.tableToExcel(printds, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
        };
        $scope.printData = function () {
            if ($scope.getReport.length > 0) {

                var innerContents = document.getElementById("printds").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                
             //   $scope.headerimg = true;
                popupWinindow.document.close();
            }
        };
        
        //----tada-----//
        $scope.addro = false;
        $scope.obj = {};
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        if ($scope.transrows.length === 1) {
            $scope.cnt = 1;
        }
        $scope.submitted = false;
        $scope.addprrows = function () {
           
           
                if ($scope.transrows.length > 1) {
                    for (var i = 0; i == $scope.transrows.length; i++) {
                        var id = $scope.transrows[i].itrS_Id;
                        var lastChar = id.substr(id.length - 1);
                        $scope.cnt = parseInt(lastChar);
                    }
                }
                $scope.cnt = $scope.cnt + 1;
                $scope.tet = 'trans' + $scope.cnt;
                var newItemNo = $scope.cnt;
                $scope.transrows.push({ 'itrS_Id': 'trans' + newItemNo });
            
           

        };

        $scope.removeprrows = function (index) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);
        //   $scope.Deleteprrows(data);
          
            
        };
        $scope.countitemAmt = function (probj, items) {
            var a = 0;
            $scope.purrate = parseFloat(probj.invtpR_ApproxAmount);
           
            probj.invtpR_ApproxAmount = $scope.purrate;
            angular.forEach($scope.transrows, function (obj) {
                a += parseFloat(obj.invtpR_ApproxAmount);
            })
            var totamt = a;
            $scope.invmpR_ApproxTotAmount = totamt;
            $scope.invmpR_ApproxTotAmount = parseFloat($scope.invmpR_ApproxTotAmount);
            $scope.invmpR_ApproxTotAmount = $scope.invmpR_ApproxTotAmount.toFixed(2);
        }
        //----tada-----//
       
        $scope.Clearid = function () {
           
            $scope.submitted = false;
            $state.reload();
            $scope.tablediv = false;
           
        }


       








    }
})();

