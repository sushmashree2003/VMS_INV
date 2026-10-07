(function () {
    'use strict';
    angular.module('app').controller('FMS_Dept_LetterHeadTrackingController', FMS_Dept_LetterHeadTrackingController);

    FMS_Dept_LetterHeadTrackingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'Excel', '$timeout'];
    function FMS_Dept_LetterHeadTrackingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, Excel, $timeout) {

        $scope.editflag = false;
        $scope.FMSDLHT_CancelledFlag = false;
        $scope.obj.search = "";
        var paginationformasters = 0;
        var copty = "";

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
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
            apiService.getURI("FMS_Dept_Letter_HeadMapping/TrackingLoadData", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.getdepartment = promise.getdepartment;
                    $scope.getinstituion = promise.getinstitution;
                    $scope.MI_Id = promise.mI_Id;
                    $scope.UserId = promise.userId;
                    $scope.getdetails = promise.getusedletterheads;
                    $scope.getinstitutiondetails = promise.getinstitutiondetails;
                    $scope.mi_name = $scope.getinstitutiondetails[0].mI_Name;
                    $scope.imagenew = $scope.getinstitutiondetails[0].mI_Logo;
                    
                }
            });
        };

        $scope.OnChangeInstiution = function () {
            $scope.getdepartment = [];
            $scope.getinstitutiondetails = [];
            $scope.getdetails = [];
            $scope.HRMD_Id = "";
            $scope.FMSDLH_LetterheadsFromNo = "";
            $scope.FMSDLH_LetterheadsToNo = "";
            $scope.FMSDLH_ReceivedBy = "";
            $scope.FMSDLH_ReceivedDate = null;
            $scope.submitted = false;
            $scope.obj.search = "";

            var data = {
                "MI_Id": $scope.MI_Id
            };

            apiService.create("FMS_Dept_Letter_HeadMapping/TrackingOnChangeInstiution", data).then(function (promise) {
                if (promise !== null) {
                    $scope.UserId = promise.userId;
                    $scope.getdepartment = promise.getdepartment;
                    $scope.getdetails = promise.getusedletterheads;
                    $scope.getinstitutiondetails = promise.getinstitutiondetails;
                    $scope.mi_name = $scope.getinstitutiondetails[0].mI_Name;
                    $scope.imagenew = $scope.getinstitutiondetails[0].mI_Logo;
                }
            });
        };

        $scope.OnChangeDept = function () {
            $scope.FMSDLH_LetterheadsFromNo = "";
            $scope.FMSDLH_LetterheadsToNo = "";
            $scope.FMSDLH_ReceivedBy = "";
            $scope.FMSDLH_ReceivedDate = null;
            $scope.submitted = false;
            $scope.obj.search = "";
            $scope.getdetails = [];
            var data = {
                "MI_Id": $scope.MI_Id,
                "HRMD_Id": $scope.HRMD_Id
            };

            apiService.create("FMS_Dept_Letter_HeadMapping/TrackingOnChangeDept", data).then(function (promise) {
                if (promise !== null) {
                    $scope.UserId = promise.userId;
                    $scope.getdeptletterheadlist = promise.getdeptletterheadlist;
                    $scope.getdetails = promise.getusedletterheads;
                    if ($scope.getdeptletterheadlist === null || $scope.getdeptletterheadlist.length === 0) {
                        swal("No Letter Head Mapped For This Department");
                    }
                }
            });
        };

        $scope.SaveLetterHead = function () {
            if ($scope.myForm.$valid) {
                var id = 0;
                if ($scope.FMSDLHT_Id !== undefined && $scope.FMSDLHT_Id !== null && $scope.FMSDLHT_Id !== "") {
                    id = $scope.FMSDLHT_Id;
                }
                var data = {
                    "FMSDLHT_Id": id,
                    "MI_Id": $scope.MI_Id,
                    "HRMD_Id": $scope.HRMD_Id,
                    "FMSDLHT_LetterheadNo": $scope.FMSDLHT_LetterheadNo,
                    "FMSDLHT_CancelledFlag": $scope.FMSDLHT_CancelledFlag,
                    "FMSDLHT_Reason": $scope.FMSDLHT_Reason,
                    "FMSDLHT_Date": new Date($scope.FMSDLHT_Date).toDateString()
                };
                apiService.create("FMS_Dept_Letter_HeadMapping/TrackingOnSaveData", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Sucessfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.message === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Sucessfully");
                            } else {
                                swal("Failed To Updae Record");
                            }
                        } else if (promise.message === "Error") {
                            swal("Failed To Save/Update Record");
                        } else {
                            swal(promise.message);
                        }
                        $scope.cancel();
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.EditLetterHead = function (dd) {

            var data = {
                "FMSDLHT_Id": dd.fmsdlhT_Id,
                "MI_Id": dd.mI_Id
            };

            apiService.create("FMS_Dept_Letter_HeadMapping/TrackingEditLetterHead", data).then(function (promise) {
                if (promise !== null) {
                    $scope.geteditdetails = promise.trackingeditdata;
                    $scope.editflag = true;
                    $scope.MI_Id = $scope.geteditdetails[0].mI_Id;
                    $scope.HRMD_Id = $scope.geteditdetails[0].hrmD_Id;
                    $scope.FMSDLHT_LetterheadNo = $scope.geteditdetails[0].fmsdlhT_LetterheadNo;
                    $scope.FMSDLHT_Date = new Date($scope.geteditdetails[0].fmsdlhT_Date);
                    $scope.FMSDLHT_CancelledFlag = $scope.geteditdetails[0].fmsdlhT_CancelledFlag;
                    $scope.FMSDLHT_Reason = $scope.geteditdetails[0].fmsdlhT_Reason;
                    $scope.FMSDLHT_Id = $scope.geteditdetails[0].fmsdlhT_Id;
                }
            });
        };

        $scope.OnDelete = function (dd) {
            var data = {
                "FMSDLH_Id": dd.fmsdlH_Id,
                "MI_Id": dd.mI_Id
            };
            swal({
                title: "Are You Sure",
                text: "Do You Want To Delete This Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,Delete It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("FMS_Dept_Letter_HeadMapping/TrackingOnDelete", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record Deleted Successfully");
                            } else {
                                swal("Failed To Delete Record");
                            }
                            $scope.cancel();
                        });
                    }
                    else {
                        swal("Cancelled");
                    }
                });
        };

        $scope.interacted = function () {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $scope.MI_Id = "";
            $scope.HRMD_Id = "";
            $scope.FMSDLHT_LetterheadNo = "";
            $scope.FMSDLHT_CancelledFlag = false;
            $scope.FMSDLHT_Reason = "";
            $scope.FMSDLHT_Id = "";
            $scope.submitted = false;
            $scope.getdetails = [];
            $scope.getinstitutiondetails = [];
            $scope.editflag = false;
            $scope.obj.search = "";
            $scope.FMSDLHT_Date = new Date($scope.today);
            $scope.dateget = new Date($scope.today);
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