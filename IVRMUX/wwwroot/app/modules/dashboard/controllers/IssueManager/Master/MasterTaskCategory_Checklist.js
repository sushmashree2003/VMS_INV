(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterTaskCategory_Checklistcontroller', MasterTaskCategory_Checklistcontroller)

    MasterTaskCategory_Checklistcontroller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce'];
    function MasterTaskCategory_Checklistcontroller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce) {

        $scope.editflag = false;
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.submitted = false;
        $scope.flg = false;
        $scope.showgridsave = true;
        $scope.searchchkbx = "";
        $scope.searchchkbxtype = "";
        $scope.search = "";

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("ISM_Master_TaskCategory/loaddata_checklist", pageid).then(function (promise) {
                $scope.departmentlist = promise.departmentlist;
                $scope.checklist = promise.checklist;

                $scope.checklist_report = [];
                $scope.checklist_report = $scope.checklist.filter(function (item) {
                    return item.ismmtcatcL_ActiveFlag == true;
                });

                console.log($scope.checklist_report);
            });
        };

        $scope.getcategory = function (cc) {
            angular.forEach($scope.departmentlist, function (dd) {
                if (dd.hrmD_Id === parseInt(cc)) {
                    $scope.tempMI_Id = parseInt(dd.mI_Id);
                }
            });
            var data = {
                "HRMD_Id": cc,
                "MI_Id": $scope.tempMI_Id
            };

            apiService.create("ISM_Master_TaskCategory/getcategory", data).then(function (promise) {
                if (promise.categorylist !== null || promise.categorylist > 0) {
                    $scope.categorylist = promise.categorylist;
                }
                else {
                    swal('No Data Found..!!');
                }
            });
        };

        $scope.save = function () {
            if ($scope.myForm.$valid) {
                $scope.arraypush = [];
                $scope.finalcount = 0;
                angular.forEach($scope.transrows, function (pr) {
                    $scope.arraypush.push({
                        ISMMTCATCL_CheckListName: pr.ISMMTCATCL_CheckListName,
                        ISMMTCATCL_UploadEnterFlg: pr.ISMMTCATCL_UploadEnterFlg
                    });
                });

                var data = {
                    "ISMMTCATCL_Id": $scope.ISMMTCATCL_Id,
                    "HRMD_Id": $scope.HRMD_Id.hrmD_Id,
                    "ISMMTCAT_Id": $scope.ISMMTCAT_Id,
                    "checklistarray": $scope.arraypush,
                    "MI_Id": $scope.tempMI_Id
                };
                apiService.create("ISM_Master_TaskCategory/savechecklist", data).then(function (promise) {

                    if (promise.message === "Add") {
                        swal("Data Saved Successfully...!!!");
                    }

                    else if (promise.message === "update") {
                        swal("Data Updated Successfully...!!!");
                    }
                    else if (promise.message === "Error") {
                        swal("Something is Wrong......");
                    }
                    $state.reload();

                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.deactivYTab1 = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.ismmtcatcL_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (usersem.ismmtcatcL_ActiveFlag === false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ISM_Master_TaskCategory/deactive_checklist", usersem).then(function (promise) {
                            if (promise.message === 'true') {
                                swal("Record Activated Successfully!!!");

                            }
                            else if (promise.message === 'false') {
                                swal("Record De-Activated Successfully!!!");

                            }
                            else if (promise.message === 'Error') {
                                swal("Operation Failed..!!!");
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;
            $scope.reverse = !$scope.reverse;
        };

        $scope.edittab1 = function (user) {
            var data = {
                "ISMMTCATCL_Id": user.ismmtcatcL_Id
            };
            apiService.create("ISM_Master_TaskCategory/EditData_checklist", data).then(function (promise) {

                if (promise.checklistedit !== null && promise.checklistedit.length > 0) {
                    $scope.editflag = true;
                    $scope.ISMMTCAT_Id = promise.checklistedit[0].ismmtcaT_Id;
                    $scope.HRMD_Id = promise.departmentlistedit[0].hrmD_Id;
                    $scope.ISMMTCATCL_Id = promise.checklistedit[0].ismmtcatcL_Id;
                    $scope.departmentlist = promise.departmentlist;
                    $scope.categorylist = promise.categorylist;
                    $scope.transrows = [];

                    angular.forEach(promise.checklistedit, function (pr) {
                        $scope.UploadEnterFlg = pr.ismmtcatcL_UploadEnterFlg;
                        $scope.transrows.push({
                            ISMMTCATCL_CheckListName: pr.ismmtcatcL_CheckListName,
                            ISMMTCATCL_UploadEnterFlg: $scope.UploadEnterFlg
                        });
                    });
                }
            });
        };

        $scope.transrows = [{ itrS_Id: 'trans1', ISMMTCATCL_UploadEnterFlg: false }];
        if ($scope.transrows.length === 1) {
            $scope.cnt = 1;
        }

        $scope.addprrows = function (objs) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.uploadflag = objs.ISMMTCATCL_UploadEnterFlg === undefined ? false : objs.ISMMTCATCL_UploadEnterFlg;

                if ($scope.transrows.length > 1) {
                    for (var i = 0; i == $scope.transrows.length; i++) {
                        var id = $scope.transrows[i].itrS_Id;
                        var lastChar = id.substr(id.length - 1);
                        $scope.cnt = parseInt(lastChar);
                    }
                }

                $scope.cnt = $scope.cnt + 1;
                $scope.tet = 'trans' + $scope.cnt;
                var newItemNo = $scope.cnt;
                $scope.transrows.push({ 'itrS_Id': 'trans' + newItemNo, ISMMTCATCL_UploadEnterFlg: false });
            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.removeprrows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);

            if (data.amstB_Id > 0) {
                $scope.Deleteprrows(data);
            }
            if ($scope.transrows.length === 0) {
                //dd
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.exportToExcel = function () {            
            var printSectionId = '#exportotexcel';
            if ($scope.checklist_report !== null && $scope.checklist_report.length > 0) {
                var exportHref = Excel.tableToExcel(printSectionId, 'Task Category Check List');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = "Task Category Check List.xls";
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        };
    }
})();

