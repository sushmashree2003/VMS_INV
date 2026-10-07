(function () {
    'use strict';
    angular.module('app')
        .controller('ISM_Sales_Lead_qr_ReportsController', ISM_Sales_Lead_qr_ReportsController)
    ISM_Sales_Lead_qr_ReportsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
    function ISM_Sales_Lead_qr_ReportsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {
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
        if (ivrmcofigsettings != null &&  ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings!=null &&  admfigsettings.length > 0) {
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
            $scope.sourcelistAll = $scope.sourcelist.every(function (itm) {
                return itm.selected;
            });
        };

        $scope.GetSourceAll = function (sourcelistAll) {
            var toggleStatus = $scope.sourcelistAll;
            angular.forEach($scope.sourcelist, function (itm) {
                itm.selected = toggleStatus;
            });

        };

        $scope.Getstate = function (state) {
            $scope.statelistAll = $scope.statelist.every(function (itm) {
                return itm.selected;
            });
        };

        $scope.GetstateAll = function (statelistAll) {
            var toggleStatus = $scope.statelistAll;
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

               
                var data = {
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate,
                    source_IdList: ids,
                    state_IdList: ids1,
                    "flag": $scope.all1
                }
                apiService.create("ISM_Client_Project/report_Quotationrequest/", data).
                    then(function (promise) {
                        if (promise.leadquotationreport.length > 0) {
                            $scope.leadquotationreport = promise.leadquotationreport;
                            $scope.obj.givenflg = $scope.all1 ;
                            $scope.taskid5 = [];
                            angular.forEach($scope.leadquotationreport, function (dev) {
                                if ($scope.taskid5.length === 0) {
                                    $scope.taskid5.push({ HRME_Id: dev.HRME_Id, Emp_Name: dev.EmployeeName });
                                }
                                else if ($scope.taskid5.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.taskid5, function (emp) {
                                        if (emp.HRME_Id === dev.HRME_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.taskid5.push({ HRME_Id: dev.HRME_Id, Emp_Name: dev.EmployeeName });
                                    }
                                }
                            });
                            angular.forEach($scope.taskid5, function (ddd) {
                                $scope.templist = [];
                                angular.forEach($scope.leadquotationreport, function (dd) {
                                    if (dd.HRME_Id === ddd.HRME_Id) {
                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.leadquotationreport = $scope.templist;
                              
                            });
                            

                        }
                        else {
                            swal('No Data Found!!!');
                            $scope.leadquotationreport = "";
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


        $scope.printDatapdf = function () {

            var doc = new jsPDF();
            var specialElementHandlers = {
                '#editor': function (element, renderer) {
                    return true;
                }
            };

            doc.fromHTML($('#printSectionId').html(), 15, 15, {
                'width': 1300,
                'elementHandlers': specialElementHandlers
            });
            doc.save('sample-page.pdf'); 


            //var innerContents = document.getElementById("printSectionId").innerHTML;
            //var popupWinindow = window.open('');
            //popupWinindow.document.open();
            //popupWinindow.document.write('<html><head>' +
            //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
            //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
            //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
            //    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            //popupWinindow.document.close();
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
            if (sub == '1' || sub == '2') {
                data = {
                    "HRME_Id": obja.HRME_Id,
                    "fromdate": obja.fromdate,
                    "todate": obja.todate,
                    "flag": obja.emp_flg,
                    "Status": sub
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

        
          
            apiService.create("ISM_Client_Project/report_Quotationrequest_details/", data).then(function (promise) {
                if (promise.leadQuotationrequestreportdetails !== null && promise.leadQuotationrequestreportdetails.length > 0) {
                    $scope.leadQuotationrequestreportdetails = promise.leadQuotationrequestreportdetails;
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