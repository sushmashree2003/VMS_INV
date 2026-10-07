(function () {
    'use strict';
    angular.module('app').controller('ISM_BlockEmployeeController', ISM_BlockEmployeeController);

    ISM_BlockEmployeeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', 'superCache'];
    function ISM_BlockEmployeeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, superCache) {

        $scope.submitted = false;
        $scope.searchValueU = "";
        $scope.disableEmp = false;
        $scope.reverse = true;
        $scope.ismbE_BlockDate = new Date();
        $scope.isebE_UnblockDate = new Date();
        //=====================Load--data.............
        $scope.Loaddata = function () {
            $scope.currentPageU = 1;
            $scope.itemsPerPageU = 10;
            $scope.BlockedDR = true;
            $scope.search = "";
            var data = {};
            apiService.create("ISM_BlockEmployee/getloaddata", data).then(function (promise) {
                if (promise.get_Emplist !== null && promise.get_Emplist.length > 0) {
                    $scope.get_Emplist = promise.get_Emplist;
                }
                $scope.get_blockedEmplist = promise.get_blockedEmplist;
                $scope.blocked = $scope.get_blockedEmplist.lenght;
                if ($scope.get_blockedEmplist.length > 0) {
                    angular.forEach($scope.get_blockedEmplist, function (uem) {                       
                        if (uem.ISMBE_BlockFlg === true) {
                            uem.blockunblock = "Unblock";
                        }
                        else {
                            uem.blockunblock = "View";
                        }
                    });
                }
            });
        };
        $scope.exportToExcel = function () {
            //var excelname = $scope.routename.toLowerCase();
            //excelname = excelname.substring(0, 1).toUpperCase() + excelname.substring(1) + ' Route Status Report.xls';
            var printSectionId = '#table1';
            if ($scope.get_blockedEmplist !== null && $scope.get_blockedEmplist.length > 0) {
                var exportHref = Excel.tableToExcel(printSectionId, 'DR BLOCKED LIST');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = "DR BLOCKED LIST.xls";
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        };

        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.get_Emplist, function (uem) {
                uem.empck = toggleStatus;
            });
        };
        $scope.togchkbx = function () {
            $scope.checkall = $scope.get_Emplist.every(function (options) {
                return options.empck;
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.get_Emplist.some(function (options) {
                return options.empck;
            });
        };

        $scope.tempadd = [];
        $scope.addnewrow = function () {
            if ($scope.myForm.$valid) {
                $scope.unblockflg = 0;
                $scope.tempEmp = [];
                angular.forEach($scope.get_Emplist, function (emp) {
                    if (emp.empck === true) {
                        emp.disableEmp = true;
                        $scope.tempEmp.push({ HRME_Id: emp.HRME_Id, HRMD_Id: emp.HRMD_Id, employeeName: emp.employeeName, HRME_EmployeeCode: emp.HRME_EmployeeCode, HRMD_DepartmentName: emp.HRMD_DepartmentName, MI_Name: emp.MI_Name });
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.delete = function (row, index) {
            for (var x = 0; x < $scope.tempEmp.length; x++) {
                if (x === index) {
                    $scope.tempEmp.splice(x, 1);

                }
            }
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.savedata = function () {
            var data = {};
            var dystring = "";
            if ($scope.unblockflg === 1) {
                dystring = "Unblock";
                data = {
                    "ISEBE_UnblockDate": new Date($scope.isebE_UnblockDate).toDateString(),
                    "ISMBE_Id": $scope.block_Id
                };
            }
            else {
                if ($scope.tempEmp.length > 0) {
                    dystring = "Block";
                    data = {
                        "ISMBE_BlockDate": new Date($scope.ismbE_BlockDate).toDateString(),
                        "ISMBE_Reason": $scope.ismbE_Reason,
                        "temp_blockEmployee": $scope.tempEmp,
                        "ISMBE_BlockFlg": $scope.BlockedDR
                    };
                }
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Selected Employee..?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,  " + dystring + "",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ISM_BlockEmployee/savedata", data).then(function (promise) {
                            if (promise !== null) {
                                if (promise.message === "Save") {
                                    if (promise.returnval === true) {
                                        swal("Blocked successfully.... !!");
                                    } else {
                                        swal("Failed To Block....!");
                                    }
                                    $('#myModalUnblock').hide('hide');
                                    $('.modal-backdrop').remove();
                                }
                                else if (promise.message === "unblock") {
                                    if (promise.returnval === true) {
                                        swal("Unblocked successfully.... !!");
                                    } else {
                                        swal("Failed To Unblock....!");
                                    }
                                    $('#myModalUnblock').modal('hide');
                                    $('.modal-backdrop').remove();
                                } else {
                                    swal("Failed To Update...!");
                                }                              
                                $state.reload();
                            }

                        });
                    }
                    else {
                        swal("" + dystring + " Cancelled");
                    }
                });

        };


        $scope.unblockEmployee = function () {
            $scope.unblockflg = 1;
            $scope.savedata();
        };

        //******************************* Department Change ****************************//
        $scope.viewunblock = function (obju) {
            $scope.block_Id = obju.ISMBE_Id;
            $scope.blockEmp_Id = obju.HRME_Id;
            var data = {
                ISMBE_Id: obju.ISMBE_Id
            };
            apiService.create("ISM_BlockEmployee/getblockedEmpDeatils", data).then(function (promise) {
                if (promise.get_blockedEmpDeatils.length > 0) {
                    $scope.get_blockedEmpDeatils = promise.get_blockedEmpDeatils;
                    $scope.emplname = $scope.get_blockedEmpDeatils[0].empname;
                    $scope.emplcode = $scope.get_blockedEmpDeatils[0].hrmE_EmployeeCode;
                    $scope.blockeddate = new Date($scope.get_blockedEmpDeatils[0].ismbE_BlockDate);
                    $scope.unblockeddate = new Date($scope.get_blockedEmpDeatils[0].isebE_UnblockDate);
                    $scope.blockedreason = $scope.get_blockedEmpDeatils[0].ismbE_Reason;
                }
                else {
                    swal("No Record  Found..... !!");
                    $scope.get_blockedEmpDeatils = "";
                }
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };

    }
})();

