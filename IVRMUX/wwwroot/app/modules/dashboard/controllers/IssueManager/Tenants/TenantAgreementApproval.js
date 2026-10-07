(function () {
    'use strict';
    angular.module('app').controller('TenantAgreementApprovalController', TenantAgreementApprovalController)

    TenantAgreementApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$timeout', 'Excel']
    function TenantAgreementApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $timeout, Excel) {



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
        $scope.remarks_array = [];
        $scope.imgname = logopath;


        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            $scope.TableGrid = false;

            var pageid = 2;


            apiService.getURI("TenantAgreementApproval/loaddata", pageid).then(function (promise) {
                $scope.getinstitution = promise.getinstitution;

                $scope.TableGrid = false;


            });
        };

        $scope.getreports = function () {
            var data = {
                "fromdate": new Date($scope.fromdate).toDateString(),
                "todate": new Date($scope.todate).toDateString(),
                "MI_Id": $scope.mI_Id
            }

            apiService.create("TenantAgreementApproval/getdatas", data).then(function (promise) {
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
                        TENANTAGRAPR_FApprovedBy: role.tenantagrapR_FApprovedBy,
                        TENANTAGR_Id: role.TENANTAGR_Id,
                        TENANTAGRAPR_Remarks: role.remarks,
                        TENANTAGRAPR_ApprovedFlg: role.flag
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
            apiService.create("TenantAgreementApproval/savedetails", data).then(function (promise) {

                if (promise.returnval == "Add") {
                    $scope.getreports();
                    swal('Record saved / Updated successfully');

                }
                else  {
                    swal('Record Request failed');
                }

                 // $state.reload();
               
            })

        };
        $scope.Clearid = function () {
            $state.reload();
        }



    }
})();