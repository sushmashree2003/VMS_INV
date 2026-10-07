
(function () {
    'use strict';
    angular
        .module('app')
        .controller('DashController', DashController);
    DashController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'Excel', '$compile', '$timeout', 'superCache']
    function DashController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, Excel, $compile, $timeout, superCache) {


        // data related to CanvasJS Charts
        //$scope.myFunc = function () {

            var countries = {
                //"India": "column",
                //"Australia": "line",
                //"Saudi": "doughnut",
                //"Canada": "area",
                //"Iran": "spline",
                //"Russia": "bar",
                //"UAE": "stackedArea",
                //"China": "stackedColumn",
                "US": "stackedColumn"
            };

            $scope.items = countries;

            angular.forEach(countries, function (value, index) {
                console.log(index);
                var dps = [{
                    x: 1,
                    y: 1
                }]; //dataPoints. 
                var chart = "chartContainer" + index;
                setInterval(() => {
                    chart = new CanvasJS.Chart("chartContainer" + index, {
                        title: {
                            text: "Live Data"
                        },
                        axisX: {
                            title: "Axis X Title"
                        },
                        axisY: {
                            title: "Units"
                        },
                        data: [{
                            type: value,
                            dataPoints: [
                                { label: "New Jersey", y: 19034.5 },
                                { label: "Texas", y: 20015 },
                                { label: "Oregon", y: 25342 },
                                { label: "Montana", y: 20088 },
                                { label: "Massachusetts", y: 28234 }
                            ]
                        }]
                    });
                    chart.render();
                },1000);
              

                //var xVal = dps.length - 1;
                //var yVal = 15;
                //var updateInterval = 1000;

                //var updateChart = function () {
                //    yVal = yVal + Math.round(5 + Math.random() * (-5 - 5));
                //    dps.push({
                //        x: xVal,
                //        y: yVal
                //    });
                //    xVal--;
                //    chart.render();

                //};
                //setInterval(function () {
                //    updateChart()
                //}, updateInterval);

            });
       // }



    }

})();