(function () {
    'use strict';
    angular
        .module('app')
        .controller('GeneralSendEmailController', GeneralSendEmailController);

    GeneralSendEmailController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$sce', '$compile'];
    function GeneralSendEmailController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $sce, $compile) {
        var vm = this;
        vm.gridOptions = {};
        $scope.submitted = false;
        $scope.sendsmsemail = function (gridOptions) {

            var data = {};
            var valu = gridOptions;
            $scope.albumNameArray = [];
            if (valu.length > 0) {
                $scope.albumNameArray = [];
                for (var i = 0; i < valu.length; i++) {
                    $scope.albumNameArray.push(valu[i]);
                }
            }

                var data = {
                    "type": "NONTPL",
                    "template": $scope.template1,
                    "msg": $scope.MSG,
                    "smsmsg": $scope.SMSMSG,
                    "snd_email": $scope.snd_email,
                    "snd_sms": $scope.snd_sms,
                    "esubject": $scope.esubject,
                    "Footer": $scope.Footer,
                    "athflag": $scope.athflag,
                    "FHEAD": $scope.FHEAD,
                    selected: $scope.selecteddata,
                    filelist: $scope.materaldocuupload,
                    "Emailtemp": $scope.albumNameArray
                  
                };


                swal({
                    title: "Are you sure?",
                    text: "Do you Want to send Email!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,send it",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: true,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("GeneralSendEmail/sendsmsemail", data).then(function (promise) {
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
        $scope.interacted = function (field) {

            return $scope.submitted;
        };
        $scope.obj = {};
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
                $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
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

                if ((input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg" || input.files[0].type === "image/JPEG" || input.files[0].type === "image/PNG" || input.files[0].type === "image/JPG" ) && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
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
            $http.post("/api/ImageUpload/UploadspaceBooking", formData,
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
        $scope.selectFileforUploadzd= function (input) {
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
        $scope.tempemailppp = function (ppprt) {

        }

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
                   var emailids ="";
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


