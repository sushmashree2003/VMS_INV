
(function () {
    'use strict';
    angular
        .module('app')
        .controller('StudentDashboardController', StudentDashboardController)

    StudentDashboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', '$window']
    function StudentDashboardController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, uiCalendarConfig, superCache, $window) {


        // data related to CanvasJS Charts

        var countries = {

            "China": "spline",
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
            }, 1000);

        });
        $scope.images = [
            { path: 'accounts', name: 'accounts' },
            { path: 'admission', name: 'admission' },
            { path: 'alumini', name: 'alumini' },
            { path: 'birthday', name: 'birthday' },
            { path: 'coe', name: 'coe' },
            { path: 'conveyance', name: 'conveyance' },
            { path: 'e-banking', name: 'e-banking' },
            { path: 'e-mail', name: 'e-mail' },
            { path: 'exam', name: 'exam' },
            { path: 'exit', name: 'exit' }
        ];
        //Tools I use Carousel
        $("#owl-demo,#owl-demo1").owlCarousel({
            items: 8, //10 items above 1000px browser width
            itemsDesktop: [1000, 5], //5 items between 1000px and 901px
            itemsDesktopSmall: [900, 3], // betweem 900px and 601px
            itemsTablet: [600, 2], //2 items between 600 and 0
            itemsMobile: false, // itemsMobile disabled - inherit from itemsTablet option
        });
        $("#owl-demo,#owl-demo1").trigger('owl.play', 2000);

        // Custom Navigation Events
        $(".next").click(function () {
            $("#owl-demo,#owl-demo1").trigger('owl.next');
        });
        $(".prev").click(function () {
            $("#owl-demo,#owl-demo1").trigger('owl.prev');
        });
        $(".play").click(function () {
            $("#owl-demo,#owl-demo1").trigger('owl.play', 1000); //owl.play event accept autoPlay speed as second parameter
        });
        $(".stop").click(function () {
            $("#owl-demo,#owl-demo1").trigger('owl.stop');
        });

        //cartoon photo slider carosusel
        $("#owl-single").owlCarousel({
            navigation: true, // Show next and prev buttons
            slideSpeed: 300,
            paginationSpeed: 400,
            singleItem: true,
            autoPlay: 5000, //Set AutoPlay to 3 seconds
        });

        console.log($scope.images);

        var miid = "";
        var configsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        miid = configsettings[0].mI_Id;

        var ivrmstoragelist = JSON.parse(localStorage.getItem("ivrmstoragelist"));
        var imgpath = "";
        if (ivrmstoragelist.length > 0) {
            imgpath = ivrmstoragelist[0].ivrM_SD_Access_Name;
        }

        //Image Slidercustom
        CanvasJS.addColorSet("graphcolor",
            [//colorSet Array
                "#3498DB",
                "#76D7C4",
                "#808B96",
                "#80DEEA",
                "#C5E1A5",
                "#AAB7B8"
            ]);
        //======================================================

        //$('#myCarousel').carousel({
        //    interval: 4000
        //})
        //$('.carousel .item').each(function () {

        //    var next = $(this).next();
        //    if (!next.length) {
        //        next = $(this).siblings(':first');
        //    }
        //    next.children(':first-child').clone().appendTo($(this));

        //    for (var i = 0; i < 1; i++) {
        //        next = next.next();
        //        if (!next.length) {
        //            next = $(this).siblings(':first');
        //        }
        //        next.children(':first-child').clone().appendTo($(this));
        //    }
        //});
        $('#myCarousel').carousel({
            interval: 10000
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
        //==================================================
        $scope.myInterval = 2000;
        $scope.noWrapSlides = false;
        $scope.active = 0;
        var slides = $scope.slides = [];

        var currIndex = 0;
        var imagetype = '.jpg';
        $scope.addSlide = function () {

            var newWidth = 600 + slides.length + 1 + imagetype;
            slides.push({
                image: 'https://bdcampusstrg.blob.core.windows.net/files/' + miid + '/dashboardimages/' + newWidth,
                text: ['Student Portal'][slides.length % 3],
                id: currIndex++
            });
        };
        $scope.randomize = function () {
            var indexes = generateIndexesArray();
            assignNewIndexesToSlides(indexes);
        };

        for (var i = 0; i < 4; i++) {
            $scope.addSlide();
        }

        // Randomize logic below

        function assignNewIndexesToSlides(indexes) {
            for (var i = 0, l = slides.length; i < l; i++) {
                slides[i].id = indexes.pop();
            }
        }

        function generateIndexesArray() {
            var indexes = [];
            for (var i = 0; i < currIndex; ++i) {
                indexes[i] = i;
            }
            return shuffle(indexes);
        }
        //Image Slider END
        $scope.tempcldrlst = [];

        $scope.loaddata = function () {

            apiService.getDATA("StudentDashboard/Getdetails").then(function (promise) {

                $scope.yearfeedetails = promise.academicyearFeedata;
                $scope.yearattdetails = promise.academicyearAttendancedata;
                $scope.studetailslst = promise.studetailslist;
                $scope.coereportlst = promise.coereportlist;
                $scope.noticelist = promise.noticelist;
                $scope.assignmentlist = promise.assignmentlist;
                $scope.imgs_list = promise.imgs_list;
                $scope.pushnotiflist = promise.pushnotiflist;
                $scope.mI_Id = promise.mI_Id;
                $scope.homeworklist = promise.homeworklist;
                $scope.pushnotification = promise.pushnotification;


                //var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
                //var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
                $scope.currentdte = new Date();
                $scope.currentdte = $scope.currentdte == null ? "" : $filter('date')($scope.currentdte, "yyyy-MM-dd");

                $scope.pusharray = [];
                angular.forEach(promise.pushnotification, function (pd) {
                    $scope.pushdate = new Date(pd.ipN_Date);
                    $scope.pushdate = $scope.pushdate == null ? "" : $filter('date')($scope.pushdate, "yyyy-MM-dd");

                    if ($scope.pushdate < $scope.currentdte) {
                        $scope.pusharray.push({
                            ipN_Id: pd.ipN_Id, ipN_StuStaffFlg: pd.ipN_StuStaffFlg, ipN_PushNotification: pd.ipN_PushNotification, ipN_Date: pd.ipN_Date,
                            ipnS_Id: pd.ipnS_Id, ipN_No: pd.ipN_No, amsT_Id: pd.amsT_Id, ivrmuL_Id: pd.ivrmuL_Id, amsT_FirstName: pd.amsT_FirstName,
                        });
                    }
                    else if ($scope.pushdate == $scope.currentdte) {
                        $scope.pusharray.push({
                            ipN_Id: pd.ipN_Id, ipN_StuStaffFlg: pd.ipN_StuStaffFlg, ipN_PushNotification: pd.ipN_PushNotification, ipN_Date: pd.ipN_Date,
                            ipnS_Id: pd.ipnS_Id, ipN_No: pd.ipN_No, amsT_Id: pd.amsT_Id, ivrmuL_Id: pd.ivrmuL_Id, amsT_FirstName: pd.amsT_FirstName,
                        });
                    }
                })



                $scope.addSlide();
                $scope.coeArray = [];

                //----display Notice details into grid view
                $scope.noticearray = [];
                $scope.noticedetailsarray = [];
                angular.forEach($scope.studetailslst, function (stu) {
                    angular.forEach($scope.noticelist, function (noti) {
                        if (stu.asmcL_Id == noti.asmcL_Id) {
                            $scope.noticearray.push({ intB_Title: noti.intB_Title, ntB_TTSylabusFlg: noti.ntB_TTSylabusFlg, intB_Attachment: noti.intB_Attachment, intB_StartDate: noti.intB_StartDate, intB_EndDate: noti.intB_EndDate, intB_FilePath: noti.intB_FilePath, intB_DisplayDate: noti.intB_DisplayDate, intB_Description: noti.intB_Description });


                            $scope.curdate = new Date();
                            $scope.noticedate = new Date(noti.intB_EndDate);
                            $scope.displaydate = new Date(noti.intB_DisplayDate);
                            //noticedate.setDate(noticedate.getDate())
                            var curntdate = $scope.curdate == null ? "" : $filter('date')($scope.curdate, "yyyy-MM-dd");
                            var enddate = $scope.noticedate == null ? "" : $filter('date')($scope.noticedate, "yyyy-MM-dd");
                            var dsplydate = $scope.displaydate == null ? "" : $filter('date')($scope.displaydate, "yyyy-MM-dd");

                            if (curntdate >= dsplydate && curntdate <= enddate) {
                                $scope.noticedetailsarray.push({ intB_Title: noti.intB_Title, ntB_TTSylabusFlg: noti.ntB_TTSylabusFlg, intB_Attachment: noti.intB_Attachment, intB_StartDate: noti.intB_StartDate, intB_EndDate: noti.intB_EndDate, intB_FilePath: noti.intB_FilePath });
                            }
                            else if (curntdate == enddate) {
                                $scope.noticedetailsarray.push({ intB_Title: noti.intB_Title, ntB_TTSylabusFlg: noti.ntB_TTSylabusFlg, intB_Attachment: noti.intB_Attachment, intB_StartDate: noti.intB_StartDate, intB_EndDate: noti.intB_EndDate, intB_FilePath: noti.intB_FilePath });
                            }
                            else if ($scope.noticedetailsarray.length == 0) {
                                $scope.nottcdata = "Records Are Not Available!";
                            }
                        }
                    })
                })


                //-----display Homework details into grid view
                //$scope.homeworkarray = [];
                //$scope.hwarray = [];
                //$scope.nowdate = new Date();
                //$scope.nowdate = $scope.nowdate == null ? "" : $filter('date')($scope.nowdate, "yyyy-MM-dd");
                ////$scope.nowdate.setDate($scope.nowdate.getDate());
                ////$scope.nowdate.setDate($scope.nowdate.getDate() - 1)
                //angular.forEach($scope.studetailslst, function (stu1) {
                //    angular.forEach($scope.homeworklist, function (hw) {
                //        if (stu1.asmcL_Id == hw.asmcL_Id) {
                //            $scope.hwdate = new Date(hw.ihW_Date);
                //            $scope.hwdate = $scope.hwdate == null ? "" : $filter('date')($scope.hwdate, "yyyy-MM-dd");

                //            $scope.homeworkarray.push({ ihW_AssignmentNo: hw.ihW_AssignmentNo, ihW_Date: $scope.hwdate, ihW_Topic: hw.ihW_Topic, ihW_Assignment: hw.ihW_Assignment, ihW_Attachment: hw.ihW_Attachment, ismS_SubjectName: hw.ismS_SubjectName, ihW_FilePath: hw.ihW_FilePath, date2: $scope.nowdate });


                //            $scope.curdate = new Date();
                //            $scope.homeworkdate = new Date(hw.ihW_Date);
                //            // homeworkdate.setDate(homeworkdate.getDate())
                //            var currentdate1 = $scope.curdate == null ? "" : $filter('date')($scope.curdate, "yyyy-MM-dd");
                //            var lastdate = $scope.homeworkdate == null ? "" : $filter('date')($scope.homeworkdate, "yyyy-MM-dd");

                //            if (currentdate1 < lastdate) {
                //                $scope.hwarray.push({ ihW_AssignmentNo: hw.ihW_AssignmentNo, ihW_Date: hw.ihW_Date, ihW_Topic: hw.ihW_Topic, ihW_Assignment: hw.ihW_Assignment, ihW_Attachment: hw.ihW_Attachment, ismS_SubjectName: hw.ismS_SubjectName, ihW_FilePath: hw.ihW_FilePath });
                //            }

                //            else if (currentdate1 == lastdate) {
                //                $scope.hwarray.push({ ihW_AssignmentNo: hw.ihW_AssignmentNo, ihW_Date: hw.ihW_Date, ihW_Topic: hw.ihW_Topic, ihW_Assignment: hw.ihW_Assignment, ihW_Attachment: hw.ihW_Attachment, ismS_SubjectName: hw.ismS_SubjectName, ihW_FilePath: hw.ihW_FilePath });
                //            }
                //            else if ($scope.hwarray.length == 0) {
                //                $scope.hwwork = "Not Given Any HomeWork!";
                //            }
                //        }
                //    })
                //})


                //-----display Assignmen details into grid view
                $scope.assignmentarray = [];
                $scope.asgnarray = [];
                $scope.nowdate = new Date();
                //$scope.nowdate.setDate($scope.nowdate.getDate())
                $scope.nowdate = $scope.nowdate == null ? "" : $filter('date')($scope.nowdate, "yyyy-MM-dd");
                angular.forEach($scope.studetailslst, function (stdlst) {
                    angular.forEach($scope.assignmentlist, function (asgn) {
                        var asgnmddate = new Date(asgn.icW_ToDate);
                        if (stdlst.asmcL_Id == asgn.asmcL_Id) {
                            $scope.lastdate = new Date(asgn.icW_ToDate);
                            $scope.lastdate = $scope.lastdate == null ? "" : $filter('date')($scope.lastdate, "yyyy-MM-dd");

                            $scope.assignmentarray.push({ icW_FromDate: asgn.icW_FromDate, icW_ToDate: $scope.lastdate, icW_Topic: asgn.icW_Topic, icW_SubTopic: asgn.icW_SubTopic, icW_Content: asgn.icW_Content, icW_TeachingAid: asgn.icW_TeachingAid, icW_Assignment: asgn.icW_Assignment, icW_Evaluation: asgn.icW_Evaluation, icW_Attachment: asgn.icW_Attachment, icW_FilePath: asgn.icW_FilePath, nowdate: $scope.nowdate });


                            var curdate = new Date();
                            var asgnmddate = new Date(asgn.icW_ToDate);
                            asgnmddate.setDate(asgnmddate.getDate())

                            var smonth = (curdate.getMonth() + 1);
                            var sday = (curdate.getDate());
                            var syear = (curdate.getFullYear());
                            var sdate = smonth + "/" + sday + "/" + syear;

                            var emonth = (asgnmddate.getMonth() + 1);
                            var eday = (asgnmddate.getDate());
                            var eyear = (asgnmddate.getFullYear());
                            var edate = emonth + "/" + eday + "/" + eyear;

                            if (sdate <= edate) {
                                $scope.asgnarray.push({ icW_FromDate: asgn.icW_FromDate, icW_ToDate: asgn.icW_ToDate, icW_Topic: asgn.icW_Topic, icW_Assignment: asgn.icW_Assignment });
                            }
                            else if (sdate == edate) {
                                $scope.asgnarray.push({ icW_FromDate: asgn.icW_FromDate, icW_ToDate: asgn.icW_ToDate, icW_Topic: asgn.icW_Topic, icW_Assignment: asgn.icW_Assignment });
                            }
                            else if ($scope.asgnarray.length == 0) {
                                $scope.assgnwork = "Not Given Any Assignment!";
                            }

                        }
                        else {
                            $scope.assgnwork = "Not Given Any Assignment!";
                        }
                    })
                })



                //-----------------Events / Reminder Date
                angular.forEach($scope.coereportlst, function (coe) {
                    var reminddate = "";
                    var enddate = "";
                    var curDate = new Date();
                    reminddate = new Date(coe.coeE_ReminderDate);
                    enddate = new Date(coe.coeE_EEndDate);
                    enddate.setDate(enddate.getDate() + 1)
                    if (curDate < enddate) {
                        if (reminddate < enddate) {
                            $scope.coeArray.push({
                                coemE_EventName: coe.coemE_EventName, coemE_EventDesc: coe.coemE_EventDesc, coeE_EStartDate: coe.coeE_EStartDate,
                                coeE_EEndDate: coe.coeE_EEndDate
                            })
                        }
                    }
                })
                //----------------------------------Calender
                $scope.calenderlist = promise.calenderlist;
                angular.forEach($scope.calenderlist, function (qwe) {
                    qwe.title = qwe.coemE_EventName;
                    var xyz = $filter('date')(qwe.coeE_EStartDate, "yyyy/MM/dd");
                    var abc = $filter('date')(qwe.coeE_EEndDate, "yyyy/MM/dd");
                    qwe.start = new Date(xyz);
                    //   qwe.end = new Date(abc);
                    //$scope.tempcldrlst.push({ title: qwe.title, start: qwe.start, end: qwe.end });
                    $scope.tempcldrlst.push({ title: qwe.title, start: qwe.start });
                })

                //------------------------------------Documents-

                $scope.docs_Details = promise.docs_Details;
                $scope.tmparydet = [];
                $scope.sylaryy = [];
                $scope.proaryy = [];
                $scope.exmaryy = [];
                $scope.clsnm = {};
                angular.forEach($scope.docs_Details, function (qwe) {
                    $scope.clsnm = qwe.asmcL_ClassName;
                    if (qwe.idU_Type == 'S') {
                        $scope.sylaryy.push(qwe)
                    }
                    if (qwe.idU_Type == 'P') {
                        $scope.proaryy.push(qwe)
                    }
                    if (qwe.idU_Type == 'ETT') {
                        $scope.exmaryy.push(qwe)
                    }
                })
                $scope.tmparydet.push({ asmcL_ClassName: $scope.clsnm, sylary: $scope.sylaryy, proary: $scope.proaryy, exmary: $scope.exmaryy })
                console.log($scope.tmparydet);
                console.log($scope.docs_Details);
                //-------------------studentDetails
                $scope.name = "";
                $scope.cls = "";
                $scope.sect = "";
                if ($scope.studetailslst != null) {
                    for (var a = 0; a < $scope.studetailslst.length; a++) {
                        $scope.name = $scope.studetailslst[a].amsT_FirstName;
                        $scope.classnamecheck = $scope.studetailslst[a].asmcL_ClassName;
                        $scope.cls = $scope.studetailslst[a].asmcL_ClassName;
                        $scope.sect = $scope.studetailslst[a].asmC_SectionName;
                        $scope.year = $scope.studetailslst[a].asmaY_Year;
                        $scope.admno = $scope.studetailslst[a].amsT_AdmNo;
                        $scope.dob = $scope.studetailslst[a].amsT_DOB;
                        $scope.email = $scope.studetailslst[a].amsT_emailId;
                        $scope.mobileno = $scope.studetailslst[a].amsT_MobileNo;
                        $scope.regno = $scope.studetailslst[a].amsT_RegistrationNo;
                        $scope.photo = $scope.studetailslst[a].amsT_Photoname;
                        $scope.tpin = $scope.studetailslst[a].AMST_Tpin;
                    }
                }
                //-------------------------------------------------------PORTAL-Graph

                //#region Graph

                //-------------------Graph Start
                $scope.total_Paid = 0;
                $scope.total_amount = 0;
                if ($scope.yearfeedetails != null) {
                    for (var a = 0; a < $scope.yearfeedetails.length; a++) {
                        $scope.total_Paid += $scope.yearfeedetails[a].paid;
                        $scope.total_amount += $scope.yearfeedetails[a].Total;
                        $scope.balance += $scope.yearfeedetails[a].balance;

                    }
                }
                $scope.total_ch = 0;
                $scope.total_p = 0;
                $scope.atten = 0.00;
                if ($scope.yearattdetails != null) {
                    for (var a = 0; a < $scope.yearattdetails.length; a++) {
                        $scope.total_ch += parseInt($scope.yearattdetails[a].CLASS_HELD);
                        $scope.total_p += $scope.yearattdetails[a].TOTAL_PRESENT;
                    }
                    $scope.atten = ($scope.total_p / $scope.total_ch) * 100;
                }
                //-----------------------Fee data
                $scope.feegraph1 = [];
                if ($scope.yearfeedetails != null) {
                    for (var a = 0; a < $scope.yearfeedetails.length; a++) {
                        $scope.feegraph1.push({ label: $scope.yearfeedetails[a].frommonth + '-' + $scope.yearfeedetails[a].tomonth, "y": $scope.yearfeedetails[a].Total })
                    }
                }
                console.log($scope.feegraph1);
                $scope.feegraph2 = [];
                if ($scope.yearfeedetails != null) {
                    for (var a = 0; a < $scope.yearfeedetails.length; a++) {
                        $scope.feegraph2.push({ label: $scope.yearfeedetails[a].frommonth + '-' + $scope.yearfeedetails[a].tomonth, "y": $scope.yearfeedetails[a].paid })
                    }
                }
                console.log($scope.feegraph2);
                //-----------------------Fee Graph

                var chart = new CanvasJS.Chart("rangeBarChat", {
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
                        // color: "rgba(40,175,101,0.6)",
                        dataPoints: $scope.feegraph1
                    },
                    {
                        name: "PAID AMOUNT",
                        showInLegend: true,
                        type: "column",
                        //  color: "rgba(0,75,141,0.7)",
                        dataPoints: $scope.feegraph2
                    }]
                });
                chart.render();
                //---End

                //-----------------------Attendance data        
                $scope.attegraph1 = [];
                if ($scope.yearattdetails != null) {
                    for (var a = 0; a < $scope.yearattdetails.length; a++) {
                        $scope.attegraph1.push({ label: $scope.yearattdetails[a].MONTH_NAME, "y": parseInt($scope.yearattdetails[a].CLASS_HELD) })
                    }
                }

                $scope.attegraph2 = [];
                if ($scope.yearattdetails != null) {
                    for (var a = 0; a < $scope.yearattdetails.length; a++) {
                        $scope.attegraph2.push({ label: $scope.yearattdetails[a].MONTH_NAME, "y": $scope.yearattdetails[a].TOTAL_PRESENT })
                    }
                }

                //-------------------Attendance Graph   
                var chart = new CanvasJS.Chart("chartContainer", {
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
                        // color: "rgba(40,175,101,0.6)",
                        dataPoints: $scope.attegraph1
                    },
                    {
                        name: "TOTAL PRESENT",
                        showInLegend: true,
                        type: "column",
                        //  color: "rgba(0,75,141,0.7)",
                        dataPoints: $scope.attegraph2
                    }]
                });
                chart.render();

                $scope.studentfeedetails = promise.studentfeedetails;
                if ($scope.studentfeedetails.length > 0) {
                    $scope.totalstudamount = promise.studentfeedetails[0].totalstudamount;
                    $scope.paidamount = promise.studentfeedetails[0].paidamount;
                    $scope.balanceamount = promise.studentfeedetails[0].balanceamount;
                }

            })
        }
        //#endregion

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
                    center: 'title',

                    // center: 'myCustomButton',

                    //center: 'myCustomButton',

                    right: ' prev,next'
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

        var HostName = location.host;
        //Redirect to Fee 
        $scope.onfee = function () {
            $window.location.href = 'http://' + HostName + '/#/app/FeeDetails';
        };
        $scope.oncoe = function () {
            $window.location.href = 'http://' + HostName + '/#/app/COE';
        };

        $scope.saveakpkfile = function () {
            var data = {
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("StudentDashboard/saveakpkfile", data).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal('File Download!');
                    }
                    //else {
                    //    swal('File Not Saved!');
                    //}
                });
        }

        $scope.onAttendance = function () {
            $window.location.href = 'http://' + HostName + '/#/app/AttendanceDetails';
        };


    };
})();