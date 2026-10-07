(function () {
    'use strict';
    angular.module('app').controller('FMSApprovalController', FMSApprovalController)

    FMSApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$timeout', 'Excel']
    function FMSApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $timeout, Excel) {



        $scope.submitted = false;
        $scope.indentapproveddetais = [];
        $scope.maxdate = new Date();
        $scope.fromdate = new Date();
        $scope.todate = new Date();

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
        $scope.remarks_array = [];
        $scope.imgname = logopath;
        

        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            $scope.TableGrid = false;

            var pageid = 2;
           

            apiService.getURI("FMSApproval/loaddata", pageid).then(function (promise) {

                $scope.TableGrid = false;
                $scope.getdatas();
                
            });
        };

        $scope.getdatas = function () {
                $scope.from_date = new Date($scope.fromdate).toDateString();
                $scope.to_date = new Date($scope.todate).toDateString();
            var data = {
                "fromdate": $scope.from_date,
                "todate": $scope.to_date
            }

            apiService.create("FMSApproval/getdatas", data).then(function (promise) {
                if (promise.viewlist != null && promise.viewlist.length > 0) {
                    $scope.TableGrid = true;
                    $scope.DisplayApprovalTable = promise.viewlist;
                    $scope.presentCountgrid = $scope.DisplayApprovalTable.length;
                }

            });
        };


        $scope.toggleAll1 = function () {
            var toggleStatus = $scope.all1;
            angular.forEach($scope.DisplayApprovalTable, function (itm) {
                itm.checkedvalue1 = toggleStatus;
            });
        }

        $scope.optionToggled1 = function (x) {

        

            $scope.all1 = $scope.DisplayApprovalTable.every(function (itm) { return itm.checkedvalue1; })

        }


        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {

            return $scope.Accept;
        };

         
        $scope.AcceptorReject = function () {
            

            $scope.typevalue = true;
            $scope.approveddetails = [];
            angular.forEach($scope.DisplayApprovalTable, function (role) {
                if (role.checkedvalue1 == true) {
                    $scope.approveddetails.push({
                        HRME_Id: role.hrmE_Id,
                        FMSFLREQ_Id: role.FMSFLREQ_Id,
                        Approveddays: role.Approveddays,
                        remarks: role.remarks,
                        approved_flag: role.flag
                    });
                }
            })

                var data = {
                    "grid_arrays": $scope.approveddetails
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("FMSApproval/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {

                        swal('Record saved successfully');


                    }
                    else if (promise.FMSFLREQ_Id > 0) {
                        swal('Record updated successfully');
                    }

                    $state.reload();
                })
           
        };


        
      
    }
})();