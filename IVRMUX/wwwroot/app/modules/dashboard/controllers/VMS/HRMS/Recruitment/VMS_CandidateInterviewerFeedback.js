(function () {
    'use strict';
    angular
        .module('app')
        .controller('vmsCandidateInterviewerFeedbackController', vmsCandidateInterviewerFeedbackController);

    vmsCandidateInterviewerFeedbackController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', 'dashboardService', 'uiGridExporterService', 'uiGridExporterConstants', '$sce'];
    function vmsCandidateInterviewerFeedbackController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, uiCalendarConfig, superCache, dashboardService, uiGridExporterService, uiGridExporterConstants, $sce) {

        $scope.viewby = 5;
        $scope.currentPage = 4;
        $scope.itemsPerPage = $scope.viewby;
        $scope.maxSize = 5;
        $scope.jobedit = {};
        $scope.Editable = false;
        $scope.tempcldrlst = [];
        $scope.exmids = [];
        $scope.EditCalander = false;
        $scope.riskfactorflag = false;

        // Datatable display
        $scope.gridOptions = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 10,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableColumnMenu: false, enableHiding: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrcD_FullName', displayName: 'Candidate Name', enableHiding: false },
                { name: 'hrcisC_InterviewRounds', displayName: 'Interview Type', enableHiding: false },
                { name: 'hrcisC_InterviewDate', displayName: 'Interview Date', enableHiding: false },
                { name: 'hrcisC_Status', displayName: 'Interview Status', enableHiding: false },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);" data-toggle="tooltip" title="Interviewer Feedback"> <i class="fa fa-pencil-square-o"  style="color:blue;" aria-hidden="true">Feedback</i></a>' +
                        '</div>'
                }
            ],
            exporterCsvFilename: 'CandidateInterviewList.csv',
            exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        $scope.exportCsv = function () {
            var grid = $scope.gridApi.grid;
            var rowTypes = uiGridExporterConstants.ALL;
            var colTypes = uiGridExporterConstants.ALL;
            uiGridExporterService.csvExport(grid, rowTypes, colTypes);
        };

        $scope.setPage = function (pageNo) {
            $scope.currentPage = pageNo;
        };

        $scope.pageChanged = function () {
            console.log('Page changed to: ' + $scope.currentPage);
        };

        $scope.setItemsPerPage = function (num) {
            $scope.itemsPerPage = num;
            $scope.currentPage = 1;
        };

        $scope.addjob = function () {
            $state.go('app.addcandidatIntvw');
        };

        $scope.onLoadGetData = function () {
            $scope.Editable = false;
            $scope.EditCalander = false;
            var pageid = 2;
            apiService.getURI("CandidateInterviewListVMS/getalldetails", pageid).then(function (promise) {

                $scope.gridOptions.data = promise.vmsCandidateInterviewList;
                if (promise.vmsCandidateInterviewList !== null && promise.vmsCandidateInterviewList.length > 0) {
                    if (promise.candidateDetailsList !== null && promise.candidateDetailsList.length > 0) {
                        $scope.candidateDetailsList = promise.candidateDetailsList;
                    }

                    if (promise.interviewerList !== null && promise.interviewerList.length > 0) {
                        $scope.interviewerList = promise.interviewerList;
                    }
                }
                if (promise.riskfactordetails !== null && promise.riskfactordetails.length > 0) {
                    $scope.riskfactordetails = promise.riskfactordetails;
                    $scope.riskfactorflag = true;
                }

                $scope.calenderlist = promise.calenderlist;
                angular.forEach($scope.calenderlist, function (qwe) {
                    qwe.title = qwe.hrcD_FullName;
                    var xyz = $filter('date')(qwe.hrcisC_InterviewDateTime, "yyyy/MM/dd");
                    qwe.start = new Date(xyz);
                    $scope.tempcldrlst.push({ title: qwe.title, start: qwe.start });
                });
                //$scope.events = $scope.tempcldrlst;
                //$scope.eventsF();
            });
        };

        $scope.EditData = function (jobid) {
            $scope.grnlist = [];
            $scope.jobedit = jobid.hrcisC_Id;
            $('#blah').removeAttr('src');
            var pageid = $scope.jobedit;
            apiService.getURI("CandidateInterviewListVMS/editRecord", pageid).
                then(function (promise) {
                    $scope.mrfReq = promise.vmsEditValue[0];
                    $scope.Editable = true;
                    if (promise.vmsEditValue[0].hrcisC_InterviewDateTime !== null) {
                        $scope.mrfReq.hrcisC_InterviewDateTime = new Date(promise.vmsEditValue[0].hrcisC_InterviewDateTime);
                    }
                    else {
                        $scope.mrfReq.hrcisC_InterviewDateTime = null;
                    }

                    if (promise.vmsEditValue[0].hrcD_Photo != null && promise.vmsEditValue[0].hrcD_Photo != "") {
                        $('#blah').attr('src', promise.vmsEditValue[0].hrcD_Photo);
                    }
                    if ($scope.riskfactordetails != null && $scope.riskfactordetails != undefined && $scope.riskfactordetails.length > 0)
                        angular.forEach($scope.riskfactordetails, function (vv) {
                            if (vv.HRCD_Id === $scope.mrfReq.hrcD_Id) {
                                $scope.mrfReq.hrcD_RiskFactor = vv.HRCD_RiskFactor;
                            }
                        });
                    $scope.mrfReq.hrcisC_Status = "Complete";
                    $scope.completedintvw = promise.completedintvw;
                    var filename = $scope.mrfReq.hrcD_Resume.toString();
                    var nameArray = filename.split('.');
                    var extention = nameArray[nameArray.length - 1];
                    $scope.mrfReq.extention = extention;

                    if (extention === 'doc' || extention === 'docx' || extention === 'ppt' || extention === 'pptx' || extention === 'xlsx' || extention === 'xls') {
                        $scope.mrfReq.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + filename;
                    }

                    //EXAM DATA
                    $scope.getstudentdetails = promise.getcandexamdetails;
                    if ($scope.getstudentdetails === null || $scope.getstudentdetails.length === 0) {
                        //swal("No Records Found");
                    } else {
                        $scope.grnlist = [];

                        angular.forEach($scope.getstudentdetails, function (ss) {
                            if ($scope.grnlist.length === 0) {
                                $scope.grnlist.push({
                                    HRCD_Id: ss.HRCD_Id, CandidateName: ss.CandidateName, InterviewDate: ss.InterviewDate, HRCD_EmailId: ss.HRCD_EmailId
                                });
                            }
                            else if ($scope.grnlist.length > 0) {
                                var al_exm_cnt = 0;
                                angular.forEach($scope.grnlist, function (exm) {
                                    if (exm.HRCD_Id === ss.HRCD_Id && exm.InterviewDate === ss.InterviewDate) {
                                        al_exm_cnt += 1;
                                    }
                                });
                                if (al_exm_cnt === 0) {
                                    $scope.grnlist.push({
                                        HRCD_Id: ss.HRCD_Id, CandidateName: ss.CandidateName, InterviewDate: ss.InterviewDate, HRCD_EmailId: ss.HRCD_EmailId
                                    });
                                }
                            }
                        });
                        if ($scope.candidateDetailsList.HRCD_Id == $scope.grnlist.HRCD_Id) {
                            $scope.count = 1;
                        }


                        angular.forEach($scope.grnlist, function (vv) {
                            var examlist = [];
                            angular.forEach($scope.getstudentdetails, function (bb) {
                                if (vv.HRCD_Id === bb.HRCD_Id && vv.InterviewDate === bb.InterviewDate) {
                                    examlist.push(bb);
                                }
                            });

                            vv.LIST = examlist;
                        });
                    }
                    //EXAM DATA

                });
        };





        $scope.cancel = function () {
            $scope.Editable = false;
            $scope.onLoadGetData();
        };

        $scope.showcalender = function () {
            $scope.EditCalander = true;
        };

        $scope.savejob = function () {
            if ($scope.myForm.$valid) {
                $scope.mrfReq.hrciS_CandidateStatus = $scope.hrciS_CandidateStatus;
                var data = $scope.mrfReq;
                apiService.create("AddCandidateInterviewVMS/savefeedback", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {

                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Feedback saved successfully.");
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                            }

                            $scope.cancel();
                        }
                    });
            }
        };

        $scope.showmodaldetails = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.hrcD_Resume.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];

            if (extention == "jpg" || extention == "jpeg") {
                $('#preview').attr('src', data.hrcD_Resume);
            }
            else if (extention == "doc" || extention == "docx") {
                $('#preview').removeAttr('src');
            }
            else if (extention == "pdf") {
                var imagedownload = data.hrcD_Resume;
                $scope.content = "";
                var fileURL = "";
                var file = "";
                $http.get(imagedownload, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        var pdfId = document.getElementById("pdfIdzz");
                        pdfId.removeChild(pdfId.childNodes[0]);
                        var embed = document.createElement('embed');
                        embed.setAttribute('src', fileURL);
                        embed.setAttribute('type', 'application/pdf');
                        embed.setAttribute('width', '100%');
                        embed.setAttribute('height', '1000');
                        pdfId.appendChild(embed);
                        $('#showpdf').modal('show');


                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        $scope.content = $sce.trustAsResourceUrl(fileURL);
                        $('#showpdf').modal('show');
                    });
            }
        };

        //--------------------------------------------------------CALENDER
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
            var m = new Date(start).getMonth();
            var events = [{
                title: 'Feed Me ' + m,
                start: s + (50000),
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

        $scope.ViewQuestion1 = function (dd) {
            $scope.listarray = [];
            $scope.listarray = dd.LIST;

            $scope.mainarray = [];
            $scope.studentname = dd.CandidateName;
            $scope.scheduledd = dd.InterviewDate;
            $scope.EmailId = dd.HRCD_EmailId;

            $scope.ttlqsn = 0;
            $scope.ttlcrctans = 0;
            $scope.ttmxmarks = 0;
            $scope.ttobtnmarks = 0;
            $scope.ttduration = 0;
            $scope.ttper = 0;
            $scope.min = 0;
            $scope.sec = 0;

            angular.forEach($scope.listarray, function (yy) {
                $scope.ttlqsn += yy.OTCANDEX_TotalQnsAnswered;
                $scope.ttlcrctans += yy.OTCANDEX_TotalCorrectAns;
                $scope.ttmxmarks += yy.OTCANDEX_TotalMaxMarks;
                $scope.ttobtnmarks += yy.OTCANDEX_TotalMarks;
                $scope.ttper += yy.OTCANDEX_Percentage;
                if (yy.OTQPTYP_MaxDuration === null || yy.OTQPTYP_MaxDuration === '' || yy.OTQPTYP_MaxDuration === undefined) {
                    yy.OTQPTYP_MaxDuration = 0;
                }
                $scope.ttduration = parseFloat($scope.ttduration) + parseFloat(yy.OTQPTYP_MaxDuration);

                var names = yy.OTCANDEX_TotalTime;
                var nameArr = names.split(':');

                $scope.min = parseFloat($scope.min) + parseFloat(nameArr[0]);
                $scope.sec = parseFloat($scope.sec) + parseFloat(nameArr[1]);
            });

            if ($scope.sec > 60) {
                var minuteValue = parseInt($scope.sec / 60);
                var secondsValue = $scope.sec % 60;
                $scope.sec = secondsValue;
                $scope.min = $scope.min + minuteValue;
            }

            angular.forEach($scope.listarray, function (cc) {
                $scope.exmids.push({ OTCANDEX_Id: cc.OTCANDEX_Id });
            });

            var data = {
                "HRCD_Id": dd.HRCD_Id,
                "type": 'O',
                exmids: $scope.exmids
            };

            apiService.create("OnlineTestCandidate/ViewResultIndRpt", data).then(function (promise) {
                if (promise !== null) {
                    $scope.objectveans = promise.getcandidatequesansdetailsobj;
                    $scope.subjectiveans = promise.getcandidatequesansdetails;
                    $scope.papertypelist = promise.papertypelist;
                    $scope.mainarray = [];

                    angular.forEach($scope.papertypelist, function (pt) {
                        angular.forEach($scope.listarray, function (ls) {
                            if (pt.otqptyP_Id === ls.OTQPTYP_Id) {
                                var araylist = [];
                                angular.forEach($scope.objectveans, function (objj) {
                                    if (objj.OTQPTYP_Id === ls.OTQPTYP_Id && ls.OTCANDEX_Id === objj.OTCANDEX_Id) {
                                        var obmrk = 0;
                                        if (objj.OTCANDEXANS_CorrectAnsFlg === true) {
                                            obmrk = objj.OTMOEQ_Marks;
                                        }
                                        araylist.push({ QID: objj.OTMOEQ_Id, QSN: objj.OTMOEQ_Question, TYPE: objj.OTMOEQ_SubjectiveFlg, MAXM: objj.OTMOEQ_Marks, OBM: obmrk, CANS: objj.QOption, GANS: objj.COption, CFLAG: objj.OTCANDEXANS_CorrectAnsFlg });
                                    }
                                })

                                angular.forEach($scope.subjectiveans, function (objj1) {
                                    $scope.empname = objj1.EmpName;
                                    $scope.usernname = objj1.UserName;
                                    if (objj1.OTQPTYP_Id === ls.OTQPTYP_Id && ls.OTCANDEX_Id === objj1.OTCANDEX_Id) {
                                        var obmrk = 0;
                                        obmrk = objj1.OTCANDEXSANS_Marks;
                                        araylist.push({ QID: objj1.OTMOEQ_Id, QSN: objj1.OTMOEQ_Question, TYPE: objj1.OTMOEQ_SubjectiveFlg, MAXM: objj1.OTMOEQ_Marks, OBM: obmrk, CANS: objj1.OTMOEQ_Answer, GANS: objj1.OTCANDEXSANS_Answer, CFLAG: '' });
                                    }
                                })

                                $scope.mainarray.push({ PPRT: ls.OTQPTYP_QuestionPaperName, EDATE: ls.OTCANDEX_Date, TQSN: ls.OTCANDEX_TotalQnsAnswered, TOTALTIME: ls.OTQPTYP_MaxDuration, TMARKS: ls.OTCANDEX_TotalMaxMarks, TKTIME: ls.OTCANDEX_TotalTime, TCNS: ls.OTCANDEX_TotalCorrectAns, TOBM: ls.OTCANDEX_TotalMarks, PER: ls.OTCANDEX_Percentage, LIST: araylist })
                            }
                        })
                    })

                    console.log($scope.mainarray);
                    $('#myModalViewresult').modal('show');
                }
            });
        };
    }

})();