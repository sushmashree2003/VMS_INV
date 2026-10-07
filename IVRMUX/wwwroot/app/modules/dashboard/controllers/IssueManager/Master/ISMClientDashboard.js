
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISMDashboardController', ISMDashboardController);


    ISMDashboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', '$window'];
    function ISMDashboardController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, uiCalendarConfig, superCache, $window) {
        var miid = "";
        var configsettings = JSON.parse(localStorage.getItem("Startconfig"));

        CanvasJS.addColorSet("graphcolor",
            [
                "#e2dc14",
                "#ea814f"
            ]);

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.get_clientcomptasklist = [];
            $scope.totaltask = 0;
            $scope.pendingcount = 0;
            $scope.search = "";
            var data = {
                "typeFlg": "Default"
            };
            apiService.create("ISM_Client_ComplaintsTaskList/getdetails", data).then(function (promise) {

                $scope.get_clientcomptasklist = promise.get_clientcomptasklist;
                $scope.totaltask = promise.taskCount;

                if ($scope.get_clientcomptasklist.length > 0) {
                    var pendingtaskcount = 0;
                    angular.forEach($scope.get_clientcomptasklist, function (clt) {
                        if (clt.ISMTCR_Status === 'Open') {
                            pendingtaskcount += 1;
                        }
                    });
                    $scope.pendingcount = pendingtaskcount;
                }
                else {
                    $scope.pendingcount = 0;
                    $scope.get_clientcomptasklist = [];
                }


                $scope.overallDetails = [];
                $scope.overallDetails.push({ label: "Total Task", y: parseInt($scope.totaltask) });
                $scope.overallDetails.push({ label: "Pending Task", y: parseInt($scope.pendingcount) });

                var chart = new CanvasJS.Chart("chartColumn", {
                    animationEnabled: true,
                    animationDuration: 3000,
                    height: 300,
                    //  width: 650,
                    colorSet: "graphcolor",
                    title: {
                        text: "Issue Manager"
                    },
                    axisX: {
                        labelFontSize: 12
                    },
                    axisY: {
                        title: "Task Count",
                        labelFontSize: 12
                    },
                    toolTip: {
                        shared: true
                    },
                    data: [
                        {
                            type: "column",
                            legendText: "Item's",
                            showInLegend: false,
                            dataPoints: $scope.overallDetails
                        }
                    ]
                });
                chart.render();
            });
        };

        var HostName = location.host;

        $scope.taskreg = function () {
            $window.location.href = 'http://' + HostName + '/#/app/ISM_Client_ComplaintsTaskList/';
        }

        var date = new Date();
        var d = date.getDate();
        var m = date.getMonth();
        var y = date.getFullYear();

        $scope.Events = [{
            title: 'All Day Event',
            start: new Date(y, m, 1)
        }, {
            title: 'Long Event',
            start: new Date(y, m, d - 5),
            end: new Date(y, m, d - 2)
        }, {
            id: 999,
            title: 'Repeating Event',
            start: new Date(y, m, d - 3, 16, 0),
            allDay: false
        }, {
            id: 999,
            title: 'Repeating Event',
            start: new Date(y, m, d + 4, 16, 0),
            allDay: false
        }, {
            title: 'Birthday Party',
            start: new Date(y, m, d + 1, 19, 0),
            end: new Date(y, m, d + 1, 22, 30),
            allDay: false
        }];


        $scope.calendarOptions = {
            calendar: {
                height: 600,
                header: {
                    left: 'title',
                    //center: 'wobble,month',
                    right: 'today prev,next'

                },
                dayClick: function (event) { console.log('Day clicking'); $scope.openModal(event); }
            }
        };
        $scope.modalShown = false;
        $scope.openModal = function (eventObj) {
            console.log('Opening modal...');
            $scope.modalShown = !$scope.modalShown;

        };
        app.directive('modalDialog', function () {
            return {
                restrict: 'E',
                scope: {
                    show: '='
                },
                template: `
    <div class="ng-modal" ng-show="show">
        <div class="ng-modal-overlay" ng-click="hideModal()"></div>
      <div class="ng-modal-dialog" ng-style="dialogStyle">
        <div class="ng-modal-close" ng-click="hideModal()">X</div>
        <div class="ng-modal-dialog-content" ng-transclude>
        </div>    
      </div>
      </div>
    `,
                transclude: true,
                link: function (scope, elem, attrs) {
                    scope.dialogStyle = {};
                    if (attrs.width) {
                        scope.dialogStyle.width = attrs.width;
                    }
                    if (attrs.height) {
                        scope.dialogStyle.height = attrs.height;
                    }
                    scope.hideModal = function () {
                        scope.show = false;
                    };
                }
            };
        });

        //var NewModalInstanceCtrl = function ($scope, $modalInstance, event) {

        //    var date = new Date(event); var d = date.getDate(); var m = date.getMonth(); var y = date.getFullYear();
        //    $scope.ok = function () {
        //        $modalInstance.close({ type: 'party', id: 999, title: 'Added from modal', start: new Date(y, m, d, 16, 0), allDay: false });
        //    };

        //    $scope.cancel = function () {
        //        $modalInstance.dismiss('cancel');
        //    };


        //};



    }
})();