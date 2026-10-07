(function () {
    'use strict';
    angular.module('app').controller('ISM_Task_DeviationController', ISM_Task_DeviationController);
    ISM_Task_DeviationController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', 'Excel', '$timeout', '$sce'];
    function ISM_Task_DeviationController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter, Excel, $timeout, $sce) {


        var paginationformasters = 10;

        $scope.obj = {};
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};

        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();

        $scope.getserverdate = function () {
            var xmlHttp;
            function srvTime() {
                try {

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
        $scope.newdate = new Date($scope.today);

        $scope.maxdate = new Date($scope.today);
        /* Load Data */
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            var data = {
                "DRApprovalTypeFlag": "DRApproval"
            }
            apiService.create("ISM_Task_Deviation/GetDetails", data).then(function (promise) {

                $scope.get_userEmplist = promise.get_userEmplist;
                $scope.departmentdropdown = promise.departmentList;
                $scope.designationdropdown = promise.designation;
                $scope.empdepartment = promise.empdepartment;
                $scope.deviationpercentage = promise.deviationpercentage;
                $scope.empname = promise.empname;
                $scope.roleflag = promise.roleflag;

                if ($scope.departmentdropdown.length > 0) {
                    angular.forEach($scope.departmentdropdown, function (uem) {
                        uem.selected = true;
                    });
                    $scope.departmentselectedAll = true;
                }
                if ($scope.designationdropdown.length > 0) {
                    angular.forEach($scope.designationdropdown, function (uem) {
                        uem.selected = true;
                    });
                    $scope.designationselectedAll = true;
                }
                if ($scope.get_userEmplist.length > 0) {
                    angular.forEach($scope.get_userEmplist, function (uem) {
                        uem.empck = true;
                    });
                    $scope.checkall = true;
                }

                $scope.Monthlist = promise.monthlist;
                $scope.Yearlist = promise.yearlist;

                //$scope.previousdate = new Date(promise.previousdate);
                //$scope.start_Date = new Date();
                //$scope.end_Date = new Date(promise.previousdate);


                //$timeout(function () { $scope.GetSavedDetails(); }, 1000);
                //added By sanjeev
               
               

            });
        };


        //*************************** DEVIATION REPORT *********************//        
       

        /* DEPARTMENT SELECTION */
        $scope.all_checkdept = function (departmentselectedAll) {
            var toggleStatus = departmentselectedAll;
            angular.forEach($scope.filterValue5, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.searchValueDesg = '';
            $scope.designationselectedAll = false;
            $scope.DeptSelectionDetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };

        $scope.togchkbxdept = function (groupType) {
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {
                return itm.selected;
            });
            $scope.searchValueDesg = '';
            $scope.designationselectedAll = false;
            $scope.DeptSelectionDetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.searchValueUEM = '';
            $scope.employeeid = [];
            $scope.checkall = false;
            $scope.get_deviationreport = [];
        };


        $scope.DeptSelectionDetails = function () {
            $scope.arrayuserEmp = [];
            $scope.designationdropdown = [];
            $scope.get_userEmplist = [];
            angular.forEach($scope.departmentdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserEmp.push(em);
                }
            });
            var data = {
              
                "departmentlist": $scope.arrayuserEmp
            };
            apiService.create("ISM_DailyReport_Approval/DeptSelectionDetails", data).then(function (promise) {
                if (promise.designation !== null && promise.designation.length > 0) {
                    $scope.designationdropdown = promise.designation;
                    if ($scope.designationdropdown.length > 0) {
                        angular.forEach($scope.designationdropdown, function (uem) {
                            uem.selected = true;
                        });
                        $scope.designationselectedAll = true;
                    }
                    $scope.DesgSelectionDetails();
                }
                else {
                    $scope.searchValueUEM = '';
                    $scope.employeeid = [];
                    $scope.checkall = false;
                    swal("No Record  Found..... !!");
                }
            });
        };

        $scope.obj.searchValueDeptd = '';
        $scope.filterValue4 = function (obj) {
            return angular.lowercase(obj.HRMDC_Name).indexOf(angular.lowercase($scope.obj.searchValueDeptd)) >= 0;
        };

        /* DESIGNATION SELECTION */
        $scope.all_checkdesg = function (designationselectedAll) {
            var toggleStatus = designationselectedAll;
            angular.forEach($scope.filterValue1, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.searchValueUEM = '';
            $scope.checkall = false;
            $scope.DesgSelectionDetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };

        $scope.togchkbxdesg = function (groupType) {
            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {
                return itm.selected;
            });
            $scope.searchValueUEM = '';
            $scope.checkall = false;
            $scope.DesgSelectionDetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };

        $scope.DesgSelectionDetails = function () {
            $scope.arrayuserEmp = [];
            $scope.get_userEmplist = [];
            angular.forEach($scope.designationdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserEmp.push(em);
                }
            });
            var data = {
                //"DRApprovalTypeFlag": "DRApproval",
                "designationlist": $scope.arrayuserEmp
            };
            apiService.create("ISM_DailyReport_Approval/DesgSelectionDetails", data).then(function (promise) {
                if (promise.get_userEmplist.length > 0) {
                    $scope.get_userEmplist = promise.get_userEmplist;
                    if ($scope.get_userEmplist.length > 0) {
                        angular.forEach($scope.get_userEmplist, function (uem) {
                            uem.empck = true;
                        });
                        $scope.checkall = true;
                    }
                }
                else {
                    $scope.searchValueUEM = '';
                    $scope.employeeid = [];
                    $scope.checkall = false;
                    swal("No Record  Found..... !!");
                }
            });
        };

        $scope.obj.searchValueDesgd = '';
        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.HRMDES_DesignationName).indexOf(angular.lowercase($scope.obj.searchValueDesgd)) >= 0 || angular.lowercase(obj.MI_Name).indexOf(angular.lowercase($scope.obj.searchValueDesgd)) >= 0;
        };

        /* EMPLOYEE SELECTION */
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

        $scope.obj.searchValueUEMd = '';
        $scope.filterValue2 = function (obj) {
            return angular.lowercase(obj.userEmpName).indexOf(angular.lowercase($scope.obj.searchValueUEMd)) >= 0 || angular.lowercase(obj.HRMD_DepartmentName).indexOf(angular.lowercase($scope.obj.searchValueUEMd)) >= 0;
        };

        //$scope.onchangestartdate = function () {
        //    $scope.end_Date = null;
        //    $scope.end_Date = $scope.start_Date;
        //    $scope.getsaveddetails = [];
        //    $scope.getsaveddetails();
        //};

        $scope.otherdaydatechange = function () {
            $scope.end_Date = null;
            $scope.start_Date = null;
        };

        // GET SEARCH DETAILS
        $scope.deviationReport = function () {
            if ($scope.myForm.$valid) {
                //$scope.headreport = [];
                var data = "";

                $scope.deptlist = [];
                $scope.desglist = [];
                $scope.emplist = [];

                angular.forEach($scope.departmentdropdown, function (d) {
                    if (d.selected === true) {
                        $scope.deptlist.push({ HRMDC_ID: d.HRMDC_ID });
                    }
                });

                angular.forEach($scope.designationdropdown, function (d) {
                    if (d.selected === true) {
                        $scope.desglist.push({ HRMDES_Id: d.HRMDES_Id });
                    }
                });

                angular.forEach($scope.get_userEmplist, function (d) {
                    if (d.empck === true) {
                        $scope.emplist.push({ HRME_Id: d.HRME_Id });
                    }
                });

                data = {
                   
                    "Fromdate": new Date($scope.start_Date).toDateString(),
                    "Todate": new Date($scope.end_Date).toDateString(),
                    "reporttype": $scope.todayorothers,
                    "deptlist": $scope.deptlist,
                    "desglist": $scope.desglist,
                    "emplist": $scope.emplist
                };

                apiService.create("ISM_Task_Deviation/getdeviationReport", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.get_deviationRemarksReport !== null && promise.get_deviationRemarksReport.length > 0) {
                            $scope.get_deviationRemarksReport = promise.get_deviationRemarksReport;

                        
                        } else {
                            swal("No Record Found.... !!");
                            $scope.get_deviationRemarksReport = [];
                        }
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        
        // save task date extention        $scope.submitted1 = false;        $scope.taskdateextention = function () {             $scope.DeviationtaskExtention = [];                           if ($scope.get_deviationRemarksReport != null && $scope.get_deviationRemarksReport.length > 0) {                    angular.forEach($scope.get_deviationRemarksReport, function (ss) {                        if (ss.checkedTask == true) {                            $scope.DeviationtaskExtention.push({                                ISMTCR_Id: ss.ISMTCR_Id,                                ISMTDE_TaskStartDate: ss.TaskstartDate,                                ISMTDE_TaskEndDate: ss.TaskendDate,                                HRME_Id:ss.HRME_Id,                               // ISMTDE_TaskExtstartDate: new Date(ss.Entendstartdate).toDateString(),                                ISMTDE_TaskExtendDate: new Date(ss.Entendenddate).toDateString(),                                ISMTDE_Remarks: ss.Extend_Remarks                                               });                        }                    })                }                if ($scope.DeviationtaskExtention != null && $scope.DeviationtaskExtention.length > 0) {                  
                    //if ($scope.myForm1.$valid) {
                        var data = {                            "DeviationtaskExtention": $scope.DeviationtaskExtention,                        }                        apiService.create("ISM_Task_Deviation/SaveDateExtention", data).then(function (promise) {                            //apiService.create("ISM_Task_Deviation/SaveDateExtention", data).then(function (promise) {                            if (promise.message == "save") {                                swal("Requested the Date Extention Successfully");                            }                            else if (promise.message == "notsave") {                                swal("Failed to Request the Date Extenstion ");                            }                            else {                                swal('Please Contcat Administrator');                            }                            $state.reload();                        });                    //}                    //else {                    //    $scope.submitted1 = true;
                    //}                }                else {                    swal('Please Select  Atleast one CheckBox');                }                                           };

     

       


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        //===========Clear Field
        $scope.Clear = function () {
            $state.reload();
        };

        $scope.isOptionsRequired = function () {
            return !$scope.get_userEmplist.some(function (options) {
                return options.empck;
            });
        };

        $scope.exportToExcel = function (export_id) {
            $scope.stdate = $filter('date')($scope.start_Date, 'dd-MM-yyyy');
            $scope.end = $filter('date')($scope.end_Date, 'dd-MM-yyyy');
            var excelname = 'CONSOLIDATED REPORT ' + $scope.stdate + ' TO ' + $scope.end;
            excelname = excelname.toUpperCase() + '.xls';
            //var printSectionId = export_id;
            var exportHref = Excel.tableToExcel('#detailedreport', $scope.routename);
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("detailedreportprint").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        var imagedownload = "";
        var docname = "";
        var studentreg = "";

        $scope.onviewpdf = function (filepath, filename) {
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            document.getElementById("pdfviewdd").innerHTML = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var htmlElements = '<embed id="pdfview" src="' + $scope.content + '"  style="width: 100%;" height="1000" type="application/pdf" />';
                    document.getElementById("pdfviewdd").innerHTML = htmlElements;
                    $('#showpdf').modal('show');
                });
        };

        $scope.downloaddirectimage = function (data, idd) {
            var studentreg = idd;
            $scope.imagedownload = data;
            imagedownload = data;
            $http.get(imagedownload, { responseType: "arraybuffer" }).success(function (data) {
                var anchor = angular.element('<a/>');
                var blob = new Blob([data]);
                anchor.attr({
                    href: window.URL.createObjectURL(blob),
                    target: '_blank',
                    download: studentreg
                })[0].click();
            });
        };


        $scope.previewimg = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalPreview').modal('show');

        };
        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalPreview').modal('show');

        };
       

     

        
        $scope.interacteddr = function () {
            return $scope.submitteddr;
        }

        $scope.PrintData = function () {
            var innerContents = document.getElementById("printdetails").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');

            popupWinindow.document.close();
        };
    }
})();