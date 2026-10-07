(function () {
    'use strict';
    angular.module('app').controller('ISM_PrinterStatisticsController', ISM_PrinterStatisticsController);
    ISM_PrinterStatisticsController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$window', 'intracontroller'];
    function ISM_PrinterStatisticsController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter, $window, intracontroller) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag === "ISMTASK") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }


      
        $scope.VMSPOD_Type = 'Color';
        $scope.submitted = false;
        $scope.VMSPOD_ToDate = new Date();
        $scope.VMSPOD_FromDate = new Date();

        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("ISM_PrinterStatistics/getdetails", pageid).then(function (promise) {

                $scope.get_department = promise.get_department;
               
                $scope.get_printdetails = promise.get_printdetails;
                $scope.presentCountgrid = $scope.get_printdetails.length;
             
                $scope.get_printers = promise.get_printers;
                //$scope.hrmD_Id = "4";

            })
            
        };


        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };



        $scope.obj = {};
        $scope.savedata = function (obj) {
            $scope.submitted = true;
            var data = {};

            if ($scope.MyForm.$valid) {
            if ($scope.VMSPOD_Type == 'Color') {
                $scope.VMSPOD_Type = 'Color';
            }
            else {
                $scope.VMSPOD_Type = 'BlackWhite';
            }

            data = {
                "HRMD_Id": $scope.hrmD_Id,


                "VMSPOD_Id": $scope.VMSPOD_Id,
                "VMSPOD_ActiveFlag": 1,

                //"VMSPOD_PODate": $scope.VMSPOD_PODate,
                "VMSPOD_NoOfPages": $scope.VMSPOD_NoOfPages,
                "VMSPOD_Remarks": $scope.VMSPOD_Remarks,
                "VMSPOD_Type": $scope.VMSPOD_Type,
                "VMSPOD_ToDate": new Date($scope.VMSPOD_ToDate).toDateString(),
                "VMSPOD_FromDate": new Date($scope.VMSPOD_FromDate).toDateString(),
                "VMSMPR_Id": $scope.vmsmpR_Id,

            };

            apiService.create("ISM_PrinterStatistics/savedata", data).then(function (promise) {
                if (promise.returnval === "true") {
                    if (promise.vmspoD_Id === 0 || promise.vmspoD_Id < 0) {
                        swal('Record saved successfully');
                        $scope.get_printdetails = promise.get_printdetails;
                        $scope.presentCountgrid = $scope.get_printdetails.length;
                    }
                    else if (promise.vmspoD_Id > 0) {
                        swal('Record updated successfully');
                        $scope.get_printdetails = promise.get_printdetails;
                        $scope.presentCountgrid = $scope.get_printdetails.length;
                    }
                    
                    $state.reload();
                }
               
                else if (promise.returnval === 'duplicate') {
                    swal('Record already exist with from date and To Date kindly edit the same record ');
                }
                else {
                    if (promise.vmspoD_Id === 0 || promise.vmspoD_Id < 0) {
                        swal('Failed to save, please contact administrator');
                    }
                    else if (promise.vmspoD_Id > 0) {
                        swal('Failed to update, please contact administrator');
                    }
                }

            });
            }
        }

        $scope.datachange = function (value) {

        
            $scope.VMSPOD_Type = value;
         
        };

        //=====================Editrecord....
        $scope.editdetails = function (task) {
          

            var data = {
                "VMSPOD_Id": task.vmspoD_Id
            };
            apiService.create("ISM_PrinterStatistics/gettaskAttachment", data).then(function (promise) {
                if (promise.get_printdetails.length > 0) {
                   // $scope.get_printdetails = promise.get_printdetails;
                    $scope.VMSPOD_Id = promise.get_printdetails[0].vmspoD_Id;
                    $scope.hrmD_Id = promise.get_printdetails[0].hrmD_Id;
                    $scope.VMSPOD_ActiveFlag = promise.get_printdetails[0].vmspoD_ActiveFlag;
                    $scope.VMSPOD_Type = promise.get_printdetails[0].vmspoD_Type;
                   // $scope.VMSPOD_PODate = new Date(promise.get_printdetails[0].vmspoD_PODate);
                    $scope.VMSPOD_Remarks = promise.get_printdetails[0].vmspoD_Remarks;
                    $scope.VMSPOD_NoOfPages = promise.get_printdetails[0].vmspoD_NoOfPages;
                    $scope.vmsmpR_Id = promise.get_printdetails[0].vmsmpR_Id;
                    $scope.VMSPOD_FromDate = new Date(promise.get_printdetails[0].vmspoD_FromDate);
                    $scope.VMSPOD_ToDate = new Date(promise.get_printdetails[0].vmspoD_ToDate);
                }
                //$scope.get_printdetails = promise.get_printdetails;
                //$scope.presentCountgrid = $scope.get_printdetails.length;
               
                //$scope.get_Attdetails = promise.get_Attdetails;
            });
        };

        //=================Active/Deactivate
        $scope.deleterecord = function (task) {

            $scope.VMSPOD_Id = task.vmspoD_Id;
            var dystring = "";
            
            if (task.vmspoD_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (task.vmspoD_ActiveFlag === false) {
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
                        apiService.create("ISM_PrinterStatistics/deactive", task).
                            then(function (promise) {
                                if (promise.returnval == "true") {
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

        $scope.interacted = function (obj) {
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.cancel = function () {
            $state.reload();
        };


        $scope.deptchange = function (id) {
            $scope.submitted = true;
            $scope.get_printers = [];
            var data = {};

      
                data = {
                    "HRMD_Id": id,


                 

                };

            apiService.create("ISM_PrinterStatistics/deptchange", data).then(function (promise) {
              

                $scope.get_printers = promise.get_printers;

                });
          
        }


    };
})();

