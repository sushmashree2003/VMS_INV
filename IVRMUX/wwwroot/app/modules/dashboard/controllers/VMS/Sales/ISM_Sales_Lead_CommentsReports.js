(function () {
    'use strict';
    angular.module('app')
        .controller('ISM_Sales_Lead_CommentsReportsController', ISM_Sales_Lead_CommentsReportsController)
    ISM_Sales_Lead_CommentsReportsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
    function ISM_Sales_Lead_CommentsReportsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {
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
            $scope.fromdate = new Date();
            $scope.todate = new Date();

            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            var pageid = 2;
            apiService.getURI("ISM_Client_Project/getcmtrpt_data", pageid).
                then(function (promise) {
                    $scope.leadlist = promise.leadlist;
                    if (promise.companies !== null && promise.companies.length > 0) {
                        $scope.companies = promise.companies;
                    }
                    if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                        $scope.departmentdropdown = promise.departmentdropdown;
                    }

                    if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                        $scope.designationdropdown = promise.designationdropdown;
                    }

                })
        };

        //By companies Single
        $scope.GetEmployeeBycompanies = function (companies) {
            $scope.departmentdropdown = [];
            $scope.designation = [];
            $scope.employeedropdown = [];
            $scope.departmentselectedAll = false;
            $scope.companiesselectedAll = $scope.companies.every(function (itm) {
                return itm.selected;
            });
            $scope.get_depart();

        };



        $scope.GetEmployeeByDepartment = function (department) {
            $scope.designationdropdown = [];
            $scope.employeedropdown = [];
            $scope.designationselectedAll = false;

            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {

                return itm.selected;
            });
            $scope.get_desig();
        };



        $scope.GetEmployeeByDesignation = function (designation) {
            $scope.employeedropdown = [];
            $scope.employeedropdownselectedAll = false;
            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {
                return itm.selected;
            });

            $scope.get_employee();
        };
        $scope.get_employee = function () {

            var ids2 = [];
            angular.forEach($scope.companies, function (grp_t) {
                if (grp_t.selected) {
                    ids2.push(grp_t.mI_Id);
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


            var data = {
                MI_Id_IdList: ids2,
                hrmdes_IdList: ids,
                hrmD_IdList: ids1

            };
            apiService.create("ISM_Client_Project/get_employee", data).
                then(function (promise) {
                    if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                        $scope.employeedropdown = promise.employeedropdown;

                    }
                });
        };


        $scope.get_depart = function () {
            var ids1 = [];
            angular.forEach($scope.companies, function (grp_t) {
                if (grp_t.selected) {
                    ids1.push(grp_t.mI_Id);
                }
            });
            var data = {
                MI_Id_IdList: ids1,
            };
            apiService.create("ISM_Client_Project/get_depart", data).
                then(function (promise) {
                    if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                        $scope.departmentdropdown = promise.departmentdropdown;
                    }
                });
        };




        $scope.get_desig = function () {

            var ids1 = [];
            angular.forEach($scope.departmentdropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids1.push(grp_t.hrmD_Id);
                }
            });
            var data = {
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
            $scope.employeedropdown = [];
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
        $scope.GetEmployeeBycompAll = function (companiesselectedAll) {

            $scope.departmentdropdown = [];
            var toggleStatus = $scope.companiesselectedAll;
            angular.forEach($scope.companies, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_depart();
        };

        $scope.GetEmployeeByDepartmentAll = function (departmentselectedAll) {
            $scope.designationdropdown = [];
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
            $scope.submitted = true;
            $scope.obj.givenflg = "";
            if ($scope.myForm.$valid) {




                var ids2 = [];
                angular.forEach($scope.companies, function (grp_t) {
                    if (grp_t.selected) {
                        ids2.push(grp_t.mI_Id);
                    }
                });

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

                $scope.Total = [];
                $scope.TotalDemoScheduled = 0;
                $scope.Executed = 0;

                $scope.NotExecuted = 0;
                $scope.Reschedule = 0;
                $scope.RequestQuotation = 0;
                $scope.HOT = 0;
                $scope.WARM = 0;
                $scope.COLD = 0;
                $scope.Charge_Leads = 0;

                var stat = "";
                if ($scope.Status == true) {
                    stat = "LeftEmployees";
                } else {
                    stat = stat;
                }

                var data = {
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate,
                    MI_Id_IdList: ids2,
                    hrmdes_IdList: ids,
                    hrmD_IdList: ids1,
                    hrmE_IdList: ids3,
                    "flag": $scope.all1,
                    "Status": stat
                }
                apiService.create("ISM_Client_Project/report_cmtrpt/", data).
                    then(function (promise) {

                        //ISMMTCAT_TaskCategoryName
                        if (promise.leadcommentsreport.length > 0 && promise.leadcommentsreport != null && promise.leadcommentsreport !=undefined) {
                            $scope.leadcommentsreport = promise.leadcommentsreport;
                            $scope.components_list = promise.components_list;

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

                            if ($scope.leadcommentsreport != null && $scope.leadcommentsreport.length > 0) {
                                //(Number() + (Number()));
                                angular.forEach($scope.leadcommentsreport, function (itm) {
                                    $scope.TotalDemoScheduled = (Number($scope.TotalDemoScheduled) + (Number(itm.Total_Leads)));
                                    $scope.Executed = (Number($scope.Executed) + (Number(itm.Demo_Given)));
                                    $scope.NotExecuted = (Number($scope.NotExecuted) + (Number(itm.Not_Given_Demo)));
                                    $scope.Reschedule = (Number($scope.Reschedule) + (Number(itm.Total_Leads_Re)));
                                    $scope.RequestQuotation = (Number($scope.RequestQuotation) + (Number(itm.SendQuotation)));
                                    $scope.HOT = (Number($scope.HOT) + (Number(itm.HOT)));
                                    $scope.WARM = (Number($scope.WARM) + (Number(itm.WARM)));
                                    $scope.COLD = (Number($scope.COLD) + (Number(itm.COLD)));
                                    $scope.Charge_Leads = (Number($scope.Charge_Leads) + (Number(itm.Charge_Leads)));

                                });
                                //adee                                                               
                                if ($scope.obj.givenflg == 'ScheduledBy') {
                                    $scope.employeeid = [];
                                    angular.forEach($scope.leadcommentsreport, function (dev) {
                                        if ($scope.employeeid.length === 0) {
                                            $scope.employeeid.push(dev);
                                        }
                                        else if ($scope.employeeid.length > 0) {
                                            var intcount = 0;
                                            angular.forEach($scope.employeeid, function (emp) {
                                                if (emp.ISMMTCAT_TaskCategoryName === dev.ISMMTCAT_TaskCategoryName) {
                                                    intcount += 1;
                                                }
                                            });
                                            if (intcount === 0) {
                                                $scope.employeeid.push(dev);
                                            }
                                        }
                                    });


                                }

                            }
                        }
                        else {
                            swal('No Data Found!!!');
                            $scope.leadcommentsreport = "";
                            $scope.Status = false;
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
        }

        //export start
        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };

        $scope.exportToExcels = function (tableId) {
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
        $scope.ismslE_FilePath = "";
        $scope.CloseModal = function () {
            $('#myModal1').modal('show');
            $('#showpdf').modal('hide');
        }
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
                $('#myModal1').modal('hide');
            }


        };
        
        //= 1874
        $scope.DemoGivenNew = function (obja, sub) {
            $scope.leadcommentsreportDeatils = [];
            var data = {};
            data = {
                "HRME_Id": obja.HRME_Id,
                "fromdate": new Date($scope.fromdate).toDateString(),
                "todate": new Date($scope.todate).toDateString(),
                "flag": obja.emp_flg,
                "FlagType": 1,
                "Status": "ALL",
                "ISMSMPR_ProductName":""       
            };

            apiService.create("ISM_Client_Project/report_cmtrptDetails/", data).then(function (promise) {
                $scope.obj.sub = sub;
                if (promise.leadcommentsreportDeatils !== null && promise.leadcommentsreportDeatils.length >0) {
                    $scope.leadcommentsreportDeatils = promise.leadcommentsreportDeatils;
                    $scope.HRME_EmployeeFirstName = promise.leadcommentsreportDeatils[0].HRME_EmployeeFirstName;
                    $scope.Scheduleto = promise.leadcommentsreportDeatils[0].Scheduleto;//promise.leadcommentsreportDeatils[0].ismsledmpR_Remarks
                    $scope.Totalcount = promise.leadcommentsreportDeatils.length;
                    $scope.leadcommentsreportDeatilsonline = [];
                    $scope.leadcommentsreportDeatilsoffline = [];
                    angular.forEach($scope.leadcommentsreportDeatils, function (dev) {
                        if (dev.ISMSLEDM_DemoType === 'Online') {
                            $scope.leadcommentsreportDeatilsonline.push(dev);
                        } else {
                            $scope.leadcommentsreportDeatilsoffline.push(dev);
                        }
                    });
                    $scope.offline = $scope.leadcommentsreportDeatilsoffline.length;
                    $scope.online = $scope.leadcommentsreportDeatilsonline.length;
                    $scope.givenflg = $scope.leadcommentsreportDeatils[0].givenflg;
                    $('#myModal1').modal('show');
                }
                else {
                    swal('No Data Found!!!');
                    $scope.leadcommentsreportDeatils = "";
                }

            });
        }
        $scope.DemoGiven = function (obja, sub) {
            $scope.leadcommentsreportDeatils = [];

            $scope.leadcommentsreportDeatilsoffline = [];
            $scope.leadcommentsreportDeatilsonline = [];
            $scope.offline = "";
            $scope.online = "";
            $scope.givenflg = "";
            $scope.HRME_EmployeeFirstName = obja.Emp_Name;
            $scope.Totalcount = 0;
            var data = {};
            if (obja.ISMMTCAT_TaskCategoryName == null) {
                $scope.ISMMTCAT_TaskCategoryName = '';
            }
            else {
                $scope.ISMMTCAT_TaskCategoryName = obja.ISMMTCAT_TaskCategoryName;
            }

            if (sub == 'COLD' || sub == 'HOT' || sub == 'WARM') {
                data = {
                    "HRME_Id": obja.HRME_Id,
                    "fromdate": obja.fromdate,
                    "todate": obja.todate,
                    "flag": obja.emp_flg,
                    "FlagType": 1,
                    "Status": sub,
                    "ISMSMPR_ProductName": $scope.ISMMTCAT_TaskCategoryName

                };
            } else {
                data = {
                    "HRME_Id": obja.HRME_Id,
                    "fromdate": obja.fromdate,
                    "todate": obja.todate,
                    "flag": obja.emp_flg,
                    "FlagType": sub,
                    "Status": "",
                    "ISMSMPR_ProductName": $scope.ISMMTCAT_TaskCategoryName         //existing DTO
                };
            }

            apiService.create("ISM_Client_Project/report_cmtrptDetails/", data).then(function (promise) {
                $scope.obj.sub = sub;
                if (promise.leadcommentsreportDeatils > 0 || promise.leadcommentsreportDeatils !== null) {
                    $scope.leadcommentsreportDeatils = promise.leadcommentsreportDeatils;
                    $scope.HRME_EmployeeFirstName = promise.leadcommentsreportDeatils[0].HRME_EmployeeFirstName;
                    $scope.Scheduleto = promise.leadcommentsreportDeatils[0].Scheduleto;
                    $scope.Totalcount = promise.leadcommentsreportDeatils.length;
                    $scope.leadcommentsreportDeatilsonline = [];
                    $scope.leadcommentsreportDeatilsoffline = [];
                    angular.forEach($scope.leadcommentsreportDeatils, function (dev) {
                        if (dev.ISMSLEDM_DemoType === 'Online') {
                            $scope.leadcommentsreportDeatilsonline.push(dev);
                        } else {
                            $scope.leadcommentsreportDeatilsoffline.push(dev);
                        }
                    });
                    $scope.offline = $scope.leadcommentsreportDeatilsoffline.length;
                    $scope.online = $scope.leadcommentsreportDeatilsonline.length;
                    $scope.givenflg = $scope.leadcommentsreportDeatils[0].givenflg;
                    $('#myModal1').modal('show');
                }
                else {
                    swal('No Data Found!!!');
                    $scope.leadcommentsreportDeatils = "";
                }

            });
        };

        $scope.TotalDemoGiven = function (sub) {
            $scope.Totalcommentreport = [];
            var data = {};
            
            if (sub == 'COLD' || sub == 'HOT' || sub == 'WARM') {
                data = {
                   
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate,
                    "FlagType": 1,
                    "Status": sub,
                };
            } 

            apiService.create("ISM_Client_Project/Total_commentreport/", data).then(function (promise) {
               
                if (promise.totalcommentreport.length > 0 && promise.totalcommentreport !== null) {
                    $scope.Totalcommentreport = promise.totalcommentreport;
                    $scope.HRME_EmployeeFirstName = promise.totalcommentreport[0].HRME_EmployeeFirstName;
                    $scope.Scheduleto = promise.totalcommentreport[0].Scheduleto;
                    $scope.Totalcount = promise.totalcommentreport.length;
                    $scope.totalcommentreportonline = [];
                    $scope.totalcommentreportoffline = [];
                    angular.forEach($scope.Totalcommentreport, function (dev) {
                        if (dev.ISMSLEDM_DemoType === 'Online') {
                            $scope.totalcommentreportonline.push(dev);
                        } else {
                            $scope.totalcommentreportoffline.push(dev);
                        }
                    });
                    $scope.offlinedata = $scope.totalcommentreportoffline.length;
                    $scope.onlinedata = $scope.totalcommentreportonline.length;
                    $scope.givenflg = $scope.Totalcommentreport[0].givenflg;
                    $('#myModal123').modal('show');
                }
                else {
                    swal('No Data Found!!!');
                    $scope.leadcommentsreportDeatils = "";
                }

            });
        };


        $scope.isOptionsRequired = function () {
            return !$scope.employeedropdown.some(function (options) {
                return options.selected;
            });
        };
       

    }
})();