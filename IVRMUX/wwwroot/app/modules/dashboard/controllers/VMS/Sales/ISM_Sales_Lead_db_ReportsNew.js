(function () {
    'use strict';
    angular.module('app')
        .controller('ISM_Sales_Lead_db_ReportsController', ISM_Sales_Lead_db_ReportsController)
    ISM_Sales_Lead_db_ReportsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
    function ISM_Sales_Lead_db_ReportsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {
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
        $scope.obj = {};
        $scope.obj.sourcelistAll = false
        $scope.obj.statelistAll = false

        $scope.searchValueDeptd = "";
        $scope.searchValueDesgd = "";
        $scope.searchValueUEMd = "";
        $scope.sortKey = "regno";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa
        $scope.presentCountgrid = 0;
        $scope.searchValue = "";
        $scope.tadprint = false;
        $scope.nameschedule = true;
        $scope.printdatatable = [];
        $scope.loaddata = function () {
            $scope.screport = false;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            var pageid = 2;
            apiService.getURI("ISM_Client_Project/getcmtrpt_data", pageid).
                then(function (promise) {
                    if (promise.sourcelist !== null && promise.sourcelist.length > 0) {
                        $scope.sourcelist = promise.sourcelist;
                    }
                    if (promise.statelist !== null && promise.statelist.length > 0) {
                        $scope.statelist = promise.statelist;
                    }
                })
        };

        //Getsource
        $scope.Getsource = function (source) {
            $scope.obj.sourcelistAll = $scope.sourcelist.every(function (itm) {
                return itm.selected;
            });
        };

        $scope.GetSourceAll = function (sourcelistAll) {
            var toggleStatus = $scope.obj.sourcelistAll;
            angular.forEach($scope.sourcelist, function (itm) {
                itm.selected = toggleStatus;
            });

        };

        $scope.Getstate = function (state) {
            $scope.obj.statelistAll = $scope.statelist.every(function (itm) {
                return itm.selected;
            });
        };

        $scope.GetstateAll = function (statelistAll) {
            var toggleStatus = $scope.obj.statelistAll;
            angular.forEach($scope.statelist, function (itm) {
                itm.selected = toggleStatus;
            });

        };


        $scope.submitted = false;
        $scope.showreport = function () {

            $scope.obj.givenflg = "";
            if ($scope.myForm.$valid) {

                var ids = [];
                angular.forEach($scope.sourcelist, function (grp_t) {
                    if (grp_t.selected) {
                        ids.push(grp_t.ismsmsO_Id);
                    }
                });

                var ids1 = [];
                angular.forEach($scope.statelist, function (grp_t) {
                    if (grp_t.selected) {
                        ids1.push(grp_t.ivrmmS_Id);
                    }
                });

                $scope.fromdatee = "";
                $scope.todatee = "";
                $scope.fromdate = $filter('date')($scope.fromdate, 'yyyy-MM-dd');
                $scope.todate = $filter('date')($scope.todate, 'yyyy-MM-dd');
                $scope.submitted = true; $scope.LeadCount = 0; $scope.Admincount = 0; $scope.Telcount = 0; $scope.AdminReject = 0; $scope.TelcountReject = 0;
                $scope.leaddropdown = []; var LeadCount = 0; var Admincount = 0; var Telcount = 0; var AdminReject = 0;
                var TelcountReject = 0;

                var Lead_Total = 0;
                var Lead_Done = 0;
                var Lead_Count_NotDone = 0;
                var Proposal_Sent = 0;
                var Executed = 0;

                var data = {
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate,
                    source_IdList: ids,
                    state_IdList: ids1,
                    "flag": $scope.all1
                }
                //leaddatabasereportdetails
                apiService.create("ISM_Client_Project/report_database/", data).
                    then(function (promise) {
                        if (promise.leaddatabasereportdetails != null && promise.leaddatabasereportdetails.length > 0) {
                            $scope.leaddropdown = promise.leaddatabasereportdetails;
                            angular.forEach($scope.leaddropdown, function (em) {
                                LeadCount = (Number(LeadCount) + (Number(em.LeadCount)));
                                Admincount = (Number(Admincount) + (Number(em.Admincount)));
                                Telcount = (Number(Telcount) + (Number(em.Telcount)));
                                AdminReject = (Number(AdminReject) + (Number(em.AdminReject)));
                                TelcountReject = (Number(TelcountReject) + (Number(em.TelcountReject)));
                                Lead_Total = (Number(Lead_Total) + (Number(em.Lead_Total)));
                                Lead_Done = (Number(Lead_Done) + (Number(em.Lead_Done)));
                                Lead_Count_NotDone = (Number(Lead_Count_NotDone) + (Number(em.Lead_Count_NotDone)));
                                Proposal_Sent = (Number(Proposal_Sent) + (Number(em.Proposal_Sent)));
                                Executed = (Number(Executed) + (Number(em.Executed)));



                            });
                            $scope.LeadCount = LeadCount;
                            $scope.Admincount = Admincount;
                            $scope.Telcount = Telcount;
                            $scope.AdminReject = AdminReject;
                            $scope.TelcountReject = TelcountReject;

                            $scope.Lead_Total = Lead_Total;
                            $scope.Lead_Done = Lead_Done;
                            $scope.Lead_Count_NotDone = Lead_Count_NotDone;
                            $scope.Proposal_Sent = Proposal_Sent;
                            $scope.Executed = Executed;
                        }
                        else {
                            swal("No Records Found");

                        }
                    })
            }
            else {

                $scope.submitted = true;
            }
        };




        $scope.DemoGiven11 = function (obja, sub) {

            //var ids = [];
            //angular.forEach($scope.sourcelist, function (grp_t) {
            //    if (grp_t.selected) {
            //        ids.push(grp_t.ismsmsO_Id);
            //    }
            //});

            $scope.leaddatabasereportdetails = [];
            var data = {};
            if (sub == '1' || sub == '2') {
                data = {
                    "HRME_Id": obja.HRME_Id,
                    "fromdate": obja.fromdate,
                    "todate": obja.todate,
                    "flag": obja.emp_flg,
                    "Status": sub
                    //,
                    //source_IdList: ids,
                };
            }
            else {
                data = {
                    "HRME_Id": obja.HRME_Id,
                    "fromdate": obja.fromdate,
                    "todate": obja.todate,
                    "flag": obja.emp_flg,
                    "Status": ""

                };
            }


            apiService.create("ISM_Client_Project/report_database_details/", data).then(function (promise) {
                if (promise.leaddatabasereportdetails != null && promise.leaddatabasereportdetails.length > 0) {
                    $scope.leaddatabasereportdetails = promise.leaddatabasereportdetails;
                    $('#myModal1').modal('show');
                }
                else {
                    swal('No Data Found!!!');
                    $scope.leaddatabasereportdetails = [];
                }

            });
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

            if (obja.HRME_Id == null && obja.HRME_EmployeeFirstName =='DIGI9') {
                obja.HRME_Id = 0;
            }

            $scope.Sales_Raw_Report_OnclickGridData = [];
            $scope.submitted = true;


            var ids = [];
            angular.forEach($scope.sourcelist, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.ismsmsO_Id);
                }
            });

            var ids1 = [];
            angular.forEach($scope.statelist, function (grp_t) {
                if (grp_t.selected) {
                    ids1.push(grp_t.ivrmmS_Id);
                }
            });



            var data = {
                "fromdate": new Date($scope.fromdate).toDateString(),
                "todate": new Date($scope.todate).toDateString(),
                "HRME_Id": obja.HRME_Id,
                "flag": sub,
                "searchleadvalue": "Ind",
                source_IdList: ids,
                state_IdList: ids1
            }

            apiService.create("ISM_Client_Project/report_database_details/", data).then(function (promise) {
                if (promise.leadQuotationrequestreportdetails != null && promise.leadQuotationrequestreportdetails.length > 0) {
                    $scope.Sales_Raw_Report_OnclickGridData = promise.leadQuotationrequestreportdetails;
                    $('#myModal1').modal('show');
                }
                else {
                    swal("No Records Found");
                    $scope.Sales_Raw_Report_OnclickGridData = [];

                }
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.sourcelist.some(function (options) {
                return options.selected;
            });
        };


        $scope.isOptionsRequired11 = function () {
            return !$scope.statelist.some(function (options) {
                return options.selected;
            });
        };




        $scope.DemoGivenTotal = function (sub) {
            $scope.Sales_Raw_Report_OnclickGridData = [];
            $scope.submitted = true;

            var ids = [];
            angular.forEach($scope.sourcelist, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.ismsmsO_Id);
                }
            });

            var ids1 = [];
            angular.forEach($scope.statelist, function (grp_t) {
                if (grp_t.selected) {
                    ids1.push(grp_t.ivrmmS_Id);
                }
            });


            var data = {
                "fromdate": new Date($scope.fromdate).toDateString(),
                "todate": new Date($scope.todate).toDateString(),
                "HRME_Id": 0,
                "flag": sub,

                source_IdList: ids,
                state_IdList: ids1
            }

            apiService.create("ISM_Client_Project/report_database_details/", data).then(function (promise) {
                if (promise.leadQuotationrequestreportdetails != null && promise.leadQuotationrequestreportdetails.length > 0) {
                    $scope.Sales_Raw_Report_OnclickGridData = promise.leadQuotationrequestreportdetails;
                    $('#myModal1').modal('show');
                }
                else {
                    swal("No Records Found");
                    $scope.Sales_Raw_Report_OnclickGridData = [];

                }
            });
        };

    }
})();