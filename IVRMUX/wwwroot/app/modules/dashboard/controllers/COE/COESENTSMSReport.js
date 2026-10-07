
(function () {
    'use strict';
    angular
.module('app')
.controller('COEReport', COEReport)

    COEReport.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache', '$filter', 'Excel', '$timeout']
    function COEReport($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache, $filter, Excel, $timeout) {
        //$scope.editEmployee = {};

         $scope.ddate = {};
        $scope.ddate = new Date();
        //var paginationformasters;
        //var copty;
        //var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings.length > 0) {
        //    paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        //    copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        //}


        var paginationformasters;
        var copty;

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
        $scope.empflag = true;
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;


        $scope.coptyright = copty;
        $scope.sortKey = "regno";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };
        $scope.grid_view = false;
        //Ui Grid view rendering data from data base
        //$scope.gridOptions = {
          
        //    enableColumnMenus: false,
        //    enableFiltering: true,
        //    paginationPageSizes: [5, 10, 15],
        //    paginationPageSize: 5,

        //    columnDefs: [
        //          { name: 'SNO', displayName: 'SL NO', width: '7%', enableFiltering: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },

        //      { name: 'eventName', width: '15%', displayName: 'Event Name' },
        //      { name: 'eventDesc', width: '50%', displayName: 'Event Description' },
        //      { name: 'coeE_EStartDate', width: '14%', displayName: 'Event Start Date', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' },
        //      { name: 'coeE_EEndDate', width: '14%', displayName: 'Event End Date', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' }, ],
        //    onRegisterApi: function (gridApi) {
        //        $scope.gridApi = gridApi;
        //        // $scope.gridApi.core.refresh($scope.gridOptions.data);
        //    },
          
        //};

        $scope.All_Individual = function (typeflag) {
            $scope.coereport = [];
            if ($scope.typeflag === typeflag)
                $scope.grid_view = false;
            else
                $scope.grid_view = true;
        };

        $scope.exportToExcel = function (tableId) {

            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
            //$state.reload();
        }


        $scope.printToCart = function () {
            $scope.printsection = true;
            var innerContents = document.getElementById("printrcp").outerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
               '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BArrearReportPdf.css" />' +
    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
            $scope.printsection = false;
        }

   
        $scope.labeldisable = true;
        $scope.onradiobuttonSelect = function (type) {
            $scope.labeldisable = false;
            // $scope.Year = "";
            //$scope.month = "";
            //$scope.ASMCL_Id = "";
        }
        $scope.clear = function () {
            $state.reload();
        }
        $scope.loaddata = function () {
            // load data
            var id = 2;
            debugger;
            apiService.getURI("COEReport/", id).then(function (promise) {
                debugger;
                $scope.year = promise.fillyear;
                $scope.classDrpdwn = promise.classlist;
                $scope.modulenames = promise.modulenames;
            });
        }

        $scope.all_checkC = function () {
            var checkStatus = $scope.ivrmM_Id;
            var count = 0;
            angular.forEach($scope.modulenames, function (itm) {
                itm.selected = checkStatus;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }


        $scope.isOptionsRequired = function () {
            return !$scope.modulenames.some(function (options) {
                return options.selected;
            });
        }

        $scope.togchkbxC = function () {

            $scope.ivrmM_Id = $scope.modulenames.every(function (options) {
                return options.selected;
            });
        }

        $scope.getReport = function () {
           
            if ($scope.myForm.$valid) {

                if ($scope.typeflag == 'EMAIL') {
                    $scope.typeflag = 'EMAIL';
                }
                else {
                    $scope.typeflag = 'SMS';
                }

                var MODULEID = [];
                angular.forEach($scope.modulenames, function (ty) {
                    if (ty.selected) {
                        MODULEID.push({ IVRMM_Id: ty.ivrmM_Id, IVRMM_ModuleName: ty.ivrmM_ModuleName});
                    }
                })

                var data = {
                    "COEE_EStartDate": new Date($scope.FromDate).toDateString(),
                    "COEE_EEndDate": new Date($scope.ToDate).toDateString(),
                    "modulearray": MODULEID,
                    "type": $scope.typeflag
                   
                }
               
                apiService.create("COEReport/getcoesmsreport", data).then(function (promise) {
                    
                    if (promise.coereport.length > 0 && promise.coereport != null) {
                        $scope.grid_view = true;
                        $scope.presentCountgrid = promise.coereport.length;
                        $scope.coereport = promise.coereport;

                    }
                    else {
                        swal("No Records Found");
                        $scope.count = 0;
                        $scope.grid_view = false;
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        }

      
        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

       

    }

})();