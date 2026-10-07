(function () {
    'use strict';
    angular
.module('app')
        .controller('ChannelpartnerDashboardController', ChannelpartnerDashboardController)

    ChannelpartnerDashboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache']
    function ChannelpartnerDashboardController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache) {
        $scope.LoadData = function () {
            
            apiService.getDATA("EmployeeStudentSearch/getdetails")
                .then(function (promise) {
                    
                    $scope.studentlst = promise.fillstudent;

                })
        };


        $scope.searchfilter = function (studentlst) {
            
            var studid = studentlst.amst_Id;
            var data = {
                "Amst_Id": studid
            }


            apiService.create("EmployeeStudentSearch/getstudentdetails", data).
                then(function (promise) {
                    
                    $scope.studentlistall = promise.fillstudentalldetails;
                    $scope.examdetails = promise.examlist;
                    if ($scope.studentlistall != null) {
                        $scope.showStudentD = true;

                    } else {
                        $scope.showStudentD = false;

                    }
                    if ($scope.examdetails != null && $scope.examdetails != 0) {
                        $scope.showExamD = true;

                    } else {
                        $scope.showExamD = false;

                        swal("Student did't attend any Exam....!!")
                    }

                })
        };
        //$scope.searchfilter = function (data) {
        //    
        //    apiService.getURI("EmployeeStudentSearch/getstudentdetails", data).
        //        then(function (promise) {
        //        $scope.studentlistall = promise.fillstudentalldetails;
        //        if ($scope.studentlistall != null) {
        //            $scope.showStudentD = true;
        //        } else {
        //            $scope.showStudentD = false;
        //        }

        //    })
        //};


    };
})();