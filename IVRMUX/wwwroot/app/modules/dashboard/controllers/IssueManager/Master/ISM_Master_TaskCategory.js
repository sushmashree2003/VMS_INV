(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_Master_TaskCategory', ISM_Master_TaskCategory)

    ISM_Master_TaskCategory.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce'];
    function ISM_Master_TaskCategory($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce) {

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.submitted = false;  
        //======================page load
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("ISM_Master_TaskCategory/loaddata", pageid).then(function (promise) {               
                $scope.departmentlist = promise.departmentlist;
                $scope.alldata1 = promise.alldata1;
            })
        };
        //======================Record Save
        $scope.search = "";
        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return (angular.lowercase(obj.asmaY_Year)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }
        $scope.filterchkbx = function (obj) {
            return (angular.lowercase(obj.hrmD_DepartmentName)).indexOf(angular.lowercase($scope.search)) >= 0;
        }

        $scope.save = function () {          
            if ($scope.myForm.$valid) {
                if ($scope.ISMMTCAT_TaskPercentage <= 100) {
                    if ($scope.ISMMTCAT_CompulsoryFlg == '1') {

                        $scope.ISMMTCAT_CompulsoryFlg = true;
                    }
                    if ($scope.ISMMTCAT_CompulsoryFlg == '0') {
                        $scope.ISMMTCAT_CompulsoryFlg = false;
                    }

                    if ($scope.ISMMTCAT_EachTaskMaxDuration == "" && $scope.ISMMTCAT_EachTaskMaxDuration == undefined) {
                        $scope.ISMMTCAT_EachTaskMaxDuration = "0";
                    }


                    var data = {
                        "ISMMTCAT_Id": $scope.ISMMTCAT_Id,
                        "HRMD_Id": $scope.HRMD_Id.hrmD_Id,
                        "ISMMTCAT_TaskCategoryName": $scope.ISMMTCAT_TaskCategoryName,
                        "ISMMTCAT_TaskPercentage": $scope.ISMMTCAT_TaskPercentage,
                        "ISMMTCAT_TCRemarks": $scope.ISMMTCAT_TCRemarks,
                        "ISMMTCAT_EachTaskMaxDuration": $scope.ISMMTCAT_EachTaskMaxDuration,
                        "ISMMTCAT_DurationFlg": $scope.ISMMTCAT_DurationFlg,
                        "ISMMTCAT_CompulsoryFlg": $scope.ISMMTCAT_CompulsoryFlg,
                    }

                    apiService.create("ISM_Master_TaskCategory/save", data).then(function (promise) {
                        if (promise.msg == 'taskpercentagegreater') {
                            swal("Task Percentage is More than 100% ...!!!")                            
                        }

                        else if (promise.msg == 'saved') {
                            swal("Data Saved Successfully...!!!")
                            $state.reload();
                        }
                        else if (promise.msg == 'Failed') {
                            swal("Data Not Saved Successfully...!!!");
                        }
                        else if (promise.msg == 'updated') {
                            swal("Data Updated Successfully...!!!");
                            $state.reload();
                        }
                        else if (promise.msg == 'Failed') {
                            swal("Data Not Updated Successfully...!!!");
                        }
                        else if (promise.returnval == true) {
                            swal("Data Already Exists");
                        }
                        else {
                            swal("Something is Wrong......");
                        }
                    })
                }
                else {
                    swal("Task Pecentage Should be lesser than or equal to 100%...")
                }
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.deactivYTab1 = function (usersem, SweetAlert) {         
            var dystring = "";
            if (usersem.ismmtcaT_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (usersem.ismmtcaT_ActiveFlag == false) {
                dystring = "Activate"
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
                        apiService.create("ISM_Master_TaskCategory/deactive", usersem).
                            then(function (promise) {
                                if (promise.ret == true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                                else {

                                    swal("Record Not " + dystring + "d " + "Successfully!!!");
                                    $state.reload();
                                }
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }

                });
        }
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;   //set the propertyName to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //================================edit data
        $scope.edittab1 = function (user) {
          
            var data = {
                "ISMMTCAT_Id": user.ismmtcaT_Id,                
            }
            apiService.create("ISM_Master_TaskCategory/EditData", data).then(function (promise) {
               
                if (promise.editlist.length > 0) {

                    $scope.flg = true;
                        $scope.ISMMTCAT_Id = promise.editlist[0].ismmtcaT_Id;
                        $scope.ISMMTCAT_TaskCategoryName = promise.editlist[0].ismmtcaT_TaskCategoryName;
                        $scope.ISMMTCAT_TaskPercentage = promise.editlist[0].ismmtcaT_TaskPercentage;
                    $scope.HRMD_Id = promise.editlist[0];
                    $scope.hrmD_DepartmentName = promise.editlist[0].HRMD_DepartmentName;
                    $scope.mI_Name = promise.editlist[0].MI_Name;
                    $scope.ISMMTCAT_DurationFlg = promise.editlist[0].ismmtcaT_DurationFlg;
                        $scope.ISMMTCAT_TCRemarks = promise.editlist[0].ismmtcaT_TCRemarks;
                    $scope.ISMMTCAT_EachTaskMaxDuration = promise.editlist[0].ismmtcaT_EachTaskMaxDuration;
                    $scope.ISMMTCAT_CompulsoryFlg = promise.editlist[0].ismmtcaT_CompulsoryFlg;
                }
            })
        }
        //$scope.exportToExcel = function (table) {
        //    var exportHref = Excel.tableToExcel(table, 'sheet name');
        //    $timeout(function () { location.href = exportHref; }, 100);
        //}

        $scope.exportToExcel = function () {
            
            var printSectionId = '#table1';           
                var exportHref = Excel.tableToExcel(printSectionId, 'CATEGORY LIST');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = "CATEGORY LIST.xls";
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
           
        };
    }   
})();

