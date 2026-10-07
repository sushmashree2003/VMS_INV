(function () {
    'use strict';
    angular
        .module('app')
        .controller('AdvPlannerImportController', AdvPlannerImportController);
    AdvPlannerImportController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', 'Excel','$timeout'];
    function AdvPlannerImportController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter, Excel, $timeout) {

        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        if (transnumconfigsettings != null && transnumconfigsettings.length > 0) {
            for (var i = 0; i < transnumconfigsettings.length; i++) {
                if (transnumconfigsettings[i].imN_Flag === "ISMTASK") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }

        $scope.filename = '';
        $scope.tblsh = false;
        var vm = this;
        vm.gridOptions = {};
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.reverse = false;
        var path = '../../../../../../DEMOSAMPLE.xls';
        //var path = '../../../../../../VMS-Functional Document.docx';

    //    $scope.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + path;
        $scope.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=C:\Users\praveen\Downloads/DEMOSAMPLE.xls";
        //$scope.loaddata = function () {
        //    debugger;
          
        //    var img = path;
        //    var imagarr = img.split('.');
        //    var lastelement = imagarr[imagarr.length - 1];
           
        //    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
        //        $scope.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + path;

        //}

        $scope.order = function (keyname) {

            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input) {
            $scope.SelectedFileForUploadzd = input.files;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    //once return success 
                    var reader = new FileReader();
                    reader.readAsDataURL(input.files[0]);
                    $scope.filename = input.files[0].name;
                    // alert($scope.filename);
                }
                else {
                    //swal("Please Upload the .xlsx file");
                    //return;
                    $scope.filename = input.files[0].name;
                }
            }
        };




        $scope.exportToExcel = function (tabel1) {

            var exportHref = Excel.tableToExcel(tabel1, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 900);
        }

        $scope.savedata1 = function () {



        }
        $rootScope.validateForm = function () {
            //debugger;
            //console.log($scope.opts.data);
            //var data1 = {
            //    "newlstget1": $scope.opts.data
            //}
            //apiService.create("ImportLibrarydata/checkvalidation", data1).then(function (promise) {
            //    if (promise.stuStatus != "" && promise.stuStatus != null) {
            //        swal(promise.stuStatus);
            //        $elm.val(null);
            //        $scope.opts.data = null;
            //    } else {
            //        $scope.opts.data = $scope.opts.data;
            //    }
            //});
        };
        

        $scope.savedata = function (gridOptions) {
            debugger;
            $scope.headers = ["Department", "Project", "Module", "TaskTitle", "Priority", "Category", "TaskDescription", "IssueType", "Remarks", "AssignedByCompanyCode", "AssignedByEmployeeCode", "AssignedToCompanyCode", "AssignedToEmployeeCode", "StartDay", "StartMonth", "StartYear", "EndDay", "EndMonth", "EndYear", "Periodicity", "TaskDay", "EffortType", "Effort", "YearlyTaskDay", "YearlyTaskMonth", "YearlyTaskYear"];
            var data = {};
            var valu = gridOptions;
            $scope.albumNameArray = [];
            if (valu.length > 0) {
                $scope.albumNameArray = [];
                for (var i = 0; i < valu.length; i++) {
                    $scope.albumNameArray.push(valu[i]);
                }
            }
            $scope.albumNameArray1 = [];
            angular.forEach($scope.albumNameArray[0], function (value, key) {
                //angular.forEach(value, function (value,key) {
                $scope.albumNameArray1.push(key);
                //});               
            });

            var excellvalidatecount = 0;

            angular.forEach($scope.headers, function (head1) {
                angular.forEach($scope.albumNameArray1, function (head2) {
                    if (head1 === head2) {
                        excellvalidatecount = excellvalidatecount + 1;
                    }
                });
            });    
            
            if (excellvalidatecount == 26) {
                data = {
                    "advimppln": $scope.albumNameArray,
                    transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("ISMImport/saveadvance", data).
                    then(function (promise) {

                        if (promise.message !== "") {
                            swal(promise.message);
                        }
                        else if (promise.returnval === true && promise.duplicate===0) {
                            swal("Saved Records--" + promise.suscnt + " \n " + "Not Saved Records --" + promise.failcnt);
                        }
                        else if (promise.returnval === true && promise.duplicate > 0) {
                            //swal("Saved Records--" + promise.suscnt + " \n              " + "Not Saved Records --" + promise.failcnt + " \n              " + "Duplicate Record --" + promise.duplicate + " \n              " + "Duplicate Rows --" + promise.returnduplicatestatus);
                            
                            var hh = "<h3><b>Saved Records-- " + promise.suscnt + "<br/><br/>" + "Not Saved Records -- " + promise.failcnt + "<br/><br/>" + "Duplicate Record -- " + promise.duplicate + " <br/><br/>" + "Duplicate Rows No. -- <span style='word-wrap: break-word'>" + promise.returnduplicatestatus +"</span></b></h3>";
                            swal({
                                title: "Import Status",
                                html: true,
                                text:  hh,                               
                                allowOutsideClick: "false"
                            });
                        }
                        else {
                            swal("Please Select Correct File Format");
                        }
                        $state.reload();
                    });
            }
            else {
                swal("Header Mismatch..!!","Please Import a Excel With All/Proper Headers.");
            }
        };
    }
})();
angular
    .module('app').filter('keys', function () {

        return function (input) {
            if (!input) {
                return [];
            }
            delete input.$$hashKey;
            return Object.keys(input);
        }

    })

