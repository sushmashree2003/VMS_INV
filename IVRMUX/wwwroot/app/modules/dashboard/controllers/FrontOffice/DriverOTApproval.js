
(function () {
    'use strict';
    angular
    .module('app')
    .controller('DriverOTApprovalController', DriverOTApprovalController)

    DriverOTApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache', '$filter', 'uiGridConstants', 'appSettings', '$sce']
    function DriverOTApprovalController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache, $filter, $uiGridConstants, appSettings, $sce) {

        $scope.itemsPerPage = 5;
        $scope.currentPage = 1;

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.hrmE_EmployeeFirstName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrmL_LeaveName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrelT_Status)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        };

        $scope.griddriverotstatus = {
            enableCellEditOnFocus: true,
            enableRowSelection: true,
            enableSelectAll: true,
            enableFiltering: true,
            enableCellEdit: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 10,

            columnDefs: [                
                { name: 'HRME_EmployeeFirstName', displayName: 'Driver Name' },
                { name: 'VMSDT_Punchdate', displayName: 'Working Date', enableCellEdit: false },
                { name: 'VMSDT_PunchIn', displayName: 'In Time', enableCellEdit: false },
                { name: 'VMSDT_PunchOut', displayName: 'Out Time', enableCellEdit: false },
                { name: 'VMSDT_TotalOt', displayName: 'OT Time', enableFiltering: false },
                { name: 'VMSDT_Remarks', displayName: 'Remark', enableFiltering: false }
            ]
        };

        $scope.loadData = function () {
            var id = 2;
            apiService.getURI("DriverOT/getApprovalStatus", id).
                then(function (promise) {
                    $scope.griddriverotstatus.data = promise.otapprovallist;
                    $scope.activityIds = promise.reportdata;
                    angular.forEach($scope.griddriverotstatus.data, function (grdobj) {
                        var fdate = grdobj.VMSDT_Punchdate.split('T');
                        grdobj.VMSDT_Punchdate = fdate[0];
                    });

                    angular.forEach($scope.activityIds, function (obj) {
                        var fdate = obj.vmsdT_Punchdate.split('T');
                        obj.vmsdT_Punchdate = fdate[0];
                    });

                    if (promise.otapprovallist.count === 0) {
                        swal("No Data Available for Approval !!");
                        $scope.ckdept = false;
                    }
                });
        };
       
        $scope.griddriverotstatus.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;
            gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                $scope.rowsSelected = $scope.gridApi.selection.getSelectedRows();
            });
            gridApi.selection.on.rowSelectionChangedBatch($scope, function (row) {
                $scope.rowsSelected = $scope.gridApi.selection.getSelectedRows();
            });
        };
       
        $scope.get_status = function () {            
            if ($scope.myForm.$valid) {
                var data = {
                    get_OT_status: $scope.gridApi.selection.getSelectedRows(),
                    "VSMDTA_Remarks": $scope.remarkstxta
                };
                apiService.create("DriverOT/get_status", data).
                    then(function (promise) {
                        if (promise.returnvalue == "Success") {
                            swal("Approved...");
                            $state.reload();
                        }
                        else {
                            swal("Something went wrong!!");
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.reject_status = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    get_OT_status: $scope.gridApi.selection.getSelectedRows(),
                    "VSMDTA_Remarks": $scope.remarkstxta
                };
                apiService.create("DriverOT/reject_status", data).
                    then(function (promise) {
                        if (promise.returnvalue == "Success") {
                            swal("Rejected...");
                            $state.reload();
                        }
                        else {
                            swal("Something went wrong!!");
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

        $scope.fromdate = function (d1, row) {
            $scope.From_Date = d1;
            angular.forEach($scope.griddriverotstatus.data, function (grdobj) {
                if (grdobj.hrelaP_Id === row.entity.hrelaP_Id) {
                    grdobj.toDate = d1;
                }
                if (grdobj.hrelS_CBLeaves !== 0) {
                    grdobj.hrelaP_TotalDays = 1;
                }
            });

            $scope.tominDate = new Date(
                $scope.From_Date.getFullYear(),
                $scope.From_Date.getMonth(),
                $scope.From_Date.getDate());
            $scope.toDate = $scope.tominDate;

            $scope.minDate = new Date(
                $scope.toDate.getFullYear(),
                $scope.toDate.getMonth(),
                $scope.toDate.getDate() + 1);
            $scope.maxDate = new Date(
                $scope.toDate.getFullYear(),
                $scope.toDate.getMonth(),
                $scope.toDate.getDate() + 1);
            angular.forEach($scope.griddriverotstatus.data, function (obj) {
                if (obj.hrelaP_Id === row.entity.hrelaP_Id) {
                    obj.hrelaP_ReportingDate = $scope.minDate;
                }
            });
            //$scope.HRELAP_ReportingDate = $scope.minDate;
        };

        $scope.todate = function (secondDate1, row) {
            $scope.ReportingDate = secondDate1;
            var firstDate1 = $filter('date')(row.entity.fromDate, "dd/MM/yy");
            secondDate1 = $filter('date')(secondDate1, "dd/MM/yy");
            var date2 = new Date($scope.formatString(secondDate1));
            var date1 = new Date($scope.formatString(firstDate1));
            var timeDiff = Math.abs(date2.getTime() - date1.getTime());
            $scope.dayDifference = Math.ceil(timeDiff / (1000 * 3600 * 24));
            angular.forEach($scope.griddriverotstatus.data, function (obj) {
                if (obj.hrelaP_Id === row.entity.hrelaP_Id) {
                    obj.hrelaP_TotalDays = $scope.dayDifference + 1;
                }
            });

            $scope.minDate = new Date(
                $scope.ReportingDate.getFullYear(),
                $scope.ReportingDate.getMonth(),
                $scope.ReportingDate.getDate() + 1);
            $scope.maxDate = new Date(
                $scope.ReportingDate.getFullYear(),
                $scope.ReportingDate.getMonth(),
                $scope.ReportingDate.getDate() + 1);
            angular.forEach($scope.griddriverotstatus.data, function (obj) {
                if (obj.hrelaP_Id === row.entity.hrelaP_Id) {
                    obj.hrelaP_ReportingDate = $scope.minDate;
                }
            });
            //$scope.hrelaP_ReportingDate = $scope.minDate;
        };

        $scope.formatString = function (format) {
            var day = parseInt(format.substring(0, 2));
            var month = parseInt(format.substring(3, 5));
            var year = parseInt(format.substring(6, 10));
            var date = new Date(year, month - 1, day);
            return date;
        };

        $scope.viewdocument = function (obj) {
            if (obj != null) {
                var img = obj.HRELAP_SupportingDocument;
                var imagarr = img.split('.');
                var extention = imagarr[imagarr.length - 1];
                obj.filetype = extention;
                if (extention === 'doc' || extention === 'docx' || extention === 'ppt' || extention === 'pptx' || extention === 'xlsx' || extention === 'xls') {
                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + obj.HRELAP_SupportingDocument;
                }
                else if (extention === "jpg" || extention === "jpeg" || extention === "JPEG") {
                    $('#preview').removeAttr('src');
                    $('#preview').attr('src', obj.HRELAP_SupportingDocument);
                    $('#myModal').modal();
                    $('#myModal').modal({ keyboard: false });
                    $('#myModal').modal('show');
                }
                else if (extention === "pdf") {
                    var imagedownload = obj.HRELAP_SupportingDocument;
                    $scope.content = "";
                    var fileURL = "";
                    var file = "";
                    $http.get(imagedownload, { responseType: 'arraybuffer' })
                        .success(function (response) {
                            file = new Blob([(response)], { type: 'application/pdf' });
                            fileURL = URL.createObjectURL(file);
                            $scope.content = $sce.trustAsResourceUrl(fileURL);
                            $('#showpdf').modal('show');
                        });
                }
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };
    }
})();