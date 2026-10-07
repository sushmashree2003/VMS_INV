    (function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_ComplaintsTaskListController', ISM_ComplaintsTaskListController);
        ISM_ComplaintsTaskListController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', 'superCache', '$filter', '$sce', 'intracontroller'];
        function ISM_ComplaintsTaskListController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, Excel, $timeout, superCache, $filter, $sce, intracontroller) {

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
            apiService.create("ISM_ComplaintsTaskList/getdetails", data).then(function (promise) {
                $scope.get_comptasklist_all = promise.get_comptasklist;
                if (promise.roletype === "Head") {
                    $scope.headtype = true;
                };
                $scope.tablename = "My Pending";
                $scope.get_comptasklist = promise.getmy_comptasklist;
                $scope.get_priority = promise.get_priority;
                $scope.get_prioritynew = promise.get_priority;
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
               
                $scope.get_tasks_completed = promise.get_tasks_completed;
                if ($scope.get_tasks_completed != null && $scope.get_tasks_completed.length > 0) {
                    $scope.compltetedcounttask = $scope.get_tasks_completed[0].completdcount;
                }

                $scope.hrmeid = promise.hrmE_Id,
                    $scope.arrlistchk1 = promise.arrlistchk;

                if ($scope.get_comptasklist.length === 0) {
                    $scope.showactive = false;
                    $scope.activeclass2 = "activet";
                    $scope.activeclass1 = "";
                    $scope.get_comptasklist = $scope.get_comptasklist_all;
                    $scope.tablename = "All Pending";
                };

                if (promise.completedcreatedbyme != null && promise.completedcreatedbyme.length > 0) {
                    $scope.compbyme = promise.completedcreatedbyme[0].ISMEDWTCC_TaskCount;
                     
                }
                else {
                    $scope.compbyme = 0;
                }
                if ($scope.compbyme > 0) {
                    intracontroller.viewcompleted = true;
                }

                if (promise.rejected_comptasklist != null && promise.rejected_comptasklist.length > 0) {
                    $scope.rejected_comptasklist = promise.rejected_comptasklist.length;
                }
                else {
                    $scope.rejected_comptasklist = 0;
                }
                if ($scope.get_comptasklist_all != null && $scope.get_comptasklist_all.length > 0) {
                    if ($scope.get_comptasklist.length > 0) {
                        $scope.pendingcount = $scope.get_comptasklist_all[0].Count;
                    }
                    else {
                        $scope.pendingcount = $scope.get_comptasklist_all.length;
                    }
                    $scope.get_comptasklistfilter = $scope.get_comptasklist;
                }
                else {
                    $scope.assignedcount = 0;
                    $scope.notassignedcount = 0;
                    $scope.pendingcount = 0;
                    $scope.transfercount = 0;
                }
                $scope.get_Status = promise.get_Status;
                //shilpa
                $scope.get_employeelist = promise.get_employeelist;
                $scope.get_userEmplist = promise.get_userEmplist;
                $scope.get_userEmplistfilter = promise.get_userEmplist;
                var filteredemployees = $filter('filter')($scope.get_userEmplistfilter, function (d) {
                    return d.HRME_LeftFlag === false && d.HRME_ActiveFlag === true;
                });
                $scope.get_userEmplist = filteredemployees;

                $scope.get_department = promise.get_department;
              
                if (promise.get_tasks !== null && promise.get_tasks.length > 0) {
                    $scope.get_tasks_count = promise.get_tasks.length;
                }
                else {
                    $scope.get_tasks_count = 0;
                }
                if (promise.get_pendingcount !== null && promise.get_pendingcount.length > 0) {
                    $scope.get_pendingcount = promise.get_pendingcount.length;
                    $scope.get_pendingcountNew = promise.get_pendingcount;
                }
                else {
                    $scope.get_pendingcount = 0;
                }
                $scope.RejectedTaskNew = [];
                if (promise.get_comptasklist !== null && promise.get_comptasklist.length > 0) {
                    $scope.RejectedTaskcount = promise.get_comptasklist.length;
                    $scope.RejectedTaskNew = promise.get_comptasklist;
                }
                //else {
                //    $scope.get_pendingcount = 0;
                //}
                $scope.assginedbyme = promise.byMecount;
                $scope.assginedtoMe = promise.toMecount;
                $scope.mypendingcount = promise.mycountonly;
                $scope.transfertoMe = promise.transferMecount;
                $scope.transferbyMe = promise.transferbycount;
                $scope.totaltask = promise.byMecount + promise.transferMecount + promise.toMecount;

                $scope.excludecreateduserlist();
                $scope.excludeassignuserlist();

                //if (intracontroller.taskno != "") {
                //    $scope.obj.ismtcRTitle = intracontroller.taskno;
                //    $scope.searchtasksdetails();
                //}
                var url_string = window.location.href;
                var paramString = url_string.split('?')[1];
                var queryString = new URLSearchParams(paramString);
                var taskno = queryString.get("taskno");
                var taskvalue = queryString.get("taskid");
                if (taskno != null) {
                    intracontroller.ismtcRTitle = taskno;
                    $scope.obj.ismtcRTitle = taskno;
                    intracontroller.ISMTCR_Id = parseInt(Number(taskvalue));
                    $scope.searchtasksdetails();
                }
               
                
                if (intracontroller.viewcompleted === true) {
                    $scope.activeclass2 = "";
                    $scope.activeclass1 = "";
                    $scope.activeclass3 = "activet";
                    $scope.activeclass4 = "";
                    $scope.getpendingclose();
                }
                if (intracontroller.viewnotassigned === true) { //added by adarsh
                    $scope.activeclass2 = "";
                    $scope.activeclass1 = "";
                    $scope.activeclass3 = "";
                    $scope.activeclass4 = "activet";
                    $scope.getnotassingcount();
                }
                 if (intracontroller.viewnotapproved === true) { //added by adarsh
                    $scope.activeclass2 = "";
                    $scope.activeclass1 = "";
                    $scope.activeclass3 = "";
                    $scope.activeclass4 = "";
                    $scope.activeclassnot = "activet";
                    $scope.getscountsApproved();
                }
                if ($scope.RejectedTaskNew != null && $scope.RejectedTaskNew.length > 0 && intracontroller.notcompleted === true ) {                   
                    $scope.getscountsApproved();
                }
                
            });
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;
                $scope.reverse = !$scope.reverse;
            };
  
        };
       
        $scope.excludecreateduserlist = function () {
            $scope.excludecreated = [];
            var a = [];
            angular.forEach($scope.get_comptasklist, function (value) {
                a.push(value.createdby);
            });

            var excludecreated = a.filter(function (value, index) { return a.indexOf(value) == index });

            var keepGoingc = true;
            angular.forEach(excludecreated, function (value2) {
                keepGoingc = true;
                angular.forEach($scope.get_comptasklist, function (value1) {
                    if (keepGoingc) {
                        if (value1.createdby == value2) {
                            keepGoingc = false;
                            $scope.excludecreated.push({
                                createdby: value2, leftflag: value1.crleft, showcross: false, creclass: 'cretaed'
                            });
                            return;
                        };
                    }
                });
            });

            //if (excludecreated.length > 0) {
            //    angular.forEach(excludecreated, function (value) {
            //        $scope.excludecreated.push({
            //            createdby: value, showcross: false, creclass: 'cretaed'
            //        });
            //    });
            //}
        }
        $scope.excludeassignuserlist = function () {
            $scope.excludeassign = [];
            var b = [];
            angular.forEach($scope.get_comptasklist, function (value) {
                b.push(value.assignedto);
            });

            var excludeassigntemp = b.filter(function (value, index) { return b.indexOf(value) == index });

            var keepGoing = true;
            angular.forEach(excludeassigntemp, function (value2) {
                keepGoing = true;
                angular.forEach($scope.get_comptasklist, function (value1) {
                    if (keepGoing) {
                        if (value1.assignedto == value2) {
                            keepGoing = false;
                            $scope.excludeassign.push({
                                assignedto: value2, leftflag: value1.empleft, showcross: false, creclass: 'cretaed'
                            });
                            return;
                        };
                    }
                });
            });

            //if ($scope.excludeassign.length > 0) {
            //    angular.forEach(excludeassign, function (value) {
            //        $scope.excludeassign.push({
            //            assignedto: value, showcross: false, creclass: 'cretaed', leftflag: value.leftflag
            //        });
            //    });
            //}
        }

        $scope.addexludecreated = function (name) {
            angular.forEach($scope.excludecreated, function (value) {
                if (name == value.createdby) {
                    value.showcross = true;
                    if (value.leftflag) {
                        value.creclass = 'createdactiveleft';
                    } else {
                        value.creclass = 'createdactive';
                    }

                    if ($scope.excludesearch.createdby == "") {
                        $scope.excludesearch.createdby = name;
                    } else {
                        $scope.excludesearch.createdby = $scope.excludesearch.createdby + ',' + name;
                    }
                }
            });
        };
        $scope.addexludeassign = function (name) {
            angular.forEach($scope.excludeassign, function (value) {
                if (name == value.assignedto) {
                    value.showcross = true;
                    if (value.leftflag) {
                        value.creclass = 'createdactiveleft';
                    } else {
                        value.creclass = 'createdactive';
                    }
                    if ($scope.excludesearch.assignedto == "") {
                        $scope.excludesearch.assignedto = name;
                    } else {
                        $scope.excludesearch.assignedto = $scope.excludesearch.assignedto + ',' + name;
                    }
                }
            });
        };

        $scope.removeexludecreated = function (name) {
            angular.forEach($scope.excludecreated, function (value) {
                if (name == value.createdby) {
                    value.showcross = false;
                    value.creclass = 'cretaed';
                    var ceatedbyarr = $scope.excludesearch.createdby.split(',');
                    var filterceatedbyarr = $filter('filter')(ceatedbyarr, function (d) {
                        return d !== name;
                    });
                    var filtr = "";
                    angular.forEach(filterceatedbyarr, function (value) {
                        if (filtr == "") {
                            filtr = value;
                        }
                        else {
                            filtr = filtr + ',' + value;
                        }
                    });
                    $scope.excludesearch.createdby = filtr;
                }
            });
        };
        $scope.removeexludeassign = function (name) {
            angular.forEach($scope.excludeassign, function (value) {
                if (name == value.assignedto) {
                    value.showcross = false;
                    value.creclass = 'cretaed';
                    var assigntoarr = $scope.excludesearch.assignedto.split(',');
                    var filterassignbyarr = $filter('filter')(assigntoarr, function (d) {
                        return d !== name;
                    });
                    var filtr = "";
                    angular.forEach(filterassignbyarr, function (value) {
                        if (filtr == "") {
                            filtr = value;
                        }
                        else {
                            filtr = filtr + ',' + value;
                        }
                    });
                    $scope.excludesearch.assignedto = filtr;
                }
            });
        };

        $scope.lodarefresh = function () {
            $scope.currentPage = 1;
            $scope.typeFlg = "Default";
            var data = {
                "typeFlg": $scope.typeFlg
            };
            apiService.create("ISM_ComplaintsTaskList/getdetails", data).then(function (promise) {
                $scope.get_comptasklist_all = promise.get_comptasklist;
                if (promise.roletype === "Head") {
                    $scope.headtype = true;
                };
                $scope.get_comptasklist = promise.getmy_comptasklist;
                $scope.get_priority = promise.get_priority;
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
                $scope.hrmeid = promise.hrmE_Id,
                    $scope.arrlistchk1 = promise.arrlistchk;
                if ($scope.get_comptasklist.length === 0) {
                    $scope.showactive = false;
                    $scope.activeclass2 = "activet";
                    $scope.activeclass1 = "";
                    $scope.get_comptasklist = $scope.get_comptasklist_all;
                };
                if ($scope.get_comptasklist_all != null && $scope.get_comptasklist_all.length > 0) {
                    var notassignedcount = 0;
                    var assignedcount = 0;
                    var pendingtaskcount = 0;
                    var transfertaskcount = 0;
                    $scope.pendingcount = $scope.get_comptasklist_all.length;
                }
                else {
                    $scope.assignedcount = 0;
                    $scope.notassignedcount = 0;
                    $scope.pendingcount = 0;
                    $scope.transfercount = 0;
                }
                $scope.get_Status = promise.get_Status;
                $scope.get_employeelist = promise.get_employeelist;
                $scope.get_userEmplist = promise.get_userEmplist;
                $scope.get_userEmplistfilter = promise.get_userEmplist;
                var filteredemployees = $filter('filter')($scope.get_userEmplistfilter, function (d) {
                    return d.HRME_LeftFlag === false && d.HRME_ActiveFlag === true;
                });
                $scope.get_userEmplist = filteredemployees;

                $scope.get_department = promise.get_department;
                if (promise.get_tasks !== null && promise.get_tasks.length > 0) {
                    $scope.get_tasks_count = promise.get_tasks.length;
                }
                else {
                    $scope.get_tasks_count = 0;
                }
                $scope.assginedbyme = promise.byMecount;
                $scope.assginedtoMe = promise.toMecount;
                $scope.mypendingcount = promise.mycountonly;
                $scope.transfertoMe = promise.transferMecount;
                $scope.transferbyMe = promise.transferbycount;
                $scope.totaltask = promise.byMecount + promise.transferMecount + promise.toMecount;
                $scope.getnotassingcount();
                $scope.get_comptasklistfilter = $scope.get_comptasklist;
            });
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;
                $scope.reverse = !$scope.reverse;
            };
        };

            

        $scope.addleftemployee = function (leftvalue) {
            if (leftvalue == true) {
                $scope.get_userEmplist = $scope.get_userEmplistfilter;
            }
            else {
                var filteredemployees = $filter('filter')($scope.get_userEmplistfilter, function (d) {
                    return d.HRME_LeftFlag === leftvalue && d.HRME_ActiveFlag !== leftvalue;
                });
                $scope.get_userEmplist = filteredemployees;
            }
            $scope.searchValueUEM = '';
        };

        $scope.advexcludecreate = function (check) {
            $scope.eacreateshow = check;
            $scope.excludesearch.createdby = '';
            if ($scope.excludecreated.length > 0) {
                angular.forEach($scope.excludecreated, function (value) {
                    value.creclass = 'cretaed';
                    value.showcross = false;
                });
            }
        };
        $scope.advexcludecreateall = function (check) {
            $scope.eacreateallleft = check;
            if (check == true) {
                $scope.excludesearch.createdby = "";
                angular.forEach($scope.excludecreated, function (value) {
                    value.showcross = true;
                    if (value.leftflag) {
                        value.creclass = 'createdactiveleft';
                    } else {
                        value.creclass = 'createdactive';
                    }
                    if ($scope.excludesearch.createdby == "") {
                        $scope.excludesearch.createdby = value.createdby;
                    } else {
                        $scope.excludesearch.createdby = $scope.excludesearch.createdby + ',' + value.createdby;
                    }
                });
            }
            else {
                $scope.excludesearch.createdby = "";
                angular.forEach($scope.excludecreated, function (value) {
                    value.showcross = false;
                    value.creclass = 'cretaed';
                });
            }
        };
        $scope.advexcludeassignto = function (check) {
            $scope.eaassignshow = check;
            $scope.excludesearch.assignedto = '';
            if ($scope.excludeassign.length > 0) {
                angular.forEach($scope.excludeassign, function (value) {
                    value.creclass = 'cretaed';
                    value.showcross = false;
                });
            }
        };
        $scope.advexcludeassigntoall = function (check) {
            $scope.eaassignallleft = check;
            if (check == true) {
                $scope.excludesearch.assignedto = "";
                angular.forEach($scope.excludeassign, function (value) {
                    value.showcross = true;
                    if (value.leftflag) {
                        value.creclass = 'createdactiveleft';
                    } else {
                        value.creclass = 'createdactive';
                    }
                    if ($scope.excludesearch.assignedto == "") {
                        $scope.excludesearch.assignedto = value.assignedto;
                    } else {
                        $scope.excludesearch.assignedto = $scope.excludesearch.assignedto + ',' + value.assignedto;
                    }
                });
            }
            else {
                $scope.excludesearch.assignedto = "";
                angular.forEach($scope.excludeassign, function (value) {
                    value.showcross = false;
                    value.creclass = 'cretaed';
                });
            }
        };
        $scope.advexcludecreateallleft = function (check) {
            if (check == true) {
                angular.forEach($scope.excludecreated, function (value) {
                    if (value.leftflag) {
                        value.showcross = true;
                        value.creclass = 'createdactiveleft';
                        if ($scope.excludesearch.createdby == "") {
                            $scope.excludesearch.createdby = value.createdby;
                        }
                        else {
                            $scope.excludesearch.createdby = $scope.excludesearch.createdby + ',' + value.createdby;
                        }
                    }
                });
            }
            else {
                $scope.excludesearch.createdby = "";
                angular.forEach($scope.excludecreated, function (value) {
                    if (value.leftflag) {
                        value.showcross = false;
                        value.creclass = 'cretaed';
                    }
                });
                if ($scope.eacreateall) {
                    angular.forEach($scope.excludecreated, function (value) {
                        if (!value.leftflag) {
                            value.showcross = true;
                            value.creclass = 'createdactive';
                            if ($scope.excludesearch.createdby == "") {
                                $scope.excludesearch.createdby = value.createdby;
                            }
                            else {
                                $scope.excludesearch.createdby = $scope.excludesearch.createdby + ',' + value.createdby;
                            }
                        }
                    });
                }
            }
        };
        $scope.advexcludeassigntoallleft = function (check) {
            if (check == true) {
                angular.forEach($scope.excludeassign, function (value) {
                    if (value.leftflag) {
                        value.showcross = true;
                        value.creclass = 'createdactiveleft';
                        if ($scope.excludesearch.assignedto == "") {
                            $scope.excludesearch.assignedto = value.assignedto;
                        }
                        else {
                            $scope.excludesearch.assignedto = $scope.excludesearch.assignedto + ',' + value.assignedto;
                        }
                    }
                });
            }
            else {
                $scope.excludesearch.assignedto = "";
                angular.forEach($scope.excludeassign, function (value) {
                    if (value.leftflag) {
                        value.showcross = false;
                        value.creclass = 'cretaed';
                    }
                });
                if ($scope.eaassignall) {
                    angular.forEach($scope.excludeassign, function (value) {
                        if (!value.leftflag) {
                            value.showcross = true;
                            value.creclass = 'createdactive';
                            if ($scope.excludesearch.assignedto == "") {
                                $scope.excludesearch.assignedto = value.assignedto;
                            }
                            else {
                                $scope.excludesearch.assignedto = $scope.excludesearch.assignedto + ',' + value.assignedto;
                            }
                        }
                    });
                }
            }
        };

        $scope.excludeyearplnxclude = function (yr) {
            if (yr == false) {
                $scope.get_comptasklist = $scope.get_comptasklistfilter;
            }
            else {
                var filteredtasklist = $filter('filter')($scope.get_comptasklistfilter, function (d) {
                    return (d.yrplan === null || d.yrplan === "");
                });
                $scope.get_comptasklist = filteredtasklist;
            }
            //$scope.from_date = null;
            //$scope.to_date = null;
            //$scope.searchValueall = null;
            //if ($scope.searchValueall !== undefined) {
            //    $scope.searchValueall = '';
            //}
            //if ($scope.searchValuecol !== undefined) {
            //    $scope.searchValuecol.ISMTCR_Title = '';
            //    $scope.searchValuecol.ISMMCLT_ClientName = '';
            //    $scope.searchValuecol.IVRMM_ModuleName = '';
            //    $scope.searchValuecol.createdby = '';
            //    $scope.searchValuecol.assignedby = '';
            //    $scope.searchValuecol.assignedto = '';
            //}
            //if ($scope.excludesearch !== undefined) {
            //    $scope.excludesearch.ISMTCR_Title = '';
            //    $scope.excludesearch.ISMMCLT_ClientName = '';
            //    $scope.excludesearch.IVRMM_ModuleName = '';
            //    $scope.excludesearch.createdby = '';
            //    $scope.excludesearch.assignedby = '';
            //    $scope.excludesearch.assignedto = '';
            //}
            //$scope.alltask = false;
        };

        $scope.loaddatacount = function () {

            var pageid = 2;
            var data = {
                "typeFlg": $scope.typeFlg
            };
            apiService.create("ISM_ComplaintsTaskList/getdetails", data).then(function (promise) {

                $scope.get_comptasklistcount = promise.get_comptasklist;
                $scope.get_comptasklistfilter = promise.get_comptasklist;
                if ($scope.get_comptasklistcount.length > 0) {
                    var notassignedcount = 0;
                    var assignedcount = 0;
                    var pendingtaskcount = 0;
                    var transfertaskcount = 0;
                    angular.forEach($scope.get_comptasklistcount, function (clt) {
                        //if (clt.ISMTCR_Status === 'Open') {
                        pendingtaskcount += 1;
                        //}
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
                    $scope.assignedcount = assignedcount;
                    $scope.notassignedcount = notassignedcount;
                    $scope.pendingcount = pendingtaskcount;
                    $scope.mypendingcount = promise.mycountonly;
                    $scope.transfercount = transfertaskcount;
                    $scope.excludecreateduserlist();
                    $scope.excludeassignuserlist();
                }
                else {
                    $scope.assignedcount = 0;
                    $scope.notassignedcount = 0;
                    $scope.pendingcount = 0;
                    $scope.transfercount = 0;
                }
                $scope.assginedbyme = promise.byMecount;
                $scope.assginedtoMe = promise.toMecount;
                $scope.transfertoMe = promise.transferMecount;
                $scope.transferbyMe = promise.transferbycount;
                $scope.totaltask = promise.byMecount + promise.transferMecount + promise.toMecount;
            });
        };

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

        //======================================== Total Task & ASSIGNED & NOT-ASSIGNED  COUNTS 
        $scope.gettotalcount = function () {
            $scope.typeFlg = "Search";
            $scope.arrayuserEmp = [];
            $scope.arrayStatus = [];
            $scope.get_comptasklist = [];
            $scope.currentPage = 1;
            angular.forEach($scope.get_userEmplist, function (em) {
                if (em.empck === true) {
                    $scope.arrayuserEmp.push(em);
                }
            });
            angular.forEach($scope.get_Status, function (sts) {
                $scope.arrayStatus.push(sts);
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
                    //if ($scope.get_comptasklist.length > 0) {
                    //    var notassignedcount = 0;
                    //    var assignedcount = 0;
                    //    var pendingtaskcount = 0;
                    //    var transfertaskcount = 0;
                    //    angular.forEach($scope.get_comptasklist, function (clt) {
                    //        if (clt.ISMTCR_Status === 'Open') {
                    //            pendingtaskcount += 1;
                    //        }
                    //        if (clt.tasktag === 2) {
                    //            transfertaskcount += 1;
                    //        }
                    //        if (clt.addtoplannerflag === 1) {
                    //            notassignedcount += 1;
                    //        }
                    //        else {
                    //            assignedcount += 1;
                    //        }
                    //    });
                    //    $scope.assignedcount = assignedcount;
                    //    $scope.notassignedcount = notassignedcount;
                    //    $scope.pendingcount = pendingtaskcount;
                    //    $scope.transfercount = transfertaskcount;
                    //}
                    //else {
                    //    $scope.assignedcount = 0;
                    //    $scope.notassignedcount = 0;
                    //    $scope.pendingcount = 0;
                    //    $scope.transfercount = 0;
                    //}
                });
        };
        $scope.getpendingcount = function () {
            $scope.clearsearch();
            $scope.typeFlg = "Search";
            $scope.countflag = "All Pending";
            $scope.tablename = "All Pending";
            $scope.notassign = false;
            var statusName = "Open,ReOpen,Inprogress,In progress";
            $scope.arrayuserEmp = [];
            $scope.arrayStatus = [];
            $scope.get_comptasklist = [];
            $scope.currentPage = 1;
            //angular.forEach($scope.get_userEmplist, function (em) {
            //    if (em.empck === true) {
            //        $scope.arrayuserEmp.push(em);
            //    }
            //});
            $scope.arrayStatus.push({ ISMMISTS_StatusName: statusName });
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
                    }
                });
        };

        //$scope.getassignedcount = function () {
        //    $scope.comptasklist = [];
        //    if ($scope.get_comptasklist.length > 0) {
        //        angular.forEach($scope.get_comptasklist, function (ctk) {
        //            if (ctk.addtoplannerflag === 1) {
        //                $scope.comptasklist.push({
        //                    ISMTCR_Id: ctk.ISMTCR_Id, HRMD_Id: ctk.HRMD_Id, HRMD_DepartmentName: ctk.HRMD_DepartmentName, HRMPR_Id: ctk.HRMPR_Id, HRMP_Name: ctk.HRMP_Name,
        //                    ISMTCR_BugOREnhancementFlg: ctk.ISMTCR_BugOREnhancementFlg, ISMTCR_CreationDate: new Date(ctk.ISMTCR_CreationDate), ISMTCR_Title: ctk.ISMTCR_Title, ISMTCR_Desc: ctk.ISMTCR_Desc, ISMTCR_Status: ctk.ISMTCR_Status, ISMTCR_ReOpenFlg: ctk.ISMTCR_ReOpenFlg, ISMTCR_ReOpenDate: ctk.ISMTCR_ReOpenDate, ISMTCR_TaskNo: ctk.ISMTCR_TaskNo, ISMMCLT_Id: ctk.ISMMCLT_Id, ISMMCLT_ClientName: ctk.ISMMCLT_ClientName, HRME_Id: ctk.HRME_Id, assignedto: ctk.assignedto, tasktag: ctk.tasktag, addtoplannerflag: ctk.addtoplannerflag, createdby: ctk.createdby
        //                });
        //            }
        //        });
        //        if ($scope.comptasklist.length > 0) {
        //            $scope.get_comptasklist = $scope.comptasklist;
        //        }
        //    }
        //};
        //$scope.getnotassignedcount = function () {
        //    $scope.comptasklist = [];
        //    if ($scope.get_comptasklist.length > 0) {                
        //        angular.forEach($scope.get_comptasklist, function (ctk) {
        //            if (ctk.addtoplannerflag === 0) {
        //                $scope.comptasklist.push({
        //                    ISMTCR_Id: ctk.ISMTCR_Id, HRMD_Id: ctk.HRMD_Id, HRMD_DepartmentName: ctk.HRMD_DepartmentName, HRMPR_Id: ctk.HRMPR_Id, HRMP_Name: ctk.HRMP_Name,
        //                    ISMTCR_BugOREnhancementFlg: ctk.ISMTCR_BugOREnhancementFlg, ISMTCR_CreationDate: new Date(ctk.ISMTCR_CreationDate), ISMTCR_Title: ctk.ISMTCR_Title, ISMTCR_Desc: ctk.ISMTCR_Desc, ISMTCR_Status: ctk.ISMTCR_Status, ISMTCR_ReOpenFlg: ctk.ISMTCR_ReOpenFlg, ISMTCR_ReOpenDate: ctk.ISMTCR_ReOpenDate, ISMTCR_TaskNo: ctk.ISMTCR_TaskNo, ISMMCLT_Id: ctk.ISMMCLT_Id, ISMMCLT_ClientName: ctk.ISMMCLT_ClientName, HRME_Id: ctk.HRME_Id, assignedto: ctk.assignedto, tasktag: ctk.tasktag, addtoplannerflag: ctk.addtoplannerflag, createdby: ctk.createdby
        //                });
        //            }
        //        });
        //        if ($scope.comptasklist.length > 0) {
        //            $scope.get_comptasklist = $scope.comptasklist;
        //        }
        //    }
        //};

        //======================================== PENDING , BY ME & To ME Count 
        $scope.getbymecount = function () {
            $scope.clearsearch();
            $scope.countflag = "ByMe";
            $scope.tablename = "Assigned By Me";
            $scope.notassign = false;
            $scope.getscounts();
        };
        $scope.getcompleteded = function () {
            $scope.clearsearch();
            $scope.countflag = "Completed";
            $scope.tablename = "Completed";
            $scope.notassign = false;
            $scope.getscounts();
        };
        $scope.getpendingclose = function () {
            intracontroller.viewcompleted = false;
            intracontroller.viewnotassigned = false;
            $scope.clearsearch();
            $scope.countflag = "Close";
            $scope.tablename = "Close Completed";
            $scope.notassign = false;
            $scope.getscounts();
        };
        $scope.getbymecountmy = function () {
            $scope.clearsearch();
            $scope.countflag = "MyPending";
            $scope.tablename = "My Pending";
            $scope.notassign = false;
            $scope.getscounts();
        };
        $scope.gettomecount = function () {
            $scope.clearsearch();
            $scope.notassign = false;
            $scope.tablename = "Assigned To Me";
            $scope.countflag = "ToMe";
            $scope.getscounts();
        };
        $scope.getcountpending = function () {
            $scope.clearsearch();
            $scope.notassign = false;
            $scope.countflag = "Pending";
            $scope.getscounts();
        };
        $scope.getcounttotal = function () {
            $scope.clearsearch();
            $scope.notassign = false;
            $scope.countflag = "total";
            $scope.getscounts();
        };
        $scope.getnotassingcount = function () {
            $scope.clearsearch();
            $scope.countflag = "NotAssign";
            $scope.tablename = "Not Assigned";
            $scope.notassign = true;
            $scope.getscounts();
        };

        $scope.gettometransfer = function () {
            $scope.clearsearch();
            $scope.notassign = false;
            $scope.tablename = "Transferred To Me";
            $scope.countflag = "transferto";
            $scope.getscounts();
        };
        $scope.getbymetransfer = function () {
            $scope.clearsearch();
            $scope.notassign = false;
            $scope.tablename = "Transferred By Me";
            $scope.countflag = "transferby";
            $scope.getscounts();
        };
        //added By Sanjeev
        $scope.getscountsT = function () {
            $scope.clearsearch();
            $scope.currentPage = 1;
            $scope.get_comptasklist = [];
            $scope.get_tasks = [];
            var data = {
                "countflag": "RejectedTask"
            };
            apiService.create("ISM_ComplaintsTaskList/getcount", data).
                then(function (promise) {
                    if (promise.get_tasks.length > 0) {
                        $scope.get_tasks = promise.get_tasks;
                        $scope.get_comptasklist = $scope.get_tasks;
                        $scope.get_comptasklistfilter = promise.get_tasks;
                        $scope.get_tasks_completed = promise.get_tasks_completed;
                        if ($scope.get_tasks_completed != null) {
                            $scope.compltetedcounttask = $scope.get_tasks_completed.length;
                        }
                        angular.forEach($scope.get_comptasklist, function (atck) {
                            atck.atck = $filter('date')(atck.ISMTCR_CreationDate, "dd-MM-yyyy");
                        });
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
                        //if ($scope.countflag == "NotAssign") {
                        //    $scope.excludeyearpln = true;
                        //    $scope.excludeyearplnxclude(true);
                        //}
                        $scope.excludecreateduserlist();
                        $scope.excludeassignuserlist();
                    }
                    else {
                        $scope.get_tasks = "";
                        $scope.get_comptasklist = [];
                        swal("No Records Found");
                    }
                });
        }
       //added By Sanjeev
        $scope.getscounts = function () {
            $scope.clearsearch();
            $scope.currentPage = 1;
            $scope.get_comptasklist = [];
            $scope.get_tasks = [];
            var data = {
                "countflag": $scope.countflag
            };
            apiService.create("ISM_ComplaintsTaskList/getcount", data).
                then(function (promise) {
                    if (promise.get_tasks.length > 0) {
                        $scope.get_tasks = promise.get_tasks;
                        $scope.get_comptasklist = $scope.get_tasks;
                        $scope.get_comptasklistfilter = promise.get_tasks;
                        $scope.get_tasks_completed = promise.get_tasks_completed;
                        if ($scope.get_tasks_completed != null) {
                            $scope.compltetedcounttask = $scope.get_tasks_completed.length;
                        }
                        angular.forEach($scope.get_comptasklist, function (atck) {
                            atck.atck = $filter('date')(atck.ISMTCR_CreationDate, "dd-MM-yyyy");
                        });
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
                        //if ($scope.countflag == "NotAssign") {
                        //    $scope.excludeyearpln = true;
                        //    $scope.excludeyearplnxclude(true);
                        //}
                        $scope.excludecreateduserlist();
                        $scope.excludeassignuserlist();
                    }
                    else {
                        $scope.get_tasks = "";
                        $scope.get_comptasklist = [];
                        swal("No Records Found");
                    }
                });
        };


        $scope.onDeptchange = function (objd) {

            $scope.get_employeelist = [];

            $scope.deptid = objd.HRMD_Id.HRMD_Id;
            var data = {
                "HRMD_Id": $scope.deptid,
                "MIId": objd.HRMD_Id.MI_Id
            };
            apiService.create("ISM_TaskRegisterReport/getemployeelist", data).
                then(function (promise) {
                    if (promise.get_employeelist.length > 0) {
                        $scope.get_employeelist = promise.get_employeelist;
                    }
                    else {
                        $scope.get_employeelist = "";
                        swal("No Record Found ....!!");
                    }

                });
        };
        //========================================= Planner




        //shilpa
        $scope.deptfilter = function (selected, arrlistchk) {

            $scope.albumNameArray = [];
            for (var j = 0; j < selected.length; j++) {
                for (var i = 0; i < arrlistchk.length; i++) {
                    if (selected[j] == arrlistchk[i].hrmdC_ID) {
                        $scope.albumNameArray.push(arrlistchk[i]);
                    }
                }
            }

            var data = {
                "savegroup": $scope.albumNameArray
            }
            apiService.create("ISM_ComplaintsTaskList/deptfilter", data).then(function (promise) {

                $scope.taskplanner = [];
                $scope.get_employeelist = promise.get_employeelist;
                if ($scope.get_comptasklist.length > 0) {
                    angular.forEach($scope.get_comptasklist, function (objct) {
                        if (objct.checkedvalue === true) {
                            $scope.empll = [];
                            angular.forEach($scope.get_employeelist, function (ll) {
                                $scope.empll.push({
                                    id: objct.ISMTCR_Id + ll.HRME_Id, HRME_Id: ll.HRME_Id, employeeName:
                                        ll.employeeName, hrmD_DepartmentName: ll.HRMD_DepartmentName
                                });
                            });
                            objct.emplist = $scope.empll;
                            objct.taskDate = new Date($scope.today);
                            $scope.taskplanner.push(objct);
                        }
                    });
                }
            })

        }


        $scope.Assigntask = function () {
            $scope.showassignflag = false;
            angular.forEach($scope.get_comptasklist, function (objct) {
                if ($scope.taskid === objct.ISMTCR_Id) {
                    objct.checkedvalue = true;
                }
            });
            $scope.addtoplannernew();
        }

        $scope.unAssigntask = function () {
            $scope.showassignflag = true;
            $scope.counttask = 0;
            angular.forEach($scope.get_comptasklist, function (objct) {
                if ($scope.taskid === objct.ISMTCR_Id) {
                    objct.checkedvalue = false;
                }
                if (objct.checkedvalue == true) {
                    $scope.counttask++;
                }
            });
            if ($scope.counttask == 0) {
                $scope.taskplanner = [];
            }
        }

        $scope.openassignlist = function () {
            $('#myModalAddtoPlannerone').modal('hide');
            $('#myModalResponse').modal('hide');
            $('.modal-backdrop').remove();
            $('.modal').on('hide.bs.modal', function (e) {
                e.stopPropagation();
                $('body').css('padding-right', '');
            });
            $scope.addtoplanner();
        }

        $scope.addtoplannernew = function (taskobj) {

            $scope.arrlistchk = $scope.arrlistchk1;
            $scope.taskplanner = [];
            $scope.todaydate = new Date($scope.today);
            if ($scope.get_comptasklist.length > 0) {
                angular.forEach($scope.get_comptasklist, function (objct) {
                    if (objct.checkedvalue === true && objct.ISMTCR_Id == $scope.taskid) {
                        objct.taskDate = '';
                        $scope.empll = [];
                        angular.forEach($scope.get_employeelist, function (ll) {
                            if ($scope.hrmeid === ll.HRME_Id) {
                                if ($scope.get_employeelist.length === 1) {
                                    $scope.empll.push({ id: objct.ISMTCR_Id + ll.HRME_Id, HRME_Id: ll.HRME_Id, employeeName: ll.employeeName, hrmD_DepartmentName: ll.HRMD_DepartmentName, checked: true, hrmdC_ID: ll.HRMDC_ID });
                                } else {
                                    $scope.empll.push({ id: objct.ISMTCR_Id + ll.HRME_Id, HRME_Id: ll.HRME_Id, employeeName: ll.employeeName, hrmD_DepartmentName: ll.HRMD_DepartmentName, checked: false, hrmdC_ID: ll.HRMDC_ID });
                                }
                            } else {
                                $scope.empll.push({ id: objct.ISMTCR_Id + ll.HRME_Id, HRME_Id: ll.HRME_Id, employeeName: ll.employeeName, hrmD_DepartmentName: ll.HRMD_DepartmentName, checked: false, hrmdC_ID: ll.HRMDC_ID });
                            }
                        });

                        if (objct.emplist == null || objct.emplist == undefined) {
                            objct.emplist = $scope.empll;
                        }
                        else {
                            objct.emplist = objct.emplist;
                        }
                        objct.taskDate = new Date(objct.todaysdate);
                        objct.enddate = new Date(objct.endOfYear);
                        if (objct.effhrs == null || objct.effhrs == undefined) {
                            objct.effhrs = 0;
                        }
                        else {
                            objct.effhrs = objct.effhrs;
                        }
                        if (objct.effmin == null || objct.effmin == undefined) {
                            objct.effmin = 0;
                        }
                        else {
                            objct.effmin = objct.effmin;
                        }


                        if (objct.ISMMTCAT_EachTaskMaxDuration != null && objct.ISMMTCAT_EachTaskMaxDuration > 0 && objct.ISMMTCAT_DurationFlg == "HOURS") {
                            var decimaltime = objct.ISMMTCAT_EachTaskMaxDuration;
                            var hrs = parseInt(Number(decimaltime));
                            var min = Math.round((Number(decimaltime) - hrs) * 60);
                            if (hrs.toString().length === 1) {
                                hrs = '0' + hrs;
                            }
                            if (min.toString().length === 1) {
                                min = '0' + min;
                            }
                            objct.effhrs = hrs;
                            objct.effmin = min;
                        }
                        else {
                            if (objct.effmin == 0) {
                                objct.effmin = parseInt(Number(objct.ISMMTCAT_EachTaskMaxDuration));
                            }
                            else {
                                objct.effmin = objct.effmin;
                            }
                            //objct.effmin = parseInt(Number(objct.ISMMTCAT_EachTaskMaxDuration));
                        }

                        //objct.ISMMTCAT_DurationFlg = "HOURS";
                        objct.ismtcrastO_StartDate = new Date(objct.todaysdate);
                        //var d = new Date(objct.todaysdate);
                        //d.setDate(d.getDate() + 7);
                        //objct.ismtcrastO_EndDate = d;
                        $scope.taskplanner.push(objct);
                    }
                });
            }
            if ($scope.taskplanner.length > 0) {
                console.log($scope.taskplanner);
                //$('#myModalAddtoPlanner').modal('show');
            }
            else {
                swal("Please Select Any Task ..... !!");
            }
        };


        $scope.addtoplanner = function (taskobj) {
            $scope.submitted1 = false;
            $scope.arrlistchk = $scope.arrlistchk1;
            $scope.taskplanner = [];
            $scope.todaydate = new Date($scope.today);
            var countnot = 0;
            var addedcount = 0;
            if ($scope.filterValue.length > 0) {
                angular.forEach($scope.filterValue, function (objct) {
                    if (addedcount < 25) {
                        if (objct.checkedvalue === true) {
                            addedcount++;
                            if (objct.addtoplannerflag === true || objct.addtoplannerflag === 1) {
                                objct.taskDate = '';
                                $scope.empll = [];
                                angular.forEach($scope.get_employeelist, function (ll) {
                                    if ($scope.hrmeid === ll.HRME_Id) {
                                        if ($scope.get_employeelist.length === 1) {
                                            $scope.empll.push({ id: objct.ISMTCR_Id + ll.HRME_Id, HRME_Id: ll.HRME_Id, employeeName: ll.employeeName, hrmD_DepartmentName: ll.HRMD_DepartmentName, checked: true, hrmdC_ID: ll.HRMDC_ID });
                                        } else {
                                            $scope.empll.push({ id: objct.ISMTCR_Id + ll.HRME_Id, HRME_Id: ll.HRME_Id, employeeName: ll.employeeName, hrmD_DepartmentName: ll.HRMD_DepartmentName, checked: false, hrmdC_ID: ll.HRMDC_ID });
                                        }
                                    } else {
                                        $scope.empll.push({ id: objct.ISMTCR_Id + ll.HRME_Id, HRME_Id: ll.HRME_Id, employeeName: ll.employeeName, hrmD_DepartmentName: ll.HRMD_DepartmentName, checked: false, hrmdC_ID: ll.HRMDC_ID });
                                    }
                                });

                                if (objct.emplist == null || objct.emplist == undefined) {
                                    objct.emplist = $scope.empll;
                                }
                                else {
                                    objct.emplist = objct.emplist;
                                }
                                objct.taskDate = new Date(objct.todaysdate);
                                objct.enddate = new Date(objct.endOfYear);
                                if (objct.effhrs == null || objct.effhrs == undefined) {
                                    objct.effhrs = 0;
                                }
                                else {
                                    objct.effhrs = objct.effhrs;
                                }
                                if (objct.effmin == null || objct.effmin == undefined) {
                                    objct.effmin = 0;
                                }
                                else {
                                    objct.effmin = objct.effmin;
                                }

                                if (objct.ISMMTCAT_EachTaskMaxDuration != null && objct.ISMMTCAT_EachTaskMaxDuration > 0 && objct.ISMMTCAT_DurationFlg == "HOURS") {
                                    var decimaltime = objct.ISMMTCAT_EachTaskMaxDuration;
                                    var hrs = parseInt(Number(decimaltime));
                                    var min = Math.round((Number(decimaltime) - hrs) * 60);
                                    if (hrs.toString().length === 1) {
                                        hrs = '0' + hrs;
                                    }
                                    if (min.toString().length === 1) {
                                        min = '0' + min;
                                    }
                                    objct.effhrs = hrs;
                                    objct.effmin = min;
                                }
                                else {
                                    if (objct.effmin == 0) {
                                        objct.effmin = parseInt(Number(objct.ISMMTCAT_EachTaskMaxDuration));
                                    }
                                    else {
                                        objct.effmin = objct.effmin;
                                    }
                                }
                                objct.ismtcrastO_StartDate = new Date(objct.todaysdate);
                                $scope.taskplanner.push(objct);
                            }
                            else {
                                countnot++;
                            }
                        }
                    }
                    else {
                        if (objct.checkedvalue === true) {
                            addedcount++;
                        }
                        objct.checkedvalue = false;
                        $scope.alltask = false;
                    }
                });
                if (addedcount > 25) {
                    swal("You can assign 25 tasks at once.");
                };
            }
            if ($scope.taskplanner.length > 0) {
                console.log($scope.taskplanner);
                //$('#myModalAddtoPlanner').modal('show');
            }
            else {
                if (countnot > 0) {
                    angular.forEach($scope.filterValue, function (objct) {
                        objct.checkedvalue = false;
                    });
                    var selectedItems = $scope.filterValue.filter(function (item) {
                        return item.checkedvalue;
                    });
                    if (selectedItems.length === $scope.filterValue.length) {
                        $scope.alltask = true;
                    }
                    else {
                        $scope.alltask = false;
                    }
                    swal("Select Un-Assigned Tasks..!!");
                }
                else {
                    swal("Please Select Any Task ..... !!");
                }
            }
        };

        $scope.addtoupdatelist = function () {
            $scope.submitted1 = false;
            $scope.taskupadtelist = [];
            var addedcountup = 0;
            if ($scope.filterValue.length > 0) {
                angular.forEach($scope.filterValue, function (objct) {
                    if (addedcountup < 100) {
                        if (objct.checkedvalue === true) {
                            addedcountup++;
                            if ($scope.roletype === "Head") {
                                $scope.headtype = true;
                            };
                            $scope.tempstatus = [];
                            if ($scope.hrmeid !== objct.createdhrmeid) {
                                if ($scope.hrmeid !== objct.crdprthead) {
                                    angular.forEach($scope.get_Status, function (task) {
                                        if (task.ismmistS_StatusName != "Close" && task.ismmistS_StatusName != "close") {
                                            $scope.tempstatus.push(task);
                                        };
                                    });
                                }
                                else {
                                    $scope.tempstatus = $scope.get_Status;
                                }
                                if (objct.addtoplannerflag === true || objct.addtoplannerflag === 1) {
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

                            if ($scope.headtype === false) {
                                for (var i = $scope.tempstatus.length - 1; i >= 0; i--) {
                                    if ($scope.tempstatus[i].ismmistS_StatusName.toLowerCase() == "onhold-department" || $scope.tempstatus[i].ismmistS_StatusName.toLowerCase() == "onhold" || $scope.tempstatus[i].ismmistS_StatusName.toLowerCase() == "management blocker") {
                                        $scope.tempstatus.splice(i, 1);
                                    }
                                }
                            }
                            objct.updatestatuslist = $scope.tempstatus;

                            if (objct.ismtcrreS_Response == null || objct.ismtcrreS_Response == undefined) {
                                objct.ismtcrreS_Response = "";
                            }
                            else {
                                objct.ismtcrreS_Response = objct.ismtcrreS_Response;
                            }
                            $scope.taskupadtelist.push(objct);
                        }
                    }
                    else {
                        if (objct.checkedvalue === true) {
                            addedcountup++;
                        }
                        objct.checkedvalue = false;
                        $scope.alltask = false;
                    }
                });
                if (addedcountup > 100) {
                    swal("You can update 100 tasks at once.");
                };
            }
            if ($scope.taskupadtelist != null && $scope.taskupadtelist.length > 0) {
                $scope.maintable = false;
                $scope.updatetable = true;
                $scope.topmultistatus = ""
                $scope.scroll();
            }
            else {
                swal("Please Select Any Task ..... !!");
            }
        };

        $scope.removeupdatetask = function (task) {
            $scope.taskupadtelist = $filter('filter')($scope.taskupadtelist, function (d) {
                return (d.ISMTCR_Id !== task);
            });
            angular.forEach($scope.filterValue, function (objct) {
                if (objct.ISMTCR_Id === task) {
                    objct.checkedvalue = false;
                }
            });

            var selectedItems = $scope.filterValue.filter(function (item) {
                return item.checkedvalue;
            });
            if (selectedItems.length === $scope.filterValue.length) {
                $scope.alltask = true;
            }
            else {
                $scope.alltask = false;
            }

            if ($scope.taskupadtelist.length == 0) {
                $scope.goback();
            }
        }

        $scope.goback = function () {
            $scope.scroll();
            $scope.maintable = true;
            $scope.updatetable = false;
        };

        $scope.univeraslstatuschange = function (unistatus) {
            angular.forEach($scope.taskupadtelist, function (objct) {
                if (unistatus == 'Close' || unistatus == 'close') {
                    if (($scope.hrmeid === objct.createdhrmeid) || ($scope.hrmeid === objct.crdprthead)) {
                        var checkexist = [];
                        checkexist = $filter('filter')(objct.updatestatuslist, function (d) {
                            return (d.ismmistS_StatusName === unistatus);
                        });
                        if (checkexist.length > 0) {
                            objct.ismmistS_StatusName = unistatus;
                            objct.ismtcrreS_Response = unistatus;
                        } else {
                            objct.ismmistS_StatusName = "";
                            objct.ismtcrreS_Response = "";
                        }
                    }
                    else {
                        objct.ismmistS_StatusName = "";
                        objct.ismtcrreS_Response = "";
                    }
                }
                else if (unistatus.toLowerCase() == 'completed' || unistatus.toLowerCase() == "development completed") {
                    if (objct.addtoplannerflag === false || objct.addtoplannerflag === 0) {
                        //if ($scope.hrmeid === objct.createdhrmeid) {
                            objct.ismmistS_StatusName = unistatus;
                            objct.ismtcrreS_Response = unistatus;
                        //}
                        //else {
                        //    objct.ismmistS_StatusName = "";
                        //    objct.ismtcrreS_Response = "";
                        //}
                    }
                    else {
                        objct.ismmistS_StatusName = "";
                        objct.ismtcrreS_Response = "";
                    }
                }
                else {
                    objct.ismmistS_StatusName = unistatus;
                    objct.ismtcrreS_Response = unistatus;
                }

                //if (objct.remarks == null || objct.remarks == undefined || objct.remarks == "") {
                //    objct.remarks = unistatus;
                //}
                //else {
                //    objct.remarks = objct.remarks;
                //}
            });
        };

        $scope.ondatechangesta = function (objplnr) {
            angular.forEach($scope.get_comptasklist, function (objct) {
                if (objplnr.ISMTCR_Id === objct.ISMTCR_Id) {
                    var d = new Date(objct.ismtcrastO_StartDate);
                    d.setDate(d.getDate() + 5);
                    objct.ismtcrastO_EndDate = d;
                }
            });
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

        //======================================== Transfer
        $scope.transfertask = function () {
            $scope.tasktransfer = [];
            if ($scope.get_comptasklist.length > 0) {
                angular.forEach($scope.get_comptasklist, function (objct) {
                    if (objct.checkedvalue === true) {
                        objct.taskcreatedate = new Date(objct.ISMTCR_CreationDate);
                        objct.effhrs = 0;
                        objct.effmin = 0;
                        $scope.tasktransfer.push(objct);
                    }
                });
            }
            if ($scope.tasktransfer.length > 0) {
                //$('#myModalTransfer').modal('show');
            }
            else {
                swal("Please Select Any Task ..... !!");
            }
        };

        $scope.responsetransfer = function () {
            $('#myModalResponse').modal('hide');
            $('.modal-backdrop').remove();
            $scope.tasktransfer = [];
            if ($scope.get_taskdetails.length > 0) {
                $scope.tasktransfer.push({
                    ISMTCR_Id: $scope.get_taskdetails[0].ISMTCR_Id,
                    ISMTCR_Title: $scope.get_taskdetails[0].ISMTCR_Title,
                    ISMTCR_TaskNo: $scope.get_taskdetails[0].ISMTCR_TaskNo,
                    ISMMCLT_ClientName: $scope.get_taskdetails[0].ISMMCLT_ClientName,
                    ISMTCR_CreationDate: $scope.get_taskdetails[0].ISMTCR_CreationDate,
                    ISMTCR_Status: $scope.get_taskdetails[0].ISMTCR_Status,
                    createdby: $scope.get_taskdetails[0].createdby,
                    ISMMCLT_ClientName: $scope.get_taskdetails[0].ISMMCLT_ClientName,
                    HRMP_Name: $scope.get_taskdetails[0].HRMP_Name,
                    ISMTCR_BugOREnhancementFlg: $scope.get_taskdetails[0].ISMTCR_BugOREnhancementFlg,
                    taskDate: new Date($scope.get_taskdetails[0].todaysdate),
                    enddate: new Date($scope.get_taskdetails[0].endOfYear),                    
                    effhrs: 0,
                    effmin: 0
                });
            }
        };

        $scope.saveTransfer = function () {
            if ($scope.myFormModelT.$valid) {
                $scope.transferArray = [];
                $scope.hrme_list = [];
                var counthrs = 0.00;
                var totalhrs = 0.00;
                angular.forEach($scope.tasktransfer, function (tt) {
                    if (tt.ismtcrtrtO_StartDate === null || tt.ismtcrtrtO_StartDate === undefined || tt.ismtcrtrtO_StartDate === "not available") {
                        tt.ismtcrtrtO_StartDate = new Date($scope.today);
                        tt.ismtcrtrtO_EndDate = new Date($scope.today);
                    }
                    counthrs = (parseFloat(tt.effmin) * 0.0166667).toFixed(2);
                    counthrs = parseFloat(tt.effhrs) + parseFloat(counthrs);

                    tt.ismtcrtrtO_EffortInHrs = counthrs;

                    $scope.transferArray.push({ ISMTCR_Id: tt.ISMTCR_Id, ismtcrtrtO_StartDate: new Date(tt.ismtcrtrtO_StartDate).toDateString(), ismtcrtrtO_EndDate: new Date(tt.ismtcrtrtO_EndDate).toDateString(), ismtcrtrtO_EffortInHrs: tt.ismtcrtrtO_EffortInHrs, ismtcrtrtO_Remarks: tt.ismtcrtrtO_Remarks, ismtcrtrtO_Id: tt.IsmtcrtrtO_Id });
                });

                angular.forEach($scope.get_employeelist, function (tt) {
                    if (tt.checked == true)
                        $scope.hrme_list.push({ HRME_Id: tt.HRME_Id });
                });

                var data = {
                    "ISMTCRTRTO_TransferredDate": new Date($scope.ismtcrtrtO_TransferredDate).toDateString(),
                    "transferArray": $scope.transferArray,
                    "hrme_list": $scope.hrme_list,
                    "transfer": false
                };
                apiService.create("ISM_ComplaintsTaskList/saveTransfer", data).then(function (promise) {
                    if (promise.returnval === true) {
                        if (promise.ismtcrtrtO_Id === 0 || promise.ismtcrtrtO_Id < 0) {
                            swal('Task Transferred successfully ... !!');
                            $('.modal-backdrop').remove();
                            $('.modal').on('hide.bs.modal', function (e) {
                                e.stopPropagation();
                                $('body').css('padding-right', '');
                            });
                            $state.reload();
                        }
                        else if (promise.ismtcrtrtO_Id > 0) {
                            $('.modal-backdrop').remove();
                            $('.modal').on('hide.bs.modal', function (e) {
                                e.stopPropagation();
                                $('body').css('padding-right', '');
                            });
                            swal('Record updated successfully');
                            $state.reload();
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else if (promise.message === 'Planner') {
                        swal({
                            title: "You can't transfer this task, it's been already added to planner!",
                            text: "Still do you want to transfer..?",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Transfer!",
                            cancelButtonText: "No",
                            closeOnConfirm: false,
                            closeOnCancel: false
                        },
                            function (isConfirm) {
                                if (isConfirm) {
                                    var datat = {
                                        "ISMTCRTRTO_TransferredDate": new Date($scope.ismtcrtrtO_TransferredDate).toDateString(),
                                        "transferArray": $scope.transferArray,
                                        "hrme_list": $scope.hrme_list,
                                        "transfer": true
                                    };
                                    apiService.create("ISM_ComplaintsTaskList/saveTransfer", datat).then(function (promise) {
                                        if (promise.returnval === true) {
                                            if (promise.transfer == false) {
                                                swal('Task Transferred successfully ... !!');
                                            }
                                            else if (promise.transfer == true) {
                                                $scope.transfer_plannerUpdate = promise.transfer_plannerUpdate;
                                                if ($scope.transfer_plannerUpdate != null && $scope.transfer_plannerUpdate != undefined && $scope.transfer_plannerUpdate.length > 0) {
                                                    var sw = "Task Transferred from Planner Dates " + $scope.transfer_plannerUpdate[0].Startdate + " To " + $scope.transfer_plannerUpdate[0].Enddate + " & Planner Effort Reduced to " + $scope.transfer_plannerUpdate[0].Total + "Hr From " + $scope.transfer_plannerUpdate[0].ActualTotal + "Hr";
                                                    swal(sw, 'Task Transferred successfully ... !!')
                                                }
                                                else {
                                                    swal('Task Transferred successfully ... !!');
                                                }
                                            }
                                            $('.modal-backdrop').remove();
                                            $('.modal').on('hide.bs.modal', function (e) {
                                                e.stopPropagation();
                                                $('body').css('padding-right', '');
                                            });
                                            $state.reload();
                                        }
                                    });
                                }
                                else {
                                    swal("Task Transfer Cancelled!!!");
                                }
                            });
                    }
                    else {
                        if (promise.ismtcrtrtO_Id === 0 || promise.ismtcrtrtO_Id < 0) {
                            swal('Failed to Transfer, please contact administrator');
                        }
                        else if (promise.ismtcrtrtO_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }

                });
            }
            else {
                $scope.submitted2 = true;
            }
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

        $scope.updatestatus = function () {
            angular.forEach($scope.taskupadtelist, function (objct) {
                if (objct.ISMTCR_Id === $scope.taskidmulti) {
                    objct.ismtcrreS_Response = $scope.Responsemodal;
                    objct.ismmistS_StatusName = $scope.statusNamemodal;
                }
            });
        };
        $scope.removeupdatetaskmodal = function (task) {
            $scope.taskupadtelist = $filter('filter')($scope.taskupadtelist, function (d) {
                return (d.ISMTCR_Id !== $scope.taskidmulti);
            });
            angular.forEach($scope.filterValue, function (objct) {
                if (objct.ISMTCR_Id === $scope.taskidmulti) {
                    objct.checkedvalue = false;
                }
            });
            var selectedItems = $scope.filterValue.filter(function (item) {
                return item.checkedvalue;
            });
            if (selectedItems.length === $scope.filterValue.length) {
                $scope.alltask = true;
            }
            else {
                $scope.alltask = false;
            }

            if ($scope.taskupadtelist.length == 0) {
                $('#myModalResponsemultistatus').modal('hide');
                $scope.goback();
            }
            else if ($scope.taskupadtelist.length == 1) {
                $scope.taskindexmulti = 0;
                $scope.nexttaskobjm.ISMTCR_Id = $scope.taskupadtelist[$scope.taskindexmulti].ISMTCR_Id;
                $scope.nexttaskobjm.assignedto = $scope.taskupadtelist[$scope.taskindexmulti].assignedto;
                $scope.nexttaskobjm.checkedvalue = $scope.taskupadtelist[$scope.taskindexmulti].checkedvalue;
                $scope.viewresponsemultiple($scope.nexttaskobjm, '', $scope.taskindexmulti);
            }
            else {
                if ($scope.taskupadtelist != null && $scope.taskindexmulti > $scope.taskupadtelist.length) {
                    $scope.taskindexmulti = $scope.taskindexmulti - 1;
                    $scope.backtaskmulti();
                }
                else if ($scope.taskindexmulti == 0) {
                    $scope.taskindexmulti = -1;
                    $scope.nexttaskmulti();
                }
                else {
                    $scope.taskindexmulti = $scope.taskindexmulti - 2;
                    $scope.nexttaskmulti();
                }
            }
        }

        $scope.removeallupdatetaskmodal = function () {
            $scope.taskupadtelist = [];

            angular.forEach($scope.filterValue, function (objct) {
                objct.checkedvalue = false;
            });

            $scope.alltask = false;

            if ($scope.taskupadtelist.length == 0) {
                $('#myModalResponsemultistatus').modal('hide');
                $scope.goback();
            }
        }


        $scope.viewresponseclose = function () {
            //$('#myModalResponse').modal('show');            
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
            //$('#myModalResponse').modal('show');
            //$('.modal').on('hide.bs.modal', function (e) {
            //    e.stopPropagation();
            //    $('body').css('padding-right', '');
            //});
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
        //=================== Upload
        $scope.stepsModel = [];
        $scope.filedata = [];
        $scope.selectFileforUpload = function (event) {
            $scope.files = event.files;
            for (var i = 0; i < $scope.files.length; i++) {
                var file = $scope.files[i].name;
                $scope.filedata = $scope.files;
                $scope.fileimg = file;
                var reader = new FileReader();
                reader.onload = $scope.imageIsLoaded;
                reader.readAsDataURL($scope.files[i]);
            }
        };
        //============= Remove Selected File 
        $scope.remove_file = function () {
            setTimeout(HideLabel, 5000);
            $scope.file_detail = "";
            $scope.files = "";
            $scope.files_paths = "";
            //$scope.loaddatacount();
            $scope.nexttaskobj.ISMTCR_Id = $scope.taskid;
            $scope.nexttaskobj.assignedto = $scope.Rep_assignedto;
            $scope.viewresponse($scope.nexttaskobj, '', $scope.taskindex, $scope.pageno, $scope.f);
        };

        function HideLabel() {
            document.getElementById("lablehide").style.display = "none";
        }
        $scope.hidelableview = function () {
            document.getElementById("lablehide").style.display = "none";
        }

        $scope.imageIsLoaded = function (e) {
            $scope.$apply(function () {
                $scope.stepsModel.push(e.target.result);
            });
        };
        function UploadFiles() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.files.length; i++) {
                formData.append("File", $scope.files[i]);
                //   $scope.file_detail = $scope.files[i].name;
            }
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_ISMAttachment", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    swal(d);
                    $scope.files_paths = d;
                    if ($scope.files_paths.length > 0) {
                        $scope.saveResponse();
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.saveResponse = function () {
            if ($scope.myFormModel.$valid) {
                if ($scope.files_paths !== null && $scope.files_paths !== undefined && $scope.files_paths !== "") {
                    $scope.files_paths;
                }
                else {
                    $scope.files_paths = "";
                }
                var data = [];
                var counthrs = 0.00;
                if ($scope.taskeditable === true || $scope.assigntaskeditable === true) {

                    if ($scope.assigntaskeditable === true) {
                        counthrs = (parseFloat($scope.objupdate.effmin) * 0.0166667).toFixed(2);
                        counthrs = parseFloat($scope.objupdate.effhrs) + parseFloat(counthrs);
                    }
                    data = {
                        "ISMTCR_Id": $scope.taskid,
                        "ISMTCRRES_ResponseDate": new Date($scope.ismtcrreS_ResponseDate).toDateString(),
                        "ISMTCRRES_Response": $scope.ismtcrreS_Response,
                        "ISMMISTS_StatusName": $scope.ismmistS_StatusName,
                        "durationFlg": $scope.objupdate.durationFlg,
                        "eachTaskMaxDuration": counthrs,
                        "taskdescription": $scope.taskdescription,
                        "delettask": $scope.delettask,
                        "taskeditable": $scope.taskeditable,
                        "assigntaskeditable": $scope.assigntaskeditable,
                        attachmentArray: $scope.files_paths
                    };
                }
                else {
                    data = {
                        "ISMTCR_Id": $scope.taskid,
                        "ISMTCRRES_ResponseDate": new Date($scope.ismtcrreS_ResponseDate).toDateString(),
                        "ISMTCRRES_Response": $scope.ismtcrreS_Response,
                        "ISMMISTS_StatusName": $scope.ismmistS_StatusName,
                        "taskdescription": $scope.taskdescription,
                        attachmentArray: $scope.files_paths
                    };
                }
                apiService.create("ISM_ComplaintsTaskList/saveResponse", data).then(function (promise) {
                    if (promise.returnval === true) {
                        $scope.showid = true;
                        if (document.getElementById('lablehide')) {
                            document.getElementById("lablehide").style.display = "inline";
                        }
                        angular.forEach($scope.get_comptasklist, function (qq) {
                            if ($scope.taskid === qq.ISMTCR_Id) {
                                qq.ISMTCR_Status = $scope.ismmistS_StatusName;
                            }
                        });
                        $scope.remove_file();
                        if ($scope.tasknext == true) {
                            $scope.nexttask()
                        }
                        //if (promise.ismtcrreS_Id === 0 || promise.ismtcrreS_Id < 0) {
                        //    //$('#myModalResponse').modal('hide');
                        //    //$('.modal-backdrop').remove();
                        //    //swal('Record saved successfully ... !!');
                        //    //$state.reload();
                        //    $scope.showid = true;
                        //    $scope.remove_file();
                        //}
                        //else if (promise.ismtcrreS_Id > 0) {
                        //    //$('#myModalResponse').modal('hide');
                        //    //$('.modal-backdrop').remove();
                        //    //swal('Record updated successfully');
                        //    //$state.reload();
                        //    $scope.showid = true;
                        //    $scope.remove_file();
                        //}
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else if (promise.returnduplicatestatus === 'Notupdate') {
                        swal("You can't update task in Current/Last planner task list.", 'Update in Daily Report!!');
                    }
                    else {
                        if (promise.ismtcrreS_Id === 0 || promise.ismtcrreS_Id < 0) {
                            swal('Failed to Update, please contact administrator');
                        }
                        else if (promise.ismtcrreS_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                });
            }
            else {
                $scope.submitted1 = true;
                //$('#myModalResponse').modal('show');
            }
        };

        $scope.updatemultistatusdrop = function (status, obj) {
            obj.ismtcrreS_Response = status;
        };

        $scope.updatemultistatus = function (form) {
            if (form.$valid) {
                if ($scope.taskupadtelist != null && $scope.taskupadtelist.length > 0) {
                    var data = {
                        savemultipletaskupdatearray: $scope.taskupadtelist
                    };
                    swal({
                        title: "Are you sure?",
                        text: "Do You Want To update..?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, update!",
                        cancelButtonText: "Cancel",
                        closeOnConfirm: false,
                        closeOnCancel: false
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                apiService.create("ISM_ComplaintsTaskList/saveMultiResponse", data).then(function (promise) {
                                    if (promise.returnval === true) {
                                        $scope.showid = true;
                                        $scope.maintable = true;
                                        $scope.updatetable = false;
                                        swal('Status Updated Successfully!!');
                                        $scope.get_comptasklist = $filter('filter')($scope.get_comptasklist, function (d) {
                                            return (d.checkedvalue === false || d.checkedvalue === undefined);
                                        });
                                        $scope.currentPage = 1;
                                        var selectedItems = $scope.filterValue.filter(function (item) {
                                            return item.checkedvalue;
                                        });
                                        if (selectedItems.length === $scope.filterValue.length) {
                                            $scope.alltask = true;
                                        }
                                        else {
                                            $scope.alltask = false;
                                        }
                                    }
                                    else {
                                        swal('Failed to update!!');
                                    }
                                });
                            }
                            else {
                                swal("Task Update Cancelled!!!");
                            }
                        });
                }
                else {
                    swal('Select any task to update!!');
                }
            }
            else {
                $scope.submitted1 = true;
            }
        };

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
        $scope.getscountsApproved = function () {
            $scope.clearsearch();
            $scope.currentPage = 1;
            $scope.get_comptasklist = [];
            $scope.get_tasks = [];
            $scope.get_approvedcountNew = [];
            if ($scope.get_pendingcountNew != null && $scope.get_pendingcountNew.length > 0) {
                $scope.alltaskk = false;              
                $scope.get_approvedcountNew = $scope.get_pendingcountNew;
                angular.forEach($scope.get_approvedcountNew, function (items) {
                    items.checkedvalue = false;                    
                    items.flag = "A";
                    items.HRMPR_Old = items.HRMPR_Id;
                    items.HRMPR_Id = items.HRMPR_Id;
                    items.ISMTCR_VerifierRemarks = "";
                });
            }

        };

        $scope.getscountsReject = function () {
            $scope.clearsearch();
            $scope.currentPage = 1;
            $scope.get_comptasklist = [];
            $scope.get_tasks = [];
            $scope.RejectedTask = [];
            if ($scope.RejectedTaskNew != null && $scope.RejectedTaskNew .length > 0) {               
                $scope.RejectedTask = $scope.RejectedTaskNew;              
            }

        };
        //
        $scope.addtoupdateTasks = function () {
            // SaveApprovaltasktemp
            $scope.SaveApprovaltasktemp = [];
            if ($scope.get_approvedcountNew != null && $scope.get_approvedcountNew.length > 0) {
                angular.forEach($scope.get_approvedcountNew, function (items) {
                    if (items.checkedvalue == true && items.HRMPR_Id > 0) {
                        $scope.SaveApprovaltasktemp.push({
                            ISMTCR_Id: items.ISMTCR_Id,
                            ISMTCRTRTO_Remarks: items.ISMTCR_VerifierRemarks,
                            ISMMAC_Id: items.HRMPR_Old,
                            HRMPR_Id: items.HRMPR_Id,
                            typeFlg: items.flag,
                            ISMTCRASTO_EffortInHrs: 0.10,
                            HRME_Id: items.createdhrmeid
                        })
                    }
                    
                    
                });
            }
            if ($scope.SaveApprovaltasktemp != null && $scope.SaveApprovaltasktemp.length > 0) {
                var data = {
                    "savegroup": $scope.SaveApprovaltasktemp
                };
                swal({
                    title: "Are you sure?",
                    text: "Do You Want To update..?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, update!",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("ISM_ComplaintsTaskList/SaveApprovaldata", data).then(function (promise) {
                                if (promise.message == "save") {
                                    swal("Record Save succesfully !");
                                }
                                else if (promise.message == "Notsave") {
                                    swal("Record Not saved  !");
                                }
                                else if (promise.message == "admin") {
                                    swal("Plaese Contact Administrator !");
                                }
                                $state.reload();
                            });
                        }
                        else {
                            swal("Task Update Cancelled!!!");
                        }
                    });
            }
            else {
                swal('Select any task to update!!');
            }
        }
        //addtoupdateTasks
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

