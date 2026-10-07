
(function () {
    'use strict';
    angular
.module('app')
        .controller('LaptopAppApprovalController', LaptopAppApprovalController)

    LaptopAppApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache', '$filter', 'uiGridConstants', 'appSettings', '$sce', '$timeout']
    function LaptopAppApprovalController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache, $filter, $uiGridConstants, appSettings, $sce, $timeout) {

        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.hrmE_EmployeeFirstName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrmL_LeaveName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrelT_Status)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        };

        $scope.gridlapstatus = {
            enableCellEditOnFocus: true,
            enableRowSelection: true,
            enableSelectAll: true,
            enableFiltering: true,
            enableCellEdit: false,
            
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'hrmE_EmployeeFirstName', displayName: 'Applicant Name', enableCellEdit: false, width: 250 },
                { name: 'vlapappL_FromDate', displayName: 'From Date', enableCellEdit: false, width:100 },
                { name: 'vlapappL_ToDate', displayName: 'To Date', enableCellEdit: false, width: 100 },
                { name: 'vlapappL_TotalDays', displayName: 'Total Days', enableCellEdit: false, width: 100 },                
                {
                    name: 'vlapappL_SupportingDocument', displayName: 'Document', enableCellEdit: false, width: 120, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-if="row.entity.vlapappL_SupportingDocument != 0" ng-model="row.entity.vlapappL_SupportingDocument" ng-click="grid.appScope.viewdocument(row.entity);" title="View Document"> <i class="fa fa-eye text-blue"></i></a>' +
                        '<a href="javascript:void(0)" ng-if="row.entity.vlapappL_SupportingDocument == 0" ng-model="row.entity.vlapappL_SupportingDocument" ng- title="No Document"> <i class="fa fa-eye-slash text-red"></i></a>' +
                        '</div>'
                },
                { name: 'vlapappL_Reason', displayName: 'Reason', enableCellEdit: false }
            ]
        };
        //cellTemplate: '<div class="abcd"><span>{{COL_FIELD}}</span></div>' 
        $scope.gridlapstatus.onRegisterApi = function (gridApi) {
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
            apiService.getURI("LaptopApplication/getApprovalStatus", id).
                then(function (promise) {
                    $scope.gridlapstatus.data = promise.approvalrequestlist;
                    angular.forEach($scope.gridlapstatus.data, function (grdobj) {
                        var fdate = grdobj.vlapappL_FromDate.split('T');
                        grdobj.vlapappL_FromDate = fdate[0];

                        var tdate = grdobj.vlapappL_ToDate.split('T');
                        grdobj.vlapappL_ToDate = tdate[0];
                    });

                    $scope.activityIds = promise.applicationApproved;
                });
        };

        $scope.get_status = function () {
            if ($scope.myForm.$valid) {

                var data = {
                    get_approval_status: $scope.gridApi.selection.getSelectedRows(),
                    "VLAPAPPLAUT_Remarks": $scope.remarkstxta
                };
               
                apiService.create("LaptopApplication/get_status", data).
                    then(function (promise) {
                        $scope.gridlapstatus.data = promise.get_leavestatus;
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
                    get_approval_status: $scope.gridApi.selection.getSelectedRows(),
                    "VLAPAPPLAUT_Remarks": $scope.remarkstxta
                };

                apiService.create("LaptopApplication/reject_status", data).
                    then(function (promise) {
                        $scope.gridlapstatus.data = promise.get_leavestatus;
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
                var img = obj.vlapappL_SupportingDocument;
                var imagarr = img.split('.');
                var extention = imagarr[imagarr.length - 1];
                obj.filetype = extention;
                if (extention === 'doc' || extention === 'docx' || extention === 'ppt' || extention === 'pptx' || extention === 'xlsx' || extention === 'xls') {
                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + obj.vlapappL_SupportingDocument;
                }
                else if (extention === "jpg" || extention === "jpeg" || extention === "JPEG") {
                    $('#preview').removeAttr('src');
                    $('#preview').attr('src', obj.vlapappL_SupportingDocument);
                    $('#myModal').modal();
                    $('#myModal').modal({ keyboard: false });
                    $('#myModal').modal('show');
                }
                else if (extention === "pdf") {
                    //var imagedownload = obj.vlapappL_SupportingDocument;
                    //$scope.content = "";
                    //var fileURL = "";
                    //var file = "";
                    //$http.get(imagedownload, { responseType: 'arraybuffer' })
                    //    .success(function (response) {
                    //        file = new Blob([(response)], { type: 'application/pdf' });
                    //        fileURL = URL.createObjectURL(file);
                    //        $scope.content = $sce.trustAsResourceUrl(fileURL);
                    //        $('#showpdf').modal('show');
                    //    });

                    var imagedownload = obj.vlapappL_SupportingDocument;
                    $scope.content = "";
                    var fileURL = "";
                    var file = "";
                    $http.get(imagedownload, { responseType: 'arraybuffer' })
                        .success(function (response) {
                            file = new Blob([(response)], { type: 'application/pdf' });
                            fileURL = URL.createObjectURL(file);
                            var pdfId = document.getElementById("pdfIdzz");
                            pdfId.removeChild(pdfId.childNodes[0]);
                            var embed = document.createElement('embed');
                            embed.setAttribute('src', fileURL);
                            embed.setAttribute('type', 'application/pdf');
                            embed.setAttribute('width', '100%');
                            embed.setAttribute('height', '1000');
                            pdfId.appendChild(embed);
                            $('#showpdf').modal('show');

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

        $scope.zoomin = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth >= 750) {
                swal("Maximum zoom-in level reached.");
            } else {
                myImg.style.width = (currWidth + 50) + "px";
            }
        };

        $scope.zoomout = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth <= 400) {
                swal("Maximum zoom-out level reached.");
            } else {
                myImg.style.width = (currWidth - 50) + "px";
            }
        };
    }
})();