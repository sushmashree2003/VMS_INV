(function () {
    'use strict';
    angular
        .module('app')
        .controller('Master_Driver_ShiftsController', Master_Driver_ShiftsController)

    Master_Driver_ShiftsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$filter']
    function Master_Driver_ShiftsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $filter) {

        //  $scope.group_check = '0';

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian; todate
        };

        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.loaddata = function () {
            $scope.all_check();
            $scope.page1 = "page1";
            $scope.page2 = "page2";
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            var pageid = 1;
            $scope.printstudents = [];
            apiService.getURI("Master_Driver_Shifts/getdetails", pageid).
                then(function (promise) {

                    $scope.drivers_array = promise.drivers_array;
                   

                });
            function ShowHide() {
                apiService.getURI("MasterHoliday/", pageid).
                    then(function (promise) {
                        lblShowHide.style.visibility = 'visible';
                    });

            }

        };

    


        $scope.search = '';
        //$scope.filterValue = function (obj) {

        //    return angular.lowercase(obj.vmsmdsH_ShiftName).indexOf(angular.lowercase($scope.search)) >= 0;
        //        //||
        //        //($filter('date')(obj.FOMHWDD_FromDate, 'dd/MM/yyyy').indexOf($scope.search) >= 0) ||
        //        //($filter('date')(obj.FOMHWDD_ToDate, 'dd/MM/yyyy').indexOf($scope.search) >= 0);
        //};



        //$scope.Deletedata = function (del_id) {
        //    swal({
        //        title: "Are you sure?",
        //        text: "Do you want to Delete This Record?",
        //        type: "warning",
        //        showCancelButton: true,
        //        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it!",
        //        cancelButtonText: "Cancel",
        //        closeOnConfirm: true,
        //        closeOnCancel: false
        //    },
        //        function (isConfirm) {
        //            if (isConfirm) {


        //                apiService.getURI("MasterHoliday/delete/", del_id.FOMHWDD_Id).
        //                    then(function (promise) {
        //                        if (promise.returnval == true) {
        //                            swal("Record Deleted Successfully");
        //                            $state.reload();
        //                        }
        //                        else {
        //                            swal("Failed to Delete, please contact administrator.");
        //                        }

        //                    });
        //            }
        //            else {
        //                swal("Cancelled");
        //            }
        //        });
        //};




        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        $scope.sort1 = function (keyname) {
            $scope.sortKey1 = keyname;   //set the sortKey to the param passed
            $scope.reverse1 = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.Refrencecheckboxchcked = [];
        $scope.day_datecheckboxchcked = [];
        $scope.submitted = false;
        angular.forEach($scope.days_list, function (itm) {
            if (itm.dayy)
                $scope.Refrencecheckboxchcked.push(itm);
        });


        angular.forEach($scope.datesanddayList, function (itm) {
            if (itm.all)
                $scope.day_datecheckboxchcked.push(itm);
        });


        $scope.selectedDaysDate = [];
        $scope.savemasterDriversdata = function () {

            if ($scope.myForm.$valid) {

                var vmsmdsH_Id = 0;
                if ($scope.VMSMDSH_Id > 0) {
                    vmsmdsH_Id = $scope.VMSMDSH_Id;
                }

                var vmsmdsH_ShiftStartTime = $scope.VMSMDSH_ShiftStartTime;
                var vmsmdsH_ShiftEndTime = $scope.VMSMDSH_ShiftEndTime;
                $scope.sresult = $filter('date')(vmsmdsH_ShiftStartTime, 'HH:mm');
                $scope.sresultend_time = $filter('date')(vmsmdsH_ShiftEndTime, 'HH:mm');
                var data = {
                    "VMSMDSH_Id": vmsmdsH_Id,
                    "VMSMDSH_ShiftStartTime": $scope.sresult,
                    "VMSMDSH_ShiftEndTime": $scope.sresultend_time,
                    "VMSMDSH_ShiftName": $scope.VMSMDSH_ShiftName,
                    "VMSMDSH_Remarks": $scope.VMSMDSH_Remarks,
                    "VMSMDSH_OnlyOnSpecificDayFlg": $scope.VMSMDSH_OnlyOnSpecificDayFlg,
                };

                apiService.create("Master_Driver_Shifts/savedata", data).
                    then(function (promise) {

                        if (promise.message != "" && promise.message != null && promise.returnval === true) {
                            swal('Record saved successfully ' + ' ' + promise.message);
                            // $state.reload();
                        }
                        else if (promise.message != "" && promise.message != null) {
                            swal(promise.message);
                            // $state.reload();
                        }
                        else if (promise.returnval === true) {
                            swal('Record saved successfully.');
                            $state.reload();
                        }

                        else if (promise.returnval === false) {
                            swal('Failed to save, please contact administrator.');
                            $state.reload();
                        }
                        else if (promise.duplicate == true) {
                            swal('Already Record Exist!.');
                            $state.reload();
                        }
                        //$scope.loadData();
                        $scope.gridOptions1.data = promise.master_eventlist;
                    })

                //$state.reload();
                //$scope.loaddata();
                $scope.clear1();
            }
            else {
                $scope.submitted = true;

            }


        };

        $scope.clear1 = function () {
            $state.reload();
            $scope.loaddata();
        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.group_check = "0";

        $scope.showgrid = false;

        $scope.isOptionsRequired = function () {
            if ($scope.group_check == "1") {
                return !$scope.days_list.some(function (options) {
                    return options.dayy;
                });
            }
            else if ($scope.group_check == "0") {
                return false;
            }

        };

        //start time and end time validation
        $scope.validateTomintime = function (timedata) {

            //$scope.timedis1 = false;
            //$scope.timedis2 = true;
            $scope.FOMST_IIHalfLogoutTime = "";
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;

            $scope.min.setHours(hh);
            $scope.min.setMinutes(mm);
            $scope.minlnc = timedata;

            $scope.minlnc.setHours(hh);
            $scope.minlnc.setMinutes(mm);
            $scope.FOMST_IHalfLogoutTime = "";
        }
        $scope.validatemax = function (maxdata) {

            // $scope.FOMST_IHalfLoginTime = maxdata;
            //$scope.FOMST_IIHalfLogoutTime = "";
            var dsttimee = $scope.FOMST_IHalfLoginTime;
            $scope.sresult = $filter('date')(dsttimee, 'HH:mm:ss a');
            $scope.eresult = $filter('date')(maxdata, 'HH:mm:ss a');
            //var startTime = moment(dsttimee, "HH:mm:ss a");
            //  var endTime = moment(maxdata, "HH:mm:ss a");
            var startTime = moment($scope.sresult, "HH:mm:ss a");
            var endTime = moment($scope.eresult, "HH:mm:ss a");
            var duration = moment.duration(endTime.diff(startTime));
            var hours = parseInt(duration.asHours());
            var minutes = parseInt(duration.asMinutes()) - hours * 60;
            var finlrst = hours + ":" + minutes;

            $scope.tmin = new Date();
            $scope.tmin.setHours(hours);
            $scope.tmin.setMinutes(minutes);
            $scope.tmax = new Date();
            $scope.tmax.setHours(hours);
            $scope.tmax.setMinutes(minutes);

            $scope.ttst = new Date();
            $scope.ttst.setHours(hours);
            $scope.ttst.setMinutes(minutes);

            $scope.FOMST_FDWHrMin = $scope.ttst;
            //   $scope.FOMST_HDWHrMin = $scope.ttst;


            $scope.htmax = new Date();
            $scope.htmax.setHours(hours);
            $scope.htmax.setMinutes(minutes);




            if (maxdata >= new Date($scope.FOMST_IHalfLoginTime)) {
                $scope.totimemax = maxdata;
                var hh = $scope.totimemax.getHours();
                var mm = $scope.totimemax.getMinutes();
                $scope.max = maxdata;

                $scope.max.setMinutes(hh);
                $scope.max.setMinutes(mm);
            }
            else {
                $scope.FOMST_IIHalfLogoutTime = "";
            }

            // $scope.FOMST_IHalfLogoutTime = "";
        }


        $scope.Editdata = function (employee) {
            debugger;
            $scope.editEmployee = employee.vmsmdsH_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("Master_Driver_Shifts/EditData", pageid).
                then(function (promise) {
                    //if (promise.edit_array != null && edit_array.length > 0) {
                        $scope.VMSMDSH_Id = promise.edit_array[0].vmsmdsH_Id;
                        $scope.VMSMDSH_ShiftName = promise.edit_array[0].vmsmdsH_ShiftName;
                        $scope.VMSMDSH_ShiftStartTime = moment(promise.edit_array[0].vmsmdsH_ShiftStartTime, 'HH:mm').format();
                        $scope.VMSMDSH_ShiftEndTime = moment(promise.edit_array[0].vmsmdsH_ShiftEndTime, 'HH:mm').format();
                    $scope.VMSMDSH_OnlyOnSpecificDayFlg = promise.edit_array[0].vmsmdsH_OnlyOnSpecificDayFlg;
                    $scope.VMSMDSH_Remarks = promise.edit_array[0].vmsmdsH_Remarks;
                    

                    //}
                 
                })
        }


    }
})();


















