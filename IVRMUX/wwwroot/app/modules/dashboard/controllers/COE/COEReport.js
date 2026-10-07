
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
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
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
        $scope.gridOptions = {
          
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', displayName: 'SL NO', width: '7%', enableFiltering: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },

              { name: 'eventName', width: '15%', displayName: 'Event Name' },
              { name: 'eventDesc', width: '50%', displayName: 'Event Description' },
              { name: 'coeE_EStartDate', width: '14%', displayName: 'Event Start Date', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' },
              { name: 'coeE_EEndDate', width: '14%', displayName: 'Event End Date', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' }, ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            },
            //    gridMenuCustomItems: [{
            //        title: 'Export all data as EXCEL',
            //        action: function ($event) {
            //            exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'all', 'all');
            //        },
            //        order: 110
            //    },
            //      {
            //          title: 'Export visible data as EXCEL',
            //          action: function ($event) {
            //              exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'visible', 'visible');
            //          },
            //          order: 111
            //      },
            //]
        };
        $scope.exportToExcel = function (tableId) {

            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
            //$state.reload();
        }
        //$scope.exportExcel = function () {
        //    var grid = $scope.gridApi.grid;
        //    var rowTypes = exportUiGridService.ALL;
        //    var colTypes = exportUiGridService.ALL;
        //    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'all', 'all');
        //};
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


        //$scope.gridOptions = {
        //    enableColumnMenus: false,
        //    enableFiltering: true,
        //    paginationPageSizes: [5, 10, 15],
        //    paginationPageSize: 5,
        //    enableGridMenu: true,
        //    enableSelectAll: true,
        //    exporterCsvFilename: 'myFile.csv',
        //    exporterPdfDefaultStyle: {fontSize: 9},
        //    exporterPdfTableStyle: {margin: [30, 30, 30, 30]},
        //    exporterPdfTableHeaderStyle: {fontSize: 10, bold: true, italics: true, color: 'red'},
        //    exporterPdfHeader: { text: "My Header", style: 'headerStyle' },
        //    exporterPdfFooter: function ( currentPage, pageCount ) {
        //        return { text: currentPage.toString() + ' of ' + pageCount.toString(), style: 'footerStyle' };
        //    },
        //    exporterPdfCustomFormatter: function ( docDefinition ) {
        //        docDefinition.styles.headerStyle = { fontSize: 22, bold: true };
        //        docDefinition.styles.footerStyle = { fontSize: 10, bold: true };
        //        return docDefinition;
        //    },
        //    exporterPdfOrientation: 'portrait',
        //    exporterPdfPageSize: 'LETTER',
        //    exporterPdfMaxGridWidth: 500,
        //    exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        //    onRegisterApi: function(gridApi){
        //        $scope.gridApi = gridApi;
        //    },
        //    exporterFieldCallback: function (grid, row, col, input) {
        //        if (col.name == 'SNO') {
        //            input = grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1
        //        }


        //        if (col.cellFilter) { // check if any filter is applied on the column
        //            var filters = col.cellFilter.split('|'); // get all the filters applied
        //            angular.forEach(filters, function (filter) {
        //                var filterName = filter.split(':')[0]; // fetch filter name
        //                var filterParams = filter.split(':').splice(1); //fetch all the filter parameters
        //                filterParams.unshift(input); // insert the input element to the filter parameters list
        //                var filterFn = $filter(filterName); // filter function
        //                // call the filter, with multiple filter parameters. 
        //                //'Apply' will call the function and pass the array elements as individual parameters to that function.
        //                input = filterFn.apply(this, filterParams);

        //            })
        //            return input;
        //        }
        //        else
        //            return input;

        //    },

        //    columnDefs: [
        //          { name: 'SNO',width:'5%', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },

        //      { name: 'eventName', width: '15%', displayName: 'Event Name' },
        //      { name: 'eventDesc', width: '50%', displayName: 'Event Description' },
        //      { name: 'coeE_EStartDate', width: '15%', displayName: 'Event Start Date', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MMM-yy"' },
        //      { name: 'coeE_EEndDate', width: '15%', displayName: 'Event End Date', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MMM-yy"' },





        //       //{
        //       //    field: 'id', name: '',
        //       //    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
        //         //'<div class="grid-action-cell">' +
        //         //'<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' +
        //         //'<a ng-if="row.entity.ttmsaB_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch(row.entity);"><md-tooltip md-direction="down">Active Now</md-tooltip> <i class="fa fa-toggle-on text-red" aria-hidden="true"></i></a>' +
        //         // '<span ng-if="row.entity.ttmsaB_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch(row.entity);"> <md-tooltip md-direction="down">Deactive Now</md-tooltip> <i class="fa fa-toggle-off text-green" aria-hidden="true"></i></a><span>' +
        //         //'</div>'
        //         //'<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>' +
        //         //'<a href="javascript:void(0)" ng-click="grid.appScope.deletedata(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
        //           //'</div>'
        //            //'<div class="grid-action-cell">' +
        //            // '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup1" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup1(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;'+
        //         //    +
        //       //  '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue1(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
        //      //  '<a ng-if="row.entity.coemE_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch1(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
        //    //  '<span ng-if="row.entity.coemE_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch1(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +

        //     //   '</div>'
        //     //  }
        //    ]          

        //};
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
            });
        }
        $scope.getReport = function () {
            $scope.all1 = "1";
            if ($scope.myForm.$valid) {
                var data = {
                    "type": $scope.all1,
                    "ASMAY_Id": $scope.Year,
                    "month": $scope.month,
                    "ASMCL_Id": $scope.ASMCL_Id
                }
                // if ($scope.myForm.$valid) {
                apiService.create("COEReport", data).then(function (promise) {

                    
                    $scope.institute = promise.institute;
                    $scope.mI_Name = promise.institute[0].mI_Name;
                    $scope.mI_Address1 = promise.institute[0].mI_Address1;
                    $scope.mI_Logo = promise.institute[0].mI_Logo;

                    if (promise.count > 0) {
                        $scope.gridOptions.data = promise.coereport;

                        $scope.count = promise.count;
                        $scope.grid_view = true;
                    }
                    else {
                        // $scope.gridOptions.data.length=0;
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