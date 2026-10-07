(function () {
    'use strict';
    angular
.module('app')
.controller('GroupwiseSubListReportController', GroupwiseSubListReportController)

    GroupwiseSubListReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function GroupwiseSubListReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.BindData = function () {
         
            apiService.getDATA("GroupwiseSubListReport/getdetails").
            then(function (promise) {
                $scope.qualification_type = 'individual';              
                $scope.group = promise.group;
                $scope.examlist = promise.examlist;
            })
        };

        $scope.subrow = 2;

        $scope.submitted = false;
        $scope.onreport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                if ($scope.qualification_type == 'all')
                {
                    $scope.EMG_Id = 0;
                }

                var data = {
                    "EMG_Id": $scope.EMG_Id,
                    "EME_Id": $scope.EME_Id, 
                    "report_type": $scope.qualification_type,
                }

                apiService.create("GroupwiseSubListReport/onreport", data).
                then(function (promise) {
                    if ($scope.qualification_type == 'all') {
                        $scope.EMG_Id = "";
                    }
                    $scope.main_list = promise.groupname;
                    $scope.sub_list = promise.subjectname;
                    $scope.flag_list = promise.applicable_flag;
                    //$scope.grp_subj_details = promise.grp_subjects;
                    console.log($scope.main_list);
                    angular.forEach($scope.sub_list, function (sub) {
                        var temp_subdetails = [];
                        angular.forEach(promise.applicable_flag, function (sd) {
                            if (sd.ismS_Id == sub.ismS_Id) {
                                temp_subdetails.push(sd);
                            }
                        })
                        if (temp_subdetails.length == 0)
                        {
                            sub.eyceS_AplResultFlg = undefined;
                        }
                        else if (temp_subdetails.length > 0)
                        {
                            var applicable_flag = Math.max.apply(Math, temp_subdetails.map(function (item) { return item.eyceS_AplResultFlg; }));
                            sub.eyceS_AplResultFlg = applicable_flag;
                        }
                    })
                    console.log($scope.sub_list);
                })
            }
        };

        $scope.onselectradio = function () {
            if ($scope.qualification_type == 'all') {
                  $scope.group_disable = true;
                  $scope.EMG_Id = "";
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.search = "";
            }
            else {
                  $scope.group_disable = false;
            }
        };

        $scope.cancel = function () {
            $scope.EMG_Id = '';
            $scope.EME_Id = '';
            $scope.main_list = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.printData = function () {
            
            var innerContents = document.getElementById("table").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                  '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

    }

})();