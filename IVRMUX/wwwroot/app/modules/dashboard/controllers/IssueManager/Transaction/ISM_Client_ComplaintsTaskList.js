(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_Client_ComplaintsTaskListController', ISM_Client_ComplaintsTaskListController);
    ISM_Client_ComplaintsTaskListController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function ISM_Client_ComplaintsTaskListController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.obj = {};
        $scope.typeFlg = "Default";
        $scope.currentDate = new Date();
        $scope.submitted = false;
        $scope.totalopenflg = false;
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.get_clientcomptasklist = [];
            $scope.get_clientcomptasklistpending = [];
            $scope.totaltask = 0;
            $scope.pendingcount = 0;
            $scope.totalopenflg = true;
            $scope.search = "";
            var data = {
                "typeFlg": $scope.typeFlg
            };
            apiService.create("ISM_Client_ComplaintsTaskList/getdetails", data).then(function (promise) {
                $scope.roletype = promise.roletype;
                $scope.get_Status = [{ id: 'Open', value: 'Open' }, { id: 'Inprogress', value: 'Inprogress' }, { id: 'Completed', value: 'Completed' }, { id: 'Development Completed', value: 'Development Completed' }, { id: 'Deployement Completed in test link', value: 'Deployement Completed in test link' }, { id: 'Deployement Completed in Live link', value: 'Deployement Completed in Live link' }, { id: 'Close', value: 'Close' }];

                if ($scope.roletype === "ClientUser") {
                    $scope.roleflg = true;
                }
                else {
                    $scope.roleflg = false;
                }
                $scope.get_client = promise.get_client;
                $scope.get_clientcomptasklist = promise.get_clientcomptasklist;
                $scope.totaltask = promise.get_clientcomptasklist.length;
                $scope.get_IEuserlist = promise.get_IEuserlist;
                if ($scope.get_clientcomptasklist.length > 0) {
                    var pendingtaskcount = 0;
                    angular.forEach($scope.get_clientcomptasklist, function (clt) {
                        if (clt.ISMTCR_Status === 'Open' || clt.ISMTCR_Status === 'Inprogress' || clt.ISMTCR_Status === 'Reopen') {
                            pendingtaskcount += 1;
                            $scope.get_clientcomptasklistpending.push(clt);
                        }
                    });
                    $scope.pendingcount = pendingtaskcount;
                    if ($scope.get_clientcomptasklistpending.length == 0) {
                        swal("No Open Tasks Found ....!!");
                    }
                }
                else {
                    $scope.pendingcount = 0;
                    swal("No Record Found ....!!");
                    $scope.get_clientcomptasklist = [];
                    $scope.get_clientcomptasklistpending = [];
                }
            });
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;
                $scope.reverse = !$scope.reverse;
            };
        };
        //====================================== Check Box Selection
        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.get_client, function (uem) {
                uem.cltck = toggleStatus;
            });
        };
        $scope.togchkbx = function () {
            $scope.checkall = $scope.get_client.every(function (options) {
                return options.cltck;
            });
        };
        $scope.isOptionsRequired = function () {
            return !$scope.get_client.some(function (options) {
                return options.cltck;
            });
        };
        $scope.all_checkS = function (ack) {
            $scope.checkallS = ack;
            var toggleStatus = $scope.checkallS;
            angular.forEach($scope.get_Status, function (sts) {
                sts.stck = toggleStatus;
            });
        };
        $scope.togchkbxS = function () {
            $scope.checkallS = $scope.get_Status.every(function (options) {
                return options.stck;
            });
        };
        //======================================== Search List
        $scope.searchtasks = function () {
            if ($scope.myForm.$valid) {
                $scope.typeFlg = "Search";
                $scope.arrayClient = [];
                $scope.arrayStatus = [];
                $scope.totalopenflg = false;
                $scope.get_clientcomptasklist = [];
                $scope.get_clientcomptasklistpending = [];
                angular.forEach($scope.get_client, function (cl) {
                    if (cl.cltck === true) {
                        $scope.arrayClient.push(cl);
                    }
                });
                angular.forEach($scope.get_Status, function (sts) {
                    if (sts.stck === true) {
                        $scope.arrayStatus.push({ ISMMISTS_StatusName: sts.id });
                    }
                });
                var data = {
                    "typeFlg": $scope.typeFlg,
                    "arrayClient": $scope.arrayClient,
                    "arrayStatus": $scope.arrayStatus
                };
                apiService.create("ISM_Client_ComplaintsTaskList/getsearchtask", data).
                    then(function (promise) {
                        $scope.get_clientcomptasklist = promise.get_clientcomptasklist;
                        if ($scope.get_clientcomptasklist.length > 0) { 
                            $scope.get_clientcomptasklistpending = $scope.get_clientcomptasklist;            
                            if ($scope.get_clientcomptasklistpending.length == 0) {
                                swal("No Record Found ....!!");
                            }
                        }
                        else {
                            $scope.pendingcount = 0;
                            swal("No Record Found ....!!");
                            $scope.get_clientcomptasklist = [];
                            $scope.get_clientcomptasklistpending = [];
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.searchchkbx = "";

        $scope.viewresponse = function (taskobj) {
            //$('#myModalResponse').modal('show');            
            $scope.ismtcrreS_Response = "";
            $scope.files_paths = "";
            $scope.ismmistS_StatusName = "";
            $scope.taskAttatchment = [];
            $scope.taskid = taskobj.ISMTCR_Id;
            $scope.created_by = taskobj.createdby;
            var data = {
                "ISMTCR_Id": $scope.taskid
            };
            apiService.create("ISM_Client_ComplaintsTaskList/gettaskdetails", data).
                then(function (promise) {
                    $scope.get_taskdetails = promise.get_taskdetails;
                    if ($scope.get_taskdetails !== null && $scope.get_taskdetails.length > 0) {
                        $scope.tasktitle = $scope.get_taskdetails[0].ISMTCR_Title;
                        $scope.taskno = $scope.get_taskdetails[0].ISMTCR_TaskNo;
                        $scope.createdby = $scope.get_taskdetails[0].createdby;
                        $scope.taskclient = $scope.get_taskdetails[0].ISMMCLT_ClientName;
                        $scope.taskdate = $scope.get_taskdetails[0].ISMTCR_CreationDate;
                        $scope.tasktype = $scope.get_taskdetails[0].tasktype;
                        $scope.tsakpriority = $scope.get_taskdetails[0].HRMP_Name;
                        $scope.taskdescription = $scope.get_taskdetails[0].ISMTCR_Desc;
                        $scope.taskstatus = $scope.get_taskdetails[0].ISMTCR_Status;
                    }
                });
        };
        //===========Clear Field
        $scope.refresh = function () {
            $state.reload();
        };
        $scope.interacted = function (field) {
            return $scope.submitted ;
        };

    }
})();

