
dashboard.controller("ExamImportController", ['$rootScope', '$scope', '$state', '$location', 'apiService', 'dashboardService', 'Flash', 'superCache', 'Excel', '$timeout', 'exportUiGridService',
function ($rootScope, $scope, $state, $location, apiService, dashboardService, Flash, superCache, Excel, $timeout, exportUiGridService) {

    var EI = this;

    EI.gridOptions = {};


    //$scope.exportToExcel = function (table) {
    //            var exportHref = Excel.tableToExcel('#table', 'sheet name');
    //            $timeout(function () { location.href = exportHref; }, 100);

    //}


    $scope.gridOptions = {

        enableColumnMenus: false,
        enableFiltering: true,
        paginationPageSizes: [10, 20, 30],
        paginationPageSize: 20,
        enableGridMenu: false,
        columnDefs: [
            { name: 'amsT_Id', displayName: 'AMST_Id' },
          { name: 'studentname', displayName: 'Student_Name' },
          { name: 'amsT_AdmNo', displayName: 'Admission_No' },
          { name: 'amaY_RollNo', displayName: 'Roll_No' },
            { name: 'totalMarks', displayName: 'Max_Marks' },
            { name: 'minMarks', displayName: 'Min_Marks' },
            { name: 'obtainmarks', displayName: 'Obtain_Marks' }

        ],
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
        },
        gridMenuCustomItems: [{
            title: 'Export all data as EXCEL',
            action: function ($event) {
                exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'all', 'all');
            },
            order: 110
        },
                {
                    title: 'Export visible data as EXCEL',
                    action: function ($event) {
                        exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'visible', 'visible');
                    },
                    order: 111
                }
        ]

    };

    $scope.exportExcel = function () {
        var grid = $scope.gridApi.grid;
        var rowTypes = exportUiGridService.ALL;
        var colTypes = exportUiGridService.ALL;
        exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'all', 'all');
    };
    //$scope.gridOptions.onRegisterApi = function (gridApi) {
    //    $scope.gridApi = gridApi;
    //};

    $scope.rediochange = function (impall) {
        if (impall == "Import") {
            //$scope.first == false;
            // $scope.second == true;
        }
        else if (impall == "Export") {
            // $scope.first == true;
            // $scope.second == false;
            $scope.exceltable = false;
        }
    }

    $scope.Clearid = function () {
        $state.reload();
    }

    $scope.BindData = function () {
        apiService.getDATA("ExamImport/Getdetails").
   then(function (promise) {
       $scope.acdlist = promise.acdlist;

   })
    };

    $scope.onselectAcdYear = function (ASMAY_Id) {
        var data = {
            "ASMAY_Id": ASMAY_Id
        }
        var config = {
            headers: {
                'Content-Type': 'application/json;'
            }
        }
        apiService.create("ExamImport/onselectAcdYear", data).
   then(function (promise) {
       $scope.ctlist = promise.ctlist;
   })
    };

    $scope.onselectclass = function (ASMCL_Id, ASMAY_Id) {
        var data = {
            "ASMAY_Id": ASMAY_Id,
            "ASMCL_Id": ASMCL_Id
        }
        var config = {
            headers: {
                'Content-Type': 'application/json;'
            }
        }
        apiService.create("ExamImport/onselectclass", data).
   then(function (promise) {
       $scope.seclist = promise.seclist;
   })
    };


    $scope.onselectSection = function (ASMS_Id, ASMCL_Id, ASMAY_Id) {
        var data = {
            "ASMS_Id": ASMS_Id,
            "ASMCL_Id": ASMCL_Id,
            "ASMAY_Id": ASMAY_Id
        }
        var config = {
            headers: {
                'Content-Type': 'application/json;'
            }
        }
        apiService.create("ExamImport/onselectSection", data).
   then(function (promise) {
       $scope.examlist = promise.examlist;
   })
    };

    $scope.onselectExam = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id) {
        var data = {
            "ASMS_Id": ASMS_Id,
            "ASMCL_Id": ASMCL_Id,
            "ASMAY_Id": ASMAY_Id,
            "EME_Id": EME_Id
        }
        var config = {
            headers: {
                'Content-Type': 'application/json;'
            }
        }
        apiService.create("ExamImport/onselectExam", data).
   then(function (promise) {
       $scope.subjectlist = promise.subjectlist;
   })
    };

    // $scope.gridOptions = {};

    $scope.onsearch = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id) {

        var data = {
            "ASMS_Id": ASMS_Id,
            "ASMCL_Id": ASMCL_Id,
            "ASMAY_Id": ASMAY_Id,
            "EME_Id": EME_Id,
            "ISMS_Id": ISMS_Id
        }
        var config = {
            headers: {
                'Content-Type': 'application/json;'
            }
        }
        apiService.create("ExamImport/onsearch", data).
   then(function (promise) {



       $scope.gridOptions.data = promise.studentList;
       $scope.subMorGFlag = promise.subMorGFlag;
       $scope.gradname = promise.gradname;
       $scope.marksdeleteflag = promise.marksdeleteflag;


   })
    };

    $scope.SelectedFileForUploadzd = [];

    $scope.selectFileforUploadzd = function (input) {
        

        $scope.SelectedFileForUploadzd = input.files;

        if (input.files && input.files[0]) {

            if (input.files[0].type == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                var reader = new FileReader();
                reader.readAsDataURL(input.files[0]);
            }
            else {
                swal("Please Upload the .xlsx file");
                return;
            }
        }


    }

 

    $scope.SaveMarks = function (gridOptions, ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id) {
        if ($scope.myForm.$valid) {
            var flag = "false";

            if ($scope.marksdeleteflag == "true") {
                swal({
                    title: "Are You Sure?",
                    text: "Marks Already Calculated, Do You Want To Continue ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                    cancelButtonText: "Cancel..!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
            function (isConfirm) {
                if (isConfirm) {
                    $scope.savedata(gridOptions,ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id);
                }
                else {
                    flag = "false"

                }

            });
            }
            else {
                $scope.savedata(gridOptions, ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id);
            }


        }
        else {
            swal('Entered Marks are nor correct..pease check red marks text..!');
        }
    };

    $scope.savedata = function (gridOptions, ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id) {

        var valu = gridOptions;
        $scope.albumNameArray = [];
        if (valu.length > 0) {
            $scope.albumNameArray = [];
            var clmns = EI.gridOptions.columnDefs.length;
            for (var i = 0; i < valu.length; i++) {
                $scope.albumNameArray.push(valu[i]);
            }

        }
        var data = {
            "ASMS_Id": ASMS_Id,
            "ASMCL_Id": ASMCL_Id,
            "ASMAY_Id": ASMAY_Id,
            "EME_Id": EME_Id,
            "ISMS_Id": ISMS_Id,
            "newlstget": $scope.albumNameArray
        }
        var config = {
            headers: {
                'Content-Type': 'application/json;'
            }
        }

        apiService.create("ExamImport/ImportMarks/", data).
        then(function (promise) {
            if (promise.returnMsg != null || promise.returnMsg != "") {
                alert(promise.returnMsg);
            }
            if (promise.messagesaveupdate == "true") {
                $scope.BindData();
                swal('Data Saved Successfully');
            }
            else {
                swal('Failed to Save/Update Data');
            }

            $state.reload();

        })

    }

}]).filter('keys', function () {
    
    return function (input) {
        if (!input) {
            return [];
        }
        delete input.$$hashKey;
        return Object.keys(input);
    }
})

