(function () {
    'use strict';
    angular.module('app').controller('flathistoryreportController', flathistoryreportController)

    flathistoryreportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter', '$q', '$sce', '$window']
    function flathistoryreportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter, $q, $sce, $window) {

        $scope.groups = [{ title: 'Dynamic Group Header - 1', content: 'Dynamic Group Body - 1' },
        { title: 'Dynamic Group Header - 2', content: 'Dynamic Group Body - 2' },
        { title: 'Dynamic Group Header - 3', content: 'Dynamic Group Body - 3' },
        { title: 'Dynamic Group Header - 4', content: 'Dynamic Group Body - 4' }];

        $scope.searc_button = true;
        $scope.sortKey = 'OTMOEQ_Id';
        $scope.sortReverse = true;
        $scope.OTMOEQ_SubjectiveFlg = false;
        $scope.edit = false;
        $scope.getexamhappenedcounttemp = false;

        $scope.duration = true;
        $scope.aggstartdate = false;
        $scope.aggenddate = false;

        $scope.answer = "";
        $scope.show_ansOption = false;
        $scope.obj = {};
        $scope.obj.searchValueddd = "";
        $scope.searchValue22 = "";

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.currentPage1 = 1;
        $scope.currentPage2 = 1;
        $scope.currentPage3 = 1;
        $scope.btn = false;
        

        $scope.loaddata = function () {
            var pageid = 2;
            $scope.grid = false;

            apiService.getURI("TenantAgreement/getdata/", pageid).then(function (promise) {

               
                $scope.building = promise.building;
                $scope.rooms = promise.rooms;
                //$scope.getloaddata = promise.getloaddata;
               
                //$scope.milist = promise.milist;
                //$scope.getarratcomplexities = promise.getarratcomplexities;
            });
        };
        
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printDeviation1").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        //added by anvesha
        $scope.getreport = function () {
            $scope.submitted = true;
            $scope.grid = true;
            if ($scope.myForm.$valid) {
          
                var data = {

                    "HRMB_Id": $scope.HRMB_Id,
                    "HRMR_Id": $scope.HRMR_Id,

                }



                apiService.create("flathistoryreport/get_report", data).then(function (promise) {
                    $scope.plannerid = [];
                    if (promise.getloaddata.length > 0) {

                        $scope.grid = true;
                        $scope.getloaddata = promise.getloaddata;
                        $scope.presentCountgrid = promise.getloaddata.length;
                    }
                    else {
                        swal("No Record Found..... !!");
                        $scope.get_printdata = "";
                        $state.reload();
                    }

                });

            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
       

        $scope.buildingchange = function () {

            var data = {
                "HRMB_Id": $scope.HRMB_Id,

            };

            apiService.create("flathistoryreport/buildingchange", data).then(function (promise) {
                if (promise !== null) {
                    $scope.rooms = promise.rooms;
                   
                }
            });
        };

        //added by anvesha

        $scope.ViewHealthCheckUpFiles = function (obj) {

            var data = {
                "TENANTAGR_Id": obj.tenantagR_Id,
                
                "MI_Id": $scope.MI_Id
            };

            apiService.create("flathistoryreport/ViewHealthCheckUpFiles", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getviewfilesdetails !== null && promise.getviewfilesdetails.length > 0) {
                        $scope.getviewfilesdetails = promise.getviewfilesdetails;
                        angular.forEach($scope.getviewfilesdetails, function (dd) {
                            if (dd.tenantagrfL_FileLocation !== null && dd.tenantagrfL_FileLocation !== "") {
                                var img = dd.tenantagrfL_FileLocation;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("dd.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.filepath;
                                }
                                else {
                                    dd.document_Pathnew = img
                                }
                            }
                        });
                        $('#modalviewfiles').modal('show');
                    } else {
                        swal("No Records Found");
                        $('#modalviewfiles').modal('hide');
                    }
                }
            });
        };

        $scope.ViewFile = function (docpath, filename, filetype) {

            if (filetype === 'Excel') {
                docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + docpath;
                $scope.detailFrame = $sce.trustAsResourceUrl(docpath);
                $('#myModaldocx').modal('show');
            } else {
                var imagedownload = docpath;
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
            }
        };


        $scope.downloaddirectimage = function (data, idd, type) {
            var studentreg = idd;
            $scope.imagedownload = data;
            var imagedownload = data;
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
        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalPreview').modal('show');
        };

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

        $scope.ViewDocExcelfles = function (docpath, filename, filetype) {
            docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + docpath;
            $scope.detailFrame = $sce.trustAsResourceUrl(docpath);
            $('#myModaldocx').modal('show');
        };

       
        $scope.exportToExcel = function (export_id) {
            var an = $scope.stdall;
            if ($scope.printdatatable.length> 0) {
                var exportHref = Excel.tableToExcel(export_id, 'printDeviation1');
                $timeout(function () {
                    location.href = exportHref;
                }, 100);
            }

            else {
  
                swal("select atleast one record!");
            }
        };
        
        $scope.printdatatable = [];
        $scope.optionToggledstd = function (SelectedStudentRecord, index) {

            $scope.stdall = $scope.getloaddata.every(function (itm) {
                return itm.stdselected;
            });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }


            if ($scope.get_printdata1.indexOf(SelectedStudentRecord) === -1) {
                $scope.get_printdata1.push(SelectedStudentRecord);
            }
            else {
                $scope.get_printdata1.splice($scope.get_printdata1.indexOf(SelectedStudentRecord), 1);
            }

            if ($scope.get_printdata1.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }


        }

        $scope.toggleAllstd = function () {

            var toggleStatus = $scope.stdall;
            angular.forEach($scope.getloaddata, function (itm) {


                itm.stdselected = toggleStatus;
                if (itm.stdselected == true) {
                    $scope.printdatatable.push(itm);
                    $scope.getloaddata.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                    $scope.getloaddata.splice(itm);
                }
            });


            if ($scope.printdatatable.length != null) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }

        }

      

        $scope.roomchange = function () {

            var data = {
                "HRMB_Id": $scope.HRMB_Id,
                "HRMR_Id": $scope.HRMR_Id,

            };

            apiService.create("flathistoryreport/roomchange", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getamenitiesdetails = promise.getamenitiesdetails;
                }
            });
        };
      
        $scope.cleartabl1 = function () {
            $state.reload();
        }

        };
    
    

})();