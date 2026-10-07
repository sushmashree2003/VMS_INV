(function () {
    'use strict';
    angular.module('app').controller('IEMonthEndReportApprovalController', IEMonthEndReportApprovalController)

    IEMonthEndReportApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function IEMonthEndReportApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {

        $scope.submitted = false;

        $scope.ddate = {};
        $scope.currentdate = new Date();
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        };

        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("IEMonthEndReportApproval/onloaddata", pageid).then(function (promise) {
                $scope.Year = promise.year;
                $scope.obj.ASMAY_Id = academicyrlst[0].asmaY_Id;
                $scope.Month = promise.month;
                $scope.intistutename = promise.intistutename;
                $scope.getApprovalDetails = promise.getApprovalDetails;               
                $scope.presentCountgrid = $scope.getApprovalDetails.length;
                $scope.getGriddata = promise.getGriddata;
                $scope.presentCountgrid1 = $scope.getGriddata.length;

                var doob = $scope.currentdate == null ? "" : $filter('date')($scope.currentdate, "yyyy-MM-dd");
                var doobyr = doob.substring(0, 4);
                var doobmnth = doob.substring(5, 7);
                var doobdays = doob.substring(8, 10);
                $scope.month_id = doobmnth;
                $scope.obj.IVRM_Month_Id = doobmnth;

            });
        };



        $scope.ApprovalView = function (indl) {
            var data = {
                "ISMCLMER_Id": indl.ISMCLMER_Id,
            };
            apiService.create("IEMonthEndReportApproval/DocView", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.multidocView != null) {
                        $('#MultiDocumentView').modal('show');
                        $scope.multidocView = promise.multidocView;
                    }
                }
            });

        };
        $scope.ApprovalViewapproved = function (indl) {
            var data = {
                "ISMCLMER_Id": indl.ISMCLMER_Id,
            };
            apiService.create("IEMonthEndReportApproval/DocView", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.multidocView != null) {
                        $('#MultiDocumentViewapproved').modal('show');
                        $scope.multidocView = promise.multidocView;
                    }
                }
            });

        };

        $scope.rowsSelected = [];

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.multidocView, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.rowsSelected.indexOf(itm) === -1) {
                        $scope.rowsSelected.push(itm);
                    }
                }
                else {
                    $scope.rowsSelected.splice(itm);
                }
            });
        }

        $scope.GetLeave = function (leave) {
            if (leave.selected) {
                $scope.rowsSelected.push(leave);
            }
            else {
                const ind = $scope.rowsSelected.indexOf(leave);
                if (ind > -1) {
                    $scope.rowsSelected.splice(ind, 1);
                }
            }
        }

        $scope.isOptionsRequired = function () {
            return !$scope.multidocView.some(function (options) {
                return options.selected;
            });
        };


        $scope.Approverecord = function (indl) {
            var data = {
                "ISMCLMER_Id": $scope.ISMCLMER_Id,
            };
            apiService.create("IEMonthEndReportApproval/DocView", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.multidocView != null) {
                        $('#MultiDocumentView').modal('show');
                        $scope.multidocView = promise.multidocView;
                    }
                }
            });
        };




        $scope.Approverecord = function () {
            var temp_array = [];
            if ($scope.myForm.$valid) {

                angular.forEach($scope.multidocView, function (row) {
                    if (row.selected == true) {
                        temp_array.push({
                            ISMCLMERFL_Id: row.ismclmerfL_Id,
                            ISMCLMERFL_ApproverRemarks: row.ismclmerfL_ApproverRemarks,
                        });
                    }
                });

                var data = {
                    getReport_status: temp_array,
                };

                apiService.create("IEMonthEndReportApproval/Approve_status", data).then(function (promise) {
                    if (promise.message == "Approved") {
                        swal("Month End Report Approved Successfully");
                        //$state.reload();
                        $scope.Clearid();
                    }
                    else {
                        swal("Month End Report Not Approved Successfully");
                    }
                });

            }
            else {
                swal("Please Contect Administrator");
            }
                    
            
        };






        $scope.deactiveY = function (user, SweetAlert) {
            var data = {
                "HREXTTRN_Id": user.hrexttrN_Id
            };
            var dystring = "";
            if (user.hrexttrN_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (user.hrexttrN_ActiveFlag === false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("IEMonthEndReportApproval/deactiveY", data).then(function (promise) {
                            if (promise.message === "Mapped") {
                                swal("Record Is Already Mapped, So You Can Not Deactive The Record");
                            } else {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            }
                        });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }
                });
        };




        //============Multiple file upload===========
        //===========================ADD==================================
        $scope.documentListOtherDetails = [{ id: 'document' }];
        $scope.addNewDocumentOtherDetail = function () {
            var newItemNo = $scope.documentListOtherDetails.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListOtherDetails.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentOtherDetail = function (index, data) {
            var newItemNo = $scope.documentListOtherDetails.length - 1;
            $scope.documentListOtherDetails.splice(index, 1);
            if (data.hreothdeT_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
        };
        //-------------------ADD------------------
        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input, document) {
            $scope.SelectedFileForUploadzd = input.files;
            $scope.vtadaA_FileName = input.files[0].name;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 5242880)  // 2097152 bytes = 2MB 
                {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/png" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/jpg" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/JPG" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/pdf" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/msword" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                //5242880
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].size > 5242880) {
                    swal("File size should be less than 5 MB");
                    return;
                }
            }
        };
        function UploaddianmateralPhoto(data) {
            console.log("Report Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= 1; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadtrnsportdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.ismclT_FilePath = d;
                    data.vtadaaF_FileName = $scope.vtadaA_FileName;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        };
        ///addd
        $scope.materaldocuupload = [{ itrS_Id: 'trans1' }];
        if ($scope.materaldocuupload.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addgrnrows = function () {
            if ($scope.materaldocuupload.length > 1) {
                for (var i = 0; i === $scope.materaldocuupload.length; i++) {
                    var id = $scope.materaldocuupload[i].itrS_Id;
                    var lastChar = id.substr(id.length - 1);
                    $scope.cnt = parseInt(lastChar);
                }
            }
            $scope.cnt = $scope.cnt + 1;
            $scope.tet = 'trans' + $scope.cnt;
            var newItemNo = $scope.cnt;
            $scope.materaldocuupload.push({ 'itrS_Id': 'trans' + newItemNo });


        };
        $scope.removegrnrows = function (index, data) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

        };


        $scope.previewimg_new = function (img) {
            $scope.imagepreview = img;
            $scope.view_videos = [];
            var img = $scope.imagepreview;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {

                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');

            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
            }
            else if ($scope.filetype2 == 'pdf') {

                ///=====================show pdf, img

                $('#showpdf').modal('hide');
                var imagedownload1 = "";
                imagedownload1 = $scope.imagepreview;


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
            else {
                $window.open($scope.imagepreview)
            }
        };
        $scope.previewimg_url = function (url) {
            $scope.urlnew = url;
            $window.open($scope.urlnew)
        }


        $scope.content1 = "";
        ///=====================show pdf, img
        $scope.previewpdf = function (filepath1, filename) {
            $('#showpdf').modal('hide');
            var imagedownload1 = "";
            imagedownload1 = filepath1;


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
        };


        $scope.previewimg = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };
        ///=====================show pdf, img end


        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

        //totalhourscal
        $scope.Caltotalhours = function () {
            $scope.srresult = [];
            $scope.erresult = [];
            $scope.sresult = $filter('date')($scope.hrexttrN_StartTime, 'HH:mm:ss a');
            $scope.eresult = $filter('date')($scope.hrexttrN_EndTime, 'HH:mm:ss a');
            var startTime = moment($scope.sresult, "HH:mm:ss a");
            var endTime = moment($scope.eresult, "HH:mm:ss a");
            var duration = moment.duration(endTime.diff(startTime));
            var hours = parseInt(duration.asHours());
            $scope.hrexttrN_TotalHrs = hours;

        }


    }
})();

