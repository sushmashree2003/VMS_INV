
(function () {
    'use strict';
    angular
        .module('app')
        .controller('DCReportController', DCReportController);
    DCReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter'];
    function DCReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter) {

        $scope.isRequiredmi = function () {

            return !$scope.get_mi_list.some(function (options) {
                return options.miid;
            });

        };

        $scope.all_miid = function (ac) {
            $scope.allmi = ac;
            var toggleStatus = $scope.allmi;
            angular.forEach($scope.get_mi_list, function (itm) {
                itm.miid = toggleStatus;
            });
        };

        $scope.disposeQFlg = false;
        $scope.locationflg = false;
        $scope.selectionflag = "year";
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //-------------------------------------------------------------------
        $scope.onclickchange = function () {
            $scope.coYear = '';
            $scope.invmlO_Id = '';
            $scope.get_locations = [];
            $scope.selectionflag = "year";
            $scope.loaddata($scope.MI_Id, $scope.selectionflag)
        }
        //==========
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = paginationformasters;
        $scope.obj = {};
        $scope.loaddata = function () {

            $scope.search = "";
            var data = {
                "selectionflag": $scope.selectionflag,
                "MI_Id_new": $scope.MI_Id
            };
            apiService.create("DCReport/getloaddata", data).
                then(function (promise) {
                    $scope.get_AssetsReportdetails = promise.get_AssetsReportdetails;

                    $scope.staffcheck = promise.staffcheck;
                    if ($scope.MI_Id == undefined) {
                        $scope.get_mi_list = promise.get_mi_list;
                    }
                    if ($scope.staffcheck == 'Staff') {
                        $scope.MI_Id = promise.get_mi_list[0].MI_Id;
                    }

                });
        };



        $scope.reschedule1 = function (obj) {

            $scope.ttl = 0;

            //$('#popup111').modal('show');

            $scope.printstudents = [];
            angular.forEach(obj.LIST, function (dd) {
                $scope.ttl = $scope.ttl + dd.INVACI_CheckInQty;
                $scope.printstudents.push(dd);
                $scope.dcdate = dd.UpdatedDate;
                $scope.imagename = dd.MI_Logo;
                $scope.miname = dd.MI_Name;
            })

            $scope.tempto = [];
            angular.forEach($scope.printstudents, function (ddd) {

                if ($scope.tempto.length == 0) {
                    $scope.tempto.push({ INVMST_Id: ddd.INVMST_Id, INVMS_StoreName: ddd.INVMS_StoreName });
                }
                else if ($scope.tempto.length > 0) {
                    var already_cnt = 0;
                    angular.forEach($scope.tempto, function (opq1) {
                        if (opq1.INVMST_Id == ddd.INVMST_Id) {
                            already_cnt += 1;
                        }
                    })

                    if (already_cnt == 0) {
                        $scope.tempto.push({ INVMST_Id: ddd.INVMST_Id, INVMS_StoreName: ddd.INVMS_StoreName });
                    }

                }
            })

            $scope.temptorecby = [];
            $scope.temptorecbyname = '';
            angular.forEach($scope.printstudents, function (ddd) {

                if ($scope.temptorecby.length == 0) {
                    $scope.temptorecby.push({ INVACI_ReceivedBy: ddd.INVACI_ReceivedBy });
                    $scope.temptorecbyname = ddd.INVACI_ReceivedBy;
                }
                else if ($scope.temptorecby.length > 0) {
                    var already_cnt = 0;
                    angular.forEach($scope.temptorecby, function (opq1) {
                        if (opq1.INVACI_ReceivedBy == ddd.INVACI_ReceivedBy) {
                            already_cnt += 1;
                        }
                    })

                    if (already_cnt == 0) {
                        $scope.temptorecby.push({ INVACI_ReceivedBy: ddd.INVACI_ReceivedBy });
                        $scope.temptorecbyname = $scope.temptorecbyname + ' , ' + ddd.INVACI_ReceivedBy;
                    }

                }
            })



            $scope.decno = obj.INVACI_No;


            $('#popup1113').modal('show');

        }

        $scope.reschedule = function (obj) {

            $scope.ttl = 0;

            //$('#popup111').modal('show');

            $scope.printstudents = [];
            angular.forEach(obj.LIST, function (dd) {
                $scope.ttl = $scope.ttl + dd.INVACO_CheckOutQty;
                $scope.printstudents.push(dd);
                $scope.dcdate = dd.UpdatedDate;

                $scope.imagename = dd.MI_Logo;
                $scope.miname = dd.MI_Name;

            })

            $scope.tempto = [];
            angular.forEach($scope.printstudents, function (ddd) {

                if ($scope.tempto.length == 0) {
                    $scope.tempto.push({ INVMLO_Id: ddd.INVMLO_Id, INVMLO_LocationRoomName: ddd.INVMLO_LocationRoomName });
                }
                else if ($scope.tempto.length > 0) {
                    var already_cnt = 0;
                    angular.forEach($scope.tempto, function (opq1) {
                        if (opq1.INVMLO_Id == ddd.INVMLO_Id) {
                            already_cnt += 1;
                        }
                    })

                    if (already_cnt == 0) {
                        $scope.tempto.push({ INVMLO_Id: ddd.INVMLO_Id, INVMLO_LocationRoomName: ddd.INVMLO_LocationRoomName });
                    }

                }
            })

            $scope.temptorecby = [];
            $scope.temptorecbyname = '';
            angular.forEach($scope.printstudents, function (ddd) {

                if ($scope.temptorecby.length == 0) {
                    $scope.temptorecby.push({ INVACO_ReceivedBy: ddd.INVACO_ReceivedBy });
                    $scope.temptorecbyname = ddd.INVACO_ReceivedBy;
                }
                else if ($scope.temptorecby.length > 0) {
                    var already_cnt = 0;
                    angular.forEach($scope.temptorecby, function (opq1) {
                        if (opq1.INVACO_ReceivedBy == ddd.INVACO_ReceivedBy) {
                            already_cnt += 1;
                        }
                    })

                    if (already_cnt == 0) {
                        $scope.temptorecby.push({ INVACO_ReceivedBy: ddd.INVACO_ReceivedBy });
                        $scope.temptorecbyname = $scope.temptorecbyname + ' , ' + ddd.INVACO_ReceivedBy;
                    }

                }
            })



            $scope.decno = obj.INVACO_No;
            $scope.Note = obj.INVACO_CheckOutRemarks;
            $scope.Mode = obj.INVACO_MODE;
            $scope.Issued = obj.INVACO_ISSUED;
            $scope.Vehicalno = obj.INVACO_Vehicalno;
            $scope.Time = obj.INVACO_Time;

            $('#popup111').modal('show');

        }
        $scope.tempdc = [];
        //   ========================================== Assets Report
        $scope.getreport = function () {
            $scope.tempdc = [];
            $scope.submitted = true;
            //debugger;
            $scope.get_checkoutdclist = [];
            if ($scope.myForm.$valid) {
                $scope.miarray = [];
                angular.forEach($scope.get_mi_list, function (rr) {
                    if (rr.miid == true) {
                        $scope.miarray.push({ MI_Id: rr.MI_Id });
                    }

                })
                $scope.start_Date = $filter('date')($scope.startdate, "yyyy-MM-dd");
                $scope.end_Date = $filter('date')($scope.enddate, "yyyy-MM-dd");
                if ($scope.bw_dates == true) {
                    $scope.sDate = $scope.start_Date;
                    $scope.eDate = $scope.end_Date;
                }
                else {
                    $scope.sDate = "";
                    $scope.eDate = "";
                }

                console.log($scope.miarray);
                var data = {};



                data = {
                    "selectionflag": $scope.optionflag,
                    get_mi_arraylist: $scope.miarray,
                    "startdate": $scope.sDate,
                    "enddate": $scope.eDate,
                    "bwdateflag": $scope.bw_dates,
                };

                apiService.create("DCReport/getreport", data).then(function (promise) {

                    if (promise.get_AssetsReport.length > 0) {
                        $scope.get_AssetsReport = promise.get_AssetsReport;
                        if ($scope.optionflag == 'OUT' || $scope.optionflag == 'ISSUE' || $scope.optionflag == 'RECEIVED') {
                            $scope.get_checkoutdclist = $scope.get_AssetsReport;

                            $scope.tempdc = [];
                            angular.forEach($scope.get_checkoutdclist, function (ddd) {

                                if ($scope.tempdc.length == 0) {
                                    $scope.tempdc.push({
                                        INVACO_No: ddd.INVACO_No,
                                        INVACO_CheckOutRemarks: ddd.INVACO_CheckOutRemarks,
                                        INVACO_MODE: ddd.INVACO_MODE,
                                        INVACO_ISSUED: ddd.INVACO_ISSUED,
                                        INVACO_Vehicalno: ddd.INVACO_Vehicalno,
                                        INVACO_Time: ddd.INVACO_Time,
                                        invmlO_Status: ddd.invmlO_Status
                                    });
                                }
                                else if ($scope.tempdc.length > 0) {
                                    var already_cnt = 0;
                                    angular.forEach($scope.tempdc, function (opq1) {
                                        if (opq1.INVACO_No == ddd.INVACO_No) {
                                            already_cnt += 1;
                                        }
                                    })

                                    if (already_cnt == 0) {
                                        $scope.tempdc.push({
                                            INVACO_No: ddd.INVACO_No,
                                            INVACO_CheckOutRemarks: ddd.INVACO_CheckOutRemarks,
                                            INVACO_MODE: ddd.INVACO_MODE,
                                            INVACO_ISSUED: ddd.INVACO_ISSUED,
                                            INVACO_Vehicalno: ddd.INVACO_Vehicalno,
                                            INVACO_Time: ddd.INVACO_Time,
                                            invmlO_Status: ddd.invmlO_Status
                                        });
                                    }

                                }
                            })

                            $scope.mainarray = [];
                            angular.forEach($scope.tempdc, function (ss) {
                                var listdat = [];
                                angular.forEach($scope.get_checkoutdclist, function (ff) {
                                    if (ss.INVACO_No == ff.INVACO_No) {
                                        listdat.push(ff);
                                    }

                                })
                                ss.LIST = listdat;
                            })
                        }
                        else if ($scope.optionflag == 'IN') {
                            $scope.get_checkindc = $scope.get_AssetsReport;
                            $scope.tempdc = [];
                            angular.forEach($scope.get_checkindc, function (ddd) {

                                if ($scope.tempdc.length == 0) {
                                    $scope.tempdc.push({ INVACI_No: ddd.INVACI_No });
                                }
                                else if ($scope.tempdc.length > 0) {
                                    var already_cnt = 0;
                                    angular.forEach($scope.tempdc, function (opq1) {
                                        if (opq1.INVACI_No == ddd.INVACI_No) {
                                            already_cnt += 1;
                                        }
                                    })

                                    if (already_cnt == 0) {
                                        $scope.tempdc.push({ INVACI_No: ddd.INVACI_No });
                                    }

                                }
                            })

                            $scope.mainarray = [];
                            angular.forEach($scope.tempdc, function (ss) {
                                var listdat = [];
                                angular.forEach($scope.get_checkindc, function (ff) {
                                    if (ss.INVACI_No == ff.INVACI_No) {
                                        listdat.push(ff);
                                    }

                                })
                                ss.LIST = listdat;
                            })
                        }
                    }
                    else {
                        swal("No record found.... !");
                    }

                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.onrdochange = function () {

            $scope.get_AssetsReport = [];
            $scope.tempdc = [];
        }
        //======================================Print & Export to Excel
        $scope.exportToExcel = function (export_id) {
            var exportHref = "";
            exportHref = Excel.tableToExcel(export_id, 'printARL');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };

        $scope.printDataout = function () {
            var innerContents = "";
            innerContents = document.getElementById("asdf").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.printDatain = function () {
            var innerContents = "";
            innerContents = document.getElementById("asdf").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.printData = function () {
            var innerContents = "";
            if ($scope.optionflag == 'OUT' || $scope.optionflag == 'ISSUE' || $scope.optionflag == 'RECEIVED') {
                innerContents = document.getElementById("printARL").innerHTML;
            }
            else {
                innerContents = document.getElementById("printARL1").innerHTML;
            }



            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();

        };
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
    }
})();