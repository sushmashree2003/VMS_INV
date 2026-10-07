(function () {
    'use strict';
    angular.module('app').controller('YearplanreportapprovalController', YearplanreportapprovalController)
    YearplanreportapprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'Excel', 'apiService', '$timeout', '$http', 'superCache', '$filter', '$q']
    function YearplanreportapprovalController($rootScope, $scope, $state, $location, Flash, appSettings, Excel, apiService, $timeout, $http, superCache, $filter, $q) {
        var paginationformasters;
        var copty;
        $scope.getchecklistarray = [];
        $scope.cateviewdetails = [];
        $scope.cateviewdetails1 = [];
        $scope.obj = {}
        $scope.taskdetails = false;
        $scope.taskdetails1 = false;
        $scope.departmenteffort = false;
        $scope.departmentempeffort = false;
        $scope.indemptaskdetails = false;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.submitted = false;
        $scope.objques = {};
        $scope.gridflag = false;
        // Master Type
        //$scope.onloadmonths = function () {
        //    $scope.gridflag = true;
        //    var pageid = 2;
        //    apiService.getURI("ModuleWiseFeedbackReport/OnLoadClient", pageid).then(function (promise) {
        //        if (promise !== null) {
        //            $scope.hr_masteryear = promise.hr_masteryear;
        //        } 
        //    });

        //};
        /* Load Data */
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 5;

            $scope.currentPage1 = 1;
            $scope.itemsPerPage2 = 10;
            $scope.currentPage2 = 1;

            $scope.currentPage3 = 1;
            $scope.itemsPerPage3 = 10;
            $scope.currentPage3 = 1;
           

            $scope.page = "page";
            $scope.page1 = "page1";
            $scope.page2 = "page2";
            $scope.pageapp = "pageapp";
            $scope.search = "";
            var pageid = 2;
            var data = {
                "Id": pageid
            }
            apiService.create("ISMImport/OnloadApproval", data).then(function (promise) {

                //$scope.getuserName = promise.get_employeelist;
                $scope.instiutionlist = promise.get_taskmodule;
                

            });
        };
      

        $scope.getreport = function () {
           $scope.getchecklistarray = [];
           $scope.get_effortdetails = [];
          $scope.get_taskdetails= [];
            $scope.TotalTime = 0;
            $scope.TotalTime1 = 0;
            $scope.TotalTimedept = 0;
            $scope.OverallEffort = 0;
            if ($scope.myForm.$valid) {

                var data = {
                    "Fromdate": new Date($scope.fromdate).toDateString(),
                    "Todate": new Date($scope.todate).toDateString(),
                    "HRME_Id": $scope.HRME_Id.hrme_id,
                    "HRMD_Id": $scope.hrmD_Id,

                };

                apiService.create("ISMImport/getyearplannerreport", data).then(function (promise) {
                    if (promise.get_plannerdetails.length > 0 && promise.get_plannerdetails != null) {
                        $scope.getchecklistarray = promise.get_plannerdetails;

                                    if (promise.get_effortdetails.length > 0 && promise.get_effortdetails != null) {
                        $scope.get_effortdetails = promise.get_effortdetails;
                       
                        angular.forEach($scope.get_effortdetails, function (ss) {                            $scope.TotalTime = $scope.TotalTime + ss.Time;                        })
                        $scope.TotalTime = Math.round($scope.TotalTime);
                    }

                    if (promise.get_taskdetails.length > 0 && promise.get_taskdetails != null) {
                        $scope.get_taskdetails = promise.get_taskdetails;

                        angular.forEach($scope.get_taskdetails, function (ss) {                            $scope.TotalTime1 = $scope.TotalTime1 + ss.Time;                        })
                        $scope.TotalTime1 = Math.round($scope.TotalTime1);
                    }

                        if (promise.get_extratasklist.length > 0 && promise.get_extratasklist != null) {
                            $scope.get_extratasklist = promise.get_extratasklist;

                            angular.forEach($scope.get_extratasklist, function (ss) {                                $scope.TotalTimedept = $scope.TotalTimedept + ss.Time;                            })
                            $scope.TotalTimedept = Math.round($scope.TotalTimedept);
                        }

                        //overall planner effort
                        $scope.OverallEffort = Math.round($scope.TotalTime + $scope.TotalTime1);

                        $scope.get_Assignedtasklist = promise.get_Assignedtasklist
                    }
                   
                    
                    else {
                        swal("No Tasks Found For Selected Dates", "", "warning")
                        $state.reload();

                    }
                    

                    

                });
                
            }
           
            else {
                $scope.submitted = true;
            }
            //$scope.OverallEffort = Math.round($scope.TotalTime + $scope.TotalTime1);
        };

        $scope.Approveyearplan = function (falg) {            $scope.approvalflg = falg;            $scope.yearplanapproval = [];            if ($scope.myForm.$valid) {                if ($scope.getchecklistarray != null && $scope.getchecklistarray.length > 0) {                    angular.forEach($scope.getchecklistarray, function (ss) {                        if (ss.usercheck == true) {                            $scope.yearplanapproval.push({                                ISMTAPL_Id: ss.ISMTAPL_Id,                            });                        }                    })                }                if ($scope.yearplanapproval != null && $scope.yearplanapproval.length > 0) {
                    var data = {                        "yearplannerapproval": $scope.yearplanapproval,                        "approvalflg": $scope.approvalflg                    }                    apiService.create("ISMImport/yearplannerapp", data).then(function (promise) {                        if (promise.message == "saved" && $scope.approvalflg == "1") {                            swal('The Year Plan tasks have been successfully approved.', "", "success");                         }                        else if (promise.message == "saved" && $scope.approvalflg == "0") {                            swal('The Year Plan tasks have been successfully rejected.', "", "success");                          }                        else if (promise.message == "failed") {                            swal("failed to save", "", "error");                        }                        else {                            swal('Please Contcat Administrator');                        }                        $state.reload();                    });                }                else if ($scope.approvalflg == "1") {                    swal('Please Select  Atleast One CheckBox For Approval ');                }                else if ($scope.approvalflg == "0")                {                    swal('Please Select  Atleast One CheckBox For Rejection ');
                }            }            else {                $scope.submitted = true;            }        };

        $scope.institutionlist = function () {
            $scope.hrmD_Id = '0';
            $scope.get_department = [];
                var data = {
                    "MI_Id": $scope.MI_Id,
                };
                apiService.create("ISMImport/department", data).then(function (promise) {
                    //if (promise != null)
                    //{
                        if (promise.get_department != null && promise.get_department.length > 0) {
                            $scope.get_department=promise.get_department;
                        }
                        else {
                            swal("No Departments Found For Selected Company");
                        }
                    
                    //}

                });

        };
        $scope.deptemplist = function () {
            $scope.getuserName = [];
            $scope.HRME_Id = '';
            var data = {
                "MI_Id": $scope.MI_Id,
                "HRMD_Id": $scope.hrmD_Id,
            };
            apiService.create("ISMImport/Depemplist", data).then(function (promise) {
                //if (promise != null) {
                if (promise.get_employeelist != null && promise.get_employeelist.length > 0) {
                    $scope.getuserName = promise.get_employeelist;
                }
                else {
                    swal("No Employees Found for Selected Department"); 
                }
                //}

            });

        };


        $scope.Viewtaskdetails = function (data) {
            $scope.cateviewdetails = [];
            $scope.cateviewdetailsdea = [];
            $scope.taskdetails = true;

          

            $('#viewcataskdetails').modal('hide');
           
            angular.forEach($scope.getchecklistarray, function (dd) {                               if (dd.ISMMTCAT_Id == data.ISMMTCAT_Id) {                    $scope.cateviewdetails.push(dd);
                }                  });
            angular.forEach($scope.cateviewdetails, function (ss) {                if (ss.Created_HRME_ID == data.Created_HRME_ID && ss.Assgin_HRME_ID == data.Assgin_HRME_ID) {                    $scope.cateviewdetailsdea.push(ss);
                }                          });
            if ($scope.cateviewdetailsdea != null && $scope.cateviewdetailsdea.length > 0)
            {
                $('#viewcataskdetails').modal('show');
            }
            console.log("data", $scope.cateviewdetailsdea);
           

        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.Viewsaaigntaskdetails = function (data) {
            $scope.MainSelected_Hrmeid = $scope.HRME_Id.hrme_id;
            $scope.cateviewdetails1 = [];
            $scope.cateviewdetailsdea1 = [];
            $scope.taskdetails1 = true;

           

            $('#viewcataskdetails1').modal('hide');

            angular.forEach($scope.getchecklistarray, function (dd) {                              if (dd.ISMMTCAT_Id == data.ISMMTCAT_Id) {                    $scope.cateviewdetails1.push(dd);
                }            });
            angular.forEach($scope.cateviewdetails1, function (ss) {                if (ss.Created_HRME_ID == data.Created_HRME_ID && ss.Created_HRME_ID != $scope.MainSelected_Hrmeid ) {                    $scope.cateviewdetailsdea1.push(ss);
                }                           });

            if ($scope.cateviewdetailsdea1 != null && $scope.cateviewdetailsdea1.length > 0) {
                $('#viewcataskdetails1').modal('show');
            }
            console.log("data", $scope.cateviewdetailsdea);


        };


        $scope.Viewdeparteffort = function () {
            $scope.departmentempeffort = false;
            if ($scope.get_extratasklist != null && $scope.get_extratasklist.length > 0)
            {
                $scope.departmentempeffort = true;

                //$('#departmenteffort').modal('show');
                $('#departmentempeffort').modal('show');
            }
            

        };


        $scope.Viewindividualteffort = function (data) {
            $scope.departmenteffort = false;
            $scope.Totalindieffort = 0;
            $scope.cateviewindividualdetails = [];
            $('#departmenteffort').modal('hide');
            if ($scope.get_Assignedtasklist != null && $scope.get_Assignedtasklist.length > 0) {

                angular.forEach($scope.get_Assignedtasklist, function (dd) {                    if (data.HRME_ID == dd.Assgin_HRME_ID) {                        $scope.Totalindieffort = $scope.Totalindieffort + dd.Time;                        $scope.cateviewindividualdetails.push(dd);
                    }                });

                $scope.Totalindieffort = Math.round($scope.Totalindieffort);
                if ($scope.cateviewindividualdetails != null && $scope.cateviewindividualdetails.length > 0) {
                    $scope.departmenteffort = true;
                    $('#departmentempeffort').modal('hide');
                    $('#departmenteffort').modal('show');
                }


               
            }


        };



        $scope.Viewinddepataskdetails = function (obj) {
            $scope.assignhrmeId = obj.Assgin_HRME_ID;
            $scope.cretehrmeId = obj.Created_HRME_ID;
            $scope.taskempdetails = [];
            $scope.taskempdetails1 = [];
            $('#indemptaskdetails').modal('hide');
            $scope.indemptaskdetails = false;
            var data = {
                "MI_Id": $scope.MI_Id,
                "HRME_Id": obj.Assgin_HRME_ID,
                "Fromdate": new Date($scope.fromdate).toDateString(),
                "Todate": new Date($scope.todate).toDateString(),
            };
            apiService.create("ISMImport/Depemptasklist", data).then(function (promise) {
                //if (promise != null) {
                if (promise.get_taskproject != null && promise.get_taskproject.length > 0) {
                    $scope.get_taskproject = promise.get_taskproject;

                    if ($scope.cretehrmeId == $scope.assignhrmeId)
                    {
                        angular.forEach($scope.get_taskproject, function (dd) {                            if (dd.ISMMTCAT_Id == obj.ISMMTCAT_Id) {                                $scope.taskempdetails1.push(dd);
                            }                        });
                        angular.forEach($scope.taskempdetails1, function (ss) {                            if (ss.Created_HRME_ID == obj.Created_HRME_ID && ss.Assgin_HRME_ID == obj.Assgin_HRME_ID) {                                $scope.taskempdetails.push(ss);
                            }                        });

                    }

                    else if ($scope.cretehrmeId != $scope.assignhrmeId) {

                        angular.forEach($scope.get_taskproject, function (dd) {                            if (dd.ISMMTCAT_Id == obj.ISMMTCAT_Id) {                                $scope.taskempdetails1.push(dd);
                            }                        });
                        angular.forEach($scope.taskempdetails1, function (ss) {                            if (ss.Created_HRME_ID == obj.Created_HRME_ID && ss.Created_HRME_ID != $scope.assignhrmeId) {                                $scope.taskempdetails.push(ss);
                            }                        });
                    }
                   
                    if ($scope.taskempdetails != null && $scope.taskempdetails.length > 0) {
                        $scope.indemptaskdetails = true;
                        $('#indemptaskdetails').modal('show');
                    }
                }


            });

        };

        
        $scope.OnChangeBehaviourEmployee = function (field) {
            $scope.getchecklistarray = [];
            $scope.fromdate = '';
            $scope.todate = '';
        };
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.getchecklistarray, function (itm) {
                itm.usercheck = toggleStatus;

            });
        }

        $scope.optionToggled = function () {

            $scope.all = $scope.getchecklistarray.every(function (options) {

                return options.usercheck;
            });
        }

        $scope.searchValueDept = '';
        $scope.filterValue4 = function (obj) {
            return angular.lowercase(obj.HRMDC_Name).indexOf(angular.lowercase($scope.searchValueDept)) >= 0;
        };

        $scope.printData = function () {
            var innerContents = document.getElementById("yearplanreport").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();

            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();

        }
        //$scope.exportToExcel = function (tabel1) {
        //    var exportHref = Excel.tableToExcel(tabel1, 'sheet name');
        //    $timeout(function () { location.href = exportHref; }, 100);
        //}
        $scope.exportToExcel = function (tabel1) {
            var excelnamemain = "Year Plan Report";
            var printSectionId = '#yearplanreport';
            excelnamemain = excelnamemain + '' + '.xls';
            var exportHref = Excel.tableToExcel(printSectionId, 'Student Register Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelnamemain;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);

        };
        $scope.Clear = function () {
            $state.reload();
        };
    }
})();