(function () {
    'use strict';

    angular
        .module('app')
        .controller('COEMonth', COEMonth);

    COEMonth.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']

    function COEMonth($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {
        

        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (configsettings.length > 0) {
            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
            receiptformat = configsettings[0].fmC_Receipt_Format;
            mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//newly Added
        }
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;





        $scope.loadData = function () {

            debugger;
            var id = 2;

            apiService.getURI("COEReport/", id).
                then(function (promise) {


                    $scope.fillmonth = promise.fillmonth;
                    $scope.fillyear = promise.fillyear;

                })
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clear_Details = function () {
            $state.reload();
            $scope.loaddata();
        }

        $scope.user_check = function () {
            if ($scope.user_check == 1) {
                $scope.userl = 1;
            }
            else {
                $scope.userl = 0;
            }
        }



        $scope.ShowReportdata = function () {


            if ($scope.myForm.$valid) {
                
                //$scope.from_date = new Date($scope.FMCB_fromDATE).toDateString();
                //$scope.to_date = new Date($scope.FMCB_toDATE).toDateString();


                var data = {
                    
                    "year": $scope.year,
                    "month": $scope.month
                    //"type": $scope.usercheck

                }




                apiService.create("COEReport/mothreport", data).
            then(function (promise) {
                
                if (promise.count > 0 && promise.count != null) {
                    $scope.monthmodelvalue = $scope.from_date;
                    $scope.report = true;
                    $scope.showGrafh = true;
                    $scope.totalCount = promise.count;
                    $scope.emailCount = promise.eventDesc;
                    $scope.smsCount = promise.eventName;

                    //var monthNames = ["January", "February", "March", "April", "May", "June",
                    //             "July", "August", "September", "October", "November", "December"
                    //];
                    //var d = $scope.FMCB_fromDATE;
                    //var month = monthNames[d.getMonth()]

                    angular.forEach($scope.fillmonth, function (dd) {

                        if (dd.monthid == $scope.month) {
                            $scope.month = dd.monthname;
                        }
                    })
                   // $scope.month = month;
                    $scope.designation = "Implimentation Engineer";
                    $scope.today = new Date();
                    // $scope.acayearnow = $scope.acayear;
                    $scope.report = true;
                    // $scope.export_flag = true;

                    //var chart = new CanvasJS.Chart("rangeBarChat", {
                    //    title: {
                    //        text: "My First Chart in CanvasJS",

                    //    },

                    //    data: [
                    //    {
                    //        // Change type to "doughnut", "line", "splineArea", etc.
                    //        type: "column",
                    //        dataPoints: [
                    //            { label: "BirthDayCount", y: $scope.totalCount },
                    //            { label: "SMS Count", y: parseInt ($scope.smsCount) },
                    //            { label: "Email Count", y: parseInt($scope.emailCount) },
                    //        ]
                    //    }
                    //    ]
                    //});
                    //chart.render();

                    //$scope.acdyr = "";
                    //angular.forEach($scope.acayyearbind, function (opq) {
                    //    $scope.acdyr += opq.asmaY_Year + " ";
                    //})


                    var chart = new CanvasJS.Chart("rangeBarChat");

                    chart.options.axisX = { interval: 1, labelFontSize: 12 };
                    chart.options.axisY = { labelFontSize: 12 };
                    //// chart.options.title = { text: "Fruits sold in First & Second Quarter" };
                    chart.options.height = 260;
                    chart.options.width = 1000;

                    var series1 = { //dataSeries - first quarter
                        type: "column",
                        name: "Count",
                        showInLegend: true,
                        dataPoints: [
                      { y: $scope.totalCount, label: "COE Count" },
                      { y: parseInt($scope.smsCount), label: "SMS Count" },
                      { y: parseInt($scope.emailCount), label: "EMAIL Count" }
                        ]
                    };



                    chart.options.data = [];
                    chart.options.data.push(series1);


                    chart.render();
                    // 


                    //chart.print();

                    // }
                    // end deepak chart

                    $scope.exportToExcel = function () {
                        
                        if (promise.count > 0 && promise.count != null) {
                            var exportHref = Excel.tableToExcel(tablegrp, 'sheet name');
                            $timeout(function () { location.href = exportHref; }, 100);
                        }
                    }

                    $scope.printData = function () {
                        
                        if (promise.count > 0 && promise.count != null) {
                            var base64Image = chart.canvas.toDataURL();
                            document.getElementById('rangeBarChat').style.display = 'none';
                            document.getElementById('chartImage').src = base64Image;
                            var innerContents = document.getElementById("tablegrp").innerHTML;
                            var popupWinindow = window.open('');
                            popupWinindow.document.open();
                            popupWinindow.document.write('<html><head>' +
               '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
          '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');

                            popupWinindow.document.close();

                            //                  var popupWinindow1 = window.open('');
                            //                  popupWinindow1.document.open();
                            //                  popupWinindow1.document.write('<html><head>' +
                            //     '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                            //      '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                            //'<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                            //  '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContentsGraph + '</html>');
                            //                  popupWinindow1.document.close();



                        }
                    }


                }
                else {
                    swal("Record Not Found");
                }
            })


            }
            else {
                $scope.submitted = true;
            }
        }
    }
})();
