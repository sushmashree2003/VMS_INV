(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmployeeDashboardController', EmployeeDashboardController)

    EmployeeDashboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', '$window']

    function EmployeeDashboardController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, uiCalendarConfig, superCache, $window) {

        $scope.tempcldrlst = [];


        //$('.carousel .item').each(function () {
        //    var next = $(this).next();
        //    if (!next.length) {
        //        next = $(this).siblings(':first');
        //    }
        //    next = next.next();
        //    if (!next.length) {
        //        next = $(this).siblings(':first');

        //    }
        //});
        $scope.LoadData = function () {

            var sal_list = [];
            var TT_list = [];
            apiService.getDATA("EmployeeDashboard/getalldetails")
                .then(function (promise) {

                    //$scope.Month_list = promise.monthName;
                    if (promise.filldepartment.length > 0) {
                        $scope.empDetails = promise.filldepartment;
                        $scope.HRME_EmployeeFirstName = $scope.empDetails[0].hrmE_EmployeeFirstName;
                        $scope.HRME_DOJ = $scope.empDetails[0].hrmE_DOJ
                        $scope.HRMD_DepartmentName = $scope.empDetails[0].hrmD_DepartmentName;
                        $scope.HRMDES_DesignationName = $scope.empDetails[0].hrmdeS_DesignationName;
                        $scope.HRME_EmployeeCode = $scope.empDetails[0].hrmE_EmployeeCode;
                        $scope.HRME_DOB = $scope.empDetails[0].hrmE_DOB;
                        $scope.photo = $scope.empDetails[0].hrmE_PhotoNo;
                        $scope.coedata = promise.coedata;
                        $scope.studentcount = promise.studentcount;
                    }
                    if (promise.mobile.length > 0) {
                        $scope.HRME_MobileNo = promise.mobile[0].hrmE_MobileNo;
                    }
                    if (promise.email.length > 0) {
                        $scope.HRME_EmailId = promise.email[0].hrmE_EmailId;
                    }

                    if ($scope.coedata != "" && $scope.coedata != null) {
                        if ($scope.coedata.length > 0) {
                            $scope.hidecoe = true;
                        }
                    }
                    else {
                        $scope.hidecoe = false;
                    }
                    //for salary count
                    var totalsalary = 0;
                    if (promise.salarylist.length > 0) {
                        $scope.salary_list = promise.salarylist;
                        for (var i = 0; i < $scope.salary_list.length; i++) {
                            totalsalary += $scope.salary_list[i].salary;
                        }

                    }

                    $scope.SalaryC = totalsalary;

                    //for timetable count
                    var totalPeriod = 0;

                    if (promise.tT_final_generation.length > 0) {
                        $scope.tt_final = promise.tT_final_generation;
                        for (var i = 0; i < $scope.tt_final.length; i++) {
                            totalPeriod += $scope.tt_final[i].periodCount;
                        }

                    }

                    $scope.PeriodC = totalPeriod;
                    $scope.AllPeriod = promise.allperiods;                 
                    //----------------------------------Calender
                    $scope.calenderlist = promise.calenderlist;
                    angular.forEach($scope.calenderlist, function (qwe) {
                        qwe.title = qwe.coemE_EventName;
                        var xyz = $filter('date')(qwe.coeE_EStartDate, "yyyy/MM/dd");
                        var abc = $filter('date')(qwe.coeE_EEndDate, "yyyy/MM/dd");
                        qwe.start = new Date(xyz);
                        $scope.tempcldrlst.push({ title: qwe.title, start: qwe.start });
                    })

                    if ($scope.salary_list != null) {

                        for (var i = 0; i < $scope.salary_list.length; i++) {
                            sal_list.push({ label: $scope.salary_list[i].monthName, "y": $scope.salary_list[i].salary })
                        }
                    }
                    if ($scope.tt_final != null && $scope.AllPeriod != null) {
                        for (var i = 0; i < $scope.AllPeriod.length; i++) {
                            for (var j = 0; j < $scope.tt_final.length; j++) {
                                if ($scope.tt_final[j].dayName == $scope.AllPeriod[i].ttmD_DayCode) {
                                    TT_list.push({ label: $scope.AllPeriod[i].ttmD_DayName, "y": $scope.tt_final[j].periodCount })
                                }

                            }
                        }
                    }
                    var chart = new CanvasJS.Chart("columnchart", {

                        axisX: {
                            labelFontSize: 12,
                        },
                        axisY: {
                            labelFontSize: 12,
                        },

                        data: [
                            {
                                type: "column",
                                showInLegend: true,
                                dataPoints: sal_list
                            }
                        ]
                    });
                    chart.render();
                    //rangeBarChat
                    var chart = new CanvasJS.Chart("areachart", {

                        axisX: {
                            labelFontSize: 12,
                        },
                        axisY: {
                            labelFontSize: 12,
                        },

                        data: [
                            {
                                type: "column",
                                showInLegend: true,
                                dataPoints: TT_list,

                            }
                        ]
                    });
                    chart.render();

                    $scope.period_list = promise.periods;
                    $scope.Class_Section_List = promise.class_sectons;


                })
        };

        //-------------------------------------------------------PORTAL-CALENDER

        // #region PortalCalender

        var date = new Date();
        var d = date.getDate();
        var m = date.getMonth();
        var y = date.getFullYear();

        $scope.changeTo = 'Hungarian';
        $scope.currentView = 'month';

        /* event source that contains custom events on the scope */
        $scope.events = $scope.tempcldrlst;
        /* event source that calls a function on every view switch */
        $scope.eventsF = function (start, end, timezone, callback) {

            var s = new Date(start).getTime() / 1000;
            //  var e = new Date(end).getTime() / 1000;
            var m = new Date(start).getMonth();
            var events = [{
                title: 'Feed Me ' + m,
                start: s + (50000),
                // end: s + (100000),
                allDay: false,
                className: ['customFeed']
            }];
            callback(events);
        };
        $scope.calEventsExt = {
            color: '#f00',
            textColor: 'yellow',
            events: []
        };
        $scope.ev = {};
        /* alert on dayClick */
        $scope.alertOnDayClick = function (date) {
            $scope.alertMessage = (date.toString() + ' was clicked ');
            $scope.ev = {
                from: date.format('YYYY-MM-DD'),
                to: date.format('YYYY-MM-DD'),
                title: '',
                allDay: true
            };
        };
        /* alert on eventClick */
        $scope.alertOnEventClick = function (date, jsEvent, view) {
            $scope.alertMessage = (date.title + ' was clicked ');
        };
        /* alert on Drop */
        $scope.alertOnDrop = function (event, delta, revertFunc, jsEvent, ui, view) {
            $scope.alertMessage = ('Event Dropped to make dayDelta ' + delta);
        };
        /* alert on Resize */
        $scope.alertOnResize = function (event, delta, revertFunc, jsEvent, ui, view) {
            $scope.alertMessage = ('Event Resized to make dayDelta ' + delta);
        };
        /* add and removes an event source of choice */
        $scope.addRemoveEventSource = function (sources, source) {
            var canAdd = 0;
            angular.forEach(sources, function (value, key) {
                if (sources[key] === source) {
                    sources.splice(key, 1);
                    canAdd = 1;
                }
            });
            if (canAdd === 0) {
                sources.push(source);
            }
        };
        /* add custom event*/
        $scope.addEvent = function () {
            $scope.events.push({
                title: $scope.ev.title,
                start: moment($scope.ev.from),
                //   end: moment($scope.ev.to),
                allDay: true,
                className: ['openSesame']
            });
        };
        /* remove event */
        /*$scope.remove = function (index) {
            $scope.events.splice(index, 1);
        };*/
        /* Change View */
        $scope.changeView = function (view, calendar) {
            uiCalendarConfig.calendars.myCalendar1.fullCalendar('removeEvents');
            uiCalendarConfig.calendars.myCalendar1.fullCalendar('addEventSource',
                $scope.tempcldrlst);
        };
        /* Change View */
        $scope.renderCalender = function (calendar) {
            $timeout(function () {
                if (uiCalendarConfig.calendars[calendar]) {
                    uiCalendarConfig.calendars[calendar].fullCalendar('render');
                }
            });
        };
        /* Render Tooltip */
        $scope.eventRender = function (event, element, view) { };
        /* config object */
        $scope.uiConfig = {
            calendar: {
                height: 325,

                editable: false,
                viewRender: $scope.changeView,
                //customButtons: {
                //    myCustomButton: {
                //        text: 'custom!',
                //        click: function () {
                //            alert('clicked the custom button!');
                //        }
                //    }
                //},
                header: {
                    left: 'title',
                    // center: 'myCustomButton',
                    right: 'today prev,next'
                },
                dayClick: $scope.alertOnDayClick,
                eventClick: $scope.alertOnEventClick,
                eventDrop: $scope.alertOnDrop,
                eventResize: $scope.alertOnResize,
                eventRender: $scope.eventRender,
                businessHours: {
                    start: '12:00', // a start time (10am in this example)
                    //     end: '18:00', // an end time (6pm in this example)

                    dow: [1, 2, 3, 4]
                    // days of week. an array of zero-based day of week integers (0=Sunday)
                    // (Monday-Thursday in this example)
                }
            }
        };
        /* event sources array*/
        $scope.eventSources = [$scope.events, $scope.eventsF];
        $scope.eventSources2 = [$scope.calEventsExt, $scope.eventsF, $scope.events];


        $scope.eventRender = function (event, element, view) {
            element.attr({
                'tooltip': event.events,
                'tooltip-append-to-body': true
            });
            $compile(element)($scope);
        };
        // #endregion

        $scope.saveakpkfile = function () {
            var data = {
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("EmployeeDashboard/saveakpkfile", data).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal('File Download!');
                    }                  
                });
        }


        var HostName = location.host;
        $scope.showStudent = function () {

            $window.location.href = 'https://' + HostName + '/#/app/employeeStudentSearch';

        };
        $scope.showTT = function () {

            $window.location.href = 'https://' + HostName + '/#/app/employeeTimetable';

        };
        $scope.showSalary = function () {

            $window.location.href = 'https://' + HostName + '/#/app/employeeSalaryDetails';

        };
    };
})();