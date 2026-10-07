(function () {
    'use strict';
    angular
        .module('app')
        .controller('GeneralRequestController', GeneralRequestController)
    GeneralRequestController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce']
    function GeneralRequestController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce) {


        $scope.FOEST_Id = 0;
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.FOEST_Date = new Date;
        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };
        $scope.ismeridian = false;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };
        $scope.morning = false;
        $scope.lunch = false;
        $scope.all = false;
        $scope.showdepartment = false;
        $scope.showdesignation = false;
        $scope.showdesignation1 = false;

        $scope.holi = {};


        //validation start
        $scope.valstrtm = function (timedata) {
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;

            $scope.min.setMinutes(hh);
            $scope.min.setMinutes(mm);
            $scope.minlnc = timedata;

            $scope.minlnc.setMinutes(hh);
            $scope.minlnc.setMinutes(mm);
            $scope.FOMST_IIHalfLogoutTime = "";
        };

        $scope.CheckedClassName = function (data) {
            if ($scope.checkboxchcked.indexOf(data) === -1) {
                $scope.checkboxchcked.push(data);
            }
            else {
                $scope.checkboxchcked.splice($scope.checkboxchcked.indexOf(data), 1);
            }
        };

        $scope.validatemax = function (maxdata) {
            if (maxdata >= new Date($scope.fomsT_IHalfLoginTime)) {
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
        };

        $scope.validateTomintime1 = function (timedata1) {
            $scope.totime1 = timedata1;
            var hh = $scope.totime1.getHours();
            var mm = $scope.totime1.getMinutes();
            $scope.min1 = timedata1;

            $scope.min1.setMinutes(hh);
            $scope.min1.setMinutes(mm);
            $scope.FOMST_IIHalfLoginTime = timedata1;
        };

        $scope.validatefromtime1 = function (maxdata1) {            
            if (maxdata1 >= new Date($scope.FOMST_IHalfLogoutTime)) {  
                $scope.FOMST_IIHalfLoginTime = new Date($scope.FOMST_IHalfLogoutTime);
            }
            else {
                $scope.FOMST_IIHalfLoginTime = "";
            }
        };

        $scope.loadData = function () {
            var date = new Date();
            var id = 2;          
            apiService.getURI("GeneralRequestProcess/getpunch", id).then(function (promise) {
                $scope.empname = promise.empname;
                $scope.punchdetails = promise.punchdetails; 
                $scope.hrmE_EmployeeFirstName = $scope.empname[0].hrmE_EmployeeFirstName;
                $scope.hrmE_DOJ = $scope.empname[0].hrmE_DOJ;
                $scope.hrmdeS_DesignationName = $scope.empname[0].hrmdeS_DesignationName;
                $scope.hrmE_EmailId = $scope.empname[0].hrmE_EmailId;
                $scope.hrmE_MobileNo = $scope.empname[0].hrmE_MobileNo;
                $scope.hrmE_Id = $scope.empname[0].hrmE_Id;
                $scope.contact = $scope.empname[0].hrmE_MobileNo; 
                $scope.Onshiftname();
             });
        
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };

         $scope.Onshiftname = function () {

            if ($scope.hrmE_Id > 0) {
                var data = {
                    HRME_Id: $scope.hrmE_Id,
                    empdate: $scope.FOEST_Date
                };
                apiService.create("GeneralRequestProcess/emprequest", data).then(
                    function (promise) {
                        $scope.showdepartment1 = true;
                        if (promise.employeelist != null) {
                            $scope.employeelist = promise.employeelist;
                            $scope.foepid = promise.employeelist[0].foeP_Id;
                            angular.forEach($scope.employeelist, function (dd) {
                                dd.foepD_PunchTime = moment(dd.foepD_PunchTime, 'H:mm').format();
                                $scope.timedis = true;
                            });
                        }
                        else {                           
                            $scope.employeelist = [];
                            $scope.foepid = 0;                            
                        }
                    });
            }
            else {
                $scope.morning = false;
                $scope.lunch = false;
                $scope.all = false;
            }
        };

        $scope.saveddata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "FOEPAR_Id": $scope.FOEPAR_Id,
                    //"FOEPD_PunchTime": $filter('date')($scope.timpic, "HH:mm"),
                    //"FOEP_Id": $scope.foepid,
                    //"FOEPD_Id": $scope.foepD_Id,
                    "HRME_Id": $scope.hrmE_Id,
                    "FOEPAR_PunchDate": $scope.FOEST_Date,
                    "FOEPAR_Remarks": $scope.FOEPAR_Remarks,
                    "FOEPARD_Time": $filter('date')($scope.FOEPARD_Time, "hh:mma"),
                    "FOEPARD_InOutFlg": $scope.inout
                };

                apiService.create("GeneralRequestProcess/grsavedetail", data).
                    then(function (promise) {

                         if (promise.retrunMsg == 'updated') {                            
                             swal('Record saved successfully');
                             $state.reload();
                                                      
                        }
                        else if (promise.returnduplicatestatus == 'Duplicate') {
                            swal('Record already exist');
                        }
                        else {
                            if (promise.foepD_Id == 0 || promise.foepD_Id < 0) {
                                swal('Failed to save, please contact administrator');
                            }
                            else if (promise.foepD_Id > 0) {
                                swal('Failed to update, please contact administrator');
                            }
                        } 
                    });

            }
            else {
                
            }

        };
    }
})();
