(function () {
    'use strict';
    angular.module('app').controller('ViewOfDeploymentController', ViewOfDeploymentController);
    ViewOfDeploymentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache', 'myFactoryvisitor', '$q', '$sce'];
    function ViewOfDeploymentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache, myFactoryvisitor, $q, $sce) {



        var date = new Date();
        $scope.date_select = date;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag === "ISMTASK") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }


        $scope.getserverdate = function () {
            var xmlHttp;
            function srvTime() {
                try {
                    //FF, Opera, Safari, Chrome
                    xmlHttp = new XMLHttpRequest();
                }
                catch (err1) {
                    //IE
                    try {
                        xmlHttp = new ActiveXObject('Msxml2.XMLHTTP');
                    }
                    catch (err2) {
                        try {
                            xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
                        }
                        catch (eerr3) {
                            //AJAX not supported, use CPU time.
                            alert("AJAX not supported");
                        }
                    }
                }
                xmlHttp.open('HEAD', window.location.href.toString(), false);
                xmlHttp.setRequestHeader("Content-Type", "text/html");
                xmlHttp.send('');
                return xmlHttp.getResponseHeader("Date");
            }
            $scope.today = srvTime();
        }




        $scope.submitted = false;
        

        $scope.obj = {};


        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.itemsPerPage1 = 5;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("ViewOfDeployment/getloaddata", pageid).then(function (promise) {
                if (promise != null && promise.get_client.length > 0) {
                    $scope.get_client = promise.get_client;
                    $scope.version_list = [];
                    // console.log($scope.get_client)
                }
                else {
                    swal("No Hosting List Found", "Kindly Map The Hosting For Client Wise", "warning")
                }

            });

        };
        //Hosting change
        $scope.OnChangehost = function () {
            var data = {
                "IVRMCH_Id": $scope.obj.ivrmcH_Id.ivrmcH_Id
            }
            apiService.create("ViewOfDeployment/GetOnchangeData", data).then(function (promise) {
                if (promise != null && promise.version_list.length > 0) {
                    $scope.version_list = promise.version_list;
                }
                else {
                    swal("No Versions Found For This Hosting", "No Deployments Was Done", "warning");
                    $scope.version_list = '';
                }
            });
        }
        //
        $scope.flag = "";
        $scope.initializeFlag = function (flag) {
            $scope.tasks_list = [];
            $scope.flag = flag;
            //  $scope.get_client = [];
            // $scope.version_list = [];
        }
        //task fetch
        $scope.gettasks_forupdate = function () {
            if ($scope.myForm2.$valid) {

                var data = {
                    "IVRMCH_Id": $scope.obj.ivrmcH_Id.ivrmcH_Id,
                    "IVRMVT_Id": $scope.obj.ivrmvT_Id.ivrmvT_Id,
                    "flag": $scope.flag
                };
                apiService.create("ViewOfDeployment/getReport", data).then(function (promise) {
                    if (promise != null && promise.tasks_list.length > 0) {
                        $scope.tasks_list = promise.tasks_list;
                        $scope.presentCountgrid = $scope.tasks_list.length;
                        angular.forEach($scope.tasks_list, function (qq) {
                            if (qq.Assigned_HRME_Id == promise.hrmE_Id) {
                                qq.edit_feature = true;
                            }
                            if (qq.Created_HRME_Id == promise.hrmE_Id) {
                                qq.edit_remark = true;
                            }
                            if (qq.IVRMVFD_Status == "Close") {
                                qq.class = "closedrow";
                            }
                        });
                        // console.log($scope.tasks_list);
                    }
                    else {
                        swal("No Tasks List Found For You", "No Tasks Was Assigned To You In The Selected Version", "info");
                    }
                });

            }
            else {
                $scope.submitted = true;
            }
        };
      
        $scope.checkalltasks = function (toggle) {
            angular.forEach($scope.tasks_list, function (qq) {
                qq.check = toggle;
            });
        }

        //========================================= Create

        $scope.obj = {};
        $scope.submitted1 = false;
        $scope.save_features = function () {
            $scope.featurestasklist = [];
            angular.forEach($scope.tasks_list, function (tsk) {
                if (tsk.check) {
                    $scope.featurestasklist.push(tsk);
                }
            });
            if ($scope.featurestasklist.length > 0) {
                if ($scope.myForm.$valid) {
                    var data = {
                        "flag": $scope.flag,
                        "featurestasklist": $scope.featurestasklist
                    };
                    apiService.create("ViewOfDeployment/savedata", data).then(function (promise) {
                        if (promise.returnval) {
                            swal("Saved Successfully", "", "success");
                            $scope.gettasks_forupdate();
                        }
                        else {
                            swal("Failed To Save"," ", "failed");
                        }

                    });
                }
                else {
                    $scope.submitted1 = true;
                }
             
            }
            else {
                swal("Kindly Select Atleast One Task", "", "info")
            }
        };
        $scope.interacted1 = function (field) {
            return field && ($scope.submitted1 || field.$touched);
        };

        //=====================Editrecord....

        //=================Active/Deactivate

        //===========Clear Field
        $scope.cancel = function () {
            $state.reload();
        };

        $scope.uploadtecherdocuments1 = [];

        $scope.selectFileForUpload = function (input, document) {

            $scope.uploadtecherdocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {
                if (input.files[0].type === "application/pdf") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianPhoto(document);
                }

                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianPhoto(document);
                }
                else {
                    swal("Upload Only .pdf/.xlsx Files only ", " ", "warning");
                }
            }
        };



        function UploaddianPhoto(data) {
            console.log("Teacher Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadtecherdocuments1.length; i++) {
                formData.append("File", $scope.uploadtecherdocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/upload_test_cases", formData,
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': undefined
                    },
                    transformRequest: angular.identity,

                })
                .success(function (d) {
                    defer.resolve(d);
                    data.IVRMVFD_Test_Case = d;
                    data.IVRMVFD_Test_Case_File_Name = $scope.filename;
                    console.log(data);
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.view_test_case_file = function (filepath, filename) {
            $scope.Documentname = filename;
            const extension = filename.split('.').pop().toLowerCase();
            if (extension == 'pdf') {
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

            }
            else if (extension == 'xlsx' || extension == 'xls') {
                //view excel files                var docpath = '';
                docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + encodeURIComponent(filepath);
                $scope.detailFrame = $sce.trustAsResourceUrl(docpath);
                $('#myModaldocx').modal('show');
            }
            else {
                swal("Unsupported File Format To View", "", "warning");
            }
        };



        //$scope.OnChangeClient = function (ismmclT_Id) {


        //    $scope.clientlistarray = [];
        //    angular.forEach($scope.get_client, function (qq) {
        //        if (qq.selected == true) {
        //            $scope.clientlistarray.push({
        //                ISMMCLT_Id: qq.ismmclT_Id
        //            })
        //        }
        //    });
        //    var data = {
        //        //"MI_Id": mi_id
        //        "clientlistarray": $scope.clientlistarray
        //    }

        //    apiService.create("ViewOfDeployment/GetOnchangeClient", data).then(function (promise) {

        //        $scope.get_tasks = promise.get_tasks;
        //        // ismtcR_TaskNo = "TASK/10108/2019-2020"
        //        //ismtcR_Id = 12680

        //    });
        //}



    }



    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });
})();

