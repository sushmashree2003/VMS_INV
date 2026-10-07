

(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISMDashboardController', ISMDashboardController);

    ISMDashboardController.$inject = ['$rootScope', '$scope', '$http', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', '$window'];
    function ISMDashboardController($rootScope, $scope, $state, $http, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

        //======================================================          uiCalendarConfig
        var miid = "";
        var configsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));

        //window.onload = function () {

    

        //}


        var chart2 = new CanvasJS.Chart("chartContainer3", {
            animationEnabled: true,
            title: {
                //text: "Desktop Search Engine Market Share - 2016"
            },
            data: [{
                type: "pie",
                startAngle: 240,
                yValueFormatString: "##0.00\"%\"",
                indexLabel: "{label} {y}",
                dataPoints: [
                    { y: 79.45, color: "green", label: "Low Stock Item" },
                    { y: 7.31, color: "yellow", label: "All Item Group" },
                    { y: 7.06, color: "red", label: "All Items" }

                ]
            }]
        });

        chart2.render(); 

        var HostName = location.host;
      
        //$scope.loadopentask = function () {
        //    $window.location.href = 'http://' + HostName + '/#/app/ISM_TaskRegisterReport/';
        //};
       
        //$scope.loadinprogresstask = function () {
        //    $window.location.href = 'http://' + HostName + '/#/app/ISM_TaskRegisterReport/';
        //};

        //$scope.loadtotalcompletedtask = function () {
        //    $window.location.href = 'http://' + HostName + '/#/app/ISM_DeviationRemarkswiseReport/';
        //};

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.search = "";
            var data = {
                "pageid": 2
            };
            apiService.create("ISMDashboard/loaddataMDDASHBOARD", data).then(function (promise) {
               
                //$scope.getabsentlist = promise.getabsentlist;
                //$scope.enentsname = promise.eventlist;
                //$scope.birthadayname = promise.birthdaylist;
                //console.log($scope.getabsentlist );
                //$scope.getdeviationdepartmentwise = promise.getdeviationdepartmentwise;
              
                $scope.dataabsentgraph = [];
                if (promise.getdeviationdepartmentwise !== null) {

                    for (var i = 0; i < promise.getdeviationdepartmentwise.length; i++) {
                        $scope.dataabsentgraph.push({ label: promise.getdeviationdepartmentwise[i].HRMDC_Name, "y": promise.getdeviationdepartmentwise[i].completePercent });
                    }
                }


                var chart = new CanvasJS.Chart("chartContainer",
                    {



                        axisX: {
                            labelFontSize: 8,
                            interval: 1,
                            labelAngle: -20,
                            labelFontColor: "black",
                            labelFontWeight: "bold"

                            //title: "Designation",
                        },
                        axisY: {
                            labelFontColor: "black",
                            labelFontSize: 12

                            // title: "No.of. Staffs",

                        },

                        data: [
                            {
                                type: "column",
                                showInLegend: true,
                                dataPoints: $scope.dataabsentgraph
                            }
                        ]
                    });

                chart.render();
                var chart1 = new CanvasJS.Chart("chartContainer5", {
                    animationEnabled: true,
                    title: {
                        //text: "Email Categories",
                        horizontalAlign: "left"
                    },
                    data: [{
                        type: "doughnut",
                        //startAngle: 20,
                        innerRadius: 60,
                        //showInLegend: true,
                        indexLabelFontSize: 12,
                        indexLabel: "{label} - #percent%",
                        toolTipContent: "<b>{label}:</b> {y} (#percent%)",
                        dataPoints: [
                            { y: promise.totalopentask.length, label: "opentask" },
                            { y: promise.totalinprogresstask.length, label: "inprogresstask" },
                            { y: promise.totalpendingtask.length, label: "totalpendingtask" }


                        ]
                    }]
                });
                chart1.render();

           
              
                if (promise.projectcount.length > 0 || promise.projectcount !== null) {
                    $scope.projectcount = promise.projectcount.length;
                    $scope.tasktransfer = promise.projectcount;
                } else {
                    $scope.projectcount = 0;
                }

                if (promise.totalopentask.length > 0 || promise.totalopentask !== null) {
                 
                    $scope.opentask = promise.totalopentask.length;
                } else {
                    $scope.opentask = 0;
                }

                if (promise.totalinprogresstask.length > 0 || promise.totalinprogresstask !== null) {
                    $scope.inprogresstask = promise.totalinprogresstask.length;
                } else {
                    $scope.inprogresstask = 0;
                }

                if (promise.totalpendingtask.length > 0 || promise.totalpendingtask !== null) {
                    $scope.totalpendingtask = promise.totalpendingtask.length;
                } else {
                    $scope.totalpendingtask = 0;
                }

                if (promise.birthdaylist.length > 0 || promise.birthdaylist !== null) {
                    $scope.birthdaylist = promise.birthdaylist.length;
                } else {
                    $scope.birthdaylist = 0;
                  
                }

                if (promise.getabsentlist !== null) {
                    if (promise.getabsentlist.length > 0) {
                        $scope.totalabsenties = 0;
                        $scope.getabsentlist = promise.getabsentlist;
                        $scope.absenties = promise.getabsentlist.length;
                        angular.forEach($scope.getabsentlist, function (aa) {
                            if (aa.absentdays === 0) {
                                $scope.totalabsenties = $scope.totalabsenties + 1;
                            }
                        });

                    } else {
                        $scope.absenties = 0;
                    }
                }

                if (promise.getdeviationdepartmentwise.length > 0) {
                    $scope.getdeviationdepartmentwise = promise.getdeviationdepartmentwise;
                  
                } else {
                    $scope.getdeviationdepartmentwise = 0;
                }

                if (promise.eventlist.length > 0) {
                    $scope.eventlist = promise.eventlist.length;
                } else {
                    $scope.eventlist = 0;
                }

                if (promise.plannernotgeneratedreport.length > 0) {
                    $scope.plannernotgeneratedreport = promise.plannernotgeneratedreport.length;
                } else {
                    $scope.plannernotgeneratedreport = 0;
                }

                if (promise.totapplied.length > 0) {
                    $scope.totapplied = promise.totapplied.length;
                } else {
                    $scope.totapplied = 0;
                }

                if (promise.totapproved.length > 0) {
                    $scope.totapproved = promise.totapproved.length;
                }
                else {
                    $scope.totapproved = 0;
                }

                if (promise.totrejected.length > 0) {
                    $scope.totrejected = promise.totrejected.length;
                } else {
                    $scope.totrejected = 0;
                }

                if (promise.totpending.length > 0) {
                    $scope.totpending = promise.totpending.length;
                } else {
                    $scope.totpending = 0;
                }
                
                if (promise.totalemployees.length > 0) {
                    $scope.totalemployees = promise.totalemployees.length;
                } else {
                    $scope.totpending = 0;
                }
                if (promise.blocklist.length > 0) {
                    $scope.blocklist = promise.blocklist.length;
                } else {
                    $scope.blocklist = 0;
                }

            });
        };
    }
})();




