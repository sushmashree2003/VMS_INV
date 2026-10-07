(function () {
    'use strict';
    angular.module('app')
        .controller('ISM_Sales_Lead_VerifyReportsController', ISM_Sales_Lead_VerifyReportsController)
    ISM_Sales_Lead_VerifyReportsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
    function ISM_Sales_Lead_VerifyReportsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.searchValueDeptd = "";
        $scope.searchValueDesgd = "";
        $scope.searchValueUEMd = "";
        $scope.sortKey = "regno";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa
        $scope.presentCountgrid = 0;
        $scope.searchValue = "";
        $scope.tadprint = false;
        $scope.nameschedule = true;
        $scope.obj = {};
        $scope.printdatatable = [];
        $scope.loaddata = function () {
            $scope.screport = false;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            var pageid = 2;
            apiService.getURI("ISM_Client_Project/getcmtrpt_data", pageid).
                then(function (promise) {
                    $scope.leadlist = promise.leadlist;
                    //$scope.imgname = logo;

                    if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                        $scope.departmentdropdown = promise.departmentdropdown;
                        //$scope.departmentselectedAll = true;
                        //$scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                    }

                    if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                        $scope.designationdropdown = promise.designationdropdown;
                        //$scope.designationselectedAll = true;
                        //$scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                    }

                })
        };

        //By Department Single
        $scope.GetEmployeeByDepartment = function (department) {
            $scope.designationdropdown = [];
            $scope.designationselectedAll = false;

            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {

                return itm.selected;
            });
            $scope.get_desig();
        };

        $scope.GetEmployeeByDesignation = function (designation) {

            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {
                return itm.selected;
            });

            $scope.get_employee();
        };
        $scope.get_employee = function () {
            var ids = [];
            angular.forEach($scope.designationdropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.hrmdeS_Id);
                }
            });
            var ids1 = [];
            angular.forEach($scope.departmentdropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids1.push(grp_t.hrmD_Id);
                }
            });
            var data = {
                hrmdes_IdList: ids,
                hrmD_IdList: ids1
            };
            apiService.create("ISM_Client_Project/get_employee", data).
                then(function (promise) {
                    if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                        $scope.employeedropdown = promise.employeedropdown;
                        //$scope.employeedropdownselectedAll = true;
                        // $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                    }
                });
        };


        $scope.get_desig = function () {
            var ids = [];
            angular.forEach($scope.groupTypedropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.hrmgT_Id);
                }
            });
            var ids1 = [];
            angular.forEach($scope.departmentdropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids1.push(grp_t.hrmD_Id);
                }
            });
            var data = {
                hrmgT_IdList: ids,
                hrmD_IdList: ids1,

            };
            apiService.create("ISM_Client_Project/get_desig", data).
                then(function (promise) {
                    if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                        $scope.designationdropdown = promise.designationdropdown;

                        // $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                    }
                });
        };

        //By Designation
        $scope.GetEmployeeByDesignationAll = function (designationselectedAll) {

            var toggleStatus = $scope.designationselectedAll;
            angular.forEach($scope.designationdropdown, function (itm) {
                itm.selected = toggleStatus;
            });

            $scope.get_employee();
        };

        $scope.GetEmployeeBygroupTypeAll = function (EmployeeselectedAll) {

            var toggleStatus = $scope.employeedropdownselectedAll;
            angular.forEach($scope.employeedropdown, function (itm) {
                itm.selected = toggleStatus;
            });

            $scope.getleads();

        };
        $scope.getleads = function () {
            var ids3 = [];
            if ($scope.employeedropdownselectedAll == true) {
                ids3 = [];
            }
            else {
                angular.forEach($scope.employeedropdown, function (grp_t) {
                    if (grp_t.selected) {
                        ids3.push(grp_t.hrmE_Id);
                    }
                });
            }



            var data = {
                hrmE_IdList: ids3,
                "flag": $scope.all1
            }
            apiService.create("ISM_Client_Project/getleads", data).
                then(function (promise) {
                    $scope.leadlist = promise.leadlist;

                })
        }


        $scope.GetEmployeedropdown = function (designation) {
            $scope.employeedropdownselectedAll = $scope.employeedropdown.every(function (itm) {
                return itm.selected;
            });

            $scope.getleads();


        };
        $scope.GetEmployeeByDepartmentAll = function (departmentselectedAll) {

            var toggleStatus = $scope.departmentselectedAll;
            angular.forEach($scope.departmentdropdown, function (itm) {
                itm.selected = toggleStatus;
            });

            $scope.get_desig();
        };

        $scope.searchleadvalue = function (searchleadvalue) {
            if (searchleadvalue.length > 1) {
                var data = {
                    "searchleadvalue": searchleadvalue,
                    "flag": "Search"
                }
                apiService.create("ISM_Client_Project/searchleadvalue", data).
                    then(function (promise) {
                        $scope.leadlist = promise.leadlist;

                    })
            }
        };
        $scope.all_checkdept = function (ISMSLE_Id) {
            var toggleStatus = ISMSLE_Id;
            angular.forEach($scope.filterValue5, function (itm) {
                itm.selected = toggleStatus;
            });

        };
        $scope.togchkbxdept = function (groupType) {
            $scope.ISMSLE_Id = $scope.leadlist.every(function (itm) {
                return itm.selected;
            });



            var data = {
                "searchleadvalue": searchleadvalue,
                "flag": "Search"
            }
            apiService.create("ISM_Client_Project/searchleadvalue", data).
                then(function (promise) {
                    $scope.leadlist = promise.leadlist;

                })


        };


        $scope.submitted = false;
        $scope.showreport = function () {

            $scope.obj.givenflg = "";
            if ($scope.myForm.$valid) {
                $scope.leaddatalist = [];
                angular.forEach($scope.leadlist, function (d) {
                    if (d.selected === true) {
                        $scope.leaddatalist.push({ ISMSLE_Id: d.ISMSLE_Id });
                    }
                });

                var ids = [];
                angular.forEach($scope.designationdropdown, function (grp_t) {
                    if (grp_t.selected) {
                        ids.push(grp_t.hrmdeS_Id);
                    }
                });

                var ids1 = [];
                angular.forEach($scope.departmentdropdown, function (grp_t) {
                    if (grp_t.selected) {
                        ids1.push(grp_t.hrmD_Id);
                    }
                });

                var ids3 = [];
                angular.forEach($scope.employeedropdown, function (grp_t) {
                    if (grp_t.selected) {
                        ids3.push(grp_t.hrmE_Id);
                    }
                });

                $scope.fromdatee = "";
                $scope.todatee = "";
                $scope.fromdate = $filter('date')($scope.fromdate, 'yyyy-MM-dd');
                $scope.todate = $filter('date')($scope.todate, 'yyyy-MM-dd');


                var data = {
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate,
                    hrmdes_IdList: ids,
                    hrmD_IdList: ids1,
                    hrmE_IdList: ids3,
                    //"flag": $scope.all1
                }
                apiService.create("ISM_Client_Project/report_cmtrptDetailsverify/", data).
                    then(function (promise) {
                        if (promise.leadcommentsreport > 0 || promise.leadcommentsreport !== null) {
                            $scope.leadcommentsreport = promise.leadcommentsreport;

                            $scope.obj.givenflg = $scope.all1;



                            $scope.taskid5 = [];
                            angular.forEach($scope.leadcommentsreport, function (dev) {
                                if ($scope.taskid5.length === 0) {
                                    $scope.taskid5.push({ HRME_Id: dev.HRME_Id, Emp_Name: dev.Emp_Name });
                                }
                                else if ($scope.taskid5.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.taskid5, function (emp) {
                                        if (emp.HRME_Id === dev.HRME_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.taskid5.push({ HRME_Id: dev.HRME_Id, Emp_Name: dev.Emp_Name });
                                    }
                                }
                            });
                            angular.forEach($scope.taskid5, function (ddd) {
                                $scope.templist = [];
                                angular.forEach($scope.leadcommentsreport, function (dd) {
                                    if (dd.HRME_Id === ddd.HRME_Id) {
                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.leadcommentsreport = $scope.templist;

                            });


                            //$scope.employeeid = [];

                            //angular.forEach($scope.leadcommentsreport, function (dev) {
                            //    if ($scope.employeeid.length === 0) {
                            //        $scope.employeeid.push({
                            //            ISMSLE_Id: dev.ISMSLE_Id,
                            //            ISMSLE_LeadName: dev.ISMSLE_LeadName
                            //        });
                            //    } else if ($scope.employeeid.length > 0) {
                            //        var intcount = 0;
                            //        angular.forEach($scope.employeeid, function (emp) {
                            //            if (emp.ISMSLE_LeadName === dev.ISMSLE_LeadName) {
                            //                intcount += 1;
                            //            }
                            //        });
                            //        if (intcount === 0) {
                            //            $scope.employeeid.push({
                            //                ISMSLE_Id: dev.ISMSLE_Id,
                            //                ISMSLE_LeadName: dev.ISMSLE_LeadName
                            //            });
                            //        }
                            //    }
                            //});
                            //angular.forEach($scope.employeeid, function (ddd) {
                            //    $scope.templist = [];
                            //    angular.forEach($scope.leadcommentsreport, function (dd) {
                            //        if (ddd.ISMSLE_LeadName === dd.ISMSLE_LeadName) {
                            //            $scope.templist.push(ddd);
                            //        }
                            //    });
                            //    ddd.plannerdetails = $scope.templist;
                            //});



                        }
                        else {
                            swal('No Data Found!!!');
                            $scope.leadcommentsreport = "";
                        }
                    })
            }
            else {

                $scope.submitted = true;
            }
        };
        $scope.toggleAll = function () {
            $scope.printstudents = [];
            var toggleStatus = $scope.all;
            angular.forEach($scope.deletemsglist, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.printdatatable.indexOf(itm) === -1) {
                        $scope.printdatatable.push(itm);
                    }
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        }
        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.deletemsglist.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }
        }      //export start
        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };
        $scope.printData = function () {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //view
        $scope.previewimg_new = function (img) {
            $scope.ismslE_FilePath = img;
            var img = $scope.ismslE_FilePath;
            if (img != null && img != "") {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];

                if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                    $window.open($scope.ismslE_FilePath)
                }
                else if (lastelement == 'pdf') {
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.ismslE_FilePath;
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
                        });
                }
            }


        };
        //= 1874

        $scope.DemoGiven = function (obja, sub) {
            var data = {};
            if (sub == 'COLD' || sub == 'HOT' || sub == 'WARM') {
                data = {
                    "HRME_Id": obja.HRME_Id,
                    "fromdate": obja.fromdate,
                    "todate": obja.todate,
                   // "flag": obja.emp_flg,
                    //"FlagType": 1,
                    "Status": sub
                };
            } else {
                data = {
                    "HRME_Id": obja.HRME_Id,
                    "fromdate": obja.fromdate,
                    "todate": obja.todate,
                    //"flag": obja.emp_flg,
                   // "FlagType": sub,
                    "Status": sub
                };
            }

            apiService.create("ISM_Client_Project/report_cmtrptDetailsverifyDetails/", data).then(function (promise) {

                if (promise.leadcommentsreportDeatils > 0 || promise.leadcommentsreportDeatils !== null) {
                    $scope.leadcommentsreportDeatils = promise.leadcommentsreportDeatils;
                    $scope.givenflg = $scope.leadcommentsreportDeatils[0].givenflg;
                    $('#myModal1').modal('show');
                }
                else {
                    swal('No Data Found!!!');
                    $scope.leadcommentsreportDeatils = "";
                }

            });
        };

    }
})();