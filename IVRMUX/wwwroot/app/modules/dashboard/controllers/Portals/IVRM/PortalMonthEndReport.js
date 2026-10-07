(function () {
    'use strict';
    angular
        .module('app')
        .controller('PortalMonthEndReportController', PortalMonthEndReportController);
    PortalMonthEndReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter'];
    function PortalMonthEndReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {

        $scope.imgdiv = false;
        $scope.imgdivMA = false;
        $scope.imgdivK = false;
        $scope.headerimg = false;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        var temp = [];
        var year = "";
        $scope.clgloaddata = function () {
            var pageid = 2;
            apiService.getURI("PortalMonthEndReport/getloaddata", pageid).
                then(function (promise) {
                    $scope.acayyearbind = promise.acayear;
                    $scope.monthlist = promise.month_array;
                });
        };
        //==================================================== ON Module Radio CHANGE
        $scope.modulechange = function () {
            $scope.ASMAY_Id = '';
            $scope.report = '';
            $scope.IVRM_Month_Id = '';
            $scope.totalCount = '';
            $scope.smsCount = '';
            $scope.emailCount = '';
            $scope.IVRM_Month_Id = '';
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
            $scope.get_monthendreport = "";

            //$scope.monthlist_temp = [];
            //temp = [];
            //$scope.month_name = "";
            //$scope.monthmodel = "";
            //$scope.yearmodel = "";
            //$scope.monthiddd = "";
            //$scope.report = false;
            //$scope.clgloaddata();
        };
        //==================================================== ON YEAR CHANGE
        //$scope.get_years = function () {
        //    $scope.month_name = [];
        //    $scope.monthmodel = "";
        //    $scope.yearmodel = "";
        //    $scope.monthiddd = "";
        //    temp = [];
        //    angular.forEach($scope.acayyearbind, function (itm) {
        //        if (itm.asmaY_Id === parseInt($scope.academicyr)) {
        //            year = itm.asmaY_Year;
        //        }
        //    });
        //    var s1 = year.substring(0, 4);
        //    var s2 = year.substring(year.length, 5);
        //    temp.push({ asmaY_Id: 0, asmaY_Year: s1 });
        //    temp.push({ asmaY_Id: 1, asmaY_Year: s2 });
        //    // $scope.years = temp;

        //    angular.forEach($scope.acayyearbind, function (itm) {
        //        if (itm.asmaY_Id === parseInt($scope.academicyr)) {
        //            $scope.yearfromdate = itm.asmaY_From_Date;
        //        }
        //    });

        //    $scope.asmaYFromDate = $scope.yearfromdate;

        //    var date = new Date($scope.asmaYFromDate);

        //    $scope.monthNames = ["January", "February", "March", "April", "May", "June",
        //        "July", "August", "September", "October", "November", "December"
        //    ];
        //    $scope.months = [];
        //    $scope.year = [];

        //    for (var i = 0; i < 12; i++) {
        //        $scope.months.push($scope.monthNames[date.getMonth()]);
        //        //$scope.year.push(date.getFullYear());
        //        // Add a month each time
        //        date.setMonth(date.getMonth() + 1);
        //    }
        //    $scope.monthByOrder = [];
        //    for (var i = 0; i < $scope.months.length; i++) {
        //        name = $scope.months[i];
        //        for (var j = 0; j < $scope.monthlist_temp.length; j++) {
        //            var monthiddd = $scope.monthlist_temp[j].ivrM_Month_Id;
        //            if (name.toLowerCase() == $scope.monthlist_temp[j].ivrM_Month_Name.toLowerCase()) {
        //                if (i == 0) {
        //                    $scope.monthiddd = $scope.monthlist_temp[j].ivrM_Month_Id;
        //                }
        //                $scope.monthByOrder.push($scope.monthlist_temp[j]);
        //            }
        //        }
        //    }
        //    $scope.monthList = $scope.monthByOrder;
        //    $scope.month_name = $scope.monthByOrder;
        //}

        $scope.onselectYear = function () {
            temp = [];
            angular.forEach($scope.acayyearbind, function (itm) {
                if (itm.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                    year = itm.asmaY_Year;
                }
            });
            var s1 = year.substring(0, 4);
            var s2 = year.substring(year.length, 5);
            temp.push({ asmaY_Id: 0, asmaY_Year: s1 });
            temp.push({ asmaY_Id: 1, asmaY_Year: s2 });
            $scope.years = temp;
        };
        $scope.get_monthendreport = '';
        $scope.submitted = false;
        //==================================================== ON MONTH CHANGE     

        //$scope.get_month = function () {
        //    temp = [];
        //    angular.forEach($scope.acayyearbind, function (mm) {
        //        if (mm.asmaY_Id === parseInt($scope.academicyr)) {

        //            var frommonth = (new Date(mm.asmaY_From_Date)).getMonth() + 1;
        //            var tomonth = (new Date(mm.asmaY_To_Date)).getMonth() + 1;
        //            var fromyear = (new Date(mm.asmaY_From_Date)).getFullYear();
        //            var tomonth = (new Date(mm.asmaY_To_Date)).getFullYear();

        //            if ($scope.monthmodel >= $scope.monthiddd && $scope.monthmodel <= 12) {
        //                var year1 = mm.asmaY_Year;

        //                var s1 = year1.substring(0, 4);
        //                var s2 = year1.substring(year1.length, 5);
        //                temp.push({ asmaY_Id: 0, asmaY_Year: s1 });
        //                //temp.push({ asmaY_Id: 1, asmaY_Year: s2 });
        //                $scope.years = temp;

        //            } else {
        //                var year12 = mm.asmaY_Year;

        //                var s1 = year12.substring(0, 4);
        //                var s2 = year12.substring(year12.length, 5);
        //                temp.push({ asmaY_Id: 1, asmaY_Year: s2 });
        //                //temp.push({ asmaY_Id: 1, asmaY_Year: s2 });
        //                $scope.years = temp;
        //            }
        //        }
        //    });
        //};
        //==================================================== REPORT DATA
        $scope.showReportdata = function () {

            if ($scope.myForm.$valid) {
                $scope.iename = $scope.iename;
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "month": $scope.IVRM_Month_Id,
                    "year": $scope.yearmodel,
                    "roleflag": $scope.roleflag,
                    "Moduleflag": $scope.moduleflag,
                    "countflag": $scope.countflag
                };

                apiService.create("PortalMonthEndReport/getmonthreport", data).
                    then(function (promise) {
                        if (promise.get_monthendreport.length > 0) {
                            $scope.get_monthendreport = promise.get_monthendreport;
                            $scope.report = true;
                            $scope.get_monthendreport = $scope.get_monthendreport;
                            $scope.totalCount = $scope.get_monthendreport[0].totalCount;
                            $scope.emailCount = $scope.get_monthendreport[0].email;
                            $scope.smsCount = $scope.get_monthendreport[0].sms;
                            $scope.designation = "Implementation Engineer";
                            $scope.today = new Date();
                            angular.forEach($scope.month_name, function (itm) {
                                if (itm.ivrM_Month_Id === $scope.monthmodel) {
                                    $scope.monthmodelvalue = itm.ivrM_Month_Name;
                                }
                            });
                            angular.forEach($scope.acayyearbind, function (itm) {
                                if (itm.asmaY_Id === parseInt($scope.academicyr)) {
                                    $scope.acayearnow = itm.asmaY_Year;
                                }
                            });

                            $scope.report = true;
                            if ($scope.moduleflag === "Portal") {

                                var chart = new CanvasJS.Chart("rangeBarChat");
                                chart.options.axisX = { interval: 1, labelFontSize: 12 };
                                chart.options.axisY = { labelFontSize: 12 };
                                chart.options.height = 260;
                                chart.options.width = 1000;
                                var series1 = {
                                    type: "column",
                                    name: "COUNT",
                                    showInLegend: true,
                                    dataPoints: [
                                        { y: parseInt($scope.totalCount), label: "LOGIN COUNT" },
                                        { y: parseInt($scope.smsCount), label: "SMS COUNT" },
                                        { y: parseInt($scope.emailCount), label: "EMAIL COUNT" }
                                    ]
                                };
                                chart.options.data = [];
                                chart.options.data.push(series1);
                                chart.render();

                                $scope.exportToExcel = function () {
                                    if ($scope.get_monthendreport.length > 0) {
                                        var exportHref = Excel.tableToExcel(tablegrp, 'sheet name');
                                        $timeout(function () { location.href = exportHref; }, 100);
                                    }
                                };
                                $scope.printData = function () {

                                    if ($scope.get_monthendreport.length > 0) {
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
                                        $scope.imgdiv = true;
                                        $scope.headerimg = true;
                                        popupWinindow.document.close();
                                    }
                                };
                            }

                            else if ($scope.moduleflag === "MobileApp") {

                                chart = new CanvasJS.Chart("rangeBarChatMA");
                                chart.options.axisX = { interval: 1, labelFontSize: 12 };
                                chart.options.axisY = { labelFontSize: 12 };
                                chart.options.height = 260;
                                chart.options.width = 1000;
                                var series1 = {
                                    type: "column",
                                    name: "Count",
                                    showInLegend: true,
                                    dataPoints: [
                                        { y: parseInt($scope.totalCount), label: "LOGIN COUNT" },
                                        { y: parseInt($scope.smsCount), label: "SMS COUNT" },
                                        { y: parseInt($scope.emailCount), label: "EMAIL COUNT" }
                                    ]
                                };
                                chart.options.data = [];
                                chart.options.data.push(series1);
                                chart.render();

                                $scope.exportToExcel = function () {
                                    if ($scope.get_monthendreport.length > 0) {
                                        var exportHref = Excel.tableToExcel(tablegrpMA, 'sheet name');
                                        $timeout(function () { location.href = exportHref; }, 100);
                                    }
                                }
                                $scope.printData = function () {

                                    if ($scope.get_monthendreport.length > 0) {
                                        var base64Image = chart.canvas.toDataURL();
                                        document.getElementById('rangeBarChatMA').style.display = 'none';
                                        document.getElementById('chartImageMA').src = base64Image;
                                        var innerContents = document.getElementById("tablegrpMA").innerHTML;
                                        var popupWinindow = window.open('');
                                        popupWinindow.document.open();
                                        popupWinindow.document.write('<html><head>' +
                                            '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                                            '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                                            '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                                            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                                        $scope.imgdivMA = true;
                                        $scope.headerimg = true;
                                        popupWinindow.document.close();
                                    }
                                };
                            }

                            else if ($scope.moduleflag === "Kiosk") {

                                var chart = new CanvasJS.Chart("rangeBarChatK");
                                chart.options.axisX = { interval: 1, labelFontSize: 12 };
                                chart.options.axisY = { labelFontSize: 12 };
                                chart.options.height = 260;
                                chart.options.width = 1000;
                                var series1 = {
                                    type: "column",
                                    name: "Count",
                                    showInLegend: true,
                                    dataPoints: [
                                        { y: parseInt($scope.totalCount), label: "LOGIN COUNT" },
                                        { y: parseInt($scope.smsCount), label: "SMS COUNT" },
                                        { y: parseInt($scope.emailCount), label: "EMAIL COUNT" }
                                    ]
                                };
                                chart.options.data = [];
                                chart.options.data.push(series1);
                                chart.render();

                                $scope.exportToExcel = function () {
                                    if ($scope.get_monthendreport.length > 0) {
                                        var exportHref = Excel.tableToExcel(tablegrpK, 'sheet name');
                                        $timeout(function () { location.href = exportHref; }, 100);
                                    }
                                };
                                $scope.printData = function () {

                                    if ($scope.get_monthendreport.length > 0) {
                                        var base64Image = chart.canvas.toDataURL();
                                        document.getElementById('rangeBarChatK').style.display = 'none';
                                        document.getElementById('chartImageK').src = base64Image;
                                        var innerContents = document.getElementById("tablegrpK").innerHTML;
                                        var popupWinindow = window.open('');
                                        popupWinindow.document.open();
                                        popupWinindow.document.write('<html><head>' +
                                            '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                                            '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                                            '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                                            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                                        $scope.imgdivK = true;
                                        $scope.headerimg = true;
                                        popupWinindow.document.close();
                                    }
                                };
                            }

                        }
                        else {
                            swal("Record Not Found....!!");
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.Clear = function () {
            $state.reload();
        };

    }
})();

