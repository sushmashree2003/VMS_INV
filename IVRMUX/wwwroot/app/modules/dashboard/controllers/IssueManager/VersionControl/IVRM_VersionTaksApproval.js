(function () {
    'use strict';
    angular
        .module('app')
        .controller('IVRM_VersionTaksApprovalController', IVRM_VersionTransactionController)

    IVRM_VersionTransactionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce', 'Excel', '$timeout']
    function IVRM_VersionTransactionController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce, Excel, $timeout) {

        //-------------Onload---Master Hosting
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.itemsPerPage1 = 10;
        $scope.showversionlist = false;
        $scope.all = true;
        $scope.typedisable = false;
        $scope.loaddata = function () {


            $scope.tasklength;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("IVRM_VersionControlProcess/VTgetdetails", pageid).then(function (promise) {
                $scope.clientHosting = promise.clientHosting;
                $scope.gettaskforapproval = [];

            });
        };

        //-------------END ON LOAD---Master Hosting


        //--get version details
        $scope.getversiondetails = function () {
            $scope.showversionlist = true;
            $scope.updatedversion = "";
            $scope.previousversion = "";
            $scope.IVRMVT_DeployementDate = "";
            $scope.previousdeploydate = "";
            var items = {
                "IVRMCH_ID": $scope.ivrmcH_Id.ivrmcH_Id,
                "flag": "existingversion"
            };
            apiService.create("IVRM_VersionControlProcess/getversiondetails", items).then(function (response) {
                if (response.lastdeployeddate != null && response.lastdeployedversion != null) {

                    $scope.previousdeploydate = response.lastdeployeddate
                    $scope.previousversion = response.lastdeployedversion
                    $scope.IVRMVT_DeployementDate = $scope.previousdeploydate;
                    $scope.noversions = false;
                    $scope.typedisable = false;
                }
                else {
                    swal("No previous Deployments Found For This Hosting", "Kindly Select The Full Deployment","info");
                    $scope.IVRMVRT_DeployType = "F";
                    $scope.typedisable = true;
                    $scope.getnextversion();
                    $scope.noversions = true;

                }
            });
        }

        $scope.getnextversion = function () {
            var items = {
                "IVRMCH_ID": $scope.ivrmcH_Id.ivrmcH_Id,
                "IVRMVRT_DeployType": $scope.IVRMVRT_DeployType
            };
            apiService.create("IVRM_VersionControlProcess/getversiondetails", items).then(function (response) {

                if (response.updatedversion != null && response.updatedversion != null) {
                    $scope.updatedversion = response.updatedversion
                }
                else {
                    swal("No previous Deployments Found For This Hosting", "Kindly Select The Full Deployment", "info");
                    $scope.typedisable = true;
                }


            });
        }
        //--end get version details

        //------------To Load Task List Based on hosting
        $scope.tasklist = function (ivrmcH_Id) {


            if ($scope.myForm.$valid) {
                $scope.gettaskforapproval = [];
                var data = {
                    "IVRMCH_ID": ivrmcH_Id.ivrmcH_Id,
                   // "IVRMVT_DeployementDate": $scope.IVRMVT_DeployementDate,
                    "IVRMVT_DeployementDate": new Date($scope.IVRMVT_DeployementDate).toDateString()
                };
                apiService.create("IVRM_VersionControlProcess/Loadthetasklist", data).then(function (promise) {

                    if (promise.gettaskforapproval.length > 0 && promise.gettaskforapproval.length != null) {
                        $scope.gettaskforapproval = promise.gettaskforapproval;
                        $scope.tasklength = $scope.gettaskforapproval.length;


                        angular.forEach($scope.gettaskforapproval, function (dd) {
                            if (dd.ISMTCR_BugOREnhancementFlg === "T" || dd.ISMTCR_BugOREnhancementFlg === "B" || dd.ISMTCR_BugOREnhancementFlg === "O") {
                               // dd.taskcheck = 1;
                                dd.taskcheck = 0;
                                dd.IVRMVRT_AddedFeature = 0;
                              //  dd.css = "disable"
                            }
                            else {
                                dd.IVRMVRT_AddedFeature = 0;
                                dd.taskcheck = 0;
                             //   dd.css = "enable"
                            }


                        });
                    }
                    else {
                        swal("No Task List Found", "", "warning");
                    }

                });
            }
            else {                $scope.submitted = true;            }
            $scope.interacted = function (field) {
                return $scope.submitted;
            };
        }
        //---------Approve For Deployment
        $scope.approve = function () {
            $scope.temtasklist = [];
            angular.forEach($scope.gettaskforapproval, function (atl) {
                if (atl.taskcheck) {
                    $scope.temtasklist.push(atl);
                }
            })
            swal({
                title: "Do You Want To Approve The Tasks",
                text: "Total Task :" + $scope.temtasklist.length + " ",
                type: "info",
                showCancelButton: true,
                confirmButtonColor: "#3cb371",
                confirmButtonText: "Yes, Approve!",
                cancelButtonColor: "#DD6B55",
                cancelButtonText: "Cancel",
                closeOnConfirm: true,
                closeOnCancel: true
            },
                function (isConfirm) {
                    if (isConfirm) {

                        var items = {
                            "temtasklist": $scope.temtasklist,
                            "IVRMVT_DeployementDate": $scope.IVRMVT_DeployementDate,
                            "IVRMCH_ID": $scope.ivrmcH_Id.ivrmcH_Id,
                            "IVRMVRT_DeployType": $scope.IVRMVRT_DeployType
                        };
                        apiService.create("IVRM_VersionControlProcess/savetasklist", items).then(function (response) {
                            if (response.returnval == "Add") {
                                swal("Tasks Approved Successfully For The Deployment", "Total Tasks : " + $scope.temtasklist.length, "success");
                                $state.reload();
                            }
                            else {
                                swal("Failed To Save Task List", "", "warning");
                                $state.reload();
                            }
                        });
                    }

                });

        }
        //---------END Approve For Deployment

        //-------------Select All in grid
        $scope.selectall = function (taskcheck) {
            angular.forEach($scope.gettaskforapproval, function (tl) {
                //if (tl.ISMTCR_BugOREnhancementFlg === "E") {
                //    tl.taskcheck = taskcheck;
                //}
                tl.taskcheck = taskcheck;
            })
        }
        //-------------END Select All in grid
        $scope.cancel = function () {
            $state.reload();
        };
    }
})();
