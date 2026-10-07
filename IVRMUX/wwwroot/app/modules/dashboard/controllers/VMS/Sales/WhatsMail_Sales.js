(function () {
    'use strict';
    angular
        .module('app')
        .controller('WhatsMail_SalesController', WhatsMail_SalesController);

    WhatsMail_SalesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce', '$compile'];
    function WhatsMail_SalesController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce, $compile) {
        
        $scope.submitted = false; 
        $scope.obj = {};
        $scope.obj.flag = "HOT";
        $scope.search = "";
        $scope.athflag = true;
        $scope.obj.Email = true;
        $scope.obj.Whatsup = true;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.loaddata = function () {
            $scope.state_list = []; $scope.sourcelist = [];
            $scope.obj.MI_Id = "";
            var pageid = 2;
            var data = {
                "retrunMsg":"save"
            }
            apiService.create("GeneralSendEmail/onload", data).then(function (promise) {
                $scope.allcompany = promise.allcompany;
                $scope.obj.MI_Id = promise.mI_Id;
                $scope.state_list = promise.state_list;
                $scope.sourcelist = promise.sourcelist;
                
            });
        };
        $scope.On_ChangeSouce = function (dev) {
            angular.forEach($scope.sourcelist, function (emp) {            
                if (emp.ISMSMSO_Id == dev.ISMSMSO_Id) {
                    dev.ISMSLERaw_Exibition = "";
                    dev.ISMSLERaw_Exibition = emp.ISMSMSO_SourceName;
                    return;
                } 
            });
        }
        $scope.getAllDetail = function () {
            $scope.lead_list2 = []; $scope.employeeid = [];
            var data = {
                "flag": $scope.obj.flag
            }
            apiService.create("ISM_Client_Project/getdata_CMT", data)
                .then(function (promise) {
                    if (promise.followuplead != null && promise.followuplead.length > 0) {
                       
                        $scope.employeeid = [];
                        angular.forEach(promise.followuplead, function (dev) {
                            if ($scope.employeeid.length === 0) {
                                $scope.employeeid.push(dev);
                            } else if ($scope.employeeid.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.employeeid, function (emp) {
                                    if (emp.ISMSLE_Id === dev.ISMSLE_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.employeeid.push(dev);
                                }
                            }
                        });

                    }
                    else {
                        swal("Leads Are Not Found !");
                    }                    
                });
        }
        $scope.onradiobuttonSelect = function (optionflag) {
            $scope.obj.flag = optionflag;
            $scope.getAllDetail();
        };
        //toggleAll
        $scope.toggleAll = function () {
            var togglestatus = $scope.obj.all;
            angular.forEach($scope.employeeid, function (emp) {
                    emp.selected = togglestatus;
            });
        }
        $scope.binddata = function () {
            angular.forEach($scope.employeeid, function (emp) {
                emp.esubject = ""; emp.MSG = ""; emp.FHEAD = ""; emp.Footer = ""; emp.Whats = "";
                emp.esubject = $scope.obj.esubject;
                emp.MSG = $scope.obj.MSG;
                emp.FHEAD = $scope.obj.FHEAD;
                emp.Footer = $scope.obj.Footer;
                emp.Whats = $scope.obj.Whats;            
            });
        }
        $scope.convertToHtml = function (text) {
            let html = text.replace(/\n/g, '<br>').replace(/^\s*-\s/gm, '<li>').replace(/\n(?=\s*<li>)/g, '</li>').replace(/\s*-\s/g, '</li><li>');
            return `<p>${html}</p>`;
        } 
        //$scope.convertToHtmlwts = function (text) {
        //    let html = text.replace(/\n/g, '<br>').replace(/^\s*-\s/gm, '<li>').replace(/\n(?=\s*<li>)/g, '</li>').replace(/\s*-\s/g, '</li><li>');
        //    return `<p>${html}</p>`;
        //} 
        //convertToHtml
        $scope.submitted = false;
        $scope.sendsmsemail = function () {
            //if ($scope.obj.MI_Id != null && $scope.obj.MI_Id != undefined && $scope.obj.MI_Id != 0 && $scope.obj.MI_Id != "") {

            //}
            //else {
            //    swal('Please select The Company..');
            //}
            if ($scope.myForm1.$valid) {
                $scope.submitted = true;
                var data = {};
                $scope.albumNameArray = [];
                if ($scope.employeeid != null && $scope.employeeid.length > 0 && $scope.obj.rdopunch == 'C') {
                    angular.forEach($scope.employeeid, function (emp) {
                        if (emp.selected == true && $scope.obj.Email == true && $scope.obj.Whatsup == false) {
                            $scope.albumNameArray.push({
                                EmailId: emp.ISMSLE_EmailId,
                                ISMSLE_ContactNo: emp.ISMSLE_ContactNo,
                                msg: document.getElementById("Test" + emp.ISMSLE_Id + "").innerHTML,
                                esubject: emp.esubject,
                                Footer: emp.Footer,
                                FHEAD: emp.FHEAD,

                            })
                        }
                        if (emp.selected == true && $scope.obj.Email == true && $scope.obj.Whatsup == true) {
                            $scope.albumNameArray.push({
                                EmailId: emp.ISMSLE_EmailId,
                                ISMSLE_ContactNo: emp.ISMSLE_ContactNo,
                                msg: document.getElementById("Test" + emp.ISMSLE_Id + "").innerHTML,
                                esubject: emp.esubject,
                                Footer: emp.Footer,
                                FHEAD: emp.FHEAD,

                            })
                        }
                        if (emp.selected == true && $scope.obj.Email == false && $scope.obj.Whatsup == true) {
                            $scope.albumNameArray.push({
                                EmailId: emp.ISMSLE_EmailId,
                                ISMSLE_ContactNo: emp.ISMSLE_ContactNo,
                            })
                        }
                    });

                }
                else if ($scope.materaldocuupload != null && $scope.materaldocuupload.length > 0 && $scope.obj.rdopunch == 'R') {
                    angular.forEach($scope.materaldocuupload, function (emp) {
                        $scope.albumNameArray.push({
                            EmailId: emp.ISMSLE_EmailId,
                            ISMSLE_ContactNo: emp.ISMSLE_ContactNo,
                            LeadAddress: emp.ISMSLE_LeadAddress1,
                            State: emp.IVRMMS_Id,
                            ISMSLE_ContactDesignation: emp.ISMSLE_ContactDesignation,
                            ISMSLE_LeadName: emp.ISMSLE_LeadName,
                            ISMSLERaw_Exibition: emp.ISMSLERaw_Exibition,
                            ISMSLERaw_ExibitionDate: new Date(emp.ISMSLERaw_ExibitionDate).toDateString(),
                            Contact_Person: emp.Contact_Person,
                            Lead_heldOn: emp.Lead_heldOn,
                            ISMSMSO_Id: emp.ISMSMSO_Id
                        })
                    });
                    $scope.obj.Email = true;
                }
                if ($scope.obj.rdopunch == 'R') {
                    if ($scope.albumNameArray != null && $scope.albumNameArray.length > 0) {
                        let isValid = true;  // Flag to check if any value is invalid

                        angular.forEach($scope.materaldocuupload, function (emp) {
                            // Check for empty or null values and invalid date
                            if (!emp.ISMSLE_EmailId || !emp.ISMSLE_ContactNo || !emp.ISMSLE_LeadAddress1 ||
                                !emp.IVRMMS_Id || !emp.ISMSLE_ContactDesignation ||
                                !emp.ISMSLE_LeadName || !emp.ISMSLERaw_Exibition || !emp.Contact_Person ||
                                !emp.Lead_heldOn || !emp.ISMSMSO_Id || isNaN(new Date(emp.ISMSLERaw_ExibitionDate).getTime())) {
                                isValid = false;
                            }
                        });


                        if (!isValid) {
                            swal('Please Enter The Valid data');
                        }
                        else {
                            if ($scope.albumNameArray != null && $scope.albumNameArray.length > 0) {
                                data = {
                                    filelist: $scope.materaldocuupload,
                                    "Emailtemp": $scope.albumNameArray,
                                    "EmailFlag": $scope.obj.Email,
                                    "Whats_Flag": $scope.obj.Whatsup,
                                    "rdopunch": $scope.obj.rdopunch,
                                    "MI_Id": $scope.obj.MI_Id,
                                    "TemplateName": "roadshow"
                                };
                                swal({
                                    title: "Are you sure?",
                                    text: "Do you Want to send Email/ WhatsUp!",
                                    type: "warning",
                                    showCancelButton: true,
                                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,send it",
                                    cancelButtonText: "Cancel",
                                    closeOnConfirm: true,
                                    closeOnCancel: true
                                },
                                    function (isConfirm) {
                                        if (isConfirm) {
                                            apiService.create("GeneralSendEmail/sendsmsemailsales", data).then(function (promise) {
                                                if (promise.retrunMsg === "true") {
                                                    swal('EMAIL/WhatsUp SENT SUCCESSFULLY');
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
                                swal("Select Atleast one Record !")
                            }
                        }
                    }

                }

                //if ($scope.albumNameArray != null && $scope.albumNameArray.length > 0) {
                //     data = {
                //         filelist: $scope.materaldocuupload,
                //        "Emailtemp": $scope.albumNameArray,
                //        "EmailFlag": $scope.obj.Email,
                //        "Whats_Flag": $scope.obj.Whatsup,
                //        "rdopunch": $scope.obj.rdopunch,
                //        "MI_Id": $scope.obj.MI_Id,
                //         "TemplateName": "roadshow"
                //    };
                //    swal({
                //        title: "Are you sure?",
                //        text: "Do you Want to send Email/ WhatsUp!",
                //        type: "warning",
                //        showCancelButton: true,
                //        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,send it",
                //        cancelButtonText: "Cancel",
                //        closeOnConfirm: true,
                //        closeOnCancel: true
                //    },
                //        function (isConfirm) {
                //            if (isConfirm) {
                //                apiService.create("GeneralSendEmail/sendsmsemailsales", data).then(function (promise) {
                //                    if (promise.retrunMsg === "true") {
                //                        swal('EMAIL/WhatsUp SENT SUCCESSFULLY');
                //                        $state.reload();
                //                    }
                //                    else {
                //                        swal('ERROR');
                //                    }
                //                });
                //            }
                //            else {
                //                swal("Sending Cancelled!!!");
                //            }
                //        });
                //}
                //else {
                //    swal("Select Atleast one Record !")
                //}

            } else {
                $scope.submitted = true;
            }
        }
        $scope.interacted = function (field) {

            return $scope.submitted;
        };
       
        $scope.tempemail = [];
        //TO clear  data
        $scope.clearid = function () {
            $state.reload();
        };
        //clear end
        $scope.materaldocuupload = [{ id: 'Materal1' }];
        $scope.addmateral = function () {
            var newItemNo = $scope.materaldocuupload.length + 1;

            if (newItemNo <= 10) {
                $scope.materaldocuupload.push({
                    'id': 'Materal' + newItemNo,
                    ISMSLERaw_ExibitionDate: new Date()
                });
            }
        };
        $scope.removemateral = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

            if ($scope.materaldocuupload.length === 0) {
                //data
            }
        };
        $scope.uploadmateraldocuments1 = [];
        $scope.uploadmateraldocuments = function (input, document) {

            $scope.uploadmateraldocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if ((input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg" || input.files[0].type === "image/JPEG" || input.files[0].type === "image/PNG" || input.files[0].type === "image/JPG") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document,") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload MP4, Pdf, Image Files Only");
                }
            }
        };
        function UploaddianmateralPhoto(data) {
            console.log("Student Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadmateraldocuments1.length; i++) {
                formData.append("File", $scope.uploadmateraldocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadSales", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    data.cfilepath = d;
                    data.cfilename = $scope.filename;
                    //$('#').attr('src', data.cfilepath);
                    var img = data.cfilepath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.cfilepath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }
        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };
        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };
        $scope.onview = function (filepath, filename) {
            //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    $scope.content = $sce.trustAsResourceUrl(fileURL);
                    $('#showpdf').modal('show');
                });
        };
        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.cfilepath;
            $scope.videdfd = data.cfilepath;
            $scope.movie = { src: data.cfilepath };
            $scope.movie1 = { src: data.cfilepath };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.cfilepath });
            console.log($scope.view_videos);
        };
        $scope.showpdf = false;
        $scope.downloadview = function (pdfview) {
            $scope.pdfurl = pdfview;
            $scope.showpdf = true;
            $('#showpdf').modal('show');
        };
        $scope.backtoview = function () {
            $scope.showpdf = false;
        };

        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input) {
            $scope.SelectedFileForUploadzd = input.files;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    //once return success 
                    var reader = new FileReader();
                    reader.readAsDataURL(input.files[0]);
                    $scope.filename = input.files[0].name;
                    // alert($scope.filename);
                }
                else {
                    //swal("Please Upload the .xlsx file");
                    //return;
                    $scope.filename = input.files[0].name;
                }
            }
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
angular
    .module('app').directive("fileread", ['$rootScope', 'apiService', function ($rootScope, apiService) {

        return {
            scope: {
                opts: '='
            },
            link: function ($scope, $elm, $attrs) {
                $elm.on('change', function (changeEvent) {
                    var emailids = "";
                    var reader = new FileReader();
                    reader.onload = function (evt) {
                        $scope.$apply(function () {
                            var data = evt.target.result;
                            var workbook = XLSX.read(data, { type: 'binary' });
                            var headerNames = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]], { header: 1 })[0];
                            data = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]]);
                            if (data.length === 0) {
                                swal("Excel Sheet is Empty");
                                $elm.val(null);
                                $scope.opts.data = null;
                                return;
                            }
                            else {
                                $scope.headers = ["EmailId",];
                                $scope.opts = {};
                                $scope.opts.columnDefs = [];
                                headerNames.forEach(function (h) {
                                    $scope.opts.columnDefs.push({ field: h });
                                });
                                $scope.checkheaders = [];
                                headerNames.forEach(function (h) {
                                    $scope.checkheaders.push(h);
                                });
                                var excellvalidatecount = 0;
                                $scope.Missinghead = [];
                                angular.forEach($scope.headers, function (head1) {
                                    var missingHead = 0;
                                    angular.forEach($scope.checkheaders, function (head2) {
                                        if (head1 === head2) {
                                            excellvalidatecount = excellvalidatecount + 1;
                                            missingHead = 1;
                                        }
                                    });
                                    if (missingHead === 0) {
                                        $scope.Missinghead.push(head1);
                                    }
                                });

                                if (excellvalidatecount === 1) {
                                    var bind = true;
                                    var excellcellvalidate = 0;
                                    $scope.fushlast = [];
                                    $scope.fushlast.push(0);
                                    angular.forEach(data, function (valu, ke) {
                                        $scope.rowheaders = [];
                                        angular.forEach(valu, function (val, ey) {
                                            $scope.rowheaders.push(ey);
                                        });
                                        var checkrow = 0;
                                        angular.forEach($scope.checkheaders, function (head1) {
                                            angular.forEach($scope.rowheaders, function (head2) {
                                                if (head1 === head2) {
                                                    checkrow = checkrow + 1;
                                                }
                                            });
                                        });
                                        if (checkrow !== $scope.checkheaders.length) {
                                            $scope.fushlast.push(valu.__rowNum__ + 1);
                                        }
                                    });
                                    if ($scope.fushlast.length === 1) {

                                        $scope.opts.data = data;
                                        console.log($scope.opts.data);
                                        for (var i = 0; i < $scope.opts.data.length; i++) {
                                            emailids = $scope.opts.data[i].EmailId;

                                        }
                                        $scope.tempemail = $scope.opts.data;
                                        console.log($scope.tempemail);
                                        //$scope.tempemailppp($scope.tempemail);

                                        $scope.opts.data = $scope.opts.data;
                                        // $scope.obj.emails = emailids;
                                        //for (var i = 0; i < $scope.opts.data.length - 1; i++) {
                                        //    emailids += $scope.opts.data[i].EmailId + ",";
                                        //}
                                        //for (var j = $scope.opts.data.length - 1; i < $scope.opts.data.length; i++) {
                                        //    emailids += $scope.opts.data[i].EmailId;
                                        //}
                                        //$scope.opts.data = emailids;
                                    }
                                    else {
                                        var Missingrows = "";
                                        if ($scope.fushlast.length > 1) {
                                            angular.forEach($scope.fushlast, function (head2) {
                                                if (head2 != 0) {
                                                    if (Missingrows === "") {
                                                        Missingrows = head2;
                                                    }
                                                    else {
                                                        Missingrows = Missingrows + "," + head2;
                                                    }
                                                }
                                            });
                                            Missingrows = "The Row " + Missingrows + " Contains Empty Cell Values, replace with the NULL for empty cell";
                                        }
                                        swal(Missingrows, "Excel Data is Not Proper!!");
                                    }
                                }
                                else {
                                    var Missingheads = "";
                                    if ($scope.Missinghead.length > 0) {
                                        angular.forEach($scope.Missinghead, function (head2) {
                                            if (head2 != 0) {
                                                if (Missingheads === "") {
                                                    Missingheads = head2;
                                                }
                                                else {
                                                    Missingheads = Missingheads + ", \n" + head2;
                                                }
                                            }
                                        });
                                        Missingheads = "The Missing Headers: \n" + Missingheads;
                                        swal(Missingheads, "Header Missing..!!");
                                    }
                                    else {
                                        swal("Header Missing..!!", "Please Import a Excel With All/Proper Headers.");
                                    }

                                }

                            }
                        });
                    };
                    reader.readAsBinaryString(changeEvent.target.files[0]);
                });
            }
        };
    }]);


