(function () {
    'use strict';
    angular
        .module('app')
        .controller('NaacDocumentApproveProcessController', NaacDocumentApproveProcessController)

    NaacDocumentApproveProcessController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter', '$timeout', 'Excel']
    function NaacDocumentApproveProcessController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter, $timeout, Excel) {


        $scope.delete = function (data) {
            data.nodes = [];
        };
        $scope.add = function (data) {
            var post = data.nodes.length + 1;
            var newName = data.name + '-' + post;
            data.nodes.push({ name: newName, nodes: [] });
        };

        $scope.array = [];
        //$scope.array = [
        //    {
        //        id: 1, name: 'Chapter 1',
        //        temparra1: [{
        //            id: 1, name: '1.1',
        //            temparray2: [{ id: 1, name: '1.1.1' }]
        //        },
        //        { id: 2, name: '1.2', temparray2: [] },
        //        { id: 3, name: '1.3', temparray2: [] }]
        //    },

        //    {
        //        id: 2, name: 'Chapter 2', temparra1: [{ id: 1, name: '2.1', temparray2: [] }, { id: 2, name: '1.2', temparray2: [] },
        //        { id: 3, name: '1.3', temparray2: [] }]
        //    },
        //    {
        //        id: 3, name: 'Chapter 3', temparra1: [{ id: 1, name: '3.1', temparray2: [] }, { id: 2, name: '1.2', temparray2: [] },
        //        { id: 3, name: '1.3', temparray2: [] }]
        //    }
        //];

        $scope.onload = function () {

            var pageid = 1;

            apiService.getURI("NaacDocumentUpload/onload", pageid).then(function (promise) {

                if (promise !== null) {
                    $scope.getparentidzero = promise.getparentidzero;
                    $scope.getalldata = promise.getalldata;
                    $scope.ddfd = false;
                    $scope.array = [];
                    $scope.getsavealldata = promise.getsavealldata;
                    angular.forEach($scope.getparentidzero, function (dd) {
                        $scope.temparra1d = [];
                        angular.forEach($scope.getalldata, function (ddd) {
                            if (ddd.naacsL_ParentId === dd.naacsL_Id) {
                                $scope.temparra1d.push(ddd);
                            }
                        });

                        angular.forEach($scope.temparra1d, function (sdd) {
                            angular.forEach($scope.getsavealldata, function (dds) {
                                if (sdd.naacsL_Id === dds.naacsL_Id) {
                                    sdd.document_Path = dds.naacmsL_Uploadpath;
                                    sdd.NAACMSL_Status = dds.naacmsL_Status;
                                    sdd.coordinatorcomments = dds.naacmsL_ConsultantRemarks;
                                    sdd.usercomments = dds.naacmsL_UserRemarks;
                                    sdd.NAACMSL_Status = dds.naacmsL_Status;
                                }
                            });
                        });


                        $scope.temparray2d = [];
                        angular.forEach($scope.temparra1d, function (levelii) {
                            $scope.temparray2d = [];
                            angular.forEach($scope.getalldata, function (ddd) {
                                if (ddd.naacsL_ParentId === levelii.naacsL_Id) {
                                    $scope.temparray2d.push(ddd);
                                }
                            });

                            angular.forEach($scope.temparray2d, function (sddd) {
                                angular.forEach($scope.getsavealldata, function (ddds) {
                                    if (sddd.naacsL_Id === ddds.naacsL_Id) {
                                        sddd.document_Path = ddds.naacmsL_Uploadpath;
                                        sddd.NAACMSL_Status = ddds.naacmsL_Status;
                                        sddd.coordinatorcomments = ddds.naacmsL_ConsultantRemarks;
                                        sddd.usercomments = ddds.naacmsL_UserRemarks;
                                        sddd.NAACMSL_Status = ddds.naacmsL_Status;
                                    }
                                });
                            });


                            levelii.temparray2 = $scope.temparray2d;
                        });

                        $scope.array.push({ NAACSL_Id: dd.naacsL_Id, NAACSL_SLNo: dd.naacsL_SLNo, NAACSL_SLNoDescription: dd.naacsL_SLNoDescription, temparra1: $scope.temparra1d });

                        console.log($scope.array);


                        var amt = 60;

                        $scope.countTo = amt;
                        $scope.countFrom = 0;

                        $timeout(function () {
                            $scope.progressValue = amt;
                        }, 200);

                        //$scope.getdata();
                    });
                }
            });
        };





        $scope.getdata = function () {

            $scope.ddfd = true;
            console.log($scope.array);

            var tree = document.querySelectorAll('ul.tree a:not(:last-child)');
            for (var i = 0; i < tree.length; i++) {
                tree[i].addEventListener('click', function (e) {
                    var parent = e.target.parentElement;
                    var classList = parent.classList;
                    if (classList.contains("open")) {
                        classList.remove('open');
                        var opensubs = parent.querySelectorAll(':scope .open');
                        for (var i = 0; i < opensubs.length; i++) {
                            opensubs[i].classList.remove('open');
                        }
                    } else {
                        classList.add('open');
                    }
                });
            }
        };


        $scope.saveapproved = function (dd) {

            if (dd.status !== null && dd.status !== undefined && dd.status !== "") {
                var uploadpath = dd.document_Path;
                var id = dd.naacsL_Id;
                var status = dd.status;
                var data = {
                    "NAACSL_Id": id,
                    "document_Path": uploadpath,
                    "status": status,
                    "coordinatorcomments": dd.coordinatorcomments
                };

                apiService.create("NaacDocumentUpload/saveapproved", data).then(function (promise) {

                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Document Submitted Successfully");
                        } else {
                            swal("Failed To Submit Document");
                        }
                    }

                });
            } else {
                swal("Kindly Select Either Approved Or Rejected");
            }   
        };
        var imagedownload = "";
        var docname = "";

        $scope.downloaddirectimage = function (data, idd, typeofphoto) {
            var studentreg = idd;

            $scope.imagedownload = data;
            imagedownload = data;
            docname = typeofphoto;

            var imgs = data;
            var imagarrd = imgs.split('.');
            var lastelementd = imagarrd[imagarrd.length - 1];
            var filetype = lastelementd;

            $http.get(imagedownload, { responseType: "arraybuffer" })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg + '.' + filetype
                    })[0].click();
                });
        };



        $scope.SelectedFileForUploadzd = [];

        $scope.selectFileforUploadzd = function (input, document) {

            $scope.SelectedFileForUploadzd = input.files;

            if (input.files && input.files[0]) {
                if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.naacsL_Id).attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofiled(document);
                }
                else if (input.files[0].type !== "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    swal("Kindly Upload The Excel File Only");
                    return;
                }
            }
        };

        function Uploadprofiled(data) {
            console.log(data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadzd.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
            }
            formData.append("Id", data.naacsL_Id);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadnaacdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.document_Path = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }
    }
})();



