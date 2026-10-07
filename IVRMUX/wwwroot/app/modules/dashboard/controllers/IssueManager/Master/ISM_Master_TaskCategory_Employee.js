(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_Master_TaskCategory_Employee', ISM_Master_TaskCategory_Employee)

    ISM_Master_TaskCategory_Employee.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce'];
    function ISM_Master_TaskCategory_Employee($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce) {

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.submitted = false;
        //======================page load
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("ISM_Master_TaskCategory_Employee/loaddata", pageid).then(function (promise) {
            
                $scope.departmentlist = promise.departmentlist;
                $scope.alldata1 = promise.alldata1;
            })
        };
        $scope.searchchkbx = "";
        $scope.searchchkbxtype = "";
        $scope.all_check = function () {

            var checkStatus = $scope.usercheck;
            angular.forEach($scope.categorylist, function (itm) {
                itm.select3 = checkStatus;
            });
        }
        $scope.all_checktype = function () {

            var checkStatus = $scope.userchecktype;
            angular.forEach($scope.employeelist, function (itmtype) {
                itmtype.select1 = checkStatus;
            })
        }
        $scope.filterchkbx = function (obj) {
            return (angular.lowercase(obj.ismmtcaT_TaskCategoryName)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }
        $scope.filterchkbxtype = function (obj) {
            return (angular.lowercase(obj.hrmE_EmployeeFirstName)).indexOf(angular.lowercase($scope.searchchkbxtype)) >= 0;
        }
        $scope.togchkbx = function () {

            $scope.usercheck = $scope.categorylist.every(function (options) {
                return options.select3;
            });
        }
        $scope.togchkbxtype = function () {
            $scope.userchecktype = $scope.employeelist.every(function (optionstype) {
                return optionstype.select1;
            })
        }
        $scope.isOptionsRequired = function () {
            return !$scope.categorylist.some(function (options) {
                return options.select3;
            });
        }
        $scope.isOptionsRequiredtype = function () {
            return !$scope.employeelist.some(function (options) {
                return options.select1;
            })
        }
        $scope.search = "";
        $scope.getcategory = function () {          
            var data = {
                "HRMD_Id": $scope.HRMD_Id.hrmD_Id
            }
            apiService.create("ISM_Master_TaskCategory_Employee/getcategory", data).then(function (promise) {               
                $scope.categorylist = promise.categorylist;
                angular.forEach($scope.categorylist, function (aqa) {
                    if (aqa.ismmtcaT_CompulsoryFlg == true) {
                        aqa.select3 = true;
                    }
                })  
                $scope.employeelist = promise.employeelist;
            })
        }
        //======================Record Save
        $scope.search = "";
        $scope.save = function () {
           
                    if ($scope.myForm.$valid) {
                    $scope.selectedcategorylist = [];
                        $scope.selectedemployeelist = [];
                        $scope.selectedcategorylist2222 = [];
                        angular.forEach($scope.categorylist, function (cat) {
                            if (cat.select3 == true) {
                                $scope.selectedcategorylist.push({ ismmtcaT_Id: cat.ismmtcaT_Id, ismmtcaT_TaskCategoryName: cat.ismmtcaT_TaskCategoryName });
                            }
                        });
                        angular.forEach($scope.employeelist, function (emp) {
                            if (emp.select1 == true) {
                                $scope.selectedemployeelist.push({ hrmE_Id: emp.hrmE_Id, hrmE_EmployeeFirstName: emp.hrmE_EmployeeFirstName });
                            }
                        })

              
                        angular.forEach($scope.emplcategorylist, function (add) {
                            if (add.isSelected22 == true) {
                                add.ISMMTCATE_CompulsoryFlg = true;
                            }
                            else {
                                add.ISMMTCATE_CompulsoryFlg = false;
                            }
                        })

                        //angular.forEach($scope.emplcategorylist, function (add) {
                        //    angular.forEach($scope.selectedemployeelist, function (emplist) {
                        //        angular.forEach($scope.selectedcategorylist, function (catlist) {
                        //            if (add.isSelected22 == true && emplist.select1 == true && catlist.select3 == true  && add.hrmE_Id == emplist.hrmE_Id && add.ismmtcaT_Id == catlist.ismmtcaT_Id) {
                        //                $scope.selectedcategorylist2222.push({ ismmtcaT_Id: catlist.ismmtcaT_Id, ismmtcaT_TaskCategoryName: catlist.ismmtcaT_TaskCategoryName, hrmE_EmployeeFirstName: emplist.hrmE_EmployeeFirstName, hrmE_Id: emplist.hrmE_Id, ISMMTCATE_CompulsoryFlg: true });
                                       
                        //            }
                        //            else {
                                       
                        //                $scope.selectedcategorylist2222.push({ ismmtcaT_Id: catlist.ismmtcaT_Id, ismmtcaT_TaskCategoryName: catlist.ismmtcaT_TaskCategoryName, hrmE_EmployeeFirstName: emplist.hrmE_EmployeeFirstName, hrmE_Id: emplist.hrmE_Id, ISMMTCATE_CompulsoryFlg: false });
                        //            }

                        //        })
                        //    })
                        //})

                  
                   
                    var data = {
                        "ISMMTCATE_Id": $scope.ISMMTCATE_Id,
                        selectedcategorylist: $scope.selectedcategorylist,
                        selectedemployeelist: $scope.selectedemployeelist,
                        emplcategorylist: $scope.emplcategorylist,
                        //"total": $scope.total,
                    }
                        apiService.create("ISM_Master_TaskCategory_Employee/save", data).then(function (promise) {                           

                        if (promise.message == "save") {
                            swal("Data Saved Successfully...!!!")
                            $state.reload();
                        }
                        else if (promise.message == "nosave") {
                            swal("Data Not Saved Successfully...!!!");
                        }
                        else if (promise.message == "update") {
                            swal("Data Updated Successfully...!!!");
                            $state.reload();
                        }
                        else if (promise.message == "noupdate") {
                            swal("Data Not Updated Successfully...!!!");
                        }
                        else if (promise.message == "duplicate") {
                            swal("Data Already Exists");
                        }
                        else {
                            swal("Something is Wrong......");
                        }
                    })
                }
            else {
                    $scope.submitted = true;
                }
        }

        $scope.deactivYTab1 = function (usersem, SweetAlert) {            
            var dystring = "";
            if (usersem.ismmtcatE_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (usersem.ismmtcatE_ActiveFlag == false) {
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
                        apiService.create("ISM_Master_TaskCategory_Employee/deactive", usersem).
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
        $scope.flg = false;
        $scope.showgridsave = true;
        //================================edit data
        $scope.edittab1 = function (user) {
            $scope.flg = true;
            if (user.ismmtcatE_ActiveFlag == false) {
                swal("Can't Update Category...!!!");
            }
            if (user.ismmtcatE_ActiveFlag == true) {
                var data = {
                    "ISMMTCATE_Id": user.ismmtcatE_Id,
                    "HRMD_Id": user.hrmD_Id,
                    "HRME_Id": user.hrmE_Id,
                }
                apiService.create("ISM_Master_TaskCategory_Employee/EditData", data).then(function (promise) {
                  
                    if (promise.editlist.length > 0) { 
                        $scope.selectflg = promise.selectflg;
                        $scope.ISMMTCATE_Id = promise.editlist[0].ismmtcatE_Id;
                        $scope.ISMMTCAT_TaskCategoryName = promise.editlist[0].ismmtcaT_TaskCategoryName;
                        $scope.ISMMTCAT_Id = promise.editlist[0].ismmtcaT_Id;
                        $scope.HRME_Id = promise.editlist[0].hrmE_Id;
                        $scope.HRME_EmployeeFirstName = promise.editlist[0].hrmE_EmployeeFirstName;
                        $scope.ISMMTCATE_TaskPercentage = promise.editlist[0].ismmtcatE_TaskPercentage;
                        $scope.HRMD_Id = promise.editlist[0];
                        $scope.HRMD_DepartmentName = promise.editlist[0].hrmD_DepartmentName; 

                        $scope.catlistselected= promise.catlistselected;
                        angular.forEach(promise.categorylist, function (tt) {
                            angular.forEach(promise.catlistselected, function (ttt) {
                                if (tt.ismmtcaT_Id == ttt.ismmtcaT_Id) {
                                tt.select3 = true;
                            }
                        })
                        })

                        $scope.categorylist =promise.categorylist;
                        angular.forEach(promise.employeelist, function (tt) {
                            angular.forEach(promise.editlist, function (ss) {
                                if (tt.hrmE_Id == ss.hrmE_Id) {
                                    tt.select1 = true;
                                }
                            })
                        })
                        $scope.employeelist = promise.employeelist;                       
                    }
                })
            }
        }
        $scope.showgrid = false;
        $scope.add = function () {
          
            if ($scope.ISMMTCATE_Id > 0) {
                $scope.showgridsave = false;
                $scope.showgrid = true;
                $scope.selectedcategorylist = [];
                $scope.selectedemployeelist = [];
                angular.forEach($scope.categorylist, function (cat) {
                    if (cat.select3 == true) {
                        $scope.selectedcategorylist.push({ ismmtcaT_Id: cat.ismmtcaT_Id, ismmtcaT_TaskCategoryName: cat.ismmtcaT_TaskCategoryName });
                    }
                });
                angular.forEach($scope.employeelist, function (emp) {
                    if (emp.select1 == true) {
                        $scope.selectedemployeelist.push({ hrmE_Id: emp.hrmE_Id, hrmE_EmployeeFirstName: emp.hrmE_EmployeeFirstName });
                    }
                })
                $scope.emplcategorylist1 = [];
                $scope.emplcategorylist = [];
            

                angular.forEach($scope.selectedcategorylist, function (catid) {
                    angular.forEach($scope.selectedemployeelist, function (empid) {
                        $scope.emplcategorylist1.push({ ismmtcaT_Id: catid.ismmtcaT_Id, ismmtcaT_TaskCategoryName: catid.ismmtcaT_TaskCategoryName, hrmE_Id: empid.hrmE_Id, hrmE_EmployeeFirstName: empid.hrmE_EmployeeFirstName })
                    })
                })
                $scope.emplcategorylist = $scope.emplcategorylist1;
         
                
                angular.forEach($scope.selectflg, function (vv) {
                 
                    angular.forEach($scope.emplcategorylist, function (vvv) {
                        
                        if (vv.hrmE_Id == vvv.hrmE_Id && vv.ismmtcaT_Id == vvv.ismmtcaT_Id) {
                            vvv.isSelected22 = true;
                        }
                        
                    })
                })
            }
            else {
                if ($scope.HRMD_Id != undefined) {
                    if ($scope.myForm.$valid) {

                        $scope.showgrid = true;
                        $scope.showgridsave = false;

                        $scope.selectedcategorylist = [];
                        $scope.selectedemployeelist = [];
                        angular.forEach($scope.categorylist, function (cat) {
                            if (cat.select3 == true) {
                                $scope.selectedcategorylist.push({ ismmtcaT_Id: cat.ismmtcaT_Id, ismmtcaT_TaskCategoryName: cat.ismmtcaT_TaskCategoryName });
                            }
                        });
                        angular.forEach($scope.employeelist, function (emp) {
                            if (emp.select1 == true) {
                                $scope.selectedemployeelist.push({ hrmE_Id: emp.hrmE_Id, hrmE_EmployeeFirstName: emp.hrmE_EmployeeFirstName });
                            }
                        })
                        $scope.emplcategorylist = [];
                       

                        angular.forEach($scope.selectedcategorylist, function (catid) {
                            angular.forEach($scope.selectedemployeelist, function (empid) {
                                $scope.emplcategorylist.push({ ismmtcaT_Id: catid.ismmtcaT_Id, ismmtcaT_TaskCategoryName: catid.ismmtcaT_TaskCategoryName, hrmE_Id: empid.hrmE_Id, hrmE_EmployeeFirstName: empid.hrmE_EmployeeFirstName })
                            })
                        })
                        $scope.emplcategorylist;
                    }
                    else {
                        $scope.submitted = true;
                    }
                }
                else {
                    swal("Please Select Department");
                }
            }

        }
        $scope.optionToggled = function () {
            $scope.usercheckadd = $scope.emplcategorylist.every(function (itm) { return itm.isSelected22; });
            if ($scope.usercheckadd == true) {
                $scope.usercheckadd = true;
            }
            if ($scope.usercheckadd == false) {
                $scope.usercheckadd = false;
            }
        };
        $scope.toggleAll = function (usercheckadd) {
            if (usercheckadd == true) {
                var toggleStatus = usercheckadd;
                angular.forEach($scope.emplcategorylist, function (itm) {
                    itm.isSelected22 = toggleStatus;
                });
            }
            else if (usercheckadd == false) {
                var toggleStatus = usercheckadd;
                angular.forEach($scope.emplcategorylist, function (itm) {
                    itm.isSelected22 = toggleStatus;
                });
            }
        };
        $scope.exportToExcel = function (table) {
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }
    }
})();

