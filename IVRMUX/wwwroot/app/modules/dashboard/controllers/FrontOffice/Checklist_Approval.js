(function () {
    'use strict';
    angular
        .module('app')
        .controller('Checklist_ApprovalController', Checklist_ApprovalController);

    Checklist_ApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter','$q'];
    function Checklist_ApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, $q) {

        $scope.addjob = {};
        $scope.EmployeeDis = false;
        $scope.submitted = false;
        $scope.FileName = '';
        $scope.ismclT_FilePath = '';
        $scope.chkk = false;
        $scope.chkkkk = false;
        var vm = this;
        vm.gridOptions = {};
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("ChecklistDetailsMapping/approvalloaddata", pageid).then(function (promise) {
                if (promise.checklisttypeList !== null && promise.checklisttypeList.length > 0) {
                    $scope.checklisttypeList = promise.checklisttypeList;
                }

                if (promise.floorList !== null && promise.floorList.length > 0) {
                    $scope.floorList = promise.floorList;
                }
                if (promise.checklistmappedList !== null && promise.checklistmappedList.length > 0) {
                    $scope.checklistmappedList = promise.checklistmappedList;
                    
                    $scope.EmployeeDis = true;
                }                
            });            
        };
        //morningqty
        $scope.Morning_chk = function () {
            if ($scope.morning === true) {
                for (var i = 0; i < $scope.checklistmappedList.length; i++) {                   
                    $scope.checklistmappedList[i].ismclD_MorningQty = $scope.checklistmappedList[i].ISMCLD_Qty;
                }
            }
            else {
                for (var i = 0; i < $scope.checklistmappedList.length; i++) {                  
                    $scope.checklistmappedList[i].ismclD_MorningQty = 0;
                }
            }          
        };
        $scope.Evening_chk = function () {
            if ($scope.evening === true) {
                for (var i = 0; i < $scope.checklistmappedList.length; i++) {
                    $scope.checklistmappedList[i].ismclD_EveningQty = $scope.checklistmappedList[i].ISMCLD_Qty;
                }
            }
            else {
                for (var i = 0; i < $scope.checklistmappedList.length; i++) {
                    $scope.checklistmappedList[i].ismclD_EveningQty = 0;
                }
            }
        };
        //=============== view doc==========

        $scope.previewimg_new = function (img) {
            $scope.ismclT_FilePath = img;
           
            var img = $scope.ismclT_FilePath;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                
                
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.ismclT_FilePath);
                    $('#myimagePreview').modal('show');
                }

                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                   $window.open($scope.ismclT_FilePath)
                   // $scope.popup.location.replace($scope.ismclT_FilePath);
                }
                else if (lastelement == 'pdf') {
                    ///=====================show pdf, img
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.ismclT_FilePath;
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
           
           
           
           
          
            else {
                $window.open($scope.ismclT_FilePath)
            }
        };
        //=========================================
        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input, document) {
          
            $scope.SelectedFileForUploadzd = input.files;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/png" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/jpg" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/JPG" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/pdf" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }                 
                else if (input.files[0].type === "application/vnd.ms-excel" && input.files[0].size <= 2097152) {
                    UploaddianmateralPhoto(document);
                }               
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
            else {

            }
        };

        function UploaddianmateralPhoto(data) {
            console.log("Checklist  Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= 1; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
            }
            //for (var i = 0; i <= $scope.SelectedFileForUploadzd.length; i++) {
            //    formData.append("File", $scope.SelectedFileForUploadzd[i]);
            //}
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Addcandidate", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    $scope.ismclT_FilePath = d;
                  
                  
                   // data.FileName = $scope.filename;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.cancel = function () {
            $scope.chklst = "";
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted;
        };

        $scope.cleardata = function () {
            $scope.EmployeeDis = false;
        };

        $scope.EditData = function (record) {
            var id = record.hrcisC_Id;
            apiService.getURI("ChecklistDetailsMapping/editRecord", id).
                then(function (promise) {

                });
        };

        $scope.savejob = function () {
            if ($scope.myForm1.$valid) {
                var data = $scope.chklst;
                apiService.create("ChecklistDetailsMapping/findchecklist", data).
                    then(function (promise) {
                        if (promise.checklistmappedList !== null && promise.checklistmappedList.length > 0) {
                            $scope.checklistmappedList = promise.checklistmappedList;
                            $scope.EmployeeDis = true;
                            if ($scope.checklistmappedList[0].Apprvl_Morningqty > 0) {
                                $scope.chkk = true;
                            }
                            else {
                                $scope.chkk = false;
                            }
                            
                            if ($scope.checklistmappedList[0].Apprvl_Eveningqty > 0) {
                                $scope.chkkkk = true;
                            }
                            else {
                                $scope.chkkkk = false;
                            }
                            angular.forEach($scope.checklistmappedList, function (ver) {
                                ver.selected = true;
                                ver.ismclT_Remarks = ver.ISMCLT_Remarks;
                                if (ver.Apprvl_qty != 0) {
                                    ver.ismclD_MorningQty = ver.Apprvl_Morningqty;
                                    ver.ismclD_EveningQty = ver.Apprvl_Eveningqty;
                                }
                                else {
                                    ver.ismclD_MorningQty = ver.ISMCLD_Qty;
                                    ver.ismclD_EveningQty = ver.ISMCLD_Qty;
                                }
                            });
                        }
                        else {
                            swal("No Item Found !!");
                            return;
                        }
                    });
            }
            else { $scope.submitted = true; }
        };

        $scope.saveapproval = function () {
            if ($scope.myForm.$valid) {
                $scope.temp_array2 = [];
                for (var x = 0; x < $scope.checklistmappedList.length; x++) {
                    if ($scope.checklistmappedList[x].selected == true) {
                        $scope.temp_array2.push($scope.checklistmappedList[x]);
                    }
                }

                var data = {
                    "ISMCLT_Id": $scope.chklst.ismclT_Id,
                    "HRMF_Id": $scope.chklst.hrmF_Id,
                    "ApprovalTempDTO": $scope.temp_array2,
                    //extra added
                    
                    "ISMCLT_FilePath": $scope.ismclT_FilePath

                };

                apiService.create("ChecklistDetailsMapping/saveapproval", data).
                    then(function (promise) {

                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                                $state.reload();
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                                $scope.cancel();
                            }
                            else if (promise.retrunMsg === "Notification") {
                                swal("Notification!");
                                $scope.cancel();
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                            }
                        }
                    });
            }            
        };

        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'CheckListDetails');
            $timeout(function () { location.href = exportHref; }, 100);
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };
    }

})();