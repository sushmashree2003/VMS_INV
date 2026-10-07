    (function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_DuplicateTaskCloseController', ISM_DuplicateTaskCloseController);
        ISM_DuplicateTaskCloseController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', 'superCache', '$filter', '$sce', 'intracontroller'];
        function ISM_DuplicateTaskCloseController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, Excel, $timeout, superCache, $filter, $sce, intracontroller) {

        //var paginationformasters;
        //var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings.length > 0) {
        //    paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        //}
        //else {
        //    paginationformasters = 0;
        //}.    
        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1);
            }
        $scope.filterValue = [];
        $scope.searchValueUEM = '';
        $scope.maintable = true;
        $scope.updatetable = false;
        $scope.excludesearch = [];
        document.getElementById("lablehide").style.display = "none";
        $scope.nexttaskshow = true;
        $scope.showid = false;
        $scope.eacreateshow = false;
        $scope.eaassignshow = false;
        $scope.searchingtask = false;
        $scope.backtaskshow = true;
        $scope.disabletasks = false;
        $scope.showassignflag = true;
        $scope.notifyMe = function () {
            if (Notification.permission !== 'granted')
                Notification.requestPermission();
            else {
                var notification = new Notification('New Task Assigned', {
                    icon: 'https://dcampusstrg.blob.core.windows.net/files/0/VMSLogo-1.png',
                    body: 'Hey there! You Got New Task!',
                });
                notification.onclick = function () {
                    window.open('http://localhost:57606/#/app/ISM_ComplaintsTaskList/729');
                };
            }
        }
        $scope.get_approvedcountNew = [];
        var formatStr = 'DD/MM/YYYY';
        $scope.from_date = "";
        $scope.to_date = "";
        $scope.resetdates = function () {
            $scope.from_date = null;
            $scope.to_date = null;
        };

        $scope.tablename = "";
        $scope.shownext = true;
        $scope.showtype = 1;
        $scope.attachmentsshow = [];
        $scope.compltetedcounttask = 0;
        $('.modal').on('hide.bs.modal', function (e) {
            e.stopPropagation();
            $('body').css('padding-right', '');
        });
        $scope.headtype = false;
        $scope.getserverdate = function () {
            var xmlHttp;
            function srvTime() {
                try {
                    //FF, Opera, Safari, Chrome
                    xmlHttp = new XMLHttpRequest();
                }
                catch (err1) {
                    //IE
                    try {
                        xmlHttp = new ActiveXObject('Msxml2.XMLHTTP');
                    }
                    catch (err2) {
                        try {
                            xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
                        }
                        catch (eerr3) {
                            //AJAX not supported, use CPU time.
                            alert("AJAX not supported");
                        }
                    }
                }
                xmlHttp.open('HEAD', window.location.href.toString(), false);
                xmlHttp.setRequestHeader("Content-Type", "text/html");
                xmlHttp.send('');
                return xmlHttp.getResponseHeader("Date");
            }
            $scope.today = srvTime();
        }
        $scope.getserverdate();

        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        $scope.obj = {};
        $scope.nexttaskobj = {};
        $scope.nexttaskobjm = {};
        $scope.objupdate = {};
        $scope.responsetrans = 0;
        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.typeFlg = "Default";
        $scope.ismtcrastO_AssignedDate = new Date($scope.today);
        $scope.ismtcrtrtO_TransferredDate = new Date($scope.today);
        $scope.ismtcrreS_ResponseDate = new Date($scope.today);
        $scope.currentDate = new Date($scope.today);
        $scope.notassign = false;
        $scope.activeclass = "activet";

        var options = [];

        $('.dropdown-menu a').on('click', function (event) {

            var $target = $(event.currentTarget),
                val = $target.attr('data-value'),
                $inp = $target.find('input'),
                idx;

            if ((idx = options.indexOf(val)) > -1) {
                options.splice(idx, 1);
                setTimeout(function () { $inp.prop('checked', false) }, 0);
            } else {
                options.push(val);
                setTimeout(function () { $inp.prop('checked', true) }, 0);
            }

            $(event.target).blur();

            console.log(options);
            return false;
        });

        $scope.clearexclude = function () {
            if ($scope.excludesearch !== undefined) {
                $scope.excludesearch.ISMTCR_Title = '';
                $scope.excludesearch.ISMMCLT_ClientName = '';
                $scope.excludesearch.IVRMM_ModuleName = '';
                $scope.excludesearch.createdby = '';
                $scope.excludesearch.assignedby = '';
                $scope.excludesearch.assignedto = '';
            }
            if ($scope.excludecreated.length > 0) {
                angular.forEach($scope.excludecreated, function (value) {
                    value.creclass = 'cretaed';
                    value.showcross = false;
                });
            }
            if ($scope.excludeassign.length > 0) {
                angular.forEach($scope.excludeassign, function (value) {
                    value.creclass = 'cretaed';
                    value.showcross = false;
                });
            }
            $scope.eacreateshow = false;
            $scope.eaassignshow = false;
            $scope.eacreate = false;
            $scope.eaassign = false;
        }

        $scope.clearsearch = function () {

            //document.getElementById("searchField1").value = null;
            //document.getElementById("searchField2").value = null;
            //document.getElementById("searchField3").value = null;
            //document.getElementById("searchField4").value = null;
            //document.getElementById("searchField5").value = null;
            //document.getElementById("searchField6").value = null;
            $scope.filterValue = [];
            $scope.get_approvedcountNew = [];
            $scope.get_tasks = [];
            //$scope.searchValueall = [];
            $scope.RejectedTask = [];
            $scope.get_comptasklist = [];
            $scope.currentPage = 1;
            $scope.from_date = null;
            $scope.to_date = null;
            $scope.searchValueall = null;
            if ($scope.searchValueall !== undefined) {
                $scope.searchValueall = '';
            }
            if ($scope.searchValuecol !== undefined) {
                $scope.searchValuecol.ISMTCR_Title = '';
                $scope.searchValuecol.ISMMCLT_ClientName = '';
                $scope.searchValuecol.IVRMM_ModuleName = '';
                $scope.searchValuecol.createdby = '';
                $scope.searchValuecol.assignedby = '';
                $scope.searchValuecol.assignedto = '';
            }
            if ($scope.excludesearch !== undefined) {
                $scope.excludesearch.ISMTCR_Title = '';
                $scope.excludesearch.ISMMCLT_ClientName = '';
                $scope.excludesearch.IVRMM_ModuleName = '';
                $scope.excludesearch.createdby = '';
                $scope.excludesearch.assignedby = '';
                $scope.excludesearch.assignedto = '';
            }
            $scope.alltask = false;
            $scope.excludeyearpln = false;
            $scope.obj = {};
            $scope.searchingtask = false;
        };

        $scope.employeecheck = function (main, checkedvalue) {
            angular.forEach($scope.taskplanner, function (qq) {
                if (main.ISMTCR_Id === qq.ISMTCR_Id) {
                    angular.forEach(qq.emplist, function (aa) {
                        if (checkedvalue.id === aa.id) {
                            aa.checked = true;
                        }
                        else {
                            aa.checked = false;
                        }
                    });
                }
            });
        };

        $scope.exportToExcel = function () {          
            var printSectionId = '#table1';
            if ($scope.get_comptasklist !== null && $scope.get_comptasklist.length > 0) {
                var exportHref = Excel.tableToExcel(printSectionId, 'COMPLAINTS/TASK');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = "COMPLAINTS/TASK LIST.xls";
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        };

        $scope.exportToExceltwo = function () {
            var printSectionId = '#artask';
            var exportHref = Excel.tableToExcel(printSectionId, 'COMPLAINTS/TASK');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = "COMPLAINTS/TASK LIST.xls";
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.loaddata = function () {
            //$scope.notifyMe();
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage = 1;
            $scope.get_comptasklist = [];
            $scope.itemsPerPage = 10;
            $scope.search = "";
            $scope.showactive = true;
            $scope.activeclass1 = "activet";
            $scope.activeclass2 = "";
            $scope.activeclass3 = "";
            $scope.activeclass4 = "";
            $scope.activeclassnot = "";
            var pageid = 2;
            var data = {
                "typeFlg": $scope.typeFlg
            };
            apiService.create("ISM_ComplaintsTaskList/getDuplicateTasksList", data).then(function (promise) {
                $scope.get_comptasklist_all = promise.get_comptasklist;
                if (promise.roletype === "Head") {
                    $scope.headtype = true;
                };
                $scope.tablename = "My Open";
                $scope.get_comptasklist = promise.get_comptasklist;
                
                
            });
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;
                $scope.reverse = !$scope.reverse;
            };
  
        };
        $scope.approvalTaskLoadData = function () {
            //$scope.notifyMe();
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage = 1;
            $scope.get_comptasklist = [];
            $scope.itemsPerPage = 10;
            $scope.search = "";
            $scope.showactive = true;
            $scope.activeclass1 = "activet";
            $scope.activeclass2 = "";
            $scope.activeclass3 = "";
            $scope.activeclass4 = "";
            $scope.activeclassnot = "";
            var pageid = 2;
            var data = {
                "typeFlg": $scope.typeFlg
            };
            apiService.create("ISM_ComplaintsTaskList/getDuplicateApprovalTasksList", data).then(function (promise) {
                $scope.get_comptasklist_all = promise.get_comptasklist;
                if (promise.roletype === "Head") {
                    $scope.headtype = true;
                };
                $scope.tablename = "Duplicate Open";
                $scope.get_comptasklist = promise.get_comptasklist;
            });
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;
                $scope.reverse = !$scope.reverse;
            };
        }
        $scope.saveDuplicateTask = function () {
            if ($scope.filterValue.length > 0) {
                console.log($scope.filterValue);
                var selectedTasks = [];
                angular.forEach($scope.get_comptasklist, function (task) {
                    if (task.checkedvalue) {
                        selectedTasks.push({
                            ISMTCR_Id: task.ISMTCR_Id,
                            ISMTCRASTO_Remarks: task.remarks || ''
                        });
                    }
                });
                if (selectedTasks.length > 0) {
                    var data = {
                        plannerArray: selectedTasks
                    };
                    apiService.create("ISM_ComplaintsTaskList/saveDuplicateTask", data).then(function (promise) {
                        if (promise.message == "Duplicate") {
                            swal("Duplicate record!");
                        }
                        else if(promise.message == "Admin") {
                            swal("Record unable to save. Please contact admnistrator!");
                        }
                        else if (promise.returnval == true) {
                            swal("Record saved successfully");
                        }
                        else if (promise.returnval == false) {
                            swal("Record not saved");
                        }
                        $scope.loaddata();
                    });
                }
                else {
                    swal("Please select atleast one task!");
                }
                console.log(selectedTasks);
            }
        }

        $scope.approveOrRejectDuplicateTask = function (flag) {
            if ($scope.filterValue.length > 0) {
                console.log($scope.filterValue);
                var selectedTasks = [];
                angular.forEach($scope.get_comptasklist, function (task) {
                    if (task.checkedvalue) {
                        selectedTasks.push(task.ISMTCR_Id);
                    }
                });
                var commaSeparatedIds = selectedTasks.join(',');
                if (selectedTasks.length > 0) {
                    var data = {
                        ISMTCR_Ids: commaSeparatedIds,
                        flag: flag
                    };
                    apiService.create("ISM_ComplaintsTaskList/approveOrRejectDuplicateTask", data).then(function (promise) {
                        if (promise.message == "Admin") {
                            swal("Record unable to save. Please contact admnistrator!");
                        }
                        else if (promise.returnval == true) {
                            swal("Record saved successfully");
                        }
                        else if (promise.returnval == false) {
                            swal("Record not saved");
                        }
                        $scope.approvalTaskLoadData();
                    });
                }
                else {
                    swal("Please select atleast one task!");
                }
                console.log(selectedTasks);
            }
        }

        $scope.all_task = function (atk) {
            $scope.alltask = atk;
            //[0].ISMTCR_Status
            var toggleStatus = $scope.alltask;
            angular.forEach($scope.filterValue, function (atck) {
                if (atck.ISMTCR_Status != 'Close') {
                    atck.checkedvalue = toggleStatus;
                }
                else {
                    atck.checkedvalue = false;
                }

            });
        };
        //all_task
        $scope.all_taskApproved = function (atk) {
            $scope.alltaskk = atk;
            var toggleStatus = $scope.alltaskk;
            angular.forEach($scope.get_approvedcountNew, function (atck) {
                atck.checkedvalue = toggleStatus;
            });
        };
        $scope.radiochange = function (ee) {
            if (ee.flag === 'A') {
                ee.classname = 'neww';
            }
            else {
                ee.classname = 'oldd';
            }

        }
        $scope.togchkbxtask = function () {
            $scope.alltask = $scope.filterValue.every(function (options) {
                //if (options.assignedflg === false || $scope.countflag === "Close") {
                return options.checkedvalue;
                //}
            });
        };

        $scope.togchkbxtask = function () {
            $scope.alltask = $scope.filterValue.every(function (options) {
                return options.checkedvalue;
            });
        };
        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.get_userEmplist, function (uem) {
                uem.empck = toggleStatus;
            });
        };
        $scope.togchkbx = function () {
            $scope.checkall = $scope.get_userEmplist.every(function (options) {
                return options.empck;
            });
        };
        $scope.all_checkS = function (ack) {
            $scope.checkallS = ack;
            var toggleStatus = $scope.checkallS;
            angular.forEach($scope.get_Status, function (sts) {
                sts.stck = toggleStatus;
            });
        };
        $scope.togchkbxS = function () {
            $scope.checkallS = $scope.get_Status.every(function (options) {
                return options.stck;
            });
        };

        //======================================== Search List
        $scope.searchtasks = function () {
            $scope.clearsearch();
            $scope.currentPage = 1;
            $scope.typeFlg = "Search";
            $scope.countflag = "Search";
            $scope.tablename = "Search Result";
            $scope.notassign = false;
            $scope.arrayuserEmp = [];
            $scope.arrayStatus = [];
            $scope.get_comptasklist = [];
            angular.forEach($scope.get_userEmplist, function (em) {
                if (em.empck === true) {
                    $scope.arrayuserEmp.push(em);
                }
            });
            angular.forEach($scope.get_Status, function (sts) {
                if (sts.stck === true) {
                    $scope.arrayStatus.push(sts);
                }
            });
            var data = {
                "typeFlg": $scope.typeFlg,
                "arrayuserEmp": $scope.arrayuserEmp,
                "arrayStatus": $scope.arrayStatus
            };
            apiService.create("ISM_ComplaintsTaskList/getsearchtask", data).
                then(function (promise) {
                    $scope.get_comptasklist = promise.get_comptasklist;
                    $scope.get_comptasklistfilter = promise.get_comptasklist;
                    if ($scope.get_comptasklist != null && $scope.get_comptasklist.length > 0 && $scope.headtype === true) {
                        angular.forEach($scope.get_comptasklist, function (tas) {
                            tas.HRMPR_Id = '';
                            angular.forEach($scope.get_priority, function (pri) {
                                if (tas.HRMP_Name === pri.hrmP_Name) {
                                    tas.HRMPR_Id = pri.hrmpR_Id;
                                }
                            });
                        });
                    }
                    angular.forEach($scope.get_comptasklist, function (atck) {
                        atck.atck = $filter('date')(atck.ISMTCR_CreationDate, "dd-MM-yyyy");
                    });
                    if ($scope.get_comptasklist.length > 0) {
                        var notassignedcount = 0;
                        var assignedcount = 0;
                        var pendingtaskcount = 0;
                        var transfertaskcount = 0;
                        angular.forEach($scope.get_comptasklist, function (clt) {
                            if (clt.ISMTCR_Status === 'Open') {
                                pendingtaskcount += 1;
                            }
                            if (clt.tasktag === 2) {
                                transfertaskcount += 1;
                            }
                            if (clt.addtoplannerflag === 1) {
                                notassignedcount += 1;
                            }
                            else {
                                assignedcount += 1;
                            }
                        });
                        $scope.excludecreateduserlist();
                        $scope.excludeassignuserlist();
                        //$scope.assignedcount = assignedcount;
                        //$scope.notassignedcount = notassignedcount;
                        //$scope.pendingcount = pendingtaskcount;
                        //$scope.transfercount = transfertaskcount;
                    }
                    else {
                        //$scope.assignedcount = 0;
                        //$scope.notassignedcount = 0;
                        //$scope.pendingcount = 0;
                        //$scope.transfercount = 0;
                        swal("No Record Found!!");
                    }
                });
        };

        $scope.searchtasksdetails = function (ismtcRTitle) {
            $scope.arrayuserEmp = [];
            $scope.countflag = "Search";
            $scope.clearexclude();
            $scope.searchingtask = true;
            $scope.currentPage = 1;
            $scope.tablename = "Searched";
            $scope.notassign = false;
            $scope.alltask = false;
            angular.forEach($scope.get_userEmplist, function (em) {
                //if (em.empck === true) {
                $scope.arrayuserEmp.push(em);
                //}
            });
            var data = {
                "ISMTCR_TaskNo": $scope.obj.ismtcRTitle,
                "arrayuserEmp": $scope.arrayuserEmp
            };
            apiService.create("ISM_ComplaintsTaskList/getsearchtaskdetails", data).
                then(function (promise) {
                    if (promise.get_comptasklist !== null && promise.get_comptasklist.length > 0) {
                        $scope.get_comptasklist = promise.get_comptasklist;
                        $scope.get_comptasklistfilter = promise.get_comptasklist;
                        if ($scope.get_comptasklist != null && $scope.get_comptasklist.length > 0 && $scope.headtype === true) {
                            angular.forEach($scope.get_comptasklist, function (tas) {
                                tas.HRMPR_Id = '';
                                angular.forEach($scope.get_priority, function (pri) {
                                    if (tas.HRMP_Name === pri.hrmP_Name) {
                                        tas.HRMPR_Id = pri.hrmpR_Id;
                                    }
                                });
                            });
                        }
                        if (intracontroller.taskno != "") {
                            $scope.viewresponse(intracontroller);
                        }
                    }
                    else {
                        //$scope.getpendingcount();
                        $scope.obj = {};
                        swal("No match found!!");
                    }
                });
        };

       

        $scope.goback = function () {
            $scope.scroll();
            $scope.maintable = true;
            $scope.updatetable = false;
        };

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.employeeName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };
        $scope.savePlanner = function () {
            if ($scope.myForm1.$valid) {
                $scope.plannerArray = [];
                $scope.taskEmpArray = [];
                var totcheck = 0;
                angular.forEach($scope.taskplanner, function (tp) {
                    if (tp.checkedvalue === true) {
                        $scope.plannerArray.push(tp);
                        angular.forEach(tp.emplist, function (ep) {
                            if (ep.checked === true) {
                                $scope.taskEmpArray.push({ ISMTCR_Id: tp.ISMTCR_Id, HRME_Id: ep.HRME_Id, HRMPR_Id: tp.HRMPR_Id });
                                // if (ep.checked === true) {
                                totcheck = totcheck + 1;
                                // }
                            }
                        });
                        $scope.totalcheck = totcheck;
                    }
                });
                var counthrs = 0.00;
                var totalhrs = 0.00;
                $scope.validationmineff = false;
                angular.forEach($scope.plannerArray, function (countefforts) {

                    //if (countefforts.ISMMTCAT_DurationFlg === "HOURS") {
                    //    counthrs = parseFloat(countefforts.ISMMTCAT_EachTaskMaxDuration);
                    //} else if (countefforts.ISMMTCAT_DurationFlg === "MINUTES") {
                    //    counthrs = (parseFloat(countefforts.ISMMTCAT_EachTaskMaxDuration) * 0.0166667).toFixed(2);
                    //}

                    counthrs = (parseFloat(countefforts.effmin) * 0.0166667).toFixed(2);
                    counthrs = parseFloat(countefforts.effhrs) + parseFloat(counthrs);
                    if (counthrs == 0 || counthrs == null) {
                        $scope.validationmineff = true;
                        countefforts.effmin = '';
                        countefforts.effhrs = '';
                        return;
                    };
                    countefforts.ismtcrastO_EffortInHrs = counthrs;
                });
                if ($scope.validationmineff === true) {
                    $scope.submitted1 = true;
                    swal("Kindly enter minimum effort for the task!!");

                    return;
                };
                if ($scope.totalcheck > 0) {
                    var data = {
                        "ISMTCRASTO_Id": $scope.ismtcrastO_Id,
                        "ISMTCRASTO_AssignedDate": new Date($scope.ismtcrastO_AssignedDate).toDateString(),
                        "plannerArray": $scope.plannerArray,
                        "taskEmpArray": $scope.taskEmpArray
                    };
                }
                else {
                    swal("Select Atleast one Employee from list.....!!");
                }
                apiService.create("ISM_ComplaintsTaskList/savePlanner", data).then(function (promise) {
                    if (promise.returnval === true) {
                        if (promise.ismtcrastO_Id === 0 || promise.ismtcrastO_Id < 0) {
                            //$scope.lodarefresh();
                            swal('Task added to planner successfully');
                            $('#myModalAddtoPlanner').modal('hide');
                            $('.modal-backdrop').remove();

                            //$scope.get_comptasklisttot = $filter('filter')($scope.get_comptasklist, function (d) {
                            //    return d.checkedvalue === true;
                            //});
                            //$scope.get_tasks_count = ($scope.get_comptasklist.length) - ($scope.get_comptasklisttot.length);
                            $scope.get_comptasklist = $filter('filter')($scope.get_comptasklist, function (d) {
                                return (d.checkedvalue === false || d.checkedvalue === undefined);
                            });
                            $scope.currentPage = 1;
                        }
                        else if (promise.ismtcrastO_Id > 0) {
                            swal('Record updated successfully');
                            $('#myModalAddtoPlanner').modal('hide');
                            $('.modal-backdrop').remove();
                            //$scope.get_comptasklisttot = $filter('filter')($scope.get_comptasklist, function (d) {
                            //    return d.checkedvalue === true;
                            //});
                            //$scope.get_tasks_count = ($scope.get_comptasklist.length) - ($scope.get_comptasklisttot.length);
                            $scope.get_comptasklist = $filter('filter')($scope.get_comptasklist, function (d) {
                                return (d.checkedvalue === false || d.checkedvalue === undefined);
                            });
                            $scope.currentPage = 1;
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.ismtcrastO_Id === 0 || promise.ismtcrastO_Id < 0) {
                            swal('Failed to add, please contact administrator');
                        }
                        else if (promise.ismtcrastO_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }

                });
            }
            else {
                //$('#myModalAddtoPlanner').modal('show');   
                $scope.submitted1 = true;
                //  $('#myModalAddtoPlanner').modal('show');   
            }
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
            
        //======================================== Response
        $scope.viewresponse = function (taskobj, fileview, taskind, pageno, f) {
            //$('#myModalResponse').modal('show'); 
            $scope.submitted1 = false;
            $scope.pageno = pageno;
            $scope.f = f;
            if (f == true) {
                $scope.disabletasks = true;
            }
            else {
                $scope.disabletasks = false;
            }
            if (taskobj.checkedvalue == true) {
                $scope.showassignflag = false;
            }
            else {
                $scope.showassignflag = true;
            }

            $scope.taskindex = taskind;
            $scope.taskindextemp = taskind;
            $scope.pagetask = 0;
            if (pageno > 1) {
                $scope.pagetask = 10 * (pageno - 1);
            }
            $scope.taskindex = $scope.taskindex + $scope.pagetask;
            $scope.taskslength = $scope.get_comptasklist.length;
            $scope.currenttaskslength = ($scope.taskindex + 1);
            if (($scope.taskslength - 1) == $scope.taskindex) {
                $scope.nexttaskshow = false;
            }
            else {
                $scope.nexttaskshow = true;
            }
            if ($scope.taskindex == 0) {
                $scope.backtaskshow = false;
            }
            else {
                $scope.backtaskshow = true;
            }
            $scope.Rep_assignedto = taskobj.assignedto;
            $scope.tasktitle = "";
            $scope.taskno = "";
            $scope.tasktype = "";
            $scope.tsakpriority = "";
            $scope.priorityswitch = "";
            $scope.prioritypreswitch = "";
            $scope.created_by = "";
            $scope.Rep_assignedto = "";
            $scope.leftflag = "";
            $scope.rep_assignedby = "";
            $scope.rep_assignedDate = "";
            $scope.plancount = "";
            $scope.periodicity = "";
            $scope.Rep_module = "";
            $scope.taskdate = "";
            $scope.taskstatus = "";
            $scope.startdate = "";
            $scope.enddate = "";
            $scope.effhrstemp = "";
            $scope.effmintemp = "";
            $scope.taskdescription = "";
            $scope.ismtcrreS_Response = "";
            $scope.files_paths = "";
            $scope.ismmistS_StatusName = "";
            $scope.taskAttatchment = [];
            $scope.get_taskdetails = [];
            $scope.objupdate.eachTaskMaxDuration = 0;
            $scope.objupdate.durationFlg = "";
            $scope.get_responseattachments = [];
            $scope.taskid = taskobj.ISMTCR_Id;
            $scope.created_by = taskobj.createdby;
            if (fileview != undefined && fileview != "" && fileview != null) {
                $scope.filedirectview = true;
            }
            else {
                $scope.filedirectview = false;
            }
            var data = {
                "ISMTCR_Id": $scope.taskid
            };
            apiService.create("ISM_ComplaintsTaskList/gettaskdetails", data).
                then(function (promise) {
                    $scope.get_taskdetails = promise.get_taskdetails;
                    $scope.get_taskresponse = promise.get_taskresponse;
                    $scope.taskeditable = promise.taskeditable;
                    $scope.assigntaskeditable = promise.assigntaskeditable;

                    if (promise.role_flag == "Head") {
                        $scope.headornot = true
                    }
                    else {
                        $scope.headornot = false
                    }

                    if ($scope.get_taskdetails !== null && $scope.get_taskdetails.length > 0) {

                        $scope.userhrmeid = promise.hrmE_Id;
                        $scope.createdhrmeid = $scope.get_taskdetails[0].createdhrmeid;
                        $scope.crdprthead = $scope.get_taskdetails[0].crdprthead;
                        $scope.tasktitle = $scope.get_taskdetails[0].ISMTCR_Title;
                        $scope.taskno = $scope.get_taskdetails[0].ISMTCR_TaskNo;
                        $scope.taskclient = $scope.get_taskdetails[0].ISMMCLT_ClientName;
                        $scope.Rep_module = $scope.get_taskdetails[0].IVRMM_ModuleName;
                        $scope.taskdate = $scope.get_taskdetails[0].ISMTCR_CreationDate;
                        $scope.tasktype = $scope.get_taskdetails[0].tasktype;
                        $scope.tsakpriority = $scope.get_taskdetails[0].HRMP_Name;
                        $scope.priorityswitch = $scope.get_taskdetails[0].priorityswitch;
                        $scope.prioritypreswitch = $scope.get_taskdetails[0].preswitch;
                        $scope.taskdescription = $scope.get_taskdetails[0].ISMTCR_Desc;
                        $scope.taskstatus = $scope.get_taskdetails[0].ISMTCR_Status;
                        $scope.createdby = $scope.get_taskdetails[0].createdby;
                        $scope.Rep_assignedto = $scope.get_taskdetails[0].assignedto;
                        $scope.leftflag = $scope.get_taskdetails[0].empleft;
                        $scope.plancount = $scope.get_taskdetails[0].plancount;
                        $scope.periodicity = $scope.get_taskdetails[0].ISMTAPL_Periodicity;
                        $scope.rep_assignedby = $scope.get_taskdetails[0].assignedby;
                        $scope.rep_assignedDate = $scope.get_taskdetails[0].assignedDate;
                        //$scope.ismmistS_StatusName = $scope.taskstatus;
                        $scope.created_by = $scope.get_taskdetails[0].createdby;
                        if ($scope.get_taskdetails[0].ISMTCRASTO_StartDate != null) {
                            $scope.startdate = new Date($scope.get_taskdetails[0].ISMTCRASTO_StartDate).toDateString();
                            $scope.enddate = new Date($scope.get_taskdetails[0].ISMTCRASTO_EndDate).toDateString();
                        }
                        $scope.effort = $scope.get_taskdetails[0].ISMTCRASTO_EffortInHrs;
                        $scope.objupdate.eachTaskMaxDuration = $scope.get_taskdetails[0].ISMTCRASTO_EffortInHrs;
                        //$scope.objupdate.durationFlg = $scope.get_taskdetails[0].ISMTCRASTO_TimeRequiredFlg;
                        $scope.objupdate.durationFlg = "HOURS";

                        var decimaltimetot = $scope.effort;
                        var hrstot = parseInt(Number(decimaltimetot));
                        var mintot = Math.round((Number(decimaltimetot) - hrstot) * 60);
                        $scope.objupdate.effhrs = hrstot;
                        $scope.objupdate.effmin = mintot;
                        if (hrstot.toString().length === 1) {
                            hrstot = '0' + hrstot;
                        }
                        if (mintot.toString().length === 1) {
                            mintot = '0' + mintot;
                        }
                        $scope.effhrstemp = hrstot;
                        $scope.effmintemp = mintot;

                        $scope.tempstatus = [];
                        if ($scope.userhrmeid !== $scope.createdhrmeid) {
                            if ($scope.userhrmeid !== $scope.crdprthead) {
                                angular.forEach($scope.get_Status, function (task) {
                                    if (task.ismmistS_StatusName != "Close" && task.ismmistS_StatusName != "close") {
                                        $scope.tempstatus.push(task);
                                    };
                                });
                            }
                            else {
                                $scope.tempstatus = $scope.get_Status;
                            }
                            if ($scope.get_taskdetails[0].addtoplannerflag === true || $scope.get_taskdetails[0].addtoplannerflag === 1) {
                                $scope.tempstatus = [];
                                angular.forEach($scope.get_Status, function (task) {
                                    if (task.ismmistS_StatusName.toLowerCase() != "close" && task.ismmistS_StatusName.toLowerCase() != "completed" && task.ismmistS_StatusName.toLowerCase() != "development completed") {
                                        $scope.tempstatus.push(task);
                                    };
                                });
                            }
                        }
                        else {
                            $scope.tempstatus = $scope.get_Status;
                        }

                        if ($scope.headornot === false) {
                            for (var i = $scope.tempstatus.length - 1; i >= 0; i--) {
                                if ($scope.tempstatus[i].ismmistS_StatusName.toLowerCase() == "onhold-department" || $scope.tempstatus[i].ismmistS_StatusName.toLowerCase() == "onhold" || $scope.tempstatus[i].ismmistS_StatusName.toLowerCase() == "management blocker") {
                                    $scope.tempstatus.splice(i, 1);
                                }
                            }
                        }

                        $scope.taskAttatchment = promise.get_taskattachments;
                        $scope.get_responseattachments = promise.get_responseattachments;
                        if ($scope.taskAttatchment.length > 0) {
                            angular.forEach($scope.taskAttatchment, function (at) {
                                if (at.ISMTCRAT_Attatchment !== null && at.ISMTCRAT_Attatchment !== "") {
                                    var img = at.ISMTCRAT_Attatchment;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];
                                    at.filetype = lastelement;
                                }
                            });
                        }
                        if ($scope.get_responseattachments.length > 0) {
                            angular.forEach($scope.get_responseattachments, function (at) {
                                if (at.ISMTCRRES_ResponseAttachment !== null && at.ISMTCRRES_ResponseAttachment !== "") {
                                    var img1 = at.ISMTCRRES_ResponseAttachment;
                                    var imagarr1 = img1.split('.');
                                    var lastelement1 = imagarr1[imagarr1.length - 1];

                                    at.filetype = lastelement1;
                                }
                            });
                        }
                        if ($scope.get_taskresponse.length > 0) {
                            if ($scope.get_responseattachments.length > 0) {
                                angular.forEach($scope.get_taskresponse, function (at) {
                                    var slno = 0;
                                    angular.forEach($scope.get_responseattachments, function (atr) {
                                        if (at.ISMTCRRES_Response === atr.ISMTCRRES_Response) {
                                            slno = slno + 1;
                                            atr.slno = slno;
                                        }
                                    });
                                    //var lines = 1;
                                    //lines = at.ISMTCRRES_Response.split(/\r\n|\r|\n/).length;
                                    //at.lines = lines;
                                    at.student_rmrks = at.ISMTCRRES_Response.split("\n");
                                });
                            }
                        }
                        //angular.forEach($scope.get_taskresponse, function (at) {
                        //    if (at.ISMTCRAT_Attatchment !== "" && at.ISMTCRAT_Attatchment !== null && at.ISMTCRAT_Attatchment !== undefined) {
                        //        $scope.taskAttatchment.push({ ISMTCRAT_Attatchment: at.ISMTCRAT_Attatchment });
                        //    }
                        //    else if (at.ISMTCRRES_ResponseAttachment !== "" && at.ISMTCRRES_ResponseAttachment !== null && at.ISMTCRRES_ResponseAttachment !== undefined) {
                        //        $scope.taskAttatchment.push({ ISMTCRAT_Attatchment: at.ISMTCRRES_ResponseAttachment });
                        //    }
                        //    else  {
                        //        $scope.taskAttatchment.length = 0;
                        //    }
                        //});

                        if ($scope.filedirectview == true) {
                            $('#myModalResponse').modal('hide');
                            $scope.viewdirecttaskfiles();
                        }
                        else {
                            $('#myModalResponse').modal('show');
                        }
                    }
                    else {
                        $('#myModalResponse').modal('hide');
                    }
                });
        };

        //========================================Multi Update Response
        $scope.viewresponsemultiple = function (taskobj, fileview, taskindmulti, pagenomulti, f) {

            taskobj = $filter('filter')($scope.taskupadtelist, function (d) {
                return (d.ISMTCR_Id === taskobj.ISMTCR_Id);
            });

            $scope.submitted1 = false;
            pagenomulti = 1;
            $scope.taskindexmulti = taskindmulti;
            $scope.pagetaskmulti = 0;
            if (pagenomulti > 1) {
                $scope.pagetaskmulti = 10 * (pagenomulti - 1);
            }
            $scope.taskindexmulti = $scope.taskindexmulti + $scope.pagetaskmulti;
            $scope.currenttaskslengthmulti = ($scope.taskindexmulti + 1);
            if ($scope.taskindexmulti == 0) {
                $scope.backtaskshowmulti = false;
            }
            else {
                $scope.backtaskshowmulti = true;
            }
            $scope.statusNamemodal = "";
            $scope.tasktitle = "";
            $scope.taskno = "";
            $scope.tasktype = "";
            $scope.tsakpriority = "";
            $scope.priorityswitch = "";
            $scope.prioritypreswitch = "";
            $scope.created_by = "";
            $scope.Rep_assignedto = "";
            $scope.leftflag = "";
            $scope.rep_assignedby = "";
            $scope.rep_assignedDate = "";
            $scope.plancount = "";
            $scope.periodicity = "";
            $scope.Rep_module = "";
            $scope.taskdate = "";
            $scope.taskstatus = "";
            $scope.startdate = "";
            $scope.enddate = "";
            $scope.effhrstemp = "";
            $scope.effmintemp = "";
            $scope.taskdescription = "";
            $scope.ismtcrreS_Response = "";
            $scope.files_paths = "";
            $scope.ismmistS_StatusName = "";
            $scope.taskAttatchment = [];
            $scope.get_taskdetails = [];
            $scope.objupdate.eachTaskMaxDuration = 0;
            $scope.objupdate.durationFlg = "";
            $scope.get_responseattachments = [];
            $scope.taskidmulti = taskobj[0].ISMTCR_Id;
            var data = {
                "ISMTCR_Id": $scope.taskidmulti
            };
            apiService.create("ISM_ComplaintsTaskList/gettaskdetails", data).
                then(function (promise) {
                    $scope.get_taskdetails = promise.get_taskdetails;
                    $scope.get_taskresponse = promise.get_taskresponse;
                    if (promise.role_flag == "Head") {
                        $scope.headornot = true
                    }
                    else {
                        $scope.headornot = false
                    }
                    if ($scope.get_taskdetails !== null && $scope.get_taskdetails.length > 0) {

                        $scope.userhrmeid = promise.hrmE_Id;
                        $scope.createdhrmeid = $scope.get_taskdetails[0].createdhrmeid;
                        $scope.crdprthead = $scope.get_taskdetails[0].crdprthead;
                        $scope.tasktitle = $scope.get_taskdetails[0].ISMTCR_Title;
                        $scope.taskno = $scope.get_taskdetails[0].ISMTCR_TaskNo;
                        $scope.taskclient = $scope.get_taskdetails[0].ISMMCLT_ClientName;
                        $scope.Rep_module = $scope.get_taskdetails[0].IVRMM_ModuleName;
                        $scope.taskdate = $scope.get_taskdetails[0].ISMTCR_CreationDate;
                        $scope.tasktype = $scope.get_taskdetails[0].tasktype;
                        $scope.tsakpriority = $scope.get_taskdetails[0].HRMP_Name;
                        $scope.priorityswitch = $scope.get_taskdetails[0].priorityswitch;
                        $scope.prioritypreswitch = $scope.get_taskdetails[0].preswitch;
                        $scope.taskdescription = $scope.get_taskdetails[0].ISMTCR_Desc;
                        $scope.taskstatus = $scope.get_taskdetails[0].ISMTCR_Status;
                        $scope.createdby = $scope.get_taskdetails[0].createdby;
                        $scope.Rep_assignedto = $scope.get_taskdetails[0].assignedto;
                        $scope.leftflag = $scope.get_taskdetails[0].empleft;
                        $scope.plancount = $scope.get_taskdetails[0].plancount;
                        $scope.periodicity = $scope.get_taskdetails[0].ISMTAPL_Periodicity;
                        $scope.rep_assignedby = $scope.get_taskdetails[0].assignedby;
                        $scope.rep_assignedDate = $scope.get_taskdetails[0].assignedDate;
                        $scope.Responsemodal = taskobj[0].ismtcrreS_Response;
                        $scope.created_by = $scope.get_taskdetails[0].createdby;
                        if ($scope.get_taskdetails[0].ISMTCRASTO_StartDate != null) {
                            $scope.startdate = new Date($scope.get_taskdetails[0].ISMTCRASTO_StartDate).toDateString();
                            $scope.enddate = new Date($scope.get_taskdetails[0].ISMTCRASTO_EndDate).toDateString();
                        }
                        $scope.effort = $scope.get_taskdetails[0].ISMTCRASTO_EffortInHrs;
                        $scope.objupdate.eachTaskMaxDuration = $scope.get_taskdetails[0].ISMTCRASTO_EffortInHrs;
                        $scope.objupdate.durationFlg = "HOURS";
                        var decimaltimetot = $scope.effort;
                        var hrstot = parseInt(Number(decimaltimetot));
                        var mintot = Math.round((Number(decimaltimetot) - hrstot) * 60);
                        $scope.objupdate.effhrs = hrstot;
                        $scope.objupdate.effmin = mintot;
                        if (hrstot.toString().length === 1) {
                            hrstot = '0' + hrstot;
                        }
                        if (mintot.toString().length === 1) {
                            mintot = '0' + mintot;
                        }
                        $scope.effhrstemp = hrstot;
                        $scope.effmintemp = mintot;

                        $scope.tempstatusmulti = [];
                        if ($scope.userhrmeid !== $scope.createdhrmeid) {
                            if ($scope.userhrmeid !== $scope.crdprthead) {
                                angular.forEach($scope.get_Status, function (task) {
                                    if (task.ismmistS_StatusName != "Close" && task.ismmistS_StatusName != "close") {
                                        $scope.tempstatusmulti.push(task);
                                    };
                                });
                            } else {
                                $scope.tempstatusmulti = $scope.get_Status;
                            }

                            if ($scope.get_taskdetails[0].addtoplannerflag === true || $scope.get_taskdetails[0].addtoplannerflag === 1) {
                                $scope.tempstatusmulti = [];
                                angular.forEach($scope.get_Status, function (task) {
                                    if (task.ismmistS_StatusName.toLowerCase() != "close" && task.ismmistS_StatusName.toLowerCase() != "completed" && task.ismmistS_StatusName.toLowerCase() != "development completed") {
                                        $scope.tempstatusmulti.push(task);
                                    };
                                });
                            }
                        }
                        else {
                            $scope.tempstatusmulti = $scope.get_Status;
                        }

                        if ($scope.headornot === false) {
                            for (var i = $scope.tempstatusmulti.length - 1; i >= 0; i--) {
                                if ($scope.tempstatusmulti[i].ismmistS_StatusName.toLowerCase() == "onhold-department" || $scope.tempstatusmulti[i].ismmistS_StatusName.toLowerCase() == "onhold" || $scope.tempstatusmulti[i].ismmistS_StatusName.toLowerCase() == "management blocker") {
                                    $scope.tempstatusmulti.splice(i, 1);
                                }
                            }
                        }
                        $scope.statusNamemodal = taskobj[0].ismmistS_StatusName;

                        $scope.taskAttatchment = promise.get_taskattachments;
                        $scope.get_responseattachments = promise.get_responseattachments;
                        if ($scope.taskAttatchment.length > 0) {
                            angular.forEach($scope.taskAttatchment, function (at) {
                                if (at.ISMTCRAT_Attatchment !== null && at.ISMTCRAT_Attatchment !== "") {
                                    var img = at.ISMTCRAT_Attatchment;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];
                                    at.filetype = lastelement;
                                }
                            });
                        }
                        if ($scope.get_responseattachments.length > 0) {
                            angular.forEach($scope.get_responseattachments, function (at) {
                                if (at.ISMTCRRES_ResponseAttachment !== null && at.ISMTCRRES_ResponseAttachment !== "") {
                                    var img1 = at.ISMTCRRES_ResponseAttachment;
                                    var imagarr1 = img1.split('.');
                                    var lastelement1 = imagarr1[imagarr1.length - 1];

                                    at.filetype = lastelement1;
                                }
                            });
                        }
                        if ($scope.get_taskresponse.length > 0) {
                            if ($scope.get_responseattachments.length > 0) {
                                angular.forEach($scope.get_taskresponse, function (at) {
                                    var slno = 0;
                                    angular.forEach($scope.get_responseattachments, function (atr) {
                                        if (at.ISMTCRRES_Response === atr.ISMTCRRES_Response) {
                                            slno = slno + 1;
                                            atr.slno = slno;
                                        }
                                    });
                                    //var lines = 1;
                                    //lines = at.ISMTCRRES_Response.split(/\r\n|\r|\n/).length;
                                    //at.lines = lines;
                                    at.student_rmrks = at.ISMTCRRES_Response.split("\n");
                                });
                            }
                        }
                        $('#myModalResponsemultistatus').modal('show');
                    }
                    else {
                        $('#myModalResponsemultistatus').modal('hide');
                    }
                });
        };


        $scope.viewresponseclose = function () {       
            $scope.ismtcrreS_Response = "";
            $scope.files_paths = "";
            $scope.ismmistS_StatusName = "";
            $scope.taskAttatchment = [];
            $scope.rep_assignedby = "";
            $scope.created_by = "";
            $scope.Rep_assignedto = "";
            $scope.leftflag = "";
            $scope.rep_assignedDate = "";
            $scope.plancount = "";
            $scope.periodicity = "";
            $scope.get_responseattachments = [];
            var data = {
                "ISMTCR_Id": $scope.taskid
            };
            apiService.create("ISM_ComplaintsTaskList/gettaskdetails", data).
                then(function (promise) {
                    $scope.get_taskdetails = promise.get_taskdetails;
                    $scope.get_taskresponse = promise.get_taskresponse;
                    if ($scope.get_taskdetails !== null && $scope.get_taskdetails.length > 0) {
                        $scope.tasktitle = $scope.get_taskdetails[0].ISMTCR_Title;
                        $scope.taskno = $scope.get_taskdetails[0].ISMTCR_TaskNo;
                        $scope.taskclient = $scope.get_taskdetails[0].ISMMCLT_ClientName;
                        $scope.Rep_module = $scope.get_taskdetails[0].IVRMM_ModuleName;
                        $scope.taskdate = $scope.get_taskdetails[0].ISMTCR_CreationDate;
                        $scope.tasktype = $scope.get_taskdetails[0].tasktype;
                        $scope.tsakpriority = $scope.get_taskdetails[0].HRMP_Name;
                        $scope.priorityswitch = $scope.get_taskdetails[0].priorityswitch;
                        $scope.prioritypreswitch = $scope.get_taskdetails[0].preswitch;
                        $scope.taskdescription = $scope.get_taskdetails[0].ISMTCR_Desc;
                        $scope.taskstatus = $scope.get_taskdetails[0].ISMTCR_Status;
                        $scope.Rep_assignedto = $scope.get_taskdetails[0].assignedto;
                        $scope.leftflag = $scope.get_taskdetails[0].empleft;
                        $scope.plancount = $scope.get_taskdetails[0].plancount;
                        $scope.periodicity = $scope.get_taskdetails[0].ISMTAPL_Periodicity;
                        $scope.rep_assignedby = $scope.get_taskdetails[0].assignedby;
                        $scope.rep_assignedDate = $scope.get_taskdetails[0].assignedDate;
                        $scope.createdby = $scope.get_taskdetails[0].createdby;
                        if ($scope.get_taskdetails[0].ISMTCRASTO_StartDate != null) {
                            $scope.startdate = new Date($scope.get_taskdetails[0].ISMTCRASTO_StartDate).toDateString();
                            $scope.enddate = new Date($scope.get_taskdetails[0].ISMTCRASTO_EndDate).toDateString();
                        }
                        //$scope.startdate = new Date($scope.get_taskdetails[0].ISMTCRASTO_StartDate).toDateString();
                        //$scope.enddate = new Date($scope.get_taskdetails[0].ISMTCRASTO_EndDate).toDateString();
                        $scope.taskAttatchment = promise.get_taskattachments;
                        $scope.get_responseattachments = promise.get_responseattachments;
                        if ($scope.taskAttatchment.length > 0) {
                            angular.forEach($scope.taskAttatchment, function (at) {
                                if (at.ISMTCRAT_Attatchment !== null && at.ISMTCRAT_Attatchment !== "") {
                                    var img = at.ISMTCRAT_Attatchment;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];
                                    at.filetype = lastelement;
                                }
                            });
                        }
                        if ($scope.get_responseattachments.length > 0) {
                            angular.forEach($scope.get_responseattachments, function (at) {
                                if (at.ISMTCRRES_ResponseAttachment !== null && at.ISMTCRRES_ResponseAttachment !== "") {
                                    var img1 = at.ISMTCRRES_ResponseAttachment;
                                    var imagarr1 = img1.split('.');
                                    var lastelement1 = imagarr1[imagarr1.length - 1];

                                    at.filetype = lastelement1;
                                }
                            });
                        }
                        if ($scope.get_taskresponse.length > 0) {
                            if ($scope.get_responseattachments.length > 0) {
                                angular.forEach($scope.get_taskresponse, function (at) {
                                    var slno = 0;
                                    angular.forEach($scope.get_responseattachments, function (atr) {
                                        if (at.ISMTCRRES_Response === atr.ISMTCRRES_Response) {
                                            slno = slno + 1;
                                            atr.slno = slno;
                                        }
                                    });
                                    at.student_rmrks = at.ISMTCRRES_Response.split("\n");
                                });
                            }
                        }
                    }
                });
        };

        //=================== Create
        $scope.responsetask = function (getvalue) {
            if (getvalue === "2") {
                $scope.tasknext = true;
            }
            else {
                $scope.tasknext = false;
            }
            if ($scope.files !== null && $scope.files !== undefined && $scope.files !== "" && $scope.files.length > 0) {
                UploadFiles();
            }
            else {
                if (getvalue === "1") {
                    $scope.delettask = false;
                }
                else {
                    $scope.delettask = true;
                }

                $scope.saveResponse();
            }
        };

        $scope.previewimgclose = function () {
            $('#myModalAddtoPlanner').modal('hide');
            $('.modal-backdrop').remove();
            $('.modal').on('hide.bs.modal', function (e) {
                e.stopPropagation();
                $('body').css('padding-right', '');
            });
        };

        //---View Direct Fies of taks
        $scope.viewdirecttaskfiles = function () {
            if ($scope.taskAttatchment[0].filetype == 'jpg' || $scope.taskAttatchment[0].filetype == 'png') {
                $scope.previewimg($scope.taskAttatchment[0], 0, 1);
            }
            else if ($scope.taskAttatchment[0].filetype == 'pdf') {
                $scope.downloadpdf($scope.taskAttatchment[0], 0, 1);
            }
            else if ($scope.taskAttatchment[0].filetype == 'doc' || $scope.taskAttatchment[0].filetype == 'docx' || $scope.taskAttatchment[0].filetype == 'ppt' || $scope.taskAttatchment[0].filetype == 'pptx') {
                $scope.viewdox($scope.taskAttatchment[0], 0, 1);
            }
            else if ($scope.taskAttatchment[0].filetype == 'xlsx' || $scope.taskAttatchment[0].filetype == 'xls' || $scope.taskAttatchment[0].filetype == 'xlx') {
                $scope.viewdox($scope.taskAttatchment[0], 0, 1);
            }
            else if ($scope.taskAttatchment[0].filetype == 'wmv' || $scope.taskAttatchment[0].filetype == 'mp4') {
                $scope.viewvideo($scope.taskAttatchment[0], 0, 1);
            }
            else if ($scope.taskAttatchment[0].filetype == 'mp3') {
                $scope.viewaudio($scope.taskAttatchment[0], 0, 1);
            }
        };

        //=========== View Response Image
        $scope.previewimg = function (data, ind, typ) {
            $scope.docindex = ind;
            $scope.showtype = typ;
            $('#showpdf').modal('hide');
            $('#myModal2').modal('hide');
            $('#myModaldocx').modal('hide');
            $('#myvideoPreview').modal('hide');
            $('#myaudioPreview').modal('hide');
            $scope.attachmentsshow = [];
            var docpath = "";
            if (typ == 1) {
                $scope.attachmentsshow = $scope.taskAttatchment;
            }
            else if (typ == 2) {
                $scope.attachmentsshow = $scope.get_responseattachments;
            }


            if (data === undefined || data === null) {
                if ($scope.docindex > $scope.attachmentsshow.length - 1) {
                    $scope.docindex = 0;
                }
                if (typ == 1) {
                    docpath = $scope.attachmentsshow[$scope.docindex].ISMTCRAT_Attatchment;
                }
                else if (typ == 2) {
                    docpath = $scope.attachmentsshow[$scope.docindex].ISMTCRRES_ResponseAttachment;
                }


                var namedoc = "";
                if (docpath != null && docpath != undefined) {
                    $scope.docname = namedoc;
                    $scope.docnameno = $scope.docindex + 1;
                    $('#preview').attr('src', docpath);
                    $('#myModalResponse').modal('hide');

                    $('.modal-backdrop').remove();
                    $('#myModalPreview').modal('show');
                    $('.modal').on('hide.bs.modal', function (e) {
                        e.stopPropagation();
                        $('body').css('padding-right', '');
                    });
                }
            } else {
                if (typ == 1) {
                    $scope.docname = data.ISMTCRAT_Attatchment;
                }
                else if (typ == 2) {
                    $scope.docname = data.ISMTCRRES_ResponseAttachment;
                    angular.forEach($scope.attachmentsshow, function (pri) {
                        if (pri.ISMTCRRES_Id === data.ISMTCRRES_Id) {
                            $scope.docname = pri.ISMTCRRES_ResponseAttachment;
                        }
                    });
                }
                $scope.docnameno = $scope.docindex + 1;
                $('#preview').attr('src', $scope.docname);
                $('#myModalResponse').modal('hide');

                $('.modal-backdrop').remove();
                $('#myModalPreview').modal('show');
                $('.modal').on('hide.bs.modal', function (e) {
                    e.stopPropagation();
                    $('body').css('padding-right', '');
                });
            }
            if ($scope.docindex <= $scope.attachmentsshow.length - 1) {
                $scope.shownext = true;
            }
            else {
                $scope.shownext = false;
            }
            if (typ == 2) {
                $scope.shownext = false;
            }
        };
        //=========== Close Image Model
        $scope.closeimg = function () {
            $('#myModalPreview').modal('hide');
            $('.modal-backdrop').remove();
            $scope.viewresponseclose();
        };
        $scope.closepdf = function () {
            $('#showpdf').modal('hide');
            $('.modal-backdrop').remove();
            $scope.viewresponseclose();
        };
        $scope.closedox = function () {
            $('#myModaldocx').modal('hide');
            $('.modal-backdrop').remove();
            $scope.viewresponseclose();
        };
        $scope.closevideo = function () {
            $('#myvideoPreview').modal('hide');
            $('.modal-backdrop').remove();
            $scope.viewresponseclose();
        };
        $scope.closeaudio = function () {
            $('#myaudioPreview').modal('hide');
            $('.modal-backdrop').remove();
            $scope.viewresponseclose();
        };
        $scope.downloadpdf = function (data, ind, typ) {
            $scope.docindex = ind;
            $scope.showtype = typ;
            $('#myModalResponse').modal('hide');
            $('#myModalPreview').modal('hide');
            $('#myModaldocx').modal('hide');
            $('#myvideoPreview').modal('hide');
            $('#myaudioPreview').modal('hide');
            var imagedownload1 = "";
            $scope.attachmentsshow = [];
            var docpath = "";
            if (typ == 1) {
                $scope.attachmentsshow = $scope.taskAttatchment;
            }
            else if (typ == 2) {
                $scope.attachmentsshow = $scope.get_responseattachments;
            }

            if (data === undefined || data === null) {
                if ($scope.docindex > $scope.attachmentsshow.length - 1) {
                    $scope.docindex = 0;
                }
            }
            //if ($scope.docname === "") {
            //    $scope.docindex = 0;
            //}
            if (typ == 1) {
                docpath = $scope.attachmentsshow[$scope.docindex].ISMTCRAT_Attatchment;
            }
            else if (typ == 2) {
                angular.forEach($scope.attachmentsshow, function (pri) {
                    if (pri.ISMTCRRES_Id === data.ISMTCRRES_Id) {
                        docpath = pri.ISMTCRRES_ResponseAttachment;
                    }
                });
            }
            var namedoc = "";
            if (docpath != null && docpath != undefined) {
                $scope.docname = namedoc;
                $scope.docnameno = $scope.docindex + 1;
                imagedownload1 = docpath;
            }

            $http.get(imagedownload1, { responseType: 'arraybuffer' })
                .success(function (response) {
                    var fileURL = "";
                    var file = "";
                    var embed = "";
                    var pdfId = "";
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);

                    pdfId = document.getElementById("pdfIdzz");
                    pdfId.removeChild(pdfId.childNodes[0]);
                    embed = document.createElement('embed');
                    embed.setAttribute('src', fileURL);
                    embed.setAttribute('type', 'application/pdf');
                    embed.setAttribute('width', '100%');
                    embed.setAttribute('height', '1000');
                    pdfId.appendChild(embed);
                    $('#showpdf').modal('show');
                    $('.modal').on('hide.bs.modal', function (e) {
                        e.stopPropagation();
                        $('body').css('padding-right', '');
                    });
                });

            if ($scope.docindex <= $scope.attachmentsshow.length - 1) {
                $scope.shownext = true;
            }
            else {
                $scope.shownext = false;
            }
            if (typ == 2) {
                $scope.shownext = false;
            }
        }

        $scope.nexttask = function () {
            $scope.taskindex = $scope.taskindex + 1;
            $scope.nexttaskobj.ISMTCR_Id = $scope.filterValue[$scope.taskindex].ISMTCR_Id;
            $scope.nexttaskobj.assignedto = $scope.filterValue[$scope.taskindex].assignedto;
            $scope.nexttaskobj.checkedvalue = $scope.filterValue[$scope.taskindex].checkedvalue;
            $scope.viewresponse($scope.nexttaskobj, '', $scope.taskindex);
        };
        $scope.backtask = function () {
            $scope.taskindex = $scope.taskindex - 1;
            $scope.nexttaskobj.ISMTCR_Id = $scope.filterValue[$scope.taskindex].ISMTCR_Id;
            $scope.nexttaskobj.assignedto = $scope.filterValue[$scope.taskindex].assignedto;
            $scope.nexttaskobj.checkedvalue = $scope.filterValue[$scope.taskindex].checkedvalue;
            $scope.viewresponse($scope.nexttaskobj, '', $scope.taskindex);
        };

        $scope.nexttaskmulti = function () {
            $scope.updatestatus();
            $scope.taskindexmulti = $scope.taskindexmulti + 1;
            $scope.nexttaskobjm.ISMTCR_Id = $scope.taskupadtelist[$scope.taskindexmulti].ISMTCR_Id;
            $scope.nexttaskobjm.assignedto = $scope.taskupadtelist[$scope.taskindexmulti].assignedto;
            $scope.nexttaskobjm.checkedvalue = $scope.taskupadtelist[$scope.taskindexmulti].checkedvalue;
            $scope.viewresponsemultiple($scope.nexttaskobjm, '', $scope.taskindexmulti);
        };
        $scope.backtaskmulti = function () {
            $scope.updatestatus();
            $scope.taskindexmulti = $scope.taskindexmulti - 1;
            $scope.nexttaskobjm.ISMTCR_Id = $scope.taskupadtelist[$scope.taskindexmulti].ISMTCR_Id;
            $scope.nexttaskobjm.assignedto = $scope.taskupadtelist[$scope.taskindexmulti].assignedto;
            $scope.nexttaskobjm.checkedvalue = $scope.taskupadtelist[$scope.taskindexmulti].checkedvalue;
            $scope.viewresponsemultiple($scope.nexttaskobjm, '', $scope.taskindexmulti);
        };

        $scope.nextdoc = function () {
            var docfile = "";
            $scope.attachmentsshow = [];
            $scope.docindex = $scope.docindex + 1;

            var typ = $scope.showtype;

            if (typ == 1) {
                $scope.attachmentsshow = $scope.taskAttatchment;
            }
            else if (typ == 2) {
                $scope.attachmentsshow = $scope.get_responseattachments;
                $scope.shownext = false;
            }


            if ($scope.docindex <= $scope.attachmentsshow.length - 1) {
                docfile = $scope.attachmentsshow[$scope.docindex].filetype;
            }
            else {
                $scope.docindex = 0;
                docfile = $scope.attachmentsshow[$scope.docindex].filetype;
            }
            if (docfile == 'jpg' || docfile == 'png' || docfile == 'jpeg') {
                $scope.previewimg(null, $scope.docindex, typ);
            }
            else if (docfile == 'pdf') {
                $scope.downloadpdf(null, $scope.docindex, typ);
            }
            else if (docfile === 'doc' || docfile === 'docx' || docfile === 'ppt' || docfile === 'pptx' || docfile === 'xlsx' || docfile === 'xls' || docfile === 'xlx') {
                $scope.viewdox(null, $scope.docindex, typ);
                //dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.cfilepath;
            }
            if (docfile == 'wmv' || docfile == 'mp4') {
                $scope.viewvideo(null, $scope.docindex, typ);
            }
            else if (docfile == 'mp3') {
                $scope.viewaudio(null, $scope.docindex, typ);
            }
        };

        $scope.viewdox = function (data, ind, typ) {
            $scope.docindex = ind;
            $scope.showtype = typ;
            $('#myModalResponse').modal('hide');
            $('#myModalPreview').modal('hide');
            $('#showpdf').modal('hide')
            $('#myvideoPreview').modal('hide');
            $('#myaudioPreview').modal('hide');
            var imagedownload1 = "";
            $scope.attachmentsshow = [];
            var docpath = "";
            if (typ == 1) {
                $scope.attachmentsshow = $scope.taskAttatchment;
            }
            else if (typ == 2) {
                $scope.attachmentsshow = $scope.get_responseattachments;
            }
            if (data === undefined || data === null) {
                if ($scope.docindex > $scope.attachmentsshow.length - 1) {
                    $scope.docindex = 0;
                }
            }
            if (typ == 1) {
                docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.attachmentsshow[$scope.docindex].ISMTCRAT_Attatchment;
            }
            else if (typ == 2) {

                angular.forEach($scope.attachmentsshow, function (pri) {
                    if (pri.ISMTCRRES_Id === data.ISMTCRRES_Id) {
                        docpath = pri.ISMTCRRES_ResponseAttachment;
                    }
                });
                docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + docpath;
            }

            var namedoc = "";
            if (docpath != null && docpath != undefined) {
                $scope.docname = namedoc;
                $scope.docnameno = $scope.docindex + 1;
                $scope.imagedownload1 = docpath;
            }

            $scope.detailFrame = $sce.trustAsResourceUrl(docpath);

            $('#myModaldocx').modal('show');
            $('.modal').on('hide.bs.modal', function (e) {
                e.stopPropagation();
                $('body').css('padding-right', '');
            });

            if ($scope.docindex <= $scope.attachmentsshow.length - 1) {
                $scope.shownext = true;
            }
            else {
                $scope.shownext = false;
            }
            if (typ == 2) {
                $scope.shownext = false;
            }
        }

        $scope.viewvideo = function (data, ind, typ) {
            $scope.docindex = ind;
            $scope.showtype = typ;
            $('#myModalResponse').modal('hide');
            $('#myModalPreview').modal('hide');
            $('#showpdf').modal('hide');
            $('#myModaldocx').modal('hide');
            $('#myaudioPreview').modal('hide');
            $scope.view_videos = [];
            var imagedownload1 = "";
            $scope.attachmentsshow = [];
            var docpath = "";
            if (typ == 1) {
                $scope.attachmentsshow = $scope.taskAttatchment;
            }
            else if (typ == 2) {
                $scope.attachmentsshow = $scope.get_responseattachments;
            }
            if (data === undefined || data === null) {
                if ($scope.docindex > $scope.attachmentsshow.length - 1) {
                    $scope.docindex = 0;
                }
            }
            if (typ == 1) {
                docpath = $scope.attachmentsshow[$scope.docindex].ISMTCRAT_Attatchment;
            }
            else if (typ == 2) {
                angular.forEach($scope.attachmentsshow, function (pri) {
                    if (pri.ISMTCRRES_Id === data.ISMTCRRES_Id) {
                        docpath = pri.ISMTCRRES_ResponseAttachment;
                    }
                });
            }
            var namedoc = "";
            if (docpath != null && docpath != undefined) {
                $scope.docname = namedoc;
                $scope.docnameno = $scope.docindex + 1;
                $scope.urlvideo = docpath;
            }
            $scope.view_videos.push({ id: 1, ihw_video: docpath });
            $('#myvideoPreview').modal('show');
            $('.modal').on('hide.bs.modal', function (e) {
                e.stopPropagation();
                $('body').css('padding-right', '');
            });

            if ($scope.docindex <= $scope.attachmentsshow.length - 1) {
                $scope.shownext = true;
            }
            else {
                $scope.shownext = false;
            }
            if (typ == 2) {
                $scope.shownext = false;
            }
        }

        $scope.viewaudio = function (data, ind, typ) {
            $scope.docindex = ind;
            $scope.showtype = typ;
            $('#myModalResponse').modal('hide');
            $('#myModalPreview').modal('hide');
            $('#showpdf').modal('hide');
            $('#myModaldocx').modal('hide');
            $('#myvideoPreview').modal('hide');
            $scope.view_videos = [];
            var imagedownload1 = "";
            $scope.attachmentsshow = [];
            var docpath = "";
            if (typ == 1) {
                $scope.attachmentsshow = $scope.taskAttatchment;
            }
            else if (typ == 2) {
                $scope.attachmentsshow = $scope.get_responseattachments;

            }
            if (data === undefined || data === null) {
                if ($scope.docindex > $scope.attachmentsshow.length - 1) {
                    $scope.docindex = 0;
                }
            }
            if (typ == 1) {
                docpath = $scope.attachmentsshow[$scope.docindex].ISMTCRAT_Attatchment;
            }
            else if (typ == 2) {
                angular.forEach($scope.attachmentsshow, function (pri) {
                    if (pri.ISMTCRRES_Id === data.ISMTCRRES_Id) {
                        docpath = pri.ISMTCRRES_ResponseAttachment;
                    }
                });
            }
            var namedoc = "";
            if (docpath != null && docpath != undefined) {
                $scope.docname = namedoc;
                $scope.docnameno = $scope.docindex + 1;
                $scope.urlvideo = docpath;
            }
            $scope.view_videos.push({ id: 1, ihw_video: docpath });
            $('#myaudioPreview').modal('show');
            $('.modal').on('hide.bs.modal', function (e) {
                e.stopPropagation();
                $('body').css('padding-right', '');
            });

            if ($scope.docindex <= $scope.attachmentsshow.length - 1) {
                $scope.shownext = true;
            }
            else {
                $scope.shownext = false;
            }
            if (typ == 2) {
                $scope.shownext = false;
            }
        }

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };
        //shilpa
        $scope.clearids = function () {
            // $scope.HRMD_Id = '';
            $scope.selected = "";
            $scope.arrlistchk = [];
            $scope.label = '';
            // $scope.arrlistchk = [];
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };
        //===========Clear Field
        $scope.cancel = function () {
            $state.reload();
        };
    }
    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });
    angular.module('app').filter('dateRange', function () {
        var formatStr = 'DD/MM/YYYY';
        return function (produceList, fromDate, toDate, type) {
            if (fromDate && toDate) {
                var filtered = [];
                var fltfromdate = new Date(fromDate);
                fltfromdate = fltfromdate.setDate(fltfromdate.getDate() - 1);
                var flttodate = new Date(toDate);
                flttodate = flttodate.setDate(flttodate.getDate() + 1);
                angular.forEach(produceList, function (items) {
                    if (items.assignedto == "Not Assign" || items.assignedto == "Not - Assigned" || items.assignedto == "Not-Assigned") {
                        var flttaskdate = new Date(items.crdate);
                        if (flttaskdate >= fltfromdate && flttaskdate <= flttodate)
                            filtered.push(items);
                    }
                    else {
                        if (type == "C") {
                            var flttaskdate = new Date(items.crdate);
                            if (flttaskdate >= fltfromdate && flttaskdate <= flttodate)
                                filtered.push(items);
                        } else {
                            var flttaskdate = new Date(items.strdate);
                            var flttaskdateto = new Date(items.flttodate);
                            if ((flttaskdate >= fltfromdate && flttaskdate <= flttodate) && (flttaskdateto >= fltfromdate && flttaskdateto <= flttodate))
                                filtered.push(items);
                        }
                    }
                });
                return filtered;
            }
            else
                return produceList;
        }
    })

    angular.module('app').filter('excludeNonMembers', function ($filter) {

        return function (produceList, exludevalue) {
            if (exludevalue) {
                var filtered = [];
                var ceatedbyarray = [];
                var assigntoarray = [];
                var title = "";
                var clientname = "";
                var modname = "";
                var ceatedby = "";
                var assignedby = "";
                var assignedto = "";
                if (exludevalue.ISMTCR_Title) {
                    title = exludevalue.ISMTCR_Title;
                }
                if (exludevalue.ISMMCLT_ClientName) {
                    clientname = exludevalue.ISMMCLT_ClientName;
                }
                if (exludevalue.IVRMM_ModuleName) {
                    modname = exludevalue.IVRMM_ModuleName;
                }
                if (exludevalue.createdby) {
                    ceatedby = exludevalue.createdby;
                }

                if (exludevalue.assignedby != undefined) {
                    assignedby = exludevalue.assignedby;
                }
                else {
                    assignedby = "";
                }
                if (exludevalue.assignedto != undefined) {
                    assignedto = exludevalue.assignedto;
                }
                else {
                    assignedto = "";
                }

                if (ceatedby != '') {
                    ceatedbyarray = ceatedby.split(',');
                }
                if (assignedto != '') {
                    assigntoarray = assignedto.split(',');
                }



                var filtered = $filter('filter')(produceList, function (d) {
                    if (title != null && title != "") {
                        return d.ISMTCR_Title.toLowerCase() !== title.toLowerCase();
                    }
                    else {
                        return d.ISMTCR_Title.toLowerCase() !== title;
                    }

                });
                var filtered = $filter('filter')(filtered, function (d) {
                    //if (clientname != null && clientname != "") {
                    //    return d.ISMMCLT_ClientName.toLowerCase() !== clientname.toLowerCase();
                    //}
                    //else {
                    //    return d.ISMMCLT_ClientName.toLowerCase() !== clientname;
                    //}
                    if (d.ISMMCLT_ClientName != null && d.ISMMCLT_ClientName != "") {
                        if (clientname != null && clientname != "") {
                            return d.ISMMCLT_ClientName.toLowerCase() !== clientname.toLowerCase();
                        }
                        else {
                            return d.ISMMCLT_ClientName.toLowerCase() !== clientname;
                        }
                    }
                    else {
                        return d;
                    }

                });
                var filtered = $filter('filter')(filtered, function (d) {
                    if (d.IVRMM_ModuleName != null && d.IVRMM_ModuleName != "") {
                        if (modname != null && modname != "") {
                            return d.IVRMM_ModuleName.toLowerCase() !== modname.toLowerCase();
                        }
                        else {
                            return d.IVRMM_ModuleName.toLowerCase() !== modname;
                        }
                    }
                    else {
                        return d;
                    }
                });
                //var filtered = $filter('filter')(filtered, function (d) {
                //    if (ceatedby != null && ceatedby != "") {
                //        return d.createdby.toLowerCase() !== ceatedby.toLowerCase();
                //    }
                //    else {
                //        return d.createdby.toLowerCase() !== ceatedby;
                //    }
                //});

                for (var i = 0; i < ceatedbyarray.length; i++) {
                    var filtered = $filter('filter')(filtered, function (d) {
                        if (d.createdby != null && d.createdby != "") {
                            if (ceatedby != null && ceatedby != "") {
                                return d.createdby.toLowerCase() !== ceatedbyarray[i].toLowerCase();
                            }
                            else {
                                return d.createdby.toLowerCase() !== ceatedbyarray[i];
                            }
                        }
                        else {
                            return d;
                        }
                    });
                }

                var filtered = $filter('filter')(filtered, function (d) {
                    if (d.assignedby != null && d.assignedby != "") {
                        if (assignedby != null && assignedby != "") {
                            return d.assignedby.toLowerCase() !== assignedby.toLowerCase();
                        }
                        else {
                            return d.assignedby.toLowerCase() !== assignedby;
                        }
                    }
                    else {
                        return d;
                    }
                });
                //var filtered = $filter('filter')(filtered, function (d) {
                //    if (assignedto != null && assignedto != "") {
                //        return d.assignedto.toLowerCase() !== assignedto.toLowerCase();
                //    }
                //    else {
                //        return d.assignedto.toLowerCase() !== assignedto;
                //    }
                //});
                for (var i = 0; i < assigntoarray.length; i++) {
                    var filtered = $filter('filter')(filtered, function (d) {
                        if (d.assignedto != null && d.assignedto != "") {
                            if (assignedto != null && assignedto != "") {
                                return d.assignedto.toLowerCase() !== assigntoarray[i].toLowerCase();
                            }
                            else {
                                return d.assignedto.toLowerCase() !== assigntoarray[i];
                            }
                        }
                        else {
                            return d;
                        }
                    });
                }

                return filtered;
            }
            else
                return produceList;
        }
    })
        
})();

