(function () {
    'use strict';
    angular.module('app').controller('VMS_Vehicle_KeyController', VMS_Vehicle_KeyController)
    VMS_Vehicle_KeyController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function VMS_Vehicle_KeyController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };


        $scope.submitted = false;

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }
        var configsettingsnew = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        $scope.itemsPerPage = paginationformasters;

        $scope.currentPage = 1;
        //-----------Tax Grid Select All
        $scope.ismeridian = true;
        $scope.hstep = 1;
        $scope.mstep = 1;
       
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian; 
        };
        $scope.VMSVKEYS_RecivedDate = new Date();
        $scope.vehicalListTemp = [];
        $scope.vehReciveFlag = false;
        $scope.VMSVKEYSVTWO_KeyReceivedHandedOverFlg = "Received";
        $scope.loaddata = function () {
          
            var pageid = 2;
            apiService.getURI("RTO_Master_Fine/loaddata", pageid).then(function (promise) {
                $scope.vehicalArray = promise.vehicalArray;
                $scope.emplist = promise.emplist;
                $scope.vehicalListTemp = promise.vehicalList;
                $scope.vehicalList = [];
                //  $scope.vehicalList = promise.vehicalList;
                if ($scope.vehicalListTemp != null && $scope.vehicalListTemp.length > 0) {
                    for (var t = 0; t < $scope.vehicalListTemp.length; t++) {
                        $scope.vehicalList.push({
                            VMSVKEYS_RecivedTime: moment($scope.vehicalListTemp[t].VMSVKEYS_RecivedTime, 'HH:mm a').format(),                                                                                                                             
                            VMSVKEYS_Id: $scope.vehicalListTemp[t].VMSVKEYS_Id,
                            VMSVKEYS_KeyReceivedHandedOverFlg: $scope.vehicalListTemp[t].VMSVKEYS_KeyReceivedHandedOverFlg,
                            VMSVKEYS_RecivedDate: new Date($scope.vehicalListTemp[t].VMSVKEYS_RecivedDate),                            
                            HRME_Id: $scope.vehicalListTemp[t].HRME_Id,
                            VMSVKEYS_OdometerReading: $scope.vehicalListTemp[t].VMSVKEYS_OdometerReading,
                            TRMV_VehicleName: $scope.vehicalListTemp[t].TRMV_VehicleName,
                            VMSVKEYS_ActiveFlag: $scope.vehicalListTemp[t].VMSVKEYS_ActiveFlag,
                            TRMV_VehicleNo: $scope.vehicalListTemp[t].TRMV_VehicleNo,
                            EMPNAME: $scope.vehicalListTemp[t].EMPNAME,
                            Applicationuser: $scope.vehicalListTemp[t].Applicationuser,
                            TRMV_Id: $scope.vehicalListTemp[t].TRMV_Id,
                            VMSVKEYS_Remarks: $scope.vehicalListTemp[t].VMSVKEYS_Remarks,
                            VMSVKEYS_KeyHandedOverFlg: $scope.vehicalListTemp[t].VMSVKEYS_KeyHandedOverFlg,
                            VMSVKEYS_Time: moment($scope.vehicalListTemp[t].VMSVKEYS_Time, 'HH:mm a').format(),  
                            VMSVKEYS_DateTime: new Date($scope.vehicalListTemp[t].VMSVKEYS_DateTime),
                        });
                    }

                }
                
            });
        };
        //Onreport
        $scope.Onreport = function () {
            $scope.VMSVKEYS_KeyReceivedHandedOverFlg = "";
            var trmV_VehicleName = "";
            if ($scope.vehicalArray != null && $scope.vehicalArray.length > 0) {
                for (var i = 0; i < $scope.vehicalArray.length; i++) {
                    if ($scope.vehicalArray[i].trmV_Id == $scope.TRMV_Id) {
                        trmV_VehicleName = $scope.vehicalArray[i].trmV_VehicleName + '/' + $scope.vehicalArray[i].trmV_VehicleNo;
                    }
                }
            }
            
            var data = {             
                "TRMV_Id": $scope.TRMV_Id,
               
            };
            apiService.create("RTO_Master_Fine/RecieptReport", data).then(function (promise) {
                if (promise.vehicales != null && promise.vehicales.length > 0) {
                    if (promise.vehicales[0].vmsvkeyS_KeyHandedOverFlg != null && promise.vehicales[0].vmsvkeyS_KeyHandedOverFlg != "") {
                       
                    }
                    else {
                        swal("Need To Receive " + trmV_VehicleName + " !");
                        $scope.VMSVKEYS_KeyReceivedHandedOverFlg = "";
                        $scope.TRMV_Id = "";
                    }
               
                    
                    
                }
               
            });
        }
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var VMSVKEYS_Id = 0;
                if ($scope.VMSVKEYS_Id > 0) {
                    VMSVKEYS_Id = $scope.VMSVKEYS_Id;
                }
                var VMSVKEYS_RecivedTime = $scope.VMSVKEYS_RecivedTime;
                
                $scope.sresult = $filter('date')(VMSVKEYS_RecivedTime, 'HH:mm');
                var data = {
                    "HRME_Id": $scope.HRME_Id,
                    "TRMV_Id": $scope.TRMV_Id,
                    "VMSVKEYS_KeyReceivedHandedOverFlg": $scope.VMSVKEYS_KeyReceivedHandedOverFlg,
                    "VMSVKEYS_RecivedTime": $scope.sresult,
                    "VMSVKEYS_Id": VMSVKEYS_Id,
                    "VMSVKEYS_RecivedDate": new Date($scope.VMSVKEYS_RecivedDate).toDateString(),
                    "VMSVKEYS_Remarks": $scope.VMSVKEYS_Remarks
                };
                apiService.create("RTO_Master_Fine/savedata", data).then(function (promise) {
                    if (promise.returnval == "save") {
                        swal("Record Saved Sucessfully !");
                    }
                    else if (promise.returnval == "Notsave") {
                        swal("Record Not Saved !");
                    }
                    else if (promise.returnval == "duplicate") {
                        swal("Record duplicate !");
                    }
                    if (promise.returnval == "update") {
                        swal("Record Update Sucessfully !");
                    }
                    else if (promise.returnval == "Notupdate") {
                        swal("Record Not Update !");
                    }
                    else if (promise.returnval == "admin") {
                        swal("Please Contact Administrator !");
                    }
                    $state.reload();
                });
            }

        };
        $scope.UPDATE = function () {
            if ($scope.VMSVKEYS_DateTime != null && $scope.VMSVKEYS_Time != null && $scope.VMSVKEYS_Time != "" && $scope.VMSVKEYS_OdometerReading != null && $scope.VMSVKEYS_OdometerReading!="") {
                var VMSVKEYS_Time = $scope.VMSVKEYS_Time;

                $scope.sresults = $filter('date')(VMSVKEYS_Time, 'HH:mm');
                var data = {
                    "VMSVKEYS_DateTime": new Date($scope.VMSVKEYS_DateTime).toDateString(),
                    "VMSVKEYS_Time": $scope.sresults,
                    "VMSVKEYS_Id": $scope.VMSVKEYS_Idtwo,
                    "VMSVKEYS_KeyHandedOverFlg": $scope.VMSVKEYSVTWO_KeyReceivedHandedOverFlg,
                    "TRMV_Id": $scope.TRMVV_Id,
                    "VMSVKEYS_Remarks": $scope.VMSVKEYS_RemarksTwo,
                    "VMSVKEYS_OdometerReading": $scope.VMSVKEYS_OdometerReading,

                }
                apiService.create("RTO_Master_Fine/VehicalUpdate", data).
                    then(function (promise) {
                        if (promise.returnval == "update") {
                            swal("Record Update SucessFully !")
                        }
                        else if (promise.returnval == "Notupdate") {
                            swal("Record Not Update  !")
                        }
                        else if (promise.returnval == "admin") {
                            swal("Please Contact Administator  !")
                        }
                        $('#popup123').modal('hide');
                        $scope.VMSVKEYS_DateTime = null;
                        $scope.sresults = "";
                        $scope.VMSVKEYS_Idtwo = "";
                      
                        $scope.TRMVV_Id = "";
                        $scope.VMSVKEYS_Id = "";
                        $scope.VMSVKEYS_RemarksTwo = "";
                     $scope.loaddata();
                     //   $state.reload();
                    })
            }
            else {
                swal("Please Select Hand Over DATE / TIME /KM")
            }
        }
        $scope.close = function () {
            $scope.VMSVKEYS_DateTime = null;
            $scope.sresults = "";
            $scope.VMSVKEYS_Idtwo = "";
     
            $scope.TRMVV_Id = "";
            $scope.VMSVKEYS_RemarksTwo = "";
            
        }
        $scope.View = function (user) {
            $scope.VMSVKEYSV_RecivedTime = user.VMSVKEYS_RecivedTime;
           // $scope.VMSVKEYSV_RecivedTime = moment(user.VMSVKEYS_RecivedTime, 'HH:mm a').format();
            $scope.VMSVKEYS_Id = user.VMSVKEYS_Id;
            $scope.HRMEV_Id = user.HRME_Id;
            $scope.TRMVV_Id = user.TRMV_Id;
            $scope.VMSVKEYSV_RecivedDate = new Date(user.VMSVKEYS_RecivedDate);
            $scope.VMSVKEYSV_KeyReceivedHandedOverFlg = user.VMSVKEYS_KeyReceivedHandedOverFlg; 
            $scope.VMSVKEYS_RemarksTwo = user.VMSVKEYS_Remarks; 
            $scope.VMSVKEYS_Idtwo = user.VMSVKEYS_Id; 
            $('#popup123').modal('show');
        }

        $scope.edit = function (user) {
            $scope.VMSVKEYS_RecivedTime = user.VMSVKEYS_RecivedTime;
         //   $scope.VMSVKEYS_RecivedTime = moment(user.VMSVKEYS_RecivedTime, 'HH:mm a').format();
            $scope.VMSVKEYS_Id = user.VMSVKEYS_Id;
            $scope.HRME_Id = user.HRME_Id;
            $scope.TRMV_Id = user.TRMV_Id;
            $scope.VMSVKEYS_RecivedDate = new Date(user.VMSVKEYS_RecivedDate);
            $scope.VMSVKEYS_KeyReceivedHandedOverFlg = user.VMSVKEYS_KeyReceivedHandedOverFlg;  
            $scope.VMSVKEYS_Remarks = user.VMSVKEYS_Remarks; 
        }
        $scope.Deletedata = function (item, SweetAlert) {
            var data = {
                "VMSVKEYS_Id": item.VMSVKEYS_Id
            }
            var dystring = "";
            if (item.VMSVKEYS_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.VMSVKEYS_ActiveFlag == false) {
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
                        apiService.create("RTO_Master_Fine/onchange", data).
                            then(function (promise) {
                                if (promise.returnval == "active") {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else if (promise.returnval == "notactive") {
                                    swal("Record Not Deactivate !");
                                }
                                else if (promise.returnval == "admin") {
                                    swal("Please Contact Administrator !");
                                }
                                $state.reload();

                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();
