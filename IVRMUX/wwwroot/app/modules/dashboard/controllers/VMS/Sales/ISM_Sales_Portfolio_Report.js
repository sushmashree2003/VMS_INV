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


                    if (promise.closurestatus !== null && promise.closurestatus.length > 0) {
                        $scope.Closurestatus = promise.closurestatus;
                    }

                    if (promise.bdElist !== null && promise.bdElist.length > 0) {
                        $scope.BDElist = promise.bdElist;
                    }

                    if (promise.clientAcquisitionlist !== null && promise.clientAcquisitionlist.length > 0) {
                        $scope.ClientAcquisitionlist = promise.clientAcquisitionlist;
                    }

                    if (promise.telecallerlist !== null && promise.telecallerlist.length > 0) {
                        $scope.Telecallerlist = promise.telecallerlist;
                    }
                    if (promise.regionlist !== null && promise.regionlist.length > 0) {
                        $scope.Regionlist = promise.regionlist;
                    }

                    if (promise.revenue !== null && promise.revenue.length > 0) {
                        $scope.Revenue = promise.revenue;
                    }

                    
                })
        };

        $scope.isOptionsRequired = function () {
            return !$scope.BDElist.some(function (options) {
                return options.selected;
            });
        };


        $scope.isOptionsRequiredtel = function () {
            return !$scope.Telecallerlist.some(function (options) {
                return options.selected;
            });
        };


        $scope.isOptionsRequiredclosure = function () {
            return !$scope.Closurestatus.some(function (options) {
                return options.selected;
            });
        };




        $scope.isOptionsRequiredreg = function () {
            return !$scope.Regionlist.some(function (options) {
                return options.selected;
            });
        };

        $scope.isOptionsRequiredrevenue = function () {
            return !$scope.Revenue.some(function (options) {
                return options.selected;
            });
        };

        $scope.isOptionsRequiredca = function () {
            return !$scope.ClientAcquisitionlist.some(function (options) {
                return options.selected;
            });
        };


        //bde list
        $scope.GetBDEAll = function (bdelistAll) {
            var toggleStatus = $scope.obj.bdelistAll;
            angular.forEach($scope.BDElist, function (itm) {
                itm.selected = toggleStatus;
            });

        };


        $scope.togchkbxm = function () {

            $scope.obj.bdelistAll = $scope.BDElist.every(function (itm) {
                return itm.selected;
            });

            $scope.searchValueDesg == '';

        };


        //telecaller list
        $scope.GetTelall = function (telecallerlistAll) {
            var toggleStatus = $scope.obj.telecallerlistAll;
            angular.forEach($scope.Telecallerlist, function (itm) {
                itm.selected = toggleStatus;
            });

        };


        $scope.togchkbxmtel = function () {

            $scope.obj.telecallerlistAll = $scope.Telecallerlist.every(function (itm) {
                return itm.selected;
            });

            $scope.searchValueDesg == '';

        };


        //closure list
        $scope.Getclosureall = function (closurestatuslistAll) {
            var toggleStatus = $scope.obj.closurestatuslistAll;
            angular.forEach($scope.Closurestatus, function (itm) {
                itm.selected = toggleStatus;
            });

        };


        $scope.togchkbxmclosure = function () {

            $scope.obj.closurestatuslistAll = $scope.Closurestatus.every(function (itm) {
                return itm.selected;
            });

            $scope.searchValueDesg == '';

        };



        //Region list
        $scope.Getregionall = function (regionlistAll) {
            var toggleStatus = $scope.obj.regionlistAll;
            angular.forEach($scope.Regionlist, function (itm) {
                itm.selected = toggleStatus;
            });

        };


        $scope.togchkbxregion = function () {

            $scope.obj.regionlistAll = $scope.Regionlist.every(function (itm) {
                return itm.selected;
            });

            $scope.searchValueDesg == '';

        };


        //Revenue list
        $scope.GetrevenueAll = function (revenuelistAll) {
            var toggleStatus = $scope.obj.revenuelistAll;
            angular.forEach($scope.Revenue, function (itm) {
                itm.selected = toggleStatus;
            });

        };


        $scope.togchkbxrevenue = function () {

            $scope.obj.revenuelistAll = $scope.Revenue.every(function (itm) {
                return itm.selected;
            });

            $scope.searchValueDesg == '';

        };


        //Client acquisition
        $scope.GetCAAll = function (calistAll) {
            var toggleStatus = $scope.obj.calistAll;
            angular.forEach($scope.ClientAcquisitionlist, function (itm) {
                itm.selected = toggleStatus;
            });

        };


        $scope.togchkbxmca = function () {

            $scope.obj.calistAll = $scope.ClientAcquisitionlist.every(function (itm) {
                return itm.selected;
            });

            $scope.searchValueDesg == '';

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

                var BDElists = [];
                var Revenues = [];
                var Closurestatuss = [];
                var Telecallerlists = [];
                var ClientAcquisitionlists = [];
                var RegionlistLists = [];


               //BDE
                angular.forEach($scope.BDElist, function (grp_t) {
                    if (grp_t.selected) {
                        BDElists.push(grp_t.HRME_Id);
                    }
                });

              //Revenue
                angular.forEach($scope.Revenue, function (grp_t) {
                    if (grp_t.selected) {
                        Revenues.push(grp_t.revenue_range);
                    }
                });
                //closure
                angular.forEach($scope.Closurestatus, function (grp_t) {
                    if (grp_t.selected) {
                        
                        Closurestatuss.push(grp_t.ClosureStatus);
                    }
                });
                //TELECALLER
                angular.forEach($scope.Telecallerlist, function (grp_t) {
                    if (grp_t.selected) {
                        Telecallerlists.push(grp_t.HRME_Id);
                    }
                });
                //CA
                angular.forEach($scope.ClientAcquisitionlist, function (grp_t) {
                    if (grp_t.selected) {
                        ClientAcquisitionlists.push(grp_t.HRME_Id);
                    }
                });
                //region
                angular.forEach($scope.Regionlist, function (grp_t) {
                    if (grp_t.selected) {
                        RegionlistLists.push(grp_t.IVRMMS_Id);
                    }
                });
            


                $scope.fromdate11 = "";
                $scope.todate11 = "";
                $scope.fromdate11 = $filter('date')($scope.obj.fromdate, 'yyyy-MM-dd');
                $scope.todate11 = $filter('date')($scope.obj.todate, 'yyyy-MM-dd');
                $scope.submitted = true;

                //$scope.LeadCount = 0; $scope.Admincount = 0; $scope.Telcount = 0; $scope.AdminReject = 0; $scope.TelcountReject = 0;
                //$scope.leaddropdown = []; var LeadCount = 0; var Admincount = 0; var Telcount = 0; var AdminReject = 0;
                //var TelcountReject = 0;

                //var Lead_Total = 0;
                //var Lead_Done = 0;
                //var Lead_Count_NotDone = 0;
                //var Proposal_Sent = 0;
                //var Executed = 0;

                var data = {
                    "fromdateNew": $scope.fromdate11,
                    "todateNew": $scope.todate11,
                   // "flag": $scope.all1,
                    "BDElists": BDElists,
                    "Revenues" : Revenues,
                    "Closurestatuss": Closurestatuss,
                    "Telecallerlists": Telecallerlists,
                    "ClientAcquisitionlists": ClientAcquisitionlists,
                    "RegionlistLists": RegionlistLists


                }
                //leaddatabasereportdetails
                apiService.create("ISM_Client_Project/SalesPortfolioReport/", data).
                    then(function (promise) {
                        if (promise.salesPortfolioReport != null && promise.salesPortfolioReport.length > 0) {

                            $scope.SalesPortfolioReport = promise.salesPortfolioReport;
                           
                        }
                        else {
                            swal("No Records Found");
                            $state.reload();

                        }
                    })
            }
            else {

                $scope.submitted = true;
            }
        };

        $scope.clearselection = function () {
            $scope.SalesPortfolioReport = [];
            $scope.obj.fromdate = "";
            $scope.obj.todate = "";

        }


   


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
            var innerContents = document.getElementById("tableexcell").innerHTML;
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




    }
})();