(function () {
    'use strict';
    angular.module('app').controller('FMSApprovalRequestPageController', FMSApprovalRequestPageController)

    FMSApprovalRequestPageController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$timeout', 'Excel']
    function FMSApprovalRequestPageController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $timeout, Excel) {



        $scope.submitted = false;
        $scope.indentapproveddetais = [];
        $scope.maxdate = new Date();

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        var temp = [];
        var year = "";
        $scope.yearfromdate = "";
        $scope.monthlist_temp = [];
        $scope.consolidata_id = [];
        $scope.get_deviationreport = [];
        $scope.imgname = logopath;

        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            $scope.fromdate = new Date();
            $scope.todate = new Date();


            var pageid = 2;
            apiService.getURI("FMSApprovalRequestPage/loaddata", pageid).then(function (promise) {

                $scope.filename = promise.filename;

                if (promise.get_tax != null && promise.get_tax.length > 0) {
                    $scope.get_tax = promise.get_tax;
                    
                    $scope.registration = $scope.get_tax.length;
                }


            });
        };

        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.from_date = new Date($scope.fromdate).toDateString();
                $scope.to_date = new Date($scope.todate).toDateString();
                var data = {
                    "FMSMPFN_Id": $scope.Filename,
                    "FMSFLREQ_RequiredDays": $scope.request,
                    "FMSFLREQ_Id": $scope.fmsflreQ_Id,
                    "FMSMPFN_PhysicalFileName": $scope.fmsmpfN_PhysicalFileName,
                    "FMSFLREQ_RequestedFromDate": $scope.from_date,
                    "FMSFLREQ_RequestedToDate": $scope.to_date
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                } 
                apiService.create("FMSApprovalRequestPage/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {

                        swal('Record saved successfully');


                    }
                    else if (promise.FMSFLREQ_Id > 0) {
                        swal('Record updated successfully');
                    }

                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };


        //added by akash (Edit)
        $scope.edit = function (item) {
            var data = {
                "FMSFLREQ_Id": item.fmsflreQ_Id
            }
            apiService.create("FMSApprovalRequestPage/editdetails", data).then(function (promise) {
                if (promise.editdata != null && promise.editdata.length > 0) {
                    $scope.editdata = promise.editdata;

                    
                    $scope.Filename = $scope.editdata[0].fmsmpfN_Id;
                    $scope.request = $scope.editdata[0].fmsflreQ_RequiredDays;
                    $scope.fmsflreQ_Id = $scope.editdata[0].fmsflreQ_Id;
                    $scope.fromdate = $scope.editdata[0].fmsflreQ_RequestedFromDate;
                    $scope.todate = $scope.editdata[0].fmsflreQ_RequestedToDate;
                }
            })
        }


        $scope.deactive = function (item, SweetAlert) {
            $scope.FMSFLREQ_Id = item.fmsflreQ_Id;
            var dystring = "";
            if (item.fmsflreQ_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.fmsflreQ_ActiveFlg == false) {
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
                        apiService.create("FMSApprovalRequestPage/deactive", item).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        }


        $scope.Clear = function () {
            $scope.HRMEID = '';
            $scope.remarks = '';
        }

        $scope.objdata = [];
        $scope.viewdocument = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;

            var data = {
                "FMSFLREQ_Id": $scope.objdata
            }


            apiService.create("FMSApprovalRequestPage/viewuploadflies", data).then(function (promise) {
                
                $scope.hrmetable = promise.hrmetable;
                $scope.iddetails = promise.iddata;
                $scope.flag = promise.fmsflreQ_ApprovedFlg;
            });
        };


        $scope.Updatedata = function () {
        
            var data = {
                "FMSFLREQ_Id": $scope.iddetails,
                "FMSFLREQ_HandedOverBy": $scope.HRMEID,
                "FMSFLREQ_ReturnRemarks": $scope.remarks 
            }


            apiService.create("FMSApprovalRequestPage/Updatedetails", data).then(function (promise) {
                if (promise.returnval == true) {

                    swal('Record saved successfully');
                    $scope.HRMEID = '';
                    $scope.remarks = '';

                }
                else if (promise.FMSFLREQ_Id > 0) {
                    swal('Record updated successfully');
                    $scope.HRMEID = '';
                    $scope.remarks = '';
                }
             


            })
        }


    }
})();
