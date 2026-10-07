(function () {
    'use strict';
    angular
        .module('app')
        .controller('SalesRawLeadVerifyTel', SalesRawLeadVerifyTel)
    SalesRawLeadVerifyTel.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function SalesRawLeadVerifyTel($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

        $scope.flagApproved = "";
        $scope.lead_list2 = [];
        $scope.Loaddata = function () {
            $scope.leadlistarray = [];
            $scope.leadlist = [];
            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            $scope.itemsPerPage = 10;
            $scope.getchanl = false;
            $scope.getchanlcheck = false;
            var pageid = 2;
            apiService.getURI("Sales_Lead/load_all_ChannelPartner", pageid).then(function (promise) {
                if (promise.personal_list_dd != null && promise.personal_list_dd.length > 0) {
                    $scope.lead_list2 = promise.personal_list_dd;
                    angular.forEach($scope.lead_list2, function (itm) {
                        itm.classname = 'neww';
                        itm.flag = "A";
                    });
                }

            });
        };
        $scope.radiochange = function (ee) {
            if (ee.flag === 'A') {

                ee.classname = 'neww';
                ee.ISMSLE_VerifedRemark = "Approved";
            }
            else {

                ee.classname = 'oldd';
                ee.ISMSLE_VerifedRemark = "Reject";
            }
        }
        //channel partner search
        $scope.searchchkbx = '';
        $scope.usercheckCC = function (obj) {
            return (angular.lowercase(obj.ismsmcP_ChannelPartnerName)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }
        $scope.searchchkbx1 = '';
        $scope.usercheckC = function (obj) {
            return (angular.lowercase(obj.ismslE_LeadName)).indexOf(angular.lowercase($scope.searchchkbx1)) >= 0;
        }


        //channel partner all check
        $scope.all_checkchannel = function () {

            var toggleStatus = $scope.usercheckCC;
            angular.forEach($scope.channalPatnerList, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.getlead();
        }

        //Lead all check
        $scope.all_checkC = function () {
            var toggleStatus = $scope.usercheckC;
            angular.forEach($scope.leadlists, function (role) {
                role.selected = toggleStatus;
            });

        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.lead_list2, function (itm) {
                itm.selected = toggleStatus;

            });
        }
        $scope.optionToggled = function () {

            $scope.all = $scope.lead_list2.every(function (options) {

                return options.selected;
            });
        }

        $scope.get_lead = function () {
            $scope.usercheckC = $scope.leadlists.every(function (options) {
                return options.selected;
            });
        }

        $scope.getlead = function () {
            $scope.IsHiddendown = false;
            $scope.usercheckCC = $scope.channalPatnerList.every(function (options) {
                return options.selected;
            });

            $scope.usercheckC = "";
            $scope.channellistarray = [];
            angular.forEach($scope.channalPatnerList, function (aa) {
                if (aa.selected == true) {
                    $scope.channellistarray.push({ ISMSMCP_Id: aa.ismsmcP_Id });
                }
            });

            if ($scope.channellistarray != null) {
                $scope.channelflag = true;
            }

            var data = {
                "channellsttwo": $scope.channellistarray
            }
            apiService.create("Sales_Lead/get_lead", data).then(function (promise) {
                if (promise.leadlists.length > 0 || promise.leadlists != null) {
                    $scope.leadlists = promise.leadlists;
                    $scope.channelflag = false;
                }
                else {
                    swal('No data Found!!!');
                }
            });
        }

        $scope.isOptionsRequiredchnl = function () {
            return !$scope.channalPatnerList.some(function (item) {
                return item.selected;
            });
        };
        $scope.isOptionsRequired = function () {
            return !$scope.leadlists.some(function (item) {
                return item.selected;
            });
        };

        $scope.viewdetails = function (dd) {
            $scope.submitted1 = false;

            var data = {
                "ISMSLE_Id": dd.ismslE_Id
            };
            apiService.create("Sales_Lead/viewdetails", data).then(function (promise) {
                $scope.leaddetails = promise.leaddetails;
                $('#myModalreadmitview').modal('show');

            });
        };



        $scope.interactedradio = function (field) {
            return $scope.submittedradio;
        };

        $scope.submittedradio = false;
        $scope.getdetails = function () {
            $scope.submittedradio = true;

            if ($scope.myForm.$valid) {
                $scope.channellistarray = [];
                angular.forEach($scope.channalPatnerList, function (aa) {
                    if (aa.selected == true) {
                        $scope.channellistarray.push({ ismsmcP_Id: aa.ismsmcP_Id })
                    }

                });
                $scope.leadlistarray = [];
                angular.forEach($scope.leadlists, function (aa) {
                    if (aa.selected == true) {
                        $scope.leadlistarray.push({ ismslE_Id: aa.ismslE_Id })
                    }
                });

                var data = {

                    "channellsttwo": $scope.channellistarray,
                    "Leadlsttwo": $scope.leadlistarray,
                };
                apiService.create("Sales_Lead/getdetails", data).
                    then(function (promise) {
                        $scope.lead_list2 = promise.lead_list2;
                        if ($scope.lead_list2.length > 0 || $scope.lead_list2 == null) {
                            $scope.IsHiddendown = true;
                        } else {
                            swal("No Records Found");
                            $scope.IsHiddendown = false;

                        }

                    });


            }


        };


        //Save======================================================================
        $scope.submitted = false;
        $scope.channelList = [];
        $scope.SaveLeadVerify = function () {
            $scope.leadverify = [];
            if ($scope.myForm.$valid) {
                angular.forEach($scope.lead_list2, function (aa) {
                    if (aa.selected == true && aa.ISMSLE_VerifedRemark != null && aa.ISMSLE_VerifedRemark != "") {
                        $scope.leadverify.push({
                            ISMSLE_Id: aa.ismslE_Id,
                            flag: aa.flag,
                            ISMSLE_ConfirmedBy: aa.ISMSLE_ConfirmedBy,
                            ISMSLE_VerifedRemark: aa.ISMSLE_VerifedRemark
                        })
                    }
                });
                if ($scope.leadverify != null && $scope.leadverify.length > 0) {
                    var data = {
                        "Leadlsttwo": $scope.leadverify,
                        "retrunMsg": "Raw_data_tel"
                    }
                    apiService.create("Sales_Lead/SaveVerifyflag", data).
                        then(function (promise) {
                            if (promise.msg == 'Update') {
                                swal("Record Update Successfully!");
                                $state.reload();
                            }
                            else if (promise.msg == "UpdateFailed") {
                                swal(" Please Select Verify Check box");
                            }
                            else {
                                swal("Sorry...something went wrong");
                            }

                        })
                }
                else {
                    swal("Please Select  Atleast one CheckBox /Remarks");
                }

            }
            else {
                $scope.submitted = true;
            }


        }


        $scope.cleardata = function () {
            $scope.usercheckCC = "";
            $scope.usercheckC = "";
            $state.reload();
        }

        //$scope.ApprovedChange = function () {
        //angular.forEach($scope.lead_list2, function (aa) {
        //    aa.ISMSLE_VerifedRemark = $scope.flagApproved;

        //});
        // }
        $scope.ApprovedChange = function () {
            if ($scope.flagApproved === "Reject") {

                angular.forEach($scope.lead_list2, function (itm) {
                    itm.classname = 'oldd';
                    itm.flag = "R";
                    itm.ISMSLE_VerifedRemark = $scope.flagApproved;
                });
            }
            else {

                angular.forEach($scope.lead_list2, function (itm) {
                    itm.classname = 'neww';
                    itm.flag = "A";
                    itm.ISMSLE_VerifedRemark = $scope.flagApproved;
                });

            }

        }


    }
})();

