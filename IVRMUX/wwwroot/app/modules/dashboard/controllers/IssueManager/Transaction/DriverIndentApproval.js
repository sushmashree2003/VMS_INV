
(function () {
    'use strict';
    angular
.module('app')
        .controller('DriverIndentApprovalController', DriverIndentApprovalController)

    DriverIndentApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache', '$filter', 'uiGridConstants', 'appSettings', '$sce']
    function DriverIndentApprovalController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache, $filter, $uiGridConstants, appSettings, $sce) {

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));

        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }

       
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        //if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
        //    $scope.itemsPerPage = 5;
        //}

       
        $scope.coptyright = copty;

        $scope.searchValue = '';
        $scope.filterValue = function (employee) {
            return (angular.lowercase(obj.hrmE_EmployeeFirstName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrmL_LeaveName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrelT_Status)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        };

        $scope.gridleavestatus = {
            enableCellEditOnFocus: true,
            enableRowSelection: true,
            enableSelectAll: true,
            enableFiltering: true,
            enableCellEdit: false,  
            
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'HRME_EmployeeFirstName', displayName: 'Driver Name', enableCellEdit: true, width: 250 },
                { name: 'TRMV_VehicleName', displayName: 'Vehicle Name', enableCellEdit: true, width: 100 },
                { name: 'TRMV_VehicleNo', displayName: 'Vehicle No', enableCellEdit: true, width: 150 },
                { name: 'ISMDIT_Date', displayName: 'Date', enableCellEdit: false, width:100 },
                { name: 'ISMDIT_Qty', displayName: 'Quantity (Ltr)', enableFiltering: false, width: 100 },
                { name: 'ISMDIT_Amount', displayName: 'Amount', enableFiltering: false, width: 100 },
                { name: 'ISMDIT_OpeningKM', displayName: 'Opening KM', enableFiltering: false, width: 100 },
                { name: 'ISMDIT_ClosingKM', displayName: 'Closing KM', enableFiltering: false, width: 100 },
                { name: 'TotalKM', displayName: 'Total KM', enableFiltering: false, width: 100 },
                { name: 'ISMDIT_Remark', displayName: 'Remark', enableFiltering: false, }
            ]
        };
        //cellTemplate: '<div class="abcd"><span>{{COL_FIELD}}</span></div>' 
        $scope.gridleavestatus.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;
            gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                $scope.rowsSelected = $scope.gridApi.selection.getSelectedRows();
            });
            gridApi.selection.on.rowSelectionChangedBatch($scope, function (row) {
                $scope.rowsSelected = $scope.gridApi.selection.getSelectedRows();
            });
        };


        $scope.loadData = function () {
            var id = 2;
            apiService.getURI("DriverIndent/getApprovalStatus", id).
                then(function (promise) {
                    $scope.gridleavestatus.data = promise.indentApprovalList;
                    angular.forEach($scope.gridleavestatus.data, function (grdobj) {
                        var fdate = grdobj.ISMDIT_Date.split('T');
                        grdobj.ISMDIT_Date = fdate[0];
                    });

                    $scope.activityIds = promise.indentApproved;
                });
        };

        $scope.get_status = function () {
            if ($scope.myForm.$valid) {

                var data = {
                    get_indent_status: $scope.gridApi.selection.getSelectedRows(),
                    "ISMDIT_Remark": $scope.remarkstxta
                };
               
                apiService.create("DriverIndent/get_status", data).
                    then(function (promise) {
                        $scope.gridleavestatus.data = promise.get_leavestatus;
                        swal("Approved...");
                        $state.reload();

                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.reject_status = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    get_indent_status: $scope.gridApi.selection.getSelectedRows(),
                    "ISMDIT_Remark": $scope.remarkstxta
                };

                apiService.create("DriverIndent/reject_status", data).
                    then(function (promise) {
                        $scope.gridleavestatus.data = promise.get_leavestatus;
                        swal("Rejected...");
                        $state.reload();
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
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

        $scope.cancel = function (obj) {
            $state.reload();
        };
    }
})();