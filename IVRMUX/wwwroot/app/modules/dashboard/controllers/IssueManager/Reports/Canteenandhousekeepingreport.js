(function () {
    'use strict';
    angular.module('app').controller('CanteenandhousekeepingreportController', ModuleWiseFeedbackReportController)
    ModuleWiseFeedbackReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'Excel', 'apiService', '$timeout', '$http', 'superCache', '$filter', '$q']
    function ModuleWiseFeedbackReportController($rootScope, $scope, $state, $location, Flash, appSettings, Excel, apiService, $timeout, $http, superCache, $filter, $q) {
        var paginationformasters;
        var copty;
        $scope.obj = {}
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.submitted = false;
        $scope.objques = {};
        $scope.gridflag = false;
        // Master Type
        //$scope.onloadmonths = function () {
        //    $scope.gridflag = true;
        //    var pageid = 2;
        //    apiService.getURI("ModuleWiseFeedbackReport/OnLoadClient", pageid).then(function (promise) {
        //        if (promise !== null) {
        //            $scope.hr_masteryear = promise.hr_masteryear;
        //        } 
        //    });
           
        //};
        $scope.onloadmonths = function () {
            $scope.types = [
                {
                    "type": "Canteen",
                    "value":"Canteen"
                },
                {
                    "type": "House Keeping",
                    "value": "HouseKeeping"
                }
            ]
        };
        $scope.clearayyary = function () {
            $scope.vmsmoduleratingreport = [];
        }

        $scope.getreport = function () {
            if ($scope.myForm.$valid) {

                var data = {
                    "fromdate": new Date($scope.fromdate).toDateString(),
                    "todate": new Date($scope.todate).toDateString(), 
                    "optionflag": $scope.optionflag,
                    "type": $scope.type
                    //"flag": "canteenandhousekeeping"
                };
             
                function formatDate(date) {
                    const day = String(date.getDate()).padStart(2, '0'); // Get day and pad with 0 if needed
                    const month = String(date.getMonth() + 1).padStart(2, '0'); // Get month (0-based) and pad
                    const year = date.getFullYear(); // Get year
                    return `${day}/${month}/${year}`; // Format as dd/mm/yyyy
                }

                function getDatesBetween(startDate, endDate) {
                    const dates = [];
                    const currentDate = new Date(startDate);

                    // Adjust the end date to include it in the loop
                    endDate = new Date(endDate);
                    endDate.setDate(endDate.getDate() + 1); // Include end date

                    while (currentDate < endDate) {
                        dates.push({ date: formatDate(currentDate) }); // Store the formatted date
                        currentDate.setDate(currentDate.getDate() + 1); // Move to the next day
                    }

                    return dates;
                }

                // Example usage
                $scope.betweendates = [];
                const fromDate = new Date($scope.fromdate); // Assuming $scope.fromdate is defined
                const toDate = new Date($scope.todate);     // Assuming $scope.todate is defined
                const dateArray = getDatesBetween(fromDate, toDate);
                $scope.betweendates = dateArray;
                console.log(dateArray); // Logs the array of dates in dd/mm/yyyy format



                apiService.create("ModuleWiseFeedbackReport/getcanteenandhousekeepingfeedback", data).then(function (promise) {
                //apiService.create("ModuleWiseFeedbackReport/getvmsmodulefeedbackreport", data).then(function (promise) {

                    if (promise.canteenfeedbackreport.length > 0 && promise.canteenfeedbackreport != null) {
                        $scope.canteenfeedbackreport = promise.canteenfeedbackreport;

                        if ($scope.optionflag == "Individual") {
                            $scope.feedbackquestion = [];
                            angular.forEach($scope.canteenfeedbackreport, function (Q) {
                                $scope.feedbackquestion.push({ FMQE_Id: Q.FMQE_Id, FMQE_FeedbackQuestions: Q.FMQE_FeedbackQuestions });
                            });
                            var questions = $scope.feedbackquestion;
                            let jsonObject = questions.map(JSON.stringify);
                            let uniqueSet = new Set(jsonObject);
                            let uniqueArray = Array.from(uniqueSet).map(JSON.parse)
                            $scope.feedbackquestion = uniqueArray;
                            console.log($scope.feedbackquestion);
                        }

                    }
                    else {
                        swal("No Records Found For Selected Dates","","warning")
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

        $scope.printData = function () {
            var innerContents = document.getElementById("modulefeedback").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();

            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();

        }
        //$scope.exportToExcel = function (tabel1) {
        //    var exportHref = Excel.tableToExcel(tabel1, 'sheet name');
        //    $timeout(function () { location.href = exportHref; }, 100);
        //}
        $scope.exportToExcel = function (tabel1) {
                var excelnamemain = "CANTEEN_REPORT";
            var printSectionId = '#modulefeedback';
                excelnamemain = excelnamemain + '' + '.xls';
                var exportHref = Excel.tableToExcel(printSectionId, 'Student Register Report');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = excelnamemain;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
            
        };
        $scope.Clear = function () {
            $state.reload();
        };
    }
})();