(function () {
    'use strict';
    angular.module('app').controller('FMS_Letter_Head_ReportController', FMS_Letter_Head_ReportController);

    FMS_Letter_Head_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'Excel', '$timeout'];
    function FMS_Letter_Head_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, Excel, $timeout) {

        $scope.getdetails = [];
        $scope.editflag = false;
        $scope.FMSDLHT_CancelledFlag = false;
        $scope.obj.search = "";
        var paginationformasters = 0;
        var copty = "";

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

        $scope.getserverdate = function () {
            var xmlHttp;
            function srvTime() {
                try {
                    //FF, Opera, Safari, Chrome
                    xmlHttp = new XMLHttpRequest();
                }
                catch (err1) {
                    //IE
                    try {
                        xmlHttp = new ActiveXObject('Msxml2.XMLHTTP');
                    }
                    catch (err2) {
                        try {
                            xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
                        }
                        catch (eerr3) {
                            //AJAX not supported, use CPU time.
                            alert("AJAX not supported");
                        }
                    }
                }
                xmlHttp.open('HEAD', window.location.href.toString(), false);
                xmlHttp.setRequestHeader("Content-Type", "text/html");
                xmlHttp.send('');
                return xmlHttp.getResponseHeader("Date");
            }
            $scope.today = srvTime();
        }
        $scope.getserverdate();

        $scope.dateget = new Date($scope.today);
        $scope.FMSDLHT_Date = new Date($scope.today);

        $scope.LoadData = function () {
            var pageid = 2;
            apiService.getURI("FMS_Dept_Letter_HeadMapping/ReportLoadData", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.getdepartment = promise.getdepartment;
                    $scope.getinstituion = promise.getinstitution;
                    $scope.MI_Id = promise.mI_Id;
                    $scope.getinstitutiondetails = promise.getinstitutiondetails;
                    $scope.mi_name = $scope.getinstitutiondetails[0].mI_Name;
                    $scope.imagenew = $scope.getinstitutiondetails[0].mI_Logo;
                }
            });
        };

        $scope.OnChangeInstiution = function () {
            $scope.getdetails = [];
            var data = {
                "MI_Id": $scope.MI_Id
            };

            apiService.create("FMS_Dept_Letter_HeadMapping/ReportOnChangeInstiution", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getdepartment = promise.getdepartment;
                    $scope.getinstitutiondetails = promise.getinstitutiondetails;
                    $scope.mi_name = $scope.getinstitutiondetails[0].mI_Name;
                    $scope.imagenew = $scope.getinstitutiondetails[0].mI_Logo;
                }
            });
        };

        $scope.OnChangeDept = function () {
            $scope.getdetails = [];
            var data = {
                "MI_Id": $scope.MI_Id,
                "HRMD_Id": $scope.HRMD_Id
            };

            apiService.create("FMS_Dept_Letter_HeadMapping/ReportOnChangeDept", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getdeptletterheadlist = promise.getdeptletterheadlist;

                    if ($scope.getdeptletterheadlist === null || $scope.getdeptletterheadlist.length === 0) {
                        swal("No Letter Head Mapped For This Department");
                    }
                }
            });
        };

        $scope.GetLetterHeadReport = function () {
            if ($scope.myForm.$valid) {
                $scope.getdetails = [];

                var letterhead = $scope.letterhead.split('-');
                var data = {
                    "MI_Id": $scope.MI_Id,
                    "HRMD_Id": $scope.HRMD_Id,
                    "FMSDLH_LetterheadsFromNo": letterhead[0],
                    "FMSDLH_LetterheadsToNo": letterhead[1]
                };

                apiService.create("FMS_Dept_Letter_HeadMapping/GetLetterHeadReport", data).then(function (promise) {
                    if (promise !== null) {

                        $scope.getdetails = promise.getreport;
                        if ($scope.getdetails !== null && $scope.getdetails.length > 0) {
                            angular.forEach($scope.getdepartment, function (dd) {
                                if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {
                                    $scope.deptartmentname = dd.hrmD_DepartmentName;
                                }
                            });
                        } else {
                            swal("No Record Found...!!");
                            $scope.getdetails = [];
                        }                       
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function () {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $scope.MI_Id = "";
            $scope.HRMD_Id = "";
            $scope.letterhead = "";
            $scope.getdeptletterheadlist = [];
            $scope.getdepartment = [];
            $scope.LoadData();
        };

        $scope.obj.search = '';
        $scope.filterValue1 = function (obj) {
            return ($filter('date')(obj.fmsdlhT_Date, 'dd/MM/yyyy').indexOf($scope.obj.search) >= 0) ||
                (angular.lowercase(obj.hrmD_DepartmentName)).indexOf(angular.lowercase($scope.obj.search)) >= 0 ||
                (angular.lowercase(obj.fmsdlhT_LetterheadNo)).indexOf(angular.lowercase($scope.obj.search)) >= 0 ||
                (angular.lowercase(obj.fmsdlhT_Reason)).indexOf(angular.lowercase($scope.obj.search)) >= 0 ||
                (angular.lowercase(obj.userName)).indexOf(angular.lowercase($scope.obj.search)) >= 0;
        };

        $scope.Print = function () {
            var innerContents = document.getElementById("printreport").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        };

        $scope.ExportToExcel = function () {
            var excelname = 'Department Letter Head Report';
            excelname = excelname.toUpperCase() + '.xls';
            var exportHref = Excel.tableToExcel('#exportexcel', 'Department Letter Head Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

    }
})();