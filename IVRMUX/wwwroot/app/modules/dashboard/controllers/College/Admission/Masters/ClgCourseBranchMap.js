(function () {
    'use strict';

    angular
        .module('app')
        .controller('ClgMasterCourseBranchMapController', ClgMasterCourseBranchMapController);

    ClgMasterCourseBranchMapController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout'];

    function ClgMasterCourseBranchMapController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {
        $scope.searc_button = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }


        $scope.chkcurslst = function (oobj) {
            
            angular.forEach($scope.MasterCourseList, function (itm) {
                if (itm.amcO_Id == oobj) {
                    $scope.noofsemlsst = itm.amcO_NoOfSemesters;
                }
            })
        }

        $scope.selcnt = {};
        $scope.chksemlsst = function (objid) {
            
            var cnt = 0;
            angular.forEach($scope.mastersemesterlist, function (itmm) {
                if (itmm.Selected1 == true) {
                    cnt++;
                    $scope.selcnt = cnt;
                    if (cnt > $scope.noofsemlsst) {
                        swal("you can select only" + " " + $scope.noofsemlsst + " " + "semester");
                        angular.forEach($scope.mastersemesterlist, function (tstobj) {
                            if (tstobj.amsE_Id == objid) {
                                tstobj.Selected1 = false;
                            }
                        })
                        return;
                    }
                }
            })
        }

        $scope.getAllDetails = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var id = 2;

            
            apiService.getURI("ClgMasterCourseBranchMap/getalldetails", id).
    then(function (promise) {
        
        $scope.MasterCourseList = promise.masterCourseList;
        $scope.Masterbranch = promise.masterbranchList;
        $scope.mastersemesterlist = promise.mastersemesterlist;
        $scope.griddetails = promise.grid;
        $scope.presentCountgrid = $scope.griddetails.length;
    })

        }


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.mastersemesterlist.some(function (options) {
                return options.Selected1;
            });
        }
        $scope.selectedSectionList = [];

        $scope.savedata = function () {
            
            if ($scope.myForm.$valid) {

                if ($scope.selcnt != $scope.noofsemlsst) {
                    swal("Please select any " + " " + $scope.noofsemlsst + " " + "semester");
                    return;
                }

                if ($scope.mastersemesterlist != "" && $scope.mastersemesterlist != null) {
                    if ($scope.mastersemesterlist.length > 0) {
                        for (var i = 0; i < $scope.mastersemesterlist.length; i++) {
                            if ($scope.mastersemesterlist[i].Selected1 == true) {
                                $scope.selectedSectionList.push($scope.mastersemesterlist[i]);
                            }
                        }
                    }
                }

                var data = {
                    "AMCOBM_Id": $scope.AMCOBM_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "semester_list": $scope.selectedSectionList
                }
                apiService.create("ClgMasterCourseBranchMap/Savedetails", data).then(function (promise) {
                    

                    if (promise.message == "Add") {
                        if (promise.returnval == true) {
                            swal("Record Saved Successfully");
                        } else {
                            swal("Failed To Save Record");
                        }
                    }
                    else if (promise.message == "Update") {
                        if (promise.returnval == true) {
                            swal("Record Updated Successfully");
                        } else {
                            swal("Failed To Update Record");
                        }
                    } else if (promise.message == "Exists") {
                        swal("You Can Not Map Same Branch For Multiple Course");
                    }
                    else {
                        swal(" Something Went Wrong . Kindly Contact Administrator!!!");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.editflag1 = false;
        $scope.edit = function (user) {

            angular.forEach($scope.mastersemesterlist, function (se) {
                se.Selected1 = false;

            })


            apiService.create("ClgMasterCourseBranchMap/edit", user).then(function (promise) {
                $scope.editdata = promise.editdata;
                $scope.editflag1 = true;
                angular.forEach($scope.mastersemesterlist, function (sec1) {
                    sec1.Selected1 = false;
                    sec1.editflag = false;
                })

                angular.forEach($scope.editdata, function (mas) {
                    angular.forEach($scope.mastersemesterlist, function (sec) {
                        if (sec.amsE_Id == mas.amsE_Id) {
                            sec.Selected1 = true;
                            sec.editflag = true;
                        }
                    })
                })
                $scope.AMCO_Id = user.amcO_Id;
                $scope.AMB_Id = user.amB_Id;
                $scope.AMCOBM_Id = user.amcobM_Id;
                $scope.chkcurslst($scope.AMCO_Id);
            })
        }

        //view details
        $scope.showmodaldetails = function (user) {
            apiService.create("ClgMasterCourseBranchMap/showmodaldetails", user).then(function (Promise) {
                $scope.semesterbrancklist = Promise.semesterbrancklist;
            })
        }

        //deactive and active semester
        $scope.deactivesem = function (usersem, SweetAlert) {
            
            var dystring = "";
            if (usersem.amcobmS_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (usersem.amcobmS_ActiveFlg == false) {
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
                    apiService.create("ClgMasterCourseBranchMap/deactivesem", usersem).
                   then(function (promise) {
                       if (promise.returnval == true) {
                           

                           swal("Record " + dystring + "Successfully!!!");
                           $state.reload();

                       }
                       else {
                           
                           swal("Record Not " + dystring + " Successfully!!!");
                           $state.reload();
                       }

                   })
                }
                else {
                    swal("Record Deactivation Cancelled!!!");
                }

            });
        }

        //For Cancel data record 
        $scope.Clearid = function () {
            $state.reload();
        }

        $scope.interacted = function (field) {
            return $scope.submitted ;
        };


        $scope.searchValue = "";
        $scope.filterValue123 = function (obj) {
            
            return (angular.lowercase(obj.amcO_CourseName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.amB_BranchName)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

        //$scope.resultClick = function () {
        //    $scope.AMCOBM_Id = item.amcobM_Id;
        //}

    }
})();
