(function () {
    'use strict';
    angular
        .module('app')
        .controller('Checklist_ApprovalReportController', Checklist_ApprovalReportController);

    Checklist_ApprovalReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'Excel', '$timeout', 'superCache'];
    function Checklist_ApprovalReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, Excel, $timeout, superCache) {

        $scope.addjob = {};
        $scope.EmployeeDis = false;
        $scope.submitted = false;
        $scope.newdate = new Date();
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
        //=============== view doc==========

        $scope.previewimg_new = function (img) {
            $scope.ismclTFilePath = img;

            var img = $scope.ismclTFilePath;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];


                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.ismclTFilePath);
                    $('#myimagePreview').modal('show');
                }

                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                    $window.open($scope.ismclTFilePath)
                    
                }
                else if (lastelement == 'pdf') {
                    ///=====================show pdf, img
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.ismclTFilePath;
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
        $scope.Report = function () {
            if ($scope.myForm1.$valid) {
                $scope.chklst.myDate_from = $scope.chklst.myDate_from == null ? "" : $filter('date')($scope.chklst.myDate_from, "yyyy-MM-dd");
                $scope.chklst.myDate_to = $scope.chklst.myDate_to == null ? "" : $filter('date')($scope.chklst.myDate_to, "yyyy-MM-dd");
                var data = $scope.chklst;
                apiService.create("ChecklistDetailsMapping/checklistreport", data).
                    then(function (promise) {
                        var temp_array = [];
                        var temp_array1 = [];
                        var i = 0;
                        var newCol = {};
                        var newCol1 = {};
                        $scope.userrole = promise.userrole;
                        $scope.userrolearray = promise.userrolearray;

                        if (promise.userrole != "ADMIN") {
                            $scope.columnnames = promise.columnnames;
                            $scope.columnnum = $scope.columnnames.length + 3;

                            for (i = 0; i < promise.filldata.length; i++) {
                                newCol = { checkdate: promise.filldata[i].ismclT_Date, remark: promise.filldata[i].ismclT_Remarks, MorningQty: promise.filldata[i].ismclD_MorningQty, EveningQty: promise.filldata[i].ismclD_EveningQty, ismclTFilePath: promise.filldata[i].ismclT_FilePath };
                                temp_array.push(newCol);
                                if (i < promise.filldata.length - 1) {
                                    if (promise.filldata[i].ismcL_Name == promise.filldata[i + 1].ismcL_Name)
                                        continue;
                                }
                                newCol1 = { pdate: temp_array, ismcL_Name: promise.filldata[i].ismcL_Name };
                                temp_array1.push(newCol1);
                                temp_array = [];
                            }

                            $scope.yearlyemprep = temp_array1;
                            $scope.EmployeeDis = true;
                        }
                        else {
                            $scope.columnnames = promise.columnnames;
                            $scope.columnnum = $scope.columnnames.length + 3;

                            for (i = 0; i < promise.filldata.length; i++) {
                                newCol = { checkdate: promise.filldata[i].ismclT_Date, remark: promise.filldata[i].ismclT_Remarks, MorningQty: promise.filldata[i].ismclD_MorningQty, EveningQty: promise.filldata[i].ismclD_EveningQty, ismclTFilePath: promise.filldata[i].ismclT_FilePath, role: angular.uppercase(promise.filldata[i].userrole) };
                                temp_array.push(newCol);
                                if (i < promise.filldata.length - 1) {
                                    if (promise.filldata[i].ismcL_Name == promise.filldata[i + 1].ismcL_Name)
                                        continue;
                                }
                                newCol1 = { pdate: temp_array, ismcL_Name: promise.filldata[i].ismcL_Name };
                                temp_array1.push(newCol1);
                                temp_array = [];
                            }

                            $scope.yearlyemprep = temp_array1;
                            $scope.EmployeeDis = true;
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
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
                    "ISMCLT_Id": $scope.ismclT_Id,
                    "HRMF_Id": $scope.hrmF_Id,
                    "ApprovalTempDTO": $scope.temp_array2
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
            var innerContents = document.getElementById("Table").innerHTML;
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
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };

        $scope.printToCartadmin = function (Baldwin) {
            var innerContents = document.getElementById("Tableadmin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExceladmin = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'CheckListDetails');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };
    }

})();