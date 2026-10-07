(function () {
    'use strict';
    angular
        .module('app')
        .controller('Student_TTController', Student_TTController)

    Student_TTController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function Student_TTController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

        //CanvasJS.addColorSet("graphcolor",
        //                    [//colorSet Array
        //                    "#3498DB",
        //                    "#76D7C4",
        //                    "#808B96",
        //                    "#80DEEA",
        //                    "#C5E1A5",
        //                    "#AAB7B8"
        //    ]);

        $scope.tt_grid = false;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("Student_TT/getloaddata", pageid).

                then(function (promise) {
                    $scope.getyear = promise.getyear;


                })
        };


        //-----------academicyear Selection
        $scope.onyearchange = function (asmaY_Id) {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
            }
            apiService.create("Student_TT/getStudentTT", data).
                then(function (promise) {

                    if (promise.getStudentTT != null && promise.getStudentTT.length != 0) {
                        $scope.tt_grid = true;
                        $scope.getStudentTT = promise.getStudentTT;
                    }
                    else {
                        swal("No Record Found....!!");
                        $scope.tt_grid = false;
                    }

                })
        }
        //-----------Subject Selection
        //$scope.examchange = function (emE_Id) {

        //    var data = {
        //        "EME_Id": $scope.emE_Id,
        //    }
        //    apiService.create("ExamReport/getexamdetails", data).
        //       then(function (promise) {

        //           $scope.examsubjdetails = promise.examsubjdetails;
        //           $scope.hide_examg = true;

        //           //--------------------Graph

        //           $scope.examrptgraph = [];
        //           $scope.examrptgraph1 = [];
        //           if ($scope.examsubjdetails.length != "0" && $scope.examsubjdetails != null) {
        //               $scope.attdetails = true;
        //               for (var i = 0; i < $scope.examsubjdetails.length; i++) {
        //                   $scope.examrptgraph.push({ label: $scope.examsubjdetails[i].ismS_SubjectName, "y": $scope.examsubjdetails[i].estmpS_ObtainedMarks, name: $scope.examsubjdetails[i].ismS_SubjectName})
        //               }
        //               //for (var i = 0; i < $scope.examsubjdetails.length; i++) {
        //               //    $scope.examrptgraph1.push({ "y": parseInt($scope.examsubjdetails[i].estmpS_MaxMarks) })
        //               //}

        //               var chart = new CanvasJS.Chart("columnchart", {
        //                   animationEnabled: true,
        //                   animationDuration: 3000,
        //                   height: 350,
        //                   colorSet: "graphcolor",
        //                   axisX: {
        //                       labelFontSize: 12,
        //                       interval: 1
        //                   },
        //                   axisY: {
        //                       labelFontSize: 12,
        //                   },

        //                   toolTip: {
        //                       shared: true
        //                   },
        //                   data: [{
        //                       // name: "Obtained Marks",
        //                       showInLegend: false ,
        //                       type: "column",
        //                       dataPoints: $scope.examrptgraph
        //                   }
        //                   //{
        //                   //    name: "Max Marks",
        //                   //    showInLegend: true,
        //                   //    type: "column",                           
        //                   //    dataPoints: $scope.examrptgraph1
        //                   //}
        //                   ]
        //               });
        //               chart.render();
        //               var chart = new CanvasJS.Chart("linechart", {
        //                   animationEnabled: true,
        //                   animationDuration: 3000,
        //                   height: 350,
        //                   colorSet: "graphcolor",
        //                   axisX: {
        //                       labelFontSize: 12,
        //                       interval: 1
        //                   },
        //                   axisY: {
        //                       labelFontSize: 12,
        //                       interval: 1
        //                   },
        //                   toolTip: {
        //                       shared: true
        //                   },
        //                   data: [{

        //                       showInLegend: true,
        //                       type: "doughnut",
        //                       radius: "100%",
        //                       dataPoints: $scope.examrptgraph
        //                   }
        //                   //{
        //                   //    name: "Max Marks",
        //                   //    showInLegend: true,
        //                   //    type: "area",                            
        //                   //    dataPoints: $scope.examrptgraph1
        //                   //}
        //                   ]
        //               });
        //               chart.render();
        //           }
        //           else {
        //               swal("No Record Found....")
        //           }
        //           //if ($scope.examgradelst != null && $scope.examgradelst.length != "0") {
        //           //    for (var a = 0; a < $scope.examgradelst.length; a++) {
        //           //        $scope.examgraph.push({ label: $scope.examgradelst[a].asmaY_Year, "y": $scope.examgradelst[a].estmpS_MaxMarks })
        //           //        $scope.examgraphd.push({ indexLabel: $scope.ehead[0], y: $scope.examgradelst[a].estmpS_MaxMarks, name: "Max Marks" });
        //           //    }
        //           //    $scope.examgraph1 = [];
        //           //    for (var a = 0; a < $scope.examgradelst.length; a++) {
        //           //        $scope.examgraph1.push({ label: $scope.examgradelst[a].asmaY_Year, "y": $scope.examgradelst[a].estmpS_ObtainedMarks })
        //           //        $scope.examgraphd.push({ indexLabel: $scope.ehead[1], y: $scope.examgradelst[a].estmpS_ObtainedMarks, name: "Obtained Marks" });
        //           //    }
        //           //    $scope.examgraph2 = [];
        //           //    for (var a = 0; a < $scope.examgradelst.length; a++) {
        //           //        $scope.percentage = ($scope.examgradelst[a].estmpS_ObtainedMarks / $scope.examgradelst[a].estmpS_MaxMarks) * 100;
        //           //        $scope.examgraph2.push({ label: $scope.examgradelst[a].asmaY_Year, "y": $scope.percentage })
        //           //        $scope.examgraphd.push({ indexLabel: $scope.ehead[2], y: $scope.percentage, name: "Marks Percentage (%)" });
        //           //    }
        //           //}
        //           //else {
        //           //    swal("No Record Found....")
        //           //    $scope.hide_examg = false;
        //           //}

        //           //var chart = new CanvasJS.Chart("columnchart", {
        //           //    animationEnabled: true,
        //           //    animationDuration: 3000,
        //           //    colorSet: "graphcolor",
        //           //    axisX: {
        //           //        labelFontSize: 12,
        //           //    },
        //           //    axisY: {
        //           //        labelFontSize: 12,
        //           //    },

        //           //    toolTip: {
        //           //        shared: true
        //           //    },
        //           //    data: [{
        //           //        name: "TOTAL MARKS",
        //           //        showInLegend: true,
        //           //        type: "column",
        //           //        // color: "rgba(40,175,101,0.6)",
        //           //        dataPoints: $scope.examgraph
        //           //    },
        //           //    {
        //           //        name: "MARKS OBTAIN",
        //           //        showInLegend: true,
        //           //        type: "column",
        //           //        //color: "rgba(0,75,141,0.7)",
        //           //        dataPoints: $scope.examgraph1
        //           //    },
        //           //     {
        //           //         name: "PERCENTAGE (%)",
        //           //         showInLegend: true,
        //           //         type: "column",
        //           //         // color: "rgba(0, 212, 255, 0.43)",
        //           //         dataPoints: $scope.examgraph2
        //           //     }
        //           //    ]
        //           //});
        //           //chart.render();

        //           //var chart = new CanvasJS.Chart("linechart", {
        //           //    animationEnabled: true,
        //           //    animationDuration: 3000,
        //           //    colorSet: "graphcolor",

        //           //    axisX: {
        //           //        interval: 1,
        //           //        labelFontSize: 12,
        //           //    },
        //           //    axisY: {
        //           //        labelFontSize: 12,
        //           //    },

        //           //    data: [
        //           //         {
        //           //             type: "doughnut",
        //           //             innerRadius: 90,
        //           //             showInLegend: true,
        //           //             dataPoints: $scope.examgraphd
        //           //         }
        //           //    ]

        //           //});
        //           //chart.render();
        //           //function explodePie(e) {
        //           //    if (typeof (e.dataSeries.dataPoints[e.dataPointIndex].exploded) === "undefined" || !e.dataSeries.dataPoints[e.dataPointIndex].exploded) {
        //           //        e.dataSeries.dataPoints[e.dataPointIndex].exploded = true;
        //           //    } else {
        //           //        e.dataSeries.dataPoints[e.dataPointIndex].exploded = false;
        //           //    }
        //           //    e.chart.render();
        //           //}

        //           //var chart = new CanvasJS.Chart("linechart", {
        //           //    animationEnabled: true,
        //           //    animationDuration: 3000,

        //           //    axisX: {
        //           //        labelFontSize: 12,
        //           //    },
        //           //    axisY: {
        //           //        labelFontSize: 12,
        //           //    },

        //           //    toolTip: {
        //           //        shared: true
        //           //    },
        //           //    data: [{
        //           //        name: "TOTAL MARKS",
        //           //        showInLegend: true,
        //           //        type: "area",
        //           //        color: "rgba(40,175,101,0.6)",
        //           //        dataPoints: $scope.examgraph
        //           //    },
        //           //    {
        //           //        name: "MARKS OBTAIN",
        //           //        showInLegend: true,
        //           //        type: "area",
        //           //        color: "rgba(0,75,141,0.7)",
        //           //        dataPoints: $scope.examgraph1
        //           //    },
        //           //    {
        //           //        name: "PERCENTAGE (%)",
        //           //        showInLegend: true,
        //           //        type: "area",
        //           //        color: "rgba(0, 212, 255, 0.43)",
        //           //        dataPoints: $scope.examgraph2
        //           //    }
        //           //    ]
        //           //});
        //           //chart.render();
        //       })
        //}
    };
})();