.directive("filereadexam", ['$rootScope','apiService', function ($rootScope,apiService) {
    
    return {
        scope: {
            opts: '='

        },
        link: function ($scope, $elm, $attrs) {

            $elm.on('change', function (changeEvent) {

                var reader = new FileReader();

                reader.onload = function (evt) {
                    $scope.$apply(function () {
                        var data = evt.target.result;

                        var workbook = XLSX.read(data, { type: 'binary' });

                        var headerNames = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]], { header: 1 })[0];

                        var data = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]]);
                        if (data.length == 0) {

                            swal("Excel Sheet is Empty");
                            $elm.val(null);
                            $scope.opts.data = null;
                            return;

                        } else {

                            $scope.opts.columnDefs = [];
                            headerNames.forEach(function (h) {
                                $scope.opts.columnDefs.push({ field: h });
                            });
                            //dfsd
                            $scope.opts.data = data;
                            // $rootScope.validateForm();
                            
                            console.log($scope.opts.data);
                            var data1 = {
                                "newlstget1": $scope.opts.data
                            }
                          
                            $scope.opts.data = $scope.opts.data;
                                
                           
                          
                            //$elm.val(null);
                        }
                    });
                };
                reader.readAsBinaryString(changeEvent.target.files[0]);
               
            });
        }
    }
}]);

