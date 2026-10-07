(function () {
    'use strict';
    angular
        .module('app')
        .controller('InwardController', InwardController)

    InwardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache', 'myFactoryvisitor', '$q','$window']
    function InwardController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache, myFactoryvisitor, $q, $window) {

        //$scope.ddate = new Date();
        //$scope.usrname = localStorage.getItem('username');
        //var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //var paginationformasters;
        //var copty;
        //var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings.length > 0) {
        //    paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
        //    copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        //}
        //$scope.coptyright = copty;
        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        //if (admfigsettings.length > 0 && admfigsettings == null) {
        //    var logopath = admfigsettings[0].asC_Logo_Path;
        //    $scope.imgname = logopath;
        //}
        $scope.search = "";
        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        var miid = myFactoryvisitor.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }
        //===============page load

        $scope.loadgrid = function () {
            //$scope.currentPage = 1;
            //$scope.itemsPerPage = paginationformasters;
            //$scope.search = "";
            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }
            apiService.getURI("Inward/getDetails/", $scope.mI_Id).then(function (promise) {
                $scope.mI_Id = promise.mI_Id;
                $scope.emplist1 = promise.emplist;
                $scope.emplist2 = promise.emplist2;
                $scope.getdataall = promise.getdataall;
                $scope.presentCountgrid = promise.getdataall;
                $scope.institutionlist = promise.institutionlist;
               $scope.emplist3 = promise.emplist3;

            });
        }

        //=====================Get emp Record ==================//
        $scope.get_empdetails = function () {
            var empid = $scope.emp1.hrmE_Id;
            var data = {
                "HRME_Id": empid,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("Inward/get_empdetails", data).then(function (promise) {
                $scope.emplist2 = promise.emplist;
            })
        }

         //=====================exportToExcel Record ==================//
        $scope.exportToExcel = function () {          
            var printSectionId = '#table1';
            if ($scope.getdataall !== null && $scope.getdataall.length > 0) {
                var exportHref = Excel.tableToExcel(printSectionId, 'INWARD RECORD');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = "INWARD RECORD.xls";
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        };
var toggleStatus=[];
      $scope.all_task = function (atk) {
            $scope.allRecords = atk;
             toggleStatus = $scope.allRecords;
            angular.forEach($scope.filterValue1, function (atck) {               
                atck.checkedvalue = toggleStatus;               
            });
        };

      $scope.togchkbxtask = function () {
            $scope.allRecords = $scope.filterValue1.every(function (options) {                
                return options.checkedvalue;               
            });
        };


        // ========filter data
        $scope.get_empdetails2 = function () {
            var empid = $scope.hrmid1.hrmE_Id;
            var empid2 = $scope.hrmid1.hrmid1;
            var data = {
                "HRME_Id": empid2,
                //"empid2": empid2,
                "MI_Id": $scope.mI_Id
            }
            apiService.create("Inward/get_empdetails2", data).then(function (promise) {
                $scope.emplist3 = promise.emplist;
            })
        }
        //====================End emp Record ==================//

        //=====================Start emp name Search ==================//

        $scope.searchfilter = function (objj) {
            if (objj.search.length >= '2') {
                var data = {
                    "searchfilter": objj.search,
                    "MI_Id": $scope.mI_Id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("Inward/searchfilter", data).
                    then(function (promise) {
                        $scope.emplist1 = promise.emplist;
                        angular.forEach($scope.emplist1, function (objectt) {
                            if (objectt.hrmE_EmployeeFirstName.length > 0) {
                                var string = objectt.hrmE_EmployeeFirstName;
                                objectt.hrmE_EmployeeFirstName = string.replace(/  +/g, ' ');
                            }
                        })
                    })
            }

        }

        // =========== search
        $scope.searchfilter2 = function (objj) {

            if (objj.search.length >= '2') {
                var empid2 = $scope.emp1.hrmE_Id;
                var data = {
                    "searchfilter": objj.search,
                    "HRME_Id": empid2,
                    "MI_Id": $scope.mI_Id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("Inward/searchfilter2", data).
                    then(function (promise) {
                        $scope.emplist1 = promise.emplist;
                        angular.forEach($scope.emplist1, function (objectt) {
                            if (objectt.hrmE_EmployeeFirstName.length > 0) {
                                var string = objectt.hrmE_EmployeeFirstName;
                                objectt.hrmE_EmployeeFirstName = string.replace(/  +/g, ' ');
                            }
                        })
                    })
            }
        }


        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.hrmid1 = '';
        $scope.hrmid12 = '';
        //$scope.hrmE_Id = '';

        ///======================save 
        $scope.submitted = false;
        $scope.saveData = function () {
            if ($scope.myForm.$valid) {
                $scope.uploadarray = [];
                if ($scope.materaldocuupload.length > 0) {
                    angular.forEach($scope.materaldocuupload, function (itm) {
                        if (itm.ismclT_FilePath !== undefined && itm.ismclT_FilePath !== null && itm.ismclT_FilePath !== "") {
                            $scope.uploadarray.push({
                                FOINF_FileName: itm.vtadaaF_FileName,
                                FOINF_FilePath: itm.ismclT_FilePath,
                                FOINF_Filedesc: itm.VTADAAF_Remarks
                                //FOINF_FileName: itm.FileName,
                                //FOINF_Filedesc: itm.foinf_FilePath,
                            });
                        }
                    });
                }
                if ($scope.emp1 != undefined) {
                    $scope.time = $filter('date')(new Date(), 'HH:mm:ss');
                    var startdate = $scope.FOIN_DateTime == null ? "" : $filter('date')($scope.FOIN_DateTime, "yyyy-MM-dd");
                    var obj = {
                        "FOIN_Id": $scope.FOIN_Id,
                        "FOIN_Adddress": $scope.FOIN_Adddress,
                        "FOIN_DateTime": startdate,
                        "FOIN_EmailId": $scope.FOIN_EmailId,
                        "FOIN_PhoneNo": $scope.FOIN_PhoneNo,
                        "FOIN_ContactPerson": $scope.FOIN_ContactPerson,
                        "FOIN_From": $scope.FOIN_From,
                        "FOIN_Discription": $scope.FOIN_Discription,
                        "HRME_Id": $scope.emp1.hrmE_Id,
                        "empid2": $scope.hrmid12.hrmid12, //dispached by
                        "empid3": $scope.hrmid1.hrmid1,
                        "FilePath_Array": $scope.uploadarray,
                        "MI_Id": $scope.mI_Id,
                    }
                    apiService.create("Inward/saveData", obj).then(function (promise) {
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == "Save") {
                                    swal('Record Saved Successfully!!!');
                                    $state.reload();
                                    $scope.Inwardss = promise.curntInwardrec;
                                    $scope.printbutton = false;
                                    $scope.SchoolLogo = promise.institution[0].mI_Logo;
                                    $scope.SchollName = promise.institution[0].mI_Name;
                                    $scope.SchollAdd = promise.institution[0].mI_Address1;
                                    $scope.sttud = true;
                                    $scope.getdataall.length = 0;
                                }
                                else if (promise.returnval == "Not Save") {
                                    swal('Record Not Saved Successfully!!!');
                                }
                                else if (promise.returnval == "Update") {
                                    swal('Record Updated Successfully!!!');
                                    $state.reload();
                                }
                                else if (promise.returnval == "Not Update") {
                                    swal('Record Not Updated Successfully!!!');
                                }
                            }
                            else {
                                swal("Record already exist");
                            }
                        }
                        else {
                            swal("Kindly Contact Administrator!!!");
                        }
                    });
                }
                else {
                    swal('Please select Person To!');
                }
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.cancel = function () {
            $state.reload();
        };

        //=============== print
        $scope.printbutton = true;
        $scope.Print = function () {
            var innerContents = '';
            innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/OutwardPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        }

        //=====================Get emp Record ==================//
        $scope.filedetails = function (item) {
            var data = {
                "FOIN_Id": item.foiN_Id,
                "secnam1": "Inward",
            }
            apiService.create("Inward/filedetails", data).then(function (promise) {
                if (promise.attachementlist != null && promise.attachementlist.length > 0) {
                    $scope.attachementlist = promise.attachementlist;
                    $('#inwardfiles').modal('show');
                }
                else {
                    swal("File Not Uploaded!")
                }
                
            })
        }


      

        //===============edit
        $scope.EditDetails = function (data) {
            $scope.materaldocuupload = [];
            $scope.employeename12 = "";
            $scope.employeename1 = "";
            $scope.hrmid12 = '';
            $scope.hrmid1 = '';
            var data = {
                "FOIN_Id": data.foiN_Id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("Inward/EditDetails/", data).then(function (promise) {
               // $scope.materaldocuupload = promise.attachementlist;
                if (promise.attachementlist != null && promise.attachementlist.length > 0) {
                    angular.forEach(promise.attachementlist, function (dd) {
                        $scope.materaldocuupload.push({                        
                            VTADAAF_Remarks: dd.foinfU_Filedesc,
                            ismclT_FilePath: dd.foinfU_FilePath,
                            ismclT_FileName: dd.foinfU_FileName
                        });                        
                    }); 
                }
                else {
                    $scope.materaldocuupload = [{ itrS_Id: 'trans1' }];
                    if ($scope.materaldocuupload.length === 1) {
                        $scope.cnt = 1;
                    }
                }
               

                $scope.editDetails = promise.editDetails;
                $scope.MI_Id = promise.editDetails[0].mI_Id;
                $scope.FOIN_Id = promise.editDetails[0].foiN_Id;
                $scope.FOIN_Discription = promise.editDetails[0].foiN_Discription;
                $scope.FOIN_From = promise.editDetails[0].foiN_From;
                $scope.FOIN_EmailId = promise.editDetails[0].foiN_EmailId;
                $scope.FOIN_PhoneNo = promise.editDetails[0].foiN_PhoneNo;
                $scope.FOIN_DateTime = new Date(promise.editDetails[0].foiN_DateTime);
                $scope.FOIN_ContactPerson = promise.editDetails[0].foiN_ContactPerson;
                $scope.FOIN_Adddress = promise.editDetails[0].foiN_Adddress;
                $scope.emp1 = promise.editDetails[0];
                $scope.emp1.hrmE_Id = promise.editDetails[0].foiN_To;
                $scope.get_empdetails($scope.emp1.hrmE_Id);
                $scope.get_empdetails2($scope.hrmid1.hrmE_Id);

                $scope.hrmid1 = promise.emplist2[0];
                if ($scope.hrmid1 != null || $scope.hrmid1 != undefined) {
                    if ($scope.hrmid1.length > 0) {
                        //$scope.hrmid1.hrmid1 = $scope.editDetails[0].hrmid1;
                        $scope.hrmid1.hrmid1 = promise.emplist2[0].hrmid1;
                    }
                }
                else {
                    $scope.hrmid1 = '';
                }
              
                $scope.employeename1 = promise.emplist2[0].employeename1;
                //$scope.get_empdetails2($scope.hrmid1.hrmid1);
                $scope.hrmid12 = promise.emplist3[0];
                
                if ($scope.hrmid12 != null || $scope.hrmid12 != undefined) {
                    if ($scope.hrmid12.length > 0) {
                        $scope.hrmid12.hrmid12 = promise.emplist3[0].hrmid12;
                    }
                }
                else {
                    $scope.hrmid12 = '';
                }

                //$scope.employeename12 = $scope.emplist3[0].employeename12;
                $scope.employeename12 = promise.emplist3[0].employeename12;
                //added praveen
            });
        }


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
            console.log("Inward  Upload  :" + data);
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
            if ($scope.materaldocuupload != null && $scope.materaldocuupload.length < 5) {
                for (var i = 0; i === $scope.materaldocuupload.length; i++) {               
                    var id = $scope.materaldocuupload[i].itrS_Id;
                    var lastChar = id.substr(id.length - 1);
                    $scope.cnt = parseInt(lastChar);
                }
                $scope.cnt = $scope.cnt + 1;
                $scope.tet = 'trans' + $scope.cnt;
                var newItemNo = $scope.cnt;
                $scope.materaldocuupload.push({ 'itrS_Id': 'trans' + newItemNo });
            }
           
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
                // $scope.filetype2 = lastelement;
            }
            if (lastelement == 'wmv' || lastelement == 'mp4') {

                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myvideoPreview').modal('show');
            }
            else if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');

            }
            else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                $window.open($scope.imagepreview);
            }
           
            else if (lastelement == 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');
            }
            else if (lastelement == 'pdf') {

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


        //============= Active/De-active record
        $scope.deactive = function (newuser1, SweetAlertt) {
            var mgs = "";
            if (newuser1.foiN_ActiveFlag == false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Inward/deactivate", newuser1).
                            then(function (promise) {
                                if (promise.returnval2 == true) {
                                    swal("Record " + mgs + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + mgs + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }




        $scope.interacted = function (field) {
            return $scope.submitted;
        };



    }

})();