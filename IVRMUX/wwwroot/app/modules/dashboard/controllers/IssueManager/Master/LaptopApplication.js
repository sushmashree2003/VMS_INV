(function () {
    'use strict';
    angular
        .module('app')
        .controller('LaptopApplicationController', LaptopApplicationController);

    LaptopApplicationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', 'uiGridGroupingConstants', '$timeout', '$q', 'appSettings', '$http'];

    function LaptopApplicationController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, uiGridGroupingConstants, $timeout, $q, appSettings, $http) {

        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.search = '';
        $scope.submitted = false;
        $scope.filtervalue1 = function (user) {
        };

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("LaptopApplication/loaddata", pageid).then(function (promise) {
                $scope.alldata = promise.alldata;
            });
        };

        $scope.ISMDIT_Date = new Date();// today's date

        //============== save
        $scope.save = function () {
            if ($scope.myForm.$valid) {
                var datefrom = $scope.vlapappL_FromDate === null ? "" : $filter('date')($scope.vlapappL_FromDate, "yyyy-MM-dd");
                var dateto = $scope.vlapappL_ToDate === null ? "" : $filter('date')($scope.vlapappL_ToDate, "yyyy-MM-dd");

                $scope.vlapappL_FromDate = new Date($scope.vlapappL_FromDate).toDateString();
                $scope.vlapappL_ToDate = new Date($scope.vlapappL_ToDate).toDateString();

                var data = {
                    "VLAPAPPL_Id": $scope.vlapappL_Id,
                    "VLAPAPPL_FromDate": $scope.vlapappL_FromDate,
                    "VLAPAPPL_ToDate": $scope.vlapappL_ToDate,
                    "VLAPAPPL_TotalDays": $scope.vlapappL_TotalDays,
                    "VLAPAPPL_Reason": $scope.vlapappL_Reason,
                    "VLAPAPPL_SupportingDocument": $scope.vlapappL_SupportingDocument
                };

                apiService.create("LaptopApplication/save", data).then(function (promise) {
                    if (promise.msg === 'Saved') {
                        swal("Request Saved Successfully.....!!!!!");
                        $state.reload();
                    }
                    else if (promise.msg === 'updated') {
                        swal("Request Updated Successfully.....!!!!!");
                        $state.reload();
                    }
                    else if (promise.msg === 'Failed') {
                        swal("Request  Not Saved .....!!!!!");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }  
        };

        $scope.edittab1 = function (user) {
            var data = {
                "VLAPAPPL_Id": user.vlapappL_Id                
            };

            apiService.create("LaptopApplication/edittab1", data).then(function (promise) {
                $scope.vlapappL_Id = promise.editlist[0].vlapappL_Id;
                $scope.vlapappL_FromDate = new Date(promise.editlist[0].ismdiT_Date);
                $scope.vlapappL_ToDate = promise.editlist[0].trmV_Id;
                $scope.vlapappL_TotalDays = promise.editlist[0].ismdiT_BillNo;
                $scope.vlapappL_Reason = promise.editlist[0].ismdiT_Qty;
                $scope.vlapappL_SupportingDocument = promise.editlist[0].ismdiT_Amount;

                if (promise.editlist[0].vlapappL_FromDate !== null) {
                    $scope.vlapappL_FromDate = new Date(promise.editlist[0].vlapappL_FromDate);
                }
                else {
                    $scope.vlapappL_FromDate = null;
                }

                if (promise.editlist[0].vlapappL_ToDate !== null) {
                    $scope.vlapappL_ToDate = new Date(promise.editlist[0].vlapappL_ToDate);
                }
                else {
                    $scope.vlapappL_ToDate = null;
                }
            });
        };

        $scope.Clearid = function () {
            $scope.vlapappL_Id = "";
            $scope.vlapappL_FromDate = "";
            $scope.vlapappL_ToDate = "";
            $scope.vlapappL_TotalDays = "";
            $scope.vlapappL_Reason = "";
            $scope.vlapappL_SupportingDocument = "";
        };

        $scope.printData = function (Employee) {
            var innerContents = document.getElementById("indenttemplate").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/EmpPaySlipPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.SelectedFileForUploadzdBOSBOE = [];
        $scope.selectFileforUploadzdBOSBOE = function (input) {
            $scope.SelectedFileForUploadzdBOSBOE = input.files;
            //$('#blah').removeAttr('src');
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                $scope.extention = nameArray[nameArray.length - 1];
                if (($scope.extention === "JPEG" || $scope.extention === "jpeg" || $scope.extention === "jpg" || $scope.extention === "JPG" || $scope.extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blah')
                            .attr('src', e.target.result);

                        $('#documentid') //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentBOSBOE();
                }
                else if ($scope.extention !== "JPEG" && $scope.extention !== "jpeg" && $scope.extention !== "jpg" && $scope.extention !== "JPG" && $scope.extention !== "pdf") {
                    $('#documentid').removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    $('#documentid').removeAttr('src');
                    swal("Document size should be less than 2MB");
                    return;
                }
            }
        };

        function UploadEmployeeDocumentBOSBOE() {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdBOSBOE.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdBOSBOE[i]);
            }
            // We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadnaacdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {
                        $scope.vlapappL_SupportingDocument = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#documentid').removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.showmodaldetailsBOSBOE = function () {
            //$('#preview').removeAttr('src');
            var filename = $scope.vlapappL_SupportingDocument.toString();
            var nameArray = filename.split('.');
            $scope.extention = nameArray[nameArray.length - 1];
            if ($scope.extention === "jpg" || $scope.extention === "jpeg" || $scope.extention === "JPG" || $scope.extention === "JPEG") {
                $('#preview').attr('src', $scope.vlapappL_SupportingDocument);
            }
            else if ($scope.extention === "doc" || $scope.extention === "docx" || $scope.extention === "xls" || $scope.extention === "xlsx") {
                //$('#preview').removeAttr('src');
                $('#preview').attr('src', $scope.vlapappL_SupportingDocument);
            }
            else if ($scope.extention === "pdf") {
                //var imagedownload = $scope.vlapappL_SupportingDocument;
                //$scope.content = "";
                //var fileURL = "";
                //var file = "";
                //$http.get(imagedownload, { responseType: 'arraybuffer' })
                //    .success(function (response) {
                //        file = new Blob([(response)], { type: 'application/pdf' });
                //        fileURL = URL.createObjectURL(file);
                //        $scope.content = $sce.trustAsResourceUrl(fileURL);
                //        $('#showpdf').modal('show');
                //    });

                var imagedownload = $scope.vlapappL_SupportingDocument;
                $scope.content = "";
                var fileURL = "";
                var file = "";
                $http.get(imagedownload, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        var pdfId = document.getElementById("pdfIdzz");
                        pdfId.removeChild(pdfId.childNodes[0]);
                        var embed = document.createElement('embed');
                        embed.setAttribute('src', fileURL);
                        embed.setAttribute('type', 'application/pdf');
                        embed.setAttribute('width', '100%');
                        embed.setAttribute('height', '1000');
                        pdfId.appendChild(embed);
                        $('#showpdf').modal('show');

                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        $scope.content = $sce.trustAsResourceUrl(fileURL);
                        $('#showpdf').modal('show');
                    });


            }
        };

        $scope.countdays = function () {
            var timeDiff = Math.abs($scope.vlapappL_FromDate.getTime() - $scope.vlapappL_ToDate.getTime());
            $scope.vlapappL_TotalDays = Math.ceil(timeDiff / (1000 * 3600 * 24)) + 1;
        };

        $scope.datechange = function () {
            $scope.vlapappL_ToDate = "";
            $scope.minDatef = new Date($scope.vlapappL_FromDate.getFullYear(), $scope.vlapappL_FromDate.getMonth(), $scope.vlapappL_FromDate.getDate());
        };

        $scope.zoomin = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth >= 750) {
                swal("Maximum zoom-in level reached.");
            } else {
                myImg.style.width = (currWidth + 50) + "px";
            }
        };

        $scope.zoomout = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth <= 400) {
                swal("Maximum zoom-out level reached.");
            } else {
                myImg.style.width = (currWidth - 50) + "px";
            }
        };
    }   
})();


