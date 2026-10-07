(function () {
    'use strict';
    angular
        .module('app')
        .controller('IVRM_ClientHostingController', IVRM_ClientHostingController)

    IVRM_ClientHostingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce', 'Excel', '$timeout']
    function IVRM_ClientHostingController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce, Excel, $timeout) {

        $scope.submitted = false;
        $scope.setremind = false;
        $scope.disabledata = false;
        $scope.presentCountgrid = 0;
        $scope.urlPattern = /^https:\/\/[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)+\/#.*$/
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
            $scope.today = new Date($scope.today);
        }
        $scope.getserverdate();
        $scope.hrplmG_EntryDateTime = new Date($scope.today);
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("IVRM_VersionControlProcess/getdetails", pageid).then(function (promise) {
                $scope.getclientlist = promise.getclientlist;
                $scope.griddata = promise.getgriddata;
                $scope.presentCountgrid = promise.getgriddata.length;
                console.log($scope.getclientlist)
            });
        };

        //========================================= Save
        $scope.obj = {};
        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.getclientArray = [];
                angular.forEach($scope.getclientlist, function (emp) {
                    if (emp.selected == true) {
                        $scope.getclientArray.push({ ISMMCLT_Id: emp.ismmclT_Id });
                    }

                });
                var data = {
                    "IVRMCH_Id": $scope.IVRMCH_Id,
                    "IVRMCH_HostName": $scope.IVRMCH_HostName,
                    "IVRMCH_HostUrl": $scope.IVRMCH_HostUrl,
                    getClientList: $scope.getclientArray
                };
                apiService.create("IVRM_VersionControlProcess/savedata", data).then(function (promise) {
                    if (promise.returnval === "Add") {
                        swal('Record Saved successfully', "", "success");
                       // $state.reload();
                        //$scope.clickreminder();
                        $scope.loaddata();
                    }
                    else if (promise.returnval === 'Update') {
                        swal('Record Updated successfully',"","success");
                       // $state.reload();
                        //$scope.clickreminder();
                        $scope.loaddata();
                    }
                    else if (promise.returnval === 'Duplicate') {
                        swal('Record already exist',"","warning");
                    }
                    else {
                        swal('Failed, please contact administrator',"", "info");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        //--Active Deactive---//
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (user.ivrmcH_ActiveFlag == true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";

            }
            else {

                mgs = "Active";
                confirmmgs = "Activated";
            }
            var date = {
                "IVRMCH_Id": user.ivrmcH_Id
            }

            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("IVRM_VersionControlProcess/activedeactive/", date).
                            then(function (promise) {
                                if (promise.message != null) {
                                    swal(promise.message);
                                }
                                else {
                                    if (promise.returnval == "Updated") {
                                        swal(confirmmgs + " " + "Successfully.");
                                        $state.reload();
                                    }
                                    else {
                                        swal(confirmmgs + " " + " Successfully");
                                    
                                    }
                                }
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
               
                });
        }


        //=====================Editrecord....
        $scope.edit = function (task) {
            var data = {
                "IVRMCH_Id": task.ivrmcH_Id
            };
            apiService.create("IVRM_VersionControlProcess/editdata", data).then(function (promise) {
                if (promise.getclientlist.length > 0) {
                    $scope.IVRMCH_HostName = promise.getclientlist[0].IVRMCH_HostName;
                    $scope.IVRMCH_HostUrl = promise.getclientlist[0].IVRMCH_HostUrl;
                    $scope.IVRMCH_Id = promise.getclientlist[0].IVRMCH_Id;

                    angular.forEach(promise.getclientlist, function (emp) {
                        $scope.getclientlist.push({ ismmclT_Id: emp.ISMMCLT_Id, ismmclT_ClientName: emp.ISMMCLT_ClientName, selected: true });
                        //angular.forEach($scope.getclientlist, function (emp1) {
                        //    if (emp.ISMMCLT_Id == emp1.ismmclT_Id) {
                        //        emp1.selected = true;
                        //    }
                        //  //  $scope.getclientlist.push({ ismmclT_Id: emp.ISMMCLT_Id, ismmclT_ClientName: emp.ISMMCLT_ClientName, selected: true });
                        //});
                    });
                }
            });
        };


        $scope.viewdocument = function (task) {
            var data = {
                "IVRMCH_Id": task.ivrmcH_Id
            };
            apiService.create("IVRM_VersionControlProcess/editdata", data).then(function (promise) {
                if (promise.getclientlist.length > 0) {
                    $scope.HostName = task.ivrmcH_HostName;
                    $scope.HostUrl = task.ivrmcH_HostUrl;

                    $scope.getClientDetails = promise.getclientlist;
                    $('#popup11').modal('show');

                }
                else {
                    swal("No Record Found!")
                }
            });
        };

        //=================Active/Deactivate
        $scope.deactiveqq = function (task) {
            $scope.ISMTCR_Id = task.ismtcR_Id;
            var dystring = "";
            if (task.ismtcR_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (task.ismtcR_ActiveFlg === false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("IVRM_VersionControlProcess/deactive", task).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.cancel = function () {
            $state.reload();
        };

        $scope.all_check_Emp = function () {
            var toggleStatuss = $scope.clientid;
            angular.forEach($scope.getclientlist, function (itm) {
                itm.selected = toggleStatuss;
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.getclientlist.some(function (options) {
                return options.selected;
            });


        };

        $scope.emptogchkbx = function () {
            $scope.employeeck = $scope.getclientlist.every(function (types) {
                return types.ismmclT_Id;
            });
        };


        //////end////

    }
})();
