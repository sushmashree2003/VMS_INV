
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgStudentDashboardController', ClgStudentDashboardController)

    ClgStudentDashboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', '$window']
    function ClgStudentDashboardController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, uiCalendarConfig, superCache, $window) {

        CanvasJS.addColorSet("graphcolor",
            [//colorSet Array
                "#2471A3",
                "#76D7C4",
                "#DAF7A6",
                "#FFC300",
                "#FF5733",
            ]);
        //======================================================
        $('#myCarousel').carousel({
            interval: 10000
        })
        $('#myCarouselGallery').carousel({
            interval: 5000
        })
        $('.carousel .item').each(function () {
            var next = $(this).next();
            if (!next.length) {
                next = $(this).siblings(':first');
            }
            next = next.next();
            if (!next.length) {
                next = $(this).siblings(':first');
            }
        });

        $scope.tempcldrlst = [];
        $scope.loaddata = function () {
            apiService.getDATA("ClgStudentDashboard/Getdetails").then(function (promise) {

                //===================================== STUDENT Details
                $scope.studentdetails = promise.studentdetails;
                if ($scope.studentdetails.length > 0) {
                    $scope.studentname = $scope.studentdetails[0].studentname;
                    $scope.course = $scope.studentdetails[0].AMCO_CourseName;
                    $scope.branch = $scope.studentdetails[0].AMB_BranchName;
                    $scope.semester = $scope.studentdetails[0].AMSE_SEMName;
                    $scope.year = $scope.studentdetails[0].ASMAY_Year;
                    $scope.admno = $scope.studentdetails[0].AMCST_AdmNo;
                    $scope.dob = $scope.studentdetails[0].AMCST_DOB;
                    $scope.email = $scope.studentdetails[0].AMCST_emailId;
                    $scope.mobileno = $scope.studentdetails[0].AMCST_MobileNo;
                    $scope.regno = $scope.studentdetails[0].AMCST_RegistrationNo;
                    $scope.photo = $scope.studentdetails[0].AMCST_StudentPhoto;
                    $scope.address = $scope.studentdetails[0].AMCST_PerStreet;
                }
                //===================================== FEE Details
                $scope.feedetails = promise.feedetails;
                $scope.feetotal = [];
                $scope.feepaid = [];
                $scope.total_Paid = 0.00;
                $scope.total_amount = 0.00;
                $scope.balance = 0.00;
                angular.forEach($scope.feedetails, function (fee) {
                    $scope.total_Paid += fee.paid;
                    $scope.total_amount += fee.Total;
                    $scope.balance += fee.balance;
                    $scope.feetotal.push({ label: "Fee" , "y": fee.Total })
                    $scope.feepaid.push({ label: "Fee", "y": fee.paid })
                })
                var chart = new CanvasJS.Chart("FeeDetailsChart", {
                    animationEnabled: true,
                    animationDuration: 3000,
                    height: 350,
                    colorSet: "graphcolor",
                    axisX: {
                        labelFontSize: 13,
                    },
                    axisY: {
                        labelFontSize: 13,
                    },

                    toolTip: {
                        shared: true
                    },
                    data: [{
                        name: "TOTAL AMOUNT",
                        showInLegend: true,
                        type: "column",
                        dataPoints: $scope.feetotal
                    },
                    {
                        name: "PAID AMOUNT",
                        showInLegend: true,
                        type: "column",
                        dataPoints: $scope.feepaid
                    }]
                });
                chart.render();


                //===================================== Notice Board Details
                $scope.noticeboard = promise.noticeboard;
                $scope.mainnotice = [];
                if ($scope.noticeboard.length > 0) {
                    angular.forEach($scope.noticeboard, function (noti) {
                        $scope.curdate = new Date();
                        $scope.noticenddate = new Date(noti.INTB_EndDate);
                        $scope.displaydate = new Date(noti.INTB_DisplayDate);
                        var curntdate = $scope.curdate == null ? "" : $filter('date')($scope.curdate, "yyyy-MM-dd");
                        var enddate = $scope.noticenddate == null ? "" : $filter('date')($scope.noticenddate, "yyyy-MM-dd");
                        var dsplydate = $scope.displaydate == null ? "" : $filter('date')($scope.displaydate, "yyyy-MM-dd");

                        if (curntdate >= dsplydate && curntdate <= enddate) {
                            $scope.mainnotice.push({ INTB_Title: noti.INTB_Title, NTB_TTSylabusFlg: noti.NTB_TTSylabusFlg, INTB_Attachment: noti.INTB_Attachment, INTB_StartDate: noti.INTB_StartDate, INTB_EndDate: noti.INTB_EndDate, INTB_FilePath: noti.INTB_FilePath });
                        }
                        else if (curntdate == enddate) {
                            $scope.mainnotice.push({ INTB_Title: noti.INTB_Title, NTB_TTSylabusFlg: noti.NTB_TTSylabusFlg, INTB_Attachment: noti.INTB_Attachment, INTB_StartDate: noti.INTB_StartDate, INTB_EndDate: noti.INTB_EndDate, INTB_FilePath: noti.INTB_FilePath });
                        }
                    })
                }

                //===================================== COE EVENTS
                $scope.coereportlist = promise.coereportlist;
                $scope.maincalender = [];
                if ($scope.coereportlist.length > 0) {
                    angular.forEach($scope.coereportlist, function (coe) {
                        $scope.curdate = new Date();
                        $scope.endDate = new Date(coe.coeE_EEndDate);
                        $scope.reminderdate = new Date(coe.coeE_ReminderDate);
                        var curntdate = $scope.curdate == null ? "" : $filter('date')($scope.curdate, "yyyy-MM-dd");
                        var eventenddate = $scope.endDate == null ? "" : $filter('date')($scope.endDate, "yyyy-MM-dd");
                        var remiderdate = $scope.reminderdate == null ? "" : $filter('date')($scope.reminderdate, "yyyy-MM-dd");

                        if (curntdate >= remiderdate && curntdate <= eventenddate) {
                            $scope.maincalender.push({
                                coemE_EventName: coe.coemE_EventName, coemE_EventDesc: coe.coemE_EventDesc, coeE_EStartDate: coe.coeE_EStartDate,
                                coeE_EEndDate: coe.coeE_EEndDate
                            });
                        }
                        else if (curntdate == eventenddate) {
                            $scope.maincalender.push({
                                coemE_EventName: coe.coemE_EventName, coemE_EventDesc: coe.coemE_EventDesc, coeE_EStartDate: coe.coeE_EStartDate,
                                coeE_EEndDate: coe.coeE_EEndDate
                            });
                        }
                    })
                }

                //===================================== Attendance Details
                $scope.attendancedetails = promise.attendancedetails;
                $scope.total_ch = 0.00;
                $scope.total_p = 0.00;
                $scope.atten = 0.00;
                $scope.attendanceCH = [];
                $scope.attendanceyTP = [];
                if ($scope.attendancedetails != null) {
                    angular.forEach($scope.attendancedetails, function (att) {
                        $scope.total_ch += parseFloat(att.CLASS_HELD);
                        $scope.total_p += parseFloat(att.TOTAL_PRESENT);
                        $scope.attendanceCH.push({ label: att.MONTH_NAME, "y": parseInt(att.CLASS_HELD) })
                        $scope.attendanceyTP.push({ label: att.MONTH_NAME, "y": parseInt(att.TOTAL_PRESENT) })
                    })
                    $scope.atten = ($scope.total_p / $scope.total_ch) * 100;

                    var chart = new CanvasJS.Chart("AttendanceDetailsChart", {
                        animationEnabled: true,
                        animationDuration: 3000,
                        height: 350,
                        colorSet: "graphcolor",
                        axisX: {
                            labelFontSize: 13,
                        },
                        axisY: {
                            labelFontSize: 13,
                        },

                        toolTip: {
                            shared: true
                        },
                        data: [{
                            name: "CLASS HELD",
                            showInLegend: true,
                            type: "column",
                            dataPoints: $scope.attendanceCH
                        },
                        {
                            name: "TOTAL PRESENT",
                            showInLegend: true,
                            type: "column",
                            dataPoints: $scope.attendanceyTP
                        }]
                    });
                    chart.render();
                }

                // #region PortalCalender
                //=====================================  Calender
                $scope.calenderlist = promise.calenderlist;
                angular.forEach($scope.calenderlist, function (qwe) {
                    qwe.title = qwe.coemE_EventName;
                    var xyz = $filter('date')(qwe.coeE_EStartDate, "yyyy/MM/dd");
                    var abc = $filter('date')(qwe.coeE_EEndDate, "yyyy/MM/dd");
                    qwe.start = new Date(xyz);
                    $scope.tempcldrlst.push({ title: qwe.title, start: qwe.start });
                })

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

                        header: {
                            left: 'title',
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

            });
        }

        var HostName = location.host;        
        $scope.onfee = function () {
            $window.location.href = 'http://' + HostName + '/#/app/ClgFeeDetails/';
        };        
        $scope.onAttendance = function () {
            $window.location.href = 'http://' + HostName + '/#/app/ClgAttendanceDetails/';
        };
        $scope.oncoe = function () {
            $window.location.href = 'http://' + HostName + '/#/app/ClgCOE/';
        };


    };
})();

