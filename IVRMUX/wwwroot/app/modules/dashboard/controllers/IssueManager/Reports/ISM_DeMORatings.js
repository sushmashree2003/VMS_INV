(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_DeMORatingsController', ISM_DeMORatingsController);
    ISM_DeMORatingsController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter', '$compile'];
    function ISM_DeMORatingsController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter, $compile) {
       // $scope.departmentdropdown = [];
       // $scope.designationdropdown = [];
        $scope.empdRawReports = [];
        $scope.gridview = false;
        var paginationformasters;
        $scope.multidepartment = false;
        $scope.IMFY_FinancialYear = "";
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 10;
        }
        $scope.obj = {};
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));

        $scope.disableemp = false;
        $scope.checkallemp = false;
        $scope.taskeffort = [];
        $scope.submitted = false;
        $scope.ismtcrastO_AssignedDate = new Date();
        $scope.ismtcrtrtO_TransferredDate = new Date();
        $scope.EmpD = true;
        $scope.page = "page"; $scope.obj.allLead = false;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 10; $scope.searchValueNew = "";
        $scope.finacialYear = []; $scope.Months = []; $scope.product_list_dd = [];
        $scope.loaddata = function () {
            $scope.currentPageL = 1;
            $scope.itemsPerPageL = 10;
            $scope.currentPage = 1; $scope.currentPage1 = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.itemsPerPage1 = paginationformasters;

            $scope.finacialYear = [{ yearid: 1, imfY_FinancialYear: '2016', start: new Date('01/01/1016'), end: new Date('12/31/1016') },
                { yearid: 2, imfY_FinancialYear: '2017', start: new Date('01/01/2017'), end: new Date('12/31/2017') },
                { yearid: 3, imfY_FinancialYear: '2018', start: new Date('01/01/2018'), end: new Date('12/31/2018') },
                { yearid: 4, imfY_FinancialYear: '2019', start: new Date('01/01/2019'), end: new Date('12/31/2019') },
                { yearid: 5, imfY_FinancialYear: '2020', start: new Date('01/01/2020'), end: new Date('12/31/2020') },
                { yearid: 6, imfY_FinancialYear: '2021', start: new Date('01/01/2021'), end: new Date('12/31/2021') },
                { yearid: 7, imfY_FinancialYear: '2022', start: new Date('01/01/2022'), end: new Date('12/31/2022') },
                { yearid: 8, imfY_FinancialYear: '2023', start: new Date('01/01/2023'), end: new Date('12/31/2023') },
                { yearid: 9, imfY_FinancialYear: '2024', start: new Date('01/01/2024'), end: new Date('12/31/2024') },
                { yearid: 10, imfY_FinancialYear: '2025', start: new Date('01/01/2025'), end: new Date('12/31/2025') },
                { yearid: 11, imfY_FinancialYear: '2026', start: new Date('01/01/2026'), end: new Date('12/31/2026') },
                { yearid: 12, imfY_FinancialYear: '2027', start: new Date('01/01/2027'), end: new Date('12/31/2027') },
                { yearid: 13, imfY_FinancialYear: '2028', start: new Date('01/01/2028'), end: new Date('12/31/2028') },
                { yearid: 14, imfY_FinancialYear: '2029', start: new Date('01/01/2029'), end: new Date('12/31/2029') },
                { yearid: 15, imfY_FinancialYear: '2030', start: new Date('01/01/2030'), end: new Date('12/31/2030') }]






            $scope.search = "";
            var pageid = 2;
            apiService.getURI("ISM_MemoNoticeReport/getdetails", pageid).then(function (promise) {
               // $scope.get_userEmplist = promise.get_userEmplist;
                $scope.departmentdropdown = promise.departmentList;
               // $scope.designationdropdown = promise.designation;
                $scope.empdepartment = promise.empdepartment;
                $scope.empname = promise.empname;
                $scope.roleflag = promise.roleflag;
                //if ($scope.departmentdropdown.length > 0) {
                //    angular.forEach($scope.departmentdropdown, function (uem) {
                //        uem.selected = true;
                //    });
                //    $scope.departmentselectedAll = true;
                //}
                //if ($scope.designationdropdown.length > 0) {
                //    angular.forEach($scope.designationdropdown, function (uem) {
                //        uem.selected = true;
                //    });
                //    $scope.designationselectedAll = true;
                //}
                //if ($scope.get_userEmplist.length > 0) {
                //    angular.forEach($scope.get_userEmplist, function (uem) {
                //        uem.empck = true;
                //        uem.selected = false;
                //        uem.ICDR_DemoDate = new Date();
                //    });
                //    $scope.checkall = true;
                //}
                //viewdetails


                  //  $scope.finacialYear=promise.viewdetails;    
                   $scope.Months = promise.arrayuserEmp;
                $scope.product_list_dd = promise.memonoticereport;
                $scope.empdRawReports = promise.empdRawReports;
            });
        };
        $scope.isErrorMessageVisible = true; // Initially, show the error message


        $scope.isErrorMessageVisible1 = true; // Initially, show the error message

        // Watch the state of individual checkboxes and the "All Department" checkbox
        $scope.$watch('designationselectedAll', function () {
            $scope.updateFormValidity();
        });

        $scope.$watch('designationdropdown', function () {
            $scope.updateFormValidity();
        }, true); // Use deep watch to detect changes in individual department checkboxes

        // Function to check form validity
        $scope.updateFormValidity = function () {
            if ($scope.designationdropdown != null && $scope.designationdropdown.length > 0 && $scope.designationdropdown != undefined) {
                // Check if any checkbox (either individual or "All Department") is selected
                $scope.isErrorMessageVisible1 = !($scope.designationselectedAll || $scope.designationdropdown.some(function (user) {
                    return user.selected;
                }));
            }
        }
        //// Watch for changes in department selection and all department checkbox
        //$scope.$watch('departmentselectedAll', function () {
        //    $scope.updateFormValidity1();
        //});

        //$scope.$watch('departmentdropdown', function () {
        //    $scope.updateFormValidity1();
        //}, true);
        //$scope.updateFormValidity1 = function () {
        //    const isDepartmentValid = $scope.departmentselectedAll || $scope.departmentdropdown.some(function (user) {
        //        return user.selected;
        //    });

        //    // Only hide error message if both department and designation are valid
        //    $scope.isErrorMessageVisible = !(isDepartmentValid);
        //}


        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.filterValue3, function (uem) {
                uem.empck = toggleStatus;
            });
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };
        $scope.togchkbx = function () {
            $scope.checkall = $scope.get_userEmplist.every(function (options) {
                return options.empck;
            });
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };

        //=============================for grid check box selection
        $scope.togglecheckboxAllDept = function (checkallemp) {
            var toggleStatus = checkallemp;
            angular.forEach($scope.DeptdeviationRemarksReport, function (slc) {
                slc.checked = toggleStatus;
            });
            $scope.DeptselectionHeaddetails(checkallemp);
        };

        $scope.togglecheckboxAll = function (checkallemp) {
            var toggleStatus = checkallemp;
            angular.forEach($scope.employeeid, function (slc) {
                slc.checked = toggleStatus;
            });
        };
        $scope.optionToggled = function () {
            $scope.checkallemp = $scope.employeeid.every(function (options) {
                return options.checked;
            });
        };
        $scope.optionToggledNew = function () {
            $scope.checkallemp = $scope.get_userEmplist.every(function (options) {
                return options.selected;
            });
        };
        $scope.optionToggleddept = function (user) {
            $scope.checkalldept = $scope.DeptdeviationRemarksReport.every(function (options) {
                return options.checked;
            });
            $scope.DeptselectionHeaddetails(user);
        };
        //*************************** Department REPORT *********************//        
        $scope.all_checkdept = function (departmentselectedAll) {
            var toggleStatus = departmentselectedAll;
            angular.forEach($scope.filterValue5, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.searchValueDesg = '';
            $scope.designationselectedAll = false;
            $scope.Deptselectiondetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };

        $scope.togchkbxdept = function () {
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {
                return itm.selected;
            });
            $scope.searchValueDesg == '';
            $scope.designationselectedAll = false;
            $scope.Deptselectiondetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.searchValueUEM = '';
            $scope.employeeid = [];
            $scope.checkall = false;
            $scope.get_deviationreport = [];
           // $scope.get_depart();
            
        };

        $scope.get_depart = function () {
            var ids1 = [];
            angular.forEach($scope.departmentdropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids1.push(grp_t.HRMDC_ID);
                }
            });
            var data = {
                MI_Id_IdList: ids1,
            };
            apiService.create("ISM_Client_Project/get_depart", data).
                then(function (promise) {
                    if (promise.designationdropdown !== null && promise.departmentdropdown.length > 0) {
                        $scope.designationdropdown = promise.departmentdropdown;
                    }
                });
        };

        //*************************** Designation REPORT *********************//        
        $scope.all_checkdesg = function (designationselectedAll) {
            var toggleStatus = designationselectedAll;
            angular.forEach($scope.filterValue11, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.searchValueUEM = '';
            $scope.checkall = false;
            $scope.Desgselectiondetails();
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
            $scope.Desgselectiondetails();
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            $scope.get_deviationreport = [];
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };

        $scope.viewresponsetask = function (taskobj) {
            var data = {
                "HRME_Id": taskobj.hrmE_ID,
                "ISMEMN_No": taskobj.ismemN_No
            };
            apiService.create("ISM_PlannerDeptDeviationReport/gettaskdetailsreport", data).
                then(function (promise) {
                    if (promise.get_taskdetails !== null && promise.get_taskdetails.length > 0) {
                        $('#myModalResponse').modal('show');
                        $scope.get_taskdetails = promise.get_taskdetails;
                    }
                    else {
                        $('#myModalResponse').modal('hide');
                        $scope.get_taskdetails = [];
                        swal("No issues pednding for these dates!!");
                    }
                });
        };

        $scope.viewresponsememo = function (taskobj) {
            var data = {
                "enddate": new Date($scope.end_Date).toDateString(),
                "HRME_Id": taskobj.HRME_Id,
                "type": false
            };
            apiService.create("ISM_MemoNoticeReport/gettaskdetails", data).
                then(function (promise) {
                    if (promise.get_taskdetails !== null && promise.get_taskdetails.length > 0) {
                        $scope.get_taskdetails = promise.get_taskdetails;
                    }
                });
        };
        $scope.toggleAll = function () {
            var toggleStatus = $scope.obj.allLead;
            angular.forEach($scope.get_userEmplist, function (itm) {
                itm.selected = toggleStatus;

            });
        }
        $scope.closeimg = function () {
            $('#myModalResponse').modal('hide');
            $('.modal-backdrop').remove();
        };

        $scope.isOptionsRequired = function () {
            return !$scope.get_userEmplist.some(function (options) {
                return options.empck;
            });
        };
        $scope.isOptionsRequired11 = function () {
            return !$scope.departmentdropdown.some(function (user) {
                return user.selected;
            });
        };
        $scope.searchValueDesg = '';
        $scope.filterValue1122 = function (obj) {
            return angular.lowercase(obj.HRMDES_DesignationName).indexOf(angular.lowercase($scope.searchValueDesg)) >= 0 || angular.lowercase(obj.MI_Name).indexOf(angular.lowercase($scope.searchValueDesg)) >= 0;
        };

        $scope.searchValueDept = '';
        $scope.filterValue4 = function (obj) {
            return angular.lowercase(obj.HRMDC_Name).indexOf(angular.lowercase($scope.searchValueDept)) >= 0;
        };

        $scope.searchValueUEM = '';
        $scope.filterValue2 = function (obj) {
            return angular.lowercase(obj.userEmpName).indexOf(angular.lowercase($scope.searchValueUEM)) >= 0 || angular.lowercase(obj.HRMD_DepartmentName).indexOf(angular.lowercase($scope.searchValueUEM)) >= 0;
        };

        $scope.Depatdeviation = function () {
            $scope.DeptdeviationRemarksReport = [];
            $scope.employeeid = [];
            if ($scope.DepD === true) {
                $scope.EmpD = false;
                if ($scope.designationdropdown.length > 0) {
                    angular.forEach($scope.designationdropdown, function (uem) {
                        uem.selected = true;
                    });
                    $scope.designationselectedAll = true;
                }
               // $scope.Desgselectiondetails();
                $scope.disableemp = true;
                $scope.employeeid = [];
            }
            else {
                $scope.DeptdeviationRemarksReport = [];
                $scope.disableemp = false;
            }
        };

        $scope.Empdeviation = function () {
            if ($scope.EmpD === true) {
                $scope.DepD = false;
                $scope.disableemp = false;
            }
        };

        $scope.DeptselectionHeaddetails = function (user) {
            $scope.detailedemp = [];
            if (user.checked === true) {
                angular.forEach($scope.DeptdeviationRemarksReport, function (em) {
                    if (em.HRMDC_ID === user.HRMDC_ID) {
                        $scope.detailedemp.push(em);
                    }
                });
            }
            var data = {
                "departmentlist": $scope.detailedemp
            };
            apiService.create("ISM_MemoNoticeReport/DeptselectionHeaddetails", data).then(function (promise) {
                if (promise.departmnthead != null && promise.departmnthead.length > 0) {
                    $scope.departmntheadselect = [];
                    if (promise.departmnthead.length > 0) {
                        $scope.departmnthead = promise.departmnthead;
                        if (promise.departmnthead.length > 1) {
                            $scope.departmntheadselect = promise.departmnthead;
                            $('#myModalswal').modal('show');
                        }
                    }
                }
            });
        };

        //*************************** Deptselectiondetails REPORT *********************//
       // 
        $scope.Deptselectiondetails = function () {
            $scope.arrayuserEmp = [];
            $scope.designationdropdown = [];
            $scope.get_userEmplist = [];
            angular.forEach($scope.departmentdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserEmp.push(em.HRMDC_ID);
                }
            });
            var data = {
                "departmentlist": $scope.arrayuserEmp
            };
           // $scope.loaddata();
            apiService.create("ISM_MemoNoticeReport/DeptselectiondetailsNew", data).then(function (promise) {
                if (promise.designation != null && promise.designation.length > 0) {
                    $scope.designationdropdown = promise.designation;
                }
                //    if ($scope.designationdropdown.length > 0) {
                //        angular.forEach($scope.designationdropdown, function (uem) {
                //            uem.selected = true;
                //        });
                //        $scope.designationselectedAll = true;
               
                //    $scope.Desgselectiondetails();
                //}
                //else {
                //    $scope.searchValueUEM = '';
                //    $scope.employeeid = [];
                //    $scope.checkall = false;
                //    swal("No Record  Found..... !!");
                //}
                $scope.empdRawReports = promise.empdRawReports;
            });
        };

        //*************************** Desgselectiondetails REPORT *********************//
        $scope.Desgselectiondetails = function () {
            $scope.arrayuserEmp = [];
            $scope.get_userEmplist = [];
            angular.forEach($scope.designationdropdown, function (em) {
                if (em.selected === true) {
                    $scope.arrayuserEmp.push(em.HRMDES_Id);
                }
            });
            var data = {
                "designationlist": $scope.arrayuserEmp
            };
            apiService.create("ISM_MemoNoticeReport/DesgselectiondetailsNew", data).then(function (promise) {
                if (promise.get_userEmplist.length > 0 && promise.get_userEmplist != undefined ) {
                    $scope.get_userEmplist = promise.get_userEmplist;
                    //if ($scope.get_userEmplist.length > 0) {
                    //    angular.forEach($scope.get_userEmplist, function (uem) {
                    //        uem.empck = true;
                    //        uem.selected = false;
                    //        user.ICDR_DemoDate = new Date();
                    //    });
                    //    $scope.checkall = true;
                    //}
                }
                //else {
                //    $scope.searchValueUEM = '';
                //    $scope.employeeid = [];
                //    $scope.checkall = false;
                //    swal("No Record  Found..... !!");
                //}
            });
           // $scope.loaddata();
        };

        $scope.Report = function () {
            if ($scope.myForm.$valid) {
                $scope.arrayuserEmp = [];
                $scope.arraydepartment = [];
                $scope.arraydesignation = []; $scope.ISM_DemoRatingsNew = [];
                angular.forEach($scope.get_userEmplist, function (em) {
                    if (em.selected === true) {
                
                        $scope.ISM_DemoRatingsNew.push({
                            ICDR_GivenBy_EmpID: em.HRME_Id,
                            ICDR_ProductID: em.ICDR_ProductID,
                            ICDR_DemoDate: new Date(em.ICDR_DemoDate).toDateString(),
                            ICDR_Remarks: em.ICDR_Remarks,
                            ICDR_Ratings: em.ICDR_Ratings,
                           // ICDR_Ratings: em.ICDR_Ratings,
                        });
                    }
                });
                if ($scope.ISM_DemoRatingsNew != null && $scope.ISM_DemoRatingsNew != undefined && $scope.ISM_DemoRatingsNew.length > 0) {

                    var allFieldsValid = true;
                    angular.forEach($scope.ISM_DemoRatingsNew, function (em) {

                        if (!em.ICDR_ProductID || !em.ICDR_DemoDate || !em.ICDR_Remarks || !em.ICDR_Ratings) {
                            allFieldsValid = false;
                            swal("Please Enter all the data for the selected employee");
                            return;
                        }

                    });


                    if (allFieldsValid) {


                        var data = {
                            "ISM_DemoRatingsNew": $scope.ISM_DemoRatingsNew,
                            "IVRM_Month_Id": $scope.IVRM_Month_Id,
                            "IMFY_FinancialYear": $scope.IMFY_FinancialYear,

                        };

                        apiService.create("Sales_Lead/LeadUpdatemail", data).then(function (promise) {
                            if (promise.retrunMsg == "save") {
                                swal("Record save Sucessfully !");
                            }
                            else if (promise.retrunMsg == "notsave") {
                                swal("Record Not save Sucessfully !");
                            }
                            else if (promise.retrunMsg == "NotAccess") {
                                //swal("Please Contact Administrator !");
                                swal("From this Current Login Credentials You can't able to give the Demo Ratings ... !");
                            } else {
                                swal("Please Contact Administrator !");
                            }
                            $state.reload();
                        });
                    }

                }
                else {
                    swal('Please select Atleast one checkbox..');
                }
              //  $state.reload();
               
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.clearreport = function () {
            $scope.get_deviationreport = [];
            $scope.employeeid = [];
            $scope.DeptdeviationRemarksReport = [];
        };

        //*************************** MEMO,NOTICE,NOTIFICATION,EMAIL SENDING *********************//
       //VIEW MEMO NOTICE
       
        $scope.closeviewmodal = function () {
            var e1 = angular.element(document.getElementById("test"));
            $compile(e1.html(""))(($scope));
        };

        $scope.printData = function () {
            var innerContents = "";
            if ($scope.DepD === true) {
                innerContents = document.getElementById("printDeviationDept").innerHTML;
            }
            else {
                innerContents = document.getElementById("printDeviation").innerHTML;
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
        //$scope.exportToExcel = function (export_id) {
        //    var exportHref = Excel.tableToExcel(export_id, 'printDeviation');
        //    $timeout(function () {
        //        location.href = exportHref;
        //    }, 100);
        //};



    

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.Clear = function () {
            //$scope.get_deviationreport = "";
            //$scope.get_userEmplist = "";
            //$scope.checkall = "";
            //$scope.start_Date = "";
            //$scope.end_Date = "";
            //$scope.loaddata();
            $state.reload();
        };

        (function () {
            $("span.fa").on("click", function () {
                var $span = $(this),
                    $parentTr = $span.closest("tr");

                //remove
                if ($span.hasClass("fa-rotate-90")) {
                    $span.removeClass("fa-rotate-90");
                    $parentTr

                        .next()
                        .children("td")
                        .animate({ padding: 0 })
                        .wrapInner("<div />")
                        .children()
                        .slideUp(function () {
                            var $tr = $(this).closest("tr");

                            // $tr
                            // 	.prev("tr")
                            // 	.find(".active")
                            // 	.removeClass("active");
                            $tr.remove();
                            $parentTr.removeClass("tr-selected");
                        });

                    return;
                }

                //add
                $span.addClass("fa-rotate-90");
                if ($parentTr.next().hasClass("tr-detail")) return;
                $parentTr.addClass("tr-selected");
                // 	.eq(1)
                // 	.addClass("active");
                $("#tr-detail")
                    .clone()
                    .removeClass("hidden")
                    .insertAfter($parentTr)
                    .children("td")
                    .animate()
                    .wrapInner("<div style='display:none'/>")
                    .children()
                    .slideDown();
            });
        })();
    }
})();

