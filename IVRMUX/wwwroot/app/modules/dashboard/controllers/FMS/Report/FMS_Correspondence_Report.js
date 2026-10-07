(function () {
    'use strict';
    angular.module('app').controller('FMS_CorrespondenceReportController', FMS_CorrespondenceReportController)
    FMS_CorrespondenceReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$sce','$timeout','Excel']
    function FMS_CorrespondenceReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $sce, $timeout, Excel) {

        var configsettings = JSON.parse(localStorage.getItem("configsettings"));

        var paginationformasters;
        var copty;
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
        $scope.obj = {};
        $scope.objs = {};
        $scope.objsu = {};
        $scope.objsales = {};
        $scope.objprj = {};

        $scope.flagdisplay = false;
        $scope.flagreq = false;

        /* FIRST TAB */
        $scope.onload = function () {
            var pageid = 2;
            $scope.labeldetails = "Client";
            $scope.submitted = false;
           
            apiService.getURI("FMS_Correspondance_Report/onload", pageid).then(function (promise) {
                $scope.getclient = promise.getclient;
                $scope.getsupplier = promise.getsupplier;
                $scope.getsaleslead = promise.getsaleslead;

                angular.forEach($scope.getclient, function (dd) {
                    dd.selected = true;
                });
                $scope.clientall = true;

                angular.forEach($scope.getsupplier, function (dd) {
                    dd.selected = true;
                });
                $scope.supplierall = true;

                angular.forEach($scope.getsaleslead, function (dd) {
                    dd.selected = true;
                });
                $scope.salesleadall = true;

                $scope.getprojectist();
            });
        };


        $scope.onchnageradiobtnsfile = function () {
            $scope.getclient = [];
            $scope.getsupplier = [];
            $scope.getsaleslead = [];

            var data = {
                "ClientSupplierFlgflag": $scope.ClientSupplierFlgflag
            };

            apiService.create("FMS_Correspondance_Report/onchnageradiobtnsfile", data).then(function (promise) {

                if ($scope.ClientSupplierFlgflag === 'Client') {
                    $scope.getclient = promise.getclient;
                    angular.forEach($scope.getclient, function (dd) {
                        dd.selected = true;
                    });
                    $scope.clientall = true;

                    $scope.getprojectist();
                }
                else if ($scope.ClientSupplierFlgflag === 'Supplier') {
                    $scope.flagdisplay = false;
                    $scope.flagreq = false;
                    $scope.getsupplier = promise.getsupplier;
                    angular.forEach($scope.getsupplier, function (dd) {
                        dd.selected = true;
                    });
                    $scope.supplierall = true;
                } else if ($scope.ClientSupplierFlgflag === 'Sales Leads') {
                    $scope.getsaleslead = promise.getsaleslead;
                    $scope.flagdisplay = false;
                    $scope.flagreq = false;
                    angular.forEach($scope.getsaleslead, function (dd) {
                        dd.selected = true;
                    });
                    $scope.salesleadall = true;
                }   
                else if ($scope.ClientSupplierFlgflag === 'File Category') {
                    $scope.getfilecategory = promise.getfilecategory;
                    $scope.flagdisplay = false;
                    $scope.flagreq = false;
                    angular.forEach($scope.getfilecategory, function (dd) {
                        dd.selected = true;
                    });
                    $scope.salesleadall = true;
                } 

            });
        };

        $scope.onchangeflag = function (objflag) {
            $scope.flagdisplay = false;
            $scope.flagreq = false;
            if (objflag.flag === "All") {
                $scope.flagdisplay = false;
                $scope.flagreq = false;
            } else if (objflag.flag === "With Project") {
                $scope.flagdisplay = true;
                $scope.flagreq = true;
            } else if (objflag.flag === "With Out Project") {
                $scope.flagdisplay = false;
                $scope.flagreq = false;
            }
        };

        $scope.objs.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.ismmclT_ClientName).indexOf(angular.lowercase($scope.objs.searchchkbx)) >= 0;
        };
      

        $scope.objsu.searchsupplier = '';
        $scope.filterValuesupplier = function (obj) {
            return angular.lowercase(obj.invmS_SupplierName).indexOf(angular.lowercase($scope.objsu.searchsupplier)) >= 0;
        };

        $scope.objsales.searchsales = '';
        $scope.filterValuesaleslead = function (obj) {
            return angular.lowercase(obj.ismslE_LeadName).indexOf(angular.lowercase($scope.objsales.searchsales)) >= 0;
        };

        $scope.objsales.searchfile = '';
        $scope.filterValuefilecategory = function (obj) {
            return angular.lowercase(obj.fmsmfC_FileCategoryName).indexOf(angular.lowercase($scope.objsales.searchfile)) >= 0;
        };

        $scope.objprj.searchproject = '';
        $scope.filterValueproject = function (obj) {
            return angular.lowercase(obj.ismmpR_ProjectName).indexOf(angular.lowercase($scope.objprj.searchproject)) >= 0;
        };


        /* CLIENT SELECTION */
        $scope.all_checkclient = function (clientselectedAll) {
            var toggleStatus = clientselectedAll;
            angular.forEach($scope.getclient, function (itm) {
                itm.selected = toggleStatus;
            });

            $scope.getprojectist();
        };

        $scope.togchkbxdept = function (groupType) {
            $scope.clientall = $scope.getclient.every(function (itm) {
                return itm.selected;
            });   

            $scope.getprojectist();
        };

        $scope.isOptionsRequired = function () {
            return !$scope.getclient.some(function (options) {
                return options.selected;
            });
        };


        /* SUPPLIER SELECTION */
        $scope.all_checksupplier = function (supplierselectedAll) {
            var toggleStatus = supplierselectedAll;
            angular.forEach($scope.getsupplier, function (itm) {
                itm.selected = toggleStatus;
            });
        };

        $scope.togchkbxsuplier = function (groupType) {
            $scope.supplierall = $scope.getsupplier.every(function (itm) {
                return itm.selected;
            });
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.getsupplier.some(function (options) {
                return options.selected;
            });
        };



        /*File Category */
        $scope.all_filecatogory = function (salesleadselectedAll) {
            var toggleStatus = salesleadselectedAll;
            angular.forEach($scope.getfilecategory, function (itm) {
                itm.selected = toggleStatus;
            });
        };
        $scope.togchkbxfilecategory = function (groupType) {
            $scope.salesleadall = $scope.getfilecategory.every(function (itm) {
                return itm.selected;
            });
        };

        
        /* SALES LEAD SELECTION */
        $scope.all_checksalesead = function (salesleadselectedAll) {
            var toggleStatus = salesleadselectedAll;
            angular.forEach($scope.getsaleslead, function (itm) {
                itm.selected = toggleStatus;
            });
        };




        $scope.togchkbxsaleslead = function (groupType) {
            $scope.salesleadall = $scope.getsaleslead.every(function (itm) {
                return itm.selected;
            });
        };

        $scope.isOptionsRequired2 = function () {
            return !$scope.getsaleslead.some(function (options) {
                return options.selected;
            });
        };
        $scope.isOptionsRequired3 = function () {
            return !$scope.getsaleslead.some(function (options) {
                return options.selected;
            });
        };


        /* PROJECT SELECTION */
        $scope.all_checkproject = function (projectselectedall) {
            var toggleStatus = projectselectedall;
            angular.forEach($scope.filterValue53, function (itm) {
                itm.selected = toggleStatus;
            });
        };

        $scope.togchkbxproject = function (groupType) {
            $scope.projectall = $scope.getprojectist.every(function (itm) {
                return itm.selected;
            });
        };

        $scope.isOptionsRequired23 = function () {
            return !$scope.getprojectist.some(function (options) {
                return options.selected;
            });
        };

        $scope.getprojectist = function () {
            $scope.tempclientarray = [];

            angular.forEach($scope.getclient, function (dd) {
                if (dd.selected === true) {
                    $scope.tempclientarray.push({ ISMMCLT_Id: dd.ismmclT_Id});
                }
            });
            if ($scope.tempclientarray.length > 0) {
                var data = {
                    "FMS_Correspondance_Report_Client_DTO": $scope.tempclientarray
                };

                apiService.create("FMS_Correspondance_Report/getprojectist", data).then(function (promise) {

                    if (promise !== null) {
                        $scope.getproject = promise.getproject;

                        if ($scope.getproject === null || $scope.getproject.length === 0) {
                            swal("No Project Details Found");
                        } else {
                            angular.forEach($scope.getproject, function (dd) {
                                dd.selected = true;
                            });
                            $scope.projectall = true;
                            $scope.flagdisplay = true;
                        }                        
                    }
                });
            }
        };

        $scope.onchangedate = function () {
            $scope.To_FMSCOR_Date = null;
        };
        $scope.obj = {};
        $scope.getreports = function () {

            if ($scope.myForm2.$valid) {
                var data = "";
                if ($scope.ClientSupplierFlgflag === 'All') {
                    data = {
                        "Fromdate": new Date($scope.From_FMSCOR_Date).toDateString(),
                        "Todate": new Date($scope.To_FMSCOR_Date).toDateString(),
                        "ClientSupplierFlgflag": $scope.ClientSupplierFlgflag,
                        "flag": $scope.obj.flag
                    };

                } else if ($scope.ClientSupplierFlgflag === 'Client') {

                    $scope.tempselectedclientarray = [];
                    angular.forEach($scope.getclient, function (dd) {
                        if (dd.selected === true) {
                            $scope.tempselectedclientarray.push({ ISMMCLT_Id: dd.ismmclT_Id });
                        }
                    });

                    $scope.tempselectedprojectarray = [];
                    angular.forEach($scope.getproject, function (dd) {
                        if (dd.selected === true) {
                            $scope.tempselectedprojectarray.push({ ISMMPR_Id: dd.ismmpR_Id });
                        }
                    });

                    data = {
                        "Fromdate": new Date($scope.From_FMSCOR_Date).toDateString(), 
                        "Todate": new Date($scope.To_FMSCOR_Date).toDateString(),
                        "ClientSupplierFlgflag": $scope.ClientSupplierFlgflag,
                        "flag": $scope.obj.flag,
                        "FMS_Correspondance_Report_Supplier_DTO": $scope.tempselectedclientarray,
                        "FMS_Correspondance_Report_Project_DTO": $scope.FMS_Correspondance_Report_Project_sDTO
                    };
                }
                else if ($scope.ClientSupplierFlgflag === 'Supplier') {

                    $scope.tempselectedsupplierarray = [];
                    angular.forEach($scope.getsupplier, function (dd) {
                        if (dd.selected === true) {
                            $scope.tempselectedsupplierarray.push({ INVMS_Id: dd.invmS_Id });
                        }
                    });                   

                    data = {
                        "Fromdate": new Date($scope.From_FMSCOR_Date).toDateString(),
                        "Todate": new Date($scope.To_FMSCOR_Date).toDateString(),
                        "ClientSupplierFlgflag": $scope.ClientSupplierFlgflag,
                        "flag": $scope.obj.flag,
                        "FMS_Correspondance_Report_Supplier_DTO": $scope.tempselectedsupplierarray                         
                    };
                }
                else if ($scope.ClientSupplierFlgflag === 'Sales Lead') {

                    $scope.tempselectedsalesleadarray = [];
                    angular.forEach($scope.getsaleslead, function (dd) {
                        if (dd.selected === true) {
                            $scope.tempselectedsalesleadarray.push({ ISMSLE_Id: dd.ismslE_Id });
                        }
                    });

                    data = {
                        "Fromdate": new Date($scope.From_FMSCOR_Date).toDateString(),
                        "Todate": new Date($scope.To_FMSCOR_Date).toDateString(),
                        "ClientSupplierFlgflag": $scope.ClientSupplierFlgflag,
                        "flag": $scope.obj.flag,
                        "FMS_Correspondance_Report_Supplier_DTO": $scope.tempselectedsalesleadarray
                    };
                }
                else if ($scope.ClientSupplierFlgflag === 'File Category') {

                    $scope.tempselectedsalesleadarray = [];
                    angular.forEach($scope.getfilecategory, function (dd) {
                        if (dd.selected === true) {
                            $scope.tempselectedsalesleadarray.push({ FMSMFC_Id: dd.fmsmfC_Id });
                        }
                    });

                    data = {
                        "Fromdate": new Date($scope.From_FMSCOR_Date).toDateString(),
                        "Todate": new Date($scope.To_FMSCOR_Date).toDateString(),
                        "ClientSupplierFlgflag": $scope.ClientSupplierFlgflag,                        
                        "File_Category_DTO": $scope.tempselectedsalesleadarray
                    };
                }
                apiService.create("FMS_Correspondance_Report/getreport", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.getreport = promise.getreport;
                        if ($scope.getreport === null || $scope.getreport.length === 0) {
                            swal("No Records Found");
                        }
                    }
                });

            } else {
                $scope.submitted2 = true;
            }
        };


        $scope.viewreferencedetais = function (userd) {
            var data = {
                "FMSCOR_Id": userd.FMSCOR_Id
            };

            apiService.create("FMS_Correspondance_Report/viewreferencedetais", data).then(function (promise) {

                if (promise !== null) {
                    $scope.getcabintdetails = promise.getcabintdetails;
                    $scope.getfilesdetails = promise.getfilesdetails;
                    $scope.getmodedetails = promise.getmodedetails;                    

                    angular.forEach($scope.getfilesdetails, function (dd) {
                        if (dd.FMSCORFL_SoftCopyFilePath !== null && dd.FMSCORFL_SoftCopyFilePath !== '') {
                            var img = dd.FMSCORFL_SoftCopyFilePath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            console.log("dd.filetype : " + dd.filetype);
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.FMSCORFL_SoftCopyFilePath;
                            }
                        }
                    });

                    angular.forEach($scope.getmodedetails, function (dd) {
                        if (dd.FMSCORMD_FilePath !== null && dd.FMSCORMD_FilePath !== '') {
                            var img = dd.FMSCORMD_FilePath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            console.log("dd.filetype : " + dd.filetype);
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.FMSCORMD_FilePath;
                            }
                        }
                    });

                    $scope.refernceno = userd.FMSCOR_RefernceNo;

                    if (($scope.getmodedetails === null || $scope.getmodedetails.length === 0)
                        && ($scope.getfilesdetails === null || $scope.getfilesdetails.length === 0)
                            ($scope.getcabintdetails === null || $scope.getcabintdetails.length === 0)) {
                        swal("No Records Found");
                    }
                }
            });
        };


        $scope.interacted1 = function (field) {
            return $scope.submitted2;
        };


        $scope.getclear = function () {
            $state.reload();
        };

        $scope.exportToExceldetails = function () {
            
            var excelname = 'FMS Correspondence Report';
            excelname = excelname.toUpperCase() + '.xls';
            //var printSectionId = export_id;s
            var exportHref = Excel.tableToExcel('#exceldetail', 'FMS Correspondence Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.printDatadetails = function () {
            var innerContents = "";
            innerContents = document.getElementById("printdetail").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExceldetails1 = function () {

            var excelname = 'FMS Correspondence Report';
            excelname = excelname.toUpperCase() + '.xls';
            //var printSectionId = export_id;s
            var exportHref = Excel.tableToExcel('#excelreferncenode', 'FMS Correspondence Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.printDatadetails1 = function () {
            var innerContents = "";
            innerContents = document.getElementById("printreferncenode").innerHTML;
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
        $scope.downloaddirectimage = function (data, idd, typeofphoto, type) {

            var studentreg = idd;

            $scope.imagedownload = data;
            imagedownload = data;
            docname = typeofphoto;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: idd
                    })[0].click();
                });
        };

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.fmscorfL_SoftCopyFilePath;
            $scope.videdfd = data.fmscorfL_SoftCopyFilePath;
            $scope.movie = { src: data.fmscorfL_SoftCopyFilePath };
            $scope.movie1 = { src: data.fmscorfL_SoftCopyFilePath };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.fmscorfL_SoftCopyFilePath });
            console.log($scope.view_videos);
        };

        $scope.onview = function (filepath, filename) {
            //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
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

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };
    }

    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });

    angular.module('app').directive('txtArea', function () {
        return {
            restrict: 'AE',
            replace: 'true',
            scope: { data: '=', model: '=ngModel' },
            template: "<textarea></textarea>",
            link: function (scope, elem) {
                scope.$watch('data', function (newVal) {
                    if (newVal) {
                        scope.model += newVal[0];
                    }
                });
            }
        };
    });
})();