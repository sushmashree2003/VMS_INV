(function () {
    'use strict';
    angular
.module('app')
.controller('CumulativeFeeAnalysisController', CumulativeFeeAnalysisController)

    CumulativeFeeAnalysisController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function CumulativeFeeAnalysisController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

        $scope.overallfee = false;
        $scope.detailsfee = false;
        CanvasJS.addColorSet("graphcolor",
                      [//colorSet Array
                      "#3498DB",
                      "#76D7C4",
                      "#808B96",
                      "#80DEEA",
                      "#C5E1A5",
                      "#AAB7B8"
                      ]);
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            
            apiService.getDATA("CumulativeFeeAnalysis/getloaddata").
                then(function (promise) {
                    
                    $scope.feeAnalysislst = promise.feeAnalysisList;


                    //--------------------Graph
                    //$scope.analysisgraph = [];
                    //if ($scope.feeAnalysislst != null) {
                    //    for (var a = 0; a < $scope.feeAnalysislst.length; a++) {
                    //        $scope.analysisgraph.push({ label: $scope.feeAnalysislst[a].ASMAY_Year, "y": $scope.feeAnalysislst[a].RECEIVABLE })
                    //    }
                    //}
                    //$scope.analysisgraph1 = [];
                    //if ($scope.feeAnalysislst != null) {
                    //    for (var a = 0; a < $scope.feeAnalysislst.length; a++) {
                    //        $scope.analysisgraph1.push({ label: $scope.feeAnalysislst[a].ASMAY_Year, "y": $scope.feeAnalysislst[a].CONCESSION })
                    //    }
                    //}
                    //$scope.analysisgraph2 = [];
                    //if ($scope.feeAnalysislst != null) {
                    //    for (var a = 0; a < $scope.feeAnalysislst.length; a++) {
                    //        $scope.analysisgraph2.push({ label: $scope.feeAnalysislst[a].ASMAY_Year, "y": $scope.feeAnalysislst[a].COLLECTION })
                    //    }
                    //}
                    //$scope.analysisgraph3 = [];
                    //if ($scope.feeAnalysislst != null) {
                    //    for (var a = 0; a < $scope.feeAnalysislst.length; a++) {
                    //        $scope.analysisgraph3.push({ label: $scope.feeAnalysislst[a].ASMAY_Year, "y": $scope.feeAnalysislst[a].ADJUSTED })
                    //    }
                    //}
                    //$scope.analysisgraph4 = [];
                    //if ($scope.feeAnalysislst != null) {
                    //    for (var a = 0; a < $scope.feeAnalysislst.length; a++) {
                    //        $scope.analysisgraph4.push({ label: $scope.feeAnalysislst[a].ASMAY_Year, "y": $scope.feeAnalysislst[a].BALANCE })
                    //    }
                    //}
                    $scope.head = ["RECEIVABLE", "CONCESSION", "COLLECTION", "ADJUSTED", "BALANCE"]
                    $scope.analysisgraph = [];
                    $scope.analysisgraph1 = [];
                    if ($scope.feeAnalysislst != null) {
                        for (var a = 0; a < $scope.feeAnalysislst.length; a++) {

                            $scope.analysisgraph.push({ label: $scope.head[0], y: $scope.feeAnalysislst[a].RECEIVABLE, name: "RECEIVABLE" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                            $scope.analysisgraph1.push({ indexLabel: $scope.head[0], y: $scope.feeAnalysislst[a].RECEIVABLE, name: "RECEIVABLE" + '-' + $scope.feeAnalysislst[a].ASMAY_Year })

                        }
                    }
                    //  $scope.analysisgraph1 = [];
                    if ($scope.feeAnalysislst != null) {
                        for (var a = 0; a < $scope.feeAnalysislst.length; a++) {

                            $scope.analysisgraph.push({ label: $scope.head[1], y: $scope.feeAnalysislst[a].CONCESSION, name: "CONCESSION" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                            $scope.analysisgraph1.push({ indexLabel: $scope.head[1], y: $scope.feeAnalysislst[a].CONCESSION, name: "CONCESSION" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });

                        }
                    }

                    if ($scope.feeAnalysislst != null) {
                        for (var a = 0; a < $scope.feeAnalysislst.length; a++) {

                            $scope.analysisgraph.push({ label: $scope.head[2], y: $scope.feeAnalysislst[a].COLLECTION, name: "COLLECTION" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                            $scope.analysisgraph1.push({ indexLabel: $scope.head[2], y: $scope.feeAnalysislst[a].COLLECTION, name: "COLLECTION" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                        }
                    }

                    if ($scope.feeAnalysislst != null) {
                        for (var a = 0; a < $scope.feeAnalysislst.length; a++) {
                            $scope.analysisgraph.push({ label: $scope.head[3], y: $scope.feeAnalysislst[a].ADJUSTED, name: "ADJUSTMENT" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                            $scope.analysisgraph1.push({ indexLabel: $scope.head[3], y: $scope.feeAnalysislst[a].ADJUSTED, name: "ADJUSTMENT" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                        }
                    }

                    if ($scope.feeAnalysislst != null) {
                        for (var a = 0; a < $scope.feeAnalysislst.length; a++) {
                            $scope.analysisgraph.push({ label: $scope.head[4], y: $scope.feeAnalysislst[a].BALANCE, name: "BALANCE" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                            $scope.analysisgraph1.push({ indexLabel: $scope.head[4], y: $scope.feeAnalysislst[a].BALANCE, name: "BALANCE" + '-' + $scope.feeAnalysislst[a].ASMAY_Year });
                        }
                    }

                    var chart = new CanvasJS.Chart("columnchart", {
                        animationEnabled: true,
                        animationDuration: 3000,
                        height: 350,
                        colorSet: "graphcolor",
                        axisX: {
                            labelFontSize: 12,                           
                        },
                        axisY: {
                            labelFontSize: 12,
                        },

                        toolTip: {
                            shared: true
                        },
                        data: [
                                {
                                    type: "column",
                                    showInLegend: false,
                                    dataPoints: $scope.analysisgraph
                                }
                        ]
                    });
                    chart.render();

                    var chart = new CanvasJS.Chart("chartContainer", {
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
                                 type: "doughnut",
                                 innerRadius: 90,
                                 showInLegend: true,
                                 dataPoints: $scope.analysisgraph1
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
                    //var chart = new CanvasJS.Chart("columnchart", {
                    //    animationEnabled: true,
                    //    animationDuration: 3000,

                    //    axisX: {
                    //        labelFontSize: 12,
                    //    },
                    //    axisY: {
                    //        labelFontSize: 12,
                    //    },

                    //    toolTip: {
                    //        shared: true
                    //    },
                    //    data: [{
                    //        name: "RECEIVABLE",
                    //        showInLegend: true,
                    //        type: "column",
                    //        color: "rgba(40,175,101,0.6)",
                    //        dataPoints: $scope.analysisgraph
                    //    },
                    //    {
                    //        name: "CONCESSION",
                    //        showInLegend: true,
                    //        type: "column",
                    //        color: "rgba(0,75,141,0.7)",
                    //        dataPoints: $scope.analysisgraph1
                    //    },
                    //     {
                    //         name: "COLLECTION",
                    //         showInLegend: true,
                    //         type: "column",
                    //         color: "rgba(255,255,0.7)",
                    //         dataPoints: $scope.analysisgraph2
                    //     },
                    //       {
                    //           name: "ADJUSTMENT",
                    //           showInLegend: true,
                    //           type: "column",
                    //           color: "rgba(255,0,0,0.4)",
                    //           dataPoints: $scope.analysisgraph3
                    //       },
                    //         {
                    //             name: "BALANCE",
                    //             showInLegend: true,
                    //             type: "column",
                    //             color: "rgba(0,0,255,0.5)",
                    //             dataPoints: $scope.analysisgraph4
                    //         }
                    //    ]
                    //});
                    //chart.render();

                    //var chart = new CanvasJS.Chart("chartContainer", {
                    //    animationEnabled: true,
                    //    animationDuration: 3000,
                    //    axisX: {
                    //        labelFontSize: 12,
                    //    },
                    //    axisY: {
                    //        labelFontSize: 12,
                    //    },

                    //    toolTip: {
                    //        shared: true
                    //    },
                    //    data: [{
                    //        name: "RECEIVABLE",
                    //        showInLegend: true,
                    //        type: "area",
                    //        color: "rgba(40,175,101,0.6)",
                    //        dataPoints: $scope.analysisgraph
                    //    },
                    //    {
                    //        name: "CONCESSION",
                    //        showInLegend: true,
                    //        type: "area",
                    //        color: "rgba(0,75,141,0.7)",
                    //        dataPoints: $scope.analysisgraph1
                    //    },
                    //    {
                    //        name: "COLLECTION",
                    //        showInLegend: true,
                    //        type: "area",
                    //        color: "rgba(0,75,141,0.7)",
                    //        dataPoints: $scope.analysisgraph2
                    //    },
                    //     {
                    //         name: "ADJUSTMENT",
                    //         showInLegend: true,
                    //         type: "area",
                    //         color: "rgba(255,0,0,0.4)",
                    //         dataPoints: $scope.analysisgraph3
                    //     },
                    //         {
                    //             name: "BALANCE",
                    //             showInLegend: true,
                    //             type: "area",
                    //             color: "rgba(0,0,255,0.5)",
                    //             dataPoints: $scope.analysisgraph4
                    //         }
                    //    ]
                    //});
                    //chart.render();


                })
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }



    };
})();