(function () {
    'use strict';
    angular
        .module('app')
        .controller('IVRM_VersionTransactionController', IVRM_VersionTransactionController)

    IVRM_VersionTransactionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce', 'Excel', '$timeout']
    function IVRM_VersionTransactionController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce, Excel, $timeout) {

        $scope.submitted = false;
        $scope.setremind = false;
        $scope.disabledata = false;
        $scope.presentCountgrid = 0;

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 10;
        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;
        $scope.loaddata = function () {

            $scope.search = "";
            var pageid = 2;
            apiService.getURI("IVRM_VersionControlProcess/VTgetdetails", pageid).then(function (promise) {
                $scope.clientHosting = promise.clientHosting;
                $scope.getversionapprovedlist = promise.getversionapprovedlist;
               
                $scope.griddata = promise.getgriddata;
                $scope.presentCountgrid = promise.getgriddata.length;

            });
        }; 
        $scope.getclientlist = function (task) {

            var data = {
                "IVRMCH_Id": task.ivrmcH_Id
            };
            apiService.create("IVRM_VersionControlProcess/VTgettasklist", data).then(function (promise) {
                if (promise.clientList.length > 0) {
                    $scope.clientList = promise.clientList;                   

                }
            });
        };
        $scope.gettasklist = function (task) {

            var data = {
                "IVRMCH_Id": task.ivrmcH_Id
            };
            apiService.create("IVRM_VersionControlProcess/VTgettasklist", data).then(function (promise) {
                if (promise.taskidlist.length > 0) {
                    $scope.clientList = promise.clientList;
                    $scope.taskidlist = promise.taskidlist;
                    
                }
            });
        };


        //========================================= Save
        $scope.obj = {};
        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {             
                $scope.getFeaturelist = [];
                angular.forEach($scope.taskidlist, function (emp) {
                    if (emp.selected == true) {
                        $scope.getFeaturelist.push({ ISMTCR_Id: emp.ISMTCR_Id });
                    }

                });
                var data = {
                    "IVRMVT_Id": $scope.IVRMVT_Id,
                    "IVRMCH_Id": $scope.ivrmcH_Id.ivrmcH_Id,
                    "IVRMVT_DeployementDate": new Date($scope.IVRMVT_DeployementDate).toDateString(),
                    getFeaturelist: $scope.getFeaturelist
                };
                apiService.create("IVRM_VersionControlProcess/VTsave", data).then(function (promise) {
                    if (promise.returnval === "Add") {
                        swal('Record Saved successfully');
                        $state.reload();
                        //$scope.clickreminder();
                    }
                    else if (promise.returnval === 'Update') {
                        swal('Record Updated successfully');
                        $state.reload();
                        //$scope.clickreminder();
                    }
                    else if (promise.returnval === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        swal('Failed, please contact administrator');
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        // save approved deployment version 

        $scope.deploymentcompletd = function (IVRMVT_Id, date) {
            if ($scope.myForm.$valid){
                var info = {
                    "IVRMVT_Id": IVRMVT_Id,
                    "IVRMVT_DeployementDate": new Date(date).toDateString()
                }
                apiService.create("IVRM_VersionControlProcess/savedeployment", info).then(function (promise) {
                    if (promise.returnval == "Updated") {
                        swal("Deployment Saved Successfully", " ", "success");
                        $scope.loaddata();
                    }
                    else if (promise.returnval == "Scope Approval Is Pending")
                    {
                        swal("Still The Scope Of Deployment Was Not Approved ","To Proceed Kindly Take The Approval","info");
                    }
                    else {
                        swal("Failed To Save", " ", "warning");
                        $scope.loaddata();
                    }

                })
            }
                else {
                $scope.submitted = true;
            }
           
        }

        //--Active Deactive---//
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (user.ivrmvT_ActiveFlag == true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";

            }
            else {

                mgs = "Active";
                confirmmgs = "Activated";
            }
            var date = {
                "IVRMVT_Id": user.ivrmvT_Id
            }

            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("IVRM_VersionControlProcess/VTactivedeactive/", date).
                            then(function (promise) {
                                if (promise.message != null) {
                                    swal(promise.message);
                                }
                                else {
                                    if (promise.returnval == "Updated") {
                                        swal(confirmmgs + " " + "Successfully.");
                                        $state.reload();
                                    }
                                    else {
                                        swal(confirmmgs + " " + " Successfully");
                                    }
                                }
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }


        ////=====================Edit record....
        //$scope.edit = function (task) {
        //    var data = {
        //        "IVRMVT_Id": task.ivrmvT_Id
        //    };
        //    apiService.create("IVRM_VersionControlProcess/VTeditdata", data).then(function (promise) {
        //        if (promise.taskidlist.length > 0) {
        //            $scope.taskiddetails = promise.taskidlist;
        //            $scope.gettasklist(task);

        //            angular.forEach($scope.taskiddetails, function (a) {
        //                $scope.taskidlist.push({ ISMTCR_Id: a.ISMTCR_Id, ISMTCR_TaskNo: a.ISMTCR_TaskNo, ISMTCR_Title: a.ISMTCR_Title, ISMTCR_Desc: a.ISMTCR_Desc, ISMTCR_Status: a.ISMTCR_Status, ISMTCR_CreationDate: a.ISMTCR_CreationDate});
        //                //$scope.taskidlist.push({ ISMTCR_Id: a.ISMTCR_Id, ISMTCR_TaskNo: a.ISMTCR_TaskNo, ISMTCR_Title: a.ISMTCR_Title, ISMTCR_Desc: a.ISMTCR_Desc, ISMTCR_Status: a.ISMTCR_Status, ISMTCR_CreationDate: a.ISMTCR_CreationDate, selected: true });
        //            });
        //            $scope.ivrmcH_Id = task;
        //            $scope.IVRMVT_Id = task.ivrmvT_Id;
        //            $scope.IVRMVT_DeployementDate = new Date(task.ivrmvT_DeployementDate);
        //            angular.forEach($scope.taskiddetails, function (emp1) {
        //                angular.forEach($scope.taskidlist, function (emp2) {
        //                    if (emp1.ISMTCR_Id == emp2.ISMTCR_Id) {
        //                        emp2.selected = true;
        //                    }
        //                    else {
        //                        emp2.selected = false;
        //                    }

        //                });

        //            });

        //            $scope.taskidlist = $scope.taskidlist;
        //        }
        //    });
        //};


        $scope.viewdocument = function (task) {
            var data = {
                "IVRMVT_Id": task.ivrmvT_Id
            };
            apiService.create("IVRM_VersionControlProcess/VTeditdata", data).then(function (promise) {
                if (promise.taskidlist.length > 0) {
                    $scope.HostName = task.ivrmcH_HostName;
                    $scope.HostUrl = task.ivrmcH_HostUrl;
                    $scope.VersionName = task.ivrmvT_VersionName;

                    $scope.taskiddetails = promise.taskidlist;

                    $('#popup11').modal('show');

                }
                else {
                    swal("No Record Found!")
                }
            });
        };

        //=================Active/Deactivate
        $scope.deactiveqq = function (task) {
            $scope.ISMTCR_Id = task.ismtcR_Id;
            var dystring = "";
            if (task.ismtcR_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (task.ismtcR_ActiveFlg === false) {
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
                        apiService.create("IVRM_VersionControlProcess/deactive", task).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        };


        $scope.toggleAll = function () {
            var toggleStatus = $scope.all2;
            angular.forEach($scope.taskidlist, function (itm) {
                itm.selected = toggleStatus;
            });
        }



        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all2 = $scope.taskidlist.every(function (itm) { return itm.selected; });
            //if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
            //    $scope.printstudents.push(SelectedStudentRecord);
            //}
            //else {
            //    $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            //}

        }


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.cancel = function () {
            $state.reload();
        };

        $scope.all_check_Emp = function () {
            var toggleStatuss = $scope.clientid;
            angular.forEach($scope.getclientlist, function (itm) {
                itm.selected = toggleStatuss;
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.getclientlist.some(function (options) {
                return options.selected;
            });


        };

        $scope.emptogchkbx = function () {
            $scope.employeeck = $scope.getclientlist.every(function (types) {
                return types.ismmclT_Id;
            });
        };


        //////end////

    }
})();
