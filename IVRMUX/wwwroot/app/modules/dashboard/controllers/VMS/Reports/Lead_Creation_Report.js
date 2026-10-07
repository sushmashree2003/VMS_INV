(function () {
    'use strict';
    angular
        .module('app')
        .controller('Lead_Creation_ReportController', lead_Creation_ReportController);

    lead_Creation_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce', '$timeout', 'Excel', 'superCache'];
    function lead_Creation_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce, $timeout, Excel, superCache) {

      

        //=============image
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.maxDatemf = new Date();
        //loading start
        $scope.collead = [{
            title: "SLNO",
            template: "<span class='row-number'></span>", width: "70px"
        },
        {
            name: 'ismslE_LeadName', field: 'ismslE_LeadName', title: 'Lead Name', width: "100px"
        },
        {
            name: 'ismslE_ContactPerson', field: 'ismslE_ContactPerson', title: 'Contact Person', width: "100px"
        },
        //ismslE_StudentStrength
        {
            name: 'ismslE_StudentStrength', field: 'ismslE_StudentStrength', title: 'Student Strength', width: "100px"
        },
        //ismslE_StaffStrength
        {
            name: 'ismslE_StaffStrength', field: 'ismslE_StaffStrength', title: 'Staff Strength', width: "100px"
        },
        //ismslE_NoOfInstitutions
        {
            name: 'ismslE_NoOfInstitutions', field: 'ismslE_NoOfInstitutions', title: 'No Of Institutions', width: "100px"
        },
        {
            name: 'ismslE_ContactNo', field: 'ismslE_ContactNo', title: 'Contact Number', width: "100px"
        },
        //ismslE_EmailId
        {
            name: 'ismslE_EmailId', field: 'ismslE_EmailId', title: 'Email Id', width: "100px"
        },
        {
            name: 'ismslE_LeadAddress', field: 'ismslE_LeadAddress', title: 'Lead Address', width: "100px"
        },
        {
            name: 'ismsmsT_StatusName', field: 'ismsmsT_StatusName', title: 'Status Name', width: "100px"
        },
        {
            name: 'ismsmsO_SourceName', field: 'ismsmsO_SourceName', title: 'Source Name', width: "100px"
        },
        {
            name: 'ismsmcA_CategoryName', field: 'ismsmcA_CategoryName', title: 'Category Name', width: "100px"
        },
        //IVRMMC_CountryName
        {
            name: 'IVRMMC_CountryName', field: 'IVRMMC_CountryName', title: 'Country Name', width: "100px"
        },
        //IVRMMS_Name
        {
            name: 'IVRMMS_Name', field: 'IVRMMS_Name', title: 'State Name', width: "100px"
        },
        {
            name: 'ISMSMPR_ProductName', field: 'ISMSMPR_ProductName', title: 'Product Name', width: "100px"
        },
        {
            name: 'CreatedDate', field: 'CreatedDate', title: 'Created Date', width: "100px"
        },
        {
            name: 'ismslE_Remarks', field: 'ismslE_Remarks', title: 'Remark', width: "100px"
        },
            //ismslE_Remarks
            //{
            //    name: 'CreatedDate', field: 'CreatedDate', title: 'Created Date', width: "130", template: "#= kendo.toString(kendo.parseDate(CreatedDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
            //}


        ];

        $scope.IMFY_Id = 0;
        $scope.athflag = false;
        $scope.loadData = function () {
            var id = 2;

            apiService.getURI("SalesSMSEMAIL/getalldetails", id).
                then(function (promise) {

                    $scope.categorylist = promise.categorylist;
                    $scope.sourcelist = promise.sourcelist;
                    $scope.productlist = promise.productlist;
                    $scope.statuslist = promise.statuslist;
                    $scope.countrylist = promise.countrylist;
                    $scope.channalPatnerList = promise.channalPatnerList;
                    $scope.yearlist = promise.yearlist;
                    // $scope.All_Individual();
                    $scope.grid_view = false;
                });
        };

        $scope.searchValue = '';
        $scope.itemsPerPage = 15;
        $scope.currentPage = 1;

        ///CATEGORY SEARCH START
        $scope.searchchkbx = '';
        $scope.filterchkbxhous = function (obj) {
            return (angular.lowercase(obj.ismsmcA_CategoryName)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }

        $scope.togchkbxhouse = function () {
            $scope.usercheckhous = $scope.categorylist.every(function (options) {
                return options.housselect;
            });
        }
        $scope.all_checkhous = function () {
            var checkStatus = $scope.usercheckhous;
            angular.forEach($scope.categorylist, function (itm) {
                itm.housselect = checkStatus;
            });
        }

        ///CATEGORY SEARCH END 
        ///SOURCE SEARCH START
        $scope.searchchkbx1 = '';
        $scope.filterchkbxhous1 = function (obj) {
            return (angular.lowercase(obj.ismsmsO_SourceName)).indexOf(angular.lowercase($scope.searchchkbx1)) >= 0;
        }

        $scope.togchkbxhouse1 = function () {
            $scope.usercheckhous1 = $scope.sourcelist.every(function (options) {
                return options.select;
            });
        }
        $scope.all_checkhous1 = function () {
            var checkStatus = $scope.usercheckhous1;
            angular.forEach($scope.sourcelist, function (itm) {
                itm.select = checkStatus;
            });
        }

        ///SOURCE SEARCH END


        ///PRODUCT SEARCH START
        $scope.searchchkbx2 = '';
        $scope.filterchkbxhous2 = function (obj) {
            return (angular.lowercase(obj.ismsmpR_ProductName)).indexOf(angular.lowercase($scope.searchchkbx2)) >= 0;
        }

        $scope.togchkbxhouse2 = function () {
            $scope.usercheckhous2 = $scope.productlist.every(function (options) {
                return options.select;
            });
        }
        $scope.all_checkhous2 = function () {
            var checkStatus = $scope.usercheckhous2;
            angular.forEach($scope.productlist, function (itm) {
                itm.select = checkStatus;
            });
        }

        ///PRODUCT SEARCH END
        ///STATUS SEARCH START
        $scope.searchchkbx3 = '';
        $scope.filterchkbxhous3 = function (obj) {
            return (angular.lowercase(obj.ismsmsT_StatusName)).indexOf(angular.lowercase($scope.searchchkbx3)) >= 0;
        }

        $scope.togchkbxhouse3 = function () {
            $scope.usercheckhous3 = $scope.statuslist.every(function (options) {
                return options.select;
            });
        }
        $scope.all_checkhous3 = function () {
            var checkStatus = $scope.usercheckhous3;
            angular.forEach($scope.statuslist, function (itm) {
                itm.select = checkStatus;
            });
        }

        ///STATUS SEARCH END

        ///COUNTRY SEARCH START
        $scope.searchchkbx4 = '';
        $scope.filterchkbxhous4 = function (obj) {
            return (angular.lowercase(obj.ivrmmC_CountryName)).indexOf(angular.lowercase($scope.searchchkbx4)) >= 0;
        }

        $scope.togchkbxhouse4 = function () {
            $scope.usercheckhous5 = false;
            $scope.usercheckhous4 = $scope.countrylist.every(function (options) {
                return options.select;
            });
            $scope.get_state();
        }
        $scope.all_checkhous4 = function () {
            $scope.usercheckhous5 = false;
            var checkStatus = $scope.usercheckhous4;
            angular.forEach($scope.countrylist, function (itm) {
                itm.select = checkStatus;
            });

            $scope.get_state();
        }

        ///COUNTRY SEARCH END 
        ///COUNTRY SEARCH START
        $scope.searchchkbx5 = '';
        $scope.filterchkbxhous5 = function (obj) {
            return (angular.lowercase(obj.ivrmmS_Name)).indexOf(angular.lowercase($scope.searchchkbx5)) >= 0;
        }

        $scope.togchkbxhouse5 = function () {
            $scope.usercheckhous5 = $scope.statelist.every(function (options) {
                return options.select;
            });


        }
        $scope.all_checkhous5 = function () {
            var checkStatus = $scope.usercheckhous5;
            angular.forEach($scope.statelist, function (itm) {
                itm.select = checkStatus;
            });


        }

        ///COUNTRY SEARCH END

        ///ChannelPartner SEARCH START
        $scope.searchchkbx6 = '';
        $scope.filterchkbxhous6 = function (obj) {
            return (angular.lowercase(obj.ismsmcP_ChannelPartnerName)).indexOf(angular.lowercase($scope.searchchkbx6)) >= 0;
        }

        $scope.togchkbxhouse6 = function () {
            $scope.usercheckhous6 = $scope.channalPatnerList.every(function (options) {
                return options.housselect;
            });
        }
        $scope.all_checkhous6 = function () {
            var checkStatus = $scope.usercheckhous6;
            angular.forEach($scope.channalPatnerList, function (itm) {
                itm.housselect = checkStatus;
            });
        }
        ///ChannelPartner SEARCH End

        //validation start
        $scope.interacted = function (field) {

            return $scope.submitted;
        };
        $scope.isOptionsRequired = function () {
            return !$scope.staff_types.some(function (options) {
                return options.selected;
            });
        };
        $scope.isOptionsRequired1 = function () {
            return !$scope.Department_types.some(function (options) {
                return options.selected;
            });
        };
        $scope.isOptionsRequired2 = function () {
            return !$scope.Designation_types.some(function (options) {
                return options.selected;
            });
        };
        $scope.sort = function (keyname) {

            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        //validation end
        //all-individual start




        //all-check button end
        // fill dep start
        $scope.get_state = function () {

            var groupidss = [];
            for (var i = 0; i < $scope.countrylist.length; i++) {
                if ($scope.countrylist[i].select === true) {
                    groupidss.push({ ivrmmC_Id: $scope.countrylist[i].ivrmmC_Id })
                }
            }
            if (groupidss !== undefined) {
                var data = {
                    stateids: groupidss,
                };
                apiService.create("SalesSMSEMAIL/get_state", data).
                    then(function (promise) {

                        $scope.statelist = promise.statelist;


                    });
            }

        };
        $scope.snd_email = false;
        $scope.snd_sms = false;
        $scope.printstudents = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.leadlist, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all === true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
        }

        $scope.optionToggled1 = function (SelectedStudentRecord, index) {

            $scope.all = $scope.leadlist.every(function (options) {

                return options.selected;
            });
        }

        $scope.viewfunction = function () {
            $scope.grid_view = false;
            $scope.upcomingintvw = [];
            $scope.inprogressintvw = [];
            $scope.completedintvw = [];
        };
        $scope.namesearch = '';
        //fill employee end
        //get report start
        $scope.grid_view = false;
        $scope.submitted = false;
        $scope.leadlist = [];
        $scope.GetReport = function () {
            if ($scope.IMFY_Id == undefined || $scope.IMFY_Id == null || $scope.IMFY_Id == 0) {
                swal("Please select the Financial Year");
                return;
                //break;
            }



            // Build catIds
            var catIds;
            for (var i = 0; i < $scope.categorylist.length; i++) {
                if ($scope.categorylist[i].housselect === true) {
                    catIds = catIds ? catIds + "," + $scope.categorylist[i].ismsmcA_Id : $scope.categorylist[i].ismsmcA_Id;
                }
            }


            if (catIds == undefined || catIds == null) {
                swal("Please select the Category");
                return;

            }



            // Build soursIds
            var soursIds;
            for (var i = 0; i < $scope.sourcelist.length; i++) {
                if ($scope.sourcelist[i].select === true) {
                    soursIds = soursIds ? soursIds + "," + $scope.sourcelist[i].ismsmsO_Id : $scope.sourcelist[i].ismsmsO_Id;
                }
            }


            if (soursIds == undefined || soursIds == null) {
                swal("Please select the Source");
                return;

            }


            // Build prodidss
            var prodidss;
            for (var i = 0; i < $scope.productlist.length; i++) {
                if ($scope.productlist[i].select === true) {
                    prodidss = prodidss ? prodidss + "," + $scope.productlist[i].ismsmpR_Id : $scope.productlist[i].ismsmpR_Id;
                }
            }


            if (prodidss == undefined || prodidss == null) {
                swal("Please select the Product");
                return;

            }

            // Build contryidss
            var contryidss;
            for (var i = 0; i < $scope.countrylist.length; i++) {
                if ($scope.countrylist[i].select === true) {
                    contryidss = contryidss ? contryidss + "," + $scope.countrylist[i].ivrmmC_Id : $scope.countrylist[i].ivrmmC_Id;
                }
            }




            if (contryidss == undefined || contryidss == null) {
                swal("Please select the Country");
                return;

            }



            // Build statidss
            var stsidss;
            if ($scope.statelist !== undefined && $scope.statelist !== null) {
                for (var i = 0; i < $scope.statelist.length; i++) {
                    if ($scope.statelist[i].select === true) {
                        stsidss = stsidss ? stsidss + "," + $scope.statelist[i].ivrmmS_Id : $scope.statelist[i].ivrmmS_Id;
                    }
                }
            }



            if (stsidss == undefined || stsidss == null) {
                swal("Please select the State");
                return;

            }
            
            //$scope.leadlist = [];
            //var catIds;
            //for (var i = 0; i < $scope.categorylist.length; i++) {
            //    if ($scope.categorylist[i].housselect === true) {

            //        if (catIds === undefined)
            //            catIds = $scope.categorylist[i].ismsmcA_Id;
            //        else
            //            catIds = catIds + "," + $scope.categorylist[i].ismsmcA_Id;
            //    }
            //}


            

            //var channelIds;
            //for (var i = 0; i < $scope.channalPatnerList.length; i++) {
            //    if ($scope.channalPatnerList[i].housselect === true) {

            //        if (channelIds === undefined)
            //            channelIds = $scope.channalPatnerList[i].ismsmcP_Id;
            //        else
            //            channelIds = channelIds + "," + $scope.channalPatnerList[i].ismsmcP_Id;
            //    }
            //}

            //var soursIds;
            //for (var i = 0; i < $scope.sourcelist.length; i++) {
            //    if ($scope.sourcelist[i].select === true) {

            //        if (soursIds === undefined)
            //            soursIds = $scope.sourcelist[i].ismsmsO_Id;
            //        else
            //            soursIds = soursIds + "," + $scope.sourcelist[i].ismsmsO_Id;
            //    }
            //}

            

            //var prodidss;
            //for (var i = 0; i < $scope.productlist.length; i++) {
            //    if ($scope.productlist[i].select === true) {

            //        if (prodidss === undefined)
            //            prodidss = $scope.productlist[i].ismsmpR_Id;
            //        else
            //            prodidss = prodidss + "," + $scope.productlist[i].ismsmpR_Id;
            //    }
            //}

         


            //var statussidss;
            //for (var i = 0; i < $scope.statuslist.length; i++) {
            //    if ($scope.statuslist[i].select === true) {

            //        if (statussidss === undefined)
            //            statussidss = $scope.statuslist[i].ismsmsT_Id;
            //        else
            //            statussidss = statussidss + "," + $scope.statuslist[i].ismsmsT_Id;
            //    }
            //}
            //var contryidss;
            //for (var i = 0; i < $scope.countrylist.length; i++) {
            //    if ($scope.countrylist[i].select === true) {

            //        if (contryidss === undefined)
            //            contryidss = $scope.countrylist[i].ivrmmC_Id;
            //        else
            //            contryidss = contryidss + "," + $scope.countrylist[i].ivrmmC_Id;
            //    }
            //}


            //var stsidss;

            //if ($scope.statelist !== undefined && $scope.statelist !== null) {
            //    for (var i = 0; i < $scope.statelist.length; i++) {
            //        if ($scope.statelist[i].select === true) {

            //            if (stsidss === undefined)
            //                stsidss = $scope.statelist[i].ivrmmS_Id;
            //            else
            //                stsidss = stsidss + "," + $scope.statelist[i].ivrmmS_Id;
            //        }
            //    }
            //}

      



            if (($scope.IMFY_Id === null || $scope.IMFY_Id === '' ) &&  stsidss === undefined  && contryidss === undefined && catIds === undefined && channelIds === undefined && soursIds === undefined && statussidss === undefined && prodidss === undefined && ($scope.namesearch === undefined || $scope.namesearch === '' || $scope.namesearch === null) && ($scope.contactname === undefined || $scope.contactname === '' || $scope.contactname === null) && ($scope.mobilesearch === undefined || $scope.mobilesearch === '' || $scope.mobilesearch === null) && ($scope.emailsearch === undefined || $scope.emailsearch === '' || $scope.emailsearch === null)) {
                swal('Select Atleast one set of search Parameter')
            }
            else {
                if ($scope.startdate !== null && $scope.startdate !== undefined) {
                    $scope.start_Date = $filter('date')($scope.startdate, "yyyy-MM-dd");
                }
                else {
                    $scope.start_Date = "";
                }
                if ($scope.enddate !== null && $scope.enddate !== undefined) {
                    $scope.end_Date = $filter('date')($scope.enddate, "yyyy-MM-dd");
                }
                else {
                    $scope.end_Date = "";
                }
                var data = {
                    "catIds": catIds,
                    "soursIds": soursIds,
                    "prodidss": prodidss,
                    "contryidss": contryidss,
                    "statidss": stsidss
                };


                //var data = {
                //    "searchstring": $scope.namesearch,
                //    "contactname": $scope.contactname,
                //    "mobilesearch": $scope.mobilesearch,
                //    "emailsearch": $scope.emailsearch,
                //    "statussidss": statussidss,
                //    "contryidss": contryidss,
                //    "channelIds": channelIds,
                //    "catIds": catIds,
                //    "soursIds": soursIds,
                //    "statidss": stsidss,
                //    "prodidss": prodidss,
                //    "start_Date": $scope.start_Date,
                //    //"end_Date": $scope.end_Date,
                //    //"IMFY_Id": $scope.IMFY_Id
                //};
                apiService.create("SalesSMSEMAIL/getrpt_lead", data).
                    then(function (promise) {

                        if (promise.leadlist !== undefined && promise.leadlist !== null && promise.leadlist.length > 0 ) {
                            $scope.grid_view = true;

                            $scope.leadlist = promise.leadlist;
                            $scope.leadlist_1 = promise.leadlist;

                            $(document).ready(function () {
                                initGridall();
                            });
                            $scope.fileName = "Sales_Lead_Creation_Report.xlsx";
                            $scope.fileNamepdf = "Sales_Lead_Creation_Report.pdf";
                            function initGridall() {
                                $('#grid567').empty();
                                //gridall =
                                $("#grid567").kendoGrid({
                                    toolbar: ["excel", "pdf"],
                                    excel: {
                                        fileName: $scope.fileName,
                                        proxyURL: "",
                                        filterable: true,
                                        allPages: true
                                    },
                                    pdf: {
                                        avoidLinks: true,
                                        landscape: true,
                                        repeatHeaders: true,
                                        fileName: $scope.fileNamepdf,
                                        allPages: true
                                    },
                                    dataSource: {
                                        data: $scope.leadlist,
                                        pageSize: 10,
                                    },
                                    sortable: true,
                                    pageable: true,
                                    groupable: false,
                                    filterable: true,
                                    columnMenu: true,
                                    reorderable: true,
                                    resizable: true,
                                    columns: $scope.collead,
                                    dataBound: function () {
                                        var pagenum = this.dataSource.page();
                                        var pageitms = this.dataSource.pageSize()
                                        var rows = this.items();
                                        $(rows).each(function () {
                                            var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                            var rowLabel = $(this).find(".row-number");
                                            $(rowLabel).html(index);
                                        });
                                    }

                                }).data("kendoGrid");
                            }
                        }
                        else {
                            swal('No Record Found')
                        }

                    });
            }
            //}
            //else {
            //    $scope.submitted = true;
            //}
        };


        $scope.printData = function () {

            if ($scope.leadlist_1 !== null && $scope.leadlist_1.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/Sports/HouseReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }
        //$scope.exportToExcel = function (export_id) {

        //    var exportHref = Excel.tableToExcel(export_id, 'sheet name');
        //    $timeout(function () {
        //        location.href = exportHref;
        //    }, 100);

        //};

        $scope.exportToExcel = function (tableId) {
            var excelname = "LEAD REPORT";
            excelname = excelname.toUpperCase() + '.xls';
            var printSectionId = tableId;
            if ($scope.leadlist !== null && $scope.leadlist.length > 0) {
                var exportHref = Excel.tableToExcel(printSectionId, 'LEAD REPORT');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = excelname;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
                //$state.reload();
            }
            else {
                swal("Please Select Records to be Exported");
            }

        };




        $scope.clearid = function () {
            $state.reload();
        };

    }

})();