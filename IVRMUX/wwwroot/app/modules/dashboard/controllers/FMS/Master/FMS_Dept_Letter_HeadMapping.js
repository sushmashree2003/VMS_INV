(function () {
    'use strict';
    angular.module('app').controller('FMS_Dept_Letter_HeadMappingController', FMS_Dept_Letter_HeadMappingController);

    FMS_Dept_Letter_HeadMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'Excel','$timeout'];
    function FMS_Dept_Letter_HeadMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, Excel, $timeout) {

        $scope.editflag = false;
        $scope.dateget = new Date();
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

        $scope.LoadData = function () {
            var pageid = 2;
            apiService.getURI("FMS_Dept_Letter_HeadMapping/LoadData", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.getdepartment = promise.getdepartment;
                    $scope.getinstituion = promise.getinstitution;
                    $scope.MI_Id = promise.mI_Id;
                    $scope.getdetails = promise.getdetails;
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

            apiService.create("FMS_Dept_Letter_HeadMapping/OnChangeInstiution", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getdepartment = promise.getdepartment;
                    $scope.getdetails = promise.getdetails;
                    $scope.getinstitutiondetails = promise.getinstitutiondetails;
                    $scope.mi_name = $scope.getinstitutiondetails[0].mI_Name;
                    $scope.imagenew = $scope.getinstitutiondetails[0].mI_Logo;
                }
            });
        };

        $scope.SaveLetterHead = function () {
            if ($scope.myForm.$valid) {
                var id = 0;
                if ($scope.FMSDLH_Id !== undefined && $scope.FMSDLH_Id !== null && $scope.FMSDLH_Id !== "") {
                    id = $scope.FMSDLH_Id;
                }
                var data = {
                    "FMSDLH_Id": id,
                    "MI_Id": $scope.MI_Id,
                    "HRMD_Id": $scope.HRMD_Id,
                    "FMSDLH_LetterheadsFromNo": $scope.FMSDLH_LetterheadsFromNo,
                    "FMSDLH_LetterheadsToNo": $scope.FMSDLH_LetterheadsToNo,
                    "FMSDLH_ReceivedBy": $scope.FMSDLH_ReceivedBy,
                    "FMSDLH_ReceivedDate": new Date($scope.FMSDLH_ReceivedDate).toDateString()
                };
                apiService.create("FMS_Dept_Letter_HeadMapping/OnSaveData", data).then(function (promise) {
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
                            swal("Something Went Wrong Contact Administrator");
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
                "FMSDLH_Id": dd.fmsdlH_Id,
                "MI_Id": dd.mI_Id
            };

            apiService.create("FMS_Dept_Letter_HeadMapping/EditLetterHead", data).then(function (promise) {
                if (promise !== null) {
                    $scope.geteditdetails = promise.geteditdetails;
                    $scope.editflag = true;
                    $scope.FMSDLH_LetterheadsFromNo = $scope.geteditdetails[0].fmsdlH_LetterheadsFromNo;
                    $scope.FMSDLH_LetterheadsToNo = $scope.geteditdetails[0].fmsdlH_LetterheadsToNo;
                    $scope.FMSDLH_ReceivedBy = $scope.geteditdetails[0].fmsdlH_ReceivedBy;
                    $scope.FMSDLH_ReceivedDate = new Date($scope.geteditdetails[0].fmsdlH_ReceivedDate);
                    $scope.FMSDLH_Id = $scope.geteditdetails[0].fmsdlH_Id;
                    $scope.HRMD_Id = $scope.geteditdetails[0].hrmD_Id;
                    $scope.MI_Id = $scope.geteditdetails[0].mI_Id;
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
                        apiService.create("FMS_Dept_Letter_HeadMapping/OnDelete", data).then(function (promise) {
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
            $scope.FMSDLH_LetterheadsFromNo = "";
            $scope.FMSDLH_LetterheadsToNo = "";
            $scope.FMSDLH_ReceivedBy = "";
            $scope.FMSDLH_ReceivedDate = null;
            $scope.submitted = false;
            $scope.getdetails = [];
            $scope.getinstitutiondetails = [];
            $scope.editflag = false;
            $scope.obj.search = "";
            $scope.dateget = new Date();
            $scope.LoadData();

        };

        $scope.obj.search = '';
        $scope.filterValue1 = function (obj) {
            return ($filter('date')(obj.fmsdlH_ReceivedDate, 'dd/MM/yyyy').indexOf($scope.obj.search) >= 0) ||
                (angular.lowercase(obj.hrmD_DepartmentName)).indexOf(angular.lowercase($scope.obj.search)) >= 0 ||
                (angular.lowercase(obj.fmsdlH_ReceivedBy)).indexOf(angular.lowercase($scope.obj.search)) >= 0 ||
                (angular.lowercase(obj.fmsdlH_LetterheadsFromNo)).indexOf(angular.lowercase($scope.obj.search)) >= 0 ||
                (angular.lowercase(obj.fmsdlH_LetterheadsToNo)).indexOf(angular.lowercase($scope.obj.search)) >= 0;
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