angular
    .module('app').directive("fileread", ['$rootScope', 'apiService', function ($rootScope, apiService) {
       
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
                            data = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]]);
                            if (data.length === 0) {
                                swal("Excel Sheet is Empty");
                                $elm.val(null);
                                $scope.opts.data = null;
                                return;
                            }
                            else {
                                $scope.headers = ["Department", "Project", "Module", "TaskTitle", "Priority", "Category", "TaskDescription", "IssueType", "Remarks", "AssignedByCompanyCode", "AssignedByEmployeeCode", "AssignedToCompanyCode", "AssignedToEmployeeCode", "StartDay", "StartMonth", "StartYear", "EndDay", "EndMonth", "EndYear", "Periodicity", "TaskDay", "EffortType", "Effort", "YearlyTaskDay", "YearlyTaskMonth", "YearlyTaskYear"];
                                $scope.opts = {};
                                $scope.opts.columnDefs = [];
                                headerNames.forEach(function (h) {
                                    $scope.opts.columnDefs.push({ field: h });
                                });     
                                $scope.checkheaders = [];
                                headerNames.forEach(function (h) {
                                    $scope.checkheaders.push(h);
                                });
                                var excellvalidatecount = 0;                                
                                $scope.Missinghead = [];
                                angular.forEach($scope.headers, function (head1) {
                                    var missingHead = 0;
                                    angular.forEach($scope.checkheaders, function (head2) {
                                        if (head1 === head2) {
                                            excellvalidatecount = excellvalidatecount + 1;
                                            missingHead = 1;
                                        }
                                    });
                                    if (missingHead === 0) {
                                        $scope.Missinghead.push(head1);
                                    }
                                });
                                debugger;
                                if (excellvalidatecount === 26) {
                                    var bind = true;                                    
                                    var excellcellvalidate = 0;    
                                    $scope.fushlast = [];
                                    $scope.fushlast.push(0);
                                    angular.forEach(data, function (valu, ke) {
                                           $scope.rowheaders = [];
                                            angular.forEach(valu, function (val, ey) {
                                                $scope.rowheaders.push(ey);                                                                                        
                                            }); 
                                            var checkrow = 0;
                                            angular.forEach($scope.checkheaders, function (head1) {
                                                angular.forEach($scope.rowheaders, function (head2) {
                                                    if (head1 === head2) {
                                                        checkrow = checkrow + 1;
                                                    }
                                                });
                                        });
                                        if (checkrow !== $scope.checkheaders.length) { 
                                                $scope.fushlast.push(valu.__rowNum__+1);
                                            }
                                    });
                                    if ($scope.fushlast.length === 1) {
                                        $scope.opts.data = data;
                                    }
                                    else { 
                                        var Missingrows = "";
                                        if ($scope.fushlast.length > 1) {
                                            angular.forEach($scope.fushlast, function (head2) {   
                                                if (head2 != 0) {
                                                    if (Missingrows === "") {
                                                        Missingrows = head2;
                                                    }
                                                    else {
                                                        Missingrows = Missingrows + "," + head2;
                                                    }    
                                                }  
                                            });
                                            Missingrows = "The Row " + Missingrows+" Contains Empty Cell Values, replace with the NULL for empty cell";
                                        }
                                        swal(Missingrows,"Excel Data is Not Proper!!");
                                    }
                                }
                                else {
                                    var Missingheads = "";
                                    if ($scope.Missinghead.length > 0) {
                                        angular.forEach($scope.Missinghead, function (head2) {
                                            if (head2 != 0) {
                                                if (Missingheads === "") {
                                                    Missingheads = head2;
                                                }
                                                else {
                                                    Missingheads = Missingheads + ", \n" + head2;
                                                }
                                            }
                                        });
                                        Missingheads = "The Missing Headers: \n" + Missingheads;
                                        swal(Missingheads, "Header Missing..!!");
                                    }
                                    else {
                                        swal("Header Missing..!!", "Please Import a Excel With All/Proper Headers.");
                                    }                                  
                                   
                                }
                                //console.log($scope.opts.data);
                                //apiService.create("ImportLibrarydata/checkvalidation", data1).then(function (promise) {
                                //    if (promise.stuStatus != "" && promise.stuStatus != null) {
                                //        swal(promise.stuStatus);
                                //        $elm.val(null);
                                //        $scope.opts.data = null;
                                //    } else {
                                //        $scope.opts.data = $scope.opts.data;
                                //    }
                                //});
                                //$elm.val(null);
                            }
                        });
                    };
                    reader.readAsBinaryString(changeEvent.target.files[0]);
                });
            }
        };
    }]);

