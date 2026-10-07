(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_RoleDeptMapping', ISM_RoleDeptMapping);

    ISM_RoleDeptMapping.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q'];
    function ISM_RoleDeptMapping($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, Excel) {

        $scope.submitted = false;
        $scope.disableDep = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.isoptionsRequired = function () {
            return !$scope.department_list.some(function (options) {
                return options.selected;
            });
        };
        //==================ALL================================//
        $scope.searchbox = "";
        $scope.all_checkC = function () {

            var checkStatus = $scope.usercheckC;
            var count = 0;
            angular.forEach($scope.department_list, function (itm) {
                itm.selected = checkStatus;
                if (itm.selected === true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        };
        //===============================clear====================================//
        $scope.Clearid = function () {
            $scope.submitted = false;
            $state.reload();
        };
        //============================================loaddata============================//
        $scope.submitted = false;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("ISM_RoleDeptMapping/loaddata/", pageid).then(function (promise) {
                $scope.role_list = promise.role_list;
                $scope.department_list = promise.department_list;
                $scope.get_deptlist = promise.get_deptlist;
                $scope.get_delist = $scope.get_deptlist.length;
            });
        };
        //=================================adddata()==================================//
        $scope.tempadd = [];
        $scope.adddata = function () {
            if ($scope.myForm.$valid) {
                $scope.unblockflg = 0;
                $scope.tempdept = [];
                angular.forEach($scope.role_list, function (rl) {
                    if (rl.ivrmrT_Id === parseInt($scope.IVRMRT_Id)) {
                        $scope.rolename = rl.ivrmrT_Role;
                    }
                });
                angular.forEach($scope.department_list, function (dep) {
                    if (dep.selected === true) {
                        dep.disableDep = true;
                        $scope.tempdept.push({ hrmD_Id: dep.hrmD_Id, hrmD_DepartmentName: dep.hrmD_DepartmentName });
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.delete = function (row, index) {
            angular.forEach($scope.department_list, function (dep) {
                if (dep.selected === true && dep.hrmD_Id == $scope.tempdept[index].hrmD_Id) {
                    dep.disableDep = false;
                    dep.selected = false;
                }
            });
            for (var y = 0; y < $scope.tempdept.length; y++) {
                if (y === index) {
                    $scope.tempdept.splice(y, 1);
                }
            }
        };
        //================================savedata=========================================//
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                $scope.departmentsss = [];
                angular.forEach($scope.department_list, function (ss) {
                    if (ss.selected === true) {
                        $scope.departmentsss.push(ss);
                    }
                });
                var data = {
                    "ISMRTDEM_Id": $scope.ismrtdeM_Id,
                    "IVRMRT_Id": $scope.IVRMRT_Id,
                    departmentids: $scope.departmentsss
                };
                apiService.create("ISM_RoleDeptMapping/savedata", data).then(function (promise) {
                    if (promise.duplicate === true) {
                        swal("Data already exist");
                    }
                    else if (promise.msg === 'Saved') {
                        swal("Data saved/Updated Successfully.");
                        $state.reload();
                    }
                    else if (promise.msg === 'Failed') {
                        swal("Failed");
                    }
                    else if (promise.msg === 'Updated') {
                        swal("Data Updated");
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };
        //======================================deptDecative=====================================================//
        $scope.deptDecative = function (usersem, SweetAlert) {
            var dystring = "";

            if (usersem.ismrtdeM_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (usersem.ismrtdeM_ActiveFlag === false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes ," + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ISM_RoleDeptMapping/deptDecative", usersem).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record " + dystring + "d" + " Successfully!!!");
                                $state.reload();
                            }
                            else {
                                swal("Record Not " + dystring + "d" + " Successfull!!!");
                                $state.reload();
                            }
                        });
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        };
        //=============================================printData()=========================================================//
        $scope.printData = function () {
            var connect = document.getElementById("printdataid").innerHTML;
            var popupWinindow = window.open("");
            popupWinindow.document.open();
            popupWinindow.document.write('<html> <head>' + '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css"/>' + '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css"/>' + '<link type="text/css" media="print" rel="stylesheet" href="css/print/preadmission/RegReportPdf.css"/>' + '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); },900);">' + connect + '</html>'
            );
            popupWinindow.document.close();
        };
        $scope.exportToExcel = function () {
            var blob = new Blob([document.getElementById('printdataid').innerHTML], {
                type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
            });
            saveAs(blob, "MyTable.xls");
        };
        //=======================================================================================================//
    }
})();