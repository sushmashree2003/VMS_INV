(function () {
    'use strict';
    angular.module('app').controller('ISM_SwapIssuesController', ISM_SwapIssuesController);
    ISM_SwapIssuesController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$sce', 'intracontroller', '$window', '$stateParams'];
    function ISM_SwapIssuesController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter, $sce, intracontroller, $window, $stateParams) {

        var imagedownload = "";
        var docname = "";
        var studentreg = "";
        var HostName = location.host;
        $scope.generate = true;
        $scope.generateothers = false;
        $scope.Approvalflag = true;
        $scope.halfday = false;
        $scope.submitted6 = false;
        $scope.savedisable = true;
        $scope.editdisable = true;
        $scope.deletedisable = true;
        $scope.userPrivileges = [];
        var pageid = $stateParams.pageId;

        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.disabled = true;
                if (privlgs[i].ivrmirP_AddFlag == true) {
                    $scope.save = true;
                    $scope.savebtn = true;
                    $scope.savedisable = true;
                }
                else {
                    $scope.save = false;
                    $scope.savebtn = false;

                }
                if (privlgs[i].ivrmirP_UpdateFlag == true) {
                    $scope.editflag = true;
                    $scope.editbtn = true;
                    $scope.editdisable = true;

                }
                else {
                    $scope.editflag = false;
                    $scope.editbtn = false;

                }
                if (privlgs[i].ivrmirP_DeleteFlag == true) {
                    $scope.deactiveflag = true;
                    $scope.deletebtn = true;
                    $scope.deletedisable = true;
                }
                else {
                    $scope.deactiveflag = false;
                    $scope.deletebtn = false;

                }


            }
        }
        this.isOpen = false;

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

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


        $scope.ISMDRPT_Date = new Date($scope.today);
        $scope.maxDatedof = new Date($scope.today);

        $scope.ISMDRPT_Date_others = new Date($scope.today);

        var fullyear = $scope.ISMDRPT_Date_others.getFullYear();
        $scope.maxDatefP = new Date(fullyear, "11", "31");


        $scope.datedisable = true;

        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date($scope.today);
        $scope.ismtcrtrtO_TransferredDate = new Date($scope.today);
        var today = (new Date($scope.today)).getDay();
        $scope.startweek = "";
        $scope.endweek = "";
        $scope.deviation_id = "";

        if (today === 6) {
            $scope.deviation_id = 1;
            $scope.startweek = new Date(new Date($scope.today).setDate(new Date($scope.today).getDate() - 5));
            $scope.endweek = new Date($scope.today);
        }
        else {
            $scope.deviation_id = 2;
            $scope.startweek = new Date($scope.today);
            $scope.endweek = new Date($scope.today);
        }

        // ******* On Load  *********//
        $scope.loaddata = function () {
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.genrateflag = false;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;
            $scope.get_plannerlist = [];

            apiService.getURI("ISM_SwapIssues/loaddata", pageid).then(function (promise) {
                $scope.get_Emplist = promise.get_userEmplist;
                $scope.empdepartment = promise.empdepartment;
                $scope.empname = promise.empname;

            });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };

        $scope.Report = function () {
            if ($scope.myForm.$valid) {
                $scope.arrayuserEmp = [];
                angular.forEach($scope.get_userEmplist, function (em) {
                    if (em.empck === true) {
                        $scope.arrayuserEmp.push(em);
                    }
                });
                var data = {
                    "HRME_Id": $scope.obj.HRME_Id.HRME_Id,
                   // "arrayuserEmp": $scope.arrayuserEmp
                };
                apiService.create("ISM_Swapissues/Report", data).then(function (promise) {
                    if (promise.getextratasks.length > 0) {
                        $scope.temparraynew=promise.getextratasks;

                    }
                    else {
                        swal("No Record  Found..... !!");
                      
                    }  

                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.clear = function () {
            $state.reload();
        };


        $scope.Swaplist = function () {
       
                $scope.arrayuserEmp = [];
                angular.forEach($scope.get_userEmplist, function (em) {
                    if (em.empck === true) {
                        $scope.arrayuserEmp.push(em);
                    }
            });

            $scope.temparraylst = [];

            angular.forEach($scope.temparraynew, function (em) {
                if (em.checkedTask === true) {
                    $scope.temparraylst.push(
                        {
                            category: em.BugOREnhancement,
                            client: em.ISMMCLT_ClientName,

                        }
                    );
                }
            });


            if ($scope.temparraylst.length > 0) {

                var data = {
                    "HRME_Id": $scope.obj.HRME_Id.HRME_Id,
                   // "arrayuserEmp": $scope.arrayuserEmp,
                    "swaplst": $scope.temparraylst,
                };


                apiService.create("ISM_Swapissues/Swaplist", data).then(function (promise) {
                    if (promise.swappinglst.length > 0) {
                        $scope.swappinglst = promise.swappinglst;

                    }
                    else {
                        swal("No Record  Found..... !!");

                    }

                });
            }
            else {
                swal("Select Tasks in the list");
            }

              
         
          
        };

        $scope.Swaptask = function () {

            swal({
                title: "Are you sure?",
                text: "Do You Want To Swap?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,Swap it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

           

            $scope.temparraylst = [];

            angular.forEach($scope.temparraynew, function (em) {
                if (em.checkedTask === true) {
                    $scope.temparraylst.push(
                        {
                            ISMTCR_Id: em.ISMTCR_Id,
                            ISMTPL_Id: em.ISMTPL_Id,

                        }
                    );
                }
            });


            $scope.temp = [];

            angular.forEach($scope.swappinglst, function (em) {
                if (em.chetask === true) {
                    $scope.temp.push(
                        {
                            ISMTCR_Id: em.ISMTCR_Id,
                            ISMTPL_Id: em.ISMTPL_Id,

                        }
                    );
                }
            });

            var time1 = 0;
            angular.forEach($scope.temparraynew, function (em) {
                if (em.checkedTask === true) {
                    time1 = time1 + em.ISMTPLTA_EffortInHrs;

                }
            });

            var time2 = 0;
            angular.forEach($scope.swappinglst, function (em) {
                if (em.chetask === true) {
                    time2 = time2 + em.ISMTPLTA_EffortInHrs;
                }
            });



            if (time2 <= time1) {
              //  swal("Time2  is greater than or equal to time 1 ");
           


            var data = {
                "HRME_Id": $scope.obj.HRME_Id.HRME_Id,
               // "arrayuserEmp": $scope.arrayuserEmp,
                "swaplst": $scope.temparraylst,
                "swappingtask": $scope.temp, 
            };


                apiService.create("ISM_Swapissues/Swaptask", data).then(function (promise) {
                    if (promise.message.length > 0) {

                        if (promise.message == 'Saved') {
                            swal("Record Swapped Sucessfully");
                            $state.reload();
                        }
                        else {
                            swal("Record Swapping Failed");
                            $state.reload();
                        }
                }
                else {
                        swal("Contact Administarator");
                        $state.reload();

                }

            });

            }
            else {
                swal("Swapping Task Effort Should not be Lesser than or Equal to " + time2 +"Hrs");
                        }

                    }
                    else {
                        swal("Request Cancelled!!!");
                    }
                });

        };

       


    }



})();

