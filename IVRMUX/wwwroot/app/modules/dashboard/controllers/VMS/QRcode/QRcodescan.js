(function () {
    'use strict';
    angular
        .module('app')
        .controller('QRcodescanController', QRcodescanController)
    QRcodescanController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$stateParams', 'Excel', '$timeout', '$q']
    function QRcodescanController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $stateParams, Excel, $timeout, $q) {


        $scope.searchValueI = "";

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;

        }
        else {
            paginationformasters = 10;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //-------------------------------------------------------------------

        $scope.onLoadGetData = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPageI = paginationformasters;
            $scope.search = "";
            $scope.presentCountgridI = "";
            $scope.itemsPerPage = 10;

            var pageid = 2;
            apiService.getURI("ISMDashboard/getdetails", pageid).then(function (promise) {
                
                $scope.role_flag = promise.role_flag;

                $scope.qRcodeActivityDetailsTemp = [];
                $scope.QRcodeActivityDetails = [];
                $scope.qRcodeActivityDetailsTemp = promise.qRcodeActivityDetails;
                if ($scope.qRcodeActivityDetailsTemp != null && $scope.qRcodeActivityDetailsTemp.length > 0) {
                    for (var t = 0; t < $scope.qRcodeActivityDetailsTemp.length; t++) {
                        $scope.QRcodeActivityDetails.push({
                            HRMARACTT_Time: moment($scope.qRcodeActivityDetailsTemp[t].HRMARACTT_Time, 'HH:mm a').format(),
                            HRMB_BuildingName: $scope.qRcodeActivityDetailsTemp[t].HRMB_BuildingName,
                            HRMF_FloorName: $scope.qRcodeActivityDetailsTemp[t].HRMF_FloorName,
                            HRMAR_AreaName: $scope.qRcodeActivityDetailsTemp[t].HRMAR_AreaName,
                            HRMARACT_ActivityName: $scope.qRcodeActivityDetailsTemp[t].HRMARACT_ActivityName,
                            HRMARACTT_ActivityDuration: $scope.qRcodeActivityDetailsTemp[t].HRMARACTT_ActivityDuration,
                            HRMARACTSTMAP_Date: new Date($scope.qRcodeActivityDetailsTemp[t].HRMARACTSTMAP_Date),
                            HRMAR_Id:$scope.qRcodeActivityDetailsTemp[t].HRMAR_Id,
                            HRMARACT_Id: $scope.qRcodeActivityDetailsTemp[t].HRMARACT_Id,
                            HRMARACTT_Id: $scope.qRcodeActivityDetailsTemp[t].HRMARACTT_Id,
                        });
                    }
                }

            });

        };


        //================Remove image

        $scope.remove_file = function () {
            $scope.file_detail = "";
            $scope.notice = "";
        };
        $scope.uploadFile = function (input, document) {

            $scope.UploadFile = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" || input.files[0].type == "image/png" || input.files[0].type == "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.ms-excel" &&
                    input.files[0].size <= 2097152) {

                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $('#blahD')
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        };
        function Uploadprofile() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadFile.length; i++) {

                formData.append("File", $scope.UploadFile[i]);
                $scope.file_detail = $scope.UploadFile[0].name;
                $scope.photo = $scope.UploadFile[0].name;
            }


            //We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_Noticefiles", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    $scope.notice = d;
                    if ($scope.notice != null && $scope.notice.length > 0) {
                        var fileurl = $scope.notice[0];
                        $scope.file_detail = $scope.notice[0];
                        var nameArray = fileurl.split('.');
                        $scope.extention = nameArray[nameArray.length - 1];
                        if ($scope.extention === 'doc' || $scope.extention === 'docx' || $scope.extention === 'ppt' || $scope.extention === 'pptx' || $scope.extention === 'xlsx' || $scope.extention === 'xls') {
                            $scope.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + fileurl;
                        }
                    }

                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });


        }


        $scope.showmodaldetails = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];

            if (extention == "jpg" || extention == "jpeg") {
                $('#preview').attr('src', data);
            }
            else if (extention == "doc" || extention == "docx") {
                $('#preview').removeAttr('src');
            }
            else if (extention == "pdf") {
                var imagedownload = data;
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

        $scope.optionToggled1 = function () {
            $('#viewaddedchecklist').modal('show');
        }

        $scope.QRclick = function () {

            $scope.submitted = true;
            $scope.tasklistarray = [];
            $scope.HRMAR_Id = 0;
            angular.forEach($scope.QRcodeActivityDetails, function (qq) {
                if (qq.selected == true) {
                    $scope.tasklistarray.push(qq)
                    $scope.HRMAR_Id = qq.HRMAR_Id;
                }
            });


            if ($scope.myForm.$valid) {
                var data = {
                    "HRMAR_Id": $scope.HRMAR_Id,
                    "HRMA_QRCODEPath": $scope.file_detail,
                    "tasklistarray": $scope.tasklistarray
                }
                apiService.create("StaffAreaActivityRecord/Activitysavedetails", data).
                    then(function (promise) {
                        if (promise.returnval == "Add") {
                            $state.reload();
                           
                            swal("saved Successfully....");
                            $('#viewaddedchecklist').modal('hide');                      
                            
                        }
                        else {
                            swal("Not saved Successfully...., Try Again.");
                            $state.reload();
                        }
                        $state.reload();
                    })
               
            }
            else {

                $scope.submitted = true;
            }

           
          
        }
        $scope.cancel = function (){
            $state.reload();
        }

        
        
    }
})();