dashboard.factory('exportUiGridService', exportUiGridService);

exportUiGridService.inject = ['uiGridExporterService'];
function exportUiGridService(uiGridExporterService) {
    var service = {
        exportToExcel: exportToExcel
    };

    return service;

    function Workbook() {
        if (!(this instanceof Workbook)) return new Workbook();
        this.SheetNames = [];
        this.Sheets = {};
    }

    function exportToExcel(sheetName, gridApi, rowTypes, colTypes) {
        var columns = gridApi.grid.options.showHeader ? uiGridExporterService.getColumnHeaders(gridApi.grid, colTypes) : [];
        var data = uiGridExporterService.getData(gridApi.grid, rowTypes, colTypes);
        var fileName = gridApi.grid.options.exporterExcelFilename ? gridApi.grid.options.exporterExcelFilename : 'StudentMarks';
        fileName += '.xlsx';
        var wb = new Workbook(),
            ws = sheetFromArrayUiGrid(data, columns);
        wb.SheetNames.push(sheetName);
        wb.Sheets[sheetName] = ws;
        var wbout = XLSX.write(wb, {
            bookType: 'xlsx',
            bookSST: true,
            type: 'binary'
        });
        saveAs(new Blob([s2ab(wbout)], {
            type: 'application/octet-stream'
        }), fileName);
    }

    function sheetFromArrayUiGrid(data, columns) {
        var ws = {};
        var range = {
            s: {
                c: 10000000,
                r: 10000000
            },
            e: {
                c: 0,
                r: 0
            }
        };
        var C = 0;
        columns.forEach(function (c) {
            var v = c.displayName || c.value || columns[i].name;
            addCell(range, v, 0, C, ws);
            C++;
        }, this);
        var R = 1;
        data.forEach(function (ds) {
            C = 0;
            ds.forEach(function (d) {
                var v = d.value;
                addCell(range, v, R, C, ws);
                C++;
            });
            R++;
        }, this);
        if (range.s.c < 10000000) ws['!ref'] = XLSX.utils.encode_range(range);
        return ws;
    }
    /**
     * 
     * @param {*} data 
     * @param {*} columns 
     */

    function datenum(v, date1904) {
        if (date1904) v += 1462;
        var epoch = Date.parse(v);
        return (epoch - new Date(Date.UTC(1899, 11, 30))) / (24 * 60 * 60 * 1000);
    }

    function s2ab(s) {
        var buf = new ArrayBuffer(s.length);
        var view = new Uint8Array(buf);
        for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
        return buf;
    }

    function addCell(range, value, row, col, ws) {
        if (range.s.r > row) range.s.r = row;
        if (range.s.c > col) range.s.c = col;
        if (range.e.r < row) range.e.r = row;
        if (range.e.c < col) range.e.c = col;
        var cell = {
            v: value
        };
        if (cell.v == null) cell.v = '-';
        var cell_ref = XLSX.utils.encode_cell({
            c: col,
            r: row
        });

        if (typeof cell.v === 'number') cell.t = 'n';
        else if (typeof cell.v === 'boolean') cell.t = 'b';
        else if (cell.v instanceof Date) {
            cell.t = 'n';
            cell.z = XLSX.SSF._table[14];
            cell.v = datenum(cell.v);
        } else cell.t = 's';

        ws[cell_ref] = cell;
    }
}



