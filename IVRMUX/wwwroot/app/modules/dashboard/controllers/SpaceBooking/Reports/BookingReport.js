
(function () {
    'use strict';
    angular
        .module('app')
        .controller('BookingReportController', BookingReportController)

    BookingReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter', 'Excel', '$timeout']
    function BookingReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter, Excel, $timeout) {
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
       
        $scope.getroom = [];
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        // ============================selection of rooms   (ALL)========================//
        $scope.searchchkbx = "";
        $scope.all_checkC = function () {
            var checkStatus = $scope.usercheckC;
            var count = 0;
            angular.forEach($scope.roomname_list, function (itm) {
                itm.selected = checkStatus;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });           
        }
        //=============All(chk)
        $scope.all_checkCCCC = function () {         
            var mI_Id = $scope.MI_Id;
            var count = 0;
            angular.forEach($scope.Master_Institution, function (itm) {
                itm.selected = mI_Id;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
            if ($scope.MI_Id === true) {
                $scope.getroom();
            }
            else {
                $scope.roomname_list = [];
            }
        }
        $scope.togchkbxC = function () {
            $scope.usercheckC = $scope.roomname_list.every(function (options) {
                return options.selected;
            });
        }
        $scope.togchkbxCCCC = function () {
            $scope.MI_Id = $scope.Master_Institution.every(function (options) {
                return options.selected;
            });
            $scope.getroom();
        }
        $scope.isOptionsRequired = function () {
            return !$scope.roomname_list.some(function (options) {
                return options.selected;
            });
        }
        $scope.isOptionsRequiredtwo = function () {
            return !$scope.Master_Institution.some(function (options) {
                return options.selected;
            });
        }
        $scope.searchchkbx1 = function (obj) {
            return (angular.lowercase(obj.hrmR_RoomName)).indexOf(angular.lowercase($scope.searchchkbx1)) >= 0;
        }
        //---------------Load --data
        $scope.submitted = false;
        $scope.loaddata = function () {        
            var pageid = 2;
            apiService.getURI("SBReport/getdetails", pageid).then(function (promise) {
                     $scope.milist = promise.milist;
                $scope.MI_Id = promise.miid;
                $scope.roomname_list = promise.roomname_list;
                $scope.userflag = promise.userflag;
                $scope.Master_Institution = promise.master_Institution;
                              
            });
        }
        //--------------End-Load --data
        $scope.ddate = new Date();
        $scope.imgname = logopath;
        //---------------Get-Report   
        $scope.printflag = false;
        $scope.submitted = false;
        $scope.reportlist = false;
        $scope.get_report = function () {          
        $scope.selectedroomlist = [];
            $scope.bookinglist_Report = []; 
            $scope.Master_Institutionsss = [];
            if ($scope.myForm.$valid) {
                angular.forEach($scope.roomname_list, function (room) {
                    if (room.selected == true) {
                        $scope.selectedroomlist.push({ hrmR_Id: room.hrmR_Id });                        
                    }
                });
                angular.forEach($scope.Master_Institution, function (comp) {
                    if (comp.selected == true) {
                        $scope.Master_Institutionsss.push({ mI_IDS: comp.mI_Id });
                    }
                });
                var data = {
                    "fromdate1": new Date($scope.testfromdate).toDateString(),
                    "fromdate2": new Date($scope.testtodate).toDateString(),
                    "bookingtype": $scope.bookingtype,
                    "bookingstatus": $scope.bookingstatus,
                    "selectedroomlist": $scope.selectedroomlist,
                    "MI_IDList": $scope.Master_Institutionsss
                    //"MI_Id_new": $scope.MI_Id,
                }
                apiService.create("SBReport/booking_report", data)
                    .then(function (promise) {
                        if (promise.bookinglist_Report.length != null && promise.bookinglist_Report.length > 0) {
                            $scope.bookinglist_Report = promise.bookinglist_Report;                         
                            $scope.printflag = true;
                            $scope.reportlist = true;
                          // ----startmi_id                                                          
                        }
                        else {
                            swal('Record is Not Available!!');
                            $scope.printflag = false;
                            $scope.reportlist = false;
                        }
                    })
            }
            else {
                $scope.submitted = true;
            }
        }
        //-------------End--Get-Report
        $scope.getroom = function () {
            $scope.roomname_list = [];
            $scope.Master_Institutiontwo = [];
                angular.forEach($scope.Master_Institution, function (comp) {
                    if (comp.selected === true) {
                        $scope.Master_Institutiontwo.push({ mI_IDS: comp.mI_Id });
                    }
                });
           
            var data = {
                "MI_IDList": $scope.Master_Institutiontwo
            }
            apiService.create("SBReport/getroom", data).then(function (promise) {
                if (promise.roomname_list.length != null && promise.roomname_list.length > 0) {
                    $scope.roomname_list = promise.roomname_list;


                }
                else {
                    swal('Room Name Not Found !');
                    $scope.roomname_list = [];
                }
            });
        };
        //===========print===========//
        $scope.printData = function () {
            var innerContents = '';
            innerContents = document.getElementById("printtable").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/SpaceBookingReport/SBReportPdf.css" />'  +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        //==============End==============//
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //--------------Clear-field.....
        $scope.submitted = false;
        $scope.searchchkbx1 = "";
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.ExportToExcel = function (table) {
            debugger;
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }
        $scope.get_listdata = function () {
            $scope.bookinglist_Report = [];
            $scope.printflag = false;
            $scope.reportlist = false;
        }
    }
})();

