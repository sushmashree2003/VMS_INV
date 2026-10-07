
(function () {
    'use strict';
    angular
        .module('app')
        .controller('QRcodeNotPunchDeviationReportController', QRcodeNotPunchDeviationReportController);
    QRcodeNotPunchDeviationReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$compile'];
    function QRcodeNotPunchDeviationReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $compile) {

        var paginationformasters;
        $scope.multidepartment = false;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.obj = {};
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();

        $scope.loaddata = function () {
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("QRcodeNotPunchDeviationReport/getloaddata", pageid).then(function (promise) {
                $scope.building_type = promise.fill_buildings;
                $scope.fillroletype = promise.fillroletype;
            });
        };
        
        $scope.isOptionsRequired = function () {
            return !$scope.building_type.some(function (options) {
                return options.selected;
            });
        }
        $scope.isOptionsRequired1 = function () {
            return !$scope.Floors_type.some(function (options) {
                return options.selected;
            });
        }
        $scope.isOptionsRequired2 = function () {
            return !$scope.Area_types.some(function (options) {
                return options.selected;
            });
        }
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //validation end
       
        //all check button start
        $scope.all_check = function () {
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.building_type, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_Floors();
        }
        $scope.all_checkdep = function () {

            var toggleStatus = $scope.deptcheck;
            angular.forEach($scope.Floors_type, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_Areas();
        }
        $scope.all_checkdesg = function () {

            var toggleStatus = $scope.desgcheck;
            angular.forEach($scope.Area_types, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_employee();
        }
        //all-check button end
        // fill dep start
        $scope.get_Floors = function () {
            $scope.usercheck = $scope.building_type.every(function (options) {
                return options.selected;
            });
            $scope.deptcheck = "";
            $scope.desgcheck = "";

            $scope.groupidss=[];
            for (var i = 0; i < $scope.building_type.length; i++) {
                if ($scope.building_type[i].selected==true) {
                    $scope.groupidss.push({
                        HRMB_Id: $scope.building_type[i].hrmB_Id
                    });
                }
            }
            var data = {
                "multipletype": $scope.groupidss,
            }
            apiService.create("QRcodeNotPunchDeviationReport/get_Floors", data).
                then(function (promise) {
                    $scope.Floors_type = promise.fill_Floors;
                })
        }


   
        $scope.get_Areas = function () {
            $scope.usercheck = $scope.Floors_type.every(function (options) {
                return options.selected;
            });
            $scope.deptcheck = "";
            $scope.desgcheck = "";

            $scope.groupidsss = [];
            for (var i = 0; i < $scope.Floors_type.length; i++) {
                if ($scope.Floors_type[i].selected == true) {
                    $scope.groupidsss.push({
                        HRMF_Id: $scope.Floors_type[i].hrmF_Id
                    });
                }
            }
            var data = {
                "multipletypea": $scope.groupidsss,
            }
            apiService.create("QRcodeNotPunchDeviationReport/get_Areas", data).
                then(function (promise) {
                    $scope.Area_types = promise.fill_Areas;

                })
        }
        //-----------Employee list
        $scope.Employeelistchange = function (role_id) {
            $scope.emp_deatils = [];
            var data = {
                "IVRMRT_Id": role_id,
            }
            apiService.create("QRcodeNotPunchDeviationReport/getEmployeeName", data).
                then(function (promise) {
                    if (promise.emp_deatils != null && promise.emp_deatils.length > 0) {
                        $scope.emp_deatils = promise.emp_deatils;
                    }
                    else {
                        swal("No Record Found....")
                    }

                })
        }
        $scope.all_checkKK = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.emp_deatils, function (uem) {
                uem.id = toggleStatus;
            });
        };
        $scope.togchkbx = function () {
            $scope.checkall = $scope.emp_deatils.every(function (options) {
                return options.id;
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.emp_deatils.some(function (options) {
                return options.id;
            });
        };
        //*************************** DEVIATION REPORT *********************//        
        $scope.deviationReport = function () {
            if ($scope.myForm.$valid) {
                $scope.arrayuserEmp = [];
             $scope.building_typeTemp = [];
                $scope.Area_typesTemp = [];
                $scope.Floors_typeTemp = [];
                if ($scope.emp_deatils != null && $scope.emp_deatils.length > 0) {
                    angular.forEach($scope.emp_deatils, function (em) {
                        if (em.id === true) {
                            $scope.arrayuserEmp.push({ UserId: em.Id });
                        }
                    });
                }
                if ($scope.building_type != null && $scope.building_type.length > 0) {
                    angular.forEach($scope.building_type, function (eyym) {
                        if (eyym.selected === true) {
                            $scope.building_typeTemp.push({
                                HRMB_Id: eyym.hrmB_Id
                            });
                        }
                    });
                }
                if ($scope.Area_types != null && $scope.Area_types.length > 0) {
                    angular.forEach($scope.Area_types, function (pp) {
                        if (pp.selected === true) {
                            $scope.Area_typesTemp.push({
                                HRMAR_ID: pp.hrmaR_Id
                            });
                        }
                    });
                }
              
                //Floors_type
                if ($scope.Floors_type != null && $scope.Floors_type.length > 0) {
                    angular.forEach($scope.Floors_type, function (pp) {
                        if (pp.selected === true) {
                            $scope.Floors_typeTemp.push({
                                HRMF_Id: pp.hrmF_Id
                            });
                        }
                    });
                }
                

                var data = {
                    "startdate": new Date($scope.start_Date).toDateString(),
                    "enddate": new Date($scope.end_Date).toDateString(),
                    "arrayuserEmp": $scope.arrayuserEmp,
                    "multipletype": $scope.building_typeTemp ,
                    "multitypesAreaee": $scope.Area_typesTemp ,
                    "multipletypea": $scope.Floors_typeTemp,
                    
                };
                apiService.create("QRcodeNotPunchDeviationReport/QRcodedeviation", data).then(function (promise) {

                    $scope.get_deviationreport_Temp = [];
                    $scope.get_deviationreport = [];
                    $scope.get_deviationreport_Temp = promise.get_deviationreport;
                    if ($scope.get_deviationreport_Temp != null && $scope.get_deviationreport_Temp.length > 0) {
                        for (var t = 0; t < $scope.get_deviationreport_Temp.length; t++) {
                            $scope.get_deviationreport.push({
                                HRMARACTT_Time: moment($scope.get_deviationreport_Temp[t].HRMARACTT_Time, 'HH:mm a').format(),
                                HRSTARACTRCD_Time: moment($scope.get_deviationreport_Temp[t].HRSTARACTRCD_Time, 'HH:mm a').format(),
                                HRMB_BuildingName: $scope.get_deviationreport_Temp[t].HRMB_BuildingName,
                                UserName: $scope.get_deviationreport_Temp[t].UserName,
                                HRMF_FloorName: $scope.get_deviationreport_Temp[t].HRMF_FloorName,
                                HRMAR_AreaName: $scope.get_deviationreport_Temp[t].HRMAR_AreaName,
                                HRMARACT_ActivityName: $scope.get_deviationreport_Temp[t].HRMARACT_ActivityName,
                                Deviation: $scope.get_deviationreport_Temp[t].Deviation,
                                status: $scope.get_deviationreport_Temp[t].status,
                                HRMARACTT_ActivityDuration: $scope.get_deviationreport_Temp[t].HRMARACTT_ActivityDuration,
                                HRMARACTSTMAP_Date: new Date($scope.get_deviationreport_Temp[t].HRMARACTSTMAP_Date),
                            });
                        }
                    }
                    else {
                        swal("No Record  Found..... !!");
                        $scope.get_deviationreport = "";
                    }

                });
            }
            else {
                $scope.submitted = true;
            }
            
        };

      

        $scope.SendMailFunction = function () {
                var Template = document.getElementById("empmailPP").innerHTML;
                var data = {
                    "Template": Template,
                };
            apiService.create("QRcodeNotPunchDeviationReport/SendMail", data).then(function (promise) {

                swal('Email sent successfully');
                $state.reload();
            });
        };


        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printDeviation").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.exportToExcel = function (export_id) {
            var exportHref = Excel.tableToExcel(export_id, 'printDeviation');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.Clear = function () {
            $scope.get_deviationreport = "";
            $scope.emp_deatils = "";
            $scope.checkall = "";
            $scope.start_Date = "";
            $scope.end_Date = "";
            $scope.loaddata();
        };
        
    }
})();