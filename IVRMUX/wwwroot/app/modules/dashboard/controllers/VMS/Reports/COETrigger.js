(function () {
    'use strict';
    angular.module('app')
        .controller('COETriggerController', COETriggerController)
    COETriggerController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout','$compile']
    function COETriggerController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout, $compile) {
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
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
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
            apiService.getURI("COETrigger/getalldetails", pageid).
                then(function (promise) {
                    $scope.leadlist = promise.leadlist;
                    $scope.vendorlist = promise.vendorlist;
                   // $scope.eventlist = promise.eventlist;
                    $scope.EventDetails();

                })
        };
        $scope.searchleadvalue = function (searchleadvalue) {
            if (searchleadvalue.length > 1) {
                var data = {
                    "searchleadvalue": searchleadvalue,
                    "flag": "Search"
                }
                apiService.create("COETrigger/searchleadvalue", data).
                    then(function (promise) {
                        $scope.leadlist = promise.leadlist;

                    })
            }
        };
        $scope.all_checkdept = function (ISMSLE_Id) {
            var toggleStatus = ISMSLE_Id;
            angular.forEach($scope.leadlist, function (itm) {
                itm.selected = toggleStatus;
            });

        };

        $scope.all_checkdept1 = function (ISMSLE_Id) {
            var toggleStatus = ISMSLE_Id;
            angular.forEach($scope.vendorlist, function (itm) {
                itm.selected1 = toggleStatus;
            });

        };
        $scope.togchkbxdept = function (groupType) {
            $scope.ismslE_Id = $scope.leadlist.every(function (itm) {
                return itm.selected;
            });
        };
        $scope.togchkbxdept1 = function (groupType) {
            $scope.invmS_Id = $scope.vendorlist.every(function (itm) {
                return itm.selected1;
            });
        };
        $scope.submitted = false;
        $scope.showreport = function () {
            $scope.leaddatalist = [];
            angular.forEach($scope.leadlist, function (d) {
                if (d.selected === true) {
                    $scope.leaddatalist.push({ ISMSLE_Id: d.ISMSLE_Id });
                }
            });
            //$scope.fromdate = "";
            //$scope.todate = "";
            //$scope.fromdate = $filter('date')($scope.fromdate, 'yyyy-MM-dd');
            //$scope.todate = $filter('date')($scope.todate, 'yyyy-MM-dd');
            var data = {
                "fromdate": new Date($scope.fromdate).toDateString(),
                "todate": new Date($scope.todate).toDateString(),
                //"fromdate": $scope.fromdate,
                //"todate": $scope.todate,
                "leaddatalist": $scope.leaddatalist
            }
            //ISMSLE_FilePath 
            apiService.create("COETrigger/report_cmtrpt/", data).
                then(function (promise) {
                    if (promise.leadcommentsreport > 0 || promise.leadcommentsreport !== null) {
                        $scope.leadcommentsreport = promise.leadcommentsreport;
                        $scope.taskid5 = [];
                        angular.forEach($scope.leadcommentsreport, function (dev) {
                            if ($scope.taskid5.length === 0) {
                                $scope.taskid5.push({ ISMSLE_Id: dev.ISMSLE_Id, ISMSLE_LeadName: dev.ISMSLE_LeadName, ISMSLE_FilePath: dev.ISMSLE_FilePath });
                            } else if ($scope.taskid5.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.taskid5, function (emp) {
                                    if (emp.ISMSLE_Id === dev.ISMSLE_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.taskid5.push({ ISMSLE_Id: dev.ISMSLE_Id, ISMSLE_LeadName: dev.ISMSLE_LeadName, ISMSLE_FilePath: dev.ISMSLE_FilePath });
                                }
                            }
                        });
                        angular.forEach($scope.taskid5, function (ddd) {
                            $scope.templist = [];
                            angular.forEach($scope.leadcommentsreport, function (dd) {
                                if (dd.ISMSLE_Id === ddd.ISMSLE_Id) {
                                    $scope.templist.push(dd);
                                }
                            });
                            ddd.leadcommentsreport = $scope.templist;
                        });
                    }
                    else {
                        swal('No Data Found!!!')
                    }
                })
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


        //change()

        $scope.EventDetails = function () {
         
               
                    var data = {
                        "type": 'GREET',
                      

                    };
                    apiService.create("COETrigger/loadtemplate", data).then(function (promise) {
                        if (promise.templatelist.length > 0) {
                            $scope.templatelist = promise.templatelist;
                           

                          
                        }
                        else {
                            swal("No Event Found");
                            $state.reload();
                        }
                    });
               
        
        };   

        $scope.viewtemplate = function () {

            var data = {
                "ISES_Id": $scope.ISES_Id
            }
            apiService.create("COETrigger/viewtemplatedetails", data).
                then(function (promise) {

                    if (promise.sourcelist !== null) {
                        $scope.htmldata = promise.sourcelist[0].iseS_MailHTMLTemplate;
                        var e1 = angular.element(document.getElementById("test"));
                        $compile(e1.html($scope.htmldata))(($scope));
                        $("#myModal").modal('show');
                    }
                });

        }

        $scope.showreport = function () {

            if ($scope.myForm.$valid) {

                $scope.selecteddata = [];
                $scope.selecteddata1 = [];

                //angular.forEach($scope.leadlist, function (rr) {
                //    if (rr.selected == true) {
                //        $scope.selecteddata.push(rr);
                //    }

                //})


                for (var i = 0; i < $scope.leadlist.length; i++) {
                    if ($scope.leadlist[i].selected == true) {
                        $scope.selecteddata.push({ ismslE_Id: $scope.leadlist[i].ismslE_Id })
                    }
                }

                for (var i = 0; i < $scope.vendorlist.length; i++) {
                    if ($scope.vendorlist[i].selected1 == true) {
                        $scope.selecteddata1.push({ invmS_Id: $scope.vendorlist[i].invmS_Id })
                    }
                }

                //angular.forEach($scope.vendorlist, function (rr) {
                //    if (rr.selected1 == true) {
                //        $scope.selecteddata.push(rr);
                //    }

                //})

                if ($scope.selecteddata.length > 0 || $scope.selecteddata1.length>0) {

                    angular.forEach($scope.templatelist, function (rr) {
                        if (rr.iseS_Id == $scope.ISES_Id) {
                            $scope.template1 = rr.iseS_Template_Name;
                            $scope.lead = rr.ismsmsO_SourceName;
                        }

                    });
                    var data = {
                        "type":'GREET',
                        "template": $scope.template1,
                        "msg": $scope.MSG,
                        "snd_email": true,
                   
                      
                        vendordetails: $scope.selecteddata1,
                        leaddetails: $scope.selecteddata,
                        //leaddetails: $scope.materaldocuupload
                    };

                    var swaltext = "";
                  
                    swal({
                        title: "Are you sure?",
                        text: swaltext,
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,send it",
                        cancelButtonText: "Cancel",
                        closeOnConfirm: true,
                        closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                apiService.create("COETrigger/sendsmsemail", data).then(function (promise) {
                                    if (promise.retrunMsg === "true") {
                                        swal('EMAIL/SMS SENT SUCCESSFULLY');
                                        $state.reload();
                                    }
                                    else {
                                        swal('ERROR');
                                    }
                                });
                            }
                            else {
                                swal("Sending Cancelled!!!");
                            }
                        });
                }
                else {
                    swal('Select atleast one record');
                }


            }
            else {
                $scope.submitted = true;
            }


        }

    }
})();