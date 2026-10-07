(function () {
    'use strict'; angular.module('app')
        .controller('DepartmentWiseSalaryReportController', DepartmentWiseSalaryReportController)
    DepartmentWiseSalaryReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function DepartmentWiseSalaryReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {

        //CanvasJS.addColorSet("graphcolor",
        //    [//colorSet Array
        //        "#3498DB",
        //        "#76D7C4",
        //        "#808B96",
        //        "#80DEEA",
        //        "#C5E1A5",
        //        "#AAB7B8"
        //    ]);

        CanvasJS.addColorSet("graphcolor",
            [//colorSet Array
                "#DDDF00",
                "#24CBE5",
                "#50B432",
                "#ED561B",
                "#64E572",
                "#FF9655"
            ]);



        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("DepartmentWiseSalaryReport/getalldetails", pageid).then(function (promise) {
                //dropdown list
                $scope.get_mi_list = promise.get_mi_list1;
                $scope.monthlist = promise.monthlist;
                $scope.yearlist = promise.yearlist;


            })
        }




        $scope.get_depts = function () {
            var ids = [];
            $scope.departmentdropdown = [];
            angular.forEach($scope.get_mi_list, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.mI_Id);
                }
            })
            var data = {
                MI_Idlist: ids
            }
            apiService.create("DepartmentWiseSalaryReport/get_depts", data).then(function (promise) {
                if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                    $scope.departmentdropdown = promise.departmentdropdown;
                    //$scope.dprtmetall = true;
                    //$scope.alldepartment();
                }
            })
        };


        $scope.all_check = function () {
            var toggleStatus = $scope.Allmi_id;
            angular.forEach($scope.get_mi_list, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_depts();
        }

        $scope.alldepartment = function (dprtmetall) {
            var toggleStatus1 = dprtmetall;
            angular.forEach($scope.departmentdropdown, function (itm) {
                itm.selected1 = toggleStatus1;
            });
        }

        $scope.isOptionsRequired = function () {
            return !$scope.get_mi_list.some(function (options) {
                return options.selected;
            });
        }
        $scope.isOptionsRequired1 = function () {
            return !$scope.departmentdropdown.some(function (options) {
                return options.selected;
            });
        }
        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };


        $scope.Getreport = function () {
           // if ($scope.myForm.$valid) {
                var ids1 = [];
                //$scope.getReportvalue = [];
                var total = 0;
                angular.forEach($scope.departmentdropdown, function (grp_t) {
                    if (grp_t.selected1) {
                        ids1.push(grp_t.hrmD_Id);
                    }
                })
                var data = {
                    IVRM_Month_Name: $scope.ivrM_Month_Name,
                    ASMAY_Year: $scope.hrmlY_LeaveYear,
                    hrmD_IdList: ids1
                }
                apiService.create("DepartmentWiseSalaryReport/Getreport", data).then(function (promise) {
                    if (promise.getReportvalue !== null && promise.getReportvalue.length > 0) {
                        $scope.getReportvalue = promise.getReportvalue;
                    }

                    angular.forEach(promise.getReportvalue, function (nt) {
                        if (nt.Salary > 0) {
                            total += nt.Salary;
                        }
                        $scope.TotalAmt = total;
                    })
                    $scope.departmentgraph = [];
                    angular.forEach($scope.getReportvalue, function (dd) {
                        $scope.departmentgraph.push({
                            label: dd.HRMD_DepartmentName,
                            y: dd.Salary,
                        })
                    })


                    var chart = new CanvasJS.Chart("columnchart", {
                        title: {
                            text: "Department Wise Monthly Salary",

                        },
                        animationEnabled: true,
                        animationDuration: 3000,
                        height: 350,
                        backgroundColor: "#3d4347",
                        //colorSet: "graphcolor",

                        axisX: {
                            labelFontSize: 12,
                        },
                        axisY: {
                            labelFontSize: 12,
                        },

                        toolTip: {
                            shared: true
                        }, data: [{
                            // backgroundColor: "#3d4347",
                            // showInLegend: true,
                            toolTipContent: "<b>{label}</b>: {y}",
                            type: "column",
                            color: "#3aa8fc",
                            dataPoints: $scope.departmentgraph
                        },]


                    });
                    chart.render();
                    var chart = new CanvasJS.Chart("linechart", {
                        title: {
                            text: "Department Wise Monthly Salary"
                        },
                        animationEnabled: true,
                        animationDuration: 3000,
                        height: 350,
                        colorSet: "graphcolor",
                        axisX: {
                            interval: 1,
                            labelFontSize: 12,
                        },
                        axisY: {
                            labelFontSize: 12,
                        },

                        data: [
                            {
                                type: "pie",
                                toolTipContent: "<b>{label}</b>: {y}",
                                innerRadius: 90,
                                //showInLegend: true,
                                dataPoints: $scope.departmentgraph,
                            }
                        ]
                    });
                    chart.render();
                    var chart = new CanvasJS.Chart("linechartmulti", {
                        title: {
                            text: "Department Wise Monthly Salary"
                        },
                        animationEnabled: true,
                        animationDuration: 3000,
                        height: 350,
                        // colorSet: "graphcolor",
                        axisX: {
                            interval: 1,
                            labelFontSize: 12,
                        },
                        axisY: {
                            labelFontSize: 12,
                        },

                        data: [
                            {
                                type: "line",
                                toolTipContent: "<b>{label}</b>: {y}",
                                innerRadius: 90,
                                color: "#3aa8fc",
                                //showInLegend: true,
                                dataPoints: $scope.departmentgraph,
                            }
                        ]
                    });
                    chart.render();

                    function explodePie(e) {
                        if (typeof (e.dataSeries.dataPoints[e.dataPointIndex].exploded) === "undefined" || !e.dataSeries.dataPoints[e.dataPointIndex].exploded) {
                            e.dataSeries.dataPoints[e.dataPointIndex].exploded = true;
                        } else {
                            e.dataSeries.dataPoints[e.dataPointIndex].exploded = false;
                        }
                        e.chart.render();
                    }


                });
            //}
            //else {
            //    $scope.submitted = true;
            //}
        }

        $scope.Clearall = function () {
            $state.reload();
        }
    }
})();



